Public Class ComponentBLL

    ''' <summary>
    ''' Obtiene las ejecuciones que tenga un job
    ''' </summary>
    ''' <param name="jobName">Nombre del job</param>
    ''' <returns></returns>
    Public Shared Function ObtenerEjecucionesJob(ByVal jobName) As List(Of Object)
        Return ComponentDAL.ObtenerEjecucionesJob(jobName)
    End Function

End Class
