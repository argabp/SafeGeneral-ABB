function disableGridTextboxWhenEdit(dataItem){
    return !!dataItem.isNew();
}

function disableGridTextbox(dataItem){
    return false;
}