var state;

$(document).ready(function () {
    $('#PengajuanMutasiKlaimTab').kendoTabStrip();

    showProgress('#PengajuanMutasiKlaimWindow');
    
    var tabstrip = $('#PengajuanMutasiKlaimTab').data("kendoTabStrip");
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
        closeProgress('#PengajuanMutasiKlaimWindow');
    }, 2000)
});


function refreshGridLampiranPengajuanMutasiKlaim(){
    var kd_cb = $("#kd_cb").val();
    var kd_cob = $("#kd_cob").val();
    var kd_scob = $("#kd_scob").val();
    var kd_thn = $("#kd_thn").val();
    var no_kl = $("#no_kl").val();
    var no_mts = $("#no_mts").val();

    var form = {};

    form.kd_cb = kd_cb;
    form.kd_cob = kd_cob;
    form.kd_scob = kd_scob;
    form.kd_thn = kd_thn;
    form.no_kl = no_kl;
    form.no_mts = no_mts;

    var data = JSON.stringify(form);

    ajaxPost("/ApprovalMutasiKlaim/GetLampiranPengajuanMutasiKlaim", data,
        function (response) {
            $('#lampiranPengajuanMutasiKlaimDS').val(JSON.stringify(response));
            loadLampiranPengajuanMutasiKlaimDS();
        }
    );
}
