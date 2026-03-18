Imports System.Data.SqlClient

Public Class RepartoActivosAmortizacionDAL

    Private Shared ms As New MPCR

    Public Function Obtener() As DataTable
        'Dim query As String = "SELECT ACR.NUM_ACTIVO, ACR.Criterio_Reparto_ID, AC.Criterio_Reparto, ACR.Planta_ID, pl.PLANTA, ACR.Proceso_ID, PR.PROCESO
        '                       FROM T_Amortizaciones_Criterios AC 
        '                       RIGHT JOIN T_Amortizaciones_Criterios_Reparto_Activos ACR On AC.ID = ACR.Criterio_Reparto_ID
        '                       LEFT JOIN T_Plantas PL ON ACR.Planta_ID = PL.ID
        '                       LEFT JOIN T_Procesos PR ON ACR.Proceso_ID = PR.ID ORDER BY ACR.NUM_ACTIVO"
        Dim query As String = "SELECT ACR.NUM_ACTIVO, ACR.Criterio_Reparto_ID, AC.Criterio_Reparto, ACR.Planta_ID, pl.PLANTA, ACR.Proceso_ID, PR.PROCESO, acm.Maquina
                                FROM T_Amortizaciones_Criterios AC 
                                RIGHT JOIN T_Amortizaciones_Criterios_Reparto_Activos ACR On AC.ID = ACR.Criterio_Reparto_ID
                                left join T_Amortizaciones_Criterios_Maquina acm on acm.NUM_ACTIVO = ACR.NUM_ACTIVO
                                LEFT JOIN T_Plantas PL ON ACR.Planta_ID = PL.ID
                                LEFT JOIN T_Procesos PR ON ACR.Proceso_ID = PR.ID 
                                ORDER BY ACR.NUM_ACTIVO"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Sub Actualizar(NUM_ACTIVO As String, Criterio_Reparto_ID As Integer, Planta_ID As Integer, Proceso_ID As Integer, Maquina_ID As String)
        'Dim query As String = "UPDATE T_Amortizaciones_Criterios_Reparto_Activos Set Criterio_Reparto_ID = '" + Criterio_Reparto_ID.ToString() + "', Planta_ID = '" + Planta_ID.ToString() + "',  Proceso_ID = '" + Proceso_ID.ToString() + "' WHERE NUM_ACTIVO = '" + NUM_ACTIVO.ToString() + "'"
        'Return Utilidades.ObtenerQuerySQLSERVER(query)
        'Dim param As New List(Of Object)
        Dim query1 As String = "UPDATE T_AMORTIZACIONES_CRITERIOS_REPARTO_ACTIVOS
                               SET
                               CRITERIO_REPARTO_ID = @CRITERIO,
                               PLANTA_ID = @PLANTA,
                               PROCESO_ID = @PROCESO
                               WHERE NUM_ACTIVO = @NUMACTIVO"
        Dim query2 As String = "DELETE FROM T_AMORTIZACIONES_CRITERIOS_MAQUINA
                                WHERE NUM_ACTIVO = @NUMACTIVO"
        Dim query3 As String = ""
        Dim lParam As New List(Of SqlParameter)
        Dim p0, p1, p2, p3, p4
        p0 = New SqlParameter("@NUMACTIVO", SqlDbType.NVarChar, ParameterDirection.Input) With {
            .Size = 20,
            .Value = NUM_ACTIVO
        }
        p1 = New SqlParameter("@CRITERIO", SqlDbType.Int, ParameterDirection.Input) With {
            .Value = Criterio_Reparto_ID
        }
        p2 = New SqlParameter("@PLANTA", SqlDbType.Int, ParameterDirection.Input) With {
            .Value = DBNull.Value
        }
        p3 = New SqlParameter("@PROCESO", SqlDbType.Int, ParameterDirection.Input) With {
            .Value = DBNull.Value
        }
        p4 = New SqlParameter("@MAQUINA", SqlDbType.Int, ParameterDirection.Input) With {
            .Value = DBNull.Value
        }
        Select Case Criterio_Reparto_ID
            Case 1 'planta
                p2 = New SqlParameter("@PLANTA", SqlDbType.Int, ParameterDirection.Input) With {
                    .Value = Planta_ID
                }
            Case 2 'proceso
                p3 = New SqlParameter("@PROCESO", SqlDbType.Int, ParameterDirection.Input) With {
                    .Value = Proceso_ID
                }
            Case 3 'planta-proceso
                p2 = New SqlParameter("@PLANTA", SqlDbType.Int, ParameterDirection.Input) With {
                    .Value = Planta_ID
                }
                p3 = New SqlParameter("@PROCESO", SqlDbType.Int, ParameterDirection.Input) With {
                    .Value = Proceso_ID
                }
            Case 4 'maquinas
                p4 = New SqlParameter("@MAQUINA", SqlDbType.Int, ParameterDirection.Input) With {
                    .Value = Maquina_ID
                }
                query3 = "INSERT INTO T_AMORTIZACIONES_CRITERIOS_MAQUINA (NUM_ACTIVO, MAQUINA)
                          VALUES (@NUMACTIVO, @MAQUINA)"
            Case 5 'todas

        End Select
        lParam.Add(p0)
        lParam.Add(p1)
        lParam.Add(p2)
        lParam.Add(p3)
        'lParam.Add(p4)
        Memcached.SQLServerDirectAccess.NoQuery(query1, ms.Cx, lParam.ToArray)
        Memcached.SQLServerDirectAccess.NoQuery(query2, ms.Cx, p0)
        If Not query3.Equals("") Then
            Dim lParam2 As New List(Of SqlParameter)
            lParam2.Add(p0)
            lParam2.Add(p4)
            Memcached.SQLServerDirectAccess.NoQuery(query3, ms.Cx, lParam2.ToArray)
        End If
    End Sub

    Public Shared Function Eliminar(NUM_ACTIVO As String) As DataTable
        Dim query As String = "DELETE FROM T_Amortizaciones_Criterios_Reparto_Activos WHERE NUM_ACTIVO = '" + NUM_ACTIVO.ToString() + "'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Sub Nuevo(NUM_ACTIVO As String, Criterio_Reparto_ID As Integer, Planta_ID As Integer, Proceso_ID As Integer, Maquina_ID As String)
        ''TODO: mirar si existe en T_Amortizaciones_Criterios_Reparto_Activos
        ''      si no existe, se crea donde corresponda
        ''      si existe, mirar el criterio
        ''          si criterio 4 (maquinas), insertar SÓLO en T_Amortizaciones_Criterios_Maquina
        ''          si criterio 1 (plantas), ¿dejamos insertar o no? de esto dependerá el PK de la tabla T_Amortizaciones_Criterios_Reparto_Activos

        If Criterio_Reparto_ID = 4 Then
            Dim lPr As New List(Of SqlParameter)
            Dim paramMaquina = If(String.IsNullOrEmpty(Maquina_ID), DBNull.Value, Maquina_ID)
            Dim pr1 = New SqlParameter("@NUM_ACTIVO", SqlDbType.NVarChar, ParameterDirection.Input)
            pr1.Size = 20
            pr1.Value = NUM_ACTIVO
            lPr.Add(pr1)
            Dim pr2 = New SqlParameter("@MAQUINA", SqlDbType.NChar, ParameterDirection.Input)
            pr2.Size = 8
            pr2.Value = paramMaquina
            lPr.Add(pr2)
            Dim q1 = "INSERT INTO T_Amortizaciones_Criterios_Maquina (NUM_ACTIVO, Maquina) VALUES (@NUM_ACTIVO,@MAQUINA)"
            Memcached.SQLServerDirectAccess.NoQuery(q1, ms.Cx, lPr.ToArray)
        End If
        Dim query As String = ""
        Dim lParam As New List(Of SqlParameter)
        Dim paramPlanta = If(Planta_ID = Integer.MinValue, DBNull.Value, Planta_ID)
        Dim paramProceso = If(Proceso_ID = Integer.MinValue, DBNull.Value, Proceso_ID)
        Dim p1 = New SqlParameter("@NUM_ACTIVO", SqlDbType.NVarChar, ParameterDirection.Input)
        p1.Size = 20
        p1.Value = NUM_ACTIVO
        lParam.Add(p1)
        Dim p2 = New SqlParameter("@CRITERIO", SqlDbType.Int, ParameterDirection.Input)
        p2.Value = Criterio_Reparto_ID
        lParam.Add(p2)
        Dim p3 = New SqlParameter("@PLANTA", SqlDbType.Int, ParameterDirection.Input)
        p3.Value = paramPlanta
        lParam.Add(p3)
        Dim p4 = New SqlParameter("@PROCESO", SqlDbType.Int, ParameterDirection.Input)
        p4.Value = paramProceso
        lParam.Add(p4)
        query = "INSERT INTO T_Amortizaciones_Criterios_Reparto_Activos (NUM_ACTIVO, Criterio_Reparto_ID, Planta_ID, Proceso_ID) VALUES (@NUM_ACTIVO,@CRITERIO,@PLANTA,@PROCESO)"
        Memcached.SQLServerDirectAccess.NoQuery(query, ms.Cx, lParam.ToArray)
    End Sub

    Public Function ObtenerComboCriterioReparto() As DataTable
        Dim query As String = "SELECT ID, CRITERIO_REPARTO FROM T_Amortizaciones_Criterios"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboPlanta() As DataTable
        Dim query As String = "SELECT ID, PLANTA FROM T_Plantas"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboProceso() As DataTable
        Dim query As String = "SELECT ID, PROCESO FROM T_Procesos"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function
    Public Function ObtenerComboNegocio() As DataTable
        Dim query As String = "SELECT ID, LANTEGI FROM D_Business"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function
    Public Function ObtenerComboMaquina() As DataTable
        Dim query As String = "SELECT MAQUINA FROM T_Maquina_Clasificada"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
