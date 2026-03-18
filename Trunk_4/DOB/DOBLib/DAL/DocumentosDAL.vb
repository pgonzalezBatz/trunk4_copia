Imports Oracle.ManagedDataAccess.Client

Namespace DAL

    Public Class DocumentosDAL
        Inherits DALBase

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un documento
        ''' </summary>
        ''' <param name="idDocumento">Id del documento</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Shared Function getDocumento(ByVal idDocumento As Integer) As ELL.Documento
            Dim query As String = "SELECT * FROM VDOCUMENTOS WHERE ID=:ID"
            Dim parameter As New OracleParameter("ID", OracleDbType.Int32, idDocumento, ParameterDirection.Input)

            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documento)(Function(r As OracleDataReader) _
            New ELL.Documento With {.Id = CInt(r("ID")), .IdTipoDocumento = CInt(r("ID_TIPO_DOCUMENTO")), .IdPadre = CInt(r("ID_PADRE")),
                                    .NombreFichero = CStr(r("NOMBRE_FICHERO")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                    .Nombre = CStr(r("NOMBRE")), .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                    .Revision = SabLib.BLL.Utils.integerNull(r("REVISION"))}, query, CadenaConexion, parameter).FirstOrDefault()
        End Function

        ''' <summary>
        ''' Obtiene un listado de documentos
        ''' </summary>
        ''' <param name="idPadre"></param>
        ''' <param name="idTipoDocumento"></param>
        ''' <param name="revision"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function loadList(ByVal idPadre As Integer, ByVal idTipoDocumento As Integer, Optional ByVal revision As Integer? = Nothing) As List(Of ELL.Documento)
            Dim query As String = "SELECT * FROM VDOCUMENTOS WHERE ID_PADRE=:ID_PADRE AND ID_TIPO_DOCUMENTO=:ID_TIPO_DOCUMENTO{0}"
            Dim where As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PADRE", OracleDbType.Int32, idPadre, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_TIPO_DOCUMENTO", OracleDbType.Int32, idTipoDocumento, ParameterDirection.Input))

            If (revision IsNot Nothing) Then
                where = " AND REVISION=:REVISION"
                lParameters.Add(New OracleParameter("REVISION", OracleDbType.Int32, revision, ParameterDirection.Input))
            End If

            query = String.Format(query, where)


            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Documento)(Function(r As OracleDataReader) _
            New ELL.Documento With {.Id = CInt(r("ID")), .IdTipoDocumento = CInt(r("ID_TIPO_DOCUMENTO")), .IdPadre = CInt(r("ID_PADRE")),
                                    .NombreFichero = CStr(r("NOMBRE_FICHERO")), .IdUsuarioAlta = CInt(r("ID_USUARIO_ALTA")), .FechaAlta = CDate(r("FECHA_ALTA")),
                                    .Nombre = CStr(r("NOMBRE")), .Apellido1 = SabLib.BLL.Utils.stringNull(r("APELLIDO1")), .Apellido2 = SabLib.BLL.Utils.stringNull(r("APELLIDO2")),
                                    .Revision = SabLib.BLL.Utils.integerNull(r("REVISION"))}, query, CadenaConexion, lParameters.ToArray())
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda un reto
        ''' </summary>
        ''' <param name="documento">Documento</param>  
        ''' <param name="buffer">Fichero</param>   
        ''' <param name="conexion"></param> 
        Public Shared Function Save(ByVal documento As ELL.Documento, ByVal buffer() As Byte, Optional conexion As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing) As Integer
            Dim con As Oracle.ManagedDataAccess.Client.OracleConnection = Nothing
            Try
                If (buffer IsNot Nothing) Then
                    Dim nuevoDoc As Boolean = (documento.Id = 0 OrElse documento.Id = Integer.MinValue)
                    If (conexion IsNot Nothing) Then
                        con = conexion
                    Else
                        con = New Oracle.ManagedDataAccess.Client.OracleConnection(CadenaConexion)
                        con.Open()
                    End If

                    Dim query As String = String.Empty
                    Dim lParameters As New List(Of OracleParameter)
                    lParameters.Add(New OracleParameter("NOMBRE_FICHERO", OracleDbType.NVarchar2, documento.NombreFichero, ParameterDirection.Input))

                    If (nuevoDoc) Then
                        query = "INSERT INTO DOCUMENTO (ID_PADRE, ID_TIPO_DOCUMENTO, NOMBRE_FICHERO, ID_USUARIO_ALTA, REVISION) VALUES (:ID_PADRE, :ID_TIPO_DOCUMENTO, :NOMBRE_FICHERO, :ID_USUARIO_ALTA, :REVISION) RETURNING ID INTO :RETURN_VALUE"

                        lParameters.Add(New OracleParameter("ID_PADRE", OracleDbType.Int32, documento.IdPadre, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_TIPO_DOCUMENTO", OracleDbType.Int32, documento.IdTipoDocumento, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("ID_USUARIO_ALTA", OracleDbType.Int32, documento.IdUsuarioAlta, ParameterDirection.Input))
                        lParameters.Add(New OracleParameter("REVISION", OracleDbType.Int32, If(documento.Revision = Integer.MinValue, DBNull.Value, documento.Revision), ParameterDirection.Input))
                        lParameters.Add(New OracleParameter() With {.ParameterName = "RETURN_VALUE", .OracleDbType = OracleDbType.Int32, .Direction = ParameterDirection.ReturnValue, .DbType = DbType.Int32})
                    Else
                        query = "UPDATE DOCUMENTO SET NOMBRE_FICHERO=:NOMBRE_FICHERO WHERE ID=:ID"
                        lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, documento.Id, ParameterDirection.Input))
                    End If

                    Memcached.OracleDirectAccess.NoQuery(query, con, lParameters.ToArray)

                    If (nuevoDoc) Then
                        documento.Id = lParameters.Last.Value
                    End If

                    ' Guardamos el fichero en la ruta física
                    Dim pathFichero As String = Configuration.ConfigurationManager.AppSettings("rootDocumentos")
                    Dim fullPathFichero = IO.Path.Combine(pathFichero, documento.Id & IO.Path.GetExtension(documento.NombreFichero))

                    If (Not IO.Directory.Exists(pathFichero)) Then
                        IO.Directory.CreateDirectory(pathFichero)
                    Else
                        ' Si existe algún documento con ese id de documento lo borramos
                        IO.Directory.GetFiles(pathFichero, documento.Id & ".*").ToList().ForEach(Sub(s) IO.File.Delete(s))
                    End If

                    IO.File.WriteAllBytes(fullPathFichero, buffer)
                End If
            Catch ex As Exception
                Throw ex
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed AndAlso conexion Is Nothing) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try

            Return documento.Id
        End Function

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' Elimina un documento
        ''' </summary>
        ''' <param name="idDocumento">Id del documento</param>
        Public Shared Sub DeleteDocumento(ByVal idDocumento As Integer)
            Dim query As String = "DELETE FROM DOCUMENTO WHERE ID=:ID"
            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID", OracleDbType.Int32, idDocumento, ParameterDirection.Input))

            Memcached.OracleDirectAccess.NoQuery(query, CadenaConexion, lParameters.ToArray())

            ' Borramos fisicamente el fichero
            Dim pathFichero As String = Configuration.ConfigurationManager.AppSettings("rootDocumentos")
            IO.Directory.GetFiles(pathFichero, idDocumento & ".*").ToList().ForEach(Sub(s) IO.File.Delete(s))
        End Sub

        ''' <summary>
        ''' Elimina un reto
        ''' </summary>
        ''' <param name="idPadre">Id padre</param>
        ''' <param name="idTipoDocumento">Id tipo documento</param>
        ''' <param name="conexion"></param>
        ''' <param name="revision"></param>
        Public Shared Sub DeleteDocumentos(ByVal idPadre As Integer, ByVal idTipoDocumento As Integer, ByVal conexion As Oracle.ManagedDataAccess.Client.OracleConnection, Optional ByVal revision As Integer? = Nothing)
            Dim query As String = "DELETE FROM DOCUMENTO WHERE ID_PADRE=:ID_PADRE AND ID_TIPO_DOCUMENTO=:ID_TIPO_DOCUMENTO{0}"
            Dim where As String = String.Empty

            Dim lParameters As New List(Of OracleParameter)
            lParameters.Add(New OracleParameter("ID_PADRE", OracleDbType.Int32, idPadre, ParameterDirection.Input))
            lParameters.Add(New OracleParameter("ID_TIPO_DOCUMENTO", OracleDbType.Int32, idTipoDocumento, ParameterDirection.Input))

            If (revision IsNot Nothing) Then
                where = " AND REVISION=:REVISION"
                lParameters.Add(New OracleParameter("REVISION", OracleDbType.Int32, revision, ParameterDirection.Input))
            End If

            query = String.Format(query, where)

            Memcached.OracleDirectAccess.NoQuery(query, conexion, lParameters.ToArray())
        End Sub

#End Region

    End Class

End Namespace