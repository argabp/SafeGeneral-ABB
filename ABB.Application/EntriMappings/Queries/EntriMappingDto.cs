using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.EntriMappings.Queries
{
    public class EntriMappingDto : IMapFrom<EntriMapping>
    {
        public string gl_akun104 { get; set; }
        public string Nama104 { get; set; } // Tambahkan ini
        public string gl_akun117 { get; set; }
        public string Nama117 { get; set; } // Tambahkan ini

        public void Mapping(Profile profile)
        {
            profile.CreateMap<EntriMapping, EntriMappingDto>();
        }
    }
}