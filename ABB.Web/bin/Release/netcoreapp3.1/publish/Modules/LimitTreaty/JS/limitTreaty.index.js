function onSaveLimitTreaty(){    
    var url = "/LimitTreaty/Save";

    var data = {
        kd_cob: $("#kd_cob").val(),
        kd_tol: $("#kd_tol").val(),
        nm_tol: $("#nm_tol").val(),
        kd_sub_grp: $("#kd_sub_grp").val()
    }
    showProgress('#LimitTreatyWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#LimitTreatyGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#LimitTreatyWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#LimitTreatyWindow").html(response);

            closeProgress('#LimitTreatyWindow');
        }
    );
}

function onDeleteLimitTreaty(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#LimitTreatyGrid');

            ajaxGet(`/LimitTreaty/Delete?kd_cob=${dataItem.kd_cob.trim()}&kd_tol=${dataItem.kd_tol.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#LimitTreatyGrid");
                    closeProgressOnGrid('#LimitTreatyGrid');
                }
            );
        }
    );
}

function btnAddLimitTreaty_OnClick() {
    openWindow('#LimitTreatyWindow', `/LimitTreaty/Add`, 'Add');
}

function btnEditLimitTreaty_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#LimitTreatyWindow', `/LimitTreaty/Edit?kd_cob=${dataItem.kd_cob.trim()}&kd_tol=${dataItem.kd_tol.trim()}`, 'Edit');
}