Namespace ELL

    Public Class IncidenciasFicheros

#Region "Variables miembro"

        Private _IdIncidencia As Integer
        Private _NombreFichero As String
        Private _Fichero As Byte()

#End Region

#Region "Properties"

        Public Property IdIncidencia() As Integer
            Get
                Return _IdIncidencia
            End Get
            Set(ByVal value As Integer)
                _IdIncidencia = value
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

#End Region

    End Class

End Namespace

