Public Class ListadoUG
    Inherits PageBase
#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak
#End Region

#Region "Eventos de Pagina"
    Private Sub ListadoUG_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Eventos de Objetos"
    Protected WithEvents gvNotificacionesUG As Global.System.Web.UI.WebControls.GridView

    Private Sub dlNotificacionesUG_ItemDataBound(sender As Object, e As DataListItemEventArgs) Handles dlNotificacionesUG.ItemDataBound
        Try
            Dim Objeto As DataList = sender
            If Objeto.Controls.Count > 0 Then
                Dim Item As DataListItem = e.Item
                Dim UG As BatzBBDD.ESTRUCTURA = DirectCast(Item.DataItem, BatzBBDD.ESTRUCTURA)

                gvNotificacionesUG = Item.FindControl("gvNotificacionesUG")
                Dim CPLANTEGI = BBDD.CPLANTEGI.Where(Function(o) o.LANTEGI = UG.ORDEN).SingleOrDefault
                gvNotificacionesUG.Caption = If(CPLANTEGI Is Nothing OrElse String.IsNullOrWhiteSpace(CPLANTEGI.DESCRI), "?", CPLANTEGI.DESCRI)
                gvNotificacionesUG.DataSource = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = UG.ID Select Reg).ToList
                gvNotificacionesUG.DataBind()
            End If
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub gvNotificacionesUG_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvNotificacionesUG.RowDataBound
        Dim Fila As GridViewRow = e.Row
        If Fila.RowType = DataControlRowType.DataRow Then
            Dim DataItem As BatzBBDD.ESTRUCTURA = If(e.Row.DataItem, Nothing)
            Dim lblNombre As Label = Fila.FindControl("lblNombre")
            Dim Usr As BatzBBDD.SAB_USUARIOS = (From Reg As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Where Reg.ID = DataItem.ORDEN).SingleOrDefault
            If Usr Is Nothing Then
                lblNombre.Text = "???"
            Else
                lblNombre.Text = Usr.NOMBRE & " " & Usr.APELLIDO1 & " " & Usr.APELLIDO2
            End If
        End If
    End Sub
    Public Sub gvNotificacionesUG_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvNotificacionesUG.RowDeleting
        Dim Tabla As GridView = sender
        e.Cancel = True

        Dim UG As BatzBBDD.ESTRUCTURA = Nothing
        Dim Reg_ID As Integer = CInt(Tabla.DataKeys(e.RowIndex).Value)
        Dim Usr As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = Reg_ID Select Reg).SingleOrDefault

        If Usr IsNot Nothing Then
            UG = Usr.ESTRUCTURA_Origen
            BBDD.ESTRUCTURA.DeleteObject(Usr)
            BBDD.SaveChanges()
        End If
        'Si la UG no tiene usuario la eliminamos
        If UG IsNot Nothing AndAlso Not UG.ESTRUCTURA1.Any Then
            BBDD.ESTRUCTURA.DeleteObject(UG)
            BBDD.SaveChanges()
        End If
    End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        Dim lUG = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.IdNotificacionesUG Select Reg Order By Reg.DESCRIPCION
        dlNotificacionesUG.DataSource = lUG.ToList : dlNotificacionesUG.DataBind()
    End Sub
#End Region

End Class