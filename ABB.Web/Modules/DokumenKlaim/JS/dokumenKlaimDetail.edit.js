$(document).ready(function () {
    var flag_wajib = $("#tempFlag_wajib").val();
    
    flag_wajib == "0" ? $("#flag_wajib").prop("checked", false) : $("#flag_wajib").prop("checked", true);
});
