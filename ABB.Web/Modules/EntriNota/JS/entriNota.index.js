
function onEditDetailNota(dataItem){
    var data = $("#NotaGrid").getKendoGrid().dataItem(dataItem.container.parent().parent().parent().parent().parent().parent()[0].previousElementSibling);
    if(dataItem.model.isNew()) {
        dataItem.model.kd_cb = data.kd_cb;
        dataItem.model.jns_tr = data.jns_tr;
        dataItem.model.jns_nt_msk = data.jns_nt_msk;
        dataItem.model.kd_thn = data.kd_thn;
        dataItem.model.kd_bln = data.kd_bln;
        dataItem.model.no_nt_msk = data.no_nt_msk;
        dataItem.model.jns_nt_kel = data.jns_nt_kel;
        dataItem.model.no_nt_kel = data.no_nt_kel;
    }

    dataItem.model.id = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2];
}

function onSaveDetailNota(dataItem){
    var url = dataItem.model.no_ang === 0 ? "/EntriNota/AddDetailNota" : "/EntriNota/EditDetailNota";

    var data = {
        kd_cb: dataItem.model.kd_cb,
        jns_tr: dataItem.model.jns_tr,
        jns_nt_msk: dataItem.model.jns_nt_msk,
        kd_thn: dataItem.model.kd_thn,
        kd_bln: dataItem.model.kd_bln,
        no_nt_msk: dataItem.model.no_nt_msk,
        jns_nt_kel: dataItem.model.jns_nt_kel,
        no_nt_kel: dataItem.model.no_nt_kel,
        no_ang: dataItem.model.no_ang,
        tgl_ang: dataItem.model.tgl_ang.toDateString(),
        tgl_jth_tempo: dataItem.model.tgl_jth_tempo.toDateString(),
        pst_ang: dataItem.model.pst_ang,
        nilai_ang: dataItem.model.nilai_ang
    }
    
    var gridId = "#grid_Detail_" + dataItem.model.id;
    showProgressOnGrid(gridId);

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid(gridId);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);

            closeProgressOnGrid(gridId);
        }
    );
}

function onDeleteDetailNota(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {

            var data = {
                kd_cb: dataItem.kd_cb,
                jns_tr: dataItem.jns_tr,
                jns_nt_msk: dataItem.jns_nt_msk,
                kd_thn: dataItem.kd_thn,
                kd_bln: dataItem.kd_bln,
                no_nt_msk: dataItem.no_nt_msk,
                jns_nt_kel: dataItem.jns_nt_kel,
                no_nt_kel: dataItem.no_nt_kel,
                no_ang: dataItem.no_ang
            }
            
            var gridId = "#grid_Detail_" + $(e.currentTarget).closest("tr").parent().parent().parent().parent()[0].id.split("_")[2];
            
            showProgressOnGrid(gridId);

            ajaxPost("/EntriNota/DeleteDetailNota", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid(gridId);
                    closeProgressOnGrid(gridId);
                }
            );
        }
    );
}

function saveRoleNavigation(url) {
    var grid = $("#NavigationGrid").data("kendoGrid");
    grid.saveChanges();
    var roleForm = getFormData($('#RoleForm'));
    var data = JSON.stringify($.extend(roleForm, { Navigations: grid.dataSource.data() }));
    ajaxPostSafely(url, data, function (dataReturn) {
        if (dataReturn.Result == "OK") {
            showMessage('Success', dataReturn.Message);
            refreshGrid("#RoleNavigationGrid");
            closeWindow('#RoleNavigationWindow')
        }
        else {
            var errors = Object.keys(dataReturn.Message).map(k => dataReturn.Message[k]);
            errors.forEach((error)=> toastr.error(error))
        }

        closeProgress('#RoleNavigationWindow');
    })
}
