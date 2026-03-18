Imports System.Collections.Generic
Imports SABLib

Partial Class Departamentos
    Inherits PageBase

#Region "Page_Load"

    ''' <summary>
    ''' Muestra los departamentos existentes 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Try
                DepartamentosDataBind()
            Catch batz As BatzException
                Master.MensajeError = batz.Termino
            End Try
        End If
    End Sub

    ''' <summary>
    ''' Se le dice los controles a traducir
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If (Not Page.IsPostBack) Then
            itzultzaileWeb.Itzuli(labelTitulo) : itzultzaileWeb.Itzuli(lnkCrearDepto) : itzultzaileWeb.Itzuli(btnSave)
            itzultzaileWeb.Itzuli(labelId) : itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(btnCancel)
            itzultzaileWeb.Itzuli(rfvNombre)
        End If
    End Sub

    ''' <summary>
    ''' Muestra los departamentos existentes
    ''' </summary>    
    Private Sub DepartamentosDataBind()
        Try
            mvDeptos.ActiveViewIndex = 0
            Dim plantBLL As New BLL.PlantasComponent
            Dim oPlant As ELL.Planta = plantBLL.GetPlanta(Master.Planta.Id)
            If (oPlant.De_Nomina) Then
                pnlDeptoKEM.Visible = False : pnlDeptoProgNominas.Visible = True
                lblMensaProgNominas.Text = itzultzaileWeb.Itzuli("Los departamentos se gestionan en el programa de nominas correspondiente")
            Else
                pnlDeptoKEM.Visible = True : pnlDeptoProgNominas.Visible = False
                Dim listDepto As List(Of ELL.Departamento) = CargarDepartamentos()
                If (listDepto.Count > 0) Then
                    gvDeptos.DataSource = listDepto
                    gvDeptos.DataBind()
                End If
            End If
        Catch ex As Exception
            Throw New BatzException("errMostrarListado", ex)
        End Try
    End Sub

    ''' <summary>
    ''' Se enlaza el repeater
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    ''' 
    Private Sub gvDeptos_RowDataBound(sender As Object, e As GridViewRowEventArgs) Handles gvDeptos.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header Or e.Row.RowType = DataControlRowType.EmptyDataRow) Then
            itzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim img As ImageButton = CType(e.Row.FindControl("imgDel"), ImageButton)
            img.OnClientClick = "return confirm('" & itzultzaileWeb.Itzuli("confirmarEliminar") & "');"
            itzultzaileWeb.Itzuli(img)
        End If
    End Sub

    ''' <summary>
    ''' Recupera todos departamentos
    ''' </summary>
    ''' <returns>Lista de departamentos recuperados</returns>
    Private Function CargarDepartamentos() As List(Of ELL.Departamento)
        Dim deptComp As New BLL.DepartamentosComponent
        Return deptComp.GetDepartamentosPlanta(Master.Planta.Id)
    End Function

#End Region

#Region "Seleccion Departamento"

    ''' <summary>
    ''' Se selecciona un departamento para ver su detalle
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub SelectDepartamento(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Dim ibtn As LinkButton = sender
            EditarDepartamento(ibtn.CommandArgument)
        Catch batz As BatzException
            Master.MensajeError = batz.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Se marca eld departamento para editar
    ''' </summary>
    ''' <param name="idDpto">Identificador del departamento</param>    
    Protected Sub EditarDepartamento(ByVal idDpto As Integer)
        btnSave.CommandArgument = idDpto
        pnlExistente.Visible = True
        lblCabecera.Text = itzultzaileWeb.Itzuli("edicionDepartamento")
        MostrarDatosDepartamento(idDpto)
    End Sub

    ''' <summary>
    ''' Se muestran los datos del departamento en el detalle
    ''' </summary>
    ''' <param name="idDpto">Identificador del departamento</param>    
    Private Sub MostrarDatosDepartamento(ByVal idDpto As Integer)
        Dim oDepto As New ELL.Departamento With {.Id = idDpto, .IdPlanta = Master.Planta.Id}
        Dim depComp As New BLL.DepartamentosComponent
        oDepto = depComp.GetDepartamento(oDepto)
        lblIdDepto.Text = idDpto
        txtNombre.Text = oDepto.Nombre
        mvDeptos.ActiveViewIndex = 1
    End Sub

#End Region

#Region "Creacion/Modificacion Departamento"

    ''' <summary>
    ''' Se prepara el formulario para crear un nuevo departamento
    ''' </summary>
    Protected Sub crearDepartamento()
        btnSave.CommandArgument = String.Empty
        lblCabecera.Text = itzultzaileWeb.Itzuli("nuevoDepartamento")
        txtNombre.Text = String.Empty
        pnlExistente.Visible = False
        mvDeptos.ActiveViewIndex = 1
    End Sub

    ''' <summary>
    ''' Llama a crearDepartamento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub lnkCrearDepartamento_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lnkCrearDepto.Click
        crearDepartamento()
    End Sub

    ''' <summary>
    ''' Se guarda la creacion o modificacion del departamento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub btnSave_Click(ByVal sender As Object, ByVal e As System.EventArgs)
        If (Page.IsValid) Then
            Dim deptComp As New Sablib.BLL.DepartamentosComponent()
            Dim bNuevo As Boolean = False
            Dim oDepto As New Sablib.ELL.Departamento With {.Nombre = txtNombre.Text.Trim, .IdPlanta = Master.Planta.Id}
            If (btnSave.CommandArgument = String.Empty) Then
                bNuevo = True
                oDepto.Id = deptComp.GenerarIdDepto(Master.Planta.Id)
                oDepto.CodigoEncargado = Master.Ticket.IdUser
                lblCabecera.Text = itzultzaileWeb.Itzuli("nuevoDepartamento")
            Else
                oDepto.Id = lblIdDepto.Text
                lblCabecera.Text = itzultzaileWeb.Itzuli("edicionDepartamento")
            End If
            Try
                deptComp.Save(oDepto, bNuevo)
                If (btnSave.CommandArgument = String.Empty) Then
                    WriteLog("Se ha creado un nuevo departamento [" & oDepto.Nombre & "] en la planta " & Master.Planta.Descripcion, TipoLog.Info)
                End If
                Master.MensajeInfoText = "infoCompGuardar"
                Volver()
            Catch ex As Exception
                Dim batzEx As New BatzException("errGuardar", ex)
                Master.MensajeError = batzEx.Termino
            End Try
        End If
    End Sub

#End Region

#Region "Cancelar"

    ''' <summary>
    ''' Se vuelve al listado de departamentos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub btnCancel_Click(ByVal sender As Object, ByVal e As EventArgs)
        Try
            Volver()
        Catch batz As BatzException
            Master.MensajeError = batz.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Vuelve al listado
    ''' </summary>    
    Private Sub Volver()
        DepartamentosDataBind()
    End Sub
#End Region

#Region "Eliminar Departamento"

    ''' <summary>
    ''' Se elimina un departamento
    ''' Antes de ello, se comprobara que no exista ningun usuario con dicho departamento asignado
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub DeleteDepartamento(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs)
        Dim ibtn As ImageButton = sender
        Dim deptComp As New SABLib.BLL.DepartamentosComponent()
        Try
            Dim userBLL As New BLL.UsuariosComponent
            Dim lUsuarios As List(Of Sablib.ELL.Usuario) = userBLL.GetUsuarios(New Sablib.ELL.Usuario With {.IdDepartamento = ibtn.CommandArgument})
            If (lUsuarios IsNot Nothing AndAlso lUsuarios.Count > 0) Then
                Master.MensajeInfoText = "No se puede borrar el departamento porque tiene usuarios asignados. Cambie primero el departamento de dichos usuarios y despues vuelva a intentarlo"
            Else
                deptComp.Delete(ibtn.CommandArgument)
                WriteLog("Se ha borrado el departamento [" & ibtn.CommandName & "] de la planta " & Master.Planta.Descripcion, TipoLog.Info)
                DepartamentosDataBind()
                Master.MensajeInfoText = "infoCompBorrar"
            End If
        Catch ex As Exception
            Dim batzEx As New BatzException("errBorrar", ex)
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub
#End Region

End Class
