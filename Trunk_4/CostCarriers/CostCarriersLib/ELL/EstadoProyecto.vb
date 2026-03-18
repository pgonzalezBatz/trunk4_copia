Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadoProyecto

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Enum EstadoProyecto
            Development = 1
            Industrialization = 2
            R_D = 3
            Predevelopment = 4
            G3_Project_Acceptance = 5
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

