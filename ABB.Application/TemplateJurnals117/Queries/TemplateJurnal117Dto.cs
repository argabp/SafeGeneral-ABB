using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.TemplateJurnals117.Queries
{
    public class TemplateJurnal117Dto : IMapFrom<TemplateJurnal117>
    {
        public string type_tr { get; set; }
        public string type_jr { get; set; }
        public string metode { get; set; }
        public string Event { get; set; }
        public string jn_ass { get; set; }
        public string nm_jr { get; set; }
        public string GridId 
            { 
                get 
                {
                    var tr = (type_tr ?? "").Trim();
                    var jr = (type_jr ?? "").Trim();
                    var met = (metode ?? "").Trim();
                    var evt = (Event ?? "").Trim();
                    var ass = (jn_ass ?? "").Trim();

                    // Gabungkan, dan Replace spasi dengan kosong jika ada (HTML ID dilarang ada spasi)
                    return $"{tr}_{jr}_{met}_{evt}_{ass}".Replace(" ", "");
                }
            }


        public void Mapping(Profile profile)
        {
            profile.CreateMap<TemplateJurnal117, TemplateJurnal117Dto>();
        }
    }
}