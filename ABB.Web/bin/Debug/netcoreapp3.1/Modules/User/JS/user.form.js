function filePhoto_OnChange() {
    $(".circular-upload-container input").change(function () {
        if (this.files && this.files[0]) {
            $('.circular-upload-container img').attr('src',
                window.URL.createObjectURL(this.files[0]));
        }
    });
}

function saveUser(url) {
    var form = $('#UserForm')[0];
    var formData = new FormData(form);
    ajaxUpload(url, formData, function (response) {
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGrid("#UserGrid");
            closeWindow('#UserWindow')
        }
        else if (response.Result == "ERROR")
            $("#UserWindow").html(response.Message);
        else
            $("#UserWindow").html(response);

        closeProgress('#UserWindow');
    })
}
