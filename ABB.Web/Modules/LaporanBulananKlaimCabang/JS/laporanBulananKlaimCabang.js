$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#LaporanBulananKlaimCabangForm'));
        setTimeout(function () {
            previewReport('/LaporanBulananKlaimCabang/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#LaporanBulananKlaimCabangForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/LaporanBulananKlaimCabang.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#LaporanBulananKlaimCabangForm'));
        }
    );
}