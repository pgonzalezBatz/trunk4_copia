Public Class ProcesandoLiq
    Inherits UserControl

#Region "Propiedades"

    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal idLiq As Integer)
    Private itzultzaileWeb As New itzultzaile

    ''' <summary>
    ''' Id de la planta actual
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private Property IdPlanta As Integer
        Get
            Return CInt(Session("IdPlanta"))
        End Get
        Set(ByVal value As Integer)
            Session("IdPlanta") = value
        End Set

    End Property
#End Region

#Region "Carga del control"

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
    End Sub

    ''' <summary>
    ''' Se le indica lo que tiene que traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelEspere)
        End If
    End Sub

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>
    ''' <param name="lHGLiqSel">Lista de hojas de gastos a liquidar</param>
    ''' <param name="fEmision">Fecha de emision</param>
    Public Sub Iniciar(ByVal lHGLiqSel As List(Of ELL.HojaGastos.Liquidacion), ByVal fEmision As Date)
        ViewState("hojas") = lHGLiqSel
        ViewState("fechaEmision") = fEmision
        btnProcesarHidden.Text = itzultzaileWeb.Itzuli("Integrar [X] hojas de gastos").ToString.Replace("[X]", lHGLiqSel.Count)
    End Sub

    ''' <summary>
    ''' Al pinchar en el boton, se empieza el proceso de integracion de hojas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnProcesarHidden_Click(sender As Object, e As EventArgs) Handles btnProcesarHidden.Click
        Try
            PageBase.log.Info("LIQUIDACION: Empieza la integracion")
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim hojasImportes As New List(Of String())
            Dim lHGLiqSel As List(Of ELL.HojaGastos.Liquidacion) = CType(ViewState("hojas"), List(Of ELL.HojaGastos.Liquidacion))
            Dim fEmision As Date = CType(ViewState("fechaEmision"), Date)
            Dim hEnvios As New Hashtable
            Dim sIdHojasIntegrar As String = String.Empty
            Dim idHojaIntegrar, hojaIntegrar, idUsuario As String
            Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
            Dim hubClientContext = hubContext.Clients.Client(hfConnectionId.Value)
            Dim ticket As SabLib.ELL.Ticket = CType(Session("Ticket"), SabLib.ELL.Ticket)
            Dim myHoja As ELL.HojaGastos.Liquidacion.Hoja
            For Each liq As ELL.HojaGastos.Liquidacion In lHGLiqSel
                myHoja = liq.Hojas.First
                idHojaIntegrar = myHoja.IdHoja
                sIdHojasIntegrar &= If(sIdHojasIntegrar = String.Empty, "", ",") & idHojaIntegrar
                hojasImportes.Add(New String() {idHojaIntegrar, liq.ImporteTotalEuros})
                If (myHoja.IdViaje > 0) Then
                    hojaIntegrar = "V" & myHoja.IdViaje
                Else
                    hojaIntegrar = "H" & myHoja.IdHojaLibre
                End If
                idUsuario = liq.Usuario.Id
                If (hEnvios.ContainsKey(idUsuario)) Then
                    hEnvios(idUsuario) = hEnvios(idUsuario) & ", " & hojaIntegrar
                Else
                    hEnvios.Add(idUsuario, hojaIntegrar)
                End If
            Next
            Dim idCab As Integer = hojasBLL.Integrar(hojasImportes, fEmision, IdPlanta, ELL.HojaGastos.Liquidacion.TipoLiq.Metalico, hubClientContext)
            PageBase.log.Info("LIQUIDACION: El usuario financiero " & ticket.NombreCompleto & " (" & IdPlanta & ") ha integrado las hojas de gastos [" & sIdHojasIntegrar & "]")
            'Se avisa por email a todas las personas a las que se le va a liquidar alguna HG
            PageBase.log.Info("LIQUIDACION: Se va avisar por email a los usuarios con hojas de gastos seleccionados")
            Dim index As Integer = 1
            Dim ratioActualizacionProgress As Integer = (hEnvios.Count / 50) + 1  '50 es el numero de actualizaciones que tendra la progress bar, sea el numero de items que sea
            For Each entry As DictionaryEntry In hEnvios
                hubClientContext.showMessage(index & " de " & hEnvios.Count, 3)
                If (index Mod ratioActualizacionProgress = 0) Then hubClientContext.showProgress(index + 1, hEnvios.Count, 4) 'Para que la actualizacion de la progress bar sea mas fina
                AvisarPorEmail(entry.Key, entry.Value)
                index += 1
            Next
            RaiseEvent Finalizado(idCab)
        Catch batzEx As BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
        End Try
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
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            Try
                emailsAccesoDirecto = String.Empty : emailsAccesoPortal = String.Empty
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim oUser As New SabLib.ELL.Usuario With {.Id = idUser}
                oUser = userBLL.GetUsuario(oUser, False)
                If (oUser IsNot Nothing) Then
                    Dim sPerfil As String() = perfBLL.loadProfile(IdPlanta, oUser.Id, idRecurso)
                    If (sPerfil(1) = "0") Then 'Acceso por portal
                        If (emailsAccesoPortal <> String.Empty) Then emailsAccesoPortal &= ";"
                        emailsAccesoPortal = oUser.Email
                    Else 'Acceso directo
                        If (emailsAccesoDirecto <> String.Empty) Then emailsAccesoDirecto &= ";"
                        emailsAccesoDirecto = oUser.Email
                    End If
                End If
                If (emailsAccesoDirecto = String.Empty AndAlso emailsAccesoPortal = String.Empty) Then
                    PageBase.log.Warn("AVISO_LIQUIDACION: No se ha encontrado ningun email para avisar de la liquidacion del usuario (" & idUser & ")")
                Else
                    body = "Se ha iniciado el tramite de la liquidacion de las hojas de gastos [" & hojas & "] .<br />En breve recibirá el cobro por transferencia bancaria"
                    subject = "Liquidacion de hojas de gastos"
                    If (emailsAccesoPortal <> String.Empty) Then
                        bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, String.Empty, True)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoPortal, subject, bodyEmail, serverEmail)
                        PageBase.log.Info("AVISO_LIQUIDACION:Se ha enviado un email a " & oUser.NombreCompleto & " para avisarle de que se le han liquidado sus hojas (" & hojas & ") con acceso por el portal => " & emailsAccesoPortal)
                    End If
                    If (emailsAccesoDirecto <> String.Empty) Then
                        bodyEmail = PageBase.getBodyHmtl("Liquidacion", String.Empty, body, String.Empty, False)
                        SabLib.BLL.Utils.EnviarEmail(ConfigurationManager.AppSettings("emailFrom"), emailsAccesoDirecto, subject, bodyEmail, serverEmail)
                        PageBase.log.Info("Se ha enviado un email a " & oUser.NombreCompleto & " para avisarle de que se le han liquidado sus hojas (" & hojas & ") con acceso directo => " & emailsAccesoDirecto)
                    End If
                End If
            Catch ex As Exception
                PageBase.log.Error("AVISO_LIQUIDACION: No se ha podido avisar al usuario de que se le han liquidado unas hojas de gastos (" & idUser & ")", ex)
            End Try
        End If
    End Sub

    'Private Sub btnProcesarHidden_Click(sender As Object, e As EventArgs) Handles btnProcesarHidden.Click
    '    Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
    '    Dim hubClientContext = hubContext.Clients.Client(hfConnectionId.Value)
    '    Dim ratioActualizacionProgress As Integer = (30 / 50) + 1
    '    For index As Integer = 1 To 30
    '        Threading.Thread.Sleep(200)
    '        hubClientContext.showMessage(index & " de 30", 1)
    '        If (index Mod ratioActualizacionProgress = 0) Then hubClientContext.showProgress(index + 1, 30, 1)
    '    Next

    '    Threading.Thread.Sleep(1000)
    '    hubClientContext.showMessage("Generando...", 2)
    '    Threading.Thread.Sleep(4000)
    '    hubClientContext.showMessage("Generado", 2)

    '    Threading.Thread.Sleep(1000)
    '    hubClientContext.showMessage("Guardando...", 3)
    '    Threading.Thread.Sleep(3000)
    '    hubClientContext.showMessage("Guardado", 3)

    '    For index As Integer = 1 To 39
    '        Threading.Thread.Sleep(200)
    '        hubClientContext.showMessage(index & " de 30", 4)
    '        If (index Mod ratioActualizacionProgress = 0) Then hubClientContext.showProgress(index + 1, 30, 4)
    '    Next
    '    Threading.Thread.Sleep(1000)
    '    RaiseEvent Finalizado(141)
    'End Sub

#End Region

End Class