Namespace ELL

    Public Class Ficheros

#Region "Variables miembro"

        Private _IdControl As Integer
        Private _IdRegistro As String
        Private _NombreFichero As String
        Private _Fichero As Byte() = Nothing
        Private _Fecha As Date = Date.MinValue

#End Region

#Region "Properties"

        Public Property IdControl() As Integer
            Get
                Return _IdControl
            End Get
            Set(ByVal value As Integer)
                _IdControl = value
            End Set
        End Property

        Public Property IdRegistro() As String
            Get
                Return _IdRegistro
            End Get
            Set(ByVal value As String)
                _IdRegistro = value
            End Set
        End Property

        Public Property NombreFichero() As String
            Get
                Return _NombreFichero
            End Get
            Set(ByVal value As String)
                _NombreFichero = value
            End Set
        End Property

        Public Property Fichero() As Byte()
            Get
                Return _Fichero
            End Get
            Set(ByVal value As Byte())
                _Fichero = value
            End Set
        End Property

        Public Property Fecha() As Date
            Get
                Return _Fecha
            End Get
            Set(ByVal value As Date)
                _Fecha = value
            End Set
        End Property

#End Region

    End Class

End Namespace

