Imports TelefoniaLib

Namespace BLL
    Public Class UsuariosComponent


        ''' <summary>
        ''' Obtiene todos los usuarios de una planta
        ''' </summary>
        ''' <returns>Lista de objetos usuario</returns>
        Public Function getUsuarios(ByVal idPlanta As Integer, Optional ByVal idDept As String = "") As List(Of SABLib.ELL.Usuario)
            Dim ouser As New SABLib.ELL.Usuario
            Dim lUsers As List(Of SABLib.ELL.Usuario)
            Dim lPlantas As New List(Of Integer)
            Try
                If (idDept <> String.Empty) Then ouser.IdDepartamento = idDept
                ouser.IdPlanta = idPlanta
                lUsers = GetUsuariosPlanta(ouser)

                'Se eliminan los que no tengan nombre
                For i As Integer = lUsers.Count - 1 To 0 Step -1
                    ouser = lUsers.Item(i)
                    If (ouser.NombreCompleto = String.Empty) Then
                        lUsers.RemoveAt(i)
                    End If
                Next

                Return lUsers
            Catch ex As Exception
                Throw New BatzException("errIKSobtenerUsuarios", ex)
            End Try

        End Function

        ''' <summary>
        ''' Dependiendo si es de Matrici o no, obtiene unos usuarios u otros
        ''' </summary>
        ''' <param name="oUser"></param>
        ''' <returns></returns>
        Private Function GetUsuariosPlanta(ByVal oUser As Sablib.ELL.Usuario)
            Dim lUsers As New List(Of Sablib.ELL.Usuario)
            Dim userComp As New Sablib.BLL.UsuariosComponent
            If (oUser.IdPlanta = ELL.Matrici.MATRICI_ID_PLANTA) Then
                Dim matricBLL As New BLL.MatriciComponent
                lUsers = matricBLL.GetUsuariosMatrici()
            Else
                lUsers = userComp.GetUsuariosPlanta(oUser, bUsuariosConDichaPlantaGestion:=True)
            End If

            Return lUsers
        End Function

        ''' <summary>
        ''' Obtiene todos los plantas que puede administrar un usuario
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>        
        ''' <returns>Lista de objetos planta</returns>
        Public Function getPlantasAdministrador(ByVal idUsuario As Integer) As List(Of SabLib.ELL.Planta)
            Dim plantComp As New SabLib.BLL.PlantasComponent
            Dim adminPlantDAL As New DAL.ADMINISTRADORES_PLANTA
            Dim lPlantas As New List(Of SabLib.ELL.Planta)
            Dim oPlanta As SabLib.ELL.Planta
            Try
                adminPlantDAL.Where.ID_USUARIO.Value = idUsuario
                adminPlantDAL.Query.Load()
                If (adminPlantDAL.RowCount > 0) Then
                    Do
                        oPlanta = plantComp.GetPlanta(adminPlantDAL.ID_PLANTA)
                        lPlantas.Add(oPlanta)
                    Loop While adminPlantDAL.MoveNext
                End If
                Return lPlantas
            Catch ex As Exception
                Throw New BatzException("errCargarPlantas", ex)
            End Try

        End Function

        ''' <summary>
        ''' Obtiene todos los plantas que puede administrar un usuario
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="superAdm">Indicara si es superadministrador. En ese caso, se devolveran todas las plantas</param>
        ''' <returns>Lista de objetos planta</returns>
        Public Function getPlantasAdministrador(ByVal idUsuario As Integer, ByVal superAdm As Boolean) As List(Of SABLib.ELL.Planta)
            Dim plantComp As New SABLib.BLL.PlantasComponent
            Dim adminPlantDAL As New DAL.ADMINISTRADORES_PLANTA
            Dim lPlantas As New List(Of SABLib.ELL.Planta)
            Dim oPlanta As SABLib.ELL.Planta
            Try
                If Not (superAdm) Then
                    adminPlantDAL.Where.ID_USUARIO.Value = idUsuario
                    adminPlantDAL.Query.Load()
                    If (adminPlantDAL.RowCount > 0) Then
                        Do
                            oPlanta = plantComp.GetPlanta(adminPlantDAL.ID_PLANTA)
                            lPlantas.Add(oPlanta)
                        Loop While adminPlantDAL.MoveNext
                    End If
                Else
                    lPlantas = plantComp.GetPlantas()
                End If

                Return lPlantas
            Catch ex As Exception
                Throw New BatzException("errCargarPlantas", ex)
            End Try

        End Function

        ''' <summary>
        ''' Obtiene todos los plantas en las que puede gestionar moviles un usuario
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>        
        ''' <returns>Lista de objetos planta</returns>
        Public Function getPlantasGestion(ByVal idUsuario As Integer) As List(Of SABLib.ELL.Planta)
            Dim plantComp As New SABLib.BLL.PlantasComponent
            Dim gestorDAL As New DAL.GESTOR_TLFNOS
            Dim lPlantas As New List(Of SABLib.ELL.Planta)
            Dim oPlanta As SABLib.ELL.Planta
            Try
                gestorDAL.Where.ID_GESTOR.Value = idUsuario
                gestorDAL.Query.Load()
                If (gestorDAL.RowCount > 0) Then
                    Do
                        oPlanta = plantComp.GetPlanta(gestorDAL.ID_PLANTA)
                        lPlantas.Add(oPlanta)
                    Loop While gestorDAL.MoveNext
                End If

                Return lPlantas
            Catch ex As Exception
                Throw New BatzException("errCargarPlantas", ex)
            End Try

        End Function

        ''' <summary>
        ''' Obtiene los administradores de una planta
        ''' </summary>
        ''' <param name="idPlanta">Planta de la que se obtendran los administradores</param>
        ''' <returns>Lista de objetos usuario</returns>
        Public Function getAdministradoresPlanta(ByVal idPlanta As Integer) As List(Of SABLib.ELL.Usuario)
            Dim adminPlantDAL As New DAL.ADMINISTRADORES_PLANTA
            Dim userComp As New SABLib.BLL.UsuariosComponent
            Dim listAdm As New List(Of SABLib.ELL.Usuario)
            Dim oUsu As SABLib.ELL.Usuario
            Try
                adminPlantDAL.Where.ID_PLANTA.Value = idPlanta
                adminPlantDAL.Query.Load()
                If (adminPlantDAL.RowCount > 0) Then
                    Do
                        oUsu = New SABLib.ELL.Usuario
                        oUsu.Id = adminPlantDAL.ID_USUARIO
                        oUsu = userComp.GetUsuario(oUsu, False)
                        listAdm.Add(oUsu)
                    Loop While adminPlantDAL.MoveNext
                End If

                Return listAdm
            Catch ex As Exception
                Throw New BatzException("errIKSobtenerAdministradores", ex)
            End Try

        End Function


        ''' <summary>
        ''' Añade un administrador a una planta
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <returns>Booleano indicando si la operacion se ha realizado correctamente</returns>
        ''' <remarks></remarks>
        Public Function AddAdministradorPlanta(ByVal idUsuario As Integer, ByVal idPlanta As Integer) As Boolean
            Dim adminPlantDAL As New DAL.ADMINISTRADORES_PLANTA
            Try
                adminPlantDAL.AddNew()
                adminPlantDAL.ID_USUARIO = idUsuario
                adminPlantDAL.ID_PLANTA = idPlanta
                adminPlantDAL.Save()
                Return True
            Catch ex As Exception
                Throw New BatzException("errIKSañadirAdministrador", ex)
            End Try

        End Function


        ''' <summary>
        ''' Elimina un administrador de una planta
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <returns>Booleano indicando si la operacion se ha realizado correctamente</returns>
        Public Function DeleteAdministradorPlanta(ByVal idUsuario As Integer, ByVal idPlanta As Integer) As Boolean
            Dim adminPlantDAL As New DAL.ADMINISTRADORES_PLANTA
            Try
                adminPlantDAL.Where.ID_USUARIO.Value = idUsuario
                adminPlantDAL.Where.ID_PLANTA.Value = idPlanta
                adminPlantDAL.Query.Load()
                If (adminPlantDAL.RowCount = 1) Then
                    adminPlantDAL.MarkAsDeleted()
                    adminPlantDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errIKSborrarAdministrador", ex)
            End Try
        End Function


        ''' <summary>
        ''' Comprueba si el usuario es adminitrador de una planta
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <returns>Booleano</returns>
        ''' <remarks></remarks>
        Public Function EsUsuarioAdministradorPlanta(ByVal idUsuario As Integer, ByVal idPlanta As Integer) As Boolean
            Dim adminPlantDAL As New DAL.ADMINISTRADORES_PLANTA
            Try
                adminPlantDAL.Where.ID_USUARIO.Value = idUsuario
                adminPlantDAL.Where.ID_PLANTA.Value = idPlanta
                adminPlantDAL.Query.Load()
                If (adminPlantDAL.RowCount > 0) Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw New BatzException("errIKScomprobarAdministrador", ex)
            End Try

        End Function


        ''' <summary>
        ''' Comprueba si el usuario es adminitrador de la aplicacion
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>        
        ''' <returns>Booleano</returns>
        ''' <remarks></remarks>
        Public Function EsUsuarioAdministrador(ByVal idUsuario As Integer) As Boolean
            Dim adminDAL As New DAL.ADMINISTRADORES
            Try
                adminDAL.Where.ID_USUARIO.Value = idUsuario
                adminDAL.Query.Load()
                If (adminDAL.RowCount > 0) Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Throw New BatzException("errIKScomprobarAdministrador", ex)
            End Try

        End Function

    End Class
End Namespace
