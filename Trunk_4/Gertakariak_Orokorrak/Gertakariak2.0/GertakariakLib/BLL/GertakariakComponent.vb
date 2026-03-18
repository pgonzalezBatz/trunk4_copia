Imports AccesoAutomaticoBD
Imports System.Web

Namespace BLL

    Public Class GertakariakComponent
        Implements GertakariakLib.BLL.IGertakariakComponent

        Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.GertakariakComponent")

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
        ''' Clase de la incidencia que usamos.
        ''' </summary>
        ''' <remarks></remarks>
        Enum ClaseIncidencia As Integer
            ''' <summary>
            ''' Incidencia del tipo de Troqueleria.
            ''' </summary>
            gtkTroqueleria = 1
            ''' <summary>
            ''' Incidencia del tipo de Servicios Generales.
            ''' </summary>
            gtkServiciosGenerales = 2
            ''' <summary>
            ''' Incidencia del tipo de Txokos.
            ''' </summary>
            gtkTxokos = 3
            ''' <summary>
            ''' Incidencia del tipo de Sugerencias.
            ''' </summary>
            gtkSugerencias = 4
        End Enum

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
        ''' Funcion para insertar Incidencias.
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <returns>
        ''' True --&gt; Si la insercion es correcta.
        ''' False --&gt;Si la inserccion es erronea.
        ''' </returns>
        ''' <remarks>No importa de que tipo sea la Incidencia puesto que la funcion la identifica.</remarks>
        Public Function Insertar(ByVal Gertakaria As Object) As Object Implements IGertakariakComponent.Insertar
            'Dependiendo del Tipo de Clase de la incidencia instanciamos el objeto de diferente forma.
            Select Case Gertakaria.GetType.Name
                Case ClaseIncidencia.gtkTroqueleria.ToString
                    Return fgTroqueleria(Gertakaria, ELL.Acciones.Insertar)
            End Select
            Return Gertakaria
        End Function
        ''' <summary>
        ''' Funcion para Modificar incidencias
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Modificar(ByVal Gertakaria As Object) As Object Implements IGertakariakComponent.Modificar
            'Dependiendo del Tipo de Clase de la incidencia instanciamos el objeto de diferente forma.
            Select Case Gertakaria.GetType.Name
                Case ClaseIncidencia.gtkTroqueleria.ToString
                    Return fgTroqueleria(Gertakaria, ELL.Acciones.Modificar)
            End Select
            Return Gertakaria
        End Function
        ''' <summary>
        ''' Funcion de consulta de Incidencias.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Public Overloads Function Consultar(ByVal Gertakaria As Object) As List(Of Object) Implements IGertakariakComponent.Consultar
			Dim lGertakariak As New List(Of Object)
			'Dependiendo del Tipo de Clase de la incidencia instanciamos el objeto de diferente forma.
			Select Case Gertakaria.GetType.Name
				Case ClaseIncidencia.gtkTroqueleria.ToString
					lGertakariak = fgGertakariak(CType(Gertakaria, ELL.gtkTroqueleria))
				Case ClaseIncidencia.gtkServiciosGenerales.ToString
					lGertakariak = fgGertakariak(CType(Gertakaria, ELL.gtkServiciosGenerales))
			End Select
			Return lGertakariak
		End Function
        ''' <summary>
        ''' Funcion de consulta de Incidencias.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Consultar(ByVal tIncidencia As ClaseIncidencia) As System.Collections.Generic.List(Of Object) Implements IGertakariakComponent.Consultar
            Dim lGertakariak As New List(Of Object)
			'Dependiendo del Tipo de Clase de la incidencia instanciamos el objeto de diferente forma.
            Select Case tIncidencia
                Case ClaseIncidencia.gtkTroqueleria, ClaseIncidencia.gtkSugerencias
                    Dim Gertakaria As New ELL.gtkTroqueleria
                    lGertakariak = fgGertakariak(Gertakaria)
                Case ClaseIncidencia.gtkServiciosGenerales
                    Dim Gertakaria As New ELL.gtkServiciosGenerales
                    lGertakariak = fgGertakariak(Gertakaria)
            End Select
            Return lGertakariak
        End Function
        ''' <summary>
        ''' Funcion para la eliminacion de Incidencias.
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Borrar(ByVal Gertakaria As Object) As Object Implements IGertakariakComponent.Borrar
            'Dependiendo del Tipo de Clase de la incidencia instanciamos el objeto de diferente forma.
            Select Case Gertakaria.GetType.Name
                Case ClaseIncidencia.gtkTroqueleria.ToString
                    Return fgTroqueleria(Gertakaria, ELL.Acciones.Borrar)
            End Select
            Return Gertakaria
        End Function

        ''' <summary>
        ''' Devuelve una lista de incidencia para el objeto pasado
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Overloads Function fgGertakariak(ByVal Gertakaria As ELL.gtkTroqueleria) As List(Of Object)
            Dim lGertakariak As New List(Of Object)
            Dim dbGertakaria As New GertakariakLib.DAL.GERTAKARIAK  'Tabla de Incidencias (DataBase - db)

            If Gertakaria.Id <> Integer.MinValue Then
                dbGertakaria.Where.ID.Value = Gertakaria.Id
            End If
            dbGertakaria.Query.Load()
            If Not dbGertakaria.EOF Then
                Do
                    Dim nGertakaria As New ELL.gtkTroqueleria
                    nGertakaria.Id = dbGertakaria.ID
                    lGertakariak.Add(fgTroqueleria(nGertakaria, ELL.Acciones.Consultar))
                Loop While dbGertakaria.MoveNext
            Else
                lGertakariak = Nothing
            End If

            Return lGertakariak
        End Function

        ''' <summary>
        ''' Devuelve una lista de incidencia para el objeto pasado
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Overloads Function fgGertakariak(ByVal Gertakaria As ELL.gtkServiciosGenerales) As List(Of Object)
            Dim lGertakariak As New List(Of Object)
            Dim dbGertakaria As New GertakariakLib.DAL.GERTAKARIAK  'Tabla de Incidencias (DataBase - db)

            dbGertakaria.Where.IDTIPOINCIDENCIA.Value = BLL.GertakariakComponent.ClaseIncidencia.gtkServiciosGenerales   'Tipo de incidencia perteneciente a Servicos Generales
            If Gertakaria.Id <> Integer.MinValue Then
                dbGertakaria.Where.ID.Value = Gertakaria.Id
            End If
            dbGertakaria.Query.Load()
            If Not dbGertakaria.EOF Then
                Do
                    Dim nGertakaria As New ELL.gtkServiciosGenerales
                    nGertakaria.Id = dbGertakaria.ID
                    lGertakariak.Add(fgServiciosGenerales(Gertakaria, ELL.Acciones.Consultar))
                Loop While dbGertakaria.MoveNext
            End If

            lGertakariak = fgGertakariak(Gertakaria)
            Return lGertakariak
        End Function

        ''' <summary>
        ''' Funcion general para las incidencias de Troqueleria.
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <param name="Accion"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function fgTroqueleria(ByVal Gertakaria As GertakariakLib.ELL.gtkTroqueleria, ByVal Accion As ELL.Acciones) As GertakariakLib.ELL.gtkTroqueleria
            Dim dbGertakaria As New GertakariakLib.DAL.GERTAKARIAK  'Tabla de Incidencias (DataBase - db)
            Dim dbGertakariak_TipoNoConformidad As New GertakariakLib.DAL.GERTAKARIAK_TIPONOCONFORMIDAD   'Tabla de GERTAKARIAK_TIPONOCONFORMIDAD
            Dim dbGertakariak_Irudiak As New GertakariakLib.DAL.GERTAKARIAK_IRUDIAK   'Tabla de GERTAKARIAK_IRUDIAK
            Dim dbTipoRepetitiva As New GertakariakLib.DAL.TIPOREPETITIVA   'Tabla de TipoRepetitiva

            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr

            Try
                '-----------------------------------------------------------------
                'Inicio de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        tx.BeginTransaction()
                End Select
                '-----------------------------------------------------------------

                '-----------------------------------------------------------------
                'Tabla de Gertakariak
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbGertakaria.AddNew()
                    Case ELL.Acciones.Modificar, ELL.Acciones.Borrar, ELL.Acciones.Consultar
                        'Cargamos el registro con el que vamos a trabajar por la clave primaria.
                        dbGertakaria.LoadByPrimaryKey(Gertakaria.Id)
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If Not Gertakaria.CapacidadProveedor Is Nothing Then dbGertakaria.CAPID = Gertakaria.CapacidadProveedor
                        If Gertakaria.FechaApertura <> DateTime.MinValue Then dbGertakaria.FECHAAPERTURA = Gertakaria.FechaApertura Else dbGertakaria.s_FECHAAPERTURA = String.Empty
                        If Gertakaria.FechaCierre <> DateTime.MinValue Then dbGertakaria.FECHACIERRE = Gertakaria.FechaCierre Else dbGertakaria.s_FECHACIERRE = String.Empty
                        If Not Gertakaria.Descripcion Is Nothing Then dbGertakaria.DESCRIPCIONPROBLEMA = Gertakaria.Descripcion.ToString.Trim
                        If Not Gertakaria.Causa Is Nothing Then dbGertakaria.CAUSAPROBLEMA = Gertakaria.Causa.ToString.Trim
                        If Gertakaria.IdTipoIncidencia <> Integer.MinValue Then dbGertakaria.IDTIPOINCIDENCIA = Gertakaria.IdTipoIncidencia
                        If Gertakaria.NumPedCab <> Integer.MinValue Then dbGertakaria.NUMPEDCAB = Gertakaria.NumPedCab Else dbGertakaria.s_NUMPEDCAB = String.Empty
                        If Gertakaria.ProcedenciaNC <> Integer.MinValue Then dbGertakaria.PROCEDENCIANC = Gertakaria.ProcedenciaNC Else dbGertakaria.s_PROCEDENCIANC = String.Empty
                        If Not Gertakaria.ObservacionesCoste Is Nothing Then dbGertakaria.OBSERVACIONESCOSTE = Gertakaria.ObservacionesCoste
                        If Not Gertakaria.Titulo Is Nothing Then dbGertakaria.TITULO = Gertakaria.Titulo.ToString.Trim
                        If Gertakaria.TotalAcordado <> Decimal.MinValue Then dbGertakaria.TOTALACORDADO = Gertakaria.TotalAcordado Else dbGertakaria.s_TOTALACORDADO = Decimal.Zero
                        If Gertakaria.IdProveedor IsNot Nothing Then
                            dbGertakaria.IDPROVEEDOR = Gertakaria.IdProveedor
                        Else
                            dbGertakaria.s_IDPROVEEDOR = Nothing
                        End If
                        If Gertakaria.IdRepetitiva <> Integer.MinValue Then dbGertakaria.IDREPETITIVA = Gertakaria.IdRepetitiva Else dbGertakaria.s_IDREPETITIVA = String.Empty
                        If Gertakaria.EsCliente Then dbGertakaria.CLIENTE = 1 Else dbGertakaria.CLIENTE = 0 '0 = False, 0 <> true
                        dbGertakaria.COMPENSADO = Gertakaria.Compensado

                        If Not Gertakaria.Creador Is Nothing Then dbGertakaria.IDCREADOR = Gertakaria.Creador.Id

                        If Not Gertakaria.Proceso Is Nothing Then
                            Dim gtkArea As New ELL.gtkAreas
                            Dim gtkSeccion As New ELL.gtkSecciones
                            Dim gtkProceso As New ELL.gtkProceso

                            gtkArea = Gertakaria.Proceso
                            dbGertakaria.CODAREA = gtkArea.ID

                            If Not gtkArea.Secciones Is Nothing AndAlso gtkArea.Secciones.Count = 1 Then
                                gtkSeccion = gtkArea.Secciones.Item(0)
                                dbGertakaria.CODSECCION = gtkSeccion.ID

                                If Not gtkSeccion.Procesos Is Nothing AndAlso gtkSeccion.Procesos.Count = 1 Then
                                    gtkProceso = gtkSeccion.Procesos.Item(0)
                                    dbGertakaria.CODPROCESO = gtkProceso.ID
                                End If
                            End If
                        End If

                        If Gertakaria.Lantegi <> Integer.MinValue Then dbGertakaria.LANTEGI = Gertakaria.Lantegi Else dbGertakaria.s_LANTEGI = String.Empty
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        Gertakaria.Id = dbGertakaria.ID
                        If dbGertakaria.s_FECHAAPERTURA <> String.Empty Then Gertakaria.FechaApertura = dbGertakaria.FECHAAPERTURA
                        If dbGertakaria.s_FECHACIERRE <> String.Empty Then Gertakaria.FechaCierre = dbGertakaria.FECHACIERRE
                        If Not dbGertakaria.DESCRIPCIONPROBLEMA Is Nothing Then Gertakaria.Descripcion = dbGertakaria.DESCRIPCIONPROBLEMA.ToString.Trim
                        If Not dbGertakaria.CAUSAPROBLEMA Is Nothing Then Gertakaria.Causa = dbGertakaria.CAUSAPROBLEMA.ToString.Trim
                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.IDTIPOINCIDENCIA) Then Gertakaria.IdTipoIncidencia = dbGertakaria.IDTIPOINCIDENCIA
                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.NUMPEDCAB) Then Gertakaria.NumPedCab = dbGertakaria.NUMPEDCAB
                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.OBSERVACIONESCOSTE) Then Gertakaria.ObservacionesCoste = dbGertakaria.OBSERVACIONESCOSTE
                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.PROCEDENCIANC) Then Gertakaria.ProcedenciaNC = dbGertakaria.PROCEDENCIANC
                        If Not dbGertakaria.TITULO Is Nothing Then Gertakaria.Titulo = dbGertakaria.TITULO.ToString.Trim
                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.TOTALACORDADO) Then Gertakaria.TotalAcordado = dbGertakaria.TOTALACORDADO
                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.IDPROVEEDOR) Then Gertakaria.IdProveedor = dbGertakaria.IDPROVEEDOR
                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.IDREPETITIVA) Then Gertakaria.IdRepetitiva = dbGertakaria.IDREPETITIVA
                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.CLIENTE) Then Gertakaria.EsCliente = dbGertakaria.CLIENTE
                        If Not dbGertakaria.CAPID Is Nothing Then Gertakaria.CapacidadProveedor = dbGertakaria.CAPID.ToString.Trim
                        Gertakaria.Compensado = dbGertakaria.COMPENSADO

                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.IDCREADOR) Then
                            Dim gnUsusario As New SABLib.ELL.Usuario
                            Dim UsrCompt As New SABLib.BLL.UsuariosComponent
                            gnUsusario = UsrCompt.GetUsuario(New SABLib.ELL.Usuario With {.Id = dbGertakaria.IDCREADOR}, False)
                            If gnUsusario IsNot Nothing Then
                                Gertakaria.Creador = gnUsusario
                            End If
                        End If

                        If dbGertakaria.s_CODAREA.Trim <> String.Empty Then
                            Dim gtkArea As New ELL.gtkAreas
                            Dim gtkAreaCompt As New gtkAreasComponent
                            Dim gtkSeccion As New ELL.gtkSecciones
                            Dim gtkSeccionCompt As New gtkSeccionesComponent
                            Dim gtkProceso As New ELL.gtkProceso
                            Dim gtkProcesoCompt As New gtkProcesosComponent

                            If dbGertakaria.s_CODPROCESO.Trim <> String.Empty And dbGertakaria.s_CODSECCION.Trim <> String.Empty Then
                                gtkProceso = gtkProcesoCompt.Consultar(dbGertakaria.CODPROCESO, dbGertakaria.CODSECCION)
                                gtkSeccion = gtkSeccionCompt.Consultar(dbGertakaria.CODSECCION)
                                If gtkSeccion IsNot Nothing Then
                                    gtkSeccion.Procesos.Clear()
                                    gtkSeccion.Procesos.Add(gtkProceso)

                                    gtkArea = gtkAreaCompt.Consultar(dbGertakaria.CODAREA)
                                    gtkArea.Secciones.Clear()
                                    gtkArea.Secciones.Add(gtkSeccion)

                                    Gertakaria.Proceso = gtkArea
                                End If
                            End If
                        End If

                        If Not dbGertakaria.IsColumnNull(DAL.GERTAKARIAK.ColumnNames.LANTEGI) Then Gertakaria.Lantegi = dbGertakaria.LANTEGI
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Borrar
                        dbGertakaria.DeleteAll()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbGertakaria.Save()
                        Gertakaria.Id = dbGertakaria.ID 'Cambiar esta linea por la llamada a la funcion que nos devuelve un Gertakaria.
                    Case ELL.Acciones.Consultar
                        'No se realiza ninguna accion.
                    Case ELL.Acciones.Borrar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbGertakaria.Save()
                End Select
                '-----------------------------------------------------------------

                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de TipoRepetitiva
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Consultar
                        'Cargamos los registro con el que vamos a trabajar.
                        dbTipoRepetitiva.Where.ID.Value = Gertakaria.IdRepetitiva
                        dbTipoRepetitiva.Query.Load()
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbTipoRepetitiva.EOF Then
                            Gertakaria.IdFamiliaRepetitiva = dbTipoRepetitiva.IDFAMILIAREPETITIVA
                        End If
                        '---------------------------------------------------------------------------
                End Select
                '*******************************************************************************************************************

                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de Gertakariak_Irudiak.
                '-----------------------------------------------------------------
                Dim gtkIrudiakComponent As New gtkIrudiakComponent
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        If Not Gertakaria.Imagenes Is Nothing Then
                            If Gertakaria.Imagenes.Count > 0 Then
                                For Each gtkIrudia As ELL.gtkIrudiak In Gertakaria.Imagenes
                                    gtkIrudia.IdIncidencia = Gertakaria.Id
                                    Select Case gtkIrudia.Accion
                                        Case ELL.Acciones.Insertar
                                            gtkIrudiakComponent.Insertar(gtkIrudia)
                                        Case ELL.Acciones.Borrar
                                            gtkIrudiakComponent.Borrar(gtkIrudia)
                                    End Select
                                Next
                            End If
                        End If
                    Case ELL.Acciones.Consultar
                        Dim gtkIrudia As New ELL.gtkIrudiak
                        gtkIrudia.IdIncidencia = Gertakaria.Id
                        Gertakaria.Imagenes = gtkIrudiakComponent.Consultar(gtkIrudia)
                End Select
                '*******************************************************************************************************************

                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de Gertakariak_Acciones.
                '-----------------------------------------------------------------
                Dim gtkEkintzakComponent As New gtkEkintzakComponent
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        For Each Ekintza As ELL.gtkEkintzak In Gertakaria.Acciones
                            Select Case Ekintza.Accion
                                Case ELL.Acciones.Insertar
                                    Ekintza.IdIncidencia = Gertakaria.Id
                                    gtkEkintzakComponent.Insertar(Ekintza)
                                Case ELL.Acciones.Modificar
                                    Ekintza.IdIncidencia = Gertakaria.Id
                                    gtkEkintzakComponent.Modificar(Ekintza)
                                Case ELL.Acciones.Borrar
                                    gtkEkintzakComponent.Borrar(Ekintza)
                            End Select
                        Next
                    Case ELL.Acciones.Consultar
                        Gertakaria.Acciones = gtkEkintzakComponent.Consultar(Gertakaria.Id)
                End Select

                '-----------------------------------------------------------------
                '*******************************************************************************************************************

                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de EquipoResolucion.
                '-----------------------------------------------------------------
                Dim dbEquipoResolucion As New DAL.EQUIPORESOLUCION
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Consultar
                        'Cargamos los registro con el que vamos a trabajar.
                        dbEquipoResolucion.Where.IDINCIDENCIA.Value = Gertakaria.Id
                        dbEquipoResolucion.Query.Load()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        If Not dbEquipoResolucion.EOF Then
                            dbEquipoResolucion.DeleteAll()
                            dbEquipoResolucion.Save()
                        End If
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If Not Gertakaria.EquipoResolucion Is Nothing Then
                            For Each Usuario As SABLib.ELL.Usuario In Gertakaria.EquipoResolucion
                                dbEquipoResolucion.AddNew()
                                dbEquipoResolucion.IDASISTENTE = Usuario.Id
                                dbEquipoResolucion.IDINCIDENCIA = Gertakaria.Id
                                dbEquipoResolucion.Save()
                            Next
                        End If
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbEquipoResolucion.EOF Then
                            Gertakaria.EquipoResolucion = New List(Of SABLib.ELL.Usuario)
                            Dim Func As New SABLib.BLL.UsuariosComponent
                            Do
                                Dim gtkUsuario As SABLib.ELL.Usuario = Func.GetUsuario(New SABLib.ELL.Usuario With {.Id = dbEquipoResolucion.IDASISTENTE}, False)
                                Gertakaria.EquipoResolucion.Add(gtkUsuario)
                            Loop While dbEquipoResolucion.MoveNext
                        End If
                        '---------------------------------------------------------------------------
                End Select
                '-----------------------------------------------------------------
                '*******************************************************************************************************************

                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de Responsable_Gertakariak.
                '-----------------------------------------------------------------
                Dim dbResponsaglesGtk As New DAL.RESPONSABLES_GERTAKARIAK
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Consultar
                        'Cargamos los registro con el que vamos a trabajar.
                        dbResponsaglesGtk.Where.IDINCIDENCIA.Value = Gertakaria.Id
                        dbResponsaglesGtk.Query.Load()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        If Not dbResponsaglesGtk.EOF Then
                            dbResponsaglesGtk.DeleteAll()
                            dbResponsaglesGtk.Save()
                        End If
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If Not Gertakaria.Responsables Is Nothing Then
                            For Each Usuario As SABLib.ELL.Usuario In Gertakaria.Responsables
                                dbResponsaglesGtk.AddNew()
                                dbResponsaglesGtk.IDUSUARIO = Usuario.Id
                                dbResponsaglesGtk.IDINCIDENCIA = Gertakaria.Id
                                dbResponsaglesGtk.Save()
                            Next
                        End If
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbResponsaglesGtk.EOF Then
                            Gertakaria.Responsables = New List(Of SABLib.ELL.Usuario)
                            Dim Func As New SABLib.BLL.UsuariosComponent
                            Do
                                Dim gtkUsuario As SABLib.ELL.Usuario = Func.GetUsuario(New SABLib.ELL.Usuario With {.Id = dbResponsaglesGtk.IDUSUARIO}, False)
                                'If GertakariakLib.BLL.Utils.stringNull(gtkUsuario.Izena) Is String.Empty Then
                                '    gtkUsuario.Izena = gtkUsuario.NombreUsuario
                                'Else
                                '    If Utils.stringNull(gtkUsuario.NombreUsuario) Is String.Empty Then gtkUsuario.Izena = gtkUsuario.Email
                                'End If
                                If gtkUsuario IsNot Nothing Then Gertakaria.Responsables.Add(gtkUsuario)
                            Loop While dbResponsaglesGtk.MoveNext
                        End If
                        '---------------------------------------------------------------------------
                End Select
                '-----------------------------------------------------------------
                '*******************************************************************************************************************

                '------------------------------------------------------
                'FROGA:
                '------------------------------------------------------
                'Dim f As New BLL.gtkLineaCosteComponent
                'Dim LC As New ELL.gtkLineaCoste
                'Dim ListaLC As New List(Of ELL.gtkLineaCoste)
                'Dim ListaLCB As New List(Of ELL.gtkLineaCoste)
                'ListaLC = f.GetLineasCostes(10090, 20, Nothing)
                'ListaLCB = f.ConsultarListado(New ELL.gtkLineaCoste With {.Id = 10399})
                'LC = f.Consultar(10393)
                '------------------------------------------------------

                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de OFMARCA.
                '-----------------------------------------------------------------
                Dim gtkOFComp As New BLL.gtkOFMComponent    'Funciones
                Dim gtkOFMarca As New ELL.gtkOFM            'Objeto para el tratamiento de los datos
                gtkOFMarca.IdIncidencia = Gertakaria.Id
                Select Case Accion
                    Case ELL.Acciones.Modificar, ELL.Acciones.Insertar
                        Dim dbOFMARCA As New DAL.OFMARCA
                        Dim gtkListaObj As New List(Of ELL.gtkOFM)  'Lista de BB.DD
                        '-----------------------------------------------------------------
                        'INSERTAMOS en la BB.DD los nuevos en la lista de OF_Operacion
                        '-----------------------------------------------------------------
                        If Not Gertakaria.OF_Operacion Is Nothing Then
                            For Each gtkOFM As ELL.gtkOFM In Gertakaria.OF_Operacion
                                gtkOFM.IdIncidencia = Gertakaria.Id

                                Dim Linea As New ELL.gtkOFM
                                Linea = gtkOFComp.Consultar(gtkOFM)

                                If gtkOFComp.Consultar(gtkOFM) Is Nothing Then
                                    gtkOFComp.Insertar(gtkOFM)
                                Else
                                    gtkOFComp.Modificar(gtkOFM)
                                End If
                            Next
                        End If
                        '-----------------------------------------------------------------
                        '-----------------------------------------------------------------
                        'ELIMINAMOS de la BB.DD los que NO ESTAN en la lista de OF_Operacion
                        '-----------------------------------------------------------------
                        gtkListaObj = gtkOFComp.ConsultarListado(gtkOFMarca)    'Lista de BB.DD.
                        If Not gtkListaObj Is Nothing Then
                            For Each gtkOFM As ELL.gtkOFM In gtkListaObj
                                Dim bBorrar As Boolean = True
                                If Not Gertakaria.OF_Operacion Is Nothing Then
                                    For Each gtkOFMInci As ELL.gtkOFM In Gertakaria.OF_Operacion
                                        If gtkOFM.Id = gtkOFMInci.Id Then
                                            bBorrar = False
                                        End If
                                    Next
                                End If
                                If bBorrar = True Then
                                    gtkOFComp.Borrar(gtkOFM)
                                End If
                            Next
                        End If
                        '-----------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        Gertakaria.OF_Operacion = gtkOFComp.ConsultarListado(gtkOFMarca)
                    Case ELL.Acciones.Borrar
                        'Se eliminan en cascada en la BB.DD.
                End Select
                '-----------------------------------------------------------------
                '*******************************************************************************************************************
                '------------------------------------------------------
                'FROGA:
                '------------------------------------------------------
                'Dim f2 As New BLL.gtkLineaCosteComponent
                'Dim LC2 As New ELL.gtkLineaCoste
                'Dim ListaLC2 As New List(Of ELL.gtkLineaCoste)
                'Dim ListaLCB2 As New List(Of ELL.gtkLineaCoste)
                'ListaLC2 = f.GetLineasCostes(10090, 20, Nothing)
                'ListaLCB2 = f.ConsultarListado(New ELL.gtkLineaCoste With {.Id = 10399})
                'LC2 = f2.Consultar(10393)
                '------------------------------------------------------
                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de DETECCION.
                '-----------------------------------------------------------------
                Dim gtkDetecInci As New ELL.gtkDeteccion            'Objeto para el tratamiento de los datos
                gtkDetecInci.IdIncidencia = Gertakaria.Id
                Dim gtkDetecComp As New BLL.gtkDeteccionComponent   'Funciones
                Select Case Accion
                    Case ELL.Acciones.Modificar, ELL.Acciones.Insertar
                        Dim dbDETECCION As New DAL.DETECCION
                        Dim gtkListaObj As New List(Of ELL.gtkDeteccion)  'Lista de BB.DD
                        '-----------------------------------------------------------------
                        'INSERTAMOS en la BB.DD los nuevos en la lista de DETECCION
                        '-----------------------------------------------------------------
                        If Not Gertakaria.Detectado Is Nothing Then
                            For Each gtkDeteccion As ELL.gtkDeteccion In Gertakaria.Detectado
                                gtkDeteccion.IdIncidencia = Gertakaria.Id
                                If gtkDetecComp.Consultar(gtkDeteccion) Is Nothing Then gtkDetecComp.Insertar(gtkDeteccion) Else gtkDetecComp.Modificar(gtkDeteccion)
                            Next
                        End If
                        '-----------------------------------------------------------------
                        '-----------------------------------------------------------------
                        'ELIMINAMOS de la BB.DD los que NO ESTAN en la lista de DETECCION
                        '-----------------------------------------------------------------
                        gtkListaObj = gtkDetecComp.ConsultarListado(gtkDetecInci)    'Lista de BB.DD.
                        If Not gtkListaObj Is Nothing Then
                            For Each gtkBBDD As ELL.gtkDeteccion In gtkListaObj                 'Lista de BB.DD.
                                Dim bBorrar As Boolean = True
                                If Not Gertakaria.Detectado Is Nothing Then
                                    For Each gtkClass As ELL.gtkDeteccion In Gertakaria.Detectado   'Lista del Campo de la Clase.
                                        If gtkBBDD.Id = gtkClass.Id Then
                                            bBorrar = False
                                        End If
                                    Next
                                End If
                                If bBorrar = True Then
                                    gtkDetecComp.Borrar(gtkBBDD)
                                End If
                            Next
                        End If
                        '-----------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        Gertakaria.Detectado = gtkDetecComp.ConsultarListado(gtkDetecInci)
                    Case ELL.Acciones.Borrar
                        'Se eliminan en cascada en la BB.DD.
                End Select
                '-----------------------------------------------------------------
                '*******************************************************************************************************************

                '------------------------------------------------------
                'FROGA:
                '------------------------------------------------------
                'Dim f3 As New BLL.gtkLineaCosteComponent
                'Dim LC3 As New ELL.gtkLineaCoste
                'Dim ListaLC3 As New List(Of ELL.gtkLineaCoste)
                'Dim ListaLCB3 As New List(Of ELL.gtkLineaCoste)
                'ListaLC3 = f.GetLineasCostes(10090, 20, Nothing)
                'ListaLCB3 = f.ConsultarListado(New ELL.gtkLineaCoste With {.Id = 10399})
                'LC3 = f3.Consultar(10393)
                '------------------------------------------------------

                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de LINEASCOSTE.
                '-----------------------------------------------------------------
                Dim gtkLinea As New ELL.gtkLineaCoste       'Objeto para el tratamiento de los datos
                gtkLinea.IdIncidencia = Gertakaria.Id
                Dim gtkLineaComp As New BLL.gtkLineaCosteComponent   'Funciones

                Select Case Accion
                    Case ELL.Acciones.Modificar, ELL.Acciones.Insertar
                        'Dim dbTabla As New DAL.LINEASCOSTE
                        Dim gtkListaObj As New List(Of ELL.gtkLineaCoste)  'Lista de BB.DD
                        '-----------------------------------------------------------------
                        'INSERTAMOS en la BB.DD los nuevos en la lista de LINEASCOSTE
                        '-----------------------------------------------------------------
                        If Not Gertakaria.LineasCoste Is Nothing Then
                            For Each gtkClass As ELL.gtkLineaCoste In Gertakaria.LineasCoste    'Lista del Campo de la Clase.
                                '----------------------------------------------------------------------------------------------------------
                                '-- Comprobamos si las Lineas de Coste tienen alguna relacion con las OFs --
                                'Si no la tienen consideramos que la "Linea de Coste" es nueva y la relacionamos con una "OFMarca".
                                '----------------------------------------------------------------------------------------------------------
                                'If gtkClass.IdOFM = Integer.MinValue Then
                                '    Dim NumOrd As Integer = gtkClass.NumOrd
                                '    Dim NumOpe As Integer = gtkClass.NumOpe
                                '    Dim NumMar As String = If(gtkClass.NumMar Is Nothing, Nothing, gtkClass.NumMar.Trim)
                                '    Dim lgtkOF As List(Of ELL.gtkOFM) = (From OF_O As ELL.gtkOFM In Gertakaria.OF_Operacion _
                                '            Where OF_O.OF = NumOrd And OF_O.OP = NumOpe _
                                '            Select OF_O Order By OF_O.OF, OF_O.OP, OF_O.Marca).ToList
                                '    If lgtkOF IsNot Nothing AndAlso lgtkOF.Count > 0 Then
                                '        Dim gtkOF As New ELL.gtkOFM
                                '        gtkOF = lgtkOF.Where(Function(o) _
                                '                                 (o.Marca IsNot Nothing And NumMar IsNot Nothing) _
                                '                                 AndAlso String.Compare(o.Marca.Trim, NumMar.Trim, CompareMethod.Text) = 0).FirstOrDefault
                                '        gtkClass.IdOFM = If(gtkOF Is Nothing, lgtkOF.FirstOrDefault.Id, gtkOF.Id)
                                '    End If
                                'End If
                                '----------------------------------------------------------------------------------------------------------
                                '----------------------------------------------------------------------------------------------------------
                                'FROGA:2015-06-22: Comprobamos que la linea de coste este relacionada con una OFMarca.
                                '----------------------------------------------------------------------------------------------------------
                                Dim NumOrd As Integer = gtkClass.NumOrd
                                Dim NumOpe As Integer = gtkClass.NumOpe
                                Dim NumMar As String = If(gtkClass.NumMar Is Nothing, Nothing, gtkClass.NumMar.Trim)
                                Dim lgtkOF As List(Of ELL.gtkOFM) = (From OF_O As ELL.gtkOFM In Gertakaria.OF_Operacion
                                                                     Where OF_O.OF = NumOrd And OF_O.OP = NumOpe
                                                                     Select OF_O Order By OF_O.OF, OF_O.OP, OF_O.Marca).ToList
                                If gtkClass.IdOFM = Integer.MinValue And lgtkOF IsNot Nothing AndAlso lgtkOF.Count > 0 Then
                                    Dim gtkOF As New ELL.gtkOFM
                                    gtkOF = lgtkOF.Where(Function(o) _
                                                             (o.Marca IsNot Nothing And NumMar IsNot Nothing) _
                                                             AndAlso String.Compare(o.Marca.Trim, NumMar.Trim, True) = 0).FirstOrDefault
                                    gtkClass.IdOFM = If(gtkOF Is Nothing, lgtkOF.FirstOrDefault.Id, gtkOF.Id)
                                End If
                                '----------------------------------------------------------------------------------------------------------

                                gtkClass.IdIncidencia = Gertakaria.Id
                                If gtkClass.Id = Integer.MinValue Then
                                    gtkLineaComp.Insertar(gtkClass)
                                ElseIf gtkLineaComp.Consultar(gtkClass.Id) Is Nothing Then
                                    gtkLineaComp.Insertar(gtkClass)
                                Else
                                    gtkLineaComp.Modificar(gtkClass)
                                    'Dim gtkClass_P As ELL.gtkLineaCoste = gtkClass
                                    'Try
                                    '    gtkLineaComp.Modificar(gtkClass)
                                    'Catch ex As Exception
                                    '    Log.Debug(ex)
                                    '    gtkClass_P = gtkClass_P
                                    'End Try

                                End If
                            Next
                        End If
                        '-----------------------------------------------------------------

                        '-----------------------------------------------------------------
                        'ELIMINAMOS de la BB.DD los que NO ESTAN en la lista de LINEASCOSTE
                        '-----------------------------------------------------------------
                        gtkListaObj = gtkLineaComp.ConsultarListado(gtkLinea)    'Lista de BB.DD.
                        If Not gtkListaObj Is Nothing Then
                            For Each gtkBBDD As ELL.gtkLineaCoste In gtkListaObj                 'Lista de BB.DD.
                                Dim bBorrar As Boolean = True
                                If Not Gertakaria.LineasCoste Is Nothing Then
                                    For Each gtkClass As ELL.gtkLineaCoste In Gertakaria.LineasCoste   'Lista del Campo de la Clase.
                                        If gtkBBDD.Id = gtkClass.Id Then
                                            bBorrar = False
                                        End If
                                    Next
                                End If
                                If bBorrar = True Then
                                    gtkLineaComp.Borrar(gtkBBDD)
                                End If
                            Next
                        End If
                        '-----------------------------------------------------------------

                    Case ELL.Acciones.Consultar
                        Gertakaria.LineasCoste = gtkLineaComp.ConsultarListado(gtkLinea)
                    Case ELL.Acciones.Borrar
                        'Se eliminan en cascada en la BB.DD.
                End Select
                '-----------------------------------------------------------------
                '*******************************************************************************************************************

                '*******************************************************************************************************************
                '-----------------------------------------------------------------
                'Tabla de Gertakariak_CAUSASNC.
                '-----------------------------------------------------------------
                Dim dbGertakariakCausasNC As New DAL.GERTAKARIAK_CAUSASNC
                Select Case Accion
                    Case ELL.Acciones.Modificar
                        dbGertakariakCausasNC.Where.IDINCIDENCIA.Value = Gertakaria.Id
                        dbGertakariakCausasNC.Query.Load()
                        dbGertakariakCausasNC.DeleteAll()
                        dbGertakariakCausasNC.Save()
                    Case ELL.Acciones.Consultar
                        dbGertakariakCausasNC.Where.IDINCIDENCIA.Value = Gertakaria.Id
                        dbGertakariakCausasNC.Query.Load()
                        If Not dbGertakariakCausasNC.EOF Then
                            Dim FunCausasNC As New BLL.gtkCausasNCComponent
                            Dim gtkCausasNC As ELL.gtkCausasNC
                            Do
                                gtkCausasNC = New ELL.gtkCausasNC
                                gtkCausasNC.Id = dbGertakariakCausasNC.IDCAUSANC
                                gtkCausasNC = FunCausasNC.Consultar(gtkCausasNC)
                                If (Gertakaria.CausasNC Is Nothing) Then Gertakaria.CausasNC = New List(Of ELL.gtkCausasNC)
                                Gertakaria.CausasNC.Add(gtkCausasNC)
                            Loop While dbGertakariakCausasNC.MoveNext
                        End If
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        If Not Gertakaria.CausasNC Is Nothing AndAlso Gertakaria.CausasNC.Count > 0 Then
                            For Each gtkCausaNC As ELL.gtkCausasNC In Gertakaria.CausasNC
                                dbGertakariakCausasNC.AddNew()
                                dbGertakariakCausasNC.IDINCIDENCIA = Gertakaria.Id
                                dbGertakariakCausasNC.IDCAUSANC = gtkCausaNC.Id
                                dbGertakariakCausasNC.Save()
                            Next
                        End If
                End Select
                '-----------------------------------------------------------------
                '*******************************************************************************************************************

                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        tx.CommitTransaction()
                End Select
                '-----------------------------------------------------------------

            Catch ex As ApplicationException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        tx.RollbackTransaction()
                End Select
                Gertakaria = Nothing
                '-----------------------------------------------------------------
                Log.Error(ex)
                Throw
            Catch ex As Exception
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        tx.RollbackTransaction()
                End Select
                Gertakaria = Nothing
                '-----------------------------------------------------------------
                Log.Error(ex)
                Throw
            End Try
            Return Gertakaria
        End Function

        ''' <summary>
        ''' Funcion general para las incidencias de Servicios Generales.
        ''' </summary>
        ''' <param name="Gertakaria"></param>
        ''' <param name="Accion"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function fgServiciosGenerales(ByVal Gertakaria As GertakariakLib.ELL.gtkServiciosGenerales, ByVal Accion As ELL.Acciones) As GertakariakLib.ELL.gtkServiciosGenerales
            Dim nGertakaria As New GertakariakLib.ELL.gtkServiciosGenerales
            Return nGertakaria
        End Function

        ''' <summary>
        ''' <para>Funcion de validacion de Ordenes de Fabricacion (OF).</para>
        ''' <example> 
        ''' <para> Dim desc as descripcion=String.empty</para>
        ''' <para> if(ValidarOF(of,op,desc)) then</para>      
        ''' <para>    Gertakaria.Descripcion=desc</para>
        ''' <para> end if</para>           
        ''' </example>
        ''' </summary>
        ''' <param name="OrdenFabricacion">Numero de Orden de Fabricacion.</param>
        ''' <param name="Operacion">Numero de Operacion.</param>
        ''' <returns>True si existe la of</returns>        >
        Public Function ValidarOF(ByVal OrdenFabricacion As Integer, ByVal Operacion As Integer, Optional ByVal Productivas As IGertakariakComponent.TipoORD = IGertakariakComponent.TipoORD.Todas) As Boolean Implements IGertakariakComponent.ValidarOF
            Dim dbW_CPCABEC As New GertakariakLib.DAL.W_CPCABEC  'Tabla de OFs (DataBase - db)

            dbW_CPCABEC.Where.NUMORD.Value = OrdenFabricacion
            dbW_CPCABEC.Where.NUMOPE.Value = Operacion
            Select Case Productivas
                Case IGertakariakComponent.TipoORD.Productivas
                    dbW_CPCABEC.Where.TIPORD.Value = 0
                Case IGertakariakComponent.TipoORD.NoProductivas
                    dbW_CPCABEC.Where.TIPORD.Operator = WhereParameter.Operand.NotEqual
                    dbW_CPCABEC.Where.TIPORD.Value = 0
            End Select
            dbW_CPCABEC.Query.Load()

            If Not dbW_CPCABEC.EOF Then
                Return True
            End If

            Return False
        End Function

        ''' <summary>
        ''' Tipo de Orden de Fabricacion.
        ''' </summary>
        Enum TipoORD
            Todas
            Productivas
            NoProductivas
        End Enum

#Region "AreaOrigen"

        '''' <summary>
        '''' Funcion general del "Area de Origen".
        '''' </summary>
        'Public Function fgAreaOrigen(ByVal Accion As ELL.Acciones, Optional ByVal lstAreaOrigen As List(Of ArrayList) = Nothing) As List(Of ArrayList)
        '    Dim Resultado As New List(Of ArrayList)

        '    Select Case Accion
        '        Case ELL.Acciones.Consultar
        '            Resultado = AreaOrigenConsultar()
        '    End Select

        '    Return Resultado
        'End Function

        '''' <summary>
        '''' Devuelve todos los IDs del area de origen.
        '''' </summary>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Private Function AreaOrigenConsultar() As List(Of ArrayList)
        'Dim lstAreaOrigen As New List(Of ArrayList)
        ''Dim dbAreaOrigen As New GertakariakLib.DAL.AREAORIGEN
        'Dim CampoTraducido As New GertakariakLib.BLL.Utils.Literal
        'dbAreaOrigen.Query.Load()
        'If Not dbAreaOrigen.EOF Then
        '    Do
        '        '--------------------------------------------------------------------
        '        'Cargamos los terminos de las diferentes culturas para este registro.
        '        '--------------------------------------------------------------------
        '        Dim dbAreaOrigenKultura As New GertakariakLib.DAL.AREAORIGENKULTURA
        '        'dbAreaOrigenKultura.IDAREAORIGEN = dbAreaOrigen.ID
        '        dbAreaOrigenKultura.Where.IDAREAORIGEN.Value = dbAreaOrigen.ID
        '        dbAreaOrigenKultura.Query.Load()
        '        '--------------------------------------------------------------------
        '        Dim arrAreaOrigen As New ArrayList
        '        arrAreaOrigen.Add(dbAreaOrigen.ID)

        '        If Not dbAreaOrigenKultura.EOF Then
        '            CampoTraducido = GertakariakLib.BLL.Utils.TraducirCampo(dbAreaOrigenKultura, DAL.AREAORIGENKULTURA.ColumnNames.DESCRIPCION.ToString, DAL.AREAORIGENKULTURA.ColumnNames.IDCULTURA.ToString, Kultura)
        '            arrAreaOrigen.Add(CampoTraducido.Descripcion)    'Descripcion traducida
        '            arrAreaOrigen.Add(CampoTraducido.IdCultura)    'Cultura traduccion
        '            'Else
        '            '   arrAreaOrigen.Add("")
        '            '  arrAreaOrigen.Add("")
        '            lstAreaOrigen.Add(arrAreaOrigen)
        '        End If
        '    Loop While dbAreaOrigen.MoveNext
        'End If

        'Return lstAreaOrigen
        'End Function

#End Region

#Region "Procedencia de la No Conformidad (TipoNoConformidad)"
        ''' <summary>
        ''' Funcion general de la "Procedencia de la No Conformidad" (TipoNoConformidad).
        ''' </summary>
        Public Function fgNoConformidad(ByVal Accion As ELL.Acciones, Optional ByVal lstTipoNC As List(Of ArrayList) = Nothing) As List(Of ArrayList)
            Dim Resultado As New List(Of ArrayList)
            Select Case Accion
                Case ELL.Acciones.Consultar
                    Resultado = TipoNCConsultar()
            End Select
            Return Resultado
        End Function
        ''' <summary>
        ''' Devuelve todos los IDs de la No Conformidad.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function TipoNCConsultar() As List(Of ArrayList)
            Dim lstTipoNC As New List(Of ArrayList)
            Dim dbTipoNoConformidad As New GertakariakLib.DAL.TIPONOCONFORMIDAD
            Dim CampoTraducido As New GertakariakLib.BLL.Utils.Literal
            dbTipoNoConformidad.Query.Load()
            If Not dbTipoNoConformidad.EOF Then
                Do
                    '--------------------------------------------------------------------
                    'Cargamos los terminos de las diferentes culturas para este registro.
                    '--------------------------------------------------------------------
                    Dim dbTipoNoConformidadKultura As New GertakariakLib.DAL.TIPONOCONFORMIDADKULTURA
                    'dbAreaOrigenKultura.IDAREAORIGEN = dbAreaOrigen.ID
                    dbTipoNoConformidadKultura.Where.IDNOCONFORMIDAD.Value = dbTipoNoConformidad.ID
                    dbTipoNoConformidadKultura.Query.Load()
                    '--------------------------------------------------------------------
                    Dim arrTipoNC As New ArrayList
                    arrTipoNC.Add(dbTipoNoConformidad.ID)

                    If Not dbTipoNoConformidadKultura.EOF Then
                        CampoTraducido = GertakariakLib.BLL.Utils.TraducirCampo(dbTipoNoConformidadKultura, DAL.TIPONOCONFORMIDADKULTURA.ColumnNames.TIPONOCONFORMIDAD.ToString, DAL.TIPONOCONFORMIDADKULTURA.ColumnNames.IDCULTURA.ToString, Kultura)
                        arrTipoNC.Add(CampoTraducido.Descripcion)    'Descripcion traducida
                        arrTipoNC.Add(CampoTraducido.IdCultura)    'Cultura traduccion
                        'Else
                        '    arrTipoNC.Add("")
                        '    arrTipoNC.Add("")
                        lstTipoNC.Add(arrTipoNC)
                    End If
                Loop While dbTipoNoConformidad.MoveNext
            End If
            Return lstTipoNC
        End Function
#End Region

#Region "Tipo de Incidencia Repetitiva"
        ''' <summary>
        ''' Funcion general para las familias de las Incidencias Repetitivas.
        ''' </summary>
        ''' <param name="Accion">Accion a realizar</param>
        ''' <param name="IdFamiliaRepetitiva">Identificador de la Familia de Tipos de Incidencias Repetitivas.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function fgTipoRepetitiva(ByVal Accion As ELL.Acciones, ByVal IdFamiliaRepetitiva As Integer, Optional ByVal lstTipoRepetitiva As List(Of ArrayList) = Nothing) As List(Of ArrayList)
            Dim Resultado As New List(Of ArrayList)

            Select Case Accion
                Case ELL.Acciones.Consultar
                    Resultado = TipoRepetitiva(IdFamiliaRepetitiva)
            End Select
            Return Resultado
        End Function

        ''' <summary>
        ''' Devuelve los Tipos de Incidencias Repetitivas.
        ''' </summary>
        ''' <param name="IdFamiliaRepetitiva">Identificador de la Familia de Tipos de Incidencias Repetitivas.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function TipoRepetitiva(ByVal IdFamiliaRepetitiva As Integer) As List(Of ArrayList)
            Dim lstTipoRepetitiva As New List(Of ArrayList)
            Dim dbTipoRepetitiva As New GertakariakLib.DAL.TIPOREPETITIVA
            Dim CampoTraducido As New GertakariakLib.BLL.Utils.Literal

            dbTipoRepetitiva.Where.IDFAMILIAREPETITIVA.Value = IdFamiliaRepetitiva
            dbTipoRepetitiva.Query.Load()

            If Not dbTipoRepetitiva.EOF Then
                Do
                    '--------------------------------------------------------------------
                    'Cargamos los terminos de las diferentes culturas para este registro.
                    '--------------------------------------------------------------------
                    Dim dbTipoRepetitivaKultura As New GertakariakLib.DAL.TIPOREPETITIVAKULTURA
                    dbTipoRepetitivaKultura.Where.IDREPETITIVA.Value = dbTipoRepetitiva.ID
                    dbTipoRepetitivaKultura.Query.Load()
                    '--------------------------------------------------------------------
                    Dim arrTerminoTraducido As New ArrayList    'Vector donde almacenamos la informacion del termino traducido (Cultura, Descripcion)
                    arrTerminoTraducido.Add(dbTipoRepetitiva.ID)

                    If Not dbTipoRepetitivaKultura.EOF Then
                        CampoTraducido = GertakariakLib.BLL.Utils.TraducirCampo(dbTipoRepetitivaKultura, DAL.TIPOREPETITIVAKULTURA.ColumnNames.DESCRIPCION.ToString, DAL.TIPOREPETITIVAKULTURA.ColumnNames.IDCULTURA.ToString, Kultura)
                        arrTerminoTraducido.Add(CampoTraducido.Descripcion)    'Descripcion traducida
                        arrTerminoTraducido.Add(CampoTraducido.IdCultura)    'Cultura traduccion
                        'Else
                        '    arrTerminoTraducido.Add("")
                        '    arrTerminoTraducido.Add("")
                        lstTipoRepetitiva.Add(arrTerminoTraducido)
                    End If
                Loop While dbTipoRepetitiva.MoveNext
            End If

            Return lstTipoRepetitiva

        End Function



#End Region
#Region "Familia de Tipo de Incidencia Repetitiva"
        ''' <summary>
        ''' Funcion general para las familias de las Incidencias Repetitivas.
        ''' </summary>
        ''' <param name="Accion">Accion a realizar</param>
        ''' <param name="lstFamiliaRepetitiva"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function fgFamiliaRepetitiva(ByVal Accion As ELL.Acciones, Optional ByVal lstFamiliaRepetitiva As List(Of ArrayList) = Nothing) As List(Of ArrayList)
            Dim Resultado As New List(Of ArrayList)

            Select Case Accion
                Case ELL.Acciones.Consultar
                    Resultado = FamiliaRepetitiva()
            End Select
            Return Resultado
        End Function

        ''' <summary>
        ''' Devuelve todas las familias de las Incidencias Repetitivas.
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function FamiliaRepetitiva() As List(Of ArrayList)
            Dim lstFamiliaRepetitiva As New List(Of ArrayList)
            Dim dbFamiliaRepetitiva As New GertakariakLib.DAL.FAMILIAREPETITIVA
            Dim CampoTraducido As New GertakariakLib.BLL.Utils.Literal
            dbFamiliaRepetitiva.Query.Load()
            If Not dbFamiliaRepetitiva.EOF Then
                Do
                    '--------------------------------------------------------------------
                    'Cargamos los terminos de las diferentes culturas para este registro.
                    '--------------------------------------------------------------------
                    Dim dbFamiliRepetitivaKultura As New GertakariakLib.DAL.FAMILIAREPETITIVAKULTURA
                    'dbAreaOrigenKultura.IDAREAORIGEN = dbAreaOrigen.ID
                    dbFamiliRepetitivaKultura.Where.IDFAMILIAREPETITIVA.Value = dbFamiliaRepetitiva.ID
                    dbFamiliRepetitivaKultura.Query.Load()
                    '--------------------------------------------------------------------
                    Dim arrTerminoTraducido As New ArrayList    'Vector donde almacenamos la informacion del termino traducido (Cultura, Descripcion)
                    arrTerminoTraducido.Add(dbFamiliaRepetitiva.ID)

                    If Not dbFamiliRepetitivaKultura.EOF Then
                        CampoTraducido = GertakariakLib.BLL.Utils.TraducirCampo(dbFamiliRepetitivaKultura, DAL.FAMILIAREPETITIVAKULTURA.ColumnNames.DESCRIPCION.ToString, DAL.FAMILIAREPETITIVAKULTURA.ColumnNames.IDCULTURA.ToString, Kultura)
                        arrTerminoTraducido.Add(CampoTraducido.Descripcion)    'Descripcion traducida
                        arrTerminoTraducido.Add(CampoTraducido.IdCultura)    'Cultura traduccion
                        'Else
                        '    arrTerminoTraducido.Add("")
                        '    arrTerminoTraducido.Add("")
                        lstFamiliaRepetitiva.Add(arrTerminoTraducido)
                    End If
                Loop While dbFamiliaRepetitiva.MoveNext
            End If

            Return lstFamiliaRepetitiva
        End Function

#End Region


#Region "Globalizacion"
		' ''' <summary>
		' ''' Funcion para obtener todos los responsables.
		' ''' Se devolvera en el campo izena el nombre de la persona si lo tuviese informado. Sino, en este mismo campo, se informaria el nombreUsuario.
		' ''' </summary>
		' ''' <returns>Lista de usuarios</returns>
		'Public Function getResponsables() As List(Of SabLib.ELL.Usuario)
		'    Dim usuariosComp As New SABLib_Z.DAL.USUARIOS
		'    Dim UserCompt As New SABLib.BLL.UsuariosComponent

		'    Dim listaUsuarios As New List(Of SABLib.ELL.Usuario)
		'    usuariosComp.Where.IDDIRECTORIOACTIVO.Operator = WhereParameter.Operand.IsNotNull
		'    usuariosComp.Query.Load()

		'    If Not usuariosComp.EOF Then
		'        Do
		'            Dim oUser As New SABLib.ELL.Usuario
		'            oUser = UserCompt.GetUsuario(New SABLib.ELL.Usuario With {.Id = usuariosComp.ID}, False)
		'            If oUser IsNot Nothing Then listaUsuarios.Add(oUser)
		'        Loop While usuariosComp.MoveNext
		'    End If

		'    Return listaUsuarios
		'End Function
#End Region

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub

#Region "Busquedas"
        ''' <summary>
        ''' Busca las incidencias segun los parametros de busqueda indicados
        ''' </summary>
        ''' <param name="filtrado">Objeto que contiene los filtros</param>
        ''' <returns>Devuelve una lista de incidencias obtenidas</returns>
        Public Function Filtrar(ByVal filtrado As ELL.Filtrado) As List(Of ELL.gtkTroqueleria)
            Dim listIncid As New List(Of ELL.gtkTroqueleria)
            Dim gertak As New DAL.GERTAKARIAK
            Dim oGertak As ELL.gtkTroqueleria
            Dim dr As Object
            dr = gertak.Filtrar(filtrado)
            If Not (dr Is Nothing) Then
                While dr.Read
                    oGertak = New ELL.gtkTroqueleria
                    oGertak.Id = CInt(dr(ELL.gtkTroqueleria.ColumnNames_Gert.ID))
                    oGertak.IdTipoIncidencia = Utils.integerNull(dr(ELL.gtkTroqueleria.ColumnNames_Gert.IDTIPOINCIDENCIA))
                    oGertak.IdRepetitiva = Utils.integerNull(dr(ELL.gtkTroqueleria.ColumnNames_Gert.IDREPETITIVA))
                    oGertak.Descripcion = Utils.stringNull(dr(ELL.gtkTroqueleria.ColumnNames_Gert.DESCRIPCIONPROBLEMA))
                    oGertak.Causa = Utils.stringNull(dr(ELL.gtkTroqueleria.ColumnNames_Gert.CAUSAPROBLEMA))
                    oGertak.FechaApertura = Utils.dateTimeNull(dr(ELL.gtkTroqueleria.ColumnNames_Gert.FECHAAPERTURA))
                    oGertak.FechaCierre = Utils.dateTimeNull(dr(ELL.gtkTroqueleria.ColumnNames_Gert.FECHACIERRE))
                    oGertak.Titulo = Utils.stringNull(dr(ELL.gtkTroqueleria.ColumnNames_Gert.TITULO))
                    oGertak.IdProveedor = Utils.integerNull(dr(ELL.gtkTroqueleria.ColumnNames.IDPROVEEDOR))
                    listIncid.Add(oGertak)
                End While
                If Not (dr Is Nothing) Then dr.close()
            End If
            Return listIncid
        End Function

        ''' <summary>
        ''' Busca las incidencias segun los parametros de busqueda indicados
        ''' </summary>
        ''' <param name="Filtro">Objeto que contiene los filtros</param>
        ''' <returns>Devuelve una lista de incidencias obtenidas</returns>
        Public Function FiltrarPor(ByVal Filtro As ELL.Filtrado, Optional ByVal Operador As ELL.Filtrado.Operador = ELL.Filtrado.Operador.OR) As dsGertakariak
            Dim dsGertakariak As New dsGertakariak
            Dim daGer As New GertakariakLib.BLL.DataReaderAdapterBatz
            Dim lstID As New List(Of Integer)   'Lista de Identificadores de incidencias
            Dim lstAux As New List(Of Integer)
            Dim iAux As Integer
            Dim bRellenado As Boolean = False

            Dim fGERTAKARIAK As New DAL.GERTAKARIAK
            Dim drGertakariak As IDataReader = Nothing
            Dim fTIPOREPETITIVA As New DAL.TIPOREPETITIVA
            Dim drTipoRepetitiva As IDataReader = Nothing
            Dim fFAMILIAREPETITIVA As New DAL.FAMILIAREPETITIVA
            Dim drFAMILIAREPETITIVA As IDataReader = Nothing

            Dim fRESPONSABLESGTK As New DAL.RESPONSABLES_GERTAKARIAK
            Dim drRESPONSABLESGTK As IDataReader = Nothing
            Dim fACCIONES_USUARIOS As New DAL.ACCIONES_USUARIOS
            Dim drACCIONES_USUARIOS As IDataReader = Nothing

            Dim fGERTAKARIAK_CAUSASNC As New DAL.GERTAKARIAK_CAUSASNC
            Dim drGtkCaudasNC As IDataReader = Nothing
            Dim fCAUSASNC As New DAL.CAUSASNC
            Dim drCAUSASNC As IDataReader = Nothing
            Dim fCAUSASNCPROC As New DAL.CAUSASNCPROC
            Dim drCAUSASNCPROC As IDataReader = Nothing

            Dim fOFMARCA As New DAL.OFMARCA
            Dim drOFMARCA As IDataReader = Nothing

            Try
                '------------------------------------------------------------------
                '- Obtenemos las incidencias para los parametros especificados. ---
                '------------------------------------------------------------------
                'ABEL:Lo he añadido porque cuando registrabas una incidencia y volvia al listado con un filtro ya guardado, ya tenia informada una lista de ids, donde no estaba la nueva
                'De esta forma, ya funciona
                Filtro.IDs = Nothing
                '********
                drGertakariak = fGERTAKARIAK.FiltrarPor(Filtro.SelectFromWhere("GERTAKARIAK", Operador))
                daGer.FillFromReader(dsGertakariak.GERTAKARIAK, drGertakariak)
                dsGertakariak.GERTAKARIAK.Select()
                For Each Fila As DataRow In dsGertakariak.GERTAKARIAK
                    lstID.Add(Fila.Item(DAL.GERTAKARIAK.ColumnNames.ID))
                    If Not Fila.IsNull(DAL.GERTAKARIAK.ColumnNames.IDREPETITIVA) Then
                        iAux = Fila.Item(DAL.GERTAKARIAK.ColumnNames.IDREPETITIVA)
                        If (Not lstAux.Exists(Function(o As Integer) o = iAux)) Then lstAux.Add(iAux)
                    End If
                Next
                'If lstID.Count <> 0 Then Filtro.IDs = lstID
                '------------------------------------------------------------------

                '------------------------------------------------------------------
                '- Obtenemos el resto de tablas para las incidencias obtenidas. ---
                '------------------------------------------------------------------

                'Tipo repetitiva
                '-----------------------------------------------------
                bRellenado = (Filtro.RepTipos IsNot Nothing AndAlso Filtro.RepTipos.Count > 0)
                Filtro.RepTipos = lstAux
                drTipoRepetitiva = fTIPOREPETITIVA.FiltrarPor(Filtro.SelectFromWhere("TIPOREPETITIVA", Operador))
                daGer.FillFromReader(dsGertakariak.TIPOREPETITIVA, drTipoRepetitiva)
                If (Not bRellenado) Then Filtro.RepTipos = Nothing

                dsGertakariak.TIPOREPETITIVA.Select()
                lstAux = New List(Of Integer)
                For Each Fila As DataRow In dsGertakariak.TIPOREPETITIVA
                    iAux = Fila.Item(DAL.TIPOREPETITIVA.ColumnNames.IDFAMILIAREPETITIVA)
                    If (Not lstAux.Exists(Function(o As Integer) o = iAux)) Then lstAux.Add(iAux)
                Next

                'Familia repetitiva
                '-----------------------------------------------------
                bRellenado = (Filtro.RepFamilias IsNot Nothing AndAlso Filtro.RepFamilias.Count > 0)
                Filtro.RepFamilias = lstAux
                drFAMILIAREPETITIVA = fFAMILIAREPETITIVA.FiltrarPor(Filtro.SelectFromWhere("FAMILIAREPETITIVA", Operador))
                daGer.FillFromReader(dsGertakariak.FAMILIAREPETITIVA, drFAMILIAREPETITIVA)
                If (Not bRellenado) Then Filtro.RepFamilias = Nothing

                'Responsables
                '-----------------------------------------------------
                lstAux = New List(Of Integer)
                Dim gertDAL As New DAL.RESPONSABLES_GERTAKARIAK
                gertDAL.LoadAll()
                If (gertDAL.RowCount > 0) Then
                    Do
                        iAux = gertDAL.IDINCIDENCIA
                        If (lstID.Exists(Function(o As Integer) o = iAux)) Then
                            iAux = gertDAL.IDUSUARIO
                            If (Not lstAux.Exists(Function(o As Integer) o = iAux)) Then lstAux.Add(iAux)
                        End If
                    Loop While gertDAL.MoveNext
                End If
                bRellenado = (Filtro.ResponsablesADM IsNot Nothing AndAlso Filtro.ResponsablesADM.Count > 0)

                'Acciones de usuario
                '-----------------------------------------------------
                Dim lstAux2 As New List(Of Integer)
                Dim gert2DAL As New DAL.GERTAKARIAK_ACCIONES
                gert2DAL.LoadAll()
                If (gert2DAL.RowCount > 0) Then
                    Do
                        iAux = gert2DAL.IDINCIDENCIA
                        If (lstID.Exists(Function(o As Integer) o = iAux)) Then
                            iAux = gert2DAL.IDACCION
                            If (Not lstAux2.Exists(Function(o As Integer) o = iAux)) Then lstAux2.Add(iAux)
                        End If
                    Loop While gert2DAL.MoveNext
                End If

                'De todas las acciones, obtenemos todos sus usuarios distintos
                Dim gert3DAL As New DAL.ACCIONES_USUARIOS
                gert3DAL.LoadAll()
                If (gert3DAL.RowCount > 0) Then
                    Do
                        iAux = gert3DAL.IDACCION
                        If (lstAux2.Exists(Function(o As Integer) o = iAux)) Then
                            iAux = gert3DAL.IDUSUARIO
                            If (Not lstAux.Exists(Function(o As Integer) o = iAux)) Then lstAux.Add(iAux)
                        End If
                    Loop While gert3DAL.MoveNext
                End If
                bRellenado = (Filtro.ResponsablesADM IsNot Nothing AndAlso Filtro.ResponsablesADM.Count > 0)
                Filtro.ResponsablesADM = lstAux
                drACCIONES_USUARIOS = fACCIONES_USUARIOS.FiltrarPor(Filtro.SelectFromWhere("ACCIONES_USUARIOS", Operador))
                drRESPONSABLESGTK = fRESPONSABLESGTK.FiltrarPor(Filtro.SelectFromWhere("RESPONSABLES_GERTAKARIAK", Operador))
                daGer.FillFromReader(dsGertakariak.ACCIONES_USUARIOS, drACCIONES_USUARIOS)
                daGer.FillFromReader(dsGertakariak.RESPONSABLES_GERTAKARIAK, drRESPONSABLESGTK)
                If (Not bRellenado) Then Filtro.ResponsablesADM = Nothing

                'Causas
                '----------------------------------------
                lstAux = New List(Of Integer)
                Dim gert4DAL As New DAL.GERTAKARIAK_CAUSASNC
                gert4DAL.LoadAll()
                If (gert4DAL.RowCount > 0) Then
                    Do
                        iAux = gert4DAL.IDINCIDENCIA
                        If (lstID.Exists(Function(o As Integer) o = iAux)) Then
                            iAux = gert4DAL.IDCAUSANC
                            If (Not lstAux.Exists(Function(o As Integer) o = iAux)) Then lstAux.Add(iAux)
                        End If
                    Loop While gert4DAL.MoveNext
                End If
                bRellenado = (Filtro.CausasNC IsNot Nothing AndAlso Filtro.CausasNC.Count > 0)
                drCAUSASNCPROC = fCAUSASNCPROC.FiltrarPor(Filtro.SelectFromWhere("CAUSASNCPROC", Operador))
                daGer.FillFromReader(dsGertakariak.CAUSASNCPROC, drCAUSASNCPROC)
                If (Not bRellenado) Then Filtro.CausasNC = Nothing

                'OFMarca
                drOFMARCA = fCAUSASNCPROC.FiltrarPor(Filtro.SelectFromWhere("OFMARCA", Operador))
                daGer.FillFromReader(dsGertakariak.OFMARCA, drOFMARCA)

                ''drGtkCaudasNC = fGERTAKARIAK_CAUSASNC.FiltrarPor(Filtro.SelectFromWhere("GERTAKARIAK_CAUSASNC", Operador))
                ''drCAUSASNC = fCAUSASNC.FiltrarPor(Filtro.SelectFromWhere("CAUSASNC", Operador))
                ''daGer.FillFromReader(dsGertakariak.GERTAKARIAK_CAUSASNC, drGtkCaudasNC)
                ''daGer.FillFromReader(dsGertakariak.CAUSASNC, drCAUSASNC)

                '------------------------------------------------------------------

                'Catch ex As BatzException
                'dsGertakariak.Clear()
                'Throw
            Catch ex As Exception
                dsGertakariak.Clear()
                'Throw New BatzException("errCompCargar", ex)
                Log.Error(ex)
                Throw
            Finally
                If Not (drGertakariak Is Nothing) Then drGertakariak.Close()
                If Not (drTipoRepetitiva Is Nothing) Then drTipoRepetitiva.Close()
                If Not (drFAMILIAREPETITIVA Is Nothing) Then drFAMILIAREPETITIVA.Close()
                If Not (drRESPONSABLESGTK Is Nothing) Then drRESPONSABLESGTK.Close()
                If Not (drACCIONES_USUARIOS Is Nothing) Then drACCIONES_USUARIOS.Close()
            End Try

            Return dsGertakariak
        End Function
#End Region
    End Class

	Public Class gtkEkintzakComponent
		Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkEkintzakComponent")
		''' <summary>
		''' Consultamos las acciones asociadas a la incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las acciones (gtkEkintzak).</param>
		''' <returns>Devuleve una lista de objetos gtkEkintzak.</returns>
		Public Overloads Function Consultar(ByVal gtkObjeto As ELL.gtkEkintzak) As List(Of ELL.gtkEkintzak)
			Return ConsultarListado(gtkObjeto)
		End Function
		''' <summary>
		''' Consultamos las acciones asociadas a la incidencia.
		''' </summary>
		''' <param name="Id">Identificador del registro.</param>
		''' <returns>Devuleve una lista de objetos gtkEkintzak.</returns>
		''' <remarks></remarks>
		Public Overloads Function Consultar(ByVal Id As Integer) As List(Of ELL.gtkEkintzak)
			Dim gtkObjeto As New ELL.gtkEkintzak
			gtkObjeto.IdIncidencia = Id
			Return ConsultarListado(gtkObjeto)
		End Function

		'''' <summary>
		'''' Consultamos las imagenes asociadas a la incidencia.
		'''' </summary>
		'''' <param name="IdIncidencia">Identificador de la Incidencia</param>
		'''' <returns></returns>
		'''' <remarks></remarks>
		'Public Overloads Function Consultar(ByVal IdIncidencia As Integer) As List(Of ELL.gtkIrudiak)
		'    Dim gtkIrudia As New ELL.gtkIrudiak
		'    gtkIrudia.IdIncidencia = IdIncidencia
		'    Return ConsultarIrudiak(gtkIrudia)
		'End Function

		''' <summary>
		''' Consultamos las acciones asociadas a la incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las acciones (gtkEkintzak).</param>
		''' <returns>Devuleve un objeto de imagen.</returns>
		Private Function ConsultarListado(ByVal gtkObjeto As ELL.gtkEkintzak) As List(Of ELL.gtkEkintzak)
			Dim lstObjeto As New List(Of GertakariakLib.ELL.gtkEkintzak)
			Dim dbGtkAcciones As New DAL.GERTAKARIAK_ACCIONES
			Dim dbAcciones As New DAL.ACCIONES
			If gtkObjeto.Id <> Integer.MinValue Then dbGtkAcciones.Where.IDACCION.Value = gtkObjeto.Id
			If gtkObjeto.IdIncidencia <> Integer.MinValue Then dbGtkAcciones.Where.IDINCIDENCIA.Value = gtkObjeto.IdIncidencia
			dbGtkAcciones.Query.Load()
			'If Not dbGtkAcciones.EOF Then
			If dbGtkAcciones.RowCount > 0 Then
				Do
					Dim nObjeto As New ELL.gtkEkintzak
					nObjeto.Id = dbGtkAcciones.IDACCION
					nObjeto.IdIncidencia = dbGtkAcciones.IDINCIDENCIA
					lstObjeto.Add(fGeneral(ELL.Acciones.Consultar, nObjeto))
				Loop While dbGtkAcciones.MoveNext
			End If
			Return lstObjeto
		End Function

		''' <summary>
		''' Funcion para la insercion de Incidencias.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las acciones (gtkEkintzak).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Insertar(ByVal gtkObjeto As ELL.gtkEkintzak) As ELL.gtkEkintzak
			Return fGeneral(ELL.Acciones.Insertar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion para la modificacion de Incidencias.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las acciones (gtkEkintzak).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Modificar(ByVal gtkObjeto As ELL.gtkEkintzak) As ELL.gtkEkintzak
			Return fGeneral(ELL.Acciones.Modificar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion para la eliminacion de las imagenes de una incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las acciones (gtkEkintzak).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Borrar(ByVal gtkObjeto As ELL.gtkEkintzak) As ELL.gtkEkintzak
			Return fGeneral(ELL.Acciones.Borrar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion general para las Acciones (Medidas Correctoras).
		''' </summary>
		''' <param name="Accion"></param>
		''' <param name="gtkObjeto">Devuelve un Objeto gtkEkintzak</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkEkintzak) As ELL.gtkEkintzak
			Dim dbAcciones As New GertakariakLib.DAL.ACCIONES  'Tabla de Acciones (DataBase - db)
			Dim dbAcciones_Usr As New GertakariakLib.DAL.ACCIONES_USUARIOS	'Tabla de Acciones_Usuario (DataBase - db)
			Dim dbAcciones_Ejc As New GertakariakLib.DAL.ACCIONES_EJECUCION	 'Tabla de Acciones_Ejecucion (DataBase - db)
			Dim dbGtk_Acciones As New GertakariakLib.DAL.GERTAKARIAK_ACCIONES  'Tabla de Gertakariak_Acciones (DataBase - db)
			Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr

			Try
				'-----------------------------------------------------------------
				'Inicio de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.BeginTransaction()
				End Select
				'-----------------------------------------------------------------

				'-----------------------------------------------------------------
				'Tabla de Acciones
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						'Cargamos el registro con el que vamos a trabajar.
						dbAcciones.Where.ID.Value = gtkObjeto.Id
						dbAcciones.Query.Load()
					Case ELL.Acciones.Insertar
						'Para hacer insert
						dbAcciones.AddNew()
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'---------------------------------------------------------------------------
						'Campos a Insertar o Modificar
						'---------------------------------------------------------------------------
						If Not gtkObjeto.Descripcion Is Nothing Then dbAcciones.DESCRIPCION = gtkObjeto.Descripcion
						If Not gtkObjeto.Eficacia Is Nothing Then dbAcciones.EFICACIA = gtkObjeto.Eficacia
						If gtkObjeto.FechaFin = DateTime.MinValue Then dbAcciones.s_FECHAFIN = String.Empty Else dbAcciones.FECHAFIN = gtkObjeto.FechaFin
						If gtkObjeto.FechaInicio = DateTime.MinValue Then dbAcciones.s_FECHAINICIO = String.Empty Else dbAcciones.FECHAINICIO = gtkObjeto.FechaInicio
						If gtkObjeto.FechaPrevista = DateTime.MinValue Then dbAcciones.s_FECHAPREVISTA = String.Empty Else dbAcciones.FECHAPREVISTA = gtkObjeto.FechaPrevista
						If gtkObjeto.Id = Integer.MinValue Then dbAcciones.s_ID = String.Empty Else dbAcciones.ID = gtkObjeto.Id
						If gtkObjeto.IdTipoAccion = 0 Then dbAcciones.s_IDTIPOACCION = String.Empty Else dbAcciones.IDTIPOACCION = gtkObjeto.IdTipoAccion
						'---------------------------------------------------------------------------
					Case ELL.Acciones.Consultar
						'---------------------------------------------------------------------------
						'Campos a devolver en la consulta
						'---------------------------------------------------------------------------
						If Not dbAcciones.EOF Then
							Do
								gtkObjeto.Accion = ELL.Acciones.Consultar
								gtkObjeto.Descripcion = dbAcciones.s_DESCRIPCION
								gtkObjeto.Eficacia = dbAcciones.s_EFICACIA
								If (dbAcciones.s_FECHAINICIO <> String.Empty) Then gtkObjeto.FechaInicio = dbAcciones.FECHAINICIO
								If (dbAcciones.s_FECHAPREVISTA <> String.Empty) Then gtkObjeto.FechaPrevista = dbAcciones.FECHAPREVISTA
								If (dbAcciones.s_FECHAFIN <> String.Empty) Then gtkObjeto.FechaFin = dbAcciones.FECHAFIN
								gtkObjeto.Id = dbAcciones.ID
								gtkObjeto.IdTipoAccion = dbAcciones.IDTIPOACCION
							Loop While dbAcciones.MoveNext
						End If
						'---------------------------------------------------------------------------
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'Realizamos la accion correspondiente (Insertar, Modificar)
						dbAcciones.Save()
						If (gtkObjeto.Accion = ELL.Acciones.Insertar) Then
							gtkObjeto.Id = dbAcciones.ID
						End If
					Case ELL.Acciones.Consultar
						'No se realiza ninguna accion.
					Case ELL.Acciones.Borrar
						dbAcciones.DeleteAll()
						dbAcciones.Save()
				End Select
				'-----------------------------------------------------------------
				'-----------------------------------------------------------------
				'Tabla de Acciones_Usuario
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'Cargamos el registro con el que vamos a trabajar.
						dbAcciones_Usr.Where.IDACCION.Value = gtkObjeto.Id
						dbAcciones_Usr.Query.Load()
						If Not gtkObjeto.Responsables Is Nothing Then
							dbAcciones_Usr.DeleteAll()
							dbAcciones_Usr.Save()
							For Each Usuario As SabLib.ELL.Usuario In gtkObjeto.Responsables
								dbAcciones_Usr.AddNew()
								dbAcciones_Usr.IDACCION = gtkObjeto.Id
								dbAcciones_Usr.IDUSUARIO = Usuario.Id
								dbAcciones_Usr.Save()
							Next
						End If
					Case ELL.Acciones.Consultar
						gtkObjeto.Responsables = getResponsablesAccion(gtkObjeto.Id)
				End Select
				'-----------------------------------------------------------------

				'-----------------------------------------------------------------
				'Tabla de Acciones_Ejecucion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'Cargamos el registro con el que vamos a trabajar.
						dbAcciones_Ejc.Where.IDACCION.Value = gtkObjeto.Id
						dbAcciones_Ejc.Query.Load()
						If Not gtkObjeto.ResponsablesEjecucion Is Nothing Then
							dbAcciones_Ejc.DeleteAll()
							dbAcciones_Ejc.Save()
							For Each Usuario As SabLib.ELL.Usuario In gtkObjeto.ResponsablesEjecucion
								dbAcciones_Ejc.AddNew()
								dbAcciones_Ejc.IDACCION = gtkObjeto.Id
								dbAcciones_Ejc.IDUSUARIO = Usuario.Id
								dbAcciones_Ejc.Save()
							Next
						End If
					Case ELL.Acciones.Consultar
						gtkObjeto.ResponsablesEjecucion = getResponsablesEjecucion(gtkObjeto.Id)
				End Select
				'-----------------------------------------------------------------

				'-----------------------------------------------------------------
				'Tabla de Gertakariak_Acciones
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar
						dbGtk_Acciones.AddNew()
						dbGtk_Acciones.IDINCIDENCIA = gtkObjeto.IdIncidencia
						dbGtk_Acciones.IDACCION = gtkObjeto.Id
						dbGtk_Acciones.Save()
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

			Catch ex As ApplicationException
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				Throw
			Catch ex As Exception
				Log.Error(ex)
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				Throw
			End Try
			Return gtkObjeto
		End Function

		''' <summary>
		''' Obtenemos los responsables de la accion.
		''' </summary>
		''' <param name="IdAccion">Identificador de la accion.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function getResponsablesAccion(ByVal IdAccion As Integer) As List(Of SabLib.ELL.Usuario)
			Dim lstUsuarios As New List(Of SabLib.ELL.Usuario)
			Dim usuariosComponent As New SabLib.BLL.UsuariosComponent
			Dim bgAccionesUsr As New GertakariakLib.DAL.ACCIONES_USUARIOS
			bgAccionesUsr.Where.IDACCION.Value = IdAccion
			bgAccionesUsr.Query.Load()
			If Not bgAccionesUsr.EOF Then
				Do
					lstUsuarios.Add(usuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = bgAccionesUsr.IDUSUARIO}, False))
				Loop While bgAccionesUsr.MoveNext
			End If
			Return lstUsuarios
		End Function

		''' <summary>
		''' Obtenemos los responsables de la accion.
		''' </summary>
		''' <param name="IdAccion">Identificador de la accion.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function getResponsablesEjecucion(ByVal IdAccion As Integer) As List(Of SabLib.ELL.Usuario)
			Dim lstUsuarios As New List(Of SabLib.ELL.Usuario)
			Dim usuariosComponent As New SabLib.BLL.UsuariosComponent
			Dim bgAccionesUsr As New GertakariakLib.DAL.ACCIONES_EJECUCION
			bgAccionesUsr.Where.IDACCION.Value = IdAccion
			bgAccionesUsr.Query.Load()
			If Not bgAccionesUsr.EOF Then
				Do
					lstUsuarios.Add(usuariosComponent.GetUsuario(New SabLib.ELL.Usuario With {.Id = bgAccionesUsr.IDUSUARIO}, False))
				Loop While bgAccionesUsr.MoveNext
			End If
			Return lstUsuarios
		End Function
	End Class

	Public Class gtkOFMComponent
		Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkOFMComponent")
		''' <summary>
		''' Consultamos las Ordenes de Fabricacion asociadas a la Incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Ordenes de Fabricacion (gtkOFM).</param>
		''' <returns>Devuleve una lista de objetos gtkEkintzak.</returns>
		Public Function Consultar(ByVal gtkObjeto As ELL.gtkOFM) As ELL.gtkOFM
			Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
		End Function

		''' <summary>
		''' Consultamos las Ordenes de Fabricacion asociadas a la incidencia.
		''' </summary>
		''' <param name="Id">Identificador del registro.</param>
		''' <returns>Devuleve una lista de objetos gtkOFM.</returns>
		''' <remarks></remarks>
		Public Function Consultar(ByVal Id As Integer) As ELL.gtkOFM
			Dim gtkObjeto As New ELL.gtkOFM
			gtkObjeto.Id = Id
			Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
		End Function

		''' <summary>
		''' Consultamos las Ordenes de Fabricacion asociadas a la incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Ordenes de Fabricacion (gtkOFM).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkOFM) As List(Of ELL.gtkOFM)
			Dim dbOFMARCA As New GertakariakLib.DAL.OFMARCA	 'Tabla de Ordenes de Fabricacion. OF (DataBase - db)            
			Dim gtkListObj As New List(Of ELL.gtkOFM)
			Try

				If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then dbOFMARCA.Where.IDOFMARCA.Value = gtkObjeto.Id
				If gtkObjeto.IdIncidencia <> Integer.MinValue And Not (gtkObjeto.IdIncidencia.ToString Is Nothing Or gtkObjeto.IdIncidencia.ToString Is DBNull.Value) Then dbOFMARCA.Where.IDINCIDENCIA.Value = gtkObjeto.IdIncidencia
				If Not gtkObjeto.OF Is Nothing Then dbOFMARCA.Where.NUMOF.Value = gtkObjeto.OF
				If Not gtkObjeto.OP Is Nothing Then dbOFMARCA.Where.OP.Value = gtkObjeto.OP
				If Not gtkObjeto.Marca Is Nothing Then dbOFMARCA.Where.MARCA.Value = gtkObjeto.Marca

				dbOFMARCA.Query.Load()
				If Not dbOFMARCA.EOF Then
					Do
						Dim gtkObjetoB As New ELL.gtkOFM	'Nuevo Objeto que se va metiendo en la lista
						If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.IDOFMARCA) Then gtkObjetoB.Id = dbOFMARCA.IDOFMARCA
						If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.IDINCIDENCIA) Then gtkObjetoB.IdIncidencia = dbOFMARCA.IDINCIDENCIA
						If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.MARCA) Then gtkObjetoB.Marca = dbOFMARCA.MARCA
						If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.NUMOF) Then gtkObjetoB.OF = dbOFMARCA.NUMOF
						If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.OP) Then gtkObjetoB.OP = dbOFMARCA.OP

						'-- Recogemos la descripción ------------------------------------------
						If Not DAL.OFMARCA.ColumnNames.NUMOF Is Nothing And Not DAL.OFMARCA.ColumnNames.OP Is Nothing And Not DAL.OFMARCA.ColumnNames.MARCA Is Nothing Then
							Dim ListaGtkOFM As Generic.List(Of ELL.gtkOFM) = Me.ListadoOFM(dbOFMARCA.NUMOF, dbOFMARCA.OP, dbOFMARCA.MARCA)
							'gtkOFM = Me.ListadoOFM(dbOFMARCA.NUMOF, dbOFMARCA.OP, dbOFMARCA.MARCA)(0)   '1º elemento de la lista
							'If Not gtkOFM Is Nothing Then
							If Not ListaGtkOFM Is Nothing Then
								Dim gtkOFM As ELL.gtkOFM = ListaGtkOFM(0) '1º elemento de la lista
								gtkObjetoB.Descripcion = gtkOFM.Descripcion
								gtkObjetoB.Cantidad = gtkOFM.Cantidad
							End If
						End If
						'----------------------------------------------------------------------

						gtkListObj.Add(gtkObjetoB)
					Loop While dbOFMARCA.MoveNext
				Else
					gtkListObj = Nothing
				End If
			Catch batzEx As BatzException
				gtkListObj = Nothing
				Throw batzEx
			Catch ex As Exception
				gtkListObj = Nothing
				'Throw New BatzException("error", ex)
				Throw New BatzException(ex.Message.ToString, ex)
			End Try

			Return gtkListObj
		End Function

		''' <summary>
		''' Funcion para la insercion de Ordenes de Fabricacion.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las acciones (gtkOFM).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Insertar(ByVal gtkObjeto As ELL.gtkOFM) As ELL.gtkOFM
			Return fGeneral(ELL.Acciones.Insertar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion para la modificación de Ordenes de Fabricacion.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las acciones (gtkOFM).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Modificar(ByVal gtkObjeto As ELL.gtkOFM) As ELL.gtkOFM
			Return fGeneral(ELL.Acciones.Modificar, gtkObjeto)
		End Function
		''' <summary>
		''' Funcion para la eliminación de Ordenes de Fabricacion de una incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Ordenes de Fabricacion (gtkOFM).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Borrar(ByVal gtkObjeto As ELL.gtkOFM) As ELL.gtkOFM
			Return fGeneral(ELL.Acciones.Borrar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion general para las Ordenes de Fabricacion.
		''' </summary>
		''' <param name="Accion">ELL.Acciones</param>
		''' <param name="gtkObjeto">Devuelve un Objeto gtkOFM</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkOFM) As ELL.gtkOFM
			Dim dbOFMARCA As New GertakariakLib.DAL.OFMARCA	 'Tabla de Ordenes de Fabricacion. OF (DataBase - db)
			Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
			Dim gtkGerComp As New BLL.GertakariakComponent
			Dim DescOF As String = String.Empty

			Try
				'-----------------------------------------------------------------
				'Inicio de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.BeginTransaction()
				End Select
				'-----------------------------------------------------------------
				'-----------------------------------------------------------------
				'Tabla de OFMARCA
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						'Cargamos el registro con el que vamos a trabajar.

						'-------------------------------------------------------------------------------------------------------------------------------------------------------------
						'dbOFMARCA.Where.IDOFMARCA.Value = gtkObjeto.Id
						'If gtkObjeto.IdIncidencia <> Integer.MinValue And Not (gtkObjeto.IdIncidencia.ToString Is Nothing Or gtkObjeto.IdIncidencia.ToString Is DBNull.Value) Then dbOFMARCA.Where.IDOFMARCA.Value = gtkObjeto.Id
						'-------------------------------------------------------------------------------------------------------------------------------------------------------------
						'FROGA:
						'-------------------------------------------------------------------------------------------------------------------------------------------------------------
						If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
							dbOFMARCA.Where.IDOFMARCA.Value = gtkObjeto.Id
						End If
						If gtkObjeto.IdIncidencia <> Integer.MinValue And Not (gtkObjeto.IdIncidencia.ToString Is Nothing Or gtkObjeto.IdIncidencia.ToString Is DBNull.Value) Then
							dbOFMARCA.Where.IDINCIDENCIA.Value = gtkObjeto.IdIncidencia
						End If
						'-------------------------------------------------------------------------------------------------------------------------------------------------------------

						If Not gtkObjeto.OF Is Nothing Then dbOFMARCA.Where.NUMOF.Value = gtkObjeto.OF
						If Not gtkObjeto.OP Is Nothing Then dbOFMARCA.Where.OP.Value = gtkObjeto.OP
						If Not gtkObjeto.Marca Is Nothing Then dbOFMARCA.Where.MARCA.Value = gtkObjeto.Marca
						dbOFMARCA.Query.Load()
					Case ELL.Acciones.Insertar
						'Para hacer insert
						dbOFMARCA.AddNew()
				End Select

				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'---------------------------------------------------------------------------
						'Campos a Insertar o Modificar
						'---------------------------------------------------------------------------
						If gtkObjeto.IdIncidencia = Integer.MinValue Then dbOFMARCA.s_IDINCIDENCIA = String.Empty Else dbOFMARCA.IDINCIDENCIA = gtkObjeto.IdIncidencia
						If Not gtkObjeto.Marca Is Nothing Then dbOFMARCA.MARCA = gtkObjeto.Marca
						If Not gtkObjeto.OF Is Nothing Then dbOFMARCA.NUMOF = gtkObjeto.OF
						If Not gtkObjeto.OP Is Nothing Then dbOFMARCA.OP = gtkObjeto.OP
						'---------------------------------------------------------------------------
					Case ELL.Acciones.Consultar
						'---------------------------------------------------------------------------
						'Campos a devolver en la consulta
						'---------------------------------------------------------------------------
						If Not dbOFMARCA.EOF Then
							Do
								gtkObjeto.Id = dbOFMARCA.IDOFMARCA
								If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.IDINCIDENCIA) Then gtkObjeto.IdIncidencia = dbOFMARCA.IDINCIDENCIA
								If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.MARCA) Then gtkObjeto.Marca = dbOFMARCA.MARCA
								If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.NUMOF) Then gtkObjeto.OF = dbOFMARCA.NUMOF
								If Not dbOFMARCA.IsColumnNull(DAL.OFMARCA.ColumnNames.OP) Then gtkObjeto.OP = dbOFMARCA.OP
								'-- Recogemos la descripción ------------------------------------------
								Dim gtkOFM As New ELL.gtkOFM

								Dim ListaOFM As New List(Of ELL.gtkOFM)
								ListaOFM = Me.ListadoOFM(dbOFMARCA.NUMOF, dbOFMARCA.OP, dbOFMARCA.MARCA)
								If Not ListaOFM Is Nothing Then
									gtkOFM = ListaOFM(0)
								End If

								'gtkOFM = Me.ListadoOFM(dbOFMARCA.NUMOF, dbOFMARCA.OP, dbOFMARCA.MARCA)(0)

								If Not gtkOFM Is Nothing Then
									gtkObjeto.Descripcion = gtkOFM.Descripcion
									gtkObjeto.Cantidad = gtkOFM.Cantidad
								End If
								'----------------------------------------------------------------------

							Loop While dbOFMARCA.MoveNext
						Else
							gtkObjeto = Nothing
						End If
						'---------------------------------------------------------------------------
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'Realizamos la accion correspondiente (Insertar, Modificar)
						dbOFMARCA.Save()
						gtkObjeto.Id = dbOFMARCA.IDOFMARCA
					Case ELL.Acciones.Borrar
						dbOFMARCA.DeleteAll()
						dbOFMARCA.Save()
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

			Catch ex As ApplicationException
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				Throw
			Catch ex As Exception
				Log.Error(ex)
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				Throw
			End Try
			Return gtkObjeto
		End Function

		''' <summary>
		''' Listado de OF/OP/MARCA en el XBAT.
		''' Obtenemos los valores de la OF.
		''' </summary>
		Public Function ListadoOFM(ByVal NumOF As Integer, ByVal OP As Integer, ByVal Marca As String) As List(Of ELL.gtkOFM)
			Dim gtkOFM As ELL.gtkOFM
			Dim dbTabla As New GertakariakLib.DAL.W_CPLISMAT  'Tabla de W_CPLISMAT (DataBase - db) 
			Dim gtkListObj As New List(Of ELL.gtkOFM)

			If NumOF <> Integer.MinValue Then dbTabla.Where.NUMORD.Value = NumOF
			If OP <> Integer.MinValue Then dbTabla.Where.NUMOPE.Value = OP
			'If Not Marca Is Nothing Then dbTabla.Where.NUMMAR.Value = Marca
			If Not String.IsNullOrWhiteSpace(Marca) Then dbTabla.Where.NUMMAR.Value = Marca
			'dbTabla.Query.AddOrderBy(DAL.W_CPLISMAT.ColumnNames.NUMMAR, WhereParameter.Dir.ASC)
			'dbTabla.Query.AddOrderBy(DAL.W_CPLISMAT.ColumnNames.MATERIAL, WhereParameter.Dir.ASC)
			dbTabla.Query.Load()

			If Not dbTabla.EOF Then
				Do
					gtkOFM = New ELL.gtkOFM	   'Nuevo Objeto que se va metiendo en la lista
					If Not dbTabla.IsColumnNull(DAL.W_CPLISMAT.ColumnNames.NUMORD) Then gtkOFM.OF = dbTabla.NUMORD
					If Not dbTabla.IsColumnNull(DAL.W_CPLISMAT.ColumnNames.NUMOPE) Then gtkOFM.OP = dbTabla.NUMOPE
					If Not dbTabla.IsColumnNull(DAL.W_CPLISMAT.ColumnNames.NUMMAR) Then gtkOFM.Marca = dbTabla.NUMMAR
					If Not dbTabla.IsColumnNull(DAL.W_CPLISMAT.ColumnNames.MATERIAL) Then gtkOFM.Descripcion = dbTabla.MATERIAL
					If Not dbTabla.IsColumnNull(DAL.W_CPLISMAT.ColumnNames.CANNEC) Then gtkOFM.Cantidad = dbTabla.CANNEC
					gtkListObj.Add(gtkOFM)
				Loop While dbTabla.MoveNext
			Else
				gtkListObj = Nothing
			End If

			Return gtkListObj
		End Function
	End Class

	''' <summary>
	''' Funciones para la gestion de los usuarios que detectaron la Incidencia.
	''' </summary>
	<Obsolete("NO USAR. Usar Entities FrameWork.", False)> _
	Public Class gtkDeteccionComponent
		Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkDeteccionComponent")

		''' <summary>
		''' Consultamos las Detecciones asociadas a la Incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Detecciones (gtkDeteccion).</param>
		''' <returns>Devuleve un objeto gtkDeteccion.</returns>
		Public Function Consultar(ByVal gtkObjeto As ELL.gtkDeteccion) As ELL.gtkDeteccion
			Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
		End Function

		''' <summary>
		''' Consultamos las Detecciones asociadas a la incidencia.
		''' </summary>
		''' <param name="Id">Identificador de la Deteccion.</param>
		''' <returns>Devuleve una lista de objetos gtkDeteccion.</returns>
		''' <remarks></remarks>
		Public Function Consultar(ByVal Id As Integer) As ELL.gtkDeteccion
			Dim gtkObjeto As New ELL.gtkDeteccion
			gtkObjeto.Id = Id
			Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
		End Function

		''' <summary>
		''' Consultamos las Detecciones asociadas a la incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Detecciones (gtkDeteccion).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkDeteccion) As List(Of ELL.gtkDeteccion)
			Dim dbTablaBBDD As New GertakariakLib.DAL.DETECCION	 'Tabla DETECCION. (DataBase - db)            
			Dim gtkListObj As New List(Of ELL.gtkDeteccion)

			If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then dbTablaBBDD.Where.ID.Value = gtkObjeto.Id
			If gtkObjeto.IdDepartamento <> Integer.MinValue And Not (gtkObjeto.IdDepartamento.ToString Is Nothing Or gtkObjeto.IdDepartamento.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDDEPARTAMENTO.Value = gtkObjeto.IdDepartamento
			If gtkObjeto.IdIncidencia <> Integer.MinValue And Not (gtkObjeto.IdIncidencia.ToString Is Nothing Or gtkObjeto.IdIncidencia.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDINCIDENCIA.Value = gtkObjeto.IdIncidencia
			If gtkObjeto.IdUsuario <> Integer.MinValue And Not (gtkObjeto.IdUsuario.ToString Is Nothing Or gtkObjeto.IdUsuario.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDUSUARIO.Value = gtkObjeto.IdUsuario

			dbTablaBBDD.Query.Load()
			If Not dbTablaBBDD.EOF Then
				Do
					Dim gtkObjetoB As New ELL.gtkDeteccion	  'Nuevo Objeto que se va metiendo en la lista
					If Not dbTablaBBDD.IsColumnNull(DAL.DETECCION.ColumnNames.ID) Then gtkObjetoB.Id = dbTablaBBDD.ID
					If Not dbTablaBBDD.IsColumnNull(DAL.DETECCION.ColumnNames.IDDEPARTAMENTO) Then gtkObjetoB.IdDepartamento = dbTablaBBDD.IDDEPARTAMENTO
					If Not dbTablaBBDD.IsColumnNull(DAL.DETECCION.ColumnNames.IDINCIDENCIA) Then gtkObjetoB.IdIncidencia = dbTablaBBDD.IDINCIDENCIA
					If Not dbTablaBBDD.IsColumnNull(DAL.DETECCION.ColumnNames.IDUSUARIO) Then gtkObjetoB.IdUsuario = dbTablaBBDD.IDUSUARIO
					gtkObjetoB = fGeneral(ELL.Acciones.Consultar, gtkObjetoB)	'Rellenamos los campos que faltan
					gtkListObj.Add(gtkObjetoB)
				Loop While dbTablaBBDD.MoveNext
			Else
				gtkListObj = Nothing
			End If
			Return gtkListObj
		End Function

		''' <summary>
		''' Funcion para la insercion de las Detecciones.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Detecciones (gtkDeteccion).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Insertar(ByVal gtkObjeto As ELL.gtkDeteccion) As ELL.gtkDeteccion
			Return fGeneral(ELL.Acciones.Insertar, gtkObjeto)
		End Function
		''' <summary>
		''' Funcion para la modificación de las Detecciones.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Detecciones (gtkDeteccion).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Modificar(ByVal gtkObjeto As ELL.gtkDeteccion) As ELL.gtkDeteccion
			Return fGeneral(ELL.Acciones.Modificar, gtkObjeto)
		End Function
		''' <summary>
		''' Funcion para la eliminación de la Detección de una incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Detecciones (gtkDeteccion).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Borrar(ByVal gtkObjeto As ELL.gtkDeteccion) As ELL.gtkDeteccion
			Return fGeneral(ELL.Acciones.Borrar, gtkObjeto)
		End Function
		''' <summary>
		''' Funcion general para las Detecciones.
		''' </summary>
		''' <param name="Accion">ELL.Acciones</param>
		''' <param name="gtkObjeto">Devuelve un Objeto gtkDeteccion</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkDeteccion) As ELL.gtkDeteccion
			Dim dbTablaBBDD As New GertakariakLib.DAL.DETECCION	 'Tabla DETECCION. (DataBase - db)
			Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr

			Dim FunGen As New SabLib.BLL.UsuariosComponent 'Funciones genericas

			Try
				'-----------------------------------------------------------------
				'Inicio de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.BeginTransaction()
				End Select
				'-----------------------------------------------------------------
				'-----------------------------------------------------------------
				'Tabla de OFMARCA
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						'Cargamos el registro con el que vamos a trabajar.
						If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then dbTablaBBDD.Where.ID.Value = gtkObjeto.Id
						If gtkObjeto.IdDepartamento <> Integer.MinValue And Not (gtkObjeto.IdDepartamento.ToString Is Nothing Or gtkObjeto.IdDepartamento.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDDEPARTAMENTO.Value = gtkObjeto.IdDepartamento
						If gtkObjeto.IdIncidencia <> Integer.MinValue And Not (gtkObjeto.IdIncidencia.ToString Is Nothing Or gtkObjeto.IdIncidencia.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDINCIDENCIA.Value = gtkObjeto.IdIncidencia
						If gtkObjeto.IdUsuario <> Integer.MinValue And Not (gtkObjeto.IdUsuario.ToString Is Nothing Or gtkObjeto.IdUsuario.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDUSUARIO.Value = gtkObjeto.IdUsuario
						dbTablaBBDD.Query.Load()
					Case ELL.Acciones.Insertar
						'Para hacer insert
						dbTablaBBDD.AddNew()
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'---------------------------------------------------------------------------
						'Campos a Insertar o Modificar
						'---------------------------------------------------------------------------
						If gtkObjeto.IdDepartamento = Integer.MinValue Then dbTablaBBDD.s_IDDEPARTAMENTO = String.Empty Else dbTablaBBDD.s_IDDEPARTAMENTO = gtkObjeto.IdDepartamento
						If gtkObjeto.IdIncidencia = Integer.MinValue Then dbTablaBBDD.s_IDINCIDENCIA = String.Empty Else dbTablaBBDD.s_IDINCIDENCIA = gtkObjeto.IdIncidencia
						If gtkObjeto.IdUsuario = Integer.MinValue Then dbTablaBBDD.s_IDUSUARIO = String.Empty Else dbTablaBBDD.s_IDUSUARIO = gtkObjeto.IdUsuario
						'---------------------------------------------------------------------------
					Case ELL.Acciones.Consultar
						'---------------------------------------------------------------------------
						'Campos a devolver en la consulta
						'---------------------------------------------------------------------------
						If Not dbTablaBBDD.EOF Then
							'- Si la consulta devuelve mas de un registro cogemos el 1º --
							If Not dbTablaBBDD.IsColumnNull(DAL.DETECCION.ColumnNames.ID) Then gtkObjeto.Id = dbTablaBBDD.ID
							If Not dbTablaBBDD.IsColumnNull(DAL.DETECCION.ColumnNames.IDDEPARTAMENTO) Then gtkObjeto.IdDepartamento = dbTablaBBDD.IDDEPARTAMENTO
							If Not dbTablaBBDD.IsColumnNull(DAL.DETECCION.ColumnNames.IDINCIDENCIA) Then gtkObjeto.IdIncidencia = dbTablaBBDD.IDINCIDENCIA
							If Not dbTablaBBDD.IsColumnNull(DAL.DETECCION.ColumnNames.IDUSUARIO) Then
								gtkObjeto.IdUsuario = dbTablaBBDD.IDUSUARIO
								Dim gtkTrabajador As New SabLib.ELL.Usuario
								gtkTrabajador = FunGen.GetUsuario(New SabLib.ELL.Usuario With {.Id = dbTablaBBDD.IDUSUARIO}, False)
								'If Not String.IsNullOrWhiteSpace(gtkTrabajador.Izena) Then
								'    gtkObjeto.NombreTrabajador = gtkTrabajador.Izena
								'Else
								'    gtkObjeto.NombreTrabajador = gtkTrabajador.NombreUsuario
								'End If
							End If
							'-------------------------------------------------------------
						Else
							gtkObjeto = Nothing
						End If
						'---------------------------------------------------------------------------
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'Realizamos la accion correspondiente (Insertar, Modificar)
						dbTablaBBDD.Save()
						gtkObjeto.Id = dbTablaBBDD.ID
					Case ELL.Acciones.Borrar
						dbTablaBBDD.DeleteAll()
						dbTablaBBDD.Save()
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

			Catch ex As ApplicationException
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				Throw
			Catch ex As Exception
				Log.Error(ex)
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				Throw
			End Try
			Return gtkObjeto
		End Function
	End Class

	''' <summary>
	''' Funciones para la gestion de las Lineas de Coste de la Incidencia.
	''' </summary>
	Public Class gtkLineaCosteComponent
		Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkLineaCosteComponent")

		''' <summary>
		''' Consultamos las Lineas de Coste asociadas a la Incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Detecciones (gtkLineaCoste).</param>
		''' <returns>Devuleve un objeto gtkLineaCoste.</returns>
		Public Function Consultar(ByVal gtkObjeto As ELL.gtkLineaCoste) As ELL.gtkLineaCoste
			Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
		End Function

		''' <summary>
		''' Consultamos las Lineas de Coste asociadas a la incidencia.
		''' </summary>
		''' <param name="Id">Identificador de la Linea de Coste.</param>
		''' <returns>Devuleve una lista de objetos gtkLineaCoste.</returns>
		''' <remarks></remarks>
		Public Function Consultar(ByVal Id As Integer) As ELL.gtkLineaCoste
			Dim gtkObjeto As New ELL.gtkLineaCoste
			gtkObjeto.Id = Id
			Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
		End Function

		''' <summary>
		''' Consultamos las Lineas de Coste asociadas a la incidencia.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de Lineas de Coste (gtkLineaCoste).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkLineaCoste) As List(Of ELL.gtkLineaCoste)
			Dim dbTablaBBDD As New GertakariakLib.DAL.LINEASCOSTE  'Tabla LINEASCOSTE. (DataBase - db)            
			Dim gtkListObj As New List(Of ELL.gtkLineaCoste)

            If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then dbTablaBBDD.Where.ID.Value = gtkObjeto.Id
            If gtkObjeto.IdIncidencia <> Integer.MinValue And Not (gtkObjeto.IdIncidencia.ToString Is Nothing Or gtkObjeto.IdIncidencia.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDINCIDENCIA.Value = gtkObjeto.IdIncidencia
            If gtkObjeto.IdOFM <> Integer.MinValue And Not (gtkObjeto.IdOFM.ToString Is Nothing Or gtkObjeto.IdOFM.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDOFMARCA.Value = gtkObjeto.IdOFM

            dbTablaBBDD.Query.AddOrderBy(DAL.LINEASCOSTE.ColumnNames.ID, WhereParameter.Dir.ASC)

			dbTablaBBDD.Query.Load()
			If Not dbTablaBBDD.EOF Then
				Do
					Dim gtkObjetoB As New ELL.gtkLineaCoste	   'Nuevo Objeto que se va metiendo en la lista
					If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.ID) Then gtkObjetoB.Id = dbTablaBBDD.ID
					If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.IDINCIDENCIA) Then gtkObjetoB.IdIncidencia = dbTablaBBDD.IDINCIDENCIA
					If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.IDOFMARCA) Then gtkObjetoB.IdOFM = dbTablaBBDD.IDOFMARCA

					'gtkListObj.Add(gtkObjetoB)
					gtkListObj.Add(fGeneral(ELL.Acciones.Consultar, gtkObjetoB))

				Loop While dbTablaBBDD.MoveNext
			Else
				gtkListObj = Nothing
			End If
			Return gtkListObj
		End Function

		''' <summary>
		''' Funcion para la insercion de las Lineas de Coste.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Lineas de Coste (gtkLineaCoste).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Insertar(ByVal gtkObjeto As ELL.gtkLineaCoste) As ELL.gtkLineaCoste
			Return fGeneral(ELL.Acciones.Insertar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion para la modificación de las Lineas de Coste.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Lineas de Coste (gtkLineaCoste).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Modificar(ByVal gtkObjeto As ELL.gtkLineaCoste) As ELL.gtkLineaCoste
			Return fGeneral(ELL.Acciones.Modificar, gtkObjeto)
		End Function
		''' <summary>
		''' Funcion para la eliminación de Lineas de Coste.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de Lineas de Coste (gtkLineaCoste).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Borrar(ByVal gtkObjeto As ELL.gtkLineaCoste) As ELL.gtkLineaCoste
			Return fGeneral(ELL.Acciones.Borrar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion general para las Lineas de Coste.
		''' </summary>
		''' <param name="Accion">ELL.Acciones</param>
		''' <param name="gtkObjeto">Devuelve un Objeto gtkLineaCoste</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkLineaCoste) As ELL.gtkLineaCoste
			Dim dbTablaBBDD As New GertakariakLib.DAL.LINEASCOSTE  'Tabla LINEASCOSTE. (DataBase - db)
			Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
			Dim Funciones As New BLL.gtkLineaCosteComponent

            Try
                '-----------------------------------------------------------------
                'Inicio de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.BeginTransaction()
                End Select
                '-----------------------------------------------------------------
                '-----------------------------------------------------------------
                'Tabla de LINEASCOSTE
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        'Cargamos el registro con el que vamos a trabajar.
                        If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then dbTablaBBDD.Where.ID.Value = gtkObjeto.Id
                        'If gtkObjeto.IdIncidencia <> Integer.MinValue And Not (gtkObjeto.IdIncidencia.ToString Is Nothing Or gtkObjeto.IdIncidencia.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDINCIDENCIA.Value = gtkObjeto.IdIncidencia
                        'If gtkObjeto.IdOFM <> Integer.MinValue And Not (gtkObjeto.IdOFM.ToString Is Nothing Or gtkObjeto.IdOFM.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDOFMARCA.Value = gtkObjeto.IdOFM

                        dbTablaBBDD.Query.AddOrderBy(DAL.LINEASCOSTE.ColumnNames.ID, WhereParameter.Dir.ASC)
                        dbTablaBBDD.Query.Load()
                        'Log.Debug(dbTablaBBDD.Query.LastQuery)
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbTablaBBDD.AddNew()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If gtkObjeto.IdIncidencia = Integer.MinValue Then dbTablaBBDD.s_IDINCIDENCIA = String.Empty Else dbTablaBBDD.s_IDINCIDENCIA = gtkObjeto.IdIncidencia.ToString

                        If gtkObjeto.IdOFM = Integer.MinValue Then dbTablaBBDD.s_IDOFMARCA = String.Empty Else dbTablaBBDD.s_IDOFMARCA = gtkObjeto.IdOFM
                        '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - - 
                        If gtkObjeto.CantidadRec = Decimal.MinValue Then dbTablaBBDD.s_CANTIDADFAC = String.Empty Else dbTablaBBDD.s_CANTIDADFAC = gtkObjeto.CantidadRec
                        If gtkObjeto.CantidadPed = Decimal.MinValue Then dbTablaBBDD.s_CANTIDADPED = String.Empty Else dbTablaBBDD.s_CANTIDADPED = gtkObjeto.CantidadPed
                        If Not gtkObjeto.CapacidadProv Is Nothing Then dbTablaBBDD.CAPACIDADPROV = gtkObjeto.CapacidadProv.ToString.Trim
                        If Not gtkObjeto.CodArt Is Nothing Then dbTablaBBDD.CODART = gtkObjeto.CodArt.ToString.Trim
                        If Not gtkObjeto.CodPro Is Nothing Then dbTablaBBDD.CODPRO = gtkObjeto.CodPro.ToString.Trim
                        If Not gtkObjeto.DescLinea Is Nothing Then dbTablaBBDD.DESCRIPCION = gtkObjeto.DescLinea.ToString.Trim
                        If Not gtkObjeto.Fase Is Nothing Then dbTablaBBDD.FASE = gtkObjeto.Fase.ToString.Trim
                        If gtkObjeto.Horas = Decimal.MinValue Then dbTablaBBDD.s_HORAS = String.Empty Else dbTablaBBDD.HORAS = gtkObjeto.Horas
                        If gtkObjeto.Importe = Decimal.MinValue Then dbTablaBBDD.s_IMPORTE = String.Empty Else dbTablaBBDD.IMPORTE = gtkObjeto.Importe
                        If Not gtkObjeto.Maquina Is Nothing Then dbTablaBBDD.MAQUINA = gtkObjeto.Maquina.ToString.Trim
                        If gtkObjeto.NumLin = Integer.MinValue Then dbTablaBBDD.s_NUMLIN = String.Empty Else dbTablaBBDD.s_NUMLIN = gtkObjeto.NumLin
                        If Not gtkObjeto.NumMar Is Nothing Then dbTablaBBDD.NUMMAR = gtkObjeto.NumMar.ToString.Trim
                        If gtkObjeto.NumOpe = Integer.MinValue Then dbTablaBBDD.s_NUMOPE = String.Empty Else dbTablaBBDD.s_NUMOPE = gtkObjeto.NumOpe
                        If gtkObjeto.NumOrd = Integer.MinValue Then dbTablaBBDD.s_NUMORD = String.Empty Else dbTablaBBDD.s_NUMORD = gtkObjeto.NumOrd
                        If gtkObjeto.NumPed = Integer.MinValue Then dbTablaBBDD.s_NUMPED = String.Empty Else dbTablaBBDD.s_NUMPED = gtkObjeto.NumPed
                        If gtkObjeto.NumPedOrigen = Integer.MinValue Then dbTablaBBDD.s_NUMPEDORIGEN = String.Empty Else dbTablaBBDD.s_NUMPEDORIGEN = gtkObjeto.NumPedOrigen
                        If Not gtkObjeto.Origen Is Nothing Then dbTablaBBDD.ORIGEN = gtkObjeto.Origen.ToString.Trim
                        If Not gtkObjeto.Proceso Is Nothing Then dbTablaBBDD.PROCESO = gtkObjeto.Proceso.ToString.Trim
                        If Not gtkObjeto.Seccion Is Nothing Then dbTablaBBDD.SECCION = gtkObjeto.Seccion.ToString.Trim
                        '---------------------------------------------------------------------------                        
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbTablaBBDD.EOF Then
                            '- Si la consulta devuelve mas de un registro cogemos el 1º --
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.ID) Then gtkObjeto.Id = dbTablaBBDD.ID
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.IDINCIDENCIA) Then gtkObjeto.IdIncidencia = dbTablaBBDD.IDINCIDENCIA
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.IDOFMARCA) Then gtkObjeto.IdOFM = dbTablaBBDD.IDOFMARCA
                            '- - - - - - - - - - - - - - - - - - - - - - - - - - - - - - -
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.CANTIDADFAC) Then gtkObjeto.CantidadRec = dbTablaBBDD.CANTIDADFAC
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.CAPACIDADPROV) Then gtkObjeto.CapacidadProv = dbTablaBBDD.CAPACIDADPROV
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.CODART) Then gtkObjeto.CodArt = dbTablaBBDD.CODART
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.CODPRO) Then gtkObjeto.CodPro = dbTablaBBDD.CODPRO
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.DESCRIPCION) Then gtkObjeto.DescLinea = dbTablaBBDD.DESCRIPCION Else Funciones.DescLinea(gtkObjeto) 'Descripcion de la linea de coste
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.FASE) Then gtkObjeto.Fase = dbTablaBBDD.FASE
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.HORAS) Then gtkObjeto.Horas = dbTablaBBDD.HORAS
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.IMPORTE) Then gtkObjeto.Importe = dbTablaBBDD.IMPORTE
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.MAQUINA) Then gtkObjeto.Maquina = dbTablaBBDD.MAQUINA
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.NUMLIN) Then gtkObjeto.NumLin = dbTablaBBDD.NUMLIN
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.NUMMAR) Then gtkObjeto.NumMar = dbTablaBBDD.NUMMAR
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.NUMOPE) Then gtkObjeto.NumOpe = dbTablaBBDD.NUMOPE
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.NUMORD) Then gtkObjeto.NumOrd = dbTablaBBDD.NUMORD
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.NUMPED) Then gtkObjeto.NumPed = dbTablaBBDD.NUMPED
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.NUMPEDORIGEN) Then gtkObjeto.NumPedOrigen = dbTablaBBDD.NUMPEDORIGEN
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.ORIGEN) Then gtkObjeto.Origen = dbTablaBBDD.ORIGEN
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.PROCESO) Then gtkObjeto.Proceso = dbTablaBBDD.PROCESO
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.SECCION) Then gtkObjeto.Seccion = dbTablaBBDD.SECCION
                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.CANTIDADPED) Then gtkObjeto.CantidadPed = dbTablaBBDD.CANTIDADPED

                            If Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.NUMPED) And Not dbTablaBBDD.IsColumnNull(DAL.LINEASCOSTE.ColumnNames.NUMLIN) Then
                                gtkObjeto.Albaranes = GetAlbaranes(dbTablaBBDD.NUMPED, dbTablaBBDD.NUMLIN)
                            End If
                            '-------------------------------------------------------------

                        Else
                            gtkObjeto = Nothing
                        End If
                        '---------------------------------------------------------------------------
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbTablaBBDD.Save()
                        gtkObjeto.Id = dbTablaBBDD.ID
                    Case ELL.Acciones.Borrar
                        dbTablaBBDD.DeleteAll()
                        dbTablaBBDD.Save()
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

            Catch ex As ApplicationException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.RollbackTransaction()
                End Select
                gtkObjeto = Nothing
                '-----------------------------------------------------------------
                Throw
                'Catch ex As BatzException
                '    '-----------------------------------------------------------------
                '    'Fin de la transaccion
                '    '-----------------------------------------------------------------
                '    Select Case Accion
                '        Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                '            Transakzio.RollbackTransaction()
                '    End Select
                '    gtkObjeto = Nothing
                '    '-----------------------------------------------------------------
                '    throw 

                '---------------------------------------------------------------------------------------------------------------------
                '---------------------------------------------------------------------------------------------------------------------
                '---------------------------------------------------------------------------------------------------------------------
                'FROGA: 2012-02-08: A veces al crear una nueva NC se produce un error que no es del tipo "OracleException".
                'Al igualar el tipo de error que recogemos a un "OracleException" se produce un segundo error que no permite 
                'salir de la pantalla de creacion de la incidencia.
                'Si no da error quitar esto.
                '---------------------------------------------------------------------------------------------------------------------
                '---------------------------------------------------------------------------------------------------------------------
                '---------------------------------------------------------------------------------------------------------------------

            Catch ex As Oracle.ManagedDataAccess.Client.OracleException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------

				If ex.Number = 2291 Then
					'Throw New BatzException("La OF no corresponde con las lineas de coste", ex)
					Throw New ApplicationException("La OF no corresponde con las lineas de coste", ex)
				Else
					'Throw New BatzException("lineasCoste", ex)
					Throw New ApplicationException("lineasCoste", ex)
				End If

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

				Throw
			End Try
			'---------------------------------------------------------------------------------------------------------------------
			'---------------------------------------------------------------------------------------------------------------------
			'---------------------------------------------------------------------------------------------------------------------
			'FROGA: 2012-02-08: FIN
			'---------------------------------------------------------------------------------------------------------------------
			'---------------------------------------------------------------------------------------------------------------------
			'---------------------------------------------------------------------------------------------------------------------

			Return gtkObjeto
		End Function

		''' <summary>
		''' Descripción de la Linea de Coste de Materiales (W_GCLINPED).
		''' </summary>
		''' <param name="NumPed">Identificador del Pedido.</param>
		''' <param name="NumLin">Identificador de la Linea del Pedido.</param>
		Private Function DescLinea(ByVal NumPed As Integer, ByVal NumLin As Integer) As String
			Dim dbTabla As New DAL.W_GCLINPED
			Dim Descripcion As String = Nothing

			dbTabla.Where.NUMPEDLIN.Value = NumPed
			dbTabla.Where.NUMLINLIN.Value = NumLin
			dbTabla.Query.Load()

			If Not dbTabla.EOF Then
				Descripcion = dbTabla.DESCART.Trim
				If Not dbTabla.DESCLISTA Is Nothing Then
					If Not String.IsNullOrWhiteSpace(dbTabla.DESCLISTA.Trim) Then Descripcion = Descripcion & " (" & dbTabla.DESCLISTA.Trim & ")"
				End If
			End If

			Return Descripcion
		End Function

		''' <summary>
		''' Descripción de la Linea de Coste por la Seccion (W_CPSECCIO).
		''' </summary>
		''' <param name="Seccion">Identificador del Pedido.</param>
		Private Function DescLinea(ByVal Seccion As String) As String
			Dim dbTabla As New DAL.W_CPSECCIO
			Dim Descripcion As String = Nothing

			dbTabla.Where.CODSEC.Value = Seccion
			dbTabla.Query.Load()

			If Not dbTabla.EOF Then
				Descripcion = dbTabla.DESCSECCIO.Trim
			End If

			Return Descripcion
		End Function

		''' <summary>
		''' Descripción de la Linea de Coste.
		''' </summary>
		''' <param name="gtkLineaCoste">Objeto de la linea de coste.</param>
		Private Sub DescLinea(ByVal gtkLineaCoste As ELL.gtkLineaCoste)
			Select Case UCase(Trim(gtkLineaCoste.Origen))
				Case "M", "S"
					If gtkLineaCoste.NumPed <> Integer.MinValue And gtkLineaCoste.NumLin <> Integer.MinValue Then gtkLineaCoste.DescLinea = DescLinea(gtkLineaCoste.NumPed, gtkLineaCoste.NumLin)
					'Case "S"
					'If gtkLineaCoste.NumPed <> Integer.MinValue And gtkLineaCoste.NumLin <> Integer.MinValue Then gtkLineaCoste.DescLinea = DescLinea(gtkLineaCoste.NumPed, gtkLineaCoste.NumLin) 'gtkLineaCoste.DescLinea = DescLinea(gtkLineaCoste.NumLin, gtkLineaCoste.NumPed)
				Case "B"
					If gtkLineaCoste.Seccion.Trim <> "" Then gtkLineaCoste.DescLinea = DescLinea(gtkLineaCoste.Seccion)
			End Select
		End Sub

		''' <summary>
		''' Obtenemos el Listado de las Lineas de Coste del XBAT.
		''' </summary>
		''' <param name="NumOrd">Orden de Fabricacion (OF)</param>
		''' <param name="NumOpe">Operacion (OP)</param>
		''' <param name="NumMar">Identificador de la Marca</param>
		''' <param name="CodPro">Código de Proveedor</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetLineasCostes(Optional ByVal NumOrd As Integer = Integer.MinValue, Optional ByVal NumOpe As Integer = Integer.MinValue, Optional ByVal NumMar As String = Nothing, Optional ByVal CodPro As String = Nothing) As List(Of ELL.gtkLineaCoste)
			Dim lstLineasCoste As New List(Of ELL.gtkLineaCoste)
			lstLineasCoste = fgLineasCoste(NumOrd, NumOpe, NumMar, CodPro)
			Return lstLineasCoste
		End Function

		''' <summary>
		''' Obtenemos el Listado de las Lineas de Coste del XBAT.
		''' </summary>
		''' <param name="NumOrd">Orden de Fabricacion (OF)</param>
		''' <param name="NumOpe">Operacion (OP)</param>
		''' <param name="NumMar">Identificador de la Marca</param>
		''' <param name="CodPro">Código de Proveedor</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function fgLineasCoste(Optional ByVal NumOrd As Integer = Integer.MinValue, Optional ByVal NumOpe As Integer = Integer.MinValue, Optional ByVal NumMar As String = Nothing, Optional ByVal CodPro As String = Nothing) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkLineaCoste)
			Dim lstLineasCoste As New List(Of ELL.gtkLineaCoste)
			Try
				Dim dbW_COSTES As New GertakariakLib.DAL.W_COSTES
				Dim Funciones As New BLL.gtkLineaCosteComponent

				If NumOrd <> Integer.MinValue Then dbW_COSTES.Where.NUMORD.Value = NumOrd
				If NumOpe <> Integer.MinValue Then dbW_COSTES.Where.NUMOPE.Value = NumOpe

				'If Not NumMar Is Nothing Then
				'	Dim CondicionNumMar As AccesoAutomaticoBD.WhereParameter
				'	dbW_COSTES.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
				'	dbW_COSTES.Query.OpenParenthesis()
				'	CondicionNumMar = dbW_COSTES.Where.TearOff.NUMMAR
				'	CondicionNumMar.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
				'	If NumMar <> "" Then
				'		CondicionNumMar = dbW_COSTES.Where.TearOff.NUMMAR
				'		CondicionNumMar.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
				'		CondicionNumMar.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
				'		CondicionNumMar.Value = NumMar
				'	End If
				'	dbW_COSTES.Query.CloseParenthesis()
				'End If
				'If Not CodPro Is Nothing Then
				'	Dim CondicionCodPro As AccesoAutomaticoBD.WhereParameter
				'	dbW_COSTES.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
				'	dbW_COSTES.Query.OpenParenthesis()
				'	CondicionCodPro = dbW_COSTES.Where.TearOff.CODPRO
				'	CondicionCodPro.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
				'	If CodPro <> "" Then
				'		CondicionCodPro = dbW_COSTES.Where.TearOff.CODPRO
				'		CondicionCodPro.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
				'		CondicionCodPro.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
				'		CondicionCodPro.Value = CodPro
				'	End If
				'	dbW_COSTES.Query.CloseParenthesis()
				'End If

				If Not NumMar Is Nothing AndAlso NumMar <> "" Then
					Dim CondicionNumMar As AccesoAutomaticoBD.WhereParameter
					dbW_COSTES.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
					dbW_COSTES.Query.OpenParenthesis()
					CondicionNumMar = dbW_COSTES.Where.TearOff.NUMMAR
					CondicionNumMar.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
					CondicionNumMar = dbW_COSTES.Where.TearOff.NUMMAR
					CondicionNumMar.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
					CondicionNumMar.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
					CondicionNumMar.Value = NumMar
					dbW_COSTES.Query.CloseParenthesis()
				End If

				If Not CodPro Is Nothing AndAlso CodPro <> "" Then
					Dim CondicionCodPro As AccesoAutomaticoBD.WhereParameter
					dbW_COSTES.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
					dbW_COSTES.Query.OpenParenthesis()
					CondicionCodPro = dbW_COSTES.Where.TearOff.CODPRO
					CondicionCodPro.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
					CondicionCodPro = dbW_COSTES.Where.TearOff.CODPRO
					CondicionCodPro.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
					CondicionCodPro.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
					CondicionCodPro.Value = CodPro
					dbW_COSTES.Query.CloseParenthesis()
				End If
				dbW_COSTES.Query.Load()
                'dbW_COSTES.Query.GenerateSQL()

                If Not dbW_COSTES.EOF Then
					Do
						Dim gtkLineaCoste As New ELL.gtkLineaCoste

						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.CANPED) Then gtkLineaCoste.CantidadPed = dbW_COSTES.CANPED
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.CODART) Then gtkLineaCoste.CodArt = dbW_COSTES.CODART
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.CODPRO) Then gtkLineaCoste.CodPro = dbW_COSTES.CODPRO
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.FASE) Then gtkLineaCoste.Fase = dbW_COSTES.FASE
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.HORAS) Then gtkLineaCoste.Horas = dbW_COSTES.HORAS
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.IMPORTE) Then gtkLineaCoste.Importe = dbW_COSTES.IMPORTE
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.MAQUINA) Then gtkLineaCoste.Maquina = dbW_COSTES.MAQUINA
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.NUMLIN) Then gtkLineaCoste.NumLin = dbW_COSTES.NUMLIN
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.NUMMAR) Then gtkLineaCoste.NumMar = dbW_COSTES.NUMMAR
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.NUMOPE) Then gtkLineaCoste.NumOpe = dbW_COSTES.NUMOPE
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.NUMORD) Then gtkLineaCoste.NumOrd = dbW_COSTES.NUMORD
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.NUMPED) Then
							gtkLineaCoste.NumPed = dbW_COSTES.NUMPED
							'---------------------------------------------------------------------------------
							'En caso que la linea de coste no tenga Marcas las buscamos con el Nº. de pedido.
							'---------------------------------------------------------------------------------
							If gtkLineaCoste.NumMar Is Nothing Or gtkLineaCoste.NumMar = String.Empty Then
								Dim dbW_SCPEDMAR As New GertakariakLib.DAL.W_SCPEDMAR
								dbW_SCPEDMAR.Where.NUMPEDMAR.Value = gtkLineaCoste.NumPed
								dbW_SCPEDMAR.Query.Load()
								If dbW_SCPEDMAR.RowCount > 0 Then
									Do
										If gtkLineaCoste.NumMar <> String.Empty Then
											gtkLineaCoste.NumMar = gtkLineaCoste.NumMar & ", "
										End If
										gtkLineaCoste.NumMar = gtkLineaCoste.NumMar & dbW_SCPEDMAR.NUMMARMAR.Trim
									Loop While dbW_SCPEDMAR.MoveNext
								End If
							End If
							'---------------------------------------------------------------------------------
						End If
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.ORIGEN) Then gtkLineaCoste.Origen = dbW_COSTES.ORIGEN
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.PROCESO) Then gtkLineaCoste.Proceso = dbW_COSTES.PROCESO
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.SECCIO) Then gtkLineaCoste.Seccion = dbW_COSTES.SECCIO

						Funciones.DescLinea(gtkLineaCoste) 'Descripcion de la linea de coste

						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.NUMPED) And Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.NUMLIN) Then
							gtkLineaCoste.Albaranes = GetAlbaranes(dbW_COSTES.NUMPED, dbW_COSTES.NUMLIN)
						End If
						If Not dbW_COSTES.IsColumnNull(DAL.W_COSTES.ColumnNames.FECHA) Then gtkLineaCoste.Fecha = dbW_COSTES.FECHA

						lstLineasCoste.Add(gtkLineaCoste)
					Loop While dbW_COSTES.MoveNext
				End If
			Catch ex As Exception
				Log.Error(ex)
				Throw
			End Try
			Return lstLineasCoste
		End Function

		''' <summary>
		''' Obtenemos los números de Albaran relacionado con esta linea de coste.
		''' </summary>
		''' <param name="NumPed">Número de Pedido.</param>
		''' <param name="NumLin">Número de Linea.</param>
		Public Function GetAlbaranes(ByVal NumPed As Integer, ByVal NumLin As Integer) As List(Of Integer)
			Dim dbAlbaranes As New GertakariakLib.DAL.GCALBARA
			Dim Lista As New List(Of Integer)
			Try
				dbAlbaranes.Where.NUMPED.Value = NumPed
				dbAlbaranes.Where.NUMLIN.Value = NumLin
				dbAlbaranes.Query.Load()

				If Not dbAlbaranes.EOF Then
					Do
						Lista.Add(dbAlbaranes.NUMALBAR)
					Loop While dbAlbaranes.MoveNext
				Else
					Lista = Nothing
				End If

				'Catch ex As BatzException
				'    throw 
			Catch ex As Exception
				'Throw New BatzException("GetAlbaranes", ex)
				Log.Error(ex)
				Throw
			End Try

			Return Lista
		End Function
	End Class

    Public Class gtkFamiliaRepetitivaComponent
        Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkFamiliaRepetitivaComponent")

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
        ''' Funcion que devuelve todas las familias y un contador para cada una de ellas, informando el numero de incidencias que estan asociadas a dicha familia
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function fgFamiliasRepetitivasContador(ByVal arrIDsClasificacion As ArrayList) As List(Of ELL.gtkFamiliaRepetitiva)
            Dim listaFam As New List(Of ELL.gtkFamiliaRepetitiva)
            Dim familiaDAL As New DAL.FAMILIAREPETITIVA
            Dim familiaKulturaDAL As DAL.FAMILIAREPETITIVAKULTURA
            Dim stLiteral As Utils.Literal
            Dim dr As IDataReader = Nothing
            Dim oFamilia As ELL.gtkFamiliaRepetitiva
            Dim arrayKulturas As New ArrayList

            Try
                For Each Id As Integer In arrIDsClasificacion
                    dr = familiaDAL.fgFamiliasRepetitivasContador(Id)
                    While dr.Read
                        oFamilia = New ELL.gtkFamiliaRepetitiva
                        oFamilia.Id = CInt(dr(ELL.gtkFamiliaRepetitiva.ColumnNames.ID))
                        oFamilia.NumeroRegistros = dr(ELL.gtkFamiliaRepetitiva.ColumnNames.NUMERO_REGISTROS)
                        oFamilia.Orden = Utils.integerNull(dr(DAL.FAMILIAREPETITIVA.ColumnNames.ORDEN))
                        'oFamilia.ClasificacionFamilia = Id
                        oFamilia.IdClasificacionRepetitiva = CInt(dr(ELL.gtkFamiliaRepetitiva.ColumnNames.IDCLASIFICACIONFAMILIA))

                        'Obtenemos el campo de la descripcion traducido
                        familiaKulturaDAL = getFamiliaKulturasByIdFamilia(oFamilia.Id)
                        If (familiaKulturaDAL.RowCount > 0) Then
                            stLiteral = Utils.TraducirCampo(familiaKulturaDAL, DAL.FAMILIAREPETITIVAKULTURA.ColumnNames.DESCRIPCION.ToString, DAL.FAMILIAREPETITIVAKULTURA.ColumnNames.IDCULTURA.ToString, Me.Kultura)
                            oFamilia.Descripcion = stLiteral.Descripcion
                            oFamilia.Kultura = stLiteral.IdCultura
                        End If
                        listaFam.Add(oFamilia)
                    End While
                    dr.Close()
                Next
                'Catch ex As BatzException
                '    throw 
            Catch ex As Exception
                'Throw New BatzException("error", ex)
                'Throw New BatzException(ex.Message.ToString, ex)
                Throw
            Finally
                If Not (dr Is Nothing) Then dr.Close()
            End Try

            Return listaFam
        End Function
        Private Function getFamiliaKulturasByIdFamilia(ByVal idFamilia As Integer) As DAL.FAMILIAREPETITIVAKULTURA
            Dim familiaKulturaDAL As New DAL.FAMILIAREPETITIVAKULTURA
            familiaKulturaDAL.Where.IDFAMILIAREPETITIVA.Operator = WhereParameter.Operand.Equal
            familiaKulturaDAL.Where.IDFAMILIAREPETITIVA.Value = idFamilia
            familiaKulturaDAL.Query.Load()

            Return familiaKulturaDAL
        End Function

        ''' <summary>
        ''' Funcion para la insercion de Familias para los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Familias (gtkFamiliaRepetitiva).</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function Insertar(ByVal gtkObjeto As ELL.gtkFamiliaRepetitiva) As ELL.gtkFamiliaRepetitiva
            gtkObjeto = fGeneral(ELL.Acciones.Insertar, gtkObjeto)
            Return gtkObjeto
        End Function
        ''' <summary>
        ''' Funcion para la modificacion de Familias para los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Familias (gtkFamiliaRepetitiva).</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function Modificar(ByVal gtkObjeto As ELL.gtkFamiliaRepetitiva) As ELL.gtkFamiliaRepetitiva
            Return fGeneral(ELL.Acciones.Modificar, gtkObjeto)
        End Function
        ''' <summary>
        ''' Funcion para la eliminacion de las Familas de los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Familias (gtkFamiliaRepetitiva).</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function Borrar(ByVal gtkObjeto As ELL.gtkFamiliaRepetitiva) As ELL.gtkFamiliaRepetitiva
            Return fGeneral(ELL.Acciones.Borrar, gtkObjeto)
        End Function

        ''' <summary>
        ''' Funcion para la eliminacion de las Familas de los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="Id">Identificador del registro.</param>
        ''' <remarks></remarks>
        Public Sub Borrar(ByVal Id As Integer)
            Dim Lista As New List(Of ELL.gtkFamiliaRepetitiva)
            Try
                If Contador(Id) <> 0 Then
                    'Throw New System.Exception("errBorrarRelacionado")
                    Throw New ApplicationException("errBorrarRelacionado")
                End If
                '-- Borramos el registro cultura a cultura --
                Lista = Consultar(Id)
                For Each Item As ELL.gtkFamiliaRepetitiva In Lista
                    Borrar(Item)
                Next
                '--------------------------------------------
            Catch ex As ApplicationException
                Throw
                'Catch ex As BatzException
                '	throw 
            Catch ex As Exception
                'Throw New BatzException(ex.Message, ex)
                Log.Error(ex)
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Funcion general para las Familias Repetitvas (Familias de Tipos de Incidencias Repetitivas).
        ''' </summary>
        ''' <param name="Accion">Accion a realizar con el Objeto (ELL.Acciones)</param>
        ''' <param name="gtkObjeto">Devuelve un Objeto gtkFamiliaRepetitiva</param>
        ''' <returns>ELL.gtkFamiliaRepetitiva</returns>
        ''' <remarks></remarks>
        Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkFamiliaRepetitiva) As ELL.gtkFamiliaRepetitiva
            Dim dbFamiliaRepetitiva As New GertakariakLib.DAL.FAMILIAREPETITIVA  'Tabla de FAMILIAREPETITIVA (DataBase - db)
            Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                '-----------------------------------------------------------------
                '- Comprobamos que el termino que se pasa exista en la BB.DD -----
                '-----------------------------------------------------------------
                If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
                    Select Case Accion
                        Case ELL.Acciones.Insertar
                            dbFamiliaRepetitiva.Where.ID.Value = gtkObjeto.Id
                            dbFamiliaRepetitiva.Query.Load()
                            If Not dbFamiliaRepetitiva.EOF Then
                                'Si existe lo Modificamos
                                Accion = ELL.Acciones.Modificar
                            Else
                                'Si no existe lo Insertamos
                                Accion = ELL.Acciones.Insertar
                            End If
                            dbFamiliaRepetitiva.FlushData()
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
                'Tabla de FAMILIAREPETITIVA
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbFamiliaRepetitiva.AddNew()
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        'Cargamos el registro con el que vamos a trabajar.
                        dbFamiliaRepetitiva.Where.ID.Value = gtkObjeto.Id
                        dbFamiliaRepetitiva.Query.Load()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If gtkObjeto.Orden = Integer.MinValue Then dbFamiliaRepetitiva.s_ORDEN = String.Empty Else dbFamiliaRepetitiva.ORDEN = gtkObjeto.Orden
                        If gtkObjeto.IdClasificacionRepetitiva = Integer.MinValue Then dbFamiliaRepetitiva.s_IDCLASIFICACIONFAMILIA = String.Empty Else dbFamiliaRepetitiva.IDCLASIFICACIONFAMILIA = gtkObjeto.IdClasificacionRepetitiva
                        dbFamiliaRepetitiva.OBSOLETO = gtkObjeto.Obsoleto
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbFamiliaRepetitiva.IsColumnNull(DAL.FAMILIAREPETITIVA.ColumnNames.ORDEN) Then gtkObjeto.Orden = dbFamiliaRepetitiva.ORDEN
                        If Not dbFamiliaRepetitiva.IsColumnNull(DAL.FAMILIAREPETITIVA.ColumnNames.IDCLASIFICACIONFAMILIA) Then gtkObjeto.IdClasificacionRepetitiva = dbFamiliaRepetitiva.IDCLASIFICACIONFAMILIA
                        If Not dbFamiliaRepetitiva.IsColumnNull(DAL.CAUSASNC.ColumnNames.OBSOLETO) Then gtkObjeto.Obsoleto = dbFamiliaRepetitiva.OBSOLETO
                        '---------------------------------------------------------------------------
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbFamiliaRepetitiva.Save()
                        gtkObjeto.Id = dbFamiliaRepetitiva.ID
                End Select
                '-----------------------------------------------------------------

                '-----------------------------------------------------------------
                'Tabla de FAMILIAREPETITIVAKULTURA
                '-----------------------------------------------------------------
                Dim dbFamiliaRepetitivaKul As New DAL.FAMILIAREPETITIVAKULTURA
                Select Case Accion
                    Case ELL.Acciones.Consultar
                        If Not gtkObjeto.Kultura Is Nothing Then
                            gtkObjeto = fgKultura(Accion, gtkObjeto)
                        Else
                            Dim CampoTraducido As New GertakariakLib.BLL.Utils.Literal
                            dbFamiliaRepetitivaKul.Where.IDFAMILIAREPETITIVA.Value = gtkObjeto.Id
                            dbFamiliaRepetitivaKul.Query.Load()
                            CampoTraducido = GertakariakLib.BLL.Utils.TraducirCampo(dbFamiliaRepetitivaKul, DAL.FAMILIAREPETITIVAKULTURA.ColumnNames.DESCRIPCION.ToString, DAL.FAMILIAREPETITIVAKULTURA.ColumnNames.IDCULTURA.ToString, Me.Kultura)
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
                        dbFamiliaRepetitivaKul.Where.IDFAMILIAREPETITIVA.Value = gtkObjeto.Id
                        dbFamiliaRepetitivaKul.Query.Load()
                        If dbFamiliaRepetitivaKul.EOF Then
                            dbFamiliaRepetitiva.DeleteAll()
                            dbFamiliaRepetitiva.Save()
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

            Catch ex As ApplicationException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.RollbackTransaction()
                End Select
                gtkObjeto = Nothing
                '-----------------------------------------------------------------
                Throw
                'Catch ex As BatzException
                '    '-----------------------------------------------------------------
                '    'Fin de la transaccion
                '    '-----------------------------------------------------------------
                '    Select Case Accion
                '        Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                '            Transakzio.RollbackTransaction()
                '    End Select
                '    gtkObjeto = Nothing
                '    '-----------------------------------------------------------------
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
                'Throw New BatzException("error", ex)
                'Throw New BatzException(ex.Message.ToString, ex)
                Log.Error(ex)
                Throw
            End Try
            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Consultamos las Familias para los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Familias (gtkFamiliaRepetitiva).</param>
        ''' <returns>Devuleve un objeto gtkFamiliaRepetitiva.</returns>
        Public Function Consultar(ByVal gtkObjeto As ELL.gtkFamiliaRepetitiva) As ELL.gtkFamiliaRepetitiva
            Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
        End Function
        ''' <summary>
        ''' Consultamos las Familias para los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="Id">Identificador del registro.</param>
        ''' <returns>Devuleve una lista de objetos gtkFamiliaRepetitiva.</returns>
        ''' <remarks></remarks>
        Public Function Consultar(ByVal Id As Integer) As List(Of ELL.gtkFamiliaRepetitiva)
            Dim gtkObjeto As New ELL.gtkFamiliaRepetitiva
            gtkObjeto.Id = Id
            Return ConsultarListado(gtkObjeto)
        End Function
        ''' <summary>
        ''' Consultamos las Familias para los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="arrIDsClasificacion">Vector de identificadores de la clasificacion de la familia.</param>
        ''' <returns>Devuleve una lista de objetos gtkFamiliaRepetitiva.</returns>
        ''' <remarks></remarks>
        ''' 
        Public Function Consultar(Optional ByVal arrIDsClasificacion As ArrayList = Nothing) As List(Of ELL.gtkFamiliaRepetitiva)
            If arrIDsClasificacion Is Nothing Then
                arrIDsClasificacion = New ArrayList
                Dim gtkCLComponent As New BLL.gtkClasificacionRepetitivaComponent
                For Each gtkClasificacionRepetitiva As ELL.gtkClasificacionRepetitiva In gtkCLComponent.ConsultarListado()
                    arrIDsClasificacion.Add(gtkClasificacionRepetitiva.Id)
                Next
            End If
            Return fgFamiliasRepetitivasContador(arrIDsClasificacion)
        End Function

        ''' <summary>
        ''' Consultamos las Familias para los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Familias para los Tipos de Incidencia Repetitiva (gtkFamiliaRepetitiva).</param>
        ''' <returns>Devuleve un objeto de gtkFamiliaRepetitiva.</returns>
        Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkFamiliaRepetitiva) As List(Of ELL.gtkFamiliaRepetitiva)
            Dim lstObjeto As New List(Of GertakariakLib.ELL.gtkFamiliaRepetitiva)
            Dim dbGtkFamiliaRepetitivaKultura As New DAL.FAMILIAREPETITIVAKULTURA
            If gtkObjeto.Id <> Integer.MinValue Then dbGtkFamiliaRepetitivaKultura.Where.IDFAMILIAREPETITIVA.Value = gtkObjeto.Id
            If Not gtkObjeto.Kultura Is Nothing Then dbGtkFamiliaRepetitivaKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
            dbGtkFamiliaRepetitivaKultura.Query.Load()
            If dbGtkFamiliaRepetitivaKultura.RowCount > 0 Then
                Do
                    Dim nObjeto As New ELL.gtkFamiliaRepetitiva
                    nObjeto.Id = dbGtkFamiliaRepetitivaKultura.IDFAMILIAREPETITIVA
                    nObjeto.Kultura = dbGtkFamiliaRepetitivaKultura.IDCULTURA
                    gtkObjeto = fGeneral(ELL.Acciones.Consultar, nObjeto)
                    lstObjeto.Add(gtkObjeto)
                Loop While dbGtkFamiliaRepetitivaKultura.MoveNext
            Else
                gtkObjeto = fGeneral(ELL.Acciones.Consultar, gtkObjeto)
                lstObjeto.Add(gtkObjeto)
            End If
            Return lstObjeto
        End Function

        ''' <summary>
        ''' Consultamos las Familias para los Tipos de Incidencia Repetitiva.
        ''' </summary>
        ''' <returns>Devuleve una lista de objetos gtkFamiliaRepetitiva.</returns>
        Public Function ConsultarListado() As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkFamiliaRepetitiva)
            Dim gtkObjeto As New ELL.gtkFamiliaRepetitiva
            Dim gtkListObj As New List(Of ELL.gtkFamiliaRepetitiva)
            gtkListObj = ConsultarListado(gtkObjeto)
            Return gtkListObj
        End Function

        ''' <summary>
        ''' Consultamos las Familias para la "Clasificacion de la Familia".
        ''' </summary>
        ''' <param name="arrIDsClasificacion">Vector de identificadores de la "Clasificacion de la Familia".</param>
        ''' <returns>Devuleve una lista de objetos gtkFamiliaRepetitiva.</returns>
        ''' <remarks></remarks>
        Public Function ConsultarListado(ByVal arrIDsClasificacion As ArrayList) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkFamiliaRepetitiva)
            Dim lstObjeto As New List(Of GertakariakLib.ELL.gtkFamiliaRepetitiva)
            For Each IdClasFam As ELL.gtkFamiliaRepetitiva In arrIDsClasificacion
                Dim dbGtkFamiliaRepetitiva As New DAL.FAMILIAREPETITIVA
                dbGtkFamiliaRepetitiva.Where.IDCLASIFICACIONFAMILIA.Value = IdClasFam.IdClasificacionRepetitiva
                dbGtkFamiliaRepetitiva.Query.Load()
                If dbGtkFamiliaRepetitiva.RowCount > 0 Then
                    Do
                        Dim gtkObjeto As New ELL.gtkFamiliaRepetitiva
                        gtkObjeto.Id = dbGtkFamiliaRepetitiva.ID
                        lstObjeto.Add(fGeneral(ELL.Acciones.Consultar, gtkObjeto))
                    Loop While dbGtkFamiliaRepetitiva.MoveNext
                End If
            Next
            Return lstObjeto
        End Function

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
                            gtkMantenimiento.Objeto = Me.Borrar(CType(gtkMantenimiento.Objeto, ELL.gtkFamiliaRepetitiva))
                        Case ELL.Acciones.Consultar
                            gtkMantenimiento.Objeto = Me.Consultar(gtkMantenimiento.Objeto)
                        Case ELL.Acciones.Insertar
                            gtkMantenimiento.Objeto = Me.Insertar(gtkMantenimiento.Objeto)
                            IdReg = gtkMantenimiento.Objeto.id
                        Case ELL.Acciones.Modificar
                            gtkMantenimiento.Objeto = Me.Modificar(gtkMantenimiento.Objeto)
                    End Select
                Next
            Catch ex As ApplicationException
                Throw
            Catch ex As BatzException
                Throw
            Catch ex As Exception
                'Throw New BatzException("error", ex)
                Throw New BatzException(ex.Message.ToString, ex)
            End Try
            Return gtkObjetoList
        End Function

        ''' <summary>
        ''' Función General para el tratamiento de la cultura.
        ''' </summary>
        ''' <param name="Accion">Accion a realizar con el objeto (ELL.Acciones).</param>
        ''' <param name="gtkObjeto">ELL.gtkFamiliaRepetitiva</param>
        ''' <returns>ELL.gtkFamiliaRepetitiva</returns>
        ''' <remarks></remarks>
        Private Function fgKultura(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkFamiliaRepetitiva) As ELL.gtkFamiliaRepetitiva
            Dim dbTablaKultura As New GertakariakLib.DAL.FAMILIAREPETITIVAKULTURA  'Tabla de FAMILIAREPETITIVAKULTURA (DataBase - db)
            Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                '-----------------------------------------------------------------
                '- Comprobamos que el termino que se pasa exista en la BB.DD -----
                '-----------------------------------------------------------------
                If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
                    Select Case Accion
                        Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                            dbTablaKultura.Where.IDFAMILIAREPETITIVA.Value = gtkObjeto.Id
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
                '-----------------------------------------------------------------
                'Inicio de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.BeginTransaction()
                End Select
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbTablaKultura.AddNew()
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        'Cargamos el registro con el que vamos a trabajar.                        
                        dbTablaKultura.FlushData()  'Descargamos el objeto
                        dbTablaKultura.Where.IDFAMILIAREPETITIVA.Value = gtkObjeto.Id
                        dbTablaKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
                        dbTablaKultura.Query.Load()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If Not gtkObjeto.Descripcion Is Nothing Then dbTablaKultura.DESCRIPCION = gtkObjeto.Descripcion.ToString.Trim
                        If gtkObjeto.Id <> Integer.MinValue Then dbTablaKultura.IDFAMILIAREPETITIVA = gtkObjeto.Id.ToString.Trim
                        If Not gtkObjeto.Kultura Is Nothing Then dbTablaKultura.IDCULTURA = gtkObjeto.Kultura.ToString.Trim
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbTablaKultura.IsColumnNull(DAL.FAMILIAREPETITIVAKULTURA.ColumnNames.DESCRIPCION) Then gtkObjeto.Descripcion = dbTablaKultura.DESCRIPCION
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
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.CommitTransaction()
                End Select
                '-----------------------------------------------------------------

            Catch ex As ApplicationException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.RollbackTransaction()
                End Select
                gtkObjeto = Nothing
                '-----------------------------------------------------------------
                Throw
                'Catch ex As BatzException
                '    '-----------------------------------------------------------------
                '    'Fin de la transaccion
                '    '-----------------------------------------------------------------
                '    Select Case Accion
                '        Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                '            Transakzio.RollbackTransaction()
                '    End Select
                '    gtkObjeto = Nothing
                '    '-----------------------------------------------------------------
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
                'Throw New BatzException("error", ex)
                'Throw New BatzException(ex.Message.ToString, ex)
                Log.Error(ex)
                Throw
            End Try
            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Contador de Registros de Familia que estan relacionados con las Incidencias.
        ''' </summary>
        ''' <param name="Id">Identificador de la Familia a contar.</param>
        ''' <returns>Numero de registros que existen</returns>
        ''' <remarks></remarks>
        Public Function Contador(ByVal Id As Integer) As Integer
            Dim bdFamiliaRepetitiva As New DAL.FAMILIAREPETITIVA
            Return bdFamiliaRepetitiva.Contador(Id)
        End Function

    End Class

	Public Class gtkClasificacionRepetitivaComponent
		Private Log As log4net.ILog = log4net.LogManager.GetLogger("root.gtkClasificacionRepetitivaComponent")
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

		Private Function getClasificacionKulturasByIdClasificacion(ByVal Id As Integer) As DAL.CLASIFICACIONREPKULTURA
			Dim bdKulturaDAL As New DAL.CLASIFICACIONREPKULTURA
			bdKulturaDAL.Where.ID.Operator = WhereParameter.Operand.Equal
			bdKulturaDAL.Where.ID.Value = Id
			bdKulturaDAL.Query.Load()

			Return bdKulturaDAL
		End Function

		''' <summary>
		''' Funcion para la insercion de Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Clasificaciones Repetitivas (gtkClasificacionRepetitiva).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function Insertar(ByVal gtkObjeto As ELL.gtkClasificacionRepetitiva) As ELL.gtkClasificacionRepetitiva
			gtkObjeto = fGeneral(ELL.Acciones.Insertar, gtkObjeto)
			Return gtkObjeto
		End Function
		''' <summary>
		''' Funcion para la modificacion de las Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Clasificaciones Repetitivas (gtkClasificacionRepetitiva).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function Modificar(ByVal gtkObjeto As ELL.gtkClasificacionRepetitiva) As ELL.gtkClasificacionRepetitiva
			Return fGeneral(ELL.Acciones.Modificar, gtkObjeto)
		End Function
		''' <summary>
		''' Funcion para la eliminacion de las Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Clasificaciones Repetitivas (gtkClasificacionRepetitiva).</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function Borrar(ByVal gtkObjeto As ELL.gtkClasificacionRepetitiva) As ELL.gtkClasificacionRepetitiva
			Return fGeneral(ELL.Acciones.Borrar, gtkObjeto)
		End Function

		''' <summary>
		''' Funcion para la eliminacion de las Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="Id">Identificador del registro.</param>
		''' <remarks></remarks>
		Public Sub Borrar(ByVal Id As Integer)
			Dim Lista As New List(Of ELL.gtkClasificacionRepetitiva)
			Try
				If Contador(Id) <> 0 Then
					'Throw New System.Exception("errBorrarRelacionado")
					Throw New ApplicationException("errBorrarRelacionado")
				End If
				'-- Borramos el registro cultura a cultura --
				Lista = Consultar(Id)
				For Each Item As ELL.gtkClasificacionRepetitiva In Lista
					Borrar(Item)
				Next
				'--------------------------------------------
			Catch ex As ApplicationException
				Throw
				'Catch ex As BatzException
				'	throw 
			Catch ex As Exception
				'Throw New BatzException(ex.Message, ex)
				Throw
			End Try
		End Sub

		''' <summary>
		''' Funcion general para las Clasificaciones Repetitivas (Clasificaciones de Tipos de Incidencias Repetitivas).
		''' </summary>
		''' <param name="Accion">Accion a realizar con el Objeto (ELL.Acciones)</param>
		''' <param name="gtkObjeto">Devuelve un Objeto gtkClasificacionRepetitiva</param>
		''' <returns>ELL.gtkClasificacionRepetitiva</returns>
		''' <remarks></remarks>
		Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkClasificacionRepetitiva) As ELL.gtkClasificacionRepetitiva
			Dim dbTabla As New GertakariakLib.DAL.CLASIFICACIONREPETITIVA  'Tabla de CLASIFICACIONREPETITIVA (DataBase - db)
			Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
			Try
				'-----------------------------------------------------------------
				'- Comprobamos que el termino que se pasa exista en la BB.DD -----
				'-----------------------------------------------------------------
				If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
					Select Case Accion
						Case ELL.Acciones.Insertar
							dbTabla.Where.ID.Value = gtkObjeto.Id
							dbTabla.Query.Load()
							If Not dbTabla.EOF Then
								'Si existe lo Modificamos
								Accion = ELL.Acciones.Modificar
							Else
								'Si no existe lo Insertamos
								Accion = ELL.Acciones.Insertar
							End If
							dbTabla.FlushData()
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
				'Tabla de CLASIFICACIONREPETITIVA
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar
						'Para hacer insert
						dbTabla.AddNew()
					Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						'Cargamos el registro con el que vamos a trabajar.
						dbTabla.Where.ID.Value = gtkObjeto.Id
						dbTabla.Query.Load()
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'---------------------------------------------------------------------------
						'Campos a Insertar o Modificar
						'---------------------------------------------------------------------------
						If gtkObjeto.Orden = Integer.MinValue Then dbTabla.s_ORDEN = String.Empty Else dbTabla.ORDEN = gtkObjeto.Orden
						If gtkObjeto.Id < 0 Then dbTabla.s_ID = String.Empty Else dbTabla.ID = gtkObjeto.Id
						dbTabla.OBSOLETO = gtkObjeto.Obsoleto
						'---------------------------------------------------------------------------
					Case ELL.Acciones.Consultar
						'---------------------------------------------------------------------------
						'Campos a devolver en la consulta
						'---------------------------------------------------------------------------
						If Not dbTabla.IsColumnNull(DAL.CLASIFICACIONREPETITIVA.ColumnNames.ORDEN) Then gtkObjeto.Orden = dbTabla.ORDEN
						If Not dbTabla.IsColumnNull(DAL.CLASIFICACIONREPETITIVA.ColumnNames.ID) Then gtkObjeto.Id = dbTabla.ID
						If Not dbTabla.IsColumnNull(DAL.CAUSASNC.ColumnNames.OBSOLETO) Then gtkObjeto.Obsoleto = dbTabla.OBSOLETO
						'---------------------------------------------------------------------------
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'Realizamos la accion correspondiente (Insertar, Modificar)
						dbTabla.Save()
						gtkObjeto.Id = dbTabla.ID
				End Select
				'-----------------------------------------------------------------

				'-----------------------------------------------------------------
				'Tabla de CLASIFICACIONREPETITIVAKULTURA
				'-----------------------------------------------------------------
				Dim dbTablaKul As New DAL.CLASIFICACIONREPKULTURA
				Select Case Accion
					Case ELL.Acciones.Consultar
						If Not gtkObjeto.Kultura Is Nothing Then
							gtkObjeto = fgKultura(Accion, gtkObjeto)
						Else
							Dim CampoTraducido As New GertakariakLib.BLL.Utils.Literal
							dbTablaKul.Where.ID.Value = gtkObjeto.Id
							dbTablaKul.Query.Load()
							CampoTraducido = GertakariakLib.BLL.Utils.TraducirCampo(dbTablaKul, DAL.CLASIFICACIONREPKULTURA.ColumnNames.DESCRIPCION.ToString, DAL.CLASIFICACIONREPKULTURA.ColumnNames.IDCULTURA.ToString, Me.Kultura)
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
						dbTablaKul.Where.ID.Value = gtkObjeto.Id
						dbTablaKul.Query.Load()
						If dbTablaKul.EOF Then
							dbTabla.DeleteAll()
							dbTabla.Save()
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
			Catch ex As ApplicationException
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				Throw
				'Catch ex As BatzException
				'    '-----------------------------------------------------------------
				'    'Fin de la transaccion
				'    '-----------------------------------------------------------------
				'    Select Case Accion
				'        Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
				'            Transakzio.RollbackTransaction()
				'    End Select
				'    gtkObjeto = Nothing
				'    '-----------------------------------------------------------------
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
				'Throw New BatzException("error", ex)
				'Throw New BatzException(ex.Message.ToString, ex)
				Throw
			End Try
			Return gtkObjeto
		End Function

		''' <summary>
		''' Consultamos las Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Clasificaciones Repetitivas (gtkClasificacionRepetitiva).</param>
		''' <returns>Devuleve un objeto gtkClasificacionRepetitiva.</returns>
		Public Function Consultar(ByVal gtkObjeto As ELL.gtkClasificacionRepetitiva) As ELL.gtkClasificacionRepetitiva
			Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
		End Function
		''' <summary>
		''' Consultamos las Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="Id">Identificador del registro.</param>
		''' <returns>Devuleve una lista de objetos gtkClasificacionRepetitiva.</returns>
		''' <remarks></remarks>
		Public Function Consultar(ByVal Id As Integer) As List(Of ELL.gtkClasificacionRepetitiva)
			Dim gtkObjeto As New ELL.gtkClasificacionRepetitiva
			gtkObjeto.Id = Id
			Return ConsultarListado(gtkObjeto)
		End Function

		''' <summary>
		''' Consultamos las Clasificaciones Repetitivas.
		''' </summary>
		''' <returns>Devuleve una lista de objetos gtkClasificacionRepetitiva.</returns>
		''' <remarks></remarks>
		Public Function Consultar() As List(Of ELL.gtkClasificacionRepetitiva)
			Dim gtkCLComponent As New BLL.gtkClasificacionRepetitivaComponent
			Return gtkCLComponent.fgClasificacionRepetitivasContador()
		End Function

		''' <summary>
		''' Consultamos las Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="gtkObjeto">Objeto de las Clasificaciones Repetitivas (gtkClasificacionRepetitiva).</param>
		''' <returns>Devuleve un objeto de gtkClasificacionRepetitiva.</returns>
		Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkClasificacionRepetitiva) As List(Of ELL.gtkClasificacionRepetitiva)
			Dim lstObjeto As New List(Of GertakariakLib.ELL.gtkClasificacionRepetitiva)
			Dim dbTablaKultura As New DAL.CLASIFICACIONREPKULTURA
			If gtkObjeto.Id <> Integer.MinValue Then dbTablaKultura.Where.ID.Value = gtkObjeto.Id
			If Not gtkObjeto.Kultura Is Nothing Then dbTablaKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
			dbTablaKultura.Query.Load()
			If dbTablaKultura.RowCount > 0 Then
				Do
					Dim nObjeto As New ELL.gtkClasificacionRepetitiva
					nObjeto.Id = dbTablaKultura.ID
					nObjeto.Kultura = dbTablaKultura.IDCULTURA
					gtkObjeto = fGeneral(ELL.Acciones.Consultar, nObjeto)
					lstObjeto.Add(gtkObjeto)
				Loop While dbTablaKultura.MoveNext
			Else
				gtkObjeto = fGeneral(ELL.Acciones.Consultar, gtkObjeto)
				lstObjeto.Add(gtkObjeto)
			End If
			Return lstObjeto
		End Function

		''' <summary>
		''' Consultamos las Clasificaciones Repetitivas.
		''' </summary>
		''' <returns>Devuleve una lista de objetos gtkClasificacionRepetitiva.</returns>
		Public Function ConsultarListado() As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkClasificacionRepetitiva)
			Dim gtkObjeto As New ELL.gtkClasificacionRepetitiva
			Dim gtkListObj As New List(Of ELL.gtkClasificacionRepetitiva)
			gtkListObj = ConsultarListado(gtkObjeto)
			Return gtkListObj
		End Function

		''' <summary>
		''' Consultamos las Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="arrIDsClasificacion">Vector de identificadores de "Clasificaciones Repetitivas".</param>
		''' <returns>Devuleve una lista de objetos gtkClasificacionRepetitiva.</returns>
		''' <remarks></remarks>
		Public Function ConsultarListado(ByVal arrIDsClasificacion As ArrayList) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkClasificacionRepetitiva)
			Dim lstObjeto As New List(Of GertakariakLib.ELL.gtkClasificacionRepetitiva)
			For Each IdClasFam As ELL.gtkClasificacionRepetitiva In arrIDsClasificacion
				Dim dbTabla As New DAL.CLASIFICACIONREPETITIVA
				dbTabla.Where.ID.Value = IdClasFam
				dbTabla.Query.Load()
				If dbTabla.RowCount > 0 Then
					Do
						Dim gtkObjeto As New ELL.gtkClasificacionRepetitiva
						gtkObjeto.Id = dbTabla.ID
						lstObjeto.Add(fGeneral(ELL.Acciones.Consultar, gtkObjeto))
					Loop While dbTabla.MoveNext
				End If
			Next
			Return lstObjeto
		End Function

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
							gtkMantenimiento.Objeto = Me.Borrar(CType(gtkMantenimiento.Objeto, ELL.gtkClasificacionRepetitiva))
						Case ELL.Acciones.Consultar
							gtkMantenimiento.Objeto = Me.Consultar(gtkMantenimiento.Objeto)
						Case ELL.Acciones.Insertar
							gtkMantenimiento.Objeto = Me.Insertar(gtkMantenimiento.Objeto)
							IdReg = gtkMantenimiento.Objeto.id
						Case ELL.Acciones.Modificar
							gtkMantenimiento.Objeto = Me.Modificar(gtkMantenimiento.Objeto)
					End Select
				Next
			Catch ex As ApplicationException
				Throw
			Catch ex As BatzException
				Throw
			Catch ex As Exception
				'Throw New BatzException("error", ex)
				Throw New BatzException(ex.Message.ToString, ex)
			End Try
			Return gtkObjetoList
		End Function

		''' <summary>
		''' Función General para el tratamiento de la cultura.
		''' </summary>
		''' <param name="Accion">Accion a realizar con el objeto (ELL.Acciones).</param>
		''' <param name="gtkObjeto">ELL.gtkFamiliaRepetitiva</param>
		''' <returns>ELL.gtkFamiliaRepetitiva</returns>
		''' <remarks></remarks>
		Private Function fgKultura(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkClasificacionRepetitiva) As ELL.gtkClasificacionRepetitiva
			Dim dbTablaKultura As New GertakariakLib.DAL.CLASIFICACIONREPKULTURA  'Tabla de CLASIFICACIONREPETITIVAKULTURA (DataBase - db)
			Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
			Try
				'-----------------------------------------------------------------
				'- Comprobamos que el termino que se pasa exista en la BB.DD -----
				'-----------------------------------------------------------------
				If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
					Select Case Accion
						Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
							dbTablaKultura.Where.ID.Value = gtkObjeto.Id
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
				'-----------------------------------------------------------------
				'Inicio de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.BeginTransaction()
				End Select
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar
						'Para hacer insert
						dbTablaKultura.AddNew()
					Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						'Cargamos el registro con el que vamos a trabajar.                        
						dbTablaKultura.FlushData()	'Descargamos el objeto
						dbTablaKultura.Where.ID.Value = gtkObjeto.Id
						dbTablaKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
						dbTablaKultura.Query.Load()
				End Select
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
						'---------------------------------------------------------------------------
						'Campos a Insertar o Modificar
						'---------------------------------------------------------------------------
						If Not gtkObjeto.Descripcion Is Nothing Then dbTablaKultura.DESCRIPCION = gtkObjeto.Descripcion.ToString.Trim
						If gtkObjeto.Id <> Integer.MinValue Then dbTablaKultura.ID = gtkObjeto.Id.ToString.Trim
						If Not gtkObjeto.Kultura Is Nothing Then dbTablaKultura.IDCULTURA = gtkObjeto.Kultura.ToString.Trim
						'---------------------------------------------------------------------------
					Case ELL.Acciones.Consultar
						'---------------------------------------------------------------------------
						'Campos a devolver en la consulta
						'---------------------------------------------------------------------------
						If Not dbTablaKultura.IsColumnNull(DAL.FAMILIAREPETITIVAKULTURA.ColumnNames.DESCRIPCION) Then gtkObjeto.Descripcion = dbTablaKultura.DESCRIPCION
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
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.CommitTransaction()
				End Select
				'-----------------------------------------------------------------

			Catch ex As ApplicationException
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				gtkObjeto = Nothing
				'-----------------------------------------------------------------
				Throw
				'Catch ex As BatzException
				'    '-----------------------------------------------------------------
				'    'Fin de la transaccion
				'    '-----------------------------------------------------------------
				'    Select Case Accion
				'        Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
				'            Transakzio.RollbackTransaction()
				'    End Select
				'    gtkObjeto = Nothing
				'    '-----------------------------------------------------------------
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
				'Throw New BatzException("error", ex)
				'Throw New BatzException(ex.Message.ToString, ex)
				Throw
			End Try
			Return gtkObjeto
		End Function

		''' <summary>
		''' Contador de Registros de las Clasificaciones Repetitivas.
		''' </summary>
		''' <param name="Id">Identificador de la Clasificacion a contar.</param>
		''' <returns>Numero de registros que existen</returns>
		''' <remarks></remarks>
		Public Function Contador(ByVal Id As Integer) As Integer
			Dim bdFamiliaRepetitiva As New DAL.CLASIFICACIONREPETITIVA
			Return bdFamiliaRepetitiva.Contador(Id)
		End Function

		''' <summary>
		''' Funcion que devuelve todas las familias y un contador para cada una de ellas, informando el numero de incidencias que estan asociadas a dicha familia
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function fgClasificacionRepetitivasContador() As List(Of ELL.gtkClasificacionRepetitiva)
			Dim listaFam As New List(Of ELL.gtkClasificacionRepetitiva)
			Dim bdTabla As New DAL.CLASIFICACIONREPETITIVA
			Try
				bdTabla.LoadAll()
				While Not bdTabla.EOF
					Dim oClasificacion As New ELL.gtkClasificacionRepetitiva
					Dim ClasRepCompt As New BLL.gtkClasificacionRepetitivaComponent
					oClasificacion.Id = bdTabla.ID
					oClasificacion = ClasRepCompt.Consultar(oClasificacion)
					listaFam.Add(oClasificacion)
					bdTabla.MoveNext()
				End While
			Catch ex As Exception
				Log.Error(ex)
				Throw
			End Try
			Return listaFam
		End Function
	End Class

    Public Class gtkCausasNCComponent
        Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkCausasNCComponent")

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
        ''' Funcion para la insercion de las Causas de la No Conformidad.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Causas de la No Conformidad (gtkCausasNC).</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function Insertar(ByVal gtkObjeto As ELL.gtkCausasNC) As GertakariakLib.ELL.gtkCausasNC
            gtkObjeto = fGeneral(ELL.Acciones.Insertar, gtkObjeto)
            Return gtkObjeto
        End Function
        ''' <summary>
        ''' Funcion para la modificacion de las Causas de la No Conformidad.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Causas de la No Conformidad (gtkCausasNC).</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function Modificar(ByVal gtkObjeto As ELL.gtkCausasNC) As GertakariakLib.ELL.gtkCausasNC
            Return fGeneral(ELL.Acciones.Modificar, gtkObjeto)
        End Function
        ''' <summary>
        ''' Funcion para la eliminacion de las Causas de la No Conformidad.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Causas de la No Conformidad (gtkCausasNC).</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function Borrar(ByVal gtkObjeto As ELL.gtkCausasNC) As GertakariakLib.ELL.gtkCausasNC
            Return fGeneral(ELL.Acciones.Borrar, gtkObjeto)
        End Function
        ''' <summary>
        ''' Funcion para la eliminacion de las Causas de la No Conformidad.
        ''' </summary>
        ''' <param name="Id">Identificador del registro.</param>
        ''' <remarks></remarks>
        Public Sub Borrar(ByVal Id As Integer)
            Dim Lista As New List(Of ELL.gtkCausasNC)
            Try
                '-- Comprobamos que este regitro no este relacionado --
                Dim dbTabla As New DAL.GERTAKARIAK_CAUSASNC
                dbTabla.Where.IDCAUSANC.Value = Id
                dbTabla.Query.Load()
                If Not dbTabla.EOF Then
                    'Throw New System.Exception("errBorrarRelacionado")
                    Throw New ApplicationException("errBorrarRelacionado")
                End If
                '------------------------------------------------------
                '-- Borramos el registro cultura a cultura ------------
                Lista = Consultar(Id)
                For Each Item As ELL.gtkCausasNC In Lista
                    Borrar(Item)
                Next
                '------------------------------------------------------
            Catch ex As ApplicationException
                Throw
            Catch ex As BatzException
                Throw
            Catch ex As Exception
                Throw New BatzException(ex.Message, ex)
            End Try
        End Sub

        ''' <summary>
        ''' Funcion general las Causas de la No Conformidad.
        ''' </summary>
        ''' <param name="Accion">Accion a realizar con el Objeto (ELL.Acciones)</param>
        ''' <param name="gtkObjeto">Devuelve un Objeto gtkCausasNC</param>
        ''' <returns>ELL.gtkCausasNC</returns>
        ''' <remarks></remarks>
        Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkCausasNC) As GertakariakLib.ELL.gtkCausasNC
            Dim dbTabla As New GertakariakLib.DAL.CAUSASNC  'Tabla de CAUSASNC (DataBase - db)
            Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                '-----------------------------------------------------------------
                '- Comprobamos que el termino que se pasa exista en la BB.DD -----
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        'If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
                        Select Case Accion
                            Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                                dbTabla.Where.ID.Value = gtkObjeto.Id
                                dbTabla.Query.Load()
                                If Not dbTabla.EOF Then
                                    'Si existe lo Modificamos
                                    Accion = ELL.Acciones.Modificar
                                Else
                                    'Si no existe lo Insertamos
                                    Accion = ELL.Acciones.Insertar
                                End If
                                dbTabla.FlushData()
                        End Select
                        '                  Else
                        ''Si no existe lo Insertamos
                        'Accion = ELL.Acciones.Insertar
                        '                  End If
                End Select
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
                'Tabla de CAUSASNC
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbTabla.AddNew()
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        'Cargamos el registro con el que vamos a trabajar.
                        dbTabla.Where.ID.Value = gtkObjeto.Id
                        dbTabla.Query.Load()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If gtkObjeto.Orden = Integer.MinValue Then dbTabla.s_ORDEN = String.Empty Else dbTabla.ORDEN = gtkObjeto.Orden
                        dbTabla.OBSOLETO = gtkObjeto.Obsoleto
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbTabla.IsColumnNull(DAL.CAUSASNC.ColumnNames.ORDEN) Then gtkObjeto.Orden = dbTabla.ORDEN
                        If Not dbTabla.IsColumnNull(DAL.CAUSASNC.ColumnNames.ID) Then gtkObjeto.Id = dbTabla.ID
                        If Not dbTabla.IsColumnNull(DAL.CAUSASNC.ColumnNames.OBSOLETO) Then gtkObjeto.Obsoleto = dbTabla.OBSOLETO
                        '---------------------------------------------------------------------------
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbTabla.Save()
                        gtkObjeto.Id = dbTabla.ID
                End Select
                '-----------------------------------------------------------------

                '-----------------------------------------------------------------
                'Tabla de CAUSASNCKUKTURA
                '-----------------------------------------------------------------
                Dim dbTablaKul As New DAL.CAUSASNCKUKTURA
                Select Case Accion
                    Case ELL.Acciones.Consultar
                        If Not gtkObjeto.Kultura Is Nothing Then
                            gtkObjeto = fgKultura(Accion, gtkObjeto)
                        Else
                            Dim CampoTraducido As New GertakariakLib.BLL.Utils.Literal
                            dbTablaKul.Where.ID.Value = gtkObjeto.Id
                            dbTablaKul.Query.Load()
                            CampoTraducido = GertakariakLib.BLL.Utils.TraducirCampo(dbTablaKul, DAL.CAUSASNCKUKTURA.ColumnNames.DESCRIPCION.ToString, DAL.CAUSASNCKUKTURA.ColumnNames.IDCULTURA.ToString, Me.Kultura)
                            gtkObjeto.Descripcion = CampoTraducido.Descripcion
                            gtkObjeto.Kultura = CampoTraducido.IdCultura
                        End If
                    Case ELL.Acciones.Modificar, ELL.Acciones.Insertar, ELL.Acciones.Borrar
                        fgKultura(Accion, gtkObjeto)
                End Select
                Select Case Accion
                    Case ELL.Acciones.Borrar
                        '-- Comprobamos que queden culturas para el registro.   --------------
                        '-- Si no eleminamos es registro correspondiente.       --------------
                        dbTablaKul.Where.ID.Value = gtkObjeto.Id
                        dbTablaKul.Query.Load()
                        If dbTablaKul.EOF Then
                            dbTabla.DeleteAll()
                            dbTabla.Save()
                            gtkObjeto.Id = Integer.MinValue
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
            Catch ex As ApplicationException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.RollbackTransaction()
                End Select
                gtkObjeto = Nothing
                '-----------------------------------------------------------------
                Throw
                'Catch ex As BatzException
                '    '-----------------------------------------------------------------
                '    'Fin de la transaccion
                '    '-----------------------------------------------------------------
                '    Select Case Accion
                '        Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                '            Transakzio.RollbackTransaction()
                '    End Select
                '    gtkObjeto = Nothing
                '    '-----------------------------------------------------------------
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
                'Throw New BatzException("error", ex)
                'Throw New BatzException(ex.Message.ToString, ex)
                Throw
            End Try
            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Consultamos las Causas de la No Conformidad.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Causas de la No Conformidad (gtkCausasNC).</param>
        ''' <returns>Devuleve un objeto gtkCausasNC.</returns>
        Public Function Consultar(ByVal gtkObjeto As ELL.gtkCausasNC) As GertakariakLib.ELL.gtkCausasNC
            Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
        End Function

        ''' <summary>
        ''' Consultamos las Causas de la No Conformidad.
        ''' </summary>
        ''' <param name="Id">Identificador del registro.</param>
        ''' <returns>Devuleve una lista de objetos gtkCausasNC.</returns>
        ''' <remarks></remarks>
        Public Function Consultar(ByVal Id As Integer) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkCausasNC)
            Dim gtkObjeto As New ELL.gtkCausasNC
            gtkObjeto.Id = Id
            Return ConsultarListado(gtkObjeto)
        End Function
        ''' <summary>
        ''' Consultamos las Causas de la No Conformidad.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de las Causas de la No Conformidad (gtkCausasNC).</param>
        ''' <returns>Devuleve un objeto de gtkCausasNC.</returns>
        Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkCausasNC) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkCausasNC)
            Dim lstObjeto As New List(Of GertakariakLib.ELL.gtkCausasNC)
            Dim dbTablaKultura As New DAL.CAUSASNCKUKTURA
            If gtkObjeto.Id <> Integer.MinValue Then dbTablaKultura.Where.ID.Value = gtkObjeto.Id
            If Not gtkObjeto.Kultura Is Nothing Then dbTablaKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
            dbTablaKultura.Query.Load()
            If dbTablaKultura.RowCount > 0 Then
                Do
                    Dim nObjeto As New ELL.gtkCausasNC
                    nObjeto.Id = dbTablaKultura.ID
                    nObjeto.Kultura = dbTablaKultura.IDCULTURA
                    gtkObjeto = fGeneral(ELL.Acciones.Consultar, nObjeto)
                    lstObjeto.Add(gtkObjeto)
                Loop While dbTablaKultura.MoveNext
            Else
                gtkObjeto = fGeneral(ELL.Acciones.Consultar, gtkObjeto)
                lstObjeto.Add(gtkObjeto)
            End If
            Return lstObjeto
        End Function

        ''' <summary>
        ''' Consultamos las Causas de la No Conformidad.
        ''' </summary>
        ''' <returns>Devuleve una lista de objetos gtkCausasNC.</returns>
        Public Function ConsultarListado() As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkCausasNC)
            Dim gtkObjeto As New ELL.gtkCausasNC
            Dim gtkListObj As New List(Of ELL.gtkCausasNC)
            gtkListObj = ConsultarListado(gtkObjeto)
            Return gtkListObj
        End Function

        '''' <summary>
        '''' Consultamos las Causas de la No Conformidad.
        '''' </summary>
        '''' <param name="arrIDsClasificacion">Vector de identificadores de "Clasificaciones Repetitivas".</param>
        '''' <returns>Devuleve una lista de objetos gtkClasificacionRepetitiva.</returns>
        '''' <remarks></remarks>
        'Public Function ConsultarListado(ByVal arrIDsClasificacion As ArrayList) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkCausasNC)
        '    Dim lstObjeto As New List(Of GertakariakLib.ELL.gtkClasificacionRepetitiva)
        '    For Each IdClasFam As ELL.gtkClasificacionRepetitiva In arrIDsClasificacion
        '        Dim dbTabla As New DAL.CLASIFICACIONREPETITIVA
        '        dbTabla.Where.ID.Value = IdClasFam
        '        dbTabla.Query.Load()
        '        If dbTabla.RowCount > 0 Then
        '            Do
        '                Dim gtkObjeto As New ELL.gtkClasificacionRepetitiva
        '                gtkObjeto.Id = dbTabla.ID
        '                lstObjeto.Add(fGeneral(ELL.Acciones.Consultar, gtkObjeto))
        '            Loop While dbTabla.MoveNext
        '        End If
        '    Next
        '    Return lstObjeto
        'End Function

        ''' <summary>
        ''' Pasamos una estructura de tipo gtkMantenimiento con la accion que se quiere realizar
        ''' </summary>
        ''' <param name="gtkObjetoList">List(Of ELL.gtkMantenimiento)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Mantenimiento(ByVal gtkObjetoList As List(Of ELL.gtkMantenimiento)) As System.Collections.Generic.List(Of ELL.gtkMantenimiento)
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
                            gtkMantenimiento.Objeto = Me.Borrar(CType(gtkMantenimiento.Objeto, ELL.gtkCausasNC))
                        Case ELL.Acciones.Consultar
                            gtkMantenimiento.Objeto = Me.Consultar(gtkMantenimiento.Objeto)
                        Case ELL.Acciones.Insertar
                            gtkMantenimiento.Objeto = Me.Insertar(gtkMantenimiento.Objeto)
                            IdReg = gtkMantenimiento.Objeto.id
                        Case ELL.Acciones.Modificar
                            gtkMantenimiento.Objeto = Me.Modificar(gtkMantenimiento.Objeto)
                    End Select
                Next
            Catch ex As ApplicationException
                Throw
            Catch ex As BatzException
                Throw
            Catch ex As Exception
                'Throw New BatzException("error", ex)
                Throw New BatzException(ex.Message.ToString, ex)
            End Try
            Return gtkObjetoList
        End Function

        ''' <summary>
        ''' Función General para el tratamiento de la cultura.
        ''' </summary>
        ''' <param name="Accion">Accion a realizar con el objeto (ELL.Acciones).</param>
        ''' <param name="gtkObjeto">ELL.gtkCausasNC</param>
        ''' <returns>ELL.gtkCausasNC</returns>
        ''' <remarks></remarks>
        Private Function fgKultura(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkCausasNC) As GertakariakLib.ELL.gtkCausasNC
            Dim dbTablaKultura As New GertakariakLib.DAL.CAUSASNCKUKTURA  'Tabla de CAUSASNCKUKTURA (DataBase - db)
            Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                '-----------------------------------------------------------------
                '- Comprobamos que el termino que se pasa exista en la BB.DD -----
                '-----------------------------------------------------------------
                If gtkObjeto.Id <> Integer.MinValue And Not (gtkObjeto.Id.ToString Is Nothing Or gtkObjeto.Id.ToString Is DBNull.Value) Then
                    Select Case Accion
                        Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                            dbTablaKultura.Where.ID.Value = gtkObjeto.Id
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
                '-----------------------------------------------------------------
                'Inicio de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.BeginTransaction()
                End Select
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbTablaKultura.AddNew()
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        'Cargamos el registro con el que vamos a trabajar.                        
                        dbTablaKultura.FlushData()  'Descargamos el objeto
                        dbTablaKultura.Where.ID.Value = gtkObjeto.Id
                        dbTablaKultura.Where.IDCULTURA.Value = gtkObjeto.Kultura
                        dbTablaKultura.Query.Load()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If Not gtkObjeto.Descripcion Is Nothing Then dbTablaKultura.DESCRIPCION = gtkObjeto.Descripcion.ToString.Trim
                        If gtkObjeto.Id <> Integer.MinValue Then dbTablaKultura.ID = gtkObjeto.Id.ToString.Trim
                        If Not gtkObjeto.Kultura Is Nothing Then dbTablaKultura.IDCULTURA = gtkObjeto.Kultura.ToString.Trim
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbTablaKultura.IsColumnNull(DAL.CAUSASNCKUKTURA.ColumnNames.DESCRIPCION) Then gtkObjeto.Descripcion = dbTablaKultura.DESCRIPCION
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
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.CommitTransaction()
                End Select
                '-----------------------------------------------------------------

                'Catch ex As BatzException
                '	'-----------------------------------------------------------------
                '	'Fin de la transaccion
                '	'-----------------------------------------------------------------
                '	Select Case Accion
                '		Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                '			Transakzio.RollbackTransaction()
                '	End Select
                '	gtkObjeto = Nothing
                '	'-----------------------------------------------------------------
                '	throw 

            Catch ex As ApplicationException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.RollbackTransaction()
                End Select
                gtkObjeto = Nothing
                '-----------------------------------------------------------------
                Throw
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
                Log.Error(ex)
                Throw
            End Try
            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Relacionamos Procesos con Causas.
        ''' </summary>
        ''' <param name="gtkProceso">Objeto de Proceso.</param>
        ''' <param name="gtkCausa">Objeto de Causa.</param>
        Public Function Insertar(ByVal gtkProceso As ELL.gtkProceso, ByVal gtkCausa As ELL.gtkCausasNC) As Boolean
            Dim dbTabla As New DAL.CAUSASNCPROC
            Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Insertar = False
            Try
                Transakzio.BeginTransaction()

                dbTabla.Where.IDCAUSA.Value = gtkCausa.Id
                dbTabla.Query.Load()
                dbTabla.DeleteAll()
                dbTabla.Save()

                dbTabla.AddNew()
                dbTabla.IDCAUSA = gtkCausa.Id
                dbTabla.CODPROC = gtkProceso.ID
                dbTabla.Save()

                Transakzio.CommitTransaction()
                Insertar = True
            Catch ex As BatzException
                Throw
                Transakzio.RollbackTransaction()
            Catch ex As Exception
                'Throw New BatzException("error", ex)
                Throw New BatzException(ex.Message.ToString, ex)
                Transakzio.RollbackTransaction()
            End Try

        End Function
    End Class

    Public Class gtkProveedorComponent
        ''' <summary>
        ''' Consultamos los Proveedores.
        ''' </summary>
        ''' <param name="CodPro">Código de Proveedor.</param>
        ''' <param name="NomProv">Nombre del Proveedor.</param>
        ''' <param name="Vigente">Indicamos si mostramos solo los vigentes (HABPOT=H->Habitual, HABPOT=P->Potencial).</param>
        ''' <returns>Lista de gtkProveedor.</returns>
        Private Function fGeneral(ByVal CodPro As String, ByVal NomProv As String, ByVal Vigente As Boolean) As List(Of GertakariakLib.ELL.gtkProveedor)
            Dim ListaProv As New List(Of ELL.gtkProveedor)
            Dim dbTabla As New DAL.W_GCPROVEE 'Vista de Proveedores

            Dim Filtrado As New ELL.Filtrado
            Dim SQL As String = String.Empty
            Dim SqlWhere As String = String.Empty

            Try
                Dim drW_GCPROVEE As IDataReader = Nothing
                SQL = "SELECT * FROM W_GCPROVEE"
                If Vigente = True Then
                    If SqlWhere Is String.Empty Then SqlWhere = " WHERE" Else SqlWhere = SqlWhere & " AND"
                    SqlWhere = SqlWhere & " " & DAL.W_GCPROVEE.ColumnNames.HABPOT & " <> 'O'" 'HABPOT (O -> Obsoleto, H -> Habitual, P -> Potencial)
                End If
                If Not CodPro Is Nothing Or Not NomProv Is Nothing Then
                    If SqlWhere Is String.Empty Then SqlWhere = " WHERE" Else SqlWhere = SqlWhere & " AND"
                    SqlWhere = SqlWhere & " ("
                    If Not CodPro Is Nothing Then
                        SqlWhere = SqlWhere & DAL.W_GCPROVEE.ColumnNames.CODPRO & " LIKE '%" & CodPro.Trim & "%'"
                    End If
                    If Not NomProv Is Nothing Then
                        If Not CodPro Is Nothing Then SqlWhere = SqlWhere & " OR "
                        Filtrado.Texto = NomProv
                        SqlWhere = SqlWhere & (" REGEXP_LIKE(W_GCPROVEE." & DAL.W_GCPROVEE.ColumnNames.NOMPROV & ",'" & Filtrado.TextoLike & "','i')")
                    End If
                    SqlWhere = SqlWhere & ")"
                End If
                SQL = SQL & SqlWhere
                drW_GCPROVEE = dbTabla.FiltrarPor(SQL)

                If Not (drW_GCPROVEE Is Nothing) Then
                    Do While drW_GCPROVEE.Read
                        Dim gtkObjeto As New ELL.gtkProveedor
                        gtkObjeto.CodPro = drW_GCPROVEE(DAL.W_GCPROVEE.ColumnNames.CODPRO)
                        gtkObjeto.NomProv = drW_GCPROVEE(DAL.W_GCPROVEE.ColumnNames.NOMPROV)
                        gtkObjeto.CIF = drW_GCPROVEE(DAL.W_GCPROVEE.ColumnNames.CIF).ToString.Trim
                        gtkObjeto.Portes = drW_GCPROVEE(DAL.W_GCPROVEE.ColumnNames.PORTES).ToString.Trim
                        ListaProv.Add(gtkObjeto)
                    Loop
                End If

                If Not (drW_GCPROVEE Is Nothing) Then drW_GCPROVEE.Close()

            Catch ex As Exception
                Throw
                'Dim batz As New BatzException("errGTKobtenerNombreProveedor", ex)   '"Error al obtener el nombre del proveedor."
                ListaProv = Nothing
            End Try

            Return ListaProv

        End Function

        ''' <summary>
        ''' Consultamos los Proveedores.
        ''' </summary>
        ''' <param name="CodPro">Código de Proveedor.</param>
        ''' <param name="NomProv">Nombre del Proveedor.</param>
        ''' <param name="Vigente">Indicamos si mostramos solo los vigentes.</param>
        ''' <returns>Lista de gtkProveedor.</returns>
        Public Function Consultar(Optional ByVal CodPro As String = Nothing, Optional ByVal NomProv As String = Nothing, Optional ByVal Vigente As Boolean = False) As List(Of ELL.gtkProveedor)
            Consultar = fGeneral(CodPro, NomProv, Vigente)
        End Function

        ''' <summary>
        ''' Consulta de Proveedores en INCIDENCIAS.W_GCPROVEE
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto con el que realizamos la busqueda</param>
        ''' <returns>List (Of gtkProveedor)</returns>
        ''' <remarks>Los datos de Proveedores se obtienen de "INCIDENCIAS.W_GCPROVEE".</remarks>
        Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkProveedor) As List(Of ELL.gtkProveedor)
            ConsultarListado = fGeneral(gtkObjeto)
        End Function

        Private Function fGeneral(ByVal gtkObjeto As ELL.gtkProveedor) As List(Of ELL.gtkProveedor)
            Dim dbTablaBBDD As New DAL.W_GCPROVEE
            Dim ListaObj As New List(Of ELL.gtkProveedor)

            Try
                If gtkObjeto.CodPro <> Integer.MinValue AndAlso gtkObjeto.CodPro IsNot Nothing AndAlso gtkObjeto.CodPro IsNot DBNull.Value Then dbTablaBBDD.Where.CODPRO.Value = gtkObjeto.CodPro
                dbTablaBBDD.Query.Load()

                If Not dbTablaBBDD.EOF Then
                    If Not dbTablaBBDD.IsColumnNull(DAL.W_GCPROVEE.ColumnNames.CODPRO) Then gtkObjeto.CodPro = dbTablaBBDD.CODPRO
                    If Not dbTablaBBDD.IsColumnNull(DAL.W_GCPROVEE.ColumnNames.NOMPROV) Then gtkObjeto.NomProv = dbTablaBBDD.NOMPROV
                    If Not dbTablaBBDD.IsColumnNull(DAL.W_GCPROVEE.ColumnNames.TIPIVA) Then gtkObjeto.TipIva = dbTablaBBDD.TIPIVA
                    If Not dbTablaBBDD.IsColumnNull(DAL.W_GCPROVEE.ColumnNames.FORPAG) Then gtkObjeto.FormaPago = dbTablaBBDD.FORPAG
                    If Not dbTablaBBDD.IsColumnNull(DAL.W_GCPROVEE.ColumnNames.CODMON) Then gtkObjeto.CodMoneda = dbTablaBBDD.CODMON
                    If Not dbTablaBBDD.IsColumnNull(DAL.W_GCPROVEE.ColumnNames.CIF) Then gtkObjeto.CIF = dbTablaBBDD.CIF.Trim
                    If Not dbTablaBBDD.IsColumnNull(DAL.W_GCPROVEE.ColumnNames.PORTES) Then gtkObjeto.Portes = dbTablaBBDD.PORTES.Trim
                    ListaObj.Add(gtkObjeto)
                Else
                    ListaObj = Nothing
                End If
            Catch ex As BatzException
                Throw
                ListaObj = Nothing
            Catch ex As Exception
                'Throw New BatzException("error", ex)
                Throw New BatzException(ex.Message.ToString, ex)
                ListaObj = Nothing
            End Try
            fGeneral = ListaObj
        End Function

        ''' <summary>
        ''' Obtenemos las capacidades del SAB.
        ''' </summary>
        ''' <param name="gtkObjeto">ELL.gtkProveedor. Si no se pasa este objeto devuelve todas las capacidades.</param>
        ''' <returns>List(Of ELL.gtkCapacidad)</returns>
        ''' <remarks></remarks>
        Public Function VerCapacidadesSAB(Optional ByVal gtkObjeto As ELL.gtkProveedor = Nothing) As List(Of ELL.gtkCapacidad)
            Dim Capacidades As New List(Of ELL.gtkCapacidad)
            Dim dbCapacidades As New DAL.CAPACIDADES

            If gtkObjeto IsNot Nothing Then
                Dim dbEMPRESAS As New DAL.EMPRESAS
                Dim dbEMPRESAS_CAPACIDADES As New DAL.EMPRESAS_CAPACIDADES

                'Obtenemos el Proveedor de SAB.
                If gtkObjeto.Id <= 0 Then
                    If gtkObjeto.CodPro <> Integer.MinValue AndAlso gtkObjeto.CodPro IsNot Nothing AndAlso gtkObjeto.CodPro IsNot DBNull.Value Then
                        dbEMPRESAS.Where.IDTROQUELERIA.Value = gtkObjeto.CodPro
                        dbEMPRESAS.Query.Load()
                    End If
                Else
                    dbEMPRESAS.Where.ID.Value = gtkObjeto.Id
                    dbEMPRESAS.Query.Load()
                End If
                If Not dbEMPRESAS.EOF Then
                    gtkObjeto.Id = dbEMPRESAS.ID
                    dbEMPRESAS_CAPACIDADES.Where.ID_EMPRESA.Value = gtkObjeto.Id
                    dbEMPRESAS_CAPACIDADES.Query.Load()
                    While Not dbEMPRESAS_CAPACIDADES.EOF
                        'Obtenemos las capacidades para esta empresa.
                        dbCapacidades.FlushData()
                        dbCapacidades.Where.CAPID.Value = dbEMPRESAS_CAPACIDADES.ID_CAPACIDADES
                        dbCapacidades.Query.Load()
                        If Not dbCapacidades.EOF Then
                            Dim gtkCapadicad As New ELL.gtkCapacidad
                            gtkCapadicad.CAPID = dbCapacidades.CAPID
                            gtkCapadicad.NOMBRE = dbCapacidades.NOMBRE
                            gtkCapadicad.OBSOLETO = dbCapacidades.OBSOLETO
                            Capacidades.Add(gtkCapadicad)
                        End If
                        dbEMPRESAS_CAPACIDADES.MoveNext()
                    End While
                End If
            Else
                'Cargamos todas las capacidades.
                dbCapacidades.LoadAll()
                While Not dbCapacidades.EOF
                    Dim gtkCapadicad As New ELL.gtkCapacidad
                    gtkCapadicad.CAPID = dbCapacidades.CAPID
                    gtkCapadicad.NOMBRE = dbCapacidades.NOMBRE
                    gtkCapadicad.OBSOLETO = dbCapacidades.OBSOLETO
                    Capacidades.Add(gtkCapadicad)
                    dbCapacidades.MoveNext()
                End While
            End If

            Return Capacidades
        End Function
        ''' <summary>
        ''' Devuelve la capacidad.
        ''' </summary>
        ''' <param name="CAPID"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerCapacidadesSAB(ByVal CAPID As String) As ELL.gtkCapacidad
            Dim tCapacidades As New DAL.CAPACIDADES
            Return tCapacidades.Consultar(CAPID)
        End Function
    End Class

    Public Class gtkUsuarioComponent
        Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkUsuarioComponent")

        ''' <summary>
        ''' Indicamos quien envia el email.
        ''' </summary>
        Private _mailAddress As String = "Gertakariak@Batz.es"
        ''' <summary>
        ''' Servidor de Exchange.
        ''' </summary>
        Private _smtpClient As String = "POSTA.BATZ.COM"    '172.17.200.3 'no se usa
        ''' <summary>
        ''' Indicamos quien envia el email.
        ''' </summary>
        Public Property MailAddress() As String
            Get
                Return _mailAddress
            End Get
            Set(ByVal value As String)
                _mailAddress = value
            End Set
        End Property
        ''' <summary>
        ''' Servidor de Exchange.
        ''' </summary>
        Public Property SmtpClient() As String
            Get
                Return _smtpClient
            End Get
            Set(ByVal value As String)
                _smtpClient = value
            End Set
        End Property

        ''' <summary>
        ''' Consulta de los Usuarios.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de usuarios de la aplicación. (gtkUsuario).</param>
        ''' <returns>Devuleve un objeto gtkUsuarioRol.</returns>
        Public Function Consultar(ByVal gtkObjeto As ELL.gtkUsuario) As GertakariakLib.ELL.gtkUsuario
            Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
        End Function
        ''' <summary>
        ''' Funcion general de Usuarios.
        ''' </summary>
        ''' <param name="Accion">Accion a realizar con el Objeto (Insertar, Modificar, Consultar, Borrar) [ELL.Acciones]</param>
        ''' <param name="gtkObjeto">Devuelve un Objeto gtkUsuario</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkUsuario) As GertakariakLib.ELL.gtkUsuario
            Dim dbTablaBBDD As New GertakariakLib.DAL.USUARIOS  'Tabla USUARIOS. (DataBase - db)
            Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                '-----------------------------------------------------------------
                'Inicio de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.BeginTransaction()
                End Select
                '-----------------------------------------------------------------
                '-----------------------------------------------------------------
                'Tabla de USUARIOS
                '-----------------------------------------------------------------
                '-- Cargamos el registro con el que vamos a trabajar -------------
                Select Case Accion
                    Case ELL.Acciones.Consultar, ELL.Acciones.Borrar
                        'Cargamos el registro con el que vamos a trabajar.
                        If gtkObjeto.IdUsrSab <> Integer.MinValue And Not (gtkObjeto.IdUsrSab.ToString Is Nothing Or gtkObjeto.IdUsrSab.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDUSRSAB.Value = gtkObjeto.IdUsrSab
                End Select
                Select Case Accion
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        If gtkObjeto.IdGrupo <> Integer.MinValue And Not (gtkObjeto.IdGrupo.ToString Is Nothing Or gtkObjeto.IdGrupo.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDGRUPO.Value = gtkObjeto.IdGrupo
                        If ((gtkObjeto.IdClasificacion <> Integer.MinValue And gtkObjeto.IdClasificacion <> 0) And Not (gtkObjeto.IdClasificacion.ToString Is Nothing Or gtkObjeto.IdClasificacion.ToString Is DBNull.Value)) Then dbTablaBBDD.Where.IDCLASFAMILIA.Value = gtkObjeto.IdClasificacion
                        If ((gtkObjeto.IdTipoIncidencia <> Integer.MinValue And gtkObjeto.IdTipoIncidencia <> 0) And Not (gtkObjeto.IdTipoIncidencia.ToString Is Nothing Or gtkObjeto.IdTipoIncidencia.ToString Is DBNull.Value)) Then dbTablaBBDD.Where.IDTIPOINCIDENCIA.Value = gtkObjeto.IdTipoIncidencia
                        If ((gtkObjeto.IdTipoIncidencia <> Integer.MinValue And gtkObjeto.IdTipoIncidencia <> 0) And Not (gtkObjeto.IdTipoIncidencia.ToString Is Nothing Or gtkObjeto.IdTipoIncidencia.ToString Is DBNull.Value)) Then dbTablaBBDD.Where.IDTIPOINCIDENCIA.Value = gtkObjeto.IdTipoIncidencia
                End Select
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        dbTablaBBDD.Query.AddOrderBy(DAL.USUARIOS.ColumnNames.IDUSRSAB.ToString(), WhereParameter.Dir.ASC)
                        dbTablaBBDD.Query.Load()
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbTablaBBDD.AddNew()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If gtkObjeto.IdUsrSab = Integer.MinValue Then dbTablaBBDD.s_IDUSRSAB = String.Empty Else dbTablaBBDD.s_IDUSRSAB = gtkObjeto.IdUsrSab
                        '---------------------------------------------------------------------------
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar
                        '---------------------------------------------------------------------------
                        If gtkObjeto.IdClasificacion = Integer.MinValue Then dbTablaBBDD.s_IDCLASFAMILIA = String.Empty Else dbTablaBBDD.s_IDCLASFAMILIA = gtkObjeto.IdClasificacion
                        If gtkObjeto.IdGrupo = Integer.MinValue Then dbTablaBBDD.s_IDGRUPO = String.Empty Else dbTablaBBDD.s_IDGRUPO = gtkObjeto.IdGrupo
                        If gtkObjeto.IdTipoIncidencia = Integer.MinValue Then dbTablaBBDD.s_IDTIPOINCIDENCIA = String.Empty Else dbTablaBBDD.s_IDTIPOINCIDENCIA = gtkObjeto.IdTipoIncidencia
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbTablaBBDD.EOF Then
                            '- Si la consulta devuelve mas de un registro cogemos el 1º --
                            If Not dbTablaBBDD.IsColumnNull(DAL.USUARIOS.ColumnNames.IDCLASFAMILIA) Then gtkObjeto.IdClasificacion = dbTablaBBDD.s_IDCLASFAMILIA
                            If Not dbTablaBBDD.IsColumnNull(DAL.USUARIOS.ColumnNames.IDGRUPO) Then gtkObjeto.IdGrupo = dbTablaBBDD.s_IDGRUPO
                            If Not dbTablaBBDD.IsColumnNull(DAL.USUARIOS.ColumnNames.IDUSRSAB) Then gtkObjeto.IdUsrSab = dbTablaBBDD.s_IDUSRSAB
                            If Not dbTablaBBDD.IsColumnNull(DAL.USUARIOS.ColumnNames.IDTIPOINCIDENCIA) Then gtkObjeto.IdTipoIncidencia = dbTablaBBDD.s_IDTIPOINCIDENCIA
                            '-------------------------------------------------------------
                        Else
                            gtkObjeto = Nothing
                        End If
                        '---------------------------------------------------------------------------
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbTablaBBDD.Save()
                    Case ELL.Acciones.Borrar
                        dbTablaBBDD.DeleteAll()
                        dbTablaBBDD.Save()
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

            Catch ex As ApplicationException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.RollbackTransaction()
                End Select
                gtkObjeto = Nothing
                '-----------------------------------------------------------------
                Throw
                'Catch ex As BatzException
                '	'-----------------------------------------------------------------
                '	'Fin de la transaccion
                '	'-----------------------------------------------------------------
                '	Select Case Accion
                '		Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                '			Transakzio.RollbackTransaction()
                '	End Select
                '	gtkObjeto = Nothing
                '	'-----------------------------------------------------------------
                '	throw 
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
                'Throw New BatzException(ex.Message.ToString, ex)
                Log.Error(ex)
                Throw
            End Try
            Return gtkObjeto
        End Function
        ''' <summary>
        ''' Funcion para la Creacion de Usuarios.
        ''' </summary>
        ''' <param name="gtkUsuario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Insertar(ByVal gtkUsuario As ELL.gtkUsuario) As GertakariakLib.ELL.gtkUsuario
            Return fGeneral(ELL.Acciones.Insertar, gtkUsuario)
        End Function
        ''' <summary>
        ''' Funcion para la Modificacion de Usuarios.
        ''' </summary>
        ''' <param name="gtkUsuario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Modificar(ByVal gtkUsuario As ELL.gtkUsuario) As GertakariakLib.ELL.gtkUsuario
            Return fGeneral(ELL.Acciones.Modificar, gtkUsuario)
        End Function
        ''' <summary>
        ''' Funcion para la Eliminacion de Usuarios.
        ''' </summary>
        ''' <param name="gtkUsuario"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Borrar(ByVal gtkUsuario As ELL.gtkUsuario) As GertakariakLib.ELL.gtkUsuario
            Return fGeneral(ELL.Acciones.Borrar, gtkUsuario)
        End Function
        ''' <summary>
        ''' Consulta de los Usuarios.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de usuarios de la aplicación. (gtkUsuario).</param>
        ''' <returns>Devuleve un listado de objetos gtkUsuario.</returns>
        ''' <remarks></remarks>
        Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkUsuario) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkUsuario)
            Dim dbTablaBBDD As New GertakariakLib.DAL.USUARIOS  'Tabla USUARIOS. (DataBase - db)            
            Dim gtkListObj As New List(Of ELL.gtkUsuario)
            Try
                'Cargamos el registro con el que vamos a trabajar.
                If ((gtkObjeto.IdClasificacion <> Integer.MinValue And gtkObjeto.IdClasificacion <> 0) And Not (gtkObjeto.IdClasificacion.ToString Is Nothing Or gtkObjeto.IdClasificacion.ToString Is DBNull.Value)) Then dbTablaBBDD.Where.IDCLASFAMILIA.Value = gtkObjeto.IdClasificacion
                If gtkObjeto.IdGrupo <> Integer.MinValue And Not (gtkObjeto.IdGrupo.ToString Is Nothing Or gtkObjeto.IdGrupo.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDGRUPO.Value = gtkObjeto.IdGrupo
                If gtkObjeto.IdUsrSab <> Integer.MinValue And Not (gtkObjeto.IdUsrSab.ToString Is Nothing Or gtkObjeto.IdUsrSab.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDUSRSAB.Value = gtkObjeto.IdUsrSab
                If gtkObjeto.IdTipoIncidencia <> Integer.MinValue And Not (gtkObjeto.IdTipoIncidencia.ToString Is Nothing Or gtkObjeto.IdTipoIncidencia.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDTIPOINCIDENCIA.Value = gtkObjeto.IdTipoIncidencia

                dbTablaBBDD.Query.AddOrderBy(DAL.USUARIOS.ColumnNames.IDUSRSAB.ToString(), WhereParameter.Dir.ASC)
                dbTablaBBDD.Query.Load()
                If Not dbTablaBBDD.EOF Then
                    Do
                        Dim gtkObjetoB As New ELL.gtkUsuario   'Nuevo Objeto que se va metiendo en la lista

                        If Not dbTablaBBDD.IsColumnNull(DAL.USUARIOS.ColumnNames.IDCLASFAMILIA) Then gtkObjetoB.IdClasificacion = dbTablaBBDD.IDCLASFAMILIA
                        If Not dbTablaBBDD.IsColumnNull(DAL.USUARIOS.ColumnNames.IDGRUPO) Then gtkObjetoB.IdGrupo = dbTablaBBDD.IDGRUPO
                        If Not dbTablaBBDD.IsColumnNull(DAL.USUARIOS.ColumnNames.IDUSRSAB) Then gtkObjetoB.IdUsrSab = dbTablaBBDD.IDUSRSAB
                        If Not dbTablaBBDD.IsColumnNull(DAL.USUARIOS.ColumnNames.IDTIPOINCIDENCIA) Then gtkObjetoB.IdTipoIncidencia = dbTablaBBDD.IDTIPOINCIDENCIA
                        gtkObjetoB = fGeneral(ELL.Acciones.Consultar, gtkObjetoB)   'Rellenamos los campos que faltan
                        gtkListObj.Add(gtkObjetoB)
                    Loop While dbTablaBBDD.MoveNext
                Else
                    gtkListObj = Nothing
                End If

            Catch ex As Exception
				Log.Error(ex)
				Throw
            End Try
            Return gtkListObj
        End Function
        ''' <summary>
        ''' Consulta de los Usuarios.
        ''' </summary>
        ''' <returns>Devuleve un listado de objetos gtkUsuario.</returns>
        ''' <remarks></remarks>
        Public Function ConsultarListado() As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkUsuario)
            Dim gtkObjeto As New ELL.gtkUsuario
            Dim gtkListObj As New List(Of ELL.gtkUsuario)
            gtkListObj = ConsultarListado(gtkObjeto)
            Return gtkListObj
        End Function

        ''' <summary>
        ''' Pasamos una estructura de tipo gtkMantenimiento con la accion que se quiere realizar
        ''' </summary>
        ''' <param name="gtkObjetoList">List(Of ELL.gtkMantenimiento)</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Mantenimiento(ByVal gtkObjetoList As List(Of ELL.gtkMantenimiento)) As List(Of ELL.gtkMantenimiento)
            'Dim Log As log4net.ILog = log4net.LogManager.GetLogger("root.GertakariakLib")
            Try
                For Each gtkMantenimiento As ELL.gtkMantenimiento In gtkObjetoList
                    Select Case gtkMantenimiento.Accion
                        Case ELL.Acciones.Borrar
                            'gtkMantenimiento.Objeto = Me.Borrar(CType(gtkMantenimiento.Objeto, ELL.gtkUsuario))
                            gtkMantenimiento.Objeto = Me.Borrar(gtkMantenimiento.Objeto)
                        Case ELL.Acciones.Consultar
                            gtkMantenimiento.Objeto = Me.Consultar(gtkMantenimiento.Objeto)
                        Case ELL.Acciones.Insertar
                            gtkMantenimiento.Objeto = Me.Insertar(gtkMantenimiento.Objeto)
                            'IdReg = gtkMantenimiento.Objeto.id
                        Case ELL.Acciones.Modificar
                            gtkMantenimiento.Objeto = Me.Modificar(gtkMantenimiento.Objeto)
                    End Select
                Next

            Catch ex As ApplicationException
                Throw
                'Catch ex As BatzException
                '	Log.Error("error", ex)
                '	throw 
            Catch ex As Exception
                'Log.Error("error", ex)
                'Throw New BatzException("error", ex)
                'Throw New BatzException(ex.Message.ToString, ex)
                Log.Error(ex)
                Throw
            End Try

            Return gtkObjetoList
        End Function
    End Class

    Public Class gtkGruposComponent
        ''' <summary>
        ''' Consulta de Grupos de Administracion.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de Grupos de Administracion. (gtkGrupo).</param>
        ''' <returns>Devuleve un objeto gtkUsuarioRol.</returns>
        Public Function Consultar(ByVal gtkObjeto As ELL.gtkGrupo) As GertakariakLib.ELL.gtkGrupo
            Return fGeneral(ELL.Acciones.Consultar, gtkObjeto)
        End Function

        ''' <summary>
        ''' Funcion general de Usuarios.
        ''' </summary>
        ''' <param name="Accion">Accion a realizar con el Objeto (Insertar, Modificar, Consultar, Borrar) [ELL.Acciones]</param>
        ''' <param name="gtkObjeto">Devuelve un Objeto gtkUsuariosRol</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function fGeneral(ByVal Accion As ELL.Acciones, ByVal gtkObjeto As ELL.gtkGrupo) As GertakariakLib.ELL.gtkGrupo
            Dim dbTablaBBDD As New GertakariakLib.DAL.GRUPOSADMINISTRACION  'Tabla GRUPOSADMINISTRACION. (DataBase - db)
            Dim Transakzio As TransactionMgr = TransactionMgr.ThreadTransactionMgr

            Try
                '-----------------------------------------------------------------
                'Inicio de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.BeginTransaction()
                End Select
                '-----------------------------------------------------------------
                '-----------------------------------------------------------------
                'Tabla de GRUPOSADMINISTRACION
                '-----------------------------------------------------------------
                '-- Cargamos el registro con el que vamos a trabajar -------------
                Select Case Accion
                    Case ELL.Acciones.Consultar, ELL.Acciones.Borrar
                        'Cargamos el registro con el que vamos a trabajar.
                        If gtkObjeto.IdGrupo <> Integer.MinValue And Not (gtkObjeto.IdGrupo.ToString Is Nothing Or gtkObjeto.IdGrupo.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDGRUPO.Value = gtkObjeto.IdGrupo
                End Select
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Consultar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        dbTablaBBDD.Query.AddOrderBy(DAL.GRUPOSADMINISTRACION.ColumnNames.IDGRUPO.ToString(), WhereParameter.Dir.ASC)
                        dbTablaBBDD.Query.Load()
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        'dbTablaBBDD.AddNew()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        'If gtkObjeto.IdRol = Integer.MinValue Then dbTablaBBDD.s_IDROL = String.Empty Else dbTablaBBDD.s_IDROL = gtkObjeto.IdRol
                        '---------------------------------------------------------------------------
                End Select
                Select Case Accion
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbTablaBBDD.EOF Then
                            '- Si la consulta devuelve mas de un registro cogemos el 1º --
                            If Not dbTablaBBDD.IsColumnNull(DAL.GRUPOSADMINISTRACION.ColumnNames.LKZDESCRIPCION) Then gtkObjeto.IdLkzDescrip = dbTablaBBDD.LKZDESCRIPCION
                            If Not dbTablaBBDD.IsColumnNull(DAL.GRUPOSADMINISTRACION.ColumnNames.LKZGRUPO) Then gtkObjeto.IdLkzGrupo = dbTablaBBDD.LKZGRUPO
                            '-------------------------------------------------------------
                        Else
                            gtkObjeto = Nothing
                        End If
                        '---------------------------------------------------------------------------
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
            Catch ex As ApplicationException
                '-----------------------------------------------------------------
                'Fin de la transaccion
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
                        Transakzio.RollbackTransaction()
                End Select
                gtkObjeto = Nothing
                '-----------------------------------------------------------------
                Throw
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
				Throw
            End Try
            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Consulta de los Usuarios.
        ''' </summary>
        ''' <param name="gtkObjeto">Objeto de usuarios de la aplicación. (gtkUsuarioRol).</param>
        ''' <returns>Devuleve un listado de objetos gtkUsuarioRol.</returns>
        ''' <remarks></remarks>
        Public Function ConsultarListado(ByVal gtkObjeto As ELL.gtkGrupo) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkGrupo)
            Dim dbTablaBBDD As New GertakariakLib.DAL.GRUPOSADMINISTRACION  'Tabla (DataBase - db)            
            Dim gtkListObj As New List(Of ELL.gtkGrupo)

            'Cargamos el registro con el que vamos a trabajar.
            If gtkObjeto.IdGrupo <> Integer.MinValue And Not (gtkObjeto.IdGrupo.ToString Is Nothing Or gtkObjeto.IdGrupo.ToString Is DBNull.Value) Then dbTablaBBDD.Where.IDGRUPO.Value = gtkObjeto.IdGrupo
            dbTablaBBDD.Query.AddOrderBy(DAL.GRUPOSADMINISTRACION.ColumnNames.IDGRUPO.ToString(), WhereParameter.Dir.ASC)
            dbTablaBBDD.Query.Load()
            If Not dbTablaBBDD.EOF Then
                Do
                    Dim gtkObjetoB As New ELL.gtkGrupo    'Nuevo Objeto que se va metiendo en la lista
                    If Not dbTablaBBDD.IsColumnNull(DAL.GRUPOSADMINISTRACION.ColumnNames.IDGRUPO) Then gtkObjetoB.IdGrupo = dbTablaBBDD.IDGRUPO
                    gtkObjetoB = fGeneral(ELL.Acciones.Consultar, gtkObjetoB)   'Rellenamos los campos que faltan
                    gtkListObj.Add(gtkObjetoB)
                Loop While dbTablaBBDD.MoveNext
            Else
                gtkListObj = Nothing
            End If
            Return gtkListObj
        End Function
        ''' <summary>
        ''' Consulta de los Usuarios.
        ''' </summary>
        ''' <returns>Devuleve un listado de objetos gtkUsuarioRol.</returns>
        ''' <remarks></remarks>
        Public Function ConsultarListado() As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkGrupo)
            Dim gtkObjeto As New ELL.gtkGrupo
            Dim gtkListObj As New List(Of ELL.gtkGrupo)
            gtkListObj = ConsultarListado(gtkObjeto)
            Return gtkListObj
        End Function

    End Class

    Public Class gtkProcesosComponent
        Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkProcesosComponent")

        ''' <summary>
        ''' Devuelve un objeto para el identificador unico.
        ''' </summary>
        ''' <param name="Id">Identificador del Proceso.</param>
        ''' <param name="IdSeccion">Identificador de la Seccion.</param>
        ''' <returns>ELL.gtkProceso</returns>
        ''' <remarks></remarks>
        Public Function Consultar(ByVal Id As String, ByVal IdSeccion As String) As ELL.gtkProceso
            Dim gtkObjeto As New ELL.gtkProceso
            Dim lstObj As New List(Of ELL.gtkProceso)

            gtkObjeto.ID = Id
            gtkObjeto.IdSeccion = IdSeccion

            lstObj = fGeneral(gtkObjeto)
            If lstObj IsNot Nothing AndAlso lstObj.Count > 0 Then
                gtkObjeto = lstObj.Item(0)
            Else
                gtkObjeto = Nothing
            End If

            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Devuelve un listado de Objetos para los terminos introducidos en él.
        ''' </summary>
        ''' <param name="gtkObjeto">ELL.gtkAreas</param>
        ''' <returns>List(Of ELL.gtkAreas)</returns>
        ''' <remarks></remarks>
        Public Function Consultar(ByVal gtkObjeto As ELL.gtkProceso) As List(Of ELL.gtkProceso)
            Dim lstObj As New List(Of ELL.gtkProceso)
            lstObj = fGeneral(gtkObjeto)
            Return lstObj
        End Function

        ''' <summary>
        ''' Funcion general para obtener objetos.
        ''' </summary>
        Private Function fGeneral(ByVal gtkObjeto As ELL.gtkProceso) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkProceso)
            Dim bdVista As New DAL.W_PROCESOS
            Dim lstObjetos As New List(Of ELL.gtkProceso)

            Try
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'Funciona
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'If gtkObjeto.ID IsNot Nothing AndAlso gtkObjeto.ID IsNot String.Empty Then bdVista.Where.CODPROC.Value = gtkObjeto.ID.Trim
                'If gtkObjeto.IdSeccion IsNot Nothing AndAlso gtkObjeto.IdSeccion IsNot String.Empty Then bdVista.Where.CODSEC.Value = gtkObjeto.IdSeccion.Trim

                'bdVista.Query.AddResultColumn(DAL.W_PROCESOS.ColumnNames.CODSEC)
                'bdVista.Query.AddResultColumn(DAL.W_PROCESOS.ColumnNames.CODPROC)
                'bdVista.Query.AddResultColumn(DAL.W_PROCESOS.ColumnNames.DESCPROC)
                'bdVista.Query.AddOrderBy(DAL.W_PROCESOS.ColumnNames.CODSEC, WhereParameter.Dir.ASC)
                'bdVista.Query.AddOrderBy(DAL.W_PROCESOS.ColumnNames.CODPROC, WhereParameter.Dir.ASC)
                'bdVista.Query.Distinct = True
                'bdVista.Query.Load()

                'If bdVista.EOF Then
                '	lstObjetos = Nothing
                'Else
                '	Do
                '		Dim obj As New ELL.gtkProceso
                '		obj.IdSeccion = bdVista.CODSEC
                '		obj.ID = bdVista.CODPROC
                '		obj.Descripcion = bdVista.DESCPROC
                '		obj.CausasNC = ConsultarCausasNC(obj.ID)
                '		lstObjetos.Add(obj)
                '	Loop While bdVista.MoveNext
                'End If
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------

                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'Modificacion
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                Dim drW_PROCESOS As IDataReader = Nothing
                Dim SqlWhere As String = String.Empty
                Dim Sql As String = "Select Distinct rtrim(CODSEC) as CODSEC, rtrim(CODPROC) as CODPROC, rtrim(DESCPROC) as DESCPROC  From W_PROCESOS"
                If gtkObjeto.ID IsNot Nothing AndAlso gtkObjeto.ID IsNot String.Empty Then SqlWhere = " Where CODPROC = " & gtkObjeto.ID
                If gtkObjeto.IdSeccion IsNot Nothing AndAlso gtkObjeto.IdSeccion IsNot String.Empty Then
                    If SqlWhere Is String.Empty Then SqlWhere = " Where" Else SqlWhere = SqlWhere & " And"
                    SqlWhere = SqlWhere & " CODSEC = " & gtkObjeto.IdSeccion
                End If
                Sql = Sql & SqlWhere
                Sql = Sql & " Order By CODSEC ASC, DESCPROC ASC"

                drW_PROCESOS = bdVista.FiltrarPor(Sql)
                If Not (drW_PROCESOS Is Nothing) Then
                    Do While drW_PROCESOS.Read
                        Dim obj As New ELL.gtkProceso
                        obj.IdSeccion = drW_PROCESOS(DAL.W_PROCESOS.ColumnNames.CODSEC)
                        obj.ID = drW_PROCESOS(DAL.W_PROCESOS.ColumnNames.CODPROC)
                        obj.Descripcion = drW_PROCESOS(DAL.W_PROCESOS.ColumnNames.DESCPROC)
                        obj.CausasNC = ConsultarCausasNC(obj.ID)
                        lstObjetos.Add(obj)
                    Loop
                End If
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------

                'Catch ex As BatzException
                '	lstObjetos = Nothing
                '	throw 
            Catch ex As Exception
                lstObjetos = Nothing
                'Throw New BatzException("error", ex)
                'Throw New BatzException(ex.Message.ToString, ex)
                Log.Error(ex)
                Throw
            End Try
            Return lstObjetos
        End Function

        ''' <summary>
        ''' Consultamos las Causas de la N.C.
        ''' </summary>
        ''' <param name="Id">Codigo de Proceso (CODPROC).</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function ConsultarCausasNC(ByRef Id As String) As List(Of ELL.gtkCausasNC)
            Dim lCausasNC As List(Of ELL.gtkCausasNC) = Nothing
            Try
                Dim dbTabla As New DAL.CAUSASNCPROC
                Dim FuncCausasNC As New BLL.gtkCausasNCComponent

                dbTabla.Query.AddOrderBy(DAL.CAUSASNCPROC.ColumnNames.CODPROC, WhereParameter.Dir.ASC)
                dbTabla.Query.Distinct = True
                dbTabla.Where.CODPROC.Value = Id
                dbTabla.Query.Load()

                If Not dbTabla.EOF Then
                    Do
                        Dim gtkCausaNC As New ELL.gtkCausasNC
                        gtkCausaNC.Id = dbTabla.IDCAUSA
                        gtkCausaNC = FuncCausasNC.Consultar(gtkCausaNC)
                        If (lCausasNC Is Nothing) Then lCausasNC = New List(Of ELL.gtkCausasNC)
                        lCausasNC.Add(gtkCausaNC)
                    Loop While dbTabla.MoveNext
                End If
            Catch ex As Exception
                If Log IsNot Nothing AndAlso Log.Logger IsNot Nothing AndAlso Log.Logger.Repository IsNot Nothing AndAlso Log.Logger.Repository.GetAppenders.Any Then
                    CType(Log.Logger.Repository.GetAppenders _
                    .Where(Function(o) o.GetType.ToString = "log4net.Appender.SmtpAppender") _
                    .Select(Function(o) o).SingleOrDefault, log4net.Appender.SmtpAppender).Subject = ex.Message
                End If
                Log.Error(ex)
                Throw
            End Try
            Return lCausasNC
        End Function
    End Class

    Public Class gtkSeccionesComponent
        Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkSeccionesComponent")

        ''' <summary>
        ''' Devuelve un objeto para el identificador unico.
        ''' </summary>
        ''' <param name="Id">Identificador de la Seccion</param>
        ''' <returns>ELL.gtkSecciones</returns>
        ''' <remarks></remarks>
        Public Function Consultar(ByVal Id As String) As ELL.gtkSecciones
            Dim gtkObjeto As New ELL.gtkSecciones
            Dim lstObj As New List(Of ELL.gtkSecciones)

            gtkObjeto.ID = Id

            lstObj = fGeneral(gtkObjeto)
            If lstObj IsNot Nothing AndAlso lstObj.Count > 0 Then
                gtkObjeto = lstObj.Item(0)
            Else
                gtkObjeto = Nothing
            End If

            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Devuelve un listado de Objetos para los terminos introducidos en él.
        ''' </summary>
        ''' <param name="gtkObjeto">ELL.gtkSecciones</param>
        ''' <returns>List(Of ELL.gtkAreas)</returns>
        ''' <remarks></remarks>
        Public Function Consultar(ByVal gtkObjeto As ELL.gtkSecciones) As List(Of ELL.gtkSecciones)
            Dim lstObj As New List(Of ELL.gtkSecciones)
            lstObj = fGeneral(gtkObjeto)
            Return lstObj
        End Function

        ''' <summary>
        ''' Funcion general para obtener objetos.
        ''' </summary>
        Private Function fGeneral(ByVal gtkObjeto As ELL.gtkSecciones) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkSecciones)
            Dim bdVista As New DAL.W_PROCESOS
            Dim lstObjetos As New List(Of ELL.gtkSecciones)
            Dim funcProcesos As New BLL.gtkProcesosComponent

            Try
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'Funciona
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'If gtkObjeto.ID IsNot Nothing AndAlso gtkObjeto.ID IsNot String.Empty Then bdVista.Where.CODSEC.Value = gtkObjeto.ID.Trim
                'If gtkObjeto.IdArea IsNot Nothing AndAlso gtkObjeto.IdArea IsNot String.Empty Then bdVista.Where.CODARE.Value = gtkObjeto.IdArea.Trim

                'bdVista.Query.AddResultColumn(DAL.W_PROCESOS.ColumnNames.CODARE)
                'bdVista.Query.AddResultColumn(DAL.W_PROCESOS.ColumnNames.CODSEC)
                'bdVista.Query.AddResultColumn(DAL.W_PROCESOS.ColumnNames.DESCSECCIO)
                'bdVista.Query.AddOrderBy(DAL.W_PROCESOS.ColumnNames.CODARE, WhereParameter.Dir.ASC)
                'bdVista.Query.AddOrderBy(DAL.W_PROCESOS.ColumnNames.CODSEC, WhereParameter.Dir.ASC)
                'bdVista.Query.Distinct = True
                'bdVista.Query.Load()

                'If bdVista.EOF Then
                '	lstObjetos = Nothing
                'Else
                '	Do
                '		Dim obj As New ELL.gtkSecciones
                '		obj.IdArea = bdVista.CODARE
                '		obj.ID = bdVista.CODSEC
                '		obj.Descripcion = bdVista.DESCSECCIO

                '		Dim objProceso As New ELL.gtkProceso
                '		objProceso.IdSeccion = obj.ID
                '		obj.Procesos = funcProcesos.Consultar(objProceso)

                '		lstObjetos.Add(obj)
                '	Loop While bdVista.MoveNext
                'End If
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------

                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'Modificacion
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                Dim drW_PROCESOS As IDataReader = Nothing
                Dim SqlWhere As String = String.Empty
                Dim Sql As String = "Select Distinct rtrim(CODARE) as CODARE, rtrim(CODSEC) as CODSEC, rtrim(DESCSECCIO) as DESCSECCIO  From W_PROCESOS"
                If gtkObjeto.ID IsNot Nothing AndAlso gtkObjeto.ID IsNot String.Empty Then SqlWhere = " Where CODSEC = " & gtkObjeto.ID
                If gtkObjeto.IdArea IsNot Nothing AndAlso gtkObjeto.IdArea IsNot String.Empty Then
                    If SqlWhere Is String.Empty Then SqlWhere = " Where" Else SqlWhere = SqlWhere & " And"
                    SqlWhere = SqlWhere & " CODARE = " & gtkObjeto.IdArea
                End If
                Sql = Sql & SqlWhere
                Sql = Sql & " Order By CODARE ASC, DESCSECCIO ASC"

                drW_PROCESOS = bdVista.FiltrarPor(Sql)
                If Not (drW_PROCESOS Is Nothing) Then
                    Do While drW_PROCESOS.Read
                        Dim obj As New ELL.gtkSecciones
                        obj.IdArea = drW_PROCESOS(DAL.W_PROCESOS.ColumnNames.CODARE)
                        obj.ID = drW_PROCESOS(DAL.W_PROCESOS.ColumnNames.CODSEC)
                        obj.Descripcion = drW_PROCESOS(DAL.W_PROCESOS.ColumnNames.DESCSECCIO)

                        Dim objProceso As New ELL.gtkProceso
                        objProceso.IdSeccion = obj.ID
                        obj.Procesos = funcProcesos.Consultar(objProceso)

                        lstObjetos.Add(obj)
                    Loop
                End If
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------

                'Catch ex As BatzException
                '	lstObjetos = Nothing
                '	throw 
            Catch ex As Exception
                lstObjetos = Nothing
                'Throw New BatzException("error", ex)
                'Throw New BatzException(ex.Message.ToString, ex)
                Log.Error(ex)
                Throw
            End Try
            Return lstObjetos
        End Function
    End Class

    Public Class gtkAreasComponent
        Dim Log As log4net.ILog = log4net.LogManager.GetLogger("GertakariakLib.gtkAreasComponent")

        ''' <summary>
        ''' Devuelve un listado de Objetos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Consultar() As List(Of ELL.gtkAreas)
            Dim gtkObjeto As New ELL.gtkAreas
            Dim lstObj As New List(Of ELL.gtkAreas)
            lstObj = fGeneral(gtkObjeto)
            Return lstObj
        End Function

        ''' <summary>
        ''' Devuelve un objeto para el identificador unico.
        ''' </summary>
        ''' <param name="Id">Identificador del Area</param>
        ''' <returns>ELL.gtkAreas</returns>
        ''' <remarks></remarks>
        Public Function Consultar(ByVal Id As String) As ELL.gtkAreas
            Dim gtkObjeto As New ELL.gtkAreas
            Dim lstObj As New List(Of ELL.gtkAreas)

            gtkObjeto.ID = Id

            lstObj = fGeneral(gtkObjeto)
            If lstObj IsNot Nothing AndAlso lstObj.Count > 0 Then
                gtkObjeto = lstObj.Item(0)
            Else
                gtkObjeto = Nothing
            End If

            Return gtkObjeto
        End Function

        ''' <summary>
        ''' Devuelve un listado de Objetos para los terminos introducidos en él.
        ''' </summary>
        ''' <param name="gtkObjeto">ELL.gtkAreas</param>
        ''' <returns>List(Of ELL.gtkAreas)</returns>
        ''' <remarks></remarks>
        Public Function Consultar(ByVal gtkObjeto As ELL.gtkAreas) As List(Of ELL.gtkAreas)
            Dim lstObj As New List(Of ELL.gtkAreas)
            lstObj = fGeneral(gtkObjeto)
            Return lstObj
        End Function

        ''' <summary>
        ''' Funcion general para obtener objetos.
        ''' </summary>
        Private Function fGeneral(ByVal gtkObjeto As ELL.gtkAreas) As System.Collections.Generic.List(Of GertakariakLib.ELL.gtkAreas)
            Dim bdVista As New DAL.W_PROCESOS
            Dim lstObjetos As New List(Of ELL.gtkAreas)
            Dim funSeccion As New BLL.gtkSeccionesComponent

            Try
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'Funciona
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'If gtkObjeto.ID IsNot Nothing AndAlso gtkObjeto.ID IsNot String.Empty Then bdVista.Where.CODARE.Value = gtkObjeto.ID.Trim
                'bdVista.Query.AddResultColumn(DAL.W_PROCESOS.ColumnNames.CODARE)
                'bdVista.Query.AddResultColumn(DAL.W_PROCESOS.ColumnNames.DESCRI)
                'bdVista.Query.AddOrderBy(DAL.W_PROCESOS.ColumnNames.CODARE, WhereParameter.Dir.ASC)
                'bdVista.Query.Distinct = True
                'bdVista.Query.Load()

                'If bdVista.EOF Then
                '	lstObjetos = Nothing
                'Else
                '	Do
                '		Dim obj As New ELL.gtkAreas
                '		obj.ID = bdVista.CODARE
                '		obj.Descripcion = bdVista.DESCRI

                '		Dim objSeccion As New ELL.gtkSecciones
                '		objSeccion.IdArea = obj.ID
                '		obj.Secciones = funSeccion.Consultar(objSeccion)

                '		lstObjetos.Add(obj)
                '	Loop While bdVista.MoveNext
                'End If
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------

                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                'Modificacion
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------
                Dim drW_PROCESOS As IDataReader = Nothing
                Dim Sql As String = "Select Distinct rtrim(CODARE) as CODARE, rtrim(DESCRI) as DESCRI  From W_PROCESOS"
                If gtkObjeto.ID IsNot Nothing AndAlso gtkObjeto.ID IsNot String.Empty Then Sql = Sql & " Where CODARE = " & gtkObjeto.ID.Trim
                Sql = Sql & " Order By DESCRI"

                drW_PROCESOS = bdVista.FiltrarPor(Sql)
                If Not (drW_PROCESOS Is Nothing) Then
                    Do While drW_PROCESOS.Read
                        Dim obj As New ELL.gtkAreas
                        obj.ID = drW_PROCESOS(DAL.W_PROCESOS.ColumnNames.CODARE)    ' bdVista.CODARE
                        obj.Descripcion = drW_PROCESOS(DAL.W_PROCESOS.ColumnNames.DESCRI) 'bdVista.DESCRI

                        Dim objSeccion As New ELL.gtkSecciones
                        objSeccion.IdArea = obj.ID
                        obj.Secciones = funSeccion.Consultar(objSeccion)

                        lstObjetos.Add(obj)
                    Loop
                End If
                '-------------------------------------------------------------------------------------
                '-------------------------------------------------------------------------------------

                'Catch ex As BatzException
                '	lstObjetos = Nothing
                '	throw 
            Catch ex As Exception
                lstObjetos = Nothing
                'Throw New BatzException(ex.Message.ToString, ex)
                Log.Error(ex)
                Throw
            End Try
            Return lstObjetos
        End Function
    End Class

End Namespace
