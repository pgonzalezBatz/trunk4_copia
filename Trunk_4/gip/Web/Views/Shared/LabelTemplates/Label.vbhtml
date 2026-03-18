@imports web
@Html.TextBox("", Model,New With{.class="calendar"}) 
<script type="text/javascript">
    $(function () {
        $.datepicker.regional["eu-ES"] = { // Default regional settings
            closeText: "Eginda", // Display text for close link
            prevText: "Aurr", // Display text for previous month link
            nextText: "Urr", // Display text for next month link
            currentText: "Gaur", // Display text for current month link
            monthNames: ['Urtarrila', 'Otsaila', 'Martxoa', 'Apirila', 'Maiatza', 'Ekaina',
    'Uztaila', 'Abuztua', 'Iraila', 'Urria', 'Azaroa', 'Abendua'], // Names of months for drop-down and formatting
            monthNamesShort: ['Urt', 'Ots', 'Mar', 'Api', 'Mai', 'Eka', 'Uzt', 'Abu', 'Ira', 'Urr', 'Aza', 'Abe'], // For formatting
            dayNames: ['Igandea', 'Astelehena', 'Asteartea', 'Asteazkena', 'Osteguna', 'Ostirala', 'Larunbata'], // For formatting
            dayNamesShort: ['Iga', 'Atl', 'Atr', 'Atz', 'Otg', 'Otr', 'Lar'], // For formatting
            dayNamesMin: ['Ig', 'Al', 'Ar', 'Az', 'Og', 'Or', 'La'], // Column headings for days starting at Sunday
            weekHeader: "Ast", // Column header for week of the year
            dateFormat: "yy/mm/dd", // See format options on parseDate
            firstDay: 1, // The first day of the week, Sun = 0, Mon = 1, ...
            isRTL: false, // True if right-to-left language, false if left-to-right
            showMonthAfterYear: false, // True if the year select precedes month, false for month then year
            yearSuffix: "" // Additional text to append to the year in the month headers
        };
        $.datepicker.regional["es-ES"] = { // Default regional settings
            closeText: "Echo", // Display text for close link
            prevText: "Atr", // Display text for previous month link
            nextText: "Sig", // Display text for next month link
            currentText: "Hoy", // Display text for current month link
            monthNames: ['Enero','Febrero','Marzo','Abril','Mayo','Junio',
    'Julio','Agosto','Septiembre','Octubre','Noviembre','Diciembre'], // Names of months for drop-down and formatting
            monthNamesShort: ['Ene', 'Feb', 'Mar', 'Abr', 'May', 'Jun', 'Jul', 'Ago', 'Sep', 'Oct', 'Nov', 'Dic'], // For formatting
            dayNames: ['Domingo', 'Lunes', 'Martes', 'Miercoles', 'Jueves', 'Viernes', 'Sabado'], // For formatting
            dayNamesShort: ['Dom', 'Lun', 'Mar', 'Mie', 'Jue', 'Vie', 'Sab'], // For formatting
            dayNamesMin: ['Do', 'Lu', 'Ma', 'Mi', 'Ju', 'Vi', 'Sa'], // Column headings for days starting at Sunday
            weekHeader: "Ast", // Column header for week of the year
            dateFormat: "dd/mm/yy", // See format options on parseDate
            firstDay: 1, // The first day of the week, Sun = 0, Mon = 1, ...
            isRTL: false, // True if right-to-left language, false if left-to-right
            showMonthAfterYear: false, // True if the year select precedes month, false for month then year
            yearSuffix: "" // Additional text to append to the year in the month headers
        };
        $('.calendar').datepicker($.datepicker.regional["@h.GetCulture()"]);
    });
</script>