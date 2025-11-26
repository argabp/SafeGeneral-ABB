$(document).ready(function () {
    btnNextResiko();
    searchKeywordResiko_OnKeyUp();
});

function btnNextResiko(){
    $('#btn-next-inquiryResiko').click(function () {
        $("#resikoTab").getKendoTabStrip().select(1);
    });
}

function searchKeywordResiko_OnKeyUp() {
    $('#SearchKeywordResiko').keyup(function () {
        refreshGrid("#InquiryResikoGrid");
    });
}

function openInquiryResikoWindow(url, title) {
    openWindow('#InquiryResikoWindow', url, title);
}

function btnViewInquiryResiko_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openInquiryResikoWindow(`/Inquiry/ViewResiko?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}`, 'View Resiko');
}

function searchFilterResiko() {
    return {
        searchkeyword: $("#SearchKeywordResiko").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#no_updt").val(),
    }
}

var resiko;

function OnResikoChange(e){
    var grid = e.sender;
    resiko = grid.dataItem(this.select());
    
    if(resiko.no_updt === 1){
        $("#btnCopyEndorsResiko").show();
    } else {
        $("#btnCopyEndorsResiko").hide();
    }

    var tabstrip = $('#resikoTab').data("kendoTabStrip");
    if(resiko.kd_endt === "I")
    {
        tabstrip.enable(tabstrip.items()[1]);
        tabstrip.enable(tabstrip.items()[2]);
        tabstrip.enable(tabstrip.items()[3]);
        tabstrip.enable(tabstrip.items()[4]);
    } else {
        tabstrip.disable(tabstrip.items()[1]);
        tabstrip.disable(tabstrip.items()[2]);
        tabstrip.disable(tabstrip.items()[3]);
        tabstrip.disable(tabstrip.items()[4]);
    }
}

function refreshTabOther(){
    var kd_cb = $("#kd_cb").val();
    var kd_cob = $("#kd_cob").val();
    var kd_scob = $("#kd_scob").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();
    var no_aks = $("#no_aks").val();
    var no_rsk = resiko?.no_rsk;
    var kd_endt = resiko?.kd_endt;

    var form = {};

    form.kd_cb = kd_cb;
    form.kd_cob = kd_cob;
    form.kd_scob = kd_scob;
    form.no_updt = no_updt;
    form.kd_thn = kd_thn;
    form.no_aks = no_aks;
    form.no_rsk = no_rsk;
    form.kd_endt = kd_endt;

    var data = JSON.stringify(form);

    ajaxPost("/Inquiry/GetResikoOther", data,
        function (response) {
            $("#tabOther").html(response);
        }
    );
}