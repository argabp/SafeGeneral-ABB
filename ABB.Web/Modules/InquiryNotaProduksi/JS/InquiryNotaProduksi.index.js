function btnViewInquiryNota_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow(
        '#InquiryNotaProduksiWindow',
        `/InquiryNotaProduksi/Add?id=${dataItem.id}`, // Asumsi 'Add' adalah action untuk view detail
        'Inquiry Nota'
    );
}

function btnPembayaranInquiryNota_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log(dataItem.no_nd)
    openWindow(
        '#InquiryNotaProduksiPembayaranWindow',
        `/InquiryNotaProduksi/Pembayaran?no_nd=${dataItem.no_nd}`, // Asumsi 'Add' adalah action untuk view detail
        'Inquiry Nota'
    );
}

function btnKeteranganInquiryNota_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log(dataItem.no_nd)
    openWindow(
        '#InquiryNotaProduksiKeterangannWindow',
        `/InquiryNotaProduksi/Keterangan?id=${dataItem.id}`, // Asumsi 'Add' adalah action untuk view detail
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
    var jenisAssetCombo = $("#JenisAsset").data("kendoComboBox");

    return {
        searchKeyword: $("#SearchKeyword").val(),
        startDate: startDatePicker && startDatePicker.value() ? kendo.toString(startDatePicker.value(), "yyyy-MM-dd") : null,
        endDate: endDatePicker && endDatePicker.value() ? kendo.toString(endDatePicker.value(), "yyyy-MM-dd") : null,
        jenisAsset: jenisAssetCombo && jenisAssetCombo.value() ? jenisAssetCombo.value() : null
    };
}



function onSearchClick() {
    // Cukup perintahkan grid untuk membaca ulang datanya
    $("#InquiryNotaProduksiGrid").data("kendoGrid").dataSource.read();
}

function onSaveKeterangan() {

    var data = {
        IdNota: $("#IdNota").val(),
        NoNota: $("#NoNota").val(),
        Tanggal: $("#Tanggal").data("kendoDatePicker").value(),
        Keterangan: $("#Keterangan").val()
    };

    $.ajax({
        type: "POST",
        url: "/InquiryNotaProduksi/SaveKeterangan",
        contentType: "application/json; charset=utf-8",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage('Success', 'Data berhasil disimpan');
                // closeWindow("#InquiryNotaProduksiKeterangannWindow");
                refreshGrid("#InquiryNotaProduksiGrid");
                $("#KeteranganProduksiGrid").data("kendoGrid").dataSource.read();
            } else {
                showMessage('Error', response.message);
            }
        }
    });
}


// Fungsi untuk membuka window view
    
$(document).ready(function () {
    // Pasang event listener 'keyup' pada search box
    $("#SearchKeyword").on("keyup", function() {
        $("#InquiryNotaProduksiGrid").data("kendoGrid").dataSource.read();
    });
});