using System;
using System.ComponentModel.DataAnnotations;
using ABB.Application.EntriPeriodes.Commands;
using ABB.Application.EntriPeriodes.Queries;
using AutoMapper;

using ABB.Application.Common.Interfaces;

namespace ABB.Web.Modules.EntriPeriode.Models
{
    public class EntriPeriodeViewModel : IMapFrom<EntriPeriodeDto>, IMapFrom<CreateEntriPeriodeCommand>, IMapFrom<UpdateEntriPeriodeCommand>
    {
        [Required(ErrorMessage = "Tahun Periode harus diisi")]
        [Display(Name = "Tahun")]
        public decimal? ThnPrd { get; set; }

        [Required(ErrorMessage = "Bulan Periode harus diisi")]
        [Display(Name = "Bulan")]
        public short? BlnPrd { get; set; }

        [Required(ErrorMessage = "Tanggal Mulai harus diisi")]
        [Display(Name = "Tanggal Mulai")]
        public DateTime? TglMul { get; set; }

        [Required(ErrorMessage = "Tanggal Akhir harus diisi")]
        [Display(Name = "Tanggal Akhir")]
        public DateTime? TglAkh { get; set; }

        [Display(Name = "Closing")]
        public string FlagClosing { get; set; } // "Y" atau "N"

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntriPeriodeDto, EntriPeriodeViewModel>();
            profile.CreateMap<EntriPeriodeViewModel, CreateEntriPeriodeCommand>();
            profile.CreateMap<EntriPeriodeViewModel, UpdateEntriPeriodeCommand>();
        }
    }
}