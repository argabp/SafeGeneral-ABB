// Fungsi untuk membuka window Tambah Data
function btnAddKasBank_OnClick() {
    openWindow('#KasBankWindow', '/KasBank/Add', 'Add New Kas Bank');
}

// Fungsi untuk membuka window Edit Data
function btnEditKasBank_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    // Parameter 'id' harus cocok dengan yang ada di Controller (public IActionResult Edit(string id))
    openWindow('#KasBankWindow', `/KasBank/Edit?id=${dataItem.Kode.trim()}`, 'Edit Kas Bank');
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

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan kode ${dataItem.Kode}?`,
        function () {
            showProgressOnGrid('#KasBankGrid');

            // Kirim request hapus ke Controller
            ajaxGet(`/KasBank/Delete?id=${dataItem.Kode.trim()}`,
                function (response) {
                    if (response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                    } else {
                        showMessage('Error', 'Gagal menghapus data.');
                    }
                    
                    refreshGrid("#KasBankGrid");
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