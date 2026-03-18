Imports SABLib_Z.BLL.Interface
Imports AccesoAutomaticoBD

Namespace BLL
    Public Class GruposComponent
        Implements IGruposComponent

#Region "Consultas"

        ''' <summary>
        ''' Obtiene un grupo para una cultura dada
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <param name="idCultura">Identificador de la cultura</param>
        ''' <returns>Devuelve una lista con los grupos que cumplen la condicion</returns>
        Function GetGrupoCultura(ByVal idGrupo As Integer, ByVal idCultura As String) As ELL.grupo Implements IGruposComponent.GetGrupoCultura
            Dim reader As IDataReader = Nothing
            Dim oGrupo As ELL.grupo = Nothing
            Try
                Dim gruposDAL As New DAL.GRUPOSCULTURAS
                Dim oGrup As New ELL.grupo
                oGrup.IdGrupo = idGrupo
                oGrup.IdCultura = idCultura
                reader = gruposDAL.GetGruposCultura(oGrup)

                While reader.Read
                    oGrupo = New ELL.grupo
                    oGrupo.IdGrupo = CInt(reader.Item(DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.IDGRUPOS))
                    oGrupo.IdCultura = reader.Item(DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.IDCULTURAS)
                    oGrupo.Nombre = reader.Item(DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.NOMBRE)
                    Exit While
                End While
            Catch                
            Finally
                If (reader IsNot Nothing) Then reader.Close()
            End Try
            Return oGrupo
        End Function

        ''' <summary>
        ''' Obtiene los grupos que cumplan las condiciones
        ''' </summary>
        ''' <param name="oGrupo">Grupo</param>
        ''' <returns>Devuelve una lista con los grupos que cumplen la condicion</returns>
        Function GetGruposCultura(ByVal oGrupo As ELL.grupo) As List(Of ELL.grupo) Implements IGruposComponent.GetGruposCultura
            Dim reader As IDataReader = Nothing
            Try
                Dim gruposDAL As New DAL.GRUPOSCULTURAS
                Dim oGrup As ELL.grupo
                Dim lGrupos As New List(Of ELL.grupo)
                reader = gruposDAL.GetGruposCultura(oGrupo)

                While reader.Read
                    oGrup = New ELL.grupo
                    oGrup.IdGrupo = CInt(reader.Item(DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.IDGRUPOS))
                    oGrup.IdCultura = reader.Item(DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.IDCULTURAS)
                    oGrup.Nombre = reader.Item(DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.NOMBRE)
                    lGrupos.Add(oGrup)
                End While

                Return lGrupos
            Catch
                Return Nothing
            Finally
                If (reader IsNot Nothing) Then reader.Close()
            End Try

        End Function


        ''' <summary>
        ''' Obtiene todos los grupos de una cultura asignados a un recurso
        ''' </summary>
        ''' <param name="idRecurso">recurso al que estan asignados</param>
        ''' <param name="IdCultura">Identificador de la cultura</param>
        ''' <returns>Lista de grupos</returns>
        Public Function GetGruposCultura(ByVal idRecurso As Integer, ByVal IdCultura As String) As System.Collections.Generic.List(Of ELL.grupo) Implements [Interface].IGruposComponent.GetGruposCultura
            Dim grupoCulturaDAL As New DAL.Views.W_GRUPOS_RECURSO
            Dim grupoDAL As New DAL.GRUPOS
            Dim lGrupos As New List(Of ELL.grupo)
            Dim oGrupo As ELL.grupo = Nothing

            grupoCulturaDAL.Where.IDCULTURAS.Value = IdCultura
            grupoCulturaDAL.Where.IDRECURSOS.Value = idRecurso
            grupoCulturaDAL.Query.Load()
            If (grupoCulturaDAL.RowCount > 0) Then
                Do
                    oGrupo = New ELL.grupo()
                    oGrupo.IdGrupo = grupoCulturaDAL.IDGRUPOS

                    grupoDAL.FlushData()
                    grupoDAL.LoadByPrimaryKey(oGrupo.IdGrupo)
                    If (grupoDAL.RowCount = 1) Then
                        If Not (grupoDAL.OBSOLETO) Then
                            oGrupo.Nombre = grupoCulturaDAL.NOMBREGRUPO
                            oGrupo.IdCultura = grupoCulturaDAL.IDCULTURAS
                            lGrupos.Add(oGrupo)
                        End If
                    Else
                        Throw New Exception
                    End If

                Loop While grupoCulturaDAL.MoveNext
            End If

            Return lGrupos

        End Function


        ''' <summary>
        ''' Obtiene los grupos, que pertenezcan a alguna planta de las de la lista
        ''' </summary>
        ''' <param name="lPlantas">Plantas a las que debe pertenecer un grupo</param>
        ''' <param name="IdCultura">Cultura en la que ha que mostrar los grupos</param>
        ''' <returns>Lista de usuarios</returns>                
        Function GetGruposCultura(ByVal lPlantas As List(Of Integer), ByVal IdCultura As String) As List(Of ELL.grupo) Implements IGruposComponent.GetGruposCultura
            Dim reader As IDataReader = Nothing
            Try
                Dim gruposCultDAL As New DAL.GRUPOSCULTURAS
                Dim lGrupos As New List(Of ELL.grupo)
                Dim cult As String
                Dim oGrupo As ELL.grupo
                reader = gruposCultDAL.GetGruposConPlanta(lPlantas, IdCultura)
                While reader.Read
                    cult = reader(DAL.GRUPOSCULTURAS.ColumnNames.IDCULTURAS)
                    If (cult = IdCultura) Then
                        oGrupo = New ELL.grupo
                        oGrupo.IdGrupo = CInt(reader(DAL.GRUPOSCULTURAS.ColumnNames.IDGRUPOS))
                        oGrupo.IdCultura = reader(DAL.GRUPOSCULTURAS.ColumnNames.IDCULTURAS)
                        If Not reader.IsDBNull(2) Then oGrupo.Nombre = reader(DAL.GRUPOSCULTURAS.ColumnNames.NOMBRE)

                        lGrupos.Add(oGrupo)
                    End If
                End While

                Return lGrupos
            Catch
                Return Nothing
            Finally
                If (reader IsNot Nothing) Then reader.close()
            End Try
		End Function

		''' <summary>
		''' Obtiene los usuarios de un grupo
		''' </summary>
		''' <param name="idGrupo">Identificador del grupo</param>
		''' <returns>Lista de usuarios</returns>        
        Function GetUsuariosGrupo(ByVal idGrupo As Integer) As List(Of ELL.Usuario) Implements IGruposComponent.GetUsuariosGrupo
            Dim userGrupsDAL As New DAL.USUARIOSGRUPOS
            Dim userComp As New BLL.UsuariosComponent
            Dim oUser As ELL.Usuario
            Dim lUsers As New List(Of ELL.Usuario)
            userGrupsDAL.Where.IDGRUPOS.Value = idGrupo
            userGrupsDAL.Query.Load()
            If (userGrupsDAL.RowCount > 0) Then
                Do
                    oUser = New ELL.Usuario
                    oUser.Id = userGrupsDAL.IDUSUARIOS
                    oUser = userComp.GetUsuario(oUser)
                    If (oUser IsNot Nothing) Then lUsers.Add(oUser)
                Loop While userGrupsDAL.MoveNext
            End If

            Return lUsers
        End Function


        ''' <summary>
        ''' Devuelve las plantas en las que esta asociado un grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <returns>Lista de plantas</returns>
        ''' <remarks></remarks>
        Public Function GetPlantas(ByVal idGrupo As Integer) As System.Collections.Generic.List(Of ELL.Planta) Implements IGruposComponent.GetPlantas
            Dim plantas As New List(Of ELL.Planta)
            Try
                Dim oPlanta As ELL.Planta
                Dim plantComp As New BLL.PlantasComponent
                Dim grupPlantasDAL As New DAL.GRUPOSPLANTAS

                grupPlantasDAL.Where.ID_GRUPO.Value = idGrupo
                grupPlantasDAL.Query.Load()

                If grupPlantasDAL.RowCount > 0 Then
                    Do
                        oPlanta = plantComp.GetPlanta(grupPlantasDAL.ID_PLANTA)
                        plantas.Add(oPlanta)
                    Loop While grupPlantasDAL.MoveNext()
                End If
            Catch

            End Try
            Return plantas
        End Function

#End Region

#Region "Add"

        ''' <summary>
        ''' Añade un grupo
        ''' </summary>        
        ''' <returns>Identificador del nuevo grupo</returns>
        Function AddGrupo() As Integer Implements IGruposComponent.AddGrupo
            Dim grupoComp As New DAL.GRUPOS()
            grupoComp.AddNew()
            If (grupoComp.RowCount = 1) Then
                grupoComp.OBSOLETO = 0
                Try
                    grupoComp.Save()
                Catch
                    Return Integer.MinValue
                End Try
                Return grupoComp.ID
            End If
            Return Integer.MinValue
        End Function


        ''' <summary>
        ''' Añade un usuario a un grupo
        ''' </summary>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <param name="idGrupo">Identificador del grupo</param>
        Function AddUsuario(ByVal idUsuario As Integer, ByVal idGrupo As Integer) As Boolean Implements IGruposComponent.AddUsuario
            Dim usuarioGrupo As New DAL.USUARIOSGRUPOS()
            usuarioGrupo.AddNew()
            usuarioGrupo.IDGRUPOS = idGrupo
            usuarioGrupo.IDUSUARIOS = idUsuario
            Try
                usuarioGrupo.Save()
            Catch
                Return False
            End Try
            Return True
        End Function

        ''' <summary>
        ''' Añade un recurso a un grupo
        ''' </summary>
        ''' <param name="IdGrupo">Identificador del grupo</param>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <remarks>Booleano que indica si ha añadido correctamente</remarks>
        Function AddRecurso(ByVal IdGrupo As Integer, ByVal idRecurso As Integer) As Boolean Implements IGruposComponent.AddRecurso
            Dim grupoRecursos As New DAL.GRUPOSRECURSOS()
            grupoRecursos.AddNew()
            grupoRecursos.IDGRUPOS = IdGrupo
            grupoRecursos.IDRECURSOS = idRecurso
            Try
                grupoRecursos.Save()
            Catch
                Return False
            End Try
            Return True
        End Function

#End Region

#Region "Delete"

        ''' <summary>
        ''' Borra el grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del Grupo</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        Function Delete(ByVal idGrupo As Integer) As Boolean Implements IGruposComponent.Delete
            Dim grupo As New DAL.GRUPOS
            grupo.LoadByPrimaryKey(idGrupo)
            If grupo.RowCount = 1 Then
                grupo.OBSOLETO = 1
                Try
                    grupo.Save()
                Catch
                    Return False
                End Try
                Return True
            End If
            Return False
        End Function


        ''' <summary>
        ''' Borra un usuario de un grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <param name="idUsuario">Identificador del usuario</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        Function DeleteUsuario(ByVal idGrupo As Integer, ByVal idUsuario As Integer) As Boolean Implements IGruposComponent.DeleteUsuario
            Dim usuariosGrupos As New DAL.USUARIOSGRUPOS()
            usuariosGrupos.LoadByPrimaryKey(idGrupo, idUsuario)
            If usuariosGrupos.RowCount = 1 Then
                usuariosGrupos.MarkAsDeleted()
                Try
                    usuariosGrupos.Save()
                Catch
                    Return False
                End Try
                Return True
            End If
            Return False
        End Function


        ''' <summary>
        ''' Borra el recurso del grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del Grupo</param>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <returns>Booleano indicando si se ha borrado correctamente</returns>
        Function DeleteRecurso(ByVal idGrupo As Integer, ByVal idRecurso As String) As Boolean Implements IGruposComponent.DeleteRecurso
            Dim grupoRecursos As New DAL.GRUPOSRECURSOS()
            grupoRecursos.LoadByPrimaryKey(idGrupo, idRecurso)
            If grupoRecursos.RowCount = 1 Then
                grupoRecursos.MarkAsDeleted()
                Try
                    grupoRecursos.Save()
                Catch
                    Return False
                End Try
                Return True
            End If
            Return False
        End Function

#End Region

#Region "Save"

        ''' <summary>
        ''' Inserta o modifica el termino del grupo en la cultura especificada
        ''' </summary>
        ''' <param name="oGrupo">Grupo</param>
        ''' <param name="bnuevo">Indica si hay que insertar o modificar</param>
        ''' <returns>Booleano</returns>
        Public Function SaveGrupoCultura(ByVal oGrupo As ELL.grupo, ByVal bnuevo As Boolean) As Boolean Implements [Interface].IGruposComponent.SaveGrupoCultura
            Try
                Dim grupoCultcomp As New DAL.GRUPOSCULTURAS
                If (bnuevo) Then
                    grupoCultcomp.AddNew()
                Else
                    grupoCultcomp.LoadByPrimaryKey(oGrupo.IdCultura, oGrupo.IdGrupo)
                End If

                If (grupoCultcomp.RowCount = 1) Then
                    grupoCultcomp.IDCULTURAS = oGrupo.IdCultura
                    grupoCultcomp.IDGRUPOS = oGrupo.IdGrupo
                    grupoCultcomp.NOMBRE = oGrupo.Nombre.Trim
                    grupoCultcomp.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Return False
            End Try
        End Function


        ''' <summary>
        ''' Inserta o modifica las plantas del grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <param name="lPlantas">Listado de plantas</param>
        ''' <returns>Booleano</returns>
        Private Function SavePlantas(ByVal idGrupo As Integer, ByVal lPlantas As List(Of ELL.Planta)) As Boolean
            Try
                Dim grupoPlantcomp As New DAL.GRUPOSPLANTAS
                grupoPlantcomp.Where.ID_GRUPO.Value = idGrupo
                grupoPlantcomp.Query.Load()

                'Primero se borran todas
                If (grupoPlantcomp.RowCount > 0) Then
                    grupoPlantcomp.DeleteAll()
                    grupoPlantcomp.Save()
                End If

                'Se insertan una a una
                For Each oPlant As ELL.Planta In lPlantas
                    grupoPlantcomp.FlushData()
                    grupoPlantcomp.AddNew()
                    grupoPlantcomp.ID_GRUPO = idGrupo
                    grupoPlantcomp.ID_PLANTA = oPlant.Id
                    grupoPlantcomp.Save()
                Next
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function


        ''' <summary>
        ''' Guarda las traducciones de las distintas culturas de un grupo y la info de un grupo
        ''' Si viene informado una traduccion y no existe se inserta, sino, se actualiza
        ''' </summary>
        ''' <param name="lGrupos">Lista de grupos a modificar</param>
        ''' <param name="lPlantas">Listado de plantas a guardar</param>
        ''' <param name="bnuevo">Indica si es un nuevo grupo</param>        
        ''' <returns>Devuelve el idGrupo</returns>  
        Public Function ActualizarGruposCultura(ByVal lGrupos As System.Collections.Generic.List(Of ELL.grupo), ByVal lPlantas As List(Of ELL.Planta), ByVal bnuevo As Boolean) As Integer Implements [Interface].IGruposComponent.ActualizarGruposCultura
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                Dim lGrups As List(Of ELL.grupo)
                Dim oGrupoAux As ELL.grupo
                Dim idGrupo As Integer

                tx.BeginTransaction()

                'Si no existe el grupo, primero habrá que crearlo
                If (bnuevo) Then
                    idGrupo = AddGrupo()
                    If (idGrupo = Integer.MinValue) Then Throw New Exception
                Else
                    idGrupo = lGrupos.Item(0).IdGrupo
                End If

                'se guardan las plantas
                If Not SavePlantas(idGrupo, lPlantas) Then Throw New Exception


                'Se guardan las traducciones
                For Each oGrupo As ELL.grupo In lGrupos
                    'Si el grupo es nuevo, se asigna el grupo insertado arriba
                    If (bnuevo) Then oGrupo.IdGrupo = idGrupo

                    'Se consulta si esta el grupo sin el nombre
                    oGrupoAux = New ELL.grupo
                    oGrupoAux.IdCultura = oGrupo.IdCultura
                    oGrupoAux.IdGrupo = oGrupo.IdGrupo
                    lGrups = GetGruposCultura(oGrupoAux)

                    If Not SaveGrupoCultura(oGrupo, Not (lGrups IsNot Nothing AndAlso lGrups.Count > 0)) Then Throw New Exception

                Next

                tx.CommitTransaction()
                Return idGrupo
            Catch ex As Exception
                tx.RollbackTransaction()
                Return Integer.MinValue
            End Try
        End Function



#End Region

    End Class
End Namespace

