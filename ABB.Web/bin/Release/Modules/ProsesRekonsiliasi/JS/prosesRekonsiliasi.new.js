$(document).ready(function () {
    var dataItem;
});

var gridName = "NewProsesRekonsiliasiGrid";

function OnClickPayment(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Payment', `Are you sure you want to payment ${dataItem.nomor_sppa}?`,
        function () {
            showProgressOnGrid('#NewProsesRekonsiliasiGrid');
            payment(dataItem);
        }
    );
}

function OnClickCancel(e) {
    e.preventDefault();
    dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#CancelWindow', `/ProsesRekonsiliasi/CancelPopUp`, 'Cancel');
}

function payment(dataItem){
    var url = "/ProsesRekonsiliasi/Payment";
    var data = {}

    data.kd_cb = dataItem.kd_cb;
    data.kd_product = dataItem.kd_product;
    data.no_sppa = dataItem.no_sppa;
    data.kd_rk = dataItem.kd_rk;
    data.kd_thn = dataItem.kd_thn;
    data.no_updt = dataItem.no_updt;
    data.kd_user_status = dataItem.kd_user_status;
    data.kd_approval = dataItem.kd_approval;
    data.kd_action = 19;
    data.nomor_sppa = dataItem.nomor_sppa;

    var json = JSON.stringify(data);

    ajaxPost(url, json,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#NewProsesRekonsiliasiGrid");
            }
            else
                showMessage('Error', response.Message);

            closeProgressOnGrid('#NewProsesRekonsiliasiGrid');
        }
    );
}