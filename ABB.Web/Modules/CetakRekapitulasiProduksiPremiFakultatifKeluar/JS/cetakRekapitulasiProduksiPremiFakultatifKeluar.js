$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#CetakRekapitulasiProduksiPremiFakultatifKeluarForm'));
        setTimeout(function () {
            previewReport('/CetakRekapitulasiProduksiPremiFakultatifKeluar/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#CetakRekapitulasiProduksiPremiFakultatifKeluarForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakRekapitulasiProduksiPremiFakultatifKeluar.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#CetakRekapitulasiProduksiPremiFakultatifKeluarForm'));
        }
    );
}