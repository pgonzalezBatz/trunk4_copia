Namespace ELL

    Public Class UnidadOrg

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _idPlanta As Integer = Integer.MinValue
        Private _responsable As Sablib.ELL.Usuario = Nothing
        Private _reqProyCli As Boolean = False
        Private _reqConSinProyecto As Boolean = False
        Private _reqOFImproductiva As Boolean = False
        Private _reqOFValidar As Boolean = False
        Private _stringConexion As String = String.Empty
        Private _departIdentif As String = String.Empty
        Private _sistema As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador
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
        ''' Nombre de la unidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property

        ''' <summary>
        ''' Id de la planta a la que pertenece
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property

        ''' <summary>
        ''' Responsable de la unidad org
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Responsable() As Sablib.ELL.Usuario
            Get
                Return _responsable
            End Get
            Set(ByVal value As Sablib.ELL.Usuario)
                _responsable = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si requerira la seleccion de un proyecto y un cliente
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ReqProyCli As Boolean
            Get
                Return _reqProyCli
            End Get
            Set(ByVal value As Boolean)
                _reqProyCli = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si requerira una eleccion de con/sin proyecto
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ReqConSinProyecto As Boolean
            Get
                Return _reqConSinProyecto
            End Get
            Set(ByVal value As Boolean)
                _reqConSinProyecto = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si requerira una OF Improductiva
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ReqOFImproductiva As Boolean
            Get
                Return _reqOFImproductiva
            End Get
            Set(ByVal value As Boolean)
                _reqOFImproductiva = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si requerira el texto de una of para validar
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property ReqOFValidar As Boolean
            Get
                Return _reqOFValidar
            End Get
            Set(ByVal value As Boolean)
                _reqOFValidar = value
            End Set
        End Property

        ''' <summary>
        ''' String de conexion de la base de datos con la que habra que conectarse
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property StringConexion() As String
            Get
                Return _stringConexion
            End Get
            Set(ByVal value As String)
                _stringConexion = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador del departamento asociado a la unidad
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property DepartamentoIdentif() As String
            Get
                Return _departIdentif
            End Get
            Set(ByVal value As String)
                _departIdentif = value
            End Set
        End Property

        ''' <summary>
        ''' Sistema del que saca los datos
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Sistema() As String
            Get
                Return _sistema
            End Get
            Set(ByVal value As String)
                _sistema = value
            End Set
        End Property

#End Region

    End Class

End Namespace