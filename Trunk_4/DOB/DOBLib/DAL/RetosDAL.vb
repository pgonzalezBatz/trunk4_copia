Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class RetosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un reto
        ''' </summary>
        ''' <param name="idReto">Id del reto</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getReto(ByVal idReto As Integer) As ELL.Reto
            Dim query As String = "SELECT * FROM VRETOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idReto, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Reto)(Function(r As OracleDataReader) _
            New ELL.Reto With {.Id = CInt(r("ID")), .IdPlanta = CInt(r("ID_PLANTA")), .Codigo = CStr(r("CODIGO")), .Titulo = CStr(r("TITULO")),
                               .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .IdDocumento = SabLib.BLL.Utils.integerNull(r("ID_DOCUMENTO")), .FechaAlta = CDate(r("FECHA_ALTA")),
                               .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                               .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .IdTipoDocumento = SabLib.BLL.Utils.integerNull(r("ID_TIPO_DOCUMENTO")),
                               .NombreFichero = SabLib.BLL.Utils.stringNull(r("NOMBRE_FICHERO")), .Planta = CStr(r("PLANTA"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de retos
        ''' </summary>
        ''' <param name="idPlanta"></param> 
        ''' <param name="plantaDeBaja"></param>
        ''' <param name="nombre"></param> 
        ''' <param name="descripcion"></param>
        ''' <param name="retoDeBaja"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(Optional ByVal idPlanta As Integer? = Nothing, Optional ByVal plantaDeBaja As Boolean? = False, Optional ByVal nombre As String = "", Optional ByVal descripcion As String = "", Optional ByVal retoDeBaja? As Boolean = Nothing) As List(Of ELL.Reto)
            Dim query As String = "SELECT * FROM VRETOS{0}"
            Dim where As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            If (idPlanta IsNot Nothing) Then
                where = " WHERE ID_PLANTA=:ID_PLANTA"
                lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))
            End If

            If (plantaDeBaja IsNot Nothing) Then
                lParameters.Add(New OracleParameter("BAJA", OracleDbType.Int32, plantaDeBaja, ParameterDirection.Input))
                If (String.IsNullOrEmpty(where)) Then
                    where = " WHERE BAJA=:BAJA"
                Else
                    where &= " AND BAJA=:BAJA"
                End If
            End If

            If (Not String.IsNullOrEmpty(nombre)) Then
                lParameters.Add(New OracleParameter("TITULO", OracleDbType.NVarchar2, nombre.ToUpper(), ParameterDirection.Input))
                If (String.IsNullOrEmpty(where)) Then
                    where = " WHERE UPPER(TITULO) LIKE '%' || :TITULO || '%'"
                Else
                    where &= " AND UPPER(TITULO) LIKE '%' || :TITULO || '%'"
                End If
            End If

            If (Not String.IsNullOrEmpty(descripcion)) Then
                lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, descripcion.ToUpper(), ParameterDirection.Input))
                If (String.IsNullOrEmpty(where)) Then
                    where = " WHERE UPPER(DESCRIPCION) LIKE '%' || :DESCRIPCION || '%'"
                Else
                    where &= " AND UPPER(DESCRIPCION) LIKE '%' || :DESCRIPCION || '%'"
                End If
            End If

            If (retoDeBaja IsNot Nothing) Then
                If (String.IsNullOrEmpty(where)) Then
                    If (retoDeBaja) Then
                        where = " WHERE FECHA_BAJA IS NOT NULL"
                    Else
                        where = " WHERE FECHA_BAJA IS NULL"
                    End If
                Else
                    If (retoDeBaja) Then
                        where &= " AND FECHA_BAJA IS NOT NULL"
                    Else
                        where &= " AND FECHA_BAJA IS NULL"
                    End If
                End If
            End If

            query = String.Format(query, where)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Reto)(Function(r As OracleDataReader) _
            New ELL.Reto With {.Id = CInt(r("ID")), .IdPlanta = CInt(r("ID_PLANTA")), .Codigo = CStr(r("CODIGO")), .Titulo = CStr(r("TITULO")),
                               .Descripcion = SabLib.BLL.Utils.stringNull(r("DESCRIPCION")), .IdDocumento = SabLib.BLL.Utils.integerNull(r("ID_DOCUMENTO")), .FechaAlta = CDate(r("FECHA_ALTA")),
                               .FechaBaja = SabLib.BLL.Utils.dateTimeNull(r("FECHA_BAJA")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")),
                               .IdUsuarioBaja = SabLib.BLL.Utils.integerNull(r("ID_USUARIO_BAJA")), .IdTipoDocumento = SabLib.BLL.Utils.integerNull(r("ID_TIPO_DOCUMENTO")),
                               .NombreFichero = SabLib.BLL.Utils.stringNull(r("NOMBRE_FICHERO")), .Planta = CStr(r("PLANTA"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

        ''' <summary>
        ''' Comprueba si existe una planta
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsPlanta(ByVal idPlanta As Integer) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM RETO WHERE ID_PLANTA=:ID_PLANTA"
            Dim parameter As New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input)

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, parameter)
            Return filas > 0
        End Function

        ''' <summary>
        ''' Comprueba si existe el codigo
        ''' </summary>
        ''' <param name="codigo"></param>
        ''' <param name="idPlanta"></param> 
        ''' <param name="idReto"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function existsCodigo(ByVal codigo As String, ByVal idPlanta As Integer, Optional ByVal idReto As Integer? = Nothing) As Boolean
            Dim query As String = "SELECT COUNT(*) FROM RETO WHERE LOWER(CODIGO)=LOWER(:CODIGO) AND ID_PLANTA=:ID_PLANTA{0}"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("CODIGO", OracleDbType.NVarchar2, codigo, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, idPlanta, ParameterDirection.Input))

            If (idReto IsNot Nothing) Then
                lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idReto, ParameterDirection.Input))
                query = String.Format(query, " AND ID <> :ID")
            Else
                query = String.Format(query, String.Empty)
            End If

            Dim filas As Integer = Memcached.OracleDirectAccess.SeleccionarEscalar(Of Integer)(query, CadenaConexion, lParameters.ToArray())
            Return filas > 0
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un reto
        ''' </summary>
        ''' <param name="reto">Reto</param>  
        ''' <param name="buffer">Fichero</param>   
        Public Shared Sub Save(ByVal reto As ELL.Reto, ByVal buffer() As Byte)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim fullPathFichero As String = String.Empty
            Dim query As String = String.Empty
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' Primero guardamos el reto 
                Dim lParameters As New List(Of OracleParameter)
                lParameters.Add(New OracleParameter("CODIGO", OracleDbType.NVarchar2, reto.Codigo, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("TITULO", OracleDbType.NVarchar2, reto.Titulo, ParameterDirection.Input))
                lParameters.Add(New OracleParameter("DESCRIPCION", OracleDbType.NVarchar2, If(String.IsNullOrEmpty(reto.Descripcion), DBNull.Value, reto.Descripcion), ParameterDirection.Input))

                If (reto.Id = 0) Then
                    lParameters.Add(New OracleParameter("ID_PLANTA", OracleDbType.Int32, reto.IdPlanta, ParameterDirection.Input))
                    query = "INSERT INTO RETO (ID_PLANTA, CODIGO, TITULO, DESCRIPCION, ID_USUARIO_ALTA) " _
                            & "VALUES (:ID_PLANTA, UPPER(:CODIGO), :TITULO, :DESCRIPCION, :ID_USUARIO_ALTA) RETURNING ID INTO :RETURN_VALUE"
                    lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, reto.IdUsuarioAlta, ParameterDirection.Input))
                    lParameters.Add(New OracleParameter() With {.ParameterName = "RETURN_VALUE", .OracleDbType = OracleDbType.Int32, .Direction = ParameterDirection.ReturnValue, .DbType = DbType.Int32})
                Else
                    query = "UPDATE RETO SET CODIGO=UPPER(:CODIGO), TITULO=:TITULO, DESCRIPCION=:DESCRIPCION WHERE ID=:ID"
                    lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, reto.Id, ParameterDirection.Input))
                End If

                Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                If (reto.Id = 0) Then
                    reto.Id = lParameters.Last.Value
                End If

                'Guardamos el documento
                If (buffer IsNot Nothing) Then
                    Dim documento As New ELL.Documento With {.Id = reto.IdDocumento, .IdPadre = reto.Id, .IdTipoDocumento = reto.IdTipoDocumento, .NombreFichero = reto.NombreFichero, .IdUsuarioAlta = reto.IdUsuarioAlta}
                    DocumentosDAL.Save(documento, buffer, con)
                End If

                transact.Commit()
            Catch ex As Exception
                If (IO.File.Exists(fullPathFichero)) Then
                    IO.File.Delete(fullPathFichero)
                End If

                transact.Rollback()
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Da de baja un reto
        ''' </summary>
        ''' <param name="idReto">Id del rol</param> 
        ''' <param name="idUsuario">Id del ususario</param>
        ''' <remarks></remarks>
        Public Shared Sub Unsubscribe(ByVal idReto As Integer, ByVal idUsuario As Integer)
            Dim query As String = "UPDATE RETO SET FECHA_BAJA=SYSDATE, ID_USUARIO_BAJA=:ID_USUARIO_BAJA WHERE ID=:ID"

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idReto, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_USUARIO_BAJA", OracleDbType.Int32, idUsuario, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())
        End Sub

        ''' <summary>
        ''' Da de baja un reto
        ''' </summary>
        ''' <param name="idReto">Id del rol</param> 
        ''' <remarks></remarks>
        Public Shared Sub Subscribe(ByVal idReto As Integer)
            Dim query As String = "UPDATE RETO SET FECHA_BAJA=NULL, ID_USUARIO_BAJA=NULL WHERE ID=:ID"
            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, New OracleParameter("ID", OracleDbType.Int32, idReto, ParameterDirection.Input))
        End Sub

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un reto
        ''' </summary>
        ''' <param name="idReto">Id del reto</param>
        Public Shared Sub DeleteReto(ByVal idReto As Integer)
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Dim transact As Oracle.ManagedDataAccess.Client.OracleTransaction = Nothing
            Dim query As String = "DELETE FROM RETO WHERE ID=:ID"
            Dim listaDocumentos As List(Of ELL.Documento) = DAL.DocumentosDAL.loadList(idReto, ELL.TipoDocumento.Tipo.Reto)
            Try
                con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                con.Open()
                transact = con.BeginTransaction()

                ' Borramos el reto
                Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idReto, ParameterDirection.Input)
                Memcached.OracleDirectAccess.NoQuery(query, con, parameter)

                ' Borramos los documentos
                DAL.DocumentosDAL.DeleteDocumentos(idReto, ELL.TipoDocumento.Tipo.Reto, con)

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
                    IO.Directory.GetFiles(pathFichero, documento.Id & ".*").ToList().ForEach(Sub(s) IO.File.Delete(s))
                Next
            End Try
        End Sub

#End Region

    End Class

End Namespace