$(document).ready(function () {
    $("input[type='radio']").click(function() { 
        calculateTotal();
    });
    $("input[type='number']").change(function () {
        calculateTotal();
    });
    loadEnd();
});

function select(id) {
    $("input[value=" + id + "]").prop("checked", true);
    calculateTotal();
}

function calculateTotal() {
    //alert($("input[type='radio'][name='gb']:checked").val());
    //alert($("input[type='radio'][name='gateway']:checked").val());
    var perMeg = $("input[type='radio'][name='gb']:checked").val() == "oneGB" ? 1000 : 10000;
    var nniMRC = $("input[type='radio'][name='gateway']:checked").val() == "yes" ?
        0 : (perMeg == 1000 ? 885.28 : 954);
    
    //alert("((" + $("#NNI_MRC").val().toString() + " + " + nniMRC.toString() + ") / " + perMeg.toString() + " * " + $("#EVC").val() + " * .75)");
    $("#Total").val("$" + Math.round(((Number($("#NNI_MRC").val()) + nniMRC) / perMeg * Number($("#EVC").val()) * 1.25) * 100) / 100);
}