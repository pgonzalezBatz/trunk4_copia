Imports System.Runtime.CompilerServices
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
    ''' Obtiene los roles que pueden acceder a la página
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Protected Overridable ReadOnly Property RolesAcceso As List(Of ELL.Rol.RolUsuario)
        Get
            Return Nothing
        End Get
    End Property

    '''' <summary>
    '''' Obtiene los roles del usuario
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    'Protected Property RolesUsuario() As List(Of ELL.UsuarioRol)
    '    Get
    '        If (Session("RolesUsuario") Is Nothing) Then
    '            Return Nothing
    '        Else
    '            Return CType(Session("RolesUsuario"), List(Of ELL.UsuarioRol))
    '        End If
    '    End Get
    '    Set(ByVal value As List(Of ELL.UsuarioRol))
    '        Session("RolesUsuario") = value
    '    End Set
    'End Property

    ''' <summary>
    ''' Rol actual del usuario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Protected Property RolActual() As ELL.UsuarioRol
        Get
            If (Session("RolActual") Is Nothing) Then
                Return Nothing
            Else
                Return CType(Session("RolActual"), ELL.UsuarioRol)
            End If
        End Get
        Set(ByVal value As ELL.UsuarioRol)
            Session("RolActual") = value
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Protected Property Planta As Integer
        Get
            Dim result As Integer = Integer.MinValue
            If (Request.Cookies("Planta") IsNot Nothing) Then
                result = CInt(Request.Cookies("Planta").Value)
            End If

            Return result
        End Get
        Set(value As Integer)
            Response.Cookies("Planta").Value = value
            Response.Cookies("Planta").Expires = DateTime.MaxValue
        End Set
    End Property

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Protected Property Ejercicio As Integer
        Get
            Dim result As Integer = Integer.MinValue
            If (Request.Cookies("Ejercicio") IsNot Nothing) Then
                result = CInt(Request.Cookies("Ejercicio").Value)
            Else
                result = DateTime.Today.Year
                Response.Cookies("Ejercicio").Value = result
                Response.Cookies("Ejercicio").Expires = DateTime.MaxValue
            End If

            Return result
        End Get
        Set(value As Integer)
            Response.Cookies("Ejercicio").Value = value
            Response.Cookies("Ejercicio").Expires = DateTime.MaxValue
        End Set
    End Property

    '''' <summary>
    '''' 
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Protected ReadOnly Property IdPlantaGerente As Integer
    '    Get
    '        ' De los roles que pueda tener el usuario tenemos que coger el de gerente para saber la planta a la que tiene acceso
    '        Dim idPlanta As Integer = Integer.MinValue
    '        If (RolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.RolUsuario.Gerente)) Then
    '            idPlanta = RolesUsuario.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.RolUsuario.Gerente).IdPlanta
    '        End If
    '        Return idPlanta
    '    End Get
    'End Property

    '''' <summary>
    '''' 
    '''' </summary>
    '''' <value></value>
    '''' <returns></returns>
    '''' <remarks></remarks>
    'Protected ReadOnly Property IdPlantaDireccion As Integer
    '    Get
    '        ' De los roles que pueda tener el usuario tenemos que coger el de director para saber la planta  a la que tiene acceso
    '        Dim idPlanta As Integer = Integer.MinValue
    '        If (RolesUsuario.Exists(Function(f) f.IdRol = ELL.Rol.RolUsuario.Director)) Then
    '            idPlanta = RolesUsuario.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.RolUsuario.Director).IdPlanta
    '        End If
    '        Return idPlanta
    '    End Get
    'End Property

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
    ''' 
    ''' </summary>
    ''' <param name="filterContext"></param>
    Protected Overrides Sub OnActionExecuting(filterContext As ActionExecutingContext)
        If (Not ComprobarAcceso()) Then
            filterContext.Result = RedirectToAction("AccesoDenegado", "Login")
            Return
        End If
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
            Dim usuarioRoles As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser)

            'Quitamos todos los roles menos el actual y administrador
            usuarioRoles.RemoveAll(Function(f) f.IdRol <> RolActual.IdRol OrElse f.IdRol <> ELL.Rol.RolUsuario.Administrador)

            For Each usuarioRol As ELL.UsuarioRol In usuarioRoles
                idRol = usuarioRol.IdRol
                existe = RolesAcceso.Exists(Function(f) f = [Enum].Parse(GetType(ELL.Rol.RolUsuario), idRol.ToString()))
                If (existe) Then
                    Return existe
                End If
            Next
        End If

        Return existe
    End Function

#End Region

End Class