Public Class ClasificarMaquinaDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT m.maquina, m.maquina_des, m.proceso As proceso_id, p.proceso, m.Planta As planta_id, pl.Planta, m.Kwh FROM T_Maquina_Clasificada m 
                                                 INNER Join T_Procesos p ON m.Proceso = p.ID 
                                                 INNER Join T_Plantas pl ON m.Planta = pl.ID"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboProcesos() As DataTable
        Dim query As String = "SELECT PROCESO, ID FROM T_Procesos"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerComboPlantas() As DataTable
        Dim query As String = "SELECT ID, PLANTA FROM T_Plantas"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Actualizar(Maquina As String, Descripcion As String, Proceso As Integer, Planta As Integer) As DataTable
        Dim query As String = ("UPDATE T_Maquina_Clasificada SET Maquina_des = '" + Descripcion.ToString() + "', Proceso = '" + Proceso.ToString() + "', Planta = '" + Planta.ToString() + "' 
                                                 WHERE Maquina = '" + Maquina.ToString() + "'")
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Eliminar(Maquina As String) As DataTable
        Dim query As String = ("DELETE FROM T_Maquina_Clasificada WHERE Maquina = '" + Maquina + "'")
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Nuevo(Maquina As Integer, Descripcion As String, Proceso As Integer, Planta As Integer, Kwh As Integer) As DataTable
        Dim query As String = ("INSERT INTO T_Maquina_Clasificada (Maquina, Maquina_des, Proceso, Planta, Kwh) 
                                VALUES ('" + Maquina.ToString() + "','" + Descripcion + "', '" + Proceso.ToString() + "', '" + Planta.ToString() + "', '" + Kwh.ToString() + "')")
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Function ObtenerMaxId(Id As Integer) As DataTable
        Dim query As String = "SELECT MAX(Id) from T_Maquina_Clasificada"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
