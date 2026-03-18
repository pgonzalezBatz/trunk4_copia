
    $(document).ready(function () {
        $('.multiDropdown').multiselect({
            includeSelectAllOption: true,
            selectAllText: 'TODOS',
            selectAllValue: 0,
            nonSelectedText: '-----',
            buttonText: function (options, select) {
                var labels = [];
                options.each(function () {
                    if ($(this).attr('label') !== undefined) {
                        labels.push($(this).attr('label'));
                    }
                    else {
                        labels.push($(this).html());
                    }
                });
                var labelsJoined = labels.join(', ') + '';
                if (options.length === 0) {
                    return '.....\xa0\xa0';
                }
                else if (options.length == options.context.length) {
                    return 'Todos\xa0\xa0';
                }
                else if (options.length === 1) {
                    return options[0].label + '\xa0\xa0';
                }
                else if (options.length > 4 || labelsJoined.length > 22) {
                    return '' + options.length + ' items elegidos';
                }
                else {
                    return labelsJoined + '\xa0';
                }
            }
        });

        var showTable = '@ViewBag.ShowTable';
        var muestraMsg = '@ViewBag.MuestraMsg';
        if (showTable == 'False' && muestraMsg == 'False') {
            seleccionarTodo();
        }
        $('.multiselect-native-select > .btn-group > button').attr('title', '');
    });

    function seleccionarTodo() {
        $('.multiDropdown').multiselect('selectAll', false);
        $(".multiDropdown").multiselect('updateButtonText');
    }

    function resetearTodo() {
        $('.multiDropdown').multiselect('deselectAll', false);
        $(".multiDropdown").multiselect('updateButtonText');
    }


    $(document).on('show.bs.tooltip', function (e) {
        $('.tooltip').not(e.target).hide();
    });




    function checkFilters(filterId) {
        var myId = '' + filterId;
        if (filterId instanceof HTMLSelectElement) {
            myId = filterId.id;
        }
        var multiselect_dropdowns = $('ul.multiselect-container');
        for (var i = 0; i < multiselect_dropdowns.length; i++) {
            var dropdown = multiselect_dropdowns[i];
            var isActive = false;
            for (var j = 0; j < dropdown.children.length; j++) {
                var select_item = dropdown.children[j];
                if (select_item.classList.contains("active")) {
                    isActive = true;
                    break;
                }
            }
            if (!isActive) {
                $('#divBusqueda').html("<button type='submit' Class='btn btn-primary' onclick='return checkFilters(" + myId + ");' style='font-size: 18px'><i class='glyphicon glyphicon-search' style='margin-right:10px;'></i>Buscar</button>&nbsp;<span style='color:red;font-style:italic;position:absolute;transform:translateY(50%);' id='msgError' class='muestraMsg'>Debes seleccionar todos los filtros</span>");
                $('.resultTable').css("display", "none");
                $('#resumenFiltros').css("display", "none");
                return false;
            }
        }
        var myDiv1a = document.getElementById(myId).nextSibling.children[1].children[0].classList;
        var hiddenProd = document.getElementById("productosAll");
        hiddenProd.value = myDiv1a;
    }