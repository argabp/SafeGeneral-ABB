$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#LaporanBulananTreatyForm'));
        setTimeout(function () {
            previewReport('/LaporanBulananTreaty/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#LaporanBulananTreatyForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/LaporanBulananTreaty.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#LaporanBulananTreatyForm'));
        }
    );
}

function OnPershAsuransiChange(e){
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_cb: $("#kd_cb").val(), kd_grp_pas : e.sender._cascadedValue});
}

function dataKodePasDropDown(){
    return {
        kd_grp: $("#kd_grp_pas").val().trim(),
        kd_cb: $("#kd_cb").val().trim(),
    }
}