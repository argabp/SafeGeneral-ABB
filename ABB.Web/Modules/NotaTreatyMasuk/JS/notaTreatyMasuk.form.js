$(document).ready(async function () {
    btnSaveNotaTreatyMasuk_Click();
    btnSelectTreatyMasuk();
});

function openTreatyMasukWindow(url, title) {
    openWindow('#TreatyMasukWindow', url, title);
}

function btnSelectTreatyMasuk(){
    $('#btn-select-treatyMasuk').click(function () {
        openTreatyMasukWindow('/NotaTreatyMasuk/TreatyMasuk', 'Treaty Masuk')
    });
}

function btnSaveNotaTreatyMasuk_Click() {
    $('#btn-save-notaTreatyMasuk').click(function () {
        showProgress('#NotaTreatyMasukWindow');
        setTimeout(function () {
            saveNotaTreatyMasuk('/NotaTreatyMasuk/SaveNotaTreatyMasuk')
        }, 500);
    });
}

function saveNotaTreatyMasuk(url){
    var form = getFormData($('#NotaTreatyMasukForm'));
    form.flag_closing = $("#flag_closing")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NotaTreatyMasukGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#NotaTreatyMasukWindow")
            }
            else if (response.Result == "ERROR") {
                showMessage('Error', response.Message);
            }
            else {
                $("#NotaTreatyMasukWindow").html(response);
            }

            closeProgress("#NotaTreatyMasukWindow");
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