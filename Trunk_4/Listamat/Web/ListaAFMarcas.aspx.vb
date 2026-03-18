Partial Public Class ListaAFMarcas
    Inherits System.Web.UI.Page
    Private strcn As String = ConfigurationManager.ConnectionStrings("LISTAMAT").ConnectionString

    Protected Overrides Sub InitializeCulture()
        If Session("IdTroqueleria") Is Nothing OrElse Session("IdUsuario") Is Nothing OrElse Session("IdCultura") Is Nothing Then
            'No deberia estar aqui
            Response.Redirect("Default.aspx")
        End If
        MyBase.InitializeCulture()
        Culture = Session("IdCultura")
    End Sub
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        If Request("ord") Is Nothing Or Request("op") Is Nothing Or Request("tipolista") Is Nothing Then
            'TODO: marron
            Response.Redirect("ListaAFMarcas.aspx")
        End If
        'Cargar materiales
        CargarListasConstantes(Request("ord"), Request("op"))
        If Request("mar") Is Nothing Then
            'Añadiendo
        Else
            'Editando marca o copiando
            Dim marca = dbAccess.GetMarca(Request("tipolista"), Request("ord"), Request("op"), Request("mar"), strcn)
            If Request("cp") Is Nothing Then
                'Editando
                txtMarca.Text = marca(0)
            Else
                'Copiando
                hpeso.Value = marca(18)
                hnorma.Value = marca(19)
            End If
            txtCantidad.Text = marca(1)
            txtDiametro.Text = marca(14)
            txtLargo.Text = marca(15)
            txtAncho.Text = marca(16)
            txtGrueso.Text = marca(17)
            txtConjunto.Text = marca(20)
            ddlMateriala.SelectedValue = marca(8)
            'otmardes
            Dim lstcplisdeno = dbAccess.GetMaterialesCplisdeno(marca(8), strcn)
            If ddlOtmardes.Items.Count > 0 Then : ddlOtmardes.SelectedValue = marca(12) : End If

            Dim añadido As String = "N"
            If lstcplisdeno(0) = "S" Or lstcplisdeno(1) = "S" Or lstcplisdeno(2) = "S" Or lstcplisdeno(3) = "S" Or lstcplisdeno(4) = "S" Then
                'Cargar desde otmardes
                CargarOtmardes(lstcplisdeno)
                ddlOtmardes.SelectedValue = marca(12)
            End If
            If lstcplisdeno(5) <> 2 Then
                'material
                CargarOtmatespe(ddlOtmatespe)
                ddlOtmatespe.SelectedValue = marca(13)
                'tratemiento
                CargarOttratam(marca(13))
                ddlTratamiento.SelectedValue = marca(9)
                If marca(9) <> "FABRICACIÓN" AndAlso marca(9) <> "" AndAlso marca(9) > 3 Then
                    'Dureza
                    CargarDurezas(marca(9))
                    ddlDureza.SelectedValue = marca(10)
                    If marca(11) <> "" AndAlso marca(11) > 0 Then
                        'Tratamiento secundario
                        ddlTraSec.SelectedValue = marca(11)
                        ddlRealiza.SelectedValue = marca(7)
                    End If
                End If
            Else
                'Codigo/proveedor
                txtCodigo.Text = marca(3)
                txtProveedor.Text = marca(5)
                CargarOtmatespe(ddlOtmatespeCodigo)
            End If
        End If
        visibility()
    End Sub
    Private Sub CargarListasConstantes(ByVal ord As Integer, ByVal op As Integer)
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        'Materiales desde Cplisdeno
        Dim lstCplisdeno = dbAccess.GetMaterialesCplisdeno(Request("tipolista"), ord, op, strcn)
        ddlMateriala.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", 0))
        For Each i In lstCplisdeno
            ddlMateriala.Items.Add(New ListItem(i(1), i(0)))
        Next
        'Tratamineto secundario. No lo mas eficiente pero lo mas elegante
        Dim lstTratamientosSecundarios = dbAccess.GetTratamientosSecundarios(strcn)
        ddlTraSec.Items.Add(New ListItem(h.traducir("Ninguno"), 0))
        For Each i In lstTratamientosSecundarios
            ddlTraSec.Items.Add(New ListItem(i(1), i(0)))
        Next
    End Sub

    ''' <summary>
    ''' Denominación 1
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlMateriala_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlMateriala.SelectedIndexChanged
        If ddlMateriala.SelectedValue > 0 Then
            Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
            ddlOtmardes.Items.Clear()
            ddlOtmatespe.Items.Clear()
            Dim lstcplisdeno = dbAccess.GetMaterialesCplisdeno(ddlMateriala.SelectedValue, strcn)
            CargarOtmardes(lstcplisdeno)
            '
            If lstcplisdeno(5) <> 2 Then
                'material
                CargarOtmatespe(ddlOtmatespe)
            Else
                'Codigo proveedor.
                CargarOtmatespe(ddlOtmatespeCodigo)
            End If
            visibility()
        End If
    End Sub
    ''' <summary>
    ''' Denominación 2
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlOtmardes_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOtmardes.SelectedIndexChanged
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim otm = dbAccess.GetMateriales2Otmardes(ddlOtmardes.SelectedValue, strcn)
        visibility()
        divDenominacion3.Visible = (otm(0) = "S")
    End Sub
    ''' <summary>
    ''' Materiales desde Otmatespe
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlOtmatespe_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOtmatespe.SelectedIndexChanged
        If ddlOtmatespe.SelectedValue > 0 Then
            CargarOttratam(ddlOtmatespe.SelectedValue)
            visibility()
        End If
    End Sub
    ''' <summary>
    ''' Tratamiento 1
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlTratamiento_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTratamiento.SelectedIndexChanged
        CargarDurezas(ddlTratamiento.SelectedValue)
        visibility()
    End Sub
    ''' <summary>
    ''' Tratamiento 2
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    ''' <remarks></remarks>
    Private Sub ddlTraSec_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTraSec.SelectedIndexChanged
        visibility()
    End Sub
    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsValid Then
            'Cascaria si no existiera entrada en CPLISCAB. Pero esto no deberia de pasar nunca
            Dim qCplisdeno = "select dureza,descri,fase from cplisdeno where recno=:recno"
            Dim pCplisdeno As New Oracle.ManagedDataAccess.Client.OracleParameter("recno", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, ddlMateriala.SelectedValue, ParameterDirection.Input)
            Dim lstCplisdeno As List(Of String()) = OracleManagedDirectAccess.seleccionar(Function(r As Oracle.ManagedDataAccess.Client.OracleDataReader) New String() {r("dureza").ToString, r("descri").ToString, r("fase").ToString}, qCplisdeno, strcn, pCplisdeno)
            If lstCplisdeno Is Nothing OrElse lstCplisdeno.Count <> 1 Then
                Throw New ArgumentNullException("lstCPlisdeno", "lstCPlisdeno es nulo o esta vacio")
            End If


            Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
            Dim observ = Nothing, tratam2 = Nothing, ottratam = Nothing, otmatespe = Nothing, otmardes = Nothing
            Dim observ2 = Nothing, ottrasec = Nothing, material = Nothing
            'Material
            If divDenominacion2.Visible Then
                material = ddlOtmardes.SelectedItem.Text.TrimEnd(" ")
                If divDenominacion3.Visible Then
                    material = material + " " + txtAñadido.Text.TrimEnd(" ")
                End If
            Else
                material = ddlMateriala.SelectedItem.Text.TrimEnd(" ")
            End If
            'Denominacion2: otmardes
            If divDenominacion2.Visible Then
                otmardes = ddlOtmardes.SelectedValue
            End If
            If divmat1.Visible Then
                otmatespe = ddlOtmatespe.SelectedValue
                observ = ddlOtmatespe.SelectedItem.Text

                If ddlTratamiento.SelectedValue = "FABRICACIÓN" Then
                    tratam2 = ddlTratamiento.SelectedValue
                Else
                    Dim qOttratam = "select nemo from ottratam where recno=:recno"
                    Dim pOttatam As New Oracle.ManagedDataAccess.Client.OracleParameter("recno", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, ddlTratamiento.SelectedValue, ParameterDirection.Input)
                    tratam2 = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(qOttratam, strcn, pOttatam)
                    ottratam = ddlTratamiento.SelectedValue
                End If
            Else
                observ = txtCodigo.Text
                tratam2 = txtProveedor.Text
            End If
            'Dureza
            Dim dureza = Nothing, realiza = Nothing
            If divmat3.Visible Then
                dureza = ddlDureza.SelectedValue
                tratam2 = tratam2.ToString.TrimEnd(" ") + " " + ddlDureza.SelectedItem.Text
            End If
            'Tratamiento secundario
            If divmat4.Visible Then
                ottrasec = ddlTraSec.SelectedValue
                If ddlTraSec.SelectedValue > 0 Then
                    observ2 = "T. Sec " + ddlTraSec.SelectedItem.Text
                    realiza = ddlRealiza.SelectedValue
                Else
                    observ2 = "No T. Secundario"
                End If
            End If
            'Medidas
            Dim diametro = Nothing, largo = Nothing, ancho = Nothing, grueso = Nothing
            If txtDiametro.Text.Length > 0 Then : diametro = txtDiametro.Text : End If
            If txtLargo.Text.Length > 0 Then : largo = txtLargo.Text : End If
            If txtAncho.Text.Length > 0 Then : ancho = txtAncho.Text : End If
            If txtGrueso.Text.Length > 0 Then : grueso = txtGrueso.Text : End If
            ''Control de los valores que tienen que ser nulos dentro de las  medidas
            'If rfvDiametro.Enabled Then
            '    'El diametro es obligatorio, por lo tanto el grueso y ancho son nulos
            '    grueso = Nothing : ancho = Nothing
            '    If divNoMat1.Visible Then
            '        'El largo tambien
            '        largo = Nothing
            '    End If
            'End If
            'If rfvGrueso.Enabled Then
            '    'El grueso es obligatorio, por lo tanto el diametro es nulo
            '    diametro = Nothing
            'End If


            'fdfsdf  k



            If Request("mar") Is Nothing Then
                'Añadiendo
                'Asegurarnos de que existe en Cpliscab
                dbAccess.AsegurarExistenciaEnCpliscab(Request("ord"), Request("op"), strcn)

                dbAccess.AddMarca(Request("ord"), Request("op"), txtMarca.Text, Request("tipolista"), material, txtCantidad.Text,
                            lstCplisdeno.First()(0), tratam2, observ, observ2, lstCplisdeno.First()(2), realiza, ddlMateriala.SelectedValue,
                            dureza, ottrasec, ottratam, diametro, largo, ancho, grueso, otmardes, otmatespe, "", "", txtConjunto.Text, strcn)
            Else
                If Request("cp") Is Nothing Then
                    'Editando   
                    dbAccess.EditMarca(Request("ord"), Request("op"), txtMarca.Text, material, txtCantidad.Text,
                                 lstCplisdeno.First()(0), tratam2, observ, observ2, lstCplisdeno.First()(2), realiza, ddlMateriala.SelectedValue,
                                dureza, ottrasec, ottratam, diametro, largo, ancho, grueso, otmardes, otmatespe, txtConjunto.Text, strcn)
                Else
                    'Copiando
                    Dim peso = hpeso.Value
                    Dim norma = hnorma.Value
                    dbAccess.AddMarca(Request("ord"), Request("op"), txtMarca.Text, Request("tipolista"), material,
                            txtCantidad.Text, lstCplisdeno.First()(0), tratam2, observ, observ2, lstCplisdeno.First()(2), realiza,
                            ddlMateriala.SelectedValue, dureza, ottrasec, ottratam, diametro, largo, ancho, grueso, otmardes, otmatespe, peso, norma, txtConjunto.Text, strcn)
                End If
            End If
            Response.Redirect("ListaAF.aspx?tipolista=" + Request("tipolista") + "&ord=" + Request("ord") + "&op=" + Request("op"))
        End If
    End Sub


    Private Sub CargarOtmardes(ByVal lstcplisdeno As String())
        If lstcplisdeno(0) = "S" Or lstcplisdeno(1) = "S" Or lstcplisdeno(2) = "S" Or lstcplisdeno(3) = "S" Or lstcplisdeno(4) = "S" Then
            'Cargar desde otmardes
            Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
            Dim lstOtmardes = dbAccess.GetMateriales2Otmardes(lstcplisdeno(1), lstcplisdeno(2), lstcplisdeno(3), _
                                                                  lstcplisdeno(4), lstcplisdeno(0), strcn)
            For Each i In lstOtmardes
                ddlOtmardes.Items.Add(New ListItem(i(1), i(0)))
            Next
        End If
    End Sub

    Private Sub CargarOtmatespe(ByVal ddl As DropDownList)
        ddl.Items.Clear()
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lst = dbAccess.GetMaterialesOtmatespe(strcn)

        ddl.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", 0))
        For Each i In lst
            ddl.Items.Add(New ListItem(i(1), i(0)))
        Next
    End Sub
    ''' <summary>
    ''' Carga de Tratamientos
    ''' </summary>
    ''' <param name="otmatespeRecno"></param>
    ''' <remarks></remarks>
    Private Sub CargarOttratam(ByVal otmatespeRecno As Integer)
        ddlTratamiento.Items.Clear()
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lst = dbAccess.GetMaterialOtmatespe(otmatespeRecno, strcn)
        Dim lstcplisdeno = dbAccess.GetMaterialesCplisdeno(ddlMateriala.SelectedValue, strcn)
        'Para ser fabricación, tiene que ser, cplisdeno.ottipmarca=3 and otmatespe.fabricación=1
        'Sino, cargar los tratamientos desde ottratam
        If lst.First()(0) = "1" AndAlso lstcplisdeno(5) = "3" Then
            ddlTratamiento.Items.Add("FABRICACIÓN")
        Else
            Dim lstTratamientos = dbAccess.GetTratamientos(3, strcn)
            For Each i In lstTratamientos
                ddlTratamiento.Items.Add(New ListItem(i(1), i(0)))
            Next
        End If
    End Sub
    Private Sub CargarDurezas(ByVal recno As Integer)
        ddlDureza.Items.Clear()
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lstDurezas = dbAccess.GetDurezas(recno, strcn)
        ddlDureza.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", 0))
        For Each i In lstDurezas
            ddlDureza.Items.Add(New ListItem(i(1) + "-" + i(2), i(0)))
        Next
    End Sub

    Public Sub visibility()
        txtMarca.Enabled = Request("mar") Is Nothing OrElse Not Request("cp") Is Nothing
        divmat1.Visible = ddlMateriala.SelectedValue > 0 AndAlso ddlOtmatespe.Items.Count > 0
        divmat2.Visible = divmat1.Visible AndAlso ddlOtmatespe.SelectedValue > 0
        divmat3.Visible = divmat1.Visible AndAlso ddlTratamiento.Items.Count > 1 AndAlso ddlDureza.Items.Count > 0

        divDenominacion2.Visible = ddlOtmardes.Items.Count > 0
        divDenominacion3.Visible = divDenominacion2.Visible AndAlso divDenominacion3.Visible
        'Material desde otmatespe
        divNoMat1.Visible = Not divmat1.Visible AndAlso ddlMateriala.SelectedValue > 0
        'Tratamiento
        divNoMat2.Visible = Not divmat1.Visible AndAlso ddlMateriala.SelectedValue > 0
        'Dureza
        divmat3.Visible = divmat3.Visible AndAlso ddlTratamiento.SelectedValue > 3
        'Tratamiento secundario
        divmat4.Visible = divmat3.Visible AndAlso ddlTratamiento.SelectedValue > 3
        'Realiza
        divmat5.Visible = divmat4.Visible AndAlso ddlTraSec.SelectedValue > 0
        'Medidas
        divLargo.Visible = divmat1.Visible
        divAncho.Visible = divmat1.Visible
        divGrueso.Visible = divmat1.Visible
        'Grueso
        rfvGrueso.Enabled = divmat1.Visible AndAlso (ddlOtmatespe.SelectedValue = 29 Or ddlOtmatespe.SelectedValue = 71 Or ddlOtmatespe.SelectedValue = 44)
        'Diametro
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        rfvDiametro.Enabled = ddlOtmardes.Visible AndAlso (dbAccess.ExistePunzon(ddlOtmardes.SelectedValue, strcn))
        divDiam.Visible = divmat1.Visible Or rfvDiametro.Enabled
    End Sub

    Private Sub cvMarca_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvMarca.ServerValidate
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        If Not Regex.IsMatch(txtMarca.Text, "^[_a-zA-Z\d\.)\(]*[\s]*$") Then
            args.IsValid = False
            Exit Sub
        End If
        If (Request("mar") Is Nothing OrElse (Not Request("cp") Is Nothing AndAlso Request("cp") = True)) AndAlso dbAccess.ExistMarca(Request("ord"), Request("op"), txtMarca.Text, strcn) Then
            args.IsValid = False
        End If
    End Sub

    Private Sub ddlOtmatespeCodigo_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlOtmatespeCodigo.SelectedIndexChanged
        txtCodigo.Text = ddlOtmatespeCodigo.SelectedItem.Text
    End Sub
End Class