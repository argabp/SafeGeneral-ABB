function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onDeleteTemplateJurnalDetail62(e){
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

            ajaxPost("/TemplateJurnal62/DeleteTemplateJurnalDetail62", JSON.stringify(data),
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

function onSaveTemplateJurnal62(dataItem){    
    var url = dataItem.model.isNew() ? "/TemplateJurnal62/AddTemplateJurnal62" : "/TemplateJurnal62/EditTemplateJurnal62";

    var data = {
        Type: dataItem.model.Type,
        JenisAss: dataItem.model.JenisAss,
        NamaJurnal: dataItem.model.NamaJurnal
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#TemplateJurnal62Grid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}




function onSaveTemplateJurnalDetail62(e) {
    // Agar Kendo tidak menjalankan save bawaan (karena kita pakai manual Ajax)
    // e.preventDefault(); // Opsional, tergantung config datasource

    var model = e.model;
    
    // PERBAIKAN: Ambil ID Grid dari sender event, bukan dari model data
    // e.sender.element adalah elemen <div> grid tersebut
    var gridElement = e.sender.element;
    var gridSelector = "#" + gridElement.attr("id"); 

    var url = model.isNew() ? "/TemplateJurnal62/AddTemplateJurnalDetail62"
                            : "/TemplateJurnal62/EditTemplateJurnalDetail62";

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

function onEditTemplateJurnalDetail62(e) {
    if (!e.model.isNew()) {
        var glAkunField = e.container.find("input[name='GlAkun']");
        
        // 2. Disable atau set Readonly input tersebut
        glAkunField.attr("readonly", true);
        
        // 3. Tambahkan class biar kelihatan abu-abu (visual cue kalau disabled)
        glAkunField.addClass("k-state-disabled");
        
        // Jika inputnya berupa DropDownList atau Numeric, disable juga widget-nya:
        // var widget = glAkunField.data("kendoDropDownList"); // contoh kalau dropdown
        // if(widget) widget.enable(false);
    }
}
