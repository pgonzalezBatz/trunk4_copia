Namespace extranet
    Public Class inproductivasController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        <SimpleRoleProvider(Role.inproductiva)>
        Function list() As ActionResult
            Return View(db.getListOfVijeTaxi(SimpleRoleProvider.GetId(), strCn).Where(Function(o) String.IsNullOrEmpty(o.subcontratado)))
        End Function
        <SimpleRoleProvider(Role.inproductiva)>
        Function create() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.inproductiva)>
        Function createsubcontratado() As ActionResult
            Return View()
        End Function
        <SimpleRoleProvider(Role.inproductiva)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function createsubcontratado(ByVal fecha As Nullable(Of DateTime), ByVal origen As String, ByVal destino As String, ByVal observacion As String, ByVal importe As Nullable(Of Decimal), kilometros As Nullable(Of Decimal), puntoespera As Nullable(Of Integer),
                        esperasuperior As Nullable(Of Decimal), suplemento As Nullable(Of Decimal), subcontratado As String) As ActionResult
            If Not fecha.HasValue Then : ModelState.AddModelError("fecha", "Obligatorio introducir fecha del viaje") : End If
            If Not importe.HasValue Then : ModelState.AddModelError("importe", "Obligatorio introducir importe del viaje") : End If
            If String.IsNullOrEmpty(subcontratado) Then : ModelState.AddModelError("subcontratado", "Obligatorio introducir el nombre del taxista subcontratado") : End If
            If ModelState.IsValid Then
                db.insertViajeNoProductivo(SimpleRoleProvider.GetId(), fecha.Value, origen, destino, observacion, importe.Value, kilometros, puntoespera, esperasuperior, suplemento, subcontratado, strCn)
                Return RedirectToAction("listSubcontratacion")
            End If
            Return View()
        End Function
        <SimpleRoleProvider(Role.inproductiva)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function create(ByVal fecha As Nullable(Of DateTime), ByVal origen As String, ByVal destino As String, ByVal observacion As String, ByVal importe As Nullable(Of Decimal), kilometros As Nullable(Of Decimal), puntoespera As Nullable(Of Integer),
                        esperasuperior As Nullable(Of Decimal), suplemento As Nullable(Of Decimal)) As ActionResult
            If Not fecha.HasValue Then : ModelState.AddModelError("fecha", "Obligatorio introducir fecha del viaje") : End If
            If Not importe.HasValue Then : ModelState.AddModelError("importe", "Obligatorio introducir importe del viaje") : End If
            If ModelState.IsValid Then
                db.insertViajeNoProductivo(SimpleRoleProvider.GetId(), fecha.Value, origen, destino, observacion, importe.Value, kilometros, puntoespera, esperasuperior, suplemento, Nothing, strCn)
                Return RedirectToAction("list")
            End If
            Return View()
        End Function
        <SimpleRoleProvider(Role.inproductiva)>
        Function delete(id As Integer) As ActionResult
            Return View(db.getVijeTaxi(id, strCn))
        End Function
        <SimpleRoleProvider(Role.inproductiva)>
        <AcceptVerbs(HttpVerbs.Post)>
        Function delete(id As Integer, confirm As String) As ActionResult
            db.DeleteViajeTaxi(id, strCn)
            Return RedirectToAction("list")
        End Function
        <SimpleRoleProvider(Role.inproductiva)>
        Function listSubcontratacion() As ActionResult
            Return View(db.getListOfVijeTaxi(SimpleRoleProvider.GetId(), strCn).Where(Function(o) Not String.IsNullOrEmpty(o.subcontratado)))
        End Function
    End Class
End Namespace