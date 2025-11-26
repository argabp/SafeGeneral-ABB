using System;
using ABB.Application.Common.Interfaces;
using ABB.Domain.Entities;
using AutoMapper;

namespace ABB.Application.InquiryNotaProduksis.Queries
{
    public class InquiryNotaProduksiDto : IMapFrom<Produksi>
    {
        public int id { get; set; }
        public string no_nd { get; set; }
        public string nm_cust2 { get; set; }
        public decimal? premi { get; set; }
        public string no_ref { get; set; }
        public DateTime? date { get; set; }
        public string type { get; set; }
        public string d_k { get; set; }
        public string lok { get; set; }
        public string no_cust2 { get; set; }
        public string no_brok { get; set; }
        public string nm_brok { get; set; }
        public string no_endos { get; set; }
        public string no_pl { get; set; }
        public string curensi { get; set; }
        public decimal? kurs { get; set; }
        public string no_kwi { get; set; }
        public decimal? hp { get; set; }
        public DateTime? d_per1 { get; set; }
        public DateTime? d_per2 { get; set; }
        public string no_pos { get; set; }
        public string nm_pos { get; set; }
        public decimal? rabat { get; set; }
        public decimal? n_rabat { get; set; }
        public decimal? n_bruto { get; set; }
        public decimal? polis { get; set; }
        public decimal? materai { get; set; }
        public decimal? komisi { get; set; }
        public decimal? n_komisi { get; set; }
        public decimal? h_fee { get; set; }
        public decimal? n_hfee { get; set; }
        public decimal? lain { get; set; }
        public decimal? klaim { get; set; }
        public decimal? netto { get; set; }
        public DateTime? tgl_byr { get; set; }
        public decimal? jumlah { get; set; }
        public DateTime? date_input { get; set; }
        public DateTime? date_edit { get; set; }
        public string created_by { get; set; }
        public string edited_by { get; set; }

         public string jn_ass { get; set; }
         public string qq { get; set; }
         public DateTime? tgl_jth_tempo { get; set; }
         public string reg { get; set; }
         public string catat { get; set; }
         public string kd_ass2 { get; set; }
         public string kd_tutup { get; set; }
         public string kd_ass3 { get; set; }
         public decimal? debet { get; set; }
         public string nm_pol { get; set; }
         public decimal? saldo { get; set; }
         public decimal? kredit { get; set; }
         public string no_bukti { get; set; }
         public decimal? post_tr { get; set; }
         public string jn_ass2 { get; set; }
         public string no_nd2 { get; set; }
         public string nm_cust { get; set; }

// tambahan kode mtu
         public string kd_mtu { get; set; }

        public void Mapping(Profile profile)
        {
            profile.CreateMap<Produksi, InquiryNotaProduksiDto>();
        }
    }
}