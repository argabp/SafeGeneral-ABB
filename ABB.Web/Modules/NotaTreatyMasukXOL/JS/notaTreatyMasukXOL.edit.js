$(document).ready(async function () {
    await setNotaTreatyMasukXOLEditedValue();
});

async function setNotaTreatyMasukXOLEditedValue(){
    var flag_closing = $("#tempFlag_closing").val();
    flag_closing == "Y" ? $("#flag_closing").prop("checked", true) : $("#flag_closing").prop("checked", false);
}