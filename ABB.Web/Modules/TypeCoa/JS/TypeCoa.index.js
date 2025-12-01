// Fungsi untuk membuka window Tambah Data
function btnAddTypeCoa_OnClick() {
    var window = $("#TypeCoaWindow").data("kendoWindow");
    openWindow('#TypeCoaWindow', '/TypeCoa/Add', 'Add New Tipe Akun 104');
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#TypeCoaGrid").data("kendoGrid").dataSource.read();
    });
});

function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}

function onSaveTypeCoa() {
    // 1. Ambil data
    var kode = $("#Type").val();
    var namaTipe = $("#Nama").val();
    var pos = $("input[name='Pos']:checked").val(); 
    var DebetKredit = $("#Dk").data("kendoDropDownList").value();   
    
    console.log(DebetKredit);
   

    // Validasi
    if (!kode || !namaTipe || !pos) {
        showMessage("Warning", "Harap lengkapi Kode, Nama Tipe, dan Posisi.");
        return;
    }

    var data = {
        Type: kode,
        Nama: namaTipe,
        Pos: pos,
        Dk: DebetKredit 
    };

    $.ajax({
        type: "POST",
        url: "/TypeCoa/Save",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage("Success", "Data berhasil disimpan.");
                $("#TypeCoaWindow").data("kendoWindow").close();
                $("#TypeCoaGrid").data("kendoGrid").dataSource.read();
            } else {
                showMessage("Error", response.message);
            }
        },
        error: function () {
            showMessage("Error", "Terjadi kesalahan pada server.");
        }
    });
}

function onDeleteTypeCoa(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan Kode ${dataItem.Type}?`,
        function () {
            showProgressOnGrid('#TypeCoaGrid');

            // Kirim request hapus ke Controller
            ajaxGet(`/TypeCoa/Delete?Type=${dataItem.Type.trim()}`,
                function (response) {
                    if (response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                    } else {
                        showMessage('Error', 'Gagal menghapus data.');
                    }
                    
                    refreshGrid("#TypeCoaGrid");
                    closeProgressOnGrid('#TypeCoaGrid');
                }
            );
        }
    );
}

function btnEditTypeCoa_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var window = $("#TypeCoaWindow").data("kendoWindow");
    
    window.refresh({ url: `/TypeCoa/Edit?id=${dataItem.Type}` });
    window.center().open();
}

function closeWindow() {
    $("#TipeAkunWindow").data("kendoWindow").close();
}



