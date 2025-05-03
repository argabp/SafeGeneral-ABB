$(document).ready(function () {
    btnPreview_Click();
});

function dataKodeTertanggungDropDown(){
    return {
        kd_grp_rk: $("#kd_grp_ttg").val().trim(),
        kd_cb: $("#kd_cb").val().trim()
    }
}

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#LossRecordForm'));
        setTimeout(function () {
            previewReport('/LossRecord/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#LossRecordForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/LossRecord.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#LossRecordForm'));
        }
    );
}

function OnKodeTertanggungChange(e){
    var kd_rk_ttg = $("#kd_rk_ttg").data("kendoDropDownList");
    kd_rk_ttg.dataSource.read({kd_grp_rk : e.sender._cascadedValue, kd_cb: $("#kd_cb").val()});
}

function OnKodeCabangChange(e){
    var kd_rk_ttg = $("#kd_rk_ttg").data("kendoDropDownList");
    kd_rk_ttg.dataSource.read({kd_grp_rk : $("#kd_grp_ttg").val(), kd_cb: e.sender._cascadedValue});
}