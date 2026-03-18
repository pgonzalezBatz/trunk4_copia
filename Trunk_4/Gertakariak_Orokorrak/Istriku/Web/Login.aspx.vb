Public Class Login
	Inherits PageBase

#Region "Propiedades"
	''' <summary>
	''' Entidades de la base de datos.
	''' </summary>
	''' <remarks></remarks>
    Dim BBDD As New BatzBBDD.Entities_Gertakariak

    Property Propiedades_gvSucesos() As gtkGridView
        Get
            If (Session("Propiedades_gvSucesos") Is Nothing) Then Session("Propiedades_gvSucesos") = New gtkGridView
            Return CType(Session("Propiedades_gvSucesos"), gtkGridView)
        End Get
        Set(value As gtkGridView)
            Session("Propiedades_gvSucesos") = value
        End Set
    End Property
#End Region

#Region "Eventos de Pagina"
    Private Sub Page_PreInit(sender As Object, e As EventArgs) Handles Me.PreInit
        Propiedades_gvSucesos.IdSeleccionadoIstriku = If(Request("idIncidencia") Is Nothing, New Integer?, CInt(Request("idIncidencia")))
    End Sub
#End Region

#Region "Eventos de Objetos"
    Private Sub imgAceptar_Click(sender As Object, e As ImageClickEventArgs) Handles imgAceptar.Click
        Try
            Dim lg As New SabLib.BLL.LoginComponent
            Dim TicketTrabajador As SabLib.ELL.Ticket = lg.Login(txtNumTrabajador.Text, SabLib.BLL.Utils.EncriptarPassword(txtClave.Text), 1)

#If DEBUG Then
            TicketTrabajador = Session("Ticket")
#End If
            Dim lPerfiles As IEnumerable(Of Integer) = From p As [Enum] In [Enum].GetValues(GetType(Perfil)) Select CInt(p.GetHashCode)
            If TicketTrabajador Is Nothing Then
                PerfilUsuario = Nothing
                Throw New ApplicationException("Numero de trabajador o Clave incorrecta")
            Else
                Ticket = TicketTrabajador
                InitializeCulture()
                FiltroGTK.TipoIncidencia = My.Settings.IdTipoIncidencia

                Dim lIDGRUPO As IQueryable(Of Decimal) = From Reg As BatzBBDD.USUARIOSGRUPOS In BBDD.USUARIOSGRUPOS
                                                         Where lPerfiles.Contains(Reg.IDGRUPO) _
                                                         And Reg.IDUSUARIO = Ticket.IdUser _
                                                         And Reg.IDTIPOINCIDENCIA = FiltroGTK.TipoIncidencia
                                                         Select Reg.IDGRUPO
                'Where (Reg.IDGRUPO = Perfil.Administrador Or Reg.IDGRUPO = Perfil.Usuario Or Reg.IDGRUPO = Perfil.Consultor) _
                PerfilUsuario = If(lIDGRUPO.Any, CType(lIDGRUPO.FirstOrDefault, Nullable(Of Perfil)), New Nullable(Of Perfil))
            End If

            '#If DEBUG Then
            '            Response.Redirect("~/Informe/Formulario.aspx", True)
            '#End If

            If Propiedades_gvSucesos Is Nothing OrElse Propiedades_gvSucesos.IdSeleccionadoIstriku Is Nothing Then
                Response.Redirect("~/Default.aspx", False)
            Else
                Response.Redirect("~/Informe/Detalle.aspx", False)
                Log.Debug("Propiedades_gvSucesos.IdSeleccionadoIstriku: " & Propiedades_gvSucesos.IdSeleccionadoIstriku)
            End If
        Catch ex As ApplicationException
            Master.ascx_Mensajes.MensajeError(ex)
        Catch ex As Exception
            Log.Error(ex)
            Master.ascx_Mensajes.MensajeError(ex)
        End Try
    End Sub
#End Region
End Class