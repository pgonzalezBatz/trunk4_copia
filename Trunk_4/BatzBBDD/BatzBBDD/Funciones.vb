Imports System.Configuration
Imports System.Reflection

Public Class Funciones
    ' ''' <summary>
    ' ''' Función que transforma el texto en una expresion regular para realizar busquedas.
    ' ''' </summary>
    ' ''' <param name="Texto">Texto a transformar en expresion regular.</param>
    '<Obsolete("2013-04-15: Usar Sablib.BLL.Utils.TextoLike", True)> _
    'Public Function TextoLike(ByVal Texto As String) As String
    '	Dim Caracteres As String() = {"aáàäâ@", "eéèëê@", "iíìïî", "oóòöô@", "uúùüû", "cČçzksx", "nñm", "vb", "gj"}
    '	Dim TextoReg As String = ""
    '	Dim TextoArray As String = ""
    '	Dim i, z, x, PosicionTexto As Integer

    '	For i = 1 To Len(Texto)
    '		PosicionTexto = InStr(1, Texto, Mid(Texto, i, 1))
    '		'-------------------------------------------------------------------------------------
    '		'For z = 0 To UBound(Caracteres)
    '		'    For x = 1 To Len(Caracteres(z))
    '           '        If String.Compare(Mid(Texto, i, 1), Mid(Caracteres(z), x, 1), 1) = 0 Then
    '		'            TextoArray = "[" & Caracteres(z) & "]"
    '		'            Exit For
    '		'        End If
    '		'    Next
    '		'Next
    '		'-------------------------------------------------------------------------------------
    '		'FROGA: 
    '		'-------------------------------------------------------------------------------------
    '           If (String.Compare(Mid(Texto, i, 1), Mid("h", x, 1), True) = 0) Then
    '               TextoArray = "[h?]" 'Indicamos que la h aparece uno o ninguna vez.
    '           Else
    '               For z = 0 To UBound(Caracteres)
    '                   For x = 1 To Len(Caracteres(z))
    '                       If String.Compare(Mid(Texto, i, 1), Mid(Caracteres(z), x, 1), True) = 0 Then
    '                           TextoArray = "[" & Caracteres(z) & "]"
    '                           Exit For
    '                       End If
    '                   Next
    '               Next
    '           End If
    '		'-------------------------------------------------------------------------------------
    '		If Trim(TextoArray) <> "" Then
    '			TextoReg = TextoReg & TextoArray
    '			TextoArray = ""
    '		Else
    '			TextoReg = TextoReg & Mid(Texto, PosicionTexto, 1)
    '		End If
    '	Next
    '	Return TextoReg
    'End Function

    ' ''' <summary>
    ' ''' Proceso para el copiado de los valores de las propiedades de un objeto en otro.
    ' ''' Cargamos los datos de los campos coincidentes del objeto Origen al Destino.
    ' ''' </summary>
    ' ''' <param name="Origen">Objeto donde 'OBTENEMOS' los valores.</param>
    ' ''' <param name="Destino">Objeto donde 'CARGAMOS' los valores.</param>
    ' ''' <remarks></remarks>
    '<Obsolete("2013-04-15: Usar Sablib.BLL.Utils.CopiarPropiedades", True)> _
    'Public Sub CopiarPropiedades(ByVal Origen As Object, ByVal Destino As Object)
    '	'------------------------------------------------------------------------------
    '	'El objeto "Origen" es la "clase base" (BaseType) del objeto "Destino".
    '	'El objeto "Destino" puede esconder propiedades del objeto "Origen".
    '	'Para evitar que algunos campos se queden sin datos, cogemos los valores del "Origen" y se los pasamos a la "clase base" del destino.
    '	'------------------------------------------------------------------------------
    '	For Each Propiedad As PropertyInfo In Destino.GetType.BaseType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
    '		If Propiedad.GetSetMethod(True) IsNot Nothing Then
    '			For Each rPropiedad As PropertyInfo In Origen.GetType.GetProperties(BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Instance)
    '				If Propiedad.Name = rPropiedad.Name And rPropiedad.CanWrite = True Then
    '					Propiedad.SetValue(Destino, rPropiedad.GetValue(Origen, Nothing), Nothing)
    '				End If
    '			Next
    '		End If
    '	Next
    '	'------------------------------------------------------------------------------
    'End Sub

    ''' <summary>
    ''' Proceso para ordenar las incidencias. Ordena a partir del orden de la incidencia.
    ''' </summary>
    ''' <param name="BBDD">Entidades con la que se va a realizar la ordenacion</param>
    ''' <param name="gtk">Incidencia que proboca la ordenacion</param>
    ''' <param name="TipoOrdenacion">Identificador del tipo de ordenacion. 
    ''' Todas - Ordena todas las incidencias del proyecto de la incidencia. 
    ''' Pendientes - Ordena solo las incidencias pendientes (abiertas) del proyecto de la incidencia.</param>
    ''' <remarks></remarks>
    Public Sub OrdenarIncidencias(BBDD As BatzBBDD.Entities_Gertakariak, ByVal gtk As BatzBBDD.GERTAKARIAK, ByRef TipoOrdenacion As Nullable(Of TipoOrdenacion), ByRef OrdenOriginal As Integer)
        Dim Orden As Integer
        If TipoOrdenacion IsNot Nothing Then
            Dim bPendientes As Boolean = If(TipoOrdenacion = Funciones.TipoOrdenacion.Pendientas, True, False) 'Identificamos si se ordenan todas o solo las pendientes.
            Using Transaccion As New TransactionScope
                '--------------------------------------------------------------------------------------------------------------------------------------
                Dim lReg As IQueryable(Of BatzBBDD.GERTAKARIAK) = Nothing
                If OrdenOriginal > gtk.ORDEN Or OrdenOriginal = 0 Then   'Se produce al retrasar el "Orden" o al crear uno nuevo.
                    '--------------------------------------------------------------------------------------------------------------
                    lReg =
                        From Reg As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                        Where Reg.IDTIPOINCIDENCIA = gtk.IDTIPOINCIDENCIA And If(gtk.IDPROYECTO Is Nothing, Reg.IDPROYECTO Is Nothing, Reg.IDPROYECTO = gtk.IDPROYECTO) _
                        And Reg.ID <> gtk.ID And Reg.ORDEN >= gtk.ORDEN _
                        And If(bPendientes, Reg.FECHACIERRE Is Nothing OrElse Reg.FECHACIERRE >= Now, True = True)
                        Select Reg Order By Reg.ORDEN Ascending, Reg.ID Descending
                    If lReg.Any Then
                        Orden = gtk.ORDEN 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
                        For Each Reg As BatzBBDD.GERTAKARIAK In lReg
                            Orden += 1
                            Reg.ORDEN = Orden
                        Next
                    End If
                    '--------------------------------------------------------------------------------------------------------------
                ElseIf OrdenOriginal <= gtk.ORDEN Then 'Se produce al avanzar el "Orden".
                    '--------------------------------------------------------------------------------------------------------------
                    lReg =
                        From Reg As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                        Where Reg.IDTIPOINCIDENCIA = gtk.IDTIPOINCIDENCIA And If(gtk.IDPROYECTO Is Nothing, Reg.IDPROYECTO Is Nothing, Reg.IDPROYECTO = gtk.IDPROYECTO) _
                        And Reg.ID <> gtk.ID And Reg.ORDEN <= gtk.ORDEN _
                        And If(bPendientes, Reg.FECHACIERRE Is Nothing OrElse Reg.FECHACIERRE >= Now, True = True)
                        Select Reg Order By Reg.ORDEN Descending, Reg.ID Ascending
                    If lReg IsNot Nothing AndAlso lReg.Any Then
                        Orden = gtk.ORDEN 'Numero de orden base a partir del que se calcula el resto de numeros de la ordenacion.
                        For Each Reg As BatzBBDD.GERTAKARIAK In lReg
                            Orden -= 1
                            Reg.ORDEN = Orden
                        Next
                    End If
                    '--------------------------------------------------------------------------------------------------------------
                End If
                BBDD.SaveChanges()
                '--------------------------------------------------------------------------------------------------------------------------------------

                '--------------------------------------------------------------------------------------------------------------------------------------
                lReg = From Reg As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                       Where Reg.IDTIPOINCIDENCIA = gtk.IDTIPOINCIDENCIA And If(gtk.IDPROYECTO Is Nothing, Reg.IDPROYECTO Is Nothing, Reg.IDPROYECTO = gtk.IDPROYECTO) _
                        And If(bPendientes, Reg.FECHACIERRE Is Nothing OrElse Reg.FECHACIERRE >= Now, True = True)
                       Select Reg Order By Reg.ORDEN Ascending
                If lReg IsNot Nothing AndAlso lReg.Any Then
                    Dim gtkOrden As Integer
                    For Each Reg As BatzBBDD.GERTAKARIAK In lReg
                        gtkOrden += 1
                        Reg.ORDEN = gtkOrden
                    Next
                End If
                BBDD.SaveChanges()
                '--------------------------------------------------------------------------------------------------------------------------------------

                Transaccion.Complete()
            End Using
            BBDD.AcceptAllChanges()
        End If
    End Sub
    Public Enum TipoOrdenacion As Integer
        Todas = 0
        Pendientas = 1
    End Enum

    ''' <summary>
    ''' Funcion para seleccionar elementos de un objeto con "ListItem" para la propiedad "Value" del ListItem.
    ''' Obtimizado para "DropDownList" y "RadioButtonList".
    ''' </summary>
    ''' <param name="ObjLista">Objeto que contenga "ListItems"</param>
    ''' <param name="value">Valor a seleccionar en el "ListItem"</param>
    ''' <returns>Devuelve el Valor del ListItem a seleccionar. 
    ''' Si no existe en la lista del WebControl, agrega automaticamete un ListItem con el nuevo valor para que al seleccionar no de error.</returns>
    ''' <remarks></remarks>
    Public Function SeleccionarItem(ByVal ObjLista As Object, ByVal value As String) As String
        Dim SelectedValue As String = String.Empty
        Dim ListItem As New Web.UI.WebControls.ListItem
        If Not String.IsNullOrWhiteSpace(value) Then
            ListItem = (From li As Web.UI.WebControls.ListItem In DirectCast(ObjLista.items, Web.UI.WebControls.ListItemCollection) Where String.Compare(li.Value, value.Trim, True) = 0 Select li).FirstOrDefault
            If ListItem Is Nothing Then
                ObjLista.Items.Add(New Web.UI.WebControls.ListItem(value.Trim, value.Trim))
                SelectedValue = value.Trim
            Else
                SelectedValue = ListItem.Value
            End If

        End If
        Return SelectedValue
    End Function
End Class

''' <summary>
''' Funciones y procesos para la aplicacion "Gertakariak Sistemas de Automocion" y "AeroSpace"
''' </summary>
''' <remarks></remarks>
Public Class GertakariakSA
    ''' <summary>
    ''' Cierre o apertura automatica de la NC. Si las etapas esta cerradas se cierra la NC.
    ''' Si alguna etapa se abre tambien se abre la NC.
    ''' </summary>
    ''' <param name="gtk">Incidencia que se comprueba</param>
    ''' <remarks></remarks>
    Sub FechaCierreAutomatico_NC(ByVal gtk As BatzBBDD.GERTAKARIAK)
        Dim G8D As New BatzBBDD.G8D
        Dim G8D_E14 As New BatzBBDD.G8D_E14
        Dim G8D_E56 As New BatzBBDD.G8D_E56
        Dim G8D_E78 As New BatzBBDD.G8D_E78
        Dim G8D_E14_FV As Nullable(Of Date)
        Dim G8D_E56_FV As Nullable(Of Date)
        Dim G8D_E78_FV As Nullable(Of Date)

        Try
            G8D = gtk.G8D.SingleOrDefault
            If G8D IsNot Nothing Then
                G8D_E14 = G8D.G8D_E14
                G8D_E56 = G8D.G8D_E56
                G8D_E78 = G8D.G8D_E78

                G8D_E14_FV = G8D_E14.FECHAVALIDACION
                G8D_E56_FV = G8D_E56.FECHAVALIDACION
                G8D_E78_FV = G8D_E78.FECHAVALIDACION

                If G8D_E14_FV IsNot Nothing And G8D_E56_FV IsNot Nothing And G8D_E78_FV IsNot Nothing Then
                    'Cierre automatico de la NC
                    Dim lFecha As New List(Of Nullable(Of Date))
                    lFecha.Add(G8D_E14_FV)
                    lFecha.Add(G8D_E56_FV)
                    lFecha.Add(G8D_E78_FV)

                    gtk.FECHACIERRE = (From Reg In lFecha Select Reg).Max
                    If gtk.ID_BEZERRESIS IsNot Nothing AndAlso gtk.ID_BEZERRESIS > 0 Then updateCierreBezerresis(gtk.ID, gtk.FECHACIERRE)
                Else
                    'Reapertura automatica de la NC
                    gtk.FECHACIERRE = Nothing
                    If gtk.ID_BEZERRESIS IsNot Nothing AndAlso gtk.ID_BEZERRESIS > 0 Then updateCierreBezerresis(gtk.ID, Nothing)
                End If
            End If
        Catch ex As Exception
            Throw
        End Try
    End Sub

    Private Sub updateCierreBezerresis(ByVal id As Integer, ByVal cierre As Date?)
        Dim query As String = "UPDATE BEZERRESIS.RECLAMACIONES_CIERRE SET FECHA_CIERRECLIENTE=:CIERRE WHERE ID=:ID"
        Dim lParam As New List(Of Oracle.ManagedDataAccess.Client.OracleParameter)
        lParam.Add(New Oracle.ManagedDataAccess.Client.OracleParameter("CIERRE", Oracle.ManagedDataAccess.Client.OracleDbType.Date, If(cierre, DBNull.Value), ParameterDirection.Input))
        lParam.Add(New Oracle.ManagedDataAccess.Client.OracleParameter("ID", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, id, ParameterDirection.Input))
        Memcached.OracleDirectAccess.NoQuery(query, ConfigurationManager.ConnectionStrings("GERTAKARIAK").ConnectionString, lParam.ToArray)
    End Sub
End Class
