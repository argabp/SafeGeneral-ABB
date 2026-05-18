$(document).ready(function () {
    btnPreview_Click();
});

function btnPreview_Click() {
    $('#btn-preview').click(function () {
        showProgressByElement($('#CetakSOATreatyMasukForm'));
        setTimeout(function () {
            previewReport('/CetakSOATreatyMasuk/GenerateReport')
        }, 500);
    });
}

function previewReport(url) {
    var form = getFormData($('#CetakSOATreatyMasukForm'));

    var data = JSON.stringify(form);

    ajaxPost(url, data,
        function (response) {
            if(response.Status === "OK"){
                window.open("/Reports/" + response.Data + "/CetakSOATreatyMasuk.pdf",  '_blank');
            } else {
                showMessage('Error', response.Message);
            }

            closeProgressByElement($('#CetakSOATreatyMasukForm'));
        }
    );
}

function onCabangChange(e){
    var kd_rk_pas = $("#kd_rk_pas").data("kendoDropDownList");
    kd_rk_pas.dataSource.read({kd_cb: e.sender._cascadedValue, kd_grp_pas : $("#kd_grp_pas").val()});
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