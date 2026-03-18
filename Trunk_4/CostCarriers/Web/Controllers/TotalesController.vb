Imports System.Web.Mvc

Namespace Controllers
    Public Class TotalesController
        Inherits BaseController

#Region "Propiedades"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Protected Overrides ReadOnly Property RolesAcceso As List(Of ELL.Rol.TipoRol)
            Get
                Dim roles As New List(Of ELL.Rol.TipoRol)
                roles.Add(ELL.Rol.TipoRol.Responsable_ingenieria_planta)
                roles.Add(ELL.Rol.TipoRol.Gerente_planta)
                roles.Add(ELL.Rol.TipoRol.Project_manager)
                roles.Add(ELL.Rol.TipoRol.Responsable_advance)
                roles.Add(ELL.Rol.TipoRol.Direccion_CMP)
                roles.Add(ELL.Rol.TipoRol.Direccion_operaciones)
                roles.Add(ELL.Rol.TipoRol.Product_manager)
                roles.Add(ELL.Rol.TipoRol.Direccion_industrial)
                Return roles
            End Get
        End Property

#End Region

#Region "Métodos"

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        ''' <param name="idPlantaSAB"></param>
        ''' <returns></returns>
        Function DetalleProyecto(ByVal idCabecera As Integer, Optional ByVal idPlantaSAB As Integer = Integer.MinValue) As ActionResult
            Dim cabecera As ELL.CabeceraCostCarrier = CargarCabecera(idCabecera)
            CargarPlantas(cabecera, idPlantaSAB)

            ViewData("ContainerFluid") = 1
            Return View()
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="idCabecera"></param>
        Private Function CargarCabecera(ByVal idCabecera As Integer) As ELL.CabeceraCostCarrier
            Dim cabecera As ELL.CabeceraCostCarrier = BLL.CabecerasCostCarrierBLL.Obtener(idCabecera, False)
            ViewData("CabeceraProyecto") = cabecera

            Return cabecera
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="cabecera"></param>
        ''' <param name="idPlantaSAB"></param>
        Private Sub CargarPlantas(ByVal cabecera As ELL.CabeceraCostCarrier, Optional ByVal idPlantaSAB As Integer = Integer.MinValue)
            ' Primero necesito saber si soy director de ingeniería o gerente
            Dim idsPlantasUnicos As New List(Of Integer)

            If (idPlantaSAB = Integer.MinValue) Then
                Dim usuariosRol As List(Of ELL.UsuarioRol) = BLL.UsuariosRolBLL.CargarListado(Ticket.IdUser).Where(Function(f) f.IdRol = ELL.Rol.TipoRol.Responsable_ingenieria_planta OrElse f.IdRol = ELL.Rol.TipoRol.Gerente_planta OrElse f.IdRol = ELL.Rol.TipoRol.Project_manager).ToList()

                If (usuariosRol IsNot Nothing AndAlso usuariosRol.Count > 0) Then
                    For Each usuarioRol In usuariosRol
                        idsPlantasUnicos.Add(usuarioRol.IdPlanta)
                    Next

                    ' Cargamos las plantas del proyecto
                    Dim idsPlantasProyecto As List(Of Integer) = cabecera.Plantas.Select(Of Integer)(Function(f) f.IdPlanta).ToList()
                    idsPlantasUnicos = idsPlantasUnicos.Intersect(idsPlantasProyecto).ToList

                    ' Hablando con Maite el 22/02/2022 si el proyecto tiene planta de corporativo se muestra siempre
                    If (idsPlantasProyecto.Contains(0) AndAlso Not idsPlantasUnicos.Contains(0)) Then
                        idsPlantasUnicos.Add(0)
                    End If
                Else
                    ' Cogemos todas las plantas del proyecto
                    idsPlantasUnicos = cabecera.Plantas.Select(Of Integer)(Function(f) f.IdPlanta).ToList()
                End If
            Else
                idsPlantasUnicos.Add(idPlantaSAB)
            End If

            ' Cargamos las distintas plantas
            ViewData("IdsPlantasUnicos") = idsPlantasUnicos.Distinct().ToList()
        End Sub

#End Region

    End Class
End Namespace