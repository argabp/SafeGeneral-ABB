
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddModule_Click();
});


function LoadIconList() {
    var iconList = $("#Icon").data("kendoComboBox");
    iconList.setDataSource(fawe);
}

function openModuleWindow(url, title) {
    openWindow('#ModuleWindow', url, title);
}

function deleteModule(id) {
    var data = { id: id };
    ajaxPostSafely("/Module/Delete", data, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#ModuleGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#ModuleGrid");
        }
        else
            $("#ModuleWindow").html(response);

        closeProgressOnGrid('#ModuleGrid');
    }, AjaxContentType.URLENCODED);
}
function saveModule(url) {
    var form = getFormData($('#ModuleForm'));
    var data = JSON.stringify(form);
    ajaxPostSafely(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#ModuleGrid");
                closeWindow('#ModuleWindow')
            }
            else if (response.Result == "ERROR")
                $("#ModuleWindow").html(response.Message);
            else
                $("#ModuleWindow").html(response);

            closeProgress('#ModuleWindow');
        }
    );
}