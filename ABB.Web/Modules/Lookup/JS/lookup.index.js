function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onSaveLookup(dataItem){    
    var url = dataItem.model.isNew() ? "/Lookup/AddLookup" : "/Lookup/EditLookup";

    var data = {
        kd_lookup: dataItem.model.kd_lookup,
        nm_kategori: dataItem.model.nm_kategori
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#LookupGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteLookup(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#LookupGrid');
            
            var data = {
                kd_lookup: dataItem.kd_lookup,
            }

            ajaxPost("/Lookup/DeleteLookup", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#LookupGrid");
                    closeProgressOnGrid('#LookupGrid');
                }
            );
        }
    );
}

function onEditDetailLookup(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_lookup = gridId;
}

function onSaveDetailLookup(dataItem){
    var url = dataItem.model.isNew() ? "/Lookup/AddDetailLookup" : "/Lookup/EditDetailLookup";

    var flag_lookup = dataItem.model.flag_lookup == "true" ? "1" : "0";
    var data = {
        kd_lookup: dataItem.model.kd_lookup,
        no_lookup: dataItem.model.no_lookup,
        nm_detail_lookup: dataItem.model.nm_detail_lookup,
        flag_lookup: flag_lookup
    }
    showProgressOnGrid("#grid_Detail_" + data.kd_lookup);

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Detail_" + data.kd_lookup );
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgressOnGrid("#grid_Detail_" + data.kd_lookup);
        }
    );
}

function onDeleteDetailLookup(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_lookup: dataItem.kd_lookup,
                no_lookup: dataItem.no_lookup
            }
            
            showProgressOnGrid("#grid_Detail_" + data.kd_lookup);

            ajaxPost("/Lookup/DeleteDetailLookup", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_Detail_" + data.kd_lookup );
                    closeProgressOnGrid("#grid_Detail_" + data.kd_lookup);
                }
            );
        }
    );
}