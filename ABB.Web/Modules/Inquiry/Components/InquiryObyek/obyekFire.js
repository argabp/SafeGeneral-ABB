$(document).ready(function () {
    btnPreviousObyek();
    btnNextObyek();
    searchKeywordObyek_OnKeyUp();
});


function btnPreviousObyek(){
    $('#btn-previous-inquiryResikoObyek').click(function () {
        $("#resikoTab").getKendoTabStrip().select(1);
    });
}
function btnNextObyek(){
    $('#btn-next-inquiryResikoObyek').click(function () {
        $("#resikoTab").getKendoTabStrip().select(3);
    });
}

function searchKeywordObyek_OnKeyUp() {
    $('#SearchKeywordObyek').keyup(function () {
        refreshGrid("#InquiryObyekGrid");
    });
}

function openInquiryObyekWindow(url, title) {
    openWindow('#InquiryObyekWindow', url, title);
}

function btnViewInquiryObyek_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openInquiryObyekWindow(`/Inquiry/ViewObyekFire?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_oby=${dataItem.no_oby}&pst_share=${resiko.pst_share_bgu}`, 'View Obyek');
}

function searchFilterObyek() {
    return {
        searchkeyword: $("#SearchKeywordObyek").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#no_updt").val(),
        no_rsk: resiko?.no_rsk,
        kd_endt: resiko?.kd_endt
    }
}
