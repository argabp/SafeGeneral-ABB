using System;
using ABB.Application.Common.Mappings; // Pastikan namespace IMapFrom benar
using ABB.Domain.Entities;
using ABB.Application.Common.Interfaces;
using AutoMapper;

namespace ABB.Application.TemplateLapKeus.Queries
{
    // Kita kembalikan inheritance IMapFrom<TemplateLapKeu>
    public class TemplateLapKeuDto : IMapFrom<TemplateLapKeu>
    {
        public long Id { get; set; }
        public string TipeLaporan { get; set; }
        public string TipeBaris { get; set; }
        public int Urutan { get; set; }
        public string Deskripsi { get; set; }
        public string Rumus { get; set; }
        public string Level { get; set; }

        // Implementasi Mapping manual di sini
        public void Mapping(Profile profile)
        {
            profile.CreateMap<TemplateLapKeu, TemplateLapKeuDto>();
            // Jika butuh sebaliknya (untuk save/update):
            // profile.CreateMap<TemplateLapKeuDto, TemplateLapKeu>(); 
        }
    }
}