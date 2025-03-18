$(document).ready(function () {
    setValueInfoKesehatanEdit();
    btnPreviousPesertaInfoKesehatan();
    btnNextPesertaInfoKesehatan();
});

function btnPreviousPesertaInfoKesehatan(){
    $('#btn-previous-pesertaInfoKesehatan').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(1);
    });
}

function btnNextPesertaInfoKesehatan(){
    $('#btn-next-pesertaInfoKesehatan').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(3);
    });
}

function setValueInfoKesehatanEdit(){
    var flag_tanya_01_value = $("#flag_tanya_01_value").val();
    var flag_tanya_02_value = $("#flag_tanya_02_value").val();
    var flag_tanya_03_value = $("#flag_tanya_03_value").val();
    var flag_tanya_04_value = $("#flag_tanya_04_value").val();
    var flag_tanya_05_value = $("#flag_tanya_05_value").val();
    var flag_tanya_06_value = $("#flag_tanya_06_value").val();
    var flag_tanya_07_value = $("#flag_tanya_07_value").val();
    var flag_tanya_08_value = $("#flag_tanya_08_value").val();
    var flag_tanya_09_value = $("#flag_tanya_09_value").val();

    if(flag_tanya_01_value === "Y")
        $("#flag_tanya_01_ya").attr("checked",true)
    else if(flag_tanya_01_value === "N")
        $("#flag_tanya_01_tidak").attr("checked",true)

    if(flag_tanya_02_value === "Y")
        $("#flag_tanya_02_ya").attr("checked",true)
    else if(flag_tanya_02_value === "N")
        $("#flag_tanya_02_tidak").attr("checked",true)

    if(flag_tanya_03_value === "Y")
        $("#flag_tanya_03_ya").attr("checked",true)
    else if(flag_tanya_03_value === "N")
        $("#flag_tanya_03_tidak").attr("checked",true)

    if(flag_tanya_04_value === "Y")
        $("#flag_tanya_04_ya").attr("checked",true)
    else if(flag_tanya_04_value === "N")
        $("#flag_tanya_04_tidak").attr("checked",true)

    if(flag_tanya_05_value === "Y")
        $("#flag_tanya_05_ya").attr("checked",true)
    else if(flag_tanya_05_value === "N")
        $("#flag_tanya_05_tidak").attr("checked",true)

    if(flag_tanya_06_value === "Y")
        $("#flag_tanya_06_ya").attr("checked",true)
    else if(flag_tanya_06_value === "N")
        $("#flag_tanya_06_tidak").attr("checked",true)

    if(flag_tanya_07_value === "Y")
        $("#flag_tanya_07_ya").attr("checked",true)
    else if(flag_tanya_07_value === "N")
        $("#flag_tanya_07_tidak").attr("checked",true)

    if(flag_tanya_08_value === "Y")
        $("#flag_tanya_08_ya").attr("checked",true)
    else if(flag_tanya_08_value === "N")
        $("#flag_tanya_08_tidak").attr("checked",true)

    if(flag_tanya_09_value === "Y")
        $("#flag_tanya_09_ya").attr("checked",true)
    else if(flag_tanya_09_value === "N")
        $("#flag_tanya_09_tidak").attr("checked",true)

}