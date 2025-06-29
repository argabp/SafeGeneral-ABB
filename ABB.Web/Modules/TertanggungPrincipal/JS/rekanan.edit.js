$(document).ready(function () {
    setTimeout(setRekananEditedValue, 1000);
});

function setRekananEditedValue(){
    var no_fax = $("#tempNo_fax").val();
    no_fax == "Y" ? $("#no_fax_rekanan").prop("checked", true) : $("#no_fax").prop("checked", false);
}
