Imports Oracle.DataAccess
Imports System.DirectoryServices
Imports log4net

Module Exportar

    Private log As ILog = LogManager.GetLogger("root.EXTENSION_LDAP")
    Private Property IdPlanta As Integer
    Private Property PathLDAP As String
    Private Property NumInicialDirecto As String
    Private Property NumInicialMovil As String

#Region "USUARIO"

    ''' <summary>
    ''' Estructura usuario
    ''' </summary>
    Public Class Usuario
        Public idSab As Integer
        Public userName As String
        Public nombre As String
        Public apellido1 As String
        Public apellido2 As String
        Public email As String
        Public extensionInterna As String
        Public directo As String
        Public movil As String
        Public planta As Integer
    End Class

#End Region

#Region "Enum BusquedaLDAP"

    Public Enum BusquedaLDAP As Integer
        nombreUsuario = 0
        email = 1
    End Enum

#End Region

#Region "MAIN"

    ''' <summary>
    ''' Script para guardar en el directorio activo la extension interna de las personas de sab
    ''' </summary>
    Sub Main()
        Try
            inicializarLog4Net()
            log.Info("Comienza ejecucion de la exportacion de extensiones al directorio activo")
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim lPlantas As List(Of SabLib.ELL.Planta) = plantBLL.GetPlantas(True)
            For Each planta As SabLib.ELL.Planta In lPlantas
                log.Info("PLANTA:" & planta.Nombre)
                log.Info("************************")
                If (ExportarExtensiones(planta)) Then
                    log.Info("Extensiones exportadas correctamente")
                Else
                    log.Error("No se ha completado correctamente la exportacion")
                End If
                log.Info("************************")
            Next
            Select Case IdPlanta
                Case 1 'Igorre
                    NumInicialDirecto = "1"
                    NumInicialMovil = "5"

                Case 2 'MbTooling
                    NumInicialDirecto = "3"
                    NumInicialMovil = "5"

                Case 3 'MbtRioja
                    NumInicialDirecto = String.Empty  '???No se porque no hay ninguno metido
                    NumInicialMovil = String.Empty    '???No se porque no hay ninguno metido

                Case 4 'Kunshan
                    NumInicialDirecto = "4"
                    NumInicialMovil = String.Empty   '???No se porque no hay ninguno metido

                Case 5 'Mexicana
                    NumInicialDirecto = "4"
                    NumInicialMovil = String.Empty   '???No se porque no hay ninguno metido

                Case 6 'Czech
                    NumInicialDirecto = "4"
                    NumInicialMovil = String.Empty  '???No se porque no hay ninguno metido

                Case 7 'Mus
                    NumInicialDirecto = "3"
                    NumInicialMovil = "5"

            End Select
            'If (ExportarExtensiones()) Then
            '    Console.WriteLine("Extensiones exportadas correctamente")
            '    log.Info("Extensiones exportadas correctamente")
            'Else
            '    Console.WriteLine("Se ha producido un error al exportar")
            '    log.Error("No se ha completado correctamente la exportacion")
            'End If
        Catch ex As Exception
            log.Error("Ha ocurrido un error en el proceso de exportacion de extensiones de usuarios", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Inicializa el log4net
    ''' </summary>
    Private Sub InicializarLog4Net()
        log4net.Config.XmlConfigurator.Configure(New IO.FileInfo(System.Configuration.ConfigurationManager.AppSettings.Get("log4netFile")))
    End Sub

#End Region

#Region "EXPORTAR EXTENSION"

    ''' <summary>
    ''' Exporta la extension interna al LDAP de los usuarios
    ''' </summary>
    ''' <param name="planta">Planta con su informacion</param>
    Private Function ExportarExtensiones(ByVal planta As SabLib.ELL.Planta) As Boolean
        Dim lUsuarios As List(Of Usuario) = GetUsuariosConEmail(planta)
        If (lUsuarios Is Nothing) Then
            log.Error("No se ha obtenido ningun usuario de la vista")
            Return False
        Else
            Return UpdateExtension(lUsuarios)
        End If
    End Function

    ''' <summary>
    ''' Busca en la vista W_TARJETAS_VISITA los usuarios de sab con email que no sean los de @batz.coop
    ''' </summary>
    ''' <param name="planta">Objeto planta</param>
    ''' <returns></returns>    
    Private Function GetUsuariosConEmail(ByVal planta As SabLib.ELL.Planta) As List(Of Usuario)
        Dim cn As New Client.OracleConnection()
        Dim oReader As Client.OracleDataReader = Nothing
        Try
            Dim cmd As New Client.OracleCommand()
            Dim userComp As New SabLib.BLL.UsuariosComponent
            Dim plantas As List(Of SabLib.ELL.Planta)
            Dim lUsuarios As New List(Of Usuario)
            Dim oUsuario As Usuario
            Dim idUser As Integer
            cn.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings("TELEFONIA").ConnectionString
            cmd.Connection = cn
            Dim Sql As New System.Text.StringBuilder
            Sql.AppendLine("SELECT * FROM( ")
            Sql.AppendLine("SELECT DISTINCT U.ID,U.NOMBRE,U.APELLIDO1,U.APELLIDO2,U.EMAIL,ExI.EXTENSION,TI.NUMERO AS DIRECTO,TM.NUMERO AS MOVIL,U.NOMBREUSUARIO,U.FECHABAJA ")
            Sql.AppendLine("FROM SAB.USUARIOS U INNER JOIN SAB.USUARIOS_PLANTAS UP ON (U.ID=UP.ID_USUARIO AND UP.ID_PLANTA=:IDPLANTA) ")
            Sql.AppendLine("INNER join extension_personas ep on ep.id_usuario=u.id ")
            Sql.AppendLine("INNER join extension exI on (exI.id=ep.id_extension AND EXI.ID_EXT_INTERNA IS NULL AND SUBSTR(EXI.EXTENSION,0,1)='" & NumInicialDirecto & "') ")
            Sql.AppendLine("left join extension exM on exM.id_ext_interna=exI.id ")
            Sql.AppendLine("left join telefono tI on exI.id_telefono=tI.id ")
            Sql.AppendLine("left join telefono tM on exM.id_telefono=tM.id ")
            Sql.AppendLine("WHERE(UP.ID_PLANTA = :IDPLANTA And U.CODPERSONA Is Not NULL) ")
            Sql.AppendLine("AND (U.FECHABAJA IS NULL OR (U.FECHABAJA IS NOT NULL AND U.FECHABAJA>SYSDATE)) ")
            Sql.AppendLine("AND ep.f_hasta is null and EXI.ID_PLANTA=:IDPLANTA ")
            Sql.AppendLine("AND 1=(SELECT count(U1.ID) FROM SAB.USUARIOS U1 WHERE lower(trim(U1.NOMBRE))=lower(trim(U.NOMBRE)) AND lower(trim(U1.APELLIDO1))=lower(trim(U.APELLIDO1)) AND lower(trim(nvl(U1.APELLIDO2,'-')))=lower(trim(nvl(U.APELLIDO2,'-'))) and (u1.fechabaja is null or (u1.fechabaja is not null and u1.fechabaja>sysdate))) ")
            Sql.AppendLine("AND 1=(SELECT count(UP1.ID_USUARIO) FROM SAB.USUARIOS_PLANTAS UP1 WHERE UP1.ID_USUARIO=U.ID) ")
            Sql.AppendLine("UNION ")
            Sql.AppendLine("SELECT DISTINCT U.ID,U.NOMBRE,U.APELLIDO1,U.APELLIDO2,U.EMAIL,NULL as EXTENSION,NULL AS DIRECTO,TM.NUMERO AS MOVIL,U.NOMBREUSUARIO,U.FECHABAJA ")
            Sql.AppendLine("FROM SAB.USUARIOS U INNER JOIN SAB.USUARIOS_PLANTAS UP ON (U.ID=UP.ID_USUARIO AND UP.ID_PLANTA=:IDPLANTA) ")
            Sql.AppendLine("INNER join extension_personas ep on ep.id_usuario=u.id ")
            Sql.AppendLine("INNER join extension exM on (exm.id=ep.id_extension AND EXm.ID_EXT_INTERNA IS NULL AND SUBSTR(EXm.EXTENSION,0,1)='" & NumInicialMovil & "') ")
            Sql.AppendLine("left join telefono tM on exM.id_telefono=tM.id ")
            Sql.AppendLine("WHERE(UP.ID_PLANTA = :IDPLANTA And U.CODPERSONA Is Not NULL) ")
            Sql.AppendLine("AND (U.FECHABAJA IS NULL OR (U.FECHABAJA IS NOT NULL AND U.FECHABAJA>SYSDATE)) ")
            Sql.AppendLine("AND ep.f_hasta is null and EXm.ID_PLANTA=:IDPLANTA ")
            Sql.AppendLine("AND 1=(SELECT count(U1.ID) FROM SAB.USUARIOS U1 WHERE lower(trim(U1.NOMBRE))=lower(trim(U.NOMBRE)) AND lower(trim(U1.APELLIDO1))=lower(trim(U.APELLIDO1)) AND lower(trim(nvl(U1.APELLIDO2,'-')))=lower(trim(nvl(U.APELLIDO2,'-'))) and (u1.fechabaja is null or (u1.fechabaja is not null and u1.fechabaja>sysdate))) ")
            Sql.AppendLine("AND 1=(SELECT count(UP1.ID_USUARIO) FROM SAB.USUARIOS_PLANTAS UP1 WHERE UP1.ID_USUARIO=U.ID) ")
            Sql.AppendLine("UNION ")
            Sql.AppendLine("SELECT DISTINCT U.ID,U.NOMBRE,U.APELLIDO1,U.APELLIDO2,U.EMAIL,NULL as EXTENSION,NULL AS DIRECTO,NULL AS MOVIL,U.NOMBREUSUARIO,U.FECHABAJA ")
            Sql.AppendLine("FROM SAB.USUARIOS U INNER JOIN SAB.USUARIOS_PLANTAS UP ON (U.ID=UP.ID_USUARIO AND UP.ID_PLANTA=:IDPLANTA) ")
            Sql.AppendLine("WHERE(UP.ID_PLANTA = :IDPLANTA And U.CODPERSONA Is Not NULL) ")
            Sql.AppendLine("AND (U.FECHABAJA IS NULL OR (U.FECHABAJA IS NOT NULL AND U.FECHABAJA>SYSDATE)) ")
            Sql.AppendLine("AND U.ID NOT IN ")
            Sql.AppendLine("(SELECT EXP.ID_USUARIO ")
            Sql.AppendLine("FROM EXTENSION_PERSONAS EXP INNER JOIN EXTENSION E ON EXP.ID_EXTENSION=E.ID ")
            Sql.AppendLine("WHERE EXP.ID_USUARIO=U.ID AND EXP.F_HASTA IS NULL AND E.ID_PLANTA=:IDPLANTA) ")
            Sql.AppendLine("AND 1=(SELECT count(U1.ID) FROM SAB.USUARIOS U1 WHERE lower(trim(U1.NOMBRE))=lower(trim(U.NOMBRE)) AND lower(trim(U1.APELLIDO1))=lower(trim(U.APELLIDO1)) AND lower(trim(nvl(U1.APELLIDO2,'-')))=lower(trim(nvl(U.APELLIDO2,'-'))) and (u1.fechabaja is null or (u1.fechabaja is not null and u1.fechabaja>sysdate))) ")
            Sql.AppendLine("AND 1=(SELECT count(UP1.ID_USUARIO) FROM SAB.USUARIOS_PLANTAS UP1 WHERE UP1.ID_USUARIO=U.ID) ")
            Sql.AppendLine(") ")
            Sql.AppendLine("WHERE (EMAIL IS NOT NULL AND (FECHABAJA IS NULL OR (FECHABAJA IS NOT NULL AND FECHABAJA>sysdate)) AND INSTR(EMAIL,'batz.coop')=0 AND ")
            If (IdPlanta = 1) Then
                Sql.AppendLine(" INSTR(EMAIL,'batz.es')>0 ) ")
            ElseIf (IdPlanta = 2) Then
                Sql.AppendLine(" INSTR(EMAIL,'mbtooling.com')>0 ) ")
            ElseIf (IdPlanta = 3) Then
                Sql.AppendLine(" INSTR(EMAIL,'mbtrioja.com')>0 ) ")
            ElseIf (IdPlanta = 4) Then
                Sql.AppendLine(" INSTR(EMAIL,'batzkunshan.com')>0 ) ")
            ElseIf (IdPlanta = 5) Then
                Sql.AppendLine(" INSTR(EMAIL,'batzmexicana.com')>0 ) ")
            ElseIf (IdPlanta = 6) Then
                Sql.AppendLine(" INSTR(EMAIL,'batzczech.cz')>0 ) ")
            ElseIf (IdPlanta = 7) Then
                Sql.AppendLine(" INSTR(EMAIL,'mondragon-us.com')>0 ) ")
            End If

            cmd.CommandText = Sql.ToString '"SELECT IDSAB,NOMBRE,APELLIDO1,APELLIDO2,EMAIL,EXTENSION,NOMBREUSUARIO,DIRECTO,NUMERO AS MOVIL FROM W_TARJETAS_VISITA WHERE EMAIL IS NOT NULL AND EXTENSION IS NOT NULL AND INSTR(EMAIL,'batz.coop')=0 AND INSTR(EMAIL,'batz.es')>0 ORDER BY NOMBRE,APELLIDO1,APELLIDO2"

            cmd.Parameters.Add("IDPLANTA", Oracle.DataAccess.Client.OracleDbType.Int32, ParameterDirection.Input)
            cmd.Parameters("IDPLANTA").Value = IdPlanta

            cn.Open()
            oReader = cmd.ExecuteReader()
            While oReader.Read
                idUser = CInt(oReader("ID"))
                oUsuario = lUsuarios.Find(Function(o As Usuario) o.idSab = idUser)
                If (oUsuario Is Nothing) Then 'no lo ha encontrado
                    oUsuario = New Usuario With {.idSab = idUser}
                    With oUsuario
                        .nombre = oReader("NOMBRE")
                        .apellido1 = oReader("APELLIDO1")
                        If (Not oReader.IsDBNull(3)) Then .apellido2 = oReader("APELLIDO2")
                        If (Not oReader.IsDBNull(4)) Then .email = oReader("EMAIL")
                        If (Not oReader.IsDBNull(5)) Then .extensionInterna = oReader("EXTENSION")
                        If (Not oReader.IsDBNull(6)) Then .directo = oReader("DIRECTO")
                        If (Not oReader.IsDBNull(7)) Then .movil = oReader("MOVIL")
                        If (Not oReader.IsDBNull(8)) Then .userName = oReader("NOMBREUSUARIO")
                        plantas = userComp.GetPlantas(oUsuario.idSab)
                        If (plantas IsNot Nothing) Then
                            oUsuario.planta = plantas.Item(0).Id
                        End If
                    End With
                    lUsuarios.Add(oUsuario)
                Else  'lo ha encontrado
                    If (oUsuario.extensionInterna <> String.Empty And Not oReader.IsDBNull(5)) Then oUsuario.extensionInterna &= " / "
                    If (Not oReader.IsDBNull(5)) Then oUsuario.extensionInterna &= oReader("EXTENSION")

                    If (oUsuario.directo <> String.Empty And Not oReader.IsDBNull(6)) Then oUsuario.directo &= " / "
                    If (Not oReader.IsDBNull(6)) Then oUsuario.directo &= oReader("DIRECTO")

                    If (oUsuario.movil <> String.Empty And Not oReader.IsDBNull(7)) Then oUsuario.movil &= " / "
                    If (Not oReader.IsDBNull(7)) Then oUsuario.movil &= oReader("MOVIL")
                End If
            End While
            Return lUsuarios
        Catch ex As Exception
            log.Error("Error al obtener los usuarios de la vista TELEFONIA.W_TARJETAS_VISITA", ex)
            Return Nothing
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Function

    ''' <summary>
    ''' Actualiza el campoExtension de los usuarios de la lista que se encuentren en el LDAP
    ''' </summary>
    ''' <param name="lUsuarios">Lista de usuarios</param>
    ''' <returns></returns>
    Private Function UpdateExtension(ByVal lUsuarios As List(Of Usuario))
        Dim lUsuariosProcesados As New List(Of Usuario)
        Dim bActualizado As Boolean = True
        Try
            log.Info("Se van a actualizar " & lUsuarios.Count & " registros")
            For Each oUser As Usuario In lUsuarios
                bActualizado = BuscarActualizarUsuarioLDAP(oUser, BusquedaLDAP.email)
                If (bActualizado) Then
                    lUsuariosProcesados.Add(oUser)
                Else
                    Exit For
                End If
            Next
            GenerarFichero(lUsuariosProcesados)
            Return bActualizado
        Catch ex As Exception
            log.Error("Error al actualizar la extension", ex)
            Return False
        End Try
    End Function


#End Region

#Region "LDAP"

    ''' <summary>
    ''' Busca alusuario en el directorio activo.Se podra buscar por nombre de usuario o por email
    ''' </summary>
    ''' <param name="oUsuario">Objeto user con el filtro a aplicar</param>
    ''' <param name="tipoBusqueda">Indicara si se buscara por email o por nombre de usuario</param>
    Public Function BuscarActualizarUsuarioLDAP(ByVal oUsuario As Usuario, ByVal tipoBusqueda As BusquedaLDAP) As Boolean
        Dim filtro As String
        Dim search As New DirectorySearcher()
        Dim entry As New DirectoryEntry()
        Dim resul As SearchResult
        Dim LDAPUserName As String = Configuration.ConfigurationManager.AppSettings("UserElkarekin")  '"TelefoniaBatz@elkarekin.com"
        Dim LDAPPassword As String = Configuration.ConfigurationManager.AppSettings("PasswordElkarekin")  '"batztlf2009"

        'Filtramos el usuario del que queremos obtener los datos por numero de cuenta
        'If (tipoBusqueda = BusquedaLDAP.nombreUsuario) Then
        '    filtro = "(sAMAccountName=" & oUsuario.userName & ")"
        '    strPath = Configuration.ConfigurationManager.AppSettings("pathLDAP_Itxina")
        'Else
        filtro = "(mail=" & oUsuario.email & ")"
        'End If

        Try
            'strPath = "OU=grupo de trabajo,OU=Sistemas de Informacion y Comunicacion,OU=Servicios Generales,OU=Departamentos,OU=Batz,DC=elkarekin,DC=com"
            entry = New DirectoryEntry(PathLDAP, LDAPUserName, LDAPPassword) With {
                .AuthenticationType = AuthenticationTypes.Secure
            }

            'Realizamos una busqueda sobre la entrada anteriormente seleccionada.
            search = New DirectorySearcher(entry) With {
                .Filter = filtro
            }

            'Y realizamos una busqueda de todos sus datos.                
            resul = search.FindOne()
        Catch ex As Exception
            log.Error("Error al buscar en LDAP")
            Throw New Exception("Error al buscar en LDAP")
        End Try

        Try
            If (resul IsNot Nothing) Then
                Dim dirEntry As DirectoryEntry = resul.GetDirectoryEntry

                'Numero directo
                Dim numDirecto As String = String.Empty
                If (oUsuario.directo IsNot Nothing) Then numDirecto = oUsuario.directo
                numDirecto = FormatearFijo(numDirecto, oUsuario.planta)
                If (dirEntry.Properties("telephoneNumber").Count > 0) Then
                    dirEntry.Properties("telephoneNumber").Clear()
                End If
                If (numDirecto <> String.Empty) Then dirEntry.Properties("telephoneNumber").Add(numDirecto)

                'Extension interna
                Dim extInterna As String = String.Empty
                If (oUsuario.extensionInterna IsNot Nothing) Then extInterna = oUsuario.extensionInterna
                If (dirEntry.Properties("pager").Count > 0) Then
                    dirEntry.Properties("pager").Clear()
                End If
                If (extInterna <> String.Empty) Then dirEntry.Properties("pager").Add(extInterna)

                'Numero movil
                Dim numMovil As String = String.Empty
                If (oUsuario.movil IsNot Nothing) Then numMovil = oUsuario.movil
                numMovil = FormatearMovil(numMovil, oUsuario.planta)
                If (dirEntry.Properties("mobile").Count > 0) Then
                    dirEntry.Properties("mobile").Clear()
                End If
                If (numMovil <> String.Empty) Then dirEntry.Properties("mobile").Add(numMovil)

                'Numero de la planta. De momento solo para Igorre
                Dim ipPhone As String = GetTlfnoPlanta(oUsuario.planta)
                If (dirEntry.Properties("ipphone").Count > 0) Then
                    dirEntry.Properties("ipphone").Clear()
                End If
                dirEntry.Properties("ipphone").Add(ipPhone)

                'Fax
                Dim fax As String = getFax(oUsuario.planta)
                If (dirEntry.Properties("facsimileTelephoneNumber").Count > 0) Then
                    dirEntry.Properties("facsimileTelephoneNumber").Clear()
                End If
                dirEntry.Properties("facsimileTelephoneNumber").Add(fax)

                dirEntry.CommitChanges()
                dirEntry.Close()
            End If
        Catch ex As Exception
            log.Error("Error al intentar actualizar LDAP del usuario " & oUsuario.userName & " /idSab=" & oUsuario.idSab)
            Throw New Exception("Error al intentar actualizar LDAP")
        Finally
            If (entry IsNot Nothing) Then entry.Close()
        End Try
        Return True

    End Function

    ''' <summary>
    ''' Obtiene el telefono de una planta
    ''' </summary>
    ''' <returns></returns>    
    Private Function GetTlfnoPlanta(ByVal idPlanta As Integer) As String
        Dim tlfno As String = String.Empty
        Select Case idPlanta
            'Case 1  'Igorre
            'tlfno = "+34 94 630 50 00"
            'Case 2  'MbTooling
            'tlfno = "+34 94 630 99 00"
            'Case 3  'MbtRijoa
            '   tlfno = "+ 34 941 48 64 48"
            ' Case 4  'Kunshan
            'tlfno = "+86 - 0512 - 551 552 30"
            'Case 5  'Mexicana
            '   tlfno = "(52) 444 499 93 00"
            'Case 6  'Chequia
            '    tlfno = "+420 558 603 011"
            'Case 7  'MUS
            'tlfno = "+34 94 630 48 55"
        End Select
        Return tlfno
    End Function

    ''' <summary>
    ''' Obtiene el fax dada una planta
    ''' </summary>
    ''' <returns></returns>    
    Private Function GetFax(ByVal idPlanta As Integer) As String
        Dim fax As String = String.Empty
        Select Case idPlanta
            'Case 1  'Igorre
            'fax = "+34 94 630 50 20"
            'Case 2  'MbTooling
            ' fax = "+ 34 94 673 47 18"
            'Case 3  'MbtRijoa
            ' fax = "+ 34 941 48 64 53"
            'Case 4  'Kunshan
            'fax = "+86 - 0512 - 551 552 31"
            'Case 5  'Mexicana
            '   fax = "(52) 444 799 72 41"
            'Case 6  'Chequia
            '    fax = "+420 558 640 168"
            'Case 7  'MUS
            'fax = "+34 94 630 48 16"
        End Select
        Return fax
    End Function

    ''' <summary>
    ''' Formatea un numero fijo anteponiendole el prefijo y con espacios entre grupos de numeros
    ''' De momento, solo se realiza para Igorre
    ''' </summary>
    ''' <param name="numero"></param>    
    ''' <returns></returns>    
    Private Function FormatearFijo(ByVal numero As String, ByVal idPlanta As Integer)
        Dim fijo As String = String.Empty
        If ((idPlanta = 1 Or idPlanta = 2 Or idPlanta = 3 Or idPlanta = 7) And numero.Trim <> String.Empty) Then
            fijo = numero.Substring(0, 2) & " " & numero.Substring(2, 3) & " " & numero.Substring(5, 2) & " " & numero.Substring(7, 2)
            fijo = "+34 " & fijo
        Else
            fijo = numero
        End If
        Return fijo
    End Function

    ''' <summary>
    ''' Formatea un numero fijo anteponiendole el prefijo y con espacios entre grupos de numeros
    ''' De momento, solo se realiza para Igorre
    ''' </summary>
    ''' <param name="numero"></param>    
    ''' <returns></returns>    
    Private Function FormatearMovil(ByVal numero As String, ByVal idPlanta As Integer)
        Dim movil As String = String.Empty
        If ((idPlanta = 1 Or idPlanta = 2 Or idPlanta = 3 Or idPlanta = 7) And numero.Trim <> String.Empty) Then
            movil = numero.Substring(0, 3) & " " & numero.Substring(3, 2) & " " & numero.Substring(5, 2) & " " & numero.Substring(7, 2)
            movil = "+34 " & movil
        Else
            movil = numero
        End If
        Return movil
    End Function

#End Region

#Region "GENERAR FICHERO"

    ''' <summary>
    ''' Generar un fichero en un directorio dado con la lista de usuarios de Sab que no hayan cumplido unas condiciones
    ''' </summary>
    ''' <param name="lista">Lista de id's de SAB</param>    
    Public Sub GenerarFichero(ByVal lista As List(Of Usuario))
        Try
            Dim fileName As String = "Usuarios_Extensiones.txt"
            Dim texto As String = "LISTADO DE USUARIOS - EXTENSIONES PROCESADOS "

            texto &= "( " & Now.Date.ToShortDateString & "-" & Now.Date.ToShortTimeString & " )" & vbCrLf & vbCrLf

            For Each oUser As Usuario In lista
                texto &= "IdSab: " & oUser.idSab & " / Email: " & oUser.email & " / Extension Int: " & oUser.extensionInterna & " / Directo: " & oUser.directo & " / Movil: " & oUser.movil & vbCrLf
            Next

            WriteFile(fileName, texto)
        Catch ex As Exception
        End Try
    End Sub

    ''' <summary>
    ''' Si no existe un fichero en el dia de hoy, lo crea y sino lo añade
    ''' </summary>
    ''' <param name="nombreFichero">Nombre del fichero</param>
    ''' <param name="texto">Texto a escribir</param>
    Public Sub WriteFile(ByVal nombreFichero As String, ByVal texto As String)
        Try
            Dim fich As String = Configuration.ConfigurationManager.AppSettings("directorioFicheros") & "\" & nombreFichero
            Dim sw As New System.IO.StreamWriter(fich, True)
            sw.WriteLine(texto)
            sw.Close()
        Catch
        End Try
    End Sub

#End Region

End Module
