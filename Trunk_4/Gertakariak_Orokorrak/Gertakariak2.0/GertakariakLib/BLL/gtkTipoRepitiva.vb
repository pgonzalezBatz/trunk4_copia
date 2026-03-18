Imports AccesoAutomaticoBD
Imports System.Web

Namespace BLL
    Public Class gtkTipoRepitivaComponent

        Sub New()
            'Inicializamos la cultura a la actual del sistema.
            If HttpContext.Current.Session("Culturas") Is Nothing Then
                Kultura.Add(System.Threading.Thread.CurrentThread.CurrentCulture.Name)
            Else
                Kultura = CType(HttpContext.Current.Session("Culturas"), ArrayList)
            End If
        End Sub
        Public Sub New(ByVal kulturas As ArrayList)
            _kultura = kulturas
        End Sub

        ''' <summary>
        ''' Vector de "Rango de Idiomas".
        ''' </summary>
        Private _kultura As New ArrayList
        ''' <summary>
        ''' Vector de "Rango de Idiomas".
        ''' </summary>
        Public Property Kultura() As ArrayList
            Get
                Return _kultura
            End Get
            Set(ByVal value As ArrayList)
                _kultura = value
            End Set
        End Property

        ''' <summary>
        ''' Funcion que devuelve todas los Tipos de Repetitivas y un contador para cada una de ellos, informando el numero de incidencias que estan asociadas a dicho tipo
        ''' </summary>
        ''' <param name="arrFamilia">Vector de Identificadores de la familia</param>
        ''' <returns>Listado de tiposRepetitivos</returns>
        ''' <remarks></remarks>
        Public Function fgTiposRepetitivasContador(ByVal arrFamilia As ArrayList) As List(Of ELL.gtkTipoRepetitiva)
            Dim listaTipos As New List(Of ELL.gtkTipoRepetitiva)
            Dim tiposDAL As New DAL.TIPOREPETITIVA
            Dim tiposKulturaDAL As DAL.TIPOREPETITIVAKULTURA
            Dim dr As IDataReader = Nothing
            Dim oTipoRep As ELL.gtkTipoRepetitiva
            Dim stLiteral As Utils.Literal

            Try
                For Each Id As Integer In arrFamilia
                    dr = tiposDAL.fgTiposRepetitivasContador(Id)
                    While dr.Read
                        oTipoRep = New ELL.gtkTipoRepetitiva
                        oTipoRep.Id = CInt(dr(ELL.gtkTipoRepetitiva.ColumnNames.ID))
                        oTipoRep.NumeroRegistros = dr(ELL.gtkTipoRepetitiva.ColumnNames.NUMERO_REGISTROS)
                        oTipoRep.Orden = Utils.integerNull(dr(DAL.TIPOREPETITIVA.ColumnNames.ORDEN))
                        oTipoRep.IdFamiliaRepetitiva = Id

                        'Obtenemos el campo de la descripcion traducido
                        tiposKulturaDAL = getTiposKulturasByIdTipo(oTipoRep.Id)
                        If (tiposKulturaDAL.RowCount > 0) Then
                            stLiteral = Utils.TraducirCampo(tiposKulturaDAL, DAL.TIPOREPETITIVAKULTURA.ColumnNames.DESCRIPCION.ToString, DAL.TIPOREPETITIVAKULTURA.ColumnNames.IDCULTURA.ToString, Me.Kultura)
                            oTipoRep.Descripcion = stLiteral.Descripcion
                            oTipoRep.Kultura = stLiteral.IdCultura
                        End If
                        listaTipos.Add(oTipoRep)
                    End While
                    dr.Close()
				Next
				'Catch ex As BatzException
				'	throw 
			Catch ex As Exception
				'Throw New BatzException(ex.Message.ToString, ex) 'Throw New BatzException("error", ex)
				throw 
            Finally
                If Not (dr Is Nothing) Then
                    dr.Close()
                End If
            End Try

            Return listaTipos
        End Function

        Private Function getTiposKulturasByIdTipo(ByVal idTipo As Integer) As DAL.TIPOREPETITIVAKULTURA
            Dim tipoKulturaDAL As New DAL.TIPOREPETITIVAKULTURA
            tipoKulturaDAL.Where.IDREPETITIVA.Operator = WhereParameter.Operand.Equal
            tipoKulturaDAL.Where.IDREPETITIVA.Value = idTipo
            tipoKulturaDAL.Query.Load()

            Return tipoKulturaDAL
        End Function

        ''' <summary>
        ''' Funcion para la insercion de Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de los Tipos de Incidencia Repetitiva (gtkTipoRepetitiva).</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Insertar(ByVal gtkObjeto As ELL.gtkTipoRepetitiva) As ELL.gtkTipoRepetitiva
            gtkObjeto = fGeneral(ELL.Acciones.Insertar, gtkObjeto)
            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Funcion general para Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="Accion">Accion a realizar con el Objeto (Insertar, Modificar, Consultar, Borrar) [ELL.Acciones]</param>
        ''' <param name="gtkObjeto">Devuelve un Objeto gtkTipoRepetitiva</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkTipoRepetitiva) As ELL.gtkTipoRepetitiva
            Dim dbTipoRepetitiva As New GertakariakLib.DAL.TIPOREPETITIVA  'Tabla de TIPOREPETITIVA (DataBase - db)
            Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                '-----------------------------------------------------------------
                '- Comprobamos que el termino que se pasa exista en la BB.DD -----
                '-----------------------------------------------------------------
                If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
                    Select Case Accion
                        Case ELL.Acciones.Insertar
                            dbTipoRepetitiva.Where.ID.Value = gtkObjeto.Id
                            dbTipoRepetitiva.Query.Load()
                            If Not dbTipoRepetitiva.EOF Then
                                'Si existe lo Modificamos
                                Accion = ELL.Acciones.Modificar
                            Else
                                'Si no existe lo Insertamos
                                Accion = ELL.Acciones.Insertar
                            End If
                            dbTipoRepetitiva.FlushData()
                    End Select
                Else
                    'Si no existe lo Insertamos
                    Accion = ELL.Acciones.Insertar
                End If
                '-----------------------------------------------------------------
                '-----------------------------------------------------------------
                'Inicio de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.BeginTransaction()
                End Select
                '-----------------------------------------------------------------
                '-----------------------------------------------------------------
                'Tabla de TIPOREPETITIVA
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbTipoRepetitiva.AddNew()
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        'Cargamos el registro con el que vamos a trabajar.
                        dbTipoRepetitiva.Where.ID.Value = gtkObjeto.Id
                        dbTipoRepetitiva.Query.Load()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If gtkObjeto.Orden = Integer.MinValue Then dbTipoRepetitiva.s_ORDEN = String.Empty Else dbTipoRepetitiva.ORDEN = gtkObjeto.Orden
                        If gtkObjeto.IdFamiliaRepetitiva < 0 Then dbTipoRepetitiva.s_IDFAMILIAREPETITIVA = String.Empty Else dbTipoRepetitiva.IDFAMILIAREPETITIVA = gtkObjeto.IdFamiliaRepetitiva
                        dbTipoRepetitiva.OBSOLETO = gtkObjeto.Obsoleto
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbTipoRepetitiva.IsColumnNull(DAL.TIPOREPETITIVA.ColumnNames.ORDEN) Then gtkObjeto.Orden = dbTipoRepetitiva.ORDEN
                        If Not dbTipoRepetitiva.IsColumnNull(DAL.TIPOREPETITIVA.ColumnNames.IDFAMILIAREPETITIVA) Then gtkObjeto.IdFamiliaRepetitiva = dbTipoRepetitiva.IDFAMILIAREPETITIVA
                        gtkObjeto.Obsoleto = dbTipoRepetitiva.OBSOLETO
                        '---------------------------------------------------------------------------
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbTipoRepetitiva.Save()
                        gtkObjeto.Id = dbTipoRepetitiva.ID
                End Select
                '-----------------------------------------------------------------

                '-----------------------------------------------------------------
                'Tabla de TIPOREPETITIVAKULTURA
                '-----------------------------------------------------------------
                Dim dbTipoRepetitivaKul As New DAL.TIPOREPETITIVAKULTURA
                Select Case Accion
                    Case ELL.Acciones.Consultar
                        If Not gtkObjeto.Kultura Is Nothing Then
                            gtkObjeto = fgKultura(Accion, gtkObjeto)
                        Else
                            Dim CampoTraducido As New GertakariakLib.BLL.Utils.Literal
                            dbTipoRepetitivaKul.Where.IDREPETITIVA.Value = gtkObjeto.Id
                            dbTipoRepetitivaKul.Query.Load()
                            CampoTraducido = GertakariakLib.BLL.Utils.TraducirCampo(dbTipoRepetitivaKul, DAL.TIPOREPETITIVAKULTURA.ColumnNames.DESCRIPCION.ToString, DAL.TIPOREPETITIVAKULTURA.ColumnNames.IDCULTURA.ToString, Me.Kultura)
                            gtkObjeto.Descripcion = CampoTraducido.Descripcion
                            gtkObjeto.Kultura = CampoTraducido.IdCultura
                        End If
                    Case ELL.Acciones.Modificar, ELL.Acciones.Insertar, ELL.Acciones.Borrar
                        fgKultura(Accion, gtkObjeto)
                End Select
                Select Case Accion
                    Case ELL.Acciones.Borrar
                        '-- Comprobamos que queden culturas para el registro.   --------------
                        '-- Si no eleminamos es registro conrrespondiente.      --------------
                        dbTipoRepetitivaKul.Where.IDREPETITIVA.Value = gtkObjeto.Id
                        dbTipoRepetitivaKul.Query.Load()
                        If dbTipoRepetitivaKul.EOF Then
                            dbTipoRepetitiva.DeleteAll()
                            dbTipoRepetitiva.Save()
                        End If
                        '---------------------------------------------------------------------
                End Select
                '-----------------------------------------------------------------

                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.CommitTransaction()
                End Select
                '-----------------------------------------------------------------
				'Catch ex As BatzException
				'    throw 
            Catch ex As Exception
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.RollbackTransaction()
                End Select
                gtkObjeto = Nothing
                '-----------------------------------------------------------------
				'Throw New BatzException(ex.Message.ToString, ex) 'Throw New BatzException("error", ex)
				throw 
			End Try
			Return gtkObjeto
		End Function

		''' <summary>
		''' Función General para el tratamiento de la cultura.
		''' </summary>
		''' <param name="Accion">Accion a realizar con el Objeto (Insertar, Modificar, Consultar, Borrar) [ELL.Acciones]</param>
		''' <param name="gtkObjeto">ELL.gtkTipoRepetitiva</param>
		''' <returns>ELL.gtkFamiliaRepetitiva</returns>
		''' <remarks></remarks>
		Private Function fgKultura(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkTipoRepetitiva) As ELL.gtkTipoRepetitiva
			Dim dbTablaKultura As New GertakariakLib.DAL.TIPOREPETITIVAKULTURA	'Tabla de TIPOREPETITIVAKULTURA (DataBase - db)
			Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
			Try
				'-----------------------------------------------------------------
				'- Comprobamos que el termino que se pasa exista en la BB.DD -----
				'-----------------------------------------------------------------
				If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
					Select Case Accion
						Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
							dbTablaKultura.Where.IDREPETITIVA.Value = gtkObjeto.Id
							dbTablaKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
							dbTablaKultura.Query.Load()
							If Not dbTablaKultura.EOF Then
								'Si existe lo Modificamos
								Accion = ELL.Acciones.Modificar
							Else
								'Si no existe lo Insertamos
								Accion = ELL.Acciones.Insertar
							End If
					End Select
				End If
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar
						'Para hacer insert
						dbTablaKultura.AddNew()
					Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						'Cargamos el registro con el que vamos a trabajar.
						dbTablaKultura.FlushData()
						dbTablaKultura.Where.IDREPETITIVA.Value = gtkObjeto.Id
						dbTablaKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
						dbTablaKultura.Query.Load()
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'---------------------------------------------------------------------------
						'Campos a Insertar o Modificar
						'---------------------------------------------------------------------------
						If Not gtkObjeto.Descripcion Is Nothing Then dbTablaKultura.DESCRIPCION = gtkObjeto.Descripcion.ToString.Trim
						If gtkObjeto.Id <> Integer.MinValue Then dbTablaKultura.IDREPETITIVA = gtkObjeto.Id.ToString.Trim
						If Not gtkObjeto.Kultura Is Nothing Then dbTablaKultura.IDCULTURA = gtkObjeto.Kultura.ToString.Trim
						'---------------------------------------------------------------------------
					Case ELL.Acciones.Consultar
						'---------------------------------------------------------------------------
						'Campos a devolver en la consulta
						'---------------------------------------------------------------------------
						If Not dbTablaKultura.IsColumnNull(DAL.TIPOREPETITIVAKULTURA.ColumnNames.DESCRIPCION) Then gtkObjeto.Descripcion = dbTablaKultura.DESCRIPCION
						'---------------------------------------------------------------------------
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'Realizamos la accion correspondiente (Insertar, Modificar)
						dbTablaKultura.Save()
					Case ELL.Acciones.Borrar
						dbTablaKultura.DeleteAll()
						dbTablaKultura.Save()
				End Select

				Select Case Accion
					Case ELL.Acciones.Borrar
						'- Comprobamos si existe algun registro cultural.
						dbTablaKultura.FlushData()
						dbTablaKultura.Where.IDREPETITIVA.Value = gtkObjeto.Id
						dbTablaKultura.Query.Load()
						'- Si no existe eliminamos el registro de la tabla principal.
						If dbTablaKultura.EOF Then
							Dim dbTablaPrincipal As New GertakariakLib.DAL.TIPOREPETITIVA  'Tabla de TIPOREPETITIVA (DataBase - db)
							dbTablaPrincipal.Where.ID.Value = gtkObjeto.Id
							dbTablaPrincipal.DeleteAll()
							dbTablaPrincipal.Save()
						End If
				End Select

			Catch ex As Exception
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				'Throw New BatzException("error", ex)
				'Throw New BatzException(ex.Message.ToString, ex)
				throw 
			End Try
			Return gtkObjeto
		End Function
		''' <summary>
		''' Consultamos Tipo de Incidencia Repetitiva.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de Tipos de Incidencia Repetitiva (gtkTipoRepetitiva).</param>
		''' <returns>Devuleve un objeto gtkTipoRepetitiva.</returns>

		Public Function Consultar(ByVal gtkObjeto As ELL.gtkTipoRepetitiva) As ELL.gtkTipoRepetitiva
			Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
		End Function
		''' <summary>
		''' Consultamos los Tipos de Incidencia Repetitiva.
		''' </summary>
		''' <param name="Id">Identificador del registro.</param>
		''' <returns>Devuleve una lista de objetos gtkTipoRepetitiva.</returns>
		''' <remarks></remarks>

		Public Function Consultar(ByVal Id As Integer) As List(Of ELL.gtkTipoRepetitiva)
			Dim gtkObjeto As New ELL.gtkTipoRepetitiva
			gtkObjeto.Id = Id
			Return ConsultarListado(gtkObjeto)
		End Function
		''' <summary>
		''' Consultamos los Tipos de Incidencia Repetitiva.
		''' </summary>
		''' <param name="arrIDsFamilia">Vector de identificadores de la Familia de los Tipos de Incidencia Repetitiva.</param>
		''' <returns>Devuleve una lista de objetos gtkTipoRepetitiva.</returns>
		''' <remarks></remarks>
		Public Function Consultar(Optional ByVal arrIDsFamilia As ArrayList = Nothing) As List(Of ELL.gtkTipoRepetitiva)
			If arrIDsFamilia Is Nothing Then
				arrIDsFamilia = New ArrayList
				Dim gtkFRComponent As New BLL.gtkFamiliaRepetitivaComponent
				For Each gtkFamiliaRepetitiva As ELL.gtkFamiliaRepetitiva In gtkFRComponent.Consultar()
					arrIDsFamilia.Add(gtkFamiliaRepetitiva.Id)
				Next
			End If
			Return fgTiposRepetitivasContador(arrIDsFamilia)
		End Function
		''' <summary>
		''' Consultamos los Tipos de Incidencia Repetitiva.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de los Tipos de Incidencia Repetitiva (gtkTipoRepetitiva).</param>
		''' <returns>Devuleve un objeto de gtkTipoRepetitiva.</returns>
		Private Function ConsultarListado(ByVal gtkObjeto As ELL.gtkTipoRepetitiva) As List(Of ELL.gtkTipoRepetitiva)
			Dim lstObjeto As New List(Of GertakariakLib.ELL.gtkTipoRepetitiva)
			Dim dbKultura As New DAL.TIPOREPETITIVAKULTURA
			If gtkObjeto.Id <> Integer.MinValue Then dbKultura.Where.IDREPETITIVA.Value = gtkObjeto.Id
			If Not gtkObjeto.Kultura Is Nothing Then dbKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
			dbKultura.Query.Load()
			If dbKultura.RowCount > 0 Then
				Do
					Dim nObjeto As New ELL.gtkTipoRepetitiva
					nObjeto.Id = dbKultura.IDREPETITIVA
					nObjeto.Kultura = dbKultura.IDCULTURA
					lstObjeto.Add(fGeneral(ELL.Acciones.Consultar, nObjeto))
				Loop While dbKultura.MoveNext
			Else
				gtkObjeto = fGeneral(ELL.Acciones.Consultar, gtkObjeto)
				lstObjeto.Add(gtkObjeto)
			End If
			Return lstObjeto
		End Function

		''' <summary>
		''' Funcion para la modificacion de los Tipos de Incidencia Repetitiva.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de Tipos de Incidencia Repetitiva (gtkTipoRepetitiva).</param>
		''' <returns>ELL.gtkTipoRepetitiva</returns>
		''' <remarks></remarks>
		Private Function Modificar(ByVal gtkObjeto As ELL.gtkTipoRepetitiva) As ELL.gtkTipoRepetitiva
			Return fGeneral(ELL.Acciones.Modificar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion para la eliminacion de los Tipos de Incidencia Repetitiva.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de Tipos de Incidencia Repetitiva (gtkTipoRepetitiva).</param>
		''' <returns>ELL.gtkTipoRepetitiva</returns>
		''' <remarks></remarks>
		Public Function Borrar(ByVal gtkObjeto As ELL.gtkTipoRepetitiva) As ELL.gtkTipoRepetitiva
			Return fGeneral(ELL.Acciones.Borrar, gtkObjeto)
		End Function
		''' <summary>
		''' Funcion para la eliminacion de los Tipos de Incidencia Repetitiva.
		''' </summary>
		''' <param name="Id">Identificador del Registro.</param>
		''' <remarks></remarks>
		Public Sub Borrar(ByVal Id As Integer)
			Dim Lista As New List(Of ELL.gtkTipoRepetitiva)
			Try
				If Contador(Id) <> 0 Then
					'Throw New System.Exception("errBorrarRelacionado")
					Throw New ApplicationException("errBorrarRelacionado")
				End If
				'-- Borramos el registro cultura a cultura --
				Lista = Consultar(Id)
				For Each Item As ELL.gtkTipoRepetitiva In Lista
					Borrar(Item)
				Next
				'--------------------------------------------
			Catch ex As ApplicationException
				throw 
				'Catch ex As BatzException
				'	throw 
			Catch ex As Exception
				throw 
			End Try
		End Sub

		''' <summary>
		''' Pasamos una estructura de tipo gtkMantenimiento con la accion que se quiere realizar
		''' </summary>
		''' <param name="gtkObjetoList">List(Of ELL.gtkMantenimiento)</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Mantenimiento(ByVal gtkObjetoList As List(Of ELL.gtkMantenimiento)) As List(Of ELL.gtkMantenimiento)
			Dim IdReg As Integer 'Identificador del registro
			Try
				For Each gtkMantenimiento As ELL.gtkMantenimiento In gtkObjetoList
					If gtkMantenimiento.Accion = ELL.Acciones.Insertar And gtkMantenimiento.Objeto.Descripcion = "" Then
						Continue For
					End If
					If gtkMantenimiento.Objeto.id = Integer.MinValue And IdReg <> 0 Then
						gtkMantenimiento.Objeto.id = IdReg
						Select Case gtkMantenimiento.Accion
							Case ELL.Acciones.Insertar
								gtkMantenimiento.Accion = ELL.Acciones.Modificar
						End Select
					End If
					Select Case gtkMantenimiento.Accion
						Case ELL.Acciones.Borrar
							gtkMantenimiento.Objeto = Me.Borrar(CType(gtkMantenimiento.Objeto, ELL.gtkTipoRepetitiva))
						Case ELL.Acciones.Consultar
							gtkMantenimiento.Objeto = Me.Consultar(gtkMantenimiento.Objeto)
						Case ELL.Acciones.Insertar
							gtkMantenimiento.Objeto = Me.Insertar(gtkMantenimiento.Objeto)
							IdReg = gtkMantenimiento.Objeto.id
						Case ELL.Acciones.Modificar
							gtkMantenimiento.Objeto = Me.Modificar(gtkMantenimiento.Objeto)
					End Select
				Next
				'Catch ex As BatzException
				'	throw 
			Catch ex As Exception
				'Throw New BatzException(ex.Message.ToString, ex) 'Throw New BatzException("error", ex)
				throw 
			End Try
			Return gtkObjetoList
		End Function

		''' <summary>
		''' Contador de Registros de Tipos de Repetitiva que estan relacionados con las Incidencias.
		''' </summary>
		''' <param name="Id">Identificador de Tipos de Repetitiva a contar.</param>
		''' <returns>Numero de registros que existen</returns>
		''' <remarks></remarks>
		Public Function Contador(ByVal Id As Integer) As Integer
			Dim bdTipoRepetitiva As New DAL.TIPOREPETITIVA
			Return bdTipoRepetitiva.Contador(Id)
		End Function
	End Class

End Namespace