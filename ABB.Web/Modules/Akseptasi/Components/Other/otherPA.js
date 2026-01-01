$(document).ready(function () {
    btnPreviousOther();
    btnSaveAkseptasiResikoOther_Click();
    btnDeleteAkseptasiResikoOtherPA_Click();
    
    $("#total_nilai_prm").text(currencyFormatter.format(Number($("#nilai_prm_tl").val()) + Number($("#nilai_prm_std").val())
        + Number($("#nilai_prm_bjr").val()) + Number($("#nilai_prm_gb").val())
        + Number($("#nilai_prm_phk").val())));
    
    $("#total_nilai_ptg").text(currencyFormatter.format(Number($("#nilai_ptg_bjr").val()) + Number($("#nilai_harga_ptg").val())
        + Number($("#nilai_ptg_phk").val()) + Number($("#nilai_ptg_gb").val())
        + Number($("#nilai_ptg_tl").val())));

    if($("#IsNewOther").val() === "True"){
        $("#btn-delete-akseptasiResikoOtherPA").hide();
    }
});

function btnPreviousOther(){
    $('#btn-previous-akseptasiResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function btnDeleteAkseptasiResikoOtherPA_Click(){
    $('#btn-delete-akseptasiResikoOtherPA').click(function () {
        showConfirmation('Confirmation', `Are you sure you want to delete?`,
            function () {
                showProgress('#AkseptasiWindow');
                setTimeout(function () { deleteAkseptasiResikoOtherPA(); }, 500);
            }
        );
    });
}

function deleteAkseptasiResikoOtherPA() {
    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: $("#resiko_other_pa_no_updt").val(),
        no_rsk: resiko.no_rsk,
        kd_endt: $("#resiko_other_pa_kd_endt").val()
    }

    ajaxPost(`/Akseptasi/DeleteOtherPA`, JSON.stringify(data), function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            $("#btn-delete-akseptasiResikoOtherPA").hide();
        }
        else {
            showMessage('Error', 'Delete data is failed, this data is already used');
        }

        var dataOther = {
            kd_cb: $("#kd_cb").val(),
            kd_cob: $("#kd_cob").val(),
            kd_scob: $("#kd_scob").val(),
            kd_thn: $("#kd_thn").val(),
            no_aks: $("#no_aks").val(),
            no_updt: $("#no_updt").val(),
            no_rsk: resiko.no_rsk,
            kd_endt: resiko.kd_endt,
            pst_share: resiko.pst_share_bgu,
        }

        ajaxPost(`/Akseptasi/CheckOther`, JSON.stringify(dataOther),
            function (response) {
                $("#tabOther").html(response);
                closeProgress('#AkseptasiWindow');
            }
        );

        closeProgress('#AkseptasiWindow');
    });
}

function dataKodeJenisKreditDropDown(){
    return {
        kd_cb: $("#kd_cb").val()
    }
}

function OnJenisKreditChange(e){
    $("#kd_grp_kr").getKendoDropDownList().dataSource.read();
}

function dataJenisCoverDropDown(){
    return {
        kd_cb: $("#kd_cb").val(),
        kd_jns_kr: $("#kd_jns_kr").val()
    }
}

function dataAsuransiJiwaDropDown(){
    return {
        kd_grp_asj: $("#kd_grp_asj").val()
    }
}

function btnSaveAkseptasiResikoOther_Click() {
    $('#btn-save-akseptasiResikoOtherPA').click(function () {
        showProgress('#AkseptasiWindow');
        setTimeout(function () {
            saveAkseptasiResikoOther('/Akseptasi/SaveAkseptasiOtherPA')
        }, 500);
    });
}

function saveAkseptasiResikoOther(url) {
    var form = getFormData($('#OtherPAForm'));
    form.kd_cb = $("#kd_cb").val();
    form.kd_cob = $("#kd_cob").val();
    form.kd_scob = $("#kd_scob").val();
    form.kd_thn = $("#kd_thn").val();
    form.no_aks = $("#no_aks").val();
    form.no_updt = $("#resiko_other_pa_no_updt").val();
    form.no_rsk = resiko.no_rsk;
    form.kd_endt = $("#resiko_other_pa_kd_endt").val();
    form.kd_updt = $("#resiko_other_pa_kd_updt").val();
    form.tgl_mul_ptg = $("#resiko_other_pa_tgl_mul_ptg").val();
    form.tgl_akh_ptg = $("#resiko_other_pa_tgl_akh_ptg").val();
    form.tgl_input = $("#resiko_other_pa_tgl_input").val();
    form.no_pol_ttg = $("#no_pol_ttg").val();
    form.no_endt = $("#other_pa_no_endt").val();

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            refreshGrid("#AkseptasiResikoGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            }
            else if (response.Result == "ERROR")
                showMessage("Error", response.Message);
            else
                showMessage("Error", response);

            closeProgress('#AkseptasiWindow');
        }
    );
}

function OnNilaiHargaPtgChange(e){
    var nilai_harga_ptg = e.sender.value();
    ajaxGet(`/Akseptasi/GenerateDataOther?no_pol_ttg=${$("#no_pol_ttg").val()}&kd_jns_kr=${$("#kd_jns_kr").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&jk_wkt=${$("#jk_wkt").val()}&usia_deb=${$("#usia_deb").val()}&nilai_harga_ptg=${nilai_harga_ptg}&flag_std=${$("#flag_std").val()}&pst_rate_std=${$("#pst_rate_std").val()}&flag_bjr=${$("#flag_bjr").val()}&pst_rate_bjr=${$("#pst_rate_bjr").val()}&flag_gb=${$("#flag_gb").val()}&pst_rate_gb=${$("#pst_rate_gb").val()}&flag_tl=${$("#flag_tl").val()}&pst_rate_tl=${$("#pst_rate_tl").val()}`, (returnValue) => {
        $("#flag_bjr").getKendoDropDownList().value(returnValue[0].split(",")[1]);
        $("#flag_bjr").getKendoDropDownList().trigger("change");
        $("#flag_gb").getKendoDropDownList().value(returnValue[1].split(",")[1]);
        $("#flag_gb").getKendoDropDownList().trigger("change");
        $("#flag_tl").getKendoDropDownList().value(returnValue[2].split(",")[1]);
        $("#flag_tl").getKendoDropDownList().trigger("change");
        $("#nilai_prm_bjr").getKendoNumericTextBox().value(returnValue[3].split(",")[1]);
        $("#nilai_prm_bjr").getKendoNumericTextBox().trigger("change");
        $("#nilai_prm_gb").getKendoNumericTextBox().value(returnValue[4].split(",")[1]);
        $("#nilai_prm_gb").getKendoNumericTextBox().trigger("change");
        $("#nilai_prm_phk").getKendoNumericTextBox().value(returnValue[5].split(",")[1]);
        $("#nilai_prm_phk").getKendoNumericTextBox().trigger("change");
        $("#nilai_prm_std").getKendoNumericTextBox().value(returnValue[6].split(",")[1]);
        $("#nilai_prm_std").getKendoNumericTextBox().trigger("change");
        $("#nilai_prm_tl").getKendoNumericTextBox().value(returnValue[7].split(",")[1]);
        $("#nilai_prm_tl").getKendoNumericTextBox().trigger("change");
        $("#nilai_ptg_bjr").getKendoNumericTextBox().value(returnValue[8].split(",")[1]);
        $("#nilai_ptg_bjr").getKendoNumericTextBox().trigger("change");
        $("#nilai_ptg_gb").getKendoNumericTextBox().value(returnValue[9].split(",")[1]);
        $("#nilai_ptg_gb").getKendoNumericTextBox().trigger("change");
        $("#nilai_ptg_phk").getKendoNumericTextBox().value(returnValue[10].split(",")[1]);
        $("#nilai_ptg_phk").getKendoNumericTextBox().trigger("change");
        $("#nilai_ptg_tl").getKendoNumericTextBox().value(returnValue[11].split(",")[1]);
        $("#nilai_ptg_tl").getKendoNumericTextBox().trigger("change");
        $("#pst_rate_bjr").getKendoNumericTextBox().value(returnValue[12].split(",")[1]);
        $("#pst_rate_bjr").getKendoNumericTextBox().trigger("change");
        $("#pst_rate_gb").getKendoNumericTextBox().value(returnValue[13].split(",")[1]);
        $("#pst_rate_gb").getKendoNumericTextBox().trigger("change");
        $("#pst_rate_phk").getKendoNumericTextBox().value(returnValue[14].split(",")[1]);
        $("#pst_rate_phk").getKendoNumericTextBox().trigger("change");
        $("#pst_rate_std").getKendoNumericTextBox().value(returnValue[15].split(",")[1]);
        $("#pst_rate_std").getKendoNumericTextBox().trigger("change");
        $("#pst_rate_tl").getKendoNumericTextBox().value(returnValue[16].split(",")[1]);
        $("#pst_rate_tl").getKendoNumericTextBox().trigger("change");
        $("#stn_rate_phk").getKendoDropDownList().value(returnValue[17].split(",")[1]);
    });
    
    $("#total_nilai_ptg").text(currencyFormatter.format(nilai_harga_ptg + Number($("#nilai_ptg_phk").val())
        + Number($("#nilai_ptg_bjr").val()) + Number($("#nilai_ptg_gb").val())
        + Number($("#nilai_ptg_tl").val())));
}

function OnFlagStdChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAPremi?stn_rate=${$("#stn_rate_std").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&flag=${e.sender._cascadedValue}&nilai_harga_ptg=${$("#nilai_harga_ptg").val()}&pst_rate=${$("#pst_rate_std").val()}&no=1`, (returnValue) => {
        $("#nilai_prm_std").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_std").getKendoNumericTextBox().trigger("change");
    });
}

function OnFlagBjrChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAPremi?stn_rate=${$("#stn_rate_bjr").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&flag=${e.sender._cascadedValue}&nilai_harga_ptg=${$("#nilai_harga_ptg").val()}&pst_rate=${$("#pst_rate_bjr").val()}&no=2`, (returnValue) => {
        $("#nilai_prm_bjr").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_bjr").getKendoNumericTextBox().trigger("change");
    });
}

function OnFlagGBChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAPremi?stn_rate=${$("#stn_rate_gb").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&flag=${e.sender._cascadedValue}&nilai_harga_ptg=${$("#nilai_harga_ptg").val()}&pst_rate=${$("#pst_rate_gb").val()}&no=3`, (returnValue) => {
        $("#nilai_prm_gb").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_gb").getKendoNumericTextBox().trigger("change");
    });
}

function OnFlagTLChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAPremi?stn_rate=${$("#stn_rate_tl").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&flag=${e.sender._cascadedValue}&nilai_harga_ptg=${$("#nilai_harga_ptg").val()}&pst_rate=${$("#pst_rate_tl").val()}&no=4`, (returnValue) => {
        $("#nilai_prm_tl").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_tl").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRateStdChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAPremi?stn_rate=${$("#stn_rate_std").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&flag=${$("#flag_std").val()}&nilai_harga_ptg=${$("#nilai_harga_ptg").val()}&pst_rate=${e.sender.value()}&no=1`, (returnValue) => {
        $("#nilai_prm_std").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_std").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRateBjrChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAPremi?stn_rate=${$("#stn_rate_bjr").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&flag=${$("#flag_bjr").val()}&nilai_harga_ptg=${$("#nilai_harga_ptg").val()}&pst_rate=${e.sender.value()}&no=1`, (returnValue) => {
        $("#nilai_prm_bjr").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_bjr").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRateGBChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAPremi?stn_rate=${$("#stn_rate_gb").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&flag=${$("#flag_gb").val()}&nilai_harga_ptg=${$("#nilai_harga_ptg").val()}&pst_rate=${e.sender.value()}&no=1`, (returnValue) => {
        $("#nilai_prm_gb").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_gb").getKendoNumericTextBox().trigger("change");
    });
}

function OnPstRateTLChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAPremi?stn_rate=${$("#stn_rate_tl").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}&flag=${$("#flag_tl").val()}&nilai_harga_ptg=${$("#nilai_harga_ptg").val()}&pst_rate=${e.sender.value()}&no=1`, (returnValue) => {
        $("#nilai_prm_tl").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_tl").getKendoNumericTextBox().trigger("change");
    });
}

function OnNilaiPtgPhkChange(e){
    var nilai_ptg_phk = e.sender.value();
    ajaxGet(`/Akseptasi/GeneratePrmPhk?nilai_ptg_phk=${nilai_ptg_phk}&pst_rate_phk=${$("#pst_rate_phk").val()}&stn_rate_std=${$("#stn_rate_std").val()}`, (returnValue) => {
        $("#nilai_prm_phk").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_phk").getKendoNumericTextBox().trigger("change");
    });
    
    $("#total_nilai_ptg").text(currencyFormatter.format(nilai_ptg_phk + Number($("#nilai_harga_ptg").val()) 
        + Number($("#nilai_ptg_bjr").val()) + Number($("#nilai_ptg_gb").val())
        + Number($("#nilai_ptg_tl").val())));
}

function OnPstRatePhkChange(e){
    ajaxGet(`/Akseptasi/GeneratePrmPhk?nilai_ptg_phk=${$("#nilai_ptg_hh").val()}&pst_rate_phk=${e.sender.value()}&stn_rate_std=${$("#stn_rate_std").val()}`, (returnValue) => {
        $("#nilai_prm_phk").getKendoNumericTextBox().value(returnValue.split(",")[1]);
        $("#nilai_prm_phk").getKendoNumericTextBox().trigger("change");
    });
}

function OnNilaiPrmStdChange(e){
    var nilai_prm_std = e.sender.value();
    // ajaxGet(`/Akseptasi/GenerateNilaiPrmBtn?nilai_prm_std=${nilai_prm_std}&nilai_prm_bjr=${$("#nilai_prm_bjr").val()}&nilai_prm_tl=${$("#nilai_prm_tl").val()}&nilai_prm_gb=${$("#nilai_prm_gb").val()}&nilai_prm_phk=${$("#nilai_prm_phk").val()}&nilai_bia_adm=${$("#nilai_bia_adm").val()}&nilai_bia_mat=${$("#other_pa_nilai_bia_mat").val()}&jk_wkt=${$("#jk_wkt").val()}&no_endt=${$("#no_endt").val()}`, (returnValue) => {
    //     $("#nilai_prm_btn").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    //     $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    // });

    var cobs = ["0552", "0560", "0561", "0562", "0563", "0564", "0565", "0566"]
    
    if(cobs.includes($("#kd_cob").val().trim())){
        nilai_prm_std = 0;
    }
    
    var totalValue = nilai_prm_std + Number($("#nilai_prm_bjr").val())
        + Number($("#nilai_prm_gb").val()) + Number($("#nilai_prm_tl").val())
        + Number($("#nilai_prm_phk").val());

    $("#nilai_prm_btn").getKendoNumericTextBox().value(totalValue);
    $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    
    $("#total_nilai_prm").text(currencyFormatter.format(totalValue));
}

function OnNilaiPrmBjrChange(e){
    var nilai_prm_bjr = e.sender.value();
    // ajaxGet(`/Akseptasi/GenerateNilaiPrmBtn?nilai_prm_std=${$("#nilai_prm_std").val()}&nilai_prm_bjr=${nilai_prm_bjr}&nilai_prm_tl=${$("#nilai_prm_tl").val()}&nilai_prm_gb=${$("#nilai_prm_gb").val()}&nilai_prm_phk=${$("#nilai_prm_phk").val()}&nilai_bia_adm=${$("#nilai_bia_adm").val()}&nilai_bia_mat=${$("#other_pa_nilai_bia_mat").val()}&jk_wkt=${$("#jk_wkt").val()}&no_endt=${$("#no_endt").val()}`, (returnValue) => {
    //     $("#nilai_prm_btn").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    //     $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    // });
    
    var totalValue = nilai_prm_bjr + Number($("#nilai_prm_std").val())
        + Number($("#nilai_prm_gb").val()) + Number($("#nilai_prm_tl").val())
        + Number($("#nilai_prm_phk").val())
    
    $("#nilai_prm_btn").getKendoNumericTextBox().value(totalValue);
    $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    
    $("#total_nilai_prm").text(currencyFormatter.format(totalValue));
}

function OnNilaiPrmGBChange(e){
    var nilai_prm_gb = e.sender.value();
    // ajaxGet(`/Akseptasi/GenerateNilaiPrmBtn?nilai_prm_std=${$("#nilai_prm_std").val()}&nilai_prm_bjr=${$("#nilai_prm_bjr").val()}&nilai_prm_tl=${$("#nilai_prm_tl").val()}&nilai_prm_gb=${nilai_prm_gb}&nilai_prm_phk=${$("#nilai_prm_phk").val()}&nilai_bia_adm=${$("#nilai_bia_adm").val()}&nilai_bia_mat=${$("#other_pa_nilai_bia_mat").val()}&jk_wkt=${$("#jk_wkt").val()}&no_endt=${$("#no_endt").val()}`, (returnValue) => {
    //     $("#nilai_prm_btn").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    //     $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    // });

    var totalValue = nilai_prm_gb + Number($("#nilai_prm_std").val())
        + Number($("#nilai_prm_bjr").val()) + Number($("#nilai_prm_tl").val())
        + Number($("#nilai_prm_phk").val());

    $("#nilai_prm_btn").getKendoNumericTextBox().value(totalValue);
    $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    
    $("#total_nilai_prm").text(currencyFormatter.format(totalValue));
}

function OnNilaiPrmTLChange(e){
    var nilai_prm_tl = e.sender.value();
    // ajaxGet(`/Akseptasi/GenerateNilaiPrmBtn?nilai_prm_std=${$("#nilai_prm_std").val()}&nilai_prm_bjr=${$("#nilai_prm_bjr").val()}&nilai_prm_tl=${nilai_prm_tl}&nilai_prm_gb=${$("#nilai_prm_gb").val()}&nilai_prm_phk=${$("#nilai_prm_phk").val()}&nilai_bia_adm=${$("#nilai_bia_adm").val()}&nilai_bia_mat=${$("#other_pa_nilai_bia_mat").val()}&jk_wkt=${$("#jk_wkt").val()}&no_endt=${$("#no_endt").val()}`, (returnValue) => {
    //     $("#nilai_prm_btn").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    //     $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    // });
    
    var totalValue = nilai_prm_tl + Number($("#nilai_prm_std").val())
        + Number($("#nilai_prm_bjr").val()) + Number($("#nilai_prm_gb").val())
        + Number($("#nilai_prm_phk").val());
    
    $("#nilai_prm_btn").getKendoNumericTextBox().value(totalValue);
    $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    
    $("#total_nilai_prm").text(currencyFormatter.format(totalValue));
}

function OnNilaiPrmPHKChange(e){
    var nilai_prm_phk = e.sender.value();
    // ajaxGet(`/Akseptasi/GenerateNilaiPrmBtn?nilai_prm_std=${$("#nilai_prm_std").val()}&nilai_prm_bjr=${$("#nilai_prm_bjr").val()}&nilai_prm_tl=${$("#nilai_prm_tl").val()}&nilai_prm_gb=${$("#nilai_prm_gb").val()}&nilai_prm_phk=${nilai_prm_phk}&nilai_bia_adm=${$("#nilai_bia_adm").val()}&nilai_bia_mat=${$("#other_pa_nilai_bia_mat").val()}&jk_wkt=${$("#jk_wkt").val()}&no_endt=${$("#no_endt").val()}`, (returnValue) => {
    //     $("#nilai_prm_btn").getKendoNumericTextBox().value(returnValue.split(",")[1]);
    //     $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");
    // });

    var totalValue = nilai_prm_phk + Number($("#nilai_prm_std").val())
        + Number($("#nilai_prm_bjr").val()) + Number($("#nilai_prm_gb").val())
        + Number($("#nilai_prm_tl").val());
    
    $("#nilai_prm_btn").getKendoNumericTextBox().value(totalValue);
    $("#nilai_prm_btn").getKendoNumericTextBox().trigger("change");

    $("#total_nilai_prm").text(currencyFormatter.format(totalValue));
}

function OnNilaiPtgBjrChange(e){
    $("#total_nilai_ptg").text(currencyFormatter.format(e.sender.value() + Number($("#nilai_harga_ptg").val())
        + Number($("#nilai_ptg_phk").val()) + Number($("#nilai_ptg_gb").val())
        + Number($("#nilai_ptg_tl").val())));
}

function OnNilaiPtgGBChange(e){
    $("#total_nilai_ptg").text(currencyFormatter.format(e.sender.value() + Number($("#nilai_harga_ptg").val())
        + Number($("#nilai_ptg_phk").val()) + Number($("#nilai_ptg_bjr").val())
        + Number($("#nilai_ptg_tl").val())));
}

function OnNilaiPtgTLChange(e){
    $("#total_nilai_ptg").text(currencyFormatter.format(e.sender.value() + Number($("#nilai_harga_ptg").val())
        + Number($("#nilai_ptg_phk").val()) + Number($("#nilai_ptg_bjr").val())
        + Number($("#nilai_ptg_gb").val())));
}

function OnNilaiBiaAdmChange(e){
    $("#resiko_other_pa_total").getKendoNumericTextBox().value(e.sender.value() + Number($("#nilai_prm_btn").val()));
}

function OnNilaiPrmBtnChange(e){
    $("#resiko_other_pa_total").getKendoNumericTextBox().value(e.sender.value() + Number($("#nilai_bia_adm").val()));
}


// Format totalNilaiAng as currency (money format)
var currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 3,
    maximumFractionDigits: 3
});

function OnTanggalPKChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAData?jk_wkt=${$("#jk_wkt").val()}&tgl_lahir=${$("#tgl_lahir").val()}&tgl_real=${e.sender.value().toLocaleDateString()}&kd_cb=${$("#kd_cb").val()}&kd_jns_kr=${$("#kd_jns_kr").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}`, (returnValue) => {
        $("#resiko_other_pa_tgl_mul_ptg").getKendoDatePicker().value(returnValue[2].split(",")[1]);
        $("#resiko_other_pa_tgl_akh_ptg").getKendoDatePicker().value(returnValue[1].split(",")[1]);
        $("#usia_deb").getKendoNumericTextBox().value(returnValue[3].split(",")[1]);
        $("#other_pa_no_endt").val(returnValue[0].split(",")[1]);
    });
}

function OnJkWktChange(e){
    ajaxGet(`/Akseptasi/GenerateOtherPAData?jk_wkt=${e.sender.value()}&tgl_lahir=${$("#tgl_lahir").val()}&tgl_real=${$("#tgl_real").val()}&kd_cb=${$("#kd_cb").val()}&kd_jns_kr=${$("#kd_jns_kr").val()}&kd_grp_kr=${$("#kd_grp_kr").val()}`, (returnValue) => {
        $("#resiko_other_pa_tgl_mul_ptg").getKendoDatePicker().value(returnValue[2].split(",")[1]);
        $("#resiko_other_pa_tgl_akh_ptg").getKendoDatePicker().value(returnValue[1].split(",")[1]);
        $("#usia_deb").getKendoNumericTextBox().value(returnValue[3].split(",")[1]);
        $("#other_pa_no_endt").val(returnValue[0].split(",")[1]);
    });
}