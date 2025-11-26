using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.KasBanks.Queries
{
    public class KasBankDto : IMapFrom<KasBank>
    {
        public string Kode { get; set; }
        public string Keterangan { get; set; }
        public string NoRekening { get; set; }
        public string NoPerkiraan { get; set; }
        public string TipeKasBank { get; set; }
        public decimal? Saldo { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KasBank, KasBankDto>();
        }
    }
}