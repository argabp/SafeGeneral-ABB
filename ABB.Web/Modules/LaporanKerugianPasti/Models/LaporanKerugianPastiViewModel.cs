using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.LaporanKerugianPasti.Queries;
using AutoMapper;

namespace ABB.Web.Modules.LaporanKerugianPasti.Models
{
    public class LaporanKerugianPastiViewModel : IMapFrom<GetLaporanKerugianPastiQuery>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public string? nm_ttg { get; set; }
        public string laporan { get; set; }
        public string tanda_tangan { get; set; }
        public string jabatan { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<LaporanKerugianPastiViewModel, GetLaporanKerugianPastiQuery>();
        }
    }
}