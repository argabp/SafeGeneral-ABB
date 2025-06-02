function saveEmailTemplate(url) {
    var data = {}

    data.Id = $("#Id").val();
    data.Name = $("#Name").val();
    data.Body = $("#Body").getKendoEditor().value();
    
    var json = JSON.stringify(data);
    
    ajaxPost(url, json, function (response) {
        if (response.Result == "OK") {
            showMessage('Success', response.Message);
            refreshGrid("#EmailTemplateGrid");
            closeWindow('#EmailTemplateWindow')
        }
        else if (response.Result == "ERROR")
            $("#EmailTemplateWindow").html(response.Message);
        else
            $("#EmailTemplateWindow").html(response);

        closeProgress('#EmailTemplateWindow');
    })
}