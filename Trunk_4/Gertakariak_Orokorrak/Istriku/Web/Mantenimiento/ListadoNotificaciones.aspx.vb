Public Class ListadoNotificaciones
    Inherits System.Web.UI.Page

#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak
#End Region

#Region "Eventos de Pagina"
    Private Sub ListadoNotificaciones_Init(sender As Object, e As EventArgs) Handles Me.Init
        Dim ESTRUCTURA As BatzBBDD.ESTRUCTURA = (From Reg In BBDD.ESTRUCTURA Where Reg.ID = My.Settings.IdListadoNotificaciones Select Reg).SingleOrDefault
        Titulo.Texto = ESTRUCTURA.DESCRIPCION
    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'End Sub
#End Region

#Region "Acciones"
    Private Sub btnGuardar_Click(sender As Object, e As ImageClickEventArgs) Handles btnGuardar.Click

        Using Transaccion As New TransactionScope
            Dim ListaUsr As List(Of Integer) = If(Request("hd_IdAfectados") Is Nothing, New List(Of Integer), Request("hd_IdAfectados").Split(",").Distinct.Select(Function(o) CInt(o)).ToList)

            '-----------------------------------------------------------
            'Vaciamos la base de datos
            '-----------------------------------------------------------
            Dim lReg As IQueryable(Of BatzBBDD.ESTRUCTURA) = From Reg In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.IdListadoNotificaciones Select Reg
            lReg.ToList().ForEach(Sub(o) BBDD.DeleteObject(o))
            BBDD.SaveChanges()
            '-----------------------------------------------------------

            '-----------------------------------------------------------
            'Creamos los nuevos registros
            '-----------------------------------------------------------
            ListaUsr.ForEach(Sub(o) BBDD.AddToESTRUCTURA(New BatzBBDD.ESTRUCTURA With {.IDITURRIA = My.Settings.IdListadoNotificaciones, .DESCRIPCION = o}))
            BBDD.SaveChanges()
            '-----------------------------------------------------------

            Transaccion.Complete()
        End Using
        BBDD.AcceptAllChanges()

        Response.Redirect("~/Default.aspx")
    End Sub

    Private Sub ListadoNotificaciones_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Dim lReg As IQueryable(Of BatzBBDD.ESTRUCTURA) = From Reg In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.IdListadoNotificaciones Select Reg

        If lReg.Any Then
            Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
            lvAfectados.DataSource = lReg.ToList.Select(Function(o) UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = CInt(o.DESCRIPCION)}, False)).Where(Function(o) o IsNot Nothing)
        End If

        lvAfectados.DataBind()
    End Sub
#End Region
End Class