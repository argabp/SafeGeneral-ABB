
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddCabang_Click();
});

function openCabangWindow(url, title) {
    openWindow('#CabangWindow', url, title);
}

function deleteCabang(id) {
    var data = { kd_cb: id };
    ajaxPostSafely("/Cabang/Delete", data, function (response) {
        if (response.Result) {
            showMessage('Success', 'Data has been deleted');
            refreshGrid("#CabangGrid");
        }
        else if (!response.Result) {
            showMessage('Error', 'Delete data is failed, this data is already used');
            refreshGrid("#CabangGrid");
        }
        else
            $("#CabangWindow").html(response);

        closeProgressOnGrid('#CabangGrid');
    }, AjaxContentType.URLENCODED);
}
function saveCabang(url) {
    var form = getFormData($('#CabangForm'));
    var data = JSON.stringify(form);
    ajaxPostSafely(url, data,
        function (response) {
            refreshGrid("#CabangGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#CabangWindow");
            }
            else if (response.Result == "ERROR")
                $("#CabangWindow").html(response.Message);
            else
                $("#CabangWindow").html(response);

            closeProgress('#CabangWindow');
        }
    );
}