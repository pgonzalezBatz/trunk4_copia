Imports System.Web
Imports log4net


Namespace BLL
    Public Class LoginComponent
        Implements ILoginComponent

        Private log As ILog = LogManager.GetLogger("root.SAB")

#Region "Login"

        ''' <summary>
        ''' Realiza el login con el nombre de usuario y password y devuelve el ticket del usuario. Login externo
        ''' </summary>
        ''' <param name="username">Nombre de usuario</param>
        ''' <param name="Password">password</param>
        ''' <returns>Ticket</returns>        
        Public Function Login(ByVal username As String, ByVal Password As String) As ELL.Ticket Implements ILoginComponent.Login
            Dim ticket As New ELL.Ticket()

            'Autenticar usuario
            Dim usuario As New DAL.USUARIOS
            Dim plantComp As New BLL.PlantasComponent
            usuario.Where.NOMBREUSUARIO.Value = username
            usuario.Where.NOMBREUSUARIO.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
            usuario.Where.PWD.Value = Password
            usuario.Where.PWD.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
            usuario.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
            usuario.Query.OpenParenthesis()
            usuario.Where.FECHABAJA.Value = DateTime.Now
            usuario.Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
            Dim wp As AccesoAutomaticoBD.WhereParameter = usuario.Where.TearOff.FECHABAJA
            wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
            wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
            usuario.Query.CloseParenthesis()
            Try
                usuario.Query.Load()
            Catch ex As Exception
                ticket = Nothing
                log.Error(ex)
            End Try
            If usuario.RowCount = 1 Then
                ticket = GetObjectTicket(usuario)

                log.Debug("Login Externo --> usuario:" + ticket.NombreUsuario)
            Else
                ticket = Nothing
            End If
            Return ticket
        End Function

        ''' <summary>
        ''' Realiza un login con el nombre de usuario precedido por el dominio. Login Interno
        ''' </summary>
        ''' <param name="directLoginId">Directorio activo</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Login(ByVal directLoginId As String) As ELL.Ticket Implements ILoginComponent.Login
            Dim ticket As New ELL.Ticket()
            'Autenticar usuario
            Dim usuario As New DAL.USUARIOS
            Dim plantComp As New BLL.PlantasComponent
            usuario.Where.IDDIRECTORIOACTIVO.Value = directLoginId
            usuario.Where.IDDIRECTORIOACTIVO.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
            usuario.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
            usuario.Query.OpenParenthesis()
            usuario.Where.FECHABAJA.Value = DateTime.Now
            usuario.Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
            Dim wp As AccesoAutomaticoBD.WhereParameter = usuario.Where.TearOff.FECHABAJA
            wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
            wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
            usuario.Query.CloseParenthesis()

			Try
				usuario.Query.Load()

			Catch ex As Exception
				log.Error("ex: " & ex.ToString)
				ticket = Nothing
			End Try

            If usuario.RowCount = 1 Then
                ticket = GetObjectTicket(usuario)

                log.Debug("Login Interno --> usuario:" + ticket.NombreUsuario)
            Else
                ticket = Nothing
            End If
            Return ticket
        End Function

        ''' <summary>
        ''' Realiza el login con el idTrabajador y con su password
        ''' </summary>
        ''' <param name="idTrabajador">Id del trabajador</param>
        ''' <param name="Password">Password</param>
        ''' <returns></returns>        
        Public Function Login(ByVal idTrabajador As Integer, ByVal Password As String) As ELL.Ticket Implements ILoginComponent.Login
            Dim ticket As New ELL.Ticket()

            Dim usuario As New DAL.USUARIOS
            usuario.Where.CODPERSONA.Value = idTrabajador
            usuario.Where.CODPERSONA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
            usuario.Where.PWD.Value = Password
            usuario.Where.PWD.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
            usuario.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
            usuario.Query.OpenParenthesis()
            usuario.Where.FECHABAJA.Value = DateTime.Now
            usuario.Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
            Dim wp As AccesoAutomaticoBD.WhereParameter = usuario.Where.TearOff.FECHABAJA
            wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
            wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
            usuario.Query.CloseParenthesis()
            Try
                usuario.Query.Load()
            Catch ex As Exception
                ticket = Nothing
                log.Error(ex)
            End Try
            If usuario.RowCount = 1 Then
                ticket = GetObjectTicket(usuario)

                log.Debug("Login Externo --> usuario:" + ticket.NombreUsuario)
            Else
                ticket = Nothing
            End If
            Return ticket
        End Function

        ''' <summary>
        ''' A partir de un objeto DAL, rellena un objeto ticket
        ''' </summary>
        ''' <param name="usuarioDAL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function GetObjectTicket(ByVal usuarioDAL As DAL.USUARIOS) As ELL.Ticket
            Dim myTicket As New ELL.Ticket
            Dim plantComp As New BLL.PlantasComponent
            myTicket.IdSession = HttpContext.Current.Session.SessionID
            myTicket.IdUser = usuarioDAL.ID
            myTicket.NombreUsuario = usuarioDAL.s_NOMBREUSUARIO
            myTicket.Culture = usuarioDAL.s_IDCULTURAS
            myTicket.NombrePersona = usuarioDAL.s_NOMBRE
            myTicket.Apellido1 = usuarioDAL.s_APELLIDO1
            myTicket.Apellido2 = usuarioDAL.s_APELLIDO2
            If Not (usuarioDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.CODPERSONA)) Then myTicket.IdTrabajador = usuarioDAL.CODPERSONA
            If Not (usuarioDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.IDEMPRESAS)) Then myTicket.IdEmpresa = usuarioDAL.IDEMPRESAS
            If Not (usuarioDAL.IsColumnNull(DAL.USUARIOS.ColumnNames.IDDEPARTAMENTO)) Then myTicket.IdDepartamento = usuarioDAL.IDDEPARTAMENTO
            myTicket.email = usuarioDAL.s_EMAIL
            myTicket.Dni = usuarioDAL.s_DNI

            myTicket.Plantas = plantComp.GetPlantas(usuarioDAL.ID)
            Return myTicket
        End Function

#End Region

#Region "Otras consultas"

        ''' <summary>
        ''' Recupera un ticket de base de datos y lo borra
        ''' </summary>
        ''' <param name="IdSession"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTicket(ByVal IdSession As String) As ELL.Ticket Implements ILoginComponent.getTicket
            Dim ticket As New ELL.Ticket()
            Dim ticketSAB As New DAL.TICKETS        
            Try
                ticketSAB.LoadByPrimaryKey(IdSession)
            Catch ex As Exception
                log.Warn("getTicket. " + ex.Message)
                ticket = Nothing
            End Try
            If ticketSAB.RowCount = 1 Then
                'Autenticado
                Dim usuario As New DAL.USUARIOS
                Try
                    usuario.LoadByPrimaryKey(ticketSAB.IDUSUARIOS)
                    If (usuario.RowCount = 1) Then
                        ticket = GetObjectTicket(usuario)

                        'Borrar el ticket de la base de datos
                        ticketSAB.MarkAsDeleted()
                        ticketSAB.Save()
                    Else
                        log.Warn("Usuario no encontrado (" & ticketSAB.IDUSUARIOS & ")")
                    End If
                Catch ex As Exception
                    log.Warn("Ha podido quedar algun ticket colgando en la base de datos", ex)
                    ticketSAB.LoadAll()
                    ticketSAB.MarkAsDeleted()
                    ticketSAB.Save()
                    ticket = Nothing
                End Try
            Else
                log.Warn("getTicket. IdSession=" + IdSession + ", No se ha podido encontrar ticket en la BD")
                ticket = Nothing
            End If
            Return ticket
        End Function

        ''' <summary>
        ''' Comprueba si el usuario,tiene acceso al recurso
        ''' </summary>
        ''' <param name="ticket">Ticket con los datos del usuario</param>
        ''' <param name="recurso">Recurso al que se quiere acceder</param>
        ''' <returns>Booleano</returns>        
        Public Function AccesoRecursoValido(ByVal ticket As ELL.Ticket, ByVal recurso As Integer) As Boolean Implements ILoginComponent.AccesoRecursoValido
            Dim acceso As Boolean = False
            If Not ticket Is Nothing Then
                Dim usuarioRecursos As New DAL.Views.W_RECURSOS_USUARIO()
                usuarioRecursos.Where.IDUSUARIO.Value = ticket.IdUser
                usuarioRecursos.Where.IDUSUARIO.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
                usuarioRecursos.Where.IDCULTURA.Value = ticket.Culture
                usuarioRecursos.Where.IDCULTURA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
                usuarioRecursos.Query.Load()
                If usuarioRecursos.RowCount > 0 Then
                    Do
                        If usuarioRecursos.IDRECURSO = recurso Then
                            Exit Do
                        End If
                    Loop While usuarioRecursos.MoveNext()
                    If usuarioRecursos.IDRECURSO = recurso Then
                        acceso = True
                    End If
                End If
            End If
            
            Return acceso
        End Function

#End Region

#Region "Set Ticket"


        ''' <summary>
        ''' Guarda en base de datos, el nuevo ticket
        ''' </summary>
        ''' <param name="ticket"></param>
        ''' <returns></returns>
        Public Function SetTicketEnBD(ByVal ticket As ELL.Ticket) As Boolean Implements ILoginComponent.SetTicketEnBD
            Dim dbTicket As New DAL.TICKETS

            Try
                dbTicket.LoadByPrimaryKey(ticket.IdSession)
                If dbTicket.RowCount = 1 Then
                    dbTicket.MarkAsDeleted()
                    dbTicket.Save()
                End If
            Catch ex As Exception
                log.Error("Error em la BD.")
                Return False
            End Try

            Try
                dbTicket.AddNew()
                dbTicket.ID = ticket.IdSession
                dbTicket.IDUSUARIOS = ticket.IdUser
                dbTicket.Save()
            Catch ex As Exception
                'El ticket ya existe
                log.Error("SetTicketEnBD. Error al guardar ticket en la base de datos. " + ex.Message)
                Return False
            End Try
            Return True
        End Function

#End Region

    End Class
End Namespace