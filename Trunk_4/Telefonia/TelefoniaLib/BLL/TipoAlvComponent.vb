Imports AccesoAutomaticoBD

Namespace BLL

    Public Class TipoAlvComponent

#Region "Consultas"

        ''' <summary>
        ''' Obtiene la informacion de un tipo de alveolo
        ''' </summary>
        ''' <param name="oTipoAlv">Objeto tipo alveolo</param>
        ''' <returns>Objeto alveolo informado</returns>        
        Public Function getTipoAlv(ByVal oTipoAlv As ELL.TipoCultura) As ELL.TipoCultura
            Dim tipoAlvDAL As New DAL.TIPO_ALVEOLO
            Dim oTipoAlveolo As ELL.TipoCultura = Nothing
            Try
                If (oTipoAlv.Id <> Integer.MinValue) Then tipoAlvDAL.Where.ID.Value = oTipoAlv.Id
                tipoAlvDAL.Query.Load()

                If (tipoAlvDAL.RowCount = 1) Then
                    oTipoAlveolo = New ELL.TipoCultura
                    oTipoAlveolo.Id = tipoAlvDAL.ID
                    oTipoAlveolo.Obsoleto = tipoAlvDAL.OBSOLETO                    
                End If
                Return oTipoAlveolo
            Catch ex As Exception
                Throw New BatzException("errObtenerInfoAlveolo", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene los tipos de linea especificados
        ''' </summary>
        ''' <param name="oTipoAlv">Objeto tipo alveolo</param>
        ''' <returns>Listado</returns>
        ''' <remarks></remarks>
        Public Function getTiposAlv(ByVal oTipoAlv As ELL.TipoCultura) As System.Collections.Generic.List(Of ELL.TipoCultura)
            Dim reader As IDataReader = Nothing
            Try
                Dim tipoAlvDAL As New DAL.TIPO_ALVEOLO
                Dim lTiposAlv As New List(Of ELL.TipoCultura)
                Dim oTipoAlveolo As ELL.TipoCultura

                reader = tipoAlvDAL.getTiposAlv(oTipoAlv)

                While reader.Read
                    oTipoAlveolo = New ELL.TipoLinea()
                    oTipoAlveolo.Id = CInt(reader.Item(DAL.TIPO_ALVEOLO.ColumnNames.ID))
                    oTipoAlveolo.Nombre = reader.Item(DAL.TIPOALV_CULTURA.ColumnNames.NOMBRE)
                    If (Not reader.IsDBNull(2)) Then oTipoAlveolo.Descripcion = reader.Item(DAL.TIPOALV_CULTURA.ColumnNames.DESCRIPCION)
                    oTipoAlveolo.Cultura = reader.Item(DAL.TIPOALV_CULTURA.ColumnNames.ID_CULTURA)
                    lTiposAlv.Add(oTipoAlveolo)
                End While

                Return lTiposAlv
            Catch ex As Exception
                Throw New BatzException("errObtenerTipos", ex)
            Finally
                If (reader IsNot Nothing) Then reader.close()
            End Try
        End Function

        'Public Function getTiposAlv(ByVal OTipoAlv As ELL.TipoCultura) As System.Collections.Generic.List(Of ELL.TipoCultura)
        '    Try
        '        Dim tipoCultDAL As New DAL.TIPOALV_CULTURA
        '        Dim lTiposAlv As New List(Of ELL.TipoCultura)
        '        Dim oTipoAlveolo As ELL.TipoCultura

        '        If (OTipoAlv.Id <> Integer.MinValue) Then tipoCultDAL.Where.ID_TIPOALV.Value = OTipoAlv.Id
        '        If (OTipoAlv.Cultura <> String.Empty) Then tipoCultDAL.Where.ID_CULTURA.Value = OTipoAlv.Cultura
        '        tipoCultDAL.Query.Load()

        '        If (tipoCultDAL.RowCount > 0) Then
        '            Do
        '                oTipoAlveolo = New ELL.TipoCultura()
        '                oTipoAlveolo.Id = tipoCultDAL.ID_TIPOALV
        '                oTipoAlveolo.Nombre = tipoCultDAL.s_NOMBRE
        '                oTipoAlveolo.Descripcion = tipoCultDAL.s_DESCRIPCION
        '                oTipoAlveolo.Cultura = tipoCultDAL.s_ID_CULTURA

        '                lTiposAlv.Add(oTipoAlveolo)
        '            Loop While tipoCultDAL.MoveNext
        '        End If

        '        Return lTiposAlv
        '    Catch ex As Exception
        '        Throw New BatzException("errObtenerTipos", ex)
        '    End Try
        'End Function


        ''' <summary>
        ''' Obtiene los terminos en todos los idiomas de un tipo de alveolo
        ''' </summary>
        ''' <param name="idTipoAlv">Identificador del tipo</param>
        ''' <returns>Lista de tipos</returns>        
        Public Function getTiposAlvKulturaByIdTipo(ByVal idTipoAlv As Integer) As System.Collections.Generic.List(Of ELL.TipoCultura)
            Dim lTipoAlv As New List(Of ELL.TipoCultura)
            Dim oTipoAlveolo As ELL.TipoCultura
            Dim tipoCultDAL As New DAL.TIPOALV_CULTURA
            Dim nameCult As String
            Dim oTipo As ELL.TipoCultura
            Try
                If (idTipoAlv = Integer.MinValue) Then
                    lTipoAlv = BLL.Utils.recuperarCulturas()
                Else
                    tipoCultDAL = New DAL.TIPOALV_CULTURA
                    tipoCultDAL.Where.ID_TIPOALV.Value = idTipoAlv
                    tipoCultDAL.Query.AddOrderBy(DAL.TIPOALV_CULTURA.ColumnNames.ID_CULTURA, WhereParameter.Dir.DESC)
                    tipoCultDAL.Query.Load()

                    If (tipoCultDAL.RowCount > 0) Then
                        Do
                            oTipoAlveolo = New ELL.TipoCultura()
                            oTipoAlveolo.Id = idTipoAlv
                            oTipoAlveolo.Nombre = tipoCultDAL.s_NOMBRE
                            oTipoAlveolo.Descripcion = tipoCultDAL.s_DESCRIPCION
                            oTipoAlveolo.Cultura = tipoCultDAL.s_ID_CULTURA

                            lTipoAlv.Add(oTipoAlveolo)
                        Loop While tipoCultDAL.MoveNext

                        'Se comprueba que no falta ningun idioma
                        For Each cult As Integer In [Enum].GetValues(GetType(ELL.TipoCultura.Idioma))
                            nameCult = [Enum].GetName(GetType(ELL.TipoCultura.Idioma), cult)
                            oTipo = New ELL.TipoCultura()
                            If (cult = ELL.TipoCultura.Idioma.Euskera) Then
                                oTipo.Cultura = ELL.TipoCultura.CULTURA_EUSKARA
                            ElseIf (cult = ELL.TipoCultura.Idioma.Español) Then
                                oTipo.Cultura = ELL.TipoCultura.CULTURA_ESPAÑOL
                            ElseIf (cult = ELL.TipoCultura.Idioma.Ingles) Then
                                oTipo.Cultura = ELL.TipoCultura.CULTURA_INGLES
                            ElseIf (cult = ELL.TipoCultura.Idioma.Checo) Then
                                oTipo.Cultura = ELL.TipoCultura.CULTURA_CHECO
                            End If

                            oTipoAlveolo = lTipoAlv.Find(Function(oTipAlv As ELL.TipoCultura) oTipAlv.Cultura = oTipo.Cultura)
                            If (oTipoAlveolo Is Nothing) Then lTipoAlv.Add(oTipo)
                        Next
                    End If
                End If

                Return lTipoAlv
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errObtenerTipos", ex)
            End Try
        End Function

#End Region

#Region "Save"

        ''' <summary>
        ''' Guarda la informacion del objeto mantenimiento (informacion distintas culturas)
        ''' </summary>
        ''' <param name="oMant">Objeto mantenimiento que tiene el nombre en distintas culturas</param>
        ''' <returns>Booleano indicando si se ha guardado correctament</returns>        
        Public Function Save(ByVal oMant As ELL.TipoCultura.Mantenimiento) As Boolean
            Dim tipoAlvDAL As DAL.TIPO_ALVEOLO
            Dim tipoAlvCultDAL As New DAL.TIPOALV_CULTURA
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Dim listTipoCul As List(Of ELL.TipoCultura)
            Dim idTipo As Integer
            Dim toSave As Boolean
            Try
                listTipoCul = CType(oMant.objecto, List(Of ELL.TipoCultura))
                tx.BeginTransaction()

                tipoAlvDAL = New DAL.TIPO_ALVEOLO
                'Se inserta primero el tipo del alveolo si fuera una insercion
                If (oMant.acc = ELL.TipoCultura.Accion.insertar) Then
                    tipoAlvDAL.AddNew()
                    tipoAlvDAL.OBSOLETO = False
                    tipoAlvDAL.Save()
                    idTipo = tipoAlvDAL.ID
                Else
                    idTipo = listTipoCul.Item(0).Id
                    tipoAlvDAL.LoadByPrimaryKey(idTipo)
                    If (tipoAlvDAL.RowCount = 1) Then    
                        tipoAlvDAL.OBSOLETO = CInt(listTipoCul.Item(0).Obsoleto)
                        tipoAlvDAL.Save()
                        tipoAlvDAL.FlushData()
                    End If
                End If

                    'Se modifican los datos de la cultura
                    For Each tipoCul As ELL.TipoCultura In listTipoCul
                        If (oMant.acc = ELL.TipoCultura.Accion.modificar) Then
                            tipoAlvCultDAL.LoadByPrimaryKey(tipoCul.Cultura, idTipo)
                        End If

                        toSave = False
                        If (oMant.acc = ELL.TipoCultura.Accion.modificar And tipoCul.Nombre = String.Empty) Then  'borrarlo
                            tipoAlvCultDAL.MarkAsDeleted()
                            toSave = True
                        ElseIf (oMant.acc = ELL.TipoCultura.Accion.modificar And tipoAlvCultDAL.RowCount = 1) Then  'update
                            tipoAlvCultDAL.NOMBRE = tipoCul.Nombre
                            tipoAlvCultDAL.DESCRIPCION = tipoCul.Descripcion
                            tipoAlvCultDAL.ID_TIPOALV = idTipo
                            tipoAlvCultDAL.ID_CULTURA = tipoCul.Cultura
                            toSave = True
                        ElseIf ((oMant.acc = ELL.TipoCultura.Accion.modificar And tipoAlvCultDAL.RowCount = 0) Or (Not (oMant.acc = ELL.TipoCultura.Accion.insertar And tipoCul.Nombre = String.Empty))) Then  'insert
                            tipoAlvCultDAL.AddNew()
                            tipoAlvCultDAL.NOMBRE = tipoCul.Nombre
                            tipoAlvCultDAL.DESCRIPCION = tipoCul.Descripcion
                            tipoAlvCultDAL.ID_TIPOALV = idTipo
                            tipoAlvCultDAL.ID_CULTURA = tipoCul.Cultura
                            toSave = True
                        ElseIf (oMant.acc = ELL.TipoCultura.Accion.modificar) Then 'hay que insertarlo                                                        
                            tipoAlvCultDAL.NOMBRE = tipoCul.Nombre
                            tipoAlvCultDAL.DESCRIPCION = tipoCul.Descripcion
                            tipoAlvCultDAL.ID_TIPOALV = idTipo
                            tipoAlvCultDAL.ID_CULTURA = tipoCul.Cultura
                            toSave = True
                        End If

                        If (toSave) Then
                            'Se guarda y se resetea el objeto
                            tipoAlvCultDAL.Save()
                            tipoAlvCultDAL.FlushData()
                        End If
                    Next
                    tx.CommitTransaction()
                    Return True
            Catch ex As Exception
                tx.RollbackTransaction()
                TransactionMgr.ThreadTransactionMgrReset()
                Throw New BatzException("errGuardar", ex)
            End Try            
        End Function

#End Region

#Region "Delete"

        ''' <summary>
        ''' Elimina un tipo de alveolo
        ''' </summary>
        ''' <param name="idTipoAlv">Identificador del alveolo</param>
        ''' <returns>Booleano</returns>        
        Public Function Delete(ByVal idTipoAlv As Integer) As Boolean
            Dim tipoAlvDAL As New DAL.TIPO_ALVEOLO
            Try
                tipoAlvDAL.LoadByPrimaryKey(idTipoAlv)
                If tipoAlvDAL.RowCount = 1 Then
                    tipoAlvDAL.MarkAsDeleted()
                    tipoAlvDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errBorrarTipo", ex)
            End Try
        End Function

#End Region

    End Class

End Namespace