$(document).ready(function () {
    btnSaveDetailAlokasi_Click();
    showProgress('#FormAlokasiWindow');
    setTimeout(setDetailAlokasiEditedValue, 500);
});

function setDetailAlokasiEditedValue(){
    var kd_rk_sor_dp = $("#kd_rk_sor").data("kendoDropDownList");
    kd_rk_sor_dp.dataSource.read({jns_lookup : $("#kd_grp_sor").val() + ",R"});

    setTimeout(() => {
        kd_rk_sor_dp.value($("#temp_kd_rk_sor").val().trim());
    } , 1000);
    
    if($("#IsViewOnly").val() == "True"){
        $("#div-save-alokasiDetail").hide();
    }

    closeProgress('#FormAlokasiWindow');
}

function btnSaveDetailAlokasi_Click() {
    $('#btn-save-detailAlokasi').click(function () {
        showProgress('#FormAlokasiWindow');
        setTimeout(function () {
            saveDetailAlokasi('/Alokasi/SaveDetailAlokasi')
        }, 500);
    });
}

function saveDetailAlokasi(url) {
    var form = getFormData($('#AlokasiForm'));
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
            refreshGrid("#DetailAlokasiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#FormAlokasiWindow');
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#FormAlokasiWindow');
        }
    );
}

function OnKodeJenisSorChange(e){
    var kd_jns_sor = e.sender._cascadedValue;
    var kd_cob = sorData?.kd_cob.trim();
    var kd_cb = sorData?.kd_cob.trim();
    var thn_uw = sorData?.thn_uw;
    var nilai_ttl_ptg = alokasi?.nilai_ttl_ptg;
    var nilai_prm = alokasi?.nilai_prm;

    ajaxGet(`/Alokasi/GetGroupAndRekananSor?kd_jns_sor=${kd_jns_sor}&kd_cob=${kd_cob}&kd_cb=${kd_cb}&thn_uw=${thn_uw}&nilai_ttl_ptg=${nilai_ttl_ptg}&nilai_prm=${nilai_prm}`,
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
    var nilai_ttl_ptg = alokasi?.nilai_ttl_ptg;
    var nilai_prm = alokasi?.nilai_prm;
    var net_prm = $("#net_prm").val();

    ajaxGet(`/Alokasi/GetShareAndPremiReas?kd_jns_sor=${kd_jns_sor}&nilai_ttl_ptg_reas=${nilai_ttl_ptg_reas}&nilai_ttl_ptg=${nilai_ttl_ptg}&nilai_prm=${nilai_prm}&net_prm=${net_prm}`,
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
    var pst_share = e.sender.value();
    var nilai_prm_reas = $("#nilai_prm_reas").val();
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var nilai_ttl_ptg = alokasi?.nilai_ttl_ptg;
    var nilai_prm = alokasi?.nilai_prm;
    var net_prm = $("#net_prm").val();

    ajaxGet(`/Alokasi/GetTTLAndPremiReas?kd_jns_sor=${kd_jns_sor}&pst_share=${pst_share}&nilai_ttl_ptg=${nilai_ttl_ptg}&nilai_prm=${nilai_prm}&net_prm=${net_prm}&nilai_prm_reas=${nilai_prm_reas}`,
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

    ajaxGet(`/Alokasi/GetKmsReas?pst_kms_reas=${pst_kms_reas}&nilai_adj_reas=${nilai_adj_reas}&nilai_prm_reas=${nilai_prm_reas}`,
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
    var nilai_prm = alokasi?.nilai_prm;
    var pst_rate_prm = $("#pst_rate_prm").val();
    var stn_rate_prm = $("#stn_rate_prm").val();
    var kd_cb = sorData?.kd_cb.trim();
    var kd_cob = sorData?.kd_cob.trim();
    var kd_scob = sorData?.kd_scob.trim();
    var kd_thn = sorData?.kd_thn.trim();
    var no_pol = sorData?.no_pol.val();
    var no_updt = sorData?.no_updt.val();
    var no_rsk = alokasi?.no_rsk;

    ajaxGet(`/Alokasi/GetAdjReas?pst_share=${pst_share}&pst_adj_reas=${pst_adj_reas}&stn_adj_reas=${stn_adj_reas}
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