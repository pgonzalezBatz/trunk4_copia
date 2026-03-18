Namespace BLL

    Public Class HojasGastosBLL

        Private hojaGastosDAL As New DAL.HojaGastosDAL
        Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Bidaiak")

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la hoja de gastos de un usuario        
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idViaje">Opcional:Id del viaje</param>        
        ''' <param name="fechaInicio">Fecha de inicio de la que se quiere obtener las hojas</param>
        ''' <param name="fechaFin">Fecha de fin de la que se quiere obtener las hojas</param>
        ''' <param name="bActivos">Una hoja de gastos estara activa mientras no pase un mes desde la fecha de vuelta del viaje o de la hoja</param>
        ''' <param name="idPlanta">Id de la planta. Por defecto la de Igorre</param>
        ''' <param name="bCargarLineas">Indica si cargara tambien las lineas</param>    
        ''' <param name="bSoloUsuarioDeLaHoja">Por defecto, se obtienen las hojas del usuario creador de la hoja y del validador. Si se pone a true, solo se obtendran las del creador</param>
        ''' <param name="bLoadSabUsuario">Indica si se cargara la informacion de SAB del usuario</param>
        ''' <param name="estado">Estado de la que se recuperaran las hojas</param>
        ''' <returns></returns>        
        Public Function loadHojas(ByVal idPlanta As Integer, Optional ByVal idUser As Integer = Integer.MinValue, Optional ByVal idViaje As Integer = Integer.MinValue, Optional ByVal fechaInicio As Date = Nothing, Optional ByVal fechaFin As Date = Nothing, Optional ByVal bActivos As Boolean = False, Optional ByVal bCargarLineas As Boolean = True, Optional ByVal bSoloUsuarioDeLaHoja As Boolean = False, Optional ByVal bLoadSabUsuario As Boolean = True, Optional ByVal estado As Integer = 0) As List(Of ELL.HojaGastos)
            Dim lHojas As List(Of ELL.HojaGastos) = (hojaGastosDAL.loadHojas(idPlanta, idUser, idViaje, fechaInicio, fechaFin, bActivos, bSoloUsuarioDeLaHoja, bLoadSabUsuario, estado))
            If (bCargarLineas) Then FillMovimientos(lHojas, idPlanta)

            Return lHojas
        End Function

        ''' <summary>
        ''' Obtiene las hojas sin viaje asociado ordenadas por mes
        ''' Si en un mes no existe, se crea un objeto con una hg ficticia
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fInicio">Fecha de inicio</param>
        ''' <param name="fFin">Fecha de fin</param>
        ''' <returns></returns>        
        Public Function loadHojasPorMesSinViaje(ByVal idUser As Integer, ByVal idPlanta As Integer, ByVal fInicio As Date, ByVal fFin As Date) As List(Of ELL.HojaGastos)
            Try
                Dim lHojasPorMes As New List(Of ELL.HojaGastos)
                Dim lHojas As List(Of ELL.HojaGastos) = (hojaGastosDAL.loadHojas(idPlanta, idUser, Integer.MinValue, fInicio, fFin, False, True, False))
                If (lHojas IsNot Nothing) Then
                    lHojas = lHojas.FindAll(Function(o As ELL.HojaGastos) (o.IdViaje = Integer.MinValue And o.Usuario.Id = idUser))
                    lHojas.Sort(Function(o1 As ELL.HojaGastos, o2 As ELL.HojaGastos) o1.FechaDesde < o2.FechaDesde)
                Else
                    lHojas = New List(Of ELL.HojaGastos)
                End If
                Dim oHoja As ELL.HojaGastos
                Dim mes As Integer
                For index As Integer = fInicio.Month To fFin.Month  'Se forma una lista con las hojas de gastos para todos los meses entre la fecha de inicio y la fecha de fin
                    mes = index
                    oHoja = lHojas.Find(Function(o As ELL.HojaGastos) o.FechaDesde.Month = mes)
                    'No existe HG para ese mes
                    If (oHoja Is Nothing) Then oHoja = New ELL.HojaGastos With {.FechaDesde = New Date(fInicio.Year, mes, 1), .FechaHasta = New Date(fInicio.Year, mes, Date.DaysInMonth(fInicio.Year, mes)), .Usuario = New SabLib.ELL.Usuario With {.Id = idUser}}
                    lHojasPorMes.Add(oHoja)
                Next
                Return lHojasPorMes
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New Exception("Error al cargar las hojas de gastos por mes", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la hoja de gastos especificada        
        ''' </summary>
        ''' <param name="id">Id de la hoja de gastos</param>        
        ''' <param name="bCargarMovimientos">Parametro opcional que indica si cargara los movimientos</param>
        ''' <returns></returns>        
        Function loadHoja(ByVal id As Integer, Optional ByVal bCargarMovimientos As Boolean = True) As ELL.HojaGastos
            Dim oHoja As ELL.HojaGastos = hojaGastosDAL.loadHoja(id)
            If (oHoja IsNot Nothing AndAlso bCargarMovimientos) Then
                FillMovimientos(oHoja, oHoja.Usuario.IdPlanta)
            End If
            Return oHoja
        End Function

        ''' <summary>
        ''' Obtiene la info de una o varias hojas de gastos dado un viaje o una hoja de gastos libre
        ''' </summary>
        ''' <param name="idViaje"></param>
        ''' <param name="idHojaLibre"></param>
        ''' <returns></returns>
        Public Function loadHojas(ByVal idViaje As Integer, ByVal idHojaLibre As Integer) As List(Of ELL.HojaGastos)
            Return hojaGastosDAL.loadHojas(idViaje, idHojaLibre)
        End Function

        ''' <summary>
        ''' Devuelve la informacion de una linea de la hoja de gastos
        ''' </summary>
        ''' <param name="id">Id de la linea</param>
        ''' <returns></returns>        
        Function loadLineaHojaGastos(ByVal id As Integer) As ELL.HojaGastos.Linea
            Return hojaGastosDAL.loadLineaHojaGastos(id)
        End Function

        ''' <summary>
        ''' Obtiene el listado de HG de financiero
        ''' </summary>
        ''' <param name="oHoja">Datos del filtro</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function loadHGFinanciero(ByVal oHoja As ELL.HojaGastos, ByVal idPlanta As Integer) As List(Of String())
            Return hojaGastosDAL.loadHGFinanciero(oHoja, idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene los integrantes de un viaje con hojas de gastos rellenadas
        ''' </summary>
        ''' <param name="IdViaje"></param>
        ''' <param name="opcion">Indicara si se quieren todos 0, solo las rellenadas 1,solo las validadas 2 o 3 las que sean distinto de rellenadas </param>
        ''' <param name="idOrganiz">Si viene informado se obtendran las hojas cuyo validador sea el indicado</param>
        ''' <returns></returns>        
        Function getIntegrantesConHojaGastos(ByVal IdViaje As Integer, ByVal opcion As Integer, Optional ByVal idOrganiz As Integer = Integer.MinValue) As List(Of SabLib.ELL.Usuario)
            Dim oUser As SabLib.ELL.Usuario
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim bGetUsuario As Boolean
            Dim lUsuarios As New List(Of SabLib.ELL.Usuario)
            Dim listaUsuarios As List(Of String()) = hojaGastosDAL.getIntegrantesConHojaGastos(IdViaje, idOrganiz)
            For Each user As String() In listaUsuarios
                bGetUsuario = False
                Select Case opcion
                    Case 0  'TODOS
                        bGetUsuario = True
                    Case 1  'Rellenadas
                        bGetUsuario = (CInt(user(1)) = ELL.HojaGastos.eEstado.Rellenada)
                    Case 2  'Validadas
                        bGetUsuario = (CInt(user(1)) = ELL.HojaGastos.eEstado.Validada)
                    Case 3  'Todas menos la rellenadas
                        bGetUsuario = (CInt(user(1)) <> ELL.HojaGastos.eEstado.Rellenada)
                End Select
                If (bGetUsuario) Then
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(user(0))}, False)
                    If (oUser IsNot Nothing) Then lUsuarios.Add(oUser)
                End If
            Next
            Return lUsuarios
        End Function

        ''' <summary>
        ''' Rellena las lineas de las hojas de gastos y los movimientos de visa
        ''' </summary>
        ''' <param name="lHojas">Hojas de gastos</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        Private Sub FillMovimientos(ByRef lHojas As List(Of ELL.HojaGastos), ByVal idPlanta As Integer)
            Dim hoja As ELL.HojaGastos
            For index = lHojas.Count - 1 To 0 Step -1
                hoja = lHojas.Item(index)
                FillMovimientos(hoja, idPlanta)
            Next
        End Sub

        ''' <summary>
        ''' Rellena las lineas de la hoja de gastos y los movimientos de visa
        ''' </summary>
        ''' <param name="oHoja"></param>
        ''' <param name="idPlanta">Id de la planta</param>
        Private Sub FillMovimientos(ByRef oHoja As ELL.HojaGastos, ByVal idPlanta As Integer)
            Dim visaBLL As New BLL.VisasBLL
            Dim fechaDesde, fechaHasta As Date
            oHoja.Lineas = hojaGastosDAL.loadLineas(oHoja.Id)
            If (oHoja.IdViaje = Integer.MinValue) Then  'Libre sin viaje asociado
                fechaDesde = oHoja.FechaDesde
                fechaHasta = oHoja.FechaHasta
            Else
                fechaDesde = DateTime.MinValue
                fechaHasta = DateTime.MinValue
            End If
            'Estos movimientos son los que vienen del banco
            Dim idViaje As Nullable(Of Integer) = Nothing
            If (oHoja.IdViaje <> Integer.MinValue) Then idViaje = oHoja.IdViaje
            oHoja.MovimientosVisa = visaBLL.loadMovimientos(oHoja.Usuario.Id, idViaje, idPlanta, fechaDesde, fechaHasta, False, idHojaLibre:=oHoja.Id)
        End Sub

        ''' <summary>
        ''' Carga las lineas de una hoja de gastos
        ''' No carga las visas
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="bLoadObjects">Indica si se cargaran los objetos usuarios y monedas</param>
        ''' <returns></returns>        
        Private Function FillMovimientos(ByVal idHoja As Integer, ByVal bLoadObjects As Boolean) As List(Of ELL.HojaGastos.Linea)
            Return hojaGastosDAL.loadLineas(idHoja, bLoadObjects)
        End Function

        ''' <summary>
        ''' Obtiene los diversos estados y sus fechas por las que ha pasado una hoja de gastos
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <returns></returns>        
        Public Function loadStates(ByVal idHoja As Integer) As List(Of String())
            Return hojaGastosDAL.loadStates(idHoja)
        End Function

        ''' <summary>
        ''' Obtiene el fichero del banco
        ''' </summary>
        ''' <param name="idCabLiq">Id de la cabecera de liquidacion</param>
        ''' <param name="tipoLiq">Tipo de la liquidacion</param>
        ''' <returns>Fichero del banco</returns>        
        Public Function loadFicheroBancoLiq(ByVal idCabLiq As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq) As Byte()
            Return hojaGastosDAL.loadFicheroBancoLiq(idCabLiq, tipoLiq)
        End Function

        ''' <summary>
        ''' Devuelve los trayectos distintos de kilometraje registrados en alguna otra hoja del usuario
        ''' </summary>
        ''' <param name="idUser">Usuario de las hojas</param>
        ''' <param name="idHoja">Id de la hoja de la que no mostrara los trayectos</param>
        ''' <returns></returns>        
        Function loadTrayectosKilometraje(ByVal idUser As Integer, ByVal idHoja As Integer) As List(Of ELL.HojaGastos.Linea)
            Return hojaGastosDAL.loadTrayectosKilometraje(idUser, idHoja)
        End Function

        ''' <summary>
        ''' Obtiene el importe de liquidacion de una hoja
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <returns></returns>
        Public Function loadImporteLiquidacion(ByVal idHoja As Integer) As Decimal
            Return hojaGastosDAL.loadImporteLiquidacion(idHoja)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Cambia el estado de una hoja de gastos
        ''' </summary>
        ''' <param name="id">Id de la hoja</param>        
        ''' <param name="estado">Estado</param>        
        Public Function ChangeState(ByVal id As Integer, ByVal estado As Integer, Optional ByVal myConnection As OracleConnection = Nothing) As ELL.HojaGastos
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                'Dim fechaLineaMenor, fechaLineaMayor As Date                
                Dim visaBLL As New BLL.VisasBLL
                If (myConnection Is Nothing) Then
                    myConn = New OracleConnection(hojaGastosDAL.Conexion)
                    myConn.Open()
                    transact = myConn.BeginTransaction()
                Else
                    myConn = myConnection
                End If
                Dim oHoja As ELL.HojaGastos = loadHoja(id)
                If (oHoja.MovimientosVisa IsNot Nothing AndAlso oHoja.MovimientosVisa.Count > 0) Then
                    For Each oMov As ELL.Visa.Movimiento In oHoja.MovimientosVisa
                        If (estado = ELL.HojaGastos.eEstado.Validada) Then
                            oMov.Estado = ELL.Visa.Movimiento.eEstado.Conforme
                        ElseIf (estado = ELL.HojaGastos.eEstado.Rellenada) Then
                            oMov.Estado = ELL.Visa.Movimiento.eEstado.Cargado
                        ElseIf (estado = ELL.HojaGastos.eEstado.Liquidada) Then
                            oMov.Estado = ELL.Visa.Movimiento.eEstado.Liquidado
                        End If
                    Next
                    visaBLL.CambiarEstadoMovimientos(oHoja.MovimientosVisa, myConn)
                End If
                hojaGastosDAL.ChangeState(id, estado, myConn)
                '290413: Ahora las HG son mensuales asi que no habra que modificar las fechas al enviar la hoja
                'If (oHoja.IdSinViaje <> Integer.MinValue) Then  'Si la hg pertenece a un viaje, no habra que modificar las fechas
                '    If (estado = ELL.HojaGastos.eEstado.Enviada) Then
                '        If (oHoja.Lineas IsNot Nothing AndAlso oHoja.Lineas.Count > 0) Then
                '            oHoja.Lineas.Sort(Function(o1 As ELL.HojaGastos.Linea, o2 As ELL.HojaGastos.Linea) o1.Fecha < o2.Fecha)
                '            fechaLineaMenor = oHoja.Lineas.First.Fecha
                '            fechaLineaMayor = oHoja.Lineas.Last.Fecha                            
                '        ElseIf (oHoja.MovimientosVisa IsNot Nothing AndAlso oHoja.MovimientosVisa.Count > 0) Then  'sino hay movimientos de linea, se propondra la fecha de movimientos de visa
                '            oHoja.MovimientosVisa.Sort(Function(o1 As ELL.Visa.Movimiento, o2 As ELL.Visa.Movimiento) o1.Fecha < o2.Fecha)
                '            fechaLineaMenor = oHoja.MovimientosVisa.First.Fecha
                '            fechaLineaMayor = oHoja.MovimientosVisa.Last.Fecha
                '        End If
                '        SaveFechas(CInt(id), fechaLineaMenor, fechaLineaMayor, myConn)                        
                '    End If
                'End If
                If (myConnection Is Nothing) Then transact.Commit()
                Return oHoja
            Catch batzEx As BidaiakLib.BatzException
                If (myConnection Is Nothing) Then transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (myConnection Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al cambiar el estado de la hoja de gastos", ex)
            Finally
                If (myConnection Is Nothing AndAlso myConn IsNot Nothing AndAlso myConn.State <> ConnectionState.Closed) Then myConn.Close()
            End Try
        End Function

        ''' <summary>
        ''' Cambia el estado de varias hojas de gastos
        ''' </summary>
        ''' <param name="ids">Ids de la hojas</param>         
        ''' <param name="estado">Estado</param>        
        Public Sub ChangeState(ByVal ids As String(), ByVal estado As Integer)
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                'Dim fechaLineaMenor, fechaLineaMayor As Date
                Dim oHoja As ELL.HojaGastos
                myConn = New OracleConnection(hojaGastosDAL.Conexion)
                myConn.Open()
                transact = myConn.BeginTransaction()
                For Each id As String In ids
                    oHoja = ChangeState(CInt(id), estado, myConn)
                    ''30/04/13: Si se cambia el estado a enviada, habra que actualizar las fechas de la HG a la fecha de inicio de la linea con menor fecha y la fecha de hasta con la linea con mayor fecha
                    'If (estado = ELL.HojaGastos.eEstado.Enviada) Then
                    '    oHoja.Lineas.Sort(Function(o1 As ELL.HojaGastos.Linea, o2 As ELL.HojaGastos.Linea) o1.Fecha < o2.Fecha)
                    '    fechaLineaMenor = oHoja.Lineas.First.Fecha
                    '    fechaLineaMayor = oHoja.Lineas.Last.Fecha
                    '    SaveFechas(CInt(id), fechaLineaMenor, fechaLineaMayor, myConn)                        
                    'End If
                Next
                transact.Commit()
            Catch batzEx As BidaiakLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al cambiar el estado de las hojas de gastos", ex)
            Finally
                If (myConn IsNot Nothing AndAlso myConn.State <> ConnectionState.Closed) Then myConn.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Se actualiza el validador de una hoja de gastos
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="idValidador">Id del nuevo validador</param>
        Public Sub UpdateHGValidator(ByVal idHoja As Integer, ByVal idValidador As Integer)
            hojaGastosDAL.UpdateHGValidator(idHoja, idValidador)
        End Sub

        ''' <summary>
        ''' Marca como entregada en administracion la hoja
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="fechaEntrega">Fecha de entrega</param>        
        Public Sub EntregarHGAdministracion(ByVal idHoja As Integer, ByVal fechaEntrega As Date)
            hojaGastosDAL.EntregarHGAdministracion(idHoja, fechaEntrega)
        End Sub


        ''' <summary>
        ''' Crea una cabecera de una HG sin lineas
        ''' </summary>
        ''' <param name="oHoja">Datos de la cabecera</param>                   
        Public Function CreateCabecera(ByVal oHoja As ELL.HojaGastos) As Integer
            'Dim myConn As OracleConnection = Nothing
            'Dim transact As OracleTransaction = Nothing
            Try
                Dim visaBLL As New BLL.VisasBLL
                'myConn = New OracleConnection(hojaGastosDAL.Conexion)
                'myConn.Open()
                'transact = myConn.BeginTransaction()
                Dim oLinea As New ELL.HojaGastos.Linea With {.Usuario = New SabLib.ELL.Usuario With {.Id = oHoja.Usuario.Id}}
                hojaGastosDAL.AddLinea(oLinea, oHoja.IdViaje, oHoja.Validador.Id, oHoja.FechaDesde, oHoja.FechaHasta, True, Nothing)
                If (oHoja.Estado = ELL.HojaGastos.eEstado.Validada) Then ChangeState(oLinea.IdHoja, ELL.HojaGastos.eEstado.Enviada)
                ChangeState(oLinea.IdHoja, oHoja.Estado)
                'transact.Commit()
                Return oLinea.IdHoja
            Catch batzEx As BidaiakLib.BatzException
                'transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                'transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al crear la cabecera de la HG", ex)
            Finally
                'If (myConn IsNot Nothing AndAlso myConn.State <> ConnectionState.Closed) Then myConn.Close()
            End Try
        End Function

        ''' <summary>
        ''' Integra las hojas de gastos
        ''' </summary>
        ''' <param name="hojasLiq">Lista con las hojas e importes de la misma</param>    
        ''' <param name="fechaEmision">Fecha de emision</param>                           
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <param name="hubContext">Contexto del hub de SignalR</param>
        ''' <returns>Id de la liquidacion de la cabecera</returns>
        Public Function Integrar(ByVal hojasLiq As List(Of String()), ByVal fechaEmision As Date, ByVal idPlanta As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq, Optional ByVal hubContext As Object = Nothing) As Integer
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                myConn = New OracleConnection(hojaGastosDAL.Conexion)
                myConn.Open()
                transact = myConn.BeginTransaction()
                Dim ratioActualizacionProgress As Integer = (hojasLiq.Count / 50) + 1  '50 es el numero de actualizaciones que tendra la progress bar, sea el numero de items que sea
                'Se marcan como liquidadas y a la vez miramos si hay hojas de gastos de usuarios subcontratados para que no se traten en los dos siguientes casos
                Dim epsilonBLL As New BLL.Epsilon(idPlanta)
                Dim oHoja As ELL.HojaGastos
                Dim hojasLiqBatz As New List(Of String())
                Dim index As Integer = 1
                log.Info("LIQUIDACION: Se van a integrar " & hojasLiq.Count & " hojas de gastos")
                For Each hLiq As String() In hojasLiq
                    If (hubContext IsNot Nothing) Then
                        hubContext.showMessage(index & " de " & hojasLiq.Count, 1)
                        If (index Mod ratioActualizacionProgress = 0) Then hubContext.showProgress(index + 1, hojasLiq.Count, 1) 'Para que la actualizacion de la progress bar sea mas fina
                    End If
                    'Se marca como liquidada
                    ChangeState(CInt(hLiq(0)), ELL.HojaGastos.eEstado.Liquidada, myConn)
                    'Se mira es un subcontratado o es de Batz
                    oHoja = loadHoja(CInt(hLiq(0)), False)
                    Dim info As String() = epsilonBLL.GetInfoPersona(oHoja.Usuario.Dni)
                    If (info IsNot Nothing) Then
                        hojasLiqBatz.Add(hLiq)
                    Else
                        Throw New BatzException("No se ha podido obtener la informacion de la persona " & oHoja.Usuario.CodPersona, Nothing)
                    End If
                    log.Info("LIQUIDACION: HG " & CInt(hLiq(0)) & " marcada como liquidada")
                    index += 1
                Next
                log.Info("LIQUIDACION: Hojas de gastos integradas. Ahora se va a proceder a generar el fichero del banco")
                'Se genera el fichero del banco, solo con los usuarios de batz
                If (hubContext IsNot Nothing) Then hubContext.showMessage("Generando...", 2)
                Dim fichero As String = GenerarFicheroBanco(hojasLiqBatz, fechaEmision, idPlanta, tipoLiq)
                Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)  'El fichero se tiene que guardar en ANSI
                Dim file As Byte() = enc.GetBytes(fichero)
                If (hubContext IsNot Nothing) Then hubContext.showMessage("Generado", 2)
                log.Info("LIQUIDACION: Fichero del banco liquidado. Ahora se va a proceder a guardar los registros de la liquidacion actual")
                'Se registra la liquidacion con todos
                If (hubContext IsNot Nothing) Then hubContext.showMessage("Guardando...", 3)
                Dim idLiq As Integer = InsertLiquidacion(fechaEmision, file, idPlanta, hojasLiq, myConn)
                If (hubContext IsNot Nothing) Then hubContext.showMessage("Guardado", 3)
                log.Info("LIQUIDACION: Registros guardados. El id de la liquidacion es:" & idLiq)
                transact.Commit()
                Return idLiq
            Catch batzEx As BidaiakLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al cambiar el estado de las hojas de gastos", ex)
            Finally
                If (myConn IsNot Nothing AndAlso myConn.State <> ConnectionState.Closed) Then myConn.Close()
            End Try
        End Function

        ''' <summary>
        ''' Integra las hojas de gastos de una factura
        ''' </summary>
        ''' <param name="idCab">Id de la cabecera</param>
        ''' <param name="hojasLiq">Lista con las hojas y su numero de factura</param>
        ''' <param name="idPlanta">Id de la planta</param>        
        Public Sub IntegrarFact(ByVal idCab As Integer, ByVal hojasLiq As List(Of String()), ByVal idPlanta As Integer)
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                myConn = New OracleConnection(hojaGastosDAL.Conexion)
                myConn.Open()
                transact = myConn.BeginTransaction()
                'Se marcan como liquidadas
                Dim epsilonBLL As New BLL.Epsilon(idPlanta)
                Dim hojasLiqBatz As New List(Of String())
                For Each sLiq As String() In hojasLiq
                    ChangeState(sLiq(0), ELL.HojaGastos.eEstado.Liquidada, myConn) 'Se marca como liquidada                    
                    hojaGastosDAL.UpdateTransferencia(sLiq(0), sLiq(1), myConn)
                Next
                hojaGastosDAL.CloseTransferencia(idCab, myConn)
                transact.Commit()
            Catch batzEx As BidaiakLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al integrar la liquidacion de factura", ex)
            Finally
                If (myConn IsNot Nothing AndAlso myConn.State <> ConnectionState.Closed) Then myConn.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Transfiere las hojas de gastos
        ''' </summary>
        ''' <param name="hojasLiq">Lista con las hojas e importes de la misma</param>
        ''' <param name="idPlantaGestion">Id de la planta de gestion</param>
        ''' <param name="idPlantaFact">Id de la planta a la que se le factura</param>
        ''' <param name="idOtraEmpresa">Id de la empresa de convenio/cat a la que se factura</param>
        ''' <returns></returns>        
        Public Function Transferir(ByVal hojasLiq As List(Of String()), ByVal idPlantaGestion As Integer, ByVal idPlantaFact As Integer, ByVal idOtraEmpresa As Integer) As Integer
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                myConn = New OracleConnection(hojaGastosDAL.Conexion)
                myConn.Open()
                transact = myConn.BeginTransaction()
                'Se marcan como transferidas y a la vez miramos si hay hojas de gastos de usuarios subcontratados para que no se traten en los dos siguientes casos
                Dim epsilonBLL As New BLL.Epsilon(idPlantaGestion)
                Dim oHoja As ELL.HojaGastos
                Dim hojasLiqBatz As New List(Of String())
                For Each hLiq As String() In hojasLiq
                    'Se marca como liquidada
                    ChangeState(CInt(hLiq(0)), ELL.HojaGastos.eEstado.Transferida, myConn)
                    'Se mira es un subcontratado o es de Batz
                    oHoja = loadHoja(CInt(hLiq(0)), False)
                    Dim info As String() = epsilonBLL.GetInfoPersona(oHoja.Usuario.Dni)
                    If (info IsNot Nothing) Then
                        hojasLiqBatz.Add(hLiq)
                    Else
                        Throw New BatzException("No se ha podido obtener la informacion de la persona " & oHoja.Usuario.CodPersona, Nothing)
                    End If
                Next
                Dim oLiqCab As New ELL.HojaGastos.Liquidacion.Cabecera With {.IdPlanta = idPlantaGestion, .IdPlantaFactura = idPlantaFact, .IdConvCatEmpresaFactura = idOtraEmpresa, .FechaEmision = Now, .TipoLiquidacion = ELL.HojaGastos.Liquidacion.TipoLiq.Factura}
                'Se genera el fichero del banco, solo con los usuarios de batz
                If (idPlantaFact > 0) Then
                    Try
                        Dim fichero As String = GenerarFicheroBanco(hojasLiqBatz, oLiqCab.FechaEmision, idPlantaGestion, ELL.HojaGastos.Liquidacion.TipoLiq.Factura, idPlantaFact)
                        'Dim enc As New System.Text.UTF8Encoding
                        Dim enc As System.Text.Encoding = System.Text.Encoding.GetEncoding(1252)  'El fichero se tiene que guardar en ANSI
                        oLiqCab.Fichero = enc.GetBytes(fichero)
                    Catch ex As Exception
                        log.Warn("No se ha podido generar el fichero del banco", ex)
                    End Try
                End If
                'Se registra la liquidacion con todos
                Dim idLiq As Integer = InsertTransferencia(oLiqCab, hojasLiq, myConn)
                transact.Commit()
                Return idLiq
            Catch batzEx As BidaiakLib.BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al cambiar el estado de las hojas de gastos", ex)
            Finally
                If (myConn IsNot Nothing AndAlso myConn.State <> ConnectionState.Closed) Then myConn.Close()
            End Try
        End Function

        ''' <summary>
        ''' Comprueba si una hoja esta excluida
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <returns></returns>
        Public Function IsHGExcluded(ByVal idHoja As Integer) As Boolean
            Return hojaGastosDAL.IsHGExcluded(idHoja)
        End Function

        ''' <summary>
        ''' Excluye la hoja de gastos del listado de la liquidacion
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>        
        Public Sub ExcluirHG(ByVal idHoja As Integer)
            hojaGastosDAL.ExcluirHG(idHoja)
        End Sub

        ''' <summary>
        ''' Quita de la exclusion la hoja de gastos del listado de la liquidacion
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param> 
        Public Sub QuitarExclusionHG(ByVal idHoja As Integer)
            hojaGastosDAL.QuitarExclusionHG(idHoja)
        End Sub

        '''' <summary>
        '''' Genera el fichero para el banco
        '''' Obsoleta desde el 01/02/2016
        '''' </summary>
        '''' <param name="hojasLiq">Lista con las hojas e importes de la misma</param>   
        '''' <param name="fechaEmision">Fecha de emision</param>            
        '''' <param name="idPlanta">Id de la planta</param>        
        '''' <param name="tipoLiq">Tipo de liquidacion</param>
        '''' <returns>Devuelve el texto del fichero</returns>
        '''' <remarks>Se modifica en enero del 2014 para la nueva especificacion SEPA</remarks>
        'Public Function GenerarFicheroBanco_Old(ByVal hojasLiq As List(Of String()), ByVal fechaEmision As Date, ByVal idPlanta As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq, Optional ByVal idPlantaFact As Integer = 0) As String
        '    Dim index As Integer
        '    Try
        '        Dim fichero As New Text.StringBuilder
        '        Dim userBLL As New SabLib.BLL.UsuariosComponent
        '        If (idPlantaFact = 0) Then idPlantaFact = idPlanta 'Esto es porque para liquidaciones de otras plantas, la cabecera tiene que ser de la planta de facturacion pero la informacion de las hojas de la suya
        '        Dim epsilonBLL As New BLL.Epsilon(idPlantaFact)
        '        Dim oUser As SabLib.ELL.Usuario
        '        Dim registro, direccionCompleta, cif, nombre, direccion, poblacion, provincia, cuenta, importeFormateado, sImporteHoja, nifActual As String
        '        Dim idUserActual, codPersoActual, totalRegistros, totalTransf, idHoja As Integer
        '        Dim importe, importeActual As Decimal
        '        Dim lHojas As List(Of ELL.HojaGastos.Liquidacion) = loadLiquidaciones(idPlanta, tipoLiq)
        '        Dim infoEmpresa As String() = epsilonBLL.getInfoEmpresa()
        '        Dim cuentaPago As String() = epsilonBLL.getCuentaPago()
        '        Dim myHojaLiq As ELL.HojaGastos.Liquidacion = Nothing
        '        totalRegistros = 0 : totalTransf = 0 : importeActual = 0 : importeFormateado = String.Empty
        '        idUserActual = 0 : codPersoActual = 0 : nifActual = 0

        '        '1-REGISTRO CABECERA ORDENANTE
        '        '--------------------------------------------------------
        '        cuenta = "ES" & cuentaPago(4).Trim & cuentaPago(0).Trim & cuentaPago(1).Trim & cuentaPago(2).Trim & cuentaPago(3).Trim  'ES Iban Banco Sucursal DC Cuenta
        '        cuenta = cuenta.PadRight(34)
        '        nombre = infoEmpresa(0).Trim.PadRight(70) : cif = infoEmpresa(1).Trim.PadRight(9)
        '        direccion = infoEmpresa(2).Trim & " " & infoEmpresa(3).Trim & " " & infoEmpresa(4).Trim : direccion = direccion.PadRight(50)
        '        poblacion = infoEmpresa(5).Trim & " " & infoEmpresa(6).Trim : poblacion = poblacion.PadRight(50)
        '        provincia = infoEmpresa(7).Trim.PadRight(40)
        '        direccionCompleta = direccion & poblacion & provincia & "ES"
        '        registro = "01ORD34145001" & cif & "000" & Date.Now.ToString("yyyyMMdd") & fechaEmision.ToString("yyyyMMdd") & "A" & cuenta & "0" & nombre & direccionCompleta
        '        registro = registro.PadRight(600)
        '        fichero.AppendLine(registro)
        '        '--------------------------------------------------------

        '        '2-REGISTRO CABECERA
        '        '2.1.Cabecera 
        '        '--------------------------------------------------------
        '        registro = "02SCT34145" & cif & "000" : registro = registro.PadRight(600)
        '        fichero.AppendLine(registro)
        '        '--------------------------------------------------------

        '        '2.2.Registros de beneficiarios
        '        '-------------------------------------------------------
        '        lHojas.Sort(Function(o1 As ELL.HojaGastos.Liquidacion, o2 As ELL.HojaGastos.Liquidacion) o1.Usuario.Id < o2.Usuario.Id)
        '        totalRegistros = 2  'Las dos cabeceras
        '        For index = 0 To hojasLiq.Count - 1
        '            idHoja = CInt(hojasLiq(index)(0))
        '            myHojaLiq = Nothing
        '            For Each oHoj As ELL.HojaGastos.Liquidacion In lHojas
        '                If (oHoj.Hojas.Exists(Function(o As ELL.HojaGastos.Liquidacion.Hoja) o.IdHoja = idHoja)) Then
        '                    myHojaLiq = oHoj
        '                    Exit For
        '                End If
        '            Next
        '            sImporteHoja = hojasLiq(index)(1)  'Nos quedamos con el importe correspondiente a la hoja                    
        '            If (CInt(myHojaLiq.Usuario.Id) <> idUserActual) Then  'Se cambia de trabajador
        '                If (idUserActual = 0) Then 'Es el primero                            
        '                    importe = CDec(sImporteHoja.Replace(".", ","))
        '                    importeActual = importe
        '                    importeFormateado = formatearImporte(importe, 11)
        '                Else 'No es el primero                            
        '                    fichero.AppendLine(GenerateRegistroTransferencia_Old(nifActual, codPersoActual, fechaEmision, cif, importeActual, idPlanta))

        '                    importeActual = CDec(sImporteHoja.Replace(".", ","))
        '                    importe += importeActual
        '                    importeFormateado = formatearImporte(importeActual, 11)
        '                    totalTransf += 1 : totalRegistros += 1
        '                End If
        '                idUserActual = myHojaLiq.Usuario.Id
        '                codPersoActual = myHojaLiq.Usuario.CodPersona
        '                nifActual = myHojaLiq.Usuario.Dni
        '            Else  'Es el mismo
        '                importeActual += CDec(sImporteHoja.Replace(".", ","))
        '                importe += CDec(sImporteHoja.Replace(".", ","))
        '                importeFormateado = formatearImporte(importeActual, 11)
        '            End If
        '        Next
        '        'Imprimimos los datos del ultimo                              
        '        oUser = myHojaLiq.Usuario
        '        fichero.AppendLine(GenerateRegistroTransferencia_Old(oUser.Dni, oUser.CodPersona, fechaEmision, cif, importeActual, idPlanta))
        '        '-------------------------------------------------------
        '        totalRegistros += 1 : totalTransf += 1

        '        '2.3.Registro de totales
        '        '---------------------------------------------------------                
        '        Dim sTotalTransf As String = totalTransf.ToString.PadLeft(8, "0")
        '        Dim sTotalRegTransf As String = (totalTransf + 2).ToString.PadLeft(10, "0") 'Transferencias + cabecera + totales
        '        registro = "04SCT" & formatearImporte(importe, 17) & sTotalTransf & sTotalRegTransf : registro = registro.PadRight(600)
        '        fichero.AppendLine(registro)
        '        totalRegistros += 1
        '        '---------------------------------------------------------

        '        '3-REGISTRO DE TOTALES GENERALES
        '        '---------------------------------------------------------
        '        totalRegistros += 1
        '        Dim sTotalReg As String = totalRegistros.ToString.PadLeft(10, "0")
        '        registro = "99ORD" & formatearImporte(importe, 17) & sTotalTransf & sTotalReg : registro = registro.PadRight(600)
        '        fichero.AppendLine(registro)
        '        '--------------------------------------------------------

        '        Return fichero.ToString
        '    Catch batzEx As BidaiakLib.BatzException
        '        Throw batzEx
        '    Catch ex As Exception
        '        Throw New BidaiakLib.BatzException("Error al generar el fichero del banco", ex)
        '    End Try
        'End Function

        ''' <summary>
        ''' Genera un registro de transferencia de una persona
        ''' Obsoleta desde el 01/02/2016
        ''' </summary>
        ''' <param name="nif">Dni</param>
        ''' <param name="codPersona">Codigo de persona</param>        
        ''' <param name="fechaEmision">Fecha de emision</param>
        ''' <param name="cif">Cif de la empresa</param>
        ''' <param name="importe">Importe</param>
        ''' <param name="idPlanta">Id de la planta</param>
        Private Function GenerateRegistroTransferencia_Old(ByVal nif As String, ByVal codPersona As Integer, ByVal fechaEmision As Date, ByVal cif As String, ByVal importe As Decimal, ByVal idPlanta As Integer) As String
            Dim concepto, nombreTra, direccionTra, cuenta, swift_bic, registro As String
            Dim epsilonBLL As New BLL.Epsilon(idPlanta)
            Dim sInfoPersona As String() = epsilonBLL.getInfoBancoTrabajador(nif)
            If (sInfoPersona Is Nothing) Then Throw New BidaiakLib.BatzException("Existe un problema al recuperar la informacion de epsilon del trabajador " & codPersona, Nothing)

            'nif, apellido1, apellido2, nombre, siglas, domicilio, piso, cpostal, d_poblacion, id_banco, id_sucursal, id_cuenta, dc_cta,dc_iban
            nif = sInfoPersona(0).Trim : nif = nif.PadRight(35)
            nombreTra = sInfoPersona(1).Trim & If(sInfoPersona(2) Is Nothing, "", " " & sInfoPersona(2).Trim) & ", " & sInfoPersona(3).Trim
            nombreTra = nombreTra.PadRight(70)
            cuenta = "ES" & sInfoPersona(13).Trim & sInfoPersona(9).Trim & sInfoPersona(10).Trim & sInfoPersona(12).Trim & sInfoPersona(11).Trim
            cuenta = cuenta.PadRight(34)
            direccionTra = String.Empty : direccionTra = direccionTra.PadRight(142)
            concepto = "BATZ " & fechaEmision.Year & ", bidai-gastuak"
            concepto = concepto.PadRight(140)
            swift_bic = sInfoPersona(14).Trim : swift_bic = swift_bic.PadRight(11)
            cif = cif.PadRight(35)

            registro = "03SCT34145002" & cif & "A" & cuenta & formatearImporte(importe, 11) & "3" & swift_bic & nombreTra & direccionTra & concepto & nif & "SALAOTHR" : registro = registro.PadRight(600)
            Return registro
        End Function

        ''' <summary>
        ''' Genera el fichero para el banco
        ''' </summary>
        ''' <param name="hojasLiq">Lista con las hojas e importes de la misma</param>   
        ''' <param name="fechaEmision">Fecha de emision</param>            
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <returns>Devuelve el texto del fichero</returns>
        ''' <remarks>Se modifica en enero del 2016 para la nueva especificacion SEPA</remarks>
        Public Function GenerarFicheroBanco(ByVal hojasLiq As List(Of String()), ByVal fechaEmision As Date, ByVal idPlanta As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq, Optional ByVal idPlantaFact As Integer = 0) As String
            Dim index As Integer
            Try
                Dim fichero As New Text.StringBuilder
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                If (idPlantaFact = 0) Then idPlantaFact = idPlanta 'Esto es porque para liquidaciones de otras plantas, la cabecera tiene que ser de la planta de facturacion pero la informacion de las hojas de la suya
                Dim epsilonBLL As New BLL.Epsilon(idPlantaFact)
                Dim oUser As SabLib.ELL.Usuario
                Dim direccionCompleta, cif, nombre, direccion, poblacion, provincia, cuenta, importeFormateado, sImporteHoja, nifActual, bic, msgId As String
                Dim idUserActual, codPersoActual, totalOperaciones, idHoja As Integer
                Dim importe, importeActual As Decimal
                Dim lHojas As List(Of ELL.HojaGastos.Liquidacion) = loadLiquidaciones(idPlanta, tipoLiq)
                Dim infoEmpresa As String() = epsilonBLL.getInfoEmpresa()
                Dim cuentaPago As String() = epsilonBLL.getCuentaPago()
                Dim myHojaLiq As ELL.HojaGastos.Liquidacion = Nothing
                totalOperaciones = 0 : importeActual = 0 : importeFormateado = String.Empty
                idUserActual = 0 : codPersoActual = 0 : nifActual = 0

                nombre = infoEmpresa(0).Trim : cif = infoEmpresa(1).Trim & "000" '11/02/2025: Cambia la norma y hay que enviarlo con 3 ceros más
                direccion = infoEmpresa(2).Trim & " " & infoEmpresa(3).Trim & " " & infoEmpresa(4).Trim
                poblacion = infoEmpresa(5).Trim & " - " & infoEmpresa(6).Trim
                provincia = infoEmpresa(7).Trim
                direccionCompleta = direccion & ",  " & poblacion & " " & provincia
                cuenta = "ES" & cuentaPago(4).Trim & cuentaPago(0).Trim & cuentaPago(1).Trim & cuentaPago(2).Trim & cuentaPago(3).Trim  'ES Iban Banco Sucursal DC Cuenta
                msgId = "BG_" & fechaEmision.ToString("yyyyMMdd")
                bic = cuentaPago(5).Trim

                '1-XML HEADER
                '--------------------------------------------------------
                fichero.AppendLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                fichero.AppendLine("<Document xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.001.001.03"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">")
                fichero.AppendLine("<CstmrCdtTrfInitn>") 'iniciación de adeudos directos.

                '2-GROUP HEADER
                '--------------------------------------------------------
                fichero.AppendLine("<GrpHdr>") ' Conjunto de características compartidas por todas las operaciones incluidas en el mensaje
                fichero.AppendLine("    <MsgId>" & msgId & "</MsgId>") 'Referencia de cliente a entidad financiera que asigna la parte iniciadora o el propio ordenante, para identificar el mensaje de forma inequívoca al enviarlo a su entidad financiera
                fichero.AppendLine("    <CreDtTm>" & Date.Now.ToString("yyyy-MM-dd") & "T" & fechaEmision.ToString("hh:mm:ss") & "</CreDtTm>") 'Fecha y hora cuando la parte iniciadora ha creado la instrucciones de pago.
                fichero.AppendLine("    <NbOfTxs>[TOTAL_OPERACIONES]</NbOfTxs>") 'Número de operaciones individuales que contiene el mensaje
                fichero.AppendLine("    <CtrlSum>[TOTAL_IMPORTE]</CtrlSum>") 'Suma total de todos los importes individuales incluidos en el mensaje, sin tener en cuenta las divisas. 18 digitos y 2 decimales. Separador el .
                fichero.AppendLine("    <InitgPty>") 'Para que inicia el pago
                fichero.AppendLine("        <Nm>" & nombre & "</Nm>") 'Nombre
                fichero.AppendLine("        <Id>") 'Identificacion
                fichero.AppendLine("            <OrgId>") 'Persona juridica
                fichero.AppendLine("                <Othr>") 'Otro
                fichero.AppendLine("                    <Id>" & cif & "</Id>") 'NIF
                fichero.AppendLine("                </Othr>")
                fichero.AppendLine("            </OrgId>")
                fichero.AppendLine("        </Id>")
                fichero.AppendLine("    </InitgPty>")
                fichero.AppendLine("</GrpHdr>")

                '3-PAYMENT INFORMATION
                '--------------------------------------------------------
                fichero.AppendLine("<PmtInf>") 'onjunto de características que se aplican a la parte del acreedor de las operaciones de pago incluidas en el mensaje de iniciación de adeudos directos.
                fichero.AppendLine("    <PmtInfId>" & cif & "</PmtInfId>") 'Referencia asignada por el ordenante para identificar claramente el bloque de información de pago dentro del mensaje
                fichero.AppendLine("    <PmtMtd>TRF</PmtMtd>") 'Metodo de pago
                fichero.AppendLine("    <BtchBookg>true</BtchBookg>") 'True:Se apunta un solo apunte en cuenta con el coste total / False: Se apunta un apunte en cuenta por cada operación individual
                fichero.AppendLine("    <NbOfTxs>[TOTAL_OPERACIONES]</NbOfTxs>") 'Número de operaciones individuales que contiene el mensaje
                fichero.AppendLine("    <CtrlSum>[TOTAL_IMPORTE]</CtrlSum>") 'Suma total de todos los importes individuales incluidos en el mensaje, sin tener en cuenta las divisas. 18 digitos y 2 decimales. Separador el .
                fichero.AppendLine("    <ReqdExctnDt>" & Now.ToString("yyyy-MM-dd") & "</ReqdExctnDt>") 'Fecha de ejecución solicitada
                fichero.AppendLine("    <Dbtr>") 'Ordenante
                fichero.AppendLine("        <Nm>" & nombre & "</Nm>") 'Nombre                
                fichero.AppendLine("        <Id>") 'Identificacion
                fichero.AppendLine("            <OrgId>") 'Persona juridica
                fichero.AppendLine("                <Othr>") 'Otro
                fichero.AppendLine("                    <Id>" & cif & "</Id>") 'CIF
                fichero.AppendLine("                </Othr>")
                fichero.AppendLine("            </OrgId>")
                fichero.AppendLine("        </Id>")
                fichero.AppendLine("    </Dbtr>")
                fichero.AppendLine("    <DbtrAcct>")  'Cuenta del ordenante
                fichero.AppendLine("        <Id>") 'Identificacion
                fichero.AppendLine("            <IBAN>" & cuenta & "</IBAN>") 'IBAN
                fichero.AppendLine("        </Id>")
                fichero.AppendLine("    </DbtrAcct>")
                fichero.AppendLine("    <DbtrAgt>") 'Entidad del ordenante
                fichero.AppendLine("        <FinInstnId>") 'dentificación
                fichero.AppendLine("            <BIC>" & bic & "</BIC>") 'Bank Identifier Code
                fichero.AppendLine("        </FinInstnId>")
                fichero.AppendLine("    </DbtrAgt>")
                fichero.AppendLine("    <ChrgBr>SLEV</ChrgBr>") 'Clausula de gastos: Detalla cuál de las partes intervinientes soportará las comisiones y gastos ligados al procesamiento de la operación

                '3-INDIVIDUAL TRANSFERENCE
                '--------------------------------------------------------
                lHojas = lHojas.OrderBy(Of Integer)(Function(o) o.Usuario.Id).ToList
                For index = 0 To hojasLiq.Count - 1
                    idHoja = CInt(hojasLiq(index)(0))
                    myHojaLiq = Nothing
                    For Each oHoj As ELL.HojaGastos.Liquidacion In lHojas
                        If (oHoj.Hojas.Exists(Function(o As ELL.HojaGastos.Liquidacion.Hoja) o.IdHoja = idHoja)) Then
                            myHojaLiq = oHoj
                            Exit For
                        End If
                    Next
                    sImporteHoja = hojasLiq(index)(1)  'Nos quedamos con el importe correspondiente a la hoja                    
                    If (CInt(myHojaLiq.Usuario.Id) <> idUserActual) Then  'Se cambia de trabajador
                        If (idUserActual = 0) Then 'Es el primero                            
                            importe = CDec(sImporteHoja.Replace(".", ","))
                            importeActual = importe
                            importeFormateado = formatearImporte_Banco(importe, 2)
                        Else 'No es el primero                            
                            GenerateRegistroTransferencia(nifActual, codPersoActual, fechaEmision, cif, importeActual, idPlanta, fichero)

                            importeActual = CDec(sImporteHoja.Replace(".", ","))
                            importe += importeActual
                            importeFormateado = formatearImporte_Banco(importeActual, 2)
                            totalOperaciones += 1
                        End If
                        idUserActual = myHojaLiq.Usuario.Id
                        codPersoActual = myHojaLiq.Usuario.CodPersona
                        nifActual = myHojaLiq.Usuario.Dni
                    Else  'Es el mismo
                        importeActual += CDec(sImporteHoja.Replace(".", ","))
                        importe += CDec(sImporteHoja.Replace(".", ","))
                        importeFormateado = formatearImporte_Banco(importeActual, 2)
                    End If
                Next
                'Imprimimos los datos del ultimo                              
                oUser = myHojaLiq.Usuario
                GenerateRegistroTransferencia(oUser.Dni, oUser.CodPersona, fechaEmision, cif, importeActual, idPlanta, fichero)
                totalOperaciones += 1
                '-------------------------------------------------------                
                fichero.AppendLine("</PmtInf>")
                fichero.AppendLine("</CstmrCdtTrfInitn>")
                fichero.AppendLine("</Document>")

                Dim ficheroResul As String = fichero.ToString
                ficheroResul = ficheroResul.Replace("[TOTAL_OPERACIONES]", totalOperaciones).Replace("[TOTAL_IMPORTE]", formatearImporte_Banco(importe, 2)) 'Otra funcion

                Return ficheroResul
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al generar el fichero del banco", ex)
            End Try
        End Function

        ''' <summary>
        ''' Genera el fichero para el banco para los intereses pedido por Igor
        ''' Se hace aqui de momento porque se quiere para ya, pero luego se pensara en otra forma de hacerlo
        ''' </summary>        
        ''' <param name="fechaEmision">Fecha de emision</param>            
        ''' <param name="idPlanta">Id de la planta</param>                
        ''' <returns>Devuelve el texto del fichero</returns>
        ''' <remarks>Se modifica en enero del 2016 para la nueva especificacion SEPA</remarks>
        Public Function GenerarFicheroBancoIntereses(ByVal fechaEmision As Date, ByVal idPlanta As Integer) As String
            Dim index As Integer
            Try
                Dim fichero As New Text.StringBuilder
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim epsilonBLL As New BLL.Epsilon(idPlanta)
                Dim oUser As SabLib.ELL.Usuario = Nothing
                Dim direccionCompleta, cif, nombre, direccion, poblacion, provincia, cuenta, importeFormateado, sImporteInteres, nifActual, bic, msgId As String
                Dim idUserActual, codPersoActual, totalOperaciones As Integer
                Dim importe, importeActual As Decimal
                Dim infoEmpresa As String() = epsilonBLL.getInfoEmpresa()
                Dim cuentaPago As String() = epsilonBLL.getCuentaPago()
                totalOperaciones = 0 : importeActual = 0 : importeFormateado = String.Empty
                idUserActual = 0 : codPersoActual = 0 : nifActual = 0

                nombre = infoEmpresa(0).Trim : cif = infoEmpresa(1).Trim
                direccion = infoEmpresa(2).Trim & " " & infoEmpresa(3).Trim & " " & infoEmpresa(4).Trim
                poblacion = infoEmpresa(5).Trim & " - " & infoEmpresa(6).Trim
                provincia = infoEmpresa(7).Trim
                direccionCompleta = direccion & ",  " & poblacion & " " & provincia
                cuenta = "ES" & cuentaPago(4).Trim & cuentaPago(0).Trim & cuentaPago(1).Trim & cuentaPago(2).Trim & cuentaPago(3).Trim  'ES Iban Banco Sucursal DC Cuenta
                msgId = "Interesak_" & fechaEmision.ToString("yyyyMMdd")
                bic = cuentaPago(5).Trim

                '1-XML HEADER
                '--------------------------------------------------------
                fichero.AppendLine("<?xml version=""1.0"" encoding=""UTF-8""?>")
                fichero.AppendLine("<Document xmlns=""urn:iso:std:iso:20022:tech:xsd:pain.001.001.03"" xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"">")
                fichero.AppendLine("<CstmrCdtTrfInitn>") 'iniciación de adeudos directos.

                '2-GROUP HEADER
                '--------------------------------------------------------
                fichero.AppendLine("<GrpHdr>") ' Conjunto de características compartidas por todas las operaciones incluidas en el mensaje
                fichero.AppendLine("    <MsgId>" & msgId & "</MsgId>") 'Referencia de cliente a entidad financiera que asigna la parte iniciadora o el propio ordenante, para identificar el mensaje de forma inequívoca al enviarlo a su entidad financiera
                fichero.AppendLine("    <CreDtTm>" & Date.Now.ToString("yyyy-MM-dd") & "T" & fechaEmision.ToString("hh:mm:ss") & "</CreDtTm>") 'Fecha y hora cuando la parte iniciadora ha creado la instrucciones de pago.
                fichero.AppendLine("    <NbOfTxs>[TOTAL_OPERACIONES]</NbOfTxs>") 'Número de operaciones individuales que contiene el mensaje
                fichero.AppendLine("    <CtrlSum>[TOTAL_IMPORTE]</CtrlSum>") 'Suma total de todos los importes individuales incluidos en el mensaje, sin tener en cuenta las divisas. 18 digitos y 2 decimales. Separador el .
                fichero.AppendLine("    <InitgPty>") 'Para que inicia el pago
                fichero.AppendLine("        <Nm>" & nombre & "</Nm>") 'Nombre
                fichero.AppendLine("        <Id>") 'Identificacion
                fichero.AppendLine("            <OrgId>") 'Persona juridica
                fichero.AppendLine("                <Othr>") 'Otro
                fichero.AppendLine("                    <Id>" & cif & "</Id>") 'NIF
                fichero.AppendLine("                </Othr>")
                fichero.AppendLine("            </OrgId>")
                fichero.AppendLine("        </Id>")
                fichero.AppendLine("    </InitgPty>")
                fichero.AppendLine("</GrpHdr>")

                '3-PAYMENT INFORMATION
                '--------------------------------------------------------
                fichero.AppendLine("<PmtInf>") 'onjunto de características que se aplican a la parte del acreedor de las operaciones de pago incluidas en el mensaje de iniciación de adeudos directos.
                fichero.AppendLine("    <PmtInfId>" & cif & "</PmtInfId>") 'Referencia asignada por el ordenante para identificar claramente el bloque de información de pago dentro del mensaje
                fichero.AppendLine("    <PmtMtd>TRF</PmtMtd>") 'Metodo de pago
                fichero.AppendLine("    <BtchBookg>true</BtchBookg>") 'True:Se apunta un solo apunte en cuenta con el coste total / False: Se apunta un apunte en cuenta por cada operación individual
                fichero.AppendLine("    <NbOfTxs>[TOTAL_OPERACIONES]</NbOfTxs>") 'Número de operaciones individuales que contiene el mensaje
                fichero.AppendLine("    <CtrlSum>[TOTAL_IMPORTE]</CtrlSum>") 'Suma total de todos los importes individuales incluidos en el mensaje, sin tener en cuenta las divisas. 18 digitos y 2 decimales. Separador el .
                fichero.AppendLine("    <ReqdExctnDt>" & Now.ToString("yyyy-MM-dd") & "</ReqdExctnDt>") 'Fecha de ejecución solicitada
                fichero.AppendLine("    <Dbtr>") 'Ordenante
                fichero.AppendLine("        <Nm>" & nombre & "</Nm>") 'Nombre                
                fichero.AppendLine("        <Id>") 'Identificacion
                fichero.AppendLine("            <OrgId>") 'Persona juridica
                fichero.AppendLine("                <Othr>") 'Otro
                fichero.AppendLine("                    <Id>" & cif & "</Id>") 'CIF
                fichero.AppendLine("                </Othr>")
                fichero.AppendLine("            </OrgId>")
                fichero.AppendLine("        </Id>")
                fichero.AppendLine("    </Dbtr>")
                fichero.AppendLine("    <DbtrAcct>")  'Cuenta del ordenante
                fichero.AppendLine("        <Id>") 'Identificacion
                fichero.AppendLine("            <IBAN>" & cuenta & "</IBAN>") 'IBAN
                fichero.AppendLine("        </Id>")
                fichero.AppendLine("    </DbtrAcct>")
                fichero.AppendLine("    <DbtrAgt>") 'Entidad del ordenante
                fichero.AppendLine("        <FinInstnId>") 'dentificación
                fichero.AppendLine("            <BIC>" & bic & "</BIC>") 'Bank Identifier Code
                fichero.AppendLine("        </FinInstnId>")
                fichero.AppendLine("    </DbtrAgt>")
                fichero.AppendLine("    <ChrgBr>SLEV</ChrgBr>") 'Clausula de gastos: Detalla cuál de las partes intervinientes soportará las comisiones y gastos ligados al procesamiento de la operación

                '3-INDIVIDUAL TRANSFERENCE
                '--------------------------------------------------------
                Dim lIntereses As List(Of String()) = hojaGastosDAL.getTrabajadoresIntereses()
                For index = 0 To lIntereses.Count - 1
                    sImporteInteres = lIntereses(index)(1)
                    oUser = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.CodPersona = lIntereses(index)(0), .IdPlanta = 1}, False)
                    If (CInt(oUser.Id) <> idUserActual) Then  'Se cambia de trabajador
                        If (idUserActual = 0) Then 'Es el primero                            
                            importe = CDec(sImporteInteres.Replace(".", ","))
                            importeActual = importe
                            importeFormateado = formatearImporte_Banco(importe, 2)
                        Else 'No es el primero                            
                            GenerateRegistroTransferenciaIntereses(nifActual, codPersoActual, fechaEmision, cif, importeActual, idPlanta, fichero)
                            importeActual = CDec(sImporteInteres.Replace(".", ","))
                            importe += importeActual
                            importeFormateado = formatearImporte_Banco(importeActual, 2)
                            totalOperaciones += 1
                        End If
                        idUserActual = oUser.Id
                        codPersoActual = oUser.CodPersona
                        nifActual = oUser.Dni
                    Else  'Es el mismo
                        importeActual += CDec(sImporteInteres.Replace(".", ","))
                        importe += CDec(sImporteInteres.Replace(".", ","))
                        importeFormateado = formatearImporte_Banco(importeActual, 2)
                    End If
                Next
                'Imprimimos los datos del ultimo                              
                GenerateRegistroTransferenciaIntereses(oUser.Dni, oUser.CodPersona, fechaEmision, cif, importeActual, idPlanta, fichero)
                totalOperaciones += 1
                '-------------------------------------------------------                
                fichero.AppendLine("</PmtInf>")
                fichero.AppendLine("</CstmrCdtTrfInitn>")
                fichero.AppendLine("</Document>")

                Dim ficheroResul As String = fichero.ToString
                ficheroResul = ficheroResul.Replace("[TOTAL_OPERACIONES]", totalOperaciones).Replace("[TOTAL_IMPORTE]", formatearImporte_Banco(importe, 2)) 'Otra funcion

                Return ficheroResul
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al generar el fichero del banco de los intereses", ex)
            End Try
        End Function

        ''' <summary>
        ''' Genera un registro de transferencia de una persona para los intereses
        ''' Obsoleta desde el 01/02/2016
        ''' </summary>
        ''' <param name="nif">Dni</param>
        ''' <param name="codPersona">Codigo de persona</param>        
        ''' <param name="fechaEmision">Fecha de emision</param>
        ''' <param name="cif">Cif de la empresa</param>
        ''' <param name="importe">Importe</param>
        ''' <param name="idPlanta">Id de la planta</param>
        Private Sub GenerateRegistroTransferenciaIntereses(ByVal nif As String, ByVal codPersona As Integer, ByVal fechaEmision As Date, ByVal cif As String, ByVal importe As Decimal, ByVal idPlanta As Integer, ByRef fichero As Text.StringBuilder)
            Dim concepto, nombreTra, direccionTra, cuenta, swift_bic, iban As String
            Dim epsilonBLL As New BLL.Epsilon(idPlanta)
            Dim sInfoPersona As String() = epsilonBLL.getInfoBancoTrabajador(nif)
            If (sInfoPersona Is Nothing) Then Throw New BidaiakLib.BatzException("Existe un problema al recuperar la informacion de epsilon del trabajador " & codPersona, Nothing)
            nif = sInfoPersona(0).Trim
            nombreTra = sInfoPersona(1).Trim & If(sInfoPersona(2) Is Nothing, "", " " & sInfoPersona(2).Trim) & ", " & sInfoPersona(3).Trim
            nombreTra = nombreTra.Replace("ñ", "n").Replace("Ñ", "N")
            cuenta = "ES" & sInfoPersona(13).Trim & sInfoPersona(9).Trim & sInfoPersona(10).Trim & sInfoPersona(12).Trim & sInfoPersona(11).Trim
            direccionTra = sInfoPersona(4).Trim & " " & sInfoPersona(5).Trim.ToString & " " & sInfoPersona(6) & ", " & sInfoPersona(8)
            direccionTra = direccionTra.Replace("ñ", "n").Replace("Ñ", "N")
            concepto = "Batz Interesak"
            swift_bic = sInfoPersona(14).Trim
            iban = "ES" & sInfoPersona(13).Trim & sInfoPersona(9).Trim & sInfoPersona(10).Trim & sInfoPersona(12).Trim & sInfoPersona(11).Trim

            fichero.AppendLine("<CdtTrfTxInf>") 'Información de transferencia individual
            fichero.AppendLine("    <PmtId>") 'Identificación del pago
            fichero.AppendLine("        <EndToEndId>" & nif & "</EndToEndId>") 'Referencia única que asigna la parte iniciadora para identificar la operación y que se transmite sin cambios a lo largo de la cadena del pago hasta el beneficiario
            fichero.AppendLine("    </PmtId>")
            fichero.AppendLine("    <Amt>") 'Importe
            fichero.AppendLine("        <InstdAmt Ccy=""EUR"">" & formatearImporte_Banco(importe, 2) & "</InstdAmt>") 'Importe de la transferencia.La longitud del importe se limita a 11 dígitos, dos de los cuales son decimales. El separador de los decimales es el carácter “.”            
            fichero.AppendLine("    </Amt>")
            fichero.AppendLine("    <CdtrAgt>") 'Entidad del beneficiario
            fichero.AppendLine("        <FinInstnId>") 'Identificación de la entidad del beneficiario
            fichero.AppendLine("            <BIC>" & swift_bic & "</BIC>") 'BIC de la entidad del beneficiario
            fichero.AppendLine("        </FinInstnId>")
            fichero.AppendLine("    </CdtrAgt>")
            fichero.AppendLine("    <Cdtr>") 'Beneficiario
            fichero.AppendLine("        <Nm>" & nombreTra & "</Nm>") 'Nombre            
            fichero.AppendLine("    </Cdtr>")
            fichero.AppendLine("    <CdtrAcct>") 'Cuenta del beneficiario
            fichero.AppendLine("        <Id>") 'Identificador
            fichero.AppendLine("            <IBAN>" & iban & "</IBAN>") 'IBAN
            fichero.AppendLine("        </Id>")
            fichero.AppendLine("    </CdtrAcct>")
            fichero.AppendLine("    <RmtInf>") 'Concepto
            fichero.AppendLine("        <Ustrd>" & concepto & "</Ustrd>") 'No estructurado
            fichero.AppendLine("    </RmtInf>")
            fichero.AppendLine("</CdtTrfTxInf>")
        End Sub

        ''' <summary>
        ''' Genera un registro de transferencia de una persona
        ''' Obsoleta desde el 01/02/2016
        ''' </summary>
        ''' <param name="nif">Dni</param>
        ''' <param name="codPersona">Codigo de persona</param>        
        ''' <param name="fechaEmision">Fecha de emision</param>
        ''' <param name="cif">Cif de la empresa</param>
        ''' <param name="importe">Importe</param>
        ''' <param name="idPlanta">Id de la planta</param>
        Private Sub GenerateRegistroTransferencia(ByVal nif As String, ByVal codPersona As Integer, ByVal fechaEmision As Date, ByVal cif As String, ByVal importe As Decimal, ByVal idPlanta As Integer, ByRef fichero As Text.StringBuilder)
            Dim concepto, nombreTra, direccionTra, cuenta, swift_bic, iban As String
            Dim epsilonBLL As New BLL.Epsilon(idPlanta)
            Dim sInfoPersona As String() = epsilonBLL.getInfoBancoTrabajador(nif)
            If (sInfoPersona Is Nothing) Then Throw New BidaiakLib.BatzException("Existe un problema al recuperar la informacion de epsilon del trabajador " & codPersona, Nothing)
            nif = sInfoPersona(0).Trim
            nombreTra = sInfoPersona(1).Trim & If(sInfoPersona(2) Is Nothing, "", " " & sInfoPersona(2).Trim) & ", " & sInfoPersona(3).Trim
            nombreTra = nombreTra.Replace("ñ", "n").Replace("Ñ", "N")
            cuenta = "ES" & sInfoPersona(13).Trim & sInfoPersona(9).Trim & sInfoPersona(10).Trim & sInfoPersona(12).Trim & sInfoPersona(11).Trim
            direccionTra = sInfoPersona(4).Trim & " " & sInfoPersona(5).Trim.ToString & " " & sInfoPersona(6) & ", " & sInfoPersona(8)
            direccionTra = direccionTra.Replace("ñ", "n").Replace("Ñ", "N")
            concepto = "BATZ " & fechaEmision.Year & ", bidai-gastuak"
            swift_bic = sInfoPersona(14).Trim
            iban = "ES" & sInfoPersona(13).Trim & sInfoPersona(9).Trim & sInfoPersona(10).Trim & sInfoPersona(12).Trim & sInfoPersona(11).Trim

            fichero.AppendLine("<CdtTrfTxInf>") 'Información de transferencia individual
            fichero.AppendLine("    <PmtId>") 'Identificación del pago
            fichero.AppendLine("        <EndToEndId>" & nif & "</EndToEndId>") 'Referencia única que asigna la parte iniciadora para identificar la operación y que se transmite sin cambios a lo largo de la cadena del pago hasta el beneficiario
            fichero.AppendLine("    </PmtId>")
            fichero.AppendLine("    <Amt>") 'Importe
            fichero.AppendLine("        <InstdAmt Ccy=""EUR"">" & formatearImporte_Banco(importe, 2) & "</InstdAmt>") 'Importe de la transferencia.La longitud del importe se limita a 11 dígitos, dos de los cuales son decimales. El separador de los decimales es el carácter “.”            
            fichero.AppendLine("    </Amt>")
            fichero.AppendLine("    <CdtrAgt>") 'Entidad del beneficiario
            fichero.AppendLine("        <FinInstnId>") 'Identificación de la entidad del beneficiario
            fichero.AppendLine("            <BIC>" & swift_bic & "</BIC>") 'BIC de la entidad del beneficiario
            fichero.AppendLine("        </FinInstnId>")
            fichero.AppendLine("    </CdtrAgt>")
            fichero.AppendLine("    <Cdtr>") 'Beneficiario
            fichero.AppendLine("        <Nm>" & nombreTra & "</Nm>") 'Nombre            
            fichero.AppendLine("    </Cdtr>")
            fichero.AppendLine("    <CdtrAcct>") 'Cuenta del beneficiario
            fichero.AppendLine("        <Id>") 'Identificador
            fichero.AppendLine("            <IBAN>" & iban & "</IBAN>") 'IBAN
            fichero.AppendLine("        </Id>")
            fichero.AppendLine("    </CdtrAcct>")
            fichero.AppendLine("    <RmtInf>") 'Concepto
            fichero.AppendLine("        <Ustrd>" & concepto & "</Ustrd>") 'No estructurado
            fichero.AppendLine("    </RmtInf>")
            fichero.AppendLine("</CdtTrfTxInf>")
        End Sub

        ''' <summary>
        ''' Funcion que formatea el importe para el envio del fichero al banco
        ''' Solo podrán tener dos decimales y separados con punto
        ''' </summary>
        ''' <param name="import">Importe</param>        
        ''' <param name="numDecimals">Numero de decimales maximo</param>
        ''' <returns></returns>        
        Private Function formatearImporte_Banco(ByVal import As Decimal, ByVal numDecimals As Integer) As String
            Dim sImport As String = Math.Round(import, numDecimals)
            sImport = sImport.Replace(",", ".")
            Return sImport
        End Function

        ''' <summary>
        ''' Funcion que formatea el importe
        ''' </summary>
        ''' <param name="import">Importe</param>
        ''' <param name="numDigitos">El numero de digitos que va a tener. Los ultimos dos seran decimal</param>
        ''' <returns></returns>        
        Private Function formatearImporte(ByVal import As Decimal, numDigitos As Integer) As String
            Dim sImporte As String = CStr(import)
            Dim datos As String() = sImporte.Split(", ")
            If datos.Length > 1 Then
                sImporte = datos(0).PadLeft(numDigitos - 2, "0") & datos(1)
                If (datos(1).Length = 1) Then sImporte &= "0"
            Else 'no tiene decimales
                sImporte = sImporte.PadLeft(numDigitos - 2, "0") & "00"  'Se le añaden 00 al final porque serian los decimales
            End If
            Return sImporte
        End Function

        ''' <summary>
        ''' Guarda las nuevas fechas de la hoja
        ''' </summary>
        ''' <param name="id">Id de la hoja</param>         
        ''' <param name="fechaDesde">Fecha desde</param>        
        ''' <param name="fechaHasta">Fecha hasta</param>
        ''' <param name="myConnection">Si es una transaccion</param>
        Public Sub SaveFechas(ByVal id As Integer, ByVal fechaDesde As Date, ByVal fechaHasta As Date, Optional ByVal myConnection As OracleConnection = Nothing)
            hojaGastosDAL.SaveFechas(id, fechaDesde, fechaHasta, myConnection)
        End Sub

        ''' <summary>
        ''' Añade una linea a una hoja de gastos
        ''' Si es una linea de una hoja de gastos sin viaje, habra que comprobar:
        '''   - No tiene ninguna otra de HG libre en estado rellenada
        '''   - No tiene otras HG libres en el rango
        '''   - Comprobar que no esta en el rango de algun viaje
        ''' </summary>
        ''' <param name="oLinea">Info de la linea</param>        
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="IdResponsable">Id del responsable</param>       
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="lineaVacia">Indica si la linea a añadir sera vacia. Si es vacia, solo se insertara la cabecera. Esto suele ocurrir cuando se crea una hoja de gastos que solo tiene gastos de visa pero necesita tener creada la HG</param>        
        ''' <param name="myConnection">Conexion por si viene de una transaccion</param>
        ''' <param name="fDesde">Es un parche para que se puedan crear hg anteriores al 1 de mayo libres</param>
        ''' <param name="fHasta">Es un parche para que se puedan crear hg anteriores al 1 de mayo libres</param>
        Public Function AddLinea(ByVal oLinea As ELL.HojaGastos.Linea, ByVal idViaje As Integer, ByVal idResponsable As Integer, ByVal idPlanta As Integer, Optional ByVal lineaVacia As Boolean = False, Optional ByVal myConnection As OracleConnection = Nothing, Optional ByVal fDesde As Date = Nothing, Optional ByVal fHasta As Date = Nothing) As Integer
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim idHojaResul As Integer
                Dim fechaDesde, fechaHasta, fechaEntregAnticipo As Date
                If (idViaje = Integer.MinValue) Then  'Solo se comprobara cuando no haya viaje y cuando no se haya creado la idhoja   And oLinea.IdHoja = Integer.MinValue             
                    Dim idHoja As Integer = Integer.MinValue
                    If (oLinea.IdHoja <> Integer.MinValue) Then
                        Dim myHoja As ELL.HojaGastos = loadHoja(oLinea.IdHoja, False)
                        fechaDesde = myHoja.FechaDesde
                        fechaHasta = myHoja.FechaHasta
                        idHoja = myHoja.Id
                    Else  'La primera vez se crea la HG desde el 1 hasta final de mes                        
                        If (fDesde <> Date.MinValue) Then
                            fechaDesde = fDesde : fechaHasta = fHasta
                        Else
                            fechaDesde = New Date(oLinea.Fecha.Year, oLinea.Fecha.Month, 1)
                            fechaHasta = New Date(oLinea.Fecha.Year, oLinea.Fecha.Month, Date.DaysInMonth(oLinea.Fecha.Year, oLinea.Fecha.Month))
                        End If
                    End If
                    'Se comprueba si tiene HG libres en estado rellenada o si contiene otra HG libre en el rango
                    Dim lHojas As List(Of ELL.HojaGastos) = loadHojas(idPlanta, oLinea.Usuario.Id, Integer.MinValue, Date.MinValue, Date.MinValue, False, False, True)
                    If (lHojas IsNot Nothing AndAlso lHojas.Count > 0) Then
                        Dim oHoja As ELL.HojaGastos = lHojas.Find(Function(o As ELL.HojaGastos) o.Id <> idHoja AndAlso (o.Estado = ELL.HojaGastos.eEstado.Rellenada Or o.Estado = ELL.HojaGastos.eEstado.NoValidada) AndAlso o.IdSinViaje <> Integer.MinValue)
                        '290412: If (oHoja IsNot Nothing) Then Throw New BidaiakLib.BatzException("No se puede añadir la linea porque porque la hoja H" & oHoja.IdSinViaje & " esta sin enviar. Solo puede tener una hoja de gastos libre activa. Envie la activa y vuelva a intentarlo", New Exception("adv"), True)
                        oHoja = lHojas.Find(Function(o As ELL.HojaGastos) o.Id <> idHoja AndAlso o.IdSinViaje <> Integer.MinValue And o.EstaEnElRango(fechaDesde, fechaHasta))
                        If (oHoja IsNot Nothing) Then
                            If (oLinea.Fecha < oHoja.FechaDesde Or oLinea.Fecha > oHoja.FechaHasta) Then
                                Throw New BidaiakLib.BatzException("No se puede añadir la linea porque la hoja de gastos actual contiene la hoja H" & oHoja.IdSinViaje & " , lo cual no esta permitido", New Exception("adv"), True)
                            Else
                                Throw New BidaiakLib.BatzException("No se puede añadir la linea porque esa fecha pertenece a la hoja H" & oHoja.IdSinViaje & ", lo cual no esta permitido", New Exception("adv"), True)
                            End If
                        End If
                    End If
                    'Se comprueba que esa linea no este en el rango de las fechas del integrante de un viaje
                    Dim viajesBLL As New BLL.ViajesBLL
                    Dim oViaje As New ELL.Viaje With {.FechaIda = oLinea.Fecha, .FechaVuelta = oLinea.Fecha}
                    oViaje.ListaIntegrantes = New List(Of ELL.Viaje.Integrante)
                    oViaje.ListaIntegrantes.Add(New ELL.Viaje.Integrante With {.Usuario = New SabLib.ELL.Usuario With {.Id = oLinea.Usuario.Id}})
                    Dim lViajes As List(Of ELL.Viaje) = viajesBLL.loadList(oViaje, False, False, idPlanta)
                    If (lViajes IsNot Nothing AndAlso lViajes.Count > 0) Then
                        Dim oInt As ELL.Viaje.Integrante
                        For Each oViaj As ELL.Viaje In lViajes
                            oInt = oViaj.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = oLinea.Usuario.Id)
                            If (oInt.FechaIda <= oLinea.Fecha And oLinea.Fecha <= oInt.FechaVuelta) Then
                                Throw New BidaiakLib.BatzException("No se puede añadir la linea porque en esas fecha estaba en el viaje V" & oViaj.IdViaje & " - " & oViaj.Destino, New Exception("adv"), True)
                            End If
                        Next
                    End If
                    fechaEntregAnticipo = oLinea.Fecha
                Else 'Es una hoja de viaje
                    Dim viajesBLL As New BLL.ViajesBLL
                    Dim anticBLL As New BLL.AnticiposBLL
                    Dim myViaje As ELL.Viaje = viajesBLL.loadInfo(idViaje)
                    Dim oInt As ELL.Viaje.Integrante = myViaje.ListaIntegrantes.Find(Function(o As ELL.Viaje.Integrante) o.Usuario.Id = oLinea.Usuario.Id)
                    fechaDesde = oInt.FechaIda
                    fechaHasta = oInt.FechaVuelta
                    If (oLinea.Fecha < fechaDesde Or oLinea.Fecha > fechaHasta) Then Throw New BidaiakLib.BatzException("No puede añadir la linea porque esta fuera de las fechas del viaje", New Exception("adv"), True)
                    fechaEntregAnticipo = anticBLL.loadAnticipoFechaEntrega(idViaje)
                    If (fechaEntregAnticipo = DateTime.MinValue) Then fechaEntregAnticipo = oLinea.Fecha
                End If

                If (oLinea.ImporteEuros = 0 AndAlso Not lineaVacia) Then
                    Dim xbatBLL As New BLL.XbatBLL
                    Dim cambioMoneda As Decimal = 0
                    oLinea.ImporteEuros = xbatBLL.ObtenerRateEuros(oLinea.Moneda.Id, oLinea.Cantidad, fechaEntregAnticipo, cambioMoneda)
                    oLinea.CambioMonedaEUR = cambioMoneda
                End If

                If (myConnection Is Nothing) Then
                    myConn = New OracleConnection(hojaGastosDAL.Conexion)
                    myConn.Open()
                    transact = myConn.BeginTransaction()
                Else
                    myConn = myConnection
                End If

                idHojaResul = hojaGastosDAL.AddLinea(oLinea, idViaje, idResponsable, fechaDesde, fechaHasta, lineaVacia, myConn)
                'Para los viajes, no hara falta actualizar las fechas ya que siempre coincidiran con las fechas del viaje
                If (idHojaResul <> Integer.MinValue And idViaje = Integer.MinValue) Then SaveFechas(oLinea.IdHoja, fechaDesde, fechaHasta, myConn)
                If (myConnection Is Nothing) Then transact.Commit()

                Return idHojaResul
            Catch batzEx As BidaiakLib.BatzException
                If (transact IsNot Nothing AndAlso myConnection Is Nothing) Then transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                If (transact IsNot Nothing AndAlso myConnection Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al añadir la linea", ex)
            Finally
                If (myConnection Is Nothing AndAlso myConn IsNot Nothing AndAlso myConn.State <> ConnectionState.Closed) Then myConn.Close()
            End Try
        End Function

        ''' <summary>
        ''' Comprueba si tiene que actualizar las fechas de la HG
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="fechaDesdeComparar">Fecha desde que se comparara con la existente</param>
        ''' <param name="fechaDesdeHG">Fecha desde actual de la HG</param>
        ''' <param name="fechaHastaComparar">Fecha hasta que se comparara con la existente</param>
        ''' <param name="fechaHastaHG">Fecha hasta actual de la HG</param>
        ''' <param name="myConn">Conexion por si es una transaccion</param>
        Private Sub GuardarFechasHG(ByVal idHoja As Integer, ByVal fechaDesdeComparar As Date, ByVal fechaDesdeHG As Date, ByVal fechaHastaComparar As Date, ByVal fechaHastaHG As Date, ByVal myConn As OracleConnection)
            'Se mira cuales son las fechas a guardar
            Dim fechaDesdeNew, fechaHastaNew As Date
            Dim bSave As Boolean = False
            If (fechaDesdeComparar < fechaDesdeHG) Then
                fechaDesdeNew = fechaDesdeComparar
                bSave = True
            Else
                fechaDesdeNew = fechaDesdeHG
            End If
            If (fechaHastaComparar > fechaHastaHG) Then
                fechaHastaNew = fechaHastaComparar
                bSave = True
            Else
                fechaHastaNew = fechaHastaHG
            End If
            If (bSave) Then SaveFechas(idHoja, fechaDesdeNew, fechaHastaNew, myConn)
        End Sub

        ''' <summary>
        ''' Elimina la hoja de gastos y todas sus lineas
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>        
        Sub Delete(ByVal idHoja As Integer)
            hojaGastosDAL.Delete(idHoja)
        End Sub

        ''' <summary>
        ''' Quita una linea de la hoja de gastos
        ''' </summary>
        ''' <param name="idLinea">Id de la linea</param>  
        Public Sub DeleteLinea(ByVal idLinea As Integer)
            hojaGastosDAL.DeleteLinea(idLinea)
        End Sub

#End Region

#Region "Liquidaciones"

        ''' <summary>
        ''' Carga las liquidaciones de las personas con hojas de gastos validadas sin haber sido gestionadas ya. Se quitan las que esten excluidas
        ''' Si se le pasa unas hojas de gastos, lo hara con ellas
        ''' Si es liquidador y la diferencia entre el anticipo y los gastos es positiva: Se cuenta solo el gasto de kilometraje. Y si la diferencia , se le sumara esta cantidad al kilometraje
        ''' Si es liquidador y la diferencia entre el anticipo y los gastos es negativa: Se cuenta el gasto de kilometraje mas la diferencia
        ''' Si no es liquidador: Se suman los gastos con/sin recibo mas los kilometrajes
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="tipoL">Tipo de la liquidacion</param>
        ''' <param name="idLiq">Id de cabecera de la liquidacion</param>
        ''' <param name="fValidacionLimite">Fecha de validacion superior limite para mostrar las hojas</param>
        ''' <param name="conHGEntregada">Si es true, solo se sacaran las HG entregadas en administracion</param>
        ''' <param name="hubContext">Contexto de signalR</param>
        ''' <returns></returns> 
        Function loadLiquidaciones(ByVal idPlanta As Integer, ByVal tipoL As ELL.HojaGastos.Liquidacion.TipoLiq, Optional ByVal idLiq As Integer = Integer.MinValue, Optional ByVal fValidacionLimite As Date = Nothing, Optional ByVal conHGEntregada As Boolean = False, Optional ByVal hubContext As Object = Nothing) As List(Of ELL.HojaGastos.Liquidacion)
            Try
                Dim lLiquidaciones As New List(Of ELL.HojaGastos.Liquidacion)
                Dim oLiquid As ELL.HojaGastos.Liquidacion
                Dim hojaLiq As ELL.HojaGastos.Liquidacion.Hoja
                Dim anticipo, cantidadLineas, kilometraje As Decimal
                Dim lLineas As List(Of ELL.HojaGastos.Linea)
                Dim epsilonBLL As New BLL.Epsilon(idPlanta)
                Dim bidaiakBLL As New BLL.BidaiakBLL
                Dim bLiquidador As Boolean
                'Dim fechaIndice As Date
                Dim anticBLL As New BLL.AnticiposBLL
                Dim lMov, lMovAll As List(Of ELL.Anticipo.Movimiento)
                'Dim w As New Stopwatch
                If (hubContext IsNot Nothing) Then hubContext.showMessage("Obteniendo hojas de gastos a procesar")
                Dim sHojasExcesoDev As String = String.Empty  'Id de las hojas que habiendo tenido que salir a pagar, se devolvio mas dinero del que se pedia y cambio a devolucion                                
                Dim lConveniosPlanta As List(Of ELL.ConvenioCategoria) = bidaiakBLL.getConveniosCategorias(idPlanta)
                Dim oConvCat As ELL.ConvenioCategoria
                'w.Start()
                Dim fDesdeHGObtener As DateTime = If(tipoL = ELL.HojaGastos.Liquidacion.TipoLiq.Metalico, DateTime.MinValue, New DateTime(2015, 7, 7))  'Las hojas de tipo factura o comision, se obtendran a partir de julio del 2015               
                Dim lHojas As List(Of Object) = hojaGastosDAL.loadHojasGastoALiquidar(idPlanta, idLiq, fValidacionLimite, fDesdeHGObtener)
                'w.Stop()
                'log.Info("LOAD HOJAS-" & w.ElapsedMilliseconds)                
                If (lHojas IsNot Nothing AndAlso lHojas.Count > 0) Then
                    If (conHGEntregada) Then lHojas = lHojas.FindAll(Function(o) o.EntregadaHG = True)
                    log.Info("LoadLiquidaciones: Se van a procesar " & lHojas.Count & " hojas")
                    Dim ratioActualizacionProgress As Integer = (lHojas.Count / 50) + 1  '50 es el numero de actualizaciones que tendra la progress bar, sea el numero de items que sea
                    Dim index As Integer = 1
                    For Each hoja In lHojas
                        If (hubContext IsNot Nothing) Then
                            hubContext.showMessage("Procesando la hoja " & index & " de " & lHojas.Count)
                            If (index Mod ratioActualizacionProgress = 0) Then hubContext.showProgress(index + 1, lHojas.Count) 'Para que la actualizacion de la progress bar sea mas fina
                        End If
                        'w.Reset():w.Start()
                        index += 1
                        bLiquidador = False : cantidadLineas = 0 : anticipo = 0 : kilometraje = 0
                        'If (CInt(sHoja(6)) = 3077 Or CInt(sHoja(6)) = 910) Then
                        '    '1º Se comprueba que esa persona tenga indice. Si no tiene, no se tratara por aqui
                        '    fechaIndice = CDate(sHoja(14))
                        '    If (Not epsilonBLL.TieneIndiceBatz(sHoja(8), fechaIndice.Year, fechaIndice.Month, 1)) Then
                        '        'log.Warn("La hoja de gastos '" & sHoja(0) & "' del usuario " & sHoja(4) & " (" & sHoja(6) & ") no se muestra en las liquidaciones porque en " & fechaIndice.ToShortDateString & " no tenia indice de Batz")
                        '        Continue For
                        '    End If
                        'End If
                        '1º Se comprueba que esa persona tenga facturacion en metalico/factura. Depende del parametro. Si no tiene, no se tratara por aqui
                        If (hoja.Dni <> String.Empty) Then
                            Dim sInfo As String() = CType(System.Web.HttpContext.Current.Cache.Get("perso_" & hoja.Dni), String())
                            If (sInfo Is Nothing) Then
                                sInfo = epsilonBLL.GetInfoPersona(hoja.Dni)
                                If (sInfo IsNot Nothing) Then System.Web.HttpContext.Current.Cache.Insert("perso_" & hoja.Dni, sInfo)
                            End If
                            If (sInfo IsNot Nothing AndAlso sInfo(4) <> String.Empty AndAlso sInfo(5) <> String.Empty) Then
                                oConvCat = lConveniosPlanta.Find(Function(o As ELL.ConvenioCategoria) o.IdConvenio = sInfo(4) And o.IdCategoria = sInfo(5))
                                If Not (oConvCat IsNot Nothing AndAlso oConvCat.TipoLiquidacion = tipoL) Then
                                    'w.Stop()
                                    'log.Info("HOJA(" & hoja.IdHoja & ") no se ejecuta:" & w.ElapsedMilliseconds)
                                    Continue For
                                End If
                            Else
                                log.Info("No se ha podido obtener la informacion de Epsilon de la persona " & hoja.IdUserHG & " por tanto no se va a mostrar en el listado")
                                Continue For
                            End If
                        Else
                            'w.Stop()
                            'log.Info("HOJA(" & hoja.IdHoja & ") no se ejecuta: " & w.ElapsedMilliseconds)
                            Continue For  'Los que no tienen DNI, no se les va a pagar nada
                        End If
                        oLiquid = New ELL.HojaGastos.Liquidacion
                        oLiquid.Usuario = New SabLib.ELL.Usuario With {.Id = hoja.IdUserHG, .CodPersona = hoja.NumTrab, .Nombre = hoja.NombrePersona.ToUpper, .Dni = hoja.Dni, .IdDepartamento = hoja.IdDepto, .FechaBaja = hoja.FechaBaja, .IdEmpresa = hoja.IdEmpresa}
                        oLiquid.CuentaContable = hoja.CuentaDepto
                        If (idLiq = Integer.MinValue) Then 'Mostrar las que no esten liquidadas
                            lMovAll = Nothing
                            'Si tiene viaje y el responsable de liquidacion es el usuario de la hoja de gastos                       
                            If (hoja.IdViaje > 0 AndAlso hoja.TieneAnticipo AndAlso hoja.IdLiquidador = hoja.IdUserHG) Then
                                If (hoja.EstadoAnticipo = ELL.Anticipo.EstadoAnticipo.Entregado Or hoja.EstadoAnticipo = ELL.Anticipo.EstadoAnticipo.cerrado) Then 'Solo se toman en cuenta los anticipos ya entregados o cerrados                                
                                    lMovAll = anticBLL.loadMovimientos(hoja.IdViaje, Integer.MinValue, False) 'Se obtienen todos
                                    lMov = lMovAll.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Entregado)
                                    If (lMov.Count > 0) Then
                                        For Each mov As ELL.Anticipo.Movimiento In lMov
                                            anticipo += mov.ImporteEuros
                                        Next
                                        bLiquidador = True
                                    Else 'Significara que se le ha hecho una transferencia ya que tiene un anticipo sin importes
                                        bLiquidador = True
                                    End If
                                End If
                            End If
                            'Se comprueba si tiene alguna transferencia el viaje
                            If (bLiquidador AndAlso hoja.IdViaje > 0) Then
                                Dim viajesBLL As New BLL.ViajesBLL
                                Dim oAntic As ELL.Anticipo = anticBLL.loadInfo(hoja.IdViaje, True, False)
                                If (oAntic IsNot Nothing) Then
                                    Dim euros As Decimal = oAntic.EurosPendientesHojaGastosLiq
                                    Dim lMovTrans As List(Of ELL.Anticipo.Movimiento) = oAntic.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Transferencia)
                                    If (lMovTrans.Count > 0) Then
                                        For Each oMov As ELL.Anticipo.Movimiento In lMovTrans
                                            If (oMov.IdViajeOrigen = hoja.IdViaje) Then
                                                anticipo -= oMov.ImporteEuros
                                            Else
                                                anticipo += oMov.ImporteEuros
                                            End If
                                        Next
                                    End If
                                    'Movimientos manuales                            
                                    lMovTrans = oAntic.Movimientos.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Manual)
                                    If (lMovTrans.Count > 0) Then
                                        For Each mov As ELL.Anticipo.Movimiento In lMovTrans
                                            cantidadLineas += (mov.ImporteEuros * -1)
                                        Next
                                    End If
                                End If
                            End If
                            lLineas = FillMovimientos(hoja.IdHoja, False)
                            'Se suman los importes de las lineas de las hojas de gastos
                            For Each oLinea As ELL.HojaGastos.Linea In lLineas
                                If (oLinea.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Kilometraje) Then
                                    kilometraje += oLinea.ImporteEuros
                                Else
                                    cantidadLineas += oLinea.ImporteEuros
                                End If
                            Next
                            If (bLiquidador OrElse anticipo <> 0) Then   'Sino es liquidador, se suman los gastos con/sin recibo mas el kilometraje                            
                                If (anticipo < cantidadLineas) Then  'es liquidador y se ha gastado mas que el anticipo, asi que se le suma el kilometraje a la diferencia del anticipo y lo gastado
                                    '25/02/15: Si se ha gastado mas, se comprueba si ha devuelto dinero
                                    If (lMovAll IsNot Nothing) Then
                                        lMov = lMovAll.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion)
                                        Dim cantidadLineasConDevolucion As Decimal = 0
                                        If (lMov.Count > 0) Then
                                            For Each mov As ELL.Anticipo.Movimiento In lMov
                                                If mov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion Then
                                                    cantidadLineasConDevolucion += mov.ImporteEuros
                                                End If
                                            Next
                                            cantidadLineasConDevolucion = anticipo - (cantidadLineas + cantidadLineasConDevolucion)
                                            If (cantidadLineasConDevolucion >= 0) Then 'Si todavia tiene que devolver dinero, se marcara solo el kilometraje
                                                cantidadLineas = kilometraje
                                            Else  'hay que devolverle la diferencia + el kilometraje
                                                cantidadLineas = Math.Abs(cantidadLineasConDevolucion) + kilometraje
                                            End If
                                        Else 'No tiene movimientos de devolucion
                                            cantidadLineas = kilometraje + (cantidadLineas - anticipo)
                                        End If
                                    Else   'No tiene movimientos
                                        cantidadLineas = kilometraje + (cantidadLineas - anticipo)
                                    End If
                                Else
                                    '19/11/13: Si ha gastado menos que el anticipo, debera devolver y solo se le pagara el km a no ser que haya devuelto mas de lo que debía (por ejemplo porque lo tiene todo en una moneda extranjera y no quiere quedarsela)
                                    If (lMovAll IsNot Nothing) Then
                                        lMov = lMovAll.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Diferencia_Cambio)
                                        If (lMov.Count > 0) Then  'Si hay diferencia de cambio, no habra que chequear nada ya que esta todo cuadrado                                            
                                            cantidadLineas = kilometraje
                                        Else
                                            lMov = lMovAll.FindAll(Function(o As ELL.Anticipo.Movimiento) o.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion)
                                            Dim cantidadLineasConDevolucion As Decimal = 0
                                            If (lMov.Count > 0) Then
                                                For Each mov As ELL.Anticipo.Movimiento In lMov
                                                    If mov.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Devolucion Then
                                                        cantidadLineasConDevolucion += mov.ImporteEuros
                                                    End If
                                                Next
                                                cantidadLineasConDevolucion = anticipo - (cantidadLineas + cantidadLineasConDevolucion)
                                            End If
                                            If (cantidadLineasConDevolucion < 0) Then 'Si es menor que 0 significa que ha devuelto más de lo que debia y hay que devolverle
                                                cantidadLineas = Math.Abs(cantidadLineasConDevolucion) + kilometraje
                                                sHojasExcesoDev &= If(sHojasExcesoDev <> String.Empty, ",", "") & hoja.IdHoja
                                            Else
                                                cantidadLineas = kilometraje
                                            End If
                                        End If
                                    Else
                                        cantidadLineas = kilometraje
                                    End If
                                End If
                            Else
                                cantidadLineas += kilometraje
                            End If
                            oLiquid.ImporteTotalEuros += Math.Round(cantidadLineas, 2)  'Importe
                            If (cantidadLineas > 0) Then  'Omitimos las hojas con 0€
                                hojaLiq = New ELL.HojaGastos.Liquidacion.Hoja With {.Estado = hoja.EstadoHG, .Entregada = hoja.EntregadaHG, .IdHoja = hoja.IdHoja,
                                    .IdViaje = hoja.IdViaje, .IdHojaLibre = hoja.NumHojaSinViaje, .ImporteEuros = Math.Round(cantidadLineas, 2), .FechaValidacion = hoja.FechaValidacionHG}
                                oLiquid.Hojas = New List(Of ELL.HojaGastos.Liquidacion.Hoja)
                                oLiquid.Hojas.Add(hojaLiq)
                                lLiquidaciones.Add(oLiquid)
                            End If
                        Else  'Las que ya se han emitido
                            hojaLiq = New ELL.HojaGastos.Liquidacion.Hoja With {.IdHoja = hoja.IdHoja, .FechaValidacion = hoja.FechaValidacionHG, .ImporteEuros = Math.Round(hoja.ImporteHG, 2),
                                .Estado = hoja.EstadoHG, .Entregada = hoja.EntregadaHG, .IdViaje = hoja.IdViaje, .IdHojaLibre = hoja.NumHojaSinViaje}
                            oLiquid.Hojas = New List(Of ELL.HojaGastos.Liquidacion.Hoja)
                            oLiquid.Hojas.Add(hojaLiq)
                            lLiquidaciones.Add(oLiquid)
                        End If
                        'w.Stop()
                        'log.Info("HOJA(" & hoja.IdHoja & ")" & w.ElapsedMilliseconds)
                    Next
                End If
                If (lLiquidaciones.Count = 0) Then lLiquidaciones = Nothing
                If (hubContext IsNot Nothing) Then hubContext.showMessage("Finalizando filtrado")
                If (sHojasExcesoDev <> String.Empty) Then log.Info("Las siguientes hojas habian salido a pagar a Batz pero al devolver mas de lo pedido, se le tienen que devolver al trabajador:" & sHojasExcesoDev)
                Return lLiquidaciones
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al calcular las liquidaciones", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga las cabeceras de las liquidaciones ya emitidas
        ''' </summary>
        ''' <param name="idCab">Id de la cabecera</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <returns></returns>        
        Function loadCabeceraLiquidacionEmitida(ByVal idCab As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq) As ELL.HojaGastos.Liquidacion.Cabecera
            Return hojaGastosDAL.loadCabeceraLiquidacionEmitida(idCab, tipoLiq)
        End Function

        ''' <summary>
        ''' Carga las cabeceras de las liquidaciones ya emitidas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <param name="oFiltro">Filtro para la busqueda</param>
        ''' <returns></returns>        
        Function loadCabecerasLiquidacionesEmitidas(ByVal idPlanta As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq, Optional ByVal oFiltro As ELL.HojaGastos.Liquidacion.Cabecera = Nothing) As List(Of ELL.HojaGastos.Liquidacion.Cabecera)
            Return hojaGastosDAL.loadCabecerasLiquidacionesEmitidas(idPlanta, tipoLiq, oFiltro)
        End Function

        ''' <summary>
        ''' Carga las hojas de gastos de la liquidacion
        ''' </summary>
        ''' <param name="idCabecera">Id de la cabecera de la liquidacion</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <returns></returns>        
        Function loadHojasLiquidacion(ByVal idCabecera As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq) As List(Of ELL.HojaGastos.Liquidacion)
            Dim lHojas As List(Of String()) = hojaGastosDAL.loadHojasLiquidacion(idCabecera, tipoLiq)
            Dim lHojasLiq As New List(Of ELL.HojaGastos.Liquidacion)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oLiq As ELL.HojaGastos.Liquidacion
            Dim oHoja As ELL.HojaGastos
            Dim oHojaLiq As ELL.HojaGastos.Liquidacion.Hoja
            If (lHojas IsNot Nothing) Then
                For Each sHoja As String() In lHojas
                    oLiq = New ELL.HojaGastos.Liquidacion With {.ImporteTotalEuros = Math.Round(BidaiakBLL.DecimalValue(sHoja(1)), 2)}
                    oLiq.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(sHoja(2))}, False)
                    oLiq.CuentaContable = SabLib.BLL.Utils.integerNull(sHoja(3))

                    oHojaLiq = New ELL.HojaGastos.Liquidacion.Hoja With {.IdHoja = CInt(sHoja(0))}
                    oHoja = loadHoja(oHojaLiq.IdHoja, False)
                    oHoja.Estados = loadStates(oHojaLiq.IdHoja)
                    If (oHoja.IdViaje > 0) Then oHojaLiq.IdViaje = oHoja.IdViaje
                    If (oHoja.IdSinViaje > 0) Then oHojaLiq.IdHojaLibre = oHoja.IdSinViaje
                    oHojaLiq.ImporteEuros = oLiq.ImporteTotalEuros
                    oHojaLiq.FechaValidacion = CDate(oHoja.Estados.Find(Function(o As String()) CInt(o(0)) = ELL.HojaGastos.eEstado.Validada)(1))
                    If (tipoLiq <> ELL.HojaGastos.Liquidacion.TipoLiq.Metalico) Then oHojaLiq.NumFactura = sHoja(4)
                    oLiq.Hojas = New List(Of ELL.HojaGastos.Liquidacion.Hoja)
                    oLiq.Hojas.Add(oHojaLiq)
                    lHojasLiq.Add(oLiq)
                Next
            End If
            Return lHojasLiq
        End Function

        ''' <summary>
        ''' Obtiene la informacion de la hoja de gastos liquidada
        ''' </summary>
        ''' <param name="idHoja">Id de la hoja</param>
        ''' <param name="tipoLiq">Tipo de liquidacion</param>
        ''' <returns></returns>
        Function loadHojaLiquidacion(ByVal idHoja As Integer, ByVal tipoLiq As ELL.HojaGastos.Liquidacion.TipoLiq)
            Return hojaGastosDAL.loadHojaLiquidacion(idHoja, tipoLiq)
        End Function

        ''' <summary>
        ''' Inserta la cabecera de liquidacion de liquidacion y sus hojas asociadas
        ''' </summary>        
        ''' <param name="fechaEmision">Fecha de emision</param>        
        ''' <param name="fileBanco">Fichero del banco</param>
        ''' <param name="idPlanta">Id planta</param>
        ''' <param name="hojasImportes">Las hojas junto con sus importes</param>
        ''' <param name="myConn">Conexion por si viene de una transaccion</param>        
        ''' <returns>Id insertado</returns>
        Private Function InsertLiquidacion(ByVal fechaEmision As Date, ByVal fileBanco As Byte(), ByVal idPlanta As Integer, ByVal hojasImportes As List(Of String()), Optional ByVal myConn As OracleConnection = Nothing) As Integer
            Return hojaGastosDAL.InsertLiquidacion(fechaEmision, fileBanco, idPlanta, hojasImportes, myConn)
        End Function

        ''' <summary>
        ''' Inserta la cabecera de liquidacion de facturas y sus hojas asociadas
        ''' </summary>        
        ''' <param name="oLiqCab">Cabecera de la liquidacion de facturas</param>        
        ''' <param name="hojasImportes">Las hojas junto con sus importes</param>
        ''' <param name="myConn">Conexion por si viene de una transaccion</param>        
        ''' <returns>Id insertado</returns>
        Private Function InsertTransferencia(ByVal oLiqCab As ELL.HojaGastos.Liquidacion.Cabecera, ByVal hojasImportes As List(Of String()), Optional ByVal myConn As OracleConnection = Nothing) As Integer
            Return hojaGastosDAL.InsertTransferencia(oLiqCab, hojasImportes, myConn)
        End Function

#End Region

    End Class

End Namespace