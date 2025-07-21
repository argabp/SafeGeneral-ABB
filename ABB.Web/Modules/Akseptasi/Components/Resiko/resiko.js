$(document).ready(function () {
    btnNextResiko();
    searchKeywordResiko_OnKeyUp();
    btnAddAkseptasiResiko_Click();
    btnCopyResiko_OnClick();
    btnCopyEndorsResiko_OnClick();
});

function btnNextResiko(){
    $('#btn-next-akseptasiResiko').click(function () {
        $("#resikoTab").getKendoTabStrip().select(1);
    });
}

function searchKeywordResiko_OnKeyUp() {
    $('#SearchKeywordResiko').keyup(function () {
        refreshGrid("#AkseptasiResikoGrid");
    });
}

function openAkseptasiResikoWindow(url, title) {
    openWindow('#AkseptasiResikoWindow', url, title);
}


function btnAddAkseptasiResiko_Click() {
    $('#btnAddNewAkseptasiResiko').click(function () {
        openAkseptasiResikoWindow(`/Akseptasi/AddResiko?kd_cb=${$("#kd_cb").val()}&kd_cob=${$("#kd_cob").val()}
                                        &kd_scob=${$("#kd_scob").val()}&kd_thn=${$("#kd_thn").val()}
                                        &no_aks=${$("#no_aks").val()}&no_updt=${$("#no_updt").val()}
                                        &tgl_mul_ptg=${$("#tgl_mul_ptg").val()}&tgl_akh_ptg=${$("#tgl_akh_ptg").val()}`, 'Add New Resiko');
    });
}
function btnEditAkseptasiResiko_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openAkseptasiResikoWindow(`/Akseptasi/EditResiko?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}
                                    &no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}`, 'Edit Resiko');
}
function btnDeleteAkseptasiResiko_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Akseptasi Resiko?`,
        function () {
            showProgressOnGrid('#AkseptasiResikoGrid');
            setTimeout(function () { deleteAkseptasiResiko(dataItem); }, 500);
        }
    );
}
function searchFilterResiko() {
    return {
        searchkeyword: $("#SearchKeywordResiko").val(),
        kd_cb: $("#kd_cb").val(),
        kd_cob: $("#kd_cob").val(),
        kd_scob: $("#kd_scob").val(),
        kd_thn: $("#kd_thn").val(),
        no_aks: $("#no_aks").val(),
        no_updt: $("#no_updt").val(),
    }
}

function deleteAkseptasiResiko(dataItem) {
    ajaxGet(`/Akseptasi/DeleteResiko?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_aks=${dataItem.no_aks}&no_updt=${dataItem.no_updt}&no_rsk=${dataItem.no_rsk}&kd_endt=${dataItem.kd_endt}`, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#AkseptasiResikoGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#AkseptasiResikoGrid");
        }
        else
            $("#AkseptasiResikoWindow").html(response);

        closeProgressOnGrid('#AkseptasiResikoGrid');
    }, AjaxContentType.URLENCODED);
}


var resiko;

function OnResikoChange(e){
    var grid = e.sender;
    resiko = grid.dataItem(this.select());
    
    if(resiko.no_updt === 1){
        $("#btnCopyEndorsResiko").show();
    } else {
        $("#btnCopyEndorsResiko").hide();
    }

    var tabstrip = $('#resikoTab').data("kendoTabStrip");
    if(resiko.kd_endt === "I")
    {
        // refreshGrid("#AkseptasiCoverageGrid");
        // refreshGrid("#AkseptasiObyekGrid");
        // refreshGrid("#AlokasiGrid");
        // refreshTabOther();
        tabstrip.enable(tabstrip.items()[1]);
        tabstrip.enable(tabstrip.items()[2]);
        tabstrip.enable(tabstrip.items()[3]);
        tabstrip.enable(tabstrip.items()[4]);
    } else {
        tabstrip.disable(tabstrip.items()[1]);
        tabstrip.disable(tabstrip.items()[2]);
        tabstrip.disable(tabstrip.items()[3]);
        tabstrip.disable(tabstrip.items()[4]);
    }
}

function refreshTabOther(){
    var kd_cb = $("#kd_cb").val();
    var kd_cob = $("#kd_cob").val();
    var kd_scob = $("#kd_scob").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();
    var no_aks = $("#no_aks").val();
    var no_rsk = resiko?.no_rsk;
    var kd_endt = resiko?.kd_endt;

    var form = {};

    form.kd_cb = kd_cb;
    form.kd_cob = kd_cob;
    form.kd_scob = kd_scob;
    form.no_updt = no_updt;
    form.kd_thn = kd_thn;
    form.no_aks = no_aks;
    form.no_rsk = no_rsk;
    form.kd_endt = kd_endt;

    var data = JSON.stringify(form);

    ajaxPost("/Akseptasi/GetResikoOther", data,
        function (response) {
            $("#tabOther").html(response);
        }
    );
}

function btnCopyResiko_OnClick() {
    $('#btnCopyResiko').click(function () {
        showConfirmation('Confirmation', `Apakah anda yakin akan meng copy resiko ini??`,
            function () {
                if(resiko == null){
                    showMessage("Error", "Please select resiko first");
                    return;
                }
                showProgressOnGrid('#AkseptasiResikoGrid');
                setTimeout(function () { copyResiko(); }, 500);
            }
        );
    });
}

function copyResiko() {
    var data ={
        kd_cb: resiko.kd_cb,
        kd_cob: resiko.kd_cob,
        kd_scob: resiko.kd_scob,
        kd_thn: resiko.kd_thn,
        no_aks: resiko.no_aks,
        no_updt: resiko.no_updt,
        no_rsk: resiko.no_rsk,
        kd_endt: resiko.kd_endt,
    }
    
    ajaxPost(`/Akseptasi/CopyResiko`, data, function (response) {
        if (response.Result) {
            showMessage('Success', response.Message);
            refreshGrid("#AkseptasiResikoGrid");
        }
        else if (!response.Result) {
            showMessage('Error', response.Message);
            refreshGrid("#AkseptasiResikoGrid");
        }
        else
            $("#AkseptasiWindow").html(response);

        closeProgressOnGrid('#AkseptasiResikoGrid');
    }, AjaxContentType.URLENCODED);
}

function openCopyEndorsWindow(url, title) {
    openWindow('#CopyEndorsWindow', url, title);
}

function btnCopyEndorsResiko_OnClick() {
    $('#btnCopyEndorsResiko').click(function () {
        openCopyEndorsWindow("/Akseptasi/CopyEndors", "Copy Endors")
    });
}