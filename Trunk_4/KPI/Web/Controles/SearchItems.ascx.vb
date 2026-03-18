Public Class SearchItems
    Inherits UserControl

    ''' <summary>
    ''' Nombre de la clase que se le asignara al textbox
    ''' </summary>
    ''' <value></value>  
    Public WriteOnly Property TextboxClass As String
        Set(ByVal value As String)
            txtItem.CssClass = value
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
            If (ViewState("sa") Is Nothing) Then
                Return True
            Else
                Return CType(ViewState("sa"), Boolean)
            End If
        End Get
        Set(ByVal value As Boolean)
            ViewState("sa") = value
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
    ''' Asigna el foco al cuadro de texto
    ''' </summary>
    ''' <value></value>    
    Public Property SetFocus As Boolean
        Get
            Return ViewState("sf")
        End Get
        Set(ByVal value As Boolean)
            ViewState("sf") = value
        End Set
    End Property

    ''' <summary>
    ''' Obtiene el texto introducido
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property Texto As String
        Get
            Return txtItem.Text.Trim
        End Get
        Set(value As String)
            txtItem.Text = value
        End Set
    End Property

    ''' <summary>
    ''' Devuelve el id del item seleccionado
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>
    Public Property SelectedId As String
        Get
            If (hfItem.Value = String.Empty OrElse Texto = String.Empty) Then
                Return String.Empty
            Else
                Return hfItem.Value
            End If
        End Get
        Set(value As String)
            hfItem.Value = value
        End Set
    End Property

    ''' <summary>
    ''' Habilita o deshabilita el textbox
    ''' </summary>
    ''' <param name="bEnable">True para habilitar</param>
    Public Sub Enable(ByVal bEnable As Boolean)
        txtItem.Enabled = bEnable
    End Sub

    ''' <summary>
    ''' Devuelve si el control está habilitado
    ''' </summary>
    ''' <returns></returns>
    Public ReadOnly Property IsEnabled
        Get
            Return txtItem.Enabled
        End Get
    End Property

    ''' <summary>
    ''' Inicializa el control con unos valores
    ''' </summary>
    ''' <param name="id">Id de sab</param>
    ''' <param name="texto">Texto a mostrar</param>
    Public Sub Inicializar(ByVal id As Integer, ByVal texto As String)
        hfItem.Value = id
        txtItem.Text = texto
    End Sub

    ''' <summary>
    ''' Limpia el texto y quita el id del hidden
    ''' </summary>
    Public Sub Limpiar()
        txtItem.Text = String.Empty
        hfItem.Value = String.Empty
    End Sub

    ''' <summary>
    ''' Evento que se lanza cuando se hace click en un elemento
    ''' </summary>    
    Public Event ItemSeleccionado(ByVal id As Integer)

    ''' <summary>
    ''' Evento que se lanza cuando se hace click en el boton de buscar
    ''' </summary>    
    Public Event BuscarItem()

    ''' <summary>
    ''' Lanza un evento si se ha marcado para que haga un postback
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnFire_Click(ByVal sender As Object, ByVal e As EventArgs) Handles btnFire.Click
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

    ''' <summary>
    ''' Se cargan los scripts
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub SearchItems_Load(sender As Object, e As EventArgs) Handles Me.Load
        aceItems.OnClientItemSelected = "ElegidoItem" & Me.ID
        Dim script As New StringBuilder
        script.AppendLine("var hdnValueId = document.getElementById('" & hfItem.ClientID & "');")
        script.AppendLine("hdnValueId.value = eventArgs.get_value();")
        If (PostBack) Then
            script.AppendLine("var str = 'ctl00$" & btnFire.ClientID.Replace("_", "$") & "';")
            'script.AppendLine("str='ctl00$' + str.replace(/_/gi, '$');")
            script.AppendLine("__doPostBack(str, '');")
        End If
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ElegidoItem" & Me.ID, "function ElegidoItem" & Me.ID & "(source, eventArgs){" & script.ToString & " }", True)
        txtItem.Attributes.Add("onkeyup", "fItemContextKey_" & Me.ID & "();")
        ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "ItemContextKey_" & Me.ID, "function fItemContextKey_" & Me.ID & "() {$find('" & aceItems.ClientID & "').set_contextKey('" & Opcion & "|" & SoloActivos & "');}", True)
        If (SetFocus) Then ScriptManager.RegisterStartupScript(Me.Page, Me.GetType, "SetFocus_" & Me.ID, "var txt=document.getElementById('" & txtItem.ClientID & "');if(txt!=null) txt.focus();", True)
    End Sub

End Class