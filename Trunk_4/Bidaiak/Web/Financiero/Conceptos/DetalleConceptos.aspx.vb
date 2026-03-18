Imports BidaiakLib

Public Class DetalleConceptos
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Pinta el estado de un curso o de un grupo de un curso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If (Not Page.IsPostBack) Then
                Dim concep As String = Request.QueryString("conc")
                mostrarDetalle(concep)
            End If
        Catch batzEx As BidaiakLib.BatzException
            pnlError.Visible = True
            lblError.Text = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelInfo)
        End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Busca en los movimientos de visa, todos los datos generales asociados al conceptos
    ''' </summary>
    ''' <param name="concepto"></param>    
    Private Sub mostrarDetalle(ByVal concepto As String)
        Try
            Dim movBLL As New BLL.VisasBLL
            Dim lMov As List(Of ELL.Visa.Movimiento) = movBLL.loadMovimientos(New ELL.Visa.Movimiento With {.Sector = concepto}, CInt(Session("IdPlanta")))
            Dim lMovResul = Nothing
            If (lMov IsNot Nothing) Then lMovResul = From mov As ELL.Visa.Movimiento In lMov Order By mov.Establecimiento, mov.Localidad, mov.NombreUsuario Select mov.Establecimiento, mov.Localidad, mov.NombreUsuario Distinct

            gvDatosConc.DataSource = lMovResul
            gvDatosConc.DataBind()
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar el detalle del concepto", ex)
        End Try
    End Sub

#End Region

End Class