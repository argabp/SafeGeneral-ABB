#pragma checksum "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\RiskAndLossProfile\Views\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "101e30a12290717a71f06efaff35bcbb3548d186"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Modules_RiskAndLossProfile_Views_Index), @"mvc.1.0.view", @"/Modules/RiskAndLossProfile/Views/Index.cshtml")]
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
#line 1 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\RiskAndLossProfile\Views\Index.cshtml"
using ABB.Application.RiskAndLossProfiles.Queries;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\RiskAndLossProfile\Views\Index.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\RiskAndLossProfile\Views\Index.cshtml"
using Kendo.Mvc.TagHelpers;

#line default
#line hidden
#nullable disable
#nullable restore
#line 4 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\RiskAndLossProfile\Views\Index.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"101e30a12290717a71f06efaff35bcbb3548d186", @"/Modules/RiskAndLossProfile/Views/Index.cshtml")]
    public class Modules_RiskAndLossProfile_Views_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Modules/RiskAndLossProfile/JS/riskAndLossProfile.index.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
        #line hidden
        #pragma warning disable 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperExecutionContext __tagHelperExecutionContext;
        #pragma warning restore 0649
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner __tagHelperRunner = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperRunner();
        #pragma warning disable 0169
        private string __tagHelperStringValueBuffer;
        #pragma warning restore 0169
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __backed__tagHelperScopeManager = null;
        private global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager __tagHelperScopeManager
        {
            get
            {
                if (__backed__tagHelperScopeManager == null)
                {
                    __backed__tagHelperScopeManager = new global::Microsoft.AspNetCore.Razor.Runtime.TagHelpers.TagHelperScopeManager(StartTagHelperWritingScope, EndTagHelperWritingScope);
                }
                return __backed__tagHelperScopeManager;
            }
        }
        private global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper;
        #pragma warning disable 1998
        public async override global::System.Threading.Tasks.Task ExecuteAsync()
        {
            WriteLiteral("\r\n");
            WriteLiteral("\r\n");
#nullable restore
#line 10 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\RiskAndLossProfile\Views\Index.cshtml"
  
    Layout = "~/Modules/Shared/_Layout.cshtml";
    ViewData["Title"] = "Risk And Loss Profile";

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n");
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "101e30a12290717a71f06efaff35bcbb3548d1864338", async() => {
            }
            );
            __Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper = CreateTagHelper<global::Microsoft.AspNetCore.Mvc.Razor.TagHelpers.UrlResolutionTagHelper>();
            __tagHelperExecutionContext.Add(__Microsoft_AspNetCore_Mvc_Razor_TagHelpers_UrlResolutionTagHelper);
            __tagHelperExecutionContext.AddHtmlAttribute(__tagHelperAttribute_0);
            await __tagHelperRunner.RunAsync(__tagHelperExecutionContext);
            if (!__tagHelperExecutionContext.Output.IsContentModified)
            {
                await __tagHelperExecutionContext.SetOutputContentAsync();
            }
            Write(__tagHelperExecutionContext.Output);
            __tagHelperExecutionContext = __tagHelperScopeManager.End();
            WriteLiteral(@"

<style>
    .div-box{
        padding-bottom: 1em;
    }
</style>

<div class=""flat-box"">
    <div class=""row"">
        <div class=""flat-box-title col-md-8"">
            Risk And Loss Profile
        </div>

        <div class=""col-md-4"">
            <button class=""flat-btn-primary"" onclick=""btnAddRiskAndLossProfile_OnClick()"">Add New Risk And Loss Profile</button>
        </div>
    </div>

    <div class=""flat-box-content"">
        <div class=""div-box"">
            ");
#nullable restore
#line 36 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\RiskAndLossProfile\Views\Index.cshtml"
        Write(Html.Kendo().Grid<RiskAndLossProfileDto>()
                .Name("RiskAndLossProfileGrid")
                .DataSource(dataSource => dataSource.Ajax()
                    .Read(read => read.Action("GetRiskAndLossProfiles", "RiskAndLossProfile").Data("searchFilter"))
                    .Model(model => model.Id(p => p.Id))
                )
                .Columns(columns =>
                {
                    columns.Bound(col => col.nm_cob).Width(150).Title("Nama COB");
                    columns.Bound(col => col.nomor).Width(150).Title("Nomor");
                    columns.Bound(col => col.bts1).Width(150).Title("Batas 1");
                    columns.Bound(col => col.bts2).Width(150).Title("Batas 2");
                    columns.Command(command =>
                    {
                        command.Custom("EditRiskAndLossProfile")
                            .Text("Edit")
                            .IconClass("fa fa-pencil-alt")
                            .HtmlAttributes(new { title = "Edit" })
                            .Click("btnEditRiskAndLossProfile_OnClick");
                        command.Custom("DeleteRiskAndLossProfile")
                            .Text("Delete")
                            .IconClass("k-icon k-i-delete")
                            .HtmlAttributes(new { title="Delete"})
                            .Click("onDeleteRiskAndLossProfile");
                    }).Width(200);
                })
                .Pageable(pager => pager
                    .Refresh(true)
                    .Info(true)
                    .PageSizes(true)
                )
                .Sortable()
                .Filterable()
                .Scrollable()
                .Height(450)
                .Resizable(a=>a.Columns(true))
                .Events(ev => ev.DataBound("gridAutoFit"))
        
                );

#line default
#line hidden
#nullable disable
            WriteLiteral("    \r\n        </div>\r\n        \r\n        ");
#nullable restore
#line 77 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\RiskAndLossProfile\Views\Index.cshtml"
    Write(Html.Kendo().Window()
            .Name("RiskAndLossProfileWindow")
            .Title("Risk And Loss Profile")
            .Width(400)
            .Modal(true)
            .Visible(false)
            .Draggable(true)
            );

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n    </div>\r\n</div>\r\n\r\n");
        }
        #pragma warning restore 1998
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public IConfiguration Configuration { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.ViewFeatures.IModelExpressionProvider ModelExpressionProvider { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IUrlHelper Url { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.IViewComponentHelper Component { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IJsonHelper Json { get; private set; }
        [global::Microsoft.AspNetCore.Mvc.Razor.Internal.RazorInjectAttribute]
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
