$(document).ready(function () {
    getModules();
});

function getModules(){
    let moduleDropdown = $("#Module")[0];
    moduleDropdown.options.length = 0;
    ajaxGet("/Home/GetModules", function (response) {
        moduleDropdown.options[moduleDropdown.options.length] = new Option('Pilih Modul', '');
        response.forEach((value) => {
            moduleDropdown.options[moduleDropdown.options.length] = new Option(value.Name, value.Id);
        })
        
        setModule();
    }, AjaxContentType.URLENCODED);
}

function setModule(){
    let selectedModule = $("#SelectedModule").val();
    if(selectedModule === "-1")
        $("#Module").options.selectedIndex = -1;
    else 
        $("#Module").val(parseInt(selectedModule));
}

function changeModule(e){
    if(e.value !== "")
        ajaxGet("/Home/SetModule?moduleId=" + e.value, () => {
            location.reload();
        }, AjaxContentType.URLENCODED);
}