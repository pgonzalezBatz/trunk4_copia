Namespace DAL

    Public Class VisasDAL

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
        ''' Obtiene la informacion de una visa
        ''' </summary>
        ''' <param name="pVisa">Objeto visa</param>        
        ''' <returns></returns>        
        Public Function loadInfo(ByVal pVisa As ELL.Visa) As ELL.Visa
            Try
                Dim query As String = "SELECT ID,NUM_TARJETA,ID_USER,FECHA_ENTREGA,OBSOLETA,ID_PLANTA FROM VISAS "
                Dim where As String = String.Empty
                Dim parametros As New List(Of OracleParameter)
                If (pVisa.Id <> Integer.MinValue) Then
                    parametros.Add(New OracleParameter("ID", OracleDbType.Int32, pVisa.Id, ParameterDirection.Input))
                    where = "ID=:ID"
                End If
                If (pVisa.NumTarjeta <> String.Empty) Then
                    parametros.Add(New OracleParameter("NUM_TARJETA", OracleDbType.Varchar2, pVisa.NumTarjeta, ParameterDirection.Input))
                    If (where <> String.Empty) Then where &= " AND "
                    where &= "NUM_TARJETA=:NUM_TARJETA"
                End If

                If (where <> String.Empty) Then query &= "WHERE " & where

                Dim lVisas As List(Of ELL.Visa) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Visa)(Function(r As OracleDataReader) _
                 New ELL.Visa With {.Id = CInt(r(0)), .NumTarjeta = r(1), .Propietario = New SabLib.ELL.Usuario With {.Id = CInt(r(2))}, .FechaEntrega = CDate(r(3)), .Obsoleta = CInt(r(4)), .IdPlanta = CInt(r(5))}, query, cn, parametros.ToArray)

                Dim oVisa As ELL.Visa = Nothing
                If (lVisas IsNot Nothing AndAlso lVisas.Count > 0) Then oVisa = lVisas.Item(0)
                Return oVisa
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la visa", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de una visa excepcion
        ''' </summary>
        ''' <param name="pVisa">Objeto visa</param>
        ''' <returns></returns>        
        Function loadInfoExcepcion(ByVal pVisa As ELL.Visa) As ELL.Visa
            Try
                Dim query As String = "SELECT ID,NUM_TARJETA,ID_PLANTA FROM VISAS_EXCEPCION "
                Dim where As String = String.Empty
                Dim parametros As New List(Of OracleParameter)
                If (pVisa.Id <> Integer.MinValue) Then
                    parametros.Add(New OracleParameter("ID", OracleDbType.Int32, pVisa.Id, ParameterDirection.Input))
                    where = "ID=:ID"
                End If
                If (pVisa.NumTarjeta <> String.Empty) Then
                    parametros.Add(New OracleParameter("NUM_TARJETA", OracleDbType.Varchar2, pVisa.NumTarjeta, ParameterDirection.Input))
                    If (where <> String.Empty) Then where &= " AND "
                    where &= "NUM_TARJETA=:NUM_TARJETA"
                End If

                If (where <> String.Empty) Then query &= "WHERE " & where

                Dim lVisas As List(Of ELL.Visa) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Visa)(Function(r As OracleDataReader) _
                 New ELL.Visa With {.Id = CInt(r(0)), .NumTarjeta = r(1), .IdPlanta = CInt(r(2))}, query, cn, parametros.ToArray)

                Dim oVisa As ELL.Visa = Nothing
                If (lVisas IsNot Nothing AndAlso lVisas.Count > 0) Then oVisa = lVisas.Item(0)
                Return oVisa
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la visa excepcion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de visas
        ''' </summary>
        ''' <param name="oVisa">Objeto visa con las condiciones</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bIncluirObsoletos">Indica si se incluiran los obsoletos</param>
        ''' <returns></returns>        
        Public Function loadList(ByVal oVisa As ELL.Visa, ByVal idPlanta As Integer, ByVal bIncluirObsoletos As Boolean) As List(Of ELL.Visa)
            Try
                Dim query As String = "SELECT V.ID,V.NUM_TARJETA,V.ID_USER,V.FECHA_ENTREGA,V.OBSOLETA,V.ID_PLANTA,U.NOMBRE,U.APELLIDO1,U.APELLIDO2,U.CODPERSONA FROM VISAS V INNER JOIN SAB.USUARIOS U ON V.ID_USER=U.ID WHERE V.ID_PLANTA=:ID_PLANTA"
                Dim where As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (oVisa.Propietario IsNot Nothing AndAlso oVisa.Propietario.Id <> Integer.MinValue) Then
                    lParametros.Add(New OracleParameter("ID_PROPIETARIO", OracleDbType.Int32, oVisa.Propietario.Id, ParameterDirection.Input))
                    where &= " AND V.ID_USER=:ID_PROPIETARIO"
                End If
                If (oVisa.NumTarjeta <> String.Empty) Then
                    lParametros.Add(New OracleParameter("NUM_TARJETA", OracleDbType.Varchar2, oVisa.NumTarjeta, ParameterDirection.Input))
                    where &= " AND NUM_TARJETA LIKE '%' || :NUM_TARJETA || '%'"
                End If
                If (Not bIncluirObsoletos) Then where &= " AND V.OBSOLETA=0"
                If (where <> String.Empty) Then query &= where

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Visa)(Function(r As OracleDataReader) _
                New ELL.Visa With {.Id = CInt(r(0)), .NumTarjeta = r(1), .Propietario = New SabLib.ELL.Usuario With {.Id = CInt(r(2)), .Nombre = r(6), .Apellido1 = r(7), .Apellido2 = SabLib.BLL.Utils.stringNull(r(8)), .CodPersona = SabLib.BLL.Utils.integerNull(r(9))},
                                   .FechaEntrega = CDate(r(3)), .Obsoleta = CInt(r(4)), .IdPlanta = CInt(r(5))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de visas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de visas de excepcion
        ''' </summary>
        ''' <param name="oVisa">Objeto visa con las condiciones</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function loadListExcepcion(ByVal oVisa As ELL.Visa, ByVal idPlanta As Integer) As List(Of ELL.Visa)
            Try
                Dim query As String = "SELECT ID,NUM_TARJETA,ID_PLANTA,NOMBRE_TARJETA FROM VISAS_EXCEPCION WHERE ID_PLANTA=:ID_PLANTA"
                Dim where As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oVisa.IdPlanta, ParameterDirection.Input))
                If (oVisa.NumTarjeta <> String.Empty) Then
                    lParametros.Add(New OracleParameter("NUM_TARJETA", OracleDbType.Varchar2, oVisa.NumTarjeta, ParameterDirection.Input))
                    where &= " AND NUM_TARJETA LIKE '%' || :NUM_TARJETA || '%'"
                End If
                If (where <> String.Empty) Then query &= where

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Visa)(Function(r As OracleDataReader) _
                New ELL.Visa With {.Id = CInt(r(0)), .NumTarjeta = r(1), .IdPlanta = CInt(r(2)), .Propietario = New SabLib.ELL.Usuario With {.Nombre = SabLib.BLL.Utils.stringNull(r(3))}}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de visas excepcion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion del movimiento movimiento de visa solicitado
        ''' </summary>
        ''' <param name="id">Id del movimiento</param>
        ''' <returns></returns>        
        Function loadMovimiento(ByVal id As Integer) As ELL.Visa.Movimiento
            Try
                Dim xbatBLL As New BLL.XbatBLL
                Dim query As String = "SELECT TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,ID_MONEDA,IMPORTE,ID_VIAJE,ID_PLANTA,LOCALIDAD,ID_RESPONSABLE,ID_USUARIO,ID,ESTADO,OBSERVACIONES,ID_HGLIBRE,ID_IMPORTACION,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO,TIPO FROM MOVIMIENTOS_VISA WHERE ID=:ID"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Visa.Movimiento)(Function(r As OracleDataReader) _
                New ELL.Visa.Movimiento With {.Id = CInt(r("ID")), .IdUsuario = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdResponsable = SabLib.BLL.Utils.integerNull(r("ID_RESPONSABLE")), .NumTarjeta = r("TARJETA"), .Sector = SabLib.BLL.Utils.stringNull(r("SECTOR")), .Establecimiento = SabLib.BLL.Utils.stringNull(r("ESTABLECIMIENTO")),
                                              .Fecha = CDate(r("FECHA")), .Moneda = xbatBLL.GetMoneda(CInt(r("ID_MONEDA"))), .ImporteEuros = CDec(r("iMPORTE")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .IdPlanta = CInt(r("ID_PLANTA")),
                                              .Localidad = SabLib.BLL.Utils.stringNull(r("LOCALIDAD")), .Estado = CInt(r("ESTADO")), .Comentarios = SabLib.BLL.Utils.stringNull(r("OBSERVACIONES")), .IdHoja = SabLib.BLL.Utils.integerNull(r("ID_HGLIBRE")),
                                              .IdImportacion = SabLib.BLL.Utils.integerNull(r("ID_IMPORTACION")), .MonedaGasto = xbatBLL.GetMoneda(CInt(r("ID_MONEDA_GASTO"))), .ImporteMonedaGasto = SabLib.BLL.Utils.decimalNull(r("IMPORTE_MONEDA_GASTO")),
                                              .Tipo = CInt(r("TIPO"))}, query, cn, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault
            Catch ex As Exception
                Throw New BatzException("Error al obtener la informacion del movimiento de visa", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de un usuario. Se pueden obtener tambien los de un viaje
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idViaje">Id del viaje.Si viene nothing, se buscaran los gastos de visa que no tengan idViaje</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="fechaInicio">Fecha de inicio a partir de la cual se va a buscar</param>
        ''' <param name="fechaFin">Fecha de fin a partir de la cual se va a buscar</param> 
        ''' <param name="bUserYPupilos">Indica si se se obtendran solo los gastos de visa del usuario o de los pupilos</param>
        ''' <param name="bSinValidar">Indica si se obtendran solo los de sin validar o todos</param>
        ''' <param name="bSinJustificar">Indica si se obtendran solo los que esten sin justificar</param>
        ''' <param name="idHojaLibre">Obtiene los movimientos de una hoja de gastos sin viaje</param>
        ''' <param name="tipoMov">Por defecto, solo muestra las de tipo de gasto. Las cuotas no las muestra</param>
        ''' <returns></returns>        
        Function loadMovimientos(ByVal idUser As Integer, ByVal idViaje As Nullable(Of Integer), ByVal idPlanta As Integer, ByVal fechaInicio As Date, ByVal fechaFin As Date, ByVal bUserYPupilos As Boolean, ByVal bSinValidar As Boolean, ByVal bSinJustificar As Boolean, ByVal idHojaLibre As Integer, ByVal tipoMov As Integer) As List(Of ELL.Visa.Movimiento)
            Try
                Dim xbatBLL As New BLL.XbatBLL
                Dim where As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT DISTINCT CONCAT(CONCAT(CONCAT(CONCAT(U.NOMBRE,' '),U.APELLIDO1),' '),U.APELLIDO2) AS NOMBRE_USUARIO,U.ID AS ID_SAB,MV.ID_RESPONSABLE,MV.TARJETA,MV.SECTOR,MV.ESTABLECIMIENTO,MV.FECHA,MV.ID_MONEDA,MV.IMPORTE,MV.ID_VIAJE,MV.ID_PLANTA,MV.LOCALIDAD,MV.ESTADO,MV.ID,NVL(MV.ID_HGLIBRE,HG.ID) AS ID_HOJA,MV.OBSERVACIONES,MV.ID_IMPORTACION,MV.ID_MONEDA_GASTO,MV.IMPORTE_MONEDA_GASTO,MV.TIPO " _
                                      & "FROM MOVIMIENTOS_VISA MV INNER JOIN VISAS V ON MV.TARJETA=V.NUM_TARJETA " _
                                      & "INNER JOIN SAB.USUARIOS U ON V.ID_USER=U.ID "
                'Con este if, solventamos los casos en los que hay un gasto de visa justo en el medio de dos viajes-> V1 del 20 al 25 y V2 del 25 al 30 y el gasto es el 25
                'Para las hojas de gasto libres este caso no se podra dar porque las fechas no se pueden colapsar al contrario que en los viajes
                If (idViaje.HasValue And idViaje <> Integer.MinValue) Then
                    query &= "LEFT JOIN HOJA_GASTOS HG ON (MV.ID_USUARIO=HG.ID_USER And MV.ID_VIAJE=HG.ID_VIAJE)"
                Else
                    query &= "LEFT JOIN HOJA_GASTOS HG ON (MV.ID_USUARIO=HG.ID_USER And ((MV.ID_HGLIBRE IS NOT NULL AND MV.ID_HGLIBRE=HG.ID) OR (MV.ID_VIAJE IS NULL AND MV.ID_HGLIBRE IS NULL AND MV.FECHA BETWEEN HG.FECHA_DESDE And HG.FECHA_HASTA)) )"
                End If
                where = "V.ID_PLANTA=:ID_PLANTA"
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (idUser <> Integer.MinValue) Then
                    where &= If(where <> String.Empty, " AND ", String.Empty)
                    If (bUserYPupilos) Then
                        where &= " (MV.ID_USUARIO=:ID_USER OR MV.ID_RESPONSABLE=:ID_USER)"
                    Else
                        where &= " MV.ID_USUARIO=:ID_USER"
                    End If
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                End If
                If (idViaje.HasValue) Then
                    If (idViaje.Value <> Integer.MinValue) Then
                        where &= If(where <> String.Empty, " AND ", String.Empty)
                        where &= "MV.ID_VIAJE=:ID_VIAJE"
                        lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                    End If
                Else
                    where &= If(where <> String.Empty, " AND ", String.Empty)
                    where &= "MV.ID_VIAJE IS NULL"
                End If
                If (bSinValidar) Then
                    where &= If(where <> String.Empty, " AND ", String.Empty)
                    where &= "(MV.ESTADO=:ESTADO_CARG OR MV.ESTADO=:ESTADO_JUST)"
                    lParametros.Add(New OracleParameter("ESTADO_CARG", OracleDbType.Int32, ELL.Visa.Movimiento.eEstado.Cargado, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ESTADO_JUST", OracleDbType.Int32, ELL.Visa.Movimiento.eEstado.Justificado, ParameterDirection.Input))
                End If
                If (bSinJustificar) Then
                    where &= If(where <> String.Empty, " AND ", String.Empty)
                    where &= "(MV.ESTADO=:ESTADO_CARG)"
                    lParametros.Add(New OracleParameter("ESTADO_CARG", OracleDbType.Int32, ELL.Visa.Movimiento.eEstado.Cargado, ParameterDirection.Input))
                End If
                If (fechaInicio <> Date.MinValue AndAlso idHojaLibre > 0) Then
                    where &= If(where <> String.Empty, " AND ", String.Empty)
                    where &= "((MV.ID_HGLIBRE IS NULL AND MV.ID_VIAJE IS NULL AND MV.FECHA BETWEEN :FECHA_INICIO AND :FECHA_FIN) OR (MV.ID_HGLIBRE IS NOT NULL AND NVL(MV.ID_HGLIBRE,HG.ID)=:ID_HGLIBRE))"
                    lParametros.Add(New OracleParameter("FECHA_INICIO", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_FIN", OracleDbType.Date, fechaFin, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_HGLIBRE", OracleDbType.Int32, idHojaLibre, ParameterDirection.Input))
                ElseIf (fechaInicio <> Date.MinValue) Then
                    where &= If(where <> String.Empty, " AND ", String.Empty)
                    where &= "MV.FECHA BETWEEN :FECHA_INICIO AND :FECHA_FIN"
                    lParametros.Add(New OracleParameter("FECHA_INICIO", OracleDbType.Date, fechaInicio, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_FIN", OracleDbType.Date, fechaFin, ParameterDirection.Input))
                ElseIf (idHojaLibre > 0 And idViaje <= 0) Then
                    where &= If(where <> String.Empty, " AND ", String.Empty)
                    where &= "NVL(MV.ID_HGLIBRE,HG.ID)=:ID_HGLIBRE"
                    lParametros.Add(New OracleParameter("ID_HGLIBRE", OracleDbType.Int32, idHojaLibre, ParameterDirection.Input))
                End If
                If (tipoMov <> -1) Then
                    where &= If(where <> String.Empty, " AND ", String.Empty)
                    where &= "MV.TIPO=:TIPO"
                    lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, tipoMov, ParameterDirection.Input))
                End If
                If (where <> String.Empty) Then query &= " WHERE " & where
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Visa.Movimiento)(Function(r As OracleDataReader) _
                New ELL.Visa.Movimiento With {.Id = CInt(r("Id")), .NombreUsuario = r("NOMBRE_USUARIO"), .IdUsuario = r("ID_SAB"), .IdResponsable = SabLib.BLL.Utils.integerNull(r("ID_RESPONSABLE")), .NumTarjeta = r("TARJETA"), .Sector = SabLib.BLL.Utils.stringNull(r("SECTOR")), .Establecimiento = SabLib.BLL.Utils.stringNull(r("ESTABLECIMIENTO")),
                                              .Fecha = CDate(r("FECHA")), .Moneda = xbatBLL.GetMoneda(CInt(r("ID_MONEDA"))), .ImporteEuros = CDec(r("iMPORTE")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .IdHoja = SabLib.BLL.Utils.integerNull(r("ID_HOJA")), .IdPlanta = CInt(r("ID_PLANTA")),
                                              .Localidad = SabLib.BLL.Utils.stringNull(r("LOCALIDAD")), .Estado = CInt(r("ESTADO")), .Comentarios = SabLib.BLL.Utils.stringNull(r("OBSERVACIONES")), .IdImportacion = SabLib.BLL.Utils.integerNull(r("ID_IMPORTACION")), .MonedaGasto = xbatBLL.GetMoneda(CInt(r("ID_MONEDA_GASTO"))), .ImporteMonedaGasto = SabLib.BLL.Utils.decimalNull(r("IMPORTE_MONEDA_GASTO")),
                                              .Tipo = CInt(r("TIPO"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener los movimientos de visas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de visa solicitados
        ''' </summary>
        ''' <param name="mov">Objeto movimiento</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="tipoMov">Por defecto, solo muestra las de tipo de gasto. Las cuotas no las muestra</param>
        ''' <returns></returns>        
        Function loadMovimientos(ByVal mov As ELL.Visa.Movimiento, ByVal idPlanta As Integer, ByVal tipoMov As Integer) As List(Of ELL.Visa.Movimiento)
            Try
                Dim xbatBLL As New BLL.XbatBLL
                Dim query As String = "SELECT TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,ID_MONEDA,IMPORTE,ID_VIAJE,ID_PLANTA,LOCALIDAD,ID_RESPONSABLE,ID_USUARIO,ID,ESTADO,USUARIO,OBSERVACIONES,ID_IMPORTACION,ID_HGLIBRE,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO,TIPO FROM MOVIMIENTOS_VISA WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (mov.Sector <> String.Empty) Then
                    query &= " AND SECTOR=:SECTOR"
                    lParametros.Add(New OracleParameter("SECTOR", OracleDbType.NVarchar2, mov.Sector, ParameterDirection.Input))
                End If
                If (mov.IdImportacion <> Integer.MinValue) Then
                    query &= " AND ID_IMPORTACION=:ID_IMPORTACION"
                    lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, mov.IdImportacion, ParameterDirection.Input))
                End If
                If (tipoMov <> -1) Then
                    query &= " AND TIPO=:TIPO"
                    lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, tipoMov, ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Visa.Movimiento)(Function(r As OracleDataReader) _
                New ELL.Visa.Movimiento With {.Id = CInt(r("ID")), .IdUsuario = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")), .IdResponsable = SabLib.BLL.Utils.integerNull(r("ID_RESPONSABLE")), .NumTarjeta = r("TARJETA"), .Sector = SabLib.BLL.Utils.stringNull(r("SECTOR")), .Establecimiento = SabLib.BLL.Utils.stringNull(r("ESTABLECIMIENTO")),
                                              .Fecha = CDate(r("FECHA")), .Moneda = xbatBLL.GetMoneda(CInt(r("ID_MONEDA"))), .ImporteEuros = CDec(r("iMPORTE")), .IdViaje = SabLib.BLL.Utils.integerNull(r("ID_VIAJE")), .IdPlanta = CInt(r("ID_PLANTA")),
                                              .Localidad = SabLib.BLL.Utils.stringNull(r("LOCALIDAD")), .Estado = CInt(r("ESTADO")), .NombreUsuario = SabLib.BLL.Utils.stringNull(r("USUARIO")), .Comentarios = SabLib.BLL.Utils.stringNull(r("OBSERVACIONES")), .Tipo = CInt(r("TIPO")),
                                              .IdHoja = SabLib.BLL.Utils.integerNull(r("ID_HGLIBRE")), .IdImportacion = SabLib.BLL.Utils.integerNull(r("ID_IMPORTACION")), .MonedaGasto = xbatBLL.GetMoneda(CInt(r("ID_MONEDA_GASTO"))), .ImporteMonedaGasto = SabLib.BLL.Utils.decimalNull(r("IMPORTE_MONEDA_GASTO"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener los movimientos de visas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de visa de excepcion solicitados
        ''' </summary>
        ''' <param name="mov">Objeto movimiento</param>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <returns></returns>        
        Function loadMovimientosExcepcion(ByVal mov As ELL.Visa.Movimiento, ByVal idPlanta As Integer) As List(Of ELL.Visa.Movimiento)
            Try
                Dim xbatBLL As New BLL.XbatBLL
                'Dim query As String = "SELECT ID,USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,MONEDA,IMPORTE,ID_PLANTA,FECHA_INSERCION,LOCALIDAD,ID_MONEDA,ID_IMPORTACION,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO,CUENTA,LANTEGI FROM MOVIMIENTOS_VISA_EXCEP WHERE ID_PLANTA=:ID_PLANTA"
                Dim query As String = "SELECT ID,USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,MONEDA,IMPORTE,ID_PLANTA,FECHA_INSERCION,LOCALIDAD,ID_MONEDA,ID_IMPORTACION,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO FROM MOVIMIENTOS_VISA_EXCEP WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (mov.Sector <> String.Empty) Then
                    query &= " AND SECTOR=:SECTOR"
                    lParametros.Add(New OracleParameter("SECTOR", OracleDbType.NVarchar2, mov.Sector, ParameterDirection.Input))
                End If
                If (mov.IdImportacion <> Integer.MinValue) Then
                    query &= " AND ID_IMPORTACION=:ID_IMPORTACION"
                    lParametros.Add(New OracleParameter("ID_IMPORTACION", OracleDbType.Int32, mov.IdImportacion, ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Visa.Movimiento)(Function(r As OracleDataReader) _
                New ELL.Visa.Movimiento With {.Id = CInt(r("ID")), .NumTarjeta = r("TARJETA"), .Sector = SabLib.BLL.Utils.stringNull(r("SECTOR")), .Establecimiento = SabLib.BLL.Utils.stringNull(r("ESTABLECIMIENTO")),
                                              .Fecha = CDate(r("FECHA")), .Moneda = xbatBLL.GetMoneda(CInt(r("ID_MONEDA"))), .ImporteEuros = CDec(r("iMPORTE")), .IdPlanta = CInt(r("ID_PLANTA")),
                                              .Localidad = SabLib.BLL.Utils.stringNull(r("LOCALIDAD")), .NombreUsuario = SabLib.BLL.Utils.stringNull(r("USUARIO")), .IdImportacion = SabLib.BLL.Utils.integerNull(r("ID_IMPORTACION")), .MonedaGasto = xbatBLL.GetMoneda(CInt(r("ID_MONEDA_GASTO"))),
                                              .ImporteMonedaGasto = SabLib.BLL.Utils.decimalNull(r("IMPORTE_MONEDA_GASTO"))}, query, cn, lParametros.ToArray)
                'Cuenta = Sablib.BLL.Utils.integerNull(r("CUENTA")), .Lantegi = r("LANTEGI")
            Catch ex As Exception
                Throw New BatzException("Error al obtener los movimientos de visas de excepcion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Comprueba si ya se ha cargado el fichero de visas del mes y año indicados
        ''' </summary>
        ''' <param name="month">Mes a consultar</param>
        ''' <param name="year">Año a consultar</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function FicheroVisasCargado(ByVal month As Integer, ByVal year As Integer, ByVal idPlanta As Integer) As Boolean
            Dim query As String = "SELECT COUNT(ID) FROM IMPORTACION_DOCS WHERE ANNO=:ANNO AND MES=:MES AND ID_PLANTA=:ID_PLANTA AND TIPO=1" 'Tipo=1=>Visas
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, year, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, month, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Return (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) = 1)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica la visa
        ''' </summary>
        ''' <param name="oVisa">Objeto con la informacion</param>        
        ''' <param name="myConn">Si no es nothing, vendra de una transaccion</param>
        Public Sub Save(ByVal oVisa As ELL.Visa, ByVal myConn As OracleConnection)
            Try
                Dim query As String = String.Empty
                Dim lParametros As New List(Of OracleParameter)
                If (oVisa.Id = Integer.MinValue) Then 'Insert
                    lParametros.Add(New OracleParameter("NUM_TARJETA", OracleDbType.Varchar2, oVisa.NumTarjeta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oVisa.Propietario.Id, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("FECHA_ENTREGA", OracleDbType.Date, oVisa.FechaEntrega, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oVisa.IdPlanta, ParameterDirection.Input))
                    query = "INSERT INTO VISAS(NUM_TARJETA,ID_USER,FECHA_ENTREGA,OBSOLETA,ID_PLANTA) VALUES(:NUM_TARJETA,:ID_USER,:FECHA_ENTREGA,0,:ID_PLANTA)"
                Else 'update                    
                    lParametros.Add(New OracleParameter("OBSOLETA", OracleDbType.Int32, oVisa.Obsoleta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oVisa.Id, ParameterDirection.Input))
                    query = "UPDATE VISAS SET OBSOLETA=:OBSOLETA WHERE ID=:ID"
                End If
                If (myConn Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, myConn, lParametros.ToArray)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la informacion", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Marca como obsoleto la visa
        ''' </summary>
        ''' <param name="id">Id del objeto</param>        
        Public Sub Delete(ByVal id As Integer)
            Try
                Dim query As String = "UPDATE VISAS SET OBSOLETO=1 WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la visa", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Borra la visa excepcion
        ''' </summary>
        ''' <param name="id">Id de la tarjeta</param>        
        Public Sub DeleteExcepcion(ByVal id As String)
            Try
                Dim query As String = "DELETE FROM VISAS_EXCEPCION WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Memcached.OracleDirectAccess.NoQuery(query, cn, parameter)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la visa excepcion", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Cambia el estado de los movimientos de visa especificados
        ''' </summary>
        ''' <param name="lMovVisa">Lista de movimientos. A cada uno se le asignara el estado que lleve informado</param>
        Public Sub CambiarEstadoMovimientos(ByVal lMovVisa As List(Of ELL.Visa.Movimiento), ByVal myConnection As OracleConnection)
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim query As String
            Try
                If (myConnection Is Nothing) Then
                    myConn = New OracleConnection(cn)
                    myConn.Open()
                    transact = myConn.BeginTransaction()
                Else
                    myConn = myConnection
                End If

                Dim lParametros As List(Of OracleParameter)

                For Each mov As ELL.Visa.Movimiento In lMovVisa
                    lParametros = New List(Of OracleParameter)
                    query = "UPDATE MOVIMIENTOS_VISA SET ESTADO=:ESTADO"
                    If (mov.Comentarios <> String.Empty) Then
                        query &= ",OBSERVACIONES=:OBSERVACIONES"
                        lParametros.Add(New OracleParameter("OBSERVACIONES", OracleDbType.Varchar2, mov.Comentarios, ParameterDirection.Input))
                    End If
                    query &= " WHERE ID=:ID"

                    lParametros.Add(New OracleParameter("ESTADO", OracleDbType.Int32, mov.Estado, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, mov.Id, ParameterDirection.Input))

                    Memcached.OracleDirectAccess.NoQuery(query, myConn, lParametros.ToArray)
                Next

                If (myConnection Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (myConnection Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al realizar la validacion de los movimientos de visa", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Añade las visas a la tabla de excepciones y se elimina el movimiento de la tabla TMP_MOVIMIENTOS_VISAS
        ''' </summary>
        ''' <param name="lVisas">Lista de visas</param>        
        Public Sub AddVisasException(ByVal lVisas As List(Of Object))
            Dim myConn As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim query As String
            Try
                myConn = New OracleConnection(cn)
                myConn.Open()
                transact = myConn.BeginTransaction()
                Dim lParametros As List(Of OracleParameter)

                For Each visa In lVisas
                    lParametros = New List(Of OracleParameter)
                    query = "INSERT INTO VISAS_EXCEPCION(NUM_TARJETA,ID_PLANTA,NOMBRE_TARJETA) VALUES (:NUM_TARJETA,:ID_PLANTA,:NOMBRE_TARJETA)"
                    lParametros.Add(New OracleParameter("NUM_TARJETA", OracleDbType.NVarchar2, visa.NumTarjeta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, visa.IdPlanta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("NOMBRE_TARJETA", OracleDbType.NVarchar2, visa.NombreTarjeta, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConn, lParametros.ToArray)

                    lParametros = New List(Of OracleParameter)
                    query = "INSERT INTO TMP_MOVIMIENTOS_VISA_EXCEP(USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,MONEDA,IMPORTE,ID_PLANTA,FECHA_INSERCION,LOCALIDAD,ID_MONEDA,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO) " _
                          & "SELECT USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,MONEDA,IMPORTE,ID_PLANTA,FECHA_INSERCION,LOCALIDAD,ID_MONEDA,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO FROM TMP_MOVIMIENTOS_VISA WHERE ID=:ID"
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, visa.IdMovimiento, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConn, lParametros.ToArray)

                    lParametros = New List(Of OracleParameter)
                    query = "DELETE FROM TMP_MOVIMIENTOS_VISA WHERE ID=:ID"
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, visa.IdMovimiento, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConn, lParametros.ToArray)
                Next

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al realizar la insercion de visas de excepcion", ex)
            Finally
                If (myConn IsNot Nothing AndAlso myConn.State <> ConnectionState.Closed) Then
                    myConn.Close()
                    myConn.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Actualiza los datos del movimiento de visa
        ''' </summary>
        ''' <param name="oMov">Informacion del objeto</param>
        ''' <param name="myConn">Conexion por si viene de una transaccion</param>
        Sub UpdateMovimiento(ByVal oMov As ELL.Visa.Movimiento, Optional ByVal myConn As OracleConnection = Nothing)
            Dim query As String
            Try
                Dim lParametros As New List(Of OracleParameter)
                query = "UPDATE MOVIMIENTOS_VISA SET SECTOR=:SECTOR WHERE ID=:ID"
                lParametros.Add(New OracleParameter("SECTOR", OracleDbType.NVarchar2, oMov.Sector, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oMov.Id, ParameterDirection.Input))

                If (myConn Is Nothing) Then
                    Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                Else
                    Memcached.OracleDirectAccess.NoQuery(query, myConn, lParametros.ToArray)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al actualizar la informacion del movimiento (" & oMov.Id & ")", ex)
            End Try
        End Sub

#End Region

#Region "Importacion de visas"

        ''' <summary>
        ''' Obtiene los movimientos de visa temporales  menos los movimientos de las visas excepcion
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta a recuperar</param>
        ''' <param name="idUser">Id del usuario a filtrar. Puede ser integer.minValue</param>
        ''' <returns></returns>        
        Function loadVisasTmp(ByVal idPlanta As Integer, ByVal idUser As Integer) As List(Of String())
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,MONEDA,IMPORTE,ID_VIAJE,ID_PLANTA,FECHA_INSERCION,LOCALIDAD,ID_MONEDA,ESTADO,ID_RESPONSABLE,ID_USUARIO,ID,ASIGNAR_VISA,ID_HGLIBRE,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO,TIPO,ID_DEPARTAMENTO FROM TMP_MOVIMIENTOS_VISA TMV WHERE TMV.ID_PLANTA=:ID_PLANTA AND TMV.TARJETA NOT IN (SELECT NUM_TARJETA FROM VISAS_EXCEPCION VE WHERE VE.NUM_TARJETA=TMV.TARJETA AND VE.ID_PLANTA=TMV.ID_PLANTA)"
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (idUser > 0) Then
                    query &= " AND TMV.ID_USUARIO=:ID_USUARIO"
                    lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUser, ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los movimientos de visa temporales", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de visa excepcion temporales
        ''' </summary>        
        ''' <param name="idPlanta">Id de la planta a recuperar</param>        
        ''' <returns></returns>        
        Function loadVisasExcepcionTmp(ByVal idPlanta As Integer) As List(Of String())
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "SELECT USUARIO,TARJETA,SECTOR,ESTABLECIMIENTO,FECHA,MONEDA,IMPORTE,ID_PLANTA,FECHA_INSERCION,LOCALIDAD,ID_MONEDA,ID_MONEDA_GASTO,IMPORTE_MONEDA_GASTO FROM TMP_MOVIMIENTOS_VISA_EXCEP TMV WHERE TMV.ID_PLANTA=:ID_PLANTA"
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los movimientos de visa de excepcion temporales", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los importes de visa de con o sin viajes del fichero para una planta en concreto de la tabla de temporal
        ''' </summary>        
        ''' <param name="IdPlanta">Planta</param>
        ''' <param name="tipoTotal">0:Con viaje,1:Sin viajes,2:Cuotas</param>
        ''' <returns></returns>        
        Function loadImporteTotalVisaConSinViajeTmp(ByVal IdPlanta As Integer, ByVal tipoTotal As Integer) As List(Of Object)
            Try
                Dim query As New Text.StringBuilder
                query.Append("SELECT SUM(TMV.IMPORTE)")
                query.AppendLine("FROM TMP_MOVIMIENTOS_VISA TMV ")
                query.AppendLine("WHERE TMV.ID_PLANTA=:ID_PLANTA ")
                Select Case tipoTotal
                    Case 0 : query.Append("AND ID_VIAJE IS NOT NULL AND TIPO=0 ")
                    Case 1 : query.Append("AND ID_VIAJE IS NULL AND TIPO=0 ")
                    Case 2 : query.Append("AND TIPO=1 ")
                End Select
                query.Append("AND TMV.TARJETA NOT IN (SELECT NUM_TARJETA FROM VISAS_EXCEPCION VE WHERE VE.NUM_TARJETA=TMV.TARJETA AND VE.ID_PLANTA=TMV.ID_PLANTA) ")
                Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) _
                 New With {.Importe = SabLib.BLL.Utils.decimalNull(r(0)), .IdMoneda = 90, .Tipo = tipoTotal}, query.ToString, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, IdPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el importe total de los movimimentos de visas temporales", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los importes de visa excepcion para una planta en concreto de la tabla de temporal
        ''' </summary>        
        ''' <param name="IdPlanta">Planta</param>        
        ''' <returns>Tipo 3: Visa excepcion</returns>        
        Function loadImporteTotalVisaExcepcionTmp(ByVal IdPlanta As Integer) As List(Of Object)
            Try
                Dim query As New Text.StringBuilder
                query.Append("SELECT SUM(TMV.IMPORTE)")
                query.AppendLine("FROM TMP_MOVIMIENTOS_VISA_EXCEP TMV ")
                query.AppendLine("WHERE TMV.ID_PLANTA=:ID_PLANTA ")
                Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(r As OracleDataReader) _
                 New With {.Importe = SabLib.BLL.Utils.decimalNull(r(0)), .IdMoneda = 90, .Tipo = 3}, query.ToString, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, IdPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el importe total de los movimimentos de visas excepcion temporales", ex)
            End Try
        End Function

        ''' <summary>
        ''' Borra los registros de la tabla temporal de visas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta de los registros a borrar</param>
        Public Sub DeleteMovVisasTmp(ByVal idPlanta As Integer)
            Try
                Dim query As String = "DELETE FROM TMP_MOVIMIENTOS_VISA WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                query = "DELETE FROM TMP_MOVIMIENTOS_VISA_EXCEP WHERE ID_PLANTA=:ID_PLANTA"
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar los registros de la tabla temporal de visas", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace