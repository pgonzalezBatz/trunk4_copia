''' <summary>
''' Representa un nodo para el treeview
''' </summary>
Public Class TreeviewNode

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property text As String

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property state As New State()

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property icon As String = Nothing

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property backColor As String = Nothing

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property nodes As List(Of TreeviewNode) = Nothing

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property dataAttributes As DataAttribute = Nothing

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property selectable As Boolean = False

End Class

''' <summary>
''' Representa el estado de un nodo
''' </summary>
Public Class State

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property checked As Boolean = False

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property disabled As Boolean = False

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property expanded As Boolean = False

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property selected As Boolean = False

End Class

''' <summary>
''' Representa los atributos de datos de un nodo
''' </summary>
Public Class DataAttribute

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property id As Integer? = Integer.MinValue

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property kind As String = String.Empty

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Property idPlanta As Integer? = Integer.MinValue

End Class
