Imports System.Web.Mvc

Public Class LoginController
    Inherits BaseController

#Region "Métodos"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Function AccesoDenegado() As ActionResult
        Return View("AccesoDenegado")
    End Function

    ''' <summary>
    ''' </summary>    
    Function Salir() As RedirectResult
        Dim url As String = "/extranetlogin"

        If (TicketExt IsNot Nothing) Then
            url &= String.Format("/access/resources/?IdSession={0}", TicketExt.Ticket.IdSession)
            Try
                Dim generi As New SabLib.BLL.LoginComponent
                generi.SetTicketEnBD(New SabLib.ELL.Ticket With {.IdSession = TicketExt.Ticket.IdSession, .IdUser = TicketExt.Ticket.IdUser})
            Catch
            End Try
        End If

        Session("Ticket") = Nothing
        Session.Abandon()
        Session.Clear()
        Return Redirect(url)
    End Function

#End Region

End Class