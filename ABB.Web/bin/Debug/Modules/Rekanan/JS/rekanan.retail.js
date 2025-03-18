$(document).ready(function () {
    setValueDetailRekananRetail();
});

function setValueDetailRekananRetail(){
    var bentukflag_value = $("#bentukflag_value").val();

    if(bentukflag_value === "1")
        $("#flag_sic_baru").attr("checked",true)
    else if(bentukflag_value === "2")
        $("#flag_sic_lama").attr("checked",true)
    
    var wniwna_value = $("#wniwna_value").val();

    if(wniwna_value === "1")
        $("#wniwna_wni").attr("checked",true)
    else if(wniwna_value === "2")
        $("#wniwna_wna").attr("checked",true)

    var wniflag_value = $("#wniflag_value").val();

    if(wniflag_value === "1")
        $("#wniflag_ktp").attr("checked",true)
    else if(wniflag_value === "2")
        $("#wniflag_sim").attr("checked",true)
    else if(wniflag_value === "3")
        $("#wniflag_paspor").attr("checked",true)

    var wnaflag_value = $("#wnaflag_value").val();

    if(wnaflag_value === "1")
        $("#wnaflag_paspor").attr("checked",true)
    else if(wnaflag_value === "2")
        $("#wnaflag_kms").attr("checked",true)
    else if(wnaflag_value === "3")
        $("#wnaflag_kitas").attr("checked",true)
    else if(wnaflag_value === "4")
        $("#wnaflag_kitap").attr("checked",true)

    var kawinflag_value = $("#kawinflag_value").val();

    if(kawinflag_value === "1")
        $("#kawinflag_belum_kawin").attr("checked",true)
    else if(kawinflag_value === "2")
        $("#kawinflag_kawin").attr("checked",true)
    else if(kawinflag_value === "3")
        $("#kawinflag_cerai").attr("checked",true)

    var pekerjaanflag_value = $("#pekerjaanflag_value").val();

    if(pekerjaanflag_value === "1")
        $("#pekerjaanflag_pns").attr("checked",true)
    else if(pekerjaanflag_value === "2")
        $("#pekerjaanflag_bumn").attr("checked",true)
    else if(pekerjaanflag_value === "3")
        $("#pekerjaanflag_wirausaha").attr("checked",true)
    else if(pekerjaanflag_value === "4")
        $("#pekerjaanflag_lain").attr("checked",true)

    var usahahasilflag_value = $("#usahahasilflag_value").val();

    if(usahahasilflag_value === "1")
        $("#usahahasilflag_1").attr("checked",true)
    else if(usahahasilflag_value === "2")
        $("#usahahasilflag_10").attr("checked",true)
    else if(usahahasilflag_value === "3")
        $("#usahahasilflag_25").attr("checked",true)
    else if(usahahasilflag_value === "4")
        $("#usahahasilflag_50").attr("checked",true)
    else if(usahahasilflag_value === "5")
        $("#usahahasilflag_100").attr("checked",true)

    var usahaflag_value = $("#usahaflag_value").val();

    if(usahaflag_value === "1")
        $("#usahaflag_hasil_usaha").attr("checked",true)
    else if(usahaflag_value === "2")
        $("#usahaflag_gaji_bulanan").attr("checked",true)
    else if(usahaflag_value === "3")
        $("#usahaflag_wirausaha").attr("checked",true)
    else if(usahaflag_value === "4")
        $("#usahaflag_lain").attr("checked",true)

    var tujuanpolisflag_value = $("#tujuanpolisflag_value").val();

    if(tujuanpolisflag_value === "1")
        $("#tujuanpolisflag_perlingungan").attr("checked",true)
    else if(tujuanpolisflag_value === "2")
        $("#tujuanpolisflag_lain").attr("checked",true)
}