$(document).ready(function () {
    setValuePesertaEdit();
    btnNextPeserta();
    setDropDownHidden();
});

function btnNextPeserta(){
    $('#btn-next-peserta').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(1);
    });
}

function setValuePesertaEdit(){
    var almt_ttg_value = $("#almt_ttg_value").val();
    var jns_kelamnin_value = $("#jns_kelamnin_value").val();

    $("#almt_ttg").getKendoTextArea().value(almt_ttg_value);
    if(jns_kelamnin_value === "L")
        $("#LakiLaki").attr("checked",true)
    else if(jns_kelamnin_value === "P")
        $("#Perempuan").attr("checked",true)
}

function setDropDownHidden(){
    var noSumberPenghasilan = $("#no_sumber_penghasilan").getKendoDropDownList().value();
    
    if(noSumberPenghasilan === "-1")
        $("#divPenghasilanLain").show();
    
    var noPekerjaan = $("#no_pekerjaan").getKendoDropDownList().value();

    if(noPekerjaan === "-1")
        $("#divPekerjaanLain").show();
    
    var noJabatan = $("#no_jabatan").getKendoDropDownList().value();

    if(noJabatan === "-1")
        $("#divJabatanLain").show();
}

function onPenghasilanChange(e)
{
    if(e.sender._cascadedValue === "-1")
        $("#divPenghasilanLain").show();
    else
    {
        $("#sumber_penghasilan_lain").val("");
        $("#divPenghasilanLain").hide();
    }
}

function onPekerjaanChange(e)
{
    if(e.sender._cascadedValue === "-1")
        $("#divPekerjaanLain").show();
    else
    {
        $("#pekerjaan_lain").val("");
        $("#divPekerjaanLain").hide();
    }
}

function onJabatanChange(e)
{
    if(e.sender._cascadedValue === "-1")
        $("#divJabatanLain").show();
    else
    {
        $("#jabatan_lain").val("");
        $("#divJabatanLain").hide();
    }
}

$(document).ready(function () {
    setValueEdit();
});

function setValueEdit(){
    var jns_kelamnin_value = $("#jns_kelamnin_value").val();

    if(jns_kelamnin_value === "L")
        $("#LakiLaki").attr("checked",true)
    else if(jns_kelamnin_value === "P")
        $("#Perempuan").attr("checked",true)
}

function setAge(){
    var birthday = $("#tgl_lahir").getKendoDatePicker().value();
    var ageDifMs = Date.now() - birthday.getTime();
    var ageDate = new Date(ageDifMs); // miliseconds from epoch
    var age =  Math.abs(ageDate.getUTCFullYear() - 1970);
    
    $("#usia").getKendoTextBox().value(age);
}