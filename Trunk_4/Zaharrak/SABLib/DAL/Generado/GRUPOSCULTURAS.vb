
'===============================================================================
'BATZ, Koop. - 25/05/2010 10:24:04
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

Public MustInherit Class _GRUPOSCULTURAS
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "SAB."
			Me.QuerySource = "GRUPOSCULTURAS"
			Me.MappingName = "GRUPOSCULTURAS"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_GRUPOSCULTURAS", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal IDCULTURAS As String, ByVal IDGRUPOS As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_GRUPOSCULTURAS.Parameters.IDCULTURAS, IDCULTURAS)

parameters.Add(_GRUPOSCULTURAS.Parameters.IDGRUPOS, IDGRUPOS)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_GRUPOSCULTURAS", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property IDGRUPOS As OracleParameter
			Get
				Return New OracleParameter("p_IDGRUPOS", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property IDCULTURAS As OracleParameter
			Get
				Return New OracleParameter("p_IDCULTURAS", OracleDbType.VARCHAR2, 5)
			End Get
		End Property
		
		Public Shared ReadOnly Property NOMBRE As OracleParameter
			Get
				Return New OracleParameter("p_NOMBRE", OracleDbType.VARCHAR2, 150)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const IDGRUPOS As String = "IDGRUPOS"
        Public Const IDCULTURAS As String = "IDCULTURAS"
        Public Const NOMBRE As String = "NOMBRE"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(IDGRUPOS) = _GRUPOSCULTURAS.PropertyNames.IDGRUPOS
				ht(IDCULTURAS) = _GRUPOSCULTURAS.PropertyNames.IDCULTURAS
				ht(NOMBRE) = _GRUPOSCULTURAS.PropertyNames.NOMBRE

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const IDGRUPOS As String = "IDGRUPOS"
        Public Const IDCULTURAS As String = "IDCULTURAS"
        Public Const NOMBRE As String = "NOMBRE"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(IDGRUPOS) = _GRUPOSCULTURAS.ColumnNames.IDGRUPOS
				ht(IDCULTURAS) = _GRUPOSCULTURAS.ColumnNames.IDCULTURAS
				ht(NOMBRE) = _GRUPOSCULTURAS.ColumnNames.NOMBRE

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const IDGRUPOS As String = "s_IDGRUPOS"
        Public Const IDCULTURAS As String = "s_IDCULTURAS"
        Public Const NOMBRE As String = "s_NOMBRE"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property IDGRUPOS As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IDGRUPOS)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IDGRUPOS, Value)
			End Set
		End Property

		Public Overridable Property IDCULTURAS As String
			Get
				Return MyBase.GetString(ColumnNames.IDCULTURAS)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.IDCULTURAS, Value)
			End Set
		End Property

		Public Overridable Property NOMBRE As String
			Get
				Return MyBase.GetString(ColumnNames.NOMBRE)
			End Get
			Set(ByVal Value As String)
				MyBase.SetString(ColumnNames.NOMBRE, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_IDGRUPOS As String
			Get
				If Me.IsColumnNull(ColumnNames.IDGRUPOS) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IDGRUPOS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDGRUPOS)
				Else
					Me.IDGRUPOS = MyBase.SetDecimalAsString(ColumnNames.IDGRUPOS, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDCULTURAS As String
			Get
				If Me.IsColumnNull(ColumnNames.IDCULTURAS) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.IDCULTURAS)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDCULTURAS)
				Else
					Me.IDCULTURAS = MyBase.SetStringAsString(ColumnNames.IDCULTURAS, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_NOMBRE As String
			Get
				If Me.IsColumnNull(ColumnNames.NOMBRE) Then
					Return String.Empty
				Else
					Return MyBase.GetStringAsString(ColumnNames.NOMBRE)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.NOMBRE)
				Else
					Me.NOMBRE = MyBase.SetStringAsString(ColumnNames.NOMBRE, Value)
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
		
	
			Public ReadOnly Property IDGRUPOS() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDGRUPOS, Parameters.IDGRUPOS)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IDCULTURAS() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCULTURAS, Parameters.IDCULTURAS)
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


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property IDGRUPOS() As WhereParameter 
			Get
				If _IDGRUPOS_W Is Nothing Then
					_IDGRUPOS_W = TearOff.IDGRUPOS
				End If
				Return _IDGRUPOS_W
			End Get
		End Property

		Public ReadOnly Property IDCULTURAS() As WhereParameter 
			Get
				If _IDCULTURAS_W Is Nothing Then
					_IDCULTURAS_W = TearOff.IDCULTURAS
				End If
				Return _IDCULTURAS_W
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

		Private _IDGRUPOS_W As WhereParameter = Nothing
		Private _IDCULTURAS_W As WhereParameter = Nothing
		Private _NOMBRE_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_IDGRUPOS_W = Nothing
			_IDCULTURAS_W = Nothing
			_NOMBRE_W = Nothing
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
		
	
		Public ReadOnly Property IDGRUPOS() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDGRUPOS, Parameters.IDGRUPOS)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IDCULTURAS() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCULTURAS, Parameters.IDCULTURAS)
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


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property IDGRUPOS() As AggregateParameter 
			Get
				If _IDGRUPOS_W Is Nothing Then
					_IDGRUPOS_W = TearOff.IDGRUPOS
				End If
				Return _IDGRUPOS_W
			End Get
		End Property

		Public ReadOnly Property IDCULTURAS() As AggregateParameter 
			Get
				If _IDCULTURAS_W Is Nothing Then
					_IDCULTURAS_W = TearOff.IDCULTURAS
				End If
				Return _IDCULTURAS_W
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

		Private _IDGRUPOS_W As AggregateParameter = Nothing
		Private _IDCULTURAS_W As AggregateParameter = Nothing
		Private _NOMBRE_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_IDGRUPOS_W = Nothing
		_IDCULTURAS_W = Nothing
		_NOMBRE_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_GRUPOSCULTURAS" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_GRUPOSCULTURAS" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_GRUPOSCULTURAS" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.IDCULTURAS)
		p.SourceColumn = ColumnNames.IDCULTURAS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDGRUPOS)
		p.SourceColumn = ColumnNames.IDGRUPOS
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.IDGRUPOS)
		p.SourceColumn = ColumnNames.IDGRUPOS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDCULTURAS)
		p.SourceColumn = ColumnNames.IDCULTURAS
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.NOMBRE)
		p.SourceColumn = ColumnNames.NOMBRE
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

