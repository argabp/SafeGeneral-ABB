$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#OutstandingKlaimReasuransiForm'));
        setTimeout(function () {
            previewReport('/OutstandingKlaimReasuransi/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#OutstandingKlaimReasuransiForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/OutstandingKlaimReasuransi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#OutstandingKlaimReasuransiForm'));
        }
    );
}
