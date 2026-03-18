Imports System.Runtime.CompilerServices
Imports System.Web.UI.WebControls
Imports System.Reflection

'Imports System.Linq
'Imports System.Linq.Enumerable
'Imports System.Linq.Queryable
'Imports System.Linq.Expressions
'Imports System.Linq.Expressions.Expression(Of Object)


Public Module WebControlExtension
	<Extension()> _
	Public Sub Ordenar(ByRef GridView As GridView, Optional ByVal CampoOrden As String = Nothing, Optional ByVal DireccionCampo As SortDirection = SortDirection.Ascending)
		'-----------------------------------------------------------------------------------------------------------------------
		'1ª Solucion.
		'A mejorar:
		'- Poder asignar el "GridView.DataSource" directamente a una lista o poder tratar el "GridView.DataSource" como una lista.
		'	Evitando el "FOR EACH" para rellenar la lista.
		'- Obtener directamete el tipo desde el "GridView.DataSource" o desde la lista sin tener que ir a uno de sus elementos.
		'-----------------------------------------------------------------------------------------------------------------------
		Dim ListaObjetos As New List(Of Object)
		If GridView.DataSource IsNot Nothing AndAlso GridView.DataSource.Count > 0 Then
			For Each Objeto As Object In GridView.DataSource
				ListaObjetos.Add(Objeto)
			Next

			'Obtenemos el Tipo de Objeto de los elementos de la lista.
			Dim TipoObjeto As Type = ListaObjetos.First.GetType
			'Obtenemos las propiedades del objeto
			Dim Propiedades As List(Of PropertyInfo) = TipoObjeto.GetProperties.ToList
			'Obtenemos la propiedad por la que se quiere ordenar.
			Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) StrComp(pi.Name, CampoOrden.Trim, CompareMethod.Text) = 0)

			If Propiedad IsNot Nothing Then
				If DireccionCampo = SortDirection.Ascending Then
					ListaObjetos = ListaObjetos.OrderBy(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing)).ToList
				ElseIf DireccionCampo = SortDirection.Descending Then
					ListaObjetos = ListaObjetos.OrderByDescending(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing)).ToList
				End If
				GridView.DataSource = ListaObjetos
			End If
		End If
		'-----------------------------------------------------------------------------------------------------------------------
		'-----------------------------------------------------------------------------------------------------------------------
		'Dim ListaSucesos As List(Of gtkMatenimientoSist) = GridView.DataSource 'Intentar asignar a una lista generica los objetos del DataSource para que se pueda utilizar con cualquier objeto.
		'Dim gtk As New gtkMatenimientoSist 'Intentar sacar de la lista de objetos el tipo que es.
		'Dim Propiedades As List(Of System.Reflection.PropertyInfo) = gtk.GetType.GetProperties.ToList
		'Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) StrComp(pi.Name, CampoOrden.Trim, CompareMethod.Text) = 0)
		'If Propiedad IsNot Nothing Then
		'	If DireccionCampo = SortDirection.Ascending Then
		'		ListaSucesos = ListaSucesos.OrderBy(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing)).ToList
		'	ElseIf DireccionCampo = SortDirection.Descending Then
		'		'------------------------------------------------------------------------
		'		'Dim ListaSucesos2
		'		'ListaSucesos2 = Objeto.DataSource.OrderByDescending(Function(o) o.Id)
		'		'------------------------------------------------------------------------

		'		'------------------------------------------------------------------------
		'		'Dim ListaSucesos2
		'		'ListaSucesos2 = ListaSucesos.OrderByDescending(Function(o) o.Id) 'SI
		'		'ListaSucesos2 = Objeto.DataSource.OrderByDescending(Function(o) o.Id) 'NO
		'		'Objeto.DataSource.OrderByDescending(Function(o) o.Id) 'NO
		'		'ListaSucesos2 = Objeto.DataSource.OrderByDescending(Function(o As gtkMatenimientoSist) o.Id) 'NO
		'		'------------------------------------------------------------------------
		'		'NO
		'		'------------------------------------------------------------------------
		'		'Dim ListaSucesos3 As List(Of gtkMatenimientoSist)
		'		'ListaSucesos3 = ListaSucesos.OrderByDescending(Function(o) o.Id)
		'		'------------------------------------------------------------------------
		'		'------------------------------------------------------------------------
		'		'Dim query As IEnumerable(Of Pet) =  pets.AsQueryable().OrderBy(Function(pet) pet.Age)
		'		'Dim ListaSucesos4 As IEnumerable(Of gtkMatenimientoSist) = Objeto.DataSource.asqueryable.orderby(Function(o) o.Id) 'NO
		'		'Dim ListaSucesos4 As IEnumerable(Of gtkMatenimientoSist) = ListaSucesos.OrderBy(Function(o) o.Id) 'SI
		'		'Dim ListaSucesos4 As List(Of gtkMatenimientoSist) = ListaSucesos.OrderBy(Function(o) o.Id) 'NO
		'		'Objeto.DataSource = ListaSucesos4

		'		'------------------------------------------------------------------------
		'		'SI
		'		'------------------------------------------------------------------------
		'		'Dim ListaSucesosOBJ As List(Of gtkMatenimientoSist) = Objeto.DataSource
		'		'Dim ListaSucesosOBJ2 As IEnumerable(Of gtkMatenimientoSist) = ListaSucesosOBJ.OrderBy(Function(o) o.Id)	'SI
		'		'Dim ListaSucesosOBJ3 As New List(Of Object)
		'		'For Each Obj As gtkMatenimientoSist In ListaSucesosOBJ2
		'		'	ListaSucesosOBJ3.Add(Obj)
		'		'Next
		'		'Objeto.DataSource = ListaSucesosOBJ3
		'		'------------------------------------------------------------------------
		'		'SI
		'		'------------------------------------------------------------------------
		'		'Dim ListaSucesosOBJ As List(Of gtkMatenimientoSist) = Objeto.DataSource
		'		'ListaSucesosOBJ = ListaSucesosOBJ.OrderBy(Function(o) o.Id).ToList 'SI
		'		'Objeto.DataSource = ListaSucesosOBJ
		'		'------------------------------------------------------------------------
		'		'------------------------------------------------------------------------

		'		'------------------------------------------------------
		'		'Indicamos el tipo de objeto
		'		'------------------------------------------------------
		'		'Dim TipoObjeto As Type = Objeto.DataSource.GetType
		'		'Dim PropiedadesObjeto As System.Reflection.PropertyInfo() = TipoObjeto.GetProperties()
		'		'------------------------------------------------------
		'		'Dim ListaSucesosOBJ As List(Of gtkMatenimientoSist) = Objeto.DataSource

		'		'------------------------------------------------------
		'		'Dim TipoObjeto As System.Type = gtk.GetType
		'		'Dim PropiedadesObjeto As System.Reflection.PropertyInfo() = TipoObjeto.GetProperties()
		'		'------------------------------------------------------
		'		'Dim PropiedadesObjeto As System.Reflection.PropertyInfo() = gtk.GetType.GetProperties
		'		'Dim Propiedad As System.Reflection.PropertyInfo = gtk.GetType.GetProperty("Id")
		'		'Dim s As String = "Id"
		'		'Dim i As Integer = Array.BinarySearch(gtk.GetType.GetProperties, s)'NO
		'		'Dim i As Integer = Array.IndexOf(gtk.GetType.GetProperties, s)'NO

		'		'------------------------------------------------------
		'		'Dim Propiedad As System.Reflection.PropertyInfo = Array.Find(gtk.GetType.GetProperties, Function(pi As System.Reflection.PropertyInfo) pi.Name.ToUpper.Trim = CampoOrden.ToUpper.Trim) 'SI
		'		'------------------------------------------------------
		'		'Dim Propiedad As System.Reflection.PropertyInfo = Array.Find(gtk.GetType.GetProperties, Function(pi As System.Reflection.PropertyInfo) pi.Name = CampoOrden) 'No encuenta la propiedad.
		'		'------------------------------------------------------

		'		'------------------------------------------------------


		'		'Dim Propiedad As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) pi.Name.ToUpper.Trim = CampoOrden.ToUpper.Trim) 'SI
		'		'------------------------------------------------------
		'		'Dim Campo = Array.Find(PropiedadesObjeto, Function(pi As PropertyInfo) pi.Name.ToUpper.Trim = CampoOrden.ToUpper.Trim).Name
		'		'Dim Propiedad As PropertyInfo = Array.Find(PropiedadesObjeto, Function(pi As PropertyInfo) pi.Name.ToUpper.Trim = CampoOrden.ToUpper.Trim)
		'		'For Each PI As PropertyInfo In PropiedadesObjeto
		'		'	Dim x = PI.GetValue(PI, Nothing)
		'		'Next

		'		'ListaSucesosOBJ = ListaSucesosOBJ.OrderBy(Function(o) o.GetType.GetProperty("Id").GetValue(o, Nothing)).ToList 'SI
		'		'ListaSucesosOBJ = ListaSucesosOBJ.OrderBy(Function(o) o.GetType.GetProperty("ID").GetValue(o, Nothing)).ToList 'NO. GetProperty sensible.
		'		ListaSucesos = ListaSucesos.OrderByDescending(Function(o) o.GetType.GetProperty(Propiedad.Name).GetValue(o, Nothing)).ToList
		'	End If
		'	GridView.DataSource = ListaSucesos
		'End If
		'-----------------------------------------------------------------------------------------------------------------------
		'FROGA: Problema con el codigo anterior. La funcion "OrdenarLista" deferencia entre Mayusculas y Minusculas.
		'-----------------------------------------------------------------------------------------------------------------------
		'If ListaSucesos IsNot Nothing AndAlso ListaSucesos.Count > 0 Then
		'	If DireccionCampo = SortDirection.Ascending Then
		'		Objeto.DataSource = ListaSucesos.OrderBy(Function(o) o.Id)
		'	ElseIf DireccionCampo = SortDirection.Descending Then
		'		Objeto.DataSource = ListaSucesos.OrderByDescending(Function(o) o.Id)
		'	End If
		'End If
		'-----------------------------------------------------------------------------------------------------------------------
		'If GridView.DataSource IsNot Nothing AndAlso GridView.DataSource.Count > 0 Then
		'	'-----------------------------------------------------------------------------------------------------------------------
		'	'1º-Obtenemos el tipo de objeto.
		'	Dim TipoObjeto As Type = TipoObjetos(GridView.DataSource)
		'	'2º-Obtenemos las propiedades del Tipo de Objeto.
		'	Dim Propiedades1 As PropertyInfo() = TipoObjeto.GetProperties
		'	Dim Propiedades2 As List(Of PropertyInfo) = TipoObjeto.GetProperties.ToList
		'	'3º-Obtenemos la propiedad por la que se va a ordenar.
		'	Dim Propiedad1 As PropertyInfo = Array.Find(Propiedades1, Function(pi As System.Reflection.PropertyInfo) StrComp(pi.Name, CampoOrden.Trim, CompareMethod.Text) = 0)
		'	Dim Propiedad2 As PropertyInfo = Propiedades2.Find(Function(pi As System.Reflection.PropertyInfo) StrComp(pi.Name, CampoOrden.Trim, CompareMethod.Text) = 0)
		'	'4º-Ordenacion
		'	Dim ListaSucesos1 As Object = GridView.DataSource


		'	'If Propiedad1 IsNot Nothing Then
		'	'Dim ListaSucesos1_B As New List(Of gtkMatenimientoSist)
		'	'ListaSucesos1_B = ListaSucesos1.OrderByDescending(Function(o) o.GetType.GetProperty(Propiedad1.Name).GetValue(o, Nothing))
		'	'GridView.DataSource = ListaSucesos1
		'	'End If

		'	If Propiedad2 IsNot Nothing Then
		'		'ListaSucesos1 = ListaSucesos1.OrderByDescending(Function(o) o.GetType.GetProperty(Propiedad2.Name).GetValue(o, Nothing)) 'NO
		'		'-----------------------------------------------------------
		'		'Dim ListaSucesos1_B As New List(Of gtkMatenimientoSist)
		'		'ListaSucesos1_B = ListaSucesos1.OrderByDescending(Function(o) o.GetType.GetProperty(Propiedad2.Name).GetValue(o, Nothing)) 'NO
		'		'-----------------------------------------------------------
		'		'Dim ListaSucesos2 As New List(Of gtkMatenimientoSist)
		'		'ListaSucesos2 = GridView.DataSource
		'		'ListaSucesos2 = ListaSucesos2.OrderByDescending(Function(o) o.GetType.GetProperty(Propiedad2.Name).GetValue(o, Nothing)).ToList	'SI
		'		'-----------------------------------------------------------
		'		'Dim ListaSucesos2 As New List(Of Object)(GridView.DataSource)
		'		'ListaSucesos2.AddRange(GridView.DataSource)
		'		'ListaSucesos2 = GridView.DataSource
		'		'ListaSucesos2 = ListaSucesos2.OrderByDescending(Function(o) o.GetType.GetProperty(Propiedad2.Name).GetValue(o, Nothing)).ToList
		'		'Dim Tipo2 As Type = GridView.DataSource.GetType
		'		'-----------------------------------------------------------
		'		Dim ListaSucesos2 As New List(Of Object)
		'		For Each Objeto As Object In GridView.DataSource
		'			ListaSucesos2.Add(Objeto)
		'		Next
		'		ListaSucesos2 = ListaSucesos2.OrderByDescending(Function(o) o.GetType.GetProperty(Propiedad2.Name).GetValue(o, Nothing)).ToList
		'		GridView.DataSource = ListaSucesos2
		'	End If

		'-----------------------------------------------------------------------------------------------------------------------

		'-----------------------------------------------------------------------------------------------------------------------
		'Dim TipoObjeto As Type '= GridView.DataSource.GetType
		''Dim ListaSucesos_List As List(Of Object) = DirectCast(GridView.DataSource, List(Of Object)).ToList

		''Dim ListaSucesos_Array As Object() = ListaSucesos_List.ToArray

		'For Each Objeto As Object In GridView.DataSource
		'	TipoObjeto = Objeto.GetType
		'	Dim Propiedades1 As PropertyInfo() = TipoObjeto.GetProperties
		'	Dim Propiedades2 As List(Of System.Reflection.PropertyInfo) = TipoObjeto.GetType.GetProperties.ToList
		'	Dim Propiedades3 As PropertyInfo() = TipoObjeto.GetProperties.ToArray

		'	Dim Propiedad2 As System.Reflection.PropertyInfo = Array.Find(Propiedades3, Function(pi As System.Reflection.PropertyInfo) StrComp(pi.Name, CampoOrden.Trim, CompareMethod.Text) = 0)
		'	'ListaSucesos2 = ListaSucesos2.OrderBy(Function(o) o.GetType.GetProperty(Propiedad2.Name).GetValue(o, Nothing)).ToList
		'	Dim ListaSucesos_Array As New Object()
		'	Dim i As Integer = 0
		'	ListaSucesos_Array.SetValue(Objeto, Nothing)
		'	i += 1
		'	ListaSucesos_Array = ListaSucesos_Array.OrderBy(Function(o) o.GetType.GetProperty(Propiedad2.Name).GetValue(o, Nothing)).ToArray
		'Next

		''Dim ListaSucesos2 As List(Of gtkMatenimientoSist) = GridView.DataSource
		'End If
		'-----------------------------------------------------------------------------------------------------------------------
	End Sub

	'Private Function TipoObjetos(ListaObjetos As Object) As Type
	'	TipoObjetos = Nothing
	'	If ListaObjetos IsNot Nothing Then
	'		For Each Objeto As Object In ListaObjetos
	'			TipoObjetos = Objeto.GetType
	'			Exit For
	'		Next
	'	End If
	'	Return TipoObjetos
	'End Function


	'Private Function OrdenarLista(ByVal o1 As Object, ByVal o2 As Object, Optional ByVal CampoOrden As String = Nothing, Optional ByVal DireccionCampo As SortDirection = SortDirection.Ascending)
	'	OrdenarLista = Nothing
	'	'------------------------------------------------------
	'	'Indicamos el tipo de objeto
	'	'------------------------------------------------------
	'	Dim TipoObjeto As Type = o1.GetType
	'	Dim PropiedadesObjeto As System.Reflection.PropertyInfo() = TipoObjeto.GetProperties()
	'	'------------------------------------------------------

	'	'-----------------------------------------------------------------------------------------------------------------------
	'	'Proble al utilizar este metodo. Si es texto diferencia entre Mayusculas y Minusculas.
	'	'-----------------------------------------------------------------------------------------------------------------------
	'	If CampoOrden IsNot Nothing AndAlso CampoOrden.Trim <> String.Empty Then
	'		If DireccionCampo = SortDirection.Ascending Then
	'			OrdenarLista = Array.Find(PropiedadesObjeto, Function(pi As System.Reflection.PropertyInfo) pi.Name.ToUpper.Trim = CampoOrden.ToUpper.Trim).GetValue(o1, Nothing) _
	'			< _
	'			 Array.Find(PropiedadesObjeto, Function(pi As System.Reflection.PropertyInfo) pi.Name.ToUpper.Trim = CampoOrden.ToUpper.Trim).GetValue(o2, Nothing)
	'		ElseIf DireccionCampo = SortDirection.Descending Then
	'			OrdenarLista = Array.Find(PropiedadesObjeto, Function(pi As System.Reflection.PropertyInfo) pi.Name.ToUpper.Trim = CampoOrden.ToUpper.Trim).GetValue(o1, Nothing) _
	'			> _
	'			 Array.Find(PropiedadesObjeto, Function(pi As System.Reflection.PropertyInfo) pi.Name.ToUpper.Trim = CampoOrden.ToUpper.Trim).GetValue(o2, Nothing)
	'		End If
	'	End If
	'	'-----------------------------------------------------------------------------------------------------------------------
	'	Return OrdenarLista
	'End Function
End Module