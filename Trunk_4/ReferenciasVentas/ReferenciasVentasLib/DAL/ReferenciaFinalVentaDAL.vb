Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class ReferenciaFinalVentaDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el listado de referencias pendientes de una solicitud
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarReferencia(ByVal idRef As Integer) As ELL.ReferenciaVenta
            Dim query As String = "SELECT REFERENCIAS_VENTA.ID as ID, ID_SOLICITUD, REFERENCIA_CLIENTE, ID_PRODUCTO, ID_TYPE, ID_TRANSMISSION_MODE, ID_PROYECTO_CLIENTE, " &
                "ESPECIFICACION, REFERENCIA_BATZ, ID_REFERENCIA_TIPO, EVOLUTION_CHANGES, PREVIOUS_BATZ_NUMBER, REFERENCIAS_VENTA.COMENTARIO as COMENTARIO, NOMBRE_FINAL_BRAIN, " &
                "INSERCION_BRAIN, ID_TIPO_NUMERO, PLANO_WEB, NIVEL_INGENIERIA, REFERENCIA_DRAWING, ENVIO_EMAIL, INTEGRADO " &
                "FROM REFERENCIAS_VENTA " &
                "INNER JOIN SOLICITUDES on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " &
                "WHERE REFERENCIAS_VENTA.ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idRef, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ReferenciaVenta)(Function(r As OracleDataReader) _
            New ELL.ReferenciaVenta With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitud = CInt(r("ID_SOLICITUD")), .CustomerPartNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_CLIENTE")), .IdProduct = SabLib.BLL.Utils.integerNull(r("ID_PRODUCTO")),
                                         .IdType = SabLib.BLL.Utils.integerNull(r("ID_TYPE")), .IdTransmissionMode = SabLib.BLL.Utils.integerNull(r("ID_TRANSMISSION_MODE")), .IdCustomerProjectName = SabLib.BLL.Utils.stringNull(r("ID_PROYECTO_CLIENTE")),
                                         .Specification = SabLib.BLL.Utils.stringNull(r("ESPECIFICACION")), .BatzPartNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_BATZ")), .IdTipoReferencia = SabLib.BLL.Utils.integerNull(r("ID_REFERENCIA_TIPO")), .EvolutionChanges = SabLib.BLL.Utils.stringNull(r("EVOLUTION_CHANGES")),
                                         .PreviousBatzPartNumber = SabLib.BLL.Utils.stringNull(r("PREVIOUS_BATZ_NUMBER")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")), .FinalNameBrain = SabLib.BLL.Utils.stringNull(r("NOMBRE_FINAL_BRAIN")),
                                         .InsercionBrain = SabLib.BLL.Utils.booleanNull(r("INSERCION_BRAIN")), .TipoNumero = SabLib.BLL.Utils.integerNull(r("ID_TIPO_NUMERO")), .PlanoWeb = SabLib.BLL.Utils.stringNull(r("PLANO_WEB")),
                                         .NivelIngenieria = SabLib.BLL.Utils.stringNull(r("NIVEL_INGENIERIA")), .DrawingNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_DRAWING")), .EnvioEmail = SabLib.BLL.Utils.booleanNull(r("ENVIO_EMAIL")),
                                         .Integrado = SabLib.BLL.Utils.booleanNull(r("INTEGRADO"))}, query, CadenaConexionReferenciasVenta, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Obtiene el listado de referencias pendientes de una solicitud
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarReferenciaPorCPN(ByVal idCPN As Integer) As ELL.ReferenciaVenta
            Dim query As String = "SELECT REFERENCIAS_VENTA.ID as ID, ID_SOLICITUD, REFERENCIA_CLIENTE, ID_PRODUCTO, ID_TYPE, ID_TRANSMISSION_MODE, ID_PROYECTO_CLIENTE, " &
                "ESPECIFICACION, REFERENCIA_BATZ, ID_REFERENCIA_TIPO, EVOLUTION_CHANGES, PREVIOUS_BATZ_NUMBER, REFERENCIAS_VENTA.COMENTARIO as COMENTARIO, NOMBRE_FINAL_BRAIN, " &
                "INSERCION_BRAIN, ID_TIPO_NUMERO, PLANO_WEB, NIVEL_INGENIERIA, REFERENCIA_DRAWING, ENVIO_EMAIL, INTEGRADO " &
                "FROM REFERENCIAS_VENTA " &
                "INNER JOIN SOLICITUDES on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " &
                "WHERE REFERENCIAS_VENTA.REFERENCIA_CLIENTE=:REFERENCIA_CLIENTE"

            Dim parameter As New OracleParameter("REFERENCIA_CLIENTE", OracleDbType.Int32, idCPN, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ReferenciaVenta)(Function(r As OracleDataReader) _
            New ELL.ReferenciaVenta With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitud = CInt(r("ID_SOLICITUD")), .CustomerPartNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_CLIENTE")), .IdProduct = SabLib.BLL.Utils.integerNull(r("ID_PRODUCTO")),
                                         .IdType = SabLib.BLL.Utils.integerNull(r("ID_TYPE")), .IdTransmissionMode = SabLib.BLL.Utils.integerNull(r("ID_TRANSMISSION_MODE")), .IdCustomerProjectName = SabLib.BLL.Utils.stringNull(r("ID_PROYECTO_CLIENTE")),
                                         .Specification = SabLib.BLL.Utils.stringNull(r("ESPECIFICACION")), .BatzPartNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_BATZ")), .IdTipoReferencia = SabLib.BLL.Utils.integerNull(r("ID_REFERENCIA_TIPO")), .EvolutionChanges = SabLib.BLL.Utils.stringNull(r("EVOLUTION_CHANGES")),
                                         .PreviousBatzPartNumber = SabLib.BLL.Utils.stringNull(r("PREVIOUS_BATZ_NUMBER")), .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")), .FinalNameBrain = SabLib.BLL.Utils.stringNull(r("NOMBRE_FINAL_BRAIN")),
                                         .InsercionBrain = SabLib.BLL.Utils.booleanNull(r("INSERCION_BRAIN")), .TipoNumero = SabLib.BLL.Utils.integerNull(r("ID_TIPO_NUMERO")),
                                         .EnvioEmail = SabLib.BLL.Utils.booleanNull(r("ENVIO_EMAIL")), .PlanoWeb = SabLib.BLL.Utils.stringNull(r("PLANO_WEB")), .NivelIngenieria = SabLib.BLL.Utils.stringNull(r("NIVEL_INGENIERIA")), .DrawingNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_DRAWING")),
                                         .Integrado = SabLib.BLL.Utils.booleanNull(r("INTEGRADO"))}, query, CadenaConexionReferenciasVenta, parameter).FirstOrDefault
        End Function

        ''' <summary>
        ''' Obtiene el listado de referencias tramitadas de una solicitud
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarReferenciasCreadasSolicitud(ByVal idSolicitud As Integer) As List(Of ELL.ReferenciaVenta)
            Dim query As String = "SELECT REFERENCIAS_VENTA.ID as ID, ID_SOLICITUD, REFERENCIA_CLIENTE, ID_PRODUCTO, ID_TYPE, ID_TRANSMISSION_MODE, ID_PROYECTO_CLIENTE, " &
                "ESPECIFICACION, REFERENCIA_BATZ, ID_REFERENCIA_TIPO, EVOLUTION_CHANGES, PREVIOUS_BATZ_NUMBER, REFERENCIAS_VENTA.COMENTARIO as COMENTARIO, NOMBRE_FINAL_BRAIN, " &
                "INSERCION_BRAIN, ID_TIPO_NUMERO, PLANO_WEB, NIVEL_INGENIERIA, REFERENCIA_DRAWING, ENVIO_EMAIL, INTEGRADO " &
                "FROM REFERENCIAS_VENTA " &
                "INNER JOIN SOLICITUDES on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " &
                "WHERE ID_SOLICITUD=:ID_SOLICITUD AND REFERENCIA_BATZ IS NOT NULL" 'AND INSERCION_BRAIN IS NOT NULL 

            Dim parameter As New OracleParameter("ID_SOLICITUD", OracleDbType.Int32, idSolicitud, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ReferenciaVenta)(Function(r As OracleDataReader) _
            New ELL.ReferenciaVenta With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitud = SabLib.BLL.Utils.integerNull(r("ID_SOLICITUD")), .CustomerPartNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_CLIENTE")),
                                         .IdProduct = SabLib.BLL.Utils.integerNull(r("ID_PRODUCTO")), .IdType = SabLib.BLL.Utils.integerNull(r("ID_TYPE")), .IdTransmissionMode = SabLib.BLL.Utils.integerNull(r("ID_TRANSMISSION_MODE")),
                                         .IdCustomerProjectName = SabLib.BLL.Utils.stringNull(r("ID_PROYECTO_CLIENTE")), .Specification = SabLib.BLL.Utils.stringNull(r("ESPECIFICACION")),
                                         .BatzPartNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_BATZ")), .IdTipoReferencia = SabLib.BLL.Utils.integerNull(r("ID_REFERENCIA_TIPO")),
                                         .EvolutionChanges = SabLib.BLL.Utils.stringNull(r("EVOLUTION_CHANGES")), .PreviousBatzPartNumber = SabLib.BLL.Utils.stringNull(r("PREVIOUS_BATZ_NUMBER")),
                                         .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")), .FinalNameBrain = SabLib.BLL.Utils.stringNull(r("NOMBRE_FINAL_BRAIN")), .InsercionBrain = SabLib.BLL.Utils.booleanNull(r("INSERCION_BRAIN")),
                                         .TipoNumero = SabLib.BLL.Utils.integerNull(r("ID_TIPO_NUMERO")), .PlanoWeb = SabLib.BLL.Utils.stringNull(r("PLANO_WEB")), .NivelIngenieria = SabLib.BLL.Utils.stringNull(r("NIVEL_INGENIERIA")),
                                         .DrawingNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_DRAWING")), .EnvioEmail = SabLib.BLL.Utils.booleanNull(r("ENVIO_EMAIL")),
                                         .Integrado = SabLib.BLL.Utils.booleanNull(r("INTEGRADO"))}, query, CadenaConexionReferenciasVenta, parameter)
        End Function

        ''' <summary>
        ''' Obtiene el listado de referencias tramitadas de una solicitud
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarReferenciasSolicitud(ByVal idSolicitud As Integer) As List(Of ELL.ReferenciaVenta)
            Dim query As String = "SELECT REFERENCIAS_VENTA.ID as ID, ID_SOLICITUD, REFERENCIA_CLIENTE, ID_PRODUCTO, ID_TYPE, ID_TRANSMISSION_MODE, ID_PROYECTO_CLIENTE, " &
                "ESPECIFICACION, REFERENCIA_BATZ, ID_REFERENCIA_TIPO, EVOLUTION_CHANGES, PREVIOUS_BATZ_NUMBER, REFERENCIAS_VENTA.COMENTARIO as COMENTARIO, NOMBRE_FINAL_BRAIN, " &
                "INSERCION_BRAIN, ID_TIPO_NUMERO, PLANO_WEB, NIVEL_INGENIERIA, REFERENCIA_DRAWING,ENVIO_EMAIL, INTEGRADO " &
                "FROM REFERENCIAS_VENTA " &
                "INNER JOIN SOLICITUDES on SOLICITUDES.ID = REFERENCIAS_VENTA.ID_SOLICITUD " &
                "WHERE ID_SOLICITUD=:ID_SOLICITUD"

            Dim parameter As New OracleParameter("ID_SOLICITUD", OracleDbType.Int32, idSolicitud, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ReferenciaVenta)(Function(r As OracleDataReader) _
            New ELL.ReferenciaVenta With {.Id = SabLib.BLL.Utils.integerNull(r("ID")), .IdSolicitud = SabLib.BLL.Utils.integerNull(r("ID_SOLICITUD")), .CustomerPartNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_CLIENTE")),
                                        .IdProduct = SabLib.BLL.Utils.integerNull(r("ID_PRODUCTO")), .IdType = SabLib.BLL.Utils.integerNull(r("ID_TYPE")), .IdTransmissionMode = SabLib.BLL.Utils.integerNull(r("ID_TRANSMISSION_MODE")),
                                        .IdCustomerProjectName = SabLib.BLL.Utils.stringNull(r("ID_PROYECTO_CLIENTE")), .Specification = SabLib.BLL.Utils.stringNull(r("ESPECIFICACION")),
                                        .BatzPartNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_BATZ")), .IdTipoReferencia = SabLib.BLL.Utils.integerNull(r("ID_REFERENCIA_TIPO")),
                                        .EvolutionChanges = SabLib.BLL.Utils.stringNull(r("EVOLUTION_CHANGES")), .PreviousBatzPartNumber = SabLib.BLL.Utils.stringNull(r("PREVIOUS_BATZ_NUMBER")),
                                        .Comentario = SabLib.BLL.Utils.stringNull(r("COMENTARIO")), .FinalNameBrain = SabLib.BLL.Utils.stringNull(r("NOMBRE_FINAL_BRAIN")), .InsercionBrain = SabLib.BLL.Utils.booleanNull(r("INSERCION_BRAIN")),
                                        .TipoNumero = SabLib.BLL.Utils.integerNull(r("ID_TIPO_NUMERO")), .PlanoWeb = SabLib.BLL.Utils.stringNull(r("PLANO_WEB")), .NivelIngenieria = SabLib.BLL.Utils.stringNull(r("NIVEL_INGENIERIA")),
                                        .DrawingNumber = SabLib.BLL.Utils.stringNull(r("REFERENCIA_DRAWING")), .EnvioEmail = SabLib.BLL.Utils.booleanNull(r("ENVIO_EMAIL")),
                                        .Integrado = SabLib.BLL.Utils.booleanNull(r("INTEGRADO"))}, query, CadenaConexionReferenciasVenta, parameter)
        End Function

        ''' <summary>
        ''' Obtiene un producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarPlantasReferencia(ByVal idRef As Integer) As List(Of ELL.ReferenciaPlantas)
            Dim query As String = "SELECT ID_REFERENCIA, ID_PLANTA " & _
                "FROM REFERENCIAS_PLANTAS " & _
                "WHERE ID_REFERENCIA=:ID_REFERENCIA " & _
                "ORDER BY ID_PLANTA ASC"

            Dim parameter As New OracleParameter("ID_REFERENCIA", OracleDbType.Int32, idRef, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.ReferenciaPlantas)(Function(r As OracleDataReader) _
            New ELL.ReferenciaPlantas With {.IdReferencia = SabLib.BLL.Utils.integerNull(r("ID_REFERENCIA")), .IdPlanta = SabLib.BLL.Utils.stringNull(r("ID_PLANTA"))}, query, CadenaConexionReferenciasVenta, parameter)
        End Function
        
        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarPreviousBatzPN(ByVal texto As String) As List(Of String())
            Dim query As String = "SELECT DISTINCT REFERENCIA_BATZ FROM " & _
                "REFERENCIAS_VENTA " & _
                "INNER JOIN SOLICITUDES on REFERENCIAS_VENTA.ID_SOLICITUD = SOLICITUDES.ID " & _
                "WHERE APROBADO=1 AND REFERENCIA_BATZ IS NOT NULL AND LOWER(REFERENCIA_BATZ) LIKE '%' || :TEXTO || '%'"

            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 20, texto.ToLower, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(query, CadenaConexionReferenciasVenta, parameter).ToList()
        End Function

        ''' <summary>
        ''' Obtiene todos los usuarios
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function CargarReferenciasTramitadasSolicitud(ByVal texto As String) As List(Of String())
            Dim query As String = "SELECT DISTINCT REFERENCIA_BATZ FROM " & _
                "REFERENCIAS_VENTA " & _
                "INNER JOIN SOLICITUDES on REFERENCIAS_VENTA.ID_SOLICITUD = SOLICITUDES.ID " & _
                "WHERE APROBADO=1 AND REFERENCIA_BATZ IS NOT NULL AND LOWER(REFERENCIA_BATZ) LIKE '%' || :TEXTO || '%'"

            Dim parameter As New OracleParameter("TEXTO", OracleDbType.Varchar2, 20, texto.ToLower, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.Seleccionar(query, CadenaConexionReferenciasVenta, parameter).ToList()
        End Function

        ''' <summary>
        ''' Verificar que una referencia de Batz existe en la base de datos (no necesariamente tiene que haber sido integrado en Brain)
        ''' </summary>
        ''' <param name="referenciaBatz">Referencia de venta en batz</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteReferenciaBatz(ByVal referenciaBatz As String) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM " &
                                  "REFERENCIAS_VENTA " &
                                  "WHERE REFERENCIA_BATZ=:REFERENCIA_BATZ"

            Dim parameter As New OracleParameter("REFERENCIA_BATZ", OracleDbType.NVarchar2, 13, referenciaBatz.ToUpper, ParameterDirection.Input)

            Return If(Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexionReferenciasVenta, parameter) > 0, True, False)
        End Function

        ''' <summary>
        ''' Cargar la última referencia de Batz generada para el grupo de producto seleccionado
        ''' </summary>
        ''' <param name="grupo">Grupo de producto</param>
        ''' <returns></returns>
        Public Function CargarUltimaReferenciaBatzProducto(ByVal grupo As String) As String
            'Dim query As String = "SELECT MAX(REFERENCIA_BATZ)  " &
            '                      "FROM REFERENCIAS_PRODUCTOS " &
            '                      "WHERE PRODUCTO=:PRODUCTO"

            'Dim parameter As New OracleParameter("PRODUCTO", OracleDbType.NVarchar2, 10, grupo, ParameterDirection.Input)

            'Return Memcached.OracleDirectAccess.SeleccionarEscalar(Of String)(query, CadenaConexionReferenciasVenta, parameter)
            Try


                Dim lista As List(Of String())
                Dim query As String = "SELECT REFERENCIA_BATZ  " &
                                      "FROM REFERENCIAS_PRODUCTOS " &
                                      "WHERE PRODUCTO=:PRODUCTO " &
                                      "ORDER BY FECHA DESC"

                Dim parameter As New OracleParameter("PRODUCTO", OracleDbType.NVarchar2, 10, grupo, ParameterDirection.Input)

                lista = Memcached.OracleDirectAccess.Seleccionar(query, CadenaConexionReferenciasVenta, parameter)
                If (lista.Count > 0) Then
                    Return lista.FirstOrDefault().First()
                Else
                    Return String.Empty
                End If
            Catch ex As Exception
                log.Error("Error al obtener la última referencia disponible", ex)
                Return String.Empty
            End Try
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guardar un nuevo registro
        ''' </summary>
        ''' <param name="articulos">Lista de objetos ReferenciaVenta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarSolicitud(ByRef articulos As List(Of ELL.ReferenciaVenta), ByVal perfilUsuario As ELL.PerfilUsuario, ByVal idTipoSolicitud As Integer, ByVal idValidador As Integer) As Integer
            'Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters1 As New List(Of OracleParameter)
            Dim idSolicitud As Integer = 0
            Dim idReferencia As Integer = 0

            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexionReferenciasVenta)
                con.Open()
                transact = con.BeginTransaction()

                query = "INSERT INTO SOLICITUDES(ID_SOLICITANTE, ID_TIPO_SOLICITUD, VALIDADO, ID_VALIDADOR) " &
                    "VALUES(:ID_SOLICITANTE, :ID_TIPO_SOLICITUD, :VALIDADO, :ID_VALIDADOR) returning ID into :RETURN_VALUE"
                lParameters1.Add(New OracleParameter("ID_SOLICITANTE", OracleDbType.Int32, perfilUsuario.IdUsuario, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_TIPO_SOLICITUD", OracleDbType.Int32, idTipoSolicitud, ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("VALIDADO", OracleDbType.Int32, If(perfilUsuario.IdRol = ELL.Roles.RolUsuario.ProductEngineer, ELL.Solicitudes.ValidacionSolicitudes.Pendiente, ELL.Solicitudes.ValidacionSolicitudes.SinNecesidad), ParameterDirection.Input))
                lParameters1.Add(New OracleParameter("ID_VALIDADOR", OracleDbType.Int32, If(idValidador = Integer.MinValue, DBNull.Value, idValidador), ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters1.Add(p)

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters1.ToArray)

                idSolicitud = lParameters1.Item(4).Value

                If ((idSolicitud <> Integer.MinValue) AndAlso (idSolicitud > 0)) Then
                    For Each articulo In articulos
                        query = "INSERT INTO REFERENCIAS_VENTA(ID_SOLICITUD, REFERENCIA_CLIENTE, ID_REFERENCIA_TIPO, EVOLUTION_CHANGES, PREVIOUS_BATZ_NUMBER, " &
                                "ID_PRODUCTO, ID_TYPE, ID_TRANSMISSION_MODE, ID_PROYECTO_CLIENTE, ESPECIFICACION, COMENTARIO, NOMBRE_FINAL_BRAIN, ID_TIPO_NUMERO, " &
                                "PLANO_WEB, NIVEL_INGENIERIA, REFERENCIA_DRAWING) " &
                                "VALUES(:ID_SOLICITUD, :REFERENCIA_CLIENTE, :ID_REFERENCIA_TIPO, :EVOLUTION_CHANGES, :PREVIOUS_BATZ_NUMBER, " &
                                ":ID_PRODUCTO, :ID_TYPE, :ID_TRANSMISSION_MODE, :ID_PROYECTO_CLIENTE, :ESPECIFICACION, :COMENTARIO, :NOMBRE_FINAL_BRAIN, :ID_TIPO_NUMERO, " &
                                ":PLANO_WEB, :NIVEL_INGENIERIA, :REFERENCIA_DRAWING) " &
                                "returning ID into :RETURN_VALUE"
                        Dim lParameters2 As New List(Of OracleParameter)
                        lParameters2.Add(New OracleParameter("ID_SOLICITUD", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("REFERENCIA_CLIENTE", OracleDbType.NVarchar2, 19, articulo.CustomerPartNumber, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ID_REFERENCIA_TIPO", OracleDbType.Int16, 1, articulo.IdTipoReferencia, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("EVOLUTION_CHANGES", OracleDbType.NVarchar2, 100, articulo.EvolutionChanges, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("PREVIOUS_BATZ_NUMBER", OracleDbType.NVarchar2, 13, articulo.PreviousBatzPartNumber, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ID_PRODUCTO", OracleDbType.Varchar2, 1, articulo.IdProduct, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ID_TYPE", OracleDbType.Int16, 1, If(articulo.IdType = Integer.MinValue, DBNull.Value, articulo.IdType), ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ID_TRANSMISSION_MODE", OracleDbType.Int16, 1, If(articulo.IdTransmissionMode = Integer.MinValue, DBNull.Value, articulo.IdTransmissionMode), ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ID_PROYECTO_CLIENTE", OracleDbType.NVarchar2, 30, articulo.IdCustomerProjectName, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ESPECIFICACION", OracleDbType.NVarchar2, 50, articulo.Specification, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("COMENTARIO", OracleDbType.NVarchar2, 100, articulo.Comentario, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("NOMBRE_FINAL_BRAIN", OracleDbType.NVarchar2, 52, articulo.FinalNameBrain, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("ID_TIPO_NUMERO", OracleDbType.Int16, 1, articulo.TipoNumero, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("PLANO_WEB", OracleDbType.NVarchar2, 10, articulo.PlanoWeb, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("NIVEL_INGENIERIA", OracleDbType.NVarchar2, 19, articulo.NivelIngenieria, ParameterDirection.Input))
                        lParameters2.Add(New OracleParameter("REFERENCIA_DRAWING", OracleDbType.NVarchar2, 19, articulo.DrawingNumber, ParameterDirection.Input))

                        Dim p2 As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                        p2.DbType = DbType.Int32
                        lParameters2.Add(p2)

                        Memcached.OracleDirectAccess.NoQuery(query, con, lParameters2.ToArray)

                        idReferencia = lParameters2.Item(16).Value

                        If ((idReferencia <> Integer.MinValue) AndAlso (idReferencia > 0)) Then
                            articulo.Id = idReferencia
                            For Each planta In articulo.PlantsToCharge
                                query = "INSERT INTO REFERENCIAS_PLANTAS(ID_REFERENCIA, ID_PLANTA)" &
                                        "VALUES(:ID_REFERENCIA, :ID_PLANTA)"
                                Dim lParameters3 As New List(Of OracleParameter)
                                lParameters3.Add(New OracleParameter("ID_REFERENCIA", OracleDbType.Int32, idReferencia, ParameterDirection.Input))
                                lParameters3.Add(New OracleParameter("ID_PLANTA", OracleDbType.NVarchar2, 2, planta, ParameterDirection.Input))

                                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters3.ToArray)
                            Next
                        End If
                    Next

                    transact.Commit()

                    'resultado = True
                Else
                    transact.Rollback()
                    'resultado = False
                End If
            Catch ex As Exception
                log.Error(ex.ToString)
                transact.Rollback()
                idSolicitud = 0
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return idSolicitud
            'Return resultado
        End Function

        ''' <summary>
        ''' Modifica el Batz Part Number de todas las referencias de una solicitud
        ''' </summary>
        ''' <param name="valores">Listado con el identificador de la referencia y el valor de Batz Part Number</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarBatzPartNumber(ByVal idSolicitud As Integer, ByVal valores As Dictionary(Of Integer, String), ByVal aprobado As Boolean, ByVal idTramitador As Integer, ByVal comentario As String) As Boolean
            Dim resultado As Boolean = False
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexionReferenciasVenta)
                con.Open()
                transact = con.BeginTransaction()

                query = "UPDATE SOLICITUDES SET FECHA_GESTION=:FECHA_GESTION, USUARIO_TRAMITADOR=:USUARIO_TRAMITADOR, APROBADO=:APROBADO, COMENTARIO=:COMENTARIO WHERE ID=:ID"
                lParameters.Add(New OracleParameter("FECHA_GESTION", OracleDbType.Date, System.DateTime.Now, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("USUARIO_TRAMITADOR", OracleDbType.Int32, idTramitador, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("APROBADO", OracleDbType.Int16, aprobado, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("COMENTARIO", OracleDbType.Varchar2, 100, comentario, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                For Each valor In valores
                    Dim lParameters1 As New List(Of OracleParameter)

                    query = "UPDATE REFERENCIAS_VENTA SET REFERENCIA_BATZ=:REFERENCIA_BATZ WHERE ID=:ID"
                    lParameters1.Add(New OracleParameter("REFERENCIA_BATZ", OracleDbType.NVarchar2, 20, valor.Value, ParameterDirection.Input))
                    lParameters1.Add(New OracleParameter("ID", OracleDbType.Int32, valor.Key, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters1.ToArray)
                Next

                transact.Commit()

                resultado = True
            Catch ex As Exception
                log.Error(ex.ToString)
                transact.Rollback()
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Modifica el Batz Part Number de todas las referencias de una solicitud
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarTramitacion(ByVal idSolicitud As Integer, ByVal aprobado As Boolean, ByVal idTramitador As Integer, ByVal comentarioDT As String) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)

            Try
                query = "UPDATE SOLICITUDES SET FECHA_GESTION=:FECHA_GESTION, USUARIO_TRAMITADOR=:USUARIO_TRAMITADOR, APROBADO=:APROBADO, COMENTARIO_DT=:COMENTARIO_DT WHERE ID=:ID"
                lParameters.Add(New OracleParameter("FECHA_GESTION", OracleDbType.Date, System.DateTime.Now, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("USUARIO_TRAMITADOR", OracleDbType.Int32, idTramitador, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("APROBADO", OracleDbType.Int16, aprobado, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("COMENTARIO_DT", OracleDbType.Varchar2, 100, comentarioDT, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idSolicitud, ParameterDirection.Input))

                Memcached.OracleDirectAccess.NoQuery(query, CadenaConexionReferenciasVenta, lParameters.ToArray)

                resultado = True
            Catch ex As Exception
                log.Error(ex.ToString)
                resultado = False
            End Try
            Return resultado
        End Function

        ''' <summary>
        ''' Modifica el flag de inserción en Brain
        ''' </summary>
        ''' <param name="idReferencia">Identificador de la referencia</param>
        ''' <param name="insercionBrain">Flag de inserción o eliminación</param>
        ''' <param name="BatzPartNumber">Referencia de Pieza de Batz</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function InsercionBrainReferenciaVenta(ByVal idReferencia As Integer, ByVal insercionBrain As Boolean, ByVal batzPartNumber As String, ByVal envioEmail As Boolean, ByVal tipoInsercion As Integer, Optional ByVal producto As String = "", Optional ByVal integrado As Boolean = False) As Boolean
            Dim resultado As Boolean = False
            Dim query As String = String.Empty
            Dim lParameters As New List(Of OracleParameter)
            Dim lParameters2 As New List(Of OracleParameter)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing

            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexionReferenciasVenta)
                con.Open()
                transact = con.BeginTransaction()

                query = "UPDATE REFERENCIAS_VENTA SET INSERCION_BRAIN=:INSERCION_BRAIN, REFERENCIA_BATZ=:REFERENCIA_BATZ, ENVIO_EMAIL=:ENVIO_EMAIL, INTEGRADO=:INTEGRADO WHERE ID=:ID"

                Select Case tipoInsercion
                    Case 0
                        lParameters.Add(New OracleParameter("INSERCION_BRAIN", OracleDbType.Int16, 1, 0, ParameterDirection.Input))
                    Case 1
                        lParameters.Add(New OracleParameter("INSERCION_BRAIN", OracleDbType.Int16, 1, 1, ParameterDirection.Input))
                    Case 2
                        lParameters.Add(New OracleParameter("INSERCION_BRAIN", OracleDbType.Int16, 1, DBNull.Value, ParameterDirection.Input))
                End Select

                lParameters.Add(New OracleParameter("INTEGRADO", OracleDbType.Int16, 1, If(integrado, 1, 0), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ENVIO_EMAIL", OracleDbType.Int16, 1, envioEmail, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("REFERENCIA_BATZ", OracleDbType.NVarchar2, 13, If(Not String.IsNullOrEmpty(batzPartNumber), batzPartNumber, DBNull.Value), ParameterDirection.Input))
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idReferencia, ParameterDirection.Input))
                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                If Not (String.IsNullOrEmpty(producto)) Then
                    query = "DELETE FROM REFERENCIAS_PRODUCTOS WHERE REFERENCIA_BATZ=:REFERENCIA_BATZ"
                    Dim parameter As New OracleParameter("REFERENCIA_BATZ", OracleDbType.NVarchar2, 13, batzPartNumber, ParameterDirection.Input)
                    Memcached.OracleDirectAccess.NoQuery(query, con, parameter)

                    query = "INSERT INTO REFERENCIAS_PRODUCTOS (REFERENCIA_BATZ, PRODUCTO) VALUES (:REFERENCIA_BATZ,:PRODUCTO)"
                    lParameters2.Add(New OracleParameter("REFERENCIA_BATZ", OracleDbType.NVarchar2, 13, batzPartNumber, ParameterDirection.Input))
                    lParameters2.Add(New OracleParameter("PRODUCTO", OracleDbType.NVarchar2, 10, producto, ParameterDirection.Input))
                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters2.ToArray)
                End If

                transact.Commit()

                resultado = True
            Catch ex As Exception
                log.Error(ex.ToString)
                resultado = False
                transact.Rollback()
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
            Return resultado
        End Function

#End Region

    End Class

End Namespace