$(document).ready(async function () {
    btnPreviousOther();
    await setOtherFireEditedValue();
});

async function setOtherFireEditedValue(){
    await restoreDropdownValue("#kd_kab", "#temp_kd_kab");
    await restoreDropdownValue("#kd_kec", "#temp_kd_kec");
    await restoreDropdownValue("#kd_kel", "#temp_kd_kel");
}

function dataKodeKabupatenDropDown(){
    return {
        kd_prop: $("#temp_kd_prop").val()
    }
}

function dataKodeKecamatanDropDown(){
    return {
        kd_prop: $("#temp_kd_prop").val(),
        kd_kab: $("#temp_kd_kab").val()
    }
}

function dataKodeKelurahanDropDown(){
    return {
        kd_prop: $("#temp_kd_prop").val(),
        kd_kab: $("#temp_kd_kab").val(),
        kd_kec: $("#temp_kd_kec").val()
    }
}

function btnPreviousOther(){
    $('#btn-previous-inquiryResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function OnKodePropinsiChange(e){
    var value = e.sender._cascadedValue;
    var kd_kab = $("#kd_kab").data("kendoDropDownList");
    kd_kab.dataSource.read({kd_prop : value});
}

function OnKodeKabupatenChange(e){
    var value = e.sender._cascadedValue;
    var kd_kec = $("#kd_kec").data("kendoDropDownList");
    kd_kec.dataSource.read({kd_prop: $("#kd_prop").val(), kd_kab: value});
}

function OnKodeKecamatanChange(e){
    var value = e.sender._cascadedValue;
    var kd_kel = $("#kd_kel").data("kendoDropDownList");
    kd_kel.dataSource.read({kd_prop: $("#kd_prop").val(), kd_kab: $("#kd_kab").val(), kd_kec: value });
}
