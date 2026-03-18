Public Class SelectPlant
    Inherits PageBase

    ''' <summary>
    ''' Carga de la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.NotShowHeader()
            Master.SetTitle = "Planta"
            cargarPlantas()
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnEntrar)
        End If
    End Sub

    ''' <summary>
    ''' Carga las plantas que de momento se pueden gestionar
    ''' De momento, solo son la planta de Igorre y la de Energy
    ''' </summary>    
    Private Sub cargarPlantas()
        If (ddlPlantas.Items.Count = 0) Then
            ddlPlantas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("SeleccioneUno"), "0"))
            ddlPlantas.Items.Add(New ListItem("Batz Igorre", "1"))
            ddlPlantas.Items.Add(New ListItem("Batz Energy", "227"))
        End If
    End Sub

    ''' <summary>
    ''' Se informa la planta y se le asigna en session el id de la planta que va a gestionar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnEntrar_Click(sender As Object, e As EventArgs) Handles btnEntrar.Click
        If (ddlPlantas.SelectedValue = "0") Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione una planta")
        Else
            log.Info("Login - " & Master.Ticket.NombreUsuario & " (" & Server.MachineName & ") va a gestionar la planta " & ddlPlantas.SelectedValue)
            Session("IdPlanta") = ddlPlantas.SelectedValue
            Response.Redirect("~/Default.aspx", False)
        End If
    End Sub
End Class