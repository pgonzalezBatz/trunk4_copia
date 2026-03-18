Imports Oracle.DataAccess.Client

Namespace BLL

    Public Class DocumentosBLL

        Private documentosDAL As New DAL.DocumentosDAL



#Region "Consultas"


        ''' <summary>
        ''' Obtiene un listado de tipos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function loadProveedor(ByVal planta As Integer, ByVal codigo As String) As List(Of ELL.Empresas)
            Return documentosDAL.loadProveedor(planta, codigo)
        End Function
        Public Function loadProveedorExacto(ByVal planta As Integer, ByVal codigo As String) As List(Of ELL.Empresas)
            Return documentosDAL.loadProveedorExacto(planta, codigo)
        End Function


        Public Function CargarRol(ByVal idUser As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            Return documentosDAL.CargarRol(idUser, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresa(ByVal id As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposEmpresa(id, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaStep(ByVal id As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposEmpresaStep(id, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaStep2(ByVal id As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposEmpresaStep2(id, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaWork(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposEmpresaWork(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaProcess(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposEmpresaProcess(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaChar(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposEmpresaChar(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaResulting(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposEmpresaResulting(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaComponent(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposEmpresaComponent(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposstepswork(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposstepswork(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposstepsworkP(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposStepsWorkP(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposstepsworkC(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposStepsWorkC(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposstepsworkR(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposStepsWorkR(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresa2(ByVal nombreusuario As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresa2(nombreusuario)
        End Function
        'Public Function CargarTiposEmpresaCIF(ByVal cif As String, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
        '    Return documentosDAL.CargarTiposEmpresaCIF(cif, plantaAdmin)
        'End Function
        Public Function CargarTiposChar() As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposChar()
        End Function
        Public Function CargarTiposCharSimb(ByVal Simb As String) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposCharSimb(Simb)
        End Function
        Public Function CargarTiposCharSimb2(ByVal Simb As String) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarTiposCharSimb2(Simb)
        End Function



        Public Function CargarTiposTrabajadorXBATCIF(ByVal dni As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposTrabajadorXBATCIF(dni)
        End Function
        Public Function CargarTiposTrabajadorXBATCIFNoCad(ByVal dni As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposTrabajadorXBATCIFNoCad(dni)
        End Function
        Public Function CargarTiposTrabajadorXBATCIFTODOS(ByVal dni As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposTrabajadorXBATCIFTODOS(dni)
        End Function
        Public Function CargarTiposTrabajadorXBATUserPlanta(ByVal planta As Integer, ByVal dni As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposTrabajadorXBATuserPlanta(planta, dni)
        End Function
        Public Function CargarTiposEmpresaXBATTroqueleria(ByVal id As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresaXBATTroqueleria(id)
        End Function
        Public Function CargarTiposTrabajadorXBATTroqueleria(ByVal id As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposTrabajadorXBATTroqueleria(id)
        End Function

        Public Function CargarListaPre(ByVal plantaAdmin As Integer) As List(Of ELL.Preventiva)
            Return documentosDAL.loadListPre(plantaAdmin)
        End Function

        Public Function CargarListaEmpresas(ByVal process As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListSteps()
        End Function
        Public Function CargarListaEmpresas2(ByVal process As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListSteps2(process)
        End Function
        Public Function CargarListaStepsTotal(ByVal id As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListStepsTotal(id)
        End Function

        Public Function loadListStepsChar777(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListStepsChar777(proceso, steps, work)
        End Function
        Public Function loadListStepsChar(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListStepsChar(proceso, steps, work)
        End Function
        Public Function loadListStepsChar2(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListStepsChar2(proceso, steps, work)
        End Function
        Public Function loadListStepsChart(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListStepsChart(proceso, steps, work)
        End Function
        Public Function loadListStepsChartVV(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListStepsChartVV(cont, referencia, componente, work, steps)
        End Function
        Public Function loadListStepsChar2t(ByVal proceso As Integer, ByVal steps As Integer, ByVal work As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListStepsChar2t(proceso, steps, work)
        End Function
        Public Function loadListStepsChar2tVV(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListStepsChar2tVV(cont, referencia, componente, work, steps)
        End Function
        Public Function CargarListaEmpresasWork(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasWork(plantaAdmin)
        End Function
        Public Function CargarListaEmpresasProcess(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasProcess(plantaAdmin)
        End Function
        Public Function CargarListaEmpresasComponent(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasComponent(plantaAdmin)
        End Function
        Public Function CargarListaCausas777(ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.CargarListaCausas777(referencia, componente, work, steps)
        End Function
        Public Function CargarListaEmpresasResulting(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasResulting(plantaAdmin)
        End Function
        Public Function CargarListaEmpresasCharacteristic(ByVal plantaAdmin As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasCharacteristic(plantaAdmin)
        End Function
        Public Function CargarListaAsociaciones(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones(cont, referencia, componente, work, steps)
        End Function
        Public Function CargarListaAsociaciones0(ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones0(referencia, componente, work, steps)
        End Function
        Public Function CargarListaAsociaciones4(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal work As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones4(cont, referencia, componente, work, steps)
        End Function
        Public Function CargarListaAsociaciones5(ByVal id As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones5(id)
        End Function
        Public Function CargarListaAsociacionesControl(ByVal id As Integer, ByVal cond As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociacionesControl(id, cond)
        End Function
        Public Function CargarListaAsociaciones6(ByVal id As Integer, ByVal cond As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones6(id, cond)
        End Function
        Public Function CargarListaAsociaciones7(ByVal id As Integer, ByVal cond As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones7(id, cond)
        End Function
        Public Function CargarListaAsociaciones6Detec(ByVal id As Integer, ByVal cond As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones6Detec(id, cond)
        End Function
        Public Function CargarListaAsociaciones6Detec3(ByVal id As Integer, ByVal cond As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones6Detec3(id, cond)
        End Function
        Public Function CargarListaAsociaciones6Detec2(ByVal id As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones6Detec2(id)
        End Function
        Public Function CargarListaAsociaciones2(ByVal referencia As String, ByVal componente As String) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones2(referencia, componente)
        End Function
        Public Function CargarListaRefComp(ByVal referencia As String) As List(Of ELL.Asociacion)
            Return documentosDAL.CargarListaRefComp(referencia)
        End Function
        Public Function CargarListaRefComp0(ByVal referencia As String) As List(Of ELL.Asociacion)
            Return documentosDAL.CargarListaRefComp0(referencia)
        End Function
        Public Function CargarListaAsociaciones2Ref(ByVal referencia As String, ByVal componente As String) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones2Ref(referencia, componente)
        End Function
        Public Function CargarListaAsociaciones4(ByVal referencia As String, ByVal id As String) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones4(referencia, id)
        End Function
        Public Function CargarListaAsociaciones3(ByVal cont As Integer, ByVal referencia As String, ByVal componente As String, ByVal proceso As Integer, ByVal steps As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListAsociaciones3(cont, referencia, componente, proceso, steps)
        End Function
        Public Function CargarListaWork(ByVal Steps As Integer) As List(Of ELL.Asociacion)
            Return documentosDAL.loadListWork(Steps)
        End Function
        Public Function CargarListaStepswork() As List(Of ELL.Kaplan)
            Return documentosDAL.loadListStepswork()
        End Function
        Public Function CargarListaStepsworkP() As List(Of ELL.Kaplan)
            Return documentosDAL.loadListStepsworkP()
        End Function
        Public Function CargarListaStepsworkC() As List(Of ELL.Kaplan)
            Return documentosDAL.loadListStepsworkC()
        End Function
        Public Function CargarListaStepsR() As List(Of ELL.Kaplan)
            Return documentosDAL.loadListStepsR()
        End Function

        Public Function loadListCausas(ByVal causa As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListCausas(causa)
        End Function

        Public Function loadListEmpresas3(ByVal steps As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresas3(steps)
        End Function

        Public Function loadListEmpresas4(ByVal workelement As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresas4(workelement)
        End Function

        Public Function loadListEmpresas4C(ByVal workelement As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresas4C(workelement)
        End Function
        Public Function loadListEmpresas4R(ByVal workelement As Integer) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresas4R(workelement)
        End Function


        Public Function CargarListaEmpresastexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoCharacteristics(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTextoCharacteristics(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoResulting(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTextoResulting(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoProcess(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTextoProcess(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoComponent(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTextoComponent(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoWork(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTextoWork(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastexto22(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto22(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastexto22P(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto22P(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastexto22C(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto22C(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastexto22R(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto22R(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastexto2(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto2(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastexto2P(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto2P(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastexto2C(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto2C(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastexto2R(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListEmpresasTexto2R(plantaAdmin, texto)
        End Function
        Public Function CargarListaDocumentostexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListDocumentosTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaDocumentostextoP(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListDocumentosTextoP(plantaAdmin, texto)
        End Function
        Public Function CargarListaDocumentostextoWork(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Kaplan)
            Return documentosDAL.loadListDocumentosTextoWork(plantaAdmin, texto)
        End Function

        Public Function CargarListaPlanta() As List(Of ELL.Documentos)
            Return documentosDAL.loadListPlanta()
        End Function
#End Region

#Region "Modificaciones"

        Public Function GuardarEmp(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveEmp(doc)
        End Function
        Public Function GuardarResulting(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveResulting(doc)
        End Function
        Public Function SaveCompRef0() As Boolean
            Return documentosDAL.SaveCompRef0()
        End Function
        Public Function SaveCompRef(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.SaveCompRef(doc)
        End Function
        Public Function GuardarChar(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveChar(doc)
        End Function
        Public Function GuardarEmp2(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveEmp2(doc)
        End Function
        Public Function GuardarOpeBorrar(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.SaveOpeBorrar(doc)
        End Function
        Public Function GuardarOpeBorrar2(ByVal id As Integer) As Boolean
            Return documentosDAL.SaveOpeBorrar2(id)
        End Function
        Public Function GuardarOpeBorrarDatos(ByVal id As String) As Boolean
            Return documentosDAL.SaveOpeBorrarDatos(id)
        End Function
        Public Function GuardarOpe(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.SaveOpe(doc)
        End Function
        Public Function GuardarOpeDatos(ByVal doc As ELL.Datos) As Boolean
            Return documentosDAL.SaveOpeDatos(doc)
        End Function
        Public Function GuardarOpeAtri(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.SaveOpe(doc)
        End Function
        Public Function GuardarOpe2(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.SaveOpe2(doc)
        End Function

        Public Function GuardarOpe2Borrar(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.SaveOpe2Borrar(doc)
        End Function
        Public Function GuardarOpe3Borrar(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.SaveOpe3Borrar(doc)
        End Function
        Public Function GuardarOpe3BorrarAsociacion(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.SaveOpe3BorrarAsociacion(doc)
        End Function

        Public Function ModOpe(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.ModOpe(doc)
        End Function
        Public Function GuardarEmp2P(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveEmp2P(doc)
        End Function
        Public Function GuardarEmp2C(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveEmp2C(doc)
        End Function
        Public Function GuardarEmp2R(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveEmp2R(doc)
        End Function
        Public Function GuardarEmpWork(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveEmpWork(doc)
        End Function
        Public Function GuardarEmpProcess(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveEmpProcess(doc)
        End Function
        Public Function GuardarEmpComponent(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.SaveEmpComponent(doc)
        End Function
        Public Function GuardarEmpSAB(ByVal doc As ELL.Empresas) As List(Of String())
            Return documentosDAL.SaveEmpSAB(doc)
        End Function

        Public Function ModificarEmp(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmp(doc, codigo)
        End Function
        Public Function ModificarEmpTotal(ByVal doc As ELL.Asociacion, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpTotal(doc, codigo)
        End Function
        Public Function ModificarEmpTotal2(ByVal doc As ELL.Asociacion, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpTotal2(doc, codigo)
        End Function
        Public Function ModificarEmpTotal2Metodo(ByVal doc As ELL.Asociacion, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpTotal2Metodo(doc, codigo)
        End Function
        Public Function ModificarEmpTotal2Efectos(ByVal doc As ELL.Asociacion, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpTotal2Efectos(doc, codigo)
        End Function
        Public Function ModificarEmpTotal2Efectos2(ByVal doc As ELL.Asociacion, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpTotal2Efectos2(doc, codigo)
        End Function
        Public Function ModificarEmpTotalR(ByVal doc As ELL.Asociacion, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpTotalR(doc, codigo)
        End Function
        Public Function ModificarChar(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateChar(doc, codigo)
        End Function
        Public Function ModificarResulting(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateResulting(doc, codigo)
        End Function
        Public Function ModificarEmpWork(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpWork(doc, codigo)
        End Function
        Public Function ModificarEmpProcess(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpProcess(doc, codigo)
        End Function
        Public Function ModificarEmpComponent(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmpComponent(doc, codigo)
        End Function
        Public Function ModificarEmp2(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmp2(doc, codigo)
        End Function
        Public Function ModificarEmp2P(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmp2P(doc, codigo)
        End Function
        Public Function ModificarEmp2C(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmp2C(doc, codigo)
        End Function
        Public Function ModificarEmp2R(ByVal doc As ELL.Kaplan, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmp2R(doc, codigo)
        End Function

        Public Function ModificarEmp(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmp(doc)
        End Function
        Public Function UpdateEmpResulting(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmpResulting(doc)
        End Function
        Public Function UpdateEmpChar(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmpChar(doc)
        End Function
        Public Function UpdateSteps(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateSteps(doc)
        End Function
        Public Function UpdateDetec(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateDetec(doc)
        End Function
        Public Function UpdateCausa(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateCausa(doc)
        End Function
        Public Function ModificarEmpWork(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmpWork(doc)
        End Function
        Public Function ModificarEmpProcess(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmpProcess(doc)
        End Function
        Public Function ModificarEmpComponent(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmpComponent(doc)
        End Function
        Public Function ModificarEmp2(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmp2(doc)
        End Function
        Public Function ModificarEmp2Char(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.UpdateEmp2Char(doc)
        End Function
        Public Function ModificarEmp2CharATR(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.UpdateEmp2CharATR(doc)
        End Function
        Public Function ModificarEmp2CharVVV(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.UpdateEmp2CharVVV(doc)
        End Function
        Public Function ModificarEmp2CharVV(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.UpdateEmp2CharVV(doc)
        End Function
        Public Function ModificarEmp3Char(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.UpdateEmp3Char(doc)
        End Function
        Public Function ModificarCondiciones(ByVal doc As ELL.Asociacion) As Boolean
            Return documentosDAL.ModificarCondiciones(doc)
        End Function
        Public Function ModificarEmp2P(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmp2P(doc)
        End Function
        Public Function ModificarEmp2C(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmp2C(doc)
        End Function
        Public Function ModificarEmp2R(ByVal doc As ELL.Kaplan) As Boolean
            Return documentosDAL.UpdateEmp2R(doc)
        End Function


#End Region



        Public Function CargarPerfilUsuario(ByVal idUsuario As Integer, ByVal idPlanta As String) As ELL.PerfilUsuario
            If System.Configuration.ConfigurationManager.AppSettings("extranet").ToString = "1" Then
                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = 99} 'extranet
            End If
            ' Llenamos los datos del perfil del usuario
            Dim RolUser As List(Of ELL.Rol)
            '     Dim RolBBDD As Integer
            Dim oDocumentosBLL As New BLL.DocumentosBLL
            RolUser = oDocumentosBLL.CargarRol(63690, idPlanta) 'idusuario
            If RolUser.Count > 0 Then
                'si hacemos que tenga mas de uno poner estos for por cada rol
                For i = 0 To RolUser.Count - 1
                    If RolUser(i).Id = ELL.Roles.RolUsuario.Administrador Then
                        Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador}
                    End If
                Next
                For i = 0 To RolUser.Count - 1
                    If RolUser(i).Id = ELL.Roles.RolUsuario.Recepcion Then
                        Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Recepcion}
                    End If
                Next

                Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = RolUser(0).Id} 'extranet

                'Else
                '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = 99} 'extranet
            End If
            Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = 99}
            'If (System.Configuration.ConfigurationManager.AppSettings("Administradores").ToString.Contains(idUsuario)) Then
            '    'Si el rol es 2 es administrador

            '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador, .IdDepartamento = idDepartamento}
            'ElseIf (System.Configuration.ConfigurationManager.AppSettings("Recepcion").ToString.Contains(idUsuario)) Then
            '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Recepcion}
            '    'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProductManager").ToString.Contains(idUsuario)) Then
            '    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProductManager}
            '    'ElseIf (System.Configuration.ConfigurationManager.AppSettings("DocumentationTechnician").ToString.Contains(idUsuario)) Then
            '    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.DocumentationTechnician}
            '    'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProjectLeaderManager").ToString.Contains(idUsuario)) Then
            '    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProjectLeaderManager}
            '    'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProjectLeader").ToString.Contains(idUsuario)) Then
            '    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProjectLeader}
            'Else
            '    'Dim usuariosRolBLL As New BLL.BonoSisBLL()
            '    'Dim rol As ELL.UsuarioRol = usuariosRolBLL.CargarRolUsuario(idUsuario)
            '    'Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = If(rol IsNot Nothing, rol.IdRol, Integer.MinValue)}
            'End If
        End Function


        Public Function CargarPerfilUsuarioETT(ByVal idUsuario As Integer, ByVal idPlanta As String) As ELL.PerfilUsuario
            'If System.Configuration.ConfigurationManager.AppSettings("extranet").ToString = "1" Then
            '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = 99} 'extranet
            'End If
            '' Llenamos los datos del perfil del usuario
            'Dim RolUser As List(Of ELL.Rol)
            ''     Dim RolBBDD As Integer
            'Dim oDocumentosBLL As New BLL.DocumentosBLL
            'RolUser = oDocumentosBLL.CargarRolETT(idUsuario, idPlanta)
            'If RolUser.Count > 0 Then
            '    'si hacemos que tenga mas de uno poner estos for por cada rol
            '    For i = 0 To RolUser.Count - 1
            '        If RolUser(i).Id = ELL.Roles.RolUsuario.RRHH Then
            '            Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.RRHH}
            '        End If
            '    Next
            '    For i = 0 To RolUser.Count - 1
            '        If RolUser(i).Id = ELL.Roles.RolUsuario.Administrador Then
            '            Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador}
            '        End If
            '    Next
            '    For i = 0 To RolUser.Count - 1
            '        If RolUser(i).Id = ELL.Roles.RolUsuario.Consultor Then
            '            Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Consultor}
            '        End If
            '    Next
            '    For i = 0 To RolUser.Count - 1
            '        If RolUser(i).Id = ELL.Roles.RolUsuario.Usuario Then
            '            Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Usuario}
            '        End If
            '    Next
            '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = RolUser(0).Id} 'extranet

            '    'Else
            '    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = 99} 'extranet
            'End If
            'Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = 99}
            ''If (System.Configuration.ConfigurationManager.AppSettings("Administradores").ToString.Contains(idUsuario)) Then
            ''    'Si el rol es 2 es administrador

            ''    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador, .IdDepartamento = idDepartamento}
            ''ElseIf (System.Configuration.ConfigurationManager.AppSettings("Recepcion").ToString.Contains(idUsuario)) Then
            ''    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Recepcion}
            ''    'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProductManager").ToString.Contains(idUsuario)) Then
            ''    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProductManager}
            ''    'ElseIf (System.Configuration.ConfigurationManager.AppSettings("DocumentationTechnician").ToString.Contains(idUsuario)) Then
            ''    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.DocumentationTechnician}
            ''    'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProjectLeaderManager").ToString.Contains(idUsuario)) Then
            ''    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProjectLeaderManager}
            ''    'ElseIf (System.Configuration.ConfigurationManager.AppSettings("ProjectLeader").ToString.Contains(idUsuario)) Then
            ''    '    Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.ProjectLeader}
            ''Else
            ''    'Dim usuariosRolBLL As New BLL.BonoSisBLL()
            ''    'Dim rol As ELL.UsuarioRol = usuariosRolBLL.CargarRolUsuario(idUsuario)
            ''    'Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = If(rol IsNot Nothing, rol.IdRol, Integer.MinValue)}
            ''End If
        End Function

        Public Function Existe(ByVal id As String) As Boolean
            'If (documentosDAL.existe(id) > 0) Then
            '    Return True
            'Else : Return False
            'End If
            Return False
        End Function
        Public Function ExisteETT(ByVal id As String) As Boolean
            'If (documentosDAL.existeETT(id) > 0) Then
            '    Return True
            'Else : Return False
            'End If
            Return False
        End Function


    End Class

End Namespace
