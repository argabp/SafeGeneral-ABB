$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddCOB_Click();
});

function openCOBWindow(url, title) {
    openWindow('#COBWindow', url, title);
}

function deleteCOB(id) {
    var data = { kd_cob: id };
    ajaxPostSafely("/COB/Delete", data, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#COBGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#COBGrid");
        }
        else
            $("#COBWindow").html(response);

        closeProgressOnGrid('#COBGrid');
    }, AjaxContentType.URLENCODED);
}
function saveCOB(url) {
    var form = getFormData($('#COBForm'));
    var data = JSON.stringify(form);
    ajaxPostSafely(url, data,
        function (response) {
            refreshGrid("#COBGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#COBWindow');
            }
            else if (response.Result == "ERROR")
                $("#COBWindow").html(response.Message);
            else
                $("#COBWindow").html(response);

            closeProgress('#COBWindow');
        }
    );
}