using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using LojiteksWeb.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Claims;
using static Dapper.SqlMapper;

namespace LojiteksWeb.Controllers
{
    public class AccountController : Controller
    {
        private readonly Context _context;
        private readonly MailSettings _mailSettings;

        public AccountController(Context context, IOptions<MailSettings> mailSettings)
        {
            _context = context;
            _mailSettings = mailSettings.Value;
        }

        public IActionResult Login(string? returnUrl)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel loginViewModel, string? returnUrl)
        {

            if (ModelState.IsValid)
            {
                var isUser = _context.TBL_Kullanicilar.FirstOrDefault(x => x.KullaniciAdi == loginViewModel.KullaniciAdi && x.Sifre == loginViewModel.Sifre);

                if (isUser != null)
                {
                    List<Claim> userClaims = new List<Claim>();

                    userClaims.Add(new Claim(ClaimTypes.NameIdentifier, isUser.KullaniciAdi));
                    userClaims.Add(new Claim(ClaimTypes.Name, isUser.KullaniciAdi));
                    userClaims.Add(new Claim(ClaimTypes.GivenName, isUser.AdSoyad));
                    userClaims.Add(new Claim(ClaimTypes.Role, isUser.Yetki.ToString()));
                    userClaims.Add(new Claim("FirmaID", isUser.FirmaID.ToString()));

                    var claimsIdentity = new ClaimsIdentity(userClaims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ViewBag.ErrorMessage = "Username or password incorrect";
                    return View(loginViewModel);
                }
            }
            ViewBag.ErrorMessage = "Username and password fields are required";
            return View(loginViewModel);
        }
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

        public IActionResult ForgotPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPassword)
        {
            if (string.IsNullOrEmpty(forgotPassword.Email))
                return View();

            var user = await _context.TBL_Kullanicilar
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Email == forgotPassword.Email);

            if (user == null)
            {
                ViewBag.ErrorMessage = "Email address not found.";
                return View(forgotPassword);
            }

            if (!user.EmailConfirmed)
            {
                ViewBag.ErrorMessage = "Email address is not verified. Contact your administrator.";
                return View(forgotPassword);
            }

            int length = 12;
            string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new Random();
            string password = new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());

            user.Sifre = password;
            var result = await SendNewPasswordAsync(user);
            if (result)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                TempData["Message"] = "Your new password has been sent via email.";
                return RedirectToAction("Login", "Account");
            }
            ViewBag.ErrorMessage = "An error occurred, try again later.";
            return View(forgotPassword);
        }

        public async Task<bool> SendNewPasswordAsync(TBL_Kullanici user)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress(_mailSettings.DisplayName, _mailSettings.Mail));

            async Task<bool> IsValidEmailAsync(string email)
            {
                try
                {
                    MailboxAddress mailboxAddress;
                    return MailboxAddress.TryParse(email, out mailboxAddress);
                }
                catch
                {
                    return false;
                }
            }

            string fullName = $"{user.AdSoyad}";
            if (!string.IsNullOrEmpty(user.Email) && await IsValidEmailAsync(user.Email))
            {
                message.To.Add(new MailboxAddress(fullName, user.Email));
            }

            message.Subject = "Lojiteks New Password";

            message.Body = new TextPart("plain")
            {
                Text = $"Hi\n\nCloud Of Rfid New Password: {user.Sifre}\n\nBest regards"
            };

            try
            {
                using (var client = new SmtpClient())
                {
                    client.Connect(_mailSettings.Host, _mailSettings.Port, SecureSocketOptions.StartTls);

                    client.Authenticate(_mailSettings.Mail, _mailSettings.Password);

                    await client.SendAsync(message);
                    client.Disconnect(true);
                }
                return true;
            }
            catch (Exception)
            {
                return false;
                throw;
            }
        }

        [Authorize]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
