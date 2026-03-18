
'===============================================================================
'BATZ, Koop. - 28/07/2008 10:50:42
' Generado por MyGeneration Version # (1.3.0.3)
' Generado desde Batz_VbNet_SQL_dOOdads_BusinessEntity.vbgen
' El soporte de la clase OracleClientEntity esta en el directorio Architecture  en "dOOdads".
'===============================================================================

Imports System
Imports System.Data
Imports System.Collections
Imports System.Collections.Specialized

Imports Oracle.DataAccess.Client
Imports AccesoAutomaticoBD


NameSpace DAL

Public MustInherit Class _USUARIOSROL
	Inherits OracleClientEntity

		Public Sub New() 
			Me.SchemaGlobal = "INCIDENCIAS."
			Me.QuerySource = "USUARIOSROL"
			Me.MappingName = "USUARIOSROL"
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
		
		
    	Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PL_USUARIOSROL", parameters)		
	End Function

	'=================================================================
	' Public Overridable Function LoadByPrimaryKey()  As Boolean
	'=================================================================
	'  Loads a single row of via the primary key
	'=================================================================
	Public Overridable Function LoadByPrimaryKey(ByVal IDCLASIFICACIONFAMILIA As Decimal, ByVal IDTIPOINCIDENCIA As Decimal, ByVal IDUSR As Decimal) As Boolean

		Dim parameters As ListDictionary = New ListDictionary()
		parameters.Add(_USUARIOSROL.Parameters.IDCLASIFICACIONFAMILIA, IDCLASIFICACIONFAMILIA)

parameters.Add(_USUARIOSROL.Parameters.IDTIPOINCIDENCIA, IDTIPOINCIDENCIA)

parameters.Add(_USUARIOSROL.Parameters.IDUSR, IDUSR)


		Dim p As OracleParameter = New OracleParameter("outCursor", OracleDbType.RefCursor)
		p.Direction = ParameterDirection.Output
		parameters.Add(p, Nothing)
				
		Return MyBase.LoadFromSql(Me.SchemaStoredProcedure + "PK_USUARIOSROL", parameters)

	End Function

	#Region "Parameters"
	Protected class Parameters 
		
		Public Shared ReadOnly Property IDUSR As OracleParameter
			Get
				Return New OracleParameter("p_IDUSR", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property IDROL As OracleParameter
			Get
				Return New OracleParameter("p_IDROL", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property IDCLASIFICACIONFAMILIA As OracleParameter
			Get
				Return New OracleParameter("p_IDCLASIFICACIONFAMILIA", OracleDbType.Int32, 0)
			End Get
		End Property
		
		Public Shared ReadOnly Property IDTIPOINCIDENCIA As OracleParameter
			Get
				Return New OracleParameter("p_IDTIPOINCIDENCIA", OracleDbType.Int32, 0)
			End Get
		End Property
		
	End Class
	#End Region	

	#Region "ColumnNames"
	Public class ColumnNames
		
        Public Const IDUSR As String = "IDUSR"
        Public Const IDROL As String = "IDROL"
        Public Const IDCLASIFICACIONFAMILIA As String = "IDCLASIFICACIONFAMILIA"
        Public Const IDTIPOINCIDENCIA As String = "IDTIPOINCIDENCIA"

		Shared Public Function ToPropertyName(ByVal columnName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(IDUSR) = _USUARIOSROL.PropertyNames.IDUSR
				ht(IDROL) = _USUARIOSROL.PropertyNames.IDROL
				ht(IDCLASIFICACIONFAMILIA) = _USUARIOSROL.PropertyNames.IDCLASIFICACIONFAMILIA
				ht(IDTIPOINCIDENCIA) = _USUARIOSROL.PropertyNames.IDTIPOINCIDENCIA

			End If
			
			Return CType(ht(columnName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing		 
	End Class
	#End Region	
	
	#Region "PropertyNames"
	Public class PropertyNames
		
        Public Const IDUSR As String = "IDUSR"
        Public Const IDROL As String = "IDROL"
        Public Const IDCLASIFICACIONFAMILIA As String = "IDCLASIFICACIONFAMILIA"
        Public Const IDTIPOINCIDENCIA As String = "IDTIPOINCIDENCIA"

		Shared Public Function ToColumnName(ByVal propertyName As String) As String

			If ht Is Nothing Then
			
				ht = new Hashtable
				
				ht(IDUSR) = _USUARIOSROL.ColumnNames.IDUSR
				ht(IDROL) = _USUARIOSROL.ColumnNames.IDROL
				ht(IDCLASIFICACIONFAMILIA) = _USUARIOSROL.ColumnNames.IDCLASIFICACIONFAMILIA
				ht(IDTIPOINCIDENCIA) = _USUARIOSROL.ColumnNames.IDTIPOINCIDENCIA

			End If
			
			Return CType(ht(propertyName), String)
			
		End Function
		
		Shared Private ht  As Hashtable = Nothing
		
	End Class
	#End Region	
	
	#Region "StringPropertyNames"
	Public class StringPropertyNames
		
        Public Const IDUSR As String = "s_IDUSR"
        Public Const IDROL As String = "s_IDROL"
        Public Const IDCLASIFICACIONFAMILIA As String = "s_IDCLASIFICACIONFAMILIA"
        Public Const IDTIPOINCIDENCIA As String = "s_IDTIPOINCIDENCIA"

	End Class
	#End Region		
	
	#Region "Properties" 
		Public Overridable Property IDUSR As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IDUSR)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IDUSR, Value)
			End Set
		End Property

		Public Overridable Property IDROL As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IDROL)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IDROL, Value)
			End Set
		End Property

		Public Overridable Property IDCLASIFICACIONFAMILIA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IDCLASIFICACIONFAMILIA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IDCLASIFICACIONFAMILIA, Value)
			End Set
		End Property

		Public Overridable Property IDTIPOINCIDENCIA As Decimal
			Get
				Return MyBase.GetDecimal(ColumnNames.IDTIPOINCIDENCIA)
			End Get
			Set(ByVal Value As Decimal)
				MyBase.SetDecimal(ColumnNames.IDTIPOINCIDENCIA, Value)
			End Set
		End Property


	#End Region  
	
	#Region "String Properties" 

		Public Overridable Property s_IDUSR As String
			Get
				If Me.IsColumnNull(ColumnNames.IDUSR) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IDUSR)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDUSR)
				Else
					Me.IDUSR = MyBase.SetDecimalAsString(ColumnNames.IDUSR, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDROL As String
			Get
				If Me.IsColumnNull(ColumnNames.IDROL) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IDROL)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDROL)
				Else
					Me.IDROL = MyBase.SetDecimalAsString(ColumnNames.IDROL, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDCLASIFICACIONFAMILIA As String
			Get
				If Me.IsColumnNull(ColumnNames.IDCLASIFICACIONFAMILIA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IDCLASIFICACIONFAMILIA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDCLASIFICACIONFAMILIA)
				Else
					Me.IDCLASIFICACIONFAMILIA = MyBase.SetDecimalAsString(ColumnNames.IDCLASIFICACIONFAMILIA, Value)
				End If
			End Set
		End Property

		Public Overridable Property s_IDTIPOINCIDENCIA As String
			Get
				If Me.IsColumnNull(ColumnNames.IDTIPOINCIDENCIA) Then
					Return String.Empty
				Else
					Return MyBase.GetDecimalAsString(ColumnNames.IDTIPOINCIDENCIA)
				End If
			End Get
			Set(ByVal Value As String)
				If String.Empty = value Then
					Me.SetColumnNull(ColumnNames.IDTIPOINCIDENCIA)
				Else
					Me.IDTIPOINCIDENCIA = MyBase.SetDecimalAsString(ColumnNames.IDTIPOINCIDENCIA, Value)
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
		
	
			Public ReadOnly Property IDUSR() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDUSR, Parameters.IDUSR)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IDROL() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDROL, Parameters.IDROL)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IDCLASIFICACIONFAMILIA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDCLASIFICACIONFAMILIA, Parameters.IDCLASIFICACIONFAMILIA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property

			Public ReadOnly Property IDTIPOINCIDENCIA() As WhereParameter
				Get
					Dim where As WhereParameter = New WhereParameter(ColumnNames.IDTIPOINCIDENCIA, Parameters.IDTIPOINCIDENCIA)
					Me._clause._entity.Query.AddWhereParemeter(where)
					Return where
				End Get
			End Property


			Private _clause as WhereClause
		End Class
		#End Region	

		Public ReadOnly Property IDUSR() As WhereParameter 
			Get
				If _IDUSR_W Is Nothing Then
					_IDUSR_W = TearOff.IDUSR
				End If
				Return _IDUSR_W
			End Get
		End Property

		Public ReadOnly Property IDROL() As WhereParameter 
			Get
				If _IDROL_W Is Nothing Then
					_IDROL_W = TearOff.IDROL
				End If
				Return _IDROL_W
			End Get
		End Property

		Public ReadOnly Property IDCLASIFICACIONFAMILIA() As WhereParameter 
			Get
				If _IDCLASIFICACIONFAMILIA_W Is Nothing Then
					_IDCLASIFICACIONFAMILIA_W = TearOff.IDCLASIFICACIONFAMILIA
				End If
				Return _IDCLASIFICACIONFAMILIA_W
			End Get
		End Property

		Public ReadOnly Property IDTIPOINCIDENCIA() As WhereParameter 
			Get
				If _IDTIPOINCIDENCIA_W Is Nothing Then
					_IDTIPOINCIDENCIA_W = TearOff.IDTIPOINCIDENCIA
				End If
				Return _IDTIPOINCIDENCIA_W
			End Get
		End Property

		Private _IDUSR_W As WhereParameter = Nothing
		Private _IDROL_W As WhereParameter = Nothing
		Private _IDCLASIFICACIONFAMILIA_W As WhereParameter = Nothing
		Private _IDTIPOINCIDENCIA_W As WhereParameter = Nothing

			Public Sub WhereClauseReset()

			_IDUSR_W = Nothing
			_IDROL_W = Nothing
			_IDCLASIFICACIONFAMILIA_W = Nothing
			_IDTIPOINCIDENCIA_W = Nothing
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
		
	
		Public ReadOnly Property IDUSR() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDUSR, Parameters.IDUSR)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IDROL() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDROL, Parameters.IDROL)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IDCLASIFICACIONFAMILIA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDCLASIFICACIONFAMILIA, Parameters.IDCLASIFICACIONFAMILIA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property

		Public ReadOnly Property IDTIPOINCIDENCIA() As AggregateParameter
			Get
				Dim where As AggregateParameter = New AggregateParameter(ColumnNames.IDTIPOINCIDENCIA, Parameters.IDTIPOINCIDENCIA)
				Me._clause._entity.Query.AddAggregateParameter(where)
				Return where
			End Get
		End Property


			Private _clause as AggregateClause
		End Class
		#End Region	

		Public ReadOnly Property IDUSR() As AggregateParameter 
			Get
				If _IDUSR_W Is Nothing Then
					_IDUSR_W = TearOff.IDUSR
				End If
				Return _IDUSR_W
			End Get
		End Property

		Public ReadOnly Property IDROL() As AggregateParameter 
			Get
				If _IDROL_W Is Nothing Then
					_IDROL_W = TearOff.IDROL
				End If
				Return _IDROL_W
			End Get
		End Property

		Public ReadOnly Property IDCLASIFICACIONFAMILIA() As AggregateParameter 
			Get
				If _IDCLASIFICACIONFAMILIA_W Is Nothing Then
					_IDCLASIFICACIONFAMILIA_W = TearOff.IDCLASIFICACIONFAMILIA
				End If
				Return _IDCLASIFICACIONFAMILIA_W
			End Get
		End Property

		Public ReadOnly Property IDTIPOINCIDENCIA() As AggregateParameter 
			Get
				If _IDTIPOINCIDENCIA_W Is Nothing Then
					_IDTIPOINCIDENCIA_W = TearOff.IDTIPOINCIDENCIA
				End If
				Return _IDTIPOINCIDENCIA_W
			End Get
		End Property

		Private _IDUSR_W As AggregateParameter = Nothing
		Private _IDROL_W As AggregateParameter = Nothing
		Private _IDCLASIFICACIONFAMILIA_W As AggregateParameter = Nothing
		Private _IDTIPOINCIDENCIA_W As AggregateParameter = Nothing

		Public Sub AggregateClauseReset()

		_IDUSR_W = Nothing
		_IDROL_W = Nothing
		_IDCLASIFICACIONFAMILIA_W = Nothing
		_IDTIPOINCIDENCIA_W = Nothing
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
		cmd.CommandText = Me.SchemaStoredProcedure + "PI_USUARIOSROL" 
	    
		CreateParameters(cmd)
		
		    
		Return cmd 

  	End Function
	
	Protected Overrides Function GetUpdateCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PU_USUARIOSROL" 
		
		CreateParameters(cmd) 
		    
		Return cmd
	
	End Function	
	
	Protected Overrides Function GetDeleteCommand() As IDbCommand
	
		Dim cmd As OracleCommand = New OracleCommand
		cmd.CommandType = CommandType.StoredProcedure    
		cmd.CommandText = Me.SchemaStoredProcedure + "PD_USUARIOSROL" 
		
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.IDCLASIFICACIONFAMILIA)
		p.SourceColumn = ColumnNames.IDCLASIFICACIONFAMILIA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDTIPOINCIDENCIA)
		p.SourceColumn = ColumnNames.IDTIPOINCIDENCIA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDUSR)
		p.SourceColumn = ColumnNames.IDUSR
		p.SourceVersion = DataRowVersion.Current

  
		Return cmd
		
	End Function	
	
	Private Sub CreateParameters(ByVal cmd As OracleCommand)
	
		Dim p As OracleParameter
		p = cmd.Parameters.Add(Parameters.IDUSR)
		p.SourceColumn = ColumnNames.IDUSR
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDROL)
		p.SourceColumn = ColumnNames.IDROL
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDCLASIFICACIONFAMILIA)
		p.SourceColumn = ColumnNames.IDCLASIFICACIONFAMILIA
		p.SourceVersion = DataRowVersion.Current

		p = cmd.Parameters.Add(Parameters.IDTIPOINCIDENCIA)
		p.SourceColumn = ColumnNames.IDTIPOINCIDENCIA
		p.SourceVersion = DataRowVersion.Current


	End Sub	

End Class

End NameSpace

