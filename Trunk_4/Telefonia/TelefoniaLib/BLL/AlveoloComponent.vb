Namespace BLL

    Public Class AlveoloComponent

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un alveolo
        ''' </summary>
        ''' <param name="oAlv">Objeto alveolo</param>
        ''' <returns>Alveolo</returns>        
        Public Function getAlveolo(ByVal oAlv As ELL.Alveolo) As ELL.Alveolo
            Dim alveoloDAL As New DAL.ALVEOLO
            Dim oAlveolo As ELL.Alveolo = Nothing
            Try
                If (oAlv.Id <> Integer.MinValue) Then alveoloDAL.Where.ID.Value = oAlv.Id
                If (oAlv.Ruta <> String.Empty) Then alveoloDAL.Where.RUTA.Value = oAlv.Ruta                
                alveoloDAL.Query.Load()
                If (alveoloDAL.RowCount = 1) Then
                    oAlveolo = New ELL.Alveolo()
                    oAlveolo.Id = alveoloDAL.ID
                    oAlveolo.Ruta = alveoloDAL.s_RUTA
                    If Not (alveoloDAL.IsColumnNull(DAL.ALVEOLO.ColumnNames.ID_TIPOALV)) Then oAlveolo.IdTipo = alveoloDAL.ID_TIPOALV
                    oAlveolo.Estado = CType(alveoloDAL.ESTADO, Boolean)
                    oAlveolo.IdPlanta = alveoloDAL.ID_PLANTA
                    oAlveolo.PosicionFila = alveoloDAL.POS_FILA
                    oAlveolo.PosicionColumna = alveoloDAL.POS_COL
                    oAlveolo.Obsoleto = alveoloDAL.OBSOLETO
                End If
                Return oAlveolo
            Catch ex As Exception
                Throw New BatzException("errObtenerInfoAlveolo", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene una lista de alveolo de una planta
        ''' </summary>
        ''' <param name="oAlv">Objeto alveolo</param>
        ''' <returns>Lista de alveolos</returns>        
        Public Function getAlveolos(ByVal oAlv As ELL.Alveolo) As System.Collections.Generic.List(Of ELL.Alveolo)
            Dim alveoloDAL As New DAL.ALVEOLO
            Dim lAlveolos As New List(Of ELL.Alveolo)
            Dim oAlveolo As ELL.Alveolo = Nothing
            Try
                If (oAlv.IdPlanta <> Integer.MinValue) Then alveoloDAL.Where.ID_PLANTA.Value = oAlv.IdPlanta
                If (oAlv.IdTipo <> Integer.MinValue) Then alveoloDAL.Where.ID_TIPOALV.Value = oAlv.IdTipo
                If (oAlv.PosicionFila <> Integer.MinValue) Then alveoloDAL.Where.POS_FILA.Value = oAlv.PosicionFila
                If (oAlv.PosicionColumna <> Integer.MinValue) Then alveoloDAL.Where.POS_COL.Value = oAlv.PosicionColumna
                If (Not oAlv.Obsoleto) Then alveoloDAL.Where.OBSOLETO.Value = CInt(oAlv.Obsoleto)
                alveoloDAL.Query.Load()
                If (alveoloDAL.RowCount > 0) Then
                    Do
                        oAlveolo = New ELL.Alveolo()
                        oAlveolo.Id = alveoloDAL.ID
                        oAlveolo.Ruta = alveoloDAL.s_RUTA
                        If Not (alveoloDAL.IsColumnNull(DAL.ALVEOLO.ColumnNames.ID_TIPOALV)) Then oAlveolo.IdTipo = alveoloDAL.ID_TIPOALV
                        oAlveolo.Estado = CType(alveoloDAL.ESTADO, Boolean)
                        oAlveolo.IdPlanta = alveoloDAL.ID_PLANTA
                        oAlveolo.Obsoleto = alveoloDAL.OBSOLETO
                        oAlveolo.PosicionFila = If(alveoloDAL.s_POS_FILA = String.Empty, Integer.MinValue, alveoloDAL.POS_FILA)
                        oAlveolo.PosicionColumna = If(alveoloDAL.s_POS_COL = String.Empty, Integer.MinValue, alveoloDAL.POS_COL)
                        lAlveolos.Add(oAlveolo)
                    Loop While alveoloDAL.MoveNext
                End If

                Return lAlveolos
            Catch ex As Exception
                Throw New BatzException("errObtenerAlveolos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene una lista de alveolo libres de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Lista de alveolos</returns>        
        Public Function getAlveolosLibres(ByVal idPlanta As Integer) As System.Collections.Generic.List(Of ELL.Alveolo)
            Dim alveoloDAL As New DAL.ALVEOLO
            Dim lAlveolos As New List(Of ELL.Alveolo)
            Dim oAlveolo As ELL.Alveolo = Nothing
            Dim reader As IDataReader = Nothing
            Try                
                reader = alveoloDAL.getAlveolosLibres(idPlanta)
                While reader.Read
                    oAlveolo = New ELL.Alveolo()
                    oAlveolo.Id = CInt(reader.Item(DAL.ALVEOLO.ColumnNames.ID))
                    oAlveolo.Ruta = reader.Item(DAL.ALVEOLO.ColumnNames.RUTA)                    
                    oAlveolo.Estado = CType(reader.Item(DAL.ALVEOLO.ColumnNames.ESTADO), Boolean)
                    oAlveolo.IdPlanta = CInt(reader.Item(DAL.ALVEOLO.ColumnNames.ID_PLANTA))
                    oAlveolo.PosicionFila = CInt(reader.Item(DAL.ALVEOLO.ColumnNames.POS_FILA))
                    oAlveolo.PosicionColumna = CInt(reader.Item(DAL.ALVEOLO.ColumnNames.POS_COL))
                    oAlveolo.Obsoleto = CType(reader.Item(DAL.ALVEOLO.ColumnNames.OBSOLETO), Boolean)
                    If (Not reader.IsDBNull(5)) Then oAlveolo.IdTipo = CInt(reader.Item(DAL.ALVEOLO.ColumnNames.ID_TIPOALV))
                    lAlveolos.Add(oAlveolo)
                End While

                Return lAlveolos
            Catch ex As Exception
                Throw New BatzException("errObtenerAlveolos", ex)
            Finally
                If (reader IsNot Nothing) Then reader.Close()
            End Try
        End Function

#End Region

#Region "Save"

        ''' <summary>
        ''' Guarda o modifica la informacion del alveolo
        ''' </summary>
        ''' <param name="oAlv">Objeto alveolo</param>
        ''' <returns>Booleano indicando si se ha realizado correctamente</returns>        
        Public Function Save(ByVal oAlv As ELL.Alveolo) As Boolean
            Dim alveoloDAL As New DAL.ALVEOLO
            Try
                If (oAlv.Id = Integer.MinValue) Then
                    alveoloDAL.AddNew()
                Else
                    alveoloDAL.LoadByPrimaryKey(oAlv.Id)
                End If
                If (alveoloDAL.RowCount = 1) Then
                    alveoloDAL.RUTA = oAlv.Ruta
                    alveoloDAL.ID_TIPOALV = oAlv.IdTipo
                    alveoloDAL.ID_PLANTA = oAlv.IdPlanta
                    alveoloDAL.ESTADO = oAlv.Estado
                    alveoloDAL.POS_FILA = oAlv.PosicionFila
                    alveoloDAL.POS_COL = oAlv.PosicionColumna
                    alveoloDAL.OBSOLETO = CInt(oAlv.Obsoleto)
                    alveoloDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errGuardar", ex)
            End Try
        End Function

#End Region

#Region "Delete(Comentado)"

        '''' <summary>
        '''' Elimina el alveolo
        '''' </summary>
        '''' <param name="idAlv">Identificador del alveolo</param>
        '''' <returns>Booleano indicando si la operacion se ha realizado correctamente</returns>
        '''' <remarks></remarks>
        'Public Function Delete(ByVal idAlv As Integer) As Boolean
        '    Dim alveoloDAL As New DAL.ALVEOLO
        '    Try
        '        alveoloDAL.LoadByPrimaryKey(idAlv)
        '        If alveoloDAL.RowCount = 1 Then
        '            alveoloDAL.OBSOLETO = CInt(True)
        '            alveoloDAL.Save()
        '            Return True
        '        End If
        '        Return False
        '    Catch ex As Exception
        '        Throw New BatzException("errBorrarAlveolo", ex)
        '    End Try
        'End Function

#End Region

    End Class

End Namespace