Imports AccesoAutomaticoBD
Namespace BLL
    Public Class gtkIrudiakComponent

		Private Log As log4net.ILog = log4net.LogManager.GetLogger("root.gtkIrudiakComponent")

        ''' <summary>
        ''' Consultamos las imagenes asociadas a la incidencia.
        ''' </summary>
        ''' <param name="gtkIrudia">Objeto de Imagen.</param>
        ''' <param name="Bytes">Indicamos si queremos que nos devuelva los Bytes de la imagen.</param>
        ''' <returns>Devuleve un objeto de imagen.</returns>
        Public Overloads Function Consultar(ByVal gtkIrudia As ELL.gtkIrudiak, Optional ByVal Bytes As Boolean = False) As List(Of ELL.gtkIrudiak)
            Return ConsultarIrudiak(gtkIrudia, Bytes)
        End Function
        ''' <summary>
        ''' Consultamos las imagenes asociadas a la incidencia.
        ''' </summary>
        ''' <param name="IdImagen">Identificador de la imagen</param>
        ''' <param name="Bytes">Indicamos si queremos que nos devuelva los Bytes de la imagen.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Overloads Function Consultar(ByVal IdImagen As Integer, Optional ByVal Bytes As Boolean = False) As List(Of ELL.gtkIrudiak)
            Dim gtkIrudia As New ELL.gtkIrudiak
            gtkIrudia.IdImagen = IdImagen
            Return ConsultarIrudiak(gtkIrudia, Bytes)
        End Function
        ''' <summary>
        ''' Consultamos las imagenes asociadas a la incidencia.
        ''' </summary>
        ''' <param name="gtkIrudia">Objeto de Imagen.</param>
        ''' <param name="Bytes">Indicamos si queremos que nos devuelva los Bytes de la imagen.</param>
        ''' <returns>Devuleve un objeto de imagen.</returns>
        Private Function ConsultarIrudiak(ByVal gtkIrudia As ELL.gtkIrudiak, Optional ByVal Bytes As Boolean = False) As List(Of ELL.gtkIrudiak)
            Dim lstIrudiak As New List(Of GertakariakLib.ELL.gtkIrudiak)
            Dim dbGtkIrudiak As New DAL.GERTAKARIAK_IRUDIAK

            If gtkIrudia.IdImagen <> Integer.MinValue Then dbGtkIrudiak.Where.ID.Value = gtkIrudia.IdImagen
            If gtkIrudia.IdIncidencia <> Integer.MinValue Then dbGtkIrudiak.Where.IDINCIDENCIA.Value = gtkIrudia.IdIncidencia

            dbGtkIrudiak.Query.Load()
            If Not dbGtkIrudiak.EOF Then
                Do
                    Dim nIrudia As New ELL.gtkIrudiak
                    nIrudia.IdImagen = dbGtkIrudiak.ID
                    nIrudia.IdIncidencia = dbGtkIrudiak.IDINCIDENCIA
                    nIrudia.Descripcion = dbGtkIrudiak.DESCRIPCION
                    nIrudia.Extension = dbGtkIrudiak.EXTENSION
                    lstIrudiak.Add(fgIrudiak(ELL.Acciones.Consultar, nIrudia, Bytes))
                Loop While dbGtkIrudiak.MoveNext
            End If

            Return lstIrudiak
        End Function
        ''' <summary>
        ''' Funcion para la asignacion de imagenes a las incidencias.
        ''' </summary>
        ''' <param name="gtkIrudia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Insertar(ByVal gtkIrudia As ELL.gtkIrudiak) As ELL.gtkIrudiak
            Return fgIrudiak(ELL.Acciones.Insertar, gtkIrudia)
        End Function
        ''' <summary>
        ''' Funcion para la modificacion de imagenes asignadas a las incidencias.
        ''' </summary>
        ''' <param name="gtkIrudia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Modificar(ByVal gtkIrudia As ELL.gtkIrudiak) As ELL.gtkIrudiak
            Return fgIrudiak(ELL.Acciones.Modificar, gtkIrudia)
        End Function
        ''' <summary>
        ''' Funcion para la eliminacion de las imagenes de una incidencia.
        ''' </summary>
        ''' <param name="gtkIrudia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Borrar(ByVal gtkIrudia As ELL.gtkIrudiak) As ELL.gtkIrudiak
            Return fgIrudiak(ELL.Acciones.Borrar, gtkIrudia)
        End Function

        ''' <summary>
        ''' Funcion general para realizar acciones con las imagenes.
        ''' </summary>
        ''' <param name="Accion"></param>
        ''' <param name="gtkIrudia"></param>
        ''' <param name="Bytes">Indicamos si queremos que nos devuelva los Bytes de la imagen.</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Private Function fgIrudiak(ByVal Accion As ELL.Acciones, ByVal gtkIrudia As ELL.gtkIrudiak, Optional ByVal Bytes As Boolean = False) As ELL.gtkIrudiak
            Dim dbGtkIrudiak As New GertakariakLib.DAL.GERTAKARIAK_IRUDIAK  'Tabla de Imagenes (DataBase - db)
            'Dim Irudiak As List(Of ELL.gtkIrudiak) = Nothing
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
                'Tabla de Gertakariak
                '-----------------------------------------------------------------
                Select Case Accion
                    Case ELL.Acciones.Insertar
                        'Para hacer insert
                        dbGtkIrudiak.AddNew()
                    Case ELL.Acciones.Modificar, ELL.Acciones.Borrar, ELL.Acciones.Consultar
                        'Cargamos el registro con el que vamos a trabajar.
                        dbGtkIrudiak.Where.IDINCIDENCIA.Value = gtkIrudia.IdIncidencia
                        dbGtkIrudiak.Where.ID.Value = gtkIrudia.IdImagen
                        dbGtkIrudiak.Query.Load()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        '---------------------------------------------------------------------------
                        'Campos a Insertar o Modificar
                        '---------------------------------------------------------------------------
                        If Not gtkIrudia.Imagen Is Nothing Then dbGtkIrudiak.IMAGEN = gtkIrudia.Imagen
                        If gtkIrudia.IdIncidencia <> Integer.MinValue Then dbGtkIrudiak.IDINCIDENCIA = gtkIrudia.IdIncidencia
                        If Not gtkIrudia.Descripcion Is Nothing Then dbGtkIrudiak.DESCRIPCION = gtkIrudia.Descripcion
                        If Not gtkIrudia.Extension Is Nothing Then dbGtkIrudiak.EXTENSION = gtkIrudia.Extension
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Consultar
                        '---------------------------------------------------------------------------
                        'Campos a devolver en la consulta
                        '---------------------------------------------------------------------------
                        If Not dbGtkIrudiak.EOF Then
                            Do
                                gtkIrudia.IdImagen = dbGtkIrudiak.ID
                                gtkIrudia.IdIncidencia = dbGtkIrudiak.IDINCIDENCIA
                                If Bytes = True Then gtkIrudia.Imagen = dbGtkIrudiak.IMAGEN
                                gtkIrudia.Descripcion = dbGtkIrudiak.DESCRIPCION
                                gtkIrudia.Accion = ELL.Acciones.Consultar
                                gtkIrudia.Extension = dbGtkIrudiak.EXTENSION
                            Loop While dbGtkIrudiak.MoveNext
                        End If
                        '---------------------------------------------------------------------------
                    Case ELL.Acciones.Borrar
                        dbGtkIrudiak.DeleteAll()
                End Select
                Select Case Accion
                    Case ELL.Acciones.Insertar, ELL.Acciones.Modificar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbGtkIrudiak.Save()
                        gtkIrudia.IdImagen = dbGtkIrudiak.ID 'Cambiar esta linea por la llamada a la funcion que nos devuelve un Gertakaria.
                    Case ELL.Acciones.Consultar
                        'No se realiza ninguna accion.
                    Case ELL.Acciones.Borrar
                        'Realizamos la accion correspondiente (Insertar, Modificar)
                        dbGtkIrudiak.Save()
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
                Return gtkIrudia
			Catch ex As Exception
				Log.Error(ex)
				'-----------------------------------------------------------------
				'Fin de la transaccion
				'-----------------------------------------------------------------
				Select Case Accion
					Case ELL.Acciones.Insertar, ELL.Acciones.Modificar, ELL.Acciones.Borrar
						Transakzio.RollbackTransaction()
				End Select
				'-----------------------------------------------------------------
				Throw
			End Try
        End Function

        '#Region "Conversor"
        '        'Option Explicit

        '        Public Numero As String
        '        Public DeBase As Integer
        '        Public tobase As Integer

        '        ' función que convierte de número Hexadecimal a Decimal
        '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '        Public Function HexToDec(ByVal HexStr As String) As Double
        '            Dim mult As Double
        '            Dim DecNum As Double
        '            Dim ch As String
        '            mult = 1
        '            DecNum = 0

        '            Dim i As Integer
        '            For i = Len(HexStr) To 1 Step -1
        '                ch = Mid(HexStr, i, 1)
        '                If (ch >= "0") And (ch <= "9") Then
        '                    DecNum = DecNum + (Val(ch) * mult)
        '                Else
        '                    If (ch >= "A") And (ch <= "F") Then
        '                        DecNum = DecNum + ((Asc(ch) - Asc("A") + 10) * mult)
        '                    Else
        '                        If (ch >= "a") And (ch <= "f") Then
        '                            DecNum = DecNum + ((Asc(ch) - Asc("a") + 10) * mult)
        '                        Else
        '                            HexToDec = 0
        '                            Exit Function
        '                        End If
        '                    End If
        '                End If
        '                mult = mult * 16
        '            Next i
        '            HexToDec = DecNum
        '        End Function


        '        ' función que convierte de número Decimal a Hexadecimal
        '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '        Public Function DecToHex(ByVal DecNum As Double) As String
        '            Dim remainder As Integer
        '            Dim HexStr As String
        '            HexStr = ""
        '            Do While DecNum <> 0
        '                remainder = DecNum Mod 16
        '                If remainder <= 9 Then
        '                    HexStr = Chr(Asc(remainder)) & HexStr
        '                Else
        '                    HexStr = Chr(Asc("A") + remainder - 10) & HexStr
        '                End If
        '                DecNum = DecNum \ 16
        '            Loop
        '            If HexStr = "" Then HexStr = "0"
        '            DecToHex = HexStr
        '        End Function


        '        ' función que convierte de número Decimal a Binario
        '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '        Public Function DecToBin(ByVal DecNum As Double) As String
        '            Dim BinStr As String
        '            BinStr = ""
        '            Do While DecNum <> 0
        '                If (DecNum Mod 2) = 1 Then
        '                    BinStr = "1" & BinStr
        '                Else
        '                    BinStr = "0" & BinStr
        '                End If
        '                DecNum = DecNum \ 2
        '            Loop
        '            If BinStr = "" Then BinStr = "0000"
        '            DecToBin = BinStr
        '        End Function


        '        ' función que convierte de número Binario a número decimal
        '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '        Public Function BinToDec(ByVal BinStr As String) As Double
        '            Dim mult As Double
        '            Dim DecNum As Double
        '            mult = 1
        '            DecNum = 0

        '            Dim i As Integer
        '            For i = Len(BinStr) To 1 Step -1
        '                If Mid(BinStr, i, 1) = "1" Then
        '                    DecNum = DecNum + mult
        '                End If
        '                mult = mult * 2
        '            Next i
        '            BinToDec = DecNum
        '        End Function


        '        ' función que convierte de número Hexadecimal a número binario
        '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''
        '        Public Function HexToBin(ByVal HexStr As String) As String
        '            Dim BinStr As String
        '            BinStr = ""
        '            Dim i As Integer
        '            For i = 1 To Len(HexStr)
        '                BinStr = BinStr & DecToBin(HexToDec(Mid(HexStr, i, 1)))
        '            Next i
        '            HexToBin = BinStr
        '        End Function


        '        ' función que convierte de número binario a Hexadecimal
        '        ''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '        Public Function BinToHex(ByVal BinStr As String) As String
        '            Dim HexStr As String
        '            HexStr = ""
        '            Dim i As Integer
        '            For i = 1 To Len(BinStr) Step 4
        '                HexStr = HexStr & DecToHex(BinToDec(Mid(BinStr, i, 4)))
        '            Next i
        '            BinToHex = HexStr
        '        End Function

        '        'función que retorna el valor
        '        '''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''''

        '        Public Function Convertir(ByVal numToConvert As Object, _
        '                                   ByVal DeBase As Integer, _
        '                                   ByVal tobase As Integer) As Object


        '            Select Case DeBase

        '                Case 2
        '                    If Not Verificar(Numero, "01") Then
        '                        MsgBox("Base 2 sólo puede contener 0 y 1")
        '                        Exit Function
        '                    End If
        '                Case 8
        '                    If Not Verificar(Numero, "01234567") Then
        '                        MsgBox("Base 8 sólo puede contener de '0' a '7'.")
        '                        Exit Function
        '                    End If
        '                Case 10
        '                    If Not Verificar(Numero, "0123456789") Then
        '                        MsgBox("Base 10 sólo puede contener de '0' a '9'.")
        '                        Exit Function
        '                    End If
        '                Case 16
        '                    If Not Verificar(Numero, "0123456789ABCDEFabcdef") Then
        '                        MsgBox("La Base 16 sólo puede contener " & _
        '                                " de '0' a '9' y de 'A' a 'F' o de 'a' a 'f'.")
        '                        Exit Function
        '                    End If
        '            End Select

        '            If DeBase = tobase Then
        '                Convertir = numToConvert
        '                Exit Function
        '            End If

        '            Select Case DeBase
        '                Case 2
        '                    Select Case tobase
        '                        Case 10
        '                            Convertir = BinToDec(numToConvert)
        '                            Exit Function
        '                        Case 16
        '                            Convertir = BinToHex(numToConvert)
        '                            Exit Function
        '                    End Select
        '                Case 10
        '                    Select Case tobase
        '                        Case 2
        '                            Convertir = DecToBin(numToConvert)
        '                            Exit Function
        '                        Case 16
        '                            Convertir = DecToHex(numToConvert)
        '                            Exit Function
        '                    End Select
        '                Case 16
        '                    Select Case tobase
        '                        Case 2
        '                            Convertir = HexToBin(numToConvert)
        '                            Exit Function
        '                        Case 10
        '                            Convertir = HexToDec(numToConvert)
        '                            Exit Function
        '                    End Select
        '            End Select
        '        End Function


        '        Private Function Verificar(ByVal valor As String, _
        '        ByVal bag As String) As Boolean

        '            Dim i As Integer
        '            Verificar = True
        '            For i = 1 To Len(valor)
        '                If InStr(1, bag, Mid(valor, i, 1)) = 0 Then
        '                    Verificar = False
        '                    Exit Function
        '                End If
        '            Next i

        '        End Function
        '#End Region

    End Class

End Namespace
