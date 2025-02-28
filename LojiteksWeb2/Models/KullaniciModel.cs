﻿using System.ComponentModel.DataAnnotations;

namespace LojiteksWeb.Models
{
    public class KullaniciModel
    {
        public int KullaniciID { get; set; }
        public string KullaniciAdi { get; set; }
        public string Sifre { get; set; }
        public int FirmaID { get; set; }
        public string AdSoyad { get; set; }
        public int Yetki { get; set; }
    }
}
