Imports System.Globalization
Imports System.Text.RegularExpressions

Namespace DAL.BRAIN

    Public Class CCMetadataDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String) As ELL.BRAIN.CCMetadata
            Dim query As String = "SELECT CC.*, T44.DENOM AS DENOMINACION
                                   FROM CUBOS.CCHEADER CC
                                   INNER JOIN CUBOS.T_44N T44 ON T44.EMPRESA = CC.EMPRESA AND T44.PLANTA = CC.PLANTA AND UPPER(T44.VALOR) = UPPER(CC.PORTADOR)
                                   WHERE CC.EMPRESA = ? AND CC.PLANTA = ? AND UPPER(CC.PORTADOR) = ?"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))
            lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador.ToUpper()))

            Return Memcached.OleDbDirectAccess.Seleccionar(Of ELL.BRAIN.CCMetadata)(Function(r As OleDb.OleDbDataReader) _
            New ELL.BRAIN.CCMetadata With {.Empresa = CStr(r("EMPRESA")), .Planta = CStr(r("PLANTA")), .CodigoPortador = CStr(r("PORTADOR")),
                                           .TipoPlanta = CStr(r("PLANTCORP")), .DenomAmpliada = SabLib.BLL.Utils.stringNull(r("DENOM")), .FechaIni = If(CInt(r("FECHAINI")) = 0, DateTime.MinValue, DateTime.ParseExact(r("FECHAINI"), "yyyyMMdd", CultureInfo.InvariantCulture)),
                                           .FechaFin = If(CInt(r("FECHAFIN")) = 0, DateTime.MinValue, DateTime.ParseExact(r("FECHAFIN"), "yyyyMMdd", CultureInfo.InvariantCulture)), .Negocio = CStr(r("NEGOCIO")),
                                           .Responsable = CStr(r("RESPONSAB")), .IdResponsableSAB = If(CInt(r("RESPIDSAB")) = 0, Integer.MinValue, CInt(r("RESPIDSAB"))),
                                           .FechaEstimIni = If(CInt(r("FECESTMINI")) = 0, DateTime.MinValue, DateTime.ParseExact(r("FECESTMINI"), "yyyyMMdd", CultureInfo.InvariantCulture)),
                                           .NumAnyosSerie = If(CInt(r("ANESSERIE")) = 0, Integer.MinValue, CInt(r("ANESSERIE"))), .Producto = SabLib.BLL.Utils.stringNull(r("PRODUCTO")), .IdProyecto = SabLib.BLL.Utils.stringNull(r("IDPROYECTO")),
                                           .Proyecto = SabLib.BLL.Utils.stringNull(r("PROYECTO")), .EstadoProyecto = SabLib.BLL.Utils.stringNull(r("ESTADOPROY")), .TipoActivo = SabLib.BLL.Utils.stringNull(r("TIPOACT")),
                                           .IdTipoActivo = If(CInt(r("IDTIPOACT")) = 0, Integer.MinValue, CInt(r("IDTIPOACT"))), .Propiedad = SabLib.BLL.Utils.stringNull(r("PROPIEDAD")), .Lantegi = CStr(r("LANTEGI")),
                                           .BudgetCode = SabLib.BLL.Utils.stringNull(r("CODPG")), .DescBudgetCode = SabLib.BLL.Utils.stringNull(r("DESCODPG")),
                                           .CantidadSolicitada = If(CDec(r("CANTSOL")) = Decimal.Zero, Decimal.MinValue, CDec(r("CANTSOL"))), .Denominacion = CStr(r("DENOMINACION"))}, query, CadenaConexionBRAIN, lstp.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Public Shared Function loadList(Optional ByVal empresa As String = Nothing, Optional ByVal planta As String = "000", Optional ByVal codigo As String = Nothing) As List(Of ELL.BRAIN.CCMetadata)
            Dim query As String = "SELECT CC.*, T44.DENOM AS DENOMINACION
                                   FROM CUBOS.CCHEADER CC
                                   INNER JOIN CUBOS.T_44N T44 ON T44.EMPRESA = CC.EMPRESA AND T44.PLANTA = CC.PLANTA AND UPPER(T44.VALOR) = UPPER(CC.PORTADOR){0}"

            Dim lstp As New List(Of OleDb.OleDbParameter)

            Dim where As String = String.Empty
            If (Not String.IsNullOrEmpty(empresa)) Then
                where = " WHERE CC.EMPRESA = ? AND CC.PLANTA = ?"

                lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
                lstp.Add(New OleDb.OleDbParameter("planta", planta))
            End If

            If (Not String.IsNullOrEmpty(codigo)) Then
                If (Not String.IsNullOrEmpty(where)) Then
                    where &= " AND UPPER(CC.PORTADOR) LIKE ?"
                Else
                    where &= " WHERE UPPER(CC.PORTADOR) LIKE ?"
                End If
                lstp.Add(New OleDb.OleDbParameter("codigo", "%" & codigo.ToUpper() & "%"))
            End If
            query = String.Format(query, where)

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.BRAIN.CCMetadata)(Function(r As OleDb.OleDbDataReader) _
            New ELL.BRAIN.CCMetadata With {.Empresa = CStr(r("EMPRESA")), .Planta = CStr(r("PLANTA")), .CodigoPortador = CStr(r("PORTADOR")),
                                           .TipoPlanta = CStr(r("PLANTCORP")), .DenomAmpliada = SabLib.BLL.Utils.stringNull(r("DENOM")), .FechaIni = If(CInt(r("FECHAINI")) = 0, DateTime.MinValue, DateTime.ParseExact(r("FECHAINI"), "yyyyMMdd", CultureInfo.InvariantCulture)),
                                           .FechaFin = If(CInt(r("FECHAFIN")) = 0, DateTime.MinValue, DateTime.ParseExact(r("FECHAFIN"), "yyyyMMdd", CultureInfo.InvariantCulture)), .Negocio = CStr(r("NEGOCIO")),
                                           .Responsable = CStr(r("RESPONSAB")), .IdResponsableSAB = If(CInt(r("RESPIDSAB")) = 0, Integer.MinValue, CInt(r("RESPIDSAB"))),
                                           .FechaEstimIni = If(CInt(r("FECESTMINI")) = 0, DateTime.MinValue, DateTime.ParseExact(r("FECESTMINI"), "yyyyMMdd", CultureInfo.InvariantCulture)),
                                           .NumAnyosSerie = If(CInt(r("ANESSERIE")) = 0, Integer.MinValue, CInt(r("ANESSERIE"))), .Producto = SabLib.BLL.Utils.stringNull(r("PRODUCTO")), .IdProyecto = SabLib.BLL.Utils.stringNull(r("IDPROYECTO")),
                                           .Proyecto = SabLib.BLL.Utils.stringNull(r("PROYECTO")), .EstadoProyecto = SabLib.BLL.Utils.stringNull(r("ESTADOPROY")), .TipoActivo = SabLib.BLL.Utils.stringNull(r("TIPOACT")),
                                           .IdTipoActivo = If(CInt(r("IDTIPOACT")) = 0, Integer.MinValue, CInt(r("IDTIPOACT"))), .Propiedad = SabLib.BLL.Utils.stringNull(r("PROPIEDAD")), .Lantegi = CStr(r("LANTEGI")),
                                           .BudgetCode = SabLib.BLL.Utils.stringNull(r("CODPG")), .DescBudgetCode = SabLib.BLL.Utils.stringNull(r("DESCODPG")),
                                           .CantidadSolicitada = If(CDec(r("CANTSOL")) = Decimal.Zero, Decimal.MinValue, CDec(r("CANTSOL"))), .Denominacion = CStr(r("DENOMINACION"))}, query, CadenaConexionBRAIN, lstp.ToArray())
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda
        ''' </summary>
        ''' <param name="ccMetadata"></param>
        ''' <param name="empresasProductivas"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal ccMetadata As ELL.BRAIN.CCMetadata, ByVal empresasProductivas As IEnumerable(Of ELL.BRAIN.CCProductionPlant))
            Dim con As OleDb.OleDbConnection = Nothing
            Dim transact As OleDb.OleDbTransaction = Nothing
            Dim query As String = String.Empty

            Try
                con = New OleDb.OleDbConnection(CadenaConexionBRAIN)
                con.Open()
                transact = con.BeginTransaction()

                Dim bNuevo As Boolean = (getObject(ccMetadata.Empresa, ccMetadata.Planta, ccMetadata.CodigoPortador) Is Nothing)

                Dim denomAmpliada = If(String.IsNullOrEmpty(ccMetadata.DenomAmpliada), String.Empty, If(ccMetadata.DenomAmpliada.Length > 200, ccMetadata.DenomAmpliada.Substring(0, 200), ccMetadata.DenomAmpliada))
                Dim responsable = If(String.IsNullOrEmpty(ccMetadata.Responsable), String.Empty, If(ccMetadata.Responsable.Length > 100, ccMetadata.Responsable.Substring(0, 100), ccMetadata.Responsable))
                Dim producto = If(String.IsNullOrEmpty(ccMetadata.Producto), String.Empty, If(ccMetadata.Producto.Length > 50, ccMetadata.Producto.Substring(0, 50), ccMetadata.Producto))
                Dim proyecto = If(String.IsNullOrEmpty(ccMetadata.Proyecto), String.Empty, If(ccMetadata.Proyecto.Length > 50, ccMetadata.Proyecto.Substring(0, 50), ccMetadata.Proyecto))
                Dim tipoActivo = If(String.IsNullOrEmpty(ccMetadata.TipoActivo), String.Empty, If(ccMetadata.TipoActivo.Length > 50, ccMetadata.TipoActivo.Substring(0, 50), ccMetadata.TipoActivo))
                Dim descBudgetCode = If(String.IsNullOrEmpty(ccMetadata.DescBudgetCode), String.Empty, If(ccMetadata.DescBudgetCode.Length > 200, ccMetadata.DescBudgetCode.Substring(0, 200), ccMetadata.DescBudgetCode))
                'El responsable puede tener caracteres con tildes y eso hace que falle. Por ello, se tiene que llamar a la funcion Normalize                
                responsable = SabLib.BLL.Utils.NormalizeString(responsable)

                ' Guardamos
                Dim lstp As New List(Of OleDb.OleDbParameter)
                lstp.Add(New OleDb.OleDbParameter("tipoPlanta", ccMetadata.TipoPlanta))
                lstp.Add(New OleDb.OleDbParameter("denomAmpliada", denomAmpliada))
                lstp.Add(New OleDb.OleDbParameter("fechaIni", String.Format("{0}{1}{2}", ccMetadata.FechaIni.Year.ToString("0000"), ccMetadata.FechaIni.Month.ToString("00"), ccMetadata.FechaIni.Day.ToString("00"))))
                lstp.Add(New OleDb.OleDbParameter("fechaFin", If(ccMetadata.FechaFin = DateTime.MinValue, 0, String.Format("{0}{1}{2}", ccMetadata.FechaFin.Year.ToString("0000"), ccMetadata.FechaFin.Month.ToString("00"), ccMetadata.FechaFin.Day.ToString("00")))))
                lstp.Add(New OleDb.OleDbParameter("responsable", responsable))
                lstp.Add(New OleDb.OleDbParameter("idResponsableSAB", ccMetadata.IdResponsableSAB))
                lstp.Add(New OleDb.OleDbParameter("fechaEstimIni", If(ccMetadata.FechaEstimIni = DateTime.MinValue, 0, String.Format("{0}{1}{2}", ccMetadata.FechaEstimIni.Year.ToString("0000"), ccMetadata.FechaEstimIni.Month.ToString("00"), ccMetadata.FechaEstimIni.Day.ToString("00")))))
                lstp.Add(New OleDb.OleDbParameter("numAnyosSerie", If(ccMetadata.NumAnyosSerie = Integer.MinValue, 0, ccMetadata.NumAnyosSerie)))
                lstp.Add(New OleDb.OleDbParameter("producto", producto))
                lstp.Add(New OleDb.OleDbParameter("idProyecto", If(String.IsNullOrEmpty(ccMetadata.IdProyecto), String.Empty, ccMetadata.IdProyecto)))
                lstp.Add(New OleDb.OleDbParameter("proyecto", proyecto))
                lstp.Add(New OleDb.OleDbParameter("estadoProyecto", If(String.IsNullOrEmpty(ccMetadata.EstadoProyecto), String.Empty, ccMetadata.EstadoProyecto)))
                lstp.Add(New OleDb.OleDbParameter("tipoActivo", tipoActivo))
                lstp.Add(New OleDb.OleDbParameter("idTipoActivo", If(ccMetadata.IdTipoActivo = Integer.MinValue, Decimal.Zero, ccMetadata.IdTipoActivo)))
                lstp.Add(New OleDb.OleDbParameter("propiedad", If(String.IsNullOrEmpty(ccMetadata.Propiedad), String.Empty, ccMetadata.Propiedad)))
                lstp.Add(New OleDb.OleDbParameter("lantegi", ccMetadata.Lantegi))
                lstp.Add(New OleDb.OleDbParameter("codigoBudgetCode", If(String.IsNullOrEmpty(ccMetadata.BudgetCode), String.Empty, ccMetadata.BudgetCode)))
                lstp.Add(New OleDb.OleDbParameter("descBudgetCode", descBudgetCode))
                lstp.Add(New OleDb.OleDbParameter("cantidadSolicitada", If(ccMetadata.CantidadSolicitada = Decimal.MinValue, Decimal.Zero, ccMetadata.CantidadSolicitada)))
                lstp.Add(New OleDb.OleDbParameter("empresaProductiva", If(String.IsNullOrEmpty(ccMetadata.EmpresaProductiva), String.Empty, ccMetadata.EmpresaProductiva)))
                lstp.Add(New OleDb.OleDbParameter("empresa", ccMetadata.Empresa))
                lstp.Add(New OleDb.OleDbParameter("planta", ccMetadata.Planta))
                lstp.Add(New OleDb.OleDbParameter("codigoPortador", ccMetadata.CodigoPortador))

                If (bNuevo) Then
                    lstp.Add(New OleDb.OleDbParameter("origen", ccMetadata.Origen))

                    query = "INSERT INTO CUBOS.CCHEADER (PLANTCORP, DENOM, FECHAINI, FECHAFIN, RESPONSAB, " _
                        & "RESPIDSAB, FECESTMINI, ANESSERIE, PRODUCTO, IDPROYECTO, PROYECTO, ESTADOPROY, TIPOACT, IDTIPOACT, PROPIEDAD, " _
                        & "LANTEGI, CODPG, DESCODPG, CANTSOL, PRODEMP, EMPRESA, PLANTA, PORTADOR, ORIGEN) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
                Else
                    query = "UPDATE CUBOS.CCHEADER SET PLANTCORP = ?, DENOM = ?, FECHAINI = ?, FECHAFIN = ?, RESPONSAB = ?, " _
                        & "RESPIDSAB = ?, FECESTMINI = ?, ANESSERIE = ?, PRODUCTO = ?, IDPROYECTO = ?, PROYECTO = ?, ESTADOPROY = ?, TIPOACT = ?, IDTIPOACT = ?, PROPIEDAD = ?, " _
                        & "LANTEGI = ?, CODPG = ?, DESCODPG = ?, CANTSOL = ?, PRODEMP = ? WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ?"
                End If

                Memcached.OleDbDirectAccess.NoQuery(query, con, transact, lstp.ToArray())

                If (empresasProductivas IsNot Nothing AndAlso empresasProductivas.Count > 0) Then
                    ' Primero borramos las posibles empresas productivas que pudiera haber
                    query = "DELETE FROM CUBOS.CCPRODPLAN WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ?"

                    lstp = New List(Of OleDb.OleDbParameter)
                    lstp.Add(New OleDb.OleDbParameter("empresa", ccMetadata.Empresa))
                    lstp.Add(New OleDb.OleDbParameter("planta", ccMetadata.Planta))
                    lstp.Add(New OleDb.OleDbParameter("codigoPortador", ccMetadata.CodigoPortador))

                    Memcached.OleDbDirectAccess.NoQuery(query, con, transact, lstp.ToArray)

                    query = "INSERT INTO CUBOS.CCPRODPLAN (EMPRESA, PLANTA, PORTADOR, PLANTIDSAB, DESCPLAN) VALUES (?, ?, ?, ?, ?)"
                    For Each empresa In empresasProductivas
                        lstp = New List(Of OleDb.OleDbParameter)
                        lstp.Add(New OleDb.OleDbParameter("empresa", ccMetadata.Empresa))
                        lstp.Add(New OleDb.OleDbParameter("planta", ccMetadata.Planta))
                        lstp.Add(New OleDb.OleDbParameter("codigoPortador", ccMetadata.CodigoPortador))
                        lstp.Add(New OleDb.OleDbParameter("plantIdSAB", empresa.IdPlantaSAB))
                        lstp.Add(New OleDb.OleDbParameter("descPlan", empresa.DescripcionPlanta))

                        Memcached.OleDbDirectAccess.NoQuery(query, con, transact, lstp.ToArray)
                    Next
                End If

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSabResponsable"></param>
        ''' <param name="portadorCoste"></param>
        Public Shared Sub UpdateResponsible(ByVal sabreponsable As String, ByVal idSabResponsable As Integer, ByVal portadorCoste As String)
            Dim query As String = "UPDATE CUBOS.CCHEADER SET RESPONSAB=?, RESPIDSAB=? WHERE UPPER(TRIM(PORTADOR)) LIKE ?"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("responsab", sabreponsable))
            lstp.Add(New OleDb.OleDbParameter("respidsab", idSabResponsable))
            lstp.Add(New OleDb.OleDbParameter("codigoPortador", portadorCoste.Trim().ToUpper() & "%"))

            Memcached.OleDbDirectAccess.NoQuery(query, CadenaConexionBRAIN, lstp.ToArray())
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        Public Shared Sub Delete(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String)
            Dim con As OleDb.OleDbConnection = Nothing
            Dim transact As OleDb.OleDbTransaction = Nothing
            Dim query As String = String.Empty

            Try
                con = New OleDb.OleDbConnection(CadenaConexionBRAIN)
                con.Open()
                transact = con.BeginTransaction()

                Dim lstp As New List(Of OleDb.OleDbParameter)
                lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
                lstp.Add(New OleDb.OleDbParameter("planta", planta))
                lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador))

                ' 1- Borramos los valores de año
                query = "DELETE FROM CUBOS.CCYEAR WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ?"
                Memcached.OleDbDirectAccess.NoQuery(query, con, transact, lstp.ToArray())

                lstp = New List(Of OleDb.OleDbParameter)
                lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
                lstp.Add(New OleDb.OleDbParameter("planta", planta))
                lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador))

                ' 2- Borramos los valores de empresas productivas
                query = "DELETE FROM CUBOS.CCPRODPLAN WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ?"
                Memcached.OleDbDirectAccess.NoQuery(query, con, transact, lstp.ToArray())

                lstp = New List(Of OleDb.OleDbParameter)
                lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
                lstp.Add(New OleDb.OleDbParameter("planta", planta))
                lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador))

                ' 3- Borramos el cost carrier
                query = "DELETE FROM CUBOS.CCHEADER WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ?"
                Memcached.OleDbDirectAccess.NoQuery(query, con, transact, lstp.ToArray)

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

#End Region

    End Class

End Namespace