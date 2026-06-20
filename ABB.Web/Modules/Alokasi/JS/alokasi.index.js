$(document).ready(function () {
    searchKeyword_OnKeyUp();
    var sorData;
});

function searchFilterSOR(e){
    var startDatePicker = $("#StartDate").data("kendoDatePicker");
    var endDatePicker = $("#EndDate").data("kendoDatePicker");
    var KodeCabang = $("#kd_cb").data("kendoDropDownList");
    var searchkeyword = $("#searchkeyword").val();

    return {
        startDate: startDatePicker && startDatePicker.value() ? kendo.toString(startDatePicker.value(), "yyyy-MM-dd") : null,
        endDate: endDatePicker && endDatePicker.value() ? kendo.toString(endDatePicker.value(), "yyyy-MM-dd") : null,
        kodeCabang: KodeCabang && KodeCabang.value() ? KodeCabang.value() : null,
        searchkeyword: searchkeyword
    };
}

function onSearchClick() {
    // Cukup perintahkan grid untuk membaca ulang datanya
    $("#SORGrid").data("kendoGrid").dataSource.read();
}

let statusFilterApplied = false;

function searchKeyword_OnKeyUp() {
    $('#SearchKeyword').keyup(function () {
        refreshGrid("#SORGrid");
    });
}

function openSORWindow(url, title) {
    openWindow('#AlokasiWindow', url, title);
}

function openEndorsWindow(url, title) {
    openWindow('#EndorsWindow', url, title);
}

function OnClickEditAlokasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    sorData = dataItem;
    console.log('dataItem', dataItem);
    openSORWindow(`/Alokasi/Edit?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}`, 'Edit');
}

function OnClickViewAlokasi(e) {
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    sorData = dataItem;
    console.log('dataItem', dataItem);
    openSORWindow(`/Alokasi/View?kd_cb=${dataItem.kd_cb}&kd_cob=${dataItem.kd_cob}
                                    &kd_scob=${dataItem.kd_scob}&kd_thn=${dataItem.kd_thn}
                                    &no_pol=${dataItem.no_pol}&no_updt=${dataItem.no_updt}`, 'View');
}

function setButtonActions(e){
    var grid = this;
    var userLogin = $("#UserLogin").val();

    grid.tbody.find("tr").each(function(e, element) {
        var dataItem = grid.dataItem(this);
        var uid = $(this).data("uid");

        // Find button container - try locked column first, then regular
        var buttonContainer = grid.element.find(".k-grid-content-locked tr[data-uid='" + uid + "'] .k-command-cell");
        if (!buttonContainer.length) {
            buttonContainer = $(this).find(".k-command-cell");
        }

        if (buttonContainer.length) {
            // Apply your business logic to hide buttons
            // if(userLogin != dataItem.kd_usr_input){
            //     buttonContainer.find(".k-grid-Edit").hide();
            //     buttonContainer.find(".k-grid-Delete").hide();
            // }
            
            if(dataItem.flag_closing == "Y"){
                buttonContainer.find(".k-grid-Edit").hide();
            } else {
                buttonContainer.find(".k-grid-View").hide();
            }
        }
    });

    gridAutoFit(grid);
}