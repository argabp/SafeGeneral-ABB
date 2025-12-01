// Fungsi untuk membuka window Tambah Data
function btnAddCOA117_OnClick() {
    var window = $("#Coa117Window").data("kendoWindow");
    openWindow('#Coa117Window', '/COA117/Add', 'Add New COA 117');
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#Coa117Grid").data("kendoGrid").dataSource.read();
    });
});

function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}

function onSaveCoa117() {
    var url = "/COA117/Save";
    var data = {
        Kode: $("#Kode").val(),
        Nama: $("#Nama").val(),
        Dept: $("#Dept").val(),
        Type: $("#Type").val()
    };
    showProgress('#Coa117Window');
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8", 
        data: JSON.stringify(data),                    
        success: function (response) {
            closeProgress('#Coa117Window');
            if (response && response.success) {
                console.log(response)
                showMessage('Success', 'Data berhasil disimpan.');
                closeWindow("#Coa117Window");
                refreshGrid("#Coa117Grid");
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeProgress('#Coa117Window');
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

function btnEditCoa117_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var window = $("#Coa117Window").data("kendoWindow");
    openWindow('#Coa117Window', `/COA117/Edit?Kode=${dataItem.Kode.trim()}`, 'Edit Coa117');
}

function onDeleteCoa117(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan Kode ${dataItem.Kode}?`,
        function () {
            showProgressOnGrid('#Coa117Grid');

            // Kirim request hapus ke Controller
            ajaxGet(`/COA117/Delete?Kode=${dataItem.Kode.trim()}`,
                function (response) {
                    if (response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                    } else {
                        showMessage('Error', 'Gagal menghapus data.');
                    }
                    
                    refreshGrid("#Coa117Grid");
                    closeProgressOnGrid('#Coa117Grid');
                }
            );
        }
    );
}

function openPilihCoa117Window() {
    var window = $("#PilihCoa117Window").data("kendoWindow");
    // Muat konten dari action PilihNota
    window.refresh({ url: "/COA117/PilihTypeCoa" });
    window.center().open();
}

function onTypeCoa117Select(e) {
    // Ambil data dari baris yang dipilih
    var selectedRow = this.dataItem(this.select());

    if (selectedRow) {
        // "Replace" nilai di form utama dengan data dari pop-up
        $("#Type").val(selectedRow.Type);
        // Anda juga bisa mengisi field lain jika perlu, contoh:
        // $("#TotalBayar").data("kendoNumericTextBox").value(selectedRow.Premi);
        
        // Tutup window pop-up setelah dipilih
        $("#PilihCoa117Window").data("kendoWindow").close();
    }
}

