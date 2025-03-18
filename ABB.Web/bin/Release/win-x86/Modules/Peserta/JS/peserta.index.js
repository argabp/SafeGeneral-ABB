$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddPeserta_Click();
});

function openPesertaWindow(url, title) {
    openWindow('#PesertaWindow', url, title);
}

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#PesertaGrid");
    });
}

function btnAddPeserta_Click() {
    $('#btnAddPeserta').click(function () {
        openPesertaWindow('/Peserta/Add', 'Add New');
    });
}

function btnEditPeserta_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPesertaWindow(`/Peserta/Edit?kd_cb=${dataItem.kd_cb}&kd_product=${dataItem.kd_product}&kd_thn=${dataItem.kd_thn}&kd_rk=${dataItem.kd_rk}&no_sppa=${dataItem.no_sppa}&no_updt=${dataItem.no_updt}`, 'Edit');
}

function btnDeletePeserta_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Confirmation', `Are you sure you want to delete ${dataItem.nomor_sppa} ?`,
        function () {
            showProgressOnGrid('#PesertaGrid');
            deletePeserta(dataItem);
        }
    );
}

function btnProcessPeserta_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Confirmation', `Are you sure you want to process ${dataItem.nomor_sppa} ?`,
        function () {
            showProgressOnGrid('#PesertaGrid');
            processPeserta(dataItem);
        }
    );
}

function deletePeserta(dataItem) {
    var url = `/Peserta/DeletePeserta?kd_cb=${dataItem.kd_cb}&kd_product=${dataItem.kd_product}&kd_thn=${dataItem.kd_thn}&kd_rk=${dataItem.kd_rk}&no_sppa=${dataItem.no_sppa}&no_updt=${dataItem.no_updt}`;
    ajaxGet(url,  function (response) {
        if (response.Result == "OK") {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#PesertaGrid");
            closeProgressOnGrid('#PesertaGrid');
        } else
            showMessage('Error', response.Message);
        closeProgressOnGrid('#PesertaGrid');
    }, AjaxContentType.URLENCODED);
}

function processPeserta(dataItem){
    var url = "/Peserta/Process";
    var data = {}

    data.kd_cb = dataItem.kd_cb;
    data.kd_product = dataItem.kd_product;
    data.no_sppa = dataItem.no_sppa;
    data.kd_rk = dataItem.kd_rk;
    data.kd_thn = dataItem.kd_thn;
    data.no_updt = dataItem.no_updt;
    data.kd_user_status = dataItem.kd_user_status;
    data.kd_approval = dataItem.kd_approval;
    data.keterangan = "Pengajuan Akseptasi sudah kami lengkapi, Mohon Persetujuan Pihak Asuransi";

    var json = JSON.stringify(data);
    
    ajaxPost(url, json,
        function (response) {
            $('#pesertaLampiranDS').val(JSON.stringify(response));
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#PesertaGrid");
            }
            else
                showMessage('Error', response.Message);

            closeProgressOnGrid('#PesertaGrid');
        }
    );
}