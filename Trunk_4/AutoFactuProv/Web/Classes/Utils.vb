Imports System.Web.Mvc
Imports System.Xml

''' <summary>
''' 
''' </summary>
Public Class Utils

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
    ''' <param name="idSab"></param> 
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GenerarMenu(ByVal idSab As Integer) As String
        Dim menu As New StringBuilder()
        Dim xDoc As New System.Xml.XmlDocument()
        xDoc.Load(HttpContext.Current.Server.MapPath("~/App_Data/menu.xml"))

        Dim root As XmlNodeList = xDoc.GetElementsByTagName("Root")
        Dim elementoRoot As XmlElement = CType(root(0), XmlElement)

        TratarElementosMenu(elementoRoot, menu, 0)

        Return menu.ToString()
    End Function

    ''' <summary>
    ''' Trata los nodos del mené de manera recursiva
    ''' </summary>
    ''' <param name="elemento"></param> 
    ''' <param name="menu"></param> 
    ''' <param name="nivel"></param>
    ''' <remarks></remarks>
    Private Shared Sub TratarElementosMenu(ByVal elemento As XmlElement, ByRef menu As StringBuilder, ByVal nivel As Integer)
        Dim existe As Boolean = True
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
                    If (String.IsNullOrEmpty(rolAux)) Then
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

                            TratarElementosMenu(elementoAux, menu, nivel + 1)

                            menu.Append("</ul>")
                            menu.Append("</li>")
                        Else
                            menu.Append("<li><a href='" & urlHelper.Action(elementoAux.GetAttribute("action"), elementoAux.GetAttribute("controller"), New With {.Tipo = elementoAux.GetAttribute("tipo"), .Origen = elementoAux.GetAttribute("origen")}) & "'>" & Traducir(elementoAux.GetAttribute("title")) & "</a></li>")
                        End If
                    End If
                Next
            Next
        Else
            If (String.IsNullOrEmpty(rolAux)) Then
                menu.Append("<li><a href='" & urlHelper.Action(elemento.GetAttribute("action"), elemento.GetAttribute("controller"), New With {.Tipo = elementoAux.GetAttribute("tipo"), .Origen = elementoAux.GetAttribute("origen")}) & "'>" & Traducir(elemento.GetAttribute("title")) & "</a></li>")
            End If
        End If
    End Sub

#End Region

End Class

''' <summary>
''' Clase que herada de Albaran
''' </summary>
Public Class AlbaranExt
    Inherits ELL.Albaran

    ''' <summary>
    ''' Indica si está marcado o no
    ''' </summary>
    ''' <returns></returns>
    Public Property Checked As Boolean

End Class

''' <summary>
''' Clase TicketExt
''' </summary>
Public Class TicketExt

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property Ticket As SabLib.ELL.Ticket

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property Proveedor As Integer

    ''' <summary>
    ''' Id empresa en sistemas
    ''' </summary>
    ''' <returns></returns>
    Public Property Empresa As String

    ''' <summary>
    ''' Id planta en sistemas
    ''' </summary>
    Public ReadOnly Property Planta As String
        Get
            Return "000" 'No hay manera de saber la planta en BRAIN
        End Get
    End Property

    Public Sub New(ByVal ticketSAB As SabLib.ELL.Ticket)
        Ticket = ticketSAB
    End Sub

End Class