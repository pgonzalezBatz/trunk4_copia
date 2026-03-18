Namespace BLL

    Public Class GrupoExtComponent

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un grupo
        ''' </summary>
        ''' <param name="oGrupo">Objeto grupo</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getGrupo(ByVal oGrupo As ELL.GrupoExtension) As ELL.GrupoExtension
            Dim grupoDAL As New DAL.GRUPO_EXTENSIONES
            Dim oGrupExt As ELL.GrupoExtension = Nothing
            Try
                If (oGrupo.Id <> Integer.MinValue) Then grupoDAL.Where.ID.Value = oGrupo.Id
                If (oGrupo.IdPlanta <> Integer.MinValue) Then grupoDAL.Where.ID_PLANTA.Value = oGrupo.IdPlanta
                If (oGrupo.Nombre <> String.Empty) Then grupoDAL.Where.GRUPO.Value = oGrupo.Nombre

                grupoDAL.Query.Load()

                If (grupoDAL.RowCount = 1) Then
                    oGrupExt = New ELL.GrupoExtension
                    oGrupExt.Id = grupoDAL.ID
                    oGrupExt.Nombre = grupoDAL.GRUPO
                    oGrupExt.IdPlanta = grupoDAL.ID_PLANTA
                    oGrupExt.Libre = grupoDAL.LIBRE
                    oGrupExt.Obsoleto = grupoDAL.OBSOLETO
                End If
                Return oGrupExt
            Catch ex As Exception
                Throw New BatzException("errObtenerInfoGrupo", ex)
            End Try
        End Function


        ''' <summary>
        ''' Listado de grupos
        ''' </summary>
        ''' <param name="idPlanta">Informacion del grupo</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getGrupos(ByVal idPlanta As Integer) As System.Collections.Generic.List(Of ELL.GrupoExtension)
            Dim grupoDAL As New DAL.GRUPO_EXTENSIONES
            Dim lGrupos As New List(Of ELL.GrupoExtension)
            Dim oGrupExt As ELL.GrupoExtension = Nothing
            Dim plantComp As New SABLib.BLL.PlantasComponent
            Try
                If (idPlanta <> Integer.MinValue) Then grupoDAL.Where.ID_PLANTA.Value = idPlanta

                grupoDAL.Query.Load()

                If (grupoDAL.RowCount > 0) Then
                    Do
                        oGrupExt = New ELL.GrupoExtension
                        oGrupExt.Id = grupoDAL.ID
                        oGrupExt.Nombre = grupoDAL.GRUPO
                        oGrupExt.IdPlanta = grupoDAL.ID_PLANTA
                        oGrupExt.Libre = grupoDAL.LIBRE
                        oGrupExt.Obsoleto = grupoDAL.OBSOLETO
                        oGrupExt.Planta = plantComp.GetPlanta(oGrupExt.IdPlanta).Nombre

                        lGrupos.Add(oGrupExt)
                    Loop While grupoDAL.MoveNext
                End If

                Return lGrupos
            Catch ex As Exception
                Throw New BatzException("errIKSobtenerGrupos", ex)
            End Try

        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda o modifica un grupo de extension
        ''' </summary>
        ''' <param name="oGrupo">Grupo con la informacion</param>
        ''' <returns>Booleano</returns>        
        Public Function Save(ByVal oGrupo As ELL.GrupoExtension) As Boolean
            Dim grupoDAL As New DAL.GRUPO_EXTENSIONES
            Try
                If (oGrupo.Id = Integer.MinValue) Then
                    grupoDAL.AddNew()
                Else
                    grupoDAL.LoadByPrimaryKey(oGrupo.Id)
                End If

                If (grupoDAL.RowCount = 1) Then
                    grupoDAL.GRUPO = oGrupo.Nombre
                    grupoDAL.ID_PLANTA = oGrupo.IdPlanta
                    grupoDAL.LIBRE = oGrupo.Libre
                    grupoDAL.OBSOLETO = oGrupo.Obsoleto
                    grupoDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errGuardar", ex)
            End Try
        End Function


        ''' <summary>
        ''' Elimina un grupo
        ''' </summary>
        ''' <param name="idGrupo">Identificador del grupo</param>
        ''' <returns>Booleano</returns>        
        Public Function Delete(ByVal idGrupo As Integer) As Boolean
            Dim grupoDAL As New DAL.GRUPO_EXTENSIONES
            Try
                grupoDAL.LoadByPrimaryKey(idGrupo)
                If grupoDAL.RowCount = 1 Then
                    grupoDAL.MarkAsDeleted()
                    grupoDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errBorrarGrupo", ex)
            End Try
        End Function

#End Region

    End Class

End Namespace