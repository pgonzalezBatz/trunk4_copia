Public Partial Class ListaAF
    Inherits System.Web.UI.Page
    Private strCn = ConfigurationManager.ConnectionStrings("LISTAMAT").ConnectionString

    Protected Overrides Sub InitializeCulture()
        If Session("IdTroqueleria") Is Nothing OrElse Session("IdUsuario") Is Nothing OrElse Session("IdCultura") Is Nothing Then
            'No deberia estar aqui
            Response.Redirect("Default.aspx")
        End If
        MyBase.InitializeCulture()
        Culture = Session("IdCultura")
    End Sub

    Private Sub ListaAF_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request("tipolista") Is Nothing Then
            'TODO: malo
        End If
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lst = dbAccess.GetOFOPs(Session("IdTroqueleria"), strCn)
        BindDDLOf(lst)
        If Not Request("ord") Is Nothing Then
            ddlOf.SelectedValue = Request("ord")
            BindDDLOp(lst, Request("ord"))
            ddlOp.SelectedValue = Request("op")
            'La OP ha sido seleccionada
            BindMarcas(Request("ord"), Request("op"))
        End If
        If Not Request.Form(ddlOf.UniqueID) Is Nothing Then
            'La OF ha sido seleccionada
            Dim lstOP = lst.FindAll(Function(s) s(0) = Request.Form(ddlOf.UniqueID))
            BindDDLOp(lst, Request.Form(ddlOf.UniqueID))
        End If
        If Not Request.Form(ddlOp.UniqueID) Is Nothing AndAlso Request.Form(ddlOp.UniqueID) >= 0 Then
            'La OP ha sido seleccionada
            BindMarcas(Request.Form(ddlOf.UniqueID), Request.Form(ddlOp.UniqueID))
        End If
        Visivility()
    End Sub
    Private Sub Visivility()
        ddlOp.Visible = (Not Request.Form(ddlOf.UniqueID) Is Nothing AndAlso Request.Form(ddlOf.UniqueID) > 0) OrElse Not Request("ord") Is Nothing
        d1.Visible = ddlOp.Visible AndAlso ((Not Request("op") Is Nothing AndAlso Request("op") >= 0) OrElse _
                    (Not Request.Form(ddlOp.UniqueID) Is Nothing AndAlso Request.Form(ddlOp.UniqueID) >= 0))
        rptMarcas.Visible = d1.Visible
    End Sub
    Protected Sub BindDDLOf(ByVal lst As List(Of String()))
        Dim lstOF = lst.GroupBy(Function(s) s(0))
        ddlOf.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", 0))
        For Each g In lstOf
            ddlOf.Items.Add(New ListItem(g.Key, g.Key))
        Next
    End Sub
    Private Sub BindDDLOp(ByVal lst As List(Of String()), ByVal ord As String)
        ddlOp.Items.Clear()
        ddlOp.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", -1))
        Dim lstOP = lst.FindAll(Function(s) s(0) = ord)
        For Each i In lstOP
            ddlOp.Items.Add(New ListItem(i(1), i(1)))
        Next
    End Sub
    Private Sub BindMarcas(ByVal ord As Integer, ByVal op As Integer)
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        rptMarcas.DataSource = dbAccess.GetMarcasSinLanzar(Request("tipolista"), ord, op, strCn)
        rptMarcas.DataBind()
    End Sub
    Protected Sub Eliminar(ByVal sender As Object, ByVal e As ImageClickEventArgs)
        If Page.IsValid Then
            Dim imgbtn As ImageButton = sender
            Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
            dbAccess.EliminarMarca(Request("tipolista"), ddlOf.SelectedValue, ddlOp.SelectedValue, imgbtn.CommandArgument, strCn)
            BindMarcas(ddlOf.SelectedValue, ddlOp.SelectedValue)
        End If
    End Sub
    Protected Sub Lanzar(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsValid Then
            Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
            For Each i As RepeaterItem In rptMarcas.Items
                Dim cb As CheckBox = i.FindControl("cbLanzar")
                If cb.Checked Then
                    Dim ltl As Literal = i.FindControl("ltlMarca")
                    dbAccess.LanzarMarca(Request("tipolista"), ddlOf.SelectedValue, ddlOp.SelectedValue, ltl.Text, strCn)
                End If
            Next
            dbAccess.ActualizarOtpropro(ddlOf.SelectedValue, ddlOp.SelectedValue, strCn)
            BindMarcas(ddlOf.SelectedValue, ddlOp.SelectedValue)
        End If
    End Sub
    Protected Sub Ordenar(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsValid Then
            Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
            dbAccess.OrdenarMarcas(Request("tipolista"), ddlOf.SelectedValue, ddlOp.SelectedValue, strCn)
            BindMarcas(ddlOf.SelectedValue, ddlOp.SelectedValue)
        End If
    End Sub
    Protected Function GetEditLink(ByVal mar As String) As String
        Dim str As New StringBuilder
        str.Append("ListaAFMarcas.aspx?tipolista=" + Request("tipolista") + "&ord=")
        If Not Request.Form(ddlOf.UniqueID) Is Nothing Then
            str.Append(Request.Form(ddlOf.UniqueID) + "&op=" + Request.Form(ddlOp.UniqueID))
        Else
            str.Append(Request("ord") + "&op=" + Request("op"))
        End If
        str.Append("&mar=" + mar)
        Return str.ToString
    End Function
    Protected Function GetCopyLink(ByVal mar As String) As String
        Return GetEditLink(mar) + "&cp=true"
    End Function


    Protected Sub SeleccionarLanzamientos(ByVal sender As Object, ByVal e As EventArgs)
        For Each rItem As RepeaterItem In rptMarcas.Items
            CType(rItem.FindControl("cbLanzar"), CheckBox).Checked = True
        Next
    End Sub
End Class