
var selectedId;
$(document).ready(function () {
    searchKeyword_OnKeyUp();
    btnAddModule_Click();
});

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#ModuleGrid");
    });
}
function btnAddModule_Click() {
    $('#btnAddNewModule').click(function () {
        openModuleWindow('/Module/Add', 'Add New Module');
    });
}
function btnEditModule_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    console.log('dataItem', dataItem);
    openModuleWindow(`/Module/Edit?id=${dataItem.Id}`, 'Edit Module Page');
}
function btnDeleteModule_OnClick(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete module ${dataItem.Name}?`,
        function () {
            showProgressOnGrid('#ModuleGrid');
            setTimeout(function () { deleteModule(dataItem.Id); }, 500);
        }
    );
}
function searchFilter() {
    return {
        searchkeyword: $("#SearchKeyword").val()
    }
}