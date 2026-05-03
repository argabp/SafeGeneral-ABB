$(document).ready(async function () {
    await setProsesPremiXOLKeluarEditedValue();
});

async function setProsesPremiXOLKeluarEditedValue(){
    var flag_closing = $("#tempFlag_closing").val();
    flag_closing == "Y" ? $("#flag_closing").prop("checked", true) : $("#flag_closing").prop("checked", false);
}