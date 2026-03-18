Namespace DAL

    Public Class DepartamentosDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Obtiene la conexion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Conexion As String
            Get
                Dim status As String = "BIDAIAKTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            cn = Conexion
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un departamento
        ''' </summary>
        ''' <param name="id">Id</param>        
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.Departamento
            Try
                Dim query As String = "SELECT ID,NOMBRE,OBSOLETO,ID_PLANTA,CUENTA_18,CUENTA_8,CUENTA_0,OF_IMPRODUCTIVA FROM DEPARTAMENTOS WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Dim lDept As List(Of ELL.Departamento) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Departamento)(Function(r As OracleDataReader) _
                 New ELL.Departamento With {.CodigoDepartamento = r(0), .Departamento = r(1), .Obsoleta = CInt(r(2)), .IdPlanta = CInt(r(3)),
                                              .Cuenta18 = SabLib.BLL.Utils.stringNull(r(4)), .Cuenta8 = SabLib.BLL.Utils.stringNull(r(5)),
                                              .Cuenta0 = SabLib.BLL.Utils.stringNull(r(6)), .OFImproductiva = SabLib.BLL.Utils.stringNull(r(7))}, query, cn, parameter)
                Dim oDept As ELL.Departamento = Nothing
                If (lDept IsNot Nothing AndAlso lDept.Count > 0) Then oDept = lDept.Item(0)
                Return oDept
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del departamento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de departamento
        ''' </summary>
        ''' <param name="oDept">Objeto cuenta con las condiciones </param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bMostrarObsoletos">Muestra los obsoletos o todos</param>
        ''' <returns></returns>  
        Public Function loadList(ByVal oDept As ELL.Departamento, ByVal idPlanta As Integer, ByVal bMostrarObsoletos As Boolean) As List(Of ELL.Departamento)
            Try
                Dim query As String = "SELECT ID,NOMBRE,OBSOLETO,ID_PLANTA,CUENTA_18,CUENTA_8,CUENTA_0,OF_IMPRODUCTIVA FROM DEPARTAMENTOS WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

                If (oDept.CodigoDepartamento <> String.Empty) Then
                    If (oDept.Cuenta0 > 0) Then  'Para hacer la busqueda conjunta
                        lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.NVarchar2, oDept.CodigoDepartamento, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("CUENTA_18", OracleDbType.Int32, oDept.Cuenta18, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("CUENTA_8", OracleDbType.Int32, oDept.Cuenta8, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("CUENTA_0", OracleDbType.Int32, oDept.Cuenta0, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("OF_IMPRODUCTIVA", OracleDbType.Int32, oDept.Cuenta0, ParameterDirection.Input))
                        query &= " AND (ID=:COD_DEPART OR (CUENTA_18=:CUENTA_18 OR CUENTA_8=:CUENTA_8 OR CUENTA_0=:CUENTA_0 OR OF_IMPRODUCTIVA=:OF_IMPRODUCTIVA) OR LOWER(NOMBRE) LIKE  '%' || :COD_DEPART || '%')"  'Tambien se busca en la descripcion el codigo departamento por si se quiere buscar 110007
                    Else  'Busqueda simple
                        lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.NVarchar2, oDept.CodigoDepartamento, ParameterDirection.Input))
                        query &= " AND ID=:COD_DEPART"
                    End If
                End If
                If (oDept.Departamento <> String.Empty) Then
                    lParametros.Add(New OracleParameter("DEPARTAMENTO", OracleDbType.Varchar2, oDept.Departamento.ToLower, ParameterDirection.Input))
                    query &= " AND LOWER(NOMBRE) LIKE  '%' || :DEPARTAMENTO || '%'"
                End If
                If (Not bMostrarObsoletos) Then query &= " AND OBSOLETO=0"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Departamento)(Function(r As OracleDataReader) _
                 New ELL.Departamento With {.CodigoDepartamento = r(0), .Departamento = r(1), .Obsoleta = CInt(r(2)), .IdPlanta = CInt(r(3)),
                                              .Cuenta18 = SabLib.BLL.Utils.stringNull(r(4)), .Cuenta8 = SabLib.BLL.Utils.stringNull(r(5)),
                                              .Cuenta0 = SabLib.BLL.Utils.stringNull(r(6)), .OFImproductiva = SabLib.BLL.Utils.stringNull(r(7))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de departamentos", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta los departamentos
        ''' </summary>
        ''' <param name="lDept">Lista de departamentos</param>        
        Public Sub Add(ByVal lDept As List(Of ELL.Departamento))
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim query As String = "INSERT INTO DEPARTAMENTOS(ID,NOMBRE,OBSOLETO,ID_PLANTA,CUENTA_18,CUENTA_8,CUENTA_0,OF_IMPRODUCTIVA) VALUES(:ID,:NOMBRE,0,:ID_PLANTA,:CUENTA_18,:CUENTA_8,:CUENTA_0,:OF_IMPRODUCTIVA)"
                Dim lParametros As List(Of OracleParameter)

                con = New OracleConnection(cn)
                con.Open()
                transact = con.BeginTransaction()

                For Each oCuenta As ELL.Departamento In lDept
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oCuenta.CodigoDepartamento, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, oCuenta.Departamento, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oCuenta.IdPlanta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CUENTA_18", OracleDbType.Int32, oCuenta.Cuenta18, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CUENTA_8", OracleDbType.Int32, oCuenta.Cuenta8, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CUENTA_0", OracleDbType.Int32, oCuenta.Cuenta0, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("OF_IMPRODUCTIVA", OracleDbType.Int32, oCuenta.OFImproductiva, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParametros.ToArray)
                Next
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al añadir el departamento", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Actualiza una cuenta contable
        ''' </summary>
        ''' <param name="oCuenta">Modifica el departamento</param>        
        Public Sub Update(ByVal oCuenta As ELL.Departamento)
            Try
                Dim query As String = "UPDATE DEPARTAMENTOS SET CUENTA_18=:CUENTA_18,CUENTA_8=:CUENTA_8,CUENTA_0=:CUENTA_0,OF_IMPRODUCTIVA=:OF_IMPRODUCTIVA WHERE ID=:ID"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("CUENTA_18", OracleDbType.Int32, oCuenta.Cuenta18, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CUENTA_8", OracleDbType.Int32, oCuenta.Cuenta8, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CUENTA_0", OracleDbType.Int32, oCuenta.Cuenta0, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("OF_IMPRODUCTIVA", OracleDbType.Int32, oCuenta.OFImproductiva, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oCuenta.CodigoDepartamento, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar el departamento", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Marca como obsoleto la lista de departamentos
        ''' </summary>
        ''' <param name="lDept">Lista de ids de departamentos</param>         
        Public Sub Delete(ByVal lDept As List(Of Integer))
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                con = New OracleConnection(cn)
                con.Open()
                transact = con.BeginTransaction()

                For Each idCuenta As Integer In lDept
                    Delete(idCuenta, con)
                Next
                transact.Commit()
            Catch batzEx As BidaiakLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al eliminar el departamento", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Marca como obsoleto el departamento
        ''' </summary>
        ''' <param name="id">Id del objeto</param> 
        ''' <param name="myCon">Parametro opcional:Conexion si tiene transferencia</param>        
        Public Sub Delete(ByVal id As Integer, Optional ByVal myCon As OracleConnection = Nothing)
            Try
                Dim query As String = "UPDATE DEPARTAMENTOS SET OBSOLETO=1 WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                If (myCon Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, myCon, parameter)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar el departamento", ex)
            End Try
        End Sub

#End Region

#Region "Cuentas plantas filiales"

        ''' <summary>
        ''' Obtiene la cuenta contable relacionada con una planta
        ''' </summary>
        ''' <param name="idPlantaGestion">Id de la planta que gestiona las cuentas</param>
        ''' <param name="idPlantaCta">Id de la planta a la que se le asigna las cuentas</param>
        ''' <returns></returns>        
        Function loadCuentaPlantaFilial(ByVal idPlantaGestion As Integer, ByVal idPlantaCta As Integer) As ELL.CuentaContable
            Try
                Dim query As String = "SELECT CUENTA_18,CUENTA_8,CUENTA_0,OF_IMPRODUCTIVA,ID_PLANTA,OF_IMPRODUCTIVA,ID_PLANTAGESTION FROM CUENTAS_PLANTAS_FILIALES WHERE ID_PLANTA=:ID_PLANTA AND ID_PLANTAGESTION=:ID_PLANTAGESTION"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlantaCta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, idPlantaGestion, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CuentaContable)(Function(r As OracleDataReader) _
                 New ELL.CuentaContable With {.Cuenta18 = CInt(r("CUENTA_18")), .Cuenta8 = CInt(r("CUENTA_8")), .Cuenta0 = CInt(r("CUENTA_0")), .IdPlantaCuenta = CInt(r("ID_PLANTA")), .IdPlantaGestion = CInt(r("ID_PLANTAGESTION"))}, query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BatzException("Error al obtener la informacion de la cuenta contable de la planta filial", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de cuentas contables de las plantas filiales
        ''' </summary>
        ''' <param name="idPlantaGestion">Id de la planta de gestion</param>   
        ''' <returns></returns>        
        Function loadCuentasPlantasFilialesList(ByVal idPlantaGestion As Integer) As List(Of ELL.CuentaContable)
            Try
                Dim query As String = "SELECT P.ID AS ID_PLANTA,P.NOMBRE AS NOMBRE_PLANTA,C.CUENTA_18,C.CUENTA_8,C.CUENTA_0,OF_IMPRODUCTIVA,C.ID_PLANTAGESTION FROM SAB.PLANTAS P LEFT JOIN CUENTAS_PLANTAS_FILIALES C ON (C.ID_PLANTA=P.ID AND C.ID_PLANTAGESTION=:ID_PLANTAGESTION)"
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CuentaContable)(Function(r As OracleDataReader) _
                 New ELL.CuentaContable With {.Cuenta18 = SabLib.BLL.Utils.integerNull(r("CUENTA_18")), .Cuenta8 = SabLib.BLL.Utils.integerNull(r("CUENTA_8")), .Cuenta0 = SabLib.BLL.Utils.integerNull(r("CUENTA_0")), .IdPlantaCuenta = CInt(r("ID_PLANTA")),
                 .NombrePlanta = CStr(r("NOMBRE_PLANTA")), .IdPlantaGestion = SabLib.BLL.Utils.integerNull(r("ID_PLANTAGESTION"))}, query, cn, New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, idPlantaGestion, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BatzException("Error al obtener el listado de cuentas contables de la planta filial", ex)
            End Try
        End Function

        ''' <summary>
        ''' Actualiza la cuenta de la planta
        ''' </summary>
        ''' <param name="oCuenta">Informacion de la cuenta</param>        
        Sub UpdateCuentaPlantaFilial(ByVal oCuenta As ELL.CuentaContable)
            Try
                Dim query As String = "SELECT COUNT(ID_PLANTA) FROM CUENTAS_PLANTAS_FILIALES WHERE ID_PLANTA=:ID_PLANTA AND ID_PLANTAGESTION=:ID_PLANTAGESTION"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oCuenta.IdPlantaCuenta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, oCuenta.IdPlantaGestion, ParameterDirection.Input))

                If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) = 0) Then 'Insert
                    query = "INSERT INTO CUENTAS_PLANTAS_FILIALES(ID_PLANTA,CUENTA_18,CUENTA_8,CUENTA_0,OF_IMPRODUCTIVA,ID_PLANTAGESTION) VALUES (:ID_PLANTA,:CUENTA_18,:CUENTA_8,:CUENTA_0,:OF_IMPRODUCTIVA,:ID_PLANTAGESTION)"
                Else 'update
                    query = "UPDATE CUENTAS_PLANTAS_FILIALES SET CUENTA_18=:CUENTA_18,CUENTA_8=:CUENTA_8,CUENTA_0=:CUENTA_0,OF_IMPRODUCTIVA=:OF_IMPRODUCTIVA WHERE ID_PLANTA=:ID_PLANTA AND ID_PLANTAGESTION=:ID_PLANTAGESTION"
                End If
                lParametros = New List(Of OracleParameter)()
                lParametros.Add(New OracleParameter("CUENTA_18", OracleDbType.Int32, oCuenta.Cuenta18, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CUENTA_8", OracleDbType.Int32, oCuenta.Cuenta8, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CUENTA_0", OracleDbType.Int32, oCuenta.Cuenta0, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("OF_IMPRODUCTIVA", OracleDbType.Int32, "1", ParameterDirection.Input))  'No se suele informar este campo. Como es obligatorio, le metemos un 1
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oCuenta.IdPlantaCuenta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, oCuenta.IdPlantaGestion, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al actualizar la cuenta de la planta", ex)
            End Try
        End Sub

#End Region

#Region "Asientos contables Facturas Eroski"

        ''' <summary>
        ''' Obtiene los asientos contables temporales de las facturas de Eroski
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="codDepto">Parametro opcional con el codigo de departamento a mostrar</param>
        ''' <param name="bAgrupUO">Parametro opcional que indica si se devolveran los asientos agrupados por uo o no</param>
        ''' <param name="factura">Parametro opcional que indica si se buscaran las de una factura en concreto o no</param>
        ''' <returns></returns>        
        Function loadAsientosContEroskiTmp(ByVal idPlanta As Integer, ByVal codDepto As Integer, ByVal bAgrupUO As Boolean, ByVal factura As String) As List(Of ELL.AsientoContableEroskiTmp)
            Try
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Dim query As String = String.Empty
                If (bAgrupUO) Then
                    query &= "SELECT 0 AS ID_SAB, 0 AS NUM_TRA,'' AS NOMBRE,COD_DEPART,DEPARTAMENTO,SUM(BASEEXE_0) AS BASEEXE_0,SUM(BASEIR_8) AS BASEIR_8,SUM(BASEIG_18) AS BASEIG_18,ID_PLANTA,UNIDAD_ORGANIZATIVA,SUM(CUOTAG_18) AS CUOTAG_18,SUM(CUOTAR_8) AS CUOTAR_8,SUM(CUOTARE_0) AS CUOTARE_0,SUM(REGESP) AS REGESP,CUENTA_18,CUENTA_8,CUENTA_0,FACTURA FROM TMP_ASIENTO_CONT WHERE ID_PLANTA=:ID_PLANTA "
                    If (codDepto = 0) Then
                        query &= "AND COD_DEPART IS NULL "
                    ElseIf (codDepto > 0) Then
                        query &= "AND COD_DEPART=:COD_DEPART "
                        lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Int32, codDepto, ParameterDirection.Input))
                    End If
                    If (factura <> String.Empty) Then
                        query &= "AND FACTURA=:FACTURA"
                        lParametros.Add(New OracleParameter("FACTURA", OracleDbType.Int32, factura, ParameterDirection.Input))
                    End If
                    query &= "GROUP BY 0,0, '',COD_DEPART,DEPARTAMENTO,ID_PLANTA, UNIDAD_ORGANIZATIVA,CUENTA_18,CUENTA_8,CUENTA_0,FACTURA " _
                          & "ORDER BY FACTURA,UNIDAD_ORGANIZATIVA,DEPARTAMENTO"
                Else
                    query = "SELECT ID_SAB,NUM_TRA,NOMBRE,COD_DEPART,DEPARTAMENTO,BASEEXE_0,BASEIR_8,BASEIG_18,FECHA_INSERCION,ID_PLANTA,UNIDAD_ORGANIZATIVA,CUOTAG_18,CUOTAR_8,CUOTARE_0,REGESP,CUENTA_18,CUENTA_8,CUENTA_0,FACTURA,PRODUCTO FROM TMP_ASIENTO_CONT WHERE ID_PLANTA=:ID_PLANTA "
                    If (codDepto = 0) Then
                        query &= "AND COD_DEPART IS NULL "
                    ElseIf (codDepto > 0) Then
                        query &= "AND COD_DEPART=:COD_DEPART "
                        lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Int32, codDepto, ParameterDirection.Input))
                    End If
                    If (factura <> String.Empty) Then
                        query &= "AND FACTURA=:FACTURA "
                        lParametros.Add(New OracleParameter("FACTURA", OracleDbType.NVarchar2, factura, ParameterDirection.Input))
                    End If
                    query &= "ORDER BY NUM_TRA"
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.AsientoContableEroskiTmp)(Function(r As OracleDataReader) _
                            New ELL.AsientoContableEroskiTmp With {.IdSab = CInt(r("ID_SAB")), .numTrabajador = CInt(r("NUM_TRA")), .Nombre = SabLib.BLL.Utils.stringNull(r("NOMBRE")), .CodigoDepart = SabLib.BLL.Utils.stringNull(r("COD_DEPART")), .Departamento = SabLib.BLL.Utils.stringNull(r("DEPARTAMENTO")), .BaseExe_0 = SabLib.BLL.Utils.decimalNull(r("BASEEXE_0")), .BaseIR_8 = SabLib.BLL.Utils.decimalNull(r("BASEIR_8")),
                                                                 .BaseIG_18 = SabLib.BLL.Utils.decimalNull(r("BASEIG_18")), .UnidadOrganizativa = SabLib.BLL.Utils.stringNull(r("UNIDAD_ORGANIZATIVA")), .Cuota_18 = SabLib.BLL.Utils.decimalNull(r("CUOTAG_18")), .Cuota_8 = SabLib.BLL.Utils.decimalNull(r("CUOTAR_8")), .Cuota_0 = SabLib.BLL.Utils.decimalNull(r("CUOTARE_0")), .RegEsp = SabLib.BLL.Utils.decimalNull(r("REGESP")),
                                                                 .Cuenta_18 = SabLib.BLL.Utils.integerNull(r("CUENTA_18")), .Cuenta_8 = SabLib.BLL.Utils.integerNull(r("CUENTA_8")), .Cuenta_0 = SabLib.BLL.Utils.integerNull(r("CUENTA_0")), .Factura = r("FACTURA"), .FechaInsercion = If(bAgrupUO, DateTime.MinValue, CDate(r("FECHA_INSERCION"))), .IdPlanta = CInt(r("ID_PLANTA")), .Producto = If(bAgrupUO, String.Empty, SabLib.BLL.Utils.stringNull(r("PRODUCTO")))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener los asientos contables temporales de las facturas Eroski", ex)
            End Try
        End Function

        ''' <summary>
        ''' Borra los registros de la tabla temporal de asientos de las facturas de Eroski
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        Sub DeleteAsientosContEroskiTmp(ByVal idPlanta As Integer)
            Try
                Dim query As String = "DELETE FROM TMP_ASIENTO_CONT WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BatzException("Error al borrar los registros de la tabla temporal de asientos contables de las facturas Eroski", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Inserta los asiento contable en la tabla temporal de asientos de las facturas de Eroski
        ''' </summary>
        ''' <param name="lAsientos">Asientos a insertar</param>       
        ''' <param name="myCon">Parametro opcional con la conexion por si esta en una transaccion</param>
        Sub SaveAsientosContEroskiTmp(ByVal lAsientos As List(Of ELL.AsientoContableEroskiTmp), ByVal myCon As OracleConnection)
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                If (myCon Is Nothing) Then
                    con = New OracleConnection(Conexion)
                    con.Open()
                    transact = con.BeginTransaction()
                Else
                    con = myCon
                End If
                Dim query As String = "INSERT INTO TMP_ASIENTO_CONT(ID_SAB,NUM_TRA,NOMBRE,COD_DEPART,DEPARTAMENTO,BASEEXE_0,BASEIR_8,BASEIG_18,FECHA_INSERCION,ID_PLANTA,UNIDAD_ORGANIZATIVA,CUOTAG_18,CUOTAR_8,CUOTARE_0,REGESP,CUENTA_18,CUENTA_8,CUENTA_0,PRODUCTO,FACTURA) VALUES " _
                                                                & "(:ID_SAB,:NUM_TRA,:NOMBRE,:COD_DEPART,:DEPARTAMENTO,:BASEEXE_0,:BASEIR_8,:BASEIG_18,:FECHA_INSERCION,:ID_PLANTA,:UNIDAD_ORGANIZATIVA,:CUOTAG_18,:CUOTAR_8,:CUOTARE_0,:REGESP,:CUENTA_18,:CUENTA_8,:CUENTA_0,:PRODUCTO,:FACTURA)"
                Dim parametros As List(Of OracleParameter) = Nothing
                For Each oAsiento As ELL.AsientoContableEroskiTmp In lAsientos
                    parametros = New List(Of OracleParameter)
                    parametros.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, oAsiento.IdSab, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("NUM_TRA", OracleDbType.Int32, oAsiento.numTrabajador, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, oAsiento.Nombre, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("COD_DEPART", OracleDbType.NVarchar2, SabLib.BLL.Utils.stringNull(oAsiento.CodigoDepart), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("DEPARTAMENTO", OracleDbType.NVarchar2, SabLib.BLL.Utils.stringNull(oAsiento.Departamento), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("BASEEXE_0", OracleDbType.Decimal, oAsiento.BaseExe_0, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("BASEIR_8", OracleDbType.Decimal, oAsiento.BaseIR_8, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("BASEIG_18", OracleDbType.Decimal, oAsiento.BaseIG_18, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, oAsiento.FechaInsercion, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oAsiento.IdPlanta, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("UNIDAD_ORGANIZATIVA", OracleDbType.NVarchar2, oAsiento.UnidadOrganizativa, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("CUOTAG_18", OracleDbType.Decimal, oAsiento.Cuota_18, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("CUOTAR_8", OracleDbType.Decimal, oAsiento.Cuota_8, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("CUOTARE_0", OracleDbType.Decimal, oAsiento.Cuota_0, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("REGESP", OracleDbType.Decimal, oAsiento.RegEsp, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("CUENTA_18", OracleDbType.Int32, If(oAsiento.Cuenta_18 > 0, oAsiento.Cuenta_18, DBNull.Value), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("CUENTA_8", OracleDbType.Int32, If(oAsiento.Cuenta_8 > 0, oAsiento.Cuenta_8, DBNull.Value), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("CUENTA_0", OracleDbType.Int32, If(oAsiento.Cuenta_0 > 0, oAsiento.Cuenta_0, DBNull.Value), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("PRODUCTO", OracleDbType.NVarchar2, oAsiento.Producto, ParameterDirection.Input))
                    parametros.Add(New OracleParameter("FACTURA", OracleDbType.NVarchar2, oAsiento.Factura, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, con, parametros.ToArray)
                Next
                If (myCon Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (myCon Is Nothing) Then transact.Rollback()
                Throw New BatzException("Error al guardar el asiento contable en la tabla temporal de las facturas Eroski", ex)
            Finally
                If (myCon Is Nothing AndAlso con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

#End Region

#Region "Asientos contables de Visas/Facturas Eroski"

        ''' <summary>
        ''' Obtiene los asientos contables temporales de las visas
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="codDepto">Parametro opcional con el codigo de departamento a mostrar</param>
        ''' <param name="bAgrupDpto">Parametro opcional que indica si se devolveran los asientos agrupados por departamento o no</param>
        ''' <returns></returns>        
        Function loadAsientosContVisasTmp(ByVal idPlanta As Integer, ByVal codDepto As String, ByVal bAgrupDpto As Boolean) As List(Of String())
            Try
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Dim query As String = String.Empty
                If (bAgrupDpto) Then
                    query &= "SELECT '' AS ID_SAB, '' AS NUM_TRA,'' AS NOMBRE,COD_DEPART,DEPARTAMENTO,SUM(IMPORTE),FECHA_INSERCION,ID_PLANTA,CUENTA,UNIDAD_ORGANIZATIVA,TIPO FROM TMP_ASIENTO_CONT_VISAS WHERE ID_PLANTA=:ID_PLANTA "
                    If (codDepto <> String.Empty) Then
                        query &= "AND COD_DEPART=:COD_DEPART "
                        lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Int32, codDepto, ParameterDirection.Input))
                    End If
                    query &= "GROUP BY '','', '',COD_DEPART,DEPARTAMENTO,FECHA_INSERCION,ID_PLANTA,CUENTA,UNIDAD_ORGANIZATIVA,TIPO " _
                          & "ORDER BY UNIDAD_ORGANIZATIVA,DEPARTAMENTO"
                Else
                    query = "SELECT * FROM TMP_ASIENTO_CONT_VISAS WHERE ID_PLANTA=:ID_PLANTA "
                    If (codDepto <> String.Empty) Then
                        query &= "AND COD_DEPART=:COD_DEPART "
                        lParametros.Add(New OracleParameter("COD_DEPART", OracleDbType.Int32, codDepto, ParameterDirection.Input))
                    End If
                    query &= "ORDER BY NUM_TRA"
                End If
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los asientos contables temporales de las visas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los asientos contables de las visas
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idImportacion">Indica si se obtendran los asientos correspondientes a una importacion realizada en una fecha</param>
        ''' <returns></returns>        
        Function loadAsientosContVisas(ByVal idPlanta As Integer, ByVal idImportacion As Integer) As List(Of String())
            Try
                Dim query As String = "SELECT ID,CUENTA,IMPORTE,FECHA_INSERCION,ID_PLANTA,ID_IMPORTACION,COD_DEPART,TIPO,LANTEGI FROM ASIENTOS_CONT_VISAS WHERE ID_PLANTA=:ID_PLANTA "
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (idImportacion > 0) Then
                    lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                    query &= "AND ID_IMPORTACION=:ID_IMPORTACION "
                End If
                query &= "ORDER BY CUENTA"
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los asientos contables de las visas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la cabecera de los asientos contables de facturas Eroski
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idImportacion">Parametro opcional que indica si se obtendran los asientos correspondientes a una importacion realizada en una fecha</param>
        ''' <returns></returns>        
        Function loadAsientosFactCabecera(ByVal idPlanta As Integer, Optional ByVal idImportacion As Integer = Integer.MinValue) As List(Of ELL.AsientoContableCab)
            Try
                Dim query As String = "SELECT ID,DOC_BATZ,FECHA_CON,FECHA_EMI,FECHA_VENC,NFACTU,IMPORTE,IVA,IMPORTE_TOTAL,FECHA_INSERCION,ID_PLANTA,ID_IMPORTACION,FECHA_FACT FROM ASIENTOS_CONT_FACT_CAB WHERE ID_PLANTA=:ID_PLANTA "
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (idImportacion > 0) Then
                    lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, idImportacion, ParameterDirection.Input))
                    query &= "AND ID_IMPORTACION=:ID_IMPORTACION"
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.AsientoContableCab)(Function(r As OracleDataReader) _
                            New ELL.AsientoContableCab With {.Id = CInt(r("ID")), .Factura = r("NFACTU"), .FechaContabilidad = CDate(r("FECHA_CON")), .FechaEmision = CDate(r("FECHA_EMI")), .FechaVencimiento = CDate(r("FECHA_VENC")),
                                                             .IVA = CDec(r("IVA")), .Importe = CDec(r("IMPORTE")), .ImporteTotal = CDec(r("IMPORTE_TOTAL")), .FechaInsercion = CDate(r("FECHA_INSERCION")), .IdPlanta = CInt(r("ID_PLANTA")),
                                                             .IdImportacion = CInt(r("ID_IMPORTACION")), .FechaFactura = SabLib.BLL.Utils.DateNull(r("FECHA_FACT")), .DocumentoBatz = r("DOC_BATZ")}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener la cabecera de los asientos contables de facturacion de Eroski", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las lineas de los asientos contables de facturas Eroski
        ''' </summary>        
        ''' <param name="idCabecera">Id de la cabecera</param>        
        ''' <returns></returns>        
        Function loadAsientosFactLineas(ByVal idCabecera As Integer) As List(Of ELL.AsientoContableCab.Linea)
            Try
                Dim query As String = "SELECT ID,ID_CAB,LINEA,CUENTA,TIPO_IVA,IMPORTE,IVA,COD_DEPART FROM ASIENTOS_CONT_FACT_LINEAS WHERE ID_CAB=:ID_CAB ORDER BY CUENTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_CAB", OracleDbType.Int32, idCabecera, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.AsientoContableCab.Linea)(Function(r As OracleDataReader) _
                            New ELL.AsientoContableCab.Linea With {.Id = CInt(r("ID")), .IdCab = idCabecera, .Linea = SabLib.BLL.Utils.integerNull(r("LINEA")), .TipoIVA = SabLib.BLL.Utils.integerNull(r("TIPO_IVA")),
                                                             .IVA = SabLib.BLL.Utils.decimalNull(r("IVA")), .Importe = CDec(r("IMPORTE")), .CodigoDepartamento = SabLib.BLL.Utils.stringNull(r("COD_DEPART")),
                                                             .Cuenta = SabLib.BLL.Utils.stringNull(r("CUENTA"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener las lineas de los asientos contables de facturacion de Eroski de idCabecera " & idCabecera, ex)
            End Try
        End Function

        ''' <summary>
        ''' Borra los registros de la tabla temporal de asientos de las visas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        Sub DeleteAsientosContVisasTmp(ByVal idPlanta As Integer)
            Try
                Dim query As String = "DELETE FROM TMP_ASIENTO_CONT_VISAS WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar los registros de la tabla temporal de asientos contables de visas", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Inserta los asiento contable en la tabla temporal de asientos de las visas
        ''' </summary>
        ''' <param name="lAsientos">Asientos a insertar</param>       
        ''' <param name="myCon">Parametro opcional con la conexion por si esta en una transaccion</param>
        Sub SaveAsientosContVisasTmp(ByVal lAsientos As List(Of String()), ByVal myCon As OracleConnection)
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                If (myCon Is Nothing) Then
                    con = New OracleConnection(Conexion)
                    con.Open()
                    transact = con.BeginTransaction()
                Else
                    con = myCon
                End If
                Dim query As String = "INSERT INTO TMP_ASIENTO_CONT_VISAS(ID_SAB,NUM_TRA,NOMBRE,COD_DEPART,DEPARTAMENTO,IMPORTE,FECHA_INSERCION,ID_PLANTA,CUENTA,UNIDAD_ORGANIZATIVA,TIPO) VALUES " _
                                                                & "(:ID_SAB,:NUM_TRA,:NOMBRE,:COD_DEPART,:DEPARTAMENTO,:IMPORTE,:FECHA_INSERCION,:ID_PLANTA,:CUENTA,:UNIDAD_ORGANIZATIVA,:TIPO)"
                Dim parametros As List(Of OracleParameter) = Nothing
                For Each sAsiento As String() In lAsientos
                    parametros = New List(Of OracleParameter)
                    parametros.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(sAsiento(0)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("NUM_TRA", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(sAsiento(1)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("NOMBRE", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(sAsiento(2)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("COD_DEPART", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(sAsiento(3)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("DEPARTAMENTO", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(sAsiento(4)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("IMPORTE", OracleDbType.Decimal, BLL.BidaiakBLL.DecimalValue(sAsiento(5)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("FECHA_INSERCION", OracleDbType.Date, CDate(sAsiento(6)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, CInt(sAsiento(7)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("CUENTA", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(sAsiento(8)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("UNIDAD_ORGANIZATIVA", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(sAsiento(9)), ParameterDirection.Input))
                    parametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, CInt(sAsiento(10)), ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, con, parametros.ToArray)
                Next
                If (myCon Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (myCon Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al guardar el asiento contable en la tabla temporal de las visas", ex)
            Finally
                If (myCon Is Nothing AndAlso con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

#End Region

    End Class

End Namespace