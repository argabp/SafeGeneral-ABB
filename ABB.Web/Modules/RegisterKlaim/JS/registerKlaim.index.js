$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var dataItem;
    btnAddRegisterKlaim_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#RegisterKlaimGrid");
    });
}

function openRegisterKlaimWindow(url, title) {
    openWindow('#RegisterKlaimWindow', url, title);
}

function btnAddRegisterKlaim_Click() {
    $('#btnAddNewRegisterKlaim').click(function () {
        openRegisterKlaimWindow('/RegisterKlaim/Add', 'Add');
    });
}

function OnClickEditRegisterKlaim(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openRegisterKlaimWindow(`/RegisterKlaim/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}`, 'Edit');
}

function OnClickViewRegisterKlaim(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openRegisterKlaimWindow(`/RegisterKlaim/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}`, 'View');
}



function OnClickInfoRegisterKlaim(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#InfoApprovalMutasiKlaimWindow',`/RegisterKlaim/Info`, 'Info');
}

//
// function OnClickSubmitPengajuanAkseptasi(e) {
//     dataItem = this.dataItem($(e.currentTarget).closest("tr"));
//     openWindow('#ApprovalWindow',`/PengajuanAkseptasi/Submit`, 'Submit');
// }
//
// function OnClickBatalAkseptasiPengajuanAkseptasi(e) {
//     dataItem = this.dataItem($(e.currentTarget).closest("tr"));
//     openWindow('#ApprovalWindow',`/PengajuanAkseptasi/BatalAkseptasi`, 'Batal Akseptasi');
// }

function OnClickPrintPengajuanKlaim(e) {
    showProgressOnGrid('#RegisterKlaimGrid');
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    var data = {
        kd_cb: dataItem.kd_cb,
        kd_cob: dataItem.kd_cob,
        kd_scob: dataItem.kd_scob,
        kd_thn: dataItem.kd_thn,
        no_kl: dataItem.no_kl,
    }

    ajaxPost("/RegisterKlaim/GenerateReport", JSON.stringify(data),
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/RegisterKlaim.pdf",  '_blank');
                window.open("/Reports/" + response.Data + "/KeteranganRegisterKlaim.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }
            closeProgressOnGrid('#RegisterKlaimGrid');
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
            if(userLogin != dataItem.kd_usr_input){
                buttonContainer.find(".k-grid-Edit").hide();
            }
        }
    });

    gridAutoFit(grid);
}