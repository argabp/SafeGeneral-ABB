$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#PenyelesaianKlaimReasuransiForm'));
        setTimeout(function () {
            previewReport('/PenyelesaianKlaimReasuransi/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#PenyelesaianKlaimReasuransiForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/PenyelesaianKlaimReasuransi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#PenyelesaianKlaimReasuransiForm'));
        }
    );
}
