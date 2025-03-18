using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Shared.Components.InputTags
{
    public class InputTagsViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke(string Name, string Placeholder,int ColumnCount, List<InputTagSourceItem> Items )
        {
            return View("_InputTags", new InputTagsModel()
            {
                Name = Name,
                PlaceHolder = Placeholder,
                Items = Items,
                ColumnCount = ColumnCount
            });
        }
    }
}