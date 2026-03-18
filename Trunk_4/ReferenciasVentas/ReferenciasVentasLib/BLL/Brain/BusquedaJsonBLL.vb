Imports System.Collections
Imports SolicitudesSistemasLib.DAL

Namespace BLL
    Public Class BusquedaJsonBLL

        Private busquedaJsonDAL As New DAL.BusquedaJsonDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene una referencia de pieza de Batz
        ''' </summary>
        ''' <param name="refPieza">Referencia de la pieza</param>
        ''' <returns>Referencia de pieza de Batz</returns>                
        Public Shared Function CargarPrevBatzPN(ByVal refPieza As String) As ELL.MaestroPiezasBrainResumen
            Return BusquedaJsonDAL.CargarPrevBatzPN(refPieza)
        End Function

        '''' <summary>
        '''' Obtiene una referencia de pieza de Batz
        '''' </summary>
        '''' <param name="refPieza">Referencia de la pieza</param>
        '''' <returns>Referencia de pieza de Batz</returns>                
        'Public Shared Function CargarPrevBatzPN(ByVal refPieza As String, ByVal plantas As String) As ELL.MaestroPiezasBrainResumen
        '    Return busquedaJsonDAL.CargarPrevBatzPN(refPieza, plantas)
        'End Function

        ''' <summary>
        ''' Obtiene una referencia de pieza de Batz
        ''' </summary>
        ''' <param name="refPieza">Referencia de la pieza</param>
        ''' <returns>Referencia de pieza de Batz</returns>                
        Public Shared Function CargarReferenciaPieza(ByVal refPieza As String) As ELL.BusquedaJson
            Return busquedaJsonDAL.CargarReferenciaPieza(refPieza)
        End Function

        ''' <summary>
        ''' Obtiene un proyecto de Brain
        ''' </summary>
        ''' <param name="idProyecto">Identificador del proyecto de Brain</param>
        ''' <returns>Datos de un proyecto</returns>                
        Public Shared Function CargarProyectoBrain(ByVal idProyecto As String) As ELL.Proyectos
            Return busquedaJsonDAL.CargarProyectoBrain(idProyecto)
        End Function

        ''' <summary>
        ''' Verifica si existe una referencia de cliente guardada en Brain para las plantas seleccionadas
        ''' </summary>
        ''' <param name="refPieza">Referencia de cliente</param>
        ''' <param name="plantas">Plantas Seleccionadas</param>
        ''' <returns></returns>
        ''' <remarks></remarks>               
        Public Shared Function CargarCustomerPN(ByVal refPieza As String, ByVal plantas As String) As ELL.BusquedaJson
            Return busquedaJsonDAL.CargarCustomerPN(refPieza, plantas)
        End Function

        '''' <summary>
        '''' Verifica si existe una referencia de cliente guardada en Brain para las plantas seleccionadas
        '''' </summary>
        '''' <param name="refPieza">Referencia Drawing</param>
        '''' <param name="plantas">Plantas Seleccionadas</param>
        '''' <returns></returns>
        '''' <remarks></remarks>               
        'Public Shared Function CargarDrawing(ByVal refPieza As String, ByVal plantas As String) As ELL.MaestroPiezasBrainResumen
        '    Return busquedaJsonDAL.CargarPrevBatzPN(refPieza, plantas)
        'End Function

        ''' <summary>
        ''' Cargar la lista de tipos relacionados con un producto
        ''' </summary>
        ''' <param name="idProducto">Identificador del producto</param>
        ''' <returns></returns>
        ''' <remarks></remarks>               
        Public Shared Function CargarTiposProducto(ByVal idProducto As String) As List(Of ELL.Type)
            Dim typesDAL As New DAL.TypeDAL
            Return typesDAL.CargarTiposProducto(idProducto)
        End Function

        ''' <summary>
        ''' Cargar los datos de un producto
        ''' </summary>
        ''' <param name="idProducto">Identificador del producto</param>
        ''' <returns></returns>
        ''' <remarks></remarks>               
        Public Shared Function CargarProducto(ByVal idProducto As String) As ELL.Producto
            Dim productoDAL As New DAL.ProductoDAL
            Return productoDAL.CargarProducto(idProducto)
        End Function

        ''' <summary>
        ''' Cargar los datos disponsibles para Transmission Mode
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>               
        Public Shared Function CargarTransmissionMode() As List(Of ELL.TransmissionMode)
            Dim transmissionModeDAL As New DAL.TransmissionModeDAL
            Return transmissionModeDAL.CargarTransmissionModeActivos()
        End Function
#End Region

    End Class

End Namespace
