function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onSaveOkupasi(dataItem){    
    var url = dataItem.model.isNew() ? "/Okupasi/AddOkupasi" : "/Okupasi/EditOkupasi";

    var data = {
        kd_okup: dataItem.model.kd_okup,
        nm_okup: dataItem.model.nm_okup,
        nm_okup_ing: dataItem.model.nm_okup_ing,
        kd_category: dataItem.model.kd_category
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#OkupasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteOkupasi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#OkupasiGrid');
            
            var data = {
                kd_okup: dataItem.kd_okup,
            }

            ajaxPost("/Okupasi/DeleteOkupasi", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#OkupasiGrid");
                    closeProgressOnGrid('#OkupasiGrid');
                }
            );
        }
    );
}

function onEditDetailOkupasi(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_okup = gridId;
}

function onSaveDetailOkupasi(dataItem){
    var url = "/Okupasi/SaveDetailOkupasi";

    var data = {
        kd_okup: $("#kd_okup").val(),
        kd_kls_konstr: $("#kd_kls_konstr").val(),
        stn_rate_prm: parseInt($("#stn_rate_prm").val()),
        pst_rate_prm: $("#pst_rate_prm").val()
    }
    showProgress('#DetailOkupasiWindow');

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Detail_" + data.kd_okup );
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#DetailOkupasiWindow');
            closeWindow("#DetailOkupasiWindow");
        }
    );
}

function onDeleteDetailOkupasi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_okup: dataItem.kd_okup,
                kd_kls_konstr: dataItem.kd_kls_konstr
            }
            
            showProgressOnGrid("#grid_Detail_" + data.kd_okup);

            ajaxPost("/Okupasi/DeleteDetailOkupasi", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_Detail_" + data.kd_okup );
                    closeProgressOnGrid("#grid_Detail_" + data.kd_okup);
                }
            );
        }
    );
}


function btnAddDetailOkupasi_OnClick(kd_okup) {
    openWindow('#DetailOkupasiWindow', `/Okupasi/AddDetailOkupasiView?kd_okup=${kd_okup}`, 'Add');
}

function btnEditDetailOkupasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DetailOkupasiWindow', `/Okupasi/EditDetailOkupasiView?kd_okup=${dataItem.kd_okup}&kd_kls_konstr=${dataItem.kd_kls_konstr}`, 'Edit');
}