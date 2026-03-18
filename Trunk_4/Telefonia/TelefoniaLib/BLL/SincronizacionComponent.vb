Imports System.Collections.Generic
Imports AccesoAutomaticoBD

Namespace BLL

    Public Class SincronizacionComponent

        ''' <summary>
        ''' Tipo de accion al sincronizar los usuarios
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum Accion As Integer
            Nuevo = 0
            DadoBaja = 1
            CambioNumero = 2
        End Enum

#Region "Fecha Alta/Baja"

        ''' <summary>
        ''' Obtiene la fecha de sincronizacion de altas/bajas de una planta
        ''' <param name="IdPlanta">Id de la planta a tratar</param>
        ''' </summary>
        ''' <returns>Fecha</returns>        
        Public Shared Function getFechaAltasBajas(ByVal IdPlanta As Integer) As Date
            Try
                Dim param As New DAL.SINCRONIZACION_SAB
                param.LoadByPrimaryKey(IdPlanta)                
                If (param.RowCount = 1) Then
                    Return param.F_ALTAS_BAJAS
                Else
                    Return Date.MinValue
                End If                
            Catch ex As Exception
                Throw ex
            End Try
        End Function

        ''' <summary>
        ''' Guarda la fecha de sincronizacion de altas/bajas de una planta para que a partir de ahora, solo se sincronice a partir de dicha hora
        ''' <param name="IdPlanta">Id de la planta a tratar</param>
        ''' <param name="fecha">Fecha a guardar</param>
        ''' </summary>
        ''' <returns>Booleano</returns>        
        Public Shared Function setFechaAltasBajas(ByVal IdPlanta As Integer, ByVal fecha As Date) As Boolean
            Try
                Dim param As New DAL.SINCRONIZACION_SAB
                param.LoadByPrimaryKey(IdPlanta)                
                If (param.RowCount = 1) Then 'update
                    param.F_ALTAS_BAJAS = fecha                    
                Else 'insert
                    param.AddNew()
                    param.ID_PLANTA = IdPlanta
                    param.F_ALTAS_BAJAS = fecha                    
                End If
                param.Save()
                Return True
            Catch ex As Exception
                Return False
            End Try
        End Function

#End Region

#Region "Sincronizar"

        ''' <summary>
        ''' Obtiene la lista de usuarios resultante de la sincronizacion de altas y bajas
        ''' </summary>
        ''' <param name="IdPlanta">Id de la planta a tratar</param>
        ''' <param name="idRecursoTlfno">Id del recurso de telefonia</param>
        ''' <param name="bMostrarBajasSinExt">Indica si se deben mostrar las bajas sin extension ni nº de telefono</param>
        ''' <returns>Lista de string()-0:idUserActual,1:idUserNew,2:Accion</returns>        
        Public Function SincronizarAltasBajas(ByVal IdPlanta As Integer, ByVal idRecursoTlfno As Integer, ByVal bMostrarBajasSinExt As Boolean) As List(Of String())
            Try
                Dim lUsers As New List(Of String())
                Dim idUser As Integer
                Dim oUserBaja As SABLib.ELL.Usuario
                Dim lVersionesUsuario As List(Of SABLib.ELL.Usuario)
                Dim userComp As New SABLib.BLL.UsuariosComponent
                Dim extComp As New BLL.ExtensionComponent
                Dim fecha As Date = getFechaAltasBajas(IdPlanta)
                Dim lPlantas As List(Of SABLib.ELL.Planta) = Nothing

                '1º Se obtienen los usuarios con acceso al recurso de sab
                Dim lUserSabRecurso As List(Of SABLib.ELL.Usuario) = userComp.GetUsuariosConRecurso(idRecursoTlfno)

                '2º Se obtienen los usuarios que tiene alguna extension o telefono asignado
                Dim lUserExt As List(Of ELL.TelefonoExtension) = extComp.VerTodos(IdPlanta, True, False)

                '3º Se recorre la lista de usuarios de sab, mirando las altas, bajas, y cambios
                If (lUserSabRecurso IsNot Nothing) Then
                    For Each oUser As SABLib.ELL.Usuario In lUserSabRecurso
                        If (oUser.CodPersona = Integer.MinValue) Then Continue For 'Sino tiene codigo de persona, continuara con la siguiente vuelta

                        'Si no tiene la planta actual, se continuara con el siguiente usuario
                        lPlantas = userComp.GetPlantas(oUser.Id)
                        If (lPlantas Is Nothing) Then
                            Continue For
                        Else
                            If (lPlantas.Find(Function(o1 As SABLib.ELL.Planta) o1.Id = IdPlanta) Is Nothing) Then Continue For                            
                        End If

                        idUser = oUser.Id
                        'Si no tiene fecha de revision o la fecha de alta es mayor que la de la ultima revision y no tiene una extension asociada
                        If (fecha = Date.MinValue Or oUser.FechaAlta >= fecha) Then
                            If (lUserExt.Find(Function(o1 As ELL.TelefonoExtension) o1.idSab = idUser) Is Nothing) Then
                                'Puede ser una alta o un cambio de trabajador
                                'Hay que buscar sus anterior registro en sab dado de baja y ver si esta en la lista de extensiones
                                oUserBaja = New SABLib.ELL.Usuario
                                Dim busquedaPor As String = String.Empty
                                If (oUser.NombreUsuario <> String.Empty) Then
                                    oUserBaja.NombreUsuario = oUser.NombreUsuario
                                    busquedaPor = "S"
                                ElseIf (oUser.Email <> String.Empty) Then
                                    oUserBaja.Email = oUser.Email
                                    busquedaPor = "S"
                                Else
                                    If (oUser.Nombre <> String.Empty) Then
                                        oUserBaja.Nombre = oUser.Nombre
                                        busquedaPor = "S"
                                        If (oUser.Apellido1 <> String.Empty) Then
                                            oUserBaja.Apellido1 = oUser.Apellido1
                                            If (oUser.Apellido2 <> String.Empty) Then
                                                oUserBaja.Apellido2 = oUser.Apellido2
                                            End If
                                        End If
                                    End If

                                End If
                                If (busquedaPor = String.Empty) Then
                                    'Sino tiene ni nombre, ni usuario, ni email, no se le mostrara
                                    lVersionesUsuario = New List(Of SABLib.ELL.Usuario)
                                    'lVersionesUsuario.Add(oUser)
                                Else
                                    lVersionesUsuario = userComp.GetUsuarios(oUserBaja, False)
                                End If

                                lVersionesUsuario.Sort(Function(o1 As SABLib.ELL.Usuario, o2 As SABLib.ELL.Usuario) o1.FechaAlta > o2.FechaAlta) 'FechaAlta DESC

                                If (lVersionesUsuario IsNot Nothing And lVersionesUsuario.Count > 0) Then  'Por lo menos, tiene que aparecer el usuario actual
                                    If (lVersionesUsuario.Count = 1) Then  'solo tiene un registro
                                        If (oUser.DadoBaja And bMostrarBajasSinExt) Then  'Es una baja y no hay 
                                            Dim user As String() = {idUser, "", Accion.DadoBaja}
                                            lUsers.Add(user)
                                        ElseIf (Not oUser.DadoBaja) Then 'Es una alta
                                            Dim user As String() = {"", idUser, Accion.Nuevo}
                                            lUsers.Add(user)
                                        End If
                                    ElseIf (lVersionesUsuario.Count > 1) Then 'significa que tiene alguno en estado de baja
                                        oUserBaja = lVersionesUsuario.Item(1)  'nos quedamos con el registro dado de baja anterior
                                        If (oUserBaja Is Nothing) Then
                                            'Alta
                                            Dim user As String() = {"", idUser, Accion.Nuevo}
                                            lUsers.Add(user)
                                        ElseIf (lUserExt.Find(Function(o1 As ELL.TelefonoExtension) o1.idSab = oUserBaja.Id) IsNot Nothing) Then
                                            'Cambio de trabajador
                                            Dim user As String() = {oUserBaja.Id, idUser, Accion.CambioNumero}
                                            lUsers.Add(user)
                                        Else
                                            'Si antes tenia otro usuario y tampoco esta en la lista de usuario, lo tomamos como un alta
                                            'Alta
                                            Dim user As String() = {"", idUser, Accion.Nuevo}
                                            lUsers.Add(user)
                                        End If
                                    End If
                                End If
                            End If
                        ElseIf (fecha = Date.MinValue Or (oUser.FechaBaja >= fecha AndAlso oUser.FechaBaja <= Date.Now)) Then
                            'Aqui solo se trataran las bajas
                            oUserBaja = New SABLib.ELL.Usuario
                            Dim busquedaPor As String = String.Empty
                            If (oUser.NombreUsuario <> String.Empty) Then
                                oUserBaja.NombreUsuario = oUser.NombreUsuario
                                busquedaPor = "S"
                            ElseIf (oUser.Email <> String.Empty) Then
                                oUserBaja.Email = oUser.Email
                                busquedaPor = "S"
                            Else
                                If (oUser.Nombre <> String.Empty) Then
                                    oUserBaja.Nombre = oUser.Nombre
                                    busquedaPor = "S"
                                    If (oUser.Apellido1 <> String.Empty) Then
                                        oUserBaja.Apellido1 = oUser.Apellido1
                                        If (oUser.Apellido2 <> String.Empty) Then
                                            oUserBaja.Apellido2 = oUser.Apellido2
                                        End If
                                    End If
                                End If
                            End If

                            If (busquedaPor = String.Empty) Then
                                'Sino tiene ni nombre, ni usuario, ni email, no se le mostrara
                                lVersionesUsuario = New List(Of SABLib.ELL.Usuario)
                                'lVersionesUsuario.Add(oUser)
                            Else
                                lVersionesUsuario = userComp.GetUsuarios(oUserBaja, False, "FECHAALTA")
                            End If

                            Dim bDarBaja As Boolean = True
                            For Each oUser2 As SABLib.ELL.Usuario In lVersionesUsuario
                                If (Not oUser2.DadoBaja And oUser2.FechaAlta >= oUser.FechaBaja) Then
                                    bDarBaja = False
                                End If
                            Next

                            Dim bConExtension As Boolean = lUserExt.Find(Function(o1 As ELL.TelefonoExtension) o1.idSab = idUser) IsNot Nothing
                            If ((bDarBaja AndAlso bConExtension AndAlso bMostrarBajasSinExt) OrElse (bDarBaja AndAlso Not bConExtension AndAlso Not bMostrarBajasSinExt)) Then bDarBaja = False

                            If (bDarBaja) Then
                                Dim user As String() = {idUser, "", Accion.DadoBaja}
                                lUsers.Add(user)
                            End If
                        End If
                    Next
                End If

                Return lUsers
            Catch ex As Exception
                Throw New BatzException("errSincronizar", ex)
            End Try
        End Function

#End Region

#Region "Cambiar IdSab"

        ''' <summary>
        ''' Para el idSabActual indicado, se le reemplaza por el idSabNuevo ya que se le ha dado de alta con otro idSab
        ''' </summary>
        ''' <param name="idSabActual">Id del usuario de sab actual</param>
        ''' <param name="idSabNuevo">Id del usuario de sab nuevo con el que hay que reemplazar</param>
        ''' <returns>Booleano</returns>        
        Public Function actualizarIdSab(ByVal idSabActual As Integer, ByVal idSabNuevo As Integer) As Boolean
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                tx.BeginTransaction()

                'Se realizaran cambios en EXTENSION_PERSONAS y en TELEFONO_PERSONAS                
                Dim lExtensiones As New List(Of ELL.ExtensionUsuDep)
                Dim extPersoDAL As New DAL.EXTENSION_PERSONAS
                extPersoDAL.Where.ID_USUARIO.Value = idSabActual
                extPersoDAL.Query.Load()

                If (extPersoDAL.RowCount > 0) Then
                    Dim oExtPersoNew As ELL.ExtensionUsuDep
                    Do
                        'Guardamos los datos en un objeto. Esto hay que hacerlo porque mygeneration no permite hacer un update sobre un campo clave 
                        oExtPersoNew = New ELL.ExtensionUsuDep
                        oExtPersoNew.FechaDesde = extPersoDAL.F_DESDE
                        If Not (extPersoDAL.IsColumnNull(DAL.EXTENSION_PERSONAS.ColumnNames.F_HASTA)) Then oExtPersoNew.FechaHasta = extPersoDAL.F_HASTA
                        oExtPersoNew.IdExtension = extPersoDAL.ID_EXTENSION
                        oExtPersoNew.IdTelefono = extPersoDAL.ID_TELEFONO
                        oExtPersoNew.IdUsuario = idSabActual
                        lExtensiones.Add(oExtPersoNew)
                    Loop While extPersoDAL.MoveNext
                End If
                extPersoDAL.FlushData()

                Dim extPersoDAL2 As New DAL.EXTENSION_PERSONAS
                For Each oExtUsuDep As ELL.ExtensionUsuDep In lExtensiones                    
                    extPersoDAL2.LoadByPrimaryKey(oExtUsuDep.FechaDesde, oExtUsuDep.IdExtension, oExtUsuDep.IdTelefono, oExtUsuDep.IdUsuario)
                    If (extPersoDAL2.RowCount = 1) Then
                        'Se borra
                        extPersoDAL2.MarkAsDeleted()
                        extPersoDAL2.Save()
                        extPersoDAL2.FlushData()

                        'Se inserta de nuevo
                        extPersoDAL2.AddNew()
                        extPersoDAL2.ID_EXTENSION = oExtUsuDep.IdExtension
                        extPersoDAL2.ID_TELEFONO = oExtUsuDep.IdTelefono
                        extPersoDAL2.ID_USUARIO = idSabNuevo
                        extPersoDAL2.F_DESDE = oExtUsuDep.FechaDesde
                        If (oExtUsuDep.FechaHasta <> DateTime.MinValue) Then extPersoDAL2.F_HASTA = oExtUsuDep.FechaHasta

                        extPersoDAL2.Save()
                    End If
                Next
               
                Dim tlfnoPersoDAL As New DAL.TELEFONO_PERSONAS
                tlfnoPersoDAL.Where.ID_USUARIO.Value = idSabActual
                tlfnoPersoDAL.Query.Load()
                If (tlfnoPersoDAL.RowCount > 0) Then
                    Do
                        tlfnoPersoDAL.ID_USUARIO = idSabNuevo
                        tlfnoPersoDAL.Save()
                    Loop While tlfnoPersoDAL.MoveNext
                End If

                tx.CommitTransaction()
                Return True
            Catch ex As Exception
                tx.RollbackTransaction()
                Return False
            End Try
        End Function

#End Region

    End Class

End Namespace
