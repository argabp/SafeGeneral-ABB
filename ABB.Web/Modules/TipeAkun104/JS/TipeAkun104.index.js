// Fungsi untuk membuka window Tambah Data
function btnAddTipeAkun104_OnClick() {
    var window = $("#TipeAkun104Window").data("kendoWindow");
    openWindow('#TipeAkun104Window', '/TipeAkun104/Add', 'Add New Tipe Akun 104');
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#TipeAkun104Grid").data("kendoGrid").dataSource.read();
    });
});

function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}

function onSaveTipeAkun104() {
    var url = "/TipeAkun104/Save";
    var data = {
        Kode: $("#Kode").val(),
        NamaTipe: $("#NamaTipe").val(),
        Pos: $('input[name="Pos"]:checked').val(),
        Kasbank: $('input[name="Kasbank"]:checked').val() || "0"
    };
    showProgress('#TipeAkun104Window');
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8", 
        data: JSON.stringify(data),                    
        success: function (response) {
            closeProgress('#TipeAkun104Window');
            if (response && response.success) {
                console.log(response)
                showMessage('Success', 'Data berhasil disimpan.');
                closeWindow("#TipeAkun104Window");
                refreshGrid("#TipeAkun104Grid");
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeProgress('#TipeAkun104Window');
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

function onDeleteTipeAkun104(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan Kode ${dataItem.Kode}?`,
        function () {
            showProgressOnGrid('#TipeAkun104Grid');

            // Kirim request hapus ke Controller
            ajaxGet(`/TipeAkun104/Delete?Kode=${dataItem.Kode.trim()}`,
                function (response) {
                    if (response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                    } else {
                        showMessage('Error', 'Gagal menghapus data.');
                    }
                    
                    refreshGrid("#TipeAkun104Grid");
                    closeProgressOnGrid('#TipeAkun104Grid');
                }
            );
        }
    );
}



