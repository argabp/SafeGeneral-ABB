$(document).ready(function () {
    $('#akseptasiTab').kendoTabStrip({select: onSelectAkseptasiTab});

    var tabstrip = $('#akseptasiTab').data("kendoTabStrip");
    tabstrip.select(0);
});

function onSelectAkseptasiTab(e) {
    var tabName = $(e.item).find("> .k-link").text().trim();
    if(tabName == "Pranota")
    {
        showProgress('#AkseptasiWindow');

        ajaxGet(`/Akseptasi/CheckPranota?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}&no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}`,
            function (response) {
                $("#tabTertanggung").html(response);
                closeProgress('#AkseptasiWindow');
            }
        );
    }
}