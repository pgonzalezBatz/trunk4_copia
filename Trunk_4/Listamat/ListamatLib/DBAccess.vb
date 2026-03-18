Imports Oracle.ManagedDataAccess.Client
Public Class DBAccess
    Implements IDBAccess

    Public Function GetOFOPs(ByVal codPro As String, ByVal strCn As String) As List(Of String()) Implements IDBAccess.GetOFOPs
        Dim q = "select numord,numope from otpropro where codpro=:codpro"
        Dim p1 = New OracleParameter("codpro", OracleDbType.Char, codPro, ParameterDirection.Input)
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("numord").ToString, r("numope").ToString}, q, strCn, p1)
    End Function

    Public Sub EliminarMarca(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal mar As String, ByVal strcn As String) Implements IDBAccess.EliminarMarca
        Dim q = "delete from cplismat where tipolista=:tipolista and numord=:ord and numope=:op and nummar=:mar"
        Dim p As New OracleParameter("tipolista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        Dim p1 As New OracleParameter("ord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("op", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim p3 As New OracleParameter("mar", OracleDbType.Char, mar, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strcn, p, p1, p2, p3)
    End Sub

    Public Sub LanzarMarca(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal mar As String, ByVal strcn As String) Implements IDBAccess.LanzarMarca
        Dim q = "update cplismat set fecdef=:fecdef where tipolista=:tipolista and numord=:ord and numope=:op and nummar=:mar"
        Dim p0 As New OracleParameter("fecdef", OracleDbType.Date, DateTime.Now, ParameterDirection.Input)
        Dim p1 As New OracleParameter("tipolista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        Dim p2 As New OracleParameter("ord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p3 As New OracleParameter("op", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim p4 As New OracleParameter("mar", OracleDbType.Char, mar, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strcn, p0, p1, p2, p3, p4)
    End Sub
    Public Sub ActualizarOtpropro(ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) Implements IDBAccess.ActualizarOtpropro
        Dim q = "update otpropro set fecha_lanz = :fecdef where numord = :ord and numope = :op"
        Dim p0 As New OracleParameter("fecdef", OracleDbType.Date, DateTime.Now, ParameterDirection.Input)
        Dim p1 As New OracleParameter("ord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("op", OracleDbType.Int32, op, ParameterDirection.Input)
        OracleManagedDirectAccess.NoQuery(q, strCn, p0, p1, p2)
    End Sub

    Public Sub OrdenarMarcas(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal strcn As String) Implements IDBAccess.OrdenarMarcas
        Dim cn As New OracleConnection(strcn)
        Dim p1 As New OracleParameter("wnumord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("wnumope", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim p3 As New OracleParameter("wlista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        cn.Open()
        OracleManagedDirectAccess.NoQueryProcedure("OrdenaMarca", cn, p1, p2, p3)
        cn.Close()
    End Sub

    Public Sub DuplicarLista(ByVal ordSource As Integer, ByVal opSource As Integer, ByVal ordDestination As Integer, ByVal opDestination As Integer, ByVal strCn As String) Implements IDBAccess.DuplicarLista
        Dim p1 = New OracleParameter("numordD", OracleDbType.Int32, ordDestination, ParameterDirection.Input)
        Dim p2 = New OracleParameter("numopeD", OracleDbType.Int32, opDestination, ParameterDirection.Input)
        Dim p3 = New OracleParameter("numordS", OracleDbType.Int32, ordSource, ParameterDirection.Input)
        Dim p4 = New OracleParameter("numopeS", OracleDbType.Int32, opSource, ParameterDirection.Input)
        Dim q = "insert into cplismat(NUMORD,NUMOPE,NUMMAR,numlista,TIPOLISTA,MATERIAL,CANNEC,CANTIMPUT,FECIMPUT,almacen,DIAMETRO,LARGO," _
        + "ANCHO,GRUESO,TRATAM,TRATAM2,OBSERV,OBSERV2,FASE,LARGDEF,ANCHDEF,GRUEDEF,REALIZA,CPLISDENO,OTMARDES,ORDEN,OTDUREZA,OTTRASEC," _
        + "OTTRATAM,OTMATESPE,REF_CLI,conjunto) SELECT :numordD,:numopeD,NUMMAR,0,TIPOLISTA,MATERIAL,CANNEC,CANTIMPUT,FECIMPUT,'N'," _
        + "DIAMETRO,LARGO,ANCHO,GRUESO,TRATAM,TRATAM2,OBSERV,OBSERV2,FASE, LARGDEF, ANCHDEF, GRUEDEF, REALIZA, CPLISDENO, OTMARDES, " _
        + "ORDEN, OTDUREZA, OTTRASEC, OTTRATAM, OTMATESPE, REF_CLI,conjunto FROM cplismat where numord=:numordS and numope=:numopeS"
        OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4)
    End Sub

    Public Function GetMarcasSinLanzar(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) As System.Collections.Generic.List(Of String()) Implements IDBAccess.GetMarcasSinLanzar
        Dim p1 = New OracleParameter("numord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 = New OracleParameter("numope", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim p3 = New OracleParameter("tipolista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        Dim q = "select nummar,cannec,material,observ,tratam,tratam2,observ2,realiza,diametro,largo,ancho,grueso,d.n1,d.n2,c.peso,c.norma,c.conjunto 
                 from cplismat c left outer join otdureza d on c.otdureza=d.recno 
                 where numord=:numord and  numope=:numope and tipolista=:tipolista and fecdef is null and nummar<>'ZZZZ' 
                 order by orden"
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("nummar").ToString, r("cannec").ToString, r("material").ToString, r("observ").ToString,
                                                      r("tratam").ToString, r("tratam2").ToString, r("observ2").ToString, r("realiza").ToString, r("diametro").ToString, r("largo").ToString, r("ancho").ToString,
                                                     r("grueso").ToString, r("n1").ToString, r("n2").ToString, r("peso").ToString, r("norma").ToString, r("conjunto").ToString}, q, strCn, p1, p2, p3)
    End Function

    Public Function GetMaterialesCplisdeno(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) As List(Of String()) Implements IDBAccess.GetMaterialesCplisdeno
        Dim q2 = "select cplisdeno.recno,cplisdeno.descri from cpcabec inner join fatipope on cpcabec.tipope=fatipope.codope inner join cplisdeno 
        On cplisdeno.tiplis=fatipope.tiplis inner join cphorfas on cplisdeno.fase=cphorfas.codfase and cpcabec.numord=cphorfas.numord 
                 And cpcabec.numope = cphorfas.numope                 where cpcabec.nummod=0 And cplisdeno.tipolista=:tipolista and cplisdeno.obsoleto=0 and cpcabec.numord=:ord and cpcabec.numope=:op
        order by cplisdeno.descri"
        Dim pmateriales1 = New OracleParameter("ord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim pmateriales2 = New OracleParameter("op", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim pmateriales = New OracleParameter("tipolista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("recno").ToString, r("descri").ToString}, q2, strCn, pmateriales, pmateriales1, pmateriales2)
    End Function

    Public Function GetMaterialCplisdeno(ByVal recno As Integer, ByVal strCn As String) As String() Implements IDBAccess.GetMaterialesCplisdeno
        Dim q = "Select carro, taquerio, standard, electrico, guiado, ottipmarca from cplisdeno where recno=: recno"
        Dim p = New OracleParameter("recno", OracleDbType.Int32, recno, ParameterDirection.Input)
        Dim lstCpelisdeno = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("carro").ToString, r("taquerio").ToString, r("standard").ToString,
                                                                  r("electrico").ToString, r("guiado").ToString, r("ottipmarca").ToString}, q, strCn, p)
        If lstCpelisdeno Is Nothing OrElse lstCpelisdeno.Count <> 1 Then
            Throw New Exception("Se ha producido un error al cargar la lista de denominaciones de cplisdeno. recno:" + recno)
        End If
        Return lstCpelisdeno.First()
    End Function

    Public Function GetMarcas(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) As System.Collections.Generic.List(Of String()) Implements IDBAccess.GetMarcas
        Dim p1 = New OracleParameter("numord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 = New OracleParameter("numope", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim p3 = New OracleParameter("tipolista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        Dim q = "select m.nummar,m.cannec,m.material,m.observ,m.tratam,m.tratam2,m.observ2,m.realiza,m.fecdef,m.fecnec,h.fecini,m.diametro,
                m.largo,m.ancho,m.grueso,d.n1,d.n2,m.conjunto from cplismat m inner join cphorfas h on m.numord=h.numord and m.numope=h.numope and 
                m.fase=h.codfase left outer join otdureza d on m.otdureza=d.recno where m.numord=:numord and m.numope=:numope and m.tipolista=:tipolista and m.nummar<>'ZZZZ' order by orden"
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("nummar").ToString, r("cannec").ToString, r("material").ToString,
                                                                  r("observ").ToString, r("tratam").ToString, r("tratam2").ToString, r("observ2").ToString, r("realiza").ToString, r("fecdef").ToString, r("fecini").ToString,
                                                                  r("diametro").ToString, r("largo").ToString, r("ancho").ToString, r("grueso").ToString, r("n1").ToString, r("n2").ToString, r("conjunto").ToString}, q, strCn, p1, p2, p3)
    End Function

    Public Function GetMarcasEnComun(ByVal ord1 As Integer, ByVal op1 As Integer, ByVal ord2 As Integer, ByVal op2 As Integer, ByVal strCn As String) As List(Of String()) Implements IDBAccess.GetMarcasEnComun
        Dim p1 = New OracleParameter("ord1", OracleDbType.Int32, ord1, ParameterDirection.Input)
        Dim p2 = New OracleParameter("op1", OracleDbType.Int32, op1, ParameterDirection.Input)
        Dim p3 = New OracleParameter("ord2", OracleDbType.Int32, ord2, ParameterDirection.Input)
        Dim p4 = New OracleParameter("op2", OracleDbType.Int32, op2, ParameterDirection.Input)
        Dim q = "select c1.numord from cplismat C1 inner join cplismat c2 on c1.numord = c2.numord And c1.numope = c2.numope And c1.nummar = c2.nummar " _
                + "where c1.numord=:ord1 and c1.numope=:op1 and c2.numord=:ord2 and c2.numope=:op2"
        'TODO:Check this
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("numord").ToString}, q, strCn, p1, p2, p3, p4)
    End Function

    Public Function GetMarca(ByVal tipolista As Integer, ByVal ord As Integer, ByVal op As Integer, ByVal mar As String, ByVal strCn As String) As String() Implements IDBAccess.GetMarca
        Dim p1 = New OracleParameter("numord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 = New OracleParameter("numope", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim p3 = New OracleParameter("nummar", OracleDbType.Char, mar, ParameterDirection.Input)
        Dim p4 = New OracleParameter("tipolista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        Dim q = "select nummar,cannec,material,observ,tratam,tratam2,observ2,realiza,cplisdeno,ottratam,otdureza,ottrasec,otmardes,otmatespe,
        diametro,largo,ancho,grueso,peso,norma, conjunto from cplismat 
        where numord=:numord and numope=:numope  and nummar=:nummar and tipolista=:tipolista"

        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("nummar").ToString, r("cannec").ToString, r("material").ToString, r("observ").ToString, r("tratam").ToString,
                                                        r("tratam2").ToString, r("observ2").ToString, r("realiza").ToString, r("cplisdeno").ToString, r("ottratam").ToString, r("otdureza").ToString,
                                                        r("ottrasec").ToString, r("otmardes").ToString, r("otmatespe").ToString, r("diametro").ToString, r("largo").ToString, r("ancho").ToString,
                                                        r("grueso").ToString, r("peso").ToString, r("norma").ToString, r("conjunto").ToString}, q, strCn, p1, p2, p3, p4)
        If lst Is Nothing OrElse lst.Count <> 1 Then
            Throw New Exception("Se ha producido un error al seleccionar la marca desde cplismat. tipolista=" + tipolista.ToString + " of=" + _
                            ord.ToString + " op=" + op.ToString + "marca=" + mar)
        End If
        Return lst.First()
    End Function
    Public Function ExistMarca(ByVal ord As Integer, ByVal op As Integer, ByVal mar As String, ByVal strCn As String) As Boolean Implements IDBAccess.ExistMarca
        Dim p1 = New OracleParameter("numord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 = New OracleParameter("numope", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim p3 = New OracleParameter("nummar", OracleDbType.Char, mar, ParameterDirection.Input)
        Dim q = "select nummar,cannec,material,observ,tratam,tratam2,observ2,realiza,cplisdeno,ottratam,otdureza,ottrasec,otmardes,otmatespe,
        diametro,largo,ancho,grueso from cplismat         where numord=:numord and numope=:numope  and nummar=:nummar"

        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("nummar").ToString, r("cannec").ToString, r("material").ToString, r("observ").ToString, r("tratam").ToString,
                                                        r("tratam2").ToString, r("observ2").ToString, r("realiza").ToString, r("cplisdeno").ToString, r("ottratam").ToString, r("otdureza").ToString,
                                                        r("ottrasec").ToString, r("otmardes").ToString, r("otmatespe").ToString, r("diametro").ToString, r("largo").ToString, r("ancho").ToString,
                                                        r("grueso").ToString}, q, strCn, p1, p2, p3)
        If lst Is Nothing OrElse lst.Count < 1 Then
            Return False
        End If
        Return True
    End Function

    Public Function GetMaterialesOtmatespe(ByVal strCn As String) As List(Of String()) Implements IDBAccess.GetMaterialesOtmatespe
        Dim q = "select recno,codigo from otmatespe where obsoleto=0 order by codigo"
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("recno").ToString, r("codigo").ToString}, q, strCn)
    End Function
    Public Function GetMaterialOtmatespe(ByVal recno As Integer, ByVal strCn As String) As String() Implements IDBAccess.GetMaterialOtmatespe
        Dim q = "select fabricacion from otmatespe where recno=:recno"
        Dim p As New OracleParameter("recno", OracleDbType.Int32, recno, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("fabricacion").ToString}, q, strCn, p)
        'Fabricación o cargar los tratamientos secundarios?
        If lst Is Nothing OrElse lst.Count <> 1 Then
            Throw New Exception("Se ha producido un error al seleccionar el material desde otmatespe. Recno=" + recno.ToString)
        End If
        Return lst.First()
    End Function

    Public Function GetTratamientos(ByVal tipolista As Integer, ByVal strCn As String) As List(Of String()) Implements IDBAccess.GetTratamientos
        Dim q = "select recno,descri from ottratam where tipolista=:tipolista"
        Dim p As New OracleParameter("tipolista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("recno").ToString, r("descri").ToString}, q, strCn, p)
    End Function

    Public Function GetDurezas(ByVal recno As Integer, ByVal strCn As String) As List(Of String()) Implements IDBAccess.GetDurezas
        'Dim q = "select d.recno,d.n1,d.n2 from otdureza d inner join ottratam t on d.tipo=t.tipo where t.recno=:recno and d.obsoleto=0"
        Dim q = "select d.recno,d.n1,d.n2 from otdureza d inner join ottratam t on d.tipo=t.tipo where t.recno=:recno and d.obsoleto=0"
        Dim p = New OracleParameter("recno", OracleDbType.Int32, recno, ParameterDirection.Input)
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("recno").ToString, r("n1").ToString, r("n2").ToString}, q, strCn, p)
    End Function

    Public Function GetTratamientosSecundarios(ByVal strCn As String) As List(Of String()) Implements IDBAccess.GetTratamientosSecundarios
        Dim q = "select recno,descri from ottrasec order by descri"
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("recno").ToString, r("descri").ToString}, q, strCn)
    End Function

    Public Function GetMateriales2Otmardes(ByVal t As String, ByVal s As String, ByVal e As String, ByVal g As String, ByVal c As String, ByVal strCn As String) As List(Of String()) Implements IDBAccess.GetMateriales2Otmardes
        Dim q As New Text.StringBuilder
        q.Append("select recno,descri,anadido from otmardes where 1=1")
        If t = "S" Then
            q.Append(" and taquerio='S'")
        End If
        If s = "S" Then
            q.Append(" and standard='S'")
        End If
        If e = "S" Then
            q.Append(" and electrico='S'")
        End If
        If g = "S" Then
            q.Append(" and guiado='S'")
        End If
        If c = "S" Then
            q.Append(" and carro='S'")
        End If
        q.Append(" order by descri")
        Return OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("recno").ToString, r("descri").ToString, r("anadido").ToString}, q.ToString, strCn)
    End Function

    Public Function GetMateriales2Otmardes(ByVal recno As Integer, ByVal strCn As String) As String() Implements IDBAccess.GetMateriales2Otmardes
        Dim q = "select anadido from otmardes where recno=:recno"
        Dim p As New OracleParameter("recno", OracleDbType.Int32, recno, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("anadido").ToString}, q, strCn, p)
        If lst Is Nothing Or lst.Count <> 1 Then
            Throw New Exception("No se ha podido seleccionar el registro desde otmardes." + " recno=" + recno.ToString)
        End If
        Return lst.First()
    End Function

    Public Sub AddMarca(ByVal ord As Integer, ByVal op As Integer, ByVal marca As String, ByVal tipolista As Integer, ByVal material As String,
                        ByVal cannec As Integer, ByVal tratam As String, ByVal tratam2 As String, ByVal observ As String, ByVal observ2 As Object,
                        ByVal fase As String, ByVal realiza As String, ByVal cplisdenoRecno As Integer, ByVal ordurezaRecno As Object,
                        ByVal ottrasecRecno As Object, ByVal ottratam As Object, ByVal diametro As Object, ByVal largo As Object,
                        ByVal ancho As Object, ByVal grueso As Object, ByVal otmardesRecno As Object, ByVal otmatespeRecno As Object, ByVal peso As String, ByVal norma As String, conjunto As String,
                        ByVal strCn As String) Implements IDBAccess.AddMarca
        Dim q = "insert into cplismat(numord,numope,nummar,numlista,tipolista,material,cannec,diametro,largo,ancho,grueso,almacen,tratam," _
        + "tratam2,observ,observ2,fase,realiza,cplisdeno,otmardes,otdureza,ottrasec,ottratam,otmatespe,ref_cli,peso,norma,conjunto) " _
        + "values(:ord,:ope,:mar,0,:tipolista,:material,:cannec,:diametro,:largo,:ancho,:grueso,'N',:tratam,:tratam2,:observ,:observ2,:fase," _
        + ":realiza,:cplisdeno,:otmardes,:otdureza,:ottrasec,:ottratam,:otmatespe,:refcli,:peso,:norma,:conjunto)"
        Dim pOrd As New OracleParameter("ord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim pOp As New OracleParameter("ope", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim pMar As New OracleParameter("mar", OracleDbType.Char, ParameterDirection.Input)
        pMar.Value = marca : pMar.Size = 11
        Dim pTipolista As New OracleParameter("tipolista", OracleDbType.Int32, tipolista, ParameterDirection.Input)
        Dim pMaterial As New OracleParameter("material", OracleDbType.Char, ParameterDirection.Input)
        pMaterial.Value = material : pMaterial.Size = 20
        Dim pCannec As New OracleParameter("cannec", OracleDbType.Int32, ParameterDirection.Input)
        pCannec.Value = cannec

        Dim pDiametro As New OracleParameter("diametro", OracleDbType.Int32, ParameterDirection.Input)
        If diametro Is Nothing Then : pDiametro.Value = DBNull.Value : Else : pDiametro.Value = diametro : End If
        Dim pLargo As New OracleParameter("largo", OracleDbType.Int32, ParameterDirection.Input)
        If largo Is Nothing Then : pLargo.Value = DBNull.Value : Else : pLargo.Value = largo : End If
        Dim pAncho As New OracleParameter("ancho", OracleDbType.Int32, ParameterDirection.Input)
        If ancho Is Nothing Then : pAncho.Value = DBNull.Value : Else : pAncho.Value = ancho : End If
        Dim pGrueso As New OracleParameter("grueso", OracleDbType.Int32, ParameterDirection.Input)
        If grueso Is Nothing Then : pGrueso.Value = DBNull.Value : Else : pGrueso.Value = grueso : End If

        Dim ptratam As New OracleParameter("tratam", OracleDbType.Char, ParameterDirection.Input)
        ptratam.Value = tratam : ptratam.Size = 20
        Dim pTratam2 As New OracleParameter("tratam2", OracleDbType.Char, ParameterDirection.Input)
        pTratam2.Value = tratam2 : pTratam2.Size = 20
        Dim pObserv As New OracleParameter("observ", OracleDbType.Varchar2, ParameterDirection.Input)
        pObserv.Value = observ : pObserv.Size = 80
        Dim pObserv2 As New OracleParameter("observ2", OracleDbType.Char, ParameterDirection.Input)
        Dim pFase As New OracleParameter("fase", OracleDbType.Char, ParameterDirection.Input)
        pFase.Value = fase : pFase.Size = 4
        Dim pRealiza As New OracleParameter("realiza", OracleDbType.Char, ParameterDirection.Input)
        Dim pCplisdeno As New OracleParameter("cplisdeno", OracleDbType.Int32, cplisdenoRecno, ParameterDirection.Input)
        Dim pOtmardes As New OracleParameter("otmardes", OracleDbType.Int32, cplisdenoRecno, ParameterDirection.Input)
        If otmardesRecno Is Nothing Then : pOtmardes.Value = DBNull.Value : Else : pOtmardes.Value = otmardesRecno : End If
        Dim pOtdureza As New OracleParameter("otdureza", OracleDbType.Int32, ParameterDirection.Input)
        Dim pOttrasec As New OracleParameter("ottrasec", OracleDbType.Int32, ParameterDirection.Input)
        Dim pOttratam As New OracleParameter("ottratam", OracleDbType.Int32, ParameterDirection.Input)
        If ottratam Is Nothing Then : pOttratam.Value = DBNull.Value : Else : pOttratam.Value = ottratam : End If
        Dim pOtmatespe As New OracleParameter("otmatespe", OracleDbType.Int32, ParameterDirection.Input)
        If pOtmatespe Is Nothing Then : pOtmatespe.Value = DBNull.Value : Else : pOtmatespe.Value = otmatespeRecno : End If
        Dim pRefcli As New OracleParameter("refcli", OracleDbType.Varchar2,
                                               Text.RegularExpressions.Regex.Replace(observ, "[\.\,\-\ ]", ""), ParameterDirection.Input)
        Dim pPeso As New OracleParameter("peso", OracleDbType.Int32, ParameterDirection.Input)
        Dim pNorma As New OracleParameter("norma", OracleDbType.NVarchar2, ParameterDirection.Input)
        Dim pConjunto As New OracleParameter("conjunto", OracleDbType.Varchar2, ParameterDirection.Input)
        pConjunto.Size = 10
        pConjunto.Value = If(conjunto, DBNull.Value)
        If IsNumeric(peso) Then : pPeso.Value = peso : Else : pPeso.Value = DBNull.Value : End If
        If norma.Length > 0 Then : pNorma.Value = norma : Else : pNorma.Value = DBNull.Value : End If

        If ottrasecRecno Is Nothing Then
            pObserv2.Value = DBNull.Value : pObserv2.Size = 20
            pRealiza.Value = DBNull.Value
            pOttrasec.Value = DBNull.Value
        Else
            pObserv2.Value = observ2 : pObserv2.Size = 20
            pRealiza.Value = realiza : pRealiza.Size = 1
            pOttrasec.Value = ottrasecRecno
        End If

        If ottratam > 1 Then
            pOtdureza.Value = ordurezaRecno
        Else
            pOtdureza.Value = DBNull.Value
        End If
        OracleManagedDirectAccess.NoQuery(q, strCn, pOrd, pOp, pMar, pTipolista, pMaterial, pCannec, pDiametro, pLargo, pAncho, pGrueso,
                                             ptratam, pTratam2, pObserv, pObserv2, pFase, pRealiza, pCplisdeno, pOtmardes, pOtdureza, pOttrasec,
                                             pOttratam, pOtmatespe, pRefcli, pPeso, pNorma, pConjunto)
    End Sub

    Public Sub EditMarca(ByVal ord As Integer, ByVal op As Integer, ByVal marca As String, ByVal material As String,
                        ByVal cannec As Integer, ByVal tratam As String, ByVal tratam2 As String, ByVal observ As String, ByVal observ2 As Object,
                        ByVal fase As String, ByVal realiza As String, ByVal cplisdenoRecno As Integer, ByVal ordurezaRecno As Object,
                        ByVal ottrasecRecno As Object, ByVal ottratam As Object, ByVal diametro As Object, ByVal largo As Object,
                        ByVal ancho As Object, ByVal grueso As Object, ByVal otmardesRecno As Object, ByVal otmatespeRecno As Object, conjunto As String,
                        ByVal strCn As String) Implements IDBAccess.EditMarca
        Dim q = "update cplismat set material=:material,cannec=:cannec,diametro=:diametro,largo=:largo,ancho=:ancho,grueso=:grueso," _
        + "tratam=:tratam,tratam2=:tratam2,observ=:observ,observ2=:observ2,fase=:fase,realiza=:realiza,cplisdeno=:cplisdeno," _
        + "otmardes=:otmardes,otdureza=:otdureza,ottrasec=:ottrasec,ottratam=:ottratam,otmatespe=:otmatespe,ref_cli=:ref_cli, conjunto=:conjunto 
        where numord=:ord and numope=:op and nummar=:mar"
        Dim pOrd As New OracleParameter("ord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim pOp As New OracleParameter("op", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim pMar As New OracleParameter("mar", OracleDbType.Char, ParameterDirection.Input)
        pMar.Value = marca : pMar.Size = 11
        Dim pMaterial As New OracleParameter("material", OracleDbType.Char, ParameterDirection.Input)
        pMaterial.Value = material : pMaterial.Size = 20
        Dim pCannec As New OracleParameter("cannec", OracleDbType.Int32, ParameterDirection.Input)
        pCannec.Value = cannec

        Dim pDiametro As New OracleParameter("diametro", OracleDbType.Int32, ParameterDirection.Input)
        If diametro Is Nothing Then : pDiametro.Value = DBNull.Value : Else : pDiametro.Value = diametro : End If
        Dim pLargo As New OracleParameter("largo", OracleDbType.Int32, ParameterDirection.Input)
        If largo Is Nothing Then : pLargo.Value = DBNull.Value : Else : pLargo.Value = largo : End If
        Dim pAncho As New OracleParameter("ancho", OracleDbType.Int32, ParameterDirection.Input)
        If ancho Is Nothing Then : pAncho.Value = DBNull.Value : Else : pAncho.Value = ancho : End If
        Dim pGrueso As New OracleParameter("grueso", OracleDbType.Int32, ParameterDirection.Input)
        If grueso Is Nothing Then : pGrueso.Value = DBNull.Value : Else : pGrueso.Value = grueso : End If

        Dim ptratam As New OracleParameter("tratam", OracleDbType.Char, ParameterDirection.Input)
        ptratam.Value = tratam : ptratam.Size = 20
        Dim pTratam2 As New OracleParameter("tratam2", OracleDbType.Char, ParameterDirection.Input)
        pTratam2.Value = tratam2 : pTratam2.Size = 20
        Dim pObserv As New OracleParameter("observ", OracleDbType.Varchar2, ParameterDirection.Input)
        pObserv.Value = observ : pObserv.Size = 80
        Dim pObserv2 As New OracleParameter("observ2", OracleDbType.Char, ParameterDirection.Input)
        Dim pFase As New OracleParameter("fase", OracleDbType.Char, ParameterDirection.Input)
        pFase.Value = fase : pFase.Size = 4
        Dim pRealiza As New OracleParameter("realiza", OracleDbType.Char, ParameterDirection.Input)
        Dim pCplisdeno As New OracleParameter("cplisdeno", OracleDbType.Int32, cplisdenoRecno, ParameterDirection.Input)
        Dim pOtmardes As New OracleParameter("otmardes", OracleDbType.Int32, cplisdenoRecno, ParameterDirection.Input)
        If otmardesRecno Is Nothing Then : pOtmardes.Value = DBNull.Value : Else : pOtmardes.Value = otmardesRecno : End If
        Dim pOtdureza As New OracleParameter("otdureza", OracleDbType.Int32, ParameterDirection.Input)
        Dim pOttrasec As New OracleParameter("ottrasec", OracleDbType.Int32, ParameterDirection.Input)
        Dim pOttratam As New OracleParameter("ottratam", OracleDbType.Int32, ParameterDirection.Input)
        If ottratam Is Nothing Then : pOttratam.Value = DBNull.Value : Else : pOttratam.Value = ottratam : End If
        Dim pOtmatespe As New OracleParameter("otmatespe", OracleDbType.Int32, ParameterDirection.Input)
        If pOtmatespe Is Nothing Then : pOtmatespe.Value = DBNull.Value : Else : pOtmatespe.Value = otmatespeRecno : End If
        Dim pRefcli As New OracleParameter("ref_cli", OracleDbType.Varchar2,
                                               Text.RegularExpressions.Regex.Replace(observ, "[\.\,\-\ ]", ""), ParameterDirection.Input)
        Dim pConjunto As New OracleParameter("conjunto", OracleDbType.Varchar2, ParameterDirection.Input)
        pConjunto.Size = 10
        pConjunto.Value = If(conjunto, DBNull.Value)
        If ottrasecRecno Is Nothing Then
            pObserv2.Value = DBNull.Value : pObserv2.Size = 20
            pRealiza.Value = DBNull.Value
            pOttrasec.Value = DBNull.Value
        Else
            pObserv2.Value = observ2 : pObserv2.Size = 20
            pRealiza.Value = realiza : pRealiza.Size = 1
            pOttrasec.Value = ottrasecRecno
        End If

        If ottratam > 1 Then
            pOtdureza.Value = ordurezaRecno
        Else
            pOtdureza.Value = DBNull.Value
        End If
        OracleManagedDirectAccess.NoQuery(q, strCn, pMaterial, pCannec, pDiametro, pLargo, pAncho, pGrueso,
                                             ptratam, pTratam2, pObserv, pObserv2, pFase, pRealiza, pCplisdeno, pOtmardes, pOtdureza, pOttrasec,
                                             pOttratam, pOtmatespe, pRefcli, pOrd, pOp, pMar, pConjunto)
    End Sub

    Public Function GetCabeceraImpresion(ByVal ord As Integer, ByVal op As Integer, ByVal strCn As String) As String() Implements IDBAccess.GetCabeceraImpresion
        Dim q = "select c.platroq,g.nomprov from cpliscab c inner join gcprovee g on c.codcli=g.codpro where c.numord=:ord and c.numope=:op "
        Dim p1 As New OracleParameter("ord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("op", OracleDbType.Int32, op, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("platroq").ToString, r("nomprov").ToString}, q, strCn, p1, p2)
        If lst Is Nothing OrElse lst.Count = 0 Then
            Return Nothing
        End If
        Return lst(0)
    End Function

    Public Function ExistePunzon(ByVal otmardes As Integer, ByVal strCn As String) As Boolean Implements IDBAccess.ExistePunzon
        Dim q = "select count(*) from gcmarpun where id_otmardes=:otmardes"
        Dim p As New OracleParameter("otmardes", OracleDbType.Int32, otmardes, ParameterDirection.Input)
        Dim c = OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p)
        Return c > 0
    End Function

    Public Function GetDatosPlano(ByVal ord As Integer, ByVal ope As Integer, ByVal strCn As String) As String()
        Dim q = "select fo.platroq,fc.cliente,fc.tiptroq from faopepre fo inner join facabpre fc on fo.numpre=fc.numpre and " _
                + "fo.numcor=fc.numcor where fc.numord=:numord and fo.numope=:numope and fc.numcor=0 and fo.platroq is not null"
        Dim p1 As New OracleParameter("numord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("numope", OracleDbType.Int32, ope, ParameterDirection.Input)
        Dim lst = OracleManagedDirectAccess.seleccionar(Function(r As OracleDataReader) New String() {r("platroq").ToString, r("cliente").ToString, r("tiptroq").ToString}, q, strCn, p1, p2)
        If lst Is Nothing OrElse lst.Count = 0 Then
            Throw New Exception("Se ha producido un error al intentar acceder a los datos del plano en las tablas faopepre y facabpre. numord=" _
                                + ord.ToString + " numope=" + ope.ToString)
        End If
        Return lst.First()
    End Function

    Public Function ExisteEnCpliscab(ByVal ord As Integer, ByVal ope As Integer, ByVal strCn As String) As Boolean
        Dim q = "Select count(*) from cpliscab where numord=:numord and numope=:numope"
        Dim p1 As New OracleParameter("numord", OracleDbType.Int32, ord, ParameterDirection.Input)
        Dim p2 As New OracleParameter("numope", OracleDbType.Int32, ope, ParameterDirection.Input)
        Return OracleManagedDirectAccess.SeleccionarEscalar(Of Integer)(q, strCn, p1, p2) > 0
    End Function

    Public Sub AsegurarExistenciaEnCpliscab(ByVal ord As Integer, ByVal ope As Integer, ByVal strCn As String) Implements IDBAccess.AsegurarExistenciaEnCpliscab
        If Not ExisteEnCpliscab(ord, ope, strCn) Then
            'Get datos del plano
            Dim s = GetDatosPlano(ord, ope, strCn)

            If s Is Nothing OrElse s.Length = 0 Then
                Throw New ArgumentException("No se han podido cargar los datos de plano. Puede que platroq sea nulo.")
            End If

            Dim q = "insert into cpliscab(numord,numope,tiptro,codcli,platroq) values(:numord,:numope,:tiptro,:codcli,:platroq)"
            Dim p1 As New OracleParameter("numord", OracleDbType.Int32, ord, ParameterDirection.Input)
            Dim p2 As New OracleParameter("numope", OracleDbType.Int32, ope, ParameterDirection.Input)
            Dim p3 As New OracleParameter("tiptro", OracleDbType.Int32, s(2), ParameterDirection.Input)
            Dim p4 As New OracleParameter("codcli", OracleDbType.Char, s(1), ParameterDirection.Input)
            Dim p5 As New OracleParameter("platroq", OracleDbType.Char, s(0), ParameterDirection.Input)
            OracleManagedDirectAccess.NoQuery(q, strCn, p1, p2, p3, p4, p5)
        End If

    End Sub

End Class
