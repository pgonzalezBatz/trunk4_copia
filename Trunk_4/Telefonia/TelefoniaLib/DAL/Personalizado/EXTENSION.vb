Imports System.Collections.Specialized
Imports AccesoAutomaticoBD
Imports log4net

NameSpace DAL

Public Class EXTENSION 
	Inherits _EXTENSION
        Private Log As ILog = LogManager.GetLogger("root.Telefonia")
	Public Sub New()
		'Decide connection string depending on state
		Try
			If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIALIVE").ConnectionString
			Else
                    Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("TELEFONIATEST").ConnectionString
			End If
		Catch ex As Exception
                Log.Error("Error al inicializar el connection string Telefonia.", ex)
		End Try
        End Sub

        ''' <summary>
        ''' Obtiene las extensiones internas libres, es decir, los que no estan asociados con ningun numero de telefono
        ''' </summary>        
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function geExtensionesInternasLibres(ByVal oExt As ELL.Extension) As IDataReader
            Dim sql As New System.Text.StringBuilder
            Dim where As String = String.Empty
            sql.AppendLine("SELECT E.* ")
            sql.AppendLine("FROM EXTENSION E ")
            sql.AppendLine("WHERE E.ID_PLANTA=" & oExt.IdPlanta & " and E.ID_TIPOEXT=" & oExt.IdTipoExtension & " and E.OBSOLETO=0 and ")
            sql.AppendLine("not exists(")
            sql.AppendLine("Select EE.ID FROM EXTENSION EE WHERE EE.ID_EXT_INTERNA=E.ID) ")
            If (oExt.IdExtensionInterna <> Integer.MinValue) Then
                sql.AppendLine("UNION ")
                sql.AppendLine("SELECT E.* ")
                sql.AppendLine("FROM EXTENSION E ")
                sql.AppendLine("WHERE E.ID_PLANTA=" & oExt.IdPlanta & " and E.ID_TIPOEXT=" & oExt.IdTipoExtension & " and E.ID= " & oExt.IdExtensionInterna)
            End If

            Return MyBase.LoadFromSqlReader(sql.ToString, Nothing, CommandType.Text)
        End Function

        ''' <summary>
        ''' Devuelve la informacion de las personas y departamentos de una planta
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <param name="bPersonas">Indica si se quieren obtener por persona o por departamento</param>
        ''' <returns>Lista de objetos busqueda</returns>        
        Public Function VerTodos(ByVal idPlanta As Integer, ByVal bPersonas As Boolean, ByVal vigentes As Boolean) As IDataReader
            Try
                Dim sql As New System.Text.StringBuilder
                Dim parameters As New ListDictionary

                Dim p As OracleParameter = New OracleParameter(":ID_PLANTA", OracleDbType.Int32, ParameterDirection.Input)
                parameters.Add(p, idPlanta)

                sql.AppendLine("SELECT *")
                sql.AppendLine(" FROM(")
                'sql.Append(" SELECT DISTINCT (trim(u.nombre) || ' ' || trim(nvl(u.apellido1,'')) || ' ' || trim(nvl(u.apellido2,''))) as nombre,ext.extension as Ext_Interna,extInt.extension as Ext_Movil,(trim(u.nombre) || ' ' || trim(nvl(u.apellido1,'')) || ' ' || trim(nvl(u.apellido2,''))) as item,tlfno.numero as fijo,tlfnoInt.numero as movil")
                sql.Append(" SELECT DISTINCT (trim(u.nombre) || ' ' || trim(nvl(u.apellido1,'')) || ' ' || trim(nvl(u.apellido2,''))) as nombre,")
                sql.Append("CASE ext.id_tipoext when 1 then ext.extension else extInt.extension end as Ext_interna,CASE ext.id_tipoext when 2 then ext.extension else extInt.extension end as Ext_Movil,")
                sql.Append("(trim(u.nombre) || ' ' || trim(nvl(u.apellido1,'')) || ' ' || trim(nvl(u.apellido2,''))) as item,CASE ext.id_tipoext when 1 then tlfno.numero else tlfnoInt.numero end as fijo,CASE ext.id_tipoext when 2 then tlfno.numero else tlfnoInt.numero end as movil")
                If (idPlanta = 1) Then
                    sql.Append(" ,wdep.id_nivel as IdDepartamento,wdep.d_nivel as Departamento")
                Else
                    sql.Append(" ,sdep.id as IdDepartamento,sdep.nombre as Departamento")
                End If
                sql.Append(" ,u.id as idSab,ext.id_tipolinea")
                sql.AppendLine(" FROM sab.usuarios u inner join sab.usuarios_plantas up on u.id=up.id_usuario")
                sql.AppendLine(" Inner join extension_personas extp on u.id=extp.id_usuario")
                sql.AppendLine(" Left join extension ext on extp.id_extension=ext.id and ext.visible<>0 and ext.obsoleto=0")
                sql.AppendLine(" Left join extension extInt on extp.id_extension=extInt.id_ext_interna and extInt.visible<>0 and extInt.obsoleto=0")
                sql.AppendLine(" Left join telefono tlfno on ext.id_telefono=tlfno.id and tlfno.obsoleto=0")
                sql.AppendLine(" Left join telefono tlfnoInt on extInt.id_telefono=tlfnoInt.id and tlfnoInt.obsoleto=0")
                If (idPlanta = 1) Then
                    sql.AppendLine(" Left join w_departamentos wdep on wdep.id_nivel=u.idDepartamento")
                Else
                    sql.AppendLine(" Left join sab.departamentos sdep on sdep.id=u.idDepartamento and sdep.id_planta=:ID_PLANTA")
                End If
                sql.AppendLine(" WHERE(extp.f_hasta Is null and ext.id_planta = :ID_PLANTA")
                If (vigentes) Then
                    sql.AppendLine(" and (u.fechaBaja is null or (u.fechaBaja is not null and u.fechaBaja>sysdate)))")
                Else
                    sql.AppendLine(")")
                End If
                sql.AppendLine(" UNION")
                'sql.Append(" SELECT DISTINCT o.nombre,ext.extension as Ext_Interna,extInt.extension as Ext_Movil,o.nombre as item,tlfno.numero as fijo,tlfnoInt.numero as movil,'','O',0 as idSab")
                sql.Append(" SELECT DISTINCT o.nombre,CASE ext.id_tipoext when 1 then ext.extension else extInt.extension end as Ext_interna,CASE ext.id_tipoext when 2 then ext.extension else extInt.extension end as Ext_Movil,")
                sql.Append("o.nombre as item,CASE ext.id_tipoext when 1 then tlfno.numero else tlfnoInt.numero end as fijo,CASE ext.id_tipoext when 2 then tlfno.numero else tlfnoInt.numero end as movil,'','O',0 as idSab,ext.id_tipolinea")
                sql.AppendLine(" FROM otros o inner join extension_otros exto on o.id=exto.id_otro")
                sql.AppendLine(" Left join extension ext on exto.id_extension=ext.id and ext.visible<>0 and ext.obsoleto=0")
                sql.AppendLine(" left join extension extInt on exto.id_extension=extInt.id_ext_interna and extInt.visible<>0 and extInt.obsoleto=0")
                sql.AppendLine(" Left join telefono tlfno on ext.id_telefono=tlfno.id and tlfno.obsoleto=0")
                sql.AppendLine(" Left join telefono tlfnoInt on extInt.id_telefono=tlfnoInt.id and tlfnoInt.obsoleto=0")
                sql.AppendLine(" WHERE(exto.f_hasta Is null And o.id_planta = :ID_PLANTA)")
                sql.AppendLine(" UNION")
                sql.Append(" SELECT DISTINCT ext.nombre,ext.extension as Ext_Interna,extInt.extension as Ext_Movil,'Depart' as item,tlfno.numero as fijo,tlfnoInt.numero as movil")
                If (idPlanta = 1) Then
                    sql.Append(" ,wdep.id_nivel as IdDepartamento,wdep.d_nivel as Departamento")
                Else
                    sql.Append(" ,sdep.id as IdDepartamento,sdep.nombre as Departamento")
                End If
                sql.Append(" ,0 as idSab,ext.id_tipolinea")
                sql.AppendLine(" FROM  extension_departamentos extd left join extension ext on extd.id_extension=ext.id and ext.visible<>0 and ext.obsoleto=0")
                sql.AppendLine(" Left join extension extInt on extd.id_extension=extInt.id_ext_interna and extInt.visible<>0 and extInt.obsoleto=0")
                sql.AppendLine(" Left join telefono tlfno on ext.id_telefono=tlfno.id and tlfno.obsoleto=0")
                sql.AppendLine(" Left join telefono tlfnoInt on extInt.id_telefono=tlfnoInt.id and tlfnoInt.obsoleto=0")
                If (idPlanta = 1) Then
                    sql.AppendLine(" Left join w_departamentos wdep on wdep.id_nivel=extd.Id_Departamento")
                Else
                    sql.AppendLine(" Left join sab.departamentos sdep on sdep.id=extd.Id_Departamento and sdep.id_planta=:ID_PLANTA")
                End If
                sql.Append(" WHERE(extd.f_hasta Is null and ext.nombre is not null and ext.id_tipoasig=1 and ext.id_planta=:ID_PLANTA)")
                sql.AppendLine(" UNION")
                sql.AppendLine(" SELECT DISTINCT (trim(u.nombre) || ' ' || trim(nvl(u.apellido1,'')) || ' ' || trim(nvl(u.apellido2,''))) as nombre,NULL as Ext_Interna,NULL as Ext_Movil,(trim(u.nombre) || ' ' || trim(nvl(u.apellido1,'')) || ' ' || trim(nvl(u.apellido2,''))) as item,(case tlfno.fijo_Movil when 0 then tlfno.numero else '' end) as fijo,(case tlfno.fijo_Movil when 1 then tlfno.numero else '' end) as movil")
                If (idPlanta = 1) Then
                    sql.Append(" ,wdep.id_nivel as IdDepartamento,wdep.d_nivel as Departamento")
                Else
                    sql.Append(" ,sdep.id as IdDepartamento,sdep.nombre as Departamento")
                End If
                sql.Append(" ,u.id as idSab,0 as id_TipoLinea")
                sql.AppendLine(" FROM sab.usuarios u inner join sab.usuarios_plantas up on u.id=up.id_usuario")
                sql.AppendLine(" Inner join telefono_personas tlfnoP on u.id=tlfnoP.id_usuario")
                sql.AppendLine(" inner join telefono tlfno on tlfnoP.id_tlfno=tlfno.id and tlfno.obsoleto=0")
                If (idPlanta = 1) Then
                    sql.AppendLine(" Left join w_departamentos wdep on wdep.id_nivel=u.idDepartamento")
                Else
                    sql.AppendLine(" Left join sab.departamentos sdep on sdep.id=u.idDepartamento and sdep.id_planta=:ID_PLANTA")
                End If
                sql.AppendLine(" Where(tlfnoP.f_hasta Is null And tlfno.id_planta = :ID_PLANTA")
                If (vigentes) Then
                    sql.AppendLine(" and (u.fechaBaja is null or (u.fechaBaja is not null and u.fechaBaja>sysdate)))")
                Else
                    sql.AppendLine(")")
                End If
                sql.AppendLine(")")
                If (Not bPersonas) Then
                    sql.AppendLine(" ORDER BY Departamento,nombre")
                End If

                Return MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)

            Catch ex As Exception
                Throw New BatzException("errMostrarExtensiones", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene todos las extensiones internas, su tipo y si tiene alveolo o no
        ''' </summary>
        ''' <param name="idCultura">Cultura</param>
        ''' <returns>DataTable</returns>  
        Public Function getExtensionesInternasTipo(ByVal idPlanta As Integer, ByVal idCultura As String) As DataTable
            Dim sql As New System.Text.StringBuilder
            Dim dt As New DataTable
            Dim where As String = String.Empty
            Dim reader As IDataReader
            Dim dataAdapter As New BLL.DataReaderAdapterBatz
            Dim parameters As New ListDictionary
            Dim p As OracleParameter = New OracleParameter(":ID_CULTURA", OracleDbType.Varchar2, ParameterDirection.Input)
            parameters.Add(p, idCultura)
            p = New OracleParameter(":ID_PLANTA", OracleDbType.Int32, ParameterDirection.Input)
            parameters.Add(p, idPlanta)
            sql.AppendLine("SELECT E.EXTENSION,E.NOMBRE,TLC.NOMBRE AS TIPOLINEA,A.RUTA ")
            sql.AppendLine("FROM EXTENSION E INNER JOIN TIPOLINEA_CULTURA TLC ON E.ID_TIPOLINEA=TLC.ID_TIPOLINEA ")
            sql.AppendLine("LEFT JOIN ALVEOLO A ON E.ID_ALVEOLO=A.ID ")
            sql.AppendLine("WHERE E.ID_EXT_INTERNA IS NULL AND TLC.ID_CULTURA=:ID_CULTURA AND E.ID_PLANTA=:ID_PLANTA AND E.OBSOLETO=0")
            sql.AppendLine("ORDER BY E.EXTENSION ")
            reader = MyBase.LoadFromSqlReader(sql.ToString, parameters, CommandType.Text)
            Return dataAdapter.FillFromReader(dt, reader)
        End Function

    End Class

End NameSpace
