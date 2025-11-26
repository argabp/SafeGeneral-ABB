// Fungsi untuk membuka window Tambah Data
function btnAddCOA_OnClick() {
    var window = $("#CoaWindow").data("kendoWindow");
    openWindow('#CoaWindow', '/COA/Add', 'Add New COA');
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#CoaGrid").data("kendoGrid").dataSource.read();
    });
});

function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}

function onSaveCoa() {
    var url = "/COA/Save";
    var data = {
        Kode: $("#Kode").val(),
        Nama: $("#Nama").val(),
        Dept: $("#Dept").val(),
        Type: $("#Type").val()
    };
    showProgress('#CoaWindow');
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8", 
        data: JSON.stringify(data),                    
        success: function (response) {
            closeProgress('#CoaWindow');
            if (response && response.success) {
                console.log(response)
                showMessage('Success', 'Data berhasil disimpan.');
                closeWindow("#CoaWindow");
                refreshGrid("#CoaGrid");
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeProgress('#CoaWindow');
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

function btnEditCoa_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var window = $("#CoaWindow").data("kendoWindow");
    openWindow('#CoaWindow', `/COA/Edit?Kode=${dataItem.Kode.trim()}`, 'Edit COA');
}

function onDeleteCoa(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan Kode ${dataItem.Kode}?`,
        function () {
            showProgressOnGrid('#CoaGrid');

            // Kirim request hapus ke Controller
            ajaxGet(`/COA/Delete?Kode=${dataItem.Kode.trim()}`,
                function (response) {
                    if (response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                    } else {
                        showMessage('Error', 'Gagal menghapus data.');
                    }
                    
                    refreshGrid("#CoaGrid");
                    closeProgressOnGrid('#CoaGrid');
                }
            );
        }
    );
}

function openPilihCoaWindow() {
    var window = $("#PilihCoaWindow").data("kendoWindow");
    // Muat konten dari action PilihNota
    window.refresh({ url: "/COA/PilihTypeCoa" });
    window.center().open();
}

function onTypeCoaSelect(e) {
    // Ambil data dari baris yang dipilih
    var selectedRow = this.dataItem(this.select());

    if (selectedRow) {
        // "Replace" nilai di form utama dengan data dari pop-up
        $("#Type").val(selectedRow.Type);
        // Anda juga bisa mengisi field lain jika perlu, contoh:
        // $("#TotalBayar").data("kendoNumericTextBox").value(selectedRow.Premi);
        
        // Tutup window pop-up setelah dipilih
        $("#PilihTypeCoaWindow").data("kendoWindow").close();
    }
}

