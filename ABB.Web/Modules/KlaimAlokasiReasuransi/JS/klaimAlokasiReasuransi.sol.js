$(document).ready(function () {
    btnSaveKlaimAlokasiReasuransi_Click();
});

function btnSaveKlaimAlokasiReasuransi_Click() {
    $('#btn-save-klaimAlokasiReasuransi').click(function () {
        showProgress('#KlaimAlokasiReasuransiWindow');
        setTimeout(function () {
            saveKlaimAlokasiReasuransi('/KlaimAlokasiReasuransi/SaveSOL')
        }, 500);
    });
}

function saveKlaimAlokasiReasuransi(url) {
    var form = getFormData($('#KlaimAlokasiReasuransiForm'));
    form.flag_nota = $("#flag_nota")[0].checked ? "Y" : "N";
    form.flag_cash_call = $("#flag_cash_call")[0].checked ? "Y" : "N";
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            
            var parentId =
                form.kd_cb.trim() + "-" +
                form.kd_cob.trim() + "-" +
                form.kd_scob.trim() + "-" +
                form.kd_thn.trim() + "-" +
                form.no_kl.trim() + "-" +
                form.no_mts.trim();

            refreshGrid('#grid_sol_' + parentId);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#KlaimAlokasiReasuransiWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#KlaimAlokasiReasuransiWindow');
        }
    );
}

function OnKodeJenisSorChange(e){
    var kd_jns_sor = e.sender.value();

    ajaxGet(`/KlaimAlokasiReasuransi/GetGroupAndRekananSor?kd_jns_sor=${kd_jns_sor}`,
        function (response) {
            if (response.Status == "OK") {
                var kd_grp_sor = response.Data.split(",")[1];
                $("#kd_grp_sor").val(kd_grp_sor);
                var kd_rk_sor = response.Data.split(",")[4];

                var kd_rk_sor_dp = $("#kd_rk_sor").data("kendoDropDownList");
                kd_rk_sor_dp.dataSource.read({jns_lookup : kd_grp_sor + ",R", kd_cb: $("#kd_cb").val(), kd_cob: $("#kd_cob").val(), kd_jns_sor : $("#kd_jns_sor").val()});

                kd_rk_sor_dp.value(kd_rk_sor);
            }
            else if (response.Status == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);
        }
    );
}

function dataRekananSorDropDown(){
    return {
        jns_lookup: $("#kd_cb").val().trim() + ",R",
        kd_cb: $("#kd_cb").val().trim(),
        kd_cob: $("#kd_cob").val().trim(),
        kd_jns_sor: $("#kd_jns_sor").val().trim()        
    }
}

function OnShareChange(e){
    var pst_share = e.sender.value();
    var nilai_ttl_kl = $("#nilai_ttl_kl").val();
    
    ajaxGet(`/KlaimAlokasiReasuransi/GenerateNilaiKlaim?pst_share=${pst_share}&nilai_ttl_kl=${nilai_ttl_kl}`,
        function (response) {
            if (response.Status == "OK") {
                var nilai_kl = response.Data.split(",")[1];

                $("#nilai_kl").data("kendoNumericTextBox").value(nilai_kl);
            }
            else if (response.Status == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);
        }
    );
}

function OnFlagNotaChange(element) {
    var isChecked = $(element).is(':checked');

    // 2. Find the Flag Cash Call checkbox by ID
    var cashCall = $("#flag_cash_call");

    // 3. Sync the checked property
    cashCall.prop('checked', isChecked);
}