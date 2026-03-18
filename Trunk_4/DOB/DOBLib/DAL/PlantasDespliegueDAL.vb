Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class PlantasDespliegueDAL
        Inherits DALBase

#Region "Consultas"


        ''' <summary>
        ''' Obtiene un listado plantas
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList() As List(Of ELL.PlantasDespliegue)
            Dim query As String = "SELECT * FROM VPLANTAS_DESPLIEGUE"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.PlantasDespliegue)(Function(r As OracleDataReader) _
            New ELL.PlantasDespliegue With {.IdPlantaPadre = CInt(r("ID_PLANTA_PADRE")), .IdPlantaHija = CInt(r("ID_PLANTA_HIJA")),
                                            .PlantaPadre = CStr(r("PLANTA_PADRE")), .PlantaHija = CStr(r("PLANTA_HIJA"))}, query, CadenaConexion, Nothing)
        End Function

        ''' <summary>
        ''' Obtiene un listado plantas
        ''' </summary>
        ''' <param name="idPlantaPadre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListByPlantaPadre(ByVal idPlantaPadre As Integer) As List(Of ELL.PlantasDespliegue)
            Dim query As String = "SELECT * FROM VPLANTAS_DESPLIEGUE WHERE ID_PLANTA_PADRE=:ID_PLANTA_PADRE"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PLANTA_PADRE", OracleDbType.Int32, idPlantaPadre, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.PlantasDespliegue)(Function(r As OracleDataReader) _
            New ELL.PlantasDespliegue With {.IdPlantaPadre = CInt(r("ID_PLANTA_PADRE")), .IdPlantaHija = CInt(r("ID_PLANTA_HIJA")),
                                            .PlantaPadre = CStr(r("PLANTA_PADRE")), .PlantaHija = CStr(r("PLANTA_HIJA"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda las plantas despliegue
        ''' </summary>
        ''' <param name="idPlantaPadre"></param>
        ''' <param name="plantaHijas"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal idPlantaPadre As Integer, ByVal plantaHijas As List(Of Integer))
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' 1º Borramos los vínculos de padre con hijo
                query = "DELETE FROM PLANTAS_DESPLIEGUE WHERE ID_PLANTA_PADRE=:ID_PLANTA_PADRE"

                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_PLANTA_PADRE", OracleDbType.Int32, idPlantaPadre, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                ' 2º Guardamos los valores
                query = "INSERT INTO PLANTAS_DESPLIEGUE (ID_PLANTA_PADRE, ID_PLANTA_HIJA) VALUES (:ID_PLANTA_PADRE, :ID_PLANTA_HIJA)"

                For Each planta In plantaHijas
                    lParameters = New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("ID_PLANTA_PADRE", OracleDbType.Int32, idPlantaPadre, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_PLANTA_HIJA", OracleDbType.Int32, planta, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                Next

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

#End Region

    End Class

End Namespace