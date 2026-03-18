Imports System
Imports System.Data
Imports System.Data.SqlClient
Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD

Namespace DAL.SiteStadistic

    Public MustInherit Class _usuarios
        Inherits SqlClientEntity

        Public Sub New()            
            Me.QuerySource = "USUARIOS"
            Me.MappingName = "USUARIOS"
        End Sub

        '=================================================================
        '  Public Overrides Sub AddNew()
        '=================================================================
        '
        '=================================================================
        Public Overrides Sub AddNew()
            MyBase.AddNew()

        End Sub

        Public Overrides Sub FlushData()
            Me._whereClause = Nothing
            Me._aggregateClause = Nothing
            MyBase.FlushData()
        End Sub


        '=================================================================
        '  	Public Function LoadAll() As Boolean
        '=================================================================
        '  Loads all of the records in the database, and sets the currentRow to the first row
        '=================================================================
        Public Function LoadAll() As Boolean
            Dim parameters As ListDictionary = Nothing
            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "proc_usuariosLoadAll", parameters)
        End Function

        '=================================================================
        ' Public Overridable Function LoadByPrimaryKey()  As Boolean
        '=================================================================
        '  Loads a single row of via the primary key
        '=================================================================
        Public Overridable Function LoadByPrimaryKey(ByVal Aplicacion As String, ByVal Fecha_hora As DateTime, ByVal Id_usuario As Integer, ByVal Usuario As String) As Boolean
            Dim parameters As ListDictionary = New ListDictionary()

            parameters.Add(_usuarios.Parameters.Aplicacion, Aplicacion)
            parameters.Add(_usuarios.Parameters.Fecha_hora, Fecha_hora)
            parameters.Add(_usuarios.Parameters.Id_usuario, Id_usuario)
            parameters.Add(_usuarios.Parameters.Usuario, Usuario)

            Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "proc_usuariosLoadByPrimaryKey", parameters)
        End Function

#Region "Parameters"
        Protected Class Parameters

            Public Shared ReadOnly Property Id_usuario() As SqlParameter
                Get
                    Return New SqlParameter("Id_usuario", SqlDbType.Decimal, 0)
                End Get
            End Property

            Public Shared ReadOnly Property Usuario() As SqlParameter
                Get
                    Return New SqlParameter("Usuario", SqlDbType.NVarChar, 50)
                End Get
            End Property

            Public Shared ReadOnly Property N_trabajador() As SqlParameter
                Get
                    Return New SqlParameter("N_trabajador", SqlDbType.Int, 0)
                End Get
            End Property

            Public Shared ReadOnly Property Aplicacion() As SqlParameter
                Get
                    Return New SqlParameter("Aplicacion", SqlDbType.NVarChar, 50)
                End Get
            End Property

            Public Shared ReadOnly Property Fecha_hora() As SqlParameter
                Get
                    Return New SqlParameter("Fecha_hora", SqlDbType.DateTime, 0)
                End Get
            End Property

            Public Shared ReadOnly Property Ip() As SqlParameter
                Get
                    Return New SqlParameter("Ip", SqlDbType.NVarChar, 50)
                End Get
            End Property

        End Class
#End Region

#Region "ColumnNames"
        Public Class ColumnNames

            Public Const Id_usuario As String = "id_usuario"
            Public Const Usuario As String = "usuario"
            Public Const N_trabajador As String = "n_trabajador"
            Public Const Aplicacion As String = "aplicacion"
            Public Const Fecha_hora As String = "fecha_hora"
            Public Const Ip As String = "ip"

            Public Shared Function ToPropertyName(ByVal columnName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(Id_usuario) = _usuarios.PropertyNames.Id_usuario
                    ht(Usuario) = _usuarios.PropertyNames.Usuario
                    ht(N_trabajador) = _usuarios.PropertyNames.N_trabajador
                    ht(Aplicacion) = _usuarios.PropertyNames.Aplicacion
                    ht(Fecha_hora) = _usuarios.PropertyNames.Fecha_hora
                    ht(Ip) = _usuarios.PropertyNames.Ip

                End If

                Return CType(ht(columnName), String)

            End Function

            Private Shared ht As Hashtable = Nothing
        End Class
#End Region

#Region "PropertyNames"
        Public Class PropertyNames

            Public Const Id_usuario As String = "Id_usuario"
            Public Const Usuario As String = "Usuario"
            Public Const N_trabajador As String = "N_trabajador"
            Public Const Aplicacion As String = "Aplicacion"
            Public Const Fecha_hora As String = "Fecha_hora"
            Public Const Ip As String = "Ip"

            Public Shared Function ToColumnName(ByVal propertyName As String) As String

                If ht Is Nothing Then

                    ht = New Hashtable

                    ht(Id_usuario) = _usuarios.ColumnNames.Id_usuario
                    ht(Usuario) = _usuarios.ColumnNames.Usuario
                    ht(N_trabajador) = _usuarios.ColumnNames.N_trabajador
                    ht(Aplicacion) = _usuarios.ColumnNames.Aplicacion
                    ht(Fecha_hora) = _usuarios.ColumnNames.Fecha_hora
                    ht(Ip) = _usuarios.ColumnNames.Ip

                End If

                Return CType(ht(propertyName), String)

            End Function

            Private Shared ht As Hashtable = Nothing

        End Class
#End Region

#Region "StringPropertyNames"
        Public Class StringPropertyNames

            Public Const Id_usuario As String = "s_Id_usuario"
            Public Const Usuario As String = "s_Usuario"
            Public Const N_trabajador As String = "s_N_trabajador"
            Public Const Aplicacion As String = "s_Aplicacion"
            Public Const Fecha_hora As String = "s_Fecha_hora"
            Public Const Ip As String = "s_Ip"

        End Class
#End Region

#Region "Properties"
        Public Overridable Property Id_usuario() As Integer
            Get
                Return MyBase.GetString(ColumnNames.Id_usuario)
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetInteger(ColumnNames.Id_usuario, Value)
            End Set
        End Property

        Public Overridable Property Usuario() As String
            Get
                Return MyBase.GetString(ColumnNames.Usuario)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.Usuario, Value)
            End Set
        End Property

        Public Overridable Property N_trabajador() As Integer
            Get
                Return MyBase.GetInteger(ColumnNames.N_trabajador)
            End Get
            Set(ByVal Value As Integer)
                MyBase.SetInteger(ColumnNames.N_trabajador, Value)
            End Set
        End Property

        Public Overridable Property Aplicacion() As String
            Get
                Return MyBase.GetString(ColumnNames.Aplicacion)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.Aplicacion, Value)
            End Set
        End Property

        Public Overridable Property Fecha_hora() As DateTime
            Get
                Return MyBase.GetDateTime(ColumnNames.Fecha_hora)
            End Get
            Set(ByVal Value As DateTime)
                MyBase.SetDateTime(ColumnNames.Fecha_hora, Value)
            End Set
        End Property

        Public Overridable Property Ip() As String
            Get
                Return MyBase.GetString(ColumnNames.Ip)
            End Get
            Set(ByVal Value As String)
                MyBase.SetString(ColumnNames.Ip, Value)
            End Set
        End Property


#End Region

#Region "String Properties"

        Public Overridable Property s_Id_usuario() As String
            Get
                If Me.IsColumnNull(ColumnNames.Id_usuario) Then
                    Return String.Empty
                Else
                    Return MyBase.GetIntegerAsString(ColumnNames.Id_usuario)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.Id_usuario)
                Else
                    Me.Id_usuario = MyBase.SetIntegerAsString(ColumnNames.Id_usuario, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_Usuario() As String
            Get
                If Me.IsColumnNull(ColumnNames.Usuario) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.Usuario)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.Usuario)
                Else
                    Me.Usuario = MyBase.SetStringAsString(ColumnNames.Usuario, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_N_trabajador() As String
            Get
                If Me.IsColumnNull(ColumnNames.N_trabajador) Then
                    Return String.Empty
                Else
                    Return MyBase.GetIntegerAsString(ColumnNames.N_trabajador)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.N_trabajador)
                Else
                    Me.N_trabajador = MyBase.SetIntegerAsString(ColumnNames.N_trabajador, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_Aplicacion() As String
            Get
                If Me.IsColumnNull(ColumnNames.Aplicacion) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.Aplicacion)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.Aplicacion)
                Else
                    Me.Aplicacion = MyBase.SetStringAsString(ColumnNames.Aplicacion, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_Fecha_hora() As String
            Get
                If Me.IsColumnNull(ColumnNames.Fecha_hora) Then
                    Return String.Empty
                Else
                    Return MyBase.GetDateTimeAsString(ColumnNames.Fecha_hora)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.Fecha_hora)
                Else
                    Me.Fecha_hora = MyBase.SetDateTimeAsString(ColumnNames.Fecha_hora, Value)
                End If
            End Set
        End Property

        Public Overridable Property s_Ip() As String
            Get
                If Me.IsColumnNull(ColumnNames.Ip) Then
                    Return String.Empty
                Else
                    Return MyBase.GetStringAsString(ColumnNames.Ip)
                End If
            End Get
            Set(ByVal Value As String)
                If String.Empty = Value Then
                    Me.SetColumnNull(ColumnNames.Ip)
                Else
                    Me.Ip = MyBase.SetStringAsString(ColumnNames.Ip, Value)
                End If
            End Set
        End Property


#End Region

#Region "Where Clause"
        Public Class WhereClause

            Public Sub New(ByVal entity As BusinessEntity)
                Me._entity = entity
            End Sub

            Public ReadOnly Property TearOff() As TearOffWhereParameter
                Get
                    If _tearOff Is Nothing Then
                        _tearOff = New TearOffWhereParameter(Me)
                    End If

                    Return _tearOff
                End Get
            End Property

#Region "TearOff's"
            Public Class TearOffWhereParameter

                Public Sub New(ByVal clause As WhereClause)
                    Me._clause = clause
                End Sub


                Public ReadOnly Property Id_usuario() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.Id_usuario, Parameters.Id_usuario)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property Usuario() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.Usuario, Parameters.Usuario)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property N_trabajador() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.N_trabajador, Parameters.N_trabajador)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property Aplicacion() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.Aplicacion, Parameters.Aplicacion)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property Fecha_hora() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.Fecha_hora, Parameters.Fecha_hora)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property Ip() As WhereParameter
                    Get
                        Dim where As WhereParameter = New WhereParameter(ColumnNames.Ip, Parameters.Ip)
                        Me._clause._entity.Query.AddWhereParemeter(where)
                        Return where
                    End Get
                End Property


                Private _clause As WhereClause
            End Class
#End Region

            Public ReadOnly Property Id_usuario() As WhereParameter
                Get
                    If _Id_usuario_W Is Nothing Then
                        _Id_usuario_W = TearOff.Id_usuario
                    End If
                    Return _Id_usuario_W
                End Get
            End Property

            Public ReadOnly Property Usuario() As WhereParameter
                Get
                    If _Usuario_W Is Nothing Then
                        _Usuario_W = TearOff.Usuario
                    End If
                    Return _Usuario_W
                End Get
            End Property

            Public ReadOnly Property N_trabajador() As WhereParameter
                Get
                    If _N_trabajador_W Is Nothing Then
                        _N_trabajador_W = TearOff.N_trabajador
                    End If
                    Return _N_trabajador_W
                End Get
            End Property

            Public ReadOnly Property Aplicacion() As WhereParameter
                Get
                    If _Aplicacion_W Is Nothing Then
                        _Aplicacion_W = TearOff.Aplicacion
                    End If
                    Return _Aplicacion_W
                End Get
            End Property

            Public ReadOnly Property Fecha_hora() As WhereParameter
                Get
                    If _Fecha_hora_W Is Nothing Then
                        _Fecha_hora_W = TearOff.Fecha_hora
                    End If
                    Return _Fecha_hora_W
                End Get
            End Property

            Public ReadOnly Property Ip() As WhereParameter
                Get
                    If _Ip_W Is Nothing Then
                        _Ip_W = TearOff.Ip
                    End If
                    Return _Ip_W
                End Get
            End Property

            Private _Id_usuario_W As WhereParameter = Nothing
            Private _Usuario_W As WhereParameter = Nothing
            Private _N_trabajador_W As WhereParameter = Nothing
            Private _Aplicacion_W As WhereParameter = Nothing
            Private _Fecha_hora_W As WhereParameter = Nothing
            Private _Ip_W As WhereParameter = Nothing

            Public Sub WhereClauseReset()

                _Id_usuario_W = Nothing
                _Usuario_W = Nothing
                _N_trabajador_W = Nothing
                _Aplicacion_W = Nothing
                _Fecha_hora_W = Nothing
                _Ip_W = Nothing
                Me._entity.Query.FlushWhereParameters()

            End Sub

            Private _entity As BusinessEntity
            Private _tearOff As TearOffWhereParameter
        End Class

        Public ReadOnly Property Where() As WhereClause
            Get
                If _whereClause Is Nothing Then
                    _whereClause = New WhereClause(Me)
                End If

                Return _whereClause
            End Get
        End Property

        Private _whereClause As WhereClause = Nothing
#End Region

#Region "Aggregate Clause"
        Public Class AggregateClause

            Public Sub New(ByVal entity As BusinessEntity)
                Me._entity = entity
            End Sub

            Public ReadOnly Property TearOff() As TearOffAggregateParameter
                Get
                    If _tearOff Is Nothing Then
                        _tearOff = New TearOffAggregateParameter(Me)
                    End If

                    Return _tearOff
                End Get
            End Property

#Region "AggregateParameter TearOff's"
            Public Class TearOffAggregateParameter

                Public Sub New(ByVal clause As AggregateClause)
                    Me._clause = clause
                End Sub


                Public ReadOnly Property Id_usuario() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.Id_usuario, Parameters.Id_usuario)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property Usuario() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.Usuario, Parameters.Usuario)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property N_trabajador() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.N_trabajador, Parameters.N_trabajador)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property Aplicacion() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.Aplicacion, Parameters.Aplicacion)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property Fecha_hora() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.Fecha_hora, Parameters.Fecha_hora)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property

                Public ReadOnly Property Ip() As AggregateParameter
                    Get
                        Dim where As AggregateParameter = New AggregateParameter(ColumnNames.Ip, Parameters.Ip)
                        Me._clause._entity.Query.AddAggregateParameter(where)
                        Return where
                    End Get
                End Property


                Private _clause As AggregateClause
            End Class
#End Region

            Public ReadOnly Property Id_usuario() As AggregateParameter
                Get
                    If _Id_usuario_W Is Nothing Then
                        _Id_usuario_W = TearOff.Id_usuario
                    End If
                    Return _Id_usuario_W
                End Get
            End Property

            Public ReadOnly Property Usuario() As AggregateParameter
                Get
                    If _Usuario_W Is Nothing Then
                        _Usuario_W = TearOff.Usuario
                    End If
                    Return _Usuario_W
                End Get
            End Property

            Public ReadOnly Property N_trabajador() As AggregateParameter
                Get
                    If _N_trabajador_W Is Nothing Then
                        _N_trabajador_W = TearOff.N_trabajador
                    End If
                    Return _N_trabajador_W
                End Get
            End Property

            Public ReadOnly Property Aplicacion() As AggregateParameter
                Get
                    If _Aplicacion_W Is Nothing Then
                        _Aplicacion_W = TearOff.Aplicacion
                    End If
                    Return _Aplicacion_W
                End Get
            End Property

            Public ReadOnly Property Fecha_hora() As AggregateParameter
                Get
                    If _Fecha_hora_W Is Nothing Then
                        _Fecha_hora_W = TearOff.Fecha_hora
                    End If
                    Return _Fecha_hora_W
                End Get
            End Property

            Public ReadOnly Property Ip() As AggregateParameter
                Get
                    If _Ip_W Is Nothing Then
                        _Ip_W = TearOff.Ip
                    End If
                    Return _Ip_W
                End Get
            End Property

            Private _Id_usuario_W As AggregateParameter = Nothing
            Private _Usuario_W As AggregateParameter = Nothing
            Private _N_trabajador_W As AggregateParameter = Nothing
            Private _Aplicacion_W As AggregateParameter = Nothing
            Private _Fecha_hora_W As AggregateParameter = Nothing
            Private _Ip_W As AggregateParameter = Nothing

            Public Sub AggregateClauseReset()

                _Id_usuario_W = Nothing
                _Usuario_W = Nothing
                _N_trabajador_W = Nothing
                _Aplicacion_W = Nothing
                _Fecha_hora_W = Nothing
                _Ip_W = Nothing
                Me._entity.Query.FlushAggregateParameters()

            End Sub

            Private _entity As BusinessEntity
            Private _tearOff As TearOffAggregateParameter
        End Class

        Public ReadOnly Property Aggregate() As AggregateClause
            Get
                If _aggregateClause Is Nothing Then
                    _aggregateClause = New AggregateClause(Me)
                End If

                Return _aggregateClause
            End Get
        End Property

        Private _aggregateClause As AggregateClause = Nothing
#End Region

        Protected Overrides Function GetInsertCommand() As IDbCommand

            Dim cmd As SqlCommand = New SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "proc_usuariosInsert"

            CreateParameters(cmd)

            Dim p As SqlParameter
            p = cmd.Parameters(Parameters.Id_usuario.ParameterName)
            p.Direction = ParameterDirection.Output

            Return cmd

        End Function

        Protected Overrides Function GetUpdateCommand() As IDbCommand

            Dim cmd As SqlCommand = New SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "proc_usuariosUpdate"

            CreateParameters(cmd)

            Return cmd

        End Function

        Protected Overrides Function GetDeleteCommand() As IDbCommand

            Dim cmd As SqlCommand = New SqlCommand
            cmd.CommandType = CommandType.StoredProcedure
            cmd.CommandText = Me.SchemaStoredProcedure + "proc_usuariosDelete"

            Dim p As SqlParameter

            p = cmd.Parameters.Add(Parameters.Aplicacion)
            p.SourceColumn = ColumnNames.Aplicacion
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.Fecha_hora)
            p.SourceColumn = ColumnNames.Fecha_hora
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.Id_usuario)
            p.SourceColumn = ColumnNames.Id_usuario
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.Usuario)
            p.SourceColumn = ColumnNames.Usuario
            p.SourceVersion = DataRowVersion.Current

            Return cmd

        End Function

        Private Sub CreateParameters(ByVal cmd As SqlCommand)

            Dim p As SqlParameter
            p = cmd.Parameters.Add(Parameters.Id_usuario)
            p.SourceColumn = ColumnNames.Id_usuario
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.Usuario)
            p.SourceColumn = ColumnNames.Usuario
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.N_trabajador)
            p.SourceColumn = ColumnNames.N_trabajador
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.Aplicacion)
            p.SourceColumn = ColumnNames.Aplicacion
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.Fecha_hora)
            p.SourceColumn = ColumnNames.Fecha_hora
            p.SourceVersion = DataRowVersion.Current

            p = cmd.Parameters.Add(Parameters.Ip)
            p.SourceColumn = ColumnNames.Ip
            p.SourceVersion = DataRowVersion.Current


        End Sub

    End Class

End Namespace

