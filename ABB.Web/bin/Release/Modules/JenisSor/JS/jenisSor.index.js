function onSaveJenisSor(){    
    var url = "/JenisSor/SaveJenisSor";

    var data = {
        kd_jns_sor: $("#kd_jns_sor").val(),
        nm_jns_sor: $("#nm_jns_sor").val(),
        grp_jns_sor: $("#grp_jns_sor").val(),
        no_urut: $("#no_urut").val()
    }
    showProgress('#JenisSorWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#JenisSorGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#JenisSorWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#JenisSorWindow").html(response);

            closeProgress('#JenisSorWindow');
        }
    );
}

function onDeleteJenisSor(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#JenisSorGrid');

            ajaxGet(`/JenisSor/DeleteJenisSor?kd_jns_sor=${dataItem.kd_jns_sor.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#JenisSorGrid");
                    closeProgressOnGrid('#JenisSorGrid');
                }
            );
        }
    );
}

function btnAddJenisSor_OnClick() {
    openWindow('#JenisSorWindow', `/JenisSor/AddJenisSorView`, 'Add');
}

function btnEditJenisSor_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#JenisSorWindow', `/JenisSor/EditJenisSorView?kd_jns_sor=${dataItem.kd_jns_sor.trim()}`, 'Edit');
}