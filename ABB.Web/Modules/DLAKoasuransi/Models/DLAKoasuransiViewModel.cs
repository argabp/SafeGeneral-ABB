using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.DLAKoasuransi.Queries;
using AutoMapper;

namespace ABB.Web.Modules.DLAKoasuransi.Models
{
    public class DLAKoasuransiViewModel : IMapFrom<GetDLAKoasuransiQuery>
    {
        public string DatabaseName { get; set; }
        public string kd_cb { get; set; }
        public string kd_cob { get; set; }
        public string kd_scob { get; set; }
        public int kd_thn { get; set; }
        public string no_kl { get; set; }
        public Int16 no_mts { get; set; }
        public string bahasa { get; set; }
        public void Mapping(Profile profile)
        {
            profile.CreateMap<DLAKoasuransiViewModel, GetDLAKoasuransiQuery>();
        }
    }
}