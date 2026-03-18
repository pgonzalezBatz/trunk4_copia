Imports System.Globalization

Public Class CalendarioAnticipo
    Inherits System.Web.UI.UserControl

#Region "Properties"

    ''' <summary>
    ''' Fecha a seleccionar en el calendario
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Fecha() As String
        Get
            Return txtFecha.Text
        End Get
        Set(ByVal value As String)
            txtFecha.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Se habilita el campo de texto para introducir la fecha
    ''' </summary>
    ''' <value></value>    
    Public WriteOnly Property HabilitarCalendario() As Boolean
        Set(ByVal value As Boolean)
            txtFecha.Enabled = value
        End Set
    End Property

    ''' <summary>
    ''' Comprueba si la fecha introducida es valida. Esta funcion esta para cuando se habilita el textbox
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public ReadOnly Property IsFechaValida() As Boolean
        Get
            Dim fechaResult As DateTime
            Return (DateTime.TryParse(Fecha, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, fechaResult))
        End Get
    End Property

    ''' <summary>
    ''' Indica la fecha del primer dia seleccionable. Los dias anteriores estaran deshabilitados
    ''' </summary>
    ''' <value></value>    
    Public Property PrimeraFechaSeleccionable As Date
        Get
            If (ViewState("PrimeraFechaSel") Is Nothing) Then
                Return Date.MinValue
            Else
                Return CDate(ViewState("PrimeraFechaSel"))
            End If
        End Get
        Set(ByVal value As Date)
            ViewState("PrimeraFechaSel") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la fecha del ultima dia seleccionable. Los dias posteriores estaran deshabilitados
    ''' </summary>
    ''' <value></value>    
    Public Property UltimaFechaSeleccionable As Date
        Get
            If (ViewState("UltimaFechaSel") Is Nothing) Then
                Return Date.MinValue
            Else
                Return CDate(ViewState("UltimaFechaSel"))
            End If
        End Get
        Set(ByVal value As Date)
            ViewState("UltimaFechaSel") = value
        End Set
    End Property

    ''' <summary>
    ''' Es para indicar que todo el calendario este disponible. Si no esta disponible, deshabilitar las fechas que 
    ''' </summary>
    ''' <value></value>    
    Public Property TodoElCalendarioDisponible As Boolean
        Get
            If (ViewState("CalDispo") Is Nothing) Then
                Return False
            Else
                Return CBool(ViewState("CalDispo"))
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("CalDispo") = value
        End Set
    End Property

    Private itzultzaileWeb As New itzultzaile

#End Region

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CalendarioAnticipo_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        Dim cultura As String = CType(Session("Ticket"), SabLib.ELL.Ticket).Culture
        Dim script As New StringBuilder
        Dim disabledDates As New StringBuilder
        If (Not TodoElCalendarioDisponible) Then
            Dim primeraFecha, ultimaFecha, myFecha As Date
            primeraFecha = PrimeraFechaSeleccionable
            ultimaFecha = UltimaFechaSeleccionable
            If (primeraFecha <> DateTime.MinValue AndAlso ultimaFecha <> DateTime.MinValue) Then
                myFecha = primeraFecha.AddMonths(-6)
                While myFecha <= ultimaFecha.AddMonths(6)
                    If (myFecha < primeraFecha OrElse myFecha > ultimaFecha OrElse (myFecha.DayOfWeek = DayOfWeek.Saturday OrElse myFecha.DayOfWeek = DayOfWeek.Sunday)) Then
                        If (disabledDates.Length > 0) Then disabledDates.Append(",")
                        disabledDates.Append("'" & String.Format("{0}/{1}/{2}", myFecha.Year, myFecha.Month, myFecha.Day) & "'")
                    End If
                    myFecha = myFecha.AddDays(1)
                End While
            End If
        End If
        script.AppendLine("$('#dtFecha').datetimepicker({showClear:true,locale:'" & cultura & "',disabledDates:[" & disabledDates.ToString & "],format:'" & Threading.Thread.CurrentThread.CurrentCulture.DateTimeFormat.ShortDatePattern.ToUpper & "'});")
        AjaxControlToolkit.ToolkitScriptManager.RegisterStartupScript(Me, Me.GetType, "DatePicker", script.ToString, True)
    End Sub

End Class