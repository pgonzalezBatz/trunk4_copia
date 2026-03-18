Imports AccesoAutomaticoBD
Imports GertakariakLib
Imports System.Diagnostics.Eventing.Reader
Imports System.Net.Mail
Public Class Facturacion
    Inherits PageBase

#Region "Propiedades"
    Private _idGtk As Integer = Integer.MinValue
    'Private log As log4net.ILog = log4net.LogManager.GetLogger("root.XBI")

    Protected Property IdGtk() As Integer
        Get
            Return _idGtk
        End Get
        Set(ByVal value As Integer)
            _idGtk = value
        End Set
    End Property

    Dim _gtkTicket As GertakariakLib.ELL.gtkTicket
    Protected ReadOnly Property gtkTicket
        Get
            If Ticket Is Nothing Then
                _gtkTicket = Nothing
            ElseIf _gtkTicket Is Nothing Then
                _gtkTicket = New GertakariakLib.ELL.gtkTicket
                _gtkTicket.Culture = Ticket.Culture
                _gtkTicket.IdDepartamento = Ticket.IdDepartamento
                _gtkTicket.IdEmpresa = Ticket.IdEmpresa
                _gtkTicket.IdSession = Ticket.IdSession
                _gtkTicket.NombrePersona = Ticket.NombrePersona
                _gtkTicket.Apellido1 = Ticket.Apellido1
                _gtkTicket.Apellido2 = Ticket.Apellido2
                _gtkTicket.IdTrabajador = Ticket.IdTrabajador
                _gtkTicket.IdUser = Ticket.IdUser
                _gtkTicket.NombreUsuario = Ticket.NombreUsuario

                Dim usercomp As New GertakariakLib.BLL.gtkUsuarioComponent
                Dim oUser As New GertakariakLib.ELL.gtkUsuario
                oUser.IdUsrSab = _gtkTicket.IdUser
                _gtkTicket.UsuarioRoles = usercomp.ConsultarListado(oUser)
            End If

            Return _gtkTicket
        End Get
    End Property
#End Region
#Region "Eventos de Pagina"
    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        IdGtk = Request("Id")

        'Dim dbTablaBBDD1 As New GertakariakLib.DAL.GERTAKARIAK  'Tabla LINEASCOSTE. (DataBase - db)
        'dbTablaBBDD1.ID = 1
        'Dim dbTablaBBDD As New GertakariakLib.DAL.LINEASCOSTE  'Tabla LINEASCOSTE. (DataBase - db)
        'dbTablaBBDD.ID = 1
        'dbTablaBBDD = dbTablaBBDD
        '#If DEBUG Then
        '        '------------------------------------------
        '        'FROGA: Pasar esta linea a DEBUG.
        '        '------------------------------------------
        '        If IdGtk <= 0 Then IdGtk = 16014
        '        '------------------------------------------
        '#End If
        '#If DEBUG Then
        '        Dim gtkGertakaria As New GertakariakLib.ELL.gtkTroqueleria
        '        Dim FuncGtk As New GertakariakLib.BLL.GertakariakComponent
        '        Dim ListaGtk As New Generic.List(Of Object)

        '        gtkGertakaria.Id = IdGtk
        '        ListaGtk = FuncGtk.Consultar(gtkGertakaria)
        '        gtkGertakaria = ListaGtk.Item(0)

        '        '---------------------------------------------------
        '        'Si las acciones estan cerradas se cierra la N.C.
        '        '---------------------------------------------------
        '        'If AccionesCerradas(gtkGertakaria) = True Then gtkGertakaria.FechaCierre = Now
        '        'Global_asax.log.Info("Modificar(gtkGertakaria) - INICIO")
        '        FuncGtk.Modificar(gtkGertakaria)
        '        'Global_asax.log.Info("Modificar(gtkGertakaria) - FIN")
        '        '---------------------------------------------------
        '#End If

    End Sub
    Protected Sub Page_PreRender(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.PreRender
        If Not Page.IsPostBack Then
            Dim Termino As String

            Termino = ItzultzaileWeb.Itzuli("recuperacion")
            Termino &= "<SMALL>"
            Termino &= " (" & ItzultzaileWeb.Itzuli("administracion")
            Termino &= " - " & ItzultzaileWeb.Itzuli("subcontratacion") & ")"
            Termino &= "</SMALL>"
            tipoFacturacion.Items.Add(New ListItem(Termino, "subcontratacion"))

            Termino = ItzultzaileWeb.Itzuli("devolucion")
            Termino &= "<SMALL>"
            Termino &= " (" & ItzultzaileWeb.Itzuli("compras") & ")"
            Termino &= "</SMALL>"
            tipoFacturacion.Items.Add(New ListItem(Termino, "compras"))
        End If
    End Sub
#End Region
#Region "Acciones"
    'Protected Sub btnVolver_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles btnVolver.Click
    '    VolverDetalle()
    'End Sub
#End Region
#Region "Funciones y Procesos"
    Sub CargarCilindros()
        Dim lstCilindros As New List(Of GertakariakLib2.gtkGCCILINDROS)

        '-------------------------------------------------------------------------
        '1º-Comprobamos que alguna de las lineas de coste de la NC sean cilindros
        '-------------------------------------------------------------------------
        Dim Gertakaria As New GertakariakLib.ELL.gtkTroqueleria
        Dim FuncGtk As New GertakariakLib.BLL.GertakariakComponent
        Dim ListaGtk As New List(Of Object)
        Gertakaria.Id = IdGtk
        ListaGtk = FuncGtk.Consultar(Gertakaria)
        Gertakaria = ListaGtk.Item(0)

        If Gertakaria.LineasCoste IsNot Nothing AndAlso Gertakaria.LineasCoste.Any Then
            For Each Linea As ELL.gtkLineaCoste In Gertakaria.LineasCoste
                Dim objCilindros As New GertakariakLib2.gtkGCCILINDROS
                Dim ListadoOBJ As New List(Of GertakariakLib2.gtkGCCILINDROS)
                objCilindros.NUMALBAR = gvAlbaranes.SelectedDataKey("NUMALBAR")
                objCilindros.NUMPED = Linea.NumPed
                objCilindros.NUMLIN = Linea.NumLin
                ListadoOBJ = objCilindros.Listado
                If ListadoOBJ IsNot Nothing AndAlso ListadoOBJ.Any Then
                    For Each Cilindro As GertakariakLib2.gtkGCCILINDROS In ListadoOBJ
                        lstCilindros.Add(Cilindro)
                    Next
                End If
            Next
        End If
        '-------------------------------------------------------------------------

        '-------------------------------------------------------------------------
        '2º- Pintamos los Cilindros
        '-------------------------------------------------------------------------
        If lstCilindros IsNot Nothing AndAlso lstCilindros.Any Then
            pnlCilindros.Visible = True
            gvCilindros.DataSource = lstCilindros
            gvCilindros.DataBind()
        End If
        '-------------------------------------------------------------------------
    End Sub
    Sub CargarBulones()
        Dim lstBulones As New List(Of GertakariakLib2.gtkGCBULONES)

        '-------------------------------------------------------------------------
        '1º-Comprobamos que alguna de las lineas de coste de la NC sean Bulones
        '-------------------------------------------------------------------------
        Dim Gertakaria As New GertakariakLib.ELL.gtkTroqueleria
        Dim FuncGtk As New GertakariakLib.BLL.GertakariakComponent
        Dim ListaGtk As New List(Of Object)
        Gertakaria.Id = IdGtk
        ListaGtk = FuncGtk.Consultar(Gertakaria)
        Gertakaria = ListaGtk.Item(0)

        If Gertakaria.LineasCoste IsNot Nothing AndAlso Gertakaria.LineasCoste.Any Then
            For Each Linea As ELL.gtkLineaCoste In Gertakaria.LineasCoste
                Dim objBulones As New GertakariakLib2.gtkGCBULONES
                Dim ListadoOBJ As New List(Of GertakariakLib2.gtkGCBULONES)
                objBulones.NUMALBAR = gvAlbaranes.SelectedDataKey("NUMALBAR")
                objBulones.NUMPED = Linea.NumPed
                objBulones.NUMLIN = Linea.NumLin
                ListadoOBJ = objBulones.Listado
                If ListadoOBJ IsNot Nothing AndAlso ListadoOBJ.Any Then
                    For Each Bulon As GertakariakLib2.gtkGCBULONES In ListadoOBJ
                        lstBulones.Add(Bulon)
                    Next
                End If
            Next
        End If
        '-------------------------------------------------------------------------

        '-------------------------------------------------------------------------
        '2º- Pintamos los Bulones
        '-------------------------------------------------------------------------
        If lstBulones IsNot Nothing AndAlso lstBulones.Any Then
            pnlBulones.Visible = True
            gvBulones.DataSource = lstBulones
            gvBulones.DataBind()
        End If
        '-------------------------------------------------------------------------
    End Sub
    ''' <summary>
    ''' Proceso de eliminacion de los "Cilindros" a devolver
    ''' </summary>
    ''' <param name="gtkGertakaria"></param>
    ''' <remarks></remarks>
    Sub EliminacionCilindros(ByRef gtkGertakaria As GertakariakLib.ELL.gtkTroqueleria, ByRef Transaccion As XBATLib.Transaccion)
        'Sub EliminacionCilindros(ByRef gtkGertakaria As GertakariakLib.ELL.gtkTroqueleria, ByRef bdGCALBARA As DAL.GCALBARA, ByRef Transaccion As XBATLib.Transaccion)
        Global_asax.log.Info("Proceso de eliminacion de los cilindros a devolver - INICIO")
        '----------------------------------------------------------------------------------------------------------
        'Proceso de eliminacion de los cilindros a devolver.
        '----------------------------------------------------------------------------------------------------------
        '1º- Comprobamos que alguna de las Lineas de Coste de la Incidencia (LineasCoste) sea de Cilindros.
        Dim objCilindros As New GertakariakLib2.gtkGCCILINDROS
        Dim ListadoOBJ As New List(Of GertakariakLib2.gtkGCCILINDROS)
        For Each LineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
            objCilindros.NUMPED = LineaCoste.NumPed
            objCilindros.NUMLIN = LineaCoste.NumLin
            objCilindros.NUMALBAR = gvAlbaranes.SelectedDataKey("NUMALBAR")
            Dim lCilindros As List(Of GertakariakLib2.gtkGCCILINDROS) = objCilindros.Listado
            If lCilindros IsNot Nothing AndAlso lCilindros.Any Then
                For Each Cilindro As GertakariakLib2.gtkGCCILINDROS In lCilindros
                    ListadoOBJ.Add(Cilindro)
                Next
            End If
        Next

        If ListadoOBJ IsNot Nothing AndAlso ListadoOBJ.Any Then
            '2º- Comparamos con las Lineas de Coste de la Incidencia sean del mismo albaran.
            For Each LineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
                If LineaCoste.Albaranes.Exists(Function(o) o = gvAlbaranes.SelectedDataKey("NUMALBAR")) = False Then
                    Throw New ApplicationException("Alguna de las Lineas de pedido de cilindros no corresponde con el albaran seleccionado.", New System.ApplicationException)
                End If
            Next

            '3º- Comprobamos que para la "Cantidad" de Linea de Coste corresponda el mismo numero de cilindros seleccionados.
            For Each LineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
                Dim ContadorCilindros As Integer = 0
                If gvCilindros.Rows.Count > 0 Then
                    For Each Fila As GridViewRow In gvCilindros.Rows
                        Dim Seleccionado As CheckBox = Fila.FindControl("chkCilindro")
                        If Seleccionado IsNot Nothing AndAlso (Seleccionado.Checked = True _
                        And gvCilindros.DataKeys(Fila.RowIndex).Item("NUMPED") = LineaCoste.NumPed _
                        And gvCilindros.DataKeys(Fila.RowIndex).Item("NUMLIN") = LineaCoste.NumLin) Then
                            ContadorCilindros += 1
                        End If
                    Next
                    If LineaCoste.CantidadPed <> ContadorCilindros Then
                        Throw New ApplicationException("Número de Cilindros seleccionados no corresponden con las lineas de coste.", New System.ApplicationException)
                    Else
                        '------------------------------------------------------------------------------------------
                        '4º- Eliminamos los cilindros seleccionados.
                        'Solo si en la funcion de "ModificarAlbaran", el campo "GCALBARA.CANREC = 0" proboca que
                        'se eliminine los registros relacionados en "GCHISMOV" y el registro de "GCALBARA". 
                        'Al eliminarse los registros de "GCHISMOV" salta un Triger que borra los cilindros relacionados con "GCHISMOV".
                        '------------------------------------------------------------------------------------------
                        Dim bdGCALBARA As New DAL.GCALBARA
                        '------------------------------------------------------------------------------------------------
                        'FROGA:2012-06-12: Si no hay errores con los cilindros dejar esto.
                        '------------------------------------------------------------------------------------------------
                        If gvAlbaranes.Rows.Count > 0 Then
                            bdGCALBARA.Where.TIPO.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(1)
                            bdGCALBARA.Where.CODPROV.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(2).ToString.Trim
                            bdGCALBARA.Where.ANNO.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(3)
                            bdGCALBARA.Where.NUMALBAR.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(0)
                        End If
                        '------------------------------------------------------------------------------------------------
                        bdGCALBARA.Where.NUMPED.Value = LineaCoste.NumPed
                        bdGCALBARA.Where.NUMLIN.Value = LineaCoste.NumLin
                        bdGCALBARA.Query.Load()

                        If bdGCALBARA.RowCount <> 0 Then
                            Try
                                If Transaccion.Estado = ConnectionState.Closed Then Transaccion.Abrir()
                                For Each Fila As GridViewRow In gvCilindros.Rows
                                    Dim Seleccionado As CheckBox = Fila.FindControl("chkCilindro")
                                    If Seleccionado IsNot Nothing AndAlso Seleccionado.Checked = True Then
                                        Dim gtkCilindro As New GertakariakLib2.gtkGCCILINDROS
                                        gtkCilindro.Cargar(gvCilindros.DataKeys(Fila.RowIndex).Value)
                                        If gtkCilindro.ID IsNot Nothing Then
                                            gtkCilindro.Eliminar()
                                        End If
                                    End If
                                Next
                            Catch ex As Exception
                                Global_asax.log.Error(ex)
                                Global_asax.log.Info("Proceso de eliminacion de los cilindros a devolver - Rollback")
                                Transaccion.Rollback()
                                Throw New ApplicationException("Error al eliminar los cilindros.", New System.ApplicationException)
                            End Try
                        End If
                        '------------------------------------------------------------------------------------------
                    End If
                End If
                '--------------------------------------------------------------------------------------------------
            Next
        End If
        '---------------------------------------------------------------------------------------------
        Global_asax.log.Info("Proceso de eliminacion de los cilindros a devolver - FIN")
    End Sub
    ''' <summary>
    ''' Proceso de eliminacion de los "Bulones" a devolver
    ''' </summary>
    ''' <param name="gtkGertakaria"></param>
    ''' <remarks></remarks>
    Sub EliminacionBulones(ByRef gtkGertakaria As GertakariakLib.ELL.gtkTroqueleria, ByRef Transaccion As XBATLib.Transaccion)
        'Sub EliminacionBulones(ByRef gtkGertakaria As GertakariakLib.ELL.gtkTroqueleria, ByRef bdGCALBARA As DAL.GCALBARA, ByRef Transaccion As XBATLib.Transaccion)
        Global_asax.log.Info("Proceso de eliminacion de los Bulones a devolver - INICIO")
        '----------------------------------------------------------------------------------------------------------
        'Proceso de eliminacion de los Bulones a devolver.
        '----------------------------------------------------------------------------------------------------------
        '1º- Comprobamos que alguna de las Lineas de Coste de la Incidencia (LineasCoste) sea de Bulones.
        Dim objBulones As New GertakariakLib2.gtkGCBULONES
        Dim ListadoOBJ As New List(Of GertakariakLib2.gtkGCBULONES)
        For Each LineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
            objBulones.NUMPED = LineaCoste.NumPed
            objBulones.NUMLIN = LineaCoste.NumLin
            objBulones.NUMALBAR = gvAlbaranes.SelectedDataKey("NUMALBAR")
            Dim lBulones As List(Of GertakariakLib2.gtkGCBULONES) = objBulones.Listado
            If lBulones IsNot Nothing AndAlso lBulones.Any Then
                For Each Bulon As GertakariakLib2.gtkGCBULONES In lBulones
                    ListadoOBJ.Add(Bulon)
                Next
            End If
        Next

        If ListadoOBJ IsNot Nothing AndAlso ListadoOBJ.Any Then
            '2º- Comparamos con las Lineas de Coste de la Incidencia sean del mismo albaran.
            For Each LineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
                If LineaCoste.Albaranes.Exists(Function(o) o = gvAlbaranes.SelectedDataKey("NUMALBAR")) = False Then
                    Throw New ApplicationException("Alguna de las Lineas de pedido de Bulones no corresponde con el albaran seleccionado.", New System.ApplicationException)
                End If
            Next

            '3º- Comprobamos que para la "Cantidad" de Linea de Coste corresponda el mismo numero de Bulones seleccionados.
            For Each LineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
                Dim ContadorBulones As Integer = 0
                If gvBulones.Rows.Count > 0 Then
                    For Each Fila As GridViewRow In gvBulones.Rows
                        Dim Seleccionado As CheckBox = Fila.FindControl("chkBulon")
                        If Seleccionado IsNot Nothing AndAlso (Seleccionado.Checked = True _
                        And gvBulones.DataKeys(Fila.RowIndex).Item("NUMPED") = LineaCoste.NumPed _
                        And gvBulones.DataKeys(Fila.RowIndex).Item("NUMLIN") = LineaCoste.NumLin) Then
                            ContadorBulones += 1
                        End If
                    Next
                    If LineaCoste.CantidadPed <> ContadorBulones Then
                        Throw New ApplicationException("Número de Bulones seleccionados no corresponden con las lineas de coste.", New System.ApplicationException)
                    Else
                        '------------------------------------------------------------------------------------------
                        '4º- Eliminamos los Bulones seleccionados.
                        'Solo si en la funcion de "ModificarAlbaran", el campo "GCALBARA.CANREC = 0" proboca que
                        'se eliminine los registros relacionados en "GCHISMOV" y el registro de "GCALBARA". 
                        'Al eliminarse los registros de "GCHISMOV" salta un Triger que borra los Bulones relacionados con "GCHISMOV".
                        '------------------------------------------------------------------------------------------
                        Dim bdGCALBARA As New DAL.GCALBARA
                        '------------------------------------------------------------------------------------------------
                        'FROGA:2012-06-12: Si no hay errores con los bulones dejar esto.
                        '------------------------------------------------------------------------------------------------
                        If gvAlbaranes.Rows.Count > 0 Then
                            bdGCALBARA.Where.TIPO.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(1)
                            bdGCALBARA.Where.CODPROV.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(2).ToString.Trim
                            bdGCALBARA.Where.ANNO.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(3)
                            bdGCALBARA.Where.NUMALBAR.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(0)
                        End If
                        '------------------------------------------------------------------------------------------------
                        bdGCALBARA.Where.NUMPED.Value = LineaCoste.NumPed
                        bdGCALBARA.Where.NUMLIN.Value = LineaCoste.NumLin
                        bdGCALBARA.Query.Load()
                        If bdGCALBARA.RowCount <> 0 Then
                            Try
                                If Transaccion.Estado = ConnectionState.Closed Then Transaccion.Abrir()
                                For Each Fila As GridViewRow In gvBulones.Rows
                                    Dim Seleccionado As CheckBox = Fila.FindControl("chkBulon")
                                    If Seleccionado IsNot Nothing AndAlso Seleccionado.Checked = True Then
                                        Dim gtkBulon As New GertakariakLib2.gtkGCBULONES
                                        gtkBulon.Cargar(gvBulones.DataKeys(Fila.RowIndex).Value)
                                        If gtkBulon.ID IsNot Nothing Then
                                            gtkBulon.Eliminar()
                                        End If
                                    End If
                                Next
                            Catch ex As Exception
                                Global_asax.log.Error(ex)
                                Global_asax.log.Info("Proceso de eliminacion de los Bulones a devolver - Rollback")
                                Transaccion.Rollback()
                                Throw New ApplicationException("Error al eliminar los Bulones.", New System.ApplicationException)
                            End Try
                        End If
                        '------------------------------------------------------------------------------------------
                    End If

                End If
                '--------------------------------------------------------------------------------------------------
            Next
        End If
        '---------------------------------------------------------------------------------------------
        Global_asax.log.Info("Proceso de eliminacion de los Bulones a devolver - FIN")
    End Sub
#End Region

    Private Sub EnviarEmail(ByVal gtkGertakaria As ELL.gtkTroqueleria)
        Dim mail As New MailMessage()
        'Dim gtkTicket As GertakariakLib.ELL.gtkTicket = Session("gtkTicket")
        Dim gtkUsuario As New SabLib.ELL.Usuario
        Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
        Dim dbAvisosEmail As New DAL.AVISOSEMAIL
        Dim gtkProveedor As New ELL.gtkProveedor
        Dim ProveedorComponent As New BLL.gtkProveedorComponent

        Try
            Dim IPLocal As Boolean = (New List(Of String) From {"::1", "192", "172", "10"}).Contains(Request.ServerVariables("REMOTE_ADDR").Split(".")(0))

            gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = gtkTicket.IdUser}, False)
            'Definir dereccion
            mail.From = New MailAddress("Gertakariak@Batz.es")


            If ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                If gtkUsuario.Email IsNot String.Empty And gtkUsuario.Email IsNot Nothing Then mail.CC.Add(gtkUsuario.Email)
                '---------------------------------------------------
                'Recogemos los email para la tramitacion
                '---------------------------------------------------
                If tipoFacturacion.SelectedValue.Trim.ToUpper = "subcontratacion".Trim.ToUpper Then
                    dbAvisosEmail.Where.IDTRAMITACION.Value = 1
                ElseIf tipoFacturacion.SelectedValue.Trim.ToUpper = "compras".Trim.ToUpper Then
                    dbAvisosEmail.Where.IDTRAMITACION.Value = 2
                End If
                dbAvisosEmail.Query.Load()

                For Each Filas As Data.DataRowView In dbAvisosEmail.DefaultView
                    gtkUsuario = New SabLib.ELL.Usuario
                    gtkUsuario = UsuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = Filas.Item(GertakariakLib.DAL.AVISOSEMAIL.ColumnNames.IDUSRSAB)}, False)
                    If gtkUsuario IsNot Nothing AndAlso gtkUsuario.Email IsNot Nothing AndAlso gtkUsuario.Email.Trim <> String.Empty Then
                        mail.To.Add(gtkUsuario.Email.ToString.Trim)
                    End If
                Next
                '---------------------------------------------------
                'mail.Bcc.Add("diglesias@batz.es")
            Else
                mail.To.Add("diglesias@batz.es")
            End If

            mail.Subject = ItzultzaileWeb.Itzuli("Pendiente de Tramitación") 'Pendiente de Tramitación.
            mail.Body = ItzultzaileWeb.Itzuli("Pendiente de Tramitación") & ":" 'Pendiente de Tramitación.

            If Not ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                mail.Subject = mail.Subject & " - FROGA"
                mail.To.Add("diglesias@batz.es")
                mail.Bcc.Clear()
            End If
            If gtkGertakaria.LineasCoste IsNot Nothing AndAlso gtkGertakaria.LineasCoste.Any Then
                Dim lstNumPed As New Generic.List(Of Integer)
                For Each LineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
                    If LineaCoste.NumPed <> Nothing AndAlso LineaCoste.NumPed <> Integer.MinValue Then
                        If lstNumPed.Contains(LineaCoste.NumPed) = False Then lstNumPed.Add(LineaCoste.NumPed)
                    End If
                Next
                If gtkGertakaria.IdProveedor IsNot Nothing AndAlso gtkGertakaria.IdProveedor IsNot String.Empty Then
                    gtkProveedor = ProveedorComponent.Consultar(gtkGertakaria.IdProveedor, Nothing, False).Item(0)
                    mail.Subject = mail.Subject & " - " & gtkProveedor.NomProv & " (" & gtkProveedor.CIF & ")"
                    mail.Body = mail.Body & "<BR>" & ItzultzaileWeb.Itzuli("Proveedor") & ": " & gtkProveedor.NomProv & " (" & gtkProveedor.CIF & ")"
                End If
                If Not lstNumPed.Any Then
                    mail.Body = mail.Body & "<BR>" & ItzultzaileWeb.Itzuli("pedidosNuevos") & ": " & gtkGertakaria.NumPedCab
                Else
                    mail.Body = mail.Body & "<BR>" & ItzultzaileWeb.Itzuli("nºPedido") & ": "
                    For i = 0 To lstNumPed.Count - 1
                        If i > 0 Then
                            mail.Body = mail.Body & ", "
                        End If
                        mail.Body = mail.Body & lstNumPed.Item(i)
                    Next
                    For Each item As Integer In lstNumPed
                        If gtkGertakaria.NumPedCab <> item Then
                            mail.Body = mail.Body & "<BR>" & ItzultzaileWeb.Itzuli("pedidosNuevos") & ": " & gtkGertakaria.NumPedCab
                            Exit For
                        End If
                    Next
                End If
            End If

            'mail.Body = mail.Body & "<A href=""" & If(IPLocal = True, "http://usotegieta2.batz.es", "https://Kuboak.batz.com") & "/ibmcognos/cgi-bin/cognosisapi.dll?b_action=cognosViewer&ui.action=run&ui.object=%2fcontent%2ffolder%5b%40name%3d%27BATZ%27%5d%2ffolder%5b%40name%3d%27Trokelgintza%20-%20Troqueleria%27%5d%2ffolder%5b%40name%3d%27Gertakariak%20-%20Incidencias%27%5d%2ffolder%5b%40name%3d%27Hobekuntza%27%5d%2freport%5b%40name%3d%27RS%20Informe%20de%20Disconformidad%27%5d&ui.name=RS%20Informe%20de%20Disconformidad&p_dincidencia=" & gtkGertakaria.Id & "&run.prompt=false&run.outputFormat=PDF&run.outputPageOrientation=portrait"" Target=""_BLANK"">" & " (" & ItzultzaileWeb.Itzuli("NoConformidad") & " - " & gtkGertakaria.Id & ")" & "</A>"
            Dim linkCognos = "https://cognos.batz.es/ibmcognos/bi/?objRef=i8C91D739AF2A43328B8ED940DC0F7481&ui.action=run&format=PDF&prompt=false&p_dincidencia=" & gtkGertakaria.Id & "&ui_appbar=false&ui_navbar=false"
            mail.Body = mail.Body & "<a href=""" & linkCognos & """ Target=""_BLANK"">" & " (" & ItzultzaileWeb.Itzuli("NoConformidad") & " - " & gtkGertakaria.Id & ")" & "</a>"


            mail.IsBodyHtml = True

            mail.BodyEncoding = System.Text.Encoding.UTF8
            mail.SubjectEncoding = System.Text.Encoding.UTF8

            Dim paramBLL As New SabLib.BLL.ParametrosBLL
            Dim serverEmail As String = paramBLL.consultarParametro("SERVER_EMAIL").Valor
            'Enviar el Mensaje
            'Dim smtp As New SmtpClient("172.17.200.3") 'IP del servidor de Exchange.
            Dim smtp As New SmtpClient(serverEmail) 'Nombre del servidor de Exchange.
            smtp.Send(mail)
            smtp.Dispose()
        Catch ex As ApplicationException
            Global_asax.log.Error(ex)
            Throw
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw
        End Try
    End Sub

    Protected Sub tipoFacturacion_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles tipoFacturacion.SelectedIndexChanged
        Dim Gertakaria As New ELL.gtkTroqueleria
        Dim lstGertakariak As New Generic.List(Of Object)
        Dim gtkFunc As New BLL.GertakariakComponent()
        Dim dbAvisosEmail As New DAL.AVISOSEMAIL
        'Dim dbAvisosEmail2 As New DAL.AVISOSEMAIL
        'dbAvisosEmail.Where.IDTRAMITACION.Value = tipoFacturacion.SelectedIndex + 1
        'dbAvisosEmail.Query.Load()
        Try
            'Gertakaria.Id = Me.IdGtk

            'lstGertakariak = gtkFunc.Consultar(Gertakaria)
            'Gertakaria = lstGertakariak.Item(0)
            ''''TODO: MEMCACHED GET GERTAKARIA
            Gertakaria = AccesoDB.GetGertakaria(Me.IdGtk)
            '-----------------------------------------------------------------
            Dim gtkLinea As New ELL.gtkLineaCoste       'Objeto para el tratamiento de los datos
            Dim gtkLineaComp As New BLL.gtkLineaCosteComponent   'Funciones
            gtkLinea.IdIncidencia = Gertakaria.Id
            Gertakaria.LineasCoste = gtkLineaComp.ConsultarListado(gtkLinea)

            'Log.Info("getting lineascoste")
            'Dim dbTablaBBDD As New GertakariakLib.DAL.LINEASCOSTE  'Tabla LINEASCOSTE. (DataBase - db)            
            'Dim gtkListObj As New List(Of ELL.gtkLineaCoste)

            'If gtkLinea.Id <> Integer.MinValue And Not (gtkLinea.Id.ToString Is Nothing Or gtkLinea.Id.ToString Is DBNull.Value) Then
            '    dbTablaBBDD.Where.ID.Value = gtkLinea.Id
            '    Log.Info("id = " & gtkLinea.Id)
            'End If
            'If gtkLinea.IdIncidencia <> Integer.MinValue And Not (gtkLinea.IdIncidencia.ToString Is Nothing Or gtkLinea.IdIncidencia.ToString Is DBNull.Value) Then
            '    dbTablaBBDD.Where.IDINCIDENCIA.Value = gtkLinea.IdIncidencia
            '    Log.Info("idincidencia = " & gtkLinea.IdIncidencia)
            'End If
            'If gtkLinea.IdOFM <> Integer.MinValue And Not (gtkLinea.IdOFM.ToString Is Nothing Or gtkLinea.IdOFM.ToString Is DBNull.Value) Then
            '    dbTablaBBDD.Where.IDOFMARCA.Value = gtkLinea.IdOFM
            '    Log.Info("ofmarca = " & gtkLinea.IdOFM)
            'End If

            'dbTablaBBDD.Query.AddOrderBy(DAL.LINEASCOSTE.ColumnNames.ID, WhereParameter.Dir.ASC)

            'dbTablaBBDD.Query.Load()
            'If Not dbTablaBBDD.EOF Then
            '    Log.Info("hay lineas")
            '    Do
            '        Dim gtkObjetoB As New ELL.gtkLineaCoste    'Nuevo Objeto que se va metiendo en la lista
            '        If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.ID) Then gtkObjetoB.Id = dbTablaBBDD.ID
            '        If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.IDINCIDENCIA) Then gtkObjetoB.IdIncidencia = dbTablaBBDD.IDINCIDENCIA
            '        If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.IDOFMARCA) Then gtkObjetoB.IdOFM = dbTablaBBDD.IDOFMARCA

            '        'gtkListObj.Add(gtkObjetoB)
            '        gtkListObj.Add(New ELL.gtkLineaCoste)

            '    Loop While dbTablaBBDD.MoveNext
            'Else
            '    Log.Info("no hay lineas")
            '    gtkListObj = Nothing
            'End If

            'Gertakaria.LineasCoste = gtkListObj

            'Log.Info("tahan")



            'dbTablaBBDD.Where.IDINCIDENCIA.Value = gtkLinea.IdIncidencia

            'dbTablaBBDD.Query.AddOrderBy(DAL.LINEASCOSTE.ColumnNames.ID, WhereParameter.Dir.ASC)

            'dbTablaBBDD.Query.Load()

            '----------------------------------------
            'Inicializamos controles
            '----------------------------------------
            Me.FecEntraga.Visible = False
            Me.Albaranes.Visible = False
            'Me.btnTratamiento.Enabled = False
            Me.btnTramitacion.Enabled = False
            '----------------------------------------

            '------------------------------------------------------------------------
            'Comprobamos que para el tipo de Tramitacion seleccionado 
            'hay usuarios que vayan a recibir el email.
            '------------------------------------------------------------------------
            dbAvisosEmail.Where.IDTRAMITACION.Value = tipoFacturacion.SelectedIndex + 1
            dbAvisosEmail.Query.Load()
            If dbAvisosEmail.EOF Then Throw New ApplicationException("Usuarios de Tramitacion no encontrados")
            '------------------------------------------------------------------------

            If ValidarNC(Gertakaria) Then
                'Me.btnTratamiento.Enabled = True
                Me.btnTramitacion.Enabled = True
                If tipoFacturacion.SelectedValue.ToUpper.Trim = "Compras".ToUpper.Trim Then Me.FecEntraga.Visible = True
            End If

        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
    Private Sub ModificarAlbaran(ByRef LineaCoste As ELL.gtkLineaCoste)
        Dim GCALBARA As New DAL.GCALBARA
        Dim GCHISMOV As New DAL.GCHISMOV

        Dim dNUMALBAR As Decimal
        Dim dANNO As Decimal
        Dim sCODPROV As String
        Dim sTIPO As String

        Try
            If gvAlbaranes.Rows.Count > 0 Then
                GCALBARA.Where.TIPO.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(1)
                GCALBARA.Where.CODPROV.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(2).ToString.Trim
                GCALBARA.Where.ANNO.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(3)
                GCALBARA.Where.NUMALBAR.Value = gvAlbaranes.DataKeys(gvAlbaranes.SelectedIndex).Values.Item(0)
            End If
            GCALBARA.Where.NUMPED.Value = LineaCoste.NumPed
            GCALBARA.Where.NUMLIN.Value = LineaCoste.NumLin

            GCALBARA.Query.Load()

            If GCALBARA.RowCount = 1 Then
                dNUMALBAR = GCALBARA.NUMALBAR
                dANNO = GCALBARA.ANNO
                sCODPROV = GCALBARA.CODPROV
                sTIPO = GCALBARA.TIPO

                GCALBARA.CANREC = GCALBARA.CANREC - LineaCoste.CantidadPed
                '#If DEBUG Then
                '                Global_asax.log.Info("GCALBARA.NUMMOV_E = " & GCALBARA.NUMMOV_E)
                '                Global_asax.log.Info("GCALBARA.NUMMOV_S = " & GCALBARA.NUMMOV_S)
                '                Global_asax.log.Info("NUMMOVES = " & GCALBARA.NUMMOV_E & " Or NUMMOVES = " & GCALBARA.NUMMOV_S)
                '#End If

                If GCALBARA.CANREC < 0 Then
                    Throw New ApplicationException("cantidaLineaCostePedido")
                ElseIf GCALBARA.CANREC = 0 Then 'Borramos el registro de GCALBARA y GCHISMOV
                    'Bilbo: NUMMOV_E y NUMMOV_S no pueden ser nulos.
                    GCHISMOV.LoadByPrimaryKey(GCALBARA.NUMMOV_E)
                    GCHISMOV.DeleteAll() 'El borrado hace saltar un triger que elimina los cilindros relacionados.
                    GCHISMOV.Save()

                    GCHISMOV.LoadByPrimaryKey(GCALBARA.NUMMOV_S)
                    GCHISMOV.DeleteAll() 'El borrado hace saltar un triger que elimina los cilindros relacionados.
                    GCHISMOV.Save()

                    GCALBARA.DeleteAll()
                Else
                    'Bilbo: NUMMOV_E y NUMMOV_S no pueden ser nulos.
                    GCHISMOV.LoadByPrimaryKey(GCALBARA.NUMMOV_E)
                    If GCHISMOV.RowCount = 1 Then GCHISMOV.CANTID = GCHISMOV.CANTID - LineaCoste.CantidadPed
                    GCHISMOV.Save()
                    GCALBARA.EIMPORTE = (GCHISMOV.EPRECIO / GCHISMOV.EDIMPRE) * GCALBARA.CANREC
                    GCHISMOV.LoadByPrimaryKey(GCALBARA.NUMMOV_S)
                    If GCHISMOV.RowCount = 1 Then GCHISMOV.CANTID = GCHISMOV.CANTID - LineaCoste.CantidadPed
                    GCHISMOV.Save()
                End If

                GCALBARA.Save()

                '----------------------------------------------------------
                'Si el albaran no tiene mas lineas se elimina su cabecera.
                '----------------------------------------------------------
                GCALBARA = New DAL.GCALBARA
                GCALBARA.Where.NUMPED.Value = LineaCoste.NumPed
                GCALBARA.Query.Load()
                If GCALBARA.RowCount = 0 Then
                    Dim GCCABALB As New DAL.GCCABALB

                    GCCABALB.Where.NUMALBAR.Value = dNUMALBAR
                    GCCABALB.Where.ANNO.Value = dANNO
                    GCCABALB.Where.CODPROV.Value = sCODPROV
                    GCCABALB.Where.TIPO.Value = sTIPO

                    GCCABALB.Query.Load()
                    GCCABALB.DeleteAll()
                    GCCABALB.Save()
                End If
                '----------------------------------------------------------

            ElseIf GCALBARA.RowCount = 0 Then
                Throw New ApplicationException("Las Líneas de Coste no corresponden con el Albarán seleccionado")
            Else
                Throw New ApplicationException("albaranes")
            End If
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw
        End Try
    End Sub
#Region "Funciones de Retorno"
    ''' <summary>
    ''' Vuelve a la pagina del listado de incidencias, a la pagina en la que estaria ubicada la incidencia, tomando en cuenta el filtro y la ordenacion existente
    ''' </summary>
    ''' <remarks></remarks>
    Private Sub Volver()
        Dim arg As String = String.Empty
        arg = "?idInc=" & IdGtk
        Response.Redirect("~/Default.aspx" & arg, True)
    End Sub

    '''' <summary>
    '''' Vuelve a la pagina de detalle de incidencias.
    '''' </summary>
    '''' <remarks></remarks>
    'Private Sub VolverDetalle()
    '    Dim arg As String = String.Empty
    '    arg = "?Id=" & IdGtk
    '    Response.Redirect("~/Incidencias/DetalleIncidencia.aspx" & arg, False)
    'End Sub
#End Region
#Region "Funciones de Validacion para la NC"
    ''' <summary>
    ''' Comprobamos si el Pedido de la N.C. esta facturado por completo.
    ''' </summary>
    ''' <param name="gtkGertakariak"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Private Function PedidoFacturado(ByVal gtkGertakariak As ELL.gtkTroqueleria) As Boolean
        Global_asax.log.Info("===================================================================")
        Global_asax.log.Info("Comprobacion Pedido Facturado (Function PedidoFacturado) - INICIO")
        Global_asax.log.Info("===================================================================")
        Try
            If gtkGertakariak.LineasCoste IsNot Nothing AndAlso gtkGertakariak.LineasCoste.Any Then
                For Each LineaCoste As ELL.gtkLineaCoste In gtkGertakariak.LineasCoste
                    If LineaCoste.NumPed <> Nothing Then
                        Dim bdGCALBARA As New DAL.GCALBARA

                        bdGCALBARA.Where.NUMPED.Value = LineaCoste.NumPed
                        bdGCALBARA.Where.NUMLIN.Value = LineaCoste.NumLin

                        '-------------------------------------------------------------------------
                        '2012-01-24: Lo ha dicho Iratxe. 
                        '-------------------------------------------------------------------------
                        If LineaCoste.CodPro Is Nothing Then Throw New ApplicationException("Falta Codigo Proveedor  (Pedido=" & LineaCoste.NumPed & ", Linea=" & LineaCoste.NumLin & ")", New System.Exception)
                        bdGCALBARA.Where.CODPROV.Value = LineaCoste.CodPro

                        If gvAlbaranes.SelectedDataKey Is Nothing Then Throw New ApplicationException("Falta Albaran", New System.Exception)
                        bdGCALBARA.Where.NUMALBAR.Value = gvAlbaranes.SelectedDataKey("NUMALBAR")
                        '-------------------------------------------------------------------------

                        Global_asax.log.Info("FROM GCALBARA WHERE NUMPED =" & LineaCoste.NumPed _
                                 & " AND NUMLIN = " & LineaCoste.NumLin _
                                 & " AND CODPROV = " & LineaCoste.CodPro _
                                 & " AND NUMALBAR = " & gvAlbaranes.SelectedDataKey("NUMALBAR"))

                        bdGCALBARA.Query.Load()
                        If Not bdGCALBARA.EOF Then
                            If Not IsDBNull(bdGCALBARA.NUMALBAR) Then
                                Dim bdGCCABALB As New DAL.GCCABALB

                                Global_asax.log.Info("FROM GCCABALB WHERE NUMALBAR =" & bdGCALBARA.NUMALBAR _
                                 & " AND TIPO = " & bdGCALBARA.TIPO _
                                 & " AND ANNO = " & bdGCALBARA.ANNO _
                                 & " AND CODPROV = " & bdGCALBARA.CODPROV)

                                bdGCCABALB.Where.NUMALBAR.Value = bdGCALBARA.NUMALBAR

                                '----------------------------------------------------------------------------------------------------
                                '2012-01-24. Lo ha dicho Iratxe.
                                '----------------------------------------------------------------------------------------------------
                                bdGCCABALB.Where.TIPO.Value = bdGCALBARA.TIPO
                                bdGCCABALB.Where.ANNO.Value = bdGCALBARA.ANNO
                                bdGCCABALB.Where.CODPROV.Value = bdGCALBARA.CODPROV
                                '----------------------------------------------------------------------------------------------------

                                bdGCCABALB.Query.Load()

                                PedidoFacturado = (Not bdGCCABALB.IsColumnNull(DAL.GCCABALB.ColumnNames.NFACTU))
                                Global_asax.log.Info("NFACTU = " & DAL.GCCABALB.ColumnNames.NFACTU & " =>  PedidoFacturado = " & (Not bdGCCABALB.IsColumnNull(DAL.GCCABALB.ColumnNames.NFACTU)))
                                Exit For
                            End If
                        Else
                            PedidoFacturado = False
                        End If
                    Else
                        Throw New ApplicationException("Faltan Nº de Pedido en la Linea de coste: " & LineaCoste.NumLin, New System.Exception)
                    End If
                Next
            Else
                Throw New ApplicationException("Faltan Lineas de Coste", New System.Exception)
            End If

        Catch ex As ApplicationException
            Global_asax.log.Error(Page.AppRelativeVirtualPath & "-->" & System.Reflection.MethodBase.GetCurrentMethod().Name & ": ", ex)
            Throw
        Catch ex As Exception
            Global_asax.log.Error(Page.AppRelativeVirtualPath & "-->" & System.Reflection.MethodBase.GetCurrentMethod().Name & ": ", ex)
            Throw
        Finally
            Global_asax.log.Info("===================================================================")
            Global_asax.log.Info("Comprobacion Pedido Facturado (Function PedidoFacturado) - FIN")
            Global_asax.log.Info("===================================================================")
        End Try
        Return PedidoFacturado
    End Function
    Private Function ProcedenciaNC(ByRef Gertakaria As ELL.gtkTroqueleria) As Integer
        Dim Resp As Integer
        Resp = Gertakaria.ProcedenciaNC
        Return Resp
    End Function
    ' ''' <summary>
    ' ''' Funcion para validar el Tratamiento de la NC
    ' ''' </summary>
    ' ''' <returns></returns>
    ' ''' <remarks></remarks>
    ' ''' 
    '-------------------------------------------------------------------------------------------------------------
    'Private Function ValidarNC(ByVal Gertakaria As ELL.gtkTroqueleria) As Boolean
    '	Dim gtkFunc As New BLL.GertakariakComponent
    '	Dim NumeroPedido As New Integer
    '	ValidarNC = False
    '	Try
    '		'-------------------------------------------------------------------------------------
    '		'Comprobamos que la NC tenga alguna "Linea de Coste" para poder ser tramitada.
    '		'-------------------------------------------------------------------------------------
    '		If Gertakaria.LineasCoste IsNot Nothing AndAlso Gertakaria.LineasCoste.Any Then
    '			'---------------------------------------------------------------------------
    '			'Validaciones Comunes
    '			'---------------------------------------------------------------------------
    '			'Las "lineas de coste" deben indicar el "Nº de Pedido" del que partio la NC.
    '			For Each LineaCoste As ELL.gtkLineaCoste In Gertakaria.LineasCoste
    '				If LineaCoste.NumPedOrigen = Integer.MinValue Or LineaCoste.NumPedOrigen <= 0 Then
    '					Throw New BatzException("FaltaPedidoOrigenEnLinea", New System.Exception)
    '				End If
    '			Next
    '			'---------------------------------------------------------------------------

    '			Select Case UCase(Me.tipoFacturacion.SelectedValue)
    '				Case UCase("SubContratacion")
    '					'1º- Comprobamos que sea de Proveedor para poder Tramitar.
    '					If ProcedenciaNC(Gertakaria) <> 2 Then Throw New BatzException("procedencianc", New System.Exception)
    '					'2º- Total Acordado no puede ser negativo.
    '					'If Gertakaria.TotalAcordado <= 0 Then Throw New BatzException("totalAcordado", New System.Exception)
    '					If Gertakaria.TotalAcordado <= 0 Then Throw New ApplicationException("totalAcordado")

    '					ValidarNC = True
    '				Case UCase("Compras")
    '					Dim lstLineasCoste As New Generic.List(Of ELL.gtkLineaCoste)
    '					lstLineasCoste = Gertakaria.LineasCoste
    '					For Each LineaCoste As ELL.gtkLineaCoste In lstLineasCoste
    '						'1º- Comprobamos que la NC tenga alguna linea de Coste con el número de pedido y todas sean reales.
    '						If LineaCoste.NumPed <> Integer.MinValue And LineaCoste.NumPed <> Nothing And LineaCoste.NumLin <> Integer.MinValue And LineaCoste.NumLin <> Nothing Then
    '							'2º- Comprobamos que las Lineas de Coste pertenezcan al mismo pedido.
    '							If NumeroPedido <> Nothing Then
    '								If NumeroPedido = LineaCoste.NumPed Then
    '									ValidarNC = True
    '									'Comprobamos que el pedido no este bloqueado por el XBAT.
    '									If BloqueoPedido(LineaCoste.NumPed) Then Throw New BatzException("pedidoBloqueado", New System.Exception)
    '								Else
    '									ValidarNC = False
    '									Throw New BatzException("errorLineasCostePedido", New System.Exception)
    '								End If
    '							Else
    '								ValidarNC = True
    '							End If
    '						Else
    '							ValidarNC = False
    '							Throw New BatzException("errorLineasCostePedido", New System.Exception)
    '						End If
    '						NumeroPedido = LineaCoste.NumPed
    '					Next

    '					'-----------------------------------------------------------------------
    '					'Comprobamos siempre si el albaran es unico.
    '					'-----------------------------------------------------------------------
    '					'Comprobamos que el albaran sea unico para este pedido.
    '					'Si no es unico debe seleccionar un albaran para descompensar.
    '					AlbaranUnico(Gertakaria)
    '					'-----------------------------------------------------------------------
    '			End Select
    '		Else
    '			ValidarNC = False 'La NC para poder ser tramitada necesita "Lineas de Coste".
    '			Throw New BatzException("Debe introducir alguna linea antes de enviarla", New System.Exception)
    '		End If
    '		'-------------------------------------------------------------------------------------

    '		If ValidarNC <> True Then Throw New BatzException("errorLineasCostePedido", New System.Exception)
    '	Catch ex As ApplicationException
    '		ValidarNC = False
    '		throw 
    '	Catch ex As BatzException
    '		ValidarNC = False
    '		throw 
    '	Catch ex As Exception
    '		ValidarNC = False
    '		Throw New BatzException("errorValidar", ex)
    '	End Try
    '	Return ValidarNC
    'End Function

    '-------------------------------------------------------------------------------------------------------------
    'FROGA:2013-07-12:
    '-------------------------------------------------------------------------------------------------------------
    Private Function ValidarNC(ByVal Gertakaria As ELL.gtkTroqueleria) As Boolean
        Dim gtkFunc As New BLL.GertakariakComponent
        Dim NumeroPedido As New Integer
        ValidarNC = False
        Try
            '-------------------------------------------------------------------------------------
            'Comprobamos que la NC tenga alguna "Linea de Coste" para poder ser tramitada.
            '-------------------------------------------------------------------------------------
            If Gertakaria.LineasCoste IsNot Nothing AndAlso Gertakaria.LineasCoste.Any Then
                '---------------------------------------------------------------------------
                'Validaciones Comunes
                '---------------------------------------------------------------------------
                'Las "lineas de coste" deben indicar el "Nº de Pedido" del que partio la NC.
                For Each LineaCoste As ELL.gtkLineaCoste In Gertakaria.LineasCoste
                    If LineaCoste.NumPedOrigen = Integer.MinValue Or LineaCoste.NumPedOrigen <= 0 Then Throw New ApplicationException("FaltaPedidoOrigenEnLinea")
                Next
                '---------------------------------------------------------------------------

                Select Case UCase(Me.tipoFacturacion.SelectedValue)
                    Case UCase("SubContratacion")
                        '1º- Comprobamos que sea de Proveedor para poder Tramitar.
                        If ProcedenciaNC(Gertakaria) <> 2 Then Throw New ApplicationException("procedencianc")
                        '2º- Total Acordado no puede ser negativo.
                        If Gertakaria.TotalAcordado <= 0 Then Throw New ApplicationException("totalAcordado")

                        ValidarNC = True
                    Case UCase("Compras")
                        '-------------------------------------------------------------------------------------------------
                        '1.- Comprobamos que las lineas de coste con NUMPED (Numero de Pedido) sea del mismo pedido. 
                        'No tenemos en cuenta las lineas sin NUMPED porque no son reales (No salen de las lineas de pedidos de XBAT) y no se van a tener en cuenta para realizar la tramitacion.
                        '-------------------------------------------------------------------------------------------------
                        If Gertakaria.LineasCoste.Where(Function(l) l.NumPed <> Integer.MinValue).Select(Function(l) l.NumPed).Distinct().Count = 1 Then
                            NumeroPedido = Gertakaria.LineasCoste.Where(Function(l) l.NumPed <> Integer.MinValue).Select(Function(l) l.NumPed).Distinct().SingleOrDefault
                        Else
                            Throw New ApplicationException("errorLineasCostePedido")
                        End If
                        '-------------------------------------------------------------------------------------------------
                        '2.- Comprobamos que el pedido no este bloqueado por XBAT.
                        '-------------------------------------------------------------------------------------------------
                        If BloqueoPedido(NumeroPedido) Then Throw New ApplicationException("pedidoBloqueado")
                        '-------------------------------------------------------------------------------------------------

                        '-----------------------------------------------------------------------
                        'Comprobamos siempre si el albaran es unico.
                        '-----------------------------------------------------------------------
                        'Comprobamos que el albaran sea unico para este pedido.
                        'Si no es unico debe seleccionar un albaran para descompensar.
                        AlbaranUnico(Gertakaria)
                        '-----------------------------------------------------------------------
                End Select
            Else
                Throw New ApplicationException("Debe introducir alguna linea antes de enviarla")
            End If
            '-------------------------------------------------------------------------------------

        Catch ex As ApplicationException
            ValidarNC = False
            Throw
        Catch ex As Exception
            ValidarNC = False
            Global_asax.log.Error(ex)
            Throw New ApplicationException("errorValidar", ex)
        End Try
        Return ValidarNC
    End Function
    Private Function AccionesCerradas(ByRef gtkGertakaria As ELL.gtkTroqueleria) As Boolean
        AccionesCerradas = False
        If gtkGertakaria.Acciones IsNot Nothing AndAlso gtkGertakaria.Acciones.Any Then
            For Each Accion As ELL.gtkEkintzak In gtkGertakaria.Acciones
                If Accion.FechaFin <> Nothing AndAlso Accion.FechaFin <> DateTime.MinValue Then
                    AccionesCerradas = True
                Else
                    AccionesCerradas = False
                    Exit For
                End If
            Next
        Else
            AccionesCerradas = True
        End If
    End Function
    Private Function BloqueoPedido(ByVal NumPedCab As Integer) As Boolean
        Dim GCCABPED As New DAL.GCCABPED
        BloqueoPedido = False
        Try

            GCCABPED.LoadByPrimaryKey(NumPedCab)
            If GCCABPED.RowCount = 1 Then
                If GCCABPED.BLOKEO = "S" Then
                    BloqueoPedido = True
                End If
            End If
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw
        End Try

    End Function
    ''' <summary>
    ''' Comprobamos que el albaran sea unico para este pedido.
    ''' Si no es unico debe seleccionar un albaran para descompensar.
    ''' </summary>
    ''' <param name="gtkGertakaria"></param>
    ''' <remarks></remarks>
    Sub AlbaranUnico(ByVal gtkGertakaria As ELL.gtkTroqueleria)
        Dim GCALBARA As New DAL.GCALBARA
        Dim dtGCALBARA As New Data.DataTable
        Dim drTabla As Data.IDataReader = Nothing
        Dim daGer As New GertakariakLib.BLL.DataReaderAdapterBatz
        Dim strSQL As String = String.Empty
        Dim strSQLIN As String = String.Empty

        Try
            strSQL = "SELECT DISTINCT GCALBARA.NUMALBAR, GCALBARA.TIPO, GCALBARA.CODPROV, GCALBARA.ANNO FROM GCALBARA INNER JOIN GCLINPED ON GCALBARA.NUMPED = GCLINPED.NUMPEDLIN AND GCALBARA.NUMLIN = GCLINPED.NUMLINLIN"

            If gtkGertakaria.LineasCoste.Any Then
                strSQL = strSQL & " WHERE (GCALBARA.NUMPED = " & gtkGertakaria.LineasCoste(0).NumPed & ")"
                For i = 0 To gtkGertakaria.LineasCoste.Count - 1
                    If gtkGertakaria.LineasCoste(i).NumMar IsNot Nothing Then
                        If i > 0 Then strSQLIN = strSQLIN & ","
                        strSQLIN = strSQLIN & "'" & gtkGertakaria.LineasCoste(i).NumMar.Trim & "'"
                    End If
                Next
                If strSQLIN IsNot String.Empty Then
                    strSQL = strSQL & " AND GCLINPED.NUMMAR IN(" & strSQLIN & ")"
                End If
            End If
            drTabla = GCALBARA.FiltrarPor(strSQL)
            daGer.FillFromReader(dtGCALBARA, drTabla)
            If dtGCALBARA.Rows.Count > 0 Then
                Albaranes.Visible = True
                If tipoFacturacion.SelectedValue.ToUpper.Trim = "Compras".ToUpper.Trim Then Me.FecEntraga.Visible = True
                gvAlbaranes.DataSource = dtGCALBARA
                gvAlbaranes.DataBind()
                If gvAlbaranes.Rows.Count > 1 Then
                    Throw New ApplicationException("selecAlbaranDescomp")
                Else
                    gvAlbaranes.SelectedIndex = 0
                    gvAlbaranes_SelectedIndexChanged(gvAlbaranes, Nothing)
                End If
            Else
                Throw New ApplicationException(ItzultzaileWeb.Itzuli("Nº Pedido") & " " & gtkGertakaria.LineasCoste(0).NumPed & " - " & ItzultzaileWeb.Itzuli("notienealbaranes"),
                                               New System.ApplicationException)
            End If
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw
        Finally
            drTabla.Close()
        End Try
    End Sub
    ''' <summary>
    ''' Priceso para la validacion de los campos de la página
    ''' </summary>
    ''' <remarks></remarks>
    Sub ValidarPagina()
        Try
            FechaEntrega()
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Throw
        End Try
    End Sub
    Sub FechaEntrega()
        Dim f1 As New DateTime
        DateTime.TryParse(Me.txtFechaEntrega.Text, f1)
        If (f1 = DateTime.MinValue) Then
            Me.FecEntraga.Visible = True
            Throw New ApplicationException("fechaIncorrecta")
        End If
    End Sub
    ''' <summary>
    ''' Obtenemos el Nº de trabajador.
    ''' </summary>
    ''' <param name="gtkTicket"></param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Langile(ByRef gtkTicket As GertakariakLib.ELL.gtkTicket) As Nullable(Of Integer)
        Dim UsrGen As New SabLib.ELL.Usuario
        Dim funUsrComp As New SabLib.BLL.UsuariosComponent

        Try
            UsrGen.Id = gtkTicket.IdUser
            UsrGen = funUsrComp.GetUsuario(UsrGen, False)

            If UsrGen.CodPersona <> Integer.MinValue AndAlso UsrGen.CodPersona.ToString IsNot Nothing AndAlso UsrGen.CodPersona.ToString IsNot DBNull.Value AndAlso UsrGen.CodPersona > 0 Then
                Langile = UsrGen.CodPersona 'XBAT.FaPersonal.CODPER (XBAT.FaPersonal.USUARIO=UserName)
            ElseIf UsrGen.CodPersona = Integer.MinValue Then
                '----------------------------------------------------------------
                'Buscamos el usuario en XBAT.FaPersonal
                '----------------------------------------------------------------
                Dim gtkFaPer As New GertakariakLib2.gtkFAPERSONAL
                Dim l_gtkFaPer As New List(Of GertakariakLib2.gtkFAPERSONAL)
                gtkFaPer.USUARIO = gtkTicket.NombreUsuario
                l_gtkFaPer = gtkFaPer.Listado()
                If l_gtkFaPer IsNot Nothing AndAlso l_gtkFaPer.Any Then
                    gtkFaPer = l_gtkFaPer.FirstOrDefault
                    Langile = gtkFaPer.CODPER
                End If
                '----------------------------------------------------------------
            End If

            If Langile Is Nothing Then Throw New ApplicationException("El usuario (" & UsrGen.NombreCompleto & ") no existe en la base de datos de 'Personal'.")
            Return Langile
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw
        End Try
    End Function
    ''' <summary>
    ''' Nº de trabajdor del pedido indicado.
    ''' </summary>
    ''' <param name="NUMPED">Nº pedido para obtener el nº. trabajador correspondiente.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Langile(ByRef NUMPED As Integer) As String
        Dim bdGCCABPEDA As New DAL.GCCABPED
        Langile = String.Empty
        Try

            With bdGCCABPEDA
                .Where.NUMPEDCAB.Value = NUMPED
                .Query.Distinct = True
                .Query.AddResultColumn(DAL.GCCABPED.ColumnNames.LANGILE)
                .Query.Load()
            End With

            If Not bdGCCABPEDA.EOF Then
                Langile = bdGCCABPEDA.LANGILE
            Else
                Throw New ApplicationException("CodigoDeTrabajador")
            End If

            Return Langile
        Catch ex As ApplicationException
            Throw
        Catch ex As Exception
            Global_asax.log.Error(ex)
            Throw New ApplicationException("CodigoDeTrabajador")
        End Try
    End Function
#End Region
#Region "GridView de Albaranes"
    ''' <summary>
    ''' Código para que cuando el raton se situe encima de una fila, se cambie el color de la misma
    ''' </summary>
    Protected Sub rowItemBound(ByVal sender As Object, ByVal e As System.Web.UI.WebControls.GridViewRowEventArgs) Handles gvAlbaranes.RowDataBound
        If (e.Row.RowType = DataControlRowType.Header) Then
            AddSortImage(e.Row)
        ElseIf (e.Row.RowType = DataControlRowType.DataRow) Then
            e.Row.Attributes.Add("onmouseover", "Sartu(this);")
            e.Row.Attributes.Add("onmouseout", "Irten(this);")
            e.Row.Attributes("OnClick") = Page.ClientScript.GetPostBackClientHyperlink(Me.gvAlbaranes, "Select$" + CStr(e.Row.RowIndex))
        End If
    End Sub
    ''' <summary>
    ''' Añade una imagen a la cabecera indicando el orden actual
    ''' </summary>
    ''' <param name="headerRow">Cabecera</param>
    ''' <remarks></remarks>
    Private Sub AddSortImage(ByVal headerRow As GridViewRow)
        If (GridViewSortExpresion <> String.Empty) Then
            Dim sortExp As String = GridViewSortExpresion
            Dim idCol As Integer = getColumnIndex(sortExp)
            If (idCol <> -1) Then
                Dim sortImage As New Image()
                If (GridViewSortDirection = SortDirection.Ascending) Then
                    sortImage.ImageUrl = "~/App_Themes/Tema1/Imagen/sortascending.gif"
                    sortImage.AlternateText = "Orden Ascendente"
                Else
                    sortImage.ImageUrl = "~/App_Themes/Tema1/Imagen/sortdescending.gif"
                    sortImage.AlternateText = "Orden Descendente"
                End If
                headerRow.Cells(idCol).Controls.Add(sortImage)
            End If
        End If
    End Sub
    ''' <summary>
    ''' Obtiene el indice de una columna
    ''' </summary>
    ''' <param name="sortExp">Expresion de orden</param>
    ''' <returns>Indice</returns>
    Private Function getColumnIndex(ByVal sortExp As String) As Integer
        For index As Integer = 0 To gvAlbaranes.Columns.Count - 1
            If (gvAlbaranes.Columns(index).SortExpression = sortExp And gvAlbaranes.Columns(index).Visible) Then
                Return index
            End If
        Next index
        Return Integer.MinValue
    End Function
    Protected Sub gvAlbaranes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles gvAlbaranes.SelectedIndexChanged
        'btnTratamiento.Enabled = True
        btnTramitacion.Enabled = True
        'CargarCilindros()
        'CargarBulones()
    End Sub
#Region "Propiedades"
    ''' <summary>
    ''' Indica la direccion en la que hay que ordenar
    ''' </summary>
    ''' <value></value>
    ''' <returns>Orden</returns>
    Public Property GridViewSortDirection() As SortDirection
        Get
            If (ViewState("sortDirection") Is Nothing) Then
                ViewState("sortDirection") = SortDirection.Descending
            End If
            Return CType(ViewState("sortDirection"), SortDirection)
        End Get
        Set(ByVal value As SortDirection)
            ViewState("sortDirection") = value
        End Set
    End Property
    ''' <summary>
    ''' Indica la expresion de ordenacion
    ''' </summary>
    ''' <value></value>
    ''' <returns>Expresion</returns>
    Public Property GridViewSortExpresion() As String
        Get
            If (ViewState("sortExpresion") Is Nothing) Then
                ViewState("sortExpresion") = "NUMALBAR"
            End If
            Return ViewState("sortExpresion").ToString()
        End Get
        Set(ByVal value As String)
            ViewState("sortExpresion") = value
        End Set
    End Property
#End Region
#End Region

    Private Sub btnTramitacion_Click(sender As Object, e As EventArgs) Handles btnTramitacion.Click
        Dim gtkGertakaria As New GertakariakLib.ELL.gtkTroqueleria

        Dim FuncGtk As New GertakariakLib.BLL.GertakariakComponent
        Dim ListaGtk As New Generic.List(Of Object)

        Dim bdGCCABPED As New DAL.GCCABPED
        Dim bdGCLINPED As New DAL.GCLINPED

        Dim bdGCCABALB As New DAL.GCCABALB

        Dim bdGCALBARA As New DAL.GCALBARA
        Dim bdGCHISMOV As New DAL.GCHISMOV

        Dim NumPedCab As Decimal

        Dim gtkProveedor As New GertakariakLib.ELL.gtkProveedor
        Dim funProveedor As New GertakariakLib.BLL.gtkProveedorComponent
        Dim ListaGtkProveedor As New Generic.List(Of ELL.gtkProveedor)

        'Dim gtkTicket As GertakariakLib.ELL.gtkTicket = Session("gtkTicket")
        Dim tx As AccesoAutomaticoBD.TransactionMgr = AccesoAutomaticoBD.TransactionMgr.ThreadTransactionMgr
        Dim Transaccion As New XBATLib.Transaccion

        '---------------------------------------------------------------------------------------------------------------------------
        'FROGA: Log.Info estas puestos para poder trazar errores de bloqueo de la BB.DD que parecen borrar o modificar albaranes.
        '---------------------------------------------------------------------------------------------------------------------------
        Try

            If Not (IdGtk = Integer.MinValue) Then
                gtkGertakaria.Id = IdGtk
                ListaGtk = FuncGtk.Consultar(gtkGertakaria)
                gtkGertakaria = ListaGtk.Item(0)

                If gtkGertakaria.NumPedCab <> Integer.MinValue AndAlso gtkGertakaria.NumPedCab.ToString IsNot Nothing AndAlso gtkGertakaria.NumPedCab.ToString IsNot DBNull.Value Then
                    'NO Permitir modificar la Factura si no existe asiento contable.
                Else

                    Global_asax.log.Info("BeginTransaction (NC-" & gtkGertakaria.Id & ")")
                    'Inicio de Transaccion			
                    tx.BeginTransaction()
                    Select Case UCase(tipoFacturacion.SelectedValue)
                        Case UCase("SubContratacion")
                            Global_asax.log.Info("SubContratacion (Recuperacion) - INICIO")
                            'Obtenemos el siguiente identificador para la Cabecera del Pedido.
                            NumPedCab = bdGCCABPED.Secuencia

                            '-------------------------------------------------
                            'Guardamos la Cabecera del Pedido.
                            '-------------------------------------------------
                            bdGCCABPED.AddNew()
                            bdGCCABPED.NUMPEDCAB = NumPedCab
                            If Not (String.IsNullOrWhiteSpace(gtkGertakaria.IdProveedor)) Then bdGCCABPED.CODPROCAB = gtkGertakaria.IdProveedor 'Codigo de Proveedor de Troqueleria (CODPRO).

                            'bdGCCABPED.FECPEDIDO = Format(Date.Today, "dd/MM/yy")
                            bdGCCABPED.FECPEDIDO = Date.Today

                            bdGCCABPED.FECENTREG = bdGCCABPED.FECPEDIDO
                            bdGCCABPED.FECHA_EMISION = bdGCCABPED.FECPEDIDO
                            bdGCCABPED.FECLANZ = bdGCCABPED.FECPEDIDO

                            If Not gtkTicket.NombreUsuario Is Nothing Then bdGCCABPED.USUARIO_EMISION = gtkTicket.NombreUsuario 'Usuario (= SAB.Usuarios.NombreUsuario)
                            bdGCCABPED.LANGILE = Langile(gtkTicket) 'XBAT.FaPersonal.CODPER (XBAT.FaPersonal.USUARIO=UserName)

                            If gtkGertakaria.OF_Operacion.Count = 1 Then
                                bdGCCABPED.NUMORDF = gtkGertakaria.OF_Operacion.Item(0).OF
                                bdGCCABPED.NUMOPE = gtkGertakaria.OF_Operacion.Item(0).OP
                            End If

                            gtkProveedor.CodPro = gtkGertakaria.IdProveedor
                            ListaGtkProveedor = funProveedor.ConsultarListado(gtkProveedor)
                            If ListaGtkProveedor.Any Then
                                gtkProveedor = ListaGtkProveedor.Item(0)
                            End If

                            If Not gtkProveedor.CodPro Is Nothing Then
                                bdGCCABPED.CODFORPAG = gtkProveedor.FormaPago
                                bdGCCABPED.CODMONEDA = gtkProveedor.CodMoneda
                            End If

                            If gtkProveedor.TipIva <> Integer.MinValue AndAlso gtkProveedor.TipIva.ToString IsNot Nothing AndAlso gtkProveedor.TipIva.ToString IsNot DBNull.Value Then
                                bdGCCABPED.TIPIVA = gtkProveedor.TipIva
                            End If
                            bdGCCABPED.CODPUERTA = 0 'Puerta Nula (Debe Existir en XBAT.GCPUERTA) -> Lo ha dicho Bilbo.
                            bdGCCABPED.DTOPP = 0 '0-Descuento pronto pago.
                            If Not (String.IsNullOrWhiteSpace(gtkProveedor.Portes)) Then bdGCCABPED.PORTES = gtkProveedor.Portes

                            bdGCCABPED.Save()
                            '-------------------------------------------------

                            '-------------------------------------------------
                            'Guardamos la Cabecera del Albaran.
                            '-------------------------------------------------
                            bdGCCABALB.AddNew()

                            bdGCCABALB.ANNO = bdGCCABPED.FECHA_EMISION.Year
                            If Not bdGCCABPED.CODPROCAB Is Nothing Then bdGCCABALB.CODPROV = bdGCCABPED.CODPROCAB
                            If bdGCCABPED.NUMPEDCAB <> Integer.MinValue AndAlso bdGCCABPED.NUMPEDCAB.ToString IsNot Nothing AndAlso bdGCCABPED.NUMPEDCAB.ToString IsNot DBNull.Value Then
                                bdGCCABALB.NUMALBAR = bdGCCABPED.NUMPEDCAB
                            End If
                            bdGCCABALB.TIPO = "C"
                            bdGCCABALB.EPORTES = 0
                            bdGCCABALB.FECHAREC = bdGCCABPED.FECHA_EMISION
                            bdGCCABALB.DOCBATZ = "GERTAKARIAK" 'Acc. Correctivas Troqueleria
                            bdGCCABALB.NFACTU = "GTK-" & gtkGertakaria.Id
                            bdGCCABALB.FECHACON = bdGCCABALB.FECHAREC
                            bdGCCABALB.USUARIO = gtkTicket.NombreUsuario.Trim.ToUpper

                            bdGCCABALB.Save()
                            '-------------------------------------------------

                            If gtkGertakaria.LineasCoste IsNot Nothing AndAlso gtkGertakaria.LineasCoste.Any Then
                                Dim TotalCoste As Decimal
                                Dim Descuento As Decimal
                                '-------------------------------------------------
                                'Calculamos el porcentaje de descuento aplicado 
                                'para aplicarselo a cada linea.
                                '-------------------------------------------------
                                For Each gtkLineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
                                    TotalCoste = TotalCoste + gtkLineaCoste.Importe
                                Next

                                '-----------------------------------------------------------------------
                                '"TotalCoste" NO puede ser 0 si "TotalAcordado" es distinto de 0.
                                '-----------------------------------------------------------------------
                                If TotalCoste > 0 Then
                                    Descuento = 100 - ((If(gtkGertakaria.TotalAcordado Is Nothing, 0, gtkGertakaria.TotalAcordado) * 100) / TotalCoste)
                                Else
                                    Throw New ApplicationException("Total de ""Lineas de Coste"" no puede ser 0.")
                                End If
                                '-----------------------------------------------------------------------

                                '-------------------------------------------------

                                Dim iNUMLINLIN As Integer
                                For Each gtkLineaCoste As ELL.gtkLineaCoste In gtkGertakaria.LineasCoste
                                    iNUMLINLIN = iNUMLINLIN + 1 'Indicamos el Número de Línea.

                                    '-------------------------------------------------
                                    'Guardamos las Lineas del Pedido.
                                    '-------------------------------------------------
                                    bdGCLINPED.AddNew()

                                    bdGCLINPED.NUMPEDLIN = bdGCCABPED.NUMPEDCAB
                                    bdGCLINPED.NUMLINLIN = iNUMLINLIN
                                    bdGCLINPED.CODPROLIN = bdGCCABPED.CODPROCAB

                                    'Bilbo: Guardamos el importe con el descuento para que a la OF se le descuente la NC.
                                    bdGCLINPED.EIMPPED = Decimal.Negate(Math.Abs(Math.Round(gtkLineaCoste.Importe - ((gtkLineaCoste.Importe * Descuento) / 100), 2)))
                                    bdGCLINPED.EIMPREC = bdGCLINPED.EIMPPED
                                    bdGCLINPED.EIMPFAC = bdGCLINPED.EIMPPED

                                    bdGCLINPED.NUMOPE = gtkLineaCoste.NumOpe
                                    bdGCLINPED.NUMORDF = gtkLineaCoste.NumOrd
                                    If gtkLineaCoste.NumMar IsNot Nothing Then
                                        bdGCLINPED.NUMMAR = gtkLineaCoste.NumMar.Trim
                                    Else
                                        bdGCLINPED.NUMMAR = "ZZZZ"
                                    End If

                                    bdGCLINPED.CANREC = -1 'Lo ha dicho Bilbo.
                                    bdGCLINPED.CANPED = bdGCLINPED.CANREC
                                    bdGCLINPED.CANFAC = bdGCLINPED.CANREC

                                    bdGCLINPED.EDIMPRE = 1
                                    bdGCLINPED.EPREUNI = bdGCLINPED.EIMPPED / bdGCLINPED.CANREC 'Lo ha dicho Bilbo.

                                    bdGCLINPED.FECENTVIG = bdGCCABPED.FECHA_EMISION
                                    bdGCLINPED.ALEACION = 0

                                    If gtkLineaCoste.DescLinea IsNot Nothing Then bdGCLINPED.DESCART = gtkLineaCoste.DescLinea

                                    bdGCLINPED.ID_ESTADO = 10   '10 - Estado Facturado.

                                    '------------------------------------------------------------------
                                    '2013-08-07: Bilbo: No hace falta el LANTEGI.
                                    '------------------------------------------------------------------
                                    'Dim bdW_CPCABEC As New DAL.W_CPCABEC
                                    'bdW_CPCABEC.Where.NUMORD.Value = bdGCLINPED.NUMORDF
                                    'bdW_CPCABEC.Where.NUMOPE.Value = bdGCLINPED.NUMOPE
                                    'bdW_CPCABEC.Query.Load()

                                    'If Not bdW_CPCABEC.EOF Then
                                    '	bdGCLINPED.LANTEGI = bdW_CPCABEC.LANTEGI_AC
                                    '	bdGCLINPED.LANTEGI_H = bdGCLINPED.LANTEGI
                                    'Else
                                    '	Throw New ApplicationException(String.Format("La linea de coste con 'OF'-{0} y 'Operación'-{1}, no se puede tramitar por estar 'ANULADA' o 'ENVIADA'.", bdGCLINPED.NUMORDF, bdGCLINPED.NUMOPE))
                                    'End If
                                    '------------------------------------------------------------------

                                    '-------------------------------------------------------------------------
                                    'Codigo de Articulo
                                    '-------------------------------------------------------------------------
                                    'Bilbo (Solo para SubContratacion-Administracion): 
                                    'No poner el Código de Artículo Original porque falsea el STOCK de los Articulos, 
                                    'Precios Medios y Estadísticas de Compra.
                                    '-------------------------------------------------------------------------
                                    'If gtkLineaCoste.CodArt IsNot Nothing AndAlso gtkLineaCoste.CodArt <> String.Empty Then
                                    'bdGCLINPED.CODART = gtkLineaCoste.CodArt
                                    'Else
                                    bdGCLINPED.CODART = "000000000425" 'Acc. Correctivas Troqueleria
                                    'End If
                                    '-------------------------------------------------------------------------

                                    bdGCLINPED.DESCTO = 0 '2014-09-12: Lo a dicho Bilbo

                                    bdGCLINPED.Save()
                                    '--------------------------------------------------------

                                    '-------------------------------------------------
                                    'Guardamos un Historico del Pedido de Compra.
                                    '-------------------------------------------------
                                    Dim NunMov_E As Integer = Nothing
                                    Dim NunMov_S As Integer = Nothing
                                    For i = 0 To 1
                                        bdGCHISMOV.AddNew()
                                        bdGCHISMOV.NUMMOVES = bdGCHISMOV.Secuencia
                                        Select Case i
                                            Case 0
                                                NunMov_E = bdGCHISMOV.NUMMOVES
                                                bdGCHISMOV.TIPOES = "E"
                                            Case 1
                                                NunMov_S = bdGCHISMOV.NUMMOVES
                                                bdGCHISMOV.TIPOES = "S"
                                                bdGCHISMOV.NUMORDF = bdGCLINPED.NUMORDF
                                                bdGCHISMOV.NUMOPE = bdGCLINPED.NUMOPE
                                                bdGCHISMOV.MARCA = bdGCLINPED.NUMMAR
                                        End Select
                                        bdGCHISMOV.TIPOMOV = "C"
                                        bdGCHISMOV.CODART = bdGCLINPED.CODART
                                        bdGCHISMOV.CODALM = "1"
                                        bdGCHISMOV.FECMOV = bdGCCABPED.FECPEDIDO
                                        bdGCHISMOV.CANTID = bdGCLINPED.CANPED
                                        bdGCHISMOV.CODPRO = bdGCCABPED.CODPROCAB
                                        bdGCHISMOV.NUMDOCU = bdGCCABALB.NUMALBAR
                                        '------------------------------------------------
                                        '2013-08-07: Bilbo: No hace falta el LANTEGI.
                                        '------------------------------------------------
                                        'bdGCHISMOV.LANTEGI = bdGCLINPED.LANTEGI
                                        'bdGCHISMOV.LANTEGI_H = bdGCLINPED.LANTEGI_H
                                        '------------------------------------------------
                                        bdGCHISMOV.EPRECIO = bdGCLINPED.EPREUNI
                                        bdGCHISMOV.EDIMPRE = bdGCLINPED.EDIMPRE
                                        bdGCHISMOV.Save()
                                    Next
                                    '-------------------------------------------------

                                    '-------------------------------------------------
                                    'Guardamos las Lineas del Albaran.
                                    '-------------------------------------------------
                                    bdGCALBARA.AddNew()

                                    bdGCALBARA.TIPO = bdGCCABALB.TIPO
                                    bdGCALBARA.CODPROV = bdGCLINPED.CODPROLIN
                                    bdGCALBARA.ANNO = bdGCCABPED.FECHA_EMISION.Year
                                    bdGCALBARA.NUMALBAR = bdGCCABALB.NUMALBAR
                                    bdGCALBARA.NUMPED = bdGCLINPED.NUMPEDLIN
                                    bdGCALBARA.NUMLIN = bdGCLINPED.NUMLINLIN

                                    bdGCALBARA.EIMPORTE = bdGCLINPED.EIMPPED
                                    bdGCALBARA.CANREC = bdGCLINPED.CANREC

                                    bdGCALBARA.ECORTES = 0
                                    bdGCALBARA.EPORRAT = 0

                                    bdGCALBARA.NUMMOV_E = NunMov_E
                                    bdGCALBARA.NUMMOV_S = NunMov_S

                                    bdGCALBARA.Save()
                                    '-------------------------------------------------
                                Next
                            End If

                            'Guardamos el Identificador de la Cabecera del Pedido para la N.C.
                            gtkGertakaria.NumPedCab = bdGCCABPED.NUMPEDCAB
                            Global_asax.log.Info("SubContratacion (Recuperacion) - FIN")
                        Case UCase("Compras")
                            Global_asax.log.Info("Compras (Administracion) - INICIO")
                            ValidarPagina()

                            Dim LineasCoste As New List(Of GertakariakLib.ELL.gtkLineaCoste)

                            '-------------------------------------------------------------------------------------------------
                            '1.- Comprobamos que las lineas de coste con NUMPED (Numero de Pedido) sea del mismo pedido. 
                            'No tenemos en cuenta las lineas sin NUMPED porque no son reales (No salen de las lineas de pedidos de XBAT) y no se van a tener en cuenta para realizar la tramitacion.
                            '-------------------------------------------------------------------------------------------------
                            If gtkGertakaria.LineasCoste.Where(Function(l) l.NumPed <> Integer.MinValue).Select(Function(l) l.NumPed).Distinct().Count = 1 Then
                                LineasCoste = gtkGertakaria.LineasCoste.Where(Function(l) l.NumPed <> Integer.MinValue).ToList()
                            Else
                                Throw New ApplicationException("errorLineasCostePedido")
                            End If
                            'NumeroPedido = Gertakaria.LineasCoste.Where(Function(l) l.NumPed <> Integer.MinValue).Select(Function(l) l.NumPed).Distinct().SingleOrDefault


                            'Obtenemos las lineas de coste del mismo pedido ignorando las que no tengan NUMPED porque son inventadas.
                            '
                            'Gertakaria.LineasCoste.Where(Function(l) l.NumPed <> Integer.MinValue).Select(Function(l) l.NumPed).Distinct().Count = 1


                            '--------------------------------------------------------------------
                            '1º- Comprobamos que la Devolucion pertenezca a un Pedido Facturado.
                            'Si esta facturado creamos uno nuevo.
                            'Si no esta facturado "Compensamos" las lineas del Pedido.
                            '--------------------------------------------------------------------
                            Dim bPedidoFacturado As Boolean = PedidoFacturado(gtkGertakaria)
                            'If PedidoFacturado(gtkGertakaria) Then
                            'Global_asax.log.Info("PedidoFacturado (NC-" & gtkGertakaria.Id & ") - INICIO")
                            Global_asax.log.Info("Pedido Nuevo (NC-" & gtkGertakaria.Id & ") - INICIO")


                            'Obtenemos el siguiente identificador para la Cabecera del Pedido.
                            NumPedCab = bdGCCABPED.Secuencia

                            '-------------------------------------------------
                            'Guardamos la Cabecera del Pedido.
                            '-------------------------------------------------
                            bdGCCABPED.AddNew()
                            bdGCCABPED.NUMPEDCAB = NumPedCab
                            If Not (String.IsNullOrWhiteSpace(gtkGertakaria.IdProveedor)) Then bdGCCABPED.CODPROCAB = gtkGertakaria.IdProveedor 'Codigo de Proveedor de Troqueleria (CODPRO).

                            'Fecha de Entraga del Pedido
                            'Dim FechaEntrega As New DateTime
                            'DateTime.TryParse(Me.txtFechaEntrega.Text, FechaEntrega)
                            'bdGCCABPED.FECPEDIDO = Format(Date.Today, "dd/MM/yy")
                            'bdGCCABPED.FECENTREG = Format(FechaEntrega, "dd/MM/yy")
                            bdGCCABPED.FECPEDIDO = Date.Today
                            bdGCCABPED.FECENTREG = CDate(Me.txtFechaEntrega.Text) 'Fecha de Entraga del Pedido

                            bdGCCABPED.FECHA_EMISION = bdGCCABPED.FECPEDIDO
                            bdGCCABPED.FECLANZ = bdGCCABPED.FECPEDIDO

                            If Not gtkTicket.NombreUsuario Is Nothing Then bdGCCABPED.USUARIO_EMISION = gtkTicket.NombreUsuario 'Usuario (= SAB.Usuarios.NombreUsuario)

                            '--------------------------------------------------------------------------------------------
                            ''UsrGen.Nombre = gtkTicket.UserName
                            'UsrGen.Id = gtkTicket.UserId
                            'UsrGen = funUsrComp.GetUsuario(UsrGen)
                            'If UsrGen.CodPersona <> Integer.MinValue AndAlso UsrGen.CodPersona.ToString IsNot Nothing AndAlso UsrGen.CodPersona.ToString IsNot DBNull.Value Then
                            '    bdGCCABPED.LANGILE = UsrGen.CodPersona 'XBAT.FaPersonal.CODPER (XBAT.FaPersonal.USUARIO=UserName)
                            'End If
                            'bdGCCABPED.LANGILE = Langile(gtkTicket) 'XBAT.FaPersonal.CODPER (XBAT.FaPersonal.USUARIO=UserName)
                            '--------------------------------------------------------------------------------------------
                            'Nº de Trabajador igual que el pedido anterior (Solocitado por Compras).
                            '--------------------------------------------------------------------------------------------
                            If LineasCoste IsNot Nothing AndAlso LineasCoste.Any Then
                                bdGCCABPED.LANGILE = Langile(LineasCoste(0).NumPed)
                            Else
                                Throw New ApplicationException("CodigoDeTrabajador")
                            End If
                            '--------------------------------------------------------------------------------------------

                            If gtkGertakaria.OF_Operacion.Count = 1 Then
                                bdGCCABPED.NUMORDF = gtkGertakaria.OF_Operacion.Item(0).OF
                                bdGCCABPED.NUMOPE = gtkGertakaria.OF_Operacion.Item(0).OP
                            End If

                            gtkProveedor.CodPro = gtkGertakaria.IdProveedor
                            ListaGtkProveedor = funProveedor.ConsultarListado(gtkProveedor)
                            If ListaGtkProveedor.Any Then
                                gtkProveedor = ListaGtkProveedor.Item(0)
                            End If

                            If Not gtkProveedor.CodPro Is Nothing Then
                                bdGCCABPED.CODFORPAG = gtkProveedor.FormaPago
                                bdGCCABPED.CODMONEDA = gtkProveedor.CodMoneda
                            End If

                            If gtkProveedor.TipIva <> Integer.MinValue AndAlso gtkProveedor.TipIva.ToString IsNot Nothing AndAlso gtkProveedor.TipIva.ToString IsNot DBNull.Value Then
                                bdGCCABPED.TIPIVA = gtkProveedor.TipIva
                            End If
                            bdGCCABPED.CODPUERTA = 0 'Puerta Nula (Debe Existir en XBAT.GCPUERTA) -> Lo ha dicho Bilbo.
                            bdGCCABPED.DTOPP = 0 '0 - Descuento Pronto Pago.
                            bdGCCABPED.TEXTO = "GTK-" & gtkGertakaria.Id
                            If Not (String.IsNullOrWhiteSpace(gtkProveedor.Portes)) Then bdGCCABPED.PORTES = gtkProveedor.Portes

                            bdGCCABPED.Save()
                            '-------------------------------------------------

                            If LineasCoste IsNot Nothing AndAlso LineasCoste.Any Then
                                Dim iNUMLINLIN As Integer

                                For Each gtkLineaCoste As ELL.gtkLineaCoste In LineasCoste
                                    iNUMLINLIN = iNUMLINLIN + 1 'Indicamos el Número de Línea.

                                    '-------------------------------------------------
                                    'Guardamos las Lineas del Pedido.
                                    '-------------------------------------------------
                                    bdGCLINPED.AddNew()

                                    bdGCLINPED.NUMPEDLIN = bdGCCABPED.NUMPEDCAB
                                    bdGCLINPED.NUMLINLIN = iNUMLINLIN
                                    bdGCLINPED.CODPROLIN = bdGCCABPED.CODPROCAB

                                    bdGCLINPED.EIMPPED = 0 'gtkLineaCoste.Importe
                                    bdGCLINPED.EIMPREC = bdGCLINPED.EIMPPED
                                    bdGCLINPED.EIMPFAC = bdGCLINPED.EIMPPED

                                    If gtkLineaCoste.NumMar IsNot Nothing Then
                                        bdGCLINPED.NUMMAR = gtkLineaCoste.NumMar.Trim
                                    End If

                                    bdGCLINPED.NUMOPE = gtkLineaCoste.NumOpe
                                    bdGCLINPED.NUMORDF = gtkLineaCoste.NumOrd
                                    bdGCLINPED.FECENTVIG = bdGCCABPED.FECENTREG
                                    bdGCLINPED.ALEACION = 0
                                    bdGCLINPED.CANPED = gtkLineaCoste.CantidadPed
                                    bdGCLINPED.CANREC = 0
                                    bdGCLINPED.CANFAC = 0 'Pedido por Bilbo
                                    bdGCLINPED.DESCTO = 100 'Pedido por Bilbo
                                    If gtkLineaCoste.DescLinea IsNot Nothing Then bdGCLINPED.DESCART = gtkLineaCoste.DescLinea

                                    '-------------------------------------------------------------------------
                                    'Comprobamos que el Proveedor tenga acceso al recurso SEGIPE.
                                    '-------------------------------------------------------------------------
                                    Dim UsuariosComponent As New SabLib.BLL.UsuariosComponent
                                    Dim lstProveedores As New System.Collections.Generic.List(Of SabLib.ELL.Empresa)
                                    Dim RecursoAsignado As Boolean = False

                                    lstProveedores = UsuariosComponent.GetProveedoresConRecurso(ConfigurationManager.AppSettings.Item("RecursoSEGIPE"))
                                    For Each Proveedor As SabLib.ELL.Empresa In lstProveedores
                                        If Proveedor.IdTroqueleria = gtkGertakaria.IdProveedor Then
                                            RecursoAsignado = True
                                            Exit For
                                        End If
                                    Next
                                    bdGCLINPED.ID_ESTADO = If(RecursoAsignado, 6, 1) ''1-Estado Generado, 6-A vencer
                                    '-------------------------------------------------------------------------

                                    '-------------------------------------------------------------------------
                                    'Codigo de Articulo
                                    '-------------------------------------------------------------------------
                                    '2014-12-9:Lo ha dicho bilbo
                                    '-------------------------------------------------------------------------
                                    bdGCLINPED.CODART = If(bPedidoFacturado, "000000000426", "000000000427") '426-Acc. Correctivas Troqueleria Pedidos Facturados, 427-Acc. Correctivas Troqueleria Pedidos Recibidos
                                    '-------------------------------------------------------------------------

                                    '-------------------------------------------------------------------------
                                    'Recogemos el Precio Medio (EDIMPRE) y Precio Unitario (EPREUNI) del articulo.
                                    '(Bilbo)
                                    '-------------------------------------------------------------------------
                                    Dim bdGCARTICU As New DAL.GCARTICU
                                    bdGCARTICU.LoadByPrimaryKey(bdGCLINPED.CODART)

                                    bdGCLINPED.EDIMPRE = bdGCARTICU.EDIMPRE
                                    bdGCLINPED.EPREUNI = bdGCARTICU.EPREMED
                                    '-------------------------------------------------------------------------

                                    bdGCLINPED.Save()
                                    '--------------------------------------------------------
                                Next
                            End If
                            '-------------------------------------------------------------
                            'Comentarios de la NC.
                            '-------------------------------------------------------------
                            Dim bdGCCABCOM As New GertakariakLib.DAL.GCCABCOM

                            bdGCCABCOM.AddNew()
                            bdGCCABCOM.NUMPED = NumPedCab
                            bdGCCABCOM.NUMCOM = 1
                            bdGCCABCOM.COMENTA = Left(gtkGertakaria.Id & " - Nº de No Conformidad que generó este pedido.", 75)
                            bdGCCABCOM.Save()

                            bdGCCABCOM.AddNew()
                            bdGCCABCOM.NUMPED = NumPedCab
                            bdGCCABCOM.NUMCOM = 2
                            bdGCCABCOM.COMENTA = Left(LineasCoste(0).NumPed & " - Nº de pedido de compra que generó la No Conformidad.", 75)
                            bdGCCABCOM.Save()
                            '-------------------------------------------------------------
                            'Global_asax.log.Info("PedidoFacturado (NC-" & gtkGertakaria.Id & " / NumPedCab=" & NumPedCab & ") - FIN")
                            Global_asax.log.Info("Pedido Nuevo (NC-" & gtkGertakaria.Id & " / NumPedCab=" & NumPedCab & ") - FIN")
                            'Else '1º- Pedido NO Facturado. Compensamos las Lineas del Pedido.
                            'Global_asax.log.Info("PedidoFacturado - NO - INICIO")
                            'For Each LineaCoste As ELL.gtkLineaCoste In LineasCoste
                            '	If LineaCoste.NumPed <> Integer.MinValue Then
                            '		'Comprobamos que el pedido NO este bloqueado por el XBAT.
                            '		If BloqueoPedido(LineaCoste.NumPed) Then Throw New BatzException("pedidoBloqueado", New System.Exception)

                            '		bdGCLINPED.LoadByPrimaryKey(LineaCoste.NumLin, LineaCoste.NumPed)
                            '		If bdGCLINPED.RowCount = 1 Then
                            '			If LineaCoste.CantidadPed <> Nothing AndAlso BLL.Utils.integerNull(LineaCoste.CantidadPed) <> Integer.MinValue AndAlso LineaCoste.CantidadPed > 0 Then
                            '				bdGCLINPED.CANREC = bdGCLINPED.CANREC - LineaCoste.CantidadPed
                            '				If bdGCLINPED.CANREC < 0 Then
                            '					'Throw New BatzException("cantidaLineaCostePedido", New System.Exception)
                            '					Throw New ApplicationException("cantidaLineaCostePedido")
                            '				Else
                            '					bdGCLINPED.EIMPREC = (bdGCLINPED.EPREUNI / bdGCLINPED.EDIMPRE) * bdGCLINPED.CANREC
                            '				End If
                            '				bdGCLINPED.ID_ESTADO = 6 '6-Vencer(pedido por Anton Egaña), 5-Estado Aceptado por el proveedor.
                            '				bdGCLINPED.Save()

                            '				'-------------------------------------------
                            '				'Busqueda de bloqueos de Tablas.
                            '				'-------------------------------------------
                            '				Global_asax.log.Info("ModificarAlbaran - INICIO: Incidencia(" & gtkGertakaria.Id & "); LineaCoste.NumPed(" & LineaCoste.NumPed & "); LineaCoste.NumLin(" & LineaCoste.NumLin & ")")
                            '				ModificarAlbaran(LineaCoste)
                            '				Global_asax.log.Info("ModificarAlbaran - FIN")
                            '				'-------------------------------------------
                            '			Else
                            '				'Throw New BatzException("cantidaLineaCoste", New System.Exception)
                            '				Throw New ApplicationException("cantidaLineaCoste")
                            '			End If
                            '			NumPedCab = LineaCoste.NumPed
                            '		ElseIf bdGCLINPED.RowCount > 1 Then
                            '			'Throw New BatzException("Linea de Coste Duplicada en el XBAT", New System.Exception)
                            '			Throw New ApplicationException("Linea de Coste Duplicada en el XBAT")
                            '		End If
                            '	Else
                            '		'Throw New BatzException("cantidaLineaCoste", New System.Exception)
                            '		Throw New ApplicationException("cantidaLineaCoste")
                            '	End If
                            'Next

                            ''Proceso de eliminacion de los cilindros a devolver
                            'EliminacionCilindros(gtkGertakaria, Transaccion)
                            ''Proceso de eliminacion de los Bulones a devolver
                            'EliminacionBulones(gtkGertakaria, Transaccion)

                            'Global_asax.log.Info("PedidoFacturado - NO - FIN")
                            'End If

                            '----------------------------------------------------------------------
                            'Ponemos el "Gertakariak.TotalAcordado" y "LineasCoste.Importe" a 0 
                            'para que no se muestren en las graficas.
                            'Consideramos que estas N.C. no se computan para hacer las graficas.
                            '----------------------------------------------------------------------
                            If NumPedCab <> Nothing Then
                                gtkGertakaria.NumPedCab = NumPedCab
                                gtkGertakaria.TotalAcordado = 0
                                If LineasCoste IsNot Nothing AndAlso LineasCoste.Any Then
                                    For Each LineaCoste As ELL.gtkLineaCoste In LineasCoste
                                        LineaCoste.Importe = 0
                                    Next
                                End If
                            End If
                            '----------------------------------------------------------------------
                            Global_asax.log.Info("Compras (Administracion) - FIN")
                    End Select

                    tx.CommitTransaction()
                    If Transaccion.Estado IsNot Nothing AndAlso Transaccion.Estado = ConnectionState.Open Then Transaccion.Cerrar()
                    Global_asax.log.Info("CommitTransaction")

                    If ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                        '---------------------------------------------------
                        'Si las acciones estan cerradas se cierra la N.C.
                        '---------------------------------------------------
                        If AccionesCerradas(gtkGertakaria) = True Then gtkGertakaria.FechaCierre = Now
                        Global_asax.log.Info("Modificar(gtkGertakaria) - INICIO")
                        FuncGtk.Modificar(gtkGertakaria)
                        Global_asax.log.Info("Modificar(gtkGertakaria) - FIN")
                        '---------------------------------------------------

                        Global_asax.log.Info("EnviarEmail() - INICIO")
                        EnviarEmail(gtkGertakaria)
                        Global_asax.log.Info("EnviarEmail() - FIN")
                    End If
                End If
            End If
            Volver()
        Catch ex As Threading.ThreadAbortException
            'Evitamos el error "ThreadAbortException" para las redirecciones que fuerzan la finalización de la página (Response.Redirect("~", True)) y así agilizar el tiempo de carga. (ExpReg: (\W|^) Response.Redirect\("(\W|$) )
        Catch ex As ApplicationException
            tx.RollbackTransaction()
            If Transaccion.Estado IsNot Nothing AndAlso Transaccion.Estado = ConnectionState.Open Then Transaccion.Rollback()
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Global_asax.log.Error(ex.Message, ex)
            tx.RollbackTransaction()
            If Transaccion.Estado IsNot Nothing AndAlso Transaccion.Estado = ConnectionState.Open Then Transaccion.Rollback()
            Dim Excep As New Exception("CabeceraDePedido", ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub


End Class