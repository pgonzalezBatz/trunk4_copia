Namespace BLL

    Public Class CiaTlfnoComponent

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la compañia especificada
        ''' </summary>
        ''' <param name="idCia">Identificador de la compañia</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getCompañia(ByVal idCia As Integer) As ELL.CiaTlfno
            Dim ciaDAL As New DAL.CIA_TLFNO
            Dim oCia As ELL.CiaTlfno = Nothing
            Try
                ciaDAL.LoadByPrimaryKey(idCia)

                If (ciaDAL.RowCount = 1) Then
                    oCia = New ELL.CiaTlfno()
                    oCia.Id = idCia
                    oCia.Nombre = ciaDAL.NOMBRE
                    oCia.IdPlanta = ciaDAL.ID_PLANTA
                    oCia.Obsoleto = ciaDAL.OBSOLETO
                    oCia.Prefijo = ciaDAL.s_PREFIJO
                End If
                Return oCia
            Catch ex As Exception
                Throw New BatzException("errObtenerCompañias", ex)
            End Try

        End Function


        ''' <summary>
        ''' Obtiene las compañias especificadas
        ''' </summary>
        ''' <param name="oCia">Objeto cia</param>
        ''' <returns>Listado</returns>        
        Public Function getCompañias(ByVal oCia As ELL.CiaTlfno) As System.Collections.Generic.List(Of ELL.CiaTlfno)
            Dim ciaDAL As New DAL.CIA_TLFNO
            Dim lCias As New List(Of ELL.CiaTlfno)
            Dim oCiaTlfno As ELL.CiaTlfno = Nothing
            Try
                If (oCia.IdPlanta <> Integer.MinValue) Then ciaDAL.Where.ID_PLANTA.Value = oCia.IdPlanta
                If (oCia.Prefijo <> String.Empty) Then ciaDAL.Where.PREFIJO.Value = oCia.Prefijo
                If (Not oCia.Obsoleto) Then ciaDAL.Where.OBSOLETO.Value = CInt(oCia.Obsoleto)

                ciaDAL.Query.Load()

                If (ciaDAL.RowCount > 0) Then
                    Do
                        oCiaTlfno = New ELL.CiaTlfno
                        oCiaTlfno.Id = ciaDAL.ID
                        oCiaTlfno.Nombre = ciaDAL.NOMBRE
                        oCiaTlfno.IdPlanta = ciaDAL.ID_PLANTA
                        oCiaTlfno.Prefijo = ciaDAL.s_PREFIJO
                        oCiaTlfno.Obsoleto = ciaDAL.OBSOLETO
                        oCiaTlfno.Descripcion = ciaDAL.s_DESCRIPCION

                        lCias.Add(oCiaTlfno)
                    Loop While ciaDAL.MoveNext
                End If

                Return lCias
            Catch ex As Exception
                Throw New BatzException("errObtenerCompañias", ex)
            End Try

        End Function

#End Region

#Region "Modificaciones"

        ''' <summary>
        ''' Guarda o inserta una nueva compañia
        ''' </summary>
        ''' <param name="oCia">Objeto que contiene la informacion</param>
        ''' <returns>Booleano</returns>        
        Public Function Save(ByVal oCia As ELL.CiaTlfno) As Boolean
            Dim ciaDAL As New DAL.CIA_TLFNO
            Try
                If (oCia.Id = Integer.MinValue) Then
                    ciaDAL.AddNew()
                Else
                    ciaDAL.LoadByPrimaryKey(oCia.Id)
                End If

                If (ciaDAL.RowCount = 1) Then
                    ciaDAL.NOMBRE = oCia.Nombre
                    ciaDAL.PREFIJO = oCia.Prefijo
                    If (oCia.IdPlanta <> Integer.MinValue) Then ciaDAL.ID_PLANTA = oCia.IdPlanta
                    ciaDAL.OBSOLETO = oCia.Obsoleto
                    ciaDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errGuardar", ex)
            End Try
        End Function


        '''' <summary>
        '''' Elimina una compañia telefonica
        '''' </summary>
        '''' <param name="idCia">Identificador de la compañia</param>
        '''' <returns>Booleano</returns>        
        'Public Function Delete(ByVal idCia As Integer) As Boolean
        '    Dim ciaDAL As New DAL.CIA_TLFNO
        '    Try
        '        ciaDAL.LoadByPrimaryKey(idCia)
        '        If ciaDAL.RowCount = 1 Then
        '            ciaDAL.MarkAsDeleted()
        '            ciaDAL.Save()
        '            Return True
        '        End If
        '        Return False
        '    Catch ex As Exception
        '        Throw New BatzException("errBorrar", ex)
        '    End Try
        'End Function

#End Region

    End Class

End Namespace