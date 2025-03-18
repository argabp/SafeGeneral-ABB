var kabupaten;
var provinsi;
var kecamatan;

function onKabupatenChange(e){
    var grid = e.sender.dataItem(this.select());
    kabupaten = grid.kd_kab;
    provinsi = grid.kd_prop;
    refreshGrid("#grid_Kecamatan_" + provinsi);
}

function getKecamatanParam(){
    return {
        kd_prop: provinsi === undefined ? null : provinsi,
        kd_kab: kabupaten === undefined ? null : kabupaten,
    }
}

function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}

function onSaveProvinsi(dataItem){    
    var url = "/Lokasi/SaveProvinsi";

    var data = {
        kd_prop: dataItem.model.kd_prop,
        nm_prop: dataItem.model.nm_prop,
        no_pos: dataItem.model.no_pos,
        kd_wilayah: dataItem.model.kd_wilayah
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#ProvinsiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteProvinsi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#ProvinsiGrid');
            
            var data = {
                kd_prop: dataItem.kd_prop
            }

            ajaxPost("/Lokasi/DeleteProvinsi", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#ProvinsiGrid");
                    refreshGrid("#grid_Kabupaten_" + data.kd_prop);
                    refreshGrid("#grid_Kecamatan_" + data.kd_prop);
                    refreshGrid("#grid_Kelurahan_" + data.kd_prop);
                    closeProgressOnGrid('#ProvinsiGrid');
                }
            );
        }
    );
}

function getKelurahanParam(){
    return {
        kd_prop: provinsi === undefined ? null : provinsi,
        kd_kab: kabupaten === undefined ? null : kabupaten,
        kd_kec: kecamatan === undefined ? null : kecamatan
    }
}

function onEditKabupaten(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_prop = gridId;
}

function onSaveKabupaten(dataItem){
    var data = {
        kd_prop: dataItem.model.kd_prop,
        kd_kab: dataItem.model.kd_kab,
        nm_kab: dataItem.model.nm_kab
    }

    ajaxPost("/Lokasi/SaveKabupaten", JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Kabupaten_" + data.kd_prop);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteKabupaten(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#grid_Kabupaten_" + dataItem.kd_prop);

            var data = {
                kd_prop: dataItem.kd_prop,
                kd_kab: dataItem.kd_kab
            }

            ajaxPost("/Lokasi/DeleteKabupaten", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_Kabupaten_" + data.kd_prop);
                    refreshGrid("#grid_Kecamatan_" + data.kd_prop);
                    refreshGrid("#grid_Kelurahan_" + data.kd_prop);
                    closeProgressOnGrid("#grid_Kabupaten_" + data.kd_prop);
                }
            );
        }
    );
}

//Kecamatan

function onEditKecamatan(dataItem){
    if(dataItem.model.isNew()){
        dataItem.model.kd_prop = provinsi;
        dataItem.model.kd_kab = kabupaten;
    }
}

function onKecamatanChange(e){
    var grid = e.sender.dataItem(this.select());
    kecamatan = grid.kd_kec;
    refreshGrid("#grid_Kelurahan_" + provinsi);
}

function onSaveKecamatan(dataItem){
    var url = "/Lokasi/SaveKecamatan";

    var data = {
        kd_prop: dataItem.model.kd_prop,
        kd_kab: dataItem.model.kd_kab,
        kd_kec: dataItem.model.kd_kec,
        nm_kec: dataItem.model.nm_kec
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Kecamatan_" + provinsi);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteKecamatan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#grid_Kecamatan_" + provinsi);

            var data = {
                kd_prop: dataItem.kd_prop,
                kd_kab: dataItem.kd_kab,
                kd_kec: dataItem.kd_kec
            }

            ajaxPost("/Lokasi/DeleteKecamatan", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#grid_Kecamatan_" + provinsi);
                    closeProgressOnGrid("#grid_Kecamatan_" + provinsi);
                }
            );
        }
    );
}


function onEditKelurahan(dataItem){
    if(dataItem.model.isNew()){
        dataItem.model.kd_prop = provinsi;
        dataItem.model.kd_kab = kabupaten;
        dataItem.model.kd_kec = kecamatan;
    }
}

function onSaveKelurahan(dataItem){
    var url = "/Lokasi/SaveKelurahan";

    var data = {
        kd_prop: dataItem.model.kd_prop,
        kd_kab: dataItem.model.kd_kab,
        kd_kec: dataItem.model.kd_kec,
        kd_kel: dataItem.model.kd_kel,
        nm_kel: dataItem.model.nm_kel
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Kelurahan_" + provinsi);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteKelurahan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#grid_Kelurahan_" + provinsi);

            var data = {
                kd_prop: dataItem.kd_prop,
                kd_kab: dataItem.kd_kab,
                kd_kec: dataItem.kd_kec,
                kd_kel: dataItem.kd_kel
            }

            ajaxPost("/Lokasi/DeleteKelurahan", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_Kelurahan_" + provinsi);
                    closeProgressOnGrid("#grid_Kelurahan_" + provinsi);
                }
            );
        }
    );
}

function btnAddDetailLokasiResiko_OnClick(kd_pos) {
    openWindow('#DetailLokasiResikoWindow', `/Lokasi/AddDetailLokasiResikoView?kd_pos=${kd_pos}`, 'Add');
}

function btnEditDetailLokasiResiko_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DetailLokasiResikoWindow', `/Lokasi/EditDetailLokasiResikoView?kd_pos=${dataItem.kd_pos}&kd_lok_rsk=${dataItem.kd_lok_rsk}`, 'Edit');
}

function getProvinsiDropdown(){
    return {
        kd_prop: $("#kd_prop").val()
    }
}

function changeProvinsi(e){
    $("#kd_kab").getKendoDropDownList().dataSource.read();
    $("#kd_kec").getKendoDropDownList().dataSource.read();
    $("#kd_kel").getKendoDropDownList().dataSource.read();
}

function getKabupatenDropdown(){
    return {
        kd_prop: $("#kd_prop").val(),
        kd_kab: $("#kd_kab").val()
    }
}

function changeKabupaten(e){
    $("#kd_kec").getKendoDropDownList().dataSource.read();
    $("#kd_kel").getKendoDropDownList().dataSource.read();
}

function getKecamatanDropdown(){
    return {
        kd_prop: $("#kd_prop").val(),
        kd_kab: $("#kd_kab").val(),
        kd_kec: $("#kd_kec").val()
    }
}

function changeKecamatan(e){
    $("#kd_kel").getKendoDropDownList().dataSource.read();
}


function onSaveLokasiResiko(dataItem){
    var url = "/Lokasi/SaveLokasiResiko";

    var data = {
        kd_pos: dataItem.model.kd_pos,
        jalan: dataItem.model.jalan,
        kota: dataItem.model.kota
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#LokasiResikoGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteLokasiResiko(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#LokasiResikoGrid");

            var data = {
                kd_pos: dataItem.kd_pos,
                jalan: dataItem.jalan,
                kota: dataItem.kota
            }

            ajaxPost("/Lokasi/DeleteLokasiResiko", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#LokasiResikoGrid");
                    closeProgressOnGrid("#LokasiResikoGrid");
                }
            );
        }
    );
}


function onEditDetailLokasiResiko(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_pos = gridId;
}

function onSaveDetailLokasiResiko(){
    var url = "/Lokasi/SaveDetailLokasiResiko";
    showProgress('#DetailLokasiResikoWindow');

    var data = {
        kd_pos: $("#kd_pos").val(),
        kd_lok_rsk: $("#kd_lok_rsk").val(),
        gedung: $("#gedung").val(),
        alamat: $("#alamat").val(),
        kd_prop: $("#kd_prop").val(),
        kd_kab: $("#kd_kab").val(),
        kd_kec: $("#kd_kec").val(),
        kd_kel: $("#kd_kel").val(),
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_LokasiResiko_" + data.kd_pos);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }

            closeProgress('#DetailLokasiResikoWindow');
            closeWindow("#DetailLokasiResikoWindow");
        }
    );
}

function onDeleteDetailLokasiResiko(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_pos: dataItem.kd_pos,
                kd_lok_rsk: dataItem.kd_lok_rsk,
                gedung: dataItem.gedung,
                alamat: dataItem.alamat,
                kd_prop: dataItem.kd_prop,
                kd_kab: dataItem.kd_kab,
                kd_kec: dataItem.kd_kec,
                kd_kel: dataItem.kd_kel,
            }

            showProgressOnGrid("#grid_LokasiResiko_" + data.kd_pos);

            ajaxPost("/Lokasi/DeleteDetailLokasiResiko", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_LokasiResiko_" + data.kd_pos);
                    closeProgressOnGrid("#grid_LokasiResiko_" + data.kd_pos);
                }
            );
        }
    );
}