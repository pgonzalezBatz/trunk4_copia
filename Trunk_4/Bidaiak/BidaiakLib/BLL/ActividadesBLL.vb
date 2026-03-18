Namespace BLL

    Public Class ActividadesBLL

        Private actividadesDAL As New DAL.ActividadesDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de una actividad
        ''' </summary>
        ''' <param name="id">Id</param>        
        ''' <param name="bGetDepartName">Indica si sera necesario obtener el nombre del departamento</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer, Optional ByVal bGetDepartName As Boolean = True) As ELL.Actividad
            Dim oActividad As ELL.Actividad = actividadesDAL.loadInfo(id)
            If (oActividad IsNot Nothing) Then
                oActividad.DepartamentosAfectados = loadDepartamentosActividad(oActividad.Id, bGetDepartName)
            End If
            Return oActividad
        End Function

        ''' <summary>
        ''' Obtiene el listado de actividades
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bExentas">Indica si se quieren obtener las exentas, las no exentas o todas</param>
        ''' <param name="bIncluirObsoletos">Indica si se incluiran los obsoletos</param>
        ''' <param name="bLoadDepto">Indica si se cargaran los departamentos</param>
        ''' <param name="textFilter">Texto de la actividad a buscar</param>
        ''' <returns></returns>  
        Public Function loadList(ByVal idPlanta As Integer, Optional ByVal bExentas As Nullable(Of Boolean) = Nothing, Optional ByVal bIncluirObsoletos As Boolean = True, Optional ByVal bLoadDepto As Boolean = False, Optional ByVal textFilter As String = "") As List(Of ELL.Actividad)
            Dim lActividades As List(Of ELL.Actividad) = actividadesDAL.loadList(idPlanta, bExentas, bIncluirObsoletos, textFilter)
            If (bLoadDepto AndAlso lActividades IsNot Nothing AndAlso lActividades.Count > 0) Then
                For Each oActiv As ELL.Actividad In lActividades
                    oActiv.DepartamentosAfectados = loadDepartamentosActividad(oActiv.Id)
                Next
            End If
            Return lActividades
        End Function

        ''' <summary>
        ''' Obtiene el listado de actividades asociados a uno o a todos los departamento
        ''' </summary>
        ''' <param name="dpto">Departamento</param>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <param name="tipoExentas">Tipo de las exentas a mostrar. 0:todas,1:Exentas solo,2:No exentas solo</param>
        ''' <returns></returns>  
        Public Function loadListDpto(ByVal dpto As String, ByVal idPlanta As Integer, ByVal tipoExentas As Integer) As List(Of ELL.Actividad)
            Return actividadesDAL.loadListDpto(dpto, idPlanta, tipoExentas)
        End Function

        ''' <summary>
        ''' Obtiene los departamentos asociados a una actividad
        ''' </summary>
        ''' <param name="idActividad">Id de la actividad</param>
        ''' <param name="bGetDepartName">Indica si sera necesario obtener el nombre del departamento</param>
        ''' <returns></returns>        
        Public Function loadDepartamentosActividad(ByVal idActividad As Integer, Optional ByVal bGetDepartName As Boolean = True) As List(Of SabLib.ELL.Departamento)
            Return actividadesDAL.loadDepartamentosActividad(idActividad, bGetDepartName)
        End Function

        ''' <summary>
        ''' Comprueba si esa actividad tienen personas relacionadas
        ''' </summary>
        ''' <param name="idActividad">Id de la actividad</param>
        ''' <returns></returns>  
        Public Function tieneIntegrantesRelacionados(ByVal idActividad As Integer) As Boolean
            Return actividadesDAL.tieneIntegrantesRelacionados(idActividad)
        End Function

        ''' <summary>
        ''' Obtiene los departamentos que no tienen asignado ninguna actividad
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function loadDepartamentosSinActividad(ByVal idPlanta As Integer) As List(Of String())
            Return actividadesDAL.loadDepartamentosSinActividad(idPlanta)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica la actividad
        ''' </summary>
        ''' <param name="oActiv">Objeto con la informacion</param>        
        Public Sub Save(ByVal oActiv As ELL.Actividad)
            actividadesDAL.Save(oActiv)
        End Sub

#End Region

    End Class

End Namespace