Imports System.Globalization

Partial Public Class Calendario
    Inherits System.Web.UI.UserControl

#Region "Properties"

    Public Property Fecha() As String
        Get
            Return txtFecha.Text.Trim 'Devolviendo lo que habia en el campo fecha, iba mal
            'Return calendarAjax.SelectedDate.ToString("dd/MM/yyyy")
        End Get
        Set(ByVal value As String)
            If (value Is Nothing) Then
                txtFecha.Text = String.Empty
            Else
                If (value <> String.Empty) Then
                    calendarAjax.SelectedDate = value
                    txtFecha.Text = calendarAjax.SelectedDate.ToShortDateString
                Else
                    calendarAjax.SelectedDate = DateTime.Now
                    txtFecha.Text = DateTime.Now.ToShortDateString()
                End If
            End If
        End Set
    End Property

    Public WriteOnly Property HabilitarCalendario() As Boolean
        Set(ByVal value As Boolean)
            Me.txtFecha.Enabled = value
        End Set
    End Property

    Public ReadOnly Property IsFechaValida() As Boolean
        Get
            Dim fechaResult As DateTime
            If (DateTime.TryParse(Fecha, CultureInfo.CurrentCulture.DateTimeFormat, DateTimeStyles.None, fechaResult)) Then
                Return True
            Else
                Return False
            End If
        End Get
    End Property

#End Region

#Region "Seleccionar una fecha del calendario"

    Protected Sub Calendar_SelectionChanged(ByVal sender As Object, ByVal e As EventArgs)
        PopupControlExtender1.Commit(calendarAjax.SelectedDate.ToShortDateString())
    End Sub

#End Region

End Class