function onSaveSebabKejadian(){    
    var url = "/SebabKejadian/Save";

    var data = {
        kd_cob: $("#kd_cob").val(),
        kd_sebab: $("#kd_sebab").val(),
        nm_sebab: $("#nm_sebab").val()
    }
    showProgress('#SebabKejadianWindow');
    
    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#SebabKejadianGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#SebabKejadianWindow");
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#SebabKejadianWindow").html(response);

            closeProgress('#SebabKejadianWindow');
        }
    );
}

function onDeleteSebabKejadian(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid('#SebabKejadianGrid');

            ajaxGet(`/SebabKejadian/DeleteSebabKejadian?kd_cob=${dataItem.kd_cob.trim()}&kd_sebab=${dataItem.kd_sebab.trim()}`,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);
                    
                    refreshGrid("#SebabKejadianGrid");
                    closeProgressOnGrid('#SebabKejadianGrid');
                }
            );
        }
    );
}

function btnAddSebabKejadian_OnClick() {
    openWindow('#SebabKejadianWindow', `/SebabKejadian/Add`, 'Add');
}

function btnEditSebabKejadian_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openWindow('#SebabKejadianWindow', `/SebabKejadian/Edit?kd_cob=${dataItem.kd_cob.trim()}&kd_sebab=${dataItem.kd_sebab.trim()}`, 'Edit');
}