Namespace BLL

    Public Class BidaiakBLL

#Region "Variables y enumeraciones"

        Private bidaiakDAL As New DAL.BidaiakDAL

        ''' <summary>
        ''' Identifica los posibles permisos existentes
        ''' </summary>		
        Enum Profiles As Integer
            Consultor = 1024 'No se le puede asignar el 0 y tampoco el -1 porque al hacer el and, habría problemas
            Administrador = 1
            Planificador = 2
            Financiero = 4
            Agencia = 8
            RRHH = 16
            Documentacion_Proyectos = 32
            Gestor_Anticipos = 64  'Es solo para las notificaciones de anticipos
        End Enum

        ''' <summary>
        ''' Enumeracion de las distintas ejecuciones que hay
        ''' </summary>        
        Enum TipoEjecucion As Integer
            Factura_Eroski = 0
            Visas = 1
        End Enum

#End Region

#Region "Perfiles"

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el perfil de un usuario
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idUser">Id del usuario</param>                
        ''' <param name="idRecursoAdmon">Id del recurso admon para saber si tiene acceso directo o no</param>
        ''' <returns></returns>                
        Public Function loadProfile(ByVal idPlanta As Integer, ByVal idUser As Integer, ByVal idRecursoAdmon As Integer) As String()
            Return bidaiakDAL.loadProfile(idPlanta, idUser, idRecursoAdmon)
        End Function

        ''' <summary>
        ''' Obtiene el perfil de un usuario
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="idPerfil">Perfil</param>
        ''' <param name="idRecursoAdmon">Id del recurso admon para saber si tiene acceso directo o no</param>
        ''' <param name="bVigentes">Indica si solo se obtendran los vigentes</param>
        ''' <returns></returns>                
        Public Function loadUsersProfile(ByVal idPlanta As Integer, ByVal idPerfil As Integer, ByVal idRecursoAdmon As Integer, Optional ByVal bVigentes As Boolean = False) As List(Of String())
            Dim lUsers As List(Of String()) = bidaiakDAL.loadUsersProfile(idPlanta, idPerfil, idRecursoAdmon, bVigentes)
            For index As Integer = lUsers.Count - 1 To 0 Step -1
                If Not ((idPerfil And lUsers(index)(2)) = idPerfil) Then
                    lUsers.RemoveAt(index)
                End If
            Next
            Return lUsers
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
        ''' <param name="idRecursoAdmon">Id del recurso admon para saber si tiene acceso directo o no</param>
        ''' <param name="idGrupoAdmon">Id del grupo admon</param>        
        Public Sub setProfile(ByVal idPlanta As Integer, ByVal idUser As Integer, ByVal idPerfil As Integer, ByVal bAccesoDir As Boolean, ByVal idRecursoAdmon As Integer, ByVal idGrupoAdmon As Integer)
            Dim perfilUser As String() = loadProfile(idPlanta, idUser, idRecursoAdmon)
            Dim idPerfilUser As Integer = CInt(perfilUser(0))
            Dim accion As Integer = 1   '1:insertar/actualizar,2:borrar
            If (idPerfil = idPerfilUser And perfilUser(1) = If(bAccesoDir, "1", "0")) Then
                'Si los dos perfiles son iguales, no se realizara ningun cambio
                Exit Sub
            End If
            'If (idPerfilUser = BLL.BidaiakBLL.Profiles.Administrador) Then
            '    'Si es el perfil actual es administrador, se comprobara si se puede borrar. Tiene que haber por lo menos un administrador
            '    Dim lProfiles As List(Of String()) = loadUsersProfile(idPlanta, BLL.BidaiakBLL.Profiles.Administrador, idRecursoAdmon)
            '    If (lProfiles IsNot Nothing AndAlso lProfiles.Count = 1 AndAlso lProfiles(0)(0) = idUser AndAlso idPerfil <> BLL.BidaiakBLL.Profiles.Administrador) Then
            '        'El es el unico asi que no se puede borrar
            '        Throw New BidaiakLib.BatzException("No se puede cambiar el perfil porque tiene que haber un administrador registrado como minimo", Nothing)
            '    End If
            'End If
            If (idPerfil = BLL.BidaiakBLL.Profiles.Consultor) Then
                'Si el nuevo perfil es consultor, se borrar
                accion = 2
            Else
                'Sino, se actualizara
                accion = 1
            End If
            bidaiakDAL.setProfile(idPlanta, idUser, idPerfil, bAccesoDir, accion, idRecursoAdmon, idGrupoAdmon)
        End Sub

#End Region

#End Region

#Region "Epsilon"

        ''' <summary>
        ''' Dada un nombre y apellidos, busca alguna coincidencia en Epsilon y en caso de encontrarlo, buscara en sab por si es subcontratado y obtiene su Numero de Sab
        ''' </summary>
        ''' <param name="nombre">Nombre</param>
        ''' <param name="apellido">Primer apellido</param>
        ''' <param name="oPlanta">Objeto planta</param>
        ''' <returns></returns>
        Public Function getUsuarioSABByName(ByVal nombre As String, ByVal apellido As String, ByVal oPlanta As SabLib.ELL.Planta) As Integer
            Try
                Dim idSab As Integer = Integer.MinValue
                'Se busca en Epsilon
                Dim nombres As List(Of String()) = bidaiakDAL.SearchByNameEpsilon(nombre, apellido, oPlanta)
                Dim dni As String = String.Empty
                If (nombres.Count = 1) Then
                    dni = nombres.Item(0)(0).Trim
                Else  'Si Epsilon no devuelve ningun resultado, se busca en SAB por si es subcontratatado
                    nombres = bidaiakDAL.SearchByNameSAB(nombre, apellido, oPlanta.Id)
                    If (nombres.Count = 1 AndAlso nombres.Item(0)(0) IsNot Nothing) Then dni = nombres.Item(0)(0).Trim
                End If
                If (dni <> String.Empty) Then
                    Dim userBLL As New SabLib.BLL.UsuariosComponent
                    Dim oUser As New SabLib.ELL.Usuario With {.Dni = dni}
                    oUser = userBLL.GetUsuario(oUser, False)
                    If (oUser IsNot Nothing) Then idSab = oUser.Id
                End If
                Return idSab
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al buscar el nombre en Epsilon o en SAB", ex)
            End Try
        End Function

        ''' <summary>
        ''' Dada un nombre y apellidos y un viaje, busca alguna coincidencia en los integrantes de un viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>
        ''' <param name="nombre">Nombre</param>
        ''' <param name="apellido">Primer apellido</param>
        ''' <returns></returns>
        Function getUsuarioSABByName(ByVal idViaje As Integer, ByVal nombre As String, ByVal apellido As String) As Integer
            Try
                Dim idSab As Integer = Integer.MinValue
                Dim viajesBLL As New BLL.ViajesBLL
                Dim lIntegr As List(Of ELL.Viaje.Integrante) = viajesBLL.loadIntegrantes(idViaje)
                lIntegr = lIntegr.FindAll(Function(o As ELL.Viaje.Integrante) o.Usuario.Nombre.ToLower Like "*" & nombre.ToLower & "*" AndAlso o.Usuario.Apellido1.ToLower Like "*" & apellido.ToLower & "*")
                If (lIntegr.Count = 1) Then idSab = lIntegr.First.Usuario.Id
                Return idSab
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al buscar el nombre entre los integrantes del viaje", ex)
            End Try
        End Function

#End Region

#Region "Parametros"

        ''' <summary>
        ''' Carga los parametros de una planta
        ''' </summary>
        ''' <returns></returns>      
        Public Function loadParameters(ByVal idPlanta As Integer) As ELL.Parametro
            Return bidaiakDAL.loadParameters(idPlanta)
        End Function

        ''' <summary>
        ''' Consulta las distintas plantas que funcionan como plantas en la aplicacion
        ''' </summary>
        ''' <returns></returns>        
        Public Function loadApplicationPlantas() As List(Of String())
            Return bidaiakDAL.loadApplicationPlantas()
        End Function

        ''' <summary>
        ''' Guarda los parametros
        ''' </summary>
        ''' <param name="parametros">Parametros</param> 
        Public Sub saveParameters(ByVal parametros As ELL.Parametro)
            bidaiakDAL.saveParameters(parametros)
        End Sub

#End Region

#Region "Ejecuciones"

        Public Class Ejecucion

            Public Fecha As Date = Date.MinValue
            Public IdUser As Integer = Integer.MinValue
            Public Paso As Integer = 0
            Public IdPlanta As Integer = Integer.MinValue
            Public Tipo As BLL.BidaiakBLL.TipoEjecucion = TipoEjecucion.Visas
            Public Anno As Integer = 0
            Public Mes As Integer = 0
            Public NombreFichero As String = String.Empty
            Public Fichero As Byte() = Nothing

        End Class

        ''' <summary>
        ''' Obtiene la informacion de ejecucion de la planta
        ''' </summary>
        ''' <param name="tipoEjec">Tipo de ejecucion</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function loadEjecucion(ByVal tipoEjec As BLL.BidaiakBLL.TipoEjecucion, ByVal idPlanta As Integer) As Ejecucion
            Return bidaiakDAL.loadEjecucion(tipoEjec, idPlanta)
        End Function

        ''' <summary>
        ''' Inserta el paso de ejecucion del usuario de la planta
        ''' </summary>
        ''' <param name="ejec">Objeto ejecucion</param>        
        Sub initEjecucion(ByVal ejec As Ejecucion)
            bidaiakDAL.initEjecucion(ejec)
        End Sub

        ''' <summary>
        ''' Actualiza el paso del usuario de la planta
        ''' </summary>
        ''' <param name="ejec">Objeto ejecucion</param> 
        Sub saveEjecucion(ByVal ejec As Ejecucion)
            Dim oEjec As Ejecucion = loadEjecucion(ejec.Tipo, ejec.IdPlanta)
            If (oEjec IsNot Nothing AndAlso oEjec.IdUser <> ejec.IdUser) Then
                Throw New BidaiakLib.BatzException("No se puede actualizar la informacion de la ejecucion porque el usuario de ejecucion es distinto del que se intenta insertar", Nothing)
            Else
                bidaiakDAL.saveEjecucion(ejec)
            End If
        End Sub

        ''' <summary>
        ''' Borra la informacion de la planta
        ''' </summary>        
        ''' <param name="tipoEjec">Tipo de ejecucion</param>
        ''' <param name="idPlanta">Id de la planta</param>
        Sub deleteEjecucion(ByVal tipoEjec As BLL.BidaiakBLL.TipoEjecucion, ByVal idPlanta As Integer)
            bidaiakDAL.deleteEjecucion(tipoEjec, idPlanta)
        End Sub

#End Region

#Region "Busqueda usuarios"

        ''' <summary>
        ''' Busca primero en Epsilon y despues en SAB el texto introducido        
        ''' </summary>
        ''' <param name="texto">Texto a buscar</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Lista de array de strings (0:codPersona, 1:Nombre completo, 2:Origen-E(epsilon) / S(sab),3:Dado de baja(1))</returns>        
        Function findPersons(ByVal texto As String, ByVal idPlanta As Integer) As List(Of String())
            Try
                Dim lPersonas As New List(Of String())
                Dim departBLL As New Sablib.BLL.DepartamentosComponent
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                Dim plantBLL As New SabLib.BLL.PlantasComponent
                Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(idPlanta)
                Dim oDepart As Sablib.ELL.Departamento
                Dim nombre As String() = GetNombreApellidos(texto)
                Dim lPersoEpsilon As List(Of String()) = SearchByNameEpsilon(nombre(0), nombre(1), nombre(2), oPlant)
                If (lPersoEpsilon IsNot Nothing AndAlso lPersoEpsilon.Count > 0) Then
                    Dim dadoBaja As Integer
                    For Each sPerso As String() In lPersoEpsilon
                        dadoBaja = 0
                        If (Not String.IsNullOrEmpty(sPerso(5))) Then
                            If (CDate(sPerso(5)) < CDate(Now.ToShortDateString)) Then dadoBaja = 1
                        End If
                        lPersonas.Add(New String() {sPerso(4), sPerso(1) & " " & sPerso(2) & " " & sPerso(3), "E", dadoBaja, sPerso(6)})
                    Next
                Else
                    Dim lUsuarios As List(Of Sablib.ELL.Usuario)
                    Dim userComp As New Sablib.BLL.UsuariosComponent
                    lUsuarios = userComp.GetUsuariosBusquedaSAB_Optimizado(texto, idPlanta:=idPlanta)
                    If (lUsuarios IsNot Nothing AndAlso lUsuarios.Count > 0) Then
                        For Each oUser As Sablib.ELL.Usuario In lUsuarios
                            If (oUser.CodPersona <> Integer.MinValue) Then  'En algunos casos se filtraban algunos                                
                                oDepart = departBLL.GetDepartamento(New Sablib.ELL.Departamento With {.Id = oUser.IdDepartamento, .IdPlanta = oUser.IdPlanta})
                                lPersonas.Add(New String() {oUser.CodPersona, oUser.NombreCompleto, "S", If(oUser.DadoBaja, "1", "0"), If(oDepart Is Nothing, String.Empty, oDepart.Nombre)})
                            End If
                        Next
                    End If
                End If
                Return lPersonas
            Catch batzEx As BidaiakLib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al buscar las personas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Busca por nombre y apellidos en Epsilon
        ''' </summary>
        ''' <param name="nombre">Nombre</param>
        ''' <param name="apellido1">Primer apellido</param>
        ''' <param name="apellido2">Segundo apellido</param>
        ''' <param name="oPlanta">Objeto planta</param>
        ''' <returns></returns>        
        Private Function SearchByNameEpsilon(ByVal nombre As String, ByVal apellido1 As String, ByVal apellido2 As String, ByVal oPlanta As SabLib.ELL.Planta) As List(Of String())
            Dim lResul As List(Of String())
            Dim query As String
            Dim lParametros As New List(Of SqlClient.SqlParameter)
            Dim parameter As SqlClient.SqlParameter
            Dim countInformed As Integer = 0
            If (nombre.Trim <> String.Empty) Then countInformed += 1
            If (apellido1.Trim <> String.Empty) Then countInformed += 1
            If (apellido2.Trim <> String.Empty) Then countInformed += 1

            query = "SELECT DISTINCT P.NIF,P.NOMBRE,P.APELLIDO1,P.APELLIDO2,T.ID_TRABAJADOR,T.F_BAJA,NO.D_NIVEL,NO.ID_NIVEL "
            query &= "FROM TRABAJADORES T INNER JOIN COD_TRA C ON T.ID_TRABAJADOR=C.ID_TRABAJADOR "
            query &= "INNER JOIN PERSONAS P ON P.NIF=C.NIF "
            query &= "INNER JOIN PUES_TRAB PT ON T.ID_TRABAJADOR=PT.ID_TRABAJADOR "
            query &= "INNER JOIN  ORDEN O  ON PT.ID_NIVEL=O.ID_NIVEL_HIJO AND O.ID_ORGANIG=PT.ID_ORGANIG "
            query &= "INNER JOIN NIV_ORG NO ON  O.N4=NO.ID_NIVEL AND O.ID_ORGANIG=NO.ID_ORGANIG "
            query &= "INNER JOIN ORGANIG_EMPRESAS OE ON OE.ID_ORGANIG=NO.ID_ORGANIG "
            query &= "INNER JOIN PARAMETROS PA ON PA.ORG_DEFECTO=OE.ID_ORGANIG "

            If (countInformed = 1) Then
                query &= "WHERE (P.NOMBRE LIKE + '%' +  @TEXTO1 + '%' OR P.APELLIDO1 LIKE + '%' +  @TEXTO1 + '%' OR P.APELLIDO2 LIKE + '%' +  @TEXTO1 + '%')"
                parameter = New SqlClient.SqlParameter("TEXTO1", SqlDbType.NVarChar, 20) : parameter.Value = nombre : lParametros.Add(parameter)
            ElseIf (countInformed = 2) Then
                'Se buscan los coincidentes en los dos campos  
                query &= "WHERE ((P.NOMBRE LIKE + '%' +  @TEXTO1 + '%' AND P.APELLIDO1 LIKE + '%' +  @TEXTO2 + '%') OR " _
                               & "(P.NOMBRE LIKE + '%' +  @TEXTO1 + '%' AND P.APELLIDO2 LIKE + '%' +  @TEXTO2 + '%') OR " _
                               & "(P.APELLIDO1 LIKE + '%' +  @TEXTO1 + '%' AND P.APELLIDO2 LIKE + '%' +  @TEXTO2 + '%'))"

                Dim texto1, texto2 As String
                texto1 = String.Empty : texto2 = String.Empty
                If (nombre <> String.Empty) Then texto1 = nombre
                If (apellido1 <> String.Empty) Then
                    If (texto1 = String.Empty) Then
                        texto1 = apellido1
                    Else
                        texto2 = apellido1
                    End If
                End If
                If (apellido2 <> String.Empty) Then
                    texto2 = apellido2
                End If
                parameter = New SqlClient.SqlParameter("TEXTO1", SqlDbType.NVarChar, 20) : parameter.Value = texto1 : lParametros.Add(parameter)
                parameter = New SqlClient.SqlParameter("TEXTO2", SqlDbType.NVarChar, 20) : parameter.Value = texto2 : lParametros.Add(parameter)
            ElseIf (countInformed = 3) Then
                'Se buscan los coincidentes en los dos campos  
                query &= "WHERE ((P.NOMBRE LIKE + '%' +  @TEXTO1 + '%' AND P.APELLIDO1 LIKE + '%' +  @TEXTO2 + '%' AND P.APELLIDO2 LIKE + '%' +  @TEXTO3 + '%'))"

                parameter = New SqlClient.SqlParameter("TEXTO1", SqlDbType.NVarChar, 20) : parameter.Value = nombre : lParametros.Add(parameter)
                parameter = New SqlClient.SqlParameter("TEXTO2", SqlDbType.NVarChar, 20) : parameter.Value = apellido1 : lParametros.Add(parameter)
                parameter = New SqlClient.SqlParameter("TEXTO3", SqlDbType.NVarChar, 20) : parameter.Value = apellido2 : lParametros.Add(parameter)
            End If
            query &= " AND (PA.ID_EMPRESA=@ID_EMPRESA AND (PT.F_FIN_PUE IS NULL OR PT.F_INI_PUE<=GETDATE()))"
            query &= " AND PT.F_INI_PUE=(SELECT MIN(PT2.F_INI_PUE) AS F_INI_PUE FROM PUES_TRAB PT2 WHERE ((PT2.ID_ORGANIG = PT.ID_ORGANIG And PT2.ID_EMPRESA = PT.ID_EMPRESA And PT2.ID_TRABAJADOR = PT.ID_TRABAJADOR) AND (PT2.F_INI_PUE<=GETDATE() AND (PT2.F_FIN_PUE IS NULL OR PT2.F_FIN_PUE>=GETDATE()))))"
            'query &= " GROUP BY P.NIF,P.NOMBRE,P.APELLIDO1,P.APELLIDO2,T.ID_TRABAJADOR,NO.D_NIVEL,NO.ID_NIVEL"
            parameter = New SqlClient.SqlParameter("ID_EMPRESA", SqlDbType.Char, 5) : parameter.Value = oPlanta.IdEpsilon : lParametros.Add(parameter)

            lResul = Memcached.SQLServerDirectAccess.Seleccionar(query, oPlanta.NominasConnectionString, lParametros.ToArray)
            Return lResul
        End Function

        ''' <summary>
        ''' Obtiene un array de string, con el nombre y apellidos
        ''' </summary>
        ''' <param name="cadena">Cadena que contiene el nombre completo</param>
        ''' <returns></returns>
        Public Function GetNombreApellidos(ByVal cadena As String) As String()
            Dim nombreAp() As String = {"", "", ""}
            Dim s() As String
            Dim sAux() As String
            Dim nombreUser As String = String.Empty
            If (cadena.IndexOf(",") <> -1) Then  'Apellidos, Nombre
                s = cadena.Split(",")
                If (s.Length > 0) Then
                    nombreAp(0) = s(1)  'Nombre
                    sAux = s(0).Split(" ")
                    If (sAux.Length = 1) Then
                        nombreAp(1) = sAux(0)
                    ElseIf (sAux.Length = 2) Then
                        nombreAp(1) = sAux(0)
                        nombreAp(2) = sAux(1)
                    ElseIf (sAux.Length = 3) Then
                        nombreAp(1) = sAux(0) & " " & sAux(1)  'Apellido 1
                        nombreAp(2) = sAux(2)  'Apellido 2
                    ElseIf (sAux.Length = 4) Then
                        nombreAp(1) = sAux(0) & " " & sAux(1)  'Apellido 1
                        nombreAp(2) = sAux(2) & " " & sAux(3) 'Apellido 2
                    ElseIf (s.Length = 5) Then
                        nombreAp(1) = sAux(0) & " " & sAux(1) & " " & sAux(2) 'Apellido 1
                        nombreAp(2) = sAux(3) & " " & sAux(4)  'Apellido 2
                    End If
                End If
            Else  'Nombre apellidos        
                If (cadena.IndexOf("""") <> -1) Then
                    Dim aux As String() = cadena.Split("""")
                    Dim aux2 As String = String.Empty
                    For index As Integer = 1 To aux.Count - 1
                        If (aux2 <> String.Empty) Then aux2 &= " "
                        aux2 &= aux(index).Trim
                    Next
                    If (aux2 <> String.Empty) Then
                        ReDim s(aux2.Split(" ").Count)
                        s(0) = aux(0)
                        aux = aux2.Split(" ")
                        For index As Integer = 0 To aux.Count - 1
                            s(index + 1) = aux(index)
                        Next
                    Else
                        s = New String() {aux(0)}
                    End If
                Else
                    s = cadena.Split(" ")
                End If
                If (s.Length > 0) Then
                    nombreAp(0) = s(0)
                    If (s.Length = 2) Then
                        nombreAp(1) = s(1)
                    ElseIf (s.Length = 3) Then
                        nombreAp(1) = s(1)
                        nombreAp(2) = s(2)
                    ElseIf (s.Length = 4) Then
                        nombreAp(0) &= " " & s(1)
                        nombreAp(1) = s(2)
                        nombreAp(2) = s(3)
                    ElseIf (s.Length = 5) Then
                        nombreAp(0) &= " " & s(1)
                        nombreAp(1) = s(2) & " " & s(3)
                        nombreAp(2) = s(4)
                    End If
                End If
            End If

            Return nombreAp
        End Function

#End Region

#Region "Validador departamento"

        ''' <summary>
        ''' Primero comprueba si su dpto tiene un validador excepcional.
        ''' Sino, devuelve el que marque su estructura
        ''' </summary>
        ''' <param name="idUser">Id de usuario</param>
        ''' <param name="idTrab">Id de trabajador</param>
        ''' <param name="idDepto">Id del departamento</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function GetResponsable(ByVal idUser As Integer, ByVal idTrab As Integer, ByVal idDepto As String, ByVal idPlanta As Integer) As Integer
            Dim idResp As Integer = getValidadorDpto(idDepto, idPlanta)
            If (idResp <= 0) Then
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                idResp = userBLL.GetResponsable(idUser, idTrab)
            End If

            Return idResp
        End Function

        ''' <summary>
        ''' Comprueba si su dpto tiene un validador excepcional.
        ''' Sino, devuelve el idResp
        ''' </summary>
        ''' <param name="dpto">Dpto a comprobar</param>
        ''' <param name="idResp">Id del responsable de su estructura</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function GetResponsable(ByVal dpto As String, ByVal idResp As Integer, ByVal idPlanta As Integer) As Integer
            Dim idRespDpto As Integer = getValidadorDpto(dpto, idPlanta)
            If (idRespDpto <= 0) Then idRespDpto = idResp

            Return idRespDpto
        End Function

        ''' <summary>
        ''' Dado un dpto, busca si tiene asociado un validador
        ''' </summary>
        ''' <param name="dpto">Departamento</param>        
        ''' <returns></returns>        
        Function getValidadorDpto(ByVal dpto As String, ByVal idPlanta As Integer) As Integer
            Return bidaiakDAL.getValidadorDpto(dpto, idPlanta)
        End Function

        ''' <summary>
        ''' Devuelve todos los registros de validadores departamentos de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <returns></returns>        
        Function getValidadoresDptos(ByVal idPlanta As String) As List(Of String())
            Return bidaiakDAL.getValidadoresDptos(idPlanta)
        End Function

        ''' <summary>
        ''' Inserta un validador y un departamento
        ''' </summary>
        ''' <param name="dpto">Departamento</param>
        ''' <param name="idVal">Id del validador</param>        
        ''' <param name="idPlanta">Id de la planta</param>
        Sub addValidadorDpto(ByVal dpto As String, ByVal idVal As Integer, ByVal idPlanta As Integer)
            bidaiakDAL.addValidadorDpto(dpto, idVal, idPlanta)
        End Sub

        ''' <summary>
        ''' Borra la informacion de un departamento
        ''' </summary>        
        ''' <param name="dpto">Departamento</param>        
        ''' <param name="idPlanta">Id de la planta</param>  
        Sub deleteValidadorDpto(ByVal dpto As String, ByVal idPlanta As Integer)
            bidaiakDAL.deleteValidadorDpto(dpto, idPlanta)
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
            Return bidaiakDAL.loadCuentaContrapartida(idPlantaGestion, idPlantaCta)
        End Function

        ''' <summary>
        ''' Obtiene todas las cuentas de contrapartida
        ''' </summary>        
        ''' <param name="idPlantaGestion">Id de la planta de gestion</param>
        ''' <returns></returns>
        Function loadCuentasContrapartida(ByVal idPlantaGestion As Integer) As List(Of ELL.CuentaContrapartida)
            Return bidaiakDAL.loadCuentasContrapartida(idPlantaGestion)
        End Function

        ''' <summary>
        ''' Se guarda la informacion de la cuenta
        ''' </summary>
        ''' <param name="oInfo">Informacion</param>        
        Sub SaveCuentaContrapartida(ByVal oInfo As ELL.CuentaContrapartida)
            bidaiakDAL.SaveCuentaContrapartida(oInfo)
        End Sub

        ''' <summary>
        ''' Se borra la informacion de la cuenta de la planta
        ''' </summary>   
        ''' <param name="idPlantaGestion">Id de la planta que gestiona las cuentas</param>
        ''' <param name="idPlantaCta">Id de la planta a la que se le asigna las cuentas</param>   
        Sub DeleteCuentaContrapartida(ByVal idPlantaGestion As Integer, ByVal idPlantaCta As Integer)
            bidaiakDAL.DeleteCuentaContrapartida(idPlantaGestion, idPlantaCta)
        End Sub

#End Region

#Region "Cuenta Visa Excepcion (COMENTADO)"

        '''' <summary>
        '''' Obtiene la cuenta de las visas de excepcion
        '''' </summary>        
        '''' <param name="idPlantaGestion">Id de la planta de gestion</param>
        '''' <returns></returns>
        'Function loadCuentaVisaExcepcion(ByVal idPlantaGestion As Integer) As Object
        '    Return bidaiakDAL.loadCuentaVisaExcepcion(idPlantaGestion)
        'End Function

        '''' <summary>
        '''' Se guarda la informacion de la cuenta
        '''' </summary>
        '''' <param name="oInfo">Informacion</param>        
        'Sub SaveCuentaVisaExcepcion(ByVal oInfo As Object)
        '    bidaiakDAL.SaveCuentaVisaExcepcion(oInfo)
        'End Sub

#End Region

#Region "Gerentes plantas"

        ''' <summary>
        ''' Obtiene el gerente de la planta
        ''' </summary>
        ''' <param name="idPlanta">IdPlanta</param>
        ''' <returns></returns>        
        Public Function loadGerentePlanta(ByVal idPlanta As Integer) As Sablib.ELL.Usuario
            Dim sGerente As String() = bidaiakDAL.loadGerentePlanta(idPlanta)
            Dim oUser As Sablib.ELL.Usuario = Nothing
            If (sGerente IsNot Nothing) Then
                Dim userBLL As New Sablib.BLL.UsuariosComponent
                oUser = userBLL.GetUsuario(New Sablib.ELL.Usuario With {.Id = CInt(sGerente(0))}, False)
            End If
            Return oUser
        End Function

        ''' <summary>
        ''' Obtiene todas las plantas con la relacion de sus gerentes
        ''' </summary>
        ''' <returns></returns>      
        Public Function loadGerentesPlantas() As List(Of String())
            Return bidaiakDAL.loadGerentesPlantas()
        End Function

        ''' <summary>
        ''' Obtiene las plantas de las que es gerente un usuario
        ''' </summary>
        ''' <returns></returns>      
        Public Function loadGerentesPlantas(idUser) As List(Of String())
            Return bidaiakDAL.loadGerentesPlantas(idUser)
        End Function

        ''' <summary>
        ''' Guarda los datos de los gerentes
        ''' </summary>
        ''' <param name="idGerente">Id del gerente</param> 
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub saveGerentePlanta(ByVal idGerente As Integer, ByVal idPlanta As Integer)
            bidaiakDAL.saveGerentePlanta(idGerente, idPlanta)
        End Sub

#End Region

#Region "Importaciones documentos"

        Public Class Importacion

            Public Id As Integer = Integer.MinValue
            Public Fecha As Date = Date.MinValue
            Public NumRegistros As Integer = 0
            Public ConViaje As Integer = 0
            Public SinViaje As Integer = 0
            Public IdPlanta As Integer = Integer.MinValue
            Public Tipo As BidaiakBLL.TipoEjecucion = TipoEjecucion.Visas
            Public Anno As Integer = 0
            Public Mes As Integer = 0
            Public NombreFichero As String = String.Empty
            Public Fichero As Byte() = Nothing

        End Class

        ''' <summary>
        ''' Obtiene la informacion la importacion del documento
        ''' </summary>
        ''' <param name="id">Id de la importacion</param>        
        ''' <returns></returns>        
        Function loadImportacionDoc(ByVal id As Integer) As Importacion
            Return bidaiakDAL.loadImportacionDoc(id)
        End Function

        ''' <summary>
        ''' Obtiene la informacion la importacion del documento en un año y mes en concreto
        ''' </summary>
        ''' <param name="tipoDoc">Tipo de la importacion.1:Visas,2:Factura Eroski</param>        
        ''' <param name="año">Año a consultar</param>
        ''' <param name="mes">Mes a consultar</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function loadImportacionDoc(ByVal tipoDoc As Integer, ByVal año As Integer, ByVal mes As Integer, ByVal idPlanta As Integer) As Importacion
            Return bidaiakDAL.loadImportacionDoc(tipoDoc, año, mes, idPlanta)
        End Function

#End Region

#Region "Importar Departamentos personas"

        ''' <summary>
        ''' Guarda en un fichero todas las personas junto con sus departamentos obtenidos en la consulta
        ''' </summary>        
        ''' <param name="rutaTemp">Ruta del directorio donde se guardara el fichero generado</param>                
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Numero de registros exportados</returns>        
        Public Function ExportarDepartamentosPersonas(ByVal rutaTemp As String, ByVal idPlanta As Integer) As Integer
            Dim fileStream As System.IO.StreamWriter = Nothing
            Try
                Dim cont As Integer = 0
                Dim epsilonBLL As New BLL.Epsilon(idPlanta)
                Dim userBLL As New Sablib.BLL.UsuariosComponent
                Dim lPersonFile As New List(Of String())
                fileStream = New System.IO.StreamWriter(rutaTemp, False, Text.Encoding.UTF8)
                Dim lPersonasEpsilon As List(Of String()) = epsilonBLL.GetDepartamentosPersonasEpsilon() '1º Se obtienen los trabajadores de Batz                
                '070619:Los subcontratados no van a aparecer
                'Dim lPersonasSubcontratadas As List(Of String()) = GetDepartamentosPersonasSubcontratadas(idPlanta) '2º Se obtienen los subcontratados
                If (lPersonasEpsilon IsNot Nothing AndAlso lPersonasEpsilon.Count > 0) Then lPersonFile.AddRange(lPersonasEpsilon)
                'If (lPersonasSubcontratadas IsNot Nothing AndAlso lPersonasSubcontratadas.Count > 0) Then lPersonFile.AddRange(lPersonasSubcontratadas)
                lPersonFile.Sort(Function(o1 As String(), o2 As String()) Trim(o1(3)) < Trim(o2(3)))
                Dim linea As String = "Sociedad;Negocio;Departamento;NumSocio;Trabajador;Email"
                fileStream.WriteLine(linea)
                'Dim sociedad As String = If(idPlanta = 1, "Batz S Coop", "Batz Energy")
                Dim sociedad As String = "Batz S Coop"
                For Each sInfo As String() In lPersonFile
                    linea = sociedad & ";" & sInfo(0).Trim & ";" & sInfo(1).Trim & ";" & sInfo(2).Trim & ";" & sInfo(3).Trim & ";" & sInfo(4).Trim
                    fileStream.WriteLine(linea)
                Next
                cont = lPersonFile.Count
                Return cont
            Catch ex As Exception
                Throw New BatzException("Error al exportar los datos de los departamentos personas al fichero csv", ex)
            Finally
                If (fileStream IsNot Nothing) Then fileStream.Close()
            End Try
        End Function

        ''' <summary>
        ''' Obtiene las personas junto con su departamento
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Function GetDepartamentosPersonasSubcontratadas(ByVal idPlanta As Integer) As List(Of String())
            Dim lPersonas As List(Of String()) = bidaiakDAL.GetDepartamentosPersonasSubcontratadas(idPlanta) '0:Id,1:Codpersona,2:idDepto,3:Nombre completo,4:email
            Dim lResul As New List(Of String())
            Dim epsilonBLL As New BLL.Epsilon(idPlanta)
            Dim sInfo As String()
            For Each sPerso As String() In lPersonas
                sInfo = epsilonBLL.GetNegocioDepartamentoSubcontratados(sPerso(2))
                If (sInfo IsNot Nothing) Then lResul.Add(New String() {sInfo(0).Trim, sInfo(1).Trim, sPerso(1).Trim, sPerso(3).Trim, If(sPerso(4) Is Nothing, String.Empty, sPerso(4).Trim)})
            Next
            Return lResul
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
            Return bidaiakDAL.loadCondicionesEspeciales(idPlanta, bSoloVigentes)
        End Function

        ''' <summary>
        ''' Dada una cultura y el formato de los decimales, devuelve el decimal con la coma o punto
        ''' </summary>
        ''' <param name="sDec">Numero a convertir</param>
        ''' <returns></returns>	
        Public Shared Function DecimalValue(ByVal sDec As String) As Decimal
            If (Not String.IsNullOrEmpty(sDec)) Then
                Dim myDec As String = String.Empty
                If (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ",") Then
                    myDec = sDec.Trim.Replace(".", ",")
                ElseIf (Threading.Thread.CurrentThread.CurrentCulture.NumberFormat.NumberDecimalSeparator = ".") Then
                    myDec = sDec.Trim.Replace(",", ".")
                End If
                If (myDec.StartsWith(",") Or myDec.StartsWith(".")) Then myDec = "0" & myDec
                myDec = If(myDec = String.Empty, "0", myDec)
                Return Convert.ToDecimal(myDec, Threading.Thread.CurrentThread.CurrentCulture.NumberFormat)
            Else
                Return 0
            End If
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
            Return bidaiakDAL.loadHVP(codPersona, idIzaro, fInicio, fFin, bExentos)
        End Function

        ''' <summary>
        ''' Obtiene las personas que han viajado entre fechas y que no han recibido nunca la formacion ISOS
        ''' </summary>
        ''' <param name="fIda">Fecha de ida del viaje</param>
        ''' <param name="fVuelta">Fecha de vuelta</param>
        ''' <returns></returns>
        Public Function GetPersonasViajerasSinFormacionISOS(ByVal fIda As Date, ByVal fVuelta As Date) As List(Of Object)
            Return bidaiakDAL.GetPersonasConFormacionISOS(fIda, fVuelta)
        End Function

#End Region

#Region "Test (COMENTADO)"

        ' ''' <summary>
        ' ''' Intenta leer el excel y devuelve el texto de la primera linea
        ' ''' </summary>
        ' ''' <param name="fileName">Excel</param>
        ' ''' <returns></returns>        
        'Public Function TestReadExcel(ByVal fileName As String) As String
        '    Dim conexionExcel As ADODB.Connection
        '    Dim rs As ADODB.Recordset = Nothing
        '    Dim num As Integer = 1
        '    Dim texto As String = String.Empty
        '    Try
        '        conexionExcel = New ADODB.Connection
        '        'HDR=Yes => para que la primera fila, no la lea ya que son las cabeceras
        '        'IMAX=1  => para que todos los valores de las columnas, las lea como texto. Sino se indica, suele tomar como tipo de datos el de los primeros valores. Si los primeros valores, son textos, pero hay uno de numeros, este ultimo devolvera blanco
        '        Dim excelConection As String = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=""Excel {3};HDR=YES;IMEX=1;"""
        '        excelConection = String.Format(excelConection, "ACE", "12.0", fileName, "12.0")
        '        conexionExcel.Open(excelConection)

        '        ' Nuevo recordset  
        '        rs = New ADODB.Recordset
        '        With rs
        '            .CursorLocation = ADODB.CursorLocationEnum.adUseClient
        '            .CursorType = ADODB.CursorTypeEnum.adOpenStatic
        '            .LockType = ADODB.LockTypeEnum.adLockOptimistic
        '        End With

        '        'hoja = "2110201094745"  'Mirar a ver como se accede a la hoja 1
        '        Dim hoja As String = GetExcelSheetName(conexionExcel)
        '        rs.Open("SELECT * FROM [" & hoja & "]", conexionExcel, , )

        '        While Not rs.EOF
        '            texto = "Campo 1:" & rs(0).Value.ToString & "| Campo 1:" & rs(1).Value.ToString
        '            rs.MoveNext()
        '            Exit While
        '        End While

        '        Return texto
        '    Catch batzEx As BidaiakLib.BatzException
        '        Throw batzEx
        '    Catch ex As Exception
        '        Throw New BidaiakLib.BatzException("Error al realizar el test de lectura del excel=> " & fileName, ex)
        '    Finally
        '        If (rs IsNot Nothing) Then
        '            rs.ActiveConnection = Nothing
        '            If (rs.State <> 0) Then rs.Close()
        '        End If
        '    End Try
        'End Function

        ' ''' <summary>
        ' ''' Obtiene el nombre de la hoja del excel
        ' ''' </summary>
        ' ''' <param name="conection">Conexion al fichero excel</param>
        ' ''' <returns></returns>        
        'Private Function GetExcelSheetName(ByVal conection As ADODB.Connection) As String
        '    Dim oRs As ADODB.Recordset = Nothing
        '    Try
        '        oRs = conection.OpenSchema(ADODB.SchemaEnum.adSchemaTables)
        '        Dim excelSheets As String = String.Empty
        '        Do While Not oRs.EOF
        '            excelSheets = oRs.Fields("table_name").Value
        '            Exit Do
        '        Loop

        '        Return excelSheets
        '    Catch ex As Exception
        '        Throw New BidaiakLib.BatzException("Error al consultar el nombre de las hojas del excel", ex)
        '    Finally
        '        If (oRs IsNot Nothing) Then oRs.Close()
        '    End Try
        'End Function

#End Region

#Region "Convenios/Categorias"

        ''' <summary>
        ''' Obtiene la informacion del convenio/categoria
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <param name="id"></param>
        ''' <returns></returns>        
        Public Function getConvenioCategoria(ByVal idPlanta As Integer, ByVal id As Integer) As ELL.ConvenioCategoria
            Dim oConvCat As ELL.ConvenioCategoria = bidaiakDAL.getConvenioCategoria(id)
            Dim epsilonBLL As New BLL.Epsilon(idPlanta)
            Dim sConvCat As String() = epsilonBLL.getConvenioCategoria(oConvCat.IdConvenio, oConvCat.IdCategoria)
            If (sConvCat IsNot Nothing) Then
                oConvCat.Convenio = sConvCat(0)
                oConvCat.Categoria = sConvCat(1)
            End If
            Return oConvCat
        End Function

        ''' <summary>
        ''' Obtiene los convenios y categorias marcados de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function getConveniosCategorias(ByVal idPlanta As Integer) As List(Of ELL.ConvenioCategoria)
            Dim epsilonBLL As New BLL.Epsilon(idPlanta)
            Dim lConvCatEpsilon As List(Of String()) = epsilonBLL.getConveniosCategorias()
            Dim lConvCatPlanta As List(Of ELL.ConvenioCategoria) = bidaiakDAL.getConveniosCategorias(idPlanta)            
            Dim oConvCat As ELL.ConvenioCategoria
            Dim sConvCat As String()
            For index As Integer = lConvCatPlanta.Count - 1 To 0 Step -1
                oConvCat = lConvCatPlanta.Item(index)
                sConvCat = lConvCatEpsilon.Find(Function(o As String()) o(0) = oConvCat.IdConvenio And o(2) = oConvCat.IdCategoria)
                If (sConvCat IsNot Nothing) Then
                    oConvCat.Convenio = sConvCat(1)
                    oConvCat.Categoria = sConvCat(3)
                    lConvCatEpsilon.Remove(sConvCat)
                Else
                    lConvCatPlanta.RemoveAt(index)
                End If
            Next
            If (lConvCatEpsilon.Count > 0) Then  'Son convenios nuevos en Epsilon
                For Each sConvCat In lConvCatEpsilon
                    lConvCatPlanta.Add(New ELL.ConvenioCategoria With {.IdConvenio = sConvCat(0), .Convenio = sConvCat(1), .IdCategoria = sConvCat(2), .Categoria = sConvCat(3), .RecibeVisasAntic = False, .TipoLiquidacion = -1})
                Next
            End If
            lConvCatPlanta.Sort(Function(o1 As ELL.ConvenioCategoria, o2 As ELL.ConvenioCategoria) o1.Convenio < o2.Convenio)
            Return lConvCatPlanta
        End Function

        ''' <summary>
        ''' Guarda los convenios y categorias marcados
        ''' </summary>
        ''' <param name="lConveniosCat">Lista con los objetos</param> 
        ''' <param name="idPlanta">Id de la planta</param>
        Public Sub SaveConveniosCategorias(ByVal lConveniosCat As List(Of ELL.ConvenioCategoria), ByVal idPlanta As Integer)
            bidaiakDAL.SaveConveniosCategorias(lConveniosCat, idPlanta)
        End Sub

        ''' <summary>
        ''' Indica si el trabajador puede recibir un anticipo o una visa
        ''' </summary>
        ''' <param name="oUser">Objeto usuario</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function PuedeRecibirVisasAnticipos(ByVal oUser As SabLib.ELL.Usuario, ByVal idPlanta As Integer) As Boolean
            Dim epsilonBLL As New BLL.Epsilon(idPlanta)
            Dim dni As String = oUser.Dni
            If (dni = String.Empty) Then
                Dim userBLL As New SabLib.BLL.UsuariosComponent
                oUser = userBLL.GetUsuario(oUser, False)
                dni = If(oUser IsNot Nothing, oUser.Dni, String.Empty)
            End If
            If (dni <> String.Empty) Then
                Dim sInfo As String() = epsilonBLL.GetInfoPersona(dni)
                If (sInfo IsNot Nothing AndAlso sInfo(4) <> String.Empty AndAlso sInfo(5) <> String.Empty) Then
                    Dim oConvCatPlanta As ELL.ConvenioCategoria = bidaiakDAL.getConvenioCategoria(sInfo(4), sInfo(5), idPlanta)
                    Return (oConvCatPlanta IsNot Nothing AndAlso oConvCatPlanta.RecibeVisasAntic)
                End If
            End If
            Return False
        End Function

        ''' <summary>
        ''' Obtiene la liquidacion del usuario. Metalico o Factura
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>
        Public Function TipoLiquidacionUser(ByVal idUser As Integer, ByVal idPlanta As Integer) As ELL.ConvenioCategoria.TipoLiq
            Dim tipoLiq As ELL.ConvenioCategoria.TipoLiq = ELL.ConvenioCategoria.TipoLiq.Metalico
            Dim epsilonBLL As New Epsilon(idPlanta)
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim oUser As New SabLib.ELL.Usuario With {.Id = idUser}
            oUser = userBLL.GetUsuario(oUser, False)
            Dim dni As String = If(oUser IsNot Nothing, oUser.Dni, String.Empty)
            If (dni <> String.Empty) Then
                Dim sInfo As String() = epsilonBLL.GetInfoPersona(dni)
                'P.NIF,P.NOMBRE,P.APELLIDO1,P.APELLIDO2,T.ID_CONVENIO,T.ID_CATEGORIA
                If (sInfo IsNot Nothing AndAlso sInfo(4) <> String.Empty AndAlso sInfo(5) <> String.Empty) Then
                    Dim oConvCatPlanta As ELL.ConvenioCategoria = bidaiakDAL.getConvenioCategoria(sInfo(4), sInfo(5), idPlanta)
                    If (oConvCatPlanta IsNot Nothing) Then tipoLiq = oConvCatPlanta.TipoLiquidacion
                End If
            End If
            Return tipoLiq
        End Function

        ''' <summary>
        ''' Obtiene los convenios/categorias que han sido marcados para mostrarse como empresas
        ''' </summary>
        ''' <param name="idPlanta">Planta de la que se recuperaran</param>
        ''' <returns></returns>        
        Public Function getEmpresasFacturacionConvCat(ByVal idPlanta As Integer) As List(Of ELL.ConvenioCategoria)
            Dim epsilonBLL As New BLL.Epsilon(idPlanta)
            Dim lConvCatEpsilon As List(Of String()) = epsilonBLL.getConveniosCategorias()
            Dim lConvCat As List(Of ELL.ConvenioCategoria) = bidaiakDAL.getConveniosCategorias(idPlanta)
            Dim oConvCat As ELL.ConvenioCategoria
            Dim sConvCat As String()
            lConvCat = lConvCat.FindAll(Function(o As ELL.ConvenioCategoria) o.MostrarEmpresaFacturacion)
            For index As Integer = lConvCat.Count - 1 To 0 Step -1
                oConvCat = lConvCat.Item(index)
                sConvCat = lConvCatEpsilon.Find(Function(o As String()) o(0) = oConvCat.IdConvenio And o(2) = oConvCat.IdCategoria)
                If (sConvCat IsNot Nothing) Then
                    oConvCat.Convenio = sConvCat(1)
                    oConvCat.Categoria = sConvCat(3)
                    lConvCatEpsilon.Remove(sConvCat)
                Else
                    lConvCat.RemoveAt(index)
                End If
            Next
            lConvCat.Sort(Function(o1 As ELL.ConvenioCategoria, o2 As ELL.ConvenioCategoria) o1.Categoria < o2.Categoria)
            Return lConvCat
        End Function

#End Region

#Region "Saldos caja"

        ''' <summary>
        ''' Obtiene el saldo de una moneda en concreto para una planta
        ''' </summary>
        ''' <param name="idMoneda">Id de la moneda</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>
        Public Function loadSaldoCaja(ByVal idMoneda As Integer, ByVal idPlanta As Integer) As String()
            Dim lSaldos As List(Of String()) = loadSaldosCaja(idPlanta)
            Dim mySaldo As String() = Nothing
            If (lSaldos IsNot Nothing AndAlso lSaldos.Count > 0) Then mySaldo = lSaldos.Find(Function(o As String()) o(1) = idMoneda)
            Return mySaldo
        End Function

        ''' <summary>
        ''' Obtiene los saldos de las diferentes monedas de la planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        ''' <returns>0:Cantidad,1:IdMoneda,2:Nombre mon,3:Abrev mon</returns>        
        Public Function loadSaldosCaja(ByVal idPlanta As Integer) As List(Of String())
            Return bidaiakDAL.loadSaldosCaja(idPlanta)
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
        Public Function loadMovimientosCaja(ByVal idPlanta As Integer, ByVal idCurrency As Integer, ByVal fInicio As Date, ByVal fFin As Date, Optional ByVal operation As ELL.SaldoCaja.EOperacion = -1) As List(Of ELL.SaldoCaja)
            Return bidaiakDAL.loadMovimientosCaja(idPlanta, idCurrency, fInicio, fFin, operation)
        End Function

        ''' <summary>
        ''' Guarda el movimiento de saldo y calculo el saldo restante
        ''' </summary>
        ''' <param name="oSaldo">Objeto con la informacion</param>
        Public Sub saveSaldoCaja(ByVal oSaldo As ELL.SaldoCaja, Optional ByVal con As OracleConnection = Nothing)
            Dim saldoActual As String() = loadSaldoCaja(oSaldo.IdMoneda, oSaldo.IdPlanta)
            Dim saldoA As Decimal = 0
            If (saldoActual IsNot Nothing) Then saldoA = DecimalValue(saldoActual(0))
            Select Case oSaldo.Operacion
                Case ELL.SaldoCaja.EOperacion.Ingreso, ELL.SaldoCaja.EOperacion.Devolucion_Anticipo
                    oSaldo.SaldoRestante = oSaldo.Cantidad + saldoA
                Case ELL.SaldoCaja.EOperacion.Extraccion, ELL.SaldoCaja.EOperacion.Entrego_Anticipo, ELL.SaldoCaja.EOperacion.Eliminar_Devolucion
                    oSaldo.SaldoRestante = saldoA - oSaldo.Cantidad
                Case ELL.SaldoCaja.EOperacion.Actualizacion
                    oSaldo.SaldoRestante = oSaldo.Cantidad
            End Select
            bidaiakDAL.saveSaldoCaja(oSaldo, con)
        End Sub

#End Region

#Region "Aviso reimpresion HG"

        ''' <summary>
        ''' Obtiene la lista de hojas de gastos que tiene que reimprimir
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <returns></returns>
        Public Function loadListHGReimprimir(ByVal idUser As Integer) As List(Of String())
            Return bidaiakDAL.HGReimprimir(idUser)
        End Function

        ''' <summary>
        ''' Añade el aviso de la hoja de gastos
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idViaje">Id del viaje</param>
        Public Sub AddHGReimprimir(ByVal idUser As Integer, ByVal idViaje As Integer)
            DeleteHGReimprimir(idUser, idViaje)
            bidaiakDAL.AddHGReimprimir(idUser, idViaje)
        End Sub

        ''' <summary>
        ''' Elimina el aviso de la hoja de gastos
        ''' </summary>
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="idViaje">Id del viaje</param>
        Public Sub DeleteHGReimprimir(ByVal idUser As Integer, ByVal idViaje As Integer)
            bidaiakDAL.DeleteHGReimprimir(idUser, idViaje)
        End Sub

#End Region

#Region "Cuestionarios de satisfaccion"

        ''' <summary>
        ''' Comprueba si existe o no el cuestionario del viaje
        ''' </summary>
        ''' <param name="idViaje">Id del viaje</param>        
        ''' <returns></returns>        
        Public Function ExistCuestionario(ByVal idViaje As Integer) As Boolean
            Return bidaiakDAL.ExistCuestionario(idViaje)
        End Function


        ''' <summary>
        ''' Guarda los datos del cuestionario de satisfaccion
        ''' </summary>
        ''' <param name="cuest">Cuestionario</param>         
        Public Sub SaveCuestionario(ByVal cuest As Object)
            bidaiakDAL.SaveCuestionario(cuest)
        End Sub

#End Region

    End Class

End Namespace