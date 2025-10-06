using System.ComponentModel.DataAnnotations;
using ABB.Application.Common.Interfaces;
using ABB.Application.InquiryNotaProduksis.Queries; // <-- Pastikan using ini ada
using AutoMapper;
using System;
namespace ABB.Web.Modules.InquiryNotaProduksi.Models
{
    public class InquiryNotaProduksiViewModel 
    {
      
        
        public InquiryNotaProduksiDto InquiryNotaProduksiHeader { get; set; }
        public int id { get; set; } 

        
    }
}