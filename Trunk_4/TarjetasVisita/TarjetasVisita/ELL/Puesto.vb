Namespace ELL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class Puesto

#Region "Propiedades"

        Public Shared ReadOnly Property Idiomas As List(Of String)
            Get
                Return New List(Of String) From {{"es-ES"}, {"en-GB"}, {"eu-ES"}, {"fr-FR"}, {"de-DE"}}
            End Get
        End Property

#End Region

#Region "Enumerados"

        ''' <summary>
        ''' 
        ''' </summary>
        Public Property Id As Integer

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property Nombre As String

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Property IdSab As Integer

#End Region

    End Class

End Namespace