namespace ABB.Application.ProsesTutupBulan.Dtos
{
    // Perbaikan Nama Class: ProsesTutupBulanDto
    public class ProsesTutupBulanDto 
    {
        public string ThnPrd { get; set; }
        public string BlnPrd { get; set; }
        public string TglMul { get; set; } 
        public string TglAkh { get; set; }
        public string Status { get; set; } 
        public bool IsReadyToClose { get; set; }
    }
}