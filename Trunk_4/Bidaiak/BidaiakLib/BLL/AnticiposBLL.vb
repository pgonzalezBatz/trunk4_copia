Namespace BLL

    Public Class AnticiposBLL

        Private anticiposDAL As New DAL.AnticiposDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion del anticipo de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="bNoCancelados">Indica si quiere la solicitud solo si no esta cancelada</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal idViaje As Integer, Optional ByVal bNoCancelados As Boolean = True, Optional ByVal bSoloCabeceras As Boolean = False) As ELL.Anticipo
            Dim oAnticipo As ELL.Anticipo = anticiposDAL.loadInfo(idViaje)
            If (bNoCancelados AndAlso oAnticipo IsNot Nothing AndAlso oAnticipo.Estado = ELL.Anticipo.EstadoAnticipo.cancelada) Then
                oAnticipo = Nothing
            Else
                If (oAnticipo IsNot Nothing AndAlso Not bSoloCabeceras) Then
                    oAnticipo.Estados = loadStates(idViaje)
                    oAnticipo.Movimientos = FillMovimientos(idViaje)
                End If
            End If
            Return oAnticipo
        End Function

        ''' <summary>
        ''' Obtiene el listado de anticipos de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>  
        Public Function loadList(ByVal idPlanta As Integer) As List(Of String())
            Return anticiposDAL.loadList(idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene los anticipos preparados cuya fecha de necesidad del anticipo este entre las fechas indicadas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fechaInicio">Fecha de inicio</param>
        ''' <param name="fechaFin">Fecha de fin</param>
        ''' <returns></returns>    
        Public Function loadLinesAnticiposPreparados(ByVal idPlanta As Integer, ByVal fechaInicio As Date, ByVal fechaFin As Date) As List(Of ELL.Anticipo.Movimiento)
            Return anticiposDAL.loadLinesAnticiposPreparados(idPlanta, fechaInicio, fechaFin)
        End Function

        ''' <summary>
        ''' Obtiene el listado de movimientos
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo</param>
        ''' <param name="tipoMov">Si se quieren solo las de un tipo en especifico</param>
        ''' <param name="bLoadObjects">Indica si se cargaran los objetos</param>
        ''' <returns></returns>  
        Public Function loadMovimientos(ByVal idAnticipo As Integer, Optional ByVal tipoMov As ELL.Anticipo.Movimiento.TipoMovimiento = Integer.MinValue, Optional ByVal bLoadObjects As Boolean = True) As List(Of ELL.Anticipo.Movimiento)
            Return FillMovimientos(idAnticipo, tipoMov, bLoadObjects)
        End Function

        ''' <summary>
        ''' Obtiene los diversos estados y sus fechas por las que ha pasado un anticipo
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <returns></returns>        
        Private Function loadStates(ByVal idViaje As Integer) As List(Of String())
            Return anticiposDAL.loadStates(idViaje)
        End Function

        ''' <summary>
        ''' Obtiene las remesas entre dos fechas. No se mostraran los anticipos ya entregados
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fechaInicio">Fecha de inicio</param>
        ''' <param name="fechaFin">Fecha de fin</param>
        ''' <param name="idMoneda">Especifica el tipo de moneda del que se quieren obtener las remesas</param>
        ''' <param name="viajeUnico">Se sabe que en estas fechas solo hay un viaje, asi que se pide la consulta sin group by,para que le devuelva el id_viaje</param>
        ''' <returns></returns>    
        Public Function loadRemesas(ByVal idPlanta As Integer, ByVal fechaInicio As Date, ByVal fechaFin As Date, Optional ByVal idMoneda As Integer = Integer.MinValue, Optional ByVal viajeunico As Boolean = False) As List(Of String())
            Return anticiposDAL.loadRemesas(idPlanta, fechaInicio, fechaFin, idMoneda, viajeunico)
        End Function

        ''' <summary>
        ''' Anticipos cancelados en los que se haya entregado el anticipo pero no haya sido devuelto
        ''' </summary>
        ''' <param name="idPlanta">Id planta</param>
        ''' <returns></returns>        
        Function loadListCanceladosNoDevueltos(ByVal idPlanta As Integer) As List(Of String())
            Return anticiposDAL.loadListCanceladosNoDevueltos(idPlanta)
        End Function

        ''' <summary>
        ''' Devuelve los movimientos de un anticipo
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo</param> 
        ''' <param name="tipoMov">Si se quieren solo las de un tipo en especifico</param>       
        ''' <param name="bLoadObjects">Indica si se cargaran los objetos</param>
        Private Function FillMovimientos(ByVal idAnticipo As Integer, Optional ByVal tipoMov As ELL.Anticipo.Movimiento.TipoMovimiento = Integer.MinValue, Optional ByVal bLoadObjects As Boolean = True) As List(Of ELL.Anticipo.Movimiento)
            Try
                Dim xbatBLL As New BLL.XbatBLL
                Dim lMovimientos As List(Of ELL.Anticipo.Movimiento) = Nothing
                Dim oMov As ELL.Anticipo.Movimiento
                Dim lDatos As List(Of String()) = anticiposDAL.loadLines(idAnticipo, tipoMov)
                If (lDatos IsNot Nothing AndAlso lDatos.Count > 0) Then
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    lMovimientos = New List(Of ELL.Anticipo.Movimiento)
                    For Each sMov As String() In lDatos
                        If (bLoadObjects) Then
                            oMov = New ELL.Anticipo.Movimiento With {.Id = CInt(sMov(0)), .IdAnticipo = CInt(sMov(1)), .Moneda = New ELL.Moneda With {.Id = CInt(sMov(2))}, .Cantidad = DecimalValue(sMov(3)), .Fecha = CDate(sMov(4)),
                                                            .IdViajeOrigen = CInt(sMov(7)), .IdViajeDestino = CInt(sMov(8)), .Comentarios = SabLib.BLL.Utils.stringNull(sMov(9)), .TipoMov = CInt(sMov(10)), .ImporteEuros = DecimalValue(sMov(11)), .CambioMonedaEUR = DecimalValue(sMov(12))}
                            If (SabLib.BLL.Utils.integerNull(sMov(5)) <> Integer.MinValue) Then oMov.UserOrigen = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(sMov(5))}, False)
                            If (SabLib.BLL.Utils.integerNull(sMov(6)) <> Integer.MinValue) Then oMov.UserDestino = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(sMov(6))}, False)
                        Else
                            oMov = New ELL.Anticipo.Movimiento With {.Id = CInt(sMov(0)), .IdAnticipo = CInt(sMov(1)), .Moneda = New ELL.Moneda With {.Id = CInt(sMov(2))}, .Cantidad = DecimalValue(sMov(3)), .Fecha = CDate(sMov(4)),
                                                           .IdViajeOrigen = CInt(sMov(7)), .IdViajeDestino = CInt(sMov(8)), .Comentarios = SabLib.BLL.Utils.stringNull(sMov(9)), .TipoMov = CInt(sMov(10)), .ImporteEuros = DecimalValue(sMov(11)), .CambioMonedaEUR = DecimalValue(sMov(12))}
                            If (SabLib.BLL.Utils.integerNull(sMov(5)) <> Integer.MinValue) Then oMov.UserOrigen = New SabLib.ELL.Usuario With {.Id = CInt(sMov(5))}
                            If (SabLib.BLL.Utils.integerNull(sMov(6)) <> Integer.MinValue) Then oMov.UserDestino = New SabLib.ELL.Usuario With {.Id = CInt(sMov(6))}
                        End If
                        oMov.Moneda = xbatBLL.GetMoneda(oMov.Moneda.Id)
                        lMovimientos.Add(oMov)
                    Next
                End If
                Return lMovimientos
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al construir los movimientos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga los anticipos pendientes de justificar
        ''' No se toman en cuenta los gastos de kilometraje
        ''' </summary>       
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idUser">Id del usuario a comprobar</param>
        ''' <param name="idAnticipoExcept">Id del anticipo que no se contara ya que es el que tiene abierto</param>
        ''' <returns>0:IdAnticipo,1:Estado,2:Fecha estado(Devolvemos blanco),3:Id user,4:CodPersona,5:Nombre liquidador,6:Dado baja,7:Fecha ida viaje,8:Fecha vuelta viaje,9:Cantidad pendiente,10:Id HG,11:Estado HG,12:Fecha estado HG</returns> 
        Public Function loadAnticiposPendientes(ByVal idPlanta As Integer, Optional ByVal idUser As Integer = 0, Optional ByVal idAnticipoExcept As Integer = 0) As List(Of Object)
            Try
                Dim lAnticiposPend As List(Of String())
                Dim lAnticiposResul As New List(Of Object)
                Dim hojasBLL As New HojasGastosBLL
                Dim anticipo, cantidadLineas, cantidadPendiente As Decimal
                Dim oHoja As ELL.HojaGastos
                Dim anticBLL As New AnticiposBLL
                Dim lMov, lMovAll As List(Of ELL.Anticipo.Movimiento)
                lAnticiposPend = anticiposDAL.loadAnticiposPendientes(idPlanta, idUser, idAnticipoExcept)
                If (lAnticiposPend IsNot Nothing AndAlso lAnticiposPend.Count > 0) Then
                    For Each sAntic As String() In lAnticiposPend
                        cantidadLineas = 0 : anticipo = 0 : cantidadPendiente = 0 : lMovAll = Nothing : oHoja = Nothing
                        'Se carga el anticipo
                        '------------------------------------------
                        lMovAll = anticBLL.loadMovimientos(CInt(sAntic(0)), Integer.MinValue, False) 'Se obtienen todos
                        lMov = lMovAll.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Entregado)
                        If (lMov.Count > 0) Then
                            For Each mov As ELL.Anticipo.Movimiento In lMov
                                anticipo += mov.ImporteEuros
                            Next
                        End If

                        'Se comprueba si tiene alguna transferencia el viaje                                                
                        Dim oAntic As ELL.Anticipo = anticBLL.loadInfo(CInt(sAntic(0)), True, False)
                        If (oAntic IsNot Nothing) Then
                            Dim lMovTrans As List(Of ELL.Anticipo.Movimiento) = oAntic.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia)
                            If (lMovTrans.Count > 0) Then
                                For Each oMov As ELL.Anticipo.Movimiento In lMovTrans
                                    If (oMov.IdViajeOrigen = CInt(sAntic(0))) Then
                                        anticipo -= oMov.ImporteEuros
                                    Else
                                        anticipo += oMov.ImporteEuros
                                    End If
                                Next
                            End If
                        End If

                        'Hoja de gastos
                        '------------------------------------------
                        If (Not String.IsNullOrEmpty(sAntic(9)) AndAlso Not String.IsNullOrEmpty(sAntic(10)) AndAlso sAntic(10) <> ELL.HojaGastos.eEstado.Rellenada) Then
                            oHoja = hojasBLL.loadHoja(CInt(sAntic(9)), True)
                            'Se suman los importes de las lineas de las hojas de gastos
                            For Each oLinea As ELL.HojaGastos.Linea In oHoja.Lineas
                                If (oLinea.TipoGasto <> ELL.HojaGastos.Linea.eTipoGasto.Kilometraje) Then
                                    cantidadLineas += oLinea.ImporteEuros
                                End If
                            Next
                        Else
                            cantidadLineas = anticipo
                        End If
                        'Movimientos manuales
                        If (lMovAll IsNot Nothing) Then
                            lMov = lMovAll.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Manual)
                            If (lMov.Count > 0) Then
                                For Each mov As ELL.Anticipo.Movimiento In lMov
                                    cantidadLineas += (mov.ImporteEuros * -1)
                                Next
                            End If
                        End If
                        'Movimientos
                        '------------------------------------------
                        If (anticipo <> 0) Then
                            Dim cantidadLineasConDevolucion As Decimal = 0
                            If (lMovAll IsNot Nothing) Then
                                lMov = lMovAll.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion)
                                If (lMov.Count > 0) Then
                                    For Each mov As ELL.Anticipo.Movimiento In lMov
                                        If mov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion Then
                                            cantidadLineasConDevolucion += mov.ImporteEuros
                                        End If
                                    Next
                                End If
                            End If
                            If (cantidadLineasConDevolucion > 0) Then 'Si el resultado es positivo significa que todavia debe dinero
                                cantidadPendiente = anticipo - (cantidadLineas + cantidadLineasConDevolucion)
                                If (cantidadPendiente < 0) Then cantidadPendiente = 0
                            Else 'Si es 0 significara que no tenia lineas devueltas
                                If (oHoja Is Nothing) Then 'Sino tiene hoja de gastos
                                    cantidadPendiente = anticipo
                                ElseIf (anticipo > cantidadLineas) Then
                                    cantidadPendiente = anticipo - cantidadLineas
                                End If
                            End If
                        End If
                        If (cantidadPendiente > 0) Then  'Omitimos las hojas con 0€                                
                            If (Not lAnticiposResul.Exists(Function(o) o.IdAnticipo = sAntic(0) And o.IdUser = sAntic(3))) Then  'Si no existe el viaje y el trabajador
                                Dim dadoBaja As Boolean = False
                                If (CDate(sAntic(6)) <> Date.MinValue AndAlso CDate(sAntic(6)) < Now) Then dadoBaja = True
                                lAnticiposResul.Add(New With {.IdAnticipo = CInt(sAntic(0)), .Estado = sAntic(1), .FechaEstado = DateTime.MinValue, .IdUser = CInt(sAntic(3)), .CodPersona = CInt(sAntic(4)), .NombreLiq = sAntic(5), .DadoBaja = dadoBaja, .FechaIdaViaje = CDate(sAntic(7)), .FechaVueltaViaje = CDate(sAntic(8)), .CantidadPendiente = Math.Round(cantidadPendiente, 2), .IdHG = CInt(sAntic(9)), .EstadoHG = CInt(sAntic(10)), .FechaEstadoHG = CDate(sAntic(11))})
                                '.FechaEstado = CDate(sAntic(2))
                            End If
                        End If
                    Next
                End If
                If (lAnticiposResul.Count = 0) Then lAnticiposResul = Nothing
                Return lAnticiposResul
            Catch ex As Exception
                Throw New BatzException("Error al calcular los anticipos pendientes de justificar", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la fecha de entrega del anticipo ya que sera la fecha que se utilizara para todos los tipos de cambio de hojas de gastos, devoluciones,transferencias
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo</param>
        ''' <returns></returns>
        Public Function loadAnticipoFechaEntrega(ByVal idAnticipo As Integer) As DateTime
            Dim fechaEntrega As DateTime
            Dim lStates As List(Of String()) = loadStates(idAnticipo)
            If (lStates IsNot Nothing AndAlso lStates.Count > 0) Then
                lStates = lStates.FindAll(Function(o) o(0) = ELL.Anticipo.EstadoAnticipo.Entregado)
                If (lStates IsNot Nothing) Then
                    If (lStates.Count = 1) Then
                        fechaEntrega = CDate(lStates.Item(0)(1))
                    ElseIf (lStates.Count > 1) Then 'Si ha habido mas de una fecha de entrega, se coge la última
                        lStates.OrderByDescending(Function(o) CDate(o(1))).ToList
                        fechaEntrega = CDate(lStates.Item(0)(1))
                    End If
                End If
            End If
            Return fechaEntrega
        End Function

        '''' <summary>
        '''' Calcula el importe total aplicando el cambio de moneda pertinente. Si existe anticipo, se cogera el de la entrega del anticipo, si no la que le corresponda
        '''' </summary>
        '''' <param name="idAnticipo">Id del anticipo</param>
        '''' <param name="importe">Importe</param>
        '''' <param name="idCurrency">Id de la moneda del importe</param>
        '''' <param name="fecha">Fecha del movimiento</param>
        '''' <param name="cambioMoneda">Variable donde se dejara el cambio de la moneda</param>
        '''' <returns></returns>
        'Public Function CalcularImporteConCambio(ByVal idAnticipo As Integer, ByVal importe As Decimal, ByVal idCurrency As Integer, ByVal fecha As Date, ByRef cambioMoneda As Decimal) As Decimal
        '    Dim importeEuros As Decimal = 0
        '    cambioMoneda = 0
        '    If (idCurrency = 90) Then 'Al ser euros, no hay que hacer nada
        '        cambioMoneda = 1
        '        importeEuros = importe
        '    Else 'Importe con moneda distinta al €                
        '        If (idAnticipo > 0) Then  'Intentamos sacar el cambio de la entrega del anticipo
        '            '????Mirar como sacar lo de las transferencias. Como lo mete en bbdd
        '            'Dim lMovs As List(Of ELL.Anticipo.Movimiento) = loadMovimientos(idAnticipo, bLoadObjects:=False)
        '            'Dim oMov As ELL.Anticipo.Movimiento=lMovs.Find(Function())
        '        End If
        '        If (cambioMoneda = 0) Then  'No se ha entregado el anticipo
        '            Dim xbatBLL As New BLL.XbatBLL
        '            importeEuros = xbatBLL.ObtenerRateEuros(idCurrency, importe, fecha, cambioMoneda)
        '        End If
        '    End If
        '    Return importeEuros
        'End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el anticipo y sus lineas
        ''' </summary>
        ''' <param name="oAntic">Objeto solicitud con la informacion</param>        
        ''' <param name="bSaveOnlyHeader">Indica si se guarda solo la cabecera</param>
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>             
        Sub Save(ByVal oAntic As ELL.Anticipo, ByVal bSaveOnlyHeader As Boolean, Optional ByVal con As OracleConnection = Nothing)
            anticiposDAL.Save(oAntic, bSaveOnlyHeader, con)
        End Sub

        ''' <summary>
        ''' Cancela la solicitud por el usuario
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>             
        Sub Delete(ByVal idViaje As Integer, Optional ByVal con As OracleConnection = Nothing)
            anticiposDAL.Delete(idViaje, con)
        End Sub

        ''' <summary>
        ''' Se borra el anticipo
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>      
        ''' <return>Antiguo liquidador</return>          
        Function Anular(ByVal idViaje As Integer) As Integer
            Return anticiposDAL.Anular(idViaje)
        End Function

        ''' <summary>
        ''' Inserta o modifica un movimiento
        ''' </summary>
        ''' <param name="oMov">Objeto con la informacion</param> 
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>       
        Public Sub SaveMovimiento(ByVal oMov As ELL.Anticipo.Movimiento, Optional ByVal con As OracleConnection = Nothing)
            anticiposDAL.SaveMovimiento(oMov, con)
        End Sub

        ''' <summary>
        ''' Inserta o modifica una lista de movimientos
        ''' </summary>
        ''' <param name="lMov">Lista de movimientos con la informacion</param>    
        Public Sub SaveMovimientos(ByVal lMov As List(Of ELL.Anticipo.Movimiento))
            anticiposDAL.SaveMovimientos(lMov)
        End Sub

        ''' <summary>
        ''' Marca un movimiento como obsoleto. No elimina de la base de datos
        ''' </summary>
        ''' <param name="idMov">Id del movimiento</param>  
        Public Sub DeleteMovimiento(ByVal idMov As Integer)
            anticiposDAL.DeleteMovimiento(idMov)
        End Sub

        ''' <summary>
        ''' Cambia el estado de un anticipo
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo</param>        
        ''' <param name="idEstado">Id del estado</param>   
        ''' <param name="idUser">Parametro opcional. Cuando se cambia a entregado, es necesario el usuario</param>     
        Public Sub CambiarEstado(ByVal idAnticipo As Integer, ByVal idEstado As Integer, Optional ByVal idUser As Integer = Integer.MinValue)
            anticiposDAL.CambiarEstado(idAnticipo, idEstado, Nothing, idUser)
        End Sub

        ''' <summary>
        ''' Obtiene todos los movimientos de entrega de un anticipo y los registro como devueltos
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo</param>        
        ''' <param name="idUser">Usuario que devuelve el anticipo</param>
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub DevolverAnticipo(ByVal idAnticipo As Integer, ByVal idUser As Integer, ByVal idPlanta As Integer)
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Dim myMov As ELL.Anticipo.Movimiento = Nothing
            Dim bidaiBLL As New BLL.BidaiakBLL
            Try
                Dim lMov As List(Of ELL.Anticipo.Movimiento) = loadMovimientos(idAnticipo, ELL.Anticipo.Movimiento.TipoMovimiento.Entregado, False)
                myConnection = New OracleConnection(anticiposDAL.Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                For Each mov As ELL.Anticipo.Movimiento In lMov
                    myMov = New ELL.Anticipo.Movimiento With {.IdAnticipo = idAnticipo}
                    myMov.Cantidad = mov.Cantidad
                    myMov.Moneda = mov.Moneda
                    myMov.Fecha = Now
                    myMov.UserOrigen = New SabLib.ELL.Usuario With {.Id = idUser}
                    myMov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion
                    SaveMovimiento(myMov, myConnection)
                    bidaiBLL.saveSaldoCaja(New ELL.SaldoCaja With {.Cantidad = mov.Cantidad, .Fecha = Now, .IdUsuario = idUser, .IdPlanta = idPlanta, .Operacion = ELL.SaldoCaja.EOperacion.Devolucion_Anticipo,
                                                                   .IdMoneda = mov.Moneda.Id, .Comentario = "Devolucion del anticipo por cancelacion de viaje del usuario " & mov.UserOrigen.NombreCompleto.Trim & " del viaje " & idAnticipo}, myConnection)
                Next
                transact.Commit()
            Catch batzEx As BatzException
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw New BatzException("Error al devolver el anticipo del viaje cancelado (" & idAnticipo & ")", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Añade la moneda para que se pueda pedir en los anticipos
        ''' </summary>
        ''' <param name="idMoneda">Id de la moneda</param>           
        ''' <param name="idPlanta">Id de la planta</param>     
        Sub AddMoneda(ByVal idMoneda As Integer, ByVal idPlanta As Integer)
            anticiposDAL.AddMoneda(idMoneda, idPlanta)
        End Sub

        ''' <summary>
        ''' Elimina la moneda para que no se pueda pedir en los anticipos
        ''' </summary>
        ''' <param name="idMoneda">Id de la moneda</param>           
        ''' <param name="idPlanta">Id de la planta</param>     
        Sub DeleteMoneda(ByVal idMoneda As Integer, ByVal idPlanta As Integer)
            anticiposDAL.DeleteMoneda(idMoneda, idPlanta)
        End Sub

        ''' <summary>
        ''' Guarda las monedas seleccionables en un anticipo
        ''' </summary>
        ''' <param name="lMonedas">Lista de los id de las monedas</param>           
        ''' <param name="idPlanta">Id de la planta</param>     
        Sub SaveMonedas(ByVal lMonedas As List(Of Integer), ByVal idPlanta As Integer)
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                myConnection = New OracleConnection(anticiposDAL.Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                anticiposDAL.DeleteMonedas(idPlanta, myConnection)
                For Each iMon As Integer In lMonedas
                    anticiposDAL.AddMoneda(iMon, idPlanta, myConnection)
                Next
                transact.Commit()
            Catch batzEx As BatzException
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw New BatzException("Error al guardar las monedas del anticipo", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Realiza una transferencia guardando los movimientos y creando las hojas de gastos de origen y fin en caso necesario
        ''' Tambien se chequeara si tiene que crear el anticipo en el viaje destino
        ''' </summary>        
        ''' <param name="lMov">Lista de movimientos con la informacion</param>    
        Public Sub Transferencia(ByVal lMov As List(Of ELL.Anticipo.Movimiento))
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Dim viajesBLL As New BLL.ViajesBLL
            Dim hojasBLL As New BLL.HojasGastosBLL
            Dim bidaiakBLL As New BLL.BidaiakBLL
            Dim sablib As New SabLib.BLL.UsuariosComponent
            Dim oViajeOrig, oViajeDest As ELL.Viaje
            Dim myLinea As ELL.HojaGastos.Linea
            Dim oUser As SabLib.ELL.Usuario
            Dim idResp As Integer
            Dim bCrearHGOrig, bCrearHGDest, bCrearAnticipoDest As Boolean
            Try
                bCrearHGOrig = True : bCrearHGDest = True : bCrearAnticipoDest = False
                '1º Se comprueba que tienen hojas de gastos creadas para que en caso negativo, crearlas
                '----------------------------------------------------------------------------------
                oViajeOrig = viajesBLL.loadInfo(lMov.First.IdViajeOrigen, True)
                If (oViajeOrig.HojasGastos IsNot Nothing AndAlso oViajeOrig.HojasGastos.Count > 0) Then
                    bCrearHGOrig = Not oViajeOrig.HojasGastos.Exists(Function(o As ELL.HojaGastos) o.Usuario.Id = lMov.First.UserOrigen.Id)
                End If
                oViajeDest = viajesBLL.loadInfo(lMov.First.IdViajeDestino, True)
                If (oViajeDest.HojasGastos IsNot Nothing AndAlso oViajeDest.HojasGastos.Count > 0) Then
                    bCrearHGDest = Not oViajeDest.HojasGastos.Exists(Function(o As ELL.HojaGastos) o.Usuario.Id = lMov.First.UserDestino.Id)
                End If

                '2º Se comprueba que el viaje destino tenga anticipo
                '-----------------------------------------------
                bCrearAnticipoDest = (oViajeDest.Anticipo Is Nothing)

                myConnection = New OracleConnection(anticiposDAL.Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()

                '3º Se crean el anticipo destino en caso necesario
                '-----------------------------------------------
                If (bCrearAnticipoDest) Then
                    Dim oAnticipo As New ELL.Anticipo
                    oAnticipo.FechaNecesidad = oViajeDest.FechaIda
                    oAnticipo.IdViaje = oViajeDest.IdViaje
                    oAnticipo.Estado = ELL.Anticipo.EstadoAnticipo.Entregado
                    Save(oAnticipo, True, myConnection)
                    anticiposDAL.CambiarEstado(oAnticipo.IdViaje, oAnticipo.Estado, myConnection, lMov.First.UserOrigen.Id)
                End If

                '4º Se crean las hojas de gastos
                '----------------------------                
                If (bCrearHGOrig) Then
                    oUser = sablib.GetUsuario(New SabLib.ELL.Usuario With {.Id = lMov.First.UserOrigen.Id}, False)
                    idResp = bidaiakBLL.GetResponsable(oUser.Id, oUser.CodPersona, oUser.IdDepartamento, oUser.IdPlanta)
                    Dim xbatBLL As New BLL.XbatBLL
                    Dim moneda As ELL.Moneda = xbatBLL.GetMoneda("EUR") 'Si no se añade la moneda, falla al intentar obtener el importe
                    myLinea = New ELL.HojaGastos.Linea With {.Fecha = oViajeOrig.FechaIda, .Usuario = oUser, .Moneda = moneda}
                    hojasBLL.AddLinea(myLinea, oViajeOrig.IdViaje, idResp, oViajeOrig.IdPlanta, True, myConnection)
                End If
                If (bCrearHGDest) Then
                    oUser = sablib.GetUsuario(New SabLib.ELL.Usuario With {.Id = lMov.First.UserDestino.Id}, False)
                    idResp = bidaiakBLL.GetResponsable(oUser.Id, oUser.CodPersona, oUser.IdDepartamento, oUser.IdPlanta)
                    Dim xbatBLL As New BLL.XbatBLL
                    Dim moneda As ELL.Moneda = xbatBLL.GetMoneda("EUR") 'Si no se añade la moneda, falla al intentar obtener el importe
                    myLinea = New ELL.HojaGastos.Linea With {.Fecha = oViajeDest.FechaIda, .Usuario = oUser, .Moneda = moneda}
                    hojasBLL.AddLinea(myLinea, oViajeDest.IdViaje, idResp, oViajeDest.IdPlanta, True, myConnection)
                End If

                '5º Guardamos los movimientos de la transferencia
                '------------------------------------------------
                anticiposDAL.SaveMovimientos(lMov, myConnection)

                '6º Se actualiza el liquidador del viaje destino en caso necesario
                '-----------------------------------------------------------------
                If (bCrearAnticipoDest) Then viajesBLL.UpdateResponsableLiquidacion(oViajeDest.IdViaje, lMov.First.UserDestino.Id, myConnection)

                If (True) Then
                    transact.Commit()
                Else
                    transact.Rollback()
                End If
            Catch batzEx As BatzException
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (transact IsNot Nothing) Then transact.Rollback()
                Throw New BatzException("Error al guardar la transferencia", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

#End Region

#Region "Decimales"

        ''' <summary>
        ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
        ''' </summary>
        ''' <param name="sDec">Numero a convertir</param>
        ''' <returns></returns>	
        Private Function DecimalValue(ByVal sDec As String) As Decimal
            If (Not String.IsNullOrEmpty(sDec)) Then
                Dim myDec As String = String.Empty
                If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
                    myDec = sDec.Trim.Replace(".", ",")
                ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
                    myDec = sDec.Trim.Replace(",", ".")
                End If
                myDec = If(myDec = String.Empty, "0", myDec)
                Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
            Else
                Return 0
            End If
        End Function

#End Region

    End Class

End Namespace