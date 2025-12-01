// File: ~/Modules/TipeAkun117/JS/tipeakun117.index.js

function btnAddTipeAkun_OnClick() {
    var window = $("#TipeAkunWindow").data("kendoWindow");
    window.refresh({ url: "/TipeAkun117/Add" });
    window.center().open();
}

function btnEditTipeAkun_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    var window = $("#TipeAkunWindow").data("kendoWindow");
    
    window.refresh({ url: `/TipeAkun117/Edit?id=${dataItem.Kode}` });
    window.center().open();
}



function btnDeleteTipeAkun_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    Swal.fire({
        title: 'Konfirmasi Hapus',
        text: "Anda yakin ingin menghapus Tipe Akun " + dataItem.NamaTipe + "?",
        icon: 'warning',
        showCancelButton: true,
        confirmButtonColor: '#d33',
        cancelButtonColor: '#3085d6',
        confirmButtonText: 'Ya, Hapus!'
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                type: "POST",
                url: `/TipeAkun117/Delete?id=${dataItem.Kode}`,
                success: function (response) {
                    if (response.success) {
                        Swal.fire('Terhapus!', 'Data berhasil dihapus.', 'success');
                        $("#TipeAkunGrid").data("kendoGrid").dataSource.read();
                    } else {
                        Swal.fire('Error', response.message, 'error');
                    }
                }
            });
        }
    });
}
function onSaveTipeAkun() {
    // 1. Ambil data
    var kode = $("#Kode").val();
    var namaTipe = $("#NamaTipe").val();
    var debetkredit = $("#DebetKredit").data("kendoDropDownList").value();   
    var pos = $("input[name='Pos']:checked").val(); // Radio button
    
    // --- PERUBAHAN DI SINI ---
    // Ambil status checkbox (true/false)
    // var kasbank = $("#KasbankCheckbox").is(":checked"); 

    // Validasi
    if (!kode || !namaTipe || !pos) {
        showMessage("Warning", "Harap lengkapi Kode, Nama Tipe, dan Posisi.");
        return;
    }

    var data = {
        Kode: kode,
        NamaTipe: namaTipe,
        Pos: pos,
        DebetKredit: debetkredit // Mengirim boolean langsung
    };

    $.ajax({
        type: "POST",
        url: "/TipeAkun117/Save",
        contentType: "application/json",
        data: JSON.stringify(data),
        success: function (response) {
            if (response.success) {
                showMessage("Success", "Data berhasil disimpan.");
                $("#TipeAkunWindow").data("kendoWindow").close();
                $("#TipeAkunGrid").data("kendoGrid").dataSource.read();
            } else {
                showMessage("Error", response.message);
            }
        },
        error: function () {
            showMessage("Error", "Terjadi kesalahan pada server.");
        }
    });
}

function closeWindow() {
    $("#TipeAkunWindow").data("kendoWindow").close();
}