Namespace DAL

    Public Class UnidadOrgDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            Dim status As String = "BIDAIAKTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
            cn = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de una unidadOrganizativa
        ''' </summary>
        ''' <param name="id">Id</param>        
        ''' <returns></returns>        
        Public Function load(ByVal id As Integer) As ELL.UnidadOrg
            Try
                Dim query As String = "SELECT ID,NOMBRE,ID_PLANTA,ID_RESPONSABLE,REQ_PROYCLI,STRING_CONEXION,DEPART_IDENTIF,REQ_CONSIN_PROYECTO,REQ_OF_IMPRODUCTIVA,REQ_OF_VALIDAR,SISTEMA FROM UNIDADES_ORG WHERE ID=:ID"
                parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)

                Dim lUnidades As List(Of ELL.UnidadOrg) = Memcached.OracleDirectAccess.seleccionar(Of ELL.UnidadOrg)(Function(r As OracleDataReader) _
                 New ELL.UnidadOrg With {.Id = CInt(r(0)), .Nombre = r(1), .IdPlanta = CInt(r(2)), .Responsable = New SabLib.ELL.Usuario With {.Id = CInt(r(3))}, .ReqProyCli = CBool(r(4)), .StringConexion = r(5), .DepartamentoIdentif = SabLib.BLL.Utils.stringNull(r(6)), .ReqConSinProyecto = CBool(r(7)), .ReqOFImproductiva = CBool(r(8)), .ReqOFValidar = CBool(r(9)), .Sistema = r(10)}, query, cn, parameter)

                Dim oUnidad As ELL.UnidadOrg = Nothing
                If (lUnidades IsNot Nothing AndAlso lUnidades.Count > 0) Then oUnidad = lUnidades.Item(0)
                Return oUnidad
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la unidad organizativa", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene una lista de unidades de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>		
        Public Function loadList(ByVal idPlanta As Integer) As List(Of ELL.UnidadOrg)
            Try
                Dim query As String = "SELECT ID,NOMBRE,ID_PLANTA,ID_RESPONSABLE,REQ_PROYCLI,STRING_CONEXION,DEPART_IDENTIF,REQ_CONSIN_PROYECTO,REQ_OF_IMPRODUCTIVA,REQ_OF_VALIDAR,SISTEMA FROM UNIDADES_ORG WHERE ID_PLANTA=:ID_PLANTA"
                parameter = New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.UnidadOrg)(Function(r As OracleDataReader) _
                New ELL.UnidadOrg With {.Id = CInt(r(0)), .Nombre = r(1), .IdPlanta = CInt(r(2)), .Responsable = New SabLib.ELL.Usuario With {.Id = CInt(r(3))}, .ReqProyCli = CBool(r(4)), .StringConexion = r(5),
                                        .DepartamentoIdentif = SabLib.BLL.Utils.stringNull(r(6)), .ReqConSinProyecto = CBool(r(7)), .ReqOFImproductiva = CBool(r(8)), .ReqOFValidar = CBool(r(9)), .Sistema = r(10)}, query, cn, If(parameter Is Nothing, Nothing, parameter))

            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de unidades organizativas", ex)
            End Try
        End Function

#End Region

    End Class

End Namespace