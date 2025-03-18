$(document).ready(function () {
    initLokasiResikoGrid();
    loadLokasiResikoData();
});

function initLokasiResikoGrid() {
    $("#LokasiResikoGrid").kendoGrid({
        sortable: true,
        pageable: true,
        detailInit: detailInit,
        dataBound: function() {
            this.expandRow(this.tbody.find("tr.k-master-row").first());
        },
        filterable: true,
        save: onSaveLokasiResiko,
        toolbar: ["create"],
        columns: [
            {
                field: "kd_pos", title: "Kode Pos", width: 150,
            },
            {
                field: "jalan", title: "Jalan",
            },
            {
                field: "kota", title: "Kota", width: 150,
            },
            {
                command: ["edit"], title: "&nbsp;", width: 70
            },
            {
                command: { text: "Delete", click: onDeleteLokasiResiko, iconClass: "k-icon k-i-delete" }, title: "&nbsp;"
            },
        ],
        editable: {
            mode: "inline",
        }
    });
}

function detailInit(e) {
    $("<div/>").appendTo(e.detailCell).kendoGrid({
        dataSource: {
            transport: {
                read: "/Lokasi/GetDetailLokasiResiko"
            },
            serverPaging: true,
            serverSorting: true,
            serverFiltering: true,
        }
        ,schema: {
            data: "items"
        },
        scrollable: false,
        sortable: true,
        pageable: true,
        columns: [
            {
                field: "kd_pos", title: "Kode Pos", width: 150,
            },
            {
                field: "kd_lok_rsk", title: "Kode Lokasi Resiko", width: 150,
            },
            {
                field: "Gedung", title: "Gedung", width: 150,
            },
            {
                field: "Alamat", title: "Alamat", width: 150,
            },
            {
                field: "kd_prop", title: "Nama Provinsi", width: 150,
                editor: function (container, options) {
                    dropDownEditor({
                        container: container,
                        options: options,
                        url: "Lokasi/GetProvinsiDropdown",
                        valueField: "Value",
                        textField: "Text"
                    });
                },
                template: function (dataItem) {
                    return dataItem.nm_prop;
                },
            },
            {
                command: ["edit"], title: "&nbsp;", width: 70
            },
            {
                command: { text: "Delete", click: onDeleteLokasiResiko, iconClass: "k-icon k-i-delete" }, title: "&nbsp;"
            },
        ]
    });
}
    
function loadLokasiResikoData() {
    ajaxGet("/Lokasi/GetLokasiResiko", (result) => {
        debugger;
        loadInlineGridDS({
            gridId: '#LokasiResikoGrid',
            arrayObj: result.Data,
            fieldKey: "kd_pos",
            model: {
                id: "kd_pos",
                fields: {
                    kd_pos: { type: "string" },
                    jalan: { type: "string"},
                    kota: { type: "string" }
                }
            }
        });
    })
}

function onSaveLokasiResiko(dataItem){
    var url = "/Lokasi/SaveLokasiResiko";

    var data = {
        kd_pos: dataItem.model.kd_pos,
        jalan: dataItem.model.jalan,
        kota: dataItem.model.kota
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#LokasiResikoGrid");
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteLokasiResiko(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            showProgressOnGrid("#LokasiResikoGrid");

            var data = {
                kd_pos: dataItem.kd_pos,
                jalan: dataItem.jalan,
                kota: dataItem.kota
            }

            ajaxPost("/Lokasi/DeleteLokasiResiko", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#LokasiResikoGrid");
                    closeProgressOnGrid("#LokasiResikoGrid");
                }
            );
        }
    );
}


function onEditDetailLokasiResiko(dataItem){
    var gridId = dataItem.container.parent().parent().parent().parent()[0].id.split("_")[2]
    if(dataItem.model.isNew())
        dataItem.model.kd_pos = gridId;
}

function onSaveDetailLokasiResiko(dataItem){
    var url = "/Lokasi/SaveDetailLokasiResiko";

    var data = {
        kd_pos: dataItem.model.kd_pos,
        kd_lok_rsk: dataItem.model.kd_lok_rsk,
        gedung: dataItem.model.gedung,
        alamat: dataItem.model.alamat,
        kd_prop: dataItem.model.kd_prop,
        kd_kab: dataItem.model.kd_kab,
        kd_kec: dataItem.model.kd_kec,
        kd_kel: dataItem.model.kd_kel,
    }

    ajaxPost(url, JSON.stringify(data),
        function (response) {
            refreshGrid("#grid_LokasiResiko_" + data.kd_pos);
            if (response.Result == "OK") {
                showMessage('Success', response.Message);
            } else {
                var errors = Object.keys(response.Message).map(k => response.Message[k]);
                errors.forEach((error)=> toastr.error(error))
            }
        }
    );
}

function onDeleteDetailLokasiResiko(e){
    e.preventDefault();
    var dataItem = this.dataItem($(e.currentTarget).closest("tr"));
    showConfirmation('Confirmation', `Are you sure you want to delete?`,
        function () {
            var data = {
                kd_pos: dataItem.kd_pos,
                kd_lok_rsk: dataItem.kd_lok_rsk,
                gedung: dataItem.gedung,
                alamat: dataItem.alamat,
                kd_prop: dataItem.kd_prop,
                kd_kab: dataItem.kd_kab,
                kd_kec: dataItem.kd_kec,
                kd_kel: dataItem.kd_kel,
            }

            showProgressOnGrid("#grid_LokasiResiko_" + data.kd_pos);

            ajaxPost("/Lokasi/DeleteDetailLokasiResiko", JSON.stringify(data),
                function (response) {
                    if (response.Result == "OK") {
                        showMessage('Success', response.Message);
                    } else
                        showMessage('Error', response.Message);

                    refreshGrid("#grid_LokasiResiko_" + data.kd_pos);
                    closeProgressOnGrid("#grid_LokasiResiko_" + data.kd_pos);
                }
            );
        }
    );
}
