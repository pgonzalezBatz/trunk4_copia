Namespace ELL

    <Serializable()> _
    Public Class Planta
        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _descripcion As String = String.Empty
        Private _obsoleto As Boolean = False
        Private _dominio As String = String.Empty


        Public Sub New()
        End Sub

        Public Sub New(ByVal idPlanta As Integer)
            _id = idPlanta
        End Sub

        Public Sub New(ByVal idPlanta As Integer, ByVal planta As String)
            _id = idPlanta
            _nombre = planta
        End Sub

        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property


        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        Public Property Dominio() As String
            Get
                Return _dominio
            End Get
            Set(ByVal value As String)
                _dominio = value
            End Set
        End Property

        Public Property Obsoleto() As Boolean
            Get
                Return _obsoleto
            End Get
            Set(ByVal value As Boolean)
                _obsoleto = False
            End Set
        End Property

#Region "Columns Names"

        Public Class ColumnNames
            Private Const _ID As String = "Id"
            Private Const _NOMBRE As String = "Nombre"            

            Public Shared ReadOnly Property ID() As String
                Get
                    Return _ID
                End Get
            End Property

            Public Shared ReadOnly Property NOMBRE() As String
                Get
                    Return _NOMBRE
                End Get
            End Property

        End Class

#End Region

    End Class

End Namespace
