Public Class Titulo
    Inherits UserControl

#Region "Propiedades"

    ''' <summary>
    ''' Texto que se visualizara en el titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>	
    Public Property Texto() As String
        Get
            Return lblTitulo.Text
        End Get
        Set(ByVal value As String)
            lblTitulo.Text = value
        End Set
    End Property

#End Region

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Titulo_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            Dim itzultzaileWeb As New itzultzaile
            itzultzaileWeb.Itzuli(lblTitulo)
        End If
    End Sub

End Class