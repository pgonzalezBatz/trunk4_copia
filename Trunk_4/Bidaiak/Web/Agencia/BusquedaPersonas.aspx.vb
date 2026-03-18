Public Class BusquedaPersonas
    Inherits PageBase

#Region "Page Load"

    ''' <summary>
    ''' Inicializa la pagina del buscador
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            Master.SetTitle = "Busqueda de personas"
            inicializar()
        End If
    End Sub

    ''' <summary>
    ''' Traducciones
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BusquedaPersonas_PreRender(sender As Object, e As EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            itzultzaileWeb.Itzuli(labelCodTrab) : itzultzaileWeb.Itzuli(labelNombre) : itzultzaileWeb.Itzuli(labelDpto)
            itzultzaileWeb.Itzuli(labelEmail)
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles
    ''' </summary>    
    Private Sub inicializar()
        searchUser.Limpiar()
        searchUser.PlaceHolder = itzultzaileWeb.Itzuli("Introduzca el nombre de la persona que quiere buscar")
        pnlInfo.Visible = False
    End Sub

#End Region

#Region "Buscar"

    ''' <summary>
    ''' Se selecciona un usuario
    ''' </summary>
    ''' <param name="id"></param>
    Private Sub searchUser_ItemSeleccionado(id As Integer) Handles searchUser.ItemSeleccionado
        Try
            mostrarDetalle(id)
        Catch batzEx As BatzException
            Master.MensajeError = batzEx.Termino
        End Try
    End Sub

    ''' <summary>
    ''' Muestra el resultado de las personas encontradas
    ''' </summary>    
    ''' <param name="idUser">Id del usuario</param>
    Private Sub mostrarDetalle(ByVal idUser As Integer)
        Try
            pnlInfo.Visible = True
            Dim userBLL As New SabLib.BLL.UsuariosComponent
            Dim dptoBLL As New SabLib.BLL.DepartamentosComponent
            Dim oUser As SabLib.ELL.Usuario = userBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idUser}, True)
            lblCodPerso.Text = oUser.CodPersona
            lblNombre.Text = oUser.NombreCompleto
            lblEmail.Text = oUser.Email
            Dim oDpto As SabLib.ELL.Departamento = dptoBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = oUser.IdDepartamento, .IdPlanta = oUser.IdPlanta})
            lblDepartamento.Text = If(oDpto IsNot Nothing, oDpto.Nombre, String.Empty)
        Catch batzEx As BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BatzException("Error al mostrar los datos de personas", ex)
        End Try
    End Sub

#End Region

End Class