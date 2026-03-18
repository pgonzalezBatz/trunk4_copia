Imports System.Data.Common

Module Principal

    ''' <summary>
    ''' Realiza varias tareas
    ''' </summary>    
    Sub Main()
        'CompararTelefonosExcel()
        'AsignarMismoPerfil()
        AsignarTarifaDatos()
    End Sub

    ''' <summary>
    ''' Dado un excel con la estructura: Tlfno - Extension - Perfil
    ''' Compara que los telefonos existan en la aplicacion, que tengan esa extension y ese perfil
    ''' Tambien compara que los telefonos de la aplicacion existan en el excel
    ''' </summary>
    Private Sub CompararTelefonosExcel()
        Dim conexionExcel As ADODB.Connection = Nothing
        Dim rs As ADODB.Recordset = Nothing
        Dim num As Integer = 1
        Dim bError As Boolean = False
        Dim hoja, telefonoExcel, extExcel, perfilExcel, telefonoWeb, extWeb, perfilWeb, fileExcel As String
        Try
            fileExcel = IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "telefonos.xls")
            conexionExcel = New ADODB.Connection
            'HDR=Yes => para que la primera fila, no la lea ya que son las cabeceras
            'IMAX=1  => para que todos los valores de las columnas, las lea como texto. Sino se indica, suele tomar como tipo de datos el de los primeros valores. Si los primeros valores, son textos, pero hay uno de numeros, este ultimo devolvera blanco
            Dim extension As String = fileExcel.Substring(fileExcel.LastIndexOf(".") + 1).ToLower
            Dim excelConection As String = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=""Excel {3};HDR=YES;IMEX=1;"""
            If (extension = "xls") Then
                conexionExcel.Open(String.Format(excelConection, "Jet", "4.0", fileExcel, "8.0"))
            ElseIf (extension = "xlsx") Then
                conexionExcel.Open(String.Format(excelConection, "ACE", "12.0", fileExcel, "12.0"))
            End If

            ' Nuevo recordset  
            rs = New ADODB.Recordset
            With rs
                .CursorLocation = ADODB.CursorLocationEnum.adUseClient
                .CursorType = ADODB.CursorTypeEnum.adOpenStatic
                .LockType = ADODB.LockTypeEnum.adLockOptimistic
            End With

            hoja = GetExcelSheetNames(fileExcel)(0)
            rs.Open("SELECT * FROM [" & hoja & "]", conexionExcel, , )

            Dim tlfnoBLL As New TelefoniaLib.BLL.TelefonoComponent
            Dim extBLL As New TelefoniaLib.BLL.ExtensionComponent
            Dim perfilBLL As New TelefoniaLib.BLL.PerfilMovComponent
            Dim perfil As TelefoniaLib.ELL.PerfilMovil
            Dim tlfno As TelefoniaLib.ELL.Telefono
            Dim extens As TelefoniaLib.ELL.Extension
            Dim lTelefonosFinal As New List(Of String())
            While Not rs.EOF
                telefonoExcel = rs(0).Value
                extExcel = rs(1).Value
                perfilExcel = rs(2).Value
                'Se busca si existe el telefono
                tlfno = tlfnoBLL.getTelefono(New TelefoniaLib.ELL.Telefono With {.Numero = telefonoExcel, .IdPlanta = 1})
                telefonoWeb = String.Empty : extWeb = String.Empty : perfilWeb = String.Empty
                If (tlfno IsNot Nothing) Then
                    telefonoWeb = tlfno.Numero
                    'Se busca la extension asociada al telefono
                    extens = extBLL.getExtension(New TelefoniaLib.ELL.Extension With {.IdTelefono = tlfno.Id}, 1, True, False)
                    If (extens IsNot Nothing) Then extWeb = extens.Extension
                    'Si el telefono tiene asignado un perfil, se busca
                    If (tlfno.IdPerfilMovil > 0) Then
                        perfil = perfilBLL.load(tlfno.IdPerfilMovil)
                        If (perfil IsNot Nothing) Then perfilWeb = perfil.Nombre
                    End If
                End If
                lTelefonosFinal.Add(New String() {telefonoExcel, extExcel, perfilExcel, telefonoWeb, extWeb, perfilWeb})
                rs.MoveNext()
            End While

            'Habria que buscar si alguna que existe en la Web, no existe en el excel
            Dim lTelefonosIgorre As List(Of TelefoniaLib.ELL.Telefono) = tlfnoBLL.getTelefonos(New TelefoniaLib.ELL.Telefono With {.IdPlanta = 1, .FijoOMovil = TelefoniaLib.ELL.Telefono.FijoMovil.movil})
            For Each oTlfno In lTelefonosIgorre
                If Not (lTelefonosFinal.Exists(Function(o As String()) o(0) = oTlfno.Numero)) Then
                    telefonoWeb = String.Empty : extWeb = String.Empty : perfilWeb = String.Empty
                    telefonoWeb = oTlfno.Numero
                    'Se busca la extension asociada al telefono
                    extens = extBLL.getExtension(New TelefoniaLib.ELL.Extension With {.IdTelefono = oTlfno.Id}, 1, True, False)
                    If (extens IsNot Nothing) Then extWeb = extens.Extension
                    'Si el telefono tiene asignado un perfil, se busca
                    If (oTlfno.IdPerfilMovil > 0) Then
                        perfil = perfilBLL.load(oTlfno.IdPerfilMovil)
                        If (perfil IsNot Nothing) Then perfilWeb = perfil.Nombre
                    End If
                    lTelefonosFinal.Add(New String() {"", "", "", telefonoWeb, extWeb, perfilWeb})
                End If
            Next
            If (rs IsNot Nothing) Then
                rs.ActiveConnection = Nothing
                If (rs.State <> 0) Then rs.Close()
                conexionExcel.Close()
            End If
            WriteExcel("Resultado", lTelefonosFinal)            
        Catch ex As Exception
            bError = True
            Throw New Exception("Error al comparar los telefonos del excel", ex)
        Finally
            If (bError AndAlso rs IsNot Nothing) Then
                rs.ActiveConnection = Nothing
                If (rs.State <> 0) Then rs.Close()
                conexionExcel.Close()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Escribe una nueva hoja en el excel
    ''' </summary>
    ''' <param name="sheetName">Nombre de la hoja a crear</param>
    ''' <param name="lTelefonos">Lista de telefonos</param>    
    Private Sub WriteExcel(sheetName As String, lTelefonos As List(Of String()))
        Dim connection As DbConnection = Nothing
        Try            
            Dim connectionString As String = "Provider=Microsoft.Jet.OLEDB.4.0;Data Source=" & IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "telefonos.xls") & ";Extended Properties=""Excel 8.0;HDR=NO;"""
            Dim factory As DbProviderFactory = DbProviderFactories.GetFactory("System.Data.OleDb")
            connection = factory.CreateConnection
            connection.ConnectionString = connectionString
            Dim command As DbCommand = connection.CreateCommand()
            connection.Open()

            command.CommandText = "CREATE TABLE [" & sheetName & "] (TLFNO char(10), EXT char(4), PERFIL char(50),TLFNO_WEB char(10), EXT_WEB char(4), PERFIL_WEB char(50),MISMA_EXT char(2),MISMO_PERFIL char(2))"
            command.ExecuteNonQuery()
            Dim bMismaExt, bMismaPerfil As Boolean
            For Each sTlfno As String() In lTelefonos
                bMismaExt = (sTlfno(1) = sTlfno(4)) : bMismaPerfil = (sTlfno(2) = sTlfno(5))
                command.CommandText = "INSERT INTO [" & sheetName & "] VALUES(""" & sTlfno(0) & """,""" & sTlfno(1) & """,""" & sTlfno(2) & """,""" & sTlfno(3) & """,""" & sTlfno(4) & """,""" & sTlfno(5) & """,""" & If(bMismaExt, "SI", "NO") & """,""" & If(bMismaPerfil, "SI", "NO") & """)"
                command.ExecuteNonQuery()
            Next
        Catch ex As Exception
            Console.WriteLine("Error al comparar los telefonos. Ex:" & ex.ToString)
        Finally
            If (connection IsNot Nothing) Then
                connection.Close() : connection.Dispose()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Obtiene los nombres de las hojas del excel
    ''' </summary>
    ''' <param name="excelFile">Fichero excel</param>
    ''' <returns></returns>        
    Private Function GetExcelSheetNames(ByVal excelFile As String) As String()
        Dim objConn As OleDb.OleDbConnection = Nothing
        Dim dt As Data.DataTable = Nothing
        Try
            Dim extension As String = excelFile.Substring(excelFile.LastIndexOf(".") + 1).ToLower
            Dim connString As String = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=""Excel {3};HDR=YES;IMEX=1;"""
            If (extension = "xls") Then
                connString = String.Format(connString, "Jet", "4.0", excelFile, "8.0")
            ElseIf (extension = "xlsx") Then
                connString = String.Format(connString, "ACE", "12.0", excelFile, "12.0")
            End If

            objConn = New OleDb.OleDbConnection(connString)
            objConn.Open()

            dt = objConn.GetOleDbSchemaTable(OleDb.OleDbSchemaGuid.Tables, Nothing)
            If (dt Is Nothing) Then Return Nothing
            Dim excelSheets(dt.Rows.Count - 1) As String
            Dim index As Integer = 0
            For Each row As DataRow In dt.Rows
                excelSheets(index) = row("TABLE_NAME").ToString()
                index += 1
            Next

            Return excelSheets
        Catch ex As Exception
            Throw New Exception("Error al consultar el nombre de las hojas del excel (" & excelFile & ")", ex)
        Finally
            If (objConn IsNot Nothing) Then
                objConn.Close()
                objConn.Dispose()
            End If
            If (dt IsNot Nothing) Then dt.Dispose()
        End Try
    End Function

    ''' <summary>
    ''' Dado un excel con la estructura: Tlfno - Extension - Perfil - TlfnoWeb - ExtensionWeb - PerfilWeb
    ''' Asigna el primer perfil al telefono    
    ''' </summary>
    Private Sub AsignarMismoPerfil()
        Dim conexionExcel As ADODB.Connection = Nothing
        Dim rs As ADODB.Recordset = Nothing
        Dim num As Integer = 1
        Dim bError As Boolean = False
        Dim hoja, telefonoExcel, extExcel, perfilExcel, telefonoWeb, extWeb, perfilWeb, fileExcel As String
        Try
            fileExcel = IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "telefonosAsig.xls")
            conexionExcel = New ADODB.Connection
            'HDR=Yes => para que la primera fila, no la lea ya que son las cabeceras
            'IMAX=1  => para que todos los valores de las columnas, las lea como texto. Sino se indica, suele tomar como tipo de datos el de los primeros valores. Si los primeros valores, son textos, pero hay uno de numeros, este ultimo devolvera blanco
            Dim extension As String = fileExcel.Substring(fileExcel.LastIndexOf(".") + 1).ToLower
            Dim excelConection As String = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=""Excel {3};HDR=YES;IMEX=1;"""
            If (extension = "xls") Then
                conexionExcel.Open(String.Format(excelConection, "Jet", "4.0", fileExcel, "8.0"))
            ElseIf (extension = "xlsx") Then
                conexionExcel.Open(String.Format(excelConection, "ACE", "12.0", fileExcel, "12.0"))
            End If

            ' Nuevo recordset  
            rs = New ADODB.Recordset
            With rs
                .CursorLocation = ADODB.CursorLocationEnum.adUseClient
                .CursorType = ADODB.CursorTypeEnum.adOpenStatic
                .LockType = ADODB.LockTypeEnum.adLockOptimistic
            End With

            hoja = GetExcelSheetNames(fileExcel)(0)
            rs.Open("SELECT * FROM [" & hoja & "]", conexionExcel, , )

            Dim tlfnoBLL As New TelefoniaLib.BLL.TelefonoComponent
            Dim tlfno As TelefoniaLib.ELL.Telefono
            Dim lTelefonosFinal As New List(Of String())
            While Not rs.EOF
                telefonoExcel = rs(0).Value
                extExcel = rs(1).Value
                perfilExcel = rs(2).Value
                perfilWeb = If(Not IsDBNull(rs(5).Value), rs(5).Value, String.Empty)
                If (perfilExcel <> perfilWeb) Then
                    'Se busca si existe el telefono
                    tlfno = tlfnoBLL.getTelefono(New TelefoniaLib.ELL.Telefono With {.Numero = telefonoExcel, .IdPlanta = 1})
                    telefonoWeb = String.Empty : extWeb = String.Empty : perfilWeb = String.Empty
                    If (tlfno IsNot Nothing) Then
                        Select Case perfilExcel.ToUpper
                            Case "INTERNACIONAL"
                                tlfno.IdPerfilMovil = 23
                            Case "INTERNACIONAL PLUS"
                                tlfno.IdPerfilMovil = 24
                            Case "GERENTES"
                                tlfno.IdPerfilMovil = 61
                            Case "NACIONAL"
                                tlfno.IdPerfilMovil = 22
                            Case "SOLO BATZ"
                                tlfno.IdPerfilMovil = 21
                        End Select
                        tlfnoBLL.Save(tlfno)
                    End If
                End If
                rs.MoveNext()
            End While

            If (rs IsNot Nothing) Then
                rs.ActiveConnection = Nothing
                If (rs.State <> 0) Then rs.Close()
                conexionExcel.Close()
            End If
        Catch ex As Exception
            bError = True
            Throw New Exception("Error al asignar los perfiles del excel", ex)
        Finally
            If (bError AndAlso rs IsNot Nothing) Then
                rs.ActiveConnection = Nothing
                If (rs.State <> 0) Then rs.Close()
                conexionExcel.Close()
            End If
        End Try
    End Sub

    ''' <summary>
    ''' Dado un excel con la estructura: Tlfno - Extension - Tarifa
    ''' Asigna la tarifa al telefono
    ''' </summary>
    Private Sub AsignarTarifaDatos()
        Dim conexionExcel As ADODB.Connection = Nothing
        Dim rs As ADODB.Recordset = Nothing
        Dim num As Integer = 1
        Dim bError As Boolean = False
        Dim hoja, fileExcel, telefono, ext, tarifa As String
        Try
            fileExcel = IO.Path.Combine(IO.Directory.GetCurrentDirectory(), "telefonosTarifaDatos.xls")
            conexionExcel = New ADODB.Connection
            'HDR=Yes => para que la primera fila, no la lea ya que son las cabeceras
            'IMAX=1  => para que todos los valores de las columnas, las lea como texto. Sino se indica, suele tomar como tipo de datos el de los primeros valores. Si los primeros valores, son textos, pero hay uno de numeros, este ultimo devolvera blanco
            Dim extension As String = fileExcel.Substring(fileExcel.LastIndexOf(".") + 1).ToLower
            Dim excelConection As String = "Provider=Microsoft.{0}.OLEDB.{1};Data Source={2};Extended Properties=""Excel {3};HDR=YES;IMEX=1;"""
            If (extension = "xls") Then
                conexionExcel.Open(String.Format(excelConection, "Jet", "4.0", fileExcel, "8.0"))
            ElseIf (extension = "xlsx") Then
                conexionExcel.Open(String.Format(excelConection, "ACE", "12.0", fileExcel, "12.0"))
            End If

            ' Nuevo recordset  
            rs = New ADODB.Recordset
            With rs
                .CursorLocation = ADODB.CursorLocationEnum.adUseClient
                .CursorType = ADODB.CursorTypeEnum.adOpenStatic
                .LockType = ADODB.LockTypeEnum.adLockOptimistic
            End With

            hoja = GetExcelSheetNames(fileExcel)(0)
            rs.Open("SELECT * FROM [" & hoja & "]", conexionExcel, , )

            Dim tlfnoBLL As New TelefoniaLib.BLL.TelefonoComponent
            Dim tlfno As TelefoniaLib.ELL.Telefono
            Dim lTelefonosFinal As New List(Of String())
            While Not rs.EOF
                telefono = rs(0).Value
                ext = rs(1).Value
                tarifa = rs(2).Value
                tlfno = tlfnoBLL.getTelefono(New TelefoniaLib.ELL.Telefono With {.Numero = telefono, .IdPlanta = 1})
                If (tlfno IsNot Nothing) Then
                    Select Case tarifa.ToUpper
                        Case "CORP12"
                            tlfno.IdTarifaDatos = 1
                        Case "CORP19"
                            tlfno.IdTarifaDatos = 2
                        Case "CORP25"
                            tlfno.IdTarifaDatos = 3
                    End Select
                    tlfnoBLL.Save(tlfno)
                End If
                rs.MoveNext()
            End While

            If (rs IsNot Nothing) Then
                rs.ActiveConnection = Nothing
                If (rs.State <> 0) Then rs.Close()
                conexionExcel.Close()
            End If
        Catch ex As Exception
            bError = True
            Throw New Exception("Error al asignar las tarifas del excel", ex)
        Finally
            If (bError AndAlso rs IsNot Nothing) Then
                rs.ActiveConnection = Nothing
                If (rs.State <> 0) Then rs.Close()
                conexionExcel.Close()
            End If
        End Try
    End Sub

End Module
