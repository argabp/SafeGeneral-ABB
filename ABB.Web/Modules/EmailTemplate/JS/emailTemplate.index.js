$(document).ready(function () {
    // searchKeyword_OnKeyUp();
    btnAddEmailTemplate_Click();
});

function openEmailTemplateWindow(url, title) {
    openWindow('#EmailTemplateWindow', url, title);
}

// function searchKeyword_OnKeyUp() {
//     $('#SearchKeyword').keyup(function () {
//         refreshGrid("#LokasiGrid");
//     });
// }

function btnAddEmailTemplate_Click() {
    $('#btnAddEmailTemplate').click(function () {
        openEmailTemplateWindow('/EmailTemplate/Add', 'Add');
    });
}

function btnEditEmailTemplate_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openEmailTemplateWindow(`/EmailTemplate/Edit?id=${dataItem.Id}`, 'Edit');
}

function btnDeleteEmailTemplate_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));

    showConfirmation('Confirmation', `Are you sure you want to delete email template ${dataItem.Id} ?`,
        function () {
            showProgressOnGrid('#EmailTemplateGrid');
            deleteEmailTemplate(dataItem);
        }
    );
}

function deleteEmailTemplate(dataItem) {
    var url = `/EmailTemplate/DeleteEmailTemplate?id=${dataItem.Id}`;
    ajaxGet(url,  function (response) {
        if (response.Result === "OK") {
            showMessage('Success', response.Message);
            refreshGrid("#EmailTemplateGrid");
            closeProgressOnGrid('#EmailTemplateGrid');
        } else
            showMessage('Error', response.Message);
        closeProgressOnGrid('#EmailTemplateGrid');
    }, AjaxContentType.URLENCODED);
}