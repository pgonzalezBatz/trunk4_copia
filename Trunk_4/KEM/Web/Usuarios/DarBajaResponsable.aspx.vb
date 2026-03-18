Public Class DarBajaResponsable
    Inherits PageBase

    Private idGerente As Integer
    Private nombreGerente As String

#Region "Page Load"

    ''' <summary>
    ''' Carga de la pagina de los usuarios que estan al mando del usuario a dar de baja
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Dim idUser As Integer = CInt(Request.QueryString("id"))
                Dim ticks As Decimal = Request.QueryString("ticks")
                Dim fBaja As Date = New Date(ticks)
                Dim userBLL As New Sablib.BLL.UsuariosComponent
                Dim oUser As Sablib.ELL.Usuario = userBLL.GetUsuario(New Sablib.ELL.Usuario With {.Id = idUser}, False)
                lblUserBaja.Text = oUser.NombreCompleto & " (" & fBaja.ToShortDateString & ")"
                pintarGenteASuCargo(idUser)
            End If
        Catch ex As Exception
            PageBase.WriteLog("Error al cargar la pagina de dar de baja un usuario con gente a su cargo", TipoLog.Err, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al cargar")
        End Try
    End Sub

    ''' <summary>
    ''' Se le dice los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(labelUserBaja)
            itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver)            
        End If
    End Sub

#End Region

#Region "Mostrar"

    ''' <summary>
    ''' Pinta la gente a su cargo
    ''' </summary>
    ''' <param name="idUser">Id user</param>    
    Private Sub pintarGenteASuCargo(ByVal idUser As Integer)
        Dim userBLL As New Sablib.BLL.UsuariosComponent
        Dim oGerente As Sablib.ELL.Usuario = userBLL.GetGerentePlanta(Master.Planta.Id)
        idGerente = oGerente.Id : nombreGerente = oGerente.NombreCompleto
        Dim lColaboradores As List(Of Sablib.ELL.Usuario) = userBLL.GetColaboradores(idUser)
        rptUsers.DataSource = lColaboradores
        rptUsers.DataBind()
    End Sub

    ''' <summary>
    ''' Se enlaza el repeater
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub rptUsers_ItemDataBound(sender As Object, e As RepeaterItemEventArgs) Handles rptUsers.ItemDataBound
        If (e.Item.ItemType = ListItemType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Item.Controls)
        ElseIf (e.Item.ItemType = ListItemType.AlternatingItem Or e.Item.ItemType = ListItemType.Item) Then
            Dim oUser As Sablib.ELL.Usuario = e.Item.DataItem
            Dim lblUsuario As Label = CType(e.Item.FindControl("lblUsuario"), Label)
            Dim hfIdUser As HiddenField = CType(e.Item.FindControl("hfIdUser"), HiddenField)
            Dim acGV As AutocompleteGV = CType(e.Item.FindControl("acGV"), AutocompleteGV)
            lblUsuario.Text = oUser.NombreCompleto : hfIdUser.Value = oUser.Id
            acGV.IdPlanta = Master.Planta.Id
            acGV.Inicializar(idGerente, nombreGerente)
        End If
    End Sub

    ''' <summary>
    ''' Se guardan los responsables de la gente a su cargo y al final se guarda la fecha de baja
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub btnGuardar_Click(sender As Object, e As EventArgs) Handles btnGuardar.Click
        Try
            Dim userBLL As New Sablib.BLL.UsuariosComponent
            Dim lColaboradores As New List(Of Integer())
            Dim acGV As AutocompleteGV
            Dim idUser As Integer
            For Each item As RepeaterItem In rptUsers.Items
                acGV = CType(item.FindControl("acGV"), AutocompleteGV)
                idUser = CType(item.FindControl("hfIdUser"), HiddenField).Value
                lColaboradores.Add(New Integer() {idUser, acGV.SelectedId})
            Next
            Dim fBaja As Date = New Date(CDec(Request.QueryString("ticks")))
            userBLL.SaveBajaResponsableConColaboradores(CInt(Request.QueryString("Id")), fBaja, lColaboradores)
            WriteLog("Se ha dado de baja el usuario " & lblUserBaja.Text & " y se ha asignado el responsable a sus colaboradores", TipoLog.Info)
            Response.Redirect("Usuarios.aspx", False)
        Catch ex As Exception
            WriteLog("Error al guardar el nuevo responsable de los colaboradores del usuario " & Request.QueryString("id"), TipoLog.Err, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("Error al guardar")
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve a la ficha
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnVolver_Click(sender As Object, e As EventArgs) Handles btnVolver.Click
        Response.Redirect("Usuarios.aspx?id=" & Request.QueryString("id"), False)
    End Sub

#End Region

End Class