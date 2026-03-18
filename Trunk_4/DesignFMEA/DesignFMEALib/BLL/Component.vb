Namespace BLL

    Public Class AMFEComponent
        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Carga la informacion del amfe
        ''' </summary>
        ''' <param name="id">Id</param>
        ''' <returns></returns>   
        Public Function loadAMFE(ByVal id As Integer) As ELL.Amfe
            Dim oAmfe As ELL.Amfe = accessDAL.loadAMFE(id)
            If (oAmfe IsNot Nothing) Then
                oAmfe.Referencias = loadReferencias(id, False, oAmfe.Tipo)
            End If
            Return oAmfe
        End Function

        ''' <summary>
        ''' Carga el listado de amfes
        ''' </summary>        
        ''' <param name="oAmfe">Informacion del tipo</param>
        ''' <returns></returns>        
        Public Function loadListAMFE(ByVal oAmfe As ELL.Amfe) As List(Of ELL.Amfe)
            Return accessDAL.loadListAMFE(oAmfe)
        End Function

        ''' <summary>
        ''' Elimina el AMFE y todas sus referencias asociadas
        ''' </summary>
        ''' <param name="idAmfe">Id del amfe</param>        
        Public Sub DeleteAmfe(ByVal idAmfe As Integer)
            accessDAL.DeleteAmfe(idAmfe)
        End Sub

        ''' <summary>
        ''' Inserta o actualiza
        ''' </summary>
        ''' <param name="oAmfe">Informacion del tipo</param>
        Public Function SaveAmfe(ByVal oAmfe As ELL.Amfe) As Integer
            Return accessDAL.SaveAmfe(oAmfe)
        End Function

        ''' <summary>
        ''' Obtiene las referencias de un amfe
        ''' </summary>
        ''' <param name="idAmfe">Id del amfe</param>
        ''' <returns></returns>        
        Public Function loadReferencias(ByVal idAmfe As Integer, Optional bLoadLessons As Boolean = True, Optional ByVal tipo As Integer = 0) As List(Of ELL.Referencia)
            Dim lReferencias As List(Of ELL.Referencia) = accessDAL.loadListReferencias(idAmfe)
            Dim lessonsBLL As New LeccionesAprendidasLib.BLL.LeccionesAprendidasComponent
            For Each oRef As ELL.Referencia In lReferencias
                oRef.Lecciones = accessDAL.loadListLecciones(idAmfe, oRef.Ref, oRef.IdEmpresa, tipo)
                If (bLoadLessons) Then
                    For index As Integer = oRef.Lecciones.Count - 1 To 0 Step -1
                        oRef.Lecciones(index).Lesson = lessonsBLL.getLA(oRef.Lecciones(index).Lesson.Codigo)
                    Next
                End If
            Next
            Return lReferencias
        End Function

        ''' <summary>
        ''' Obtiene las referencias de un amfe
        ''' </summary>
        ''' <param name="oAmfe">amfe</param>
        ''' <returns></returns>        
        Public Function loadReferencias(ByVal oAmfe As DesignFMEALib.ELL.Amfe, Optional bLoadLessons As Boolean = True) As List(Of ELL.Referencia)
            Dim lReferencias As List(Of ELL.Referencia) = accessDAL.loadListReferencias(oAmfe.Id)
            Dim lessonsBLL As New LeccionesAprendidasLib.BLL.LeccionesAprendidasComponent
            For Each oRef As ELL.Referencia In lReferencias
                oRef.Lecciones = accessDAL.loadListLecciones(oAmfe, oRef.Ref, oRef.IdEmpresa)
                If (bLoadLessons) Then
                    For index As Integer = oRef.Lecciones.Count - 1 To 0 Step -1
                        oRef.Lecciones(index).Lesson = lessonsBLL.getLA(oRef.Lecciones(index).Lesson.Codigo)
                    Next
                End If
            Next
            Return lReferencias
        End Function

        ''' <summary>
        ''' Añade una referencia y las lecciones marcadas
        ''' </summary>
        ''' <param name="idAmfe">Id del amfe</param>
        ''' <param name="oRef">Referencia</param>        
        Public Sub AddReferencia(ByVal idAmfe As Integer, ByVal oRef As ELL.Referencia)
            Try
                If (accessDAL.OpenTransaction()) Then
                    accessDAL.AddReferencia(idAmfe, oRef)
                    If (oRef.Lecciones IsNot Nothing AndAlso oRef.Lecciones.Count > 0) Then
                        For Each oLecc As ELL.Referencia.Leccion In oRef.Lecciones
                            accessDAL.AddLeccion(idAmfe, oRef, oLecc)
                        Next
                    End If
                    accessDAL.CommitTransaction()
                End If
            Catch ex As Exception
                accessDAL.RollBackTransaction()
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Elimina una referencia de un amfe
        ''' </summary>
        ''' <param name="idAmfe">Id del amfe</param>
        ''' <param name="oRef">Referencia</param>    
        Public Sub DeleteReferencia(ByVal idAmfe As Integer, ByVal oRef As ELL.Referencia)
            accessDAL.DeleteReferencia(idAmfe, oRef)
        End Sub

        '''' <summary>
        '''' Obtiene las lecciones de una referencia
        '''' </summary>
        '''' <param name="idAmfe">Id del amfe</param>
        '''' <param name="oRef">Referencia</param>
        '''' <returns></returns>        
        Public Function loadLeccionesReferencia(ByVal idAmfe As Integer, ByVal oRef As ELL.Referencia) As List(Of ELL.Referencia.Leccion)
            Return accessDAL.loadListLecciones(idAmfe, oRef.Ref, oRef.IdEmpresa)
        End Function

        ''' <summary>
        ''' Guarda las lecciones de la referencias de un amfe
        ''' </summary>
        ''' <param name="lReferencias">Lista de referencias</param>        
        Public Sub SaveLeccionesReferencias(ByVal idAmfe As Integer, ByVal lReferencias As List(Of ELL.Referencia))
            Try
                If (accessDAL.OpenTransaction()) Then
                    accessDAL.DeleteLecciones(idAmfe)
                    For Each oRef As ELL.Referencia In lReferencias
                        For Each oLecc As ELL.Referencia.Leccion In oRef.Lecciones
                            accessDAL.AddLeccion(idAmfe, oRef, oLecc)
                        Next
                    Next
                    accessDAL.CommitTransaction()
                End If
            Catch ex As Exception
                accessDAL.RollBackTransaction()
                Throw
            End Try
        End Sub

    End Class

    Public Class BrainComponent
        Private accessDAL As New DAL.AccessDB

        ''' <summary>
        ''' Carga las referencias de Brain
        ''' Primero la busca en Brain y tambien en referencias de ventas por si fuera de desarrollo
        ''' </summary>
        ''' <param name="texto">Texto a buscar</param>
        ''' <returns></returns>        
        Public Function loadReferenciasVentas(ByVal texto As String) As List(Of String())
            Dim lResul As New List(Of String())
            Dim lRefBrain As List(Of String()) = accessDAL.loadReferenciasBrain(texto)
            Dim lRefVentas As List(Of String()) = accessDAL.loadReferenciasVentas(texto)
            If (lRefBrain IsNot Nothing AndAlso lRefBrain.Count > 0) Then lResul.AddRange(lRefBrain)
            If (lRefVentas IsNot Nothing AndAlso lRefVentas.Count > 0) Then lResul.AddRange(lRefVentas)
            Return lResul
        End Function

    End Class

End Namespace
