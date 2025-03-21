#pragma checksum "C:\Users\Formulatrix\RiderProjects\Mine\ABB - Copy\ABB.Web\Modules\Shared\Components\Navigation\_Navigation.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "f92f40872623211f29d27445550d4954cebd2aad"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Modules_Shared_Components_Navigation__Navigation), @"mvc.1.0.view", @"/Modules/Shared/Components/Navigation/_Navigation.cshtml")]
namespace AspNetCore
{
    #line hidden
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Rendering;
    using Microsoft.AspNetCore.Mvc.ViewFeatures;
#nullable restore
#line 1 "C:\Users\Formulatrix\RiderProjects\Mine\ABB - Copy\ABB.Web\Modules\Shared\Components\Navigation\_Navigation.cshtml"
using System.Text;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Formulatrix\RiderProjects\Mine\ABB - Copy\ABB.Web\Modules\Shared\Components\Navigation\_Navigation.cshtml"
using ABB.Application.Navigations.Queries;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"f92f40872623211f29d27445550d4954cebd2aad", @"/Modules/Shared/Components/Navigation/_Navigation.cshtml")]
    public class Modules_Shared_Components_Navigation__Navigation : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<List<ABB.Application.Navigations.Queries.LoggedInNavigationDto>>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
#nullable restore
#line 5 "C:\Users\Formulatrix\RiderProjects\Mine\ABB - Copy\ABB.Web\Modules\Shared\Components\Navigation\_Navigation.cshtml"
  
    var CurrentController = ViewContext.RouteData.Values["controller"].ToString();
    var CurrentAction = ViewContext.RouteData.Values["action"].ToString();

#line default
#line hidden
#nullable disable
            WriteLiteral("<nav class=\"mt-2\">\r\n    <ul class=\"nav nav-pills nav-sidebar flex-column\" data-widget=\"treeview\" role=\"menu\" data-accordion=\"false\">\r\n        ");
#nullable restore
#line 11 "C:\Users\Formulatrix\RiderProjects\Mine\ABB - Copy\ABB.Web\Modules\Shared\Components\Navigation\_Navigation.cshtml"
   Write(Html.Raw(BuildNavigation(Model, CurrentController, CurrentAction)));

#line default
#line hidden
#nullable disable
            WriteLiteral(";\r\n    </ul>\r\n</nav>\r\n");
        }
        #pragma warning restore 1998
#nullable restore
#line 15 "C:\Users\Formulatrix\RiderProjects\Mine\ABB - Copy\ABB.Web\Modules\Shared\Components\Navigation\_Navigation.cshtml"
 
    private static string BuildNavigation(List<LoggedInNavigationDto> navigations, string CurrentController, string CurrentAction)
    {
        var topLevelNavigations = navigations.Where(x => x.ParentId == 0).ToList();
        var strBuilder = new StringBuilder();
        foreach (var nav in topLevelNavigations)
        {
            if (HasSubNavigation(navigations,nav.NavigationId))
            {
                strBuilder.Append(GetSubNavigationItem(navigations, nav, CurrentController, CurrentAction));
            }
            else
            {
                strBuilder.Append(GetNavigationItem(nav, CurrentController, CurrentAction));
            }

        }
        return strBuilder.ToString();
    }
    private static bool HasSubNavigation(List<LoggedInNavigationDto> navigations, int id)
    {
        return navigations.Where(x => x.ParentId == id)?.Any() ?? false;
    }

    private static string GetNavigationItem(LoggedInNavigationDto nav, string CurrentController, string CurrentAction)
    {
        string openInNewWindow = "";
        if(nav.Controller=="WebReportDesigner" && nav.Action == "Index")
            openInNewWindow=" target='_blank' ";
        var selectedCss = nav.Controller == CurrentController && nav.Action == CurrentAction ? "selected" : "";
        var strBuilder = new StringBuilder();
        strBuilder.Append($"<li class='nav-item {selectedCss}'>");
        strBuilder.Append($"<a href='/{nav.Controller}/{nav.Action}' {openInNewWindow} class='nav-link'>");
        strBuilder.Append($"<i class='nav-icon fas {nav.Icon}'></i>");
        strBuilder.Append($"<p> {nav.Text} </p>");
        strBuilder.Append("</a></li>");
        return strBuilder.ToString();
    }
    private static string GetOpenParentNavigationCss(List<LoggedInNavigationDto> subNavigation, string CurrentController, string CurrentAction)
    {
        string css = "";
        foreach (var subnav in subNavigation)
        {
            if (subnav.Controller == CurrentController && subnav.Action == CurrentAction)
                css = "menu-is-opening menu-open";
        }
        return css;
    }
    private static string GetSubNavigationItem(List<LoggedInNavigationDto> navigations, LoggedInNavigationDto nav, string CurrentController, string CurrentAction)
    {
        var strBuilder = new StringBuilder();
        var subNavigation = navigations.Where(x => x.ParentId == nav.NavigationId).OrderBy(x => x.Sort).ToList();
        strBuilder.Append($"<li class='nav-item {GetOpenParentNavigationCss(subNavigation, CurrentController, CurrentAction)}'>");
        strBuilder.Append($"<a href='#' class='nav-link'>");
        strBuilder.Append($"<i class='nav-icon fas {nav.Icon}'></i>");
        strBuilder.Append($"<p> {nav.Text} <i class='right fas fa-angle-left'></i></p>");
        strBuilder.Append($"</a>");
        strBuilder.Append("<ul class='nav nav-treeview'>");
        foreach (var subnav in subNavigation)
        {
            strBuilder.Append(GetNavigationItem(subnav, CurrentController, CurrentAction));
        }
        strBuilder.Append(@"</ul></li> ");
        return strBuilder.ToString();
    }


#line default
#line hidden
#nullable disable
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<List<ABB.Application.Navigations.Queries.LoggedInNavigationDto>> Html { get; private set; }
    }
}
#pragma warning restore 1591
