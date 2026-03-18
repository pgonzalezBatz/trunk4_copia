Imports DOBLib.DAL

Namespace BLL

    Public Class ProcesosBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un proceso
        ''' </summary>
        ''' <param name="idProceso">Id del proceso</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function ObtenerProceso(ByVal idProceso As Integer) As ELL.Proceso
            Return ProcesosDAL.getProceso(idProceso)
        End Function

        ''' <summary>
        ''' Obtiene un listado de procesos
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="baja"></param>
        ''' <param name="nombre"></param>
        ''' <param name="procesoDeBaja"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CargarListado(Optional ByVal idPlanta As Integer? = Nothing, Optional ByVal baja As Boolean? = False, Optional ByVal nombre As String = "", Optional ByVal procesoDeBaja? As Boolean = Nothing) As List(Of ELL.Proceso)
            Return ProcesosDAL.loadList(idPlanta, baja, nombre, procesoDeBaja)
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExistePlanta(ByVal idPlanta As Integer) As Boolean
            Return ProcesosDAL.existsPlanta(idPlanta)
        End Function

        ''' <summary>
        ''' Comprueba si existe el codigo
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="idPlanta"></param> 
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ExisteCodigo(ByVal codigo As String, ByVal idPlanta As Integer, Optional ByVal idProceso As Integer? = Nothing) As Boolean
            Return ProcesosDAL.existsCodigo(codigo, idPlanta, idProceso)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un proceso
        ''' </summary>
        ''' <param name="proceso">Proceso</param>  
        Public Shared Function Guardar(ByVal proceso As ELL.Proceso) As Integer
            Return ProcesosDAL.Save(proceso)
        End Function

        ''' <summary>
        ''' Cambia el orden
        ''' </summary>
        ''' <param name="idProceso"></param>
        ''' <param name="idProcesoCambio"></param>
        ''' <remarks></remarks>
        Public Shared Sub CambiarOrden(ByVal idProceso As Integer, ByVal idProcesoCambio As Integer)
            ProcesosDAL.ChangeOrder(idProceso, idProcesoCambio)
        End Sub

        ''' <summary>
        ''' Da de baja un proceso
        ''' </summary>
        ''' <param name="idProceso">Id del proceso</param>
        ''' <param name="idUsuario">Id del ususario</param>
        ''' <remarks></remarks>
        Public Shared Sub DarDeBaja(ByVal idProceso As Integer, ByVal idUsuario As Integer)
            ProcesosDAL.Unsubscribe(idProceso, idUsuario)
        End Sub

        ''' <summary>
        ''' Da de alta un proceso
        ''' </summary>
        ''' <param name="idProceso">Id del proceso</param> 
        ''' <remarks></remarks>
        Public Shared Sub DarDeAlta(ByVal idProceso As Integer)
            ProcesosDAL.Subscribe(idProceso)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un proceso
        ''' </summary>
        ''' <param name="idProceso">Id del proceso</param>
        Public Shared Sub Eliminar(ByVal idProceso As Integer)
            ProcesosDAL.DeleteProceso(idProceso)
        End Sub

#End Region

    End Class

End Namespace