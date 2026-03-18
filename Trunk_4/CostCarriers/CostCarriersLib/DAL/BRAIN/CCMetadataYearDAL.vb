Imports System.Globalization

Namespace DAL.BRAIN

    Public Class CCMetadataYearDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <param name="anyo"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String, ByVal anyo As Integer) As ELL.BRAIN.CCMetadataYear
            Dim query As String = "SELECT * FROM CUBOS.CCYEAR WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ? AND ANNE = ?"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))
            lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador))
            lstp.Add(New OleDb.OleDbParameter("anyo", anyo))

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.BRAIN.CCMetadataYear)(Function(r As OleDb.OleDbDataReader) _
            New ELL.BRAIN.CCMetadataYear With {.Empresa = CStr(r("EMPRESA")), .Planta = CStr(r("PLANTA")), .Anyo = CInt(r("ANNE")), .CodigoPortador = CStr(r("PORTADOR")),
                                               .CodigoMoneda = CInt(r("CODMON")), .Moneda = CStr(r("MONEDA")), .PresupBonosPersona = CDec(r("PRESUBONOS")), .PresupFacturas = CDec(r("PRESUFACT")),
                                               .PresupViajes = CDec(r("PRESUVIAJE")), .ImporteVentaCliente = CDec(r("IMPVENTA"))}, query, CadenaConexionBRAIN, lstp.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <returns></returns>
        Public Shared Function loadList(Optional ByVal empresa As String = Nothing, Optional ByVal planta As String = Nothing, Optional ByVal codigoPortador As String = Nothing) As List(Of ELL.BRAIN.CCMetadataYear)
            Dim query As String = "SELECT * FROM CUBOS.CCYEAR{0}"
            Dim where As String = String.Empty

            Dim lstp As New List(Of OleDb.OleDbParameter)
            If (Not String.IsNullOrEmpty(empresa)) Then
                where = " WHERE EMPRESA = ?"
                lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            End If

            If (Not String.IsNullOrEmpty(planta)) Then
                If (String.IsNullOrEmpty(where)) Then
                    where = " WHERE PLANTA = ?"
                Else
                    where &= " AND PLANTA = ?"
                End If

                lstp.Add(New OleDb.OleDbParameter("planta", planta))
            End If

            If (Not String.IsNullOrEmpty(codigoPortador)) Then
                If (String.IsNullOrEmpty(where)) Then
                    where = " WHERE PORTADOR = ?"
                Else
                    where &= " AND PORTADOR = ?"
                End If

                lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador))
            End If

            query = String.Format(query, where)

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.BRAIN.CCMetadataYear)(Function(r As OleDb.OleDbDataReader) _
            New ELL.BRAIN.CCMetadataYear With {.Empresa = CStr(r("EMPRESA")), .Planta = CStr(r("PLANTA")), .Anyo = CInt(r("ANNE")), .CodigoPortador = CStr(r("PORTADOR")),
                                               .CodigoMoneda = CInt(r("CODMON")), .Moneda = CStr(r("MONEDA")), .PresupBonosPersona = CDec(r("PRESUBONOS")), .PresupFacturas = CDec(r("PRESUFACT")),
                                               .PresupViajes = CDec(r("PRESUVIAJE")), .ImporteVentaCliente = CDec(r("IMPVENTA"))}, query, CadenaConexionBRAIN, lstp.ToArray())
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda
        ''' </summary>
        ''' <param name="ccMetadataYear"></param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal ccMetadataYear As ELL.BRAIN.CCMetadataYear)
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = (getObject(ccMetadataYear.Empresa, ccMetadataYear.Planta, ccMetadataYear.CodigoPortador, ccMetadataYear.Anyo) Is Nothing)

            ' Guardamos
            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("codigoMoneda", ccMetadataYear.CodigoMoneda))
            lstp.Add(New OleDb.OleDbParameter("moneda", ccMetadataYear.Moneda))
            lstp.Add(New OleDb.OleDbParameter("presupBonosPersona", ccMetadataYear.PresupBonosPersona))
            lstp.Add(New OleDb.OleDbParameter("presupFacturas", ccMetadataYear.PresupFacturas))
            lstp.Add(New OleDb.OleDbParameter("presupViajes", ccMetadataYear.PresupViajes))
            lstp.Add(New OleDb.OleDbParameter("importeVentaCliente", If(ccMetadataYear.ImporteVentaCliente = Decimal.MinValue, Decimal.Zero, ccMetadataYear.ImporteVentaCliente)))
            lstp.Add(New OleDb.OleDbParameter("empresa", ccMetadataYear.Empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", ccMetadataYear.Planta))
            lstp.Add(New OleDb.OleDbParameter("anyo", ccMetadataYear.Anyo))
            lstp.Add(New OleDb.OleDbParameter("codigoPortador", ccMetadataYear.CodigoPortador))

            If (bNuevo) Then
                query = "INSERT INTO CUBOS.CCYEAR (CODMON, MONEDA, PRESUBONOS, PRESUFACT, PRESUVIAJE, IMPVENTA, EMPRESA, " _
                        & "PLANTA, ANNE, PORTADOR) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?)"
            Else
                query = "UPDATE CUBOS.CCYEAR SET CODMON = ?, MONEDA = ?, PRESUBONOS = ?, PRESUFACT = ?, PRESUVIAJE = ?, IMPVENTA = ? " _
                        & "WHERE EMPRESA = ? AND PLANTA = ? AND ANNE = ? AND PORTADOR = ?"
            End If

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
        ''' <param name="anyo"></param>
        Public Shared Sub Delete(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String, ByVal anyo As Integer)
            Dim query As String = "DELETE FROM CUBOS.CCYEAR WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ? AND ANNE = ?"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))
            lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador))
            lstp.Add(New OleDb.OleDbParameter("anyo", anyo))

            Memcached.OleDbDirectAccess.NoQuery(query, CadenaConexionBRAIN, lstp.ToArray())
        End Sub

        ''' <summary>
        ''' Elimina
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        Public Shared Sub Delete(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String)
            Dim query As String = "DELETE FROM CUBOS.CCYEAR WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ?"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))
            lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador))

            Memcached.OleDbDirectAccess.NoQuery(query, CadenaConexionBRAIN, lstp.ToArray())
        End Sub

#End Region

    End Class

End Namespace