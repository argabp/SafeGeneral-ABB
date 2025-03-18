using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Http;

namespace ABB.Api.Dto
{
    public class UpdatePembayaranDto
    {
        public int id_nds { get; set; }

        public string keterangan { get; set; }

        public DateTime tgl_status { get; set; }
        
        public List<IFormFile> Attachments { get; set; }
    }
}