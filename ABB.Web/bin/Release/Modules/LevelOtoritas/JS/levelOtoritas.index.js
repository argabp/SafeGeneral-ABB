function onSaveLevelOtoritas(){    
    var url = "/LevelOtoritas/Save";

    var data = {
        kd_user: $("#kd_user").val(),
        kd_pass: $("#kd_pass").val(),
        flag_xol: $("#flag_xol").val()
    }
    showProgress('#LevelOtoritasWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#LevelOtoritasGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#LevelOtoritasWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#LevelOtoritasWindow").html(response);

            closeProgress('#LevelOtoritasWindow');
        }
    );
}

function onDeleteLevelOtoritas(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#LevelOtoritasGrid');

            ajaxGet(`/LevelOtoritas/Delete?kd_user=${dataItem.kd_user.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#LevelOtoritasGrid");
                    closeProgressOnGrid('#LevelOtoritasGrid');
                }
            );
        }
    );
}

function btnAddLevelOtoritas_OnClick() {
    openWindow('#LevelOtoritasWindow', `/LevelOtoritas/Add`, 'Add');
}

function btnEditLevelOtoritas_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#LevelOtoritasWindow', `/LevelOtoritas/Edit?kd_user=${dataItem.kd_user.trim()}`, 'Edit');
}