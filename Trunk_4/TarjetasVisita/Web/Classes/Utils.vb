Imports System.Web.Mvc
Imports System.Xml
Imports System.IO

''' <summary>
''' 
''' </summary>
Public Class Utils

#Region "Traducción"

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="key"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Traducir(ByVal key As String) As String
        Return AccesoGenerico.GetTerminoStatic(key, Threading.Thread.CurrentThread.CurrentCulture.Name, ConfigurationManager.AppSettings("LocalPath"))
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="key"></param>
    ''' <param name="cultureName"></param>
    ''' <returns></returns>
    Public Shared Function Traducir(ByVal key As String, ByVal cultureName As String) As String
        Return AccesoGenerico.GetTerminoStatic(key, cultureName, ConfigurationManager.AppSettings("LocalPath"))
    End Function

#End Region

#Region "Menú navegacion"

    ''' <summary>
    ''' 
    ''' </summary>
    Public Enum ElementoMenuNav
        GestionPermisos = 1
    End Enum


    ''' <summary>
    ''' Genera el menú
    ''' </summary>
    ''' <param name="rolesUsuario"></param>
    ''' <param name="idUser"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GenerarMenu(ByVal rolesUsuario As List(Of TarjetasVisitaLib.ELL.UsuarioRol), ByVal idUser As Integer) As String
        Dim menu As New StringBuilder()
        Dim xDoc As New System.Xml.XmlDocument()
        xDoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/menu.xml"))

        Dim root As XmlNodeList = xDoc.GetElementsByTagName("Root")
        Dim elementoRoot As XmlElement = CType(root(0), XmlElement)

        TratarElementosMenu(elementoRoot, menu, rolesUsuario, 0, idUser)

        Return menu.ToString()
    End Function

    ''' <summary>
    ''' Trata los nodos del mené de manera recursiva
    ''' </summary>
    ''' <param name="elemento"></param> 
    ''' <param name="menu"></param> 
    ''' <param name="rolesUsuario"></param>
    ''' <param name="nivel"></param>
    ''' <param name="idUser"></param>
    ''' <remarks></remarks>
    Private Shared Sub TratarElementosMenu(ByVal elemento As XmlElement, ByRef menu As StringBuilder, ByVal rolesUsuario As List(Of TarjetasVisitaLib.ELL.UsuarioRol), ByVal nivel As Integer, ByVal idUser As Integer)
        Dim existe As Boolean = True
        Dim roles As String() = Nothing
        Dim id As String = String.Empty
        Dim rolAux As String = String.Empty
        Dim nodoAux As XmlNode = Nothing
        Dim elementoAux As XmlElement = Nothing
        Dim urlHelper As New UrlHelper(HttpContext.Current.Request.RequestContext)
        Dim cargarPuntoMenu As Boolean = True

        If (elemento.ChildNodes.Count > 0) Then
            For Each nodo As XmlNode In elemento.ChildNodes
                elementoAux = CType(nodo, XmlElement)
                roles = elementoAux.GetAttribute("roles").Split(",")
                id = elementoAux.GetAttribute("id")
                For Each rol As String In roles
                    rolAux = rol.Trim()

                    ' Si en el nodo los roles vienen vacios quiere decir que todos los roles tienes acceso
                    If (String.IsNullOrEmpty(rolAux) OrElse (Not String.IsNullOrEmpty(rolAux) AndAlso rolesUsuario.Exists(Function(f) f.IdRol = CInt(rolAux)))) Then
                        cargarPuntoMenu = True
                        ' Hay algunos nodos especiales como el de gestion de permisos que su visibilidad depende de valores de la BBDD
                        If (Not String.IsNullOrEmpty(id) AndAlso CInt(id) = ElementoMenuNav.GestionPermisos) Then
                            cargarPuntoMenu = PermisoBLL.CargarListadoAutorizar(idUser).Count > 0
                        End If

                        If (cargarPuntoMenu) Then
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

                                TratarElementosMenu(elementoAux, menu, rolesUsuario, nivel + 1, idUser)

                                menu.Append("</ul>")
                                menu.Append("</li>")
                                Exit For
                            Else
                                menu.Append("<li><a href='" & HttpUtility.UrlDecode(urlHelper.Action(elementoAux.GetAttribute("action"), elementoAux.GetAttribute("controller"), Nothing)) & "'>" & Traducir(elementoAux.GetAttribute("title")) & "</a></li>")
                                Exit For
                            End If
                        End If
                    End If
                Next
            Next
        Else
            If (String.IsNullOrEmpty(rolAux) OrElse (Not String.IsNullOrEmpty(rolAux) AndAlso rolesUsuario.Exists(Function(f) f.IdRol = CInt(rolAux)))) Then
                menu.Append("<li><a href='" & HttpUtility.UrlDecode(urlHelper.Action(elemento.GetAttribute("action"), elemento.GetAttribute("controller"), Nothing)) & "'>" & Traducir(elemento.GetAttribute("title")) & "</a></li>")
            End If
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