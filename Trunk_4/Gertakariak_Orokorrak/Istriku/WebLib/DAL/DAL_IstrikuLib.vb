Imports Oracle.DataAccess.Client
Imports System.Reflection
Imports log4net


''' <summary>
''' 
''' </summary>
''' <remarks>DAL</remarks>
Public MustInherit Class gtkIstrikuDAL
	Dim Conexion As String
	Private Log As ILog = LogManager.GetLogger("Istriku")
	Sub New()
		'Try
		'    If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
		'        Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("ConexionWeb_LIVE").ConnectionString
		'    Else
		'        Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("ConexionWeb_TEST").ConnectionString
		'    End If
		'Catch ex As Exception
		'    Log.Error("Error al inicializar el connection string the Gertakariak.", ex)
		'End Try

		Try
			'Me.Conexion = "Data Source=batz;Persist Security Info=True;User ID=incidencias;Password=incidencias12;Unicode=True"
#If DEBUG Then
			Me.Conexion = "Data Source=GARAPEN;User Id=incidencias;Password=incidencias12;Connection LifeTime=300;"
#Else
            Me.Conexion = "Data Source=XBAT;User Id=incidencias;Password=incidencias12;Connection LifeTime=300;"
#End If

		Catch ex As Exception
			Log.Error("Error al inicializar el connection string the Istriku.", ex)
		End Try
	End Sub
	Protected Sub Insert(ByRef obj As IstrikuLib.gtkIstriku)
		Dim sSQL As String = "INSERT INTO GERTAKARIAK " _
		& "(IDTIPOINCIDENCIA, FECHAAPERTURA, DESCRIPCIONPROBLEMA, IDCREADOR, PROCEDENCIANC) " _
		& "VALUES (:p_IdTipoIncidencia, :p_FechaApertura, :p_Descripcion, :p_usrCreador, :p_ProcedenciaNC) " _
		& "RETURNING ID INTO :p_id "

		Dim ConexionBBDD As New OracleConnection(Me.Conexion)
		ConexionBBDD.Open()
		Dim Transaccion As OracleTransaction = ConexionBBDD.BeginTransaction()

		Try
			Dim p_IdTipoIncidencia As New OracleParameter("p_IdTipoIncidencia", OracleDbType.Int32, obj.IdTipoIncidencia, ParameterDirection.Input)
			Dim p_FechaApertura As New OracleParameter("p_FechaApertura", OracleDbType.Date, obj.FechaSuceso, ParameterDirection.Input)
			Dim p_Descripcion As New OracleParameter("p_Descripcion", OracleDbType.NVarchar2, obj.Descripcion, ParameterDirection.Input)
			Dim p_usrCreador As New OracleParameter("p_usrCreador", OracleDbType.Int32, obj.usrCreador.Id, ParameterDirection.Input)
			Dim p_ProcedenciaNC As New OracleParameter("p_ProcedenciaNC", OracleDbType.Int32, obj.ProcedenciaNC, ParameterDirection.Input)
			Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, ParameterDirection.InputOutput)

			'--------------------------------------------------------------------
			'El orden de los parametros debe ser igual que el de la consulta SQL.
			'--------------------------------------------------------------------
			Memcached.OracleDirectAccess.NoQuery(sSQL, ConexionBBDD, _
			p_IdTipoIncidencia, _
			p_FechaApertura, _
			p_Descripcion, _
			p_usrCreador, _
			p_ProcedenciaNC, _
			p_id)
			'--------------------------------------------------------------------

			obj.ID = p_id.Value.ToString

			'--------------------------------------------------------------------
			'Insertamos los afectados.
			'Usamos la tabla de "DETECCION" para insertar los afectados.
			'--------------------------------------------------------------------
			If obj.Afectados IsNot Nothing AndAlso obj.Afectados.Count > 0 Then
				'For Each Afectado As Sablib.ELL.Usuario In obj.Afectados
				For Each Afectado As gtkAfectado In obj.Afectados
					Dim p_IdUsuario As New OracleParameter("p_IdUsuario", OracleDbType.Int32, Afectado.Usuario.Id, ParameterDirection.Input)
					Dim p_id2 As New OracleParameter("p_id2", OracleDbType.Int32, obj.ID, ParameterDirection.Input)
					sSQL = "INSERT INTO DETECCION (IDINCIDENCIA, IDUSUARIO) VALUES (:p_id2, :p_IdUsuario)"
					Memcached.OracleDirectAccess.NoQuery(sSQL, ConexionBBDD, p_id2, p_IdUsuario)
				Next
			End If
			'--------------------------------------------------------------------

			Transaccion.Commit()
			ConexionBBDD.Close()
		Catch ex As Exception
			Transaccion.Rollback()
			ConexionBBDD.Close()
			throw 
		End Try
	End Sub
	Protected Function Load(ByVal Obj As gtkIstriku) As List(Of gtkIstriku)
		Try
			Dim Resultado As List(Of gtkIstriku) = Load()
			If Obj.ID.HasValue Then Resultado = Resultado.FindAll(Function(r As gtkIstriku) r.ID = Obj.ID)
			Return Resultado
		Catch ex As Exception
			Dim be As New BatzException(ex.Message.ToString, ex)
			Return Nothing
		End Try
	End Function
	Protected Function Load() As List(Of gtkIstriku)
		Dim objSuceso As New gtkIstriku
		Dim fUsuario As New SabLib.BLL.UsuariosComponent
		Dim sSQL As String = "SELECT ID, DESCRIPCIONPROBLEMA, IDCREADOR, FECHAAPERTURA, FECHACIERRE, PROCEDENCIANC FROM GERTAKARIAK " _
		  & "WHERE IDTIPOINCIDENCIA = :p_IdTipoIncidencia "
		Dim sSQL_Afectados As String = "SELECT IDUSUARIO FROM DETECCION WHERE IDINCIDENCIA = :p_Id"
		Dim p_IdTipoIncidencia As New OracleParameter("p_IdTipoIncidencia", OracleDbType.Int32, objSuceso.IdTipoIncidencia, ParameterDirection.Input)
		Load = Memcached.OracleDirectAccess.Seleccionar(Of gtkIstriku) _
		(Function(Incidencia As OracleDataReader) New gtkIstriku _
		  With { _
		  .ID = Incidencia("ID").ToString _
		  , .Descripcion = Incidencia("DESCRIPCIONPROBLEMA") _
		  , .FechaSuceso = Incidencia("FECHAAPERTURA") _
		  , .FechaCierre = IIf(Incidencia("FECHACIERRE") Is Nothing Or Incidencia("FECHACIERRE") Is DBNull.Value, Nothing, Incidencia("FECHACIERRE")) _
		  , .HoraTrabajo = CDate(Incidencia("FECHAAPERTURA")).Second _
		  , .usrCreador = fUsuario.GetUsuario(New SabLib.ELL.Usuario With {.Id = Incidencia("IDCREADOR")}, False) _
		  , .ProcedenciaNC = Incidencia("PROCEDENCIANC").ToString _
		  }, sSQL, Conexion, p_IdTipoIncidencia)
		'--------------------------------------------------------------------------------------
		Return Load
	End Function
	Protected Sub Save(ByRef obj As IstrikuLib.gtkIstriku)
		Dim sSQL As String = "UPDATE GERTAKARIAK SET " _
		& "FECHAAPERTURA = :p_FechaApertura " _
		& ", DESCRIPCIONPROBLEMA = :p_Descripcion " _
		& ", IDCREADOR = :p_usrCreador " _
		& ", PROCEDENCIANC = :p_ProcedenciaNC " _
		& "WHERE ID = :p_id"

		Dim ConexionBBDD As New OracleConnection(Me.Conexion)
		ConexionBBDD.Open()
		Dim Transaccion As OracleTransaction = ConexionBBDD.BeginTransaction()

		Try
			Dim p_FechaApertura As New OracleParameter("p_FechaApertura", OracleDbType.Date, obj.FechaSuceso, ParameterDirection.Input)
			Dim p_Descripcion As New OracleParameter("p_Descripcion", OracleDbType.NVarchar2, obj.Descripcion, ParameterDirection.Input)
			Dim p_usrCreador As New OracleParameter("p_usrCreador", OracleDbType.Int32, obj.usrCreador.Id, ParameterDirection.Input)
			Dim p_ProcedenciaNC As New OracleParameter("p_ProcedenciaNC", OracleDbType.Int32, obj.ProcedenciaNC, ParameterDirection.Input)
			Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, obj.ID, ParameterDirection.Input)

			Memcached.OracleDirectAccess.NoQuery(sSQL, ConexionBBDD, p_FechaApertura, p_Descripcion, p_usrCreador, p_ProcedenciaNC, p_id)

			'--------------------------------------------------------------------
			'Afectados.
			'--------------------------------------------------------------------
			Dim sSQL_Afectados As String = "SELECT ID, IDUSUARIO FROM DETECCION WHERE IDINCIDENCIA = :p_id2"
			Dim p_id2 As New OracleParameter("p_id2", OracleDbType.Int32, obj.ID, ParameterDirection.Input)
			Dim AfectadosBD As Dictionary(Of String, String) = Memcached.OracleDirectAccess.SeleccionarDiccionario(sSQL_Afectados, Me.Conexion, p_id2)

			'Comprobamos que los Afectados de la BB.DD no existen en el Objeto para eliminarlos de la BB.DD.
			For Each Item In AfectadosBD
				Dim Afectado As KeyValuePair(Of String, String) = Item
				'If Not obj.Afectados.Exists(Function(o As Sablib.ELL.Usuario) o.Id = Afectado.Value) Then
				If Not obj.Afectados.Exists(Function(o As gtkAfectado) o.Usuario.Id = Afectado.Value) Then
					'Borrar Afectado de la BB.DD
					Dim sSQL_Deteccion As String = "DELETE FROM DETECCION WHERE ID=:p_id3"
					Dim p_id3 As New OracleParameter("p_id3", OracleDbType.Int32, Afectado.Key, ParameterDirection.Input)
					Memcached.OracleDirectAccess.NoQuery(sSQL_Deteccion, ConexionBBDD, p_id3)
				End If
			Next

			'Comprobamos que los Afectados del Objeto NO existen en la BB.DD. para insertarlos.
			If obj.Afectados IsNot Nothing AndAlso obj.Afectados.Count > 0 Then
				'For Each Usuario As Sablib.ELL.Usuario In obj.Afectados
				For Each Afectado As gtkAfectado In obj.Afectados
					If Not AfectadosBD.ContainsValue(Afectado.Usuario.Id) Then
						'----------------------------------------------------------------------------------------------
						Dim p_id3 As New OracleParameter("p_id3", OracleDbType.Int32, obj.ID, ParameterDirection.Input)
						Dim p_IdUsuario As New OracleParameter("p_IdUsuario", OracleDbType.Int32, Afectado.Usuario.Id, ParameterDirection.Input)
						Dim p_idDepartamento As New OracleParameter("p_idDepartamento", OracleDbType.Int32, Afectado.EstadoParte, ParameterDirection.Input)
						sSQL = "INSERT INTO DETECCION (IDINCIDENCIA, IDUSUARIO, IDDEPARTAMENTO) VALUES (:p_id3, :p_IdUsuario, :p_idDepartamento)"
						Memcached.OracleDirectAccess.NoQuery(sSQL, ConexionBBDD, p_id3, p_IdUsuario, p_idDepartamento)
						'----------------------------------------------------------------------------------------------
						'FROGA: La parte de arriba funciona. Intentar usar el objeto gtkAfectado para hacer la modificacion.
						'----------------------------------------------------------------------------------------------

						'----------------------------------------------------------------------------------------------
					End If
				Next
			End If
			'--------------------------------------------------------------------
			Transaccion.Commit()
			ConexionBBDD.Close()
		Catch ex As Exception
			Transaccion.Rollback()
			ConexionBBDD.Close()
			throw 
		End Try
	End Sub
	Protected Sub Delete(ByRef obj As IstrikuLib.gtkIstriku)
		Dim ConexionBBDD As New OracleConnection(Me.Conexion)
		ConexionBBDD.Open()

		Try
			Dim sSQL As String = "DELETE FROM GERTAKARIAK WHERE ID=:p_id"
			Dim p_id As New OracleParameter("p_id", OracleDbType.Int32, obj.ID, ParameterDirection.Input)
			Memcached.OracleDirectAccess.NoQuery(sSQL, ConexionBBDD, p_id)
			ConexionBBDD.Close()
		Catch ex As Exception
			ConexionBBDD.Close()
			throw 
		End Try
	End Sub
End Class
Public MustInherit Class gtkAfectadoDAL
	Inherits gtkAfectadoELL
	Dim Conexion As String
	Private Log As ILog = LogManager.GetLogger("Istriku")
	Sub New()
		'Try
		'	If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
		'		Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("GERTAKARIAKLIVE").ConnectionString
		'	Else
		'		Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("GERTAKARIAKTEST").ConnectionString
		'	End If
		'Catch ex As Exception
		'	Log.Error("Error al inicializar el connection string the Gertakariak.", ex)
		'End Try

		Try
			'Me.Conexion = "Data Source=batz;Persist Security Info=True;User ID=incidencias;Password=incidencias12;Unicode=True"
			Me.Conexion = "Data Source=GARAPEN;User Id=incidencias;Password=incidencias12;Connection LifeTime=300;"
		Catch ex As Exception
			Log.Error("Error al inicializar el connection string the Istriku.", ex)
		End Try
	End Sub
	''' <summary>
	''' Carga de Afectados.
	''' </summary>
	''' <returns>List(Of gtkAfectado)</returns>
	''' <remarks></remarks>
	Protected Function Load() As List(Of gtkAfectado2)
		Dim sSQL As String = "SELECT ID, IDUSUARIO, IDINCIDENCIA, IDDEPARTAMENTO FROM DETECCION"
		Dim sqlWHERE As String = String.Empty
		Dim Parametros As New List(Of OracleParameter)
		'------------------------------------------------------------------------------
		'Parametros de Busqueda.
		'------------------------------------------------------------------------------
		If IdAfectadoSuceso IsNot Nothing Then
			sqlWHERE &= " WHERE ID = :p_ID"
			Parametros.Add(New OracleParameter("p_ID", OracleDbType.Int32, IdAfectadoSuceso, ParameterDirection.Input))
		End If
		If IdSuceso IsNot Nothing Then
			sqlWHERE &= " WHERE IDINCIDENCIA = :p_IdIncidencia"
			Parametros.Add(New OracleParameter("p_IdIncidencia", OracleDbType.Int32, IdSuceso, ParameterDirection.Input))
		End If
		'------------------------------------------------------------------------------
		'------------------------------------------------------------------------------
		'Cargamos los datos especificos del objeto (gtkAfectado).
		'------------------------------------------------------------------------------
		sSQL &= sqlWHERE

		Load = Memcached.OracleDirectAccess.Seleccionar(Of gtkAfectado2) _
		   (Function(Afectado As OracleDataReader) New gtkAfectado2 _
		   With { _
		  .IdAfectadoSuceso = IIf(Afectado("ID") IsNot Nothing, CInt(Afectado("ID")), Nothing) _
		   , .IdSuceso = IIf(Afectado("IDINCIDENCIA") IsNot Nothing, CInt(Afectado("IDINCIDENCIA")), Nothing) _
		   , .Id = Afectado("IDUSUARIO") _
		   , .EstadoParte = IIf((Afectado("IDDEPARTAMENTO") IsNot Nothing And Afectado("IDDEPARTAMENTO") IsNot DBNull.Value AndAlso [Enum].IsDefined(GetType(EstadoParte), CInt(Afectado("IDDEPARTAMENTO")))), Afectado("IDDEPARTAMENTO"), Nothing) _
		 }, sSQL, Conexion, Parametros.ToArray)
		'------------------------------------------------------------------------------

		If Load IsNot Nothing AndAlso Load.Count > 0 Then
			Dim sablibComp As New SabLib.BLL.UsuariosComponent
			For Each Afectado As gtkAfectado2 In Load
				Dim UsuarioSAB As SabLib.ELL.Usuario = sablibComp.GetUsuario(New SabLib.ELL.Usuario With {.Id = Afectado.Id}, False)
				Dim objAfectado As New gtkAfectado2
				'------------------------------------------------------------------------------
				'Cargamos los datos de los campos coincidentes 
				'del objeto original (Sablib.ELL.Usuario) en nuestro objeto (gtkAfectado).
				'------------------------------------------------------------------------------
				If UsuarioSAB IsNot Nothing Then
					For Each Propiedad As PropertyInfo In Afectado.GetType.GetProperties
						If Propiedad.GetSetMethod IsNot Nothing Then
							For Each rPropiedad As PropertyInfo In UsuarioSAB.GetType.GetProperties
								If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
									Propiedad.SetValue(Afectado, rPropiedad.GetValue(UsuarioSAB, Nothing), Nothing)
								End If
							Next
						End If
					Next
				End If
				'------------------------------------------------------------------------------
			Next
		End If
		Return Load
	End Function
	Protected Sub Save()
		Dim sSQL As String = "UPDATE DETECCION SET IDDEPARTAMENTO = :p_EstadoParte " _
		 & "WHERE ID = :p_idAfectadoSuceso"
		Dim Parametros As New List(Of OracleParameter)
		Dim ConexionBBDD As New OracleConnection(Me.Conexion)
		ConexionBBDD.Open()
		Dim Transaccion As OracleTransaction = ConexionBBDD.BeginTransaction()
		Try
			If Me.IdAfectadoSuceso Is Nothing Then Throw New BatzException("Falta el identificador unico del registro.", New ApplicationException)
			'------------------------------------------------------------------------------
			'Parametros
			'------------------------------------------------------------------------------
			Parametros.Add(New OracleParameter("p_EstadoParte", OracleDbType.Int32, Me.EstadoParte, ParameterDirection.Input))
			Parametros.Add(New OracleParameter("p_idAfectadoSuceso", OracleDbType.Int32, Me.IdAfectadoSuceso, ParameterDirection.Input))
			'------------------------------------------------------------------------------
			Memcached.OracleDirectAccess.NoQuery(sSQL, ConexionBBDD, Parametros.ToArray)
			Transaccion.Commit()
			ConexionBBDD.Close()
		Catch ex As Exception
			Transaccion.Rollback()
			ConexionBBDD.Close()
			throw 
		End Try
	End Sub
End Class