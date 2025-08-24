$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddAkseptasi_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#AkseptasiGrid");
    });
}

function openAkseptasiWindow(url, title) {
    openWindow('#AkseptasiWindow', url, title);
}

function btnAddAkseptasi_Click() {
    $('#btnAddNewAkseptasi').click(function () {
        openAkseptasiWindow('/Akseptasi/Add', 'Add New Polis');
        resiko = null;
    });
}

function btnEditAkseptasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    resiko = null;
    openAkseptasiWindow(`/Akseptasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}`, 'Edit Akseptasi');
}

function btnClosingAkseptasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to closing Akseptasi?`,
        function () {        
            showProgressOnGrid('#AkseptasiGrid');
            setTimeout(function () { closingAkseptasi(dataItem); }, 500);
        }
    );
}

function btnKeteranganEndorsment_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiWindow(`/Akseptasi/KeteranganEndorsment?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}`, 'Edit Akseptasi');
}

function closingAkseptasi(dataItem){

    var form = {};

    form.kd_cb = dataItem.kd_cb;
    form.kd_cob = dataItem.kd_cob;
    form.kd_scob = dataItem.kd_scob;
    form.kd_thn = dataItem.kd_thn;
    form.no_aks = dataItem.no_aks;
    form.no_updt = dataItem.no_updt;
    
    var data = JSON.stringify(form);
    ajaxPost(`/Akseptasi/ClosingAkseptasi`, data,  function (response) {
        if (response.Result === "OK") {
            showMessage('Closing Successfully', 'Data has closed');
            refreshGrid("#AkseptasiGrid");
        }
        else {
            showMessage('Error', 'Clossing is failed, ' + response.Message);
            refreshGrid("#AkseptasiGrid");
        };

        closeProgressOnGrid('#AkseptasiGrid');
    });
}

function btnDeleteAkseptasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi?`,
        function () {
            showProgressOnGrid('#AkseptasiGrid');
            setTimeout(function () { deleteAkseptasi(dataItem); }, 500);
        }
    );
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function deleteAkseptasi(dataItem) {
    ajaxGet(`/Akseptasi/Delete?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AkseptasiGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AkseptasiGrid");
        }
        else
            $("#AkseptasiWindow").html(response);

        closeProgressOnGrid('#AkseptasiGrid');
    }, AjaxContentType.URLENCODED);
}

function onAkseptasiDataBound(e) {
    // Check each row for the value of `no_updt`
    var grid = $("#AkseptasiGrid").data("kendoGrid");
    grid.tbody.find("tr").each(function() {
        var dataItem = grid.dataItem(this);
        // Check if the value of no_updt is 1
        if (dataItem.no_updt === 0) {
            // Find the "Closing" button and hide it
            $(this).find("a[title='KeteranganEndorsment']").hide();
        }
    });
}