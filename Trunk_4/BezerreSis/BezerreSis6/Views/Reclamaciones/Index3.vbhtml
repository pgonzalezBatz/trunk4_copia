@ModelType IEnumerable(Of BezerreSis.ReclamacionViewModel)
@Code
    ViewData("Title") = "Reclamaciones"
    Dim isOficial = ViewBag.Oficial

    Dim reporteCognosExcel = ""
    Dim reporteCognosPdf = ""
    If isOficial Then
        reporteCognosExcel = "https://cognos.batz.es/ibmcognos/bi/?objRef=iEB3FDFF69D914F6FA2DFF5936F4A374D&prompt=true&ui_appbar=false&ui_navbar=false&format=spreadsheetML"
        reporteCognosPdf = "https://cognos.batz.es/ibmcognos/bi/?objRef=iEB3FDFF69D914F6FA2DFF5936F4A374D&prompt=true&ui_appbar=false&ui_navbar=false&format=PDF"
    Else
        reporteCognosExcel = "https://cognos.batz.es/ibmcognos/bi/?objRef=i01AFBA00CF2D418DB1428FBCA74A3957&prompt=true&ui_appbar=false&ui_navbar=false&format=spreadsheetML"
        reporteCognosPdf = "https://cognos.batz.es/ibmcognos/bi/?objRef=i01AFBA00CF2D418DB1428FBCA74A3957&prompt=true&ui_appbar=false&ui_navbar=false&format=PDF"

    End If

    Dim REPETITIVAID = System.Configuration.ConfigurationManager.AppSettings.Get("repetitivaId")
    Dim NIVELIMPORTANCIAID = System.Configuration.ConfigurationManager.AppSettings.Get("nivelImportanciaId")
    Dim idUser = Session("idUser")
    Dim adminIds = System.Configuration.ConfigurationManager.AppSettings.Get("Admins").Split(",")

    If Model.Count > 0 Then
        ViewBag.ShowTable = True
    End If
End Code

<br />
<h2>@ViewBag.MyTitle</h2>
<p>
    <div style="text-align:center">
        <button type="button" class="btn btn-info" onclick="window.location.href = '@Url.Action("Create")';" style="font-size:18px">
            <i class="glyphicon glyphicon-plus" style="margin-right: 10px;"></i>Nueva Reclamación
        </button>
    </div>
</p>

<div style="border:1px solid #ccc;border-radius:5px;background-color:#eee">
    <br />
    @Using (Html.BeginForm("Index_Filtro", "RECLAMACIONES", FormMethod.Get))
        @Html.AntiForgeryToken()
        @<div Class="row filters">
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Cliente", htmlAttributes:=New With {.Class = "control-label"})
                </h4>
                <p class="col-md-6">
                    @Html.DropDownList("lClientes", Nothing, htmlAttributes:=New With {.Class = "form-control multiDropdown", .multiple = "multiple"})
                </p>
            </div>
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Productos", htmlAttributes:=New With {.Class = "control-label"})
                </h4>
                <p class="col-md-6">
                    @Html.DropDownList("lProductos", Nothing, htmlAttributes:=New With {.Class = "form-control multiDropdown", .multiple = "multiple"})
                    @Html.Hidden("productosAll")
                </p>
            </div>

        </div>
        @<div Class="row filters">
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Estado", htmlAttributes:=New With {.Class = "control-label"})
                </h4>
                <p class="col-md-6">
                    @Html.DropDownList("lEstados", Nothing, htmlAttributes:=New With {.Class = "form-control multiDropdown", .multiple = "multiple"})
                </p>
            </div>
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Creador", htmlAttributes:=New With {.Class = "control-label"})
                </h4>
                <p class="col-md-6">
                    @Html.DropDownList("lCreadores", Nothing, htmlAttributes:=New With {.Class = "form-control multiDropdown", .multiple = "multiple"})
                </p>
            </div>
        </div>
        @<div Class="row filters">
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Fecha desde", htmlAttributes:=New With {.class = "control-label"})
                </h4>
                <div class='input-group date myDatePicker' id='datetimepicker_1'>
                    @Html.TextBox("FECHA_DESDE", If(ViewBag.FECHA_DESDE Is Nothing, Date.ParseExact("01/01/2019", "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd"), Date.ParseExact(ViewBag.FECHA_DESDE, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd")), New With {.class = "form-control"})
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
            <div Class="col-md-6">
                <h4 class="col-md-6">
                    @Html.Label("Fecha hasta", htmlAttributes:=New With {.class = "control-label"})
                </h4>
                <div class='input-group date myDatePicker' id='datetimepicker_2'>
                    @Html.TextBox("FECHA_HASTA", If(ViewBag.FECHA_HASTA Is Nothing, Date.Today.ToString("yyyy/MM/dd"), Date.ParseExact(ViewBag.FECHA_HASTA, "dd/MM/yyyy", System.Globalization.DateTimeFormatInfo.InvariantInfo).ToString("yyyy/MM/dd")), New With {.class = "form-control"})
                    <span class="input-group-addon">
                        <span class="glyphicon glyphicon-calendar"></span>
                    </span>
                </div>
            </div>
        </div>
        @<div class="hidden-md"><br /></div>
        @<div style="text-align:center">
            <button type="submit" Class="btn btn-success" onclick="seleccionarTodo(); return false;">
                <i class="glyphicon glyphicon-ok" style="margin-right:10px;"></i>Seleccionar todo
            </button>
            <button type="submit" Class="btn btn-danger" onclick="resetearTodo(); return false;">
                <i class="glyphicon glyphicon-refresh" style="margin-right:10px;"></i>Borrar
            </button>
        </div>
        @<p>
            <div id="divBusqueda" style="text-align:center">
                <button type="submit" Class="btn btn-primary" onclick="return checkFilters('lProductos');" style="font-size:18px">
                    <i class="glyphicon glyphicon-search" style="margin-right:10px;"></i>Buscar
                </button>
                @If ViewBag.MuestraMsg Then@<span id="msgError" class="muestraMsg">Debes seleccionar todos los filtros</span>
                Else
                    @<span>&nbsp;</span>
                End If
            </div>
        </p>
        @<br/>@<br/>
        @<p class="reportes" style="text-align:center">
            <span>Reportes COGNOS: </span>
            <a href="@reporteCognosExcel" target="_blank"><img src="~/Content/themes/base/Imagenes/excel.png"  /></a>
            <a href="@reporteCognosPdf" target="_blank"><img src="~/Content/themes/base/Imagenes/pdf.png"  /></a>
        </p>
    End Using
</div>
<br />
@If ViewBag.ShowTable Then
    @<div id="resumenFiltros" class="resumenFiltros">
        <h4 style="margin-left:20px;color:#337ab7"><strong> Filtros seleccionados</strong></h4>
        <div style="margin-left:40px">
            <strong>CLIENTES: </strong>@ViewBag.clientesResumen
            <br />
            <strong>PRODUCTOS: </strong> @ViewBag.productosResumen
            <br />
            <strong>ESTADOS: </strong> @ViewBag.estadosResumen
            <br />
            <strong>CREADORES: </strong> @ViewBag.creadoresResumen
            <br />
        </div>
    </div>
    @<br />

    @<table class="table resultTable" id="reclamacionesTable">
    <tr style="background-color:#337ab7;color:white">
        <th>@Html.DisplayNameFor(Function(model) model.ESTADO)</th>
        <th>@Html.DisplayNameFor(Function(model) model.CODIGOGTK)</th>
        <th>@Html.DisplayNameFor(Function(model) model.FECHACREACION)</th>
        <th>@Html.DisplayNameFor(Function(model) model.FFIN_PREVISTO_E56)</th>
        <th>@Html.DisplayNameFor(Function(model) model.CREADOR)</th>
        <th>@Html.DisplayNameFor(Function(model) model.RESPONSABLE_O_PERSEGUIDOR)</th>
        <th>@Html.DisplayNameFor(Function(model) model.REFINTERNAPIEZA)</th>
        <th>@Html.DisplayNameFor(Function(model) model.DENOMINACION)</th>
        <th>@Html.DisplayNameFor(Function(model) model.CLIENTE)</th>
        <th>@Html.DisplayNameFor(Function(model) model.PRODUCTO)</th>
        <th>@Html.DisplayNameFor(Function(model) model.CODXCLIENTE)</th>
        <th>@Html.DisplayNameFor(Function(model) model.NUMPIEZASNOK)</th>
        <th>@Html.DisplayNameFor(Function(model) model.DESCRIPCION)</th>
        <th>@Html.DisplayNameFor(Function(model) model.PROCEDENCIA)</th>
        <th>@Html.DisplayNameFor(Function(model) model.CLASIFICACION)</th>
        <th>@Html.DisplayNameFor(Function(model) model.REPETITIVA)</th>
        @If Not isOficial Then
            @<th>@Html.DisplayNameFor(Function(model) model.OFICIAL)</th>
        End If
        <th>@Html.DisplayNameFor(Function(model) model.AFECTA_INDICADORES)</th>
        <th></th>
    </tr>
    @For Each item In Model
        @Code
            Dim disabled = "enabled"
            If Not item.CREADOR_ID = idUser AndAlso Not adminIds.Contains(idUser) Then
                disabled = "disabled"
            End If
            Dim oracleDatabase As New oracleDB
            Dim myDatabase As New myDb
            Dim glyphiconName As String
            Dim estado As String
            Dim bgColor As String
            Dim cierreTitle As String
            'Dim fechaFinPrevisto = "-"
            Dim fechaFinStyle = ""
            If item.CODIGOGTK IsNot Nothing AndAlso item.CODIGOGTK > 0 Then
                If Not item.FFIN_PREVISTO_E56.Equals(Date.MinValue) AndAlso (item.FFIN_PREVISTO_E56 < item.FECHA_CIERRE OrElse (item.FFIN_PREVISTO_E56 < Now.Date And item.FECHA_CIERRE = Date.MinValue)) Then
                    fechaFinStyle = "color:white;background-color:red"
                End If
            End If
            Dim intranetPrefix As String = "intranet-test.batz.es"
            If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                intranetPrefix = "intranet2.batz.es"
            End If
            If item IsNot Nothing AndAlso item.FECHA_RESP_CONTENCION > Date.MinValue AndAlso item.FECHA_RESP_CORRECTIVAS > Date.MinValue Then
                glyphiconName = "glyphicon-edit"
                estado = "Cerrada"
                bgColor = "darkSeaGreen"
                cierreTitle = "Editar cierre"
            Else
                glyphiconName = "glyphicon-check"
                estado = "Abierta"
                bgColor = "indianRed"
                cierreTitle = "Cerrar"
            End If
            Dim vinculoTitle As String
            Dim vinculoFuncion As String
            Dim vinculoGlyphicon As String

            If item.CODIGOGTK Is Nothing OrElse item.CODIGOGTK < 1 Then
                vinculoTitle = "Vincular"
                vinculoFuncion = "Reassign2"
                vinculoGlyphicon = "glyphicon-sort"
            Else
                vinculoTitle = "Desvincular"
                vinculoFuncion = "Unassign"
                vinculoGlyphicon = "glyphicon-sort line-through"
            End If

            Dim fechaCreacion As String = item.FECHACREACION.ToShortDateString
        End Code
        @<tr onclick="location.href = '@Url.Action("Details", New With {.id = item.ID})'" title="Detalle" data-toggle="tooltip" data-placement="top" data-container="body">
    <td style="background-color:@bgColor;color:white;">@estado</td>
    <td>
        @If item.CODIGOGTK Is Nothing OrElse item.CODIGOGTK <= 0 Then
            @<span>-</span>
        Else
            @<Button Class="btn btn-default" onclick="window.open('https://@intranetPrefix/GertakariakSA/Index.aspx?idincidencia=@item.CODIGOGTK','_blank');event.stopPropagation();" title="Ir a GTK" data-toggle="tooltip">
                <span>@myDatabase.getNomenclatura(item.CODIGOGTK, item.PROCEDENCIA)</span>
            </Button>
        End If
    </td>
    @*<td>@Html.DisplayFor(Function(o) item.FECHACREACION.ToShortDateString)</td>*@
    <td>@Html.DisplayFor(Function(o) fechaCreacion)</td>
    <td><span style=@fechaFinStyle>@(If(item.FFIN_PREVISTO_E56.Equals(Date.MinValue), "-", item.FFIN_PREVISTO_E56.ToShortDateString))</span></td>
    <td>@item.CREADOR</td>
    <td id="nombreCompletoTd">@item.RESPONSABLE_O_PERSEGUIDOR</td>
    <td>@item.REFINTERNAPIEZA</td>
    <td>@item.DENOMINACION</td>
    <td>@item.CLIENTE</td>
    <td>@item.PRODUCTO</td>
    <td>@item.CODXCLIENTE</td>
    <td>@Html.DisplayFor(Function(o) item.NUMPIEZASNOK)</td>
    <td>@item.DESCRIPCION</td>
    <td>@item.PROCEDENCIA</td>
    <td>@item.CLASIFICACION</td>
    <td>@item.REPETITIVA</td>
    @If Not isOficial Then
        @<td>@item.OFICIAL</td>
    End If
    <td>@item.AFECTA_INDICADORES</td>
    <td style="width:196px;">
        <div class="btn-group" role="group" aria-label="..." style="width:180px;">
            <button class="btn btn-default" onclick="location.href = '@Url.Action("Edit", New With {.id = item.ID})';event.stopPropagation();" title="Editar" data-toggle="tooltip" data-container="body" @disabled>
                <span class="glyphicon glyphicon-pencil" style="color:royalblue" aria-hidden="true"></span>
            </button>
            <button class="btn btn-default" onclick="location.href = '@Url.Action("Close", New With {.id = item.ID})';event.stopPropagation();" title="@cierreTitle" data-toggle="tooltip" @disabled>
                <span class="glyphicon @glyphiconName" style="color:green" aria-hidden="true"></span>
            </button>
            <button class="btn btn-default" onclick="if(confirm('Estás seguro de que deseas borrar esta reclamación? La NC asignada no se borrará de GTK, tendrás que hacerlo manualmente si procede')){location.href = '@Url.Action("Delete", New With {.id = item.ID})'};event.stopPropagation();" title="Borrar" data-toggle="tooltip" @disabled>
                <span class="glyphicon glyphicon-remove" style="color:red" aria-hidden="true"></span>
            </button>
            @If vinculoTitle.Equals("Vincular") Then
                @<button Class="btn btn-default" onclick="location.href = '@Url.Action(vinculoFuncion, New With {.id = item.ID})';event.stopPropagation();" title="@vinculoTitle" data-toggle="tooltip" @disabled>
                    <span class="glyphicon @vinculoGlyphicon" style="color:orange" aria-hidden="true"></span>
                </button>
            Else
                @<button Class="btn btn-default" onclick="if(confirm('Estás seguro de que deseas desvincular esta reclamación de su NC correspondiente de GTK?')){location.href = '@Url.Action(vinculoFuncion, New With {.id = item.ID})'};event.stopPropagation();" title="@vinculoTitle" data-toggle="tooltip" @disabled>
                    <span class="glyphicon @vinculoGlyphicon" style="color:orange" aria-hidden="true"></span>
                </button>
            End If
        </div>
    </td>
</tr>
            Next
</table>
            End If
<br />

@Html.Hidden("recienBorrado", ViewBag.Modal)
<!-- Modal -->
<div class="modal fade" id="ElementoBorrado" tabindex="-1" role="dialog">
    <div class="modal-dialog" role="document" style="width:300px;">
        <div class="modal-content">
            <div class="modal-body" style="text-align:center">
                <h4> Elemento borrado!</h4>
                <button class="btn btn-lg btn-info" data-dismiss="modal">Aceptar</button>
            </div>
        </div>
    </div>
</div>

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    @Scripts.Render("~/bundles/filters")
    <script>
        window.onload = function () {
            var elemBorrado = $('#recienBorrado')[0].value;
            if (elemBorrado == "DeleteOK") {
                ElementoBorrado();
            }
        };
        function ElementoBorrado() {
            $("#ElementoBorrado").modal();
        }
        function atajoTeclado(e) {
            var evtobj = window.event ? event : e
            if (evtobj.ctrlKey && evtobj.shiftKey && evtobj.keyCode == 49) { //ctrl + shift + 1
                window.location.href = "@Url.Action("Index", "Reclamaciones")"
            } else if (evtobj.ctrlKey && evtobj.shiftKey && evtobj.keyCode == 50) { // ctrl + shift + 2
                window.location.href = "@Url.Action("Index2", "Reclamaciones")"
            }
        }
        document.onkeydown = atajoTeclado;
        $('.myDatePicker').datetimepicker({
            format: 'YYYY/MM/DD',
            defaultDate: @Date.Today.ToString("yyyy/MM/dd"),
            maxDate: moment()
        });


        $(function () {
            $.validator.addMethod('date',
                function (value, element) {
                    if (this.optional(element)) {
                        return true;
                    }
                    var valid = true;
                    try {
                        $.datepicker.parseDate('yy/mm/dd', value);
                    }
                    catch (err) {
                        valid = false;
                    }
                    return valid;
                });
            //$(".datetype").datepicker({ dateFormat: 'dd/mm/yy' });
        });

        $(document).ready(function () {
            var showTable = '@ViewBag.ShowTable';
            var muestraMsg = '@ViewBag.MuestraMsg';
            if (showTable == 'False' && muestraMsg == 'False') {
                seleccionarTodo();
            }
            retocaCreadores();
        })

        function retocaCreadores() {
            //$('#lCreadores > option').each(function () {
            //    $(this).removeAttr('disabled');
            //    $(this).removeProp('disabled');
            //    //$(this).css('color', '#ffcccc');
            //});
            //$('#lCreadores').next().find('ul > li > a > label > input').each(function () {

            //    debugger;
            //    if ($(this).value.charAt(0) == '0') {
            //        $(this).css('color', 'red');
            //    }
            //    //$(this).removeClass('disabled');
            //    //$(this).addClass('active');
            //    //$(this).find('input').attr('enabled', 'enabled');
            //    //$(this).find('input').css('cursor', 'pointer');
            //    ////debugger;

            //    //var d1 = $(this).children('a');
            //    //d1.css('color', 'ffcccc');
            //});
            //$('#lCreadores > option').each(function () {
            //    if (this.value.substring(0, 2) == '0-') {
            //        $(this).css('color', '#ffcccc');
            //    }
            //});
            $('#lCreadores').next().find('ul > li > a > label > input').each(function () {
                if (this.value.substring(0, 2) == '0-') {
                    this.parentElement.style.color = '#f66';
                }
            });
        }        
    </script>
End Section
