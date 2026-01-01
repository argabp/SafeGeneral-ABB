$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
    btnAddPengajuanAkseptasi_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#PengajuanAkseptasiGrid");
    });
}

function openPengajuanAkseptasiWindow(url, title) {
    openWindow('#PengajuanAkseptasiWindow', url, title);
}

function btnAddPengajuanAkseptasi_Click() {
    $('#btnAddNewPengajuanAkseptasi').click(function () {
        openPengajuanAkseptasiWindow('/PengajuanAkseptasi/Add', 'Add');
    });
}

function OnClickEditPengajuanAkseptasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPengajuanAkseptasiWindow(`/PengajuanAkseptasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, 'Edit');
}

function OnClickViewPengajuanAkseptasi(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPengajuanAkseptasiWindow(`/PengajuanAkseptasi/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, 'View');
}

function OnClickInfoPengajuanAkseptasi(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoPengajuanAkseptasiWindow',`/PengajuanAkseptasi/Info`, 'Info');
}

function OnClickSubmitPengajuanAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/PengajuanAkseptasi/Submit`, 'Submit');
}

function OnClickBatalAkseptasiPengajuanAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/PengajuanAkseptasi/BatalAkseptasi`, 'Batal Akseptasi');
}

function OnClickBandingPengajuanAkseptasi(e) {
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    openWindow('#ApprovalWindow',`/PengajuanAkseptasi/Banding`, 'Banding');
}

function OnClickPrintPengajuanAkseptasi(e) {
    showProgressOnGrid('#PengajuanAkseptasiGrid');
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    
    var data = {
        kd_cb: dataItem.kd_cb,
        kd_cob: dataItem.kd_cob,
        kd_scob: dataItem.kd_scob,
        kd_thn: dataItem.kd_thn,
        no_aks: dataItem.no_aks,
    }
    
    ajaxPost("/PengajuanAkseptasi/GenerateReport", JSON.stringify(data),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/PengajuanAkseptasi.pdf",  '_blank');
                window.open("/Reports/" + response.Data + "/KeteranganPengajuanAkseptasi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#PengajuanAkseptasiGrid');
        },
    );
}

function setButtonActions(e){
    var grid = this;
    var userLogin = $("#UserLogin").val();

    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            // Apply your business logic to hide buttons
            if(userLogin != dataItem.kd_user_input){
                buttonContainer.find(".k-grid-Edit, .k-grid-Submit, .k-grid-Delete").hide();
            }

            if(userLogin != dataItem.kd_user_status){
                buttonContainer.find(".k-grid-Edit, .k-grid-Submit").hide();
            }

            if (dataItem.status !== "New" && dataItem.status !== "Revised") {
                buttonContainer.find(".k-grid-Edit, .k-grid-Submit").hide();
            }

            if (dataItem.status !== "New") {
                buttonContainer.find(".k-grid-Delete").hide();
            }

            if(dataItem.flag_banding != "1" || dataItem.flag_closing != "N" || userLogin != dataItem.kd_user_input){
                buttonContainer.find(".k-grid-Banding").hide();
            }
        }
    });

    gridAutoFit(grid);
}

function OnClickDeletPengajuanAkseptasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi?`,
        function () {
            showProgressOnGrid('#PengajuanAkseptasiGrid');
            setTimeout(function () { deleteAkseptasi(dataItem); }, 500);
        }
    );
}

function deleteAkseptasi(dataItem) {
    ajaxGet(`/PengajuanAkseptasi/Delete?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
        }
        else {
            showMessage('Error', 'Delete data is failed, this data is already used');
        }
        
        refreshGrid("#PengajuanAkseptasiGrid");

        closeProgressOnGrid('#PengajuanAkseptasiGrid');
    }, AjaxContentType.URLENCODED);
}