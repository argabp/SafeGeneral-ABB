function onEditDetailTarifKebakaranOJK(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_okup = gridId;
}

function onSaveDetailTarifKebakaranOJK(dataItem){
    var url = "/TarifKebakaranOJK/SaveDetailTarifKebakaranOJK";

    var data = {
        kd_okup: $("#kd_okup").val(),
        kd_kls_konstr: $("#kd_kls_konstr").val(),
        stn_rate_prm: parseInt($("#stn_rate_prm").val()),
        pst_rate_prm_min: $("#pst_rate_prm_min").val(),
        pst_rate_prm_max: $("#pst_rate_prm_max").val()
    }
    showProgress('#DetailTarifKebakaranOJKWindow');

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_Detail_" + data.kd_okup );
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else
                showMessage('Error', response.Message);
            
            closeProgress('#DetailTarifKebakaranOJKWindow');
            closeWindow("#DetailTarifKebakaranOJKWindow");
        }
    );
}

function onDeleteDetailTarifKebakaranOJK(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_okup: dataItem.kd_okup,
                kd_kls_konstr: dataItem.kd_kls_konstr
            }
            
            showProgressOnGrid("#grid_Detail_" + data.kd_okup);

            ajaxPost("/TarifKebakaranOJK/DeleteDetailTarifKebakaranOJK", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_Detail_" + data.kd_okup );
                    closeProgressOnGrid("#grid_Detail_" + data.kd_okup);
                }
            );
        }
    );
}


function btnAddDetailTarifKebakaranOJK_OnClick(kd_okup) {
    openWindow('#DetailTarifKebakaranOJKWindow', `/TarifKebakaranOJK/AddDetailTarifKebakaranOJKView?kd_okup=${kd_okup}`, 'Add');
}

function btnEditDetailTarifKebakaranOJK_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#DetailTarifKebakaranOJKWindow', `/TarifKebakaranOJK/EditDetailTarifKebakaranOJKView?kd_okup=${dataItem.kd_okup}&kd_kls_konstr=${dataItem.kd_kls_konstr}`, 'Edit');
}