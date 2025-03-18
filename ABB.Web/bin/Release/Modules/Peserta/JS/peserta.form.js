var state;

function setPesertaModel(model){
    $("#kd_cb").val(model.kd_cb);
    $("#kd_product").val(model.kd_product);
    $("#no_sppa").val(model.no_sppa);
    $("#kd_rk").val(model.kd_rk);
    $("#kd_thn").val(model.kd_thn);
    $("#no_updt").val(model.no_updt);
    $("#nomor_sppa").data("kendoTextBox").value(model.nomor_sppa);
    $("#nomor_sppa_hidden").val(model.nomor_sppa.replaceAll("/", ""));
}

$(document).ready(function () {
    $('#pesertaTab').kendoTabStrip();

    var tabstrip = $('#pesertaTab').data("kendoTabStrip");
    tabstrip.select(0);
});


function refreshGridPesertaLampiran(){
    var kd_cb = $("#kd_cb").val();
    var kd_product = $("#kd_product").val();
    var no_sppa = $("#no_sppa").val();
    var kd_rk = $("#kd_rk").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();

    var form = {};

    form.kd_cb = kd_cb;
    form.kd_product = kd_product;
    form.no_sppa = no_sppa;
    form.kd_rk = kd_rk;
    form.kd_thn = kd_thn;
    form.no_updt = no_updt;

    var data = JSON.stringify(form);

    ajaxPost("/Peserta/GetPesertaLampiran", data,
        function (response) {
            $('#pesertaLampiranDS').val(JSON.stringify(response));
            loadPesertaLampiranDS();
        }
    );
}

function refreshTabAsuransi(){
    var kd_cb = $("#kd_cb").val();
    var kd_product = $("#kd_product").val();
    var no_sppa = $("#no_sppa").val();
    var kd_rk = $("#kd_rk").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();

    var form = {};

    form.kd_cb = kd_cb;
    form.kd_product = kd_product;
    form.no_sppa = no_sppa;
    form.kd_rk = kd_rk;
    form.kd_thn = kd_thn;
    form.no_updt = no_updt;

    var data = JSON.stringify(form);

    ajaxPost("/Peserta/GetPesertaSirama", data,
        function (response) {
            $("#tabAsuransi").html(response);
            if($("#berat_badan").val() == 0)
                $("#btn-next-pesertaSirama").prop("disabled", true);
        }
    );
}

function refreshPesertaInfoKesehatan(){
    var kd_cb = $("#kd_cb").val();
    var kd_product = $("#kd_product").val();
    var no_sppa = $("#no_sppa").val();
    var kd_rk = $("#kd_rk").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();

    var form = {};

    form.kd_cb = kd_cb;
    form.kd_product = kd_product;
    form.no_sppa = no_sppa;
    form.kd_rk = kd_rk;
    form.kd_thn = kd_thn;
    form.no_updt = no_updt;

    var data = JSON.stringify(form);

    ajaxPost("/Peserta/GetPesertaInfoKesehatan", data,
        function (response) {
            $("#tabKesehatan").html(response);

            if($("#berat_badan").val() == 0)
                $("#btn-next-pesertaInfoKesehatan").prop("disabled", true);
        }
    );
}