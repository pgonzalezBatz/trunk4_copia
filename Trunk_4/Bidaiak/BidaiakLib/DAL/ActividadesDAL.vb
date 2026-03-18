Namespace DAL

    Public Class ActividadesDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Obtiene la conexion
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>        
        Public ReadOnly Property Conexion As String
            Get
                Dim status As String = "BIDAIAKTEST"
                If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
                Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            End Get
        End Property

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            cn = Conexion
        End Sub

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de una actividad
        ''' </summary>
        ''' <param name="id">Id</param>        
        ''' <returns></returns>        
        Public Function loadInfo(ByVal id As Integer) As ELL.Actividad
            Try
                Dim query As String = "SELECT ID,NOMBRE,EXENTA,REQ_TEXTO,OBSOLETA,ID_PLANTA,PAP FROM ACTIVIDADES_IRPF WHERE ID=:ID"

                Dim lActiv As List(Of ELL.Actividad) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Actividad)(Function(r As OracleDataReader) _
                 New ELL.Actividad With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .ExentaIRPF = CType(r("EXENTA"), Boolean), .RequiereTexto = CType(r("REQ_TEXTO"), Boolean),
                                         .Obsoleta = CType(r("OBSOLETA"), Boolean), .IdPlanta = CInt(r("ID_PLANTA")), .PuestaAPunto = SabLib.BLL.Utils.booleanNull(r("PAP"))},
                 query, cn, (New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)))

                Dim oActiv As ELL.Actividad = Nothing
                If (lActiv IsNot Nothing AndAlso lActiv.Count > 0) Then oActiv = lActiv.First
                Return oActiv
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la actividad", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de actividades
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bExentas">Indica si se quieren obtener las exentas, las no exentas o todas</param>
        ''' <param name="bIncluirObsoletos">Indica si se incluiran los obsoletos</param>
        ''' <param name="textFilter">Texto de la actividad a buscar</param>
        ''' <returns></returns>  
        Public Function loadList(ByVal idPlanta As Integer, Optional ByVal bExentas As Nullable(Of Boolean) = Nothing, Optional ByVal bIncluirObsoletos As Boolean = True, Optional ByVal textFilter As String = "") As List(Of ELL.Actividad)
            Try
                Dim query As String = "SELECT ID,NOMBRE,EXENTA,REQ_TEXTO,OBSOLETA,ID_PLANTA,PAP FROM ACTIVIDADES_IRPF WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (Not bIncluirObsoletos) Then query &= " AND OBSOLETA=0"
                If (bExentas.HasValue) Then
                    query &= If(bExentas.Value, " AND EXENTA=1", " AND EXENTA=0")
                End If
                If (textFilter <> String.Empty) Then
                    query &= " AND LOWER(NOMBRE) LIKE :NOMBRE"
                    lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, "%" & textFilter.ToLower & "%", ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Actividad)(Function(r As OracleDataReader) _
                New ELL.Actividad With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .ExentaIRPF = CType(r("EXENTA"), Boolean), .RequiereTexto = CType(r("REQ_TEXTO"), Boolean),
                                         .Obsoleta = CType(r("OBSOLETA"), Boolean), .IdPlanta = CInt(r("ID_PLANTA")), .PuestaAPunto = SabLib.BLL.Utils.booleanNull(r("PAP"))}, query, cn, lParametros.ToArray)

            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de actividades", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de actividades asociados a uno o a todos los departamento
        ''' </summary>
        ''' <param name="dpto">Departamento</param>
        ''' <param name="idPlanta">Id de la planta</param>    
        ''' <param name="tipoExentas">Tipo de las exentas a mostrar. 0:todas,1:Exentas solo,2:No exentas solo</param>    
        ''' <returns></returns>  
        Public Function loadListDpto(ByVal dpto As String, ByVal idPlanta As Integer, ByVal tipoExentas As Integer) As List(Of ELL.Actividad)
            Try
                Dim query As String = "SELECT AI.ID,AI.NOMBRE,AI.EXENTA,AI.REQ_TEXTO,AI.OBSOLETA,AI.ID_PLANTA,AI.PAP FROM ACTIVIDADES_IRPF AI LEFT JOIN ACTIVIDADES_DPTO AD ON AI.ID=AD.ID_ACTIVIDAD WHERE ID_PLANTA=:ID_PLANTA AND OBSOLETA=0"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (dpto = String.Empty) Then
                    query &= " AND AD.ID_DPTO IS NULL"
                Else
                    query &= " AND (CAST(AD.ID_DPTO AS INTEGER)=:ID_DPTO OR AD.ID_DPTO IS NULL)"
                    lParametros.Add(New OracleParameter("ID_DPTO", OracleDbType.Int32, CInt(dpto), ParameterDirection.Input))
                End If
                If (tipoExentas > 0) Then
                    query &= " AND AI.EXENTA=:EXENTO"
                    lParametros.Add(New OracleParameter("EXENTO", OracleDbType.Int32, If(tipoExentas = 1, 1, 0), ParameterDirection.Input))
                End If

                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Actividad)(Function(r As OracleDataReader) _
                New ELL.Actividad With {.Id = CInt(r("ID")), .Nombre = r("NOMBRE"), .ExentaIRPF = CType(r("EXENTA"), Boolean), .RequiereTexto = CType(r("REQ_TEXTO"), Boolean),
                                         .Obsoleta = CType(r("OBSOLETA"), Boolean), .IdPlanta = CInt(r("ID_PLANTA")), .PuestaAPunto = SabLib.BLL.Utils.booleanNull(r("PAP"))}, query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de actividades de un departamento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los departamentos asociados a una actividad
        ''' </summary>
        ''' <param name="idActividad">Id de la actividad</param>
        ''' <returns></returns>        
        Public Function loadDepartamentosActividad(ByVal idActividad As Integer, ByVal bGetDepartName As Boolean) As List(Of SabLib.ELL.Departamento)
            Try
                Dim query As String = "SELECT AD.ID_DPTO,A.ID_PLANTA FROM ACTIVIDADES_DPTO AD INNER JOIN ACTIVIDADES_IRPF A ON AD.ID_ACTIVIDAD=A.ID WHERE AD.ID_ACTIVIDAD=:ID_ACTIVIDAD"
                Dim dptoBLL As New SabLib.BLL.DepartamentosComponent
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_ACTIVIDAD", OracleDbType.Int32, idActividad, ParameterDirection.Input))

                If (bGetDepartName) Then
                    Return Memcached.OracleDirectAccess.seleccionar(Of SabLib.ELL.Departamento)(Function(r As OracleDataReader) _
                    dptoBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = r("ID_DPTO"), .IdPlanta = CInt(r("ID_PLANTA"))}),
                    query, cn, lParametros.ToArray)
                Else
                    Return Memcached.OracleDirectAccess.seleccionar(Of SabLib.ELL.Departamento)(Function(r As OracleDataReader) _
                    New SabLib.ELL.Departamento With {.Id = r("ID_DPTO"), .IdPlanta = CInt(r("ID_PLANTA"))},
                    query, cn, lParametros.ToArray)
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de actividades", ex)
            End Try
        End Function

        ''' <summary>
        ''' Comprueba si esa actividad tienen personas relacionadas
        ''' </summary>
        ''' <param name="idActividad">Id de la actividad</param>
        ''' <returns></returns>  
        Public Function tieneIntegrantesRelacionados(ByVal idActividad As Integer) As Boolean
            Try
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim query As String = "SELECT COUNT(*) FROM INTEGRANTES WHERE ID_ACTIVIRPF=:ID_ACTIVIDAD"
                parameter = New OracleParameter("ID_ACTIVIDAD", OracleDbType.Int32, idActividad, ParameterDirection.Input)

                Return (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, parameter) > 0)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al comprobar si tiene integrantes relacionados con la actividad", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los departamentos que no tienen asignado ninguna actividad
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function loadDepartamentosSinActividad(ByVal idPlanta As Integer) As List(Of String())
            Try
                Dim query As String = "SELECT D.ID,D.NOMBRE FROM DEPARTAMENTOS D LEFT JOIN ACTIVIDADES_DPTO AD ON D.ID=CAST(AD.ID_DPTO AS INTEGER) WHERE D.ID_PLANTA=:ID_PLANTA AND AD.ID_ACTIVIDAD IS NULL AND D.OBSOLETO=0 ORDER BY D.NOMBRE"
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de departamentos sin actividades", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Inserta o modifica la actividad
        ''' </summary>
        ''' <param name="oActiv">Objeto con la informacion</param>        
        Public Sub Save(ByVal oActiv As ELL.Actividad)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim idActividad As Integer = oActiv.Id
                Dim query As String = String.Empty
                myConnection = New OracleConnection(Me.cn)
                myConnection.Open()
                transact = myConnection.BeginTransaction()

                Dim lParametros As New List(Of OracleParameter)
                If (idActividad = Integer.MinValue) Then 'Insert                    
                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParametros.Add(p)
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oActiv.IdPlanta, ParameterDirection.Input))
                    query = "INSERT INTO ACTIVIDADES_IRPF(NOMBRE,EXENTA,REQ_TEXTO,OBSOLETA,ID_PLANTA,PAP) VALUES(:NOMBRE,:EXENTA,:REQ_TEXTO,0,:ID_PLANTA,:PAP) RETURNING ID INTO :RETURN_VALUE"
                Else 'update                    
                    lParametros.Add(New OracleParameter("OBSOLETA", OracleDbType.Int32, oActiv.Obsoleta, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oActiv.Id, ParameterDirection.Input))
                    query = "UPDATE ACTIVIDADES_IRPF SET NOMBRE=:NOMBRE,EXENTA=:EXENTA,REQ_TEXTO=:REQ_TEXTO,OBSOLETA=:OBSOLETA,PAP=:PAP WHERE ID=:ID"
                End If
                lParametros.Add(New OracleParameter("NOMBRE", OracleDbType.Varchar2, oActiv.Nombre, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("EXENTA", OracleDbType.Int32, oActiv.ExentaIRPF, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("REQ_TEXTO", OracleDbType.Int32, oActiv.RequiereTexto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("PAP", OracleDbType.Int32, oActiv.PuestaAPunto, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)

                'Se borra los departamentos a los que afecta
                If (idActividad <> Integer.MinValue) Then
                    query = "DELETE FROM ACTIVIDADES_DPTO WHERE ID_ACTIVIDAD=:ID_ACTIVIDAD"
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, New OracleParameter("ID_ACTIVIDAD", OracleDbType.Int32, idActividad, ParameterDirection.Input))
                Else
                    idActividad = CInt(lParametros.Item(0).Value)
                End If

                'Se añaden de nuevo
                If (oActiv.DepartamentosAfectados IsNot Nothing AndAlso oActiv.DepartamentosAfectados.Count > 0) Then
                    For Each oDpto As SabLib.ELL.Departamento In oActiv.DepartamentosAfectados
                        lParametros = New List(Of OracleParameter)
                        query = "INSERT INTO ACTIVIDADES_DPTO(ID_ACTIVIDAD,ID_DPTO) VALUES(:ID_ACTIVIDAD,:ID_DPTO)"
                        lParametros.Add(New OracleParameter("ID_ACTIVIDAD", OracleDbType.Int32, idActividad, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_DPTO", OracleDbType.Varchar2, oDpto.Id, ParameterDirection.Input))
                        Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                    Next
                End If

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al guardar la informacion", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then myConnection.Close()
            End Try
        End Sub

#End Region

    End Class

End Namespace