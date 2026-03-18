Public Class EnviarCorreo
    Inherits PageBase

#Region "Propiedades"
    ''' <summary>
    ''' Entidades de la base de datos.
    ''' </summary>
    ''' <remarks></remarks>
    Public BBDD As New BatzBBDD.Entities_Gertakariak
    Public Incidencia As New BatzBBDD.GERTAKARIAK

    ''' <summary>
    ''' Estructura donde se almacenamos las propiedades que queremos del GridView para realiza la seleccion, paginacion y ordenacion.
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Property Propiedades_gvSucesos() As gtkGridView
        Get
            If (Session("Propiedades_gvSucesos") Is Nothing) Then Session("Propiedades_gvSucesos") = New gtkGridView
            Return CType(Session("Propiedades_gvSucesos"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("Propiedades_gvSucesos") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub EnviarCorreo_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        '#If DEBUG Then
        '        If Not IsPostBack Then
        '            If Propiedades_gvSucesos.IdSeleccionado Is Nothing Then Propiedades_gvSucesos.IdSeleccionado = 23416
        '        End If
        '#End If
        Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = Propiedades_gvSucesos.IdSeleccionadoIstriku Select gtk).SingleOrDefault
    End Sub   
    Private Sub EnviarCorreo_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub btnCancelar_Click(sender As Object, e As ImageClickEventArgs) Handles btnCancelar.Click
		Response.Redirect("~/Informe/Detalle.aspx", False)
	End Sub
	Private Sub btnAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar.Click
		Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
		Dim lRespUsr As New List(Of SabLib.ELL.Usuario)
		Try
			If Incidencia Is Nothing Then
				Throw New ApplicationException(String.Format("No se ha encontrado la incidencia ({0})", If(Incidencia Is Nothing, String.Empty, Propiedades_gvSucesos.IdSeleccionadoIstriku)))
			Else
				'--------------------------------------------------------------------------------------------------
				'EnviarEmail()
				'Response.Redirect("~/Default.aspx", False)
				'--------------------------------------------------------------------------------------------------
				'Si el creardor del Incidente es un validador de los afectados, cargar la pagina de detalle. 
				'--------------------------------------------------------------------------------------------------
				Dim lSAB_USUARIOS = From Reg As BatzBBDD.DETECCION In Incidencia.DETECCION Select Reg.SAB_USUARIOS
				If lSAB_USUARIOS.Any Then
					For Each Reg As BatzBBDD.SAB_USUARIOS In lSAB_USUARIOS
						Dim IdMando As Integer = UsuariosComponent.GetResponsable(Reg.ID)
						If IdMando > Integer.MinValue AndAlso Not lRespUsr.Where(Function(o) o.Id = IdMando).Any Then
							Dim RespUsr As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = IdMando}, False)
							lRespUsr.Add(RespUsr)
						End If
					Next
				End If

				Dim lTO As List(Of Integer) = If(Request("hd_IdUsuarios") Is Nothing, New List(Of Integer), Request("hd_IdUsuarios").Split(",").ToList.ConvertAll(Function(o) Integer.Parse(o)))
				EnviarEmail(lTO, Incidencia.ID)

				If lRespUsr.Where(Function(Reg) Reg.Id = Ticket.IdUser).Any Then
					Response.Redirect("~/Informe/Detalle.aspx", False)
				Else
					Response.Redirect("~/Default.aspx", False)
				End If
				'--------------------------------------------------------------------------------------------------
			End If
		Catch ex As ApplicationException
			Master.ascx_Mensajes.MensajeError(ex)
		Catch ex As Exception
			Dim msg As String = vbCrLf & StrDup(90, "=") _
								& vbCrLf & String.Format("Incidencia.ID: {0}" & vbCrLf & "hd_IdUsuarios: {1}", Incidencia.ID, Request("hd_IdUsuarios")) _
								& vbCrLf & StrDup(90, "=")
			Log.Error(msg, ex)
			Master.ascx_Mensajes.MensajeError(ex)
		End Try
	End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim lRespUsr As New List(Of SabLib.ELL.Usuario)
        If Incidencia Is Nothing OrElse Incidencia.EntityKey Is Nothing Then
            Throw New ApplicationException(ItzultzaileWeb.Itzuli("No hay ningun suceso seleccionado") & "<br>" & ItzultzaileWeb.Itzuli("Regrese a la pagina de inicio"))
        Else
            '----------------------------------------------------------------------------------------------------------------
            'Perfiles de usuarios a los que se les envia el aviso por correo (Administradores y Usuarios de Gestion).
            '----------------------------------------------------------------------------------------------------------------
            Dim lPerfiles As IEnumerable(Of Integer) = From p As [Enum] In [Enum].GetValues(GetType(Perfil))
                                                       Where (p.GetHashCode = Perfil.Administrador OrElse p.GetHashCode = Perfil.Usuario OrElse p.GetHashCode = Perfil.Consultor OrElse p.GetHashCode = Perfil.AdministradorPlanta)
                                                       Select CInt(p.GetHashCode)
            'Dim lUsuarios As IQueryable(Of BatzBBDD.SAB_USUARIOS) = From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
            '                                                        Where lPerfiles.Contains(Reg.IDGRUPO) And Reg.IDUSUARIO <> Ticket.IdUser And Reg.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia
            '                                                        Select Reg.SAB_USUARIOS
            ''And Not ConfigurationManager.AppSettings("developers").Split(";").Contains(Reg.IDUSUARIO)

            'Dim myList = ConfigurationManager.AppSettings("developers").Split(";").ToList
            'Dim lUsuarios2 = From reg In lUsuarios
            '                 Where Not myList.Contains(reg.ID)
            'Dim lUsuarios As IQueryable(Of BatzBBDD.SAB_USUARIOS) = From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
            '                                                        Where lPerfiles.Contains(Reg.IDGRUPO) And Reg.IDUSUARIO <> Ticket.IdUser And Reg.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia _
            '                                                        And Not ConfigurationManager.AppSettings("developers").Contains(";" & Reg.IDUSUARIO & ";")
            '                                                        Select Reg.SAB_USUARIOS


            Dim lUsuarios As IQueryable(Of BatzBBDD.SAB_USUARIOS) = From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                                                    Where lPerfiles.Contains(Reg.IDGRUPO) And Reg.IDUSUARIO <> Ticket.IdUser And Reg.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia
                                                                    Select Reg.SAB_USUARIOS

            For Each dev In ConfigurationManager.AppSettings("developers").Split(";")
                lUsuarios = lUsuarios.Where(Function(o) o.ID <> dev)
            Next

            If lUsuarios IsNot Nothing AndAlso lUsuarios.ToList.Any Then
                For Each Reg As BatzBBDD.SAB_USUARIOS In lUsuarios
                    Dim RespUsr As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Reg.ID}, False)
                    lRespUsr.Add(RespUsr)
                Next

            End If
            '----------------------------------------------------------------------------------------------------------------
            '----------------------------------------------------------------------------------------------------------------
            'Mandos a los que se envia un aviso por correo.
            '----------------------------------------------------------------------------------------------------------------
            Dim lSAB_USUARIOS = From Reg As BatzBBDD.DETECCION In Incidencia.DETECCION Select Reg.SAB_USUARIOS
            If lSAB_USUARIOS.Any Then
                For Each Reg As BatzBBDD.SAB_USUARIOS In lSAB_USUARIOS
                    Dim IdMando As Integer = UsuariosComponent.GetResponsable(Reg.ID)
                    If IdMando > Integer.MinValue AndAlso Not lRespUsr.Where(Function(o) o.Id = IdMando).Any Then
                        Dim RespUsr As SabLib.ELL.Usuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = IdMando}, False)
                        lRespUsr.Add(RespUsr)
                    End If
                Next
            End If
            '----------------------------------------------------------------------------------------------------------------
            If lRespUsr.Any Then lvUsuarios.DataSource = lRespUsr
        End If
        lvUsuarios.DataBind()
    End Sub

    '    Private Sub EnviarEmail()
    '        Dim mail As New MailMessage()
    '        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
    '        Try
    '            Dim lIdUsuarios As List(Of String) = If(Request("hd_IdUsuarios") Is Nothing, New List(Of String), Request("hd_IdUsuarios").Split(",").ToList)

    '            If lIdUsuarios.Any Then
    '                Dim href As String = Request.Url.Scheme & Uri.SchemeDelimiter & Request.Url.Authority & Request.ApplicationPath & "/Login.aspx?idIncidencia=" & Incidencia.ID

    '                'Definir dereccion (FROM)
    '                'mail.From = New MailAddress("jrevuelta@Batz.es")
    '                mail.From = New MailAddress(UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}).Email)
    '                'Para (TO)
    '                For Each Id As String In lIdUsuarios
    '                    mail.To.Add(UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Id}).Email)
    '                Next
    '#If DEBUG Then
    '                mail.To.Clear()
    '                mail.To.Add("diglesias@batz.es")
    '#End If
    '                'Asunto (SUBJECT)
    '                mail.Subject = ItzultzaileWeb.Itzuli("AVISO de Accidente/Incidente")
    '                'Cuerpo del mensaje (BODY)
    '                mail.Body &= "<a href=""" & href & """ target=""_blank"" type=""text/html"">" & ItzultzaileWeb.Itzuli("Accidente/Incidente") & " " & ItzultzaileWeb.Itzuli("Nº:") & Incidencia.ID & "</a>"

    '                mail.IsBodyHtml = True
    '                mail.BodyEncoding = System.Text.Encoding.UTF8
    '                mail.SubjectEncoding = System.Text.Encoding.UTF8

    '                'Enviar el Mensaje
    '                Dim smtp As New SmtpClient("POSTA.BATZ.COM") 'Nombre del servidor de Exchange (IP => 172.17.200.3).
    '                smtp.Send(mail)
    '                smtp.Dispose()
    '            Else
    '                Throw New ApplicationException("Falta los email de destino.")
    '            End If
    '        Catch ex As ApplicationException
    '            Throw
    '        Catch ex As Exception
    '            Log.Error(ex)
    '            Throw
    '        End Try
    '    End Sub
#End Region
End Class