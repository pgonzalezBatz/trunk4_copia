Public Class BusquedaUsuarios
    Inherits UserControl

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
    ''' Asigna el foco al cuadro de texto
    ''' </summary>
    ''' <value></value>    
    Public WriteOnly Property SetFocusText As Boolean
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
    ''' Asigna el texto al placeholder del textbox
    ''' </summary>
    Public WriteOnly Property PlaceHolder() As String
        Set(ByVal value As String)
            txtInput.Attributes.Add("placeholder", value)
        End Set
    End Property

    ''' <summary>
    ''' Devuelve el clientId del hidden. Es necesario para obtener el valor del request.form
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property HiddenValue_ClientId As String
        Get
            Return hfValue.ClientID
        End Get
    End Property

    ''' <summary>
    ''' Devuelve el clientId del textbox. Es necesario para obtener el valor del request.form
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property TextBox_ClientId As String
        Get
            Return txtInput.ClientID
        End Get
    End Property

    ''' <summary>
    ''' Limpia el texto y quita el id del hidden
    ''' </summary>
    Public Sub Limpiar()
        txtInput.Text = String.Empty
        hfValue.Value = String.Empty
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
    ''' Lanza un evento si se ha marcado para que haga un postback
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub btnFire_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFire.Click
        RaiseEvent ItemSeleccionado(SelectedId)
    End Sub

    ''' <summary>
    ''' Traduce un texto
    ''' </summary>
    ''' <param name="texto">Texto a traducir</param>
    ''' <returns></returns>
    Protected Function Traducir(ByVal texto As String) As String
        Dim itzultzaileWeb As New itzultzaile
        Return itzultzaileWeb.Itzuli(texto)
    End Function

#End Region

End Class