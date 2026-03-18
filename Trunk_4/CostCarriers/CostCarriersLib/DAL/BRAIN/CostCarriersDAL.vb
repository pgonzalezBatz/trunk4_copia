Namespace DAL.BRAIN

    Public Class CostCarriersDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="empresa"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal codigo As String, ByVal empresa As String) As ELL.BRAIN.CostCarrier
            Dim query As String = "SELECT EMPRESA,PLANTA,VALOR,DENOM,LANTEGI,DATOS,FECHA AS FAPERTURA " _
                                  & "FROM CUBOS.T_44N " _
                                  & "WHERE EMPRESA=? AND UPPER(VALOR)=?"
            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("codigo", codigo.ToUpper))

            Return Memcached.OleDbDirectAccess.Seleccionar(Of ELL.BRAIN.CostCarrier)(Function(r As OleDb.OleDbDataReader) _
            New ELL.BRAIN.CostCarrier With {.Empresa = r("EMPRESA"), .Planta = r("PLANTA"), .Valor = r("VALOR"), .Denominacion = r("DENOM"), .Lantegi = r("LANTEGI"),
                                            .FechaApertura = DateTime.ParseExact(r("FAPERTURA"), "yyyyMMdd", Nothing), .Datos = SabLib.BLL.Utils.stringNull(r("DATOS"))}, query, CadenaConexionBRAIN, lstp.ToArray).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="empresa"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal codigo As String, ByVal empresa As String) As List(Of ELL.BRAIN.CostCarrier)
            Dim query As String = "SELECT EMPRESA, PLANTA, VALOR, DENOM, LANTEGI, DATOS, FECHA AS FAPERTURA " _
                                  & "FROM CUBOS.T_44N " _
                                  & "WHERE EMPRESA=? AND UPPER(VALOR) LIKE ?"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("codigo", "%" & codigo.ToUpper() & "%"))

            Return Memcached.OleDbDirectAccess.Seleccionar(Of ELL.BRAIN.CostCarrier)(Function(r As OleDb.OleDbDataReader) _
            New ELL.BRAIN.CostCarrier With {.Empresa = r("EMPRESA"), .Planta = r("PLANTA"), .Valor = r("VALOR"), .Denominacion = r("DENOM"), .Lantegi = r("LANTEGI"),
                                            .FechaApertura = DateTime.ParseExact(r("FAPERTURA"), "yyyyMMdd", Nothing), .Datos = SabLib.BLL.Utils.stringNull(r("DATOS"))}, query, CadenaConexionBRAIN, lstp.ToArray())
        End Function

#End Region

    End Class

End Namespace