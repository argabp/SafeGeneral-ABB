using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.DataProduksiCabangs.Queries;
using AutoMapper;

namespace ABB.Web.Modules.DataProduksiCabang.Models
{
    public class DataProduksiCabangViewModel : IMapFrom<GetDataProduksiCabangsQuery>
    {
        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }
        
        public string kd_cb { get; set; }

        public string kd_cob { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DataProduksiCabangViewModel, GetDataProduksiCabangsQuery>();
        }
    }
}