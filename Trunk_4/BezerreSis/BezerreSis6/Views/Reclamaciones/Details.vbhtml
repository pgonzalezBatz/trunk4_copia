@ModelType BezerreSis.RECLAMACIONES
@Code
    ViewData("Title") = "Details"
    Dim oficialVisible = If(TempData.Peek("ReturnUrl").Equals("Index"), "none", "")
    Dim idUser = Session("idUser")
    Dim adminIds = System.Configuration.ConfigurationManager.AppSettings.Get("Admins").Split(",")
    Dim disabled = "enabled"
    If Not Model.CREADOR = idUser AndAlso Not adminIds.Contains(idUser) Then
        disabled = "disabled"
    End If

    Dim myDB As New myDb
    Dim oracleDB As New oracleDB
    Dim myCreador = oracleDB.getCreadorFromId(Model.CREADOR)
    Dim myProcedencia = myDB.getProcedencia(Model.PROCEDENCIA)
    Dim myClasificacion = myDB.getClasificacion(Model.CLASIFICACION)
    Dim myReclamacion = myDB.getReclamacion(Model.RECLAMACIONOFICIAL)
    Dim myImportancia = oracleDB.getMyEstructura(Configuration.ConfigurationManager.AppSettings("nivelImportanciaId"), Model.NIVELIMPORTANCIA)
    Dim myRepetitiva = oracleDB.getMyEstructura(Configuration.ConfigurationManager.AppSettings("repetitivaId"), Model.REPETITIVA)

    Dim codigoEnGtk As String = oracleDB.getCodigoGtk(Model.ID)
    Dim codigoNumericoEnGtk As String = oracleDB.getCodigoNumericoGtk(Model.ID)
    Dim nombreCompletoResponsable = "-"
    Dim fechaFinPrevisto = "-"
    Dim fechaFinStyle = ""
    If Not codigoEnGtk.Equals("-") Then
        If codigoEnGtk.StartsWith("NCI-") OrElse codigoEnGtk.StartsWith("NCPP-") Then
            nombreCompletoResponsable = oracleDB.getResponsable(codigoNumericoEnGtk)
        ElseIf codigoEnGtk.StartsWith("NCP-") Then
            nombreCompletoResponsable = oracleDB.getPerseguidor(codigoNumericoEnGtk)
        End If
        fechaFinPrevisto = oracleDB.getFechaFinPrevisto(codigoNumericoEnGtk)
        If Not fechaFinPrevisto.Equals("?") AndAlso oracleDB.isFechaFinOutdated(codigoNumericoEnGtk, fechaFinPrevisto) Then
            fechaFinStyle = "color:white;background-color:red"
        End If
    End If
    Dim intranetPrefix As String = "intranet-test.batz.es"
    If (ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
        intranetPrefix = "intranet2.batz.es"
    End If

    Dim db As New Entities_BezerreSis
    Dim recCierre = db.RECLAMACIONES_CIERRE.Find(Model.ID)
    Dim hayDatosCierre = False
    Dim closureGlyph
    If recCierre IsNot Nothing Then
        hayDatosCierre = True
    End If
    Dim estado As String
    'If recCierre IsNot Nothing AndAlso recCierre.FECHA_RESP_CONTENCION > Date.MinValue AndAlso recCierre.FECHA_RESP_CORRECTIVAS > Date.MinValue Then 
    If recCierre IsNot Nothing AndAlso recCierre.FECHA_CIERRECLIENTE > Date.MinValue Then '''' cambiamos la lógica para que lo que importe sea la fecha_cierrecliente
        estado = "Cerrada"
        closureGlyph = "edit"
    Else
        estado = "Abierta"
        closureGlyph = "check"
    End If
End Code
<br />
<h2>Detalle</h2>
<br />
<div>
    <h3 style="text-align:center">DATOS RECLAMACIÓN</h3>
    @If Not codigoEnGtk.Equals("-") Then
        @<h3 style="text-align:center">
            <Button Class="btn btn-default" onclick="window.open('https://@intranetPrefix/GertakariakSA/Index.aspx?idincidencia=@codigoNumericoEnGtk','_blank');event.stopPropagation();" title="Ir a GTK" data-toggle="tooltip">
                <span>@codigoEnGtk</span>
            </Button>
        </h3>
    End If
    <dl class="dl-horizontal">
        <dt>@Html.DisplayNameFor(Function(model) model.FECHACREACION)</dt>
        <dd>@Html.DisplayFor(Function(model) model.FECHACREACION)</dd>
        <dt>Fecha fin previsto Etapa 5-6</dt>
        <dd><span style=@fechaFinStyle>@fechaFinPrevisto</span></dd>
        <dt>@Html.DisplayNameFor(Function(model) model.CREADOR)</dt>
        <dd>@myCreador</dd>
        <dt>Responsable o Perseguidor</dt>
        <dd>@Html.Raw(nombreCompletoResponsable)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.REFINTERNAPIEZA)</dt>
        <dd>@Html.DisplayFor(Function(model) model.REFINTERNAPIEZA)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.DENOMINACION)</dt>
        <dd>@Html.DisplayFor(Function(model) model.DENOMINACION)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.PROYECTO)</dt>
        <dd>@Html.DisplayFor(Function(model) model.PROYECTO)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.REFCLIENTE)</dt>
        <dd>@Html.DisplayFor(Function(model) model.REFCLIENTE)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.CLIENTE)</dt>
        <dd>@Html.DisplayFor(Function(model) model.CLIENTES.NOMBRE)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.PRODUCTO)</dt>
        <dd>@Html.DisplayFor(Function(model) model.PRODUCTOS.NOMBRE)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.CODXCLIENTE)</dt>
        <dd>@Html.DisplayFor(Function(model) model.CODXCLIENTE)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.NUMPIEZASNOK)</dt>
        <dd>@Html.DisplayFor(Function(model) model.NUMPIEZASNOK)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.DESCRIPCION)</dt>
        <dd>@Html.DisplayFor(Function(model) model.DESCRIPCION)</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.PROCEDENCIA)</dt>
        <dd>@myProcedencia</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.CLASIFICACION)</dt>
        <dd>@myClasificacion</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.NIVELIMPORTANCIA)</dt>
        <dd>@myImportancia</dd>
        <dt>@Html.DisplayNameFor(Function(model) model.REPETITIVA)</dt>
        <dd>@myRepetitiva</dd>
        <dt style="display:@oficialVisible">@Html.DisplayNameFor(Function(model) model.RECLAMACIONOFICIAL)</dt>
        <dd style="display:@oficialVisible">@myReclamacion</dd>
    </dl>
</div>

<h3 style="text-align:center">DATOS CIERRE</h3>
<div>
    <dl class="dl-horizontal">
        <dt>Estado</dt>
        <dd>@estado</dd>
        <dt>@Html.DisplayNameFor(Function(o) recCierre.FECHA_RESP_CONTENCION)</dt>
        <dd>
            @If recCierre IsNot Nothing AndAlso recCierre.FECHA_RESP_CONTENCION IsNot Nothing AndAlso recCierre.FECHA_RESP_CONTENCION > Date.MinValue Then
                @CDate(recCierre.FECHA_RESP_CONTENCION).ToString("yyyy/MM/dd")
            Else
                @<span>-</span>
            End If
        </dd>
        <dt>@Html.DisplayNameFor(Function(o) recCierre.FECHA_RESP_CORRECTIVAS)</dt>
        <dd>
            @If recCierre IsNot Nothing AndAlso recCierre.FECHA_RESP_CORRECTIVAS IsNot Nothing AndAlso recCierre.FECHA_RESP_CORRECTIVAS > Date.MinValue Then
                @CDate(recCierre.FECHA_RESP_CORRECTIVAS).ToString("yyyy/MM/dd")
            Else
                @<span>-</span>
            End If
        </dd>
        @If hayDatosCierre Then
            @If recCierre.FECHA_CIERRECLIENTE IsNot Nothing AndAlso recCierre.FECHA_CIERRECLIENTE > Date.MinValue Then
                @<dt>@Html.DisplayNameFor(Function(o) recCierre.FECHA_CIERRECLIENTE)</dt>
                @<dd>@CDate(recCierre.FECHA_CIERRECLIENTE).ToString("yyyy/MM/dd")</dd>
            End If
            @If recCierre.COSTE_REVISIONCLIENTE IsNot Nothing AndAlso recCierre.COSTE_REVISIONCLIENTE > 0 Then
                @<dt>@Html.DisplayNameFor(Function(o) recCierre.COSTE_REVISIONCLIENTE)</dt>
                @<dd>@recCierre.COSTE_REVISIONCLIENTE €</dd>
            End If
            @If recCierre.COSTE_CARGOSCLIENTE IsNot Nothing AndAlso recCierre.COSTE_CARGOSCLIENTE > 0 Then
                @<dt>@Html.DisplayNameFor(Function(o) recCierre.COSTE_CARGOSCLIENTE)</dt>
                @<dd>@recCierre.COSTE_CARGOSCLIENTE €</dd>
            End If
            @If recCierre.COSTE_REVISIONINTERNA IsNot Nothing AndAlso recCierre.COSTE_REVISIONINTERNA > 0 Then
                @<dt>@Html.DisplayNameFor(Function(o) recCierre.COSTE_REVISIONINTERNA)</dt>
                @<dd>@recCierre.COSTE_REVISIONINTERNA €</dd>
            End If
            @If recCierre.COSTE_MATERIALESCHATARRA IsNot Nothing AndAlso recCierre.COSTE_MATERIALESCHATARRA > 0 Then
                @<dt>@Html.DisplayNameFor(Function(o) recCierre.COSTE_MATERIALESCHATARRA)</dt>
                @<dd>@recCierre.COSTE_MATERIALESCHATARRA €</dd>
            End If
            @If recCierre.COSTE_OTROS IsNot Nothing AndAlso recCierre.COSTE_OTROS > 0 Then
                @<dt>@Html.DisplayNameFor(Function(o) recCierre.COSTE_OTROS)</dt>
                @<dd>@recCierre.COSTE_OTROS € 
                    @If recCierre.COSTE_OTROS_DESCRIPCION IsNot Nothing AndAlso Not recCierre.COSTE_OTROS_DESCRIPCION.Trim.Equals("") Then
                        @<span>( @recCierre.COSTE_OTROS_DESCRIPCION )</span>
                    End If
                </dd>
            End If
        End If
    </dl>
</div>
<div style="text-align:center">
    <div class="btn-group">
        <button class="btn btn-default" onclick="window.location.href = '@Url.Action("Edit", New With {.id = Model.ID})'" title="Editar" data-toggle="tooltip" @disabled>
            <span class="glyphicon glyphicon-pencil" style="color:royalblue" aria-hidden="true"></span>
        </button>
        <button class="btn btn-default" onclick="window.location.href = '@Url.Action("Close", New With {.id = Model.ID})'" title="Editar cierre" data-toggle="tooltip" @disabled>
            <span class="glyphicon glyphicon-@closureGlyph" style="color:green" aria-hidden="true"></span>
        </button>
        <button class="btn btn-default" onclick="if(confirm('Are you sure you want to delete this reclamation? It will be deleted also from GTK application')){location.href = '@Url.Action("Delete", New With {.id = Model.ID})'}" title="Borrar" data-toggle="tooltip" @disabled>
            <span class="glyphicon glyphicon-remove" style="color:red" aria-hidden="true"></span>
        </button>
        <button class="btn btn-default" onclick="window.location.href = '@Url.Action(TempData.Peek("ReturnUrl"))'" title="Volver al Listado" data-toggle="tooltip">
            <span class="glyphicon glyphicon-backward" aria-hidden="true"></span>
        </button>
    </div>
</div>
<br />
