Namespace extranet
    Public Class productivasController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        <SimpleRoleProvider(Role.productiva)>
        Function list() As ActionResult
            Return View(db.GetListOfProductivasPorValorar(SimpleRoleProvider.GetId(), strCn))
        End Function
        <SimpleRoleProvider(Role.productiva)>
        <AcceptVerbs(HttpVerbs.Post)> _
        Function list(ByVal viaje As Integer, ByVal importe As Nullable(Of Decimal), comentario As String, kilometros As Nullable(Of Decimal), puntoespera As Nullable(Of Integer),
                        esperasuperior As Nullable(Of Decimal), suplemento As Nullable(Of Decimal)) As ActionResult
            If Not importe.HasValue Then : ModelState.AddModelError("importe", "Obligatorio introducir importe del viaje") : End If

            If ModelState.IsValid Then
                db.SetImporte(viaje, importe.Value, If(comentario Is Nothing, "", comentario), kilometros, puntoespera, esperasuperior, suplemento, ConfigurationManager.AppSettings("taxistas").Split(",").Contains(SimpleRoleProvider.GetId()), strCn)
                Return RedirectToAction("list")
            End If
            Return View(db.GetListOfProductivasPorValorar(SimpleRoleProvider.GetId(), strCn))
        End Function
    End Class
End Namespace