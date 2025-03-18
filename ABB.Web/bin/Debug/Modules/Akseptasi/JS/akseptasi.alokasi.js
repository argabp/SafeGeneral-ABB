$(document).ready(function () {
    btnSaveAkseptasiAlokasi_Click();
    showProgress('#AlokasiWindow');
    setTimeout(setAlokasiEditedValue, 500);
});

function setAlokasiEditedValue(){
    var kd_rk_sor_dp = $("#kd_rk_sor").data("kendoDropDownList");
    kd_rk_sor_dp.dataSource.read({jns_lookup : $("#kd_grp_sor").val() + ",R"});

    setTimeout(() => {
        kd_rk_sor_dp.value($("#temp_kd_rk_sor").val().trim());
    } , 1000)

    closeProgress('#AlokasiWindow');
}

function btnSaveAkseptasiAlokasi_Click() {
    $('#btn-save-akseptasiAlokasi').click(function () {
        showProgress('#AlokasiWindow');
        setTimeout(function () {
            saveAkseptasiAlokasi('/Akseptasi/SaveDetailAlokasi')
        }, 500);
    });
}

function saveAkseptasiAlokasi(url) {
    var form = getFormData($('#AlokasiForm'));
    form.pst_kms_reas = $("#pst_kms_reas_alokasi").val();
    form.nilai_kms_reas = $("#nilai_kms_reas_alokasi").val();
    form.pst_adj_reas = $("#pst_adj_reas_alokasi").val();
    form.stn_adj_reas = $("#stn_adj_reas_alokasi").val();
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_pol = $("#no_aks").val();
    form.no_updt = $("#alokasi_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = resiko.kd_endt;
    form.no_updt_reas = $("#no_updt_reas").val();
    
    var data = JSON.stringify(form);
    
    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AlokasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#AlokasiWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AlokasiWindow');
        }
    );
}

function OnKodeJenisSorChange(e){
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var kd_cob = $("#kd_cob").val().trim();
    var kd_cb = $("#kd_cb").val().trim();
    var thn_uw = $("#thn_uw").val();
    var nilai_ttl_ptg = $("#nilai_ttl_ptg").val();
    var nilai_prm = $("#nilai_prm").val();
    
    ajaxGet(`/Akseptasi/GetGroupAndRekananSor?kd_jns_sor=${kd_jns_sor}&kd_cob=${kd_cob}&kd_cb=${kd_cb}&thn_uw=${thn_uw}&nilai_ttl_ptg=${nilai_ttl_ptg}&nilai_prm=${nilai_prm}`,
        function (response) {
            if (response.Status == "OK") {
                var kd_grp_sor = response.Data.split(",")[1];
                $("#kd_grp_sor").getKendoNumericTextBox().value(kd_grp_sor);
                var kd_rk_sor = response.Data.split(",")[4];

                var kd_rk_sor_dp = $("#kd_rk_sor").data("kendoDropDownList");
                kd_rk_sor_dp.dataSource.read({jns_lookup : kd_grp_sor + ",R"});

                kd_rk_sor_dp.value(kd_rk_sor);
                
                if(kd_jns_sor == "FAC"){
                    $("#premiAdjustmentDiv").show()
                } else{
                    $("#premiAdjustmentDiv").hide()
                }
            }
            else if (response.Status == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);
        }
    );
}

function OnPtgReasChange(e){
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var nilai_ttl_ptg_reas = $("#nilai_ttl_ptg_reas").val();
    var nilai_ttl_ptg = $("#nilai_ttl_ptg").val();
    var nilai_prm = $("#nilai_prm").val();
    var net_prm = $("#net_prm").val();

    ajaxGet(`/Akseptasi/GetShareAndPremiReas?kd_jns_sor=${kd_jns_sor}&nilai_ttl_ptg_reas=${nilai_ttl_ptg_reas}&nilai_ttl_ptg=${nilai_ttl_ptg}&nilai_prm=${nilai_prm}&net_prm=${net_prm}`,
        function (response) {
            if (response.Status == "OK") {
                var pst_share = response.Data.split(",")[1];
                $("#pst_share").getKendoNumericTextBox().value(pst_share);
                var nilai_prm_reas = response.Data.split(",")[4];
                $("#nilai_prm_reas").getKendoNumericTextBox().value(nilai_prm_reas);
            }
            else if (response.Status == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);
        }
    );
}

function OnPstShareChange(e){
    var pst_share = $("#pst_share").val();
    var nilai_prm_reas = $("#nilai_prm_reas").val();
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var nilai_ttl_ptg = $("#nilai_ttl_ptg").val();
    var nilai_prm = $("#nilai_prm").val();
    var net_prm = $("#net_prm").val();

    ajaxGet(`/Akseptasi/GetTTLAndPremiReas?kd_jns_sor=${kd_jns_sor}&pst_share=${pst_share}&nilai_ttl_ptg=${nilai_ttl_ptg}&nilai_prm=${nilai_prm}&net_prm=${net_prm}&nilai_prm_reas=${nilai_prm_reas}`,
        function (response) {
            if (response.Status == "OK") {
                var nilai_ttl_ptg = response.Data.split(",")[1];
                $("#nilai_ttl_ptg").getKendoNumericTextBox().value(nilai_ttl_ptg);
                var nilai_prm_reas = response.Data.split(",")[4];
                $("#nilai_prm_reas").getKendoNumericTextBox().value(nilai_prm_reas);
            }
            else if (response.Status == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);
        }
    );
}

function OnPstKmsReasChange(e){
    var pst_kms_reas = $("#pst_kms_reas_alokasi").val();
    var nilai_prm_reas = $("#nilai_prm_reas").val();
    var nilai_adj_reas = $("#nilai_adj_reas").val();

    ajaxGet(`/Akseptasi/GetKmsReas?pst_kms_reas=${pst_kms_reas}&nilai_adj_reas=${nilai_adj_reas}&nilai_prm_reas=${nilai_prm_reas}`,
        function (response) {
            if (response.Status == "OK") {
                var nilai_kms_reas = response.Data.split(",")[1];
                $("#nilai_kms_reas_alokasi").getKendoNumericTextBox().value(nilai_kms_reas);
            }
            else if (response.Status == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);
        }
    );
}

function OnAdjReasChange(e){
    var pst_share = $("#pst_share").val();
    var pst_adj_reas = $("#pst_adj_reas_alokasi").val();
    var stn_adj_reas = $("#stn_adj_reas").val();
    var pst_kms = $("#pst_kms").val();
    var nilai_prm_reas = $("#nilai_prm_reas").val();
    var nilai_prm = $("#nilai_prm").val();
    var pst_rate_prm = $("#pst_rate_prm").val();
    var stn_rate_prm = $("#stn_rate_prm").val();
    var kd_cb = $("#kd_cb").val().trim();
    var kd_cob = $("#kd_cob").val().trim();
    var kd_scob = $("#kd_scob").val().trim();
    var kd_thn = $("#kd_thn").val().trim();
    var no_pol = $("#no_pol").val();
    var no_updt = $("#no_updt").val();
    var no_rsk = resiko.no_rsk;

    ajaxGet(`/Akseptasi/GetAdjReas?pst_share=${pst_share}&pst_adj_reas=${pst_adj_reas}&stn_adj_reas=${stn_adj_reas}
                &pst_kms=${pst_kms}&nilai_prm_reas=${nilai_prm_reas}&nilai_prm=${nilai_prm}&pst_rate_prm=${pst_rate_prm}
                &stn_rate_prm=${stn_rate_prm}&kd_cb=${kd_cb}&kd_cob=${kd_cob}&kd_scob=${kd_scob}&kd_thn=${kd_thn}
                &no_pol=${no_pol}&no_updt=${no_updt}&no_rsk=${no_rsk}`,
        function (response) {
            if (response.Status == "OK") {
                var nilai_adj_reas = response.Data.split(",")[1];
                $("#nilai_adj_reas").getKendoNumericTextBox().value(nilai_adj_reas);
            }
            else if (response.Status == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);
        }
    );
}