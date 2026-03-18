Namespace DAL.Navision

    Public Class CostCarriersDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal codigo As String) As ELL.BRAIN.CostCarrier
            Dim query As String = "SELECT 'Z' AS EMPRESA, '000' AS PLANTA, [Code] AS VALOR, [Name] AS DENOM
                                   FROM ZCOGNOS_DIM_PROYECTO
                                   WHERE [Global Dimension No_] = 2 AND [Blocked] = 0 AND UPPER([Code]) = '" & codigo.ToUpper() & "'"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.BRAIN.CostCarrier)(Function(r As SqlClient.SqlDataReader) _
            New ELL.BRAIN.CostCarrier With {.Empresa = r("EMPRESA"), .Planta = r("PLANTA"), .Valor = r("VALOR").Replace(vbCrLf, ""), .Denominacion = r("DENOM").Replace(vbCrLf, ""), .Lantegi = String.Empty,
                                            .FechaApertura = DateTime.Today, .Datos = String.Empty}, query, CadenaConexionNavisionZamudio, Nothing).FirstOrDefault
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal codigo As String) As List(Of ELL.BRAIN.CostCarrier)
            Dim query As String = "SELECT 'Z' AS EMPRESA, '000' AS PLANTA, [Code] AS VALOR, [Name] AS DENOM
                                   FROM ZCOGNOS_DIM_PROYECTO
                                   WHERE [Global Dimension No_] = 2 AND [Blocked] = 0 AND UPPER([Code]) LIKE '%" & codigo.ToUpper() & "%'"

            Return Memcached.SQLServerDirectAccess.Seleccionar(Of ELL.BRAIN.CostCarrier)(Function(r As SqlClient.SqlDataReader) _
            New ELL.BRAIN.CostCarrier With {.Empresa = r("EMPRESA"), .Planta = r("PLANTA"), .Valor = r("VALOR").Replace(vbCrLf, ""), .Denominacion = r("DENOM").Replace(vbCrLf, ""), .Lantegi = String.Empty,
                                            .FechaApertura = DateTime.Today, .Datos = String.Empty}, query, CadenaConexionNavisionZamudio, Nothing)
        End Function

#End Region

    End Class

End Namespace