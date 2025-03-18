using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.MataUangs.Commands;
using AutoMapper;

namespace ABB.Web.Modules.MataUang.Models
{
    public class DeleteDetailMataUangViewModel : IMapFrom<DeleteDetailMataUangCommand>
    {
        public string kd_mtu { get; set; }

        public DateTime tgl_mul { get; set; }
        
        public DateTime tgl_akh { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<DeleteDetailMataUangViewModel, DeleteDetailMataUangCommand>();
        }
    }
}