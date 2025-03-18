$(document).ready(function () {
});

function editPeriodeProses(dataItem){
    return dataItem.isNew();
}

function onSavePeriodeProses(dataItem){
    var url = ""
    
    if(dataItem.model.isNew())
        url = "/PeriodeProses/AddPeriodeProses";
    else
        url = "/PeriodeProses/EditPeriodeProses"

    var data = {
        PeriodeProses: dataItem.model.PeriodeProses.toDateString(),
        FlagProses: dataItem.model.FlagProses
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#PeriodeProsesGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
        }
    );
}

function onDeletePeriodeProses(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#PeriodeProsesGrid');

            var data = {
                PeriodeProses: dataItem.PeriodeProses.toDateString(),
                FlagProses: dataItem.FlagProses
            }

            ajaxPost("/PeriodeProses/DeletePeriodeProses", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    closeProgressOnGrid('#PeriodeProsesGrid');
                    refreshGrid("#PeriodeProsesGrid");
                }
            );
        }
    );
}