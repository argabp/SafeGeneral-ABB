using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.VoucherKass.Queries
{
    public class VoucherKasDto : IMapFrom<VoucherKas>
    {
       public string KodeCabang { get; set; }
        public string JenisVoucher { get; set; }
        public string DebetKredit { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string DibayarKepada { get; set; }
        public DateTime? TanggalVoucher { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? TotalVoucher { get; set; }
        public decimal? TotalDalamRupiah { get; set; }
        public string KeteranganVoucher { get; set; }
        public bool FlagPosting { get; set; }
        public bool FlagFinal { get; set; }
        public DateTime? TanggalInput { get; set; }
        public DateTime? TanggalUpdate { get; set; }
        public string KodeUserInput { get; set; }
        public string KodeUserUpdate { get; set; }
        public string JenisPembayaran { get; set; }
        public string KodeKas { get; set; }

        // definisi
        public string NamaKas { get; set; }
        public string NamaMataUang { get; set; }
        public string DetailMataUang { get; set; }
        public string NamaCabang { get; set; }

        public string KeteranganKasBank { get; set; }
        public decimal? Saldo { get; set; }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<VoucherKas, VoucherKasDto>();
        }
    }
}