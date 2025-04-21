$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#LaporanBulananRekapForm'));
        setTimeout(function () {
            previewReport('/LaporanBulananRekap/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#LaporanBulananRekapForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/LaporanBulananRekap.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#LaporanBulananRekapForm'));
        }
    );
}