Imports System
Imports System.Collections.Generic
Imports System.Data.SqlClient
Imports System.Linq
Imports System.Web
Imports GrupoMaterial
Imports Oracle.DataAccess.Client
Imports SabLib.BLL.Utils
Imports System.Web.Script.Serialization

Public Class DataAccessLayer
    Public cnString4 As String = ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString
    Public cnStr As String = Configuration.ConfigurationManager.ConnectionStrings("CONNECTION").ConnectionString
    Public cnStrOracle As String = Configuration.ConfigurationManager.ConnectionStrings("GRUPOLIVE").ConnectionString


    Public Function JsonSerializer(Of T)(o As T) As String
        Dim ser As New JavaScriptSerializer()
        Return ser.Serialize(o)
    End Function

    Friend Function isNameAvailable(name As String, myObject As Object) As Boolean
        Dim tableName = ""
        Dim columnName = ""
        Select Case myObject.GetType().Name
            Case "Comodity"
                tableName = "COMODITY"
                columnName = "CODESC"
            Case "Family"
                tableName = "FAMILIAGM"
                columnName = "CFDESC"
            Case "SubFamily"
                tableName = "SUBFAMIGM"
                columnName = "CSDESC"
            Case "Element"
                tableName = "ELEMENTGM"
                columnName = "CEDESC"
        End Select
        Dim query As String = "SELECT COUNT(*) FROM " & tableName & " WHERE " & columnName & " = ?"
        Dim result As Boolean
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@" & columnName, name)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If oReader.Read() Then
                result = oReader.Item(0).Equals(0)
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function


    Friend Function isNameAvailableEditMode(id As String, name As String, myObject As Object) As Boolean
        Dim tableName = ""
        Dim columnName = ""
        Dim columnName2 = ""
        Select Case myObject.GetType().Name
            Case "Comodity"
                tableName = "COMODITY"
                columnName = "CODESC"
            Case "Family"
                tableName = "FAMILIAGM"
                columnName = "CFDESC"
            Case "SubFamily"
                tableName = "SUBFAMIGM"
                columnName = "CSDESC"
            Case "FullElement"
                tableName = "ELEMENTGM"
                columnName = "CEDESC"
                columnName2 = "CECODE"
        End Select
        Dim query As String = "SELECT COUNT(*) FROM " & tableName & " WHERE " & columnName & " = ? AND " & columnName2 & " <> ?"
        Dim result As Boolean
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@" & columnName, name)
            cm.Parameters.AddWithValue("@" & columnName2, id)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If oReader.Read() Then
                result = oReader.Item(0).Equals(0)
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

#Region "GetAll"
    Public Function GetComodityAll() As List(Of Comodity)
        Dim result As New List(Of Comodity)
        Dim cnStr = Configuration.ConfigurationManager.ConnectionStrings.Item("CONNECTION").ConnectionString
        Dim query As String = "select COFIRM, COWKNR,COCODE,CODESC from comodity"
        Dim lParametros As New List(Of SqlParameter)
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn As New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            While (oReader.Read())
                result.Add(New Comodity With {.Id = stringNull(oReader.Item(2)), .Name = stringNull(oReader.Item(3))})
            End While
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function GetFamilyAll() As List(Of Family)
        Dim result As New List(Of Family)
        Dim query As String = "select CFCODE,CFDESC,CODESC from FAMILIAGM left join comodity ON CFCOMO = COCODE"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn As New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            While (oReader.Read())
                result.Add(New Family With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)), .Parent = stringNull(oReader.Item(2))})
            End While
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function GetSubFamilyAll() As List(Of SubFamily)
        Dim result As New List(Of SubFamily)
        Dim query As String = "select CSCODE,CSDESC,CSFAMI,F.CFDESC,C.CODESC from SUBFAMIGM LEFT JOIN FAMILIAGM F ON CSFAMI = F.CFCODE LEFT JOIN COMODITY C ON C.COCODE = F.CFCOMO ORDER BY CSFAMI,CSCODE"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                While (oReader.Read())
                    result.Add(New SubFamily With {.Id = stringNull(oReader.Item(0)).Trim, .Name = stringNull(oReader.Item(1)).Trim, .Parent = stringNull(oReader.Item(3)).Trim, .Grandparent = stringNull(oReader.Item(4)).Trim})
                End While
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function

    Friend Function GetFamiliesFromComodityName(desc As String) As List(Of Family)
        Dim result As New List(Of Family)
        Dim query As String = "SELECT CFCODE,CFDESC FROM FAMILIAGM LEFT JOIN COMODITY C ON C.COCODE = CFCOMO WHERE CODESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CCDESC", desc)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            While (oReader.Read())
                result.Add(New Family With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))})
            End While
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    Friend Function GetFamiliesFromComodityCode(newcode As String) As List(Of Family)
        Dim result As New List(Of Family)
        Dim query As String = "SELECT CFCODE,CFDESC FROM FAMILIAGM LEFT JOIN COMODITY C ON C.COCODE = CFCOMO WHERE CFCOMO = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFCOMO", newcode)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            While (oReader.Read())
                result.Add(New Family With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))})
            End While
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="familyName"></param>
    ''' <returns></returns>
    Friend Function GetSubfamiliesFromFamilyName(familyName As String) As List(Of SubFamily)
        Dim result As New List(Of SubFamily)
        Dim query As String = "SELECT CSCODE,CSDESC FROM SUBFAMIGM LEFT JOIN FAMILIAGM F ON F.CFCODE = CSFAMI WHERE CFDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFDESC", familyName)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            While (oReader.Read())
                result.Add(New SubFamily With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))})
            End While
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    Friend Function GetSubfamiliesFromFamilyCode(familyCode As String) As List(Of SubFamily)
        Dim result As New List(Of SubFamily)
        Dim query As String = "SELECT CSCODE,CSDESC FROM SUBFAMIGM LEFT JOIN FAMILIAGM F ON F.CFCODE = CSFAMI WHERE CFCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFCODE", familyCode)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            While (oReader.Read())
                result.Add(New SubFamily With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))})
            End While
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    Friend Sub saveUnassignedElement(e As FullElement, oldName As Object)
        Dim result As String = ""
        Dim query As String = "UPDATE ELEMENTGM SET CEDESC = ?, CESUBF = ? WHERE CEDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()

            Dim cm = New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CEDESC", e.Name)
            cm.Parameters.AddWithValue("@CESUBF", If(e.ParentId Is Nothing, e.Parent, e.ParentId))
            cm.Parameters.AddWithValue("@CEDESC", oldName)
            cm.CommandTimeout = 30
            result = cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Sub


    Friend Sub saveUnassignedElementOracle(e As FullElement, oldName As Object)
        Dim result As String = ""
        Dim query As String = "UPDATE ELEMENTGM SET CEDESC = ?, CESUBF = ? WHERE CEDESC = ?"
        'Dim oReader As OleDb.OleDbDataReader = Nothing
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()

            'Dim cm = New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CEDESC", e.Name)
            'cm.Parameters.AddWithValue("@CESUBF", If(e.ParentId Is Nothing, e.Parent, e.ParentId))
            'cm.Parameters.AddWithValue("@CEDESC", oldName)
            'cm.CommandTimeout = 30
            'result = cm.ExecuteNonQuery()
            query = "UPDATE ELEMENTGM SET CEDESC = '" & e.Name & "', CESUBF = '" & If(e.ParentId Is Nothing, e.Parent, e.ParentId) & "' WHERE CEDESC = '" & oldName & "'"
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Oracle", ex)

        End Try
    End Sub

    Friend Function getNextCodeFor(myObject As Object) As String
        Dim tableName = String.Empty
        Dim columnName = String.Empty
        Select Case myObject.GetType().Name
            Case "Comodity"
                tableName = "COMODITY"
                columnName = "COCODE"
            Case "Family"
                tableName = "FAMILIAGM"
                columnName = "CFCODE"
            Case "SubFamily"
                tableName = "SUBFAMIGM"
                columnName = "CSCODE"
            Case "Element"
                tableName = "ELEMENTGM"
                columnName = "CECODE"
        End Select
        Dim query = "select max(" & columnName & ")+1 FROM " & tableName
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Dim result As String
        Try
            cn.Open()
            Dim cm = New OleDb.OleDbCommand(query, cn)
            cm.CommandTimeout = 30
            result = cm.ExecuteScalar()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result.ToString
    End Function


    Friend Sub assignFullData(e As DataFull)
        Dim result As String = String.Empty
        'result = updateFullData(e)
        e.Code = e.GMCode
        Dim query As String = "UPDATE ELEMENTGM SET CESUBF = ?, CECODE = ? WHERE CEDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()

            Dim cm = New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CESUBF", e.SubfamilyId)
            cm.Parameters.AddWithValue("@CECODE", e.Code)
            cm.Parameters.AddWithValue("@CEDESC", e.Name)
            cm.CommandTimeout = 30
            result = cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        'Return result
    End Sub

    'Public Function GetDataAll() As List(Of FullElement)
    '    Dim result As New List(Of FullElement)
    '    Dim query As String = "select CECODE,CEDESC,CSCODE,S.CSDESC,CFCODE,CFDESC,COCODE,CODESC       
    '                           from ELEMENTGM 
    '                           FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
    '                           FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
    '                           FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE"
    '    Dim oReader As OleDb.OleDbDataReader = Nothing
    '    Using cn = New OleDb.OleDbConnection(cnStr)
    '        Try
    '            cn.Open()
    '            Dim cm As New OleDb.OleDbCommand(query, cn)
    '            cm.CommandTimeout = 30
    '            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
    '            While (oReader.Read())
    '                result.Add(New FullElement With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)),
    '                                                      .ParentId = stringNull(oReader.Item(2)), .Parent = stringNull(oReader.Item(3)),
    '                                                      .GrandparentId = stringNull(oReader.Item(4)), .Grandparent = stringNull(oReader.Item(5)),
    '                                                      .GrandgrandparentId = stringNull(oReader.Item(6)), .Grandgrandparent = stringNull(oReader.Item(7))})
    '            End While
    '        Catch ex As Exception
    '            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
    '        End Try
    '    End Using
    '    Return result
    'End Function


    Public Function GetDataFromXpert() As List(Of DataXpert)
        Dim result As New List(Of DataXpert)
        Dim query As String = "SELECT ELTO, DENO_S FROM T_40N"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                While (oReader.Read())
                    result.Add(New DataXpert With {.Code = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))})
                End While
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function

    Public Function GetFullData() As List(Of DataFull)
        Dim result As New List(Of DataFull)
        Dim query As String = "select DISTINCT O.ELTO,O.DENO_S,CECODE,CEDESC,CSCODE,S.CSDESC,CFCODE,CFDESC,COCODE,CODESC       
                               from ELEMENTGM 
                               FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
                               FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
                               FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE
                               RIGHT JOIN T_40N O ON O.ELTO = CECODE
                               ORDER BY CODESC,CFDESC,CSDESC,CEDESC"
        'WHERE O.ELTO Is Not NULL And O.DENO_S Is Not NULL
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                While (oReader.Read())
                    result.Add(New DataFull With {.GMCode = stringNull(oReader.Item(0)), .GMName = stringNull(oReader.Item(1)),
                                                  .Code = stringNull(oReader.Item(2)), .Name = stringNull(oReader.Item(3)),
                                                  .SubfamilyId = stringNull(oReader.Item(4)), .Subfamily = stringNull(oReader.Item(5)),
                                                  .FamilyId = stringNull(oReader.Item(6)), .Family = stringNull(oReader.Item(7)),
                                                  .ComodityId = stringNull(oReader.Item(8)), .Comodity = stringNull(oReader.Item(9))
                    })
                End While
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function




    Public Function Valoresplanta(ByVal id As String) As List(Of DataFull)
        Dim result As New List(Of DataFull)
        Dim query As String = ""
        If id = "" Then
            query = "select DISTINCT O.ELTO,O.DENO_S,CECODE,CEDESC,CSCODE,S.CSDESC,CFCODE,CFDESC,COCODE,CODESC       
                               from ELEMENTGM 
                               FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
                               FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
                               FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE
                               RIGHT JOIN T_40N O ON O.ELTO = CECODE    
                               ORDER BY CODESC, CFDESC, CSDESC, CEDESC"
        Else
            query = "select DISTINCT O.ELTO,O.DENO_S,CECODE,CEDESC,CSCODE,S.CSDESC,CFCODE,CFDESC,COCODE,CODESC       
                               from ELEMENTGM 
                               FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
                               FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
                               FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE
                               RIGHT JOIN T_40N O ON O.ELTO = CECODE    
                               WHERE CECODE='" & id & "'
                               ORDER BY CODESC, CFDESC, CSDESC, CEDESC"
        End If

        'WHERE O.ELTO Is Not NULL And O.DENO_S Is Not NULL           WHERE CECODE=" & CInt(id) & "
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                While (oReader.Read())
                    result.Add(New DataFull With {.GMCode = stringNull(oReader.Item(0)), .GMName = stringNull(oReader.Item(1)),
                                                  .Code = stringNull(oReader.Item(2)), .Name = stringNull(oReader.Item(3)),
                                                  .SubfamilyId = stringNull(oReader.Item(4)), .Subfamily = stringNull(oReader.Item(5)),
                                                  .FamilyId = stringNull(oReader.Item(6)), .Family = stringNull(oReader.Item(7)),
                                                  .ComodityId = stringNull(oReader.Item(8)), .Comodity = stringNull(oReader.Item(9))
                    })
                End While
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function



    Public Function Valorplanta(ByVal id As String) As DataFull
        Dim result As New DataFull
        Dim query As String = "select DISTINCT O.ELTO,O.DENO_S,CECODE,CEDESC,CSCODE,S.CSDESC,CFCODE,CFDESC,COCODE,CODESC       
                               from ELEMENTGM 
                               FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
                               FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
                               FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE
                               RIGHT JOIN T_40N O ON O.ELTO = CECODE
							   WHERE CECODE='" & id & "'
                               ORDER BY CODESC,CFDESC,CSDESC,CEDESC"
        'WHERE O.ELTO Is Not NULL And O.DENO_S Is Not NULL
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                If (oReader.Read()) Then
                    result = New DataFull With {.GMCode = stringNull(oReader.Item(0)), .GMName = stringNull(oReader.Item(1)),
                                                  .Code = stringNull(oReader.Item(2)), .Name = stringNull(oReader.Item(3)),
                                                  .SubfamilyId = stringNull(oReader.Item(4)), .Subfamily = stringNull(oReader.Item(5)),
                                                  .FamilyId = stringNull(oReader.Item(6)), .Family = stringNull(oReader.Item(7)),
                                                  .ComodityId = stringNull(oReader.Item(8)), .Comodity = stringNull(oReader.Item(9))
                    }
                End If
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function


    Public Function GetFullDataWithoutNulls() As List(Of DataFull)
        Dim result As New List(Of DataFull)
        Dim query As String = "select DISTINCT O.ELTO,O.DENO_S,CECODE,CEDESC,CSCODE,S.CSDESC,CFCODE,CFDESC,COCODE,CODESC       
                               from ELEMENTGM 
                               FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
                               FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
                               FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE
                               FULL OUTER JOIN T_40N O ON O.ELTO = CECODE
                               WHERE CEDESC IS NOT NULL
                               ORDER BY CODESC,CFDESC,CSDESC,CEDESC"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                While (oReader.Read())
                    result.Add(New DataFull With {.GMCode = stringNull(oReader.Item(0)), .GMName = stringNull(oReader.Item(1)),
                                                  .Code = stringNull(oReader.Item(2)), .Name = stringNull(oReader.Item(3)),
                                                  .SubfamilyId = stringNull(oReader.Item(4)), .Subfamily = stringNull(oReader.Item(5)),
                                                  .FamilyId = stringNull(oReader.Item(6)), .Family = stringNull(oReader.Item(7)),
                                                  .ComodityId = stringNull(oReader.Item(8)), .Comodity = stringNull(oReader.Item(9))
                    })
                End While
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function GetFullData(ByVal id As String, name As String) As DataFull
        Dim result As New DataFull
        Dim query As String = "select DISTINCT O.ELTO,O.DENO_S,CECODE,CEDESC,CSCODE,S.CSDESC,CFCODE,CFDESC,COCODE,CODESC       
                               from ELEMENTGM 
                               FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
                               FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
                               FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE
                               FULL OUTER JOIN T_40N O ON O.ELTO = CECODE
                               WHERE O.ELTO = ?"
        'WHERE CEDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.Parameters.AddWithValue("@ELTO", id)
                'cm.Parameters.AddWithValue("@CEDESC", name)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                While (oReader.Read())
                    result = New DataFull With {.GMCode = stringNull(oReader.Item(0)), .GMName = stringNull(oReader.Item(1)),
                                                  .Code = stringNull(oReader.Item(2)), .Name = stringNull(oReader.Item(3)),
                                                  .SubfamilyId = stringNull(oReader.Item(4)), .Subfamily = stringNull(oReader.Item(5)),
                                                  .FamilyId = stringNull(oReader.Item(6)), .Family = stringNull(oReader.Item(7)),
                                                  .ComodityId = stringNull(oReader.Item(8)), .Comodity = stringNull(oReader.Item(9))
                    }
                End While
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function


    Public Function getUnassignedElements() As List(Of DataFull)
        Dim result As New List(Of DataFull)
        Dim query As String = "SELECT DISTINCT CECODE, CEDESC, CSCODE, S.CSDESC, CFCODE, CFDESC, COCODE, CODESC       
                               From ELEMENTGM 
                               FULL OUTER JOIN SUBFAMIGM S ON CESUBF = S.CSCODE 
                               FULL OUTER JOIN FAMILIAGM F ON CSFAMI = F.CFCODE
                               FULL OUTER JOIN COMODITY C ON CFCOMO = C.COCODE
                               FULL OUTER JOIN T_40N O ON O.ELTO = CECODE WHERE O.DENO_S IS NULL AND O.ELTO IS NULL AND CEDESC IS NOT NULL
                               ORDER BY CODESC,CFDESC,CSDESC,CEDESC"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                While (oReader.Read())
                    result.Add(New DataFull With {.Code = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)),
                                                  .SubfamilyId = stringNull(oReader.Item(2)), .Subfamily = stringNull(oReader.Item(3)),
                                                  .FamilyId = stringNull(oReader.Item(4)), .Family = stringNull(oReader.Item(5)),
                                                  .ComodityId = stringNull(oReader.Item(6)), .Comodity = stringNull(oReader.Item(7))
                    })
                End While
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function
#End Region

#Region "Get individual data"
    'Friend Function GetGrandgrandparentidFromName(parent As String) As String
    '    Dim result As String = ""
    '    Dim query As String = "select COCODE from COMODITY where CODESC = ?"
    '    Dim oReader As OleDb.OleDbDataReader = Nothing
    '    Dim cn = New OleDb.OleDbConnection(cnStr)
    '    Try
    '        cn.Open()
    '        Dim cm As New OleDb.OleDbCommand(query, cn)
    '        cm.Parameters.AddWithValue("@CODESC", parent)
    '        cm.CommandTimeout = 30
    '        oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
    '        If (oReader.Read()) Then
    '            result = oReader.Item(0)
    '        End If
    '    Catch ex As Exception
    '        Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
    '    Finally
    '        If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
    '        If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
    '    End Try
    '    Return result
    'End Function


    'Friend Function GetParentidFromName(parent As String) As String
    '    Dim result As String = ""
    '    Dim query As String = "select CSCODE from SUBFAMIGM where CSDESC = ?"
    '    Dim oReader As OleDb.OleDbDataReader = Nothing
    '    Dim cn = New OleDb.OleDbConnection(cnStr)
    '    Try
    '        cn.Open()
    '        Dim cm As New OleDb.OleDbCommand(query, cn)
    '        cm.Parameters.AddWithValue("@CSDESC", parent)
    '        cm.CommandTimeout = 30
    '        oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
    '        If (oReader.Read()) Then
    '            result = oReader.Item(0)
    '        End If
    '    Catch ex As Exception
    '        Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
    '    Finally
    '        If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
    '        If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
    '    End Try
    '    Return result
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    Public Function GetComodityIdFromName(name As String) As String
        Dim result As String = ""
        Dim query As String = "select COCODE from COMODITY where CODESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CODESC", name)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = oReader.Item(0)
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="name"></param>
    ''' <returns></returns>
    Private Function GetFamiliaIdFromName(name As String) As String
        Dim result As String = ""
        Dim query As String = "select CFCODE from FAMILIAGM where CFDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFDESC", name)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = oReader.Item(0)
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result

    End Function
#End Region

#Region "Get One"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function GetComodityByCode(id As String) As Comodity
        Dim result As New Comodity
        Dim query As String = "select COCODE,CODESC from COMODITY where COCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@COCODE", id.ToString)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New Comodity With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="newcode"></param>
    ''' <returns></returns>
    Friend Function GetComodityFromFamilyCode(newcode As String) As Comodity
        Dim result As New Comodity
        Dim query As String = "SELECT COCODE,CODESC FROM COMODITY LEFT JOIN FAMILIAGM F ON F.CFCOMO = COCODE WHERE CFCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFCODE", newcode)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New Comodity With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="desc"></param>
    ''' <returns></returns>
    Friend Function GetComodityFromFamilyName(desc As String) As Comodity
        Dim result As New Comodity
        Dim query As String = "SELECT COCODE,CODESC FROM COMODITY LEFT JOIN FAMILIAGM F ON F.CFCOMO = COCODE WHERE CFDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFCODE", desc)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New Comodity With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function GetFamilyByCode(id As String) As Family
        Dim result As New Family
        Dim query As String = "select CFCODE,CFDESC,COCODE,CODESC from FAMILIAGM 
                               LEFT JOIN COMODITY ON CFCOMO = COCODE where CFCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFCODE", id.ToString)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New Family With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)),
                                           .ParentId = stringNull(oReader.Item(2)), .Parent = stringNull(oReader.Item(3))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Friend Function GetFamilyFromSubfamilyCode(id As String) As Family
        Dim result As New Family
        Dim query As String = "SELECT CFCODE,CFDESC FROM FAMILIAGM LEFT JOIN SUBFAMIGM S ON S.CSFAMI = CFCODE WHERE S.CSCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CSCODE", id)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New Family With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="val"></param>
    ''' <returns></returns>
    Friend Function GetFamilyFromSubfamilyName(val As String) As Family
        Dim result As New Family
        Dim query As String = "SELECT CFCODE,CFDESC FROM FAMILIAGM LEFT JOIN SUBFAMIGM S ON S.CSFAMI = CFCODE WHERE S.CSDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CSDESC", val)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New Family With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function GetSubFamilyByCode(id As String) As SubFamily
        Dim result As New SubFamily
        Dim query As String = "select CSCODE,CSDESC,CSFAMI,CFDESC,COCODE,CODESC from SUBFAMIGM
                               LEFT JOIN FAMILIAGM ON CSFAMI = CFCODE
                               LEFT JOIN COMODITY ON CFCOMO = COCODE where CSCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CSCODE", id.ToString)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New SubFamily With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)),
                                              .ParentId = stringNull(oReader.Item(2)), .Parent = stringNull(oReader.Item(3)),
                                              .GrandparentId = stringNull(oReader.Item(4)), .Grandparent = stringNull(oReader.Item(5))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="value"></param>
    ''' <returns></returns>
    Public Function GetSubFamilyByName(value As String) As SubFamily
        Dim result As New SubFamily
        Dim query As String = "select CSCODE,CSDESC,CSFAMI,CFDESC,COCODE,CODESC from SUBFAMIGM
                               LEFT JOIN FAMILIAGM ON CSFAMI = CFCODE
                               LEFT JOIN COMODITY ON CFCOMO = COCODE where CSDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CSDESC", value.ToString)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New SubFamily With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)),
                                              .ParentId = stringNull(oReader.Item(2)), .Parent = stringNull(oReader.Item(3)),
                                              .GrandparentId = stringNull(oReader.Item(4)), .Grandparent = stringNull(oReader.Item(5))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function GetElementByCode(id As String) As Element
        Dim result As New Element
        Dim query As String = "select CECODE,CEDESC,CESUBF,CSCODE,CSDESC from ELEMENTGM
                               LEFT JOIN SUBFAMIGM S ON CESUBF = CSCODE 
                                where CECODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CECODE", id)
            cm.CommandTimeout = 30
            oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
            If (oReader.Read()) Then
                result = New Element With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)), .ParentId = stringNull(oReader.Item(3)), .Parent = stringNull(oReader.Item(4))}
            End If
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function GetFullElementByCode(ByVal id As String) As FullElement
        Dim result As New FullElement
        Dim query As String = "select CECODE,CEDESC,CESUBF,CSDESC,
                                      CSCODE,       CSFAMI,CFDESC,
                                      CFCODE,       COCODE,CODESC 
                               from ELEMENTGM 
                               LEFT JOIN SUBFAMIGM S ON CESUBF = CSCODE 
                               LEFT JOIN FAMILIAGM F ON CSFAMI = CFCODE
                               LEFT JOIN COMODITY C ON CFCOMO = COCODE
                               where CECODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.Parameters.AddWithValue("@CECODE", id)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                If (oReader.Read()) Then
                    result = New FullElement With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)),
                                                        .ParentId = stringNull(oReader.Item(2)), .Parent = stringNull(oReader.Item(3)),
                                                        .GrandparentId = stringNull(oReader.Item(5)), .Grandparent = stringNull(oReader.Item(6)),
                                                        .GrandgrandparentId = stringNull(oReader.Item(8)), .Grandgrandparent = stringNull(oReader.Item(9))}
                End If
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function GetFullElementByName(ByVal newname As String) As FullElement
        Dim result As New FullElement
        Dim query As String = "select CECODE,CEDESC,CESUBF,CSDESC,
                                      CSCODE,       CSFAMI,CFDESC,
                                      CFCODE,       COCODE,CODESC 
                               from ELEMENTGM 
                               LEFT JOIN SUBFAMIGM S ON CESUBF = CSCODE 
                               LEFT JOIN FAMILIAGM F ON CSFAMI = CFCODE
                               LEFT JOIN COMODITY C ON CFCOMO = COCODE
                               where CEDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.Parameters.AddWithValue("@CEDESC", newname)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                If (oReader.Read()) Then
                    result = New FullElement With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)),
                                                        .ParentId = stringNull(oReader.Item(2)), .Parent = stringNull(oReader.Item(3)),
                                                        .GrandparentId = stringNull(oReader.Item(5)), .Grandparent = stringNull(oReader.Item(6)),
                                                        .GrandgrandparentId = stringNull(oReader.Item(8)), .Grandgrandparent = stringNull(oReader.Item(9))}
                End If
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="newname"></param>
    ''' <returns></returns>
    Public Function GetFullDataByName(ByVal newname As String) As DataFull
        Dim result As New DataFull
        Dim query As String = "select CECODE,CEDESC,CESUBF,CSDESC,
                                      CSCODE,       CSFAMI,CFDESC,
                                      CFCODE,       COCODE,CODESC 
                               from ELEMENTGM 
                               LEFT JOIN SUBFAMIGM S ON CESUBF = CSCODE 
                               LEFT JOIN FAMILIAGM F ON CSFAMI = CFCODE
                               LEFT JOIN COMODITY C ON CFCOMO = COCODE
                               where CEDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.Parameters.AddWithValue("@CEDESC", newname)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                If (oReader.Read()) Then
                    result = New DataFull With {.Code = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)),
                                                        .SubfamilyId = stringNull(oReader.Item(2)), .Subfamily = stringNull(oReader.Item(3)),
                                                        .FamilyId = stringNull(oReader.Item(5)), .Family = stringNull(oReader.Item(6)),
                                                        .ComodityId = stringNull(oReader.Item(8)), .Comodity = stringNull(oReader.Item(9))}
                End If
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Friend Function GetElementsBySubfamilyCode(id As String) As List(Of Element)
        Dim result As New List(Of Element)
        Dim query As String = "SELECT CECODE,CEDESC from ELEMENTGM
                               LEFT JOIN SUBFAMIGM ON CESUBF = CSCODE 
                                where CSCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query, cn)
                cm.Parameters.AddWithValue("@CSCODE", id)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader(CommandBehavior.CloseConnection)
                While (oReader.Read())
                    result.Add(New Element With {.Id = stringNull(oReader.Item(0)), .Name = stringNull(oReader.Item(1)), .ParentId = id})
                End While
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de Brain", ex)
            End Try
        End Using
        Return result
    End Function
#End Region

#Region "Create"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="itemName"></param>
    ''' <returns></returns>
    Public Function CreateComodity(itemName As String) As String
        Dim count As Integer = 1
        Dim exists As Boolean = False
        Dim query0 As String = "SELECT * FROM COMODITY WHERE CODESC = ?"
        Dim query1 As String = "select max(COCODE)+1 FROM COMODITY"
        Dim query2 As String = "insert into COMODITY (COFIRM,COWKNR,COCODE,CODESC) VALUES(?,?,?,?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query0, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CODESC", itemName)
                oReader = cm.ExecuteReader()
                If oReader.Read() Then
                    Return Nothing
                End If
                cm = New OleDb.OleDbCommand(query1, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader()
                If (oReader.Read()) Then
                    count = oReader.Item(0)
                End If
                cm = New OleDb.OleDbCommand(query2, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@COFIRM", 1)
                cm.Parameters.AddWithValue("@COWKNR", 0)
                cm.Parameters.AddWithValue("@COCODE", count)
                cm.Parameters.AddWithValue("@CODESC", itemName)
                cm.ExecuteNonQuery()
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de un cost carrier de Brain", ex)
            End Try
            Return count.ToString()
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="itemName"></param>
    ''' <returns></returns>
    Public Function CreateComodityOracle(itemName As String) As String
        Dim count As Integer = 1
        Dim exists As Boolean = False
        Dim query0 As String = "SELECT * FROM COMODITY WHERE CODESC = ?"
        Dim query1 As String = "select max(COCODE)+1 FROM COMODITY"
        Dim query2 As String = "insert into COMODITY (COFIRM,COWKNR,COCODE,CODESC) VALUES(?,?,?,?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query0, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CODESC", itemName)
                oReader = cm.ExecuteReader()
                If oReader.Read() Then
                    Return Nothing
                End If
                cm = New OleDb.OleDbCommand(query1, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader()
                If (oReader.Read()) Then
                    count = oReader.Item(0)
                End If
                cm = New OleDb.OleDbCommand(query2, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@COFIRM", 1)
                cm.Parameters.AddWithValue("@COWKNR", 0)
                cm.Parameters.AddWithValue("@COCODE", count)
                cm.Parameters.AddWithValue("@CODESC", itemName)

                query2 = "insert into COMODITY (COFIRM,COWKNR,COCODE,CODESC) VALUES(1,0," & count & ",'" & itemName & "')"
                Memcached.OracleDirectAccess.NoQuery(query2, cnStrOracle)

                'cm.ExecuteNonQuery()
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de un cost carrier de Oracle", ex)
            End Try
            Return count.ToString()
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    Public Function CreateFamily(item As Family) As String
        Dim count As Integer = 1
        Dim query0 As String = "SELECT * FROM FAMILIAGM WHERE CFDESC = ?"
        Dim query1 As String = "select max(CFCODE)+1 FROM FAMILIAGM"
        Dim query2 As String = "insert into FAMILIAGM (CFFIRM,CFWKNR,CFCODE,CFDESC,CFCOMO) VALUES(?,?,?,?,?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query0, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CFDESC", item.Name)
                oReader = cm.ExecuteReader()
                If oReader.Read() Then
                    Return Nothing
                End If

                cm = New OleDb.OleDbCommand(query1, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader()
                If (oReader.Read()) Then
                    count = oReader.Item(0)
                End If
                cm = New OleDb.OleDbCommand(query2, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CFFIRM", "1")
                cm.Parameters.AddWithValue("@CFWKNR", "000")
                cm.Parameters.AddWithValue("@CFCODE", count.ToString("D2"))
                cm.Parameters.AddWithValue("@CFDESC", item.Name)
                cm.Parameters.AddWithValue("@CFCOMO", item.Parent)
                Dim affectedCount = cm.ExecuteNonQuery()
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de un cost carrier de Brain", ex)
            End Try
        End Using
        Return count
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    Public Function CreateFamilyOracle(item As Family) As String
        Dim count As Integer = 1
        Dim query0 As String = "SELECT * FROM FAMILIAGM WHERE CFDESC = ?"
        Dim query1 As String = "select max(CFCODE)+1 FROM FAMILIAGM"
        Dim query2 As String = "insert into FAMILIAGM (CFFIRM,CFWKNR,CFCODE,CFDESC,CFCOMO) VALUES(?,?,?,?,?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query0, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CFDESC", item.Name)
                oReader = cm.ExecuteReader()
                If oReader.Read() Then
                    Return Nothing
                End If

                cm = New OleDb.OleDbCommand(query1, cn)
                cm.CommandTimeout = 30
                oReader = cm.ExecuteReader()
                If (oReader.Read()) Then
                    count = oReader.Item(0)
                End If
                cm = New OleDb.OleDbCommand(query2, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CFFIRM", "1")
                cm.Parameters.AddWithValue("@CFWKNR", "000")
                cm.Parameters.AddWithValue("@CFCODE", count.ToString("D2"))
                cm.Parameters.AddWithValue("@CFDESC", item.Name)
                cm.Parameters.AddWithValue("@CFCOMO", item.Parent)
                'Dim affectedCount = cm.ExecuteNonQuery()
                query2 = "insert into FAMILIAGM (CFFIRM,CFWKNR,CFCODE,CFDESC,CFCOMO) VALUES('1','000','" & count.ToString("D2") & "','" & item.Name & "','" & item.Parent & "')"
                Memcached.OracleDirectAccess.NoQuery(query2, cnStrOracle)

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de un cost carrier de Brain", ex)
            End Try
        End Using
        Return count
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    Public Function CreateSubFamily(item As SubFamily) As String
        ''TODO: id generation from familycode...
        Dim count As Integer = 1
        Dim query0 As String = "SELECT * FROM SUBFAMIGM WHERE CSDESC = ?"
        'Dim query1 As String = "SELECT MAX(CSCODE)+1 FROM SUBFAMIGM WHERE CSFAMI = ?"
        Dim query1 As String = "SELECT MAX(CSCODE)+1 FROM SUBFAMIGM"
        Dim query2 As String = "insert into SUBFAMIGM (CSFIRM,CSWKNR,CSCODE,CSDESC,CSFAMI) VALUES(?,?,?,?,?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query0, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CSDESC", item.Name)
                oReader = cm.ExecuteReader()
                If oReader.Read() Then
                    Return Nothing
                End If

                cm = New OleDb.OleDbCommand(query1, cn)
                cm.CommandTimeout = 30
                'cm.Parameters.AddWithValue("@CSFAMI", item.ParentId)
                oReader = cm.ExecuteReader()
                If (oReader.Read()) Then
                    Dim itemTemp = oReader.Item(0)
                    count = If(itemTemp Is Nothing OrElse itemTemp.ToString.Equals(""), 1, itemTemp)
                End If

                cm = New OleDb.OleDbCommand(query2, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CSFIRM", "1")
                cm.Parameters.AddWithValue("@CSWKNR", "000")
                cm.Parameters.AddWithValue("@CSCODE", count.ToString("D4"))
                cm.Parameters.AddWithValue("@CSDESC", item.Name)
                cm.Parameters.AddWithValue("@CSFAMI", item.ParentId)
                Dim affectedCount = cm.ExecuteNonQuery()
            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de un cost carrier de Brain", ex)
            End Try
        End Using
        Return count
    End Function



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="item"></param>
    ''' <returns></returns>
    Public Function CreateSubFamilyOracle(item As SubFamily) As String
        ''TODO: id generation from familycode...
        Dim count As Integer = 1
        Dim query0 As String = "SELECT * FROM SUBFAMIGM WHERE CSDESC = ?"
        'Dim query1 As String = "SELECT MAX(CSCODE)+1 FROM SUBFAMIGM WHERE CSFAMI = ?"
        Dim query1 As String = "SELECT MAX(CSCODE)+1 FROM SUBFAMIGM"
        Dim query2 As String = "insert into SUBFAMIGM (CSFIRM,CSWKNR,CSCODE,CSDESC,CSFAMI) VALUES(?,?,?,?,?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Using cn = New OleDb.OleDbConnection(cnStr)
            Try
                cn.Open()
                Dim cm As New OleDb.OleDbCommand(query0, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CSDESC", item.Name)
                oReader = cm.ExecuteReader()
                If oReader.Read() Then
                    Return Nothing
                End If

                cm = New OleDb.OleDbCommand(query1, cn)
                cm.CommandTimeout = 30
                'cm.Parameters.AddWithValue("@CSFAMI", item.ParentId)
                oReader = cm.ExecuteReader()
                If (oReader.Read()) Then
                    Dim itemTemp = oReader.Item(0)
                    count = If(itemTemp Is Nothing OrElse itemTemp.ToString.Equals(""), 1, itemTemp)
                End If

                cm = New OleDb.OleDbCommand(query2, cn)
                cm.CommandTimeout = 30
                cm.Parameters.AddWithValue("@CSFIRM", "1")
                Dim par1 As String = "1"
                cm.Parameters.AddWithValue("@CSWKNR", "000")
                Dim par2 As String = "000"
                cm.Parameters.AddWithValue("@CSCODE", count.ToString("D4"))
                Dim par3 As String = count.ToString("D4")
                cm.Parameters.AddWithValue("@CSDESC", item.Name)
                Dim par4 As String = item.Name
                cm.Parameters.AddWithValue("@CSFAMI", item.ParentId)
                Dim par5 As String = item.ParentId
                '         Dim affectedCount = cm.ExecuteNonQuery()
                'ORACLE

                query2 = "insert into SUBFAMIGM (CSFIRM,CSWKNR,CSCODE,CSDESC,CSFAMI) VALUES('" & par1 & "','" & par2 & "','" & par3 & "','" & par4 & "','" & par5 & "')"

                Memcached.OracleDirectAccess.NoQuery(query2, cnStrOracle)

            Catch ex As Exception
                Throw New SabLib.BatzException("Error al obtener los datos de un cost carrier de Brain", ex)
            End Try
        End Using
        Return count
    End Function

    ''''''' <summary>
    ''''''' 
    ''''''' </summary>
    ''''''' <param name="item"></param>
    ''''''' <returns></returns>
    ''''Public Function CreateElement(item As Element) As String
    ''''    Dim count As Integer = 1
    ''''    Dim query0 As String = "SELECT * FROM ELEMENTGM WHERE CEDESC = ?"
    ''''    Dim query1 As String = "SELECT MAX(CECODE)+1 FROM ELEMENTGM WHERE CESUBF = ?"
    ''''    Dim query2 As String = "insert into ELEMENTGM (CEFIRM,CEWKNR,CECODE,CEDESC,CESUBF) VALUES(?,?,?,?,?)"
    ''''    Dim oReader As OleDb.OleDbDataReader = Nothing
    ''''    Using cn = New OleDb.OleDbConnection(cnStr)
    ''''        Try
    ''''            cn.Open()
    ''''            Dim cm As New OleDb.OleDbCommand(query0, cn)
    ''''            cm.CommandTimeout = 30
    ''''            cm.Parameters.AddWithValue("@CEDESC", item.Name)
    ''''            oReader = cm.ExecuteReader()
    ''''            If oReader.Read() Then
    ''''                Return Nothing
    ''''            End If

    ''''            cm = New OleDb.OleDbCommand(query1, cn)
    ''''            cm.CommandTimeout = 30
    ''''            cm.Parameters.AddWithValue("@CESUBF", item.Parent)
    ''''            oReader = cm.ExecuteReader()
    ''''            If (oReader.Read()) Then
    ''''                count = oReader.Item(0)
    ''''            End If
    ''''            cm = New OleDb.OleDbCommand(query2, cn)
    ''''            cm.CommandTimeout = 30
    ''''            cm.Parameters.AddWithValue("@CEFIRM", "1")
    ''''            cm.Parameters.AddWithValue("@CEWKNR", "000")
    ''''            cm.Parameters.AddWithValue("@CECODE", count.ToString("D6"))
    ''''            cm.Parameters.AddWithValue("@CEDESC", item.Name)
    ''''            cm.Parameters.AddWithValue("@CESUBF", item.Parent)
    ''''            Dim affectedCount = cm.ExecuteNonQuery()
    ''''        Catch ex As Exception
    ''''            Throw New SabLib.BatzException("Error al obtener los datos de un cost carrier de Brain", ex)
    ''''        End Try
    ''''    End Using
    ''''    Return count
    ''''End Function

    '''' <summary>
    '''' 
    '''' </summary>
    '''' <param name="e"></param>
    '''' <returns></returns>
    'Friend Function CreateFullElement(e As FullElement) As String
    '    Dim count As Integer = 1
    '    Dim query0 As String = "SELECT * FROM ELEMENTGM WHERE CEDESC = ?"
    '    Dim query1 As String = "SELECT MAX(CECODE) FROM ELEMENTGM WHERE CESUBF = ?"
    '    Dim query2 As String = "insert into ELEMENTGM (CEFIRM,CEWKNR,CECODE,CEDESC,CESUBF) VALUES(?,?,?,?,?)"
    '    Dim oReader As OleDb.OleDbDataReader = Nothing
    '    Using cn = New OleDb.OleDbConnection(cnStr)
    '        Try
    '            cn.Open()
    '            Dim cm As New OleDb.OleDbCommand(query0, cn)
    '            cm.CommandTimeout = 30
    '            cm.Parameters.AddWithValue("@CEDESC", e.Name)
    '            oReader = cm.ExecuteReader()
    '            If oReader.Read() Then
    '                Return Nothing
    '            End If

    '            cm = New OleDb.OleDbCommand(query1, cn)
    '            cm.CommandTimeout = 30
    '            cm.Parameters.AddWithValue("@CESUBF", e.Parent)
    '            oReader = cm.ExecuteReader()
    '            If (oReader.Read()) Then
    '                Dim itemTemp = oReader.Item(0)
    '                count = If(itemTemp Is Nothing OrElse itemTemp.ToString.Equals(""), 1, itemTemp + 1)
    '            End If
    '            cm = New OleDb.OleDbCommand(query2, cn)
    '            cm.CommandTimeout = 30
    '            cm.Parameters.AddWithValue("@CEFIRM", "1")
    '            cm.Parameters.AddWithValue("@CEWKNR", "000")
    '            cm.Parameters.AddWithValue("@CECODE", count.ToString("D6"))
    '            cm.Parameters.AddWithValue("@CEDESC", e.Name)
    '            cm.Parameters.AddWithValue("@CESUBF", e.Parent)
    '            Dim affectedCount = cm.ExecuteNonQuery()
    '        Catch ex As Exception
    '            Throw New SabLib.BatzException("Error al obtener los datos de un cost carrier de Brain", ex)
    '        End Try
    '    End Using
    '    Return count
    'End Function

    Friend Sub createNewElement(e As Element)
        Dim result As String = ""
        Dim query As String = "INSERT INTO ELEMENTGM (CEFIRM,CEWKNR,CECODE,CESUBF,CEDESC) 
                               VALUES ('1','000',?,?,?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()

            Dim cm = New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CECODE", e.Id)
            cm.Parameters.AddWithValue("@CESUBF", If(e.ParentId Is Nothing, e.Parent, e.ParentId))
            cm.Parameters.AddWithValue("@CEDESC", e.Name)
            cm.CommandTimeout = 30
            result = cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Sub

    Friend Sub createNewElementOracle(e As Element)
        Dim result As String = ""
        Dim query As String = "INSERT INTO ELEMENTGM (CEFIRM,CEWKNR,CECODE,CESUBF,CEDESC) 
                               VALUES ('1','000',?,?,?)"
        'Dim oReader As OleDb.OleDbDataReader = Nothing
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()

            'Dim cm = New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CECODE", e.Id)
            'cm.Parameters.AddWithValue("@CESUBF", If(e.ParentId Is Nothing, e.Parent, e.ParentId))
            'cm.Parameters.AddWithValue("@CEDESC", e.Name)
            'cm.CommandTimeout = 30
            'result = cm.ExecuteNonQuery()
            query = "INSERT INTO ELEMENTGM (CEFIRM,CEWKNR,CECODE,CESUBF,CEDESC) 
                               VALUES ('1','000','" & e.Id & "','" & If(e.ParentId Is Nothing, e.Parent, e.ParentId) & "','" & e.Name & "')"
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Oracle", ex)

        End Try
    End Sub

#End Region

#Region "Save/Update"
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Friend Function saveElement(e As Element) As Object
        Dim ec As New FullElement With {.Name = e.Name, .ParentId = e.ParentId, .Id = e.Id}
        Return saveMyElement(ec)
        Return saveMyElementOracle(ec)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Friend Function saveFullElement(e As FullElement) As String

        saveMyElementOracle(e)
        Dim result = saveMyElement(e)
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Friend Function saveMyElementOracle(e As FullElement) As String
        Dim result As String = ""
        'Dim query2 As String = "UPDATE ELEMENTGM SET CEDESC = ?, CESUBF = ? WHERE CECODE = ?"

        Try
            Dim queryO As String
            queryO = "UPDATE ELEMENTGM SET CEDESC = '" & e.Name & "',CESUBF = '" & If(e.ParentId Is Nothing, e.Parent, e.ParentId) & "' WHERE CECODE =" & e.Id
            Memcached.OracleDirectAccess.NoQuery(queryO, cnStrOracle)




        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Oracle", ex)
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Friend Function saveMyElement(e As FullElement) As String
        Dim result As String = ""
        Dim query As String = "UPDATE ELEMENTGM SET CEDESC = ?, CESUBF = ? WHERE CECODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()

            Dim cm = New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CEDESC", e.Name)
            cm.Parameters.AddWithValue("@CESUBF", If(e.ParentId Is Nothing, e.Parent, e.ParentId))
            cm.Parameters.AddWithValue("@CECODE", e.Id)
            cm.CommandTimeout = 30
            result = cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Friend Function saveFullData(e As DataFull) As String
        Dim result As String = String.Empty
        If e.GMCode IsNot Nothing Then
            Dim exists = checkData(e.GMCode)
            If exists Then
                updateFullDataOracle(e)
                result = updateFullData(e)
            Else
                createFullDataOracle(e)
                result = createFullData(e)
            End If
        Else
            createDataOracle(e)
            result = createData(e)
        End If
        Return result
    End Function


    '''' <summary>
    '''' 
    '''' </summary>
    '''' <param name="e"></param>
    '''' <returns></returns>
    'Friend Function CreateElementFullData(e As DataFull) As String
    '    '''''
    '    Dim result As String = String.Empty
    '    If e.GMCode IsNot Nothing Then
    '        Dim exists = checkData(e.GMCode)
    '        If exists Then
    '            result = updateFullData(e)
    '        Else
    '            result = createFullData(e)
    '        End If
    '    Else
    '        result = createData(e)
    '    End If
    '    Return result
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="code"></param>
    ''' <returns></returns>
    Private Function checkData(code As String) As Boolean
        Dim result As Boolean = False
        Dim query = "SELECT CEDESC FROM ELEMENTGM WHERE CECODE LIKE ? "
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()

            Dim cm = New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CECODE", code)
            cm.CommandTimeout = 30
            Dim description As String = cm.ExecuteScalar()

            result = description IsNot Nothing AndAlso Not description.Trim.Equals("")
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function createFullData(e As DataFull) As String
        Dim result As String = ""
        Dim query As String = "INSERT INTO ELEMENTGM (CEFIRM, CEWKNR,CECODE,CESUBF,CEDESC)
                               VALUES ('1','000', ? , ? , ?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()

            Dim cm = New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CECODE", e.GMCode)
            cm.Parameters.AddWithValue("@CESUBF", If(e.SubfamilyId1 Is Nothing, "", e.SubfamilyId1))
            cm.Parameters.AddWithValue("@CEDESC", e.Name)
            cm.CommandTimeout = 30
            result = cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function createFullDataOracle(e As DataFull) As String
        Dim result As String = ""
        Dim query As String = "INSERT INTO ELEMENTGM (CEFIRM, CEWKNR,CECODE,CESUBF,CEDESC)
                               VALUES ('1','000', ? , ? , ?)"
        'Dim oReader As OleDb.OleDbDataReader = Nothing
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()

            'Dim cm = New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CECODE", e.GMCode)
            'cm.Parameters.AddWithValue("@CESUBF", If(e.SubfamilyId1 Is Nothing, "", e.SubfamilyId1))
            'cm.Parameters.AddWithValue("@CEDESC", e.Name)
            'cm.CommandTimeout = 30
            'result = cm.ExecuteNonQuery()
            query = "INSERT INTO ELEMENTGM (CEFIRM, CEWKNR,CECODE,CESUBF,CEDESC)
                               VALUES ('1','000', '" & e.GMCode & "' , '" & If(e.SubfamilyId1 Is Nothing, "", e.SubfamilyId1) & "' , '" & e.Name & "')"
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)

        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function createData(e As DataFull) As String
        'Dim code = getNextCodeFor(New Element)
        'e.GMCode = code
        'If checkIfElementNameExists(e.Name) Then
        '    Return ""
        'End If

        Dim result As String = ""
        Dim query As String = "INSERT INTO ELEMENTGM (CEFIRM, CEWKNR,CECODE,CESUBF,CEDESC)
                               VALUES ('1','000', '' , ? , ?)"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm = New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CECODE", e.GMCode)
            'cm.Parameters.AddWithValue("@CECODE", "")
            cm.Parameters.AddWithValue("@CESUBF", If(e.SubfamilyId Is Nothing, e.Subfamily, e.SubfamilyId))
            cm.Parameters.AddWithValue("@CEDESC", e.Name)
            cm.CommandTimeout = 30
            result = cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Private Function createDataOracle(e As DataFull) As String
        'Dim query As String = "INSERT INTO ELEMENTGM (CEFIRM, CEWKNR,CECODE,CESUBF,CEDESC)
        '                       VALUES ('1','000', '' ,'" & e.Subfamily & "', '" & e.Name1 & "')"
        Dim par1 As String = If(e.SubfamilyId Is Nothing, e.Subfamily, e.SubfamilyId)
        Dim par2 As String = e.Name
        Dim result As String = ""
        Dim query2 As String
        query2 = "INSERT INTO ELEMENTGM (CEFIRM, CEWKNR,CECODE,CESUBF,CEDESC)
                               VALUES ('1','000', '' ,'" & par1 & "', '" & par2 & "')"
        Try
            Memcached.OracleDirectAccess.NoQuery(query2, cnStrOracle)

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Oracle", ex)
        End Try
        Return result
    End Function

    'Private Function checkIfElementNameExists(name As String) As Boolean
    '    Dim result As String = ""
    '    Dim query As String = "SELECT CEFIRM FROM ELEMENTGM WHERE CEDESC LIKE ?"
    '    Dim oReader As OleDb.OleDbDataReader = Nothing
    '    Dim cn = New OleDb.OleDbConnection(cnStr)
    '    Try
    '        cn.Open()
    '        Dim cm = New OleDb.OleDbCommand(query, cn)
    '        cm.Parameters.AddWithValue("@CEDESC", name)
    '        cm.CommandTimeout = 30
    '        result = cm.ExecuteScalar()
    '    Catch ex As Exception
    '        Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
    '    Finally
    '        If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
    '        If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
    '    End Try
    '    Return result
    'End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Friend Function updateFullData(e As DataFull) As String
        e.Code = e.GMCode
        Dim result As String = ""
        Dim query As String = "UPDATE ELEMENTGM SET CESUBF = ?, CECODE = ? WHERE CEDESC = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()

            Dim cm = New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CESUBF", e.SubfamilyId1)
            cm.Parameters.AddWithValue("@CECODE", e.Code)
            cm.Parameters.AddWithValue("@CEDESC", e.Name)
            cm.CommandTimeout = 30
            result = cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    ''' <returns></returns>
    Friend Function updateFullDataOracle(e As DataFull) As String
        e.Code = e.GMCode
        Dim result As String = ""
        Dim query As String = "UPDATE ELEMENTGM SET CESUBF = ?, CECODE = ? WHERE CEDESC = ?"
        'Dim oReader As OleDb.OleDbDataReader = Nothing
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()

            'Dim cm = New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CESUBF", e.SubfamilyId1)
            'cm.Parameters.AddWithValue("@CECODE", e.Code)
            'cm.Parameters.AddWithValue("@CEDESC", e.Name)
            'cm.CommandTimeout = 30
            'result = cm.ExecuteNonQuery()
            query = "UPDATE ELEMENTGM SET CESUBF = '" & e.SubfamilyId1 & "', CECODE = '" & e.Code & "' WHERE CEDESC = '" & e.Name & "'"
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Oracle", ex)

        End Try
        Return result
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    Friend Sub saveSubfamily(e As SubFamily)
        'Dim familiaId = e.Parent
        'If e.Parent Is Nothing Then
        '    familiaId = GetFamiliaIdFromName(e.Parent)
        'End If
        Dim query As String = "UPDATE SUBFAMIGM SET CSDESC = ?, CSFAMI = ? WHERE CSCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CSDESC", e.Name)
            cm.Parameters.AddWithValue("@CSFAMI", e.ParentId)
            cm.Parameters.AddWithValue("@CSCODE", e.Id)
            cm.CommandTimeout = 30
            cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    Friend Sub saveSubfamilyOracle(e As SubFamily)
        'Dim familiaId = e.Parent
        'If e.Parent Is Nothing Then
        '    familiaId = GetFamiliaIdFromName(e.Parent)
        'End If
        Dim query As String = "UPDATE SUBFAMIGM SET CSDESC = ?, CSFAMI = ? WHERE CSCODE = ?"
        'Dim oReader As OleDb.OleDbDataReader = Nothing
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()
            'Dim cm As New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CSDESC", e.Name)
            Dim par1 As String = e.Name
            'cm.Parameters.AddWithValue("@CSFAMI", e.ParentId)
            Dim par2 As String = e.ParentId
            'cm.Parameters.AddWithValue("@CSCODE", e.Id)
            Dim par3 As String = e.Id
            'cm.CommandTimeout = 30
            'cm.ExecuteNonQuery()


            Query = "UPDATE SUBFAMIGM SET CSDESC = '" & par1 & "', CSFAMI = '" & par2 & "' WHERE CSCODE = '" & par3 & "'"
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)


        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Oracle", ex)

        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    Friend Sub saveFamily(e As Family)
        Dim comodityId = e.ParentId
        If e.ParentId Is Nothing Then
            comodityId = GetComodityIdFromName(e.Parent)
        End If

        Dim query As String = "UPDATE FAMILIAGM SET CFDESC = ?, CFCOMO = ? WHERE CFCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFDESC", e.Name)
            cm.Parameters.AddWithValue("@CFCOMO", comodityId)
            cm.Parameters.AddWithValue("@CFCODE", e.Id)
            cm.CommandTimeout = 30
            cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Sub



    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    Friend Sub saveFamilyOracle(e As Family)
        Dim comodityId = e.ParentId
        If e.ParentId Is Nothing Then
            comodityId = GetComodityIdFromName(e.Parent)
        End If

        Dim query As String = "UPDATE FAMILIAGM SET CFDESC = ?, CFCOMO = ? WHERE CFCODE = ?"
        'Dim oReader As OleDb.OleDbDataReader = Nothing
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()
            'Dim cm As New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CFDESC", e.Name)
            Dim par1 As String = e.Name
            'cm.Parameters.AddWithValue("@CFCOMO", comodityId)
            Dim par2 As String = comodityId
            'cm.Parameters.AddWithValue("@CFCODE", e.Id)
            Dim par3 As String = e.Id
            'cm.CommandTimeout = 30
            'cm.ExecuteNonQuery()

            query = "UPDATE FAMILIAGM SET CFDESC = '" & par1 & "', CFCOMO = '" & par2 & "' WHERE CFCODE = '" & par3 & "'"
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Oracle", ex)

        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="e"></param>
    Friend Sub saveComodity(e As Comodity)
        Dim query As String = "UPDATE COMODITY SET CODESC = ? WHERE COCODE = ?"
        Dim oReader As OleDb.OleDbDataReader = Nothing
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CODESC", e.Name)
            cm.Parameters.AddWithValue("@COCODE", e.Id)
            cm.CommandTimeout = 30
            cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al actualizar los datos de elemento en Brain", ex)
        Finally
            If (oReader IsNot Nothing AndAlso Not oReader.IsClosed) Then oReader.Close()
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Sub
#End Region

#Region "Delete"


    ''' </summary>
    ''' <param name="id"></param>
    Friend Sub Homologar(id As String)
        'Dim query As String = "DELETE FROM ELEMENTGM WHERE CECODE = ?"
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        'Try
        '    cn.Open()
        '    Dim cm As New OleDb.OleDbCommand(query, cn)
        '    cm.Parameters.AddWithValue("@CECODE", id)
        '    cm.CommandTimeout = 30
        '    cm.ExecuteNonQuery()
        'Catch ex As Exception
        '    Throw New SabLib.BatzException("Error al hacer el 'unassign'", ex)
        'Finally
        '    If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        'End Try
        If id.Trim.Equals("") Then
            Exit Sub
        End If
        Dim query As String = "xxxxxxxx"

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Friend Sub UnAssign(id As String)
        'Dim query As String = "DELETE FROM ELEMENTGM WHERE CECODE = ?"
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        'Try
        '    cn.Open()
        '    Dim cm As New OleDb.OleDbCommand(query, cn)
        '    cm.Parameters.AddWithValue("@CECODE", id)
        '    cm.CommandTimeout = 30
        '    cm.ExecuteNonQuery()
        'Catch ex As Exception
        '    Throw New SabLib.BatzException("Error al hacer el 'unassign'", ex)
        'Finally
        '    If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        'End Try
        If id.Trim.Equals("") Then
            Exit Sub
        End If
        Dim query As String = "UPDATE ELEMENTGM SET CECODE = '' WHERE CECODE = ?"
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CECODE", id)
            cm.CommandTimeout = 30
            cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al hacer el 'unassign'", ex)
        Finally
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub DeleteComodityOracle(id As String)
        Dim query As String = "DELETE FROM COMODITY WHERE COCODE=" & id
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()
            'Dim cm As New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@COCODE", id)
            'cm.CommandTimeout = 30
            'cm.ExecuteNonQuery()
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al eliminar los datos de elemento en Oracle", ex)

        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub DeleteComodity(id As String)
        Dim query As String = "DELETE FROM COMODITY WHERE COCODE=?"
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@COCODE", id)
            cm.CommandTimeout = 30
            cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al eliminar los datos de elemento en Brain", ex)
        Finally
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub DeleteFamily(id As String)
        Dim query As String = "DELETE FROM FAMILIAGM WHERE CFCODE=?"
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CFCODE", id)
            cm.CommandTimeout = 30
            cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al eliminar los datos de elemento en Brain", ex)
        Finally
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub DeleteFamilyOracle(id As String)
        Dim query As String = "DELETE FROM FAMILIAGM WHERE CFCODE=" & id
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()
            'Dim cm As New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CFCODE", id)
            'cm.CommandTimeout = 30
            'cm.ExecuteNonQuery()
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al eliminar los datos de elemento en Brain", ex)

        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub DeleteSubFamily(id As String)
        Dim query As String = "DELETE FROM SUBFAMIGM WHERE CSCODE=?"
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CSCODE", id)
            cm.CommandTimeout = 30
            cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al eliminar los datos de elemento en Brain", ex)
        Finally
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub DeleteSubFamilyOracle(id As String)
        Dim query As String = "DELETE FROM SUBFAMIGM WHERE CSCODE=" & id
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()
            'Dim cm As New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CSCODE", id)
            'cm.CommandTimeout = 30
            'cm.ExecuteNonQuery()
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al eliminar los datos de elemento en Oracle", ex)

        End Try

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub DeleteElement(id As String, name As String)
        Dim result As New Element
        'Dim query As String = "DELETE FROM ELEMENTGM WHERE CECODE=?"
        Dim query As String = "DELETE FROM ELEMENTGM WHERE CEDESC=?"
        Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            cn.Open()
            Dim cm As New OleDb.OleDbCommand(query, cn)
            cm.Parameters.AddWithValue("@CEDESC", name)
            cm.CommandTimeout = 30
            cm.ExecuteNonQuery()
        Catch ex As Exception
            Throw New SabLib.BatzException("Error al eliminar los datos de elemento en Brain", ex)
        Finally
            If (cn IsNot Nothing AndAlso cn.State <> ConnectionState.Closed) Then cn.Close()
        End Try
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="id"></param>
    Public Sub DeleteElementOracle(id As String, name As String)
        Dim result As New Element
        'Dim query As String = "DELETE FROM ELEMENTGM WHERE CECODE=?"
        Dim query As String = "DELETE FROM ELEMENTGM WHERE CEDESC=?"
        'Dim cn = New OleDb.OleDbConnection(cnStr)
        Try
            'cn.Open()
            'Dim cm As New OleDb.OleDbCommand(query, cn)
            'cm.Parameters.AddWithValue("@CEDESC", name)
            'cm.CommandTimeout = 30
            'cm.ExecuteNonQuery()
            'query = "DELETE FROM ELEMENTGM WHERE CEDESC= " & name
            query = "DELETE FROM ELEMENTGM WHERE CEDESC= '" & name & "'"
            Memcached.OracleDirectAccess.NoQuery(query, cnStrOracle)

        Catch ex As Exception
            Throw New SabLib.BatzException("Error al eliminar los datos de elemento en Brain", ex)

        End Try
    End Sub

#End Region
End Class
