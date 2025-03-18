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

    var tujuanpolisflag_value = $("#tujuanpolisflag_value").val();

    if(tujuanpolisflag_value === "1")
        $("#tujuanpolisflag_perlingungan").attr("checked",true)
    else if(tujuanpolisflag_value === "2")
        $("#tujuanpolisflag_lain").attr("checked",true)

    var npwpinstitusi_value = $("#npwpinstitusi_value").val();
    
    if(npwpinstitusi_value === "1")
        $("#npwpinstitusi").attr("checked",true)
    else
        $("#npwpinstitusi").attr("checked",false)

    var siupinstitusi_value = $("#siupinstitusi_value").val();
    
    if(siupinstitusi_value === "1")
        $("#siupinstitusi").attr("checked",true)
    else
        $("#siupinstitusi").attr("checked",false)

    var tdpinstitusi_value = $("#tdpinstitusi_value").val();
    
    if(tdpinstitusi_value === "1")
        $("#tdpinstitusi").attr("checked",true)
    else
        $("#tdpinstitusi").attr("checked",false)

    var hukumhaminstitusi_value = $("#hukumhaminstitusi_value").val();
    
    if(hukumhaminstitusi_value === "1")
        $("#hukumhaminstitusi").attr("checked",true)
    else
        $("#hukumhaminstitusi").attr("checked",false)
}