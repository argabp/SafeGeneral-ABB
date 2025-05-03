$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#OutstandingKlaimForm'));
        setTimeout(function () {
            previewReport('/OutstandingKlaim/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#OutstandingKlaimForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/OutstandingKlaim.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#OutstandingKlaimForm'));
        }
    );
}
