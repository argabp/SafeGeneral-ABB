#pragma checksum "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\TrackingRekonsiliasi\Views\New.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "0527350c06c42bcb232b9c4931f1de290a989f72"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Modules_TrackingRekonsiliasi_Views_New), @"mvc.1.0.view", @"/Modules/TrackingRekonsiliasi/Views/New.cshtml")]
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
#line 1 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\TrackingRekonsiliasi\Views\New.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\TrackingRekonsiliasi\Views\New.cshtml"
using ABB.Application.TrackingRekonsiliasi.Queries;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\TrackingRekonsiliasi\Views\New.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"0527350c06c42bcb232b9c4931f1de290a989f72", @"/Modules/TrackingRekonsiliasi/Views/New.cshtml")]
    public class Modules_TrackingRekonsiliasi_Views_New : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Modules/trackingRekonsiliasi/JS/trackingRekonsiliasi.new.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 9 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\TrackingRekonsiliasi\Views\New.cshtml"
  
    Layout = "~/Modules/Shared/_Layout.cshtml";
    ViewData["Title"] = "Tracking Rekonsiliasi New";

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "0527350c06c42bcb232b9c4931f1de290a989f724036", async() => {
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
            WriteLiteral("\r\n\r\n<div class=\"flat-box\">\r\n    <div class=\"row\">\r\n        <div class=\"flat-box-title col-md-8\">\r\n            New\r\n        </div>\r\n    </div>\r\n\r\n    <div class=\"flat-box-content\">\r\n        ");
#nullable restore
#line 23 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\TrackingRekonsiliasi\Views\New.cshtml"
    Write(Html.Kendo().Grid<TrackingRekonsiliasiDto>()
        .Name("NewTrackingRekonsiliasiGrid")
        .DataSource(dataSource => dataSource.Ajax()
            .Read(read => read.Action("GetTrackingRekonsiliasiNews", "TrackingRekonsiliasi"))
        )
        .Columns(columns =>
        {
            columns.Bound(col => col.nm_rk).Title("Kreditur").Width(250);
            columns.Bound(col => col.nomor_sppa).Title("No. SPPA");
            columns.Bound(col => col.no_ktp).Title("No. Identitas");
            columns.Bound(col => col.nm_ttg).Title("Nama Peserta");
            columns.Bound(col => col.nm_kategori).Title("Kategori");
            columns.Bound(col => col.nm_status_payment).Title("Status");
            columns.Bound(col => col.tgl_status).Title("Tanggal Status").Format("{0:dd MMM yyyy • HH:mm}");
            columns.Bound(col => col.nomor_sert).Title("No. Sertifikat");

            columns.Command(command =>
            {
                command.Custom("PrintSertifikat")
                    .Text(" ")
                    .IconClass("fa fa-file-alt")
                    .HtmlAttributes(new { title = "Print Sertifikat" })
                    .Click("OnClickPrintSertifikat");
            }).Title("Action");
        })
        .Pageable(pager => pager
                    .Refresh(true)
                    .Info(true)
                    .PageSizes(true)
                    )
        .Sortable()
        .Filterable()
        .Scrollable()
        .Height(380)
        .Resizable(a=>a.Columns(true))
        .Events(ev => ev.DataBound("gridAutoFit"))

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
