Imports System.Reflection

Public Class Funciones
	''' <summary>
	''' Proceso para el copiado de los valores de las propiedades de un objeto en otro.
	''' Cargamos los datos de los campos coincidentes del objeto Origen al Destino.
	''' </summary>
	''' <param name="Origen">Objeto donde 'OBTENEMOS' los valores.</param>
	''' <param name="Destino">Objeto donde 'CARGAMOS' los valores.</param>
	''' <remarks></remarks>
	Public Sub CopiarPropiedades(ByVal Origen As Object, ByVal Destino As Object)
		'------------------------------------------------------------------------------
		'El objeto "Origen" es la "clase base" (BaseType) del objeto "Destino".
		'El objeto "Destino" puede esconder propiedades del objeto "Origen".
		'Para evitar que algunos campos se queden sin datos, cogemos los valores del "Origen" y se los pasamos a la "clase base" del destino.
        '------------------------------------------------------------------------------
        For Each Propiedad As PropertyInfo In Destino.GetType.BaseType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
            If Propiedad.GetSetMethod(True) IsNot Nothing Then
                For Each rPropiedad As PropertyInfo In Origen.GetType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
                    If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
                        Propiedad.SetValue(Destino, rPropiedad.GetValue(Origen, Nothing), Nothing)
                    End If
                Next
            End If
        Next
		'------------------------------------------------------------------------------
	End Sub

    ''' <summary>
    ''' Función que transforma el texto en una expresion regular para realizar busquedas.
    ''' </summary>
    ''' <param name="Texto">Texto a transformar en expresion regular.</param>
    <Obsolete("Usar Sablib.BLL.Utils.TextoLike", True)> _
    Public Function TextoLike(ByVal Texto As String) As String
        Dim Caracteres As String() = {"aáàäâ@", "eéèëê@", "iíìïî", "oóòöô@", "uúùüû", "cČçzksx", "nñm", "vb", "gj"}
        Dim TextoReg As String = ""
        Dim TextoArray As String = ""
        Dim i, z, x, PosicionTexto As Integer

        For i = 1 To Len(Texto)
            PosicionTexto = InStr(1, Texto, Mid(Texto, i, 1))
            '-------------------------------------------------------------------------------------
            'For z = 0 To UBound(Caracteres)
            '	For x = 1 To Len(Caracteres(z))
            '		If StrComp(Mid(Texto, i, 1), Mid(Caracteres(z), x, 1), 1) = 0 Then
            '			TextoArray = "[" & Caracteres(z) & "]"
            '			Exit For
            '		End If
            '	Next
            '         Next
            '-------------------------------------------------------------------------------------
            'FROGA: 
            '-------------------------------------------------------------------------------------
            If (String.Compare(Mid(Texto, i, 1), Mid("h", x, 1), True) = 0) Then
                TextoArray = "[h?]" 'Indicamos que la h aparece uno o ninguna vez.
            Else
                For z = 0 To UBound(Caracteres)
                    For x = 1 To Len(Caracteres(z))
                        If String.Compare(Mid(Texto, i, 1), Mid(Caracteres(z), x, 1), True) = 0 Then
                            TextoArray = "[" & Caracteres(z) & "]"
                            Exit For
                        End If
                    Next
                Next
            End If
            '-------------------------------------------------------------------------------------
            If Trim(TextoArray) <> "" Then
                TextoReg = TextoReg & TextoArray
                TextoArray = ""
            Else
                TextoReg = TextoReg & Mid(Texto, PosicionTexto, 1)
            End If
        Next
        Return TextoReg
    End Function
End Class