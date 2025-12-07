function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function onSaveSlideShow(e){
    var url = "/SlideShow/SaveSlideShow";
    
    var grid = e.sender;
    var model = e.model;

    // Get the upload control from the edited row
    var row = grid.tbody.find("tr[data-uid='" + model.uid + "']");
    // Find the file input directly
    var fileInput = row.find("input[type='file'][name='FileName.FileName']")[0];
    var file = null;

    if (fileInput && fileInput.files && fileInput.files.length > 0) {
        file = fileInput.files[0];
    }

    // Prepare form data
    var formData = new FormData();

    if (file) {
        formData.append("File", file);
    }
    
    formData.append("Order", e.model.Order);
    formData.append("Id", e.model.Id);

    ajaxUpload(url, formData,
        function (response) {
            refreshGrid("#SlideShowGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                showMessage('Error', response.Message);
            }
        }
    );
}

function onDeleteSlideShow(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#SlideShowGrid");

            ajaxGet("/SlideShow/DeleteSlideShow?id=" + dataItem.Id,
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#SlideShowGrid");
                    closeProgressOnGrid("#SlideShowGrid");
                }
            );
        }
    );
}
