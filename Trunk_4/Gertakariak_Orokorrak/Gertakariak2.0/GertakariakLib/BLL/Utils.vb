Imports System.Data

Namespace BLL

    Public Class Utils

#Region "Estructura para guardar traducciones"
		Public Structure Literal

			Private _descripcion As String
			Private _idCultura As String

			Public Property Descripcion() As String
				Get
					Return _descripcion
				End Get
				Set(ByVal value As String)
					_descripcion = value
				End Set
			End Property

			Public Property IdCultura() As String
				Get
					Return _idCultura
				End Get
				Set(ByVal value As String)
					_idCultura = value
				End Set
			End Property
		End Structure

#End Region

#Region "Traducir Campo"
        ''' <summary>
        ''' Traduccion de campos para tablas multi-cultura.
        ''' </summary>
        ''' <param name="item">Objeto que representa el resultado de una consulta con sus registros. E.j.: BatzLib.GERTAKARIAK.TIPONOCONFORMIDADKULTURA</param>
        ''' <param name="CampoDescripcion">Objeto que representa el Nombre del campo donde se encuentra la informacion del registro a traducir. E.j.:DAL.dbAreaOrigenKultura.ColumnNames.TIPONOCONFORMIDAD.ToString</param>
        ''' <param name="CampoKultura">Objeto que representa el Nombre del campo donde se encuentra la cultura del registro. E.j.:DAL.dbAreaOrigenKultura.ColumnNames.IDCULTURA.ToString</param>
        ''' <param name="arrKultura">Vector de Idiomas de la aplicación que da el rango de traducción.</param>
        ''' <returns>Estructura Literal.</returns>
        ''' <remarks></remarks>
        Public Shared Function TraducirCampo(ByVal item As Object, ByVal CampoDescripcion As Object, ByVal CampoKultura As Object, ByVal arrKultura As ArrayList) As Literal
            If arrKultura.Count <> 0 Then
                If (arrKultura(0).GetType() Is System.Type.GetType("System.Collections.ArrayList")) Then
                    arrKultura = ConvertirArrayUnidimensional(arrKultura)
                End If
            End If
            Return EremuItzulpena(item, CampoDescripcion, CampoKultura, arrKultura)
        End Function
        ''' <summary>
        ''' Traduccion de campos para tablas multi-cultura.
        ''' </summary>
        ''' <param name="item">Objeto que representa el resultado de una consulta con sus registros. E.j.: BatzLib.GERTAKARIAK.TIPONOCONFORMIDADKULTURA</param>
        ''' <param name="CampoDescripcion">Objeto que representa el Nombre del campo donde se encuentra la informacion del registro a traducir. E.j.:DAL.dbAreaOrigenKultura.ColumnNames.TIPONOCONFORMIDAD.ToString</param>
        ''' <param name="CampoKultura">Objeto que representa el Nombre del campo donde se encuentra la cultura del registro. E.j.:DAL.dbAreaOrigenKultura.ColumnNames.IDCULTURA.ToString</param>
        ''' <param name="strKultura">Cadena de Caracteres que representa UN idioma de la aplicación que da el rango de traducción.</param>
        ''' <returns>Estructura Literal.</returns>
        ''' <remarks></remarks>
        Public Shared Function TraducirCampo(ByVal item As Object, ByVal CampoDescripcion As Object, ByVal CampoKultura As Object, ByVal strKultura As String) As Literal
            Dim arrKultura As New ArrayList
            arrKultura.Add(strKultura)
            Return EremuItzulpena(item, CampoDescripcion, CampoKultura, arrKultura)
        End Function
        ''' <summary>
        ''' Traduccion de campos para tablas multi-cultura.
        ''' </summary>
        ''' <param name="item">Objeto que representa el resultado de una consulta con sus registros. E.j.: BatzLib.GERTAKARIAK.TIPONOCONFORMIDADKULTURA</param>
        ''' <param name="CampoDescripcion">Objeto que representa el Nombre del campo donde se encuentra la informacion del registro a traducir. E.j.:DAL.dbAreaOrigenKultura.ColumnNames.TIPONOCONFORMIDAD.ToString</param>
        ''' <param name="CampoKultura">Objeto que representa el Nombre del campo donde se encuentra la cultura del registro. E.j.:DAL.dbAreaOrigenKultura.ColumnNames.IDCULTURA.ToString</param>
        ''' <param name="arrKultura">Vector de Idiomas de la aplicación que da el orden del rango de traducción.</param>
        ''' <returns>Estructura Literal.</returns>
        ''' <remarks></remarks>
        Private Shared Function EremuItzulpena(ByVal item As Object, ByVal CampoDescripcion As Object, ByVal CampoKultura As Object, ByVal arrKultura As ArrayList) As Literal
            Dim Cultura As String = ""
            Dim Descripcion As String = ""
            Dim stLiteral As Literal

            Dim i, z As Integer
            Dim dvItem As DataView = item.DefaultView
            If item.RowCount > 0 Then
                'Recorremos el vector de culturas (arrKultura) que nos da el orden de preferencia de la cultura.
                For i = 0 To arrKultura.Count - 1 ' .Length - 1
                    'Recorremos la tabla con las traducciones del termino
                    For z = 0 To dvItem.Count - 1
                        If dvItem.Item(z).Row.Item(CampoKultura).ToString.Trim.ToUpper.Equals(arrKultura(i).ToString.Trim.ToUpper) Then
                            Cultura = dvItem.Item(z).Row.Item(CampoKultura).ToString.Trim
                            Descripcion = dvItem.Item(z).Row.Item(CampoDescripcion).ToString.Trim 'dvItem.Item(z).Item(CampoDescripcion)
                            stLiteral = New Literal
                            stLiteral.Descripcion = Descripcion
                            stLiteral.IdCultura = Cultura
                            Return stLiteral
                        End If
                    Next
                Next
            End If

            stLiteral = New Literal
            stLiteral.Descripcion = Descripcion
            stLiteral.IdCultura = Cultura
            Return stLiteral
        End Function
#End Region

#Region "Traducir Termino"
        ''' <summary>
        ''' Traduce un termino
        ''' </summary>
        ''' <param name="key">Clave a traducir</param>
        ''' <returns>Termino traducido</returns>
        <Obsolete()> _
        Public Shared Function TraducirTermino(ByVal key As String) As String
            Try
                Return TraduccionesLib.Itzuli(key)
            Catch ex As Exception
                Return String.Empty
            End Try
        End Function
#End Region

#Region "Convertir Array Unidimensional"
        ''' <summary>
        ''' Convierte un array de arrays en uno unidimensional
        ''' </summary>
        ''' <param name="pArray">Array de arrays</param>
        ''' <returns>Array unidimensional</returns>
        Public Shared Function ConvertirArrayUnidimensional(ByVal pArray As ArrayList) As ArrayList
            Dim arrayResul As New ArrayList()
            For Each subArray As ArrayList In pArray
                arrayResul.AddRange(subArray)
            Next
            Return arrayResul
        End Function
#End Region

#Region "Obtener Culturas"

        '''' <summary>
        '''' Obtiene todas las culturas
        '''' <list>
        '''' <listheader>Orden de las culturas</listheader>
        '''' <item>
        ''''   1º Cultura seleccionada por el usuario
        '''' </item>
        '''' <item>
        ''''   2º Culturas del navegador
        '''' </item>
        '''' <item>
        ''''   3º Culturas de la base de datos relaccionadas con la primera cultura anterior encontrada
        '''' </item>
        '''' <item>
        ''''   4º Todas las culturas de la base de datos por si ninguna de las anteriores fuera valida
        '''' </item>
        '''' </list>
        '''' </summary>
        '''' <param name="kulturaActual">Cultura actual si la tiene. Ej.:"eu-ES" (Ticket.Culture)</param>
        '''' <param name="UserLanguages">Array de lenguages de usuarios. Ej.:Request.UserLanguages</param>
        '''' <returns>Matriz de 3 elementos, que contiene:
        '''' <para>ArrayList con la cultura del ticket </para> 
        '''' <para>ArrayList con las culturas del navegador</para>
        '''' <para>ArrayList con las culturas de la base de datos</para>
        '''' </returns>
        'Public Shared Function getCulturas(ByVal kulturaActual As String, ByVal UserLanguages As String()) As ArrayList
        '    Dim aKulturas As New ArrayList
        '    Try
        '        Dim aKulturaTicket As New ArrayList
        '        Dim aKulturaNavegador As New ArrayList
        '        Dim aKulturaBaseDatos As New ArrayList
        '        Dim primeraCultura As String = String.Empty

        '        'Si ha seleccionado un idioma del desplegable, se marca como principal el idioma elegido
        '        If (kulturaActual <> String.Empty) Then
        '            aKulturaTicket.Add(kulturaActual)
        '            primeraCultura = kulturaActual
        '        End If

        '        'Se obtienen las culturas del navegador
        '        If Not UserLanguages Is Nothing Then
        '            aKulturaNavegador.AddRange(CulturasNavegador(UserLanguages))
        '            If (primeraCultura = String.Empty And aKulturaNavegador.Count > 0) Then
        '                primeraCultura = aKulturaNavegador.Item(0)
        '            End If
        '        End If

        '        'Si se tiene una cultura del ticket o alguna cultura en el navegador, se obtienen las culturas de la base de datos relacionadas con el 1º elemento del Array.
        '        If (aKulturaTicket.Count > 0) Then
        '            aKulturaBaseDatos.AddRange(CulturasBaseDatos(aKulturaTicket.Item(0).ToString()))
        '        ElseIf (aKulturaNavegador.Count > 0) Then
        '            aKulturaBaseDatos.AddRange(CulturasBaseDatos(aKulturaNavegador.Item(0).ToString()))
        '        End If

        '        'Por ultimo, se obtiene todas las culturas de la base de datos
        '        aKulturaBaseDatos.AddRange(CulturasBaseDatos())
        '        If (primeraCultura = String.Empty And aKulturaBaseDatos.Count > 0) Then
        '            primeraCultura = aKulturaBaseDatos.Item(0)
        '        End If

        '        'Añadimos todos los arrays al array principal
        '        aKulturas.Add(aKulturaTicket)
        '        aKulturas.Add(aKulturaNavegador)
        '        aKulturas.Add(aKulturaBaseDatos)

        '        'Eliminamos las culturas repetidas
        '        aKulturas = EliminarKulturasRepetidas(aKulturas)

        '        If Not aKulturas Is Nothing Then
        '            System.Threading.Thread.CurrentThread.CurrentCulture = System.Globalization.CultureInfo.CreateSpecificCulture(primeraCultura)
        '        End If

        '    Catch ex As Exception
        '        Return Nothing
        '    End Try

        '    Return aKulturas
        'End Function


        ''' <summary>
        ''' Dado un array de culturas de idiomas, devuelve el primer idioma encontrado
        ''' </summary>
        ''' <param name="aCulturas"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function CulturaActiva(ByVal aCulturas As ArrayList)
            For Each aLista As ArrayList In aCulturas
                If (aLista.Count > 0) Then
                    Return aLista.Item(0)
                End If
            Next
            Return System.Threading.Thread.CurrentThread.CurrentCulture.Name
        End Function
#Region "Eliminar culturas repetidas"

        ''' <summary>
        ''' Elimina las culturas repetidas de una matriz de arrays
        ''' </summary>
        ''' <param name="aKult">Array de culturas </param>
        Public Shared Function EliminarKulturasRepetidas(ByVal aKult As ArrayList) As ArrayList
            Dim akultFinal As New ArrayList
            Try
                Dim i As Integer
                For i = 1 To aKult.Count
                    akultFinal.Add(New ArrayList())
                Next

                For i = 0 To aKult.Count - 1
                    For Each item As String In aKult.Item(i)
                        If (Not estaEnArraysCulturas(item, akultFinal)) Then
                            CType(akultFinal.Item(i), ArrayList).Add(item)
                        End If
                    Next
                Next

            Catch ex As Exception
                Dim sms As String = ex.ToString()
            End Try

            Return akultFinal
        End Function

        ''' <summary>
        ''' Comprueba si un item esta en un array
        ''' </summary>
        ''' <param name="item">String a comparar</param>
        ''' <param name="aKultFinal">Array de culturas donde hay que buscar el item</param>
        ''' <returns>Booleano que indica si el item ya existe en el array</returns>
        Private Shared Function estaEnArraysCulturas(ByVal item As String, ByVal aKultFinal As ArrayList) As Boolean
            Dim resul As Boolean = False
            Dim aArray As ArrayList
            Dim i As Integer

            For i = 0 To aKultFinal.Count - 1
                aArray = aKultFinal.Item(i)
                For Each itemArray As String In aArray
                    If (itemArray = item) Then Return True
                Next
            Next
            Return False
        End Function
#End Region

#Region "Culturas Navegador"
        ''' <summary>
        ''' Obtiene las culturas del navegador del usuario
        ''' </summary>
        ''' <param name="userLanguages">Array de lenguajes</param>
        ''' <returns>ArrayList con las culturas encontradas</returns>
        ''' <remarks></remarks>
        Public Shared Function CulturasNavegador(ByVal userLanguages As String()) As ArrayList

            Dim ci As System.Globalization.CultureInfo
            Dim aAux As Array
            Dim IECultura As New ArrayList
            If Not userLanguages Is Nothing Then
                For Each Item As String In userLanguages
                    aAux = Item.Split(";")
                    Try
                        ci = System.Globalization.CultureInfo.CreateSpecificCulture(aAux(0).ToString)
                        IECultura.Add(ci.Name)
                    Catch
                    End Try
                Next
            End If

            Return IECultura
        End Function
#End Region

#Region "Culturas Base de datos"

        '      ''' <summary>
        '      ''' Obtiene todas las culturas de la base de datos
        '      ''' </summary>
        '      ''' <returns>ArrayList con las culturas encontradas</returns>
        '      ''' <remarks></remarks>
        '      Public Shared Function CulturasBaseDatos() As ArrayList
        '          Dim arrRangoKultura As New ArrayList
        '	Dim tRangoKultura As New SABLib_Z.DAL.RANGOKULTURA
        '	tRangoKultura.Query.AddOrderBy(SABLib_Z.DAL.RANGOKULTURA.ColumnNames.ORDEN, AccesoAutomaticoBD.WhereParameter.Dir.ASC)
        '	tRangoKultura.Query.Load()
        '	If tRangoKultura.RowCount > 0 Then
        '		Do
        '			arrRangoKultura.Add(tRangoKultura.IDKULTURA.Trim)
        '		Loop While tRangoKultura.MoveNext
        '	End If
        '	Return arrRangoKultura
        'End Function

        '      ''' <summary>
        '      ''' Obtiene las vector de rangos de idiomas relacionadas con la pasada como parametro de la base de datos
        '      ''' </summary>
        '      ''' <param name="kultura">Identificador de la cultura a partir de la que obtener el rango</param>
        '      ''' <returns>ArrayList con las culturas encontradas</returns>
        '      ''' <remarks></remarks>
        '      Private Shared Function CulturasBaseDatos(ByVal kultura As String) As ArrayList
        '	Dim arrRangoKultura As New ArrayList
        '	Dim tRangoKultura As New SABLib_Z.DAL.RANGOKULTURA

        '	tRangoKultura.Where.IDCULTURA.Value = kultura.Trim
        '	tRangoKultura.Query.AddOrderBy(SABLib_Z.DAL.RANGOKULTURA.ColumnNames.ORDEN, AccesoAutomaticoBD.WhereParameter.Dir.ASC)

        '	tRangoKultura.Query.Load()

        '	If tRangoKultura.RowCount > 0 Then
        '		Do
        '			arrRangoKultura.Add(tRangoKultura.IDKULTURA.Trim)
        '		Loop While tRangoKultura.MoveNext
        '	End If

        '	Return arrRangoKultura
        'End Function


#End Region

#End Region

#Region "Control de nulos"

        Public Shared Function stringNull(ByVal o As Object) As String
            Dim strResul As String = String.Empty
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                strResul = o.ToString()
            End If
            Return strResul
        End Function

        Public Shared Function integerNull(ByVal o As Object) As Integer
            Dim intResul As Integer = Integer.MinValue
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                intResul = CInt(o.ToString())
            End If
            Return intResul
        End Function

        Public Shared Function dateTimeNull(ByVal o As Object) As DateTime
            Dim dtResul As DateTime = DateTime.MinValue
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                dtResul = CType(o.ToString(), DateTime)
            End If
            Return dtResul
        End Function

        ''' <summary>
        ''' Devuelve el string si no es vacio y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlStringNull(ByVal o As String) As String
            Dim strResul As String = Nothing
            If (o <> String.Empty) Then
                strResul = o
            End If
            Return strResul
        End Function

        ''' <summary>
        ''' Devuelve el integer si no es Integer.MinValue y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlIntegerNull(ByVal o As Integer) As Nullable(Of Integer)
            Dim intResul As Nullable(Of Integer) = Nothing
            If (o <> Integer.MinValue) Then
                intResul = o
            End If
            If (intResul.HasValue) Then
                Return intResul.Value
            Else
                Return intResul
            End If
        End Function

        ''' <summary>
        ''' Devuelve la fecha si no es DateTime.MinValue y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlDateTimeNull(ByVal o As DateTime) As Nullable(Of DateTime)
            Dim dtResul As Nullable(Of DateTime) = Nothing
            If (o <> DateTime.MinValue) Then
                dtResul = o
            End If
            Return dtResul
        End Function

#End Region

    End Class
End Namespace
