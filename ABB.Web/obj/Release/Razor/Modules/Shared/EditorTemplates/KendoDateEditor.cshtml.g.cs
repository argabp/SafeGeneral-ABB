#pragma checksum "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Shared\EditorTemplates\KendoDateEditor.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "54c807d45425c805c5efbcf5662aa7a59054846a"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Modules_Shared_EditorTemplates_KendoDateEditor), @"mvc.1.0.view", @"/Modules/Shared/EditorTemplates/KendoDateEditor.cshtml")]
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
#line 1 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Shared\EditorTemplates\KendoDateEditor.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"54c807d45425c805c5efbcf5662aa7a59054846a", @"/Modules/Shared/EditorTemplates/KendoDateEditor.cshtml")]
    public class Modules_Shared_EditorTemplates_KendoDateEditor : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<DateTime>
    {
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\n");
#nullable restore
#line 4 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Shared\EditorTemplates\KendoDateEditor.cshtml"
Write(Html.Kendo().DatePickerFor(m => m)
    .Value(Model)
    .DateInput()
    );

#line default
#line hidden
#nullable disable
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<DateTime> Html { get; private set; }
    }
}
#pragma warning restore 1591
