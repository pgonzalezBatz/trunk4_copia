Public Partial Class ListaFundicionMarcas
    Inherits System.Web.UI.Page
    Dim strcn As String = ConfigurationManager.ConnectionStrings("LISTAMAT").ConnectionString

    Protected Overrides Sub InitializeCulture()
        If Session("IdTroqueleria") Is Nothing OrElse Session("IdUsuario") Is Nothing OrElse Session("IdCultura") Is Nothing Then
            'No deberia estar aqui
            Response.Redirect("Default.aspx")
        End If
        MyBase.InitializeCulture()
        Culture = Session("IdCultura")
    End Sub

    Private Sub ListaFundicionMarcas_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        If Request("ord") Is Nothing Or Request("op") Is Nothing Then
            Throw New ArgumentNullException("El numero de orden y/o operacion no ha sido proporcionado")
        End If
        'Cargar materiales
        Dim q2 = "select cplisdeno.recno,cplisdeno.descri 
                  from cpcabec inner join fatipope on cpcabec.tipope=fatipope.codope 
                       inner join cplisdeno  on cplisdeno.tiplis=fatipope.tiplis inner join cphorfas on cplisdeno.fase=cphorfas.codfase and cpcabec.numord=cphorfas.numord and cpcabec.numope=cphorfas.numope 
                where cpcabec.nummod=0 and cplisdeno.tipolista=1 and cplisdeno.obsoleto=0 and cpcabec.numord=:ord and cpcabec.numope=:op"
        Dim pmateriales1 = New Oracle.ManagedDataAccess.Client.OracleParameter("ord", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, Request("ord"), ParameterDirection.Input)
        Dim pmateriales2 = New Oracle.ManagedDataAccess.Client.OracleParameter("op", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, Request("op"), ParameterDirection.Input)
        Dim lstMateriales = OracleManagedDirectAccess.seleccionar(Function(r As Oracle.ManagedDataAccess.Client.OracleDataReader) New String() {r("recno").ToString, r("descri").ToString}, q2, strcn, pmateriales1, pmateriales2)
        For Each i In lstMateriales
            ddlMateriala.Items.Add(New ListItem(i(1), i(0)))
        Next
        CargarTratamientos(strcn)
        If Request("mar") Is Nothing Then
            'Añadiendo
        Else
            'Editando marca o copiando
            Dim p1 = New Oracle.ManagedDataAccess.Client.OracleParameter("numord", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, Request("ord"), ParameterDirection.Input)
            Dim p2 = New Oracle.ManagedDataAccess.Client.OracleParameter("numope", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, Request("op"), ParameterDirection.Input)
            Dim p3 = New Oracle.ManagedDataAccess.Client.OracleParameter("nummar", Oracle.ManagedDataAccess.Client.OracleDbType.Char, Request("mar"), ParameterDirection.Input)
            Dim q = "select nummar,cannec,material,observ,tratam,tratam2,observ2,realiza,cplisdeno,ottratam,otdureza,ottrasec,peso,norma,conjunto from cplismat where numord=:numord and numope=:numope  and nummar=:nummar and tipolista=1"
            Dim lstMarcas As List(Of String()) = OracleManagedDirectAccess.seleccionar(Function(r As Oracle.ManagedDataAccess.Client.OracleDataReader) New String() {r("nummar").ToString, r("cannec").ToString,
                                                                                       r("material").ToString, r("observ").ToString, r("tratam").ToString, r("tratam2").ToString, r("observ2").ToString, r("realiza").ToString, r("cplisdeno").ToString, r("ottratam").ToString,
                                                                                        r("otdureza").ToString, r("ottrasec").ToString, r("peso").ToString, r("norma").ToString, r("conjunto").ToString}, q, strcn, p1, p2, p3)
            If Not lstMarcas Is Nothing AndAlso lstMarcas.Count = 1 Then
                'Bind Materiales y tratamientos temple
                If Request("cp") Is Nothing Then
                    'Editando
                    txtMarca.Text = lstMarcas.First()(0)
                Else
                    'Copiando
                    hpeso.Value = lstMarcas.First()(12)
                    hnorma.Value = lstMarcas.First()(13)
                End If
                ddlMateriala.SelectedValue = lstMarcas.First()(8)
                txtCantidad.Text = lstMarcas.First()(1)
                txtNombre.Text = lstMarcas.First()(2)
                txtConjunto.Text = lstMarcas.First()(14)
                'Tratamiento temple
                If lstMarcas.First()(9) <> String.Empty Then
                    'Dureza?
                    ddlTTemple.SelectedValue = lstMarcas.First()(9)
                    CargarDurezas(strcn, lstMarcas.First()(9))
                    ddlDureza.SelectedValue = lstMarcas.First()(10)
                    'Tratamiento secundario
                    If lstMarcas.First()(11) <> String.Empty Then
                        'Acitvar tratamientos secundarios y cargalos
                        CargarTratamientosSecundarios(strcn)
                        ddlTSecundario.SelectedValue = lstMarcas.First()(11)
                        'Quien realiza el tratamiento secundario?
                        If lstMarcas.First()(7) <> String.Empty Then
                            ddlRealiza.SelectedValue = lstMarcas.First()(7)
                        End If
                    End If
                End If
            End If
        End If
        Visibility()
    End Sub
    Public Sub CargarTratamientos(ByVal strcn As String)
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lstTratamientos = dbAccess.GetTratamientos(1, strcn)
        ddlTTemple.Items.Add(New ListItem("Seleccionar", 0))
        For Each i In lstTratamientos
            ddlTTemple.Items.Add(New ListItem(i(1), i(0)))
        Next
    End Sub
    Public Sub CargarDurezas(ByVal strcn As String, ByVal recno As Integer)
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lstDurezas = dbAccess.GetDurezas(recno, strcn)
        ddlDureza.Items.Add(New ListItem("[" + h.traducir("Seleccionar") + "]", 0))
        For Each i In lstDurezas
            ddlDureza.Items.Add(New ListItem(i(1) + "-" + i(2), i(0)))
        Next
    End Sub
    Public Sub CargarTratamientosSecundarios(ByVal strcn As String)
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
        Dim lstTratamientosSecundarios = dbAccess.GetTratamientosSecundarios(strcn)
        ddlTSecundario.Items.Add(New ListItem(h.traducir("Ninguno"), 0))
        For Each i In lstTratamientosSecundarios
            ddlTSecundario.Items.Add(New ListItem(i(1), i(0)))
        Next
    End Sub

    Private Sub ddlTTemple_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTTemple.SelectedIndexChanged
        ddlDureza.Items.Clear()
        CargarTratamientosSecundarios(strcn)
        CargarDurezas(strcn, ddlTTemple.SelectedValue)
        Visibility()
    End Sub

    Private Sub Visibility()
        txtMarca.Enabled = Request("mar") Is Nothing OrElse Not Request("cp") Is Nothing
        divDureza.Visible = ddlTTemple.SelectedValue <> "" AndAlso ddlTTemple.SelectedValue <> "0" AndAlso ddlTTemple.SelectedValue <> "1"
        divTratSec.Visible = divDureza.Visible
        divRealiza.Visible = divTratSec.Visible AndAlso ddlTSecundario.SelectedValue.Length > 0 AndAlso ddlTSecundario.SelectedValue > 0
    End Sub

    Protected Sub Save(ByVal sender As Object, ByVal e As EventArgs)
        If Page.IsValid Then
            Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()
            'Cascaria si no existiera entrada en CPLISCAB. Pero esto no deberia de pasar nunca
            Dim qCplisdeno = "select dureza,descri,fase from cplisdeno where recno=:recno"
            Dim pCplisdeno As New Oracle.ManagedDataAccess.Client.OracleParameter("recno", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, ddlMateriala.SelectedValue, ParameterDirection.Input)
            Dim lstCplisdeno As List(Of String()) = OracleManagedDirectAccess.seleccionar(Function(r As Oracle.ManagedDataAccess.Client.OracleDataReader) New String() {r("dureza").ToString, r("descri").ToString, r("fase").ToString}, qCplisdeno, strcn, pCplisdeno)
            If lstCplisdeno Is Nothing OrElse lstCplisdeno.Count <> 1 Then
                Throw New ArgumentNullException("lstCPlisdeno", "lstCPlisdeno es nulo o esta vacio")
            End If
            Dim qOttratam = "select nemo from ottratam where recno=:recno"
            Dim pOttatam As New Oracle.ManagedDataAccess.Client.OracleParameter("recno", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, ddlTTemple.SelectedValue, ParameterDirection.Input)
            Dim v = OracleManagedDirectAccess.SeleccionarEscalar(Of String)(qOttratam, strcn, pOttatam).ToString.TrimEnd(" ")




            Dim observ2 = Nothing, realiza = Nothing, dureza = Nothing, ottrasec = Nothing, ottratam = Nothing
            'Tratamiento temple
            If ddlTTemple.SelectedValue > 0 Then
                ottratam = ddlTTemple.SelectedValue
            End If

            If divTratSec.Visible Then
                ottrasec = ddlTSecundario.SelectedValue
                If ddlTSecundario.SelectedValue > 0 Then
                    realiza = ddlRealiza.SelectedValue
                    observ2 = "T. Sec " + ddlTSecundario.SelectedItem.Text
                Else
                    observ2 = "No T. Secundario"
                End If
            End If
            'Dureza
            If divDureza.Visible Then
                dureza = ddlDureza.SelectedValue
                Dim qdureza = "select n1,n2 from otdureza where recno=:recno"
                Dim lstOtdureza As New List(Of Oracle.ManagedDataAccess.Client.OracleParameter)
                lstOtdureza.Add(New Oracle.ManagedDataAccess.Client.OracleParameter("recno", Oracle.ManagedDataAccess.Client.OracleDbType.Int32, dureza, ParameterDirection.Input))
                Dim dur = OracleManagedDirectAccess.seleccionar(Function(r As Oracle.ManagedDataAccess.Client.OracleDataReader) New String() {r("n1").ToString, r("n2").ToString}, qdureza, strcn, lstOtdureza.ToArray)(0)
                v = v + " " + dur(0) + "-" + dur(1)
            End If
            If Request("mar") Is Nothing Then
                'Añadiendo
                'Asegurarnos de que existe en Cpliscab
                dbAccess.AsegurarExistenciaEnCpliscab(Request("ord"), Request("op"), strcn)
                dbAccess.AddMarca(Request("ord"), Request("op"), txtMarca.Text, 1, txtNombre.Text, txtCantidad.Text, lstCplisdeno.First()(0), v,
                           lstCplisdeno.First()(1), observ2, lstCplisdeno.First()(2), realiza, ddlMateriala.SelectedValue, dureza, ottrasec,
                        ottratam, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, "", "", txtConjunto.Text, strcn)
            Else
                If Request("cp") Is Nothing Then
                    'Editando
                    dbAccess.EditMarca(Request("ord"), Request("op"), txtMarca.Text, txtNombre.Text, txtCantidad.Text, lstCplisdeno.First()(0), v,
                           lstCplisdeno.First()(1), observ2, lstCplisdeno.First()(2), realiza, ddlMateriala.SelectedValue, dureza, ottrasec,
                        ottratam, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, txtConjunto.Text, strcn)
                Else
                    'Copiando
                    dbAccess.AddMarca(Request("ord"), Request("op"), txtMarca.Text, 1, txtNombre.Text, txtCantidad.Text, lstCplisdeno.First()(0), v,
                           lstCplisdeno.First()(1), observ2, lstCplisdeno.First()(2), realiza, ddlMateriala.SelectedValue, dureza, ottrasec,
                        ottratam, Nothing, Nothing, Nothing, Nothing, Nothing, Nothing, hpeso.Value, hnorma.Value, txtConjunto.Text, strcn)
                End If
            End If
        End If
        Response.Redirect("ListaFundicion.aspx?ord=" + Request("ord") + "&op=" + Request("op"))
    End Sub
 

    Private Sub ddlTSecundario_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles ddlTSecundario.SelectedIndexChanged
        Visibility()
    End Sub

    ''' <summary>
    ''' Validacion de la marca
    ''' </summary>
    ''' <param name="source"></param>
    ''' <param name="args"></param>
    ''' <remarks></remarks>
    Private Sub cvMarca_ServerValidate(ByVal source As Object, ByVal args As System.Web.UI.WebControls.ServerValidateEventArgs) Handles cvMarca.ServerValidate
        Dim dbAccess As ListamatLib.IDBAccess = Utilities.GetIDBInstance()

        If Not Regex.IsMatch(txtMarca.Text, "^[_a-zA-Z\d\.)\(]*[\s]*$") Then
            args.IsValid = False
            Exit Sub
        End If
        If (Request("mar") Is Nothing OrElse (Not Request("cp") Is Nothing AndAlso Request("cp") = True)) AndAlso dbAccess.ExistMarca(Request("ord"), Request("op"), txtMarca.Text, strcn) Then
            args.IsValid = False
        Else
            args.IsValid = True
        End If
    End Sub
End Class
