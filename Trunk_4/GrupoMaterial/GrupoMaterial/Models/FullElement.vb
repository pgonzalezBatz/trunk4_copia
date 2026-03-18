Imports System.ComponentModel.DataAnnotations

Public Class FullElement
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
    <Required(ErrorMessage:="Enter element name")>
    <Display(Name:="Element")>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property
    Private _parent As String
    <Required(ErrorMessage:="Enter subfamily")>
    <Display(Name:="Subfamily")>
    Public Property Parent() As String
        Get
            Return _parent
        End Get
        Set(ByVal value As String)
            _parent = value
        End Set
    End Property

    Private _parentId As String
    <Display(Name:="Subfamily Id")>
    Public Property ParentId() As String
        Get
            Return _parentId
        End Get
        Set(ByVal value As String)
            _parentId = value
        End Set
    End Property

    Private _grandparent As String
    <Required(ErrorMessage:="Enter family")>
    <Display(Name:="Family")>
    Public Property Grandparent() As String
        Get
            Return _grandparent
        End Get
        Set(ByVal value As String)
            _grandparent = value
        End Set
    End Property

    Private _grandparentId As String
    <Display(Name:="Family Id")>
    Public Property GrandparentId() As String
        Get
            Return _grandparentId
        End Get
        Set(ByVal value As String)
            _grandparentId = value
        End Set
    End Property

    Private _grandgrandparent As String
    <Display(Name:="Comodity")>
    Public Property Grandgrandparent() As String
        Get
            Return _grandgrandparent
        End Get
        Set(ByVal value As String)
            _grandgrandparent = value
        End Set
    End Property

    Private _grandgrandparentId As String
    <Required(ErrorMessage:="Enter comodity")>
    <Display(Name:="Comodity Id")>
    Public Property GrandgrandparentId() As String
        Get
            Return _grandgrandparentId
        End Get
        Set(ByVal value As String)
            _grandgrandparentId = value
        End Set
    End Property

End Class
