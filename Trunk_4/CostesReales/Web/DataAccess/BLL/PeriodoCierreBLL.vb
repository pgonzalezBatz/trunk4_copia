Public Class PeriodoCierreBLL

    Public Shared Sub Nuevo(Id As Integer, Anyo As Integer, Mes As Integer, Fecha_cierre As Integer, Anyo_AA As Integer, Mes_AA As Integer, Fecha_cierre_inicio_mes As Integer, Fecha_TM As Integer, Tasa_chatarra As Decimal, Activo As Boolean, PYG As String)
        'Dim db As New PeriodoCierreDAL()
        'db.Nuevo(Id, Anyo, Mes, Fecha_cierre, Anyo_AA, Mes_AA, Fecha_cierre_inicio_mes, Fecha_TM, Tasa_chatarra, Activo, PYG)
        PeriodoCierreDAL.Nuevo(Id, Anyo, Mes, Fecha_cierre, Anyo_AA, Mes_AA, Fecha_cierre_inicio_mes, Fecha_TM, Tasa_chatarra, Activo, PYG)
    End Sub

    Public Shared Function Obtener(Anyo As Integer) As DataTable
        Dim db As New PeriodoCierreDAL()
        Return db.Obtener(Anyo)

    End Function

    Public Shared Function ObtenerTodo() As DataTable
        Dim db As New PeriodoCierreDAL()
        Return db.ObtenerTodo()

    End Function

    Public Shared Function Buscar(Anyo As Integer, Mes As Integer) As DataTable
        Dim db As New PeriodoCierreDAL()
        Return db.Buscar(Anyo, Mes)

    End Function

    Friend Shared Sub InsertarCharra(fechaCierre As String, cantidadChatarra As Integer, valorChatarra As Double, valorChatarraRepartido As Double)
        Dim db As New PeriodoCierreDAL()
        db.InsertarChatarra(fechaCierre, cantidadChatarra, valorChatarra, valorChatarraRepartido)
    End Sub

    ''' <summary>
    ''' Se añade un ejercicio nuevo
    ''' </summary>
    ''' <param name="annoEjercicio"></param>
    Public Shared Sub AnadirEjercicio(ByVal annoEjercicio As Integer)
        Dim con As SqlClient.SqlConnection = Nothing
        Dim transact As SqlClient.SqlTransaction = Nothing
        Try
            Dim ms As New MPCR
            con = New SqlClient.SqlConnection(ms.Cx)
            con.Open()
            transact = con.BeginTransaction()

            'Se inicializa la tabla d_time
            Dim query As New Text.StringBuilder
            query.Append("insert into d_time(Fecha_id,year,month_id,month,day) ")
            query.Append("SELECT cast(DATEPART(YYYY, Fecha) as varchar(4)) +''+ RIGHT('00' + CAST(DATEPART(MM, Fecha) AS varchar(2)), 2) +''+ RIGHT('00' + CAST(DATEPART(DD, Fecha) AS varchar(2)), 2) as FECHA_ID,")
            query.Append("DATEPART(YYYY, Fecha) YEAR,DATEPART(MM, Fecha) MONTH_ID,DATENAME(MM,Fecha) MONTH,DATEPART(DD, Fecha) DAY ")
            query.Append("FROM ( ")
            query.Append("SELECT Fecha = DATEADD(DAY, rn - 1,@STRING_FECHA_INI) ")
            query.Append("FROM ( ")
            query.Append("SELECT TOP (DATEDIFF(DAY, @STRING_FECHA_INI, @STRING_FECHA_FIN)) ")
            query.Append("rn = ROW_NUMBER() OVER (ORDER BY s1.[object_id]) ")
            query.Append("FROM sys.all_objects AS s1 CROSS JOIN sys.all_objects AS s2 ")
            query.Append("ORDER BY s1.[object_id]) AS x ")
            query.Append(") AS y")
            'Ejemplo--> STRING_FECHA_INI='20230101' y STRING_FECHA_FIN='20240101'
            Memcached.SQLServerDirectAccess.NoQuery(query.ToString, con, transact, New SqlClient.SqlParameter("STRING_FECHA_INI", annoEjercicio & "0101"),
                                                                                   New SqlClient.SqlParameter("STRING_FECHA_FIN", annoEjercicio + 1 & "0101"))

            'Reparto por negocio
            query.Clear()
            query.Append("INSERT INTO T_PCT_Reparto_Negocio ")
            query.Append("SELECT @ANYO,lantegi,reparto ")
            query.Append("FROM T_PCT_Reparto_Negocio ")
            query.Append("WHERE anyo=@ANYO-1")
            Memcached.SQLServerDirectAccess.NoQuery(query.ToString, con, transact, New SqlClient.SqlParameter("ANYO", annoEjercicio))

            'Reparto por proceso departamental
            query.Clear()
            query.Append("INSERT INTO T_PCT_MOI_PROCESOS ")
            query.Append("SELECT @ANYO,proceso,dpto,pct ")
            query.Append("FROM T_PCT_MOI_PROCESOS ")
            query.Append("WHERE anyo=@ANYO-1")
            Memcached.SQLServerDirectAccess.NoQuery(query.ToString, con, transact, New SqlClient.SqlParameter("ANYO", annoEjercicio))

            transact.Commit()
        Catch ex As Exception
            transact.Rollback()
        Finally
            If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                con.Close()
                con.Dispose()
            End If
        End Try
    End Sub

End Class
