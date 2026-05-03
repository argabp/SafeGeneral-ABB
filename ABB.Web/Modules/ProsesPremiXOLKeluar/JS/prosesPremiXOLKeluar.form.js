$(document).ready(async function () {
    btnSaveProsesPremiXOLKeluar_Click();
    btnSelectTreatyKeluar();
});

function openTreatyKeluarWindow(url, title) {
    openWindow('#TreatyKeluarWindow', url, title);
}

function btnSelectTreatyKeluar(){
    $('#btn-select-treatyKeluar').click(function () {
        openTreatyKeluarWindow('/ProsesPremiXOLKeluar/TreatyKeluar', 'Treaty Keluar')
    });
}

function btnSaveProsesPremiXOLKeluar_Click() {
    $('#btn-save-prosesPremiXOLKeluar').click(function () {
        showProgress('#ProsesPremiXOLKeluarWindow');
        setTimeout(function () {
            saveProsesPremiXOLKeluar('/ProsesPremiXOLKeluar/SaveProsesPremiXOLKeluar')
        }, 500);
    });
}

function saveProsesPremiXOLKeluar(url){
    var form = getFormData($('#ProsesPremiXOLKeluarForm'));
    form.flag_closing = $("#flag_closing")[0].checked ? "Y" : "N";
    
    ajaxPost(url, JSON.stringify(form),
        function (response) {
            refreshGrid("#ProsesPremiXOLKeluarGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                closeWindow("#ProsesPremiXOLKeluarWindow")
            }
            else if (response.Result == "ERROR") {
                showMessage('Error', response.Message);
            }
            else {
                $("#ProsesPremiXOLKeluarWindow").html(response);
            }

            closeProgress("#ProsesPremiXOLKeluarWindow");
        }
    );
}
