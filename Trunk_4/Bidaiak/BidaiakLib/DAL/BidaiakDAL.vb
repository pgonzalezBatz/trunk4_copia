Namespace DAL

    Public Class BidaiakDAL

#Region "Variables"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Conexion
        ''' </summary>
        ''' <returns></returns>        
        Public Function ConexionGestionHoras() As String
            Dim status As String = "GESTIONHORASTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "GESTIONHORASLIVE"
            Return Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Function

        ''' <summary>
        ''' Constructor
        ''' </summary>       
        Sub New()
            Dim status As String = "BIDAIAKTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "BIDAIAKLIVE"
            cn = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Sub

#End Region

#Region "Perfiles"

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el perfil del usuario
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idRecAdmon">Id del recurso admon para saber si tiene acceso directo o no</param>
        ''' <returns></returns>        
        Public Function loadProfile(ByVal idPlanta As Integer, ByVal idUser As Integer, ByVal idRecAdmon As Integer) As String()
            Try
                Dim query As String = "SELECT PERFIL," _
                                    & "NVL((SELECT CASE NVL(GR.IDRECURSOS,0) WHEN 0 THEN '0' ELSE '1' END FROM PERFILES P LEFT JOIN SAB.USUARIOSGRUPOS UG ON P.ID_USER=UG.IDUSUARIOS LEFT JOIN SAB.GRUPOSRECURSOS GR ON UG.IDGRUPOS=GR.IDGRUPOS WHERE GR.IDRECURSOS=:ID_RECURSO AND P.ID_USER=:ID_USER AND P.ID_PLANTA=:ID_PLANTA),0) AS ACCESO_DIRECTO " _
                                    & "FROM PERFILES " _
                                    & "WHERE ID_USER=:ID_USER AND ID_PLANTA=:ID_PLANTA"
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_RECURSO", OracleDbType.Int32, idRecAdmon, ParameterDirection.Input))

                Dim sPerfil As String() = Memcached.OracleDirectAccess.Seleccionar(query, Me.cn, lParameters.ToArray).FirstOrDefault
                If (sPerfil Is Nothing) Then sPerfil = New String() {BLL.BidaiakBLL.Profiles.Consultor, 0}

                Return sPerfil
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el perfil del usuario", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el perfil de un usuario
        ''' </summary>   
        ''' <param name="idPlanta">Id de la planta</param>     
        ''' <param name="idPerfil">Perfil</param>
        ''' <param name="idRecAdmon">Id del recurso admon para saber si tiene acceso directo o no</param>
        ''' <param name="bVigentes">Indica si solo se obtendran los vigentes</param>
        ''' <returns></returns>                
        Public Function loadUsersProfile(ByVal idPlanta As Integer, ByVal idPerfil As Integer, ByVal idRecAdmon As Integer, ByVal bVigentes As Boolean) As List(Of String())
            Try
                Dim query As String = "SELECT ID_USER," _
                                   & "NVL((SELECT CASE NVL(GR.IDRECURSOS,0)  WHEN 0 THEN '0'   ELSE '1' END FROM PERFILES P LEFT JOIN SAB.USUARIOSGRUPOS UG ON P.ID_USER=UG.IDUSUARIOS LEFT JOIN SAB.GRUPOSRECURSOS GR ON UG.IDGRUPOS=GR.IDGRUPOS WHERE GR.IDRECURSOS=:ID_RECURSO AND P.PERFIL>=:PERFIL AND P.ID_PLANTA=:ID_PLANTA AND PERFILES.ID_USER=UG.IDUSUARIOS),0) AS ACCESO_DIRECTO, PERFIL " _
                                   & "FROM PERFILES "
                If (bVigentes) Then query &= "LEFT JOIN SAB.USUARIOS U ON U.ID=PERFILES.ID_USER "
                query &= "WHERE ID_PLANTA=:ID_PLANTA AND PERFIL>=:PERFIL"
                If (bVigentes) Then query &= " AND (U.FECHABAJA IS NULL OR (U.FECHABAJA IS NOT NULL AND U.FECHABAJA>=SYSDATE))"

                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("PERFIL", OracleDbType.Int32, idPerfil, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID_RECURSO", OracleDbType.Int32, idRecAdmon, ParameterDirection.Input))

                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, lParameters.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los usuarios del perfil", ex)
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Establece el perfil de un usuario
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idPerfil">Id del perfil</param>            
        ''' <param name="bAccesoDir">Indica si accede directamente, sin pasar por el portal del empleado</param>
        ''' <param name="accion">Indica la accion a realizar=>1:insertar/actualizar,2:borrar</param>
        ''' <param name="idRecursoAdmon">Id del recurso admon para saber si tiene acceso directo o no</param>
        ''' <param name="idGrupoAdmon">Id del grupo admon</param>        
        Public Sub setProfile(ByVal idPlanta As Integer, ByVal idUser As Integer, ByVal idPerfil As Integer, ByVal bAccesoDir As Boolean, ByVal accion As Integer, ByVal idRecursoAdmon As Integer, ByVal idGrupoAdmon As Integer)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim query As String = String.Empty
                myConnection = New OracleConnection(cn)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                Dim bExiste As Boolean = False
                Dim lParametros As List(Of OracleParameter)
                If (accion = 1) Then
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                    query = "SELECT COUNT(*) FROM PERFILES WHERE ID_USER=:ID_USER AND ID_PLANTA=:ID_PLANTA"
                    bExiste = If(Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myConnection, lParametros.ToArray) = 1, True, False)
                End If
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                If (accion = 1) Then
                    If (Not bExiste) Then
                        query = "INSERT INTO PERFILES(ID_USER,ID_PLANTA,PERFIL) VALUES (:ID_USER,:ID_PLANTA,:PERFIL)"
                    Else
                        query = "UPDATE PERFILES SET PERFIL=:PERFIL WHERE ID_USER=:ID_USER AND ID_PLANTA=:ID_PLANTA"
                    End If
                    lParametros.Add(New OracleParameter("PERFIL", OracleDbType.Int32, idPerfil, ParameterDirection.Input))
                ElseIf (accion = 2) Then
                    query = "DELETE FROM PERFILES WHERE ID_USER=:ID_USER AND ID_PLANTA=:ID_PLANTA"
                End If
                If (query <> String.Empty) Then Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)

                query = "SELECT COUNT(*) FROM SAB.USUARIOSGRUPOS WHERE IDUSUARIOS=:ID_USER AND IDGRUPOS=:ID_GRUPO"
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_GRUPO", OracleDbType.Int32, idGrupoAdmon, ParameterDirection.Input))
                Dim bConAccesoActual As Boolean = If(Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, myConnection, lParametros.ToArray) = 1, True, False)

                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_GRUPO", OracleDbType.Int32, idGrupoAdmon, ParameterDirection.Input))
                query = String.Empty
                'Se actualiza la tabla de sab
                If (accion = 1) Then
                    If (Not bExiste) Then
                        If (bAccesoDir) Then
                            query = "INSERT INTO SAB.USUARIOSGRUPOS(IDUSUARIOS,IDGRUPOS) VALUES (:ID_USER,:ID_GRUPO)"
                        Else  'Es consultor, pero se ha quitado el acceso
                            query = "DELETE FROM SAB.USUARIOSGRUPOS WHERE IDUSUARIOS=:ID_USER AND IDGRUPOS=:ID_GRUPO"
                        End If
                    Else
                        If (bAccesoDir And Not bConAccesoActual) Then 'insert
                            query = "INSERT INTO SAB.USUARIOSGRUPOS(IDUSUARIOS,IDGRUPOS) VALUES (:ID_USER,:ID_GRUPO)"
                        ElseIf (Not bAccesoDir And bConAccesoActual) Then  'Si al actualizar, se marca sin acceso directo, se intentara borrar
                            query = "DELETE FROM SAB.USUARIOSGRUPOS WHERE IDUSUARIOS=:ID_USER AND IDGRUPOS=:ID_GRUPO"
                        End If
                    End If
                ElseIf (accion = 2 And Not bAccesoDir And bConAccesoActual) Then
                    query = "DELETE FROM SAB.USUARIOSGRUPOS WHERE IDUSUARIOS=:ID_USER AND IDGRUPOS=:ID_GRUPO"
                End If
                If (query <> String.Empty) Then Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al establecer el perfil del usuario", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Sub

#End Region

#End Region

#Region "Clientes"

        ''' <summary>
        ''' Obtiene la informacion de un cliente
        ''' 0:id,1:nombre
        ''' </summary>
        ''' <param name="id">Id del cliente</param>
        ''' <returns></returns>		
        Public Function GetCliente(ByVal id As String, ByVal stringConexion As String) As String()
            Dim query As String = "SELECT CODCLI,NOMBRE,OBSOLETO FROM FACLIENTE WHERE CODCLI=:CODCLI"
            parameter = New OracleParameter("CODCLI", OracleDbType.Char, id, ParameterDirection.Input)
            Dim lDatos As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, stringConexion, parameter)
            Dim sDatos As String() = Nothing
            If (lDatos IsNot Nothing) Then sDatos = lDatos.Item(0)

            Return sDatos
        End Function

        ''' <summary>
        ''' Obtiene los clientes que cumplan las condiciones
        ''' </summary>
        ''' <param name="obsoleto">
        ''' Indicar el tipo de clientes a obtener=>
        ''' H:Habitual / P:Potencial / O:Obsoleto		
        ''' </param>
        ''' <returns></returns>		
        Public Function GetClientes(ByVal stringConexion As String, ByVal obsoleto As String) As List(Of String())
            Dim query As String = "SELECT CODCLI,NOMBRE,OBSOLETO FROM FACLIENTE WHERE OBSOLETO=:OBSOLETO"
            parameter = New OracleParameter("OBSOLETO", OracleDbType.Varchar2, obsoleto, ParameterDirection.Input)
            Dim lDatos As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, stringConexion, parameter)

            Return lDatos
        End Function

#End Region

#Region "Proyectos"

        ''' <summary>
        ''' Obtiene la informacion del proyecto especificado		
        ''' </summary>
        ''' <param name="id">Id del proyecto</param>
        ''' <param name="stringConexion">String de conexion de la base de datos donde tiene que conectarse</param> 
        ''' <returns></returns>		
        Public Function GetProyecto(ByVal id As Integer, ByVal stringConexion As String) As String()
            Dim query As String = "SELECT ID,PROGRAMA,CODCLI,NUMORD,VIGENTE FROM FAPROGRAMA WHERE ID=:ID"
            parameter = New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)
            Dim lDatos As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, stringConexion, parameter)
            Dim sDatos As String() = Nothing
            If (lDatos IsNot Nothing) Then sDatos = lDatos.Item(0)
            Return sDatos
        End Function

        ''' <summary>
        ''' Obtiene los proyectos vigentes de un cliente
        ''' </summary>
        ''' <param name="codCli">Codigo del cliente</param>
        ''' <returns></returns>		
        Public Function GetProyectos(ByVal codCli As String, ByVal stringConexion As String) As List(Of String())
            'Dim query As String = "SELECT ID,PROGRAMA,CODCLI,NUMORD,VIGENTE FROM FAPROGRAMA WHERE CODCLI=:CODCLI AND VIGENTE='S'"
            '050220:Cambia la query porque con la anterior no se gestionaban ya los proyectos vigentes
            Dim query As String = "SELECT DISTINCT ID,DESCRI AS PROGRAMA FROM W_PROYECTO_CLIENTE_OF_TODAS WHERE NUMORD<90000 AND FEC_CIERRE IS NULL AND CODCLI=:CODCLI"
            parameter = New OracleParameter("CODCLI", OracleDbType.Char, codCli, ParameterDirection.Input)
            Dim lDatos As List(Of String()) = Memcached.OracleDirectAccess.Seleccionar(query, stringConexion, parameter)
            Return lDatos
        End Function

#End Region

#Region "Epsilon/SAB"

        ''' <summary>
        ''' Dada un nombre y apellidos, busca alguna coincidencia en Epsilon
        ''' La busqueda en base de datos lo hace sobre el numero de caracteres especificado. Es decir, si pone AGUIRREGOMEZC, se buscara en SUBSTRING(APELLIDO1,1,LEN(@APELLIDO1)) y no sobre APELLIDO1
        ''' </summary>
        ''' <param name="nombre">Nombre</param>
        ''' <param name="apellido">Primer apellido</param>
        ''' <param name="oPlanta">Objeto planta</param>
        ''' <returns>Lista de codPersona,nombre,apellido</returns>
        Public Function SearchByNameEpsilon(ByVal nombre As String, ByVal apellido As String, ByVal oPlanta As SabLib.ELL.Planta) As List(Of String())
            Dim query As String = "SELECT NIF,NOMBRE,APELLIDO1 " _
                                 & "FROM PERSONAS " _
                                 & "WHERE NOMBRE Like + '%' +  @NOMBRE + '%'  AND SUBSTRING(APELLIDO1,1,LEN(@APELLIDO)) LIKE + '%' + @APELLIDO + '%'"
            Dim p1 As New SqlClient.SqlParameter("NOMBRE", SqlDbType.NVarChar, 20)
            p1.Value = nombre
            Dim p2 As New SqlClient.SqlParameter("APELLIDO", SqlDbType.NVarChar, 20)
            p2.Value = apellido
            Return Memcached.SQLServerDirectAccess.Seleccionar(query, oPlanta.NominasConnectionString, p1, p2)
        End Function

        ''' <summary>
        ''' Funcion utiliza para la busqueda de personas en epsilon, mostrando tambien los dados de baja
        ''' Dada un nombre y apellidos, busca alguna coincidencia en Epsilon
        ''' La busqueda en base de datos lo hace sobre el numero de caracteres especificado. Es decir, si pone AGUIRREGOMEZC, se buscara en SUBSTRING(APELLIDO1,1,LEN(@APELLIDO1)) y no sobre APELLIDO1
        ''' </summary>
        ''' <param name="nombre">Nombre</param>
        ''' <param name="apellido">Primer apellido</param>
        ''' <param name="apellido2">Segundo apellido</param>
        ''' <param name="oPlanta">Objeto planta</param>
        ''' <returns>Lista de codPersona,nombre,apellido</returns>
        Public Function SearchByNameEpsilon(ByVal nombre As String, ByVal apellido As String, ByVal apellido2 As String, ByVal oPlanta As SabLib.ELL.Planta) As List(Of String())
            Dim query As String = "SELECT DISTINCT P.NIF,P.NOMBRE,P.APELLIDO1,P.APELLIDO2,T.ID_TRABAJADOR,MAX(T.F_BAJA) " _
                                & "FROM TRABAJADORES T INNER JOIN COD_TRA C ON T.ID_TRABAJADOR=C.ID_TRABAJADOR " _
                                & "INNER JOIN PERSONAS P ON P.NIF=C.NIF " _
                                & "WHERE T.ID_EMPRESA=@ID_EMPRESA AND P.NOMBRE Like + '%' +  @NOMBRE + '%'  AND SUBSTRING(P.APELLIDO1,1,LEN(@APELLIDO)) LIKE + '%' + @APELLIDO + '%'"
            Dim p1 As New SqlClient.SqlParameter("NOMBRE", SqlDbType.NVarChar, 20)
            p1.Value = nombre
            Dim p2 As New SqlClient.SqlParameter("APELLIDO", SqlDbType.NVarChar, 20)
            p2.Value = apellido
            Dim p3 As New SqlClient.SqlParameter("ID_EMPRESA", SqlDbType.NVarChar, 20)
            p3.Value = oPlanta.IdEpsilon
            Dim p4 As SqlClient.SqlParameter = Nothing
            If (apellido2 <> String.Empty) Then
                query &= " AND SUBSTRING(P.APELLIDO2,1,LEN(@APELLIDO2)) LIKE + '%' + @APELLIDO2 + '%'"
                p4 = New SqlClient.SqlParameter("APELLIDO2", SqlDbType.NVarChar, 20)
                p4.Value = apellido2
            End If
            query &= " GROUP BY P.NIF,P.NOMBRE,P.APELLIDO1,P.APELLIDO2,T.ID_TRABAJADOR"
            If (p4 Is Nothing) Then
                Return Memcached.SQLServerDirectAccess.Seleccionar(query, oPlanta.NominasConnectionString, p1, p2, p3)
            Else
                Return Memcached.SQLServerDirectAccess.Seleccionar(query, oPlanta.NominasConnectionString, p1, p2, p3, p4)
            End If
        End Function

        ''' <summary>
        ''' Dada un nombre y apellidos, busca alguna coincidencia en SAB        
        ''' </summary>
        ''' <param name="nombre">Nombre</param>
        ''' <param name="apellido">Primer apellido</param>
        ''' <param name="idPlanta">Id de la planta de SAB</param>
        ''' <returns>Lista de codPersona,nombre,apellido</returns>
        Public Function SearchByNameSAB(ByVal nombre As String, ByVal apellido As String, ByVal idPlanta As Integer) As List(Of String())
            Dim status As String = "SABTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "SABLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            Dim query As String = "SELECT DNI,NOMBRE,APELLIDO1 FROM USUARIOS WHERE LOWER(NOMBRE) LIKE '%' || :NOMBRE || '%'  AND LOWER(SUBSTR(APELLIDO1,1,LENGTH(:APELLIDO))) LIKE '%' || :APELLIDO || '%' AND IDPLANTA=:IDPLANTA"
            Dim p1 As New OracleParameter("NOMBRE", OracleDbType.Varchar2, nombre.ToLower, ParameterDirection.Input)
            Dim p2 As New OracleParameter("APELLIDO", OracleDbType.Varchar2, apellido.ToLower, ParameterDirection.Input)
            Dim p3 As New OracleParameter("IDPLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
            Return Memcached.OracleDirectAccess.Seleccionar(query, conexion, p1, p2, p3)
        End Function

        ''' <summary>
        ''' Obtiene las personas y su departamento de SAB de los subcontratados y usuarios de otras plantas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>0:Id,1:Codpersona,2:idDepto,3:Nombre completo</returns>
        Function GetDepartamentosPersonasSubcontratadas(ByVal idPlanta As Integer) As List(Of String())
            Dim status As String = "SABTEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "SABLIVE"
            Dim conexion As String = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
            '"Negocio|Departamento|num_socio|Trabajador
            Dim query As String = "SELECT ID,CODPERSONA,IDDEPARTAMENTO,LTRIM(RTRIM(APELLIDO1)) || NVL2(APELLIDO2,' ' || LTRIM(RTRIM(APELLIDO2)),'') || ',' || LTRIM(RTRIM(NOMBRE)) AS NOMBRE,EMAIL FROM USUARIOS WHERE IDPLANTA=:IDPLANTA AND IDEMPRESAS<>1 AND IDDEPARTAMENTO IS NOT NULL AND CODPERSONA IS NOT NULL AND (FECHABAJA IS NULL OR FECHABAJA>SYSDATE)"
            Dim p As New OracleParameter("IDPLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
            Return Memcached.OracleDirectAccess.Seleccionar(query, conexion, p)
        End Function

#End Region

#Region "Parametros"

        ''' <summary>
        ''' Carga los parametros de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>      
        Public Function loadParameters(ByVal idPlanta As Integer) As ELL.Parametro
            Try
                Dim query As String = "SELECT PRECIOKM,CODPROV_AGENCIA,ID_PLANTA,CADUCIDAD_VIAJE,DIAS_SOLICITAR_ANTIC,ID_CONCEPTO_KM FROM PARAMETROS WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As List(Of ELL.Parametro) = Memcached.OracleDirectAccess.seleccionar(Of ELL.Parametro)(Function(r As OracleDataReader) _
                 New ELL.Parametro With {.PrecioKm = CDec(r("PRECIOKM")), .CodProvAgencia = r("CODPROV_AGENCIA"), .IdPlanta = CInt(r("ID_PLANTA")), .DiasCaducidadViaje = CInt(r("CADUCIDAD_VIAJE")),
                                         .DiasSolicitarAnticipo = CInt(r("DIAS_SOLICITAR_ANTIC")), .IdConceptoKm = SabLib.BLL.Utils.integerNull(r("ID_CONCEPTO_KM"))},
                                        query, cn, (New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)))
                Return lParametros.FirstOrDefault
            Catch ex As Exception
                Throw New BatzException("Error al obtener los parametros", ex)
            End Try
        End Function

        ''' <summary>
        ''' Consulta las distintas plantas que funcionan como plantas en la aplicacion
        ''' </summary>
        ''' <returns></returns>        
        Public Function loadApplicationPlantas() As List(Of String())
            Try
                Dim query As String = "SELECT P.ID_PLANTA AS ID,PL.NOMBRE FROM PARAMETROS P INNER JOIN SAB.PLANTAS PL ON P.ID_PLANTA=PL.ID"
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn)
            Catch ex As Exception
                Throw New BatzException("Error al obtener los parametros", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda los parametros
        ''' </summary>
        ''' <param name="parametros">Parametros</param> 
        Public Sub saveParameters(ByVal parametros As ELL.Parametro)
            Try
                Dim query As String = "SELECT COUNT(ID_PLANTA) FROM PARAMETROS WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, parametros.IdPlanta, ParameterDirection.Input))

                If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) = 0) Then 'Insert
                    query = "INSERT INTO PARAMETROS(PRECIOKM,CODPROV_AGENCIA,ID_PLANTA,CADUCIDAD_VIAJE,DIAS_SOLICITAR_ANTIC,ID_CONCEPTO_KM) VALUES (:PRECIOKM,:CODPROV_AGENCIA,:ID_PLANTA,:CADUCIDAD_VIAJE,:DIAS_SOLICITAR_ANTIC,:ID_CONCEPTO_KM)"
                Else 'update
                    query = "UPDATE PARAMETROS SET PRECIOKM=:PRECIOKM,CODPROV_AGENCIA=:CODPROV_AGENCIA,CADUCIDAD_VIAJE=:CADUCIDAD_VIAJE,DIAS_SOLICITAR_ANTIC=:DIAS_SOLICITAR_ANTIC,ID_CONCEPTO_KM=:ID_CONCEPTO_KM WHERE ID_PLANTA=:ID_PLANTA"
                End If
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("PRECIOKM", OracleDbType.Decimal, parametros.PrecioKm, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CODPROV_AGENCIA", OracleDbType.Varchar2, parametros.CodProvAgencia, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, parametros.IdPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CADUCIDAD_VIAJE", OracleDbType.Int32, parametros.DiasCaducidadViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("DIAS_SOLICITAR_ANTIC", OracleDbType.Int32, parametros.DiasSolicitarAnticipo, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_CONCEPTO_KM", OracleDbType.Int32, If(parametros.IdConceptoKm > 0, parametros.IdConceptoKm, DBNull.Value), ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al guardar los parametros de una planta", ex)
            End Try
        End Sub

#End Region

#Region "Ejecuciones"

        ''' <summary>
        ''' Obtiene la informacion de ejecucion de la planta
        ''' </summary>        
        ''' <param name="tipoEjec">Tipo de ejecucion</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function loadEjecucion(ByVal tipoEjec As BLL.BidaiakBLL.TipoEjecucion, ByVal idPlanta As Integer) As BLL.BidaiakBLL.Ejecucion
            Try
                Dim query As String = "SELECT FECHA,ID_USER,PASO,ID_PLANTA,ANNO,MES,NOMBRE_FICHERO,FICHERO FROM EJECUCIONES_POR_PASOS WHERE ID_PLANTA=:ID_PLANTA AND TIPO=:TIPO"

                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, tipoEjec, ParameterDirection.Input))
                Dim lEjecucion As List(Of BLL.BidaiakBLL.Ejecucion) = Memcached.OracleDirectAccess.seleccionar(Of BLL.BidaiakBLL.Ejecucion)(Function(r As OracleDataReader) _
                            New BLL.BidaiakBLL.Ejecucion With {.Fecha = CDate(r("FECHA")), .IdUser = CInt(r("ID_USER")), .Paso = CInt(r("PASO")), .IdPlanta = CInt(r("ID_PLANTA")), .Anno = CInt(r("ANNO")), .Mes = CInt(r("MES")),
                                                               .NombreFichero = r("NOMBRE_FICHERO"), .Fichero = CType(r("FICHERO"), Byte())},
                                                                query, cn, lParametros.ToArray)

                If (lEjecucion IsNot Nothing AndAlso lEjecucion.Count = 1) Then
                    Return lEjecucion.First
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la ejecucion por pasos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Inserta el paso de ejecucion del usuario de la planta
        ''' </summary>
        ''' <param name="ejec">Objeto ejecucion</param>        
        Sub initEjecucion(ByVal ejec As BLL.BidaiakBLL.Ejecucion)
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "INSERT INTO EJECUCIONES_POR_PASOS(FECHA,ID_USER,PASO,ID_PLANTA,TIPO,ANNO,MES,NOMBRE_FICHERO,FICHERO) VALUES (:FECHA,:ID_USER,2,:ID_PLANTA,:TIPO,:ANNO,:MES,:NOMBRE_FICHERO,:FICHERO)"

                lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, Now, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, ejec.IdUser, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, ejec.IdPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, ejec.Tipo, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NOMBRE_FICHERO", OracleDbType.NVarchar2, ejec.NombreFichero, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FICHERO", OracleDbType.Blob, ejec.Fichero, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, ejec.Anno, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, ejec.Mes, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la informacion de la ejecucion por pasos", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Actualiza el paso del usuario de la planta
        ''' </summary>
        ''' <param name="ejec">Objeto ejecucion</param> 
        Sub saveEjecucion(ByVal ejec As BLL.BidaiakBLL.Ejecucion)
            Try
                Dim query As String = "UPDATE EJECUCIONES_POR_PASOS SET FECHA=:FECHA,PASO=:PASO WHERE ID_PLANTA=:ID_PLANTA AND TIPO=:TIPO"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, Now, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("PASO", OracleDbType.Int32, ejec.Paso, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, ejec.IdPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, ejec.Tipo, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar la informacion de la ejecucion por pasos", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Borra la informacion de la planta y tipo
        ''' </summary>               
        ''' <param name="tipoEjec">Tipo de ejecucion</param>
        ''' <param name="idPlanta">Id de la planta</param>
        Sub deleteEjecucion(ByVal tipoEjec As BLL.BidaiakBLL.TipoEjecucion, ByVal idPlanta As Integer)
            Try
                Dim query As String = "DELETE FROM EJECUCIONES_POR_PASOS WHERE ID_PLANTA=:ID_PLANTA AND TIPO=:TIPO"
                Memcached.OracleDirectAccess.NoQuery(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input),
                                                                New OracleParameter("TIPO", OracleDbType.Int32, tipoEjec, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al borrar la informacion de la ejecucion por pasos de la planta", ex)
            End Try
        End Sub

#End Region

#Region "Validador departamento"

        ''' <summary>
        ''' Dado un dpto, busca si tiene asociado un validador
        ''' </summary>
        ''' <param name="dpto">Departamento</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function getValidadorDpto(ByVal dpto As String, ByVal idPlanta As Integer) As Integer
            Try
                Dim query As String = "SELECT ID_VALIDADOR FROM VALIDADORES_VIAJE WHERE ID_DPTO=:DPTO AND ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("DPTO", OracleDbType.Varchar2, dpto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el validador del departamento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Devuelve todos los registros de validadores departamentos de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <returns></returns>        
        Function getValidadoresDptos(ByVal idPlanta As String) As List(Of String())
            Try
                Dim query As String = "SELECT ID_DPTO,ID_VALIDADOR FROM VALIDADORES_VIAJE WHERE ID_PLANTA=:ID_PLANTA"

                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el listado de validadores departamento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Inserta un validador y un departamento
        ''' </summary>
        ''' <param name="dpto">Departamento</param>
        ''' <param name="idVal">Id del validador</param>  
        ''' <param name="idPlanta">Id de la planta</param>      
        Sub addValidadorDpto(ByVal dpto As String, ByVal idVal As Integer, ByVal idPlanta As Integer)
            Try
                Dim query As String = "INSERT INTO VALIDADORES_VIAJE(ID_DPTO,ID_VALIDADOR,ID_PLANTA) VALUES (:ID_DPTO,:ID_VALIDADOR,:ID_PLANTA)"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_DPTO", OracleDbType.Varchar2, dpto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, idVal, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al añadir un validador - departamento", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Borra la informacion de un departamento
        ''' </summary>        
        ''' <param name="dpto">Departamento</param>  
        ''' <param name="idPlanta">Id de la planta</param>     
        Sub deleteValidadorDpto(ByVal dpto As String, ByVal idPlanta As Integer)
            Try
                Dim query As String = "DELETE FROM VALIDADORES_VIAJE WHERE ID_DPTO=:DPTO AND ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("DPTO", OracleDbType.Varchar2, dpto, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar una relacion validador - departamento", ex)
            End Try
        End Sub

#End Region

#Region "Cuenta contrapartida"

        ''' <summary>
        ''' Obtiene la cuenta de contrapartida
        ''' </summary>
        ''' <param name="idPlantaGestion">Id de la planta que gestiona las cuentas</param>
        ''' <param name="idPlantaCta">Id de la planta a la que se le asigna las cuentas</param>
        ''' <returns>Se devuelve un string() por si en un futuro se añaden nuevos campos a esta tabla</returns>        
        Function loadCuentaContrapartida(ByVal idPlantaGestion As Integer, ByVal idPlantaCta As Integer) As ELL.CuentaContrapartida
            Try
                Dim query As String = "SELECT ID_PLANTA,CONTRAPARTIDA,CUOTA,ID_PLANTAGESTION FROM CUENTAS_CONTRAPARTIDA WHERE ID_PLANTA=:ID_PLANTA AND ID_PLANTAGESTION=:ID_PLANTAGESTION"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlantaCta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, idPlantaGestion, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CuentaContrapartida)(Function(o As OracleDataReader) New ELL.CuentaContrapartida With {.IdPlantaCuenta = CInt(o("ID_PLANTA")), .IdPlantaGestion = CInt(o("ID_PLANTAGESTION")),
                                                                                        .CtaContrapartida = SabLib.BLL.Utils.integerNull(o("CONTRAPARTIDA")), .CtaCuota = SabLib.BLL.Utils.integerNull(o("CUOTA"))},
                                                                                    query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BatzException("Error al obtener la cuenta de contrapartida de la planta", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todas las cuentas de contrapartida
        ''' </summary>
        ''' <param name="idPlantaGestion">Id de la planta de gestion</param>
        ''' <returns></returns>
        Function loadCuentasContrapartida(ByVal idPlantaGestion As Integer) As List(Of ELL.CuentaContrapartida)
            Try
                Dim query As String = "SELECT P.ID,C.CONTRAPARTIDA,C.CUOTA,P.NOMBRE,C.ID_PLANTAGESTION FROM SAB.PLANTAS P INNER JOIN PLANTAS_BIDAIAK PB ON P.ID=PB.ID_PLANTA LEFT JOIN CUENTAS_CONTRAPARTIDA C ON (P.ID=C.ID_PLANTA AND C.ID_PLANTAGESTION=:ID_PLANTAGESTION)"
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.CuentaContrapartida)(Function(o As OracleDataReader) New ELL.CuentaContrapartida With {.IdPlantaCuenta = CInt(o("ID")), .IdPlantaGestion = SabLib.BLL.Utils.integerNull(o("ID_PLANTAGESTION")),
                                                                                        .CtaContrapartida = SabLib.BLL.Utils.integerNull(o("CONTRAPARTIDA")), .CtaCuota = SabLib.BLL.Utils.integerNull(o("CUOTA")), .NombrePlanta = SabLib.BLL.Utils.stringNull(o("NOMBRE"))},
                                                                                    query, cn, New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, idPlantaGestion, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BatzException("Error al obtener las cuentas de contrapartida de la planta", ex)
            End Try
        End Function

        ''' <summary>
        ''' Se guarda la informacion de la cuenta
        ''' </summary>
        ''' <param name="oInfo">Informacion</param>        
        Sub SaveCuentaContrapartida(ByVal oInfo As ELL.CuentaContrapartida)
            Try
                Dim query As String = "SELECT COUNT(ID_PLANTA) FROM CUENTAS_CONTRAPARTIDA WHERE ID_PLANTA=:ID_PLANTA AND ID_PLANTAGESTION=:ID_PLANTAGESTION"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oInfo.IdPlantaCuenta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, oInfo.IdPlantaGestion, ParameterDirection.Input))

                If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) = 0) Then 'Insert
                    query = "INSERT INTO CUENTAS_CONTRAPARTIDA(ID_PLANTA,CONTRAPARTIDA,CUOTA,ID_PLANTAGESTION) VALUES (:ID_PLANTA,:CONTRAPARTIDA,:CUOTA,:ID_PLANTAGESTION)"
                Else 'update
                    query = "UPDATE CUENTAS_CONTRAPARTIDA SET CONTRAPARTIDA=:CONTRAPARTIDA,CUOTA=:CUOTA WHERE ID_PLANTA=:ID_PLANTA AND ID_PLANTAGESTION=:ID_PLANTAGESTION"
                End If
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("CONTRAPARTIDA", OracleDbType.Int32, oInfo.CtaContrapartida, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CUOTA", OracleDbType.Int32, SabLib.BLL.Utils.integerNull(oInfo.CtaCuota), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oInfo.IdPlantaCuenta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, oInfo.IdPlantaGestion, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al guardar la cuenta de contrapartida", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Se borra la informacion de la cuenta de la planta
        ''' </summary>
        ''' <param name="idPlantaGestion">Id de la planta que gestiona las cuentas</param>
        ''' <param name="idPlantaCta">Id de la planta a la que se le asigna las cuentas</param>     
        Sub DeleteCuentaContrapartida(ByVal idPlantaGestion As Integer, ByVal idPlantaCta As Integer)
            Try
                Dim query As String = "DELETE FROM CUENTAS_CONTRAPARTIDA WHERE ID_PLANTA=:ID_PLANTA AND ID_PLANTAGESTION=:ID_PLANTAGESTION"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlantaCta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTAGESTION", OracleDbType.Int32, idPlantaGestion, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al eliminar la cuenta de contrapartida de la planta", ex)
            End Try
        End Sub

#End Region

#Region "Cuenta visa excepcion (COMENTADO)"

        '''' <summary>
        '''' Obtiene la cuenta de las visas de excepcion
        '''' </summary>        
        '''' <param name="idPlanta">Id de la planta de gestion</param>
        '''' <returns></returns>
        'Function loadCuentaVisaExcepcion(ByVal idPlanta As Integer) As Object
        '    Try
        '        Dim query As String = "SELECT ID_PLANTA,CUENTA,LANTEGI FROM CUENTAS_VISAS_EXCEPCION WHERE ID_PLANTA=:ID_PLANTA"
        '        Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(o As OracleDataReader) New With {.IdPlanta = CInt(o("ID_PLANTA")), .Cuenta = CInt(o("CUENTA")), .Lantegi = o("LANTEGI")},
        '                                                                            query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)).FirstOrDefault
        '    Catch ex As Exception
        '        Throw New BatzException("Error al obtener la cuenta de visa excepcion de la planta", ex)
        '    End Try
        'End Function

        '''' <summary>
        '''' Se guarda la informacion de la cuenta
        '''' </summary>
        '''' <param name="oInfo">Informacion</param>        
        'Sub SaveCuentaVisaExcepcion(ByVal oInfo As Object)
        '    Try
        '        Dim query As String = "SELECT COUNT(ID_PLANTA) FROM CUENTAS_VISAS_EXCEPCION WHERE ID_PLANTA=:ID_PLANTA"
        '        Dim lParametros As New List(Of OracleParameter)
        '        lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oInfo.IdPlanta, ParameterDirection.Input))

        '        If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) = 0) Then 'Insert
        '            query = "INSERT INTO CUENTAS_VISAS_EXCEPCION(ID_PLANTA,CUENTA,LANTEGI) VALUES (:ID_PLANTA,:CUENTA,:LANTEGI)"
        '        Else 'update
        '            query = "UPDATE CUENTAS_VISAS_EXCEPCION SET CUENTA=:CUENTA,LANTEGI=:LANTEGI WHERE ID_PLANTA=:ID_PLANTA"
        '        End If
        '        lParametros = New List(Of OracleParameter)
        '        lParametros.Add(New OracleParameter("CUENTA", OracleDbType.Int32, oInfo.Cuenta, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("LANTEGI", OracleDbType.NVarchar2, oInfo.Lantegi, ParameterDirection.Input))
        '        lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oInfo.IdPlanta, ParameterDirection.Input))
        '        Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
        '    Catch ex As Exception
        '        Throw New BatzException("Error al guardar la cuenta de visas excepcion", ex)
        '    End Try
        'End Sub

#End Region

#Region "Gerentes plantas"

        ''' <summary>
        ''' Obtiene el gerente de la planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function loadGerentePlanta(ByVal idPlanta As Integer) As String()
            Try
                Dim query As String = "SELECT ID_USER,ID_PLANTA FROM GERENTES_PLANTAS WHERE ID_PLANTA=:ID_PLANTA"

                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los gerentes", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los gerentes existentes
        ''' </summary>
        ''' <returns></returns>      
        Public Function loadGerentesPlantas() As List(Of String())
            Try
                Dim query As String = "SELECT G.ID_USER,P.ID AS ID_PLANTA,P.NOMBRE AS PLANTA,CONCAT(CONCAT(CONCAT(CONCAT(U.NOMBRE,' '),U.APELLIDO1),' '),U.APELLIDO2) AS NOMBRE_GERENTE " _
                                    & "FROM SAB.PLANTAS P LEFT JOIN GERENTES_PLANTAS G ON G.ID_PLANTA=P.ID LEFT JOIN SAB.USUARIOS U ON G.ID_USER=U.ID WHERE (P.UBICACION<>'N' OR P.ID=1) AND P.OBSOLETO=0" 'Tiene que salir la de Igorre
                Return Memcached.OracleDirectAccess.Seleccionar(query, cn)
            Catch ex As Exception
                Throw New BatzException("Error al obtener los gerentes", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las plantas de las que es gerente un usuario
        ''' </summary>
        ''' <returns></returns>      
        Public Function loadGerentesPlantas(idUser) As List(Of String())
            Try
                Dim query As String = "SELECT G.ID_USER,P.ID AS ID_PLANTA,P.NOMBRE AS PLANTA,CONCAT(CONCAT(CONCAT(CONCAT(U.NOMBRE,' '),U.APELLIDO1),' '),U.APELLIDO2) AS NOMBRE_GERENTE " _
                                    & "FROM GERENTES_PLANTAS G INNER JOIN SAB.PLANTAS P ON G.ID_PLANTA=P.ID INNER JOIN SAB.USUARIOS U ON G.ID_USER=U.ID " _
                                    & "WHERE G.ID_USER=:ID_USER"

                Return Memcached.OracleDirectAccess.Seleccionar(query, cn, New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los gerentes", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda los datos de los gerentes
        ''' </summary>
        ''' <param name="idGerente">Id del gerente</param> 
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub saveGerentePlanta(ByVal idGerente As Integer, ByVal idPlanta As Integer)
            Try
                Dim query As String = "SELECT COUNT(ID_PLANTA) FROM GERENTES_PLANTAS WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

                If (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, cn, lParametros.ToArray) = 0) Then 'Insert
                    query = "INSERT INTO GERENTES_PLANTAS(ID_USER,ID_PLANTA) VALUES (:ID_USER,:ID_PLANTA)"
                Else 'update
                    query = "UPDATE GERENTES_PLANTAS SET ID_USER=:ID_USER WHERE ID_PLANTA=:ID_PLANTA"
                End If
                lParametros = New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idGerente, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar el gerente de una planta", ex)
            End Try
        End Sub

#End Region

#Region "Importacion documentos"

        ''' <summary>
        ''' Obtiene la informacion la importacion del documento
        ''' </summary>
        ''' <param name="id">Id de la importacion</param>        
        ''' <returns></returns>        
        Function loadImportacionDoc(ByVal id As Integer) As BidaiakLib.BLL.BidaiakBLL.Importacion
            Try
                Dim query As String = "SELECT ID,FECHA,CON_VIAJE,SIN_VIAJE,TIPO,NUM_REGISTROS,ANNO,MES,NOMBRE_FICHERO,FICHERO,ID_PLANTA FROM IMPORTACION_DOCS WHERE ID=:ID"
                Return Memcached.OracleDirectAccess.seleccionar(Of BLL.BidaiakBLL.Importacion)(Function(r As OracleDataReader) _
                           New BLL.BidaiakBLL.Importacion With {.Id = CInt(r("ID")), .Fecha = CDate(r("FECHA")), .ConViaje = SabLib.BLL.Utils.integerNull(r("CON_VIAJE")), .SinViaje = SabLib.BLL.Utils.integerNull(r("SIN_VIAJE")), .Anno = CInt(r("ANNO")), .Mes = CInt(r("MES")),
                                                              .NumRegistros = CInt(r("NUM_REGISTROS")), .Tipo = CType(r("TIPO"), BLL.BidaiakBLL.TipoEjecucion), .NombreFichero = SabLib.BLL.Utils.stringNull(r("NOMBRE_FICHERO")), .Fichero = SabLib.BLL.Utils.byteNull(r("FICHERO")), .IdPlanta = CInt(r("ID_PLANTA"))},
                                                               query, cn, New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input)).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la importacion de documentos dada un id", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion la importacion del documento en un año y mes en concreto
        ''' </summary>
        ''' <param name="tipoDoc">Tipo de la importacion.1:Visas,2:Factura Eroski</param>        
        ''' <param name="año">Año a consultar</param>
        ''' <param name="mes">Mes a consultar</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function loadImportacionDoc(ByVal tipoDoc As Integer, ByVal año As Integer, ByVal mes As Integer, ByVal idPlanta As Integer) As BidaiakLib.BLL.BidaiakBLL.Importacion
            Try
                Dim query As String = "SELECT ID,FECHA,CON_VIAJE,SIN_VIAJE,TIPO,NUM_REGISTROS,ANNO,MES,NOMBRE_FICHERO,FICHERO,ID_PLANTA FROM IMPORTACION_DOCS WHERE TIPO=:TIPO AND ANNO=:ANNO AND MES=:MES AND ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("TIPO", OracleDbType.Int32, tipoDoc, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ANNO", OracleDbType.Int32, año, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("MES", OracleDbType.Int32, mes, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of BLL.BidaiakBLL.Importacion)(Function(r As OracleDataReader) _
                           New BLL.BidaiakBLL.Importacion With {.Id = CInt(r("ID")), .Fecha = CDate(r("FECHA")), .ConViaje = SabLib.BLL.Utils.integerNull(r("CON_VIAJE")), .SinViaje = SabLib.BLL.Utils.integerNull(r("SIN_VIAJE")), .Anno = CInt(r("ANNO")), .Mes = CInt(r("MES")),
                                                              .NumRegistros = CInt(r("NUM_REGISTROS")), .Tipo = CType(r("TIPO"), BLL.BidaiakBLL.TipoEjecucion), .NombreFichero = SabLib.BLL.Utils.stringNull(r("NOMBRE_FICHERO")), .Fichero = SabLib.BLL.Utils.byteNull(r("FICHERO")), .IdPlanta = CInt(r("ID_PLANTA"))},
                                                               query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la importacion de documentos dado un mes y año", ex)
            End Try
        End Function

#End Region

#Region "Varias"

        ''' <summary>
        ''' Obtiene las condiciones especiales registradas
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="bSoloVigentes">True si solo las vigentes y false en caso contrario</param>
        ''' <returns></returns>        
        Public Function loadCondicionesEspeciales(ByVal idPlanta As Integer, bSoloVigentes As Boolean) As List(Of String())
            Try
                '22052015:ESTO VA A IR SIN PLANTA. SI UN DIA LLEGA EL CASO, SE TENDRA QUE SEPARAR E INCLUIR EN BBDD ALGUN CAMPO YA QUE 'PAISES EN VIAS DE DESARROLLO' TIENE LOGICA. SOLO SE ACTIVA PARA EL RESTO DEL MUNDO
                Dim query As String = "SELECT ID,NOMBRE,OBSOLETA FROM CONDICIONES_ESPECIALES " 'WHERE ID_PLANTA=:ID_PLANTA"
                If (bSoloVigentes) Then query &= " WHERE OBSOLETA=0"

                Return Memcached.OracleDirectAccess.Seleccionar(query, cn) ' New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las condiciones especiales", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las hojas de viajes y pernoctas de una persona entre las fechas de un viaje
        ''' </summary>
        ''' <param name="codPersona">Numero de trabajador</param>
        ''' <param name="idIzaro">Id de la planta de izaro</param>
        ''' <param name="fInicio">Fecha de inicio de busqueda</param>
        ''' <param name="fFin">Fecha de fin</param>
        ''' <param name="bExentos">Parametro nulleable. Si es true or false, se buscara si es exento o no y si es nulo, todos</param>
        ''' <returns></returns>
        Public Function loadHVP(ByVal codPersona As Integer, ByVal idIzaro As Integer, ByVal fInicio As Date, ByVal fFin As Date, ByVal bExentos As Nullable(Of Boolean)) As List(Of Object)
            Try
                Dim query As String = "SELECT VIA020,VIA030,VIA040,VIA090 FROM VIAJES WHERE VIA000=:ID_EMPRESA AND VIA010=:NUM_TRABAJADOR AND VIA020>=:FECHA_INICIO AND VIA030<=:FECHA_FIN"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Int32, idIzaro, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NUM_TRABAJADOR", OracleDbType.Int32, codPersona, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_INICIO", OracleDbType.Date, fInicio.Date, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_FIN", OracleDbType.Date, fFin.Date, ParameterDirection.Input))
                If (bExentos.HasValue) Then
                    query &= " AND VIA090=:EXENTO"
                    lParametros.Add(New OracleParameter("EXENTO", OracleDbType.Char, If(bExentos.Value, "S", "N"), ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(o As OracleDataReader) New With {.FechaInicio = CDate(o("VIA020")), .FechaFin = CDate(o("VIA030")), .Estado = o("VIA040"),
                                                                                                                     .Exenta = (o("VIA090") = "S")}, query, ConexionGestionHoras, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las hojas de viajes y pernoctas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las personas que han recibido la formacion ISOS
        ''' </summary>
        ''' <param name="fIda">Fecha de ida</param>
        ''' <param name="fVuelta">Fecha de vuelta</param>
        ''' <returns></returns>
        Public Function GetPersonasConFormacionISOS(ByVal fIda As Date, ByVal fVuelta As Date) As List(Of Object)
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("select distinct u.codpersona,u.Nombre,concat(concat(u.APELLIDO1,' '),u.apellido2) as Apellidos,")
                query.AppendLine("(select count(int.id_usuario) from integrantes int where int.id_usuario=u.id and int.fecha_ida>=:FECHA_IDA and int.fecha_ida<=:FECHA_VUELTA) as Num_Viajes")
                query.AppendLine("from integrantes i inner join sab.usuarios u on i.id_usuario=u.id")
                query.AppendLine("inner join viajes v on v.id=i.id_viaje")
                query.AppendLine("left join matrizpuestos.ADOK_TRD M on M.TRD000=u.id and M.TRD001=:ID_DOCUMENTO_ISOS")
                query.AppendLine("where i.fecha_ida>=:FECHA_IDA and i.fecha_ida<=:FECHA_VUELTA and u.idplanta=1 and v.estado=1 and M.TRD000 is null")
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_DOCUMENTO_ISOS", OracleDbType.Int32, 570, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_IDA", OracleDbType.Date, fIda.Date, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FECHA_VUELTA", OracleDbType.Date, fVuelta.Date, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of Object)(Function(o As OracleDataReader) New With {.CodPersona = CInt(o("codpersona")), .NombreCompleto = (o("Nombre") & " " & SabLib.BLL.Utils.stringNull(o("Apellidos"))).Trim, .NumViajes = CInt(o("Num_viajes"))}, query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener las personas sin formacion en ISOS", ex)
            End Try
        End Function

#End Region

#Region "Convenios/Categorias"

        ''' <summary>
        ''' Obtiene el convenios/categorias especificado
        ''' </summary>
        ''' <param name="id">Id</param>
        ''' <returns></returns>        
        Public Function getConvenioCategoria(ByVal id As Integer) As ELL.ConvenioCategoria
            Try
                Dim query As String = "SELECT ID,ID_CONVENIO,ID_CATEGORIA,RECIBE_VISASANTIC,TIPO_LIQUIDACION,EMPRESA_FACTURACION FROM CONVENIOS_CATEGORIAS WHERE ID=:ID"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, id, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ConvenioCategoria)(Function(o As OracleDataReader) New ELL.ConvenioCategoria With {.Id = CInt(o("ID")), .IdConvenio = o("ID_CONVENIO"),
                                                                                        .IdCategoria = o("ID_CATEGORIA"), .RecibeVisasAntic = SabLib.BLL.Utils.booleanNull(o("RECIBE_VISASANTIC")),
                                                                                        .TipoLiquidacion = CInt(o("TIPO_LIQUIDACION")), .MostrarEmpresaFacturacion = SabLib.BLL.Utils.booleanNull(o("EMPRESA_FACTURACION"))},
                                                                                    query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el convenio/categoria", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el convenios/categorias especificados
        ''' </summary>
        ''' <param name="idConvenio">Id del convenio</param>
        ''' <param name="idCategoria">Id de la categoria</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function getConvenioCategoria(ByVal idConvenio As String, ByVal idCategoria As String, ByVal idPlanta As Integer) As ELL.ConvenioCategoria
            Try
                Dim query As String = "SELECT ID,ID_CONVENIO,ID_CATEGORIA,RECIBE_VISASANTIC,TIPO_LIQUIDACION,EMPRESA_FACTURACION FROM CONVENIOS_CATEGORIAS WHERE ID_PLANTA=:ID_PLANTA AND ID_CONVENIO=:ID_CONVENIO AND ID_CATEGORIA=:ID_CATEGORIA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_CONVENIO", OracleDbType.Varchar2, idConvenio, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_CATEGORIA", OracleDbType.Varchar2, idCategoria, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ConvenioCategoria)(Function(o As OracleDataReader) New ELL.ConvenioCategoria With {.Id = CInt(o("ID")), .IdConvenio = o("ID_CONVENIO"),
                                                                                        .IdCategoria = o("ID_CATEGORIA"), .RecibeVisasAntic = SabLib.BLL.Utils.booleanNull(o("RECIBE_VISASANTIC")),
                                                                                        .TipoLiquidacion = CInt(o("TIPO_LIQUIDACION")), .MostrarEmpresaFacturacion = SabLib.BLL.Utils.booleanNull(o("EMPRESA_FACTURACION"))},
                                                                                    query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el convenio/categoria", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los convenios/categorias marcados
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function getConveniosCategorias(ByVal idPlanta As Integer) As List(Of ELL.ConvenioCategoria)
            Try
                Dim query As String = "SELECT ID,ID_CONVENIO,ID_CATEGORIA,RECIBE_VISASANTIC,TIPO_LIQUIDACION,EMPRESA_FACTURACION FROM CONVENIOS_CATEGORIAS WHERE ID_PLANTA=:ID_PLANTA"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ConvenioCategoria)(Function(o As OracleDataReader) New ELL.ConvenioCategoria With {.Id = CInt(o("ID")), .IdConvenio = o("ID_CONVENIO"),
                                                                                        .IdCategoria = o("ID_CATEGORIA"), .RecibeVisasAntic = SabLib.BLL.Utils.booleanNull(o("RECIBE_VISASANTIC")),
                                                                                        .TipoLiquidacion = CInt(o("TIPO_LIQUIDACION")), .MostrarEmpresaFacturacion = SabLib.BLL.Utils.booleanNull(o("EMPRESA_FACTURACION"))},
                                                                                    query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los convenios/categorias", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda los convenios y categorias marcados
        ''' </summary>
        ''' <param name="lConveniosCat">Lista con los objetos</param> 
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub SaveConveniosCategorias(ByVal lConveniosCat As List(Of ELL.ConvenioCategoria), ByVal idPlanta As Integer)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                Dim query As String = String.Empty
                myConnection = New OracleConnection(cn)
                myConnection.Open()
                transact = myConnection.BeginTransaction()
                Dim lParametros As New List(Of OracleParameter)
                For Each oConv As ELL.ConvenioCategoria In lConveniosCat
                    lParametros = New List(Of OracleParameter)
                    If (oConv.Id > 0) Then
                        query = "UPDATE CONVENIOS_CATEGORIAS SET RECIBE_VISASANTIC=:RECIBE_VISASANTIC,TIPO_LIQUIDACION=:TIPO_LIQUIDACION,EMPRESA_FACTURACION=:EMPRESA_FACTURACION WHERE ID=:ID"
                        lParametros.Add(New OracleParameter("ID", OracleDbType.Int32, oConv.Id, ParameterDirection.Input))
                    Else
                        query = "INSERT INTO CONVENIOS_CATEGORIAS(ID_CONVENIO,ID_CATEGORIA,ID_PLANTA,RECIBE_VISASANTIC,TIPO_LIQUIDACION,EMPRESA_FACTURACION) VALUES (:ID_CONVENIO,:ID_CATEGORIA,:ID_PLANTA,:RECIBE_VISASANTIC,:TIPO_LIQUIDACION,:EMPRESA_FACTURACION)"
                        lParametros.Add(New OracleParameter("ID_CONVENIO", OracleDbType.Varchar2, oConv.IdConvenio, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_CATEGORIA", OracleDbType.Varchar2, oConv.IdCategoria, ParameterDirection.Input))
                        lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                    End If
                    lParametros.Add(New OracleParameter("RECIBE_VISASANTIC", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oConv.RecibeVisasAntic), ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("TIPO_LIQUIDACION", OracleDbType.Int32, oConv.TipoLiquidacion, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("EMPRESA_FACTURACION", OracleDbType.Int32, SabLib.BLL.Utils.BooleanToInteger(oConv.MostrarEmpresaFacturacion), ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, myConnection, lParametros.ToArray)
                Next
                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al guardar los convenios/categorias", ex)
            Finally
                If (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Sub

#End Region

#Region "Saldos caja"

        ''' <summary>
        ''' Obtiene los saldos de las diferentes monedas de la planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <returns>0:Cantidad,1:IdMoneda,2:Nombre mon,3:Abrev mon</returns>        
        Public Function loadSaldosCaja(ByVal idPlanta As Integer) As List(Of String())
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT SC.SALDO_RESTANTE, SC.ID_MONEDA, C.DESMON, C.CURRENCY")
                query.AppendLine("FROM SALDOS_CAJA SC INNER JOIN XBAT.COMON C ON SC.ID_MONEDA=C.CODMON")
                query.AppendLine("WHERE ID_PLANTA=:ID_PLANTA AND FECHA=(SELECT MAX(SC2.FECHA) FROM SALDOS_CAJA SC2 WHERE SC2.ID_MONEDA=SC.ID_MONEDA AND SC2.ID_PLANTA=:ID_PLANTA)")
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los saldos de caja", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los movimientos de caja
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idCurrency">Id de la planta</param>        
        ''' <param name="fInicio">Fecha de inicio</param>
        ''' <param name="fFin">Fecha de fin</param>
        ''' <param name="operation">Tipo de operacion</param>
        ''' <returns></returns>
        Public Function loadMovimientosCaja(ByVal idPlanta As Integer, ByVal idCurrency As Integer, ByVal fInicio As Date, ByVal fFin As Date, ByVal operation As ELL.SaldoCaja.EOperacion) As List(Of ELL.SaldoCaja)
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT FECHA,OPERACION,ID_MONEDA,SALDO_RESTANTE,CANTIDAD,COMENTARIO")
                query.AppendLine("FROM SALDOS_CAJA")
                query.AppendLine("WHERE ID_PLANTA=:ID_PLANTA AND ID_MONEDA=:ID_CURRENCY AND TRUNC(FECHA) BETWEEN :FINICIO AND :FFIN")
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_CURRENCY", OracleDbType.Int32, idCurrency, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FINICIO", OracleDbType.Date, fInicio, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("FFIN", OracleDbType.Date, fFin, ParameterDirection.Input))
                If (operation > -1) Then
                    query.Append(" AND OPERACION=:OPERATION")
                    lParametros.Add(New OracleParameter("OPERATION", OracleDbType.Int32, operation, ParameterDirection.Input))
                End If
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.SaldoCaja)(Function(o As OracleDataReader) New ELL.SaldoCaja With {.Fecha = CDate(o("FECHA")), .Operacion = CInt(o("OPERACION")), .IdMoneda = CInt(o("ID_MONEDA")),
                                                                                  .SaldoRestante = CDec(o("SALDO_RESTANTE")), .Comentario = SabLib.BLL.Utils.stringNull(o("COMENTARIO")), .Cantidad = CDec(o("CANTIDAD"))},
                                                                                  query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los movimientos de la caja", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda el movimiento de saldo y calculo el saldo restante
        ''' </summary>
        ''' <param name="oSaldo">Objeto con la informacion</param>
        Public Sub saveSaldoCaja(ByVal oSaldo As ELL.SaldoCaja, Optional ByVal con As OracleConnection = Nothing)
            Dim myConnection As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Try
                If (con Is Nothing) Then
                    myConnection = New OracleConnection(cn)
                    myConnection.Open()
                    transact = myConnection.BeginTransaction()
                Else
                    myConnection = con
                End If
                Dim query As String = "INSERT INTO SALDOS_CAJA(FECHA,OPERACION,ID_MONEDA,ID_USUARIO,ID_PLANTA,SALDO_RESTANTE,COMENTARIO,CANTIDAD) VALUES " _
                                    & "(:FECHA,:OPERACION,:ID_MONEDA,:ID_USUARIO,:ID_PLANTA,:SALDO_RESTANTE,:COMENTARIO,:CANTIDAD)"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("FECHA", OracleDbType.Date, oSaldo.Fecha, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("OPERACION", OracleDbType.Int32, oSaldo.Operacion, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_MONEDA", OracleDbType.Int32, oSaldo.IdMoneda, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, oSaldo.IdUsuario, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, oSaldo.IdPlanta, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("SALDO_RESTANTE", OracleDbType.Decimal, oSaldo.SaldoRestante, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, SabLib.BLL.Utils.OracleStringDBNull(oSaldo.Comentario), ParameterDirection.Input))
                lParametros.Add(New OracleParameter("CANTIDAD", OracleDbType.Decimal, oSaldo.Cantidad, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
                If (con Is Nothing) Then transact.Commit()
            Catch ex As Exception
                If (con Is Nothing) Then transact.Rollback()
                Throw New BidaiakLib.BatzException("Error al guardar el saldo de caja", ex)
            Finally
                If (con Is Nothing AndAlso (myConnection IsNot Nothing AndAlso myConnection.State <> ConnectionState.Closed)) Then
                    myConnection.Close()
                    myConnection.Dispose()
                End If
            End Try
        End Sub

#End Region

#Region "Aviso reimpresion HG"

        ''' <summary>
        ''' Obtiene la lista de hojas de gastos que tiene que reimprimir
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <returns></returns>
        Public Function HGReimprimir(ByVal idUser As Integer) As List(Of String())
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT ID_VIAJE,ID_USUARIO FROM AVISO_REIMPRIMIR_HG WHERE ID_USUARIO=:ID_USUARIO")
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUser, ParameterDirection.Input))
                Return Memcached.OracleDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al obtener los avisos de reimpresion de hojas de gastos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Añade el aviso de la hoja de gastos
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idViaje">Id del viaje</param>
        Public Sub AddHGReimprimir(ByVal idUser As Integer, ByVal idViaje As Integer)
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "INSERT INTO AVISO_REIMPRIMIR_HG(ID_VIAJE,ID_USUARIO) VALUES (:ID_VIAJE,:ID_USUARIO)"
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUser, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al guardar el aviso de la reimpresion de la hoja de gastos", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Elimina el aviso de la hoja de gastos
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idViaje">Id del viaje</param>
        Public Sub DeleteHGReimprimir(ByVal idUser As Integer, ByVal idViaje As Integer)
            Try
                Dim lParametros As New List(Of OracleParameter)
                Dim query As String = "DELETE FROM AVISO_REIMPRIMIR_HG WHERE ID_VIAJE=:ID_VIAJE AND ID_USUARIO=:ID_USUARIO"
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("ID_USUARIO", OracleDbType.Int32, idUser, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BatzException("Error al eliminar el aviso de la reimpresion de la hoja de gastos", ex)
            End Try
        End Sub

#End Region

#Region "Cuestionarios satisfaccion"

        ''' <summary>
        ''' Comprueba si existe o no el cuestionario del viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Public Function ExistCuestionario(ByVal idViaje As Integer) As Boolean
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT COUNT(ID_VIAJE)")
                query.AppendLine("FROM CUESTIONARIO")
                query.AppendLine("WHERE ID_VIAJE=:ID_VIAJE")
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, idViaje, ParameterDirection.Input))
                Return (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString, cn, lParametros.ToArray) = 1)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al comprobar si existe el cuestionario", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda los datos del cuestionario de satisfaccion
        ''' </summary>
        ''' <param name="cuest">Cuestionario</param>         
        Public Sub SaveCuestionario(ByVal cuest As Object)
            Try
                Dim query As String = "INSERT INTO CUESTIONARIO(ID_VIAJE,PREGUNTA_1,TEXTO_1,PREGUNTA_2,PREGUNTA_3,FECHA) VALUES (:ID_VIAJE,:PREG1,:TEXTO1,:PREG2,:PREG3,SYSDATE)"
                Dim lParametros As New List(Of OracleParameter)
                lParametros.Add(New OracleParameter("ID_VIAJE", OracleDbType.Int32, cuest.IdViaje, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("PREG1", OracleDbType.Int32, cuest.Answer1, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("TEXTO1", OracleDbType.NVarchar2, cuest.TextAnswer1, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("PREG2", OracleDbType.Int32, cuest.Answer2, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("PREG3", OracleDbType.Int32, cuest.Answer3, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al guardar los datos del cuestionario", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace