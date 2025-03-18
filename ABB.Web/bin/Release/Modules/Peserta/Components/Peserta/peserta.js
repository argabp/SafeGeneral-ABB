// Restricts input for the set of matched elements to the given inputFilter function.
(function($) {
    $.fn.inputFilter = function(callback, errMsg) {
        return this.on("input keydown keyup mousedown mouseup select contextmenu drop focusout", function(e) {            
            if (callback(this.value)) {
                if (["keydown","mousedown","focusout"].indexOf(e.type) >= 0){
                    $(this).removeClass("input-error");
                    this.setCustomValidity("");
                }
                this.oldValue = this.value;
                this.oldSelectionStart = this.selectionStart;
                this.oldSelectionEnd = this.selectionEnd;
            } else if (this.hasOwnProperty("oldValue")) {
                // Rejected value - restore the previous one
                $(this).addClass("input-error");
                this.setCustomValidity(errMsg);
                this.reportValidity();
                this.value = this.oldValue;
                this.setSelectionRange(this.oldSelectionStart, this.oldSelectionEnd);
            } else {
                // Rejected value - nothing to restore
                this.value = "";
            }
        });
    };
}(jQuery));

$(document).ready(function () {
    btnSavePeserta_Click();
    setValuePesertaEdit();
    btnNextPeserta();
    setDropDownHidden();

    $("#no_ktp").inputFilter(function(value) {
        return /^\d*$/.test(value);    // Allow digits only, using a RegExp
    },"Only digits allowed");
    
    $("#nomor_sppa_hidden").val($("#nomor_sppa").val().replaceAll("/", ""));
});

function btnNextPeserta(){
    $('#btn-next-peserta').click(function () {
        $("#pesertaTab").getKendoTabStrip().select(1);
    });
}

function btnSavePeserta_Click() {
    $('#btn-save-peserta').click(function () {
        showProgress('#PesertaWindow');
        setTimeout(function () {
            savePeserta('/Peserta/SavePeserta')
        }, 500);
    });
}

function savePeserta(url) {
    var form = getFormData($('#PesertaForm'));
    var kd_cb = $("#kd_cb").val();
    var kd_product = $("#kd_product").val();
    var no_sppa = $("#no_sppa").val();
    var kd_rk = $("#kd_rk").val();
    var kd_thn = $("#kd_thn").val();
    var no_updt = $("#no_updt").val();
    var jns_kelamin = ""

    if ($("input[name='jns_kelamin']")[0].checked)
        jns_kelamin = "L"
    else if($("input[name='jns_kelamin']")[1].checked)
        jns_kelamin = "P"

    form.jns_kelamin = jns_kelamin;
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

                if(response.Model != undefined) {
                    setPesertaModel(response.Model);
                    refreshTabAsuransi();
                }
                
                $("#btn-next-peserta").prop("disabled", false);
                var tabstrip = $('#pesertaTab').data("kendoTabStrip");
                tabstrip.enable(tabstrip.items()[1]);
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#tabPeserta").html(response);

            closeProgress('#PesertaWindow');
        }
    );
}

function setValuePesertaEdit(){
    var almt_ttg_value = $("#almt_ttg_value").val();
    var jns_kelamnin_value = $("#jns_kelamnin_value").val();

    $("#almt_ttg").getKendoTextArea().value(almt_ttg_value);
    if(jns_kelamnin_value === "L")
        $("#LakiLaki").attr("checked",true)
    else if(jns_kelamnin_value === "P")
        $("#Perempuan").attr("checked",true)
}

function setDropDownHidden(){
    var noSumberPenghasilan = $("#no_sumber_penghasilan").getKendoDropDownList().value();
    
    if(noSumberPenghasilan === "-1")
        $("#divPenghasilanLain").show();
    
    var noPekerjaan = $("#no_pekerjaan").getKendoDropDownList().value();

    if(noPekerjaan === "-1")
        $("#divPekerjaanLain").show();
    
    var noJabatan = $("#no_jabatan").getKendoDropDownList().value();

    if(noJabatan === "-1")
        $("#divJabatanLain").show();
}

function onPenghasilanChange(e)
{
    if(e.sender._cascadedValue === "-1")
        $("#divPenghasilanLain").show();
    else
    {
        $("#sumber_penghasilan_lain").val("");
        $("#divPenghasilanLain").hide();
    }
}

function onPekerjaanChange(e)
{
    if(e.sender._cascadedValue === "-1")
        $("#divPekerjaanLain").show();
    else
    {
        $("#pekerjaan_lain").val("");
        $("#divPekerjaanLain").hide();
    }
}

function onJabatanChange(e)
{
    if(e.sender._cascadedValue === "-1")
        $("#divJabatanLain").show();
    else
    {
        $("#jabatan_lain").val("");
        $("#divJabatanLain").hide();
    }
}

$(document).ready(function () {
    setValueEdit();
});

function setValueEdit(){
    var jns_kelamnin_value = $("#jns_kelamnin_value").val();

    if(jns_kelamnin_value === "L")
        $("#LakiLaki").attr("checked",true)
    else if(jns_kelamnin_value === "P")
        $("#Perempuan").attr("checked",true)
}

function setAge(){
    var birthday = $("#tgl_lahir").val();
    
    ajaxGet("/Peserta/GetUsia?tgl_lahir=" + birthday, (result) => {
        $("#usia").val(result);
    })
}