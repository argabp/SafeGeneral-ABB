using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.Jurnals62.Queries
{
    public class Jurnals62Dto : IMapFrom<Jurnal62>
    {
         public long Id { get; set; }
        public string GlLok { get; set; }
        public string GlTran { get; set; }
        public string GlBukti { get; set; }
        public DateTime? GlTanggal { get; set; }
        public string GlNota { get; set; }
        public string GlMtu { get; set; }
        public string GlKet { get; set; }
        public short GlUrut { get; set; }
        public string GlDk { get; set; }
        public decimal? GlNilaiOrg { get; set; }
        public decimal? GlNilaiIdr { get; set; }
        public string GlAkun { get; set; }
        public bool? FlagClosed { get; set; }
        public DateTime? TglInput { get; set; }
        public string KodeUserInput { get; set; }
        public DateTime? TglUpdate { get; set; }
        public string KodeUserUpdate { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Jurnal62, Jurnals62Dto>();
        }
    }
}