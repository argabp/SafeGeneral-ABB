$(document).ready(function () {
    btnPreviousOther();
    
    if($("#kd_cob").val().trim() == "H"){
        $("#label_no_rangka").text("Nomor IMO");
        $("#label_no_msn").text("Nomor Register");
    }else {
        $("#label_no_rangka").text("Nomor Rangka");
        $("#label_no_msn").text("Nomor Mesin");
    }
});

function btnPreviousOther(){
    $('#btn-previous-inquiryResikoOther').click(function () {
        $("#resikoTab").getKendoTabStrip().select(2);
    });
}
