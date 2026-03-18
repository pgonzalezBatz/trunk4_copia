Namespace ELL

    Public Class TelefonoExtension

        Private _nombre As String = String.Empty
        Private _extInterna As Integer = Integer.MinValue
        Private _extMovil As Integer = Integer.MinValue
        Private _tlfnoDirecto As String = String.Empty
        Private _tlfnoMovil As String = String.Empty
        Private _idDepartamento As String = String.Empty
        Private _departamento As String = String.Empty
        Private _idPlanta As Integer = Integer.MinValue
        Private _planta As String = String.Empty
        Private _visible As Boolean = False
        Private _comentario As String = String.Empty
        Private _idSab As Integer = Integer.MinValue
        Private _idTipoLinea As Integer = Integer.MinValue

#Region "Constante"

        Public Shared EXTENSION_DECT As Integer = 23
        Public Shared EXTENSION_ZOIPER As Integer = 86

#End Region

        ''' <summary>
        ''' Nombre de la persona o del departamento
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Nombre() As String
            Get
                Return _nombre
            End Get
            Set(ByVal value As String)
                _nombre = value
            End Set
        End Property


        Public Property ExtensionInterna() As Integer
            Get
                Return _extInterna
            End Get
            Set(ByVal value As Integer)
                _extInterna = value
            End Set
        End Property


        Public Property ExtensionMovil() As Integer
            Get
                Return _extMovil
            End Get
            Set(ByVal value As Integer)
                _extMovil = value
            End Set
        End Property


        Public Property TlfnoDirecto() As String
            Get
                Return _tlfnoDirecto
            End Get
            Set(ByVal value As String)
                _tlfnoDirecto = value
            End Set
        End Property


        Public Property TlfnoMovil() As String
            Get
                Return _tlfnoMovil
            End Get
            Set(ByVal value As String)
                _tlfnoMovil = value
            End Set
        End Property

        Public Property IdDepartamento() As String
            Get
                Return _idDepartamento
            End Get
            Set(ByVal value As String)
                _idDepartamento = value
            End Set
        End Property

        Public Property IdTipoLinea() As Integer
            Get
                Return _idTipoLinea
            End Get
            Set(ByVal value As Integer)
                _idTipoLinea = value
            End Set
        End Property


        Public Property Departamento() As String
            Get
                Return _departamento
            End Get
            Set(ByVal value As String)
                _departamento = value
            End Set
        End Property

        Public Property IdPlanta() As Integer
            Get
                Return _idPlanta
            End Get
            Set(ByVal value As Integer)
                _idPlanta = value
            End Set
        End Property


        Public Property Planta() As String
            Get
                Return _planta
            End Get
            Set(ByVal value As String)
                _planta = value
            End Set
        End Property


        ''' <summary>
        ''' Indica si la extension es visible
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Property Visible() As Boolean
            Get
                Return _visible
            End Get
            Set(ByVal value As Boolean)
                _visible = value
            End Set
        End Property

        Public Property Comentarios() As String
            Get
                Return _comentario
            End Get
            Set(ByVal value As String)
                _comentario = value
            End Set
        End Property

        Public Property idSab() As Integer
            Get
                Return _idSab
            End Get
            Set(ByVal value As Integer)
                _idSab = value
            End Set
        End Property

        ''' <summary>
        ''' Devuelve la extension interna cuando su tipo sea IP, Analogica, Digital, etc...
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property ExtFija() As String
            Get
                Dim returnValue As String = String.Empty
                If (_idTipoLinea <> EXTENSION_DECT And _idTipoLinea <> EXTENSION_ZOIPER) Then returnValue = ExtensionInterna

                Return returnValue
            End Get
        End Property

        ''' <summary>
        ''' Devuelve el numero directo cuando el tipo de la extension interna sea IP, Analogica, Digital, etc...
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Fijo As String
            Get
                Dim returnValue As String = String.Empty
                If (_idTipoLinea <> EXTENSION_DECT And _idTipoLinea <> EXTENSION_ZOIPER) Then returnValue = TlfnoDirecto

                Return returnValue
            End Get
        End Property

        ''' <summary>
        ''' Devuelve la extension interna cuando su tipo sea DECT
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property ExtInalambrica
            Get
                Dim returnValue As String = String.Empty
                If (_idTipoLinea = EXTENSION_DECT) Then returnValue = ExtensionInterna

                Return returnValue
            End Get
        End Property

        ''' <summary>
        ''' Devuelve el numero directo cuando el tipo de la extension interna sea DECT
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>    
        Public ReadOnly Property Inalambrico
            Get
                Dim returnValue As String = String.Empty
                If (_idTipoLinea = EXTENSION_DECT) Then returnValue = TlfnoDirecto

                Return returnValue
            End Get
        End Property

        ''' <summary>
        '''  Devuelve la extension cuando su tipo sea Zoiper...
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Zoiper
            Get
                Dim returnValue As String = String.Empty
                If (_idTipoLinea = EXTENSION_ZOIPER) Then returnValue = ExtensionInterna

                Return returnValue
            End Get
        End Property

#Region "Property Names"

        ''' <summary>
        ''' Clase para definir los nombres de las propiedades de la clase
        ''' </summary>
        ''' <remarks></remarks>
        Public Class PropertyNames
            Private Const _TLFNO_MOVIL As String = "TlfnoMovil"
            Private Const _EXTENSION_MOVIL As String = "ExtensionMovil"

            Public Shared ReadOnly Property TLFNO_MOVIL() As String
                Get
                    Return _TLFNO_MOVIL
                End Get
            End Property

            Public Shared ReadOnly Property EXTENSION_MOVIL() As String
                Get
                    Return _EXTENSION_MOVIL
                End Get
            End Property

        End Class

#End Region

    End Class

End Namespace
