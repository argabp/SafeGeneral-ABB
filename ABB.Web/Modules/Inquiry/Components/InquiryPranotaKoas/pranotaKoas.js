$(document).ready(function () {
    btnPreviousPranotaKoas();
    searchKeywordPranotaKoas_OnKeyUp();
});

function btnPreviousPranotaKoas(){
    $('#btn-previous-inquiryResikoPranotaKoas').click(function () {
        $("#pranotaTab").getKendoTabStrip().select(0);
    });
}

function searchKeywordPranotaKoas_OnKeyUp() {
    $('#SearchKeywordPranotaKoas').keyup(function () {
        refreshGrid("#InquiryPranotaKoasGrid");
    });
}

function openInquiryPranotaKoasWindow(url, title) {
    openWindow('#InquiryPranotaKoasWindow', url, title);
}

function btnViewInquiryPranotaKoas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openInquiryPranotaKoasWindow(`/Inquiry/ViewPranotaKoas?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}&kd_mtu=${dataItem.kd_mtu}&kd_grp_pas=${dataItem.kd_grp_pas}&kd_rk_pas=${dataItem.kd_rk_pas}`, 'View Koasuransi Member');
}

function searchFilterPranotaKoas() {
    return {
        searchkeyword: $("#SearchKeywordPranotaKoas").val(),
        kd_cb: pranota?.kd_cb,
        kd_cob: pranota?.kd_cob,
        kd_scob: pranota?.kd_scob,
        kd_thn: pranota?.kd_thn,
        no_pol: pranota?.no_pol,
        no_updt: pranota?.no_updt,
        kd_mtu: pranota?.kd_mtu,
    }
}

function onPranotaKoasBound(e){
    var totalPercentage = 0;
    var totalPremi = 0;
    e.sender.dataSource.data().forEach((value, key) => {
        totalPercentage += value.pst_share;
        totalPremi += value.nilai_prm + value.nilai_kl;
    });
    
    $("#totalPercentagePranotaKoas").text(totalPercentage);
    $("#totalPremiPranotaKoas").text(totalPremi);
}
