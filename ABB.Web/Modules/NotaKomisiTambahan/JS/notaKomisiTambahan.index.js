$(document).ready(function () {
    searchKeyword_OnKeyUp();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#NotaKomisiTambahanGrid");
    });
}

function openNotaKomisiTambahanWindow(url, title) {
    openWindow('#NotaKomisiTambahanWindow', url, title);
}

function openNomorAkseptasiWindow(url, title) {
    openWindow('#NomorAkseptasiWindow', url, title);
}

function btnAddNotaKomisiTambahan_OnClick() {
    openNotaKomisiTambahanWindow(`/NotaKomisiTambahan/Add`, 'Add Nota Komisi Tambahan');
}

function onEditNotaKomisiTambahan(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNotaKomisiTambahanWindow(`/NotaKomisiTambahan/Edit?jns_sb_nt=${dataItem.jns_sb_nt}&kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`, 'Edit Nota Komisi Tambahan');
}

function dataKodeTertujuDropDown(){
    return {
        kd_grp_rk: $("#kd_grp_ttj").val().trim(),
    }
}


function onDeleteNotaKomisiTambahan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#NotaKomisiTambahanGrid');

            ajaxGet(`/NotaKomisiTambahan/Delete?jns_sb_nt=${dataItem.jns_sb_nt}&kd_cb=${dataItem.kd_cb}&jns_tr=${dataItem.jns_tr}&jns_nt_msk=${dataItem.jns_nt_msk}&kd_thn=${dataItem.kd_thn}&kd_bln=${dataItem.kd_bln}&no_nt_msk=${dataItem.no_nt_msk}&jns_nt_kel=${dataItem.jns_nt_kel}&no_nt_kel=${dataItem.no_nt_kel}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#NotaKomisiTambahanGrid");
                    closeProgressOnGrid('#NotaKomisiTambahanGrid');
                }
            );
        }
    );
}

function OnSelectAkseptasi(e) {
    openNomorAkseptasiWindow(`/NotaKomisiTambahan/NomorAkseptasi`, 'Nomor Akseptasi');
}
