Imports System.ComponentModel.DataAnnotations

Public Class Element
    Private _id As String
    <Key>
    <Display(Name:="Element Id")>
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
    <Display(Name:="Element")>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
    Private _parentId As String
    <Required(ErrorMessage:="Enter parent")>
    <Display(Name:="Subfamily Id")>
    Public Property ParentId() As String
        Get
            Return _parentId
        End Get
        Set(ByVal value As String)
            _parentId = value
        End Set
    End Property
    Private _parent As String
    <Display(Name:="Subfamily")>
    Public Property Parent() As String
        Get
            Return _parent
        End Get
        Set(ByVal value As String)
            _parent = value
        End Set
    End Property
End Class
