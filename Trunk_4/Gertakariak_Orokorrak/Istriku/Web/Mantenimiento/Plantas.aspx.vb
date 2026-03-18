Imports Oracle.ManagedDataAccess.Client

Public Class Plantas
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            cargarPlantas()
            ddlPlantas.SelectedValue = Session("IDPLANTA")
        End If
    End Sub

    Private Sub cargarPlantas()
        Using cnn As OracleConnection = New OracleConnection("Data Source=XBAT;User Id=sab;Password=sab12;Connection LifeTime=300;statement cache purge=True;validate connection=True;")
            Dim query As String = "SELECT * FROM PLANTAS WHERE OBSOLETO = 0 AND ID IN (" & ConfigurationManager.AppSettings("plantasActivas") & ")"
            Dim cmd As OracleCommand = New OracleCommand(query, cnn)
                cmd.CommandType = CommandType.Text
                Dim da As OracleDataAdapter = New OracleDataAdapter(cmd)
                Dim dt As DataTable = New DataTable()
                da.Fill(dt)
                ddlPlantas.DataTextField = "NOMBRE"
                ddlPlantas.DataValueField = "ID"
                ddlPlantas.DataSource = dt
                ddlPlantas.DataBind()
            End Using
    End Sub

    Protected Sub imgAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles imgAceptar.Click
        Session("IDPLANTA") = ddlPlantas.SelectedValue
        Response.Redirect("~/Default.aspx", False)
    End Sub

End Class