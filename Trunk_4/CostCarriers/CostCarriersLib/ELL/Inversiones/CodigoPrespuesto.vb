Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CodigoPrespuesto

#Region "Properties"

        ''' <summary>
        ''' Código
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Codigo() As String

        ''' <summary>
        ''' Descripción
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String

        ''' <summary>
        ''' Descripción completa
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property DescripcionCompleta() As String
            Get
                Return String.Format("{0} - {1}", Codigo, Descripcion)
            End Get
        End Property

#End Region

    End Class

End Namespace

