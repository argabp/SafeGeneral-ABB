using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.JurnalMemorial117.Commands; // (Akan dibuat di bawah)
using ABB.Application.JurnalMemorial117.Queries;
using AutoMapper;

namespace ABB.Web.Modules.JurnalMemorial117.Models
{
    // Item Detail untuk Grid (mirip PenyelesaianPiutangItem)
    public class JurnalMemorial117Item
    {
        public int No { get; set; }
        public string NoVoucher { get; set; }
        public string KodeAkun { get; set; }
        public string NoNota { get; set; }
        public string KodeMataUang { get; set; }
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiDebetRp { get; set; }
        public decimal? NilaiKredit { get; set; }
        public decimal? NilaiKreditRp { get; set; }
    }

    public class JurnalMemorial117ViewModel : IMapFrom<CreateJurnalMemorial117HeaderCommand>
    {
        // Header
        public JurnalMemorial117Dto JurnalHeader { get; set; }

        // List Detail
        public List<JurnalMemorial117Item> JurnalItems { get; set; }

        public JurnalMemorial117ViewModel()
        {
            JurnalHeader = new JurnalMemorial117Dto();
            JurnalItems = new List<JurnalMemorial117Item>();
        }

        // --- Properti Input Detail untuk Form Add/Edit ---
        public int No { get; set; } // ID Detail untuk edit
        public string NoVoucher { get; set; }

        [Display(Name = "Kode Akun")]
        public string KodeAkun { get; set; }

        [Display(Name = "No Nota")]
        public string NoNota { get; set; }

        [Display(Name = "Mata Uang")]
        public string KodeMataUang { get; set; }

        [Display(Name = "Nilai Debet")]
        public decimal? NilaiDebet { get; set; }
        public decimal? NilaiDebetRp { get; set; }

        [Display(Name = "Nilai Kredit")]
        public decimal? NilaiKredit { get; set; }
        public decimal? NilaiKreditRp { get; set; }

        public void Mapping(Profile profile)
        {
            // Mapping Header
            profile.CreateMap<JurnalMemorial117Dto, CreateJurnalMemorial117HeaderCommand>();
            
            // Mapping Detail (Dari ViewModel ke Command Detail)
            profile.CreateMap<JurnalMemorial117ViewModel, CreateJurnalMemorial117DetailCommand>();
            
            // Mapping Detail (Dari DTO ke Item View)
            profile.CreateMap<JurnalMemorial117DetailDto, JurnalMemorial117Item>();
        }
    }
}