$(document).ready(function () {
    uploadNotaRisiko();
});

function uploadNotaRisiko(){
    $('#btn-upload-nota-risiko').click(function () {
        var form = new FormData();

        $("#notaRisikoFile").getKendoUpload().getFiles().forEach((data, index) => {
            form.append("file", data.rawFile);
        });
        ajaxUpload("/ImportNotaRisiko/UploadNotaRisiko", form, (result) => {
            if(result.Error === undefined){
                showMessage("Sukses", result)
            } else {
                showMessage("Error", result.Error)
            }
        })
    });
}