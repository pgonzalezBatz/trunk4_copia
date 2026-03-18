Imports log4net

Namespace DAL
    Public Class USUARIOS
        Inherits _USUARIOS

        Private log As ILog = LogManager.GetLogger("root.LISTAMAT")

        Public Sub New()
            'Decide connection string depending on state
            If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABLIVE").ConnectionString
            Else
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABTEST").ConnectionString
            End If
        End Sub


        ''' <summary>
        ''' Genera un codigo de la persona, dependiendo de la planta. Para igorre, no se aplicara.
        ''' Si no encuentra ninguno, se le asigna el primero de la serie
        ''' </summary>
        ''' <param name="idPlanta"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GenerarCodPersona(ByVal idPlanta As Integer) As Integer
            Dim codigo As Integer = 0
            Try
                Dim sql As String = "SELECT max(CODPERSONA) FROM USUARIOS WHERE length(codPersona)=6 and codpersona LIKE '" & idPlanta & "%'"
                codigo = MyBase.LoadFromSqlScalar(sql, Nothing, CommandType.Text)
                codigo += 1
            Catch
                'no existe ninguno con ese codigo. Le asignamos el primero
                codigo = CInt(idPlanta & "00001")
            End Try
            Return codigo
        End Function


        ''' <summary>
        ''' Obtiene la informacion de todos los usuarios buscados por nombre
        ''' </summary>
        ''' <param name="nombre">Nombre de la persona a buscar</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUsuariosByNombre(ByVal nombre As String) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim where As String = String.Empty
            Dim nombreSplit As String() = nombre.Split(" ")

            sql.AppendLine("SELECT distinct ID,IDEMPRESAS,IDCULTURAS,NOMBREUSUARIO,IDDIRECTORIOACTIVO,CODPERSONA,PWD,FECHAALTA,FECHABAJA,APELLIDO1,APELLIDO2,IDMATRIX,IDFTP,EMAIL,IDDEPARTAMENTO,NOMBRE,DNI ")
            sql.AppendLine("FROM USUARIOS ")
            sql.Append("WHERE ")

            For Each oStr As String In nombreSplit
                oStr = "'%" & oStr.ToLower & "%'"
                If (where <> String.Empty) Then where &= " OR " 'A excepcion de la primera vez, hay que añadir un OR
                where &= "lower(NOMBRE) LIKE " & oStr & " OR "
                where &= "lower(APELLIDO1) LIKE " & oStr & " OR "
                where &= "lower(APELLIDO2) LIKE " & oStr & " OR "
                where &= "lower(NOMBREUSUARIO) LIKE " & oStr
            Next
            sql.Append(where)

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)

        End Function



        ''' <summary>
        ''' Obtiene la informacion de todos los usuarios buscados por nombre con un algoritmo mas eficaz
        ''' </summary>
        ''' <param name="nombre">Nombre de la persona a buscar</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUsuariosByNombre2(ByVal nombre As String) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim where As String = String.Empty
            Dim nombreSplit As String() = nombre.Split(" ")

            sql.AppendLine("SELECT distinct ID,IDEMPRESAS,IDCULTURAS,NOMBREUSUARIO,IDDIRECTORIOACTIVO,CODPERSONA,PWD,FECHAALTA,FECHABAJA,APELLIDO1,APELLIDO2,IDMATRIX,IDFTP,EMAIL,IDDEPARTAMENTO,NOMBRE,DNI ")
            sql.AppendLine("FROM USUARIOS ")
            sql.Append("WHERE ")

            For Each oStr As String In nombreSplit
                oStr = "'%" & oStr.ToLower & "%'"
                If (where <> String.Empty) Then where &= " OR " 'A excepcion de la primera vez, hay que añadir un OR
                where &= "lower(NOMBRE) LIKE " & oStr & " OR "
                where &= "lower(APELLIDO1) LIKE " & oStr & " OR "
                where &= "lower(APELLIDO2) LIKE " & oStr & " OR "
                where &= "lower(NOMBREUSUARIO) LIKE " & oStr
            Next
            sql.Append(where)

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)

        End Function

        ''' <summary>
        ''' Obtiene la informacion de todos los usuarios que pertenezcan a una planta
        ''' </summary>
        ''' <param name="lPlantas"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUsuariosConPlanta(ByVal lPlantas As List(Of Integer)) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim sIn As String = String.Empty

            sql.AppendLine("SELECT distinct U.ID,U.IDEMPRESAS,U.IDCULTURAS,NOMBREUSUARIO,IDDIRECTORIOACTIVO,CODPERSONA,PWD,FECHAALTA,FECHABAJA,APELLIDO1,APELLIDO2,IDMATRIX,IDFTP,EMAIL,IDDEPARTAMENTO,NOMBRE,DNI ")
            sql.AppendLine("FROM USUARIOS_PLANTAS UP INNER JOIN USUARIOS U ON UP.ID_USUARIO=U.ID ")
            sql.Append("WHERE UP.ID_PLANTA IN (")

            For Each oInt As Integer In lPlantas
                If (sIn <> String.Empty) Then sIn &= ","
                sIn &= "'" & oInt.ToString & "'"
            Next
            sql.Append(sIn)
            sql.Append(") AND U.IDDIRECTORIOACTIVO IS NOT NULL AND ( (U.FECHABAJA IS NULL) OR (FECHABAJA IS NOT NULL AND FECHABAJA>sysdate) ) ")
            sql.AppendLine("ORDER BY U.NOMBRE,APELLIDO1,APELLIDO2")

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)

        End Function



        ''' <summary>
        ''' Obtiene la informacion de todos los usuarios que pertenezcan a una planta y que cumplan las condiciones del objeto
        ''' </summary>
        ''' <param name="oUser"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUsuariosConPlanta2(ByVal oUser As ELL.Usuario) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim sWhere As String = String.Empty

            sql.AppendLine("SELECT distinct U.ID,U.IDEMPRESAS,U.IDCULTURAS,NOMBREUSUARIO,IDDIRECTORIOACTIVO,CODPERSONA,PWD,FECHAALTA,FECHABAJA,APELLIDO1,APELLIDO2,IDMATRIX,IDFTP,EMAIL,IDDEPARTAMENTO,NOMBRE,DNI ")
            sql.AppendLine("FROM USUARIOS_PLANTAS UP INNER JOIN USUARIOS U ON UP.ID_USUARIO=U.ID ")
            sql.Append("WHERE UP.ID_PLANTA=" & oUser.IdPlanta)
            If (oUser.Nombre <> String.Empty) Then                
                sql.AppendLine(" and (lower(U.NOMBRE) LIKE '%" & oUser.Nombre.ToLower & "%' OR lower(U.APELLIDO1) LIKE '%" & oUser.Nombre.ToLower & "%' OR lower(U.APELLIDO2) LIKE '%" & oUser.Nombre.ToLower.ToLower & "%')")
            End If
            If (oUser.NombreUsuario <> String.Empty) Then
                sql.AppendLine(" and (lower(U.NOMBREUSUARIO) LIKE '%" & oUser.NombreUsuario.ToLower & "%'")
            End If
            If (oUser.IdDepartamento <> String.Empty) Then
                sql.AppendLine(" and U.IDDEPARTAMENTO='" & oUser.IdDepartamento & "'")
            End If
            If (oUser.IdEmpresa <> Integer.MinValue) Then
                sql.AppendLine(" and U.IDEMPRESA LIKE=" & oUser.IdEmpresa)
            End If
            sql.Append("AND (U.IDDIRECTORIOACTIVO IS NOT NULL OR U.CODPERSONA IS NOT NULL) AND ( (U.FECHABAJA IS NULL) OR (FECHABAJA IS NOT NULL AND FECHABAJA>sysdate) ) ")
            sql.AppendLine("ORDER BY U.NOMBRE,APELLIDO1,APELLIDO2")

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)

        End Function

        ''' <summary>
        ''' Obtiene los usuarios que estan en un recurso
        ''' </summary>
		''' <param name="idRecurso">Id del recurso</param>
		''' <param name="vigentes">Indica si se obtendran los vigentes o todos</param>
		''' <param name="idPlanta">Parametro opcional con el Id de la planta a la que pertenece</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Function GetUsuariosConRecurso(ByVal idRecurso As Integer, ByVal vigentes As Boolean, Optional ByVal idPlanta As Integer = Integer.MinValue) As IDataReader
			Dim sql As New System.Text.StringBuilder
			Dim sIn As String = String.Empty

			sql.AppendLine("SELECT distinct U.ID,U.IDEMPRESAS,U.IDCULTURAS, U.NOMBREUSUARIO,U.IDDIRECTORIOACTIVO,U.CODPERSONA,U.PWD,U.FECHAALTA,U.FECHABAJA,U.APELLIDO1,U.APELLIDO2,U.IDMATRIX,U.IDFTP,U.EMAIL,U.IDDEPARTAMENTO,U.NOMBRE,DNI ")
			sql.AppendLine("FROM W_RECURSOS_USUARIO RP INNER JOIN USUARIOS U ON RP.IDUSUARIO=U.ID ")
			If (idPlanta <> Integer.MinValue) Then sql.AppendLine("INNER JOIN USUARIOS_PLANTAS UP ON U.ID=UP.ID_USUARIO ")			
			sql.Append("WHERE RP.IDRECURSO=" & idRecurso)
			If (vigentes) Then sql.Append(" AND ((U.FECHABAJA IS NULL) OR (U.FECHABAJA IS NOT NULL AND U.FECHABAJA>=sysdate))")
			If (idPlanta <> Integer.MinValue) Then sql.Append(" AND UP.ID_PLANTA=" & idPlanta)
			sql.AppendLine(" ORDER BY U.NOMBRE,APELLIDO1,APELLIDO2")

			Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)

		End Function
    End Class
End Namespace