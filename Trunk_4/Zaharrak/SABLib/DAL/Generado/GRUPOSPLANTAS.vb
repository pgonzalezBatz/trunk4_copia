
'===============================================================================
'BATZ, Koop. - 25/05/2010 10:24:05
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_BusinessEntity.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized


Imports AccesoAutomaticoBD


NameSpace DAL

Public MustInherit Class _GRUPOSPLANTAS
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "SAB."
			Me.QuerySource = "GRUPOSPLANTAS"
			Me.MappingName = "GRUPOSPLANTAS"
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
		Me._whereClause = nothing
		Me._aggregateClause = nothing		
		MyBase.FlushData()
	End Sub
	
		
	'=================================================================
	'  	Public Function LoadAll() As Boolean
	'=================================================================
	'  Loads all of the records in the database, and sets the currentRow to the first row
	'=================================================================
	Public Function LoadAll() As Boolean
	
		Dim parameters As ListDictionary = Nothing
		
		parameters = New ListDictionary		
		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GRUPOSPLANTAS", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal ID_GRUPO As Decimal, ByVal ID_PLANTA As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_GRUPOSPLANTAS.Parameters.ID_GRUPO, ID_GRUPO)

parameters.Add(_GRUPOSPLANTAS.Parameters.ID_PLANTA, ID_PLANTA)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GRUPOSPLANTAS", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property ID_GRUPO As OracleParameter
			Get
				Return New OracleParameter("p_ID_GRUPO", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property ID_PLANTA As OracleParameter
			Get
				Return New OracleParameter("p_ID_PLANTA", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const ID_GRUPO As String = "ID_GRUPO"
        Public Const ID_PLANTA As String = "ID_PLANTA"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID_GRUPO) = _GRUPOSPLANTAS.PropertyNames.ID_GRUPO
				ht(ID_PLANTA) = _GRUPOSPLANTAS.PropertyNames.ID_PLANTA

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const ID_GRUPO As String = "ID_GRUPO"
        Public Const ID_PLANTA As String = "ID_PLANTA"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(ID_GRUPO) = _GRUPOSPLANTAS.ColumnNames.ID_GRUPO
				ht(ID_PLANTA) = _GRUPOSPLANTAS.ColumnNames.ID_PLANTA

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const ID_GRUPO As String = "s_ID_GRUPO"
        Public Const ID_PLANTA As String = "s_ID_PLANTA"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property ID_GRUPO As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_GRUPO)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_GRUPO, Value)
			End Set
		End Property

		Public Overridable Property ID_PLANTA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.ID_PLANTA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.ID_PLANTA, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_ID_GRUPO As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_GRUPO) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_GRUPO)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_GRUPO)
				Else
					Me.ID_GRUPO = MyBase.SetDecimalAsString(ColumnNames.ID_GRUPO, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_ID_PLANTA As String
			Get
				If Me.IsColumnNull(ColumnNames.ID_PLANTA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.ID_PLANTA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.ID_PLANTA)
				Else
					Me.ID_PLANTA = MyBase.SetDecimalAsString(ColumnNames.ID_PLANTA, Value)
				End If
			End Set
		End Property


	#End Region  	

	#Region "Where Clause"
    Public Class WhereClause

        Public Sub New(ByVal entity As BusinessEntity)
            Me._entity = entity
        End Sub
		
		Public ReadOnly Property TearOff As TearOffWhereParameter
			Get
				If _tearOff Is Nothing Then
					_tearOff = new TearOffWhereParameter(Me)
				End If

				Return _tearOff
			End Get
		End Property

		#Region "TearOff's"
		Public class TearOffWhereParameter

			Public Sub New(ByVal clause As WhereClause)
				Me._clause = clause
			End Sub
		
	
			Public ReadOnly Property ID_GRUPO() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_GRUPO, Parameters.ID_GRUPO)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property ID_PLANTA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property ID_GRUPO() As WhereParameter 
			Get
				If _ID_GRUPO_W Is Nothing Then
					_ID_GRUPO_W = TearOff.ID_GRUPO
				End If
				Return _ID_GRUPO_W
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As WhereParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
			End Get
		End Property

		Private _ID_GRUPO_W As WhereParameter = Nothing
		Private _ID_PLANTA_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_ID_GRUPO_W = Nothing
			_ID_PLANTA_W = Nothing
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
		
		Public ReadOnly Property TearOff As TearOffAggregateParameter
			Get
				If _tearOff Is Nothing Then
					_tearOff = new TearOffAggregateParameter(Me)
				End If

				Return _tearOff
			End Get
		End Property

		#Region "AggregateParameter TearOff's"
		Public class TearOffAggregateParameter

			Public Sub New(ByVal clause As AggregateClause)
				Me._clause = clause
			End Sub
		
	
		Public ReadOnly Property ID_GRUPO() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_GRUPO, Parameters.ID_GRUPO)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.ID_PLANTA, Parameters.ID_PLANTA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property ID_GRUPO() As AggregateParameter 
			Get
				If _ID_GRUPO_W Is Nothing Then
					_ID_GRUPO_W = TearOff.ID_GRUPO
				End If
				Return _ID_GRUPO_W
			End Get
		End Property

		Public ReadOnly Property ID_PLANTA() As AggregateParameter 
			Get
				If _ID_PLANTA_W Is Nothing Then
					_ID_PLANTA_W = TearOff.ID_PLANTA
				End If
				Return _ID_PLANTA_W
			End Get
		End Property

		Private _ID_GRUPO_W As AggregateParameter = Nothing
		Private _ID_PLANTA_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_ID_GRUPO_W = Nothing
		_ID_PLANTA_W = Nothing
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
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_GRUPOSPLANTAS" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_GRUPOSPLANTAS" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_GRUPOSPLANTAS" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID_GRUPO)
		p.SourceColumn = ColumnNames.ID_GRUPO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.ID_GRUPO)
		p.SourceColumn = ColumnNames.ID_GRUPO
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.ID_PLANTA)
		p.SourceColumn = ColumnNames.ID_PLANTA
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

