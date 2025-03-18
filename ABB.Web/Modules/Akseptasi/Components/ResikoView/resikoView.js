$(document).ready(function () {
    $('#resikoTab').kendoTabStrip({select: onSelect});

    var tabstrip = $('#resikoTab').data("kendoTabStrip");
    tabstrip.select(0);
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);
    tabstrip.disable(tabstrip.items()[3]);
    tabstrip.disable(tabstrip.items()[4]);

    btnNextAkseptasiResiko();
    btnPreviousAkseptasiResiko();
});

function onSelect(e) {
    if($(e.item).find("> .k-link").text().trim() == "Alokasi")
    {
        showProgress('#AkseptasiWindow');
        
        var data = {
            kd_cb: $("#kd_cb").val().trim(),
            kd_cob: $("#kd_cob").val(),
            kd_scob: $("#kd_scob").val(),
            kd_thn: $("#kd_thn").val(),
            no_pol: $("#no_pol").val(),
            no_updt: $("#no_updt").val(),
            tgl_closing: $("#tgl_closing").val(),
            st_tty: "Y",
            flag_survey: "Y",
        }

        ajaxPost(`/Akseptasi/ProsesAlokasi`, JSON.stringify(data),
            function (response) {
                if (response.Result == "OK") {
                    var nilai_kms_reas = response.Data.split(",")[1];
                    $("#nilai_kms_reas").val(nilai_kms_reas);
                }
                else if (response.Result == "ERROR")
                    showMessage("Error", response.Message);
                else
                    showMessage("Error", response);
                
                closeProgress('#AkseptasiWindow');
            }
        );
    }
}

function btnNextAkseptasiResiko(){
    $('#btn-next-akseptasiResikoView').click(function () {
        $("#akseptasiTab").getKendoTabStrip().select(2);
    });
}
function btnPreviousAkseptasiResiko(){
    $('#btn-previous-akseptasiResikoView').click(function () {
        $("#akseptasiTab").getKendoTabStrip().select(0);
    });
}