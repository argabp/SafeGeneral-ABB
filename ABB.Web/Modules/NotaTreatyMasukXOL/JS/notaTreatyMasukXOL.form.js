$(document).ready(async function () {
    btnSaveNotaTreatyMasukXOL_Click();
    btnSelectTreatyMasuk();
});

function openTreatyMasukWindow(url, title) {
    openWindow('#TreatyMasukWindow', url, title);
}

function btnSelectTreatyMasuk(){
    $('#btn-select-treatyMasuk').click(function () {
        openTreatyMasukWindow('/NotaTreatyMasukXOL/TreatyMasuk', 'Treaty Masuk')
    });
}

function btnSaveNotaTreatyMasukXOL_Click() {
    $('#btn-save-notaTreatyMasukXOL').click(function () {
        showProgress('#NotaTreatyMasukXOLWindow');
        setTimeout(function () {
            saveNotaTreatyMasukXOL('/NotaTreatyMasukXOL/SaveNotaTreatyMasukXOL')
        }, 500);
    });
}

function saveNotaTreatyMasukXOL(url){
    var form = getFormData($('#NotaTreatyMasukXOLForm'));
    form.flag_closing = $("#flag_closing")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NotaTreatyMasukXOLGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#NotaTreatyMasukXOLWindow")
            }
            else if (response.Result == "ERROR") {
                showMessage('Error', response.Message);
            }
            else {
                $("#NotaTreatyMasukXOLWindow").html(response);
            }

            closeProgress("#NotaTreatyMasukXOLWindow");
        }
    );
}

function OnNilaiPremiChange(e){
    var nilai_prm = e.sender.value();
    var nilai_kms = $("#nilai_kms").data("kendoNumericTextBox").value();
    var nilai_kl = $("#nilai_kl").data("kendoNumericTextBox").value();
    
    $("#due_to_us").getKendoNumericTextBox().value(nilai_prm - nilai_kms - nilai_kl);
}

function OnNilaiKomisiChange(e){
    var nilai_prm = $("#nilai_prm").data("kendoNumericTextBox").value();
    var nilai_kms = e.sender.value();
    var nilai_kl = $("#nilai_kl").data("kendoNumericTextBox").value();

    $("#due_to_us").getKendoNumericTextBox().value(nilai_prm - nilai_kms - nilai_kl);
}

function OnNilaiKlaimChange(e){
    var nilai_prm = $("#nilai_prm").data("kendoNumericTextBox").value();
    var nilai_kms = $("#nilai_kms").data("kendoNumericTextBox").value();
    var nilai_kl = e.sender.value();

    $("#due_to_us").getKendoNumericTextBox().value(nilai_prm - nilai_kms - nilai_kl);
}