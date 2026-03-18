Public Partial Class Titulo
    Inherits System.Web.UI.UserControl

#Region "Propiedades"


    ''' <summary>
    ''' Texto que se visualizara en el titulo
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Property Texto() As String
        Get
            Return lblTitulo.Text
        End Get
        Set(ByVal value As String)
            lblTitulo.Text = value  'Se ha puesto porque sino habia problemas a la hora de pintar y traducir el termino
        End Set
    End Property

#End Region

End Class