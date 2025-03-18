$(document).ready(function () {
    OnClickPreview();
});

function onKodeCabangDataBound(e){
    loadKodeRekanan();
}

function loadKodeRekanan()
{
    var kantor = $("#kantor").val();
    var rekanan = $("#rekanan").data("kendoDropDownList");
    rekanan.dataSource.read({kd_cb : kantor});
}

function OnClickPreview(){
    $('#btn-preview').click(function () {
        var kantor = $("#kantor").data("kendoDropDownList").value();
        var rekanan = $("#rekanan").data("kendoDropDownList").value();
        var tgl_awal_pengajuan = $("#tgl_awal_pengajuan").val();
        var tgl_akhir_pengajuan = $("#tgl_akhir_pengajuan").val();
        
        window.open(`ViewReport?input_str=${kantor.trim()},${rekanan.trim()},${tgl_awal_pengajuan},${tgl_akhir_pengajuan}`);
    });
}