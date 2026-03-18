Public Class Relacionar
    Inherits PageBase

#Region "Propiedades"
    Public BBDD As New BatzBBDD.Entities_Gertakariak

    Dim ESTRUCTURA_O As BatzBBDD.ESTRUCTURA
    Dim ESTRUCTURA_D As BatzBBDD.ESTRUCTURA

    Property Caracteristica_Origen As Integer?
        Get
            Return CType(Session("Caracteristica_Origen"), Nullable(Of Integer))
        End Get
        Set(value As Integer?)
            Session("Caracteristica_Origen") = value
        End Set
    End Property
    Property Caracteristica_Destino As Integer?
        Get
            Return CType(Session("Caracteristica_Destino"), Nullable(Of Integer))
        End Get
        Set(value As Integer?)
            Session("Caracteristica_Destino") = value
        End Set
    End Property

#End Region

#Region "Eventos de Pagina"
    Private Sub Relacionar_Init(sender As Object, e As EventArgs) Handles Me.Init
        ESTRUCTURA_O = BBDD.ESTRUCTURA.Where(Function(o) o.ID = Caracteristica_Origen).SingleOrDefault
        ESTRUCTURA_D = BBDD.ESTRUCTURA.Where(Function(o) o.ID = Caracteristica_Destino).SingleOrDefault

        lblEstructura_Origen.Text = If(ESTRUCTURA_O Is Nothing, "Capacidad", ESTRUCTURA_O.DESCRIPCION)
        lblEstructura_Destino.Text = If(ESTRUCTURA_D Is Nothing, "?", ESTRUCTURA_D.DESCRIPCION)
    End Sub
    'Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
    'End Sub
    Private Sub Relacionar_LoadComplete(sender As Object, e As EventArgs) Handles Me.LoadComplete
        Try
            CargarDatos()
        Catch ex As ApplicationException
            Log.Debug(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            'Log.Error(String.Format("hf_Planta.Value: {0} / hf_IdRecepcion.Value: {1}", hf_Planta.Value, hf_IdRecepcion.Value), ex)
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub btnAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles btnAceptar.Click
        Try
            Dim Id_Origen As String = tvOrigen.CheckedNodes.Cast(Of TreeNode).ToArray.Select(Function(o) o.Value).SingleOrDefault 'Identificador de Origen seleccionado por el usuario.
            Dim lDestino_Usr = tvDestino.CheckedNodes.Cast(Of TreeNode).ToArray.Select(Function(o) CInt(o.Value)) 'Identificadores de estructuras selecciadas por el usuario.
            Dim lDestino_BD As IQueryable(Of BatzBBDD.ESTRUCTURA_TROQUELERIA) 'Estructuras de destino en la base de datos
            '--------------------------------------------------------------------------------------------------------------
            'Comparamos las estructuras seleccionadas por el usuario con las que estan en la base de datos.
            '--------------------------------------------------------------------------------------------------------------
            If IsNumeric(Id_Origen.Replace(".", "_")) Then
                lDestino_BD = From Reg As BatzBBDD.ESTRUCTURA_TROQUELERIA In BBDD.ESTRUCTURA_TROQUELERIA Where Reg.IDEST_ORIGEN = CDec(Id_Origen)
            Else
                lDestino_BD = From Reg As BatzBBDD.ESTRUCTURA_TROQUELERIA In BBDD.ESTRUCTURA_TROQUELERIA Where Reg.IDCAP_ORIGEN = Id_Origen
            End If

            'Quitamos de la base de datos las que no estan seleccionadas.
            Dim l_No_BD = lDestino_BD.Where(Function(o) Not lDestino_Usr.Contains(o.IDESTRUCTURA))
            If l_No_BD.Any Then
                l_No_BD.ToList.ForEach(Sub(o) BBDD.ESTRUCTURA_TROQUELERIA.DeleteObject(o))
                BBDD.SaveChanges()
            End If
            '--------------------------------------------------------------------------------------------------------------

            '--------------------------------------------------------------------------------------------------------------
            'Agregamos a la base de datos los elementos seleccionados que no esten en la base de datos.
            If lDestino_Usr.Any Then
                Dim l_Si_BD = lDestino_Usr.Where(Function(o) Not lDestino_BD.Select(Function(d) d.IDESTRUCTURA).Contains(o))
                If l_Si_BD.Any Then
                    If IsNumeric(Id_Origen.Replace(".", "_")) Then
                        l_Si_BD.ToList.ForEach(Sub(o) BBDD.ESTRUCTURA_TROQUELERIA.AddObject(New BatzBBDD.ESTRUCTURA_TROQUELERIA With {.IDESTRUCTURA = o, .IDEST_ORIGEN = CDec(Id_Origen)}))
                    Else
                        l_Si_BD.ToList.ForEach(Sub(o) BBDD.ESTRUCTURA_TROQUELERIA.AddObject(New BatzBBDD.ESTRUCTURA_TROQUELERIA With {.IDESTRUCTURA = o, .IDCAP_ORIGEN = Id_Origen}))
                    End If
                    BBDD.SaveChanges()
                End If
            End If
            '--------------------------------------------------------------------------------------------------------------

            Master.ascx_Mensajes.MensajeError(New ApplicationException(ItzultzaileWeb.Itzuli("Datos Actualizados")))
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region

#Region "Funciones y Procesos"
    Sub CargarDatos()
        Try
            tvOrigen.Nodes.Clear() : tvDestino.Nodes.Clear()
            If ESTRUCTURA_O Is Nothing Then
                '--------------------------------------------------------------------------------------------------------------------
                'Capacidades
                '--------------------------------------------------------------------------------------------------------------------
                Dim lCapacidades = From Reg In BBDD.CAPACIDADES.Where(Function(d) d.OBSOLETO = 0) Select Reg Distinct
                lCapacidades.OrderBy(Function(o) o.NOMBRE).ToList.ForEach(Sub(o) tvOrigen.Nodes.Add(New TreeNode With {.Value = o.CAPID, .Text = o.NOMBRE, .SelectAction = TreeNodeSelectAction.Select, .ShowCheckBox = True}))
                '--------------------------------------------------------------------------------------------------------------------
            Else
                CargarTreeView(tvOrigen, ESTRUCTURA_O, Nothing)
            End If
            CargarTreeView(tvDestino, ESTRUCTURA_D, Nothing)

            tvOrigen.ExpandAll() : tvDestino.ExpandAll()
            tvOrigen.DataBind() : tvDestino.DataBind()
            'lvOrigen.DataBind() ': lvDestino.DataBind()
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Log.Error(ex)
            Throw
        End Try
    End Sub

    Sub CargarTreeView(ByRef TreeView As TreeView, ByRef Estructura As BatzBBDD.ESTRUCTURA, Optional ByRef TreeNodo As TreeNode = Nothing)
        If Estructura IsNot Nothing Then

            '-------------------------------------------------------------------------------
            'Creamos el nodo. 
            '-------------------------------------------------------------------------------
            Dim bEvento_OnClick As Boolean = (From Key As String In TreeView.Attributes.Keys Where String.Compare(Key, "onClick", True) = 0 Select Key).Any
            Dim Nodo As New TreeNode With
                        {.Value = Estructura.ID, .Text = Estructura.DESCRIPCION _
                        , .SelectAction = If(bEvento_OnClick And TreeNodo IsNot Nothing, TreeNodeSelectAction.Select, TreeNodeSelectAction.Expand) _
                        , .ShowCheckBox = (TreeNodo IsNot Nothing)                         ', .Checked = bNodoIncidencia
                        }
            '-------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------
            'Indicamos si el nodo es "Primario" o "Secundario".
            '-------------------------------------------------------------------------------
            If TreeNodo Is Nothing Then TreeView.Nodes.Add(Nodo) Else TreeNodo.ChildNodes.Add(Nodo)
            '-------------------------------------------------------------------------------

            '-------------------------------------------------------------------------------
            'Generamos el siguiente Nodo.
            '---------------------------------------------------------------------------------------------
            If Estructura.ESTRUCTURA1.Any Then
                For Each Reg As BatzBBDD.ESTRUCTURA In Estructura.ESTRUCTURA1.OrderBy(Function(o) o.ORDEN).ThenBy(Function(o) o.DESCRIPCION)
                    CargarTreeView(TreeView, Reg, Nodo)
                Next
            End If
            '-------------------------------------------------------------------------------
        End If
    End Sub
#End Region
End Class