Namespace ELL
    Public Class Empresa

#Region "VARIABLES MIEMBRO"
        Private _id As Integer
        Private _nombre As String
        Private _cif As String
        Private _direccion As String
        Private _telefono As String
        Private _fechaAlta As DateTime
        Private _fechaBaja As DateTime
        Private _idTroqueleria As String
        Private _idSistemas As Integer
#End Region

#Region "CONSTRUCTOR"
        Public Sub New()
            _id = 0
            _nombre = ""
            _cif = ""
            _direccion = ""
            _telefono = ""
            _fechaAlta = DateTime.MinValue
            _fechaBaja = DateTime.MinValue
            _idTroqueleria = ""
            _idSistemas = 0
        End Sub
#End Region

#Region "PROPERTIES"
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

        Public Property Cif() As String
            Get
                Return _cif
            End Get
            Set(ByVal value As String)
                _cif = value
            End Set
        End Property

        Public Property Direccion() As String
            Get
                Return _direccion
            End Get
            Set(ByVal value As String)
                _direccion = value
            End Set
        End Property


        Public Property Telefono() As String
            Get
                Return _telefono
            End Get
            Set(ByVal value As String)
                _telefono = value
            End Set
        End Property


        Public Property FechaAlta() As DateTime
            Get
                Return _fechaAlta
            End Get
            Set(ByVal value As DateTime)
                _fechaAlta = value
            End Set
        End Property


        Public Property FechaBaja() As DateTime
            Get
                Return _fechaBaja
            End Get
            Set(ByVal value As DateTime)
                _fechaBaja = value
            End Set
        End Property



        Public Property IdTroqueleria() As String
            Get
                Return _idTroqueleria
            End Get
            Set(ByVal value As String)
                _idTroqueleria = value
            End Set
        End Property

        Public Property IdSistemas() As Integer
            Get
                Return _idSistemas
            End Get
            Set(ByVal value As Integer)
                _idSistemas = value
            End Set
        End Property
#End Region

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

