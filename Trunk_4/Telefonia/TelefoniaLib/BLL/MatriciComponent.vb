Namespace BLL

    Public Class MatriciComponent

        ''' <summary>
        ''' Obtiene los usuarios de Matrici
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetUsuariosMatrici() As List(Of Sablib.ELL.Usuario)
            Try
                Dim matriciDAL As New DAL.MatriciDAL
                Dim lUsuariosResul As New List(Of Sablib.ELL.Usuario)
                Dim lUsuarios As List(Of ELL.Matrici) = matriciDAL.GetUsuariosMatrici()
                For Each myUser As ELL.Matrici In lUsuarios
                    lUsuariosResul.Add(New Sablib.ELL.Usuario With {.Id = myUser.Id, .Nombre = myUser.Nombre, .Apellido1 = myUser.Apellidos})
                Next

                Return lUsuariosResul
            Catch ex As Exception
                Throw New BatzException("Error al obtener los usuarios de Matrici", ex)
            End Try
        End Function

        ''' <summary>
        ''' Dado un fichero csv, lee cada linea y los importa en la tabla de telefonos de Matrici
        ''' </summary>
        ''' <param name="fichero">Fichero a leer</param>
        ''' <returns>Numero de telefonos insertados</returns>        
        Public Function ImportarMatrici(ByVal fichero As String) As Integer
            Dim fileStream As System.IO.StreamReader = Nothing
            Try
                Dim guiaTelefonos As New List(Of String())
                Dim matriciDAL As New DAL.MatriciDAL
                Dim numLineas As Integer = 0
                fileStream = New System.IO.StreamReader(fichero, System.Text.Encoding.GetEncoding(1252))
                'persona|unidad|area|seccion|extFija|extInalambrica|directoFijo|directoInal|movil|mov_codigo|skype_code
                While Not fileStream.EndOfStream
                    If (numLineas = 0) Then
                        fileStream.ReadLine() 'La primera fila son las cabeceras
                    Else
                        guiaTelefonos.Add(fileStream.ReadLine.Split("|"))
                    End If
                    numLineas += 1
                End While
                'Se insertan las lineas en base de datos
                matriciDAL.ImportarMatrici(guiaTelefonos)
                Return numLineas
            Catch batzEx As Sablib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New Exception("Error al leer el fichero csv", ex)
            Finally
                If (fileStream IsNot Nothing) Then fileStream.Close()
            End Try
        End Function

        ''' <summary>
        ''' Lee todos las personas junto con su informacion de extensiones que sean activas y no obsoletas y las guarda en un fichero
        ''' </summary>        
        ''' <param name="rutaTemp">Ruta del directorio donde se guardara el fichero generado</param>        
        ''' <param name="idPlanta">Id de la planta de la que se obtendran los resultados</param>
        ''' <returns>Numero de telefonos exportados</returns>        
        Public Function ExportarMatrici(ByVal rutaTemp As String, ByVal idPlanta As Integer) As String
            Dim fileStream As System.IO.StreamWriter = Nothing
            Try
                fileStream = New System.IO.StreamWriter(rutaTemp, False, Text.Encoding.UTF8)                
                Dim lTlfnoExt As List(Of ELL.TelefonoExtension)
                Dim extComp As New BLL.ExtensionComponent
                Dim depComp As New Sablib.BLL.DepartamentosComponent
                Dim oDep As Sablib.ELL.Departamento = Nothing
                lTlfnoExt = extComp.VerTodos(idPlanta, True)
                lTlfnoExt.Sort(Function(o1 As ELL.TelefonoExtension, o2 As ELL.TelefonoExtension) o1.Nombre < o2.Nombre)  'Se ordena por nombre                

                Dim linea As String = "Persona|Departamento|Ext_Fija|Fijo|Ext_Inalambrica|Inalambrico|Ext_Movil|Movil|Zoiper"
                fileStream.WriteLine(linea)
                For Each oTlfnoExt As ELL.TelefonoExtension In lTlfnoExt
                    linea = oTlfnoExt.Nombre.Trim & "|" & oTlfnoExt.Departamento.Trim & "|" & FormatInt(oTlfnoExt.ExtFija) & "|" & oTlfnoExt.Fijo & "|" & FormatInt(oTlfnoExt.ExtInalambrica) & "|" _
                            & oTlfnoExt.Inalambrico & "|" & FormatInt(oTlfnoExt.ExtensionMovil) & "|" & oTlfnoExt.TlfnoMovil & "|" & FormatInt(oTlfnoExt.Zoiper)
                    fileStream.WriteLine(linea)
                Next

                Return lTlfnoExt.Count
            Catch batzEx As Sablib.BatzException
                Throw batzEx
            Catch ex As Exception
                Throw New BatzException("Error al exportar los datos al fichero csv", ex)
            Finally
                If (fileStream IsNot Nothing) Then fileStream.Close()
            End Try
        End Function

        ''' <summary>
        ''' Si el entero es minvalue, se devuelve vacio, sino el entero recibido
        ''' </summary>
        ''' <param name="param">Entero a comprobar</param>
        ''' <returns></returns>        
        Private Function FormatInt(ByVal param As String) As String            
            If (param = String.Empty OrElse (param <> String.Empty AndAlso CInt(param) = Integer.MinValue)) Then
                Return String.Empty
            Else
                Return param
            End If
        End Function

        ''' <summary>
        ''' Dada un id de usuario o un departamento, busca la informacion de un registro de Matrici
        ''' </summary>        
        Public Function GetInfoMatrici(ByVal idUser As Integer, ByVal departamento As String, ByVal OrderByPersonas As Boolean) As List(Of ELL.Matrici)
            Dim matriciDAL As New DAL.MatriciDAL
            Return matriciDAL.GetInfoMatrici(idUser, departamento, OrderByPersonas)
        End Function

    End Class

End Namespace