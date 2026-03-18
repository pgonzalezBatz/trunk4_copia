Imports System
Imports System.Data

Imports System.Collections
Imports System.Collections.Specialized
Imports AccesoAutomaticoBD
Imports log4net

Namespace DAL.Views

	Public Class W_RECURSOS_EMPRESA
		Inherits OracleClientEntity

		Public Sub New()
			Me.SchemaGlobal = "SAB."
			Me.QuerySource = "W_RECURSOS_EMPRESA"
			Me.MappingName = "W_RECURSOS_EMPRESA"

			'Decide connection string depending on state
			If Configuration.ConfigurationManager.AppSettings.Get("CurrentStatus") = "Live" Then
				Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABLIVE").ConnectionString
			Else
				Me.ConnectionString = Configuration.ConfigurationManager.ConnectionStrings.Item("SABTEST").ConnectionString
			End If
		End Sub

		'=================================================================
		'  	Public Function LoadAll() As Boolean
		'=================================================================
		'  Loads all of the records in the database, and sets the currentRow to the first row
		'=================================================================
		Public Function LoadAll() As Boolean
			Return MyBase.Query.Load()
		End Function

		Public Overrides Sub FlushData()
			Me._whereClause = Nothing
			Me._aggregateClause = Nothing
			MyBase.FlushData()
		End Sub

#Region "Parameters"
		Protected Class Parameters

			Public Shared ReadOnly Property ID() As OracleParameter
				Get
					Return New OracleParameter("ID", OracleDbType.Int32, 0)
				End Get
			End Property

			Public Shared ReadOnly Property NOMBRE() As OracleParameter
				Get
					Return New OracleParameter("NOMBRE", OracleDbType.Varchar2, 100)
				End Get
			End Property

			Public Shared ReadOnly Property IDTROQUELERIA() As OracleParameter
				Get
					Return New OracleParameter("IDTROQUELERIA", OracleDbType.Varchar2, 12)
				End Get
			End Property

			Public Shared ReadOnly Property IDRECURSOS() As OracleParameter
				Get
					Return New OracleParameter("IDRECURSOS", OracleDbType.Int32, 0)
				End Get
			End Property

		End Class
#End Region

#Region "ColumnNames"
		Public Class ColumnNames

			Public Const ID As String = "ID"
			Public Const NOMBRE As String = "NOMBRE"
			Public Const IDTROQUELERIA As String = "IDTROQUELERIA"
			Public Const IDRECURSOS As String = "IDRECURSOS"

			Public Shared Function ToPropertyName(ByVal columnName As String) As String

				If ht Is Nothing Then

					ht = New Hashtable

					ht(ID) = W_RECURSOS_EMPRESA.PropertyNames.ID
					ht(NOMBRE) = W_RECURSOS_EMPRESA.PropertyNames.NOMBRE
					ht(IDTROQUELERIA) = W_RECURSOS_EMPRESA.PropertyNames.IDTROQUELERIA
					ht(IDRECURSOS) = W_RECURSOS_EMPRESA.PropertyNames.IDRECURSOS

				End If

				Return CType(ht(columnName), String)

			End Function

			Private Shared ht As Hashtable = Nothing
		End Class
#End Region

#Region "PropertyNames"
		Public Class PropertyNames

			Public Const ID As String = "ID"
			Public Const NOMBRE As String = "NOMBRE"
			Public Const IDTROQUELERIA As String = "IDTROQUELERIA"
			Public Const IDRECURSOS As String = "IDRECURSOS"

			Public Shared Function ToColumnName(ByVal propertyName As String) As String

				If ht Is Nothing Then

					ht = New Hashtable

					ht(ID) = W_RECURSOS_EMPRESA.ColumnNames.ID
					ht(NOMBRE) = W_RECURSOS_EMPRESA.ColumnNames.NOMBRE
					ht(IDTROQUELERIA) = W_RECURSOS_EMPRESA.ColumnNames.IDTROQUELERIA
					ht(IDRECURSOS) = W_RECURSOS_EMPRESA.ColumnNames.IDRECURSOS

				End If

				Return CType(ht(propertyName), String)

			End Function

			Private Shared ht As Hashtable = Nothing

		End Class
#End Region

#Region "StringPropertyNames"
		Public Class StringPropertyNames

			Public Const ID As String = "s_ID"
			Public Const NOMBRE As String = "s_NOMBRE"
			Public Const IDTROQUELERIA As String = "s_IDTROQUELERIA"
			Public Const IDRECURSOS As String = "s_IDRECURSOS"

		End Class
#End Region

#Region "Properties"
		Public Overridable Property ID() As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID, Value)
			End Set
		End Property

		Public Overridable Property NOMBRE() As String
			Get
				Return MyBase.GetString(ColumnNames.NOMBRE)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.NOMBRE, Value)
			End Set
		End Property

		Public Overridable Property IDTROQUELERIA() As String
			Get
				Return MyBase.GetString(ColumnNames.IDTROQUELERIA)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.IDTROQUELERIA, Value)
			End Set
		End Property

		Public Overridable Property IDRECURSOS() As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IDRECURSOS)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IDRECURSOS, Value)
			End Set
		End Property


#End Region

#Region "String Properties"

		Public Overridable Property s_ID() As String
			Get
				If Me.IsColumnNull(ColumnNames.ID) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = Value Then
					Me.SetColumnNull(ColumnNames.ID)
				Else
					Me.ID = MyBase.SetDecimalAsString(ColumnNames.ID, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_NOMBRE() As String
			Get
				If Me.IsColumnNull(ColumnNames.NOMBRE) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.NOMBRE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = Value Then
					Me.SetColumnNull(ColumnNames.NOMBRE)
				Else
					Me.NOMBRE = MyBase.SetStringAsString(ColumnNames.NOMBRE, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDTROQUELERIA() As String
			Get
				If Me.IsColumnNull(ColumnNames.IDTROQUELERIA) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.IDTROQUELERIA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = Value Then
					Me.SetColumnNull(ColumnNames.IDTROQUELERIA)
				Else
					Me.IDTROQUELERIA = MyBase.SetStringAsString(ColumnNames.IDTROQUELERIA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDRECURSOS() As String
			Get
				If Me.IsColumnNull(ColumnNames.IDRECURSOS) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IDRECURSOS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = Value Then
					Me.SetColumnNull(ColumnNames.IDRECURSOS)
				Else
					Me.IDRECURSOS = MyBase.SetDecimalAsString(ColumnNames.IDRECURSOS, Value)
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


				Public ReadOnly Property ID() As WhereParameter
					Get
						Dim where As WhereParameter = New WhereParameter(ColumnNames.ID, Parameters.ID)
						Me._clause._entity.Query.AddWhereParemeter(where)
						Return where
					End Get
				End Property

				Public ReadOnly Property NOMBRE() As WhereParameter
					Get
						Dim where As WhereParameter = New WhereParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
						Me._clause._entity.Query.AddWhereParemeter(where)
						Return where
					End Get
				End Property

				Public ReadOnly Property IDTROQUELERIA() As WhereParameter
					Get
						Dim where As WhereParameter = New WhereParameter(ColumnNames.IDTROQUELERIA, Parameters.IDTROQUELERIA)
						Me._clause._entity.Query.AddWhereParemeter(where)
						Return where
					End Get
				End Property

				Public ReadOnly Property IDRECURSOS() As WhereParameter
					Get
						Dim where As WhereParameter = New WhereParameter(ColumnNames.IDRECURSOS, Parameters.IDRECURSOS)
						Me._clause._entity.Query.AddWhereParemeter(where)
						Return where
					End Get
				End Property


				Private _clause As WhereClause
			End Class
#End Region

			Public ReadOnly Property ID() As WhereParameter
				Get
					If _ID_W Is Nothing Then
						_ID_W = TearOff.ID
					End If
					Return _ID_W
				End Get
			End Property

			Public ReadOnly Property NOMBRE() As WhereParameter
				Get
					If _NOMBRE_W Is Nothing Then
						_NOMBRE_W = TearOff.NOMBRE
					End If
					Return _NOMBRE_W
				End Get
			End Property

			Public ReadOnly Property IDTROQUELERIA() As WhereParameter
				Get
					If _IDTROQUELERIA_W Is Nothing Then
						_IDTROQUELERIA_W = TearOff.IDTROQUELERIA
					End If
					Return _IDTROQUELERIA_W
				End Get
			End Property

			Public ReadOnly Property IDRECURSOS() As WhereParameter
				Get
					If _IDRECURSOS_W Is Nothing Then
						_IDRECURSOS_W = TearOff.IDRECURSOS
					End If
					Return _IDRECURSOS_W
				End Get
			End Property

			Private _ID_W As WhereParameter = Nothing
			Private _NOMBRE_W As WhereParameter = Nothing
			Private _IDTROQUELERIA_W As WhereParameter = Nothing
			Private _IDRECURSOS_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

				_ID_W = Nothing
				_NOMBRE_W = Nothing
				_IDTROQUELERIA_W = Nothing
				_IDRECURSOS_W = Nothing
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


				Public ReadOnly Property ID() As AggregateParameter
					Get
						Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID, Parameters.ID)
						Me._clause._entity.Query.AddAggregateParameter(where)
						Return where
					End Get
				End Property

				Public ReadOnly Property NOMBRE() As AggregateParameter
					Get
						Dim where As AggregateParameter = New AggregateParameter(ColumnNames.NOMBRE, Parameters.NOMBRE)
						Me._clause._entity.Query.AddAggregateParameter(where)
						Return where
					End Get
				End Property

				Public ReadOnly Property IDTROQUELERIA() As AggregateParameter
					Get
						Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDTROQUELERIA, Parameters.IDTROQUELERIA)
						Me._clause._entity.Query.AddAggregateParameter(where)
						Return where
					End Get
				End Property

				Public ReadOnly Property IDRECURSOS() As AggregateParameter
					Get
						Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDRECURSOS, Parameters.IDRECURSOS)
						Me._clause._entity.Query.AddAggregateParameter(where)
						Return where
					End Get
				End Property


				Private _clause As AggregateClause
			End Class
#End Region

			Public ReadOnly Property ID() As AggregateParameter
				Get
					If _ID_W Is Nothing Then
						_ID_W = TearOff.ID
					End If
					Return _ID_W
				End Get
			End Property

			Public ReadOnly Property NOMBRE() As AggregateParameter
				Get
					If _NOMBRE_W Is Nothing Then
						_NOMBRE_W = TearOff.NOMBRE
					End If
					Return _NOMBRE_W
				End Get
			End Property

			Public ReadOnly Property IDTROQUELERIA() As AggregateParameter
				Get
					If _IDTROQUELERIA_W Is Nothing Then
						_IDTROQUELERIA_W = TearOff.IDTROQUELERIA
					End If
					Return _IDTROQUELERIA_W
				End Get
			End Property

			Public ReadOnly Property IDRECURSOS() As AggregateParameter
				Get
					If _IDRECURSOS_W Is Nothing Then
						_IDRECURSOS_W = TearOff.IDRECURSOS
					End If
					Return _IDRECURSOS_W
				End Get
			End Property

			Private _ID_W As AggregateParameter = Nothing
			Private _NOMBRE_W As AggregateParameter = Nothing
			Private _IDTROQUELERIA_W As AggregateParameter = Nothing
			Private _IDRECURSOS_W As AggregateParameter = Nothing

			Public Sub AggregateClauseReset()

				_ID_W = Nothing
				_NOMBRE_W = Nothing
				_IDTROQUELERIA_W = Nothing
				_IDRECURSOS_W = Nothing
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
			Return Nothing
		End Function

		Protected Overrides Function GetUpdateCommand() As IDbCommand
			Return Nothing
		End Function

		Protected Overrides Function GetDeleteCommand() As IDbCommand
			Return Nothing
		End Function

	End Class

End Namespace

