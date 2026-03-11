using System;

namespace ABB.Domain.Entities
{
    public class SpLaporanNeracaSaldoResult
    {
        public DateTime? posisi { get; set; }     
        public string pos { get; set; }          
        public string tipe { get; set; }          
        public string nm_tipe { get; set; }        
        public string lokasi { get; set; }         
        public string kd_akun { get; set; }        
        public string nm_akun { get; set; }       
        public decimal? saldoawal_debet { get; set; }   
        public decimal? saldoawal_kredit { get; set; }            
        public decimal? mutasi_debet { get; set; }   
        public decimal? mutasi_kredit { get; set; }   
        public decimal? saldoakhir_debet { get; set; }  
        public decimal? saldoakhir_kredit { get; set; } 

       
    }
}