Imports System.ComponentModel

Public Class Data
    Private _key As String
    <DisplayName("Columna")>
    Public Property Key() As String
        Get
            Return _key
        End Get
        Set(ByVal value As String)
            _key = value
        End Set
    End Property
    Private _readerValue As String
    <DisplayName("Valor leído")>
    Public Property ReaderValue() As String
        Get
            Return _readerValue
        End Get
        Set(ByVal value As String)
            _readerValue = value
        End Set
    End Property
    Private _dbValue As String
    <DisplayName("Valor en BD")>
    Public Property DbValue() As String
        Get
            Return _dbValue
        End Get
        Set(ByVal value As String)
            _dbValue = value
        End Set
    End Property
    Private _status As String
    <DisplayName("Estado")>
    Public Property Status() As String
        Get
            Return _status
        End Get
        Set(ByVal value As String)
            _status = value
        End Set
    End Property
End Class
