#pragma checksum "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\ProsesRekonsiliasi\Views\CancelPayment.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "7b69935f7c685bc26c5347457aaccb3a8df95561"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Modules_ProsesRekonsiliasi_Views_CancelPayment), @"mvc.1.0.view", @"/Modules/ProsesRekonsiliasi/Views/CancelPayment.cshtml")]
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
#line 1 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\ProsesRekonsiliasi\Views\CancelPayment.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\ProsesRekonsiliasi\Views\CancelPayment.cshtml"
using ABB.Application.ProsesRekonsiliasi.Queries;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\ProsesRekonsiliasi\Views\CancelPayment.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"7b69935f7c685bc26c5347457aaccb3a8df95561", @"/Modules/ProsesRekonsiliasi/Views/CancelPayment.cshtml")]
    public class Modules_ProsesRekonsiliasi_Views_CancelPayment : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Modules/ProsesRekonsiliasi/JS/prosesRekonsiliasi.cancelPayment.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 9 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\ProsesRekonsiliasi\Views\CancelPayment.cshtml"
  
    Layout = "~/Modules/Shared/_Layout.cshtml";
    ViewData["Title"] = "Proses Rekonsiliasi Cancel Payment";

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "7b69935f7c685bc26c5347457aaccb3a8df955614121", async() => {
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
            WriteLiteral("\r\n\r\n<div class=\"flat-box\">\r\n    <div class=\"row\">\r\n        <div class=\"flat-box-title col-md-8\">\r\n            Cancel Payment\r\n        </div>\r\n    </div>\r\n\r\n    <div class=\"flat-box-content\">\r\n        ");
#nullable restore
#line 23 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\ProsesRekonsiliasi\Views\CancelPayment.cshtml"
    Write(Html.Kendo().Grid<ProsesRekonsiliasiDto>()
        .Name("CancelPaymentProsesRekonsiliasiGrid")
        .DataSource(dataSource => dataSource.Ajax()
            .Read(read => read.Action("GetProsesRekonsiliasiCancelPayments", "ProsesRekonsiliasi"))
        )
        .Columns(columns =>
        {
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
                command.Custom("Info")
                    .Text(" ")
                    .IconClass("fa fa-info-circle")
                    .HtmlAttributes(new { title = "Info" })
                    .Click("OnClickInfo");
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
            WriteLiteral("\r\n        \r\n        ");
#nullable restore
#line 66 "C:\Users\Formulatrix\RiderProjects\Mine\ABB\ABB.Web\Modules\ProsesRekonsiliasi\Views\CancelPayment.cshtml"
    Write(Html.Kendo().Window()
                            .Name("InfoWindow")
                            .Title("Info Page")
                            .Width(1000)
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
