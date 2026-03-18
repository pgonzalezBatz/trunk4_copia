$(function () {
    $(".numeric").on("keypress keyup blur", function (e) {
        /* Sólo acepta números y decimales separados por punto*/
        //$(this).val($(this).val().replace(/[^0-9\.]/g,''));
        //if ((event.which != 46 || $(this).val().indexOf('.') != -1) && (event.which < 48 || event.which > 57)) {
        //    event.preventDefault();
        //}        
        var ret = true
        var keyCode = e.which ? e.which : e.keyCode
        if (keyCode) {
            ret = (keyCode >= 48 && keyCode <= 57);
        }
        return ret;
    });
})