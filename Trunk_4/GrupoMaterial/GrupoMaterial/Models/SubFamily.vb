Imports System.ComponentModel.DataAnnotations
Public Class SubFamily

    Private _id As String
    <Key>
    <Display(Name:="Subfamily Id")>
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
    <Display(Name:="Subfamily")>
    Public Property Name() As String
        Get
            Return _name
        End Get
        Set(ByVal value As String)
            _name = value
        End Set
    End Property

    Private _parent As String
    <Display(Name:="Family")>
    Public Property Parent() As String
        Get
            Return _parent
        End Get
        Set(ByVal value As String)
            _parent = value
        End Set
    End Property

    Private _parentID As String
    <Required(ErrorMessage:="Enter family")>
    <Display(Name:="Family Id")>
    Public Property ParentId() As String
        Get
            Return _parentID
        End Get
        Set(ByVal value As String)
            _parentID = value
        End Set
    End Property

    Private _grandparent As String
    <Display(Name:="Commodity")>
    Public Property Grandparent() As String
        Get
            Return _grandparent
        End Get
        Set(ByVal value As String)
            _grandparent = value
        End Set
    End Property
    Private _grandparentId As String
    <Required(ErrorMessage:="Enter comodity")>
    <Display(Name:="Comodity Id")>
    Public Property GrandparentId() As String
        Get
            Return _grandparentId
        End Get
        Set(ByVal value As String)
            _grandparentId = value
        End Set
    End Property
End Class
