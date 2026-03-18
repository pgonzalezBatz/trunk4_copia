Public Class ConceptosGenericos
    Inherits PageBase

#Region "Property"

    ''' <summary>
    ''' Concepto de las lineas a cargar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property Concepto As String
        Get
            If (String.IsNullOrEmpty(ViewState("Conc"))) Then
                Return String.Empty
            Else
                Return ViewState("Conc")
            End If
        End Get
        Set(ByVal value As String)
            ViewState("Conc") = value
        End Set
    End Property

#End Region

#Region "Eventos de pagina"

    ''' <summary>
    ''' Carga la pagina con los movimientos del sector del parametro recibido
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        Try
            If Not Page.IsPostBack Then
                Master.SetTitle = "Conceptos genericos"
                Concepto = Request.QueryString("conc")
                mostrarDetalle()
            End If
        Catch
        End Try
    End Sub

    ''' <summary>
    ''' Traduccion
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelInfo) : itzultzaileWeb.Itzuli(btnGuardar) : itzultzaileWeb.Itzuli(btnVolver)
        End If
    End Sub

#End Region

#Region "Mostrar detalle"

    ''' <summary>
    ''' Busca en los movimientos de visa, todos los datos generales asociados al concepto
    ''' </summary> 
    Private Sub mostrarDetalle()
        Try
            If (ViewState("ConceptosBatz") Is Nothing) Then
                Dim conceptosBLL As New BLL.ConceptosBLL
                Dim lConceptos As List(Of ELL.Concepto) = conceptosBLL.loadList(Master.IdPlantaGestion)
                If (lConceptos IsNot Nothing) Then lConceptos = lConceptos.OrderBy(Of String)(Function(o) o.Nombre).ToList
                ViewState("ConceptosBatz") = lConceptos
            End If
            Dim movBLL As New BLL.VisasBLL
            Dim lMov As List(Of ELL.Visa.Movimiento) = movBLL.loadMovimientos(New ELL.Visa.Movimiento With {.Sector = Concepto}, Master.IdPlantaGestion, tipoMov:=-1)
            If (lMov IsNot Nothing) Then lMov = lMov.OrderBy(Of Date)(Function(o) o.Fecha).ToList
            gvConceptos.DataSource = lMov
            gvConceptos.DataBind()
            btnGuardar.Visible = (lMov IsNot Nothing AndAlso lMov.Count > 0)
            If (lMov.Count <= 0) Then log.Info("No existen mas movimientos para el concepto generico '" & Concepto & "'")
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar el detalle del concepto", ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvConceptos_RowDataBound(ByVal sender As Object, ByVal e As GridViewRowEventArgs) Handles gvConceptos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim oMov As ELL.Visa.Movimiento = e.Row.DataItem
            Dim ddlSectores As DropDownList = CType(e.Row.FindControl("ddlSectores"), DropDownList)
            ddlSectores.Items.Add(New ListItem(itzultzaileWeb.Itzuli("seleccioneUno"), Integer.MinValue))
            ddlSectores.DataSource = CType(ViewState("ConceptosBatz"), List(Of ELL.Concepto))
            ddlSectores.DataBind()
            ddlSectores.SelectedValue = Integer.MinValue
        End If
    End Sub

    ''' <summary>
    ''' Se guardan la asignacion de conceptos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnGuardar_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnGuardar.Click
        Try
            Dim lConcept As New List(Of String())
            Dim drop As DropDownList
            Dim idMov As Integer
            For Each row As GridViewRow In gvConceptos.Rows
                drop = CType(row.FindControl("ddlSectores"), DropDownList)
                If (drop.SelectedValue <> Integer.MinValue) Then
                    idMov = gvConceptos.DataKeys(row.RowIndex).Value
                    lConcept.Add(New String() {idMov, drop.SelectedItem.Text, drop.SelectedValue})
                End If
            Next

            If (lConcept.Count = 0) Then
                Master.MensajeAdvertencia = itzultzaileWeb.Itzuli("Debe seleccionar un elemento")
            Else
                Dim conceptBLL As New BLL.ConceptosBLL
                conceptBLL.UpdateConceptosGenericos(lConcept)
                log.Info("Se han actualizado y mapeado " & lConcept.Count & " movimientos del concepto " & Concepto)
                Master.MensajeInfo = itzultzaileWeb.Itzuli("datosGuardados")
                mostrarDetalle()
            End If
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        Catch ex As Exception
            log.Error("Error al guardar la asignacion de conceptos genericos del concepto " & Concepto, ex)
            Master.MensajeError = itzultzaileWeb.Itzuli("errGuardar")
        End Try
    End Sub

#End Region

End Class