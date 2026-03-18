Public Class ChequeoEjecucionVisas
    Inherits System.Web.UI.UserControl

#Region "Eventos"

    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado()
    Public Event GoToStep(ByVal paso As Integer)
    Public Event GoToImportsList()
    Private itzultzaileWeb As New Itzultzaile

#End Region

#Region "Page load"

    ''' <summary>
    ''' inicializa los controles
    ''' </summary>
    Private Sub inicializar()
        pnlDistintoUsuario.Visible = False : pnlMismoUsuario.Visible = False
        lblMensa1.Text = String.Empty : lblMensa2.Text = String.Empty
        btnSi.CommandArgument = String.Empty
    End Sub

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelTit1) : itzultzaileWeb.Itzuli(labelTit2)
            itzultzaileWeb.Itzuli(btnSi) : itzultzaileWeb.Itzuli(btnNo)
            itzultzaileWeb.Itzuli(btnCancelar)
        End If
    End Sub

#End Region

#Region "Iniciar"

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>    
    ''' <param name="_idUser">Id del usuario</param>
    ''' <returns>Devuelve true si hay que quedarse en el control y false si hay que salir</returns>
    Public Function Iniciar(ByVal _idUser As Integer) As Boolean
        Try
            inicializar()
            Dim idPlanta As Integer = CInt(Session("IdPlanta"))
            Dim bidaiakBLL As New BidaiakLib.BLL.BidaiakBLL
            Dim oEjec As BidaiakLib.BLL.BidaiakBLL.Ejecucion = bidaiakBLL.loadEjecucion(BidaiakLib.BLL.BidaiakBLL.TipoEjecucion.Visas, idPlanta)
            If (oEjec Is Nothing) Then  'No hay ninguna ejecucion incompleta
                PageBase.log.Info("IMPORT_VISA: No existe ningun ejecucion incompleta")
                RaiseEvent GoToStep(1)
                Return False
            Else
                Dim mensa As String = String.Empty
                Dim fecha As Date = oEjec.Fecha
                If (oEjec.IdUser = _idUser) Then  'El usuario tiene iniciada una ejecucion
                    pnlMismoUsuario.Visible = True
                    mensa = itzultzaileWeb.Itzuli("Tiene iniciada una ejecucion sin completar") & "<br />"
                    mensa &= itzultzaileWeb.Itzuli("Paso") & ": " & oEjec.Paso & "<br />"
                    mensa &= itzultzaileWeb.Itzuli("fecha") & ": " & fecha.ToShortDateString & " - " & fecha.ToShortTimeString & "<br /><br />"
                    mensa &= itzultzaileWeb.Itzuli("¿Desea continuar? Si responde que si, seguira a partir del paso en el que se quedo. Si por el contrario responde que no, se borraran todos los datos temporales y tendra que empezar de nuevo")
                    lblMensa2.Text = mensa
                    btnSi.CommandArgument = oEjec.Paso
                    PageBase.log.Warn("IMPORT_VISA: El usuario " & oEjec.IdUser & " tiene iniciada una ejecucion en el paso " & oEjec.Paso & " en " & fecha.ToShortDateString & "-" & fecha.ToShortTimeString & ".¿Desea continuar?")
                    Return True
                Else 'Existe otro usuario propietario de la ejecucion 
                    Dim userBLL As New Sablib.BLL.UsuariosComponent
                    pnlDistintoUsuario.Visible = True
                    mensa = itzultzaileWeb.Itzuli("No puede iniciar la ejecucion porque el usuario XXX se encuentra en el paso YYY")
                    mensa = mensa.Replace("XXX", userBLL.GetUsuario(New Sablib.ELL.Usuario With {.Id = oEjec.IdUser}).NombreCompleto)
                    mensa = mensa.Replace("YYY", oEjec.Paso)
                    mensa &= "<br />"
                    mensa &= itzultzaileWeb.Itzuli("Fecha de ejecucion") & ":" & fecha.ToShortDateString & " - " & fecha.ToShortTimeString
                    lblMensa1.Text = mensa
                    PageBase.log.Warn("IMPORT_VISA: Se ha interrumpido la ejecucion debido a que el usuario " & oEjec.IdUser & " tiene iniciada una ejecucion en el paso " & oEjec.Paso & " en " & fecha.ToShortDateString & "-" & fecha.ToShortTimeString)
                    Return True
                End If
            End If
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("Error al chequear si existen ejecuciones sin terminar"))
        Catch ex As Exception
            Dim sms As String = "Error al chequear si existen ejecuciones sin terminar"
            PageBase.log.Error(sms, ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli(sms))
        End Try
    End Function

#End Region

#Region "Botones"

    ''' <summary>
    ''' Continua en el paso especificado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnSi_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnSi.Click
        PageBase.log.Info("IMPORT_VISA: Continua en el paso " & btnSi.CommandArgument)
        RaiseEvent GoToStep(CInt(btnSi.CommandArgument))
    End Sub

    ''' <summary>
    ''' Hay que borrar las tablas para empezar de nuevo. con redirigir al paso 1 ya vale
    ''' Se borra el registro de la ejecucion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnNo_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnNo.Click
        PageBase.log.Info("IMPORT_VISA: No continua asi que se borran los temporales y se empieza de nuevo")
        Try
            Dim bidaiakBLL As New BidaiakLib.BLL.BidaiakBLL
            bidaiakBLL.deleteEjecucion(BidaiakLib.BLL.BidaiakBLL.TipoEjecucion.Visas, CInt(Session("IdPlanta")))  'IdPlanta
            PageBase.log.Info("IMPORT_VISA: Informacion de ejecucion borrada")
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("Al pulsar No, no se ha podido borrar el registro de ejecucion de la planta"))
        End Try
        RaiseEvent GoToImportsList()
    End Sub

    ''' <summary>
    ''' se sale del wizard
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        PageBase.log.Info("IMPORT_VISA: Se cancela la ejecucion")
        Response.Redirect("~/Default.aspx", False)
    End Sub

#End Region

End Class