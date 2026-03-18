Imports System.Reflection

Public Class Asset_BLL
	Inherits PrismaLib.Entidades.Asset_DAL

	''' <summary>
	''' Devuelve una lista de Objetos.
	''' </summary>
	''' <returns>List(Of gtkAfectado)</returns>
	''' <remarks></remarks>
	Public Function Listado() As List(Of Asset_BLL)
		Dim ListaObjetos As List(Of PrismaLib.Entidades.Asset_DAL) = Me.Load
		Listado = Nothing
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count > 0 Then
			Listado = New List(Of Asset_BLL)
			For Each item As PrismaLib.Entidades.Asset_DAL In ListaObjetos
				Dim CopiaObjeto As New Asset_BLL
				Dim f As New Funciones
				f.CopiarPropiedades(item, CopiaObjeto)
				Listado.Add(CopiaObjeto)
			Next
		End If
		Return Listado
	End Function

	''' <summary>
	''' Obtiene los datos en base al campo ASSET.
	''' Si no encuentra ningun resultado el campo ASSET es NOTHING.
	''' </summary>
	''' <param name="Asset">Identificador del Registro.</param>
	''' <remarks></remarks>
	Public Sub Cargar(ByVal Asset As String)
		Dim ListaObjetos As New List(Of Asset_BLL)
		Dim funcGTK As New PrismaLib.Funciones
		MyBase.Asset = Asset
		ListaObjetos = Me.Listado()
		If ListaObjetos IsNot Nothing AndAlso ListaObjetos.Count = 1 Then
			funcGTK.CopiarPropiedades(ListaObjetos(0), Me)
		Else
			MyBase.Asset = Nothing
		End If
	End Sub
End Class

