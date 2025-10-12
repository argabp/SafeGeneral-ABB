$(document).ready(function () {
    btnPreviousOtherMotorDetail();
    searchKeywordOtherMotorDetail_OnKeyUp();
});


function btnPreviousOtherMotorDetail(){
    $('#btn-previous-inquiryOtherMotorDetail').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(0);
    });
}

function searchKeywordOtherMotorDetail_OnKeyUp() {
    $('#SearchKeywordOtherMotorDetail').keyup(function () {
        refreshGrid("#InquiryOtherMotorDetailGrid");
    });
}

function openInquiryOtherMotorDetailWindow(url, title) {
    openWindow('#InquiryOtherMotorDetailWindow', url, title);
}

function btnViewInquiryOtherMotorDetail_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openInquiryOtherMotorDetailWindow(`/Inquiry/ViewOtherMotorDetail?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}&thn_ptg_kend=${dataItem.thn_ptg_kend}`, 'View Other Motor Detail');
}

function searchFilterOtherMotorDetail() {
    return {
        searchkeyword: $("#SearchKeywordOtherMotorDetail").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#other_motor_detail_no_updt").val(),
        kd_endt: $("#resiko_other_kd_endt").val(),
        no_rsk: resiko?.no_rsk,
    }
}
