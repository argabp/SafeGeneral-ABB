function onSaveKapal(){    
    var url = "/Kapal/Save";

    var data = {
        kd_kapal: $("#kd_kapal").val(),
        nm_kapal: $("#nm_kapal").val(),
        merk_kapal: $("#merk_kapal").val(),
        kd_negara: $("#kd_negara").val(),
        thn_buat: $("#thn_buat").val(),
        grt: $("#grt").val(),
        st_class: $("#st_class").val(),
        no_reg: $("#no_reg").val(),
        no_imo: $("#no_imo").val(),
        ekuitas: $("#ekuitas").val()
    }
    showProgress('#KapalWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#KapalGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#KapalWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#KapalWindow").html(response);

            closeProgress('#KapalWindow');
        }
    );
}

function onDeleteKapal(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#KapalGrid');

            ajaxGet(`/Kapal/DeleteKapal?kd_kapal=${dataItem.kd_kapal.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#KapalGrid");
                    closeProgressOnGrid('#KapalGrid');
                }
            );
        }
    );
}

function btnAddKapal_OnClick() {
    openWindow('#KapalWindow', `/Kapal/Add`, 'Add');
}

function btnEditKapal_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#KapalWindow', `/Kapal/Edit?kd_kapal=${dataItem.kd_kapal.trim()}`, 'Edit');
}