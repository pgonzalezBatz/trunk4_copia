@Code
    ViewData("Title") = "Reassign2"
    Layout = "~/Views/Shared/_Layout.vbhtml"

    Dim oracleDatabase As New oracleDB
    Dim aCookie As HttpCookie = Request.Cookies("BezerreSis_Filtro")
    Dim idPlantaSAB = If(Model.CLASIFICACION = 3, oracleDatabase.getIdSabPlantaForClienteFilial(Model.CLIENTE), aCookie.Values("IdPlanta"))
    If ConfigurationManager.AppSettings("PlantasFiliales").Split(",").Contains(Model.CLIENTE) Then
        idPlantaSAB = oracleDatabase.getIdSabPlantaForClienteFilial(Model.CLIENTE)
    End If
    Dim options = oracleDatabase.getNCsToLink(idPlantaSAB, Model.id)

    Dim intranetPrefix As String = "intranet-test.batz.es"
    If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
        intranetPrefix = "intranet2.batz.es"
    End If
End Code

<h2>Vincular</h2>
<br />
<h4 class="text-center">Listado de NCs disponibles:</h4>

@If options.Count > 0 Then
    @<Table Class="table">
        <tbody>
            <tr style = "color: white; background-color: rgb(51, 122, 183);" >
                <th>Id GTK</th>
                <th>Descripcion</th>
                <th>Acciones</th>
            </tr>
            @For Each opt In options
                @<tr onclick="if(confirm('Esta acción vinculará la reclamación con la NC elegida, y sobreescribirá la información contenida en GTK. Estás seguro?')){location.href = '@Url.Action("Reassign3", New With {.idGtk = opt.id, .idBezerresis = Model.id})'}" title="Vincular" data-toggle="tooltip" data-container="body">
                    <td>@opt.id </td>
                    <td>@opt.label </td>
                    <td>
                        <Button Class="btn btn-default" onclick="window.open('https://@intranetPrefix/GertakariakSA/Index.aspx?idincidencia=@opt.id','_blank');event.stopPropagation();" title="Ver en GTK" data-toggle="tooltip">
                            <i class="glyphicon glyphicon-eye-open"></i>
                        </Button>
                    </td>
                </tr>
            Next
        </tbody>
    </table>
Else
    @<h4 class="text-center" style="color:red;font-style:italic">No hay NCs que cumplan los criterios para la vinculación</h4>
End If


@Section Scripts
    <script type="text/javascript">
        $(document).on('show.bs.tooltip', function (e) {
            $('.tooltip').not(e.target).hide();
        });
    </script>
End Section