Partial Public Class _Default
    Inherits System.Web.UI.Page
    Private strCn As String = ConfigurationManager.ConnectionStrings("SAB").ConnectionString
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Request("IdSession") Is Nothing Then
            'TODO: Remove this
            If Request("IdSession") = "d0mic3auhzhumxvv3ip1yp45" Then
                Culture = "es-ES"
                Session("IdCultura") = "es-ES"
                Session("IdTroqueleria") = 3226
                Session("NombreEmpresa") = "GLOBAL EQUIPAMIENTO INTEGRAL"
                Session("IdUsuario") = 434
                h.SetCulture(828, strCn)
                Response.Redirect("ListaFundicion.aspx")
            End If

            Dim q = "SELECT IDCULTURAS, IDTROQUELERIA,EMPRESAS.NOMBRE, USUARIOS.ID FROM TICKETS INNER JOIN USUARIOS ON USUARIOS.ID=" _
                    + "TICKETS.IDUSUARIOS INNER JOIN EMPRESAS ON EMPRESAS.ID=USUARIOS.IDEMPRESAS WHERE TICKETS.ID=:ID"
            Dim t = OracleManagedDirectAccess.seleccionar(Function(r As Oracle.ManagedDataAccess.Client.OracleDataReader) New String() {r("IDCULTURAS").ToString, r("IDTROQUELERIA").ToString, r("NOMBRE").ToString, r("ID").ToString}, q, strCn,
                            New Oracle.ManagedDataAccess.Client.OracleParameter("ID", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2,
                                                                         Request("IdSession"), ParameterDirection.Input))
            If t.Count = 0 Then
                Response.Redirect(ConfigurationManager.AppSettings.Get("LoginPage"))
            End If

            Dim q2 = "DELETE FROM TICKETS WHERE ID =:ID"
            Dim p2 As New Oracle.ManagedDataAccess.Client.OracleParameter("ID", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, Request("IdSession"), ParameterDirection.Input)
            OracleManagedDirectAccess.NoQuery(q2, strCn, p2)
            Culture = t(0)(0)
            Session("IdCultura") = t(0)(0)
            Session("IdTroqueleria") = t(0)(1)
            Session("NombreEmpresa") = t(0)(2)
            Session("IdUsuario") = t(0)(3)
            h.SetCulture(t(0)(3), strCn)
            Response.Redirect("ListaFundicion.aspx")
        End If
        'TODO
        'Session("IdCultura") = "es-ES" 
        'Session("IdTroqueleria") = "5317 "
        'Session("NombreEmpresa") = "Moldistec"
        'Session("IdUsuario") = "54"
        If Not Session("IdTroqueleria") Is Nothing And Not Session("IdUsuario") Is Nothing Then
            'OK
            'Response.Redirect("ListaFundicion.aspx")
            Response.Redirect("Duplicadolista.aspx")
        End If
        'Pasa algo raro
        Response.Redirect(ConfigurationManager.AppSettings("ExtranetURL"))
    End Sub
End Class