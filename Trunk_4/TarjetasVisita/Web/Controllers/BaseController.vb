Imports System.Web.Mvc

Public Class BaseController
    Inherits System.Web.Mvc.Controller

#Region "Propiedades"

    ''' <summary>
    ''' Obtiene el ticket de la session
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property Ticket() As SabLib.ELL.Ticket
        Get
            If (Session("Ticket") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("Ticket"), SabLib.ELL.Ticket)
            End If
        End Get
        Set(ByVal value As SabLib.ELL.Ticket)
            Session("Ticket") = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable ReadOnly Property RolesAcceso As List(Of TarjetasVisitaLib.ELL.Rol.TipoRol)
        Get
            Dim roles As New List(Of TarjetasVisitaLib.ELL.Rol.TipoRol)
            roles.Add(TarjetasVisitaLib.ELL.Rol.TipoRol.Administrador)
            Return roles
        End Get
    End Property

    ''' <summary>
    ''' Obtiene los roles del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Protected Property RolesUsuario() As List(Of TarjetasVisitaLib.ELL.UsuarioRol)
        Get
            If (Session("RolesUsuario") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("RolesUsuario"), List(Of TarjetasVisitaLib.ELL.UsuarioRol))
            End If
        End Get
        Set(ByVal value As List(Of TarjetasVisitaLib.ELL.UsuarioRol))
            Session("RolesUsuario") = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Overridable ReadOnly Property Page_Size
        Get
            Return 20
        End Get
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
        TempData("topbar") = New Topbar With {.Mensaje = mensaje, .Estilo = "alert alert-success"}
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mensaje"></param>
    ''' <remarks></remarks>
    Public Sub MensajeError(ByVal mensaje As String)
        TempData("topbar") = New Topbar With {.Mensaje = mensaje, .Estilo = "alert alert-danger"}
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="mensaje"></param>
    ''' <remarks></remarks>
    Public Sub MensajeAlerta(ByVal mensaje As String)
        TempData("topbar") = New Topbar With {.Mensaje = mensaje, .Estilo = "alert alert-warning"}
    End Sub

#End Region

End Class