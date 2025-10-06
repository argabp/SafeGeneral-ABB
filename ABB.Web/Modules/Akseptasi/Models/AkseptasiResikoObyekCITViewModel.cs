using System;
using ABB.Application.Akseptasis.Commands;
using ABB.Application.Akseptasis.Queries;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Web.Modules.Akseptasi.Models
{
    public class AkseptasiResikoObyekCITViewModel : IMapFrom<AkseptasiObyekCITDto>
    {
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public string kd_scob { get; set; }

        public string kd_thn { get; set; }

        public string no_aks { get; set; }

        public Int16 no_updt { get; set; }

        public Int16 no_rsk { get; set; }

        public string kd_endt { get; set; }

        public Int16 no_oby { get; set; }

        public string? kd_lok_asal { get; set; }

        public string? ket_tujuan { get; set; }

        public DateTime tgl_kirim { get; set; }

        public decimal nilai_srt { get; set; }

        public decimal nilai_kas { get; set; }

        public decimal? pst_rate { get; set; }

        public Int16? jarak { get; set; }

        public string? ket_kirim { get; set; }

        public Int16? jml_wesel { get; set; }

        public decimal? nilai_pa { get; set; }

        public decimal? pst_rate_pa { get; set; }

        public decimal? nilai_prm_pa { get; set; }

        public decimal? pst_share { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<AkseptasiObyekCITDto, AkseptasiResikoObyekCITViewModel>();
            profile.CreateMap<AkseptasiResikoObyekCITViewModel, SaveAkseptasiObyekCITCommand>();
        }
    }
}