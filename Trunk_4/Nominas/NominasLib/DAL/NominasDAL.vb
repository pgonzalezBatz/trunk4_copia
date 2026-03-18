Imports Oracle.ManagedDataAccess.Client

Public Class NominasDAL

#Region "Properties"

    Private Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.Nominas")

    ''' <summary>
    ''' Devuelve la cadena de conexion dependiendo de si esta en TEST o en LIVE
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Shared ReadOnly Property GetStringConnection() As String
        Get
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then
                Return Configuration.ConfigurationManager.ConnectionStrings("GESTIONHORASLIVE").ConnectionString
            Else
                Return Configuration.ConfigurationManager.ConnectionStrings("GESTIONHORASTEST").ConnectionString
            End If
        End Get
    End Property

    ''' <summary>
    ''' Devuelve la cadena de conexion de epsilon dependiendo de si esta en TEST o en LIVE
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Shared ReadOnly Property GetEpsilonStringConnection() As String
        Get
            Return Configuration.ConfigurationManager.ConnectionStrings("EPSILON").ConnectionString
        End Get
    End Property

#End Region

#Region "Nominas"

    ''' <summary>
    ''' Se descarga la nomina
    ''' </summary>    
    ''' <param name="idNomina">Id de la nomina</param>    
    ''' <returns></returns>    
    Public Shared Function DownloadNomina(ByVal idNomina As Integer) As Byte()
        Try
            Dim query As String = "SELECT NOMINA FROM NOMINAS WHERE ID=:ID"
            Dim params As New List(Of OracleParameter)
            params.Add(New OracleParameter("ID", OracleDbType.Int32, idNomina, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of Byte())(query, GetStringConnection(), params.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error obteniendo las nominas del trabajador", ex)
        End Try

    End Function

    ''' <summary>
    ''' Obtiene las nominas de una persona en un mes
    ''' </summary>    
    ''' <param name="NumTra">Nº de trabajador</param>
    ''' <param name="idEmpresaEpsilon">Id de la empresa de Epsilon</param>
    ''' <param name="año">Año</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarNominas(ByVal numTra As Integer, ByVal idEmpresaEpsilon As String, ByVal año As Integer) As List(Of String())
        Try
            Dim query As String = "SELECT PAGA,PARTE_PAGA,ID FROM NOMINAS WHERE NUM_TRA=:NUM_TRA AND ID_EMPRESA=:ID_EMPRESA AND ANO=:ANO ORDER BY PAGA,PARTE_PAGA"
            Dim params As New List(Of OracleParameter)
            params.Add(New OracleParameter("NUM_TRA", OracleDbType.Int32, numTra, ParameterDirection.Input))
            params.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            params.Add(New OracleParameter("ANO", OracleDbType.Int32, año, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.Seleccionar(query, GetStringConnection, params.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error obteniendo las nominas del trabajador", ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene las nominas de una persona en un mes
    ''' </summary>    
    ''' <param name="desde">Fecha de inicio</param>
    ''' <param name="hasta">Fecha de fin</param>
    ''' <param name="users">Codigos de trabajador de los usuarios a consultar</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarNominasIgorre(ByVal desde As String, ByVal hasta As String, ByVal users As String) As List(Of String())
        Try
            Dim query As String = "SELECT N.NUM_TRA, U.NOMBRE || ' ' || U.APELLIDO1 || ' ' || U.APELLIDO2,N.ANO,N.PAGA,N.PARTE_PAGA,N.ID,U.DNI 
                                    FROM NOMINAS N
                                    INNER JOIN SAB.USUARIOS U ON U.CODPERSONA = N.NUM_TRA AND U.IDPLANTA = 1 
                                    WHERE N.NUM_TRA IN (" & String.Join(",", users.Split(";")) & ") 
                                    AND ID_EMPRESA=1 
                                    AND ANO BETWEEN :ANOMIN AND :ANOMAX
                                    ORDER BY N.NUM_TRA,ANO DESC,PAGA DESC,PARTE_PAGA DESC"
            Dim params As New List(Of OracleParameter)
            params.Add(New OracleParameter("ANOMIN", OracleDbType.Int32, CInt(desde.Substring(0, 4)), ParameterDirection.Input))
            params.Add(New OracleParameter("ANOMAX", OracleDbType.Int32, CInt(hasta.Substring(0, 4)), ParameterDirection.Input))
            Return Memcached.OracleDirectAccess.Seleccionar(query, GetStringConnection, params.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error obteniendo las nominas del trabajador", ex)
        End Try
    End Function

#End Region

#Region "Documentos 10T"

    ''' <summary>
    ''' Actualiza el estado del proceso de docs 10T. Si tiene informado la fecha de fin, habra acabado, sino empezara
    ''' </summary>    
    ''' <param name="anno">Año</param>
    ''' <param name="idEmpresaEpsilon">Id de la empresa de Epsilon</param>
    ''' <param name="numRegOk">Numero de registros procesados ok</param>
    ''' <param name="numRegError">Numero de registros procesados con error</param>  
    Public Shared Sub EstadoProceso10T(ByVal anno As Integer, ByVal idEmpresaEpsilon As String, ByVal simular As Boolean, ByVal numRegOk As Integer, ByVal numRegError As Integer)
        Try
            Dim query As String = "SELECT FFIN FROM DOC_10T_EJEC WHERE ANO=:ANO AND ID_EMPRESA=:ID_EMPRESA AND SIMULACION=:SIMULACION"
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ANO", OracleDbType.Int32, anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("SIMULACION", OracleDbType.Int32, If(simular, 1, 0), ParameterDirection.Input))
            Dim sInfo As String() = Memcached.OracleDirectAccess.Seleccionar(query, GetStringConnection(), lParametros.ToArray).FirstOrDefault
            If (sInfo Is Nothing OrElse (sInfo IsNot Nothing AndAlso CDate(sInfo(0)) <> Date.MinValue)) Then
                If (sInfo IsNot Nothing) Then
                    query = "DELETE FROM DOC_10T_EJEC WHERE ANO=:ANO AND ID_EMPRESA=:ID_EMPRESA AND SIMULACION=:SIMULACION"
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ANO", OracleDbType.Int32, anno, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("SIMULACION", OracleDbType.Int32, If(simular, 1, 0), ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, lParametros.ToArray)
                End If
                lParametros = New List(Of OracleParameter)
                query = "INSERT INTO DOC_10T_EJEC(ANO,FINICIO,FFIN,NUMREG_OK,NUMREG_ERROR,ID_EMPRESA,SIMULACION) VALUES (:ANO,SYSDATE,NULL,0,0,:ID_EMPRESA,:SIMULACION)"
            Else
                lParametros = New List(Of OracleParameter)
                query = "UPDATE DOC_10T_EJEC SET FFIN=SYSDATE,NUMREG_OK=:NUMREG_OK,NUMREG_ERROR=:NUMREG_ERROR WHERE ANO=:ANO AND ID_EMPRESA=:ID_EMPRESA AND SIMULACION=:SIMULACION"
                lParametros.Add(New OracleParameter("NUMREG_OK", OracleDbType.Int32, numRegOk, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NUMREG_ERROR", OracleDbType.Int32, numRegError, ParameterDirection.Input))
            End If
            lParametros.Add(New OracleParameter("ANO", OracleDbType.Int32, anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("SIMULACION", OracleDbType.Int32, If(simular, 1, 0), ParameterDirection.Input))
            Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, lParametros.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al cambiar el estado del proceso 10T", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Comprueba si se esta ejecutando
    ''' </summary>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function estaEjecutandoseProceso10T(ByVal idEmpresaEpsilon As String) As Boolean
        Try
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT COUNT(*) FROM DOC_10T_EJEC WHERE FFIN IS NULL AND ID_EMPRESA=:ID_EMPRESA"
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            Return (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, GetStringConnection, lParametros.ToArray) > 0)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al comprobar si el proceso se esta ejecutando", ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene todas las ejecuciones del proceso
    ''' </summary>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarEjecucionesProceso10T(ByVal idEmpresaEpsilon As String) As List(Of String())
        Try
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT * FROM DOC_10T_EJEC WHERE ID_EMPRESA=:ID_EMPRESA"
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            Return Memcached.OracleDirectAccess.Seleccionar(query, GetStringConnection, lParametros.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener el listado de ejecuciones", ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene todos los documentos del año indicado
    ''' </summary>
    ''' <param name="ano">Año</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarDocumentos10T(ano As Integer, ByVal idEmpresaEpsilon As String, simulacion As Integer) As List(Of Nomina.Proceso10TTemp)
        Try


            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT ID,DNI,ID_SAB,ANO,ESTADO,MENSAJE,ID_EMPRESA FROM DOC_10T WHERE ANO=:ANO AND ID_EMPRESA=:ID_EMPRESA AND SIMULACION=:SIMULACION"
            lParametros.Add(New OracleParameter("ANO", OracleDbType.Int32, ano, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("SIMULACION", OracleDbType.Int32, simulacion, ParameterDirection.Input))
            Return Memcached.OracleDirectAccess.seleccionar(Of Nomina.Proceso10TTemp)(Function(o As OracleDataReader) _
                            New Nomina.Proceso10TTemp With {.id = CInt(o("ID")), .ano = ano, .dni = o("DNI"), .idSab = SabLib.BLL.Utils.integerNull(o("ID_SAB")), .state = CInt(o("ESTADO")), .mensaje = SabLib.BLL.Utils.stringNull(o("MENSAJE")), .idEmpresa = o("ID_EMPRESA")}, query, GetStringConnection, lParametros.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener el listado de documentos 10T del año " & ano, ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene los datos de un documento
    ''' </summary>
    ''' <param name="doc">Documento</param>    
    ''' <returns></returns>    
    Public Shared Function GetDocumento10T(ByVal doc As Nomina.Proceso10TTemp, ByVal simular As Boolean, Optional ByVal con As OracleConnection = Nothing) As Nomina.Proceso10TTemp
        Try
            Dim query As String = "SELECT ID,DNI,ID_SAB,ANO,ESTADO,MENSAJE,ID_EMPRESA FROM DOC_10T WHERE "
            Dim where As String = String.Empty
            Dim params As New List(Of OracleParameter)
            If (doc.idEmpresa <> String.Empty) Then
                where = "ID_EMPRESA=:ID_EMPRESA"
                params.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, doc.idEmpresa, ParameterDirection.Input))
            End If
            If (doc.ano > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "ANO=:ANO"
                params.Add(New OracleParameter("ANO", OracleDbType.Int32, doc.ano, ParameterDirection.Input))
            End If
            If (doc.dni <> String.Empty) Then
                where &= If(where <> String.Empty, " AND ", "") & "DNI=:DNI"
                params.Add(New OracleParameter("DNI", OracleDbType.Varchar2, doc.dni, ParameterDirection.Input))
            End If
            where &= If(where <> String.Empty, " AND ", "") & "SIMULACION=:SIMULAR"
            params.Add(New OracleParameter("SIMULAR", OracleDbType.Int32, If(simular, 1, 0), ParameterDirection.Input))
            query &= where
            If (con Is Nothing) Then
                Return Memcached.OracleDirectAccess.seleccionar(Of Nomina.Proceso10TTemp)(Function(o As OracleDataReader) _
                                New Nomina.Proceso10TTemp With {.id = CInt(o("ID")), .ano = CInt(o("ANO")), .dni = o("DNI"), .idSab = SabLib.BLL.Utils.integerNull(o("ID_SAB")), .state = CInt(o("ESTADO")),
                                                                .mensaje = SabLib.BLL.Utils.stringNull(o("MENSAJE")), .idEmpresa = o("ID_EMPRESA")}, query, GetStringConnection, params.ToArray).FirstOrDefault
            Else
                Return Memcached.OracleDirectAccess.seleccionar(Of Nomina.Proceso10TTemp)(Function(o As OracleDataReader) _
                                New Nomina.Proceso10TTemp With {.id = CInt(o("ID")), .ano = CInt(o("ANO")), .dni = o("DNI"), .idSab = SabLib.BLL.Utils.integerNull(o("ID_SAB")), .state = CInt(o("ESTADO")),
                                                                .mensaje = SabLib.BLL.Utils.stringNull(o("MENSAJE")), .idEmpresa = o("ID_EMPRESA")}, query, con, params.ToArray).FirstOrDefault
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos del documento 10 T", ex)
        End Try
    End Function

    ''' <summary>
    ''' Se insertan los registros la tabla 10T    
    ''' </summary>    
    ''' <param name="oDoc">Documento</param>    
    Public Shared Sub Insert10T(ByVal oDoc As Nomina.Proceso10TTemp, ByVal simular As Boolean)
        Dim query As String = String.Empty
        Dim params As New List(Of OracleParameter)
        Try
            params = New List(Of OracleParameter)
            If (oDoc.id <= 0) Then
                query = "INSERT INTO DOC_10T(DNI,ID_SAB,ANO,ESTADO,MENSAJE,ID_EMPRESA,SIMULACION) VALUES (:DNI,:ID_SAB,:ANO,:ESTADO,:MENSAJE,:ID_EMPRESA,:SIMULAR)"
                params.Add(New OracleParameter("DNI", OracleDbType.Varchar2, oDoc.dni, ParameterDirection.Input))
                params.Add(New OracleParameter("ANO", OracleDbType.Int32, oDoc.ano, ParameterDirection.Input))
                params.Add(New OracleParameter("ID_EMPRESA", OracleDbType.Varchar2, oDoc.idEmpresa, ParameterDirection.Input))
                params.Add(New OracleParameter("SIMULAR", OracleDbType.Int32, If(simular, 1, 0), ParameterDirection.Input))
            Else
                query = "UPDATE DOC_10T SET ID_SAB=:ID_SAB,ESTADO=:ESTADO,MENSAJE=:MENSAJE WHERE ID=:ID"
                params.Add(New OracleParameter("ID", OracleDbType.Int32, oDoc.id, ParameterDirection.Input))
            End If
            params.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oDoc.idSab), ParameterDirection.Input))
            params.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oDoc.state, ParameterDirection.Input))
            params.Add(New OracleParameter("MENSAJE", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oDoc.mensaje), ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, params.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error en la funcion de Insertar10T", ex)
        End Try
    End Sub

#End Region

#Region "Documento de Intereses"

    ''' <summary>
    ''' Actualiza el estado del proceso de docs de intereses. Si tiene informado la fecha de fin, habra acabado, sino empezara
    ''' </summary>
    ''' <param name="anno">Año</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <param name="numRegOk">Numero de registros procesados ok</param>
    ''' <param name="numRegError">Numero de registros procesados con error</param>   
    Public Shared Sub EstadoProcesoIntereses(ByVal anno As Integer, ByVal idEmpresaEpsilon As String, ByVal numRegOk As Integer, ByVal numRegError As Integer)
        Try
            Dim query As String = "SELECT FFIN FROM DOC_INTERES_EJEC WHERE ANO=:ANO AND ID_EMPRESA=:ID_EMPRESA"
            Dim lParametros As New List(Of OracleParameter)
            lParametros.Add(New OracleParameter("ANO", OracleDbType.Int32, anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            Dim sInfo As String() = Memcached.OracleDirectAccess.Seleccionar(query, GetStringConnection(), lParametros.ToArray).FirstOrDefault
            If (sInfo Is Nothing OrElse (sInfo IsNot Nothing AndAlso CDate(sInfo(0)) <> Date.MinValue)) Then
                If (sInfo IsNot Nothing) Then
                    query = "DELETE FROM DOC_INTERES_EJEC WHERE ANO=:ANO AND ID_EMPRESA=:ID_EMPRESA"
                    lParametros = New List(Of OracleParameter)
                    lParametros.Add(New OracleParameter("ANO", OracleDbType.Int32, anno, ParameterDirection.Input))
                    lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, lParametros.ToArray)
                End If
                lParametros = New List(Of OracleParameter)
                query = "INSERT INTO DOC_INTERES_EJEC(ANO,FINICIO,FFIN,NUMREG_OK,NUMREG_ERROR,ID_EMPRESA) VALUES (:ANO,SYSDATE,NULL,0,0,:ID_EMPRESA)"
            Else
                lParametros = New List(Of OracleParameter)
                query = "UPDATE DOC_INTERES_EJEC SET FFIN=SYSDATE,NUMREG_OK=:NUMREG_OK,NUMREG_ERROR=:NUMREG_ERROR WHERE ANO=:ANO AND ID_EMPRESA=:ID_EMPRESA"
                lParametros.Add(New OracleParameter("NUMREG_OK", OracleDbType.Int32, numRegOk, ParameterDirection.Input))
                lParametros.Add(New OracleParameter("NUMREG_ERROR", OracleDbType.Int32, numRegError, ParameterDirection.Input))
            End If
            lParametros.Add(New OracleParameter("ANO", OracleDbType.Int32, anno, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, lParametros.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al cambiar el estado del proceso de intereses", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Comprueba si se esta ejecutando
    ''' </summary>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function estaEjecutandoseProcesoIntereses(ByVal idEmpresaEpsilon As String) As Boolean
        Try
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT COUNT(*) FROM DOC_INTERES_EJEC WHERE FFIN IS NULL AND ID_EMPRESA=:ID_EMPRESA"
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            Return (Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, GetStringConnection, lParametros.ToArray) > 0)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al comprobar si el proceso se esta ejecutando", ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene todas las ejecuciones del proceso
    ''' </summary>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarEjecucionesProcesoIntereses(ByVal idEmpresaEpsilon As String) As List(Of String())
        Try
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT * FROM DOC_INTERES_EJEC WHERE ID_EMPRESA=:ID_EMPRESA"
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            Return Memcached.OracleDirectAccess.Seleccionar(query, GetStringConnection, lParametros.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener el listado de ejecuciones", ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene todos los documentos del año indicado
    ''' </summary>
    ''' <param name="ano">Año</param>
    ''' <param name="idEmpresaEpsilon">Id de Epsilon</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarDocumentosIntereses(ByVal ano As Integer, ByVal idEmpresaEpsilon As String) As List(Of Nomina.ProcesoInteresesTemp)
        Try
            Dim lParametros As New List(Of OracleParameter)
            Dim query As String = "SELECT ID,DNI,ID_SAB,ANO,ESTADO,MENSAJE,ID_EMPRESA FROM DOC_INTERES WHERE ANO=:ANO AND ID_EMPRESA=:ID_EMPRESA"
            lParametros.Add(New OracleParameter("ANO", OracleDbType.Int32, ano, ParameterDirection.Input))
            lParametros.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, idEmpresaEpsilon, ParameterDirection.Input))
            Return Memcached.OracleDirectAccess.seleccionar(Of Nomina.ProcesoInteresesTemp)(Function(o As OracleDataReader) _
                            New Nomina.ProcesoInteresesTemp With {.id = CInt(o("ID")), .ano = ano, .dni = o("DNI"), .idSab = SabLib.BLL.Utils.integerNull(o("ID_SAB")), .state = CInt(o("ESTADO")), .mensaje = SabLib.BLL.Utils.stringNull(o("MENSAJE")), .idEmpresa = o("ID_EMPRESA")}, query, GetStringConnection, lParametros.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener el listado de documentos de intereses del año " & ano, ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene los datos de un documento
    ''' </summary>
    ''' <param name="doc">Documento</param>    
    ''' <returns></returns>    
    Public Shared Function GetDocumentoIntereses(ByVal doc As Nomina.ProcesoInteresesTemp, Optional ByVal con As OracleConnection = Nothing) As Nomina.ProcesoInteresesTemp
        Try
            Dim query As String = "SELECT ID,DNI,ID_SAB,ANO,ESTADO,MENSAJE,ID_EMPRESA FROM DOC_INTERES WHERE "
            Dim where As String = String.Empty
            Dim params As New List(Of OracleParameter)
            If (doc.idEmpresa <> String.Empty) Then
                where = "ID_EMPRESA=:ID_EMPRESA"
                params.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, doc.idEmpresa, ParameterDirection.Input))
            End If
            If (doc.ano > 0) Then
                where &= If(where <> String.Empty, " AND ", "") & "ANO=:ANO"
                params.Add(New OracleParameter("ANO", OracleDbType.Int32, doc.ano, ParameterDirection.Input))
            End If
            If (doc.dni <> String.Empty) Then
                where &= If(where <> String.Empty, " AND ", "") & "DNI=:DNI"
                params.Add(New OracleParameter("DNI", OracleDbType.Varchar2, doc.dni, ParameterDirection.Input))
            End If
            query &= where
            If (con Is Nothing) Then
                Return Memcached.OracleDirectAccess.seleccionar(Of Nomina.ProcesoInteresesTemp)(Function(o As OracleDataReader) _
                                New Nomina.ProcesoInteresesTemp With {.id = CInt(o("ID")), .ano = CInt(o("ANO")), .dni = o("DNI"), .idSab = SabLib.BLL.Utils.integerNull(o("ID_SAB")), .state = CInt(o("ESTADO")),
                                                                .mensaje = SabLib.BLL.Utils.stringNull(o("MENSAJE")), .idEmpresa = o("ID_EMPRESA")}, query, GetStringConnection, params.ToArray).FirstOrDefault
            Else
                Return Memcached.OracleDirectAccess.seleccionar(Of Nomina.ProcesoInteresesTemp)(Function(o As OracleDataReader) _
                                New Nomina.ProcesoInteresesTemp With {.id = CInt(o("ID")), .ano = CInt(o("ANO")), .dni = o("DNI"), .idSab = SabLib.BLL.Utils.integerNull(o("ID_SAB")), .state = CInt(o("ESTADO")),
                                                                .mensaje = SabLib.BLL.Utils.stringNull(o("MENSAJE")), .idEmpresa = o("ID_EMPRESA")}, query, con, params.ToArray).FirstOrDefault
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos del documento de intereses", ex)
        End Try
    End Function

    ''' <summary>
    ''' Se insertan los registros la tabla de intereses    
    ''' </summary>    
    ''' <param name="oDoc">Documento</param>    
    Public Shared Sub InsertIntereses(ByVal oDoc As Nomina.ProcesoInteresesTemp)
        Dim query As String = String.Empty
        Dim params As New List(Of OracleParameter)
        Try
            params = New List(Of OracleParameter)
            If (oDoc.id <= 0) Then
                query = "INSERT INTO DOC_INTERES(DNI,ID_SAB,ANO,ESTADO,MENSAJE,ID_EMPRESA) VALUES (:DNI,:ID_SAB,:ANO,:ESTADO,:MENSAJE,:ID_EMPRESA)"
                params.Add(New OracleParameter("DNI", OracleDbType.Varchar2, oDoc.dni, ParameterDirection.Input))
                params.Add(New OracleParameter("ANO", OracleDbType.Int32, oDoc.ano, ParameterDirection.Input))
                params.Add(New OracleParameter("ID_EMPRESA", OracleDbType.NVarchar2, oDoc.idEmpresa, ParameterDirection.Input))
            Else
                query = "UPDATE DOC_INTERES SET ID_SAB=:ID_SAB,ESTADO=:ESTADO,MENSAJE=:MENSAJE WHERE ID=:ID"
                params.Add(New OracleParameter("ID", OracleDbType.Int32, oDoc.id, ParameterDirection.Input))
            End If
            params.Add(New OracleParameter("ID_SAB", OracleDbType.Int32, SabLib.BLL.Utils.OracleIntegerDBNull(oDoc.idSab), ParameterDirection.Input))
            params.Add(New OracleParameter("ESTADO", OracleDbType.Int32, oDoc.state, ParameterDirection.Input))
            params.Add(New OracleParameter("MENSAJE", OracleDbType.Varchar2, SabLib.BLL.Utils.OracleStringDBNull(oDoc.mensaje), ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, params.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error en la funcion de InsertIntereses", ex)
        End Try
    End Sub

#End Region

#Region "Plantas nomina"

    ''' <summary>
    ''' Obtiene la informacion de la planta de la nomina
    ''' </summary>
    ''' <param name="idPlantaEpsilon">Id de la planta</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarPlantaNomina(ByVal idPlantaEpsilon As Integer) As String()
        Try
            Dim query As String = "SELECT PN.ID_EPSILON,PN.RUTA,P.NOMBRE FROM PLANTASNOMINA PN INNER JOIN SAB.PLANTAS P ON PN.ID_EPSILON=P.ID_EPSILON WHERE PN.ID_EPSILON=:ID_PLANTA"
            Return Memcached.OracleDirectAccess.Seleccionar(query, GetStringConnection, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlantaEpsilon, ParameterDirection.Input)).FirstOrDefault
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al consultar la planta de la nomina", ex)
        End Try
    End Function

    ''' <summary>
    ''' Obtiene las plantas de las nominas
    ''' </summary>    
    ''' <returns></returns>    
    Public Shared Function ConsultarPlantasNominas() As List(Of String())
        Try
            Dim query As String = "SELECT PN.ID_EPSILON,PN.RUTA,P.NOMBRE FROM PLANTASNOMINA PN INNER JOIN SAB.PLANTAS P ON PN.ID_EPSILON=P.ID_EPSILON"
            Return Memcached.OracleDirectAccess.Seleccionar(query, GetStringConnection)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al consultar las plantas de nominas", ex)
        End Try
    End Function

    ''' <summary>
    ''' Guarda los datos de la planta
    ''' </summary>
    ''' <param name="sInfo">Informacion de la ruta</param>
    ''' <param name="bNew">Indica si es nuevo o una modificacion</param>
    Public Shared Sub SavePlantaNomina(ByVal sInfo As String(), ByVal bNew As Boolean)
        Dim query As String = String.Empty
        Dim params As New List(Of OracleParameter)
        Try
            params = New List(Of OracleParameter)
            If (bNew) Then
                query = "INSERT INTO PLANTASNOMINA(ID_EPSILON,RUTA) VALUES (:ID_EPSILON,:RUTA)"
            Else
                query = "UPDATE PLANTASNOMINA SET RUTA=:RUTA WHERE ID_EPSILON=:ID_EPSILON"
            End If
            params.Add(New OracleParameter("ID_EPSILON", OracleDbType.Int32, CInt(sInfo(0)), ParameterDirection.Input))
            params.Add(New OracleParameter("RUTA", OracleDbType.NVarchar2, sInfo(1), ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, params.ToArray)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de la planta de la nomina", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Elimina la planta de la nomina
    ''' </summary>        
    Public Shared Sub DeletePlantaNomina(ByVal idPlanta As Integer)
        Try
            Dim query As String = "DELETE FROM PLANTASNOMINA WHERE ID_EPSILON=:ID_PLANTA"
            Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al borrar la planta de la nomina", ex)
        End Try
    End Sub

#End Region

#Region "Roles"

    ''' <summary>
    ''' Consulta los datos del rol del usuario
    ''' </summary>    
    ''' <param name="idUser">Id del usuario</param>
    ''' <param name="idPlanta">Id de la planta</param>
    ''' <returns></returns>    
    Public Shared Function ConsultarRol(ByVal idUser As Integer, ByVal idPlanta As Integer) As Integer()
        Try
            Dim query As String = "SELECT ID_USER,ID_PLANTA,ROL FROM ROLES_NOMINAS WHERE ID_USER=:ID_USER AND ID_PLANTA=:ID_PLANTA"
            Dim params As New List(Of OracleParameter)
            params.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
            params.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of Integer())(Function(o As OracleDataReader) _
                            New Integer() {CInt(o("ID_USER")), CInt(o("ID_PLANTA")), CInt(o("ROL"))}, query, GetStringConnection, params.ToArray).FirstOrDefault
        Catch ex As Exception
            Throw New SabLib.BatzException("Error obteniendo el rol del trabajador", ex)
        End Try
    End Function

    ''' <summary>
    ''' Guarda el rol
    ''' </summary>
    ''' <param name="iRol">Array con los datos del rol. 0:idUser,1:idPlanta,2:Rol</param>    
    Public Shared Sub SaveRol(ByVal iRol As Integer())
        Dim query As String = String.Empty
        Dim params As New List(Of OracleParameter)
        params.Add(New OracleParameter("ID_USER", OracleDbType.Int32, iRol(0), ParameterDirection.Input))
        params.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, iRol(1), ParameterDirection.Input))
        params.Add(New OracleParameter("ROL", OracleDbType.Int32, iRol(2), ParameterDirection.Input))
        Dim iRolAux As Integer() = ConsultarRol(iRol(0), iRol(1))
        If (iRolAux Is Nothing) Then
            query = "INSERT INTO ROLES_NOMINAS(ID_USER,ID_PLANTA,ROL) VALUES (:ID_USER,:ID_PLANTA,:ROL)"
        Else
            query = "UPDATE ROLES_NOMINAS SET ROL=:ROL WHERE ID_USER=:ID_USER AND ID_PLANTA=:ID_PLANTA"
        End If
        Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, params.ToArray)
    End Sub

    ''' <summary>
    ''' Elimina el rol
    ''' </summary>
    ''' <param name="idUser">Id del usuario</param>
    ''' <param name="idPlanta">Id de la planta</param>
    Public Shared Sub DeleteRol(ByVal idUser As Integer, ByVal idPlanta As Integer)
        Dim query As String = "DELETE FROM ROLES_NOMINAS WHERE ID_USER=:ID_USER AND ID_PLANTA=:ID_PLANTA"
        Dim params As New List(Of OracleParameter)
        params.Add(New OracleParameter("ID_USER", OracleDbType.Int32, idUser, ParameterDirection.Input))
        params.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, GetStringConnection, params.ToArray)
    End Sub

#End Region

#Region "Epsilon"

    ''' <summary>
    ''' Obtiene la descripcion del concepto paga
    ''' </summary>
    ''' <param name="numPaga">Numero de la paga</param>
    ''' <returns></returns>    
    Public Shared Function getPaga(ByVal numPaga As Integer) As String
        Dim query As String = "SELECT D_MES_ABR FROM MES_PAGAS WHERE MES=@MES"
        Dim param As New SqlClient.SqlParameter("MES", SqlDbType.Int, ParameterDirection.Input)
        param.Value = numPaga
        Return Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(query, GetEpsilonStringConnection, param)
    End Function

    ''' <summary>
    ''' Obtiene la descripcion del concepto parte paga
    ''' </summary>
    ''' <param name="parteP">Parte de la paga</param>
    ''' <returns></returns>    
    Public Shared Function getPartePaga(ByVal parteP As Integer) As String
        Dim query As String = "SELECT D_NOMINA FROM TIP_NOMINAS WHERE ID_NOMINA=@PARTE_PAGA"
        Dim param As New SqlClient.SqlParameter("PARTE_PAGA", SqlDbType.Int, ParameterDirection.Input)
        param.Value = parteP
        Return Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(query, GetEpsilonStringConnection, param)
    End Function

#End Region

End Class
