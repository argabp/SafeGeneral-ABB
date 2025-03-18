$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddPolisInduk_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#PolisIndukGrid");
    });
}

function openPolisIndukWindow(url, title) {
    openWindow('#PolisIndukWindow', url, title);
}

function btnAddPolisInduk_Click() {
    $('#btnAddNewPolisInduk').click(function () {
        openPolisIndukWindow('/PolisInduk/Add', 'Add New Polis Induk');
    });
}
function btnEditPolisInduk_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openPolisIndukWindow(`/PolisInduk/Edit?no_pol_induk=${dataItem.no_pol_induk}`, 'Edit Polis Induk');
}
function btnDeletePolisInduk_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete Polis Induk ${dataItem.no_pol_induk}?`,
        function () {
            showProgressOnGrid('#PolisIndukGrid');
            setTimeout(function () { deletePolisInduk(dataItem.no_pol_induk); }, 500);
        }
    );
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}

function deletePolisInduk(id) {
    ajaxGet("/PolisInduk/Delete?no_pol_induk=" + id, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#PolisIndukGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#PolisIndukGrid");
        }
        else
            $("#PolisIndukWindow").html(response);

        closeProgressOnGrid('#PolisIndukGrid');
    }, AjaxContentType.URLENCODED);
}