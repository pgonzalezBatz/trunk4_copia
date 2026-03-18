Imports System.Net
Imports System.Web.SessionState

Public Class Global_asax
    Inherits System.Web.HttpApplication

    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.GertakariakMS")

    Sub Application_Start(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al iniciar la aplicación
        log4net.Config.XmlConfigurator.Configure()
        'log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(Server.MapPath("~") & "\App_Data\log4netConfig.xml"))

#If DEBUG Then
        '--------------------------------------------------------------------------------
        'FROGA: Cambia el "Log" para que se guarde en local.
        '--------------------------------------------------------------------------------
        '      Dim Repositorio As log4net.Repository.ILoggerRepository = log4net.LogManager.GetRepository
        'Dim Anexos As log4net.Appender.IAppender() = Repositorio.GetAppenders()
        'Dim ArchivoAnexo As log4net.Appender.FileAppender = Anexos(0)
        'ArchivoAnexo.File = ".\Logs\Log"
        'ArchivoAnexo.ActivateOptions()

        ConfigurationManager.AppSettings.Item("CurrentStatus") = "DEBUG"
        ConfigurationManager.AppSettings.Item("RecursoWeb") = "265"

        'PageBase.log.Debug("KAIXO - 1")
#Else
		'PageBase.log.Debug("KAIXO - 2")
#End If
        '--------------------------------------------------------------------------------

    End Sub

    Sub Session_Start(ByVal sender As Object, ByVal e As EventArgs)
        ServicePointManager.Expect100Continue = False
        ServicePointManager.SecurityProtocol = CType(3072, SecurityProtocolType)
        ' Se desencadena al iniciar la sesión
        Dim Recurso As String = System.Configuration.ConfigurationManager.AppSettings.Get("RecursoWeb")
        Dim myTicket As New SabLib.ELL.Ticket
        Dim lg As New SabLib.BLL.LoginComponent

        '------------------------------------------------------
        'Codigo para el acceso a la aplicacion Sin Login.
        '------------------------------------------------------
        'myTicket = lg.Login(Request.LogonUserIdentity.Name.ToLower)
        myTicket = lg.Login(User.Identity.Name.ToLower)

        'myTicket = lg.Login("batznt\abelgarcia")
#If DEBUG Then
        'Session("gvGertakariak_Propiedades") = New gtkGridView With {.IdSeleccionado = 23856}
#End If

        If Not myTicket Is Nothing Then
            If lg.AccesoRecursoValido(myTicket, Recurso) Then
                '#If DEBUG Then
                '				myTicket.Culture = "eu-ES"
                '#End If
                Session("Ticket") = myTicket
                Session("PerfilUsuario") = UsuarioPerfil(myTicket.IdUser)
                '#If DEBUG Then
                '	Session("PerfilUsuario") = MPWeb.Perfil.Consultor
                '#End If
            Else
                Response.Redirect(PageBase.PAG_PERMISODENEGADO)
            End If
        Else
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End If
        '------------------------------------------------------
    End Sub

    Sub Application_BeginRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al comienzo de cada solicitud

#If DEBUG And Trace Then
        'Habilitamos el seguimiento para la aplicacion
		Context.Trace.IsEnabled = True
#End If
    End Sub

    Sub Application_AuthenticateRequest(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena al intentar autenticar el uso
    End Sub

    Sub Application_Error(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando se produce un error
        If Server.GetLastError IsNot Nothing Then log.Error(DirectCast(sender, System.Web.HttpApplication).Request.Url.ToString, Server.GetLastError)
    End Sub

    Sub Session_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la sesión
    End Sub

    Sub Application_End(ByVal sender As Object, ByVal e As EventArgs)
        ' Se desencadena cuando finaliza la aplicación
    End Sub

#Region "Funciones y Procesos"
    Function UsuarioPerfil(IdUser As Integer) As MPWeb.Perfil
        UsuarioPerfil = MPWeb.Perfil.Consultor
        '-----------------------------------------------------
        'Determinamos de que estructuras obtenemos el perfil
        '-----------------------------------------------------
        Dim ListaIDs As New List(Of Integer) From {5}
        '-----------------------------------------------------
        Dim Usuario As New gtkEstructura

        Dim p1 = From Perfil As gtkEstructura In New gtkEstructura().Listado


        Dim Perfiles As List(Of gtkEstructura) = (From Perfil As gtkEstructura In New gtkEstructura().Listado
                                                  Where If(Perfil.IdIturria Is Nothing, Nothing, ListaIDs.Find(Function(i) i = Perfil.IdIturria))
                                                  Select Perfil).ToList
        For Each usr As gtkEstructura In Perfiles
            Usuario = If(usr.Nodos Is Nothing, Nothing, usr.Nodos.Find(Function(o) o.Descripcion = IdUser))
            If Usuario IsNot Nothing Then Exit For
        Next
        If Usuario Is Nothing Then
            UsuarioPerfil = MPWeb.Perfil.Consultor
        ElseIf Usuario.IdIturria = 6 Then
            UsuarioPerfil = MPWeb.Perfil.Administrador
        ElseIf Usuario.IdIturria = 7 Then
            UsuarioPerfil = MPWeb.Perfil.Usuario
        Else
            UsuarioPerfil = MPWeb.Perfil.Consultor
        End If
        Return UsuarioPerfil
    End Function
#End Region
End Class

Public Class UserControlBase
    Inherits System.Web.UI.UserControl

    Public ItzultzaileWeb As New LocalizationLib.Itzultzaile

End Class

Public Class gtkGridView
    ''' <summary>
    ''' Identificador del Registro Seleccionado.
    ''' </summary>
    Private _IdSeleccionado As Nullable(Of Integer)
    ''' <summary>
    ''' Nombre de la PROPIEDAD por la que se quiere ordenar los objetos.
    ''' </summary>
    Private _CampoOrdenacion As String
    ''' <summary>
    ''' Direccion de Ordenacion para el Campo de Ordenacion (Nombre de la Propiedad).
    ''' </summary>
    Private _DireccionOrdenacion As Nullable(Of System.ComponentModel.ListSortDirection)
    ''' <summary>
    ''' Indice de la página en curso.
    ''' </summary>
    Private _Pagina As Nullable(Of Integer)

    ''' <summary>
    ''' Identificador del Registro Seleccionado.
    ''' </summary>
    Public Property IdSeleccionado() As Nullable(Of Integer)
        Get
            Return _IdSeleccionado
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _IdSeleccionado = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre de la PROPIEDAD por la que se quiere ordenar los objetos.
    ''' </summary>
    Public Property CampoOrdenacion() As String
        Get
            Return _CampoOrdenacion
        End Get
        Set(ByVal value As String)
            _CampoOrdenacion = value
        End Set
    End Property
    ''' <summary>
    ''' Direccion de Ordenacion para el Campo de Ordenacion (Nombre de la Propiedad).
    ''' </summary>
    Public Property DireccionOrdenacion() As Nullable(Of System.ComponentModel.ListSortDirection)
        Get
            Return _DireccionOrdenacion
        End Get
        Set(ByVal value As Nullable(Of System.ComponentModel.ListSortDirection))
            _DireccionOrdenacion = value
        End Set
    End Property

    ''' <summary>
    ''' Indice de la página en curso.
    ''' </summary>
    Public Property Pagina As Nullable(Of Integer)
        Get
            Return _Pagina
        End Get
        Set(ByVal value As Nullable(Of Integer))
            _Pagina = value
        End Set
    End Property

End Class