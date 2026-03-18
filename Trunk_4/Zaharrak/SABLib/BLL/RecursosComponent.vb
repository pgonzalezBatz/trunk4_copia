Imports SABLib_Z.BLL.Interface
Imports AccesoAutomaticoBD

Namespace BLL
    Public Class RecursosComponent
        Implements IRecursosComponent

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el recurso asociado
        ''' </summary>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <returns>Objeto recursos</returns>
        Function GetRecurso(ByVal IdRecurso As Integer) As ELL.recurso Implements IRecursosComponent.GetRecurso
            Dim recursosDAL As New DAL.RECURSOS
            Dim objRecurso As New ELL.recurso
            recursosDAL.LoadByPrimaryKey(IdRecurso)

            If (recursosDAL.RowCount = 1) Then
                objRecurso = getObject(recursosDAL)
            End If

            Return objRecurso

        End Function

        ''' <summary>
        ''' Obtiene un recurso para una cultura dada
        ''' </summary>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <param name="idCultura">Identificador de la cultura</param>
        ''' <returns>Devuelve una lista con los grupos que cumplen la condicion</returns>
        Function GetRecursoCultura(ByVal idRecurso As Integer, ByVal idCultura As String) As ELL.recurso Implements IRecursosComponent.GetRecursoCultura
            Dim oRecurso As ELL.recurso = Nothing
            Dim reader As IDataReader = Nothing
            Try
                Dim recursosDAL As New DAL.RECURSOSCULTURAS

                Dim oRec As New ELL.recurso
                oRec.Id = idRecurso
                oRec.IdCultura = idCultura
                reader = recursosDAL.GetRecursosCultura(oRec)

                While reader.Read
                    oRecurso = New ELL.recurso
                    oRecurso.Id = CInt(reader.Item(DAL.RECURSOS.ColumnNames.ID))
                    oRecurso.IdCultura = reader.Item(DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.IDCULTURAS)
                    oRecurso.Nombre = reader.Item(DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.NOMBRE)
                    If (Not reader.IsDBNull(3)) Then oRecurso.IdParent = reader.Item(DAL.RECURSOS.ColumnNames.IDPARENT)
                    If (Not reader.IsDBNull(4)) Then oRecurso.Url = reader.Item(DAL.RECURSOS.ColumnNames.URL)
					If (Not reader.IsDBNull(5)) Then oRecurso.Imagen = CType(reader.Item(DAL.RECURSOS.ColumnNames.IMAGEN), Byte())
					oRecurso.Tipo = reader.Item(DAL.RECURSOS.ColumnNames.TIPO)
					If (Not reader.IsDBNull(7)) Then oRecurso.Descripcion = reader.Item(DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.DESCRIPCION)
					If (Not reader.IsDBNull(8)) Then oRecurso.Visible = reader.Item(DAL.RECURSOS.ColumnNames.VISIBLE)					
					If (Not reader.IsDBNull(9)) Then oRecurso.NombreFichero = reader.Item(DAL.RECURSOS.ColumnNames.NOMBRE_FICHERO)
					If (Not reader.IsDBNull(10)) Then oRecurso.Target = reader.Item(DAL.RECURSOS.ColumnNames.TARGET)
					Exit While
				End While

            Catch
                Return Nothing
            Finally
                If (reader IsNot Nothing) Then reader.close()
            End Try

            Return oRecurso

        End Function

        ''' <summary>
        ''' Obtiene los recursos que cumplan las condiciones
        ''' </summary>
        ''' <param name="oRecurso">Recurso</param>
        ''' <param name="bConIcono">Indicara si tiene que devolver el conjunto de bytes del icono</param>
        ''' <returns>Devuelve una lista con los recurso que cumplen la condicion</returns>
        Public Function GetRecursosCultura(ByVal oRecurso As ELL.recurso, Optional ByVal bConIcono As Boolean = False) As System.Collections.Generic.List(Of ELL.recurso) Implements [Interface].IRecursosComponent.GetRecursosCultura
            Dim reader As IDataReader = Nothing
            Try
                Dim recursosDAL As New DAL.RECURSOSCULTURAS
                Dim oRec As ELL.recurso
                Dim lRecursos As New List(Of ELL.recurso)
                reader = recursosDAL.GetRecursosCultura(oRecurso)

                While reader.Read
                    oRec = New ELL.recurso
                    oRec.Id = CInt(reader.Item(DAL.RECURSOS.ColumnNames.ID))
                    oRec.IdCultura = reader.Item(DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.IDCULTURAS)
                    oRec.Nombre = reader.Item(DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.NOMBRE)
                    If (Not reader.IsDBNull(3)) Then oRec.IdParent = reader.Item(DAL.RECURSOS.ColumnNames.IDPARENT)
                    If (Not reader.IsDBNull(4)) Then oRec.Url = reader.Item(DAL.RECURSOS.ColumnNames.URL)
					If (Not reader.IsDBNull(5)) Then oRec.Imagen = CType(reader.Item(DAL.RECURSOS.ColumnNames.IMAGEN), Byte())

					oRec.Tipo = reader.Item(DAL.RECURSOS.ColumnNames.TIPO)
					If (Not reader.IsDBNull(7)) Then oRec.Descripcion = reader.Item(DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.DESCRIPCION)
					If (Not reader.IsDBNull(8)) Then oRec.Visible = reader.Item(DAL.RECURSOS.ColumnNames.VISIBLE)
					'If (Not reader.IsDBNull(9)) Then oRec.Fichero = CType(reader.Item(DAL.RECURSOS.ColumnNames.FICHERO), Byte())
					If (Not reader.IsDBNull(9)) Then oRec.NombreFichero = reader.Item(DAL.RECURSOS.ColumnNames.NOMBRE_FICHERO)
					If (Not reader.IsDBNull(10)) Then oRec.Target = reader.Item(DAL.RECURSOS.ColumnNames.TARGET)

					lRecursos.Add(oRec)
				End While

                Return lRecursos
            Catch
                Return Nothing
            Finally
                If (reader IsNot Nothing) Then reader.Close()
            End Try

        End Function

        ''' <summary>
        ''' Obtiene todos los recursos de una cultura asignados a un grupo
        ''' </summary>
        ''' <param name="idGrupo">Grupo al que estan asignados</param>
        ''' <param name="IdCultura">Identificador de la cultura</param>
        ''' <returns>Lista de recursos</returns>
        Public Function GetRecursosCultura(ByVal idGrupo As Integer, ByVal IdCultura As String) As System.Collections.Generic.List(Of ELL.recurso) Implements [Interface].IRecursosComponent.GetRecursosCultura
            Dim recursoCulturaDAL As New DAL.Views.W_RECURSOS_GRUPO
            Dim recursoDAL As New DAL.RECURSOS
            Dim lRecursos As New List(Of ELL.recurso)
            Dim oRecurso As ELL.recurso = Nothing

            recursoCulturaDAL.Where.IDCULTURAS.Value = IdCultura
            recursoCulturaDAL.Where.IDGRUPOS.Value = idGrupo
            recursoCulturaDAL.Query.Load()
            If (recursoCulturaDAL.RowCount > 0) Then
                Do
                    oRecurso = New ELL.recurso()
                    oRecurso.Id = recursoCulturaDAL.IDRECURSOS

                    recursoDAL.FlushData()
                    recursoDAL.LoadByPrimaryKey(oRecurso.Id)
                    If (recursoDAL.RowCount = 1) Then
                        If Not (recursoDAL.OBSOLETO) Then
                            oRecurso = getObject(recursoDAL)
							oRecurso.Nombre = recursoCulturaDAL.NOMBRERECURSO						
                            lRecursos.Add(oRecurso)
                        End If
                    Else
                        Throw New Exception
                    End If

                Loop While recursoCulturaDAL.MoveNext
            End If

            Return lRecursos

		End Function

		''' <summary>
		''' Obtiene todos los recursos culturizados de un usuario
		''' </summary>
		''' <param name="idUser">Id del usuario</param>
		''' <param name="idCultura">Cultura</param>
		''' <returns>Devuelve una lista con los recurso que cumplen la condicion</returns>
		Public Function GetRecursosCulturaAll(ByVal idUser As Integer, ByVal idCultura As String) As System.Collections.Generic.List(Of ELL.recurso) Implements [Interface].IRecursosComponent.GetRecursosCulturaAll
			Dim reader As IDataReader = Nothing
			Try
				Dim recursosDAL As New DAL.RECURSOSCULTURAS
				Dim oRec As ELL.recurso
				Dim lRecursos As New List(Of ELL.recurso)
				reader = recursosDAL.GetRecursosCulturaAll(idUser, idCultura)

				While reader.Read
					oRec = New ELL.recurso
					oRec.Id = CInt(reader.Item(DAL.RECURSOS.ColumnNames.ID))
					oRec.IdCultura = idCultura
					oRec.Nombre = reader.Item(DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.NOMBRE)
					If (Not reader.IsDBNull(1)) Then oRec.Url = reader.Item(DAL.RECURSOS.ColumnNames.URL)
					If (Not reader.IsDBNull(3)) Then oRec.Descripcion = reader.Item(DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.DESCRIPCION)
					If (Not reader.IsDBNull(4)) Then oRec.Visible = reader.Item(DAL.RECURSOS.ColumnNames.VISIBLE)
					If (Not reader.IsDBNull(5)) Then oRec.IdParent = reader.Item(DAL.RECURSOS.ColumnNames.IDPARENT)
					'If (Not reader.IsDBNull(7)) Then oRec.Fichero = CType(reader.Item(DAL.RECURSOS.ColumnNames.FICHERO), Byte())
					If (Not reader.IsDBNull(7)) Then oRec.NombreFichero = reader.Item(DAL.RECURSOS.ColumnNames.NOMBRE_FICHERO)
					If (Not reader.IsDBNull(8)) Then oRec.Target = reader.Item(DAL.RECURSOS.ColumnNames.TARGET)
					oRec.Tipo = reader.Item(DAL.RECURSOS.ColumnNames.TIPO)

					lRecursos.Add(oRec)
				End While

				Return lRecursos
			Catch ex As Exception
				Return Nothing
			Finally
				If (reader IsNot Nothing) Then reader.Close()
			End Try

		End Function

        ''' <summary>
        ''' A partir de un objeto mygeneration, devuelve un objeto departamento
        ''' </summary>
        ''' <param name="recursoDAL"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function getObject(ByVal recursoDAL As DAL.RECURSOS) As ELL.recurso
            Dim oRecurso As New ELL.recurso
            oRecurso.Id = recursoDAL.s_ID
            If Not (recursoDAL.IsColumnNull(DAL.RECURSOS.ColumnNames.IDPARENT)) Then oRecurso.IdParent = recursoDAL.IDPARENT
            If Not (recursoDAL.IsColumnNull(DAL.RECURSOS.ColumnNames.OBSOLETO)) Then oRecurso.Obsoleto = recursoDAL.OBSOLETO
            oRecurso.Url = recursoDAL.s_URL
            oRecurso.Tipo = recursoDAL.TIPO            
			If Not (recursoDAL.IsColumnNull(DAL.RECURSOS.ColumnNames.IMAGEN)) Then oRecurso.Imagen = recursoDAL.IMAGEN
			If Not (recursoDAL.IsColumnNull(DAL.RECURSOS.ColumnNames.VISIBLE)) Then oRecurso.Visible = recursoDAL.VISIBLE
			If Not (recursoDAL.IsColumnNull(DAL.RECURSOS.ColumnNames.FICHERO)) Then oRecurso.Fichero = recursoDAL.FICHERO
			If Not (recursoDAL.IsColumnNull(DAL.RECURSOS.ColumnNames.NOMBRE_FICHERO)) Then oRecurso.NombreFichero = recursoDAL.NOMBRE_FICHERO
			oRecurso.Target = recursoDAL.s_TARGET

            Return oRecurso
        End Function

#End Region

#Region "Add"

        ''' <summary>
        ''' Añade recurso para todas las culturas existentes
        ''' Devuelve el id del recurso añadido. Devuelve -1 si ha habido algun error
        ''' </summary>
        ''' <param name="nombreRecurso">Nombre del recurso</param>
        ''' <param name="objRecurso">Objeto recurso que contiene los datos</param>
        ''' <returns>Identificador del nuevo recurso</returns>
        Function AddRecurso(ByVal nombreRecurso As String, ByVal objRecurso As ELL.recurso) As Integer Implements IRecursosComponent.AddRecurso
            Dim id As Integer = -1
            Dim recursos As New DAL.RECURSOS()
            Dim recursosCulturas As New DAL.RECURSOSCULTURAS()
            Dim culturas As New DAL.CULTURAS
            'Transaccion
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                culturas.LoadAll()
                'Empezar transacción
                tx.BeginTransaction()
                'Añadir recurso a Recursos
                recursos.AddNew()
                recursos.OBSOLETO = 0
                If objRecurso.Url.Length > 0 Then
                    recursos.URL = objRecurso.Url
                End If
                'If objRecurso.UrlImagen.Length > 0 Then
                '    recursos.URL_IMAGEN = objRecurso.UrlImagen
                'End If
                If (Not (objRecurso.Imagen Is Nothing) And (objRecurso.Imagen.Length > 0)) Then
                    recursos.IMAGEN = objRecurso.Imagen
				End If
				If (Not (objRecurso.Fichero Is Nothing) AndAlso (objRecurso.Fichero.Length > 0)) Then
					recursos.FICHERO = objRecurso.Fichero
					recursos.NOMBRE_FICHERO = objRecurso.NombreFichero
				End If
				recursos.VISIBLE = objRecurso.Visible
				recursos.TARGET = objRecurso.Target
                recursos.Save()

                If (recursos.RowCount = 1) Then
                    Do
                        'Añadir nombre de recurso a RecursosCulturas
                        recursosCulturas.AddNew()
                        recursosCulturas.IDRECURSOS = recursos.ID
                        recursosCulturas.IDCULTURAS = culturas.ID
						recursosCulturas.NOMBRE = nombreRecurso						
                        recursosCulturas.Save()
                    Loop While culturas.MoveNext()
                Else
                    Throw New Exception()
                End If
                tx.CommitTransaction()
                id = recursos.ID
            Catch ex As Exception
                'Rollback and reset transaction
                tx.RollbackTransaction()
                TransactionMgr.ThreadTransactionMgrReset()
            End Try
            Return id
        End Function


#End Region

#Region "Delete"

        ''' <summary>
        ''' Elimina el recurso especificado
        ''' </summary>
        ''' <param name="idRecurso">Identificador del recurso</param>
        ''' <returns>Booleano que indica si se ha eliminado correctamente</returns>
        Function Delete(ByVal idRecurso As Integer) As Boolean Implements IRecursosComponent.Delete
            Dim recurso As New DAL.RECURSOS()
            recurso.LoadByPrimaryKey(idRecurso)
            If recurso.RowCount = 1 Then
                recurso.OBSOLETO = 1
                Try
                    recurso.Save()
                Catch
                    Return False
                End Try
                Return True
            End If
            Return False
        End Function

        ''' <summary>
        ''' Elimina la imagen de un recurso
        ''' </summary>        
        ''' <param name="idRecurso">Identificador del recurso</param>        
        ''' <returns>Booleano que indica si se ha borrado la imagen correctatemente</returns>
        Function DeleteImageRecurso(ByVal idRecurso As Integer) As Boolean Implements IRecursosComponent.DeleteImageRecurso
            Try
                Dim recursosDAL As New DAL.RECURSOS
                recursosDAL.LoadByPrimaryKey(idRecurso)
                If recursosDAL.RowCount = 1 Then
                    recursosDAL.IMAGEN = Nothing
                    recursosDAL.Save()
                    Return True
                Else
                    Return False
                End If
            Catch
                Return False
            End Try
		End Function

		''' <summary>
		''' Elimina el fichero de un recurso
		''' </summary>        
		''' <param name="idRecurso">Identificador del recurso</param>        
		''' <returns>Booleano que indica si se ha borrado la imagen correctatemente</returns>
		Function DeleteFicheroRecurso(ByVal idRecurso As Integer) As Boolean Implements IRecursosComponent.DeleteFicheroRecurso
			Try
				Dim recursosDAL As New DAL.RECURSOS
				recursosDAL.LoadByPrimaryKey(idRecurso)
				If recursosDAL.RowCount = 1 Then
					recursosDAL.FICHERO = Nothing
					recursosDAL.s_NOMBRE_FICHERO = String.Empty
					recursosDAL.Save()
					Return True
				Else
					Return False
				End If
			Catch
				Return False
			End Try
		End Function

#End Region

#Region "Save"

        ''' <summary>
        ''' Guarda un recurso
        ''' </summary>
        ''' <param name="objRecurso">Objeto recurso que tiene los datos</param>
        ''' <returns>Entero con el id del recurso</returns>
        Function SaveRecurso(ByVal objRecurso As ELL.recurso) As Integer Implements IRecursosComponent.SaveRecurso
            Dim recursos As New DAL.RECURSOS()
            If (objRecurso.Id = Integer.MinValue) Then
                recursos.AddNew()
            Else
                recursos.LoadByPrimaryKey(objRecurso.Id)
            End If

            If recursos.RowCount = 1 Then
                recursos.URL = objRecurso.Url
                If (objRecurso.IdParent = Integer.MinValue) Then
                    recursos.IDPARENT = Nothing
                Else
                    recursos.IDPARENT = objRecurso.IdParent
                End If
                recursos.TIPO = objRecurso.Tipo
                'La imagen no se quitara a traves de este metodo. Existe otro para esto
				If (objRecurso.Imagen IsNot Nothing) Then recursos.IMAGEN = objRecurso.Imagen
				If (objRecurso.Fichero IsNot Nothing) Then
					recursos.FICHERO = objRecurso.Fichero
					recursos.NOMBRE_FICHERO = objRecurso.NombreFichero
				End If

                If (objRecurso.Obsoleto = Integer.MinValue) Then
                    recursos.OBSOLETO = 0
                Else
                    recursos.OBSOLETO = 1
				End If
				recursos.VISIBLE = objRecurso.Visible
				recursos.TARGET = objRecurso.Target
                Try
                    recursos.Save()
                Catch ex As Exception
                    Return Integer.MinValue
                End Try
                Return recursos.ID
            Else
                Return Integer.MinValue
            End If
        End Function

        ''' <summary>
        ''' Inserta o modifica el termino del recurso en la cultura especificada
        ''' </summary>
        ''' <param name="oRecurso">Recurso</param>
        ''' <param name="bnuevo">Indica si hay que insertar o modificar</param>
        ''' <returns>Booleano</returns>
        Public Function SaveRecursoCultura(ByVal oRecurso As ELL.recurso, ByVal bnuevo As Boolean) As Boolean Implements [Interface].IRecursosComponent.SaveRecursoCultura
            Try
                Dim recCultcomp As New DAL.RECURSOSCULTURAS
                If (bnuevo) Then
                    recCultcomp.AddNew()
                Else
                    recCultcomp.LoadByPrimaryKey(oRecurso.IdCultura, oRecurso.Id)
                End If

                If (recCultcomp.RowCount = 1) Then
                    recCultcomp.IDCULTURAS = oRecurso.IdCultura
                    recCultcomp.IDRECURSOS = oRecurso.Id
                    recCultcomp.NOMBRE = oRecurso.Nombre
                    recCultcomp.DESCRIPCION = oRecurso.Descripcion
                    recCultcomp.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Guarda las traducciones de las distintas culturas de un recurso y el resto de datos
        ''' Si viene informado una traduccion y no existe se inserta, sino, se actualiza
        ''' </summary>
        ''' <param name="lRecursos">Lista de recursos a modificar</param>
        ''' <param name="oRecurso">Recurso con los datos</param>
        ''' <returns>Devuelve el idGrupo</returns>  
        Public Function ActualizarRecursosCultura(ByVal lRecursos As System.Collections.Generic.List(Of ELL.recurso), ByVal oRecurso As ELL.recurso) As Integer Implements [Interface].IRecursosComponent.ActualizarRecursosCultura
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                Dim lRecs As List(Of ELL.recurso)
                Dim idRecurso As Integer
                Dim oRecAux As ELL.recurso
                Dim bnuevo As Boolean = False

                tx.BeginTransaction()

                If (oRecurso.Id = Integer.MinValue) Then
                    bnuevo = True
                End If
                idRecurso = SaveRecurso(oRecurso)
                If (idRecurso = Integer.MinValue) Then Throw New Exception


                For Each oRec As ELL.recurso In lRecursos
                    'Si el recurso es nuevo, se asigna el recurso insertado arriba
                    If (bnuevo) Then oRec.Id = idRecurso

                    'Se consulta si esta el recurso sin el nombre
                    oRecAux = New ELL.recurso
                    oRecAux.IdCultura = oRec.IdCultura
                    oRecAux.Id = oRec.Id
                    lRecs = GetRecursosCultura(oRecAux)

                    If Not SaveRecursoCultura(oRec, Not (lRecs IsNot Nothing AndAlso lRecs.Count > 0)) Then Throw New Exception
                Next

                tx.CommitTransaction()
                Return idRecurso
            Catch ex As Exception
                tx.RollbackTransaction()
                Return Integer.MinValue
            End Try
        End Function

#End Region

    End Class
End Namespace
