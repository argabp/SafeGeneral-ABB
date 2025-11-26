$(document).ready(function () {
    btnPreviousOther();
});

function btnPreviousOther(){
    $('#btn-previous-inquiryResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}

function dataKodeSerutyDropDown(){
    return {
        kd_cb: $("#kd_cb").val(),
        kd_grp_surety: $("#kd_grp_surety").val()
    }
}

function dataKodePrincipalDropDown(){
    return {
        kd_cb: $("#kd_cb").val(),
        kd_grp_rk: $("#kd_grp_prc").val()
    }
}

function OnChangeKodePrincipal(e){
    $("#kd_rk_prc").getKendoDropDownList().dataSource.read();
}

function dataGroupObligeeDropDown(){
    return {
        grp_obl: $("#grp_obl").val()
    }
}

function dataKodeObligeeDropDown(){
    return {
        kd_grp_rk: $("#kd_grp_obl").val()
    }
}

function dataGroupKontrakDropDown(){
    return {
        grp_kontr: $("#grp_kontr").val()
    }
}

function dataKodePekerjaanDropDown(){
    return {
        kd_grp_rk: $("#grp_jns_pekerjaan").val()
    }
}

function dataTTDSuretyDropDown(){
    return {
        kd_cb: $("#kd_cb").val()
    }
}

