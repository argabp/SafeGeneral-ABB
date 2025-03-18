using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.PertanggunganKendaraans.Commands;
using ABB.Application.PertanggunganKendaraans.Queries;
using AutoMapper;

namespace ABB.Web.Modules.PertanggunganKendaraan.Models
{
    public class DetailPertanggunganKendaraanViewModel : IMapFrom<AddDetailPertanggunganKendaraanCommand>
    {
        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_jns_ptg { get; set; }

        public short no_urut { get; set; }

        public decimal? nilai_tsi_tjh_mul { get; set; }

        public decimal? nilai_tsi_tjh_akh { get; set; }

        public decimal? pst_rate_tjh { get; set; }

        public byte? stn_rate_tjh { get; set; }

        public decimal? nilai_prm_tjh { get; set; }

        public decimal? nilai_tsi_tjp { get; set; }

        public decimal? nilai_prm_tjp { get; set; }

        public decimal? pst_rate_pad { get; set; }

        public decimal? pst_rate_pap { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DetailPertanggunganKendaraanViewModel, AddDetailPertanggunganKendaraanCommand>();
            profile.CreateMap<DetailPertanggunganKendaraanViewModel, DeleteDetailPertanggunganKendaraanCommand>();
            profile.CreateMap<DetailPertanggunganKendaraanViewModel, EditDetailPertanggunganKendaraanCommand>();
            profile.CreateMap<DetailPertanggunganKendaraanDto, DetailPertanggunganKendaraanViewModel>();
        }
    }
}