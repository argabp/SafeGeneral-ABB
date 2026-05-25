function btnAddMapping_OnClick() {
    var window = $("#EntriMappingWindow").data("kendoWindow");
    openWindow('#EntriMappingWindow', '/EntriMapping/Add', 'Add New Mapping');
}

$(document).ready(function () {
    $("#SearchKeyword").on("keyup", function() {
        $("#EntriMappingGrid").data("kendoGrid").dataSource.read();
    });
});

function getSearchFilter() {
    return {
        searchKeyword: $("#SearchKeyword").val()
    };
}

function onSaveEntriMapping() {
    var url = "/EntriMapping/Save";
    var data = {
        gl_akun104: $("#gl_akun104").val(),
        gl_akun117: $("#gl_akun117").val()
    };
    showProgress('#EntriMappingWindow');
    $.ajax({
        type: "POST",
        url: url,
        contentType: "application/json; charset=utf-8", 
        data: JSON.stringify(data),                    
        success: function (response) {
            closeProgress('#EntriMappingWindow');
            if (response && response.success) {
                console.log(response)
                showMessage('Success', 'Data berhasil disimpan.');
                closeWindow("#EntriMappingWindow");
                refreshGrid("#MappingGrid");
            } else {
                showMessage('Error', 'Gagal menyimpan data.');
            }
        },
        error: function (jqXHR, textStatus, errorThrown) {
            closeProgress('#EntriMappingWindow');
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

function btnEditEntriMapping_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var window = $("#EntriMappingWindow").data("kendoWindow");
    openWindow('#EntriMappingWindow', `/EntriMapping/Edit?gl_akun104=${dataItem.gl_akun104.trim()}`, 'Edit Mapping');
}

function onDeleteEntriMapping(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    // Tampilkan dialog konfirmasi
    showConfirmation('Konfirmasi Hapus', `Apakah Anda yakin ingin menghapus data dengan Akun ${dataItem.gl_akun104}?`,
        function () {
            showProgressOnGrid('#MappingGrid');

            // Gunakan FormData agar lolos dari validasi ketat Middleware JSON
            var formData = new FormData();
            formData.append("gl_akun104", dataItem.gl_akun104.trim());

            $.ajax({
                type: "POST",
                url: "/EntriMapping/DeleteMapping", // Arahkan ke endpoint yang baru
                data: formData,
                processData: false, // Wajib false untuk FormData
                contentType: false, // Wajib false untuk FormData
                success: function (response) {
                    closeProgressOnGrid('#MappingGrid');
                    
                    if (response && response.success) {
                        showMessage('Success', 'Data berhasil dihapus.');
                        refreshGrid("#MappingGrid"); 
                    } else {
                        // Jika gagal, pop-up ini akan menunjukkan APA penyebab aslinya
                        showMessage('Error', response.message ? response.message : 'Gagal menghapus data.');
                    }
                },
                error: function (jqXHR) {
                    closeProgressOnGrid('#MappingGrid');
                    showMessage('Error', 'Terjadi kesalahan pada server. Status: ' + jqXHR.status);
                }
            });
        }
    );
}