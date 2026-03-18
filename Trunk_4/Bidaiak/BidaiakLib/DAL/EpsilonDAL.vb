Imports System.Data.SqlClient

Namespace DAL

    Public Class EpsilonDAL

#Region "Variables"

        Private cn As String
        Private cnNavsilon As String
        Private parameter As SqlParameter
        Private mIdEmpresa As String

        ''' <summary>
        ''' Constructor con la empresa
        ''' </summary>
        ''' <param name="idEmpresaEpsilon">Id de la empresa de Epsilon</param>  
        ''' <param name="conString">Su string de conexion</param>
        Sub New(ByVal idEmpresaEpsilon As String, ByVal conString As String)
            mIdEmpresa = idEmpresaEpsilon
            cn = conString
            If (Configuration.ConfigurationManager.AppSettings("CurrentStatus") = "Debug") Then
                cnNavsilon = Configuration.ConfigurationManager.ConnectionStrings("NAVSILONTEST").ConnectionString
            Else
                cnNavsilon = Configuration.ConfigurationManager.ConnectionStrings("NAVSILONLIVE").ConnectionString
            End If
        End Sub

#End Region

        ''' <summary>
        ''' Obtiene la informacion de la persona
        ''' </summary>
        ''' <param name="dni">Dni</param>
        ''' <returns>'0:NIF,1:NOMBRE,2:APELLIDO1,3:APELLIDO3,4:COD_TRA</returns>        
        Public Function GetInfoPersona(ByVal dni As String) As String()
            Try
                Dim query As String = "SELECT DISTINCT P.NIF,P.NOMBRE,P.APELLIDO1,P.APELLIDO2,T.ID_CONVENIO,T.ID_CATEGORIA " _
                                     & "FROM COD_TRA C INNER JOIN PERSONAS P ON C.NIF=P.NIF " _
                                     & "INNER JOIN TRABAJADORES T ON C.ID_TRABAJADOR=T.ID_TRABAJADOR AND C.ID_EMPRESA=T.ID_EMPRESA " _
                                     & "WHERE LTRIM(RTRIM(P.NIF))=@NIF AND T.ID_EMPRESA=@ID_EMPRESA " _
                                     & "AND T.F_INI_SEC=(SELECT MAX(TR.F_INI_SEC) FROM TRABAJADORES TR INNER JOIN COD_TRA CT ON TR.ID_TRABAJADOR=CT.ID_TRABAJADOR AND TR.ID_EMPRESA=CT.ID_EMPRESA WHERE TR.ID_EMPRESA= T.ID_EMPRESA AND CT.NIF=C.NIF)"                
                Dim lParametros As New List(Of SqlParameter)
                lParametros.Add(New SqlParameter("NIF", dni))
                lParametros.Add(New SqlParameter("ID_EMPRESA", mIdEmpresa))
                'Si lo hacia asi, me tardaba 4 segundos. Si le quitaba el tipo, devolvia rapido pero sin resultados
                'parameter = New SqlParameter("NIF", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 14 : parameter.Value = dni : lParametros.Add(parameter)
                'parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)
                Return Memcached.SQLServerDirectAccess.Seleccionar(query, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de una persona", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene informacion del banco del trabajador
        ''' </summary>
        ''' <param name="dni">DNI</param>
        ''' <returns></returns>        
        Function getInfoBancoTrabajador(ByVal dni As String) As String()
            Try
                Dim query As String = "SELECT distinct cod_tra.nif,personas.apellido1,personas.apellido2,personas.nombre,personas.siglas,personas.domicilio,personas.piso,personas.cpostal,poblac.d_poblacion,cta_per.id_banco,cta_per.id_sucursal,cta_per.id_cuenta,cta_per.dc_cta,cta_per.dc_iban,sucursales.swift " _
                                    & "FROM cod_tra,personas,cta_per,provincias,poblac,trabajadores,sucursales " _
                                    & "WHERE cod_tra.nif = personas.nif And cod_tra.nif = cta_per.nif AND cta_per.cl_nomina = '1' AND cta_per.cl_anticipo = '1' AND personas.id_provincia = provincias.id_provincia " _
                                    & "AND personas.id_provincia = poblac.id_provincia AND personas.id_poblacion = poblac.id_poblacion AND cod_tra.id_trabajador=trabajadores.id_trabajador and cta_per.id_banco=sucursales.id_banco and cta_per.id_sucursal=sucursales.id_sucursal " _
                                    & "AND cod_tra.id_empresa=trabajadores.id_empresa and trabajadores.id_empresa=@ID_EMPRESA " _
                                    & "and personas.nif=@DNI " _
                                    & "and trabajadores.f_ini_sec=(select max(tr.f_ini_sec) from trabajadores tr inner join cod_tra ct on tr.id_trabajador=ct.id_trabajador and tr.id_empresa=ct.id_empresa where tr.id_empresa= trabajadores.id_empresa and ct.nif=cod_tra.nif)"

                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("DNI", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 14 : parameter.Value = dni : lParametros.Add(parameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)
                Dim lResul As List(Of String()) = Memcached.SQLServerDirectAccess.Seleccionar(query, cn, lParametros.ToArray)
                If (lResul IsNot Nothing AndAlso lResul.Count = 1) Then
                    Return lResul.First
                Else
                    Return Nothing
                End If
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del trabajador a traves del DNI", ex)
            End Try
            'Dim query As String = "SELECT nif,apellido1,apellido2,nombre,siglas,domicilio,piso,cpostal,d_poblacion,id_banco,id_sucursal,id_cuenta,dc_cta FROM EPSILON WHERE LTRIM(RTRIM(NIF))=:DNI AND (ID_TRABAJADOR=:ID_TRABAJADOR OR ID_TRABAJADOR=:ID_TRABAJADOR_MAS_900000)"
        End Function

        ''' <summary>
        ''' Obtiene la descripcion de la unidad organizativa dada un deparmento
        ''' </summary>
        ''' <param name="codDepto">Codigo del departamento</param>
        ''' <returns></returns>           		
        Public Function GetUnidadOrganizativa(ByVal codDepto As String) As String
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT NO2.D_NIVEL")
                query.AppendLine("FROM ORDEN O INNER JOIN NIV_ORG NO ON O.N4=NO.ID_NIVEL AND O.ID_ORGANIG=NO.ID_ORGANIG")
                query.AppendLine("INNER JOIN NIV_ORG NO2 ON O.N2=NO2.ID_NIVEL AND NO2.ID_ORGANIG=NO.ID_ORGANIG")
                query.AppendLine("INNER JOIN ORGANIG_EMPRESAS OE ON NO2.ID_ORGANIG=OE.ID_ORGANIG")
                query.AppendLine("INNER JOIN PARAMETROS PA ON OE.ID_ORGANIG=PA.ORG_DEFECTO")
                query.AppendLine("WHERE NO.ID_NIVEL=@ID_DEPTO AND PA.ID_EMPRESA=@ID_EMPRESA AND O.NIVEL=3")
                'Si se le pasa 560 y es un string, no encuentra el 00560. Sin embargo si se le pasa 560 como entero si lo encuentra
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_DEPTO", SqlDbType.Int, ParameterDirection.Input) : parameter.Size = 4 : parameter.Value = CInt(codDepto) : lParametros.Add(parameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)
                Return Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la unidad organizativa de un departamento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el listado de los departamentos activos y los trabajadores asociados
        ''' </summary>
        ''' <returns></returns>
        Public Function GetDepartamentosPersonasEpsilon() As List(Of String())
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("select n2.d_nivel as NEGOCIO,n4.d_nivel as DEPARTAMENTO,cast(pt.id_trabajador as integer) as NUMERO_SOCIO, CASE WHEN p.apellido2 is not null THEN (rtrim(p.apellido1) + ' ' + rtrim(p.apellido2) +', ' +p.nombre) ELSE (rtrim(p.apellido1) +', ' +p.nombre) END as TRABAJADOR,p.email")
                query.AppendLine("from orden o, pues_trab pt,niv_org n2,niv_org n4, cod_tra ct, personas p, organig_empresas oe, parametros pa")
                query.AppendLine("where o.id_nivel_hijo=pt.id_nivel and pt.id_organig=o.id_organig")
                query.AppendLine("and pt.id_organig=pa.org_defecto and pt.id_organig=pa.org_defecto and (pt.f_fin_pue is null or pt.f_fin_pue>getDate())")
                query.AppendLine("and n2.id_nivel=o.n2 and n2.id_organig=o.id_organig")
                query.AppendLine("and n4.id_nivel=o.n4 and n4.id_organig=o.id_organig")
                query.AppendLine("and ct.id_trabajador=pt.id_trabajador and pt.id_empresa=ct.id_empresa")
                query.AppendLine("and oe.id_organig = n2.id_organig")
                query.AppendLine("and pa.org_defecto = oe.id_organig")
                query.AppendLine("and pt.id_empresa=@ID_EMPRESA and ct.nif=p.nif")
                query.AppendLine("AND PT.F_INI_PUE=(SELECT MIN(PT2.F_INI_PUE) AS F_INI_PUE FROM PUES_TRAB PT2 WHERE ((PT2.ID_ORGANIG = PT.ID_ORGANIG And PT2.ID_EMPRESA = PT.ID_EMPRESA And PT2.ID_TRABAJADOR = PT.ID_TRABAJADOR) AND (PT2.F_INI_PUE<=GETDATE() AND (PT2.F_FIN_PUE IS NULL OR PT2.F_FIN_PUE>=GETDATE()))))")
                query.AppendLine("order by n2.d_nivel,n4.d_nivel")
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)

                Return Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la estructura de departamentos personas", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el negocio y el departamento de un codigo de departamento
        ''' </summary>
        ''' <param name="depto">Codigo del departamento</param>
        ''' <returns></returns>        
        Function GetNegocioDepartamentoSubcontratados(ByVal depto As String) As String()
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("select niv_org2.d_nivel as NEGOCIO,niv_org.d_nivel as DEPARTAMENTO")
                query.AppendLine("from orden inner join niv_org on orden.id_organig=niv_org.id_organig and orden.id_nivel_hijo=niv_org.id_nivel")
                query.AppendLine("inner join niv_org as niv_org2 on orden.n2=niv_org2.id_nivel and orden.id_organig=niv_org2.id_organig")
                query.AppendLine("inner join organig_empresas oe on niv_org2.id_organig=oe.id_organig")
                query.AppendLine("inner join parametros pa on oe.id_organig=pa.org_defecto")
                query.AppendLine("where niv_org.id_nivel=@ID_DEPTO and pa.id_empresa=@ID_EMPRESA")
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_DEPTO", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = depto : lParametros.Add(parameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)

                Dim info As String() = Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray).FirstOrDefault
                Return info
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el negocio y el departamento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la lista de departamentos activos. Los que tengan fechas futuras no apareceran
        ''' </summary>
        ''' <returns></returns>        
        Function GetDepartamentos() As List(Of String())
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("select id_nivel,d_nivel")
                query.AppendLine("from orden,niv_org,organig_empresas,parametros")
                query.AppendLine("where orden.id_organig = niv_org.id_organig and orden.id_nivel_hijo = niv_org.id_nivel")
                query.AppendLine("and organig_empresas.id_organig=niv_org.id_organig")
                query.AppendLine("and parametros.org_defecto=organig_empresas.id_organig")
                query.AppendLine("and nivel = 3 and f_inhabilitacion is null and parametros.id_empresa=@ID_EMPRESA")
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)

                Return Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los departamentos", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la cuenta de pago para realizar las transferencias bancarias
        ''' </summary>        
        ''' <returns>0:Banco,1:Sucursal,2:Digito Control,3:Cuenta,4:Iban,5:SWIFT o BIC</returns>        
        Function getCuentaPago() As String()
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT ce.id_banco,ce.id_sucursal,ce.dc_cta,ce.id_cuenta,ce.dc_iban,s.swift")
                query.AppendLine("FROM cta_emp ce inner join sucursales s on ce.id_sucursal=s.id_sucursal")
                query.AppendLine("WHERE ce.id_empresa=@ID_EMPRESA and ce.cl_ctaor=1") 'cl_ctaor es la cuenta por defecto que se puede cambiar desde la interfaz de Epsilon                                
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)
                Return Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la cuenta de pago", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion de la empresa
        ''' </summary>
        ''' <returns>0:Nombre empresa, 1:Nif, 2:Tipo calle, 3:Domicilio, 4:Num, 5:CP, 6:Poblacion, 7:Provincia</returns>        
        Function getInfoEmpresa() As String()
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT e.d_empresa,e.nif,e.siglas,e.domicilio,e.num,e.cpostal,po.d_poblacion,pr.d_provincia")
                query.AppendLine("FROM empresas e inner join poblac po on (po.id_provincia=e.id_provincia and po.id_poblacion=e.id_poblacion)")
                query.AppendLine("inner join provincias pr on pr.id_provincia=e.id_provincia")
                query.AppendLine("WHERE e.id_empresa = @ID_EMPRESA")
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)
                Return Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion de la empresa", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene informacion del departamento
        ''' </summary>
        ''' <param name="idDepto">Id del departamento a consultar</param>
        Function getInfoOrdenDepartamento(ByVal idDepto As String) As String()
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT O.N2 AS IDNEGOCIO, NO2.D_NIVEL AS NEGOCIO,O.N3 AS IDORGANIZACION,NO3.D_NIVEL AS ORGANIZACION,O.N4 AS IDDEPTO,NO4.D_NIVEL AS DEPARTAMENTO")
                query.AppendLine("FROM ORDEN O INNER JOIN NIV_ORG NO2 ON O.N2=NO2.ID_NIVEL AND NO2.ID_ORGANIG=O.ID_ORGANIG")
                query.AppendLine("INNER JOIN NIV_ORG NO3 ON O.N3=NO3.ID_NIVEL AND NO3.ID_ORGANIG=O.ID_ORGANIG")
                query.AppendLine("INNER JOIN NIV_ORG NO4 ON O.N4=NO4.ID_NIVEL AND NO4.ID_ORGANIG=O.ID_ORGANIG")
                query.AppendLine("INNER JOIN ORGANIG_EMPRESAS OE ON NO4.ID_ORGANIG=OE.ID_ORGANIG")
                query.AppendLine("INNER JOIN PARAMETROS PA ON OE.ID_ORGANIG=PA.ORG_DEFECTO")
                query.AppendLine("WHERE PA.ID_EMPRESA=@ID_EMPRESA AND O.ID_NIVEL_HIJO=@ID_DPTO")
                Dim lParametros As New List(Of SqlParameter)
                'El departamento lo paso como integer porque si se consulta el 00560 como string o el 560 como numero, devuelve el departamento. Sin embargo, si le pasas 560 como string, no devuelve nada
                parameter = New SqlParameter("ID_DPTO", SqlDbType.Int, ParameterDirection.Input) : parameter.Size = 4 : parameter.Value = CInt(idDepto) : lParametros.Add(parameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)

                Return Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del orden de un departamento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene la informacion del lantegi
        ''' </summary>
        ''' <param name="idDepto">Id del departamento</param>
        ''' <returns></returns>        
        Function getInfoLantegi(ByVal idDepto As String) As String
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT LANTEGI")
                query.AppendLine("FROM dbo.EPSILON_CONTABILIDAD EC INNER JOIN dbo.DEPARTAMENTO D ON EC.ID_DEPARTAMENTO=D.ID")
                query.AppendLine("WHERE EC.ID_EPSILON=@ID_DPTO")
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_DPTO", SqlDbType.Int, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = CInt(idDepto) : lParametros.Add(parameter)
                Return Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of String)(query.ToString, cnNavsilon, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener la informacion del lantegi de un departamento", ex)
            End Try
        End Function

        ''' <summary>
        ''' Comprueba si el trabajador tiene indice o no
        ''' </summary>
        ''' <param name="dni">DNI</param>
        ''' <param name="anno">Año a comprobar</param>
        ''' <param name="mes">Mes a comprobar</param>
        ''' <param name="mesesAtras">Se comprobara tambien en los meses atras especificados</param>
        ''' <returns></returns>        
        Function TieneIndiceBatz(ByVal dni As String, ByVal anno As Integer, ByVal mes As Integer, ByVal mesesAtras As Integer) As Boolean
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT COUNT(T.NIF)")
                query.AppendLine("FROM COD_TRA T INNER JOIN COOP_INDICES I ON T.ID_TRABAJADOR=I.ID_TRABAJADOR AND T.ID_EMPRESA=I.ID_EMPRESA")
                query.AppendLine("WHERE T.NIF=@NIF AND T.ID_EMPRESA=@ID_EMPRESA AND")
                query.AppendLine("( (I.EJERCICIO=@ANNO AND I.ID_MES=@MES)")

                If (mesesAtras > 0) Then
                    Dim originalDate As New DateTime(anno, mes, 1)
                    For index As Integer = 0 To mesesAtras - 1
                        originalDate = originalDate.AddMonths(-1)
                        query.AppendLine(" OR (I.EJERCICIO=" & originalDate.Year & " AND I.ID_MES=" & originalDate.Month & ")")
                    Next
                End If
                query.Append(")")

                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("NIF", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 14 : parameter.Value = dni : lParametros.Add(parameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)
                parameter = New SqlParameter("ANNO", SqlDbType.Int) : parameter.Value = anno : lParametros.Add(parameter)
                parameter = New SqlParameter("MES", SqlDbType.Int) : parameter.Value = mes : lParametros.Add(parameter)

                Return (Memcached.SQLServerDirectAccess.SeleccionarEscalar(Of Integer)(query.ToString, cn, lParametros.ToArray) > 0)
                'Dim lResul As List(Of String()) = Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray)
                'Return (lResul IsNot Nothing AndAlso CInt(lResul.Item(0)(0)) > 0)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al comprobar si un trabajador tiene indice o no", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene los convenios y categorias de una planta
        ''' </summary>
        ''' <returns></returns>        
        Function getConveniosCategorias() As List(Of String())
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT CO.ID_CONVENIO,CO.D_CONVENIO,CA.ID_CATEGORIA,CA.D_CATEGORIA")
                query.AppendLine("FROM CONVENIOS CO INNER JOIN CATEGORIAS CA ON CO.ID_CONVENIO=CA.ID_CONVENIO")
                query.AppendLine("INNER JOIN (SELECT DISTINCT ID_CONVENIO,ID_CATEGORIA FROM TRABAJADORES WHERE ID_EMPRESA=@ID_EMPRESA) C ON CO.ID_CONVENIO=C.ID_CONVENIO AND CA.ID_CATEGORIA=C.ID_CATEGORIA")
                query.AppendLine("ORDER BY CO.D_CONVENIO,CA.D_CATEGORIA")
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_EMPRESA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 5 : parameter.Value = mIdEmpresa : lParametros.Add(parameter)
                Return Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray)
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener los convenios y categorias", ex)
            End Try
        End Function

        ''' <summary>
        ''' Obtiene el convenio y categoria dado
        ''' </summary>
        ''' <param name="idConvenio">Id del convenio</param>
        ''' <param name="idCategoria">Id de la categoria</param>
        ''' <returns></returns>        
        Function getConvenioCategoria(ByVal idConvenio As String, ByVal idCategoria As String) As String()
            Try
                Dim query As New Text.StringBuilder
                query.AppendLine("SELECT CO.D_CONVENIO,CA.D_CATEGORIA")
                query.AppendLine("FROM CONVENIOS CO INNER JOIN CATEGORIAS CA ON CO.ID_CONVENIO=CA.ID_CONVENIO")
                query.AppendLine("WHERE CO.ID_CONVENIO=@ID_CONVENIO AND CA.ID_CATEGORIA=@ID_CATEGORIA")
                Dim lParametros As New List(Of SqlParameter)
                parameter = New SqlParameter("ID_CONVENIO", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 3 : parameter.Value = idConvenio : lParametros.Add(parameter)
                parameter = New SqlParameter("ID_CATEGORIA", SqlDbType.Char, ParameterDirection.Input) : parameter.Size = 3 : parameter.Value = idCategoria : lParametros.Add(parameter)
                Return Memcached.SQLServerDirectAccess.Seleccionar(query.ToString, cn, lParametros.ToArray).FirstOrDefault
            Catch ex As Exception
                Throw New BidaiakLib.BatzException("Error al obtener el convenio y categoria", ex)
            End Try
        End Function

    End Class

End Namespace