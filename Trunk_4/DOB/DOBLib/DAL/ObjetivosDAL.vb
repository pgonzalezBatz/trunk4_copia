Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class ObjetivosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un objetivo
        ''' </summary>
        ''' <param name="idObjetivo">Id del objetivo</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getObjetivo(ByVal idObjetivo As Integer) As ELL.Objetivo
            Dim query As String = "SELECT * FROM VOBJETIVOS WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idObjetivo, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Objetivo)(Function(r As OracleDataReader) _
            New ELL.Objetivo With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdReto = CInt(r("ID_RETO")), .IdProceso = CInt(r("ID_PROCESO")),
                                   .IdResponsable = CInt(r("ID_RESPONSABLE")), .FechaObjetivo = CDate(r("FECHA_OBJETIVO")), .NombreIndicador = SabLib.BLL.Utils.stringNull(r("NOMBRE_INDICADOR")),
                                   .DescripcionIndicador = SabLib.BLL.Utils.stringNull(r("DESCRIPCION_INDICADOR")), .IdTipoIndicador = CInt(r("ID_TIPO_INDICADOR")), .ValorInicial = CDec(r("VALOR_INICIAL")),
                                   .ValorObjetivo = CDec(r("VALOR_OBJETIVO")), .TituloReto = CStr(r("TITULO_RETO")),
                                   .CodigoProceso = CStr(r("CODIGO_PROCESO")), .TipoIndicador = SabLib.BLL.Utils.stringNull(r("TIPO_INDICADOR")), .MesObjetivo = CInt(r("MES_OBJETIVO")),
                                   .AñoObjetivo = CInt(r("AÑO_OBJETIVO")), .Responsable = CStr(r("RESPONSABLE")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                   .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                   .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .CumplimientoAcciones = SabLib.BLL.Utils.decimalNull(r("CUMPLIMIENTO_ACCIONES")),
                                   .IdPlanta = CInt(r("ID_PLANTA")), .Periodicidad = CInt(r("PERIODICIDAD")), .ValorActual = SabLib.BLL.Utils.decimalNull(r("VALOR_ACTUAL")),
                                   .Reto = CStr(r("RETO")), .ValorAnterior = SabLib.BLL.Utils.decimalNull(r("VALOR_ANTERIOR")),
                                   .TieneDocumentos = CBool(r("TIENE_DOCUMENTOS")), .Sentido = CBool(r("SENTIDO")), .TieneAcciones = CBool(r("TIENE_ACCIONES")),
                                   .IdObjetivoPadre = SabLib.BLL.Utils.integerNull(r("ID_OBJETIVO_PADRE"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlanta"></param>
        Friend Shared Function getEjercicios(idPlanta As Integer) As List(Of Integer)
            Dim query As String = "SELECT AÑO_OBJETIVO FROM VOBJETIVOS WHERE ID_PLANTA=:ID_PLANTA GROUP BY AÑO_OBJETIVO ORDER BY AÑO_OBJETIVO DESC"

            Dim parameter As New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of Integer)(Function(r As OracleDataReader) _
                                                                        CInt(r("AÑO_OBJETIVO")), query, CadenaConexion, parameter)
        End Function

        ''' <summary>
        ''' Obtiene un listado de objetivos
        ''' </summary>
        ''' <param name="idPlanta"></param> 
        ''' <param name="idResponsable"></param>
        ''' <param name="ejercicio"></param>
        ''' <param name="idReto"></param>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idPlanta As Integer, Optional ByVal idResponsable As Integer? = Nothing, Optional ByVal ejercicio As Integer? = Nothing,
                                        Optional ByVal idReto As Integer? = Nothing, Optional ByVal idProceso As Integer? = Nothing) As List(Of ELL.Objetivo)
            Dim query As String = "SELECT * FROM VOBJETIVOS WHERE ID_PLANTA=:ID_PLANTA{0}"
            Dim where As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            If (idResponsable IsNot Nothing AndAlso idResponsable <> Integer.MinValue) Then
                where = " AND ID_RESPONSABLE=:ID_RESPONSABLE"
                lParameters.Add(New OracleParameter("ID_RESPONSABLE", OracleDbType.Int32, idResponsable, ParameterDirection.Input))
            End If

            'If (baja IsNot Nothing) Then
            '    lParameters.Add(New OracleParameter("BAJA", OracleDbType.Int32, baja, ParameterDirection.Input))
            '    where &= " AND BAJA=:BAJA"
            'End If

            If (ejercicio IsNot Nothing AndAlso ejercicio <> Integer.MinValue) Then
                lParameters.Add(New OracleParameter("AÑO_OBJETIVO", OracleDbType.Int32, ejercicio, ParameterDirection.Input))
                where &= " AND AÑO_OBJETIVO=:AÑO_OBJETIVO"
            End If

            If (idReto IsNot Nothing AndAlso idReto <> Integer.MinValue) Then
                lParameters.Add(New OracleParameter("ID_RETO", OracleDbType.Int32, idReto, ParameterDirection.Input))
                where &= " AND ID_RETO=:ID_RETO"
            End If

            If (idProceso IsNot Nothing AndAlso idProceso <> Integer.MinValue) Then
                lParameters.Add(New OracleParameter("ID_PROCESO", OracleDbType.Int32, idProceso, ParameterDirection.Input))
                where &= " AND ID_PROCESO=:ID_PROCESO"
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Objetivo)(Function(r As OracleDataReader) _
            New ELL.Objetivo With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdReto = CInt(r("ID_RETO")), .IdProceso = CInt(r("ID_PROCESO")),
                                   .IdResponsable = CInt(r("ID_RESPONSABLE")), .FechaObjetivo = CDate(r("FECHA_OBJETIVO")), .NombreIndicador = SabLib.BLL.Utils.stringNull(r("NOMBRE_INDICADOR")),
                                   .DescripcionIndicador = SabLib.BLL.Utils.stringNull(r("DESCRIPCION_INDICADOR")), .IdTipoIndicador = CInt(r("ID_TIPO_INDICADOR")), .ValorInicial = CDec(r("VALOR_INICIAL")),
                                   .ValorObjetivo = CDec(r("VALOR_OBJETIVO")), .TituloReto = CStr(r("TITULO_RETO")),
                                   .CodigoProceso = CStr(r("CODIGO_PROCESO")), .TipoIndicador = SabLib.BLL.Utils.stringNull(r("TIPO_INDICADOR")), .MesObjetivo = CInt(r("MES_OBJETIVO")),
                                   .AñoObjetivo = CInt(r("AÑO_OBJETIVO")), .Responsable = CStr(r("RESPONSABLE")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                   .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                   .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .CumplimientoAcciones = SabLib.BLL.Utils.decimalNull(r("CUMPLIMIENTO_ACCIONES")),
                                   .IdPlanta = CInt(r("ID_PLANTA")), .Periodicidad = CInt(r("PERIODICIDAD")), .ValorActual = SabLib.BLL.Utils.decimalNull(r("VALOR_ACTUAL")),
                                   .Reto = CStr(r("RETO")), .ValorAnterior = SabLib.BLL.Utils.decimalNull(r("VALOR_ANTERIOR")),
                                   .TieneDocumentos = CBool(r("TIENE_DOCUMENTOS")), .Sentido = CBool(r("SENTIDO")), .TieneAcciones = CBool(r("TIENE_ACCIONES")),
                                   .IdObjetivoPadre = SabLib.BLL.Utils.integerNull(r("ID_OBJETIVO_PADRE"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene un listado de objetivos
        ''' </summary>
        ''' <param name="idObjetivoPadre"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListByPadre(ByVal idObjetivoPadre As Integer) As List(Of ELL.Objetivo)
            Dim query As String = "SELECT * FROM VOBJETIVOS WHERE ID_OBJETIVO_PADRE=:ID_OBJETIVO_PADRE"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_OBJETIVO_PADRE", OracleDbType.Int32, idObjetivoPadre, ParameterDirection.Input))

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Objetivo)(Function(r As OracleDataReader) _
            New ELL.Objetivo With {.Id = CInt(r("ID")), .Descripcion = CStr(r("DESCRIPCION")), .IdReto = CInt(r("ID_RETO")), .IdProceso = CInt(r("ID_PROCESO")),
                                   .IdResponsable = CInt(r("ID_RESPONSABLE")), .FechaObjetivo = CDate(r("FECHA_OBJETIVO")), .NombreIndicador = SabLib.BLL.Utils.stringNull(r("NOMBRE_INDICADOR")),
                                   .DescripcionIndicador = SabLib.BLL.Utils.stringNull(r("DESCRIPCION_INDICADOR")), .IdTipoIndicador = CInt(r("ID_TIPO_INDICADOR")), .ValorInicial = CDec(r("VALOR_INICIAL")),
                                   .ValorObjetivo = CDec(r("VALOR_OBJETIVO")), .TituloReto = CStr(r("TITULO_RETO")),
                                   .CodigoProceso = CStr(r("CODIGO_PROCESO")), .TipoIndicador = SabLib.BLL.Utils.stringNull(r("TIPO_INDICADOR")), .MesObjetivo = CInt(r("MES_OBJETIVO")),
                                   .AñoObjetivo = CInt(r("AÑO_OBJETIVO")), .Responsable = CStr(r("RESPONSABLE")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                   .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                   .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .CumplimientoAcciones = SabLib.BLL.Utils.decimalNull(r("CUMPLIMIENTO_ACCIONES")),
                                   .IdPlanta = CInt(r("ID_PLANTA")), .Periodicidad = CInt(r("PERIODICIDAD")), .ValorActual = SabLib.BLL.Utils.decimalNull(r("VALOR_ACTUAL")),
                                   .Reto = CStr(r("RETO")), .ValorAnterior = SabLib.BLL.Utils.decimalNull(r("VALOR_ANTERIOR")),
                                   .TieneDocumentos = CBool(r("TIENE_DOCUMENTOS")), .Sentido = CBool(r("SENTIDO")), .TieneAcciones = CBool(r("TIENE_ACCIONES")),
                                   .IdObjetivoPadre = SabLib.BLL.Utils.integerNull(r("ID_OBJETIVO_PADRE"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idTipoIndicador"></param>
        ''' <returns></returns>
        Public Shared Function existTipoIndicador(ByVal idTipoIndicador As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM OBJETIVO WHERE ID_TIPO_INDICADOR=:ID_TIPO_INDICADOR"
            Dim parameter As New OracleParameter("ID_TIPO_INDICADOR", OracleDbType.Int32, idTipoIndicador, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idResponsable"></param>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        Public Shared Function existResponsable(ByVal idResponsable As Integer, ByVal idPlanta As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM OBJETIVO OBJ " _
                                  & "INNER JOIN RETO RET ON RET.ID = OBJ.ID_RETO " _
                                  & "WHERE OBJ.ID_RESPONSABLE=:ID_RESPONSABLE AND RET.ID_PLANTA=:ID_PLANTA"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_RESPONSABLE", OracleDbType.Int32, idResponsable, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())
            Return filas > 0
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idReto"></param>
        ''' <returns></returns>
        Public Shared Function existReto(ByVal idReto As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM OBJETIVO WHERE ID_RETO=:ID_RETO"
            Dim parameter As New OracleParameter("ID_RETO", OracleDbType.Int32, idReto, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idProceso"></param>
        ''' <returns></returns>
        Public Shared Function existProceso(ByVal idProceso As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM OBJETIVO WHERE ID_PROCESO=:ID_PROCESO"
            Dim parameter As New OracleParameter("ID_PROCESO", OracleDbType.Int32, idProceso, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un objetivo
        ''' </summary>
        ''' <param name="objetivo">Objetivo</param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal objetivo As ELL.Objetivo)
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = (objetivo.Id = 0)

            ' Guardamos el objetivo
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, objetivo.Descripcion, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_RETO", OracleDbType.Int32, objetivo.IdReto, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PROCESO", OracleDbType.Int32, objetivo.IdProceso, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_RESPONSABLE", OracleDbType.Int32, objetivo.IdResponsable, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("FECHA_OBJETIVO", OracleDbType.Date, objetivo.FechaObjetivo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("NOMBRE_INDICADOR", OracleDbType.NVarchar2, objetivo.NombreIndicador, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("DESCRIPCION_INDICADOR", OracleDbType.NVarchar2, objetivo.DescripcionIndicador, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_TIPO_INDICADOR", OracleDbType.Int32, objetivo.IdTipoIndicador, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("VALOR_INICIAL", OracleDbType.Decimal, objetivo.ValorInicial, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("VALOR_OBJETIVO", OracleDbType.Decimal, objetivo.ValorObjetivo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("PERIODICIDAD", OracleDbType.Int32, objetivo.Periodicidad, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, If(objetivo.IdPlanta = Integer.MinValue, DBNull.Value, objetivo.IdPlanta), ParameterDirection.Input))
            lParameters.Add(New OracleParameter("SENTIDO", OracleDbType.Int32, objetivo.Sentido, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_OBJETIVO_PADRE", OracleDbType.Int32, If(objetivo.IdObjetivoPadre = Integer.MinValue, DBNull.Value, objetivo.IdObjetivoPadre), ParameterDirection.Input))

            If (bNuevo) Then
                query = "INSERT INTO OBJETIVO (DESCRIPCION, ID_RETO, ID_PROCESO, ID_RESPONSABLE, FECHA_OBJETIVO, NOMBRE_INDICADOR, DESCRIPCION_INDICADOR, " _
                        & "ID_TIPO_INDICADOR, VALOR_INICIAL, VALOR_OBJETIVO, ID_USUARIO_ALTA, PERIODICIDAD, ID_PLANTA, SENTIDO, ID_OBJETIVO_PADRE) VALUES(:DESCRIPCION, :ID_RETO, :ID_PROCESO, :ID_RESPONSABLE, " _
                        & ":FECHA_OBJETIVO, :NOMBRE_INDICADOR, :DESCRIPCION_INDICADOR, :ID_TIPO_INDICADOR, :VALOR_INICIAL, :VALOR_OBJETIVO, :ID_USUARIO_ALTA, " _
                        & ":PERIODICIDAD, :ID_PLANTA, :SENTIDO, :ID_OBJETIVO_PADRE) RETURNING ID INTO :RETURN_VALUE"

                lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, objetivo.IdUsuarioAlta, ParameterDirection.Input))

                Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                p.DbType = DbType.Int32
                lParameters.Add(p)
            Else
                query = "UPDATE OBJETIVO SET DESCRIPCION=:DESCRIPCION, ID_RETO=:ID_RETO, ID_PROCESO=:ID_PROCESO, ID_RESPONSABLE=:ID_RESPONSABLE, FECHA_OBJETIVO=:FECHA_OBJETIVO, " _
                        & "NOMBRE_INDICADOR=:NOMBRE_INDICADOR, DESCRIPCION_INDICADOR=:DESCRIPCION_INDICADOR, ID_TIPO_INDICADOR=:ID_TIPO_INDICADOR, VALOR_INICIAL=:VALOR_INICIAL, " _
                        & "VALOR_OBJETIVO=:VALOR_OBJETIVO, PERIODICIDAD=:PERIODICIDAD, ID_PLANTA=:ID_PLANTA, SENTIDO=:SENTIDO WHERE ID=:ID"

                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, objetivo.Id, ParameterDirection.Input))
            End If

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray)

            If (bNuevo) Then
                objetivo.Id = lParameters.Last.Value
            End If
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un reto
        ''' </summary>
        ''' <param name="idObjetivo">Id del objetivo</param>
        Public Shared Sub DeleteObjetivo(ByVal idObjetivo As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = "DELETE FROM OBJETIVO WHERE ID=:ID"
            Dim listaDocumentos As List(Of ELL.Documento) = DAL.DocumentosDAL.loadList(idObjetivo, ELL.TipoDocumento.Tipo.Objetivo)
            listaDocumentos.AddRange(DAL.DocumentosDAL.loadList(idObjetivo, ELL.TipoDocumento.Tipo.Revision_cierre))
            Dim listaAcciones As List(Of ELL.Accion) = DAL.AccionesDAL.loadListByObjetivo(idObjetivo, Nothing)
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                'Borramos las acciones del objetivo
                For Each accion In listaAcciones
                    DAL.AccionesDAL.DeleteAccion(accion.Id, con)
                Next

                ' Borramos el objetivo
                Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idObjetivo, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, con, parameter)

                ' Borramos los documentos tanto los del propio objetivo como las revisiones
                DAL.DocumentosDAL.DeleteDocumentos(idObjetivo, ELL.TipoDocumento.Tipo.Objetivo, con)
                DAL.DocumentosDAL.DeleteDocumentos(idObjetivo, ELL.TipoDocumento.Tipo.Revision_cierre, con)

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If

                ' Borramos fisicamente los ficheros
                Dim pathFichero As String = Configuration.ConfigurationManager.AppSettings("rootDocumentos")
                For Each documento In listaDocumentos
                    If (IO.Directory.Exists(pathFichero)) Then
                        IO.Directory.GetFiles(pathFichero, documento.Id & ".*").ToList().ForEach(Sub(s) IO.File.Delete(s))
                    End If
                Next
            End Try
        End Sub

#End Region

    End Class

End Namespace