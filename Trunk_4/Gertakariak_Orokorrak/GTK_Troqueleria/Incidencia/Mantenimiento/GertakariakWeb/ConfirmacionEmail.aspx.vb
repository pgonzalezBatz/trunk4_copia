Imports GertakariakLib
Imports System.Net.Mail
Imports TraduccionesLib

Public Class ConfirmacionEmail
    Inherits PageBase

#Region "Propiedades"
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Incidencia As New BatzBBDD.GERTAKARIAK
    Dim IdIncidencia As String

    Property gvGertakariak_Propiedades() As gtkGridView
        Get
            If (Session("gvGertakariak_Propiedades") Is Nothing) Then Session("gvGertakariak_Propiedades") = New gtkGridView
            Return CType(Session("gvGertakariak_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvGertakariak_Propiedades") = value
        End Set
    End Property
    Dim lTO As New List(Of String)

    Dim _gtkTicket As GertakariakLib.ELL.gtkTicket
    Protected ReadOnly Property gtkTicket
        Get
            If Ticket Is Nothing Then
                _gtkTicket = Nothing
            ElseIf _gtkTicket Is Nothing Then
                _gtkTicket = New GertakariakLib.ELL.gtkTicket
                _gtkTicket.Culture = Ticket.Culture
                _gtkTicket.IdDepartamento = Ticket.IdDepartamento
                _gtkTicket.IdEmpresa = Ticket.IdEmpresa
                _gtkTicket.IdSession = Ticket.IdSession
                _gtkTicket.NombrePersona = Ticket.NombrePersona
                _gtkTicket.Apellido1 = Ticket.Apellido1
                _gtkTicket.Apellido2 = Ticket.Apellido2
                _gtkTicket.IdTrabajador = Ticket.IdTrabajador
                _gtkTicket.IdUser = Ticket.IdUser
                _gtkTicket.NombreUsuario = Ticket.NombreUsuario

                Dim usercomp As New GertakariakLib.BLL.gtkUsuarioComponent
                Dim oUser As New GertakariakLib.ELL.gtkUsuario
                oUser.IdUsrSab = _gtkTicket.IdUser
                _gtkTicket.UsuarioRoles = usercomp.ConsultarListado(oUser)
            End If

            Return _gtkTicket
        End Get
    End Property

#End Region
#Region "Eventos Pagina"
    Private Sub ConfirmacionEmail_PreInit(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreInit
        IdIncidencia = If(String.IsNullOrWhiteSpace(Request.QueryString("idInc")), gvGertakariak_Propiedades.IdSeleccionado, Request.QueryString("idInc"))
    End Sub
    Private Sub Page_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        Try
            CargarDatos()
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Acciones"
    Private Sub btnAceptar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnAceptar.Click
        '		Dim gtkGertakariak As New GertakariakLib.ELL.gtkTroqueleria
        '		Dim fGTK As New GertakariakLib.BLL.GertakariakComponent
        '		Dim ListaGTK As New List(Of Object)
        '		Try
        '			gtkGertakariak.Id = IdIncidencia
        '			ListaGTK = fGTK.Consultar(gtkGertakariak)
        '			If ListaGTK IsNot Nothing AndAlso ListaGTK.Count = 1 Then EnviarEmail(ListaGTK(0))
        '			Volver()
        '		Catch ex As ApplicationException
        '			Master.MensajeAdvertencia = ex.Message
        '		Catch ex As Exception
        '			Global_asax.log.Error(ex)
        '			Master.MensajeError = ex.Message
        '		End Try
        Try
            If Incidencia Is Nothing Then
                Throw New ApplicationException(String.Format("No se ha encontrado la incidencia ({0})", If(IdIncidencia Is Nothing, String.Empty, IdIncidencia)))
            Else

                EnviarEmail()
                Volver()
            End If
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnCancelar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnCancelar.Click
        Volver()
    End Sub
#End Region
#Region "Funciones y Procesos"
    Sub Volver()
        Dim arg As String
        arg = "?idInc=" & IdIncidencia
        'Response.Redirect("~/Default.aspx" & arg, False)
        Response.Redirect("~/Incidencia/Detalle.aspx" & arg, True)
    End Sub
    ''' <summary>
    ''' Proceso de aviso por email a los responsables de la lista.
    ''' 
    ''' </summary>
    ''' <param name="gtkGertakaria">ELL.gtkTroqueleria</param>
    ''' <remarks></remarks>
    Private Sub EnviarEmail(ByVal gtkGertakaria As ELL.gtkTroqueleria)
        Dim mail As New MailMessage()
        'Dim gtkTicket As GertakariakLib.ELL.gtkTicket = Session("gtkTicket")
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim dbAvisosEmail As New DAL.AVISOSEMAIL
        Dim gtkProveedor As New ELL.gtkProveedor
        Dim ProveedorComponent As New BLL.gtkProveedorComponent

        Try
            Dim IPLocal As Boolean = (New List(Of String) From {"::1", "192", "172", "10"}).Contains(Request.ServerVariables("REMOTE_ADDR").Split(".")(0))
            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = gtkTicket.IdUser})
            'Definir dereccion
            mail.From = New MailAddress("Gertakariak@Batz.es")

            If gtkUsuario.Email IsNot String.Empty And gtkUsuario.Email IsNot Nothing Then mail.CC.Add(gtkUsuario.Email)
            '---------------------------------------------------
            'Recogemos los email para el aviso
            '---------------------------------------------------
            dbAvisosEmail.Where.IDTRAMITACION.Value = 3
            dbAvisosEmail.Query.Load()
            If dbAvisosEmail.EOF Then Throw New ApplicationException("No hay usuarios en la lista de envio".Itzuli)
            If ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                For Each Filas As Data.DataRowView In dbAvisosEmail.DefaultView
                    gtkUsuario = New SabLib.ELL.Usuario With {.Id = Filas.Item(GertakariakLib.DAL.AVISOSEMAIL.ColumnNames.IDUSRSAB)}
                    gtkUsuario = UsuariosComponent.GetUsuario(gtkUsuario)
                    If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                        mail.To.Add(gtkUsuario.Email.ToString.Trim)
                    End If
                Next
                '---------------------------------------------------
                'mail.Bcc.Add("diglesias@batz.es")
            Else
                mail.To.Add("diglesias@batz.es")
            End If

            mail.Subject = ItzultzaileWeb.Itzuli("AVISO de No Conformidad de Proveedor")
            If gtkGertakaria.FechaCierre <> Nothing AndAlso gtkGertakaria.FechaCierre <> Date.MinValue Then
                mail.Body = "CIERRE de No Conformidad de Proveedor".Itzuli & ":"
            Else
                mail.Body = "AVISO de No Conformidad de Proveedor".Itzuli & ":"
            End If
            'mail.Body = mail.Body & "<A href=""" & If(IPLocal = True, "http://usotegieta2.batz.es", "https://Kuboak.batz.com") & "/ibmcognos/cgi-bin/cognosisapi.dll?b_action=cognosViewer&ui.action=run&ui.object=%2fcontent%2ffolder%5b%40name%3d%27BATZ%27%5d%2ffolder%5b%40name%3d%27Trokelgintza%20-%20Troqueleria%27%5d%2ffolder%5b%40name%3d%27Gertakariak%20-%20Incidencias%27%5d%2ffolder%5b%40name%3d%27Hobekuntza%27%5d%2freport%5b%40name%3d%27RS%20Informe%20de%20Disconformidad%27%5d&ui.name=RS%20Informe%20de%20Disconformidad&p_dincidencia=" & gtkGertakaria.Id & "&p_Ver=NO&p_Idioma=" & Ticket.Culture & "&run.prompt=true&run.outputFormat=PDF&run.outputPageOrientation=portrait"" Target=""_BLANK"">" & " (" & ItzultzaileWeb.Itzuli("NoConformidad") & " - " & gtkGertakaria.Id & ")" & "</A>"
            Dim linkCognos = "https://cognos.batz.es/ibmcognos/bi/?objRef=i8C91D739AF2A43328B8ED940DC0F7481&ui.action=run&format=PDF&prompt=false&p_dincidencia=" & gtkGertakaria.Id & "&ui_appbar=false&ui_navbar=false"
            mail.Body = mail.Body & "<a href=""" & linkCognos & """ Target=""_BLANK"">" & " (" & ItzultzaileWeb.Itzuli("NoConformidad") & " - " & gtkGertakaria.Id & ")" & "</a>"

            gtkProveedor = ProveedorComponent.Consultar(gtkGertakaria.IdProveedor, Nothing, False).Item(0)
            mail.Body = mail.Body & "<BR>" & ItzultzaileWeb.Itzuli("Proveedor") & ": " & gtkProveedor.NomProv & " (" & gtkProveedor.CIF & ")"

            '--------------------------------------------------------------
            'Indicamos a que OF/OP pertenece la NC
            '--------------------------------------------------------------
            mail.Body = mail.Body & "<BR>"
            mail.Body = mail.Body & "<p style=""text-decoration:underline; font-weight: bold; font-style: normal; text-transform: uppercase; font-family: Verdana, Geneva, Tahoma, sans-serif;"">"
            mail.Body = mail.Body & ItzultzaileWeb.Itzuli("OF/OP") & " (" & ItzultzaileWeb.Itzuli("Marca") & "):"
            mail.Body = mail.Body & "</p>"
            If gtkGertakaria.OF_Operacion IsNot Nothing AndAlso gtkGertakaria.OF_Operacion.Count > 0 Then
                mail.Body = mail.Body & "<ul>"
                For Each OFM As GertakariakLib.ELL.gtkOFM In gtkGertakaria.OF_Operacion
                    mail.Body = mail.Body & "<li><p>"
                    mail.Body = mail.Body & OFM.OFOP & " (" & OFM.Marca & ")"
                    mail.Body = mail.Body & "</p></li>"
                Next
                mail.Body = mail.Body & "</ul>"
            End If
            '--------------------------------------------------------------

            mail.IsBodyHtml = True
            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            'Enviar el Mensaje
            Dim smtp As New SmtpClient(serverEmail) 'Nombre del servidor de Exchange (IP => 172.17.200.3).
            smtp.Send(mail)
            smtp.Dispose()
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw
        End Try
    End Sub
    Private Sub EnviarEmail()
        'Dim gtkTicket As GertakariakLib.ELL.gtkTicket = Session("gtkTicket")
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Try
            If lTO.Any Then
                Dim mail As New MailMessage()
                Dim gtkUsuario As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = gtkTicket.IdUser})

                'Definir dereccion (FROM)
                mail.From = New MailAddress("Gertakariak@Batz.es")
                'Copia (CC)
                gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = gtkTicket.IdUser})
                If gtkUsuario.Email IsNot String.Empty And gtkUsuario.Email IsNot Nothing Then mail.CC.Add(gtkUsuario.Email)
                'Para (TO)
                lTO.ForEach(Sub(o) mail.To.Add(o))

                'Asunto (SUBJECT)
                mail.Subject = ItzultzaileWeb.Itzuli("AVISO de No Conformidad de Proveedor")

                'Cuerpo del mensaje (BODY)
                mail.Body = If(Incidencia.FECHACIERRE IsNot Nothing AndAlso Incidencia.FECHACIERRE <> Date.MinValue _
                               , "CIERRE de No Conformidad de Proveedor".Itzuli & ":" _
                               , "AVISO de No Conformidad de Proveedor".Itzuli & ":")

                Dim IPLocal As Boolean = (New List(Of String) From {"::1", "192", "172", "10"}).Contains(Request.ServerVariables("REMOTE_ADDR").Split(".")(0))
                'mail.Body = mail.Body & "<A href=""" & If(IPLocal = True, "http://usotegieta2.batz.es", "https://Kuboak.batz.com") & "/ibmcognos/cgi-bin/cognosisapi.dll?b_action=cognosViewer&ui.action=run&ui.object=%2fcontent%2ffolder%5b%40name%3d%27BATZ%27%5d%2ffolder%5b%40name%3d%27Trokelgintza%20-%20Troqueleria%27%5d%2ffolder%5b%40name%3d%27Gertakariak%20-%20Incidencias%27%5d%2ffolder%5b%40name%3d%27Hobekuntza%27%5d%2freport%5b%40name%3d%27RS%20Informe%20de%20Disconformidad%27%5d&ui.name=RS%20Informe%20de%20Disconformidad&p_dincidencia=" & Incidencia.ID & "&p_Ver=SI&p_Idioma=" & Ticket.Culture & "&run.prompt=false&run.outputFormat=PDF&run.outputPageOrientation=portrait"" Target=""_BLANK"">" & " (" & ItzultzaileWeb.Itzuli("NoConformidad") & " - " & Incidencia.ID & ")" _
                '    & "<img src='" & Request.Url.Scheme & "://intranet2.batz.es/GertakariakTroqueleria/App_Themes/Batz/Imagenes/TipoArchivos/pdf-icon24.png' style='vertical-align:middle'/></A>"

                Dim linkCognos = "https://cognos.batz.es/ibmcognos/bi/?objRef=i8C91D739AF2A43328B8ED940DC0F7481&ui.action=run&format=PDF&prompt=false&p_dincidencia=" & Incidencia.ID & "&ui_appbar=false&ui_navbar=false"
                mail.Body = mail.Body & "<a href=""" & linkCognos & """ Target=""_BLANK"">" & " (" & ItzultzaileWeb.Itzuli("NoConformidad") & " - " & Incidencia.ID & ")" & "<img src='" & Request.Url.Scheme & "://intranet2.batz.es/GTK_Troqueleria/App_Themes/Batz/Imagenes/TipoArchivos/pdf-icon24.png' style='vertical-align:middle'/></a>"

                Dim Proveedor As BatzBBDD.GCPROVEE = (From Reg In BBDD.GCPROVEE Where Reg.CODPRO.Trim = Incidencia.IDPROVEEDOR Select Reg).SingleOrDefault
                If Proveedor IsNot Nothing Then mail.Body = mail.Body & "<BR>" & ItzultzaileWeb.Itzuli("Proveedor") & ": " & Proveedor.NOMPROV & " (" & Proveedor.CIF & ")"

                '--------------------------------------------------------------
                'Indicamos a que OF/OP pertenece la NC
                '--------------------------------------------------------------
                mail.Body = mail.Body & "<BR>"
                mail.Body = mail.Body & "<p style=""text-decoration:underline; font-weight: bold; font-style: normal; text-transform: uppercase; font-family: Verdana, Geneva, Tahoma, sans-serif;"">"
                mail.Body = mail.Body & ItzultzaileWeb.Itzuli("OF/OP") & " (" & ItzultzaileWeb.Itzuli("Marca") & "):"
                mail.Body = mail.Body & "</p>"
                If Incidencia.OFMARCA.Any Then
                    mail.Body = mail.Body & "<ul>"
                    For Each OFM In Incidencia.OFMARCA
                        mail.Body = mail.Body & "<li><p>"
                        mail.Body = mail.Body & OFM.NUMOF & "-" & OFM.OP & " (" & OFM.MARCA & ")"
                        mail.Body = mail.Body & "</p></li>"
                    Next
                    mail.Body = mail.Body & "</ul>"
                End If
                '--------------------------------------------------------------

                mail.IsBodyHtml = True
                mail.BodyEncoding = System.Text.Encoding.UTF8
                mail.SubjectEncoding = System.Text.Encoding.UTF8

                If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                    mail.To.Clear()
                    mail.To.Add("diglesias@batz.es")
                End If

                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                'Enviar el Mensaje
                Dim smtp As New SmtpClient(serverEmail) 'Nombre del servidor de Exchange (IP => 172.17.200.3).
                smtp.Send(mail)
                smtp.Dispose()
            Else
                Throw New ApplicationException("Falta los email de destino.")
            End If
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw
        End Try
    End Sub
    Sub CargarDatos()
        'Dim PTK As New MatrixPTK.EnoviaSoapClient()
        Dim PTK As New MatrixPTK.Enovia
        Try
            Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
            If Incidencia Is Nothing Then Throw New ApplicationException("No se pueden enviar los correos. Sesion caducada. Vuelva a la pantalla de inicio.")

            '-------------------------------------------------------------------------------------------------------
            'Lista de avisos (No Conformidad - Proveedores)
            '-------------------------------------------------------------------------------------------------------
            Dim lSAB_USUARIOS As IEnumerable(Of BatzBBDD.SAB_USUARIOS) =
                From Reg As BatzBBDD.AVISOSEMAIL In BBDD.AVISOSEMAIL Where Reg.IDTRAMITACION = 3 Select Reg.SAB_USUARIOS
            If lSAB_USUARIOS.Any Then
                bl_chkNCProveedor.DataSource = From Reg In lSAB_USUARIOS Select NombreCompleto = Reg.NOMBRE & " " & Reg.APELLIDO1 & " " & Reg.APELLIDO2 Order By NombreCompleto
                bl_chkNCProveedor.DataBind()
                lTO = lTO.Select(Function(o) o.ToUpper.Trim).ToList
                If chkNCProveedor.Checked Then lSAB_USUARIOS.ToList.ForEach(Sub(o) If Not lTO.Contains(o.EMAIL.ToUpper.Trim) Then lTO.Add(o.EMAIL))
            Else
                bl_chkNCProveedor.Items.Add(New ListItem("Sin Usuarios"))
            End If
            '-------------------------------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------------------------------
            'Gestor de Proyecto
            '-------------------------------------------------------------------------------------------------------
            Dim FechaActual As Nullable(Of Date) = Now.Date
            Dim lOF = From Reg In Incidencia.OFMARCA Select Reg.NUMOF Distinct
            If lOF.Any Then
                Dim lGestor As New List(Of String)
                For Each Reg In lOF
                    Dim GestorOF As String = PTK.emailgestor(Reg)
                    If Not String.IsNullOrWhiteSpace(GestorOF) Then
                        If Not lGestor.Contains(GestorOF) Then
                            lGestor.Add(GestorOF.ToUpper.Trim)
                        End If
                    End If
                Next

                'lGestor = lGestor.Select(Function(o) o.ToUpper.Trim).ToList
                Dim lUsr = From Reg As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS
                           Where (Reg.FECHABAJA Is Nothing Or Reg.FECHABAJA >= FechaActual) And Reg.FECHABAJA Is Nothing And lGestor.Contains(Reg.EMAIL.ToUpper.Trim) Select Reg

                If lUsr.Any Then
                    bl_chkGestor.DataSource = From Reg In lUsr.AsEnumerable Select NombreCompleto = Reg.NOMBRE & " " & Reg.APELLIDO1 & " " & Reg.APELLIDO2 Order By NombreCompleto
                    bl_chkGestor.DataBind()
                    lTO = lTO.Select(Function(o) o.ToUpper.Trim).ToList
                    If chkGestor.Checked Then lUsr.ToList.ForEach(Sub(o) If Not lTO.Contains(o.EMAIL.ToUpper.Trim) Then lTO.Add(o.EMAIL))
                Else
                    bl_chkGestor.Items.Add(New ListItem("Sin Usuarios"))
                End If
            End If
            '-------------------------------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------------------------------
            'Pedido por Retroceso
            '-------------------------------------------------------------------------------------------------------
            '?
            '-------------------------------------------------------------------------------------------------------
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            'Global_asax.log.Error(ex)
            Throw ex
        End Try
    End Sub
#End Region

End Class