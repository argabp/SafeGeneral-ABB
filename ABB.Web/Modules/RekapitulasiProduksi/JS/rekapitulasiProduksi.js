$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#RekapitulasiProduksiForm'));
        setTimeout(function () {
            previewReport('/RekapitulasiProduksi/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#RekapitulasiProduksiForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/RekapitulasiProduksi.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#RekapitulasiProduksiForm'));
        }
    );
}

function OnKodeTertanggungChange(e){
    var kd_rk_ttg = $("#kd_rk_ttg").data("kendoDropDownList");
    kd_rk_ttg.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}