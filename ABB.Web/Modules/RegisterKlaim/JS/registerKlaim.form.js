var state;

$(document).ready(function () {
    showProgress('#RegisterKlaimWindow');
    
    setTimeout(() => {
        $("#ket_anev").kendoEditor({
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
        closeProgress('#RegisterKlaimWindow');
    }, 2000)
});


function refreshGridDokumenRegisterKlaim(){
    var kd_cb = $("#kd_cb").val();
    var kd_cob = $("#kd_cob").val();
    var kd_scob = $("#kd_scob").val();
    var kd_thn = $("#kd_thn").val();
    var no_kl = $("#no_kl").val();

    var form = {};

    form.kd_cb = kd_cb;
    form.kd_cob = kd_cob;
    form.kd_scob = kd_scob;
    form.kd_thn = kd_thn;
    form.no_kl = no_kl;

    var data = JSON.stringify(form);

    ajaxPost("/RegisterKlaim/GetDokumenRegisterKlaims", data,
        function (response) {
            $('#dokumenRegisterKlaimDS').val(JSON.stringify(response));
            loadDokumenRegisterKlaimDS();
        }
    );
}
