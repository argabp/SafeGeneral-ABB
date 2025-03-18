using System;

namespace ABB.Domain.Entities
{
    public class NotaDinasStatusDokumen
    {
        public int id_nds { get; set; }

        public Int16 no_urut { get; set; }

        public Int16 no_dokumen { get; set; }

        public string dokumen { get; set; }
    }
}