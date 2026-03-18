
Namespace BLL

    Public Class BrainBLL

        Private brainDAL As New DAL.BrainDAL

#Region "UNIDAD MEDIDA"

        ''' <summary>
        ''' Obtiene el nombre de unidad de medida
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreUnidadMedida(ByVal idUnidad As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarNombreUnidadMedida(idUnidad, empresa)
        End Function

        ''' <summary>
        ''' Obtiene todas las unidades de medida
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUnidadesMedida(ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarUnidadesMedida(empresa)
        End Function

        ''' <summary>
        ''' Obtiene las unidades de medida que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUnidadMedida(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarUnidadMedida(texto, empresa)
        End Function

        ''' <summary>
        ''' Obtiene las unidades de medida que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarUnidadesMedida(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarUnidadesMedida(texto, empresa)
        End Function

#End Region

#Region "CATEGORIAS PRODUCTO"

        ''' <summary>
        ''' Obtiene el nombre de categoría de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreCategoriaProducto(ByVal idCategoriaProducto As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarNombreCategoriaProducto(idCategoriaProducto, empresa)
        End Function

        ''' <summary>
        ''' Obtiene todas las categorías de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarCategoriasProducto(ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarCategoriasProducto(empresa)
        End Function

        ''' <summary>
        ''' Obtiene la categoría de producto que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarCategoriaProducto(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarCategoriaProducto(texto, empresa)
        End Function

        ''' <summary>
        ''' Obtiene las categorías de producto que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarCategoriasProducto(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarCategoriasProducto(texto, empresa)
        End Function

#End Region

#Region "GRUPOS MATERIAL"

        ''' <summary>
        ''' Obtiene el nombre del grupo de material
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreGrupoMaterial(ByVal idGrupoMaterial As String) As ELL.BrainBase
            Return brainDAL.CargarNombreGrupoMaterial(idGrupoMaterial)
        End Function

        ''' <summary>
        ''' Obtiene todos los grupos de material
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarGruposMaterial() As List(Of ELL.BrainBase)
            Return brainDAL.CargarGruposMaterial()
        End Function

        ''' <summary>
        ''' Obtiene el grupos de material que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarGrupoMaterial(ByVal texto As String) As ELL.BrainBase
            Return brainDAL.CargarGrupoMaterial(texto)
        End Function

        ''' <summary>
        ''' Obtiene los grupos de material que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarGruposMaterial(ByVal texto As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarGruposMaterial(texto)
        End Function

#End Region

#Region "GRUPOS PRODUCTO"

        ''' <summary>
        ''' Obtiene el nombre del grupo de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreGrupoProducto(ByVal idGrupoProducto As String) As ELL.BrainBase
            Return brainDAL.CargarNombreGrupoProducto(idGrupoProducto)
        End Function

        ''' <summary>
        ''' Obtiene todos los grupos de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarGruposProducto() As List(Of ELL.BrainBase)
            Return brainDAL.CargarGruposProducto()
        End Function

        ''' <summary>
        ''' Obtiene el grupos de producto que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarGrupoProducto(ByVal texto As String) As ELL.BrainBase
            Return brainDAL.CargarGrupoProducto(texto)
        End Function

        ''' <summary>
        ''' Obtiene los grupos de producto que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarGruposProducto(ByVal texto As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarGruposProducto(texto)
        End Function

#End Region

#Region "TIPOS PRODUCTO"

        ''' <summary>
        ''' Obtiene el nombre del tipo de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreTipoProducto(ByVal idTipoProducto As String) As ELL.BrainBase
            Return brainDAL.CargarNombreTipoProducto(idTipoProducto)
        End Function

        ''' <summary>
        ''' Obtiene todos los tipos de producto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposProducto() As List(Of ELL.BrainBase)
            Return brainDAL.CargarTiposProducto()
        End Function

        ''' <summary>
        ''' Obtiene el tipo de producto que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTipoProducto(ByVal texto As String) As ELL.BrainBase
            Return brainDAL.CargarTipoProducto(texto)
        End Function

        ''' <summary>
        ''' Obtiene los tipos de producto que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposProducto(ByVal texto As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarTiposProducto(texto)
        End Function

#End Region

#Region "TIPOS PIEZA"

        ''' <summary>
        ''' Obtiene el nombre del tipo de pieza
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreTipoPieza(ByVal idTipoPieza As String) As ELL.BrainBase
            Return brainDAL.CargarNombreTipoPieza(idTipoPieza)
        End Function

        ''' <summary>
        ''' Obtiene todos los tipos de pieza
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposPieza() As List(Of ELL.BrainBase)
            Return brainDAL.CargarTiposPieza()
        End Function

        ''' <summary>
        ''' Obtiene el tipo de pieza que cumpla los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTipoPieza(ByVal texto As String) As ELL.BrainBase
            Return brainDAL.CargarTipoPieza(texto)
        End Function

        ''' <summary>
        ''' Obtiene los tipos de pieza que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarTiposPieza(ByVal texto As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarTiposPieza(texto)
        End Function

        ' ''' <summary>
        ' ''' Obtiene todos los tipos de pieza
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function CargarTiposPieza(ByVal empresa As String) As List(Of ELL.BrainBase)
        '    Return brainDAL.CargarTiposPieza(empresa)
        'End Function

        ' ''' <summary>
        ' ''' Obtiene el tipo de pieza que cumpla los requisitos del texto introducido
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function CargarTipoPieza(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
        '    Return brainDAL.CargarTipoPieza(texto, empresa)
        'End Function

        ' ''' <summary>
        ' ''' Obtiene los tipos de pieza que cumplan los requisitos del texto introducido
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function CargarTiposPieza(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
        '    Return brainDAL.CargarTiposPieza(texto, empresa)
        'End Function

#End Region

#Region "DISPONENTES"

        ''' <summary>
        ''' Obtiene el nombre del disponente
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreDisponente(ByVal idDisponente As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarNombreDisponente(idDisponente, empresa)
        End Function

        ''' <summary>
        ''' Obtiene todos los disponentes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDisponentes(ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarDisponentes(empresa)
        End Function

        ''' <summary>
        ''' Obtiene el disponente que cumpla los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDisponente(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarDisponente(texto, empresa)
        End Function

        ''' <summary>
        ''' Obtiene los disponentes que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDisponentes(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarDisponentes(texto, empresa)
        End Function

#End Region

#Region "ALMACENES"

        ''' <summary>
        ''' Obtiene el nombre del almacén
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreAlmacen(ByVal idAlmacen As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarNombreAlmacen(idAlmacen, empresa)
        End Function

        ''' <summary>
        ''' Obtiene todos los almacenes
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarAlmacenes(ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarAlmacenes(empresa)
        End Function

        ''' <summary>
        ''' Obtiene el almacen que cumpla los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarAlmacen(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarAlmacen(texto, empresa)
        End Function

        ''' <summary>
        ''' Obtiene los almacenes que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarAlmacenes(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarAlmacenes(texto, empresa)
        End Function

#End Region

#Region "PROYECTOS"

        ''' <summary>
        ''' Obtiene el listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista(ByVal texto As String) As List(Of ELL.Proyectos)
            Return brainDAL.loadList(texto)
        End Function

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectoPorId(ByVal id As String) As String
            Return brainDAL.CargarProyectoPorId(id)
        End Function

        ''' <summary>
        ''' Obtiene el listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectosBrain() As List(Of ELL.BrainBase)
            Return brainDAL.CargarProyectosBrain()
        End Function

        ''' <summary>
        ''' Obtiene un proyecto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectoBrain(ByVal texto As String) As ELL.BrainBase
            Return brainDAL.CargarProyectoBrain(texto)
        End Function

        ''' <summary>
        ''' Obtiene el listado
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarProyectosBrain(ByVal texto As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarProyectosBrain(texto)
        End Function

#End Region

#Region "SUBPROYECTOS"

        ''' <summary>
        ''' Obtiene el nombre del subproyecto
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarNombreSubproyecto(ByVal idSubproyecto As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarNombreSubproyecto(idSubproyecto, empresa)
        End Function

        ''' <summary>
        ''' Obtiene todos los subproyectos
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarSubproyectos(ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarSubproyectos(empresa)
        End Function

        ''' <summary>
        ''' Obtiene el subproyecto que cumpla los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarSubproyecto(ByVal texto As String, ByVal empresa As String) As ELL.BrainBase
            Return brainDAL.CargarSubproyecto(texto, empresa)
        End Function

        ''' <summary>
        ''' Obtiene los subproyectos que cumplan los requisitos del texto introducido
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarSubproyectos(ByVal texto As String, ByVal empresa As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarSubproyectos(texto, empresa)
        End Function

#End Region

#Region "PORTADORES DE COSTE"

        ''' <summary>
        ''' Devuelve el siguiente portador RP de coste para un producto
        ''' </summary>
        ''' <param name="producto"></param>
        ''' <param name="anteriorBatzPN"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>        
        Public Function GetSiguienteBatzPN(ByVal producto As String, ByRef anteriorBatzPN As String) As String
            Return brainDAL.GetSiguienteBatzPN(producto, anteriorBatzPN)
        End Function

#End Region

#Region "PREVIOUS BATZ PART NUMBER"

        ''' <summary>
        ''' Obtiene el listado de Batz Part Number ya registrados
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPreviousBatzPN(ByVal texto As String) As List(Of ELL.BrainBase)
            Return brainDAL.CargarPreviousBatzPN(texto)
        End Function

#End Region

#Region "SELECT, INSERT, UPDATE, DELETE REFERENCIAS EN CUBOS.SOLICIPZA"

#Region "SELECT"

        ''' <summary>
        ''' Cargar los datos de una referencia guardada en Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarReferenciaBrain(ByVal referencia As String) As ELL.DatosBrain
            Return brainDAL.CargarReferenciaBrain(referencia)
        End Function

        ' ''' <summary>
        ' ''' Verificar si existe una referencia en Brain
        ' ''' </summary>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function ExisteReferenciaBrain(ByVal referencia As String) As Boolean
        '    Return brainDAL.ExisteReferenciaBrain(referencia)
        'End Function

        ''' <summary>
        ''' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarIntegracionReferenciasBrainRechazo(ByVal idSolicitud As Integer) As Boolean
            Dim oSolicitudesBLL As New BLL.SolicitudesBLL
            Dim numPlantas As Integer = oSolicitudesBLL.CargarPlantasAfectadasSolicitud(idSolicitud)
            If (brainDAL.VerificarInegracionReferenciasBrain(idSolicitud) = numPlantas) Then
                Return True
            Else
                Return False
            End If
        End Function

        ''' <summary>
        ''' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        ''' </summary>
        ''' <param name="idSolicitud">Identificador de la solicitud</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarIntegracionReferenciasBrain(ByVal idSolicitud As Integer, ByVal referencia As ELL.ReferenciaVenta) As Boolean
            If (referencia.TipoNumero = ELL.ReferenciaVenta.NumberType.Development) Then
                Return False
            Else
                Dim numPlantas As Integer = brainDAL.CargarPlantasAfectadasReferencia(referencia.CustomerPartNumber, idSolicitud)
                If (brainDAL.VerificarInegracionReferenciasBrainSolicipza(idSolicitud, referencia.CustomerPartNumber) = numPlantas) Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        '' <summary>
        '' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        '' </summary>
        '' <param name="idSolicitud">Identificador de la solicitud</param>
        '' <returns></returns>
        '' <remarks></remarks>
        Public Function VerificarIntegracionReferenciasBrain(ByVal idSolicitud As Integer, Optional ByVal idRef As String = "") As Boolean
            If Not (String.IsNullOrEmpty(idRef)) Then
                Dim numPlantas As Integer = brainDAL.CargarPlantasAfectadasReferencia(idRef, idSolicitud)
                If (brainDAL.VerificarInegracionReferenciasBrainSolicipza(idSolicitud, idRef) = numPlantas) Then
                    Return True
                Else
                    Return False
                End If
            Else
                If (brainDAL.VerificarInegracionReferenciasBrainSolicipza(idSolicitud) > 0) Then
                    Return True
                Else
                    Return False
                End If
            End If

        End Function

        '''' <summary>
        '''' Verificar que todas las referencias de una solicitud han sido integrados en el maestro de piezas de Brain satisfactoriamente
        '''' </summary>
        '''' <param name="idSolicitud">Identificador de la solicitud</param>
        '''' <returns></returns>
        '''' <remarks></remarks>
        'Public Function VerificarIntegracionReferenciasBrain_AprobarSolicitud(ByVal idSolicitud As Integer) As Boolean
        '    Dim oSolicitudesBLL As New BLL.SolicitudesBLL
        '    Dim numPlantas As Integer = oSolicitudesBLL.CargarPlantasAfectadasSolicitud(idSolicitud)
        '    If (brainDAL.VerificarInegracionReferenciasBrain(idSolicitud) = numPlantas) Then
        '        Return True
        '    Else
        '        Return False
        '    End If
        'End Function

        ''' <summary>
        ''' Verificar que se han generado todas las referencias en la tabla Solicipza en Brain (faltaría por añadir la referencia que se va a introducir a continuación)
        ''' </summary>
        ''' <param name="idSolicitud"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function VerificarReferenciasBrainSolicipza(ByVal idSolicitud As Integer, ByVal idReferencia As Integer) As Boolean
            Dim oSolicitudesBLL As New BLL.SolicitudesBLL
            Dim oReferenciasVentaBLL As New BLL.ReferenciaFinalVentaBLL
            Dim numPlantas As Integer = oSolicitudesBLL.CargarPlantasAfectadasSolicitud(idSolicitud)
            Dim plantasGuardadas As Integer = brainDAL.VerificarReferenciasBrainSolicipza(idSolicitud)
            Dim plantasReferencia As Integer = oReferenciasVentaBLL.CargarPlantasReferencia(idReferencia).Count
            If (numPlantas = plantasGuardadas + plantasReferencia) Then
                Return True
            Else
                Return False
            End If
        End Function

#End Region

#Region "INSERT"

        ''' <summary>
        ''' Guardar referencia en Brain
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GuardarReferenciaBrain(ByVal datosBrain As ELL.DatosBrain, ByVal procesadoListo As Boolean) As Boolean
            Return brainDAL.GuardarReferenciaBrain(datosBrain, procesadoListo)
        End Function

#End Region

#Region "UPDATE"

        ''' <summary>
        ''' Modificar referencia en Brain
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ModificarReferenciaBrain(ByVal datosBrain As ELL.DatosBrain) As Boolean
            Return brainDAL.ModificarReferenciaBrain(datosBrain)
        End Function

#End Region

#Region "DELETE"

        ''' <summary>
        ''' Elimina la referencia generada en Brain (la solicitud de la referencia aún no ha sido tramitada)
        ''' </summary>
        ''' <param name="referencia">Identificador de la referencia</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminacionBrainReferenciaVenta(ByVal referencia As ELL.ReferenciaVenta) As Boolean
            Return brainDAL.EliminacionBrainReferenciaVenta(referencia)
        End Function

        ''' <summary>
        ''' Elimina todas las referencias de una solicitud que hayan podido ser generadas en Brain (la solicitud de las referencias aún no ha sido tramitada)
        ''' </summary>
        ''' <param name="referencias">Listado de referencias a eliminar en Brain</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function EliminarReferenciasVentaSolicitudBrain(ByVal referencias As List(Of ELL.ReferenciaVenta)) As Boolean
            Return brainDAL.EliminarReferenciasVentaSolicitudBrain(referencias)
        End Function

#End Region

#End Region

#Region "SELECT, INSERT, UPDATE, DELETE REFERENCIAS EN MAESTRO DE PIEZAS X33SD.TEILS"

        ''' <summary>
        ''' Cargar los datos de una referencia guardada en el maestro de piezas de Brain por la referencia de Batz
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDatosAltaReferenciaPiezaMaestroBrain(ByVal referencia As String) As ELL.MaestroPiezasBrainResumen
            Return brainDAL.CargarDatosAltaReferenciaPiezaMaestroBrain(referencia)
        End Function

        ''' <summary>
        ''' Cargar los datos de una referencia guardada en el maestro de piezas de Brain por la referencia de Batz o la de cliente
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDatosReferenciaClienteBatzMaestroBrain(ByVal referencia As String, ByVal plantasSeleccionadas As List(Of String)) As ELL.MaestroPiezasBrainResumen
            Return brainDAL.CargarDatosReferenciaClienteBatzMaestroBrain(referencia, plantasSeleccionadas)
        End Function

        ''' <summary>
        ''' Cargar los datos de un drawing number guardada en el maestro de piezas de Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarDrawingMaestroBrain(ByVal referencia As String) As ELL.MaestroPiezasBrainResumen
            Return brainDAL.CargarDrawingMaestroBrain(referencia)
        End Function

        ''' <summary>
        ''' Cargar los datos de una referencia guardada en el maestro de piezas de Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarReferenciaPiezaMaestroBrain(ByVal referencia As String) As ELL.DatosBrain
            Return brainDAL.CargarReferenciaPiezaMaestroBrain(referencia)
        End Function

        ''' <summary>
        ''' Verificar si existe una referencia en el maestro de piezas de Brain
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteReferenciaBrain(ByVal referencia As String, ByVal plantasSeleccionadas As List(Of String), Optional ByVal verificar As Boolean = False) As Boolean
            Return brainDAL.ExisteReferenciaBrain(referencia, plantasSeleccionadas, verificar)
        End Function

        ''' <summary>
        ''' Verificar si existe una referencia de cliente en el maestro de piezas de Brain
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteReferenciaClienteBrain(ByVal referencia As String, ByVal plantasSeleccionadas As List(Of String), Optional ByVal verificar As Boolean = False) As Boolean
            Return brainDAL.ExisteReferenciaClienteBrain(referencia, plantasSeleccionadas, verificar)
        End Function

        ''' <summary>
        ''' Verificar si existe un drawing number en el maestro de piezas de Brain
        ''' </summary>
        ''' <param name="referencia"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function ExisteDrawingBrain(ByVal referencia As String, ByVal plantasSeleccionadas As List(Of String)) As Boolean
            Return brainDAL.ExisteDrawingBrain(referencia, plantasSeleccionadas)
        End Function

#End Region

    End Class

End Namespace
