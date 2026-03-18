'Imports Oracle.ManagedDataAccess.Client

Namespace BLL

    Public Class PlantasBLL

        Private plantasDAL As New DAL.PlantasDAL

#Region "Consultas"

        ''' <summary>
        ''' Obtiene el listad        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarLista() As List(Of ELL.Plantas)
            Dim p As List(Of ELL.Plantas) = plantasDAL.loadList()

            ' Me he encontrado solicitudes con la planta de Zamudio pero como no tiene Id brain no lo cargnt
            Dim planta As ELL.Plantas = plantasDAL.CargarPlanta(47)
            planta.Codigo = "47"
            p.Add(planta)
            Return p
        End Function

        ''' <summary>
        ''' Obtiene una planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPlanta(ByVal idPlanta As String) As ELL.Plantas
            Return plantasDAL.CargarPlanta(idPlanta)
        End Function

        ''' <summary>
        ''' Obtiene una planta
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function CargarPlanta(ByVal idPlanta As Integer) As ELL.Plantas
            Return plantasDAL.CargarPlanta(idPlanta)
        End Function

        ''' <summary>
        ''' Comprobar si una cadena existe en la tabla
        ''' </summary>
        ''' <param name="planta">Objeto Plantas</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function Existe(ByVal planta As ELL.Plantas) As Boolean
            If (plantasDAL.existe(planta) > 0) Then
                Return True
            Else : Return False
            End If
        End Function

#End Region

#Region "Modificaciones"

        ' ''' <summary>
        ' ''' Guarda un nuevo registro
        ' ''' </summary>
        ' ''' <param name="planta">Objeto Plantas</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function GuardarPlanta(ByVal planta As ELL.Plantas) As Boolean
        '    Return plantasDAL.Save(planta)
        'End Function

        ' ''' <summary>
        ' ''' Modifica los datos de un registro
        ' ''' </summary>
        ' ''' <param name="planta">Objeto Plantas</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function ModificarPlanta(ByVal planta As ELL.Plantas) As Boolean
        '    Return plantasDAL.Update(planta)
        'End Function

        ' ''' <summary>
        ' ''' Elimina un registro
        ' ''' </summary>
        ' ''' <param name="idPlanta">Identificador de la planta</param>
        ' ''' <returns></returns>
        ' ''' <remarks></remarks>
        'Public Function EliminarPlanta(ByVal idPlanta As Integer) As Boolean
        '    Return plantasDAL.Delete(idPlanta)
        'End Function

#End Region

    End Class

End Namespace
