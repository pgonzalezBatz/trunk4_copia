Namespace web
    Public Class revisionController
        Inherits System.Web.Mvc.Controller

        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        <SimpleRoleProvider(Role.revision)> _
        Function productivas() As ActionResult
            Return View(DBAccess.GetListEnvio(True, GetStringTransportistas(strCn), strCn).FindAll(Function(v As Viaje) v.Precio > 0))
        End Function
        <SimpleRoleProvider(Role.revision)> _
        <AcceptVerbs(HttpVerbs.Post)> _
        Function productivas(id As Integer, ByVal precio As Nullable(Of Decimal)) As ActionResult
            If precio.HasValue Then
                DBAccess.InsertOUpdatePedido(id, precio, SimpleRoleProvider.GetId(), False, strCn)
                Return RedirectToAction("productivas")
            End If
            Return View(DBAccess.GetListEnvio(True, GetStringTransportistas(strCn), strCn).FindAll(Function(v As Viaje) v.Precio > 0))
        End Function

        Private Function GetStringTransportistas(ByVal strCn As String) As String
            Dim s As New Text.StringBuilder
            For Each o In h.GetListOfDefaultEmpresaFromStrCn(strCn)
                s.Append(o.id.ToString)
            Next
            s.Remove(s.Length - 1, 1)
            Return s.ToString
        End Function
    End Class
End Namespace