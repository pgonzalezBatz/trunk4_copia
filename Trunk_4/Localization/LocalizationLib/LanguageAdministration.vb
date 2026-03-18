Imports System.IO
Imports System.Collections.Generic
Imports System.Xml
Imports System.Globalization.CultureInfo
Imports System.Web
Imports System.Text.RegularExpressions

Public Class LanguageAdministration

#Region "Gets"
	'Returns the culture name (ie, eu-ES, fr-CA, en-US) of the current request
	Public Shared ReadOnly Property CurrentCultureName() As String
		Get

			'El lenguaje por defecto para usuarios desconocidos
			Try
				Return CurrentCulture.Name
			Catch ex As Exception
				Return "es-ES"
			End Try
		End Get
	End Property


	''' <summary>
	''' Devuleve un diccionario con una entrada para cada idioma activo
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetListaDeIdiomasActuales(ByVal pathLanguages As String) As Dictionary(Of String, String)
		Dim idiomas As New Dictionary(Of String, String)
		Dim dirInfo As New DirectoryInfo(pathLanguages)
        For Each dir As DirectoryInfo In dirInfo.GetDirectories()            
            idiomas.Add(dir.Name, dir.FullName)
        Next
		Return idiomas
    End Function

    ''' <summary>
    ''' Devuleve un diccionario con una entrada para cada idioma activo
    ''' </summary>
    ''' <param name="pathLanguages"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function GetListaDeIdiomasActualesConNombre(ByVal pathLanguages As String) As Dictionary(Of String, String)
        Dim idiomas As New Dictionary(Of String, String)
        Dim dirInfo As New DirectoryInfo(pathLanguages)
        For Each dir As DirectoryInfo In dirInfo.GetDirectories()
            idiomas.Add(dir.Name.Itzuli, dir.Name)            
        Next
        Return idiomas
    End Function


	''' <summary>
	''' Devuelve un diccionario con terminos traducidos del idioma
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTermino(ByVal pathLanguages As String, ByVal language As String, ByVal key As String) As Termino
		Dim xml As New XmlDocument
		Dim termino As New Termino
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")

		For Each n As XmlNode In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso n.Attributes("name").Value = key Then
				termino.Key = key
				termino.Traduccion = n.InnerText
				termino.Traducido = (n.Attributes("translated").Value = 1)
			End If
		Next
		Return termino
	End Function

#Region "GetTerminos Antiguo"

	''' <summary>
	''' Devuelve un diccionario con terminos traducidos del idioma
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTerminosTraducidosDelIdioma(ByVal pathLanguages As String, ByVal language As String) As SortedDictionary(Of String, String)
		Dim xml As New XmlDocument
		Dim terminos As New SortedDictionary(Of String, String)
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")

		For Each n As XmlNode In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso n.Attributes("translated").Value = "1" AndAlso Not terminos.ContainsKey(n.Attributes("name").Value) Then
				terminos.Add(n.Attributes("name").Value, n.InnerText)
			End If
		Next
		Return terminos
	End Function

	''' <summary>
	''' Devuelve un diccionario con termino no traducidos del idioma
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTerminosNoTraducidosDelIdioma(ByVal pathLanguages As String, ByVal language As String) As SortedDictionary(Of String, String)
		Dim xml As New XmlDocument
		Dim terminos As New SortedDictionary(Of String, String)
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")
		For Each n As XmlNode In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso n.Attributes("translated").Value = "0" AndAlso Not terminos.ContainsKey(n.Attributes("name").Value) Then
				terminos.Add(n.Attributes("name").Value, n.InnerText)
			End If
		Next
		Return terminos
	End Function

#End Region

#Region "GetTerminos Nuevo"

	''' <summary>
	''' Devuelve un diccionario con los terminos solicitados
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <param name="queTerminos">Indica si se obtendran todos(0),traducidos(1),no traducidos(2)</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTerminosDelIdioma(ByVal pathLanguages As String, ByVal language As String, ByVal queTerminos As Integer, ByVal termino As String, ByVal busquedaExacta As Boolean, ByVal busquedaSensitiva As Boolean) As SortedDictionary(Of String, Termino)
		Dim terminos As New SortedDictionary(Of String, Termino)
		Dim n As XmlNode
		Dim xml As New XmlDocument
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")
		If (busquedaExacta) Then termino = "^" & termino & "$"
		For Each n In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso (queTerminos = 0 OrElse (queTerminos = 1 AndAlso n.Attributes("translated").Value = "1") OrElse (queTerminos = 2 AndAlso n.Attributes("translated").Value = "0")) Then
				If (busquedaSensitiva) Then
					If Regex.IsMatch(n.Attributes("name").Value, termino, RegexOptions.Compiled) Then
						terminos.Add(n.Attributes("name").Value, New Termino(n.Attributes("name").Value, n.InnerText, (n.Attributes("translated").Value = 1)))
					End If
				Else
					If Regex.IsMatch(n.Attributes("name").Value, termino, RegexOptions.IgnoreCase) Then
						terminos.Add(n.Attributes("name").Value, New Termino(n.Attributes("name").Value, n.InnerText, (n.Attributes("translated").Value = True)))
					End If
				End If
			End If
		Next
		Return terminos
	End Function

	''' <summary>
	''' Devuelve un diccionario con todos los terminos del idioma (METODO ANTIGUO)
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTerminosDelIdioma(ByVal pathLanguages As String, ByVal language As String) As Dictionary(Of String, String)
		Dim xml As New XmlDocument
		Dim terminos As New Dictionary(Of String, String)
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")

		For Each n As XmlNode In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso Not terminos.ContainsKey(n.Attributes("name").Value) Then
				terminos.Add(n.Attributes("name").Value, n.InnerText)
			End If
		Next
		Return terminos
	End Function

	''' <summary>
	''' Devuelve un diccionario con todos los terminos del idioma
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTerminosDelIdiomaNew(ByVal pathLanguages As String, ByVal language As String) As SortedDictionary(Of String, Termino)
		Dim xml As New XmlDocument
		Dim terminos As New SortedDictionary(Of String, Termino)
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")

		For Each n As XmlNode In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso Not terminos.ContainsKey(n.Attributes("name").Value) Then
				terminos.Add(n.Attributes("name").Value, New Termino(n.Attributes("name").Value, n.InnerText, (n.Attributes("translated").Value = 1)))
			End If
		Next
		Return terminos
	End Function

	''' <summary>
	''' Devuelve un diccionario con terminos traducidos del idioma
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTerminosTraducidosDelIdiomaNew(ByVal pathLanguages As String, ByVal language As String) As SortedDictionary(Of String, Termino)
		Dim xml As New XmlDocument
		Dim terminos As New SortedDictionary(Of String, Termino)
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")

		For Each n As XmlNode In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso n.Attributes("translated").Value = "1" AndAlso Not terminos.ContainsKey(n.Attributes("name").Value) Then
				terminos.Add(n.Attributes("name").Value, New Termino(n.Attributes("name").Value, n.InnerText, (n.Attributes("translated").Value = 1)))
			End If
		Next
		Return terminos
	End Function

	''' <summary>
	''' Devuelve un diccionario con termino no traducidos del idioma
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTerminosNoTraducidosDelIdiomaNew(ByVal pathLanguages As String, ByVal language As String) As SortedDictionary(Of String, Termino)
		Dim xml As New XmlDocument
		Dim terminos As New SortedDictionary(Of String, Termino)
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")
		For Each n As XmlNode In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso n.Attributes("translated").Value = "0" AndAlso Not terminos.ContainsKey(n.Attributes("name").Value) Then
				terminos.Add(n.Attributes("name").Value, New Termino(n.Attributes("name").Value, n.InnerText, (n.Attributes("translated").Value = 1)))
			End If
		Next
		Return terminos
	End Function

	''' <summary>
	''' Devuelve un diccionario con los terminos que tienen mas de una repeticion
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>	
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function GetTerminosDelIdiomaConRepeticion(ByVal pathLanguages As String, ByVal language As String) As List(Of String)
		Dim terminos As New SortedDictionary(Of String, Termino)
		Dim terminosRepes As New List(Of String)
		Dim n As XmlNode
		Dim xml As New XmlDocument
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")
		For Each n In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment Then
				Try
					terminos.Add(n.Attributes("name").Value, New Termino(n.Attributes("name").Value, n.InnerText, (n.Attributes("translated").Value = 1)))
				Catch
					Try
						terminosRepes.Add(n.Attributes("name").Value)
					Catch
						'Si este tambien da error, es porque se esta intentando meter un termino que ya esta repe y ya se registro
					End Try
				End Try			
			End If
		Next
		Return terminosRepes
	End Function

#End Region

#End Region

#Region "Update"
	''' <summary>
	''' Cambia los valores del termino especificado en el idioma especificado
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="language"></param>
	''' <param name="termino"></param>
	''' <param name="traduccion"></param>
	''' <param name="traducido"></param>
	''' <remarks></remarks>
	Public Shared Sub UpdateTermino(ByVal pathLanguages As String, ByVal language As String, ByVal termino As String, ByVal traduccion As String, ByVal traducido As String)
		Dim xml As New XmlDocument
		xml.Load(pathLanguages & "\" & language & "\Resource.xml")
		For Each n As XmlNode In xml.SelectSingleNode("Resource")
			If n.NodeType <> XmlNodeType.Comment AndAlso n.Attributes("name").Value = termino Then
				n.Attributes("translated").Value = traducido
				n.InnerText = traduccion
			End If
		Next
		xml.Save(pathLanguages & "\" & language & "\Resource.xml")
	End Sub

#End Region

#Region "Insert"
	''' <summary>
	''' Añade el termino como traducido en el idioma especificado y como no traducido en los demas
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="idioma"></param>
	''' <param name="termino"></param>
	''' <param name="traduccion"></param>
	''' <remarks></remarks>
	Public Shared Sub InsertTermino(ByVal pathLanguages As String, ByVal idioma As String, ByVal termino As String, ByVal traduccion As String)
		If Not ExisteTermino(pathLanguages, idioma, termino) Then
			Dim idiomas As IEnumerator = GetListaDeIdiomasActuales(pathLanguages).GetEnumerator()
			While idiomas.MoveNext
				Dim k As KeyValuePair(Of String, String) = idiomas.Current
				Dim xml As New XmlDocument
				xml.Load(k.Value & "\Resource.xml")
				Dim newElement As XmlElement = xml.CreateElement("Item")
				newElement.InnerText = traduccion
				newElement.SetAttribute("name", termino)
				If k.Key = idioma Then
					'Guardar el nuevo termino en el xml correspondiente como traducido
					newElement.SetAttribute("translated", "1")
				Else
					'Guardar el nuevo termino en el resto de xml como no traducido
					newElement.SetAttribute("translated", "0")
				End If

				Dim n As XmlNode = xml.SelectSingleNode("Resource")
				n.InsertAfter(newElement, n.LastChild)
				xml.Save(k.Value & "\Resource.xml")
			End While
		End If
	End Sub

	''' <summary>
	''' Inserta un termino en el idioma especificado
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="idioma"></param>
	''' <param name="termino"></param>
	''' <param name="traduccion"></param>
	''' <remarks></remarks>
	Public Shared Sub InsertTerminoUno(ByVal pathLanguages As String, ByVal idioma As String, ByVal termino As String, ByVal traduccion As String, ByVal traducido As String)
		Dim xml As New XmlDocument
		xml.Load(pathLanguages & "\" & idioma & "\Resource.xml")
		Dim newElement As XmlElement = xml.CreateElement("Item")
		newElement.InnerText = traduccion
		newElement.SetAttribute("name", termino)
		newElement.SetAttribute("translated", traducido)

		Dim n As XmlNode = xml.SelectSingleNode("Resource")
		n.InsertAfter(newElement, n.LastChild)
		xml.Save(pathLanguages & "\" & idioma & "\Resource.xml")
	End Sub

	Public Shared Function ExisteTermino(ByVal pathLanguages As String, ByVal idioma As String, ByVal termino As String) As Boolean
		Return GetTerminosDelIdioma(pathLanguages, idioma).ContainsKey(termino)
	End Function

	''' <summary>
	''' Comprueba que exista el termino y devuelve tambien la traduccion si la tuviera
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="idioma"></param>
	''' <param name="termino"></param>
	''' <param name="traduccion">Valor por referencia donde se dejara la traduccion</param>
	''' <remarks></remarks>
    Public Shared Function ExisteTermino2(ByVal pathLanguages As String, ByVal idioma As String, ByVal termino As String, ByRef traduccion As String) As Boolean
        Dim bExiste As Boolean = False
		Try
			Dim terminosIdiomas As Dictionary(Of String, String) = GetTerminosDelIdioma(pathLanguages, idioma)

			For Each term As String In terminosIdiomas.Keys
				If (term.Length = termino.Length AndAlso Regex.IsMatch(term, termino, RegexOptions.IgnoreCase)) Then
					bExiste = True
					traduccion = terminosIdiomas.Item(term)
					Exit For
				End If
			Next
		Catch ex As Exception
			'No se quiere lanzar excepcion
		End Try
        Return bExiste
    End Function

#End Region

#Region "Delete"
	''' <summary>
	''' Elimina el termino especificado
	''' </summary>
	''' <param name="pathLanguages"></param>
	''' <param name="termino"></param>
	''' <remarks></remarks>
	Public Shared Sub DeleteTermino(ByVal pathLanguages As String, ByVal termino As String)
		Dim idiomas As IEnumerator = GetListaDeIdiomasActuales(pathLanguages).GetEnumerator()
		While idiomas.MoveNext
			Dim k As KeyValuePair(Of String, String) = idiomas.Current
			If Not (k.Key = "es-ES" OrElse k.Key = "eu-ES") Then
				Dim xml As New XmlDocument
				xml.Load(k.Value & "\Resource.xml")
				For Each n As XmlNode In xml.SelectSingleNode("Resource")
					If n.NodeType <> XmlNodeType.Comment AndAlso n.Attributes("name").Value = termino Then
						n.ParentNode.RemoveChild(n)
					End If
				Next
				xml.Save(k.Value & "\Resource.xml")
			End If
		End While
	End Sub
#End Region

#Region "Class Termino"

	Public Class Termino
		Private _key As String = String.Empty
		Private _traduccion As String = String.Empty
		Private _traducido As Boolean = False

		Public Sub New()
		End Sub

		Public Sub New(ByVal pKey As String, ByVal pTraduccion As String, ByVal pTraduccido As Boolean)
			_key = pKey
			_traduccion = pTraduccion
			_traducido = pTraduccido
		End Sub

		Public Property Key() As String
			Get
				Return _key
			End Get
			Set(ByVal value As String)
				_key = value
			End Set
		End Property

		Public Property Traduccion() As String
			Get
				Return _traduccion
			End Get
			Set(ByVal value As String)
				_traduccion = value
			End Set
		End Property

		Public Property Traducido() As Boolean
			Get
				Return _traducido
			End Get
			Set(ByVal value As Boolean)
				_traducido = value
			End Set
		End Property
	End Class

#End Region
End Class
