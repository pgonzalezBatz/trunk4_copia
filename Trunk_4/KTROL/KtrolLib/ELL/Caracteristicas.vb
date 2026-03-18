Namespace ELL

    Public Class Caracteristicas

#Region "Variables miembro"

        Private _idRegistro As String = String.Empty
        Private _caracteristica As String = String.Empty
        Private _tipo As String = String.Empty
        Private _valor As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador del registro
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property IdRegistro() As String
            Get
                Return _idRegistro
            End Get
            Set(ByVal value As String)
                _idRegistro = value
            End Set
        End Property

        ''' <summary>
        ''' Especificación de la característica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Caracteristica() As String
            Get
                Return _caracteristica
            End Get
            Set(ByVal value As String)
                _caracteristica = value
            End Set
        End Property

        ''' <summary>
        ''' Tipo de la característica
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Tipo() As String
            Get
                Return _tipo
            End Get
            Set(ByVal value As String)
                _tipo = value
            End Set
        End Property

        ''' <summary>
        ''' Identificador del usuario
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Valor() As String
            Get
                Return _valor
            End Get
            Set(ByVal value As String)
                _valor = value
            End Set
        End Property

#End Region

    End Class

End Namespace

