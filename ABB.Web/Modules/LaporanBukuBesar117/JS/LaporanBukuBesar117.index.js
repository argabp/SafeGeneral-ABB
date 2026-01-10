function generateBukuBesar117() {
        var kodeCabang = $("#KodeCabang").val();
        var tgl1 = kendo.toString($("#tgl1").data("kendoDatePicker").value(), "yyyy-MM-dd");
        var tgl2 = kendo.toString($("#tgl2").data("kendoDatePicker").value(), "yyyy-MM-dd");
        var akun1 = $("#AkunAwal").data("kendoComboBox").value();
        var akun2 = $("#AkunAkhir").data("kendoComboBox").value();

        if (!tgl1 || !tgl2) {
            alert("Periode tanggal harus diisi!");
            return;
        }

        var payload = {
            KodeCabang: kodeCabang,
            PeriodeAwal: tgl1,
            PeriodeAkhir: tgl2,
            AkunAwal: akun1,
            AkunAkhir: akun2
        };
        
        // Panggil Controller LaporanBukuBesar117
        $.ajax({
            url: '/LaporanBukuBesar117/GenerateReport',
            type: 'POST',
            contentType: 'application/json',
            data: JSON.stringify(payload),
            success: function (res) {
                if (res.Status === "OK") {
                    window.open("/Reports/" + res.Data + "/LaporanBukuBesar117.pdf", "_blank");
                } else {
                    alert("Error: " + res.Message);
                }
            },
            error: function () {
                alert("Terjadi kesalahan server");
            }
        });
    }