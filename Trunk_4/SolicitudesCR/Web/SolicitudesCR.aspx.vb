Imports log4net
Imports System.Text
Imports System.Security.Cryptography

Partial Public Class SolicitudesCR
    Inherits PageBase

    Private Log As log4net.ILog = log4net.LogManager.GetLogger("root.SolicitudesCR")
    Private itzultzaileWeb As New LocalizationLib.Itzultzaile
    Dim idtra As Integer
    ''' <summary>
    ''' Obtiene el ticket
    ''' </summary>
    ''' <returns></returns>
    Private Property Ticket() As SabLib.ELL.Ticket
        Get
            If (Session("Ticket") IsNot Nothing) Then
                Return CType(Session("Ticket"), SabLib.ELL.Ticket)
            Else
                Return Nothing
            End If
        End Get
        Set(ByVal value As SabLib.ELL.Ticket)
            Session("Ticket") = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el Id de Izaro
    ''' </summary>
    ''' <returns></returns>
    Private ReadOnly Property IdIzaro As Integer
        Get
            If (Session("IdIzaro") IsNot Nothing) Then
                Return CInt(Session("IdIzaro"))
            Else
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim _idIzaro As Integer = plantBLL.GetPlanta(CInt(Session("PlantaSel"))).IdIzaro
                Session("IdIzaro") = _idIzaro
                Return _idIzaro
            End If
        End Get
    End Property



    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then   ' itzultzaileWeb.Itzuli(Label3) :
            itzultzaileWeb.Itzuli(btnDocumento) : itzultzaileWeb.Itzuli(DdlAsunto) : itzultzaileWeb.Itzuli(lblNombre)
            itzultzaileWeb.Itzuli(lblAsunto) : itzultzaileWeb.Itzuli(lblDescC) : itzultzaileWeb.Itzuli(lblComment)
            itzultzaileWeb.Itzuli(txtComentario) : itzultzaileWeb.Itzuli(btnCancelar) : itzultzaileWeb.Itzuli(btnGuardarNuevaSolicitud)
            'itzultzaileWeb.Itzuli(Label2)
        End If
    End Sub

    ''' <summary>
    ''' Se pinta la pagina con los iconos del usuario
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Dim recBLL As New SabLib.BLL.RecursosComponent

        Dim oDocumentosBLL As New CEticoLib.BLL.cEtico
        Dim listaType2 As List(Of CEticoLib.ELL.Responsables)

        If Not IsPostBack Then
            'voy a usarlo para el trabajador inicial
            PageBase.plantaAdmin = Ticket.IdTrabajador
            PageBase.rolUsuario = Ticket.IdUser
            If (Request.QueryString("id") IsNot Nothing And Session("F5") Is Nothing) Then 'Si viene un idSession en la url, hay que cargar el ticket
                Dim loginComp As New SabLib.BLL.LoginComponent
                Ticket = loginComp.getTicket(Request.QueryString("id"))
                Session("Ticket") = Ticket
                If (Session("PlantaSel") Is Nothing) Then Session("PlantaSel") = Ticket.IdPlanta
            End If
        End If


        If hfEmpresa.Value <> "" Then
            listaType2 = oDocumentosBLL.CargarTrabajador(CInt(hfEmpresa.Value))
            NomEmp.Text = listaType2(0).Nombre & "*"

            idtra = listaType2(0).Planta 'es el codpersona
        Else
            NomEmp.Text = Ticket.NombreCompleto
            idtra = Ticket.IdTrabajador
        End If

        Session("F5") = "menu"
        If Session("Ticket") Is Nothing Then
            Log.Warn("Ticket vacio, se vuelve a la pagina de login [" & User.Identity.Name.ToLower & "]")
            Response.Redirect("default.aspx")
        End If
        Dim myTicket As SabLib.ELL.Ticket = Ticket
        If myTicket IsNot Nothing Then
            Dim qRecursos As List(Of RRHHLib.ELL.recursoPE)
            Dim claseMenu As New RRHHLib.BLL.RRHHComponent
            Dim MachineUserName As String = String.Empty
            Dim nombreUsuario As String() = User.Identity.Name.ToLower.Split("\")
            If (nombreUsuario IsNot Nothing AndAlso nombreUsuario.Length = 2) Then

                Dim lRecursos As List(Of String()) = recBLL.GetListadoLimitacionRecUser(nombreUsuario(1))
                If (lRecursos.Count > 0) Then MachineUserName = nombreUsuario(1)
            End If
            Dim selPlanta As String = Session("PlantaSel")
            'Recursos
            '------------------------------------------           
            Dim numControl As Integer = 0
            Dim idRecursoPadre As Integer = CInt(ConfigurationManager.AppSettings("RecursoSolicitudesCR"))
            qRecursos = claseMenu.dameRecursosHijo(idRecursoPadre, myTicket.IdUser, Globalization.CultureInfo.CurrentCulture.Name, MachineUserName)
            If Not qRecursos Is Nothing AndAlso qRecursos.Count > 0 Then 'comprobamos que al menos tenga algún recurso
                qRecursos = qRecursos.FindAll(Function(o As RRHHLib.ELL.recursoPE) o.Visible)
                'rptRec.DataSource = qRecursos
                'rptRec.DataBind()
            Else
                'pnlRecursos.Controls.Add(New Label With {.Text = itzultzaileWeb.Itzuli("Sin recursos"), .CssClass = "negrita"})
            End If


            'sacar los datos




        End If


        'Dim oDocBLL As New CEticoLib.BLL.cEtico
        'Dim lista As List(Of CEticoLib.ELL.Rol)
        'lista = oDocBLL.CargarRolCualquierPlanta(myTicket.IdUser)
        'If lista.Count > 0 Then
        '    PageBase.plantaAdmin = lista(0).rol
        '    PageBase.rolUsuario = lista(0).Id


        '    Label2.Visible = True
        'Else
        '    Label2.Visible = False
        'End If



        If System.Configuration.ConfigurationManager.AppSettings("presidencia").ToString.Contains(PageBase.rolUsuario.ToString) Or System.Configuration.ConfigurationManager.AppSettings("RRHH").ToString.Contains(PageBase.rolUsuario.ToString) Then
            '  Label2.Visible = True
            NomEmp.ReadOnly = False
        Else
            NomEmp.ReadOnly = True
        End If




    End Sub



    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        'grabar el registro. User, fecha, categoria, comentario,
        Dim textoEncriptado As String
        Dim textoEncriptado2 As String
        Dim prefijo As String = ""
        If fuDoc.HasFile Then
            Dim fmt2 As String = "00"
            Dim fmt4 As String = "0000"


            Dim fechatext As String = Now.Year.ToString(fmt4) & Now.Month.ToString(fmt2) & Now.Day.ToString(fmt2) & Now.Hour.ToString(fmt2) & Now.Minute.ToString(fmt2) & Now.Second.ToString(fmt2)
            prefijo = "" & fechatext & Right(fuDoc.FileName, 5)
            fuDoc.SaveAs(System.Configuration.ConfigurationManager.AppSettings("PathFicherosSubir").ToString() & prefijo)
        Else
            textoEncriptado = "guardar2"
        End If


        Dim oDocumentosBLL As New CEticoLib.BLL.cEtico



        '    Dim textoDesEncriptado As String
        textoEncriptado = (txtComentario.Text)
        textoEncriptado2 = (DdlAsunto.SelectedValue)
        'Dim planta As Integer = DirectCast(Session("Ticket"), SabLib.ELL.Ticket).IdPlanta
        'Dim plantades As String = ""
        'Dim plantBLL As New SabLib.BLL.PlantasComponent
        Dim dominio As String = User.Identity.Name.ToLower.Split("\")(0)
        'Dim lPlantasIzaro As List(Of SabLib.ELL.Planta) = plantBLL.GetPlantas(True, Nothing, Nothing)


        Dim Solicitud As New CEticoLib.ELL.CEtico With {.plantaDesc = NomEmp.Text, .planta = PageBase.plantaAdmin, .Idtra = IdTra.ToString, .Fecha = Now, .codCategoria = prefijo, .comentario = textoEncriptado}

        Dim prueba As String
        prueba = DirectCast(Session("Ticket"), SabLib.ELL.Ticket).Culture
        DirectCast(Session("Ticket"), SabLib.ELL.Ticket).Culture = "eu-ES"




        Dim mailTo As String = ""

        'Dim lista As List(Of CEticoLib.ELL.Rol)
        'lista = oDocumentosBLL.CargarRolMail(planta)
        'For i = 0 To lista.Count - 1
        '    mailTo = mailTo & oDocumentosBLL.CargarEmailTo(lista(i).Id)(0).email & ";"
        'Next
        'If mailTo.Length > 0 Then
        mailTo = Ticket.email


        If (oDocumentosBLL.GuardarSolicitud(Solicitud)) Then
            Master.MensajeInfo = itzultzaileWeb.Itzuli("La solicitud se ha guardado correctamente")


            'mandar mail a jgalan@batz.es
            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()

            'principio de mensajes
            'tengo que buscar en sab la cultura, cambiarla para envia mail y volver a poner la que tenia
            '     CurrentCulture.Name = "eu-ES"
            'buscar en sab idculturas segun email de usuarios
            Dim tmpCultura As String = ""
            Dim mailcultura As String = ""
            Dim mailcultura2 As String = ""
            mailcultura = System.Configuration.ConfigurationManager.AppSettings("emailpresidencia".ToString())
            mailcultura2 = System.Configuration.ConfigurationManager.AppSettings("emailRRHH".ToString())

            Dim listaType2 As List(Of CEticoLib.ELL.CEtico)
            listaType2 = oDocumentosBLL.CargarCultura(mailcultura)
            If listaType2.Count > 0 Then
                tmpCultura = listaType2(0).campo1
            End If

            Dim cultureInfotmp As Globalization.CultureInfo
            Dim cultureInfo2 As Globalization.CultureInfo

            cultureInfotmp = Threading.Thread.CurrentThread.CurrentCulture

            If tmpCultura <> "" Then
                cultureInfo2 = Globalization.CultureInfo.CreateSpecificCulture(tmpCultura)
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo2
            End If
            SabLib.BLL.Utils.EnviarEmail("SolicitudesCR@batz.es", System.Configuration.ConfigurationManager.AppSettings("emailpresidencia").ToString(), itzultzaileWeb.Itzuli("Nueva entrada en Solicitudes a Consejo Rector") & ". ", itzultzaileWeb.Itzuli("Se ha recibido una nueva entrada en Solicitudes a Consejo Rector") & ".<br><br>", oParams.ServidorEmail)

            listaType2 = oDocumentosBLL.CargarCultura(mailcultura2)
            If listaType2.Count > 0 Then
                tmpCultura = listaType2(0).campo1
            End If
            If tmpCultura <> "" Then
                cultureInfo2 = Globalization.CultureInfo.CreateSpecificCulture(tmpCultura)
                Threading.Thread.CurrentThread.CurrentCulture = cultureInfo2
            End If
            SabLib.BLL.Utils.EnviarEmail("SolicitudesCR@batz.es", System.Configuration.ConfigurationManager.AppSettings("emailRRHH").ToString(), itzultzaileWeb.Itzuli("Nueva entrada en Solicitudes a Consejo Rector") & ". ", itzultzaileWeb.Itzuli("Se ha recibido una nueva entrada en Solicitudes a Consejo Rector") & ".<br><br>", oParams.ServidorEmail)

            Threading.Thread.CurrentThread.CurrentCulture = cultureInfotmp
            'fin de mensajes


        Else
            Master.MensajeError = itzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba la solicitud")

        End If


        LimpiarCampos()
        '  Initialize()

    End Sub






    Private Sub btnSubir2_Click(sender As Object, e As EventArgs) Handles btnSubir2.Click
        Try
            If fuDoc.HasFile Then

            Else
                '           lblPlantillaSubida.Text = "Fichero no seleccionado."
            End If


        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba") & " " & fuDoc.FileName
            '     Throw New SabLib.BatzException("Error al mostrar un documento", ex)
        End Try
    End Sub



    'Public Function Desencriptar(ByVal Input As String) As String


    '    Return Input

    'End Function

    'Public Function Encriptar(ByVal Input As String) As String



    '    Return Input

    'End Function





    Private Sub LimpiarCampos()
        DdlAsunto.SelectedValue = ""
        txtComentario.Text = String.Empty


    End Sub
    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click
        DdlAsunto.ClearSelection()
        txtComentario.Text = ""
    End Sub


    Protected Sub Documento_Click(sender As Object, e As EventArgs) Handles btnDocumento.Click
        Dim idDocumento As String
        Try

            Dim cultura As String
            cultura = System.Globalization.CultureInfo.CurrentCulture.TwoLetterISOLanguageName

            '      If Not Session("miCultura") Is Nothing Then Page.Culture = Session("miCultura")



            Dim docnombre As String = ""
            Select Case cultura
                Case "es"
                    idDocumento = System.Configuration.ConfigurationManager.AppSettings("urlDocEs").ToString

                Case "eu"
                    idDocumento = System.Configuration.ConfigurationManager.AppSettings("urlDocEu").ToString

                Case Else
                    idDocumento = System.Configuration.ConfigurationManager.AppSettings("urlDocEn").ToString
            End Select



            Dim script As String = "<script type=""text/javascript"">window.open('" & idDocumento & "');</script>"
            ClientScript.RegisterStartupScript(Me.GetType, "openWindow", script)

        Catch ex As Exception
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al ver el documento")
        End Try
    End Sub


End Class