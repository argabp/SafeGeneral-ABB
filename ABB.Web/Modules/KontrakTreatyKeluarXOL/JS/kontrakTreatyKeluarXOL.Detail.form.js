$(document).ready(async function () {
    btnSaveDetailKontrakTreatyKeluarXOL_Click();
});

function btnSaveDetailKontrakTreatyKeluarXOL_Click() {
    $('#btn-save-detailKontrakTreatyKeluarXOL').click(function () {
        showProgress('#KontrakTreatyKeluarXOLWindow');
        setTimeout(function () {
            saveDetailKontrakTreatyKeluarXOL()
        }, 500);
    });
}

function saveDetailKontrakTreatyKeluarXOL(){
    var grid = $("#DetailKontrakTreatyKeluarXOLGrid").data("kendoGrid");
    
    var kd_cb = $("#kd_cb").val().trim();
    var kd_jns_sor = $("#kd_jns_sor").val().trim();
    var kd_tty_npps = $("#kd_tty_npps").val().trim();
    var kd_grp_pas = $("#kd_grp_pas").val().trim();
    var kd_rk_pas = $("#kd_rk_pas").val().trim();
    
    var id = kd_cb + kd_jns_sor + kd_tty_npps + kd_grp_pas + kd_rk_pas;
    var dataItem = grid.dataSource.get(id);

    // Get DropDownList Instances to grab the Text
    var ddlGrpPas = $("#kd_grp_pas").data("kendoDropDownList");
    var ddlRkPas = $("#kd_rk_pas").data("kendoDropDownList");
    var ddlGrpSb = $("#kd_grp_sb_bis").data("kendoDropDownList");
    var ddlRkSb = $("#kd_rk_sb_bis").data("kendoDropDownList");

    // Capture Values and Text
    var detailData = {
        Id: id,
        kd_grp_pas: ddlGrpPas.value(),
        nm_grp_pas: ddlGrpPas.text(), // Captured Text
        kd_rk_pas: ddlRkPas.value(),
        nm_rk_pas: ddlRkPas.text(),   // Captured Text
        kd_grp_sb_bis: ddlGrpSb.value(),
        nm_grp_sb_bis: ddlGrpSb.text(), // Captured Text
        kd_rk_sb_bis: ddlRkSb.value(),
        nm_rk_sb_bis: ddlRkSb.text(),   // Captured Text
        pst_share: $("#pst_share").data("kendoNumericTextBox").value(),
        pst_com: $("#pst_com").data("kendoNumericTextBox").value()
    };

    if (dataItem) {
        dataItem.set("kd_grp_sb_bis", detailData.kd_grp_sb_bis);
        dataItem.set("kd_rk_sb_bis", detailData.kd_rk_sb_bis);
        dataItem.set("pst_share", detailData.pst_share);
        dataItem.set("pst_com", detailData.pst_com);
    } else {
        grid.dataSource.add(detailData);
    }
    
    // Recalculate totals on the main form
    OnDetailKontrakTreatyKeluarXOLDataBound({ sender: grid });

    closeProgress('#KontrakTreatyKeluarXOLWindow');
    // Close only the second window
    closeWindow("#DetailKontrakTreatyKeluarXOLWindow");
}
