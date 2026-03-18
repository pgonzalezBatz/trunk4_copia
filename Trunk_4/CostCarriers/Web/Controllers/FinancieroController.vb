Imports System.Web.Mvc

Namespace Controllers
    Public Class FinancieroController
        Inherits BaseController

#Region "Propiedades"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides ReadOnly Property RolesAcceso As List(Of ELL.Rol.TipoRol)
            Get
                Dim roles As New List(Of ELL.Rol.TipoRol)
                roles.Add(ELL.Rol.TipoRol.Financiero)
                Return roles
            End Get
        End Property

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        ReadOnly Property ElementosPorPagina As Integer = 20

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="paginaActual"></param>
        ''' <returns></returns>
        Function Index(Optional ByVal paginaActual As Integer = 1) As ActionResult
            CargarCabeceras()

            ViewData("ElementosPorPagina") = ElementosPorPagina
            ViewData("PaginaActual") = paginaActual

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Function DetallePasos(ByVal idCabecera As Integer)
            CargarCabecera(idCabecera)
            CargarStepsAGestionar(idCabecera)

            Return View()
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        Private Sub CargarCabeceras()
            ' Primero necesito saber si soy financiero en alguna panta
            Dim usuariosRol As List(Of ELL.UsuarioRol) = RolesUsuario.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero).ToList()
            Dim steps As New List(Of ELL.Step)

            For Each usuarioRol In usuariosRol
                ' Cargamos los steps en estado approved, opened o closed
                steps.AddRange(BLL.StepsBLL.CargarListadoPorPlantaConEstadoValidacion(usuarioRol.IdPlanta).Where(Function(f) (f.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Opened OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Closed) AndAlso Not f.EsInfoGeneral).ToList())
            Next

            ' De todos los steps sacamos los id proyecto diferentes
            Dim listaIdProyectos As List(Of String) = (From lstStep In steps
                                                       Group lstStep By lstStep.Proyecto Into agrupacion = Group
                                                       Select Proyecto).ToList()

            ' De cada proyecto sacamos sus datos de cabecera
            Dim listaCabeceras As New List(Of ELL.CabeceraCostCarrier)

            Dim cabeceraCostCarrier As ELL.CabeceraCostCarrier
            For Each idProyecto In listaIdProyectos
                cabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.ObtenerByProyecto(idProyecto, False)

                cabeceraCostCarrier.Steps = steps.Where(Function(f) f.IdCabecera = cabeceraCostCarrier.Id).ToList()

                If (cabeceraCostCarrier.Steps.Count > 0) Then
                    If (cabeceraCostCarrier.Steps.Exists(Function(f) f.IdEstadoValidacion = ELL.Validacion.Estado.Approved AndAlso String.IsNullOrEmpty(f.CostCarrier))) Then
                        cabeceraCostCarrier.ContienePasosAbrir = True
                    End If
                End If

                listaCabeceras.Add(cabeceraCostCarrier)
            Next

            ViewData("CabecerasProyecto") = listaCabeceras
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdValidacionesLineaAbrir"></param>
        ''' <returns></returns>
        <HttpPost>
        Function Abrir(ByVal hIdValidacionesLineaAbrir As String) As ActionResult
            Try
                Dim listaIdValidacionesLinea As List(Of String) = hIdValidacionesLineaAbrir.Split("-").ToList()
                Dim validacionLinea As ELL.ValidacionLinea
                Dim paso As ELL.Step

                For Each idValidacionLinea In listaIdValidacionesLinea.Where(Function(f) Not String.IsNullOrEmpty(f))
                    BLL.ValidacionesLineaBLL.AñadirEstadoValidacion(idValidacionLinea, Ticket.IdUser, String.Empty, ELL.Validacion.Estado.Opened, ELL.Validacion.Accion.Open, Integer.MinValue)

                    'Se envía el mail al usuario para indicarle que su paso ha sido abierto
                    validacionLinea = BLL.ValidacionesLineaBLL.Obtener(idValidacionLinea)
                    paso = BLL.StepsBLL.Obtener(validacionLinea.IdStep)

                    Try
                        EnviarEmailUsuario(paso, validacionLinea.Id)
                    Catch ex As Exception
                        log.Error("Error al enviar mail al usuario paso abierto")
                    End Try
                Next

                MensajeInfo(Utils.Traducir("Paso/s abierto/s correctamente"))
            Catch ex As Exception
                log.Error("Error al abrir el/los paso/s", ex)
                MensajeError(Utils.Traducir("Error al abrir el/los paso/s"))
            End Try

            Return RedirectToAction("Index")
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        Private Sub CargarCabecera(ByVal idCabecera As Integer)
            ViewData("CabeceraProyecto") = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, False)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        Private Sub CargarStepsAGestionar(ByVal idCabecera As Integer)
            ' Primero necesito saber si soy financiero en alguna planta
            Dim usuariosRol As List(Of ELL.UsuarioRol) = RolesUsuario.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero).ToList()
            Dim steps As New List(Of ELL.Step)

            For Each usuarioRol In usuariosRol
                ' Sacamos el id planta de la plantilla
                steps.AddRange(BLL.StepsBLL.CargarListadoPorPlantaConEstadoValidacion(usuarioRol.IdPlanta).Where(Function(f) f.IdCabecera = idCabecera AndAlso Not f.EsInfoGeneral AndAlso (f.IdEstadoValidacion = ELL.Validacion.Estado.Approved OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Opened OrElse f.IdEstadoValidacion = ELL.Validacion.Estado.Closed)).ToList())
            Next

            ' Vamos a obtener las distintos steps
            steps = steps.GroupBy(Function(f) f.Id).Select(Function(o) o.First).ToList()

            ViewData("Steps") = steps.OrderBy(Function(f) f.IdCostGroup).ToList
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="paso"></param>
        ''' <param name="idValidacionLinea"></param>
        Private Sub EnviarEmailUsuario(ByVal paso As ELL.Step, ByVal idValidacionLinea As Integer)
            Try
                ' Para obtener el mail de usuario que realizó el envío tenemos que navegador por el histórico de estados y buscar el primer estado waiting for aproval
                Dim historicoEstado As ELL.HistoricoEstadoLinea = BLL.HistoricosEstadoLineaBLL.CargarListadoPorValidacionLinea(idValidacionLinea).OrderBy(Function(f) f.Fecha).FirstOrDefault()
                Dim uri = Url.Action("Index", "CostCarriers", Nothing, Request.Url.Scheme)
                Dim subject As String = Utils.Traducir("Paso abierto")
                Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepAbierto.html"))
                Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                Dim mailto As String = historicoEstado.EmailUsuario
                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent()
                Dim nombreUsuarioAbre As String = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = Ticket.IdUser}).NombreCompleto
                Dim nombreProyecto As String = BLL.CabecerasCostCarrierBLL.Obtener(paso.IdCabecera, False).NombreProyecto
                Dim planta As String = BLL.PlantasBLL.Obtener(paso.IdPlanta).Planta
                Dim estado As String = BLL.EstadosBLL.Obtener(paso.IdEstado).Estado
                Dim costGroup As String = BLL.CostsGroupBLL.Obtener(paso.IdCostGroup).Descripcion

                '***************************************            
                body = String.Format(body, nombreProyecto, nombreUsuarioAbre, planta, estado, costGroup, paso.Descripcion, uri, paso.Id)
                '***************************************

#If Not DEBUG Then
                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                If (notificacionesActivas) Then
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, serverEmail)
                End If
#End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail de paso abierto", ex)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="codigoProyecto"></param>
        Private Sub EnviarEmailCambioCodigoProyecto(ByVal idCabecera As Integer, ByVal codigoProyecto As String)
            Try
                ' Vamos a ver todas las plantas que componen el proyecto
                Dim listaPlantas As List(Of ELL.Planta) = BLL.PlantasBLL.CargarListado(idCabecera)
                Dim mailto As String = String.Empty
                ' Por cada planta vamos a ver quien es el financiero. Excluimos corporativo
                Dim usuariosRoles As New List(Of ELL.UsuarioRol)
                For Each planta In listaPlantas.Where(Function(f) f.IdPlanta <> 0)
                    usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorPlanta(planta.IdPlanta).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Financiero AndAlso Not String.IsNullOrEmpty(f.Email)).ToList()
                Next

                ' El 26/02/2021 Silvia pide que se incluya al solicitante en este envio de mail
                ' Cogemos el responsable del proyecto y a través de eso sacamos el usuario
                Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, False)
                Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
                Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.NombreUsuario = cabecera.Responsable})

                If (usuario IsNot Nothing) Then
                    mailto = usuario.Email & ";"
                End If

                If (usuariosRoles.Count > 0) Then
                    usuariosRoles.Select(Function(f) f.Email).Distinct.ToList.ForEach(Sub(s) mailto &= s & ";")

                    Dim subject As String = Utils.Traducir("Código de proyecto establecido")
                    Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\CodigoProyectoEstablecido.html"))
                    Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                    Dim paramBLL As New SabLib.BLL.ParametrosBLL
                    Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                    Dim nombreProyecto As String = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, False).NombreProyecto
                    Dim uri = Url.Action("Index", "CostCarriers", Nothing, Request.Url.Scheme)

                    '***************************************            
                    body = String.Format(body, codigoProyecto, nombreProyecto, uri)
                    '***************************************

#If Not DEBUG Then
                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                If (notificacionesActivas) Then
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, mailto, subject, body, serverEmail)
                End If
#End If
                End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail de cambio de código de proyecto", ex)
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hIdCabecera"></param>
        ''' <param name="txtCodigo"></param>
        ''' <returns></returns>
        <HttpPost>
        Function CambiarCodigo(ByVal hIdCabecera As Integer, ByVal txtCodigo As String) As ActionResult
            Try
                BLL.CabecerasCostCarrierBLL.CambiarCodigoProyecto(hIdCabecera, txtCodigo.ToUpper())
                Dim idProject As String = BLL.CabecerasCostCarrierBLL.Obtener(hIdCabecera, False).Proyecto
                ' Aquí llamamos al servicio de bonos para que actualice en PTKSIS
                Using cliente As New ServicioBonos.ServicioBonos
                    log.Info("Llamamos a UpdateProjectCostCarrier con proyecto " & idProject & "(IdCab:" & hIdCabecera & ") y portador " & txtCodigo.ToUpper())

                    Dim ret As Integer = cliente.UpdateProjectCostCarrier(idProject, txtCodigo.ToUpper())

                    Select Case ret
                        Case 0
                            ' Todo correcto
                            log.Info("Llamada a UpdateProjectCostCarrier correcta")
                            ' Enviamos un mail a los financieros de todas las plantas para indicarles que se ha establecido el código de proyecto
                            EnviarEmailCambioCodigoProyecto(hIdCabecera, txtCodigo)
                            MensajeInfo(Utils.Traducir("Código de proyecto cambiado correctamente"))
                        Case 1, 2
                            log.Error("Llamada a UpdateProjectCostCarrier errónea")
                            Dim owner As String = cliente.GetOwner(idProject)
                            MensajeError(String.Format(Utils.Traducir("Se ha producido un error al actualizar el portador PXXX en PTKSIS. Contacte con el owner YYYY para que lo introduzca manualmente"), txtCodigo.ToUpper(), owner))
                    End Select
                End Using
            Catch ex As Exception
                log.Error("Error al cambiar el código del proyecto", ex)
                MensajeError(Utils.Traducir("Error al cambiar el codigo del proyecto"))
            End Try

            Return RedirectToAction("Index")
        End Function

#End Region

    End Class
End Namespace