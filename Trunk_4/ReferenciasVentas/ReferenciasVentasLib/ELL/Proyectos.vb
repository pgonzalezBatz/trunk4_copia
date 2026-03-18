Namespace ELL

    Public Class Proyectos

#Region "Variables miembro"

        Private _id As String = String.Empty
        Private _nombre As String = String.Empty
        Private _programa As String = String.Empty

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As String
            Get
                Return _id
            End Get
            Set(ByVal value As String)
                _id = value
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

        ''' <summary>
        ''' Programa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Programa() As String
            Get
                Return If(String.IsNullOrEmpty(_programa), String.Empty, _programa.Trim())
            End Get
            Set(ByVal value As String)
                _programa = If(String.IsNullOrEmpty(value), String.Empty, value.Trim())
            End Set
        End Property

#End Region

    End Class

End Namespace

