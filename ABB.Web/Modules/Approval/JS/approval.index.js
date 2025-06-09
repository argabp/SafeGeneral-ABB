$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ApprovalGrid");
    });
}

function btnAddApproval() {
    openWindow('#ApprovalWindow', `/Approval/Add`, 'Add');
}

function btnEditApproval(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ApprovalWindow', `/Approval/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}`, 'Edit');
}

function btnAddApprovalDetail(kd_cb, kd_cob, kd_scob){
    openWindow('#ApprovalWindow', `/Approval/AddDetail?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}`, 'Add Detail');
}

function btnEditApprovalDetail(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#ApprovalWindow', `/Approval/EditDetail?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_status=${dataItem.kd_status}&nilai_limit_awal=${dataItem.nilai_limit_awal}&nilai_limit_akhir=${dataItem.nilai_limit_akhir}`, 'Edit');
}

function onAddApproval(){
    showProgress('#ApprovalWindow');
    onSaveApproval("/Approval/Add");
}

function onEditApproval(){
    showProgress('#ApprovalWindow');
    onSaveApproval("/Approval/Edit");
}

function onSaveApproval(url){
    var form = getFormData($('#ApprovalForm'));
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#ApprovalGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#ApprovalWindow');
            closeWindow('#ApprovalWindow');
        }
    );
}

function onDeleteApproval(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#ApprovalGrid');
            
            var data = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob
            }

            ajaxPost("/Approval/Delete", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#ApprovalGrid");
                    closeProgressOnGrid('#ApprovalGrid');
                }
            );
        }
    );
}

function onAddApprovalDetail(){
    showProgress('#ApprovalWindow');
    onSaveApprovalDetail("/Approval/AddDetail");
}

function onEditApprovalDetail(){
    showProgress('#ApprovalWindow');
    onSaveApprovalDetail("/Approval/EditDetail");
}

function onSaveApprovalDetail(url){
    var form = getFormData($('#ApprovalDetailForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            refreshGrid("#ApprovalGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress('#ApprovalWindow');
            closeWindow('#ApprovalWindow');
        }
    );
}

function onDeleteApprovalDetail(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_status: dataItem.kd_status,
                nilai_limit_awal: dataItem.nilai_limit_awal,
                nilai_limit_akhir: dataItem.nilai_limit_akhir
            }
            
            showProgressOnGrid("#ApprovalGrid");

            ajaxPost("/Approval/DeleteDetail", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#ApprovalGrid");
                    closeProgressOnGrid("#ApprovalGrid");
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