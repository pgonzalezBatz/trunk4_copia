Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class AccessDB

#Region "Variables/Constructor"

        Public cn As String
        Public transaction As OracleTransaction
        Public connection As OracleConnection
        Private query As String
        Private lParametros As List(Of OracleParameter) = Nothing

#End Region

#Region "Transacciones/Querys"

        ''' <summary>
        ''' Obtiene la conexion de oferta tecnica sis
        ''' </summary>
        ''' <returns></returns>
        Private Function GetConexionDesignFMEA() As String
            Dim status As String = "DESIGNFMEATEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "DESIGNFMEALIVE"
            Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Function

        ''' <summary>
        ''' Obtiene la conexion de brain
        ''' </summary>
        ''' <returns></returns>
        Private Function GetConexionBrain() As String
            Dim status As String = "BRAIN"
            Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Function

        ''' <summary>
        ''' Ejecuta una query dependiendo si es una transaccion o no
        ''' </summary>
        ''' <param name="f">Funcion a ejecutar</param>
        ''' <returns></returns>
        Public Function Seleccionar(f)
            If (connection IsNot Nothing) Then
                Return Memcached.OracleDirectAccess.Seleccionar(f, query, connection, If(lParametros Is Nothing, Nothing, lParametros.ToArray))
            Else
                Return Memcached.OracleDirectAccess.Seleccionar(f, query, GetConexionDesignFMEA, If(lParametros Is Nothing, Nothing, lParametros.ToArray))
            End If
        End Function

        ''' <summary>
        ''' Ejecuta una query dependiendo si es una transaccion o no
        ''' </summary>
        ''' <returns></returns>
        Public Function Seleccionar()
            If (connection IsNot Nothing) Then
                Return Memcached.OracleDirectAccess.Seleccionar(query, connection, If(lParametros Is Nothing, Nothing, lParametros.ToArray))
            Else
                Return Memcached.OracleDirectAccess.Seleccionar(query, GetConexionDesignFMEA, If(lParametros Is Nothing, Nothing, lParametros.ToArray))
            End If
        End Function

        ''' <summary>
        ''' Ejecuta una query y devuelve un escalar
        ''' </summary>        
        ''' <returns></returns>
        Public Function SeleccionarEscalar() As Integer
            If (connection IsNot Nothing) Then
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, connection, If(lParametros Is Nothing, Nothing, lParametros.ToArray))
            Else
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, GetConexionDesignFMEA, If(lParametros Is Nothing, Nothing, lParametros.ToArray))
            End If
        End Function

        ''' <summary>
        ''' Ejecuta la query
        ''' </summary>        
        Public Sub ExecuteQuery()
            If (connection IsNot Nothing) Then
                Memcached.OracleDirectAccess.NoQuery(query, connection, lParametros.ToArray)
            Else
                Memcached.OracleDirectAccess.NoQuery(query, GetConexionDesignFMEA, lParametros.ToArray)
            End If
        End Sub

        ''' <summary>
        ''' Indica si hay una transaccion abierta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function IsTransactionOpen()
            Return (transaction IsNot Nothing)
        End Function

        ''' <summary>
        ''' Abre una transaccion
        ''' </summary>        
        Public Function OpenTransaction() As Boolean
            Try
                If (transaction Is Nothing) Then 'Si ya esta abierta, no la vuelve a abrir
                    connection = Nothing
                    connection = New OracleConnection(GetConexionDesignFMEA)
                    connection.Open()
                    transaction = connection.BeginTransaction()
                End If
                Return True
            Catch ex As Exception
                connection = Nothing : transaction = Nothing
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Comite una transaccion
        ''' </summary>        
        Public Sub CommitTransaction()
            transaction.Commit()
            If (connection IsNot Nothing AndAlso connection.State <> ConnectionState.Closed) Then
                connection.Close()
                connection.Dispose()
            End If
            connection = Nothing : transaction = Nothing
        End Sub

        ''' <summary>
        ''' Comite una transaccion
        ''' </summary>        
        Public Sub RollBackTransaction()
            transaction.Rollback()
            If (connection IsNot Nothing AndAlso connection.State <> ConnectionState.Closed) Then
                connection.Close()
                connection.Dispose()
            End If
            connection = Nothing : transaction = Nothing
        End Sub

#End Region

#Region "AMFE"

        ''' <summary>
        ''' Carga la informacion del amfe
        ''' </summary>
        ''' <param name="id">Id</param>
        ''' <returns></returns>   
        Public Function loadAMFE(ByVal id As Integer) As ELL.Amfe
            query = "SELECT A.ID,A.ID_PROYECTO,A.ID_USER,A.F_CREACION,P.NOMBRE AS PROYECTO,P.PRODUCTO AS PRODUCTO,T.ID AS TIPO,T.NOMBRE FROM AMFE A INNER JOIN BONOSIS.PROYECTOS P ON A.ID_PROYECTO=P.ID LEFT JOIN LECCIONESAPRENDIDAS.TIPOLECCION T ON A.TIPO=T.ID WHERE A.ID=:ID "
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
            Return loadObjectAmfe().FirstOrDefault
        End Function

        ''' <summary>
        ''' Carga el listado de amfes
        ''' </summary>        
        ''' <param name="oAmfe">Informacion del tipo</param>
        ''' <returns></returns>        
        Public Function loadListAMFE(ByVal oAmfe As ELL.Amfe) As List(Of ELL.Amfe)
            query = "SELECT DISTINCT A.ID,A.ID_PROYECTO,A.ID_USER,A.F_CREACION,T.ID AS TIPO,T.NOMBRE,P.NOMBRE AS PROYECTO,P.PRODUCTO AS PRODUCTO FROM AMFE A INNER JOIN BONOSIS.PROYECTOS P ON A.ID_PROYECTO=P.ID LEFT JOIN LECCIONESAPRENDIDAS.TIPOLECCION T ON A.TIPO=T.ID"
            Dim where As String = String.Empty
            lParametros = New List(Of OracleParameter)
            If (oAmfe.IdProyecto <> String.Empty) Then
                where &= If(where <> String.Empty, " AND ", String.Empty) & "A.ID_PROYECTO=:ID_PROYECTO"
                lParametros.Add(New OracleParameter("ID_PROYECTO", OracleDbType.Varchar2, oAmfe.IdProyecto, ParameterDirection.Input))
            End If
            If (oAmfe.Producto <> String.Empty) Then
                where &= If(where <> String.Empty, " AND ", String.Empty) & "P.PRODUCTO=:PRODUCTO"
                lParametros.Add(New OracleParameter("PRODUCTO", OracleDbType.Varchar2, oAmfe.Producto, ParameterDirection.Input))
            End If
            If (oAmfe.IdUser > 0) Then
                where &= If(where <> String.Empty, " AND ", String.Empty) & "A.ID_USER=:ID_USER"
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oAmfe.IdUser, ParameterDirection.Input))
            End If
            If (oAmfe.Tipo > 0) Then
                where &= If(where <> String.Empty, " AND ", String.Empty) & "A.TIPO=:TIPO"
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, oAmfe.Tipo, ParameterDirection.Input))
            End If
            If (where <> String.Empty) Then
                query &= " WHERE " & where
            Else
                lParametros = Nothing
            End If
            Dim res = loadObjectAmfe(oAmfe.Tipo)
            Return res
        End Function

        ''' <summary>
        ''' Carga la lista de tipos de amfe
        ''' </summary>
        ''' <param name="tipo"></param>
        ''' <returns></returns>        
        Private Function loadObjectAmfe(Optional ByVal tipo As Integer = 0) As List(Of ELL.Amfe)
            Dim f As System.Func(Of OracleDataReader, ELL.Amfe) = Function(r As OracleDataReader) New ELL.Amfe With {.Id = CInt(r("ID")), .IdProyecto = r("ID_PROYECTO"), .IdUser = CInt(r("ID_USER")), .FechaCreacion = CDate(r("F_CREACION")), .Proyecto = r("PROYECTO"), .Producto = r("PRODUCTO"), .Tipo = SabLib.BLL.Utils.integerNull(r("TIPO")), .TipoString = SabLib.BLL.Utils.stringNull(r("NOMBRE"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oAmfe">Informacion del tipo</param>
        Public Function SaveAmfe(ByVal oAmfe As ELL.Amfe) As Integer
            Dim idAmfe As Integer = oAmfe.Id
            lParametros = New List(Of OracleParameter)
            If (idAmfe = 0) Then
                query = "INSERT INTO AMFE(ID_PROYECTO,ID_USER,F_CREACION,TIPO) VALUES(:ID_PROYECTO,:ID_USER,SYSDATE,:TIPO) RETURNING ID INTO :RETURN_VALUE"
                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParametros.Add(p)
                lParametros.Add(New OracleParameter("ID_PROYECTO", OracleDbType.NVarchar2, oAmfe.IdProyecto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, oAmfe.Tipo, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, oAmfe.IdUser, ParameterDirection.Input))
            Else
                'query = "UPDATE AMFE SET PERCENTAGE=:PERCENTAGE WHERE ID=:ID"
                'lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oCash.Id, ParameterDirection.Input))
            End If            
            ExecuteQuery()
            If (oAmfe.Id = 0) Then idAmfe = CInt(lParametros.Item(0).Value)
            Return idAmfe
        End Function

        ''' <summary>
        ''' Elimina el AMFE y todas sus referencias asociadas
        ''' </summary>
        ''' <param name="idAmfe">Id del amfe</param>        
        Public Sub DeleteAmfe(ByVal idAmfe As Integer)
            lParametros = New List(Of OracleParameter)
            query = "DELETE FROM AMFE WHERE ID=:ID"
            lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, idAmfe, ParameterDirection.Input))
            ExecuteQuery()
        End Sub


        ''' <summary>
        ''' Carga el listado de referencias
        ''' </summary>        
        ''' <param name="idAmfe">Id del amfe</param>
        ''' <returns></returns>        
        Public Function loadListReferencias(ByVal idAmfe As Integer) As List(Of ELL.Referencia)
            query = "SELECT REF,ID_EMPRESA FROM REFS_AMFE WHERE ID_AMFE=:ID_AMFE"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_AMFE", OracleDbType.Int32, idAmfe, ParameterDirection.Input))

            Dim f As System.Func(Of OracleDataReader, ELL.Referencia) = Function(r As OracleDataReader) New ELL.Referencia With {.Ref = r("REF"), .IdEmpresa = r("ID_EMPRESA")}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Añade una referencia a un amfe
        ''' </summary>
        ''' <param name="idAmfe">Id de amfe</param>
        ''' <param name="oRef">Referencia</param>        
        Public Sub AddReferencia(ByVal idAmfe As Integer, ByVal oRef As ELL.Referencia)
            query = "INSERT INTO REFS_AMFE(ID_AMFE,REF,ID_EMPRESA) VALUES (:ID_AMFE,:REF,:ID_EMPRESA)"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_AMFE", OracleDbType.Int32, idAmfe, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("REF", OracleDbType.Varchar2, oRef.Ref, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Varchar2, oRef.IdEmpresa, ParameterDirection.Input))            

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Elimina una referencia de un amfe
        ''' </summary>
        ''' <param name="idAmfe">Id del amfe</param>
        ''' <param name="oRef">Referencia</param>     
        Public Sub DeleteReferencia(ByVal idAmfe As Integer, ByVal oRef As ELL.Referencia)
            query = "DELETE FROM REFS_AMFE WHERE ID_AMFE=:ID_AMFE AND REF=:REF AND ID_EMPRESA=:ID_EMPRESA"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_AMFE", OracleDbType.Int32, idAmfe, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("REF", OracleDbType.Varchar2, oRef.Ref, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Varchar2, oRef.IdEmpresa, ParameterDirection.Input))

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Carga el listado de lecciones
        ''' </summary>        
        ''' <param name="idAmfe">Id del amfe</param>
        ''' <param name="ref">Referencia</param>
        ''' <param name="idEmpresa">Id empresa</param>
        ''' <returns></returns>        
        Public Function loadListLecciones(ByVal idAmfe As Integer, ByVal ref As String, ByVal idEmpresa As String, Optional ByVal tipo As Integer = 0) As List(Of ELL.Referencia.Leccion)
            query = "SELECT LR.ID_LECCION,LR.INCLUIDA,LR.COMENTARIO FROM LECCIONES_REF LR"
            Dim where = " WHERE ID_AMFE=:ID_AMFE AND REF=:REF AND ID_EMPRESA=:ID_EMPRESA"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_AMFE", OracleDbType.Int32, idAmfe, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("REF", OracleDbType.Varchar2, ref, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Varchar2, idEmpresa, ParameterDirection.Input))
            If (tipo <> 0) Then
                query &= " INNER JOIN LECCIONESAPRENDIDAS.LECCIONES2 LL ON LR.ID_LECCION = LL.IDLA"
                'where &= " AND LL.TIPOLA = :TIPOLA"
                'lParametros.Add(New OracleParameter("TIPOLA", OracleDbType.Int32, tipo, ParameterDirection.Input))
            End If
            query &= where
            Dim f As System.Func(Of OracleDataReader, ELL.Referencia.Leccion) = Function(r As OracleDataReader) New ELL.Referencia.Leccion With {.Lesson = New LeccionesAprendidasLib.ELL.Leccion With {.Codigo = CInt(r("ID_LECCION"))}, .Incluida = CBool(r("INCLUIDA")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Carga el listado de lecciones
        ''' </summary>        
        ''' <param name="oAmfe">Amfe</param>
        ''' <param name="ref">Referencia</param>
        ''' <param name="idEmpresa">Id empresa</param>
        ''' <returns></returns>        
        Public Function loadListLecciones(ByVal oAmfe As DesignFMEALib.ELL.Amfe, ByVal ref As String, ByVal idEmpresa As String) As List(Of ELL.Referencia.Leccion)
            query = "SELECT LR.ID_LECCION,LR.INCLUIDA,LR.COMENTARIO FROM LECCIONES_REF LR"
            Dim where = " WHERE ID_AMFE=:ID_AMFE AND REF=:REF AND ID_EMPRESA=:ID_EMPRESA"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_AMFE", OracleDbType.Int32, oAmfe.Id, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("REF", OracleDbType.Varchar2, ref, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Varchar2, idEmpresa, ParameterDirection.Input))
            If (oAmfe.Tipo <> 0) Then
                query &= " INNER JOIN LECCIONESAPRENDIDAS.LECCIONES2 LL ON LR.ID_LECCION = LL.IDLA"
                'where &= " AND LL.ID_TIPOLA = :TIPOLA"
                'lParametros.Add(New OracleParameter("TIPOLA", OracleDbType.Int32, oAmfe.Tipo, ParameterDirection.Input))
            End If
            query &= where
            Dim f As System.Func(Of OracleDataReader, ELL.Referencia.Leccion) = Function(r As OracleDataReader) New ELL.Referencia.Leccion With {.Lesson = New LeccionesAprendidasLib.ELL.Leccion With {.Codigo = CInt(r("ID_LECCION"))}, .Incluida = CBool(r("INCLUIDA")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO"))}
            Return Seleccionar(f)
        End Function

        ''' <summary>
        ''' Añade una leccion a una referencia
        ''' </summary>
        ''' <param name="idAmfe">Id de amfe</param>
        ''' <param name="oRef">Referencia</param>
        ''' <param name="oLecc">Leccion</param>        
        Public Sub AddLeccion(ByVal idAmfe As Integer, ByVal oRef As ELL.Referencia, ByVal oLecc As ELL.Referencia.Leccion)
            query = "INSERT INTO LECCIONES_REF(ID_AMFE,REF,ID_EMPRESA,ID_LECCION,INCLUIDA,COMENTARIO) VALUES (:ID_AMFE,:REF,:ID_EMPRESA,:ID_LECCION,:INCLUIDA,:COMENTARIO)"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_AMFE", OracleDbType.Int32, idAmfe, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("REF", OracleDbType.Varchar2, oRef.Ref, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Varchar2, oRef.IdEmpresa, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_LECCION", OracleDbType.Int32, oLecc.Lesson.Codigo, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("INCLUIDA", OracleDbType.Int32, Sablib.BLL.Utils.BooleanToInteger(oLecc.Incluida), ParameterDirection.Input))
            lParametros.Add(New OracleParameter("COMENTARIO", OracleDbType.Varchar2, oLecc.Comentario, ParameterDirection.Input))

            ExecuteQuery()
        End Sub

        ''' <summary>
        ''' Elimina las lecciones del amfe
        ''' </summary>
        ''' <param name="idAmfe">Id del amfe</param>        
        Public Sub DeleteLecciones(ByVal idAmfe As Integer)
            query = "DELETE FROM LECCIONES_REF WHERE ID_AMFE=:ID_AMFE"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ID_AMFE", OracleDbType.Int32, idAmfe, ParameterDirection.Input))

            ExecuteQuery()
        End Sub

#End Region

#Region "Brain"

        ''' <summary>
        ''' Carga las referencias de Brain
        ''' </summary>
        ''' <param name="texto">Texto a buscar</param>
        ''' <returns></returns>        
        Public Function loadReferenciasBrain(ByVal texto As String) As List(Of String())
            Dim cn As New OleDb.OleDbConnection(GetConexionBrain)
            'Dim query As String = "SELECT REF,DENOM1,EMP FROM CUBOS.PZAS WHERE LOWER(REF) LIKE '%" & texto.ToLower & "%' OR LOWER(DENOM1) LIKE '%" & texto.ToLower & "%'"
            'Dim query As String = "SELECT REF,DENOM1,EMP FROM CUBOS.PZAS WHERE (LOWER(REF) LIKE '%" & texto.ToLower & "%' OR LOWER(DENOM1) LIKE '%" & texto.ToLower & "%') AND TIPOPZA_01 IN ('1','2','3','9') AND EST1 = 1 AND CATPROD_TA <> 'Z'"
            Dim query As String = "SELECT PZA,DENOM00001,EMPR FROM CUBOS.PIEZAS WHERE (LOWER(PZA) LIKE '%" & texto.ToLower & "%' OR LOWER(DENOM00001) LIKE '%" & texto.ToLower & "%') AND TIPOPZA IN ('1','2','3','9') AND CATPR00001 <> 'Z'"

            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            Dim lRef As New List(Of String())
            Dim ref(1) As String
            Dim oReader As OleDb.OleDbDataReader = Nothing
            Dim hPlantas As New Hashtable
            Dim idEmpresa As String
            Dim idPlanta As Integer
            Dim plantBLL As New Sablib.BLL.PlantasComponent
            Dim oPlant As Sablib.ELL.Planta
            Try
                cn.Open()
                oReader = cm.ExecuteReader(System.Data.CommandBehavior.CloseConnection)
                While oReader.Read
                    idPlanta = 0
                    idEmpresa = oReader.Item("EMPR")
                    If (hPlantas.ContainsKey(idEmpresa)) Then
                        idPlanta = CInt(hPlantas(idEmpresa))
                    Else
                        oPlant = plantBLL.GetPlantaByIdBRAIN(idEmpresa)
                        If (oPlant IsNot Nothing) Then
                            idPlanta = oPlant.Id
                            hPlantas.Add(idEmpresa, idPlanta)
                        End If
                    End If
                    If (idPlanta > 0) Then lRef.Add(New String() {oReader.Item("PZA"), oReader.Item("DENOM00001"), idPlanta})
                End While
                Return lRef
            Catch ex As Exception
                Throw New Exception("Error al obtener las referencias", ex)
            Finally
                If (Not oReader.IsClosed) Then oReader.Close()
            End Try
        End Function

        ''' <summary>
        ''' Carga las referencias de la aplicacion de ventas que pueden estar en Brain o no
        ''' </summary>
        ''' <param name="texto">Texto a buscar</param>
        ''' <returns></returns>        
        Public Function loadReferenciasVentas(ByVal texto As String) As List(Of String())
            'query = "SELECT REFERENCIA_BATZ,ESPECIFICACION,RP.ID_PLANTA FROM SOLSISTEMAS.REFERENCIAS_VENTA RV 
            '        INNER JOIN SOLSISTEMAS.REFERENCIAS_PLANTAS RP ON RV.ID=RP.ID_REFERENCIA 
            '        WHERE ID_TIPO_NUMERO=4 
            '        AND REFERENCIA_BATZ IS NOT NULL 
            '        AND (LOWER(REFERENCIA_BATZ) LIKE '%' || :TEXTO || '%' 
            '            OR LOWER(ESPECIFICACION) LIKE '%' || :TEXTO || '%')"
            query = "SELECT REFERENCIA_BATZ,ESPECIFICACION,0 FROM SOLSISTEMAS.REFERENCIAS_VENTA RV 
                    WHERE ID_TIPO_NUMERO=4 
                    AND REFERENCIA_BATZ IS NOT NULL 
                    AND (LOWER(REFERENCIA_BATZ) LIKE '%' || :TEXTO || '%' 
                        OR LOWER(ESPECIFICACION) LIKE '%' || :TEXTO || '%')"
            lParametros = New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("TEXTO", OracleDbType.NVarchar2, texto.ToLower, ParameterDirection.Input))
            Return Seleccionar()
        End Function

#End Region

    End Class

End Namespace

