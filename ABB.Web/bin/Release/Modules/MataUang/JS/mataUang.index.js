function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onSaveMataUang(dataItem){    
    var url = dataItem.model.isNew() ? "/MataUang/AddMataUang" : "/MataUang/EditMataUang";

    var data = {
        kd_mtu: dataItem.model.kd_mtu,
        nm_mtu: dataItem.model.nm_mtu,
        symbol: dataItem.model.symbol
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#MataUangGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteMataUang(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#MataUangGrid');
            
            var data = {
                kd_mtu: dataItem.kd_mtu,
            }

            ajaxPost("/MataUang/DeleteMataUang", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#MataUangGrid");
                    closeProgressOnGrid('#MataUangGrid');
                }
            );
        }
    );
}

function onEditDetailMataUang(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_mtu = gridId;
}

function onSaveDetailMataUang(dataItem){
    var url = dataItem.model.isNew() ? "/MataUang/AddDetailMataUang" : "/MataUang/EditDetailMataUang";

    var data = {
        kd_mtu: dataItem.model.kd_mtu,
        tgl_mul: dataItem.model.tgl_mul.toDateString(),
        tgl_akh: dataItem.model.tgl_akh.toDateString(),
        nilai_kurs: dataItem.model.nilai_kurs
    }
    showProgressOnGrid("#grid_Periode_" + data.kd_mtu);

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Periode_" + data.kd_mtu );
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgressOnGrid("#grid_Periode_" + data.kd_mtu);
        }
    );
}

function onDeleteDetailMataUang(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_mtu: dataItem.kd_mtu,
                tgl_mul: dataItem.tgl_mul.toDateString(),
                tgl_akh: dataItem.tgl_akh.toDateString()
            }
            
            showProgressOnGrid("#grid_Periode_" + data.kd_mtu);

            ajaxPost("/MataUang/DeleteDetailMataUang", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_Periode_" + data.kd_mtu );
                    closeProgressOnGrid("#grid_Periode_" + data.kd_mtu);
                }
            );
        }
    );
}