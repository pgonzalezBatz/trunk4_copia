Public Class cambiarUsuario
    Inherits System.Web.UI.Page


    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.GTK_Troqueleria")
    Public BBDD As New BatzBBDD.Entities_Gertakariak

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        ddlPlantas.DataSource = From Reg As BatzBBDD.PLANTAS In BBDD.PLANTAS.AsEnumerable
                                Where Not String.IsNullOrWhiteSpace(Reg.XBAT_CONSTRING)
                                Select New ListItem(Reg.NOMBRE, Reg.ID)
        ddlPlantas.DataBind()
    End Sub

    Private Sub cambioUsuario_Click(ByVal sender As Object, ByVal e As EventArgs) Handles cambioUsuario.Click
        Dim codTrab = tbLogin.Text
        Dim pass = tbPass.Text
        Dim planta = ddlPlantas.SelectedValue
        Dim Recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")
        Dim myTicket As New SabLib.ELL.Ticket
        Dim lg As New SabLib.BLL.LoginComponent

        myTicket = lg.Login(CInt(codTrab), SabLib.BLL.Utils.EncriptarPassword(pass), CInt(planta))

        If Not myTicket Is Nothing Then
            Session("Ticket") = myTicket
            'If lg.AccesoRecursoValido(myTicket, Recurso) Then
            '    'Login correcto
            'Else
            '    Log.Info(String.Format("Permiso denegado. Sin acceso al recurso ({0})." _
            '                            & vbNewLine & "Usuario: {1}" _
            '                            , Recurso, myTicket.IdUser))
            '    Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            'End If
        Else
            Log.Info("Permiso denegado. Sin Login.")
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End If
        Response.Redirect("~/Default.aspx", True)
    End Sub

End Class