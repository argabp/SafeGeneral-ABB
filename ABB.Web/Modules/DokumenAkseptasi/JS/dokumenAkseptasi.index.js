$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#DokumenAkseptasiGrid");
    });
}

function btnAddDokumenAkseptasi() {
    openWindow('#DokumenAkseptasiWindow', `/DokumenAkseptasi/Add`, 'Add');
}

function btnEditDokumenAkseptasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DokumenAkseptasiWindow', `/DokumenAkseptasi/Edit?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}`, 'Edit');
}

function btnAddDokumenAkseptasiDetail(kd_cob, kd_scob){
    openWindow('#DokumenAkseptasiWindow', `/DokumenAkseptasi/AddDetail?kd_cob=${kd_cob}&kd_scob=${kd_scob}`, 'Add Detail');
}

function btnEditDokumenAkseptasiDetail(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DokumenAkseptasiWindow', `/DokumenAkseptasi/EditDetail?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_dokumen=${dataItem.kd_dokumen}`, 'Edit');
}

function onAddDokumenAkseptasi(){
    showProgress('#DokumenAkseptasiWindow');
    onSaveDokumenAkseptasi("/DokumenAkseptasi/Add");
}

function onEditDokumenAkseptasi(){
    showProgress('#DokumenAkseptasiWindow');
    onSaveDokumenAkseptasi("/DokumenAkseptasi/Edit");
}

function onSaveDokumenAkseptasi(url){
    var form = getFormData($('#DokumenAkseptasiForm'));
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#DokumenAkseptasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#DokumenAkseptasiWindow');
            closeWindow('#DokumenAkseptasiWindow');
        }
    );
}

function onDeleteDokumenAkseptasi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#DokumenAkseptasiGrid');
            
            var data = {
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob
            }

            ajaxPost("/DokumenAkseptasi/Delete", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#DokumenAkseptasiGrid");
                    closeProgressOnGrid('#DokumenAkseptasiGrid');
                }
            );
        }
    );
}

function onAddDokumenAkseptasiDetail(){
    showProgress('#DokumenAkseptasiWindow');
    onSaveDokumenAkseptasiDetail("/DokumenAkseptasi/AddDetail");
}

function onEditDokumenAkseptasiDetail(){
    showProgress('#DokumenAkseptasiWindow');
    onSaveDokumenAkseptasiDetail("/DokumenAkseptasi/EditDetail");
}

function onSaveDokumenAkseptasiDetail(url){
    var form = getFormData($('#DokumenAkseptasiDetailForm'));
    form.flag_wajib = $("#flag_wajib")[0].checked;
    
    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            refreshGrid("#DokumenAkseptasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress('#DokumenAkseptasiWindow');
            closeWindow('#DokumenAkseptasiWindow');
        }
    );
}

function onDeleteDokumenAkseptasiDetail(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_dokumen: dataItem.kd_dokumen
            }
            
            showProgressOnGrid("#DokumenAkseptasiGrid");

            ajaxPost("/DokumenAkseptasi/DeleteDetail", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#DokumenAkseptasiGrid");
                    closeProgressOnGrid("#DokumenAkseptasiGrid");
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