
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddUser_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#UserGrid");
    });
}
function btnAddUser_Click() {
    $('#btnAddUser').click(function () {
        openUserWindow('/User/Add', 'Add New User');
    });
}

function btnEditUser_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openUserWindow(`/User/Edit?id=${dataItem.Id}`, 'Edit User');
}

function btnEditPassword_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openUserWindow(`/User/Changepassword?id=${dataItem.Id}`, 'Change Password');
}


function btnDeleteUser_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Confirmation', `Are you sure you want to delete User ${dataItem.FullName} ?`,
        function () {
            showProgressOnGrid('#UserGrid');
            deleteUser(dataItem.Id);
        }
    );
}

function deleteUser(id) {
    var data = { id: id };
    ajaxPost("/User/Delete", data, function (response) {
        if (response.Result == "OK") {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#UserGrid");
            closeProgressOnGrid('#UserGrid');
        }
        else
            showMessage('Error', response.Message);
        closeProgressOnGrid('#UserGrid');
    }, AjaxContentType.URLENCODED);
}


function openUserWindow(url, title) {
    openWindow('#UserWindow', url, title);
}

function photoTemplate(dataItem) {
    var result = '';
    var photo = `${$('#profilePictureFolder').val()}${dataItem.Photo}`;
    if (dataItem.Photo.includes("default")) photo = `/img/${dataItem.Photo}`;
    result = `<img class='circle-image' src='${photo}' alt='${dataItem.FullName}' width='36' height='36'`;
    return result;
}
