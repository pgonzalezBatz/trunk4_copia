Imports System.Web.Mvc

Public Class BaseController
    Inherits System.Web.Mvc.Controller

#Region "Propiedades"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property TicketExt() As TicketExt
        Get
            If (Session("Ticket") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("Ticket"), TicketExt)
            End If
        End Get
        Set(ByVal value As TicketExt)
            Session("Ticket") = value
        End Set
    End Property

    ''' <summary>
    ''' Log
    ''' </summary>
    ''' <remarks></remarks>
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root")

#End Region

#Region "Mensajes"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mensaje"></param>
    ''' <remarks></remarks>
    Public Sub MensajeInfo(ByVal mensaje As String)
        TempData("topbar") = New Topbar With {.Mensaje = Utils.Traducir(mensaje), .Estilo = "alert alert-success"}
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mensaje"></param>
    ''' <remarks></remarks>
    Public Sub MensajeError(ByVal mensaje As String)
        TempData("topbar") = New Topbar With {.Mensaje = Utils.Traducir(mensaje), .Estilo = "alert alert-danger"}
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mensaje"></param>
    ''' <remarks></remarks>
    Public Sub MensajeAlerta(ByVal mensaje As String)
        TempData("topbar") = New Topbar With {.Mensaje = Utils.Traducir(mensaje), .Estilo = "alert alert-warning"}
    End Sub

#End Region

#Region "Métodos"

    ''' <summary>
    ''' Establece la cultura en cada ejecución de acción
    ''' </summary>
    ''' <param name="filterContext"></param>
    Protected Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
        Dim ticketExt As TicketExt = CType(filterContext.Controller, BaseController).TicketExt
        If (ticketExt IsNot Nothing) Then
            Dim ticket As SabLib.ELL.Ticket = ticketExt.Ticket
            If (ticket IsNot Nothing) Then
                Dim defaultCulture As String = "es-ES"
                If (ticket IsNot Nothing) Then
                    defaultCulture = ticket.Culture
                End If

                Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.GetCultureInfo(defaultCulture)
                Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.GetCultureInfo(defaultCulture)
            End If
        End If
    End Sub

#End Region

End Class