Namespace BRAIN.DAL

    Public Class AlbaranesDAL
        Inherits AutoFactuProvLib.DAL.DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un albarán
        ''' </summary>
        ''' <param name="albaran"></param>
        ''' <param name="pedido"></param>
        ''' <param name="linea"></param>
        ''' <returns></returns>
        Public Shared Function getItem(ByVal albaran As String, ByVal pedido As Integer, ByVal linea As Integer) As ELL.Albaran
            Dim query As String = "SELECT * FROM CUBOS.ALBPFATOT WHERE ALLIES=? AND ALBENR=? AND ALBEPO=?"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("albaran", albaran))
            lstp.Add(New OleDb.OleDbParameter("pedido", pedido))
            lstp.Add(New OleDb.OleDbParameter("linea", linea))

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.Albaran)(Function(r As OleDb.OleDbDataReader) _
            New ELL.Albaran With {.Empresa = r("ALFIRM"), .Planta = r("ALWKNR"), .Proveedor = CInt(r("ALLINR")), .NombreProveedor = r("ALNAME"),
                                  .Tipo = r("ALTIPO"), .Albaran = r("ALLIES"), .Pedido = CInt(r("ALBENR")), .Linea = CInt(r("ALBEPO")), .CantRecibida = CDec(r("ALLIMG")),
                                  .PrecioUnitario = CDec(r("ALPREC")) / 1000, .Moneda = r("ALWACD"), .Concepto = r("ALBEZ1"), .CantPendiente = CDec(r("ALCPTE")),
                                  .Solicitante = r("ALANFA"), .RefArticulo = r("ALTENR")}, query, CadenaConexionBRAIN, lstp.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de albaranes para un proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="tipo">A: albaranes pendientes
        ''' P: pedidos pendientes de recepcionar
        ''' Nothing: todos</param> 
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListPendRecep(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String, Optional ByVal tipo As String = Nothing) As List(Of ELL.Albaran)
            Dim query As String = "SELECT * FROM CUBOS.ALBPFATOT WHERE ALLINR=? AND ALFIRM=? AND ALWKNR=? AND ALTIPO='P' ORDER BY ALLIES, ALBENR, ALBEPO"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("proveedor", proveedor))
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.Albaran)(Function(r As OleDb.OleDbDataReader) _
            New ELL.Albaran With {.Empresa = r("ALFIRM"), .Planta = r("ALWKNR"), .Proveedor = CInt(r("ALLINR")), .NombreProveedor = r("ALNAME"),
                                  .Tipo = r("ALTIPO"), .Albaran = r("ALLIES"), .Pedido = CInt(r("ALBENR")), .Linea = CInt(r("ALBEPO")), .CantRecibida = CDec(r("ALLIMG")),
                                  .PrecioUnitario = CDec(r("ALPREC")) / 1000, .Moneda = r("ALWACD"), .Concepto = r("ALBEZ1"), .CantPendiente = CDec(r("ALCPTE")),
                                  .Solicitante = r("ALANFA"), .RefArticulo = r("ALTENR")}, query, CadenaConexionBRAIN, lstp.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene un listado de albaranes para un proveedor facturables por Batz
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListFacturablesBatz(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.Albaran)
            Dim query As String = "SELECT * FROM CUBOS.ALBPFATOT ALB " _
                                  & "INNER JOIN CUBOS.CONTRATOSCOMPRA BE ON BE.BOFIRM=ALB.ALFIRM AND BE.BOWKNR=ALB.ALWKNR AND BE.BOLINR=ALB.ALLINR AND BE.BOTENR=ALB.ALTENR " _
                                  & "WHERE ALB.ALLINR=? AND ALB.ALFIRM=? AND ALB.ALWKNR=? AND ALB.ALTIPO='A' AND BE.BOARAR='02' AND ALB.ALWEDA>=BE.BOGUVO AND ALB.ALWEDA<=BE.BOGUBI " _
                                  & "ORDER BY ALB.ALLIES, ALB.ALBENR, ALB.ALBEPO"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("proveedor", proveedor))
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.Albaran)(Function(r As OleDb.OleDbDataReader) _
            New ELL.Albaran With {.Empresa = r("ALFIRM"), .Planta = r("ALWKNR"), .Proveedor = CInt(r("ALLINR")), .NombreProveedor = r("ALNAME"),
                                  .Tipo = r("ALTIPO"), .Albaran = r("ALLIES"), .Pedido = CInt(r("ALBENR")), .Linea = CInt(r("ALBEPO")), .CantRecibida = CDec(r("ALLIMG")),
                                  .PrecioUnitario = CDec(r("ALPREC")) / 1000, .Moneda = r("ALWACD"), .Concepto = r("ALBEZ1"), .CantPendiente = CDec(r("ALCPTE")),
                                  .Solicitante = r("ALANFA"), .RefArticulo = r("ALTENR")}, query, CadenaConexionBRAIN, lstp.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene un listado de albaranes para un proveedor facturables por el proveedor
        ''' </summary>
        ''' <param name="proveedor"></param> 
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListFacturablesProveedor(ByVal proveedor As Integer, ByVal empresa As String, ByVal planta As String) As List(Of ELL.Albaran)
            Dim query As String = "SELECT ALB1.* FROM CUBOS.ALBPFATOT ALB1 " _
                                  & "WHERE ALB1.ALLINR=? AND ALB1.ALFIRM=? AND ALB1.ALWKNR=? AND ALB1.ALTIPO='A' " _
                                  & "AND CONCAT(ALB1.ALFIRM, CONCAT(ALB1.ALWKNR, CONCAT(ALB1.ALLINR, CONCAT(ALB1.ALTENR, CONCAT(ALB1.ALWENR,ALB1.ALBENR))))) " _
                                  & "NOT IN " _
                                  & "(SELECT CONCAT(ALB.ALFIRM, CONCAT(ALB.ALWKNR, CONCAT(ALB.ALLINR, CONCAT(ALB.ALTENR, CONCAT(ALB.ALWENR,ALB.ALBENR))))) " _
                                  & "FROM CUBOS.ALBPFATOT ALB " _
                                  & "INNER JOIN CUBOS.CONTRATOSCOMPRA BE ON BE.BOFIRM=ALB.ALFIRM AND BE.BOWKNR=ALB.ALWKNR AND BE.BOLINR=ALB.ALLINR AND BE.BOTENR=ALB.ALTENR " _
                                  & "WHERE ALB.ALLINR=? AND ALB.ALFIRM=? AND ALB.ALWKNR=? AND ALB.ALTIPO='A' AND BE.BOARAR='02' AND ALB.ALWEDA>=BE.BOGUVO AND ALB.ALWEDA<=BE.BOGUBI)"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("proveedor", proveedor))
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))
            lstp.Add(New OleDb.OleDbParameter("proveedor1", proveedor))
            lstp.Add(New OleDb.OleDbParameter("empresa1", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta1", planta))

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.Albaran)(Function(r As OleDb.OleDbDataReader) _
            New ELL.Albaran With {.Empresa = r("ALFIRM"), .Planta = r("ALWKNR"), .Proveedor = CInt(r("ALLINR")), .NombreProveedor = r("ALNAME"),
                                  .Tipo = r("ALTIPO"), .Albaran = r("ALLIES"), .Pedido = CInt(r("ALBENR")), .Linea = CInt(r("ALBEPO")), .CantRecibida = CDec(r("ALLIMG")),
                                  .PrecioUnitario = CDec(r("ALPREC")) / 1000, .Moneda = r("ALWACD"), .Concepto = r("ALBEZ1"), .CantPendiente = CDec(r("ALCPTE")),
                                  .Solicitante = r("ALANFA"), .RefArticulo = r("ALTENR")}, query, CadenaConexionBRAIN, lstp.ToArray())
        End Function

#End Region

    End Class

End Namespace