$(document).ready(function () {
    $('#pranotaTab').kendoTabStrip({select: onSelectPranotaTab});

    var tabstrip = $('#pranotaTab').data("kendoTabStrip");
    tabstrip.select(0);

    btnPreviousPranota();
});

function btnPreviousPranota(){
    $('#btn-previous-akseptasiPranotaKoas').click(function () {
        $("#pranotaTab").getKendoTabStrip().select(0);
    });
}

function onSelectPranotaTab(e) {
    var tabName = $(e.item).find("> .k-link").text().trim();
    if(tabName == "Koasuransi Member")
    {
        showProgress('#AkseptasiWindow');

        ajaxGet(`/Akseptasi/CheckKoasuransi?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}`,
            function (response) {
                $("#tabKoasuransi").html(response);
                closeProgress('#AkseptasiWindow');
            }
        );
    }
}