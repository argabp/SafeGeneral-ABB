$(document).ready(function () {
    btnPreviousObyekCIT();
    btnNextObyekCIT();
    searchKeywordObyekCIT_OnKeyUp();
});

function btnPreviousObyekCIT(){
    $('#btn-previous-inquiryResikoObyekCIT').click(function () {
        $("#resikoTab").getKendoTabStrip().select(1);
    });
}
function btnNextObyekCIT(){
    $('#btn-next-inquiryResikoObyekCIT').click(function () {
        $("#resikoTab").getKendoTabStrip().select(3);
    });
}

function searchKeywordObyekCIT_OnKeyUp() {
    $('#SearchKeywordObyekCIT').keyup(function () {
        refreshGrid("#InquiryObyekGrid");
    });
}

function openInquiryObyekCITWindow(url, title) {
    openWindow('#InquiryObyekWindow', url, title);
}

function btnViewInquiryObyekCIT_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openInquiryObyekCITWindow(`/Inquiry/ViewObyekCIT?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_oby=${dataItem.no_oby}&pst_share=${resiko.pst_share_bgu}`, 'View Obyek');
}

function searchFilterObyekCIT() {
    return {
        searchkeyword: $("#SearchKeywordObyekCIT").val(),
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
