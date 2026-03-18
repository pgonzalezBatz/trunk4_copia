Imports TelefoniaLib

Partial Public Class NumerosDirectos
    Inherits PageBase

    ''' <summary>
    ''' Carga el listado con los numeros directos y su informacion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            Dim tlfnoComp As New BLL.TelefonoComponent
            Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
            lTlfnoExt = tlfnoComp.NumerosDirectos(Master.Ticket.IdPlantaActual)
            gvDirectos.DataSource = lTlfnoExt
            gvDirectos.DataBind()
        Catch ex As Exception
            Dim batzEx As New BatzException("errCompListar", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>    
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gvDirectos_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvDirectos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            'Estilo para que al posicionarse sobre la fila, se pinte de un color
            e.Row.Attributes.Add("onmouseover", "SartuY(this);")
            e.Row.Attributes.Add("onmouseout", "IrtenY(this);")
        End If
    End Sub

    ''' <summary>
    ''' Devuelve el string de un entero. Si es integer.MinValue, devovera String.Empty
    ''' </summary>
    ''' <param name="oInt">Objeto entero</param>
    ''' <returns>String</returns>    
    Protected Function FormatInt(ByVal oInt As Object) As String
        If (oInt = Integer.MinValue) Then
            Return String.Empty
        Else
            Return oInt.ToString()
        End If
    End Function

End Class