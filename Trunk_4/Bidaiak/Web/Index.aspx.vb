Public Class Index
    Inherits Page

    ''' <summary>
    ''' Acceso desde el portal del empleado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        ctrlIndex.loadIndex(Index1.Acceso.Portal_Empleado, User.Identity.Name.ToLower, CInt(ConfigurationManager.AppSettings.Get("RecursoWeb")))
    End Sub

End Class