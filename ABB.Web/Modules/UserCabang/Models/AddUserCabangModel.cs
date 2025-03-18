using System.Collections.Generic;

namespace ABB.Web.Modules.UserCabang.Models
{
    public class AddUserCabangModel
    {
        public AddUserCabangModel()
        {
            Cabangs = new List<CabangItem>();
        }
        
        public string userid { get; set; }

        public List<CabangItem> Cabangs { get; set; }
    }
}