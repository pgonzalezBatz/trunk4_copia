Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class CostsGroupBLL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene cost group
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.CostGroup
            Return DAL.CostsGroupDAL.getObject(id)
        End Function

        ''' <summary>
        ''' Obtiene costs group
        ''' </summary>
        ''' <param name="idEstado"></param>
        ''' <param name="datosPresupuesto"></param>
        ''' <param name="datosPresupViajes"></param>
        ''' <param name="datosPresupAnyos"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idEstado As Integer, ByVal datosPresupuesto As List(Of ELL.DatosDistribucion), ByVal datosPresupViajes As List(Of ELL.DatosDistribucion), ByVal datosPresupAnyos As List(Of ELL.DatosDistribucionAnyos)) As List(Of ELL.CostGroup)
            Dim costGroups As List(Of ELL.CostGroup) = DAL.CostsGroupDAL.loadList(idEstado)

            For Each costGroup In costGroups
                costGroup._datosPresupuesto = datosPresupuesto.FirstOrDefault(Function(f) f.IdGrupoDistrib = costGroup.IdBonos)
                costGroup._datosPresupViajes = datosPresupViajes.FirstOrDefault()
                costGroup._datosPresupAnyos = datosPresupAnyos.Where(Function(f) f.IdGrupoDistrib = costGroup.IdBonos).ToList()
            Next

            Return costGroups
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlantaSAB"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorCabeceraPlanta(ByVal idCabecera As Integer, ByVal idPlantaSAB As Integer) As List(Of ELL.CostGroup)
            Return DAL.CostsGroupDAL.loadListByCabeceraPlanta(idCabecera, idPlantaSAB)
        End Function

        ''' <summary>
        ''' Obtiene costs group
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorPlanta(ByVal idPlanta As Integer) As List(Of ELL.CostGroup)
            Return DAL.CostsGroupDAL.loadListByPlanta(idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene los costgroups para un cambio de planta de un step
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <param name="idCostGroupOT"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoCambioPlantaStep(ByVal idPlanta As Integer, ByVal idCostGroupOT As Integer) As List(Of ELL.CostGroupCambioPlantaStep)
            Return DAL.CostsGroupDAL.loadListCambioPlantaStep(idPlanta, idCostGroupOT)
        End Function

#End Region

    End Class

End Namespace