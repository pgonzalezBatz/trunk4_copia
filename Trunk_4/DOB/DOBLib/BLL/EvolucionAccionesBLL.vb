Imports DOBLib.DAL

Namespace BLL

    Public Class EvolucionAccionesBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de evolucion de acción
        ''' </summary>
        ''' <param name="idAccion"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idAccion As Integer) As List(Of ELL.EvolucionAccion)
            Return EvolucionAccionesDAL.loadList(idAccion)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda las evoluciones de una acción
        ''' </summary>
        ''' <param name="listaEvolucionesAccion"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal listaEvolucionesAccion As List(Of ELL.EvolucionAccion))
            EvolucionAccionesDAL.Save(listaEvolucionesAccion)
        End Sub

    End Class

#End Region

End Namespace