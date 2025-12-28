$(document).ready(function () {
    setValueDetailRekananRetail();
});

function setValueDetailRekananRetail(){
    var bentukflag_value = $("#bentukflag_value").val();

    if(bentukflag_value === "1")
        $("#flag_sic_baru").prop("checked",true)
    else if(bentukflag_value === "2")
        $("#flag_sic_lama").prop("checked",true)
    
    var wniwna_value = $("#wniwna_value").val();

    if(wniwna_value === "1")
        $("#wniwna_wni").prop("checked",true)
    else if(wniwna_value === "2")
        $("#wniwna_wna").prop("checked",true)

    var wniflag_value = $("#wniflag_value").val();

    if(wniflag_value === "1")
        $("#wniflag_ktp").prop("checked",true)
    else if(wniflag_value === "2")
        $("#wniflag_sim").prop("checked",true)
    else if(wniflag_value === "3")
        $("#wniflag_paspor").prop("checked",true)

    var wnaflag_value = $("#wnaflag_value").val();

    if(wnaflag_value === "1")
        $("#wnaflag_paspor").prop("checked",true)
    else if(wnaflag_value === "2")
        $("#wnaflag_kims").prop("checked",true)
    else if(wnaflag_value === "3")
        $("#wnaflag_kitas").prop("checked",true)
    else if(wnaflag_value === "4")
        $("#wnaflag_suratKuasa").prop("checked",true)

    var tujuanpolisflag_value = $("#tujuanpolisflag_value").val();

    if(tujuanpolisflag_value === "1")
        $("#tujuanpolisflag_perlingungan").prop("checked",true)
    else if(tujuanpolisflag_value === "2")
        $("#tujuanpolisflag_lain").prop("checked",true)

    var npwpinstitusi_value = $("#npwpinstitusi_value").val();
    
    if(npwpinstitusi_value === "1")
        $("#npwpinstitusi").prop("checked",true)
    else
        $("#npwpinstitusi").prop("checked",false)

    var siupinstitusi_value = $("#siupinstitusi_value").val();
    
    if(siupinstitusi_value === "1")
        $("#siupinstitusi").prop("checked",true)
    else
        $("#siupinstitusi").prop("checked",false)

    var tdpinstitusi_value = $("#tdpinstitusi_value").val();
    
    if(tdpinstitusi_value === "1")
        $("#tdpinstitusi").prop("checked",true)
    else
        $("#tdpinstitusi").prop("checked",false)

    var hukumhaminstitusi_value = $("#hukumhaminstitusi_value").val();
    
    if(hukumhaminstitusi_value === "1")
        $("#hukumhaminstitusi").prop("checked",true)
    else
        $("#hukumhaminstitusi").prop("checked",false)
}