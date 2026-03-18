Namespace ELL

    Public Class Matrici

#Region "Variables miembro"

        Public Shared MATRICI_ID_PLANTA As Integer = -1

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _apellidos As String = String.Empty
        Private _unidad As String = String.Empty
        Private _area As String = String.Empty
        Private _seccion As String = String.Empty
        Private _extFija As Integer = Integer.MinValue
        Private _fijo As String = String.Empty
        Private _extInalambrica As Integer = Integer.MinValue
        Private _inalambrico As String = String.Empty
        Private _extMovil As Integer = Integer.MinValue
        Private _movil As String = String.Empty
        Private _skype As Integer = Integer.MinValue

#End Region

#Region "Properties"

        Public Property Id As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        Public Property Nombre As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        Public Property Apellidos As String
            Get
                Return _apellidos
            End Get
            Set(ByVal value As String)
                _apellidos = value
            End Set
        End Property

        Public ReadOnly Property NombreCompleto As String
            Get
                Return Nombre & " " & Apellidos
            End Get
        End Property

        Public Property Unidad As String
            Get
                Return _unidad
            End Get
            Set(ByVal value As String)
                _unidad = value
            End Set
        End Property

        Public Property Area As String
            Get
                Return _area
            End Get
            Set(ByVal value As String)
                _area = value
            End Set
        End Property

        Public Property Seccion As String
            Get
                Return _seccion
            End Get
            Set(ByVal value As String)
                _seccion = value
            End Set
        End Property

        Public Property ExtFija As Integer
            Get
                Return _extFija
            End Get
            Set(ByVal value As Integer)
                _extFija = value
            End Set
        End Property

        Public Property Fijo As String
            Get
                Return _fijo
            End Get
            Set(ByVal value As String)
                _fijo = value
            End Set
        End Property

        Public Property ExtInalambrica As Integer
            Get
                Return _extInalambrica
            End Get
            Set(ByVal value As Integer)
                _extInalambrica = value
            End Set
        End Property

        Public Property Inalambrico As String
            Get
                Return _inalambrico
            End Get
            Set(ByVal value As String)
                _inalambrico = value
            End Set
        End Property

        Public Property ExtMovil As Integer
            Get
                Return _extMovil
            End Get
            Set(ByVal value As Integer)
                _extMovil = value
            End Set
        End Property

        Public Property Movil As String
            Get
                Return _movil
            End Get
            Set(ByVal value As String)
                _movil = value
            End Set
        End Property

        Public Property Skype As Integer
            Get
                Return _skype
            End Get
            Set(ByVal value As Integer)
                _skype = value
            End Set
        End Property

#End Region

    End Class

End Namespace
