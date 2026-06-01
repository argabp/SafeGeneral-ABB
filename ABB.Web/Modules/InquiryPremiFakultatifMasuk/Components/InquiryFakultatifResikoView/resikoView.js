$(document).ready(function () {
    $('#resikoTab').kendoTabStrip({select: onSelect});

    var tabstrip = $('#resikoTab').data("kendoTabStrip");
    tabstrip.select(0);
    tabstrip.disable(tabstrip.items()[1]);
    tabstrip.disable(tabstrip.items()[2]);
    tabstrip.disable(tabstrip.items()[3]);
    tabstrip.disable(tabstrip.items()[4]);

    btnNextAkseptasiResiko();
    btnPreviousAkseptasiResiko();
});

function btnNextAkseptasiResiko(){
    $('#btn-next-akseptasiResikoView').click(function () {
        $("#akseptasiTab").getKendoTabStrip().select(2);
    });
}
function btnPreviousAkseptasiResiko(){
    $('#btn-previous-akseptasiResikoView').click(function () {
        $("#akseptasiTab").getKendoTabStrip().select(0);
    });
}

function onSelect(e) {
    var tabName = $(e.item).find("> .k-link").text().trim();
    if(tabName == "Coverage")
    {
        showProgress('#AkseptasiWindow');

        ajaxGet(`/InquiryPremiFakultatifMasuk/CheckCoverage?kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}`,
            function (response) {
                $("#tabCoverage").html(response);
                if($("#AkseptasiCoverageGrid").getKendoGrid() != undefined){
                    refreshGrid("#AkseptasiCoverageGrid");
                }
                closeProgress('#AkseptasiWindow');
            }
        );
    }
    else if(tabName == "Obyek")
    {
        showProgress('#AkseptasiWindow');

        ajaxGet(`/InquiryPremiFakultatifMasuk/CheckObyek?kd_cob=${$("#kd_cob").val()}&kd_scob=${$("#kd_scob").val()}&pst_share=${resiko.pst_share_bgu}`,
            function (response) {
                $("#tabObyek").html(response);
                if($("#AkseptasiObyekGrid").getKendoGrid() != undefined){
                    refreshGrid("#AkseptasiObyekGrid");
                }
                closeProgress('#AkseptasiWindow');
            }
        );
    }
    else if(tabName == "Others")
    {
        showProgress('#AkseptasiWindow');

        var dataOther = {
            kd_cb: $("#kd_cb").val(),
            kd_cob: $("#kd_cob").val(),
            kd_scob: $("#kd_scob").val(),
            kd_thn: $("#kd_thn").val(),
            no_pol: $("#no_pol").val(),
            no_updt: $("#no_updt").val(),
            no_rsk: resiko.no_rsk,
            kd_endt: resiko.kd_endt,
            pst_share: resiko.pst_share_bgu,
        }

        ajaxPost(`/InquiryPremiFakultatifMasuk/CheckOther`, JSON.stringify(dataOther),
            function (response) {
                $("#tabOther").html(response);
                closeProgress('#AkseptasiWindow');
            }
        );
    }
}