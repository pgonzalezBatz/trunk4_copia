Imports SABLib_Z.BLL.Interface
Imports AccesoAutomaticoBD
Imports log4net
Imports System.Runtime.CompilerServices
Imports System.Text.RegularExpressions
Imports System.DirectoryServices

<Assembly: InternalsVisibleTo("Sab")> 

Namespace BLL
	Public Class UsuariosComponent
		Implements IUsuariosComponent

		Private log As ILog = LogManager.GetLogger("root.SAB")

#Region "Consultas"

		''' <summary>
		''' Devuelve el usuario que tenga idUsuario como id vigente
		''' </summary>
		''' <param name="idUsuario"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		<Obsolete("Usar la funcion GetUsuario(oUser as ELL.Usuario) informando los campos que se requieran")> _
		Public Function GetUsuario(ByVal idUsuario As Integer, Optional ByVal obtenerFoto As Boolean = False) As ELL.Usuario Implements IUsuariosComponent.GetUsuario
			Dim usuariosDAL As New DAL.USUARIOS
			Dim oUser As ELL.Usuario = Nothing
			usuariosDAL.LoadByPrimaryKey(idUsuario)

			If usuariosDAL.RowCount = 1 Then
				oUser = getObject(usuariosDAL, obtenerFoto)
			End If

			Return oUser
		End Function

		''' <summary>
		''' Devuelve el usuario que cumpla las condiciones del objeto
		''' </summary>
		''' <param name="oUser"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetUsuario(ByVal oUser As ELL.Usuario, Optional ByVal Vigentes As Boolean = True, Optional ByVal obtenerFoto As Boolean = False) As ELL.Usuario Implements IUsuariosComponent.GetUsuario
			Dim usuariosDAL As New DAL.USUARIOS
			Dim oUserResul As ELL.Usuario = Nothing
			If (oUser.Id <> Integer.MinValue) Then usuariosDAL.Where.ID.Value = oUser.Id
			If (oUser.CodPersona <> Integer.MinValue) Then usuariosDAL.Where.CODPERSONA.Value = oUser.CodPersona
			If (oUser.NombreUsuario <> String.Empty) Then usuariosDAL.Where.NOMBREUSUARIO.Value = oUser.NombreUsuario
			If (oUser.Email <> String.Empty) Then usuariosDAL.Where.EMAIL.Value = oUser.Email
			If (oUser.IdDirectorioActivo <> String.Empty) Then usuariosDAL.Where.IDDIRECTORIOACTIVO.Value = oUser.IdDirectorioActivo
			If (oUser.Dni <> String.Empty) Then usuariosDAL.Where.DNI.Value = oUser.Dni

			'03/09/2009 Yuste: Adaptamos la funcion para filtrar entre vigentes o todos
			If (Vigentes) Then
				usuariosDAL.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
				usuariosDAL.Query.OpenParenthesis()
				usuariosDAL.Where.FECHABAJA.Value = DateTime.Now
				usuariosDAL.Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
				Dim wp As AccesoAutomaticoBD.WhereParameter = usuariosDAL.Where.TearOff.FECHABAJA
				wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
				wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
				usuariosDAL.Query.CloseParenthesis()
			End If

			usuariosDAL.Query.Load()
			If usuariosDAL.RowCount = 1 Then
				oUserResul = getObject(usuariosDAL, obtenerFoto)
			End If

			Return oUserResul
		End Function

		''' <summary>
		''' Obtiene los usuarios que cumplan las condiciones del objeto ordenados por un campo
		''' </summary>
		''' <param name="oUser">Objeto usuario</param>
		''' <param name="vigentes">Parametro opcional para mostrar todos los usuarios o solo vigentes</param>
		''' <param name="sortField">Campo a ordenar</param>
		''' <returns>Lista de usuarios</returns> 
		Public Function GetUsuarios(ByVal oUser As ELL.Usuario, Optional ByVal vigentes As Boolean = True, Optional ByVal sortField As String = DAL.USUARIOS.ColumnNames.NOMBREUSUARIO) As List(Of ELL.Usuario) Implements IUsuariosComponent.GetUsuarios
			Dim wUsuarios As New DAL.USUARIOS
			Dim listUsuarios As New List(Of ELL.Usuario)
			Dim bAnd As Boolean = False

			If (oUser.Id <> Integer.MinValue) Then
				wUsuarios.Where.ID.Value = oUser.Id
				bAnd = True
			End If

			If (oUser.IdDepartamento <> String.Empty) Then
				wUsuarios.Where.IDDEPARTAMENTO.Value = oUser.IdDepartamento
				bAnd = True
			End If

			If (oUser.IdEmpresa <> Integer.MinValue) Then
				wUsuarios.Where.IDEMPRESAS.Value = oUser.IdEmpresa
				bAnd = True
			End If

			'If (oUser.CodPersona <> Integer.MinValue) Then
			'    wUsuarios.Where.CODPERSONA.Value = oUser.CodPersona
			'    bAnd = True
			'    'Else
			'    'wUsuarios.Where.CODPERSONA.Operator = WhereParameter.Operand.IsNotNull
			'    'bAnd = True
			'End If

			'If (oUser.IdDirectorioActivo <> String.Empty) Then
			'    wUsuarios.Where.IDDIRECTORIOACTIVO.Value = oUser.IdDirectorioActivo
			'    bAnd = True
			'Else
			'    wUsuarios.Where.IDDIRECTORIOACTIVO.Operator = WhereParameter.Operand.IsNotNull
			'    bAnd = True
			'End If

			If (oUser.Email <> String.Empty) Then
				wUsuarios.Where.EMAIL.Value = oUser.Email.Trim
				bAnd = True
			End If

			If (oUser.NombreUsuario <> String.Empty) Then
				wUsuarios.Where.NOMBREUSUARIO.Value = oUser.NombreUsuario.Trim
				bAnd = True
			End If

			If (oUser.Nombre <> String.Empty) Then
				wUsuarios.Where.NOMBRE.Value = "%" & oUser.Nombre.Trim & "%"
				wUsuarios.Where.NOMBRE.Operator = WhereParameter.Operand.Like_
				bAnd = True
			End If

			If (oUser.Apellido1 <> String.Empty) Then
				wUsuarios.Where.APELLIDO1.Value = "%" & oUser.Apellido1.Trim & "%"
				wUsuarios.Where.APELLIDO1.Operator = WhereParameter.Operand.Like_
				bAnd = True
			End If

			If (oUser.Apellido2 <> String.Empty) Then
				wUsuarios.Where.APELLIDO2.Value = "%" & oUser.Apellido2.Trim & "%"
				wUsuarios.Where.APELLIDO2.Operator = WhereParameter.Operand.Like_
				bAnd = True
			End If

			If (bAnd) Then wUsuarios.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
			wUsuarios.Query.OpenParenthesis()
			wUsuarios.Where.CODPERSONA.Operator = WhereParameter.Operand.IsNotNull
			Dim wp1 As AccesoAutomaticoBD.WhereParameter = wUsuarios.Where.IDDIRECTORIOACTIVO
			wp1.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
			wp1.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNotNull
			wUsuarios.Query.CloseParenthesis()
			bAnd = True

			If (vigentes) Then
				If (bAnd) Then wUsuarios.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
				wUsuarios.Query.OpenParenthesis()
				wUsuarios.Where.FECHABAJA.Value = DateTime.Now
				wUsuarios.Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
				Dim wp As AccesoAutomaticoBD.WhereParameter = wUsuarios.Where.TearOff.FECHABAJA
				wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
				wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
				wUsuarios.Query.CloseParenthesis()
			End If
			wUsuarios.Query.AddOrderBy(sortField, AccesoAutomaticoBD.WhereParameter.Dir.ASC)

			wUsuarios.Query.Load()

			If (wUsuarios.RowCount > 0) Then
				Do
					listUsuarios.Add(getObject(wUsuarios, False))
				Loop While wUsuarios.MoveNext()
			End If

			Return listUsuarios
		End Function


		''' <summary>
		''' Obtiene los usuarios activos, que pertenezcan a alguna planta de las de la lista
		''' </summary>
		''' <param name="lPlantas">Plantas a las que debe pertenecer un usuario</param>
		''' <returns>Lista de usuarios</returns>        
		Public Function GetUsuarios(ByVal lPlantas As List(Of Integer)) As List(Of ELL.Usuario) Implements IUsuariosComponent.GetUsuarios
			Dim reader As IDataReader = Nothing
			Try
				Dim usersDAL As New DAL.USUARIOS
				Dim lUsuarios As New List(Of ELL.Usuario)
				Dim oUser As ELL.Usuario
				reader = usersDAL.GetUsuariosConPlanta(lPlantas)
				While reader.Read
					oUser = New ELL.Usuario
					oUser.Id = CInt(reader(DAL.USUARIOS.ColumnNames.ID))
					If Not reader.IsDBNull(3) Then oUser.NombreUsuario = reader(DAL.USUARIOS.ColumnNames.NOMBREUSUARIO)
					oUser.IdEmpresa = CInt(reader(DAL.USUARIOS.ColumnNames.IDEMPRESAS))
					oUser.Cultura = reader(DAL.USUARIOS.ColumnNames.IDCULTURAS)
					If Not reader.IsDBNull(4) Then oUser.IdDirectorioActivo = reader(DAL.USUARIOS.ColumnNames.IDDIRECTORIOACTIVO)
					If Not reader.IsDBNull(5) Then oUser.CodPersona = CInt(reader(DAL.USUARIOS.ColumnNames.CODPERSONA))
					If Not reader.IsDBNull(6) Then oUser.PWD = reader(DAL.USUARIOS.ColumnNames.PWD)
					If Not reader.IsDBNull(7) Then oUser.FechaAlta = CType(reader(DAL.USUARIOS.ColumnNames.FECHAALTA), Date)
					If Not reader.IsDBNull(8) Then oUser.FechaBaja = CType(reader(DAL.USUARIOS.ColumnNames.FECHABAJA), Date)
					If Not reader.IsDBNull(9) Then oUser.Apellido1 = reader(DAL.USUARIOS.ColumnNames.APELLIDO1)
					If Not reader.IsDBNull(10) Then oUser.Apellido2 = reader(DAL.USUARIOS.ColumnNames.APELLIDO2)
					If Not reader.IsDBNull(11) Then oUser.IdMatrix = reader(DAL.USUARIOS.ColumnNames.IDMATRIX)
					If Not reader.IsDBNull(12) Then oUser.IdFTP = reader(DAL.USUARIOS.ColumnNames.IDFTP)
					If Not reader.IsDBNull(13) Then oUser.Email = reader(DAL.USUARIOS.ColumnNames.EMAIL)
					If Not reader.IsDBNull(14) Then oUser.IdDepartamento = reader(DAL.USUARIOS.ColumnNames.IDDEPARTAMENTO)
					If Not reader.IsDBNull(15) Then oUser.Nombre = reader(DAL.USUARIOS.ColumnNames.NOMBRE)
					If Not reader.IsDBNull(16) Then oUser.Dni = reader(DAL.USUARIOS.ColumnNames.DNI)

					lUsuarios.Add(oUser)
				End While

				Return lUsuarios
			Catch
				Return Nothing
			Finally
				If (reader IsNot Nothing) Then reader.Close()
			End Try
		End Function


		''' <summary>
		''' Obtiene los usuarios activos que cumplan las condiciones del objeto y que pertenezcan a alguna planta de las de la lista
		''' </summary>
		''' <param name="oUser">Objeto usuario con las condiciones</param>
		''' <returns>Lista de usuarios</returns>        
		Public Function GetUsuariosPlanta(ByVal oUser As SABLib_Z.ELL.Usuario) As List(Of ELL.Usuario) Implements IUsuariosComponent.GetUsuariosPlanta
			Dim reader As IDataReader = Nothing
			Try
				Dim usersDAL As New DAL.USUARIOS
				Dim lUsuarios As New List(Of ELL.Usuario)
				reader = usersDAL.GetUsuariosConPlanta2(oUser)
				While reader.Read
					oUser = New ELL.Usuario
					oUser.Id = CInt(reader(DAL.USUARIOS.ColumnNames.ID))
					If Not reader.IsDBNull(3) Then oUser.NombreUsuario = reader(DAL.USUARIOS.ColumnNames.NOMBREUSUARIO)
					oUser.IdEmpresa = CInt(reader(DAL.USUARIOS.ColumnNames.IDEMPRESAS))
					oUser.Cultura = reader(DAL.USUARIOS.ColumnNames.IDCULTURAS)
					If Not reader.IsDBNull(4) Then oUser.IdDirectorioActivo = reader(DAL.USUARIOS.ColumnNames.IDDIRECTORIOACTIVO)
					If Not reader.IsDBNull(5) Then oUser.CodPersona = CInt(reader(DAL.USUARIOS.ColumnNames.CODPERSONA))
					If Not reader.IsDBNull(6) Then oUser.PWD = reader(DAL.USUARIOS.ColumnNames.PWD)
					If Not reader.IsDBNull(7) Then oUser.FechaAlta = CType(reader(DAL.USUARIOS.ColumnNames.FECHAALTA), Date)
					If Not reader.IsDBNull(8) Then oUser.FechaBaja = CType(reader(DAL.USUARIOS.ColumnNames.FECHABAJA), Date)
					If Not reader.IsDBNull(9) Then oUser.Apellido1 = reader(DAL.USUARIOS.ColumnNames.APELLIDO1)
					If Not reader.IsDBNull(10) Then oUser.Apellido2 = reader(DAL.USUARIOS.ColumnNames.APELLIDO2)
					If Not reader.IsDBNull(11) Then oUser.IdMatrix = reader(DAL.USUARIOS.ColumnNames.IDMATRIX)
					If Not reader.IsDBNull(12) Then oUser.IdFTP = reader(DAL.USUARIOS.ColumnNames.IDFTP)
					If Not reader.IsDBNull(13) Then oUser.Email = reader(DAL.USUARIOS.ColumnNames.EMAIL)
					If Not reader.IsDBNull(14) Then oUser.IdDepartamento = reader(DAL.USUARIOS.ColumnNames.IDDEPARTAMENTO)
					If Not reader.IsDBNull(15) Then oUser.Nombre = reader(DAL.USUARIOS.ColumnNames.NOMBRE)
					If Not reader.IsDBNull(16) Then oUser.Dni = reader(DAL.USUARIOS.ColumnNames.DNI)

					lUsuarios.Add(oUser)
				End While

				Return lUsuarios
			Catch
				Return Nothing
			Finally
				If (reader IsNot Nothing) Then reader.Close()
			End Try
		End Function

		''' <summary>
		''' Realiza una busqueda de usuarios a partir de un texto en la aplicacion de SAB con un algoritmo mas eficaz      
		''' </summary>
		''' <param name="texto">Texto a buscar</param>
		''' <returns>Lista de usuarios</returns>
		''' <remarks></remarks>        
		Friend Function GetUsuariosBusquedaSAB(ByVal texto As String) As List(Of ELL.Usuario) Implements IUsuariosComponent.GetUsuariosBusquedaSAB
			Dim reader As IDataReader = Nothing
			Try
				Dim usersDAL As New DAL.USUARIOS
				Dim lUsuarios As New List(Of ELL.Usuario)
				Dim oUser As ELL.Usuario
				If (texto <> String.Empty And Not IsNumeric(texto)) Then
					reader = usersDAL.GetUsuariosByNombre(texto)
					While reader.Read
						oUser = New ELL.Usuario
						oUser.Id = CInt(reader(DAL.USUARIOS.ColumnNames.ID))
						If Not reader.IsDBNull(3) Then oUser.NombreUsuario = reader(DAL.USUARIOS.ColumnNames.NOMBREUSUARIO)
						oUser.IdEmpresa = CInt(reader(DAL.USUARIOS.ColumnNames.IDEMPRESAS))
						oUser.Cultura = reader(DAL.USUARIOS.ColumnNames.IDCULTURAS)
						If Not reader.IsDBNull(4) Then oUser.IdDirectorioActivo = reader(DAL.USUARIOS.ColumnNames.IDDIRECTORIOACTIVO)
						If Not reader.IsDBNull(5) Then oUser.CodPersona = CInt(reader(DAL.USUARIOS.ColumnNames.CODPERSONA))
						If Not reader.IsDBNull(6) Then oUser.PWD = reader(DAL.USUARIOS.ColumnNames.PWD)
						If Not reader.IsDBNull(7) Then oUser.FechaAlta = CType(reader(DAL.USUARIOS.ColumnNames.FECHAALTA), Date)
						If Not reader.IsDBNull(8) Then oUser.FechaBaja = CType(reader(DAL.USUARIOS.ColumnNames.FECHABAJA), Date)
						If Not reader.IsDBNull(9) Then oUser.Apellido1 = reader(DAL.USUARIOS.ColumnNames.APELLIDO1)
						If Not reader.IsDBNull(10) Then oUser.Apellido2 = reader(DAL.USUARIOS.ColumnNames.APELLIDO2)
						If Not reader.IsDBNull(11) Then oUser.IdMatrix = reader(DAL.USUARIOS.ColumnNames.IDMATRIX)
						If Not reader.IsDBNull(12) Then oUser.IdFTP = reader(DAL.USUARIOS.ColumnNames.IDFTP)
						If Not reader.IsDBNull(13) Then oUser.Email = reader(DAL.USUARIOS.ColumnNames.EMAIL)
						If Not reader.IsDBNull(14) Then oUser.IdDepartamento = reader(DAL.USUARIOS.ColumnNames.IDDEPARTAMENTO)
						If Not reader.IsDBNull(15) Then oUser.Nombre = reader(DAL.USUARIOS.ColumnNames.NOMBRE)
						If Not reader.IsDBNull(16) Then oUser.Dni = reader(DAL.USUARIOS.ColumnNames.DNI)

						lUsuarios.Add(oUser)
					End While
				Else
					oUser = New ELL.Usuario
					lUsuarios = GetUsuarios(oUser)

					If (IsNumeric(texto)) Then
						lUsuarios = lUsuarios.FindAll(Function(oUsuario As ELL.Usuario) oUsuario.Id = CInt(texto) Or oUsuario.CodPersona = CInt(texto))
					End If
				End If

				Return lUsuarios
			Catch
				Return Nothing
			Finally
				If (reader IsNot Nothing) Then reader.Close()
			End Try


		End Function


		''' <summary>
		''' Realiza una busqueda de usuarios a partir de un texto en la aplicacion de SAB.Si no se indica nada en el texto, no obtiene ninguno
		''' </summary>
		''' <param name="texto">Texto a buscar</param>
		''' <param name="recurso">Parametro opcional indicando el recurso.Si se indica el recurso, la lista de usuarios que se obtiene para la busqueda, sera la de usuarios que pertenecen a ese recurso</param>
		''' <param name="bGetTodosSiTextoVacio">Parametro opcional para indicar que si el texto que se manda es vacio, obtenga todos los usuarios encontrados</param>
		''' <returns>Lista de usuarios</returns>      
		Public Function GetUsuariosBusquedaSAB2(ByVal texto As String, Optional ByVal recurso As Integer = Integer.MinValue, Optional ByVal bGetTodosSiTextoVacio As Boolean = False) As List(Of ELL.Usuario) Implements IUsuariosComponent.GetUsuariosBusquedaSAB2
			Dim usersDAL As New DAL.USUARIOS
			Dim lUsuarios, lUserResul, lUserResul2 As List(Of ELL.Usuario)
			lUserResul2 = Nothing : lUsuarios = Nothing
			Dim pattern, pattern2, pattern3, pattern4 As String
			Dim oUser As New ELL.Usuario
			oUser = New ELL.Usuario
			lUserResul = Nothing
			Dim strSplit As String() = texto.Split(" ")

			If (recurso = Integer.MinValue) Then
				'Se obtienen todos los usuarios
				lUsuarios = GetUsuarios(oUser, False)
			Else
				'Se obtienen los usuarios de ese recurso
				lUsuarios = GetUsuariosConRecurso(recurso)
			End If

			If (texto <> String.Empty And Not IsNumeric(texto)) Then
				If (strSplit.Length = 1 And strSplit(0).Trim <> String.Empty) Then
					pattern = "[a-z]*" & strSplit(0).Trim & "[a-z]*"
					lUserResul = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) Or _
					   Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) Or _
					   Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) Or _
					   Regex.IsMatch(oUsu.NombreUsuario, pattern, RegexOptions.IgnoreCase))


				ElseIf (strSplit.Length = 2) Then
					pattern = "[a-z]*" & strSplit(0).Trim & "[a-z]*"
					pattern2 = "[a-z]*" & strSplit(1).Trim & "[a-z]*"

					'Se buscan los coincidentes en los dos campos
					lUserResul = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase)))


					If (lUserResul Is Nothing OrElse lUserResul.Count = 0) Then
						'Se buscan los coincidentes en algun campo
						lUserResul2 = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
						   Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern2, RegexOptions.IgnoreCase))
					End If

					'Formamos una unica lista con los mas coincidentes arriba y el resto debajo.
					If (lUserResul.Count = 0 And (lUserResul2 IsNot Nothing AndAlso lUserResul2.Count > 0)) Then
						lUserResul = lUserResul2
					ElseIf (lUserResul.Count > 0 And (lUserResul2 IsNot Nothing AndAlso lUserResul2.Count > 0)) Then
						lUserResul.AddRange(lUserResul2)
					End If

				ElseIf (strSplit.Length = 3) Then
					pattern = "[a-z]*" & strSplit(0).Trim & "[a-z]*"
					pattern2 = "[a-z]*" & strSplit(1).Trim & "[a-z]*"
					pattern3 = "[a-z]*" & strSplit(2).Trim & "[a-z]*"

					'Se buscan los coincidentes en los dos campos
					lUserResul = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)))

					If (lUserResul Is Nothing OrElse lUserResul.Count = 0) Then
						'Se buscan los coincidentes en algun campo
						lUserResul2 = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
						   Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Nombre, pattern3, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern3, RegexOptions.IgnoreCase))
					End If

					'Formamos una unica lista con los mas coincidentes arriba y el resto debajo.
					If (lUserResul.Count = 0 And (lUserResul2 IsNot Nothing AndAlso lUserResul2.Count > 0)) Then
						lUserResul = lUserResul2
					ElseIf (lUserResul.Count > 0 And (lUserResul2 IsNot Nothing AndAlso lUserResul2.Count > 0)) Then
						lUserResul.AddRange(lUserResul2)
					End If

				Else  'para mas palabras, se hara la busqueda antigua
					pattern = "[a-z]*" & strSplit(0).Trim & "[a-z]*"
					pattern2 = "[a-z]*" & strSplit(1).Trim & "[a-z]*"
					pattern3 = "[a-z]*" & strSplit(2).Trim & "[a-z]*"
					pattern4 = "[a-z]*" & strSplit(3).Trim & "[a-z]*"

					lUserResul = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
						Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Nombre, pattern3, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Nombre, pattern4, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Apellido1, pattern4, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.Apellido2, pattern4, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.NombreUsuario, pattern, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.NombreUsuario, pattern2, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.NombreUsuario, pattern3, RegexOptions.IgnoreCase) Or _
						Regex.IsMatch(oUsu.NombreUsuario, pattern4, RegexOptions.IgnoreCase))
				End If

			Else
				If (IsNumeric(texto)) Then	 'Si es numerico(codigo persona o idSAB)
					lUserResul = lUsuarios.FindAll(Function(oUsuario As ELL.Usuario) oUsuario.Id = CInt(texto) Or oUsuario.CodPersona = CInt(texto))
				Else
					If (bGetTodosSiTextoVacio) Then lUserResul = lUsuarios
				End If
			End If

			Return lUserResul

		End Function

		''' <summary>
		''' Realiza una busqueda de usuarios a partir de varias condiciones. Es para utilizarlo con el nuevo control de seleccion de usuarios
		''' </summary>
		''' <param name="texto">Texto a buscar</param>
		''' <param name="bConIdDirectorioActivo">Indica si tendra que tener IdDirectorioActivo o no</param>
		''' <param name="bConEmail">Indica si debe tener email</param>
		''' <param name="bConCodPersona">Indica si debe tener codigo de persona</param>
		''' <param name="bVigentes">Indica si se quieren solo los vigentes, o todos</param>
		''' <param name="idEmpresa">Se puede indicar el id de la empresa</param>		
		''' <returns>Lista de usuarios</returns>      
		Public Function GetUsuariosBusquedaSAB(ByVal texto As String, ByVal bConIdDirectorioActivo As Nullable(Of Boolean), ByVal bConEmail As Nullable(Of Boolean), ByVal bConCodPersona As Nullable(Of Boolean), ByVal bVigentes As Boolean, Optional ByVal idEmpresa As Integer = Integer.MinValue) As List(Of ELL.Usuario) Implements IUsuariosComponent.GetUsuariosBusquedaSAB
			Dim usersDAL As New DAL.USUARIOS
			Dim lUsuarios, lUserResul, lUserResul2 As List(Of ELL.Usuario)
			lUserResul2 = Nothing : lUsuarios = Nothing
			Dim pattern, pattern2, pattern3, pattern4 As String
			Dim oUser As New ELL.Usuario
			oUser = New ELL.Usuario
			lUserResul = Nothing
			Dim strSplit As String() = texto.Split(" ")

			oUser.IdEmpresa = idEmpresa

			lUsuarios = GetUsuarios(oUser, bVigentes)

			'Se filtran los campos IdDirectorioActivo, Email y Codigo de persona
			lUsuarios = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) If(bConIdDirectorioActivo.HasValue, If(bConIdDirectorioActivo, oUsu.IdDirectorioActivo <> String.Empty, oUsu.IdDirectorioActivo = String.Empty), True) And _
		  If(bConEmail.HasValue, If(bConEmail, oUsu.Email <> String.Empty, oUsu.Email = String.Empty), True) And _
		  If(bConCodPersona.HasValue, If(bConCodPersona, oUsu.CodPersona <> Integer.MinValue, oUsu.CodPersona = Integer.MinValue), True))

			If (texto <> String.Empty And Not IsNumeric(texto)) Then
				If (strSplit.Length = 1 And strSplit(0).Trim <> String.Empty) Then
					pattern = "[a-z]*" & strSplit(0).Trim & "[a-z]*"
					lUserResul = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) Or _
					   Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) Or _
					   Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) Or _
					   Regex.IsMatch(oUsu.NombreUsuario, pattern, RegexOptions.IgnoreCase))


				ElseIf (strSplit.Length = 2) Then
					pattern = "[a-z]*" & strSplit(0).Trim & "[a-z]*"
					pattern2 = "[a-z]*" & strSplit(1).Trim & "[a-z]*"

					'Se buscan los coincidentes en los dos campos
					lUserResul = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase)))


					If (lUserResul Is Nothing OrElse lUserResul.Count = 0) Then
						'Se buscan los coincidentes en algun campo
						lUserResul2 = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
						   Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern2, RegexOptions.IgnoreCase))
					End If

					'Formamos una unica lista con los mas coincidentes arriba y el resto debajo.
					If (lUserResul.Count = 0 And (lUserResul2 IsNot Nothing AndAlso lUserResul2.Count > 0)) Then
						lUserResul = lUserResul2
					ElseIf (lUserResul.Count > 0 And (lUserResul2 IsNot Nothing AndAlso lUserResul2.Count > 0)) Then
						lUserResul.AddRange(lUserResul2)
					End If

				ElseIf (strSplit.Length = 3) Then
					pattern = "[a-z]*" & strSplit(0).Trim & "[a-z]*"
					pattern2 = "[a-z]*" & strSplit(1).Trim & "[a-z]*"
					pattern3 = "[a-z]*" & strSplit(2).Trim & "[a-z]*"

					'Se buscan los coincidentes en los dos campos
					lUserResul = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)) Or _
					   (Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) And Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase)))

					If (lUserResul Is Nothing OrElse lUserResul.Count = 0) Then
						'Se buscan los coincidentes en algun campo
						lUserResul2 = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
						   Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Nombre, pattern3, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern2, RegexOptions.IgnoreCase) Or _
						   Regex.IsMatch(oUsu.NombreUsuario, pattern3, RegexOptions.IgnoreCase))
					End If

					'Formamos una unica lista con los mas coincidentes arriba y el resto debajo.
					If (lUserResul.Count = 0 And (lUserResul2 IsNot Nothing AndAlso lUserResul2.Count > 0)) Then
						lUserResul = lUserResul2
					ElseIf (lUserResul.Count > 0 And (lUserResul2 IsNot Nothing AndAlso lUserResul2.Count > 0)) Then
						lUserResul.AddRange(lUserResul2)
					End If

				Else  'para mas palabras, se hara la busqueda antigua
					pattern = "[a-z]*" & strSplit(0).Trim & "[a-z]*"
					pattern2 = "[a-z]*" & strSplit(1).Trim & "[a-z]*"
					pattern3 = "[a-z]*" & strSplit(2).Trim & "[a-z]*"
					pattern4 = "[a-z]*" & strSplit(3).Trim & "[a-z]*"

					lUserResul = lUsuarios.FindAll(Function(oUsu As ELL.Usuario) _
					 Regex.IsMatch(oUsu.Nombre, pattern, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Nombre, pattern2, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Nombre, pattern3, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Nombre, pattern4, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Apellido1, pattern, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Apellido1, pattern2, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Apellido1, pattern3, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Apellido1, pattern4, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Apellido2, pattern, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Apellido2, pattern2, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Apellido2, pattern3, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.Apellido2, pattern4, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.NombreUsuario, pattern, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.NombreUsuario, pattern2, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.NombreUsuario, pattern3, RegexOptions.IgnoreCase) Or _
					 Regex.IsMatch(oUsu.NombreUsuario, pattern4, RegexOptions.IgnoreCase))
				End If

			Else
				If (IsNumeric(texto)) Then	 'Si es numerico(codigo persona o idSAB)
					lUserResul = lUsuarios.FindAll(Function(oUsuario As ELL.Usuario) oUsuario.Id = CInt(texto) Or oUsuario.CodPersona = CInt(texto))
				End If
			End If

			Return lUserResul

		End Function

		''' <summary>
		''' Obtiene los grupos de un usuario y una cultura
		''' </summary>
		''' <param name="idUsuario">Identificador del usuario</param>
		''' <param name="idCultura">Identificador de la cultura</param>
		''' <returns>Lista de grupos</returns>
		Function GetGruposUsuario(ByVal idUsuario As Integer, ByVal idCultura As String) As List(Of ELL.grupo) Implements IUsuariosComponent.GetGruposUsuario
			Dim gruposUsuario As New DAL.Views.W_GRUPOS_USUARIO()
			Dim lGrupos As New List(Of ELL.grupo)
			Dim oGrupo As ELL.grupo
			gruposUsuario.Where.IDUSUARIOS.Value = idUsuario
			gruposUsuario.Where.IDUSUARIOS.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
			gruposUsuario.Where.IDCULTURAS.Value = idCultura
			gruposUsuario.Where.IDCULTURAS.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
			gruposUsuario.Query.AddResultColumn(DAL.Views.W_GRUPOS_USUARIO.ColumnNames.IDGRUPOS)
			gruposUsuario.Query.AddResultColumn(DAL.Views.W_GRUPOS_USUARIO.ColumnNames.GRUPO)
			gruposUsuario.Query.Load()
			If (gruposUsuario.RowCount > 0) Then
				Do
					oGrupo = New ELL.grupo
					oGrupo.IdGrupo = gruposUsuario.IDGRUPOS
					oGrupo.Nombre = gruposUsuario.GRUPO
					oGrupo.IdCultura = idCultura
					lGrupos.Add(oGrupo)
				Loop While gruposUsuario.MoveNext
			End If
			Return lGrupos
		End Function

		''' <summary>
		''' Obtiene los grupos de un usuario y una cultura de todos los usuarios, aunque esten dados de baja
		''' </summary>
		''' <param name="idUsuario">Identificador del usuario</param>
		''' <param name="idCultura">Identificador de la cultura</param>		
		''' <returns>Lista de grupos</returns>
		Function GetGruposUsuarioAll(ByVal idUsuario As Integer, ByVal idCultura As String) As List(Of ELL.grupo) Implements IUsuariosComponent.GetGruposUsuarioAll
			Dim gruposUsuario As New DAL.Views.W_GRUPOS_USUARIO_ALL
			Dim lGrupos As New List(Of ELL.grupo)
			Dim oGrupo As ELL.grupo
			gruposUsuario.Where.IDUSUARIOS.Value = idUsuario
			gruposUsuario.Where.IDUSUARIOS.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
			gruposUsuario.Where.IDCULTURAS.Value = idCultura
			gruposUsuario.Where.IDCULTURAS.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
			gruposUsuario.Query.AddResultColumn(DAL.Views.W_GRUPOS_USUARIO_ALL.ColumnNames.IDGRUPOS)
			gruposUsuario.Query.AddResultColumn(DAL.Views.W_GRUPOS_USUARIO_ALL.ColumnNames.GRUPO)
			gruposUsuario.Query.Load()
			If (gruposUsuario.RowCount > 0) Then
				Do
					oGrupo = New ELL.grupo
					oGrupo.IdGrupo = gruposUsuario.IDGRUPOS
					oGrupo.Nombre = gruposUsuario.GRUPO
					oGrupo.IdCultura = idCultura
					lGrupos.Add(oGrupo)
				Loop While gruposUsuario.MoveNext
			End If
			Return lGrupos
		End Function

		''' <summary>
		''' A partir de un objeto mygeneration, devuelve un objeto usuario
		''' </summary>
		''' <param name="usuariosDAL"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function getObject(ByVal usuariosDAL As DAL.USUARIOS, ByVal obtenerFoto As Boolean) As ELL.Usuario
			Dim oUser As New ELL.Usuario

			oUser = New ELL.Usuario
			oUser.Id = usuariosDAL.ID
			oUser.NombreUsuario = usuariosDAL.s_NOMBREUSUARIO
			oUser.IdEmpresa = usuariosDAL.IDEMPRESAS
			oUser.Cultura = usuariosDAL.s_IDCULTURAS
			oUser.Email = usuariosDAL.s_EMAIL
			If Not usuariosDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.CODPERSONA) Then oUser.CodPersona = usuariosDAL.CODPERSONA
			If Not usuariosDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.FECHAALTA) Then oUser.FechaAlta = usuariosDAL.FECHAALTA
			If Not usuariosDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.FECHABAJA) Then oUser.FechaBaja = usuariosDAL.FECHABAJA
			oUser.IdDirectorioActivo = usuariosDAL.s_IDDIRECTORIOACTIVO
			oUser.IdFTP = usuariosDAL.s_IDFTP
			oUser.IdMatrix = usuariosDAL.s_IDMATRIX
			oUser.PWD = usuariosDAL.s_PWD
			If Not usuariosDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.IDDEPARTAMENTO) Then oUser.IdDepartamento = usuariosDAL.IDDEPARTAMENTO
			oUser.Nombre = usuariosDAL.s_NOMBRE.Trim
			oUser.Apellido1 = usuariosDAL.s_APELLIDO1.Trim
			oUser.Apellido2 = usuariosDAL.s_APELLIDO2.Trim
			If Not usuariosDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.DNI) Then oUser.Dni = usuariosDAL.DNI
			oUser.Plantas = GetPlantas(oUser.Id)
			If (obtenerFoto) Then
				If Not usuariosDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.FOTO) Then oUser.Foto = usuariosDAL.FOTO
			End If
			Return oUser
		End Function


		''' <summary>
		''' Devuelve las plantas en las que esta asociado un usuario
		''' </summary>
		''' <param name="idUser">Identificador del usuario</param>
		''' <returns>Lista de plantas</returns>
		''' <remarks></remarks>
		Public Function GetPlantas(ByVal idUser As Integer) As System.Collections.Generic.List(Of ELL.Planta) Implements IUsuariosComponent.GetPlantas
			Dim plantas As New List(Of ELL.Planta)
			Try
				Dim oPlanta As ELL.Planta
				Dim plantComp As New BLL.PlantasComponent
				Dim userPlantasDAL As New DAL.USUARIOS_PLANTAS

				userPlantasDAL.Where.ID_USUARIO.Value = idUser
				userPlantasDAL.Query.Load()

				If userPlantasDAL.RowCount > 0 Then
					Do
						oPlanta = plantComp.GetPlanta(userPlantasDAL.ID_PLANTA)
						plantas.Add(oPlanta)
					Loop While userPlantasDAL.MoveNext()
				End If
			Catch

			End Try
			Return plantas
		End Function



		''' <summary>
		''' Realiza una busqueda de usuarios que tengan acceso a un recurso
		''' </summary>
		''' <param name="idRecurso">Recurso</param>
		''' <param name="vigentes">Indica si se listaran todos o solo los vigentes</param>
		''' <param name="idPlanta">Id de la planta</param>
		''' <returns>Lista de usuarios</returns>
		''' <remarks></remarks>
		Function GetUsuariosConRecurso(ByVal idRecurso As Integer, Optional ByVal vigentes As Boolean = False, Optional ByVal idPlanta As Integer = Integer.MinValue) As List(Of ELL.Usuario) Implements IUsuariosComponent.GetUsuariosConRecurso
			Dim reader As IDataReader = Nothing
			Try
				Dim usersDAL As New DAL.USUARIOS
				Dim lUsuarios As New List(Of ELL.Usuario)
				Dim oUser As ELL.Usuario
				reader = usersDAL.GetUsuariosConRecurso(idRecurso, vigentes, idPlanta)
				While reader.Read
					oUser = New ELL.Usuario
					oUser.Id = CInt(reader(DAL.USUARIOS.ColumnNames.ID))
					If Not reader.IsDBNull(3) Then oUser.NombreUsuario = reader(DAL.USUARIOS.ColumnNames.NOMBREUSUARIO).ToString.Trim
					oUser.IdEmpresa = CInt(reader(DAL.USUARIOS.ColumnNames.IDEMPRESAS))
					oUser.Cultura = reader(DAL.USUARIOS.ColumnNames.IDCULTURAS)
					If Not reader.IsDBNull(4) Then oUser.IdDirectorioActivo = reader(DAL.USUARIOS.ColumnNames.IDDIRECTORIOACTIVO)
					If Not reader.IsDBNull(5) Then oUser.CodPersona = CInt(reader(DAL.USUARIOS.ColumnNames.CODPERSONA))
					If Not reader.IsDBNull(6) Then oUser.PWD = reader(DAL.USUARIOS.ColumnNames.PWD)
					If Not reader.IsDBNull(7) Then oUser.FechaAlta = CType(reader(DAL.USUARIOS.ColumnNames.FECHAALTA), Date)
					If Not reader.IsDBNull(8) Then oUser.FechaBaja = CType(reader(DAL.USUARIOS.ColumnNames.FECHABAJA), Date)
					If Not reader.IsDBNull(9) Then oUser.Apellido1 = reader(DAL.USUARIOS.ColumnNames.APELLIDO1).ToString.Trim
					If Not reader.IsDBNull(10) Then oUser.Apellido2 = reader(DAL.USUARIOS.ColumnNames.APELLIDO2).ToString.Trim
					If Not reader.IsDBNull(11) Then oUser.IdMatrix = reader(DAL.USUARIOS.ColumnNames.IDMATRIX)
					If Not reader.IsDBNull(12) Then oUser.IdFTP = reader(DAL.USUARIOS.ColumnNames.IDFTP)
					If Not reader.IsDBNull(13) Then oUser.Email = reader(DAL.USUARIOS.ColumnNames.EMAIL).ToString.Trim
					If Not reader.IsDBNull(14) Then oUser.IdDepartamento = reader(DAL.USUARIOS.ColumnNames.IDDEPARTAMENTO)
					If Not reader.IsDBNull(15) Then oUser.Nombre = reader(DAL.USUARIOS.ColumnNames.NOMBRE).ToString.Trim
					If Not reader.IsDBNull(16) Then oUser.Dni = reader(DAL.USUARIOS.ColumnNames.DNI)

					lUsuarios.Add(oUser)
				End While
				Return lUsuarios
			Catch
				Return Nothing
			Finally
				If (reader IsNot Nothing) Then reader.Close()
			End Try
		End Function

		''' <summary>
		''' Obtiene la foto del usuario vigente
		''' </summary>
		''' <param name="oUser"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetFoto(ByVal oUser As ELL.Usuario) As Byte()
			Try
				oUser = GetUsuario(oUser, True, True)
				If (oUser IsNot Nothing) Then
					Return oUser.Foto
				Else
					Return Nothing
				End If
			Catch ex As Exception
				Return Nothing
			End Try
		End Function

		''' <summary>
		''' Devuelve los usuarios con el recurso idRecurso y que no pertenezca a Batz
		''' </summary>
		''' <param name="idRecurso"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetProveedoresConRecurso(ByVal idRecurso As Integer) As System.Collections.Generic.List(Of ELL.Empresa) Implements IUsuariosComponent.GetProveedoresConRecurso
			Dim proveedores As New List(Of ELL.Empresa)
			Dim wResponsables As New DAL.Views.W_RECURSOS_EMPRESA
			wResponsables.Where.IDRECURSOS.Value = idRecurso
			wResponsables.Where.IDRECURSOS.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
			wResponsables.Query.Load()
			If wResponsables.RowCount > 0 Then
				Do
					Dim proveedor As New ELL.Empresa
					proveedor.Id = wResponsables.ID
					If Not wResponsables.IsColumnNull(DAL.Views.W_RECURSOS_EMPRESA.ColumnNames.IDTROQUELERIA) Then
						proveedor.IdTroqueleria = wResponsables.IDTROQUELERIA
					End If
					proveedor.Nombre = wResponsables.s_NOMBRE
					proveedores.Add(proveedor)
				Loop While wResponsables.MoveNext()
			End If
			Return proveedores
		End Function

#End Region

#Region "Delete"

		''' <summary>
		''' Borra el usuario
		''' </summary>
		''' <param name="idUsuario">Identificador del usuario</param>
		''' <returns>Booleano indicando si se ha borrado correctamente</returns>
		''' <remarks></remarks>
		Function Delete(ByVal idUsuario As Integer) As Boolean Implements IUsuariosComponent.Delete
			Try
				Dim usuario As New DAL.USUARIOS()
				usuario.LoadByPrimaryKey(idUsuario)
				If (usuario.RowCount = 1) Then
					usuario.MarkAsDeleted()
					usuario.Save()
					Return True
				End If
				Return False
			Catch
				Return False
			End Try
		End Function


		''' <summary>
		''' Elimina la planta al usuario
		''' </summary>        
		''' <param name="idUsuario">Id del usuario</param>
		''' <param name="idPlanta">Id de la planta</param>
		''' <returns>Booleano que indica si se ha borrado correctamente</returns>
		Public Function DeletePlanta(ByVal idUsuario As Integer, ByVal idPlanta As Integer) As Boolean Implements IUsuariosComponent.DeletePlanta
			Try
				Dim plantUser As New DAL.USUARIOS_PLANTAS
				plantUser.LoadByPrimaryKey(idPlanta, idUsuario)
				If (plantUser.RowCount = 1) Then
					plantUser.MarkAsDeleted()
					plantUser.Save()
					Return True
				End If
				Return False
			Catch ex As Exception
				log.Error("Error al borrar la planta del usuario", ex)
				Return False
			End Try

		End Function

#End Region

#Region "Save"

		''' <summary>
		''' Guarda los datos del usuario y la planta en el caso en que sea nuevo
		''' </summary>        
		''' <param name="objUsuario">Objeto usuario a guardar</param>
		''' <returns>Booleano que indica si se ha guardado correctamente</returns>
		Public Function Save(ByVal objUsuario As ELL.Usuario) As Integer Implements IUsuariosComponent.Save
			Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
			Dim bNuevo As Boolean = objUsuario.Id = 0 Or objUsuario.Id = Integer.MinValue
			Dim idUser As Integer = Integer.MinValue
			Try
				Dim usuario As New DAL.USUARIOS()

				If (bNuevo) Then
					tx.BeginTransaction()
					usuario.AddNew()
				Else
					usuario.LoadByPrimaryKey(objUsuario.Id)
				End If

				If (usuario.RowCount = 1) Then
					If (objUsuario.Cultura <> String.Empty) Then usuario.IDCULTURAS = objUsuario.Cultura
					If (objUsuario.IdDirectorioActivo <> String.Empty) Then usuario.IDDIRECTORIOACTIVO = objUsuario.IdDirectorioActivo
					If (objUsuario.IdEmpresa > 0) Then usuario.IDEMPRESAS = objUsuario.IdEmpresa
					If (objUsuario.IdFTP <> String.Empty) Then usuario.IDFTP = objUsuario.IdFTP
					If (objUsuario.IdMatrix <> String.Empty) Then usuario.IDMATRIX = objUsuario.IdMatrix
					If (objUsuario.NombreUsuario <> String.Empty) Then usuario.NOMBREUSUARIO = objUsuario.NombreUsuario
					If (objUsuario.CodPersona > 0) Then usuario.CODPERSONA = objUsuario.CodPersona
					If (objUsuario.Email <> String.Empty) Then usuario.EMAIL = objUsuario.Email
					If (objUsuario.FechaAlta <> Date.MinValue) Then usuario.FECHAALTA = objUsuario.FechaAlta
					If (objUsuario.FechaBaja <> Date.MinValue) Then
						usuario.FECHABAJA = objUsuario.FechaBaja
					Else
						usuario.s_FECHABAJA = String.Empty
					End If

					If (objUsuario.PWD <> String.Empty) Then usuario.PWD = objUsuario.PWD
					If (objUsuario.IdDepartamento <> String.Empty) Then usuario.IDDEPARTAMENTO = objUsuario.IdDepartamento
					If (objUsuario.Nombre <> String.Empty) Then usuario.NOMBRE = objUsuario.Nombre
					If (objUsuario.Apellido1 <> String.Empty) Then usuario.APELLIDO1 = objUsuario.Apellido1
					If (objUsuario.Apellido2 <> String.Empty) Then usuario.APELLIDO2 = objUsuario.Apellido2
					If (objUsuario.Dni <> String.Empty) Then usuario.DNI = objUsuario.Dni
					If (objUsuario.Foto IsNot Nothing) Then usuario.FOTO = objUsuario.Foto
					'If (objUsuario.Foto IsNot Nothing AndAlso objUsuario.Foto.Length = 0) Then 'Para que una foto existente, se pueda borrar
					'usuario.FOTO = Nothing
					'End If

					usuario.Save()
					idUser = usuario.ID

					If (bNuevo) Then
						'Se inserta la planta
						If (objUsuario.IdPlanta <> Integer.MinValue) Then
							Dim plantUser As New DAL.USUARIOS_PLANTAS
							plantUser.AddNew()
							If (plantUser.RowCount = 1) Then
								plantUser.ID_PLANTA = objUsuario.IdPlanta
								plantUser.ID_USUARIO = usuario.ID
								plantUser.Save()
							End If
						End If
						tx.CommitTransaction()
					End If

					Return idUser
				Else
					Throw New Exception("No se ha encontrado el usuario")
				End If
			Catch ex As Exception
				If (bNuevo) Then
					tx.RollbackTransaction()
					TransactionMgr.ThreadTransactionMgrReset()
				End If
				log.Error("Error al guardar los datos del usuario", ex)
				Return Integer.MinValue
			End Try

		End Function


		''' <summary>
		''' Guarda el nombre de usuario en SAB y en IZARO
		''' </summary>        
		''' <param name="idUser">Id del usuario</param>
		''' <param name="nombre">Nombre de usuario a guardar</param>
		''' <returns>Booleano que indica si se ha guardado correctamente</returns>
		Function SaveUserSABIzaro(ByVal idUser As Integer, ByVal nombre As String) As Boolean Implements IUsuariosComponent.SaveUserSABIzaro
            Dim cn As OracleConnection = Nothing
            Try
                Dim userComp As New BLL.UsuariosComponent
                Dim userDAL As New DAL.USUARIOS
                Dim idTrab As Integer

                userDAL.LoadByPrimaryKey(idUser)
                If (userDAL.RowCount = 1) Then
                    idTrab = userDAL.CODPERSONA
                    userDAL.NOMBREUSUARIO = nombre
                    userDAL.IDDIRECTORIOACTIVO = "batznt\" & nombre
                    userDAL.Save()

                    'Se guarda en IZARO solo para los contratados, ya que en este campo, para los subcontratados, se guarda el idEmpresa
                    If (userDAL.IDEMPRESAS = 1) Then
                        cn = New OracleConnection()
                        Dim bEstado As Boolean = False
                        cn.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings("CN_DBIZARO").ConnectionString

                        Dim cmd As New OracleCommand()
                        cmd.Connection = cn
                        cmd.CommandText = "UPDATE FPERTIC SET TI300=:NOMBREUSUARIO WHERE TI000=1 AND TI010=:IDTRABAJADOR"

                        cmd.Parameters.Add("NOMBREUSUARIO", OracleDbType.NVarchar2, ParameterDirection.Input)
                        cmd.Parameters.Add("IDTRABAJADOR", OracleDbType.Int32, ParameterDirection.Input)

                        cmd.Parameters("NOMBREUSUARIO").Value = nombre
                        cmd.Parameters("IDTRABAJADOR").Value = idTrab
                        cn.Open()
                        If (cmd.ExecuteNonQuery = 1) Then  'se ha insertado 1 registro
                            Return True
                        End If
                        Return False
                    Else
                        Return True
                    End If
                End If

                Return False
            Catch ex As Exception
                If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
                log.Error("Error al guardar los datos del usuario", ex)
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Guarda la nueva fecha de baja de usuario en SAB y en IZARO
        ''' </summary>        
        ''' <param name="idUser">Id del usuario</param>
        ''' <param name="fechaBaja">Fecha de baja</param>
        ''' <returns>Booleano que indica si se ha guardado correctamente</returns>
        Function SaveFechaBajaUserSABIzaro(ByVal idUser As Integer, ByVal fechaBaja As Date) As Boolean Implements IUsuariosComponent.SaveFechaBajaUserSABIzaro
            Dim cn As OracleConnection = Nothing
            Try
                Dim userComp As New BLL.UsuariosComponent
                Dim userDAL As New DAL.USUARIOS
                Dim idTrab As Integer

                userDAL.LoadByPrimaryKey(idUser)
                If (userDAL.RowCount = 1) Then
                    idTrab = userDAL.CODPERSONA
                    If (fechaBaja = Date.MinValue) Then
                        userDAL.s_FECHABAJA = String.Empty
                    Else
                        userDAL.FECHABAJA = fechaBaja
                    End If
                    userDAL.Save()

                    'Se guarda en IZARO

                    cn = New OracleConnection()
                    Dim bEstado As Boolean = False
                    cn.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings("CN_DBIZARO").ConnectionString

                    Dim cmd As New OracleCommand()
                    cmd.Connection = cn
                    cmd.CommandText = "UPDATE FCPWTRA SET TR270=:FECHA_BAJA WHERE TR010=:IDTRABAJADOR"

                    cmd.Parameters.Add("FECHA_BAJA", OracleDbType.Date, ParameterDirection.Input)
                    cmd.Parameters.Add("IDTRABAJADOR", OracleDbType.Int32, ParameterDirection.Input)

                    If (fechaBaja = Date.MinValue) Then
						cmd.Parameters("FECHA_BAJA").Value = DBNull.Value
					Else
						cmd.Parameters("FECHA_BAJA").Value = fechaBaja
					End If
					cmd.Parameters("IDTRABAJADOR").Value = idTrab
					cn.Open()
					If (cmd.ExecuteNonQuery = 1) Then  'se ha insertado 1 registro
						Return True
					End If
					Return False
				Else
					Return True
				End If

				Return False
			Catch ex As Exception
				If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
				log.Error("Error al guardar los datos del usuario", ex)
				Return False
			End Try
		End Function

		''' <summary>
		''' Guarda la password encriptada
		''' </summary>        
		''' <param name="idUser">Id del usuario</param>
		''' <param name="password">Password</param>
		''' <returns>Booleano que indica si se ha guardado correctamente</returns>
		Function SavePassword(ByVal idUser As Integer, ByVal password As String) As Boolean Implements IUsuariosComponent.SavePassword
			Try
				Dim userComp As New BLL.UsuariosComponent
				Dim oUser As New ELL.Usuario
				oUser.Id = idUser
				oUser = userComp.GetUsuario(oUser)
				oUser.PWD = BLL.Utils.EncriptarPassword(password)
				Return userComp.Save(oUser)
			Catch ex As Exception
				log.Error("Error al guardar los datos del usuario", ex)
				Return False
			End Try
		End Function

		''' <summary>
		''' Guarda la foto
		''' </summary>        
		''' <param name="idUser">Id del usuario</param>
		''' <param name="foto">Foto</param>
		''' <returns>Booleano que indica si se ha guardado correctamente</returns>
		Function SaveFoto(ByVal idUser As Integer, ByVal foto As Byte()) As Boolean Implements IUsuariosComponent.SaveFoto
			Try
				Dim userComp As New BLL.UsuariosComponent
				Dim oUser As New ELL.Usuario
				oUser.Id = idUser
				oUser = userComp.GetUsuario(oUser)
				oUser.Foto = foto
				Return userComp.Save(oUser)
			Catch ex As Exception
				log.Error("Error al guardar los datos del usuario", ex)
				Return False
			End Try
		End Function

		''' <summary>
		''' Actualiza los datos necesarios en la creacion de usuarios en KEM
		''' </summary>        
		''' <param name="objUsuario">Objeto usuario a guardar</param>
		Public Function updateKEM(ByVal objUsuario As ELL.Usuario) As Boolean Implements IUsuariosComponent.updateKEM
			Try
				Dim usuarioDAL As New DAL.USUARIOS()
				usuarioDAL.LoadByPrimaryKey(objUsuario.Id)

				If (usuarioDAL.RowCount = 1) Then
					usuarioDAL.IDDIRECTORIOACTIVO = objUsuario.IdDirectorioActivo
					usuarioDAL.NOMBREUSUARIO = objUsuario.NombreUsuario
					usuarioDAL.CODPERSONA = objUsuario.CodPersona
					usuarioDAL.EMAIL = objUsuario.Email
					usuarioDAL.IDDEPARTAMENTO = objUsuario.IdDepartamento
					usuarioDAL.NOMBRE = objUsuario.Nombre
					usuarioDAL.APELLIDO1 = objUsuario.Apellido1
					usuarioDAL.APELLIDO2 = objUsuario.Apellido2
					usuarioDAL.DNI = objUsuario.Dni
					If (objUsuario.FechaBaja <> Date.MinValue) Then
						usuarioDAL.FECHABAJA = objUsuario.FechaBaja
					Else
						usuarioDAL.s_FECHABAJA = String.Empty
					End If
					usuarioDAL.FOTO = objUsuario.Foto

					usuarioDAL.Save()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try

		End Function

		''' <summary>
		''' Añade la planta al usuario
		''' </summary>        
		''' <param name="idUsuario">Id del usuario</param>
		''' <param name="idPlanta">Id de la planta</param>
		''' <returns>Booleano que indica si se ha guardado correctamente</returns>
		Public Function AddPlanta(ByVal idUsuario As Integer, ByVal idPlanta As Integer) As Boolean Implements IUsuariosComponent.AddPlanta
			Try
				Dim plantUser As New DAL.USUARIOS_PLANTAS
				plantUser.AddNew()
				If (plantUser.RowCount = 1) Then
					plantUser.ID_PLANTA = idPlanta
					plantUser.ID_USUARIO = idUsuario
					plantUser.Save()
					Return True
				End If
				Return False
			Catch ex As Exception
				log.Error("Error al añadir la planta al usuario", ex)
				Return False
			End Try

		End Function

		''' <summary>
		''' Cambia la password del usuario en LDAP (Itxina)
		''' De momento, solo es para usuarios con el email BatzCoop
		''' </summary>
		''' <param name="email">Email</param>
		''' <param name="currentPassword">Password actual</param>
		''' <param name="newPassword">Nueva contraseña</param>		
		Public Sub ChangePasswordLDAP(ByVal email As String, ByVal currentPassword As String, ByVal newPassword As String) Implements [Interface].IUsuariosComponent.ChangePasswordLDAP
			Dim entry, myEntry As DirectoryEntry
			entry = Nothing
			myEntry = Nothing
			Try
				If (email.ToLower.Contains("batz.coop")) Then
					Dim oUser As ELL.Usuario = Nothing
					Dim strPath As String = "LDAP://itxina"
					Dim search As New DirectorySearcher()
					entry = New DirectoryEntry()

					entry = New DirectoryEntry(strPath)
					entry.AuthenticationType = AuthenticationTypes.Secure
					entry.Username = Configuration.ConfigurationManager.AppSettings("usuarioLDAPAdmin")
					entry.Password = Configuration.ConfigurationManager.AppSettings("passwordLDAPAdmin")

					'Realizamos una busqueda sobre la entrada anteriormente seleccionada.
					search = New DirectorySearcher(entry)
					search.Filter = "(mail=" & email & ")"
					'search.Filter = "(userprincipalname=" & email & ")"

					'Y realizamos una busqueda de todos sus datos.
					Dim result As SearchResult = search.FindOne()

					If (result IsNot Nothing) Then
						myEntry = result.GetDirectoryEntry()

						myEntry.Invoke("ChangePassword", New Object() {currentPassword, newPassword})
						myEntry.CommitChanges()
					Else
						Throw New Exception("Usuario no encontrado en el Directorio Activo")
					End If
				Else
					Throw New Exception("Funcion de cambio de password solo valida para usuarios de Batz.coop")
				End If
			Finally
				If (entry IsNot Nothing) Then entry.Close()
				If (myEntry IsNot Nothing) Then myEntry.Close()
			End Try
		End Sub

#End Region

#Region "Generar Codigo de persona"

		''' <summary>
		''' Genera un codigo de persona, dependiendo de la planta externa en la que se encuentre
		''' Para la planta 1, no se generara
		''' </summary>
		''' <param name="idPlanta">Identificador de la planta</param>
		''' <returns>Codigo de persona</returns>        
		Public Function GenerarCodPersona(ByVal idPlanta As Integer) As Integer
			Dim codPersona As Integer = 0
			Dim userDAL As New DAL.USUARIOS
			If (idPlanta <> Integer.MinValue And idPlanta <> 1) Then 'Para batz Igorre, no se aplicara
				codPersona = userDAL.GenerarCodPersona(idPlanta)
			End If
			Return codPersona
		End Function

#End Region

#Region "Importar datos"

		''' <summary>
		''' Importa los datos seleccionados (se indican el nombre del campo a actualizar) del usuario origen al destino
		''' </summary>
		''' <param name="idUserOrigen">Id del usuario origen. Del que se importan</param>
		''' <param name="idUserDestino">Id del usuario destino. Al que se importan</param>
		''' <param name="datos">Array con los nombres de las columnas. Indica los campos a copiar</param>
		''' <returns></returns>		
		Function ImportarDatos(ByVal idUserOrigen As Integer, ByVal idUserDestino As Integer, ByVal datos As String()) As Boolean Implements IUsuariosComponent.ImportarDatos
			Try
				Dim bImportado As Boolean = False
				'Se obtiene la informacion del usuario origen
				Dim oUserOri As New ELL.Usuario With {.Id = idUserOrigen}
				oUserOri = GetUsuario(oUserOri, False, True)

				'Se obtiene la informacion del usuario destino
				Dim oUserDest As New ELL.Usuario With {.Id = idUserDestino}
				oUserDest = GetUsuario(oUserDest, True, True)

				If (oUserOri Is Nothing OrElse oUserDest Is Nothing) Then
					bImportado = False
				Else
					For Each dato As String In datos
						'Aqui se añadiria un case para cada nuevo datos
						Select Case dato
							Case ELL.Usuario.ColumnNames.FOTO
								oUserDest.Foto = oUserOri.Foto
						End Select
					Next

					'Se guardan los cambios
					If (Save(oUserDest) <> Integer.MinValue) Then bImportado = True

				End If
				Return bImportado
			Catch ex As Exception
				log.Error("Ha ocurrido un error al importar los datos del usuario", ex)
			End Try
		End Function

#End Region

	End Class
End Namespace

