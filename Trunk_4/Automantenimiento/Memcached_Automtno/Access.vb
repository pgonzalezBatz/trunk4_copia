Imports Oracle.DataAccess.Client

Public Class Keys
    Public Const LstUsuariosAlta As String = "LstUsuariosAlta"
    Public Const LstUsuariosAltaSimple As String = "LstUsuariosAltaSimple"
    Public Const Usuario As String = "Usuario"
    Public Const Departamentos As String = "Departamentos"
    Public Const Organigrama As String = "Organigrama"
    Public Const NumeroAbreviado As String = "NumeroAbreviado"
    Public Const Pais As String = "Pais"
End Class

<Obsolete("2013-02-28: Dejar de usar esta clase porque va ha desaparecer", False)> _
Public Class Access

	''' <summary>
	''' Devulve una lista de los numeros abreviados que se utilizan para los proveedores
	''' 0=id empresa de sab, 1=extension, 2=numero de proveedor xbat, 3=nombre de proveedor
	''' </summary>
	''' <param name="strCn"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetNumeroAbreviadoProveedor(ByVal strCn As String) As List(Of String())
		'Dim u = GetMemCacheInstance().Get(Keys.NumeroAbreviado)
		'If u Is Nothing Then
		Dim q = "select n.id_empresa,n.num_abreviado,e.idtroqueleria,e.nombre from numeros_abreviados n inner join sab.empresas e on n.id_empresa=e.id order by e.nombre"
		Dim u = Memcached.OracleDirectAccess.Seleccionar(q, strCn)
		'End If
		Return u
	End Function

	''' <summary>
	''' Devulve un lista con todos los usuarios de SAB que estan de alta.
	''' </summary>
	''' <returns>r(0)=ID, r(1)=IDEMPRESAS, r(2)=IDCULTURAS, r(3)=NOMBREUSUARIO, r(4)=IDDIRECTORIOACTIVO, 
	''' r(5)=CODPERSONA,r(6)=PWD, r(7)=FECHAALTA, r(8)=FECHABAJA, r(9)=APELLIDO1, r(10)=APELLIDO2, 
	''' r(11)=IDMATRIX, r(12)=IDFTP, r(13)=EMAIL, r(14)=IDDEPARTAMENTO, r(15)=NOMBRE</returns>
	''' <remarks></remarks>
	Public Shared Function GetListaUsuariosAlta(ByVal cnSAB As String) As List(Of String())
		Return OracleDirectAccess.Seleccionar("SELECT * FROM USUARIOS WHERE FECHABAJA IS NULL OR FECHABAJA>SYSDATE", cnSAB)
	End Function

	''' <summary>
	''' Devulve una lista simple con todos los usuarios que estan de alta en SAB
	''' </summary>
	''' <param name="cnSAB"></param>
	''' <returns>r(0)=ID, r(1)=NOMBREUSUARIO, r(2)=CODPERSONA</returns>
	''' <remarks></remarks>
	Public Shared Function GetListaUsuariosAltaSimple(ByVal cnSAB As String) As List(Of String())
		Return OracleDirectAccess.Seleccionar("SELECT ID, NOMBREUSUARIO, CODPERSONA FROM USUARIOS WHERE FECHABAJA IS NULL OR FECHABAJA>SYSDATE", cnSAB)
	End Function
	''' <summary>
	''' Devuelve un array con los detalles del usuario
	''' 0=ID, 1=IDEMPRESAS, 2=IDCULTURAS, 3=NOMBREUSUARIO, 4=IDDIRECTORIOACTIVO, 5=CODPERSONA,6=PWD, 7=FECHAALTA, 8=FECHABAJA, 9=APELLIDO1, 
	''' 10=APELLIDO2, 11=IDMATRIX, 12=IDFTP, 13=EMAIL, 14=IDDEPARTAMENTO, 15=NOMBRE
	''' </summary>
	''' <param name="idSAB"></param>
	''' <param name="cnSAB"></param>
	''' <returns>r(0)=ID, r(1)=IDEMPRESAS, r(2)=IDCULTURAS, r(3)=NOMBREUSUARIO, r(4)=IDDIRECTORIOACTIVO, 
	''' r(5)=CODPERSONA,r(6)=PWD, r(7)=FECHAALTA, r(8)=FECHABAJA, r(9)=APELLIDO1, r(10)=APELLIDO2, 
	''' r(11)=IDMATRIX, r(12)=IDFTP, r(13)=EMAIL, r(14)=IDDEPARTAMENTO, r(15)=NOMBRE, r(16)=foto,r(17)=dni</returns>
	''' <remarks></remarks>
	Public Shared Function GetUsuario(ByVal idSAB As Integer, ByVal cnSAB As String) As String()
		'Dim u = GetMemCacheInstance().Get(Keys.Usuario + idSAB.ToString())
		'If u Is Nothing Then
		Dim u = OracleDirectAccess.Seleccionar("SELECT * FROM USUARIOS WHERE ID=:ID", cnSAB, _
										 New OracleParameter(":ID", OracleDbType.Int32, idSAB, ParameterDirection.Input)).First()
		'End If
		Return u
	End Function
	Public Shared Function GetNombrePais(ByVal CodigoPais As Integer, ByVal strcn As String) As String
		'Dim u = GetMemCacheInstance().Get(Keys.Pais + CodigoPais.ToString())
		'If u Is Nothing Then
		Dim u = OracleDirectAccess.SeleccionarEscalar(Of String)("SELECT nompai FROM xbat.copais WHERE codpai=:codpai", strcn, _
										 New OracleParameter("codpai", OracleDbType.Int32, CodigoPais, ParameterDirection.Input))
		'End If
		Return u
	End Function

	''' <summary>
	''' Devuelve el organigrama completo de Batz
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetOrganigrama(ByVal cnEpsilon As String) As List(Of String())
		Dim query As String = "select id_nivel,d_nivel,nivel,niv_org.f_inhabilitacion,n1, n2, n3, n4, n5, n6, n7,n8 from orden inner join niv_org on " _
					+ "orden.id_organig=niv_org.id_organig and orden.id_nivel_hijo=niv_org.id_nivel " _
					+ "where orden.id_organig=00001"
		Return SQLServerDirectAccess.Seleccionar(query, cnEpsilon)
	End Function
	''' <summary>
	''' Devuelve toda la informacion de un departamento.
	''' 0=Id_nivel,1=descripcion,2=profuncidad nivel,3=fecha inabilitacion,4=Primer nivel,5=segundo nivel,6=tercer nivel,...
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <param name="IdDep">Identificador del departamento</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetOrganigrama(ByVal cnEpsilon As String, ByVal IdDep As String) As String()
		'Dim sDep As String() = GetMemCacheInstance().Get("IdDep_" & IdDep)
		'If sDep Is Nothing Then
		Dim query As String = "select id_nivel,d_nivel,nivel,niv_org.f_inhabilitacion,n1, n2, n3, n4, n5, n6, n7,n8 from orden inner join niv_org on " _
				+ "orden.id_organig=niv_org.id_organig and orden.id_nivel_hijo=niv_org.id_nivel " _
				+ "where orden.id_organig=00001 and id_nivel=@ID_DEP"
		Dim parameter As New SqlClient.SqlParameter("@ID_DEP", SqlDbType.VarChar, 5, ParameterDirection.Input)
		parameter.Value = IdDep
		Dim sDep As String() = SQLServerDirectAccess.Seleccionar(query, cnEpsilon, parameter).First()
		'GetMemCacheInstance().Store(Enyim.Caching.Memcached.StoreMode.Set, "IdDep_" & IdDep, sDep)
		'End If
		Return sDep
	End Function

	''' <summary>
	''' Devuelve la informacion de un departamento
	''' 0=Id_nivel,1=descripción departamento,2=descripción negocio, 3=id negocio
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <param name="IdDep">Identificador del departamento</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetOrganigramaNegocio(ByVal cnEpsilon As String, ByVal IdDep As String) As String()
		Dim query As String = "select id_nivel_hijo, no4.d_nivel, no.d_nivel, no.id_nivel " _
							  + "from orden o inner join niv_org no on o.n2=no.id_nivel and o.id_organig=no.id_organig " _
							  + "inner join niv_org no4 on o.n4=no4.id_nivel and o.id_organig=no4.id_organig " _
							  + "where o.id_organig='00001' and id_nivel_hijo=@ID_DEP"
		Dim parameter As New SqlClient.SqlParameter("@ID_DEP", SqlDbType.VarChar, 5, ParameterDirection.Input)
		parameter.Value = IdDep
		Return SQLServerDirectAccess.Seleccionar(query, cnEpsilon, parameter).First()
	End Function
	''' <summary>
	''' Devuelve una lista con los nombre de los negocios de Batz (Trokelgintza, Sistemak,...)
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetNegocios(ByVal cnEpsilon As String) As List(Of String())
		Return SQLServerDirectAccess.Seleccionar("select niv_org.d_nivel from niv_org inner join orden on niv_org.id_nivel=orden.id_nivel_hijo " _
										+ "and niv_org.id_organig=orden.id_organig where niv_org.id_organig='00001' and orden.nivel=1 " _
										+ "group by niv_org.d_nivel", cnEpsilon)
	End Function
	''' <summary>
	''' Devuelve una lista con los nombre de las profesiones de Batz (zerbitzu teknikaria, proiektu gestorea, ...)
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetProfesiones(ByVal cnEpsilon As String) As List(Of String())
		Return SQLServerDirectAccess.Seleccionar("select niv_org.d_nivel from niv_org inner join orden on niv_org.id_nivel=orden.id_nivel_hijo " _
										+ "and niv_org.id_organig=orden.id_organig where niv_org.id_organig='00001' and orden.nivel=4 " _
										+ "group by niv_org.d_nivel", cnEpsilon)
	End Function
	''' <summary>
	''' Devulve una lista con los niveles de Batz (1. maila,2. maila,...) en funcion a la profesion
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetNivelesProfesion(ByVal cnEpsilon As String, ByVal nombreProfesion As String) As List(Of String())
		Dim q = "select niv_org.d_nivel from niv_org inner join orden on niv_org.id_nivel=orden.id_nivel_hijo and niv_org.id_organig=" _
				+ "orden.id_organig  left outer join niv_org N5 on N5.id_nivel=Orden.n5 where niv_org.id_organig='00001' and orden.nivel=5 " _
				+ "and N5.d_nivel = @d_nivel group by niv_org.d_nivel"
		Dim p As New SqlClient.SqlParameter("@d_nivel", SqlDbType.Char, 60, ParameterDirection.Input)
		p.Value = nombreProfesion
		Return SQLServerDirectAccess.Seleccionar(q, cnEpsilon, p)
	End Function
	''' <summary>
	''' Devulve una lista con los niveles de Batz (1. maila,2. maila,...)
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetNivelesProfesion(ByVal cnEpsilon As String) As List(Of String())
		Dim q = "select niv_org.d_nivel from niv_org inner join orden on niv_org.id_nivel=orden.id_nivel_hijo and niv_org.id_organig=" _
				+ "orden.id_organig  left outer join niv_org N5 on N5.id_nivel=Orden.n5 where niv_org.id_organig='00001' and orden.nivel=5 " _
				+ "group by niv_org.d_nivel"
		Return SQLServerDirectAccess.Seleccionar(q, cnEpsilon)
	End Function
	''' <summary>
	''' Devuelve una lista con las especialidades de Batz
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetEspecialidades(ByVal cnEpsilon As String) As List(Of String())
		Return SQLServerDirectAccess.Seleccionar("select niv_org.d_nivel from niv_org inner join orden on niv_org.id_nivel=orden.id_nivel_hijo " _
										+ "and niv_org.id_organig=orden.id_organig where niv_org.id_organig='00001' and orden.nivel=6 " _
										+ "group by niv_org.d_nivel", cnEpsilon)
	End Function
	''' <summary>
	''' Devuelve una lista con las personas de Batz y el organigrama de cada uno de ell@s
	''' 0=Numero de trabajador, 1=Negocio, 2=Puesto de trabajo, 3=Nivel, 4=Especialidad
	''' </summary>
	''' <param name="cnEpsilon"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetPuestosPersonas(ByVal cnEpsilon As String) As List(Of String())
		Dim q = "select  PT.id_trabajador, N2.d_nivel,N5.d_nivel,N6.d_nivel,N7.d_nivel from pues_trab PT inner join orden O on " _
			+ "PT.id_nivel=O.id_nivel_hijo and PT.id_organig=O.id_organig left outer join niv_org N2 on N2.id_nivel=O.N2 and " _
			+ "N2.id_organig=O.id_organig left outer join niv_org N5 on N5.id_nivel=O.N5 and N5.id_organig=O.id_organig left outer join " _
			+ "niv_org N6 on N6.id_nivel=O.N6 and N6.id_organig=O.id_organig left outer join niv_org N7 on N7.id_nivel=O.N7 and " _
			+ "N7.id_organig=O.id_organig	where O.id_organig='00001' and PT.f_fin_pue is null"
		Return SQLServerDirectAccess.Seleccionar(q, cnEpsilon)
	End Function

	Public Shared Function GetIdiomas(ByVal cnSab As String) As List(Of String())
		Return OracleDirectAccess.Seleccionar("select * from culturas", cnSab)
	End Function
End Class
