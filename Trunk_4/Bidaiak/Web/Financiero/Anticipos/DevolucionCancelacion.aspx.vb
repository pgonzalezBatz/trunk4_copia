Imports BidaiakLib

Public Class DevolucionCancelacion
    Inherits PageBase

#Region "Page load"

    ''' <summary>
    ''' Se carga la info del anticipo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                imgDevolver.CommandArgument = Request.QueryString("id")                
                cargarAnticipo()
                TextoAyuda()
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("errCargarPagina")
        End Try
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Devolucion_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(imgVolver) : itzultzaileWeb.Itzuli(labelViaje) : itzultzaileWeb.Itzuli(labelInfo)
            itzultzaileWeb.Itzuli(labelEurSol) : itzultzaileWeb.Itzuli(labelUsuario) : itzultzaileWeb.Itzuli(imgDevolver)
            itzultzaileWeb.Itzuli(labelEurEntreg) : itzultzaileWeb.Itzuli(labelInformacion)
        End If
    End Sub

    ''' <summary>
    ''' Se configura el texto de la ayuda
    ''' </summary>    
    Private Sub TextoAyuda()
        Dim texto As New Text.StringBuilder
        texto.AppendLine("- Pagina para registrar las devoluciones de anticipos entregados cuyo viaje ha sido cancelado.")        
        Master.TextoAyuda = texto.ToString
    End Sub

#End Region

#Region "Carga Informacion"

    ''' <summary>
    ''' Carga la informacion del anticipo
    ''' </summary>	
    Private Sub cargarAnticipo()               
        Dim viajesBLL As New BLL.ViajesBLL
        Dim anticBLL As New BLL.AnticiposBLL
        Dim oViaje As ELL.Viaje = viajesBLL.loadInfo(CInt(imgDevolver.CommandArgument))
        lblViaje.Text = oViaje.IdViaje & " - " & oViaje.Destino & "(" & oViaje.FechaIda.ToShortDateString & " - " & oViaje.FechaVuelta.ToShortDateString & ")"
        cargarUsuariosViaje()       
        Dim oAntic As ELL.Anticipo = anticBLL.loadInfo(oViaje.IdViaje, False)
        selImportes.Importes = oAntic.AnticiposSolicitados
        selImportes.Inicializar()
        lblEurosSolicitados.Text = Math.Round(oAntic.EurosSolicitados, 2)
        Dim eurosEntreg As Decimal = oAntic.EurosEntregados
        pnlEurosEntregados.Visible = (eurosEntreg > 0)
        If (eurosEntreg > 0) Then lblEurosEntregados.Text = eurosEntreg
    End Sub

    ''' <summary>
    ''' Carga los usuarios del viaje del parametro
    ''' </summary>	
    Private Sub cargarUsuariosViaje()
        If (ddlUsuarios.Items.Count = 0) Then
            Try
                ddlUsuarios.Items.Clear()
                ddlUsuarios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
                Dim viajeBLL As New BLL.ViajesBLL
                Dim integrantes As List(Of ELL.Viaje.Integrante) = viajeBLL.loadIntegrantes(CInt(imgDevolver.CommandArgument))
                For Each item As ELL.Viaje.Integrante In integrantes
                    ddlUsuarios.Items.Add(New ListItem(item.Usuario.NombreCompleto, item.Usuario.Id))
                Next
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al cargar los integrantes del viaje", ex)
            End Try
        End If
        ddlUsuarios.SelectedIndex = 0
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Se marca como devuelto el anticipo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgDevolver_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgDevolver.Click
        Try
            If (CInt(ddlUsuarios.SelectedValue) > 0) Then
                Dim anticBLL As New BLL.AnticiposBLL
                anticBLL.DevolverAnticipo(CInt(imgDevolver.CommandArgument), ddlUsuarios.SelectedValue, Master.IdPlantaGestion)
                WriteLog("DEV_CANCEL: Se ha devuelto el anticipo del viaje cancelado " & imgDevolver.CommandArgument, TipoLog.Info)
                Volver()
            Else
                Master.MensajeAdvertencia = "Seleccione un usuario"
            End If
        Catch batzEx As BidaiakLib.BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se vuelve al listado de anticipos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgVolver_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgVolver.Click
        Volver()
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>
    Private Sub Volver()
        Response.Redirect("GestionAnticipos.aspx", False)
    End Sub

#End Region

End Class