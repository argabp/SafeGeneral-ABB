$(document).ready(function () {
    btnPreviousOther();
    
    $("#total_nilai_prm").text(currencyFormatter.format(Number($("#nilai_prm_tl").val()) + Number($("#nilai_prm_std").val())
        + Number($("#nilai_prm_bjr").val()) + Number($("#nilai_prm_gb").val())
        + Number($("#nilai_prm_phk").val())));
    
    $("#total_nilai_ptg").text(currencyFormatter.format(Number($("#nilai_ptg_bjr").val()) + Number($("#nilai_harga_ptg").val())
        + Number($("#nilai_ptg_phk").val()) + Number($("#nilai_ptg_gb").val())
        + Number($("#nilai_ptg_tl").val())));
});

function btnPreviousOther(){
    $('#btn-previous-inquiryResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
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

// Format totalNilaiAng as currency (money format)
var currencyFormatter = new Intl.NumberFormat('en-US', {
    style: 'decimal',
    minimumFractionDigits: 3,
    maximumFractionDigits: 3
});
