Namespace ELL

    Public Class TipoIndicador

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Division
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Nombre() As String

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
                Return String.Format("{0}{1}", Descripcion, If(String.IsNullOrEmpty(Nombre), String.Empty, " (" & Nombre & ")"))
            End Get
        End Property

#End Region

    End Class

End Namespace
