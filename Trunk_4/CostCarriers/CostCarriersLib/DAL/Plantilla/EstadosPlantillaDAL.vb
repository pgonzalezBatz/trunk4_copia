Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class EstadosPlantillaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un estado plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getEstadoPlantilla(ByVal id As Integer) As ELL.EstadoPlantilla
            Dim query As String = "SELECT * FROM VESTADOS_PLANTILLA WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EstadoPlantilla)(Function(r As OracleDataReader) _
            New ELL.EstadoPlantilla With {.Id = CInt(r("ID")), .IdPlantaPlantilla = CInt(r("ID_PLANTA_PLANTILLA")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                                          .Estado = CStr(r("ESTADO"))}, query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene esatdos plantilla por planta plantilla
        ''' </summary>
        ''' <param name="idPlantaPlantilla"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idPlantaPlantilla As Integer) As List(Of ELL.EstadoPlantilla)
            Dim query As String = "SELECT * FROM VESTADOS_PLANTILLA WHERE ID_PLANTA_PLANTILLA=:ID_PLANTA_PLANTILLA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EstadoPlantilla)(Function(r As OracleDataReader) _
            New ELL.EstadoPlantilla With {.Id = CInt(r("ID")), .IdPlantaPlantilla = CInt(r("ID_PLANTA_PLANTILLA")), .IdEstadoProyecto = CInt(r("ID_ESTADO_PROYECTO")),
                                          .Estado = CStr(r("ESTADO"))}, query, CadenaConexion, New OracleParameter("ID_PLANTA_PLANTILLA", OracleDbType.Int32, idPlantaPlantilla, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un estado plantilla
        ''' </summary>
        ''' <param name="estadoPlantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal estadoPlantilla As ELL.EstadoPlantilla)
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = (estadoPlantilla.Id = 0)

            ' Guardamos el objetivo
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PLANTA_PLANTILLA", OracleDbType.Int32, estadoPlantilla.IdPlantaPlantilla, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_ESTADO_PROYECTO", OracleDbType.Int32, estadoPlantilla.IdEstadoProyecto, ParameterDirection.Input))

            If (bNuevo) Then
                query = "INSERT INTO ESTADOS_PLANTILLA (ID_PLANTA_PLANTILLA, ID_ESTADO_PROYECTO) VALUES (:ID_PLANTA_PLANTILLA, :ID_ESTADO_PROYECTO) RETURNING ID INTO :RETURN_VALUE"

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)
            Else
                query = "UPDATE ESTADOS_PLANTILLA SET ID_PLANTA_PLANTILLA=:ID_PLANTA_PLANTILLA, ID_ESTADO_PROYECTO:=ID_ESTADO_PROYECTO WHERE ID=:ID"

                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, estadoPlantilla.Id, ParameterDirection.Input))
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            If (bNuevo) Then
                estadoPlantilla.Id = lParameters.Last.Value
            End If
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un estado plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM ESTADOS_PLANTILLA WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace