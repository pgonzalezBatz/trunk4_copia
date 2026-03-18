@*@ModelType BezerreSis.RECLAMACIONES
@Code
    ViewData("Title") = "Reassign"
    Dim oracleDB As New oracleDB
    Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
    Dim idPlantaSAB = If(Model.CLASIFICACION = 3, oracleDB.getIdSabPlantaForClienteFilial(Model.CLIENTE), aCookie.Values("IdPlanta"))

    Dim intranetPrefix As String = "intranet-test.batz.es"
    If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
        intranetPrefix = "intranet2.batz.es"
    End If
End Code

<h2>Vincular</h2>
<br />

@Using (Html.BeginForm())
    @Html.AntiForgeryToken()
    @<div class="form-horizontal">
        <dl class="dl-horizontal">
            <dt>
                @Html.Label("Buscar en GTK (Código ID de NC)", htmlAttributes:=New With {.class = "control-label"})
            </dt>
            <dd>
                @Html.Editor("BUSCARENGTK", New With {.htmlAttributes = New With {.class = "form-control", .style = "display:inline-block!important", .autofocus = "autofocus"}})
                <button id="verButton" type="button" class="btn btn-default" style="visibility:hidden" title="Ver en GTK" data-toggle="tooltip">
                    <i class="glyphicon glyphicon-eye-open"></i>
                </button>
                <button id="borrarButton" type="button" class="btn btn-default" onclick="borrarNC();" style="visibility:hidden" title="Borrar" data-toggle="tooltip">
                    <i class="glyphicon glyphicon-trash"></i>
                </button>
            </dd>
        </dl>
    </div>
    @<div id="divAsignar" style="text-align:center">
        <button id="asignarButton" type="submit" Class="btn btn-info" style="font-size:18px;" onclick="return checkData();" disabled="disabled">
            Vincular
        </button>
    </div>
                                        @Html.Hidden("IDBEZERRESIS", Model.ID)
End Using

@Section Scripts
    @Scripts.Render("~/bundles/jqueryval")
    <script type="text/javascript">
         $(function () {
                $('#BUSCARENGTK').autocomplete({
                    source: '@Url.Action("SuggestNC", New With {.emp = idPlantaSAB, .id = Model.ID})',
                    minLength: 1,
                    select: function (event, ui) {
                        $(this).attr("readonly", "readonly");
                        $('#borrarButton').css("visibility","visible");
                        $('#verButton').css("visibility","visible");
                        $('#verButton').attr("onclick", "window.open('https://@intranetPrefix/GertakariakSA/Index.aspx?idincidencia=" + ui.item.value + "','_blank');");
                        $('#asignarButton').removeAttr("disabled");
                    }
                });
        });

        function checkData() {
            var exists = $('#BUSCARENGTK').val().length > 0;
            if (exists) {
                var result = confirm("Esta acción vinculará la reclamación con la NC elegida, y sobreescribirá la información contenida en GTK. Estás seguro?");
                if (result) {
                    return true;
                } else {
                    $('#divAsignar').html("<input id='asignarButton' type='submit' value='Asignar' Class='btn btn-info' style='font-size: 18px;' onclick='return checkData()'/>");
                    return false;
                }
            }
            else {
                $('#divAsignar').html("<input id='asignarButton' type='submit' value='Asignar' Class='btn btn-info' style='font-size: 18px;' onclick='return checkData()'/><span style='color:red;font-style:italic;position:absolute;transform:translateY(50%);margin-left:10px;' id='msgError' class='muestraMsg'>Debes elegir una NC</span>"); /* aquí no va a entrar nunca, ya que está 'disabled' */
                return false;
            }
        }
        
        function borrarNC() {
            $('#BUSCARENGTK').removeAttr("readonly");
            $('#BUSCARENGTK').val("");
            $('#verButton').css("visibility","hidden")
            $('#borrarButton').css("visibility", "hidden")
            $('#asignarButton').attr("disabled", "disabled");
        }

    </script>
End Section*@
