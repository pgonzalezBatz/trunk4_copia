Imports System.Security.Permissions
Namespace IntranetMVC
    Public Class pedidoController
        Inherits System.Web.Mvc.Controller

        Dim strCn As String = ConfigurationManager.ConnectionStrings("segipe").ConnectionString

        <SimpleRoleProvider(1)>
        Function Listcabecera(ByVal idEstado As Integer) As ActionResult
            ViewData("action") = "detallepedido"
            ViewData("enviar") = False
            Dim idResponsable = "0"
            Dim idProveedor = "0"
            Dim lstFilters As New Specialized.NameValueCollection()
            If idEstado = 1 Then
                ViewData("enviar") = True
            End If
            If Request("responsable") IsNot Nothing Then
                lstFilters.Add("responsable", Request("responsable"))
                idResponsable = Request("responsable")
            End If
            If Request("proveedor") IsNot Nothing Then
                lstFilters.Add("proveedor", Request("proveedor"))
                idProveedor = Request("proveedor")
            End If
            ViewData("listOfFilters") = lstFilters
            Dim lst As List(Of Object) = db.GetListOfCabecera(idEstado, idResponsable, idProveedor, strCn)
            'TODO: Stand by
            'If idEstado = 1 Then
            '    'Añadido para corregir los peidos que llevan certificado
            '    lst.AddRange(db.GetListOfCabecera(17, idResponsable, idProveedor, strCn))
            'End If
            If Request("s") IsNot Nothing Then
                lst.Sort(Function(o1, o2) CDate(o1.fechaMinimaLinea).CompareTo(o2.fechaMinimaLinea))
            End If
            Return View("listcabecera", lst)
        End Function
        <SimpleRoleProvider(1)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function listcabecera(ByVal idPedido As Integer, ByVal idEstado As Integer, ByVal urgente As String, ByVal notificar As String) As ActionResult
            If idEstado = 1 Then
                db.EnviarPedido(idPedido, urgente IsNot Nothing, strCn)
                If notificar IsNot Nothing Then
                    h.SendEmail(db.GetEmailProveedorDelPedido(idPedido, strCn), "Notificación de nuevo pedido de Batz S.Coop", "Puede acceder a los detalles del pedido desde: https://extranet.batz.es/extranetlogin")
                End If
            End If
            Return Redirect("listcabecera?" + Request.QueryString.ToString)
        End Function
        <SimpleRoleProvider(1)>
        Function buscar(ByVal pedido As String, idEstado As Integer) As ActionResult
            If pedido.Length > 0 Then
                ViewData("enviar") = False
                ViewData("action") = "detallepedido"
                Return View(db.GetListOfCabeceraBuscar(pedido.Trim(" "), "0", "0", strCn))
            End If
            Return RedirectToAction("listcabecera", New With {.idestado = idEstado})
        End Function
        <SimpleRoleProvider(1)>
        Function DetallePedido(ByVal idPedido As Integer, ByVal idEstado As Integer) As ActionResult
            ViewData("cabecera") = db.GetCabecera(idPedido, strCn)
            ViewData("observacionesexternas") = db.GetObservaciones(idPedido, 0, strCn)
            ViewData("observacionesinternas") = db.GetObservaciones(idPedido, 1, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(idPedido, strCn)
            ViewData("lineas") = db.GetListOfLineas(idPedido, idEstado, strCn)
            Return View()
        End Function
        <SimpleRoleProvider(1)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function DetallePedido(ByVal idPedido As Integer, ByVal idEstado As Integer, ByVal seleccionar As List(Of Integer), ByVal concertado As List(Of Integer), ByVal proveedor As List(Of Integer)) As ActionResult
            If seleccionar Is Nothing Then
                ModelState.AddModelError("seleccionar", "No se ha seleccionado ningun elemento para aceptar")
            End If
            If ModelState.IsValid Then
                For Each idlinea As Integer In seleccionar
                    If Request.Form("precio_" + idlinea.ToString) Is Nothing AndAlso Request.Form("fecha_" + idlinea.ToString) Is Nothing AndAlso Request.Form("descuento_" + idlinea.ToString) Is Nothing Then
                        'En este caso el proveedor a cometido algun error y a asignado el estado propuesto a algo que no tiene propuestas
                        db.PasarLineaAAceptado(idPedido, idlinea, strCn)
                    ElseIf (Request.Form("precio_" + idlinea.ToString) Is Nothing OrElse IsNumeric(Request.Form("precio_" + idlinea.ToString))) AndAlso _
                        (Request.Form("fecha_" + idlinea.ToString) Is Nothing OrElse IsDate(Request.Form("fecha_" + idlinea.ToString))) AndAlso _
                        (Request.Form("descuento_" + idlinea.ToString) Is Nothing OrElse IsNumeric(Request.Form("descuento_" + idlinea.ToString))) Then
                        db.AcceptChanges(idPedido, idlinea, If(Request.Form("precio_" + idlinea.ToString) Is Nothing, New Nullable(Of Decimal), CType(Request.Form("precio_" + idlinea.ToString), Decimal?)), If(Request.Form("fecha_" + idlinea.ToString) Is Nothing, New Nullable(Of DateTime), CType(Request.Form("fecha_" + idlinea.ToString), DateTime?)), If(Request.Form("descuento_" + idlinea.ToString) Is Nothing, New Nullable(Of Decimal), CType(Request.Form("descuento_" + idlinea.ToString), Decimal?)), concertado IsNot Nothing AndAlso concertado.Contains(idlinea), proveedor IsNot Nothing AndAlso proveedor.Contains(idlinea), strCn)
                    Else
                        ModelState.AddModelError("seleccionar", "Alguno de los valores es incorrento en la linea. Puede que algunas lineas se hayan procesado " + idlinea.ToString)
                    End If
                Next
                If ModelState.IsValid Then
                    'Es valido tadavia?
                    Return Redirect("detallepedido?" + Request.QueryString.ToString)
                End If
            End If
            Return DetallePedido(idPedido, idEstado)
        End Function
        <SimpleRoleProvider(1)>
        Function FilterResponsable() As ActionResult
            ViewData("tipofiltro") = "responsable"
            Return View("filter", db.GetListOfResponsable(ConfigurationManager.AppSettings("recursoSegipe"), strCn))
        End Function
        <SimpleRoleProvider(1)>
        Function FilterProveedor(idEstado As Integer) As ActionResult
            ViewData("tipofiltro") = "proveedor"
            Return View("filter", db.GetListOfProveedor(ConfigurationManager.AppSettings("recursoSegipeProveedor"), idEstado, strCn))
        End Function
        <SimpleRoleProvider(1)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function ObservacionesExternas(ByVal idpedido As Integer, ByVal fromaction As String, ByVal texto As String) As ActionResult
            If texto.Length > 0 Then
                db.SetObservaciones(idpedido, 0, texto, SimpleRoleProvider.GetId(), strCn)
                h.SendEmail(db.GetEmailProveedorDelPedido(idpedido, strCn), "Batz gestion de pedidos. Se ha añadido una observación al pedido " + idpedido.ToString, texto)
            End If
            Return Redirect(fromaction)
        End Function
        <SimpleRoleProvider(1)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function ObservacionesInternas(ByVal idpedido As Integer, ByVal fromaction As String, ByVal texto As String) As ActionResult
            If texto.Length > 0 Then
                db.SetObservaciones(idpedido, 1, texto, SimpleRoleProvider.GetId(), strCn)
            End If
            Return Redirect(fromaction)
        End Function
        <SimpleRoleProvider(1)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function addadjunto(ByVal idpedido As Integer, ByVal fromaction As String, ByVal adjunto As HttpPostedFileBase) As ActionResult
            If adjunto IsNot Nothing Then
                Dim by(adjunto.ContentLength) As Byte
                adjunto.InputStream.Read(by, 0, adjunto.ContentLength)
                'Internet explorer adjunta el nombre completo incluyendo la ruta.
                Dim FileName As String = adjunto.FileName
                If FileName.LastIndexOf("\") > 0 Then
                    FileName = FileName.Substring(FileName.LastIndexOf("\") + 1)
                End If

                Dim log As log4net.ILog = log4net.LogManager.GetLogger("root")
                log.Info("Guardando adjunto en la BD. Nombre: " + FileName)
                db.SetAdjunto(idpedido, FileName, by, strCn)
            End If
            Return Redirect(fromaction)
        End Function
        <SimpleRoleProvider(1)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function deleteAdjunto(ByVal id As Integer, ByVal fromaction As String) As ActionResult
            db.DeleteAdjunto(id, strCn)
            Return Redirect(fromaction)
        End Function
        <SimpleRoleProvider(1)>
        Function adjunto(ByVal id As Integer, ByVal nombre As String) As FileContentResult
            Return File(db.GetAdjunto(id, strCn), " ", nombre)
        End Function
        Function historico(idPedido As Integer, idEstado As Integer, idLinea As Integer)
            ViewData("cabecera") = db.GetCabecera(idPedido, strCn)
            ViewData("observacionesexternas") = db.GetObservaciones(idPedido, 0, strCn)
            ViewData("observacionesinternas") = db.GetObservaciones(idPedido, 1, strCn)
            ViewData("adjuntos") = db.GetAdjuntos(idPedido, strCn)
            ViewData("historico") = db.GetHistoricoLinea(idPedido, idLinea, strCn)
            ViewData("cambiospropuestos") = db.GetCambiosPropuestoLinea(idPedido, idLinea, strCn)
            Return View()
        End Function
    End Class
End Namespace