$(document).ready(function () {
    btnAddNotaDinasDetail();
    btnDeleteNotaDinasDetail();
});

function onKantorChange(e){
    loadKreditur();
}

function onKantorDataBound(e){
    loadKreditur();
}

function loadKreditur()
{
    var kd_cb = $("#kd_cb").val();
    var kd_rk = $("#kd_rk").data("kendoDropDownList");
    kd_rk.dataSource.read({kd_cb : kd_cb});        
}

function dataNotaDinas()
{
    return {
        id_nds: $("#id_nds").val()
    }
}

function saveNotaDinas(url) {
    var form = getFormData($('#NotaDinasForm'));
    var data = JSON.stringify(form);
    ajaxPost(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#NotaDinasGrid");

                if(response.Model != undefined)
                    $("#id_nds").val(response.Model.id_nds);
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#NotaDinasWindow").html(response);

            closeProgress('#NotaDinasWindow');
        }
    );
}

function btnAddNotaDinasDetail(){
    $('#btn-add-notaDinasDetail').click(function () {
        showProgressOnGrid('#NotaDinasDetailGrid');
        setTimeout(function () {
            var id_nds = $("#id_nds").val();
            var kd_cb = $("#kd_cb").val();
            var kd_rk = $("#kd_rk").val();

            addNotaDinasDetail(`/PengajuanNotaDinas/AddNotaDinasDetail?id_nds=${id_nds}&kd_cb=${kd_cb}&kd_rk=${kd_rk}`);
        }, 500);
    });
}

function addNotaDinasDetail(url) {
    ajaxGet(url,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Model.Message);
                refreshGrid("#NotaDinasDetailGrid");
                refreshGrid("#NotaDinasGrid");

                reloadDataNotaDinasForm(response.Model);
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Model.Message);
            else
                $("#NotaDinasWindow").html(response);

            closeProgressOnGrid('#NotaDinasDetailGrid');
        }
    );
}

function btnDeleteNotaDinasDetail(){
    $('#btn-delete-notaDinasDetail').click(function () {
        showProgressOnGrid('#NotaDinasDetailGrid');
        setTimeout(function () {
            var id_nds = $("#id_nds").val();

            deleteNotaDinasDetail(`/PengajuanNotaDinas/DeleteNotaDinasDetail?id_nds=${id_nds}`);
        }, 500);
    });
}

function deleteNotaDinasDetail(url) {
    ajaxGet(url,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Model.Message);
                refreshGrid("#NotaDinasDetailGrid");
                refreshGrid("#NotaDinasGrid");

                reloadDataNotaDinasForm(response.Model);
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Model.Message);
            else
                $("#NotaDinasWindow").html(response);

            closeProgressOnGrid('#NotaDinasDetailGrid');
        }
    );
}

function reloadDataNotaDinasForm(dataModel){
    $("#jml_peserta").getKendoNumericTextBox().value(dataModel.jml_peserta);
    $("#prm_bruto").getKendoNumericTextBox().value(dataModel.prm_bruto);
    $("#kms").getKendoNumericTextBox().value(dataModel.kms);
    $("#pst_kms").getKendoNumericTextBox().value(dataModel.pst_kms);
    $("#pph").getKendoNumericTextBox().value(dataModel.pph);
    $("#pst_pph").getKendoNumericTextBox().value(dataModel.pst_pph);
    $("#prm_netto").getKendoNumericTextBox().value(dataModel.prm_netto);
    $("#nomor_polis").getKendoTextBox().value(dataModel.nomor_polis);
}

function onTglNdsChange(e){
    showProgress("#NotaDinasWindow");
    var tgl_nds= $("#tgl_nds").val();

    ajaxGet(`/PengajuanNotaDinas/GetPerihal?tgl_nds=${tgl_nds}`,
        function (response) {
            if (response.Result == "OK") {
                $("#Perihal").getKendoTextArea().value(response.Value)
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#NotaDinasWindow").html(response);

            closeProgress('#NotaDinasWindow');
        }
    );
}