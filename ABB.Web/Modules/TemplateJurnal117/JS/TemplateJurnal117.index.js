function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onDeleteTemplateJurnal117(e) {
    e.preventDefault();
    
    var gridWidget = this;
    var gridId = gridWidget.element.attr("id"); 
    var gridSelector = "#" + gridId; 

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                type_tr: dataItem.type_tr,        
                type_jr: dataItem.type_jr,
                metode: dataItem.metode,
                Event: dataItem.Event,
                jn_ass: dataItem.jn_ass
            };
            
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

function onDeleteTemplateJurnalDetail117(e) {
    e.preventDefault();
    
    var gridWidget = this;
    var gridId = gridWidget.element.attr("id"); 
    var gridSelector = "#" + gridId; 

    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                type_tr: dataItem.type_tr,        
                type_jr: dataItem.type_jr,
                metode: dataItem.metode,
                Event: dataItem.Event,
                jn_ass: dataItem.jn_ass, 
                gl_akun: dataItem.gl_akun
            };
            
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

function onSaveTemplateJurnal117(dataItem) {    
    var url = dataItem.model.isNew() ? "/TemplateJurnal117/AddTemplateJurnal117" : "/TemplateJurnal117/EditTemplateJurnal117";

    var data = {
        type_tr: dataItem.model.type_tr,
        type_jr: dataItem.model.type_jr,
        metode: dataItem.model.metode,
        Event: dataItem.model.Event,
        jn_ass: dataItem.model.jn_ass,
        nm_jr: dataItem.model.nm_jr
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
    var model = e.model;
    var gridElement = e.sender.element;
    var gridSelector = "#" + gridElement.attr("id"); 

    var url = model.isNew() ? "/TemplateJurnal117/AddTemplateJurnalDetail117"
                            : "/TemplateJurnal117/EditTemplateJurnalDetail117";

    var data = {
        type_tr: model.type_tr ? model.type_tr.trim() : "",
        type_jr: model.type_jr ? model.type_jr.trim() : "",
        metode: model.metode ? model.metode.trim() : "",
        Event: model.Event ? model.Event.trim() : "",
        jn_ass: model.jn_ass ? model.jn_ass.trim() : "",
        gl_akun: model.gl_akun, // Sekarang ini Tipe Numerik (short), hindari .trim() langsung
        
        gl_rumus: model.gl_rumus,
        gl_dk: model.gl_dk,
        gl_urut: model.gl_urut,
        flag_detail: model.flag_detail,
        flag_nt: model.flag_nt 
    };

    showProgressOnGrid(gridSelector);

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid(gridSelector);
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid(gridSelector);
        });
}

function onEditTemplateJurnalDetail117(e) {
    // 1. Jika Mode ADD (Data baru)
    if (e.model.isNew()) {
        // Logika Auto Number GlUrut
        var grid = e.sender;
        var data = grid.dataSource.data();
        var maxUrut = 0;

        for (var i = 0; i < data.length; i++) {
            if (data[i].uid !== e.model.uid) {
                var currentVal = parseInt(data[i].gl_urut) || 0;
                if (currentVal > maxUrut) maxUrut = currentVal;
            }
        }
        e.model.set("gl_urut", maxUrut + 1);
        
        // Pastikan gl_akun tidak readonly saat Add
        e.container.find("input[name='gl_akun']").prop("readonly", false);
    } 
    // 2. Jika Mode EDIT (Data lama)
    else {
        // Kunci GlAkun agar tidak bisa diubah (karena dia PK)
        var glAkunField = e.container.find("input[name='gl_akun']");
        glAkunField.attr("readonly", true);
        glAkunField.addClass("k-state-disabled");
    }

    // Logic Dropdown D/K
    var dkField = e.container.find("input[name='gl_dk']");
    if (dkField.length > 0) {
        dkField.kendoDropDownList({
            dataSource: ["D", "K"]
        });
    }
}