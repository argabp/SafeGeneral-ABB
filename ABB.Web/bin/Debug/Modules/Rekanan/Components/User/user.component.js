$(document).ready(function () {
    getUsers();
    btnPreviousUser();
    btnClose();
    initUserGrid();
    loadUserDS();
});

var users;

function getUsers() {
    ajaxGet(`/Rekanan/GetUsers`, function (result) {
        users = result;
    });
}

function btnPreviousUser(){
    $('#btn-previous-user').click(function () {
        $("#rekananTab").getKendoTabStrip().select(2);
    });
}

function btnClose(){
    $('#btn-close').click(function () {
        closeWindow("#RekananWindow");
    });
}

function initUserGrid() {
    $("#UserGrid").kendoGrid({
        pageable: true,
        save: saveUser,
        toolbar: ["create"],
        columns: [
            {
                field: "userid", title: "User", width: 200,
                editor: function (container, options) {
                    if(options.model.username === undefined){
                        dropDownEditor({
                            container: container,
                            options: options,
                            data: users,
                            valueField: "UserId",
                            textField: "Username"
                        });
                    } else {
                        $('<p>' + options.model.username +  '</p>')
                            .appendTo(container);
                    }
                },
                template: function (dataItem) {
                    return dataItem.username;
                },
            },
            {
                command: ["edit"], title: "&nbsp;", width: "100px"
            },
            {
                command: { text: "Delete", click: deleteUser, iconClass: "k-icon k-i-delete" }, title: "&nbsp;"
            },
        ],
        editable: {
            mode: "inline",
        }
    });
}

function loadUserDS() {
    var userDS = JSON.parse($('#userDS').val());
    loadInlineGridDS({
        gridId: '#UserGrid',
        arrayObj: userDS,
        fieldKey: "Id",
        model: {
            id: "Id",
            fields: {
                kd_cb: { type: "string" },
                kd_rk: { type: "string" },
                userid: { type: "string" },
            }
        }
    });
}

function saveUser(e){
    showProgressOnGrid("#UserGrid");

    var data = {}

    var kd_cb = $("#kd_cb_hidden").val();
    var kd_rk = $("#kd_rk_hidden").val();

    data.kd_cb = kd_cb;
    data.kd_rk = kd_rk;
    data.userid = e.model.userid;

    var stringifyData = JSON.stringify(data);

    ajaxPost("/Rekanan/SaveUser", stringifyData, function (response) {
        closeProgressOnGrid("#UserGrid");
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
        }
        else if (response.Result == "ERROR")
            showMessage('Error', response.Message);
        else
            $("#tabUser").html(response);

        ajaxGet(`/Rekanan/GetUserRekanans?kd_rk=${kd_rk}&kd_cb=${kd_cb}`,
            function (response) {
                $('#userDS').val(JSON.stringify(response.Data));
                loadUserDS();
            }
        );
    })
}

function deleteUser(event){
    var dataItem = $("#UserGrid").getKendoGrid().dataItem($(event.target).parent().parent()[0])
    showConfirmation('Confirmation', `Are you sure you want to delete this data?`,
        function () {
            showProgressOnGrid("#UserGrid")

            var kd_cb = $("#kd_cb_hidden").val();
            var kd_rk = $("#kd_rk_hidden").val();

            var form = {};

            form.kd_cb = kd_cb;
            form.kd_rk = kd_rk;
            form.userid = dataItem.userid;
            
            var data = JSON.stringify(form);

            ajaxPost("/Rekanan/DeleteUser", data,
                function (response) {
                    if (response.Result == "OK")
                        showMessage('Success', response.Message);
                    else
                        showMessage('Error', response.Message);

                    closeProgressOnGrid("#UserGrid");

                    ajaxGet(`/Rekanan/GetUserRekanans?kd_rk=${kd_rk}&kd_cb=${kd_cb}`,
                        function (response) {
                            $('#userDS').val(JSON.stringify(response.Data));
                            loadUserDS();
                        }
                    );
                }
            );
        }
    );
}