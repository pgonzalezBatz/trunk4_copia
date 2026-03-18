Partial Public Class SeleccionPlanta
    Inherits PageBase

    ''' <summary>
    ''' Se le dice los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(btnIr)
        End If
    End Sub

    ''' <summary>
    ''' Si solo tiene acceso a una planta, se redireccionara a la pagina default.aspx. En caso contrario, se elegira la planta a tratar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim lPlantas As List(Of SabLib.ELL.Planta)
                Dim plantComp As New SabLib.BLL.PlantasComponent
                'Esta pagina solo sera accesible para los de la planta 1
                If (Session(PageBase.PLANTA_IGORRE) IsNot Nothing) Then
                    lPlantas = plantComp.GetPlantas(True)
                    'De momento,se excluye Igorre y de los que tienen Izaro informado, solo MbLusitana                    
                    For index As Integer = lPlantas.Count - 1 To 0 Step -1
                        If ((lPlantas.Item(index).De_Epsilon OrElse lPlantas.Item(index).De_Izaro) OrElse lPlantas.Item(index).De_Nomina) Then 'Se excluyen las que tienen epsilon,izaro o nominas
                            If (lPlantas.Item(index).Id = 5 OrElse lPlantas.Item(index).Id = 6 OrElse lPlantas.Item(index).Id = 47) Then
                                '140720: Se mete a Chequia para los trabajadores que no se pueden registrar en el KSProgram
                                '031120: Se mete a Mexico para los trabajadores que no se pueden registrar en TRESS
                                '220221: Se mete a Zamudio para los trabajadores que no se puede registrar en Epsilon
                                lPlantas.Item(index).Nombre &= " (" & itzultzaileWeb.Itzuli("No cobran nomina alli") & ")"
                            Else
                                lPlantas.RemoveAt(index)
                            End If
                        End If
                    Next
                    ddlPlantas.Items.Add(itzultzaileWeb.Itzuli("seleccioneUno"))
                    If (lPlantas IsNot Nothing AndAlso lPlantas.Count > 0) Then lPlantas.Sort(Function(o1 As SabLib.ELL.Planta, o2 As SabLib.ELL.Planta) o1.Nombre < o2.Nombre)
                    ddlPlantas.DataSource = lPlantas
                    ddlPlantas.DataTextField = SabLib.ELL.Planta.ColumnNames.NOMBRE
                    ddlPlantas.DataValueField = SabLib.ELL.Planta.ColumnNames.ID
                    ddlPlantas.DataBind()
                Else
                    Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
                End If
            End If
            Master.VisualizarCabecera = False
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarPlantas", ex)
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End Try
    End Sub

    ''' <summary>
    ''' Redirecciona a la pagina default.aspx una vez informada la planta en la que se va a trabajar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnIr_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnIr.Click
        If (ddlPlantas.SelectedIndex > 0) Then
            Dim plantBLL As New SabLib.BLL.PlantasComponent
            Dim oPlant As SabLib.ELL.Planta = plantBLL.GetPlanta(CInt(ddlPlantas.SelectedValue))
            Session(PageBase.PLANTA) = oPlant
            WriteLog("Acceso a la planta de " & ddlPlantas.SelectedItem.Text, TipoLog.Info)
            Response.Redirect(PageBase.PAG_INICIO)
        Else
            Master.MensajeAdvertenciaText = "seleccionePlanta"
        End If
    End Sub
End Class