function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onSaveZona(dataItem){    
    var url = dataItem.model.isNew() ? "/Zona/AddZona" : "/Zona/EditZona";

    var data = {
        kd_zona: dataItem.model.kd_zona,
        nm_zona: dataItem.model.nm_zona
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#ZonaGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteZona(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#ZonaGrid');
            
            var data = {
                kd_zona: dataItem.kd_zona,
            }

            ajaxPost("/Zona/DeleteZona", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#ZonaGrid");
                    closeProgressOnGrid('#ZonaGrid');
                }
            );
        }
    );
}

function onEditDetailZona(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_zona = gridId;
}

function onSaveDetailZona(dataItem){
    var url = "/Zona/SaveDetailZona";

    var data = {
        kd_zona: $("#kd_zona").val(),
        kd_kls_konstr: $("#kd_kls_konstr").val(),
        nm_zona_gb: $("#nm_zona_gb").val(),
        kd_okup: $("#kd_okup").val(),
        stn_rate_prm: parseInt($("#stn_rate_prm").val()),
        pst_rate_prm: $("#pst_rate_prm").val()
    }
    showProgress('#DetailZonaWindow');

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Detail_" + data.kd_zona );
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#DetailZonaWindow');
            closeWindow("#DetailZonaWindow");
        }
    );
}

function onDeleteDetailZona(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_zona: dataItem.kd_zona,
                kd_kls_konstr: dataItem.kd_kls_konstr
            }
            
            showProgressOnGrid("#grid_Detail_" + data.kd_zona);

            ajaxPost("/Zona/DeleteDetailZona", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_Detail_" + data.kd_zona );
                    closeProgressOnGrid("#grid_Detail_" + data.kd_zona);
                }
            );
        }
    );
}


function btnAddDetailZona_OnClick(kd_zona) {
    openWindow('#DetailZonaWindow', `/Zona/AddDetailZonaView?kd_zona=${kd_zona}`, 'Add');
}

function btnEditDetailZona_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DetailZonaWindow', `/Zona/EditDetailZonaView?kd_zona=${dataItem.kd_zona}&kd_kls_konstr=${dataItem.kd_kls_konstr}`, 'Edit');
}