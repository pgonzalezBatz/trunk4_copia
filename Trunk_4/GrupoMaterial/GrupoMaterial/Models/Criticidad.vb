Imports System.ComponentModel.DataAnnotations
Public Class Criticidad
    Private _name As String
    '<Required(ErrorMessage:="Enter name")>
    '<Display(Name:="Comodity")>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
    Private _desc As String
    '<Required(ErrorMessage:="Enter descipción")>
    '<Display(Name:="Comodity")>
    Public Property desc() As String
        Get
            Return _desc
        End Get
        Set(ByVal value As String)
            _desc = value
        End Set
    End Property

    Private _code As String
    Public Property Code() As String
        Get
            Return _code
        End Get
        Set(ByVal value As String)
            _code = value
        End Set
    End Property


End Class
