Imports System.Collections.Generic

Namespace BLL
	Public Class EjecucionProcComponent

#Region "Consultas"

		''' <summary>
		''' Consulta los datos de una ejecucion
		''' </summary>
		''' <param name="id">Identificador de la ejecucion</param>
		''' <remarks></remarks>
		Public Function consultar(ByVal id As Integer) As ELL.EjecucionProc
			Dim ejecDAL As New DAL.EJECUCION_PROCESOS()
			ejecDAL.LoadByPrimaryKey(id)

			Return getObject(ejecDAL)
		End Function

		''' <summary>
		''' Consulta el listado de ejecuciones
		''' </summary>
		''' <param name="oEjec">Objeto donde se localizan los parametros</param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function consultarListado(ByVal oEjec As ELL.EjecucionProc) As List(Of ELL.EjecucionProc)
			Dim ejecDAL As New DAL.EJECUCION_PROCESOS()
			Dim listEjec As New List(Of ELL.EjecucionProc)
			If (oEjec.IdUsuario <> Integer.MinValue) Then ejecDAL.Where.ID_USUARIO.Value = oEjec.IdUsuario
			If (oEjec.Fecha <> DateTime.MinValue) Then ejecDAL.Where.FECHA.Value = oEjec.Fecha
			If (oEjec.Flag > 0) Then ejecDAL.Where.FLAG.Value = oEjec.Flag
			ejecDAL.Query.Load()
			If ejecDAL.RowCount > 0 Then
				Do
					listEjec.Add(getObject(ejecDAL))
				Loop While ejecDAL.MoveNext
			End If

			Return listEjec
		End Function


		''' <summary>
		''' A partir de un objeto mygeneration, devuelve un objeto EjecucionProc
		''' </summary>
		''' <param name="ejecDAL"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Private Function getObject(ByVal ejecDAL As DAL.EJECUCION_PROCESOS) As ELL.EjecucionProc
			Dim oEjecProc As New ELL.EjecucionProc
			oEjecProc.Id = ejecDAL.ID
			oEjecProc.IdUsuario = ejecDAL.ID_USUARIO
			oEjecProc.Fecha = ejecDAL.FECHA
			oEjecProc.Flag = ejecDAL.FLAG
			oEjecProc.Descripcion = ejecDAL.s_DESCRIPCION

			Return oEjecProc
		End Function

#End Region

#Region "Modificaciones"

		''' <summary>
		''' Inserta los datos de la ejecucion
		''' </summary>
		''' <param name="oEjec">Objeto ejecucion a guardar</param>        
		''' <returns>Booleano que indica si se ha guardado correctamente</returns>
		Public Function Insert(ByVal oEjec As ELL.EjecucionProc) As Boolean
			Dim ejecDAL As New DAL.EJECUCION_PROCESOS()
			Try
				ejecDAL.AddNew()

				If (ejecDAL.RowCount = 1) Then
					ejecDAL.ID = oEjec.Id
					ejecDAL.ID_USUARIO = oEjec.IdUsuario
					ejecDAL.FECHA = oEjec.Fecha
					ejecDAL.FLAG = oEjec.Flag
					ejecDAL.DESCRIPCION = oEjec.Descripcion

					ejecDAL.Save()
					Return True
				End If
				Return False
			Catch
				Return False
			End Try

		End Function

		''' <summary>
		''' Borra la ejecucion
		''' </summary>
		''' <param name="idEjec">Identificador de la ejecucion</param>
		''' <returns>Booleano indicando si se ha borrado correctamente</returns>
		Function Delete(ByVal idEjec As Integer) As Boolean
			Dim ejecDAL As New DAL.EJECUCION_PROCESOS()
			ejecDAL.LoadByPrimaryKey(idEjec)
			If ejecDAL.RowCount = 1 Then
				ejecDAL.MarkAsDeleted()
				ejecDAL.Save()
				Return True
			End If
			Return False
		End Function

#End Region	

	End Class
End Namespace
