$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#CetakNotaPremiTreatyKeluarForm'));
        setTimeout(function () {
            previewReport('/CetakNotaPremiTreatyKeluar/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#CetakNotaPremiTreatyKeluarForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakNotaPremiTreatyKeluar.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#CetakNotaPremiTreatyKeluarForm'));
        }
    );
}