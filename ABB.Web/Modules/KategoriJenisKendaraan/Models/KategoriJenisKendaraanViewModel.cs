using ABB.Application.Common.Interfaces;
using ABB.Application.KategoriJenisKendaraans.Commands;
using ABB.Application.KategoriJenisKendaraans.Queries;
using AutoMapper;

namespace ABB.Web.Modules.KategoriJenisKendaraan.Models
{
    public class KategoriJenisKendaraanViewModel : IMapFrom<SaveKategoriJenisKendaraanCommand>
    {
        public string kd_grp_rsk { get; set; }

        public string kd_rsk { get; set; }

        public string desk_rsk { get; set; }

        public string? kd_ref { get; set; }
        public string kd_ref1 { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<KategoriJenisKendaraanViewModel, SaveKategoriJenisKendaraanCommand>();
            profile.CreateMap<KategoriJenisKendaraanDto, KategoriJenisKendaraanViewModel>();
        }
    }
}