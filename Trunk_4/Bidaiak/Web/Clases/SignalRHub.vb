Imports Microsoft.AspNet.SignalR

Namespace SignalR

    Public Class SignalRHub
        Inherits Hub

        '''' <summary>
        '''' Finaliza el proceso de importacion de visas
        '''' </summary>
        '''' <param name="lAsientosContCab">Lista de asientos contables</param>
        'Public Function ImportarFacturaEroski(ByVal lAsientosContCab As List(Of BidaiakLib.ELL.AsientoContableCab)) As Integer
        '    Dim solicAgenBLL As New BidaiakLib.BLL.SolicAgenciasBLL
        '    Return solicAgenBLL.ImportarFacturaEroski(lAsientosContCab, Clients.Caller)
        'End Function

        '''' <summary>
        '''' Finaliza el proceso de importacion de visas
        '''' </summary>
        '''' <param name="idPlanta"></param>
        'Public Sub ImportarVisas(ByVal idPlanta As Integer)
        '    Dim visasBLL As New BidaiakLib.BLL.VisasBLL
        '    visasBLL.ImportarVisas(idPlanta, Clients.Caller)
        'End Sub


        'Public Sub Send(ByVal message As String)
        'Dim hubContext As Microsoft.AspNet.SignalR.IHubContext = Microsoft.AspNet.SignalR.GlobalHost.ConnectionManager.GetHubContext(Of SignalR.SignalRHub)()                        
        'Clients.Caller.sendMessage(message)
        'hubContext.Clients.All.sendMessage(message)
        'End Sub

        'Public Sub RedirectTo(ByVal url As String)
        '   Clients.Caller.redirectTo(url)
        'End Sub

    End Class

End Namespace