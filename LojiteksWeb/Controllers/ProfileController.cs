using LojiteksDataAccess.DBContext;
using LojiteksEntity.Entities;
using LojiteksWeb.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using MimeKit;
using System.Security.Claims;
using static Dapper.SqlMapper;

namespace LojiteksWeb.Controllers
{
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly Context _context;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly ClaimsPrincipal _user;
        private readonly MailSettings _mailSettings;

        public ProfileController(IOptions<MailSettings> mailSettings, Context context, IHttpContextAccessor httpContextAccessor)
        {
            _mailSettings = mailSettings.Value;
            _context = context;
            _user = httpContextAccessor.HttpContext.User;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> Index(string username)
        {
            if (username == null)
            {
                return RedirectToAction("Index", "Home", new { error = "No Records Found" });
            }
            var user = await _context.TBL_Kullanicilar
                .AsNoTracking()
                .Where(x => x.KullaniciAdi == username)
                .Select(x => new EmailConfirmedModel
                {
                    EmailConfirmed = x.EmailConfirmed,
                    TwoFactorCodeExpiration = x.TwoFactorCodeExpiration
                })
                .FirstOrDefaultAsync();

            if (user == null)
            {
                return RedirectToAction("Index", "Home", new { error = "No Records Found" });
            }

            return View(user);
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail()
        {
            var username = User.Identity.Name;
            var user = await _context.TBL_Kullanicilar.FirstOrDefaultAsync(x => x.KullaniciAdi == username);
            if (user == null)
                return BadRequest();

            var twoFactorCode = new Random().Next(100000, 999999).ToString();
            user.TwoFactorCode = twoFactorCode;
            user.TwoFactorCodeExpiration = DateTime.Now.AddMinutes(5);
            user.TwoFactorCounter = 0;

            var result = await SendEmailVerificationCodeAsync(user);
            if (result)
            {
                _context.Update(user);
                await _context.SaveChangesAsync();
                return Ok(new { expiration = user.TwoFactorCodeExpiration });
            }
            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> EmailConfirmed([FromBody] string code)
        {
            if (string.IsNullOrEmpty(code) || code.Length != 6)
                return BadRequest();

            var username = User.Identity.Name;

            var user = await _context.TBL_Kullanicilar.FirstOrDefaultAsync(x => x.KullaniciAdi == username);
            if (user == null)
                return BadRequest();

            if (user.TwoFactorCounter < 3 && user.TwoFactorCodeExpiration > DateTime.Now)
            {
                user.TwoFactorCounter++;
                if (user.TwoFactorCode == code)
                {
                    user.TwoFactorCounter = 0;
                    user.TwoFactorCode = "";
                    user.TwoFactorCodeExpiration = null;
                    user.EmailConfirmed = true;

                    _context.Update(user);
                    await _context.SaveChangesAsync();

                    return Ok(true);
                }
                return BadRequest();
            }
            else
            {
                user.TwoFactorCounter = 0;
                user.TwoFactorCode = "";
                user.TwoFactorCodeExpiration = null;

                _context.Update(user);
                await _context.SaveChangesAsync();
            }
            return BadRequest();
        }

        public async Task<bool> SendEmailVerificationCodeAsync(TBL_Kullanici user)
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

            message.Subject = "Lojiteks Mail Verification";

            message.Body = new TextPart("plain")
            {
                Text = $"Hi,\n\nYou can use the code below to confirm your email address via the CloudOfRfid system." +
                $"\n\n{user.TwoFactorCode}.\n\nYour code's expiration date: {user.TwoFactorCodeExpiration}\n\nBest regards"
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
    }
}
