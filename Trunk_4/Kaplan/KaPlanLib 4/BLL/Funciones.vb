Imports log4net
Imports System.Reflection
Imports System.Data.Common
Imports System.Web

Public Class Funciones

	Private log As ILog = LogManager.GetLogger("root.KaPlanLib.Funciones")

	''' <summary>
	''' Proceso para generar el informe del Nivel.
	''' </summary>
	''' <param name="Referencia">Referencia del Articulo. Si 'Tipo=3' Codigo de Operacion</param>
	''' <param name="Empresa"></param>
	''' <param name="Idioma"></param>
	''' <param name="FechaGeneracion"></param>
	''' <param name="Tipo">Tipo de Documento guardado. 1.- AMFE Articulo. 2.- Plan de Control Articulo. 3.- Hoja de Instrucciones Operacion</param>
	''' <param name="IdNivel"></param>
	''' <param name="IdUsuario"></param>
	''' <param name="Maquina"></param>
	''' <remarks></remarks>
	Sub GenerarPDF(ByRef Referencia As String, ByRef Empresa As String, ByRef Idioma As String, ByRef FechaGeneracion As Date, ByRef Tipo As Integer, ByRef IdNivel As String, ByRef IdUsuario As String, ByRef Maquina As String, ByRef HttpRequest As Web.HttpRequest)
		Dim CognosURL As String = String.Empty
        Try
            CognosURL = String.Format("{0}/samples/index.jsp?", "https://cognos.batz.es") &
              "Referencia=" & Referencia &
              "&empresa=" & Empresa &
              "&Idioma=" & Idioma &
              "&fecGen=" & FechaGeneracion.ToString("dd/MM/yyyy hh:mm:ss") &
              "&tipo=" & Tipo &
              "&nivel=" & IdNivel &
              "&usuario=" & IdUsuario &
              "&maquina=" & Maquina
            '----------------------------------------------------------------------

            Dim stm As IO.Stream
            Dim req As System.Net.HttpWebRequest = System.Net.HttpWebRequest.Create(CognosURL)
            req.Credentials = System.Net.CredentialCache.DefaultCredentials
            req.Method = "GET"

            Dim wResponse As System.Net.WebResponse = req.GetResponse()
            stm = wResponse.GetResponseStream
            Dim stmr As New IO.StreamReader(stm)
            Dim result As String = stmr.ReadToEnd
            stmr.Close()
            stm.Close()

            If result.Trim.ToUpper <> "OK" Then
                log.Error(result.Trim)
                Throw New ApplicationException("errCrearDocumento")
            End If
        Catch ex As ApplicationException
            log.Debug("CognosURL: " & CognosURL, ex)
            Throw
        Catch ex As Exception
            log.Error("CognosURL: " & CognosURL, ex)
			Throw
		End Try
	End Sub

	''' <summary>
	''' Proceso para el copiado de los valores de las propiedades de un objeto en otro.
	''' Cargamos los datos de los campos coincidentes del objeto Origen al Destino.
	''' </summary>
	''' <param name="Origen">Objeto donde 'OBTENEMOS' los valores.</param>
	''' <param name="Destino">Objeto donde 'CARGAMOS' los valores.</param>
	''' <remarks></remarks>
	Public Sub CopiarPropiedades(ByVal Origen As Object, ByVal Destino As Object)
		'-------------------------------------------------------------------------------------------------------------------------------------
		'El objeto "Origen" es la "clase base" (BaseType) del objeto "Destino".
		'El objeto "Destino" puede esconder propiedades del objeto "Origen".
		'-------------------------------------------------------------------------------------------------------------------------------------
		For Each Propiedad As PropertyInfo In Destino.GetType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
			If Propiedad.GetSetMethod(True) IsNot Nothing Then
				For Each rPropiedad As PropertyInfo In Origen.GetType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
					If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
						Propiedad.SetValue(Destino, rPropiedad.GetValue(Origen, Nothing), Nothing)
					End If
				Next
			End If
		Next
		'-------------------------------------------------------------------------------------------------------------------------------------
		'-------------------------------------------------------------------------------------------------------------------------------------
		'Para evitar que algunos campos se queden sin datos, cogemos los valores del "Origen" y se los pasamos a la "clase base" del destino.
		'-------------------------------------------------------------------------------------------------------------------------------------
		For Each Propiedad As PropertyInfo In Destino.GetType.BaseType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
			If Propiedad.GetSetMethod(True) IsNot Nothing Then
				For Each rPropiedad As PropertyInfo In Origen.GetType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
					If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
						Propiedad.SetValue(Destino, rPropiedad.GetValue(Origen, Nothing), Nothing)
					End If
				Next
			End If
		Next
		'-------------------------------------------------------------------------------------------------------------------------------------
	End Sub


	''' <summary>
	''' Genera la consulta SQL que se ejecuta en la base de datos. 
	''' </summary>
	''' <param name="Consulta">Consulta LINQ antes de su ejecucion.</param>
	''' <param name="BBDD">Contexto de la base de datos.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Function Generar_LINQ_SQL(Consulta As IEnumerable(Of Object), BBDD As KaPlanLib.DAL.ELL) As String
		Dim dc As DbCommand = BBDD.GetCommand(Consulta)
		Generar_LINQ_SQL = String.Empty
		For Each Param As SqlClient.SqlParameter In dc.Parameters
			Generar_LINQ_SQL &= vbNewLine & "DECLARE " & Param.ParameterName & " " & Param.SqlDbType.ToString & ";"
			Generar_LINQ_SQL &= vbNewLine & "SELECT " & Param.ParameterName & " = '" & Param.Value & "';"
		Next
		Generar_LINQ_SQL &= vbNewLine & dc.CommandText & ";"

		Return Generar_LINQ_SQL
	End Function

	''' <summary>
	''' Proceso que guarda un registro de los posibles cambios en la base de datos.
	''' Colocar antes del BBDD.SubmitChanges()
	''' </summary>
	''' <param name="BBDD"></param>
	''' <param name="NombreFuncion">New StackFrame(0).GetMethod().Name</param>
	''' <example>Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)</example>	
	Sub Segumiento_Actividad(ByRef BBDD As KaPlanLib.DAL.ELL, ByVal NombreFuncion As String)
		Dim msg_log As String = Nothing
		Dim lEntidades As Data.Linq.ChangeSet = BBDD.GetChangeSet

		'---------------------------------------------------------------------------------------------------------------------
		For Each Entidad In lEntidades.Deletes
			Dim Tabla_BBDD As Data.Linq.ITable = BBDD.GetTable(Entidad.GetType())
			Dim modifiedMembers As Data.Linq.ModifiedMemberInfo() = Tabla_BBDD.GetModifiedMembers(Entidad)
			Dim Inf_Entidad As String = Seguimiento_Especifico(Entidad)

			msg_log = String.Format("{0} - {1} (Borrar):", BBDD.Connection.Database, Tabla_BBDD.ElementType.Name) _
				& If(String.IsNullOrWhiteSpace(Inf_Entidad), String.Empty, Inf_Entidad) _
				& vbCrLf & StrDup(60, "-")
		Next
		'---------------------------------------------------------------------------------------------------------------------
		For Each Entidad In lEntidades.Inserts
			Dim Tabla_BBDD As Data.Linq.ITable = BBDD.GetTable(Entidad.GetType())
			Dim modifiedMembers As Data.Linq.ModifiedMemberInfo() = Tabla_BBDD.GetModifiedMembers(Entidad)
			Dim Inf_Entidad As String = Seguimiento_Especifico(Entidad)

			msg_log = String.Format("{0} - {1} (Insertar):", BBDD.Connection.Database, Tabla_BBDD.ElementType.Name) _
				& If(String.IsNullOrWhiteSpace(Inf_Entidad), String.Empty, Inf_Entidad) _
				& vbCrLf & StrDup(60, "-")
		Next
		'---------------------------------------------------------------------------------------------------------------------
		For Each Entidad In lEntidades.Updates
			Dim Tabla_BBDD As Data.Linq.ITable = BBDD.GetTable(Entidad.GetType())
			Dim modifiedMembers As Data.Linq.ModifiedMemberInfo() = Tabla_BBDD.GetModifiedMembers(Entidad)
			'Dim original As Object = Tabla_BBDD.GetOriginalEntityState(Entidad)
			Dim Inf_Entidad As String = Seguimiento_Especifico(Entidad)

			msg_log = String.Format("{0} - {1} (Actualizacion):", BBDD.Connection.Database, Tabla_BBDD.ElementType.Name) _
				& If(String.IsNullOrWhiteSpace(Inf_Entidad), String.Empty, Inf_Entidad) _
				& vbCrLf & String.Join(vbCrLf, modifiedMembers.Select(Function(o) _
																		  String.Format("-{0} (Original): {1}" & vbCrLf & "-{0} " & "(Nuevo): ".PadLeft(12) & "{2}" _
																		  , o.Member.Name _
																		  , o.OriginalValue _
																		  , o.CurrentValue))) _
																		  & vbCrLf & StrDup(60, "-")
		Next
		'---------------------------------------------------------------------------------------------------------------------

		If Not String.IsNullOrWhiteSpace(msg_log) Then
			log.Info(vbCrLf & StrDup(60, "=") & vbCrLf & "Seguimiento de actividad (" & NombreFuncion & ")" & vbCrLf & StrDup(60, "=") _
								& vbCrLf & msg_log _
								& vbCrLf & StrDup(60, "="))
		End If

	End Sub

    ''' <summary>
    ''' Obtenemos informacion extra de entidades especificas para realizar su seguimiento.
    ''' </summary>
    ''' <param name="Entidad"></param>
    ''' <returns></returns>
    Private Function Seguimiento_Especifico(ByRef Entidad As Object) As String
        Seguimiento_Especifico = Nothing
        Dim IdReferencia As String = Nothing
        Dim CodOperacion As String = Nothing

        If (HttpContext.Current IsNot Nothing) Then
            IdReferencia = HttpContext.Current.Session("IdReferencia")
            CodOperacion = HttpContext.Current.Session("CodOperacion")
        End If

        If Entidad.GetType.Name.Equals("AMFES") Then
            Dim Obj_Entidad As Registro.AMFES = DirectCast(Entidad, Registro.AMFES)
            Seguimiento_Especifico = String.Format(vbCrLf & "- Articulo: {0}" _
                                                        & vbCrLf & "- Operacion: {1}" _
                                                        & vbCrLf & "- ID: {2}" _
                                                         , If(Obj_Entidad Is Nothing OrElse Obj_Entidad.OPERACIONES_TIPO Is Nothing OrElse Not Obj_Entidad.OPERACIONES_TIPO.OPERACIONES_DE_UN_ARTICULO.Any, If(String.IsNullOrWhiteSpace(IdReferencia), "?", IdReferencia), String.Join(", ", Obj_Entidad.OPERACIONES_TIPO.OPERACIONES_DE_UN_ARTICULO.Select(Function(o) o.CODIGO))) _
                                                         , If(Obj_Entidad Is Nothing OrElse Obj_Entidad.OPERACIONES_TIPO Is Nothing, If(String.IsNullOrWhiteSpace(CodOperacion), "?", CodOperacion), Obj_Entidad.OPERACIONES_TIPO.COD_OPERACION) _
                                                         , Obj_Entidad.ID)

        ElseIf Entidad.GetType.Name.Equals("CARACTERISTICAS_AMFE") Then
            Dim Obj_Entidad As Registro.CARACTERISTICAS_AMFE = DirectCast(Entidad, Registro.CARACTERISTICAS_AMFE)
            Seguimiento_Especifico = String.Format(vbCrLf & "- Articulo: {0}" _
                                                                & vbCrLf & "- Operacion: {1}" _
                                                                & vbCrLf & "- ID_CARACTERISTICA: {2}" _
                                                                & vbCrLf & "- CARACTERISTICA: {3}" _
                                                                 , If(Obj_Entidad.AMFES Is Nothing OrElse Obj_Entidad.AMFES.OPERACIONES_TIPO Is Nothing OrElse Not Obj_Entidad.AMFES.OPERACIONES_TIPO.OPERACIONES_DE_UN_ARTICULO.Any, If(String.IsNullOrWhiteSpace(IdReferencia), "?", IdReferencia), String.Join(", ", Obj_Entidad.AMFES.OPERACIONES_TIPO.OPERACIONES_DE_UN_ARTICULO.Select(Function(o) o.CODIGO))) _
                                                                 , If(Obj_Entidad.AMFES Is Nothing OrElse Obj_Entidad.AMFES.OPERACIONES_TIPO Is Nothing, If(String.IsNullOrWhiteSpace(CodOperacion), "?", CodOperacion), Obj_Entidad.AMFES.OPERACIONES_TIPO.COD_OPERACION) _
                                                                 , Obj_Entidad.ID_CARACTERISTICA _
                                                                 , Obj_Entidad.CARACTERISTICA)

        ElseIf Entidad.GetType.Name.Equals("CARACTERISTICAS_DEL_PLAN") Then
            Dim Obj_Entidad As Registro.CARACTERISTICAS_DEL_PLAN = DirectCast(Entidad, Registro.CARACTERISTICAS_DEL_PLAN)
            Seguimiento_Especifico = String.Format(vbCrLf & "- Articulo: {0}" _
                                                                & vbCrLf & "- Operacion: {1}" _
                                                                & vbCrLf & "- ID_REGISTRO: {2}" _
                                                                & vbCrLf & "- CARACTERISTICA: {3}" _
                                                                 , If(Obj_Entidad.PLAN_DE_CONTROL Is Nothing OrElse Obj_Entidad.PLAN_DE_CONTROL.OPERACIONES_TIPO Is Nothing OrElse Not Obj_Entidad.PLAN_DE_CONTROL.OPERACIONES_TIPO.OPERACIONES_DE_UN_ARTICULO.Any, If(String.IsNullOrWhiteSpace(IdReferencia), "?", IdReferencia), String.Join(", ", Obj_Entidad.PLAN_DE_CONTROL.OPERACIONES_TIPO.OPERACIONES_DE_UN_ARTICULO.Select(Function(o) o.CODIGO))) _
                                                                 , If(Obj_Entidad.PLAN_DE_CONTROL Is Nothing OrElse Obj_Entidad.PLAN_DE_CONTROL.OPERACIONES_TIPO Is Nothing, If(String.IsNullOrWhiteSpace(CodOperacion), "?", CodOperacion), Obj_Entidad.PLAN_DE_CONTROL.OPERACIONES_TIPO.COD_OPERACION) _
                                                                 , Obj_Entidad.ID_REGISTRO _
                                                                 , Obj_Entidad.CARAC_PARAM)
        End If
    End Function
End Class