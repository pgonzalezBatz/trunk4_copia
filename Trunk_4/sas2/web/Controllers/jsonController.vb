Imports System.Security.Permissions
Namespace web
    Public Class jsonController
        Inherits System.Web.Mvc.Controller
        'TODO: conexion multiplanta
        Private strCnXbat As String = ConfigurationManager.ConnectionStrings("xbat").ConnectionString
        Private strCn As String = ConfigurationManager.ConnectionStrings("SAS").ConnectionString

        Function Index() As ActionResult
            Return View()
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function Buscar(ByVal q As String) As JsonResult
            Dim l As New List(Of Object)
            l = DBAccess.BuscarHelbide(q, strCnXbat)
            Return Json(l, JsonRequestBehavior.AllowGet)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function BuscarProveedor(ByVal q As String) As JsonResult
            Return Json(DBAccess.BuscarProveedor(q, strCn), JsonRequestBehavior.AllowGet)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function GetProveedor(ByVal id As Integer) As JsonResult
            Return Json(DBAccess.GetProveedorConDireccion(id, strCn), JsonRequestBehavior.AllowGet)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function BuscarNumord(ByVal q As String) As JsonResult
            Dim l = DBAccess.GetListNumOrdActivas(strCn)
            Return Json(l.FindAll(Function(s) Regex.IsMatch(s.ToString, q, RegexOptions.IgnoreCase)), JsonRequestBehavior.AllowGet)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function BuscarNumope(ByVal numord As Integer) As JsonResult
            Return Json(DBAccess.GetListNumOpActivas(numord, strCn), JsonRequestBehavior.AllowGet)
        End Function
        <PrincipalPermission(SecurityAction.Demand, Authenticated:=True)> _
        Function BuscarMarca(ByVal numord As Integer, ByVal numope As Integer) As JsonResult
            Return Json(DBAccess.GetListMarcas(numord, numope, strCn), JsonRequestBehavior.AllowGet)
        End Function
    End Class
End Namespace