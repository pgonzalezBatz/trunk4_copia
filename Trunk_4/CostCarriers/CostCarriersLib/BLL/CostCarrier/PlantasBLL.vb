Imports System.Web.Script.Serialization

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PlantasBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function Obtener(ByVal id As Integer) As ELL.Planta
            Return DAL.PlantasDAL.getObject(id)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorIdPlantaSAB(ByVal id As Integer) As List(Of ELL.Planta)
            Return DAL.PlantasDAL.loadListByIdPlantaSAB(id)
        End Function

        ''' <summary>
        ''' Obtiene plantas
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function CargarListado() As List(Of ELL.Planta)
            Dim plantas As List(Of ELL.Planta) = DAL.PlantasDAL.loadList()

            Return plantas
        End Function

        ''' <summary>
        ''' Obtiene plantas
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idCabecera As Integer) As List(Of ELL.Planta)
            Dim plantas As List(Of ELL.Planta) = DAL.PlantasDAL.loadList(idCabecera)

            Return plantas
        End Function

        ''' <summary>
        ''' Obtiene plantas
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="proyecto"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idCabecera As Integer, ByVal proyecto As String) As List(Of ELL.Planta)
            Dim plantas As List(Of ELL.Planta) = DAL.PlantasDAL.loadList(idCabecera)

            Dim jss As New JavaScriptSerializer()

            Using cliente As New ServicioBonos.ServicioBonos
                For Each planta In plantas
                    planta._datosPresupuesto = jss.Deserialize(Of List(Of ELL.DatosDistribucion))(cliente.GetDatosDistribucion(proyecto, ELL.OrigenDatosStep.TipoDistribucion.Presupuesto, Integer.MinValue, planta.IdMoneda))
                    planta._datosPresupViajes = jss.Deserialize(Of List(Of ELL.DatosDistribucion))(cliente.GetDatosDistribucion(proyecto, ELL.OrigenDatosStep.TipoDistribucion.Viaje, Integer.MinValue, planta.IdMoneda))
                    planta._datosPresupAnyos = jss.Deserialize(Of List(Of ELL.DatosDistribucionAnyos))(cliente.GetDatosDistribucionPorAnyos(proyecto, ELL.OrigenDatosStep.TipoDistribucion.Planificacion, Integer.MinValue, planta.IdMoneda))
                Next
            End Using

            Return plantas
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idTipoProyecto"></param>
        ''' <param name="idProyecto"></param>
        ''' <returns></returns>
        Public Shared Function CargarPlantasAgregar(ByVal idCabecera As Integer, ByVal idTipoProyecto As Integer, ByVal idProyecto As String) As List(Of SabLib.ELL.Planta)
            ' 1- Obtenemos las plantas de la plantilla
            Dim plantasPlantilla As List(Of ELL.PlantaPlantilla) = BLL.PlantasPlantillaBLL.CargarListadoPorTipoProyecto(idTipoProyecto)

            ' 2- Obtenemos las plantas del proyecto
            Dim plantasProyecto As List(Of ELL.Planta) = BLL.PlantasBLL.CargarListado(idCabecera)

            ' 3- Seleccionamos los id de plantas que estén en la plantilla y que no estén en el proyecto ya añadidas
            Dim listaPlantasMostrar As New List(Of SabLib.ELL.Planta)
            Dim plantasBLL As New SabLib.BLL.PlantasComponent
            For Each idPlanta In plantasPlantilla.Select(Function(f) f.IdPlanta).Where(Function(f) Not plantasProyecto.Select(Function(h) h.IdPlanta).Contains(f)).ToList()
                listaPlantasMostrar.Add(plantasBLL.GetPlanta(idPlanta))
            Next

            '' 3- Cargamos las plant to charge del proyecto
            'Dim listaIdPlantasCargar As List(Of Integer) = PlantasACargarBLL.ObtenerPlantasACargar(idProyecto)

            '' 4- Seleccionamos los id de plantas que estén en la plantilla y que no estén en el proyecto ya añadidas
            'Dim listaPlantasMostrar As New List(Of SabLib.ELL.Planta)
            'Dim plantasBLL As New SabLib.BLL.PlantasComponent
            'For Each idPlanta In listaIdPlantasCargar.Where(Function(f) plantasPlantilla.Select(Function(g) g.IdPlanta).Contains(f) AndAlso Not plantasProyecto.Select(Function(h) h.IdPlanta).Contains(f)).ToList()
            '    listaPlantasMostrar.Add(plantasBLL.GetPlanta(idPlanta))
            'Next

            Return listaPlantasMostrar
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Cambiar el SOP y años serie
        ''' </summary>
        ''' <param name="id"></param>
        ''' <param name="sop"></param>
        ''' <param name="añosSerie"></param>
        Public Shared Sub CambiarSOPAñosSerie(ByVal id As Integer, ByVal sop As DateTime, ByVal añosSerie As Integer)
            DAL.PlantasDAL.ChangeSOPAñosSerie(id, sop, añosSerie)
        End Sub

#End Region

    End Class

End Namespace