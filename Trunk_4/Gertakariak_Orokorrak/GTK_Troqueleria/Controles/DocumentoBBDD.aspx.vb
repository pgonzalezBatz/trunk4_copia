Public Class DocumentoBBDD
    Inherits System.Web.UI.Page

#Region "Propiedades"
	''' <summary>
	''' Identificador del documento que se quiere cargar.
	''' </summary>
	''' <remarks></remarks>
	Public Id_Doc As Integer
#End Region

#Region "Eventos de Pagina"
	Private Sub DocumentoBBDD_PreInit(sender As Object, e As System.EventArgs) Handles Me.PreInit

		Dim BBDD As New BatzBBDD.Entities_Gertakariak
		Dim Documento As New BatzBBDD.DOCUMENTOS

		Id_Doc = Request("Id_Doc")

		Documento = (From Doc As BatzBBDD.DOCUMENTOS In BBDD.DOCUMENTOS Where Doc.ID = Id_Doc Select Doc).FirstOrDefault

		If Documento IsNot Nothing AndAlso Documento.DOCUMENTO IsNot Nothing Then
			'----------------------------------------------------------------
			'Si el Explorador reconoce el archivo lo abrira dentro de él. 
			'Si no pedira al usuario la accion a realizar.
			'----------------------------------------------------------------
			Response.Clear()
			Response.ClearHeaders()
			Response.ClearContent()

			Response.Buffer = True
			'Response.AppendHeader("Content-Disposition", "attachment; filename=" & Documento.NOMBRE & "." & Documento.EXTENSION)
			Response.AppendHeader("Content-Disposition", "inline; filename=" & Documento.NOMBRE & "." & Documento.EXTENSION)
			Response.ContentType = Documento.CONTENT_TYPE

			Response.BinaryWrite(Documento.DOCUMENTO)
			'----------------------------------------------------------------

			Response.End()
		End If
	End Sub
#End Region


End Class