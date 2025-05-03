$(document).ready(function () {
    btnPreview_Click();
});



function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#BukuKerugianForm'));
        setTimeout(function () {
            previewReport('/BukuKerugian/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#BukuKerugianForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/BukuKerugian.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#BukuKerugianForm'));
        }
    );
}