Partial Public Class SeleccionPlanta
    Inherits PageBase

    ''' <summary>
    ''' Si solo tiene acceso a una planta, se redireccionara a la pagina default.aspx. En caso contrario, se elegira la planta a tratar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Planta de administracion"
                Master.NotShowHeader()
                cargarPlantas()
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errMostrarPlantas", ex)
            Response.Redirect(PageBase.PAG_PERMISODENEGADO)
        End Try
    End Sub

    ''' <summary>
    ''' Carga las plantas que de momento se pueden gestionar    
    ''' </summary>    
    Private Sub cargarPlantas()
        Dim myTicket As ELL.TicketTlfno = CType(Session(PageBase.STICKET), ELL.TicketTlfno)
        Dim userComp As New BLL.UsuariosComponent
        Dim lPlantas As List(Of SabLib.ELL.Planta) = userComp.getPlantasAdministrador(myTicket.IdUser)
        If (lPlantas.Count = 1) Then 'si solo hay una planta, se redirecciona directamente
            myTicket.IdPlantaActual = lPlantas.Item(0).Id
            myTicket.Planta = lPlantas.Item(0).Nombre
            Response.Redirect(PageBase.PAG_ADMINISTRACION, False)
        ElseIf (lPlantas.Count > 1) Then
            lPlantas.Sort(Function(o1 As SabLib.ELL.Planta, o2 As SabLib.ELL.Planta) o1.Nombre < o2.Nombre)
            ddlPlantas.Items.Add(itzultzaileWeb.Itzuli("seleccioneUno"))
            ddlPlantas.DataSource = lPlantas
            ddlPlantas.DataBind()
        Else
            Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
        End If
    End Sub

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnEntrar)
        End If
    End Sub

    ''' <summary>
    ''' Redirecciona a la pagina default.aspx una vez informada la planta en la que se va a trabajar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnEntrar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnEntrar.Click
        If (ddlPlantas.SelectedValue = "0") Then
            Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Seleccione una planta")
        Else
            Master.Ticket.IdPlantaActual = ddlPlantas.SelectedValue
            Master.Ticket.Planta = ddlPlantas.SelectedItem.Text
            log.Info("Login - " & Master.Ticket.NombreUsuario & " (" & Server.MachineName & ") va a gestionar la planta " & ddlPlantas.SelectedValue)
            Response.Redirect(PageBase.PAG_ADMINISTRACION, False)
        End If
    End Sub

End Class