var selectedData;

function onSaveRekanan(){    
    var url = "/TertanggungPrincipal/SaveRekanan";

    var data = {
        kd_cb: $("#kd_cb").val(),
        kd_grp_rk: $("#kd_grp_rk").val(),
        kd_rk: $("#kd_rk").val(),
        nm_rk: $("#nm_rk").val(),
        flag_sic: $("#flag_sic").val(),
        almt: $("#almt").val(),
        kt: $("#kt").val(),
        no_fax: $("#no_fax_rekanan")[0].checked ? "Y" : "N"
    }
    
    showProgress('#RekananWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#RekananGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#RekananWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#RekananWindow").html(response);

            closeProgress('#RekananWindow');
        }
    );
}

function onDeleteRekanan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#RekananGrid');
            
            var data = {
                kd_cb: dataItem.kd_cb.trim(),
                kd_grp_rk: dataItem.kd_grp_rk,
                kd_rk: dataItem.kd_rk,
            }

            ajaxPost("/TertanggungPrincipal/DeleteRekanan", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#RekananGrid");
                    closeProgressOnGrid('#RekananGrid');
                }
            );
        }
    );
}

function onSaveDetailRekananIndividu(){
    var url = "/TertanggungPrincipal/SaveDetailRekanan";

    var data = {
        kd_cb: selectedData.kd_cb,
        kd_grp_rk: selectedData.kd_grp_rk,
        kd_rk: selectedData.kd_rk,
        ktp_nm: $("#ktp_nm").val(),
        ktp_tempat: $("#ktp_tempat").val(),
        ktp_tgl: $("#ktp_tgl").val(),
        ktp_no: $("#ktp_no").val(),
        ktp_alamat: $("#ktp_alamat").val(),
        ktp_kota: $("#ktp_kota").val(),
        ktp_normh: $("#ktp_normh").val(),
        ktp_rtrw: $("#ktp_rtrw").val(),
        kodepos: $("#kodepos").val(),
        telp: $("#telp").val(),
        hp: $("#hp").val(),
        npwp: $("#npwp").val(),
        kawinflag: $("#kawinflag").val(),
        pekerjaanflag: $("#pekerjaanflag").val(),
        pekerjaanlain: $("#pekerjaanlain").val(),
        jabatan: $("#jabatan").val(),
        usaha: $("#usaha").val(),
        usahathn: $("#usahathn").val(),
        usahabln: $("#usahabln").val(),
        usahaalamat: $("#usahaalamat").val(),
        usahakota: $("#usahakota").val(),
        usahakodepos: $("#usahakodepos").val(),
        usahatelp: $("#usahatelp").val(),
        usahatelpext: $("#usahatelpext").val(),
        usahaflag: $("#usahaflag").val(),
        usahahasilflag: $("#usahahasilflag").val(),
        nopolis1: $("#nopolis1").val(),
        jenispolis1: $("#jenispolis1").val(),
        nopolis2: $("#nopolis2").val(),
        jenispolis2: $("#jenispolis2").val(),
        asuransipolis1: $("#asuransipolis1").val(),
        asuransipolis2: $("#asuransipolis2").val(),
        tujuanpolisflag: $("#tujuanpolisflag").val(),
        tujuanpolislain: $("#tujuanpolislain").val(),
        
        //Corporate
        perusahaaninstitusi: $("#perusahaaninstitusi").val(),
        siup: $("#siup").val(),
        npwpinstitusi: $("#npwpinstitusi").val(),
        siupinstitusi: $("#siupinstitusi").val(),
        tdpinstitusi: $("#tdpinstitusi").val(),
        hukumhaminstitusi: $("#hukumhaminstitusi").val(),
        usahainstitusi: $("#usahainstitusi").val(),
        kotainstitusi: $("#kotainstitusi").val(),
        kodeposinstitusi: $("#kodeposinstitusi").val(),
        telpinstitusi: $("#telpinstitusi").val(),
        telpextinstitusi: $("#telpextinstitusi").val(),
        no_fax: $("#no_fax").val(),
        website: $("#website").val(),
        dirinstitusi: $("#dirinstitusi").val(),
        kelamin: $("#kelamin").val()
    }
    
    var bentukflag = "";

    if ($("input[name='bentukflag']")[0].checked)
        bentukflag = "1"
    else if($("input[name='bentukflag']")[1].checked)
        bentukflag = "2"

    data.bentukflag = bentukflag;

    var wniwna = "";

    if ($("input[name='wniwna']")[0].checked)
        wniwna = "1"
    else if($("input[name='wniwna']")[1].checked)
        wniwna = "2"
    
    data.wniwna = wniwna;
    
    var wniflag = "";

    if ($("input[name='wniflag']")[0].checked)
        wniflag = "1"
    else if($("input[name='wniflag']")[1].checked)
        wniflag = "2"
    else if($("input[name='wniflag']")[2].checked)
        wniflag = "3"

    data.wniflag = wniflag;
    
    var wnaflag = "";

    if ($("input[name='wnaflag']")[0].checked)
        wnaflag = "1"
    else if($("input[name='wnaflag']")[1].checked)
        wnaflag = "2"
    else if($("input[name='wnaflag']")[2].checked)
        wnaflag = "3"
    else if($("input[name='wnaflag']")[3].checked)
        wnaflag = "4"

    data.wnaflag = wnaflag;
    
    if($("input[name='kawinflag']")[0] != undefined) {
        var kawinflag = "";

        if ($("input[name='kawinflag']")[0].checked)
            kawinflag = "1"
        else if($("input[name='kawinflag']")[1].checked)
            kawinflag = "2"
        else if($("input[name='kawinflag']")[2].checked)
            kawinflag = "3"

        data.kawinflag = kawinflag;
    }
    
    if($("input[name='pekerjaanflag']")[0] != undefined) {

        var pekerjaanflag = "";

        if ($("input[name='pekerjaanflag']")[0].checked)
            pekerjaanflag = "1"
        else if ($("input[name='pekerjaanflag']")[1].checked)
            pekerjaanflag = "2"
        else if ($("input[name='pekerjaanflag']")[2].checked)
            pekerjaanflag = "3"
        else if ($("input[name='pekerjaanflag']")[3].checked)
            pekerjaanflag = "4"

        data.pekerjaanflag = pekerjaanflag;
    }

    if($("input[name='usahahasilflag']")[0] != undefined) {

        var usahahasilflag = "";

        if ($("input[name='usahahasilflag']")[0].checked)
            usahahasilflag = "1"
        else if ($("input[name='usahahasilflag']")[1].checked)
            usahahasilflag = "2"
        else if ($("input[name='usahahasilflag']")[2].checked)
            usahahasilflag = "3"
        else if ($("input[name='usahahasilflag']")[3].checked)
            usahahasilflag = "4"
        else if ($("input[name='usahahasilflag']")[3].checked)
            usahahasilflag = "5"

        data.usahahasilflag = usahahasilflag;
    }
    
    if($("input[name='usahaflag']")[0] != undefined) {

        var usahaflag = "";

        if ($("input[name='usahaflag']")[0].checked)
            usahaflag = "1"
        else if ($("input[name='usahaflag']")[1].checked)
            usahaflag = "2"
        else if ($("input[name='usahaflag']")[2].checked)
            usahaflag = "3"
        else if ($("input[name='usahaflag']")[3].checked)
            usahaflag = "4"

        data.usahaflag = usahaflag;
    }

    var tujuanpolisflag = "";

    if ($("input[name='tujuanpolisflag']")[0].checked)
        tujuanpolisflag = "1"
    else if($("input[name='tujuanpolisflag']")[1].checked)
        tujuanpolisflag = "2"

    data.tujuanpolisflag = tujuanpolisflag;
    
    if($("#npwpinstitusi")[0] != undefined) {

        var npwpinstitusi;

        if ($("#npwpinstitusi")[0].checked)
            npwpinstitusi = "1"
        else
            npwpinstitusi = "2"

        data.npwpinstitusi = npwpinstitusi;
    }

    if($("#siupinstitusi")[0] != undefined) {

        var siupinstitusi;

        if ($("#siupinstitusi")[0].checked)
            siupinstitusi = "1"
        else
            siupinstitusi = "2"

        data.siupinstitusi = siupinstitusi;
    }

    if($("#tdpinstitusi")[0] != undefined) {
        
    var tdpinstitusi;

    if ($("#tdpinstitusi")[0].checked)
        tdpinstitusi = "1"
    else
        tdpinstitusi = "2"

    data.tdpinstitusi = tdpinstitusi;
    }

    if($("#hukumhaminstitusi")[0] != undefined) {
        var hukumhaminstitusi;

        if ($("#hukumhaminstitusi")[0].checked)
            hukumhaminstitusi = "1"
        else
            hukumhaminstitusi = "2"

        data.hukumhaminstitusi = hukumhaminstitusi;
    }
    
    showProgress('#DetailRekananWindow');

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeWindow("#DetailRekananWindow");
        }
    );
}

function onDeleteDetailRekanan(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_cb: dataItem.kd_cb.trim(),
                kd_grp_rk: dataItem.kd_grp_rk,
                kd_rk: dataItem.kd_rk
            }
            
            showProgressOnGrid("#RekananGrid");

            ajaxPost("/TertanggungPrincipal/DeleteDetailRekanan", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#RekananGrid");
                    closeProgressOnGrid("#RekananGrid");
                }
            );
        }
    );
}


function btnAddRekanan_OnClick() {
    openWindow('#RekananWindow', `/TertanggungPrincipal/AddRekananView`, 'Add');
}

function btnEditRekanan_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#RekananWindow', `/TertanggungPrincipal/EditRekananView?kd_cb=${dataItem.kd_cb.trim()}&kd_grp_rk=${dataItem.kd_grp_rk}&kd_rk=${dataItem.kd_rk}`, 'Edit');
}

function btnEditDetailRekanan_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    selectedData = dataItem;
    console.log('dataItem', dataItem);
    openWindow('#DetailRekananWindow', `/TertanggungPrincipal/EditDetailRekananView?kd_cb=${dataItem.kd_cb.trim()}&kd_grp_rk=${dataItem.kd_grp_rk}&kd_rk=${dataItem.kd_rk}&flag_sic=${dataItem.flag_sic}`, 'Edit');
}