Imports Oracle.ManagedDataAccess.Client
'Clases que repersentan las tablas de la base de datos.
Namespace Entidades
	Public Class Gertakariak_DAL
		Inherits Gertakariak_ELL

		''' <summary>
		''' Carga de Incidencias.
		''' </summary>
		''' <returns>List(Of gtkGertakariak)</returns>
		''' <remarks></remarks>
		<Obsolete("Dejar de usar esta funcion para usar LINQ to Entities.")> _
		Protected Function Load(Optional FiltroGTK As gtkFiltro = Nothing) As List(Of Gertakariak_DAL)
			Dim Funciones As New Funciones
			Dim CerrarConexion As Boolean = False
			Dim sqlWHERE As String = String.Empty
			Dim sqlLIKE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)

			'------------------------------------------------------------------------------
			'Consulta y relaciones entre las tablas (SELECT - FROM).
			'------------------------------------------------------------------------------
			Dim sSQL As String = "SELECT DISTINCT GERTAKARIAK.ID, DESCRIPCIONPROBLEMA, IDTIPOINCIDENCIA, FECHAAPERTURA, FECHACIERRE, IDCREADOR, IDACTIVO FROM GERTAKARIAK"
			sSQL &= If(FiltroGTK Is Nothing OrElse FiltroGTK.FechaPrevista Is Nothing, " LEFT OUTER", String.Empty) & _
				" JOIN (ACCIONES INNER JOIN GERTAKARIAK_ACCIONES ON  ACCIONES.ID = GERTAKARIAK_ACCIONES.IDACCION) ON GERTAKARIAK.ID = GERTAKARIAK_ACCIONES.IDINCIDENCIA"

			If FiltroGTK IsNot Nothing Then
				If FiltroGTK.Responsables IsNot Nothing Then
					sSQL &= " LEFT OUTER JOIN RESPONSABLES_GERTAKARIAK ON GERTAKARIAK.ID = RESPONSABLES_GERTAKARIAK.IDINCIDENCIA"
					sSQL &= " LEFT OUTER JOIN EQUIPORESOLUCION ON GERTAKARIAK.ID = EQUIPORESOLUCION.IDINCIDENCIA"
					sSQL &= " LEFT OUTER JOIN DETECCION ON GERTAKARIAK.ID = DETECCION.ID"
					sSQL &= " LEFT OUTER JOIN ACCIONES_USUARIOS ON ACCIONES.ID = ACCIONES_USUARIOS.IDACCION"
					sSQL &= " LEFT OUTER JOIN ACCIONES_EJECUCION ON ACCIONES.ID = ACCIONES_EJECUCION.IDACCION"
				End If
				If FiltroGTK.Responsables IsNot Nothing Or FiltroGTK.Descripcion <> String.Empty Then
					sSQL &= " LEFT OUTER JOIN ACCIONES_OBSERVACIONES ON ACCIONES.ID = ACCIONES_OBSERVACIONES.IDACCION"
				End If
				If FiltroGTK.Caracteristicas IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Count > 0 Then
					sSQL &= " JOIN CARACTERISTICAS ON GERTAKARIAK.ID = CARACTERISTICAS.IDINCIDENCIA"
				End If
			End If
			'------------------------------------------------------------------------------

			'------------------------------------------------------------------------------
			'Parametros de Busqueda (WHERE).
			'------------------------------------------------------------------------------
			If Me.IdTipoIncidencia IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " GERTAKARIAK.IDTIPOINCIDENCIA = :p_IDTIPOINCIDENCIA"
				Parametros.Add(New OracleParameter("p_IDTIPOINCIDENCIA", OracleDbType.Int32, Me.IdTipoIncidencia, ParameterDirection.Input))
			End If
			If FiltroGTK Is Nothing Then
				If Me.Id IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " GERTAKARIAK.ID = :p_ID"
					Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
				End If
			Else
				If Me.IdCreador IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " GERTAKARIAK.IDCREADOR = :p_IDCREADOR"
					Parametros.Add(New OracleParameter("p_IDCREADOR", OracleDbType.Int32, Me.IdCreador, ParameterDirection.Input))
				End If

				'------------------------------------------------------------------------------
				'Parametros de Busqueda cuando hay Filtro (FiltroGTK as gtkFiltro).
				'------------------------------------------------------------------------------
				'If FiltroGTK.FechaPrevista IsNot Nothing Then
				'    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " ACCIONES.FECHAPREVISTA = :p_FECHAPREVISTA"
				'    Parametros.Add(New OracleParameter("p_FECHAPREVISTA", OracleDbType.Date, FiltroGTK.FechaPrevista, ParameterDirection.Input))
				'End If
				If FiltroGTK.Estado IsNot Nothing Then
					If FiltroGTK.Estado = gtkFiltro.EstadoIncidencia.Abierta Then
						sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " GERTAKARIAK.FECHACIERRE IS NULL"
					Else
						sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " GERTAKARIAK.FECHACIERRE IS NOT NULL"
					End If
				End If
				If FiltroGTK.Descripcion IsNot Nothing Then
					If IsNumeric(FiltroGTK.Descripcion) Then
						sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " GERTAKARIAK.ID = :p_ID"
						Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, CInt(FiltroGTK.Descripcion), ParameterDirection.Input))
					End If
					sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(GERTAKARIAK.DESCRIPCIONPROBLEMA, :p_DESCRIPCIONPROBLEMA, 'i')"
					sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(GERTAKARIAK.TITULO, :p_DESCRIPCIONPROBLEMA, 'i')"
					sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(GERTAKARIAK.CAUSAPROBLEMA, :p_DESCRIPCIONPROBLEMA, 'i')"
					sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(GERTAKARIAK.OBSERVACIONESCOSTE, :p_DESCRIPCIONPROBLEMA, 'i')"
					sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(ACCIONES.DESCRIPCION, :p_DESCRIPCIONPROBLEMA, 'i')"
					sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(ACCIONES.EFICACIA, :p_DESCRIPCIONPROBLEMA, 'i')"
					sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(ACCIONES_OBSERVACIONES.DESCRIPCION, :p_DESCRIPCIONPROBLEMA, 'i')"
					Parametros.Add(New OracleParameter("p_DESCRIPCIONPROBLEMA", OracleDbType.NVarchar2, SABLib.BLL.Utils.TextoLike(FiltroGTK.Descripcion), ParameterDirection.Input))
				End If
				If sqlLIKE IsNot String.Empty Then sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " (" & sqlLIKE & ")"

				'Responsables
				If FiltroGTK.Responsables IsNot Nothing AndAlso FiltroGTK.Responsables.Count > 0 Then
					Dim strIDs As String = String.Empty
					For Each item As Integer In FiltroGTK.Responsables
						If Not strIDs Is String.Empty Then strIDs = strIDs & ","
						strIDs = strIDs & item.ToString()
					Next
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND")
					sqlWHERE &= (" (")
					sqlWHERE &= (" RESPONSABLES_GERTAKARIAK.IDUSUARIO IN (" & strIDs & ") OR DETECCION.IDUSUARIO IN (" & strIDs & ") OR EQUIPORESOLUCION.IDASISTENTE IN (" & strIDs & ")")
					sqlWHERE &= (" OR")
					sqlWHERE &= (" ACCIONES_USUARIOS.IDUSUARIO IN (" & strIDs & ") OR ACCIONES_EJECUCION.IDUSUARIO IN (" & strIDs & ") OR ACCIONES_OBSERVACIONES.IDUSUARIO IN (" & strIDs & ")")
					sqlWHERE &= (")")
				End If

				'Caracteristicas.
				If FiltroGTK.Caracteristicas IsNot Nothing AndAlso FiltroGTK.Caracteristicas.Count > 0 Then
					Dim strIDs As String = String.Empty
					For Each item As Integer In FiltroGTK.Caracteristicas
						If Not strIDs Is String.Empty Then strIDs = strIDs & ","
						strIDs = strIDs & item.ToString()
					Next
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " CARACTERISTICAS.IDESTRUCTURA IN (" & strIDs & ")"
				End If

				'Activos (Asset).
				If FiltroGTK.Activos IsNot Nothing AndAlso FiltroGTK.Activos.Count > 0 Then
					Dim strIDs As String = String.Empty
					For Each item As String In FiltroGTK.Activos
						If Not strIDs Is String.Empty Then strIDs = strIDs & ","
						strIDs = strIDs & "'" & item.ToString & "'"
					Next
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " GERTAKARIAK.IDACTIVO IN (" & strIDs & ")"
				End If
				'------------------------------------------------------------------------------
			End If
			'------------------------------------------------------------------------------

			'------------------------------------------------------------------------------
			'Cargamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			sSQL &= sqlWHERE
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Load = Memcached.OracleDirectAccess.Seleccionar(Of Gertakariak_DAL) _
			 (Function(ODR As OracleDataReader) New Gertakariak_DAL _
			 With { _
			  .Id = If(ODR("ID") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("ID"))) _
			 , .DescripcionProblema = If(ODR("DESCRIPCIONPROBLEMA") Is DBNull.Value, Nothing, ODR("DESCRIPCIONPROBLEMA")) _
			 , .IdTipoIncidencia = If(ODR("IDTIPOINCIDENCIA") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDTIPOINCIDENCIA"))) _
			 , .FechaApertura = If(ODR("FECHAAPERTURA") Is DBNull.Value, New Nullable(Of Date), CDate(ODR("FECHAAPERTURA"))) _
			 , .FechaCierre = If(ODR("FECHACIERRE") Is DBNull.Value, New Nullable(Of Date), CDate(ODR("FECHACIERRE"))) _
			 , .IdCreador = If(ODR("IDCREADOR") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDCREADOR"))) _
			 , .IdActivo = If(ODR("IDACTIVO") Is DBNull.Value, Nothing, ODR("IDACTIVO")) _
			 }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Return Load
		End Function
		Protected Sub Insert()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "INSERT INTO GERTAKARIAK " _
			 & "(IDTIPOINCIDENCIA, FECHAAPERTURA, FECHACIERRE, DESCRIPCIONPROBLEMA, IDCREADOR, IDACTIVO) " _
			 & "VALUES (:p_IdTipoIncidencia, :p_FechaApertura, :p_FechaCierre, :p_DescripcionProblema, :p_IDCREADOR, :p_IDACTIVO) " _
			 & "RETURNING ID INTO :p_id "
			'------------------------------------------------------------------------------
			'Parametros
			'------------------------------------------------------------------------------
			Dim Parametros As New List(Of OracleParameter)
			Parametros.Add(New OracleParameter("p_IdTipoIncidencia", OracleDbType.Int32, Me.IdTipoIncidencia, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_FechaApertura", OracleDbType.Date, Me.FechaApertura, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_FechaCierre", OracleDbType.Date, Me.FechaCierre, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_DescripcionProblema", OracleDbType.NVarchar2, Me.DescripcionProblema, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_IDCREADOR", OracleDbType.Int32, Me.IdCreador, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_IDACTIVO", OracleDbType.NVarchar2, Me.IdActivo, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.InputOutput))
			'------------------------------------------------------------------------------
			'------------------------------------------------------------------------------
			'Insertamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If Cerrarconexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Me.Id = Parametros.Find(Function(o As OracleParameter) o.ParameterName = "p_id").Value.ToString
		End Sub
		Protected Sub Update()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "UPDATE GERTAKARIAK SET FECHAAPERTURA = :p_FechaApertura,  FECHACIERRE = :p_FechaCierre, DESCRIPCIONPROBLEMA = :p_DescripcionProblema, IDCREADOR = :p_IDCREADOR, IDACTIVO = :p_IDACTIVO " _
		  & "WHERE ID = :p_id"
			Dim Parametros As New List(Of OracleParameter)
			Try
				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Parametros.Add(New OracleParameter("p_FechaApertura", OracleDbType.Date, Me.FechaApertura, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_FechaCierre", OracleDbType.Date, Me.FechaCierre, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_DescripcionProblema", OracleDbType.NVarchar2, Me.DescripcionProblema, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_IDCREADOR", OracleDbType.Int32, Me.IdCreador, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_IDACTIVO", OracleDbType.NVarchar2, Me.IdActivo, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
		Protected Sub Delete()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "DELETE FROM GERTAKARIAK WHERE ID=:p_id"
			Dim Parametros As New List(Of OracleParameter)
			Try

				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input)
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, p_id)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			Catch ex As Exception
				throw 
			End Try
		End Sub
	End Class
	Public Class Deteccion_DAL
		Inherits Deteccion_ELL

		''' <summary>
		''' Carga de Afectados.
		''' </summary>
		''' <returns>List(Of gtkAfectado)</returns>
		''' <remarks></remarks>
		Protected Function Load() As List(Of Deteccion_DAL)
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "SELECT ID, IDUSUARIO, IDINCIDENCIA, IDDEPARTAMENTO FROM DETECCION"
			Dim sqlWHERE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			'------------------------------------------------------------------------------
			'Parametros de Busqueda.
			'------------------------------------------------------------------------------
			If Me.Id IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " ID = :p_ID"
				Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
			End If
			If Me.IdUsuario IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDUSUARIO = :p_IDUSUARIO"
				Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
			End If
			If Me.IdIncidencia IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDINCIDENCIA = :p_IdIncidencia"
				Parametros.Add(New OracleParameter("p_IdIncidencia", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
			End If
			'------------------------------------------------------------------------------
			'------------------------------------------------------------------------------
			'Cargamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			sSQL &= sqlWHERE
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Load = Memcached.OracleDirectAccess.Seleccionar(Of Deteccion_DAL) _
			 (Function(ODR As OracleDataReader) New Deteccion_DAL _
			 With { _
			  .Id = If(ODR("ID") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("ID"))) _
			 , .IdUsuario = If(ODR("IDUSUARIO") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDUSUARIO"))) _
			 , .IdIncidencia = If(ODR("IDINCIDENCIA") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDINCIDENCIA"))) _
			 , .IdDepartamento = If(ODR("IDDEPARTAMENTO") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDDEPARTAMENTO"))) _
			 }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Return Load
		End Function
		''' <summary>
		''' Actualizacion de la tabla DETECCION.
		''' </summary>
		''' <remarks></remarks>
		Protected Sub Save()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "UPDATE DETECCION SET IDDEPARTAMENTO = :p_EstadoParte " _
			 & "WHERE ID = :p_idAfectadoSuceso"
			Dim Parametros As New List(Of OracleParameter)
			Try
				'If Me.IdAfectadoSuceso Is Nothing Then Throw New BatzException("Falta el identificador unico del registro.", New ApplicationException)
				If Me.Id Is Nothing Then Throw New Exception("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Parametros.Add(New OracleParameter("p_EstadoParte", OracleDbType.Int32, Me.IdDepartamento, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_idAfectadoSuceso", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			Catch ex As Exception
				throw 
			End Try
		End Sub
		Protected Sub Delete()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "DELETE FROM DETECCION WHERE ID=:p_id"
			Try
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input)
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, p_id)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
	End Class
	Public Class Responsables_Gertakariak_DAL
		Inherits Responsables_Gertakariak_ELL
		''' <summary>
		''' Carga de Incidencias.
		''' </summary>
		''' <returns>List(Of gtkGertakariak)</returns>
		''' <remarks></remarks>
		Protected Function Load() As List(Of Responsables_Gertakariak_DAL)
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "SELECT ID, IDUSUARIO, IDINCIDENCIA FROM RESPONSABLES_GERTAKARIAK"
			Dim sqlWHERE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			'------------------------------------------------------------------------------
			'Parametros de Busqueda.
			'------------------------------------------------------------------------------
			If Me.Id IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " ID = :p_ID"
				Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
			End If
			If Me.IdIncidencia IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDINCIDENCIA = :p_IDINCIDENCIA"
				Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
			End If
			'------------------------------------------------------------------------------
			'------------------------------------------------------------------------------
			'Cargamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			sSQL &= sqlWHERE
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Load = Memcached.OracleDirectAccess.Seleccionar(Of Responsables_Gertakariak_DAL) _
			 (Function(ODR As OracleDataReader) New Responsables_Gertakariak_DAL _
			 With { _
			  .Id = If(ODR("ID") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("ID"))) _
			 , .IdIncidencia = If(ODR("IDINCIDENCIA") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDINCIDENCIA"))) _
			 , .IdUsuario = If(ODR("IDUSUARIO") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDUSUARIO"))) _
			 }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Return Load
		End Function
		Protected Sub Delete()
			Dim sSQL As String = "DELETE FROM RESPONSABLES_GERTAKARIAK WHERE ID=:p_id"
			Dim Parametros As New List(Of OracleParameter)
			Try

				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input)
				'------------------------------------------------------------------------------
				Memcached.OracleDirectAccess.NoQuery(sSQL _
					, If(My.Settings.ConexionOracle.State = ConnectionState.Closed, My.Settings.ConexionOracle.ConnectionString, My.Settings.ConexionOracle) _
					, p_id)
			Catch ex As Exception
				throw 
			End Try
		End Sub
		Protected Sub Insert()
			Dim sSQL As String = "INSERT INTO RESPONSABLES_GERTAKARIAK " _
			 & "(IDUSUARIO, IDINCIDENCIA) " _
			 & "VALUES (:p_IDUSUARIO, :p_IDINCIDENCIA) " _
			 & "RETURNING ID INTO :p_id "
			'------------------------------------------------------------------------------
			'Parametros
			'------------------------------------------------------------------------------
			Dim Parametros As New List(Of OracleParameter)
			Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.InputOutput))
			'------------------------------------------------------------------------------
			'------------------------------------------------------------------------------
			'Insertamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			Memcached.OracleDirectAccess.NoQuery(sSQL _
	, If(My.Settings.ConexionOracle.State = ConnectionState.Closed, My.Settings.ConexionOracle.ConnectionString, My.Settings.ConexionOracle) _
	, Parametros.ToArray)
			'------------------------------------------------------------------------------
			Me.Id = Parametros.Find(Function(o As OracleParameter) o.ParameterName = "p_id").Value.ToString
		End Sub
		Protected Sub Update()
			Dim sSQL As String = "UPDATE RESPONSABLES_GERTAKARIAK SET IDUSUARIO = :p_IDUSUARIO,  IDINCIDENCIA = :p_IDINCIDENCIA " _
	 & "WHERE ID = :p_id"
			Dim Parametros As New List(Of OracleParameter)
			Try
				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
				'------------------------------------------------------------------------------
				Memcached.OracleDirectAccess.NoQuery(sSQL _
				 , If(My.Settings.ConexionOracle.State = ConnectionState.Closed, My.Settings.ConexionOracle.ConnectionString, My.Settings.ConexionOracle) _
				, Parametros.ToArray)
			Catch ex As Exception
				throw 
			End Try
		End Sub
	End Class
	Public Class Estructura_DAL
		Inherits GertakariakLib2.Estructura_ELL

        Protected Sub Delete()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "DELETE FROM ESTRUCTURA WHERE ID=:p_id"
            Dim Parametros As New List(Of OracleParameter)
            Try
                If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
                '------------------------------------------------------------------------------
                'Parametros
                '------------------------------------------------------------------------------
                Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input)
                '------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                'Ejecutamos la Query.
                '------------------------------------------------------------------------------
                If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
                Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, p_id)
                If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
                '------------------------------------------------------------------------------
            Catch ex As Exception
                throw 
            End Try
        End Sub
        Protected Sub Insert()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "INSERT INTO ESTRUCTURA " _
             & "(IDITURRIA, DESCRIPCION) " _
             & "VALUES (:p_IDITURRIA, :p_DESCRIPCION) " _
             & "RETURNING ID INTO :p_id "
            '------------------------------------------------------------------------------
            'Parametros
            '------------------------------------------------------------------------------
            Dim Parametros As New List(Of OracleParameter)
            Parametros.Add(New OracleParameter("p_IDITURRIA", OracleDbType.Int32, Me.IdIturria, ParameterDirection.Input))
            Parametros.Add(New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, Me.Descripcion, ParameterDirection.Input))
            Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.InputOutput))
            '------------------------------------------------------------------------------
            '------------------------------------------------------------------------------
            'Insertamos los datos especificos del objeto.
            '------------------------------------------------------------------------------
            If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
            Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
            If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
            '------------------------------------------------------------------------------
            Me.Id = Parametros.Find(Function(o As OracleParameter) o.ParameterName = "p_id").Value.ToString
        End Sub
		''' <summary>
		''' Carga de Registro.
		''' </summary>
		''' <returns>List(Of Estructura_DAL)</returns>
		''' <remarks></remarks>
		<Obsolete("Dejar de usar esta funcion para usar LINQ.")> _
		Protected Function Load() As List(Of Estructura_DAL)
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "SELECT ESTRUCTURA.ID, IDITURRIA, DESCRIPCION, IDTIPOINCIDENCIA FROM ESTRUCTURA " & _
								" LEFT JOIN CLASIFICACION ON ESTRUCTURA.ID = CLASIFICACION.ID"
			'SELECT ESTRUCTURA.ID,  ESTRUCTURA.IDITURRIA,  ESTRUCTURA.DESCRIPCION,  CLASIFICACION.IDTIPOINCIDENCIA FROM ESTRUCTURA LEFT JOIN CLASIFICACION ON ESTRUCTURA.ID = CLASIFICACION.ID WHERE CLASIFICACION.IDTIPOINCIDENCIA = 6
			'SELECT ESTRUCTURA.ID,  ESTRUCTURA.IDITURRIA,  ESTRUCTURA.DESCRIPCION,  CLASIFICACION.IDTIPOINCIDENCIA FROM ESTRUCTURA LEFT JOIN CLASIFICACION ON ESTRUCTURA.ID = CLASIFICACION.ID WHERE IDITURRIA=4
			Dim sqlWHERE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			'------------------------------------------------------------------------------
			'Parametros de Busqueda.
			'------------------------------------------------------------------------------
			If Me.Id IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " ESTRUCTURA.ID = :p_ID"
				Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
			End If
			If Me.IdIturria IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDITURRIA = :p_IDITURRIA"
				Parametros.Add(New OracleParameter("p_IDITURRIA", OracleDbType.Int32, Me.IdIturria, ParameterDirection.Input))
			End If
			If Me.IdTipoIncidencia IsNot Nothing And IdIturria Is Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " CLASIFICACION.IDTIPOINCIDENCIA = :p_IDTIPOINCIDENCIA"
				Parametros.Add(New OracleParameter("p_IDTIPOINCIDENCIA", OracleDbType.Int32, Me.IdTipoIncidencia, ParameterDirection.Input))
			End If
			'------------------------------------------------------------------------------
			'------------------------------------------------------------------------------
			'Cargamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			sSQL &= sqlWHERE
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Load = Memcached.OracleDirectAccess.Seleccionar(Of Estructura_DAL) _
			 (Function(ODR As OracleDataReader) New Estructura_DAL _
			 With { _
			  .Id = If(ODR("ID") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("ID"))) _
			 , .IdIturria = If(ODR("IDITURRIA") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDITURRIA"))) _
			 , .Descripcion = If(ODR("DESCRIPCION") Is DBNull.Value, Nothing, ODR("DESCRIPCION")) _
		   , .IdTipoIncidencia = If(ODR("IDTIPOINCIDENCIA") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDTIPOINCIDENCIA"))) _
			 }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Return Load
		End Function
		Protected Sub Update()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "UPDATE ESTRUCTURA SET IDITURRIA = :p_IDITURRIA,  DESCRIPCION = :p_DESCRIPCION " _
			  & "WHERE ID = :p_id"
			Dim Parametros As New List(Of OracleParameter)
			Try
				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Parametros.Add(New OracleParameter("p_IDITURRIA", OracleDbType.Int32, Me.IdIturria, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, Me.Descripcion, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
    End Class
    Public Class Caracteristica_DAL
        Inherits GertakariakLib2.Caracteristicas_ELL

        Protected Sub Delete()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "DELETE FROM CARACTERISTICAS"
            Dim sqlWHERE As String = String.Empty
            Dim Parametros As New List(Of OracleParameter)

            Try
                If Me.IdEstructura Is Nothing Or Me.IdIncidencia Is Nothing Then Throw New ApplicationException("Falta los identificadores del registro.", New ApplicationException)
                '------------------------------------------------------------------------------
                'Parametros de Busqueda.
                '------------------------------------------------------------------------------
                If Me.IdEstructura IsNot Nothing Then
                    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDESTRUCTURA = :p_IDESTRUCTURA"
                    Parametros.Add(New OracleParameter("p_IDESTRUCTURA", OracleDbType.Int32, Me.IdEstructura, ParameterDirection.Input))
                End If
                If Me.IdIncidencia IsNot Nothing Then
                    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDINCIDENCIA = :p_IDINCIDENCIA"
                    Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
                End If
                '------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                'Ejecutamos la Query.
                '------------------------------------------------------------------------------
                sSQL &= sqlWHERE
                If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
                Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
                If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
                '------------------------------------------------------------------------------
            Catch ex As Exception
                throw 
            End Try
        End Sub
        Protected Sub Insert()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "INSERT INTO CARACTERISTICAS" _
             & " (IDESTRUCTURA, IDINCIDENCIA)" _
             & " VALUES (:p_IDESTRUCTURA, :p_IDINCIDENCIA)"
            Try
                '------------------------------------------------------------------------------
                'Parametros
                '------------------------------------------------------------------------------
                Dim Parametros As New List(Of OracleParameter)
                Parametros.Add(New OracleParameter("p_IDESTRUCTURA", OracleDbType.Int32, Me.IdEstructura, ParameterDirection.Input))
                Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
                '------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                'Insertamos los datos especificos del objeto.
                '------------------------------------------------------------------------------
                If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
                Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
                If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
                '------------------------------------------------------------------------------
            Catch ex As Exception
                throw 
            End Try
        End Sub
        ''' <summary>
        ''' Carga de Registro.
        ''' </summary>
        ''' <returns>List(Of Estructura_DAL)</returns>
        ''' <remarks></remarks>
        Protected Function Load() As List(Of Caracteristica_DAL)
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "SELECT IDESTRUCTURA, IDINCIDENCIA FROM CARACTERISTICAS"
            Dim sqlWHERE As String = String.Empty
            Dim Parametros As New List(Of OracleParameter)
            '------------------------------------------------------------------------------
            'Parametros de Busqueda.
            '------------------------------------------------------------------------------
            If Me.IdEstructura IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDESTRUCTURA = :p_IDESTRUCTURA"
                Parametros.Add(New OracleParameter("p_IDESTRUCTURA", OracleDbType.Int32, Me.IdEstructura, ParameterDirection.Input))
            End If
            If Me.IdIncidencia IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDINCIDENCIA = :p_IDINCIDENCIA"
                Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
            End If
            '------------------------------------------------------------------------------
            '------------------------------------------------------------------------------
            'Cargamos los datos especificos del objeto.
            '------------------------------------------------------------------------------
            sSQL &= sqlWHERE
            If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
            Load = Memcached.OracleDirectAccess.Seleccionar(Of Caracteristica_DAL) _
             (Function(ODR As OracleDataReader) New Caracteristica_DAL _
             With { _
              .IdEstructura = If(ODR("IDESTRUCTURA") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDESTRUCTURA"))) _
             , .IdIncidencia = If(ODR("IDINCIDENCIA") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDINCIDENCIA"))) _
             }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
            If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
            '------------------------------------------------------------------------------
            Return Load
        End Function
        Protected Sub Update()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "UPDATE CARACTERISTICAS SET IDESTRUCTURA = :p_IDESTRUCTURA,  IDINCIDENCIA = :p_IDINCIDENCIA"
            Dim sqlWHERE As String = String.Empty
            Dim Parametros As New List(Of OracleParameter)
            Try
                If Me.IdEstructura Is Nothing Or Me.IdIncidencia Is Nothing Then Throw New ApplicationException("Falta los identificadores del registro.", New ApplicationException)
                '------------------------------------------------------------------------------
                'Parametros de Busqueda.
                '------------------------------------------------------------------------------
                If Me.IdEstructura IsNot Nothing Then
                    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDESTRUCTURA = :p_IDESTRUCTURA"
                    Parametros.Add(New OracleParameter("p_IDESTRUCTURA", OracleDbType.Int32, Me.IdEstructura, ParameterDirection.Input))
                End If
                If Me.IdIncidencia IsNot Nothing Then
                    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDINCIDENCIA = :p_IDINCIDENCIA"
                    Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
                End If
                '------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                'Ejecutamos la Query.
                '------------------------------------------------------------------------------
                sSQL &= sqlWHERE
                If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
                Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
                If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
                '------------------------------------------------------------------------------
            Catch ex As Exception
                throw 
            End Try
        End Sub
    End Class
	Public Class Acciones_DAL
		Inherits GertakariakLib2.Acciones_ELL

		Protected Sub Delete()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "DELETE FROM ACCIONES WHERE ID=:p_id"
			Dim Parametros As New List(Of OracleParameter)
			Try

				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input)
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, p_id)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
		Protected Sub Insert()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "INSERT INTO ACCIONES " _
			 & "(DESCRIPCION, IDTIPOACCION, FECHAINICIO, FECHAFIN, FECHAPREVISTA, EFICACIA, REALIZACION) " _
			 & "VALUES (:p_DESCRIPCION, :p_IDTIPOACCION, :p_FECHAINICIO, :p_FECHAFIN, :p_FECHAPREVISTA, :p_EFICACIA, :p_REALIZACION) " _
			 & "RETURNING ID INTO :p_id "
			'------------------------------------------------------------------------------
			'Parametros
			'------------------------------------------------------------------------------
			Dim Parametros As New List(Of OracleParameter)
			Parametros.Add(New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, Me.Descripcion, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_IDTIPOACCION", OracleDbType.Int32, Me.IdTipoAccion, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_FECHAINICIO", OracleDbType.Date, Me.FechaInicio, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_FECHAFIN", OracleDbType.Date, Me.FechaFin, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_FECHAPREVISTA", OracleDbType.Date, Me.FechaPrevista, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_EFICACIA", OracleDbType.NVarchar2, Me.Eficacia, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_REALIZACION", OracleDbType.Int32, Me.Realizacion, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.InputOutput))
			'------------------------------------------------------------------------------
			'------------------------------------------------------------------------------
			'Insertamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Me.Id = Parametros.Find(Function(o As OracleParameter) o.ParameterName = "p_id").Value.ToString
		End Sub
		''' <summary>
		''' Carga de Registro.
		''' </summary>
		''' <returns>List(Of Acciones_DAL)</returns>
		''' <remarks></remarks>
		Protected Function Load() As List(Of Acciones_DAL)
			Dim Funciones As New Funciones
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "SELECT ID, DESCRIPCION, IDTIPOACCION, FECHAINICIO, FECHAFIN, FECHAPREVISTA, EFICACIA, REALIZACION FROM ACCIONES"
			Dim sqlWHERE As String = String.Empty
			Dim sqlLIKE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			'------------------------------------------------------------------------------
			'Parametros de Busqueda.
			'------------------------------------------------------------------------------
			If Me.Id IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " ID = :p_ID"
				Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
			End If
			If Me.FechaPrevista IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " FECHAPREVISTA = :p_FECHAPREVISTA"
				Parametros.Add(New OracleParameter("p_FECHAPREVISTA", OracleDbType.Date, Me.FechaPrevista, ParameterDirection.Input))
			End If
			'------------------------------------------------------------------------------
			''------------------------------------------------------------------------------
			''Parametros de Busqueda LIKE.
			''------------------------------------------------------------------------------
			'If Me.Descripcion IsNot Nothing Then
			'	sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(DESCRIPCION, :p_DESCRIPCION, 'i')"
			'	Parametros.Add(New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, Funciones.TextoLike(Me.Descripcion), ParameterDirection.Input))
			'End If
			'If Me.Eficacia IsNot Nothing Then
			'	sqlLIKE &= If(sqlLIKE Is String.Empty, sqlLIKE, " OR") & " REGEXP_LIKE(EFICACIA, :p_EFICACIA, 'i')"
			'	Parametros.Add(New OracleParameter("p_EFICACIA", OracleDbType.NVarchar2, Funciones.TextoLike(Me.Eficacia), ParameterDirection.Input))
			'End If
			'If sqlLIKE IsNot String.Empty Then sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " (" & sqlLIKE & ")"
			''------------------------------------------------------------------------------

			'------------------------------------------------------------------------------
			'Cargamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			sSQL &= sqlWHERE
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Load = Memcached.OracleDirectAccess.Seleccionar(Of Acciones_DAL) _
			 (Function(ODR As OracleDataReader) New Acciones_DAL _
			 With { _
			   .Id = If(ODR("ID") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("ID"))) _
			 , .Descripcion = If(ODR("DESCRIPCION") Is DBNull.Value, Nothing, ODR("DESCRIPCION")) _
			 , .IdTipoAccion = If(ODR("IdTipoAccion") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IdTipoAccion"))) _
			 , .FechaInicio = If(ODR("FECHAINICIO") Is DBNull.Value, New Nullable(Of Date), CDate(ODR("FECHAINICIO"))) _
			 , .FechaFin = If(ODR("FECHAFIN") Is DBNull.Value, New Nullable(Of Date), CDate(ODR("FECHAFIN"))) _
			 , .FechaPrevista = If(ODR("FECHAPREVISTA") Is DBNull.Value, New Nullable(Of Date), CDate(ODR("FECHAPREVISTA"))) _
			 , .Eficacia = If(ODR("EFICACIA") Is DBNull.Value, Nothing, ODR("EFICACIA")) _
			 , .Realizacion = If(ODR("REALIZACION") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("REALIZACION"))) _
			 }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Return Load
		End Function
		Protected Sub Update()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "UPDATE ACCIONES SET DESCRIPCION = :p_DESCRIPCION,  IDTIPOACCION = :p_IDTIPOACCION" _
			   & " ,FECHAINICIO = :p_FECHAINICIO, FECHAFIN = :p_FECHAFIN, FECHAPREVISTA = :p_FECHAPREVISTA" _
			   & " ,EFICACIA = :p_EFICACIA, REALIZACION = :p_REALIZACION" _
			  & " WHERE ID = :p_ID"
			Dim Parametros As New List(Of OracleParameter)
			Try
				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Parametros.Add(New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, Me.Descripcion, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_IDTIPOACCION", OracleDbType.Int32, Me.IdTipoAccion, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_FECHAINICIO", OracleDbType.Date, Me.FechaInicio, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_FECHAFIN", OracleDbType.Date, Me.FechaFin, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_FECHAPREVISTA", OracleDbType.Date, Me.FechaPrevista, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_EFICACIA", OracleDbType.NVarchar2, Me.Eficacia, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_REALIZACION", OracleDbType.Int32, Me.Realizacion, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
	End Class
	Public Class Gertakariak_Acciones_DAL
		Inherits GertakariakLib2.Gertakariak_Acciones_ELL
		Protected Sub Delete()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "DELETE FROM GERTAKARIAK_ACCIONES"
			Dim sqlWHERE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			Try
				If Me.IdAccion Is Nothing Or Me.IdAccion Is Nothing Then Throw New ApplicationException("Falta los identificadores del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros de Busqueda.
				'------------------------------------------------------------------------------
				If Me.IdAccion IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDACCION = :p_IDACCION"
					Parametros.Add(New OracleParameter("p_IDESTRUCTURA", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
				End If
				If Me.IdIncidencia IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDINCIDENCIA = :p_IDINCIDENCIA"
					Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
				End If
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
                '------------------------------------------------------------------------------
                sSQL &= sqlWHERE
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
		Protected Sub Insert()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "INSERT INTO GERTAKARIAK_ACCIONES " _
			   & "(IDACCION, IDINCIDENCIA) " _
			   & "VALUES (:p_IDACCION, :p_IDINCIDENCIA) "
			Try
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Dim Parametros As New List(Of OracleParameter)
				Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Insertamos los datos especificos del objeto.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
		''' <summary>
		''' Carga de Registro.
		''' </summary>
		''' <returns>List(Of Gertakariak_Acciones_DAL)</returns>
		Protected Function Load() As List(Of Gertakariak_Acciones_DAL)
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "SELECT IDINCIDENCIA, IDACCION FROM GERTAKARIAK_ACCIONES"
			Dim sqlWHERE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			Try
				'------------------------------------------------------------------------------
				'Parametros de Busqueda.
				'------------------------------------------------------------------------------
				If Me.IdAccion IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDACCION = :p_IDACCION"
					Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
				End If
				If Me.IdIncidencia IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDINCIDENCIA = :p_IDINCIDENCIA"
					Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
				End If
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Cargamos los datos especificos del objeto.
				'------------------------------------------------------------------------------
				sSQL &= sqlWHERE
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Load = Memcached.OracleDirectAccess.Seleccionar(Of Gertakariak_Acciones_DAL) _
				 (Function(ODR As OracleDataReader) New Gertakariak_Acciones_DAL _
				 With { _
				  .IdAccion = If(ODR("IDACCION") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDACCION"))) _
				 , .IdIncidencia = If(ODR("IDINCIDENCIA") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDINCIDENCIA"))) _
				 }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
				Return Load
			Catch ex As Exception
				throw 
			End Try
		End Function
		Protected Sub Update()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "UPDATE GERTAKARIAK_ACCIONES SET IDINCIDENCIA = :p_IDINCIDENCIA,  IDACCION = :p_IDACCION"
			Dim sqlWHERE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			Try
				If Me.IdAccion Is Nothing Or Me.IdIncidencia Is Nothing Then Throw New ApplicationException("Falta los identificadores del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros de Busqueda.
				'------------------------------------------------------------------------------
				If Me.IdAccion IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDACCION = :p_IDACCION"
					Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
				End If
				If Me.IdIncidencia IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDINCIDENCIA = :p_IDINCIDENCIA"
					Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
				End If
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				sSQL &= sqlWHERE
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
	End Class
	Public Class Acciones_Usuarios_DAL
		Inherits GertakariakLib2.Acciones_Usuario_ELL

		Protected Sub Insert()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "INSERT INTO ACCIONES_USUARIOS" _
			 & " (IDACCION, IDUSUARIO)" _
			 & " VALUES (:p_IDACCION, :p_IDUSUARIO)"
			Try
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Dim Parametros As New List(Of OracleParameter)
				Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Insertamos los datos especificos del objeto.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
		''' <summary>
		''' Carga de Registro.
		''' </summary>
		''' <returns>List(Of Acciones_DAL)</returns>
		''' <remarks></remarks>
		Protected Function Load() As List(Of Acciones_Usuarios_DAL)
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "SELECT IDACCION, IDUSUARIO FROM ACCIONES_USUARIOS"
			Dim sqlWHERE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			Try
			'------------------------------------------------------------------------------
			'Parametros de Busqueda.
			'------------------------------------------------------------------------------
			If Me.IdAccion IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDACCION = :p_IDACCION"
				Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
			End If
			If Me.IdUsuario IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDUSUARIO = :p_IDUSUARIO"
				Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
			End If
			'------------------------------------------------------------------------------
			'------------------------------------------------------------------------------
			'Cargamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			sSQL &= sqlWHERE
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Load = Memcached.OracleDirectAccess.Seleccionar(Of Acciones_Usuarios_DAL) _
			 (Function(ODR As OracleDataReader) New Acciones_Usuarios_DAL _
			 With { _
			  .IdAccion = If(ODR("IDACCION") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDACCION"))) _
			, .IdUsuario = If(ODR("IDUSUARIO") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDUSUARIO"))) _
			 }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
				Return Load
			Catch ex As Exception
				throw 
			End Try
		End Function
		Protected Sub Delete()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "DELETE FROM ACCIONES_USUARIOS"
			Dim sqlWHERE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			Try
				If Me.IdAccion Is Nothing Or Me.IdAccion Is Nothing Then Throw New ApplicationException("Falta los identificadores del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros de Busqueda.
				'------------------------------------------------------------------------------
				If Me.IdAccion IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDACCION = :p_IDACCION"
					Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
				End If
				If Me.IdUsuario IsNot Nothing Then
					sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDUSUARIO = :p_IDUSUARIO"
					Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
				End If
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				sSQL &= sqlWHERE
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
    End Class
    Public Class Acciones_Ejecucion_DAL
        Inherits GertakariakLib2.Acciones_Ejecucion_ELL

        Protected Sub Insert()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "INSERT INTO ACCIONES_EJECUCION" _
             & " (IDACCION, IDUSUARIO)" _
             & " VALUES (:p_IDACCION, :p_IDUSUARIO)"
            Try
                '------------------------------------------------------------------------------
                'Parametros
                '------------------------------------------------------------------------------
                Dim Parametros As New List(Of OracleParameter)
                Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
                Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
                '------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                'Insertamos los datos especificos del objeto.
                '------------------------------------------------------------------------------
                If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
                Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
                If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
                '------------------------------------------------------------------------------
            Catch ex As Exception
                throw 
            End Try
        End Sub
        ''' <summary>
        ''' Carga de Registro.
        ''' </summary>
        ''' <returns>List(Of Acciones_DAL)</returns>
        ''' <remarks></remarks>
        Protected Function Load() As List(Of Acciones_Ejecucion_DAL)
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "SELECT IDACCION, IDUSUARIO FROM ACCIONES_EJECUCION"
            Dim sqlWHERE As String = String.Empty
            Dim Parametros As New List(Of OracleParameter)
            Try
                '------------------------------------------------------------------------------
                'Parametros de Busqueda.
                '------------------------------------------------------------------------------
                If Me.IdAccion IsNot Nothing Then
                    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDACCION = :p_IDACCION"
                    Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
                End If
                If Me.IdUsuario IsNot Nothing Then
                    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDUSUARIO = :p_IDUSUARIO"
                    Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
                End If
                '------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                'Cargamos los datos especificos del objeto.
                '------------------------------------------------------------------------------
                sSQL &= sqlWHERE
                If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
                Load = Memcached.OracleDirectAccess.Seleccionar(Of Acciones_Ejecucion_DAL) _
                 (Function(ODR As OracleDataReader) New Acciones_Ejecucion_DAL _
                 With { _
                  .IdAccion = If(ODR("IDACCION") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDACCION"))) _
                , .IdUsuario = If(ODR("IDUSUARIO") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDUSUARIO"))) _
                 }, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
                If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
                '------------------------------------------------------------------------------
                Return Load
            Catch ex As Exception
                throw 
            End Try
        End Function
        Protected Sub Delete()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "DELETE FROM ACCIONES_EJECUCION"
            Dim sqlWHERE As String = String.Empty
            Dim Parametros As New List(Of OracleParameter)
            Try
                If Me.IdAccion Is Nothing Or Me.IdAccion Is Nothing Then Throw New ApplicationException("Falta los identificadores del registro.", New ApplicationException)
                '------------------------------------------------------------------------------
                'Parametros de Busqueda.
                '------------------------------------------------------------------------------
                If Me.IdAccion IsNot Nothing Then
                    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDACCION = :p_IDACCION"
                    Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
                End If
                If Me.IdUsuario IsNot Nothing Then
                    sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDUSUARIO = :p_IDUSUARIO"
                    Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
                End If
                '------------------------------------------------------------------------------
                '------------------------------------------------------------------------------
                'Ejecutamos la Query.
                '------------------------------------------------------------------------------
                sSQL &= sqlWHERE
                If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
                Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
                If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
                '------------------------------------------------------------------------------
            Catch ex As Exception
                throw 
            End Try
        End Sub
    End Class
	Public Class Acciones_Observaciones_DAL
		Inherits GertakariakLib2.Acciones_Observaciones_ELL

		Protected Sub Delete()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "DELETE FROM ACCIONES_OBSERVACIONES WHERE ID=:p_id"
			Dim Parametros As New List(Of OracleParameter)
			Try

				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, Me.Id, ParameterDirection.Input)
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, p_id)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub

		Protected Sub Insert()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "INSERT INTO ACCIONES_OBSERVACIONES " _
			 & "(IDACCION, IDUSUARIO, FECHA, DESCRIPCION) " _
			 & "VALUES (:p_IDACCION, :p_IDUSUARIO, :p_FECHA, :p_DESCRIPCION) " _
			 & "RETURNING ID INTO :p_id "
			'------------------------------------------------------------------------------
			'Parametros
			'------------------------------------------------------------------------------
			Dim Parametros As New List(Of OracleParameter)

			Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_FECHA", OracleDbType.Date, Me.Fecha, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, Me.Descripcion, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.InputOutput))
			'------------------------------------------------------------------------------
			'------------------------------------------------------------------------------
			'Insertamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Me.Id = Parametros.Find(Function(o As OracleParameter) o.ParameterName = "p_id").Value.ToString
		End Sub

		''' <summary>
		''' Carga de Registro.
		''' </summary>
		''' <returns>List(Of Acciones_Observaciones_DAL)</returns>
		''' <remarks></remarks>
		Protected Function Load() As List(Of Acciones_Observaciones_DAL)
			Dim Funciones As New Funciones
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "SELECT ID, IDACCION, IDUSUARIO, FECHA, DESCRIPCION FROM ACCIONES_OBSERVACIONES"
			Dim sqlWHERE As String = String.Empty
			Dim sqlLIKE As String = String.Empty
			Dim Parametros As New List(Of OracleParameter)
			'------------------------------------------------------------------------------
			'Parametros de Busqueda.
			'------------------------------------------------------------------------------
			If Me.Id IsNot Nothing Then
				sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " ID = :p_ID"
				Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
            End If
            If Me.IdAccion IsNot Nothing Then
                sqlWHERE &= If(sqlWHERE Is String.Empty, " WHERE", " AND") & " IDACCION = :p_IDACCION"
                Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
            End If
			'------------------------------------------------------------------------------

			'------------------------------------------------------------------------------
			'Cargamos los datos especificos del objeto.
			'------------------------------------------------------------------------------
			sSQL &= sqlWHERE
			If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
			Load = Memcached.OracleDirectAccess.Seleccionar(Of Acciones_Observaciones_DAL) _
			 (Function(ODR As OracleDataReader) New Acciones_Observaciones_DAL _
			 With { _
			.Id = If(ODR("ID") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("ID"))) _
			, .IdAccion = If(ODR("IDACCION") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDACCION"))) _
			, .IdUsuario = If(ODR("IDUSUARIO") Is DBNull.Value, New Nullable(Of Integer), CInt(ODR("IDUSUARIO"))) _
			, .Fecha = If(ODR("FECHA") Is DBNull.Value, New Nullable(Of Date), CDate(ODR("FECHA"))) _
			, .Descripcion = If(ODR("DESCRIPCION") Is DBNull.Value, Nothing, ODR("DESCRIPCION")) _
			}, sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
			If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
			'------------------------------------------------------------------------------
			Return Load
		End Function

		Protected Sub Update()
			Dim CerrarConexion As Boolean = False
			Dim sSQL As String = "UPDATE ACCIONES_OBSERVACIONES SET IDACCION = :p_IDACCION,  IDUSUARIO = :p_IDUSUARIO" _
			 & " ,FECHA = :p_FECHA, DESCRIPCION = :p_DESCRIPCION" _
			 & " WHERE ID = :p_ID"
			Dim Parametros As New List(Of OracleParameter)
			Try
				If Me.Id Is Nothing Then Throw New ApplicationException("Falta el identificador unico del registro.", New ApplicationException)
				'------------------------------------------------------------------------------
				'Parametros
				'------------------------------------------------------------------------------
				Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_FECHA", OracleDbType.Date, Me.Fecha, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_DESCRIPCION", OracleDbType.NVarchar2, Me.Descripcion, ParameterDirection.Input))
				Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, Me.Id, ParameterDirection.Input))
				'------------------------------------------------------------------------------
				'------------------------------------------------------------------------------
				'Ejecutamos la Query.
				'------------------------------------------------------------------------------
				If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
				Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
				If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
				'------------------------------------------------------------------------------
			Catch ex As Exception
				throw 
			End Try
		End Sub
    End Class


    Public Class Actas_MS_DAL
        Inherits GertakariakLib2.Actas_MS_ELL

        Protected Sub Insert()
            Dim CerrarConexion As Boolean = False
            Dim sSQL As String = "INSERT INTO ACTAS_MS " _
             & "(FECHA, IDINCIDENCIA, IDACCION, IDOBSERVACION, NUEVO, IDUSUARIO) " _
             & "VALUES (:p_FECHA, :p_IDINCIDENCIA, :p_IDACCION, :p_IDOBSERVACION, :p_NUEVO, :p_IDUSUARIO) " _
             & "RETURNING ID INTO :p_id "
            '------------------------------------------------------------------------------
            'Parametros
            '------------------------------------------------------------------------------
            Dim Parametros As New List(Of OracleParameter)

            Parametros.Add(New OracleParameter("p_FECHA", OracleDbType.Date, Me.Fecha, ParameterDirection.Input))
            Parametros.Add(New OracleParameter("p_IDINCIDENCIA", OracleDbType.Int32, Me.IdIncidencia, ParameterDirection.Input))
            Parametros.Add(New OracleParameter("p_IDACCION", OracleDbType.Int32, Me.IdAccion, ParameterDirection.Input))
            Parametros.Add(New OracleParameter("p_IDOBSERVACION", OracleDbType.Int32, Me.IdObservacion, ParameterDirection.Input))
            Parametros.Add(New OracleParameter("p_NUEVO", OracleDbType.Int32, Me.Nuevo, ParameterDirection.Input))
            Parametros.Add(New OracleParameter("p_IDUSUARIO", OracleDbType.Int32, Me.IdUsuario, ParameterDirection.Input))
            Parametros.Add(New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.InputOutput))
            '------------------------------------------------------------------------------
            '------------------------------------------------------------------------------
            'Insertamos los datos especificos del objeto.
            '------------------------------------------------------------------------------
            If My.Settings.ConexionOracle.State = ConnectionState.Closed Then My.Settings.ConexionOracle.Open() : CerrarConexion = True
            Memcached.OracleDirectAccess.NoQuery(sSQL, My.Settings.ConexionOracle, Parametros.ToArray)
            If CerrarConexion = True Then My.Settings.ConexionOracle.Close()
            '------------------------------------------------------------------------------
            Me.Id = Parametros.Find(Function(o As OracleParameter) o.ParameterName = "p_id").Value.ToString
        End Sub
    End Class
End Namespace

''' <summary>
''' Automatizacion de transacciones para las 'Entidades'.
''' </summary>
''' <remarks></remarks>
Public Class Transaccion
	Dim _Transaccion As OracleTransaction

	Public ReadOnly Property Transaccion As OracleTransaction
		Get
			Return _Transaccion
		End Get
	End Property
	Public ReadOnly Property Estado As Nullable(Of ConnectionState)
		Get
			Return If(Transaccion Is Nothing, Nothing, Transaccion.Connection.State)
		End Get
	End Property
	''' <summary>
	''' Apertura de la Transaccion.
	''' </summary>
	Public Sub Abrir()
		My.Settings.ConexionOracle.Open()
		_Transaccion = My.Settings.ConexionOracle.BeginTransaction
	End Sub
	''' <summary>
	''' Ejecucion(Commit) y Cierre de la transaccion.
	''' En caso de producirse un error realiza automaticamente el RollBack.
	''' </summary>
	Public Sub Cerrar()
		Try
			_Transaccion.Commit()
			_Transaccion.Dispose()
			My.Settings.ConexionOracle.Close()
		Catch ex As Exception
			Rollback()
			throw 
		End Try
	End Sub
	''' <summary>
	''' Devolvemos a la base de datos su estado original.
	''' </summary>
	''' <remarks></remarks>
	Public Sub Rollback()
		_Transaccion.Rollback()
		_Transaccion.Dispose()
		My.Settings.ConexionOracle.Close()
	End Sub
End Class