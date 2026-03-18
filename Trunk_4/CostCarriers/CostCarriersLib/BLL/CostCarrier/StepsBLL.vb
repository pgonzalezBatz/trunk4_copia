Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class StepsBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene step
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="datosPresupuesto"></param>
        ''' <param name="datosPresupViajes"></param>
        ''' <param name="datosPresupAnyos"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer, Optional ByVal datosPresupuesto As ELL.DatosDistribucion = Nothing, Optional ByVal datosPresupViajes As ELL.DatosDistribucion = Nothing, Optional ByVal datosPresupAnyos As List(Of ELL.DatosDistribucionAnyos) = Nothing) As ELL.Step
            Dim [step] As ELL.Step = DAL.StepsDAL.getObject(id)

            [step]._datosPresupuesto = datosPresupuesto
            [step]._datosPresupViajes = datosPresupViajes
            [step]._datosPresupAnyos = datosPresupAnyos

            Return [step]
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <param name="datosPresupuesto"></param>
        ''' <param name="datosPresupViajes"></param>
        ''' <param name="datosPresupAnyos"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idCostGroup As Integer, ByVal datosPresupuesto As ELL.DatosDistribucion, ByVal datosPresupViajes As ELL.DatosDistribucion, ByVal datosPresupAnyos As List(Of ELL.DatosDistribucionAnyos)) As List(Of ELL.Step)
            Dim steps As List(Of ELL.Step) = DAL.StepsDAL.loadList(idCostGroup)

            For Each [step] In steps
                [step]._datosPresupuesto = datosPresupuesto
                [step]._datosPresupViajes = datosPresupViajes
                [step]._datosPresupAnyos = datosPresupAnyos
            Next

            Return steps
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CargarListado() As List(Of ELL.Step)
            Return DAL.StepsDAL.loadList()
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorCabecera(ByVal idCabecera As Integer) As List(Of ELL.Step)
            Return DAL.StepsDAL.loadListByCabecera(idCabecera)
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorCodigoProyecto(ByVal codigo As String) As List(Of ELL.Step)
            Return DAL.StepsDAL.loadListByCodigo(codigo)
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="idPlantaSab"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorPlantaConEstadoValidacion(Optional ByVal idPlantaSab As Integer? = Nothing) As List(Of ELL.Step)
            Return DAL.StepsDAL.loadListByPlantaConEstadoValidacion(idPlantaSab)
        End Function

        ''' <summary>
        ''' Obtiene los steps validados para un costgroup y un usuario
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <param name="idUser"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoValidados(ByVal idCostGroup As Integer, ByVal idUser As Integer) As List(Of ELL.Step)
            Return DAL.StepsDAL.loadListValidados(idCostGroup, idUser)
        End Function

        ''' <summary>
        ''' Obtiene los steps validados para un costgroup
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoValidadosFinal(ByVal idCostGroup As Integer) As List(Of ELL.Step)
            Return DAL.StepsDAL.loadListValidadosFinal(idCostGroup)
        End Function

        ''' <summary>
        ''' Obtiene steps
        ''' </summary>
        ''' <param name="idValidacion"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorValidacion(ByVal idValidacion As Integer) As List(Of ELL.Step)
            Return DAL.StepsDAL.loadListByValidacion(idValidacion)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlanta"></param>
        ''' <param name="idEstado"></param>
        ''' <param name="idAgrupacion"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerNumStepsAgrupados(ByVal idCabecera As Integer, ByVal idPlanta As Integer, ByVal idEstado As String, ByVal idAgrupacion As Integer) As Integer
            Return DAL.StepsDAL.getNumStepsAgrupados(idCabecera, idPlanta, idEstado, idAgrupacion)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function ObtenerUltimoSecuencia(ByVal idCabecera As Integer) As Integer
            Return DAL.StepsDAL.getLastSecuencia(idCabecera)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="idCostGroup"></param>
        Public Shared Sub CambiarPlanta(ByVal idStep As Integer, idCostGroup As Integer)
            DAL.StepsDAL.ChangeCompany(idStep, idCostGroup)
        End Sub

        ''' <summary>
        ''' Borrar, inserta y actualiza steps
        ''' </summary>
        ''' <param name="idCostGroup"></param>
        ''' <param name="listaStepsInsertar"></param>
        ''' <param name="listaIdStepsOfertaBorrar"></param>
        ''' <param name="listaStepsActualizar"></param>
        Public Shared Sub BorrarViejosGuardarNuevos(ByVal idCostGroup As Integer, ByVal listaStepsInsertar As List(Of ELL.StepOferta), ByVal listaIdStepsOfertaBorrar As List(Of String), ByVal listaStepsActualizar As List(Of ELL.StepOferta))
            DAL.StepsDAL.DeleteOldSaveNew(idCostGroup, listaStepsInsertar, listaIdStepsOfertaBorrar, listaStepsActualizar)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="descripcion"></param>
        Public Shared Sub CambiarDescripcionStep(ByVal idStep As Integer, descripcion As String)
            DAL.StepsDAL.ChangeDescription(idStep, descripcion)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="porcentaje"></param>
        Public Shared Sub CambiarPorcentajeStep(ByVal idStep As Integer, porcentaje As Integer)
            DAL.StepsDAL.ChangePercentage(idStep, porcentaje)
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="codigoPortador"></param>
        Public Shared Sub CambiarCodigoPortador(ByVal idStep As Integer, ByVal codigoPortador As String)
            DAL.StepsDAL.ChangeCodigoPortador(idStep, codigoPortador)
        End Sub

        ''' <summary>
        ''' Guarda un step
        ''' </summary>
        ''' <param name="step"></param>
        ''' <remarks></remarks>
        Public Shared Sub Guardar(ByVal [step] As ELL.Step)
            DAL.StepsDAL.Save([step])
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="listaSteps"></param>
        Public Shared Sub GuardarLista(ByVal listaSteps As List(Of ELL.Step))
            For Each [step] In listaSteps
                DAL.StepsDAL.Save([step])
            Next
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <param name="correlativo"></param>
        Public Shared Sub GuardarCorrelativo(ByVal idStep As Integer, ByVal correlativo As Integer)
            DAL.StepsDAL.ChangeCorrelative(idStep, correlativo)
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un step plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Eliminar(ByVal id As Integer)
            DAL.StepsDAL.Delete(id)
        End Sub

#End Region

    End Class

End Namespace