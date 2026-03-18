Namespace DAL

    Public Class XbatDAL

#Region "Variables"

        Private cnBidaiak, cnXBAT As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            Dim statusBidaiak, statusXBAT As String
            statusBidaiak = "BIDAIAKTEST" : statusXBAT = "XBATTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                statusBidaiak = "BIDAIAKLIVE" : statusXBAT = "XBATLIVE"
            End If
            cnBidaiak = Configuration.ConfigurationManager.ConnectionStrings(statusBidaiak).ConnectionString
            cnXBAT = Configuration.ConfigurationManager.ConnectionStrings(statusXBAT).ConnectionString
        End Sub

#End Region

#Region "Monedas"

        ''' <summary>
        ''' Obtiene la informacion de una moneda		
        ''' </summary>
        ''' <param name="id">Id de la moneda</param>
        ''' <returns></returns>		
        Public Function GetMoneda(ByVal id As Integer) As ELL.Moneda
            Dim query As String = "SELECT CODMON,DESMON,RATE,CURRENCY,OBSOLETO,COD_ISO FROM XBAT.COMON WHERE CODMON=:CODMON"
            parameter = New OracleParameter("CODMON", OracleDbType.Int32, id, ParameterDirection.Input)

            Dim lMonedas As List(Of ELL.Moneda) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Moneda)(Function(r As OracleDataReader) _
             New ELL.Moneda With {.Id = SabLib.BLL.Utils.integerNull(r(0)), .Nombre = r(1), .ConversionEuros = SabLib.BLL.Utils.decimalNull(r(2)),
                .Abreviatura = SabLib.BLL.Utils.stringNull(r(3)), .CodISO = SabLib.BLL.Utils.stringNull(r(5))}, query, Me.cnBidaiak, parameter)

            Dim oMoneda As ELL.Moneda = Nothing
            If (lMonedas IsNot Nothing AndAlso lMonedas.Count > 0) Then oMoneda = lMonedas.Item(0)
            Return oMoneda
        End Function

        ''' <summary>
        ''' Obtiene la informacion de una moneda a partir de la abreviatura
        ''' </summary>
        ''' <param name="abreviateName">Abreviatura de la moneda</param>
        ''' <returns></returns>		
        Public Function GetMoneda(ByVal abreviateName As String) As ELL.Moneda
            Dim query As String = "SELECT CODMON,DESMON,RATE,CURRENCY,OBSOLETO,COD_ISO FROM XBAT.COMON WHERE CURRENCY=:CURRENCY"
            parameter = New OracleParameter("CURRENCY", OracleDbType.Varchar2, abreviateName, ParameterDirection.Input)

            Dim lMonedas As List(Of ELL.Moneda) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Moneda)(Function(r As OracleDataReader) _
             New ELL.Moneda With {.Id = SabLib.BLL.Utils.integerNull(r(0)), .Nombre = r(1), .ConversionEuros = SabLib.BLL.Utils.decimalNull(r(2)),
             .Abreviatura = SabLib.BLL.Utils.stringNull(r(3)), .CodISO = SabLib.BLL.Utils.stringNull(r(5))}, query, Me.cnBidaiak, parameter)

            Dim oMoneda As ELL.Moneda = Nothing
            If (lMonedas IsNot Nothing AndAlso lMonedas.Count > 0) Then oMoneda = lMonedas.Item(0)
            Return oMoneda
        End Function

        ''' <summary>
        ''' Obtiene las monedas que cumplan las condiciones de busqueda	
        ''' </summary>		
        ''' <param name="vigentes">Indica si se quieren las vigentes</param>
        ''' <param name="idPlantaAnticipo">Si es mayor que 0, se obtendran las monedas asociadas a los anticipos de la planta seleccionada</param>
        ''' <returns>Lista de monedas</returns>		
        Public Function GetMonedas(ByVal vigentes As Boolean, ByVal idPlantaAnticipo As Integer) As List(Of ELL.Moneda)
            Dim query As String = "SELECT C.CODMON,C.DESMON,C.RATE,C.CURRENCY,C.OBSOLETO,C.COD_ISO FROM XBAT.COMON C "
            Dim where As String = String.Empty
            Dim parameter As OracleParameter = Nothing
            If (idPlantaAnticipo > 0) Then
                query &= "INNER JOIN ANTICIPO_MONEDAS AM ON C.CODMON=AM.ID_MONEDA "
                where &= If(where <> String.Empty, " AND ", "") & "AM.ID_PLANTA=:ID_PLANTA "
                parameter = New OracleParameter("ID_PLANTA", OracleDbType.Varchar2, idPlantaAnticipo, ParameterDirection.Input)
            End If
            If (vigentes) Then where &= If(where <> String.Empty, " AND ", "") & "C.OBSOLETO=0 "
            If (where <> String.Empty) Then query &= " WHERE " & where
            query &= " ORDER BY C.DESMON"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Moneda)(Function(r As OracleDataReader) _
             New ELL.Moneda With {.Id = SabLib.BLL.Utils.integerNull(r(0)), .Nombre = r(1), .ConversionEuros = SabLib.BLL.Utils.decimalNull(r(2)),
             .Abreviatura = SabLib.BLL.Utils.stringNull(r(3)), .CodISO = SabLib.BLL.Utils.stringNull(r(5))}, query, Me.cnBidaiak, parameter)
        End Function

        ''' <summary>
        ''' Obtiene la informacion de cambio de una moneda en una fecha		
        ''' </summary>
        ''' <param name="currency">Nombre de la moneda</param>
        ''' <param name="fecha">Fecha en la que se consultara</param>
        ''' <returns></returns>		
        Public Function GetRateHistorico(ByVal currency As String, ByVal fecha As Date) As Decimal
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT RATE FROM XBAT.COMON_HISTORIKO WHERE CURRENCY=:CURRENCY AND FECHA_ACT=:FECHA"
            lParametros.Add(New OracleParameter("CURRENCY", OracleDbType.Varchar2, currency, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, CDate(fecha.ToShortDateString), ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Decimal)(query, Me.cnBidaiak, lParametros.ToArray)
        End Function

        ''' <summary>
        ''' Obtiene la informacion de cambio de una moneda de media de una año y mes
        ''' Hay que truncar el resultado porque sino da error de desbordamiento ya que en la base de datos hay muchos decimales		
        ''' </summary>
        ''' <param name="currency">Nombre de la moneda</param>
        ''' <param name="anno">Año</param>
        ''' <param name="mes">Mes</param>
        ''' <returns></returns>		
        Public Function GetRateMedia(ByVal currency As String, ByVal anno As Integer, ByVal mes As Integer) As Decimal
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT TRUNC(RATE,4) FROM XBAT.COMON_MEDIA WHERE CURRENCY=:CURRENCY AND ANNO=:ANNO AND MES=:MES"
            lParametros.Add(New OracleParameter("CURRENCY", OracleDbType.Varchar2, currency, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, mes, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Decimal)(query, Me.cnBidaiak, lParametros.ToArray)
        End Function

        ''' <summary>
        ''' Obtiene la informacion de un proyecto
        ''' </summary>
        ''' <param name="idProy">Id del proyecto</param>
        ''' <returns>0:IdProy,1:Proyecto,2:CodCli,3:Cliente</returns>
        Public Function GetInfoClienteProyecto(ByVal idProy As Integer) As List(Of String())
            Dim query As String = "SELECT P.ID,P.PROGRAMA,C.CODCLI,C.NOMBRE FROM FACLIENTE C INNER JOIN FAPROGRAMA P ON C.CODCLI=P.CODCLI WHERE P.ID=:ID"
            Return Memcached.OracleDirectAccess.Seleccionar(query, Me.cnXBAT, New OracleParameter("ID", OracleDbType.Int32, idProy, ParameterDirection.Input))
        End Function

#End Region

#Region "Documentacion de proyectos"

        ''' <summary>
        ''' Obtiene la informacion del documento del proyecto		
        ''' </summary>
        ''' <param name="id">Id del documento</param>
        ''' <returns></returns>		
        Public Function GetDocumentoProyecto(ByVal id As Integer) As ELL.DocumentoProyecto
            Dim query As String = "SELECT D.ID_FAPROGRAMA,D.DESCRI,D.ADJUNTO,D.ID_SAB,D.FECHA,D.CONTENT_TYPE,P.PROGRAMA FROM FAPROGRAMADOC D INNER JOIN FAPROGRAMA P ON D.ID_FAPROGRAMA=P.ID WHERE D.ID=:ID"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.DocumentoProyecto)(Function(r As OracleDataReader) _
             New ELL.DocumentoProyecto With {.Id = id, .IdProyecto = CInt(r("ID_FAPROGRAMA")), .Descripcion = r("DESCRI"), .IdUsuario = CInt(r("ID_SAB")),
             .Fecha = CDate(r("FECHA")), .Adjunto = CType(r("ADJUNTO"), Byte()), .ContentType = r("CONTENT_TYPE"), .NombreProyecto = "PROGRAMA"},
              query, Me.cnXBAT, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault
        End Function

        ''' <summary>
        ''' Obtiene el listado de documentos asociados a un proyecto
        ''' No se obtiene el adjunto
        ''' </summary>
        ''' <param name="idProyecto">Id del proyecto</param>
        ''' <returns></returns>		
        Public Function GetDocumentosProyecto(ByVal idProyecto As Integer) As List(Of ELL.DocumentoProyecto)
            Dim query As String = "SELECT D.ID,D.ID_FAPROGRAMA,D.DESCRI,D.ADJUNTO,D.ID_SAB,D.FECHA,D.CONTENT_TYPE,P.PROGRAMA FROM FAPROGRAMADOC D INNER JOIN FAPROGRAMA P ON D.ID_FAPROGRAMA=P.ID WHERE D.ID_FAPROGRAMA=:ID_FAPROGRAMA"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.DocumentoProyecto)(Function(r As OracleDataReader) _
             New ELL.DocumentoProyecto With {.Id = CInt(r("ID")), .IdProyecto = idProyecto, .Descripcion = r("DESCRI"), .IdUsuario = CInt(r("ID_SAB")),
             .Fecha = CDate(r("FECHA")), .ContentType = r("CONTENT_TYPE"), .NombreProyecto = r("PROGRAMA")}, query, Me.cnXBAT, New OracleParameter("ID_FAPROGRAMA", OracleDbType.Int32, idProyecto, ParameterDirection.Input))
        End Function

        ''' <summary>
        ''' Inserta o actualiza el documento especificado
        ''' </summary>
        ''' <param name="oDocProy">Documento del proyecto</param>        
        Public Sub SaveDocumentoProyecto(ByVal oDocProy As ELL.DocumentoProyecto)
            Dim query As String = String.Empty
            Dim lParametros As New List(Of OracleParameter)
            If (oDocProy.Id > 0) Then
                query = "UPDATE FAPROGRAMADOC SET DESCRI=:DESCRI "
                If (oDocProy.Adjunto IsNot Nothing) Then
                    query &= ",ADJUNTO=:ADJUNTO,CONTENT_TYPE=:CONTENT_TYPE "
                    lParametros.Add(New OracleParameter("ADJUNTO", OracleDbType.Blob, oDocProy.Adjunto, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("CONTENT_TYPE", OracleDbType.NVarchar2, oDocProy.ContentType, ParameterDirection.Input))
                End If
                query &= "WHERE ID=:ID"
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oDocProy.Id, ParameterDirection.Input))
            Else
                query = "INSERT INTO FAPROGRAMADOC(ID_FAPROGRAMA,DESCRI,ADJUNTO,ID_SAB,FECHA,CONTENT_TYPE) VALUES (:ID_FAPROGRAMA,:DESCRI,:ADJUNTO,:ID_SAB,SYSDATE,:CONTENT_TYPE)"
                lParametros.Add(New OracleParameter("ID_FAPROGRAMA", OracleDbType.Int32, oDocProy.IdProyecto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, oDocProy.IdUsuario, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ADJUNTO", OracleDbType.Blob, oDocProy.Adjunto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CONTENT_TYPE", OracleDbType.NVarchar2, oDocProy.ContentType, ParameterDirection.Input))
            End If
            lParametros.Add(New OracleParameter("DESCRI", OracleDbType.NVarchar2, oDocProy.Descripcion, ParameterDirection.Input))
            Memcached.OracleDirectAccess.NoQuery(query, Me.cnXBAT, lParametros.ToArray)
        End Sub

        ''' <summary>
        ''' Elimina el documento especificado
        ''' </summary>
        ''' <param name="idDoc">Id del documento</param>        
        Public Sub DeleteDocumentoProyecto(ByVal idDoc As Integer)
            Dim query As String = "DELETE FROM FAPROGRAMADOC WHERE ID=:ID"
            Memcached.OracleDirectAccess.NoQuery(query, Me.cnXBAT, New OracleParameter("ID", OracleDbType.Int32, idDoc, ParameterDirection.Input))
        End Sub

#End Region

    End Class

End Namespace