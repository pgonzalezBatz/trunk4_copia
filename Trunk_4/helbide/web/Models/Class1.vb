Imports System.ComponentModel
Imports System.ComponentModel.DataAnnotations
Public Class Helbide
    Private idValue As Integer
    Private calleValue As String
    Private codigoPostalValue As String
    Private poblacionValue As String
    Private provinciaValue As String
    Private paisValue As String

    Public Property Id As Integer
        Get
            Return idValue
        End Get
        Set(ByVal value As Integer)
            idValue = value
        End Set
    End Property
    <Required()> _
    Public Property Calle As String
        Get
            Return calleValue
        End Get
        Set(ByVal value As String)
            calleValue = value
        End Set
    End Property
    <Required()> _
    Public Property CodigoPostal As String
        Get
            Return codigoPostalValue
        End Get
        Set(ByVal value As String)
            codigoPostalValue = value
        End Set
    End Property
    <Required()> _
    Public Property Poblacion As String
        Get
            Return poblacionValue
        End Get
        Set(ByVal value As String)
            poblacionValue = value
        End Set
    End Property
    Public Property Provincia As String
        Get
            Return provinciaValue
        End Get
        Set(ByVal value As String)
            provinciaValue = value
        End Set
    End Property
    <Required()> _
    Public Property Pais As String
        Get
            Return paisValue
        End Get
        Set(ByVal value As String)
            paisValue = value
        End Set
    End Property
End Class
