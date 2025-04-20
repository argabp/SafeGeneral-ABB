$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#LaporanBulananCabangForm'));
        setTimeout(function () {
            previewReport('/LaporanBulananCabang/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#LaporanBulananCabangForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/LaporanBulananCabang.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#LaporanBulananCabangForm'));
        }
    );
}