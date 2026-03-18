Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class AccionesDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una accion
        ''' </summary>
        ''' <param name="idAccion">Id acción</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getAccion(ByVal idAccion As Integer) As ELL.Accion
            Dim query As String = "SELECT * FROM VACCIONES WHERE ID=:ID"

            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idAccion, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Accion)(Function(r As OracleDataReader) _
            New ELL.Accion With {.Id = CInt(r("ID")), .IdObjetivo = CInt(r("ID_OBJETIVO")), .Descripcion = CStr(r("DESCRIPCION")), .FechaObjetivo = CDate(r("FECHA_OBJETIVO")),
                                 .Porcentaje = SabLib.BLL.Utils.decimalNull(r("PORCENTAJE")), .GradoImportancia = CDec(r("GRADO_IMPORTANCIA")), .DescripcionObjetivo = CStr(r("DESCRIPCION_OBJETIVO")),
                                 .IdResponsable = CInt(r("ID_RESPONSABLE")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                 .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                 .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .Periodicidad = CInt(r("PERIODICIDAD")),
                                 .TieneDocumentos = CBool(r("TIENE_DOCUMENTOS"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de acciones
        ''' </summary>
        ''' <param name="idResponsable"></param> 
        ''' <param name="baja"></param>
        ''' <param name="idObjetivo"></param> 
        ''' <param name="plazoDesde"></param>
        ''' <param name="plazoHasta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idResponsable As Integer, Optional ByVal baja As Boolean? = False, Optional idObjetivo As Integer? = Nothing, Optional plazoDesde As DateTime? = Nothing, Optional plazoHasta As DateTime? = Nothing) As List(Of ELL.Accion)
            Dim query As String = "SELECT * FROM VACCIONES WHERE ID_RESPONSABLE=:ID_RESPONSABLE{0}"
            Dim where As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_RESPONSABLE", OracleDbType.Int32, idResponsable, ParameterDirection.Input))
            If (baja IsNot Nothing) Then
                lParameters.Add(New OracleParameter("BAJA", OracleDbType.Int32, baja, ParameterDirection.Input))
                where &= " AND BAJA=:BAJA"
            End If

            If (idObjetivo IsNot Nothing AndAlso idObjetivo <> Integer.MinValue) Then
                lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, idObjetivo, ParameterDirection.Input))
                where &= " AND ID_OBJETIVO=:ID_OBJETIVO"
            End If

            If (plazoDesde IsNot Nothing) Then
                lParameters.Add(New OracleParameter("PLAZO_DESDE", OracleDbType.Date, plazoDesde, ParameterDirection.Input))
                If (plazoHasta IsNot Nothing) Then
                    lParameters.Add(New OracleParameter("PLAZO_HASTA", OracleDbType.Date, plazoHasta, ParameterDirection.Input))
                    where &= " AND FECHA_OBJETIVO BETWEEN :PLAZO_DESDE AND :PLAZO_HASTA"
                Else
                    where &= " AND FECHA_OBJETIVO >= :PLAZO_DESDE"
                End If
            Else
                If (plazoHasta IsNot Nothing) Then
                    lParameters.Add(New OracleParameter("PLAZO_HASTA", OracleDbType.Date, plazoHasta, ParameterDirection.Input))
                    where &= " AND FECHA_OBJETIVO <= :PLAZO_HASTA"
                End If
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Accion)(Function(r As OracleDataReader) _
            New ELL.Accion With {.Id = CInt(r("ID")), .IdObjetivo = CInt(r("ID_OBJETIVO")), .Descripcion = CStr(r("DESCRIPCION")), .FechaObjetivo = CDate(r("FECHA_OBJETIVO")),
                                 .Porcentaje = SabLib.BLL.Utils.decimalNull(r("PORCENTAJE")), .GradoImportancia = CDec(r("GRADO_IMPORTANCIA")), .DescripcionObjetivo = CStr(r("DESCRIPCION_OBJETIVO")),
                                 .IdResponsable = CInt(r("ID_RESPONSABLE")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                 .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                 .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .Periodicidad = CInt(r("PERIODICIDAD")),
                                 .TieneDocumentos = CBool(r("TIENE_DOCUMENTOS"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Obtiene un listado de acciones
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <param name="baja"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadListByObjetivo(ByVal idObjetivo As Integer, Optional ByVal baja As Boolean? = False) As List(Of ELL.Accion)
            Dim query As String = "SELECT * FROM VACCIONES WHERE ID_OBJETIVO=:ID_OBJETIVO{0}"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, idObjetivo, ParameterDirection.Input))
            If (baja IsNot Nothing) Then
                lParameters.Add(New OracleParameter("BAJA", OracleDbType.Int32, baja, ParameterDirection.Input))
                query = String.Format(query, " AND BAJA=:BAJA")
            Else
                query = String.Format(query, String.Empty)
            End If

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Accion)(Function(r As OracleDataReader) _
            New ELL.Accion With {.Id = CInt(r("ID")), .IdObjetivo = CInt(r("ID_OBJETIVO")), .Descripcion = CStr(r("DESCRIPCION")), .FechaObjetivo = CDate(r("FECHA_OBJETIVO")),
                                 .Porcentaje = CDec(r("PORCENTAJE")), .GradoImportancia = CDec(r("GRADO_IMPORTANCIA")), .DescripcionObjetivo = CStr(r("DESCRIPCION_OBJETIVO")),
                                 .IdResponsable = CInt(r("ID_RESPONSABLE")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                 .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                                 .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .Periodicidad = CInt(r("PERIODICIDAD")),
                                 .TieneDocumentos = CBool(r("TIENE_DOCUMENTOS"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idObjetivo"></param>
        ''' <returns></returns>
        Public Shared Function existObjetivo(ByVal idObjetivo As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM ACCION WHERE ID_OBJETIVO=:ID_OBJETIVO"
            Dim parameter As New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, idObjetivo, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda una accion
        ''' </summary>
        ''' <param name="accion">Acción</param>
        ''' <remarks></remarks>
        Public Shared Sub Save(ByVal accion As ELL.Accion)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = String.Empty
            Dim bNuevo As Boolean = (accion.Id = 0)
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                Dim lParameters As New List(Of OracleParameter)

                lParameters.Add(New OracleParameter("ID_OBJETIVO", OracleDbType.Int32, accion.IdObjetivo, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, accion.Descripcion, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("FECHA_OBJETIVO", OracleDbType.Date, accion.FechaObjetivo, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("GRADO_IMPORTANCIA", OracleDbType.Decimal, accion.GradoImportancia, ParameterDirection.Input))

                If (bNuevo) Then
                    query = "INSERT INTO ACCION (ID_OBJETIVO, DESCRIPCION, FECHA_OBJETIVO, GRADO_IMPORTANCIA, ID_USUARIO_ALTA) " _
                        & "VALUES (:ID_OBJETIVO, :DESCRIPCION, :FECHA_OBJETIVO, :GRADO_IMPORTANCIA, :ID_USUARIO_ALTA) RETURNING ID INTO :RETURN_VALUE"

                    lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, accion.IdUsuarioAlta, ParameterDirection.Input))

                    Dim p As New OracleParameter("RETURN_VALUE", OracleDbType.Int32, ParameterDirection.ReturnValue)
                    p.DbType = DbType.Int32
                    lParameters.Add(p)
                Else
                    query = "UPDATE ACCION SET ID_OBJETIVO=:ID_OBJETIVO, DESCRIPCION=:DESCRIPCION, FECHA_OBJETIVO=:FECHA_OBJETIVO, " _
                        & "GRADO_IMPORTANCIA=:GRADO_IMPORTANCIA WHERE ID=:ID"

                    lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, accion.Id, ParameterDirection.Input))
                End If

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                If (bNuevo) Then
                    accion.Id = lParameters.Last.Value
                End If

                transact.Commit()
            Catch ex As Exception
                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina una acción
        ''' </summary>
        ''' <param name="idAccion">Id de la acción</param>
        Public Shared Sub DeleteAccion(ByVal idAccion As Integer, Optional ByVal con As OracleConnection = Nothing)
            Dim myCon As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = "DELETE FROM ACCION WHERE ID=:ID"
            Dim listaDocumentos As List(Of ELL.Documento) = DAL.DocumentosDAL.loadList(idAccion, ELL.TipoDocumento.Tipo.Accion)
            Try
                If (con Is Nothing) Then
                    myCon = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                    myCon.Open()
                    transact = myCon.BeginTransaction()
                Else
                    myCon = con
                End If

                ' Borramos la acción
                Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idAccion, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, myCon, parameter)

                ' Borramos los documentos
                DAL.DocumentosDAL.DeleteDocumentos(idAccion, ELL.TipoDocumento.Tipo.Accion, myCon)

                If (con Is Nothing) Then
                    transact.Commit()
                End If
            Catch ex As Exception
                If (con Is Nothing) Then
                    transact.Rollback()
                End If
                Throw ex
            Finally
                If (con Is Nothing AndAlso myCon IsNot Nothing AndAlso myCon.State <> ConnectionState.Closed) Then
                    myCon.Close()
                    myCon.Dispose()
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