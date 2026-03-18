Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PlantasPlantillaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una planta plantilla
        ''' </summary>
        ''' <param name="idPlantaPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function getPlantaPlantilla(ByVal idPlantaPlantilla As Integer) As ELL.PlantaPlantilla
            Dim query As String = "SELECT * FROM VPLANTAS_PLANTILLA WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.PlantaPlantilla)(Function(r As OracleDataReader) _
            New ELL.PlantaPlantilla With {.Id = CInt(r("ID")), .IdPlantilla = CInt(r("ID_PLANTILLA")), .IdPlanta = CInt(r("ID_PLANTA")),
                                          .Planta = CStr(r("PLANTA")), .IdTipoproyecto = CInt(r("ID_TIPO_PROYECTO"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, idPlantaPlantilla, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene plantas plantilla por plantilla
        ''' </summary>
        ''' <param name="idPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idPlantilla As Integer) As List(Of ELL.PlantaPlantilla)
            Dim query As String = "SELECT * FROM VPLANTAS_PLANTILLA WHERE ID_PLANTILLA=:ID_PLANTILLA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.PlantaPlantilla)(Function(r As OracleDataReader) _
            New ELL.PlantaPlantilla With {.Id = CInt(r("ID")), .IdPlantilla = CInt(r("ID_PLANTILLA")), .IdPlanta = CInt(r("ID_PLANTA")),
                                          .Planta = CStr(r("PLANTA")), .IdTipoproyecto = CInt(r("ID_TIPO_PROYECTO"))}, query, CadenaConexion, New OracleParameter("ID_PLANTILLA", OracleDbType.Int32, idPlantilla, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene plantas plantilla por plantilla
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <returns></returns>
        Public Shared Function loadListByIdTipoProyecto(ByVal idTipoProyecto As Integer) As List(Of ELL.PlantaPlantilla)
            Dim query As String = "SELECT * FROM VPLANTAS_PLANTILLA WHERE ID_TIPO_PROYECTO=:ID_TIPO_PROYECTO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.PlantaPlantilla)(Function(r As OracleDataReader) _
            New ELL.PlantaPlantilla With {.Id = CInt(r("ID")), .IdPlantilla = CInt(r("ID_PLANTILLA")), .IdPlanta = CInt(r("ID_PLANTA")),
                                          .Planta = CStr(r("PLANTA")), .IdTipoproyecto = CInt(r("ID_TIPO_PROYECTO"))}, query, CadenaConexion, New OracleParameter("ID_TIPO_PROYECTO", OracleDbType.Int32, idTipoProyecto, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una planta plantilla
        ''' </summary>
        ''' <param name="plantaPlantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal plantaPlantilla As ELL.PlantaPlantilla)
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = (plantaPlantilla.Id = 0)

            ' Guardamos el objetivo
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PLANTILLA", OracleDbType.Int32, plantaPlantilla.IdPlantilla, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, plantaPlantilla.IdPlanta, ParameterDirection.Input))

            If (bNuevo) Then
                query = "INSERT INTO PLANTAS_PLANTILLA (ID_PLANTILLA, ID_PLANTA) VALUES (:ID_PLANTILLA, :ID_PLANTA) RETURNING ID INTO :RETURN_VALUE"

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)
            Else
                query = "UPDATE PLANTILLA SET ID_PLANTILLA=:ID_PLANTILLA, ID_PLANTA:=ID_PLANTA WHERE ID=:ID"

                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, plantaPlantilla.Id, ParameterDirection.Input))
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            If (bNuevo) Then
                plantaPlantilla.Id = lParameters.Last.Value
            End If
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un planta plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM PLANTAS_PLANTILLA WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace