using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.NotaFakultatifKeluars.Queries
{
    public class DetailNotaFakultatifKeluarDataDto : IMapFrom<DetailNotaFakultatifKeluar>
    {
        public string kd_cb { get; set; }

        public string jns_tr { get; set; }

        public string jns_nt_msk { get; set; }

        public string kd_thn { get; set; }

        public string kd_bln { get; set; }

        public string no_nt_msk { get; set; }

        public string jns_nt_kel { get; set; }

        public string no_nt_kel { get; set; }

        public byte no_ang { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailNotaFakultatifKeluarDataDto, DetailNotaFakultatifKeluar>();
        }
    }
}