$(document).ready(function () {
    btnPreviousOtherCargoDetail();
    searchKeywordOtherCargoDetail_OnKeyUp();
});


function btnPreviousOtherCargoDetail(){
    $('#btn-previous-inquiryOtherCargoDetail').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(0);
    });
}

function searchKeywordOtherCargoDetail_OnKeyUp() {
    $('#SearchKeywordOtherCargoDetail').keyup(function () {
        refreshGrid("#InquiryOtherCargoDetailGrid");
    });
}

function openInquiryOtherCargoDetailWindow(url, title) {
    openWindow('#InquiryOtherCargoDetailWindow', url, title);
}

function btnViewInquiryOtherCargoDetail_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openInquiryOtherCargoDetailWindow(`/Inquiry/ViewOtherCargoDetail?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&no_urut=${dataItem.no_urut}`, 'View Other Alat Angkut');
}

function searchFilterOtherCargoDetail() {
    return {
        searchkeyword: $("#SearchKeywordOtherCargoDetail").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#other_cargo_detail_no_updt").val(),
        kd_endt: $("#resiko_other_kd_endt").val(),
        no_rsk: resiko?.no_rsk,
    }
}
