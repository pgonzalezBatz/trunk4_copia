Imports System.IO
Imports System.Web.Mvc
Imports System.Xml

''' <summary>
''' 
''' </summary>
Public Class Utils

    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root")

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Traducir(ByVal key As String) As String
        Return AccesoGenerico.GetTerminoStatic(key, Threading.Thread.CurrentThread.CurrentCulture.Name, ConfigurationManager.AppSettings("LocalPath"))
    End Function

#Region "Menú navegacion"

    ''' <summary>
    ''' Genera el menú
    ''' </summary>
    ''' <param name="regenerarMenu"></param>
    ''' <param name="cargaInicial"></param>
    ''' <param name="idUser"></param>
    ''' <param name="rolActual"></param>
    ''' <returns></returns>
    Public Shared Function GenerarMenu(ByVal regenerarMenu As Boolean, ByVal cargaInicial As Boolean, ByVal idUser As Integer, ByVal rolActual As ELL.UsuarioRol) As String
        Dim menuString = Runtime.Caching.MemoryCache.Default("menu")

        Try
            Dim menu As New StringBuilder()
            Dim usuariosRoles As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(idUser)

            If (cargaInicial) Then
                If (usuariosRoles.Where(Function(f) f.IdRol <> ELL.Rol.RolUsuario.Administrador).Count >= 1) Then
                    ' Si el usuario tiene más de un rol hay que áñadir el punto de menú para poder cambiarlo en caliente
                    Dim urlHelper As New UrlHelper(HttpContext.Current.Request.RequestContext)
                    menu.Append("<li><a href='" & urlHelper.Action("Index", "Login", New With {.seleccionarRol = True}) & "'>" & Traducir("Cambiar planta") & "</a></li>")
                    menuString = menu.ToString()
                End If
            ElseIf (menuString Is Nothing OrElse regenerarMenu) Then
                Dim xDoc As New System.Xml.XmlDocument()
                xDoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/menu.xml"))

                Dim root As XmlNodeList = xDoc.GetElementsByTagName("Root")
                Dim elementoRoot As XmlElement = CType(root(0), XmlElement)
                Dim rolesUsuario As New List(Of ELL.UsuarioRol)

                If (rolActual IsNot Nothing) Then
                    rolesUsuario.Add(rolActual)
                End If

                If (usuariosRoles.Exists(Function(f) f.IdRol = ELL.Rol.RolUsuario.Administrador)) Then
                    rolesUsuario.Add(usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.RolUsuario.Administrador))
                End If

                TratarElementosMenu(elementoRoot, menu, rolesUsuario, 0)

                If (usuariosRoles.Where(Function(f) f.IdRol <> ELL.Rol.RolUsuario.Administrador).Count > 1) Then
                    ' Si el usuario tiene más de un rol hay que áñadir el punto de menú para poder cambiarlo en caliente
                    Dim urlHelper As New UrlHelper(HttpContext.Current.Request.RequestContext)
                    menu.Append("<li><a href='" & urlHelper.Action("Index", "Login", New With {.seleccionarRol = True}) & "'>" & Traducir("Cambiar planta") & "</a></li>")
                    'menuString = menu.ToString()
                End If

                menuString = menu.ToString()
            End If

            Runtime.Caching.MemoryCache.Default("menu") = menuString
        Catch ex As Exception
            log.Error("Error al generar menu", ex)
        End Try

        Return menuString
    End Function

    ''' <summary>
    ''' Trata los nodos del mené de manera recursiva
    ''' </summary>
    ''' <param name="elemento"></param> 
    ''' <param name="menu"></param> 
    ''' <param name="rolesUsuario"></param>
    ''' <remarks></remarks>
    Private Shared Sub TratarElementosMenu(ByVal elemento As XmlElement, ByRef menu As StringBuilder, ByVal rolesUsuario As List(Of ELL.UsuarioRol), ByVal nivel As Integer)
        Dim roles As String() = Nothing
        Dim rolAux As String = String.Empty
        Dim nodoAux As XmlNode = Nothing
        Dim elementoAux As XmlElement = Nothing
        Dim urlHelper As New UrlHelper(HttpContext.Current.Request.RequestContext)

        If (elemento.ChildNodes.Count > 0) Then
            For Each nodo As XmlNode In elemento.ChildNodes
                elementoAux = CType(nodo, XmlElement)
                roles = elementoAux.GetAttribute("roles").Split(",")
                For Each rol As String In roles
                    rolAux = rol.Trim()

                    ' Si en el nodo los roles vienen vacios quiere decir que todos los roles tienes acceso
                    If (String.IsNullOrEmpty(rolAux) OrElse (Not String.IsNullOrEmpty(rolAux) AndAlso rolesUsuario.Exists(Function(f) f.IdRol = CInt(rolAux)))) Then
                        If (elementoAux.ChildNodes.Count > 0) Then
                            If (nivel = 0) Then
                                menu.Append("<li>")
                                menu.Append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" & Traducir(elementoAux.GetAttribute("title")) & "<b Class='caret'></b></a>")
                                menu.Append("<ul Class='dropdown-menu multi-level'>")
                            Else
                                menu.Append("<li class='dropdown-submenu'>")
                                menu.Append("<a href='#' class='dropdown-toggle' data-toggle='dropdown'>" & Traducir(elementoAux.GetAttribute("title")) & "</a>")
                                menu.Append("<ul class='dropdown-menu'>")
                            End If

                            TratarElementosMenu(elementoAux, menu, rolesUsuario, nivel + 1)

                            menu.Append("</ul>")
                            menu.Append("</li>")
                            Exit For
                        Else
                            menu.Append("<li><a href='" & urlHelper.Action(elementoAux.GetAttribute("action"), elementoAux.GetAttribute("controller")) & "'>" & Traducir(elementoAux.GetAttribute("title")) & "</a></li>")
                            Exit For
                        End If
                    End If
                Next
            Next
        Else
            roles = elemento.GetAttribute("roles").Split(",")
            For Each rol As String In roles
                rolAux = rol.Trim()

                If (String.IsNullOrEmpty(rolAux) OrElse (Not String.IsNullOrEmpty(rolAux) AndAlso rolesUsuario.Exists(Function(f) f.IdRol = CInt(rolAux)))) Then
                    menu.Append("<li><a href='" & urlHelper.Action(elemento.GetAttribute("action"), elemento.GetAttribute("controller")) & "'>" & Traducir(elemento.GetAttribute("title")) & "</a></li>")
                End If
            Next
        End If

    End Sub

    ''' <summary>
    ''' Lee un fichero de texto y devuelve su contenido
    ''' </summary>txtFechaFactura
    ''' <param name="ruta"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Friend Shared Function LeerFicheroTexto(ByVal ruta As String) As String
        Dim fp As StreamReader = Nothing
        Dim texto As String = String.Empty
        Try
            If (File.Exists(ruta)) Then
                fp = File.OpenText(ruta)
                texto = fp.ReadToEnd()
            End If
        Catch
        Finally
            If (fp IsNot Nothing) Then
                fp.Close()
                fp.Dispose()
            End If
        End Try

        Return texto
    End Function

#End Region

End Class