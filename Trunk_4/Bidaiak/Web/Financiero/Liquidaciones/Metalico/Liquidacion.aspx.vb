Imports BidaiakLib

''' <summary>
''' Esta pagina debería borrarse una vez funcione la nueva (ProcesarLiquidaciones.aspx)
''' </summary>
Public Class Liquidacion
    Inherits PageBase

#Region "Page Load"

    Private departmentInfo As List(Of String())

    ''' <summary>
    ''' Inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                hfStatePag.Value = "liqActual"
                chbHGEntregada.Checked = True
                mostrarInfo()
                TextoAyuda()
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se le indica lo que tiene que traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelProcesando)
            itzultzaileWeb.Itzuli(labelFEmision) : itzultzaileWeb.Itzuli(chbHGEntregada)
            itzultzaileWeb.Itzuli(btnIntegrar) : itzultzaileWeb.Itzuli(labelCompruebe)
            itzultzaileWeb.Itzuli(btnVolver) : itzultzaileWeb.Itzuli(btnIntegrar)
            itzultzaileWeb.Itzuli(btnFiltrar) : itzultzaileWeb.Itzuli(btnContinuar)
            itzultzaileWeb.Itzuli(labelFiltrar)
        End If
    End Sub

    ''' <summary>
    ''' Muestra la informacion de la pagina
    ''' </summary>    
    ''' <param name="lLineasSel">Lineas seleccionados</param>
    ''' <param name="idCab">Id de la cabecera a mostrar</param>
    ''' <param name="bBuscarRegistros">Indica si realizara la busqueda o no</param>
    Private Sub mostrarInfo(Optional ByVal lLineasSel As List(Of Integer) = Nothing, Optional ByVal idCab As Integer = 0, Optional ByVal bBuscarRegistros As Boolean = True)
        Dim hojasBLL As New BLL.HojasGastosBLL
        pnlMarca.Visible = False : pnlInfoIntegracion.Visible = False : pnlInfo.Visible = False
        btnContinuar.Visible = False : btnVolver.Visible = False : btnIntegrar.Visible = False
        pnlFiltro.Visible = False
        If (hfStatePag.Value = "liqActual") Then
            Dim lLiq As List(Of ELL.HojaGastos.Liquidacion.Cabecera) = hojasBLL.loadCabecerasLiquidacionesEmitidas(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Metalico)
            lblTextoLiquidacion.Text = itzultzaileWeb.Itzuli("Se muestra el listado de los importes a pagar. Se mostrarán todas las personas que tengan hojas de gastos validadas, que no hayan sido liquidadas y que su tipo de liquidacion sea en metalico")
            lnkTipoLiq.Visible = Not (lLiq Is Nothing OrElse (lLiq IsNot Nothing AndAlso lLiq.Count = 0))  'Esto se hace para que si no existe ninguna liquidacion ya emitida, no se visualizara el link
            lnkTipoLiq.Text = itzultzaileWeb.Itzuli("Ver liquidaciones emitidas")
            pnlBotones.Visible = True : pnlInfo.Visible = True : pnlFiltro.Visible = True
            txtFechaEmision.Text = Now.ToShortDateString
            If (Page.IsPostBack AndAlso bBuscarRegistros) Then
                BuscarLiquidaciones() 'La primera vez, no se consultas
            Else
                gvLiquidaciones.DataSource = Nothing
                gvLiquidaciones.DataBind()
            End If
        ElseIf (hfStatePag.Value = "liquidar") Then
            btnVolver.Visible = True : btnIntegrar.Visible = True : pnlInfoIntegracion.Visible = True
            Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasBLL.loadLiquidaciones(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Metalico, conHGEntregada:=chbHGEntregada.Checked)
            If (lLiquidaciones IsNot Nothing) Then Ordenar(lLiquidaciones)
            Dim lLiqNew As New List(Of ELL.HojaGastos.Liquidacion)
            For Each idLinea As Integer In lLineasSel
                lLiqNew.Add(lLiquidaciones.Find(Function(o As ELL.HojaGastos.Liquidacion) o.Hojas.First.IdHoja = idLinea))
            Next
            gvLiquidaciones.DataSource = lLiqNew
            gvLiquidaciones.DataBind()
            gvLiquidaciones.Columns(2).Visible = False
            btnIntegrar.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("Si continua, se integraran las hojas de gastos de los usuarios y ya no se visualizaran mas en este listado. Ademas se generara el fichero de texto para enviar al banco y se avisara por email a las personas para indicarles que en breve cobraran la liquidacion ¿Desea continuar?") & "');"
        End If
    End Sub

    ''' <summary>
    ''' Se redirige a ver las liquidaciones emitidas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub lnkTipoLiq_Click(ByVal sender As Object, ByVal e As EventArgs) Handles lnkTipoLiq.Click
        Response.Redirect("VerLiquidacionesMetalico.aspx", False)
    End Sub

    ''' <summary>
    ''' Se configura el texto de la ayuda
    ''' </summary>    
    Private Sub TextoAyuda()
        Dim texto As New Text.StringBuilder
        texto.AppendLine("- En esta pagina se mostraran el dinero a devolver a cada persona dependiendo de las hojas de gastos remitidas")
        texto.AppendLine("- Se podra seleccionar que registros liquidar")
        texto.AppendLine("- Se podran ver los ficheros generados en otras fechas anteriores")
        Master.TextoAyuda = texto.ToString
    End Sub

#End Region

#Region "Buscar liquidaciones"

    ''' <summary>
    ''' Busca las hojas de gastos que se encuentren entre las fechas señaladas, y calcula el importe a pagar o a ingresar
    ''' </summary>    
    Private Sub BuscarLiquidaciones()
        Try
            Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
            Dim hubClientContext = hubContext.Clients.Client(hfConnectionId.Value)
            Dim hojasGastosBLL As New BLL.HojasGastosBLL
            Dim fLimite As Date = If(txtFechaFiltro.Text <> String.Empty, CDate(txtFechaFiltro.Text), CDate(Now.ToShortDateString))
            Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = hojasGastosBLL.loadLiquidaciones(Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Metalico, Integer.MinValue, fLimite, chbHGEntregada.Checked, hubClientContext)
            If (lLiquidaciones IsNot Nothing) Then Ordenar(lLiquidaciones)
            'Se guardan las idHojas
            hfHojas.Value = String.Empty
            If (lLiquidaciones IsNot Nothing AndAlso lLiquidaciones.Count > 0) Then
                For Each liq As ELL.HojaGastos.Liquidacion In lLiquidaciones
                    For Each hojaLiq As ELL.HojaGastos.Liquidacion.Hoja In liq.Hojas
                        If (hfHojas.Value <> String.Empty) Then hfHojas.Value &= ","
                        hfHojas.Value &= hojaLiq.IdHoja
                    Next
                Next
            Else
                hfHojas.Value = String.Empty
            End If
            Session("Liquidaciones_Resul_State") = "0"
            Session("Liquidaciones_Resul_Gridview") = lLiquidaciones
        Catch batzEx As BidaiakLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BidaiakLib.BatzException("Error al buscar", ex)
        End Try
    End Sub

#End Region

#Region "Gridview"

    ''' <summary>
    ''' Enlaza los datos con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvLiquidaciones_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvLiquidaciones.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            Dim chbSelectAll As CheckBox = CType(e.Row.FindControl("chbSelectAll"), CheckBox)
            chbSelectAll.Attributes("onclick") = "ChangeAllCheckBoxStates(this.checked);"
            CheckBoxIDsArray.Text = chbSelectAll.ClientID  'Se guarda el id en esta variable para que luego en el footer, sepa cual es
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oLiq As ELL.HojaGastos.Liquidacion = e.Row.DataItem
            Dim lblId As Label = CType(e.Row.FindControl("lblId"), Label)
            Dim lblImportes As Label = CType(e.Row.FindControl("lblImportes"), Label)
            Dim lblPersona As Label = CType(e.Row.FindControl("lblPersona"), Label)
            Dim lblLiquidacion As Label = CType(e.Row.FindControl("lblLiquidacion"), Label)
            Dim lblIdUser As Label = CType(e.Row.FindControl("lblIdUser"), Label)
            Dim hlViajeHoja As HyperLink = CType(e.Row.FindControl("hlViajeHoja"), HyperLink)
            Dim lblFVal As Label = CType(e.Row.FindControl("lblFVal"), Label)
            Dim lblCuenta As Label = CType(e.Row.FindControl("lblCuenta"), Label)
            Dim chbMarcar As CheckBox = CType(e.Row.FindControl("chbMarcar"), CheckBox)
            Dim lblOrganizacion As Label = CType(e.Row.FindControl("lblOrganizacion"), Label)
            Dim imgExcluir As ImageButton = CType(e.Row.FindControl("imgExcluir"), ImageButton)
            Dim imgHGEntreg As Image = CType(e.Row.FindControl("imgHGEntreg"), Image)
            Dim lblLantegi As Label = CType(e.Row.FindControl("lblLantegi"), Label)
            Dim userBLL As New Sablib.BLL.UsuariosComponent
            Dim epsilonBLL As New BLL.Epsilon(Master.IdPlantaGestion)
            gvLiquidaciones.Columns(gvLiquidaciones.Columns.Count - 4).Visible = False 'No se muestra la organizacion
            gvLiquidaciones.Columns(gvLiquidaciones.Columns.Count - 3).Visible = True  'Se muestra el icono de HG entregado
            gvLiquidaciones.Columns(gvLiquidaciones.Columns.Count - 2).Visible = True  'Se muestra el icono para excluir
            gvLiquidaciones.Columns(gvLiquidaciones.Columns.Count - 1).Visible = False 'No se muestra el lantegi
            lblPersona.Text = oLiq.Usuario.NombreCompleto & " (" & oLiq.Usuario.CodPersona & ")"
            If (oLiq.Usuario.DadoBaja) Then lblPersona.Style.Add("color", "#FF0000")
            lblIdUser.Text = oLiq.Usuario.Id
            Dim myHoja As ELL.HojaGastos.Liquidacion.Hoja = oLiq.Hojas.First
            hlViajeHoja.NavigateUrl = "~/Viaje/HojaGastos.aspx?id=" & myHoja.IdHoja & "&orig=LIQ"
            If (myHoja.IdHojaLibre <> Integer.MinValue) Then
                hlViajeHoja.Text = "H" & myHoja.IdHojaLibre
            ElseIf (myHoja.IdViaje <> Integer.MinValue) Then
                hlViajeHoja.Text = "V" & myHoja.IdViaje
            End If
            lblId.Text = myHoja.IdHoja
            lblImportes.Text = myHoja.ImporteEuros
            lblLiquidacion.Text = myHoja.ImporteEuros & " €"
            lblFVal.Text = myHoja.FechaValidacion.ToShortDateString
            lblCuenta.Text = If(oLiq.CuentaContable > 0, oLiq.CuentaContable, String.Empty)
            'Se mira si es un subcontratado o es de Batz.1:Socio,2:Eventual,0:Subcontratado
            If (oLiq.Usuario.IdEmpresa <> 1) Then
                lblPersona.Text &= "(*)"
                pnlMarca.Visible = True
            End If
            imgHGEntreg.Visible = myHoja.Entregada
            imgExcluir.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("Si excluye esta hoja desparecera del listado de liquidaciones") & "');"
            imgExcluir.ToolTip = itzultzaileWeb.Itzuli("Excluir hoja")
            imgExcluir.CommandArgument = myHoja.IdHoja
        ElseIf (e.Row.RowType = DataControlRowType.Footer) Then
            Dim ArrayValues As New List(Of String)
            Dim lblTotal As Label = CType(e.Row.FindControl("lblTotal"), Label)
            Dim labelTotal As Label = CType(e.Row.FindControl("labelTotal"), Label)
            'Se añade el primero el de la cabecera
            ArrayValues.Add(String.Concat("'", CheckBoxIDsArray.Text, "'"))  'En la cabecera, se ha guardado el nombre del check de la cabecera
            Dim cont As Integer = 0
            Dim total As Decimal = 0
            For Each gvr As GridViewRow In gvLiquidaciones.Rows
                Dim cb As CheckBox = CType(gvr.FindControl("chbMarcar"), CheckBox)
                Dim lblImp As Label = CType(gvr.FindControl("lblImportes"), Label)
                'If the checkbox is unchecked, ensure that the Header CheckBox is unchecked
                cb.Attributes("onclick") = "ChangeHeaderAsNeeded();"
                'Add the CheckBox's ID to the client-side CheckBoxIDs array
                ArrayValues.Add(String.Concat("'", cb.ClientID, "'"))
                If (cb.Enabled) Then cont += 1
                total += PageBase.DecimalValue(lblImp.Text.Trim)
            Next
            If (labelTotal IsNot Nothing) Then itzultzaileWeb.Itzuli(labelTotal)
            lblTotal.Text = total & "€"
            btnIntegrar.Enabled = (cont > 0)
            CheckBoxIDsArray.Text = "<script type=""text/javascript"">" & vbCrLf & _
                    "<!--" & vbCrLf & _
                    String.Concat("var CheckBoxIDs = new Array(", String.Join(",", ArrayValues.ToArray()), ");") & vbCrLf & _
                    "// -->" & vbCrLf & _
                    "</script>"
        End If
    End Sub

    ''' <summary>
    ''' Ordenacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvLiquidaciones_Sorting(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewSortEventArgs) Handles gvLiquidaciones.Sorting
        Try
            gvLiquidaciones.Attributes("CurrentSortField") = e.SortExpression
            If (gvLiquidaciones.Attributes("CurrentSortDirection") Is Nothing) Then
                gvLiquidaciones.Attributes("CurrentSortDirection") = SortDirection.Ascending
            Else
                If (gvLiquidaciones.Attributes("CurrentSortDirection") = SortDirection.Ascending) Then
                    gvLiquidaciones.Attributes("CurrentSortDirection") = SortDirection.Descending
                Else
                    gvLiquidaciones.Attributes("CurrentSortDirection") = SortDirection.Ascending
                End If
            End If
            BuscarLiquidaciones()
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Ordena la lista de liquidaciones
    ''' </summary>
    ''' <param name="lLiquidaciones">Lista de liquidaciones</param>
    Private Sub Ordenar(ByRef lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion))
        Dim sortExp As String = "Persona"
        Dim sortDir As SortDirection = SortDirection.Ascending
        If (gvLiquidaciones.Attributes("CurrentSortField") IsNot Nothing) Then sortExp = gvLiquidaciones.Attributes("CurrentSortField").ToString
        If (gvLiquidaciones.Attributes("CurrentSortDirection") IsNot Nothing) Then sortDir = CType(gvLiquidaciones.Attributes("CurrentSortDirection"), SortDirection)
        Select Case sortExp
            Case "Persona"
                If (sortDir = SortDirection.Ascending) Then
                    lLiquidaciones = lLiquidaciones.OrderBy(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
                Else
                    lLiquidaciones = lLiquidaciones.OrderByDescending(Of String)(Function(o) o.Usuario.NombreCompleto).ToList
                End If
            Case "Liquidacion"
                If (sortDir = SortDirection.Ascending) Then
                    lLiquidaciones = lLiquidaciones.OrderBy(Of Decimal)(Function(o) o.ImporteTotalEuros).ToList
                Else
                    lLiquidaciones = lLiquidaciones.OrderByDescending(Of Decimal)(Function(o) o.ImporteTotalEuros).ToList
                End If
        End Select
    End Sub

    ''' <summary>
    ''' Paginacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvLiquidaciones_PageIndexChanging(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewPageEventArgs) Handles gvLiquidaciones.PageIndexChanging
        Try
            gvLiquidaciones.PageIndex = e.NewPageIndex
            BuscarLiquidaciones()
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se excluye la hoja para que no aparezca
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub imgExcluir_Click(sender As Object, e As ImageClickEventArgs)
        Try
            Dim idHoja As Integer = CInt(CType(sender, ImageButton).CommandArgument)
            Dim hojBLL As New BLL.HojasGastosBLL
            hojBLL.ExcluirHG(idHoja)
            log.Info("Se ha excluido la hoja de gastos " & idHoja & " de la liquidacion")
            BuscarLiquidaciones()
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

#End Region

#Region "Integrar y generar fichero"

    ''' <summary>
    ''' Los marca como integrados
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnIntegrarHidden_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIntegrarHidden.Click
        Try
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim hojasImportes As New List(Of String())
            Dim hEnvios As New Hashtable
            Dim sIdHojasIntegrar As String = String.Empty
            Dim idHojaIntegrar, hojaIntegrar, idUsuario As String
            For Each row As GridViewRow In gvLiquidaciones.Rows
                idHojaIntegrar = CType(row.Cells(0).Controls(0), Label).Text
                sIdHojasIntegrar &= If(sIdHojasIntegrar = String.Empty, "", ",") & idHojaIntegrar
                hojasImportes.Add(New String() {idHojaIntegrar, CType(row.Cells(1).Controls(0), Label).Text})
                hojaIntegrar = CType(row.Cells(5).Controls(0), HyperLink).Text
                idUsuario = CInt(CType(row.Cells(6).Controls(0), Label).Text)
                If (hEnvios.ContainsKey(idUsuario)) Then
                    hEnvios(idUsuario) = hEnvios(idUsuario) & ", " & hojaIntegrar
                Else
                    hEnvios.Add(idUsuario, hojaIntegrar)
                End If
            Next
            If (hojasImportes.Count = 0) Then
                Session("LiquidacionesInt_Resul_State") = 1
                Session("LiquidacionesInt_Resul_Message") = itzultzaileWeb.Itzuli("Debe seleccionar algun elemento para integrar")
            Else
                Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
                Dim hubClientContext = hubContext.Clients.Client(hfConnectionId.Value)
                Dim idCab As Integer = hojasBLL.Integrar(hojasImportes, CDate(txtFechaEmision.Text), Master.IdPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Metalico, hubClientContext)
                PageBase.log.Info("LIQUIDACION: El usuario financiero " & Master.Ticket.NombreCompleto & " (" & Master.Ticket.IdUser & ") ha integrado las hojas de gastos [" & sIdHojasIntegrar & "]")
                Master.MensajeInfo = itzultzaileWeb.Itzuli("Se han integrados las hojas de gastos")
                'Se avisa por email a todas las personas a las que se le va a liquidar alguna HG
                log.Info("LIQUIDACION: Se va avisar por email a los usuarios con hojas de gastos seleccionados")
                Dim index As Integer = 1
                For Each entry As DictionaryEntry In hEnvios
                    hubClientContext.showMessage_Integracion("Paso 3/3 : Enviando email " & index & " de " & hEnvios.Count)
                    AvisarPorEmail(entry.Key, entry.Value)
                    index += 1
                Next
                Session("LiquidacionesInt_Resul_State") = 0
                Session("LiquidacionesInt_Resul_Message") = idCab
            End If
        Catch batzEx As BatzException
            Session("LiquidacionesInt_Resul_State") = 2
            Session("LiquidacionesInt_Resul_Message") = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se pinta el resultado de la integracion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnIntegrar_Click(sender As Object, e As EventArgs) Handles btnIntegrar.Click
        Dim state As Integer = CType(Session("LiquidacionesInt_Resul_State"), Integer)
        Dim message As String = CType(Session("LiquidacionesInt_Resul_Message"), String)
        Session("LiquidacionesInt_Resul_State") = Nothing : Session("LiquidacionesInt_Resul_Message") = Nothing
        Select Case state
            Case 0
                Try  'Se muestra el historico con el registro subido                
                    hfStatePag.Value = "historico"
                    mostrarInfo(Nothing, CInt(message)) 'idCab
                Catch ex As Exception
                    Master.MensajeError = itzultzaileWeb.Itzuli("Se han integrado las hojas pero ha dado un error al mostrar el resultado de la ejecucion")
                End Try
            Case 1 : Master.MensajeAdvertencia = message
            Case 2 : Master.MensajeError = message
        End Select
    End Sub

    ''' <summary>
    ''' Previsualiza el ultimo paso antes de integrar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnContinuar_Click(sender As Object, e As EventArgs) Handles btnContinuar.Click
        Try
            Dim hojasGastosBLL As New BLL.HojasGastosBLL
            Dim numSel As Integer = 0
            Dim lLiquidaciones As New List(Of ELL.HojaGastos.Liquidacion)
            Dim lLineas As New List(Of Integer)
            For Each row As GridViewRow In gvLiquidaciones.Rows
                If (CType(row.Cells(2).Controls(0), CheckBox).Checked) Then
                    numSel += 1
                    lLineas.Add(CInt(CType(row.Cells(0).Controls(0), Label).Text))
                End If
            Next
            If (numSel > 0) Then
                hfStatePag.Value = "liquidar"
                mostrarInfo(lLineas)
            Else
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione alguna linea")
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al mostrar las hojas de gasto a liquidar")
        End Try
    End Sub

    ''' <summary>
    ''' Se vuelve a la primera vista para seleccionar las liquidaciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Try
            hfStatePag.Value = "liqActual"
            mostrarInfo()
        Catch batzEx As BatzException
            Master.MensajeAdvertencia = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra de nuevo el listado de liquidaciones con el nuevo filtro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnFiltrarHidden_Click(sender As Object, e As EventArgs) Handles btnFiltrarHidden.Click
        Try
            BuscarLiquidaciones()
        Catch batzEx As BatzException
            Session("Liquidaciones_Resul_State") = "2"
            Session("Liquidaciones_Resul_Message") = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se pinta el gridview de las liquidaciones procesadas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnFiltrar_Click(sender As Object, e As EventArgs) Handles btnFiltrar.Click
        Dim state As Integer = CType(Session("Liquidaciones_Resul_State"), Integer)
        Dim message As String = CType(Session("Liquidaciones_Resul_Message"), String)
        Dim lLiquidaciones As List(Of ELL.HojaGastos.Liquidacion) = CType(Session("Liquidaciones_Resul_Gridview"), List(Of ELL.HojaGastos.Liquidacion))
        Session("Liquidaciones_Resul_State") = Nothing : Session("Liquidaciones_Resul_Message") = Nothing : Session("Liquidaciones_Resul_Gridview") = Nothing
        Select Case state
            Case 0
                gvLiquidaciones.Columns(2).Visible = True
                gvLiquidaciones.DataSource = lLiquidaciones
                gvLiquidaciones.DataBind()
                btnContinuar.Visible = (lLiquidaciones IsNot Nothing AndAlso lLiquidaciones.Count > 0)
            Case 2 : Master.MensajeError = message
        End Select
    End Sub

    ''' <summary>
    ''' Avisa a las personas que se les va a pagar la liquidacion
    ''' </summary>
    ''' <param name="idUser">Id del usuario a avisar</param>
    ''' <param name="hojas">Cadena con las hojas que se le van a liquidar</param>
    Private Sub AvisarPorEmail(ByVal idUser As Integer, ByVal hojas As String)
        If (ConfigurationManager.AppSettings("avisarPorEmail") = "1") Then
            Dim perfBLL As New BLL.BidaiakBLL
            Dim emailsAccesoDirecto, emailsAccesoPortal, subject, body, bodyEmail As String
            Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings("RecursoWeb_Admon"))
            Dim paramBLL As New Sablib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                Dim userBLL As New Sablib.BLL.UsuariosComponent
                Dim oUser As New Sablib.ELL.Usuario With {.Id = idUser}
                oUser = userBLL.GetUsuario(oUser, False)
                If (oUser IsNot Nothing) Then
                    Dim sPerfil As String() = perfBLL.loadProfile(Master.IdPlantaGestion, oUser.Id, idRecurso)
                    If (sPerfil(1) = "0") Then 'Acceso por portal
                        If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                        emailsAccesoPortal = oUser.Email
                    Else 'Acceso directo
                        If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                        emailsAccesoDirecto = oUser.Email
                    End If
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    log.Warn("AVISO_LIQUIDACION: No se ha encontrado ningun email para avisar de la liquidacion del usuario (" & idUser & ")")
                Else
                    body = "Se ha iniciado el tramite de la liquidacion de las hojas de gastos [" & hojas & "] .<br />En breve recibirá el cobro por transferencia bancaria"                    
                    subject = "Liquidacion de hojas de gastos"
                    If (emailsAccesoPortal <> String.Empty) Then
                        bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, String.Empty, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        log.Info("AVISO_LIQUIDACION:Se ha enviado un email a " & oUser.NombreCompleto & " para avisarle de que se le han liquidado sus hojas (" & hojas & ") con acceso por el portal => " & emailsAccesoPortal)
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then                                                
                        bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, String.Empty, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        log.Info("Se ha enviado un email a " & oUser.NombreCompleto & " para avisarle de que se le han liquidado sus hojas (" & hojas & ") con acceso directo => " & emailsAccesoDirecto)
                    End If
                End If
            Catch ex As Exception
                log.Error("AVISO_LIQUIDACION: No se ha podido avisar al usuario de que se le han liquidado unas hojas de gastos (" & idUser & ")", ex)
            End Try
        End If
    End Sub

#End Region

#Region "Temporizador"

    ''' <summary>
    ''' Se inicializa el temporizador con 3 minutos antes para que no caduque
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub temporizador_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles temporizador.Init
        temporizador.Interval = ((Session.Timeout - 2) * 60) * 1000
    End Sub

    ''' <summary>
    ''' Tick del timer
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub temporizador_Tick(sender As Object, e As EventArgs) Handles temporizador.Tick
        log.Info("Autorrefresco de la liquidacion para que no caduque la pagina")
    End Sub

#End Region

End Class