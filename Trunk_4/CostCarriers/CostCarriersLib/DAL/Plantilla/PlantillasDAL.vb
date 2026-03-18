Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class PlantillasDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una plantilla
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Public Shared Function getPlantilla(ByVal id As Integer) As ELL.Plantilla
            Dim query As String = "SELECT * FROM PLANTILLA WHERE ID=:ID"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Plantilla)(Function(r As OracleDataReader) _
            New ELL.Plantilla With {.Id = CInt(r("ID")), .IdTipoProyecto = CStr(r("ID_TIPO_PROYECTO")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .FechaAlta = CDate(r("FECHA_ALTA"))},
                                    query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene una plantilla
        ''' </summary>
        ''' <param name="idTipoProyecto"></param>
        ''' <returns></returns>
        Public Shared Function getPlantillaByTipoProyecto(ByVal idTipoProyecto As Integer) As ELL.Plantilla
            Dim query As String = "SELECT * FROM PLANTILLA WHERE ID_TIPO_PROYECTO=:ID_TIPO_PROYECTO"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Plantilla)(Function(r As OracleDataReader) _
            New ELL.Plantilla With {.Id = CInt(r("ID")), .IdTipoProyecto = CInt(r("ID_TIPO_PROYECTO")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .FechaAlta = CDate(r("FECHA_ALTA"))},
                                    query, CadenaConexion, New OracleParameter("ID_TIPO_PROYECTO", OracleDbType.Int32, idTipoProyecto, ParameterDirection.Input)).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene todas las plantillas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.Plantilla)
            Dim query As String = "SELECT * FROM PLANTILLA"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Plantilla)(Function(r As OracleDataReader) _
            New ELL.Plantilla With {.Id = CInt(r("ID")), .IdTipoProyecto = CStr(r("ID_TIPO_PROYECTO")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .FechaAlta = CDate(r("FECHA_ALTA"))},
                                    query, CadenaConexion)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una plantilla
        ''' </summary>
        ''' <param name="plantilla"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal plantilla As ELL.Plantilla)
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = (plantilla.Id = 0)

            ' Guardamos el objetivo
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_TIPO_PROYECTO", OracleDbType.Int32, plantilla.IdTipoProyecto, ParameterDirection.Input))

            If (bNuevo) Then
                query = "INSERT INTO PLANTILLA (ID_TIPO_PROYECTO, ID_USUARIO_ALTA) VALUES(:ID_TIPO_PROYECTO, :ID_USUARIO_ALTA) RETURNING ID INTO :RETURN_VALUE"

                lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, plantilla.IdUsuarioAlta, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)
            Else
                query = "UPDATE PLANTILLA SET ID_TIPO_PROYECTO=:ID_TIPO_PROYECTO WHERE ID=:ID"

                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, plantilla.Id, ParameterDirection.Input))
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            If (bNuevo) Then
                plantilla.Id = lParameters.Last.Value
            End If
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un plantilla en cascada
        ''' </summary>
        ''' <param name="id"></param>
        Public Shared Sub Delete(ByVal id As Integer)
            Dim query As String = "DELETE FROM PLANTILLA WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace