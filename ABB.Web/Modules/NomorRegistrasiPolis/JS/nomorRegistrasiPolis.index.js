function openNomorRegistrasiPolisWindow(url, title) {
    openWindow('#NomorRegistrasiPolisWindow', url, title);
}

function onEditNomorRegistrasiPolis(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openNomorRegistrasiPolisWindow(`/NomorRegistrasiPolis/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}&kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}&no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}`, 'Edit Nomor Registrasi Polis');
}
