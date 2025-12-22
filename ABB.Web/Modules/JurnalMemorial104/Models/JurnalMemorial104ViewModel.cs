using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.JurnalMemorials104.Commands; // (Akan dibuat di bawah)
using ABB.Application.JurnalMemorials104.Queries;
using AutoMapper;

namespace ABB.Web.Modules.JurnalMemorial104.Models
{
    // Item Detail untuk Grid (mirip PenyelesaianPiutangItem)
    public class JurnalMemorial104Item
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
        public string KeteranganDetail { get; set; }
    }

    public class JurnalMemorial104ViewModel : IMapFrom<CreateJurnalMemorial104HeaderCommand>
    {
        // Header
        public JurnalMemorial104Dto JurnalHeader { get; set; }

        // List Detail
        public List<JurnalMemorial104Item> JurnalItems { get; set; }

        public JurnalMemorial104ViewModel()
        {
            JurnalHeader = new JurnalMemorial104Dto();
            JurnalItems = new List<JurnalMemorial104Item>();
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

         [Display(Name = "Keterangan")]
        public string KeteranganDetail { get; set; }


        public void Mapping(Profile profile)
        {
            // Mapping Header
            profile.CreateMap<JurnalMemorial104Dto, CreateJurnalMemorial104HeaderCommand>();
            
            // Mapping Detail (Dari ViewModel ke Command Detail)
            profile.CreateMap<JurnalMemorial104ViewModel, CreateJurnalMemorial104DetailCommand>();

            profile.CreateMap<JurnalMemorial104ViewModel, UpdateJurnalMemorial104DetailCommand>();
            
            // Mapping Detail (Dari DTO ke Item View)
            profile.CreateMap<JurnalMemorial104DetailDto, JurnalMemorial104Item>();
        }
    }
}