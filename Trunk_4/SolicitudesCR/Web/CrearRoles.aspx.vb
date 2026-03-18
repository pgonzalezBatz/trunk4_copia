
Public Class CrearRoles
    Inherits PageBase

    Private itzultzaileWeb As New LocalizationLib.Itzultzaile
    Dim oDocumentosBLL As New CEticoLib.BLL.cEtico



#Region "METODOS"


    ''' <summary>
    ''' Inicializa las variables y vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub Initialize()
        ClearGridView()
        BindDataView()
    End Sub

    ''' <summary>
    ''' Limpia los datos vinculados
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub ClearGridView()
        gvType.DataSource = Nothing
        gvType.DataBind()
    End Sub

    ''' <summary>
    ''' Vincula los datos
    ''' </summary>
    ''' <remarks></remarks>
    Protected Sub BindDataView()
        Try
            Dim oDocBLL As New CEticoLib.BLL.cEtico
            Dim listaType As List(Of CEticoLib.ELL.Rol)

            listaType = oDocBLL.CargarListaRol(PageBase.plantaAdmin)

            If (listaType.Count > 0) Then
                gvType.DataSource = listaType
                gvType.DataBind()
                gvType.Caption = String.Empty
            Else
                gvType.DataSource = Nothing
                gvType.DataBind()
                gvType.Caption = "No hay registros"
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar Periodicidad", ex)
        End Try
    End Sub



#End Region



    Protected Sub gvType_RowEditing(sender As Object, e As GridViewEditEventArgs)

    End Sub

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try





            If Not (Page.IsPostBack) Then
                Dim s As String
                s = Request.QueryString("id")
                If s = "0" Then
                    mView.ActiveViewIndex = 0
                End If
                If s = "1" Then
                    BindDataView()
                    CargarDetalle(0)
                End If


                Initialize()


                ''''''''''Dim plantBLL As New Sablib.BLL.PlantasComponent
                ''''''''''Dim dominio As String = User.Identity.Name.ToLower.Split("\")(0)
                ''''''''''Dim lPlantasIzaro As List(Of Sablib.ELL.Planta) = plantBLL.GetPlantas(True, Nothing, Nothing)
                ''''''''''Dim oPlant As Sablib.ELL.Planta = lPlantasIzaro.Find(Function(o As Sablib.ELL.Planta) o.Dominio.ToLower = dominio)

                ''''''''''ddlUni.Items.Clear()
                ''''''''''ddlUni.Items.Add(New ListItem(ItzultzaileWeb.Itzuli("seleccioneUno"), 0))
                ''''''''''For Each plant As Sablib.ELL.Planta In lPlantasIzaro
                ''''''''''    'MIRAR AQUI, SI PONGO ESTO RESTRINJO     Se añaden las plantas de Igorre y las que tengan el IdIzaro informado
                ''''''''''    '            If (plant.Id = 1 OrElse plant.IdIzaro > 0) Then
                ''''''''''    ddlUni.Items.Add(New ListItem(plant.Nombre, plant.Id))
                ''''''''''    ''''ddlUniChange.Items.Add(New ListItem(plant.Nombre, plant.Id))
                ''''''''''    '           End If
                ''''''''''Next


                'pongo plantas de la tabla 
                'ddlCaducidad
                ddlUni.Items.Clear()

                Dim listaCad As List(Of SabLib.ELL.Planta)
                listaCad = oDocumentosBLL.loadListCad()

                For Each caducidad In listaCad
                    Dim licaducidad As New ListItem(caducidad.Nombre, caducidad.Id)
                    ddlUni.Items.Add(licaducidad)

                Next





            End If




        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un Responsable", ex)
        End Try
    End Sub


    ''' <summary>
    ''' <para>Evento que captura el click del raton en cualquier area de la fila.</para>
    ''' <para>Redirecciona al detalle del item seleccionado</para>
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Protected Sub gvType_RowCommand(ByVal sender As Object, ByVal e As GridViewCommandEventArgs) Handles gvType.RowCommand
        Try
            If e.CommandName = "Desactivar" Then
                Initialize()
                Dim idres As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
                oDocumentosBLL.DeleteRol(plantaAdmin, idres)' no miro planta
                Initialize()
            End If
            If e.CommandName = "Edit" Then 'de momento nada
                ''''''''Initialize()

                ''''''''Dim idres As Int32 = DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).DataItemContainer.DataKeysContainer, System.Web.UI.WebControls.BaseDataBoundControl).DataSource(CInt(DirectCast(DirectCast(e.CommandSource, System.Web.UI.Control).NamingContainer, System.Web.UI.WebControls.GridViewRow).DataItemIndex)).id
                ''''''''BindDataView()   'para limpiar el grid
                ''''''''CargarDetalle(idres)
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un Responsable", ex)
        End Try
    End Sub
    Protected Sub gvType_OnRowCommand(sender As Object, e As GridViewCommandEventArgs)
        If e.CommandName = "Add" Then
            '    Dim idPlanta As String = DirectCast(gvType.FooterRow.FindControl("ddlPlanta"), DropDownList).SelectedValue

            'Dim txtDescript As String = DirectCast(gvType.FooterRow.FindControl("txtDescript"), TextBox).Text
            'Dim txtDescAbrev As String = DirectCast(gvType.FooterRow.FindControl("ddlUni"), DropDownList).SelectedValue



            'If Not (oDocumentosBLL.Existe(txtDescript)) Then
            '    GuardarRegistro(txtDescript, txtDescAbrev)
            'Else
            '    'El nombre ya existe en base de datos
            '    Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe otro elemento con el mismo nombre")
            'End If


        End If
    End Sub
    'Protected Sub gvType_OnRowCommand2(sender As Object, e As GridViewCommandEventArgs)
    '    If e.CommandName = "Add" Then


    '        Dim txtDescript As String = DirectCast(gvType.FooterRow.FindControl("txtDescript"), TextBox).Text
    '        Dim txtDescAbrev As String = DirectCast(gvType.FooterRow.FindControl("txtDescAbrev"), TextBox).Text


    '    End If
    'End Sub

    ''' <summary>
    ''' Guardar un nuevo registro de Tipo
    ''' </summary>
    ''' <param name="nombre"></param>
    ''' <remarks></remarks>
    Private Sub GuardarRegistro(ByVal nombre As String, ByVal txtDescAbrev As String, ByVal txtUsuario As String)
        Try

            If (txtResponsable.Text) = "" Or ddlUni.SelectedValue = "0" Then
                Master.MensajeError = ItzultzaileWeb.Itzuli("Campos Usuario y planta son obligatorios")
                mView.ActiveViewIndex = 0
                Exit Sub
            End If

            '.Id = nombre rol de planta es 2
            Dim tipo As New CEticoLib.ELL.Rol With {.Planta = txtDescAbrev, .Id = CInt(nombre), .rol = 2, .NombreUser = txtUsuario, .NombreRol = ddlUni.SelectedItem.[Text]}


            If (oDocumentosBLL.GuardarRol(tipo)) Then

                Master.MensajeInfo = ItzultzaileWeb.Itzuli("El Rol se ha guardado correctamente").ToUpper

            Else
                Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el Rol").ToUpper
            End If


            mView.ActiveViewIndex = 0

            Initialize()


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al grabar el Rol", ex)
        End Try


    End Sub

    ''' <summary>
    ''' Cargar los tipos y Habilitar/Deshabilitar Transmission Mode dependiendo del producto seleccionado
    ''' </summary>
    ''' <param name="idDocumento"></param>
    ''' <remarks></remarks>
    Private Sub ConfiguracionProduct(ByVal idDocumento As Integer, Optional ByVal idType As Integer = 0, Optional ByVal idTransmissionMode As Integer = 0)
        Try
            Dim oDocBLL As New CEticoLib.BLL.cEtico
            Dim lista As List(Of CEticoLib.ELL.Rol)
            Dim userBLL As New SabLib.BLL.UsuariosComponent


            'si es nuevo elemento
            If idDocumento = 0 Then
                'lblNuevaSolicitud.Text = "Creación de un nuevo Rol"
                flag_Modificar.Value = "0"


            Else
                flag_Modificar.Value = idDocumento
                lista = oDocBLL.CargarRol(idDocumento, PageBase.plantaAdmin)

                '   lblNuevaSolicitud.Text = "Modificación del Rol " ' & lista(0).Nombre
                txtNombre.Text = lista(0).NombreUser

                ddlUnidades.SelectedValue = lista(0).Id

            End If


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al mostrar un Rol", ex)
        End Try

    End Sub




    ''' <summary>
    ''' Cargar el detalle de un portador de coste
    ''' </summary>
    Private Sub CargarDetalle(ByVal idDocumento As Integer)
        mView.ActiveViewIndex = 1

        ConfiguracionProduct(idDocumento)

    End Sub

    'Private Sub btnBorrar2_Click(sender As Object, e As EventArgs) Handles btnBorrar.Click
    '    mView.ActiveViewIndex = 1

    'End Sub

    ''' <summary>
    ''' Guardar un documento
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Protected Sub btnGuardarNuevaReferencia_Click(sender As Object, e As EventArgs) Handles btnGuardarNuevaSolicitud.Click
        Try

            Dim iCodDocum As Int32 = CInt(flag_Modificar.Value)


            Dim tipo As New CEticoLib.ELL.Rol With {.Planta = PageBase.plantaAdmin, .Id = iCodDocum, .rol = ddlUnidades.SelectedValue}

            If flag_Modificar.Value = "0" Then
                If (oDocumentosBLL.GuardarRol(tipo)) Then

                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El Rol se ha guardado correctamente").ToUpper

                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se guardaba el Rol").ToUpper
                End If
            Else



                If (oDocumentosBLL.ModificarRol(tipo, iCodDocum)) Then


                    Master.MensajeInfo = ItzultzaileWeb.Itzuli("El Rol se ha modificado correctamente").ToUpper
                    BindDataView()
                Else
                    Master.MensajeError = ItzultzaileWeb.Itzuli("Un error ha ocurrido cuando se modificaba el Rol").ToUpper
                End If
            End If

            mView.ActiveViewIndex = 0

            Initialize()


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al grabar el Rol", ex)
        End Try

    End Sub




    Protected Sub btnCancelar_Click(sender As Object, e As EventArgs) Handles btnCancelar.Click

        mView.ActiveViewIndex = 0
    End Sub

    Protected Sub btnNuevo_Click(sender As Object, e As EventArgs) Handles btnNuevo.Click
        Dim txtUsuario As String = txtResponsable.Text
        Dim txtDescript As String = hfResponsable.Value
        Dim txtDescAbrev As String = ddlUni.SelectedValue


        '     If Not (oDocumentosBLL.Existe(txtDescript)) Then
        GuardarRegistro(txtDescript, txtDescAbrev, txtUsuario)
        '    Else
        'El nombre ya existe en base de datos
        '   Master.MensajeError = ItzultzaileWeb.Itzuli("Ya existe otro elemento con el mismo nombre")
        '  End If

    End Sub


End Class