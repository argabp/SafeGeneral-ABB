// Fungsi untuk membuka window Tambah Data
function btnAddKasBank_OnClick() {
    openWindow('#KasBankWindow', '/KasBank/Add', 'Add New Kas Bank');
}

// Fungsi untuk membuka window Edit Data
// Fungsi untuk membuka window Edit Data
function btnEditKasBank_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    if (!dataItem) return;

    // Ambil 3 kunci utamanya
    var kodeCabang = dataItem.KodeCabang.trim();
    var kode = dataItem.Kode.trim();
    var tipeKasBank = dataItem.TipeKasBank.trim();

    // Kirim ketiganya lewat URL
    var url = `/KasBank/Edit?kodeCabang=${kodeCabang}&kode=${kode}&tipeKasBank=${tipeKasBank}`;
    
    openWindow('#KasBankWindow', url, 'Edit Kas Bank');
}

// Fungsi untuk menyimpan data (dari form Add atau Edit)
function onSaveKasBank() {
   var kodeCabangVal = "";
    var cb = $("#KodeCabang").data("kendoComboBox");
    if(cb) {
        kodeCabangVal = cb.value().trim();
    } else {
        kodeCabangVal = $("#KodeCabang").val();
    }

    var url = "/KasBank/Save";
    var data = {
        Kode: $("#Kode").val(),
        Keterangan: $("#Keterangan").val(),
        NoRekening: $("#NoRekening").val(),
        NoPerkiraan: $("#NoPerkiraan").val(),
        TipeKasBank: $("#TipeKasBank").val(),
        Saldo: $("#Saldo").val(),
        kodeCabang : kodeCabangVal
    };

    console.log(data)
    
    showProgress('#KasBankWindow');

    $.ajax({
        type: "POST",
        url: url,
        // ---> DUA BARIS INI SANGAT PENTING <---
        contentType: "application/json; charset=utf-8", // 1. Beritahu server kita mengirim JSON
        data: JSON.stringify(data),                     // 2. Ubah objek JavaScript menjadi string JSON
        
        success: function (response) {
            closeProgress('#KasBankWindow');
            if (response && response.success) {
                showMessage('Success', 'Data berhasil disimpan.');
                closeWindow("#KasBankWindow");
                refreshGrid("#KasBankGrid");
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeProgress('#KasBankWindow');
            if (jqXHR.status === 400) {
                // Menangani error validasi jika ada
                var errorData = jqXHR.responseJSON;
                var errorMessage = "Terdapat error validasi:\n";
                for (var key in errorData.errors) {
                    errorMessage += "- " + errorData.errors[key][0] + "\n";
                }
                alert(errorMessage);
            } else {
                showMessage('Error', 'Tidak dapat terhubung ke server. Status: ' + jqXHR.status);
            }
        }
    });
}

// Fungsi untuk menghapus data
function onDeleteKasBank(e) {
    e.preventDefault();

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // 🔒 pastikan data ada
    if (!dataItem) return;

    showConfirmation(
        'Konfirmasi Hapus',
        `Apakah Anda yakin ingin menghapus data ?`,
        function () {

            showProgressOnGrid('#KasBankGrid');

            // 🔑 kirim composite key
            var param = {
                KodeCabang: dataItem.KodeCabang,
                TipeKasBank: dataItem.TipeKasBank,
                Kode: dataItem.Kode
            };

            ajaxPost(
                '/KasBank/Delete',
                JSON.stringify(param),
                function (response) {

                    if (response && response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                        refreshGrid("#KasBankGrid");
                    } else {
                        showMessage('Error', response?.message || 'Gagal menghapus data.');
                    }

                    closeProgressOnGrid('#KasBankGrid');
                }
            );
        }
    );
}




$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#KasBankGrid").data("kendoGrid").dataSource.read();
    });
});
    function getSearchFilter() {
        return {
            searchKeyword: $("#SearchKeyword").val()
        };
    }