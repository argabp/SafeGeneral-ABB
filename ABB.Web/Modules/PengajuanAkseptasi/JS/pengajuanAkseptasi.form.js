var state;

$(document).ready(function () {
    $('#PengajuanAkseptasiTab').kendoTabStrip();

    showProgress('#PengajuanAkseptasiWindow');
    
    var tabstrip = $('#PengajuanAkseptasiTab').data("kendoTabStrip");
    tabstrip.select(0);
    
    setTimeout(() => {
        $("#ket_rsk").kendoEditor({
            tools: [
                "bold", "italic", "underline",
                "justifyLeft", "justifyCenter", "justifyRight",
                "insertUnorderedList", "insertOrderedList",
                "outdent", "indent",
                "createLink", "unlink",
                "insertImage", "tableWizard",
                "fontName", "fontSize", "foreColor", "backColor"
            ],
            // other configuration as needed
        });
        closeProgress('#PengajuanAkseptasiWindow');
    }, 2000)
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
