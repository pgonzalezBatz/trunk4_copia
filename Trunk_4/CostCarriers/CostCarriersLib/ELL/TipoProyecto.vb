Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class TipoProyecto

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum TipoProyecto
            R_D = 1
            Industrialization = 2
            Predev = 3
        End Enum

#End Region

#Region "Properties"

        ''' <summary>
        ''' Id
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Id() As Integer

        ''' <summary>
        ''' Descripción
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public Property Descripcion() As String

#End Region

    End Class

End Namespace

