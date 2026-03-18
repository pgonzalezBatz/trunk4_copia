Public Partial Class Titulo
    Inherits System.Web.UI.UserControl

#Region "Propiedades"

    Private _texto As String = String.Empty

    ''' <summary>
    ''' Texto que se visualizara en el titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Texto() As String
        Get
            Return _texto
        End Get
        Set(ByVal value As String)
            _texto = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Asigna el texto al titulo
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim pg As New PageBase
        lblTitulo.Text = Texto
        pg.itzultzaileWeb.Itzuli(lblTitulo)
    End Sub
End Class