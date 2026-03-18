Public Class PorcentajePorProcesoDepartamentoDAL

    Public Function Obtener() As DataTable
        Dim query As String = "SELECT * FROM T_Porcentaje_Proceso_Departamento"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

    Public Shared Function Eliminar(Id As Integer) As DataTable
        Return Utilidades.ObtenerQuerySQLSERVER("DELETE FROM T_Porcentaje_Proceso_Departamento WHERE ID_PROCESO = " + Id.ToString())

    End Function

    Public Shared Function Nuevo(IdProceso As Integer, IdDpto As Integer, Porcentaje As Decimal) As DataTable
        Dim query As String = "INSERT INTO T_Porcentaje_Proceso_Departamento VALUES (" + IdProceso.ToString() + ", " + IdDpto.ToString() + ", '" + Porcentaje.ToString().Replace(",", ".") + "')"
        Return Utilidades.ObtenerQuerySQLSERVER(query)

    End Function

End Class
