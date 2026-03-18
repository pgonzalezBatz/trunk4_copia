Imports System.ComponentModel.DataAnnotations
Public Class Comodity

    Private _id As String
    <Key>
    <Display(Name:="Comodity Id")>
    Public Property Id() As String
        Get
            Return _id
        End Get
        Set(ByVal value As String)
            _id = value
        End Set
    End Property

    Private _name As String
    <Required(ErrorMessage:="Enter name")>
    <Display(Name:="Comodity")>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
End Class
