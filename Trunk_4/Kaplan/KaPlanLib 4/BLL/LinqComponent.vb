Imports System.Data.Common
Imports log4net

Namespace BLL
	Public Class LinqComponent

		Private log As ILog = LogManager.GetLogger("root.KaPlanLib.LinqComponent")
        Private ItzultzaileWeb As New Traduccioneslib.Itzultzaile
        Dim Funciones As New KaPlanLib.Funciones

#Region "Enumeracion"
		''' <summary>
		''' Filtra las referencias a mostrar con la tabla de resumen amfe, sin ella o mostrando todos
		''' </summary>
		''' <remarks></remarks>
		Public Enum whereResumen As Integer
			Todo = 0
			ConResumen = 1
			SinResumen = 2
		End Enum

		''' <summary>
        ''' Plantas existentes. 
        ''' Los id de cada planta, deben corresponderse con los del BRAIN
		''' </summary>
		''' <remarks></remarks>
		Public Enum plantas As Integer
			BATZ_IGORRE = 1
			BATZ_CZECH = 2
			BATZ_KUNSHAN = 3
            BATZ_MEXICANA = 4
            BATZ_CHENGDU = 6
			BATZ_GUANZHOU = 7
            BATZ_FPK_ZAMUDIO = 47 'En este caso representa "SAB.PLANTAS.ID"
            Batz_Solaire_Maroc_SARL_AU = 227 'En este caso representa "SAB.PLANTAS.ID"

            '----------------------------------------
            'FROGA: Pruebas de desarrollo. Eliminar.
            '----------------------------------------
            'BATZ_IGORRE_TEST = 10
            KAPLAN_TEST = 10
            '----------------------------------------
		End Enum

		''' <summary>
		''' Perfiles de los usuarios
		''' </summary>
		''' <remarks></remarks>
		Public Enum Perfiles As Integer
			Administrador = 1
			Consultor = 2
			Write = 3
		End Enum
#End Region

#Region "MAESTRO_ARTICULOS"
		''' <summary>
		''' Devuelve un registro de Maestro articulos.
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarMaestroArticulo(ByVal idRef As String, Optional ByVal filtroResumen As whereResumen = whereResumen.Todo) As KaPlanLib.Registro.MAESTRO_ARTICULOS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL()
				Dim RegArticulos As KaPlanLib.Registro.MAESTRO_ARTICULOS = Nothing
                If (filtroResumen = whereResumen.Todo) Then
                    RegArticulos = (From Articulos In BBDD.MAESTRO_ARTICULOS
                                    Where Articulos.CODIGO = idRef
                                    Select Articulos).FirstOrDefault()
                ElseIf (filtroResumen = whereResumen.ConResumen) Then
                    RegArticulos = (From Articulos In BBDD.MAESTRO_ARTICULOS
                                    Join AmfesPieza In BBDD.AMFES_PIEZA On Articulos.CODIGO Equals AmfesPieza.CODIGO
                                    Join ResumenAmfe In BBDD.RESUMEN_AMFE On AmfesPieza.ID_AMFE_PIEZA Equals ResumenAmfe.ID_AMFE
                                    Order By Articulos.CODIGO
                                    Where Articulos.CODIGO = idRef
                                    Select Articulos).FirstOrDefault
                    If RegArticulos Is Nothing Then Throw New ApplicationException("La referencia no existe o falta el ""Sistesis AMFE"" para la referencia.")
                ElseIf (filtroResumen = whereResumen.SinResumen) Then
                    RegArticulos = (From Articulos In BBDD.MAESTRO_ARTICULOS
                                    Join AmfesPieza In BBDD.AMFES_PIEZA On Articulos.CODIGO Equals AmfesPieza.CODIGO
                                    Group Join ResumenAmfe In BBDD.RESUMEN_AMFE On AmfesPieza.ID_AMFE_PIEZA Equals ResumenAmfe.ID_AMFE Into resumenAmfesPieza = Group
                                    From resAmfe In resumenAmfesPieza.DefaultIfEmpty
                                    Where resAmfe.ID_AMFE Is Nothing And Articulos.CODIGO = idRef
                                    Order By Articulos.CODIGO
                                    Select Articulos).FirstOrDefault
                    'If RegArticulos Is Nothing Then Throw New ApplicationException("La referencia no existe o falta el ""AMFE Articulo"" para la referencia.")
                    If RegArticulos Is Nothing Then Throw New ApplicationException("La referencia no existe o falta el ""AMFE Articulo"" o ya tiene un ""Sistesis AMFE"" para la referencia.")
                End If

                Return RegArticulos

			Catch ex As ApplicationException
				Throw 
			Catch ex As Exception
				Throw 
			End Try
        End Function
#End Region

#Region "M_CLASES"

        ''' <summary>
        ''' Devuelve todos las clases
        ''' </summary>
        ''' <returns></returns>
        Function consultarListadoClases()
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim listClases = From Clases In BBDD.M_CLASE _
                Select Clases

                Return listClases
            Catch ex As Exception
				Throw 
            End Try
        End Function

#End Region

#Region "AVISOS"
        ''' <summary>
        ''' Devuelve todos los usuarios de aviso y el id_sab
        ''' </summary>
        ''' <param name="idDoc">Documento de cuales extrae los avisos</param>
        ''' <returns></returns>
        Function consultarListadoAvisos(ByVal idDoc As Integer)
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL
                Dim listAvisos = From Avisos In BBDD.AVISOS _
                  Where Avisos.DOCUMENTO = idDoc _
                Select Avisos

                Return listAvisos
            Catch ex As Exception
				Throw 
            End Try
        End Function
        ''' <summary>
        ''' Guarda los avisos. Para ello, primero borra todos los avisos del documento especificado y luego inserta los de la lista
        ''' </summary>
        ''' <param name="lUsuarios">Lista de usuarios a vincular</param>
        ''' <param name="idDoc">Identificador del documento</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarAvisos(ByVal lUsuarios As List(Of Integer), ByVal idDoc As Integer) As Boolean
            Dim transaction As DbTransaction = Nothing
            Dim BBDD As New KaPlanLib.DAL.ELL
            Try

                BBDD.Connection.Open()
                transaction = BBDD.Connection.BeginTransaction
                BBDD.Transaction = transaction

                Dim avisoDoc = From Avisos In BBDD.AVISOS _
                   Where Avisos.DOCUMENTO = idDoc _
                   Select Avisos

                BBDD.AVISOS.DeleteAllOnSubmit(avisoDoc)
                BBDD.SubmitChanges()

                For Each iUser As Integer In lUsuarios
                    Dim newAviso As New KaPlanLib.Registro.AVISOS With {.ID_USUARIO = iUser, .DOCUMENTO = idDoc}
                    BBDD.AVISOS.InsertOnSubmit(newAviso)
                    BBDD.SubmitChanges()
                Next
                transaction.Commit()
                Return True
            Catch ex As Exception
                transaction.Rollback()
                Return False
            Finally
                BBDD.Connection.Close()
            End Try
        End Function
#End Region

#Region "MAESTRO_PERSONAL"
        ''' <summary>
        ''' Devuelve un registro de Maestro personal
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
		Function consultarMaestroPersonal(ByVal oUser As Registro.MAESTRO_PERSONAL) As KaPlanLib.Registro.MAESTRO_PERSONAL
			consultarMaestroPersonal = Nothing
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim lRegUsuario As IQueryable(Of Registro.MAESTRO_PERSONAL) = From Usuarios In BBDD.MAESTRO_PERSONAL _
					Where Usuarios.ID_USUARIO = CType(IIf(oUser.ID_USUARIO = 0, Usuarios.ID_USUARIO, oUser.ID_USUARIO), Integer) _
					And Usuarios.CODIGO_PERSONA = CType(IIf(String.IsNullOrWhiteSpace(oUser.CODIGO_PERSONA), Usuarios.CODIGO_PERSONA.ToString, oUser.CODIGO_PERSONA), String) _
					And Usuarios.E_MAIL = CType(IIf(oUser.E_MAIL = "", Usuarios.E_MAIL, oUser.E_MAIL), String) _
					Select Usuarios

				If lRegUsuario.Any Then
					If lRegUsuario.Count = 1 Then
						consultarMaestroPersonal = lRegUsuario.SingleOrDefault
					ElseIf lRegUsuario.Count > 1 Then
						consultarMaestroPersonal = lRegUsuario.FirstOrDefault
						'Eliminamos los usuarios duplicados. En la tabla "MAESTRO_PERSONAL" no puedes existir usuarios con "CODIGO_PERSONA" y "E_MAIL" iguales.
						EliminarMaestroPersonal(lRegUsuario.ToList.LastOrDefault.ID_USUARIO)
					End If
				End If

				Return consultarMaestroPersonal
			Catch ex As Exception
				log.Error(ex)
				Dim s As String
				s = If(Web.HttpContext.Current.Session("Planta") Is Nothing, String.Empty, Web.HttpContext.Current.Session("Planta")(2).ToString)

				Dim msg As String
				msg = "Function consultarMaestroPersonal:" & vbCrLf _
				& "HttpContext.Current.Session('Planta')(2).ToString: """ & If(Web.HttpContext.Current.Session("Planta") Is Nothing, String.Empty, Web.HttpContext.Current.Session("Planta")(2).ToString) & """"
				log.Error(msg)

				Throw
			End Try
		End Function

		'      ''' <summary>
		'      ''' Guarda los datos de la persona
		'      ''' </summary>
		'      ''' <param name="oMaestro">Datos de la persona</param>                        
		'      ''' <returns></returns>
		'      ''' <remarks></remarks>
		'      Public Function GuardarMaestroPersonal(ByVal oMaestro As Registro.MAESTRO_PERSONAL) As Boolean
		'          Dim BBDD As New KaPlanLib.DAL.ELL
		'          Try
		'              If (oMaestro.ID_USUARIO = 0) Then 'Nuevo
		'                  BBDD.MAESTRO_PERSONAL.InsertOnSubmit(oMaestro)
		'              Else  'Modificar
		'                  Dim maestroP As KaPlanLib.Registro.MAESTRO_PERSONAL = (From maestro In BBDD.MAESTRO_PERSONAL _
		'                     Where maestro.ID_USUARIO = oMaestro.ID_USUARIO _
		'                     Select maestro).FirstOrDefault
		'                  If (maestroP Is Nothing) Then Return False

		'                  'No se introduce el lantegi, se deja a null porque no se va a utilizar
		'                  maestroP.NOMBRE = oMaestro.NOMBRE
		'                  maestroP.CODIGO_PERSONA = oMaestro.CODIGO_PERSONA  'Aqui se va a guardar el idSab
		'                  maestroP.ID_PERFIL = oMaestro.ID_PERFIL
		'                  maestroP.E_MAIL = oMaestro.E_MAIL
		'                  maestroP.ALIAS_ENTRADA = oMaestro.ALIAS_ENTRADA   'Aqui se va a guardar el nombre e usuario                   
		'              End If
		'              BBDD.SubmitChanges()

		'              Return True
		'          Catch ex As Exception
		'              Return False
		'          End Try
		'End Function

		''' <summary>
		''' Guarda los datos de la persona
		''' </summary>
		''' <param name="oMaestro">Datos de la persona</param>                        
		Sub GuardarMaestroPersonal(ByVal oMaestro As Registro.MAESTRO_PERSONAL)
			Dim BBDD As New KaPlanLib.DAL.ELL

			If (oMaestro.ID_USUARIO = 0) Then 'Nuevo
				BBDD.MAESTRO_PERSONAL.InsertOnSubmit(oMaestro)
			Else  'Modificar
				Dim maestroP As KaPlanLib.Registro.MAESTRO_PERSONAL = (From maestro In BBDD.MAESTRO_PERSONAL _
				   Where maestro.ID_USUARIO = oMaestro.ID_USUARIO _
				   Select maestro).FirstOrDefault
				If (maestroP Is Nothing) Then
					Throw New ApplicationException("No se ha encontrado el usuario.")
				End If

				'No se introduce el lantegi, se deja a null porque no se va a utilizar
				maestroP.NOMBRE = oMaestro.NOMBRE
				maestroP.CODIGO_PERSONA = oMaestro.CODIGO_PERSONA  'Aqui se va a guardar el idSab
				maestroP.ID_PERFIL = oMaestro.ID_PERFIL
				maestroP.E_MAIL = oMaestro.E_MAIL
				maestroP.ALIAS_ENTRADA = oMaestro.ALIAS_ENTRADA	  'Aqui se va a guardar el nombre e usuario                   
			End If
			BBDD.SubmitChanges()
		End Sub

		'      ''' <summary>
		'      ''' Elimina un usuario
		'      ''' </summary>
		'      ''' <param name="idUsuario">Id del usuario</param>        
		'      ''' <returns></returns>
		'      ''' <remarks></remarks>
		'      Public Function EliminarMaestroPersonal(ByVal idUsuario As Integer) As Boolean
		'          Try
		'              Dim BBDD As New KaPlanLib.DAL.ELL
		'		Dim MaestroPerso = From maestro In BBDD.MAESTRO_PERSONAL _
		'		   Where maestro.ID_USUARIO = idUsuario _
		'		   Select maestro

		'              If (MaestroPerso.Count = 1) Then
		'                  BBDD.MAESTRO_PERSONAL.DeleteOnSubmit(MaestroPerso.First)
		'			BBDD.SubmitChanges()

		'			Return True
		'              Else
		'                  Return False
		'              End If
		'          Catch ex As Exception
		'              Return False
		'          End Try
		'End Function

		''' <summary>
		''' Elimina un usuario
		''' </summary>
		''' <param name="idUsuario">Id del usuario</param>        
		''' <remarks></remarks>
		Public Sub EliminarMaestroPersonal(ByVal idUsuario As Integer)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim MaestroPerso = (From maestro In BBDD.MAESTRO_PERSONAL _
			   Where maestro.ID_USUARIO = idUsuario _
			   Select maestro).SingleOrDefault
			If MaestroPerso Is Nothing Then
				Throw New ApplicationException("No se ha encontrado el usuario.")
			End If
			BBDD.MAESTRO_PERSONAL.DeleteOnSubmit(MaestroPerso)
			BBDD.SubmitChanges()
		End Sub

#End Region

#Region "PLAN_CONTROL_PIEZA"
        ''' <summary>
        ''' Devuelve un registro de plan de control de pieza
        ''' </summary>
        ''' <param name="ref">Referencia del registro a buscar</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function consultarPlanControlPieza(ByVal ref As String) As KaPlanLib.Registro.PLAN_DE_CONTROL_PIEZA
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

				Dim RegPlan = From PlanControl In BBDD.PLAN_DE_CONTROL_PIEZA _
				Where PlanControl.CODIGO = ref _
				Select PlanControl

				If (Not RegPlan.Any) Then
					Return Nothing
				Else
					Return RegPlan.First
				End If
            Catch ex As Exception
				Throw 
            End Try
        End Function

		''' <summary>
		''' Guarda el plan de control.
		''' </summary>
		''' <param name="oPlan">Datos del plan de control</param>        
		Public Sub GuardarPlanControlPieza(ByVal oPlan As Registro.PLAN_DE_CONTROL_PIEZA)
			Dim transaction As System.Data.Common.DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try

				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

                '1º Comprobamos que exista la "Ficha de Articulo"
                Dim FichaArticulo As IQueryable(Of Registro.MAESTRO_ARTICULOS) = From Reg As KaPlanLib.Registro.MAESTRO_ARTICULOS In BBDD.MAESTRO_ARTICULOS Where Reg.CODIGO = oPlan.CODIGO Select Reg
                If Not FichaArticulo.Any Then Throw New ApplicationException(String.Format("{0} --> {1}", oPlan.CODIGO, ItzultzaileWeb.Itzuli("Referencia no existente")))

                '2º Guarda los datos en el plan de control pieza
                Dim bInsertar As Boolean = True
				Dim planContrPieza = (From PlanControlPieza In BBDD.PLAN_DE_CONTROL_PIEZA _
				   Where PlanControlPieza.CODIGO = oPlan.CODIGO _
				   Select PlanControlPieza).FirstOrDefault
				If (planContrPieza Is Nothing) Then	 'si no devuelve resultados, se hara un insert
					BBDD.PLAN_DE_CONTROL_PIEZA.InsertOnSubmit(oPlan)
				Else  'Sino se actualiza el registro con los nuevos datos
					'-----------------------------------------------------------------------------------
					'FROGA:2012-09-19: Comentamos el codigo pq da error si se intenta modificar este campo por ser el identificador del objeto.
					'Si no da errores borrar esto.
					'-----------------------------------------------------------------------------------
					'planContrPieza.CODIGO = oPlan.CODIGO
					'-----------------------------------------------------------------------------------
					planContrPieza.NIVEL = oPlan.NIVEL
					planContrPieza.FECHA = oPlan.FECHA
					planContrPieza.CLASE = oPlan.CLASE
					planContrPieza.EQUIPO = oPlan.EQUIPO
					planContrPieza.PLAN_RECEP = oPlan.PLAN_RECEP
					planContrPieza.TIPO_PLAN = oPlan.TIPO_PLAN
					planContrPieza.APROBACION_ING_CLI = oPlan.APROBACION_ING_CLI
					planContrPieza.APROBACION_CAL_CLI = oPlan.APROBACION_CAL_CLI
					planContrPieza.OTRAS_APROBACIONES1 = oPlan.OTRAS_APROBACIONES1
					planContrPieza.COD_SUMINISTRADOR = oPlan.COD_SUMINISTRADOR
					planContrPieza.SUMINISTRADOR = oPlan.SUMINISTRADOR
					planContrPieza.APROBACION_PLANTA = oPlan.APROBACION_PLANTA
					planContrPieza.OTRAS_APROBACIONES2 = oPlan.OTRAS_APROBACIONES2
					planContrPieza.TELEFONO_CONTACTO = oPlan.TELEFONO_CONTACTO
					planContrPieza.ACCIONES = oPlan.ACCIONES
				End If
				BBDD.SubmitChanges()

                '3º Eliminar plan pieza
                'If (Not EliminarPlanControlPiezaFabricacion(oPlan.CODIGO, BBDD)) Then Throw New BatzException("Error al eliminar el plan de control de pieza de fabricacion", Nothing)
                'If (Not EliminarPlanControlPiezaFabricacion(oPlan.CODIGO, BBDD)) Then Throw New Exception("Error al eliminar el plan de control de pieza de fabricacion")
                EliminarPlanControlPiezaFabricacion(oPlan.CODIGO, BBDD)

                '4º Añadir plan pieza
                Dim oPlanContPiezaFab As New Registro.PLAN_DE_CONTROL_PIEZA_FABRICACION
				oPlanContPiezaFab.CODIGO = oPlan.CODIGO
				oPlanContPiezaFab.NIVEL = oPlan.NIVEL
				oPlanContPiezaFab.FECHA = oPlan.FECHA
				oPlanContPiezaFab.CLASE = oPlan.CLASE
				oPlanContPiezaFab.EQUIPO = oPlan.EQUIPO
				oPlanContPiezaFab.PLAN_RECEP = oPlan.PLAN_RECEP
				oPlanContPiezaFab.TIPO_PLAN = oPlan.TIPO_PLAN
				oPlanContPiezaFab.APROBACION_ING_CLI = oPlan.APROBACION_ING_CLI
				oPlanContPiezaFab.APROBACION_CAL_CLI = oPlan.APROBACION_CAL_CLI
				oPlanContPiezaFab.OTRAS_APROBACIONES1 = oPlan.OTRAS_APROBACIONES1
				oPlanContPiezaFab.COD_SUMINISTRADOR = oPlan.COD_SUMINISTRADOR
				oPlanContPiezaFab.SUMINISTRADOR = oPlan.SUMINISTRADOR
				oPlanContPiezaFab.APROBACION_PLANTA = oPlan.APROBACION_PLANTA
				oPlanContPiezaFab.OTRAS_APROBACIONES2 = oPlan.OTRAS_APROBACIONES2
				oPlanContPiezaFab.TELEFONO_CONTACTO = oPlan.TELEFONO_CONTACTO
				'Estos dos campos, se cogen de la consulta porque en el formulario, no muestran ni modifican
				If planContrPieza IsNot Nothing Then
					oPlanContPiezaFab.CODIGO_CLIENTE = planContrPieza.CODIGO_CLIENTE
					oPlanContPiezaFab.PREPARADO_POR = planContrPieza.PREPARADO_POR
				End If
				oPlanContPiezaFab.ACCIONES = oPlan.ACCIONES

				'If (Not GuardarPlanControlPiezaFabricacion(oPlanContPiezaFab, BBDD)) Then Throw New BatzException("Error al guardar el plan de control de pieza de fabricacion", Nothing)
				'If (Not GuardarPlanControlPiezaFabricacion(oPlanContPiezaFab, BBDD)) Then Throw New Exception("Error al guardar el plan de control de pieza de fabricacion")
				GuardarPlanControlPiezaFabricacion(oPlanContPiezaFab, BBDD)

                '5º Añadir niveles plan pieza
                Try
					Dim nivelesPlan = From NivPlanControlPieza In BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA _
					   Where NivPlanControlPieza.CODIGO = oPlan.CODIGO _
					   Select NivPlanControlPieza

					If (nivelesPlan.Any) Then
						For Each nivPlan As Registro.NIVELES_PLANES_DE_CONTROL_PIEZA In nivelesPlan
							Dim newNivelFabr As New Registro.NIVELES_PLANES_DE_CONTROL_PIEZA_FABRICACION
							newNivelFabr.CODIGO = nivPlan.CODIGO
							newNivelFabr.FECHA = nivPlan.FECHA
							'newNivelFabr.ID_REGISTRO = nivPlan.ID_REGISTRO
							newNivelFabr.MODIFICACION = nivPlan.MODIFICACION
							newNivelFabr.NIVEL_PLAN = nivPlan.NIVEL_PLAN
							BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA_FABRICACION.InsertOnSubmit(newNivelFabr)
							BBDD.SubmitChanges()
						Next
					End If
				Catch ex As Exception
					Throw New Exception("Error al guardar los niveles plan de control de pieza de fabricacion", ex)
				End Try

				transaction.Commit()
			Catch ex As ApplicationException
				transaction.Rollback()
				Throw 
			Catch ex As Exception
				transaction.Rollback()
                log.Error(If(oPlan Is Nothing, "oPlan: Nothing", "PLAN_DE_CONTROL_PIEZA.CODIGO: " & oPlan.CODIGO), ex)
                Throw 
			Finally
				BBDD.Connection.Close()
			End Try
		End Sub

		''' <summary>
		''' Elimina una plan de control de pieza
		''' </summary>
		''' <param name="cod">Codigo</param>
		''' <returns></returns>
		''' <remarks></remarks>
        Public Function EliminarPlanControlPieza(ByVal cod As String) As Boolean
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim planControl = From Plan In BBDD.PLAN_DE_CONTROL_PIEZA _
                  Where Plan.CODIGO = cod _
                   Select Plan

				If (planControl.Count = 1) Then
					EliminarPlanControlPiezaFabricacion(planControl.First.CODIGO, BBDD)
					BBDD.PLAN_DE_CONTROL_PIEZA.DeleteOnSubmit(planControl.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
            Catch ex As Exception
                Return False
            End Try
        End Function
#End Region

#Region "PLAN DE CONTROL PIEZA FABRICACION"
		''' <summary>
		''' Guarda el plan de control de pieza de fabricacion
		''' </summary>
		''' <param name="oPlan">Datos del plan de control</param>        
		''' <param name="Conexion">Opcional:Indica si viene de una transaccion</param>
		Public Sub GuardarPlanControlPiezaFabricacion(ByVal oPlan As Registro.PLAN_DE_CONTROL_PIEZA_FABRICACION, Optional ByVal Conexion As KaPlanLib.DAL.ELL = Nothing)
			Try
				If (Conexion Is Nothing) Then
					'Aunque se asigne la transaccion, no se hara commit ni rollback porque la transaccion viene abierta de otro metodo que es donde se hara
					Conexion = New KaPlanLib.DAL.ELL
				End If

				Dim planContrPiezaFab As KaPlanLib.Registro.PLAN_DE_CONTROL_PIEZA_FABRICACION = (From PlanControlPiezaFabricacion In Conexion.PLAN_DE_CONTROL_PIEZA_FABRICACION _
				   Where PlanControlPiezaFabricacion.CODIGO = oPlan.CODIGO _
				   Select PlanControlPiezaFabricacion).FirstOrDefault

				If (planContrPiezaFab Is Nothing) Then 'insertar
					Conexion.PLAN_DE_CONTROL_PIEZA_FABRICACION.InsertOnSubmit(oPlan)
				Else
					planContrPiezaFab.CODIGO = oPlan.CODIGO
					planContrPiezaFab.NIVEL = oPlan.NIVEL
					planContrPiezaFab.FECHA = oPlan.FECHA
					planContrPiezaFab.CLASE = oPlan.CLASE
					planContrPiezaFab.EQUIPO = oPlan.EQUIPO
					planContrPiezaFab.PLAN_RECEP = oPlan.PLAN_RECEP
					planContrPiezaFab.TIPO_PLAN = oPlan.TIPO_PLAN
					planContrPiezaFab.APROBACION_ING_CLI = oPlan.APROBACION_ING_CLI
					planContrPiezaFab.APROBACION_CAL_CLI = oPlan.APROBACION_CAL_CLI
					planContrPiezaFab.OTRAS_APROBACIONES1 = oPlan.OTRAS_APROBACIONES1
					planContrPiezaFab.COD_SUMINISTRADOR = oPlan.COD_SUMINISTRADOR
					planContrPiezaFab.SUMINISTRADOR = oPlan.SUMINISTRADOR
					planContrPiezaFab.APROBACION_PLANTA = oPlan.APROBACION_PLANTA
					planContrPiezaFab.OTRAS_APROBACIONES2 = oPlan.OTRAS_APROBACIONES2
					planContrPiezaFab.TELEFONO_CONTACTO = oPlan.TELEFONO_CONTACTO
					planContrPiezaFab.CODIGO_CLIENTE = oPlan.CODIGO_CLIENTE
					planContrPiezaFab.PREPARADO_POR = oPlan.PREPARADO_POR
					planContrPiezaFab.ACCIONES = oPlan.ACCIONES
				End If
				Conexion.SubmitChanges()
			Catch ex As Exception
				log.Error(String.Format("oPlan.CODIGO: {0}", If(oPlan Is Nothing OrElse oPlan.CODIGO Is Nothing, "", oPlan.CODIGO)), ex)
				Throw New Exception("Error al guardar el plan de control de pieza de fabricacion", ex)
			End Try
		End Sub
		''' <summary>
		''' Elimina una plan de control de pieza fabricacion
		''' </summary>
		''' <param name="cod">Codigo</param>
		''' <param name="Conexion">Opcional:Indica si viene de una transaccion</param>
		''' <remarks></remarks>
		Public Sub EliminarPlanControlPiezaFabricacion(ByVal cod As String, Optional ByVal Conexion As KaPlanLib.DAL.ELL = Nothing)
			Try
				If (Conexion Is Nothing) Then
					'Aunque se asigne la transaccion, no se hara commit ni rollback porque la transaccion viene abierta de otro metodo que es donde se hara                    
					Conexion = New KaPlanLib.DAL.ELL
				End If

				Dim planControl = From Plan In Conexion.PLAN_DE_CONTROL_PIEZA_FABRICACION _
				  Where Plan.CODIGO = cod _
				   Select Plan

				If (planControl.Count = 1) Then
					Conexion.PLAN_DE_CONTROL_PIEZA_FABRICACION.DeleteOnSubmit(planControl.First)
					Conexion.SubmitChanges()
				End If
			Catch ex As Exception
				log.Error(ex)
				Throw New Exception("Error al eliminar el plan de control de pieza de fabricacion", ex)
			End Try
		End Sub
#End Region

#Region "PLAN_CONTROL"
		''' <summary>
		''' Devuelve un registro de plan de control
		''' </summary>
		''' <param name="cod">Codigo de operacion del registro a buscar</param>
		''' <returns></returns>
		''' <remarks></remarks>
        Function consultarPlanControl(ByVal cod As String) As KaPlanLib.Registro.PLAN_DE_CONTROL
			Try
				If String.IsNullOrWhiteSpace(cod) Then Throw New ApplicationException("Debe seleccionar una operacion")

				'------------------------------------------------------------------------------------------------------
				'Si no existe "Plan de Control" para la operacion lo creamos automaticamente.
				'------------------------------------------------------------------------------------------------------
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim PlanControl As New Registro.PLAN_DE_CONTROL
				Dim RegPlan As IQueryable(Of Registro.PLAN_DE_CONTROL) =
					From PC In BBDD.PLAN_DE_CONTROL Where PC.CODIGO = cod Select PC

				If RegPlan.Any Then
					PlanControl = RegPlan.FirstOrDefault
				Else
					PlanControl.CODIGO = cod
					PlanControl.FECHA = FormatDateTime(Now, DateFormat.ShortDate)
					PlanControl.NIVEL = "0"

					BBDD.PLAN_DE_CONTROL.InsertOnSubmit(PlanControl)
					BBDD.SubmitChanges()
				End If
				'-----------------------------------------------------------------------------------------------------

				Return PlanControl
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error("PLAN_DE_CONTROL.CODIGO = " & cod, ex)
				Throw 
			End Try
        End Function

        ''' <summary>
        ''' Guarda el plan de control de operacion
        ''' </summary>
        ''' <param name="planCont">Plan de control</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarNivelFechaPlanControlOperacion(ByVal planCont As Registro.PLAN_DE_CONTROL) As Boolean
            Dim BBDD As New KaPlanLib.DAL.ELL
            Try
                Dim planContr = (From PlanControl In BBDD.PLAN_DE_CONTROL _
                   Where PlanControl.CODIGO = planCont.CODIGO _
                   Select PlanControl).First
                If (planContr Is Nothing) Then  'si no devuelve resultados, se hara un insert
                    Return False
                Else  'Sino se actualiza el registro con los nuevos datos
                    planContr.NIVEL = planCont.NIVEL
                    planContr.FECHA = planCont.FECHA
                    BBDD.SubmitChanges()
                End If

                Return True
			Catch ex As Exception
				Throw New Exception("Error al guardar el plan de control de operacion", ex)
            End Try
        End Function

		''' <summary>
		''' Elimina un plan de control
		''' </summary>
		''' <param name="cod">Codigo</param>
		''' <remarks></remarks>
		Sub EliminarPlanControl(ByVal cod As String)
			Dim transaction As DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try

				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				'-------------------------------------------------------------
				'Eliminamos el plan
				'-------------------------------------------------------------
				'Dim planControl = From Plan In BBDD.PLAN_DE_CONTROL _
				'   Where Plan.CODIGO = cod _
				'   Select Plan
				'If (planControl.Count = 1) Then
				'	BBDD.PLAN_DE_CONTROL.DeleteOnSubmit(planControl.First)
				'	BBDD.SubmitChanges()
				'Else
				'	'Si no obtiene el plan de control, se hara un rollback
				'	transaction.Rollback()
				'	Return False
				'End If
				'-------------------------------------------------------------
				'FROGA:2012-12-18:
				'-------------------------------------------------------------
				Dim planControl As Registro.PLAN_DE_CONTROL = (From Plan In BBDD.PLAN_DE_CONTROL _
				   Where Plan.CODIGO = cod _
				   Select Plan).FirstOrDefault
				If planControl Is Nothing Then
					'transaction.Rollback()
					Throw New ApplicationException(String.Format("No se puede eliminiar el 'Plan de Control' para la operación '{0}'", cod))
				Else
					BBDD.PLAN_DE_CONTROL.DeleteOnSubmit(planControl)
					BBDD.SubmitChanges()
				End If
				'-------------------------------------------------------------

				'Eliminamos la hoja de instrucciones
				Dim hojaInstr = From HojaInstrucciones In BBDD.HOJA_DE_INSTRUCCIONES _
				   Where HojaInstrucciones.CODIGO = cod _
				   Select HojaInstrucciones

				If (hojaInstr.Count = 1) Then
					BBDD.HOJA_DE_INSTRUCCIONES.DeleteOnSubmit(hojaInstr.First)
					BBDD.SubmitChanges()
				End If

				transaction.Commit()
			Catch ex As ApplicationException
				transaction.Rollback()
				Throw 
			Catch ex As Exception
				log.Error(ex)
				transaction.Rollback()
				Throw 
			Finally
				BBDD.Connection.Close()
			End Try
		End Sub
#End Region

#Region "PLAN DE CONTROL FABRICACION"
        ''' <summary>
        ''' Guarda el plan de control de operacion
        ''' </summary>
        ''' <param name="codigo">Codigo de operacion</param>        
		''' <remarks></remarks>
		Public Sub GuardarPlanControlFabricacion(ByVal codigo As String)
			Dim transaction As System.Data.Common.DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim oCaracteristFab As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION = Nothing
			Try
				If String.IsNullOrWhiteSpace(codigo) Then Throw New ApplicationException("Debe seleccionar una operacion")

				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				'1º Elimina el plan 
				If (Not EliminarPlanControlFabricacion(codigo, BBDD)) Then Throw New ApplicationException("Error al eliminar el plan de control de fabricacion")

				'2º Copiar el plan 
				Dim oPlanControl As KaPlanLib.Registro.PLAN_DE_CONTROL = consultarPlanControl(codigo)
				Dim oPlanControlFab As New KaPlanLib.Registro.PLAN_DE_CONTROL_FABRICACION
				oPlanControlFab.CODIGO = oPlanControl.CODIGO
				oPlanControlFab.FECHA = oPlanControl.FECHA
				oPlanControlFab.NIVEL = oPlanControl.NIVEL
                BBDD.PLAN_DE_CONTROL_FABRICACION.InsertOnSubmit(oPlanControlFab)
                Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
                BBDD.SubmitChanges()

				'3º Copiar caracteristicas
				Dim lCaracteristicas = consultarListadoCaracteristicasPlan(codigo)

				For Each oCaract As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN In lCaracteristicas
					oCaracteristFab = New KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION
					oCaracteristFab.CODIGO = oCaract.CODIGO
					oCaracteristFab.OPERACION = oCaract.OPERACION
					oCaracteristFab.MAQUINA = oCaract.MAQUINA
					oCaracteristFab.ORDEN_CARAC = oCaract.ORDEN_CARAC

					oCaracteristFab.POSICION = oCaract.POSICION
					oCaracteristFab.CARAC_PARAM = oCaract.CARAC_PARAM

					oCaracteristFab.ESPECIFICACION = oCaract.ESPECIFICACION
					oCaracteristFab.FRECUENCIA_CONTROL = oCaract.FRECUENCIA_CONTROL
					oCaracteristFab.FRECUENCIA_CONTROL_CAL = oCaract.FRECUENCIA_CONTROL_CAL
					oCaracteristFab.FRECUENCIA_REGISTRO = oCaract.FRECUENCIA_REGISTRO
					oCaracteristFab.ID_Doc_Registro = oCaract.ID_Doc_Registro
					oCaracteristFab.METODO_EVALUACION = oCaract.METODO_EVALUACION
					oCaracteristFab.RESPONSABLE = oCaract.RESPONSABLE
					oCaracteristFab.MEDIO_DENOMINACION = oCaract.MEDIO_DENOMINACION

					oCaracteristFab.Control_DET_ME_CTRL = oCaract.Control_DET_ME_CTRL
					oCaracteristFab.Control_DET_ME_FAB = oCaract.Control_DET_ME_FAB
					oCaracteristFab.Codigo_Control_ME_FAB = oCaract.Codigo_Control_ME_FAB

					oCaracteristFab.CLASE = oCaract.CLASE
					oCaracteristFab.OBSERVACIONES = oCaract.OBSERVACIONES
					oCaracteristFab.ACCION_RECOMENDADA = oCaract.ACCION_RECOMENDADA
					oCaracteristFab.CONT_CAUSA = oCaract.CONT_CAUSA
					oCaracteristFab.ID_CARACTERISTICA = oCaract.ID_CARACTERISTICA
					oCaracteristFab.PROCEDE_DE = oCaract.PROCEDE_DE
					oCaracteristFab.PROCESO_PRODUCTO = oCaract.PROCESO_PRODUCTO
					'oCaracteristFab.TAMAÑO = If(oCaract.MAXIM Is Nothing, String.Empty, oCaract.MAXIM)
					oCaracteristFab.TAMAÑO = oCaract.TAMAÑO
					oCaracteristFab.TAMAÑO_CAL = oCaract.TAMAÑO_CAL
					oCaracteristFab.METODO_CONTROL = oCaract.METODO_CONTROL
					oCaracteristFab.METODO_CONTROL_FAB = oCaract.METODO_CONTROL_FAB
					oCaracteristFab.HOJA_REGISTROS = oCaract.HOJA_REGISTROS
					oCaracteristFab.VER_REG_REC = oCaract.VER_REG_REC
					oCaracteristFab.VER_REG_PRO = oCaract.VER_REG_PRO
					oCaracteristFab.VER_REG_DIM = oCaract.VER_REG_DIM
					oCaracteristFab.VER_REG_MAT = oCaract.VER_REG_MAT
					oCaracteristFab.VER_REG_FUN = oCaract.VER_REG_FUN
					oCaracteristFab.MAXIM = oCaract.MAXIM
					oCaracteristFab.MINIM = oCaract.MINIM
					oCaracteristFab.MEDIO_RFA = oCaract.MEDIO_RFA

					oCaracteristFab.Responsable_Maquina = oCaract.Responsable_Maquina
					oCaracteristFab.Responsable_Operario = oCaract.Responsable_Operario
					oCaracteristFab.Responsable_Calidad = oCaract.Responsable_Calidad
					'--------------------------------------------------------------------------------------------------------
					'Responsables de Registro.
					'--------------------------------------------------------------------------------------------------------
					oCaracteristFab.RESPONSABLE_REGISTRO = oCaract.RESPONSABLE_REGISTRO 'Campo Obsoleto. No usar para meter datos.
					oCaracteristFab.Responsable_Reg_Ope = oCaract.Responsable_Reg_Ope : oCaracteristFab.Responsable_Reg_Gestor = oCaract.Responsable_Reg_Gestor
					oCaracteristFab.Responsable_Reg_Cal = oCaract.Responsable_Reg_Cal
					'--------------------------------------------------------------------------------------------------------

					'--------------------------------------------------------------------------------------------------------
					'oCaracteristFab.AyudaVisual = oCaract.AyudaVisual
					'--------------------------------------------------------------------------------------------------------
					'FROGA:2013-03-05: Copia de la ayuda visual a Fabricacion.
					'--------------------------------------------------------------------------------------------------------
					AyudaVisualCopiar_FAB(oCaract, oCaracteristFab)
					'--------------------------------------------------------------------------------------------------------

					BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION.InsertOnSubmit(oCaracteristFab)
					BBDD.SubmitChanges()
				Next

				'4º Copiar niveles
				Dim lNivelesPC = consultarListadoNivelesPlanControl(codigo)
				Dim oNivelesPCFab As KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL_FABRICACION
				For Each oNivelPC As KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL In lNivelesPC
					oNivelesPCFab = New KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL_FABRICACION
					oNivelesPCFab.CODIGO = oNivelPC.CODIGO.Trim
					oNivelesPCFab.FECHA = oNivelPC.FECHA
					oNivelesPCFab.MODIFICACION = oNivelPC.MODIFICACION
					oNivelesPCFab.NIVEL_PLAN = oNivelPC.NIVEL_PLAN
					BBDD.NIVELES_PLANES_DE_CONTROL_FABRICACION.InsertOnSubmit(oNivelesPCFab)
					BBDD.SubmitChanges()
				Next

				transaction.Commit()
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				transaction.Rollback()
				log.Error("CARACTERISTICAS_DEL_PLAN.CODIGO= " & oCaracteristFab.CODIGO, ex)
				'Throw New Exception("Error al guardar el plan de control de operacion", ex)
				Throw
			Finally
				BBDD.Connection.Close()
			End Try
		End Sub
        ''' <summary>
        ''' Elimina una plan de control de fabricacion
        ''' </summary>
        ''' <param name="cod">Codigo</param>
        ''' <param name="Conexion">Opcional:Indica si viene de una transaccion</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarPlanControlFabricacion(ByVal cod As String, Optional ByVal Conexion As KaPlanLib.DAL.ELL = Nothing) As Boolean
            Try
                If (Conexion Is Nothing) Then
                    'Aunque se asigne la transaccion, no se hara commit ni rollback porque la transaccion viene abierta de otro metodo que es donde se hara                    
                    Conexion = New KaPlanLib.DAL.ELL
                End If

                Dim planControl = From Plan In Conexion.PLAN_DE_CONTROL_FABRICACION _
                  Where Plan.CODIGO = cod _
                   Select Plan

                If (planControl.Count = 1) Then
                    Conexion.PLAN_DE_CONTROL_FABRICACION.DeleteOnSubmit(planControl.First)
                    Funciones.Segumiento_Actividad(Conexion, New StackFrame(0).GetMethod().Name)
                    Conexion.SubmitChanges()
                End If
                Return True
			Catch ex As Exception
				log.Error("PLAN_DE_CONTROL_FABRICACION.CODIGO = " & cod, ex)
				Return False
            End Try
        End Function
#End Region

#Region "NIVEL_PLAN_CONTROL_PIEZA"
		'----------------------------------------------------------------------------------------------------------------------------
		' ''' <summary>
		' ''' Guarda el nivel del plan de control de pieza de fabricacion
		' ''' Si la fecha original y la del objeto son distintas, habra que eliminar el registro y volverlo a insertar. Porque es un campo principal
		' ''' </summary>
		' ''' <param name="oNivel">Datos del nivel plan de control</param>                
		' ''' <param name="fechaOrginal">Como se puede modificar el campo fecha y es clave, habra que mantener el valor original para luego poder hacer la where</param>
		' ''' <returns></returns>
		' ''' <remarks></remarks>
		'Public Function GuardarNivelPlanControlPieza(ByVal oNivel As Registro.NIVELES_PLANES_DE_CONTROL_PIEZA, ByVal fechaOrginal As String) As Boolean
		'    Dim transaction As System.Data.Common.DbTransaction = Nothing
		'    Dim BBDD As New KaPlanLib.DAL.ELL
		'    Dim bTransaccion As Boolean = False
		'    Try
		'        If (fechaOrginal = String.Empty) Then 'es nuevo: insert
		'            BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA.InsertOnSubmit(oNivel)
		'            BBDD.SubmitChanges()
		'        Else
		'            'Dim nivPlanContrPieza As KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL_PIEZA = (From nivPlanControl In BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA _
		'            '   Where nivPlanControl.CODIGO = oNivel.CODIGO And nivPlanControl.FECHA = fechaOrginal _
		'            '   Select nivPlanControl).FirstOrDefault
		'            Dim FechaOriginal As Date = CDate(fechaOrginal)
		'            Dim nivPlanContrPieza As KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL_PIEZA = (From nivPlanControl In BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA _
		'               Where nivPlanControl.CODIGO = oNivel.CODIGO _
		'               And nivPlanControl.FECHA.Year = FechaOriginal.Year And nivPlanControl.FECHA.Month = FechaOriginal.Month And nivPlanControl.FECHA.Day = FechaOriginal.Day _
		'               And nivPlanControl.FECHA.Hour = FechaOriginal.Hour And nivPlanControl.FECHA.Minute = FechaOriginal.Minute And nivPlanControl.FECHA.Second = FechaOriginal.Second _
		'               Select nivPlanControl).FirstOrDefault
		'            If (nivPlanContrPieza Is Nothing) Then Return False

		'            'If (fechaOrginal <> oNivel.FECHA.ToShortDateString) Then
		'            'If (Date.Compare(FechaOriginal.ToString("dd/MM/yyyy hh:mm:ss"), oNivel.FECHA.ToString("dd/MM/yyyy hh:mm:ss")) <> 0) Then
		'            If FechaOriginal.ToString("dd/MM/yyyy hh:mm:ss") <> oNivel.FECHA.ToString("dd/MM/yyyy hh:mm:ss") Then
		'                'hay que abrir una transaccion y primero borrar el registro y luego añadirlo
		'                bTransaccion = True
		'                BBDD.Connection.Open()
		'                transaction = BBDD.Connection.BeginTransaction
		'                BBDD.Transaction = transaction
		'                '1º Se guardan los datos del objeto
		'                Dim nivPlanContrPiezaInsert As New KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL_PIEZA
		'                nivPlanContrPiezaInsert.CODIGO = oNivel.CODIGO
		'                nivPlanContrPiezaInsert.FECHA = oNivel.FECHA
		'                'nivPlanContrPiezaInsert.ID_REGISTRO = nivPlanContrPieza.ID_REGISTRO	 'se coge de la base de datos porque no esta informado en el objeto
		'                nivPlanContrPiezaInsert.MODIFICACION = oNivel.MODIFICACION
		'                nivPlanContrPiezaInsert.NIVEL_PLAN = oNivel.NIVEL_PLAN
		'                '2º Se elimina el registro
		'                BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA.DeleteOnSubmit(nivPlanContrPieza)
		'                BBDD.SubmitChanges()
		'                '3º Se vuelve a insertar
		'                BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA.InsertOnSubmit(nivPlanContrPiezaInsert)
		'                BBDD.SubmitChanges()

		'                transaction.Commit()
		'            Else  'no se ha modificado la fecha, por tanto se hace un update
		'                nivPlanContrPieza.CODIGO = oNivel.CODIGO
		'                nivPlanContrPieza.FECHA = oNivel.FECHA
		'                nivPlanContrPieza.MODIFICACION = oNivel.MODIFICACION
		'                nivPlanContrPieza.NIVEL_PLAN = oNivel.NIVEL_PLAN
		'                BBDD.SubmitChanges()
		'            End If
		'        End If

		'        Return True
		'    Catch batzEx As BatzException
		'        If (bTransaccion) Then transaction.Rollback()
		'        Return False
		'    Catch ex As Exception
		'        If (bTransaccion) Then transaction.Rollback()
		'        Return False
		'    Finally
		'        If (bTransaccion) Then BBDD.Connection.Close()
		'    End Try
		'End Function
		' ''' <summary>
		' ''' Elimina un nivel de plan de control de pieza
		' ''' </summary>
		' ''' <param name="codigo">Codigo del nivel</param>
		' ''' <param name="fecha">Fecha del nivel</param>
		' ''' <returns></returns>
		' ''' <remarks></remarks>
		'Public Function EliminarNivelPlanControlPieza(ByVal codigo As String, ByVal fecha As String) As Boolean
		'	Try
		'		Dim BBDD As New KaPlanLib.DAL.ELL
		'		Dim dfecha As Date = fecha
		'		'-------------------------------------------------------------
		'		'Consulta para evitar los milisegundos de las fechas.
		'		'-------------------------------------------------------------
		'		Dim NivelPlanControl = From NivelPlan In BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA _
		'		 Where NivelPlan.CODIGO = codigo _
		'		 And NivelPlan.FECHA.Date = dfecha.Date _
		'		 And NivelPlan.FECHA.Hour = dfecha.Hour And NivelPlan.FECHA.Minute = dfecha.Minute And NivelPlan.FECHA.Second = dfecha.Second _
		'		 Select NivelPlan
		'		'-------------------------------------------------------------

		'		If (NivelPlanControl.Count = 1) Then
		'			BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA.DeleteOnSubmit(NivelPlanControl.First)
		'			BBDD.SubmitChanges()
		'			Return True
		'		Else
		'			Return False
		'		End If
		'	Catch ex As Exception
		'		Return False
		'	End Try
		'End Function

		'----------------------------------------------------------------------------------------------------------------------------
		'FROGA: 2012-10-08:
		'----------------------------------------------------------------------------------------------------------------------------
		''' <summary>
		''' Guarda el nivel del plan de control de pieza de fabricacion
		''' Si la fecha original y la del objeto son distintas, habra que eliminar el registro y volverlo a insertar. Porque es un campo principal
		''' </summary>
		''' <param name="oNivel">Datos del nivel plan de control</param>                
		''' <param name="fechaOrginal">Como se puede modificar el campo fecha y es clave, habra que mantener el valor original para luego poder hacer la where</param>
		''' <remarks></remarks>
		Public Sub GuardarNivelPlanControlPieza(ByVal oNivel As Registro.NIVELES_PLANES_DE_CONTROL_PIEZA, ByVal fechaOrginal As String)
			Dim transaction As System.Data.Common.DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim bTransaccion As Boolean = False
			Try
				If (fechaOrginal = String.Empty) Then 'es nuevo: insert
					BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA.InsertOnSubmit(oNivel)
					BBDD.SubmitChanges()
				Else
					Dim FechaOriginal As Date = CDate(fechaOrginal)
					Dim nivPlanContrPieza As KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL_PIEZA = (From nivPlanControl In BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA _
					   Where nivPlanControl.CODIGO = oNivel.CODIGO _
					   And nivPlanControl.FECHA.Year = FechaOriginal.Year And nivPlanControl.FECHA.Month = FechaOriginal.Month And nivPlanControl.FECHA.Day = FechaOriginal.Day _
					   And nivPlanControl.FECHA.Hour = FechaOriginal.Hour And nivPlanControl.FECHA.Minute = FechaOriginal.Minute And nivPlanControl.FECHA.Second = FechaOriginal.Second _
					   Select nivPlanControl).FirstOrDefault
					If (nivPlanContrPieza Is Nothing) Then Throw New ApplicationException("No existe Nivel para el Plan de Control.")

					If FechaOriginal.ToString("dd/MM/yyyy hh:mm:ss") <> oNivel.FECHA.ToString("dd/MM/yyyy hh:mm:ss") Then
						'hay que abrir una transaccion y primero borrar el registro y luego añadirlo
						bTransaccion = True
						BBDD.Connection.Open()
						transaction = BBDD.Connection.BeginTransaction
						BBDD.Transaction = transaction
						'1º Se guardan los datos del objeto
						Dim nivPlanContrPiezaInsert As New KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL_PIEZA
						nivPlanContrPiezaInsert.CODIGO = oNivel.CODIGO
						nivPlanContrPiezaInsert.FECHA = oNivel.FECHA
						'nivPlanContrPiezaInsert.ID_REGISTRO = nivPlanContrPieza.ID_REGISTRO	 'se coge de la base de datos porque no esta informado en el objeto
						nivPlanContrPiezaInsert.MODIFICACION = oNivel.MODIFICACION
						nivPlanContrPiezaInsert.NIVEL_PLAN = oNivel.NIVEL_PLAN
						'2º Se elimina el registro
						BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA.DeleteOnSubmit(nivPlanContrPieza)
						BBDD.SubmitChanges()
						'3º Se vuelve a insertar
						BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA.InsertOnSubmit(nivPlanContrPiezaInsert)
						BBDD.SubmitChanges()

						transaction.Commit()
					Else  'no se ha modificado la fecha, por tanto se hace un update
						nivPlanContrPieza.CODIGO = oNivel.CODIGO
						nivPlanContrPieza.FECHA = oNivel.FECHA
						nivPlanContrPieza.MODIFICACION = oNivel.MODIFICACION
						nivPlanContrPieza.NIVEL_PLAN = oNivel.NIVEL_PLAN
						BBDD.SubmitChanges()
					End If
				End If
			Catch ex As ApplicationException
				If (bTransaccion) Then transaction.Rollback()
				Throw 
			Catch ex As Exception
				If (bTransaccion) Then transaction.Rollback()
				Throw
			Finally
				If (bTransaccion) Then BBDD.Connection.Close()
			End Try
		End Sub

		''' <summary>
		''' Elimina un nivel de plan de control de pieza
		''' </summary>
		''' <remarks></remarks>
		Public Sub EliminarNivelPlanControlPieza(ByVal Id_Registro As Integer)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				'-------------------------------------------------------------
				'Consulta para evitar los milisegundos de las fechas.
				'-------------------------------------------------------------
				Dim NivelPlanControl = From NivelPlan In BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA _
				 Where NivelPlan.ID_REGISTRO = Id_Registro Select NivelPlan
				'-------------------------------------------------------------

				If NivelPlanControl IsNot Nothing AndAlso (NivelPlanControl.Count = 1) Then
					BBDD.NIVELES_PLANES_DE_CONTROL_PIEZA.DeleteOnSubmit(NivelPlanControl.First)
					BBDD.SubmitChanges()
				Else
					Throw New ApplicationException("No existe Nivel para el Plan de Control.")
				End If
			Catch ex As ApplicationException
				Throw 
			Catch ex As Exception
				log.Error(ex)
				Throw 
			End Try
		End Sub
		'----------------------------------------------------------------------------------------------------------------------------
#End Region

#Region "NIVEL_PLAN_CONTROL"

		''' <summary>
		''' Devuelve todos los niveles de un plan de control
		''' </summary>
		''' <param name="codigo">Codigo</param>
		''' <returns></returns>
		Function consultarListadoNivelesPlanControl(ByVal codigo As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listNiveles = From Niveles In BBDD.NIVELES_PLANES_DE_CONTROL _
				   Where Niveles.CODIGO.Trim = codigo.ToString.Trim Select Niveles
				Return listNiveles
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		' ''' <summary>
		' ''' Guarda el nivel del plan de control
		' ''' Si la fecha original y la del objeto son distintas, habra que eliminar el registro y volverlo a insertar. Porque es un campo principal
		' ''' </summary>
		' ''' <param name="oNivel">Datos del nivel plan de control</param>                
		' ''' <param name="fechaOrginal">Como se puede modificar el campo fecha y es clave, habra que mantener el valor original para luego poder hacer la where</param>
		' ''' <returns></returns>
		' ''' <remarks></remarks>
		'Public Function GuardarNivelPlanControl(ByVal oNivel As Registro.NIVELES_PLANES_DE_CONTROL, ByVal fechaOrginal As String) As Boolean
		'	Dim transaction As System.Data.Common.DbTransaction = Nothing
		'	Dim BBDD As New KaPlanLib.DAL.ELL
		'	Dim bTransaccion As Boolean = False
		'	Try
		'		If (fechaOrginal = String.Empty) Then 'es nuevo: insert
		'			BBDD.NIVELES_PLANES_DE_CONTROL.InsertOnSubmit(oNivel)
		'			BBDD.SubmitChanges()
		'		Else
		'			Dim nivPlanContr As KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL = (From nivPlanControl In BBDD.NIVELES_PLANES_DE_CONTROL _
		'			   Where nivPlanControl.CODIGO.Trim = oNivel.CODIGO.Trim And nivPlanControl.FECHA = fechaOrginal _
		'			   Select nivPlanControl).FirstOrDefault
		'			If (nivPlanContr Is Nothing) Then Return False

		'			If (fechaOrginal <> oNivel.FECHA.ToShortDateString) Then
		'				'hay que abrir una transaccion y primero borrar el registro y luego añadirlo
		'				bTransaccion = True
		'				BBDD.Connection.Open()
		'				transaction = BBDD.Connection.BeginTransaction
		'				BBDD.Transaction = transaction
		'				'1º Se guardan los datos del objeto
		'				Dim nivPlanContrInsert As New KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL
		'				nivPlanContrInsert.CODIGO = oNivel.CODIGO.Trim
		'				nivPlanContrInsert.FECHA = oNivel.FECHA
		'				nivPlanContrInsert.MODIFICACION = oNivel.MODIFICACION
		'				nivPlanContrInsert.NIVEL_PLAN = oNivel.NIVEL_PLAN
		'				'2º Se elimina el registro
		'				BBDD.NIVELES_PLANES_DE_CONTROL.DeleteOnSubmit(nivPlanContr)
		'				BBDD.SubmitChanges()
		'				'3º Se vuelve a insertar
		'				BBDD.NIVELES_PLANES_DE_CONTROL.InsertOnSubmit(nivPlanContrInsert)
		'				BBDD.SubmitChanges()

		'				transaction.Commit()
		'			Else  'no se ha modificado la fecha, por tanto se hace un update
		'				nivPlanContr.CODIGO = oNivel.CODIGO.Trim
		'				nivPlanContr.FECHA = oNivel.FECHA
		'				nivPlanContr.MODIFICACION = oNivel.MODIFICACION
		'				nivPlanContr.NIVEL_PLAN = oNivel.NIVEL_PLAN
		'				BBDD.SubmitChanges()
		'			End If
		'		End If

		'		Return True
		'	Catch batzEx As BatzException
		'		If (bTransaccion) Then transaction.Rollback()
		'		Return False
		'	Catch ex As Exception
		'		log.Error(ex)
		'		If (bTransaccion) Then transaction.Rollback()
		'		Return False
		'	Finally
		'		If (bTransaccion) Then BBDD.Connection.Close()
		'	End Try
		'End Function

		''' <summary>
		''' Guarda el nivel del plan de control
		''' Si la fecha original y la del objeto son distintas, habra que eliminar el registro y volverlo a insertar. Porque es un campo principal
		''' </summary>
		''' <param name="oNivel">Datos del nivel plan de control</param>                
		''' <param name="fechaOrginal">Como se puede modificar el campo fecha y es clave, habra que mantener el valor original para luego poder hacer la where</param>
		''' <remarks></remarks>
		Public Sub GuardarNivelPlanControl(ByVal oNivel As Registro.NIVELES_PLANES_DE_CONTROL, ByVal fechaOrginal As String)
			Dim transaction As System.Data.Common.DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim bTransaccion As Boolean = False
			Try
				If (fechaOrginal = String.Empty) Then 'es nuevo: insert
					Dim nivPlanContr As IQueryable(Of KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL) = From nivPlanControl In BBDD.NIVELES_PLANES_DE_CONTROL _
					   Where nivPlanControl.CODIGO.Trim = oNivel.CODIGO.Trim And nivPlanControl.FECHA = oNivel.FECHA Select nivPlanControl
					If nivPlanContr.Any Then Throw New ApplicationException("No se puede crear un nivel con la misma fecha")
					BBDD.NIVELES_PLANES_DE_CONTROL.InsertOnSubmit(oNivel)
					BBDD.SubmitChanges()
				Else
					Dim nivPlanContr As KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL = (From nivPlanControl In BBDD.NIVELES_PLANES_DE_CONTROL _
					   Where nivPlanControl.CODIGO.Trim = oNivel.CODIGO.Trim And nivPlanControl.FECHA = fechaOrginal _
					   Select nivPlanControl).FirstOrDefault
					If (nivPlanContr Is Nothing) Then Throw New ApplicationException("Nivel no encontrado")

					If (fechaOrginal <> oNivel.FECHA.ToShortDateString) Then
						'hay que abrir una transaccion y primero borrar el registro y luego añadirlo
						bTransaccion = True
						BBDD.Connection.Open()
						transaction = BBDD.Connection.BeginTransaction
						BBDD.Transaction = transaction
						'1º Se guardan los datos del objeto
						Dim nivPlanContrInsert As New KaPlanLib.Registro.NIVELES_PLANES_DE_CONTROL
						nivPlanContrInsert.CODIGO = oNivel.CODIGO.Trim
						nivPlanContrInsert.FECHA = oNivel.FECHA
						nivPlanContrInsert.MODIFICACION = oNivel.MODIFICACION
						nivPlanContrInsert.NIVEL_PLAN = oNivel.NIVEL_PLAN
						'2º Se elimina el registro
						BBDD.NIVELES_PLANES_DE_CONTROL.DeleteOnSubmit(nivPlanContr)
						BBDD.SubmitChanges()
						'3º Se vuelve a insertar
						BBDD.NIVELES_PLANES_DE_CONTROL.InsertOnSubmit(nivPlanContrInsert)
						BBDD.SubmitChanges()

						transaction.Commit()
					Else  'no se ha modificado la fecha, por tanto se hace un update
						nivPlanContr.CODIGO = oNivel.CODIGO.Trim
						nivPlanContr.FECHA = oNivel.FECHA
						nivPlanContr.MODIFICACION = oNivel.MODIFICACION
						nivPlanContr.NIVEL_PLAN = oNivel.NIVEL_PLAN
						BBDD.SubmitChanges()
					End If
				End If
			Catch ex As ApplicationException
                If (bTransaccion) Then transaction.Rollback()
                Throw
			Catch ex As Exception
				log.Error(ex)
				If (bTransaccion) Then transaction.Rollback()
				Throw
			Finally
				If (bTransaccion) Then BBDD.Connection.Close()
			End Try
		End Sub

		''' <summary>
		''' Elimina un nivel de plan de control
		''' </summary>
		''' <param name="codigo">Codigo del nivel</param>
		''' <param name="fecha">Fecha del nivel</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarNivelPlanControl(ByVal codigo As String, ByVal fecha As String) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim NivelPlanControl = From NivelPlan In BBDD.NIVELES_PLANES_DE_CONTROL _
				   Where NivelPlan.CODIGO.Trim = codigo.Trim And NivelPlan.FECHA = fecha _
				   Select NivelPlan

				If (NivelPlanControl.Count = 1) Then
					BBDD.NIVELES_PLANES_DE_CONTROL.DeleteOnSubmit(NivelPlanControl.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "NIVELES_AMFE_PIEZA"
		''' <summary>
		''' Devuelve todos los niveles de un amfe pieza
		''' </summary>
		''' <param name="idAmfePieza">Codigo por el que se agrupan los niveles de Amfe</param>
		''' <returns></returns>
		Function consultarListadoNivelesAmfePieza(ByVal idAmfePieza As Integer) As List(Of KaPlanLib.Registro.NIVELES_AMFE_PIEZA)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listNiveles = From Niveles In BBDD.NIVELES_AMFE_PIEZA _
				   Where Niveles.IDENTIFICA = idAmfePieza _
				   Select Niveles
				Return listNiveles.ToList
			Catch ex As Exception
				Throw
			End Try
		End Function

		''' <summary>
		''' Guarda el nivel del amfe de pieza        
		''' </summary>
		''' <param name="oNivel">Datos del nivel amfe de la pieza</param>                        
		''' <remarks></remarks>
		Public Sub GuardarNivelAmfePieza(ByVal oNivel As Registro.NIVELES_AMFE_PIEZA)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim OrdenOriginal As Integer = Nothing
			Try
				'-------------------------------------------------------
				'Abrimos la conexion y creamos una transaccion
				'-------------------------------------------------------
				BBDD.Connection.Open()
				BBDD.Transaction = BBDD.Connection.BeginTransaction
				'-------------------------------------------------------

				If (oNivel.ID_REGISTRO = 0) Then 'Nuevo
					BBDD.NIVELES_AMFE_PIEZA.InsertOnSubmit(oNivel)
					BBDD.SubmitChanges()
					'ReorganizarOrden_NIVELES_AMFE_PIEZA(oNivel, oNivel.NIVEL_AMFE)
				Else  'Modificar
					Dim nivelAmfePieza As KaPlanLib.Registro.NIVELES_AMFE_PIEZA = (From nivAmfePieza In BBDD.NIVELES_AMFE_PIEZA _
					   Where nivAmfePieza.ID_REGISTRO = oNivel.ID_REGISTRO _
					   Select nivAmfePieza).FirstOrDefault
					If (nivelAmfePieza Is Nothing) Then Throw New ApplicationException("No se ha encontrado el nivel.")

                    OrdenOriginal = If(IsNumeric(nivelAmfePieza.NIVEL_AMFE), nivelAmfePieza.NIVEL_AMFE, Nothing)

					nivelAmfePieza.CODIGO = oNivel.CODIGO
					nivelAmfePieza.FECHA = oNivel.FECHA
					nivelAmfePieza.MODIFICACION = oNivel.MODIFICACION
					nivelAmfePieza.NIVEL_AMFE = oNivel.NIVEL_AMFE
					nivelAmfePieza.IDENTIFICA = oNivel.IDENTIFICA
					BBDD.SubmitChanges()

					oNivel = nivelAmfePieza
				End If

                ReorganizarOrden_NIVELES_AMFE_PIEZA(oNivel, OrdenOriginal, BBDD)

				BBDD.Transaction.Commit()

			Catch ex As ApplicationException
				log.Error(ex)
				Throw 
            Catch ex As Exception
                Dim msg As String = String.Format("oNivel.ID_REGISTRO = {0}", oNivel.ID_REGISTRO)
                log.Error(msg, ex)
                Throw
			Finally
				BBDD.Transaction.Dispose()
				BBDD.Connection.Close()
				BBDD.Dispose()
			End Try
		End Sub
		''' <summary>
		''' Elimina un nivel de amfe pieza
		''' </summary>
		''' <param name="idRegistro">Id Registro del nivel</param>        
		''' <remarks></remarks>
		Public Sub EliminarNivelAmfePieza(ByVal idRegistro As Integer)
			Dim transaction As System.Data.Common.DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim IDENTIFICA As Integer

			Try
				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				Dim NivelAmfePieza As IQueryable(Of Registro.NIVELES_AMFE_PIEZA) = From NivelAmfe In BBDD.NIVELES_AMFE_PIEZA _
				   Where NivelAmfe.ID_REGISTRO = idRegistro _
				   Select NivelAmfe

				If (NivelAmfePieza.Count = 1) Then
					Dim Nivel As Registro.NIVELES_AMFE_PIEZA = NivelAmfePieza.First
					Dim CODIGO As String = Nivel.AMFES_PIEZA.CODIGO
					IDENTIFICA = Nivel.IDENTIFICA

					BBDD.NIVELES_AMFE_PIEZA.DeleteOnSubmit(Nivel)
					BBDD.SubmitChanges()

					ReorganizarOrden_NIVELES_AMFE_PIEZA(IDENTIFICA, BBDD)

					'----------------------------------------------------------------
					'Actualizamos la Fecha y Nivel del AMFE Articulo.
					'----------------------------------------------------------------
					Dim AmfePieza As KaPlanLib.Registro.AMFES_PIEZA = (From AmfesPieza In BBDD.AMFES_PIEZA _
																	   Where AmfesPieza.CODIGO = CODIGO Select AmfesPieza).FirstOrDefault
					Nivel = (From nap As Registro.NIVELES_AMFE_PIEZA In AmfePieza.NIVELES_AMFE_PIEZA _
							 Where IsNumeric(nap.NIVEL_AMFE) Select nap Order By CInt(nap.NIVEL_AMFE)).ToList.LastOrDefault
					If Nivel Is Nothing Then
						AmfePieza.NIVEL_AMFE = Nothing
						AmfePieza.FECHA_AMFE = Nothing
					Else
						AmfePieza.NIVEL_AMFE = Nivel.NIVEL_AMFE
						AmfePieza.FECHA_AMFE = Nivel.FECHA
					End If
					'----------------------------------------------------------------
				Else
					Throw New ApplicationException("No se puede borrar porque se ha encontrado mas de un nivel.")
				End If

				transaction.Commit()
			Catch ex As ApplicationException
				transaction.Rollback()
				Throw 
			Catch ex As Exception
				transaction.Rollback()
				log.Error(ex)
				Throw 
			Finally
				BBDD.Connection.Close()
			End Try
		End Sub

		''' <summary>
		''' Ordena los registro de forma consecutiva del primero al ultimo.
		''' </summary>
		''' <param name="IDENTIFICA">Identificador del AMFE_PIEZA.</param>
		''' <remarks></remarks>
		Sub ReorganizarOrden_NIVELES_AMFE_PIEZA(ByVal IDENTIFICA As String, BBDD As KaPlanLib.DAL.ELL)
			Dim Orden As Integer = 0
			'-----------------------------------------------------------------
			'Dim lReg As List(Of Registro.NIVELES_AMFE_PIEZA) = _
			' (From Reg As Registro.NIVELES_AMFE_PIEZA In BBDD.NIVELES_AMFE_PIEZA Where Reg.IDENTIFICA = IDENTIFICA Select Reg Order By CInt(Reg.NIVEL_AMFE) Ascending, Reg.ID_REGISTRO Descending).ToList
			'-----------------------------------------------------------------
			Dim lReg As List(Of Registro.NIVELES_AMFE_PIEZA) = _
			 (From Reg As Registro.NIVELES_AMFE_PIEZA In BBDD.NIVELES_AMFE_PIEZA Where Reg.IDENTIFICA = IDENTIFICA Select Reg).ToList _
			 .Where(Function(o) IsNumeric(o.NIVEL_AMFE)).OrderBy(Function(o) CInt(o.NIVEL_AMFE)).ThenByDescending(Function(o) o.ID_REGISTRO).ToList
			'-----------------------------------------------------------------
			If lReg IsNot Nothing AndAlso lReg.Any Then
				For Each Reg As Registro.NIVELES_AMFE_PIEZA In lReg
					Orden += 1
					Reg.NIVEL_AMFE = Orden
				Next
				BBDD.SubmitChanges()
			End If
		End Sub
		''' <summary>
		''' Ordenacion de los registros
		''' </summary>
		''' <param name="NIVELES_AMFE_PIEZA">Tipo de elementos que se va a reordenar.</param>
		''' <param name="OrdenOriginal">Posicion original que ocupaba el elemento.</param>
		''' <remarks></remarks>
		Sub ReorganizarOrden_NIVELES_AMFE_PIEZA(ByVal NIVELES_AMFE_PIEZA As Registro.NIVELES_AMFE_PIEZA, ByRef OrdenOriginal As Integer, BBDD As KaPlanLib.DAL.ELL)
			Dim Orden As Integer

            If OrdenOriginal > NIVELES_AMFE_PIEZA.NIVEL_AMFE Or OrdenOriginal = 0 Then  'Se produce al retrasar la posicion del orden o al crear uno nuevo.
                '--------------------------------------------------------------------------------------------------------------
				'Dim lReg As List(Of Registro.NIVELES_AMFE_PIEZA) = _
				'	(From Reg As Registro.NIVELES_AMFE_PIEZA In BBDD.NIVELES_AMFE_PIEZA _
				'	Where Reg.ID_REGISTRO <> NIVELES_AMFE_PIEZA.ID_REGISTRO And Reg.IDENTIFICA = NIVELES_AMFE_PIEZA.IDENTIFICA _
				'	And IsNumeric(Reg.NIVEL_AMFE) And CInt(Reg.NIVEL_AMFE) >= CInt(NIVELES_AMFE_PIEZA.NIVEL_AMFE) _
				'	Select Reg Order By CInt(Reg.NIVEL_AMFE) Ascending, Reg.ID_REGISTRO Descending).ToList
				'--------------------------------
				Dim lReg As List(Of Registro.NIVELES_AMFE_PIEZA) = _
					(From Reg As Registro.NIVELES_AMFE_PIEZA In BBDD.NIVELES_AMFE_PIEZA _
					Where Reg.ID_REGISTRO <> NIVELES_AMFE_PIEZA.ID_REGISTRO And Reg.IDENTIFICA = NIVELES_AMFE_PIEZA.IDENTIFICA _
					Select Reg).ToList _
				.Where(Function(o) IsNumeric(o.NIVEL_AMFE) AndAlso CInt(o.NIVEL_AMFE) >= CInt(NIVELES_AMFE_PIEZA.NIVEL_AMFE)).OrderBy(Function(o) CInt(o.NIVEL_AMFE)).ThenByDescending(Function(o) o.ID_REGISTRO).ToList
				'--------------------------------
                If lReg IsNot Nothing AndAlso lReg.Any Then
                    Orden = NIVELES_AMFE_PIEZA.NIVEL_AMFE 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
                    For Each Reg As Registro.NIVELES_AMFE_PIEZA In lReg
                        Orden += 1
                        Reg.NIVEL_AMFE = Orden
                    Next
                    BBDD.SubmitChanges()
					End If
					'--------------------------------------------------------------------------------------------------------------
            ElseIf OrdenOriginal < NIVELES_AMFE_PIEZA.NIVEL_AMFE Then 'Se produce al avanzar la posicion  del orden.
					'--------------------------------------------------------------------------------------------------------------
				'Dim lReg As List(Of Registro.NIVELES_AMFE_PIEZA) = _
				'	(From Reg As Registro.NIVELES_AMFE_PIEZA In BBDD.NIVELES_AMFE_PIEZA _
				'	Where Reg.ID_REGISTRO <> NIVELES_AMFE_PIEZA.ID_REGISTRO And Reg.IDENTIFICA = NIVELES_AMFE_PIEZA.IDENTIFICA And CInt(Reg.NIVEL_AMFE) <= CInt(NIVELES_AMFE_PIEZA.NIVEL_AMFE) _
				'	Select Reg Order By CInt(Reg.NIVEL_AMFE) Descending, Reg.ID_REGISTRO Ascending).ToList
				'----------------------------------------
				Dim lReg As List(Of Registro.NIVELES_AMFE_PIEZA) = _
					(From Reg As Registro.NIVELES_AMFE_PIEZA In BBDD.NIVELES_AMFE_PIEZA _
					Where Reg.ID_REGISTRO <> NIVELES_AMFE_PIEZA.ID_REGISTRO And Reg.IDENTIFICA = NIVELES_AMFE_PIEZA.IDENTIFICA _
					Select Reg).ToList _
				.Where(Function(o) IsNumeric(o.NIVEL_AMFE) AndAlso CInt(o.NIVEL_AMFE) <= CInt(NIVELES_AMFE_PIEZA.NIVEL_AMFE)).OrderByDescending(Function(o) CInt(o.NIVEL_AMFE)).ThenBy(Function(o) o.ID_REGISTRO).ToList
				'----------------------------------------
                If lReg IsNot Nothing AndAlso lReg.Any Then
                    Orden = NIVELES_AMFE_PIEZA.NIVEL_AMFE 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
                    For Each Reg As Registro.NIVELES_AMFE_PIEZA In lReg
                        Orden -= 1
                        Reg.NIVEL_AMFE = Orden
                    Next
                    BBDD.SubmitChanges()
					End If
					'--------------------------------------------------------------------------------------------------------------
            End If
			ReorganizarOrden_NIVELES_AMFE_PIEZA(NIVELES_AMFE_PIEZA.IDENTIFICA, BBDD)
		End Sub
#End Region

#Region "NIVELES_AMFE"

		''' <summary>
		''' Guarda el nivel del amfe        
		''' </summary>
		''' <param name="oNivel">Datos del nivel amfe</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarNivelAmfe(ByVal oNivel As Registro.NIVELES_AMFE) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oNivel.ID_REGISTRO = 0) Then 'Nuevo
					'Se consulta el idAmfe de ese codigo
					Dim oAmfe As Registro.AMFES = consultarAmfe(oNivel.CODIGO)
					If (oAmfe IsNot Nothing) Then oNivel.IDENTIFICA = oAmfe.ID

					BBDD.NIVELES_AMFE.InsertOnSubmit(oNivel)
					Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim nivelAmfe As KaPlanLib.Registro.NIVELES_AMFE = (From nivAmfe In BBDD.NIVELES_AMFE _
					   Where nivAmfe.ID_REGISTRO = oNivel.ID_REGISTRO _
					   Select nivAmfe).FirstOrDefault
					If (nivelAmfe Is Nothing) Then Return False

					nivelAmfe.CODIGO = oNivel.CODIGO
					nivelAmfe.FECHA = oNivel.FECHA
					nivelAmfe.MODIFICACION = oNivel.MODIFICACION
					nivelAmfe.NIVEL_AMFE = oNivel.NIVEL_AMFE
					nivelAmfe.IDENTIFICA = oNivel.IDENTIFICA
					Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As ApplicationException
				Throw 
			Catch ex As Exception
				log.Error(ex)
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina un nivel de amfe
		''' </summary>
		''' <param name="idRegistro">Id Registro del nivel</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarNivelAmfe(ByVal idRegistro As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim NivelAmfePieza = From NivelAmfe In BBDD.NIVELES_AMFE _
				   Where NivelAmfe.ID_REGISTRO = idRegistro _
				   Select NivelAmfe

				If (NivelAmfePieza.Count = 1) Then
					BBDD.NIVELES_AMFE.DeleteOnSubmit(NivelAmfePieza.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "DISTRIBUCION_DOCUMENTACION"

		''' <summary>
		''' Devuelve el desglose de tareas
		''' </summary>
		''' <param name="idOrigen">Origen</param>
		''' <param name="de">Campo DE</param>
		''' <returns></returns>
		Function consultarDesgloseTareas(ByVal idOrigen As Integer, ByVal de As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listDesglose = From distriDoc In BBDD.DISTRIBUCION_de_DOCUMENTACION _
				  Join origenDoc In BBDD.ORIGEN_de_DOCUMENTACION On distriDoc.ID_ORIGEN Equals origenDoc.ID_ORIGEN _
				  Where distriDoc.ID_ORIGEN = CType(IIf(idOrigen > 0, idOrigen, CInt(distriDoc.ID_ORIGEN)), Nullable(Of Integer)) And _
				  distriDoc.DE_ = CStr(IIf(de <> String.Empty, de, distriDoc.DE_)) _
				  Order By origenDoc.ORIGEN, distriDoc.DE_ _
				  Select distriDoc
				Return listDesglose
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Devuelve el listado de distribucion de documentos para rellenar los De
		''' </summary>
		''' <param name="idOrigen">Origen</param>
		''' <returns></returns>
		Function consultarListadoDistribucionDocDE(ByVal idOrigen As Integer)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listDistribucion = From distriDoc In BBDD.DISTRIBUCION_de_DOCUMENTACION _
				  Where distriDoc.ID_ORIGEN = CType(IIf(idOrigen > 0, idOrigen, CInt(distriDoc.ID_ORIGEN)), Nullable(Of Integer)) _
				  Order By distriDoc.DE_ _
				  Group By MyDE = distriDoc.DE_, MyIdOrigen = distriDoc.ID_ORIGEN Into Group _
				  Where MyIdOrigen = idOrigen _
				  Select New With {.DE_ = MyDE}

				Return listDistribucion
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la distribucion del documento
		''' </summary>
		''' <param name="oDistrDoc">Distribucion del documento</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarDistribucionDocumento(ByVal oDistrDoc As KaPlanLib.Registro.DISTRIBUCION_de_DOCUMENTACION) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oDistrDoc.ID_REGISTRO = 0) Then	'insertar
					BBDD.DISTRIBUCION_de_DOCUMENTACION.InsertOnSubmit(oDistrDoc)
				Else 'actualizar
					Dim distrDoc As KaPlanLib.Registro.DISTRIBUCION_de_DOCUMENTACION = (From DistribucionDoc In BBDD.DISTRIBUCION_de_DOCUMENTACION _
					   Where DistribucionDoc.ID_REGISTRO = oDistrDoc.ID_REGISTRO _
					   Select DistribucionDoc).FirstOrDefault

					If (distrDoc IsNot Nothing) Then
						distrDoc.CODIGO = oDistrDoc.CODIGO
						distrDoc.COPIA_Nº = oDistrDoc.COPIA_Nº
						distrDoc.DE_ = oDistrDoc.DE_
						distrDoc.EDICION = oDistrDoc.EDICION
						distrDoc.ID_ORIGEN = oDistrDoc.ID_ORIGEN
						distrDoc.TITULO = oDistrDoc.TITULO
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function


#End Region

#Region "DEPARTAMENTO (DE)"

		''' <summary>
		''' Devuelve todos departamentos
		''' </summary>
		''' <returns></returns>
		Function consultarListadoDepartamento()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listDe = From De In BBDD.DE _
				Select De

				Return listDe
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el departamento
		''' </summary>
		''' <param name="oDepart">Departamento</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarDepartamento(ByVal oDepart As KaPlanLib.Registro.DE) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oDepart.ID_REGISTRO = 0) Then 'insertar
					BBDD.DE.InsertOnSubmit(oDepart)
				Else 'actualizar
					Dim departDoc As KaPlanLib.Registro.DE = (From Depart In BBDD.DE _
					   Where Depart.ID_REGISTRO = oDepart.ID_REGISTRO _
					   Select Depart).FirstOrDefault

					If (departDoc IsNot Nothing) Then
						departDoc.DE = oDepart.DE
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "ORIGEN_DOCUMENTACION"

		''' <summary>
		''' Devuelve todos los registros de la tabla ORIGEN_DOCUMENTACION
		''' </summary>
		''' <returns></returns>
		Function consultarListadoOrigenDocumentacion()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listOrigen = From origenDoc In BBDD.ORIGEN_de_DOCUMENTACION _
				Select origenDoc

				Return listOrigen
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el origen del documento
		''' </summary>
		''' <param name="oOrigenDoc">Origen del documento</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarOrigenDocumentacion(ByVal oOrigenDoc As KaPlanLib.Registro.ORIGEN_de_DOCUMENTACION) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oOrigenDoc.ID_ORIGEN = 0) Then 'insertar
					BBDD.ORIGEN_de_DOCUMENTACION.InsertOnSubmit(oOrigenDoc)
				Else 'actualizar
					Dim origenDoc As KaPlanLib.Registro.ORIGEN_de_DOCUMENTACION = (From OrigenDeDocumentacion In BBDD.ORIGEN_de_DOCUMENTACION _
					   Where OrigenDeDocumentacion.ID_ORIGEN = oOrigenDoc.ID_ORIGEN _
					   Select OrigenDeDocumentacion).FirstOrDefault

					If (origenDoc IsNot Nothing) Then
						origenDoc.ORIGEN = origenDoc.ORIGEN
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "DESTINO_DOCUMENTACION"

		''' <summary>
		''' Guarda el destino del documento
		''' </summary>
		''' <param name="oDestinoDoc">Destino del documento</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarDestinoDocumentacion(ByVal oDestinoDoc As KaPlanLib.Registro.DESTINO_de_DOCUMENTACION) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oDestinoDoc.ID_REGISTRO = 0) Then 'insertar
					BBDD.DESTINO_de_DOCUMENTACION.InsertOnSubmit(oDestinoDoc)
				Else 'actualizar
					Dim destinoDoc As KaPlanLib.Registro.DESTINO_de_DOCUMENTACION = (From DestinoDeDocumentacion In BBDD.DESTINO_de_DOCUMENTACION _
					   Where DestinoDeDocumentacion.ID_REGISTRO = oDestinoDoc.ID_REGISTRO _
					   Select DestinoDeDocumentacion).FirstOrDefault

					If (destinoDoc IsNot Nothing) Then
						destinoDoc.DESTINO = oDestinoDoc.DESTINO
						destinoDoc.ID_ORIGEN = oDestinoDoc.ID_ORIGEN
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function


#End Region

#Region "OPERACIONES_TIPO"
		''' <summary>
		''' Devuelve la informacion del codigo de operacion
		''' </summary>
		''' <param name="codOpe">Codigo de operacion</param>
		''' <returns></returns>
		Function consultarCodigoOperacion(ByVal codOpe As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim regCodigo = From opTipo In BBDD.OPERACIONES_TIPO _
				 Where opTipo.COD_OPERACION.Trim = codOpe.Trim _
				 Select opTipo

				If (Not regCodigo.Any) Then
					Return Nothing
				Else
					Return regCodigo.First
				End If

			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Devuelve los codigos de operacion
		''' </summary>
		''' <param name="ref">Referencia</param>
		''' <param name="codOpe">Codigo de operacion</param>
		''' <returns></returns>
		Function consultarCodigoOperacion(ByVal ref As String, ByVal codOpe As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				'-----------------------------------------------------------------------------------------------------------------
				'Unificamos el metodo de busqueda para las operaciones por que no da el mismo resultado 
				'si se usa el selector de Operaciones o si se busca una operacion directamente en la caja de texto.
				'-----------------------------------------------------------------------------------------------------------------
				Dim lObjetos As Object = consultarListadoCodigosOperacion(ref)
				Dim OperacionTipo As Registro.OPERACIONES_TIPO = Nothing

				If lObjetos Is Nothing AndAlso lObjetos.Count = 0 Then
					Throw New ApplicationException("""" & ref & """ sin operaciones asignadas o """ & codOpe & """ no tiene maquina asignada.")
				Else
					OperacionTipo = (From ot As Registro.OPERACIONES_TIPO In BBDD.OPERACIONES_TIPO Where ot.COD_OPERACION.Trim = codOpe.Trim Select ot).FirstOrDefault
					If OperacionTipo Is Nothing Then Throw New ApplicationException(codOpe & " - NO existente.")

					'--------------------------------------------------------------------------------------------------------------------------------
					'Nos recorremos los objetos de tipo anonimo para poder obtener el "COD_OPERACION" y la "OPERACIONES_TIPO" que le corresponde.
					'--------------------------------------------------------------------------------------------------------------------------------
					Dim lOperaciones As New List(Of Registro.OPERACIONES_TIPO)
					For Each obj As Object In lObjetos
						Dim OBJ_Type As Type = obj.GetType
						Dim ValorPropiedad As String = ""
						Dim Propiedades As List(Of System.Reflection.PropertyInfo) = OBJ_Type.GetProperties.ToList
						Dim COD_OPERACION As System.Reflection.PropertyInfo = Propiedades.Find(Function(pi As System.Reflection.PropertyInfo) String.Compare(pi.Name, "COD_OPERACION", true) = 0)

						If COD_OPERACION.GetValue(obj, Nothing) IsNot Nothing Then
							ValorPropiedad = COD_OPERACION.GetValue(obj, Nothing).ToString.Trim
							'------------------------------------------------------------------------------------------
							'Dim OperacionTipoB As Registro.OPERACIONES_TIPO = consultarCodigoOperacion(ValorPropiedad)
							'------------------------------------------------------------------------------------------
							'FROGA: 2013-05-21: Si no da error dejar esto.
							'------------------------------------------------------------------------------------------
							Dim OperacionTipoB As Registro.OPERACIONES_TIPO = _
								(From Opt As Registro.OPERACIONES_TIPO In BBDD.OPERACIONES_TIPO Where Opt.COD_OPERACION = ValorPropiedad Select Opt).SingleOrDefault
							'------------------------------------------------------------------------------------------
							If OperacionTipoB IsNot Nothing Then lOperaciones.Add(OperacionTipoB)					
						End If
					Next

					OperacionTipo = (From ot As Registro.OPERACIONES_TIPO In lOperaciones Where ot.COD_OPERACION.Trim = codOpe.Trim Select ot).FirstOrDefault
					If OperacionTipo Is Nothing Then Throw New ApplicationException(codOpe & " - NO tiene maquina asignada.")
					'--------------------------------------------------------------------------------------------------------------------------------
				End If

				Return OperacionTipo
				'-----------------------------------------------------------------------------------------------------------------
			Catch ex As ApplicationException
				Throw 
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Function

		''' <summary>
		''' Devuelve los codigos de operacion que estan relacionados con alguna máquina.
		''' </summary>
		''' <param name="ref">Referencia</param>
		''' <returns></returns>
		Function consultarListadoCodigosOperacion(ByVal ref As String, Optional ByVal SortExpresion As String = "NUM_OPERACION", Optional ByVal SortDirection As System.Web.UI.WebControls.SortDirection = Web.UI.WebControls.SortDirection.Ascending, Optional ByRef Conexion As String = Nothing)
			Try
				Dim BBDD As KaPlanLib.DAL.ELL = If(Conexion Is Nothing, New KaPlanLib.DAL.ELL, New KaPlanLib.DAL.ELL(Conexion))
				'--------------------------------------------------------------------------------------------------------------------------
				'Las operaciones deben estar relacionadas con alguna maquina. Si no estan relacionadas con la maquina no aparecen en el listado.
				'--------------------------------------------------------------------------------------------------------------------------
				Dim listCodigos = (From opTipo In BBDD.OPERACIONES_TIPO _
				  Join opArticulo In BBDD.OPERACIONES_DE_UN_ARTICULO On opTipo.COD_OPERACION Equals opArticulo.COD_OPERACION _
				  Join primUltMaq In BBDD.PRIMERA_Y_ULTIMA_MAQUINA On opArticulo.COD_OPERACION Equals primUltMaq.COD_OPERACION _
				  Where opArticulo.CODIGO.Trim = CType(IIf(ref <> String.Empty, ref.Trim, opArticulo.CODIGO.Trim), String) _
				  Order By opArticulo.NUM_OPERACION _
				  Group By COD_OPERACION = opTipo.COD_OPERACION, OPERACION_GENERAL = opTipo.OPERACION_GENERAL, OPERACION_TIPO = opTipo.OPERACION_TIPO, IDSECCION = opTipo.ID_SECCION, NUM_OP = opArticulo.NUM_OPERACION, PRIMAQ = primUltMaq.PrimeroDeTIPO_SOL_MAQUINA, AUDITORIA = opTipo.AUDITORIA Into Group _
				  Select COD_OPERACION, OPERACION_GENERAL, OPERACION_TIPO, IDSECCION, NUM_OP, PRIMAQ, AUDITORIA).ToList
				Return listCodigos
				'--------------------------------------------------------------------------------------------------------------------------
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function
#End Region

#Region "M_FRECUENCIA_CONTROL"

		''' <summary>
		''' Devuelve todos las frecuencias de control
		''' </summary>
		''' <returns></returns>
		Function consultarListadoFreControl()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listFrec = From frec In BBDD.M_FRECUENCIA_CONTROL _
				   Order By frec.FRECUENCIA_CONTROL _
				Select frec

				Return listFrec
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

#End Region

#Region "MAQUINAS"

		''' <summary>
		''' Consulta la maquina de numero de maquina la indicada
		''' </summary>
		''' <returns></returns>
		Function consultarMaquina(ByVal numMaquina As String) As Registro.MAQUINAS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim RegMaq = From Maquinas In BBDD.MAQUINAS _
				Where Maquinas.Nº_MAQUINA = numMaquina _
				Select Maquinas

				If (Not RegMaq.Any) Then
					Return Nothing
				Else
					Return RegMaq.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Consulta el listado de maquinas y sus secciones asociadas
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarMaquinasSeccion(ByVal idSeccion As Integer)
			Dim BBDD As New KaPlanLib.DAL.ELL

			Dim listMaquinas = From secc In BBDD.M_SECCION _
			  Join celul In BBDD.CELULAS On secc.ID_SECCION Equals celul.ID_SECCION _
			  Join maq In BBDD.MAQUINAS On maq.ID_CELULA Equals celul.ID_CELULA _
			  Where secc.ID_SECCION = idSeccion _
			  Select maq

			Return listMaquinas
		End Function

#End Region

#Region "CARACTERISTICAS DEL PLAN"
		''' <summary>
		''' Consulta la caraceristica del plan especificada
		''' </summary>
		''' <returns></returns>
		Function consultarCaracteristicaPlan(ByVal idRegistro As Integer) As Registro.CARACTERISTICAS_DEL_PLAN
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim RegCaract = From Caracteristicas In BBDD.CARACTERISTICAS_DEL_PLAN _
				Where Caracteristicas.ID_REGISTRO = idRegistro _
				Select Caracteristicas

				If (Not RegCaract.Any) Then
					Return Nothing
				Else
					Return RegCaract.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Obtiene las caracteristicas de un plan
		''' </summary>
		''' <param name="cod">Codigo de operacion</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarListadoCaracteristicasPlan(ByVal cod As String, Optional BBDD As KaPlanLib.DAL.ELL = Nothing)
			Try
				'-----------------------------------------------------------------
				'Dim BBDD As New KaPlanLib.DAL.ELL
				'Dim listCaract = From caract In BBDD.CARACTERISTICAS_DEL_PLAN _
				'  Where caract.CODIGO = cod And caract.PROCEDE_DE <> "HOJA REGISTRO" _
				'  Order By caract.ORDEN_CARAC _
				'  Select caract
				'-----------------------------------------------------------------
				'FROGA:2013-01-09: Se deja que se vean las caracteristicas 
				'que se crearon desde el PAC en la "Hoja de Instrucciones".
				'-----------------------------------------------------------------
				If BBDD Is Nothing Then BBDD = New KaPlanLib.DAL.ELL
				Dim listCaract = From caract In BBDD.CARACTERISTICAS_DEL_PLAN _
					Where caract.CODIGO = cod Order By caract.ORDEN_CARAC Select caract
				'-----------------------------------------------------------------
				Return listCaract
			Catch ex As Exception
				'Throw New Exception("error", ex)
				log.Error(ex)
				Throw 
			End Try
		End Function

		''' <summary>
		''' Guarda la caracteristica del plan de control
		''' </summary>
		''' <param name="oCaract">Caracteristica del plan</param>
		''' <param name="BBDD">Contexto de la conexion (KaPlanLib.DAL.ELL)</param>
		''' <param name="bEjecutarOtrasActualizacion">Realiza la actualizacion del AMFE Operacion ('CARACTERISTICAS_AMFE' y 'CAUSAS_DE_FALLO').</param>
		''' <remarks></remarks>
		Public Sub GuardarCaracteristicaPlanControl(ByVal oCaract As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN, ByVal BBDD As KaPlanLib.DAL.ELL, Optional ByVal bEjecutarOtrasActualizacion As Boolean = False)
            Try
                If (oCaract.ID_REGISTRO = 0) Then 'insertar
                    BBDD.CARACTERISTICAS_DEL_PLAN.InsertOnSubmit(oCaract)
                End If
                Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
                BBDD.SubmitChanges()

                Dim idRegistro As Integer = 0
                If (oCaract.ID_REGISTRO = 0) Then 'se obtiene el ultimo insertado
                    idRegistro = (From caract In BBDD.CARACTERISTICAS_DEL_PLAN Select caract.ID_REGISTRO).Max
                Else 'se obtiene el id_Registro del objeto
                    idRegistro = oCaract.ID_REGISTRO
                End If
                If (bEjecutarOtrasActualizacion) Then OtrasActualizacionesCaracteristicasPlan(idRegistro)

            Catch ex As ApplicationException
                Throw
            Catch ex As Exception
                Dim msg As String = String.Format("CARACTERISTICAS_DEL_PLAN.ID_REGISTRO: {0}" & vbCrLf & "CARACTERISTICAS_DEL_PLAN.CODIGO: '{1}'", oCaract.ID_REGISTRO, oCaract.CODIGO)
                log.Error(msg, ex)
                Throw
            End Try
        End Sub
		'------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------

		''' <summary>
		''' Realiza otras consultar de las caracteristicas del plan
		''' </summary>
		''' <param name="idRegistroCaract"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function OtrasActualizacionesCaracteristicasPlan(ByVal idRegistroCaract As Integer) As Boolean
			Dim transaction As DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				Dim caractPlan As Registro.CARACTERISTICAS_DEL_PLAN = consultarCaracteristicaPlan(idRegistroCaract)
				If (caractPlan Is Nothing) Then Return False

				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				'Se actualiza la tabla caracteristicas Amfe
				If (caractPlan.ID_CARACTERISTICA IsNot Nothing AndAlso caractPlan.ID_CARACTERISTICA.Value > 0) Then
					Dim caracAmfe As KaPlanLib.Registro.CARACTERISTICAS_AMFE = (From carac In BBDD.CARACTERISTICAS_AMFE _
					  Where carac.ID_CARACTERISTICA = caractPlan.ID_CARACTERISTICA _
					  Select carac).FirstOrDefault

					If (caracAmfe IsNot Nothing) Then
						caracAmfe.CARACTERISTICA = caractPlan.CARAC_PARAM
						caracAmfe.ESPECIFICACION = caractPlan.ESPECIFICACION
						caracAmfe.CLASE = caractPlan.CLASE
						caracAmfe.ORDEN = caractPlan.ORDEN_CARAC
					End If
				End If

				If (caractPlan.CONT_CAUSA IsNot Nothing AndAlso caractPlan.CONT_CAUSA.Value > 0) Then
					Dim caracAmfe2 = From causaFallo In BBDD.CAUSAS_DE_FALLO Join modoFallo In BBDD.MODOS_DE_FALLO On modoFallo.ID_MODO Equals causaFallo.ID_MODO
									 Join carac In BBDD.CARACTERISTICAS_AMFE On carac.ID_CARACTERISTICA Equals modoFallo.ID_CARACTERISTICA
									 Where causaFallo.CONT_CAUSA = caractPlan.CONT_CAUSA
									 Select carac

					For Each item As Registro.CARACTERISTICAS_AMFE In caracAmfe2
						item.CARACTERISTICA = caractPlan.CARAC_PARAM
						item.ESPECIFICACION = caractPlan.ESPECIFICACION
						item.CLASE = caractPlan.CLASE
					Next
				End If

				'Se actualiza la tabla causas fallo
				If (caractPlan.CONT_CAUSA IsNot Nothing AndAlso caractPlan.CONT_CAUSA.Value > 0) Then
					Dim causaFallo As KaPlanLib.Registro.CAUSAS_DE_FALLO = (From causa In BBDD.CAUSAS_DE_FALLO
																			Where causa.CONT_CAUSA = caractPlan.CONT_CAUSA
																			Select causa).FirstOrDefault
					If (causaFallo IsNot Nothing) Then
						causaFallo.FRECUENCIA_DE_CONTROL = caractPlan.FRECUENCIA_CONTROL
						causaFallo.FRECUENCIA_DE_CONTROL_CAL = caractPlan.FRECUENCIA_CONTROL_CAL
						causaFallo.CONTROLES = caractPlan.MEDIO_DENOMINACION

						causaFallo.NOMBRE_MEDIO_FAB = caractPlan.Control_DET_ME_FAB
						causaFallo.NOMBRE_MEDIO = caractPlan.Control_DET_ME_CTRL

						causaFallo.Codigo_Control_ME_FAB = caractPlan.Codigo_Control_ME_FAB

						causaFallo.Tamaño_OP = caractPlan.TAMAÑO
						causaFallo.Tamaño_CAL = caractPlan.TAMAÑO_CAL

						causaFallo.METODO_CONTROL = caractPlan.METODO_CONTROL
						causaFallo.METODO_CONTROL_FAB = caractPlan.METODO_CONTROL_FAB
					End If
				End If
				Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
				BBDD.SubmitChanges()

				transaction.Commit()
				Return True
			Catch ex As Exception
				transaction.Rollback()
				Return False
			Finally
				If (BBDD.Connection.State <> ConnectionState.Closed) Then BBDD.Connection.Close()
			End Try
		End Function

        ' ''' <summary>
        ' ''' Elimina una caracteristica de plan de control
        ' ''' </summary>
        ' ''' <param name="idRegistro">Identificador de la caracteristica</param>        
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        '<Obsolete("Usar la proceso 'EliminarCaracteristicaPlanControl2'")> _
        'Public Function EliminarCaracteristicaPlanControl(ByVal idRegistro As Integer) As Boolean
        '    Try
        '        Dim BBDD As New KaPlanLib.DAL.ELL

        '        Dim Caracteristica = From Caract In BBDD.CARACTERISTICAS_DEL_PLAN _
        '           Where Caract.ID_REGISTRO = idRegistro _
        '           Select Caract

        '        If (Caracteristica.Count = 1) Then
        '            BBDD.CARACTERISTICAS_DEL_PLAN.DeleteOnSubmit(Caracteristica.First)
        '            BBDD.SubmitChanges()
        '            Return True
        '        Else
        '            Return False
        '        End If
        '    Catch ex As Exception
        '        Return False
        '    End Try
        'End Function

        ''' <summary>
        ''' Elimina una caracteristica de plan de control
        ''' </summary>
        ''' <param name="idRegistro">Identificador de la caracteristica</param>        
        ''' <remarks></remarks>
        Public Sub EliminarCaracteristicaPlanControl(ByVal idRegistro As Integer)
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim Caracteristica As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN = _
                    (From Caract In BBDD.CARACTERISTICAS_DEL_PLAN Where Caract.ID_REGISTRO = idRegistro Select Caract).FirstOrDefault

                If Caracteristica Is Nothing Then
					Throw New ApplicationException(String.Format(ItzultzaileWeb.Itzuli("Caracteristica {0} no encontrada."), idRegistro))
                Else
                    BBDD.CARACTERISTICAS_DEL_PLAN.DeleteOnSubmit(Caracteristica)
                    Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
                    BBDD.SubmitChanges()
				End If
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error(ex)
				Throw
            End Try
        End Sub

        Sub EliminarCaracteristicasPlanControl(ByVal IdCaracteristica As Integer, Optional ByVal ContCausa As Nullable(Of Integer) = Nothing)
            Dim BBDD As New KaPlanLib.DAL.ELL
            Try
                Dim lCaracPlan As List(Of Registro.CARACTERISTICAS_DEL_PLAN) = _
                            (From CP As Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN _
                             Where CP.ID_CARACTERISTICA = IdCaracteristica _
                             And CP.CONT_CAUSA = If(ContCausa Is Nothing, CP.CONT_CAUSA, ContCausa) _
                             Select CP).ToList
                If lCaracPlan IsNot Nothing AndAlso lCaracPlan.Any Then
                    BBDD.CARACTERISTICAS_DEL_PLAN.DeleteAllOnSubmit(lCaracPlan)
                    BBDD.SubmitChanges()
                    ReorganizarOrden_CARACTERISTICAS_DEL_PLAN(lCaracPlan.FirstOrDefault.CODIGO, BBDD)
                End If
            Catch ex As Exception
                Throw 
            End Try
        End Sub

        ''' <summary>
        ''' Nos indica si el responsable de control es una maquina.
        ''' Si el campo “De.Med.Fab – Control de Deteccion (CAUSAS_DE_FALLO.NOMBRE_MEDIO_FAB / CARACTERISTICAS_DEL_PLAN.Control_DET_ME_FAB)” o “De.Med.Fab – Código de Control (CAUSAS_DE_FALLO.Codigo_Control_ME_FAB)" 
        ''' tiene datos el responsable es una maquina.
        ''' </summary>
        ''' <param name="CP">CARACTERISTICAS_DEL_PLAN</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Responsable_Maquina(CP As Registro.CARACTERISTICAS_DEL_PLAN) As Boolean
			'Responsable_Maquina = Not String.IsNullOrWhiteSpace(CP.Control_DET_ME_FAB)
			Responsable_Maquina = (Not String.IsNullOrWhiteSpace(CP.Control_DET_ME_FAB) Or Not String.IsNullOrWhiteSpace(CP.Codigo_Control_ME_FAB))
			Return Responsable_Maquina
		End Function

		''' <summary>
		''' Indica si el responsable de control es el operario.
		''' Si “Frecuencia Operación (CAUSAS_DE_FALLO.FRECUENCIA_DE_CONTROL / CARACTERISTICAS_DEL_PLAN.FRECUENCIA_CONTROL)” y “Tamaño Operación (CAUSAS_DE_FALLO.Tamaño_OP / CARACTERISTICAS_DEL_PLAN.TAMAÑO)”
		'''  tiene datos el responsable es un operario. 
		''' </summary>
		''' <param name="CP"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Responsable_Operario(CP As Registro.CARACTERISTICAS_DEL_PLAN) As Boolean
			Responsable_Operario = (Not String.IsNullOrWhiteSpace(CP.FRECUENCIA_CONTROL) And Not String.IsNullOrWhiteSpace(CP.TAMAÑO))
			Return Responsable_Operario
		End Function

		''' <summary>
		''' Indica si el responsable es "Calidad".
		''' Si “Frecuencia Calidad (CAUSAS_DE_FALLO.FRECUENCIA_DE_CONTROL_CAL / CARACTERISTICAS_DEL_PLAN. FRECUENCIA_CONTROL_CAL )” y “Tamaño Calidad (CAUSAS_DE_FALLO.Tamaño_CAL / CARACTERISTICAS_DEL_PLAN. TAMAÑO_CAL)” 
		''' tienen datos el responsable es "Calidad".
		''' </summary>
		''' <param name="CP"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function Responsable_Calidad(CP As Registro.CARACTERISTICAS_DEL_PLAN) As Boolean
			Responsable_Calidad = (Not String.IsNullOrWhiteSpace(CP.FRECUENCIA_CONTROL_CAL) And Not String.IsNullOrWhiteSpace(CP.TAMAÑO_CAL))
			Return Responsable_Calidad
		End Function

		''' <summary>
		''' Ordena los registro de forma consecutiva del primero al ultimo.
		''' </summary>
		''' <param name="CodOperacion"></param>
		''' <remarks></remarks>
		Sub ReorganizarOrden_CARACTERISTICAS_DEL_PLAN(ByVal CodOperacion As String, ByVal BBDD As KaPlanLib.DAL.ELL)
			Dim Orden As Integer = 0
			'Dim CodLetra As Integer = Asc("A".ToUpper) '65
			'Dim Posicion As String = String.Empty

			Try
				Dim lReg As IQueryable(Of Registro.CARACTERISTICAS_DEL_PLAN) = _
					From Reg As Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN _
					Where Reg.CODIGO = CodOperacion Select Reg Order By Reg.ORDEN_CARAC Ascending, Reg.ID_REGISTRO Descending

				If lReg.Any Then
					For Each Reg As Registro.CARACTERISTICAS_DEL_PLAN In lReg
						Orden += 1
						Reg.ORDEN_CARAC = Orden
						'-------------------------------------------------------
						'Ortuzar:2013-02-11: No cambiar la letra "Posicion" automaticamente.
						'-------------------------------------------------------
						'Calculamos la "Posicion".
						'-------------------------------------------------------
						'If Not String.IsNullOrWhiteSpace(Reg.POSICION) Then
						'	Reg.POSICION = Posicion & Chr(CodLetra)
						'	CodLetra += 1 'Calculamos la siguiente letra.
						'	If CodLetra > 90 Then
						'		Posicion &= Chr(90)	'Z
						'		CodLetra = 65 'A
						'	End If
						'End If
						'-------------------------------------------------------
					Next
					BBDD.SubmitChanges()
				End If
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub

		Sub ReorganizarOrden_CARACTERISTICAS_DEL_PLAN(ByVal CARACTERISTICAS_DEL_PLAN As Registro.CARACTERISTICAS_DEL_PLAN, ByRef OrdenOriginal As Integer)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim Orden As Integer
			Dim lReg As New List(Of Registro.CARACTERISTICAS_DEL_PLAN)

			If OrdenOriginal > CARACTERISTICAS_DEL_PLAN.ORDEN_CARAC Or OrdenOriginal = 0 Then	'Se produce al retrasar una posicion el orden o al crear uno nuevo.
				'--------------------------------------------------------------------------------------------------------------
				lReg = (From Reg As Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN _
					Where Reg.CODIGO = CARACTERISTICAS_DEL_PLAN.CODIGO And Reg.ORDEN_CARAC >= CARACTERISTICAS_DEL_PLAN.ORDEN_CARAC And Reg.ID_REGISTRO <> CARACTERISTICAS_DEL_PLAN.ID_REGISTRO _
					Select Reg Order By Reg.ORDEN_CARAC Ascending, Reg.ID_REGISTRO Descending).ToList
				If lReg IsNot Nothing AndAlso lReg.Any Then
					Orden = CARACTERISTICAS_DEL_PLAN.ORDEN_CARAC 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
					For Each Reg As Registro.CARACTERISTICAS_DEL_PLAN In lReg
						Orden += 1
						Reg.ORDEN_CARAC = Orden
					Next
					BBDD.SubmitChanges()
				End If
				'--------------------------------------------------------------------------------------------------------------
			ElseIf OrdenOriginal < CARACTERISTICAS_DEL_PLAN.ORDEN_CARAC Then 'Se produce al avanzar una posicion  el orden.
				'--------------------------------------------------------------------------------------------------------------
				lReg = (From Reg As Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN _
					Where Reg.CODIGO = CARACTERISTICAS_DEL_PLAN.CODIGO And Reg.ORDEN_CARAC <= CARACTERISTICAS_DEL_PLAN.ORDEN_CARAC And Reg.ID_REGISTRO <> CARACTERISTICAS_DEL_PLAN.ID_REGISTRO _
					Select Reg Order By Reg.ORDEN_CARAC Descending, Reg.ID_REGISTRO Ascending).ToList
				If lReg IsNot Nothing AndAlso lReg.Any Then
					Orden = CARACTERISTICAS_DEL_PLAN.ORDEN_CARAC 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
					For Each Reg As Registro.CARACTERISTICAS_DEL_PLAN In lReg
						Orden -= 1
						Reg.ORDEN_CARAC = Orden
					Next
					BBDD.SubmitChanges()
				End If
				'--------------------------------------------------------------------------------------------------------------
			End If
			ReorganizarOrden_CARACTERISTICAS_DEL_PLAN(CARACTERISTICAS_DEL_PLAN.CODIGO, BBDD)
		End Sub

		''' <summary>
		''' Proceso para copiar la 'Ayuda Visual' de las caracteristicas del 'Plan de Control Operacion' a 'Fabricacion'.
		''' </summary>
		''' <param name="Origen"></param>
		''' <param name="Destino"></param>
		''' <remarks></remarks>
		Sub AyudaVisualCopiar_FAB(Origen As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN, Destino As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION)
			Dim Archivos_Caracteristicas_Plan As KaPlanLib.Registro.Archivos_Caracteristicas_Plan = Origen.Archivos_Caracteristicas_Plan.FirstOrDefault
			Dim Archivos_Caracteristicas_Plan_FAB As New KaPlanLib.Registro.Archivos_Caracteristicas_Plan_FAB
            If Archivos_Caracteristicas_Plan IsNot Nothing AndAlso Archivos_Caracteristicas_Plan.Archivos IsNot Nothing Then
                Dim Archivos_FAB As New KaPlanLib.Registro.Archivos
                Archivos_FAB.Archivo = Archivos_Caracteristicas_Plan.Archivos.Archivo
                Archivos_FAB.Content_Type = Archivos_Caracteristicas_Plan.Archivos.Content_Type
                Archivos_FAB.Descripcion = Archivos_Caracteristicas_Plan.Archivos.Descripcion
                Archivos_FAB.Nombre = Archivos_Caracteristicas_Plan.Archivos.Nombre
                Archivos_FAB.Titulo = Archivos_Caracteristicas_Plan.Archivos.Titulo
                Archivos_FAB.FechaCreacion = Now

                Archivos_Caracteristicas_Plan_FAB.Archivos = Archivos_FAB
                Archivos_Caracteristicas_Plan_FAB.CARACTERISTICAS_DEL_PLAN_FABRICACION = Destino

                Destino.Archivos_Caracteristicas_Plan_FAB.Add(Archivos_Caracteristicas_Plan_FAB)
            Else
                Destino.Archivos_Caracteristicas_Plan_FAB.Clear()
			End If
		End Sub
		''' <summary>
		''' Proceso para copiar la 'Ayuda Visual' de las caracteristicas del 'Plan de Control Operacion'.
		''' </summary>
		''' <param name="Origen"></param>
		''' <param name="Destino"></param>
		''' <remarks></remarks>
		Sub AyudaVisualCopiar(Origen As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN, Destino As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN)
			Dim Archivos_Caracteristicas_Plan_Origen As KaPlanLib.Registro.Archivos_Caracteristicas_Plan = Origen.Archivos_Caracteristicas_Plan.FirstOrDefault
			Dim Archivos_Caracteristicas_Plan_Destino As New KaPlanLib.Registro.Archivos_Caracteristicas_Plan
			If Archivos_Caracteristicas_Plan_Origen IsNot Nothing Then
				Dim Archivos As New KaPlanLib.Registro.Archivos
				Archivos.Archivo = Archivos_Caracteristicas_Plan_Origen.Archivos.Archivo
				Archivos.Content_Type = Archivos_Caracteristicas_Plan_Origen.Archivos.Content_Type
				Archivos.Descripcion = Archivos_Caracteristicas_Plan_Origen.Archivos.Descripcion
				Archivos.Nombre = Archivos_Caracteristicas_Plan_Origen.Archivos.Nombre
				Archivos.Titulo = Archivos_Caracteristicas_Plan_Origen.Archivos.Titulo
				Archivos.FechaCreacion = Now

				Archivos_Caracteristicas_Plan_Destino.Archivos = Archivos
				Archivos_Caracteristicas_Plan_Destino.CARACTERISTICAS_DEL_PLAN = Destino

				Destino.Archivos_Caracteristicas_Plan.Add(Archivos_Caracteristicas_Plan_Destino)
			Else
				Destino.Archivos_Caracteristicas_Plan.Clear()
			End If
		End Sub
#End Region

#Region "CARACTERISTICAS AMFE"
		''' <summary>
		''' Guarda la caracteristica del amfe
		''' </summary>
		''' <param name="oCaract">Caracteristica amfe</param>
		''' <remarks></remarks>
		Public Sub GuardarCaracteristicasAmfe(ByVal oCaract As KaPlanLib.Registro.CARACTERISTICAS_AMFE)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim caractAmfe As New KaPlanLib.Registro.CARACTERISTICAS_AMFE
			Dim CaracPlan As New KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN
			Dim OrdenOriginal As Integer = Nothing
			Dim OrdenOriginal_PLAN As Integer = Nothing

			Try
				'------------------------------------------------------------------------------------------------
				BBDD.Connection.Open()
				BBDD.Transaction = BBDD.Connection.BeginTransaction
				'------------------------------------------------------------------------------------------------

				'------------------------------------------------------------------------------------------------
				If (oCaract.ID_CARACTERISTICA = 0) Then 'insertar
					BBDD.CARACTERISTICAS_AMFE.InsertOnSubmit(oCaract)
					caractAmfe = oCaract
				Else 'actualizar
					caractAmfe = (From caracteristica In BBDD.CARACTERISTICAS_AMFE
								  Where caracteristica.ID_CARACTERISTICA = oCaract.ID_CARACTERISTICA
								  Select caracteristica).FirstOrDefault

					If (caractAmfe IsNot Nothing) Then
						OrdenOriginal = caractAmfe.ORDEN
						caractAmfe.ID_AMFE = oCaract.ID_AMFE
						caractAmfe.CARACTERISTICA = oCaract.CARACTERISTICA
						caractAmfe.ESPECIFICACION = oCaract.ESPECIFICACION
						caractAmfe.ORDEN = oCaract.ORDEN
						caractAmfe.CLASE = oCaract.CLASE
					Else
						'Return False
						Throw New ApplicationException("Caracteristica NO encontrada", New ApplicationException)
					End If
				End If

				Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
				BBDD.SubmitChanges()

				'------------------------------------------------------------------------------------------------

				'------------------------------------------------------------------------------------------------
				CaracPlan = (From Caracteristica In BBDD.CARACTERISTICAS_DEL_PLAN Where Caracteristica.ID_CARACTERISTICA = oCaract.ID_CARACTERISTICA Select Caracteristica).FirstOrDefault
				If CaracPlan IsNot Nothing Then
					OrdenOriginal_PLAN = CaracPlan.ORDEN_CARAC
					CaracPlan.CARAC_PARAM = oCaract.CARACTERISTICA
					CaracPlan.ESPECIFICACION = oCaract.ESPECIFICACION
					CaracPlan.CLASE = oCaract.CLASE
					'Evitamos que el orden de la caracteristica de "Plan de Control Operacion" se modifique conservando su valor de ordenacion.
					If CaracPlan.ORDEN_CARAC Is Nothing OrElse CaracPlan.ORDEN_CARAC <= 0 Then CaracPlan.ORDEN_CARAC = oCaract.ORDEN
					BBDD.SubmitChanges()
					ReorganizarOrden_CARACTERISTICAS_DEL_PLAN(CaracPlan.CODIGO, BBDD)
				End If
				'------------------------------------------------------------------------------------------------				

				BBDD.Transaction.Commit()

			Catch ex As ApplicationException
				BBDD.Transaction.Rollback()
				Throw
			Catch ex As Exception
				BBDD.Transaction.Rollback()
				Dim msg As String = String.Format(vbCrLf & StrDup(90, "=") _
												  & "CARACTERISTICAS_AMFE: {0}" & vbCrLf & "CARACTERISTICAS_AMFE.ID_CARACTERISTICA: {1}" _
												  & vbCrLf & StrDup(90, "="), (oCaract IsNot Nothing), If(oCaract Is Nothing, String.Empty, oCaract.ID_CARACTERISTICA))
				log.Error(msg, ex)
				Throw
			Finally
				BBDD.Transaction.Dispose()
				BBDD.Connection.Close()
				BBDD.Dispose()
			End Try

			Try
				ReorganizarOrden_CARACTERISTICAS_AMFE(caractAmfe, OrdenOriginal)
			Catch ex As Exception
				Throw
			End Try
		End Sub

		''' <summary>
		''' Elimina una caracteristica amfe
		''' </summary>
		''' <param name="idCaracteristica">Identificador de la caracteristica</param>        
		''' <remarks></remarks>
		Sub EliminarCaracteristicaAMFE(ByVal idCaracteristica As Integer)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim lCaracteristica_Plan As New List(Of Registro.CARACTERISTICAS_DEL_PLAN)
				Dim IdAMFE As Integer
				Dim Caracteristica_AMFE As Registro.CARACTERISTICAS_AMFE =
					(From Caract In BBDD.CARACTERISTICAS_AMFE Where Caract.ID_CARACTERISTICA = idCaracteristica Select Caract).SingleOrDefault

				If Caracteristica_AMFE Is Nothing Then
					Throw New ApplicationException("No se encontrado la Caracteristica a eliminar")
				Else
					IdAMFE = Caracteristica_AMFE.ID_AMFE
					EliminarCaracteristicasPlanControl(Caracteristica_AMFE.ID_CARACTERISTICA)
					BBDD.CARACTERISTICAS_AMFE.DeleteOnSubmit(Caracteristica_AMFE)
					Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
					BBDD.SubmitChanges()

					ReorganizarOrden_CARACTERISTICAS_AMFE(IdAMFE, BBDD)
				End If
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub

		''' <summary>
		''' Ordena los registro de forma consecutiva del primero al ultimo.
		''' </summary>
		''' <param name="IdAmfe"></param>
		''' <remarks></remarks>
		Sub ReorganizarOrden_CARACTERISTICAS_AMFE(ByVal IdAmfe As Integer, BBDD As KaPlanLib.DAL.ELL)
			Dim Orden As Integer = 0
			Dim lCA As List(Of Registro.CARACTERISTICAS_AMFE) =
			 (From CA As Registro.CARACTERISTICAS_AMFE In BBDD.CARACTERISTICAS_AMFE Where CA.ID_AMFE = IdAmfe Select CA Order By CA.ORDEN Ascending, CA.ID_CARACTERISTICA Descending).ToList

			If lCA IsNot Nothing AndAlso lCA.Any Then
				For Each Caracteristica As Registro.CARACTERISTICAS_AMFE In lCA
					Orden += 1
					Caracteristica.ORDEN = Orden
				Next
				BBDD.SubmitChanges()
			End If
		End Sub

		''' <summary>
		''' Ordenacion de Caracteristicas
		''' </summary>
		''' <param name="CARACTERISTICAS_AMFE">Caracteristica que se va a reordenar.</param>
		''' <param name="OrdenOriginal">Posicion original que ocupaba la caracteristica.</param>
		''' <remarks></remarks>
		Sub ReorganizarOrden_CARACTERISTICAS_AMFE(ByVal CARACTERISTICAS_AMFE As Registro.CARACTERISTICAS_AMFE, ByRef OrdenOriginal As Integer)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim Orden As Integer

			If OrdenOriginal > CARACTERISTICAS_AMFE.ORDEN Or OrdenOriginal = 0 Then 'Se produce al retrasar una posicion el orden o al crear uno nuevo.
				'--------------------------------------------------------------------------------------------------------------
				Dim lCA As List(Of Registro.CARACTERISTICAS_AMFE) =
					(From CA As Registro.CARACTERISTICAS_AMFE In BBDD.CARACTERISTICAS_AMFE
					 Where CA.ID_AMFE = CARACTERISTICAS_AMFE.ID_AMFE And CA.ORDEN >= CARACTERISTICAS_AMFE.ORDEN And CA.ID_CARACTERISTICA <> CARACTERISTICAS_AMFE.ID_CARACTERISTICA
					 Select CA Order By CA.ORDEN Ascending, CA.ID_CARACTERISTICA Descending).ToList
				If lCA IsNot Nothing AndAlso lCA.Any Then
					Orden = CARACTERISTICAS_AMFE.ORDEN 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
					For Each Caracteristica As Registro.CARACTERISTICAS_AMFE In lCA
						Orden += 1
						Caracteristica.ORDEN = Orden
					Next
					BBDD.SubmitChanges()
				End If
				'--------------------------------------------------------------------------------------------------------------
			ElseIf OrdenOriginal < CARACTERISTICAS_AMFE.ORDEN Then 'Se produce al avanzar una posicion  el orden.
				'--------------------------------------------------------------------------------------------------------------
				Dim lCA As List(Of Registro.CARACTERISTICAS_AMFE) =
					(From CA As Registro.CARACTERISTICAS_AMFE In BBDD.CARACTERISTICAS_AMFE
					 Where CA.ID_AMFE = CARACTERISTICAS_AMFE.ID_AMFE And CA.ORDEN <= CARACTERISTICAS_AMFE.ORDEN And CA.ID_CARACTERISTICA <> CARACTERISTICAS_AMFE.ID_CARACTERISTICA
					 Select CA Order By CA.ORDEN Descending, CA.ID_CARACTERISTICA Ascending).ToList
				If lCA IsNot Nothing AndAlso lCA.Any Then
					Orden = CARACTERISTICAS_AMFE.ORDEN 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
					For Each Caracteristica As Registro.CARACTERISTICAS_AMFE In lCA
						Orden -= 1
						Caracteristica.ORDEN = Orden
					Next
					BBDD.SubmitChanges()
				End If
				'--------------------------------------------------------------------------------------------------------------
			End If
			ReorganizarOrden_CARACTERISTICAS_AMFE(CARACTERISTICAS_AMFE.ID_AMFE, BBDD)
		End Sub
#End Region

#Region "AMFES PIEZAS"
		''' <summary>
		''' Devuelve un registro de Amfes pieza
		''' </summary>
		''' <param name="cod">Referencia de operacion del registro a buscar</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarAmfePieza(ByVal cod As String) As KaPlanLib.Registro.AMFES_PIEZA
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim RegAmfes = From AmfesPieza In BBDD.AMFES_PIEZA
							   Where AmfesPieza.CODIGO = cod
							   Select AmfesPieza

				If (Not RegAmfes.Any) Then
					Return Nothing
				Else
					Return RegAmfes.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el Amfe pieza
		''' </summary>
		''' <param name="oAmfePieza">Amfe pieza</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarAmfePieza(ByVal oAmfePieza As KaPlanLib.Registro.AMFES_PIEZA) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oAmfePieza.ID_AMFE_PIEZA = 0) Then 'insertar
					BBDD.AMFES_PIEZA.InsertOnSubmit(oAmfePieza)
				Else 'actualizar
					Dim AmfePieza As KaPlanLib.Registro.AMFES_PIEZA = (From AmfeP In BBDD.AMFES_PIEZA
																	   Where AmfeP.ID_AMFE_PIEZA = oAmfePieza.ID_AMFE_PIEZA
																	   Select AmfeP).FirstOrDefault

					If (AmfePieza IsNot Nothing) Then
						AmfePieza.FECHA_AMFE = CType(IIf(oAmfePieza.FECHA_AMFE Is Nothing, New Nullable(Of Date), CDate(oAmfePieza.FECHA_AMFE)), Nullable(Of Date))
						AmfePieza.NIVEL_AMFE = oAmfePieza.NIVEL_AMFE
						AmfePieza.PREPARADO_POR = oAmfePieza.PREPARADO_POR
						AmfePieza.CLASE = oAmfePieza.CLASE
						AmfePieza.CODIGO = oAmfePieza.CODIGO
						AmfePieza.DISENO = oAmfePieza.DISENO
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		'''' <summary>
		'''' Elimina una pieza Amfe
		'''' </summary>
		'''' <param name="cod">Codigo</param>        
		'''' <returns></returns>
		'''' <remarks></remarks>
		'Public Function EliminarAmfePieza(ByVal cod As String) As Boolean
		'    Dim BBDD As New KaPlanLib.DAL.ELL
		'    Try
		'        Dim AmfePieza = From AmfePiez In BBDD.AMFES_PIEZA
		'                        Where AmfePiez.CODIGO = cod
		'                        Select AmfePiez

		'        If (AmfePieza.Count = 1) Then
		'            BBDD.AMFES_PIEZA.DeleteOnSubmit(AmfePieza.First)
		'            BBDD.SubmitChanges()
		'            Return True
		'        End If
		'        Return False
		'    Catch ex As Exception
		'        Return False
		'    End Try
		'End Function

		''' <summary>
		''' Elimina una pieza Amfe
		''' </summary>
		''' <param name="cod">Codigo</param>
		Sub EliminarAmfePieza(ByVal cod As String)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				Dim lAmfePieza = From AmfePiez In BBDD.AMFES_PIEZA Where AmfePiez.CODIGO = cod Select AmfePiez
				If lAmfePieza.Any Then
					BBDD.AMFES_PIEZA.DeleteAllOnSubmit(lAmfePieza)
					BBDD.SubmitChanges()
				End If
			Catch ex As Exception
				Throw
			End Try
		End Sub
#End Region

#Region "AMFES"
		''' <summary>
		''' Devuelve un registro de "Amfes".
		''' Si no encuentra el "AMFE" crea uno automaticamente.
		''' </summary>
		''' <param name="cod">Referencia de operacion del registro a buscar</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarAmfe(ByVal cod As String) As KaPlanLib.Registro.AMFES
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim RegAmfes = From Amfes In BBDD.AMFES
							   Where Amfes.CODIGO = cod
							   Select Amfes

				If (Not RegAmfes.Any) Then
					'---------------------------------------------------------------------------------------------------------------------------------
					'Return Nothing
					'---------------------------------------------------------------------------------------------------------------------------------
					'FROGA:2013-08-23: El AMFE es un elemento esencial para las consultas de los informes y el correcto funcionamiento de la aplicacion.
					'---------------------------------------------------------------------------------------------------------------------------------
					Dim Amfe As New Registro.AMFES
					Amfe.CODIGO = cod
					Amfe.NIVEL_AMFE = 1
					Amfe.FECHA_AMFE = Now
					GuardarAmfe(Amfe)
					Return Amfe
					'---------------------------------------------------------------------------------------------------------------------------------
				Else
					Return RegAmfes.First
				End If
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				Throw
			End Try
		End Function

		' ''' <summary>
		' ''' Guarda el Amfe
		' ''' </summary>
		' ''' <param name="oAmfe">Amfe</param>
		' ''' <returns></returns>
		' ''' <remarks></remarks>
		'Public Function GuardarAmfe(ByVal oAmfe As KaPlanLib.Registro.AMFES) As Boolean
		'	Dim BBDD As New KaPlanLib.DAL.ELL
		'	Try
		'		If (oAmfe.ID = 0) Then 'insertar
		'			BBDD.AMFES.InsertOnSubmit(oAmfe)
		'		Else 'actualizar
		'			Dim Amfe As KaPlanLib.Registro.AMFES = (From AmfeP In BBDD.AMFES _
		'			   Where AmfeP.ID = oAmfe.ID _
		'			   Select AmfeP).FirstOrDefault

		'			If (Amfe IsNot Nothing) Then
		'				Amfe.FECHA_AMFE = CType(IIf(oAmfe.FECHA_AMFE Is Nothing, New Nullable(Of Date), CDate(oAmfe.FECHA_AMFE)), Nullable(Of Date))
		'				Amfe.NIVEL_AMFE = oAmfe.NIVEL_AMFE
		'				Amfe.CODIGO = oAmfe.CODIGO
		'			Else
		'				Return False
		'			End If
		'		End If
		'		BBDD.SubmitChanges()
		'		Return True
		'	Catch ex As Exception
		'		log.Error(ex)
		'		Return False
		'	End Try
		'End Function

		''' <summary>
		''' Guarda el Amfe
		''' </summary>
		''' <param name="oAmfe">Amfe</param>
		''' <remarks></remarks>
		Public Sub GuardarAmfe(ByVal oAmfe As KaPlanLib.Registro.AMFES)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oAmfe.ID = 0) Then 'insertar
					BBDD.AMFES.InsertOnSubmit(oAmfe)
				Else 'actualizar
					Dim Amfe As KaPlanLib.Registro.AMFES = (From AmfeP In BBDD.AMFES
															Where AmfeP.ID = oAmfe.ID
															Select AmfeP).FirstOrDefault

					If (Amfe Is Nothing) Then
						Dim msg As String = "No se ha encontrado el 'AMFE'."
						log.Error(msg & " ID=" & oAmfe.ID)
						Throw New ApplicationException(msg)
					Else
						Amfe.FECHA_AMFE = CType(IIf(oAmfe.FECHA_AMFE Is Nothing, New Nullable(Of Date), CDate(oAmfe.FECHA_AMFE)), Nullable(Of Date))
						Amfe.NIVEL_AMFE = oAmfe.NIVEL_AMFE
						Amfe.CODIGO = oAmfe.CODIGO
					End If
				End If
				Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
				BBDD.SubmitChanges()
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error("oAmfe.ID = " & oAmfe.ID & vbCrLf & "oAmfe.CODIGO = " & oAmfe.CODIGO, ex)
				Throw
			End Try
		End Sub

		''' <summary>
		''' Elimina anfe
		''' </summary>
		''' <param name="idAmfe">Id amfe</param>
		''' <param name="cod">Codigo</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarAmfe(ByVal idAmfe As Integer, ByVal cod As String) As Boolean
			Dim transaction As DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try

				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				'Eliminamos Amfe
				Dim Amfe = (From Amf In BBDD.AMFES Where Amf.ID = idAmfe Select Amf).SingleOrDefault

				If Amfe IsNot Nothing Then
					BBDD.AMFES.DeleteOnSubmit(Amfe)
					Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
					BBDD.SubmitChanges()
				Else
					'Si no obtiene el amfe, se hara un rollback
					transaction.Rollback()
					Return False
				End If

				'Eliminamos plan por Amfe
				Dim planControl = From PlanContr In BBDD.PLAN_DE_CONTROL
								  Where PlanContr.CODIGO = cod
								  Select PlanContr

				If (planControl.Count = 1) Then
					BBDD.PLAN_DE_CONTROL.DeleteOnSubmit(planControl.First)
					BBDD.SubmitChanges()
				End If

				'Eliminamos hoja por Amfe
				Dim hojaInstr = From hojaI In BBDD.HOJA_DE_INSTRUCCIONES
								Where hojaI.CODIGO = cod
								Select hojaI

				If (hojaInstr.Count = 1) Then
					BBDD.HOJA_DE_INSTRUCCIONES.DeleteOnSubmit(hojaInstr.First)
					BBDD.SubmitChanges()
				End If

				transaction.Commit()
				Return True
			Catch ex As Exception
				transaction.Rollback()
				Return False
			Finally
				BBDD.Connection.Close()
			End Try

		End Function
#End Region

#Region "RESUMEN_AMFE"
		''' <summary>
		''' Devuelve un registro del resumen Amfe
		''' </summary>
		''' <param name="idAmfe">Id del registro Amfe</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarResumenAmfe(ByVal idAmfe As Integer) As KaPlanLib.Registro.RESUMEN_AMFE
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResAmfes = From ResumenAmfe In BBDD.RESUMEN_AMFE
							   Where ResumenAmfe.ID_AMFE = idAmfe
							   Select ResumenAmfe

				If (Not ResAmfes.Any) Then
					Return Nothing
				Else
					Return ResAmfes.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el resumen Amfe
		''' </summary>
		''' <param name="oResumen">Resumen</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarResumenAmfe(ByVal oResumen As KaPlanLib.Registro.RESUMEN_AMFE) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oResumen.ID_REGISTRO = 0) Then 'insertar
					BBDD.RESUMEN_AMFE.InsertOnSubmit(oResumen)
				Else 'actualizar
					Dim resumenAmfe As KaPlanLib.Registro.RESUMEN_AMFE = (From resumen In BBDD.RESUMEN_AMFE
																		  Where resumen.ID_AMFE = oResumen.ID_AMFE
																		  Select resumen).FirstOrDefault

					If (resumenAmfe IsNot Nothing) Then
						resumenAmfe.SOLICITA = oResumen.SOLICITA
						resumenAmfe.RESPONSABLE_PIEZA = oResumen.RESPONSABLE_PIEZA
						resumenAmfe.OBJETIVO_DEL_ESTUDIO = oResumen.OBJETIVO_DEL_ESTUDIO
						resumenAmfe.CAUSAS_DEL_ESTUDIO = oResumen.CAUSAS_DEL_ESTUDIO
						resumenAmfe.LIMITES_DEL_ESTUDIO = oResumen.LIMITES_DEL_ESTUDIO
						resumenAmfe.PERMANENTE_1 = oResumen.PERMANENTE_1
						resumenAmfe.PERMANENTE_2 = oResumen.PERMANENTE_2
						resumenAmfe.PERMANENTE_3 = oResumen.PERMANENTE_3
						resumenAmfe.PERMANENTE_4 = oResumen.PERMANENTE_4
						resumenAmfe.SECCION_1 = oResumen.SECCION_1
						resumenAmfe.SECCION_2 = oResumen.SECCION_2
						resumenAmfe.SECCION_3 = oResumen.SECCION_3
						resumenAmfe.SECCION_4 = oResumen.SECCION_4
						resumenAmfe.SECCION_21 = oResumen.SECCION_21
						resumenAmfe.SECCION_22 = oResumen.SECCION_22
						resumenAmfe.SECCION_23 = oResumen.SECCION_23
						resumenAmfe.SECCION_24 = oResumen.SECCION_24
						resumenAmfe.NUEVO_1 = oResumen.NUEVO_1
						resumenAmfe.NUEVO_2 = oResumen.NUEVO_2
						resumenAmfe.NUEVO_3 = oResumen.NUEVO_3
						resumenAmfe.NUEVO_4 = oResumen.NUEVO_4
						resumenAmfe.SECCION = oResumen.SECCION
						resumenAmfe.ANIMADOR = oResumen.ANIMADOR
						resumenAmfe.OBSERVACIONES = oResumen.OBSERVACIONES
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "PLANNING RESUMEN"
		''' <summary>
		''' Devuelve todos los planning resumen de una amfe pieza
		''' </summary>
		''' <param name="idAmfePieza">Codigo por el que se agrupan los planning resumen de Amfe</param>
		''' <returns></returns>
		Function consultarListadoPlanningResumen(ByVal idAmfePieza As Integer)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listPlanningRes = From PlaRes In BBDD.PLANNING_RESUMEN
									  Where PlaRes.ID_AMFE = idAmfePieza
									  Select PlaRes

				Return listPlanningRes
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el planning resumen de un amfe de pieza        
		''' </summary>
		''' <param name="oPlanRes">Datos del planning resumen amfe de la pieza</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarPlanningResumen(ByVal oPlanRes As Registro.PLANNING_RESUMEN) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oPlanRes.ID_REGISTRO = 0) Then 'Nuevo
					BBDD.PLANNING_RESUMEN.InsertOnSubmit(oPlanRes)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim planningResumen As KaPlanLib.Registro.PLANNING_RESUMEN = (From plaRes In BBDD.PLANNING_RESUMEN
																				  Where plaRes.ID_REGISTRO = oPlanRes.ID_REGISTRO
																				  Select plaRes).FirstOrDefault
					If (planningResumen Is Nothing) Then Return False

					planningResumen.ID_AMFE = oPlanRes.ID_AMFE
					planningResumen.PREVISTO = CType(IIf(oPlanRes.PREVISTO Is Nothing, New Nullable(Of Date), oPlanRes.PREVISTO.Value), Nullable(Of Date))
					planningResumen.REALIZADO = CType(IIf(oPlanRes.REALIZADO Is Nothing, New Nullable(Of Date), oPlanRes.REALIZADO.Value), Nullable(Of Date))
					planningResumen.EVENTO = oPlanRes.EVENTO
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina un planning resumen de amfe pieza
		''' </summary>
		''' <param name="idRegistro">Id Registro del planning</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarPlanningResumen(ByVal idRegistro As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim PlanningResumen = From PlanRes In BBDD.PLANNING_RESUMEN
									  Where PlanRes.ID_REGISTRO = idRegistro
									  Select PlanRes

				If (PlanningResumen.Count = 1) Then
					BBDD.PLANNING_RESUMEN.DeleteOnSubmit(PlanningResumen.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function
#End Region

#Region "BALANCE"

		''' <summary>
		''' Devuelve todos los balances de una amfe pieza
		''' </summary>
		''' <param name="idAmfePieza">Codigo por el que se agrupan los balances de Amfe</param>
		''' <returns></returns>
		Function consultarListadoBalance(ByVal idAmfePieza As Integer)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listBalance = From Balance In BBDD.BALANCE
								  Where Balance.ID_AMFE = idAmfePieza
								  Select Balance

				Return listBalance
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Devuelve el numero de causa de una amfe pieza
		''' </summary>
		''' <param name="codigo">Codigo</param>
		''' <returns></returns>
		Function consultarCountCausasBalance(ByVal codigo As String) As String()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listCausas = From Causa In BBDD.V_CONTAR_CAUSAS
								 Where Causa.CODIGO = codigo
								 Select Causa

				Dim contadores(1) As String
				Dim suma As Integer = 0
				contadores(0) = listCausas.Count
				For Each causa As Registro.V_CONTAR_CAUSAS In listCausas
					If (causa.NPR_CAUSA > causa.NPR_CLIENTE) Then suma += 1 'SUM(CASE WHEN NPR_CAUSA > NPR_CLIENTE THEN 1 ELSE 0 END) AS MALAS _
				Next
				contadores(1) = suma
				Return contadores
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el balance de un amfe de pieza        
		''' </summary>
		''' <param name="oBalance">Datos del balance amfe de la pieza</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarBalance(ByVal oBalance As Registro.BALANCE) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oBalance.ID_REGISTRO = 0) Then 'Nuevo
					BBDD.BALANCE.InsertOnSubmit(oBalance)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim balance As KaPlanLib.Registro.BALANCE = (From Balan In BBDD.BALANCE
																 Where Balan.ID_REGISTRO = oBalance.ID_REGISTRO
																 Select Balan).FirstOrDefault
					If (balance Is Nothing) Then Return False

					balance.ID_AMFE = oBalance.ID_AMFE
					balance.FECHA = CType(IIf(oBalance.FECHA Is Nothing, New Nullable(Of Date), oBalance.FECHA.Value), Nullable(Of Date))
					balance.Nº_TOTAL_DE_CAUSAS = CType(IIf(oBalance.Nº_TOTAL_DE_CAUSAS Is Nothing, New Nullable(Of Short), oBalance.Nº_TOTAL_DE_CAUSAS.Value), Nullable(Of Short))
					balance.Nº_CAUSAS_CON_NPR_100 = CType(IIf(oBalance.Nº_CAUSAS_CON_NPR_100 Is Nothing, New Nullable(Of Short), oBalance.Nº_CAUSAS_CON_NPR_100.Value), Nullable(Of Short))
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina un balance de amfe pieza
		''' </summary>
		''' <param name="idRegistro">Id Registro del balance</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarBalance(ByVal idRegistro As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim Balance = From Balan In BBDD.BALANCE
							  Where Balan.ID_REGISTRO = idRegistro
							  Select Balan

				If (Balance.Count = 1) Then
					BBDD.BALANCE.DeleteOnSubmit(Balance.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "MODO FALLO"
		''' <summary>
		''' Guarda el modo fallo
		''' </summary>
		''' <param name="oModoF">Modo fallo pieza</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarModoFallo(ByVal oModoF As KaPlanLib.Registro.MODOS_DE_FALLO) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oModoF.ID_MODO = 0) Then 'insertar
					BBDD.MODOS_DE_FALLO.InsertOnSubmit(oModoF)
				Else 'actualizar
					Dim ModoFallo As KaPlanLib.Registro.MODOS_DE_FALLO = (From MF In BBDD.MODOS_DE_FALLO
																		  Where MF.ID_MODO = oModoF.ID_MODO
																		  Select MF).FirstOrDefault

					If (ModoFallo IsNot Nothing) Then
						ModoFallo.MODO_DE_FALLO = oModoF.MODO_DE_FALLO
						ModoFallo.Modos_de_Fallo_Origen = oModoF.Modos_de_Fallo_Origen
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina un modo fallo
		''' </summary>
		''' <param name="idModo">Codigo</param>        
		''' <remarks></remarks>
		Sub EliminarModoFallo(ByVal idModo As Integer)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				Dim ModoFallo As Registro.MODOS_DE_FALLO = (From ModoF In BBDD.MODOS_DE_FALLO Where ModoF.ID_MODO = idModo Select ModoF).SingleOrDefault

				If ModoFallo IsNot Nothing Then
					Dim lCF As List(Of Registro.CAUSAS_DE_FALLO) = ModoFallo.CAUSAS_DE_FALLO.ToList

					If lCF IsNot Nothing AndAlso lCF.Any Then
						For Each cf As Registro.CAUSAS_DE_FALLO In lCF
							EliminarCaracteristicasPlanControl(ModoFallo.CARACTERISTICAS_AMFE.ID_CARACTERISTICA, cf.CONT_CAUSA)
						Next
					End If

					BBDD.MODOS_DE_FALLO.DeleteOnSubmit(ModoFallo)
					BBDD.SubmitChanges()
				End If

			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub
#End Region

#Region "EFECTOS"

		''' <summary>
		''' Guarda el efecto
		''' </summary>
		''' <param name="oEfecto">Efecto</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarEfecto(ByVal oEfecto As KaPlanLib.Registro.EFECTOS) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oEfecto.ID_REGISTRO = 0) Then 'insertar
					BBDD.EFECTOS.InsertOnSubmit(oEfecto)
				Else 'actualizar
					Dim Efect As KaPlanLib.Registro.EFECTOS = (From Ef In BBDD.EFECTOS
															   Where Ef.ID_REGISTRO = oEfecto.ID_REGISTRO
															   Select Ef).FirstOrDefault

					If (Efect IsNot Nothing) Then
						Efect.EFECTO = oEfecto.EFECTO
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina un efecto
		''' </summary>
		''' <param name="idRegistro">Codigo</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarEfecto(ByVal idRegistro As String) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				Dim Efecto = From Efec In BBDD.EFECTOS
							 Where Efec.ID_REGISTRO = idRegistro
							 Select Efec

				If (Efecto.Count = 1) Then
					BBDD.EFECTOS.DeleteOnSubmit(Efecto.First)
					BBDD.SubmitChanges()
					Return True
				End If
				Return False
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "OCURRENCIA"

		''' <summary>
		''' Devuelve todos las ocurrencias
		''' </summary>
		''' <returns></returns>
		Function consultarListadoOcurrencias()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listOcu = From Ocurrencia In BBDD.OCURRENCIA
							  Select Ocurrencia

				Return listOcu
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

#End Region

#Region "GRAVEDAD"
		''' <summary>
		''' Devuelve todos los registros gravedad
		''' </summary>
		''' <returns></returns>
		Function consultarListadoGravedad()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listGrav = From Gravedad In BBDD.GRAVEDAD
							   Select Gravedad

				Return listGrav
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function
#End Region

#Region "DETECCION"
		''' <summary>
		''' Devuelve todos los registros deteccion
		''' </summary>
		''' <returns></returns>
		Function consultarListadoDeteccion()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listDet = From Deteccion In BBDD.DETECCION
							  Select Deteccion

				Return listDet
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function
#End Region

#Region "CAUSAS_DE_FALLO"
		''' <summary>
		''' Devuelve un registro de causa fallo
		''' </summary>
		''' <param name="contCausa">Id del registro</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarCausaFallo(ByVal contCausa As Integer) As KaPlanLib.Registro.CAUSAS_DE_FALLO
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim causaFallo = From CausaF In BBDD.CAUSAS_DE_FALLO
								 Where CausaF.CONT_CAUSA = contCausa
								 Select CausaF

				If (Not causaFallo.Any) Then
					Return Nothing
				Else
					Return causaFallo.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda causas fallo
		''' </summary>
		''' <param name="oCausasF">Causas Fallo</param>
		''' <remarks></remarks>
		Sub GuardarCausaFallo(ByVal oCausasF As KaPlanLib.Registro.CAUSAS_DE_FALLO, CodOperacion As String)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim causasF As KaPlanLib.Registro.CAUSAS_DE_FALLO = oCausasF
			Try
				BBDD.Connection.Open()
				BBDD.Transaction = BBDD.Connection.BeginTransaction

				'------------------------------------------------------------------------------------------------
				'1º - Actualizamos las "Causas de Fallo"
				'------------------------------------------------------------------------------------------------
				If (oCausasF.CONT_CAUSA = 0) Then 'insertar
					BBDD.CAUSAS_DE_FALLO.InsertOnSubmit(causasF)
				Else 'actualizar
					causasF = (From causa In BBDD.CAUSAS_DE_FALLO Where causa.CONT_CAUSA = oCausasF.CONT_CAUSA Select causa).FirstOrDefault

					If (causasF Is Nothing) Then Throw New ApplicationException("Error en ""Causas de Fallo""") 'Return False

					causasF.ID_MODO = oCausasF.ID_MODO

					causasF.ACCION_RECOMENDADA = oCausasF.ACCION_RECOMENDADA
					causasF.NUEVO_MEDIO_COTROL = oCausasF.NUEVO_MEDIO_COTROL
					causasF.CAUSA = oCausasF.CAUSA
					causasF.GRAVEDAD = oCausasF.GRAVEDAD
					causasF.OCURRENCIA = oCausasF.OCURRENCIA
					causasF.DETECCION = oCausasF.DETECCION
					causasF.NUEVA_GRAVEDAD = oCausasF.NUEVA_GRAVEDAD
					causasF.NUEVA_OCURRENCIA = oCausasF.NUEVA_OCURRENCIA
					causasF.NUEVA_DETECCION = oCausasF.NUEVA_DETECCION
					'causasF.ENVIAR_A_PLAN = oCausasF.ENVIAR_A_PLAN
					'causasF._EN_PLAN_ = oCausasF._EN_PLAN_
					causasF.FRECUENCIA_DE_CONTROL = oCausasF.FRECUENCIA_DE_CONTROL
					causasF.FRECUENCIA_DE_CONTROL_CAL = oCausasF.FRECUENCIA_DE_CONTROL_CAL
					causasF.FECHA_DE_INICIO = oCausasF.FECHA_DE_INICIO
					causasF.FECHA_DE_FIN = oCausasF.FECHA_DE_FIN
					causasF.NOMBRE_MEDIO = oCausasF.NOMBRE_MEDIO
					causasF.NOMBRE_MEDIO_FAB = oCausasF.NOMBRE_MEDIO_FAB
					causasF.Codigo_Control_ME_FAB = oCausasF.Codigo_Control_ME_FAB
					causasF.Tamaño_OP = oCausasF.Tamaño_OP
					causasF.Tamaño_CAL = oCausasF.Tamaño_CAL
					causasF.NOMBRE_MEDIO_HUM = oCausasF.NOMBRE_MEDIO_HUM
					causasF.PREVENTIVA_MEDIO_FAB = oCausasF.PREVENTIVA_MEDIO_FAB
					causasF.DEPARTAMENTO = oCausasF.DEPARTAMENTO
					causasF.PROGRESO = oCausasF.PROGRESO
					causasF.CONTROLES = oCausasF.CONTROLES

					causasF.METODO_CONTROL = oCausasF.METODO_CONTROL
					causasF.METODO_CONTROL_FAB = oCausasF.METODO_CONTROL_FAB
				End If
				BBDD.SubmitChanges()
				'------------------------------------------------------------------------------------------------

				'------------------------------------------------------------------------------------------------
				'2º - Comprobamos que exista niveles para la Operacion.
				'------------------------------------------------------------------------------------------------
				Dim lPlancontrol As List(Of Registro.PLAN_DE_CONTROL) = (From PC As Registro.PLAN_DE_CONTROL In BBDD.PLAN_DE_CONTROL Where PC.CODIGO = CodOperacion Select PC).ToList
				If lPlancontrol Is Nothing OrElse Not lPlancontrol.Any Then
					Dim PlanControl As New Registro.PLAN_DE_CONTROL
					PlanControl.CODIGO = CodOperacion
					PlanControl.FECHA = FormatDateTime(Now, DateFormat.ShortDate)
					PlanControl.NIVEL = "0"

					BBDD.PLAN_DE_CONTROL.InsertOnSubmit(PlanControl)
					BBDD.SubmitChanges()
				End If
				'------------------------------------------------------------------------------------------------

				BBDD.SubmitChanges()
				BBDD.Transaction.Commit()
			Catch ex As ApplicationException
				BBDD.Transaction.Rollback()
				Throw
			Catch ex As Exception
				log.Error(ex)
				BBDD.Transaction.Rollback()
				Throw
			End Try

			'-----------------------------------------------------------------------------------------------------------------------
			'FROGA:2012-12-17: Comprobamos que para la Caracteristica de la "Causa de Fallo" no tenga mas "Causas de Fallo" enviadas al plan.
			'Solo puede haber una "Causa de Fallo" enviada al "Plan de Control Operacion".
			'-----------------------------------------------------------------------------------------------------------------------
			Try
				'------------------------------------------------------------------------------------------------
				'3º - Insertamos, Modificamos o Borramos las "CARACTERISTICAS DEL PLAN" dependiendo de si ya se ha enviado al plan o no.
				'------------------------------------------------------------------------------------------------
				If (oCausasF.ENVIAR_A_PLAN = "Sí" Or oCausasF.ENVIAR_A_PLAN = "Si") Then
					'-----------------------------------------------------------------------------------------------------------------
					'Comprobamos que para la "Caracteristica" no tenga mas "Causas de Fallo" enviadas al "Plan de Control Operacion".
					'-----------------------------------------------------------------------------------------------------------------

					Dim CARAC_AMFE As Registro.CARACTERISTICAS_AMFE = oCausasF.MODOS_DE_FALLO.CARACTERISTICAS_AMFE
					Dim lMODOS_DE_FALLO As List(Of Registro.MODOS_DE_FALLO) = CARAC_AMFE.MODOS_DE_FALLO.ToList
					Dim lCausas As New List(Of Registro.CAUSAS_DE_FALLO)
					For Each Modo As Registro.MODOS_DE_FALLO In lMODOS_DE_FALLO
						For Each Causa As Registro.CAUSAS_DE_FALLO In Modo.CAUSAS_DE_FALLO
							If Causa._EN_PLAN_ = "1" And oCausasF.CONT_CAUSA <> Causa.CONT_CAUSA Then

								causasF.ENVIAR_A_PLAN = "No"
								causasF._EN_PLAN_ = "0" 'NO Esta en el "Plan de Control Operacion"
								BBDD.SubmitChanges()

								Dim Mensaje As String
								Mensaje = "No se ha podido enviar al 'Plan de Control Operacion'."
								Mensaje &= "El Modo de Fallo:'" & Modo.MODO_DE_FALLO & "' ya tiene la Causa de Fallo:'" & Causa.CAUSA & "' enviada al 'Plan de Control Operacion'"
								Throw New ApplicationException(Mensaje)
							End If
						Next
					Next
					'-----------------------------------------------------------------------------------------------------------------

					Dim lCaracPlan =
						From OT As Registro.OPERACIONES_TIPO In BBDD.OPERACIONES_TIPO
						Join AMFE As Registro.AMFES In BBDD.AMFES On OT.COD_OPERACION Equals AMFE.CODIGO
						Join CA As Registro.CARACTERISTICAS_AMFE In BBDD.CARACTERISTICAS_AMFE On CA.ID_AMFE Equals AMFE.ID
						Join MF As Registro.MODOS_DE_FALLO In BBDD.MODOS_DE_FALLO On MF.ID_CARACTERISTICA Equals CA.ID_CARACTERISTICA
						Join CF As Registro.CAUSAS_DE_FALLO In BBDD.CAUSAS_DE_FALLO On MF.ID_MODO Equals CF.ID_MODO
						Where CF.CONT_CAUSA = oCausasF.CONT_CAUSA
						Select AMFE.CODIGO, CA.ID_CARACTERISTICA, CA.CARACTERISTICA, CA.ORDEN, CA.CLASE, CA.ESPECIFICACION,
						CF.CONTROLES, CF.NUEVO_MEDIO_COTROL, CF.FRECUENCIA_DE_CONTROL, CF.FRECUENCIA_DE_CONTROL_CAL,
						OT.AUDITORIA, CF.CONT_CAUSA, CF.Codigo_Control_ME_FAB, CF.Tamaño_OP, CF.Tamaño_CAL, CF.NOMBRE_MEDIO, CF.NOMBRE_MEDIO_FAB, CF.METODO_CONTROL, CF.METODO_CONTROL_FAB

					If lCaracPlan IsNot Nothing AndAlso lCaracPlan.Any Then
						For Each Reg In lCaracPlan
							Dim CaracteristicaPlan As New Registro.CARACTERISTICAS_DEL_PLAN

							'-------------------------------------------------------------------------------
							'Comprobamos que exista para hacer una modificacion.
							'------------------------------------------------------------------------------- 
							CaracteristicaPlan = (From CP As Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN
												  Where CP.CONT_CAUSA = oCausasF.CONT_CAUSA _
												  And CP.ID_CARACTERISTICA = Reg.ID_CARACTERISTICA _
												  And CP.CODIGO = Reg.CODIGO
												  Select CP).FirstOrDefault
							'-------------------------------------------------------------------------------------------------------------------------------- 
							If CaracteristicaPlan Is Nothing Then CaracteristicaPlan = New Registro.CARACTERISTICAS_DEL_PLAN
							'-------------------------------------------------------------------------------                         

							CaracteristicaPlan.CODIGO = Reg.CODIGO
							CaracteristicaPlan.ID_CARACTERISTICA = Reg.ID_CARACTERISTICA
							CaracteristicaPlan.CARAC_PARAM = Reg.CARACTERISTICA
							'Evitamos que el orden de la caracteristica de "Plan de Control Operacion" se modifique conservando su valor de ordenacion.
							If CaracteristicaPlan.ORDEN_CARAC Is Nothing OrElse CaracteristicaPlan.ORDEN_CARAC <= 0 Then CaracteristicaPlan.ORDEN_CARAC = Reg.ORDEN
							CaracteristicaPlan.CLASE = Reg.CLASE
							CaracteristicaPlan.ESPECIFICACION = Reg.ESPECIFICACION

							CaracteristicaPlan.Control_DET_ME_FAB = Reg.NOMBRE_MEDIO_FAB

							'-----------------------------------------------------------------------------------------------------------------------------------
							'If (oCausasF.NUEVO_MEDIO_COTROL Is Nothing OrElse oCausasF.NUEVO_MEDIO_COTROL = String.Empty) Then
							'    CaracteristicaPlan.MEDIO_DENOMINACION = Reg.CONTROLES
							'Else
							'    CaracteristicaPlan.MEDIO_DENOMINACION = Reg.NUEVO_MEDIO_COTROL
							'End If
							'CaracteristicaPlan.Control_DET_ME_CTRL = Reg.NOMBRE_MEDIO
							'-----------------------------------------------------------------------------------------------------------------------------------
							'FROGA:2012-08-07: Han pedido que si "txtNuevoMedioControl (CAUSAS_DE_FALLO.NUEVO_MEDIO_COTROL)" tiene algo se debe guardar en
							'"txtControl_DET_ME_CTRL/lblControl_DET_ME_CTRL" (CARACTERISTICAS_DEL_PLAN.Control_DET_ME_CTRL).
							'-----------------------------------------------------------------------------------------------------------------------------------
							CaracteristicaPlan.MEDIO_DENOMINACION = Reg.CONTROLES
							'CaracteristicaPlan.Control_DET_ME_CTRL = If(String.IsNullOrWhiteSpace(Reg.NUEVO_MEDIO_COTROL), Reg.NOMBRE_MEDIO, Reg.NUEVO_MEDIO_COTROL)
							'-----------------------------------------------------------------------------------------------------------------------------------
							'FROGA:2014-08-29: Han pedido que NO pase el valor "txtNuevoMedioControl (CAUSAS_DE_FALLO.NUEVO_MEDIO_COTROL)" a
							'"txtControl_DET_ME_CTRL/lblControl_DET_ME_CTRL" (CARACTERISTICAS_DEL_PLAN.Control_DET_ME_CTRL).
							'-----------------------------------------------------------------------------------------------------------------------------------
							CaracteristicaPlan.Control_DET_ME_CTRL = Reg.NOMBRE_MEDIO
							'-----------------------------------------------------------------------------------------------------------------------------------

							CaracteristicaPlan.Codigo_Control_ME_FAB = Reg.Codigo_Control_ME_FAB

							CaracteristicaPlan.FRECUENCIA_CONTROL = Reg.FRECUENCIA_DE_CONTROL
							CaracteristicaPlan.FRECUENCIA_CONTROL_CAL = Reg.FRECUENCIA_DE_CONTROL_CAL

							CaracteristicaPlan.TAMAÑO = Reg.Tamaño_OP
							CaracteristicaPlan.TAMAÑO_CAL = Reg.Tamaño_CAL

							CaracteristicaPlan.PROCEDE_DE = "A.M.F.E."

							CaracteristicaPlan.CONT_CAUSA = Reg.CONT_CAUSA

							'------------------------------------------------------------
							'2012-10-31: Me piden que comente esta parte del codigo.
							'------------------------------------------------------------
							'If Reg.AUDITORIA <> False Then
							'	CaracteristicaPlan.FRECUENCIA_REGISTRO = "Mod. 17.03-A"
							'Else
							'	CaracteristicaPlan.FRECUENCIA_REGISTRO = "Mod. 10.02-A"
							'End If
							'CaracteristicaPlan.HOJA_REGISTROS = False
							'------------------------------------------------------------

							'------------------------------------------------------------------------------------
							'Calculamos quienes son los responsables.
							'------------------------------------------------------------------------------------
							CaracteristicaPlan.Responsable_Maquina = Responsable_Maquina(CaracteristicaPlan)
							CaracteristicaPlan.Responsable_Operario = Responsable_Operario(CaracteristicaPlan)
							CaracteristicaPlan.Responsable_Calidad = Responsable_Calidad(CaracteristicaPlan)
							'------------------------------------------------------------------------------------

							CaracteristicaPlan.METODO_CONTROL = Reg.METODO_CONTROL
							CaracteristicaPlan.METODO_CONTROL_FAB = Reg.METODO_CONTROL_FAB

							'---------------------------------------------------------------------------
							'Comprobamos si es un registro nuevo para hacer una insercion.
							'---------------------------------------------------------------------------
							If CaracteristicaPlan.ID_REGISTRO = 0 Then BBDD.CARACTERISTICAS_DEL_PLAN.InsertOnSubmit(CaracteristicaPlan)
							'---------------------------------------------------------------------------
							BBDD.SubmitChanges()
							ReorganizarOrden_CARACTERISTICAS_DEL_PLAN(CaracteristicaPlan, Nothing)
						Next
						causasF.ENVIAR_A_PLAN = oCausasF.ENVIAR_A_PLAN
						causasF._EN_PLAN_ = "1" 'Esta en el "Plan de Control Operacion"
					End If
					'-------------------------------------------------------------------------------------------------------------------------
				ElseIf oCausasF.ENVIAR_A_PLAN = "No" Then
					EliminarCaracteristicasPlanControl(causasF.MODOS_DE_FALLO.ID_CARACTERISTICA)

					causasF.ENVIAR_A_PLAN = oCausasF.ENVIAR_A_PLAN
					causasF._EN_PLAN_ = "0" 'NO Esta en el "Plan de Control Operacion"

					ReorganizarOrden_CARACTERISTICAS_DEL_PLAN(causasF.MODOS_DE_FALLO.CARACTERISTICAS_AMFE.AMFES.CODIGO, BBDD)
				End If
				'------------------------------------------------------------------------------------------------

				BBDD.SubmitChanges()
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
			'-----------------------------------------------------------------------------------------------------------------------

		End Sub

		''' <summary>
		''' Elimina una causa fallo
		''' </summary>
		''' <param name="contCausa">Id del registro</param>        
		''' <remarks></remarks>
		Sub EliminarCausaFallo(ByVal contCausa As Integer)
			Dim BBDD As New KaPlanLib.DAL.ELL
			'Dim CodOperacion As String
			Try
				Dim CausaFallo As Registro.CAUSAS_DE_FALLO = (From causaF In BBDD.CAUSAS_DE_FALLO Where causaF.CONT_CAUSA = contCausa Select causaF).SingleOrDefault
				If CausaFallo IsNot Nothing Then
					EliminarCaracteristicasPlanControl(CausaFallo.MODOS_DE_FALLO.ID_CARACTERISTICA, CausaFallo.CONT_CAUSA)
					BBDD.CAUSAS_DE_FALLO.DeleteOnSubmit(CausaFallo)
					BBDD.SubmitChanges()
				End If
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub
#End Region

#Region "MAESTRO CAUSAS DE FALLO"

		''' <summary>
		''' Devuelve todos los registros de maestro causas de fallo
		''' </summary>
		''' <returns></returns>
		Function consultarListadoMaestroCausasFallo(ByVal operacion As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim MCausaFallo = (From MCausaF In BBDD.MAESTRO_DE_CAUSAS_DE_FALLO
								   Where MCausaF.OPERACION = operacion
								   Select MCausaF).ToList

				Return MCausaFallo
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

#End Region

#Region "PLANTAS ACCESO"

		''' <summary>
		''' Accede a las bases de datos para ver si tiene acceso a dicha planta
		''' Se crea un array de plantas. Cada planta es un array de string con 3 elementos: IdPlanta,NombrePlanta y cadena de conexion
		''' </summary>
		''' <param name="email">Email del usuario</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function PlantasAcceso(ByVal email As String) As List(Of String())
			Try
				Dim lPlantas As New List(Of String())
				Dim conexionString, namePlanta As String

				For Each idPlanta As Integer In [Enum].GetValues(GetType(plantas))
					namePlanta = [Enum].GetName(GetType(plantas), idPlanta)
					'-----------------------------------------------------------------------------------------
					'Comprobamos que las conexiones definidas en KaPlanLib coincidan con los de la aplicacion.
					'-----------------------------------------------------------------------------------------
					If Configuration.ConfigurationManager.ConnectionStrings(namePlanta) IsNot Nothing Then
						conexionString = Configuration.ConfigurationManager.ConnectionStrings(namePlanta).ConnectionString
						Dim BBDD As New KaPlanLib.DAL.ELL(conexionString)
						Dim Acceso = (From Personal In BBDD.MAESTRO_PERSONAL
									  Where Personal.E_MAIL.ToLower = email.ToLower
									  Select Personal).FirstOrDefault
						If (Acceso IsNot Nothing) Then
							Dim sPlanta As String() = {idPlanta, namePlanta, conexionString}
							lPlantas.Add(sPlanta)
						End If
					End If
					'-----------------------------------------------------------------------------------------
				Next

				If (Not lPlantas.Any) Then lPlantas = Nothing
				Return lPlantas
			Catch ex As Exception
				log.Error(ex)
				Return Nothing
			End Try
		End Function

		''' <summary>
		''' Obtenemos los accesos a la base de datos.
		''' Se crea un array de plantas. Cada planta es un array de string con 3 elementos: IdPlanta,NombrePlanta y cadena de conexion
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function PlantasAcceso() As List(Of String())
			Try
				Dim lPlantas As New List(Of String())
				Dim conexionString, namePlanta As String

				For Each idPlanta As Integer In [Enum].GetValues(GetType(plantas))
					namePlanta = [Enum].GetName(GetType(plantas), idPlanta)
					'-----------------------------------------------------------------------------------------
					'Comprobamos que las conexiones definidas en KaPlanLib coincidan con los de la aplicacion.
					'-----------------------------------------------------------------------------------------
					If Configuration.ConfigurationManager.ConnectionStrings(namePlanta) IsNot Nothing Then
						conexionString = Configuration.ConfigurationManager.ConnectionStrings(namePlanta).ConnectionString
						Dim sPlanta As String() = {idPlanta, namePlanta, conexionString}
						lPlantas.Add(sPlanta)
					End If
					'-----------------------------------------------------------------------------------------
				Next

				If (Not lPlantas.Any) Then lPlantas = Nothing
				Return lPlantas
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Function

#End Region

#Region "PLANNING SEGUIMIENTO"

		''' <summary>
		''' Devuelve el registro del planning de seguimiento
		''' </summary>
		''' <param name="ref">Referencia</param>
		''' <returns></returns>
		Function consultarPlanningSeguimiento(ByVal ref As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim planning = From PlanningS In BBDD.PLANNING_SEGUIMIENTO
							   Where PlanningS.CODIGO = ref
							   Select PlanningS

				If (Not planning.Any) Then
					Return Nothing
				Else
					Return planning.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el plan de segumiento
		''' </summary>
		''' <param name="planSeg">Plan de seguimiento</param>        
		''' <remarks></remarks>
		Sub GuardarPlanningSeguimiento(ByVal planSeg As Registro.PLANNING_SEGUIMIENTO)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				Dim planningSeg = (From PlanControl In BBDD.PLANNING_SEGUIMIENTO
								   Where PlanControl.CODIGO = planSeg.CODIGO
								   Select PlanControl).FirstOrDefault
				If (planningSeg Is Nothing) Then  'si no devuelve resultados, se hara un insert
					BBDD.PLANNING_SEGUIMIENTO.InsertOnSubmit(planSeg)
				Else  'Sino se actualiza el registro con los nuevos datos
					planningSeg.CODIGO = planSeg.CODIGO
					planningSeg.OFERTA = planSeg.OFERTA
					planningSeg.VºBº = planSeg.VºBº
					planningSeg.REALIZADO = planSeg.REALIZADO
					planningSeg.FECHA_INICIO = planSeg.FECHA_INICIO
					planningSeg.FECHA_FINAL = planSeg.FECHA_FINAL
					planningSeg.FECHA_PREVISTA_FIN = planSeg.FECHA_PREVISTA_FIN
					planningSeg.FECHA_VºBº = planSeg.FECHA_VºBº
					planningSeg.FECHA_REALIZADO = planSeg.FECHA_REALIZADO
				End If
				BBDD.SubmitChanges()

			Catch ex As Exception
				log.Error(ex)
				'Throw New Exception("Error al guardar el plan de control de operacion", ex)
				Throw
			End Try
		End Sub


		''' <summary>
		''' Elimina una plan de seguimiento
		''' </summary>
		''' <param name="ref">Referencia</param>
		''' <remarks></remarks>
		Public Sub EliminarPlanningSeguimiento(ByVal ref As String)
			Dim BBDD As New KaPlanLib.DAL.ELL

			Dim planSeguimiento = From Plan In BBDD.PLANNING_SEGUIMIENTO
								  Where Plan.CODIGO = ref
								  Select Plan

			If planSeguimiento.Any Then
				BBDD.PLANNING_SEGUIMIENTO.DeleteOnSubmit(planSeguimiento.First)
				BBDD.SubmitChanges()
			End If
		End Sub

#End Region

#Region "CONCEPTO SEGUIMIENTO"

		''' <summary>
		''' Devuelve todos los conceptos de seguimientos de una referencia
		''' </summary>
		''' <param name="ref">Referencia</param>
		''' <returns></returns>
		Function consultarListadoConceptoSeguimiento(ByVal ref As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim Concepto = From Concep In BBDD.CONCEPTOS_SEGUIMIENTO
							   Where Concep.CODIGO = ref
							   Select Concep

				Return Concepto
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el concepto del plan de seguimiento
		''' </summary>
		''' <param name="oConcepto">Datos del concepto</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarConceptoSeguimiento(ByVal oConcepto As Registro.CONCEPTOS_SEGUIMIENTO) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oConcepto.ID_REGISTRO = 0) Then 'es nuevo: insert
					BBDD.CONCEPTOS_SEGUIMIENTO.InsertOnSubmit(oConcepto)
					BBDD.SubmitChanges()
				Else
					Dim concepSeg As KaPlanLib.Registro.CONCEPTOS_SEGUIMIENTO = (From conceptoSeg In BBDD.CONCEPTOS_SEGUIMIENTO
																				 Where conceptoSeg.ID_REGISTRO = oConcepto.ID_REGISTRO
																				 Select conceptoSeg).FirstOrDefault
					If (concepSeg Is Nothing) Then Return False

					concepSeg.CODIGO = oConcepto.CODIGO
					concepSeg.NUMERO_CONCEPTO = oConcepto.NUMERO_CONCEPTO
					concepSeg.CONCEPTO = oConcepto.CONCEPTO
					concepSeg.DEPARTAMENTO = oConcepto.DEPARTAMENTO
					concepSeg.PERSONA = oConcepto.PERSONA
					concepSeg.OBSERVACIONES = oConcepto.OBSERVACIONES
					concepSeg.INICIO_PREVISTO = oConcepto.INICIO_PREVISTO
					concepSeg.INICIO_REAL = oConcepto.INICIO_REAL
					concepSeg.FINAL_PREVISTO = oConcepto.FINAL_PREVISTO
					concepSeg.FINAL_REAL = oConcepto.FINAL_REAL
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina un concepto de plan de seguimiento
		''' </summary>
		''' <param name="idRegistro">Id registro del concepto</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarConceptoSeguimiento(ByVal idRegistro As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ConceptoSeguim = From concepSeg In BBDD.CONCEPTOS_SEGUIMIENTO
									 Where concepSeg.ID_REGISTRO = idRegistro
									 Select concepSeg

				If (ConceptoSeguim.Count = 1) Then
					BBDD.CONCEPTOS_SEGUIMIENTO.DeleteOnSubmit(ConceptoSeguim.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "NIVEL_SEGUIMIENTO"

		''' <summary>
		''' Devuelve todos los niveles de un planning de segumiento
		''' </summary>
		''' <param name="codigo">Codigo</param>
		''' <returns></returns>
		Function consultarListadoNivelesSeguimiento(ByVal codigo As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listNiveles = From Niveles In BBDD.NIVELES_SEGUIMIENTO
								  Where Niveles.CODIGO = codigo
								  Select Niveles

				Return listNiveles
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el nivel del plan de seguimiento
		''' </summary>
		''' <param name="oNivel">Datos del nivel</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarNivelSeguimiento(ByVal oNivel As Registro.NIVELES_SEGUIMIENTO) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oNivel.ID_REGISTRO = 0) Then 'es nuevo: insert
					BBDD.NIVELES_SEGUIMIENTO.InsertOnSubmit(oNivel)
					BBDD.SubmitChanges()
				Else
					Dim nivSeg As KaPlanLib.Registro.NIVELES_SEGUIMIENTO = (From nivSeguimiento In BBDD.NIVELES_SEGUIMIENTO
																			Where nivSeguimiento.ID_REGISTRO = oNivel.ID_REGISTRO
																			Select nivSeguimiento).FirstOrDefault
					If (nivSeg Is Nothing) Then Return False

					nivSeg.CODIGO = oNivel.CODIGO
					nivSeg.NIVEL = oNivel.NIVEL
					nivSeg.MODIFICACION = oNivel.MODIFICACION
					nivSeg.FECHA_PREVISTA = oNivel.FECHA_PREVISTA
					nivSeg.FECHA_REALIZADA = oNivel.FECHA_REALIZADA
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina un nivel de plan de seguimiento
		''' </summary>
		''' <param name="idRegistro">Id registro del nivel</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarNivelSeguimiento(ByVal idRegistro As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim NivelSeguimiento = From NivelSeg In BBDD.NIVELES_SEGUIMIENTO
									   Where NivelSeg.ID_REGISTRO = idRegistro
									   Select NivelSeg

				If (NivelSeguimiento.Count = 1) Then
					BBDD.NIVELES_SEGUIMIENTO.DeleteOnSubmit(NivelSeguimiento.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "COPIAS"
		''' <summary>
		''' Copia una articulo completo en otro nuevo
		''' </summary>
		''' <param name="refOrigen"></param>
		''' <param name="refDestino"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function CopiarArticulo(ByVal refOrigen As String, ByVal refDestino As String) As Boolean
			Dim transaction As System.Data.Common.DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				'1º Se copian los datos de MAESTRO_ARTICULOS (1)
				Dim oMaestro As Registro.MAESTRO_ARTICULOS = consultarMaestroArticulo(refOrigen)
				If (oMaestro Is Nothing) Then Throw New ApplicationException("Error al obtener los datos de la referencia de origen")

				Dim oMaestroNew As New Registro.MAESTRO_ARTICULOS
				oMaestroNew.CODIGO = refDestino
				oMaestroNew.FAMILIA = oMaestro.FAMILIA
				oMaestroNew.REFERENCIA_CLIENTE = oMaestro.REFERENCIA_CLIENTE
				oMaestroNew.JEFE_DE_PROYECTO = oMaestro.JEFE_DE_PROYECTO
				oMaestroNew.NIVEL = oMaestro.NIVEL
				oMaestroNew.DENOMINACION = oMaestro.DENOMINACION
				oMaestroNew.PLANO = oMaestro.PLANO
				oMaestroNew.CLIENTE = oMaestro.CLIENTE
				oMaestroNew.VEHICULO = oMaestro.VEHICULO
				oMaestroNew.FECHA_SERIE = oMaestro.FECHA_SERIE
				oMaestroNew.ORGANO = oMaestro.ORGANO
				oMaestroNew.OBJETIVO = oMaestro.OBJETIVO
				oMaestroNew.ESLABON = oMaestro.ESLABON
				oMaestroNew.LANTEGI = oMaestro.LANTEGI

				BBDD.MAESTRO_ARTICULOS.InsertOnSubmit(oMaestroNew)
				BBDD.SubmitChanges()

				'2º Se copian los datos de CLIENTES_ARTICULOS (Varios)
				Dim lClientesArtic = From clientArt In BBDD.CLIENTES_ARTICULO
									 Where clientArt.CODIGO = refOrigen
									 Select clientArt
				If (lClientesArtic IsNot Nothing) Then
					Dim oClientArtNew As Registro.CLIENTES_ARTICULO
					For Each oClientArt As Registro.CLIENTES_ARTICULO In lClientesArtic
						oClientArtNew = New Registro.CLIENTES_ARTICULO
						oClientArtNew.CODIGO = refDestino
						oClientArtNew.REFERENCIA_CLIENTE = oClientArt.REFERENCIA_CLIENTE

						BBDD.CLIENTES_ARTICULO.InsertOnSubmit(oClientArtNew)
						BBDD.SubmitChanges()
					Next
				End If

				'3º Se copian los datos de SINOPTICO (1)
				Dim oSinoptico As Registro.SINOPTICO = Nothing
				oSinoptico = (From sinop In BBDD.SINOPTICO
							  Where sinop.CODIGO = refOrigen
							  Select sinop).FirstOrDefault()
				'En este caso, no se produce un error cuando no tenga sinoptico ya que no tiene porque ser un dato obligatorio
				If (oSinoptico IsNot Nothing) Then
					Dim oSinopticoNew As New Registro.SINOPTICO
					oSinopticoNew.CODIGO = refDestino
					oSinopticoNew.NIVEL_SINOPTICO = oSinoptico.NIVEL_SINOPTICO
					oSinopticoNew.DIBUJO = oSinoptico.DIBUJO

					BBDD.SINOPTICO.InsertOnSubmit(oSinopticoNew)
					BBDD.SubmitChanges()
				End If

				'4º Se copian los datos de OPERACIONES_ARTICULO (Varios)
				Dim lOperacArt = From opeArt In BBDD.OPERACIONES_DE_UN_ARTICULO
								 Where opeArt.CODIGO = refOrigen
								 Select opeArt
				If (lOperacArt IsNot Nothing) Then
					Dim oOperaArtNew As New Registro.OPERACIONES_DE_UN_ARTICULO
					For Each oOperaArt As Registro.OPERACIONES_DE_UN_ARTICULO In lOperacArt
						oOperaArtNew = New Registro.OPERACIONES_DE_UN_ARTICULO
						oOperaArtNew.CODIGO = refDestino
						oOperaArtNew.COD_OPERACION = oOperaArt.COD_OPERACION
						oOperaArtNew.NUM_OPERACION = oOperaArt.NUM_OPERACION
						oOperaArtNew.VER_PLAN = oOperaArt.VER_PLAN

						BBDD.OPERACIONES_DE_UN_ARTICULO.InsertOnSubmit(oOperaArtNew)
						BBDD.SubmitChanges()
					Next
				End If

				transaction.Commit()
				Return True
			Catch ex As ApplicationException
				transaction.Rollback()
				Throw
			Catch ex As Exception
				transaction.Rollback()
				Throw
			Finally
				BBDD.Connection.Close()
			End Try
		End Function

		''' <summary>
		''' Copia una operacion en otra existente
		''' </summary>
		''' <param name="idAmfeOrigen">Identificador del AMFE que queremos copiar.</param>
		''' <param name="opeOrigen">Codigo de Operacion que queremos copiar.</param>
		''' <param name="opeDestino">Codigo de Operacion donde se copiaran los datos de la operacion origen.</param>
		''' <param name="BBDD">Base de datos donde se realizara la operacion de copiado.</param>
		''' <param name="rutaArchivos">Ruta donde se guardaran los archivos copiados.</param>
		''' <param name="IdPlanta">Identificador de la planta (Base de datos).</param>
		''' <remarks></remarks>
		Public Sub CopiarOperacion(ByVal idAmfeOrigen As Integer, ByVal opeOrigen As String, ByVal opeDestino As String, ByVal BBDD As KaPlanLib.DAL.ELL, ByRef rutaArchivos As String, ByRef IdPlanta As Integer)
			Try

				Dim lCaracteristicas = ListaCaracteristicas(opeOrigen, BBDD) 'ID_CARACTERISTICA, ID_REGISTRO, OrdenCA,OrdenCP

				For Each Caracteristica In lCaracteristicas
					If Caracteristica.ID_CARACTERISTICA Is Nothing OrElse Caracteristica.ID_CARACTERISTICA = 0 Then
						'Si no hay ID_CARACTERISTICA consideramos que la caracteristica solo pertenece a "PLAN DE CONTROL OPERACION"
						CopiarCaracteristica_PLANCONTROL(Nothing, Nothing, Caracteristica.ID_REGISTRO, opeDestino, BBDD)
					ElseIf Caracteristica.ID_CARACTERISTICA IsNot Nothing Then
						'Si hay ID_CARACTERISTICA consideramos que la caracteristica pertenece a "AMFE OPERACION"

						'---------------------------------------------------------------------------------------
						'Comprobamos que exista la caracteristica en "CARACTERISTICAS_AMFE"
						'---------------------------------------------------------------------------------------
						Dim IdCA As Integer = Caracteristica.ID_CARACTERISTICA
						Dim IdCP As Integer = Caracteristica.ID_REGISTRO
						Dim CA As Registro.CARACTERISTICAS_AMFE = (From caract In BBDD.CARACTERISTICAS_AMFE Where caract.ID_CARACTERISTICA = IdCA Select caract).FirstOrDefault
						If CA Is Nothing Then
							Dim CP As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN = (From CPO As Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN Where CPO.ID_REGISTRO = IdCP Select CPO).FirstOrDefault
							Throw New ApplicationException(String.Format("La característica del 'Plan de Control': [Orden=""{0}"", Caracteristica=""{1}""] para la Operación ""{2}"" Y Referenia(s) ""{3}"", no existe en el 'AMFE Operación'. Eliminar la característica del 'Plan de Control' y marcar 'Enviar a Plan' la característica del AMFE Operación que corresponda." _
														 , CP.ORDEN_CARAC, CP.CARAC_PARAM, CP.CODIGO, CP.PLAN_DE_CONTROL.OPERACIONES_TIPO.OPERACIONES_DE_UN_ARTICULO.Select(Function(o) o.CODIGO).Aggregate(Function(current, [next]) current & ", " & [next])))
						End If
						'---------------------------------------------------------------------------------------

						CopiarCaracteristica_AMFEOPERACION(idAmfeOrigen, IdCA, opeOrigen, opeDestino, BBDD)
					End If
				Next

				'--------------------------------------------------------------------------------------------------------------------------
				'FROGA:2013-12-04: Copia de Hoja de Instruccion
				'--------------------------------------------------------------------------------------------------------------------------
				'Dim RutaArchivos As String = ConfigurationManager.AppSettings("rutaArchivos").ToString
				Dim HI As Registro.HOJA_DE_INSTRUCCIONES = (From h As Registro.HOJA_DE_INSTRUCCIONES In BBDD.HOJA_DE_INSTRUCCIONES Where h.CODIGO = opeOrigen Select h).FirstOrDefault
				Dim OperacionesTipo As Registro.OPERACIONES_TIPO = (From OT As Registro.OPERACIONES_TIPO In BBDD.OPERACIONES_TIPO Where OT.COD_OPERACION.Trim = opeDestino.Trim).FirstOrDefault

				If HI IsNot Nothing And OperacionesTipo IsNot Nothing Then
					Dim HINew As New Registro.HOJA_DE_INSTRUCCIONES
					Dim Func As New KaPlanLib.Funciones
					Dim Original, CarpetaArchivos, Copia As String

					'---------------------------------------------------------------------------------
					'Relacionamos la nueva Hoja de Instruccion con la Operacion (Operaciones Tipo).
					'---------------------------------------------------------------------------------
					HINew.OPERACIONES_TIPO = OperacionesTipo
					HINew.CODIGO = OperacionesTipo.COD_OPERACION.Trim
					'---------------------------------------------------------------------------------
					HINew.ACCIONES = HI.ACCIONES
					HINew.CLASE = HI.CLASE
					HINew.EN_RECEP = HI.EN_RECEP
					HINew.FECHA_HI = HI.FECHA_HI
					HINew.MAQUINA_1 = HI.MAQUINA_1
					HINew.MAQUINA_2 = HI.MAQUINA_2
					HINew.MATERIAL = HI.MATERIAL
					HINew.MEDIDAS = HI.MEDIDAS
					HINew.N1 = HI.N1
					HINew.N2 = HI.N2
					HINew.NIVEL_HOJA = HI.NIVEL_HOJA
					HINew.NIVEL_OPERACION = HI.NIVEL_OPERACION
					HINew.Nº_PIEZAS = HI.Nº_PIEZAS
					HINew.NUMERO_DE_OPERACION = HI.NUMERO_DE_OPERACION
					HINew.OBSER_2 = HI.OBSER_2
					HINew.OPERACION = HI.OPERACION
					HINew.TIPO_CONTENEDOR = HI.TIPO_CONTENEDOR
					HINew.TIPO_CROQUIS = HI.TIPO_CROQUIS
					HINew.TiempoCiclo = HI.TiempoCiclo
					'---------------------------------------------------
					'CROQUIS
					'---------------------------------------------------
					HINew.CROQUIS = HI.CROQUIS
                    '#If DEBUG Then
                    'CarpetaArchivos = "Froga"
                    '#Else
                    CarpetaArchivos = "CroquisHojaInstrucciones"
                    '#End If
                    If Not String.IsNullOrWhiteSpace(HI.DIBUJO) Then
						Original = IO.Path.Combine(IO.Path.Combine(rutaArchivos, CarpetaArchivos), HI.DIBUJO)
						If (IO.File.Exists(Original)) Then
							Dim NombreArchivo As String = "¿(" & HINew.CODIGO & ")_" & IdPlanta & "." & HI.DIBUJO.Split(".")(HI.DIBUJO.Split(".").Length - 1)
							'Reemplazamos los caracteres ilegales en el nombre del archivo.
							IO.Path.GetInvalidFileNameChars().ToList().ForEach(Sub(s) NombreArchivo = NombreArchivo.Replace(s, ""))
							Copia = IO.Path.Combine(IO.Path.Combine(rutaArchivos, CarpetaArchivos), NombreArchivo)
							'Comprobamos que no exista la "Copia" pq al copiar daria error. No permite sobreescribir.
							If (IO.File.Exists(Copia)) Then IO.File.Delete(Copia)
							IO.File.Copy(Original, Copia)
							HINew.DIBUJO = NombreArchivo
						Else
							HINew.DIBUJO = Nothing
						End If
					End If
					'---------------------------------------------------

					'-------------------------------------------------------------------------------
					'Tablas secundarias
					'-------------------------------------------------------------------------------
					If HI.PROVEEDORES_POR_HOJA.Any Then
						For Each PH As Registro.PROVEEDORES_POR_HOJA In HI.PROVEEDORES_POR_HOJA
							Dim PHNew As New Registro.PROVEEDORES_POR_HOJA

							PHNew.HOJA_DE_INSTRUCCIONES = HINew
							PHNew.CODIGO = HINew.CODIGO

							PHNew.PROVEEDOR = PH.PROVEEDOR

							BBDD.PROVEEDORES_POR_HOJA.InsertOnSubmit(PHNew)
						Next
					End If
					'-------------------------------------------------------------------------------
					If HI.NIVELES_HOJAS_DE_INSTRUCCIONES.Any Then
						For Each NHI As Registro.NIVELES_HOJAS_DE_INSTRUCCIONES In HI.NIVELES_HOJAS_DE_INSTRUCCIONES
							Dim NHINew As New Registro.NIVELES_HOJAS_DE_INSTRUCCIONES

							NHINew.HOJA_DE_INSTRUCCIONES = HINew
							NHINew.OPERACION = HINew.CODIGO

							NHINew.CALIDAD = NHI.CALIDAD
							NHINew.FECHA = NHI.FECHA
							NHINew.MODIFICACION = NHI.MODIFICACION
							NHINew.NIVEL_HOJA = NHI.NIVEL_HOJA
							NHINew.PRODUCCION = NHI.PRODUCCION
							NHINew.TIPO_SOL_MAQUINA = NHI.TIPO_SOL_MAQUINA
							NHINew.PROCESOS = NHI.PROCESOS

							NHINew.vProduccion = NHI.vProduccion
							NHINew.vCalidad = NHI.vCalidad
							NHINew.vProduccion = NHI.vProduccion

							BBDD.NIVELES_HOJAS_DE_INSTRUCCIONES.InsertOnSubmit(NHINew)
						Next
					End If
					'-------------------------------------------------------------------------------
					If HI.INSTRUCCIONES_DE_TRABAJO.Any Then
						For Each IT As Registro.INSTRUCCIONES_DE_TRABAJO In HI.INSTRUCCIONES_DE_TRABAJO
							Dim ITNew As New Registro.INSTRUCCIONES_DE_TRABAJO

							ITNew.HOJA_DE_INSTRUCCIONES = HINew
							ITNew.OPERACION = HINew.CODIGO

							ITNew.NUMERO = IT.NUMERO
							ITNew.INSTRUCCION = IT.INSTRUCCION

							BBDD.INSTRUCCIONES_DE_TRABAJO.InsertOnSubmit(ITNew)
						Next
					End If
					'-------------------------------------------------------------------------------
					If HI.PARAMETROS_DE_MAQUINAS.Any Then
						For Each PM As Registro.PARAMETROS_DE_MAQUINAS In HI.PARAMETROS_DE_MAQUINAS
							Dim PMNew As New Registro.PARAMETROS_DE_MAQUINAS

							PMNew.HOJA_DE_INSTRUCCIONES = HINew
							PMNew.OPERACION = HINew.CODIGO

							PMNew.ORDEN = PM.ORDEN
							PMNew.CLAVE = PM.CLAVE
							PMNew.DESCRIPCION = PM.DESCRIPCION
							PMNew.CONSIGNA = PM.CONSIGNA
							PMNew.UNIDAD = PM.UNIDAD

							BBDD.PARAMETROS_DE_MAQUINAS.InsertOnSubmit(PMNew)
						Next
					End If
					'-------------------------------------------------------------------------------
					If HI.ORDEN_DE_MONTAJE_DE_PIEZAS.Any Then
						For Each OMP As Registro.ORDEN_DE_MONTAJE_DE_PIEZAS In HI.ORDEN_DE_MONTAJE_DE_PIEZAS
							Dim OMPNew As New Registro.ORDEN_DE_MONTAJE_DE_PIEZAS

							OMPNew.HOJA_DE_INSTRUCCIONES = HINew
							OMPNew.NUMOPER = HINew.CODIGO

							OMPNew.ORDEN = OMP.ORDEN
							OMPNew.CODIGO_PIEZA = OMP.CODIGO_PIEZA
							OMPNew.DENOMINACION = OMP.DENOMINACION

							BBDD.ORDEN_DE_MONTAJE_DE_PIEZAS.InsertOnSubmit(OMPNew)
						Next
					End If
					'-------------------------------------------------------------------------------

					BBDD.HOJA_DE_INSTRUCCIONES.InsertOnSubmit(HINew)
					BBDD.SubmitChanges()
				End If
				'--------------------------------------------------------------------------------------------------------------------------

			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub

		''' <summary>
		''' Copia la caracteristica del "AMFE OPERACION" y la correspondiente del "PLAN DE CONTROL OPERACION".
		''' </summary>
		''' <param name="idAmfeOrigen">Id Amfe origen</param>
		''' <param name="idCaracteristicaOrigen">Id de la caracteristica a copiar</param>
		''' <param name="opeOrigen">Operacion origen</param>
		''' <param name="opeDestino">Operacion destino</param>
		''' <param name="BBDD">Contexto de la conexion (KaPlanLib.DAL.ELL)</param>
		''' <remarks></remarks>
		Public Sub CopiarCaracteristica_AMFEOPERACION(ByVal idAmfeOrigen As Integer, ByVal idCaracteristicaOrigen As Integer, ByVal opeOrigen As String, ByVal opeDestino As String, ByVal BBDD As KaPlanLib.DAL.ELL)
			Try
				Dim idAmfeDestino As Integer = 0
				opeOrigen = opeOrigen.Trim

				'1º Se duplica el Amfe si no tiene ningun registro de codigo OpeDestino            
				Dim Amfes As IQueryable(Of KaPlanLib.Registro.AMFES) = From Amfe In BBDD.AMFES
																	   Where Amfe.CODIGO = opeDestino
																	   Select Amfe

				If Amfes.FirstOrDefault Is Nothing Then
					Dim oAmfesNew As New Registro.AMFES
					oAmfesNew.CODIGO = opeDestino
					oAmfesNew.FECHA_AMFE = Date.Now
					oAmfesNew.NIVEL_AMFE = "0"

					BBDD.AMFES.InsertOnSubmit(oAmfesNew)
					Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
					BBDD.SubmitChanges()

					'Se obtiene el idAmfeDestino insertado
					idAmfeDestino = (From Amfe In BBDD.AMFES
									 Select Amfe.ID).Max
				Else
					Dim myAmfe = (From Amfe In BBDD.AMFES
								  Where Amfe.CODIGO = opeDestino
								  Select Amfe).FirstOrDefault

					'Se obtiene el idAmfe del amfe
					If (myAmfe IsNot Nothing) Then idAmfeDestino = myAmfe.ID
				End If

				If (idAmfeDestino <= 0) Then Throw New ApplicationException("Error al obtener el id del Amfe destino", Nothing)

				'-------------------------------------------------------------------------
				'2º Duplicar "CARACTERISTICAS_AMFE"
				'-------------------------------------------------------------------------
				Dim CA As Registro.CARACTERISTICAS_AMFE = (From caract In BBDD.CARACTERISTICAS_AMFE Where caract.ID_CARACTERISTICA = idCaracteristicaOrigen Select caract).FirstOrDefault
				Dim oCaractNew As New Registro.CARACTERISTICAS_AMFE
				If CA Is Nothing Then
					Throw New ApplicationException("No se ha encontrado la caracteristica CARACTERISTICAS_AMFE.ID_CARACTERISTICA=" & idCaracteristicaOrigen)
				Else
					oCaractNew = New Registro.CARACTERISTICAS_AMFE
					oCaractNew.ID_AMFE = idAmfeDestino
					oCaractNew.ORDEN = CA.ORDEN + 1 'La copia tendrá una posición más en la ordenación.

                    If Not Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                        oCaractNew.CARACTERISTICA = Now & " - " & CA.CARACTERISTICA
                    Else
                        oCaractNew.CARACTERISTICA = CA.CARACTERISTICA
                    End If
                    oCaractNew.ESPECIFICACION = CA.ESPECIFICACION
                        oCaractNew.CLASE = CA.CLASE

                        BBDD.CARACTERISTICAS_AMFE.InsertOnSubmit(oCaractNew)
                        Funciones.Segumiento_Actividad(BBDD, New StackFrame(0).GetMethod().Name)
                        BBDD.SubmitChanges()

                        '-----------------------------------------------------------------
                        'Reorganizar el Orden de "CARACTERISTICAS_AMFE" 
                        '-----------------------------------------------------------------
                        ReorganizarOrden_CARACTERISTICAS_AMFE(oCaractNew.ID_AMFE, BBDD)
                        '----------------------------------------------------------------
                    End If
                    '-------------------------------------------------------------------------

                    '3º Duplicar los MODOS DE FALLO DE LA CARACTERISTICA
                    Dim lModosFallo = From ModosFallo In BBDD.MODOS_DE_FALLO _
				   Where ModosFallo.ID_CARACTERISTICA = idCaracteristicaOrigen _
				   Select ModosFallo

				If (lModosFallo IsNot Nothing) Then
					Dim oModoFalloNew As Registro.MODOS_DE_FALLO
					For Each oModoFallo As Registro.MODOS_DE_FALLO In lModosFallo
						oModoFalloNew = New Registro.MODOS_DE_FALLO
						oModoFalloNew.ID_CARACTERISTICA = oCaractNew.ID_CARACTERISTICA
						oModoFalloNew.MODO_DE_FALLO = oModoFallo.MODO_DE_FALLO

						BBDD.MODOS_DE_FALLO.InsertOnSubmit(oModoFalloNew)
						BBDD.SubmitChanges()
					Next
				End If

				'4º Duplicar AMFE EFECTOS
				Dim lEfectos = From ModosFallo In BBDD.MODOS_DE_FALLO, ModosFallo1 In BBDD.MODOS_DE_FALLO, Efectos In BBDD.EFECTOS _
				  Where ModosFallo.MODO_DE_FALLO = ModosFallo1.MODO_DE_FALLO And ModosFallo.ID_MODO = Efectos.ID_MODO _
				  And ModosFallo.ID_CARACTERISTICA = idCaracteristicaOrigen And ModosFallo1.ID_CARACTERISTICA = oCaractNew.ID_CARACTERISTICA _
				  Select ModosFallo1.ID_MODO, Efectos.EFECTO, Efectos.CLAVE

				If (lEfectos IsNot Nothing) Then
					Dim oEfectoNew As Registro.EFECTOS
					For Each oEfecto In lEfectos
						oEfectoNew = New Registro.EFECTOS
						oEfectoNew.ID_MODO = oEfecto.ID_MODO
						oEfectoNew.EFECTO = oEfecto.EFECTO
						oEfectoNew.CLAVE = oEfecto.CLAVE

						BBDD.EFECTOS.InsertOnSubmit(oEfectoNew)
						BBDD.SubmitChanges()
					Next
				End If

				'5º Duplicar AMFE CAUSAS
				Dim lCausasFallo = From ModosFallo In BBDD.MODOS_DE_FALLO, ModosFallo1 In BBDD.MODOS_DE_FALLO, CausasFallo In BBDD.CAUSAS_DE_FALLO _
				 Where ModosFallo.MODO_DE_FALLO = ModosFallo1.MODO_DE_FALLO And ModosFallo.ID_MODO = CausasFallo.ID_MODO _
				 And ModosFallo.ID_CARACTERISTICA = idCaracteristicaOrigen And ModosFallo1.ID_CARACTERISTICA = oCaractNew.ID_CARACTERISTICA _
				 Select ModosFallo1.ID_MODO, CausasFallo

				If (lCausasFallo IsNot Nothing) Then
					Dim oCausaFalloNew As Registro.CAUSAS_DE_FALLO
					For Each oCausaFallo In lCausasFallo
						oCausaFalloNew = New Registro.CAUSAS_DE_FALLO
						oCausaFalloNew.ID_MODO = oCausaFallo.ID_MODO
						oCausaFalloNew.CAUSA = oCausaFallo.CausasFallo.CAUSA
						oCausaFalloNew.CONTROLES = oCausaFallo.CausasFallo.CONTROLES
						oCausaFalloNew.FRECUENCIA_DE_CONTROL = oCausaFallo.CausasFallo.FRECUENCIA_DE_CONTROL
						oCausaFalloNew.FRECUENCIA_DE_CONTROL_CAL = oCausaFallo.CausasFallo.FRECUENCIA_DE_CONTROL_CAL
						oCausaFalloNew.OCURRENCIA = oCausaFallo.CausasFallo.OCURRENCIA
						oCausaFalloNew.GRAVEDAD = oCausaFallo.CausasFallo.GRAVEDAD
						oCausaFalloNew.DETECCION = oCausaFallo.CausasFallo.DETECCION
						oCausaFalloNew.ACCION_RECOMENDADA = oCausaFallo.CausasFallo.ACCION_RECOMENDADA
						oCausaFalloNew.NUEVO_MEDIO_COTROL = oCausaFallo.CausasFallo.NUEVO_MEDIO_COTROL
						oCausaFalloNew.DEPARTAMENTO = oCausaFallo.CausasFallo.DEPARTAMENTO
						oCausaFalloNew.FECHA_DE_INICIO = oCausaFallo.CausasFallo.FECHA_DE_INICIO
						oCausaFalloNew.FECHA_DE_FIN = oCausaFallo.CausasFallo.FECHA_DE_FIN
						oCausaFalloNew.PROGRESO = oCausaFallo.CausasFallo.PROGRESO
						oCausaFalloNew.EFECTO_DE_LA_ACCION = oCausaFallo.CausasFallo.EFECTO_DE_LA_ACCION
						oCausaFalloNew.NUEVA_OCURRENCIA = oCausaFallo.CausasFallo.NUEVA_OCURRENCIA
						oCausaFalloNew.NUEVA_GRAVEDAD = oCausaFallo.CausasFallo.NUEVA_GRAVEDAD
						oCausaFalloNew.NUEVA_DETECCION = oCausaFallo.CausasFallo.NUEVA_DETECCION
						oCausaFalloNew.M_CONTROL = oCausaFallo.CausasFallo.M_CONTROL
						oCausaFalloNew.M_FABRICAC = oCausaFallo.CausasFallo.M_FABRICAC
						oCausaFalloNew.M_HUMAN = oCausaFallo.CausasFallo.M_HUMAN
						oCausaFalloNew.ENVIAR_A_PLAN = oCausaFallo.CausasFallo.ENVIAR_A_PLAN
						oCausaFalloNew._EN_PLAN_ = oCausaFallo.CausasFallo._EN_PLAN_
						oCausaFalloNew.NOMBRE_MEDIO = oCausaFallo.CausasFallo.NOMBRE_MEDIO
						oCausaFalloNew.NOMBRE_MEDIO_FAB = oCausaFallo.CausasFallo.NOMBRE_MEDIO_FAB
						oCausaFalloNew.Codigo_Control_ME_FAB = oCausaFallo.CausasFallo.Codigo_Control_ME_FAB
						oCausaFalloNew.Tamaño_OP = oCausaFallo.CausasFallo.Tamaño_OP
						oCausaFalloNew.Tamaño_CAL = oCausaFallo.CausasFallo.Tamaño_CAL
						oCausaFalloNew.PREVENTIVA_MEDIO_FAB = oCausaFallo.CausasFallo.PREVENTIVA_MEDIO_FAB
						oCausaFalloNew.NOMBRE_MEDIO_HUM = oCausaFallo.CausasFallo.NOMBRE_MEDIO_HUM

						oCausaFalloNew.METODO_CONTROL = oCausaFallo.CausasFallo.METODO_CONTROL
						oCausaFalloNew.METODO_CONTROL_FAB = oCausaFallo.CausasFallo.METODO_CONTROL_FAB

						BBDD.CAUSAS_DE_FALLO.InsertOnSubmit(oCausaFalloNew)
						BBDD.SubmitChanges()

						'----------------------------------------------------------------------------------------------
						'4.1º Duplicar AMFE_CARACTERISTICAS_PLAN
						'----------------------------------------------------------------------------------------------
						'If ID_REGISTRO IsNot Nothing AndAlso (oCausaFalloNew.ENVIAR_A_PLAN = "Sí" Or oCausaFalloNew.ENVIAR_A_PLAN = "Si") Then
						'	'CopiarCaracteristica_PLANCONTROL(oCaractNew.ID_CARACTERISTICA, oCausaFalloNew.CONT_CAUSA, ID_REGISTRO, opeOrigen, opeDestino, BBDD)
						'	CopiarCaracteristica_PLANCONTROL(oCaractNew.ID_CARACTERISTICA, oCausaFalloNew.CONT_CAUSA, ID_REGISTRO, opeDestino, BBDD)
						'End If
						'----------------------------------------------------------------------------------------------
						'FROGA:2013-08-28:
						'----------------------------------------------------------------------------------------------
						'4.1º Duplicar AMFE_CARACTERISTICAS_PLAN
						'----------------------------------------------------------------------------------------------
						'Comprobamos que la caracteristica del AMFE (CARACTERISTICAS_AMFE) para esta causa (CAUSAS_DE_FALLO)
						'tiene una caracateristica en el plan (CARACTERISTICAS_DEL_PLAN) para copiar.
						'----------------------------------------------------------------------------------------------
						'Dim ID_Registro As Nullable(Of Integer) = (From CP As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN _
						'                                           Where CP.ID_CARACTERISTICA = idCaracteristicaOrigen _
						'                                           And CP.CONT_CAUSA = oCausaFallo.CausasFallo.CONT_CAUSA _
						'                                           Select CType(CP.ID_REGISTRO, Nullable(Of Integer)) Distinct).SingleOrDefault
						'If ID_Registro IsNot Nothing AndAlso (oCausaFalloNew.ENVIAR_A_PLAN = "Sí" Or oCausaFalloNew.ENVIAR_A_PLAN = "Si") Then
						'    CopiarCaracteristica_PLANCONTROL(oCaractNew.ID_CARACTERISTICA, oCausaFalloNew.CONT_CAUSA, ID_Registro, opeDestino, BBDD)
						'End If
						'------------------------------------------------------------------------------------------------------------------------------
						'FROGA: 2013-12-16: Por errores en la base de datos existen  "CARACTERISTICAS_DEL_PLAN" duplicadas.
						'Comprobamos que la caracterisitca a copiar sea unica. Si no es así avisamos al usuario para que compruebe las caracteristicas.
						'------------------------------------------------------------------------------------------------------------------------------
						Dim lCP As IQueryable(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN) = From CP As KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN _
																   Where CP.ID_CARACTERISTICA = idCaracteristicaOrigen _
																   And CP.CONT_CAUSA = oCausaFallo.CausasFallo.CONT_CAUSA _
																   Select CP
						If lCP.Any Then
							If lCP.Count = 1 Then
								Dim CP As Registro.CARACTERISTICAS_DEL_PLAN = lCP.FirstOrDefault
								If (oCausaFalloNew.ENVIAR_A_PLAN = "Sí" Or oCausaFalloNew.ENVIAR_A_PLAN = "Si") Then
									CopiarCaracteristica_PLANCONTROL(oCaractNew.ID_CARACTERISTICA, oCausaFalloNew.CONT_CAUSA, CP.ID_REGISTRO, opeDestino, BBDD)
								End If
							ElseIf lCP.Count >= 2 Then
								Dim Mensaje As String = "Caracateristicas del Plan de Control Operacion Duplicadas. Compruebe que las operaciones con orden ""{0}"" tengan su correspondencia en el AMFE Operacion."
								Dim Orden As String = String.Empty
								For Each CP As Registro.CARACTERISTICAS_DEL_PLAN In lCP
									If String.IsNullOrWhiteSpace(Orden) = False Then Orden &= ", "
									Orden &= CP.ORDEN_CARAC
								Next
								Throw New ApplicationException(String.Format(Mensaje, Orden))
							End If
						End If
						'------------------------------------------------------------------------------------------------------------------------------
					Next
				End If
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				Dim msg As String = String.Format("opeOrigen: {0}", opeOrigen)
				log.Error(msg, ex)
				Throw
			End Try
		End Sub

		''' <summary>
		''' Copia la caracteristica del "PLAN DE CONTROL OPERACION".
		''' </summary>
		''' <param name="ID_CARAC_AMFE">Identificador de la caracteristica en el "AMFE OPERACION" (CARACTERISTICAS_AMFE.ID_CARACTERISTICA) con la que se relacionara la copia de la caracteristica del "PLAN DE CONTROL OPERACION".</param>
		''' <param name="Cont_Causa">Identificador de "CAUSAS_DE_FALLO" (CAUSAS_DE_FALLO.CONT_CAUSA) con la que se relacionara la copia de la caracteristica del "PLAN DE CONTROL OPERACION".</param>
		''' <param name="ID_REGISTRO">Identificador de la caracteristica de "PLAN DE CONTROL OPERACION" que se va a duplicar.</param>
		''' <param name="opeDestino">Operacion destino para realizar la copia de caracteristica. Si es la misma que la de origen se duplica la caracteirstica.</param>
		''' <param name="BBDD">Contexto de la conexion (KaPlanLib.DAL.ELL)</param>
		''' <remarks></remarks>
		'''Public Sub CopiarCaracteristica_PLANCONTROL(ByVal ID_CARAC_AMFE As Nullable(Of Integer), ByVal Cont_Causa As Nullable(Of Integer), ByVal ID_REGISTRO As Integer, ByVal opeOrigen As String, ByVal opeDestino As String, ByVal BBDD As KaPlanLib.DAL.ELL)
		Public Sub CopiarCaracteristica_PLANCONTROL(ByVal ID_CARAC_AMFE As Nullable(Of Integer), ByVal Cont_Causa As Nullable(Of Integer), ByVal ID_REGISTRO As Integer, ByVal opeDestino As String, ByVal BBDD As KaPlanLib.DAL.ELL)
			'--------------------------------------------------------------------------------
			'1-Duplicar el PLAN DE CONTROL
			'--------------------------------------------------------------------------------
			Dim PLAN_DE_CONTROL As Registro.PLAN_DE_CONTROL = (From planC In BBDD.PLAN_DE_CONTROL _
			  Where planC.CODIGO = opeDestino _
			  Select planC).SingleOrDefault

			If PLAN_DE_CONTROL Is Nothing Then
				Dim oPlanControlNew As New Registro.PLAN_DE_CONTROL
				oPlanControlNew.CODIGO = opeDestino
				oPlanControlNew.FECHA = Date.Now
				oPlanControlNew.NIVEL = "0"

				BBDD.PLAN_DE_CONTROL.InsertOnSubmit(oPlanControlNew)
				BBDD.SubmitChanges()
			End If
			'--------------------------------------------------------------------------------

			'--------------------------------------------------------------------------------
			'2-Duplicar AMFE_CARACTERISTICAS_PLAN
			'--------------------------------------------------------------------------------

			'Dim lCaracteristicasPlan = From CaractPlan In BBDD.CARACTERISTICAS_DEL_PLAN _
			'  Where CaractPlan.CODIGO = opeOrigen And CaractPlan.ID_CARACTERISTICA = ID_CARAC_PLAN _
			'  Select CaractPlan Order By CaractPlan.ORDEN_CARAC
			'Dim oCaractPlan As Registro.CARACTERISTICAS_DEL_PLAN = (From CaractPlan In BBDD.CARACTERISTICAS_DEL_PLAN _
			'  Where CaractPlan.CODIGO = opeOrigen And CaractPlan.ID_REGISTRO = ID_REGISTRO _
			'  Select CaractPlan).SingleOrDefault
			Dim oCaractPlan As Registro.CARACTERISTICAS_DEL_PLAN = (From CaractPlan In BBDD.CARACTERISTICAS_DEL_PLAN _
			  Where CaractPlan.ID_REGISTRO = ID_REGISTRO _
			  Select CaractPlan).SingleOrDefault
			'If (lCaracteristicasPlan IsNot Nothing) Then
			If (oCaractPlan IsNot Nothing) Then
				Dim oCaractPlanNew As Registro.CARACTERISTICAS_DEL_PLAN
				'For Each oCaractPlan As Registro.CARACTERISTICAS_DEL_PLAN In lCaracteristicasPlan
				oCaractPlanNew = New Registro.CARACTERISTICAS_DEL_PLAN
				oCaractPlanNew.PROCEDE_DE = oCaractPlan.PROCEDE_DE
				oCaractPlanNew.CODIGO = opeDestino
				oCaractPlanNew.MAQUINA = oCaractPlan.MAQUINA
				oCaractPlanNew.POSICION = oCaractPlan.POSICION
				oCaractPlanNew.ORDEN_CARAC = oCaractPlan.ORDEN_CARAC + 1 'La copia tendrá una posición más en la ordenación.

                If Not Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus").ToLower.Equals("live") Then
                    oCaractPlanNew.CARAC_PARAM = ID_CARAC_AMFE & " - " & oCaractPlan.CARAC_PARAM
                Else
                    oCaractPlanNew.CARAC_PARAM = oCaractPlan.CARAC_PARAM
                End If
                oCaractPlanNew.MAXIM = oCaractPlan.MAXIM
                    oCaractPlanNew.MINIM = oCaractPlan.MINIM
                    oCaractPlanNew.ESPECIFICACION = oCaractPlan.ESPECIFICACION
                    oCaractPlanNew.FRECUENCIA_CONTROL = oCaractPlan.FRECUENCIA_CONTROL
                    oCaractPlanNew.FRECUENCIA_CONTROL_CAL = oCaractPlan.FRECUENCIA_CONTROL_CAL
                    oCaractPlanNew.FRECUENCIA_REGISTRO = oCaractPlan.FRECUENCIA_REGISTRO
                    oCaractPlanNew.ID_Doc_Registro = oCaractPlan.ID_Doc_Registro
                    oCaractPlanNew.TAMAÑO = oCaractPlan.TAMAÑO
                    oCaractPlanNew.TAMAÑO_CAL = oCaractPlan.TAMAÑO_CAL
                    oCaractPlanNew.METODO_EVALUACION = oCaractPlan.METODO_EVALUACION
                    oCaractPlanNew.RESPONSABLE = oCaractPlan.RESPONSABLE
                    oCaractPlanNew.MEDIO_DENOMINACION = oCaractPlan.MEDIO_DENOMINACION

                    oCaractPlanNew.Control_DET_ME_CTRL = oCaractPlan.Control_DET_ME_CTRL
                    oCaractPlanNew.Control_DET_ME_FAB = oCaractPlan.Control_DET_ME_FAB
                    oCaractPlanNew.Codigo_Control_ME_FAB = oCaractPlan.Codigo_Control_ME_FAB

                    oCaractPlanNew.MEDIO_RFA = oCaractPlan.MEDIO_RFA
                    oCaractPlanNew.CLASE = oCaractPlan.CLASE
                    oCaractPlanNew.OBSERVACIONES = oCaractPlan.OBSERVACIONES
                    oCaractPlanNew.ACCION_RECOMENDADA = oCaractPlan.ACCION_RECOMENDADA

                    oCaractPlanNew.CONT_CAUSA = Cont_Causa

                    oCaractPlanNew.ID_CARACTERISTICA = ID_CARAC_AMFE
                    oCaractPlanNew.HOJA_REGISTROS = oCaractPlan.HOJA_REGISTROS

                    oCaractPlanNew.VER_REG_PRO = oCaractPlan.VER_REG_PRO
                    oCaractPlanNew.VER_REG_REC = oCaractPlan.VER_REG_REC
                    oCaractPlanNew.VER_REG_DIM = oCaractPlan.VER_REG_DIM
                    oCaractPlanNew.VER_REG_FUN = oCaractPlan.VER_REG_FUN
                    oCaractPlanNew.VER_REG_MAT = oCaractPlan.VER_REG_MAT

                    oCaractPlanNew.OPERACION = oCaractPlan.OPERACION
                    oCaractPlanNew.PROCESO_PRODUCTO = oCaractPlan.PROCESO_PRODUCTO

                    oCaractPlanNew.Responsable_Maquina = oCaractPlan.Responsable_Maquina
                    oCaractPlanNew.Responsable_Operario = oCaractPlan.Responsable_Operario
                    oCaractPlanNew.Responsable_Calidad = oCaractPlan.Responsable_Calidad

                    '--------------------------------------------------------------------------------------------------------
                    'Responsables de Registro.
                    '--------------------------------------------------------------------------------------------------------
                    oCaractPlanNew.RESPONSABLE_REGISTRO = oCaractPlan.RESPONSABLE_REGISTRO 'Campo Obsoleto. No usar para meter datos.
                    oCaractPlanNew.Responsable_Reg_Ope = oCaractPlan.Responsable_Reg_Ope : oCaractPlanNew.Responsable_Reg_Gestor = oCaractPlan.Responsable_Reg_Gestor
                    oCaractPlanNew.Responsable_Reg_Cal = oCaractPlan.Responsable_Reg_Cal
                    '--------------------------------------------------------------------------------------------------------

                    AyudaVisualCopiar(oCaractPlan, oCaractPlanNew)

                    oCaractPlanNew.METODO_CONTROL = oCaractPlan.METODO_CONTROL
                    oCaractPlanNew.METODO_CONTROL_FAB = oCaractPlan.METODO_CONTROL_FAB

                    BBDD.CARACTERISTICAS_DEL_PLAN.InsertOnSubmit(oCaractPlanNew)
                    BBDD.SubmitChanges()

                    '-----------------------------------------------------------------------------------------------------------------------------
                    'Reorganizar el Orden de "CARACTERISTICAS_DEL_PLAN" (NO incluido el campo Posicion).
                    '-----------------------------------------------------------------------------------------------------------------------------
                    ReorganizarOrden_CARACTERISTICAS_DEL_PLAN(opeDestino, BBDD)
                    '-----------------------------------------------------------------------------------------------------------------------------
                    'Next
                End If
            '--------------------------------------------------------------------------------
        End Sub

		''' <summary>
		''' Buscamos todas la caracteristicas de "AMFE OPERACION" y "PLAN DE CONTROL OPERACION".
		''' </summary>
		''' <param name="opeOrigen">Operacion de la que obtenemos las caracteristicas</param>
		''' <param name="BBDD">Contexto de la conexion (KaPlanLib.DAL.ELL)</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function ListaCaracteristicas(ByVal opeOrigen As String, ByVal BBDD As KaPlanLib.DAL.ELL)
            Try
                '---------------------------------------------------------------------------------------
                'Full Outer Join: Buscamos todas la caracteristicas de "AMFE OPERACION" y "PLAN DE CONTROL OPERACION".
                'Tomamos el orden del Plan de Control como referencia
                '---------------------------------------------------------------------------------------
                Dim lCA = From CAUSASDEFALLO As KaPlanLib.Registro.CAUSAS_DE_FALLO In BBDD.CAUSAS_DE_FALLO
                          Join MODOSDEFALLO As KaPlanLib.Registro.MODOS_DE_FALLO In BBDD.MODOS_DE_FALLO On CAUSASDEFALLO.ID_MODO Equals MODOSDEFALLO.ID_MODO
                          Join CARACTERISTICASAMFE In BBDD.CARACTERISTICAS_AMFE On MODOSDEFALLO.ID_CARACTERISTICA Equals CARACTERISTICASAMFE.ID_CARACTERISTICA
                          Join AMFES In BBDD.AMFES On CARACTERISTICASAMFE.ID_AMFE Equals AMFES.ID
                          Where AMFES.CODIGO = opeOrigen
                          Select CAUSASDEFALLO

                Dim lCP = From CARACTERISTICASDELPLAN In BBDD.CARACTERISTICAS_DEL_PLAN
                          Where CARACTERISTICASDELPLAN.CODIGO = opeOrigen
                          Order By CARACTERISTICASDELPLAN.ORDEN_CARAC, CARACTERISTICASDELPLAN.ID_REGISTRO
                          Select CARACTERISTICASDELPLAN
                Dim lCaracteristicas = (From ca In lCA
                                        Group Join cp In lCP On ca.MODOS_DE_FALLO.ID_CARACTERISTICA Equals cp.ID_CARACTERISTICA
                                        Into HaveMatch = Any()
                                        Where Not HaveMatch
                                        Select ID_CARACTERISTICA = ca.MODOS_DE_FALLO.ID_CARACTERISTICA _
                                               , ID_REGISTRO = New Nullable(Of Decimal) _
                                               , Orden = If(ca.MODOS_DE_FALLO.CARACTERISTICAS_AMFE.ORDEN Is Nothing, New Integer, CInt(ca.MODOS_DE_FALLO.CARACTERISTICAS_AMFE.ORDEN))
                                        Distinct) _
                                .Concat(From ca In lCA
                                        Join cp In lCP On ca.MODOS_DE_FALLO.ID_CARACTERISTICA Equals cp.ID_CARACTERISTICA
                                        Select ID_CARACTERISTICA = ca.MODOS_DE_FALLO.ID_CARACTERISTICA _
                                           , ID_REGISTRO = CType(cp.ID_REGISTRO, Nullable(Of Decimal)) _
                                           , Orden = If(ca.MODOS_DE_FALLO.CARACTERISTICAS_AMFE.ORDEN Is Nothing, New Integer, CInt(ca.MODOS_DE_FALLO.CARACTERISTICAS_AMFE.ORDEN))
                                        Distinct) _
                                .Concat(From cp In lCP Group Join ca In lCA On ca.MODOS_DE_FALLO.ID_CARACTERISTICA Equals cp.ID_CARACTERISTICA
                                Into HaveMatch = Any()
                                        Where Not HaveMatch
                                        Select ID_CARACTERISTICA = CType(cp.ID_CARACTERISTICA, Nullable(Of Integer)) _
                                        , ID_REGISTRO = CType(cp.ID_REGISTRO, Nullable(Of Decimal)) _
                                        , Orden = If(cp.ORDEN_CARAC Is Nothing, New Integer, CInt(cp.ORDEN_CARAC))
                                        Distinct).OrderBy(Function(o) o.Orden)
                '---------------------------------------------------------------------------------------
                '#If DEBUG Then
                'Throw New ApplicationException("Copia cancelada")
                '#End If
                Return lCaracteristicas
            Catch ex As ApplicationException
                Throw
            Catch ex As Exception
                Throw
            End Try
        End Function
#End Region

#Region "NIVEL_HOJA_INSTRUCCIONES"
		''' <summary>
		''' Guarda el nivel de la hoja de instrucciones
		''' </summary>
		''' <param name="oNivel">Datos del nivel</param>                        
		''' <remarks></remarks>
		Public Sub GuardarNivelHojaInstrucciones(ByVal oNivel As Registro.NIVELES_HOJAS_DE_INSTRUCCIONES)
			'Public Function GuardarNivelHojaInstrucciones(ByVal oNivel As Registro.NIVELES_HOJAS_DE_INSTRUCCIONES) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oNivel.ID_REGISTRO = 0) Then 'es nuevo: insert
					BBDD.NIVELES_HOJAS_DE_INSTRUCCIONES.InsertOnSubmit(oNivel)
					BBDD.SubmitChanges()
				Else
					Dim nivHoj As KaPlanLib.Registro.NIVELES_HOJAS_DE_INSTRUCCIONES = (From nivHojaInstr In BBDD.NIVELES_HOJAS_DE_INSTRUCCIONES _
					   Where nivHojaInstr.ID_REGISTRO = oNivel.ID_REGISTRO _
					   Select nivHojaInstr).FirstOrDefault
					'If (nivHoj Is Nothing) Then Return False
					If (nivHoj Is Nothing) Then Throw New ApplicationException("Error al cargar el ""Nivel de Hojas de Instrucciones"" para el ID " & oNivel.ID_REGISTRO)

					nivHoj.NIVEL_HOJA = oNivel.NIVEL_HOJA
					nivHoj.FECHA = oNivel.FECHA
					nivHoj.MODIFICACION = oNivel.MODIFICACION
					nivHoj.OPERACION = oNivel.OPERACION
					nivHoj.CALIDAD = oNivel.CALIDAD
					nivHoj.PROCESOS = oNivel.PROCESOS
					nivHoj.PRODUCCION = oNivel.PRODUCCION
					nivHoj.TIPO_SOL_MAQUINA = oNivel.TIPO_SOL_MAQUINA

					nivHoj.vProcesos = oNivel.vProcesos
					nivHoj.vCalidad = oNivel.vCalidad
					nivHoj.vProduccion = oNivel.vProduccion

					BBDD.SubmitChanges()
				End If
			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub

        ''' <summary>
        ''' Elimina un nivel de la hoja de instrucciones
        ''' </summary>
        ''' <param name="idRegistro">Id registro del nivel</param>        
        ''' <remarks></remarks>
        Public Sub EliminarNivelHojaInstrucciones(ByVal idRegistro As Integer)
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL
                Dim NivelHojaInstru = From NivelHoja In BBDD.NIVELES_HOJAS_DE_INSTRUCCIONES Where NivelHoja.ID_REGISTRO = idRegistro Select NivelHoja
                If NivelHojaInstru.Any Then
                    BBDD.NIVELES_HOJAS_DE_INSTRUCCIONES.DeleteOnSubmit(NivelHojaInstru.First)
                    BBDD.SubmitChanges()
                End If
            Catch ex As Exception
                Throw
            End Try
        End Sub
#End Region

#Region "PROVEEDORES_HOJA"

        ''' <summary>
        ''' Guarda el proveedor de la hoja de instrucciones
        ''' </summary>
        ''' <param name="oProvHoja">Datos del proveedor</param>                        
        ''' <remarks></remarks>
        Public Sub GuardarProveedorHoja(ByVal oProvHoja As Registro.PROVEEDORES_POR_HOJA)
            Dim BBDD As New KaPlanLib.DAL.ELL
            Try
                If (oProvHoja.ID_REGISTRO = 0) Then 'es nuevo: insert
                    BBDD.PROVEEDORES_POR_HOJA.InsertOnSubmit(oProvHoja)
                    BBDD.SubmitChanges()
                Else
                    Dim provHoj As KaPlanLib.Registro.PROVEEDORES_POR_HOJA = (From proveHoja In BBDD.PROVEEDORES_POR_HOJA Where proveHoja.ID_REGISTRO = oProvHoja.ID_REGISTRO Select proveHoja).FirstOrDefault
                    If (provHoj IsNot Nothing) Then
                        provHoj.CODIGO = oProvHoja.CODIGO
                        provHoj.PROVEEDOR = oProvHoja.PROVEEDOR
                        BBDD.SubmitChanges()
                    End If
                End If
            Catch ex As Exception
                Throw
            End Try
        End Sub

        ''' <summary>
        ''' Elimina un proveedor de la hoja de instrucciones
        ''' </summary>
        ''' <param name="idRegistro">Id registro del nivel</param>        
        ''' <remarks></remarks>
        Public Sub EliminarProveedorHoja(ByVal idRegistro As Integer)
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL
                Dim provHoja = From proveHoja In BBDD.PROVEEDORES_POR_HOJA Where proveHoja.ID_REGISTRO = idRegistro Select proveHoja
                If provHoja.Any Then
                    BBDD.PROVEEDORES_POR_HOJA.DeleteOnSubmit(provHoja.First)
                    BBDD.SubmitChanges()
                End If
            Catch ex As Exception
                Throw
            End Try
        End Sub

#End Region

#Region "HOJA DE INTRUCCIONES"
        ''' <summary>
        ''' Devuelve el registro de la hoja de instrucciones
        ''' </summary>
        ''' <param name="cod">Codigo</param>
        ''' <returns></returns>
        Function consultarHojaInstrucciones(ByVal cod As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim hojaInstr = From hoja In BBDD.HOJA_DE_INSTRUCCIONES _
				Where hoja.CODIGO = cod _
				Select hoja

				If (Not hojaInstr.Any) Then
					Return Nothing
				Else
					Return hojaInstr.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la hoja de instrucciones
		''' </summary>
		''' <param name="hojaInst">Plan de seguimiento</param>        
		''' <remarks></remarks>
		Public Sub GuardarHojaInstrucciones(ByVal hojaInst As Registro.HOJA_DE_INSTRUCCIONES)
			Dim transaction As DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				'-----------------------------------------------------------------------------------------------------------------------------------------------
				'1º Se guardan los datos de la hoja de instrucciones
				'-----------------------------------------------------------------------------------------------------------------------------------------------
				Dim HojaInstrucciones As Registro.HOJA_DE_INSTRUCCIONES = (From hoja In BBDD.HOJA_DE_INSTRUCCIONES _
				   Where hoja.CODIGO = hojaInst.CODIGO _
				   Select hoja).SingleOrDefault
				If (HojaInstrucciones Is Nothing) Then	'si no devuelve resultados, se hara un insert
					HojaInstrucciones = hojaInst
					HojaInstrucciones.FECHA_HI = Now 'Guardamos la fehca de creacion de la Hoja de Instruccion.
					BBDD.HOJA_DE_INSTRUCCIONES.InsertOnSubmit(hojaInst)
				Else  'Sino se actualiza el registro con los nuevos datos
					'HojaInstrucciones.CODIGO = hojaInst.CODIGO 'Este campo es la clave primaria y no se puede modificar.
					'HojaInstrucciones.FECHA_HI = hojaInst.FECHA_HI
					HojaInstrucciones.ACCIONES = hojaInst.ACCIONES
					HojaInstrucciones.EN_RECEP = hojaInst.EN_RECEP
					HojaInstrucciones.CLASE = hojaInst.CLASE
					HojaInstrucciones.MAQUINA_1 = hojaInst.MAQUINA_1
					HojaInstrucciones.MAQUINA_2 = hojaInst.MAQUINA_2
					HojaInstrucciones.MATERIAL = hojaInst.MATERIAL
					HojaInstrucciones.MEDIDAS = hojaInst.MEDIDAS
					HojaInstrucciones.N1 = hojaInst.N1
					HojaInstrucciones.N2 = hojaInst.N2
					HojaInstrucciones.Nº_PIEZAS = hojaInst.Nº_PIEZAS
					HojaInstrucciones.OBSER_2 = hojaInst.OBSER_2
					HojaInstrucciones.TIPO_CONTENEDOR = hojaInst.TIPO_CONTENEDOR
					HojaInstrucciones.DIBUJO = hojaInst.DIBUJO
					HojaInstrucciones.TIPO_CROQUIS = hojaInst.TIPO_CROQUIS
					HojaInstrucciones.NIVEL_HOJA = hojaInst.NIVEL_HOJA
					HojaInstrucciones.NIVEL_OPERACION = hojaInst.NIVEL_OPERACION
					HojaInstrucciones.TiempoCiclo = hojaInst.TiempoCiclo
					BBDD.SubmitChanges()
				End If
				'-----------------------------------------------------------------------------------------------------------------------------------------------

				'-----------------------------------------------------------------------------------------------------------------------------------------------
				'2º Eliminar hoja fabricacion
				'-----------------------------------------------------------------------------------------------------------------------------------------------
				Dim hojaInstFab As IQueryable(Of KaPlanLib.Registro.HOJA_DE_INSTRUCCIONES_FABRICACION) = From hoja In BBDD.HOJA_DE_INSTRUCCIONES_FABRICACION _
				   Where hoja.CODIGO = hojaInst.CODIGO _
				   Select hoja
				If hojaInstFab IsNot Nothing AndAlso (hojaInstFab.Count = 1) Then
					BBDD.HOJA_DE_INSTRUCCIONES_FABRICACION.DeleteOnSubmit(hojaInstFab.First)
					BBDD.SubmitChanges()
				End If
				'-----------------------------------------------------------------------------------------------------------------------------------------------

				'-----------------------------------------------------------------------------------------------------------------------------------------------
				'4º Copiar a HOJA DE INSTRUCCIONES FABRICACION
				'-----------------------------------------------------------------------------------------------------------------------------------------------
				Dim oHojaInstrFab As New Registro.HOJA_DE_INSTRUCCIONES_FABRICACION
				oHojaInstrFab.CODIGO = HojaInstrucciones.CODIGO
				oHojaInstrFab.OPERACION = HojaInstrucciones.OPERACION
				oHojaInstrFab.NIVEL_HOJA = HojaInstrucciones.NIVEL_HOJA
				oHojaInstrFab.DIBUJO = HojaInstrucciones.DIBUJO
				oHojaInstrFab.CROQUIS = HojaInstrucciones.CROQUIS
				oHojaInstrFab.NUMERO_DE_OPERACION = HojaInstrucciones.NUMERO_DE_OPERACION
				oHojaInstrFab.MAQUINA_1 = HojaInstrucciones.MAQUINA_1
				oHojaInstrFab.MAQUINA_2 = HojaInstrucciones.MAQUINA_2
				oHojaInstrFab.N1 = HojaInstrucciones.N1
				oHojaInstrFab.N2 = HojaInstrucciones.N2
				oHojaInstrFab.OBSER_2 = HojaInstrucciones.OBSER_2
				oHojaInstrFab.TIPO_CROQUIS = HojaInstrucciones.TIPO_CROQUIS
				oHojaInstrFab.MATERIAL = HojaInstrucciones.MATERIAL
				oHojaInstrFab.MEDIDAS = HojaInstrucciones.MEDIDAS
				oHojaInstrFab.ACCIONES = HojaInstrucciones.ACCIONES
				oHojaInstrFab.FECHA_HI = HojaInstrucciones.FECHA_HI
				oHojaInstrFab.NIVEL_OPERACION = HojaInstrucciones.NIVEL_OPERACION
				oHojaInstrFab.TIPO_CONTENEDOR = HojaInstrucciones.TIPO_CONTENEDOR
				oHojaInstrFab.Nº_PIEZAS = HojaInstrucciones.Nº_PIEZAS
				oHojaInstrFab.CLASE = HojaInstrucciones.CLASE
				oHojaInstrFab.TiempoCiclo = HojaInstrucciones.TiempoCiclo

				BBDD.HOJA_DE_INSTRUCCIONES_FABRICACION.InsertOnSubmit(oHojaInstrFab)
				BBDD.SubmitChanges()
				'-----------------------------------------------------------------------------------------------------------------------------------------------

				'-----------------------------------------------------------------------------------------------------------------------------------------------
				'5º Copiar a NIVELES HOJAS DE INSTRUCCIONES FABRICACION
				'-----------------------------------------------------------------------------------------------------------------------------------------------
				Dim NivHojInstr = From nivHoj In BBDD.NIVELES_HOJAS_DE_INSTRUCCIONES _
				 Where nivHoj.OPERACION = hojaInst.CODIGO _
				 Select nivHoj
				If (NivHojInstr IsNot Nothing) Then
					Dim nivHojInstFabNew As Registro.NIVELES_HOJAS_DE_INSTRUCCIONES_FABRICACION
					For Each oNivHoja As Registro.NIVELES_HOJAS_DE_INSTRUCCIONES In NivHojInstr
						nivHojInstFabNew = New Registro.NIVELES_HOJAS_DE_INSTRUCCIONES_FABRICACION
						nivHojInstFabNew.OPERACION = oNivHoja.OPERACION
						nivHojInstFabNew.TIPO_SOL_MAQUINA = oNivHoja.TIPO_SOL_MAQUINA
						nivHojInstFabNew.NIVEL_HOJA = oNivHoja.NIVEL_HOJA
						nivHojInstFabNew.FECHA = oNivHoja.FECHA
						nivHojInstFabNew.MODIFICACION = oNivHoja.MODIFICACION
						nivHojInstFabNew.PROCESOS = oNivHoja.PROCESOS
						nivHojInstFabNew.CALIDAD = oNivHoja.CALIDAD
						nivHojInstFabNew.PRODUCCION = oNivHoja.PRODUCCION

						nivHojInstFabNew.vProduccion = oNivHoja.vProduccion
						nivHojInstFabNew.vCalidad = oNivHoja.vCalidad
						nivHojInstFabNew.vProduccion = oNivHoja.vProduccion

						BBDD.NIVELES_HOJAS_DE_INSTRUCCIONES_FABRICACION.InsertOnSubmit(nivHojInstFabNew)
					Next
					If (NivHojInstr.Any) Then BBDD.SubmitChanges()
				End If
				'-----------------------------------------------------------------------------------------------------------------------------------------------

				'-----------------------------------------------------------------------------------------------------------------------------------------------
				'6º Copiar a INSTRUCCIONES DE TRABAJO FABRICACION
				'-----------------------------------------------------------------------------------------------------------------------------------------------
				Dim InsTrab = From instruccion In BBDD.INSTRUCCIONES_DE_TRABAJO _
				 Where instruccion.OPERACION = hojaInst.CODIGO _
				 Select instruccion
				If (InsTrab IsNot Nothing) Then
					Dim InstTrabNew As Registro.INSTRUCCIONES_DE_TRABAJO_FABRICACION
					For Each oInstru As Registro.INSTRUCCIONES_DE_TRABAJO In InsTrab
						InstTrabNew = New Registro.INSTRUCCIONES_DE_TRABAJO_FABRICACION
						InstTrabNew.OPERACION = oInstru.OPERACION
						InstTrabNew.NUMERO = oInstru.NUMERO
						InstTrabNew.INSTRUCCION = oInstru.INSTRUCCION

						BBDD.INSTRUCCIONES_DE_TRABAJO_FABRICACION.InsertOnSubmit(InstTrabNew)
					Next
					If (InsTrab.Any) Then BBDD.SubmitChanges()
				End If
				'-----------------------------------------------------------------------------------------------------------------------------------------------

				'-----------------------------------------------------------------------------------------------------------------------------------------------
				'7º Copiar a PARAMETROS DE MAQUINAS FABRICACION
				'-----------------------------------------------------------------------------------------------------------------------------------------------
				Dim ParamMaq = From parametro In BBDD.PARAMETROS_DE_MAQUINAS _
				   Where parametro.OPERACION = hojaInst.CODIGO _
				   Select parametro
				If (ParamMaq IsNot Nothing) Then
					Dim ParamMaqNew As Registro.PARAMETROS_DE_MAQUINAS_FABRICACION
					For Each oParam As Registro.PARAMETROS_DE_MAQUINAS In ParamMaq
						ParamMaqNew = New Registro.PARAMETROS_DE_MAQUINAS_FABRICACION
						ParamMaqNew.OPERACION = oParam.OPERACION
						ParamMaqNew.ORDEN = oParam.ORDEN
						ParamMaqNew.CLAVE = oParam.CLAVE
						ParamMaqNew.DESCRIPCION = oParam.DESCRIPCION
						ParamMaqNew.CONSIGNA = oParam.CONSIGNA
						ParamMaqNew.UNIDAD = oParam.UNIDAD

						BBDD.PARAMETROS_DE_MAQUINAS_FABRICACION.InsertOnSubmit(ParamMaqNew)
					Next
					If (ParamMaq.Any) Then BBDD.SubmitChanges()
				End If
				'-----------------------------------------------------------------------------------------------------------------------------------------------

				'-----------------------------------------------------------------------------------------------------------------------------------------------
				'8º Copiar a ORDEN DE MONTAJE DE PIEZAS FABRICACION
				'-----------------------------------------------------------------------------------------------------------------------------------------------
				Dim OrdenMontaje = From orden In BBDD.ORDEN_DE_MONTAJE_DE_PIEZAS _
				 Where orden.NUMOPER = hojaInst.CODIGO _
				 Select orden
				If (OrdenMontaje IsNot Nothing) Then
					Dim OrdenMontNew As Registro.ORDEN_DE_MONTAJE_DE_PIEZAS_FABRICACION
					For Each oOrden As Registro.ORDEN_DE_MONTAJE_DE_PIEZAS In OrdenMontaje
						OrdenMontNew = New Registro.ORDEN_DE_MONTAJE_DE_PIEZAS_FABRICACION
						OrdenMontNew.NUMOPER = oOrden.NUMOPER
						OrdenMontNew.ORDEN = oOrden.ORDEN
						OrdenMontNew.CODIGO_PIEZA = oOrden.CODIGO_PIEZA
						OrdenMontNew.DENOMINACION = oOrden.DENOMINACION

						BBDD.ORDEN_DE_MONTAJE_DE_PIEZAS_FABRICACION.InsertOnSubmit(OrdenMontNew)
					Next
					If (OrdenMontaje.Any) Then BBDD.SubmitChanges()
				End If
				'-----------------------------------------------------------------------------------------------------------------------------------------------

				'-------------------------------------------------------------------------------
				'3º Eliminar CARACTERISTICAS DEL PLAN FABRICACION (TODAS)
				'-------------------------------------------------------------------------------
				'Dim caracPlanFab = From caractPlan In BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION _
				'   Where caractPlan.CODIGO = hojaInst.CODIGO _
				'   Select caractPlan

				'If (caracPlanFab.Count = 1) Then
				'    BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION.DeleteOnSubmit(caracPlanFab.First)
				'    BBDD.SubmitChanges()
				'End If
				'-------------------------------------------------------------------------------
				Dim caracPlanFab = From caractPlan In BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION _
				  Where caractPlan.CODIGO = hojaInst.CODIGO _
				  Select caractPlan

				If (caracPlanFab IsNot Nothing AndAlso caracPlanFab.Count >= 1) Then
					BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION.DeleteAllOnSubmit(caracPlanFab)
					BBDD.SubmitChanges()
				End If
				'-------------------------------------------------------------------------------

				'------------------------------------------------------------------------
				'9º Copiar a CARACTERISTICAS DEL PLAN FABRICACION
				'------------------------------------------------------------------------
				'--------------------------------------------------------------------------------
				'Eliminamos todos los PLAN_DE_CONTROL_FABRICACION y CARACTERISTICAS_DEL_PLAN_FABRICACION 
				'para el CODIGO especificado.
				'--------------------------------------------------------------------------------
				Dim lPlanControlF As List(Of KaPlanLib.Registro.PLAN_DE_CONTROL_FABRICACION) = _
				   (From PCF As KaPlanLib.Registro.PLAN_DE_CONTROL_FABRICACION In BBDD.PLAN_DE_CONTROL_FABRICACION _
								  Where PCF.CODIGO = hojaInst.CODIGO Select PCF).ToList
				If lPlanControlF IsNot Nothing AndAlso lPlanControlF.Count Then
					BBDD.PLAN_DE_CONTROL_FABRICACION.DeleteAllOnSubmit(lPlanControlF)
					BBDD.SubmitChanges()
				End If

				Dim lCaracteristicaPlanF As List(Of KaPlanLib.Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION) = _
					(From CPF In BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION Where CPF.CODIGO = hojaInst.CODIGO _
				   Select CPF).ToList
				If lCaracteristicaPlanF IsNot Nothing AndAlso lCaracteristicaPlanF.Any Then
					BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION.DeleteAllOnSubmit(lCaracteristicaPlanF)
					BBDD.SubmitChanges()
				End If
				'--------------------------------------------------------------------------------

				'--------------------------------------------------------------------------------
				'Insertamos el PLAN_DE_CONTROL_FABRICACION de PLAN_DE_CONTROL 
				'y todas las CARACTERISTICAS_DEL_PLAN_FABRICACION de CARACTERISTICAS_DEL_PLAN para el CODIGO especificado
				'--------------------------------------------------------------------------------
				Dim PlanControl As Registro.PLAN_DE_CONTROL = _
				 (From PCF As Registro.PLAN_DE_CONTROL In BBDD.PLAN_DE_CONTROL _
				Where PCF.CODIGO = hojaInst.CODIGO Select PCF Order By PCF.FECHA Descending).FirstOrDefault

				If PlanControl IsNot Nothing Then
					'------------------------------------------------------------------------
					Dim PlanControlFabricacion As New Registro.PLAN_DE_CONTROL_FABRICACION
					PlanControlFabricacion.CODIGO = PlanControl.CODIGO
					PlanControlFabricacion.FECHA = PlanControl.FECHA
					PlanControlFabricacion.NIVEL = PlanControl.NIVEL
					BBDD.PLAN_DE_CONTROL_FABRICACION.InsertOnSubmit(PlanControlFabricacion)
					'------------------------------------------------------------------------

					'------------------------------------------------------------------------
					Dim lCaracteristicaPlan As List(Of Registro.CARACTERISTICAS_DEL_PLAN) = _
						(From CP As Registro.CARACTERISTICAS_DEL_PLAN In BBDD.CARACTERISTICAS_DEL_PLAN _
						 Where CP.CODIGO = hojaInst.CODIGO Select CP).ToList
					If (lCaracteristicaPlan IsNot Nothing AndAlso lCaracteristicaPlan.Any) Then
						Dim CaracteristicaPlanF As Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION
						For Each oCaract As Registro.CARACTERISTICAS_DEL_PLAN In lCaracteristicaPlan
							CaracteristicaPlanF = New Registro.CARACTERISTICAS_DEL_PLAN_FABRICACION
							CaracteristicaPlanF.CODIGO = oCaract.CODIGO
							CaracteristicaPlanF.MAQUINA = oCaract.MAQUINA
							CaracteristicaPlanF.POSICION = oCaract.POSICION
							CaracteristicaPlanF.ORDEN_CARAC = oCaract.ORDEN_CARAC
							CaracteristicaPlanF.CARAC_PARAM = oCaract.CARAC_PARAM
							CaracteristicaPlanF.ESPECIFICACION = oCaract.ESPECIFICACION
							CaracteristicaPlanF.FRECUENCIA_CONTROL = oCaract.FRECUENCIA_CONTROL
							CaracteristicaPlanF.FRECUENCIA_CONTROL_CAL = oCaract.FRECUENCIA_CONTROL_CAL
							CaracteristicaPlanF.FRECUENCIA_REGISTRO = oCaract.FRECUENCIA_REGISTRO
							CaracteristicaPlanF.ID_Doc_Registro = oCaract.ID_Doc_Registro
							CaracteristicaPlanF.METODO_EVALUACION = oCaract.METODO_EVALUACION
							CaracteristicaPlanF.RESPONSABLE = oCaract.RESPONSABLE
							CaracteristicaPlanF.MEDIO_DENOMINACION = oCaract.MEDIO_DENOMINACION

							CaracteristicaPlanF.Control_DET_ME_CTRL = oCaract.Control_DET_ME_CTRL
							CaracteristicaPlanF.Control_DET_ME_FAB = oCaract.Control_DET_ME_FAB
							CaracteristicaPlanF.Codigo_Control_ME_FAB = oCaract.Codigo_Control_ME_FAB

							CaracteristicaPlanF.MEDIO_RFA = oCaract.MEDIO_RFA
							CaracteristicaPlanF.CLASE = oCaract.CLASE
							CaracteristicaPlanF.OBSERVACIONES = oCaract.OBSERVACIONES
							CaracteristicaPlanF.ACCION_RECOMENDADA = oCaract.ACCION_RECOMENDADA
							CaracteristicaPlanF.CONT_CAUSA = oCaract.CONT_CAUSA
							CaracteristicaPlanF.ID_CARACTERISTICA = oCaract.ID_CARACTERISTICA
							CaracteristicaPlanF.PROCEDE_DE = oCaract.PROCEDE_DE
							CaracteristicaPlanF.PROCESO_PRODUCTO = oCaract.PROCESO_PRODUCTO
							CaracteristicaPlanF.TAMAÑO = oCaract.TAMAÑO
							CaracteristicaPlanF.TAMAÑO_CAL = oCaract.TAMAÑO_CAL
							CaracteristicaPlanF.METODO_CONTROL = oCaract.METODO_CONTROL
							CaracteristicaPlanF.METODO_CONTROL_FAB = oCaract.METODO_CONTROL_FAB
							CaracteristicaPlanF.HOJA_REGISTROS = oCaract.HOJA_REGISTROS
							CaracteristicaPlanF.VER_REG_REC = oCaract.VER_REG_REC
							CaracteristicaPlanF.VER_REG_PRO = oCaract.VER_REG_PRO
							CaracteristicaPlanF.VER_REG_DIM = oCaract.VER_REG_DIM
							CaracteristicaPlanF.VER_REG_MAT = oCaract.VER_REG_MAT
							CaracteristicaPlanF.VER_REG_FUN = oCaract.VER_REG_FUN
							CaracteristicaPlanF.MAXIM = oCaract.MAXIM
							CaracteristicaPlanF.MINIM = oCaract.MINIM

							CaracteristicaPlanF.Responsable_Maquina = oCaract.Responsable_Maquina
							CaracteristicaPlanF.Responsable_Operario = oCaract.Responsable_Operario
							CaracteristicaPlanF.Responsable_Calidad = oCaract.Responsable_Calidad

							'--------------------------------------------------------------------------------------------------------
							'Responsables de Registro.
							'--------------------------------------------------------------------------------------------------------
							CaracteristicaPlanF.RESPONSABLE_REGISTRO = oCaract.RESPONSABLE_REGISTRO 'Campo Obsoleto. No usar para meter datos.
                            CaracteristicaPlanF.Responsable_Reg_Ope = oCaract.Responsable_Reg_Ope : CaracteristicaPlanF.Responsable_Reg_Gestor = oCaract.Responsable_Reg_Gestor
                            CaracteristicaPlanF.Responsable_Reg_Cal = oCaract.Responsable_Reg_Cal
							'--------------------------------------------------------------------------------------------------------

							'--------------------------------------------------------------------------------------------------------
							'Copia de la ayuda visual a Fabricacion.
							'--------------------------------------------------------------------------------------------------------
							AyudaVisualCopiar_FAB(oCaract, CaracteristicaPlanF)
							'--------------------------------------------------------------------------------------------------------

							BBDD.CARACTERISTICAS_DEL_PLAN_FABRICACION.InsertOnSubmit(CaracteristicaPlanF)
						Next
						BBDD.SubmitChanges()
					End If
					'------------------------------------------------------------------------
				End If
				'------------------------------------------------------------------------

				transaction.Commit()
			Catch ex As ApplicationException
				transaction.Rollback()
				Throw
			Catch ex As Exception
				transaction.Rollback()
				Throw
			Finally
				BBDD.Connection.Close()
			End Try
		End Sub

		''' <summary>
		''' Elimina la hoja de instrucciones
		''' </summary>
		''' <param name="cod"></param>
		''' <remarks></remarks>
		Public Sub EliminarHojaIntrucciones(ByVal cod As String)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim hojaInstr = From hoja In BBDD.HOJA_DE_INSTRUCCIONES _
				 Where hoja.CODIGO = cod _
				 Select hoja

				If (hojaInstr.Any) Then
					BBDD.HOJA_DE_INSTRUCCIONES.DeleteOnSubmit(hojaInstr.First)
					BBDD.SubmitChanges()
				End If
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub
#End Region

#Region "MAESTRO_SECCION"

		''' <summary>
		''' Devuelve un registro de Maestro seccion
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarMaestroSeccion(ByVal idSec As Integer) As KaPlanLib.Registro.M_SECCION
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim RegSeccion = From Seccion In BBDD.M_SECCION _
				Where Seccion.ID_SECCION = idSec _
				Select Seccion

				If (Not RegSeccion.Any) Then
					Return Nothing
				Else
					Return RegSeccion.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

#End Region

#Region "MAESTRO_ACCIONES"

		''' <summary>
		''' Devuelve el listado de acciones
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarListadoMaestroAcciones()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listAcciones = From Acciones In BBDD.MAESTRO_DE_ACCIONES _
				 Select Acciones

				Return listAcciones
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

#End Region

#Region "ORDEN MONTAJE DE PIEZA"
		''' <summary>
		''' Guarda el orden de montaje de pieza
		''' </summary>
		''' <param name="oOrden">Datos del orden de la pieza</param>
		Public Sub GuardarOrdenMontajePieza(ByVal oOrden As Registro.ORDEN_DE_MONTAJE_DE_PIEZAS)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oOrden.ID_REGISTRO = 0) Then 'Nuevo
					BBDD.ORDEN_DE_MONTAJE_DE_PIEZAS.InsertOnSubmit(oOrden)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim ordenMont As KaPlanLib.Registro.ORDEN_DE_MONTAJE_DE_PIEZAS = (From Orden In BBDD.ORDEN_DE_MONTAJE_DE_PIEZAS _
					   Where Orden.ID_REGISTRO = oOrden.ID_REGISTRO _
					   Select Orden).FirstOrDefault

					If (ordenMont Is Nothing) Then
						Throw New ApplicationException("No se encuentra la 'Orden de Montaje'.")
					End If

					ordenMont.ORDEN = oOrden.ORDEN
					ordenMont.CODIGO_PIEZA = oOrden.CODIGO_PIEZA
					ordenMont.DENOMINACION = oOrden.DENOMINACION
					BBDD.SubmitChanges()
				End If

			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub

		''' <summary>
		''' Elimina un el orden de montaje de pieza
		''' </summary>
		''' <param name="idRegistro">Id Registro</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarOrdenMontajePieza(ByVal idRegistro As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim OrdenMont = From orden In BBDD.ORDEN_DE_MONTAJE_DE_PIEZAS _
				 Where orden.ID_REGISTRO = idRegistro _
				 Select orden

				If (OrdenMont.Count = 1) Then
					BBDD.ORDEN_DE_MONTAJE_DE_PIEZAS.DeleteOnSubmit(OrdenMont.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function
#End Region

#Region "PARAMETROS_MAQUINA"
		'-------------------------------------------------------------------------------------------------
		' ''' <summary>
		' ''' Guarda el parametro de maquina
		' ''' </summary>
		' ''' <param name="oParamMaq">Parametro de maquina</param>
		' ''' <returns></returns>
		' ''' <remarks></remarks>
		'      Public Function GuardarParametroMaquina(ByVal oParamMaq As KaPlanLib.Registro.PARAMETROS_DE_MAQUINAS) As Boolean
		'          Dim BBDD As New KaPlanLib.DAL.ELL
		'          Try
		'              If (oParamMaq.ID_REGISTRO = 0) Then 'insertar
		'                  BBDD.PARAMETROS_DE_MAQUINAS.InsertOnSubmit(oParamMaq)
		'              Else 'actualizar
		'                  Dim paramM As KaPlanLib.Registro.PARAMETROS_DE_MAQUINAS = (From parametroMaq In BBDD.PARAMETROS_DE_MAQUINAS _
		'                    Where parametroMaq.ID_REGISTRO = oParamMaq.ID_REGISTRO _
		'                    Select parametroMaq).FirstOrDefault
		'                  If (paramM Is Nothing) Then Return False

		'                  paramM.ORDEN = oParamMaq.ORDEN
		'                  paramM.CLAVE = oParamMaq.CLAVE
		'                  paramM.DESCRIPCION = oParamMaq.DESCRIPCION
		'                  paramM.CONSIGNA = oParamMaq.CONSIGNA
		'                  paramM.UNIDAD = oParamMaq.UNIDAD
		'                  paramM.OPERACION = oParamMaq.OPERACION
		'              End If
		'              BBDD.SubmitChanges()
		'              Return True
		'          Catch ex As Exception
		'              Return False
		'          End Try
		'End Function
		'-------------------------------------------------------------------------------------------------
		'FROGA:2012-09-13:
		'-------------------------------------------------------------------------------------------------

		''' <summary>
		''' Guarda el parametro de maquina
		''' </summary>
		''' <param name="oParamMaq">Parametro de maquina</param>
		''' <remarks></remarks>
		Public Sub GuardarParametroMaquina(ByVal oParamMaq As KaPlanLib.Registro.PARAMETROS_DE_MAQUINAS)
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oParamMaq.ID_REGISTRO = 0) Then	'insertar
					BBDD.PARAMETROS_DE_MAQUINAS.InsertOnSubmit(oParamMaq)
				Else 'actualizar
					Dim paramM As KaPlanLib.Registro.PARAMETROS_DE_MAQUINAS = (From parametroMaq In BBDD.PARAMETROS_DE_MAQUINAS _
					  Where parametroMaq.ID_REGISTRO = oParamMaq.ID_REGISTRO _
					  Select parametroMaq).FirstOrDefault

					If (paramM Is Nothing) Then
						Throw New ApplicationException("No se encuentra el parametro.")
					End If

					paramM.ORDEN = oParamMaq.ORDEN
					paramM.CLAVE = oParamMaq.CLAVE
					paramM.DESCRIPCION = oParamMaq.DESCRIPCION
					paramM.CONSIGNA = oParamMaq.CONSIGNA
					paramM.UNIDAD = oParamMaq.UNIDAD
					paramM.OPERACION = oParamMaq.OPERACION
				End If
				BBDD.SubmitChanges()

			Catch ex As ApplicationException
				Throw
			Catch ex As Exception
				log.Error(ex)
				Throw
			End Try
		End Sub
		'-------------------------------------------------------------------------------------------------

		''' <summary>
		''' Elimina un parametro de maquina
		''' </summary>
		''' <param name="idRegistro">Id del registro</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarParametroMaquina(ByVal idRegistro As Integer) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				Dim ParamMaq = From paramM In BBDD.PARAMETROS_DE_MAQUINAS _
				 Where paramM.ID_REGISTRO = idRegistro _
				 Select paramM

				If (ParamMaq.Count = 1) Then
					BBDD.PARAMETROS_DE_MAQUINAS.DeleteOnSubmit(ParamMaq.First)
					BBDD.SubmitChanges()
					Return True
				End If
				Return False
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Copia los parametros de los Parametros Generales de Seccion
		''' </summary>
		''' <param name="cod">Codigo de operacion</param>
		''' <param name="idSeccion">Id seccion de la operacion</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function CopiarParametrosMaquina(ByVal cod As String, ByVal idSeccion As Integer) As Boolean
			Dim transaction As System.Data.Common.DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				Dim paramGenSec = From paramGS In BBDD.PARAMETRO_GENERAL_SECCION _
				   Where paramGS.SECCION = idSeccion _
				   Select paramGS

				If (paramGenSec.Any) Then
					Dim oParamMaqNew As Registro.PARAMETROS_DE_MAQUINAS
					For Each oParam As Registro.PARAMETRO_GENERAL_SECCION In paramGenSec
						oParamMaqNew = New Registro.PARAMETROS_DE_MAQUINAS
						oParamMaqNew.OPERACION = cod
						oParamMaqNew.ORDEN = oParam.ORDEN
						oParamMaqNew.CLAVE = oParam.CLAVE
						oParamMaqNew.DESCRIPCION = oParam.DESCRIPCION
						BBDD.PARAMETROS_DE_MAQUINAS.InsertOnSubmit(oParamMaqNew)
					Next
					BBDD.SubmitChanges()
				End If

				transaction.Commit()
				Return True
			Catch ex As Exception
				transaction.Rollback()
				Return False
			Finally
				If (BBDD.Connection.State <> ConnectionState.Closed) Then BBDD.Connection.Close()
			End Try
		End Function

#End Region

#Region "INSTRUCCIONES_TRABAJO"

		''' <summary>
		''' Guarda la instruccion de trabajo
		''' </summary>
		''' <param name="oInstrTrab">Instruccion de trabajo</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarInstruccionTrabajo(ByVal oInstrTrab As KaPlanLib.Registro.INSTRUCCIONES_DE_TRABAJO) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oInstrTrab.ID_REGISTRO = 0) Then 'insertar
					BBDD.INSTRUCCIONES_DE_TRABAJO.InsertOnSubmit(oInstrTrab)
				Else 'actualizar
					Dim instrT As KaPlanLib.Registro.INSTRUCCIONES_DE_TRABAJO = (From instruccionTrab In BBDD.INSTRUCCIONES_DE_TRABAJO _
					  Where instruccionTrab.ID_REGISTRO = oInstrTrab.ID_REGISTRO _
					  Select instruccionTrab).FirstOrDefault
					If (instrT Is Nothing) Then Return False

					instrT.NUMERO = oInstrTrab.NUMERO
					instrT.INSTRUCCION = oInstrTrab.INSTRUCCION
					instrT.OPERACION = oInstrTrab.OPERACION
				End If
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina la instruccion de trabajo
		''' </summary>
		''' <param name="idRegistro">Id del registro</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarInstruccionTrabajo(ByVal idRegistro As Integer) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				Dim InstrTrab = From instrT In BBDD.INSTRUCCIONES_DE_TRABAJO _
				 Where instrT.ID_REGISTRO = idRegistro _
				 Select instrT

				If (InstrTrab.Count = 1) Then
					BBDD.INSTRUCCIONES_DE_TRABAJO.DeleteOnSubmit(InstrTrab.First)
					BBDD.SubmitChanges()
					Return True
				End If
				Return False
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Copia las instrucciones del Maestro de Instrucciones de Trabajo
		''' </summary>
		''' <param name="cod">Codigo de operacion</param>
		''' <param name="opeGeneral">Operacion general</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function CopiarInstruccionesTrabajo(ByVal cod As String, ByVal opeGeneral As String) As Boolean
			Dim transaction As System.Data.Common.DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				Dim maestroInstTrab = From maestroI In BBDD.MAESTRO_INSTRUCCIONES_DE_TRABAJO _
				 Where maestroI.OPERACION_GENERAL = opeGeneral _
				 Select maestroI

				If (maestroInstTrab.Any) Then
					Dim oInstrTrabNew As Registro.INSTRUCCIONES_DE_TRABAJO
					For Each oInstr As Registro.MAESTRO_INSTRUCCIONES_DE_TRABAJO In maestroInstTrab
						oInstrTrabNew = New Registro.INSTRUCCIONES_DE_TRABAJO
						oInstrTrabNew.OPERACION = cod
						oInstrTrabNew.NUMERO = oInstr.NUMERO
						oInstrTrabNew.INSTRUCCION = oInstr.INSTRUCCION
						BBDD.INSTRUCCIONES_DE_TRABAJO.InsertOnSubmit(oInstrTrabNew)
					Next
					BBDD.SubmitChanges()
				End If

				transaction.Commit()
				Return True
			Catch ex As Exception
				transaction.Rollback()
				Return False
			Finally
				If (BBDD.Connection.State <> ConnectionState.Closed) Then BBDD.Connection.Close()
			End Try
		End Function

#End Region

#Region "MAESTRO_LANTEGIS"

		''' <summary>
		''' Devuelve el listado de lantegis
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarListadoLantegis()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim lantegis = From lanteg In BBDD.M_LANTEGI _
				  Order By lanteg.LANTEGI _
				  Select lanteg
				Return lantegis
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

#End Region

#Region "R_M_NORMAS"

		''' <summary>
		''' Guarda la norma
		''' </summary>
		''' <param name="oNorma">Datos de la norma</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarNorma(ByVal oNorma As Registro.R_M_NORMAS) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oNorma.ID_M_NORMA = 0) Then	'Nuevo
					BBDD.R_M_NORMAS.InsertOnSubmit(oNorma)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim norma As KaPlanLib.Registro.R_M_NORMAS = (From Normas In BBDD.R_M_NORMAS _
					Where Normas.ID_M_NORMA = oNorma.ID_M_NORMA _
					Select Normas).FirstOrDefault
					If (norma Is Nothing) Then Return False

					norma.NORMA = oNorma.NORMA
					BBDD.SubmitChanges()
				End If

				Return True			
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina una norma
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarNorma(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim norma = From normas In BBDD.R_M_NORMAS _
				 Where normas.ID_M_NORMA = id _
				 Select normas

				If (norma.Count = 1) Then
					BBDD.R_M_NORMAS.DeleteOnSubmit(norma.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_CERTIFICADORES"

		''' <summary>
		''' Guarda el certificador
		''' </summary>
		''' <param name="oCertif">Datos del certificador</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarCertificador(ByVal oCertif As Registro.R_M_CERTIFICADORES) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oCertif.ID_M_CERTIFICADOR = 0) Then	'Nuevo
					BBDD.R_M_CERTIFICADORES.InsertOnSubmit(oCertif)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim certif As KaPlanLib.Registro.R_M_CERTIFICADORES = (From Certificadores In BBDD.R_M_CERTIFICADORES _
					Where Certificadores.ID_M_CERTIFICADOR = oCertif.ID_M_CERTIFICADOR _
					Select Certificadores).FirstOrDefault
					If (certif Is Nothing) Then Return False

					certif.CERTIFICADOR = oCertif.CERTIFICADOR
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina un certificador
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarCertificador(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim certif = From certificadores In BBDD.R_M_CERTIFICADORES _
				 Where certificadores.ID_M_CERTIFICADOR = id _
				 Select certificadores

				If (certif.Count = 1) Then
					BBDD.R_M_CERTIFICADORES.DeleteOnSubmit(certif.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_PERIODICIDAD"

		''' <summary>
		''' Guarda la periodicidad
		''' </summary>
		''' <param name="oPeriodic">Datos de la periodicidad</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarPeriodicidad(ByVal oPeriodic As Registro.R_M_PERIODICIDAD) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oPeriodic.ID_PERIODICIDAD = 0) Then	'Nuevo
					BBDD.R_M_PERIODICIDAD.InsertOnSubmit(oPeriodic)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim periodic As KaPlanLib.Registro.R_M_PERIODICIDAD = (From periodicidades In BBDD.R_M_PERIODICIDAD _
					  Where periodicidades.ID_PERIODICIDAD = oPeriodic.ID_PERIODICIDAD _
					  Select periodicidades).FirstOrDefault
					If (periodic Is Nothing) Then Return False

					periodic.DESCRIPCION = oPeriodic.DESCRIPCION
					periodic.DIAS = oPeriodic.DIAS
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina la periodicidad
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarPeriodicidad(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim periodic = From periodicidades In BBDD.R_M_PERIODICIDAD _
				 Where periodicidades.ID_PERIODICIDAD = id _
				 Select periodicidades

				If (periodic.Count = 1) Then
					BBDD.R_M_PERIODICIDAD.DeleteOnSubmit(periodic.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_FRECUENCIA_CONTROL"

		''' <summary>
		''' Guarda la frecuencia de control
		''' </summary>
		''' <param name="oFrec">Datos de la frecuencia de control</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarFrecuenciaControl(ByVal oFrec As Registro.R_M_FRECUENCIAS_CONTROL) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oFrec.ID_M_FRECUENCIA = 0) Then	'Nuevo
					BBDD.R_M_FRECUENCIAS_CONTROL.InsertOnSubmit(oFrec)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim frecuen As KaPlanLib.Registro.R_M_FRECUENCIAS_CONTROL = (From frecuencias In BBDD.R_M_FRECUENCIAS_CONTROL _
					  Where frecuencias.ID_M_FRECUENCIA = oFrec.ID_M_FRECUENCIA _
					  Select frecuencias).FirstOrDefault
					If (frecuen Is Nothing) Then Return False

					frecuen.HASTA_ENTRADAS = oFrec.HASTA_ENTRADAS
					frecuen.INSPECCIONAR_CADA = oFrec.INSPECCIONAR_CADA
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina la frecuencia de control
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarFrecuenciaControl(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim frecuen = From frecuencias In BBDD.R_M_FRECUENCIAS_CONTROL _
				 Where frecuencias.ID_M_FRECUENCIA = id _
				 Select frecuencias

				If (frecuen.Count = 1) Then
					BBDD.R_M_FRECUENCIAS_CONTROL.DeleteOnSubmit(frecuen.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_VERIFICADORES"

		''' <summary>
		''' Guarda el verificador
		''' </summary>
		''' <param name="oVerif">Datos del verificador</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarVerificador(ByVal oVerif As Registro.R_M_VERIFICADORES) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oVerif.ID_M_VERIFICADOR = 0) Then	'Nuevo
					BBDD.R_M_VERIFICADORES.InsertOnSubmit(oVerif)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim verif As KaPlanLib.Registro.R_M_VERIFICADORES = (From verificadores In BBDD.R_M_VERIFICADORES _
					  Where verificadores.ID_M_VERIFICADOR = oVerif.ID_M_VERIFICADOR _
					  Select verificadores).FirstOrDefault
					If (verif Is Nothing) Then Return False

					verif.VERIFICADOR = oVerif.VERIFICADOR
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina el verificador
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarVerificador(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim verif = From verificadores In BBDD.R_M_VERIFICADORES _
				Where verificadores.ID_M_VERIFICADOR = id _
				Select verificadores

				If (verif.Count = 1) Then
					BBDD.R_M_VERIFICADORES.DeleteOnSubmit(verif.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_LINEAS"

		''' <summary>
		''' Guarda la linea
		''' </summary>
		''' <param name="oLinea">Datos de la linea</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarLinea(ByVal oLinea As Registro.R_M_LINEAS) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oLinea.ID_M_LINEA = 0) Then	'Nuevo
					BBDD.R_M_LINEAS.InsertOnSubmit(oLinea)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim linea As KaPlanLib.Registro.R_M_LINEAS = (From lineas In BBDD.R_M_LINEAS _
					  Where lineas.ID_M_LINEA = oLinea.ID_M_LINEA _
					  Select lineas).FirstOrDefault
					If (linea Is Nothing) Then Return False

					linea.COD_LINEA = oLinea.COD_LINEA
					linea.DES_LINEA = oLinea.DES_LINEA
					linea.PRECIO_LINEA = oLinea.PRECIO_LINEA
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina el verificador
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarLinea(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim linea = From lineas In BBDD.R_M_LINEAS _
				Where lineas.ID_M_LINEA = id _
				Select lineas

				If (linea.Count = 1) Then
					BBDD.R_M_LINEAS.DeleteOnSubmit(linea.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_TIPOS_BRAIN"

		''' <summary>
		''' Guarda el tipo Brain
		''' </summary>
		''' <param name="oTipo">Datos del tipo</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarTipoBrain(ByVal oTipo As Registro.R_M_TIPOS_BRAIN) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oTipo.ID_M_TIPO_BRAIN = 0) Then	'Nuevo
					BBDD.R_M_TIPOS_BRAIN.InsertOnSubmit(oTipo)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim tipo As KaPlanLib.Registro.R_M_TIPOS_BRAIN = (From tipos In BBDD.R_M_TIPOS_BRAIN _
					Where tipos.ID_M_TIPO_BRAIN = oTipo.ID_M_TIPO_BRAIN _
					Select tipos).FirstOrDefault
					If (tipo Is Nothing) Then Return False

					tipo.COD_TIPO_BRAIN = oTipo.COD_TIPO_BRAIN
					tipo.DES_TIPO_BRAIN = oTipo.DES_TIPO_BRAIN
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina el tipo Brain
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarTipoBrain(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim tipo = From tipos In BBDD.R_M_TIPOS_BRAIN _
				Where tipos.ID_M_TIPO_BRAIN = id _
				Select tipos

				If (tipo.Count = 1) Then
					BBDD.R_M_TIPOS_BRAIN.DeleteOnSubmit(tipo.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_PRECIOS_NC"

		''' <summary>
		''' Devuelve el primer registro de los precios de la NC
		''' </summary>
		''' <returns></returns>
		Function consultarPreciosNC() As Registro.R_M_PRECIOS_NC
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listPrecios = From Precios In BBDD.R_M_PRECIOS_NC _
				Select Precios

				If (Not listPrecios.Any) Then
					Return Nothing
				Else
					Return listPrecios.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Devuelve todos los precios de la NC
		''' </summary>
		''' <returns></returns>
		Function consultarListadoPreciosNC()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listPrecios = From Precios In BBDD.R_M_PRECIOS_NC _
				Select Precios

				Return listPrecios
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda los precios de la NC
		''' </summary>
		''' <param name="oPrecios">Precios</param>                        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarPreciosNC(ByVal oPrecios As Registro.R_M_PRECIOS_NC) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oPrecios.ID_M_PRECIOS = 0) Then	'Nuevo
					BBDD.R_M_PRECIOS_NC.InsertOnSubmit(oPrecios)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim precio As KaPlanLib.Registro.R_M_PRECIOS_NC = (From precios In BBDD.R_M_PRECIOS_NC _
					Where precios.ID_M_PRECIOS = oPrecios.ID_M_PRECIOS _
					Select precios).FirstOrDefault
					If (precio Is Nothing) Then Return False

					precio.PRECIO_CHATARRA = oPrecios.PRECIO_CHATARRA
					precio.PRECIO_CONTROL_CAL = oPrecios.PRECIO_CONTROL_CAL
					precio.PRECIO_GARANTIA_CAL = oPrecios.PRECIO_GARANTIA_CAL
					precio.PRECIO_GASTOS_ADMIN = oPrecios.PRECIO_GASTOS_ADMIN
					precio.PRECIO_INGENIERIA = oPrecios.PRECIO_INGENIERIA
					precio.PRECIO_RECUPERACION = oPrecios.PRECIO_RECUPERACION
					precio.PRECIO_SELECCION = oPrecios.PRECIO_SELECCION
					precio.PRECIO_TRANSPORTE = oPrecios.PRECIO_TRANSPORTE
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_PROYECTOS"

		''' <summary>
		''' Devuelve un registro de proyecto
		''' </summary>
		''' <param name="id">Id proyecto</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarProyecto(ByVal id As Integer) As KaPlanLib.Registro.R_M_PROYECTOS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResProy = From Proyectos In BBDD.R_M_PROYECTOS _
				Where Proyectos.ID_PROYECTO = id _
				Select Proyectos

				If (Not ResProy.Any) Then
					Return Nothing
				Else
					Return ResProy.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function


		''' <summary>
		''' Devuelve la lista de proveedores
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarListadoProyectos()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listaProyectos = From Proyectos In BBDD.R_M_PROYECTOS _
				 Order By Proyectos.PROYECTO _
				Select Proyectos

				Return listaProyectos
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el proyecto
		''' </summary>
		''' <param name="oProy">Datos del proyecto</param>                        
		''' <returns>Devuelve el idProyecto</returns>
		''' <remarks></remarks>
		Public Function GuardarProyecto(ByVal oProy As Registro.R_M_PROYECTOS) As Integer
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				Dim idProyResul As Integer = 0
				If (oProy.ID_PROYECTO = 0) Then	'Nuevo
					BBDD.R_M_PROYECTOS.InsertOnSubmit(oProy)
					BBDD.SubmitChanges()
					idProyResul = oProy.ID_PROYECTO
				Else  'Modificar
					idProyResul = oProy.ID_PROYECTO
					Dim proy As KaPlanLib.Registro.R_M_PROYECTOS = (From proyectos In BBDD.R_M_PROYECTOS _
					  Where proyectos.ID_PROYECTO = oProy.ID_PROYECTO _
					  Select proyectos).FirstOrDefault
					If (proy Is Nothing) Then Return False

					proy.PROYECTO = oProy.PROYECTO
					BBDD.SubmitChanges()
				End If

				Return idProyResul
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina el proyecto
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarProyecto(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim proy = From proyectos In BBDD.R_M_PROYECTOS _
				Where proyectos.ID_PROYECTO = id _
				Select proyectos

				If (proy.Count = 1) Then
					BBDD.R_M_PROYECTOS.DeleteOnSubmit(proy.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_PROVEEDORES"

		''' <summary>
		''' Devuelve un registro de proveedor
		''' </summary>
		''' <param name="id">Id prov</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarProveedor(ByVal id As Integer) As KaPlanLib.Registro.R_M_PROVEEDORES
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResProv = From proveedores In BBDD.R_M_PROVEEDORES _
				Where proveedores.ID_PROVEEDOR = id _
				Select proveedores

				If (ResProv.Any = False) Then
					Return Nothing
				Else
					Return ResProv.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Devuelve la lista de proveedores
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarListadoProveedores(ByVal oProv As Registro.R_M_PROVEEDORES)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listaProveedores
				If (oProv IsNot Nothing) Then
					listaProveedores = From Proveedores In BBDD.R_M_PROVEEDORES _
					 Order By Proveedores.COD_PROVEEDOR _
					 Where Proveedores.COD_PROVEEDOR.Contains(oProv.COD_PROVEEDOR) And _
					 Proveedores.DES_PROVEEDOR.Contains(oProv.DES_PROVEEDOR) _
					 Select Proveedores
				Else
					listaProveedores = From Proveedores In BBDD.R_M_PROVEEDORES _
					Order By Proveedores.COD_PROVEEDOR _
					Select Proveedores
				End If

				'Where Proveedores.COD_PROVEEDOR.Contains(If(oProv.COD_PROVEEDOR <> String.Empty, oProv.COD_PROVEEDOR, Proveedores.COD_PROVEEDOR)) And _
				'	 Proveedores.DES_PROVEEDOR.Contains(If(oProv.DES_PROVEEDOR <> String.Empty, oProv.DES_PROVEEDOR, Proveedores.DES_PROVEEDOR)) _
				Return listaProveedores
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Elimina el proveedor
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarProveedor(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim prov = From proveedores In BBDD.R_M_PROVEEDORES _
				Where proveedores.ID_PROVEEDOR = id _
				Select proveedores

				If (prov.Count = 1) Then
					BBDD.R_M_PROVEEDORES.DeleteOnSubmit(prov.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_PROVEEDORES_PROYECTOS"

		''' <summary>
		''' Devuelve la lista de proveedores de un proyecto
		''' </summary>
		''' <param name="idProy"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarListadoProveedoresProyecto(ByVal idProy As Integer) As List(Of Registro.R_M_PROVEEDORES)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim listaProveedores = (From ProveedoresProyectos In BBDD.R_M_PROVEEDORES_PROYECTOS _
				Join Proveedores In BBDD.R_M_PROVEEDORES On ProveedoresProyectos.ID_PROVEEDOR Equals Proveedores.ID_PROVEEDOR _
				Order By Proveedores.DES_PROVEEDOR _
				Where ProveedoresProyectos.ID_PROYECTO = idProy _
				Select Proveedores)

				Return listaProveedores.ToList
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function


		'''' <summary>
		'''' Guarda el proveedor asociado al proyecto. Primero se mira si hay que borrar el antiguo y luego insertar el nuevo
		'''' </summary>
		'''' <param name="oProvProy">Objeto actual</param>                        		
		'''' <param name="idProvNew">Id del proveedor con el valor a actualizar</param>
		'''' <returns>Devuelve el idProyecto</returns>
		'''' <remarks></remarks>
		'Public Function GuardarProveedorProyecto(ByVal oProvProy As Registro.R_M_PROVEEDORES_PROYECTOS, ByVal idProvNew As Integer) As Boolean
		'	Dim transaction As DbTransaction = Nothing
		'	Dim BBDD As New KaPlanLib.DAL.ELL
		'	Try
		'		If (oProvProy.ID_PROVEEDOR Is Nothing) Then	  'Nuevo
		'			oProvProy.ID_PROVEEDOR = idProvNew
		'			BBDD.R_M_PROVEEDORES_PROYECTOS.InsertOnSubmit(oProvProy)
		'			BBDD.SubmitChanges()
		'		Else  'Modificar
		'			'Se borra el actual
		'			BBDD.Connection.Open()
		'			transaction = BBDD.Connection.BeginTransaction
		'			BBDD.Transaction = transaction

		'			Dim provProy As KaPlanLib.Registro.R_M_PROVEEDORES_PROYECTOS = (From proyectosProyect In BBDD.R_M_PROVEEDORES_PROYECTOS _
		'			  Where proyectosProyect.ID_PROYECTO = oProvProy.ID_PROYECTO And proyectosProyect.ID_PROVEEDOR = oProvProy.ID_PROYECTO _
		'			  Select proyectosProyect).FirstOrDefault
		'			If (provProy IsNot Nothing) Then
		'				BBDD.R_M_PROVEEDORES_PROYECTOS.DeleteOnSubmit(oProvProy)
		'				BBDD.SubmitChanges()
		'			End If

		'			'Se inserta el nuevo
		'			oProvProy.ID_PROVEEDOR = idProvNew
		'			BBDD.R_M_PROVEEDORES_PROYECTOS.InsertOnSubmit(oProvProy)
		'			BBDD.SubmitChanges()
		'			transaction.Commit()
		'		End If

		'		Return True
		'	Catch batzEx As BatzException
		'		If (transaction IsNot Nothing) Then transaction.Rollback()
		'		Return False
		'	Catch ex As Exception
		'		If (transaction IsNot Nothing) Then transaction.Rollback()
		'		Return False
		'	Finally
		'		BBDD.Connection.Close()
		'	End Try
		'End Function

		''' <summary>
		''' Guarda el proveedor asociado al proyecto.
		''' </summary>
		''' <param name="oProvProy">Objeto actual</param>                        		
		''' <returns>Devuelve el idProyecto</returns>
		''' <remarks></remarks>
		Public Function GuardarProveedorProyecto(ByVal oProvProy As Registro.R_M_PROVEEDORES_PROYECTOS) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				BBDD.R_M_PROVEEDORES_PROYECTOS.InsertOnSubmit(oProvProy)
				BBDD.SubmitChanges()
				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina el proveedor del proyecto
		''' </summary>
		''' <param name="idProv">Id del proveedor</param>
		''' <param name="idProy">Id del proyecto a desligar el proveedor</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarProveedorProyecto(ByVal idProv As Integer, ByVal idProy As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim proveedorProyecto = From provProy In BBDD.R_M_PROVEEDORES_PROYECTOS _
				Where provProy.ID_PROVEEDOR = idProv And provProy.ID_PROYECTO = idProy _
				Select provProy

				If (proveedorProyecto.Count = 1) Then
					BBDD.R_M_PROVEEDORES_PROYECTOS.DeleteOnSubmit(proveedorProyecto.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_TEXTOS"

		''' <summary>
		''' Devuelve un registro de texto
		''' </summary>
		''' <param name="id">Id texto</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarTexto(ByVal id As Integer) As KaPlanLib.Registro.R_M_TEXTOS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResText = From textos In BBDD.R_M_TEXTOS _
				Where textos.ID_TEXTO = id _
				Select textos

				If (ResText.Any = False) Then
					Return Nothing
				Else
					Return ResText.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el texto
		''' </summary>
		''' <param name="oText">Datos del texto</param>                        
		''' <remarks></remarks>
		Public Function GuardarTexto(ByVal oText As Registro.R_M_TEXTOS) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oText.ID_TEXTO = 0) Then	'Nuevo
					BBDD.R_M_TEXTOS.InsertOnSubmit(oText)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim text As KaPlanLib.Registro.R_M_TEXTOS = (From textos In BBDD.R_M_TEXTOS _
					  Where textos.ID_TEXTO = oText.ID_TEXTO _
					  Select textos).FirstOrDefault
					If (text Is Nothing) Then Return False

					text.NOM_TEXTO = oText.NOM_TEXTO
					text.DES_TEXTO = oText.DES_TEXTO
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina el texto
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarTexto(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim text = From texto In BBDD.R_M_TEXTOS _
				Where texto.ID_TEXTO = id _
				Select texto

				If (text.Count = 1) Then
					BBDD.R_M_TEXTOS.DeleteOnSubmit(text.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_VALORACION_CALIDAD"

		''' <summary>
		''' Devuelve un registro valoracion calidad
		''' </summary>
		''' <param name="id">Id valoracion</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarValoracionCalidad(ByVal id As Integer) As KaPlanLib.Registro.R_M_VALORACION_CALIDAD
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResVal = From valoraciones In BBDD.R_M_VALORACION_CALIDAD _
				Where valoraciones.ID_VALORACION = id _
				Select valoraciones

				If (ResVal.Any = False) Then
					Return Nothing
				Else
					Return ResVal.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la valoracion de calidad
		''' </summary>
		''' <param name="oValCal">Datos de la valoracion</param>                        
		''' <remarks></remarks>
		Public Function GuardarValoracionCalidad(ByVal oValCal As Registro.R_M_VALORACION_CALIDAD) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oValCal.ID_VALORACION = 0) Then	'Nuevo
					BBDD.R_M_VALORACION_CALIDAD.InsertOnSubmit(oValCal)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim valCal As KaPlanLib.Registro.R_M_VALORACION_CALIDAD = (From valoraciones In BBDD.R_M_VALORACION_CALIDAD _
					  Where valoraciones.ID_VALORACION = oValCal.ID_VALORACION _
					  Select valoraciones).FirstOrDefault
					If (valCal Is Nothing) Then Return False

					valCal.VALORACION = oValCal.VALORACION
					valCal.DESCRIPCION = oValCal.DESCRIPCION
					valCal.DEMERITOS = oValCal.DEMERITOS
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina la valoracion de calidad
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarValoracionCalidad(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim valCal = From valoraciones In BBDD.R_M_VALORACION_CALIDAD _
				Where valoraciones.ID_VALORACION = id _
				Select valoraciones

				If (valCal.Count = 1) Then
					BBDD.R_M_VALORACION_CALIDAD.DeleteOnSubmit(valCal.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_VALORACION_SERVICIO"

		''' <summary>
		''' Devuelve un registro valoracion servicio
		''' </summary>
		''' <param name="id">Id valoracion</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarValoracionServicio(ByVal id As Integer) As KaPlanLib.Registro.R_M_VALORACION_SERVICIO
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResVal = From valoraciones In BBDD.R_M_VALORACION_SERVICIO _
				Where valoraciones.ID_VALORACION = id _
				Select valoraciones

				If (ResVal.Any = False) Then
					Return Nothing
				Else
					Return ResVal.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la valoracion de servicio
		''' </summary>
		''' <param name="oValServ">Datos de la servicio</param>                        
		''' <remarks></remarks>
		Public Function GuardarValoracionServicio(ByVal oValServ As Registro.R_M_VALORACION_SERVICIO) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oValServ.ID_VALORACION = 0) Then	'Nuevo
					BBDD.R_M_VALORACION_SERVICIO.InsertOnSubmit(oValServ)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim valServ As KaPlanLib.Registro.R_M_VALORACION_SERVICIO = (From valoraciones In BBDD.R_M_VALORACION_SERVICIO _
					  Where valoraciones.ID_VALORACION = oValServ.ID_VALORACION _
					  Select valoraciones).FirstOrDefault
					If (valServ Is Nothing) Then Return False

					valServ.VALORACION = oValServ.VALORACION
					valServ.DEMERITOS = oValServ.DEMERITOS
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina la valoracion de servicio
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarValoracionServicio(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim valServ = From valoraciones In BBDD.R_M_VALORACION_SERVICIO _
				Where valoraciones.ID_VALORACION = id _
				Select valoraciones

				If (valServ.Count = 1) Then
					BBDD.R_M_VALORACION_SERVICIO.DeleteOnSubmit(valServ.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "M_FRECUENCIA_ENTREGA"

		''' <summary>
		''' Devuelve un registro frecuencia de entrega
		''' </summary>
		''' <param name="id">Id</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarFrecuenciaEntrega(ByVal id As Integer) As KaPlanLib.Registro.R_M_FRECUENCIAS_ENTREGA
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResFrec = From frecuencias In BBDD.R_M_FRECUENCIAS_ENTREGA _
				Where frecuencias.ID_FRECUENCIA = id _
				Select frecuencias

				If (ResFrec.Any = False) Then
					Return Nothing
				Else
					Return ResFrec.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Devuelve el listado de frecuencia de entrega
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarListadoFrecEntrega()
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim frecs = From frecuencias In BBDD.R_M_FRECUENCIAS_ENTREGA _
				Select frecuencias

				Return frecs
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la frecuencia de entrega
		''' </summary>
		''' <param name="oFrecEntr">Datos de la servicio</param>                        
		''' <remarks></remarks>
		Public Function GuardarFrecuenciaEntrega(ByVal oFrecEntr As Registro.R_M_FRECUENCIAS_ENTREGA) As Boolean
			Dim transaction As DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				If (oFrecEntr.ID_FRECUENCIA = 0) Then	'Nuevo
					BBDD.R_M_FRECUENCIAS_ENTREGA.InsertOnSubmit(oFrecEntr)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim frecEntr As KaPlanLib.Registro.R_M_FRECUENCIAS_ENTREGA = (From frecuencias In BBDD.R_M_FRECUENCIAS_ENTREGA _
					  Where frecuencias.ID_FRECUENCIA = oFrecEntr.ID_FRECUENCIA _
					  Select frecuencias).FirstOrDefault
					If (frecEntr Is Nothing) Then Return False

					frecEntr.FRECUENCIA_ENTREGA = oFrecEntr.FRECUENCIA_ENTREGA
					frecEntr.FREC_PREDETERMINADA = oFrecEntr.FREC_PREDETERMINADA
					BBDD.SubmitChanges()
				End If
				'Si se ha marcado como predeterminada, el resto, se le quita el check
				If (oFrecEntr.FREC_PREDETERMINADA) Then
					Dim lFrecEntr = From frecuencias In BBDD.R_M_FRECUENCIAS_ENTREGA _
					 Where frecuencias.ID_FRECUENCIA <> oFrecEntr.ID_FRECUENCIA _
					Select frecuencias

					If (lFrecEntr IsNot Nothing) Then
						For Each oFrecuency As Registro.R_M_FRECUENCIAS_ENTREGA In lFrecEntr
							oFrecuency.FREC_PREDETERMINADA = False
							BBDD.SubmitChanges()
						Next
					End If
				End If

				transaction.Commit()
				Return True
			Catch ex As Exception
				transaction.Rollback()
				Return False
			Finally
				BBDD.Connection.Close()
			End Try
		End Function

		''' <summary>
		''' Elimina la frecuencia de entrega
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarFrecuenciaEntrega(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim frecEntr = From frecuencias In BBDD.R_M_FRECUENCIAS_ENTREGA _
				Where frecuencias.ID_FRECUENCIA = id _
				Select frecuencias

				If (frecEntr.Count = 1) Then
					BBDD.R_M_FRECUENCIAS_ENTREGA.DeleteOnSubmit(frecEntr.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_M_PLAZOS"

		''' <summary>
		''' Devuelve un registro plazo
		''' </summary>
		''' <param name="id">Id plazo</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarPlazo(ByVal id As Integer) As KaPlanLib.Registro.R_M_PLAZOS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResPlaz = From plazos In BBDD.R_M_PLAZOS _
				Where plazos.ID_PLAZO = id _
				Select plazos

				If (ResPlaz.Any = False) Then
					Return Nothing
				Else
					Return ResPlaz.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el plazo
		''' </summary>
		''' <param name="oPlazo">Datos del plazo</param>                        
		''' <remarks></remarks>
		Public Function GuardarPlazo(ByVal oPlazo As Registro.R_M_PLAZOS) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oPlazo.ID_PLAZO = 0) Then	'Nuevo
					BBDD.R_M_PLAZOS.InsertOnSubmit(oPlazo)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim plazo As KaPlanLib.Registro.R_M_PLAZOS = (From plazos In BBDD.R_M_PLAZOS _
					  Where plazos.ID_PLAZO = oPlazo.ID_PLAZO _
					  Select plazos).FirstOrDefault
					If (plazo Is Nothing) Then Return False

					plazo.ID_FRECUENCIA = oPlazo.ID_FRECUENCIA
					plazo.PLAZO = oPlazo.PLAZO
					plazo.VALORACION = oPlazo.VALORACION
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina el plazo
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarPlazo(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim plazo = From plazos In BBDD.R_M_PLAZOS _
				Where plazos.ID_PLAZO = id _
				Select plazos

				If (plazo.Count = 1) Then
					BBDD.R_M_PLAZOS.DeleteOnSubmit(plazo.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "ACTUALIZAR VALORACIONES"

		''' <summary>
		''' Actualiza las valoraciones
		''' </summary>
		''' <param name="fDesde">Fecha desde</param>
		''' <param name="fHasta">Fecha hasta</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function ActualizarValoraciones(ByVal fDesde As Date, ByVal fHasta As Date) As Boolean
			Dim transaction As DbTransaction = Nothing
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				BBDD.Connection.Open()
				transaction = BBDD.Connection.BeginTransaction
				BBDD.Transaction = transaction

				'1º Obtiene las recepciones de brain
				Dim Recepciones = From brain In BBDD.R_BRAIN_RECEPCIONES_EXCEL _
				Where brain.WEWEDF >= fDesde And brain.WEWEDF <= fHasta _
				Select brain

				Dim demeritos, myWEWENR As Decimal
				Dim dias As Long

				If (Recepciones IsNot Nothing) Then
					For Each oRecep As Registro.R_BRAIN_RECEPCIONES_EXCEL In Recepciones
						dias = DateDiff(DateInterval.Day, oRecep.WEBEDF.Value, oRecep.WEWEDF.Value, FirstDayOfWeek.Monday, FirstWeekOfYear.Jan1)
						myWEWENR = oRecep.WEWENR
						'2º Consulta el demerito
						demeritos = consultarDemeritosServicios(oRecep.WELINR, oRecep.WETENR, Math.Abs(dias))

						'3º Obtiene las recepciones con el campo WEWNR indicado
						Dim BrainDemeritos = From demeBrain In BBDD.R_BRAIN_RECEPCIONES_EXCEL _
						 Where demeBrain.WEWENR = myWEWENR _
						 Select demeBrain
						If (BrainDemeritos IsNot Nothing) Then
							'4º Actualiza los demeritos
							For Each oRecep2 As Registro.R_BRAIN_RECEPCIONES_EXCEL In BrainDemeritos
								oRecep2.DEMERITOS = demeritos
								BBDD.SubmitChanges()
							Next
						End If
					Next
				End If

				transaction.Commit()
				Return True
			Catch ex As Exception
				transaction.Rollback()
				Throw New Exception("errGuardar", ex)
			Finally
				BBDD.Connection.Close()
			End Try
		End Function

		''' <summary>
		''' Consulta los demeritos de servicios
		''' </summary>
		''' <param name="CodProveedor"></param>
		''' <param name="CodArticulo"></param>
		''' <param name="diasDif"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function consultarDemeritosServicios(ByVal CodProveedor As String, ByVal CodArticulo As String, ByVal diasDif As Long) As Decimal
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim demeritos As Decimal = 0
			Dim RegDemerito As Registro.R_V_DEMERITOS = (From rvplazos In BBDD.R_V_PLAZOS Join rvDemeritos In BBDD.R_V_DEMERITOS _
			 On rvplazos.ID_PROVEEDOR Equals rvDemeritos.ID_PROVEEDOR And rvplazos.ID_ARTICULO Equals rvDemeritos.ID_ARTICULO _
			 Order By rvDemeritos.PLAZO Descending _
			 Where diasDif >= rvDemeritos.PLAZO And _
			rvplazos.COD_PROVEEDOR = CodProveedor And _
			rvplazos.COD_ARTICULO = CodArticulo _
			Select rvDemeritos).FirstOrDefault

			If (RegDemerito Is Nothing) Then
				Dim RegDemeritoFrec As Registro.R_V_FRECUENCIA_PREDETERMINADA = (From demeFrec In BBDD.R_V_FRECUENCIA_PREDETERMINADA _
				 Order By demeFrec.PLAZO Descending _
				 Where diasDif >= demeFrec.PLAZO _
				 Select demeFrec).FirstOrDefault
				If (RegDemeritoFrec IsNot Nothing) Then
					demeritos = RegDemeritoFrec.DEMERITOS
				End If
			Else
				demeritos = RegDemerito.DEMERITOS
			End If
			Return demeritos
		End Function

#End Region

#Region "FRECUENCIA ENTREGAS PROVEEDOR/ARTICULO"

		''' <summary>
		''' Devuelve el registro de frecuencia de entrega de proveedores y articulos
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarFrecuenciaPA(ByVal id As Integer) As Registro.R_FRECUENCIAS_PA
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim regFrecuencia = From frecPA In BBDD.R_FRECUENCIAS_PA _
				 Where frecPA.ID_FRECUENCIA_PA = id _
				Select frecPA

				If (regFrecuencia.Any = False) Then
					Return Nothing
				Else
					Return regFrecuencia.First
				End If

				Return regFrecuencia
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Devuelve el listado de frecuencia de entrega de proveedores y articulos
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarListadoFrecuenciaPA(ByVal bSinFrecuencia As Boolean) As List(Of Registro.R_FRECUENCIAS_PA)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim frecuencias As List(Of Registro.R_FRECUENCIAS_PA)
				If (Not bSinFrecuencia) Then 'Todos
					frecuencias = (From frecPA In BBDD.R_FRECUENCIAS_PA _
					  Order By frecPA.R_M_PROVEEDORES.DES_PROVEEDOR _
					  Select frecPA).ToList
				Else
					frecuencias = (From frecPA In BBDD.R_FRECUENCIAS_PA _
					  Order By frecPA.R_M_PROVEEDORES.DES_PROVEEDOR _
					  Where CInt(If(CType(frecPA.ID_FRECUENCIA, Decimal?) Is Nothing, 0, frecPA.ID_FRECUENCIA)) = 0 _
					  Select frecPA).ToList
				End If
				Return frecuencias
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

        ''' <summary>
        ''' Guarda la frecuencia de entrega
        ''' </summary>
        ''' <param name="oFrec">Objeto a guardar</param>        
        ''' <remarks></remarks>
        Public Sub GuardarFrecuenciaPA(ByVal oFrec As Registro.R_FRECUENCIAS_PA)
            Dim BBDD As New KaPlanLib.DAL.ELL

            If (oFrec.ID_FRECUENCIA_PA = 0) Then    'Nuevo
                    BBDD.R_FRECUENCIAS_PA.InsertOnSubmit(oFrec)
                    BBDD.SubmitChanges()
                Else  'Modificar
                    Dim frec As KaPlanLib.Registro.R_FRECUENCIAS_PA = (From frecuencias In BBDD.R_FRECUENCIAS_PA
                                                                       Where frecuencias.ID_FRECUENCIA_PA = oFrec.ID_FRECUENCIA_PA
                                                                       Select frecuencias).FirstOrDefault
                If frec IsNot Nothing Then
                    frec.ID_ARTICULO = oFrec.ID_ARTICULO
                    frec.ID_FRECUENCIA = oFrec.ID_FRECUENCIA
                    frec.ID_PROVEEDOR = oFrec.ID_PROVEEDOR
                    BBDD.SubmitChanges()
                End If
            End If
        End Sub

        ''' <summary>
        ''' Elimina la frecuencia de entrega
        ''' </summary>
        ''' <param name="id">Id</param>        
        ''' <remarks></remarks>
        Public Sub EliminarFrecuenciaPA(ByVal id As Integer)
            Dim BBDD As New KaPlanLib.DAL.ELL
            Dim frec = From frecuencias In BBDD.R_FRECUENCIAS_PA Where frecuencias.ID_FRECUENCIA_PA = id Select frecuencias
            If frec.Any Then
                BBDD.R_FRECUENCIAS_PA.DeleteOnSubmit(frec.First)
                BBDD.SubmitChanges()
            End If
        End Sub

#End Region

#Region "R_M_ARTICULOS"

        ''' <summary>
        ''' Devuelve el registro del articulo
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function consultarArticulo(ByVal id As Integer) As Registro.R_M_ARTICULOS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim regArticulo = From art In BBDD.R_M_ARTICULOS _
				 Where art.ID_ARTICULO = id _
				Select art

				If (regArticulo.Any = False) Then
					Return Nothing
				Else
					Return regArticulo.First
				End If

				Return regArticulo
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function


		''' <summary>
		''' Devuelve el registro del articulo
		''' </summary>
		''' <param name="cod"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarArticulo(ByVal cod As String) As Registro.R_M_ARTICULOS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim regArticulo = From art In BBDD.R_M_ARTICULOS _
				 Where art.CODIGO = cod _
				Select art

				If (regArticulo.Any = False) Then
					Return Nothing
				Else
					Return regArticulo.First
				End If

				Return regArticulo
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function


		''' <summary>
		''' Devuelve la lista de articulos
		''' </summary>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarListadoArticulos(ByVal oArt As Registro.R_M_ARTICULOS)
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim listaArticulos
				If (oArt IsNot Nothing) Then
					listaArticulos = From articulos In BBDD.R_M_ARTICULOS _
					 Order By articulos.CODIGO _
					 Where articulos.CODIGO.Contains(oArt.CODIGO) And _
					 articulos.DENOMINACION.Contains(oArt.DENOMINACION) _
					 Select articulos
				Else
					listaArticulos = From articulos In BBDD.R_M_ARTICULOS _
					Order By articulos.CODIGO _
					Select articulos
				End If

				Return listaArticulos
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda el articulo
		''' </summary>
		''' <param name="oArt">Objeto a guardar</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarArticulo(ByVal oArt As Registro.R_M_ARTICULOS) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oArt.ID_ARTICULO = 0) Then	'Nuevo
					BBDD.R_M_ARTICULOS.InsertOnSubmit(oArt)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim art As KaPlanLib.Registro.R_M_ARTICULOS = (From articulos In BBDD.R_M_ARTICULOS _
					 Where articulos.ID_ARTICULO = oArt.ID_ARTICULO _
					 Select articulos).FirstOrDefault
					If (art Is Nothing) Then Return False

					art.CODIGO = oArt.CODIGO
					art.COMPRA = oArt.COMPRA
					art.DENOMINACION = oArt.DENOMINACION
					art.MATERIAL = oArt.MATERIAL
					art.LANTEGI_RH = oArt.LANTEGI_RH
					art.MEDIDAS = oArt.MEDIDAS
					art.PRECIO = oArt.PRECIO
					art.COD_PLANO = oArt.COD_PLANO
					art.COD_ESPECIFICACION_TECNICA = oArt.COD_ESPECIFICACION_TECNICA
					art.N_PLANO = oArt.N_PLANO
					art.N_ESPECIFICACION_TECNICA = oArt.N_ESPECIFICACION_TECNICA
					art.F_PLANO = oArt.F_PLANO
					art.F_ESPECIFICACION_TECNICA = oArt.F_ESPECIFICACION_TECNICA
					art.PLANO = oArt.PLANO
					art.ESPECIFICACION_TECNICA = oArt.ESPECIFICACION_TECNICA
					If (oArt.NIVEL_HOMOLOGADO <> Date.MinValue) Then art.NIVEL_HOMOLOGADO = oArt.NIVEL_HOMOLOGADO
					art.NIVEL_HOMOLOGADO = oArt.NIVEL_HOMOLOGADO
					art.HOMOLOGACION = oArt.HOMOLOGACION
					If (oArt.id_clase <> "0") Then
						art.id_clase = oArt.id_clase
					Else
						art.id_clase = Nothing
					End If
					art.LANTEGI = oArt.LANTEGI
					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina el articulo
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarArticulo(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim art = From articulos In BBDD.R_M_ARTICULOS _
				Where articulos.ID_ARTICULO = id _
				Select articulos

				If (art.Count = 1) Then
					BBDD.R_M_ARTICULOS.DeleteOnSubmit(art.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_BRAIN_ARTICULOS_EXCEL"

		''' <summary>
		''' Devuelve el registro del articulo de brain
		''' </summary>
		''' <param name="cod"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarArticuloBrainExcel(ByVal cod As String) As Registro.R_BRAIN_ARTICULOS_EXCEL
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL
				Dim regArticulo = From art In BBDD.R_BRAIN_ARTICULOS_EXCEL _
				 Where art.TETENR = cod _
				Select art

				If (regArticulo.Any = False) Then
					Return Nothing
				Else
					Return regArticulo.First
				End If

				Return regArticulo
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

        ''' <summary>
        ''' Vuelca los articulos brain a R_M_ARTICULOS
        ''' </summary>
        ''' <param name="lArticulosB">Lista de articulos brain</param>        
        ''' <remarks></remarks>
        Public Sub VolcarArticulosBrain(ByVal lArticulosB As List(Of Registro.R_BRAIN_ARTICULOS_EXCEL))
            Dim BBDD As New KaPlanLib.DAL.ELL
            Dim transaction As DbTransaction = Nothing
            Try
                Dim oArt As Registro.R_M_ARTICULOS
                BBDD.Connection.Open()
                transaction = BBDD.Connection.BeginTransaction
                BBDD.Transaction = transaction

                For Each oArtBrain In lArticulosB
                    oArt = New Registro.R_M_ARTICULOS
                    oArt.CODIGO = oArtBrain.TETENR
                    oArt.DENOMINACION = oArtBrain.TEBEZ1
                    oArt.DENOMINACION_2 = oArtBrain.TEBEZ2
                    oArt.LANTEGI_RH = oArtBrain.TEWKNR
                    oArt.MATERIAL = oArtBrain.TEWKSF
                    oArt.MEDIDAS = oArtBrain.TEWKFE
                    oArt.COD_ESPECIFICACION_TECNICA = oArtBrain.TEESVA
                    oArt.N_PLANO = oArtBrain.TEZINR
                    oArt.F_PLANO = oArtBrain.TEASBZ
                    oArt.N_ESPECIFICACION_TECNICA = oArtBrain.TEVAAU
                    oArt.COMPRA = oArtBrain.TETART
                    BBDD.R_M_ARTICULOS.InsertOnSubmit(oArt)
                    BBDD.SubmitChanges()
                Next

                transaction.Commit()
            Catch ex As Exception
                transaction.Rollback()
                Throw
            Finally
                BBDD.Connection.Close()
            End Try
        End Sub

#End Region

#Region "Volcado de datos desde el AS400 (BRAIN)"
        ''' <summary>
        ''' Volcado de Proveedores del AS400 (BRAIN)
        ''' </summary>
        ''' <param name="CadenaConexion">Cadena de conexion para la BBDD</param>
        ''' <remarks></remarks>
        Public Sub Volcar_Proveedores_AS400(ByVal CadenaConexion As String)
			Dim BBDD As New KaPlanLib.DAL.ELL(CadenaConexion)
			'---------------------------------------------------------------------------------------------------------------------------------------------------------
			'Dim ProveedoresListado = _
			' From R_V_PROVEEDORES_AS400 As KaPlanLib.Registro.R_V_PROVEEDORES_AS400 In BBDD.R_V_PROVEEDORES_AS400 _
			' Join R_V_PAISES_AS400 As KaPlanLib.Registro.R_V_PAISES_AS400 In BBDD.R_V_PAISES_AS400 On R_V_PROVEEDORES_AS400.PAIS Equals R_V_PAISES_AS400.COD_PAIS _
			' Join R_V_PRODUCTOS_AS400 As KaPlanLib.Registro.R_V_PRODUCTOS_AS400 In BBDD.R_V_PRODUCTOS_AS400 _
			' On R_V_PRODUCTOS_AS400.COD_PRODUCTO Equals R_V_PROVEEDORES_AS400.PRODUCTO _
			' Group Join R_BRAIN_PROVEEDORES_EXCEL As KaPlanLib.Registro.R_BRAIN_PROVEEDORES_EXCEL In BBDD.R_BRAIN_PROVEEDORES_EXCEL _
			' On CType(R_V_PROVEEDORES_AS400.COD_PROVEEDOR, String).Trim Equals R_BRAIN_PROVEEDORES_EXCEL.LILINR.Trim Into ProveedoresExcel = Group _
			' From ProveedoresExcelGroup In ProveedoresExcel.DefaultIfEmpty _
			' Where ProveedoresExcelGroup.LILINR Is Nothing _
			' Select COD_PROVEEDOR = CType(R_V_PROVEEDORES_AS400.COD_PROVEEDOR, Decimal?), _
			' R_V_PROVEEDORES_AS400.DES_PROVEEDOR, _
			' R_V_PROVEEDORES_AS400.DIRECCION, _
			' R_V_PROVEEDORES_AS400.CODIGO_POSTAL, _
			' R_V_PROVEEDORES_AS400.POBLACION, _
			' R_V_PAISES_AS400.DES_PAIS, _
			' R_V_PROVEEDORES_AS400.CIF, _
			' PRODUCTO = R_V_PRODUCTOS_AS400.DES_PRODUCTO, _
			' R_V_PROVEEDORES_AS400.TELEFONO, _
			' R_V_PROVEEDORES_AS400.FAX, _
			' VOLCAR = 0
			'---------------------------------------------------------------------------------------------------------------------------------------------------------
			'Cambiar consulta para no tener en cuenta "Código Producto" y "Código País".
			'---------------------------------------------------------------------------------------------------------------------------------------------------------
			'Join R_V_PAISES_AS400 As KaPlanLib.Registro.R_V_PAISES_AS400 In BBDD.R_V_PAISES_AS400 On R_V_PROVEEDORES_AS400.PAIS Equals R_V_PAISES_AS400.COD_PAIS _
			'Join R_V_PRODUCTOS_AS400 As KaPlanLib.Registro.R_V_PRODUCTOS_AS400 In BBDD.R_V_PRODUCTOS_AS400 On R_V_PRODUCTOS_AS400.COD_PRODUCTO Equals R_V_PROVEEDORES_AS400.PRODUCTO _
			Dim ProveedoresListado = _
			 From R_V_PROVEEDORES_AS400 As KaPlanLib.Registro.R_V_PROVEEDORES_AS400 In BBDD.R_V_PROVEEDORES_AS400 _
			 Group Join RV_PAIS_AS400 As KaPlanLib.Registro.R_V_PAISES_AS400 In BBDD.R_V_PAISES_AS400 On R_V_PROVEEDORES_AS400.PAIS Equals RV_PAIS_AS400.COD_PAIS Into Pais_AS400 = Group From R_V_PAISES_AS400 In Pais_AS400.DefaultIfEmpty _
			 Group Join RV_PRODUCTO_AS400 As KaPlanLib.Registro.R_V_PRODUCTOS_AS400 In BBDD.R_V_PRODUCTOS_AS400 On R_V_PROVEEDORES_AS400.PRODUCTO Equals RV_PRODUCTO_AS400.COD_PRODUCTO Into RV_PRODUCTO_AS400 = Group From R_V_PRODUCTOS_AS400 In RV_PRODUCTO_AS400.DefaultIfEmpty _
			 Group Join R_BRAIN_PROVEEDORES_EXCEL As KaPlanLib.Registro.R_BRAIN_PROVEEDORES_EXCEL In BBDD.R_BRAIN_PROVEEDORES_EXCEL _
			 On CType(R_V_PROVEEDORES_AS400.COD_PROVEEDOR, String).Trim Equals R_BRAIN_PROVEEDORES_EXCEL.LILINR.Trim Into ProveedoresExcel = Group _
			 From ProveedoresExcelGroup In ProveedoresExcel.DefaultIfEmpty _
			 Where ProveedoresExcelGroup.LILINR Is Nothing _
			 Select COD_PROVEEDOR = CType(R_V_PROVEEDORES_AS400.COD_PROVEEDOR, Decimal?), _
			 R_V_PROVEEDORES_AS400.DES_PROVEEDOR, _
			 R_V_PROVEEDORES_AS400.DIRECCION, _
			 R_V_PROVEEDORES_AS400.CODIGO_POSTAL, _
			 R_V_PROVEEDORES_AS400.POBLACION, _
			 R_V_PAISES_AS400.DES_PAIS, _
			 R_V_PROVEEDORES_AS400.CIF, _
			 PRODUCTO = R_V_PRODUCTOS_AS400.DES_PRODUCTO, _
			 R_V_PROVEEDORES_AS400.TELEFONO, _
			 R_V_PROVEEDORES_AS400.FAX, _
			 VOLCAR = 0
			'---------------------------------------------------------------------------------------------------------------------------------------------------------

			For Each ItemLista In ProveedoresListado
				Dim R_BRAIN_PROVEEDORES_EXCEL As New KaPlanLib.Registro.R_BRAIN_PROVEEDORES_EXCEL
				R_BRAIN_PROVEEDORES_EXCEL.LILINR = ItemLista.COD_PROVEEDOR
				R_BRAIN_PROVEEDORES_EXCEL.LINAME = ItemLista.DES_PROVEEDOR
				R_BRAIN_PROVEEDORES_EXCEL.LISTRA = ItemLista.DIRECCION
				R_BRAIN_PROVEEDORES_EXCEL.LIPOLZ = ItemLista.CODIGO_POSTAL
				R_BRAIN_PROVEEDORES_EXCEL.LIWORT = ItemLista.POBLACION
				R_BRAIN_PROVEEDORES_EXCEL.LILAKZ = ItemLista.DES_PAIS
				R_BRAIN_PROVEEDORES_EXCEL.LIIDNR = ItemLista.CIF
				R_BRAIN_PROVEEDORES_EXCEL.LILIAR = ItemLista.PRODUCTO
				R_BRAIN_PROVEEDORES_EXCEL.LITLNR = ItemLista.TELEFONO
				R_BRAIN_PROVEEDORES_EXCEL.LITFAX = ItemLista.FAX
				R_BRAIN_PROVEEDORES_EXCEL.VOLCAR = ItemLista.VOLCAR

				BBDD.R_BRAIN_PROVEEDORES_EXCEL.InsertOnSubmit(R_BRAIN_PROVEEDORES_EXCEL)
			Next
			BBDD.SubmitChanges()
		End Sub
		''' <summary>
		''' Volcado de Articulos del AS400 (BRAIN)
		''' </summary>
		''' <remarks></remarks>
		Sub Volcar_Articulos_AS400(ByVal CadenaConexion As String)
			Dim BBDD As New KaPlanLib.DAL.ELL(CadenaConexion)

            Dim ArticulosListado =
            From ARTICULOS As KaPlanLib.Registro.R_V_ARTICULOS_AS400_AGRUP In BBDD.R_V_ARTICULOS_AS400_AGRUP
            Group Join ArticulosBrain As KaPlanLib.Registro.R_BRAIN_ARTICULOS_EXCEL In BBDD.R_BRAIN_ARTICULOS_EXCEL
            On ARTICULOS.COD_ARTICULO Equals ArticulosBrain.TETENR Into ArtBrain = Group
            From ArtBrainGroup In ArtBrain.DefaultIfEmpty
            Where ArtBrainGroup.TETENR Is Nothing
            Select ARTICULOS

            For Each Articulo As KaPlanLib.Registro.R_V_ARTICULOS_AS400_AGRUP In ArticulosListado
                Dim ArticulosBrain As New KaPlanLib.Registro.R_BRAIN_ARTICULOS_EXCEL
                ArticulosBrain.TETENR = Articulo.COD_ARTICULO
                ArticulosBrain.TEBEZ1 = Articulo.DES_ARTICULO
                ArticulosBrain.TEBEZ2 = Articulo.DES_ARTICULO_2
                ArticulosBrain.TEWKNR = Articulo.LANTEGI
                ArticulosBrain.TEWKSF = Articulo.MATERIAL
                ArticulosBrain.TEWKFE = Articulo.MEDIDAS
                ArticulosBrain.TEESVA = Articulo.ESPECIFICACION_TECNICA
                ArticulosBrain.TEZINR = Articulo.N_PLANO
                ArticulosBrain.TEASBZ = Articulo.F_PLANO
                ArticulosBrain.TEVAAU = Articulo.N_ESPECIFICACION_TECNICA
                ArticulosBrain.TETART = Articulo.COMPRA
                ArticulosBrain.VOLCAR = 0

                BBDD.R_BRAIN_ARTICULOS_EXCEL.InsertOnSubmit(ArticulosBrain)
            Next
            BBDD.SubmitChanges()
		End Sub
		''' <summary>
		''' Volcado de Articulos Recepcionados en el AS400 (BRAIN)
		''' </summary>
		''' <remarks></remarks>
		Sub Volcar_Recepciones_AS400(ByVal CadenaConexion As String)
			Dim n_Recepcion As Decimal
			Dim Demeritos As Double
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL(CadenaConexion)
                Dim RECEPCIONES_AS400 =
                From R_V_RECEPCIONES_AS400 As KaPlanLib.Registro.R_V_RECEPCIONES_AS400 In BBDD.R_V_RECEPCIONES_AS400
                Group Join R_BRAIN_RECEPCIONES_EXCEL As KaPlanLib.Registro.R_BRAIN_RECEPCIONES_EXCEL In BBDD.R_BRAIN_RECEPCIONES_EXCEL
                On R_V_RECEPCIONES_AS400.N_RECEPCION Equals R_BRAIN_RECEPCIONES_EXCEL.WEWENR Into RECEPCIONES_Group = Group
                From RECEPCIONES In RECEPCIONES_Group.DefaultIfEmpty
                Where RECEPCIONES.WEWENR.ToString Is Nothing
                Select R_V_RECEPCIONES_AS400

                For Each ItemRecep In RECEPCIONES_AS400
                    If ItemRecep.FECHA_RECEPCION.Trim <> String.Empty And ItemRecep.FECHA_PREVISTA.Trim <> String.Empty Then
                        Dim FechaRecepcion As Date = Date.ParseExact(ItemRecep.FECHA_RECEPCION, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
                        Dim FechaPrevista As Date = Date.ParseExact(ItemRecep.FECHA_PREVISTA, "dd/MM/yyyy", Globalization.CultureInfo.InvariantCulture)
                        Demeritos = System.Math.Abs(DateDiff(DateInterval.Day, FechaRecepcion, FechaPrevista))
                        If ItemRecep.N_RECEPCION.ToString Is Nothing Then n_Recepcion = 0 Else n_Recepcion = ItemRecep.N_RECEPCION

                        'log.Debug(String.Format("P_InsertRecepcionesAS400({0}, {1})", n_Recepcion, Demeritos))

                        BBDD.P_InsertRecepcionesAS400(n_Recepcion, Demeritos)
                    End If
                Next
            Catch ex As Exception
                log.Error("-n_Recepcion: " & n_Recepcion & " -Demeritos: " & Demeritos & " -CadenaConexion: " & CadenaConexion, ex)
                Throw
            End Try
		End Sub
		''' <summary>
		''' Obtenemos la fecha de actualizacion de la Base de datos para de la conexion.
		''' </summary>
		''' <param name="Conexion"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function FechaActualizacion(ByVal Conexion As String) As Nullable(Of Date)
			Dim BBDD As New KaPlanLib.DAL_KaPlan.ELL
			Dim Reg As KaPlanLib.Registro.Actualizaciones = (From Actualizacion As KaPlanLib.Registro.Actualizaciones In BBDD.Actualizaciones _
			 Where Actualizacion.Catalogo = CatalogoConexion(Conexion)).FirstOrDefault
			If Reg IsNot Nothing Then FechaActualizacion = Reg.Fecha

			Return FechaActualizacion
		End Function
		''' <summary>
		''' Se obtiene el nombre de la base de datos de la cadena de conexion.
		''' </summary>
		''' <param name="Conexion">Cadena de conexion a la base de datos.</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function CatalogoConexion(ByVal Conexion As String) As String
			Dim ConexionArray As Array = Conexion.Split(";")
			CatalogoConexion = Nothing
			For Each item As String In ConexionArray
				Dim Propiedad As Array = item.Split("=")
				If Propiedad(0) = "Initial Catalog" Then CatalogoConexion = Propiedad(1) : Exit For
			Next item
			Return CatalogoConexion
		End Function
		''' <summary>
		''' Proceso para indicar la fecha de actualizacion.
		''' </summary>
		''' <param name="Conexion"></param>
		''' <remarks></remarks>
		Private Sub ActualizarFecha(ByVal Conexion As String)
			Dim BBDD As New KaPlanLib.DAL_KaPlan.ELL()
			Dim RegActualizacion As KaPlanLib.Registro.Actualizaciones = (From Actualizacion As KaPlanLib.Registro.Actualizaciones In BBDD.Actualizaciones _
			 Where Actualizacion.Catalogo = CatalogoConexion(Conexion)).FirstOrDefault

			If RegActualizacion Is Nothing Then
				RegActualizacion = New KaPlanLib.Registro.Actualizaciones
				RegActualizacion.Catalogo = CatalogoConexion(Conexion)
				RegActualizacion.Conexion = Conexion
				RegActualizacion.Fecha = Now
				BBDD.Actualizaciones.InsertOnSubmit(RegActualizacion)
			Else
				RegActualizacion.Fecha = Now
			End If

			BBDD.SubmitChanges()
		End Sub
        ''' <summary>
        ''' Actulizacion de la base de datos de la conexion.
        ''' </summary>
        ''' <param name="Conexion">Cadena de Conexion a la base de datos.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function ActualizarDatos(ByVal Conexion As String) As Nullable(Of Date)
            Dim FuncionesBBDD As New KaPlanLib.BLL.LinqComponent
            '1º
            FuncionesBBDD.Volcar_Articulos_AS400(Conexion)
            '2º
            FuncionesBBDD.Volcar_Proveedores_AS400(Conexion)
            '3º
            FuncionesBBDD.Volcar_Recepciones_AS400(Conexion)
            '4º 
            ActualizarFecha(Conexion)

            Return FechaActualizacion(Conexion)
        End Function
        ''' <summary>
        '''Actulizacion de las bases de datos del KaPlan.
        ''' </summary>
        ''' <remarks></remarks>
        Sub ActualizarDatos()
			Dim BBDD As New KaPlanLib.DAL_KaPlan.ELL
			Dim Reg As IQueryable(Of KaPlanLib.Registro.Actualizaciones) = From Actualizacion As KaPlanLib.Registro.Actualizaciones In BBDD.Actualizaciones
			Try
				If Reg IsNot Nothing AndAlso Reg.Any Then
					For Each item As KaPlanLib.Registro.Actualizaciones In Reg
						Dim BBDD2 As New KaPlanLib.DAL.ELL(item.Conexion) 'Base de datos a actualizar.
						BBDD2.DatabaseExists()
						If BBDD2.DatabaseExists = True Then ActualizarDatos(item.Conexion)
					Next
				End If
			Catch ex As Exception
				log.Error(ex)
				Throw New Exception("error", ex)
			End Try
		End Sub
#End Region

#Region "R_FACTURAS"

		''' <summary>
		''' Devuelve un registro factura
		''' </summary>
		''' <param name="id">Id factura</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarFactura(ByVal id As Integer) As KaPlanLib.Registro.R_FACTURAS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResFac = From facturas In BBDD.R_FACTURAS _
				Where facturas.ID_FACTURA = id _
				Select facturas

				If (ResFac.Any = False) Then
					Return Nothing
				Else
					Return ResFac.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la factura
		''' </summary>
		''' <param name="oFactura">Datos de la factura</param>                        
		''' <returns>Identificador</returns>
		Public Function GuardarFactura(ByVal oFactura As Registro.R_FACTURAS) As Integer
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim idFactura As Integer = 0
			Try
				If (oFactura.ID_FACTURA = 0) Then	'Nuevo
					Dim añoFactura As String = String.Format("{0:yy}", Date.Now)
					'Se genera un nuevo N_FACTURA
					Dim max As String = (From vistaFac In BBDD.R_V_Facturas _
					   Where vistaFac.AÑO_FACTURA = añoFactura _
					   Select vistaFac.NUM_FACTURA).Max
					If (String.IsNullOrWhiteSpace(max)) Then max = "0"
					oFactura.ESTADO = "P"  'Pendiente
					oFactura.IVA = 16
					oFactura.N_FACTURA = añoFactura & "/P/" & String.Format("{0:00000}", CInt(max) + 1)

					BBDD.R_FACTURAS.InsertOnSubmit(oFactura)
					BBDD.SubmitChanges()
					idFactura = oFactura.ID_FACTURA
				Else  'Modificar
					Dim factura As KaPlanLib.Registro.R_FACTURAS = (From facturas In BBDD.R_FACTURAS _
					  Where facturas.ID_FACTURA = oFactura.ID_FACTURA _
					  Select facturas).FirstOrDefault
					If (factura Is Nothing) Then Return Integer.MinValue

					factura.FECHA_FACTURA = oFactura.FECHA_FACTURA
					factura.LANTEGI = oFactura.LANTEGI
					factura.COD_PROVEEDOR = oFactura.COD_PROVEEDOR
					factura.LANTEGI_RH = oFactura.LANTEGI_RH
					factura.SOLICITANTE = oFactura.SOLICITANTE
					factura.ESTADO = oFactura.ESTADO
					factura.SU_FECHA_PEDIDO = oFactura.SU_FECHA_PEDIDO
					factura.SU_REFERENCIA = oFactura.SU_REFERENCIA
					factura.SU_PEDIDO = oFactura.SU_PEDIDO
					factura.N_REFERENCIA = oFactura.N_REFERENCIA
					factura.MONEDA = oFactura.MONEDA
					factura.BASE = oFactura.BASE
					factura.IVA = oFactura.IVA
					idFactura = oFactura.ID_FACTURA
					BBDD.SubmitChanges()
				End If

				Return idFactura
			Catch ex As Exception
				Return Integer.MinValue
			End Try
		End Function

		''' <summary>
		''' Elimina la factura
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarFactura(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim factura = From facturas In BBDD.R_FACTURAS _
				  Where facturas.ID_FACTURA = id _
				  Select facturas

				If (factura.Count = 1) Then
					BBDD.R_FACTURAS.DeleteOnSubmit(factura.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_LINEAS_FACTURA"

		''' <summary>
		''' Devuelve un registro linea factura
		''' </summary>
		''' <param name="id">Id linea factura</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Function consultarLineaFactura(ByVal id As Integer) As KaPlanLib.Registro.R_LINEAS_FACTURA
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim ResLineaFac = From lineas In BBDD.R_LINEAS_FACTURA _
				Where lineas.ID_LINEA_FACTURA = id _
				Select lineas

				If (ResLineaFac.Any = False) Then
					Return Nothing
				Else
					Return ResLineaFac.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la linea de una factura
		''' </summary>
		''' <param name="oLineaFac">Datos de la linea de una factura</param>                        
		''' <returns>Booleano</returns>
		Public Function GuardarLineaFactura(ByVal oLineaFac As Registro.R_LINEAS_FACTURA) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oLineaFac.ID_LINEA_FACTURA = 0) Then	'Nuevo
					BBDD.R_LINEAS_FACTURA.InsertOnSubmit(oLineaFac)
					BBDD.SubmitChanges()
				Else  'Modificar
					Dim lineaFactura As KaPlanLib.Registro.R_LINEAS_FACTURA = (From lineas In BBDD.R_LINEAS_FACTURA _
					  Where lineas.ID_LINEA_FACTURA = oLineaFac.ID_LINEA_FACTURA _
					  Select lineas).FirstOrDefault
					If (lineaFactura Is Nothing) Then Return Integer.MinValue

					lineaFactura.DESCRIPCION = oLineaFac.DESCRIPCION
					lineaFactura.REFERENCIA = oLineaFac.REFERENCIA
					lineaFactura.IMPORTE = oLineaFac.IMPORTE
					lineaFactura.PRECIO = oLineaFac.PRECIO
					lineaFactura.CANTIDAD = oLineaFac.CANTIDAD

					BBDD.SubmitChanges()
				End If

				Return True
			Catch ex As Exception
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina la linea de la factura
		''' </summary>
		''' <param name="id">Id</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarLineaFactura(ByVal id As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim lineasFactura = From lineas In BBDD.R_LINEAS_FACTURA _
				  Where lineas.ID_LINEA_FACTURA = id _
				  Select lineas

				If (lineasFactura.Count = 1) Then
					BBDD.R_LINEAS_FACTURA.DeleteOnSubmit(lineasFactura.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_CARACTERISTICA_AUDITORIA"

		''' <summary>
		''' Consulta la caraceristica de la auditoria
		''' </summary>
		''' <returns></returns>
		Function consultarCaracteristicaAuditoria(ByVal idRegistro As Integer) As Registro.R_CARACTERISTICAS_AUDITORIA
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim RegCaract = From Caracteristicas In BBDD.R_CARACTERISTICAS_AUDITORIA _
				  Where Caracteristicas.ID_CARAC = idRegistro _
				  Select Caracteristicas

				If (RegCaract.Any = False) Then
					Return Nothing
				Else
					Return RegCaract.First
				End If
			Catch ex As Exception
				log.Error(ex)
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la caracteristica de la auditoria
		''' </summary>
		''' <param name="oCaract">Caracteristica de la auditoria</param>		
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GuardarCaracteristicaAuditoria(ByVal oCaract As KaPlanLib.Registro.R_CARACTERISTICAS_AUDITORIA) As Boolean
			Dim BBDD As New KaPlanLib.DAL.ELL
			Try
				If (oCaract.ID_CARAC = 0) Then 'insertar
					BBDD.R_CARACTERISTICAS_AUDITORIA.InsertOnSubmit(oCaract)
				Else 'actualizar
					Dim caractAudit As KaPlanLib.Registro.R_CARACTERISTICAS_AUDITORIA = (From caracteristica In BBDD.R_CARACTERISTICAS_AUDITORIA _
					   Where caracteristica.ID_CARAC = oCaract.ID_CARAC _
					   Select caracteristica).FirstOrDefault

					If (caractAudit IsNot Nothing) Then
						caractAudit.CARACTERISTICA = oCaract.CARACTERISTICA
						caractAudit.CLASE = oCaract.CLASE
						caractAudit.DECISION = oCaract.DECISION
						caractAudit.ESPECIFICACION = oCaract.ESPECIFICACION
						caractAudit.ID_AUDITORIA = oCaract.ID_AUDITORIA
						caractAudit.MAXIM = oCaract.MAXIM
						caractAudit.METODO_CONTROL = oCaract.METODO_CONTROL
						caractAudit.MINIM = oCaract.MINIM
						caractAudit.ORDEN_CARAC = oCaract.ORDEN_CARAC
						caractAudit.TAMAÑO = oCaract.TAMAÑO
						caractAudit.TECNICA_EVALUACION = oCaract.TECNICA_EVALUACION
					Else
						Return False
					End If
				End If
				BBDD.SubmitChanges()

				Return True
			Catch ex As Exception
				log.Error(ex)
				Return False
			End Try
		End Function

		''' <summary>
		''' Elimina una caracteristica de la auditoria
		''' </summary>
		''' <param name="idRegistro">Identificador de la caracteristica</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarCaracteristicaAuditoria(ByVal idRegistro As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim Caracteristica = From Caract In BBDD.R_CARACTERISTICAS_AUDITORIA _
				 Where Caract.ID_CARAC = idRegistro _
				 Select Caract

				If (Caracteristica.Count = 1) Then
					BBDD.R_CARACTERISTICAS_AUDITORIA.DeleteOnSubmit(Caracteristica.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				log.Error(ex)
				Return False
			End Try
		End Function

#End Region

#Region "R_AUDITORIAS"

		''' <summary>
		''' Consulta la auditoria
		''' </summary>
		''' <returns></returns>
		Function consultarAuditoria(ByVal idRegistro As Integer) As Registro.R_AUDITORIAS
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim RegAudit = From auditoria In BBDD.R_AUDITORIAS _
				  Where auditoria.ID_AUDITORIA = idRegistro _
				  Select auditoria

				If (RegAudit.Any = False) Then
					Return Nothing
				Else
					Return RegAudit.First
				End If
			Catch ex As Exception
				Throw New Exception("error", ex)
			End Try
		End Function

		''' <summary>
		''' Guarda la auditoria
		''' </summary>
		''' <param name="oAudit">Auditoria</param>		
		''' <returns>Devuelve el identificador de la auditoria</returns>
		''' <remarks></remarks>
		Public Function GuardarAuditoria(ByVal oAudit As KaPlanLib.Registro.R_AUDITORIAS) As Integer
			Dim BBDD As New KaPlanLib.DAL.ELL
			Dim idAuditoria As Integer = Integer.MinValue
			Try
				If (oAudit.ID_AUDITORIA = 0) Then 'insertar
					Dim nsAuditoria As String = String.Empty

					Dim nAuditoria As String = (From audit In BBDD.R_AUDITORIAS _
					 Where audit.TIPO_AUDITORIA = oAudit.TIPO_AUDITORIA And audit.TIPO = oAudit.TIPO _
					 Select audit.N_AUDITORIA.Substring(0, 5)).Max
					If (String.IsNullOrWhiteSpace(nAuditoria)) Then nAuditoria = "0"

					nsAuditoria = String.Format("{0:00000}", CInt(nAuditoria) + 1)
					nsAuditoria &= "/EXT/" & oAudit.TIPO_AUDITORIA

					oAudit.N_AUDITORIA = nsAuditoria

					BBDD.R_AUDITORIAS.InsertOnSubmit(oAudit)
					BBDD.SubmitChanges()
					idAuditoria = oAudit.ID_AUDITORIA
				Else 'actualizar
					Dim auditoria As KaPlanLib.Registro.R_AUDITORIAS = (From audit In BBDD.R_AUDITORIAS _
					Where audit.ID_AUDITORIA = oAudit.ID_AUDITORIA _
					Select audit).FirstOrDefault

					If (auditoria IsNot Nothing) Then
						auditoria.COD_ARTICULO = oAudit.COD_ARTICULO
						auditoria.COD_PROVEEDOR = oAudit.COD_PROVEEDOR
						auditoria.LANTEGI = oAudit.LANTEGI
						auditoria.VERIFICADOR = oAudit.VERIFICADOR
						auditoria.TIPO = oAudit.TIPO
						auditoria.TIPO_AUDITORIA = oAudit.TIPO_AUDITORIA
						auditoria.FECHA = oAudit.FECHA
						auditoria.FECHA_PREVISTA = oAudit.FECHA_PREVISTA
						auditoria.HORA = oAudit.HORA
						auditoria.IE = oAudit.IE
						auditoria.N_AUDITORIA = oAudit.N_AUDITORIA
						auditoria.OBSERVACIONES = oAudit.OBSERVACIONES
						auditoria.REALIZADA = oAudit.REALIZADA
						BBDD.SubmitChanges()

						idAuditoria = oAudit.ID_AUDITORIA
					End If
				End If

				Return idAuditoria
			Catch ex As Exception
				Return Integer.MinValue
			End Try
		End Function

		''' <summary>
		''' Elimina una la auditoria
		''' </summary>
		''' <param name="idRegistro">Identificador de la auditoria</param>        
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function EliminarAuditoria(ByVal idRegistro As Integer) As Boolean
			Try
				Dim BBDD As New KaPlanLib.DAL.ELL

				Dim Auditoria = From audit In BBDD.R_AUDITORIAS _
				 Where audit.ID_AUDITORIA = idRegistro _
				 Where audit.ID_AUDITORIA = idRegistro _
				 Select audit

				If (Auditoria.Count = 1) Then
					BBDD.R_AUDITORIAS.DeleteOnSubmit(Auditoria.First)
					BBDD.SubmitChanges()
					Return True
				Else
					Return False
				End If
			Catch ex As Exception
				Return False
			End Try
		End Function

#End Region

#Region "R_VALORES_AUDITORIAS"

        ''' <summary>
        ''' Consulta el valor de una auditoria
        ''' </summary>
        ''' <returns></returns>
        Function consultarValorAuditoria(ByVal idRegistro As Integer) As Registro.R_VALORES_AUDITORIA
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim RegValor = From valor In BBDD.R_VALORES_AUDITORIA _
                  Where valor.ID_VALOR = idRegistro _
                  Select valor

                If (RegValor.Any = False) Then
                    Return Nothing
                Else
                    Return RegValor.First
                End If
            Catch ex As Exception
                Throw New Exception("error", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda el valor de una auditoria
        ''' </summary>
        ''' <param name="oValor">Auditoria</param>		
        ''' <returns>Booleano</returns>
        ''' <remarks></remarks>
        Public Function GuardarValorAuditoria(ByVal oValor As KaPlanLib.Registro.R_VALORES_AUDITORIA) As Boolean
            Dim BBDD As New KaPlanLib.DAL.ELL
            Try
                If (oValor.ID_VALOR = 0) Then 'insertar
                    BBDD.R_VALORES_AUDITORIA.InsertOnSubmit(oValor)

                Else 'actualizar
                    Dim valor As KaPlanLib.Registro.R_VALORES_AUDITORIA = (From val In BBDD.R_VALORES_AUDITORIA _
                    Where val.ID_VALOR = oValor.ID_VALOR _
                    Select val).FirstOrDefault

                    If (valor IsNot Nothing) Then
                        valor.ID_CARAC = oValor.ID_CARAC
                        valor.TIPO_VALOR = oValor.TIPO_VALOR
                        valor.VALOR_A = oValor.VALOR_A
                        valor.VALOR_V = oValor.VALOR_V
                        BBDD.SubmitChanges()
                    Else
                        Return False
                    End If
                End If
                BBDD.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Elimina un valor de una auditoria
        ''' </summary>
        ''' <param name="idRegistro">Identificador del valor</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarValorAuditoria(ByVal idRegistro As Integer) As Boolean
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim Valor = From val In BBDD.R_VALORES_AUDITORIA _
                 Where val.ID_VALOR = idRegistro _
                 Select val

                If (Valor.Count = 1) Then
                    BBDD.R_VALORES_AUDITORIA.DeleteOnSubmit(Valor.First)
                    BBDD.SubmitChanges()
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

#Region "Recuperar valores"

        ''' <summary>
        ''' Recupera los valores de una auditoria, codArticulo y codProveedor
        ''' </summary>
        ''' <returns>0:sin recuperaciones,>0:con recuperacion,Integer.MinValue:error</returns>
        ''' <remarks></remarks>
        Public Function RecuperarValores(ByVal idAuditoria As Integer, ByVal codArticulo As String, ByVal codProveedor As String) As Integer
            Dim transaction As DbTransaction = Nothing
            Dim BBDD As New KaPlanLib.DAL.ELL
            Try
                Dim regInsert As Integer = 0
                Dim idRecepcion, index As Integer
                Dim ordenC As Double?

                BBDD.Connection.Open()
                transaction = BBDD.Connection.BeginTransaction
                BBDD.Transaction = transaction

                For i As Integer = 1 To 2
                    index = i
                    Dim recepciones As Integer = (From recep In BBDD.R_RECEPCIONES _
                     Where recep.COD_ARTICULO = codArticulo And _
                     recep.COD_PROVEEDOR = If(index = 1, codProveedor, recep.COD_PROVEEDOR) _
                     Order By recep.FECHA_REGISTRO Descending, recep.HORA_REGISTRO Descending _
                     Select recep.ID_RECEPCION).FirstOrDefault

                    If (recepciones > 0) Then
                        idRecepcion = recepciones
                        Dim caracAud As IQueryable(Of Registro.R_CARACTERISTICAS_AUDITORIA) = From caracteAuditoria In BBDD.R_CARACTERISTICAS_AUDITORIA _
                         Where caracteAuditoria.ID_AUDITORIA = idAuditoria _
                         Select caracteAuditoria

                        If (index = 1) Then
                            caracAud = caracAud.Where(Function(o As Registro.R_CARACTERISTICAS_AUDITORIA) o.VER_REG_REC <> 0)
                        Else
                            caracAud = caracAud.Where(Function(o As Registro.R_CARACTERISTICAS_AUDITORIA) o.VER_REG_PRO <> 0)
                        End If

                        For Each item In caracAud
                            ordenC = item.ORDEN_CARAC
                            Dim valoresInsertar = From carac In BBDD.R_CARACTERISTICAS _
                              Join valores In BBDD.R_VALORES On carac.ID_CARAC Equals valores.ID_CARAC _
                              Where carac.ID_RECEPCION = idRecepcion And _
                              carac.ORDEN_CARAC = ordenC _
                              Order By valores.ID_VALOR _
                              Select valores.VALOR_V, valores.VALOR_A, valores.TIPO_VALOR Take 5

                            If (valoresInsertar.Any) Then
                                For Each itemInsertar In valoresInsertar
                                    Dim newItem As New Registro.R_VALORES_AUDITORIA
                                    newItem.ID_CARAC = item.ID_CARAC
                                    newItem.VALOR_V = itemInsertar.VALOR_V
                                    newItem.VALOR_A = itemInsertar.VALOR_A
                                    newItem.TIPO_VALOR = itemInsertar.TIPO_VALOR
                                    BBDD.R_VALORES_AUDITORIA.InsertOnSubmit(newItem)
                                    regInsert += 1
                                Next
                                BBDD.SubmitChanges()
                            End If
                        Next
                    Else
                        transaction.Commit()
                        Return 0
                    End If
                Next

                transaction.Commit()
                Return regInsert
            Catch ex As Exception
                log.Error(ex)
                transaction.Rollback()
                Return Integer.MinValue
            Finally
                BBDD.Connection.Close()
            End Try
        End Function

#End Region

#Region "Comprobar caracteristicas Auditoria"

        ''' <summary>
        ''' Comprueba si existe alguna caracteristica para la auditoria. Sino, se insertan las del plan
        ''' </summary>
        Public Sub ComprobarCaracteristicasAuditoria(ByVal IdAuditoria As Integer)
            Dim BBDD As New KaPlanLib.DAL.ELL
            Dim transaction As DbTransaction = Nothing
            Dim art As Integer = 0
            Dim oAuditoria As Registro.R_AUDITORIAS
            Dim tipoAuditoria As String = String.Empty
            Try
                oAuditoria = consultarAuditoria(IdAuditoria)
                If (oAuditoria.TIPO_AUDITORIA <> String.Empty) Then tipoAuditoria = oAuditoria.TIPO_AUDITORIA.Substring(0, 1).ToUpper
                If (oAuditoria.COD_ARTICULO.Trim <> String.Empty) Then art = CInt(oAuditoria.COD_ARTICULO.Trim)
                Dim caracteristicas = From caractAudit In BBDD.R_CARACTERISTICAS_AUDITORIA _
                   Where caractAudit.ID_AUDITORIA = IdAuditoria _
                   Select caractAudit

                If (caracteristicas.Any = False) Then
                    Dim valoresInsertar = From carac In BBDD.CARACTERISTICAS_DEL_PLAN _
                      Where carac.CODIGO = art And carac.PROCEDE_DE <> "HOJA REGISTRO" _
                      Select carac

                    If (tipoAuditoria = "D") Then
                        valoresInsertar.Where(Function(o As Registro.CARACTERISTICAS_DEL_PLAN) o.VER_REG_DIM)
                    ElseIf (tipoAuditoria = "M") Then
                        valoresInsertar.Where(Function(o As Registro.CARACTERISTICAS_DEL_PLAN) o.VER_REG_MAT <> 0)
                    Else
                        valoresInsertar.Where(Function(o As Registro.CARACTERISTICAS_DEL_PLAN) o.VER_REG_FUN <> 0)
                    End If

                    If (valoresInsertar.Any) Then
                        Try
                            BBDD.Connection.Open()
                            transaction = BBDD.Connection.BeginTransaction
                            BBDD.Transaction = transaction

                            For Each itemInsertar In valoresInsertar
                                Dim newItem As New Registro.R_CARACTERISTICAS_AUDITORIA
                                newItem.ID_AUDITORIA = IdAuditoria
                                newItem.ORDEN_CARAC = itemInsertar.ORDEN_CARAC
                                newItem.CARACTERISTICA = itemInsertar.CARAC_PARAM
                                newItem.ESPECIFICACION = itemInsertar.ESPECIFICACION
                                newItem.MINIM = itemInsertar.MINIM
                                newItem.MAXIM = itemInsertar.MAXIM
                                newItem.TECNICA_EVALUACION = itemInsertar.MEDIO_DENOMINACION
                                newItem.TAMAÑO = itemInsertar.TAMAÑO
                                newItem.FRECUENCIA = itemInsertar.FRECUENCIA_CONTROL
                                newItem.METODO_CONTROL = itemInsertar.METODO_CONTROL
                                newItem.CLASE = itemInsertar.CLASE
                                newItem.DECISION = 1
                                newItem.VER_REG_REC = itemInsertar.VER_REG_REC
                                newItem.VER_REG_PRO = itemInsertar.VER_REG_PRO
                                BBDD.R_CARACTERISTICAS_AUDITORIA.InsertOnSubmit(newItem)
                            Next
                            BBDD.SubmitChanges()
                            transaction.Commit()
                        Catch ex As Exception
                            log.Error(ex)
                            transaction.Rollback()
							Throw New Exception("Error al comprobar si tiene caracteristicas", ex)
                        End Try
                    End If
                End If
            Catch ex As Exception
                log.Error(ex)
				Throw New Exception("Error al comprobar si tiene caracteristicas", ex)
            End Try
        End Sub

#End Region

#Region "R_AUDITORIAS_PRODUCTOS"

        ''' <summary>
        ''' Consulta el valor de una auditoria producto
        ''' </summary>
        ''' <returns></returns>
        Function consultarAuditoriaProducto(ByVal idRegistro As Integer) As Registro.R_AUDITORIAS_PRODUCTO
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim RegAudit = From auditProd In BBDD.R_AUDITORIAS_PRODUCTO _
                  Where auditProd.ID_AUDITORIA_PRODUCTO = idRegistro _
                  Select auditProd

                If (RegAudit.Any = False) Then
                    Return Nothing
                Else
                    Return RegAudit.First
                End If
            Catch ex As Exception
                Throw New Exception("error", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda el valor de una auditoria producto
        ''' </summary>
        ''' <param name="oAuditProd">Auditoria</param>		
        ''' <returns>Booleano</returns>
        ''' <remarks></remarks>
        Public Function GuardarAuditoriaProducto(ByVal oAuditProd As KaPlanLib.Registro.R_AUDITORIAS_PRODUCTO) As Boolean
            Dim BBDD As New KaPlanLib.DAL.ELL
            Try
                If (oAuditProd.ID_AUDITORIA_PRODUCTO = 0) Then 'insertar
                    BBDD.R_AUDITORIAS_PRODUCTO.InsertOnSubmit(oAuditProd)
                Else 'actualizar
                    Dim auditProd As KaPlanLib.Registro.R_AUDITORIAS_PRODUCTO = (From auditPr In BBDD.R_AUDITORIAS_PRODUCTO _
                     Where auditPr.ID_AUDITORIA_PRODUCTO = oAuditProd.ID_AUDITORIA_PRODUCTO _
                     Select auditPr).FirstOrDefault

                    If (auditProd IsNot Nothing) Then
                        auditProd.AUDITAR = oAuditProd.AUDITAR
                        auditProd.COD_ARTICULO = oAuditProd.COD_ARTICULO
                        auditProd.COD_PROVEEDOR = oAuditProd.COD_PROVEEDOR
                        auditProd.FECHA_ENTRADA = oAuditProd.FECHA_ENTRADA
                        auditProd.LANTEGI = oAuditProd.LANTEGI
                        auditProd.LANTEGI_RH = oAuditProd.LANTEGI_RH
                        auditProd.N_AUDITORIA = oAuditProd.N_AUDITORIA
                        auditProd.PERIODICIDAD = oAuditProd.PERIODICIDAD
                        auditProd.TIPO = oAuditProd.TIPO
                        auditProd.TIPO_AB = oAuditProd.TIPO_AB
                        BBDD.SubmitChanges()
                    Else
                        Return False
                    End If
                End If
                BBDD.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Elimina una auditoria producto
        ''' </summary>
        ''' <param name="idRegistro">Identificador del registro</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarAuditoriaProducto(ByVal idRegistro As Integer) As Boolean
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim AuditProduct = From auditProd In BBDD.R_AUDITORIAS_PRODUCTO _
                  Where auditProd.ID_AUDITORIA_PRODUCTO = idRegistro _
                  Select auditProd

                If (AuditProduct.Count = 1) Then
                    BBDD.R_AUDITORIAS_PRODUCTO.DeleteOnSubmit(AuditProduct.First)
                    BBDD.SubmitChanges()
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

#Region "R_REALIZADOS_AUDITORIA_PRODUCTO"

        ''' <summary>
        ''' Consulta el valor de un realizado de una auditoria producto
        ''' </summary>
        ''' <returns></returns>
        Function consultarRealizadoAuditoriaProducto(ByVal idRegistro As Integer) As Registro.R_REALIZADOS_AUDITORIA_PRODUCTO
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim RegRealiz = From realizado In BBDD.R_REALIZADOS_AUDITORIA_PRODUCTO _
                  Where realizado.ID_REALIZADO = idRegistro _
                  Select realizado

                If (RegRealiz.Any = False) Then
                    Return Nothing
                Else
                    Return RegRealiz.First
                End If
            Catch ex As Exception
                Throw New Exception("error", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda el realizado de una auditoria producto
        ''' </summary>
        ''' <param name="oRealizado">Realizado</param>		
        ''' <returns>Booleano</returns>
        ''' <remarks></remarks>
        Public Function GuardarRealizadoAuditoriaProducto(ByVal oRealizado As KaPlanLib.Registro.R_REALIZADOS_AUDITORIA_PRODUCTO) As Boolean
            Dim BBDD As New KaPlanLib.DAL.ELL
            Try
                If (oRealizado.ID_REALIZADO = 0) Then 'insertar
                    BBDD.R_REALIZADOS_AUDITORIA_PRODUCTO.InsertOnSubmit(oRealizado)
                Else 'actualizar
                    Dim realizado = (From realiz In BBDD.R_REALIZADOS_AUDITORIA_PRODUCTO _
                     Where realiz.ID_REALIZADO = oRealizado.ID_REALIZADO _
                     Select realiz).FirstOrDefault

                    If (realizado IsNot Nothing) Then
                        realizado.AUDITORIA = oRealizado.AUDITORIA
                        realizado.DIMENSIONAL = oRealizado.DIMENSIONAL
                        realizado.ENVIADO_PPAP = oRealizado.ENVIADO_PPAP
                        realizado.FECHA_PREVISTA = oRealizado.FECHA_PREVISTA
                        realizado.FECHA_REAL = oRealizado.FECHA_REAL
                        realizado.FUNCIONAL = oRealizado.FUNCIONAL
                        realizado.HOMOLOGACION = oRealizado.HOMOLOGACION
                        realizado.ID_AUDITORIA_PRODUCTO = oRealizado.ID_AUDITORIA_PRODUCTO
                        realizado.MATERIAL = oRealizado.MATERIAL
                        realizado.OBSERVACIONES = oRealizado.OBSERVACIONES
                        realizado.RECIBIDO_PPAP = oRealizado.RECIBIDO_PPAP
                        BBDD.SubmitChanges()
                    Else
                        Return False
                    End If
                End If
                BBDD.SubmitChanges()

                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

        ''' <summary>
        ''' Elimina un realizado auditoria producto
        ''' </summary>
        ''' <param name="idRegistro">Identificador del registro</param>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarRealizadoAuditoriaProducto(ByVal idRegistro As Integer) As Boolean
            Try
                Dim BBDD As New KaPlanLib.DAL.ELL

                Dim RegRealiz = From realizado In BBDD.R_REALIZADOS_AUDITORIA_PRODUCTO _
                  Where realizado.ID_REALIZADO = idRegistro _
                  Select realizado

                If (RegRealiz.Count = 1) Then
                    BBDD.R_REALIZADOS_AUDITORIA_PRODUCTO.DeleteOnSubmit(RegRealiz.First)
                    BBDD.SubmitChanges()
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

#Region "OPERACIONES_DE_UN_ARTICULO"
        ''' <summary>
        ''' Ordena los registro de forma consecutiva del primero al ultimo.
        ''' </summary>
        ''' <param name="CODIGO">Codigo de Articulo.</param>
        ''' <remarks></remarks>
        Sub ReorganizarOPERACIONES_DE_UN_ARTICULO(ByVal CODIGO As String, BBDD As KaPlanLib.DAL.ELL)
            Dim Orden As Integer = 0
            Dim lReg As List(Of Registro.OPERACIONES_DE_UN_ARTICULO) = _
             (From Reg As Registro.OPERACIONES_DE_UN_ARTICULO In BBDD.OPERACIONES_DE_UN_ARTICULO Where Reg.CODIGO = CODIGO Select Reg Order By Reg.NUM_OPERACION Ascending, Reg.ID_REGISTRO Descending).ToList

            If lReg IsNot Nothing AndAlso lReg.Any Then
                For Each Reg As Registro.OPERACIONES_DE_UN_ARTICULO In lReg
                    Orden += 1
                    Reg.NUM_OPERACION = Orden
                Next
                BBDD.SubmitChanges()
            End If
        End Sub

        ''' <summary>
        ''' Ordenacion de los registros
        ''' </summary>
        ''' <param name="OPERACIONES_DE_UN_ARTICULO">Tipo de elementos que se va a reordenar.</param>
        ''' <param name="OrdenOriginal">Posicion original que ocupaba el elemento.</param>
        ''' <remarks></remarks>
        Sub ReorganizarOPERACIONES_DE_UN_ARTICULO(ByVal OPERACIONES_DE_UN_ARTICULO As Registro.OPERACIONES_DE_UN_ARTICULO, ByRef OrdenOriginal As Integer, BBDD As KaPlanLib.DAL.ELL)
            Dim Orden As Integer

            If OrdenOriginal > OPERACIONES_DE_UN_ARTICULO.NUM_OPERACION Or OrdenOriginal = 0 Then   'Se produce al retrasar una posicion el orden o al crear uno nuevo.
                '--------------------------------------------------------------------------------------------------------------
                Dim lReg As List(Of Registro.OPERACIONES_DE_UN_ARTICULO) = _
                    (From Reg As Registro.OPERACIONES_DE_UN_ARTICULO In BBDD.OPERACIONES_DE_UN_ARTICULO _
                    Where Reg.ID_REGISTRO <> OPERACIONES_DE_UN_ARTICULO.ID_REGISTRO And Reg.CODIGO = OPERACIONES_DE_UN_ARTICULO.CODIGO And Reg.NUM_OPERACION >= OPERACIONES_DE_UN_ARTICULO.NUM_OPERACION _
                    Select Reg Order By Reg.NUM_OPERACION Ascending, Reg.ID_REGISTRO Descending).ToList
                If lReg IsNot Nothing AndAlso lReg.Any Then
                    Orden = OPERACIONES_DE_UN_ARTICULO.NUM_OPERACION 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
                    For Each Reg As Registro.OPERACIONES_DE_UN_ARTICULO In lReg
                        Orden += 1
                        Reg.NUM_OPERACION = Orden
                    Next
                    BBDD.SubmitChanges()
                End If
                '--------------------------------------------------------------------------------------------------------------
            ElseIf OrdenOriginal < OPERACIONES_DE_UN_ARTICULO.NUM_OPERACION Then 'Se produce al avanzar una posicion  el orden.
                '--------------------------------------------------------------------------------------------------------------
                Dim lReg As List(Of Registro.OPERACIONES_DE_UN_ARTICULO) = _
                    (From Reg As Registro.OPERACIONES_DE_UN_ARTICULO In BBDD.OPERACIONES_DE_UN_ARTICULO _
                    Where Reg.ID_REGISTRO <> OPERACIONES_DE_UN_ARTICULO.ID_REGISTRO And Reg.CODIGO = OPERACIONES_DE_UN_ARTICULO.CODIGO And Reg.NUM_OPERACION <= OPERACIONES_DE_UN_ARTICULO.NUM_OPERACION _
                    Select Reg Order By Reg.NUM_OPERACION Descending, Reg.ID_REGISTRO Ascending).ToList
                If lReg IsNot Nothing AndAlso lReg.Any Then
                    Orden = OPERACIONES_DE_UN_ARTICULO.NUM_OPERACION 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
                    For Each Reg As Registro.OPERACIONES_DE_UN_ARTICULO In lReg
                        Orden -= 1
                        Reg.NUM_OPERACION = Orden
                    Next
                    BBDD.SubmitChanges()
                End If
                '--------------------------------------------------------------------------------------------------------------
            End If
            ReorganizarOPERACIONES_DE_UN_ARTICULO(OPERACIONES_DE_UN_ARTICULO.CODIGO, BBDD)
        End Sub
#End Region
	End Class
End Namespace