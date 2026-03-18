Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValoresStepDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene valores del step
        ''' </summary>
        ''' <param name="idStep"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal idStep As Integer) As List(Of ELL.ValorStep)
            Dim query As String = "SELECT * FROM VALORES_STEP WHERE ID_STEP=:ID_STEP"

            Return Memcached.OracleDirectAccess.Seleccionar(Of ELL.ValorStep)(Function(r As OracleDataReader) _
            New ELL.ValorStep With {.IdStep = CInt(r("ID_STEP")), .IdColumna = CInt(r("ID_COLUMNA")), .Año = CInt(r("AÑO")),
                                    .Valor = CInt(r("VALOR")), .Trimestre = CInt(r("TRIMESTRE"))}, query, CadenaConexion, New OracleParameter("ID_STEP", OracleDbType.Int32, idStep, ParameterDirection.Input))
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda los valores de los steps
        ''' </summary>
        ''' <param name="valoresSteps"></param>
        ''' <param name="porcentajesStep"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal valoresSteps As List(Of ELL.ValorStep), ByVal porcentajesStep As List(Of KeyValuePair(Of Integer, Integer)))
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim contador As Integer = Integer.MinValue

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                Dim lParameters As New List(Of OracleParameter)

                ' 1- Para cada valor guardamos. Puede ser insertar o actualizar
                For Each valor In valoresSteps
                    query = "SELECT COUNT(*) FROM VALORES_STEP WHERE ID_STEP=:ID_STEP AND ID_COLUMNA=:ID_COLUMNA AND AÑO=:AÑO AND TRIMESTRE=:TRIMESTRE"

                    lParameters.Clear()
                    lParameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, valor.IdStep, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_COLUMNA", OracleDbType.Int32, valor.IdColumna, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("AÑO", OracleDbType.Int32, valor.Año, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("TRIMESTRE", OracleDbType.Int32, valor.Trimestre, ParameterDirection.Input))

                    contador = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, con, lParameters.ToArray())

                    lParameters.Clear()
                    lParameters.Add(New OracleParameter("ID_STEP", OracleDbType.Int32, valor.IdStep, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID_COLUMNA", OracleDbType.Int32, valor.IdColumna, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("AÑO", OracleDbType.Int32, valor.Año, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("TRIMESTRE", OracleDbType.Int32, valor.Trimestre, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("VALOR", OracleDbType.Int32, If(valor.Valor = Integer.MinValue, DBNull.Value, valor.Valor), ParameterDirection.Input))

                    If (contador = 0) Then
                        If (valor.Valor <> Integer.MinValue) Then
                            query = "INSERT INTO VALORES_STEP (ID_STEP, ID_COLUMNA, VALOR, AÑO, TRIMESTRE) VALUES (:ID_STEP, :ID_COLUMNA, :VALOR, :AÑO, :TRIMESTRE)"
                        End If

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    Else
                        If (valor.Valor = Integer.MinValue) Then
                            query = "DELETE VALORES_STEP WHERE ID_STEP=:ID_STEP AND ID_COLUMNA=:ID_COLUMNA AND AÑO=:AÑO AND TRIMESTRE=:TRIMESTRE"
                        Else
                            query = "UPDATE VALORES_STEP SET VALOR=:VALOR WHERE ID_STEP=:ID_STEP AND ID_COLUMNA=:ID_COLUMNA AND AÑO=:AÑO AND TRIMESTRE=:TRIMESTRE"
                        End If

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    End If
                Next

                ' 2- Guardamos cada porcentajes de step
                query = "UPDATE STEP SET PORCENTAJE=:PORCENTAJE WHERE ID=:ID"

                For Each keyValuePair In porcentajesStep
                    lParameters.Clear()

                    lParameters.Add(New OracleParameter("PORCENTAJE", OracleDbType.Int32, keyValuePair.Value, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, keyValuePair.Key, ParameterDirection.Input))

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

        Friend Shared Function CargarPaises(q As String) As List(Of String())
            Dim query As String = "SELECT C.NOMPAI,C.CODPAI
                                    FROM XBAT.COPAIS C 
                                    LEFT OUTER join PAISES P ON P.CODPAI = C.CODPAI
                                    WHERE P.CODPAI IS NULL 
                                   AND C.NOMPAI LIKE :Q
                                    ORDER BY C.NOMPAI ASC"
            Dim result As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, CadenaConexion, New OracleParameter("Q", OracleDbType.NVarchar2, String.Format("%{0}%", q.ToUpper()), ParameterDirection.Input))
            Return result
        End Function

        Friend Shared Function CargarPaisesImportados() As List(Of String())
            Dim query As String = "SELECT X.CODPAI,X.NOMPAI 
                                    FROM PAISES P
                                    JOIN XBAT.COPAIS X ON X.CODPAI = P.CODPAI
                                    ORDER BY X.NOMPAI ASC"
            Dim result As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, CadenaConexion)
            Return result
        End Function

        Friend Shared Sub AgregarPais(codigoPais As Integer)
            Dim query As String = "INSERT INTO PAISES (CODPAI) VALUES (:CODPAI)"
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("CODPAI", OracleDbType.Int32, codigoPais, ParameterDirection.Input))
        End Sub

        Friend Shared Sub EliminarPais(codigo As Integer)
            Dim query As String = "DELETE FROM PAISES WHERE CODPAI=:CODPAI"
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("CODPAI", OracleDbType.Int32, codigo, ParameterDirection.Input))
        End Sub

#End Region

    End Class

End Namespace