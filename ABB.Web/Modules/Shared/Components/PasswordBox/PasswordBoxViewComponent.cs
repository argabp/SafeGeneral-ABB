using Microsoft.AspNetCore.Mvc;

namespace ABB.Web.Modules.Shared.Components.PasswordBox
{
    public class PasswordBoxViewComponent : ViewComponent
    {
        public PasswordBoxViewComponent()
        {
        }

        public IViewComponentResult Invoke(string Name, string Value, string PlaceHolder)
        {
            return View("_PasswordBox", new PasswordBoxModel() { Name = Name, Value = Value, PlaceHolder = PlaceHolder });
        }
    }
}