$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var sorData;
    btnAddAlokasi_Click();
});

let statusFilterApplied = false;

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#SORGrid");
    });
}

function openAlokasiWindow(url, title) {
    openWindow('#AlokasiWindow', url, title);
}

function btnAddAlokasi_Click() {
    $('#btnAddNewAlokasi').click(function () {
        openAlokasiWindow('/Alokasi/Add', 'Add');
    });
}

function OnClickEditAlokasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    sorData = dataItem;
    console.log('dataItem', dataItem);
    openAlokasiWindow(`/Alokasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}
                                    &no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                                    &no_updt_reas=${dataItem.no_updt_reas}`, 'Edit');
}

function OnClickViewAlokasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    sorData = dataItem;
    console.log('dataItem', dataItem);
    openAlokasiWindow(`/Alokasi/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}
                                    &no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                                    &no_updt_reas=${dataItem.no_updt_reas}`, 'View');
}

function OnClickDeleteAlokasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Klaim?`,
        function () {
            showProgressOnGrid('#SORGrid');
            setTimeout(function () { deleteAlokasi(dataItem); }, 500);
        }
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
            // if(userLogin != dataItem.kd_usr_input){
            //     buttonContainer.find(".k-grid-Edit").hide();
            //     buttonContainer.find(".k-grid-Delete").hide();
            // }
            
            if(dataItem.flag_closing == "Y"){
                buttonContainer.find(".k-grid-Edit").hide();
            } else {
                buttonContainer.find(".k-grid-View").hide();
            }
        }
    });

    gridAutoFit(grid);
}

function deleteAlokasi(dataItem) {
    ajaxGet(`/Alokasi/EditAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}
                                    &no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                                    &no_updt_reas=${dataItem.no_updt_reas}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
        }
        else {
            showMessage('Error', 'Delete data is failed, this data is already used');
        }

        refreshGrid("#SORGrid");

        closeProgressOnGrid('#SORGrid');
    }, AjaxContentType.URLENCODED);
}