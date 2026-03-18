Imports BidaiakLib

Public Class ActivDptoPopUp
    Inherits System.Web.UI.Page

    Private itzultzaileWeb As New Itzultzaile

    ''' <summary>
    ''' Carga de la pagina
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
            mostrarDetalle(Request.QueryString("codDepto"), Request.QueryString("idPlanta"))
        Catch batzEx As BatzException
            pnlError.Visible = True
            lblMensa.Text = itzultzaileWeb.Itzuli("Error al mostrar el detalle de las actividades")
        End Try
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelInfo)
        End If
    End Sub

    ''' <summary>
    ''' Muestra el detalle del departamento
    ''' </summary>
    ''' <param name="codDepto">Codigo del departamento</param>    
    ''' <param name="idPlanta">Id de la planta</param>
    Private Sub mostrarDetalle(ByVal codDepto As String, ByVal idPlanta As Integer)
        Try
            Dim activBLL As New BLL.ActividadesBLL
            Dim lActiv As List(Of ELL.Actividad) = activBLL.loadListDpto(codDepto, idPlanta, 0)
            gvActividades.DataSource = lActiv
            gvActividades.DataBind()
        Catch batzEx As BidaiakLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BidaiakLib.BatzException("Error al mostrar las actividades del departamento", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlaza el listado con el gridview
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvActividades_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvActividades.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        End If
    End Sub

End Class