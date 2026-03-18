Imports DOBLib.DAL

Namespace BLL

    Public Class ResponsablesAlertaBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de responsables sin el indicador informado para el mes actual
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CargarListado() As List(Of ELL.ResponsableAlerta)
            Return ResponsablesAlertaDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene un listado de responsables para revisión
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoRevision(ByVal ejercicio As Integer) As List(Of ELL.ResponsableAlerta)
            Return ResponsablesAlertaDAL.loadListRevision(ejercicio)
        End Function
#End Region

    End Class

End Namespace