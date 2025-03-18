var asumsiPeriode;

function onAsumsiPeriodeChange(e){
    var grid = e.sender;
    asumsiPeriode = grid.dataItem(this.select());;
    refreshGrid("#grid_Detail" + asumsiPeriode.KodeAsumsi);
}

function getAsumsiDetailParam(){
    return {
        kodeAsumsi: asumsiPeriode === undefined ? null : asumsiPeriode.KodeAsumsi,
        kodeProduk: asumsiPeriode === undefined ? null : asumsiPeriode.KodeProduk,
        periodeProses: asumsiPeriode === undefined ? null : asumsiPeriode.PeriodeProses,
    }
}

function editKodeAsumsi(dataItem){
    return !!dataItem.isNew();
}

function editReadOnlyField(dataItem){
    return false;
}

function editKodeProduk(dataItem){
    return !!dataItem.isNew();
}

function editPeriodeProses(dataItem){
    return !!dataItem.isNew();
}

function editTahun(dataItem){
    return !!dataItem.isNew();
}

function onSaveAsumsi(dataItem){    
    var url = dataItem.model.isNew() ? "/Asumsi/AddAsumsi" : "/Asumsi/EditAsumsi";

    var data = {
        KodeAsumsi: dataItem.model.KodeAsumsi,
        NamaAsumsi: dataItem.model.NamaAsumsi
    }
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#AsumsiGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteAsumsi(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#AsumsiGrid');
            
            var data = {
                KodeAsumsi: dataItem.KodeAsumsi,
                NamaAsumsi: dataItem.NamaAsumsi
            }

            ajaxPost("/Asumsi/DeleteAsumsi", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#AsumsiGrid");
                    refreshGrid("#AsumsiPeriodeGrid");
                    refreshGrid("#AsumsiDetailGrid");
                    closeProgressOnGrid('#AsumsiGrid');
                }
            );
        }
    );
}

function onEditAsumsiPeriode(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.KodeAsumsi = gridId;
}

function onSaveAsumsiPeriode(dataItem){
    if(!dataItem.model.isNew())
        return;

    var data = {
        KodeAsumsi: dataItem.model.KodeAsumsi,
        KodeProduk: dataItem.model.KodeProduk,
        PeriodeProses: dataItem.model.PeriodeProses.toDateString()
    }

    ajaxPost("/Asumsi/AddAsumsiPeriode", JSON.stringify(data),
        function (response) {
            refreshGrid("#AsumsiPeriodeGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteAsumsiPeriode(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#AsumsiPeriodeGrid');

            var data = {
                KodeAsumsi: dataItem.KodeAsumsi,
                KodeProduk: dataItem.KodeProduk,
                PeriodeProses: dataItem.PeriodeProses.toDateString()
            }

            ajaxPost("/Asumsi/DeleteAsumsiPeriode", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#AsumsiPeriodeGrid");
                    refreshGrid("#AsumsiDetailGrid");
                    closeProgressOnGrid('#AsumsiPeriodeGrid');
                }
            );
        }
    );
}

function onEditAsumsiDetail(dataItem){
    if(dataItem.model.isNew()){
        dataItem.model.KodeAsumsi = asumsiPeriode.KodeAsumsi;
        dataItem.model.KodeProduk = asumsiPeriode.KodeProduk;
        dataItem.model.PeriodeProses = asumsiPeriode.PeriodeProses;
    }
}

function onSaveAsumsiDetail(dataItem){
    var url = dataItem.model.isNew() ? "/Asumsi/AddAsumsiDetail" : "/Asumsi/EditAsumsiDetail";

    var data = {
        KodeAsumsi: dataItem.model.KodeAsumsi,
        KodeProduk: dataItem.model.KodeProduk,
        PeriodeProses: dataItem.model.PeriodeProses.toDateString(),
        Thn: dataItem.model.Thn,
        Persentase: dataItem.model.Persentase
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#AsumsiDetailGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeleteAsumsiDetail(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#AsumsiDetailGrid');

            var data = {
                KodeAsumsi: dataItem.KodeAsumsi,
                KodeProduk: dataItem.KodeProduk,
                PeriodeProses: dataItem.PeriodeProses.toDateString(),
                Thn: dataItem.Thn,
                Persentase: dataItem.Persentase
            }

            ajaxPost("/Asumsi/DeleteAsumsiDetail", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#AsumsiDetailGrid");
                    closeProgressOnGrid('#AsumsiDetailGrid');
                }
            );
        }
    );
}