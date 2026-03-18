Namespace BLL

    Public Class ViajesBLL

        Private viajesDAL As New DAL.ViajesDAL
        Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Bidaiak")

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un viaje
        ''' </summary>
        ''' <param name="id">Id del viaje</param>
        ''' <param name="bGetHojasGasto">Indica si debe obtener las hojas de gastos</param>
        ''' <param name="bCalcularPendienteHojas">Calcula los euros pendientes, tomando en cuenta las hojas de gastos del liquidador</param>
        ''' <param name="bSoloCabecera">Indica si solo se obtendra la cabecera, sin la info de anticipos, sol agencia, etc..</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer, Optional ByVal bGetHojasGasto As Boolean = False, Optional ByVal bCalcularPendienteHojas As Boolean = False, Optional ByVal bSoloCabecera As Boolean = False) As ELL.Viaje
            Dim oViaje As ELL.Viaje = viajesDAL.loadInfo(id)
            FillObject(oViaje, Not bSoloCabecera, bGetHojasGasto, bCalcularPendienteHojas, bSoloCabecera)   'Obtiene los integrantes, las solicitudes de agencia y los anticipos
            Return oViaje
        End Function

        ''' <summary>
        ''' Dadas unas condiciones, obtiene un listado de objetos viaje
        ''' Obtendra los que hayan sido planificados por ellos, sean integrantes o algun subordinado sea integrante de un viaje
        ''' </summary>
        ''' <param name="oViaje">Objeto con las condiciones</param>
        ''' <param name="bActivos">Un viaje estara activo mientras todas sus hojas de gastos, no esten integradas y la fecha de fin no hay llegado</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bHojas">Parametro opcional para indicar si se quieren las hojas de gastos</param>        
        ''' <param name="bSoloCabeceras">Indica si obtiene solo las cabeceras de los objetos o toda su informacion</param>        
        ''' <param name="bFilterState">Indica si se filtra tb por estado</param>
        ''' <returns></returns>        
        Public Function loadList(ByVal oViaje As ELL.Viaje, ByVal bActivos As Boolean, ByVal idPlanta As Integer, Optional ByVal bHojas As Boolean = False, Optional ByVal bSoloCabeceras As Boolean = False, Optional ByVal bFilterState As Boolean = True) As List(Of ELL.Viaje)
            Dim lViajes As List(Of ELL.Viaje) = viajesDAL.loadList(oViaje, bActivos, idPlanta, bFilterState)
            If (lViajes IsNot Nothing) Then
                For Each viaje As ELL.Viaje In lViajes
                    FillObject(viaje, True, bHojas, bSoloCabeceras)
                Next
            End If
            Return lViajes
        End Function

        ''' <summary>
        ''' Dadas unas condiciones, obtiene un listado de objetos viaje. Busca por Organizador o integrantes, entre fechas, idViaje
        ''' </summary>
        ''' <param name="oViaje">Objeto con las condiciones</param>
        ''' <param name="bActivos">Un viaje estara activo mientras todas sus hojas de gastos, no esten integradas y la fecha de fin no hay llegado</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bHojas">Parametro opcional para indicar si se quieren las hojas de gastos</param>        
        ''' <param name="bSoloCabeceras">Indica si obtiene solo las cabeceras de los objetos o toda su informacion</param>        
        ''' <returns></returns>        
        Public Function BuscarViajes(ByVal oViaje As ELL.Viaje, ByVal bActivos As Boolean, ByVal idPlanta As Integer, Optional ByVal bHojas As Boolean = False, Optional ByVal bSoloCabeceras As Boolean = False) As List(Of ELL.Viaje)
            Dim lViajes As List(Of ELL.Viaje) = viajesDAL.BuscarViajes(oViaje, bActivos, idPlanta)
            If (lViajes IsNot Nothing) Then
                For Each viaje As ELL.Viaje In lViajes
                    FillObject(viaje, True, bHojas, bSoloCabeceras)
                Next
            End If
            Return lViajes
        End Function

        ''' <summary>
        ''' Dadas unas condiciones, obtiene un listado de objetos viaje adecuado para reducir tiempo al mostrar el listado
        ''' Obtendra los que hayan sido planificados por ellos, sean integrantes o algun subordinado sea integrante de un viaje
        ''' </summary>
        ''' <param name="oViaje">Objeto con las condiciones</param>
        ''' <param name="bActivos">Un viaje estara activo mientras todas sus hojas de gastos, no esten integradas y la fecha de fin no hay llegado</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bHojas">Parametro opcional para indicar si se quieren las hojas de gastos</param>        
        ''' <returns></returns>        
        Public Function loadListViajes(ByVal oViaje As ELL.Viaje, ByVal bActivos As Boolean, ByVal idPlanta As Integer, Optional ByVal bHojas As Boolean = False, Optional ByVal bSoloCabeceras As Boolean = False) As List(Of ELL.Viaje)
            Dim lViajes As List(Of ELL.Viaje) = viajesDAL.loadList2(oViaje, bActivos, idPlanta)
            Dim hojasBLL As New BLL.HojasGastosBLL
            For index As Integer = lViajes.Count - 1 To 0 Step -1
                lViajes.Item(index).ListaIntegrantes = loadIntegrantes(lViajes.Item(index).IdViaje)
                If (bHojas) Then lViajes.Item(index).HojasGastos = hojasBLL.loadHojas(idPlanta, Integer.MinValue, lViajes.Item(index).IdViaje, Date.MinValue, Date.MinValue, bActivos, False)
            Next
            Return lViajes
        End Function

        ''' <summary>
        ''' Obtiene los viajes con solicitud de agencia
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <param name="estados">Parametro opcional para especificar el o los estados de los viajes</param>
        ''' <param name="año">Parametro opcional para especificar el año de los viajes</param>
        ''' <param name="mes">Parametro opcional para especificar el mes de los viajes</param>
        ''' <param name="idUser">Parametro opcional para especificar el usuario de los viajes</param>
        ''' <param name="idViaje">Parametro opcional para especificar el Id del viaje</param>
        ''' <returns></returns>        
        Public Function loadListWithAgency(ByVal idPlanta As Integer, Optional ByVal estados As List(Of Integer) = Nothing, Optional ByVal año As Integer = Integer.MinValue, Optional ByVal mes As Integer = Integer.MinValue, Optional ByVal idUser As Integer = Integer.MinValue, Optional ByVal idViaje As Integer = Integer.MinValue) As List(Of String())
            Return viajesDAL.loadListWithAgency(idPlanta, estados, año, mes, idUser, idViaje)
        End Function

        ''' <summary>
        ''' Obtiene los viajes con anticipo
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <param name="estados">Parametro opcional para especificar el o los estados de los viajes</param>
        ''' <param name="año">Parametro opcional para especificar el año de los viajes</param>
        ''' <param name="mes">Parametro opcional para especificar el mes de los viajes</param>
        ''' <param name="idUser">Parametro opcional para especificar el usuario de los viajes</param>
        ''' <param name="idViaje">Parametro opcional para especificar el Id del viaje</param>
        ''' <returns></returns>        
        Public Function loadListWithAnticipo(ByVal idPlanta As Integer, Optional ByVal estados As List(Of Integer) = Nothing, Optional ByVal año As Integer = Integer.MinValue, Optional ByVal mes As Integer = Integer.MinValue, Optional ByVal idUser As Integer = Integer.MinValue, Optional ByVal idViaje As Integer = Integer.MinValue) As List(Of Object)
            Dim lAnticipos As List(Of Object) = viajesDAL.loadListWithAnticipo(idPlanta, estados, año, mes, idUser, idViaje)
            Dim anticBLL As New BLL.AnticiposBLL
            Dim lMovs As List(Of ELL.Anticipo.Movimiento)
            Dim antSol As String
            For Each ant In lAnticipos
                lMovs = anticBLL.loadMovimientos(ant.IdViaje, ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado, True)
                antSol = String.Empty
                If (lMovs IsNot Nothing AndAlso lMovs.Count > 0) Then
                    For Each mov As ELL.Anticipo.Movimiento In lMovs
                        antSol &= If(antSol <> String.Empty, ",", "") & mov.Cantidad & " " & mov.Moneda.Abreviatura
                    Next
                End If
                ant.AnticipoSolicitado = antSol
            Next
            Return lAnticipos
        End Function

        ''' <summary>
        ''' Obtiene los integrantes de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="bLoadOrganizador">Indica si se cargara tambien el organizador</param>
        ''' <returns></returns>        
        Public Function loadIntegrantes(ByVal idViaje As Integer, Optional ByVal bLoadOrganizador As Boolean = False) As List(Of ELL.Viaje.Integrante)
            Dim lIntegrantes As List(Of ELL.Viaje.Integrante) = viajesDAL.loadIntegrantes(idViaje, bLoadOrganizador)

            For index = lIntegrantes.Count - 1 To 0 Step -1  'Se quitan aquellos cuyo usuario no se haya podido recuperar
                If (lIntegrantes.Item(index).Usuario Is Nothing) Then lIntegrantes.RemoveAt(index)
            Next
            Return lIntegrantes
        End Function

        ''' <summary>
        ''' Indica si un usuario es integrante de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="idUser">Id del usuario</param>
        ''' <returns></returns>        
        Public Function esIntegranteViaje(ByVal idViaje As Integer, ByVal idUser As Integer) As Boolean
            Return viajesDAL.esIntegranteViaje(idViaje, idUser)
        End Function

        ''' <summary>
        ''' Indica si un usuario es integrante de algun viaje en la fecha de servicio dada
        ''' </summary>        
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="fServicio">Fecha en la que se realizo el servicio</param>
        ''' <returns>Devuelve el Id viaje del que es integrante</returns>        
        Public Function esIntegranteViaje(ByVal idUser As Integer, ByVal fServicio As Date) As Integer
            Return viajesDAL.esIntegranteViaje(idUser, fServicio)
        End Function

        ''' <summary>
        ''' Obtiene los proyectos asociados a un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Public Function loadProyectos(ByVal idViaje As Integer) As List(Of ELL.Viaje.Proyecto)
            Return viajesDAL.loadProyectos(idViaje)
        End Function

        ''' <summary>
        ''' Obtiene informacion del viaje requerida para calcular la liquidacion
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <returns></returns>        
        Public Function loadInfoViajeLiquidacion(ByVal idViaje As Integer) As ELL.Viaje
            Dim anticBLL As New BLL.AnticiposBLL
            Dim oViaje As ELL.Viaje = viajesDAL.loadInfo(idViaje)

            Dim lMov As List(Of ELL.Anticipo.Movimiento) = anticBLL.loadMovimientos(idViaje)

            Return oViaje
        End Function

        ''' <summary>
        ''' Rellena un objeto viaje con la informacion de los anticipos, agencias y hojas de gasto
        ''' </summary>
        ''' <param name="oViaje">Objeto viaje informado donde se dejaran los datos</param>
        ''' <param name="bInfoViajes">Indica si se debe obtener la informacion extra del viaje (anticipos y solicitud de agencia)</param>
        ''' <param name="bGetHojasGasto">Indica si debe obtener las hojas de gastos</param>        
        ''' <param name="bCalcularPendienteHojas">Calcula los euros pendientes, tomando en cuenta las hojas de gastos del liquidador</param>
        Private Sub FillObject(ByRef oViaje As ELL.Viaje, ByVal bInfoViajes As Boolean, ByVal bGetHojasGasto As Boolean, Optional ByVal bCalcularPendienteHojas As Boolean = False, Optional ByVal bSoloCabeceras As Boolean = False)
            If (oViaje IsNot Nothing) Then
                oViaje.ListaIntegrantes = loadIntegrantes(oViaje.IdViaje)
                oViaje.Proyectos = loadProyectos(oViaje.IdViaje)
                If (bInfoViajes) Then
                    Dim anticBLL As New BLL.AnticiposBLL
                    Dim solAgenBLL As New BLL.SolicAgenciasBLL
                    Dim unidadBLL As New BLL.UnidadOrgBLL
                    oViaje.SolicitudAgencia = solAgenBLL.loadInfo(oViaje.IdViaje, False, bSoloCabeceras)
                    oViaje.Anticipo = anticBLL.loadInfo(oViaje.IdViaje, False, bSoloCabeceras)
                    oViaje.UnidadOrganizativa = unidadBLL.load(oViaje.UnidadOrganizativa.Id)
                    If (oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Plantas_Filiales) Then
                        oViaje.SolicitudesPlantasFilial = loadSolPlantasFiliales(oViaje.IdViaje)
                    ElseIf (oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Cliente Or oViaje.TipoDesplazamiento = ELL.Viaje.TipoDesplaz.Otros) Then   '05/06/13: Se incluye lo de otros
                        Dim xbatBLL As New XbatBLL
                        oViaje.DocumentosCliente = loadDocumentosCliente(oViaje.IdViaje)
                        If (oViaje.Proyectos IsNot Nothing AndAlso oViaje.Proyectos.Count > 0) Then
                            Dim lDocsProy As List(Of ELL.DocumentoProyecto)
                            oViaje.DocumentosProyecto = New List(Of ELL.DocumentoProyecto)
                            For Each oProy As ELL.Viaje.Proyecto In oViaje.Proyectos
                                If (oProy.IdPrograma > 0) Then
                                    lDocsProy = xbatBLL.GetDocumentosProyecto(oProy.IdPrograma)
                                    If (lDocsProy IsNot Nothing AndAlso lDocsProy.Count > 0) Then oViaje.DocumentosProyecto.AddRange(lDocsProy)
                                End If
                            Next
                            If (oViaje.DocumentosProyecto.Count = 0) Then oViaje.DocumentosProyecto = Nothing
                        End If
                    End If
                End If

                If (bGetHojasGasto) Then
                    Dim hojasGastoBLL As New BLL.HojasGastosBLL
                    oViaje.HojasGastos = hojasGastoBLL.loadHojas(oViaje.IdPlanta, Integer.MinValue, oViaje.IdViaje)
                    'Si tiene anticipos, calculamos los euros pendientes tomando en cuenta la hoja de gastos del liquidador
                    If (bCalcularPendienteHojas AndAlso oViaje.Anticipo IsNot Nothing) Then
                        Dim total As Decimal = 0
                        If (oViaje.HojasGastos IsNot Nothing) Then
                            Dim lHojas As List(Of ELL.HojaGastos)
                            Dim idResponsable As Integer = oViaje.ResponsableLiquidacion.Id
                            'Nos quedamos solo con las validadas                            
                            lHojas = oViaje.HojasGastos.FindAll(Function(o As ELL.HojaGastos) o.Usuario.Id = idResponsable) '12/11/2012 =>Lo comentamos para que se puedan saber los euros pendientes en las hojas rellenadas tambien
                            'lHojas = oViaje.HojasGastos.FindAll(Function(o As ELL.HojaGastos) (o.Estado = ELL.HojaGastos.eEstado.Validada Or o.Estado = ELL.HojaGastos.eEstado.Liquidada) And o.Usuario.Id = idResponsable)

                            Dim xbatBLL As New BLL.XbatBLL
                            Dim moneda As ELL.Moneda = xbatBLL.GetMoneda("EUR")

                            For Each hoja As ELL.HojaGastos In lHojas
                                For Each gasto As ELL.HojaGastos.Linea In hoja.Lineas
                                    If (gasto.TipoGasto = ELL.HojaGastos.Linea.eTipoGasto.Metalico) Then
                                        total += gasto.ImporteEuros
                                    End If
                                Next
                            Next
                        End If
                        oViaje.Anticipo.EurosPendientesHojaGastosLiq = total
                    End If
                End If
            End If
        End Sub

        ''' <summary>
        ''' Devuelve una lista indicando si cada integrante esta libre o no en esas fechas
        ''' </summary>
        ''' <param name="idViaje">Id del viaje que habra que excluir en la busqueda</param>
        ''' <param name="fechaIda">Fecha de ida</param>
        ''' <param name="fechaVuelta">Fecha de vuelta</param>
        ''' <param name="lIntegrantes">Los integrantes a chequear</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Lista de array de enteros.Pos 0:IdIntegrante,1:(0:free,1:busy)</returns>
        Function freeBusy(ByVal idViaje As Integer, ByVal fechaIda As Date, ByVal fechaVuelta As Date, ByVal lIntegrantes As List(Of SabLib.ELL.Usuario), ByVal idPlanta As Integer) As List(Of Integer())
            Return viajesDAL.freeBusy(idViaje, fechaIda, fechaVuelta, lIntegrantes, idPlanta)
        End Function

        ''' <summary>
        ''' Devuelve una lista indicando si cada integrante esta libre o no en esas fechas
        ''' </summary>
        ''' <param name="idViaje">Id del viaje que habra que excluir en la busqueda</param>
        ''' <param name="lIntegFecha">Los integrantes a chequear con sus fechas de ida y vuelta</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Lista de array de enteros.Pos 0:IdIntegrante,1:(0:free,1:busy)</returns>
        Function freeBusyIntegrantes(ByVal idViaje As Integer, ByVal lIntegFecha As List(Of String()), ByVal idPlanta As Integer) As List(Of Integer())
            Dim lResul As New List(Of Integer())
            Dim lInteg As List(Of SabLib.ELL.Usuario)
            Dim iResul As Integer()
            For Each sInteg As String() In lIntegFecha
                lInteg = New List(Of SabLib.ELL.Usuario)
                lInteg.Add(New SabLib.ELL.Usuario With {.Id = CInt(sInteg(0))})
                iResul = viajesDAL.freeBusy(idViaje, CDate(sInteg(1)), CDate(sInteg(2)), lInteg, idPlanta).FirstOrDefault
                If (iResul IsNot Nothing) Then lResul.Add(iResul)
            Next
            Return lResul
        End Function

        ''' <summary>
        ''' Comprueba si algun viaje, tiene el proyecto asignado
        ''' </summary>
        ''' <param name="idPrograma">Id del proyecto</param>
        ''' <returns></returns>        
        Function existeViajeConProyecto(ByVal idPrograma As Integer) As Boolean
            Return viajesDAL.existeViajeConProyecto(idPrograma)
        End Function


        ''' <summary>
        ''' Obtiene los paises por tipo de viaje
        ''' </summary>
        ''' <returns></returns>		
        Public Function GetPaisesTipoViaje(ByVal idTipoViaje As Integer) As List(Of ELL.Pais)
            Try
                Return viajesDAL.GetPaisesTipoViaje(idTipoViaje)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de los paises", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el viaje
        ''' </summary>
        ''' <param name="oViaje">Objeto viaje con la informacion</param>
        ''' <param name="idUserModif">Id del usuario que guarda los cambios</param>
        ''' <returns>Id del viaje</returns>        
        Public Function Save(ByVal oViaje As ELL.Viaje, ByVal idUserModif As Integer) As Integer
            Return viajesDAL.Save(oViaje, idUserModif)
        End Function

        ''' <summary>
        ''' Actualiza el responsable de liquidacion de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id viaje</param>
        ''' <param name="idRespLiq">Id responsable liquidacion</param>
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>        
        Public Sub UpdateResponsableLiquidacion(ByVal idViaje As Integer, ByVal idRespLiq As Integer, Optional ByVal con As OracleConnection = Nothing)
            viajesDAL.UpdateResponsableLiquidacion(idViaje, idRespLiq, con)
        End Sub

        ''' <summary>
        ''' Marca el viaje como obsoleto
        ''' Cambia el estado de la solicitud a cancelada
        ''' Cambia el estado de la solicitud a cancelada
        ''' Si los parametros opcionales vienen a nothing, habra que consultar el viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje a cancelar</param>  
        ''' <param name="idUser">Id del usuario que cancela el viaje</param>
        ''' <param name="bTieneSolicitud">Si tiene solicitud de agencia, cambia su estado a cancelada</param>      
        ''' <param name="bTieneAnticipo">Si tiene anticipo, cambia su estado a cancelada</param>      
        Public Sub Delete(ByVal idViaje As Integer, ByVal idUser As Integer, Optional ByVal bTieneSolicitud As Nullable(Of Boolean) = Nothing, Optional ByVal bTieneAnticipo As Nullable(Of Boolean) = Nothing)
            Dim transact As OracleTransaction = Nothing
            Dim myConnection As OracleConnection = Nothing
            Try
                myConnection = New OracleConnection(viajesDAL.Conexion)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                If (bTieneSolicitud Is Nothing) Then
                    Dim oViaje As ELL.Viaje = loadInfo(idViaje)
                    bTieneSolicitud = (oViaje.SolicitudAgencia IsNot Nothing)
                    bTieneAnticipo = (oViaje.Anticipo IsNot Nothing)
                End If
                '1º Se marca el viaje como obsoleto
                viajesDAL.Delete(idViaje, idUser, myConnection)

                '2º Se marca la solicitud de agencia como cerrada
                If (bTieneSolicitud) Then
                    Dim agenciasBLL As New BLL.SolicAgenciasBLL
                    agenciasBLL.Delete(idViaje, myConnection)
                End If

                '3º Si el estado del anticipo es menor a 4(entregado), se borra. sino, no se hace nada
                If (bTieneAnticipo) Then
                    Dim anticipBLL As New BLL.AnticiposBLL
                    anticipBLL.Delete(idViaje, myConnection)
                End If

                transact.Commit()
            Catch batzEx As BatzException
                transact.Rollback()
                Throw batzEx
            Catch ex As Exception
                transact.Rollback()
                Throw New BatzException("Error al cancelar el viaje", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

        ''' <summary>
        ''' Añade a la tabla de estados, el cambio de estado del viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="estado">Estado al que cambia</param>
        ''' <param name="fecha">Fecha</param>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="comentario">Comentario de rechazo o validacion</param>
        Public Sub CambiarEstadoViaje(ByVal idViaje As Integer, ByVal estado As Integer, ByVal fecha As DateTime, ByVal idUser As Integer, ByVal comentario As String)
            viajesDAL.CambiarEstadoViaje(idViaje, estado, fecha, idUser, comentario)
        End Sub

#End Region

#Region "Solicitudes plantas filiales"

        ''' <summary>
        ''' Obtiene las solicitudes de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>
        Public Function loadSolPlantasFiliales(ByVal idViaje As Integer, Optional ByVal idPlanta As Integer = Integer.MinValue) As List(Of ELL.Viaje.SolicitudPlantaFilial)
            Return viajesDAL.loadSolPlantasFiliales(idViaje, idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene las solicitudes de plantas filiales de un usuario entre dos fechas
        ''' </summary>
        ''' <param name="idUser">Id del gerente a buscar</param>
        ''' <param name="fechaInicio">Fecha de inicio. Si no se informa, no se tendran en cuenta</param>
        ''' <param name="fechaFin">Fecha de fin. Si no se informa, no se tendran en cuenta</param>
        ''' <param name="estado">Estado de la solicitud.Si es integer.minvalue no se tendra en cuenta</param>
        ''' <returns></returns>
        Public Function loadSolPlantasFiliales(ByVal idUser As Integer, ByVal fechaInicio As DateTime, ByVal fechaFin As DateTime, ByVal estado As Integer) As List(Of String())
            Return viajesDAL.loadSolPlantasFiliales(idUser, fechaInicio, fechaFin, estado)
        End Function

        ''' <summary>
        ''' Guarda los cambios de las solicitudes nuevas
        ''' - Nuevas:Inserta
        ''' - La misma: Update
        ''' - Son viejas: Delete
        ''' </summary>
        ''' <param name="lSolicitudes">Lista de solicitudes</param>        
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>             
        Sub SaveSolPlantaFilial(ByVal lSolicitudes As List(Of ELL.Viaje.SolicitudPlantaFilial), Optional ByVal con As OracleConnection = Nothing)
            Dim lSolOld As List(Of ELL.Viaje.SolicitudPlantaFilial) = loadSolPlantasFiliales(lSolicitudes.First.IdViaje)
            For Each sol As ELL.Viaje.SolicitudPlantaFilial In lSolicitudes
                SaveSolPlantaFilial(sol, con)
            Next
            If (lSolOld IsNot Nothing AndAlso lSolOld.Count > 0) Then
                For Each sol As ELL.Viaje.SolicitudPlantaFilial In lSolOld
                    If (Not lSolicitudes.Exists(Function(o As ELL.Viaje.SolicitudPlantaFilial) o.IdPlantaFilial = sol.IdPlantaFilial)) Then
                        DeleteSolPlantaFilial(sol.IdViaje, sol.IdPlantaFilial, con)
                    End If
                Next
            End If
        End Sub

        ''' <summary>
        ''' Inserta o modifica la informacion de la solicitud de plantas filiales
        ''' </summary>
        ''' <param name="oSolicitud">Lista de solicitudes</param>        
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>             
        Sub SaveSolPlantaFilial(ByVal oSolicitud As ELL.Viaje.SolicitudPlantaFilial, Optional ByVal con As OracleConnection = Nothing)
            viajesDAL.SaveSolPlantaFilial(oSolicitud, con)
        End Sub

        ''' <summary>
        ''' Elimina las solicitudes de plantas filiales
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="idPlanta">Si la planta es menor que 0, se borraran todos los del viaje</param>
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>             
        Sub DeleteSolPlantaFilial(ByVal idViaje As Integer, ByVal idPlanta As Integer, Optional ByVal con As OracleConnection = Nothing)
            viajesDAL.DeleteSolPlantaFilial(idViaje, idPlanta, con)
        End Sub
#End Region

#Region "Documentos de cliente"

        ''' <summary>
        ''' Obtiene el documento especificado de un cliente     
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>                
        ''' <returns></returns>             
        Public Function loadDocumentoCliente(ByVal idDoc As Integer) As ELL.Viaje.DocumentoCliente
            Return viajesDAL.loadDocumentoCliente(idDoc)
        End Function

        ''' <summary>
        ''' Obtiene todos los documentos de cliente de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Public Function loadDocumentosCliente(ByVal idViaje As Integer) As List(Of ELL.Viaje.DocumentoCliente)
            Return viajesDAL.loadDocumentosViaje(idViaje)
        End Function

        ''' <summary>
        ''' Añade un documento de cliente a un viaje
        ''' Se marca el viaje como tipo de desplazamiento de cliente, por si no lo estuviera todavia
        ''' </summary>
        ''' <param name="oDoc">Documento a añadir</param>
        ''' <returns>Devuelve el idDoc añadido</returns>                
        Public Function AddDocumentoCliente(ByVal oDoc As ELL.Viaje.DocumentoCliente) As Integer
            Dim idDoc As Integer = viajesDAL.AddDocumentoCliente(oDoc)
            '05/06/13: Se comenta porque a veces sera de otros o de cliente. viajesDAL.SetTipoDesplazamiento(oDoc.IdViaje, ELL.Viaje.TipoDesplaz.Cliente)
            Return idDoc
        End Function

        ''' <summary>
        ''' Elimina un documento de cliente del viaje
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>                 
        Public Sub DeleteDocumentoCliente(ByVal idDoc As Integer)
            viajesDAL.DeleteDocumentoCliente(idDoc)
        End Sub

#End Region

#Region "Documentos de los integrantes"

        ''' <summary>
        ''' Obtiene el documento especificado        
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>                
        ''' <returns></returns>        
        Public Function loadDocumentoIntegrante(ByVal idDoc As Integer) As ELL.Viaje.DocumentoIntegrante
            Return viajesDAL.loadDocumentoIntegrante(idDoc)
        End Function

        ''' <summary>
        ''' Obtiene todos los documentos de un viaje y un usuario
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="idIntegrante">Id del integrante</param>
        ''' <returns></returns>        
        Public Function loadDocumentosIntegrante(ByVal idViaje As Integer, ByVal idIntegrante As Integer) As List(Of ELL.Viaje.DocumentoIntegrante)
            Return viajesDAL.loadDocumentosIntegrante(idViaje, idIntegrante)
        End Function

        ''' <summary>
        ''' Añade un documento a un viaje
        ''' </summary>
        ''' <param name="oDoc">Documento a añadir</param>
        ''' <returns>Devuelve el idDoc añadido</returns>                
        Public Function AddDocumentoIntegrante(ByVal oDoc As ELL.Viaje.DocumentoIntegrante) As Integer
            Return viajesDAL.AddDocumentoIntegrante(oDoc)
        End Function

        ''' <summary>
        ''' Elimina un documento de la hoja
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>                
        Public Sub DeleteDocumentoIntegrante(ByVal idDoc As Integer)
            viajesDAL.DeleteDocumentoIntegrante(idDoc)
        End Sub

#End Region

    End Class

End Namespace