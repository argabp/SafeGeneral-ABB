function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onDeleteTemplateJurnal117(e){
    e.preventDefault();
    
    var gridWidget = this;
    var gridId = gridWidget.element.attr("id"); 
    var gridSelector = "#" + gridId; 
    // -------------------------

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                Type: dataItem.Type,         
                JenisAss: dataItem.JenisAss
            };
            
            console.log(data);
            // Sekarang selector ini pasti benar (misal: #grid_detail_1024)
            showProgressOnGrid(gridSelector);

            ajaxPost("/TemplateJurnal117/DeleteTemplateJurnal117", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                        refreshGrid(gridSelector);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    closeProgressOnGrid(gridSelector);
                }
            );
        }
    );
}

function onDeleteTemplateJurnalDetail117(e){
    e.preventDefault();
    
    // --- PERBAIKAN DI SINI ---
    // 'this' merujuk ke Kendo Grid Widget.
    // Kita ambil ID elemen HTML grid-nya secara langsung.
    // Ini jauh lebih aman daripada mengandalkan dataItem.GridId.
    var gridWidget = this;
    var gridId = gridWidget.element.attr("id"); 
    var gridSelector = "#" + gridId; 
    // -------------------------

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                Type: dataItem.Type,         
                JenisAss: dataItem.JenisAss, 
                GlAkun: dataItem.GlAkun
            };
            
            // Sekarang selector ini pasti benar (misal: #grid_detail_1024)
            showProgressOnGrid(gridSelector);

            ajaxPost("/TemplateJurnal117/DeleteTemplateJurnalDetail117", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                        refreshGrid(gridSelector);
                    } else {
                        showMessage('Error', response.Message);
                    }

                    closeProgressOnGrid(gridSelector);
                }
            );
        }
    );
}

function onSaveTemplateJurnal117(dataItem){    
    var url = dataItem.model.isNew() ? "/TemplateJurnal117/AddTemplateJurnal117" : "/TemplateJurnal117/EditTemplateJurnal117";

    var data = {
        Type: dataItem.model.Type,
        JenisAss: dataItem.model.JenisAss,
        NamaJurnal: dataItem.model.NamaJurnal
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#TemplateJurnal117Grid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}




function onSaveTemplateJurnalDetail117(e) {
    // Agar Kendo tidak menjalankan save bawaan (karena kita pakai manual Ajax)
    // e.preventDefault(); // Opsional, tergantung config datasource

    var model = e.model;
    
    // PERBAIKAN: Ambil ID Grid dari sender event, bukan dari model data
    // e.sender.element adalah elemen <div> grid tersebut
    var gridElement = e.sender.element;
    var gridSelector = "#" + gridElement.attr("id"); 

    var url = model.isNew() ? "/TemplateJurnal117/AddTemplateJurnalDetail117"
                            : "/TemplateJurnal117/EditTemplateJurnalDetail117";

    var data = {
        // Pastikan di-TRIM agar key cocok di DB
        Type: model.Type ? model.Type.trim() : "",
        JenisAss: model.JenisAss ? model.JenisAss.trim() : "",
        GlAkun: model.GlAkun ? model.GlAkun.trim() : "",
        
        GlRumus: model.GlRumus,
        GlDk: model.GlDk,
        GlUrut: model.GlUrut,
        FlagDetail: model.FlagDetail,
        // Konversi Boolean Grid ke String/Boolean sesuai ViewModel C#
        FlagNt: model.FlagNt 
    };

    showProgressOnGrid(gridSelector);

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                // Refresh Grid yang benar
                refreshGrid(gridSelector);
            } else {
                showMessage('Error', response.Message);
                // Jika error, cancel changes di grid biar balik ke nilai awal
                // $(gridSelector).data("kendoGrid").cancelChanges();
            }
            closeProgressOnGrid(gridSelector);
        });
}
function onEditTemplateJurnalDetail117(e) {
    // === LOGIC SAAT ADD NEW RECORD ===
    if (e.model.isNew()) {
        // 1. Logic Auto Number untuk GlUrut
        var grid = e.sender; // Ambil instance grid
        var data = grid.dataSource.data(); // Ambil semua data di grid
        var maxUrut = 0;

        // Loop semua data untuk cari nilai GlUrut tertinggi
        for (var i = 0; i < data.length; i++) {
            // Kita abaikan baris yang sedang kita edit/add ini (cek via uid)
            if (data[i].uid !== e.model.uid) {
                var currentVal = parseInt(data[i].GlUrut) || 0;
                if (currentVal > maxUrut) {
                    maxUrut = currentVal;
                }
            }
        }

        // Set nilai GlUrut baru = (Nilai Tertinggi + 1)
        // e.model.set trigger update tampilan di grid
        e.model.set("GlUrut", maxUrut + 1);
    } 
    // === LOGIC SAAT EDIT RECORD LAMA ===
    else {
        // Readonly GL AKUN jika Edit Mode
        var glAkunField = e.container.find("input[name='GlAkun']");
        glAkunField.attr("readonly", true);
        glAkunField.addClass("k-state-disabled");
    }

    // === LOGIC DROPDOWN D/K (Berlaku untuk Add & Edit) ===
    var dkField = e.container.find("input[name='GlDk']");
    if (dkField.length > 0) {
        dkField.kendoDropDownList({
            autoBind: true,
            dataTextField: "text",
            dataValueField: "value",
            dataSource: [
                { text: "D", value: "D" },
                { text: "K", value: "K" }
            ],
            valuePrimitive: true 
        });
    }

    
}