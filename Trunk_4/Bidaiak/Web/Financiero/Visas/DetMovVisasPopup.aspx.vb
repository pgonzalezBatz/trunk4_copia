Public Class DetMovVisasPopup
    Inherits Page

    Private itzultzaileWeb As New itzultzaile
    Private monedas As List(Of ELL.Moneda)

    ''' <summary>
    ''' Carga los asientos del departamento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                If Session("Ticket") Is Nothing Then
                    Dim cultureInfo As System.Globalization.CultureInfo = System.Globalization.CultureInfo.CreateSpecificCulture("es-ES")
                    System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo
                    System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo
                    Culture = cultureInfo.Name
                Else
                    Culture = CType(Session("Ticket"), SabLib.ELL.Ticket).Culture
                End If
            End If
            pnlError.Visible = False
            mostrarDetalle(Request.QueryString("codDepto"), Request.QueryString("tipomov"))
        Catch batzEx As BatzException
            pnlError.Visible = True
            lblMensa.Text = itzultzaileWeb.Itzuli("Error al mostrar el detalle")
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el detalle del departamento
    ''' </summary>
    ''' <param name="codDepto">Codigo del departamento</param>    
    ''' <param name="tipoMov">Tipo de movimiento. 0:Gasto,1:Cuota,2:Visa excepcion</param>
    Private Sub mostrarDetalle(ByVal codDepto As String, ByVal tipoMov As String)
        Try
            Dim cuentaBLL As New BLL.DepartamentosBLL
            Dim xbatBLL As New BLL.XbatBLL
            Dim visasBLL As New BLL.VisasBLL
            Dim totalImpEur As Decimal = 0
            Dim idUser As Integer
            Dim idPlanta As Integer = CInt(Session("IdPlanta"))
            Dim lVisas As List(Of String()) = Nothing
            If (tipoMov = 0 OrElse tipoMov = 1) Then
                lVisas = visasBLL.loadVisasTmp(idPlanta)
                lVisas = lVisas.FindAll(Function(o) o(20) = tipoMov)
            Else
                lVisas = visasBLL.loadVisasExcepcionTmp(idPlanta)
            End If
            Dim lVisasShow As New List(Of Object)
            Dim lAsientos As List(Of String()) = Nothing
            If (tipoMov = 0 OrElse tipoMov = 1) Then
                lAsientos = cuentaBLL.loadAsientosContVisasTmp(idPlanta, codDepto)
                lAsientos = lAsientos.FindAll(Function(o) o(10) = tipoMov)
                For Each sAsiento As String() In lAsientos
                    totalImpEur += PageBase.DecimalValue(sAsiento(5))
                    If (tipoMov = 0 OrElse tipoMov = 1) Then
                        idUser = CInt(sAsiento(0))
                        Dim lVisasUser As List(Of String()) = lVisas.FindAll(Function(o As String()) CInt(o(14)) = idUser And CInt(o(21)) = CInt(codDepto))
                        If (lVisasUser IsNot Nothing AndAlso lVisasUser.Count > 0) Then
                            For Each myVisaUser As String() In lVisasUser
                                lVisasShow.Add(New With {.IdTrab = sAsiento(1), .NombrePersona = sAsiento(2), .Concepto = myVisaUser(2), .ImporteEur = PageBase.DecimalValue(myVisaUser(6)), .Localidad = myVisaUser(10), .Establecimiento = myVisaUser(3), .Fecha = CDate(myVisaUser(4)), .IdMonedaGasto = CInt(myVisaUser(18)), .ImporteMonedaGasto = PageBase.DecimalValue(myVisaUser(19))})
                            Next
                        End If
                    Else
                        If (lVisas IsNot Nothing AndAlso lVisas.Count > 0) Then 'Solo debería haber una
                            For Each myVisaTmp As String() In lVisas
                                lVisasShow.Add(New With {.IdTrab = 0, .NombrePersona = myVisaTmp(0), .Concepto = myVisaTmp(2), .ImporteEur = PageBase.DecimalValue(myVisaTmp(6)), .Localidad = myVisaTmp(9), .Establecimiento = myVisaTmp(3), .Fecha = CDate(myVisaTmp(4)), .IdMonedaGasto = CInt(myVisaTmp(11)), .ImporteMonedaGasto = PageBase.DecimalValue(myVisaTmp(12))})
                            Next
                        End If
                    End If
                Next
            Else
                For Each myVisaTmp As String() In lVisas
                    lVisasShow.Add(New With {.IdTrab = 0, .NombrePersona = myVisaTmp(0), .Concepto = myVisaTmp(2), .ImporteEur = PageBase.DecimalValue(myVisaTmp(6)), .Localidad = myVisaTmp(9), .Establecimiento = myVisaTmp(3), .Fecha = CDate(myVisaTmp(4)), .IdMonedaGasto = CInt(myVisaTmp(11)), .ImporteMonedaGasto = PageBase.DecimalValue(myVisaTmp(12))})
                Next
            End If

            lVisasShow = lVisasShow.OrderBy(Of String)(Function(o) o.NombrePersona).ToList
            monedas = xbatBLL.GetMonedas()
            gvInfo.DataSource = lVisasShow
            gvInfo.DataBind()
            CType(gvInfo.FooterRow.Cells(6).FindControl("lblTotalImporteEur"), Label).Text = totalImpEur & " EUR"
            If (tipoMov = 1) Then 'En las cuotas, estas columnas siempre estan vacias                
                gvInfo.Columns(4).Visible = False
                gvInfo.Columns(5).Visible = False
            End If
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
        If (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oMovVisa = e.Row.DataItem
            CType(e.Row.FindControl("lblNumTrab"), Label).Text = If(oMovVisa.IdTrab > 0, oMovVisa.IdTrab, "")
            CType(e.Row.FindControl("lblPersona"), Label).Text = oMovVisa.NombrePersona
            CType(e.Row.FindControl("lblFecha"), Label).Text = oMovVisa.Fecha.ToShortDateString
            CType(e.Row.FindControl("lblConcepto"), Label).Text = oMovVisa.Concepto
            CType(e.Row.FindControl("lblLocalidad"), Label).Text = oMovVisa.Localidad
            CType(e.Row.FindControl("lblEstablecimiento"), Label).Text = oMovVisa.Establecimiento
            CType(e.Row.FindControl("lblImporteEur"), Label).Text = oMovVisa.ImporteEur & " EUR"
            CType(e.Row.FindControl("lblImporteMonGasto"), Label).Text = oMovVisa.ImporteMonedaGasto & " " & monedas.Find(Function(o) o.Id = oMovVisa.IdMonedaGasto).Abreviatura
        End If
    End Sub

End Class