using System;
using System.Collections.Generic;
using ABB.Application.Common.Interfaces;
using ABB.Application.KontrakTreatyKeluarXOLs.Commands;
using ABB.Application.KontrakTreatyKeluarXOLs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.KontrakTreatyKeluarXOL.Models
{
    public class KontrakTreatyKeluarXOLViewModel : IMapFrom<SaveKontrakTreatyKeluarXOLCommand>
    {
        
        public string kd_cb { get; set; }

        public string kd_jns_sor { get; set; }

        public string kd_tty_npps { get; set; }

        public string kd_cob { get; set; }

        public decimal thn_tty_npps { get; set; }

        public string npps_layer { get; set; }
        
        public string nm_tty_npps { get; set; }

        public DateTime tgl_mul_ptg { get; set; }

        public DateTime tgl_akh_ptg { get; set; }

        public decimal nilai_bts_or { get; set; }

        public decimal nilai_bts_tty { get; set; }

        public decimal pst_adj_onrpi { get; set; }

        public string? ket_tty_npps { get; set; }

        public decimal? nilai_kurs { get; set; }

        public decimal? pst_reinst { get; set; }

        public decimal? mindep { get; set; }

        public short? hit { get; set; }

        public List<DetailKontrakTreatyKeluarXOLDataDto> Details { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KontrakTreatyKeluarXOLViewModel, SaveKontrakTreatyKeluarXOLCommand>();
            profile.CreateMap<Domain.Entities.KontrakTreatyKeluarXOL, KontrakTreatyKeluarXOLViewModel>();
        }
    }
}