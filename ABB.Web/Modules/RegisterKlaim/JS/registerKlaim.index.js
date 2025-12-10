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
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openRegisterKlaimWindow(`/RegisterKlaim/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}`, 'Edit');
}

// function OnClickViewRegisterKlaim(e) {
//     e.preventDefault();
//     dataItem = this.dataItem($(e.currentTarget).closest("tr"));
//     console.log('dataItem', dataItem);
//     openRegisterKlaimWindow(`/RegisterKlaim/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_kl=${dataItem.no_kl}`, 'View');
// }
//


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

// function setButtonActions(e){
//     var grid = this;
//
//     // Loop through data items and rows
//     grid.tbody.find("tr").each(function() {
//         var dataItem = grid.dataItem(this);
//        
//         if($("#UserLogin").val() != dataItem.kd_user_input){
//             $(this).find(".k-grid-Edit").hide(); // "custom" is the command name
//             $(this).find(".k-grid-Submit").hide(); // "custom" is the command name
//             $(this).find(".k-grid-Cancel").hide(); // "custom" is the command name
//             $(this).find(".k-grid-BatalAkseptasi").hide(); // "custom" is the command name
//         }
//        
//         if($("#UserLogin").val() != dataItem.kd_user_status){
//             $(this).find(".k-grid-Edit").hide(); // "custom" is the command name
//             $(this).find(".k-grid-Submit").hide(); // "custom" is the command name
//             $(this).find(".k-grid-BatalAkseptasi").hide(); // "custom" is the command name
//         }
//        
//         if (dataItem.status !== "New" && dataItem.status !== "Revised") {
//             // Hide the custom button in this row
//             $(this).find(".k-grid-Edit").hide(); // "custom" is the command name
//             $(this).find(".k-grid-Submit").hide(); // "custom" is the command name
//         }
//        
//         if(dataItem.flag_closing !== "N" && dataItem.status !== "Approved"){
//             // Hide the custom button in this row
//             $(this).find(".k-grid-BatalAkseptasi").hide(); // "custom" is the command name
//         }
//     });
//    
//     gridAutoFit(grid);
// }