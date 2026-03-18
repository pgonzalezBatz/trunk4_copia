Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostGroupBonosBLL

        ''' <summary>
        ''' Obtiene los cost groups de bonos
        ''' </summary>
        ''' <returns>Json</returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idPlanta As Integer) As String
            Dim ret As String = String.Empty
            Using cliente As New ServicioBonos.ServicioBonos
                ret = cliente.GetGruposDistribucion(idPlanta)
            End Using

            Return ret
        End Function

    End Class

End Namespace

