$(document).ready(function () {
    var tjh = $("#tempTJH").val();
    var rscc = $("#tempRSCC").val();
    var flood = $("#tempBanjir").val();
    var accessories = $("#tempAccessories").val();
    var lainLain01 = $("#tempLainLain01").val();
    var lainLain02 = $("#tempLainLain02").val();

    tjh == 0 ? $("#flag_tjh").prop("checked", false) : $("#flag_tjh").prop("checked", true);
    rscc == 0 ? $("#flag_rscc").prop("checked", false) : $("#flag_rscc").prop("checked", true);
    flood == 0 ? $("#flag_banjir").prop("checked", false) : $("#flag_banjir").prop("checked", true);
    accessories == 0 ? $("#flag_accessories").prop("checked", false) : $("#flag_accessories").prop("checked", true);
    lainLain01 == 0 ? $("#flag_lain_lain01").prop("checked", false) : $("#flag_lain_lain01").prop("checked", true);
    lainLain02 == 0 ? $("#flag_lain_lain02").prop("checked", false) : $("#flag_lain_lain02").prop("checked", true);
});