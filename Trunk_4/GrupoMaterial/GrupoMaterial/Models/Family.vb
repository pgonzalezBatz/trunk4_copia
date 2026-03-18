Imports System.ComponentModel.DataAnnotations
Public Class Family

    Private _id As String
    <Key>
    <Display(Name:="Family Id")>
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
    <Display(Name:="Family")>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
    Private _parent As String
    <Required(ErrorMessage:="Choose a comodity")>
    <Display(Name:="Comodity")>
    Public Property Parent() As String
        Get
            Return _parent
        End Get
        Set(ByVal value As String)
            _parent = value
        End Set
    End Property

    Private _parentID As String
    <Display(Name:="Comodity Id")>
    Public Property ParentId() As String
        Get
            Return _parentID
        End Get
        Set(ByVal value As String)
            _parentID = value
        End Set
    End Property
End Class
