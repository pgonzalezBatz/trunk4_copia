Public Class TotalAcordado
    Inherits PageBase
#Region "Propiedades"
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Public Incidencia As New BatzBBDD.GERTAKARIAK
    Dim Funciones As New SabLib.BLL.Utils

    Property gvGertakariak_Propiedades() As gtkGridView
        Get
            If (Session("gvGertakariak_Propiedades") Is Nothing) Then Session("gvGertakariak_Propiedades") = New gtkGridView
            Return CType(Session("gvGertakariak_Propiedades"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("gvGertakariak_Propiedades") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Incidencia = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK Where gtk.ID = gvGertakariak_Propiedades.IdSeleccionado Select gtk).SingleOrDefault
    End Sub

    Private Sub TotalAcordado_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()

        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Dim msg As String = String.Format("Incidencia.ID: {0}", If(Incidencia Is Nothing, "?", Incidencia.ID))
            Log.Error(msg, ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub btnGuarbar_Click(sender As Object, e As ImageClickEventArgs) Handles btnGuarbar.Click
        Try
            Incidencia.OBSERVACIONESCOSTE = If(String.IsNullOrWhiteSpace(txtObservacionesCoste.Text), Nothing, txtObservacionesCoste.Text.Trim)
            Incidencia.COMPENSADO = chkCompensado.Checked

            'If System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator = "." Then
            '    txtTotalAcordado.Text = Replace(txtTotalAcordado.Text, ",", ".")
            'ElseIf System.Globalization.CultureInfo.CurrentCulture.NumberFormat.NumberDecimalSeparator = "," Then
            '    txtTotalAcordado.Text = Replace(txtTotalAcordado.Text, ".", ",")
            'End If
            'txtTotalAcordado.Text = If(String.IsNullOrWhiteSpace(txtTotalAcordado.Text), Nothing, CDec(Funciones.SeparadorDecimal(txtTotalAcordado.Text)))

            Incidencia.TOTALACORDADO = If(String.IsNullOrWhiteSpace(txtTotalAcordado.Text), Nothing, CDec(Funciones.SeparadorDecimal(txtTotalAcordado.Text)))

            BBDD.SaveChanges()

            Response.Redirect("~/Incidencia/Detalle.aspx", True)
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            Log.Debug(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
#Region "Funciones y Procesos"
    Sub CargarDatos()
        Try
            If Incidencia IsNot Nothing Then
                Dim Detalle As New GTK_Troqueleria.Detalle
                Dim CosteReal As Decimal = Incidencia.LINEASCOSTE.Select(Function(o) o.IMPORTE).Sum
                lblCosteReal.Text = Format(CosteReal, "C")
                txtObservacionesCoste.Text = Incidencia.OBSERVACIONESCOSTE
                chkCompensado.Checked = Incidencia.COMPENSADO
                txtTotalAcordado.Text = If(Incidencia.TOTALACORDADO Is Nothing, String.Empty, Incidencia.TOTALACORDADO)

                pnlBotones.Visible = Not (Detalle.fCosteCerrado(Incidencia, PerfilUsuario))
            End If
        Catch ex As ApplicationException
            Log.Debug(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub


#End Region
End Class