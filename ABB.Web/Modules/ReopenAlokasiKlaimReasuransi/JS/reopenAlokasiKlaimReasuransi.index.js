$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchFilterReopenAlokasiKlaimReasuransi(e) {
    const gridReq = buildGridRequest(e, "SearchKeyword");

    return {
        grid: gridReq
    };
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ReopenAlokasiKlaimReasuransiGrid");
    });
}

function openReopenAlokasiKlaimReasuransiWindow(url, title) {
    openWindow('#ReopenAlokasiKlaimReasuransiWindow', url, title);
}
//
// function OnClickViewMutasiKlaim(e) {
//     e.preventDefault();
//     var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
//     console.log('dataItem', dataItem);
//    
//     showProgressByElement($("#KlaimAlokasiReasuransiWindow"));
//    
//     openKlaimAlokasiReasuransiWindow(`/KlaimAlokasiReasuransi/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}&no_mts=${dataItem.no_mts}`, 'View');
//    
// }

function OnClickReopenAlokasiKlaimReasuransi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    showConfirmation('Confirmation', `Are you sure you want to reopen SOL?`,
        function () {
            showProgressOnGrid('#ReopenAlokasiKlaimReasuransiGrid');
            setTimeout(function () { reopenAlokasiKlaimReasuransi(dataItem); }, 500);
        }
    );
}

function reopenAlokasiKlaimReasuransi(dataItem){    
    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_kl = dataItem.no_kl;
    form.no_mts = dataItem.no_mts;

    var data = JSON.stringify(form);
    ajaxPost(`/ReopenAlokasiKlaimReasuransi/ReopenAlokasiKlaimReasuransi`, data,  function (response) {
        if (response.Status === "OK") {
            showMessage('Reopen Successfully', response.Message);
        }
        else {
            showMessage('Error', 'Reopen is failed, ' + response.Message);
        };

        refreshGrid("#ReopenAlokasiKlaimReasuransiGrid");
        closeProgressOnGrid('#ReopenAlokasiKlaimReasuransiGrid');
    });
}
