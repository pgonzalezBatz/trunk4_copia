Public Class Index_Directo
    Inherits Page

    ''' <summary>
    ''' Pagina de inicio creada para los accesos directos. Sin pasar por el portal del empleado    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ctrlIndex.loadIndex(Index1.Acceso.Directo, User.Identity.Name.ToLower, CInt(ConfigurationManager.AppSettings.Get("RecursoWeb_Admon")))
    End Sub

End Class