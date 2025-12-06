function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onDeleteTemplateJurnal62(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#TemplateJurnal62Grid');
            
            var data = {
                Type: dataItem.Type,
                JenisAss: dataItem.JenisAss,
            }

            ajaxPost("/TemplateJurnal62/DeleteTemplateJurnal62", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#TemplateJurnal62Grid");
                    closeProgressOnGrid('#TemplateJurnal62Grid');
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

function onDeleteTemplateJurnalDetail62(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                GlAkun: dataItem.GlAkun
            }
            
            showProgressOnGrid("#grid_detail_" + data.GlAkun);

            ajaxPost("/TemplateJurnal62/DeleteTemplateJurnalDetail62", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_detail_" + data.GlAkun );
                    closeProgressOnGrid("#grid_detail_" + data.GlAkun);
                }
            );
        }
    );
}

function onSaveTemplateJurnalDetail62(dataItem){
    var url = dataItem.model.isNew() ? "/TemplateJurnal62/AddTemplateJurnalDetail62" : "/TemplateJurnal62/EditTemplateJurnalDetail62";

    var data = {
        Type: dataItem.model.Type,
        JenisAss: dataItem.model.JenisAss,
        GlAkun: dataItem.model.GlAkun,
        GlRumus: dataItem.model.GlRumus,
        GlDk: dataItem.model.GlDk,
        GlUrut: dataItem.model.GlUrut,
        FlagDetail: dataItem.model.FlagDetail,
        FlagNt: dataItem.model.FlagNt

    }
    showProgressOnGrid("#grid_detail_" + data.GlAkun);

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_detail_" + data.GlAkun );
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgressOnGrid("#grid_detail_" + data.GlAkun);
        }
    );
}

function onEditTemplateJurnalDetail62(dataItem){
    var gridId = dataItem.GridId
    if(dataItem.model.isNew())
        dataItem.model.GridId = GridId;
}