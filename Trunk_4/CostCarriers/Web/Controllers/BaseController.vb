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
    Protected Overridable ReadOnly Property RolesAcceso As List(Of ELL.Rol.TipoRol)
        Get
            Dim roles As New List(Of ELL.Rol.TipoRol)
            roles.Add(ELL.Rol.TipoRol.Responsable_ingenieria_planta)
            roles.Add(ELL.Rol.TipoRol.Gerente_planta)
            roles.Add(ELL.Rol.TipoRol.Financiero)
            roles.Add(ELL.Rol.TipoRol.Project_manager)
            roles.Add(ELL.Rol.TipoRol.Responsable_advance)
            roles.Add(ELL.Rol.TipoRol.Admin)
            roles.Add(ELL.Rol.TipoRol.Direccion_CMP)
            roles.Add(ELL.Rol.TipoRol.Direccion_operaciones)
            roles.Add(ELL.Rol.TipoRol.Product_manager)
            Return roles
        End Get
    End Property

    ''' <summary>
    ''' Obtiene los roles del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Protected Property RolesUsuario() As List(Of ELL.UsuarioRol)
        Get
            If (Session("RolesUsuario") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("RolesUsuario"), List(Of ELL.UsuarioRol))
            End If
        End Get
        Set(ByVal value As List(Of ELL.UsuarioRol))
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

#Region "Métodos"

    ''' <summary>
    ''' Establece la cultura en cada ejecución de acción
    ''' </summary>
    ''' <param name="filterContext"></param>
    Protected Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
        Dim defaultCulture As String = "en-GB"

        Threading.Thread.CurrentThread.CurrentCulture = Globalization.CultureInfo.GetCultureInfo(defaultCulture)
        Threading.Thread.CurrentThread.CurrentUICulture = Globalization.CultureInfo.GetCultureInfo(defaultCulture)
    End Sub

    ''' <summary>
    ''' Comprueba que alguno de los roles del usuario está contenido en la lista de roles de acceso de la pagina
    ''' </summary>
    ''' <returns>True si existe alguno. False en caso contrario</returns>
    ''' <remarks></remarks>
    Private Function ComprobarAcceso() As Boolean
        Dim idRol As Integer = Integer.MinValue
        Dim existe As Boolean = True
        If (RolesAcceso IsNot Nothing) Then
            For Each usuarioRol As ELL.UsuarioRol In RolesUsuario
                idRol = usuarioRol.IdRol
                existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Rol.TipoRol), idRol.ToString()))
                If (existe) Then
                    Return existe
                End If
            Next
        End If

        Return existe
    End Function

#End Region

End Class