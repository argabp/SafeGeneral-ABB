function setRekananModel(model){
    var objectModel = JSON.parse(model);
    $("#kd_cb_hidden").val(objectModel.kd_cb);
    $("#kd_rk_hidden").val(objectModel.kd_rk);
}

$(document).ready(function () {
    $('#rekananTab').kendoTabStrip();

    var tabstrip = $('#rekananTab').data("kendoTabStrip");
    tabstrip.select(0);
});