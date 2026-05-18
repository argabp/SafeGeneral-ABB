$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#CetakRekapitulasiProduksiPremiFakultatifMasukForm'));
        setTimeout(function () {
            previewReport('/CetakRekapitulasiProduksiPremiFakultatifMasuk/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#CetakRekapitulasiProduksiPremiFakultatifMasukForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakRekapitulasiProduksiPremiFakultatifMasuk.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#CetakRekapitulasiProduksiPremiFakultatifMasukForm'));
        }
    );
}