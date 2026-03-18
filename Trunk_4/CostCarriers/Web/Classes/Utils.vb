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

#End Region

#Region "Menú navegacion"

    ''' <summary>
    ''' Genera el menú
    ''' </summary>
    ''' <param name="rolesUsuario"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GenerarMenu(ByVal rolesUsuario As List(Of ELL.UsuarioRol)) As String
        Dim menu As New StringBuilder()
        Dim xDoc As New System.Xml.XmlDocument()
        xDoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/menu.xml"))

        Dim root As XmlNodeList = xDoc.GetElementsByTagName("Root")
        Dim elementoRoot As XmlElement = CType(root(0), XmlElement)

        TratarElementosMenu(elementoRoot, menu, rolesUsuario, 0)

        Return menu.ToString()
    End Function

    ''' <summary>
    ''' Trata los nodos del mené de manera recursiva
    ''' </summary>
    ''' <param name="elemento"></param> 
    ''' <param name="menu"></param> 
    ''' <param name="rolesUsuario"></param>
    ''' <param name="nivel"></param>
    ''' <remarks></remarks>
    Private Shared Sub TratarElementosMenu(ByVal elemento As XmlElement, ByRef menu As StringBuilder, ByVal rolesUsuario As List(Of ELL.UsuarioRol), ByVal nivel As Integer)
        Dim existe As Boolean = True
        Dim roles As String() = Nothing
        Dim rolAux As String = String.Empty
        Dim nodoAux As XmlNode = Nothing
        Dim elementoAux As XmlElement = Nothing
        Dim urlHelper As New UrlHelper(HttpContext.Current.Request.RequestContext)
        Dim nodoTratado As Boolean = False

        If (elemento.ChildNodes.Count > 0) Then
            For Each nodo As XmlNode In elemento.ChildNodes
                elementoAux = CType(nodo, XmlElement)
                roles = elementoAux.GetAttribute("roles").Split(",")
                For Each rol As String In roles
                    rolAux = rol.Trim()

                    If (Not nodoTratado) Then
                        ' Si en el nodo los roles vienen vacios quiere decir que todos los roles tienes acceso
                        If (String.IsNullOrEmpty(rolAux) OrElse (Not String.IsNullOrEmpty(rolAux) AndAlso rolesUsuario.Exists(Function(f) f.IdRol = CInt(rolAux)))) Then
                            nodoTratado = True
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
                            Else
                                menu.Append("<li><a href='" & urlHelper.Action(elementoAux.GetAttribute("action"), elementoAux.GetAttribute("controller"), New With {.idManual = elementoAux.GetAttribute("idManual")}) & "'>" & Traducir(elementoAux.GetAttribute("title")) & "</a></li>")
                            End If
                        End If
                    End If
                Next
                nodoTratado = False
            Next
        Else
            If (String.IsNullOrEmpty(rolAux) OrElse (Not String.IsNullOrEmpty(rolAux) AndAlso rolesUsuario.Exists(Function(f) f.IdRol = CInt(rolAux)))) Then
                menu.Append("<li><a href='" & urlHelper.Action(elemento.GetAttribute("action"), elemento.GetAttribute("controller"), New With {.idManual = elementoAux.GetAttribute("idManual")}) & "'>" & Traducir(elemento.GetAttribute("title")) & "</a></li>")
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