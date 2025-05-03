$(document).ready(function () {
    btnPreview_Click();
});



function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#PenyelesaianKlaimForm'));
        setTimeout(function () {
            previewReport('/PenyelesaianKlaim/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#PenyelesaianKlaimForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/PenyelesaianKlaim.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#PenyelesaianKlaimForm'));
        }
    );
}