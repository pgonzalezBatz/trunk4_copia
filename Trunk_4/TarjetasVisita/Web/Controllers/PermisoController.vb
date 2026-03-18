Imports System.Web.Mvc
Imports TarjetasVisitaLib

Namespace Controllers
    Public Class PermisoController
        Inherits BaseController

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function SolicitudesPermiso() As ActionResult
            ViewData("Permisos") = PermisoBLL.CargarListadoAutorizar(Ticket.IdUser)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Historico() As ActionResult
            ViewData("Permisos") = PermisoBLL.CargarListado().OrderByDescending(Function(f) f.FechaSolicitud).ToList()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="autorizado"></param>
        ''' <returns></returns>
        Function Autorizar(ByVal id As Integer, ByVal autorizado As Boolean) As ActionResult
            Try
                Dim permiso As ELL.Permiso = BLL.PermisoBLL.Consultar(id)
                permiso.FechaRespuesta = DateTime.Today
                permiso.Autorizado = autorizado
                PermisoBLL.Autorizar(permiso)
                EnviarMailAutorizada(permiso)

                MensajeInfo(Utils.Traducir("Solicitud de permiso autorizada/rechazada correctamente"))
            Catch ex As Exception
                MensajeError(Utils.Traducir("Se ha producido un error al autorizar/rechazar la solicitud de permiso"))
            End Try

            Return RedirectToAction("SolicitudesPermiso")
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="permiso"></param>
        Private Sub EnviarMailAutorizada(ByVal permiso As ELL.Permiso)
            Try
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
                Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = permiso.IdSab})

                If (usuario IsNot Nothing AndAlso Not String.IsNullOrEmpty(usuario.Email)) Then
                    Dim mailto As String = usuario.Email
                    Dim subject As String = String.Format("Solicitud de permiso de tarjetas de visita {0}", If(permiso.Autorizado, "AUTORIZADA", "RECHAZADA"))
                    Dim uri = Url.Action("MisSolicitudes", "Solicitud", Nothing, Request.Url.Scheme)
                    Dim body As String = String.Format(Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\SolicitudPermisoRespuesta.html")), If(permiso.Autorizado, Utils.Traducir("autorizada", "es-ES"), Utils.Traducir("rechazada", "es-ES")), uri, If(permiso.Autorizado, Utils.Traducir("autorizada", "en-GB"), Utils.Traducir("rechazada", "en-GB")))

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

    End Class
End Namespace