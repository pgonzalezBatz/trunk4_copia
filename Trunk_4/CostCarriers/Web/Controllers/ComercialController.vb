Imports System.Web.Mvc

Namespace Controllers
    Public Class ComercialController
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
                roles.Add(ELL.Rol.TipoRol.Comercial)
                Return roles
            End Get
        End Property

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Function Index() As ActionResult
            CargarLineasPedido()

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Function DetalleProyecto(ByVal idCabecera As Integer) As ActionResult
            CargarLineasPedido(idCabecera:=idCabecera)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPedido"></param>
        ''' <returns></returns>
        Function DetallePedido(ByVal idPedido As Integer) As ActionResult
            CargarPedido(idPedido)
            CargarLineasPedido(idPedido:=idPedido)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPedido"></param>
        ''' <returns></returns>
        Function CargarPedido(ByVal idPedido As Integer) As ActionResult
            ViewData("Pedido") = BLL.PedidosBLL.Obtener(idPedido)

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPedido"></param>
        ''' <returns></returns>
        Function CargarLineasPedido(Optional ByVal idCabecera As Integer? = Nothing, Optional ByVal idPedido As Integer? = Nothing)
            ' Primero vamos a ver de que plantas soy comercial
            Dim usuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Comercial).Where(Function(f) f.IdSab = Ticket.IdUser).ToList()
            Dim plantas As List(Of Integer) = usuariosRol.Select(Function(f) f.IdPlanta).ToList()

            Dim lineaspedido As New List(Of ELL.LineaPedido)
            If (idCabecera IsNot Nothing) Then
                lineaspedido = BLL.LineasPedidoBLL.CargarListado().Where(Function(f) plantas.Exists(Function(g) g = f.IdPlantaSAB AndAlso f.IdCabecera = idCabecera)).ToList()
            ElseIf (idPedido IsNot Nothing) Then
                lineaspedido = BLL.LineasPedidoBLL.CargarListado(idPedido).Where(Function(f) plantas.Exists(Function(g) g = f.IdPlantaSAB)).ToList()
            Else
                lineaspedido = BLL.LineasPedidoBLL.CargarListado().Where(Function(f) plantas.Exists(Function(g) g = f.IdPlantaSAB)).ToList()
            End If

            ViewData("LineasPedido") = lineaspedido.OrderBy(Function(f) f.Posiciones).ToList() ' Pide Maite el 02/07/2020 que se ordenen por posicion de menor a mayor

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="hfIdPedido"></param>
        ''' <param name="hfIdCabecera"></param>
        ''' <returns></returns>
        <HttpPost>
        Public Function Facturar(ByVal hfIdPedido As Integer, ByVal hfIdCabecera As Integer) As ActionResult
            Dim idLinea As Integer
            Dim numFactura As String
            Dim bError As Boolean = False
            Dim idUsuarioAlta As Integer
            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim email As String
            Dim lineaPedido As ELL.LineaPedido
            For Each key In Request.Params.AllKeys.Where(Function(f) f.StartsWith("txtNumFactura-"))
                idLinea = key.Split("-")(1)
                numFactura = Request.Params(key)
                Try
                    If (Not String.IsNullOrEmpty(numFactura)) Then
                        BLL.LineasPedidoBLL.Facturar(idLinea, numFactura, Ticket.IdUser)
                        idUsuarioAlta = BLL.HistoricosEstadoFacturacionBLL.CargarListado(idLinea).OrderBy(Function(f) f.FechaAlta).First().IdUsuarioAlta
                        email = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUsuarioAlta}).Email
                        lineaPedido = BLL.LineasPedidoBLL.Obtener(idLinea)

                        EnviarEmailCreador(email, lineaPedido)
                    End If
                Catch ex As Exception
                    bError = True
                End Try
            Next

            If (bError) Then
                MensajeError(Utils.Traducir("Error al facturar"))
                Return DetallePedido(hfIdPedido)
            Else
                MensajeInfo(Utils.Traducir("Facturación realizada correctamente"))
                Return RedirectToAction("DetalleProyecto", New With {.idCabecera = hfIdCabecera})
            End If

            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="email"></param>
        Private Sub EnviarEmailCreador(ByVal email As String, ByVal lineaPedido As ELL.LineaPedido)
            Try
                Dim body As String = Utils.LeerFicheroTexto(Server.MapPath("~\Content\PlantillasMail\StepFacturado.html"))
                Dim mailFrom As String = ConfigurationManager.AppSettings("MailFrom")
                Dim uri = Url.Action("Index", "CostCarriers", Nothing, Request.Url.Scheme)
                Dim subject As String = Utils.Traducir("Facturado") & ": " & lineaPedido.NombreStep

                Dim paramBLL As New SabLib.BLL.ParametrosBLL
                Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
                body = String.Format(body, uri, lineaPedido.NombreProyecto, lineaPedido.CostCarrier, lineaPedido.NombreStep, lineaPedido.Posiciones, lineaPedido.Porcentaje, lineaPedido.Importe.ToString("N0", System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")), lineaPedido.NumPedido)

#If Not DEBUG Then
                Dim notificacionesActivas As Boolean = CBool(ConfigurationManager.AppSettings("NotificacionesActivas"))

                If (notificacionesActivas) Then
                    SabLib.BLL.Utils.EnviarEmail(mailFrom, email, subject, body, serverEmail)
                End If
#End If
            Catch ex As Exception
                log.Error("Se ha producido un error al enviar el mail a los comerciales", ex)
                Throw ex
            End Try
        End Sub

#End Region
    End Class
End Namespace