using System;
using ABB.Application.Common.Interfaces;
using ABB.Application.NotaResiko.Queries;
using ABB.Application.PostingNotaRisiko.Commands;
using ABB.Web.Modules.NotaRisiko.Models;
using AutoMapper;

namespace ABB.Web.Modules.PostingNotaRisiko.Models
{
    public class PostingNotaRisikoViewModel : IMapFrom<PostingNotaRisikoCommand>
    {
        public string TipeTransaksi { get; set; }

        public DateTime PeriodeProses { get; set; }
   
        public void Mapping(Profile profile)
        {
            profile.CreateMap<PostingNotaRisikoViewModel, PostingNotaRisikoCommand>();
        }
    }
}