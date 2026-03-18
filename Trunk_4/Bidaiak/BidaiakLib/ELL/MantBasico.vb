Namespace ELL

    <Serializable()> _
    Public Class MantBasico

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _nombre As String = String.Empty
        Private _descripcion As String = String.Empty
        Private _obsoleto As Boolean = False
        Private _idPlanta As Integer = Integer.MinValue

#End Region

#Region "Properties"

        ''' <summary>
        ''' Identificador unico
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
        ''' Nombre del objeto
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
        ''' Descripcion del mismo
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Descripcion() As String
            Get
                Return _descripcion
            End Get
            Set(ByVal value As String)
                _descripcion = value
            End Set
        End Property

        ''' <summary>
        ''' Indica si esta obsoleto o no
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>		
        Public Property Obsoleto() As Boolean
            Get
                Return _obsoleto
            End Get
            Set(ByVal value As Boolean)
                _obsoleto = value
            End Set
        End Property

        ''' <summary>
        ''' Id de la planta 
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
        ''' Clona el objeto en uno nuevo
        ''' </summary>
        ''' <returns></returns>        
        Public Function Clone() As MantBasico
            Dim oMantNew As New MantBasico
            oMantNew.Id = Id
            oMantNew.Nombre = Nombre
            oMantNew.Descripcion = Descripcion
            oMantNew.Obsoleto = Obsoleto
            oMantNew.IdPlanta = IdPlanta
            Return oMantNew
        End Function

#End Region

    End Class

End Namespace