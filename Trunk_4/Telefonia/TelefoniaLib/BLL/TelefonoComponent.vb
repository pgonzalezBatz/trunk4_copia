Imports AccesoAutomaticoBD

Namespace BLL

    Public Class TelefonoComponent

        Private tlfnoDAL As New DAL.TelefonoDAL

#Region "Enum"

        ''' <summary>
        ''' Estado en el que se puede encontrar un movil
        ''' </summary>
        ''' <remarks></remarks>
        Public Enum Estado As Integer
            libre = 0
            ocupado = 1
            todos = 2
        End Enum

        Public Enum Asignacion As Integer
            asignar = 0
            desasignar = 1
            reemplazar = 2
        End Enum

#End Region

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el telefono que cumple las condiciones
        ''' </summary>
        ''' <param name="oTlfno">Objeto tlfno</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getTelefono(ByVal oTlfno As ELL.Telefono) As ELL.Telefono
            Dim tlfnoDAL As New DAL.TELEFONO
            Dim oTlfnoMov As ELL.Telefono = Nothing
            Dim gestComp As New BLL.TelefonoComponent.GestorTlfnoComponent
            Try
                If (oTlfno.Id <> Integer.MinValue) Then tlfnoDAL.Where.ID.Value = oTlfno.Id
                If (oTlfno.Numero <> String.Empty) Then tlfnoDAL.Where.NUMERO.Value = oTlfno.Numero
                If (oTlfno.IdPlanta <> Integer.MinValue) Then tlfnoDAL.Where.ID_PLANTA.Value = oTlfno.IdPlanta                
                tlfnoDAL.Query.Load()

                If (tlfnoDAL.RowCount = 1) Then
                    oTlfnoMov = New ELL.Telefono()
                    oTlfnoMov.Id = tlfnoDAL.ID
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ID_CIA_TLFNO)) Then oTlfnoMov.IdCiaTlfno = tlfnoDAL.ID_CIA_TLFNO
                    oTlfnoMov.IdPlanta = tlfnoDAL.ID_PLANTA
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ID_GESTOR)) Then oTlfnoMov.IdUsuarioGestor = tlfnoDAL.ID_GESTOR                                        

                    oTlfnoMov.Modelo = tlfnoDAL.s_MODELO
                    oTlfnoMov.Numero = tlfnoDAL.s_NUMERO
                    oTlfnoMov.PIN = tlfnoDAL.s_PIN
                    oTlfnoMov.PUK = tlfnoDAL.s_PUK

                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ROAMING)) Then oTlfnoMov.Roaming = CType(tlfnoDAL.ROAMING, Boolean)
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.TIPOLINEAFIJO)) Then oTlfnoMov.Tipo_LineaFijo = tlfnoDAL.TIPOLINEAFIJO
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.VOZ_DATOS)) Then oTlfnoMov.VozODatos = tlfnoDAL.VOZ_DATOS
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.DUALIZADO)) Then oTlfnoMov.Dualizado = CType(tlfnoDAL.DUALIZADO, Boolean)
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.FIJO_MOVIL)) Then oTlfnoMov.FijoOMovil = tlfnoDAL.FIJO_MOVIL

                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.COMENTARIO)) Then oTlfnoMov.Comentarios = tlfnoDAL.COMENTARIO

                    oTlfnoMov.FechaAlta = tlfnoDAL.F_ALTA
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.F_BAJA)) Then oTlfnoMov.FechaBaja = tlfnoDAL.F_BAJA
                    oTlfnoMov.Obsoleto = tlfnoDAL.OBSOLETO
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ID_PERFIL_MOV)) Then oTlfnoMov.IdPerfilMovil = tlfnoDAL.ID_PERFIL_MOV
                    If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ID_TARIFA_DATOS)) Then oTlfnoMov.IdTarifaDatos = tlfnoDAL.ID_TARIFA_DATOS

                    oTlfnoMov.ListaPersonasAsig = PersonasAsignadas(oTlfnoMov.Id)
                End If
                Return oTlfnoMov
            Catch ex As Exception
                Throw New BatzException("errObtenerInfo", ex)
            End Try
        End Function


        ''' <summary>
        ''' Lista las personas asignadas a un telefono sin extension
        ''' </summary>
        ''' <param name="idTlfno">Identificador de un telefono</param>
        ''' <returns>Lista de usuarios</returns>        
        Private Function PersonasAsignadas(ByVal idTlfno As Integer) As List(Of ELL.TelefonoUsuDep)
            Try
                Dim tlfnoPersoDAL As New DAL.TELEFONO_PERSONAS
                Dim lUser As New List(Of ELL.TelefonoUsuDep)
                Dim oUser As ELL.TelefonoUsuDep = Nothing
                Dim userComp As New SABLib.BLL.UsuariosComponent
                Dim ousu As SABLib.ELL.Usuario

                tlfnoPersoDAL.Where.ID_TLFNO.Value = idTlfno
                'tlfnoPersoDAL.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
                'tlfnoPersoDAL.Query.OpenParenthesis()
                'tlfnoPersoDAL.Where.F_HASTA.Value = DateTime.Now
                'tlfnoPersoDAL.Where.F_HASTA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
                'Dim wp As AccesoAutomaticoBD.WhereParameter = tlfnoPersoDAL.Where.TearOff.F_HASTA
                'wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                'wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                'tlfnoPersoDAL.Query.CloseParenthesis()
                tlfnoPersoDAL.Query.Load()

                If (tlfnoPersoDAL.RowCount > 0) Then

                    Do
                        oUser = New ELL.TelefonoUsuDep
                        oUser.IdUsuario = tlfnoPersoDAL.ID_USUARIO
                        oUser.IdTelefono = idTlfno
                        ousu = New SABLib.ELL.Usuario
                        ousu.Id = oUser.IdUsuario
                        oUser.NombreUsuario = userComp.GetUsuario(ousu, False).NombreCompleto
                        oUser.FechaDesde = tlfnoPersoDAL.F_DESDE
                        If (Not tlfnoPersoDAL.IsColumnNull(DAL.TELEFONO_PERSONAS.ColumnNames.F_HASTA)) Then oUser.FechaHasta = tlfnoPersoDAL.F_HASTA

                        lUser.Add(oUser)
                    Loop While tlfnoPersoDAL.MoveNext
                End If

                Return lUser
            Catch ex As Exception
                Throw New BatzException("errObtenerPersonasAsignadas", ex)
            End Try
        End Function



        ''' <summary>
        ''' Obtiene todos los telefonos registrados en una planta
        ''' </summary>
        ''' <param name="oTlfno">Objeto telefono</param>
        ''' <returns>Lista de telefonos</returns>        
        Public Function getTelefonos(ByVal oTlfno As ELL.Telefono) As System.Collections.Generic.List(Of ELL.Telefono)
            Dim tlfnoDAL As New DAL.TELEFONO
            Dim lTelefonos As New List(Of ELL.Telefono)
            Dim oTlfnoMov As ELL.Telefono = Nothing
            Dim gestComp As New BLL.TelefonoComponent.GestorTlfnoComponent
            Try
                If (oTlfno.IdPlanta <> Integer.MinValue) Then tlfnoDAL.Where.ID_PLANTA.Value = oTlfno.IdPlanta
                If (oTlfno.FijoOMovil <> ELL.Telefono.FijoMovil.null) Then tlfnoDAL.Where.FIJO_MOVIL.Value = oTlfno.FijoOMovil
                If (oTlfno.IdCiaTlfno <> Integer.MinValue) Then tlfnoDAL.Where.ID_CIA_TLFNO.Value = oTlfno.IdCiaTlfno
                If (oTlfno.IdUsuarioGestor <> Integer.MinValue) Then tlfnoDAL.Where.ID_GESTOR.Value = oTlfno.IdUsuarioGestor
                If (oTlfno.Numero <> String.Empty) Then tlfnoDAL.Where.NUMERO.Value = oTlfno.Numero
                If (Not oTlfno.Obsoleto) Then tlfnoDAL.Where.OBSOLETO.Value = CInt(oTlfno.Obsoleto)
                tlfnoDAL.Query.Load()

                If (tlfnoDAL.RowCount > 0) Then
                    Do
                        oTlfnoMov = New ELL.Telefono()
                        oTlfnoMov.Id = tlfnoDAL.ID
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ID_CIA_TLFNO)) Then oTlfnoMov.IdCiaTlfno = tlfnoDAL.ID_CIA_TLFNO
                        oTlfnoMov.IdPlanta = tlfnoDAL.ID_PLANTA
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ID_GESTOR)) Then oTlfnoMov.IdUsuarioGestor = tlfnoDAL.ID_GESTOR

                        oTlfnoMov.Modelo = tlfnoDAL.s_MODELO
                        oTlfnoMov.Numero = tlfnoDAL.s_NUMERO
                        oTlfnoMov.PIN = tlfnoDAL.s_PIN
                        oTlfnoMov.PUK = tlfnoDAL.s_PUK

                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ROAMING)) Then oTlfnoMov.Roaming = CType(tlfnoDAL.ROAMING, Boolean)
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.TIPOLINEAFIJO)) Then oTlfnoMov.Tipo_LineaFijo = tlfnoDAL.TIPOLINEAFIJO
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.VOZ_DATOS)) Then oTlfnoMov.VozODatos = tlfnoDAL.VOZ_DATOS
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.DUALIZADO)) Then oTlfnoMov.Dualizado = CType(tlfnoDAL.DUALIZADO, Boolean)
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.FIJO_MOVIL)) Then oTlfnoMov.FijoOMovil = tlfnoDAL.FIJO_MOVIL

                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.COMENTARIO)) Then oTlfnoMov.Comentarios = tlfnoDAL.COMENTARIO

                        oTlfnoMov.FechaAlta = tlfnoDAL.F_ALTA
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.F_BAJA)) Then oTlfnoMov.FechaBaja = tlfnoDAL.F_BAJA
                        oTlfnoMov.Obsoleto = tlfnoDAL.OBSOLETO
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ID_PERFIL_MOV)) Then oTlfnoMov.IdPerfilMovil = tlfnoDAL.ID_PERFIL_MOV
                        If (Not tlfnoDAL.IsColumnNull(DAL.TELEFONO.ColumnNames.ID_TARIFA_DATOS)) Then oTlfnoMov.IdTarifaDatos = tlfnoDAL.ID_TARIFA_DATOS

                        lTelefonos.Add(oTlfnoMov)
                    Loop While tlfnoDAL.MoveNext
                End If
                Return lTelefonos
            Catch ex As Exception
                Throw New BatzException("errObtenerInfo", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todos los telefonos registrados en una planta que no esten asignados
        ''' Tampoco se mostraran los moviles que sean de tipo Datos
        ''' </summary>
        ''' <param name="oTlfno">Objeto telefono</param>
        ''' <returns>Lista de telefonos</returns>        
        Public Function getTelefonosLibres(ByVal oTlfno As ELL.Telefono) As System.Collections.Generic.List(Of ELL.Telefono)
            Dim tlfnoDAL As New DAL.TELEFONO
            Dim lTelefonos As New List(Of ELL.Telefono)
            Dim oTlfnoMov As ELL.Telefono = Nothing
            Dim gestComp As New BLL.TelefonoComponent.GestorTlfnoComponent
            Dim reader As IDataReader = Nothing
            Try
                reader = tlfnoDAL.getTelefonosLibres(oTlfno)

                While reader.Read
                    oTlfnoMov = New ELL.Telefono()
                    oTlfnoMov.Id = CInt(reader.Item(DAL.TELEFONO.ColumnNames.ID))                    
                    oTlfnoMov.IdPlanta = CInt(reader.Item(DAL.TELEFONO.ColumnNames.ID_PLANTA))
                    oTlfnoMov.FechaAlta = CType(reader.Item(DAL.TELEFONO.ColumnNames.F_ALTA), Date)

                    If (Not reader.IsDBNull(1)) Then oTlfnoMov.Numero = reader.Item(DAL.TELEFONO.ColumnNames.NUMERO)
                    If (Not reader.IsDBNull(3)) Then oTlfnoMov.FechaBaja = CType(reader.Item(DAL.TELEFONO.ColumnNames.F_BAJA), Date)
                    If (Not reader.IsDBNull(4)) Then oTlfnoMov.IdCiaTlfno = CInt(reader.Item(DAL.TELEFONO.ColumnNames.ID_CIA_TLFNO))
                    If (Not reader.IsDBNull(6)) Then oTlfnoMov.PIN = reader.Item(DAL.TELEFONO.ColumnNames.PIN)
                    If (Not reader.IsDBNull(7)) Then oTlfnoMov.PUK = reader.Item(DAL.TELEFONO.ColumnNames.PUK)
                    If (Not reader.IsDBNull(8)) Then oTlfnoMov.Dualizado = CType(reader.Item(DAL.TELEFONO.ColumnNames.DUALIZADO), Boolean)
                    If (Not reader.IsDBNull(9)) Then oTlfnoMov.VozODatos = reader.Item(DAL.TELEFONO.ColumnNames.VOZ_DATOS)
                    If (Not reader.IsDBNull(10)) Then oTlfnoMov.FijoOMovil = reader.Item(DAL.TELEFONO.ColumnNames.FIJO_MOVIL)
                    If (Not reader.IsDBNull(11)) Then oTlfnoMov.Modelo = reader.Item(DAL.TELEFONO.ColumnNames.MODELO)
                    If (Not reader.IsDBNull(12)) Then oTlfnoMov.IdUsuarioGestor = CInt(reader.Item(DAL.TELEFONO.ColumnNames.ID_GESTOR))
                    If (Not reader.IsDBNull(13)) Then oTlfnoMov.Roaming = CType(reader.Item(DAL.TELEFONO.ColumnNames.ROAMING), Boolean)
                    If (Not reader.IsDBNull(14)) Then oTlfnoMov.Comentarios = reader.Item(DAL.TELEFONO.ColumnNames.COMENTARIO)
                    If (Not reader.IsDBNull(15)) Then oTlfnoMov.Tipo_LineaFijo = reader.Item(DAL.TELEFONO.ColumnNames.TIPOLINEAFIJO)
                    If (Not reader.IsDBNull(16)) Then oTlfnoMov.Obsoleto = CType(reader.Item(DAL.TELEFONO.ColumnNames.OBSOLETO), Boolean)
                    If (Not reader.IsDBNull(17)) Then oTlfnoMov.IdPerfilMovil = reader.Item(DAL.TELEFONO.ColumnNames.ID_PERFIL_MOV)

                    lTelefonos.Add(oTlfnoMov)
                End While

                Return lTelefonos
            Catch ex As Exception
                Throw New BatzException("errObtenerInfo", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga los numeros de telefonos moviles que:
        ''' -Que esten gestionados por el gestor
        ''' -Que tengan asociada una extension
        ''' -Que su extension movil, coincida con un grupo de extension libre
        ''' </summary>
        ''' <param name="oTlfno">Objeto telefono</param>
        ''' <returns>Lista de telefonos</returns>        
        Public Function getTelefonosGestor(ByVal oTlfno As ELL.Telefono, ByVal state As Estado) As DataTable
            Dim tlfnoDAL As New DAL.TELEFONO
            Try                
                Return tlfnoDAL.getTelefonosGestor(oTlfno, state)
            Catch ex As Exception
                Throw New BatzException("errObtenerInfo", ex)
            End Try
        End Function

        ''' <summary>
        ''' Carga los numeros de telefonos moviles que:
        ''' -Que esten gestionados por el gestor
        ''' -Que tengan asociada una extension
        ''' -Que su extension movil, coincida con un grupo de extension libre
        ''' </summary>
        ''' <param name="oTlfno">Objeto telefono</param>
        ''' <returns>Lista de telefonos</returns>        
        Public Function getTelefonosGestor2(ByVal oTlfno As ELL.Telefono, ByVal idPlanta As Integer, ByVal state As Estado) As DataTable
            Dim tlfnoDAL As New DAL.TELEFONO
            Try                        
                Dim dataRow As DataRow
                Dim dt As DataTable = tlfnoDAL.getTelefonosGestor2(oTlfno)
                Dim deptoBLL As New Sablib.BLL.DepartamentosComponent
                Dim dtResul As New DataTable
                Dim extComp As New BLL.ExtensionComponent
                Dim oExt As ELL.Extension
                Dim bAñadir As Boolean
                Dim nombre, idDepto As String
                Dim dpto As Sablib.ELL.Departamento
                Dim fechaDesde As Date

                dtResul.Columns.Add("ID")
                dtResul.Columns.Add("NUMERO")
                dtResul.Columns.Add("EXTENSION")
                dtResul.Columns.Add("NOMBRE")
                dtResul.Columns.Add("DEPARTAMENTO")
                dtResul.Columns.Add("FECHA_DESDE")

                For Each row As DataRow In dt.Rows
                    fechaDesde = Date.MinValue
                    oExt = New ELL.Extension
                    oExt.Id = CInt(row.Item("ID_EXTENSION"))
                    oExt = extComp.getExtension(oExt, idPlanta)
                    bAñadir = False : dpto = Nothing
                    nombre = String.Empty : idDepto = String.Empty
                    If (state = Estado.libre) Then
                        bAñadir = (oExt.ListaPersonasAsig.Count = 0 And oExt.ListaOtrosAsig.Count = 0)
                    ElseIf (state = Estado.ocupado) Then
                        bAñadir = (oExt.ListaPersonasAsig.Count <> 0 Or oExt.ListaOtrosAsig.Count <> 0)
                    ElseIf (state = Estado.todos) Then
                        bAñadir = True
                    End If

                    If (bAñadir) Then
                        If (oExt.ListaPersonasAsig.Count > 0) Then
                            nombre = oExt.ListaPersonasAsig.Item(0).NombreUsuario
                            idDepto = oExt.ListaPersonasAsig.Item(0).IdDepartamento
                            fechaDesde = oExt.ListaPersonasAsig.Item(0).FechaDesde
                        ElseIf (oExt.ListaOtrosAsig.Count > 0) Then
                            nombre = oExt.ListaOtrosAsig.Item(0).NombreOtros
                            fechaDesde = oExt.ListaOtrosAsig.Item(0).FechaDesde
                        End If
                        dataRow = dtResul.NewRow
                        dataRow(0) = CInt(row.Item("ID_TELEFONO"))
                        dataRow(1) = row.Item(DAL.TELEFONO.ColumnNames.NUMERO)
                        dataRow(2) = row.Item(DAL.EXTENSION.ColumnNames.EXTENSION)
                        dataRow(3) = nombre                        
                        If (idDepto <> String.Empty) Then dpto = deptoBLL.GetDepartamento(New Sablib.ELL.Departamento With {.Id = idDepto, .IdPlanta = idPlanta})
                        dataRow(4) = If(dpto IsNot Nothing, dpto.Nombre, String.Empty)
                        dataRow(5) = fechaDesde
                        dtResul.Rows.Add(dataRow)
                    End If
                Next

                Return dtResul
            Catch ex As Exception
                Throw New BatzException("errObtenerInfo", ex)
            End Try
        End Function

        ''' <summary>
        ''' Comprueba si la fila cumple las caracteristicas, para estar libre de estado
        ''' </summary>
        ''' <param name="row">Fila con los datos a comprobar</param>
        ''' <returns></returns>        
        Private Function cumpleExtensionLibre(ByVal row As DataRow) As Boolean
			If (row.IsNull(Sablib.ELL.Usuario.ColumnNames.ID)) Then
				Return True
			ElseIf (Not row.IsNull(DAL.EXTENSION_PERSONAS.ColumnNames.F_HASTA)) Then 'El usuario no es nulo y la fecha de hasta es nula
				Return True
			Else
				Return False
			End If
        End Function

        ''' <summary>
        ''' Comprueba si la fila cumple las caracteristicas, para estar ocupado de estado
        ''' </summary>
        ''' <param name="row">Fila con los datos a comprobar</param>
        ''' <returns></returns>
		Private Function cumpleExtensionOcupado(ByVal row As DataRow) As Boolean
			If (Not row.IsNull(Sablib.ELL.Usuario.ColumnNames.ID) And row.IsNull(DAL.EXTENSION_PERSONAS.ColumnNames.F_HASTA)) Then 'el usuario no es nulo y la fecha hasta si
				Return True
			Else
				Return False
			End If
		End Function

        ''' <summary>
        ''' Obtiene el listado de la relacion de los numeros directos, su extension interna y su descripcion
        ''' </summary>
        ''' <returns>Listado de numeros directos</returns>        
        Public Function NumerosDirectos(ByVal idPlanta As Integer) As List(Of ELL.TelefonoExtension)
            Dim tlfnoDAL As New DAL.TELEFONO
            Dim dt As DataTable
            Try
                Dim oTlfnoExt As ELL.TelefonoExtension
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                dt = tlfnoDAL.NumerosDirectos(idPlanta)
                For Each row As DataRow In dt.Rows
                    oTlfnoExt = New ELL.TelefonoExtension
                    oTlfnoExt.TlfnoDirecto = row(DAL.TELEFONO.ColumnNames.NUMERO)
                    If (Not row.IsNull(DAL.EXTENSION.ColumnNames.EXTENSION)) Then oTlfnoExt.ExtensionInterna = row(DAL.EXTENSION.ColumnNames.EXTENSION)
                    If (Not row.IsNull(DAL.EXTENSION.ColumnNames.NOMBRE)) Then oTlfnoExt.Nombre = row(DAL.EXTENSION.ColumnNames.NOMBRE)
                    If (Not row.IsNull(DAL.TELEFONO.ColumnNames.COMENTARIO)) Then oTlfnoExt.Comentarios = row(DAL.TELEFONO.ColumnNames.COMENTARIO)
                    lTlfnoExt.Add(oTlfnoExt)
                Next

                Return lTlfnoExt
            Catch ex As Exception
                Throw New BatzException("errCompListar", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene el listado de la relacion de los moviles, su extension, su relacion y si es visible o no
        ''' </summary>
        ''' <returns>Listado de moviles</returns>        
        Public Function Moviles(ByVal idPlanta As Integer) As List(Of ELL.TelefonoExtension)
            Dim tlfnoDAL As New DAL.TELEFONO
            Dim dt As DataTable
            Try
                Dim oTlfnoExt As ELL.TelefonoExtension
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                dt = tlfnoDAL.Moviles(idPlanta)
                For Each row As DataRow In dt.Rows
                    oTlfnoExt = New ELL.TelefonoExtension
                    oTlfnoExt.TlfnoMovil = row(DAL.TELEFONO.ColumnNames.NUMERO)
                    If (Not row.IsNull(DAL.EXTENSION.ColumnNames.EXTENSION)) Then oTlfnoExt.ExtensionMovil = row(DAL.EXTENSION.ColumnNames.EXTENSION)
                    If (Not row.IsNull("ExtensionInterna")) Then oTlfnoExt.Nombre = row("ExtensionInterna")
                    If (Not row.IsNull(DAL.EXTENSION.ColumnNames.NOMBRE)) Then oTlfnoExt.Nombre = row(DAL.EXTENSION.ColumnNames.NOMBRE)
                    If (Not row.IsNull(DAL.TELEFONO.ColumnNames.COMENTARIO)) Then oTlfnoExt.Comentarios = row(DAL.TELEFONO.ColumnNames.COMENTARIO)
                    If (Not row.IsNull(DAL.EXTENSION.ColumnNames.VISIBLE)) Then oTlfnoExt.Visible = CType(row(DAL.EXTENSION.ColumnNames.VISIBLE), Boolean)
                    lTlfnoExt.Add(oTlfnoExt)
                Next

                Return lTlfnoExt
            Catch ex As Exception
                Throw New BatzException("errCompListar", ex)
            End Try
        End Function


#End Region

#Region "Consultas de telefonos de personas"

        ' ''' <summary>
        ' ''' Obtiene la informacion de los telefonos de una o varias personas
        ' ''' </summary>
        ' ''' <param name="oUser">Usuario</param>
        ' ''' <returns>Lista de objetos de tipo busqueda</returns>        
        'Public Function getTelefonoPersona(ByVal oUser As Sablib.ELL.Usuario) As ELL.TelefonoExtension
        '    Try
        '        Dim oTlfnoExt As New ELL.TelefonoExtension
        '        Dim tlfnoPersoDAL As New DAL.TELEFONO_PERSONAS

        '        oTlfnoExt.Nombre = oUser.NombreCompleto
        '        '1º Se obtiene el telefono activa del usuario
        '        tlfnoPersoDAL.Where.ID_USUARIO.Value = oUser.Id
        '        tlfnoPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
        '        tlfnoPersoDAL.Query.AddOrderBy(DAL.TELEFONO_PERSONAS.ColumnNames.F_DESDE, WhereParameter.Dir.ASC)
        '        tlfnoPersoDAL.Query.Load()

        '        If (tlfnoPersoDAL.RowCount = 1) Then
        '            '2º Se busca el telefono
        '            Dim tlfnoDAL As New DAL.TELEFONO
        '            tlfnoDAL.Where.ID.Value = tlfnoPersoDAL.ID_TLFNO
        '            tlfnoDAL.Query.Load()

        '            If (tlfnoDAL.RowCount = 1) Then
        '                If (tlfnoDAL.FIJO_MOVIL = ELL.Telefono.FijoMovil.fijo) Then
        '                    oTlfnoExt.TlfnoDirecto = tlfnoDAL.NUMERO
        '                Else
        '                    oTlfnoExt.TlfnoMovil = tlfnoDAL.NUMERO
        '                End If
        '                Dim oTlfno As ELL.Telefono = getTelefono(New ELL.Telefono With {.Id = tlfnoPersoDAL.ID_TLFNO})
        '                If (oTlfno IsNot Nothing) Then
        '                    Dim plantComp As New Sablib.BLL.PlantasComponent
        '                    oTlfnoExt.IdPlanta = oTlfno.IdPlanta
        '                    oTlfnoExt.Planta = plantComp.GetPlanta(oTlfno.IdPlanta).Nombre
        '                End If
        '            End If
        '        Else
        '            Return Nothing
        '        End If

        '        Return oTlfnoExt
        '    Catch batzEx As BatzException
        '        Throw batzEx
        '    Catch ex As Exception
        '        Throw New BatzException("errBuscar", ex)
        '    End Try
        'End Function

        ''' <summary>
        ''' Obtiene la informacion de los telefonos de una o varias personas
        ''' </summary>
        ''' <param name="oUser">Usuario</param>
        ''' <returns>Lista de objetos de tipo busqueda</returns>        
        Public Function getTelefonosPersona(ByVal oUser As Sablib.ELL.Usuario) As List(Of ELL.TelefonoExtension)
            Try
                Dim lTlfnoExt As New List(Of ELL.TelefonoExtension)
                Dim oTlfnoExt As ELL.TelefonoExtension
                Dim tlfnoPersoDAL As New DAL.TELEFONO_PERSONAS

                '1º Se obtiene el telefono activa del usuario
                tlfnoPersoDAL.Where.ID_USUARIO.Value = oUser.Id
                tlfnoPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                tlfnoPersoDAL.Query.AddOrderBy(DAL.TELEFONO_PERSONAS.ColumnNames.F_DESDE, WhereParameter.Dir.ASC)
                tlfnoPersoDAL.Query.Load()

                If (tlfnoPersoDAL.RowCount > 0) Then
                    Dim tlfnoDAL As New DAL.TELEFONO
                    Do                        '2º Se busca el telefono
                        tlfnoDAL = New DAL.TELEFONO
                        tlfnoDAL.Where.ID.Value = tlfnoPersoDAL.ID_TLFNO
                        tlfnoDAL.Query.Load()

                        If (tlfnoDAL.RowCount = 1) Then
                            oTlfnoExt = New ELL.TelefonoExtension With {.Nombre = oUser.NombreCompleto}
                            If (tlfnoDAL.FIJO_MOVIL = ELL.Telefono.FijoMovil.fijo) Then
                                oTlfnoExt.TlfnoDirecto = tlfnoDAL.NUMERO
                            Else
                                oTlfnoExt.TlfnoMovil = tlfnoDAL.NUMERO
                            End If
                            Dim oTlfno As ELL.Telefono = getTelefono(New ELL.Telefono With {.Id = tlfnoPersoDAL.ID_TLFNO})
                            If (oTlfno IsNot Nothing) Then
                                Dim plantComp As New Sablib.BLL.PlantasComponent
                                oTlfnoExt.IdPlanta = oTlfno.IdPlanta
                                oTlfnoExt.Planta = plantComp.GetPlanta(oTlfno.IdPlanta).Nombre
                            End If
                            lTlfnoExt.Add(oTlfnoExt)
                        End If
                    Loop While tlfnoPersoDAL.MoveNext
                Else
                    Return Nothing
                End If

                Return lTlfnoExt
            Catch batzEx As BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("errBuscar", ex)
            End Try
        End Function

#End Region

#Region "Asignaciones"

        ''' <summary>
        ''' Delega la llamada a su correspondiente asignacion
        ''' </summary>
        ''' <param name="oTlfno"></param>
        ''' <param name="asig"></param>
        ''' <remarks></remarks>
        Public Sub modificarAsignacion(ByVal oTlfno As ELL.Telefono, ByVal asig As Asignacion, Optional ByVal transaccionAbierta As Boolean = False)
            modificarAsignacionPersona(oTlfno, asig, transaccionAbierta)
        End Sub

        ''' <summary>
        ''' Modifica las asignaciones de los telefonos de la persona
        ''' </summary>
        ''' <param name="oTlfno">Telefono</param>
        ''' <param name="asig">Indicara si habra que asignar,desasignar o reemplazar</param>
        ''' <remarks></remarks>
        Private Sub modificarAsignacionPersona(ByVal oTlfno As ELL.Telefono, ByVal asig As Asignacion, Optional ByVal transaccionAbierta As Boolean = False)
            Dim tx As TransactionMgr = TransactionMgr.ThreadTransactionMgr
            Try
                If (oTlfno.ListaPersonasAsig IsNot Nothing AndAlso oTlfno.ListaPersonasAsig.Count > 0) Then
                    Dim tlfnoPersoDAL As New DAL.TELEFONO_PERSONAS
                    If (Not transaccionAbierta) Then tx.BeginTransaction()

                    If (asig = Asignacion.asignar) Then
                        For Each perso As ELL.TelefonoUsuDep In oTlfno.ListaPersonasAsig
                            tlfnoPersoDAL.Where.ID_TLFNO.Value = perso.IdTelefono
                            tlfnoPersoDAL.Where.ID_USUARIO.Value = perso.IdUsuario
                            tlfnoPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            tlfnoPersoDAL.Query.Load()
                            If (tlfnoPersoDAL.RowCount = 0) Then   'si esta actualmente asignado
                                tlfnoPersoDAL.FlushData()
                                tlfnoPersoDAL.AddNew()
                                tlfnoPersoDAL.ID_USUARIO = perso.IdUsuario
                                If (perso.IdTelefono <> Integer.MinValue) Then
                                    tlfnoPersoDAL.ID_TLFNO = perso.IdTelefono
                                Else
                                    tlfnoPersoDAL.ID_TLFNO = oTlfno.Id
                                End If
                                tlfnoPersoDAL.F_DESDE = Date.Now
                                tlfnoPersoDAL.Save()
                            End If
                        Next
                    ElseIf (asig = Asignacion.desasignar) Then
                        For Each user As ELL.TelefonoUsuDep In oTlfno.ListaPersonasAsig
                            tlfnoPersoDAL.Where.ID_TLFNO.Value = user.IdTelefono
                            tlfnoPersoDAL.Where.ID_USUARIO.Value = user.IdUsuario
                            tlfnoPersoDAL.Where.F_HASTA.Operator = WhereParameter.Operand.IsNull
                            tlfnoPersoDAL.Query.Load()
                            If (tlfnoPersoDAL.RowCount = 1) Then
                                tlfnoPersoDAL.F_HASTA = Now.Date
                                tlfnoPersoDAL.Save()
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

#End Region

#Region "Save"

        ''' <summary>
        ''' Modifica o inserta un telefono
        ''' </summary>
        ''' <param name="oTlfno">Informacion del telefono</param>
        ''' <returns>Booleano indicando si la operacion se ha realizado correctamente</returns>        
        Public Function Save(ByVal oTlfno As ELL.Telefono) As Boolean
            Dim tlfnoDAL As New DAL.TELEFONO
            Dim gestorComp As New GestorTlfnoComponent
            Try

                If (oTlfno.Id = Integer.MinValue) Then
                    tlfnoDAL.AddNew()
                Else
                    tlfnoDAL.LoadByPrimaryKey(oTlfno.Id)
                End If

                If (tlfnoDAL.RowCount = 1) Then
                    tlfnoDAL.ID_PLANTA = oTlfno.IdPlanta
                    tlfnoDAL.NUMERO = oTlfno.Numero
                    tlfnoDAL.FIJO_MOVIL = oTlfno.FijoOMovil
                    If (oTlfno.IdCiaTlfno <> Integer.MinValue) Then
                        tlfnoDAL.ID_CIA_TLFNO = oTlfno.IdCiaTlfno
                    Else
                        tlfnoDAL.s_ID_CIA_TLFNO = String.Empty
                    End If

                    If (oTlfno.EsMovil) Then
                        tlfnoDAL.MODELO = oTlfno.Modelo
                        tlfnoDAL.PIN = oTlfno.PIN
                        tlfnoDAL.PUK = oTlfno.PUK
                        tlfnoDAL.DUALIZADO = oTlfno.Dualizado
                        tlfnoDAL.ROAMING = oTlfno.Roaming
                        If (oTlfno.VozODatos = ELL.Telefono.VozDatos.null) Then
                            tlfnoDAL.s_VOZ_DATOS = String.Empty
                        Else
                            tlfnoDAL.VOZ_DATOS = oTlfno.VozODatos
                        End If

                        If (oTlfno.IdUsuarioGestor <> Integer.MinValue) Then
                            tlfnoDAL.ID_GESTOR = oTlfno.IdUsuarioGestor
                        Else
                            tlfnoDAL.s_ID_GESTOR = String.Empty  'null
                        End If

                        If (oTlfno.IdPerfilMovil <> Integer.MinValue) Then
                            tlfnoDAL.ID_PERFIL_MOV = oTlfno.IdPerfilMovil
                        Else
                            tlfnoDAL.s_ID_PERFIL_MOV = String.Empty 'null
                        End If

                        If (oTlfno.IdTarifaDatos <> Integer.MinValue) Then
                            tlfnoDAL.ID_TARIFA_DATOS = oTlfno.IdTarifaDatos
                        Else
                            tlfnoDAL.s_ID_TARIFA_DATOS = String.Empty 'null
                        End If
                    Else 'Fijo
                        tlfnoDAL.TIPOLINEAFIJO = oTlfno.Tipo_LineaFijo
                    End If

                    tlfnoDAL.COMENTARIO = oTlfno.Comentarios
                    If (oTlfno.FechaAlta <> Date.MinValue) Then tlfnoDAL.F_ALTA = oTlfno.FechaAlta
                    If (oTlfno.FechaBaja <> Date.MinValue) Then tlfnoDAL.F_BAJA = oTlfno.FechaBaja
                    tlfnoDAL.OBSOLETO = oTlfno.Obsoleto

                    tlfnoDAL.Save()

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
        '''' Elimina un telefono
        '''' </summary>
        '''' <param name="idTlfno">Identificador del telefono</param>
        '''' <returns>Booleano indicando si la operacion se ha realizado correctamente</returns>        
        'Public Function Delete(ByVal idTlfno As Integer) As Boolean
        '    Dim tlfnoDAL As New DAL.TELEFONO
        '    Try
        '        tlfnoDAL.LoadByPrimaryKey(idTlfno)
        '        If tlfnoDAL.RowCount = 1 Then
        '            tlfnoDAL.MarkAsDeleted()
        '            tlfnoDAL.Save()
        '            Return True
        '        End If
        '        Return False
        '    Catch ex As Exception
        '        Throw New BatzException("errBorrar", ex)
        '    End Try
        'End Function

#End Region

#Region "Clase de los gestores"

        Public Class GestorTlfnoComponent

            ''' <summary>
            ''' Obtiene un gestor 
            ''' </summary>
            ''' <param name="idPlanta">Identificador del planta</param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function getGestor(ByVal idPlanta As Integer) As ELL.Telefono.GestorTlfno
                Dim gestorDAL As New DAL.GESTOR_TLFNOS
                Dim oGestor As ELL.Telefono.GestorTlfno = Nothing
                Dim genComp As New SABLib.BLL.UsuariosComponent
                Dim plantComp As New SABLib.BLL.PlantasComponent
                Dim ouser As SABLib.ELL.Usuario
                Try                    
                    gestorDAL.Where.ID_PLANTA.Value = idPlanta                    
                    gestorDAL.Query.Load()

                    If (gestorDAL.RowCount > 0) Then
                        oGestor = New ELL.Telefono.GestorTlfno()
                        oGestor.IdUsuarioGestor = gestorDAL.ID_GESTOR
                        ouser = New SABLib.ELL.Usuario
                        ouser.Id = oGestor.IdUsuarioGestor
                        oGestor.UsuarioGestor = genComp.GetUsuario(ouser, False).NombreCompleto
                        oGestor.IdPlanta = gestorDAL.ID_PLANTA
                        oGestor.Planta = plantComp.GetPlanta(oGestor.IdPlanta).Nombre
                    End If
                    Return oGestor
                Catch ex As Exception
                    Throw New BatzException("errObtenerInfo", ex)
                End Try
            End Function


            ''' <summary>
            ''' Obtiene los gestores que de una planta o todos
            ''' </summary>
            ''' <param name="idPlanta">Identificador de la planta</param>
            ''' <returns></returns>            
            Public Function getGestores(ByVal idPlanta As Integer) As System.Collections.Generic.List(Of ELL.Telefono.GestorTlfno)
                Dim gestorDAL As New DAL.GESTOR_TLFNOS
                Dim lGestores As New List(Of ELL.Telefono.GestorTlfno)
                Dim oGestor As ELL.Telefono.GestorTlfno = Nothing
                Dim genComp As New SABLib.BLL.UsuariosComponent
                Dim plantComp As New SABLib.BLL.PlantasComponent
                Dim ouser As SABLib.ELL.Usuario
                Try
                    If (idPlanta <> Integer.MinValue) Then gestorDAL.Where.ID_PLANTA.Value = idPlanta
                    gestorDAL.Query.Load()

                    If (gestorDAL.RowCount > 0) Then
                        Do
                            oGestor = New ELL.Telefono.GestorTlfno()
                            oGestor.IdUsuarioGestor = gestorDAL.ID_GESTOR
                            ouser = New SABLib.ELL.Usuario
                            ouser.Id = oGestor.IdUsuarioGestor
                            oGestor.UsuarioGestor = genComp.GetUsuario(ouser, False).NombreCompleto
                            If (oGestor.UsuarioGestor.Trim <> String.Empty) Then
                                oGestor.IdPlanta = gestorDAL.ID_PLANTA
                                oGestor.Planta = plantComp.GetPlanta(oGestor.IdPlanta).Nombre
                                lGestores.Add(oGestor)
                            End If
                        Loop While gestorDAL.MoveNext

                    End If
                    Return lGestores
                Catch ex As Exception
                    Throw New BatzException("errObtenerInfo", ex)
                End Try
            End Function


            ''' <summary>
            ''' Guarda el nuevo gestor
            ''' </summary>
            ''' <param name="oGestor"></param>
            ''' <returns></returns>
            ''' <remarks></remarks>
            Public Function Save(ByVal oGestor As ELL.Telefono.GestorTlfno) As Boolean
                Dim gestorDAL As New DAL.GESTOR_TLFNOS
                Try
                    gestorDAL.AddNew()
                    gestorDAL.ID_GESTOR = oGestor.IdUsuarioGestor
                    gestorDAL.ID_PLANTA = oGestor.IdPlanta                    

                    gestorDAL.Save()
                    Return True
                Catch ex As Exception
                    Throw New BatzException("errGuardar", ex)
                End Try
            End Function


           
            ''' <summary>
            ''' Elimina la relacion del gestor con el telefono
            ''' </summary>
            ''' <param name="oGestor">Objeto gestor</param>
            ''' <returns>Booleano indicando si la operacion se ha realizado correctamente</returns>
            Public Function Delete(ByVal oGestor As ELL.Telefono.GestorTlfno) As Boolean
                Dim gestorDAL As New DAL.GESTOR_TLFNOS
                Try
                    If (oGestor.IdUsuarioGestor <> Integer.MinValue) Then gestorDAL.Where.ID_GESTOR.Value = oGestor.IdUsuarioGestor
                    If (oGestor.IdPlanta <> Integer.MinValue) Then gestorDAL.Where.ID_PLANTA.Value = oGestor.IdPlanta                    

                    gestorDAL.Query.Load()
                    If gestorDAL.RowCount = 1 Then
                        gestorDAL.MarkAsDeleted()
                        gestorDAL.Save()
                        Return True
                    End If
                    Return False
                Catch ex As Exception
                    Throw New BatzException("errBorrar", ex)
                End Try
            End Function

        End Class

#End Region

#Region "Unificar registros de telefonos"

        ''' <summary>
        ''' Dado un conjunto de extensiones telefonos, unifica los registros de las mismas personas. Se uniran excepto cuando tengan mas de un dato en la misma columna (en la columna Fijo tiene 1505 y en otro registro 1606)
        ''' </summary>
        ''' <param name="lTlfnoExt"></param>
        ''' <returns></returns>
        Public Function UnificarTelefonosExtensiones(ByVal lTlfnoExt As List(Of ELL.TelefonoExtension)) As List(Of String())
            Dim lItems As New List(Of String())
            Dim sItems As String()
            Dim bYaTieneEsaColumna As Boolean
            Dim myTlfnoExt As ELL.TelefonoExtension
            For index As Integer = lTlfnoExt.Count - 1 To 0 Step -1
                myTlfnoExt = lTlfnoExt.Item(index)
                If (lItems.Count = 0) Then
                    lItems.Add(New String() {myTlfnoExt.IdPlanta, myTlfnoExt.idSab, FormatInt(myTlfnoExt.ExtFija), myTlfnoExt.Fijo, FormatInt(myTlfnoExt.ExtInalambrica), myTlfnoExt.Inalambrico, FormatInt(myTlfnoExt.ExtensionMovil), myTlfnoExt.TlfnoMovil, FormatInt(myTlfnoExt.Zoiper), myTlfnoExt.Planta, myTlfnoExt.Nombre, myTlfnoExt.Departamento, myTlfnoExt.IdDepartamento})
                Else
                    bYaTieneEsaColumna = False
                    'Hay que buscar por idPlanta a ver si se puede escribir el valor en la fila existente
                    sItems = lItems.Find(Function(o As String()) o(9) = myTlfnoExt.Planta And String.Compare(o(10), myTlfnoExt.Nombre) = 0)
                    If (sItems Is Nothing OrElse sItems.Length = 0) Then 'es de otra planta, se añade
                        lItems.Add(New String() {myTlfnoExt.IdPlanta, myTlfnoExt.idSab, FormatInt(myTlfnoExt.ExtFija), myTlfnoExt.Fijo, FormatInt(myTlfnoExt.ExtInalambrica), myTlfnoExt.Inalambrico, FormatInt(myTlfnoExt.ExtensionMovil), myTlfnoExt.TlfnoMovil, FormatInt(myTlfnoExt.Zoiper), myTlfnoExt.Planta, myTlfnoExt.Nombre, myTlfnoExt.Departamento, myTlfnoExt.IdDepartamento})
                    Else 'es de la misma planta, hay que unir valores
                        If (FormatInt(myTlfnoExt.ExtFija) <> String.Empty) Then
                            If (sItems(2) <> String.Empty) Then
                                bYaTieneEsaColumna = True
                            Else
                                sItems(2) = FormatInt(myTlfnoExt.ExtFija)
                            End If
                        End If
                        If (Not bYaTieneEsaColumna AndAlso myTlfnoExt.Fijo <> String.Empty) Then
                            If (sItems(3) <> String.Empty) Then
                                bYaTieneEsaColumna = True
                            Else
                                sItems(3) = myTlfnoExt.Fijo
                            End If
                        End If
                        If (Not bYaTieneEsaColumna AndAlso FormatInt(myTlfnoExt.ExtInalambrica) <> String.Empty) Then
                            If (sItems(4) <> String.Empty) Then
                                bYaTieneEsaColumna = True
                            Else
                                sItems(4) = Format(myTlfnoExt.ExtInalambrica)
                            End If
                        End If
                        If Not bYaTieneEsaColumna AndAlso (myTlfnoExt.Inalambrico <> String.Empty) Then
                            If (sItems(5) <> String.Empty) Then
                                bYaTieneEsaColumna = True
                            Else
                                sItems(5) = myTlfnoExt.Inalambrico
                            End If
                        End If
                        If (Not bYaTieneEsaColumna AndAlso FormatInt(myTlfnoExt.ExtensionMovil) <> String.Empty) Then
                            If (sItems(6) <> String.Empty) Then
                                bYaTieneEsaColumna = True
                            Else
                                sItems(6) = FormatInt(myTlfnoExt.ExtensionMovil)
                            End If
                        End If
                        If (Not bYaTieneEsaColumna AndAlso myTlfnoExt.TlfnoMovil <> String.Empty) Then
                            If (sItems(7) <> String.Empty) Then
                                bYaTieneEsaColumna = True
                            Else
                                sItems(7) = myTlfnoExt.TlfnoMovil
                            End If
                        End If
                        If (Not bYaTieneEsaColumna AndAlso FormatInt(myTlfnoExt.Zoiper) <> String.Empty) Then
                            If (sItems(8) <> String.Empty) Then
                                bYaTieneEsaColumna = True
                            Else
                                sItems(8) &= FormatInt(myTlfnoExt.Zoiper)
                            End If
                        End If
                    End If
                    If (bYaTieneEsaColumna) Then  'Si ya tenia una columna rellenada, no se concatenara al valor que tenia sino que creara uno nuevo
                        lItems.Add(New String() {myTlfnoExt.IdPlanta, myTlfnoExt.idSab, FormatInt(myTlfnoExt.ExtFija), myTlfnoExt.Fijo, FormatInt(myTlfnoExt.ExtInalambrica), myTlfnoExt.Inalambrico, FormatInt(myTlfnoExt.ExtensionMovil), myTlfnoExt.TlfnoMovil, FormatInt(myTlfnoExt.Zoiper), myTlfnoExt.Planta, myTlfnoExt.Nombre, myTlfnoExt.Departamento, myTlfnoExt.IdDepartamento})
                    End If
                End If
            Next
            Return lItems
        End Function

        ''' <summary>
        ''' Recibe un entero y si es integer.minvalue, no devolvera nada. En cc, devolvera el numero
        ''' </summary>
        ''' <param name="oInt">Entero</param>
        ''' <returns>String</returns>    
        Protected Function FormatInt(ByVal oInt As String) As String
            If (oInt = String.Empty OrElse (oInt <> String.Empty AndAlso CInt(oInt) = Integer.MinValue)) Then
                Return String.Empty
            Else
                Return oInt.ToString
            End If
        End Function

#End Region

#Region "Prefijos"

        ''' <summary>
        ''' Obtiene el prefijo de una planta
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns></returns>        
        Public Function getPrefijo(ByVal idPlanta As Integer) As String
            Try
                Dim tlfnoDAL As New DAL.TELEFONO
                Return tlfnoDAL.getPrefijo(idPlanta)
            Catch ex As Exception
                Throw New BatzException("Error al obtener el prefijo", ex)
            End Try
        End Function

        ''' <summary>
        ''' Guarda el prefijo
        ''' </summary>
        ''' <param name="idPlanta">Id de la planta</param>        
        Public Sub savePrefijo(ByVal idPlanta As Integer, ByVal prefijo As String)
            Try
                Dim tlfnoDAL As New DAL.TELEFONO
                tlfnoDAL.SavePrefijo(idPlanta, prefijo)
            Catch ex As Exception
                Throw New BatzException("Error al guardar el prefijo", ex)
            End Try
        End Sub

#End Region

#Region "Tarifa de datos"

        ''' <summary>
        ''' Obtiene la informacion de un perfil
        ''' </summary>
        ''' <param name="id">id del perfil</param>
        ''' <returns>Tarifa</returns>        
        Public Function loadTarifa(ByVal id As Integer) As ELL.Telefono.TarifaDatos
            Return tlfnoDAL.loadTarifa(id)
        End Function

        ''' <summary>
        ''' Obtiene una lista de tarifas de una planta
        ''' </summary>
        ''' <param name="bVigentes">Indica si se obtendran los vigentes o todos</param>
        ''' <param name="idPlanta">Id de la planta</param>
        ''' <returns>Lista de tarifas</returns>        
        Public Function loadListTarifas(ByVal bVigentes As Boolean, ByVal idPlanta As Integer) As List(Of ELL.Telefono.TarifaDatos)
            Return tlfnoDAL.loadListTarifas(bVigentes, idPlanta)
        End Function

        ''' <summary>
        ''' Guarda o modifica la informacion de una tarifa
        ''' </summary>
        ''' <param name="oTarifa">Tarifa de datos</param>        
        Public Sub SaveTarifa(ByVal oTarifa As ELL.Telefono.TarifaDatos)
            tlfnoDAL.SaveTarifa(oTarifa)
        End Sub

#End Region

    End Class

End Namespace
