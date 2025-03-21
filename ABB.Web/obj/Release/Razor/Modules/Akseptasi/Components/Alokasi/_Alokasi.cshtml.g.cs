#pragma checksum "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Akseptasi\Components\Alokasi\_Alokasi.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "74dc1def3b324bffe36d5c86cf67e0d7c7578afd"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Modules_Akseptasi_Components_Alokasi__Alokasi), @"mvc.1.0.view", @"/Modules/Akseptasi/Components/Alokasi/_Alokasi.cshtml")]
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
#line 1 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Akseptasi\Components\Alokasi\_Alokasi.cshtml"
using ABB.Application.Alokasis.Queries;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Akseptasi\Components\Alokasi\_Alokasi.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"74dc1def3b324bffe36d5c86cf67e0d7c7578afd", @"/Modules/Akseptasi/Components/Alokasi/_Alokasi.cshtml")]
    public class Modules_Akseptasi_Components_Alokasi__Alokasi : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Modules/Akseptasi/Components/Alokasi/alokasi.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "74dc1def3b324bffe36d5c86cf67e0d7c7578afd3553", async() => {
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

<div class=""window-body"">
    <div class=""row"">
        <div class=""col-md-4"">
            <input type=""text"" class=""search-field-top"" style=""height: 1em"" id=""SearchKeywordAlokasi"" placeholder=""Search""/>
        </div>
        <div class=""offset-5""></div>
        <div class=""col-md-2"">
            <button class=""flat-btn-primary"" style=""height: 1em"" id=""btnAddNewAkseptasiAlokasi"">Add New Alokasi</button>
        </div>
    </div>
    
    ");
#nullable restore
#line 17 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Akseptasi\Components\Alokasi\_Alokasi.cshtml"
Write(Html.Kendo().Grid<DetailAlokasiDto>()
        .Name("AlokasiGrid")
        .DataSource(dataSource => dataSource.Ajax()
            .Read(read => read.Action("GetDetailAlokasis", "Akseptasi").Data("searchFilterAlokasi"))
            .Model(model => model.Id(k => new { k.kd_cb, k.kd_cob, k.kd_scob, k.kd_thn, 
                k.no_pol, k.no_updt, k.no_rsk, k.kd_endt, k.no_updt_reas, k.kd_jns_sor, 
                k.kd_grp_sor, k.kd_rk_sor } ))
        )
        .Columns(columns =>
        {
            columns.Bound(col => col.kd_jns_sor).Width(150).Title("Jenis SOR");
            columns.Bound(col => col.kd_rk_sor).Width(150).Title("Kode SOR");
            columns.Bound(col => col.nilai_ttl_ptg_reas).Width(150).Title("TSI");
            columns.Bound(col => col.pst_share).Width(150).Title("Share (%)");
            columns.Bound(col => col.nilai_prm_reas).Width(150).Title("Premi");
            columns.Bound(col => col.pst_kms_reas).Width(150).Title("Pst (%)");
            columns.Bound(col => col.nilai_kms_reas).Width(150).Title("Komisi");
            columns.Bound(col => col.pst_adj_reas).Width(150).Title("Rate Adjustment");
            columns.Bound(col => col.stn_adj_reas).Width(150).Title("Rate Adjustment").ClientTemplate("#= data.stn_adj_reas == 1 ? '%' : '%o' #");
            columns.Bound(col => col.nilai_adj_reas).Width(150).Title("Premi Adjustment");
            columns.Command(command =>
            {
                command.Custom("EditAkseptasiAlokasi")
                    .Text("Edit")
                    .IconClass("fa fa-pencil-alt")
                    .HtmlAttributes(new { title = "Edit" })
                    .Click("btnEditAkseptasiAlokasi_OnClick");
                command.Custom("DeleteAkseptasiAlokasi")
                    .Text("Delete")
                    .IconClass("k-icon k-i-delete")
                    .HtmlAttributes(new { title = "Delete" })
                    .Click("btnDeleteAkseptasiAlokasi_OnClick");
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
        .AutoBind(false)
        .Height(450)
        .Resizable(a => a.Columns(true))
        .Events(ev => ev.DataBound("OnAkseptasiAlokasiDataBound"))
    
        );

#line default
#line hidden
#nullable disable
            WriteLiteral(@"
    
    <div id=""totalAlokasi"" class=""col-sm-12 row""></div>
    <div class=""window-footer"">
        <button type=""button"" id=""btn-previous-akseptasiResikoAlokasi"" class=""btn btn-primary space-right"">
            <span class=""fa fa-arrow-left""></span> Previous
        </button>
    </div>
</div>");
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
        public global::Microsoft.AspNetCore.Mvc.Rendering.IHtmlHelper<dynamic> Html { get; private set; }
    }
}
#pragma warning restore 1591
