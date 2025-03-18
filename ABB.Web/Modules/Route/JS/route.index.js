var selectedId;
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    $('#btnGenerateRoute').click(function () {
        showProgress('#RouteWindow');
        setTimeout(function () {
            GenerateRoute('/Route/Generate')
        }, 500);
    });
});

function GenerateRoute(url) {
    $.ajax({
        url: url,
        type: "POST",
        processData: false,
        contentType: false,
        success: function (data) {
            console.log("DATA>>>", data);
            if (data.Result == "OK") {
                showMessage('Success', data.Message);
                refreshGrid("#RouteGrid");
                
                closeWindow('#RouteWindow')
            }
            else if (data.Result == "ERROR")
                $("#RouteWindow").html(data.Message);
            else
                $("#RouteWindow").html(data);

            closeProgress('#RouteWindow');
            refreshGrid("#RouteGrid");
        },
        error: function (data) {
            console.log('Error Saving e:', data);
        }
    });
}
function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#RouteGrid");
    });
}

function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}
function openRouteWindow(url, title) {
    openWindow('#RouteWindow', url, title);
}