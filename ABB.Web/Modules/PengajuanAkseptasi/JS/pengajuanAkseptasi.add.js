
$(document).ready(function () {
    $('#PengajuanAkseptasiTab').kendoTabStrip();

    var tabstrip = $('#PengajuanAkseptasiTab').data("kendoTabStrip");
    tabstrip.disable(tabstrip.items()[1]);

    $("#btn-next-pengajuanAkseptasi").prop("disabled", true);
    
    $("#kd_cb").getKendoDropDownList().value($("#KodeCabang").val());
});