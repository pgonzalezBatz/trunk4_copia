Imports AccesoAutomaticoBD

Namespace BLL

    Public Class TipoLineaComponent

        ''' <summary>
        ''' Obtiene el tipo de linea especificado
        ''' </summary>
        ''' <param name="idTipoLin">Identificador del tipo linea</param>
        ''' <returns>Objeto Tipo lineaa</returns>
        ''' <remarks></remarks>
        Public Function getTipoLinea(ByVal idTipoLin As Integer) As ELL.TipoLinea
            Try
                Dim tipoLineaCultDal As New DAL.TIPO_LINEA
                Dim oTipoLinea As ELL.TipoLinea = Nothing

                tipoLineaCultDal.LoadByPrimaryKey(idTipoLin)

                If (tipoLineaCultDal.RowCount = 1) Then
                    oTipoLinea = New ELL.TipoLinea()
                    oTipoLinea.Id = tipoLineaCultDal.ID
                    oTipoLinea.RequiereAlveolo = tipoLineaCultDal.s_CON_ALVEOLO
                    oTipoLinea.IdTipoExtension = tipoLineaCultDal.ID_TIPOEXT
                    oTipoLinea.Obsoleto = tipoLineaCultDal.OBSOLETO
                End If

                Return oTipoLinea
            Catch ex As Exception
                Throw New BatzException("errObtenerTipos", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene los tipos de linea especificados
        ''' </summary>
        ''' <param name="oTipoLin">Objeto tipo linea</param>
        ''' <returns>Listado</returns>
        ''' <remarks></remarks>
        Public Function getTiposLinea(ByVal oTipoLin As ELL.TipoLinea) As System.Collections.Generic.List(Of ELL.TipoLinea)
            Dim reader As IDataReader = Nothing
            Try
                Dim tipoLineaDAL As New DAL.TIPO_LINEA
                Dim lTiposLinea As New List(Of ELL.TipoLinea)
                Dim oTipoLinea As ELL.TipoLinea

                reader = tipoLineaDAL.getTiposLinea(oTipoLin)

                While reader.Read
                    oTipoLinea = New ELL.TipoLinea()
                    oTipoLinea.Id = CInt(reader.Item(DAL.TIPO_LINEA.ColumnNames.ID))
                    oTipoLinea.Nombre = reader.Item(DAL.TIPOLINEA_CULTURA.ColumnNames.NOMBRE)
                    If (Not reader.IsDBNull(2)) Then oTipoLinea.Descripcion = reader.Item(DAL.TIPOLINEA_CULTURA.ColumnNames.DESCRIPCION)
                    oTipoLinea.Cultura = reader.Item(DAL.TIPOLINEA_CULTURA.ColumnNames.ID_CULTURA)
                    oTipoLinea.IdTipoExtension = CInt(reader.Item(DAL.TIPO_LINEA.ColumnNames.ID_TIPOEXT))
                    lTiposLinea.Add(oTipoLinea)
                End While

                Return lTiposLinea
            Catch ex As Exception
                Throw New BatzException("errObtenerTipos", ex)
            Finally
                If (reader IsNot Nothing) Then reader.Close()
            End Try
        End Function


        ''' <summary>
        ''' Obtiene los terminos en todos los idiomas de un tipo de linea
        ''' </summary>
        ''' <param name="idTipoLinea">Identificador del tipo</param>
        ''' <returns>Lista de tipos</returns>
        Public Function getTiposLineaKulturaByIdTipo(ByVal idTipoLinea As Integer) As System.Collections.Generic.List(Of ELL.TipoLinea)
            Dim lTipoLinea As New List(Of ELL.TipoLinea)
            Dim oTipoLinea As ELL.TipoLinea
            Dim tipoLCultDAL As New DAL.TIPOLINEA_CULTURA
            Dim nameCult As String
            Dim oTipo As ELL.TipoLinea
            Try
                If (idTipoLinea = Integer.MinValue) Then
                    lTipoLinea = BLL.Utils.recuperarCulturasLinea()
                Else
                    tipoLCultDAL = New DAL.TIPOLINEA_CULTURA
                    tipoLCultDAL.Where.ID_TIPOLINEA.Value = idTipoLinea
                    tipoLCultDAL.Query.AddOrderBy(DAL.TIPOALV_CULTURA.ColumnNames.ID_CULTURA, WhereParameter.Dir.DESC)
                    tipoLCultDAL.Query.Load()

                    If (tipoLCultDAL.RowCount > 0) Then
                        Do
                            oTipoLinea = New ELL.TipoLinea()
                            oTipoLinea.Id = idTipoLinea
                            oTipoLinea.Nombre = tipoLCultDAL.s_NOMBRE
                            oTipoLinea.Descripcion = tipoLCultDAL.s_DESCRIPCION
                            oTipoLinea.Cultura = tipoLCultDAL.s_ID_CULTURA

                            lTipoLinea.Add(oTipoLinea)
                        Loop While tipoLCultDAL.MoveNext

                        'Se comprueba que no falta ningun idioma
                        For Each cult As Integer In [Enum].GetValues(GetType(ELL.TipoLinea.Idioma))
                            nameCult = [Enum].GetName(GetType(ELL.TipoLinea.Idioma), cult)
                            oTipo = New ELL.TipoLinea()
                            If (cult = ELL.TipoLinea.Idioma.Euskera) Then
                                oTipo.Cultura = ELL.TipoLinea.CULTURA_EUSKARA
                            ElseIf (cult = ELL.TipoLinea.Idioma.Español) Then
                                oTipo.Cultura = ELL.TipoLinea.CULTURA_ESPAÑOL
                            ElseIf (cult = ELL.TipoLinea.Idioma.Ingles) Then
                                oTipo.Cultura = ELL.TipoLinea.CULTURA_INGLES
                            ElseIf (cult = ELL.TipoLinea.Idioma.Checo) Then
                                oTipo.Cultura = ELL.TipoLinea.CULTURA_CHECO
                            End If

                            oTipoLinea = lTipoLinea.Find(Function(oTipLin As ELL.TipoLinea) oTipLin.Cultura = oTipo.Cultura)
                            If (oTipoLinea Is Nothing) Then lTipoLinea.Add(oTipo)
                        Next
                    End If
                End If

                Return lTipoLinea
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errObtenerTipos", ex)
            End Try
        End Function


        ''' <summary>
        ''' Guarda la informacion del objeto mantenimiento (informacion distintas culturas)
        ''' </summary>
        ''' <param name="oMant">Objeto mantenimiento que tiene el nombre en distintas culturas</param>
        ''' <returns>Booleano indicando si se ha guardado correctament</returns>      
        Public Function Save(ByVal oMant As ELL.TipoLinea.Mantenimiento) As Boolean
            Dim tipoLineaDAL As DAL.TIPO_LINEA
            Dim tipoLineaCultDAL As New DAL.TIPOLINEA_CULTURA
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Dim listTipoCul As List(Of ELL.TipoLinea)
            Dim idTipo As Integer
            Dim toSave As Boolean
            Try
                listTipoCul = CType(oMant.objecto, List(Of ELL.TipoLinea))
                tx.BeginTransaction()

                'Se inserta primero el tipo del alveolo si fuera una insercion
                If (oMant.acc = ELL.TipoCultura.Accion.insertar) Then
                    tipoLineaDAL = New DAL.TIPO_LINEA
                    tipoLineaDAL.AddNew()
                    tipoLineaDAL.OBSOLETO = False
                    tipoLineaDAL.CON_ALVEOLO = listTipoCul.Item(0).RequiereAlveolo
                    tipoLineaDAL.ID_TIPOEXT = listTipoCul.Item(0).IdTipoExtension
                    tipoLineaDAL.Save()
                    idTipo = tipoLineaDAL.ID
                Else
                    idTipo = listTipoCul.Item(0).Id
                    'Se guarda el requiere alveolo
                    tipoLineaDAL = New DAL.TIPO_LINEA
                    tipoLineaDAL.LoadByPrimaryKey(idTipo)
                    If (tipoLineaDAL.RowCount = 1) Then
                        tipoLineaDAL.CON_ALVEOLO = listTipoCul.Item(0).RequiereAlveolo
                        tipoLineaDAL.ID_TIPOEXT = listTipoCul.Item(0).IdTipoExtension
                        tipoLineaDAL.OBSOLETO = listTipoCul.Item(0).Obsoleto
                        tipoLineaDAL.Save()
                    End If
                End If

                'Se modifican los datos de la cultura
                For Each tipoCul As ELL.TipoLinea In listTipoCul
                    If (oMant.acc = ELL.TipoCultura.Accion.modificar) Then
                        tipoLineaCultDAL.LoadByPrimaryKey(tipoCul.Cultura, idTipo)
                    End If

                    toSave = False
                    If (oMant.acc = ELL.TipoCultura.Accion.modificar And tipoCul.Nombre = String.Empty) Then  'borrarlo
                        tipoLineaCultDAL.MarkAsDeleted()
                        toSave = True
                    ElseIf (oMant.acc = ELL.TipoCultura.Accion.modificar And tipoLineaCultDAL.RowCount = 1) Then  'update
                        tipoLineaCultDAL.NOMBRE = tipoCul.Nombre
                        tipoLineaCultDAL.DESCRIPCION = tipoCul.Descripcion
                        tipoLineaCultDAL.ID_TIPOLINEA = idTipo
                        tipoLineaCultDAL.ID_CULTURA = tipoCul.Cultura
                        toSave = True
                    ElseIf ((oMant.acc = ELL.TipoCultura.Accion.modificar And tipoLineaCultDAL.RowCount = 0) Or (Not (oMant.acc = ELL.TipoCultura.Accion.insertar And tipoCul.Nombre = String.Empty))) Then  'insert
                        tipoLineaCultDAL.AddNew()
                        tipoLineaCultDAL.NOMBRE = tipoCul.Nombre
                        tipoLineaCultDAL.DESCRIPCION = tipoCul.Descripcion
                        tipoLineaCultDAL.ID_TIPOLINEA = idTipo
                        tipoLineaCultDAL.ID_CULTURA = tipoCul.Cultura
                        toSave = True
                    ElseIf (oMant.acc = ELL.TipoCultura.Accion.modificar) Then 'hay que insertarlo                                                        
                        tipoLineaCultDAL.NOMBRE = tipoCul.Nombre
                        tipoLineaCultDAL.DESCRIPCION = tipoCul.Descripcion
                        tipoLineaCultDAL.ID_TIPOLINEA = idTipo
                        tipoLineaCultDAL.ID_CULTURA = tipoCul.Cultura
                        toSave = True
                    End If

                    If (toSave) Then
                        'Se guarda y se resetea el objeto
                        tipoLineaCultDAL.Save()
                        tipoLineaCultDAL.FlushData()
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


        ''' <summary>
        ''' Elimina un tipo de linea
        ''' </summary>
        ''' <param name="oTipoLinea">Identificador de linea</param>
        ''' <returns>Booleano</returns>        
        Public Function Delete(ByVal oTipoLinea As Integer) As Boolean
            Dim tipoLineaDAL As New DAL.TIPO_LINEA
            Try
                tipoLineaDAL.LoadByPrimaryKey(oTipoLinea)
                If tipoLineaDAL.RowCount = 1 Then
                    tipoLineaDAL.MarkAsDeleted()
                    tipoLineaDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errBorrarTipo", ex)
            End Try
        End Function

    End Class

End Namespace