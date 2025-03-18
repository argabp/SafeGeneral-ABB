function onSaveRiskAndLossProfile(){    
    var url = "/RiskAndLossProfile/Save";

    var data = {
        kd_cob: $("#kd_cob").val(),
        nomor: $("#nomor").val(),
        bts1: $("#bts1").val(),
        bts2: $("#bts2").val()
    }
    showProgress('#RiskAndLossProfileWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#RiskAndLossProfileGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#RiskAndLossProfileWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#RiskAndLossProfileWindow").html(response);

            closeProgress('#RiskAndLossProfileWindow');
        }
    );
}

function onDeleteRiskAndLossProfile(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#RiskAndLossProfileGrid');

            ajaxGet(`/RiskAndLossProfile/Delete?kd_cob=${dataItem.kd_cob.trim()}&nomor=${dataItem.nomor}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#RiskAndLossProfileGrid");
                    closeProgressOnGrid('#RiskAndLossProfileGrid');
                }
            );
        }
    );
}

function btnAddRiskAndLossProfile_OnClick() {
    openWindow('#RiskAndLossProfileWindow', `/RiskAndLossProfile/Add`, 'Add');
}

function btnEditRiskAndLossProfile_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#RiskAndLossProfileWindow', `/RiskAndLossProfile/Edit?kd_cob=${dataItem.kd_cob.trim()}&nomor=${dataItem.nomor}`, 'Edit');
}