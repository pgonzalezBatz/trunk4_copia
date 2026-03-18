Namespace BLL

    Public Class DepartamentosBLL

        Private deptDAL As New DAL.DepartamentosDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de una cuenta contable
        ''' Se mirara si la cuenta es de Igorre o es de una planta de fuera      
        ''' </summary>
        ''' <param name="id">Id</param>
        ''' <param name="idPlanta">Id de la planta</param>      
        ''' <param name="cargarActividades">Parametro opcional para cargar las actividades relacionadas</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As String, ByVal idPlanta As Integer, Optional ByVal cargarActividades As Boolean = False) As ELL.Departamento
            Dim deptBLL As New SabLib.BLL.DepartamentosComponent
            Dim oDeptCuenta As New ELL.Departamento
            Dim oDept As SabLib.ELL.Departamento = deptBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = id, .IdPlanta = idPlanta})
            If (oDept IsNot Nothing AndAlso oDept.Nombre <> "Not found") Then
                oDeptCuenta.IdPlanta = oDept.IdPlanta
                oDeptCuenta.CodigoDepartamento = oDept.Id
                oDeptCuenta = deptDAL.loadInfo(id)
                oDeptCuenta.Departamento = oDept.Nombre
                If (oDeptCuenta Is Nothing) Then
                    Dim oCuenta As ELL.CuentaContable = loadCuentaPlantaFilial(idPlanta, oDeptCuenta.IdPlanta)
                    oDeptCuenta.Cuenta18 = oCuenta.Cuenta18
                    oDeptCuenta.Cuenta8 = oCuenta.Cuenta8
                    oDeptCuenta.Cuenta0 = oCuenta.Cuenta0
                    oDeptCuenta.OFImproductiva = 1 'TODO:No se usa, habrá que quitarlo
                End If
            Else
                oDeptCuenta = deptDAL.loadInfo(id)
            End If
            If (cargarActividades AndAlso oDeptCuenta IsNot Nothing) Then
                Dim activBLL As New BLL.ActividadesBLL
                oDeptCuenta.Actividades = activBLL.loadListDpto(oDeptCuenta.CodigoDepartamento, idPlanta, 0)
            End If
            Return oDeptCuenta
        End Function

        ''' <summary>
        ''' Obtiene el listado de cuentas contables
        ''' </summary>
        ''' <param name="oCuentaCont">Objeto cuenta con las condiciones </param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bMostrarObsoletos">Parametro opcional para mostrar los obsoletos</param>
        ''' <param name="cargarActividades">Parametro opcional para indicar si debe cargar las actividades del departamento</param>
        ''' <returns></returns>  
        Public Function loadList(ByVal oCuentaCont As ELL.Departamento, ByVal idPlanta As Integer, Optional ByVal bMostrarObsoletos As Boolean = True, Optional ByVal cargarActividades As Boolean = False) As List(Of ELL.Departamento)
            Dim lDeptos As List(Of ELL.Departamento) = deptDAL.loadList(oCuentaCont, idPlanta, bMostrarObsoletos)
            If (cargarActividades) Then
                Dim activBLL As New ActividadesBLL
                For Each dpto In lDeptos
                    dpto.Actividades = activBLL.loadListDpto(dpto.CodigoDepartamento, idPlanta, 0)
                Next
            End If
            Return lDeptos
        End Function

        ''' <summary>
        ''' Obtiene la informacion del usuario acerca de cual es la planta activa del usuario
        ''' </summary>
        ''' <param name="oUser">Usuario a comprobar</param>
        ''' <param name="fecha">Fecha en la que se quiere obtener su departamento</param>>
        ''' <param name="idPlantaGestion">Id de la planta de gestion</param>
        ''' <returns>Cuenta contable del usuario</returns>
        Public Function loadInfoCuentaPlantaActiva(ByVal oUser As SabLib.ELL.Usuario, ByVal fecha As Date, ByVal idPlantaGestion As Integer) As ELL.Departamento
            Try
                Dim oDeptCuenta As New ELL.Departamento
                Dim oDept As SabLib.ELL.Departamento
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(idPlantaGestion)
                Dim deptBLL As New SabLib.BLL.DepartamentosComponent
                If (oUser.Nombre = String.Empty AndAlso oUser.CodPersona = Integer.MinValue) Then 'Si el usuario no viene informado, se obtiene su informacion
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    oUser = userBLL.GetUsuario(oUser, False)
                End If
                oDeptCuenta.IdPlanta = oUser.IdPlanta
                oDept = deptBLL.GetDepartamentoPersonaEnFecha(oPlant, oUser.CodPersona, fecha)
                Dim idDpto As String = If(oDept Is Nothing, oUser.IdDepartamento, oDept.Id)
                Dim lCuentas As List(Of ELL.Departamento) = loadList(New ELL.Departamento With {.CodigoDepartamento = idDpto}, idPlantaGestion, False)
                If (lCuentas.Count = 0) Then 'Puede que pertenezca a una planta filial                                        
                    If (oDept IsNot Nothing) Then
                        oDeptCuenta.CodigoDepartamento = oDept.Id
                        oDeptCuenta.Departamento = oDept.Nombre
                        Dim oCuenta As ELL.CuentaContable = loadCuentaPlantaFilial(idPlantaGestion, oDeptCuenta.IdPlanta)
                        oDeptCuenta.Cuenta18 = oCuenta.Cuenta18
                        oDeptCuenta.Cuenta8 = oCuenta.Cuenta8
                        oDeptCuenta.Cuenta0 = oCuenta.Cuenta0
                        oDeptCuenta.OFImproductiva = 1 'No se usa, en el futuro habra que quitarla
                    Else
                        Throw New BatzException("La planta " & oDeptCuenta.IdPlanta & " no tiene una cuenta asociada", Nothing)
                    End If
                ElseIf (lCuentas.Count = 1) Then
                    oDeptCuenta = lCuentas.First
                Else
                    Dim nombreDepartamento As String = String.Empty
                    Try
                        If (oDept Is Nothing) Then oDept = deptBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = idDpto, .IdPlanta = oUser.IdPlanta})
                        If (oDept IsNot Nothing) Then nombreDepartamento = oDept.Nombre.Trim
                    Catch 'Solo queremos esta funcion para obtener el nombre del departamento. Si falla, queremos que proceda                        
                    End Try
                    Throw New BatzException("El departamento " & nombreDepartamento & " (" & idDpto & ")  tiene que tener una unica cuenta asociada", Nothing)
                End If
                Return oDeptCuenta
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al obtener la cuenta del usuario", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta una lista de cuentas contables
        ''' </summary>
        ''' <param name="lCuentas">Cuentas a insertar</param>        
        Public Sub Add(ByVal lCuentas As List(Of ELL.Departamento))
            deptDAL.Add(lCuentas)
        End Sub

        ''' <summary>
        ''' Actualiza una cuenta contable
        ''' </summary>
        ''' <param name="oCuenta">Modifica el departamento</param>        
        Public Sub Update(ByVal oCuenta As ELL.Departamento)
            deptDAL.Update(oCuenta)
        End Sub

        ''' <summary>
        ''' Marca como obsoleto la cuenta contable
        ''' </summary>
        ''' <param name="id">Id del objeto</param>       
        Public Sub Delete(ByVal id As Integer)
            deptDAL.Delete(id)
        End Sub

        ''' <summary>
        ''' Marca como obsoleto las cuentas contables
        ''' </summary>
        ''' <param name="lCuentas">Lista de cuentas</param>         
        Public Sub Delete(ByVal lCuentas As List(Of Integer))
            deptDAL.Delete(lCuentas)
        End Sub

#End Region

#Region "Sincronizar"

        ''' <summary>
        ''' Sincroniza las cuentas con una tabla donde se encuentran los departamentos activos
        ''' Los departamentos a importar, deberan empezar por un nº. Esto es para diferenciar los departamentos de Igorre con los creados para las otras plantas
        ''' </summary> 
        ''' <param name="IdPlanta">Id de la planta</param>
        ''' <param name="lAñadidos">Lista de cuentas añadidas en el origen</param>      
        ''' <param name="lEliminados">Lista de cuentas eliminadas en el origen</param>   
        Public Sub Sincronizar(ByVal IdPlanta As Integer, ByRef lAñadidos As List(Of ELL.Departamento), ByRef lEliminados As List(Of ELL.Departamento))
            Try
                Dim epsilonBLL As New BLL.Epsilon(IdPlanta)
                Dim lDep As List(Of String()) = epsilonBLL.GetDepartamentos()
                Dim lDept As List(Of ELL.Departamento) = loadList(New ELL.Departamento With {.IdPlanta = IdPlanta}, IdPlanta, False)
                Dim oDept As ELL.Departamento = Nothing
                Dim idDept As Integer
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim lUsuarios As List(Of SabLib.ELL.Usuario) = Nothing
                'Comprobamos los nuevos dados de alta
                For Each sDep As String() In lDep
                    idDept = CInt(sDep(0))
                    If (Not lDept.Exists(Function(o As ELL.Departamento) CInt(o.CodigoDepartamento) = idDept)) Then
                        ''Solamente se añadira, si en SAB, existe alguna persona asignada a ese departamento
                        'lUsuarios = userBLL.GetUsuarios(New SabLib.ELL.Usuario With {.IdDepartamento = sDep(0), .IdPlanta = 1})
                        'If (lUsuarios IsNot Nothing AndAlso lUsuarios.Count > 0) Then
                        oDept = New ELL.Departamento
                        oDept.CodigoDepartamento = idDept
                        oDept.Departamento = sDep(1).Trim
                        oDept.IdPlanta = IdPlanta
                        If (lAñadidos Is Nothing) Then lAñadidos = New List(Of ELL.Departamento)
                        lAñadidos.Add(oDept)
                        'End If
                    End If
                Next

                'Comprobamos los que ya no estan                
                For Each myCuenta As ELL.Departamento In lDept
                    idDept = myCuenta.CodigoDepartamento
                    If (Not lDep.Exists(Function(o As String()) CInt(o(0)) = idDept)) Then
                        If (lEliminados Is Nothing) Then lEliminados = New List(Of ELL.Departamento)
                        lEliminados.Add(myCuenta)
                    End If
                Next
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al sincronizar", ex)
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
            Return deptDAL.loadCuentaPlantaFilial(idPlantaGestion, idPlantaCta)
        End Function

        ''' <summary>
        ''' Obtiene el listado de cuentas contables de las plantas filiales
        ''' </summary> 
        ''' <param name="idPlantaGestion">Id de la planta de gestion</param>       
        ''' <returns></returns>        
        Function loadCuentasPlantasFilialesList(ByVal idPlantaGestion As Integer) As List(Of ELL.CuentaContable)
            Return deptDAL.loadCuentasPlantasFilialesList(idPlantaGestion)
        End Function

        ''' <summary>
        ''' Actualiza la cuenta de la planta
        ''' </summary>
        ''' <param name="oCuenta">Informacion de la cuenta</param>        
        Sub UpdateCuentaPlantaFilial(ByVal oCuenta As ELL.CuentaContable)
            deptDAL.UpdateCuentaPlantaFilial(oCuenta)
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
        Function loadAsientosContEroskiTmp(ByVal idPlanta As Integer, Optional ByVal codDepto As Integer = Integer.MinValue, Optional ByVal bAgrupUO As Boolean = False, Optional ByVal factura As String = "") As List(Of ELL.AsientoContableEroskiTmp)
            Return deptDAL.loadAsientosContEroskiTmp(idPlanta, codDepto, bAgrupUO, factura)
        End Function

        ''' <summary>
        ''' Importa los asientos contables provenientes de las facturas de Eroski a la tabla temporal
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta de gestion</param>
        ''' <returns></returns>        
        Function ImportarAsientosContEroskiTmp(ByVal idPlanta As Integer) As List(Of ELL.AsientoContableEroskiTmp)
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim parametros As List(Of OracleParameter) = Nothing
            Try
                Dim idUser As Integer
                Dim prod, unidadOrgan, factura As String
                Dim lAsientos As New List(Of ELL.AsientoContableEroskiTmp)
                Dim oAsiento, oAsientoNew As ELL.AsientoContableEroskiTmp
                Dim solicAgenBLL As New SolicAgenciasBLL
                Dim epsilonBLL As New Epsilon(idPlanta)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim deptBLL As New SabLib.BLL.DepartamentosComponent
                Dim oDept As ELL.Departamento
                Dim dptoValido As Boolean
                Dim oDeptSAB As SabLib.ELL.Departamento
                Dim oUser As SabLib.ELL.Usuario
                Dim oViaje As ELL.Viaje
                Dim hViajes As New Hashtable
                Dim fechaConsulta As Date
                Dim viajesBLL As New BLL.ViajesBLL
                Dim lFacturas As List(Of ELL.FakturaEroski) = solicAgenBLL.loadFacturasEroskiTmp(idPlanta)
                myCon = New OracleConnection(deptDAL.Conexion)
                myCon.Open()
                transact = myCon.BeginTransaction()
                For Each oFactura As ELL.FakturaEroski In lFacturas
                    dptoValido = True
                    idUser = oFactura.IdUser
                    prod = oFactura.Producto
                    factura = oFactura.Factura
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUser}, False)
                    Try
                        fechaConsulta = oFactura.FechaServicio
                        If (oFactura.IdViajes <> String.Empty) Then
                            If (hViajes.ContainsKey(CInt(oFactura.IdViajes))) Then
                                fechaConsulta = CDate(hViajes.Item(CInt(oFactura.IdViajes)))
                            Else
                                oViaje = viajesBLL.loadInfo(CInt(oFactura.IdViajes), bSoloCabecera:=True)
                                fechaConsulta = oViaje.FechaSolicitud
                                hViajes.Add(oViaje.IdViaje, oViaje.FechaSolicitud)
                            End If
                        End If
                        oDept = loadInfoCuentaPlantaActiva(oUser, fechaConsulta, idPlanta)
                    Catch batzEx As BatzException
                        oDept = Nothing
                    End Try
                    If (oDept Is Nothing) Then
                        oAsiento = lAsientos.Find(Function(o) o.IdSab = idUser AndAlso o.Factura = factura AndAlso o.CodigoDepart = oUser.IdDepartamento)  'Se busca si existe el mismo usuario con la misma factura
                    Else
                        oAsiento = lAsientos.Find(Function(o) o.IdSab = idUser AndAlso o.Factura = factura AndAlso o.CodigoDepart = oDept.CodigoDepartamento)  'Se busca si existe el mismo usuario con la misma factura y el mismo departamento
                    End If
                    If (oAsiento Is Nothing) Then  'Es un usuario no repetido                        
                        oAsientoNew = New ELL.AsientoContableEroskiTmp With {.IdSab = idUser}
                        oAsientoNew.numTrabajador = oUser.CodPersona
                        oAsientoNew.Nombre = oUser.NombreCompleto
                        oAsientoNew.CodigoDepart = If(oDept Is Nothing, oUser.IdDepartamento, oDept.CodigoDepartamento)
                        If (oDept Is Nothing And oUser.IdDepartamento <> String.Empty) Then
                            'Tiene un idDepartamento asignado pero no existe                            
                            oAsientoNew.Departamento = oUser.IdDepartamento
                            dptoValido = False
                            oDeptSAB = deptBLL.GetDepartamento(New SabLib.ELL.Departamento With {.IdPlanta = idPlanta, .Id = oUser.IdDepartamento})
                            If (oDeptSAB IsNot Nothing) Then
                                oAsientoNew.CodigoDepart = oDeptSAB.Id  'Si no se encuentra el departamento, se mete el del usuario
                                oAsientoNew.Departamento = oDeptSAB.Nombre.Trim
                            Else
                                oAsientoNew.CodigoDepart = String.Empty  'Se pone a blanco para que luego se sepa que no existe el departamento
                            End If
                        ElseIf (oDept IsNot Nothing AndAlso oDept.Cuenta18 <= 0) Then  'Se ha encontrado el departamento pero no tiene asignado ninguna cuenta
                            oAsientoNew.CodigoDepart = oDept.CodigoDepartamento
                            oAsientoNew.Departamento = oDept.Departamento.Trim
                            dptoValido = False
                        ElseIf (oDept IsNot Nothing) Then
                            oAsientoNew.CodigoDepart = oDept.CodigoDepartamento
                            oAsientoNew.Departamento = oDept.Departamento.Trim
                        End If
                        oAsientoNew.BaseExe_0 = oFactura.BaseExe  'Importe sin IVA     - Base EXE
                        oAsientoNew.BaseIR_8 = oFactura.BaseIR  'Importe con 8% IVA  - Base IR
                        oAsientoNew.BaseIG_18 = oFactura.BaseIG   'Importe con 18% IVA - Base IG
                        oAsientoNew.FechaInsercion = Now
                        oAsientoNew.IdPlanta = idPlanta
                        unidadOrgan = String.Empty
                        If (Not dptoValido) Then
                            unidadOrgan = "Sin cuenta o departamento valido"
                        Else
                            If (oAsientoNew.CodigoDepart <> String.Empty) Then
                                unidadOrgan = epsilonBLL.GetUnidadOrganizativa(oAsientoNew.CodigoDepart)
                                If (unidadOrgan = String.Empty) Then unidadOrgan = "Otros"
                            Else
                                unidadOrgan = "Sin cuenta o departamento valido"
                            End If
                        End If
                        oAsientoNew.UnidadOrganizativa = If(String.IsNullOrEmpty(unidadOrgan), String.Empty, unidadOrgan.Trim)
                        oAsientoNew.Cuota_18 = oFactura.CuotaG  'Iva 18% - CuotaG
                        oAsientoNew.Cuota_8 = oFactura.CuotaR  'Iva 8% - CuotaR
                        oAsientoNew.Cuota_0 = oFactura.CuotaRE  'Iva 0% - Cuotare
                        oAsientoNew.RegEsp = oFactura.RegEsp  'Regesp
                        If (oDept IsNot Nothing) Then
                            oAsientoNew.Cuenta_18 = oDept.Cuenta18
                            oAsientoNew.Cuenta_8 = oDept.Cuenta8
                            oAsientoNew.Cuenta_0 = oDept.Cuenta0
                        Else
                            oAsientoNew.Cuenta_18 = 0
                            oAsientoNew.Cuenta_8 = 0
                            oAsientoNew.Cuenta_0 = 0
                        End If
                        oAsientoNew.Producto = oFactura.Producto  'Producto                        
                        oAsientoNew.Factura = factura
                        lAsientos.Add(oAsientoNew)
                    Else 'Ya esta repetido. Hay que sumarle los importes
                        oAsiento.BaseExe_0 += oFactura.BaseExe
                        oAsiento.BaseIR_8 += oFactura.BaseIR
                        oAsiento.BaseIG_18 += oFactura.BaseIG
                        oAsiento.Cuota_18 += oFactura.CuotaG
                        oAsiento.Cuota_8 += oFactura.CuotaR
                        oAsiento.Cuota_0 += oFactura.CuotaRE
                        oAsiento.RegEsp += oFactura.RegEsp
                    End If
                Next
                SaveAsientosContEroskiTmp(lAsientos)
                transact.Commit()
                Return lAsientos
            Catch batzEx As BatzException
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw New BatzException("Error al importar los asientos contables a la tabla temporal de las facturas Eroski", ex)
            Finally
                If (myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Borra los registros de la tabla temporal de asientos de las facturas de Eroski
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        Sub DeleteAsientosContEroskiTmp(ByVal idPlanta As Integer)
            deptDAL.DeleteAsientosContEroskiTmp(idPlanta)
        End Sub

        ''' <summary>
        ''' Inserta los asiento contable en la tabla temporal de asientos de las facturas de Eroski
        ''' </summary>
        ''' <param name="lAsientos">Asientos a insertar</param>       
        ''' <param name="myCon">Parametro opcional con la conexion por si esta en una transaccion</param>
        Private Sub SaveAsientosContEroskiTmp(ByVal lAsientos As List(Of ELL.AsientoContableEroskiTmp), Optional ByVal myCon As OracleConnection = Nothing)
            deptDAL.SaveAsientosContEroskiTmp(lAsientos, myCon)
        End Sub

#End Region

#Region "Asientos contables de Visas"

        ''' <summary>
        ''' Obtiene los asientos contables temporales de las visas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="codDepto">Parametro opcional con el codigo de departamento a mostrar</param>
        ''' <param name="bAgrupDpto">Parametro opcional que indica si se devolveran los asientos agrupados por departamento o no</param>
        ''' <returns></returns>        
        Function loadAsientosContVisasTmp(ByVal idPlanta As Integer, Optional ByVal codDepto As String = "", Optional ByVal bAgrupDpto As Boolean = False) As List(Of String())
            Return deptDAL.loadAsientosContVisasTmp(idPlanta, codDepto, bAgrupDpto)
        End Function

        ''' <summary>
        ''' Obtiene los asientos contables de las visas
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idImportacion">Parametro opcional que indica si se obtendran los asientos correspondientes a una importacion realizada en una fecha</param>
        ''' <returns></returns>        
        Function loadAsientosContVisas(ByVal idPlanta As Integer, Optional ByVal idImportacion As Integer = Integer.MinValue) As List(Of String())
            Return deptDAL.loadAsientosContVisas(idPlanta, idImportacion)
        End Function

        ''' <summary>
        ''' Obtiene la cabecera de los asientos contables de facturas Eroski
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idImportacion">Parametro opcional que indica si se obtendran los asientos correspondientes a una importacion realizada en una fecha</param>
        ''' <returns></returns>        
        Function loadAsientosFactCabecera(ByVal idPlanta As Integer, Optional ByVal idImportacion As Integer = Integer.MinValue) As List(Of ELL.AsientoContableCab)
            Return deptDAL.loadAsientosFactCabecera(idPlanta, idImportacion)
        End Function

        ''' <summary>
        ''' Obtiene las lineas de los asientos contables de facturas Eroski
        ''' </summary>        
        ''' <param name="idCabecera">Id de la cabecera</param>        
        ''' <returns></returns>        
        Function loadAsientosFactLineas(ByVal idCabecera As Integer) As List(Of ELL.AsientoContableCab.Linea)
            Return deptDAL.loadAsientosFactLineas(idCabecera)
        End Function

        ''' <summary>
        ''' Importa los asientos contables provenientes de las visas a la tabla temporal
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta de gestion</param>
        ''' <returns></returns>        
        Function ImportarAsientosContVisasTmp(ByVal idPlanta As Integer) As List(Of String())
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim parametros As List(Of OracleParameter) = Nothing
            Try
                Dim idUser, idViaje As Integer
                Dim unidadOrgan As String
                Dim lAsientos As New List(Of String())
                Dim visasBLL As New BLL.VisasBLL
                Dim epsilonBLL As New BLL.Epsilon(idPlanta)
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim deptBLL As New SabLib.BLL.DepartamentosComponent
                Dim bidaiBLL As New BLL.BidaiakBLL
                Dim oDept As ELL.Departamento = Nothing
                Dim oDeptSAB As SabLib.ELL.Departamento
                Dim oUser As SabLib.ELL.Usuario
                Dim hViajes As New Hashtable
                Dim fechaConsulta As Date
                Dim oViaje As ELL.Viaje
                Dim viajesBLL As New BLL.ViajesBLL
                Dim tipoAsiento As Integer
                Dim lVisas As List(Of String()) = visasBLL.loadVisasTmp(idPlanta)
                Dim lVisasExcep As List(Of String()) = visasBLL.loadVisasExcepcionTmp(idPlanta)
                Dim cuentaContr As ELL.CuentaContrapartida = bidaiBLL.loadCuentaContrapartida(idPlanta, idPlanta)
                myCon = New OracleConnection(deptDAL.Conexion)
                myCon.Open()
                transact = myCon.BeginTransaction()
                For Each sVisa As String() In lVisas
                    idUser = CInt(sVisa(14))
                    oUser = New SabLib.ELL.Usuario With {.Id = idUser}
                    oUser = userBLL.GetUsuario(oUser, False)
                    If (String.IsNullOrEmpty(sVisa(2))) Then
                        tipoAsiento = 0
                    Else
                        tipoAsiento = If(sVisa(2).ToLower.StartsWith("cuota") OrElse sVisa(2).ToLower.StartsWith("bonificacion cuota"), 1, 0)
                    End If
                    Try
                        fechaConsulta = CDate(sVisa(4))
                        idViaje = CInt(sVisa(7))
                        If (idViaje > 0) Then
                            If (hViajes.ContainsKey(idViaje)) Then
                                fechaConsulta = CDate(hViajes.Item(idViaje))
                            Else
                                oViaje = viajesBLL.loadInfo(idViaje, bSoloCabecera:=True)
                                fechaConsulta = oViaje.FechaSolicitud
                                hViajes.Add(oViaje.IdViaje, oViaje.FechaSolicitud)
                            End If
                        End If
                        oDept = loadInfoCuentaPlantaActiva(oUser, fechaConsulta, idPlanta) 'Fecha del gasto
                    Catch batzEx As BatzException
                        oDept = Nothing
                    End Try
                    Dim sAsiento As String() = Nothing
                    If (oDept Is Nothing) Then
                        sAsiento = lAsientos.Find(Function(o As String()) o(0) = idUser And o(10) = tipoAsiento)
                    Else
                        sAsiento = lAsientos.Find(Function(o As String()) o(0) = idUser And o(10) = tipoAsiento And o(3) = oDept.CodigoDepartamento)  'Se busca si existe el mismo usuario con el mismo tipo de asiento y el mismo departamento
                    End If
                    If (sAsiento Is Nothing) Then  'Es un usuario no repetido                        
                        Dim sAsientoNew(12) As String
                        sAsientoNew(0) = idUser
                        sAsientoNew(1) = oUser.CodPersona
                        sAsientoNew(2) = oUser.NombreCompleto
                        sAsientoNew(3) = If(oDept Is Nothing, oUser.IdDepartamento, oDept.CodigoDepartamento)
                        If (oDept Is Nothing) Then
                            oDeptSAB = deptBLL.GetDepartamento(New SabLib.ELL.Departamento With {.IdPlanta = idPlanta, .Id = oUser.IdDepartamento})
                            If (oDeptSAB IsNot Nothing) Then sAsientoNew(4) = oDeptSAB.Nombre.Trim
                        Else
                            sAsientoNew(4) = oDept.Departamento.Trim
                        End If
                        sAsientoNew(5) = BidaiakBLL.DecimalValue(sVisa(6))  'Importe
                        sAsientoNew(6) = Now.ToShortDateString
                        sAsientoNew(7) = idPlanta
                        If (tipoAsiento = 0) Then
                            sAsientoNew(8) = If(oDept Is Nothing, 0, oDept.Cuenta0)
                        Else 'Cuota
                            sAsientoNew(8) = cuentaContr.CtaCuota
                        End If
                        unidadOrgan = epsilonBLL.GetUnidadOrganizativa(sAsientoNew(3))
                        sAsientoNew(9) = If(String.IsNullOrEmpty(unidadOrgan), String.Empty, unidadOrgan.Trim)
                        sAsientoNew(10) = tipoAsiento
                        sAsientoNew(11) = "" 'Lantegi (Este campo es para las visas excepcion)
                        lAsientos.Add(sAsientoNew)
                    Else 'Ya esta repetido. Hay que sumarle el importe
                        sAsiento(5) += BidaiakBLL.DecimalValue(sVisa(6))
                    End If
                Next
                'If (lVisasExcep.Count > 0) Then
                '    'Dim cuentaVisaExcep = bidaiBLL.loadCuentaVisaExcepcion(idPlanta)
                '    Dim sAsientoNew(11) As String
                '    sAsientoNew(0) = 0 'idUser
                '    sAsientoNew(1) = 0 'CodPersona
                '    sAsientoNew(2) = "" 'NombreCompleto
                '    sAsientoNew(3) = "" 'IdDepartamento
                '    sAsientoNew(4) = "" 'Departamento
                '    sAsientoNew(5) = lVisasExcep.Sum(Function(o) BidaiakBLL.DecimalValue(o(6)))  'Importe
                '    sAsientoNew(6) = Now.ToShortDateString
                '    sAsientoNew(7) = idPlanta
                '    sAsientoNew(8) = "" '"cuentaVisaExcep.Cuenta
                '    sAsientoNew(9) = "" ' Unidad Organizativa
                '    sAsientoNew(10) = 2 'Asiento de visas excepcion
                '    sAsientoNew(11) = "" '"cuentaVisaExcep.Lantegi
                '    lAsientos.Add(sAsientoNew)
                'End If
                SaveAsientosContVisasTmp(lAsientos)
                transact.Commit()
                Return lAsientos
            Catch batzEx As BatzException
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw New BatzException("Error al importar los asientos contables a la tabla temporal de las visas", ex)
            Finally
                If (myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Borra los registros de la tabla temporal de asientos de las visas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        Sub DeleteAsientosContVisasTmp(ByVal idPlanta As Integer)
            deptDAL.DeleteAsientosContVisasTmp(idPlanta)
        End Sub

        ''' <summary>
        ''' Inserta los asiento contable en la tabla temporal de asientos de las visas
        ''' </summary>
        ''' <param name="lAsientos">Asientos a insertar</param>       
        ''' <param name="myCon">Parametro opcional con la conexion por si esta en una transaccion</param>
        Private Sub SaveAsientosContVisasTmp(ByVal lAsientos As List(Of String()), Optional ByVal myCon As OracleConnection = Nothing)
            deptDAL.SaveAsientosContVisasTmp(lAsientos, myCon)
        End Sub

#End Region

    End Class

End Namespace