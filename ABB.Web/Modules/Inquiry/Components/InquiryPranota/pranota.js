$(document).ready(function () {
    btnNextPranota();
});

var pranota;

function btnNextPranota(){
    $('#btn-next-inquiryPranota').click(function () {
        $("#pranotaTab").getKendoTabStrip().select(1);
    });
}

function searchKeywordPranota_OnKeyUp() {
    $('#SearchKeywordPranota').keyup(function () {
        refreshGrid("#InquiryPranotaGrid");
    });
}

function openInquiryPranotaWindow(url, title) {
    openWindow('#InquiryPranotaWindow', url, title);
}

function btnViewInquiryPranota_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openInquiryPranotaWindow(`/Inquiry/ViewPranota?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}&kd_mtu=${dataItem.kd_mtu}`, 'View Pranota');
}

function searchFilterPranota() {
    return {
        searchkeyword: $("#SearchKeywordPranota").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_pol: $("#no_pol").val(),
        no_updt: $("#no_updt").val(),
    }
}

function OnPranotaChange(e){
    var grid = e.sender;
    pranota = grid.dataItem(this.select());
    refreshGrid("#InquiryPranotaKoasGrid");
}