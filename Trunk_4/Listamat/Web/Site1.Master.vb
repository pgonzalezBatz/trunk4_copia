Public Partial Class Site1
    Inherits System.Web.UI.MasterPage

    Protected Sub MenuPrincipal(ByVal sender As Object, ByVal e As EventArgs)
        OracleManagedDirectAccess.NoQuery("INSERT INTO TICKETS VALUES(:ID, :IDSAB)",
                                              ConfigurationManager.ConnectionStrings("SAB").ConnectionString,
                                New Oracle.ManagedDataAccess.Client.OracleParameter("ID", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2,
                                                                             Session.SessionID, ParameterDirection.Input),
                                New Oracle.ManagedDataAccess.Client.OracleParameter("IDSAB", Oracle.ManagedDataAccess.Client.OracleDbType.Int32,
                                                                             Session("IdUsuario"), ParameterDirection.Input))

        Response.Redirect(ConfigurationManager.AppSettings("ExtranetURL") + "Default.aspx?IdSession=" + Session.SessionID)
    End Sub

    Protected Sub Logout(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        Session.Clear()
        Response.Redirect(ConfigurationManager.AppSettings("ExtranetURL"))
    End Sub
End Class