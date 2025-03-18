var cabangs;
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddUserCabang_Click();
    getCabangs();
});
function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#UserCabangGrid");
    });
}
function btnAddUserCabang_Click() {
    $('#btnAddNewUserCabang').click(function () {
        openUserCabangWindow('/UserCabang/Add', 'Set New User Cabang');
    });
}

function btnEditUserCabang_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openUserCabangWindow(`/UserCabang/Edit?id=${dataItem.userid}`, 'Edit User Cabang');
}
function btnDeleteUserCabang_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete role for menu ${dataItem.username}?`,
        function () {
            showProgressOnGrid('#UserCabangGrid');
            setTimeout(function () { deleteUserCabang(dataItem.userid); }, 500);
        }
    );
}
function deleteUserCabang(id) {
    $.ajax({
        url: "/UserCabang/Delete",
        type: "POST",
        data: { id: id },
        success: function (data) {
            if (data.Result == "OK") {
                showMessage('Success', 'Data has been deleted');
                refreshGrid("#UserCabangGrid");
                closeProgressOnGrid('#UserCabangGrid');
            }
            else if (data.Result == "ERROR")
                $("#UserCabangWindow").html(data.Message);
            else
                $("#UserCabangWindow").html(data);
        },
        error: function (data) {
            console.log('Error e:', data);
        }
    });
}
function openUserCabangWindow(url, title) {
    openWindow('#UserCabangWindow', url, title);
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}
function saveUserCabang(url) {
    var grid = $("#CabangGrid").data("kendoGrid");
    grid.saveChanges();
    var userCabangForm = getFormData($('#UserCabangForm'));
    var data = JSON.stringify($.extend(userCabangForm, { Cabangs: grid.dataSource.data() }));
    ajaxPostSafely(url, data, function (dataReturn) {
        if (dataReturn.Result == "OK") {
            showMessage('Success', dataReturn.Message);
            refreshGrid("#UserCabangGrid");
            closeWindow('#UserCabangWindow')
        }
        else {
            var errors = Object.keys(dataReturn.Message).map(k => dataReturn.Message[k]);
            errors.forEach((error)=> toastr.error(error))
        }

        closeProgress('#UserCabangWindow');
    })
}

function getCabangs() {
    $.ajax({
        url: '/UserCabang/GetCabangs',
        type: 'GET',
        success: function (result) {
            cabangs = result;
        }
    });
}