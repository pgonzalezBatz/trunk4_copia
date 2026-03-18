Imports DOBLib.DAL

Namespace BLL

    Public Class EvolucionObjetivosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de evolucion de objetivo
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function Obtener(ByVal idObjetivo As Integer, ByVal periodicidad As Integer) As ELL.EvolucionObjetivo
            Return EvolucionObjetivosDAL.getObject(idObjetivo, periodicidad)
        End Function

        ''' <summary>
        ''' Obtiene un listado de evolucion de objetivo
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idObjetivo As Integer) As List(Of ELL.EvolucionObjetivo)
            Return EvolucionObjetivosDAL.loadList(idObjetivo)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda las evoluciones de un objetivo
        ''' </summary>
        ''' <param name="listaEvolucionesObjetivo"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal listaEvolucionesObjetivo As List(Of ELL.EvolucionObjetivo))
            EvolucionObjetivosDAL.Save(listaEvolucionesObjetivo)
        End Sub

#End Region

    End Class

End Namespace