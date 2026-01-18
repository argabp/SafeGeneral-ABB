$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#LimitKlaimGrid");
    });
}

function btnAddLimitKlaim() {
    openWindow('#LimitKlaimWindow', `/LimitKlaim/Add`, 'Add');
}

function btnEditLimitKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#LimitKlaimWindow', `/LimitKlaim/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&thn=${dataItem.thn}`, 'Edit');
}

function btnAddLimitKlaimDetail(kd_cb, kd_cob, kd_scob, thn){
    openWindow('#LimitKlaimWindow', `/LimitKlaim/AddDetail?kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&thn=${thn}`, 'Add Detail');
}

function btnEditLimitKlaimDetil(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#LimitKlaimWindow', `/LimitKlaim/EditDetil?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&thn=${dataItem.thn}&kd_user=${dataItem.kd_user}`, 'Edit Detail');
}

function onAddLimitKlaim(){
    showProgress('#LimitKlaimWindow');
    onSaveLimitKlaim("/LimitKlaim/Add");
}

function onEditLimitKlaim(){
    showProgress('#LimitKlaimWindow');
    onSaveLimitKlaim("/LimitKlaim/Edit");
}

function onEditLimitKlaimDetail(){
    showProgress('#LimitKlaimWindow');
    onSaveLimitKlaimDetail("/LimitKlaim/EditDetail");
}

function onSaveLimitKlaim(url){
    var form = getFormData($('#LimitKlaimForm'));
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#LimitKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#LimitKlaimWindow');
            closeWindow('#LimitKlaimWindow');
        }
    );
}

function onDeleteLimitKlaim(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#LimitKlaimGrid');
            
            var data = {
                kd_cb: dataItem.kd_cb,
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                thn: dataItem.thn
            }

            ajaxPost("/LimitKlaim/Delete", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#LimitKlaimGrid");
                    closeProgressOnGrid('#LimitKlaimGrid');
                }
            );
        }
    );
}

function onAddLimitKlaimDetail(){
    showProgress('#LimitKlaimWindow');
    onSaveLimitKlaimDetail("/LimitKlaim/AddDetail");
}

function onSaveLimitKlaimDetail(url){
    var form = getFormData($('#LimitKlaimDetailForm'));

    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#LimitKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress('#LimitKlaimWindow');
            closeWindow('#LimitKlaimWindow');
        }
    );
}

function onDeleteLimitKlaimDetail(e){
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
            
            showProgressOnGrid("#LimitKlaimGrid");

            ajaxPost("/LimitKlaim/DeleteDetail", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#LimitKlaimGrid");
                    closeProgressOnGrid("#LimitKlaimGrid");
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