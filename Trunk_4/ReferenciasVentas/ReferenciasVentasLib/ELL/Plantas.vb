Namespace ELL

    Public Class Plantas

#Region "Variables miembro"

        Private _id As Integer = Integer.MinValue
        Private _codigo As String = String.Empty
        Private _nombre As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
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
        ''' Código
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Codigo() As String
            Get
                Return _codigo
            End Get
            Set(ByVal value As String)
                _codigo = value
            End Set
        End Property

        ''' <summary>
        ''' Nombre
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

#End Region

    End Class

End Namespace

