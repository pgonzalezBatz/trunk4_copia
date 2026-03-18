Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class EvolucionAccionesDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de evolucion de acción
        ''' </summary>
        ''' <param name="idAccion"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idAccion As Integer) As List(Of ELL.EvolucionAccion)
            Dim query As String = "SELECT * FROM EVOLUCION_ACCION WHERE ID_ACCION=:ID_ACCION ORDER BY ID_PERIODICIDAD"

            Dim parameter As New OracleParameter("ID_ACCION", OracleDbType.Int32, idAccion, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EvolucionAccion)(Function(r As OracleDataReader) _
            New ELL.EvolucionAccion With {.Id = CInt(r("ID")), .IdAccion = CInt(r("ID_ACCION")), .Porcentaje = SabLib.BLL.Utils.decimalNull(r("PORCENTAJE"), Decimal.MinValue), .IdPeriodicidad = CInt(r("ID_PERIODICIDAD")),
                                          .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA"))}, query, CadenaConexion, parameter)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda las evoluciones de una acción
        ''' </summary>
        ''' <param name="listaEvolucionesAccion"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal listaEvolucionesAccion As List(Of ELL.EvolucionAccion))
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                Dim lParameters As New List(Of OracleParameter)
                For Each evolucionAccion In listaEvolucionesAccion
                    lParameters.Clear()
                    lParameters.Add(New OracleParameter("PORCENTAJE", OracleDbType.Decimal, If(String.IsNullOrEmpty(evolucionAccion.Porcentaje), DBNull.Value, CDec(evolucionAccion.Porcentaje)), ParameterDirection.Input))

                    If (evolucionAccion.Id = 0 AndAlso evolucionAccion.Porcentaje <> Decimal.MinValue AndAlso evolucionAccion.Porcentaje IsNot Nothing) Then
                        query = "INSERT INTO EVOLUCION_ACCION (ID_ACCION, PORCENTAJE, ID_PERIODICIDAD, ID_USUARIO_ALTA) " _
                                & "VALUES(:ID_ACCION, :PORCENTAJE, :ID_PERIODICIDAD, :ID_USUARIO_ALTA)"

                        lParameters.Add(New OracleParameter("ID_ACCION", OracleDbType.Int32, evolucionAccion.IdAccion, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_PERIODICIDAD", OracleDbType.Int32, evolucionAccion.IdPeriodicidad, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, evolucionAccion.IdUsuarioAlta, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    ElseIf (evolucionAccion.Id <> 0 AndAlso Not String.IsNullOrEmpty(evolucionAccion.Porcentaje)) Then
                        query = "UPDATE EVOLUCION_ACCION SET PORCENTAJE=:PORCENTAJE WHERE ID=:ID"
                        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, evolucionAccion.Id, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    ElseIf (evolucionAccion.Id <> 0 AndAlso String.IsNullOrEmpty(evolucionAccion.Porcentaje)) Then
                        query = "DELETE EVOLUCION_ACCION WHERE ID=:ID"
                        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, evolucionAccion.Id, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    End If
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