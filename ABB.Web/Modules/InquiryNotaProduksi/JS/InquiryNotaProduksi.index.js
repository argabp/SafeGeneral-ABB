function btnViewInquiryNota_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow(
        '#InquiryNotaProduksiWindow',
        `/InquiryNotaProduksi/Add?id=${dataItem.id}`, // Asumsi 'Add' adalah action untuk view detail
        'Inquiry Nota'
    );
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#InquiryNotaProduksiGrid").data("kendoGrid").dataSource.read();
    });
});

function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}

function getAllFilters() {
    var startDatePicker = $("#StartDate").data("kendoDatePicker");
    var endDatePicker = $("#EndDate").data("kendoDatePicker");

    return {
        searchKeyword: $("#SearchKeyword").val(),
        // Kirim tanggal dalam format yang benar dan tangani nilai null
        startDate: startDatePicker && startDatePicker.value() ? kendo.toString(startDatePicker.value(), "yyyy-MM-dd") : null,
        endDate: endDatePicker && endDatePicker.value() ? kendo.toString(endDatePicker.value(), "yyyy-MM-dd") : null
    };
}


function onSearchClick() {
    // Cukup perintahkan grid untuk membaca ulang datanya
    $("#InquiryNotaProduksiGrid").data("kendoGrid").dataSource.read();
}


// Fungsi untuk membuka window view
    
$(document).ready(function () {
    // Pasang event listener 'keyup' pada search box
    $("#SearchKeyword").on("keyup", function() {
        $("#InquiryNotaProduksiGrid").data("kendoGrid").dataSource.read();
    });
});