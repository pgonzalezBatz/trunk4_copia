Imports System.Data.SqlClient

Public Class HorasSerialDAL

    Private Shared ms As New MPCR

    Public Function Obtener() As DataTable
        'Return Utilidades.ObtenerQuerySQLSERVER("SELECT SCR.PORTADOR, SCR.Criterio_Reparto_ID, SC.Criterio As Criterio_Reparto, SCR.Business_ID As Business_ID, B.LANTEGI As Business FROM T_Serial_Criterios_Reparto SCR
        '                                         LEFT JOIN T_Serial_Criterios SC ON SCR.Criterio_Reparto_ID = SC.ID
        '                                         LEFT JOIN D_Business B ON SCR.Business_ID = B.ID")

        'Return Utilidades.ObtenerQuerySQLSERVER("SELECT SCR.PORTADOR, SCR.Criterio_Reparto_ID, SC.Criterio As Criterio_Reparto, SCR.Business_ID As Business_ID, B.LANTEGI As Lantegi,  MC.Maquina_des, M.Maquina As Maquina_ID
        '                                         FROM T_Serial_Criterios_Reparto SCR
        '                                         Left Join T_Serial_Criterios SC ON SCR.Criterio_Reparto_ID = SC.ID
        '                                         Left Join D_Business B ON SCR.Business_ID = B.ID
        '                                         Left Join T_Serial_Criterios_Maquina M ON SCR.PORTADOR = M.PORTADOR
        '                                         Left Join T_Maquina_Clasificada MC On M.Maquina = MC.Maquina")

        'Return Utilidades.ObtenerQuerySQLSERVER("Select SCR.PORTADOR, SCR.Criterio_Reparto_ID, SC.Criterio As Criterio_Reparto, SCR.Business_ID As Business_ID, 
        '                                         CASE WHEN Business_ID IS NOT NULL THEN B.LANTEGI else MC.Maquina_des END As Maquina_des, M.Maquina As Maquina_ID FROM T_Serial_Criterios_Reparto SCR
        '                                         LEFT JOIN T_Serial_Criterios SC ON SCR.Criterio_Reparto_ID = SC.ID
        '                                         LEFT JOIN D_Business B ON SCR.Business_ID = B.ID
        '				 LEFT JOIN T_Serial_Criterios_Maquina M ON SCR.PORTADOR = M.PORTADOR
        '				 LEFT JOIN T_Maquina_Clasificada MC ON M.Maquina = MC.Maquina")
        'Dim query As String = "SELECT SCR.PORTADOR, SCR.Criterio_Reparto_ID, SC.Criterio As Criterio_Reparto, SCR.Business_ID As Business_ID, B.Lantegi,' ' As Maquina_Des,' ' As Maquina_ID 
        '                                         FROM T_Serial_Criterios_Reparto SCR
        '                                         LEFT JOIN T_Serial_Criterios SC ON SCR.Criterio_Reparto_ID = SC.ID
        '                                         LEFT JOIN D_Business B ON SCR.Business_ID = B.ID 
        '                                         WHERE Criterio_Reparto_ID = 1
        '                                         UNION
        '                                         SELECT SCR.PORTADOR, 2, 'Maquina/s' As Criterio_Reparto, NULL As Business_ID, NULL As Lantegi, MC.Maquina_Des, M.Maquina As Maquina_ID
        '                                         FROM T_Serial_Criterios_Reparto SCR
        '                                         LEFT JOIN T_Serial_Criterios_Maquina M ON RTRIM(REPLACE(REPLACE(SCR.PORTADOR,CHAR(10),''),CHAR(13),'')) = RTRIM(M.PORTADOR)
        '                                         LEFT JOIN T_Maquina_Clasificada MC ON RTRIM(M.Maquina) = RTRIM(MC.Maquina)
        '                                         WHERE SCR.Criterio_Reparto_ID = 2 ORDER BY PORTADOR ASC"
        'Dim query As String = "
        '                        SELECT SCR.PORTADOR,  'Negocio' as Criterio_Reparto, B.Lantegi as Maquina_Lantegi,1 as CID, CONCAT(',',B.ID) as ID
        '                        FROM T_Serial_Criterios_Reparto SCR
        '                        LEFT JOIN T_Serial_Criterios SC ON SCR.Criterio_Reparto_ID = SC.ID
        '                        LEFT JOIN D_Business B ON SCR.Business_ID = B.ID 
        '                        WHERE Criterio_Reparto_ID = 1
        '                        UNION
        '                        SELECT SCR.PORTADOR,  'Maquina/s' As Criterio_Reparto, SCR.Business_ID As Maquina_Lantegi,2 as CID, SCR.Business_ID as ID
        '                        FROM T_Serial_Criterios_Reparto SCR
        '                        LEFT JOIN T_Maquina_Clasificada MC ON RTRIM(SCR.Business_ID) = RTRIM(MC.Maquina)
        '                        WHERE SCR.Criterio_Reparto_ID = 2 ORDER BY PORTADOR ASC"
        Dim query As String = "SELECT SCR.PORTADOR,  'Negocio' as Criterio_Reparto, B.Lantegi as Maquina_Lantegi,1 as CID
                                FROM T_Serial_Criterios_Reparto SCR
                                LEFT JOIN T_Serial_Criterios SC ON SCR.Criterio_Reparto_ID = SC.ID
                                LEFT JOIN D_Business B ON SCR.Business_ID = B.ID 
                                WHERE Criterio_Reparto_ID = 1
                                UNION
                                SELECT SCR.PORTADOR,  'Maquina/s' As Criterio_Reparto, SCM.Maquina As Maquina_Lantegi,2 as CID
                                FROM T_Serial_Criterios_Reparto SCR
								LEFT JOIN T_Serial_Criterios_Maquina SCM ON RTRIM(REPLACE(REPLACE(SCR.PORTADOR,CHAR(10),''),CHAR(13),'')) = RTRIM(SCM.PORTADOR)
                                LEFT JOIN T_Maquina_Clasificada MC ON RTRIM(SCM.Maquina) = RTRIM(MC.Maquina)
                                WHERE SCR.Criterio_Reparto_ID = 2 ORDER BY PORTADOR ASC"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerPortador(Portador As String) As DataTable
        Dim query As String = "SELECT PORTADOR FROM T_Serial_Criterios_Reparto WHERE PORTADOR LIKE '%" + Portador.ToString() + "%'"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboCriteriosReparto() As DataTable
        Dim query As String = "SELECT ID, CRITERIO FROM T_Serial_Criterios"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboBusiness() As DataTable
        Dim query As String = "SELECT ID, LANTEGI FROM D_Business ORDER BY LANTEGI"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboMaquinas() As DataTable
        'Dim query As String = "SELECT Maquina, Maquina_des FROM T_Maquina_Clasificada ORDER BY Maquina_des"
        Dim query As String = "SELECT Maquina FROM T_Maquina_Clasificada ORDER BY Maquina"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Sub ActualizarNegocio(Portador As String, Criterio_Reparto As Integer, Negocio As Integer, idPlanta As Integer)
        'Dim query As String = "UPDATE T_Serial_Criterios_Reparto SET Criterio_Reparto_ID = '" + Criterio_Reparto.ToString() + "', Business_ID = '" + Negocio.ToString() + "' WHERE PORTADOR Like '%" + Portador.ToString() + "%'"
        'Dim query As String = "UPDATE T_Serial_Criterios_Reparto SET Criterio_Reparto_ID = '1', Business_ID = NULL WHERE PORTADOR LIKE '%" + Portador.ToString() + "%'"
        'Return Utilidades.ObtenerQuerySQLSERVER(query)
        ''''TODO
        'Dim query = "INSERT INTO T_Amortizaciones_Criterios_Reparto_Activos ([NUM_ACTIVO],[Criterio_Reparto_ID],[Planta_ID],[Proceso_ID])
        '             VALUES(@PORTADOR,@CRITERIO,@PLANTA,@PROCESO)"
        'Dim lParam As New List(Of SqlParameter)
        'Dim p1 = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input) With {
        '    .Size = 10,
        '    .Value = Truncate(Portador, 10)
        '}
        'lParam.Add(p1)
        'Dim p2 = New SqlParameter("@CRITERIO", SqlDbType.Int, ParameterDirection.Input) With {
        '    .Value = Criterio_Reparto
        '}
        'lParam.Add(p2)
        'Dim p3 = New SqlParameter("@PLANTA", SqlDbType.Int, ParameterDirection.Input) With {
        '    .Value = idPlanta
        '}
        'lParam.Add(p3)
        'Dim p4 = New SqlParameter("@PROCESO", SqlDbType.Int, ParameterDirection.Input)
        'p4.Value = Negocio
        'lParam.Add(p4)        
        Dim q = "DELETE FROM T_SERIAL_CRITERIOS_MAQUINA WHERE PORTADOR LIKE @PORTADOR"
        Dim param = New SqlParameter("@PORTADOR", SqlDbType.Char, ParameterDirection.Input) With {
                .Size = 10,
                .Value = Truncate(Portador, 10)}
        Memcached.SQLServerDirectAccess.NoQuery(q, ms.Cx, param)

        If Criterio_Reparto = 1 Then
            Dim query = "UPDATE T_SERIAL_CRITERIOS_REPARTO SET CRITERIO_REPARTO_ID = @CRITERIO, BUSINESS_ID = @NEGOCIO WHERE PORTADOR LIKE @PORTADOR"
            Dim lParam As New List(Of SqlParameter)
            Dim p1 = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input) With {
                .Size = 10,
                .Value = Truncate(Portador, 10)
            }
            lParam.Add(p1)
            Dim p2 = New SqlParameter("@CRITERIO", SqlDbType.Int, ParameterDirection.Input) With {
            .Size = 10,
            .Value = Truncate(Criterio_Reparto, 10)
        }
            lParam.Add(p2)
            Dim p3 = New SqlParameter("@NEGOCIO", SqlDbType.NChar, ParameterDirection.Input) With {
            .Size = 8,
            .Value = Truncate(Negocio, 8)
        }
            lParam.Add(p3)
            Memcached.SQLServerDirectAccess.NoQuery(query, ms.Cx, lParam.ToArray)
        Else
            'Dim q = "INSERT INTO T_SERIAL_CRITERIOS_MAQUINA (PORTADOR,MAQUINA) VALUES(@PORTADOR,@MAQUINA)"
            'Dim param = New SqlParameter("@PORTADOR", SqlDbType.Char, ParameterDirection.Input) With {
            '    .Size = 10,
            '    .Value = Truncate(Portador, 10)}
            'Memcached.SQLServerDirectAccess.NoQuery(q, connectionString_sql_server, param)
        End If
    End Sub

    'Public Shared Sub ActualizarMaquina(Portador As String, Maquina As String)
    '    'Dim query As String = "UPDATE T_Serial_Criterios_Maquina SET Maquina = '" + Maquina.ToString() + "' WHERE PORTADOR LIKE '%" + Portador.ToString() + "%'"
    '    'Return Utilidades.ObtenerQuerySQLSERVER(query)
    '    ''''TODO
    'End Sub

    Public Sub NuevoPortadorNegocio(Portador As String, negocio As String)
        Dim ms As New MPCR
        Dim query = "INSERT INTO T_SERIAL_CRITERIOS_REPARTO (PORTADOR, CRITERIO_REPARTO_ID, BUSINESS_ID) VALUES(@PORTADOR, 1, @NEGOCIO)"
        Dim lParam As New List(Of SqlParameter)
        Dim p1 = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input)
        p1.Size = 10
        p1.Value = Truncate(Portador, 10)
        lParam.Add(p1)
        Dim p2 = New SqlParameter("@NEGOCIO", SqlDbType.NChar, ParameterDirection.Input)
        p2.Size = 8
        p2.Value = Truncate(negocio, 8)
        lParam.Add(p2)
        Memcached.SQLServerDirectAccess.NoQuery(query, ms.Cx, lParam.ToArray)
    End Sub

    Public Sub NuevoPortadorMaquina(Portador As String, maquina As String)

        Dim query0 = "SELECT COUNT(*) FROM T_SERIAL_CRITERIOS_REPARTO WHERE PORTADOR = @PORTADOR AND CRITERIO_REPARTO_ID = 2"
        Dim lParam0 As New List(Of SqlParameter)
        Dim p01 = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input)
        p01.Size = 10
        p01.Value = Truncate(Portador, 10)
        lParam0.Add(p01)
        Dim yaExiste = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(query0, ms.Cx, lParam0.ToArray) > 0

        If Not yaExiste Then
            Dim query = "INSERT INTO T_SERIAL_CRITERIOS_REPARTO (PORTADOR, CRITERIO_REPARTO_ID, BUSINESS_ID) VALUES(@PORTADOR, 2, @MAQUINA)"
            Dim lParam As New List(Of SqlParameter)
            Dim p1 = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input)
            p1.Size = 10
            p1.Value = Truncate(Portador, 10)
            lParam.Add(p1)
            Dim p2 = New SqlParameter("@MAQUINA", SqlDbType.NChar, ParameterDirection.Input)
            p2.Size = 8
            p2.Value = DBNull.Value
            lParam.Add(p2)
            Memcached.SQLServerDirectAccess.NoQuery(query, ms.Cx, lParam.ToArray)
        End If

        Dim query2 = "INSERT INTO T_SERIAL_CRITERIOS_MAQUINA (PORTADOR, MAQUINA) VALUES(@PORTADOR, @MAQUINA)"
        Dim lParam2 As New List(Of SqlParameter)
        Dim p12 = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input)
        p12.Size = 10
        p12.Value = Truncate(Portador, 10)
        lParam2.Add(p12)
        Dim p22 = New SqlParameter("@MAQUINA", SqlDbType.NChar, ParameterDirection.Input)
        p22.Size = 8
        p22.Value = Truncate(maquina, 8)
        lParam2.Add(p22)
        Memcached.SQLServerDirectAccess.NoQuery(query2, ms.Cx, lParam2.ToArray)
    End Sub

    Public Shared Sub EliminarNegocio(Portador As String)
        Dim q2 = "DELETE FROM T_SERIAL_CRITERIOS_REPARTO WHERE PORTADOR = @PORTADOR AND CRITERIO_REPARTO_ID=1 AND BUSINESS_ID IS NOT NULL"
        Dim param = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input)
        param.Size = 10
        param.Value = Truncate(Portador, 10)
        Memcached.SQLServerDirectAccess.NoQuery(q2, ms.Cx, param)
    End Sub

    Public Shared Sub EliminarMaquina(Portador As String, Maquina As String)
        Dim q = "DELETE FROM T_SERIAL_CRITERIOS_MAQUINA WHERE PORTADOR = @PORTADOR AND MAQUINA = @MAQUINA"
        Dim lParam As New List(Of SqlParameter)
        Dim p1 = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input)
        p1.Size = 10
        p1.Value = Truncate(Portador, 10)
        lParam.Add(p1)
        Dim p2 = New SqlParameter("@MAQUINA", SqlDbType.NChar, ParameterDirection.Input)
        p2.Size = 8
        p2.Value = Truncate(Maquina, 8)
        lParam.Add(p2)
        Memcached.SQLServerDirectAccess.NoQuery(q, ms.Cx, lParam.ToArray)

        Dim q1 = "SELECT COUNT(*) FROM T_SERIAL_CRITERIOS_MAQUINA WHERE PORTADOR = @PORTADOR"
        Dim lParam1 As New List(Of SqlParameter)
        Dim p11 = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input)
        p11.Size = 10
        p11.Value = Truncate(Portador, 10)
        lParam1.Add(p11)
        Dim eraUnico = Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(q1, ms.Cx, lParam1.ToArray) = 0

        If eraUnico Then
            Dim q2 = "DELETE FROM T_SERIAL_CRITERIOS_REPARTO WHERE PORTADOR = @PORTADOR AND CRITERIO_REPARTO_ID=2 AND BUSINESS_ID IS NULL"
            Dim param = New SqlParameter("@PORTADOR", SqlDbType.NChar, ParameterDirection.Input)
            param.Size = 10
            param.Value = Truncate(Portador, 10)
            Memcached.SQLServerDirectAccess.NoQuery(q2, ms.Cx, param)
        End If
    End Sub

    Public Shared Function Truncate(value As String, length As Integer) As String
        If value Is Nothing Or value.Equals("") Then Return ""
        If value.Length <= length Then Return value
        Return value.Substring(0, length)
    End Function
End Class
