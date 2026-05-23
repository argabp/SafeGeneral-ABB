$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#ListingSpreadingOfRiskForm'));
        setTimeout(function () {
            previewReport('/ListingSpreadingOfRisk/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#ListingSpreadingOfRiskForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/ListingSpreadingOfRisk.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#ListingSpreadingOfRiskForm'));
        }
    );
}
