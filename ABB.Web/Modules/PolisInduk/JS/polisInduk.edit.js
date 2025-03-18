$(document).ready(function () {
    btnEdit_Click();

    var flag_konv = $("#tempFlag_konv").val();
    flag_konv == "N" ? $("#flag_konv").prop("checked", false) : $("#flag_konv").prop("checked", true);
    
    setTimeout(() => {
        $("#kd_rk_ttg").data("kendoDropDownList").value($("#temp_kd_rk_ttg").val().trim());
        $("#kd_rk_sb_bis").data("kendoDropDownList").value($("#temp_kd_rk_sb_bis").val().trim());
        $("#kd_rk_brk").data("kendoDropDownList").value($("#temp_kd_rk_brk").val().trim());
        $("#kd_rk_pas").data("kendoDropDownList").value($("#temp_kd_rk_pas").val().trim());
        $("#kd_rk_bank").data("kendoDropDownList").value($("#temp_kd_rk_bank").val().trim());
        $("#kd_rk_mkt").data("kendoDropDownList").value($("#temp_kd_rk_mkt").val().trim());
        $("#kd_scob").data("kendoDropDownList").value($("#temp_kd_scob").val().trim());
    }, 500);
});

function btnEdit_Click() {
    $('#btn-edit-polisInduk').click(function () {
        showConfirmation("Confirmation", "Do you want to save the changes?", function () {
            showProgress('#PolisIndukWindow');
            setTimeout(function () {
                savePolisInduk('/PolisInduk/Edit')
            }, 500);
        });
    });
}
