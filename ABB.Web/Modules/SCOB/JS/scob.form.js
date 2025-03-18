
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddSCOB_Click();
});

function openSCOBWindow(url, title) {
    openWindow('#SCOBWindow', url, title);
}

function deleteSCOB(kd_cob, kd_scob) {
    var data = { kd_cob: kd_cob, kd_scob: kd_scob };
    ajaxPostSafely("/SCOB/Delete", data, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#SCOBGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#SCOBGrid");
        }
        else
            $("#SCOBWindow").html(response);

        closeProgressOnGrid('#SCOBGrid');
    }, AjaxContentType.URLENCODED);
}
function saveSCOB(url) {
    var form = getFormData($('#SCOBForm'));
    var data = JSON.stringify(form);
    ajaxPostSafely(url, data,
        function (response) {
            refreshGrid("#SCOBGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow('#SCOBWindow');
            }
            else if (response.Result == "ERROR")
                $("#SCOBWindow").html(response.Message);
            else
                $("#SCOBWindow").html(response);

            closeProgress('#SCOBWindow');
        }
    );
}