Public Class Detalle1
    Inherits PageBase

#Region "Propiedades"
    Public Property Seleccionado As Nullable(Of Integer)
        Get
            Return ViewState("Seleccionado")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            ViewState("Seleccionado") = value
        End Set
    End Property
    Public Property IdIturria As Nullable(Of Integer)
        Get
            Return ViewState("IdIturria")
        End Get
        Set(ByVal value As Nullable(Of Integer))
            ViewState("IdIturria") = value
        End Set
    End Property
#End Region
#Region "Eventos Pagina"
    Private Sub Detalle1_PreLoad(sender As Object, e As System.EventArgs) Handles Me.PreLoad
        If Not IsPostBack Then
            If PreviousPage IsNot Nothing Then
                Dim myPropertyID As System.Reflection.PropertyInfo = Array.Find(PreviousPage.GetType.GetProperties, Function(o As System.Reflection.PropertyInfo) o.Name = "Seleccionado")
                If myPropertyID.GetValue(PreviousPage, Nothing) IsNot Nothing Then Seleccionado = myPropertyID.GetValue(PreviousPage, Nothing)
            End If
            If Seleccionado IsNot Nothing Then
                Dim Estructura As New gtkEstructura
                Estructura.Cargar(Seleccionado)
                txtCaracteristica.Text = Estructura.Descripcion
                btnEliminar.Visible = (Estructura.IdIturria IsNot Nothing AndAlso Estructura.Nodos Is Nothing) 'No se pueden eliminar los elementos con nodos ni los que sean el origen de la estructura.
            End If
        End If
    End Sub
#End Region
#Region "Acciones"
    Private Sub btnVolver_Click(sender As Object, e As System.EventArgs) Handles btnVolver.Click
        Server.Transfer("Inicio.aspx")
    End Sub
    Private Sub btnEliminar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnEliminar.Click
        Try
            If Seleccionado IsNot Nothing Then
                Dim Estructura As New gtkEstructura
                Dim Caracteristicas As List(Of gtkCaracteristica) = New gtkCaracteristica() With {.IdEstructura = Seleccionado}.Listado
                '---------------------------------------------------------------------------------------------------------------------------
                'Comprobamos que no sea un nodo pricipal y que no tiene subnodos.
                '---------------------------------------------------------------------------------------------------------------------------
                Estructura.Cargar(Seleccionado)
                If Estructura.Nodos IsNot Nothing Then Throw New ApplicationException("errBorrarRelacionado".Itzuli & " / " & "Tiene SubNodos", New ApplicationException)
                '---------------------------------------------------------------------------------------------------------------------------
                '---------------------------------------------------------------------------------------------------------------------------
                'Comprobamos que no este relacionado con alguna incidencia.
                '---------------------------------------------------------------------------------------------------------------------------
				If Caracteristicas IsNot Nothing AndAlso Caracteristicas.Any Then Throw New ApplicationException("errBorrarRelacionado".Itzuli & " / " & "RevisarIncidencias".Itzuli, New ApplicationException)
                '---------------------------------------------------------------------------------------------------------------------------
                Estructura.Eliminar()
                btnVolver_Click(Nothing, Nothing)
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub btnNuevaIncidencia_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnNuevaIncidencia.Click
        IdIturria = Seleccionado
        btnEliminar.Visible = False
        txtCaracteristica.Text = String.Empty
    End Sub
    Private Sub btnGuardar_Click(sender As Object, e As System.Web.UI.ImageClickEventArgs) Handles btnGuardar.Click
        Dim Estructura As New gtkEstructura
        If IdIturria IsNot Nothing Then
            Estructura.Descripcion = txtCaracteristica.Text
            Estructura.IdIturria = IdIturria
            Estructura.Guardar()
        Else
            Estructura.Cargar(Seleccionado)
            Estructura.Descripcion = txtCaracteristica.Text
            Estructura.Guardar()
        End If
        Seleccionado = Estructura.Id
        btnVolver_Click(Nothing, Nothing)
    End Sub
#End Region
End Class