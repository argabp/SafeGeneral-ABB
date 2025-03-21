#pragma checksum "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml" "{ff1816ec-aa5e-4d10-87f7-6f4963833460}" "34ea8ce02dede586c3fad3bc2eacc66bf2daf263"
// <auto-generated/>
#pragma warning disable 1591
[assembly: global::Microsoft.AspNetCore.Razor.Hosting.RazorCompiledItemAttribute(typeof(AspNetCore.Modules_Okupasi_Views_Index), @"mvc.1.0.view", @"/Modules/Okupasi/Views/Index.cshtml")]
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
#line 1 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml"
using Kendo.Mvc.UI;

#line default
#line hidden
#nullable disable
#nullable restore
#line 2 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml"
using ABB.Application.Okupasis.Queries;

#line default
#line hidden
#nullable disable
#nullable restore
#line 3 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml"
using Microsoft.Extensions.Configuration;

#line default
#line hidden
#nullable disable
    [global::Microsoft.AspNetCore.Razor.Hosting.RazorSourceChecksumAttribute(@"SHA1", @"34ea8ce02dede586c3fad3bc2eacc66bf2daf263", @"/Modules/Okupasi/Views/Index.cshtml")]
    public class Modules_Okupasi_Views_Index : global::Microsoft.AspNetCore.Mvc.Razor.RazorPage<dynamic>
    {
        private static readonly global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute __tagHelperAttribute_0 = new global::Microsoft.AspNetCore.Razor.TagHelpers.TagHelperAttribute("src", new global::Microsoft.AspNetCore.Html.HtmlString("~/Modules/Okupasi/JS/okupasi.index.js"), global::Microsoft.AspNetCore.Razor.TagHelpers.HtmlAttributeValueStyle.DoubleQuotes);
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
#line 9 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml"
  
    Layout = "~/Modules/Shared/_Layout.cshtml";
    ViewData["Title"] = "Okupasi";

#line default
#line hidden
#nullable disable
            __tagHelperExecutionContext = __tagHelperScopeManager.Begin("script", global::Microsoft.AspNetCore.Razor.TagHelpers.TagMode.StartTagAndEndTag, "34ea8ce02dede586c3fad3bc2eacc66bf2daf2633938", async() => {
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
    #kd_okup{
        background-color: #00FFFF;
    }
    #nm_okup, #nm_okup_ing{
        background-color: #FFFFE1;
    }
</style>

<div class=""flat-box"">
    <div class=""row"">
        <div class=""flat-box-title col-md-6"">
            Okupasi
        </div>
    </div>

    <div class=""flat-box-content"">
        <div class=""div-box"">
            ");
#nullable restore
#line 36 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml"
        Write(Html.Kendo().Grid<OkupasiDto>()
                .Name("OkupasiGrid")
                .ToolBar(toolbar => toolbar.Create())
                .Editable(editable => editable.Mode(GridEditMode.InLine))
                .DataSource(dataSource => dataSource.Ajax()
                    .Read(read => read.Action("GetOkupasis", "Okupasi").Data("searchFilter"))
                    .Model(model => model.Id(p => p.kd_okup))
                )
                .Columns(columns =>
                {
                    columns.Bound(col => col.kd_okup).Width(150).Title("Kode Okupasi").Editable("disableGridTextboxWhenEdit");
                    columns.Bound(col => col.nm_okup).Width(150).Title("Nama Okupasi");
                    columns.Bound(col => col.nm_okup_ing).Width(150).Title("Nama Okupasi Inggris");
                    columns.Bound(col => col.kd_category).Width(150).Title("Kode Category");
                    columns.Command(command =>
                    {
                        command.Edit();
                        command.Custom("DeleteOkupasi")
                            .Text("Delete")
                            .IconClass("k-icon k-i-delete")
                            .HtmlAttributes(new { title="Delete"})
                            .Click("onDeleteOkupasi");
                    }).Width(200);
                })
                .Pageable(pager => pager
                    .Refresh(true)
                    .Info(true)
                    .PageSizes(true)
                )
                .ClientDetailTemplateId("template")
                .Sortable()
                .Filterable()
                .Scrollable()
                .Height(450)
                .Resizable(a=>a.Columns(true))
                .Events(ev => ev.DataBound("gridAutoFit").Save("onSaveOkupasi"))
        
                );

#line default
#line hidden
#nullable disable
            WriteLiteral(@"    
        </div>
        
        <script id=""template"" type=""text/kendo-tmpl"">
            <div class=""row"">
                <div class=""col-md-4 k-button k-button-icontext"">
                    <button style=""border: none;"" onclick=""btnAddDetailOkupasi_OnClick(#=kd_okup#)"">+ ADD NEW RECORD</button>
                </div>
            </div>
            ");
#nullable restore
#line 82 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml"
        Write(Html.Kendo().TabStrip()
                    .Name("tabStrip_#=kd_okup#")
                    .SelectedIndex(0)
                    .Animation(animation => animation.Open(open => open.Fade(FadeDirection.In)))
                    .Items(items =>
                    {
                        items.Add().Text("Detail").Content(item => new global::Microsoft.AspNetCore.Mvc.Razor.HelperResult(async(__razor_template_writer) => {
    PushWriter(__razor_template_writer);
    WriteLiteral("\r\n                            ");
#nullable restore
#line 89 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml"
                        Write(Html.Kendo().Grid<DetailOkupasiDto>()
                                .Name("grid_Detail_#=kd_okup#") // template expression, to be evaluated in the master context
                                .DataSource(dataSource => dataSource.Ajax()
                                    .Read(read => read.Action("GetDetailOkupasis", "Okupasi", new { kd_okup = "#=kd_okup#" }))//.Data("getAsumsiPeriodeParam"))
                                    .Model(m => m.Id(p => p.Id))
                                )
                                .Columns(columns =>
                                {
                                    columns.Bound(col => col.kd_okup).Width(150).Title("Kode Okupasi").Editable("disableGridTextbox");
                                    columns.Bound(col => col.text_kls_konstr).Width(150).Title("Kode Kelas Konstruksi").Editable("disableGridTextboxWhenEdit");
                                    columns.Bound(col => col.text_stn_rate_premi).Width(150).Title("Satuan Rate Premi").EditorTemplateName("KendoNumericTextBoxInt");
                                    columns.Bound(col => col.pst_rate_prm).Width(200).Title("Rate Premi").Format("{0:n2}").EditorTemplateName("KendoNumericTextBox");
                                    columns.Command(command =>
                                    {
                                        command.Custom("EditDetailOkupasi")
                                            .Text("Edit")
                                            .IconClass("fa fa-pencil-alt")
                                            .HtmlAttributes(new { title = "Edit" })
                                            .Click("btnEditDetailOkupasi_OnClick");
                                        command.Custom("DeleteDetailOkupasi")
                                            .Text("Delete")
                                            .IconClass("k-icon k-i-delete")
                                            .HtmlAttributes(new { title="Delete"})
                                            .Click("onDeleteDetailOkupasi");
                                    }).Width(200);
                                })
                                .Pageable(pager => pager
                                    .Refresh(true)
                                    .Info(true)
                                    .PageSizes(true)
                                )
                                .Sortable()
                                .Selectable()
                                .Filterable()
                                .Scrollable()
                                .Height(380)
                                .Resizable(a=>a.Columns(true))
                                .Events(ev => ev.DataBound("gridAutoFit").Save("onSaveDetailOkupasi").Edit("onEditDetailOkupasi"))
                                .ToClientTemplate());

#line default
#line hidden
#nullable disable
    WriteLiteral("\r\n                        ");
    PopWriter();
}
)
                        );
                    })
                    .ToClientTemplate());

#line default
#line hidden
#nullable disable
            WriteLiteral("\r\n        </script>\r\n\r\n        ");
#nullable restore
#line 134 "C:\Users\FORMULATRIX\RiderProjects\Mine\SafeGeneralABB\ABB.Web\Modules\Okupasi\Views\Index.cshtml"
    Write(Html.Kendo().Window()
            .Name("DetailOkupasiWindow")
            .Title("Detail Okupasi")
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
