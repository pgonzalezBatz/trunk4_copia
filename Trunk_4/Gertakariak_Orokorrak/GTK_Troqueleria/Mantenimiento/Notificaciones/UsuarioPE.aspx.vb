Public Class UsuarioPE
    Inherits PageBase
#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak

#End Region
#Region "Eventos de Pagina"
    Private Sub UsuarioPE_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
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
            ''''TODO: AQUÍ ESTÁ AÑADIENDO UNA NUEVA ESTRUCTURA CUANDO YA EXISTE!
            '-----------------------------------------------------------------------
            'Obtenemos los usuarios para la OF seleccionada
            '-----------------------------------------------------------------------
            Dim lUSUARIOS_OF As New List(Of BatzBBDD.ESTRUCTURA)
            Dim Estructura = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.ID = My.Settings.IdPEs Select Reg).SingleOrDefault
            Dim ofData = txt_OFOPM.Text.Split("-")(0)
            If Estructura IsNot Nothing Then
                'Dim UG As BatzBBDD.ESTRUCTURA = Estructura.ESTRUCTURA1.Where(Function(o) o.ID = ddl.SelectedValue).SingleOrDefault
                Dim UG As BatzBBDD.ESTRUCTURA = Estructura.ESTRUCTURA1.Where(Function(o) o.ID = ofData).SingleOrDefault
                If UG IsNot Nothing Then lUSUARIOS_OF = UG.ESTRUCTURA1.ToList
            End If
            '-----------------------------------------------------------------------
            Dim lOtros As List(Of Integer) = If(Request("hd_IdOtros") Is Nothing, New List(Of Integer), Request("hd_IdOtros").Split(",").Select(Function(n) Integer.Parse(n)).ToList())
            'LEFT OUTER JOIN - LEFT JOIN
            Dim lUsr = From Usr In lOtros
                       Group Join UsrPE In lUSUARIOS_OF On UsrPE.ORDEN Equals Usr Into lUsr_UsrPE = Group From grp In lUsr_UsrPE.DefaultIfEmpty
                       Where grp Is Nothing
                       Select Usr

            If lUsr.Any Then
                'Comprobamos que exista la UG en la estructura.
                'Dim itemOF As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.IdPEs And Reg.ID = ddl.SelectedValue Select Reg).SingleOrDefault
                Dim itemOF As BatzBBDD.ESTRUCTURA = (From Reg As BatzBBDD.ESTRUCTURA In BBDD.ESTRUCTURA Where Reg.IDITURRIA = My.Settings.IdPEs And Reg.ID = ofData Select Reg).SingleOrDefault

                If itemOF Is Nothing Then
                    'itemOF = New BatzBBDD.ESTRUCTURA With {.IDITURRIA = My.Settings.IdPEs, .ID = ddl.SelectedValue, .DESCRIPCION = ddl.SelectedItem.Text}
                    itemOF = New BatzBBDD.ESTRUCTURA With {.IDITURRIA = My.Settings.IdPEs, .DESCRIPCION = ofData}
                    BBDD.ESTRUCTURA.AddObject(itemOF)
                    BBDD.SaveChanges()
                End If
                For Each ID_Usr In lUsr
                    Dim UsrSab As BatzBBDD.SAB_USUARIOS = (From Reg As BatzBBDD.SAB_USUARIOS In BBDD.SAB_USUARIOS Where Reg.ID = ID_Usr Select Reg).SingleOrDefault
                    If UsrSab IsNot Nothing Then
                        BBDD.ESTRUCTURA.AddObject(New BatzBBDD.ESTRUCTURA With {.IDITURRIA = itemOF.ID, .ORDEN = UsrSab.ID, .DESCRIPCION = String.Format("{0} {1} {2}", UsrSab.NOMBRE, UsrSab.APELLIDO1, UsrSab.APELLIDO2)})
                    End If
                Next
                BBDD.SaveChanges()
            End If

            Response.Redirect("~/Mantenimiento/Notificaciones/ListadoPE.aspx", True)
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
        'ddl.DataSource = BBDD.ESTRUCTURA.Where(Function(f) f.IDITURRIA = My.Settings.IdPEs).OrderBy(Function(o) o.ORDEN).ToList.Select(Function(o) New ListItem(o.DESCRIPCION, o.ID))
        'ddl.DataBind()
        lvOtros.DataBind()
    End Sub
#End Region
End Class