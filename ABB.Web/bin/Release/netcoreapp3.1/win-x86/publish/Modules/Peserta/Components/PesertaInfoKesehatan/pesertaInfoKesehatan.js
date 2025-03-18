$(document).ready(function () {
    btnSaveKesehatan_Click();
    setValueInfoKesehatanEdit();
    btnPreviousPesertaInfoKesehatan();
    btnNextPesertaInfoKesehatan();
});

function btnPreviousPesertaInfoKesehatan(){
    $('#btn-previous-pesertaInfoKesehatan').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(1);
    });
}

function btnNextPesertaInfoKesehatan(){
    $('#btn-next-pesertaInfoKesehatan').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(3);
    });
}

function btnSaveKesehatan_Click() {
    $('#btn-save-pesertaInfoKesehatan').click(function () {
        showProgress('#PesertaWindow');
        setTimeout(function () {
            savePesertaInfoKesehatan('/Peserta/SavePesertaInfoKesehatan')
        }, 500);
    });
}

function savePesertaInfoKesehatan(url) {    
    var form = getFormData($('#PesertaInfoKesehatanForm'));
    var kd_cb = $("#kd_cb").val();
    var kd_product = $("#kd_product").val();
    var no_sppa = $("#no_sppa").val();
    var kd_rk = $("#kd_rk").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();
    var flag_tanya_01 = "";
    var flag_tanya_02 = "";
    var flag_tanya_03 = "";
    var flag_tanya_04 = "";
    var flag_tanya_05 = "";
    var flag_tanya_06 = "";
    var flag_tanya_07 = "";
    var flag_tanya_08 = "";
    var flag_tanya_09 = "";

    if ($("input[name='flag_tanya_01']")[0].checked)
        flag_tanya_01 = "Y"
    else if($("input[name='flag_tanya_01']")[1].checked)
        flag_tanya_01 = "N"

    if ($("input[name='flag_tanya_02']")[0].checked)
        flag_tanya_02 = "Y"
    else if($("input[name='flag_tanya_02']")[1].checked)
        flag_tanya_02 = "N"

    if ($("input[name='flag_tanya_03']")[0].checked)
        flag_tanya_03 = "Y"
    else if($("input[name='flag_tanya_03']")[1].checked)
        flag_tanya_03 = "N"

    if ($("input[name='flag_tanya_04']")[0].checked)
        flag_tanya_04 = "Y"
    else if($("input[name='flag_tanya_04']")[1].checked)
        flag_tanya_04 = "N"

    if ($("input[name='flag_tanya_05']")[0].checked)
        flag_tanya_05 = "Y"
    else if($("input[name='flag_tanya_05']")[1].checked)
        flag_tanya_05 = "N"

    if ($("input[name='flag_tanya_06']")[0].checked)
        flag_tanya_06 = "Y"
    else if($("input[name='flag_tanya_06']")[1].checked)
        flag_tanya_06 = "N"

    if ($("input[name='flag_tanya_07']")[0].checked)
        flag_tanya_07 = "Y"
    else if($("input[name='flag_tanya_07']")[1].checked)
        flag_tanya_07 = "N"

    if ($("input[name='flag_tanya_08']")[0].checked)
        flag_tanya_08 = "Y"
    else if($("input[name='flag_tanya_08']")[1].checked)
        flag_tanya_08 = "N"

    if ($("input[name='flag_tanya_09']")[0].checked)
        flag_tanya_09 = "Y"
    else if($("input[name='flag_tanya_09']")[1].checked)
        flag_tanya_09 = "N"

    form.flag_tanya_01 = flag_tanya_01;
    form.flag_tanya_02 = flag_tanya_02;
    form.flag_tanya_03 = flag_tanya_03;
    form.flag_tanya_04 = flag_tanya_04;
    form.flag_tanya_05 = flag_tanya_05;
    form.flag_tanya_06 = flag_tanya_06;
    form.flag_tanya_07 = flag_tanya_07;
    form.flag_tanya_08 = flag_tanya_08;
    form.flag_tanya_09 = flag_tanya_09;
    form.kd_cb = kd_cb;
    form.kd_product = kd_product;
    form.no_sppa = no_sppa;
    form.kd_rk = kd_rk;
    form.kd_thn = kd_thn;
    form.no_updt = no_updt;

    var data = JSON.stringify(form);
    ajaxPost(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#PesertaGrid");
                
                if(response.Model != undefined)
                    setPesertaModel(response.Model);
            }
            else if (response.Result == "ERROR")
                $("#tabKesehatan").html(response.Message);
            else
                $("#tabKesehatan").html(response);

            closeProgress('#PesertaWindow');
        }
    );
}

function setValueInfoKesehatanEdit(){
    var flag_tanya_01_value = $("#flag_tanya_01_value").val();
    var flag_tanya_02_value = $("#flag_tanya_02_value").val();
    var flag_tanya_03_value = $("#flag_tanya_03_value").val();
    var flag_tanya_04_value = $("#flag_tanya_04_value").val();
    var flag_tanya_05_value = $("#flag_tanya_05_value").val();
    var flag_tanya_06_value = $("#flag_tanya_06_value").val();
    var flag_tanya_07_value = $("#flag_tanya_07_value").val();
    var flag_tanya_08_value = $("#flag_tanya_08_value").val();
    var flag_tanya_09_value = $("#flag_tanya_09_value").val();

    if(flag_tanya_01_value === "Y")
        $("#flag_tanya_01_ya").attr("checked",true)
    else if(flag_tanya_01_value === "N")
        $("#flag_tanya_01_tidak").attr("checked",true)

    if(flag_tanya_02_value === "Y")
        $("#flag_tanya_02_ya").attr("checked",true)
    else if(flag_tanya_02_value === "N")
        $("#flag_tanya_02_tidak").attr("checked",true)

    if(flag_tanya_03_value === "Y")
        $("#flag_tanya_03_ya").attr("checked",true)
    else if(flag_tanya_03_value === "N")
        $("#flag_tanya_03_tidak").attr("checked",true)

    if(flag_tanya_04_value === "Y")
        $("#flag_tanya_04_ya").attr("checked",true)
    else if(flag_tanya_04_value === "N")
        $("#flag_tanya_04_tidak").attr("checked",true)

    if(flag_tanya_05_value === "Y")
        $("#flag_tanya_05_ya").attr("checked",true)
    else if(flag_tanya_05_value === "N")
        $("#flag_tanya_05_tidak").attr("checked",true)

    if(flag_tanya_06_value === "Y")
        $("#flag_tanya_06_ya").attr("checked",true)
    else if(flag_tanya_06_value === "N")
        $("#flag_tanya_06_tidak").attr("checked",true)

    if(flag_tanya_07_value === "Y")
        $("#flag_tanya_07_ya").attr("checked",true)
    else if(flag_tanya_07_value === "N")
        $("#flag_tanya_07_tidak").attr("checked",true)

    if(flag_tanya_08_value === "Y")
        $("#flag_tanya_08_ya").attr("checked",true)
    else if(flag_tanya_08_value === "N")
        $("#flag_tanya_08_tidak").attr("checked",true)

    if(flag_tanya_09_value === "Y")
        $("#flag_tanya_09_ya").attr("checked",true)
    else if(flag_tanya_09_value === "N")
        $("#flag_tanya_09_tidak").attr("checked",true)

}