var state;

$(document).ready(function () {
    $('#PengajuanAkseptasiTab').kendoTabStrip();

    var tabstrip = $('#PengajuanAkseptasiTab').data("kendoTabStrip");
    tabstrip.select(0);
});


function refreshGridLampiranPengajuanAkseptasi(){
    var kd_cb = $("#kd_cb").val();
    var kd_cob = $("#kd_cob").val();
    var kd_scob = $("#kd_scob").val();
    var kd_thn = $("#kd_thn").val();
    var no_aks = $("#no_aks").val();

    var form = {};

    form.kd_cb = kd_cb;
    form.kd_cob = kd_cob;
    form.kd_scob = kd_scob;
    form.kd_thn = kd_thn;
    form.no_aks = no_aks;

    var data = JSON.stringify(form);

    ajaxPost("/PengajuanAkseptasi/GetLampiranPengajuanAkseptasi", data,
        function (response) {
            $('#lampiranPengajuanAkseptasiDS').val(JSON.stringify(response));
            loadLampiranPengajuanAkseptasiDS();
        }
    );
}
