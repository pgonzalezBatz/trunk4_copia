Imports AutomntoLib

Friend Class Main

    ''' <summary>
    ''' Se lanza el formulario con los puntos de los grupos que vienen como parametros
    ''' </summary>    
    <STAThread()> _
    Shared Sub Main()
        Try
            Modulo.Equipo = Environment.MachineName.ToLower
            'TEST:Modulo.Equipo = "ltabelgarcia"
            Modulo.SetServidorComunicacion(1)
            Modulo.ConfigureLog4net()
            Dim maqBLL As New BLL.MaquinaBLL
            Dim automntoBLL As New BLL.AutomntoBLL
            Dim servidor As String = String.Empty
            Try
                Dim params As String() = Environment.GetCommandLineArgs  'El primer parametro es el nombre del ejecutable, el segundo el tipo, el tercero el identificador del grupo
                '******PRUEBAS*******                
                'Dim params As String() = {"", AutomntoLib.ELL.Clases.HojaCab.TipoHoja.Automantenimiento, 4} 'Grupos - Troqueleria
                'Dim params As String() = {"", AutomntoLib.ELL.Clases.HojaCab.TipoHoja.Automantenimiento, 374} 'Grupos - Sistemas
                'Dim params As String() = {"", AutomntoLib.ELL.Clases.HojaCab.TipoHoja.Inicio_produccion, "265,266"} 'Inicio de produccion - Sistemas
                'Dim params As String() = {"", AutomntoLib.ELL.Clases.HojaCab.TipoHoja.Auditoria, "428,429"} 'Auditoria - Sistemas
                '********************                
                Modulo.Tipo = CType(params(1), ELL.Clases.HojaCab.TipoHoja)
                Modulo.IdPlanta = 0 'Se inicializa pero luego se obtendra del grupo
                Dim stringGrupos As String = String.Empty
                Dim sIdGrupos As String() = params(2).Split(",")
                Modulo.IdGrupos = New List(Of Integer)
                For Each idGrup As String In sIdGrupos
                    stringGrupos &= If(stringGrupos <> String.Empty, ",", "") & idGrup
                    Modulo.IdGrupos.Add(idGrup)
                    If (Modulo.IdPlanta = 0) Then
                        Dim grupBLL As New BLL.GrupoBLL
                        Modulo.IdPlanta = grupBLL.consultar(idGrup).IdPlanta
                    End If
                Next
                log.Info("Recogida de parametros de " & [Enum].GetName(GetType(ELL.Clases.HojaCab.TipoHoja), Modulo.Tipo).Replace("_", " ") & ". Ejecucion para los grupos " & params(2) & " de la planta " & Modulo.IdPlanta)
                If (Modulo.IdPlanta = 0) Then
                    log.Warn("No se ha podido obtener el idPlanta del grupo asi que se para la ejecucion")
                    Application.Exit()
                    Exit Sub
                End If
                If (Modulo.Tipo <> ELL.Clases.HojaCab.TipoHoja.Automantenimiento And Modulo.Tipo <> ELL.Clases.HojaCab.TipoHoja.Inicio_produccion_automatico And Modulo.Tipo <> ELL.Clases.HojaCab.TipoHoja.Auditoria) Then
                    log.Warn("Recogida de parametros desconocido. El tipo " & Modulo.Tipo & " no esta contemplado. Se para la ejecucion")
                    Application.Exit()
                    Exit Sub
                End If
                Dim loginBLL As New SabLib.BLL.LoginComponent
                If (Modulo.IdPlanta <> 1) Then Modulo.SetServidorComunicacion(Modulo.IdPlanta)
                Modulo.Ticket = loginBLL.Login(System.Security.Principal.WindowsIdentity.GetCurrent.Name.ToLower)
                Dim cultura As String = "es-ES"
                If (Modulo.Ticket IsNot Nothing) Then cultura = Modulo.Ticket.Culture
                Application.CurrentCulture = New System.Globalization.CultureInfo(cultura)  'Se asigna la cultura del ticket a la aplicacion
                Dim paramBLL As New BLL.ParametroBLL
                Dim oParam As ELL.Clases.Parametros = paramBLL.consultar(Modulo.IdPlanta)
                If (oParam IsNot Nothing) Then servidor = oParam.Servidor
            Catch ex As Exception
                log.Error("Error al obtener los parametros o al aplicar la cultura del usuario logeado en el equipo", ex)
                Application.Exit()
                Exit Sub
            End Try
            If (Modulo.Tipo = ELL.Clases.HojaCab.TipoHoja.Auditoria) Then
                Modulo.Negocio = ELL.Clases.Perfil.RolesUnNeg.Sistemas
                automntoBLL.UpdateComunicacion(New ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = Modulo.IdPlanta, .FechaInicio = Now, .Servidor = servidor})
                frmAvisoAuditoria.ShowDialog()  'Para que se lance en modal
            Else
                'Se ejecuta con el primer grupo que existe. Si hubiera mas grupos, al pulsar "Salir" en la pagina de resultado, se continuara con el resto de grupos
                Dim grupBLL As New BLL.GrupoBLL
                Modulo.IdGrupoEjecucion = Modulo.IdGrupos.First
                Dim oGrup As ELL.Clases.Grupo = grupBLL.consultar(Modulo.IdGrupoEjecucion)
                'Se buscan todas las que tengan el nombre del equipo, porque pueden existir mas de una para un mismo equipo
                Dim lMaq As List(Of ELL.Clases.Maquina) = maqBLL.consultarListado(New ELL.Clases.Maquina With {.PC = Modulo.Equipo, .IdPlanta = Modulo.IdPlanta})
                If (lMaq Is Nothing Or (lMaq IsNot Nothing AndAlso lMaq.Count = 0)) Then
                    log.Warn("No se ha encontrado ninguna maquina asociada al equipo " & Modulo.Equipo)
                    Application.Exit()
                    Exit Sub
                Else
                    Dim oMaq As ELL.Clases.Maquina = lMaq.Find(Function(o As ELL.Clases.Maquina) o.Id = oGrup.IdMaquina)
                    If (oMaq Is Nothing) Then
                        log.Warn("No se ha encontrado la maquina asociada al grupo " & oGrup.Id)
                        Application.Exit()
                        Exit Sub
                    Else
                        log.Info("Se ejecuta el grupo " & Modulo.IdGrupoEjecucion)
                        Modulo.IdMaquina = oMaq.Id
                        Modulo.DescMaquina = oMaq.DescripcionCompleta
                        Modulo.Negocio = oMaq.Maq_UnidadNegocio
                        automntoBLL.UpdateComunicacion(New ELL.Clases.ComunicacionEquipo With {.Equipo = Equipo, .Aplicacion = ELL.Clases.ComunicacionEquipo.Aplication.Formulario, .IdPlanta = IdPlanta, .FechaInicio = Now, .Servidor = servidor})
                        Dim frmLogin As New frmLogin
                        frmLogin.ShowDialog()  'Para que se lance en modal
                    End If
                End If
            End If
        Catch ex As Exception
            log.Error("Error en el main", ex)
            Application.Exit()
        End Try
    End Sub

End Class
