Public Class DeleteCache
    Inherits Page

    ''' <summary>
    ''' Borra la cache para que se visualicen las nuevas perseonas
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim lPlantas As List(Of SabLib.ELL.Planta) = plantBLL.GetPlantas()
            For Each oPlant As SabLib.ELL.Planta In lPlantas
                Cache.Remove("user_" & oPlant.Id)
                Cache.Remove("depart_" & oPlant.Id)
            Next
            lblInfo.Text = "Los usuarios se han refrescado. Todas las nuevas altas de personas que se hayan hecho en las ultimas 3 horas y que tengan un usuario asignado, se muestran ya en el buscador de telefonos"
        End If
    End Sub

End Class