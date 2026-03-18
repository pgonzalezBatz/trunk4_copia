Imports System.Globalization

Namespace DAL.BRAIN

    Public Class CCProductionPlantDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <param name="codigoPortador"></param>
        ''' <returns></returns>
        Public Shared Function loadList(ByVal empresa As String, ByVal planta As String, ByVal codigoPortador As String) As List(Of ELL.BRAIN.CCProductionPlant)
            Dim query As String = "SELECT * FROM CUBOS.CCPRODPLAN WHERE EMPRESA = ? AND PLANTA = ? AND PORTADOR = ?"

            Dim lstp As New List(Of OleDb.OleDbParameter)
            lstp.Add(New OleDb.OleDbParameter("empresa", empresa))
            lstp.Add(New OleDb.OleDbParameter("planta", planta))
            lstp.Add(New OleDb.OleDbParameter("codigoPortador", codigoPortador))

            Return Memcached.OleDbDirectAccess.seleccionar(Of ELL.BRAIN.CCProductionPlant)(Function(r As OleDb.OleDbDataReader) _
            New ELL.BRAIN.CCProductionPlant With {.Empresa = CStr(r("EMPRESA")), .Planta = CStr(r("PLANTA")), .CodigoPortador = CStr(r("PORTADOR")),
                                                  .IdPlantaSAB = CInt(r("PLANTIDSAB")), .DescripcionPlanta = CStr(r("DESCPLAN"))}, query, CadenaConexionBRAIN, lstp.ToArray())
        End Function

#End Region

    End Class

End Namespace