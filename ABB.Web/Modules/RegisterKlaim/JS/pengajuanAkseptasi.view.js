
$(document).ready(function () {
    setTimeout(() => {
        $(".k-grid-add").hide();
        $("#LampiranPengajuanAkseptasiGrid").data("kendoGrid").hideColumn(0); // hide first column
        $("#LampiranPengajuanAkseptasiGrid").data("kendoGrid").hideColumn(1); // hide second column
        $("#btn-save-pengajuanAkseptasi").hide();
    }, 3000);
});