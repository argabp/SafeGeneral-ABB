$(document).ready(function () {
    btnUpdateRekanan_Click();
    btnNextRekanan();
    btnLookUp();
});

function btnNextRekanan(){
    $('#btn-next-rekanan').click(function () {
        $("#rekananTab").getKendoTabStrip().select(1);
    });
}

function btnUpdateRekanan_Click() {
    $('#btn-update-rekanan').click(function () {
        showProgress('#RekananWindow');
        setTimeout(function () {
            updateRekanan('/Rekanan/UpdateRekanan')
        }, 500);
    });
}

function updateRekanan(url) {
    var form = getFormData($('#RekananForm'));

    var data = JSON.stringify(form);
    ajaxPost(url, data,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#RekananGrid");
                setRekananModel(data);
                
                var tabstrip = $('#rekananTab').data("kendoTabStrip");
                tabstrip.enable(tabstrip.items()[1]);
                tabstrip.enable(tabstrip.items()[2]);
                tabstrip.enable(tabstrip.items()[3]);

                $("#btn-next-rekanan").prop("disabled", false);
            }
            else if (response.Result == "ERROR")
                showMessage('Error', response.Message);
            else
                $("#tabRekanan").html(response);

            closeProgress('#RekananWindow');
        }
    );
}


function btnLookUp(){
    $('#btn-rekanan-lookUp').click(function () {
        openWindow('#LookUpWindow', `/Rekanan/LookUp`, 'Look Up');
    });
}