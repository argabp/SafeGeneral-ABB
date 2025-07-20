$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#LimitAkseptasiGrid");
    });
}

function btnAddLimitAkseptasi() {
    openWindow('#LimitAkseptasiWindow', `/LimitAkseptasi/Add`, 'Add');
}

function btnEditLimitAkseptasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#LimitAkseptasiWindow', `/LimitAkseptasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&thn=${dataItem.thn}`, 'Edit');
}

function btnAddLimitAkseptasiDetail(kd_cb, kd_cob, kd_scob, thn){
    openWindow('#LimitAkseptasiWindow', `/LimitAkseptasi/AddDetail?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&thn=${thn}`, 'Add Detail');
}

function btnEditLimitAkseptasiDetil(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#LimitAkseptasiWindow', `/LimitAkseptasi/EditDetil?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&thn=${dataItem.thn}&kd_user=${dataItem.kd_user}`, 'Edit Detail');
}

function onAddLimitAkseptasi(){
    showProgress('#LimitAkseptasiWindow');
    onSaveLimitAkseptasi("/LimitAkseptasi/Add");
}

function onEditLimitAkseptasi(){
    showProgress('#LimitAkseptasiWindow');
    onSaveLimitAkseptasi("/LimitAkseptasi/Edit");
}

function onEditLimitAkseptasiDetail(){
    showProgress('#LimitAkseptasiWindow');
    onSaveLimitAkseptasiDetail("/LimitAkseptasi/EditDetail");
}

function onSaveLimitAkseptasi(url){
    var form = getFormData($('#LimitAkseptasiForm'));
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#LimitAkseptasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#LimitAkseptasiWindow');
            closeWindow('#LimitAkseptasiWindow');
        }
    );
}

function onDeleteLimitAkseptasi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#LimitAkseptasiGrid');
            
            var data = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                thn: dataItem.thn
            }

            ajaxPost("/LimitAkseptasi/Delete", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#LimitAkseptasiGrid");
                    closeProgressOnGrid('#LimitAkseptasiGrid');
                }
            );
        }
    );
}

function onAddLimitAkseptasiDetail(){
    showProgress('#LimitAkseptasiWindow');
    onSaveLimitAkseptasiDetail("/LimitAkseptasi/AddDetail");
}

function onSaveLimitAkseptasiDetail(url){
    var form = getFormData($('#LimitAkseptasiDetailForm'));

    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#LimitAkseptasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress('#LimitAkseptasiWindow');
            closeWindow('#LimitAkseptasiWindow');
        }
    );
}

function onDeleteLimitAkseptasiDetail(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                thn: dataItem.thn,
                kd_user: dataItem.kd_user
            }
            
            showProgressOnGrid("#LimitAkseptasiGrid");

            ajaxPost("/LimitAkseptasi/DeleteDetail", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#LimitAkseptasiGrid");
                    closeProgressOnGrid("#LimitAkseptasiGrid");
                }
            );
        }
    );
}

function OnKodeCOBChange(e){
    var value = e.sender._cascadedValue;
    $("#temp_kd_cob").val(value);
    var kd_scob = $("#kd_scob").data("kendoDropDownList");
    kd_scob.dataSource.read({kd_cob : e.sender._cascadedValue});
}

function dataKodeSCOBDropDown(){
    return {
        kd_cob: $("#temp_kd_cob").val().trim()
    }
}