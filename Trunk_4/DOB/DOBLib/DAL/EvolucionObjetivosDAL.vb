Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class EvolucionObjetivosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un listado de evolucion de objetivo
        ''' </summary>
        ''' <param name="idObjetivo"></param> 
        ''' <returns></returns>
        ''' <remarks></remarks>Se
        Public Shared Function getObject(ByVal idObjetivo As Integer, ByVal periodicidad As Integer) As ELL.EvolucionObjetivo
            Dim query As String = "SELECT * FROM VEVOLUCION_OBJETIVOS WHERE ID_OBJETIVO=:ID_OBJETIVO AND ID_PERIODICIDAD=:ID_PERIODICIDAD ORDER BY ID_PERIODICIDAD"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, idObjetivo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PERIODICIDAD", OracleDbType.Int32, periodicidad, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.EvolucionObjetivo)(Function(r As OracleDataReader) _
            New ELL.EvolucionObjetivo With {.Id = CInt(r("ID")), .IdObjetivo = CInt(r("ID_OBJETIVO")), .ValorActual = SabLib.BLL.Utils.decimalNull(r("VALOR_ACTUAL"), Decimal.MinValue), .IdPeriodicidad = CInt(r("ID_PERIODICIDAD")),
                                           .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .ValorInicial = CDec(r("VALOR_INICIAL")),
                                           .ValorObjetivo = CDec(r("VALOR_OBJETIVO")), .TipoIndicador = SabLib.BLL.Utils.stringNull(r("TIPO_INDICADOR")), .Planta = CStr(r("PLANTA"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de evolucion de objetivo
        ''' </summary>
        ''' <param name="idObjetivo"></param> 
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idObjetivo As Integer) As List(Of ELL.EvolucionObjetivo)
            Dim query As String = "SELECT * FROM VEVOLUCION_OBJETIVOS WHERE ID_OBJETIVO=:ID_OBJETIVO ORDER BY ID_PERIODICIDAD"

            Dim parameter As New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, idObjetivo, ParameterDirection.Input)

            Dim result = Memcached.OracleDirectAccess.Seleccionar(Of ELL.EvolucionObjetivo)(Function(r As OracleDataReader) _
            New ELL.EvolucionObjetivo With {.Id = CInt(r("ID")), .IdObjetivo = CInt(r("ID_OBJETIVO")), .ValorActual = SabLib.BLL.Utils.decimalNull(r("VALOR_ACTUAL"), Decimal.MinValue), .IdPeriodicidad = CInt(r("ID_PERIODICIDAD")),
                                           .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .ValorInicial = CDec(r("VALOR_INICIAL")),
                                           .ValorObjetivo = CDec(r("VALOR_OBJETIVO")), .TipoIndicador = SabLib.BLL.Utils.stringNull(r("TIPO_INDICADOR")), .Planta = CStr(r("PLANTA"))}, query, CadenaConexion, parameter)
            Return result
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda las evoluciones de un objetivo
        ''' </summary>
        ''' <param name="listaEvolucionesObjetivo"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal listaEvolucionesObjetivo As List(Of ELL.EvolucionObjetivo))
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                Dim lParameters As New List(Of OracleParameter)
                For Each evolucionObjetivo In listaEvolucionesObjetivo
                    lParameters.Clear()
                    lParameters.Add(New OracleParameter("VALOR_ACTUAL", OracleDbType.Decimal, If(String.IsNullOrEmpty(evolucionObjetivo.ValorActual), DBNull.Value, CDec(evolucionObjetivo.ValorActual)), ParameterDirection.Input))

                    If (evolucionObjetivo.Id = 0 AndAlso evolucionObjetivo.ValorActual <> Decimal.MinValue AndAlso evolucionObjetivo.ValorActual IsNot Nothing) Then
                        query = "INSERT INTO EVOLUCION_OBJETIVO (ID_OBJETIVO, VALOR_ACTUAL, ID_PERIODICIDAD, ID_USUARIO_ALTA) " _
                                & "VALUES(:ID_OBJETIVO, :VALOR_ACTUAL, :ID_PERIODICIDAD, :ID_USUARIO_ALTA)"

                        lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, evolucionObjetivo.IdObjetivo, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_PERIODICIDAD", OracleDbType.Int32, evolucionObjetivo.IdPeriodicidad, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, evolucionObjetivo.IdUsuarioAlta, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    ElseIf (evolucionObjetivo.Id <> 0 AndAlso Not String.IsNullOrEmpty(evolucionObjetivo.ValorActual)) Then
                        query = "UPDATE EVOLUCION_OBJETIVO SET VALOR_ACTUAL=:VALOR_ACTUAL WHERE ID=:ID"
                        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, evolucionObjetivo.Id, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    ElseIf (evolucionObjetivo.Id <> 0 AndAlso String.IsNullOrEmpty(evolucionObjetivo.ValorActual)) Then
                        query = "DELETE EVOLUCION_OBJETIVO WHERE ID=:ID"
                        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, evolucionObjetivo.Id, ParameterDirection.Input))

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