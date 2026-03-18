Public Class Test_SignalR
    Inherits Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load

    End Sub

    Private Sub btnTestHidden_Click(sender As Object, e As EventArgs) Handles btnTestHidden.Click
        Dim myHub As New SignalR.SignalRHub
        Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()
        hubContext.Clients.All.sendMessage("25", "100")
        hubContext.Clients.All.showMessage("Paso 1")
        Threading.Thread.Sleep(3000)
        hubContext.Clients.All.sendMessage("50", "100")
        hubContext.Clients.All.showMessage("Paso 2")
        Threading.Thread.Sleep(3000)
        hubContext.Clients.All.sendMessage("100", "100")
        hubContext.Clients.All.showMessage("Paso 3")
        Threading.Thread.Sleep(3000)
        Session("resul") = "1"
        'hubContext.Clients.All.redirectTo(Request.Url.Scheme & ":\\www.google.es")
        'hubContext.Clients.All.redirectTo(Request.Url.Scheme & ":\\www.google.es")
        'myHub.RedirectTo("www.google.es")
    End Sub

End Class