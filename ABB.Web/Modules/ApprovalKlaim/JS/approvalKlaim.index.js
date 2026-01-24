$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ApprovalKlaimGrid");
    });
}

function btnAddApprovalKlaim() {
    openWindow('#ApprovalKlaimWindow', `/ApprovalKlaim/Add`, 'Add');
}

function btnEditApprovalKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ApprovalKlaimWindow', `/ApprovalKlaim/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}`, 'Edit');
}

function btnAddApprovalDetailKlaim(kd_cb, kd_cob, kd_scob){
    openWindow('#ApprovalKlaimWindow', `/ApprovalKlaim/AddDetail?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}`, 'Add Detail');
}

function btnEditApprovalKlaimDetail(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ApprovalKlaimWindow', `/ApprovalKlaim/EditDetail?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_status=${dataItem.kd_status}&kd_user=${dataItem.kd_user}&kd_user_sign=${dataItem.kd_user_sign}`, 'Edit');
}

function onAddApprovalKlaim(){
    showProgress('#ApprovalKlaimWindow');
    onSaveApprovalKlaim("/ApprovalKlaim/Add");
}

function onEditApprovalKlaim(){
    showProgress('#ApprovalKlaimWindow');
    onSaveApprovalKlaim("/ApprovalKlaim/Edit");
}

function onSaveApprovalKlaim(url){
    var form = getFormData($('#ApprovalKlaimForm'));
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#ApprovalKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#ApprovalKlaimWindow');
            closeWindow('#ApprovalKlaimWindow');
        }
    );
}

function onDeleteApprovalKlaim(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#ApprovalKlaimGrid');
            
            var data = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob
            }

            ajaxPost("/ApprovalKlaim/Delete", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#ApprovalKlaimGrid");
                    closeProgressOnGrid('#ApprovalKlaimGrid');
                }
            );
        }
    );
}

function onAddApprovalKlaimDetail(){
    showProgress('#ApprovalKlaimWindow');
    onSaveApprovalKlaimDetail("/ApprovalKlaim/AddDetail");
}

function onEditApprovalKlaimDetail(){
    showProgress('#ApprovalKlaimWindow');
    onSaveApprovalKlaimDetail("/ApprovalKlaim/EditDetail");
}

function onSaveApprovalKlaimDetail(url){
    var form = getFormData($('#ApprovalKlaimDetailForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            refreshGrid("#ApprovalKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress('#ApprovalKlaimWindow');
            closeWindow('#ApprovalKlaimWindow');
        }
    );
}

function onDeleteApprovalKlaimDetail(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_status: dataItem.kd_status,
                kd_user: dataItem.kd_user,
                kd_user_sign: dataItem.kd_user_sign
            }
            
            showProgressOnGrid("#ApprovalKlaimGrid");

            ajaxPost("/ApprovalKlaim/DeleteDetail", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#ApprovalKlaimGrid");
                    closeProgressOnGrid("#ApprovalKlaimGrid");
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