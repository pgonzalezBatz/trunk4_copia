Public Partial Class ListaFundicionImprimir
    Inherits System.Web.UI.Page
    Private strCn = ConfigurationManager.ConnectionStrings("LISTAMAT").ConnectionString

    Protected Overrides Sub InitializeCulture()
        If Session("IdTroqueleria") Is Nothing OrElse Session("IdUsuario") Is Nothing OrElse Session("IdCultura") Is Nothing Then
            'No deberia estar aqui
            Response.Redirect("Default.aspx")
        End If
        MyBase.InitializeCulture()
        Culture = Session("IdCultura")
    End Sub
    Private Sub ListaFundicionImprimir_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request("ord") Is Nothing OrElse Request("op") Is Nothing Then
            Response.Redirect("ListaFundicion.aspx")
        End If
        ltlOf.Text = Request("ord") + "," + Request("op")
        'Numero de troquel y nombre del proveedor
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim c = dbAccess.GetCabeceraImpresion(Request("ord"), Request("op"), strCn)
        If Not c Is Nothing Then
            ltlNTroquel.Text = c(0)
            ltlCliente.Text = c(1)
        End If
        rptMarcas.DataSource = dbAccess.GetMarcas(1, Request("ord"), Request("op"), strCn)
        rptMarcas.DataBind()
    End Sub
End Class