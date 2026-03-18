Public Class IndiceFecuencia
    Inherits PageBase
#Region "Propiedades"
    Dim BBDD As New BatzBBDD.Entities_Gertakariak
    Dim Epsilo_BD As New BatzBBDD.Entities_Epsilon
#End Region

#Region "Eventos de Pagina"
    Private Sub IndiceFecuencia_Init(sender As Object, e As EventArgs) Handles Me.Init

    End Sub

    Private Sub IndiceFecuencia_PreLoad(sender As Object, e As EventArgs) Handles Me.PreLoad
        If IsPostBack Then
            ce_imgFechaRevision.SelectedDate = CDate(txtFechaCruzVerde.Text)
        Else
            ce_imgFechaRevision.SelectedDate = Now.Date
            txtFechaCruzVerde.Text = ce_imgFechaRevision.SelectedDate
        End If

#If DEBUG Then
        If Not IsPostBack Then
            ce_imgFechaRevision.SelectedDate = Now.AddMonths(-1) : txtFechaCruzVerde.Text = ce_imgFechaRevision.SelectedDate
            tc_CruzVerde.ActiveTabIndex = tc_CruzVerde.Controls.IndexOf(tc_CruzVerde.FindControl(tp_Troqueleria.ID))
            'ddlUG.SelectedIndex = 1
        End If
#End If

        lblFechaCruzVerde.Text = CDate(ce_imgFechaRevision.SelectedDate).ToString("MMMM (yyyy)")

        Cargar_CruzVerde()
    End Sub
#End Region

#Region "Eventos de Objetos"
#End Region

#Region "Funciones y Procesos"
    Sub Cargar_CruzVerde()
        Dim Lista As New List(Of BatzBBDD.GERTAKARIAK)
        Dim FechaInicio As Date = DateSerial(ce_imgFechaRevision.SelectedDate.Value.Year, 1, 1)
        Dim FechaFin As Date = DateSerial(ce_imgFechaRevision.SelectedDate.Value.Year, 12, 31)
        Dim FechaUltimaBaja As Date = Now.Date
        Dim FechaUltimaBaja_T As Date = Now.Date
        Dim FechaUltimaBaja_S As Date = Now.Date
        Dim IDTIPOINCIDENCIA As Integer = If(FiltroGTK.TipoIncidencia, My.Settings.IdTipoIncidencia) '5-Istriku

        Lista = (From gtk As BatzBBDD.GERTAKARIAK In BBDD.GERTAKARIAK
                 Where gtk.IDTIPOINCIDENCIA = IDTIPOINCIDENCIA And gtk.FECHAAPERTURA >= FechaInicio And gtk.FECHAAPERTURA <= FechaFin Select gtk).ToList

        For i As Integer = 1 To 31
            Dim NumDiaMes As Integer = i
            Dim CeldaHTML As HtmlTableCell = TablaCruzVerde.FindControl("td" + i.ToString("00"))
            Dim CeldaHTML_T As HtmlTableCell = TablaCruzVerde_T.FindControl("td_T" + i.ToString("00"))
            Dim CeldaHTML_S As HtmlTableCell = TablaCruzVerde_S.FindControl("td_S" + i.ToString("00"))
            If CeldaHTML IsNot Nothing Then
                Dim DiaMes As Date = DateSerial(ce_imgFechaRevision.SelectedDate.Value.Year, ce_imgFechaRevision.SelectedDate.Value.Month, 1).AddDays(NumDiaMes - 1)
                CeldaHTML.Attributes.Item("class") = "CeldaCruzVerde CeldaCruzVerde_SinAccidentes"
                CeldaHTML.Attributes.Item("class") = If(DiaMes = Now.Date, "Hoy " & CeldaHTML.Attributes.Item("class"), CeldaHTML.Attributes.Item("class"))
                CeldaHTML_S.Attributes.Item("class") = CeldaHTML.Attributes.Item("class")
                CeldaHTML_T.Attributes.Item("class") = CeldaHTML.Attributes.Item("class")
                If Lista.Any Then
                    Dim lAux = Lista.Where(Function(gtk) gtk.FECHAAPERTURA.Value.ToShortDateString = DiaMes)
                    If lAux.Any Then
                        Dim bTieneBaja As Boolean = False
                        'For Each Incidencia As BatzBBDD.GERTAKARIAK In lAux.Where(Function(o) o.PROCEDENCIANC = 4) 'Con Lesion=4
                        For Each Incidencia As BatzBBDD.GERTAKARIAK In lAux
                            For Each UsrSab As BatzBBDD.SAB_USUARIOS In Incidencia.DETECCION.Select(Function(o) o.SAB_USUARIOS)
                                bTieneBaja = New _Default().TieneBaja(Incidencia, UsrSab)
                                'Dim Cod_Negocio As String = (From COD_TRA As BatzBBDD.COD_TRA In Epsilo_BD.COD_TRA
                                '                             Join PT As BatzBBDD.PUES_TRAB In Epsilo_BD.PUES_TRAB On PT.ID_EMPRESA Equals COD_TRA.ID_EMPRESA And PT.ID_TRABAJADOR Equals COD_TRA.ID_TRABAJADOR
                                '                             Where COD_TRA.NIF = UsrSab.DNI And COD_TRA.ID_EMPRESA = "00001" _
                                '                                 And PT.ID_ORGANIG = "00001" And PT.F_INI_PUE <= DiaMes And (PT.F_FIN_PUE >= DiaMes Or PT.F_FIN_PUE Is Nothing)
                                '                             Select PT.NIV_ORG.ORDEN.N2).FirstOrDefault
                                Dim Cod_Negocio As String = CodigoNegocio(UsrSab.DNI, DiaMes)
                                If Cod_Negocio IsNot Nothing Then
                                    If Cod_Negocio = "00985" Then 'Sistemas de Automocion
                                        CeldaHTML_S.Attributes.Item("class") = CeldaHTML.Attributes.Item("class") & " " & If(bTieneBaja = True, "CeldaCruzVerde_Accidente", "CeldaCruzVerde_Incidente")
                                    ElseIf Cod_Negocio = "00003" OrElse Cod_Negocio = "02590" Then 'Troqueleria
                                        'If ddlUG.SelectedIndex > 0 Then
                                        '    Dim sD_NIVEL As String = (From NIV_ORG As BatzBBDD.NIV_ORG In Epsilo_BD.NIV_ORG
                                        '                              Where NIV_ORG.ORDEN IsNot Nothing AndAlso NIV_ORG.ORDEN.NIVEL = 3 And NIV_ORG.ID_NIVEL = UsrSab.IDDEPARTAMENTO
                                        '                              Select NIV_ORG.D_NIVEL Distinct).FirstOrDefault

                                        '    If sD_NIVEL IsNot Nothing AndAlso (StrComp(sD_NIVEL.Substring(2, 2), ddlUG.SelectedValue,) = 0) = True Then
                                        '        CeldaHTML_T.Attributes.Item("class") = CeldaHTML.Attributes.Item("class") & " " & If(bTieneBaja = True, "CeldaCruzVerde_Accidente", "CeldaCruzVerde_Incidente")
                                        '    End If
                                        'Else
                                        CeldaHTML_T.Attributes.Item("class") = CeldaHTML.Attributes.Item("class") & " " & If(bTieneBaja = True, "CeldaCruzVerde_Accidente", "CeldaCruzVerde_Incidente")
                                        'End If
                                    End If
                                End If
                                If bTieneBaja = True Then Exit For
                            Next
                            If bTieneBaja = True Then Exit For
                        Next
                        CeldaHTML.Attributes.Item("class") &= " " & If(bTieneBaja = True, "CeldaCruzVerde_Accidente", "CeldaCruzVerde_Incidente")
                    End If
                End If
            End If
        Next

        '----------------------------------------------------------------------------------------------
        'Calculamos los dias sin accidentes con bajas
        '----------------------------------------------------------------------------------------------
        lblNumDiasMax.Text = String.Empty
        lblNumDiasMax_Sistemas.Text = String.Empty
        lblNumDiasMax_Troqueleria.Text = String.Empty

        Dim ListaLesion As List(Of BatzBBDD.GERTAKARIAK) = (Lista.Where(Function(o) o.PROCEDENCIANC = 4).OrderByDescending(Function(o) o.FECHAAPERTURA)).ToList 'Con Lesion=4
        For Each Incidencia As BatzBBDD.GERTAKARIAK In ListaLesion
            For Each UsrSab As BatzBBDD.SAB_USUARIOS In Incidencia.DETECCION.OrderByDescending(Function(o) o.GERTAKARIAK.FECHAAPERTURA).Select(Function(o) o.SAB_USUARIOS)
                If FechaInicio > FechaUltimaBaja Then Exit For 'Se cancela la busqueda si no esta en el año en curso.
                If New _Default().TieneBaja(Incidencia, UsrSab) = True Then
                    'Dias desde el ultimo accidente
                    If String.IsNullOrWhiteSpace(lblNumDias.Text) Then
                        lblNumDias.Text = DateDiff(DateInterval.Day, CDate(Incidencia.FECHAAPERTURA), Now.Date)
                        lblNumDiasMax.Text = lblNumDias.Text
                    End If

                    'Batz sKoop
                    Dim NumDiasMax As Integer = DateDiff(DateInterval.Day, CDate(Incidencia.FECHAAPERTURA), FechaUltimaBaja)
                    If CInt(If(IsNumeric(lblNumDiasMax.Text), lblNumDiasMax.Text, 0)) < NumDiasMax Then lblNumDiasMax.Text = NumDiasMax
                    'Global_asax.log.Debug(vbCrLf)
                    'Global_asax.log.Debug("G:" & NumDiasMax & String.Format(" ({0} -> {1})", Incidencia.FECHAAPERTURA, FechaUltimaBaja))
                    FechaUltimaBaja = Incidencia.FECHAAPERTURA

                    Dim Cod_Negocio As String = CodigoNegocio(UsrSab.DNI, Incidencia.FECHAAPERTURA)
                    If Cod_Negocio IsNot Nothing Then
                        If Cod_Negocio = "00985" Then 'Sistemas de Automocion
                            Dim NumDiasMax_S As Integer = DateDiff(DateInterval.Day, CDate(Incidencia.FECHAAPERTURA), FechaUltimaBaja_S)
                            If CInt(If(IsNumeric(lblNumDiasMax_Sistemas.Text), lblNumDiasMax_Sistemas.Text, 0)) < NumDiasMax_S Then lblNumDiasMax_Sistemas.Text = NumDiasMax_S
                            'Global_asax.log.Debug("S:" & NumDiasMax_S & String.Format(" ({0} -> {1})", Incidencia.FECHAAPERTURA, FechaUltimaBaja_S))
                            FechaUltimaBaja_S = Incidencia.FECHAAPERTURA
                        ElseIf Cod_Negocio = "00003" OrElse Cod_Negocio = "02590" Then 'Troqueleria
                            'If ddlUG.SelectedIndex > 0 Then
                            '    Dim sD_NIVEL As String = (From NIV_ORG As BatzBBDD.NIV_ORG In Epsilo_BD.NIV_ORG
                            '                              Where NIV_ORG.ORDEN IsNot Nothing AndAlso NIV_ORG.ORDEN.NIVEL = 3 And NIV_ORG.ID_NIVEL = UsrSab.IDDEPARTAMENTO
                            '                              Select NIV_ORG.D_NIVEL Distinct).SingleOrDefault
                            '    If sD_NIVEL IsNot Nothing AndAlso (StrComp(sD_NIVEL.Substring(2, 2), ddlUG.SelectedValue,) = 0) = True Then
                            '        Dim NumDiasMax_T As Integer = DateDiff(DateInterval.Day, CDate(Incidencia.FECHAAPERTURA), FechaUltimaBaja_T)
                            '        If CInt(If(IsNumeric(lblNumDiasMax_Troqueleria.Text), lblNumDiasMax_Troqueleria.Text, 0)) < NumDiasMax_T Then lblNumDiasMax_Troqueleria.Text = NumDiasMax_T
                            '        FechaUltimaBaja_T = Incidencia.FECHAAPERTURA
                            '    End If
                            'Else
                            Dim NumDiasMax_T As Integer = DateDiff(DateInterval.Day, CDate(Incidencia.FECHAAPERTURA), FechaUltimaBaja_T)
                            If CInt(If(IsNumeric(lblNumDiasMax_Troqueleria.Text), lblNumDiasMax_Troqueleria.Text, 0)) < NumDiasMax_T Then lblNumDiasMax_Troqueleria.Text = NumDiasMax_T
                            'Global_asax.log.Debug("T:" & NumDiasMax_T & String.Format(" ({0} -> {1})", Incidencia.FECHAAPERTURA, FechaUltimaBaja_T))
                            FechaUltimaBaja_T = Incidencia.FECHAAPERTURA
                            'End If

                        End If
                    End If
                End If
            Next
        Next
        If lblNumDiasMax_Troqueleria.Text.Equals("") Then
            lblNumDiasMax_Troqueleria.Text = "-"
            'lblNumDiasMax_Troqueleria.Text = DateDiff(DateInterval.Day, FechaInicio, Date.Today)
        End If
        '----------------------------------------------------------------------------------------------
    End Sub

    ''' <summary>
    ''' Identificador del Negocio a la que pertencece una persona en la fecha indicada.
    ''' </summary>
    ''' <param name="DNI"></param>
    ''' <param name="DiaMes"></param>
    ''' <returns></returns>
    Function CodigoNegocio(ByVal DNI As String, ByVal DiaMes As Date) As String
        Return (From COD_TRA As BatzBBDD.COD_TRA In Epsilo_BD.COD_TRA
                Join PT As BatzBBDD.PUES_TRAB In Epsilo_BD.PUES_TRAB On PT.ID_EMPRESA Equals COD_TRA.ID_EMPRESA And PT.ID_TRABAJADOR Equals COD_TRA.ID_TRABAJADOR
                Where COD_TRA.NIF = DNI And COD_TRA.ID_EMPRESA = "00001" And PT.ID_ORGANIG = "00001" And PT.F_INI_PUE <= DiaMes And (PT.F_FIN_PUE >= DiaMes Or PT.F_FIN_PUE Is Nothing)
                Select PT.NIV_ORG.ORDEN.N2).FirstOrDefault
    End Function
#End Region
End Class