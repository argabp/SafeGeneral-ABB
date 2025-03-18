$(document).ready(function () {
    btnNextPesertaSirama();
});

function btnNextPesertaSirama(){
    $('#btn-next-dataBackup').click(function () {
        $("#dataBackupTab").getKendoTabStrip().select(1);
    });
}
