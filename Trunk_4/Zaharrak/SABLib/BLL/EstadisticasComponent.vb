Imports System.Collections.Generic
Imports SABLib_Z.BLL.Interface
Imports AccesoAutomaticoBD

Namespace BLL
    Public Class EstadisticasComponent
        Implements IEstadisticasComponent

        Private Log As log4net.ILog = log4net.LogManager.GetLogger("root.SAB")

#Region "Guardar accesos"

        ''' <summary>
        ''' Guarda un acceso a una aplicacion
        ''' </summary>
        ''' <param name="nombreUsuario">Nombre del usuario del acceso</param>
        ''' <param name="numeroTrabajador">Numero del trabajador</param>
        ''' <param name="aplicacion">Aplicacion a la que ha entrado</param>
        ''' <param name="ip">Direccion IP</param>
        Function GuardarAcceso(ByVal nombreUsuario As String, ByVal numeroTrabajador As Integer, ByVal aplicacion As String, ByVal ip As String) As Boolean Implements IEstadisticasComponent.GuardarAcceso
            Dim estanet As New DAL.SiteStadistic.usuarios
            Try
                estanet.AddNew()
                If nombreUsuario.Length > 0 Then
                    estanet.Usuario = nombreUsuario
                End If
                If numeroTrabajador > 0 Then
                    estanet.N_trabajador = numeroTrabajador
                End If
                estanet.Aplicacion = aplicacion
                estanet.Ip = ip
                estanet.Fecha_hora = DateTime.Now
                estanet.Save()
            Catch ex As Exception
                Log.Error("GuardarAcceso. " + ex.Message)
                Return False
            End Try
            Return True
        End Function

#End Region

#Region "Accesos a recursos"
        Public Function AccesosARecurso(ByVal nombreRecurso As String, ByVal fechaInicio As DateTime, _
                                                ByVal fechaFin As DateTime) As Integer Implements IEstadisticasComponent.AccesosARecurso
            Dim usuarios As New DAL.SiteStadistic.usuarios
            usuarios.Where.Aplicacion.Value = nombreRecurso
            usuarios.Where.Aplicacion.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
            If (fechaInicio <> DateTime.MinValue) Then
                usuarios.Where.Fecha_hora.Value = fechaInicio
                usuarios.Where.Fecha_hora.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThanOrEqual
                Dim wp As AccesoAutomaticoBD.WhereParameter = usuarios.Where.TearOff.Fecha_hora
                wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.AND_
                wp.Value = fechaFin
                wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.LessThanOrEqual
            End If
            usuarios.Query.Load()
            Return usuarios.RowCount()
        End Function

        Public Function AccesosARecursoPorUsuario(ByVal nombreRecurso As String, ByVal fechaInicio As DateTime, _
                                                  ByVal fechaFin As DateTime) As DataTable _
                                                    Implements IEstadisticasComponent.AccesosARecursoPorUsuario
            Dim usuarios As New DAL.SiteStadistic.usuarios
            usuarios.Where.Aplicacion.Value = nombreRecurso
            usuarios.Where.Aplicacion.Operator = AccesoAutomaticoBD.WhereParameter.Operand.Equal
            If (fechaInicio <> DateTime.MinValue) Then
                usuarios.Where.Fecha_hora.Value = fechaInicio
                usuarios.Where.Fecha_hora.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThanOrEqual
                Dim wp As AccesoAutomaticoBD.WhereParameter = usuarios.Where.TearOff.Fecha_hora
                wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.AND_
                wp.Value = fechaFin
                wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.LessThanOrEqual
            End If
            usuarios.Query.AddGroupBy(DAL.SiteStadistic.usuarios.ColumnNames.Usuario)
            usuarios.Query.AddResultColumn(DAL.SiteStadistic.usuarios.ColumnNames.Usuario)                        
            usuarios.Query.Load()
            Return usuarios.DefaultView.Table
        End Function


        Public Function AccesosARecursoPorUsuarioSQL(ByVal nombreRecurso As String, ByVal fechaInicio As DateTime, _
                                                  ByVal fechaFin As DateTime) As List(Of String()) _
                                                    Implements IEstadisticasComponent.AccesosARecursoPorUsuarioSQL
            Dim usuarios As New DAL.SiteStadistic.usuarios
            Return usuarios.AccesosARecursoPorUsuarioSQL(nombreRecurso, fechaInicio, fechaFin)
        End Function

        Public Function EstadisticasMesuales(ByVal year As Integer, ByVal nombreRecurso As String) _
                                            As List(Of Integer) Implements IEstadisticasComponent.EstadisticasMesuales
            Dim l As New List(Of Integer)
            Dim i As Integer = 1
            While i < 13
                l.Add(AccesosARecurso(nombreRecurso, DateTime.Parse("01/" + i.ToString + "/" + year.ToString), DateTime.Parse(DateTime.DaysInMonth(year, i).ToString + "/" + i.ToString + "/" + year.ToString)))
                i += 1
            End While

            Return l
        End Function
#End Region

    End Class
End Namespace
