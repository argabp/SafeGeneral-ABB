namespace ABB.Domain.Entities
{
    public class LabaRugiKurs
    {
        public long Id { get; set; }        // Primary Key asli di DB
        public int gl_kode { get; set; } // Kode Akun (misal: 70010100)
        public int gl_dept { get; set; }  // Kode Cabang (misal: 10, 20, 50)
        public string gl_nama { get; set; } // Nama Akun (misal: Selisih Kurs)
    }
}