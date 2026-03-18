Imports Oracle.DataAccess.Client

Namespace DAL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class ValidadoresDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="empresa"></param>
        ''' <param name="planta"></param>
        ''' <returns></returns>
        Public Shared Function getObject(ByVal empresa As String, ByVal planta As String) As ELL.Validador
            Dim query As String = "SELECT * FROM VVALIDADORES WHERE EMPRESA=:EMPRESA AND PLANTA=:PLANTA"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("EMPRESA", OracleDbType.NVarchar2, empresa, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PLANTA", OracleDbType.NVarchar2, planta, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Validador)(Function(r As OracleDataReader) _
            New ELL.Validador With {.Empresa = CStr(r("EMPRESA")), .Planta = CStr(r("PLANTA")), .IdDirectorIngenieria = CInt(r("ID_DIRECTOR_INGENIERIA")),
                                    .IdGerente = CInt(r("ID_GERENTE")), .IdFinanciero = CInt(r("ID_FINANCIERO")), .DirectorIngenieria = CStr(r("DIRECTOR_INGENIERIA")),
                                    .EmailDirectorIngenieria = CStr(r("EMAIL_DIRECTOR_INGENIERIA")), .Gerente = CStr(r("GERENTE")), .EmailGerente = CStr(r("EMAIL_GERENTE")),
                                    .Financiero = CStr(r("FINANCIERO")), .EmailFinanciero = CStr(r("EMAIL_FINANCIERO"))}, query, CadenaConexion, lParameters.ToArray()).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <returns></returns>
        Public Shared Function loadList() As List(Of ELL.Validador)
            Dim query As String = "SELECT * FROM VVALIDADORES"

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Validador)(Function(r As OracleDataReader) _
            New ELL.Validador With {.Empresa = CStr(r("EMPRESA")), .Planta = CStr(r("PLANTA")), .IdDirectorIngenieria = CInt(r("ID_DIRECTOR_INGENIERIA")),
                                    .IdGerente = CInt(r("ID_GERENTE")), .IdFinanciero = CInt(r("ID_FINANCIERO")), .DirectorIngenieria = CStr(r("DIRECTOR_INGENIERIA")),
                                    .EmailDirectorIngenieria = CStr(r("EMAIL_DIRECTOR_INGENIERIA")), .Gerente = CStr(r("GERENTE")), .EmailGerente = CStr(r("EMAIL_GERENTE")),
                                    .Financiero = CStr(r("FINANCIERO")), .EmailFinanciero = CStr(r("EMAIL_FINANCIERO"))}, query, CadenaConexion)
        End Function

#End Region

    End Class

End Namespace