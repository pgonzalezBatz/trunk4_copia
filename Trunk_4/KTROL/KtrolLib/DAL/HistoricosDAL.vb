Imports Oracle.ManagedDataAccess.Client
Imports System.Configuration
Imports System.Globalization

Namespace DAL

    Public Class HistoricosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene los controles de un día
        ''' </summary>
        ''' <param name="codOp">Código de operación</param>
        ''' <param name="carac">Característica</param>
        ''' <param name="fechaDesde">Fecha desde del control</param>
        ''' <param name="fechaHasta">Fecha hasta del control</param>
        ''' <param name="listaVerificadores">Códigos de los verificador</param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerControles(ByVal codOp As String, ByVal carac As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal listaVerificadores As List(Of Integer)) As List(Of ELL.Historicos)
            Dim query As String
            Dim fecha_desde_String As String = fechaDesde + " 00:00:00"
            Dim fecha_desde As DateTime = DateTime.ParseExact(fecha_desde_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)
            Dim fecha_hasta_String As String = fechaHasta + " 23:59:59"
            Dim fecha_hasta As DateTime = DateTime.ParseExact(fecha_hasta_String, "dd/MM/yyyy HH:mm:ss", CultureInfo.InvariantCulture)

            Dim lParametros As New List(Of OracleParameter)
            If (String.IsNullOrEmpty(carac)) Then
                query = "SELECT DISTINCT c.ID as ID, c.COD_OPERACION as COD_OPERACION, c.ID_USUARIO as ID_USUARIO, c.ID_PLANTA as ID_PLANTA, c.FECHA as FECHA, TO_CHAR(FECHA, 'dd-MM-yyyy') as FECHA_SOLO, TO_CHAR(FECHA, 'hh24:mi:ss') as HORA, c.INFO_PIEZA as INFO_PIEZA, ce.VALIDACION as VALIDACION, ce.REPARACION as REPARACION, ce.CAMBIO_REFERENCIA as CAMBIO_REFERENCIA, ce.VALIDACION_USUARIO as VALIDACION_USUARIO, ce.COMENTARIO as COMENTARIO FROM CONTROLES c " &
                " LEFT JOIN CONTROLES_ERRORES ce ON c.ID=ce.ID_CONTROL "
                query += "WHERE FECHA>=:FECHA_DESDE AND FECHA<=:FECHA_HASTA "
                If (listaVerificadores.Count > 0) Then
                    query += " AND ce.VALIDACION=1 AND ce.VALIDACION_USUARIO IN ("
                    Dim sIn As String = String.Empty
                    For Each oInt As Integer In listaVerificadores
                        If (sIn <> String.Empty) Then sIn &= ","
                        query += oInt.ToString()
                    Next
                    query += ") "
                End If
                If (codOp <> Integer.MinValue) Then
                    query += " AND c.COD_OPERACION=:COD_OPERACION "
                    lParametros.Add(New OracleParameter("COD_OPERACION", OracleDbType.NVarchar2, 50, codOp, ParameterDirection.Input))
                End If
                query += " ORDER BY FECHA ASC"
                lParametros.Add(New OracleParameter("FECHA_DESDE", OracleDbType.Date, fecha_desde, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_HASTA", OracleDbType.Date, fecha_hasta, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Historicos)(Function(r As OracleDataReader) _
                New ELL.Historicos With {.Id = CInt(r("ID")), .CodOperacion = SabLib.BLL.Utils.integerNull(r("COD_OPERACION")), .IdUsuario = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")),
                .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("FECHA")),
                .FechaCorta = SabLib.BLL.Utils.stringNull(r("FECHA_SOLO")), .Hora = SabLib.BLL.Utils.stringNull(r("HORA")), .InfoPieza = SabLib.BLL.Utils.stringNull(r("INFO_PIEZA")),
                .Validacion = SabLib.BLL.Utils.booleanNull(r("VALIDACION")), .ValidacionUsuario = SabLib.BLL.Utils.integerNull(r("VALIDACION_USUARIO")),
                .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")), .Reparacion = SabLib.BLL.Utils.booleanNull(r("REPARACION")),
                .CambioReferencia = SabLib.BLL.Utils.booleanNull(r("CAMBIO_REFERENCIA"))}, query, CadenaConexion, lParametros.ToArray)
            Else
                'c.OPERARIO as OPERARIO, c.CALIDAD as CALIDAD, 
                query = "SELECT DISTINCT c.ID as ID, c.COD_OPERACION as COD_OPERACION, c.ID_USUARIO as ID_USUARIO, c.ID_PLANTA as ID_PLANTA, " &
                    "c.FECHA as FECHA, TO_CHAR(FECHA, 'dd-MM-yyyy') as FECHA_SOLO, TO_CHAR(FECHA, 'hh:mm:ss') as HORA, c.INFO_PIEZA as INFO_PIEZA, ce.VALIDACION as VALIDACION, " &
                    "ce.REPARACION as REPARACION, ce.CAMBIO_REFERENCIA as CAMBIO_REFERENCIA, ce.VALIDACION_USUARIO as VALIDACION_USUARIO, ce.COMENTARIO as COMENTARIO, " &
                    "cv.ID_REGISTRO as ID_REGISTRO, cv.POSICION as POSICION, cv.VALOR as VALOR " &
                    " FROM CONTROLES c " &
                    "LEFT JOIN CONTROLES_ERRORES ce ON c.ID = ce.ID_CONTROL " &
                    "INNER JOIN CONTROLES_VALORES cv on c.ID = cv.ID_CONTROL " &
                    "WHERE FECHA>=:FECHA_DESDE And FECHA<=:FECHA_HASTA AND cv.POSICION LIKE '%' || :POSICION || '%' "
                If (listaVerificadores.Count > 0) Then
                    query += " AND ce.VALIDACION=1 AND ce.VALIDACION_USUARIO IN ("
                    Dim sIn As String = String.Empty
                    For Each oInt As Integer In listaVerificadores
                        If (sIn <> String.Empty) Then sIn &= ","
                        query &= oInt.ToString()
                    Next
                    query += ") "
                End If
                If (codOp <> Integer.MinValue) Then
                    query += " AND c.COD_OPERACION=:COD_OPERACION"
                    lParametros.Add(New OracleParameter("COD_OPERACION", OracleDbType.Int32, codOp, ParameterDirection.Input))
                End If
                query += " ORDER BY FECHA ASC"
                lParametros.Add(New OracleParameter("FECHA_DESDE", OracleDbType.Date, fecha_desde, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_HASTA", OracleDbType.Date, fecha_hasta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("POSICION", OracleDbType.Varchar2, 10, carac, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Historicos)(Function(r As OracleDataReader) _
                New ELL.Historicos With {.Id = CInt(r("ID")), .CodOperacion = SabLib.BLL.Utils.stringNull(r("COD_OPERACION")), .IdUsuario = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")),
                .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("FECHA")),
                .FechaCorta = SabLib.BLL.Utils.stringNull(r("FECHA_SOLO")), .Hora = SabLib.BLL.Utils.stringNull(r("HORA")), .InfoPieza = SabLib.BLL.Utils.stringNull(r("INFO_PIEZA")),
                .Validacion = SabLib.BLL.Utils.booleanNull(r("VALIDACION")), .ValidacionUsuario = SabLib.BLL.Utils.integerNull(r("VALIDACION_USUARIO")),
                .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")), .Reparacion = SabLib.BLL.Utils.booleanNull(r("REPARACION")),
                .CambioReferencia = SabLib.BLL.Utils.booleanNull(r("CAMBIO_REFERENCIA")), .IdRegistro = SabLib.BLL.Utils.stringNull(r("ID_REGISTRO")),
                .Posicion = SabLib.BLL.Utils.stringNull(r("POSICION")), .Valor = SabLib.BLL.Utils.stringNull(r("VALOR"))}, query, CadenaConexion, lParametros.ToArray)
            End If
        End Function

        ''' <summary>
        ''' Obtiene los controles de un día
        ''' </summary>
        ''' <param name="codOp">Código de operación</param>
        ''' <param name="carac">Característica</param>
        ''' <param name="fechaDesde">Fecha desde del control</param>
        ''' <param name="fechaHasta">Fecha hasta del control</param>
        ''' <param name="listaVerificadores">Códigos de los verificador</param>
        ''' <param name="idControl">Id del control</param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function ObtenerControlesPantalla(ByVal codOp As String, ByVal carac As String, ByVal fechaDesde As String, ByVal fechaHasta As String, ByVal listaVerificadores As List(Of Integer), ByVal idControl As String) As List(Of ELL.Historicos)
            Dim query As String

            Dim lParametros As New List(Of OracleParameter)
            If (String.IsNullOrEmpty(carac)) Then
                query = "SELECT dat.* FROM ("
                query += "SELECT DISTINCT c.ID as ID, c.COD_OPERACION as COD_OPERACION, c.ID_USUARIO as ID_USUARIO, c.ID_PLANTA as ID_PLANTA, c.FECHA as FECHA, TO_CHAR(FECHA, 'dd-MM-yyyy') as FECHA_SOLO, TO_CHAR(FECHA, 'hh24:mi:ss') as HORA, c.INFO_PIEZA as INFO_PIEZA, ce.VALIDACION as VALIDACION, ce.REPARACION as REPARACION, ce.CAMBIO_REFERENCIA as CAMBIO_REFERENCIA, ce.VALIDACION_USUARIO as VALIDACION_USUARIO, ce.COMENTARIO as COMENTARIO FROM CONTROLES c " &
                " LEFT JOIN CONTROLES_ERRORES ce ON c.ID=ce.ID_CONTROL "
                query += "WHERE 1=1 "
                If (listaVerificadores.Count > 0) Then
                    query += " AND ce.VALIDACION=1 AND ce.VALIDACION_USUARIO IN ("
                    Dim sIn As String = String.Empty
                    For Each oInt As Integer In listaVerificadores
                        If (sIn <> String.Empty) Then sIn &= ","
                        query += oInt.ToString()
                    Next
                    query += ") "
                End If
                If Not (String.IsNullOrEmpty(codOp)) Then
                    query += " AND c.COD_OPERACION=:COD_OPERACION "
                    lParametros.Add(New OracleParameter("COD_OPERACION", OracleDbType.NVarchar2, 50, codOp, ParameterDirection.Input))
                End If
                If Not (String.IsNullOrEmpty(idControl)) Then
                    query += " AND c.ID=:ID_CONTROL "
                    lParametros.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input))
                End If
                If Not (String.IsNullOrEmpty(fechaDesde)) Then
                    query += " AND  TRUNC(FECHA) >= :FECHA_DESDE "
                    lParametros.Add(New OracleParameter("FECHA_DESDE", OracleDbType.Date, CDate(fechaDesde), ParameterDirection.Input))

                End If
                If Not (String.IsNullOrEmpty(fechaHasta)) Then
                    query += " AND TRUNC(FECHA) <= :FECHA_HASTA "
                    lParametros.Add(New OracleParameter("FECHA_HASTA", OracleDbType.Date, CDate(fechaHasta), ParameterDirection.Input))
                End If
                query += " ORDER BY ID DESC "
                query += ") dat "
                query += " WHERE ROWNUM <= 100 "
                query += " ORDER BY ID DESC"

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Historicos)(Function(r As OracleDataReader) _
                New ELL.Historicos With {.Id = CInt(r("ID")), .CodOperacion = SabLib.BLL.Utils.stringNull(r("COD_OPERACION")), .IdUsuario = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")),
                                        .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("FECHA")), .FechaCorta = SabLib.BLL.Utils.stringNull(r("FECHA_SOLO")),
                                        .Hora = SabLib.BLL.Utils.stringNull(r("HORA")), .InfoPieza = SabLib.BLL.Utils.stringNull(r("INFO_PIEZA")), .Validacion = SabLib.BLL.Utils.booleanNull(r("VALIDACION")),
                                        .ValidacionUsuario = SabLib.BLL.Utils.integerNull(r("VALIDACION_USUARIO")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")),
                                        .Reparacion = SabLib.BLL.Utils.booleanNull(r("REPARACION")), .CambioReferencia = SabLib.BLL.Utils.booleanNull(r("CAMBIO_REFERENCIA"))},
                                        query, CadenaConexion, lParametros.ToArray)
            Else
                query = "SELECT dat.* FROM ("
                query += "SELECT DISTINCT c.ID as ID, c.COD_OPERACION as COD_OPERACION, c.ID_USUARIO as ID_USUARIO, c.ID_PLANTA as ID_PLANTA, c.FECHA as FECHA, TO_CHAR(FECHA, 'dd-MM-yyyy') as FECHA_SOLO, TO_CHAR(FECHA, 'hh:mm:ss') as HORA, c.INFO_PIEZA as INFO_PIEZA, ce.VALIDACION as VALIDACION, ce.REPARACION as REPARACION, ce.CAMBIO_REFERENCIA as CAMBIO_REFERENCIA, ce.VALIDACION_USUARIO as VALIDACION_USUARIO, ce.COMENTARIO as COMENTARIO, cv.ID_REGISTRO as ID_REGISTRO, cv.POSICION as POSICION, cv.OK_NOK as OK_NOK, cv.VALOR as VALOR FROM CONTROLES c " &
                "LEFT JOIN CONTROLES_ERRORES ce ON c.ID = ce.ID_CONTROL " &
                "INNER JOIN CONTROLES_VALORES cv on c.ID = cv.ID_CONTROL " &
                "WHERE UPPER(cv.POSICION) LIKE '%' || :POSICION || '%' "
                If (listaVerificadores.Count > 0) Then
                    query += " AND ce.VALIDACION=1 AND ce.VALIDACION_USUARIO IN ("
                    Dim sIn As String = String.Empty
                    For Each oInt As Integer In listaVerificadores
                        If (sIn <> String.Empty) Then sIn &= ","
                        query += oInt.ToString()
                    Next
                    query += ") "
                End If
                If Not (String.IsNullOrEmpty(codOp)) Then
                    query += " AND c.COD_OPERACION=:COD_OPERACION "
                    lParametros.Add(New OracleParameter("COD_OPERACION", OracleDbType.NVarchar2, 50, codOp, ParameterDirection.Input))
                End If
                If Not (String.IsNullOrEmpty(idControl)) Then
                    query += " AND c.ID=:ID_CONTROL "
                    lParametros.Add(New OracleParameter("ID_CONTROL", OracleDbType.Int32, idControl, ParameterDirection.Input))
                End If
                If Not (String.IsNullOrEmpty(fechaDesde)) Then
                    query += " AND  TRUNC(FECHA) >= :FECHA_DESDE "
                    lParametros.Add(New OracleParameter("FECHA_DESDE", OracleDbType.Date, CDate(fechaDesde), ParameterDirection.Input))
                End If
                If Not (String.IsNullOrEmpty(fechaHasta)) Then
                    query += " AND TRUNC(FECHA) <= :FECHA_HASTA "
                    lParametros.Add(New OracleParameter("FECHA_HASTA", OracleDbType.Date, CDate(fechaHasta), ParameterDirection.Input))
                End If
                query += " ORDER BY ID DESC "
                query += ") dat "
                query += " WHERE ROWNUM <= 100 "
                query += " ORDER BY ID DESC"
                lParametros.Add(New OracleParameter("POSICION", OracleDbType.Varchar2, 10, carac, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Historicos)(Function(r As OracleDataReader) _
                New ELL.Historicos With {.Id = CInt(r("ID")), .CodOperacion = SabLib.BLL.Utils.stringNull(r("COD_OPERACION")), .IdUsuario = SabLib.BLL.Utils.integerNull(r("ID_USUARIO")),
                                        .IdPlanta = SabLib.BLL.Utils.integerNull(r("ID_PLANTA")), .Fecha = SabLib.BLL.Utils.dateTimeNull(r("FECHA")),
                                        .FechaCorta = SabLib.BLL.Utils.stringNull(r("FECHA_SOLO")), .Hora = SabLib.BLL.Utils.stringNull(r("HORA")), .InfoPieza = SabLib.BLL.Utils.stringNull(r("INFO_PIEZA")),
                                        .Validacion = SabLib.BLL.Utils.booleanNull(r("VALIDACION")), .ValidacionUsuario = SabLib.BLL.Utils.integerNull(r("VALIDACION_USUARIO")),
                                        .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")), .Reparacion = SabLib.BLL.Utils.booleanNull(r("REPARACION")),
                                        .CambioReferencia = SabLib.BLL.Utils.booleanNull(r("CAMBIO_REFERENCIA")), .IdRegistro = SabLib.BLL.Utils.stringNull(r("ID_REGISTRO")),
                                        .Posicion = SabLib.BLL.Utils.stringNull(r("POSICION")), .Valor = SabLib.BLL.Utils.stringNull(r("VALOR")), .OkNok = SabLib.BLL.Utils.stringNull(r("OK_NOK"))},
                                        query, CadenaConexion, lParametros.ToArray)
            End If
        End Function

#End Region

    End Class

End Namespace