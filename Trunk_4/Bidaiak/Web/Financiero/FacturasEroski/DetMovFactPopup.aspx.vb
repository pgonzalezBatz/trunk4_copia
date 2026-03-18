Imports BidaiakLib

Public Class DetMovFacPopup
    Inherits Page

    Private itzultzaileWeb As New Itzultzaile

    ''' <summary>
    ''' Carga los asientos del departamento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Session("Ticket") Is Nothing Then
                    Dim cultureInfo As Globalization.CultureInfo = Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                    Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                    Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
                    Culture = cultureInfo.Name
                Else
                    Culture = CType(Session("Ticket"), SabLib.ELL.Ticket).Culture
                End If
            End If
            pnlError.Visible = False
            mostrarDetalle(Request.QueryString("codDepto"), Request.QueryString("factu"))
        Catch batzEx As BatzException
            pnlError.Visible = True
            lblMensa.Text = itzultzaileWeb.Itzuli("Error al mostrar el detalle del asiento")
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el detalle del departamento
    ''' </summary>
    ''' <param name="codDepto">Codigo del departamento</param>    
    ''' <param name="factura">Numero de la factura</param>
    Private Sub mostrarDetalle(ByVal codDepto As String, ByVal factura As String)
        Try
            Dim cuentaBLL As New BLL.DepartamentosBLL
            Dim idDepto As Integer = 0
            If (codDepto <> String.Empty) Then idDepto = CInt(codDepto)
            If (idDepto > 0) Then
                Dim oCuenta As ELL.Departamento = cuentaBLL.loadInfo(codDepto, CInt(Session("IdPlanta")))
                If (oCuenta IsNot Nothing) Then lblDeptCuenta.Text = oCuenta.Departamento & " (" & itzultzaileWeb.Itzuli("Normal") & " - " & oCuenta.Cuenta18 & " | " & itzultzaileWeb.Itzuli("Reducida") & " - " & oCuenta.Cuenta8 & " | " & itzultzaileWeb.Itzuli("Exenta") & " - " & oCuenta.Cuenta0 & ")"
            End If
            Dim lAsientos As List(Of ELL.AsientoContableEroskiTmp) = cuentaBLL.loadAsientosContEroskiTmp(CInt(Session("IdPlanta")), idDepto, False, factura)
            gvInfo.DataSource = lAsientos
            gvInfo.DataBind()
            Dim totalBase18 As Label = CType(gvInfo.FooterRow.Cells(3).FindControl("lblTotalBase18"), Label)
            Dim totalBase8 As Label = CType(gvInfo.FooterRow.Cells(5).FindControl("lblTotalBase8"), Label)
            Dim totalBase0 As Label = CType(gvInfo.FooterRow.Cells(7).FindControl("lblTotalBase0"), Label)
            Dim totalCuota18 As Label = CType(gvInfo.FooterRow.Cells(4).FindControl("lblTotalCuota18"), Label)
            Dim totalCuota8 As Label = CType(gvInfo.FooterRow.Cells(6).FindControl("lblTotalCuota8"), Label)
            Dim totalCuota0 As Label = CType(gvInfo.FooterRow.Cells(8).FindControl("lblTotalCuota0"), Label)
            Dim totalRegEsp As Label = CType(gvInfo.FooterRow.Cells(9).FindControl("lblTotalRegEsp"), Label)
            Dim base18, base8, base0, cuota18, cuota8, cuota0, regesp As Decimal
            For Each oAsiento As ELL.AsientoContableEroskiTmp In lAsientos
                base18 += PageBase.DecimalValue(oAsiento.BaseIG_18) : base8 += PageBase.DecimalValue(oAsiento.BaseIR_8) : base0 += PageBase.DecimalValue(oAsiento.BaseExe_0)
                cuota18 += PageBase.DecimalValue(oAsiento.Cuota_18) : cuota8 += PageBase.DecimalValue(oAsiento.Cuota_8) : cuota0 += PageBase.DecimalValue(oAsiento.Cuota_0)
                regesp += PageBase.DecimalValue(oAsiento.RegEsp)
            Next
            totalBase18.Text = base18 : totalBase8.Text = base8 : totalBase0.Text = base0
            totalCuota18.Text = cuota18 : totalCuota8.Text = cuota8 : totalCuota0.Text = cuota0
            totalRegEsp.Text = regesp
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar el detalle de la cuenta", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlaza el listado con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvInfo_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvInfo.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oAsiento As ELL.AsientoContableEroskiTmp = e.Row.DataItem
            Dim lblNumTrab As Label = CType(e.Row.FindControl("lblNumTrab"), Label)
            Dim lblPersona As Label = CType(e.Row.FindControl("lblPersona"), Label)
            Dim lblProducto As Label = CType(e.Row.FindControl("lblProducto"), Label)
            Dim lblBase0 As Label = CType(e.Row.FindControl("lblBase0"), Label)
            Dim lblBase8 As Label = CType(e.Row.FindControl("lblBase8"), Label)
            Dim lblBase18 As Label = CType(e.Row.FindControl("lblBase18"), Label)
            Dim lblCuota0 As Label = CType(e.Row.FindControl("lblCuota0"), Label)
            Dim lblCuota8 As Label = CType(e.Row.FindControl("lblCuota8"), Label)
            Dim lblCuota18 As Label = CType(e.Row.FindControl("lblCuota18"), Label)
            Dim lblRegEsp As Label = CType(e.Row.FindControl("lblRegEsp"), Label)
            lblNumTrab.Text = oAsiento.numTrabajador : lblPersona.Text = oAsiento.Nombre
            lblProducto.Text = oAsiento.Producto
            lblBase0.Text = oAsiento.BaseExe_0 : lblBase8.Text = oAsiento.BaseIR_8 : lblBase18.Text = oAsiento.BaseIG_18
            lblCuota18.Text = oAsiento.Cuota_18 : lblCuota8.Text = oAsiento.Cuota_8 : lblCuota0.Text = oAsiento.Cuota_0
            lblRegEsp.Text = oAsiento.RegEsp
        End If
    End Sub

End Class