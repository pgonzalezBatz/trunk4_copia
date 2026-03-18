Imports System.Web.Script.Serialization
Imports CostCarriersLib.DAL

Namespace BLL

    ''' <summary>
    ''' 
    ''' </summary>
    Public Class UsuariosRolBLL

#Region "Consultas"

        '''' <summary>
        '''' Obtiene un usuario
        '''' </summary>
        '''' <param name="idUsuarioRol"></param>
        '''' <returns></returns>
        'Public Shared Function Obtener(ByVal idUsuarioRol As Integer) As ELL.UsuarioRol
        '    Return UsuariosRolDAL.getUsuarioRol(idUsuarioRol)
        'End Function

        '''' <summary>
        '''' Obtiene un usuario
        '''' </summary>
        '''' <param name="idRol"></param>
        '''' <param name="idSab"></param>
        '''' <param name="idPlanta"></param>
        '''' <returns></returns>
        'Public Shared Function Obtener(ByVal idRol As Integer, ByVal idSab As Integer, ByVal idPlanta As Integer) As ELL.UsuarioRol
        '    Return UsuariosRolDAL.getUsuarioRol(idRol, idSab, idPlanta)
        'End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idSab"></param>
        ''' <returns></returns>
        Public Shared Function CargarListado(ByVal idSab As Integer) As List(Of ELL.UsuarioRol)
            Dim roles As List(Of ELL.UsuarioRol) = UsuariosRolDAL.loadList(idSab)

            '******** Vamos a obtener los roles que no vienen de la propia aplicación. Product manager de Bonosis y Gerentes de Viajes ***********

            Dim jss As New JavaScriptSerializer()
            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                Dim usuario As SabLib.ELL.Usuario
                Dim empresasGerente As List(Of Integer) = jss.Deserialize(Of List(Of Integer))(cliente.GetFactoriesIsGerente(idSab))

                If (empresasGerente IsNot Nothing AndAlso empresasGerente.Count > 0) Then
                    'Si en la lista de roles de usuario ya hay uno cogemos el primero para tener los datos del usuario
                    'Si no hay accedemos a sab para obtener los datos de dicho usuario
                    If (roles IsNot Nothing AndAlso roles.Count > 0) Then
                        usuario = New SabLib.ELL.Usuario With {.Id = roles.First.IdSab, .Apellido1 = roles.First.Apellido1, .Apellido2 = roles.First.Apellido2, .Nombre = roles.First.Nombre, .Email = roles.First.Email}
                    Else
                        Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
                        usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idSab})
                    End If

                    If (usuario IsNot Nothing) Then
                        empresasGerente.ForEach(Sub(s) roles.Add(New ELL.UsuarioRol With {.IdSab = usuario.Id, .Apellido1 = usuario.Apellido1, .Apellido2 = usuario.Apellido2, .Nombre = usuario.Nombre, .Email = usuario.Email, .IdRol = ELL.Rol.TipoRol.Gerente_planta, .IdPlanta = s}))
                    End If
                End If

                    Dim productosProductManager As List(Of String) = jss.Deserialize(Of List(Of String))(cliente.GetProductsIsProductManager(idSab))

                If (productosProductManager IsNot Nothing AndAlso productosProductManager.Count > 0) Then
                    'Si en la lista de roles de usuario ya hay uno cogemos el primero para tener los datos del usuario
                    'Si no hay accedemos a sab para obtener los datos de dicho usuario

                    If (roles IsNot Nothing AndAlso roles.Count > 0) Then
                        usuario = New SabLib.ELL.Usuario With {.Id = roles.First.IdSab, .Apellido1 = roles.First.Apellido1, .Apellido2 = roles.First.Apellido2, .Nombre = roles.First.Nombre, .Email = roles.First.Email}
                    Else
                        Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
                        usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idSab})
                    End If

                    ' Product manager siempre va asociado a la planta de Corporativo
                    If (usuario IsNot Nothing) Then
                        roles.Add(New ELL.UsuarioRol With {.IdSab = usuario.Id, .Apellido1 = usuario.Apellido1, .Apellido2 = usuario.Apellido2, .Nombre = usuario.Nombre, .Email = usuario.Email, .IdRol = ELL.Rol.TipoRol.Product_manager, .IdPlanta = 0, .ListaProductosProductManager = productosProductManager})
                    End If
                End If
            End Using

            '************************************************************************************************************************************

            Return roles
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idPlantaSab"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorPlanta(ByVal idPlantaSab As Integer) As List(Of ELL.UsuarioRol)
            Dim roles As List(Of ELL.UsuarioRol) = UsuariosRolDAL.loadListByIdPlanta(idPlantaSab)
            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim usuario As SabLib.ELL.Usuario

            '******** Vamos a obtener los roles que no vienen de la propia aplicación. Product manager de Bonosis y Gerentes de Viajes ***********
            Dim jss As New JavaScriptSerializer()
            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers
                Try
                    Dim idGerente As Integer = jss.Deserialize(Of Integer)(cliente.GetGerente(idPlantaSab))
                    usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = idGerente})
                    If (usuario IsNot Nothing) Then
                        roles.Add(New ELL.UsuarioRol With {.IdSab = usuario.Id, .Apellido1 = usuario.Apellido1, .Apellido2 = usuario.Apellido2, .Nombre = usuario.Nombre, .Email = usuario.Email, .IdRol = ELL.Rol.TipoRol.Gerente_planta, .IdPlanta = idPlantaSab})
                    End If
                Catch
                End Try

                Dim productManagers As List(Of ELL.ProductManager) = jss.Deserialize(Of List(Of ELL.ProductManager))(cliente.GetProductManagers(Nothing))
                For Each productManager In productManagers
                    usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = productManager.IdUser})

                    ' Product manager siempre va asociado a la planta de Corporativo
                    If (usuario IsNot Nothing) Then
                        roles.Add(New ELL.UsuarioRol With {.IdSab = usuario.Id, .Apellido1 = usuario.Apellido1, .Apellido2 = usuario.Apellido2, .Nombre = usuario.Nombre, .Email = usuario.Email, .IdRol = ELL.Rol.TipoRol.Product_manager, .IdPlanta = 0, .ListaProductosProductManager = productManager.Producto})
                    End If
                Next

            End Using
            '************************************************************************************************************************************

            Return roles
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idRol"></param>
        ''' <returns></returns>
        Public Shared Function CargarListadoPorRol(ByVal idRol As Integer) As List(Of ELL.UsuarioRol)
            Dim roles As List(Of ELL.UsuarioRol) = UsuariosRolDAL.loadListByIdRol(idRol)
            Dim usuariosBLL As New SabLib.BLL.UsuariosComponent
            Dim usuario As SabLib.ELL.Usuario

            '******** Vamos a obtener los roles que no vienen de la propia aplicación. Product manager de Bonosis y Gerentes de Viajes ***********
            Dim jss As New JavaScriptSerializer()
            Using cliente As New ServicioOfertaTecnica.ServiceCostCarriers

                If (idRol = ELL.Rol.TipoRol.Gerente_planta) Then
                    Dim gerentes As List(Of ELL.Gerente) = jss.Deserialize(Of List(Of ELL.Gerente))(cliente.GetGerentes())

                    For Each gerente In gerentes
                        usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = gerente.IdUser})

                        If (usuario IsNot Nothing) Then
                            roles.Add(New ELL.UsuarioRol With {.IdSab = usuario.Id, .Apellido1 = usuario.Apellido1, .Apellido2 = usuario.Apellido2, .Nombre = usuario.Nombre, .Email = usuario.Email, .IdRol = ELL.Rol.TipoRol.Gerente_planta, .IdPlanta = gerente.IdPlanta})
                        End If
                    Next
                End If

                If (idRol = ELL.Rol.TipoRol.Product_manager) Then
                    Dim productManagers As List(Of ELL.ProductManager) = jss.Deserialize(Of List(Of ELL.ProductManager))(cliente.GetProductManagers(Nothing))
                    For Each productManager In productManagers
                        usuario = usuariosBLL.GetUsuario(New SabLib.ELL.Usuario With {.Id = productManager.IdUser})

                        ' Product manager siempre va asociado a la planta de Corporativo
                        If (usuario IsNot Nothing) Then
                            roles.Add(New ELL.UsuarioRol With {.IdSab = usuario.Id, .Apellido1 = usuario.Apellido1, .Apellido2 = usuario.Apellido2, .Nombre = usuario.Nombre, .Email = usuario.Email, .IdRol = ELL.Rol.TipoRol.Product_manager, .IdPlanta = 0, .ListaProductosProductManager = productManager.Producto})
                        End If
                    Next
                End If

            End Using
            '************************************************************************************************************************************

            Return roles
        End Function

        '''' <summary>
        '''' Comprueba si existe un usuario rol
        '''' </summary>
        '''' <param name="idRol">Id del rol</param>
        '''' <param name="idSab">Id del usuario</param>
        '''' <param name="idPlanta">Id planta</param> 
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Shared Function Existe(ByVal idRol As Integer, ByVal idSab As String, ByVal idPlanta As Integer) As Boolean
        '    Return UsuariosRolDAL.existsUsuarioRol(idRol, idSab, idPlanta)
        'End Function

        '''' <summary>
        '''' Comprueba si existe una planta
        '''' </summary>
        '''' <param name="idPlanta"></param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Shared Function ExistePlanta(ByVal idPlanta As Integer) As Boolean
        '    Return UsuariosRolDAL.existsPlanta(idPlanta)
        'End Function

#End Region

#Region "Modificaciones"

        '''' <summary>
        '''' Guarda un usuario rol
        '''' </summary>
        '''' <param name="usuarioRol">Usuario rol</param>
        '''' <param name="idRecurso"></param>
        '''' <remarks></remarks>
        'Public Shared Sub Guardar(ByVal usuarioRol As ELL.UsuarioRol, ByVal idRecurso As Integer)
        '    Dim recursosBLL As New SabLib.BLL.RecursosComponent()
        '    recursosBLL.AddUsuario(usuarioRol.IdSab, idRecurso)

        '    UsuariosRolDAL.SaveUsuarioRol(usuarioRol)
        'End Sub

#End Region

#Region "Eliminaciones"

        '''' <summary>
        '''' Elimina un usuario rol
        '''' </summary>
        '''' <param name="id"></param>
        '''' <param name="idRecurso"></param>
        '''' <remarks></remarks>
        'Public Shared Sub Eliminar(ByVal id As Integer, ByVal idRecurso As Integer)
        '    ' Obtenemos el usuario/rol
        '    Dim usuarioRol As ELL.UsuarioRol = BLL.UsuariosRolBLL.Obtener(id)

        '    Dim listaUsuarioRol As List(Of ELL.UsuarioRol) = UsuariosRolDAL.loadList(usuarioRol.IdSab, Nothing)

        '    ' Si sólo tiene un rol quiere decir que es el que vamos a eliminar con lo cual eliminamos su vinculación al recurso
        '    If (listaUsuarioRol.Count = 1) Then
        '        Dim recursosBLL As New SabLib.BLL.RecursosComponent()
        '        recursosBLL.DeleteUsuario(usuarioRol.IdSab, idRecurso)
        '    End If

        '    UsuariosRolDAL.DeleteUsuarioRol(id)
        'End Sub

#End Region

    End Class

End Namespace