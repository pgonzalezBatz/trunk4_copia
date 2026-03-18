Imports System.Web.Mvc
Imports TarjetasVisitaLib

Namespace Controllers
    Public Class SolicitudController
        Inherits BaseController

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function MisSolicitudes() As ActionResult
            ViewData("ContainerFluid") = True
            Dim permiso As ELL.Permiso = BLL.PermisoBLL.ConsultarUltimo(Ticket.IdUser)
            ViewData("Permiso") = permiso
            ViewData("Solicitudes") = Nothing

            If (permiso IsNot Nothing AndAlso permiso.FechaSolicitud <> DateTime.MinValue AndAlso permiso.Autorizado) Then
                ' El usuario está autorizado. Consultamos sus solicitudes de tarjetas
                ViewData("Solicitudes") = BLL.SolicitudesBLL.CargarListadoPorIdSab(Ticket.IdUser)
                'CargarComboIdiomas()
            End If

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Historico() As ActionResult
            ViewData("ContainerFluid") = True
            ViewData("Solicitudes") = Nothing

            ViewData("Solicitudes") = BLL.SolicitudesBLL.CargarListado().OrderByDescending(Function(f) f.FechaAlta).ToList()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="radioDatos"></param>
        ''' <returns></returns>
        Function Agregar(Optional ByVal radioDatos As Integer = Integer.MinValue) As ActionResult
            ViewData("ContainerFluid") = True
            If (radioDatos <> Integer.MinValue) Then
                ViewData("DatosAlternativos") = BLL.DatosAlternativosBLL.Obtener(radioDatos)
            Else
                ViewData("DatosAlternativos") = New ELL.DatosAlternativos()
            End If

            Dim puesto As ELL.Puesto = BLL.PKSBLL.ObtenerPuesto(Ticket.IdUser)
            'Forzamos a ingles. Según Enara la idea es que esté en los 2 idiomas
            puesto.Nombre = Utils.Traducir(puesto.Nombre, "en-GB")
            ViewData("Puesto") = puesto

            ViewData("Telefonos") = BLL.TelefoniaBLL.CargarListado(Ticket.IdUser).FirstOrDefault(Function(f) f.IdSabPlanta = Ticket.IdPlanta)
            ViewData("IdNegocio") = ObtenerDepartamento().IdNegocio
            CargarComboIdiomas()
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function Ver(ByVal id As Integer) As ActionResult
            ViewData("ContainerFluid") = True
            ViewData("Solicitud") = BLL.SolicitudesBLL.Obtener(id)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function SolicitarPermiso() As ActionResult
            Try
                ' Vamos a ver quien es el responble para enviarle la solicitiud
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
                Dim usuarioPeticionario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser})

                If (usuarioPeticionario IsNot Nothing) Then
                    ' Puede ser que el usuario que tiene que autorizar no tenga permiso en la aplicación
                    ' Vamos a ver si el usuario ya es solicitante de la aplicación
                    Dim usuarioResponsable As SabLib.ELL.Usuario
#If DEBUG Then
                    usuarioResponsable = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = 64395})
#Else
                    usuarioResponsable = usuariosBLL.GetUsuariosConRecurso(ConfigurationManager.AppSettings("RecursoWeb"), True, Ticket.IdPlanta).FirstOrDefault(Function(f) f.Id = usuarioPeticionario.IdResponsable)

                    If (usuarioResponsable Is Nothing) Then
                        BLL.UsuariosRolBLL.GuardarSolicitante(usuarioResponsable.Id, ConfigurationManager.AppSettings("RecursoWeb"))
                    End If
#End If
                    Dim permiso As New Permiso With {.IdSab = Ticket.IdUser, .IdSabResponsable = usuarioResponsable.Id}
                    BLL.PermisoBLL.Guardar(permiso)
                    EnviarMailSolicitudPermiso(usuarioPeticionario, usuarioResponsable)
                    MensajeInfo(Utils.Traducir("Solicitud de permiso enviada correctamente"))
                End If
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al enviar la solicitud de permiso"))
            End Try

            Return RedirectToAction("MisSolicitudes")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="usuarioPeticionario"></param>
        ''' <param name="usuarioResponsable"></param>
        Private Sub EnviarMailSolicitudPermiso(ByVal usuarioPeticionario As SabLib.ELL.Usuario, ByVal usuarioResponsable As SabLib.ELL.Usuario)
            Try
                If (Not String.IsNullOrEmpty(usuarioResponsable.Email)) Then
                    Dim mailto As String = usuarioResponsable.Email
                    Dim subject As String = "Nueva solicitud de permiso de tarjetas de visita"
                    Dim uri = "https://intranet2.batz.es/langileentxokoa/?url=" & Url.Action("SolicitudesPermiso", "Permiso", Nothing, Request.Url.Scheme)
                    Dim body As String = String.Format(Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\NuevaSolicitudPermiso.html")), usuarioPeticionario.NombreCompleto, uri)

                    '***************************************
#If Not DEBUG Then
                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()
                    Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                    Dim userExchange As String = ConfigurationManager.AppSettings("UserExchange")
                    Dim passExchange As String = ConfigurationManager.AppSettings("PassExchange")
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, oParams.ServidorEmail, Nothing, Nothing, userExchange, passExchange)
#End If
                End If
            Catch ex As Exception
                log.Error("Se ha producido un error al EnviarMailSolicitudPermiso", ex)
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="solicitud"></param>
        Private Sub EnviarMailSolicitudCompras(ByVal solicitud As ELL.Solicitud)
            Dim mailto As String = String.Empty

            Try
                If (solicitud.IdNegocio = ELL.Solicitud.NEGOCIO_SERVICIOS_GENERALES OrElse solicitud.IdNegocio = ELL.Solicitud.NEGOCIO_TROQUELERIA) Then
                    mailto = ConfigurationManager.AppSettings("MailToComprasTroqueleria")
                ElseIf (solicitud.IdNegocio = ell.Solicitud.NEGOCIO_SISTEMAS) Then
                    mailto = ConfigurationManager.AppSettings("MailToComprasSistemas")
                End If

                If (Not String.IsNullOrEmpty(mailto)) Then
                    Dim subject As String = "Nueva solicitud de tarjetas de visita"
                    Dim body As String = String.Format(Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\NuevaSolicitud.html")),
                                                       Ticket.NombreCompleto, solicitud.Nombre, solicitud.Puesto, solicitud.Movil, solicitud.Direccion, solicitud.Fijo, solicitud.Email, solicitud.Cajas)

                    '***************************************
#If Not DEBUG Then
                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim oParams As SabLib.ELL.Parametros = paramBLL.consultar()
                    Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                    Dim userExchange As String = ConfigurationManager.AppSettings("UserExchange")
                    Dim passExchange As String = ConfigurationManager.AppSettings("PassExchange")
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, oParams.ServidorEmail, Nothing, Nothing, userExchange, passExchange)
#End If
                End If
            Catch ex As Exception
                log.Error("Se ha producido un error al EnviarMailSolicitudCompras", ex)
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        'Function CargarIdiomas() As List(Of String)
        '    Los idiomos que vamos a dejar seleccionar son aquellos para los cuales el puesto este seleccionado.
        '     Partimos de que los puestos en PKS están en inglés
        '    Dim listaIdiomas As New List(Of String)
        '    listaIdiomas.Add("en-GB")

        '    Vamos a ver cual es el puesto del usuario conectado en PKS
        '    Dim puesto As ELL.Puesto = BLL.PKSBLL.ObtenerPuesto(Ticket.IdUser)

        '    Vamos a tratar de traducir este puesto a los diferentes idiomas disponibles. El inglés lo omitimos
        '    For Each idioma In ELL.Puesto.Idiomas.Where(Function(f) f <> "en-GB")
        '        If (Utils.Traducir(puesto.Nombre, idioma) <> puesto.Nombre) Then
        '            listaIdiomas.Add(idioma)
        '        End If
        '    Next

        '    Return listaIdiomas
        'End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cultura"></param>
        Private Sub CargarComboIdiomas(Optional ByVal cultura As String = Nothing)
            ' Los idiomas que vamos a dejar seleccionar son aquellos para los cuales el puesto este seleccionado.
            ' Partimos de que los puestos en PKS están en inglés
            Dim listaCulturas As New List(Of String)
            listaCulturas.Add("en-GB")

            ' Vamos a ver cual es el puesto del usuario conectado en PKS
            Dim puesto As ELL.Puesto = BLL.PKSBLL.ObtenerPuesto(Ticket.IdUser)

            ' Vamos a tratar de traducir este puesto a los diferentes idiomas disponibles. El inglés lo omitimos
            Dim termino As LocalizationLib.LanguageAdministration.Termino
            For Each idioma In ELL.Puesto.Idiomas
                termino = LocalizationLib.LanguageAdministration.GetTermino(ConfigurationManager.AppSettings("LocalPath"), idioma, puesto.Nombre)

                If (termino.Traducido) Then
                    listaCulturas.Add(idioma)
                End If
            Next

            Dim idiomasLI As New List(Of Mvc.SelectListItem)
            idiomasLI = listaCulturas.Select(Function(f) New Mvc.SelectListItem With {.Text = Utils.Traducir(f), .Value = f}).OrderBy(Function(f) f.Text).ToList()

            If (Not String.IsNullOrEmpty(cultura) AndAlso idiomasLI.Exists(Function(f) f.Value = cultura)) Then
                idiomasLI.First(Function(f) f.Value = cultura).Selected = True
            Else
                idiomasLI.First(Function(f) f.Value = "en-GB").Selected = True
            End If

            ViewData("Idiomas") = idiomasLI
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="nombre"></param>
        ''' <param name="puesto"></param>
        ''' <param name="movil"></param>
        ''' <param name="direccion"></param>
        ''' <param name="fijo"></param>
        ''' <param name="email"></param>
        ''' <param name="cajas"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Agregar(ByVal nombre As String, ByVal puesto As String, ByVal movil As String, ByVal direccion As String, ByVal fijo As String, ByVal email As String, ByVal cajas As Integer) As ActionResult
            ' Tenemos que ver el negocio al que pertenece al usuario solicitante
            Dim departamento As SabLib.ELL.Departamento = ObtenerDepartamento()

            Dim solicitud As New ELL.Solicitud With {.Nombre = nombre, .Puesto = puesto, .Movil = movil, .Direccion = direccion, .Fijo = fijo, .Email = email, .Cajas = cajas, .IdUsuarioAlta = Ticket.IdUser, .IdNegocio = departamento.IdNegocio}

            Try
                BLL.SolicitudesBLL.Guardar(solicitud)
                EnviarMailSolicitudCompras(solicitud)
                MensajeInfo(Utils.Traducir("Datos guardados correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al guardar los datos"))
                Return Agregar()
            End Try

            Return RedirectToAction("MisSolicitudes")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function ReenviarMailCompras(ByVal id As Integer)
            Dim solicitud As ELL.Solicitud = BLL.SolicitudesBLL.Obtener(id)

            Try
                EnviarMailSolicitudCompras(solicitud)
                MensajeInfo(Utils.Traducir("Datos enviados correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al enviar los datos"))
                Return Agregar()
            End Try

            Return RedirectToAction("MisSolicitudes")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Private Function ObtenerDepartamento() As SabLib.ELL.Departamento
            ' Tenemos que ver el negocio al que pertenece al usuario solicitante
            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser})
            Dim departamentosBLL As New SabLib.BLL.DepartamentosComponent()
            Return departamentosBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = usuario.IdDepartamento, .IdPlanta = usuario.IdPlanta})
        End Function
    End Class
End Namespace