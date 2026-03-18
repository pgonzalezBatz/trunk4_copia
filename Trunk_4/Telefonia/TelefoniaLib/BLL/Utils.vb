Namespace BLL

    Public Class Utils

#Region "Traducir Termino"

        ''' <summary>
        ''' Obtiene todos los usuarios que no esten dados de baja y que tengan numero de trabajador
        ''' </summary>
        ''' <param name="idPlanta">Identificador de la planta</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function getUsuarios(ByVal idPlanta As Integer) As List(Of SABLib.ELL.Usuario)
            Try
                '    Dim userDAL As New Generico.DAL.SAB.USUARIOS
                '    Dim usuario As Generico.ELL.Usuario
                '    Dim listUser As New List(Of Generico.ELL.Usuario)
                '    userDAL.Where.CODPERSONA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNotNull
                '    userDAL.Query.AddConjunction(AccesoAutomaticoBD.WhereParameter.Conj.AND_)
                '    userDAL.Query.OpenParenthesis()
                '    userDAL.Where.FECHABAJA.Value = DateTime.Now
                '    userDAL.Where.FECHABAJA.Operator = AccesoAutomaticoBD.WhereParameter.Operand.GreaterThan
                '    Dim wp As AccesoAutomaticoBD.WhereParameter = userDAL.Where.TearOff.FECHABAJA
                '    wp.Conjuction = AccesoAutomaticoBD.WhereParameter.Conj.OR_
                '    wp.Operator = AccesoAutomaticoBD.WhereParameter.Operand.IsNull
                '    userDAL.Query.CloseParenthesis()

                '    userDAL.Query.Load()
                '    Do
                '        usuario = New Generico.ELL.Usuario
                '        usuario.Id = userDAL.ID
                '        usuario.Nombre = userDAL.s_NOMBREUSUARIO
                '        usuario.IdEmpresa = userDAL.IDEMPRESAS
                '        usuario.Cultura = userDAL.s_IDCULTURAS
                '        usuario.Email = userDAL.s_EMAIL
                '        usuario.Izena = userDAL.s_IZENA
                '        usuario.CodPersona = userDAL.CODPERSONA
                '        listUser.Add(usuario)
                '    Loop While userDAL.MoveNext()

                '    Return listUser
                Return Nothing
            Catch ex As Exception
                Dim batzEx As New BatzException("errIKSobtenerUsuarios", ex)
                Throw batzEx
            End Try
        End Function


        ''' <summary>
        ''' Obtiene una lista de tiposCulturas con todas las culturas existentes
        ''' </summary>
        ''' <returns></returns>    
        Public Shared Function recuperarCulturas() As List(Of ELL.TipoCultura)
            Try
                Dim listCulturas As New List(Of ELL.TipoCultura)
                Dim oTipoCul As ELL.TipoCultura
                For Each cult As Integer In [Enum].GetValues(GetType(ELL.TipoCultura.Idioma))
                    oTipoCul = New ELL.TipoCultura
                    Select Case cult
                        Case ELL.TipoCultura.Idioma.Español
                            oTipoCul.Cultura = ELL.TipoCultura.CULTURA_ESPAÑOL
                        Case ELL.TipoCultura.Idioma.Euskera
                            oTipoCul.Cultura = ELL.TipoCultura.CULTURA_EUSKARA
                        Case ELL.TipoCultura.Idioma.Ingles
                            oTipoCul.Cultura = ELL.TipoCultura.CULTURA_INGLES
                        Case ELL.TipoCultura.Idioma.Checo
                            oTipoCul.Cultura = ELL.TipoCultura.CULTURA_CHECO
                    End Select

                    listCulturas.Add(oTipoCul)
                Next

                Return listCulturas
            Catch ex As Exception
                Throw New BatzException("errObtenerCulturas", ex)
            End Try
        End Function


        ''' <summary>
        ''' Obtiene una lista de tiposCulturas con todas las culturas existentes
        ''' </summary>
        ''' <returns></returns>    
        Public Shared Function recuperarCulturasLinea() As List(Of ELL.TipoLinea)
            Try
                Dim listCulturas As New List(Of ELL.TipoLinea)
                Dim oTipoCul As ELL.TipoLinea
                For Each cult As Integer In [Enum].GetValues(GetType(ELL.TipoLinea.Idioma))
                    oTipoCul = New ELL.TipoLinea
                    Select Case cult
                        Case ELL.TipoLinea.Idioma.Español
                            oTipoCul.Cultura = ELL.TipoCultura.CULTURA_ESPAÑOL
                        Case ELL.TipoLinea.Idioma.Euskera
                            oTipoCul.Cultura = ELL.TipoCultura.CULTURA_EUSKARA
                        Case ELL.TipoLinea.Idioma.Ingles
                            oTipoCul.Cultura = ELL.TipoCultura.CULTURA_INGLES
                        Case ELL.TipoLinea.Idioma.Checo
                            oTipoCul.Cultura = ELL.TipoCultura.CULTURA_CHECO
                    End Select

                    listCulturas.Add(oTipoCul)
                Next

                Return listCulturas
            Catch ex As Exception
                Throw New BatzException("errObtenerCulturas", ex)
            End Try
        End Function

#End Region

#Region "Control de nulos"

        Public Shared Function stringNull(ByVal o As Object) As String
            Dim strResul As String = String.Empty
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                strResul = o.ToString()
            End If
            Return strResul
        End Function

        Public Shared Function integerNull(ByVal o As Object) As Integer
            Dim intResul As Integer = Integer.MinValue
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                intResul = CInt(o.ToString())
            End If
            Return intResul
        End Function

        Public Shared Function dateTimeNull(ByVal o As Object) As DateTime
            Dim dtResul As DateTime = DateTime.MinValue
            If Not (o Is Nothing Or o Is DBNull.Value) Then
                dtResul = CType(o.ToString(), DateTime)
            End If
            Return dtResul
        End Function

        ''' <summary>
        ''' Devuelve el string si no es vacio y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlStringNull(ByVal o As String) As String
            Dim strResul As String = Nothing
            If (o <> String.Empty) Then
                strResul = o
            End If
            Return strResul
        End Function

        ''' <summary>
        ''' Devuelve el integer si no es Integer.MinValue y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlIntegerNull(ByVal o As Integer) As Nullable(Of Integer)
            Dim intResul As Nullable(Of Integer) = Nothing
            If (o <> Integer.MinValue) Then
                intResul = o
            End If
            If (intResul.HasValue) Then
                Return intResul.Value
            Else
                Return intResul
            End If
        End Function

        ''' <summary>
        ''' Devuelve la fecha si no es DateTime.MinValue y nulo en caso contrario
        ''' </summary>
        ''' <param name="o"></param>
        Public Shared Function sqlDateTimeNull(ByVal o As DateTime) As Nullable(Of DateTime)
            Dim dtResul As Nullable(Of DateTime) = Nothing
            If (o <> DateTime.MinValue) Then
                dtResul = o
            End If
            Return dtResul
        End Function

#End Region

#Region "Pooling"

        ''' <summary>
        ''' Realiza unas consultas a la base de datos ya que el pooling se quedaba colgado y la primera vez del dia, daba fallo
        ''' </summary>
        ''' <remarks></remarks>
        Public Shared Sub ConsultaPooling()
            Try
                Memcached.OracleDirectAccess.Seleccionar("select sysdate from dual", Configuration.ConfigurationManager.ConnectionStrings("TELEFONIALIVE").ConnectionString)
            Catch
            End Try
            Try
                Memcached.OracleDirectAccess.Seleccionar("select sysdate from dual", Configuration.ConfigurationManager.ConnectionStrings("SABLIVE").ConnectionString)
            Catch
            End Try
        End Sub

#End Region

#Region "Acceso Zoiper"

        ''' <summary>
        ''' función que nos devuelve true si el usuario está dado de alta en la BBDD de asterisk
        ''' Para poder probarlo, tiene que estar instalador el conector mysql
        ''' </summary>
        ''' <param name="email"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Shared Function usuarioZoiper(ByVal email As String) As Boolean
            Dim conexionAsterisk As ADODB.Connection, rs As ADODB.Recordset
            rs = Nothing
            Try
                conexionAsterisk = New ADODB.Connection
                conexionAsterisk.Open(Configuration.ConfigurationManager.ConnectionStrings("ASTERISK").ConnectionString)

                ' Nuevo recordset  
                rs = New ADODB.Recordset

                With rs
                    .CursorLocation = ADODB.CursorLocationEnum.adUseClient
                    .CursorType = ADODB.CursorTypeEnum.adOpenStatic
                    .LockType = ADODB.LockTypeEnum.adLockOptimistic
                End With

                rs.Open("select * from iax_buddies where mailbox='" & email & "'", conexionAsterisk, , )

                If (Not rs.EOF) Then
                    Return True
                Else
                    Return False
                End If
            Catch ex As Exception
                Return False
            Finally
                If (rs IsNot Nothing) Then
                    rs.ActiveConnection = Nothing
                    If (rs.State <> 0) Then rs.Close()
                End If
            End Try
        End Function

#End Region

    End Class

End Namespace
