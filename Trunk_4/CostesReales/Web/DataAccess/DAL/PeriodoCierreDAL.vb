Imports System.Data.SqlClient

Public Class PeriodoCierreDAL

    Private ms As New MPCR
    Public Function Obtener(Anyo As Integer) As DataTable
        Dim query As String = "SELECT * from T_Parametros"
        If Not String.IsNullOrEmpty(Anyo) Then query += " WHERE anyo = '" & Anyo & "' "
        query += " ORDER BY Anyo, Mes ASC"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerTodo() As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT * from T_Parametros")

    End Function

    Public Function Buscar(Anyo As Integer, Mes As Integer) As DataTable
        Dim query As String = "SELECT * FROM T_Parametros WHERE Anyo = " + Anyo.ToString()
        If Not String.IsNullOrEmpty(Mes) Then query += " AND mes = '" + Mes.ToString() + "' "
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Eliminar(Id As String) As DataTable
        Dim query As String = "DELETE FROM T_Parametros WHERE id = " + Id
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Actualizar(id As Integer, Anyo As Integer, Mes As Integer, Fecha_cierre As Integer, Anyo_AA As Integer, Mes_AA As Integer, Fecha_cierre_inicio_mes As Integer, Fecha_TM As Integer, Tasa_chatarra As String, Activo As Boolean, PYG As String) As DataTable
        Dim query As String = "UPDATE T_Parametros Set Anyo = " + Anyo.ToString() + ", Mes = " + Mes.ToString() + ", Fecha_cierre = " + Fecha_cierre.ToString() + ", 
                               Anyo_AA = " + Anyo_AA.ToString() + ", Mes_AA = " + Mes_AA.ToString() + ", Fecha_cierre_inicio_mes = " + Fecha_cierre_inicio_mes.ToString() + ", 
                               Fecha_TM = " + Fecha_TM.ToString() + ", Tasa_chatarra = " + Tasa_chatarra.Replace(",", ".") + ", Activo = '" + Activo.ToString() + "', PYG = '" + PYG + "' 
                               WHERE id = " + id.ToString()
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Friend Sub InsertarChatarra(fechaCierre As String, cantidadChatarra As Integer, valorChatarra As Double, valorChatarraRepartido As Double)
        Dim query As String = "INSERT INTO R_MATERIAL_CHATARRA (Fecha_Id,Cantidad_Chatarra,Valor_Chatarra,Valor_Chatarra_Repartido) 
                                VALUES(@FECHACIERRE,@CANTIDADCHATARRA,@VALORCHATARRA,@VALORCHATARRAREPARTIDO)"
        Dim lParam As New List(Of SqlParameter)
        Dim p1 = New SqlParameter("@FECHACIERRE", SqlDbType.Int, ParameterDirection.Input)
        p1.Value = CInt(fechaCierre)
        lParam.Add(p1)
        Dim p2 = New SqlParameter("@CANTIDADCHATARRA", SqlDbType.Int, ParameterDirection.Input)
        p2.Value = If(cantidadChatarra > Integer.MinValue, cantidadChatarra, DBNull.Value)
        lParam.Add(p2)
        Dim p3 = New SqlParameter("@VALORCHATARRA", SqlDbType.Float, ParameterDirection.Input)
        p3.Value = If(valorChatarra > Double.MinValue, valorChatarra, DBNull.Value)
        lParam.Add(p3)
        Dim p4 = New SqlParameter("@VALORCHATARRAREPARTIDO", SqlDbType.Float, ParameterDirection.Input)
        p4.Value = If(valorChatarraRepartido > Double.MinValue, valorChatarraRepartido, DBNull.Value)
        lParam.Add(p4)
        Memcached.SQLServerDirectAccess.NoQuery(query, ms.Cx, lParam.ToArray)
    End Sub

    Public Shared Function Nuevo(Id As Integer, Anyo As Integer, Mes As Integer, Fecha_cierre As Integer, Anyo_AA As Integer, Mes_AA As Integer, Fecha_cierre_inicio_mes As Integer, Fecha_TM As Integer, Tasa_chatarra As Decimal, Activo As Boolean, PYG As String) As DataTable
        Dim query As String = "INSERT INTO T_Parametros (Id, Anyo, Mes, Fecha_cierre, Anyo_AA, Mes_AA, Fecha_cierre_inicio_mes, Fecha_TM, Tasa_chatarra, Activo, PYG) VALUES ('" + Id.ToString() + "', '" + Anyo.ToString() + "','" + Mes.ToString() + "', '" + Fecha_cierre.ToString() + "','" + Anyo_AA.ToString() + "','" + Mes_AA.ToString() + "', '" + Fecha_cierre_inicio_mes.ToString() + "','" + Fecha_TM.ToString() + "', " + Tasa_chatarra.ToString().Replace(",", ".") + ",'" + Activo.ToString + "', '" + PYG.ToString() + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerMaxId(Id As Integer) As DataTable
        Dim query As String = "SELECT MAX(Id) from T_parametros"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
