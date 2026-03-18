
Namespace DAL
    Public Class GRUPOSCULTURAS
        Inherits _GRUPOSCULTURAS        

        Public Sub New()
            'Decide connection string depending on state
            If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABLIVE").ConnectionString
            Else
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABTEST").ConnectionString
            End If
        End Sub


        ''' <summary>
        ''' Obtiene los grupos que cumplan las condiciones
        ''' </summary>
        ''' <param name="oGrupo">Grupo</param>        
        Public Function GetGruposCultura(ByVal oGrupo As ELL.grupo) As IDataReader            
            Dim sql As New System.Text.StringBuilder
            Dim Where As String = String.Empty

            sql.Append("SELECT * ")
            sql.Append("FROM GRUPOSCULTURASACTIVOS ")            
            If (oGrupo.IdGrupo <> Integer.MinValue) Then
                Where = DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.IDGRUPOS & "=" & oGrupo.IdGrupo
            End If
            If (oGrupo.IdCultura <> String.Empty) Then
                If (Where <> String.Empty) Then Where &= " and "
                Where &= "lower(" & DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.IDCULTURAS & ")='" & oGrupo.IdCultura.ToLower & "'"
            End If
            If (oGrupo.Nombre <> String.Empty) Then
                If (Where <> String.Empty) Then Where &= " and "
                Where &= "lower(" & DAL.Views.GRUPOSCULTURASACTIVOS.ColumnNames.NOMBRE & ") like '%" & oGrupo.Nombre.ToLower & "%'"
            End If

            If (Where <> String.Empty) Then
                sql.Append("WHERE ")
                sql.Append(Where)
            End If

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
        End Function


        ''' <summary>
        ''' Obtiene la informacion de todos los grupos que pertenezcan a una planta
        ''' </summary>
        ''' <param name="lPlantas">Lista de identificadores de plantas</param>
        ''' <param name="IdCultura">Cultura en la que ha que mostrar los grupos</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function GetGruposConPlanta(ByVal lPlantas As List(Of Integer), ByVal idCultura As String) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim sIn As String = String.Empty

            sql.AppendLine("SELECT distinct GC.IDGRUPOS,GC.IDCULTURAS,GC.NOMBRE ")
            sql.AppendLine("FROM GRUPOSPLANTAS GP INNER JOIN GRUPOSCULTURAS GC ON GP.ID_GRUPO=GC.IDGRUPOS ")
            sql.AppendLine("INNER JOIN USUARIOS_PLANTAS UP ON GP.ID_PLANTA=UP.ID_PLANTA ")
            sql.Append("WHERE UP.ID_PLANTA IN (")

            For Each oInt As Integer In lPlantas
                If (sIn <> String.Empty) Then sIn &= ","
                sIn &= "'" & oInt.ToString & "'"
            Next
            sql.Append(sIn)
            sql.Append(") ")
            sql.AppendLine("ORDER BY GC.NOMBRE")

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)

		End Function

    End Class
End Namespace
