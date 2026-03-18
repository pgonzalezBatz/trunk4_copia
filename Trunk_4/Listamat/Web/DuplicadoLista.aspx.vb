Public Partial Class DuplicadoLista
    Inherits System.Web.UI.Page
    Private strCn = ConfigurationManager.ConnectionStrings("LISTAMAT").ConnectionString

    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lst = dbAccess.GetOFOPs(Session("IdTroqueleria"), strCn)
        BindDDLOfs(lst)
        If Not Request.Form(ddlOfSource.UniqueID) Is Nothing AndAlso Request.Form(ddlOfSource.UniqueID) > 0 Then
            'La OF ha sido seleccionada
            Dim lstOPSource = lst.FindAll(Function(s) s(0) = Request.Form(ddlOfSource.UniqueID))
            BindDDLOp(lst, Request.Form(ddlOfSource.UniqueID), ddlOpSource)
        End If
        If Not Request.Form(ddlOfDestination.UniqueID) AndAlso Request.Form(ddlOfDestination.UniqueID) > 0 Then
            'La OF ha sido seleccionada
            Dim lstOPDestination = lst.FindAll(Function(s) s(0) = Request.Form(ddlOfDestination.UniqueID))
            BindDDLOp(lst, Request.Form(ddlOfSource.UniqueID), ddlOpDestination)
        End If
    End Sub
    Protected Sub BindDDLOfs(ByVal lst As List(Of String()))
        Dim lstOF = lst.GroupBy(Function(s) s(0))
        ddlOfSource.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", 0))
        ddlOfDestination.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", 0))
        For Each g In lstOF
            ddlOfSource.Items.Add(New ListItem(g.Key, g.Key))
            ddlOfDestination.Items.Add(New ListItem(g.Key, g.Key))
        Next
    End Sub
    Private Sub BindDDLOp(ByVal lst As List(Of String()), ByVal ord As String, ByVal ddl As DropDownList)
        ddl.Items.Clear()
        ddl.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", 0))
        Dim lstOP = lst.FindAll(Function(s) s(0) = ord)
        For Each i In lstOP
            ddl.Items.Add(New ListItem(i(1), i(1)))
        Next
    End Sub
    Protected Sub Duplicar(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsValid Then
            Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
            dbAccess.AsegurarExistenciaEnCpliscab(ddlOfDestination.SelectedValue, ddlOpDestination.SelectedValue, strCn)
            dbAccess.DuplicarLista(ddlOfSource.SelectedValue, ddlOpSource.SelectedValue, ddlOfDestination.SelectedValue, _
                                   ddlOpDestination.SelectedValue, strCn)
        End If
    End Sub

    Private Sub cvDuplicar_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs)
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lst = dbAccess.GetMarcasEnComun(ddlOfSource.SelectedValue, ddlOpSource.SelectedValue, ddlOfDestination.SelectedValue,
                                            ddlOpDestination.SelectedValue, strCn)
        If lst Is Nothing OrElse lst.Count = 0 Then
            args.IsValid = True
        Else
            args.IsValid = False
        End If
    End Sub
End Class