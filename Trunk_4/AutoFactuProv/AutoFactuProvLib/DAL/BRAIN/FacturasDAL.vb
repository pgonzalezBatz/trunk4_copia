Namespace BRAIN.DAL

    Public Class FacturasDAL
        Inherits AutoFactuProvLib.DAL.DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una factura. Sólo en estado 51
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="proveedor"></param>
        ''' <param name="año"></param>
        ''' <param name="factura"></param>
        ''' <returns></returns>
        Public Shared Function getItem(ByVal empresa As String, ByVal proveedor As Integer, ByVal año As Integer, ByVal factura As Integer) As ELL.Factura
            Dim query As String = "SELECT REFIRM, REWKNR, RELINR, REBLNR, REBLDA, CAST(REWAWT AS CHAR(20)) AS REWAWT, REWACD, CAST(REBRWT AS CHAR(20)) AS REBRWT, CAST(REMWWT AS CHAR(20)) AS REMWWT, RERNLI FROM CUBOS.CABFACTCOMPRA WHERE REFIRM=? AND RELINR=? AND REBUJA=? AND REBLNR=? AND RESTAT ='51'"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("proveedor", proveedor))
            lstp.Add(New OleDb.OleDbParameter("anyo", año))
            lstp.Add(New OleDb.OleDbParameter("factura", factura))

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.Factura)(Function(r As OleDb.OleDbDataReader) _
            New ELL.Factura With {.Empresa = r("REFIRM"), .Planta = r("REWKNR"), .Proveedor = CInt(r("RELINR")), .NumFactura = CInt(r("REBLNR")),
                                  .FechaAlta = DateTime.ParseExact(r("REBLDA"), "yyyyMMdd", Globalization.CultureInfo.InvariantCulture), .ImporteSinIVA = r("REWAWT"),
                                  .Moneda = r("REWACD"), .TotalFactura = r("REBRWT"), .ImporteIVA = r("REMWWT"), .Factura = r("RERNLI")}, query, CadenaConexionBRAIN, lstp.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de facturas autofacturadas para un proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.Factura)
            Dim query As String = "SELECT * FROM CUBOS.CABFACTCOMPRA WHERE RELINR=? AND REFIRM=? AND REWKNR=? AND REREAR='32' AND RESTAT='99' ORDER BY REBLDA"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("proveedor", proveedor))
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))


            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.Factura)(Function(r As OleDb.OleDbDataReader) _
            New ELL.Factura With {.Empresa = r("REFIRM"), .Planta = r("REWKNR"), .Proveedor = CInt(r("RELINR")), .NumFactura = CInt(r("REBLNR")),
                                  .FechaAlta = DateTime.ParseExact(r("REBLDA"), "yyyyMMdd", Globalization.CultureInfo.InvariantCulture), .ImporteSinIVA = CDec(r("REWAWT")),
                                  .Moneda = r("REWACD"), .TotalFactura = CDec(r("REBRWT")), .ImporteIVA = CDec(r("REMWWT"))}, query, CadenaConexionBRAIN, lstp.ToArray())
        End Function

#End Region

    End Class

End Namespace