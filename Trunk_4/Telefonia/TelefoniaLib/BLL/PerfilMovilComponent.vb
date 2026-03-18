Namespace BLL

    Public Class PerfilMovComponent

        Private perfilMovDAL As New DAL.PerfilMovDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un perfil
        ''' </summary>
        ''' <param name="id">id del perfil</param>
        ''' <returns>Alveolo</returns>        
        Public Function load(ByVal id As Integer) As ELL.PerfilMovil
            Return perfilMovDAL.load(id)
        End Function


        ''' <summary>
        ''' Obtiene una lista de perfiles de una planta
        ''' </summary>
        ''' <param name="bVigentes">Indica si se obtendran los vigentes o todos</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Lista de perfiles</returns>        
        Public Function loadList(ByVal bVigentes As Boolean, ByVal idPlanta As Integer) As List(Of ELL.PerfilMovil)
            Return perfilMovDAL.loadList(bVigentes, idPlanta)
        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda o modifica la informacion de un perfil
        ''' </summary>
        ''' <param name="oPerf">Objeto perfil</param>        
        Public Sub Save(ByVal oPerf As ELL.PerfilMovil)
            perfilMovDAL.Save(oPerf)
        End Sub

        '        '''' <summary>
        '        '''' Elimina el alveolo
        '        '''' </summary>
        '        '''' <param name="idAlv">Identificador del alveolo</param>
        '        '''' <returns>Booleano indicando si la operacion se ha realizado correctamente</returns>
        '        '''' <remarks></remarks>
        '        'Public Function Delete(ByVal idAlv As Integer) As Boolean
        '        '    Dim alveoloDAL As New DAL.ALVEOLO
        '        '    Try
        '        '        alveoloDAL.LoadByPrimaryKey(idAlv)
        '        '        If alveoloDAL.RowCount = 1 Then
        '        '            alveoloDAL.OBSOLETO = CInt(True)
        '        '            alveoloDAL.Save()
        '        '            Return True
        '        '        End If
        '        '        Return False
        '        '    Catch ex As Exception
        '        '        Throw New BatzException("errBorrarAlveolo", ex)
        '        '    End Try
        '        'End Function

#End Region

    End Class

End Namespace