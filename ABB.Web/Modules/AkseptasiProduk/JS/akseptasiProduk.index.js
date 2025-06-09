$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#AkseptasiProdukGrid");
    });
}

function btnAddAkseptasiProduk() {
    openWindow('#AkseptasiProdukWindow', `/AkseptasiProduk/Add`, 'Add');
}

function btnEditAkseptasiProduk(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#AkseptasiProdukWindow', `/AkseptasiProduk/Edit?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}`, 'Edit');
}

function onAddAkseptasiProduk(){
    showProgress('#AkseptasiProdukWindow');
    onSaveAkseptasiProduk("/AkseptasiProduk/Add");
}

function onEditAkseptasiProduk(){
    showProgress('#AkseptasiProdukWindow');
    onSaveAkseptasiProduk("/AkseptasiProduk/Edit");
}

function onSaveAkseptasiProduk(url){
    var form = getFormData($('#AkseptasiProdukForm'));
    form.Desc_AkseptasiProduk = $("#Desc_AkseptasiProduk").getKendoEditor().value();
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiProdukGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#AkseptasiProdukWindow');
            closeWindow('#AkseptasiProdukWindow');
        }
    );
}

function onDeleteAkseptasiProduk(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#AkseptasiProdukGrid');

            ajaxGet(`/AkseptasiProduk/Delete?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#AkseptasiProdukGrid");
                    closeProgressOnGrid('#AkseptasiProdukGrid');
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