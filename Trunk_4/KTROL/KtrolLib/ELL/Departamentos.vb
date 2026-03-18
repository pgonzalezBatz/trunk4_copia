Namespace ELL

    Public Class Departamentos

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _codDpto As String = String.Empty
        Private _RutaAcceso As String = String.Empty
        Private _deptEsCalidad As Boolean = False
        Private _deptEsOperario As Boolean = False
        Private _IdPlanta As Integer = Integer.MinValue

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer
            Get
                Return _id
            End Get
            Set(ByVal value As Integer)
                _id = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String
            Get
                Return If(String.IsNullOrEmpty(_nombre), String.Empty, _nombre.Trim())
            End Get
            Set(ByVal value As String)
                _nombre = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
        End Property

        ''' <summary>
        ''' Código del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property CodigoDpto() As String
            Get
                Return If(String.IsNullOrEmpty(_codDpto), String.Empty, _codDpto.Trim())
            End Get
            Set(ByVal value As String)
                _codDpto = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
        End Property

        ''' <summary>
        ''' Página de inicio del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property RutaAcceso() As String
            Get
                Return If(String.IsNullOrEmpty(_RutaAcceso), String.Empty, _RutaAcceso.Trim())
            End Get
            Set(ByVal value As String)
                _RutaAcceso = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
        End Property

        ''' <summary>
        ''' El departamento es de calidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DptoEsCalidad() As Boolean
            Get
                Return _deptEsCalidad
            End Get
            Set(ByVal value As Boolean)
                _deptEsCalidad = value
            End Set
        End Property

        ''' <summary>
        ''' El departamento es de operarios
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DptoEsOperario() As Boolean
            Get
                Return _deptEsOperario
            End Get
            Set(ByVal value As Boolean)
                _deptEsOperario = value
            End Set
        End Property

        ''' <summary>
        ''' La planta del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdPlanta() As Integer
            Get
                Return _IdPlanta
            End Get
            Set(ByVal value As Integer)
                _IdPlanta = value
            End Set
        End Property
#End Region

    End Class

End Namespace

