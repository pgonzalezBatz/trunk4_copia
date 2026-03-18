Imports System.Web.UI.WebControls
Imports System.Globalization.CultureInfo
Imports System.Reflection

Public Class Itzultzaile
    Public Shared log As log4net.ILog = log4net.LogManager.GetLogger("root.LokalizationLib")
	''' <summary>
	''' Lista de IDs de elementos que no se desea traducir.
	''' </summary>
	''' <remarks></remarks>
	Private Shared _ObjetosNoTraducibles As List(Of String)
	''' <summary>
	''' Lista de IDs de elementos que no se desea traducir.
	''' </summary>
	''' <value>List(Of String)</value>
	''' <returns>List(Of String)</returns>
	''' <remarks></remarks>
    Public Shared Property ObjetosNoTraducibles() As List(Of String)
        Get
            If (_ObjetosNoTraducibles Is Nothing) Then
                Return New List(Of String)
            Else
                Return _ObjetosNoTraducibles
            End If
        End Get
        Set(ByVal value As List(Of String))
            _ObjetosNoTraducibles = value
        End Set
    End Property
#Region "Lokalization"
    ''' <summary>
    ''' Traductor de Terminos.
    ''' Si no encuentra el termino devuelve lo mismo que se mando.
    ''' <para>Función "Friend" para que se vea dentro del mismo ensamblado</para>
    ''' </summary>
    ''' <param name="Termino"></param>
    ''' <returns></returns>
	Friend Shared Function TraducirTermino(ByRef Termino As String) As String
		'Comprobamos que no este vacio para no llamar al Servicio Web que traduce.
		If Termino.Trim <> String.Empty Then
			Dim accesoGenerico As New AccesoGenerico()
			Dim Texto As String = accesoGenerico.GetTermino(Termino, CurrentCulture.Name)
			If Texto.Trim <> String.Empty Then Termino = Texto
		End If
		Return Termino
	End Function
#End Region
    ''' <summary>
    ''' Proceso recursivo que rastrea los System.Web.UI.ControlCollection
    ''' para traducirlos.
    ''' Para la traduccion de paginas Web se recomienda colocarlo en "Page_PreRender".
    ''' </summary>
    ''' <param name="Objeto">System.Object</param>
	Shared Sub TraducirObjetos(ByRef Objeto As Object)
		For Each ElementoWeb As Object In Objeto
			Dim idObjeto As String = ""
			Dim controlType As Type = ElementoWeb.GetType
			'--------------------------------------------------------------------------------------------------
            Dim Propiedades As List(Of PropertyInfo) = controlType.GetProperties.ToList
			Dim myPropertyID As PropertyInfo = Propiedades.Find(Function(pi As PropertyInfo) StrComp(pi.Name, "ID", CompareMethod.Text) = 0)
			Dim myPropertyVisible As PropertyInfo = Propiedades.Find(Function(o As PropertyInfo) StrComp(o.Name, "Visible", CompareMethod.Text) = 0)
			'--------------------------------------------------------------------------------------------------

			'--------------------------------------------------------------------------------------------------
			'El objeto se traduce solo si es visible (Visible=TRUE)
			'--------------------------------------------------------------------------------------------------
			If myPropertyVisible IsNot Nothing AndAlso myPropertyVisible.GetValue(ElementoWeb, Nothing) = False Then Continue For
			'--------------------------------------------------------------------------------------------------

			'--------------------------------------------------------------------------------------------------
			'Comprobamos que el "ElementoWeb" que queremos traducir no se encuentre en la lista de exclusion
			'de "ObjetosNoTraducibles".
			'--------------------------------------------------------------------------------------------------
            If myPropertyID.GetValue(ElementoWeb, Nothing) IsNot Nothing Then
                idObjeto = myPropertyID.GetValue(ElementoWeb, Nothing).ToString
#If DEBUG Then
                log.Debug(Now & "." & Now.Millisecond & " - idObjeto = " & idObjeto)
#End If
            End If

			If ObjetosNoTraducibles IsNot Nothing AndAlso (ObjetosNoTraducibles.Count > 0 And idObjeto <> String.Empty) AndAlso _
			 (ObjetosNoTraducibles.Exists(Function(o As String) o = idObjeto)) Then _
			 Continue For
			'--------------------------------------------------------------------------------------------------

			If ElementoWeb.Controls.Count >= 1 Then
                ElementoWeb = Itzuli_Nagusia(ElementoWeb)
                TraducirObjetos(ElementoWeb.Controls)
			Else
                ElementoWeb = Itzuli_Nagusia(ElementoWeb)
			End If
		Next
	End Sub
    ''' <summary>
    ''' Traductor generico de Objetos.
    ''' </summary>
    ''' <param name="Objeto"></param>
    ''' <returns></returns>
	''' <remarks></remarks>
	Friend Shared Function Itzuli_Nagusia(ByVal Objeto As Object)
		'--------------------------------------------------------------------------------
		'Identificamos con que objetos se puede usar la funcion "Itzuli".
		'Si no existe una funcion especifica para el objeto en curso no se traduce.
		'--------------------------------------------------------------------------------
		Dim mType As Type = GetType(Itzultzaile)
        Dim Metodos As List(Of MethodInfo) = mType.GetMethods().ToList
		Metodos = Metodos.FindAll(Function(o As MethodInfo) StrComp(o.Name, "Itzuli", CompareMethod.Text) = 0)
		For Each info As MethodInfo In Metodos
			Dim parameterArray As List(Of ParameterInfo) = info.GetParameters().ToList
            If (parameterArray.Exists(Function(o As ParameterInfo) _
               StrComp(o.ParameterType.FullName.ToString, Objeto.GetType.FullName.ToString, CompareMethod.Text) = 0 _
               Or StrComp(o.ParameterType.FullName.ToString, Objeto.GetType.BaseType.FullName.ToString, CompareMethod.Text) = 0)) Then
                Objeto = Itzuli(Objeto)
            End If
		Next
		'--------------------------------------------------------------------------------
		Return Objeto
	End Function
#Region "Controles Web Estandar"
	''' <summary>
	''' ASP:Literal
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.Literal</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.Literal) As Literal
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:Label
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.Label</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.Label) As WebControl
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:TextBox
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.TextBox</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.TextBox) As WebControl
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:Button
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.Button</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.Button) As WebControl
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:LinkButton
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.LinkButton</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.LinkButton) As WebControl
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:HyperLink
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.HyperLink</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.HyperLink) As WebControl
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:Image
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.Image</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.Image) As WebControl
		If WebControl.AlternateText.Trim <> String.Empty Then WebControl.AlternateText = TraducirTermino(WebControl.AlternateText)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:ImageButton
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.ImageButton</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.ImageButton) As WebControl
		If WebControl.AlternateText.Trim <> String.Empty Then WebControl.AlternateText = TraducirTermino(WebControl.AlternateText)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:DropDownList
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.DropDownList</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.DropDownList) As WebControl
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		For Each ElementoWeb As ListItem In WebControl.Items
			ElementoWeb = Itzuli_Nagusia(ElementoWeb)
		Next
		Return WebControl
    End Function
    ''' <summary>
    ''' ASP:BulletedList
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.BulletedList</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.BulletedList) As WebControl
        If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
        For Each ElementoWeb As ListItem In WebControl.Items
            ElementoWeb = Itzuli_Nagusia(ElementoWeb)
        Next
        Return WebControl
    End Function
    ''' <summary>
    ''' ASP:CheckBoxList
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.CheckBoxList</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.CheckBoxList) As WebControl
        If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
        For Each ElementoWeb As ListItem In WebControl.Items
            ElementoWeb = Itzuli_Nagusia(ElementoWeb)
        Next
        Return WebControl
    End Function
	''' <summary>
	''' ASP:RadioButtonList
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.RadioButtonList</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.RadioButtonList) As WebControl
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		For Each ElementoWeb As ListItem In WebControl.Items
			ElementoWeb = Itzuli_Nagusia(ElementoWeb)
		Next
		Return WebControl
	End Function
	''' <summary>
	''' ASP:RadioButton
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.RadioButton</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.RadioButton) As WebControl
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:ListItem
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.ListItem</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.ListItem) As ListItem
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:CheckBox
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.CheckBox</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.CheckBox) As CheckBox
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
        If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
    ''' <summary>
    ''' ASP:Panel
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.Panel</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.Panel) As Panel
		If WebControl.GroupingText.Trim <> String.Empty Then WebControl.GroupingText = TraducirTermino(WebControl.GroupingText)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
#End Region
#Region "Controles Web"
	'------------------------------------------------------------------------------------------------------
	'Esta funcion hace que se recorra todas las celdas de las tablas para traducir su contenido.
	'Si la activamos para que traduzca el contenido puede que se realentice la generacion de la pagina.
	'------------------------------------------------------------------------------------------------------
	' ''' <summary>
	' ''' Accede al contenido de las celdas de una tabla.
	' ''' </summary>
	' ''' <param name="WebControl">System.Web.UI.WebControls.TableCell</param>
	' ''' <returns></returns>
	' ''' <remarks></remarks>
	'Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.TableCell) As TableCell
	'	If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
	'	If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
	'	Return WebControl
	'End Function
    '------------------------------------------------------------------------------------------------------
#End Region
#Region "GridView, DetailsView, Repeater, FormView, ListView"
#Region "GridView"
	''' <summary>
	''' ASP:GridView
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.GridView</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.GridView) As WebControl
		If WebControl.Caption.Trim <> String.Empty Then WebControl.Caption = TraducirTermino(WebControl.Caption)
		If WebControl.EmptyDataText.Trim <> String.Empty Then WebControl.EmptyDataText = TraducirTermino(WebControl.EmptyDataText)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)

		'------------------------------------------------------------------------------------------------------
        'Con este codigo accedemos a los campos del GridView (CommandField, ButtonField,...)
        '------------------------------------------------------------------------------------------------------
        For Each Columna As Object In WebControl.Columns
            Columna = Itzuli_Nagusia(Columna)
        Next
		'------------------------------------------------------------------------------------------------------

		'-----------------------------------------------------------------------
		'Acceso a las lineas creadas por un TemplateField.
		'-----------------------------------------------------------------------
		'For Each Fila As GridViewRow In WebControl.Rows
		'	For Each Celda As TableCell In Fila.Cells
		'		For Each Objeto As Object In Celda.Controls
		'			Objeto = Itzuli(Objeto)
		'		Next
		'	Next
		'Next
		'-----------------------------------------------------------------------

		Return WebControl
	End Function
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.GridViewRow) As GridViewRow
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' En escenarios de accesibilidad, representa una celda de encabezado en la tabla procesada de un control enlazado a datos de ASP.NET tabular, como por ejemplo GridView.
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.DataControlFieldHeaderCell</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.DataControlFieldHeaderCell) As DataControlFieldHeaderCell
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
#End Region
#Region "DetailsView"
    ''' <summary>
    ''' ASP:DetailsView
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.DetailsView</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.DetailsView) As WebControl
        If WebControl.Caption.Trim <> String.Empty Then WebControl.Caption = TraducirTermino(WebControl.Caption)
        If WebControl.HeaderText.Trim <> String.Empty Then WebControl.HeaderText = TraducirTermino(WebControl.HeaderText)
        If WebControl.EmptyDataText.Trim <> String.Empty Then WebControl.EmptyDataText = TraducirTermino(WebControl.EmptyDataText)
        If WebControl.FooterText.Trim <> String.Empty Then WebControl.FooterText = TraducirTermino(WebControl.FooterText)
        If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
        For Each Campo As Object In WebControl.Fields
            Campo = Itzuli_Nagusia(Campo)
        Next
        Return WebControl
    End Function
#End Region
#Region "Repeater"
    ''' <summary>
    ''' ASP:Repeater
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.Repeater</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.Repeater) As Repeater
        For Each RepeaterItem As RepeaterItem In WebControl.Items
            Itzuli_Nagusia(RepeaterItem)
        Next
        Return WebControl
    End Function
    ''' <summary>
    ''' ASP:RepeaterItem
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.RepeaterItem</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.RepeaterItem) As RepeaterItem
        For Each Objeto As Object In WebControl.Controls
            Select Case Objeto.GetType.FullName
                Case GetType(System.Web.UI.HtmlControls.HtmlTableRow).FullName
                    Dim HtmlTableRow As System.Web.UI.HtmlControls.HtmlTableRow = Objeto
                    For Each ControlCelda As System.Web.UI.HtmlControls.HtmlTableCell In HtmlTableRow.Cells
                        TraducirObjetos(ControlCelda.Controls)
                    Next
            End Select
        Next
        Return WebControl
    End Function
#End Region
#Region "FormView"
    ''' <summary>
    ''' ASP:FormView
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.FormView</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.FormView) As WebControl
        If WebControl.Caption.Trim <> String.Empty Then WebControl.Caption = TraducirTermino(WebControl.Caption)
        If WebControl.HeaderText.Trim <> String.Empty Then WebControl.HeaderText = TraducirTermino(WebControl.HeaderText)
        If WebControl.EmptyDataText.Trim <> String.Empty Then WebControl.EmptyDataText = TraducirTermino(WebControl.EmptyDataText)
        If WebControl.FooterText.Trim <> String.Empty Then WebControl.FooterText = TraducirTermino(WebControl.FooterText)
        If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
        Return WebControl
    End Function
#End Region
#Region "ListView"
    ''' <summary>
    ''' ASP:ListView
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.ListView</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.ListView) As WebControl
        Dim Contenedor As New System.Web.UI.Control
        '----------------------------------------------------------------------------------------------------------------------------------------------------
        'Obtenemos los objetos de las "Plantillas" (Templates) para traduccirlos.
        '----------------------------------------------------------------------------------------------------------------------------------------------------
        If WebControl.AlternatingItemTemplate IsNot Nothing Then WebControl.AlternatingItemTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.EditItemTemplate IsNot Nothing Then WebControl.EditItemTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.EmptyDataTemplate IsNot Nothing Then WebControl.EmptyDataTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.EmptyItemTemplate IsNot Nothing Then WebControl.EmptyItemTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.GroupSeparatorTemplate IsNot Nothing Then WebControl.GroupSeparatorTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.GroupTemplate IsNot Nothing Then WebControl.GroupTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.InsertItemTemplate IsNot Nothing Then WebControl.InsertItemTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.ItemSeparatorTemplate IsNot Nothing Then WebControl.ItemSeparatorTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.ItemTemplate IsNot Nothing Then WebControl.ItemTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.LayoutTemplate IsNot Nothing Then WebControl.LayoutTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        If WebControl.SelectedItemTemplate IsNot Nothing Then WebControl.SelectedItemTemplate.InstantiateIn(Contenedor) : Itzuli_Nagusia(Contenedor)
        '----------------------------------------------------------------------------------------------------------------------------------------------------
        Return WebControl
    End Function
#End Region
#Region "Elementos comunes en los WebControls(GridView, DetailsView, Repeater, FormView, ListView)"
    ''' <summary>
    ''' ASP:CommandField
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.CommandField</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.CommandField) As System.Web.UI.WebControls.CommandField
        If WebControl.CancelText.Trim <> String.Empty Then WebControl.CancelText = TraducirTermino(WebControl.CancelText)
        If WebControl.DeleteText.Trim <> String.Empty Then WebControl.DeleteText = TraducirTermino(WebControl.DeleteText)
        If WebControl.EditText.Trim <> String.Empty Then WebControl.EditText = TraducirTermino(WebControl.EditText)
        If WebControl.HeaderText.Trim <> String.Empty Then WebControl.HeaderText = TraducirTermino(WebControl.HeaderText)
        If WebControl.InsertText.Trim <> String.Empty Then WebControl.InsertText = TraducirTermino(WebControl.InsertText)
        If WebControl.NewText.Trim <> String.Empty Then WebControl.NewText = TraducirTermino(WebControl.NewText)
        If WebControl.SelectText.Trim <> String.Empty Then WebControl.SelectText = TraducirTermino(WebControl.SelectText)
        If WebControl.UpdateText.Trim <> String.Empty Then WebControl.UpdateText = TraducirTermino(WebControl.UpdateText)

        'WebControl.ShowEditButton = False
        'WebControl.ShowSelectButton = False
        Return WebControl
    End Function
    ''' <summary>
    ''' ASP:BoundField
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.BoundField</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.BoundField) As System.Web.UI.WebControls.BoundField
        If WebControl.AccessibleHeaderText.Trim <> String.Empty Then WebControl.AccessibleHeaderText = TraducirTermino(WebControl.AccessibleHeaderText)
        If WebControl.FooterText.Trim <> String.Empty Then WebControl.FooterText = TraducirTermino(WebControl.FooterText)
        If WebControl.HeaderText.Trim <> String.Empty Then WebControl.HeaderText = TraducirTermino(WebControl.HeaderText)
        If WebControl.NullDisplayText.Trim <> String.Empty Then WebControl.NullDisplayText = TraducirTermino(WebControl.NullDisplayText)
        Return WebControl
    End Function
    ''' <summary>
    ''' ASP:CheckBoxField
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.CheckBoxField</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.CheckBoxField) As System.Web.UI.WebControls.CheckBoxField
        If WebControl.AccessibleHeaderText.Trim <> String.Empty Then WebControl.AccessibleHeaderText = TraducirTermino(WebControl.AccessibleHeaderText)
        If WebControl.FooterText.Trim <> String.Empty Then WebControl.FooterText = TraducirTermino(WebControl.FooterText)
        If WebControl.HeaderText.Trim <> String.Empty Then WebControl.HeaderText = TraducirTermino(WebControl.HeaderText)
        If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
        Return WebControl
    End Function
    ''' <summary>
    ''' ASP:TemplateField
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.TemplateField</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.TemplateField) As System.Web.UI.WebControls.TemplateField
        If WebControl.AccessibleHeaderText.Trim <> String.Empty Then WebControl.AccessibleHeaderText = TraducirTermino(WebControl.AccessibleHeaderText)
        If WebControl.FooterText.Trim <> String.Empty Then WebControl.FooterText = TraducirTermino(WebControl.FooterText)
        If WebControl.HeaderText.Trim <> String.Empty Then WebControl.HeaderText = TraducirTermino(WebControl.HeaderText)
        Return WebControl
    End Function
    ''' <summary>
    ''' ASP:ButtonField
    ''' </summary>
    ''' <param name="WebControl">System.Web.UI.WebControls.ButtonField</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.ButtonField) As System.Web.UI.WebControls.ButtonField
        If WebControl.AccessibleHeaderText.Trim <> String.Empty Then WebControl.AccessibleHeaderText = TraducirTermino(WebControl.AccessibleHeaderText)
        If WebControl.FooterText.Trim <> String.Empty Then WebControl.FooterText = TraducirTermino(WebControl.FooterText)
        If WebControl.HeaderText.Trim <> String.Empty Then WebControl.HeaderText = TraducirTermino(WebControl.HeaderText)
        Return WebControl
    End Function
#End Region
#End Region
#Region "Controles de Exploracion"
	''' <summary>
	''' ASP:Menu
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.Menu</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.Menu) As System.Web.UI.WebControls.Menu
		If WebControl.ScrollDownText.Trim <> String.Empty Then WebControl.ScrollDownText = TraducirTermino(WebControl.ScrollDownText)
		If WebControl.ScrollUpText.Trim <> String.Empty Then WebControl.ScrollUpText = TraducirTermino(WebControl.ScrollUpText)
		If WebControl.SkipLinkText.Trim <> String.Empty Then WebControl.SkipLinkText = TraducirTermino(WebControl.SkipLinkText)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		For Each ElementoMenu As Object In WebControl.Items
			ElementoMenu = Itzuli_Nagusia(ElementoMenu)
		Next
		Return WebControl
	End Function
	''' <summary>
	''' ASP:MenuItem
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.MenuItem</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.MenuItem) As System.Web.UI.WebControls.MenuItem
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		For Each ElementoMenu As Object In WebControl.ChildItems
			ElementoMenu = Itzuli_Nagusia(ElementoMenu)
		Next
		Return WebControl
	End Function
	''' <summary>
	''' ASP:TreeView
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.TreeView</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.TreeView) As System.Web.UI.WebControls.TreeView
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		For Each ElementoMenu As Object In WebControl.Nodes
			ElementoMenu = Itzuli_Nagusia(ElementoMenu)
		Next
		Return WebControl
	End Function
	''' <summary>
	''' ASP:TreeNode
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.TreeNode</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.TreeNode) As System.Web.UI.WebControls.TreeNode
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		If WebControl.ImageToolTip.Trim <> String.Empty Then WebControl.ImageToolTip = TraducirTermino(WebControl.ImageToolTip)
		For Each ElementoMenu As Object In WebControl.ChildNodes
			ElementoMenu = Itzuli_Nagusia(ElementoMenu)
		Next
		Return WebControl
	End Function
#End Region
#Region "Controles de Validación"
	''' <summary>
	''' ASP:RegularExpressionValidator
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.RegularExpressionValidator</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.RegularExpressionValidator) As RegularExpressionValidator
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		If WebControl.ErrorMessage.Trim <> String.Empty Then WebControl.ErrorMessage = TraducirTermino(WebControl.ErrorMessage)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:RequiredFieldValidator
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.RequiredFieldValidator</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.RequiredFieldValidator) As RequiredFieldValidator
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		If WebControl.ErrorMessage.Trim <> String.Empty Then WebControl.ErrorMessage = TraducirTermino(WebControl.ErrorMessage)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:CompareValidator
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.CompareValidator</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.CompareValidator) As CompareValidator
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		If WebControl.ErrorMessage.Trim <> String.Empty Then WebControl.ErrorMessage = TraducirTermino(WebControl.ErrorMessage)
		Return WebControl
	End Function
	''' <summary>
	''' ASP:RangeValidator
	''' </summary>
	''' <param name="WebControl">System.Web.UI.WebControls.RangeValidator</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As System.Web.UI.WebControls.RangeValidator) As RangeValidator
		If WebControl.Text.Trim <> String.Empty Then WebControl.Text = TraducirTermino(WebControl.Text)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		If WebControl.ErrorMessage.Trim <> String.Empty Then WebControl.ErrorMessage = TraducirTermino(WebControl.ErrorMessage)
		Return WebControl
	End Function
#End Region
#Region "AjaxControlToolKit"
	''' <summary>
	''' TabContainer
	''' </summary>
	''' <param name="WebControl">AjaxControlToolkit.TabContainer</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As AjaxControlToolkit.TabContainer) As AjaxControlToolkit.TabContainer
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' TabPanel
	''' </summary>
	''' <param name="WebControl">AjaxControlToolkit.TabPanel</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As AjaxControlToolkit.TabPanel) As AjaxControlToolkit.TabPanel
		If WebControl.HeaderText.Trim <> String.Empty Then WebControl.HeaderText = TraducirTermino(WebControl.HeaderText)
		If WebControl.ToolTip.Trim <> String.Empty Then WebControl.ToolTip = TraducirTermino(WebControl.ToolTip)
		Return WebControl
	End Function
	''' <summary>
	''' ListSearchExtender
	''' </summary>
	''' <param name="WebControl">AjaxControlToolkit.ListSearchExtender</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As AjaxControlToolkit.ListSearchExtender) As AjaxControlToolkit.ListSearchExtender
		If WebControl.PromptText.Trim <> String.Empty Then WebControl.PromptText = TraducirTermino(WebControl.PromptText)
		Return WebControl
	End Function
	''' <summary>
	''' ConfirmButtonExtender
	''' </summary>
	''' <param name="WebControl">AjaxControlToolkit.ConfirmButtonExtender</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As AjaxControlToolkit.ConfirmButtonExtender) As AjaxControlToolkit.ConfirmButtonExtender
		If WebControl.ConfirmText.Trim <> String.Empty Then WebControl.ConfirmText = TraducirTermino(WebControl.ConfirmText)
		Return WebControl
	End Function
	''' <summary>
	''' CollapsiblePanelExtender
	''' </summary>
	''' <param name="WebControl">AjaxControlToolkit.CollapsiblePanelExtender</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As AjaxControlToolkit.CollapsiblePanelExtender) As AjaxControlToolkit.CollapsiblePanelExtender
		If WebControl.CollapsedText.Trim <> String.Empty Then WebControl.CollapsedText = TraducirTermino(WebControl.CollapsedText)
		If WebControl.ExpandedText.Trim <> String.Empty Then WebControl.ExpandedText = TraducirTermino(WebControl.ExpandedText)
		Return WebControl
	End Function
	''' <summary>
	''' TextBoxWatermarkExtender
	''' </summary>
	''' <param name="WebControl">AjaxControlToolkit.TextBoxWatermarkExtender</param>
	''' <returns></returns>
	''' <remarks></remarks>
	Public Shared Function Itzuli(ByVal WebControl As AjaxControlToolkit.TextBoxWatermarkExtender) As AjaxControlToolkit.TextBoxWatermarkExtender
		If WebControl.WatermarkText.Trim <> String.Empty Then WebControl.WatermarkText = TraducirTermino(WebControl.WatermarkText)
		Return WebControl
	End Function
#End Region
#Region "Prototipos"
#End Region
End Class