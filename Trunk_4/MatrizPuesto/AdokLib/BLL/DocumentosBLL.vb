Imports Oracle.DataAccess.Client

Namespace BLL

    Public Class DocumentosBLL

        Private documentosDAL As New DAL.DocumentosDAL
        Public Function EmpresasActivas(ByVal plantaAdmin As Integer) As List(Of String())
            Return documentosDAL.EmpresasActivas(plantaAdmin)
        End Function
        Public Function AvisoTrabajadores(ByVal plantaAdmin As Integer) As List(Of String())
            Return documentosDAL.AvisoTrabajadores(plantaAdmin)
        End Function
        Public Function ActualizarEmpresaInactivas(ByVal solicitudes As List(Of String())) As Boolean
            Return documentosDAL.ActualizarEmpresaInactivas(solicitudes)
        End Function
        Public Function ActualizarAvisoTrabajadores(ByVal solicitudes As List(Of String())) As Boolean
            Return documentosDAL.ActualizarAvisoTrabajadores(solicitudes)
        End Function


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
        Public Function eliminaAsignacionTarjeta(ByVal planta As Integer, ByVal codigo As Integer) As Boolean
            Return documentosDAL.eliminaAsignacionTarjeta(planta, codigo)
        End Function
        'Public Function loadIzaroTra(ByVal plantaAdmin As Integer, ByVal dni As String) As List(Of String())
        '    Return documentosDAL.loadIzaroTra(plantaAdmin, dni)
        'End Function
        Public Function loadIzaroTrabajador(ByVal plantaAdmin As Integer, ByVal dni As String) As List(Of String())
            Return documentosDAL.loadIzaroTrabajador(plantaAdmin, dni)
        End Function
        Public Function loadIzaroTrabajador2(ByVal plantaAdmin As Integer, ByVal cod As Integer) As List(Of String())
            Return documentosDAL.loadIzaroTrabajador2(plantaAdmin, cod)
        End Function
        Public Function loadIzaroTrabajadorNoExiste(ByVal plantaAdmin As Integer) As List(Of String())
            Return documentosDAL.loadIzaroTrabajadorNoExiste(plantaAdmin)
        End Function
        Public Function deptoIzaro(ByVal plantaAdmin As Integer, ByVal trabajador As Integer) As List(Of String())
            Return documentosDAL.deptoIzaro(plantaAdmin, trabajador)
        End Function

        Public Function loadIzaroTarjeta(ByVal plantaAdmin As Integer, ByVal tarjeta As String) As List(Of String())
            Return documentosDAL.loadIzaroTarjeta(plantaAdmin, tarjeta)
        End Function
        Public Function loadIzaroTarjetaTra(ByVal plantaAdmin As Integer, ByVal trabajador As Integer) As List(Of String())
            Return documentosDAL.loadIzaroTarjetaTra(plantaAdmin, trabajador)
        End Function
        Public Function tr230IzaroTra(ByVal plantaAdmin As Integer, ByVal dni As String) As List(Of String())
            Return documentosDAL.tr230IzaroTra(plantaAdmin, dni)
        End Function
        Public Function ta010IzaroTra(ByVal plantaAdmin As Integer, ByVal cod As Integer) As List(Of String())
            Return documentosDAL.ta010IzaroTra(plantaAdmin, cod)
        End Function
        Public Function existeIzaroTra(ByVal plantaAdmin As Integer, ByVal dni As String) As List(Of String())
            Return documentosDAL.existeIzaroTra(plantaAdmin, dni)
        End Function
        Public Function UpdateIzaroTra(tipoTarjetaIzaro As ELL.TarjetaIZARO) As Boolean
            Return documentosDAL.updateIzaroTra(tipoTarjetaIzaro)
        End Function
        Public Function AltaIzaroTra(tipoTarjetaIzaro As ELL.TarjetaIZARO) As Boolean
            Return documentosDAL.AltaIzaroTra(tipoTarjetaIzaro)
        End Function
        Public Function AltaIzaroTraTarjeta(ByVal plantaAdmin As Integer, ByVal tarjeta As String, ByVal trabajador As Integer, ByVal fecfin As Date) As Boolean
            Return documentosDAL.AltaIzaroTraTarjeta(plantaAdmin, tarjeta, trabajador, fecfin)
        End Function
        Public Function ActualizaIzaroTra(tipoTarjetaIzaro As ELL.TarjetaIZARO) As Boolean
            Return documentosDAL.ActualizaIzaroTra(tipoTarjetaIzaro)
        End Function
        Public Function BorraIzaroTra(tipoTarjetaIzaro As ELL.TarjetaIZARO) As Boolean
            Return documentosDAL.BorraIzaroTra(tipoTarjetaIzaro)
        End Function
        Public Function CargarListaTodosTraDocAsignadosNew(ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTodosTraDocAsignadosNew(plantaAdmin)
        End Function
        Public Function CargarTiposTrabajadorActivosNew(ByVal plantaAdmin As Integer, ByVal idEmpresa As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveTraActivosNew(plantaAdmin, idEmpresa)
        End Function
        Public Function CargarDocumentos(ByVal idDoc As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.CargarDocumentos(idDoc, plantaAdmin)
        End Function
        Public Function CargarDocumentosCer(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Documentos)
            Return documentosDAL.CargarDocumentosCer(plantaAdmin, texto)
        End Function
        Public Function CargarProfesion(ByVal texto As String) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.CargarProfesion(texto)
        End Function
        Public Function CargarResponsable(ByVal texto As String) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.CargarResponsable(texto)
        End Function
        Public Function CargarResponsables(ByVal cod As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.CargarResponsables(cod)
        End Function
        Public Function CargarDocumentosETT(ByVal idDoc As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.CargarDocumentosETT(idDoc, plantaAdmin)
        End Function
        Public Function CargarRol(ByVal idUser As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            Return documentosDAL.CargarRol(idUser, plantaAdmin)
        End Function
        Public Function CargarRolETT(ByVal idUser As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            Return documentosDAL.CargarRolETT(idUser, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresa(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresa(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarEmpresas() As List(Of ELL.Empresas)
            Return documentosDAL.CargarEmpresas()
        End Function
        Public Function CargarTiposTrabajador(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposTrabajador(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresa2(ByVal doc As Integer, ByVal tra As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresa2(doc, tra)
        End Function
        Public Function CargarTiposEmpresaSAB(ByVal idEmpresa As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresaSAB(idEmpresa, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaS(ByVal idEmpresa As Integer, ByVal idEmpresa2 As String, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresaS(idEmpresa, idEmpresa2, plantaAdmin)
        End Function
        Public Function CargarTiposEmpresaCIF(ByVal cif As String, ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresaCIF(cif, plantaAdmin)
        End Function
        Public Function CargarCodusuario(ByVal mail As String) As List(Of ELL.CEtico)
            Return documentosDAL.CargarCodusuario(mail)
        End Function
        Public Function CargarTiposEmpresaCIFADOK(ByVal cif As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresaCIFADOK(cif)
        End Function
        Public Function CargarTiposTrabajadorCIF(ByVal cif As String, ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.CargarTiposTrabajadorCIF(cif, plantaAdmin)
        End Function
        Public Function CargarTiposSolicitudClave(ByVal cif As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.CargarTiposSolicitudClave(cif, plantaAdmin)
        End Function
        Public Function CargarTiposSolicitudClaveETT(ByVal planta As Integer, ByVal clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.CargarTiposSolicitudClaveETT(planta, clave)
        End Function
        Public Function CargarTiposTrabajadorActivos(ByVal plantaAdmin As Integer, ByVal idEmpresa As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveTraActivos(plantaAdmin, idEmpresa)
        End Function



        Public Function CargarTiposEmpresaXBAT(ByVal idEmpresa As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresaXBAT(idEmpresa)
        End Function
        Public Function CargarTiposEmpresaXBATNombre(ByVal nombre As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresaXBATNombre(nombre)
        End Function
        Public Function CargarTiposEmpresaXBATCIF(ByVal cif As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposEmpresaXBATCIF(cif)
        End Function
        Public Function CargarTiposTrabajadorXBATCIF(ByVal dni As String) As List(Of ELL.Empresas)
            Return documentosDAL.CargarTiposTrabajadorXBATCIF(dni)
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
        Public Function CargarTiposDorletporDNI(ByVal dni As String) As List(Of ELL.Dorlet)
            Return documentosDAL.CargarTiposDorletporDNI(dni)
        End Function

        Public Function CargarListaRes(ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            Return documentosDAL.loadListRes(plantaAdmin)
        End Function
        Public Function CargarListaSol(ByVal plantaAdmin As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSol(plantaAdmin)
        End Function
        Public Function CargarListaSolETT(ByVal plantaAdmin As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolETT(plantaAdmin)
        End Function
        Public Function CargarResponsables(ByVal cod As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            Return documentosDAL.loadResponsables(cod, plantaAdmin)
        End Function
        Public Function CargarResponsablesMail(ByVal cod As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            Return documentosDAL.loadResponsablesMail(cod, plantaAdmin)
        End Function
        Public Function CargarResponsablesNombreMail(ByVal cod As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            Return documentosDAL.loadResponsablesNombreMail(cod, plantaAdmin)
        End Function
        Public Function CargarMailSAB(ByVal cod As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            Return documentosDAL.loadMailSAB(cod, plantaAdmin)
        End Function

        Public Function CargarGlobalesNombreMail(ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            Return documentosDAL.loadGlobalesNombreMail(plantaAdmin)
        End Function
        Public Function CargarListaPla(ByVal plantaAdmin As Integer, ByVal idDocumento As Integer) As List(Of ELL.Plantillas)
            Return documentosDAL.loadListPla(plantaAdmin, idDocumento)
        End Function
        Public Function CargarListaPlaDoc(ByVal plantaAdmin As Integer, ByVal DocNombre As String) As List(Of ELL.Plantillas)
            Return documentosDAL.loadListPlaDoc(plantaAdmin, DocNombre)
        End Function
        Public Function CargarListaPla2(ByVal plantaAdmin As Integer, ByVal idDocumento As Integer) As List(Of ELL.Plantillas)
            Return documentosDAL.loadListPla2(plantaAdmin, idDocumento)
        End Function
        Public Function CargarListaPre(ByVal plantaAdmin As Integer) As List(Of ELL.Preventiva)
            Return documentosDAL.loadListPre(plantaAdmin)
        End Function
        Public Function CargarPre(ByVal plantaAdmin As Integer, ByVal codpre As Integer) As List(Of ELL.Preventiva)
            Return documentosDAL.loadPre(plantaAdmin, codpre)
        End Function
        Public Function CargarListaCad(ByVal plantaAdmin As Integer) As List(Of ELL.Caducidades)
            Return documentosDAL.loadListCad(plantaAdmin)
        End Function
        Public Function CargarDocTipo4(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListDocTipo4(plantaAdmin)
        End Function
        Public Function CargarListaRol(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            Return documentosDAL.loadListRol(plantaAdmin)
        End Function
        Public Function CargarListaRolETT(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            Return documentosDAL.loadListRolETT(plantaAdmin)
        End Function
        Public Function CargarListaDos(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            Return documentosDAL.loadListDos(plantaAdmin)
        End Function
        Public Function CargarListaDosETT(ByVal plantaAdmin As Integer) As List(Of ELL.Rol)
            Return documentosDAL.loadListDosETT(plantaAdmin)
        End Function
        Public Function CargarCad(ByVal plantaAdmin As Integer, ByVal codigo As Integer) As List(Of ELL.Caducidades)
            Return documentosDAL.loadCad(plantaAdmin, codigo)
        End Function
        Public Function CargarInt(ByVal plantaAdmin As Integer, ByVal codigo As Integer) As List(Of ELL.Caducidades)
            Return documentosDAL.loadInt(plantaAdmin, codigo)
        End Function
        Public Function CargarLista(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadList(plantaAdmin)
        End Function
        Public Function loadListTipo(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTipo(plantaAdmin, tipo)
        End Function
        Public Function loadListTiposinplantilla(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTipoSinPlantilla(plantaAdmin, tipo)
        End Function
        Public Function loadListTipoDOS4(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTipoDOS4(plantaAdmin, tipo)
        End Function
        Public Function loadListTipo4(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTipo4(plantaAdmin)
        End Function
        Public Function loadListTipo4ETT(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTipo4ETT(plantaAdmin)
        End Function
        Public Function loadListTipo4Activos(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTipo4Activos(plantaAdmin)
        End Function
        Public Function loadListTipo4ActivosETT(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTipo4ActivosETT(plantaAdmin)
        End Function
        Public Function CargarListaTodos(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListtodos(plantaAdmin)
        End Function
        Public Function CargarListaTodosPara4(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListtodosPara4(plantaAdmin)
        End Function
        Public Function CargarListaTodosPara4ETT(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListtodosPara4ETT(plantaAdmin, tipo)
        End Function
        Public Function CargarListaTodosResp(ByVal plantaAdmin As Integer) As List(Of ELL.Responsables)
            Return documentosDAL.loadListtodosResp(plantaAdmin)
        End Function
        Public Function CargarListaTra(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTra(plantaAdmin)
        End Function
        Public Function CargarListaTraActivos(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTraActivos(plantaAdmin)
        End Function
        Public Function CargarListaTraActivosMail(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTraActivosMail(plantaAdmin)
        End Function
        Public Function CargarListaTraActivosNoCad(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTraActivosNoCad(plantaAdmin)
        End Function
        Public Function CargarListaTraActivosNoCadResponsable(ByVal plantaAdmin As Integer, ByVal responsables As Integer()) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTraActivosNoCadResponsable(plantaAdmin, responsables)
        End Function
        Public Function CargarListaEmpresas(ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresas(plantaAdmin)
        End Function
        Public Function CargarListaEmpresasSAS(ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasSAS(plantaAdmin)
        End Function
        Public Function CargarListaEmpresasActivas(ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasActivas(plantaAdmin)
        End Function
        Public Function CargarListaEmpresasActivasConTra(ByVal plantaAdmin As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasActivasConTra(plantaAdmin)
        End Function
        Public Function CargarListaEmpresasActivasConTraResponsable(ByVal plantaAdmin As Integer, ByVal responsables As Integer()) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasActivasConTraResponsable(plantaAdmin, responsables)
        End Function

        Public Function loadContaTrabajadores(ByVal plantaAdmin As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadContaTrabajadores(plantaAdmin)
        End Function
        Public Function CargarListaEmpresastexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaDocumentostexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Documentos)
            Return documentosDAL.loadListDocumentosTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoActivas(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasTextoActivas(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoActivasResponsables(ByVal plantaAdmin As Integer, ByVal texto As String, ByVal responsables As Integer()) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasTextoActivasResponsables(plantaAdmin, texto, responsables)
        End Function
        Public Function CargarListaEmpresastextoActivasExacto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasTextoActivasExacto(plantaAdmin, texto)
        End Function
        Public Function CargarListaTratexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTraTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaTratextoEmp(ByVal Emp As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTraTextoEmp(Emp, texto)
        End Function
        Public Function CargarListaTratextoDNI(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTraTextoDNI(plantaAdmin, texto)
        End Function
        Public Function CargarListaResponsabletexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Responsables)
            Return documentosDAL.loadListResponsableTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaTrabajadorestexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores) 'no tengo el valor ByVal empresa As Integer,
            Return documentosDAL.loadListTrabajadoresTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaCursostexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores) 'no tengo el valor ByVal empresa As Integer,
            Return documentosDAL.loadListCursosTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaProfesiontexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores) 'no tengo el valor ByVal empresa As Integer,
            Return documentosDAL.loadListProfesionTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaResponsablestexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores) 'no tengo el valor ByVal empresa As Integer,
            Return documentosDAL.loadListResponsablesTexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaTrabajadorestextoResponsables(ByVal plantaAdmin As Integer, ByVal texto As String, ByVal responsables As Integer()) As List(Of ELL.Trabajadores) 'no tengo el valor ByVal empresa As Integer,
            Return documentosDAL.loadListTrabajadoresTextoResponsables(plantaAdmin, texto, responsables)
        End Function
        Public Function CargarListaTrabajadorestextoPuesto(ByVal plantaAdmin As Integer, ByVal texto As String, ByVal puesto As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresTextoPuesto(plantaAdmin, texto, puesto)
        End Function
        Public Function CargarListaTrabajadorestextoResponsables2(ByVal plantaAdmin As Integer, ByVal texto As String, ByVal responsables As Integer(), ByVal puestos As Integer()) As List(Of ELL.Trabajadores) 'no tengo el valor ByVal empresa As Integer,
            Return documentosDAL.loadListTrabajadoresTextoResponsables2(plantaAdmin, texto, responsables, puestos)
        End Function
        Public Function CargarListaDoctexto(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.TrabajadoresDoc) 'no tengo el valor ByVal empresa As Integer,
            Return documentosDAL.CargarListaDoctexto(plantaAdmin, texto)
        End Function
        Public Function CargarListaDocTraTipo(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.CargarListaDocTraTipo(plantaAdmin, tipo)
        End Function
        Public Function CargarListaDocEmpTipo(ByVal plantaAdmin As Integer, ByVal tipo As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.CargarListaDocEmpTipo(plantaAdmin, tipo)
        End Function
        Public Function CargarListaTrabajadorestextoNoCad(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Trabajadores) 'no tengo el valor ByVal empresa As Integer,
            Return documentosDAL.loadListTrabajadoresTextoNoCad(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoXBATCod(ByVal plantaAdmin As Integer, ByVal texto As Integer) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasTextoXBATCod(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoXBATNombre(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasTextoXBATNombre(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoXBATNombre2(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasTextoActivas(plantaAdmin, texto)
        End Function
        Public Function CargarListaEmpresastextoXBATCIF(ByVal plantaAdmin As Integer, ByVal texto As String) As List(Of ELL.Empresas)
            Return documentosDAL.loadListEmpresasTextoXBATCIF(plantaAdmin, texto)
        End Function
        Public Function CargarListaTrabajadorestexto(ByVal plantaAdmin As Integer, ByVal nombre As String, ByVal apellidos As String) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresTexto(plantaAdmin, nombre, apellidos)
        End Function
        'Public Function CargarListaCursostexto(ByVal plantaAdmin As Integer, codigo As Integer) As List(Of ELL.Trabajadores)
        '    Return documentosDAL.loadListCursosTexto(plantaAdmin, codigo)
        'End Function
        Public Function CargarListaTrabajadorestextoActivos(ByVal plantaAdmin As Integer, ByVal nombre As String, ByVal apellidos As String) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresTextoActivos(plantaAdmin, nombre, apellidos)
        End Function
        Public Function CargarListaTrabajadoresClaveEmp(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveEmp(plantaAdmin, Clave)
        End Function
        Public Function CargarListaTrabajadoresClaveEmpSub(ByVal plantaAdmin As Integer, ByVal Clave As String) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveEmpSub(plantaAdmin, Clave)
        End Function
        Public Function CargarListaTrabajadoresClaveEmpTODOS(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveEmpTODOS(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveEmp(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolicitudesClaveEmp(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveEmpETT(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolicitudesClaveEmpETT(plantaAdmin, Clave)
        End Function
        Public Function CargarListaCertificados(ByVal plantaAdmin As Integer) As List(Of ELL.Certificados)
            Return documentosDAL.CargarListaCertificados(plantaAdmin)
        End Function
        Public Function CargarListaCertificadosEmp(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Certificados)
            Return documentosDAL.CargarListaCertificadosEmp(plantaAdmin, Clave)
        End Function
        Public Function CargarListaCertificadosDoc(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Certificados)
            Return documentosDAL.CargarListaCertificadosDoc(plantaAdmin, Clave)
        End Function
        Public Function CargarListaCertificadosEmpDoc(ByVal plantaAdmin As Integer, ByVal Clave As Integer, ByVal doc As Integer) As List(Of ELL.Certificados)
            Return documentosDAL.CargarListaCertificadosEmpDoc(plantaAdmin, Clave, doc)
        End Function
        'Public Function CargarListaSolicitudesClaveEmpPed(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Trabajadores)
        '    '    Return documentosDAL.loadListTrabajadoresClaveEmp(plantaAdmin, Clave)
        '    Return documentosDAL.loadListSolicitudesClaveEmpPed(plantaAdmin, Clave)
        'End Function
        Public Function CargarListaSolicitudesClaveEmpRest(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolicitudesClaveEmpRest(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveEmpRestETT(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolicitudesClaveEmpRestETT(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveEmpRestAct(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolicitudesClaveEmpRestAct(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveEmpRest2(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolicitudesClaveEmpRest2(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveUser(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.CargarListaSolicitudesClaveUser(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveUserETT(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.CargarListaSolicitudesClaveUserETT(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveUserETTRRHH(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.CargarListaSolicitudesClaveUserETTRRHH(plantaAdmin, Clave)
        End Function
        Public Function CargarListaTrabajadoresClaveEmpEstado(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveEmpEstado(plantaAdmin, Clave)
        End Function
        Public Function CargarListaTrabajadoresClaveEmpEstadoResponsable(ByVal plantaAdmin As Integer, ByVal Clave As Integer, ByVal responsables As Integer()) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveEmpEstadoResponsables(plantaAdmin, Clave, responsables)
        End Function
        Public Function CargarListaTrabajadoresClaveEmpEstado2(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveEmpEstado2(plantaAdmin, Clave)
        End Function
        Public Function CargarListaTrabajadoresClaveEmpSolicitud(ByVal plantaAdmin As Integer, ByVal Clave As Integer, ByVal Empresa As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveEmpSolicitud(plantaAdmin, Clave, Empresa)
        End Function
        'Public Function CargarListaTrabajadoresClaveEmpSolicitud2(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Trabajadores)
        '    Return documentosDAL.loadListTrabajadoresClaveEmpSolicitud2(plantaAdmin, Clave)
        'End Function
        Public Function CargarListaTrabajadoresClaveTra(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadListTrabajadoresClaveTra(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveTra(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolicitudesClaveTra(plantaAdmin, Clave)
        End Function
        Public Function CargarListaSolicitudesClaveTraETT(ByVal plantaAdmin As Integer, ByVal Clave As Integer) As List(Of ELL.Solicitudes)
            Return documentosDAL.loadListSolicitudesClaveTraETT(plantaAdmin, Clave)
        End Function
        Public Function loadTrabajadores(ByVal empresa As Integer) As List(Of ELL.Trabajadores)
            Return documentosDAL.loadTrabajadores(empresa)
        End Function
        Public Function CargarListaEmpDocTot(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListEmpDoc(plantaAdmin)
        End Function
        Public Function CargarListaEmpDocTot2(ByVal nombre As String) As List(Of ELL.Documentos)
            Return documentosDAL.loadListEmpDoc2(nombre)
        End Function
        Public Function CargarListaEmpDocTotETT(ByVal plantaAdmin As Integer, ByVal area As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListEmpDocETT(plantaAdmin, area)
        End Function
        Public Function CargarListaEmpDocTotTra(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListEmpDocTra(plantaAdmin)
        End Function
        Public Function CargarListaEmpDocTotTra2(ByVal plantaAdmin As Integer, ByVal idempresa As Integer) As List(Of ELL.FinSemana)
            Return documentosDAL.loadListEmpDocTra2(plantaAdmin, idempresa)
        End Function
        Public Function CargarListaTraDocTot(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTraDoc(plantaAdmin)
        End Function
        Public Function CargarListaTraDocTotETT(ByVal plantaAdmin As Integer, ByVal codsol As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTraDocETT(plantaAdmin, codsol)
        End Function
        Public Function CargarListaTraDocTotAutonomo(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListTraDocAutonomo(plantaAdmin)
        End Function
        Public Function CargarListaEmpDocAsignados(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignados(plantaAdmin, codemp)
        End Function
        Public Function CargarListaEmpDocAsignadosMenor4(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignadosMenor4(plantaAdmin, codemp)
        End Function
        Public Function CargarListaEmpDocAsignadosMenor4ETT(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal soli As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignadosMenor4ETT(plantaAdmin, codemp, soli)
        End Function
        Public Function CargarListaEmpDocAsignadosTipo4(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignadosTipo4(plantaAdmin, codemp)
        End Function
        Public Function CargarListaEmpDocAsignadosTra(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignadosTra(plantaAdmin, codemp)
        End Function
        Public Function CargarListaEmpDocAsignadosTraCer(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignadosTraCer(plantaAdmin, coddoc, codemp)
        End Function
        Public Function CargarListaEmpDocAsignadosTraCer2(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignadosTraCer2(plantaAdmin, coddoc, codemp)
        End Function
        Public Function CargarListaEmpDocAsignadosTraCertipo5(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignadosTraCertipo5(plantaAdmin, coddoc, codemp)
        End Function
        Public Function CargarListaEmpDocAsignadosTraCer3(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer, ByVal fecha As Date) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignadosTraCer3(plantaAdmin, coddoc, codemp, fecha)
        End Function
        Public Function CargarListaEmpDocAsignados161(ByVal plantaAdmin As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignados161(plantaAdmin)
        End Function
        Public Function CargarListaEmpDocAsignados163(ByVal plantaAdmin As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDocAsignados163(plantaAdmin)
        End Function
        Public Function CargarListaTraDocAsignados(ByVal plantaadmin As Integer, ByVal codtra As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTraDocAsignados(plantaadmin, codtra)
        End Function
        Public Function CargarListaTraDocAsignadosMatriz(ByVal coddoc As Integer, ByVal codtra As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTraDocAsignadosMatriz(coddoc, codtra, plantaAdmin)
        End Function
        Public Function CargarListaTraDocAsignadosMatrizTodos(ByVal coddoc As Integer, ByVal codtra As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTraDocAsignadosMatrizTodos(coddoc, codtra, plantaAdmin)
        End Function
        Public Function CargarListaTraDocObligatorio(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListTraDocObligatorio(plantaAdmin, coddoc, codemp)
        End Function
        Public Function CargarListaTraDocObligatorioTRD(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codtra As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListTraDocObligatorioTRD(plantaAdmin, coddoc, codtra)
        End Function
        Public Function CargarListaTraDocObligatorio2(ByVal plantaAdmin As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListTraDocObligatorio2(plantaAdmin)
        End Function
        Public Function CargarListaTraDocObligatorio3(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codemp As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListTraDocObligatorio3(plantaAdmin, coddoc, codemp)
        End Function
        Public Function CargarListaTraDocObligatorio4(ByVal plantaAdmin As Integer, ByVal coddoc As Integer, ByVal codtra As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListTraDocObligatorio4(plantaAdmin, coddoc, codtra)
        End Function
        Public Function CargarListaTraDocObligatorio5(ByVal plantaAdmin As Integer, ByVal coddoc As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListTraDocObligatorio5(plantaAdmin, coddoc)
        End Function
        Public Function CargarListaTraDocAsignados156(ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTraDocAsignados156(plantaAdmin)
        End Function
        Public Function CargarListaTodosTraDocAsignados(ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTodosTraDocAsignados(plantaAdmin)
        End Function
        Public Function CargarListaMat() As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListMat()
        End Function
        Public Function CargarListaMatClave(ByVal Clave As String) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListMatClave(Clave)
        End Function
        Public Function SaveMatriz(ByVal curso As String, ByVal valores As String) As Boolean
            Return documentosDAL.SaveMatriz(curso, valores)
        End Function
        Public Function SaveMatrizInsert(ByVal curso As String, ByVal valores As String) As Boolean
            Return documentosDAL.SaveMatrizInsert(curso, valores)
        End Function
        Public Function DeleteMatriz() As Boolean
            Return documentosDAL.DeleteMatriz()
        End Function
        Public Function CargarListaMatriz2(ByVal codpuesto As String, ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListMatriz2(codpuesto, plantaAdmin)
        End Function
        Public Function CargarListaMatriz3(ByVal codpuesto As String, ByVal curso As Integer, ByVal todos As Integer, ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListMatriz3(codpuesto, curso, todos, plantaAdmin)
        End Function
        Public Function CargarListaMatrizCadaTra(ByVal codpuesto As Integer, ByVal trabajador As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListMatrizCadaTra(codpuesto, trabajador)
        End Function
        Public Function CargarListaHist(ByVal coddoc As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListhist(coddoc)
        End Function
        Public Function CargarListaMatriz(ByVal coddoc As Integer, ByVal planta As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListMatriz(coddoc, planta)
        End Function
        Public Function CargarListaHistFec(ByVal coddoc As String) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListHistFec(coddoc)
        End Function
        'Public Function CargarListaMatrizTrabajadores2(ByVal Intrabajador As String, ByVal profesion As Integer) As List(Of ELL.TrabajadoresDoc)
        '    Return documentosDAL.loadListMatrizTrabajadores2(Intrabajador, profesion)
        'End Function
        Public Function CargarListaMatrizTrabajadores(ByVal Intrabajador As String, ByVal profesion As Integer, ByVal responsable As Integer, ByVal todos As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListMatrizTrabajadores(Intrabajador, profesion, responsable, todos)
        End Function
        Public Function CargarListaMatrizTra(ByVal plantaAdmin As Integer) As List(Of ELL.TrabajadoresDocMatriz)
            Return documentosDAL.loadListMatrizTra(plantaAdmin)
        End Function
        Public Function CargarListaMatrizTraEmp(ByVal plantaAdmin As Integer, ByVal Empresa As Integer) As List(Of ELL.TrabajadoresDocMatriz)
            Return documentosDAL.loadListMatrizTraEmp(plantaAdmin, Empresa)
        End Function
        Public Function CargarListaMatrizTraEmpTra(ByVal plantaAdmin As Integer, ByVal Empresa As Integer, ByVal puesto As Integer) As List(Of ELL.TrabajadoresDocMatriz)
            Return documentosDAL.loadListMatrizTraEmpTra(plantaAdmin, Empresa, puesto)
        End Function
        Public Function CargarListaMatrizTrabajador(ByVal plantaAdmin As Integer, ByVal Empresa As Integer) As List(Of ELL.TrabajadoresDocMatriz)
            Return documentosDAL.loadListMatrizTrabajador(plantaAdmin, Empresa)
        End Function
        Public Function CargarListaMatrizDoc(ByVal plantaAdmin As Integer) As List(Of ELL.Documentos)
            Return documentosDAL.loadListMatrizDoc(plantaAdmin)
        End Function
        Public Function CargarListaTodosEmpDocAsignados(ByVal plantaAdmin As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListTodosEmpDocAsignados(plantaAdmin)
        End Function
        Public Function CargarListaEmpDoc(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal coddoc As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListEmpDoc(plantaAdmin, codemp, coddoc)
        End Function
        Public Function CargarListaSolDoc(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal coddoc As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListSolDoc(plantaAdmin, codemp, coddoc)
        End Function
        Public Function CargarListaSolDocETT(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal coddoc As Integer) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.loadListSolDocETT(plantaAdmin, codemp, coddoc)
        End Function
        Public Function CargarListaFS(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal codtra As Integer, ByVal fecha As Date) As List(Of ELL.FinSemana)
            Return documentosDAL.loadListFS(plantaAdmin, codemp, codtra, fecha)
        End Function
        Public Function CargarListaFS1(ByVal plantaAdmin As Integer, ByVal codemp As Integer) As List(Of ELL.FinSemana)
            Return documentosDAL.loadListFS1(plantaAdmin, codemp)
        End Function
        Public Function CargarListaFS2(ByVal plantaAdmin As Integer, ByVal fecha As String) As List(Of ELL.FinSemana)
            Return documentosDAL.loadListFS2(plantaAdmin, fecha)
        End Function
        Public Function CargarListaFS3(ByVal plantaAdmin As Integer, ByVal codemp As Integer, ByVal fecha As String) As List(Of ELL.FinSemana)
            Return documentosDAL.loadListFS3(plantaAdmin, codemp, fecha)
        End Function
        Public Function CargarListaTraDoc(ByVal plantaAdmin As Integer, ByVal codtra As Integer, ByVal coddoc As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTraDoc(plantaAdmin, codtra, coddoc)
        End Function
        Public Function CargarListaTraSol(ByVal plantaAdmin As Integer, ByVal codtra As Integer, ByVal codsol As Integer, ByVal codemp As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTraSol(plantaAdmin, codtra, codsol, codemp)
        End Function
        'Public Function CargarListaTraSolETT(ByVal plantaAdmin As Integer, ByVal codtra As Integer, ByVal codsol As Integer, ByVal codemp As Integer) As List(Of ELL.TrabajadoresDoc)
        '    Return documentosDAL.loadListTraSolETT(plantaAdmin, codtra, codsol, codemp)
        'End Function
        Public Function CargarListaTraSol2(ByVal plantaAdmin As Integer, ByVal codsol As Integer, ByVal codemp As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTraSol2(plantaAdmin, codsol, codemp)
        End Function
        Public Function CargarListaTraSolClave(ByVal plantaAdmin As Integer, ByVal codsol As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.loadListTraSolClave(plantaAdmin, codsol)
        End Function
        Public Function CargarListaPlanta() As List(Of ELL.Documentos)
            Return documentosDAL.loadListPlanta()
        End Function
#End Region

#Region "Modificaciones"

        Public Function GuardarTipo(ByVal doc As ELL.Documentos) As Boolean
            Return documentosDAL.Save(doc)
        End Function
        Public Function GuardarResponsables(ByVal Responsables As ELL.Responsables) As Boolean
            Return documentosDAL.SaveResponsables(Responsables)
        End Function
        Public Function GuardarCaducidades(ByVal Caducidades As ELL.Caducidades) As Boolean
            Return documentosDAL.SaveCaducidades(Caducidades)
        End Function
        Public Function GuardarRol(ByVal rol As ELL.Rol) As Boolean
            Return documentosDAL.SaveRol(rol)
        End Function
        Public Function GuardarRolETT(ByVal rol As ELL.Rol) As Boolean
            Return documentosDAL.SaveRolETT(rol)
        End Function
        Public Function GuardarEmail(ByVal rol As ELL.Rol) As Boolean
            Return documentosDAL.SaveEmail(rol)
        End Function
        Public Function GuardarEmailETT(ByVal rol As ELL.Rol) As Boolean
            Return documentosDAL.SaveEmailETT(rol)
        End Function
        Public Function GuardarEmp(ByVal doc As ELL.Empresas) As Boolean
            Return documentosDAL.SaveEmp(doc)
        End Function
        Public Function GuardarEmpSAB(ByVal doc As ELL.Empresas) As List(Of String())
            Return documentosDAL.SaveEmpSAB(doc)
        End Function
        Public Function GuardarEmpADOK(ByVal doc As ELL.Empresas) As List(Of String())
            Return documentosDAL.SaveEmpADOK(doc)
        End Function
        Public Function GuardarUserSAB(ByVal doc As ELL.Empresas) As List(Of String())
            Return documentosDAL.SaveUserSAB(doc)
        End Function
        Public Function GuardarUserSABExiste(ByVal doc As ELL.Empresas) As List(Of String())
            Return documentosDAL.SaveUserSABExiste(doc)
        End Function
        Public Function GuardarUserSABExisteModificar(ByVal doc As ELL.Empresas) As List(Of String())
            Return documentosDAL.SaveUserSABExisteModificar(doc)
        End Function
        Public Function GuardarPlantaSAB(ByVal user As Integer, ByVal planta As Integer) As Boolean
            Return documentosDAL.SavePlantaSAB(user, planta)
        End Function
        Public Function GuardarTra(ByVal doc As ELL.Trabajadores) As Boolean
            Return documentosDAL.SaveTra(doc)
        End Function
        Public Function GuardarSol(ByVal doc As ELL.Solicitudes) As Boolean
            Return documentosDAL.SaveSol(doc)
        End Function
        Public Function GuardarSolETT(ByVal doc As ELL.Solicitudes) As Boolean
            Return documentosDAL.SaveSolETT(doc)
        End Function
        Public Function GuardarCer(ByVal planta As Integer, ByVal coddoc As Integer, ByVal codemp As Integer, ByVal aceptado As Integer, ByVal nombre As String) As Boolean
            Return documentosDAL.SaveCer(planta, coddoc, codemp, aceptado, nombre)
        End Function
        Public Function GuardarDorlet(ByVal doc As ELL.Dorlet) As Boolean
            Return documentosDAL.SaveDorlet(doc)
        End Function
        Public Function GuardarPlant(ByVal pla As ELL.Plantillas) As Boolean
            Return documentosDAL.SavePlant(pla)
        End Function
        Public Function GuardarPlant2(ByVal pla As ELL.Plantillas) As Boolean
            Return documentosDAL.SavePlant2(pla)
        End Function
        Public Function ModificarTipo(ByVal doc As ELL.Documentos, ByVal codigo As Integer) As Boolean
            Return documentosDAL.Update(doc, codigo)
        End Function
        Public Function ModificarResp(ByVal Responsables As ELL.Responsables, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateResponsables(Responsables, codigo)
        End Function
        Public Function ModificarCad(ByVal Caducidades As ELL.Caducidades, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateCaducidades(Caducidades, codigo)
        End Function
        Public Function ModificarRol(ByVal Rol As ELL.Rol, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateRol(Rol, codigo)
        End Function
        Public Function ModificarRolETT(ByVal Rol As ELL.Rol, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateRolETT(Rol, codigo)
        End Function
        Public Function UpdateTraCaducados() As Boolean
            Return documentosDAL.UpdateTraCaducados()
        End Function
        Public Function ModificarEmp(ByVal doc As ELL.Empresas, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmp(doc, codigo)
        End Function
        Public Function UpdateEmailSol(ByVal doc As ELL.Empresas, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateEmailSol(doc, codigo)
        End Function

        Public Function ModificarTra(ByVal doc As ELL.Trabajadores, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateTra(doc, codigo)
        End Function
        Public Function ModificarSol(ByVal doc As ELL.Solicitudes, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateSol(doc, codigo)
        End Function
        Public Function ModificarSolETT(ByVal doc As ELL.Solicitudes, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateSolETT(doc, codigo)
        End Function
        Public Function ModificarSolSub(ByVal doc As ELL.Solicitudes, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateSolSub(doc, codigo)
        End Function
        Public Function UpdateSolSubcontrata(ByVal doc As ELL.Solicitudes, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateSolSubcontrata(doc, codigo)
        End Function

        Public Function ModificarDorLet(ByVal doc As ELL.Dorlet, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateDorlet(doc, codigo)
        End Function
        Public Function BorrarPuestos() As Boolean
            Return documentosDAL.BorrarPuestos()
        End Function
        Public Function BorrarTrabajadores() As Boolean
            Return documentosDAL.BorrarTrabajadores()
        End Function
        Public Function ModificarEmpDoc(ByVal doc As ELL.EmpresasDoc, ByVal codigo As Integer, ByVal nombre As String) As Boolean
            Return documentosDAL.UpdateEmpDoc(doc, codigo, nombre)
        End Function
        Public Function ModificarEmpDocETT(ByVal doc As ELL.EmpresasDoc, ByVal codigo As Integer, ByVal nombre As String) As Boolean
            Return documentosDAL.UpdateEmpDocETT(doc, codigo, nombre)
        End Function
        Public Function ModificarSolDoc(ByVal doc As ELL.EmpresasDoc, ByVal codigo As Integer, ByVal nombre As String) As Boolean
            Return documentosDAL.UpdateSolDoc(doc, codigo, nombre)
        End Function
        Public Function ModificarSolDocETT(ByVal doc As ELL.EmpresasDoc, ByVal codigo As Integer, ByVal nombre As String) As Boolean
            Return documentosDAL.UpdateSolDocETT(doc, codigo, nombre)
        End Function
        Public Function ModificarFS(ByVal planta As Integer, ByVal empresa As Integer, ByVal trabajador As Integer, ByVal fecha As Date, ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateFS(planta, empresa, trabajador, fecha, codigo)
        End Function
        Public Function ModificarTraDoc(ByVal doc As ELL.TrabajadoresDoc, ByVal codigo As Integer, ByVal nombre As String) As Boolean
            Return documentosDAL.UpdateTraDoc(doc, codigo, nombre)
        End Function
        Public Function ModificarTraDoc2(ByVal doc As ELL.TrabajadoresDoc, ByVal codigo As Integer, ByVal nombre As String) As Boolean
            Return documentosDAL.UpdateTraDoc2(doc, codigo, nombre)
        End Function
        Public Function ModificarTraDoc3(ByVal doc As ELL.TrabajadoresDoc, ByVal codigo As Integer, ByVal nombre As String) As Boolean
            Return documentosDAL.UpdateTraDoc3(doc, codigo, nombre)
        End Function
        Public Function DesactivarTraDoc(ByVal cod As String) As Boolean
            Return documentosDAL.DesactivarTraDoc(cod)
        End Function
        Public Function ModificarTraSol(ByVal doc As ELL.TrabajadoresDoc, ByVal codigo As Integer, ByVal responsable As Integer) As Boolean
            Return documentosDAL.UpdateTraSol(doc, codigo, responsable)
        End Function
        Public Function ModificarTraSolETT(ByVal doc As ELL.TrabajadoresDoc, ByVal codigo As Integer, ByVal responsable As Integer) As Boolean
            Return documentosDAL.UpdateTraSolETT(doc, codigo, responsable)
        End Function
        Public Function ModificarTraDocSol(ByVal doc As ELL.TrabajadoresDoc, ByVal codigo As Integer, ByVal tipo As Integer) As Boolean
            Return documentosDAL.UpdateTraDocSol(doc, codigo, tipo)
        End Function
        Public Function ModificarEmd(ByVal doc As ELL.EmpresasDoc) As Boolean
            Return documentosDAL.UpdateEmd(doc)
        End Function
        Public Function ModificarEmdEmp(ByVal doc As ELL.EmpresasDoc) As Boolean
            Return documentosDAL.UpdateEmdEmp(doc)
        End Function
        Public Function ModificarTrdTra(ByVal doc As ELL.TrabajadoresDoc) As Boolean
            Return documentosDAL.UpdateTrdTra(doc)
        End Function
        Public Function ModificarTra(ByVal doc As ELL.Trabajadores) As Boolean
            Return documentosDAL.UpdateTra(doc)
        End Function
        Public Function ModificarTrd(ByVal codigo As Integer) As Boolean
            Return documentosDAL.UpdateTrd(codigo)
        End Function
        Public Function ModificarTraFecha(ByVal doc As ELL.Trabajadores) As Boolean
            Return documentosDAL.UpdateTraFecha(doc)
        End Function
        Public Function ModificarEmp(ByVal doc As ELL.Trabajadores) As Boolean
            Return documentosDAL.UpdateEmp(doc)
        End Function
        Public Function ModificarSolDoc(ByVal doc As ELL.Trabajadores) As Boolean
            Return documentosDAL.UpdateSolDoc(doc)
        End Function
        'Public Function ModificarSolDocETT(ByVal doc As ELL.Trabajadores) As Boolean
        '    Return documentosDAL.UpdateSolDocETT(doc)
        'End Function
        Public Function ModificarDOS(ByVal doc As ELL.Trabajadores) As Boolean
            Return documentosDAL.UpdateDOS(doc)
        End Function
        Public Function ModificarDOSTra(ByVal doc As ELL.Trabajadores) As Boolean
            Return documentosDAL.UpdateDOSTra(doc)
        End Function
        Public Function ModificarSol(ByVal doc As ELL.Solicitudes) As Boolean
            Return documentosDAL.UpdateSol(doc)
        End Function
        Public Function ModificarSolETT(ByVal doc As ELL.Solicitudes) As Boolean
            Return documentosDAL.UpdateSolETT(doc)
        End Function
        Public Function ModificarDocAct(ByVal doc As ELL.Documentos) As Boolean
            Return documentosDAL.UpdateDocAct(doc)
        End Function
        Public Function DeletePer(ByVal planta As Integer, ByVal codigo As Integer) As Boolean
            Return documentosDAL.DeletePer(planta, codigo)
        End Function
        Public Function DeleteSol(ByVal planta As Integer, ByVal codigo As Integer) As Boolean
            Return documentosDAL.DeleteSol(planta, codigo)
        End Function
        Public Function DeleteRes(ByVal planta As Integer, ByVal codigo As Integer) As Boolean
            Return documentosDAL.DeleteRes(planta, codigo)
        End Function
        Public Function DeleteRol(ByVal planta As Integer, ByVal codigo As Integer, ByVal rol As Int32) As Boolean
            Return documentosDAL.DeleteRol(planta, codigo, rol)
        End Function
        Public Function DeleteRolETT(ByVal planta As Integer, ByVal codigo As Integer) As Boolean
            Return documentosDAL.DeleteRolETT(planta, codigo)
        End Function
        Public Function DeleteEmail(ByVal planta As Integer, ByVal codigo As Integer) As Boolean
            Return documentosDAL.DeleteEmail(planta, codigo)
        End Function
        Public Function DeleteEmailETT(ByVal planta As Integer, ByVal codigo As Integer) As Boolean
            Return documentosDAL.DeleteEmailETT(planta, codigo)
        End Function
        'Public Function ModificarEmpDocTodos(ByVal doc As ELL.EmpresasDoc, ByVal codigo As Integer) As Boolean
        '    Return documentosDAL.UpdateEmpDocTodos(doc, codigo)
        'End Function
        Public Function LeerEmpDoc(ByVal doc As ELL.EmpresasDoc) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.LeerEmpDoc(doc)
        End Function
        Public Function LeerTraDoc(ByVal doc As ELL.TrabajadoresDoc) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.LeerTraDoc(doc)
        End Function
        Public Function LeerEmpDocHis(ByVal doc As ELL.EmpresasDoc) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.LeerEmpDocHis(doc)
        End Function
        Public Function LeerTraDocHis(ByVal doc As ELL.TrabajadoresDoc) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.LeerTraDocHis(doc)
        End Function
        Public Function LeerTraDocHisTodos(ByVal doc As ELL.TrabajadoresDoc, ByVal todos As Integer) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.LeerTraDocHisTodos(doc, todos)
        End Function
        Public Function LeerTraDocHisCurso(ByVal doc As ELL.Trabajadores, ByVal todos As Integer) As List(Of ELL.TrabajadoresDocMatriz)
            Return documentosDAL.LeerTraDocHisCurso(doc, todos)
        End Function
        Public Function LeerTraDocHis2(ByVal doc As ELL.TrabajadoresDocMatriz) As List(Of ELL.TrabajadoresDocMatriz)
            Return documentosDAL.LeerTraDocHis2(doc)
        End Function
        Public Function LeerEmpDocHisClave(ByVal doc As ELL.EmpresasDoc) As List(Of ELL.EmpresasDoc)
            Return documentosDAL.LeerEmpDocHisClave(doc)
        End Function
        Public Function LeerTraDocHisClave(ByVal doc As ELL.TrabajadoresDoc) As List(Of ELL.TrabajadoresDoc)
            Return documentosDAL.LeerTraDocHisClave(doc)
        End Function


#End Region



        Public Function CargarPerfilUsuario(ByVal idUsuario As Integer, ByVal idPlanta As String) As ELL.PerfilUsuario
            'jon     If System.Configuration.ConfigurationManager.AppSettings("extranet").ToString = "1" Then
            Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador} 'extranet
            '  End If
            ' Llenamos los datos del perfil del usuario
            Dim RolUser As List(Of ELL.Rol)
            '     Dim RolBBDD As Integer
            Dim oDocumentosBLL As New BLL.DocumentosBLL
            RolUser = oDocumentosBLL.CargarRol(idUsuario, idPlanta)
            If RolUser.Count > 0 Then
                'si hacemos que tenga mas de uno poner estos for por cada rol
                For i = 0 To RolUser.Count - 1
                    If RolUser(i).Id = ELL.Roles.RolUsuario.Administrador Then
                        Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador}
                    End If
                Next
                For i = 0 To RolUser.Count - 1
                    If RolUser(i).Id = ELL.Roles.RolUsuario.Supervisores Then
                        Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Supervisores}
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

            ' Llenamos los datos del perfil del usuario
            Dim RolUser As List(Of ELL.Rol)
            '     Dim RolBBDD As Integer
            Dim oDocumentosBLL As New BLL.DocumentosBLL
            RolUser = oDocumentosBLL.CargarRolETT(idUsuario, idPlanta)
            If RolUser.Count > 0 Then
                'si hacemos que tenga mas de uno poner estos for por cada rol
                For i = 0 To RolUser.Count - 1
                    If RolUser(i).Id = ELL.Roles.RolUsuario.RRHH Then
                        Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.RRHH}
                    End If
                Next
                For i = 0 To RolUser.Count - 1
                    If RolUser(i).Id = ELL.Roles.RolUsuario.Administrador Then
                        Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador}
                    End If
                Next
                For i = 0 To RolUser.Count - 1
                    If RolUser(i).Id = ELL.Roles.RolUsuario.Administrador2 Then
                        Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Administrador2}
                    End If
                Next
                For i = 0 To RolUser.Count - 1
                    If RolUser(i).Id = ELL.Roles.RolUsuario.Usuario Then
                        Return New ELL.PerfilUsuario With {.IdUsuario = idUsuario, .IdRol = ELL.Roles.RolUsuario.Usuario}
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

        Public Function Existe(ByVal id As String, ByVal rol As Integer, ByVal planta As Integer) As Boolean
            If (documentosDAL.existe(id, rol, planta) > 0) Then
                Return True
            Else : Return False
            End If
        End Function
        Public Function ExisteETT(ByVal id As String) As Boolean
            If (documentosDAL.existeETT(id) > 0) Then
                Return True
            Else : Return False
            End If
        End Function


    End Class

End Namespace
