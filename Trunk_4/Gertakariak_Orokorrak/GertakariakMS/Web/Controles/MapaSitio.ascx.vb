Public Class MapaSitio
    Inherits System.Web.UI.UserControl

#Region "Eventos de Página"
	'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
	'End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub smpKaPlan_Load(sender As Object, e As System.EventArgs) Handles smpKaPlan.Load
        Dim Objeto As SiteMapPath = sender
        'Si no tiene elementos a mostrar no lo pintamos para que no ocupe espacio en la pagina.
		Me.Visible = (Objeto.Controls IsNot Nothing AndAlso Objeto.Controls.Count > 0)
	End Sub
#End Region
End Class