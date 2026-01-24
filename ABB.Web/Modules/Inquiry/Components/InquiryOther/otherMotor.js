$(document).ready(async function () {
    btnNextResikoOtherMotor();
    await setOtherMotorEditedValue();
});

async function setOtherMotorEditedValue(){
    await restoreDropdownValue("#kd_merk_kend", "#temp_kd_merk_kend");
    var flag_hh = $("#tempFlag_hh").val();
    flag_hh == "Y" ? $("#flag_hh").prop("checked", true) : $("#flag_hh").prop("checked", false);
    var flag_aog = $("#tempFlag_aog").val();
    flag_aog == "Y" ? $("#flag_aog").prop("checked", true) : $("#flag_aog").prop("checked", false);
    var flag_banjir = $("#tempFlag_banjir").val();
    flag_banjir == "Y" ? $("#flag_banjir").prop("checked", true) : $("#flag_banjir").prop("checked", false);
    var flag_trs = $("#tempFlag_trs").val();
    flag_trs == "Y" ? $("#flag_trs").prop("checked", true) : $("#flag_trs").prop("checked", false);
}

function dataTipeKendaraanDropDown(){
    return {
        kd_grp_rsk: $("#temp_grp_merk_kend").val()
    }
}

function btnNextResikoOtherMotor(){
    $('#btn-next-inquiryResikoOtherMotor').click(function () {
        $("#resikoOtherTab").getKendoTabStrip().select(1);
    });
}

function OnMerkKendaraanChange(e){
    var value = e.sender._cascadedValue;
    var kd_merk_kend = $("#kd_merk_kend").data("kendoDropDownList");
    kd_merk_kend.dataSource.read({kd_grp_rsk : value});
}
