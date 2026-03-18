Imports AccesoAutomaticoBD
Imports TelefoniaLib

Namespace BLL

    Public Class ExtensionComponent

#Region "Enum"

        Public Enum Asignacion As Integer
            asignar = 0       'se asigna
            desasignar = 1    'se quita
            reemplazar = 2    'se le quita y se le añade otro al especificado
            reasignar = 3     'se le quita y se informa la lista para que se sepan cuales se han modificado
            asignarNoExtInt = 4    'para los casos en los que deja de ser extInt
        End Enum


        Public Enum AsignacionDev As Integer
            asignacion = 0
            devolucion = 1
        End Enum

#End Region

#Region "Consultas"


        ''' <summary>
        ''' Obtiene la informacion de una extension
        ''' </summary>
        ''' <param name="oExt"></param>
        ''' <param name="bActivo">Indica si obtendra las relaciones (usuarios,depart,otros) que esten activos o no</param>
        ''' <param name="bSiExtMovilGetExtInterna">Indicara que si la extension es movil y tiene una interna, se obtendran los datos de la interna</param>
        ''' <returns></returns>
        Public Function getExtension(ByVal oExt As ELL.Extension, ByVal idPlanta As Integer, Optional ByVal bActivo As Boolean = True, Optional ByVal bCargarListas As Boolean = True, Optional ByVal bSiExtMovilGetExtInterna As Boolean = False) As ELL.Extension
            Dim extDAL As New DAL.EXTENSION
            Dim oExtension As ELL.Extension = Nothing
            Try
                If (oExt.Id <> Integer.MinValue) Then extDAL.Where.ID.Value = oExt.Id
                If (oExt.Extension <> Integer.MinValue) Then extDAL.Where.EXTENSION.Value = oExt.Extension
                If (oExt.Nombre <> String.Empty) Then extDAL.Where.NOMBRE.Value = oExt.Nombre
                If (oExt.IdTelefono <> Integer.MinValue) Then extDAL.Where.ID_TELEFONO.Value = oExt.IdTelefono
                If (oExt.IdAlveolo <> Integer.MinValue) Then extDAL.Where.ID_ALVEOLO.Value = oExt.IdAlveolo
                '11/11/10: SE HA AÑADIDO LA ID_PLANTA PARA QUE CUANDO CONSULTES POR UNA PLANTA, TE DEVUELVA LA EXTENSION DE ESA PLANTA
                If (idPlanta <> Integer.MinValue) Then extDAL.Where.ID_PLANTA.Value = idPlanta
                extDAL.Query.Load()

                If (extDAL.RowCount = 1) Then
                    oExtension = cargarExtension(extDAL, bCargarListas, idPlanta, bActivo, bSiExtMovilGetExtInterna)
                End If
                Return oExtension
            Catch ex As Exception
                Throw New BatzException("errObtenerInfo", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga el objeto extension
        ''' </summary>
        ''' <param name="ExtDAL"></param>
        ''' <param name="bCargarListas"></param>
        ''' <param name="bActivos">Indica si recuperara todos las relaciones o solo las activas</param>
        ''' <param name="bSiExtMovilGetExtInterna">Indicara que si la extension es movil y tiene una interna, se obtendran los datos de la interna</param>
        ''' <returns></returns>
        Private Function cargarExtension(ByVal extDAL As DAL.EXTENSION, ByVal bCargarListas As Boolean, ByVal idPlanta As Integer, Optional ByVal bActivos As Boolean = True, Optional ByVal bSiExtMovilGetExtInterna As Boolean = False)
            Dim oExtension As New ELL.Extension()
            oExtension.Id = extDAL.ID
            oExtension.Extension = extDAL.EXTENSION
            oExtension.Nombre = extDAL.NOMBRE
            oExtension.Visible = extDAL.VISIBLE
            oExtension.Prestamo = extDAL.PRESTAMO
            If (idPlanta > 0) Then oExtension.IdPlanta = extDAL.ID_PLANTA
            If (Not extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TIPOASIG)) Then oExtension.IdTipoAsignacion = extDAL.ID_TIPOASIG
            If (Not extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_ALVEOLO)) Then oExtension.IdAlveolo = extDAL.ID_ALVEOLO
            If (Not extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TELEFONO)) Then oExtension.IdTelefono = extDAL.ID_TELEFONO
            If (Not extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TIPOLINEA)) Then oExtension.IdTipoLinea = extDAL.ID_TIPOLINEA
            If (Not extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TIPOEXT)) Then oExtension.IdTipoExtension = extDAL.ID_TIPOEXT
            If (Not extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_DEPART_FAC)) Then oExtension.IdDepartamentoFac = extDAL.ID_DEPART_FAC
            If (Not extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_EXT_INTERNA)) Then oExtension.IdExtensionInterna = extDAL.ID_EXT_INTERNA
            oExtension.Obsoleto = extDAL.OBSOLETO

            If (bCargarListas) Then
                Dim idExtension As Integer = oExtension.Id
                If (bSiExtMovilGetExtInterna AndAlso oExtension.IdExtensionInterna <> Integer.MinValue) Then idExtension = oExtension.IdExtensionInterna
                oExtension.ListaPersonasAsig = PersonasAsignadas(idExtension, oExtension.IdTelefono, bActivos)
                oExtension.ListaDepartamentosAsig = DepartamentosAsignados(idExtension, oExtension.IdTelefono, idPlanta, bActivos)
                oExtension.ListaOtrosAsig = OtrosAsignados(idExtension, oExtension.IdTelefono, bActivos)
            End If

            Return oExtension
        End Function

        ''' <summary>
        ''' Lista las personas asignadas con una extension
        ''' </summary>
        ''' <param name="idExt">Identificador de una extension</param>
        ''' <param name="idTlfno">Telefono de la extension</param>
        ''' <param name="bActivos">Indica si recuperara todos las relaciones o solo las activas</param>
        ''' <returns>Lista de usuarios</returns>        
        Private Function PersonasAsignadas(ByVal idExt As Integer, ByVal idTlfno As Integer, ByVal bActivos As Boolean) As List(Of ELL.ExtensionUsuDep)
            Try
                Dim extPersoDAL As New DAL.EXTENSION_PERSONAS
                Dim lUser As New List(Of ELL.ExtensionUsuDep)
                Dim oUser As ELL.ExtensionUsuDep = Nothing
                Dim userComp As New SABLib.BLL.UsuariosComponent
                Dim ousu As SABLib.ELL.Usuario

                extPersoDAL.Where.ID_EXTENSION.Value = idExt
                If (bActivos) Then
                    extPersoDAL.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
                    extPersoDAL.Query.OpenParenthesis()
                    extPersoDAL.Where.F_HASTA.Value = DateTime.Now
                    extPersoDAL.Where.F_HASTA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
                    Dim wp As AccesoAutomaticoBD.WhereParameter = extPersoDAL.Where.TearOff.F_HASTA
                    wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                    wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                    extPersoDAL.Query.CloseParenthesis()
                End If
                extPersoDAL.Query.Load()

                If (extPersoDAL.RowCount > 0) Then

                    Do
                        oUser = New ELL.ExtensionUsuDep
                        oUser.IdUsuario = extPersoDAL.ID_USUARIO
                        oUser.IdExtension = idExt
                        'If (idTlfno <> Integer.MinValue) Then oUser.IdTelefono = idTlfno
                        oUser.IdTelefono = extPersoDAL.ID_TELEFONO
                        ousu = userComp.GetUsuario(New Sablib.ELL.Usuario With {.Id = oUser.IdUsuario}, False)
                        oUser.NombreUsuario = ousu.NombreCompleto
                        oUser.IdDepartamento = ousu.IdDepartamento
                        oUser.FechaDesde = extPersoDAL.F_DESDE
                        If (Not extPersoDAL.IsColumnNull(DAL.EXTENSION_PERSONAS.ColumnNames.F_HASTA)) Then oUser.FechaHasta = extPersoDAL.F_HASTA

                        lUser.Add(oUser)
                    Loop While extPersoDAL.MoveNext
                End If

                Return lUser
            Catch ex As Exception
                Throw New BatzException("errObtenerPersonasAsignadas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Lista los departamentos asignadas con una extension
        ''' </summary>
        ''' <param name="idExt">Identificador de una extension</param>
        ''' <param name="idTlfno">Telefono de la extension</param>
        ''' <param name="idPlanta">IdPlanta del departamento</param>
        ''' <param name="bActivos">Indica si recuperara todos las relaciones o solo las activas</param>
        ''' <returns>Lista de Departamentos</returns>        
        Private Function DepartamentosAsignados(ByVal idExt As Integer, ByVal idTlfno As Integer, ByVal idPlanta As Integer, ByVal bActivos As Boolean) As List(Of ELL.ExtensionUsuDep)
            Try
                Dim extDeptDAL As New DAL.EXTENSION_DEPARTAMENTOS
                Dim lDept As New List(Of ELL.ExtensionUsuDep)
                Dim oDept As ELL.ExtensionUsuDep = Nothing
                Dim deptComp As New BLL.DepartamentosComponent

                extDeptDAL.Where.ID_EXTENSION.Value = idExt
                If (bActivos) Then
                    extDeptDAL.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
                    extDeptDAL.Query.OpenParenthesis()
                    extDeptDAL.Where.F_HASTA.Value = DateTime.Now
                    extDeptDAL.Where.F_HASTA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
                    Dim wp As AccesoAutomaticoBD.WhereParameter = extDeptDAL.Where.TearOff.F_HASTA
                    wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                    wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                    extDeptDAL.Query.CloseParenthesis()
                End If
                extDeptDAL.Query.Load()

                If (extDeptDAL.RowCount > 0) Then

                    Do
                        oDept = New ELL.ExtensionUsuDep
                        oDept.IdDepartamento = extDeptDAL.ID_DEPARTAMENTO
                        oDept.IdExtension = idExt
                        'If (idTlfno <> Integer.MinValue) Then oDept.IdTelefono = idTlfno
                        oDept.IdTelefono = extDeptDAL.ID_TELEFONO
                        Dim myDept As ELL.Departamento = deptComp.getDepartamento(oDept.IdDepartamento, idPlanta)
                        Dim nombre As String = String.Empty
                        If (myDept IsNot Nothing) Then nombre = myDept.Nombre
                        oDept.NombreDepartamento = nombre
                        oDept.FechaDesde = extDeptDAL.F_DESDE
                        If (Not extDeptDAL.IsColumnNull(DAL.EXTENSION_DEPARTAMENTOS.ColumnNames.F_HASTA)) Then oDept.FechaHasta = extDeptDAL.F_HASTA
                        lDept.Add(oDept)
                    Loop While extDeptDAL.MoveNext
                End If

                Return lDept
            Catch ex As Exception
                Throw New BatzException("errObtenerDepartamentosAsignadas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Lista otros items asignadas con una extension
        ''' </summary>
        ''' <param name="idExt">Identificador de una extension</param>
        ''' <param name="idTlfno">Telefono de la extension</param>
        ''' <param name="bActivos">Indica si recuperara todos las relaciones o solo las activas</param>
        ''' <returns>Lista de Otros</returns>        
        Private Function OtrosAsignados(ByVal idExt As Integer, ByVal idTlfno As Integer, ByVal bActivos As Boolean) As List(Of ELL.ExtensionUsuDep)
            Try
                Dim extOtrosDAL As New DAL.EXTENSION_OTROS
                Dim lOtros As New List(Of ELL.ExtensionUsuDep)
                Dim oOtros As ELL.ExtensionUsuDep = Nothing
                Dim otrosComp As New BLL.OtrosComponent

                extOtrosDAL.Where.ID_EXTENSION.Value = idExt
                If (bActivos) Then
                    extOtrosDAL.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
                    extOtrosDAL.Query.OpenParenthesis()
                    extOtrosDAL.Where.F_HASTA.Value = DateTime.Now
                    extOtrosDAL.Where.F_HASTA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
                    Dim wp As AccesoAutomaticoBD.WhereParameter = extOtrosDAL.Where.TearOff.F_HASTA
                    wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                    wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                    extOtrosDAL.Query.CloseParenthesis()
                End If
                extOtrosDAL.Query.Load()

                If (extOtrosDAL.RowCount > 0) Then

                    Do
                        oOtros = New ELL.ExtensionUsuDep
                        oOtros.IdOtros = extOtrosDAL.ID_OTRO
                        oOtros.IdExtension = idExt
                        oOtros.IdTelefono = extOtrosDAL.ID_TELEFONO
                        'If (idTlfno <> Integer.MinValue) Then oOtros.IdTelefono = idTlfno
                        oOtros.NombreOtros = otrosComp.getOtro(oOtros.IdOtros).Nombre
                        oOtros.FechaDesde = extOtrosDAL.F_DESDE
                        If (Not extOtrosDAL.IsColumnNull(DAL.EXTENSION_OTROS.ColumnNames.F_HASTA)) Then oOtros.FechaHasta = extOtrosDAL.F_HASTA
                        lOtros.Add(oOtros)
                    Loop While extOtrosDAL.MoveNext
                End If

                Return lOtros
            Catch ex As Exception
                Throw New BatzException("errObtenerElementos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Se buscaran las extensiones que cumplan con los requisitos
        ''' </summary>
        ''' <param name="oExten">Informacion con los filtros</param>
        ''' <param name="bCargarListas">Indica si se deberan cargar las listas de asignaciones o no</param>
        ''' <returns></returns>
        Public Function getExtensiones(ByVal oExten As ELL.Extension, ByVal idPlanta As Integer, Optional ByVal bCargarListas As Boolean = True) As System.Collections.Generic.List(Of ELL.Extension)
            Dim i As Integer = 0
            Try
                Dim extDAL As New DAL.EXTENSION
                Dim lExt As New List(Of ELL.Extension)
                Dim oExt As ELL.Extension = Nothing

                If (oExten.IdPlanta <> Integer.MinValue) Then extDAL.Where.ID_PLANTA.Value = oExten.IdPlanta
                If (oExten.IdTipoExtension <> Integer.MinValue) Then extDAL.Where.ID_TIPOEXT.Value = oExten.IdTipoExtension
                If (oExten.IdAlveolo <> Integer.MinValue) Then extDAL.Where.ID_ALVEOLO.Value = oExten.IdAlveolo
                If (oExten.IdTipoLinea <> Integer.MinValue) Then extDAL.Where.ID_TIPOLINEA.Value = oExten.IdTipoLinea
                If (oExten.IdTelefono <> Integer.MinValue) Then extDAL.Where.ID_TELEFONO.Value = oExten.IdTelefono
                If (Not oExten.Obsoleto) Then extDAL.Where.OBSOLETO.Value = CInt(oExten.Obsoleto)

                extDAL.Query.Load()

                If (extDAL.RowCount > 0) Then
                    Do
                        oExt = cargarExtension(extDAL, bCargarListas, idPlanta)
                        i += 1
                        lExt.Add(oExt)
                    Loop While extDAL.MoveNext
                End If

                Return lExt
            Catch ex As Exception
                Throw New BatzException("errMostrarExtensiones", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todos las extensiones internas libres, y aquella que se pasa como parametro
        ''' </summary>
        ''' <param name="oExt">Extension interna a incluir en los resultados</param>
        ''' <returns></returns>        
        Public Function getExtensionesLibres(ByVal oExt As ELL.Extension) As System.Collections.Generic.List(Of ELL.Extension)
            Dim extDAL As New DAL.EXTENSION
            Dim lExtensiones As New List(Of ELL.Extension)
            Dim oExten As ELL.Extension = Nothing
            Dim gestComp As New BLL.TelefonoComponent.GestorTlfnoComponent
            Dim reader As IDataReader = Nothing
            Try
                reader = extDAL.geExtensionesInternasLibres(oExt)
                While reader.Read
                    oExten = New ELL.Extension()
                    oExten.Id = CInt(reader.Item(DAL.EXTENSION.ColumnNames.ID))
                    oExten.Extension = CInt(reader.Item(DAL.EXTENSION.ColumnNames.EXTENSION))
                    oExten.Nombre = reader.Item(DAL.EXTENSION.ColumnNames.NOMBRE)
                    oExten.Visible = CType(reader.Item(DAL.EXTENSION.ColumnNames.VISIBLE), Boolean)
                    oExten.IdPlanta = CInt(reader.Item(DAL.EXTENSION.ColumnNames.ID_PLANTA))
                    oExten.Prestamo = CType(reader.Item(DAL.EXTENSION.ColumnNames.PRESTAMO), Boolean)
                    If (Not reader.IsDBNull(3)) Then oExten.IdAlveolo = CInt(reader.Item(DAL.EXTENSION.ColumnNames.ID_ALVEOLO))
                    If (Not reader.IsDBNull(4)) Then oExten.IdTipoExtension = CInt(reader.Item(DAL.EXTENSION.ColumnNames.ID_TIPOEXT))
                    If (Not reader.IsDBNull(7)) Then oExten.IdTelefono = CInt(reader.Item(DAL.EXTENSION.ColumnNames.ID_TELEFONO))
                    If (Not reader.IsDBNull(8)) Then oExten.IdExtensionInterna = CInt(reader.Item(DAL.EXTENSION.ColumnNames.ID_EXT_INTERNA))
                    If (Not reader.IsDBNull(9)) Then oExten.IdTipoAsignacion = reader.Item(DAL.EXTENSION.ColumnNames.ID_TIPOASIG)
                    If (Not reader.IsDBNull(10)) Then oExten.IdDepartamentoFac = CInt(reader.Item(DAL.EXTENSION.ColumnNames.ID_DEPART_FAC))
                    If (Not reader.IsDBNull(11)) Then oExten.IdTipoLinea = CInt(reader.Item(DAL.EXTENSION.ColumnNames.ID_TIPOLINEA))
                    lExtensiones.Add(oExten)
                End While
                Return lExtensiones
            Catch ex As Exception
                Throw New BatzException("errObtenerInfo", ex)
            Finally
                If (reader IsNot Nothing) Then reader.Close()
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todos las extensiones internas, su tipo y si tiene alveolo o no
        ''' </summary>             
        ''' <param name="idCultura">Cultura</param>
        Public Function getExtensionesInternasTipo(ByVal idPlanta As Integer, ByVal idCultura As String) As DataTable
            Dim extDAL As New DAL.EXTENSION
            Return extDAL.getExtensionesInternasTipo(idPlanta, idCultura)
        End Function

#End Region

#Region "Guardar"

        ''' <summary>
        ''' Guarda o modifica la informacion del extension y modifica las asignaciones pertinentes
        ''' </summary>
        ''' <param name="oExtNew">Objeto extension</param>
        ''' <param name="oExtOld">Objeto nuevo</param>
        ''' <returns>Booleano indicando si se ha realizado correctamente</returns>     
        Public Function Save(ByVal oExtNew As ELL.Extension, ByVal oExtOld As ELL.Extension) As Boolean
            Dim extDAL As New DAL.EXTENSION
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                tx.BeginTransaction()
                If (oExtNew.Id = Integer.MinValue) Then
                    extDAL.AddNew()
                Else
                    extDAL.LoadByPrimaryKey(oExtNew.Id)
                End If
                If (extDAL.RowCount = 1) Then
                    extDAL.EXTENSION = oExtNew.Extension
                    extDAL.NOMBRE = oExtNew.Nombre
                    extDAL.VISIBLE = oExtNew.Visible
                    extDAL.ID_PLANTA = oExtNew.IdPlanta
                    extDAL.ID_TIPOEXT = oExtNew.IdTipoExtension
                    extDAL.PRESTAMO = oExtNew.Prestamo
                    If (oExtNew.IdTipoAsignacion <> Integer.MinValue) Then
                        extDAL.ID_TIPOASIG = oExtNew.IdTipoAsignacion
                    Else
                        extDAL.s_ID_TIPOASIG = String.Empty
                    End If
                    If (oExtNew.IdAlveolo <> Integer.MinValue) Then
                        extDAL.ID_ALVEOLO = oExtNew.IdAlveolo
                    Else
                        extDAL.s_ID_ALVEOLO = String.Empty
                    End If
                    If (oExtNew.IdTelefono <> Integer.MinValue) Then
                        extDAL.ID_TELEFONO = oExtNew.IdTelefono
                    Else
                        extDAL.s_ID_TELEFONO = String.Empty
                    End If
                    If (oExtNew.IdTipoLinea <> Integer.MinValue) Then
                        extDAL.ID_TIPOLINEA = oExtNew.IdTipoLinea
                    Else
                        extDAL.s_ID_TIPOLINEA = String.Empty
                    End If
                    If (oExtNew.IdDepartamentoFac <> String.Empty) Then
                        extDAL.ID_DEPART_FAC = oExtNew.IdDepartamentoFac
                    Else
                        extDAL.s_ID_DEPART_FAC = String.Empty
                    End If

                    If (oExtNew.IdExtensionInterna <> Integer.MinValue) Then
                        extDAL.ID_EXT_INTERNA = oExtNew.IdExtensionInterna
                    Else
                        extDAL.s_ID_EXT_INTERNA = String.Empty
                    End If
                    extDAL.OBSOLETO = CInt(oExtNew.Obsoleto)
                    extDAL.Save()
                    oExtNew.Id = extDAL.ID
                    'Si es una extension movil, se guardara las asignaciones a extensines in si la extension old no es nothing. Si es nothing, significara que no ha cambiado
                    If (oExtOld IsNot Nothing) Then
                        If (oExtNew.IdTipoExtension = ELL.Extension.TipoExt.movil And Not oExtNew.Obsoleto) Then
                            modificarAsignacion(oExtNew, Asignacion.reemplazar, True)
                        ElseIf (oExtNew.IdTipoExtension = ELL.Extension.TipoExt.movil And oExtNew.Obsoleto) Then
                            modificarAsignacion(oExtNew, Asignacion.desasignar, True)
                        End If
                    End If
                    'Si la extension a guardar no es nueva y Si la extension2 no es nothing, significara que ha habido algun cambio el cual tiene que realizar reasignaciones
                    If (oExtNew.Id <> Integer.MinValue And oExtOld IsNot Nothing) Then
                        CambiosExtension(oExtOld, oExtNew)
                    End If
                    tx.CommitTransaction()
                    Return True
                End If
                tx.RollbackTransaction()
                TransactionMgr.ThreadTransactionMgrReset()
                Return False
            Catch ex As Exception
                tx.RollbackTransaction()
                TransactionMgr.ThreadTransactionMgrReset()
                Throw New BatzException("errGuardar", ex)
            End Try
        End Function

#End Region

#Region "Asignaciones"

        ''' <summary>
        ''' Delega la llamada a su correspondiente asignacion
        ''' </summary>
        ''' <param name="oExt"></param>
        ''' <param name="asig"></param>
        Public Sub modificarAsignacion(ByVal oExt As ELL.Extension, ByVal asig As Asignacion, Optional ByVal transaccionAbierta As Boolean = False)
            If (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.personal) Then
                modificarAsignacionPersona(oExt, asig, transaccionAbierta)
            ElseIf (oExt.IdTipoAsignacion = ELL.Extension.AsociarA.departamental) Then
                modificarAsignacionDepartamento(oExt, asig, transaccionAbierta)
            Else
                modificarAsignacionOtros(oExt, asig, transaccionAbierta)
            End If
        End Sub

        ''' <summary>
        ''' Modifica las extensiones de la persona
        ''' </summary>
        ''' <param name="oExt">Extension</param>
        ''' <param name="asig">Indicara si habra que asignar,desasignar o reemplazar</param>
        Private Sub modificarAsignacionPersona(ByRef oExt As ELL.Extension, ByVal asig As Asignacion, Optional ByVal transaccionAbierta As Boolean = False)
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                Dim oUser As ELL.ExtensionUsuDep
                If (oExt.ListaPersonasAsig IsNot Nothing) Then
                    Dim extPersoDAL As New DAL.EXTENSION_PERSONAS
                    If (Not transaccionAbierta) Then tx.BeginTransaction()

                    If (asig = Asignacion.desasignar Or asig = Asignacion.reemplazar Or asig = Asignacion.reasignar) Then
                        For Each user As ELL.ExtensionUsuDep In oExt.ListaPersonasAsig
                            extPersoDAL.FlushData()
                            extPersoDAL.Where.ID_EXTENSION.Value = user.IdExtension
                            If (user.IdTelefono <> Integer.MinValue) Then extPersoDAL.Where.ID_TELEFONO.Value = user.IdTelefono
                            If (user.IdUsuario <> Integer.MinValue) Then extPersoDAL.Where.ID_USUARIO.Value = user.IdUsuario
                            extPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extPersoDAL.Query.Load()
                            If (extPersoDAL.RowCount > 0) Then
                                'Si es reasignar, se inicializa para informar todos los registros que se van a actualizar
                                If (asig = Asignacion.reasignar) Then
                                    oExt.ListaPersonasAsig = New List(Of ELL.ExtensionUsuDep)
                                End If

                                Do
                                    extPersoDAL.F_HASTA = Now.Date
                                    extPersoDAL.Save()
                                    If (asig = Asignacion.reasignar) Then
                                        oUser = New ELL.ExtensionUsuDep
                                        oUser.IdExtension = user.IdExtension
                                        oUser.IdTelefono = user.IdTelefono
                                        oUser.IdUsuario = extPersoDAL.ID_USUARIO
                                        oExt.ListaPersonasAsig.Add(oUser)
                                    End If
                                Loop While extPersoDAL.MoveNext
                            Else
                                'Si es reasignar y no se ha encontrado ningun resultado, se pone a nothing los listados 
                                If (asig = Asignacion.reasignar) Then
                                    oExt.ListaPersonasAsig = Nothing
                                End If
                            End If
                        Next
                    End If

                    If (asig = Asignacion.asignar Or asig = Asignacion.reemplazar And (oExt.ListaPersonasAsig IsNot Nothing AndAlso oExt.ListaPersonasAsig.Count > 0)) Then
                        For Each perso As ELL.ExtensionUsuDep In oExt.ListaPersonasAsig
                            extPersoDAL.FlushData()
                            extPersoDAL.Where.ID_EXTENSION.Value = perso.IdExtension
                            extPersoDAL.Where.ID_USUARIO.Value = perso.IdUsuario
                            extPersoDAL.Where.ID_TELEFONO.Value = perso.IdTelefono
                            extPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extPersoDAL.Query.Load()
                            If (extPersoDAL.RowCount = 0) Then   'si esta actualmente asignado
                                extPersoDAL.FlushData()
                                extPersoDAL.AddNew()
                                extPersoDAL.ID_USUARIO = perso.IdUsuario
                                If (perso.IdExtension <> Integer.MinValue) Then
                                    extPersoDAL.ID_EXTENSION = perso.IdExtension
                                Else
                                    extPersoDAL.ID_EXTENSION = oExt.Id
                                End If
                                extPersoDAL.ID_TELEFONO = perso.IdTelefono
                                extPersoDAL.F_DESDE = Date.Now
                                extPersoDAL.Save()
                            End If
                        Next
                    End If

                    'Para cuando antes era una ext interna y ahora no, se pasa asignacion libre, es decir, que puede indicar la fecha desde y fecha hasta
                    If (asig = Asignacion.asignarNoExtInt And (oExt.ListaPersonasAsig IsNot Nothing AndAlso oExt.ListaPersonasAsig.Count > 0)) Then
                        Dim oExtenInt As ELL.Extension
                        For Each perso As ELL.ExtensionUsuDep In oExt.ListaPersonasAsig
                            extPersoDAL.FlushData()
                            extPersoDAL.AddNew()
                            extPersoDAL.ID_USUARIO = perso.IdUsuario
                            extPersoDAL.ID_EXTENSION = perso.IdExtension
                            extPersoDAL.ID_TELEFONO = perso.IdTelefono
                            extPersoDAL.F_DESDE = perso.FechaDesde
                            extPersoDAL.F_HASTA = perso.FechaHasta
                            extPersoDAL.Save()

                            'Se busca la extension interna y se pone su fecha a null
                            oExtenInt = New ELL.Extension With {.Id = perso.IdExtension}
                            oExtenInt = getExtension(oExtenInt, Integer.MinValue)  'La idPlanta no es necesaria
                            If (oExtenInt IsNot Nothing AndAlso oExtenInt.IdExtensionInterna <> Integer.MinValue) Then
                                extPersoDAL.FlushData()
                                extPersoDAL.Where.ID_USUARIO.Value = perso.IdUsuario
                                extPersoDAL.Where.ID_EXTENSION.Value = oExtenInt.Id
                                extPersoDAL.Where.ID_TELEFONO.Value = perso.IdTelefono
                                extPersoDAL.Where.F_DESDE.Value = perso.FechaDesde
                                extPersoDAL.Where.F_HASTA.Value = String.Empty
                                extPersoDAL.Query.Load()
                                If (extPersoDAL.RowCount = 1) Then
                                    extPersoDAL.F_HASTA = Date.Now
                                    extPersoDAL.Save()
                                End If
                            End If
                        Next
                    End If

                    If (Not transaccionAbierta) Then tx.CommitTransaction()
                End If
            Catch ex As Exception
                If (Not transaccionAbierta) Then
                    tx.RollbackTransaction()
                    TransactionMgr.ThreadTransactionMgrReset()
                End If
                Throw New BatzException("errAsignacion", ex)
            End Try
        End Sub

        ''' <summary>
        ''' Modifica las extensiones del departamento
        ''' </summary>
        ''' <param name="oExt">Extension</param>
        ''' <param name="asig">Indicara si habra que asignar,desasignar o reemplazar</param>
        Private Sub modificarAsignacionDepartamento(ByVal oExt As ELL.Extension, ByVal asig As Asignacion, Optional ByVal transaccionAbierta As Boolean = False)
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                Dim oDept As ELL.ExtensionUsuDep
                If (oExt.ListaDepartamentosAsig IsNot Nothing) Then
                    Dim extDeptoDAL As New DAL.EXTENSION_DEPARTAMENTOS
                    If (Not transaccionAbierta) Then tx.BeginTransaction()

                    If (asig = Asignacion.desasignar Or asig = Asignacion.reemplazar Or asig = Asignacion.reasignar) Then
                        For Each dept As ELL.ExtensionUsuDep In oExt.ListaDepartamentosAsig
                            extDeptoDAL.FlushData()
                            extDeptoDAL.Where.ID_EXTENSION.Value = dept.IdExtension
                            If (dept.IdTelefono <> Integer.MinValue) Then extDeptoDAL.Where.ID_TELEFONO.Value = dept.IdTelefono
                            If (dept.IdDepartamento <> String.Empty) Then extDeptoDAL.Where.ID_DEPARTAMENTO.Value = dept.IdDepartamento
                            extDeptoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extDeptoDAL.Query.Load()
                            If (extDeptoDAL.RowCount > 0) Then
                                'Si es reasignar, se inicializa para informar todos los registros que se van a actualizar
                                If (asig = Asignacion.reasignar) Then
                                    oExt.ListaDepartamentosAsig = New List(Of ELL.ExtensionUsuDep)
                                End If

                                Do
                                    extDeptoDAL.F_HASTA = Now.Date
                                    extDeptoDAL.Save()
                                    If (asig = Asignacion.reasignar) Then
                                        oDept = New ELL.ExtensionUsuDep
                                        oDept.IdExtension = dept.IdExtension
                                        oDept.IdTelefono = dept.IdTelefono
                                        oDept.IdDepartamento = extDeptoDAL.ID_DEPARTAMENTO
                                        oExt.ListaDepartamentosAsig.Add(oDept)
                                    End If
                                Loop While extDeptoDAL.MoveNext
                            Else
                                'Si es reasignar y no se ha encontrado ningun resultado, se pone a nothing los listados 
                                If (asig = Asignacion.reasignar) Then
                                    oExt.ListaDepartamentosAsig = Nothing
                                End If
                            End If
                        Next
                    End If

                    If (asig = Asignacion.asignar Or asig = Asignacion.reemplazar And (oExt.ListaDepartamentosAsig IsNot Nothing AndAlso oExt.ListaDepartamentosAsig.Count > 0)) Then
                        For Each dept As ELL.ExtensionUsuDep In oExt.ListaDepartamentosAsig
                            extDeptoDAL.FlushData()
                            extDeptoDAL.Where.ID_EXTENSION.Value = dept.IdExtension
                            extDeptoDAL.Where.ID_DEPARTAMENTO.Value = dept.IdDepartamento
                            extDeptoDAL.Where.ID_TELEFONO.Value = dept.IdTelefono
                            extDeptoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extDeptoDAL.Query.Load()
                            If (extDeptoDAL.RowCount = 0) Then   'si esta actualmente asignado
                                extDeptoDAL.FlushData()
                                extDeptoDAL.AddNew()
                                extDeptoDAL.ID_DEPARTAMENTO = dept.IdDepartamento
                                If (dept.IdExtension <> Integer.MinValue) Then
                                    extDeptoDAL.ID_EXTENSION = dept.IdExtension
                                Else
                                    extDeptoDAL.ID_EXTENSION = oExt.Id
                                End If
                                extDeptoDAL.ID_TELEFONO = dept.IdTelefono
                                extDeptoDAL.F_DESDE = Date.Now
                                extDeptoDAL.Save()
                            End If
                        Next
                    End If
                    If (Not transaccionAbierta) Then tx.CommitTransaction()
                End If
            Catch ex As Exception
                If (Not transaccionAbierta) Then
                    tx.RollbackTransaction()
                    TransactionMgr.ThreadTransactionMgrReset()
                End If
                Throw New BatzException("errAsignacion", ex)
            End Try
        End Sub


        ''' <summary>
        ''' Modifica las extensiones de otros elementos
        ''' </summary>
        ''' <param name="oExt">Extension</param>
        ''' <param name="asig">Indicara si habra que asignar,desasignar o reemplazar</param>
        Private Sub modificarAsignacionOtros(ByVal oExt As ELL.Extension, ByVal asig As Asignacion, Optional ByVal transaccionAbierta As Boolean = False)
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                Dim oOtros As ELL.ExtensionUsuDep
                If (oExt.ListaOtrosAsig IsNot Nothing) Then
                    Dim extOtrosDAL As New DAL.EXTENSION_OTROS
                    If (Not transaccionAbierta) Then tx.BeginTransaction()

                    If (asig = Asignacion.desasignar Or asig = Asignacion.reemplazar) Then
                        For Each otr As ELL.ExtensionUsuDep In oExt.ListaOtrosAsig
                            extOtrosDAL.FlushData()
                            extOtrosDAL.Where.ID_EXTENSION.Value = otr.IdExtension
                            If (otr.IdTelefono <> Integer.MinValue) Then extOtrosDAL.Where.ID_TELEFONO.Value = otr.IdTelefono
                            If (otr.IdOtros <> Integer.MinValue) Then extOtrosDAL.Where.ID_OTRO.Value = otr.IdOtros
                            extOtrosDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extOtrosDAL.Query.Load()
                            If (extOtrosDAL.RowCount > 0) Then
                                'Si es reasignar, se inicializa para informar todos los registros que se van a actualizar
                                If (asig = Asignacion.reasignar) Then
                                    oExt.ListaOtrosAsig = New List(Of ELL.ExtensionUsuDep)
                                End If
                                Do
                                    extOtrosDAL.F_HASTA = Now.Date
                                    extOtrosDAL.Save()
                                    If (asig = Asignacion.reasignar) Then
                                        oOtros = New ELL.ExtensionUsuDep
                                        oOtros.IdExtension = otr.IdExtension
                                        oOtros.IdTelefono = otr.IdTelefono
                                        oOtros.IdOtros = extOtrosDAL.ID_OTRO
                                        oExt.ListaOtrosAsig.Add(oOtros)
                                    End If
                                Loop While extOtrosDAL.MoveNext
                            Else
                                'Si es reasignar y no se ha encontrado ningun resultado, se pone a nothing los listados 
                                If (asig = Asignacion.reasignar) Then
                                    oExt.ListaOtrosAsig = Nothing
                                End If
                            End If
                        Next
                    End If

                    If (asig = Asignacion.asignar Or asig = Asignacion.reemplazar And (oExt.ListaOtrosAsig IsNot Nothing AndAlso oExt.ListaOtrosAsig.Count > 0)) Then
                        For Each otro As ELL.ExtensionUsuDep In oExt.ListaOtrosAsig
                            extOtrosDAL.FlushData()
                            extOtrosDAL.Where.ID_EXTENSION.Value = otro.IdExtension
                            extOtrosDAL.Where.ID_OTRO.Value = otro.IdOtros
                            extOtrosDAL.Where.ID_TELEFONO.Value = otro.IdTelefono
                            extOtrosDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extOtrosDAL.Query.Load()
                            If (extOtrosDAL.RowCount = 0) Then   'si esta actualmente asignado
                                extOtrosDAL.FlushData()
                                extOtrosDAL.AddNew()
                                extOtrosDAL.ID_OTRO = otro.IdOtros
                                If (otro.IdExtension <> Integer.MinValue) Then
                                    extOtrosDAL.ID_EXTENSION = otro.IdExtension
                                Else
                                    extOtrosDAL.ID_EXTENSION = oExt.Id
                                End If
                                extOtrosDAL.ID_TELEFONO = otro.IdTelefono
                                extOtrosDAL.F_DESDE = Date.Now
                                extOtrosDAL.Save()
                            End If
                        Next
                    End If
                    If (Not transaccionAbierta) Then tx.CommitTransaction()
                End If
            Catch ex As Exception
                If (Not transaccionAbierta) Then
                    tx.RollbackTransaction()
                    TransactionMgr.ThreadTransactionMgrReset()
                End If
                Throw New BatzException("errAsignacion", ex)
            End Try
        End Sub


        ''' <summary>
        ''' Realiza la asignacion o devolucion de un telefono
        ''' </summary>        
        ''' <param name="oExtUsu">Objeto que contiene la informacion</param>                
        ''' <param name="tipo">Tipo de asignacion (asignacion,devolucion)</param>
        ''' <param name="asignac">Indicara si las asignacion o devolucion es a una persona, departamento u otro</param>
        Public Function AsigDevolucionTlfno(ByVal oExtUsu As ELL.ExtensionUsuDep, ByVal tipo As AsignacionDev, ByVal asignac As ELL.Extension.AsociarA) As Boolean
            If (asignac = ELL.Extension.AsociarA.personal) Then
                Return AsigDevolucionTlfnoPersona(oExtUsu, tipo)
            ElseIf (asignac = ELL.Extension.AsociarA.otros) Then
                Return AsigDevolucionTlfnoOtro(oExtUsu, tipo)
            End If
        End Function


        ''' <summary>
        ''' Realiza la asignacion o devolucion de un telefono a una persona
        ''' </summary>
        ''' <param name="oExtUsu"></param>
        ''' <param name="tipo"></param>
        ''' <returns></returns>
        Private Function AsigDevolucionTlfnoPersona(ByVal oExtUsu As ELL.ExtensionUsuDep, ByVal tipo As AsignacionDev) As Boolean
            Try
                Dim extUsuDAL As New DAL.EXTENSION_PERSONAS

                If (tipo = AsignacionDev.asignacion) Then
                    extUsuDAL.AddNew()
                Else
                    extUsuDAL.LoadByPrimaryKey(oExtUsu.FechaDesde, oExtUsu.IdExtension, oExtUsu.IdTelefono, oExtUsu.IdUsuario)
                End If

                If (extUsuDAL.RowCount = 1) Then
                    extUsuDAL.ID_EXTENSION = oExtUsu.IdExtension
                    extUsuDAL.ID_TELEFONO = oExtUsu.IdTelefono
                    extUsuDAL.ID_USUARIO = oExtUsu.IdUsuario

                    extUsuDAL.F_DESDE = oExtUsu.FechaDesde
                    If (oExtUsu.FechaHasta <> Date.MinValue) Then extUsuDAL.F_HASTA = oExtUsu.FechaHasta

                    extUsuDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errAsignacion", ex)
            End Try
        End Function


        ''' <summary>
        ''' Realiza la asignacion o devolucion de un telefono a un item otro
        ''' </summary>
        ''' <param name="oExtUsu"></param>
        ''' <param name="tipo"></param>
        ''' <returns></returns>
        Private Function AsigDevolucionTlfnoOtro(ByVal oExtUsu As ELL.ExtensionUsuDep, ByVal tipo As AsignacionDev) As Boolean
            Try
                Dim extOtroDAL As New DAL.EXTENSION_OTROS

                If (tipo = AsignacionDev.asignacion) Then
                    extOtroDAL.AddNew()
                Else
                    extOtroDAL.LoadByPrimaryKey(oExtUsu.FechaDesde, oExtUsu.IdExtension, oExtUsu.IdOtros, oExtUsu.IdTelefono)
                End If

                If (extOtroDAL.RowCount = 1) Then
                    extOtroDAL.ID_EXTENSION = oExtUsu.IdExtension
                    extOtroDAL.ID_TELEFONO = oExtUsu.IdTelefono
                    extOtroDAL.ID_OTRO = oExtUsu.IdOtros

                    extOtroDAL.F_DESDE = oExtUsu.FechaDesde
                    If (oExtUsu.FechaHasta <> Date.MinValue) Then extOtroDAL.F_HASTA = oExtUsu.FechaHasta

                    extOtroDAL.Save()
                    Return True
                End If
                Return False
            Catch ex As Exception
                Throw New BatzException("errAsignacion", ex)
            End Try
        End Function

#End Region

#Region "Consultas de extensiones de personas y departamentos"

        ''' <summary>
        ''' Obtiene la informacion de las extensiones de una o varias personas
        ''' </summary>
        ''' <param name="oUser">Usuario</param>
        ''' <returns>Lista de objetos de tipo busqueda</returns>        
        Public Function getExtensionPersona(ByVal oUser As SABLib.ELL.Usuario) As ELL.TelefonoExtension
            Try
                Dim oTlfnoExt As ELL.TelefonoExtension = Nothing
                Dim extPersoDAL As New DAL.EXTENSION_PERSONAS

                '1º Se obtiene la extension activa del usuario
                extPersoDAL.Where.ID_USUARIO.Value = oUser.Id
                extPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                extPersoDAL.Query.AddOrderBy(DAL.EXTENSION_PERSONAS.ColumnNames.F_DESDE, WhereParameter.Dir.ASC)
                extPersoDAL.Query.Load()

                If (extPersoDAL.RowCount = 1) Then
                    '2º Se buscan la extension interna y por si tuviera, la extension movil (id_ext_interna=idExt)
                    Dim extDAL As New DAL.EXTENSION
                    extDAL.Where.VISIBLE.Value = CInt(True)
                    extDAL.Query.AddConjunction(WhereParameter.Conj.AND_)
                    extDAL.Query.OpenParenthesis()
                    extDAL.Where.ID.Value = extPersoDAL.ID_EXTENSION
                    Dim wp As AccesoAutomaticoBD.WhereParameter = extDAL.Where.TearOff.ID_EXT_INTERNA
                    wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                    wp.Value = extPersoDAL.ID_EXTENSION
                    extDAL.Query.CloseParenthesis()
                    extDAL.Query.Load()

                    If (extDAL.RowCount > 0) Then
                        oTlfnoExt = New ELL.TelefonoExtension
                        oTlfnoExt.Nombre = oUser.NombreCompleto                        
                        Do
                            If (extDAL.ID_TIPOEXT = ELL.Extension.TipoExt.interna) Then
                                '3aº Si encuentra una extension interna
                                oTlfnoExt.ExtensionInterna = extDAL.EXTENSION
                                oTlfnoExt.IdTipoLinea = extDAL.ID_TIPOLINEA
                                If Not (extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TELEFONO)) Then
                                    '4aº Si la extension tiene asociado un tlfno, se busca el mismo
                                    Dim tlfnoComp As New BLL.TelefonoComponent
                                    Dim oTlfno As New ELL.Telefono
                                    oTlfno.Id = extDAL.ID_TELEFONO
                                    oTlfno = tlfnoComp.getTelefono(oTlfno)
                                    oTlfnoExt.TlfnoDirecto = oTlfno.Numero                                    
                                End If
                            ElseIf (extDAL.ID_TIPOEXT = ELL.Extension.TipoExt.movil) Then
                                '3bº Si encuentra una extension movil
                                oTlfnoExt.ExtensionMovil = extDAL.EXTENSION
                                If Not (extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TELEFONO)) Then
                                    '4bº Si la extension tiene asociado un movil, se busca el mismo
                                    Dim tlfnoComp As New BLL.TelefonoComponent
                                    Dim oTlfno As New ELL.Telefono
                                    oTlfno.Id = extDAL.ID_TELEFONO
                                    oTlfno = tlfnoComp.getTelefono(oTlfno)
                                    oTlfnoExt.TlfnoMovil = oTlfno.Numero
                                End If
                            End If
                        Loop While extDAL.MoveNext
                    End If
                End If

                Return oTlfnoExt
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene la informacion de las extensiones de una o varias personas
        ''' </summary>
        ''' <param name="oUser">Usuario</param>
        ''' <returns>Lista de objetos de tipo busqueda</returns>        
        Public Function getExtensionesPersona(ByVal oUser As SABLib.ELL.Usuario) As List(Of ELL.TelefonoExtension)
            Try
                Dim lTlfno As New List(Of ELL.TelefonoExtension)
                Dim oTlfnoExt As ELL.TelefonoExtension = Nothing
                Dim extPersoDAL As New DAL.EXTENSION_PERSONAS
                Dim plantComp As New SABLib.BLL.PlantasComponent
                Dim oPlanta As SABLib.ELL.Planta

                '1º Se obtiene la extension activa del usuario
                extPersoDAL.Where.ID_USUARIO.Value = oUser.Id
                extPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                extPersoDAL.Query.AddOrderBy(DAL.EXTENSION_PERSONAS.ColumnNames.F_DESDE, WhereParameter.Dir.ASC)
                extPersoDAL.Query.Load()

                If (extPersoDAL.RowCount > 0) Then
                    Do
                        '2º Se buscan la extension interna y por si tuviera, la extension movil (id_ext_interna=idExt)                        
                        Dim extDAL As New DAL.EXTENSION
                        extDAL.Where.VISIBLE.Value = CInt(True)
                        extDAL.Query.AddConjunction(WhereParameter.Conj.AND_)
                        extDAL.Query.OpenParenthesis()
                        extDAL.Where.ID.Value = extPersoDAL.ID_EXTENSION
                        Dim wp As AccesoAutomaticoBD.WhereParameter = extDAL.Where.TearOff.ID_EXT_INTERNA
                        wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                        wp.Value = extPersoDAL.ID_EXTENSION
                        extDAL.Query.CloseParenthesis()
                        extDAL.Query.Load()

                        If (extDAL.RowCount > 0) Then
                            oTlfnoExt = New ELL.TelefonoExtension
                            oTlfnoExt.idSab = oUser.Id
                            oTlfnoExt.Nombre = oUser.NombreCompleto                            
                            oPlanta = plantComp.GetPlanta(extDAL.ID_PLANTA)
                            oTlfnoExt.IdPlanta = extDAL.ID_PLANTA
                            oTlfnoExt.Planta = oPlanta.Nombre                            
                            Do
                                If (extDAL.ID_TIPOEXT = ELL.Extension.TipoExt.interna) Then
                                    '3aº Si encuentra una extension interna
                                    oTlfnoExt.ExtensionInterna = extDAL.EXTENSION
                                    oTlfnoExt.IdTipoLinea = extDAL.ID_TIPOLINEA
                                    If Not (extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TELEFONO)) Then
                                        '4aº Si la extension tiene asociado un tlfno, se busca el mismo
                                        Dim tlfnoComp As New BLL.TelefonoComponent
                                        Dim oTlfno As New ELL.Telefono
                                        oTlfno.Id = extDAL.ID_TELEFONO
                                        oTlfno = tlfnoComp.getTelefono(oTlfno)
                                        oTlfnoExt.TlfnoDirecto = oTlfno.Numero
                                    End If
                                ElseIf (extDAL.ID_TIPOEXT = ELL.Extension.TipoExt.movil) Then
                                    '3bº Si encuentra una extension movil
                                    oTlfnoExt.ExtensionMovil = extDAL.EXTENSION
                                    If Not (extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TELEFONO)) Then
                                        '4bº Si la extension tiene asociado un movil, se busca el mismo
                                        Dim tlfnoComp As New BLL.TelefonoComponent
                                        Dim oTlfno As New ELL.Telefono
                                        oTlfno.Id = extDAL.ID_TELEFONO
                                        oTlfno = tlfnoComp.getTelefono(oTlfno)
                                        oTlfnoExt.TlfnoMovil = oTlfno.Numero
                                    End If
                                End If
                            Loop While extDAL.MoveNext
                            lTlfno.Add(oTlfnoExt)
                        End If
                    Loop While extPersoDAL.MoveNext
                End If

                Return lTlfno
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function

        '''' <summary>
        '''' Obtiene la informacion de las extensiones de todas las personas de una planta
        '''' </summary>
        '''' <param name="idPlanta">Identificador de la planta</param>
        '''' <returns>Lista de objetos de tipo busqueda</returns>        
        'Public Function getExtensionesPersonas(ByVal idPlanta As Integer) As List(Of ELL.TelefonoExtension)
        '    Try
        '        Dim oUser As SABLib.ELL.Usuario
        '        Dim oTlfnoExt As ELL.TelefonoExtension
        '        Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
        '        Dim lUsuarios As List(Of SABLib.ELL.Usuario)
        '        Dim oDep As SABLib.ELL.Departamento
        '        Dim depComp As New SABLib.BLL.DepartamentosComponent
        '        Dim userComp As New BLL.UsuariosComponent
        '        Dim nombreDpto As String = String.Empty
        '        Dim idDpto As Integer = Integer.MinValue


        '        lUsuarios = userComp.getUsuarios(idPlanta)

        '        For Each oUser In lUsuarios
        '            nombreDpto = String.Empty
        '            idDpto = Integer.MinValue
        '            If (oUser.IdDepartamento <> String.Empty) Then
        '                oDep = New SABLib.ELL.Departamento
        '                oDep.Id = oUser.IdDepartamento
        '                oDep.IdPlanta = idPlanta
        '                oDep = depComp.GetDepartamento(oDep)
        '                If (oDep IsNot Nothing) Then
        '                    nombreDpto = oDep.Nombre
        '                    idDpto = oDep.Id
        '                End If
        '            End If
        '            oTlfnoExt = getExtensionPersona(oUser)
        '            oTlfnoExt.Departamento = nombreDpto
        '            oTlfnoExt.IdDepartamento = idDpto
        '            lTlfnoExt.Add(oTlfnoExt)
        '        Next
        '        Return lTlfnoExt
        '    Catch batzEx As BatzException
        '        Throw batzEx
        '    Catch ex As Exception
        '        Throw New BatzException("errBuscar", ex)
        '    End Try
        'End Function

        ''' <summary>
        ''' Obtiene la informacion de la extension de una departamento
        ''' </summary>
        ''' <param name="oDept">Objeto departamento</param> 
        ''' <param name="obtainWithOutExt">Indica si se quieren obtener los registros de usuarios sin extensiones</param>       
        ''' <returns>Objeto de tipo busqueda</returns>        
        Public Function getExtensionDepartamento(ByVal oDept As ELL.Departamento, Optional ByVal obtainWithOutExt As Boolean = False) As List(Of ELL.TelefonoExtension)
            Try
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                Dim lTlfnoExtAux As List(Of ELL.TelefonoExtension) = Nothing
                Dim oTlfnoExt As ELL.TelefonoExtension = Nothing
                Dim extDeptoDAL As New DAL.EXTENSION_DEPARTAMENTOS
                Dim usuComp As New BLL.UsuariosComponent
                Dim lUsers As List(Of Sablib.ELL.Usuario)
                Dim bConExtension As Boolean
                Dim nombrePlanta As String = String.Empty

                lUsers = usuComp.getUsuarios(oDept.IdPlanta, oDept.ID)

                If (obtainWithOutExt) Then
                    Dim plantComp As New Sablib.BLL.PlantasComponent
                    Dim oPlanta As Sablib.ELL.Planta = plantComp.GetPlanta(oDept.IdPlanta)
                    If (oPlanta IsNot Nothing) Then nombrePlanta = oPlanta.Nombre
                End If

                For Each ouser In lUsers
                    If (ouser.IdDepartamento = oDept.ID.ToString()) Then
                        bConExtension = False
                        'oTlfnoExt = getExtensionPersona(ouser)
                        lTlfnoExtAux = getExtensionesPersona(ouser)
                        If (lTlfnoExtAux IsNot Nothing AndAlso lTlfnoExtAux.Count > 0) Then
                            lTlfnoExt.AddRange(lTlfnoExtAux)
                            bConExtension = True
                        End If

                        'Buscar en Telefonos_personas por si se le ha asignado un telefono sin extension
                        Dim tlfnoComp As New BLL.TelefonoComponent
                        lTlfnoExtAux = tlfnoComp.getTelefonosPersona(ouser)
                        If (lTlfnoExtAux IsNot Nothing) Then
                            For Each oExt As ELL.TelefonoExtension In lTlfnoExtAux
                                oExt.idSab = ouser.Id
                                oExt.Nombre = ouser.NombreCompleto
                                oExt.IdDepartamento = oDept.ID
                                oExt.Departamento = oDept.Nombre
                                oExt.IdPlanta = oDept.IdPlanta
                                oExt.Planta = nombrePlanta
                                lTlfnoExt.Add(oExt)
                                bConExtension = True
                            Next
                        End If
                        'oTlfnoExt = tlfnoComp.getTelefonoPersona(ouser)

                        'If (oTlfnoExt IsNot Nothing) Then
                        '    oTlfnoExt.idSab = ouser.Id
                        '    oTlfnoExt.Nombre = ouser.NombreCompleto
                        '    oTlfnoExt.IdDepartamento = oDept.ID
                        '    oTlfnoExt.Departamento = oDept.Nombre
                        '    oTlfnoExt.IdPlanta = oDept.IdPlanta
                        '    oTlfnoExt.Planta = nombrePlanta
                        '    lTlfnoExt.Add(oTlfnoExt)
                        '    bConExtension = True
                        'End If

                        'Sino tiene ninguna extension y se quieren obtener los usuarios sin extension, se mete uno
                        If (obtainWithOutExt And Not bConExtension) Then
                            oTlfnoExt = New ELL.TelefonoExtension
                            oTlfnoExt.idSab = ouser.Id
                            oTlfnoExt.Nombre = ouser.NombreCompleto
                            oTlfnoExt.IdDepartamento = oDept.ID
                            oTlfnoExt.Departamento = oDept.Nombre
                            oTlfnoExt.IdPlanta = oDept.IdPlanta
                            oTlfnoExt.Planta = nombrePlanta
                            lTlfnoExt.Add(oTlfnoExt)
                        End If
                    End If
                Next

                lTlfnoExt.AddRange(getExtensionesDeptByIdDept(oDept.ID))

                lTlfnoExt.Sort(Function(oTlfnoExt1 As ELL.TelefonoExtension, oTlfnoExt2 As ELL.TelefonoExtension) oTlfnoExt1.Nombre < oTlfnoExt2.Nombre)

                Return lTlfnoExt
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de las extensiones de todas las departamentos de una planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <returns>Lista de objetos de tipo busqueda</returns>        
        Public Function getExtensionesDepartamento(ByVal idPlanta As Integer) As List(Of ELL.TelefonoExtension)
            Try
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                Dim depComp As New BLL.DepartamentosComponent
                Dim listDep As List(Of Sablib.ELL.Departamento) = depComp.getDepartamentos(DepartamentosComponent.EDepartamentos.Activos, idPlanta)
                listDep.Sort(Function(oDep1 As Sablib.ELL.Departamento, oDep2 As Sablib.ELL.Departamento) oDep1.Nombre < oDep2.Nombre)
                For Each oDept As Sablib.ELL.Departamento In listDep
                    oDept.IdPlanta = idPlanta
                    lTlfnoExt.AddRange(getExtensionDepartamento(New ELL.Departamento With {.ID = oDept.Id, .Nombre = oDept.Nombre}))
                Next
                Return lTlfnoExt
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de las extensiones de todas los items de un departamento
        ''' </summary>
        ''' <param name="idDep">Identificador del departamento</param>               
        Public Function getExtensionesDeptByIdDept(ByVal idDep As Integer) As List(Of ELL.TelefonoExtension)
            Try
                Dim extDAL As New DAL.EXTENSION_DEPARTAMENTOS
                Dim tlfnoExt As ELL.TelefonoExtension
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                Dim depComp As New BLL.DepartamentosComponent
                Dim oDep As New ELL.Departamento With {.Id = idDep}                
                Dim dt As DataTable

                dt = extDAL.getExtensionesDepartamento(idDep)

                For Each row As DataRow In dt.Rows
                    tlfnoExt = New ELL.TelefonoExtension
                    If Not row.IsNull(DAL.EXTENSION.ColumnNames.NOMBRE) Then tlfnoExt.Nombre = row(DAL.EXTENSION.ColumnNames.NOMBRE)
                    If Not row.IsNull("ExtensionInterna") Then tlfnoExt.ExtensionInterna = row("ExtensionInterna")
                    If Not row.IsNull("TlfnoDirecto") Then tlfnoExt.TlfnoDirecto = row("TlfnoDirecto")
                    If Not row.IsNull("ExtensionMovil") Then tlfnoExt.ExtensionMovil = row("ExtensionMovil")
                    If Not row.IsNull("TlfnoMovil") Then tlfnoExt.TlfnoMovil = row("TlfnoMovil")
                    If Not row.IsNull("Planta") Then tlfnoExt.Planta = row("Planta")
                    If Not row.IsNull("Id_Planta") Then tlfnoExt.IdPlanta = row("Id_Planta")
                    If Not row.IsNull("Id_TipoLinea") Then tlfnoExt.IdTipoLinea = CInt(row("Id_TipoLinea"))
                    lTlfnoExt.Add(tlfnoExt)
                Next
                Return lTlfnoExt
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene la informacion de las extensiones de una o varios items de otro
        ''' </summary>
        ''' <param name="oOtro">Otro</param>
        ''' <returns>Lista de objetos de tipo busqueda</returns>        
        Public Function getExtensionOtro(ByVal oOtro As ELL.Otros) As ELL.TelefonoExtension
            Try
                Dim oTlfnoExt As ELL.TelefonoExtension = Nothing
                Dim extOtrosDAL As New DAL.EXTENSION_OTROS
                Dim plantComp As New SABLib.BLL.PlantasComponent
                Dim oPlanta As SABLib.ELL.Planta

                '1º Se obtiene la extension activa del item otro
                extOtrosDAL.Where.ID_OTRO.Value = oOtro.Id
                extOtrosDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                extOtrosDAL.Query.AddOrderBy(DAL.EXTENSION_OTROS.ColumnNames.F_DESDE, WhereParameter.Dir.ASC)
                extOtrosDAL.Query.Load()

                If (extOtrosDAL.RowCount = 1) Then
                    '2º Se buscan la extension interna y por si tuviera, la extension movil (id_ext_interna=idExt)
                    Dim extDAL As New DAL.EXTENSION
                    extDAL.Where.VISIBLE.Value = CInt(True)
                    extDAL.Query.AddConjunction(WhereParameter.Conj.AND_)
                    extDAL.Query.OpenParenthesis()
                    extDAL.Where.ID.Value = extOtrosDAL.ID_EXTENSION
                    Dim wp As AccesoAutomaticoBD.WhereParameter = extDAL.Where.TearOff.ID_EXT_INTERNA
                    wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                    wp.Value = extOtrosDAL.ID_EXTENSION
                    extDAL.Query.CloseParenthesis()
                    extDAL.Query.Load()

                    If (extDAL.RowCount > 0) Then
                        Do
                            oTlfnoExt = New ELL.TelefonoExtension
                            oTlfnoExt.Nombre = oOtro.Nombre
                            oPlanta = plantComp.GetPlanta(extDAL.ID_PLANTA)                            
                            If (extDAL.ID_TIPOEXT = ELL.Extension.TipoExt.interna) Then
                                '3aº Si encuentra una extension interna
                                oTlfnoExt.ExtensionInterna = extDAL.EXTENSION
                                oTlfnoExt.IdTipoLinea = extDAL.ID_TIPOLINEA
                                If Not (extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TELEFONO)) Then
                                    '4aº Si la extension tiene asociado un tlfno, se busca el mismo
                                    Dim tlfnoComp As New BLL.TelefonoComponent
                                    Dim oTlfno As New ELL.Telefono
                                    oTlfno.Id = extDAL.ID_TELEFONO
                                    oTlfno = tlfnoComp.getTelefono(oTlfno)
                                    oTlfnoExt.TlfnoDirecto = oTlfno.Numero
                                End If
                            ElseIf (extDAL.ID_TIPOEXT = ELL.Extension.TipoExt.movil) Then
                                '3bº Si encuentra una extension movil
                                oTlfnoExt.ExtensionMovil = extDAL.EXTENSION
                                If Not (extDAL.IsColumnNull(DAL.EXTENSION.ColumnNames.ID_TELEFONO)) Then
                                    '4bº Si la extension tiene asociado un movil, se busca el mismo
                                    Dim tlfnoComp As New BLL.TelefonoComponent
                                    Dim oTlfno As New ELL.Telefono
                                    oTlfno.Id = extDAL.ID_TELEFONO
                                    oTlfno = tlfnoComp.getTelefono(oTlfno)
                                    oTlfnoExt.TlfnoMovil = oTlfno.Numero
                                End If
                            End If
                        Loop While extDAL.MoveNext
                    End If
                End If

                Return oTlfnoExt
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene la informacion de las extensiones de todas los items de otros de una planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>       
        Public Function getExtensionesOtros(ByVal idPlanta As Integer) As List(Of ELL.TelefonoExtension)
            Try
                Dim oOtros As ELL.Otros
                Dim tlfnoExt As ELL.TelefonoExtension
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                Dim lOtros As List(Of ELL.Otros)
                Dim otrosComp As New BLL.OtrosComponent
                oOtros = New ELL.Otros With {.IdPlanta = idPlanta}

                lOtros = otrosComp.getOtros(oOtros)

                For Each oOtros In lOtros
                    tlfnoExt = getExtensionOtro(oOtros)
                    lTlfnoExt.Add(tlfnoExt)
                Next
                Return lTlfnoExt
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de las extensiones de todas los items de otros de una planta
        ''' </summary>
		''' <param name="idOtro">Identificador del otro</param>  
		''' <param name="bVisible">Indicara si se quieren obtener los visibles, no visibles o todos</param>             
		Public Function getExtensionesOtrosByIdOtro(ByVal idOtro As Integer, ByVal bVisible As Nullable(Of Boolean)) As List(Of ELL.TelefonoExtension)
            Try
                Dim oOtros As New ELL.Otros With {.Id = idOtro}
                Dim tlfnoExt As ELL.TelefonoExtension
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                Dim otrosComp As New BLL.OtrosComponent
                tlfnoExt = getExtensionOtro(oOtros)
                If (tlfnoExt Is Nothing) Then
                    lTlfnoExt = Nothing
                Else
                    lTlfnoExt.Add(tlfnoExt)
                End If
                Return lTlfnoExt
                'Dim extDAL As New DAL.EXTENSION_OTROS
                'Dim tlfnoExt As ELL.TelefonoExtension
                'Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                '            Dim otrosComp As New BLL.OtrosComponent
                '            Dim plantComp As New Sablib.BLL.PlantasComponent
                'Dim oOtro As New ELL.Otros With {.Id = idOtro}
                'Dim dt As DataTable

                'dt = extDAL.getExtensionesOtro(idOtro, bVisible)

                'For Each row As DataRow In dt.Rows
                '	tlfnoExt = New ELL.TelefonoExtension
                '	If Not row.IsNull(DAL.EXTENSION.ColumnNames.NOMBRE) Then tlfnoExt.Nombre = row(DAL.EXTENSION.ColumnNames.NOMBRE)
                '	If Not row.IsNull("ExtensionInterna") Then tlfnoExt.ExtensionInterna = row("ExtensionInterna")
                '	If Not row.IsNull("TlfnoDirecto") Then tlfnoExt.TlfnoDirecto = row("TlfnoDirecto")
                '	If Not row.IsNull("ExtensionMovil") Then tlfnoExt.ExtensionMovil = row("ExtensionMovil")
                '                If Not row.IsNull("TlfnoMovil") Then tlfnoExt.TlfnoMovil = row("TlfnoMovil")
                '                If Not row.IsNull("Id_TipoLinea") Then tlfnoExt.IdTipoLinea = CInt(row("Id_TipoLinea"))
                '                If Not row.IsNull("Id_Planta") Then                        
                '                    tlfnoExt.IdPlanta = CInt(row("Id_Planta"))                        
                '                    tlfnoExt.Planta = plantComp.GetPlanta(CInt(row("Id_Planta"))).Nombre
                '                End If

                '	lTlfnoExt.Add(tlfnoExt)
                'Next
                'Return lTlfnoExt
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
			End Try
        End Function

        ''' <summary>
        ''' Dada una extension, busca a quien pertenece en la planta seleccionada
        ''' </summary>
        ''' <param name="ext">Extension</param>
        ''' <param name="idPlanta">Si no se informa el id planta, se buscara en todas</param>
        ''' <returns>Lista de objetos de tipo busqueda</returns>        
        Public Function getExtensionesByExtension(ByVal ext As String, ByVal idPlanta As Integer) As List(Of ELL.TelefonoExtension)
            Try
                Dim lTlfno As List(Of ELL.TelefonoExtension)
                Dim oTlfnoExt As ELL.TelefonoExtension = Nothing
                Dim userComp As New Sablib.BLL.UsuariosComponent
                Dim deptComp As New BLL.DepartamentosComponent
                Dim tlfnoComp As New BLL.TelefonoComponent
                Dim otrosComp As New BLL.OtrosComponent                
                Dim oUser As Sablib.ELL.Usuario
                Dim oOtro As ELL.Otros
                Dim oDepto As ELL.Departamento
                Dim idExtension As Integer

                lTlfno = New List(Of ELL.TelefonoExtension)

                '1º Se obtiene la extension
                Dim extDAL As New DAL.EXTENSION
                extDAL.Where.VISIBLE.Value = CInt(True)
                extDAL.Where.EXTENSION.Value = ext
                If (idPlanta > 0) Then extDAL.Where.ID_PLANTA.Value = idPlanta
                extDAL.Query.Load()

                If (extDAL.RowCount > 0) Then                    
                    Do
                        'Si es una extension movil, se busca lo de abajo pero para su extension interna
                        If (extDAL.VISIBLE = CInt(True)) Then
                            If (extDAL.s_ID_EXT_INTERNA <> String.Empty) Then
                                idExtension = extDAL.ID_EXT_INTERNA
                            Else
                                idExtension = extDAL.ID
                            End If
                            '2º Se busca en extensiones_personas
                            Dim extPersoDAL As New DAL.EXTENSION_PERSONAS
                            extPersoDAL.Where.ID_EXTENSION.Value = idExtension
                            extPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extPersoDAL.Query.AddOrderBy(DAL.EXTENSION_OTROS.ColumnNames.F_DESDE, WhereParameter.Dir.ASC)
                            extPersoDAL.Query.Load()
                            If (extPersoDAL.RowCount > 0) Then
                                Do              
                                    oUser = userComp.GetUsuario(New Sablib.ELL.Usuario With {.Id = extPersoDAL.ID_USUARIO}, False)
                                    Dim lTlfnoExtensiones As List(Of ELL.TelefonoExtension) = getExtensionesPersona(oUser)
                                    If (lTlfnoExtensiones IsNot Nothing) Then lTlfno.AddRange(lTlfnoExtensiones)

                                    'Buscar en Telefonos_personas por si se le ha asignado un telefono sin extension
                                    'oTlfnoExt = tlfnoComp.getTelefonoPersona(oUser)
                                    'If (oTlfnoExt IsNot Nothing) Then lTlfno.Add(oTlfnoExt)                                    
                                    lTlfnoExtensiones = tlfnoComp.getTelefonosPersona(oUser)
                                    If (lTlfnoExtensiones IsNot Nothing AndAlso lTlfnoExtensiones.Count > 0) Then lTlfno.AddRange(lTlfnoExtensiones)

                                Loop While extPersoDAL.MoveNext
                            End If

                            '3º Se busca en extensiones_departamentos
                            Dim extDeptoDAL As New DAL.EXTENSION_DEPARTAMENTOS
                            extDeptoDAL.Where.ID_EXTENSION.Value = idExtension
                            extDeptoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extDeptoDAL.Query.AddOrderBy(DAL.EXTENSION_OTROS.ColumnNames.F_DESDE, WhereParameter.Dir.ASC)
                            extDeptoDAL.Query.Load()
                            If (extDeptoDAL.RowCount > 0) Then
                                Do
                                    oDepto = deptComp.getDepartamento(extDeptoDAL.ID_DEPARTAMENTO, extDAL.ID_PLANTA)
                                    lTlfno = getExtensionDepartamento(oDepto, True)  'Se quieren obtener tambien los usuarios del departamento que no tengan extension                                                       
                                Loop While extDeptoDAL.MoveNext
                            End If

                            '4º Se busca en extensiones_otros
                            Dim extOtrosDAL As New DAL.EXTENSION_OTROS
                            extOtrosDAL.Where.ID_EXTENSION.Value = idExtension
                            extOtrosDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            extOtrosDAL.Query.AddOrderBy(DAL.EXTENSION_OTROS.ColumnNames.F_DESDE, WhereParameter.Dir.ASC)
                            extOtrosDAL.Query.Load()
                            If (extOtrosDAL.RowCount > 0) Then
                                Do
                                    oTlfnoExt = New ELL.TelefonoExtension
                                    oOtro = otrosComp.getOtro(extOtrosDAL.ID_OTRO)
                                    lTlfno = getExtensionesOtrosByIdOtro(extOtrosDAL.ID_OTRO, True)
                                    If (lTlfno IsNot Nothing) Then                                        
                                        For Each tlfno As ELL.TelefonoExtension In lTlfno
                                            tlfno.Nombre = oOtro.Nombre
                                        Next
                                    End If                                   
                                Loop While extOtrosDAL.MoveNext
                            End If
                        End If
                    Loop While extDAL.MoveNext
                End If

                Return lTlfno
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function

#End Region

#Region "Ver Todos"

        ''' <summary>
        ''' Devuelve la informacion de las personas y departamentos de una planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <param name="bPersonas">Indica si se quieren obtener por persona o por departamento</param>
        ''' <returns>Lista de objetos busqueda</returns>        
        Public Function VerTodos(ByVal idPlanta As Integer, ByVal bPersonas As Boolean, Optional ByVal vigentes As Boolean = True) As List(Of ELL.TelefonoExtension)
            Dim reader As IDataReader = Nothing
            Try
                Dim extensionDAL As New DAL.EXTENSION
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                Dim oTlfnoExt As ELL.TelefonoExtension
                reader = extensionDAL.VerTodos(idPlanta, bPersonas, vigentes)
                While reader.Read
                    oTlfnoExt = New ELL.TelefonoExtension
                    oTlfnoExt.Nombre = BLL.Utils.stringNull(reader.GetValue(0)).ToUpper
                    oTlfnoExt.ExtensionInterna = BLL.Utils.integerNull(reader.GetValue(1))
                    oTlfnoExt.ExtensionMovil = BLL.Utils.integerNull(reader.GetValue(2))
                    oTlfnoExt.TlfnoDirecto = BLL.Utils.stringNull(reader.GetValue(4))
                    oTlfnoExt.TlfnoMovil = BLL.Utils.stringNull(reader.GetValue(5))
                    oTlfnoExt.IdDepartamento = BLL.Utils.stringNull(reader.GetValue(6))
                    If (oTlfnoExt.IdDepartamento = String.Empty) Then  'Sino se controla, mete una 'O' en departamento
                        oTlfnoExt.Departamento = String.Empty
                    Else
                        oTlfnoExt.Departamento = BLL.Utils.stringNull(reader.GetValue(7))
                    End If
                    If (reader.GetValue(8) > 0) Then oTlfnoExt.idSab = CInt(reader.GetValue(8))
                    If (Not reader.IsDBNull(9) AndAlso reader.GetValue(9) > 0) Then oTlfnoExt.IdTipoLinea = CInt(reader.GetValue(9))
                    lTlfnoExt.Add(oTlfnoExt)
                End While

                Return lTlfnoExt
            Catch ex As Exception
                Throw New BatzException("errMostrarExtensiones", ex)
            Finally
                If (reader IsNot Nothing AndAlso Not reader.IsClosed) Then reader.Close()
            End Try
        End Function

#End Region

#Region "Sincronizacion con Geminix"

        ''' <summary>
        ''' Sincroniza SAB con telefonos, para saber que usuarios se han dado de baja, y tienen asociada una extension o un telefono
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta de gestion</param>
        ''' <returns></returns>        
        Public Function SincronizacionGeminix(ByVal idPlanta As Integer) As DataTable
            Dim sincroComp As New DAL.EXTENSION_PERSONAS
            Dim idOld As Integer = Integer.MinValue
            Dim row As DataRow
            Dim dt As DataTable = sincroComp.SincronizacionGeminix(idPlanta)
            dt.Columns.Add("Extensiones")  'Se añade esta columna, porque la columna extension es de tipo numerico, y no se podrian anexar mas de una extension            
            If (dt IsNot Nothing AndAlso dt.Rows.Count > 0) Then
                For i As Integer = dt.Rows.Count - 1 To 0 Step -1
                    row = dt.Rows(i)
                    If (CInt(row("Id")) = idOld) Then
                        If (dt.Rows(i + 1)("Extensiones") Is DBNull.Value AndAlso dt.Rows(i + 1)("Extensiones").ToString = String.Empty) Then
                            row("Extensiones") = dt.Rows(i + 1)("Extension")
                        Else
                            row("Extensiones") = dt.Rows(i + 1)("Extensiones") & "," & dt.Rows(i + 1)("Extension")
                        End If

                        If (dt.Rows(i + 1)("Numero") Is DBNull.Value AndAlso dt.Rows(i + 1)("Numero").ToString() = String.Empty) Then
                            row("Numero") = dt.Rows(i + 1)("Numero")
                        Else
                            If (row("Numero").ToString.IndexOf(dt.Rows(i + 1)("Numero")) = -1) Then
                                row("Numero") &= "," & dt.Rows(i + 1)("Numero")
                            End If
                        End If
                        dt.Rows(i + 1).Delete()
                    Else
                        If (row("Extension") IsNot DBNull.Value AndAlso row("Extension").ToString() <> String.Empty) Then
                            row("Extensiones") = dt.Rows(i)("Extension")
                        End If
                    End If
                    idOld = CInt(row("Id"))
                Next
                dt.AcceptChanges()
            End If
            Return dt
        End Function

#End Region

#Region "Cambios de datos de extensiones"

        ''' <summary>
        ''' Cuando se cambian datos de las extensiones, hay que dar de baja y dar de alta las asignaciones pertinentes
        ''' </summary>
        ''' <param name="oExtOld">Extension antigua</param>
        ''' <param name="oExtNew">Extension nueva</param>        
        Public Sub CambiosExtension(ByVal oExtOld As ELL.Extension, ByVal oExtNew As ELL.Extension)
            Try
                Dim bSinAsignar As Boolean = (oExtNew.TipoAsignacionMovil = ELL.Extension.AsignarA.sinAsignar)
                If (oExtOld.IdTipoExtension = ELL.Extension.TipoExt.movil) Then
                    '1-Formacion de objetos
                    '*****************************
                    'Si no tiene asignaciones, sera que se ha realizado un cambio de telefono                
                    If (oExtOld.ListaPersonasAsig Is Nothing And oExtOld.ListaDepartamentosAsig Is Nothing And oExtOld.ListaOtrosAsig Is Nothing) Then
                        Dim oExtUsuDep As New ELL.ExtensionUsuDep
                        oExtUsuDep.IdExtension = oExtOld.Id
                        If (oExtOld.IdTipoExtension = ELL.Extension.TipoExt.movil) Then oExtUsuDep.IdTelefono = oExtOld.IdTelefono
                        If (oExtOld.TipoAsignacionMovil = ELL.Extension.AsignarA.persona Or oExtOld.TipoAsignacionMovil = ELL.Extension.AsignarA.extensionInterna) Then  'Or oExtOld.TipoAsignacionMovil = ELL.Extension.AsignarA.sinAsignar
                            oExtOld.ListaPersonasAsig = New List(Of ELL.ExtensionUsuDep)
                            oExtOld.ListaPersonasAsig.Add(oExtUsuDep)

                            oExtOld.ListaDepartamentosAsig = New List(Of ELL.ExtensionUsuDep)
                            oExtOld.ListaDepartamentosAsig.Add(oExtUsuDep)
                        ElseIf (oExtOld.TipoAsignacionMovil = ELL.Extension.AsignarA.otros) Then
                            oExtOld.ListaOtrosAsig = New List(Of ELL.ExtensionUsuDep)
                            oExtOld.ListaOtrosAsig.Add(oExtUsuDep)
                        ElseIf (oExtOld.TipoAsignacionMovil = ELL.Extension.AsignarA.sinAsignar) Then
                            'Para que quite de las tres
                            oExtOld.ListaPersonasAsig = New List(Of ELL.ExtensionUsuDep)
                            oExtOld.ListaPersonasAsig.Add(oExtUsuDep)

                            oExtOld.ListaDepartamentosAsig = New List(Of ELL.ExtensionUsuDep)
                            oExtOld.ListaDepartamentosAsig.Add(oExtUsuDep)

                            oExtOld.ListaOtrosAsig = New List(Of ELL.ExtensionUsuDep)
                            oExtOld.ListaOtrosAsig.Add(oExtUsuDep)
                        End If

                        '2-Asignaciones 
                        '*****************************

                        'Desasignaciones de las extensiones antigua
                        'Al pasarles el estado reasignar, todas las filas modificadas, seran informadas en la lista, para que luego, se sepan que filas se tienen que asignar en la extension nueva
                        Select Case oExtOld.TipoAsignacionMovil
                            Case ELL.Extension.AsignarA.persona, ELL.Extension.AsignarA.extensionInterna, ELL.Extension.AsignarA.sinAsignar
                                modificarAsignacionPersona(oExtOld, Asignacion.reasignar, True)
                                modificarAsignacionDepartamento(oExtOld, Asignacion.reasignar, True)
                            Case ELL.Extension.AsignarA.otros
                                modificarAsignacionOtros(oExtOld, Asignacion.reasignar, True)
                        End Select

                        'Se asignan a las extensiones nuevas las listas que tendran que modificar
                        oExtNew.ListaPersonasAsig = oExtOld.ListaPersonasAsig
                        oExtNew.ListaDepartamentosAsig = oExtOld.ListaDepartamentosAsig
                        oExtNew.ListaOtrosAsig = oExtOld.ListaOtrosAsig


                        'Se les cambia el nº de telefono por el nuevo
                        If (oExtOld.ListaPersonasAsig IsNot Nothing) Then
                            For Each item As ELL.ExtensionUsuDep In oExtOld.ListaPersonasAsig
                                item.IdTelefono = oExtNew.IdTelefono
                            Next
                        End If

                        If (oExtOld.ListaDepartamentosAsig IsNot Nothing) Then
                            For Each item As ELL.ExtensionUsuDep In oExtOld.ListaDepartamentosAsig
                                item.IdTelefono = oExtNew.IdTelefono
                            Next
                        End If


                        If (oExtOld.ListaOtrosAsig IsNot Nothing) Then
                            For Each item As ELL.ExtensionUsuDep In oExtOld.ListaOtrosAsig
                                item.IdTelefono = oExtNew.IdTelefono
                            Next
                        End If

                    Else

                        '2-Asignaciones 
                        '*****************************

                        'Desasignaciones de las extensiones antigua
                        Select Case oExtOld.TipoAsignacionMovil
                            Case ELL.Extension.AsignarA.extensionInterna
                                modificarAsignacionPersona(oExtOld, Asignacion.asignarNoExtInt, True)
                                modificarAsignacionDepartamento(oExtOld, Asignacion.asignarNoExtInt, True)
                            Case ELL.Extension.AsignarA.persona, ELL.Extension.AsignarA.sinAsignar
                                modificarAsignacionPersona(oExtOld, Asignacion.desasignar, True)
                                modificarAsignacionDepartamento(oExtOld, Asignacion.desasignar, True)
                            Case ELL.Extension.AsignarA.otros
                                modificarAsignacionOtros(oExtOld, Asignacion.desasignar, True)
                        End Select
                    End If

                    If (Not oExtNew.Obsoleto) Then   'Cuando sea obsoleto, no habra que reasignar nada
                        If (bSinAsignar) Then  'si era sin asignar, abra que quitar las extensiones
                            modificarAsignacionPersona(oExtNew, Asignacion.desasignar, True)
                            modificarAsignacionDepartamento(oExtNew, Asignacion.desasignar, True)
                            modificarAsignacionOtros(oExtNew, Asignacion.desasignar, True)
                        Else
							'Asignaciones de las extensiones nuevas							
                            Select Case oExtNew.TipoAsignacionMovil
								Case ELL.Extension.AsignarA.persona	 ', ELL.Extension.AsignarA.extensionInterna
									modificarAsignacionPersona(oExtNew, Asignacion.asignar, True)
									modificarAsignacionDepartamento(oExtNew, Asignacion.asignar, True)
                                Case ELL.Extension.AsignarA.otros
									modificarAsignacionOtros(oExtNew, Asignacion.asignar, True)
								Case ELL.Extension.AsignarA.extensionInterna
									'Cuando una extension movil, pasa a estar ligada a una extension interna, solo se actualizan las personas o departamentos cuando tienen asiganados mas de una persona
									'Si solo tuvieran una, se habria cambiado de asignada a una persona a asignada a una extension interna, y por tanto, la extension movil deja de estar ligada a una persona
									'y no hay que reasignar (Antes se ejecutaban siempre y cuando una extension movil asignada a una persona pasaba a estar asignada a una extension interna, en este paso de nuevo
									'se volvia a asignar la extension movil [la oExtNew] a la misma persona)
									If (oExtNew.ListaPersonasAsig IsNot Nothing AndAlso oExtNew.ListaPersonasAsig.Count > 1) Then
										modificarAsignacionPersona(oExtNew, Asignacion.asignar, True)
									End If
									If (oExtNew.ListaDepartamentosAsig IsNot Nothing AndAlso oExtNew.ListaDepartamentosAsig.Count > 1) Then
										modificarAsignacionDepartamento(oExtNew, Asignacion.asignar, True)
									End If
							End Select
                        End If
                    End If
                Else 'interna

                    If (oExtOld.ListaPersonasAsig Is Nothing And oExtOld.ListaDepartamentosAsig Is Nothing And oExtOld.ListaOtrosAsig Is Nothing) Then
                        'Aqui entrara cuando se cambie el telefono
                        Dim oExtUsuDep As New ELL.ExtensionUsuDep
                        oExtUsuDep.IdExtension = oExtOld.Id
                        If (oExtOld.IdTipoExtension = ELL.Extension.TipoExt.movil) Then oExtUsuDep.IdTelefono = oExtOld.IdTelefono

                        'Para que quite de las tres
                        oExtOld.ListaPersonasAsig = New List(Of ELL.ExtensionUsuDep)
                        oExtOld.ListaPersonasAsig.Add(oExtUsuDep)

                        oExtOld.ListaDepartamentosAsig = New List(Of ELL.ExtensionUsuDep)
                        oExtOld.ListaDepartamentosAsig.Add(oExtUsuDep)

                        oExtOld.ListaOtrosAsig = New List(Of ELL.ExtensionUsuDep)
                        oExtOld.ListaOtrosAsig.Add(oExtUsuDep)

                        '2-Asignaciones 
                        '*****************************
                        modificarAsignacionPersona(oExtOld, Asignacion.reasignar, True)
                        modificarAsignacionDepartamento(oExtOld, Asignacion.reasignar, True)


                        'Se asignan a las extensiones nuevas las listas que tendran que modificar
                        oExtNew.ListaPersonasAsig = oExtOld.ListaPersonasAsig
                        oExtNew.ListaDepartamentosAsig = oExtOld.ListaDepartamentosAsig
                        oExtNew.ListaOtrosAsig = oExtOld.ListaOtrosAsig


                        'Se les cambia el nº de telefono por el nuevo
                        If (oExtOld.ListaPersonasAsig IsNot Nothing) Then
                            For Each item As ELL.ExtensionUsuDep In oExtOld.ListaPersonasAsig
                                item.IdTelefono = oExtNew.IdTelefono
                            Next
                        End If

                        If (oExtOld.ListaDepartamentosAsig IsNot Nothing) Then
                            For Each item As ELL.ExtensionUsuDep In oExtOld.ListaDepartamentosAsig
                                item.IdTelefono = oExtNew.IdTelefono
                            Next
                        End If


                        If (oExtOld.ListaOtrosAsig IsNot Nothing) Then
                            For Each item As ELL.ExtensionUsuDep In oExtOld.ListaOtrosAsig
                                item.IdTelefono = oExtNew.IdTelefono
                            Next
                        End If

                        If (Not oExtNew.Obsoleto) Then   'Cuando sea obsoleto, no habra que reasignar nada
                            modificarAsignacionPersona(oExtNew, Asignacion.asignar, True)
                            modificarAsignacionDepartamento(oExtNew, Asignacion.asignar, True)
                        End If
                    Else
                        'Cuando se cambio el tipo de extension (personal,departamental, otro), se limpia todo
                        modificarAsignacionPersona(oExtOld, Asignacion.desasignar, True)
                        modificarAsignacionDepartamento(oExtOld, Asignacion.desasignar, True)
                        modificarAsignacionOtros(oExtOld, Asignacion.desasignar, True)
                    End If


                End If
            Catch ex As Exception
                Throw New BatzException("", ex)
            End Try
        End Sub

#End Region

    End Class

End Namespace