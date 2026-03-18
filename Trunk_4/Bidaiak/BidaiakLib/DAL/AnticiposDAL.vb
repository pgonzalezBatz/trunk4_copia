Namespace DAL

    Public Class AnticiposDAL

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
        ''' Obtiene los anticipos de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <returns></returns>        
        Public Function loadInfo(ByVal idViaje As Integer) As ELL.Anticipo
            Try
                Dim query As String = "SELECT ID_VIAJE,FECHA_NECESIDAD,ESTADO FROM ANTICIPOS WHERE ID_VIAJE=:ID_VIAJE"
                parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input)
                Dim sablibBLL As New SabLib.BLL.UsuariosComponent

                Dim lAnticipos As List(Of ELL.Anticipo) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Anticipo)(Function(r As OracleDataReader) _
                 New ELL.Anticipo With {.IdViaje = CInt(r(0)), .FechaNecesidad = CDate(r(1)), .Estado = CInt(r(2))}, query, cn, parameter)

                Dim oAnticipo As ELL.Anticipo = Nothing
                If (lAnticipos IsNot Nothing AndAlso lAnticipos.Count > 0) Then oAnticipo = lAnticipos.Item(0)
                Return (oAnticipo)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el anticipo de un viaje", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los movimientos del anticipo de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="tipoMov">Si se quieren solo las de un tipo en especifico</param>
        ''' <returns></returns>        
        Public Function loadLines(ByVal idViaje As Integer, Optional ByVal tipoMov As ELL.Anticipo.Movimiento.TipoMovimiento = Integer.MinValue) As List(Of String())
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT ID,ID_ANTICIPO,ID_MONEDA,CANTIDAD,FECHA,ID_USER_ORIG,ID_USER_DEST,ID_VIAJE_ORIG,ID_VIAJE_DEST,COMENTARIO,TIPO_MOV,EUROS,CAMBIO_MONEDA FROM MOVIMIENTOS WHERE (ID_ANTICIPO=:ID_ANTICIPO or ID_VIAJE_DEST=:ID_ANTICIPO) AND OBSOLETO=0"
                lParametros.Add(New OracleParameter("ID_ANTICIPO", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                If (tipoMov <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("TIPO_MOV", OracleDbType.Int32, tipoMov, ParameterDirection.Input))
                    query &= " AND TIPO_MOV=:TIPO_MOV"
                End If
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de las lineas del anticipos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los diversos estados y sus fechas por las que ha pasado un anticipo
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <returns></returns>        
        Public Function loadStates(ByVal idViaje As Integer) As List(Of String())
            Try
                Dim query As String = "SELECT ESTADO,FECHA FROM ANTICIPOS_ESTADOS WHERE ID_ANTICIPO=:ID_ANTICIPO"
                parameter = New OracleParameter("ID_ANTICIPO", OracleDbType.Int32, idViaje, ParameterDirection.Input)
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los estados del anticipo", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de anticipos de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>  
        Public Function loadList(ByVal idPlanta As Integer) As List(Of String())
            Try
                Dim query As String = "SELECT A.ID_VIAJE,V.DESTINO,A.FECHA_NECESIDAD,A.ESTADO,CONCAT(CONCAT(CONCAT(CONCAT(U.NOMBRE,' '),U.APELLIDO1),' '),U.APELLIDO2) AS NOMBRE FROM ANTICIPOS A INNER JOIN VIAJES V ON A.ID_VIAJE=V.ID INNER JOIN SAB.USUARIOS U ON V.ID_USER_SOLIC=U.ID WHERE V.ID_PLANTA=:ID_PLANTA "
                Dim parameter As New OracleParameter
                parameter = New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los anticipos en un estado preparado cuya fecha de necesidad del anticipo este entre las fechas indicadas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fechaInicio">Fecha de inicio</param>
        ''' <param name="fechaFin">Fecha de fin</param>
        ''' <returns></returns>    
        Public Function loadLinesAnticiposPreparados(ByVal idPlanta As Integer, ByVal fechaInicio As Date, ByVal fechaFin As Date) As List(Of ELL.Anticipo.Movimiento)
            Dim query As String = "SELECT M.ID_MONEDA,M.CANTIDAD " &
                            "FROM ANTICIPOS A INNER JOIN MOVIMIENTOS M ON A.ID_VIAJE=M.ID_ANTICIPO INNER JOIN VIAJES V ON A.ID_VIAJE=V.ID " &
                            "WHERE A.FECHA_NECESIDAD BETWEEN :FECHA_INICIO AND :FECHA_FIN AND M.OBSOLETO=0 AND M.TIPO_MOV=1 AND V.ID_PLANTA=:ID_PLANTA AND V.ESTADO=1 AND A.ESTADO=:ESTADO " &
                            "AND NOT EXISTS (SELECT M2.ID_VIAJE_ORIG FROM MOVIMIENTOS M2 WHERE M2.TIPO_MOV=2 AND M2.ID_ANTICIPO=M.ID_ANTICIPO)"
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("FECHA_INICIO", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("FECHA_FIN", OracleDbType.Date, fechaFin, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, ELL.Anticipo.EstadoAnticipo.Preparado, ParameterDirection.Input))
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Anticipo.Movimiento)(Function(r As OracleDataReader) _
                 New ELL.Anticipo.Movimiento With {.Cantidad = CDec(r("CANTIDAD")), .Moneda = New ELL.Moneda With {.Id = CInt(r("ID_MONEDA"))}}, query, cn, lParametros.ToArray)
        End Function

        ''' <summary>
        ''' Obtiene las remesas entre dos fechas
        ''' Agrupa los de igual tipo de moneda(€,$) y le asigna la fecha de necesidad la mas baja de las que encuentre
        ''' No se mostraran los anticipos ya entregados
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fechaInicio">Fecha de inicio</param>
        ''' <param name="fechaFin">Fecha de fin</param>
        ''' <param name="idMoneda">Especifica el tipo de moneda del que se quieren obtener las remesas</param>
        ''' <param name="viajeUnico">Se sabe que en estas fechas solo hay un viaje, asi que se pide la consulta sin group by,para que le devuelva el id_viaje</param>
        ''' <returns></returns>    
        Public Function loadRemesas(ByVal idPlanta As Integer, ByVal fechaInicio As Date, ByVal fechaFin As Date, ByVal idMoneda As Integer, ByVal viajeUnico As Boolean) As List(Of String())
            Try
                Dim query As String = String.Empty
                If (idMoneda <> Integer.MinValue) Then
                    'Aqui no se pone group by porque sino, no me devuelve todos los resultados de una misma moneda
                    query = "SELECT ID_MONEDA,CANTIDAD,A.FECHA_NECESIDAD,A.ID_VIAJE,A.ESTADO " &
                            "FROM ANTICIPOS A INNER JOIN MOVIMIENTOS M ON A.ID_VIAJE=M.ID_ANTICIPO INNER JOIN VIAJES V ON A.ID_VIAJE=V.ID " &
                            "WHERE A.FECHA_NECESIDAD BETWEEN :FECHA_INICIO AND :FECHA_FIN AND ID_MONEDA=:ID_MONEDA AND M.OBSOLETO=0 AND M.TIPO_MOV=1 AND V.ID_PLANTA=:ID_PLANTA AND V.ESTADO=1 AND A.ESTADO<>0 " &
                            "AND NOT EXISTS (SELECT M2.ID_VIAJE_ORIG FROM MOVIMIENTOS M2 WHERE M2.TIPO_MOV=2 AND M2.ID_ANTICIPO=M.ID_ANTICIPO)"
                Else
                    If (viajeUnico) Then
                        query = "SELECT ID_MONEDA,CANTIDAD,A.FECHA_NECESIDAD,ID_VIAJE,A.ESTADO " &
                                "FROM ANTICIPOS A INNER JOIN MOVIMIENTOS M ON A.ID_VIAJE=M.ID_ANTICIPO INNER JOIN VIAJES V ON A.ID_VIAJE=V.ID " &
                                "WHERE A.FECHA_NECESIDAD BETWEEN :FECHA_INICIO AND :FECHA_FIN AND M.OBSOLETO=0 AND M.TIPO_MOV=1 AND V.ID_PLANTA=:ID_PLANTA AND V.ESTADO=1 AND A.ESTADO<>0 " &
                                "AND NOT EXISTS (SELECT M2.ID_VIAJE_ORIG FROM MOVIMIENTOS M2 WHERE M2.TIPO_MOV=2 AND M2.ID_ANTICIPO=M.ID_ANTICIPO)"
                    Else
                        query = "SELECT ID_MONEDA,SUM(CANTIDAD) AS CANTIDAD,MIN(A.FECHA_NECESIDAD) AS FECHA_NECESIDAD " &
                                "FROM ANTICIPOS A INNER JOIN MOVIMIENTOS M ON A.ID_VIAJE=M.ID_ANTICIPO INNER JOIN VIAJES V ON A.ID_VIAJE=V.ID " &
                                "WHERE A.FECHA_NECESIDAD BETWEEN :FECHA_INICIO And :FECHA_FIN AND M.OBSOLETO=0 AND M.TIPO_MOV=1 AND V.ID_PLANTA=:ID_PLANTA AND V.ESTADO=1 AND A.ESTADO<>0 " &
                                "AND NOT EXISTS (SELECT M2.ID_VIAJE_ORIG FROM MOVIMIENTOS M2 WHERE M2.TIPO_MOV=2 AND M2.ID_ANTICIPO=M.ID_ANTICIPO) " &
                                "GROUP BY ID_MONEDA"
                    End If
                End If
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("FECHA_INICIO", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_FIN", OracleDbType.Date, fechaFin, ParameterDirection.Input))
                If (idMoneda <> Integer.MinValue) Then lParametros.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, idMoneda, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener las remesas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Anticipos cancelados en los que se haya entregado el anticipo pero no haya sido devuelto
        ''' </summary>
        ''' <param name="idPlanta">Id planta</param>
        ''' <returns></returns>        
        Function loadListCanceladosNoDevueltos(ByVal idPlanta As Integer) As List(Of String())
            Try
                'Se compara con 2 porque el tipo_mov 2 es entregado. Si tuviera el 3 ya seria devuelto y entonces este no entraria
                Dim query As String = "SELECT A.ID_VIAJE,V.DESTINO FROM ANTICIPOS A INNER JOIN VIAJES V ON A.ID_VIAJE=V.ID WHERE A.ESTADO=:EST_CANCEL And V.ID_PLANTA=:ID_PLANTA AND 2=(SELECT MAX(M.TIPO_MOV) FROM MOVIMIENTOS M WHERE M.ID_ANTICIPO=A.ID_VIAJE)"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("EST_CANCEL", OracleDbType.Int32, ELL.Anticipo.EstadoAnticipo.cancelada, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParameters.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de anticipos cancelados no devueltos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga los anticipos pendientes de justificar
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idUser">Id del usuario a comprobar</param>
        ''' <param name="idAnticipoExcept">Id del anticipo que no se contara ya que es el que tiene abierto</param>
        ''' <returns></returns>
        Public Function loadAnticiposPendientes(ByVal idPlanta As Integer, ByVal idUser As Integer, ByVal idAnticipoExcept As Integer)
            Try
                Dim query As String = "SELECT DISTINCT A.ID_VIAJE,A.ESTADO AS ESTADO_ANTIC,'' AS FECHA_ESTADO_ANTIC,U.ID AS ID_LIQ,U.CODPERSONA AS NUM_TRAB,concat(concat(concat(concat(trim(U.NOMBRE),' '),trim(U.APELLIDO1)),' '),trim(U.APELLIDO2)) AS LIQUIDADOR, " _
                                    & "U.FECHABAJA,V.FECHA_IDA,V.FECHA_VUELTA,HG.ID AS ID_HG,HG.ESTADO AS ESTADO_HG,HGE.FECHA AS FECHA_ESTADO_HG " _
                                    & "FROM ANTICIPOS A INNER JOIN VIAJES V ON A.ID_VIAJE=V.ID " _
                                    & "INNER JOIN ANTICIPOS_ESTADOS AE ON (AE.ID_ANTICIPO=A.ID_VIAJE AND AE.ESTADO=A.ESTADO) " _
                                    & "INNER JOIN SAB.USUARIOS U ON V.ID_RESP_LIQUIDACION=U.ID " _
                                    & "LEFT JOIN HOJA_GASTOS HG ON (HG.ID_VIAJE=V.ID AND HG.ID_USER=V.ID_RESP_LIQUIDACION) " _
                                    & "LEFT JOIN HOJA_GASTOS_ESTADOS HGE ON (HGE.ID_HOJA=HG.ID AND HGE.ESTADO=HG.ESTADO) " _
                                    & "WHERE A.ESTADO=4 AND V.ESTADO=:ESTADO_VALIDADO AND A.ID_VIAJE NOT IN(SELECT M.ID_ANTICIPO FROM MOVIMIENTOS M WHERE M.ID_ANTICIPO=A.ID_VIAJE AND M.TIPO_MOV=5) AND V.FECHA_IDA>=:FECHA AND V.ID_PLANTA=:ID_PLANTA"
                'AE.FECHA AS FECHA_ESTADO_ANTIC
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, New Date(2020, 1, 1), ParameterDirection.Input)) 'Antes del 2020 no mira
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ESTADO_VALIDADO", OracleDbType.Int32, ELL.Viaje.eEstadoViaje.Validado, ParameterDirection.Input))
                If (idUser > 0) Then
                    query &= " AND U.ID=:ID_USER"
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                End If
                If (idAnticipoExcept > 0) Then
                    query &= " AND A.ID_VIAJE<>:ID_ANTICIPO_EXCEPT"
                    lParametros.Add(New OracleParameter("ID_ANTICIPO_EXCEPT", OracleDbType.Int32, idAnticipoExcept, ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los anticipos pendientes de justificar", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica el anticipo y sus lineas
        ''' </summary>
        ''' <param name="oAntic">Objeto solicitud con la informacion</param>        
        ''' <param name="bSaveOnlyHeader">Indica si se guarda solo la cabecera</param>
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>                     
        Sub Save(ByVal oAntic As ELL.Anticipo, ByVal bSaveOnlyHeader As Boolean, Optional ByVal con As OracleConnection = Nothing)
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim bUpdate As Boolean
            Try
                'Esta funcion de por si, sera una transaccion, pero si ya viene una conexion abierta, se funcionara con ella
                If (con Is Nothing) Then
                    myCon = New OracleConnection(cn)
                    myCon.Open()
                    transact = myCon.BeginTransaction()
                Else
                    myCon = con
                End If
                Dim query As String = String.Empty
                Dim bNuevo As Boolean = False
                '1º Se comprueba si hay que insertar o modificar
                query = "SELECT COUNT(ID_VIAJE) FROM ANTICIPOS WHERE ID_VIAJE=:ID_VIAJE"
                parameter = New OracleParameter("ID_VIAJE", OracleDbType.Int32, oAntic.IdViaje, ParameterDirection.Input)
                bNuevo = (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, con, parameter) = 0)
                '2º Se inserta o actualiza la cabecera
                Dim lParameters As New List(Of OracleParameter)
                If (bNuevo) Then 'Insert
                    query = "INSERT INTO ANTICIPOS(FECHA_NECESIDAD,ESTADO,ID_VIAJE) VALUES(:FECHA_NECESIDAD,:ESTADO,:ID_VIAJE)"
                Else 'update
                    query = "UPDATE ANTICIPOS SET FECHA_NECESIDAD=:FECHA_NECESIDAD,ESTADO=:ESTADO WHERE ID_VIAJE=:ID_VIAJE"
                End If
                lParameters.Add(New OracleParameter("FECHA_NECESIDAD", OracleDbType.Date, oAntic.FechaNecesidad, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oAntic.Estado, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, oAntic.IdViaje, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myCon, lParameters.ToArray)
                If (Not bSaveOnlyHeader) Then
                    '3º Si es nuevo, se inserta el estado 
                    If (bNuevo) Then CambiarEstado(oAntic.IdViaje, ELL.Anticipo.EstadoAnticipo.solicitado, myCon)
                    '3º Se eliminan aquellos movimientos de la base de datos que no existen en los movimientos a guardar
                    Dim lMovBBDD As List(Of ELL.Anticipo.Movimiento) = Nothing
                    Dim anticBLL As New BLL.AnticiposBLL
                    If (Not bNuevo) Then lMovBBDD = anticBLL.loadMovimientos(oAntic.IdViaje)
                    Dim idMov As Integer
                    If (lMovBBDD IsNot Nothing) Then
                        'Se eliminan de la base de datos, aquellas filas que estan en la base de datos pero no en la lista a guardar                                           
                        For Each oMov As ELL.Anticipo.Movimiento In lMovBBDD
                            idMov = oMov.Id
                            If Not (oAntic.Movimientos.Exists(Function(o As ELL.Anticipo.Movimiento) o.Id = idMov)) Then
                                query = "DELETE FROM MOVIMIENTOS WHERE ID=:ID"
                                Memcached.OracleDirectAccess.NoQuery(query, myCon, New OracleParameter(":ID", OracleDbType.Int32, idMov, ParameterDirection.Input))
                            End If
                        Next
                    End If
                    '4º Se insertan o actualizan los movimientos   
                    For Each oMov As ELL.Anticipo.Movimiento In oAntic.Movimientos
                        bUpdate = False
                        idMov = oMov.Id

                        If (Not bNuevo) Then 'Insert                        
                            If (lMovBBDD.Exists(Function(o As ELL.Anticipo.Movimiento) o.Id = idMov)) Then  'Existe la linea, por tanto se actualiza
                                bUpdate = True
                            End If
                        End If
                        If (Not bUpdate) Then
                            query = "INSERT INTO MOVIMIENTOS(ID_ANTICIPO,ID_MONEDA,CANTIDAD,FECHA,ID_USER_ORIG,ID_USER_DEST,ID_VIAJE_ORIG,ID_VIAJE_DEST,COMENTARIO,TIPO_MOV,OBSOLETO,EUROS,CAMBIO_MONEDA) VALUES" _
                              & "(:ID_ANTICIPO,:ID_MONEDA,:CANTIDAD,:FECHA,:ID_USER_ORIG,:ID_USER_DEST,:ID_VIAJE_ORIG,:ID_VIAJE_DEST,:COMENTARIO,:TIPO_MOV,0,:EUROS,:CAMBIO_MONEDA)"
                            lParameters = New List(Of OracleParameter)
                            lParameters.Add(New OracleParameter("ID_ANTICIPO", OracleDbType.Int32, oAntic.IdViaje, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, oMov.Moneda.Id, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("CANTIDAD", OracleDbType.Decimal, oMov.Cantidad, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("FECHA", OracleDbType.Date, oMov.Fecha, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_USER_ORIG", OracleDbType.Int32, oMov.UserOrigen.Id, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_USER_DEST", OracleDbType.Int32, If(oMov.UserDestino IsNot Nothing, oMov.UserDestino.Id, DBNull.Value), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_VIAJE_ORIG", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oMov.IdViajeOrigen), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_VIAJE_DEST", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oMov.IdViajeDestino), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("COMENTARIO", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oMov.Comentarios), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("TIPO_MOV", OracleDbType.Int32, oMov.TipoMov, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("EUROS", OracleDbType.Decimal, oMov.ImporteEuros, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("CAMBIO_MONEDA", OracleDbType.Decimal, oMov.CambioMonedaEUR, ParameterDirection.Input))
                        Else
                            query = "UPDATE MOVIMIENTOS SET ID_MONEDA=:ID_MONEDA,CANTIDAD=:CANTIDAD,FECHA=:FECHA,ID_USER_ORIG=:ID_USER_ORIG,ID_USER_DEST=:ID_USER_DEST," _
                                       & "ID_VIAJE_ORIG=:ID_VIAJE_ORIG,ID_VIAJE_DEST=:ID_VIAJE_DEST,COMENTARIO=:COMENTARIO, TIPO_MOV=:TIPO_MOV, EUROS=:EUROS, CAMBIO_MONEDA=:CAMBIO_MONEDA WHERE ID=:ID"
                            lParameters = New List(Of OracleParameter)
                            lParameters.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, oMov.Moneda.Id, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("CANTIDAD", OracleDbType.Decimal, oMov.Cantidad, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("FECHA", OracleDbType.Date, oMov.Fecha, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_USER_ORIG", OracleDbType.Int32, oMov.UserOrigen.Id, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_USER_DEST", OracleDbType.Int32, If(oMov.UserDestino IsNot Nothing, oMov.UserDestino.Id, DBNull.Value), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_VIAJE_ORIG", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oMov.IdViajeOrigen), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID_VIAJE_DEST", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oMov.IdViajeDestino), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("COMENTARIO", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oMov.Comentarios), ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("TIPO_MOV", OracleDbType.Int32, oMov.TipoMov, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, oMov.Id, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("EUROS", OracleDbType.Decimal, oMov.ImporteEuros, ParameterDirection.Input))
                            lParameters.Add(New OracleParameter("CAMBIO_MONEDA", OracleDbType.Decimal, oMov.CambioMonedaEUR, ParameterDirection.Input))
                        End If
                        Memcached.OracleDirectAccess.NoQuery(query, myCon, lParameters.ToArray)
                    Next
                End If
                If (con Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (con Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al guardar el anticipo", ex)
            Finally
                If (con Is Nothing AndAlso myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Cancela la solicitud por el usuario. La marca como cancelada
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <param name="con">Conexion en caso de venir de una transaccion</param>             
        Sub Delete(ByVal idViaje As Integer, ByVal con As OracleConnection)
            Try
                'Primero se comprueba que este viaje haya tenido anticipos
                Dim query As String = "SELECT COUNT(*) FROM ANTICIPOS WHERE ID_VIAJE=:ID_VIAJE"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Decimal, idViaje, ParameterDirection.Input))
                Dim bTieneIdViaje As Boolean
                If (con Is Nothing) Then
                    bTieneIdViaje = (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParameters.ToArray) > 0)
                Else
                    bTieneIdViaje = (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, con, lParameters.ToArray) > 0)
                End If

                If (bTieneIdViaje) Then
                    query = "UPDATE ANTICIPOS SET ESTADO=0 WHERE ID_VIAJE=:ID_VIAJE"
                    lParameters = New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("ID_VIAJE", OracleDbType.Decimal, idViaje, ParameterDirection.Input))

                    If (con Is Nothing) Then
                        Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
                    Else
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    End If

                    query = "INSERT INTO ANTICIPOS_ESTADOS(ID_ANTICIPO,ESTADO,FECHA) VALUES(:ID_ANTICIPO,:ESTADO,:FECHA)"
                    lParameters = New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("ID_ANTICIPO", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("ESTADO", OracleDbType.Int32, ELL.Anticipo.EstadoAnticipo.cancelada, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter("FECHA", OracleDbType.Date, Now, ParameterDirection.Input))

                    If (con Is Nothing) Then
                        Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
                    Else
                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)
                    End If
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al cancelar la solicitud de anticipo por parte del usuario", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Se borra el anticipo
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>                
        ''' <return>Antiguo liquidador</return>
        Function Anular(ByVal idViaje As Integer) As Integer
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim idLiquidador As Integer = 0
            Try
                myCon = New OracleConnection(cn)
                myCon.Open()
                transact = myCon.BeginTransaction()

                'Se comprueba si existen transferencias
                Dim query As String = "SELECT COUNT(ID) FROM MOVIMIENTOS WHERE ID_VIAJE_DEST=:ID"
                Dim numTrans As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myCon, New OracleParameter("ID", OracleDbType.Int32, idViaje, ParameterDirection.Input))

                query = "DELETE FROM MOVIMIENTOS WHERE ID_ANTICIPO=:ID"
                Memcached.OracleDirectAccess.NoQuery(query, myCon, New OracleParameter("ID", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                'Obtener el liquidador
                query = "SELECT ID_RESP_LIQUIDACION FROM VIAJES WHERE ID=:ID"
                idLiquidador = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myCon, New OracleParameter("ID", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                'Si hay transferencias no se borran las tablas ya que se tienen que mantener para el contacto
                If (numTrans = 0) Then
                    query = "DELETE FROM ANTICIPOS_ESTADOS WHERE ID_ANTICIPO=:ID"
                    Memcached.OracleDirectAccess.NoQuery(query, myCon, New OracleParameter("ID", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    query = "DELETE FROM ANTICIPOS WHERE ID_VIAJE=:ID"
                    Memcached.OracleDirectAccess.NoQuery(query, myCon, New OracleParameter("ID", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    query = "UPDATE VIAJES SET ID_RESP_LIQUIDACION=NULL WHERE ID=:ID"
                    Memcached.OracleDirectAccess.NoQuery(query, myCon, New OracleParameter("ID", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                End If
                transact.Commit()
                Return idLiquidador
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al anular el anticipo" & ":" & idViaje, ex)
            Finally
                If (myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Function

        ''' <summary>
        ''' Inserta o modifica un movimiento
        ''' </summary>
        ''' <param name="oMov">Objeto con la informacion</param> 
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>   
        Public Sub SaveMovimiento(ByVal oMov As ELL.Anticipo.Movimiento, Optional ByVal con As OracleConnection = Nothing)
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim parameters(11) As OracleParameter
            Try
                'Esta funcion de por si, sera una transaccion, pero si ya viene una conexion abierta, se funcionara con ella
                If (con Is Nothing) Then
                    myCon = New OracleConnection(cn)
                    myCon.Open()
                    transact = myCon.BeginTransaction()
                Else
                    myCon = con
                End If
                If (oMov.Id = Integer.MinValue) Then 'Insert
                    query = "INSERT INTO MOVIMIENTOS(ID_ANTICIPO,ID_MONEDA,CANTIDAD,FECHA,ID_USER_ORIG,ID_USER_DEST,ID_VIAJE_ORIG,ID_VIAJE_DEST,COMENTARIO,TIPO_MOV,OBSOLETO,EUROS,CAMBIO_MONEDA) VALUES" _
                            & "(:ID_ANTICIPO,:ID_MONEDA,:CANTIDAD,:FECHA,:ID_USER_ORIG,:ID_USER_DEST,:ID_VIAJE_ORIG,:ID_VIAJE_DEST,:COMENTARIO,:TIPO_MOV,0,:EUROS,:CAMBIO_MONEDA)"

                    parameters(0) = New OracleParameter(":ID_ANTICIPO", OracleDbType.Int32, oMov.IdAnticipo, ParameterDirection.Input)
                    parameters(1) = New OracleParameter(":ID_MONEDA", OracleDbType.Int32, oMov.Moneda.Id, ParameterDirection.Input)
                    parameters(2) = New OracleParameter(":CANTIDAD", OracleDbType.Decimal, oMov.Cantidad, ParameterDirection.Input)
                    parameters(3) = New OracleParameter(":FECHA", OracleDbType.Date, oMov.Fecha, ParameterDirection.Input)
                    parameters(4) = New OracleParameter(":ID_USER_ORIG", OracleDbType.Int32, oMov.UserOrigen.Id, ParameterDirection.Input)
                    parameters(5) = New OracleParameter(":ID_USER_DEST", OracleDbType.Int32, ParameterDirection.Input)
                    If (oMov.UserDestino IsNot Nothing) Then
                        parameters(5).Value = oMov.UserDestino.Id
                    Else
                        parameters(5).Value = DBNull.Value
                    End If
                    parameters(6) = New OracleParameter(":ID_VIAJE_ORIG", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oMov.IdViajeOrigen), ParameterDirection.Input)
                    parameters(7) = New OracleParameter(":ID_VIAJE_DEST", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oMov.IdViajeDestino), ParameterDirection.Input)
                    parameters(8) = New OracleParameter(":COMENTARIO", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oMov.Comentarios), ParameterDirection.Input)
                    parameters(9) = New OracleParameter(":TIPO_MOV", OracleDbType.Int32, oMov.TipoMov, ParameterDirection.Input)
                    parameters(10) = New OracleParameter("EUROS", OracleDbType.Decimal, oMov.ImporteEuros, ParameterDirection.Input)
                    parameters(11) = New OracleParameter("CAMBIO_MONEDA", OracleDbType.Decimal, oMov.CambioMonedaEUR, ParameterDirection.Input)
                Else 'update
                    query = "UPDATE MOVIMIENTOS SET ID_MONEDA=:ID_MONEDA,CANTIDAD=:CANTIDAD,FECHA=:FECHA,ID_USER_ORIG=:ID_USER_ORIG,ID_USER_DESTINO=:ID_USER_DESTINO" _
                                    & "ID_VIAJE_ORIG=:ID_VIAJE_ORIG,ID_VIAJE_DEST=:ID_VIAJE_DEST,COMENTARIO=:COMENTARIO, TIPO_MOV=:TIPO_MOV,EUROS:=EUROS,CAMBIO_MONEDA=:CAMBIO_MONEDA WHERE ID=:ID"

                    parameters(0) = New OracleParameter(":ID_MONEDA", OracleDbType.Int32, oMov.Moneda.Id, ParameterDirection.Input)
                    parameters(1) = New OracleParameter(":CANTIDAD", OracleDbType.Decimal, oMov.Cantidad, ParameterDirection.Input)
                    parameters(2) = New OracleParameter(":FECHA", OracleDbType.Date, oMov.Fecha, ParameterDirection.Input)
                    parameters(3) = New OracleParameter(":ID_USER_ORIG", OracleDbType.Int32, oMov.UserOrigen.Id, ParameterDirection.Input)
                    parameters(4) = New OracleParameter(":ID_USER_DEST", OracleDbType.Int32, ParameterDirection.Input)
                    If (oMov.UserDestino IsNot Nothing) Then
                        parameters(4).Value = oMov.UserDestino.Id
                    Else
                        parameters(4).Value = DBNull.Value
                    End If
                    parameters(5) = New OracleParameter(":ID_VIAJE_ORIG", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oMov.IdViajeOrigen), ParameterDirection.Input)
                    parameters(6) = New OracleParameter(":ID_VIAJE_DEST", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oMov.IdViajeDestino), ParameterDirection.Input)
                    parameters(7) = New OracleParameter(":COMENTARIO", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oMov.Comentarios), ParameterDirection.Input)
                    parameters(8) = New OracleParameter(":TIPO_MOV", OracleDbType.Int32, oMov.TipoMov, ParameterDirection.Input)
                    parameters(9) = New OracleParameter(":ID", OracleDbType.Int32, oMov.Id, ParameterDirection.Input)
                    parameters(10) = New OracleParameter("EUROS", OracleDbType.Decimal, oMov.ImporteEuros, ParameterDirection.Input)
                    parameters(11) = New OracleParameter("CAMBIO_MONEDA", OracleDbType.Decimal, oMov.CambioMonedaEUR, ParameterDirection.Input)
                End If

                Memcached.OracleDirectAccess.NoQuery(query, myCon, parameters)
                If (con Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (con Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al guardar la informacion", ex)
            Finally
                If (con Is Nothing AndAlso myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Inserta o modifica una lista de movimientos
        ''' </summary>
        ''' <param name="lMov">Lista de movimientos</param>
        ''' <param name="con">Parametro opcional con la conexion en caso de venir de una transaccion</param>         
        Public Sub SaveMovimientos(ByVal lMov As List(Of ELL.Anticipo.Movimiento), Optional ByVal con As OracleConnection = Nothing)
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                'Esta funcion de por si, sera una transaccion, pero si ya viene una conexion abierta, se funcionara con ella
                If (con Is Nothing) Then
                    myCon = New OracleConnection(cn)
                    myCon.Open()
                    transact = myCon.BeginTransaction()
                Else
                    myCon = con
                End If

                For Each oMov As ELL.Anticipo.Movimiento In lMov
                    SaveMovimiento(oMov, myCon)
                Next

                If (con Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (con Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al guardar la informacion", ex)
            Finally
                If (con Is Nothing AndAlso myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Marca un movimiento como obsoleto. No elimina de la base de datos
        ''' </summary>
        ''' <param name="idMov">Id del movimiento</param>  
        Public Sub DeleteMovimiento(ByVal idMov As Integer, Optional ByVal con As OracleConnection = Nothing)
            Try
                If (con Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery("DELETE FROM MOVIMIENTOS WHERE ID=:ID", cn, New OracleParameter("ID", OracleDbType.Int32, idMov, ParameterDirection.Input))
                Else
                    Memcached.OracleDirectAccess.NoQuery("DELETE FROM MOVIMIENTOS WHERE ID=:ID", con, New OracleParameter("ID", OracleDbType.Int32, idMov, ParameterDirection.Input))
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar el movimiento la informacion", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Cambia el estado de un anticipo
        ''' Si se cambiara a entregado, habra que pasar todos los movimientos solicitados a entregados
        ''' </summary>
        ''' <param name="idAnticipo">Id del anticipo</param>        
        ''' <param name="idEstado">Id del estado</param>     
        ''' <param name="idUser">Parametro opcional. Cuando se cambia a entregado, es necesario el usuario</param>
        Public Sub CambiarEstado(ByVal idAnticipo As Integer, ByVal idEstado As Integer, Optional ByVal con As OracleConnection = Nothing, Optional ByVal idUser As Integer = Integer.MinValue)
            Dim myCon As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim query As String = String.Empty
            Try
                Dim lParametros As List(Of OracleParameter) = Nothing
                'Esta funcion de por si, sera una transaccion, pero si ya viene una conexion abierta, se funcionara con ella
                If (con Is Nothing) Then
                    myCon = New OracleConnection(cn)
                    myCon.Open()
                    transact = myCon.BeginTransaction()
                Else
                    myCon = con
                End If
                query = "INSERT INTO ANTICIPOS_ESTADOS(ID_ANTICIPO,ESTADO,FECHA) VALUES (:ID_ANTICIPO,:ESTADO,:FECHA)"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":ID_ANTICIPO", OracleDbType.Int32, idAnticipo, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ESTADO", OracleDbType.Int32, idEstado, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":FECHA", OracleDbType.Date, Date.Now, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)

                query = "SELECT ESTADO FROM ANTICIPOS WHERE ID_VIAJE=:ID_VIAJE"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":ID_VIAJE", OracleDbType.Int32, idAnticipo, ParameterDirection.Input))
                Dim idEstadoOld As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myCon, lParametros.ToArray)

                query = "UPDATE ANTICIPOS SET ESTADO=:ESTADO WHERE ID_VIAJE=:ID_VIAJE"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter(":ESTADO", OracleDbType.Int32, idEstado, ParameterDirection.Input))
                lParametros.Add(New OracleParameter(":ID_VIAJE", OracleDbType.Int32, idAnticipo, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, myCon, lParametros.ToArray)

                'Si se cambia a entregado, se insertaran las lineas de solicitado a entregado
                If (idEstado = ELL.Anticipo.EstadoAnticipo.Entregado) Then
                    Dim anticBLL As New BLL.AnticiposBLL
                    Dim lMovs As List(Of ELL.Anticipo.Movimiento) = anticBLL.loadMovimientos(idAnticipo, ELL.Anticipo.Movimiento.TipoMovimiento.Solicitado)
                    If (lMovs IsNot Nothing AndAlso lMovs.Count > 0) Then
                        Dim oMovNew As ELL.Anticipo.Movimiento
                        For Each oMov As ELL.Anticipo.Movimiento In lMovs
                            oMovNew = oMov
                            oMovNew.Id = Integer.MinValue
                            If (idUser <> Integer.MinValue) Then  'Deberia venir informado con el usuario al que se le hace entrega
                                oMovNew.UserOrigen = New SabLib.ELL.Usuario With {.Id = idUser}
                            End If
                            oMovNew.Fecha = Date.Now
                            oMovNew.TipoMov = ELL.Anticipo.Movimiento.TipoMovimiento.Entregado
                            oMovNew.ImporteEuros = 0   'Para que se recalcule con la fecha actual
                            SaveMovimiento(oMov, myCon)
                        Next
                    End If
                ElseIf (idEstadoOld = ELL.Anticipo.EstadoAnticipo.Entregado AndAlso (idEstado = ELL.Anticipo.EstadoAnticipo.solicitado OrElse idEstado = ELL.Anticipo.EstadoAnticipo.Preparado)) Then
                    'Se eliminan los movimientos de entregado
                    Dim anticBLL As New BLL.AnticiposBLL
                    Dim lMovs As List(Of ELL.Anticipo.Movimiento) = anticBLL.loadMovimientos(idAnticipo, ELL.Anticipo.Movimiento.TipoMovimiento.Entregado, False)
                    If (lMovs IsNot Nothing AndAlso lMovs.Count > 0) Then
                        For Each oMov As ELL.Anticipo.Movimiento In lMovs
                            DeleteMovimiento(oMov.Id, myCon)
                        Next
                    End If
                End If
                If (con Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (con Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al cambiar el estado del anticipo", ex)
            Finally
                If (con Is Nothing AndAlso (myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed)) Then
                    myCon.Close()
                    myCon.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Añade una moneda a las posibles seleccionables en un anticipo
        ''' </summary>
        ''' <param name="idMon">Id de la moneda</param>
        ''' <param name="idPlanta">Id de la planta</param>  
        ''' <param name="myConn">Conexion por si viene de una transaccion</param>
        Public Sub AddMoneda(ByVal idMon As Integer, ByVal idPlanta As Integer, Optional ByVal myConn As OracleConnection = Nothing)
            Try
                Dim query As String = "INSERT INTO ANTICIPO_MONEDAS(ID_MONEDA,ID_PLANTA) VALUES (:ID_MONEDA,:ID_PLANTA)"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, idMon, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (myConn Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, myConn, lParameters.ToArray)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al asociar una moneda a un anticipo", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Elimina la moneda para que no se pueda pedir en los anticipos
        ''' </summary>
        ''' <param name="idMon">Id de la moneda</param>           
        ''' <param name="idPlanta">Id de la planta</param>     
        Sub DeleteMoneda(ByVal idMon As Integer, ByVal idPlanta As Integer)
            Try
                Dim query As String = "DELETE FROM ANTICIPO_MONEDAS WHERE ID_MONEDA=:ID_MONEDA AND ID_PLANTA=:ID_PLANTA"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, idMon, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParameters.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al desasociar una moneda de un anticipo", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Elimina las monedas que se podran solicitar en una anticipo de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>  
        ''' <param name="myConn">Conexion por si viene de una transaccion</param>
        Public Sub DeleteMonedas(ByVal idPlanta As Integer, Optional ByVal myConn As OracleConnection = Nothing)
            Try
                Dim query As String = "DELETE FROM ANTICIPO_MONEDAS WHERE ID_PLANTA=:ID_PLANTA"
                Dim parameter As New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
                If (myConn Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, myConn, parameter)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar las monedas asociadas a un anticipo", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace