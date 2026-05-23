$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#CetakRekapitulasiProduksiTreatyXOLKeluarForm'));
        setTimeout(function () {
            previewReport('/CetakRekapitulasiProduksiTreatyXOLKeluar/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#CetakRekapitulasiProduksiTreatyXOLKeluarForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakRekapitulasiProduksiTreatyXOLKeluar.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#CetakRekapitulasiProduksiTreatyXOLKeluarForm'));
        }
    );
}