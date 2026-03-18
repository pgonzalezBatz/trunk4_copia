Imports TelefoniaLib

Partial Public Class AltasBajas
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub SeleccionPlanta_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTitulo) : itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelFInicio)
            itzultzaileWeb.Itzuli(btnSincronizar) : itzultzaileWeb.Itzuli(btnFijarFecha) : itzultzaileWeb.Itzuli(chkMostrarBajasSinExt)
            itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(imgImprimir) : itzultzaileWeb.Itzuli(labelImprimir)
            itzultzaileWeb.Itzuli(labelInfo2) : itzultzaileWeb.Itzuli(imgImprimir) : itzultzaileWeb.Itzuli(labelImprimir)
        End If
    End Sub

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            inicializarPagina()
        End If
    End Sub

    ''' <summary>
    ''' Se inicializan los datos de la pagina
    ''' </summary>
    Private Sub inicializarPagina()
        rptUsuarios.DataSource = Nothing : rptUsuarios.DataBind()
        Dim fecha As DateTime = BLL.SincronizacionComponent.getFechaAltasBajas(Master.Ticket.IdPlantaActual) 'Se obtiene la ultima fecha
        lblFechaSincro.Text = If(fecha <> Date.MinValue, fecha.ToShortDateString, "Se sincronizaran todos")
        chkMostrarBajasSinExt.Checked = False
        pnlImprimir.Visible = False
    End Sub

#End Region

#Region "Repeater"

    ''' <summary>
    ''' Se enlazan la lista de usuarios con el repeater
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub rptUsuarios_ItemDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.RepeaterItemEventArgs) Handles rptUsuarios.ItemDataBound
        Try
            If (e.Item.ItemType = ListItemType.Header) Then
                itzultzaileWeb.TraducirWebControls(e.Item.Controls)
            ElseIf (e.Item.ItemType = ListItemType.Item Or e.Item.ItemType = ListItemType.AlternatingItem) Then
                Dim itemUser As String() = e.Item.DataItem
                Dim lblUsuario As Label = CType(e.Item.FindControl("lblUsuario"), Label)
                Dim lblInformacion As Label = CType(e.Item.FindControl("lblInformacion"), Label)
                Dim lblAccion As Label = CType(e.Item.FindControl("lblAccion"), Label)
                Dim pnlCambiar As Panel = CType(e.Item.FindControl("pnlCambiar"), Panel)
                Dim myTr As HtmlTableRow = CType(e.Item.FindControl("myTr"), HtmlTableRow)
                Dim btnCambiar As Button = CType(e.Item.FindControl("btnCambiar"), Button)
                Dim userComp As New Sablib.BLL.UsuariosComponent
                Dim oUserActual, oUserNuevo As Sablib.ELL.Usuario
                Dim extComp As New BLL.ExtensionComponent
                Dim bUsuarioNoEncontrado As Boolean = False
                oUserActual = Nothing : oUserNuevo = Nothing
                If (itemUser(0) <> String.Empty) Then
                    Dim vigentes As Boolean = False
                    oUserActual = New SabLib.ELL.Usuario With {.Id = CInt(itemUser(0))}
                    oUserActual = userComp.GetUsuario(oUserActual, vigentes)
                    If (oUserActual Is Nothing) Then bUsuarioNoEncontrado = True
                End If
                If (itemUser(1) <> String.Empty) Then
                    oUserNuevo = New Sablib.ELL.Usuario With {.Id = CInt(itemUser(1))}
                    oUserNuevo = userComp.GetUsuario(oUserNuevo)
                    If (oUserNuevo Is Nothing) Then bUsuarioNoEncontrado = True
                End If                
                pnlCambiar.Visible = False
                If (Not bUsuarioNoEncontrado) Then
                    Select Case CInt(itemUser(2))
                        Case BLL.SincronizacionComponent.Accion.CambioNumero
                            If (oUserActual.NombreCompleto <> String.Empty) Then
                                lblUsuario.Text = oUserActual.NombreCompleto
                            ElseIf (oUserActual.NombreUsuario <> String.Empty) Then
                                lblUsuario.Text = oUserActual.NombreUsuario
                            Else
                                lblUsuario.Text = oUserActual.Email
                            End If
                            lblInformacion.Text = itzultzaileWeb.Itzuli("Al realizar el cambio, se actualizaran lo enlace")
                            lblAccion.Text = "Cambio de nº de trabajador"
                            pnlCambiar.Visible = True
                            btnCambiar.CommandName = oUserActual.Id
                            btnCambiar.CommandArgument = oUserNuevo.Id
                            myTr.Style.Add("background-color", "#B5F2FD")
                        Case BLL.SincronizacionComponent.Accion.DadoBaja
                            If (oUserActual.NombreCompleto <> String.Empty) Then
                                lblUsuario.Text = oUserActual.NombreCompleto
                            ElseIf (oUserActual.NombreUsuario <> String.Empty) Then
                                lblUsuario.Text = oUserActual.NombreUsuario
                            Else
                                lblUsuario.Text = oUserActual.Email
                            End If
                            Dim lExtTlfno As List(Of ELL.TelefonoExtension) = extComp.getExtensionesPersona(oUserActual)
                            If (lExtTlfno Is Nothing) Then   'Buscar en Telefonos_personas por si se le ha asignado un telefono sin extension
                                Dim tlfnoComp As New BLL.TelefonoComponent
                                Dim lTlfnoExtAux As List(Of ELL.TelefonoExtension) = tlfnoComp.getTelefonosPersona(oUserActual)
                                If (lTlfnoExtAux IsNot Nothing AndAlso lTlfnoExtAux.Count > 0) Then
                                    lExtTlfno = New List(Of ELL.TelefonoExtension)
                                    lExtTlfno.AddRange(lTlfnoExtAux)
                                End If
                            End If
                            If (lExtTlfno IsNot Nothing AndAlso lExtTlfno.Count > 0) Then
                                Dim extInt, extMov As Integer
                                Dim directo, movil As String
                                extInt = 0 : extMov = 0
                                directo = String.Empty : movil = String.Empty
                                For Each oTlfnoExt As ELL.TelefonoExtension In lExtTlfno
                                    If (oTlfnoExt.ExtensionInterna <> Integer.MinValue) Then extInt = oTlfnoExt.ExtensionInterna
                                    If (oTlfnoExt.ExtensionMovil <> Integer.MinValue) Then extMov = oTlfnoExt.ExtensionMovil
                                    If (oTlfnoExt.TlfnoDirecto <> String.Empty) Then directo = oTlfnoExt.TlfnoDirecto
                                    If (oTlfnoExt.TlfnoMovil <> String.Empty) Then movil = oTlfnoExt.TlfnoMovil
                                Next
                                Dim mensa As String = String.Empty
                                If (extInt <> 0) Then mensa = "<b>" & itzultzaileWeb.Itzuli("Ext. interna") & " :</b> " & extInt & "<br />"
                                If (extMov <> 0) Then mensa &= "<b>" & itzultzaileWeb.Itzuli("Ext. movil") & " :</b> " & extMov & "<br />"
                                If (directo <> String.Empty) Then mensa &= "<b>" & itzultzaileWeb.Itzuli("directo") & " :</b> " & directo & "<br />"
                                If (movil <> String.Empty) Then mensa &= "<b>" & itzultzaileWeb.Itzuli("movil") & " :</b> " & movil
                                lblInformacion.Text = mensa
                            Else
                                lblInformacion.Text = itzultzaileWeb.Itzuli("Sin extensiones ni telefonos asociados")
                            End If
                            lblAccion.Text = "Dado de baja"
                            myTr.Style.Add("background-color", "#FF7171")
                        Case BLL.SincronizacionComponent.Accion.Nuevo
                            If (oUserNuevo.NombreCompleto <> String.Empty) Then
                                lblUsuario.Text = oUserNuevo.NombreCompleto
                            ElseIf (oUserNuevo.NombreUsuario <> String.Empty) Then
                                lblUsuario.Text = oUserNuevo.NombreUsuario
                            Else
                                lblUsuario.Text = oUserNuevo.Email
                            End If
                            lblAccion.Text = "Nuevo"
                            myTr.Style.Add("background-color", "#95FF95")
                    End Select
                End If
                itzultzaileWeb.Itzuli(btnCambiar) : itzultzaileWeb.Itzuli(lblAccion)
                If (bUsuarioNoEncontrado) Then myTr.Height = "0px"
            End If
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Cambia el idSab de un usuario que se ha dado de baja y despues se le ha vuelto a dar de alta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub CambiarIdSab(ByVal sender As Object, ByVal e As System.EventArgs)
        Try
            Dim btnCambio As Button = CType(sender, Button)
            Dim idSabActual As Integer = CInt(btnCambio.CommandName)
            Dim idSabNuevo As Integer = CInt(btnCambio.CommandArgument)
            If Not (idSabActual > 0 And idSabNuevo > 0) Then
                Master.MensajeError = "error"
            Else
                Dim sincrComp As New BLL.SincronizacionComponent
                If (sincrComp.actualizarIdSab(idSabActual, idSabNuevo)) Then
                    btnCambio.Enabled = False
                    Master.MensajeInfo = "Operacion realizada"
                    log.Info("Se ha cambiado el id de sab en la sincronizacion del usuario [" & idSabActual & " -> " & idSabNuevo & "]")
                Else
                    Master.MensajeError = "errGuardar"
                End If
            End If
        Catch ex As Exception
            Master.MensajeError = "errGuardar"
        End Try
    End Sub

#End Region

#Region "Botones"

    ''' <summary>
    ''' Se sincronizan todos los usuarios desde la fecha de ultima revision, comprobando que los usuarios de la aplicacion de telefonia no haya surgido ningun cambio con respecto a los de sab
    ''' Si hay usuarios nuevos, cambios de nº de trabajador o bajas, se avisara
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnSincronizar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSincronizar.Click
        Try
            log.Info("Se van a sincronizar las altas y bajas")
            Dim sincrComp As New BLL.SincronizacionComponent
            Dim idRecurso As Integer = CInt(ConfigurationManager.AppSettings.Item("RecursoTelefonia"))
            Dim lUsers As List(Of String()) = sincrComp.SincronizarAltasBajas(Master.Ticket.IdPlantaActual, idRecurso, chkMostrarBajasSinExt.Checked)
            'Se ordena por accion para tener los usuarios agrupados (Cambios, dados de baja, nuevos)
            If (lUsers IsNot Nothing) Then
                lUsers.Sort(Function(o1 As String(), o2 As String()) CInt(o1(2)) < CInt(o2(2)))
                pnlImprimir.Visible = (lUsers.Count > 0)
            End If
            rptUsuarios.DataSource = lUsers
            rptUsuarios.DataBind()
        Catch ex As Exception
            Master.MensajeError = "errSincronizarTerminos"
        End Try
    End Sub

    ''' <summary>
    ''' Fija la nueva fecha para que en la proxima sincronizacion, se sincronice a partir de esta
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnFijarFecha_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFijarFecha.Click
        Try
            If (BLL.SincronizacionComponent.setFechaAltasBajas(Master.Ticket.IdPlantaActual, Now.Date)) Then
                inicializarPagina()
                Master.MensajeInfo = "Fecha actualizada. A partir de ahora, las sincronizaciones se harán a partir de la fecha de hoy"
                log.Info("Se va fijado la fecha de sincronizaciones a dia de hoy")
            Else
                Master.MensajeError = "Error al actualizar"
            End If
        Catch ex As Exception
            Master.MensajeError = "errGuardar"
        End Try
    End Sub

#End Region

End Class
