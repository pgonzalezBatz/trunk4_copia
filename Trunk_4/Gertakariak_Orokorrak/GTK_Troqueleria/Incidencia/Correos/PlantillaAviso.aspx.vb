Public Class PlantillaAviso
	Inherits PageBase
#Region "Eventos de Pagina"
	Private Sub PlantillaAviso_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
		Dim CodCultura As String = Request.QueryString("CodCultura")
		Dim cultureInfo As System.Globalization.CultureInfo

		If Not String.IsNullOrWhiteSpace(CodCultura) Then
			'--------------------------------------------------------------------------------------
			'Configuramos la cultura del usuario que va a recibir el correo
			'--------------------------------------------------------------------------------------
			cultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture(CodCultura)
			System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
			'--------------------------------------------------------------------------------------
		End If
	End Sub
#End Region
End Class