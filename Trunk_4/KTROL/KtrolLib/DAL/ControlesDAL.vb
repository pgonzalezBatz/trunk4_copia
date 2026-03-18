Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration

Namespace DAL

    Public Class ControlesDAL
        Inherits DALBase

#Region "Consultas"
        ''' <summary>
        ''' Obtiene los datos de un control
        ''' </summary>
        ''' <param name="idControl">Id del usuario</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerControl(ByVal idControl As Integer) As ELL.Controles
            Dim query As String = "SELECT ID, COD_OPERACION, ID_USUARIO, ID_PLANTA, TURNO, FECHA, ID_TIPO FROM CONTROLES WHERE ID=:ID_CONTROL" 'OPERARIO, CALIDAD, 
            Dim parameter As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Controles)(Function(r As OracleDataReader) _
            New ELL.Controles With {.Id = CInt(r("ID")), .CodOperacion = SabLib.BLL.Utils.stringNull(r("COD_OPERACION")), .IdUsuario = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")),
                                    .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")), .Turno = SabLib.BLL.Utils.stringNull(r("TURNO")),
                                    .Fecha = SabLib.BLL.Utils.dateTimeNull(r("FECHA")), .IdTipo = SabLib.BLL.Utils.integerNull(r("ID_TIPO"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene la lista de valores de un control de un usuario
        ''' </summary>
        ''' <param name="idControl">Id del control</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerControlValores(ByVal idControl As Integer) As List(Of ELL.ControlesValoresResumen)
            Dim query As String = "SELECT ID_CONTROL, ID_REGISTRO, TIPO, OK_NOK, VALOR, CARAC_PARAM, ESPECIFICACION, POSICION " &
                       "FROM CONTROLES_VALORES WHERE ID_CONTROL=:ID_CONTROL"
            Dim parameter As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ControlesValoresResumen)(Function(r As OracleDataReader) _
            New ELL.ControlesValoresResumen With {.IdControl = SabLib.BLL.Utils.integerNull(r("ID_CONTROL")), .IdRegistro = SabLib.BLL.Utils.integerNull(r("ID_REGISTRO")), .Tipo = SabLib.BLL.Utils.stringNull(r("TIPO")), .OkNok = SabLib.BLL.Utils.stringNull(r("OK_NOK")),
                                                  .Valor = SabLib.BLL.Utils.stringNull(r("VALOR")), .CaracParam = SabLib.BLL.Utils.stringNull(r("CARAC_PARAM")), .Especificacion = SabLib.BLL.Utils.stringNull(r("ESPECIFICACION")), .Posicion = SabLib.BLL.Utils.stringNull(r("POSICION"))}, query, CadenaConexion, parameter)
        End Function

        ''' <summary>
        ''' Obtiene la lista de valores de un control de un usuario
        ''' </summary>
        ''' <param name="idControl">Id del control</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerControlErrores(ByVal idControl As Integer) As ELL.ControlesErrores
            Dim query As String = "SELECT ID_CONTROL, VALIDACION, REPARACION, CAMBIO_REFERENCIA, VALIDACION_USUARIO, COMENTARIO, ID_CONTROL_VALIDACION " &
                                   "FROM CONTROLES_ERRORES " &
                                   "WHERE ID_CONTROL=:ID_CONTROL"
            Dim parameter As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ControlesErrores)(Function(r As OracleDataReader) _
            New ELL.ControlesErrores With {.IdControl = SabLib.BLL.Utils.integerNull(r("ID_CONTROL")), .Validado = SabLib.BLL.Utils.booleanNull(r("VALIDACION")),
                                           .Reparado = SabLib.BLL.Utils.booleanNull(r("REPARACION")), .CambioReferencia = SabLib.BLL.Utils.booleanNull(r("CAMBIO_REFERENCIA")),
                                           .ValidacionUsuario = SabLib.BLL.Utils.integerNull(r("VALIDACION_USUARIO")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")),
                                           .IdControlValidacion = SabLib.BLL.Utils.integerNull(r("ID_CONTROL_VALIDACION"))},
                                           query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene la lista de valores de un control de un usuario
        ''' </summary>
        ''' <param name="idControl">Id del control</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ControlSinErrores(ByVal idControl As Integer) As Boolean
            Dim query As String = "SELECT ID_CONTROL FROM CONTROLES " &
                       "INNER JOIN CONTROLES_ERRORES on CONTROLES.ID = CONTROLES_ERRORES.ID_CONTROL " &
                       "WHERE ID_CONTROL=:ID_CONTROL"

            Dim parameter As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter) = 0
        End Function

        ''' <summary>
        ''' Obtiene la lista de valores de un control de un usuario
        ''' </summary>
        ''' <param name="idControl">Id del control</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CaracteristicasSinErrores(ByVal idControl As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM CONTROLES_VALORES " &
                       "WHERE OK_NOK=0 AND ID_CONTROL=:ID_CONTROL"

            Dim parameter As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter) = 0
        End Function

        ''' <summary>
        ''' Obtiene el último control realizado para un código de operación
        ''' </summary>
        ''' <param name="codOperacion">Código de operación</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerUltimoControlCodigoOperacion(ByVal codOperacion As String) As Integer
            Dim query As String = "SELECT ID " &
                                "FROM CONTROLES " &
                                "WHERE COD_OPERACION = :COD_OPERACION " &
                                "ORDER BY ID DESC "

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, New OracleParameter("COD_OPERACION", OracleDbType.NVarchar2, 50, codOperacion, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Obtiene el último control realizado para un código de operación
        ''' </summary>
        ''' <param name="codOperacion">Código de operación</param>        
        ''' <returns></returns> 
        ''' <remarks></remarks>        
        Public Function ObtenerUltimoControlCodigoOperacionErrores(ByVal codOperacion As String) As ELL.ControlesErrores
            Dim query As String = "SELECT ID_CONTROL, VALIDACION, REPARACION, CAMBIO_REFERENCIA, VALIDACION_USUARIO, COMENTARIO " &
                                "FROM CONTROLES_ERRORES " &
                                "INNER JOIN CONTROLES ON CONTROLES.ID = CONTROLES_ERRORES.ID_CONTROL " &
                                "AND COD_OPERACION = :COD_OPERACION " &
                                "ORDER BY ID DESC"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ControlesErrores)(Function(r As OracleDataReader) _
            New ELL.ControlesErrores With {.IdControl = SabLib.BLL.Utils.integerNull(r("ID_CONTROL")), .Validado = SabLib.BLL.Utils.booleanNull(r("VALIDACION")),
                                           .Reparado = SabLib.BLL.Utils.booleanNull(r("REPARACION")), .CambioReferencia = SabLib.BLL.Utils.booleanNull(r("CAMBIO_REFERENCIA")),
                                           .ValidacionUsuario = SabLib.BLL.Utils.integerNull(r("VALIDACION_USUARIO")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO"))},
                                           query, CadenaConexion, New OracleParameter("COD_OPERACION", OracleDbType.NVarchar2, 50, codOperacion, ParameterDirection.Input)).FirstOrDefault()
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar un control nuevo y sus valores
        ''' </summary>
        ''' <param name="control">Objeto Controles</param>
        ''' <param name="controlValores">Objeto ControlesValores</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarControlYValores(ByVal control As ELL.Controles, ByVal controlValores As List(Of ELL.ControlesValores), ByVal controlErrores As ELL.ControlesErrores) As Integer
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As List(Of OracleParameter) = Nothing
            Dim lParameters3 As New List(Of OracleParameter)
            Dim idControl As Integer = 0

            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing

            Try
                con = New OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                query = "INSERT INTO CONTROLES(COD_OPERACION, ID_USUARIO, ID_PLANTA, INFO_PIEZA, TURNO, NIVEL_PLAN, ID_TIPO) " &
                    "VALUES(:COD_OPERACION, :ID_USUARIO, :ID_PLANTA, :INFO_PIEZA, :TURNO, : NIVEL_PLAN, :ID_TIPO) returning ID into :RETURN_VALUE"
                lParameters1.Add(New OracleParameter("COD_OPERACION", OracleDbType.NVarchar2, 50, control.CodOperacion, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, control.IdUsuario, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, control.IdPlanta, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("INFO_PIEZA", OracleDbType.NVarchar2, 200, control.InfoPieza, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("TURNO", OracleDbType.Varchar2, 1, control.Turno, ParameterDirection.Input))
                If Not (String.IsNullOrEmpty(control.NivelPlan)) Then
                    lParameters1.Add(New OracleParameter("NIVEL_PLAN", OracleDbType.Varchar2, 10, control.NivelPlan, ParameterDirection.Input))
                Else
                    lParameters1.Add(New OracleParameter("NIVEL_PLAN", OracleDbType.Varchar2, 10, DBNull.Value, ParameterDirection.Input))
                End If
                lParameters1.Add(New OracleParameter("ID_TIPO", OracleDbType.Int32, control.IdTipo, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters1.Add(p)

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters1.ToArray)

                'idControl = lParameters1.Item(9).Value
                idControl = lParameters1.Item(7).Value

                If ((idControl <> Integer.MinValue) AndAlso (idControl > 0)) Then
                    'Guardamos los valores
                    For i As Integer = 0 To controlValores.Count - 1
                        query = "INSERT INTO CONTROLES_VALORES(ID_CONTROL, ID_REGISTRO, TIPO, OK_NOK, VALOR,  " &
                        "POSICION, ORDEN_CARAC, CARAC_PARAM, ESPECIFICACION, CLASE) " &
                        "VALUES (:ID_CONTROL, :ID_REGISTRO, :TIPO, :OK_NOK, :VALOR, " &
                        ":POSICION, :ORDEN_CARAC, :CARAC_PARAM, :ESPECIFICACION, :CLASE)"
                        lParameters2 = New List(Of OracleParameter)

                        lParameters2.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ID_REGISTRO", OracleDbType.Int32, controlValores(i).IdRegistro, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("TIPO", OracleDbType.Varchar2, 1, controlValores(i).Tipo, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("OK_NOK", OracleDbType.Int16, 1, controlValores(i).OkNok, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("VALOR", OracleDbType.Double, CDbl(controlValores(i).Valor), ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("OPERACION", OracleDbType.Int32, If(controlValores(i).Operacion = Integer.MinValue, DBNull.Value, controlValores(i).Operacion), ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("MAQUINA", OracleDbType.NVarchar2, 25, controlValores(i).Maquina, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("POSICION", OracleDbType.Varchar2, 10, controlValores(i).Posicion, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ORDEN_CARAC", OracleDbType.Single, controlValores(i).OrdenCarac, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("CARAC_PARAM", OracleDbType.NVarchar2, 100, controlValores(i).CaracParam, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ESPECIFICACION", OracleDbType.NVarchar2, 50, controlValores(i).Especificacion, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("FRECUENCIA_CONTROL", OracleDbType.NVarchar2, 50, controlValores(i).FrecuenciaControl, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("FRECUENCIA_REGISTRO", OracleDbType.NVarchar2, 50, controlValores(i).FrecuenciaRegistro, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("METODO_EVALUACION", OracleDbType.NVarchar2, 50, controlValores(i).MetodoEvaluacion, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("RESPONSABLE", OracleDbType.NVarchar2, 20, controlValores(i).Responsable, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("MEDIO_DENOMINACION", OracleDbType.NVarchar2, 20, controlValores(i).MedioDenominacion, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("MEDIO_RFA", OracleDbType.NVarchar2, 50, controlValores(i).MedioRFA, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("CLASE", OracleDbType.NVarchar2, 50, controlValores(i).Clase, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("OBSERVACIONES", OracleDbType.NVarchar2, 1000, controlValores(i).Observaciones, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("ACCION_RECOMENDADA", OracleDbType.NVarchar2, 100, controlValores(i).AccionRecomendada, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("RESPONSABLE_REGISTRO", OracleDbType.NVarchar2, 20, controlValores(i).ResponsableRegistro, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("CONT_CAUSA", OracleDbType.Int32, controlValores(i).ContCausa, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("ID_CARACTERISTICA", OracleDbType.Int32, controlValores(i).IdCaracteristica, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("PROCEDE_DE", OracleDbType.NVarchar2, 20, controlValores(i).ProcedeDe, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("PROCESO_PRODUCTO", OracleDbType.NVarchar2, 50, controlValores(i).ProcesoProducto, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("TAMAÑO", OracleDbType.NVarchar2, 50, controlValores(i).Tamaño, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("METODO_CONTROL", OracleDbType.NVarchar2, 50, controlValores(i).MetodoControl, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("HOJA_REGISTROS", OracleDbType.Int16, 1, controlValores(i).HojaRegistros, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("VER_REG_REC", OracleDbType.Int16, 1, controlValores(i).VerRegRec, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("VER_REG_PRO", OracleDbType.Int16, 1, controlValores(i).VerRegPro, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("VER_REG_DIM", OracleDbType.Int16, 1, controlValores(i).VerRegDim, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("VER_REG_MAT", OracleDbType.Int16, 1, controlValores(i).VerRegMat, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("VER_REG_FUN", OracleDbType.Int16, 1, controlValores(i).VerRegFun, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("MAXIM", OracleDbType.Single, If(controlValores(i).Maxim = Single.MinValue, DBNull.Value, controlValores(i).Maxim), ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("MINIM", OracleDbType.Single, If(controlValores(i).Minim = Single.MinValue, DBNull.Value, controlValores(i).Minim), ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("FRECUENCIA_CONTROL_CAL", OracleDbType.NVarchar2, 50, controlValores(i).FrecuenciaControlCal, ParameterDirection.Input))
                        'lParameters2.Add(New OracleParameter("TAMAÑO_CAL", OracleDbType.NVarchar2, 50, controlValores(i).TamañoCal, ParameterDirection.Input))

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters2.ToArray)
                    Next

                    'En caso de haber errores, también se guardan
                    If (controlErrores IsNot Nothing) Then
                        'Dim lParameters3 As New List(Of OracleParameter)
                        query = "INSERT INTO CONTROLES_ERRORES(ID_CONTROL, VALIDACION, REPARACION, CAMBIO_REFERENCIA, VALIDACION_USUARIO, COMENTARIO) " &
                        "VALUES(:ID_CONTROL, :VALIDACION, :REPARACION, :CAMBIO_REFERENCIA, :VALIDACION_USUARIO, :COMENTARIO)"
                        lParameters3.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input))
                        lParameters3.Add(New OracleParameter("VALIDACION", OracleDbType.Int16, 1, controlErrores.Validado, ParameterDirection.Input))
                        lParameters3.Add(New OracleParameter("REPARACION", OracleDbType.Int16, 1, controlErrores.Reparado, ParameterDirection.Input))
                        lParameters3.Add(New OracleParameter("CAMBIO_REFERENCIA", OracleDbType.Int16, 1, controlErrores.CambioReferencia, ParameterDirection.Input))
                        If (controlErrores.ValidacionUsuario <> Integer.MinValue) Then
                            lParameters3.Add(New OracleParameter("VALIDACION_USUARIO", OracleDbType.Int32, controlErrores.ValidacionUsuario, ParameterDirection.Input))
                        Else
                            lParameters3.Add(New OracleParameter("VALIDACION_USUARIO", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
                        End If
                        If Not (String.IsNullOrEmpty(controlErrores.Comentario)) Then
                            lParameters3.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, 1000, controlErrores.Comentario, ParameterDirection.Input))
                        Else
                            lParameters3.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, 1000, DBNull.Value, ParameterDirection.Input))
                        End If

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters3.ToArray)
                    End If

                    'Si el control anterior de este código de operación tuvo errores, se indica que este nuevo control será el que validará el anterior
                    Dim oControlesBLL As New BLL.ControlesBLL
                    If (oControlesBLL.UltimoControlConErrores(control.CodOperacion)) Then
                        lParameters1.Clear()
                        Dim controlError As ELL.ControlesErrores
                        controlError = ObtenerUltimoControlCodigoOperacionErrores(control.CodOperacion)
                        query = "UPDATE CONTROLES_ERRORES SET ID_CONTROL_VALIDACION=:ID_CONTROL_VALIDACION WHERE ID_CONTROL=:ID_CONTROL"
                        lParameters1.Add(New OracleParameter("ID_CONTROL_VALIDACION", OracleDbType.Int32, idControl, ParameterDirection.Input))
                        lParameters1.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, controlError.IdControl, ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters1.ToArray)
                    End If

                    transact.Commit()
                Else
                    transact.Rollback()
                    idControl = 0
                End If
            Catch ex As Exception
                transact.Rollback()
                Dim msg As String = String.Format("SQL: {0} " & vbCrLf & "lParameters1: {1}" & vbCrLf & "lParameters2: {2}" & vbCrLf & "lParameters3: {3}",
                                                  query,
                                                  If(lParameters1 Is Nothing OrElse Not lParameters1.Any, String.Empty, String.Join("; ", From Reg In lParameters1 Select New String(Reg.ParameterName & ":" & Reg.Value))),
                                                  If(lParameters2 Is Nothing OrElse Not lParameters2.Any, String.Empty, String.Join("; ", From Reg In lParameters2 Select New String(Reg.ParameterName & ":" & Reg.Value))),
                                                  If(lParameters3 Is Nothing OrElse Not lParameters3.Any, String.Empty, String.Join("; ", From Reg In lParameters3 Select New String(Reg.ParameterName & ":" & Reg.Value))))
                log.Error(msg, ex)
                idControl = 0
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try

            Return idControl
        End Function

        ''' <summary>
        ''' Guardar el valor de una característica
        ''' </summary>
        ''' <param name="control">Objeto ControlesValoresResumen</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarValorControl(ByVal control As ELL.ControlesValoresResumen) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "UPDATE CONTROLES_VALORES SET OK_NOK=:OK_NOK, VALOR=:VALOR WHERE ID_CONTROL=:ID_CONTROL AND ID_REGISTRO=:ID_REGISTRO"
                lParameters1.Add(New OracleParameter("OK_NOK", OracleDbType.Int16, 1, control.OkNok, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("VALOR", OracleDbType.Double, control.Valor, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, control.IdControl, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_REGISTRO", OracleDbType.Int32, control.IdRegistro, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                resultado = True
            Catch ex As Exception
                resultado = False
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Modificar el valor de un registro de un control y guardar el error de este control
        ''' </summary>
        ''' <param name="controlError">Objeto ControlesErrores</param>
        ''' <param name="controlValor">Objeto ControlesValoresResumen</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarErrorYValorControl(ByVal controlError As ELL.ControlesErrores, ByVal controlValor As ELL.ControlesValoresResumen) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing

            Try
                con = New OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                query = "UPDATE CONTROLES_VALORES SET OK_NOK=:OK_NOK, VALOR=:VALOR WHERE ID_CONTROL=:ID_CONTROL AND ID_REGISTRO=:ID_REGISTRO"
                lParameters1.Add(New OracleParameter("OK_NOK", OracleDbType.Int16, 1, controlValor.OkNok, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("VALOR", OracleDbType.Double, controlValor.Valor, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, controlValor.IdControl, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_REGISTRO", OracleDbType.Int32, controlValor.IdRegistro, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters1.ToArray)

                query = "INSERT INTO CONTROLES_ERRORES(ID_CONTROL, VALIDACION, REPARACION, CAMBIO_REFERENCIA, COMENTARIO, VALIDACION_USUARIO) VALUES (:ID_CONTROL, :VALIDACION, :REPARACION, :CAMBIO_REFERENCIA, :COMENTARIO, :VALIDACION_USUARIO)"
                lParameters2.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, controlValor.IdControl, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("VALIDACION", OracleDbType.Int16, 1, controlError.Validado, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("REPARACION", OracleDbType.Int16, 1, controlError.Reparado, ParameterDirection.Input))
                lParameters2.Add(New OracleParameter("CAMBIO_REFERENCIA", OracleDbType.Int16, 1, controlError.CambioReferencia, ParameterDirection.Input))
                If (controlError.ValidacionUsuario <> Integer.MinValue) Then
                    lParameters2.Add(New OracleParameter("VALIDACION_USUARIO", OracleDbType.Int32, controlError.ValidacionUsuario, ParameterDirection.Input))
                Else
                    lParameters2.Add(New OracleParameter("VALIDACION_USUARIO", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
                End If
                If Not (String.IsNullOrEmpty(controlError.Comentario)) Then
                    lParameters2.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, 1000, controlError.Comentario, ParameterDirection.Input))
                Else
                    lParameters2.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, 1000, DBNull.Value, ParameterDirection.Input))
                End If

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters2.ToArray)

                transact.Commit()

                resultado = True
            Catch ex As Exception
                transact.Rollback()
                resultado = False
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Modificar los datos de error de un control
        ''' </summary>
        ''' <param name="controlError">Objeto ControlesErrores</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarError(ByVal controlError As ELL.ControlesErrores) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)

            Try
                query = "UPDATE CONTROLES_ERRORES SET VALIDACION=:VALIDACION, REPARACION=:REPARACION, CAMBIO_REFERENCIA=:CAMBIO_REFERENCIA, COMENTARIO=:COMENTARIO, VALIDACION_USUARIO=:VALIDACION_USUARIO WHERE ID_CONTROL=:ID_CONTROL"
                lParameters1.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, controlError.IdControl, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("VALIDACION", OracleDbType.Int16, 1, controlError.Validado, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("REPARACION", OracleDbType.Int16, 1, controlError.Reparado, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("CAMBIO_REFERENCIA", OracleDbType.Int16, 1, controlError.CambioReferencia, ParameterDirection.Input))
                If (controlError.ValidacionUsuario <> Integer.MinValue) Then
                    lParameters1.Add(New OracleParameter("VALIDACION_USUARIO", OracleDbType.Int32, controlError.ValidacionUsuario, ParameterDirection.Input))
                Else
                    lParameters1.Add(New OracleParameter("VALIDACION_USUARIO", OracleDbType.Int32, DBNull.Value, ParameterDirection.Input))
                End If
                If Not (String.IsNullOrEmpty(controlError.Comentario)) Then
                    lParameters1.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, 1000, controlError.Comentario, ParameterDirection.Input))
                Else
                    lParameters1.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, 1000, DBNull.Value, ParameterDirection.Input))
                End If

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters1.ToArray)

                resultado = True
            Catch ex As Exception
                resultado = False
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Eliminar el error de un control
        ''' </summary>
        ''' <param name="idControl">Identificador del control</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarControlError(ByVal idControl As Integer) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty

            Try
                query = "DELETE FROM CONTROLES_ERRORES WHERE ID_CONTROL=:ID_CONTROL"
                Dim parameter As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, parameter)

                resultado = True
            Catch ex As Exception
                resultado = False
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Eliminar los datos de un control
        ''' </summary>
        ''' <param name="idControl"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarControl(ByVal idControl As Integer) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty

            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing

            Try
                con = New OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                query = "DELETE FROM CONTROLES_ERRORES WHERE ID_CONTROL=:ID_CONTROL"
                Dim parameter1 As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, con, parameter1)

                query = "DELETE FROM CONTROLES_VALORES WHERE ID_CONTROL=:ID_CONTROL"
                Dim parameter2 As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, con, parameter2)

                query = "DELETE FROM CONTROLES WHERE ID=:ID_CONTROL"
                Dim parameter4 As New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, con, parameter4)

                transact.Commit()

                resultado = True
            Catch ex As Exception
                transact.Rollback()
                resultado = False
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace