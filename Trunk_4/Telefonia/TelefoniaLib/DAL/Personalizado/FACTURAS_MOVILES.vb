'===============================================================================
'BATZ, Koop. - 08/10/2008 15:48:28
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_View.vbgen
' El soporte de la clase  esta en el directorio Architecture  en "dOOdads".
'===============================================================================
Imports System.Runtime.InteropServices
Imports AccesoAutomaticoBD
Imports log4net

Namespace DAL

    Public Class FACTURAS_MOVILES
        Inherits _FACTURAS_MOVILES
        Private Log As ILog = LogManager.GetLogger("root.Telefonia")
        Public Sub New()
            'Decide connection string depending on state
            Try
                If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIALIVE").ConnectionString
                Else
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIATEST").ConnectionString
                End If
            Catch ex As Exception
                Log.Error("Error al inicializar el connection string Telefonia.", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Lee la cabecera de la factura del origen pasado como parametro y devuelve informado los datos en una factura
        ''' </summary>
        ''' <param name="ficheroImp">Origen del fichero donde se extraen los datos</param>
        ''' <returns>Factura con la cabecera informada</returns>
        Public Function LeerCabeceraFactura(ByVal ficheroImp As String) As ELL.Factura
            'Try
            Dim oFactura As ELL.Factura = Nothing
            Dim query As String = "SELECT FAC_NUM_FACTURA,FAC_FECHA_FACTURA,FAC_TOTAL_SRV_MEDIDO,FAC_TOTAL_DESCUENTOS,FAC_TOTAL_IMPUESTOS,FAC_TOTAL_PAGAR,FAC_CIF_EMPRESA FROM FACTURA WHERE FAC_CIF_EMPRESA='F48037600'"
            'Dim connectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ficheroImp & ";"
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ficheroImp & ";"
            Dim lCab As List(Of String()) = MSAccessDirectAccess.Seleccionar(query, connectionString, Nothing)

            If (lCab IsNot Nothing) Then                
                oFactura = New ELL.Factura
                For Each sCab As String() In lCab
                    If (oFactura.NumFactura <> String.Empty) Then oFactura.NumFactura &= ","
                    oFactura.NumFactura &= "'" & sCab(0) & "'"
                    oFactura.FechaFactura = CType(sCab(1), Date)
                    oFactura.Total += CDec(sCab(2))
                    oFactura.Descuento += CDec(sCab(3))
                    oFactura.IVA += CDec(sCab(4))
                    oFactura.TotalPagar += CDec(sCab(5))
                    oFactura.CifEmpresa = sCab(6)
                Next
            End If

            Return oFactura
            'Catch ex As Exception
            '   Throw New BatzException("errLeerCabeceraFactura", ex)
            'End Try
        End Function

        ''' <summary>
        ''' Lee las cabeceras de la factura del origen pasado como parametro y devuelve informado los datos en una factura
        ''' </summary>
        ''' <param name="ficheroImp">Origen del fichero donde se extraen los datos</param>
        ''' <returns>Factura con la cabecera informada</returns>
        Public Function LeerCabecerasFactura(ByVal ficheroImp As String) As List(Of ELL.Factura)
            'Try
            Dim lFacturas As List(Of ELL.Factura) = Nothing
            Dim query As String = "SELECT FAC_NUM_FACTURA,FAC_FECHA_FACTURA,FAC_TOTAL_SRV_MEDIDO,FAC_TOTAL_DESCUENTOS,FAC_TOTAL_IMPUESTOS,FAC_TOTAL_PAGAR,FAC_CIF_EMPRESA FROM FACTURA"            
            Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ficheroImp & ";"  '"Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & ficheroImp & ";"
            Dim lCab As List(Of String()) = MSAccessDirectAccess.Seleccionar(query, connectionString, Nothing)

            If (lCab IsNot Nothing) Then
                Dim plantBLL As New Sablib.BLL.PlantasComponent
                Dim oFactura As ELL.Factura
                Dim oPlanta As Sablib.ELL.Planta
                lFacturas = New List(Of ELL.Factura)
                For Each sCab As String() In lCab
                    oFactura = New ELL.Factura
                    oFactura.NumFactura = sCab(0)
                    oFactura.FechaFactura = CType(sCab(1), Date)
                    oFactura.Total = CDec(sCab(2))
                    oFactura.Descuento = CDec(sCab(3))
                    oFactura.IVA = CDec(sCab(4))
                    oFactura.TotalPagar = CDec(sCab(5))
                    oFactura.CifEmpresa = sCab(6)
                    If (oFactura.CifEmpresa <> String.Empty) Then
                        oPlanta = plantBLL.GetPlantaByCif(oFactura.CifEmpresa)
                        If (oPlanta IsNot Nothing) Then
                            oFactura.IdPlanta = oPlanta.Id
                            oFactura.NombrePlanta = oPlanta.Nombre
                        End If
                    End If
                    lFacturas.Add(oFactura)
                Next
            End If

            Return lFacturas
            'Catch ex As Exception
            '   Throw New BatzException("errLeerCabeceraFactura", ex)
            'End Try
        End Function

        ''' <summary>
        ''' Lee la cabecera de la factura del origen pasado como parametro y devuelve informado los datos en una factura
        ''' </summary>
        ''' <param name="connectionString">String de conexion</param>
        ''' <returns>Factura con la cabecera informada</returns>
        Public Function LeerCabeceraFactura2(ByVal connectionString As String) As ELL.Factura
            'Try
            Dim oFactura As ELL.Factura = Nothing
            Dim query As String = "SELECT FAC_FECHA_FACTURA,FAC_TOTAL_SRV_MEDIDO,FAC_TOTAL_DESCUENTOS,FAC_TOTAL_IMPUESTOS,FAC_TOTAL_PAGAR FROM FACTURA"
            Dim lCab As List(Of String()) = MSAccessDirectAccess.Seleccionar(query, connectionString, Nothing)

            If (lCab IsNot Nothing) Then
                oFactura = New ELL.Factura
                For Each sCab As String() In lCab
                    oFactura.FechaFactura = CType(sCab(0), Date)
                    oFactura.Total += CDec(sCab(1))
                    oFactura.Descuento += CDec(sCab(2))
                    oFactura.IVA += CDec(sCab(3))
                    oFactura.TotalPagar += CDec(sCab(4))
                Next
            End If

            Return oFactura
            'Catch ex As Exception
            '   Throw New BatzException("errLeerCabeceraFactura", ex)
            'End Try
        End Function

        ''' <summary>
        ''' Lee la cabecera de la factura del origen pasado como parametro y devuelve informado los datos en una factura
        ''' </summary>
        ''' <param name="ficheroImp">Origen del fichero donde se extraen los datos</param>
        ''' <returns>Factura con la cabecera informada</returns>
        Public Function LeerCabeceraFacturaODBC(ByVal ficheroImp As String) As ELL.Factura
            Try
                Dim oFactura As ELL.Factura = Nothing
                Dim query As String = "SELECT FAC_FECHA_FACTURA,FAC_TOTAL_SRV_MEDIDO,FAC_TOTAL_DESCUENTOS,FAC_TOTAL_IMPUESTOS,FAC_TOTAL_PAGAR FROM FACTURA"
                'Dim connectionString As String = "Driver={Microsoft Access Driver(*.mdb)};Dbq=" & ficheroImp & ";"
                'Dim connectionString As String = "DSN=Factura_Telefonia"
                Dim connectionString As String = "FILEDSN=" & Configuration.ConfigurationManager.AppSettings("FacturasTelefonia") & "\Factura.dsn;"
                Dim lCab As List(Of String()) = MSAccessDirectAccess.SeleccionarODBC(query, connectionString, Nothing)

                If (lCab IsNot Nothing) Then
                    oFactura = New ELL.Factura
                    For Each sCab As String() In lCab
                        oFactura.FechaFactura = CType(sCab(0), Date)
                        oFactura.Total += CDec(sCab(1))
                        oFactura.Descuento += CDec(sCab(2))
                        oFactura.IVA += CDec(sCab(3))
                        oFactura.TotalPagar += CDec(sCab(4))
                    Next
                End If

                Return oFactura
            Catch ex As Exception
                Throw New BatzException("errLeerCabeceraFactura", ex)
            End Try
        End Function

        ''' <summary>
        ''' Lee las linea de la factura del origen pasado como parametro y devuelve informado los datos en una factura
        ''' </summary>
        ''' <param name="ficheroImp">Origen del fichero donde se extraen los datos</param>
        ''' <returns>Factura con la cabecera informada</returns>
        Public Function LeerLineasFactura(ByVal ficheroImp As String) As DataSet
            Try
                Dim oFactura As ELL.Factura = Nothing

                Dim query As String = "SELECT DET_NU_TELEFONO,DET_NU_EXTENSION,DET_TIPO_TRAFICO,DET_DESCRIP_TIPO_LLAMADA,DET_TIPO_DESTINO,DET_NUMERO_LLAMADO,DET_FECHA,DET_HORA_INICIO,DET_CANTIDAD_MEDIDA_ORIGINADA,DET_IMPORTE,DET_NUM_FACTURA FROM DETALLE"
                Dim connectionString As String = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" & ficheroImp & ";"
                Return MSAccessDirectAccess.Seleccionar2(query, connectionString, Nothing)
            Catch ex As Exception
                Throw New BatzException("errLeerLineasFactura", ex)
            End Try
        End Function

        ''' <summary>
        ''' Lee las linea de la factura del origen pasado como parametro y devuelve informado los datos en una factura
        ''' </summary>
        ''' <param name="ficheroImp">Origen del fichero donde se extraen los datos</param>
        ''' <returns>Factura con la cabecera informada</returns>
        Public Function LeerLineasFacturaODBC(ByVal ficheroImp As String) As DataSet
            Try
                Dim oFactura As ELL.Factura = Nothing

                Dim query As String = "SELECT DET_NU_TELEFONO,DET_NU_EXTENSION,DET_TIPO_TRAFICO,DET_DESCRIP_TIPO_LLAMADA,DET_TIPO_DESTINO,DET_NUMERO_LLAMADO,DET_FECHA,DET_HORA_INICIO,DET_CANTIDAD_MEDIDA_ORIGINADA,DET_IMPORTE FROM DETALLE"
                'Dim connectionString As String = "Driver={Microsoft Access Driver (*.mdb)};Dbq=" & ficheroImp & ";"
                'Dim connectionString As String = "DSN=Factura_Telefonia"
                Dim connectionString As String = "FILEDSN=" & Configuration.ConfigurationManager.AppSettings("FacturasTelefonia") & "\Factura.dsn;"
                Return MSAccessDirectAccess.Seleccionar2ODBC(query, connectionString, Nothing)
            Catch ex As Exception
                Throw New BatzException("errLeerLineasFactura", ex)
            End Try
        End Function


        ' ''' <summary>
        ' ''' Obtiene para un usuario, la facturacion anual de su telefono movil
        ' ''' </summary>
        ' ''' <param name="idUser">Identificador del usuario</param>
        ' ''' <param name="año">Año a consultar</param>
        ' ''' <returns>Reader con todos los datos</returns>
        ' ''' <remarks></remarks>
        'Public Function getFacturacionAnualOld(ByVal idUser As Integer, ByVal año As Integer) As IDataReader
        '    Try
        '        Dim sql As New System.Text.StringBuilder
        '        Dim where As String = String.Empty
        '        sql.AppendLine(" SELECT FM.IMPORTE,FM.TIPO_LLAMADA,FM.FECHA,FM.TIEMPO ,E.EXTENSION,EP.ID_USUARIO")
        '        sql.AppendLine(" FROM FACTURAS_MOVILES FM inner JOIN EXTENSION E ON FM.EXTENSION=E.EXTENSION")
        '        sql.AppendLine(" INNER JOIN EXTENSION_PERSONAS EP ON (EP.ID_EXTENSION=E.ID or ep.id_extension=E.ID_EXT_INTERNA)")
        '        sql.AppendLine(" WHERE to_char(FM.FECHA,'YYYY')='" & año & "' AND EP.ID_USUARIO=" & idUser & " AND ")
        '        sql.AppendLine(" ((to_char(EP.F_DESDE,'YYYY')<='" & año & "') and (EP.F_HASTA IS NULL OR to_char(EP.F_HASTA,'YYYY')>='" & año & "')) AND")
        '        sql.AppendLine(" (EP.F_DESDE<=FM.FECHA AND (EP.F_HASTA IS NULL OR (EP.F_HASTA IS NOT NULL AND EP.F_HASTA>=FM.FECHA))) AND instr(e.extension,'59')<>1")
        '        Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
        '    Catch ex As Exception
        '        Throw New BatzException("errCalcularFacturacion", ex)
        '    End Try
        'End Function

        ''' <summary>
        ''' Obtiene para un usuario, la facturacion anual de su telefono movil
        ''' </summary>
        ''' <param name="idUser">Identificador del usuario</param>
        ''' <param name="año">Año a consultar</param>
        ''' <returns>Reader con todos los datos</returns>
        Public Function getFacturacionAnual(ByVal idUser As Integer, ByVal año As Integer) As IDataReader
            Try
                Dim sql As New System.Text.StringBuilder
                Dim where As String = String.Empty
                sql.AppendLine(" SELECT IMPORTE,TIPO_LLAMADA,MES,ANNO,TIEMPO,EXTENSION,ID_USUARIO")
                sql.AppendLine(" FROM NUEVO_FACTURAS_MOVIL ")
                sql.AppendLine(" WHERE ANNO='" & año & "' AND ID_USUARIO=" & idUser)
                'sql.AppendLine("  AND instr(extension,'59')<>1")

                Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
            Catch ex As Exception
                Throw New BatzException("errCalcularFacturacion", ex)
            End Try
        End Function

        ''' <summary>
        ''' Fuerza el refresco de la vista materializada de facturas ejecutando un procedimiento almacenado
        ''' Debido a que el procedimiento tarda unos 5 minutos en ejecutarse, hay que pasarle unos parametros para el aviso mediante email de que ha terminado      
        ''' </summary>        
        ''' <param name="emailFrom">Email desde la que se enviara el email</param>
        ''' <param name="emailTo">Email al que se enviara</param>
        ''' <param name="subject">Asunto</param>
        ''' <param name="body">Cuerpo del mensaje</param>       
        Public Sub RefrescarVistaMaterializadaFacturas(ByVal emailFrom As String, ByVal emailTo As String, ByVal subject As String, ByVal body As String)
            Dim cn As New OracleConnection(Me.ConnectionString)
            Dim cmd As New OracleCommand("ACTUALIZA_W_FACTURA_MOVIL", cn)
            cmd.Parameters.Add(New OracleParameter("de", OracleDbType.NVarchar2, emailFrom, ParameterDirection.Input))
            cmd.Parameters.Add(New OracleParameter("para", OracleDbType.NVarchar2, emailTo, ParameterDirection.Input))
            cmd.Parameters.Add(New OracleParameter("asunto", OracleDbType.NVarchar2, subject, ParameterDirection.Input))
            cmd.Parameters.Add(New OracleParameter("cuerpo", OracleDbType.NVarchar2, body, ParameterDirection.Input))
            cmd.CommandType = Data.CommandType.StoredProcedure
            Try
                cn.Open()
                cmd.ExecuteNonQuery()
                cn.Close()
                cmd.Dispose()
            Catch ex As Exception
                cn.Close()
                cmd.Dispose()
                Throw ex
            End Try
        End Sub

        ''' <summary>
        ''' Obtiene la fecha de la ultima ejecucion de la vista materializada
        ''' </summary>        
        Public Function getFechaUltimoRefrescoVistaMat() As DateTime
            Try
                Dim miConexion As String
                If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                    miConexion = Configuration.ConfigurationManager.ConnectionStrings.Item("SYSTEMLIVE").ConnectionString
                Else
                    miConexion = Configuration.ConfigurationManager.ConnectionStrings.Item("SYSTEMTEST").ConnectionString
                End If
                Dim sql As String = "select last_refresh_date from dba_mviews where owner = 'TELEFONIA' and mview_name='W_FACTURA_MOVIL'"
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of DateTime)(sql, miConexion)
            Catch ex As Exception
                Log.Error("Ha ocurrido un error al intentar obtener la fecha de la ultimo refresco de la vista materializa", ex)
                Return DateTime.MinValue
            End Try
        End Function

    End Class

End Namespace

