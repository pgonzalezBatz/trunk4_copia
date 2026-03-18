Friend Class ListadoMaquinasDAL

    Public Function Obtener() As DataTable
        'Return Utilidades.ObtenerQuerySQLSERVER("SELECT SCM.PORTADOR, SCM.Maquina, MC.Maquina_des FROM T_Serial_Criterios_Maquina SCM 
        'INNER JOIN T_Maquina_Clasificada MC ON SCM.Maquina = MC.Maquina ORDER BY PORTADOR, MAQUINA ASC")
        Return Utilidades.ObtenerQuerySQLSERVER("SELECT SCM.Maquina, MC.Maquina_des FROM T_Serial_Criterios_Maquina SCM 
                                                 INNER JOIN T_Maquina_Clasificada MC ON SCM.Maquina = MC.Maquina ORDER BY MAQUINA ASC")

    End Function

End Class
