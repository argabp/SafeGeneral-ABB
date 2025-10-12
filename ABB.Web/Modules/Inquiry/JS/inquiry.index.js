$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#InquiryGrid");
    });
}

function openInquiryWindow(url, title) {
    openWindow('#InquiryWindow', url, title);
}

function btnViewInquiry_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    resiko = null;
    openInquiryWindow(`/Inquiry/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}`, 'View Inquiry');
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}
