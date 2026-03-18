Imports DOBLib.DAL

Namespace BLL

    Public Class ObjetivosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un objetivo
        ''' </summary>
        ''' <param name="idObjetivo">Id del objetivo</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerObjetivo(ByVal idObjetivo As Integer) As ELL.Objetivo
            Return ObjetivosDAL.getObjetivo(idObjetivo)
        End Function

        ''' <summary>
        ''' Obtiene un listado de objetivos
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="idResponsable"></param>
        ''' <param name="ejercicio"></param> 
        ''' <param name="idReto"></param>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(ByVal idPlanta As Integer, Optional ByVal idResponsable As Integer? = Nothing,
                                             Optional ByVal ejercicio As Integer? = Nothing, Optional ByVal idReto As Integer? = Nothing, Optional ByVal idProceso As Integer? = Nothing) As List(Of ELL.Objetivo)
            Return ObjetivosDAL.loadList(idPlanta, idResponsable, ejercicio, idReto, idProceso)
        End Function

        ''' <summary>
        ''' Obtiene un listado de objetivos
        ''' </summary>
        ''' <param name="idObjetivoPadre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListadoPorPadre(ByVal idObjetivoPadre As Integer) As List(Of ELL.Objetivo)
            Return ObjetivosDAL.loadListBypadre(idObjetivoPadre)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idTipoIndicador"></param>
        ''' <returns></returns>
        Public Shared Function ExisteTipoIndicador(ByVal idTipoIndicador As Integer) As Boolean
            Return ObjetivosDAL.existTipoIndicador(idTipoIndicador)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idResponsable"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function ExisteResponsable(ByVal idResponsable As Integer, ByVal idPlanta As Integer) As Boolean
            Return ObjetivosDAL.existResponsable(idResponsable, idPlanta)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idReto"></param>
        ''' <returns></returns>
        Public Shared Function ExisteReto(ByVal idReto As Integer) As Boolean
            Return ObjetivosDAL.existReto(idReto)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        Public Shared Function ExisteProceso(ByVal idProceso As Integer) As Boolean
            Return ObjetivosDAL.existProceso(idProceso)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlanta"></param>
        Public Shared Function ObtenerEjercicios(ByVal idPlanta As Integer) As List(Of Integer)
            Return ObjetivosDAL.getEjercicios(idPlanta)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un objetivo
        ''' </summary>
        ''' <param name="objetivo">Objetivo</param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal objetivo As ELL.Objetivo)
            ObjetivosDAL.Save(objetivo)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un objetivo
        ''' </summary>
        ''' <param name="idObjetivo">Id del objetivo</param>
        Public Shared Sub Eliminar(ByVal idObjetivo As Integer)
            ObjetivosDAL.DeleteObjetivo(idObjetivo)
        End Sub

#End Region

    End Class

End Namespace