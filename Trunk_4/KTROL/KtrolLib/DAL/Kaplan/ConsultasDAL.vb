Imports Oracle.ManagedDataAccess.Client
Namespace DAL

    Public Class ConsultasDAL
        Inherits DALBase

        Public Function consultarCodigoOperacion(ByVal codOpe As String) As String()
            Dim query As String = "SELECT * FROM [OPERACIONES TIPO] " &
                                    "INNER JOIN [OPERACIONES DE UN ARTICULO] on [OPERACIONES DE UN ARTICULO].[COD OPERACION]=[OPERACIONES TIPO].[COD OPERACION] " &
                                    "WHERE [OPERACIONES TIPO].[COD OPERACION]=@COD_OPERACION"

            Dim parametro As New SqlClient.SqlParameter("COD_OPERACION", codOpe)

            Return Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, CnKaplanIgorre, parametro).FirstOrDefault
        End Function


        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codOpe"></param>
        ''' <returns></returns>
        Public Function cargarDatosCodigosOperacionFabricacionPorRol(ByVal codOpe As String, ByVal idRol As Integer, Optional ByVal idSubRol As Integer = 0) As List(Of ELL.Caracteristicas_Plan_Fabricacion)
            Dim query As String = String.Empty

            query = "SELECT * FROM [CARACTERISTICAS DEL PLAN FABRICACION] " &
                    "WHERE [CARACTERISTICAS DEL PLAN FABRICACION].[CODIGO]=@COD_OPERACION "

            Select Case idRol
                Case ELL.Usuarios.RolesUsuario.Administrador
                    Select Case idSubRol
                        Case ELL.Usuarios.RolesUsuarioControl.Gestor
                            query &= "AND [CARACTERISTICAS DEL PLAN FABRICACION].[Responsable_Reg_Gestor] = 1 "
                        Case ELL.Usuarios.RolesUsuarioControl.Calidad
                            query &= "AND [CARACTERISTICAS DEL PLAN FABRICACION].[Responsable_Reg_Cal] = 1 "
                    End Select
                Case ELL.Usuarios.RolesUsuario.Gestor
                    query &= "AND [CARACTERISTICAS DEL PLAN FABRICACION].[Responsable_Reg_Gestor] = 1 "
                Case ELL.Usuarios.RolesUsuario.Calidad
                    query &= "AND [CARACTERISTICAS DEL PLAN FABRICACION].[Responsable_Reg_Cal] = 1 "
                Case ELL.Usuarios.RolesUsuario.Operario
                    query &= "AND [CARACTERISTICAS DEL PLAN FABRICACION].[Responsable_Reg_Ope] = 1 " &
                             "AND ([CARACTERISTICAS DEL PLAN FABRICACION].[Responsable_Reg_Gestor] is null or [CARACTERISTICAS DEL PLAN FABRICACION].[Responsable_Reg_Gestor] = 0) "
            End Select

            query &= "AND ([Responsable_Reg_Gestor] != 0 or [Responsable_Reg_Gestor] != null or [Responsable_Reg_Ope] != 0 or [Responsable_Reg_Cal] != 0)"
            query &= "ORDER BY [CARACTERISTICAS DEL PLAN FABRICACION].[ORDEN CARAC] ASC"

			Return Memcached.SQLServerDirectAccess.seleccionar(Of ELL.Caracteristicas_Plan_Fabricacion)(Function(r As SqlClient.SqlDataReader) _
           New ELL.Caracteristicas_Plan_Fabricacion With {.ID_REGISTRO = CInt(r("ID_REGISTRO")), .POSICION = SabLib.BLL.Utils.stringNull(r("POSICION")), .CODIGO = SabLib.BLL.Utils.stringNull(r("CODIGO")),
                                                          .CARAC_PARAM = SabLib.BLL.Utils.stringNull(r("CARAC/PARAM")), .ESPECIFICACION = SabLib.BLL.Utils.stringNull(r("ESPECIFICACION")),
                                                          .CLASE = SabLib.BLL.Utils.stringNull(r("CLASE")), .METODO_CONTROL = SabLib.BLL.Utils.stringNull(r("METODO CONTROL")),
                                                          .MAXIM = SabLib.BLL.Utils.stringNull(r("MAXIM")), .MINIM = SabLib.BLL.Utils.stringNull(r("MINIM")),
                                                          .OPERACION = SabLib.BLL.Utils.integerNull(r("OPERACION")), .VER_REG_PRO = SabLib.BLL.Utils.booleanNull(r("VER_REG_PRO")),
                                                          .VER_REG_REC = SabLib.BLL.Utils.booleanNull(r("VER_REG_REC")), .VER_REG_MAT = SabLib.BLL.Utils.booleanNull(r("VER_REG_MAT")),
                                                          .VER_REG_FUN = SabLib.BLL.Utils.booleanNull(r("VER_REG_FUN")), .VER_REG_DIM = SabLib.BLL.Utils.booleanNull(r("VER_REG_DIM")),
                                                          .TAMANYO_CAL = SabLib.BLL.Utils.stringNull(r("TAMAÑO CAL")), .TAMANYO = SabLib.BLL.Utils.stringNull(r("TAMAÑO")),
                                                          .Responsable_Reg_Ope = SabLib.BLL.Utils.stringNull(r("Responsable_Reg_Ope")),
                                                          .Responsable_Reg_Cal = SabLib.BLL.Utils.stringNull(r("Responsable_Reg_Cal")),
                                                          .Responsable_Reg_Gestor = SabLib.BLL.Utils.stringNull(r("Responsable_Reg_Gestor")),
                                                          .Responsable_Registro = SabLib.BLL.Utils.stringNull(r("RESPONSABLE REGISTRO")),
                                                          .Responsable = SabLib.BLL.Utils.stringNull(r("RESPONSABLE")), .PROCESO_PRODUCTO = SabLib.BLL.Utils.stringNull(r("PROCESO/PRODUCTO")),
                                                          .PROCEDE_DE = SabLib.BLL.Utils.stringNull(r("PROCEDE DE")), .ORDEN_CARAC = SabLib.BLL.Utils.stringNull(r("ORDEN CARAC")),
                                                          .OBSERVACIONES = SabLib.BLL.Utils.stringNull(r("OBSERVACIONES")), .METODO_EVALUACION = SabLib.BLL.Utils.stringNull(r("METODO EVALUACION")),
                                                          .MEDIO_RFA = SabLib.BLL.Utils.stringNull(r("MEDIO RFA")), .MEDIO_DENOMINACION = SabLib.BLL.Utils.stringNull(r("MEDIO DENOMINACION")),
                                                          .MAQUINA = SabLib.BLL.Utils.stringNull(r("MAQUINA")), .ID_CARACTERISTICA = SabLib.BLL.Utils.integerNull(r("ID CARACTERISTICA")),
                                                          .HOJA_REGISTROS = SabLib.BLL.Utils.booleanNull(r("HOJA REGISTROS")), .FRECUENCIA_REGISTRO = SabLib.BLL.Utils.stringNull(r("FRECUENCIA REGISTRO")),
                                                          .FRECUENCIA_CONTROL_CAL = SabLib.BLL.Utils.stringNull(r("FRECUENCIA CONTROL CAL")), .FRECUENCIA_CONTROL = SabLib.BLL.Utils.stringNull(r("FRECUENCIA CONTROL")),
                                                          .CONT_CAUSA = SabLib.BLL.Utils.integerNull(r("CONT CAUSA")), .ACCION_RECOMENDADA = SabLib.BLL.Utils.stringNull(r("ACCION RECOMENDADA"))},
                                                          query, CnKaplanIgorre, New SqlClient.SqlParameter("COD_OPERACION", codOpe))

        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idRegistro"></param>
        ''' <returns></returns>
        Public Function cargarAyudaVisual(ByVal idRegistro As Integer) As ELL.AyudaVisual
            Dim query As String = "SELECT * FROM [Archivos] " &
                                  "INNER JOIN [Archivos_Caracteristicas_Plan_FAB] on [Archivos_Caracteristicas_Plan_FAB].Id_Archivo = [Archivos].ID " &
                                  "INNER JOIN [CARACTERISTICAS DEL PLAN FABRICACION] on [CARACTERISTICAS DEL PLAN FABRICACION].ID_REGISTRO = [Archivos_Caracteristicas_Plan_FAB].Id_Carac_Plan " &
                                  "WHERE [CARACTERISTICAS DEL PLAN FABRICACION].ID_REGISTRO = @ID_REGISTRO"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.AyudaVisual)(Function(r As SqlClient.SqlDataReader) _
           New ELL.AyudaVisual With {.ID = CInt(r("ID")), .NOMBRE = SabLib.BLL.Utils.stringNull(r("Nombre")),
                                     .ARCHIVO = SabLib.BLL.Utils.byteNull(r("Archivo")), .ContentType = SabLib.BLL.Utils.stringNull(r("Content_Type"))}, query, CnKaplanIgorre, New SqlClient.SqlParameter("ID_REGISTRO", idRegistro)).FirstOrDefault
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codOpe"></param>
        ''' <returns></returns>
        Public Function cargarNivelPlanFabricacion(ByVal codOpe As String) As String
            Dim query As String = "SELECT [NIVEL PLAN] FROM [NIVELES PLANES DE CONTROL PIEZA FABRICACION] " &
                                  "WHERE [NIVELES PLANES DE CONTROL PIEZA FABRICACION].CODIGO = @COD_OPERACION " &
                                  "ORDER BY FECHA DESC"

            Return Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(query, CnKaplanIgorre, New SqlClient.SqlParameter("COD_OPERACION", codOpe))
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codOpe"></param>
        ''' <returns></returns>
        Public Function cargarHojaInstruccion(ByVal codOpe As String) As String
            Dim query As String = "SELECT [DIBUJO] FROM [HOJA DE INSTRUCCIONES FABRICACION] " &
                                  "WHERE [HOJA DE INSTRUCCIONES FABRICACION].CODIGO = @COD_OPERACION "

            Return Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(query, CnKaplanIgorre, New SqlClient.SqlParameter("COD_OPERACION", codOpe))
        End Function

    End Class

End Namespace