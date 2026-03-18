Public Class PermisoDenegado
	Inherits PageBase

	Private Sub Page_Init(sender As Object, e As EventArgs) Handles Me.Init
		Dim msg As String = Request.QueryString("msg")
        If Not String.IsNullOrWhiteSpace(msg) Then
            lblMensaje.Text = msg
            Log.Info(msg)
        End If
    End Sub

    Private Sub PermisoDenegado_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        '-------------------------------------------------------------------------------------------------
        'En caso de no tener sesion mandamos al usuario a la pagina de 'Login' si esta en la extranet
        '-------------------------------------------------------------------------------------------------
        If EsExtranet(Request) = True Then
            Dim Menu As Menu = Master.FindControl("mnOpciones")
            If Menu IsNot Nothing AndAlso Menu.Items.Count > 0 Then
                Dim MenuItem As MenuItem = Menu.Items(0)
                MenuItem.ToolTip = "Inicio"
                MenuItem.NavigateUrl = "https://extranet.batz.es/"
                Session.Clear()
                Session.Abandon()
            End If
        End If
        '-------------------------------------------------------------------------------------------------
    End Sub
End Class