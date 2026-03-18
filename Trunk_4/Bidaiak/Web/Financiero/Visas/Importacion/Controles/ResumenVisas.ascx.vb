Imports BidaiakLib

Public Class ResumenVisas
    Inherits System.Web.UI.UserControl

#Region "Eventos"

    Private itzultzaileWeb As New Itzultzaile
    Public Event Advertencia(ByVal mensaje As String)
    Public Event ErrorGenerado(ByVal mensaje As String)
    Public Event Finalizado(ByVal numAsociados As Integer, ByVal numNoAsociados As Integer)

#End Region

#Region "Propiedades"

    ''' <summary>
    ''' Id de la planta actual
    ''' </summary>
    ''' <value></value>
    ''' <returns></returns>    
    Private ReadOnly Property IdPlanta As Integer
        Get
            Return CInt(Session("IdPlanta"))
        End Get       
    End Property

#End Region

#Region "Page Load"

    ''' <summary>
    ''' Carga la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If Not Page.IsPostBack Then
            inicializar()
        End If
    End Sub

    ''' <summary>
    ''' Inicializa los controles del control
    ''' </summary>    
    Private Sub inicializar()
        lblNumRegProc.Text = "0" : lblRegViajes.Text = "0" : lblRegSinViaje.Text = "0"
        pnlPendientes.Visible = False : pnlTodosOk.Visible = False
    End Sub

    ''' <summary>
    ''' Se traduce el control
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        itzultzaileWeb.Itzuli(labelNReg) : itzultzaileWeb.Itzuli(labelRegAsoc) : itzultzaileWeb.Itzuli(labelRegSinAsoc)
        itzultzaileWeb.Itzuli(pnlPendientes) : itzultzaileWeb.Itzuli(imgGuardar) : itzultzaileWeb.Itzuli(pnlTodosOk)
        itzultzaileWeb.Itzuli(btnContinuar)
    End Sub

#End Region

#Region "Iniciar"

    ''' <summary>
    ''' Proceso para que se inicie el control
    ''' </summary>    
    ''' <param name="numRegAsociados">Numero de registros asociados a un viaje</param>
    ''' <param name="numRegNoAsociados">Numero de registros sin asociar a ningun viaje</param>
    Public Function Iniciar(ByVal numRegAsociados As Integer, ByVal numRegNoAsociados As Integer) As Boolean
        Try
            inicializar()
            lblNumRegProc.Text = numRegAsociados + numRegNoAsociados
            lblRegViajes.Text = numRegAsociados
            lblRegSinViaje.Text = numRegNoAsociados
            Dim visasBLL As New BLL.VisasBLL
            Dim lVisas As List(Of String()) = visasBLL.loadVisasTmp(IdPlanta)
            Dim lVisasPend As List(Of String()) = lVisas.FindAll(Function(o As String()) o(14) = String.Empty)  'Se buscan las que tengan como idUsuario un null
            If (lVisasPend IsNot Nothing AndAlso lVisasPend.Count > 0) Then
                pnlPendientes.Visible = True
                Dim lVisasAux As List(Of String())
                Dim persona As String
                Dim sVisa As String()
                For Index As Integer = 0 To lVisasPend.Count - 1
                    sVisa = lVisasPend.Item(Index)
                    persona = sVisa(0)
                    If (Index < lVisasPend.Count - 1) Then
                        lVisasAux = lVisasPend.FindAll(Function(o As String()) o(0) = persona)
                        If (lVisasAux IsNot Nothing AndAlso lVisasAux.Count > 1) Then
                            'Si tiene mas de un elemento y el primer elemento es el mismo, habra que pintarle el mas, sino no
                            If (lVisasAux.First Is sVisa) Then sVisa(12) &= "|rep" 'Indica que tiene repeticione. Estado|rep
                        End If
                    End If
                Next
                gvVisas.DataSource = lVisasPend
                gvVisas.DataBind()
            Else
                pnlTodosOk.Visible = True
            End If
            Return True
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli("Error al mostrar el resumen de las visas despues de la importacion"))
            Return False
        Catch ex As Exception
            Dim sms As String = "Error al mostrar el resumen de las visas despues de la importacion"
            PageBase.log.Error(sms, ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli(sms))
            Return False
        End Try
    End Function

    ''' <summary>
    ''' Una vez guardada, recarga la pagina
    ''' </summary>    
    Public Sub recargar()
        Try
            inicializar()
            Dim visasBLL As New BLL.VisasBLL
            Dim lVisas As List(Of String()) = visasBLL.loadVisasTmp(IdPlanta)
            'NumGestionadas:las que el campo idUser no es null            
            Dim numGestionadas As Integer = lVisas.FindAll(Function(o As String()) o(7) <> String.Empty).Count
            Dim numNoGestionadas As Integer = lVisas.FindAll(Function(o As String()) o(7) = String.Empty And o(14) <> String.Empty).Count
            lblNumRegProc.Text = numGestionadas + numNoGestionadas
            lblRegViajes.Text = numGestionadas
            lblRegSinViaje.Text = numNoGestionadas
            Dim lVisasPend As List(Of String()) = lVisas.FindAll(Function(o As String()) o(14) = String.Empty)
            If (lVisasPend IsNot Nothing AndAlso lVisasPend.Count > 0) Then
                pnlPendientes.Visible = True
                Dim persona As String
                Dim sVisa As String()
                Dim lVisasAux As List(Of String())
                For Index As Integer = 0 To lVisasPend.Count - 1
                    sVisa = lVisasPend.Item(Index)
                    persona = sVisa(0)
                    If (Index < lVisasPend.Count - 1) Then
                        lVisasAux = lVisasPend.FindAll(Function(o As String()) o(0) = persona)
                        If (lVisasAux IsNot Nothing AndAlso lVisasAux.Count > 1) Then
                            'Si tiene mas de un elemento y el primer elemento es el mismo, habra que pintarle el mas, sino no
                            If (lVisasAux.First Is sVisa) Then sVisa(12) &= "|rep" 'Indica que tiene repeticione. Estado|rep
                        End If
                    End If
                Next
                gvVisas.DataSource = lVisasPend
                gvVisas.DataBind()
            Else
                pnlTodosOk.Visible = True
                pnlPendientes.Visible = Not pnlTodosOk.Visible
            End If
        Catch batzEx As BidaiakLib.BatzException
            Throw batzEx
        Catch ex As Exception
            Throw New BidaiakLib.BatzException(itzultzaileWeb.Itzuli("Ha ocurrido un error al recargar las facturas temporales de eroski"), ex)
        End Try
    End Sub

#End Region

#Region "Eventos"

    ''' <summary>
    ''' Se enlazan los datos
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub gvVisas_RowDataBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvVisas.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            ItzultzaileWeb.TraducirWebControls(e.Row.Controls)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            Dim sVisa As String() = e.Row.DataItem
            Dim lblId As Label = CType(e.Row.FindControl("lblId"), Label)
            Dim lblFechaMov As Label = CType(e.Row.FindControl("lblFecha"), Label)
            Dim lblTarjeta As Label = CType(e.Row.FindControl("lblTarjeta"), Label)
            Dim lblPersona As Label = CType(e.Row.FindControl("lblPersona"), Label)
            Dim txtNombreUsuario As TextBox = CType(e.Row.FindControl("txtNombreUsuario"), TextBox)
            Dim hfIdResponsable As HiddenField = CType(e.Row.FindControl("hfIdResponsable"), HiddenField)
            Dim helper As HtmlGenericControl = CType(e.Row.FindControl("helper"), HtmlGenericControl)
            Dim imgAutoRellenar As ImageButton = CType(e.Row.FindControl("imgAutoRellenar"), ImageButton)
            Dim imgException As ImageButton = CType(e.Row.FindControl("imgException"), ImageButton)
            Dim userRep(2) As String
            If (sVisa(12) IsNot Nothing) Then
                userRep = sVisa(12).Split("|")  'Para indicar si el usuario esta repetido. Si tiene 2 elementos en la columna de estado estara repetida la persona
            Else
                userRep(0) = String.Empty
            End If
            lblId.Text = sVisa(15)
            lblTarjeta.Text = sVisa(1)
            lblPersona.Text = sVisa(0)
            lblFechaMov.Text = sVisa(4)
            Dim idViajes As String = String.Empty
            txtNombreUsuario.Text = String.Empty
            'Se comprueba si esa persona tiene mas repeticiones en el grid para añadirle un boton de autorrellenado
            Dim autoRellenar As Boolean = (userRep.Length = 2 AndAlso userRep(0) <> String.Empty)
            imgAutoRellenar.Visible = (userRep.Length = 2)   'Solo se visualizara, si en el nombre de la persona viene nombre|rep
            If (userRep.Length = 2) Then
                imgAutoRellenar.OnClientClick = "Autorrellenar('" & sVisa(0) & "'," & e.Row.RowIndex + 1 & ");return false;"  'Se le pasa el nombre de la persona y el indice de su primera aparicion. El indice es +1 porque luego en Javascript, parece que empieza en 1
            End If
            imgException.OnClientClick = "Exception('" & sVisa(1) & "','" & itzultzaileWeb.Itzuli("Excepcion") & "'," & e.Row.RowIndex + 1 & ");return false;"
            itzultzaileWeb.Itzuli(imgAutoRellenar) : itzultzaileWeb.Itzuli(imgException)
            'Este script se añade para que se pueda buscar un usuario por JSON escribiendo. Se mostraran tambien los que estan de baja y solo los de Igorre
            Dim script As String = "init('" & txtNombreUsuario.ClientID & "', '" & hfIdResponsable.ClientID & "', '" & helper.ClientID & "','../../../Publico/BuscarUsuarios.aspx?baja=1&igorre=1',false,'');"
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "init_" & e.Row.RowIndex, script, True)
        End If
    End Sub

    ''' <summary>
    ''' Guarda los cambios realizados
    ''' Si al guardar comprueba que todavia existen visas por asignar, se mantiene en la pagina
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub imgGuardar_Click(ByVal sender As Object, ByVal e As System.Web.UI.ImageClickEventArgs) Handles imgGuardar.Click
        Try
            Dim lVisas As New List(Of String())
            Dim lVisasExc As New List(Of Object)
            Dim hiddenValue, numTarjeta, tipo As String
            Dim idPlantaGestion As Integer = CType(Session("IdPlanta"), Integer)
            Dim idMovimiento As Integer
            For Each row As GridViewRow In gvVisas.Rows
                hiddenValue = CType(row.Cells(4).Controls(3), HiddenField).Value
                tipo = String.Empty
                If (hiddenValue <> String.Empty) Then
                    tipo = If(hiddenValue.StartsWith("E|"), "Ex", "User")
                    idMovimiento = CInt(CType(row.Cells(0).Controls(0), Label).Text)
                    If (tipo = "User") Then
                        Dim sVisa(5) As String  '0:id_mov,1:fecha,2:id_planta,3:id_user
                        sVisa(0) = idMovimiento
                        sVisa(1) = CType(row.Cells(1).Controls(0), Label).Text  'Fecha_mov
                        sVisa(2) = idPlantaGestion
                        sVisa(3) = hiddenValue  'Id del usuario
                        lVisas.Add(sVisa)
                    Else
                        numTarjeta = hiddenValue.Split("|")(1)
                        If (Not lVisasExc.Exists(Function(o) o.IdMovimiento = idMovimiento)) Then lVisasExc.Add(New With {.IdMovimiento = idMovimiento, .IdPlanta = IdPlanta, .NumTarjeta = numTarjeta, .NombreTarjeta = CType(row.Cells(3).Controls(0), Label).Text})
                    End If
                End If
            Next
            If (lVisas.Count = 0 And lVisasExc.Count = 0) Then
                RaiseEvent Advertencia(itzultzaileWeb.Itzuli("noHayDatosQueGuardar"))
            Else
                Dim visasBLL As New BLL.VisasBLL
                If (lVisas.Count > 0) Then
                    PageBase.log.Info("IMPORT_VISA (PASO 3): Se van a guardar las relaciones de visas con personas")
                    visasBLL.UpdateUserVisasTmp(lVisas)
                    PageBase.log.Info("IMPORT_VISA: Personas relacionadas con exito")
                End If
                If (lVisasExc.Count > 0) Then
                    PageBase.log.Info("IMPORT_VISA (PASO 3): Se van a guardar las excepciones de visa")
                    visasBLL.AddVisasException(lVisasExc)
                    PageBase.log.Info("IMPORT_VISA: Excepcion de visas guardadas con exito")
                End If
                Try
                    recargar()
                Catch batzEx As BidaiakLib.BatzException
                    RaiseEvent Advertencia(itzultzaileWeb.Itzuli("Los datos ha sido guardados pero ha ocurrido un error al recargar los movimientos de visa"))
                End Try
            End If
        Catch batzEx As BidaiakLib.BatzException
            RaiseEvent ErrorGenerado(batzEx.Termino)
        Catch ex As Exception
            Dim sms As String = "Error al guardar los movimientos de visa"
            PageBase.log.Error(sms, ex)
            RaiseEvent ErrorGenerado(itzultzaileWeb.Itzuli(sms))
        End Try
    End Sub

    ''' <summary>
    ''' Continua al siguiente paso
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>    
    Private Sub btnContinuar_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnContinuar.Click
        Try
            Dim visaBLL As New BLL.VisasBLL
            Dim lVisas As List(Of String()) = visaBLL.loadVisasTmp(IdPlanta)
            Dim numGestionadas As Integer = lVisas.FindAll(Function(o As String()) o(7) <> String.Empty).Count
            Dim numNoGestionadas As Integer = lVisas.FindAll(Function(o As String()) o(7) = String.Empty And o(14) <> String.Empty).Count
            RaiseEvent Finalizado(numGestionadas, numNoGestionadas)
        Catch ex As Exception
            PageBase.log.Error("IMPORT_VISA: ERROR al obtener los viajes gestionados y no gestionados al ir a continuar al paso siguiente")
            RaiseEvent Finalizado(0, 0)
        End Try
    End Sub

#End Region

End Class