Partial Public Class DepartamentosPopUp
    Inherits System.Web.UI.Page

    ''' <summary>
    ''' Carga el flash del departamento pasado como parametro
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            flsSituacion.IdDpto = Request.QueryString("id")
            flsSituacion.Departamento = Request.QueryString("name")
            flsSituacion.Plano = flash.TipoPlano.Grande
        End If
    End Sub

End Class