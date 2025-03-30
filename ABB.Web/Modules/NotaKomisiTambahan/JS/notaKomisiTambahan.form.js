$(document).ready(function () {
    btnSaveNotaKomisiTambahan_Click();
});

function btnSaveNotaKomisiTambahan_Click() {
    $('#btn-save-notaKomisiTambahan').click(function () {
        showProgress('#NotaKomisiTambahanWindow');
        setTimeout(function () {
            saveNotaKomisiTambahan('/NotaKomisiTambahan/SaveNotaKomisiTambahan')
        }, 500);
    });
}

function saveNotaKomisiTambahan(url){
    var form = getFormData($('#NotaKomisiTambahanForm'));
    form.flag_posting = $("#flag_posting")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#NotaKomisiTambahanGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeProgress("#NotaKomisiTambahanWindow");
                closeWindow("#NotaKomisiTambahanWindow")
            } else {
                showMessage('Error', response.Message);

                closeProgress("#NotaKomisiTambahanWindow");
            }
        }
    );
}

function OnJenisNotaChange(e){
    $("#nomor_nota").val(`${$("#kd_cb").val()}.${$("#jns_tr").val()}.${$("#jns_nt_msk").val()}.${$("#kd_thn").val()}.${$("#kd_bln").val()}.${$("#no_nt_msk").val()}/${e.sender._cascadedValue}.${$("#no_nt_kel").val()}`);
}


function OnKodeTertujuChange(e){
    var kd_rk_ttj = $("#kd_rk_ttj").data("kendoDropDownList");
    kd_rk_ttj.dataSource.read({
        kd_grp_ttj : e.sender._cascadedValue,
        kd_cb: $("#kd_cb").val().trim(),
        kd_cob: $("#kd_cob").val().trim(),
        kd_scob: $("#kd_scob").val().trim(),
        kd_thn: $("#kd_thn").val().trim(),
        no_pol: $("#no_pol").val().trim(),
        no_updt: $("#no_updt").val()
    });
}

function OnNomorAkseptasiChange(e){
    showProgressOnGrid("#NomorAkseptasiGrid");
    
    var selectedRow = this.dataItem(this.select());
    $("#kd_cb").val(selectedRow.kd_cb);
    $("#kd_cob").val(selectedRow.kd_cob);
    $("#kd_scob").val(selectedRow.kd_scob);
    $("#kd_grp_sb_bis").val(selectedRow.kd_grp_sb_bis);
    $("#kd_mtu").getKendoDropDownList().value(selectedRow.kd_mtu);
    $("#kd_rk_sb_bis").val(selectedRow.kd_rk_sb_bis);
    $("#kd_thn").val(selectedRow.kd_thn);
    $("#no_pol_ttg").getKendoTextBox().value(selectedRow.no_pol_ttg);
    $("#no_updt").getKendoNumericTextBox().value(selectedRow.no_updt);
    $("#no_pol").val(selectedRow.no_pol);
    $("#nomor_akseptasi").val(selectedRow.no_akseptasi);
    $("#nomor_nota").val(`${selectedRow.kd_cb}.${$("#jns_tr").val()}.${$("#jns_nt_msk").val()}.${selectedRow.kd_thn}.${$("#kd_bln").val()}.${$("#no_nt_msk").val()}/${$("#jns_nt_kel").val()}.${$("#no_nt_kel").val()}`);
    
    var nilai_nt = $("#nilai_nt").val();
    
    ajaxGet(`/NotaKomisiTambahan/GetTertanggungData?kd_cb=${selectedRow.kd_cb}&kd_cob=${selectedRow.kd_cob}&kd_scob=${selectedRow.kd_scob}
                &kd_thn=${selectedRow.kd_thn}&no_pol=${selectedRow.no_pol}&no_updt=${selectedRow.no_updt}&kd_mtu=${selectedRow.kd_mtu}`, (returnValue) => {

        var kd_rk_ttj = returnValue[5].split(",")[1];
        var kd_grp_ttj = returnValue[2].split(",")[1];
        
        $("#almt_ttj").getKendoTextArea().value(returnValue[0].split(",")[1]);
        $("#kd_grp_sb_bis").val(returnValue[1].split(",")[1]);
        $("#kd_grp_ttj").getKendoDropDownList().value(kd_grp_ttj);
        $("#kd_mtu").getKendoDropDownList().value(returnValue[3].split(",")[1]);
        $("#kd_rk_sb_bis").val(returnValue[4].split(",")[1]);
        $("#kd_rk_ttj").getKendoDropDownList().value(kd_rk_ttj);
        $("#kt_ttj").getKendoTextBox().value(returnValue[6].split(",")[1]);
        $("#nilai_prm").getKendoNumericTextBox().value(returnValue[7].split(",")[1]);
        $("#nm_ttj").getKendoTextBox().value(returnValue[8].split(",")[1]);
        
        ajaxGet(`/NotaKomisiTambahan/GetNilaiPremiAndPercentageNt?kd_cb=${selectedRow.kd_cb}&kd_cob=${selectedRow.kd_cob}&kd_scob=${selectedRow.kd_scob}
                &kd_thn=${selectedRow.kd_thn}&no_pol=${selectedRow.no_pol}&no_updt=${selectedRow.no_updt}&kd_mtu=${selectedRow.kd_mtu}&nilai_nt=${nilai_nt}`, 
            (returnValueFirst) => {
            
            var pst_nt = returnValueFirst.split(",")[1];
            var nilai_prm = returnValueFirst.split(",")[4];
            
            $("#pst_nt").getKendoNumericTextBox().value(pst_nt);
            $("#nilai_prm").getKendoNumericTextBox().value(nilai_prm);
            
            var jns_nt_kel = $("#jns_nt_kel").val();
            
            ajaxGet(`/NotaKomisiTambahan/GetPercentage?jns_nt_kel=${jns_nt_kel}&kd_grp_ttj=${kd_grp_ttj}
                &nilai_nt=${nilai_nt}&kd_rk_ttj=${kd_rk_ttj}`, (returnValueSecond) => {
                if(returnValueSecond[0] != null) {
                    $("#pst_ppn").getKendoNumericTextBox().value(returnValueSecond[0].split(",")[1]);
                    $("#nilai_ppn").getKendoNumericTextBox().value(returnValueSecond[0].split(",")[4]);   
                }
                
                if(returnValueSecond[1] != null) {
                    $("#pst_pph").getKendoNumericTextBox().value(returnValueSecond[1].split(",")[1]);
                    $("#nilai_pph").getKendoNumericTextBox().value(returnValueSecond[1].split(",")[4]);
                }
            });

            ajaxGet(`/NotaKomisiTambahan/GetPstShareAndNilaiPremiReas?kd_cb=${selectedRow.kd_cb}
                &kd_grp_ttj=${kd_grp_ttj}&kd_rk_ttj=${kd_rk_ttj}`, (returnValueSecond) => {
                $("#nm_ttj").getKendoTextBox().value(returnValueSecond.split(",")[1]);
                $("#almt_ttj").getKendoTextArea().value(returnValueSecond.split(",")[4]);
                $("#kt_ttj").getKendoTextBox().value(returnValueSecond.split(",")[7]);
            });

            ajaxGet(`/NotaKomisiTambahan/GetNilaiAndPercentage?kd_mtu=${selectedRow.kd_mtu}
                &tgl_nt=${$("#tgl_nt").val()}&pst_nt=${pst_nt}&nilai_nt=${nilai_nt}
                &jns_nt_kel=${jns_nt_kel}&kd_grp_ttj=${kd_grp_ttj}&uraian=${$("#uraian").val()}
                &kd_cb=${selectedRow.kd_cb}&kd_rk_ttj=${kd_rk_ttj}`, (returnValueSecond) => {
                if(returnValueSecond.length === 1){
                    $("#uraian").val(returnValue[0].split(",")[1]);
                } else {
                    $("#nilai_nt").getKendoNumericTextBox().value(returnValue[0].split(",")[1]);
                    $("#pst_ppn").getKendoNumericTextBox().value(returnValue[1].split(",")[1]);
                    $("#pst_pph").getKendoNumericTextBox().value(returnValue[2].split(",")[1]);
                    $("#nilai_ppn").getKendoNumericTextBox().value(returnValue[3].split(",")[1]);
                    $("#nilai_pph").getKendoNumericTextBox().value(returnValue[4].split(",")[1]);
                    $("#uraian").val(returnValue[5].split(",")[1]);
                    $("#pst_lain").getKendoNumericTextBox().value(returnValue[6].split(",")[1]);
                    $("#nilai_lain").getKendoNumericTextBox().value(returnValue[7].split(",")[1]);
                }
            });
            
            closeWindow($("#NomorAkseptasiWindow"));
        });
    });
}