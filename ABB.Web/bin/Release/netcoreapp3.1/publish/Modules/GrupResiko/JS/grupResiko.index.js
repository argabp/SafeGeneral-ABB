function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onSaveGrupResiko(dataItem){    
    var url = dataItem.model.isNew() ? "/GrupResiko/AddGrupResiko" : "/GrupResiko/EditGrupResiko";

    var data = {
        kd_grp_rsk: dataItem.model.kd_grp_rsk,
        desk_grp_rsk: dataItem.model.desk_grp_rsk,
        kd_jns_grp: dataItem.model.kd_jns_grp
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#GrupResikoGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteGrupResiko(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#GrupResikoGrid');
            
            var data = {
                kd_grp_rsk: dataItem.kd_grp_rsk,
            }

            ajaxPost("/GrupResiko/DeleteGrupResiko", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#GrupResikoGrid");
                    closeProgressOnGrid('#GrupResikoGrid');
                }
            );
        }
    );
}

function onEditDetailGrupResiko(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_grp_rsk = gridId;
}

function onSaveDetailGrupResiko(dataItem){
    var url = dataItem.model.isNew() ? "/GrupResiko/AddDetailGrupResiko" : "/GrupResiko/EditDetailGrupResiko";

    var data = {
        kd_grp_rsk: dataItem.model.kd_grp_rsk,
        kd_rsk: dataItem.model.kd_rsk,
        desk_rsk: dataItem.model.desk_rsk,
        kd_ref: dataItem.model.kd_ref,
        kd_ref1: dataItem.model.kd_ref1
    }
    showProgressOnGrid("#grid_Detail_" + data.kd_grp_rsk);

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Detail_" + data.kd_grp_rsk );
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgressOnGrid("#grid_Detail_" + data.kd_grp_rsk);
        }
    );
}

function onDeleteDetailGrupResiko(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_grp_rsk: dataItem.kd_grp_rsk,
                kd_rsk: dataItem.kd_rsk,
                desk_rsk: dataItem.desk_rsk,
                kd_ref: dataItem.kd_ref,
                kd_ref1: dataItem.kd_ref1
            }
            
            showProgressOnGrid("#grid_Detail_" + data.kd_grp_rsk);

            ajaxPost("/GrupResiko/DeleteDetailGrupResiko", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_Detail_" + data.kd_grp_rsk );
                    closeProgressOnGrid("#grid_Detail_" + data.kd_grp_rsk);
                }
            );
        }
    );
}