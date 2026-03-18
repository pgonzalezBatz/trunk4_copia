Public Class UsuarioUG
    Inherits PageBase
#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak

#End Region
#Region "Eventos de Pagina"
    Private Sub UsuarioUG_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Eventos de Objetos"
    Private Sub btnAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar.Click
        Try
            '-----------------------------------------------------------------------
            'Obtenemos los usuarios para la UG seleccionada
            '-----------------------------------------------------------------------
            Dim lUSUARIOS_UG As New List(Of BatzBBDD.ESTRUCTURA)
            Dim Estructura = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = My.Settings.IdNotificacionesUG Select Reg).SingleOrDefault
            If Estructura IsNot Nothing Then
                Dim UG As BatzBBDD.ESTRUCTURA = Estructura.ESTRUCTURA1.Where(Function(o) o.ORDEN = ddlUG.SelectedValue).SingleOrDefault
                If UG IsNot Nothing Then lUSUARIOS_UG = UG.ESTRUCTURA1.ToList
            End If
            '-----------------------------------------------------------------------
            Dim lOtros As List(Of Integer) = If(Request("hd_IdOtros") Is Nothing, New List(Of Integer), Request("hd_IdOtros").Split(",").Select(Function(n) Integer.Parse(n)).ToList())
            'LEFT OUTER JOIN - LEFT JOIN
            Dim lUsr = From Usr In lOtros
                       Group Join UsrG In lUSUARIOS_UG On UsrG.ORDEN Equals Usr Into lUsr_UsrG = Group From grp In lUsr_UsrG.DefaultIfEmpty
                       Where grp Is Nothing
                       Select Usr

            If lUsr.Any Then
                'Comprobamos que exista la UG en la estructura.
                Dim UG As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.IdNotificacionesUG And Reg.ORDEN = ddlUG.SelectedValue Select Reg).SingleOrDefault

                If UG Is Nothing Then
                    UG = New BatzBBDD.ESTRUCTURA With {.IDITURRIA = My.Settings.IdNotificacionesUG, .ORDEN = ddlUG.SelectedValue, .DESCRIPCION = ddlUG.SelectedItem.Text}
                    BBDD.ESTRUCTURA.AddObject(UG)
                    BBDD.SaveChanges()
                End If
                For Each ID_Usr In lUsr
                    Dim UsrSab As BatzBBDD.SAB_USUARIOS = (From Reg As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Where Reg.ID = ID_Usr Select Reg).SingleOrDefault
                    If UsrSab IsNot Nothing Then
                        BBDD.ESTRUCTURA.AddObject(New BatzBBDD.ESTRUCTURA With {.IDITURRIA = UG.ID, .ORDEN = UsrSab.ID, .DESCRIPCION = String.Format("{0} {1} {2}", UsrSab.NOMBRE, UsrSab.APELLIDO1, UsrSab.APELLIDO2)})
                    End If
                Next
                BBDD.SaveChanges()
            End If

            Response.Redirect("~/Mantenimiento/Notificaciones/ListadoUG.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub

#End Region
#Region "Funciones y Procesos"
    Sub CargarDatos()
        ddlUG.DataSource = BBDD.CPLANTEGI.OrderBy(Function(o) o.DESCRI).ToList.Select(Function(o) New ListItem(o.DESCRI, o.LANTEGI))
        ddlUG.DataBind()
        lvOtros.DataBind()
    End Sub
#End Region
End Class