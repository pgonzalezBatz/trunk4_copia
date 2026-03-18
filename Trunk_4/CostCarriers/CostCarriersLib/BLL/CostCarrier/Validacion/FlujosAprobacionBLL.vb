Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class FlujosAprobacionBLL

#Region "Consultas"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorIdCabecera(ByVal idCabecera As Integer) As List(Of ELL.FlujoAprobacion)
            Return DAL.FlujosAprobacionDAL.loadListByCabecera(idCabecera)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idUsuario"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorUsuario(ByVal idUsuario As Integer) As List(Of ELL.FlujoAprobacion)
            Return DAL.FlujosAprobacionDAL.loadListByUsuario(idUsuario)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorValidacionLinea(ByVal idValidacionLinea As Integer) As List(Of ELL.FlujoAprobacion)
            Return DAL.FlujosAprobacionDAL.loadListByValidacionLinea(idValidacionLinea)
        End Function

        ''' <summary>
        ''' Obtiene proyectos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function ComponerFlujoAprobacion(ByVal cabecera As ELL.CabeceraCostCarrier, ByVal validacionLinea As ELL.ValidacionLinea) As List(Of ELL.FlujoAprobacion)
            ' Vamos a obtener el project info
            Dim projectInfo As ELL.ProjectInfo = BLL.ProjectsInfoBLL.ObtenerProjectInfo(cabecera.Proyecto)

            ' Para los proyectos ECO puede darse el caso de que el flujo no sea el suyo propio
            ' - Si la company del owner es corporativo o el atributo Indicators de PTKSIS está con valor Yes  --> va por el flujo de INDUSTRIALIZATION
            ' - Si la company del owner es planta       --> va por el flujo de ECO BATZ 
            Dim tipoProyectoPtksis As String = cabecera.TipoProyPtksis
            If (cabecera.TipoProyPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ) Then
                'Comprobamos si el atributo Indicadors es Yes
                If (projectInfo.Indicator) Then
                    tipoProyectoPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION
                Else
                    'Comprobamos si el owner del corporativo
                    Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
                    Dim usuario As SabLib.ELL.Usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.NombreUsuario = projectInfo.Owner.ToLower()})

                    If (usuario.IdPlanta = 1) Then
                        Dim departamentosBLL As New SabLib.BLL.DepartamentosComponent
                        Dim departamento As SabLib.ELL.Departamento = departamentosBLL.GetDepartamento(New SabLib.ELL.Departamento With {.Id = usuario.IdDepartamento, .IdPlanta = usuario.IdPlanta})

                        If (departamento IsNot Nothing AndAlso departamento.Organizacion.Trim.ToUpper = "SISTEMAS GROUP") Then
                            tipoProyectoPtksis = ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION
                        End If
                    End If
                End If
            End If

            Dim listaFlujoAprobacion As New List(Of ELL.FlujoAprobacion)
            Dim contador As Integer = 1
            Dim usuariosRoles As New List(Of ELL.UsuarioRol)

            ' Vamos a ver si el paso es de corporativo o de planta
            Dim paso As ELL.Step = BLL.StepsBLL.Obtener(validacionLinea.IdStep)
            Dim pasoCorporativo As Boolean = (paso.IdPlantaSAB = 0)
            Select Case tipoProyectoPtksis
                Case ELL.CabeceraCostCarrier.TIPO_PROY_ECO_BATZ
                    If (pasoCorporativo) Then
                        ' Obtenemos los usuarios roles de las plantas a cargar
                        projectInfo.PlantToCharges.ForEach(Sub(s) usuariosRoles.AddRange(BLL.UsuariosRolBLL.CargarListadoPorPlanta(s.IdPlanta)))
                        ' Cogemos los responsables de ingenieria
                        For Each usuario In usuariosRoles.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta)
                            listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuario.IdSab, .Email = usuario.Email, .IdValidacionLinea = validacionLinea.Id, .Porcentaje = projectInfo.PlantToCharges.FirstOrDefault(Function(f) f.IdPlanta = usuario.IdPlanta).Porcentaje})
                            contador += 1
                        Next

                        ' Cogemos los gerentes
                        For Each usuario In usuariosRoles.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Gerente_planta)
                            listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuario.IdSab, .Email = usuario.Email, .IdValidacionLinea = validacionLinea.Id, .Porcentaje = projectInfo.PlantToCharges.FirstOrDefault(Function(f) f.IdPlanta = usuario.IdPlanta).Porcentaje})
                            contador += 1
                        Next
                    Else
                        ' Cogemos el responsable de ingenieria de la planta
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorPlanta(paso.IdPlantaSAB).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta OrElse f.IdRol = ELL.Rol.TipoRol.Gerente_planta).ToList()
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta).IdSab, .Email = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta).Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1

                        ' Cogemos el gerente de la planta
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Gerente_planta).IdSab, .Email = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Gerente_planta).Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1
                    End If
                Case ELL.CabeceraCostCarrier.TIPO_PROY_INDUSTRIALIZATION
                    If (pasoCorporativo AndAlso projectInfo.PlantToCharges.Exists(Function(f) f.PlantToCharge.ToLower.Equals("batz sistemas s coop"))) Then
                        ' Cogemos el project manager
                        'TEMPORAL: HASTA QUE NO VUELVA JOSEBA ROQUE, SE VA A SALTAR ESTE PASO
                        '************************************************************************
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Project_manager)
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1
                        '************************************************************************

                        ' Cogemos el director técnico
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Director_técnico)
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1
                    Else
                        ' Cogemos el project manager
                        'TEMPORAL: HASTA QUE NO VUELVA JOSEBA ROQUE, SE VA A SALTAR ESTE PASO
                        '************************************************************************
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Project_manager)
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1
                        '************************************************************************

                        ' Cogemos dirección de operaciones
                        'usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Direccion_operaciones)
                        'listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        'contador += 1

                        If (pasoCorporativo) Then
                            ' Obtenemos los usuarios roles de las plantas a cargar
                            projectInfo.PlantToCharges.ForEach(Sub(s) usuariosRoles.AddRange(BLL.UsuariosRolBLL.CargarListadoPorPlanta(s.IdPlanta)))
                            ' Cogemos dirección de operaciones. Cogemos solo los distintos
                            Dim directoresUsados As New List(Of Int32)
                            For Each usuario In usuariosRoles.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Direccion_operaciones).Select(Function(s) New With {.IdPlanta = s.IdPlanta, .IdSab = s.IdSab, .Email = s.Email})
                                If (Not directoresUsados.Exists(Function(f) f = usuario.IdSab)) Then
                                    listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuario.IdSab, .Email = usuario.Email, .IdValidacionLinea = validacionLinea.Id, .Porcentaje = projectInfo.PlantToCharges.FirstOrDefault(Function(f) f.IdPlanta = usuario.IdPlanta).Porcentaje})
                                    contador += 1
                                    directoresUsados.Add(usuario.IdSab)
                                End If
                            Next
                        Else
                            ' Cogemos dirección de operaciones
                            usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorPlanta(paso.IdPlantaSAB)
                            listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Direccion_operaciones).IdSab, .Email = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Direccion_operaciones).Email, .IdValidacionLinea = validacionLinea.Id})
                            contador += 1
                        End If

                        ' Eliminado del flujo desde 15/12/2021
                        ' Cogemos dirección CMP
                        'usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Direccion_CMP)
                        'listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        'contador += 1

                        ' Cogemos el director técnico
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Director_técnico)
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1

                        If (pasoCorporativo) Then
                            ' Obtenemos los usuarios roles de las plantas a cargar
                            projectInfo.PlantToCharges.ForEach(Sub(s) usuariosRoles.AddRange(BLL.UsuariosRolBLL.CargarListadoPorPlanta(s.IdPlanta)))
                            ' Cogemos los gerentes
                            For Each usuario In usuariosRoles.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Gerente_planta)
                                ' Si el director de operaciones y el gerente coinciden obviamos la validación del gerente
                                Dim directoresOp = usuariosRoles.Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Direccion_operaciones).Select(Function(s) New With {.IdPlanta = s.IdPlanta, .IdSab = s.IdSab, .Email = s.Email}).ToList()

                                If (Not directoresOp.Exists(Function(f) f.IdSab = usuario.IdSab)) Then
                                    listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuario.IdSab, .Email = usuario.Email, .IdValidacionLinea = validacionLinea.Id, .Porcentaje = projectInfo.PlantToCharges.FirstOrDefault(Function(f) f.IdPlanta = usuario.IdPlanta).Porcentaje})
                                    contador += 1
                                End If
                            Next
                        Else
                            ' Si el director de operaciones y el gerente coinciden obviamos la validación del gerente
                            usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorPlanta(paso.IdPlantaSAB)
                            Dim gerente As ELL.UsuarioRol = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Gerente_planta)
                            Dim directorOPs As ELL.UsuarioRol = usuariosRoles.FirstOrDefault(Function(f) f.IdRol = ELL.Rol.TipoRol.Direccion_operaciones)
                            If (gerente.IdSab <> directorOPs.IdSab) Then
                                ' Cogemos el gerente de la planta                            
                                listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = gerente.IdSab, .Email = gerente.Email, .IdValidacionLinea = validacionLinea.Id})
                                contador += 1
                            End If
                        End If
                    End If
                Case ELL.CabeceraCostCarrier.TIPO_PROY_PREDEVELOPMENT
                    ' Cogemos el product manager para el producto de la cabecera
                    usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Product_manager)
                    listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault(Function(f) f.ListaProductosProductManager.Contains(cabecera.Producto)).IdSab, .Email = usuariosRoles.FirstOrDefault(Function(f) f.ListaProductosProductManager.Contains(cabecera.Producto)).Email, .IdValidacionLinea = validacionLinea.Id})
                    contador += 1

                    ' Cogemos el responsable de ingenieria coporativo
                    usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Responsable_ingenieria_planta)
                    listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault(Function(f) f.IdPlanta = 0).IdSab, .Email = usuariosRoles.FirstOrDefault(Function(f) f.IdPlanta = 0).Email, .IdValidacionLinea = validacionLinea.Id})
                    contador += 1

                    ' Eliminado del flujo desde 15/12/2021
                    ' Cogemos dirección CMP
                    'usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Direccion_CMP)
                    'listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                    'contador += 1
                Case ELL.CabeceraCostCarrier.TIPO_PROY_R_D
                    If (projectInfo.Program.Trim().ToUpper().Equals("INDUSTRY_4.0")) Then
                        ' Cogemos el director de industrial
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Direccion_industrial)
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1

                        ' Cogemos dirección de operacion
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Direccion_operaciones)
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1
                    Else
                        ' Cogemos el responsable de advanced
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Responsable_advance)
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1

                        ' Cogemos el responsable de ingenieria coporativo
                        usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Responsable_ingenieria_planta)
                        listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault(Function(f) f.IdPlanta = 0).IdSab, .Email = usuariosRoles.FirstOrDefault(Function(f) f.IdPlanta = 0).Email, .IdValidacionLinea = validacionLinea.Id})
                        contador += 1

                        ' Eliminado del flujo desde 15/12/2021
                        ' Cogemos dirección CMP
                        'usuariosRoles = BLL.UsuariosRolBLL.CargarListadoPorRol(ELL.Rol.TipoRol.Direccion_CMP)
                        'listaFlujoAprobacion.Add(New ELL.FlujoAprobacion With {.Orden = contador, .IdSab = usuariosRoles.FirstOrDefault.IdSab, .Email = usuariosRoles.FirstOrDefault.Email, .IdValidacionLinea = validacionLinea.Id})
                        'contador += 1
                    End If
            End Select

            Return listaFlujoAprobacion
        End Function

#End Region

#Region "Eliminaciones"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idValidacionLinea"></param>
        Public Shared Sub Eliminar(ByVal idValidacionLinea As Integer)
            DAL.FlujosAprobacionDAL.Delete(idValidacionLinea)
        End Sub

#End Region

    End Class

End Namespace