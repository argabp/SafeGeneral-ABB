using ABB.Application.Common.Dtos;

namespace ABB.Web.Modules.UserCabang.Models
{
    public class CabangItem
    {
        public CabangItem()
        {
            Cabang = new DropdownOptionDto();
            Database = new DropdownOptionDto();
        }

        public string userid { get; set; }

        public DropdownOptionDto Cabang { get; set; }

        public DropdownOptionDto Database { get; set; }
    }
}