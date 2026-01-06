$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#DokumenKlaimGrid");
    });
}

function btnAddDokumenKlaim() {
    openWindow('#DokumenKlaimWindow', `/DokumenKlaim/Add`, 'Add');
}

function btnEditDokumenKlaim(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DokumenKlaimWindow', `/DokumenKlaim/Edit?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}`, 'Edit');
}

function btnAddDokumenKlaimDetail(kd_cob, kd_scob){
    openWindow('#DokumenKlaimWindow', `/DokumenKlaim/AddDetail?kd_cob=${kd_cob}&kd_scob=${kd_scob}`, 'Add Detail');
}

function btnEditDokumenKlaimDetail(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DokumenKlaimWindow', `/DokumenKlaim/EditDetail?kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_dokumen=${dataItem.kd_dokumen}`, 'Edit');
}

function onSaveDokumenKlaim(){
    showProgress('#DokumenKlaimWindow');
    var form = getFormData($('#DokumenKlaimForm'));

    var data = JSON.stringify(form);

    ajaxPost("/DokumenKlaim/Save", data,
        function (response) {
            refreshGrid("#DokumenKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress('#DokumenKlaimWindow');
            closeWindow('#DokumenKlaimWindow');
        }
    );
}

function onDeleteDokumenKlaim(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#DokumenKlaimGrid');

            var data = {
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob
            }

            ajaxPost("/DokumenKlaim/Delete", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#DokumenKlaimGrid");
                    closeProgressOnGrid('#DokumenKlaimGrid');
                }
            );
        }
    );
}

function onSaveDokumenKlaimDetail(){
    showProgress('#DokumenKlaimWindow');
    var form = getFormData($('#DokumenKlaimDetailForm'));
    form.flag_wajib = $("#flag_wajib")[0].checked;

    var data = JSON.stringify(form);

    ajaxPost("/DokumenKlaim/SaveDetail", data,
        function (response) {
            refreshGrid("#DokumenKlaimGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgress('#DokumenKlaimWindow');
            closeWindow('#DokumenKlaimWindow');
        }
    );
}

function onDeleteDokumenKlaimDetail(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_cob: dataItem.kd_cob,
                kd_scob: dataItem.kd_scob,
                kd_dokumen: dataItem.kd_dokumen
            }

            showProgressOnGrid("#DokumenKlaimGrid");

            ajaxPost("/DokumenKlaim/DeleteDetail", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#DokumenKlaimGrid");
                    closeProgressOnGrid("#DokumenKlaimGrid");
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