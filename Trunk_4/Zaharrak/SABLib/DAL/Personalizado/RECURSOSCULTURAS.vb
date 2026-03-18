Imports log4net

Namespace DAL
    Public Class RECURSOSCULTURAS
        Inherits _RECURSOSCULTURAS

        Private log As ILog = LogManager.GetLogger("root.LISTAMAT")

        Public Sub New()
            'Decide connection string depending on state
            If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABLIVE").ConnectionString
            Else
                Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABTEST").ConnectionString
            End If
        End Sub


        ''' <summary>
        ''' Obtiene los recursos que cumplan las condiciones
        ''' </summary>
        ''' <param name="oRecurso">Recurso</param>        
        Public Function GetRecursosCultura(ByVal oRecurso As ELL.recurso) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim Where As String = String.Empty

			sql.Append("SELECT R.ID,RCA.IDCULTURAS,RCA.NOMBRE,R.IDPARENT,R.URL,R.IMAGEN,R.TIPO,RCA.DESCRIPCION,R.VISIBLE,R.NOMBRE_FICHERO,R.TARGET ")
            sql.Append("FROM RECURSOSCULTURASACTIVOS RCA INNER JOIN RECURSOS R ON RCA.IDRECURSOS=R.ID ")
            If (oRecurso.Id <> Integer.MinValue) Then
                Where = "RCA." & DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.IDRECURSOS & "=" & oRecurso.Id
            End If
            If (oRecurso.IdCultura <> String.Empty) Then
                If (Where <> String.Empty) Then Where &= " and "
                Where &= "lower(RCA." & DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.IDCULTURAS & ")='" & oRecurso.IdCultura.ToLower & "'"
            End If
            If (oRecurso.Nombre <> String.Empty) Then
                If (Where <> String.Empty) Then Where &= " and "
                Where &= "lower(RCA." & DAL.Views.RECURSOSCULTURASACTIVOS.ColumnNames.NOMBRE & ") like '%" & oRecurso.Nombre.ToLower & "%'"
            End If

            If (Where <> String.Empty) Then
                sql.Append("WHERE ")
                sql.Append(Where)
            End If

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
		End Function

		''' <summary>
		''' Obtiene todos los recursos traducidos a la cultura a los que tiene acceso el usuario
		''' </summary>
		''' <param name="idUser"></param>
		''' <param name="idCultura"></param>
		''' <returns></returns>
		''' <remarks></remarks>
		Public Function GetRecursosCulturaAll(ByVal idUser As Integer, ByVal idCultura As String) As IDataReader
			Dim sql As New System.Text.StringBuilder

			sql.Append("SELECT distinct recursos.ID, recursos.url, recursosculturas.nombre, recursosculturas.descripcion,recursos.visible,recursos.idparent,recursos.tipo,recursos.nombre_fichero,recursos.target ")
			sql.Append("FROM recursos JOIN gruposrecursos ON (recursos.ID = gruposrecursos.idrecursos) ")
			sql.Append("JOIN grupos ON (grupos.ID = gruposrecursos.idgrupos) ")
			sql.Append("JOIN usuariosgrupos ON (grupos.ID = usuariosgrupos.idgrupos) ")
			sql.Append("JOIN recursosculturas ON (recursos.ID = recursosculturas.idrecursos) ")
			sql.Append("Where(USUARIOSGRUPOS.IDUSUARIOS =" & idUser & " ")
			sql.Append("AND (recursosculturas.idculturas = '" & idCultura & "') ")
			sql.Append("AND (recursos.obsoleto=0 and grupos.obsoleto=0))")
			sql.Append("ORDER BY recursosculturas.nombre")

			Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
			
		End Function

    End Class
End Namespace