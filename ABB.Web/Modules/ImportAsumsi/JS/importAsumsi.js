$(document).ready(function () {
    uploadAsumsi();
});

function uploadAsumsi(){
    $('#btn-upload-asumsi').click(function () {
        var form = new FormData();

        $("#asumsiFile").getKendoUpload().getFiles().forEach((data, index) => {
            form.append("file", data.rawFile);
        });
        ajaxUpload("/ImportAsumsi/UploadAsumsi", form, (result) => {
            if(result.Error === undefined){
                showMessage("Sukses", result)
            } else {
                showMessage("Error", result.Error)
            }
        })
    });
}