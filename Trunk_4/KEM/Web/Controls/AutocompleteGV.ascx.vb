Public Class AutocompleteGV
    Inherits System.Web.UI.UserControl


#Region "Propiedades"

    ''' <summary>
    ''' Ruta donde se encuentra la pagina que realiza la busqueda
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property RutaPaginaBusqueda As String
        Get
            Return ViewState("rpb")
        End Get
        Set(ByVal value As String)
            ViewState("rpb") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si realizara postback
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property PostBack As Boolean
        Get
            If (ViewState("pb") Is Nothing) Then
                Return False
            Else
                Return CType(ViewState("pb"), Boolean)
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("pb") = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre de la clase que se le asignara al gridview
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property GridviewClass As String
        Get
            Return ViewState("gvc")
        End Get
        Set(ByVal value As String)
            ViewState("gvc") = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre del id del resultado del grid que se almacenara en el hidden al seleccionar un registro
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property IdName As String
        Get
            Return ViewState("id")
        End Get
        Set(ByVal value As String)
            ViewState("id") = value
        End Set
    End Property

    ''' <summary>
    ''' Nombre del value del resultado del grid que se almacenara en el textbox al seleccionar un registro
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property ValueName As String
        Get
            Return ViewState("v")
        End Get
        Set(ByVal value As String)
            ViewState("v") = value
        End Set
    End Property

    ''' <summary>
    ''' Numero minimo de caracteres que harán falta para que se realice la busqueda. Por debajo de este número, no se atacará a la bbdd
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property MinSearchLength As String
        Get
            Return ViewState("msl")
        End Get
        Set(ByVal value As String)
            ViewState("msl") = value
        End Set
    End Property

    ''' <summary>
    ''' Altura máxima de la capa que mostrará los resultados. En caso necesario, se mostrará el scroll
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property MaxDivHeight As String
        Get
            Return ViewState("mdh")
        End Get
        Set(ByVal value As String)
            ViewState("mdh") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica la opcion que determinara la consulta que hara la pagina
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property Opcion As String
        Get
            Return ViewState("o")
        End Get
        Set(ByVal value As String)
            ViewState("o") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si se mostrara el boton de buscar
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property ShowButton As String
        Get
            Return ViewState("sb")
        End Get
        Set(ByVal value As String)
            ViewState("sb") = value
        End Set
    End Property

    ''' <summary>
    ''' Requiere este valor ya que segun el contenedor donde se encuentre, la imagen de si esta relleno el nombre o no a veces no se alinea con el resto
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property MarginTop As Integer
        Get
            If (ViewState("mt") Is Nothing) Then
                Return 0
            Else
                Return CInt(ViewState("mt"))
            End If
        End Get
        Set(ByVal value As Integer)
            ViewState("mt") = value
        End Set
    End Property

    ''' <summary>
    ''' Indica si se mostrara el solo los activos o todos
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property SoloActivos As Boolean
        Get
            Return ViewState("sa")
        End Get
        Set(ByVal value As Boolean)
            ViewState("sa") = value
        End Set
    End Property

    ''' <summary>
    ''' Id de la planta de la que se obtendran los usuarios
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Public Property IdPlanta As Integer
        Get
            Return ViewState("idP")
        End Get
        Set(value As Integer)
            ViewState("idP") = value
        End Set
    End Property

    ''' <summary>
    ''' Asigna el foco al cuadro de texto
    ''' </summary>
    ''' <value></value>    
    Public WriteOnly Property SetFocus As Boolean
        Set(ByVal value As Boolean)
            If (value) Then txtInput.Focus()
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el texto introducido
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property Texto As String
        Get
            Return txtInput.Text.Trim
        End Get
        Set(value As String)
            txtInput.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Devuelve el id del item seleccionado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property SelectedId As String
        Get
            If (hfValue.Value = String.Empty) Then
                Return String.Empty
            Else
                Return hfValue.Value
            End If
        End Get
        Set(value As String)
            hfValue.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Limpia el texto y quita el id del hidden
    ''' </summary>
    Public Sub Limpiar()
        txtInput.Text = String.Empty
        hfValue.Value = String.Empty
    End Sub

    ''' <summary>
    ''' Inicializa el control
    ''' </summary>
    ''' <param name="idUser">Id del usuario</param>
    ''' <param name="nombre">Nombre del mismo</param>    
    Public Sub Inicializar(ByVal idUser As Integer, ByVal nombre As String)
        txtInput.Text = nombre
        hfValue.Value = idUser
    End Sub

    ''' <summary>
    ''' Evento que se lanza cuando se hace click en un elemento
    ''' </summary>    
    Public Event ItemSeleccionado(ByVal id As Integer)

    ''' <summary>
    ''' Evento que se lanza cuando se hace click en el boton de buscar
    ''' </summary>    
    Public Event BuscarItem()

#End Region

#Region "Registro del script"

    ''' <summary>
    ''' Registra el script de busqueda
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        Dim key As String = "init_" & txtInput.ClientID
        If (hfValue.Value <> String.Empty) Then
            divImgGV.Attributes("class") = "imagen-seleccionado"
        Else
            divImgGV.Attributes("class") = "imagen-no-seleccionado"
        End If
        divImgGV.Style.Add("margin-top", MarginTop & "px") 'Para que el icono se alinee con el cuadro de texto
        imgBuscar.Visible = ShowButton
        Dim postBackControlId As String = String.Empty
        If (PostBack) Then postBackControlId = btnFire.ClientID
        Dim script As String = "init('" & txtInput.ClientID & "', '" & hfValue.ClientID & "', '" & helper.ClientID & "','" & RutaPaginaBusqueda & "','" & PostBack.ToString & "','" & postBackControlId & "','" & divImgGV.ClientID & "','" & GridviewClass & "','" & IdName & "','" & ValueName & "','" & MinSearchLength & "','" & MaxDivHeight & "','" & Opcion & "','" & SoloActivos & "'," & IdPlanta & ");"
        ScriptManager.RegisterStartupScript(Page, Page.GetType(), key, script, True)
    End Sub

    ''' <summary>
    ''' Pulsa el boton de buscar
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgBuscar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgBuscar.Click
        RaiseEvent BuscarItem()
    End Sub

    ''' <summary>
    ''' Lanza un evento si se ha marcado para que haga un postback
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnFire_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnFire.Click
        RaiseEvent ItemSeleccionado(SelectedId)
    End Sub

#End Region

End Class