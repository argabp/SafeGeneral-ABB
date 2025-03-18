$(document).ready(function () {
    OnClickPosting();
});


function OnClickPosting() {
    $('#btn-posting-sertifikat').click(function () {
        showConfirmation('Posting', `Are you sure you want to posting?`,
            function () {
                showProgressOnGrid('#PostingSertifikatGrid');
                postingSertifikat();
            }
        );
    });
}

function postingSertifikat(){
    var url = "/PostingSertifikat/Posting";
    ajaxGet(url,
        function (response) {
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
                refreshGrid("#PostingSertifikatGrid");
            }
            else
                showMessage('Error', response.Message);

            closeProgressOnGrid('#PostingSertifikatGrid');
        }
    );
}