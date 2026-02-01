$(document).ready(function () {
    btnSaveAkseptasiOtherCargoDetail_Click();
});

function btnSaveAkseptasiOtherCargoDetail_Click() {
    $('#btn-save-akseptasiOtherCargoDetail').click(function () {
        showProgress('#AkseptasiOtherCargoDetailWindow');
        setTimeout(function () {
            saveAkseptasiOtherCargoDetail('/Akseptasi/SaveAkseptasiOtherCargoDetail')
        }, 500);
    });
}

function saveAkseptasiOtherCargoDetail(url) {
    var form = getFormData($('#OtherCargoDetailForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = resiko.no_updt;
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = resiko.kd_endt;
    form.no_urut = $("#resiko_other_cargo_no_urut").val();
    form.no_bl = $("#resiko_other_cargo_detail_no_bl").val();
    form.no_inv = $("#resiko_other_cargo_detail_no_inv").val();
    form.no_po = $("#resiko_other_cargo_detail_no_po").val();
    form.no_pol_ttg = $("#no_pol_ttg").val();
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiOtherCargoDetailGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AkseptasiOtherCargoDetailWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiOtherCargoDetailWindow');
        }
    );
}

function OnJenisAngkutChange(e){
    switch(e.sender._cascadedValue){
        case "01":
            $("#label_nm_angkut").text("No. Alat Angkut");
            $("#label_no_bl").text("No. Surat Jalan/DO");
            break;
        case "02":
            $("#label_nm_angkut").text("Nama Kapal");
            $("#label_no_bl").text("No. B/L");
            break;
        case "03":
            $("#label_nm_angkut").text("No. Penerbangan");
            $("#label_no_bl").text("No. AWB");
            break;
    }
}

function OnKodeKapalChange(e){
    ajaxGet(`/Akseptasi/GenerateNamaAngkut?kd_angkut=${e.sender._cascadedValue}`, (returnValue) => {
        $("#nm_angkut").getKendoTextBox().value(returnValue.split(",")[1]);
    });
}