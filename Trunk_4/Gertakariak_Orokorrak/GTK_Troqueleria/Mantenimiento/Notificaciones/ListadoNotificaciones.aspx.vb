Public Class ListadoNotificaciones
    Inherits PageBase
#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak
#End Region

#Region "Eventos de Pagina"
    Private Sub Listado_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Eventos de Objetos"
    Protected WithEvents gvNotificaciones As Global.System.Web.UI.WebControls.GridView

    Private Sub dlNotificaciones_ItemDataBound(sender As Object, e As DataListItemEventArgs) Handles dlNotificaciones.ItemDataBound
        Try
            Dim Objeto As DataList = sender
            If Objeto.Controls.Count > 0 Then
                Dim Item As DataListItem = e.Item
                Dim tipoNotificacion As BatzBBDD.ESTRUCTURA = DirectCast(Item.DataItem, BatzBBDD.ESTRUCTURA)

                gvNotificaciones = Item.FindControl("gvNotificaciones")
                gvNotificaciones.Caption = If(tipoNotificacion.DESCRIPCION Is Nothing OrElse String.IsNullOrWhiteSpace(tipoNotificacion.DESCRIPCION), "?", tipoNotificacion.DESCRIPCION)
                gvNotificaciones.DataSource = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = tipoNotificacion.ID Select Reg).ToList
                gvNotificaciones.DataBind()
            End If
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub gvNotificaciones_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvNotificaciones.RowDataBound
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
    Public Sub gvNotificaciones_RowDeleting(sender As Object, e As GridViewDeleteEventArgs) Handles gvNotificaciones.RowDeleting
        Dim Tabla As GridView = sender
        e.Cancel = True

        Dim tipoNotificacion As BatzBBDD.ESTRUCTURA = Nothing
        Dim Reg_ID As Integer = CInt(Tabla.DataKeys(e.RowIndex).Value)
        Dim Usr As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = Reg_ID Select Reg).SingleOrDefault

        If Usr IsNot Nothing Then
            tipoNotificacion = Usr.ESTRUCTURA_Origen
            BBDD.ESTRUCTURA.DeleteObject(Usr)
            BBDD.SaveChanges()
        End If
    End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        Dim lTipoNotificacion = From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.IdNotificaciones Select Reg Order By Reg.DESCRIPCION
        dlNotificaciones.DataSource = lTipoNotificacion.ToList : dlNotificaciones.DataBind()
    End Sub
#End Region

End Class