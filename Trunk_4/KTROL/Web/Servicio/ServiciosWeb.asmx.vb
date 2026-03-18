Imports System.Web.Services
Imports System.Web.Services.Protocols
Imports System.ComponentModel

' Para permitir que se llame a este servicio Web desde un script, usando ASP.NET AJAX, quite la marca de comentario de la línea siguiente.
<System.Web.Script.Services.ScriptService()> _
<System.Web.Services.WebService(Namespace:="http://tempuri.org/")> _
<System.Web.Services.WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<ToolboxItem(False)> _
Public Class ServiciosWeb
	Inherits System.Web.Services.WebService
    'Public log As log4net.ILog = log4net.LogManager.GetLogger("root.KaPlan")

	<WebMethod()> _
	Public Function HelloWorld() As String
		Return "Hola a todos"
	End Function
	<WebMethod()> _
	Public Function BuscarReferencias(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Try
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
			Dim aPrefixText As String() = prefixText.Trim.Split(" ")
			'------------------------------------------------------------------------------------
			'Like es por defecto insensitivo.
			'------------------------------------------------------------------------------------
			Dim MAESTRO_ARTICULOS As IQueryable(Of KaPlanLib.Registro.MAESTRO_ARTICULOS) = From Reg As KaPlanLib.Registro.MAESTRO_ARTICULOS In BBDD.MAESTRO_ARTICULOS Select Reg
			For Each Texto As String In aPrefixText
				Dim TextoLike As String = Sablib.BLL.Utils.TextoLike(Texto)
				MAESTRO_ARTICULOS = From Reg As KaPlanLib.Registro.MAESTRO_ARTICULOS In MAESTRO_ARTICULOS _
						Where Reg.CODIGO Like "*" & TextoLike & "*" _
						Or Reg.DENOMINACION Like "*" & TextoLike & "*" _
						Or Reg.FAMILIA Like "*" & TextoLike & "*" _
						Or Reg.REFERENCIA_CLIENTE Like "*" & TextoLike & "*" _
						Or Reg.JEFE_DE_PROYECTO Like "*" & TextoLike & "*" _
						Or Reg.NIVEL Like "*" & TextoLike & "*" _
						Or Reg.PLANO Like "*" & TextoLike & "*" _
						Or Reg.CLIENTE Like "*" & TextoLike & "*" _
						Or Reg.VEHICULO Like "*" & TextoLike & "*" _
						Or Reg.FECHA_SERIE Like "*" & TextoLike & "*" _
						Or Reg.ORGANO Like "*" & TextoLike & "*" _
						Or Reg.OBJETIVO Like "*" & TextoLike & "*" _
						Or Reg.ESLABON Like "*" & TextoLike & "*" _
						Or Reg.LANTEGI Like "*" & TextoLike & "*" _
						Select Reg Distinct Order By Reg.CODIGO
			Next
			BuscarReferencias = MAESTRO_ARTICULOS.Select(Function(o) o.CODIGO.Trim).ToArray
			'------------------------------------------------------------------------------------
		Catch ex As Exception
            'log.Error(ex)
			BuscarReferencias = Nothing
		End Try
		Return BuscarReferencias
	End Function
    '<WebMethod()> _
    'Public Function BuscarOperaciones(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
    '	Try
    '		'--------------------------------------------------------------------------------------------------------------------
    '		'Dim aPrefixText As String() = prefixText.Trim.Split(" ")
    '		'Dim Parametros As Array = contextKey.Split("|")
    '		'Dim lObjetos As Object = New KaPlanLib.BLL.LinqComponent().consultarListadoCodigosOperacion(Parametros(1).ToString, Nothing, Nothing, Parametros(0).ToString)
    '		'Dim Lista As New List(Of String)

    '		'If lObjetos Is Nothing OrElse lObjetos.Count = 0 Then
    '		'	BuscarOperaciones = Nothing
    '		'Else
    '		'	For Each Texto As String In aPrefixText
    '		'		Dim TextoLike As String = Sablib.BLL.Utils.TextoLike(Texto)	'Patron de la expresion regular.
    '		'		Dim RegExp As New Regex(TextoLike, RegexOptions.IgnoreCase)
    '		'		For Each Reg As Object In lObjetos
    '		'			Dim regTexto As String = Reg.COD_OPERACION & " " & Reg.OPERACION_GENERAL & " " & Reg.OPERACION_TIPO
    '		'			If RegExp.Match(regTexto).Success = True Then Lista.Add(Reg.COD_OPERACION.trim)
    '		'		Next
    '		'	Next
    '		'	BuscarOperaciones = Lista.ToArray
    '		'End If
    '		'--------------------------------------------------------------------------------------------------------------------
    '		'FROGA:2013-01-03: Cambiamos el sistema de busqueda para que no de error al buscar caracteres como "(".
    '		'--------------------------------------------------------------------------------------------------------------------
    '		Dim aPrefixText As String() = prefixText.Trim.Split(" ")
    '		Dim Parametros As Array = contextKey.Split("|")
    '		Dim lObjetos As Object = New KaPlanLib.BLL.LinqComponent().consultarListadoCodigosOperacion(Parametros(1).ToString, Nothing, Nothing, Parametros(0).ToString)
    '		Dim ListaObjetos As New List(Of Object)
    '		Dim lCOD_OPERACION As New List(Of String)

    '		If lObjetos Is Nothing OrElse lObjetos.Count = 0 Then
    '			BuscarOperaciones = Nothing
    '		Else
    '			'-------------------------------------------------------------------------------------
    '			'Transformamos el resultado en una lista para poder manipularla con mas facilidad.
    '			'-------------------------------------------------------------------------------------
    '			For Each Reg As Object In lObjetos
    '				ListaObjetos.Add(Reg)
    '			Next
    '			'-------------------------------------------------------------------------------------
    '			For Each Texto As String In aPrefixText
    '				Dim TextoLike As String = Sablib.BLL.Utils.TextoLike(Texto)	'Patron de la expresion regular.
    '                   ListaObjetos = (From reg As Object In ListaObjetos _
    '                              Where reg.COD_OPERACION Like "*" & TextoLike & "*" _
    '                               Or reg.OPERACION_GENERAL Like "*" & TextoLike & "*" _
    '                               Or reg.OPERACION_TIPO Like "*" & TextoLike & "*" _
    '                               Select reg).ToList
    '			Next
    '			ListaObjetos.ForEach(Sub(o) lCOD_OPERACION.Add(o.COD_OPERACION.trim))
    '			BuscarOperaciones = lCOD_OPERACION.ToArray
    '		End If
    '		'--------------------------------------------------------------------------------------------------------------------
    '	Catch ex As Exception
    '           'log.Error(ex)
    '		BuscarOperaciones = Nothing
    '	End Try
    '	Return BuscarOperaciones
    '   End Function

    <WebMethod()> _
       Public Function BuscarOperaciones(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
        Try            
            '--------------------------------------------------------------------------------------------------------------------
            'FROGA:2013-01-03: Cambiamos el sistema de busqueda para que no de error al buscar caracteres como "(".
            '--------------------------------------------------------------------------------------------------------------------
            Dim aPrefixText As String() = prefixText.Trim.Split(" ")
            Dim Parametros As Array = contextKey.Split("|")
            Dim lObjetos As Object = New KaPlanLib.BLL.LinqComponent().consultarListadoCodigosOperacion(Parametros(1).ToString, Nothing, Nothing, Parametros(0).ToString)
            Dim ListaObjetos As New List(Of Object)
            Dim lCOD_OPERACION As New List(Of String)

            If lObjetos Is Nothing OrElse lObjetos.Count = 0 Then
                BuscarOperaciones = Nothing
            Else
                '-------------------------------------------------------------------------------------
                'Transformamos el resultado en una lista para poder manipularla con mas facilidad.
                '-------------------------------------------------------------------------------------
                For Each Reg As Object In lObjetos
                    ListaObjetos.Add(Reg)
                Next
                '-------------------------------------------------------------------------------------
                For Each Texto As String In aPrefixText
                    Dim TextoLike As String = Sablib.BLL.Utils.TextoLike(Texto) 'Patron de la expresion regular.
                    ListaObjetos = (From reg As Object In ListaObjetos _
                               Where reg.COD_OPERACION Like "*" & TextoLike & "*" _
                                Or reg.OPERACION_GENERAL Like "*" & TextoLike & "*" _
                                Or reg.OPERACION_TIPO Like "*" & TextoLike & "*" _
                                Select reg).ToList
                Next
                ListaObjetos.ForEach(Sub(o) lCOD_OPERACION.Add(o.COD_OPERACION.trim))
                'Dim listaFinal As String() = lCOD_OPERACION.Distinct()
                'BuscarOperaciones = lCOD_OPERACION.ToArray
                BuscarOperaciones = lCOD_OPERACION.Distinct().ToArray
            End If
            '--------------------------------------------------------------------------------------------------------------------
        Catch ex As Exception
            'log.Error(ex)
            BuscarOperaciones = Nothing
        End Try
        Return BuscarOperaciones
    End Function

	''' <summary>
	''' Obtenemos las "Causas de Fallo" del Maestro "MAESTRO_DE_CAUSAS_DE_FALLO.CAUSA" y de "CAUSAS_DE_FALLO.CAUSA"
	''' </summary>
	''' <param name="prefixText">Texto a buscar</param>
	''' <param name="count">Nº de elementos que se muestran de la busqueda. "0" muestra todos los elementos.</param>
	''' <param name="contextKey">Conexion a la base de datos.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<WebMethod()> _
	Public Function BuscarCausas(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim aCausasFallo As String()
		Try
			'log.Debug("BuscarCausas (""" & prefixText & """) - INICIO")
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
			'Dim Funciones As New GertakariakLib2.Funciones

			Dim aPrefixText As String() = prefixText.Split(" ")
			'Obtenemos los tipos de fallos del maestro.
			Dim MAESTRO_DE_CAUSAS_DE_FALLO As List(Of String) = (From MCausaF In BBDD.MAESTRO_DE_CAUSAS_DE_FALLO Where MCausaF.CAUSA IsNot Nothing And MCausaF.CAUSA.Trim IsNot String.Empty _
																 Select MCausaF.CAUSA Distinct Order By CAUSA).ToList
			'Obtenemos los tipos de fallos de las causas pq algunas causas pueden diferir del maestro al ser texto libre.
			Dim CAUSAS_DE_FALLO As List(Of String) = (From CausaF In BBDD.CAUSAS_DE_FALLO Where CausaF.CAUSA IsNot Nothing And CausaF.CAUSA.Trim IsNot String.Empty _
													  Select CausaF.CAUSA Distinct Order By CAUSA).ToList
			'Juntamos todas las causas en una sola lista
			Dim CausasFallo As List(Of String) = (MAESTRO_DE_CAUSAS_DE_FALLO.Union(CAUSAS_DE_FALLO).Distinct).ToList

			For Each Texto As String In aPrefixText
				Dim cfTXT As String = Texto
				CausasFallo = (From cf As String In CausasFallo Select cf Distinct _
					Where cf.ToLower Like "*" & Sablib.BLL.Utils.TextoLike(cfTXT).ToLower & "*" Order By cf).ToList
			Next
			aCausasFallo = CausasFallo.ToArray
			'log.Debug("BuscarCausas (""" & prefixText & """) - FIN")
			'Return CausasFallo.ToArray
		Catch ex As Exception
            'log.Error("WebMethod()_ BuscarCausas", ex)
			'Return Nothing
			aCausasFallo = Nothing
		End Try
		Return aCausasFallo
	End Function

	''' <summary>
	''' Obtenemos las "Causas de Fallo" del Maestro "M_FRECUENCIA_CONTROL.FRECUENCIA_CONTROL" y de "CAUSAS_DE_FALLO.FRECUENCIA_DE_CONTROL"
	''' </summary>
	''' <param name="prefixText">Texto a buscar</param>
	''' <param name="count">Nº de elementos que se muestran de la busqueda. "0" muestra todos los elementos.</param>
	''' <param name="contextKey">Conexion a la base de datos.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<WebMethod()> _
	Public Function Buscar_FRECUENCIA_CONTROL(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim aRegistros As String()
		Try
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
			Dim aPrefixText As String() = prefixText.Split(" ")
			'Obtenemos los registros del Maestro.
			Dim Tabla1 As List(Of String) = (From Reg In BBDD.M_FRECUENCIA_CONTROL Where Reg.FRECUENCIA_CONTROL IsNot Nothing And Reg.FRECUENCIA_CONTROL.Trim IsNot String.Empty _
																 Select Reg.FRECUENCIA_CONTROL Distinct Order By FRECUENCIA_CONTROL).ToList
			'Obtenemos los registros.
			Dim Tabla2 As List(Of String) = (From Reg In BBDD.CAUSAS_DE_FALLO Where Reg.FRECUENCIA_DE_CONTROL IsNot Nothing And Reg.FRECUENCIA_DE_CONTROL.Trim IsNot String.Empty _
													  Select Reg.FRECUENCIA_DE_CONTROL Distinct Order By FRECUENCIA_DE_CONTROL).ToList
			'Juntamos todas las causas en una sola lista
			Dim TablasUnidas As List(Of String) = (Tabla1.Union(Tabla2).Distinct).ToList

			For Each Texto As String In aPrefixText
				Dim cfTXT As String = Texto
				TablasUnidas = (From Reg As String In TablasUnidas Select Reg Distinct _
					Where Reg.ToLower Like "*" & Sablib.BLL.Utils.TextoLike(cfTXT).ToLower & "*" Order By Reg).ToList
			Next
			aRegistros = TablasUnidas.ToArray
		Catch ex As Exception
            'log.Error("WebMethod()_ Buscar_FRECUENCIA_CONTROL", ex)
			aRegistros = Nothing
		End Try
		Return aRegistros
	End Function
	''' <summary>
	''' Obtenemos las "Causas de Fallo" del Maestro "M_FRECUENCIA_CONTROL.FRECUENCIA_CONTROL" y de "CAUSAS_DE_FALLO.FRECUENCIA_DE_CONTROL_CAL"
	''' </summary>
	''' <param name="prefixText">Texto a buscar</param>
	''' <param name="count">Nº de elementos que se muestran de la busqueda. "0" muestra todos los elementos.</param>
	''' <param name="contextKey">Conexion a la base de datos.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<WebMethod()> _
	Public Function Buscar_FRECUENCIA_DE_CONTROL_CAL(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim aRegistros As String()
		Try
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
			Dim aPrefixText As String() = prefixText.Split(" ")
			'Obtenemos los registros del Maestro.
			Dim Tabla1 As List(Of String) = (From Reg In BBDD.M_FRECUENCIA_CONTROL Where Reg.FRECUENCIA_CONTROL IsNot Nothing And Reg.FRECUENCIA_CONTROL.Trim IsNot String.Empty _
																 Select Reg.FRECUENCIA_CONTROL Distinct Order By FRECUENCIA_CONTROL).ToList
			'Obtenemos los registros.
			Dim Tabla2 As List(Of String) = (From Reg In BBDD.CAUSAS_DE_FALLO Where Reg.FRECUENCIA_DE_CONTROL_CAL IsNot Nothing And Reg.FRECUENCIA_DE_CONTROL_CAL.Trim IsNot String.Empty _
													  Select Reg.FRECUENCIA_DE_CONTROL_CAL Distinct Order By FRECUENCIA_DE_CONTROL_CAL).ToList
			'Juntamos todas las causas en una sola lista
			Dim TablasUnidas As List(Of String) = (Tabla1.Union(Tabla2).Distinct).ToList

			For Each Texto As String In aPrefixText
				Dim cfTXT As String = Texto
				TablasUnidas = (From Reg As String In TablasUnidas Select Reg Distinct _
					Where Reg.ToLower Like "*" & Sablib.BLL.Utils.TextoLike(cfTXT).ToLower & "*" Order By Reg).ToList
			Next
			aRegistros = TablasUnidas.ToArray
		Catch ex As Exception
            'log.Error("WebMethod()_ Buscar_FRECUENCIA_DE_CONTROL_CAL", ex)
			aRegistros = Nothing
		End Try
		Return aRegistros
	End Function

	''' <summary>
	''' Obtenemos las "Causas de Fallo" del Maestro "M_FRECUENCIA_CONTROL.FRECUENCIA_CONTROL" y de "CARACTERISTICAS_DEL_PLAN.FRECUENCIA_DE_CONTROL"
	''' </summary>
	''' <param name="prefixText">Texto a buscar</param>
	''' <param name="count">Nº de elementos que se muestran de la busqueda. "0" muestra todos los elementos.</param>
	''' <param name="contextKey">Conexion a la base de datos.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<WebMethod()> _
	Public Function Buscar_FRECUENCIA_CONTROL_PC(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim aRegistros As String()
		Try
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
			Dim aPrefixText As String() = prefixText.Split(" ")
			'Obtenemos los registros del Maestro.
			Dim Tabla1 As List(Of String) = (From Reg In BBDD.M_FRECUENCIA_CONTROL Where Reg.FRECUENCIA_CONTROL IsNot Nothing And Reg.FRECUENCIA_CONTROL.Trim IsNot String.Empty _
																 Select Reg.FRECUENCIA_CONTROL Distinct Order By FRECUENCIA_CONTROL).ToList
			'Obtenemos los registros.
			Dim Tabla2 As List(Of String) = (From Reg In BBDD.CARACTERISTICAS_DEL_PLAN Where Reg.FRECUENCIA_CONTROL IsNot Nothing And Reg.FRECUENCIA_CONTROL.Trim IsNot String.Empty _
													  Select Reg.FRECUENCIA_CONTROL Distinct Order By FRECUENCIA_CONTROL).ToList
			'Juntamos todas las causas en una sola lista
			Dim TablasUnidas As List(Of String) = (Tabla1.Union(Tabla2).Distinct).ToList

			For Each Texto As String In aPrefixText
				Dim cfTXT As String = Texto
				TablasUnidas = (From Reg As String In TablasUnidas Select Reg Distinct _
					Where Reg.ToLower Like "*" & Sablib.BLL.Utils.TextoLike(cfTXT).ToLower & "*" Order By Reg).ToList
			Next
			aRegistros = TablasUnidas.ToArray
		Catch ex As Exception
            'log.Error("WebMethod()_ Buscar_FRECUENCIA_CONTROL_PC", ex)
			aRegistros = Nothing
		End Try
		Return aRegistros
	End Function
	''' <summary>
	''' Obtenemos las "Causas de Fallo" del Maestro "M_FRECUENCIA_CONTROL.FRECUENCIA_CONTROL" y de "CARACTERISTICAS_DEL_PLAN.FRECUENCIA_CONTROL_CAL"
	''' </summary>
	''' <param name="prefixText">Texto a buscar</param>
	''' <param name="count">Nº de elementos que se muestran de la busqueda. "0" muestra todos los elementos.</param>
	''' <param name="contextKey">Conexion a la base de datos.</param>
	''' <returns></returns>
	''' <remarks></remarks>
	<WebMethod()> _
	Public Function Buscar_FRECUENCIA_CONTROL_CAL_PC(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim aRegistros As String()
		Try
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
			Dim aPrefixText As String() = prefixText.Split(" ")
			'Obtenemos los registros del Maestro.
			Dim Tabla1 As List(Of String) = (From Reg In BBDD.M_FRECUENCIA_CONTROL Where Reg.FRECUENCIA_CONTROL IsNot Nothing And Reg.FRECUENCIA_CONTROL.Trim IsNot String.Empty _
																 Select Reg.FRECUENCIA_CONTROL Distinct Order By FRECUENCIA_CONTROL).ToList
			'Obtenemos los registros.
			Dim Tabla2 As List(Of String) = (From Reg In BBDD.CARACTERISTICAS_DEL_PLAN Where Reg.FRECUENCIA_CONTROL_CAL IsNot Nothing And Reg.FRECUENCIA_CONTROL_CAL.Trim IsNot String.Empty _
													  Select Reg.FRECUENCIA_CONTROL_CAL Distinct Order By FRECUENCIA_CONTROL_CAL).ToList
			'Juntamos todas las causas en una sola lista
			Dim TablasUnidas As List(Of String) = (Tabla1.Union(Tabla2).Distinct).ToList

			For Each Texto As String In aPrefixText
				Dim cfTXT As String = Texto
				TablasUnidas = (From Reg As String In TablasUnidas Select Reg Distinct _
					Where Reg.ToLower Like "*" & Sablib.BLL.Utils.TextoLike(cfTXT).ToLower & "*" Order By Reg).ToList
			Next
			aRegistros = TablasUnidas.ToArray
		Catch ex As Exception
            'log.Error("WebMethod()_ Buscar_FRECUENCIA_CONTROL_CAL_PC", ex)
			aRegistros = Nothing
		End Try
		Return aRegistros
	End Function
	<WebMethod()> _
	Public Function BuscarFamilias(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Dim Textos As String()
		Try
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
            'Dim Funciones As New GertakariakLib2.Funciones

			Dim aPrefixText As String() = prefixText.Split(" ")
			'Obtenemos los tipos de Familias del maestro.
			Dim MaestroFamilias As List(Of String) = (From Familia In BBDD.Familia Where Familia.FAMILIA IsNot Nothing And Familia.FAMILIA.Trim IsNot String.Empty _
													  Select Familia.FAMILIA Distinct Order By FAMILIA).ToList
			'Obtenemos los tipos de Familas del "Maestro de Articulos" pq algunas pueden diferir del maestro al ser texto libre.
			Dim ArticulosFamilias As List(Of String) = (From Articulo In BBDD.MAESTRO_ARTICULOS Where Articulo.FAMILIA IsNot Nothing And Articulo.FAMILIA.Trim IsNot String.Empty _
														Select Articulo.FAMILIA Distinct Order By FAMILIA).ToList
			'Juntamos todas las Familias en una sola lista
			Dim lTextos As List(Of String) = (MaestroFamilias.Union(ArticulosFamilias).Distinct).ToList

			For Each Texto As String In aPrefixText
				Dim cfTXT As String = Texto
				lTextos = (From txt As String In lTextos Select txt Distinct _
					Where txt Like "*" & Sablib.BLL.Utils.TextoLike(cfTXT) & "*" Order By txt).ToList
			Next

			Textos = lTextos.ToArray
		Catch ex As Exception
            'log.Error("WebMethod()_ BuscarFamilias", ex)
			Textos = Nothing
		End Try
		Return Textos
	End Function

#Region "Buscador de Registro de Recepciones"
	''' <summary>
	''' Bucamos articulos (R_M_ARTICULOS) relacionados con alguna recepcion (R_RECEPCIONES).
	''' </summary>
	''' <param name="prefixText">Texto a buscar</param>
	''' <param name="count"></param>
	''' <param name="contextKey">Conexion a la base de datos.</param>
	''' <returns>Lista de elementos.</returns>
	''' <remarks></remarks>
	<WebMethod()> _
	Public Function BuscarArticulosRecepciones(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Try
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
			Dim Lista As New List(Of String)
			Dim aPrefixText As String() = prefixText.Trim.Split(" ")

			'---------------------------------------------------------------------------------------------------------------------
			'Si defininos solo los campos que necesitamos, en vez de devolver el objeto completo, la consulata tarda mucho menos.
			'---------------------------------------------------------------------------------------------------------------------
			Dim l_R_M_ARTICULOS = _
				From Recepciones In BBDD.R_RECEPCIONES _
				Group Join Prov In BBDD.R_M_PROVEEDORES On Recepciones.COD_PROVEEDOR Equals Prov.COD_PROVEEDOR Into Proveedor = Group From Prov In Proveedor.DefaultIfEmpty _
				Group Join Art In BBDD.R_M_ARTICULOS On Recepciones.COD_ARTICULO Equals Art.CODIGO Into Articulo = Group From Art In Articulo.DefaultIfEmpty _
				Where Recepciones.TIPO = "R" Select New With {.CODIGO = Art.CODIGO, .DENOMINACION = Art.DENOMINACION} Distinct
			'-----------------------------------------------------------------------------------------------------------------------

			For Each Texto As String In aPrefixText
				Dim TextoLike As String = Sablib.BLL.Utils.TextoLike(Texto)
				l_R_M_ARTICULOS = From Reg In l_R_M_ARTICULOS Where Reg.CODIGO Like "*" & TextoLike & "*" Or Reg.DENOMINACION Like "*" & TextoLike & "*" Select Reg Distinct
			Next
			If l_R_M_ARTICULOS IsNot Nothing AndAlso l_R_M_ARTICULOS.Count > 0 Then
				'Ordenamos despues de haber obtenido todos los resultados para obtimizar el tiempo.
				l_R_M_ARTICULOS = l_R_M_ARTICULOS.OrderBy(Function(o) o.CODIGO)
				For Each Art In l_R_M_ARTICULOS
					Lista.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem( _
							  If(String.IsNullOrEmpty(Art.CODIGO), String.Empty, Art.CODIGO.Trim) & " - " & If(String.IsNullOrEmpty(Art.DENOMINACION), String.Empty, Art.DENOMINACION.Trim), _
							  If(String.IsNullOrEmpty(Art.CODIGO), String.Empty, Art.CODIGO.Trim)))
				Next
			Else
				Lista.Add("")
			End If

			Return Lista.ToArray()
		Catch ex As Exception
            'log.Error(ex)
			BuscarArticulosRecepciones = Nothing
		End Try
	End Function

	''' <summary>
	''' Bucamos Proveedores (R_M_PROVEEDORES) relacionados con alguna recepcion (R_RECEPCIONES).
	''' </summary>
	''' <param name="prefixText">Texto a buscar</param>
	''' <param name="count"></param>
	''' <param name="contextKey">Conexion a la base de datos.</param>
	''' <returns>Lista de elementos.</returns>
	''' <remarks></remarks>
	<WebMethod()> _
	Public Function BuscarProveedoresRecepciones(ByVal prefixText As String, ByVal count As Integer, ByVal contextKey As String) As String()
		Try
			Dim BBDD As New KaPlanLib.DAL.ELL(contextKey)
			Dim Lista As New List(Of String)
			Dim aPrefixText As String() = prefixText.Trim.Split(" ")

			'---------------------------------------------------------------------------------------------------------------------
			'Si defininos solo los campos que necesitamos, en vez de devolver el objeto completo, la consulata tarda mucho menos.
			'---------------------------------------------------------------------------------------------------------------------
			Dim l_R_M_PROVEEDORES = From Recepciones In BBDD.R_RECEPCIONES _
							Group Join Prov In BBDD.R_M_PROVEEDORES On Recepciones.COD_PROVEEDOR Equals Prov.COD_PROVEEDOR Into Proveedor = Group From Prov In Proveedor.DefaultIfEmpty _
							Group Join Art In BBDD.R_M_ARTICULOS On Recepciones.COD_ARTICULO Equals Art.CODIGO Into Articulo = Group From Art In Articulo.DefaultIfEmpty _
							Where Recepciones.TIPO = "R" And Prov IsNot Nothing _
							Select New With {.ID_PROVEEDOR = Prov.ID_PROVEEDOR, .COD_PROVEEDOR = Prov.COD_PROVEEDOR, .DES_PROVEEDOR = If(Prov.DES_PROVEEDOR Is Nothing, Nothing, Prov.DES_PROVEEDOR.Trim)} Distinct
			'---------------------------------------------------------------------------------------------------------------------
			For Each Texto As String In aPrefixText
				Dim TextoLike As String = Sablib.BLL.Utils.TextoLike(Texto)
				l_R_M_PROVEEDORES = From Reg In l_R_M_PROVEEDORES _
									Where Reg.COD_PROVEEDOR Like "*" & TextoLike & "*" Or Reg.DES_PROVEEDOR Like "*" & TextoLike & "*" _
									Select Reg
			Next
			If l_R_M_PROVEEDORES IsNot Nothing AndAlso l_R_M_PROVEEDORES.Count > 0 Then
				'Ordenamos despues de haber obtenido todos los resultados para obtimizar el tiempo.
				l_R_M_PROVEEDORES = l_R_M_PROVEEDORES.OrderBy(Function(o) o.DES_PROVEEDOR)
				For Each Reg In l_R_M_PROVEEDORES
					Lista.Add(AjaxControlToolkit.AutoCompleteExtender.CreateAutoCompleteItem( _
							  If(String.IsNullOrEmpty(Reg.COD_PROVEEDOR), String.Empty, Reg.COD_PROVEEDOR.Trim) & " - " & If(String.IsNullOrEmpty(Reg.DES_PROVEEDOR), String.Empty, Reg.DES_PROVEEDOR.Trim), _
								Reg.COD_PROVEEDOR))
				Next
			Else
				Lista.Add("")
			End If
			'-----------------------------------------------------------------------------------------------------------------------

			Return Lista.ToArray()
		Catch ex As Exception
            'log.Error(ex)
			BuscarProveedoresRecepciones = Nothing
		End Try
	End Function
#End Region
End Class