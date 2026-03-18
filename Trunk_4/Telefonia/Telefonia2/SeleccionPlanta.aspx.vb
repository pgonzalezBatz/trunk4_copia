Imports TelefoniaLib

Partial Public Class SeleccionPlanta
    Inherits PageBase

    ''' <summary>
    ''' Se indican los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTitulo) : itzultzaileWeb.Itzuli(labelSelPlanta) : itzultzaileWeb.Itzuli(btnIr)
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
                Dim myTicket As ELL.TicketTlfno = CType(Session(PageBase.STICKET), ELL.TicketTlfno)
                Dim userComp As New BLL.UsuariosComponent
                Dim lPlantas As List(Of SabLib.ELL.Planta)
                If (myTicket.EsAdministrador Or myTicket.EsAdministradorPlanta) Then
                    lPlantas = userComp.getPlantasAdministrador(myTicket.IdUser, myTicket.EsAdministrador)
                Else
                    lPlantas = userComp.getPlantasGestion(myTicket.IdUser)
                End If
                If (lPlantas.Count = 1) Then 'si solo hay una planta, se redirecciona directamente
                    myTicket.IdPlantaActual = lPlantas.Item(0).Id
                    myTicket.Planta = lPlantas.Item(0).Nombre
                    Response.Redirect(PageBase.PAG_ADMINISTRACION, False)
                ElseIf (lPlantas.Count > 1) Then
                    lPlantas.Sort(Function(o1 As SabLib.ELL.Planta, o2 As SabLib.ELL.Planta) o1.Nombre < o2.Nombre)
                    ddlPlantas.Items.Add(itzultzaileWeb.Itzuli("seleccioneUno"))
                    ddlPlantas.DataSource = lPlantas
                    ddlPlantas.DataTextField = "Nombre"
                    ddlPlantas.DataValueField = "Id"
                    ddlPlantas.DataBind()
                Else
                    Response.Redirect(PageBase.PAG_PERMISODENEGADO, False)
                End If
            End If
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
            Dim myTicket As ELL.TicketTlfno = CType(Session(PageBase.STICKET), ELL.TicketTlfno)
            If (myTicket IsNot Nothing) Then
                myTicket.IdPlantaActual = ddlPlantas.SelectedValue
                myTicket.Planta = ddlPlantas.SelectedItem.Text
            End If
            log.Info("Acceso a la administracion de la planta")
            Response.Redirect(PageBase.PAG_ADMINISTRACION, False)
        End If
    End Sub
End Class