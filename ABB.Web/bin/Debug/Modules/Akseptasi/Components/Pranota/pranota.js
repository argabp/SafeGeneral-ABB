$(document).ready(function () {
    btnNextPranota();
    btnSaveAkseptasiPranota_Click();
    setPranotaEditedValue();
});

function btnNextPranota(){
    $('#btn-next-akseptasiPranota').click(function () {
        $("#pranotaTab").getKendoTabStrip().select(1);
    });
}


function btnSaveAkseptasiPranota_Click() {
    $('#btn-save-akseptasiPranota').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasiPranota('/Akseptasi/SaveAkseptasiPranota')
        }, 500);
    });
}

function saveAkseptasiPranota(url) {
    var form = getFormData($('#PranotaForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#pranota_no_updt").val();

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                $("#kd_mtu").getKendoDropDownList().readonly(true);
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiWindow');
        }
    );
}
function setPranotaEditedValue()
{
    if($("#IsEditPranota").val() === 'True')
    {
        $("#kd_mtu").getKendoDropDownList().readonly(true);
    }
}