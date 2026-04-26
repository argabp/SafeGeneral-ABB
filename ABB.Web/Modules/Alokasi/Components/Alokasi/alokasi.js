var alokasi;

$(document).ready(function () {
    btnNextAlokasi();
    searchKeywordAlokasi_OnKeyUp();
    btnAddAlokasi_Click();

    if($("#IsViewOnly").val() == "True") {
        $("#divAddAlokasi").hide();
    } else{
        $("#divAddAlokasi").show();
    }
});

function btnNextAlokasi(){
    $('#btn-next-alokasi').click(function () {
        $("#AlokasiTab").getKendoTabStrip().select(1);
    });
}

function searchKeywordAlokasi_OnKeyUp() {
    $('#SearchKeywordAlokasi').keyup(function () {
        refreshGrid("#AlokasiGrid");
    });
}

function openAlokasiWindow(url, title) {
    openWindow('#FormAlokasiWindow', url, title);
}


function btnAddAlokasi_Click() {
    $('#btnAddNewAlokasi').click(function () {
        openAlokasiWindow(`/Alokasi/AddAlokasi`, 'Add New Alokasi');
    });
}
function btnViewAlokasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAlokasiWindow(`/Alokasi/ViewAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}
                                    &no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                                    &no_updt_reas=${dataItem.no_updt_reas}`, 'View Alokasi');
}
function btnDeleteAlokasi_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Alokasi?`,
        function () {
            showProgressOnGrid('#AlokasiGrid');
            setTimeout(function () { deleteAlokasi(dataItem); }, 500);
        }
    );
}
function searchFilterAlokasi(e) {
    const gridReq = buildGridRequest(e, "SearchKeywordAlokasi");

    return {
        grid: gridReq,
        kd_cb: sorData?.kd_cb,
        kd_cob: sorData?.kd_cob,
        kd_scob: sorData?.kd_scob,
        kd_thn: sorData?.kd_thn,
        no_pol: sorData?.no_pol,
        no_updt: sorData?.no_updt
    };
}

function deleteAlokasi(dataItem) {
    ajaxGet(`/Alokasi/DeleteAlokasi?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}
                &no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}
                &no_updt_reas=${dataItem.no_updt_reas}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AlokasiGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AlokasiGrid");
        }
        else
            $("#FormAlokasiWindow").html(response);

        closeProgressOnGrid('#AlokasiGrid');
    }, AjaxContentType.URLENCODED);
}

function OnAlokasiChange(e) {
    var grid = e.sender;
    alokasi = grid.dataItem(this.select());
    refreshGrid("#DetailAlokasiGrid");
}

function onAlokasiDataBound(e){
    var grid = e.sender;

    grid.tbody.find("tr").each(function(e, element) {
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

            if($("#IsViewOnly").val() == "True") {
                buttonContainer.find(".k-grid-EditAlokasi").hide();
            } else{
                buttonContainer.find(".k-grid-ViewAlokasi").hide();
            }
        }
    });

    gridAutoFit(grid);
}