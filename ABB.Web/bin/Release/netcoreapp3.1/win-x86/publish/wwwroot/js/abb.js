var emptyFunction = function () {
  return;
};
var AppFieldType = Object.freeze({
  TextBox: `TextBox`,
  Radio: "Radio",
  DropDown: "DropDown",
  CheckBox: "CheckBox",
  TextArea: "TextArea",
  DateEdit: "DateEdit",
  TimeEdit: "TimeEdit",
  Calculator: "Calculator",
  File: "File",
});
var AjaxContentType = Object.freeze({
  URLENCODED: "application/x-www-form-urlencoded",
  JSON: "application/json; charset=utf-8",
});
$(document).ready(function () {
  switch_OnClick();
});
function createDialog(id, title, message) {
  var ctl = $(id).data("kendoDialog");
  if (ctl) ctl.destroy();

  $("body").append(`<div id="${id.replace("#", "")}" ></div>`);
  $(id).kendoDialog({
    title: title,
    content: message,
    width: 300,
    modal: true,
    visible: false,
    actions: [
      {
        text: "Ok",
      },
    ],
  });
}
function getPhotoUrl(url) {
  var photoUrl;
  if (url == "default-profile-picture.png" || url == "broadcast.png") {
    photoUrl = "/img/" + url;
  } else {
    photoUrl = "/img/profile-picture/" + url;
  }
  return photoUrl;
}
function createConfirmDialog(id, title, message, yesFunction) {
  var ctl = $(id).data("kendoDialog");
  if (ctl) ctl.destroy();

  $("body").append(`<div id="${id.replace("#", "")}" ></div>`);
  $(id).kendoDialog({
    title: title,
    content: message,
    width: 300,
    modal: true,
    visible: false,
    actions: [
      {
        text: "Yes",
        action: yesFunction,
      },
      {
        text: "Cancel",
        action: function () {
          return true;
        },
      },
    ],
  });
}
function createConfirmDialogYesNo(id, title, message, yesFunction, noFunction) {
  var ctl = $(id).data("kendoDialog");
  if (ctl) ctl.destroy();

  $("body").append(`<div id="${id.replace("#", "")}" ></div>`);
  $(id).kendoDialog({
    title: title,
    content: message,
    width: 300,
    modal: true,
    visible: false,
    actions: [
      {
        text: "Yes",
        action: yesFunction,
      },
      {
        text: "No",
        action: noFunction,
      },
    ],
  });
}
function refreshGrid(id) {
  var grid = $(id).data("kendoGrid");
  grid?.dataSource?.page(1);
}
function refreshTreeList(id) {
  var tree = $(id).data("kendoTreeList");
  tree?.dataSource?.read();
}
function refreshTreeView(id) {
  var tree = $(id).data("kendoTreeView");
  tree?.dataSource?.read();
}
function showMessage(title, message) {
  createDialog("#msgBox", title, message);
  openDialog("#msgBox", title, message);
}
function showConfirmation(title, message, yesFunction) {
  showConfirmationById("#confirmBox", title, message, yesFunction);
}
function showConfirmationById(id, title, message, yesFunction) {
  createConfirmDialog(id, title, message, yesFunction);
  openDialog(id, title, message);
}
function showConfirmationYesNo(title, message, yesFunction, noFunction) {
  showConfirmationYesNoById(
    "#confirmBox",
    title,
    message,
    yesFunction,
    noFunction
  );
}
function showConfirmationYesNoById(
  id,
  title,
  message,
  yesFunction,
  noFunction
) {
  createConfirmDialogYesNo(id, title, message, yesFunction, noFunction);
  openDialog(id, title, message);
}
function openDialog(id, title, message) {
  var exist = $(id).data("kendoDialog");
  if (exist)
    $(id)
      .data("kendoDialog")
      .title(title)
      .content(`<div style="margin:20px;font-size:13px">${message}</div>`)
      .open();
}
function closeMessageBox() {
  $("#msgBox").data("kendoDialog").close();
}
function showProgress(containerId) {
  kendoProgress(containerId, true);
}
function closeProgress(containerId) {
  kendoProgress(containerId, false);
}
function showProgressOnGrid(containerId) {
  kendoProgressOnGrid(containerId, true);
}
function closeProgressOnGrid(containerId) {
  kendoProgressOnGrid(containerId, false);
}
function showProgressByElement($element) {
  kendo.ui.progress($element, true);
}
function closeProgressByElement($element) {
  kendo.ui.progress($element, false);
}
function kendoProgress(id, state) {
  var win = $(id).data("kendoWindow");
  kendo.ui.progress(win.element, state);
}
function kendoProgressOnGrid(id, state) {
  var win = $(id).data("kendoGrid");
  kendo.ui.progress(win.element, state);
}
function openWindow(id, url, title, type = "GET", data = {}) {
  var win = $(id).data("kendoWindow");
  win.one("refresh", function (e) {
    e.sender.center();
  });
  win.title(title);
  win.refresh({
    url: url,
    type: type,
    data: data,
  });
  win.center().open();
}
function closeWindow(id) {
  $(id).data("kendoWindow").close();
}
function getIndexById(arrObj, idColumnName, id) {
  var l = arrObj.length;

  for (var j = 0; j < l; j++) {
    if (arrObj[j][`${idColumnName}`] == id) {
      return j;
    }
  }
  return null;
}
function generateId(length, characters) {
  var result = "";
  var charactersLength = characters.length;
  for (var i = 0; i < length; i++) {
    result += characters.charAt(Math.floor(Math.random() * charactersLength));
  }
  return result;
}
function getFormData($form) {
  var unindexed_array = $form.serializeArray();
  var indexed_array = {};

  $.map(unindexed_array, function (n, i) {
    indexed_array[n["name"]] = n["value"];
  });
  return indexed_array;
}

function generateStringId() {
  return generateId(
    32,
    "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789"
  );
}
function generateNumberId() {
  return Number(generateId(7, "0123456789"));
}
function getGUID() {
  return kendo.guid().replaceAll("-", "");
}
function dropDownTemplate(value) {
  if (value === undefined) value = "";
  return `
        <span class="k-widget k-dropdown">
            <span unselectable="on" class="k-dropdown-wrap k-state-default">
                <span unselectable="on" class="k-input">${value}</span>
                <span unselectable="on" class="k-select" aria-label="select">
                    <span class="k-icon k-i-arrow-60-down"></span>
                </span>
            </span>
        </span>`;
}
function multiSelectTemplate(arrObj, descriptionColumn) {
  var data = JSON.parse(JSON.stringify(arrObj));
  if (arrObj == []) data = [];
  var middle = "";
  var start = `
        <div class="k-widget k-multiselect" style="height:20px">
            <div class="k-multiselect-wrap k-floatwrap">
                <ul class="k-reset">`;
  data?.forEach(function (item) {
    middle =
      middle +
      ` <li class="k-button">
                <span>${item[descriptionColumn]}</span>
            </li> `;
  });
  var end = `</ul>
            </div>
        </div>`;
  return `${start} ${middle} ${end}`;
}

function switchTemplate(value, labeltrue = "Active", labelfalse = "Inactive") {
  var swichState = value ? "on" : "off";
  var swichLabel = value ? labeltrue : labelfalse;
  return `
                <span class="k-switch k-widget k-switch-${swichState}">
                    <span class="k-switch-container">
                    <span class="k-switch-handle"></span>
                    </span>
                </span
                <label> ${swichLabel}</label>
            `;
}
function checkBoxTemplate(value) {
  var checked = value ? "checked='checked'" : "";
  return `<input type="checkbox" class="k-checkbox" ${checked} />`;
}
function timeDifference(current, previous) {
  var msPerMinute = 60 * 1000;
  var msPerHour = msPerMinute * 60;
  var msPerDay = msPerHour * 24;
  var msPerMonth = msPerDay * 30;
  var msPerYear = msPerDay * 365;

  var elapsed = current - previous;

  if (elapsed < msPerMinute) {
    return Math.round(elapsed / 1000) + " seconds ago";
  } else if (elapsed < msPerHour) {
    return Math.round(elapsed / msPerMinute) + " minutes ago";
  } else if (elapsed < msPerDay) {
    return Math.round(elapsed / msPerHour) + " hours ago";
  } else if (elapsed < msPerMonth) {
    return "approximately " + Math.round(elapsed / msPerDay) + " days ago";
  } else if (elapsed < msPerYear) {
    return "approximately " + Math.round(elapsed / msPerMonth) + " months ago";
  } else {
    return "approximately " + Math.round(elapsed / msPerYear) + " years ago";
  }
}
function convertStringTemplateToBlock(value) {
  // The array of regex patterns to look for
  if (value == null) return "";
  var str = value;
  var format_search = [/\[(.*?)\]/gi]; // note: NO comma after the last entry

  // The matching array of strings to replace matches with
  var format_replace = ['<span class="k-button-in-grid">$1</span>'];

  // Perform the actual conversion
  for (var i = 0; i < format_search.length; i++) {
    str = str.replace(format_search[i], format_replace[i]);
  }
  return str;
}
function brTemplate(value) {
  if (value == null) return "";
  return value.replaceAll("#", "<br />");
}
function arrowTemplate(value) {
  if (value == null) return "";
  return value.replaceAll(
    "|",
    `<span class='fa fa-arrow-right orange'></span>`
  );
}

function brTemplateWithComma(value) {
  if (value == null) return "";
  value = value.replaceAll("#", "<br />");
  return value.replaceAll(",", "<br />");
}

function booleanTemplate(value, labeltrue = "Active", labelfalse = "Inactive") {
  var result;
  if (value) {
    result = `<i class="fas fa-check-circle fa-lg" style = "color:var(--jade-green)" ></i> ${labeltrue}`;
  } else {
    result = `<i class="fas fa-times-circle fa-lg" style = "color:var(--coral-red)" ></i> ${labelfalse}`;
  }
  return result;
}

function booleanTemplateWithComma(value) {
  var result = "";
  value = value.replaceAll("#", "<br />");
  value = value.replaceAll(",", "<br />");
  value.split("<br/>");
  $.each(value.split("<br />"), function (index, valueArray) {
    if (valueArray == 1) {
      result =
        result +
        `<i class="fas fa-check-circle fa-lg" style = "color:var(--jade-green)" ></i> Yes`;
    } else {
      result =
        result +
        `<i class="fas fa-times-circle fa-lg" style = "color:var(--coral-red)" ></i> No`;
    }
    result = result + "<br />";
  });

  return result;
}

function switchEditor(args) {
  $(`<input name="${args.options.field}" style="margin-left:10px"/>`)
    .appendTo(args.container)
    .kendoSwitch();
}
function datePickerEditor(args) {
  $(`<input name="${args.options.field}"/>`)
    .appendTo(args.container)
    .kendoDatePicker({ format: "dd MMM yyyy" });
}
function timePickerEditor(args) {
  if (args.format == undefined) args.format = "hh:mm:ss tt";
  if (args.interval == undefined) args.interval = 30;

  $(`<input name="${args.options.field}"/>`)
    .appendTo(args.container)
    .kendoTimePicker({ format: args.format, interval: args.interval });
}
function textBoxEditor(args) {
    var changeEvt = emptyFunction;
    if (args.change != undefined) changeEvt = args.change;

    $(`<input name="${args.options.field}"/>`)
        .appendTo(args.container)
        .kendoTextBox({
            change: changeEvt
        });
}
function autoCompleteEditor(args) {
    var changeEvt = function () { return; };
    var ds = {
        transport: {
            read: function (e) {
                e.success(args.data);
            }
        }
    };
    if (args.change != undefined) changeEvt = args.change;
    if (args.dataSource != undefined) {
        ds = args.dataSource;
    }
    if (args.url != undefined) {
        ds = GetDataSource(args.url);
    }
    $(`<input name="${args.options.field}"/>`)
        .appendTo(args.container)
        .kendoAutoComplete({
            change: changeEvt,
            dataSource: ds
        });

}
function checkBoxEditor(args) {
  $(
    `<input name="${args.options.field}" type="checkbox" class="k-checkbox"/>`
  ).appendTo(args.container);
}
function dropDownEditor(args) {
  var changeEvt = emptyFunction;
  var ds = {
    transport: {
      read: function (e) {
        e.success(args.data);
      },
    },
  };
  if (args.change != undefined) changeEvt = args.change;
  if (args.dataSource != undefined) {
    ds = args.dataSource;
  }
  if (args.url != undefined) {
    ds = GetDataSource(args.url);
  }

  $('<input name="' + args.options.field + '"/>')
    .appendTo(args.container)
    .kendoDropDownList({
      autoBind: false,
      dataTextField: args.textField,
      dataValueField: args.valueField,
      filter: "contains",
      dataSource: ds,
      change: changeEvt,
    });
}
function multiSelectEditor(args) {
  var changeEvt = emptyFunction;
  var openEvt = emptyFunction;
  var ds = {
    transport: {
      read: function (e) {
        e.success(args.data);
      },
    },
  };
  if (args.change != undefined) changeEvt = args.change;
  if (args.open != undefined) openEvt = args.open;
  if (args.dataSource != undefined) {
    ds = args.dataSource;
  }
  if (args.url != undefined) {
    ds = GetDataSource(args.url);
  }
  $('<input name="' + args.options.field + '"/>')
    .appendTo(args.container)
    .kendoMultiSelect({
      autoBind: false,
      dataTextField: args.textField,
      dataValueField: args.valueField,
      filter: "contains",
      dataSource: ds,
      change: changeEvt,
      open: openEvt,
    });
}
function comboBoxEditor(args) {
  var changeEvt = function () {
    return;
  };
  var ds = {
    transport: {
      read: function (e) {
        e.success(args.data);
      },
    },
  };
  if (args.change != undefined) changeEvt = args.change;
  if (args.dataSource != undefined) {
    ds = args.dataSource;
  }
  if (args.url != undefined) {
    ds = GetDataSource(args.url);
  }
  $('<input name="' + args.options.field + '"/>')
    .appendTo(args.container)
    .kendoComboBox({
      autoBind: false,
      dataTextField: args.textField,
      dataValueField: args.valueField,
      suggest: true,
      filter: "contains",
      change: changeEvt,
      dataSource: ds,
      footerTemplate: args?.footerTemplate,
    });
}

function getCRUDDataSource(args) {
  var arrObj = args.arrayObj;
  var fieldKey = args.fieldKey;
  return new kendo.data.DataSource({
    transport: {
      read: function (e) {
        e.success(arrObj);
      },
      create: function (e) {
        e.data[fieldKey] = generateNumberId();
        arrObj.push(e.data);
        e.success(e.data);
      },
      update: function (e) {
        arrObj[getIndexById(arrObj, fieldKey, e.data[fieldKey])] = e.data;
        e.success();
      },
      destroy: function (e) {
        arrObj.splice(getIndexById(arrObj, fieldKey, e.data[fieldKey]), 1);
        e.success();
      },
    },
    error: function (e) {
      alert("Status: " + e.status + "; Error message: " + e.errorThrown);
    },
    batch: false,
    schema: {
      model: args.model,
    },
  });
}

function loadInlineGridDS(args) {
  loadDataSource(args, "kendoGrid");
}
function loadTabDS(args) {
  loadDataSource(args, "kendoTabStrip");
}
function loadDataSource(args, type) {
  var dataSource = getCRUDDataSource(args);
  if (args.gridId != undefined) args.id = args.gridId;
  var ctl = $(args.id).data(type);
  ctl.setDataSource(dataSource);
}
function autoFitAllColumn($this, customColumn = []) {
  $this.columns.forEach(function (item, index) {
    if (item.width != undefined || item.width > 1) customColumn.push(index);
  });

  for (var i = 0; i < $this.columns.length; i++) {
    if (i !== 0 && !customColumn.includes(i)) {
      $this.autoFitColumn(i);
    }
  }
}
function initSortableGrid(id) {
  var grid = $(id).data("kendoGrid");
  grid.table.kendoSortable({
    filter: ">tbody >tr:not(.k-grid-edit-row)",
    hint: function (e) {
      return $(
        '<div class="k-grid k-widget"><table><tbody><tr>' +
          e.html() +
          "</tr></tbody></table></div>"
      );
    },
    cursor: "move",
    ignore: "TD, input",
    placeholder: function (element) {
      return element.clone().addClass("k-state-hover").css("opacity", 0.65);
    },
    container: `${id} tbody`,
    change: onGridSortableRowChange,
  });
}
function onGridSortableRowChange(e) {
  var grid = $(e.sender.element).closest(".k-grid").getKendoGrid(),
    newIndex = e.newIndex,
    dataItem = grid.dataSource.getByUid(e.item.data("uid")); //retrieve the moved dataItem

  grid.dataSource.remove(dataItem);
  grid.dataSource.insert(newIndex, dataItem);
}
function GetDataSource(url) {
  return new kendo.data.DataSource({
    autoSync: true,
    transport: {
      read: {
        url: url,
        contentType: "application/json",
        type: "GET",
      },
    },
  });
}
function searchFilter() {
  return {
    searchkeyword: $("#SearchKeyword").val(),
  };
}
function gridAutoFit(e) {
  var $this = this;
  autoFitAllColumn($this);
}
function directAddTemplate(instance, url, type) {
  var value = instance.input.val();
  var name = instance.element[0].name;
  var trimmed = value.replace(/\s+/g, "");
  if (value != "" && trimmed != "" && !value.includes("Select")) {
    $(document).off("click", `.${name}`);
    $(document).on("click", `.${name}`, function () {
      directAdd(url, name, value, type);
    });
    return `
                <div class="d-flex">
                <button class="${name} k-button flex-fill orangetext" style="display:block; text-align:left">
                    + add <span class="blacktext">${value}</span>
                </button></div>`;
  } else return "";
}
function directAdd(url, name, value, type) {
  var widget = $(`[name='${name}']`).data(type);
  var dataSource = widget.dataSource;
  var selectedValue = widget.value();
  showConfirmation(
    "Confirmation",
    `Are you sure you want to add ${value} ?`,
    function () {
      $.ajax({
        url: url,
        type: "GET",
        data: {
          value: value,
        },
        success: function (response) {
          if (response?.Result == "VALIDATION")
            showMessage("Validation", response.Message);
          else {
            dataSource.add(response);
            dataSource.sync();
            if (type == "kendoComboBox") selectedValue = response.Id;
            else selectedValue.push(response.Id);
            widget.value(selectedValue);
            widget.focus();
            showMessage("Info", "Success add " + value);
          }
        },
      });
    }
  );
}
function directAddByMultiSelect(instance, url) {
  return directAddTemplate(instance, url, "kendoMultiSelect");
}
function directAddByComboBox(instance, url) {
  return directAddTemplate(instance, url, "kendoComboBox");
}
function ajaxGet(url, successFunction = emptyFunction) {
  ajaxGetWithParam(url, {}, successFunction);
}
function ajaxGetWithParam(url, data = {}, successFunction = emptyFunction) {
  $.ajax({
    url: url,
    type: "GET",
    data: data,
    success: successFunction,
    error: function (dataReturn) {
      showAccessDeniedMessage(dataReturn.responseText);
      console.log(`Error ${url}:`, dataReturn);
    },
  });
}
function ajaxPost(
  url,
  data = {},
  successFunction = emptyFunction,
  contentType = "application/json; charset=utf-8",
  headers = {}
) {
  $.ajax({
    url: url,
    type: "POST",
    contentType: contentType,
    headers: headers,
    data: data,
    success: successFunction,
    error: function (dataReturn) {
      showAccessDeniedMessage(dataReturn.responseText);
      console.log(`Error ${url}:`, dataReturn);
    },
  });
}
function ajaxUpload(
  url,
  data = {},
  successFunction = emptyFunction,
  headers = {}
) {
  $.ajax({
    url: url,
    type: "POST",
    contentType: false,
    processData: false,
    headers: headers,
    data: data,
    success: successFunction,
    error: function (dataReturn) {
      showAccessDeniedMessage(dataReturn.responseText);
      console.log(`Error ${url}:`, dataReturn);
    },
  });
}
function ajaxPostSafely(
  url,
  data = {},
  successFunction = emptyFunction,
  contentType = "application/json; charset=utf-8"
) {
  var headers = {
    RequestVerificationToken: $(
      "input[name='__RequestVerificationToken']"
    ).val(),
  };
  ajaxPost(url, data, successFunction, contentType, headers);
}
function showAccessDeniedMessage(responseText) {
  if (responseText?.includes("Access Denied")) {
    showMessage("Warning", "Access Denied");
  }
  removeProgress();
}
function removeProgress() {
  $(".k-loading-mask").each(function (index, element) {
    $(element).remove();
  });
}

function setLocalStorage(name, value) {
  localStorage.setItem(name, JSON.stringify(value));
}
function getLocalStorage(name) {
  return JSON.parse(localStorage.getItem(name));
}
function removeLocalStorage(name) {
  localStorage.removeItem(name);
}
function addLocalArray(storageName, item) {
  var arr = getLocalStorage(storageName);
  if (Array.isArray(arr)) {
    arr.push(item);
    setLocalStorage(storageName, arr);
  } else {
    var newArray = [];
    newArray.push(arr);
    newArray.push(item);
    setLocalStorage(storageName, newArray);
  }
}
function editLocalArray(storageName, keyField, editItem) {
  var array = getLocalStorage(storageName);
  array.forEach(function (item, index, arr) {
    if (item[keyField] == editItem[keyField]) arr[index] = editItem;
  });

  setLocalStorage(storageName, array);
}
function getLocalArrayById(storageName, keyField, id) {
  var array = getLocalStorage(storageName);
  var result = {};
  array.forEach(function (item) {
    if (item[keyField] == id) result = item;
  });
  return result;
}
function removeLocalArray(storageName, keyField, id) {
  var array = getLocalStorage(storageName);
  array.forEach(function (item, index, arr) {
    if (item[keyField] == id) arr.splice(index, 1);
  });

  setLocalStorage(storageName, array);
}
function moveArrayElement(arr, old_index, new_index) {
  if (new_index >= arr.length) {
    var k = new_index - arr.length + 1;
    while (k--) {
      arr.push(undefined);
    }
  }
  arr.splice(new_index, 0, arr.splice(old_index, 1)[0]);
}
function selectInputContent(input) {
  try {
    var inputLen = input.val().length;
    input.focus();
    input[0].setSelectionRange(0, inputLen);
  } catch (ex) {
    console.log("error selectInputContent:", ex);
  }
}
function getMaxValue(element) {
  var values = element
    .map(function () {
      return isNaN(this.value) ? [] : +this.value;
    })
    .get();
  var result = Math.max.apply(null, values);
  return result == -Infinity ? 0 : result;
}

function switch_OnClick() {
  $(document)
    .off("click", ".k-switch")
    .on("click", ".k-switch", function (e) {
      var parent = $(this).closest(".switch-box");
      var hidden = parent.find('input[type="hidden"]');
      var checkbox = parent.find('input[type="checkbox"]');
      hidden.val(checkbox.is(":checked"));
    });
}
function switchReloadData() {
  $("form")
    .find(".k-switch")
    .each(function (key, item) {
      var parent = $(item).closest(".switch-box");
      parent
        .find("input[type=hidden]")
        .val(parent.find("input[type=checkbox]").is(":checked"));
    });
}
function trimChars(str, c) {
  var re = new RegExp("^[" + c + "]+|[" + c + "]+$", "g");
  return str.replace(re, "");
}

jQuery.fn.extend({
  insertAtCaret: function (myValue) {
    return this.each(function (i) {
      if (document.selection) {
        //For browsers like Internet Explorer
        this.focus();
        var sel = document.selection.createRange();
        sel.text = myValue;
        this.focus();
      } else if (this.selectionStart || this.selectionStart == "0") {
        //For browsers like Firefox and Webkit based
        var startPos = this.selectionStart;
        var endPos = this.selectionEnd;
        var scrollTop = this.scrollTop;
        this.value =
          this.value.substring(0, startPos) +
          myValue +
          this.value.substring(endPos, this.value.length);
        this.focus();
        this.selectionStart = startPos + myValue.length;
        this.selectionEnd = startPos + myValue.length;
        this.scrollTop = scrollTop;
      } else {
        this.value += myValue;
        this.focus();
      }
    });
  },
});
function getWeek(inputDate) {
  let d = new Date(inputDate);
  let out = [];

  // set to "Sunday" for the previous week
  d.setDate(d.getDate() - (d.getDay() || 7)); // if getDay is 0 (Sunday), take 7 days
  for (let i = 0; i < 7; i++) {
    // note, the value of i is unused
    out.push(new Date(d.setDate(d.getDate() + 1))); // increment by one day
  }
  return out;
}

function dateOnChange(event, firstElement, secondElement) {
  var today = new Date();
  var dateFrom = $("#" + firstElement).getKendoDatePicker();
  var dateTo = $("#" + secondElement).getKendoDatePicker();
  switch (event.sender._cascadedValue) {
    case "Today":
      enableDate(false, firstElement, secondElement);
      dateFrom.value(today);
      dateTo.value(today);
      break;
    case "Yesterday":
      enableDate(false, firstElement, secondElement);
      var yesterday = new Date(today);
      yesterday.setDate(yesterday.getDate() - 1);
      dateFrom.value(yesterday);
      dateTo.value(yesterday);
      break;
    case "This Week":
      enableDate(false, firstElement, secondElement);
      var thisWeek = getWeek(today);
      dateFrom.value(thisWeek[0]);
      dateTo.value(thisWeek[6]);
      break;
    case "Last Week":
      enableDate(false, firstElement, secondElement);
      today.setDate(today.getDate() - 7);
      var week = getWeek(today);
      dateFrom.value(week[0]);
      dateTo.value(week[6]);
      break;
    case "Last 7 Days":
      enableDate(false, firstElement, secondElement);
      dateFrom.value(new Date(Date.now() - 7 * 24 * 60 * 60 * 1000));
      dateTo.value(today);
      break;
    case "This Month":
      enableDate(false, firstElement, secondElement);
      var firstDay = new Date(today.getFullYear(), today.getMonth(), 1);
      var lastDay = new Date(today.getFullYear(), today.getMonth() + 1, 0);
      dateFrom.value(firstDay);
      dateTo.value(lastDay);
      break;
    case "Last Month":
      enableDate(false, firstElement, secondElement);
      var fDay = new Date(today.getFullYear(), today.getMonth() - 1, 1);
      var lDay = new Date(today.getFullYear(), today.getMonth(), 0);
      dateFrom.value(fDay);
      dateTo.value(lDay);
      break;
    case "This Year":
      enableDate(false, firstElement, secondElement);
      var fDay = new Date(today.getFullYear(), 0, 1);
      var lDay = new Date(today.getFullYear(), 11, 31);
      dateFrom.value(fDay);
      dateTo.value(lDay);
      break;
    case "Select Date":
      enableDate(true, firstElement, secondElement);
      break;
    case "":
      dateFrom.value(null);
      dateTo.value(null);
      break;
  }
}


function enableDate(isEnable, firstElement, secondElement) {
  $("#" + firstElement)
    .getKendoDatePicker()
    .readonly(!isEnable);
  $("#" + secondElement)
    .getKendoDatePicker()
    .readonly(!isEnable);
}

function preventInvalidDate_OnChange() {
  var input = $(this);
  var widget = input.data("kendoDatePicker");
  if (
    (widget && widget.value() === null && input.val()) ||
    !isDate(input.val())
  ) {
    widget.value("");
  }
}
function isDate(date) {
  return new Date(date) !== "Invalid Date" && !isNaN(new Date(date));
}

$.getScript("/js/calculation.js", function () {
  console.log("Script calculation.js loaded");
});

function initPageSizeDropdownGrid(showAllOption = true) {
  setTimeout(function () {
    $('[data-role="grid"]').each(function (index, element) {
      var arrSize = ["25", "50", "100", "500", "1000", "5000"];
      var grid = $(element).data("kendoGrid");
      if (grid.pager) {
        var pageSizeDropDownList = grid.wrapper
          .children(".k-grid-pager")
          .find("[data-role='dropdownlist']")
          .data("kendoDropDownList");
        pageSizeDropDownList?.dataSource?.data([]);
        for (let size of arrSize) {
          pageSizeDropDownList?.dataSource?.add({ text: size, value: size });
        }
        if (showAllOption)
          pageSizeDropDownList?.dataSource?.add({ text: "All", value: "all" });
        pageSizeDropDownList?.dataSource?.sync();
        grid.dataSource.pageSize(25);
      }
    });
  }, 200);
}
function initPageSizeTreelistGrid() {
  setTimeout(function () {
    $('[data-role="treelist"]').each(function (index, element) {
      var arrSize = ["25", "50", "100", "500", "1000", "5000"];
      var grid = $(element).data("kendoTreeList");
      if (grid) {
        var pageSizeDropDownList = grid.wrapper
          .children(".k-grid-pager")
          .find("[data-role='dropdownlist']")
          .data("kendoDropDownList");
        pageSizeDropDownList?.dataSource?.data([]);
        for (let size of arrSize) {
          pageSizeDropDownList?.dataSource?.add({ text: size, value: size });
        }
        pageSizeDropDownList?.dataSource?.add({ text: "All", value: "all" });
        pageSizeDropDownList?.dataSource?.sync();
        grid.dataSource.pageSize(25);
      }
    });
  }, 200);
}
function clearValidation() {
  $(".has-error").each(function () {
    var $this = $(this);
    $this.removeClass("has-error");
  });
  $(".field-validation-error").each(function () {
    var $this = $(this);
    $this.removeClass("text-danger");
    $this.text("");
  });
  $(".text-danger").each(function () {
    var $this = $(this);
    $this.removeClass("text-danger");
  });
}

function roundDecimal(value, precision) {
  var multiplier = Math.pow(10, precision || 0);
  return Math.round(value * multiplier) / multiplier;
}

function dateTimeFilter(element) {
  element.kendoDateTimePicker({
    timeFormat: "HH:mm",
    format: "{0:MM/dd/yyyy HH:mm}",
    parseFormats: ["MM/dd/yyyy HH:mm"],
  });
}

function setDropDownListDS(id, arrayObj) {
    setWidgetDS(id, arrayObj, 'kendoDropDownList');
}
function setMultiSelectDS(id, arrayObj) {
    setWidgetDS(id, arrayObj, 'kendoMultiSelect');
}
function setAutoCompleteDS(id, arrayObj) {
    setWidgetDS(id, arrayObj, 'kendoAutoComplete');
}
function setWidgetDS(id, arrayObj, type) {
    var dataSource = new kendo.data.DataSource({
        data: arrayObj
    });
    var dropdownlist = $('#' + id).data(type);
    dropdownlist.setDataSource(dataSource);
}
function generateGuid() {
    return (S4() + S4() + "-" + S4() + "-4" + S4().substr(0, 3) + "-" + S4() + "-" + S4() + S4() + S4()).toLowerCase();
}
function S4() {
    return (((1 + Math.random()) * 0x10000) | 0).toString(16).substring(1);
}

function setAppFieldValue(field, value) {
    var dataType;
    if (field.FieldType == AppFieldType.TextBox || field.FieldType == AppFieldType.Calculator) {
        dataType = field.ColumnType == "VARCHAR" ? "kendoTextBox" : "kendoNumericTextBox";
    }
    else if (field.FieldType == AppFieldType.DropDown) {
        dataType = 'kendoDropDownList';
    }
    else if (field.FieldType == AppFieldType.DateEdit) {
        dataType = 'kendoDatePicker';
    }
    else if (field.FieldType == AppFieldType.TimeEdit) {
        dataType = 'kendoMaskedTextBox';
    }
    else if (field.FieldType == AppFieldType.Radio) {
        setAppFieldTypeRadio(field.ColumnName, value);
        return;
    }
    else if (field.FieldType == AppFieldType.CheckBox) {
        setAppFieldTypeCheckBox(field.ColumnName, value);
        return;
    }
    $("#" + field.ColumnName).data(dataType).value(value);
}
function setAppFieldTypeRadio(ColumnName, value) {
    $(`input[name="${ColumnName}"]`).each(function (idx, element) {
        if ($(element).val() == value) {
            $(element).prop('checked', true);
        }
    });
}
function setAppFieldTypeCheckBox(ColumnName, value) {
    $(`input[name="${ColumnName}"]`).each(function (idx, element) {
        value?.split('|')?.forEach(function (itemValue) {
            if ($(element).val().trim() == itemValue.trim())
                $(element).prop('checked', true);
        });
    });
}

function getAppFieldValue(field) {
    var dataType;
    if (field.FieldType == AppFieldType.TextBox || field.FieldType == AppFieldType.Calculator) {
        dataType = field.ColumnType == "VARCHAR" ? "kendoTextBox" : "kendoNumericTextBox";
    }
    else if (field.FieldType == AppFieldType.DropDown) {
        dataType = 'kendoDropDownList';
    }
    else if (field.FieldType == AppFieldType.DateEdit) {
        dataType = 'kendoDatePicker';
        return moment($("#" + field.ColumnName).data(dataType).value()).format('YYYY-MM-DD');
    }
    else if (field.FieldType == AppFieldType.TimeEdit) {
        dataType = 'kendoMaskedTextBox';
    }
    else if (field.FieldType == AppFieldType.Radio) {
        return $(`input[name="${field.ColumnName}"]:checked`).val();
    }
    else if (field.FieldType == AppFieldType.CheckBox) {
        let result = "";
        $(`input[name="${field.ColumnName}"]:checked`).each(function () {
            result += `${this.value}|`;
        });
        return trimChars(result, '|');
    }
    return $("#" + field.ColumnName).data(dataType).value();

}
