Public Class Informes
    Inherits PageBase

    ''' <summary>
    ''' Se inicializa la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        'Try
        If (Not Page.IsPostBack) Then
            cargarURLInformes()
            'cargarPlantas()
        End If
        'Catch batzEx As SabLib.BatzException
        '    Master.MensajeError = batzEx.Termino
        'End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(lnkVerKPIBMS) : itzultzaileWeb.Itzuli(lnkVerKPICommittee)
            'itzultzaileWeb.Itzuli(labelPlanta) : itzultzaileWeb.Itzuli(labelNegocio) : itzultzaileWeb.Itzuli(btnVer)
        End If
    End Sub

    ''' <summary>
    ''' Carga la url de los informes
    ''' </summary>
    Private Sub cargarURLInformes()
        lnkVerKPIBMS.OnClientClick = "window.open('" & Master.GetUrlInforme("KPI_REPORT_BMS") & "','KPI BMS','_blank');return false;"
        lnkVerKPICommittee.OnClientClick = "window.open('" & Master.GetUrlInforme("KPI_REPORT_COMMITTEE") & "','KPI committee','_blank');return false;"
    End Sub

    '''' <summary>
    '''' Carga las plantas a las que tiene acceso el usuario
    '''' </summary>    
    'Private Sub cargarPlantas()
    '    If (ddlPlantas.Items.Count = 0) Then
    '        Dim plantBLL As New BLL.PlantasComponent
    '        Dim lPlantas As List(Of ELL.Planta) = plantBLL.loadListPlantas(Master.Ticket.IdUser)
    '        ddlPlantas.DataSource = (From plant In lPlantas Order By plant.Nombre Select plant.Id, plant.IdMoneda, plant.Nombre, plant.IdPlantaSAB).Distinct().ToList()
    '        ddlPlantas.DataBind()
    '        If (ddlPlantas.Items.Count > 1) Then
    '            ddlPlantas.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0)) 'Solo se añade el seleccione uno si tiene mas de una planta
    '            ddlNegocios.Items.Clear()
    '            ddlNegocios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccionePlanta"), 0))
    '        ElseIf (ddlplantas.Items.Count = 1) Then
    '            ddlPlantas.SelectedIndex = 0
    '            cargarNegocios()
    '        Else
    '            ddlPlantas.Items.Add(New ListItem(itzultzaileWeb.Itzuli("No tiene asignada ninguna planta"), 0))
    '            ddlNegocios.Items.Clear()
    '            ddlNegocios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccionePlanta"), 0))
    '        End If
    '    Else
    '        ddlNegocios.Items.Clear()
    '        ddlNegocios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccionePlanta"), 0))
    '    End If
    '    ddlPlantas.SelectedIndex = -1
    'End Sub

    '''' <summary>
    '''' Carga los negocios a los que tiene acceso
    '''' </summary>    
    'Private Sub cargarNegocios()
    '    Dim perfBLL As New BLL.PerfilAreaComponent
    '    Dim lPerfiles As List(Of ELL.PerfilArea) = perfBLL.loadListPerfiles(New ELL.PerfilArea With {.IdPlanta = CInt(ddlPlantas.SelectedValue), .IdUsuario = Master.Ticket.IdUser})
    '    If (lPerfiles.Count > 0) Then
    '        Dim lPerfDistinct = (From perf In lPerfiles Order By perf.NombreNegocio Select perf.IdNegocio, perf.NombreNegocio).Distinct().ToList()
    '        ddlNegocios.Items.Clear()
    '        ddlNegocios.DataSource = lPerfDistinct
    '        ddlNegocios.DataBind()
    '        If (lPerfDistinct.Count > 1) Then
    '            ddlNegocios.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("Seleccione uno"), 0))
    '        ElseIf (lPerfDistinct.Count = 1) Then
    '            VerInforme()
    '        Else
    '            ddlNegocios.Items.Insert(0, New ListItem(itzultzaileWeb.Itzuli("No existe ningun negocio asociado al usuario con dicha planta"), 0))
    '        End If
    '    End If
    'End Sub

    '''' <summary>
    '''' Se cargan las areas de la planta
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>    
    'Private Sub ddlPlantas_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ddlPlantas.SelectedIndexChanged
    '    Try
    '        If (ddlPlantas.SelectedIndex = 0) Then
    '            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("seleccionePlanta")
    '            ddlNegocios.Items.Clear()
    '            ddlNegocios.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccionePlanta"), 0))
    '        Else
    '            cargarNegocios()
    '        End If
    '    Catch ex As Exception
    '        log.Error("Error al cargar los negocios despues de seleccionar las plantas", ex)
    '        Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar los datos")
    '    End Try
    'End Sub

    '''' <summary>
    '''' Lanza el link del informe dependiendo el negocio elegido
    '''' </summary>
    'Private Sub VerInforme()
    '    Dim urlInforme, nombreInforme As String
    '    If (ddlNegocios.SelectedValue = "1") Then 'Sistemas
    '        nombreInforme = "KPI_REPORT_SISTEMAS"
    '    Else 'BMS
    '        nombreInforme = "KPI_REPORT_BMS"
    '    End If
    '    urlInforme = Master.GetUrlInforme(nombreInforme)
    '    Dim Script As String = "window.location='" & urlInforme & "';"
    '    ScriptManager.RegisterStartupScript(Page, Page.GetType(), "Informes", Script, True)
    'End Sub

    '''' <summary>
    '''' Lanza el informe
    '''' </summary>
    '''' <param name="sender"></param>
    '''' <param name="e"></param>
    'Private Sub btnVer_Click(sender As Object, e As EventArgs) Handles btnVer.Click
    '    If (ddlPlantas.SelectedValue <> "0" AndAlso ddlNegocios.SelectedValue <> "0") Then
    '        VerInforme()
    '    Else
    '        Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Introduzca los datos")
    '    End If
    'End Sub

End Class