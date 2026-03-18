Namespace DAL

    Public Class MatriciDAL

#Region "Variables/Constructor"

        Private cn As String
        Private parameter As OracleParameter

        ''' <summary>
        ''' Constructor
        ''' </summary>        
        Sub New()
            Dim status As String = "TELEFONIATEST"
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus").ToLower = "live") Then status = "TELEFONIALIVE"
            cn = Configuration.ConfigurationManager.ConnectionStrings(status).ConnectionString
        End Sub

#End Region


        ''' <summary>
        ''' Importa los telefonos de Matrici
        ''' </summary>
        ''' <param name="guiaTelefonos">Telefonos</param>      
        Public Sub ImportarMatrici(ByVal guiaTelefonos As List(Of String()))
            Dim con As OracleConnection = Nothing
            Dim transact As OracleTransaction = Nothing
            Dim numLineas As Integer = 0
            Dim guia As String()
            Try
                con = New OracleConnection(cn)
                con.Open()
                transact = con.BeginTransaction()

                '1º Se borran los registros existentes
                Dim query As String = "DELETE FROM TLFNOS_MATRICI"
                Memcached.OracleDirectAccess.NoQuery(query, con)

                'Se insertan los telefonos                
                query = "INSERT INTO TLFNOS_MATRICI(NOMBRE,APELLIDOS,UNIDAD,AREA,SECCION,EXT_FIJA,FIJO,EXT_INALAMBRICA,INALAMBRICO,EXT_MOVIL,MOVIL,SKYPE) VALUES " _
                        & "(:NOMBRE,:APELLIDOS,:UNIDAD,:AREA,:SECCION,:EXT_FIJA,:FIJO,:EXT_INALAMBRICA,:INALAMBRICO,:EXT_MOVIL,:MOVIL,:SKYPE)"

                'apellidos|nombre|unidad|area|seccion|extFija|extInalambrica|directoFijo|directoInal|movil|mov_codigo|skype_code                       
                For Each guia In guiaTelefonos
                    Dim parameters(11) As OracleParameter
                    parameters(0) = New OracleParameter("NOMBRE", OracleDbType.Varchar2, guia(1), ParameterDirection.Input)
                    parameters(1) = New OracleParameter("APELLIDOS", OracleDbType.Varchar2, guia(0), ParameterDirection.Input)
                    parameters(2) = New OracleParameter("UNIDAD", OracleDbType.Varchar2, guia(2), ParameterDirection.Input)
                    parameters(3) = New OracleParameter("AREA", OracleDbType.Varchar2, sqlStringNull(guia(3)), ParameterDirection.Input)
                    parameters(4) = New OracleParameter("SECCION", OracleDbType.Varchar2, sqlStringNull(guia(4)), ParameterDirection.Input)
                    parameters(5) = New OracleParameter("EXT_FIJA", OracleDbType.Int32, sqlStringNull(guia(5)), ParameterDirection.Input)
                    parameters(6) = New OracleParameter("FIJO", OracleDbType.Varchar2, sqlStringNull(guia(7)), ParameterDirection.Input)
                    parameters(7) = New OracleParameter("EXT_INALAMBRICA", OracleDbType.Int32, sqlStringNull(guia(6)), ParameterDirection.Input)
                    parameters(8) = New OracleParameter("INALAMBRICO", OracleDbType.Varchar2, sqlStringNull(guia(8)), ParameterDirection.Input)
                    parameters(9) = New OracleParameter("EXT_MOVIL", OracleDbType.Int32, sqlStringNull(guia(10)), ParameterDirection.Input)
                    parameters(10) = New OracleParameter("MOVIL", OracleDbType.Varchar2, sqlStringNull(guia(9)), ParameterDirection.Input)
                    parameters(11) = New OracleParameter("SKYPE", OracleDbType.Int32, sqlStringNull(guia(11)), ParameterDirection.Input)

                    Memcached.OracleDirectAccess.NoQuery(query, con, parameters)
                    numLineas += 1
                Next

                transact.Commit()
            Catch batzEx As SabLib.BatzException
                transact.Rollback()
            Catch ex As Exception
                transact.Rollback()
                Throw New SabLib.BatzException("Error al importar", ex)
            Finally
                If (con IsNot Nothing AndAlso con.State <> ConnectionState.Closed) Then
                    con.Close()
                    con.Dispose()
                End If
            End Try
        End Sub

        ''' <summary>
        ''' Obtiene el nombre y apellidos
        ''' </summary>
        ''' <param name="persona">Informacion con el nombre y apellidos que viene en el csv</param>
        ''' <param name="nombre">Nombre resultante</param>
        ''' <param name="apellidos">Apellidos resultantes</param>        
        Private Sub getNombreApellidos(ByVal persona As String, ByRef nombre As String, ByRef apellidos As String)
            Dim nombreAp As String() = persona.Split(",")
            If (nombreAp.Length = 2) Then
                nombre = nombreAp(1).Trim
                apellidos = nombreAp(0).Trim
            Else 'Hay algunos que no tienen coma
                nombreAp = persona.Split(" ")
                apellidos = nombreAp(0).Trim
                For index As Integer = 1 To nombreAp.Length - 1
                    If (nombre <> String.Empty) Then nombre &= " "
                    nombre = nombreAp(index).Trim
                Next
            End If
        End Sub

        ''' <summary>
        ''' Obtiene todos los usuarios de Matrici
        ''' </summary>        
        Public Function GetUsuariosMatrici() As List(Of ELL.Matrici)
            Dim query As String = "SELECT ID,NOMBRE,APELLIDOS FROM TLFNOS_MATRICI ORDER BY NOMBRE"
            Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Matrici)(Function(r As OracleDataReader) New ELL.Matrici With {.Id = CInt(r(0)), .Nombre = BLL.Utils.stringNull(r(1)), .Apellidos = BLL.Utils.stringNull(r(2))}, query, Me.cn)
        End Function

        ''' <summary>
        ''' Obtiene todos los departamentos de Matrici
        ''' </summary>        
        Public Function GetDepartamentosMatrici() As List(Of String())
            Return Memcached.OracleDirectAccess.Seleccionar("SELECT DISTINCT AREA FROM TLFNOS_MATRICI ORDER BY AREA", cn)
        End Function

        ''' <summary>
        ''' Dada un id de usuario o un departamento, busca la informacion de un registro de Matrici
        ''' </summary>        
        ''' <param name="OrderByPersonas">Ordenado por personas o por departamentos</param>
        Public Function GetInfoMatrici(ByVal idUser As Integer, ByVal departamento As String, ByVal OrderByPersonas As Boolean) As List(Of ELL.Matrici)
            Dim query As String = "SELECT * FROM TLFNOS_MATRICI"
            Dim where As String = String.Empty
            parameter = Nothing
            If (idUser <> Integer.MinValue) Then
                parameter = New OracleParameter("ID", OracleDbType.Int32, idUser, ParameterDirection.Input)
                where &= "ID=:ID"
            ElseIf (departamento <> String.Empty) Then
                parameter = New OracleParameter("DEPART", OracleDbType.Varchar2, departamento, ParameterDirection.Input)
                If (where <> String.Empty) Then where &= " AND "
                where &= "AREA=:DEPART"
            End If

            If (where <> String.Empty) Then query &= " WHERE " & where

            query &= " ORDER BY "
            If (OrderByPersonas) Then
                query &= "NOMBRE,APELLIDOS"
            Else
                query &= "AREA,NOMBRE,APELLIDOS"
            End If

            If (parameter Is Nothing) Then
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Matrici) _
                    (Function(r As OracleDataReader) New ELL.Matrici With {.Id = CInt(r(0)), .Nombre = BLL.Utils.stringNull(r(1)), .Apellidos = BLL.Utils.stringNull(r(2)), .Unidad = BLL.Utils.stringNull(r(3)),
                                                    .Area = BLL.Utils.stringNull(r(4)), .Seccion = BLL.Utils.stringNull(r(5)), .ExtFija = BLL.Utils.integerNull(r(6)), .Fijo = BLL.Utils.stringNull(r(7)),
                                                    .ExtInalambrica = BLL.Utils.integerNull(r(8)), .Inalambrico = BLL.Utils.stringNull(r(9)), .ExtMovil = BLL.Utils.integerNull(r(10)),
                                                    .Movil = BLL.Utils.stringNull(r(11)), .Skype = BLL.Utils.integerNull(r(12))}, query, Me.cn)
            Else
                Return Memcached.OracleDirectAccess.seleccionar(Of ELL.Matrici) _
                    (Function(r As OracleDataReader) New ELL.Matrici With {.Id = CInt(r(0)), .Nombre = BLL.Utils.stringNull(r(1)), .Apellidos = BLL.Utils.stringNull(r(2)), .Unidad = BLL.Utils.stringNull(r(3)),
                                                    .Area = BLL.Utils.stringNull(r(4)), .Seccion = BLL.Utils.stringNull(r(5)), .ExtFija = BLL.Utils.integerNull(r(6)), .Fijo = BLL.Utils.stringNull(r(7)),
                                                    .ExtInalambrica = BLL.Utils.integerNull(r(8)), .Inalambrico = BLL.Utils.stringNull(r(9)), .ExtMovil = BLL.Utils.integerNull(r(10)),
                                                    .Movil = BLL.Utils.stringNull(r(11)), .Skype = BLL.Utils.integerNull(r(12))}, query, Me.cn, parameter)
            End If
        End Function

        ''' <summary>
        ''' Devuelve un null o el string del objeto dependiendo si esta en blanco o no
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function sqlStringNull(ByVal obj As String)
            If (obj = String.Empty) Then
                Return DBNull.Value
            Else
                Return obj.ToString
            End If
        End Function

        ''' <summary>
        ''' Devuelve un null o el string del objeto dependiendo si esta en blanco o no
        ''' </summary>
        ''' <param name="obj"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function sqlIntegerNull(ByVal obj As Integer)
            If (obj < 0) Then
                Return DBNull.Value
            Else
                Return CInt(obj.ToString)
            End If
        End Function

    End Class

End Namespace