Imports System.Web.Mvc
Imports Newtonsoft

Namespace Controllers
    Public Class DefaultController
        Inherits Controller


        Dim DAL As DataAccessLayer = New DataAccessLayer()


        '<CustomAuthorize>
        '<SimpleRoleProvider(Roles.mando)>
        Function Index() As ActionResult

            Return View()
        End Function

#Region "getters"
        Function GetFamilies() As ActionResult
            Return View(DAL.GetFamilyAll())
        End Function

        Function GetSubFamilies() As ActionResult
            Return View(DAL.GetSubFamilyAll())
        End Function

        Function GetElements() As ActionResult
            'Return View(DAL.GetDataAll())
            Return View(DAL.GetFullDataWithoutNulls())
        End Function

        Function GetComodities() As ActionResult
            Return View(DAL.GetComodityAll())
        End Function
#End Region

#Region "create"


        Function AddCriticidad() As JsonResult
            Dim resultado As String
            resultado = "prueba"

            Dim name As String
            Dim desc As String

            name = Request.Form("campo1")
            desc = Request.Form("campo2")
            Dim oracleDAL As New OracleDataAccess
            oracleDAL.AddCriticidad(name, desc)

            Return Json(resultado, JsonRequestBehavior.AllowGet)


            'Dim Criticidades = oracleDAL.getCriticidades()
            'Return View("Criticidades", Criticidades)
        End Function


        Function AddValores() As JsonResult
            Dim textoJason As String
            Dim DAL = New DataAccessLayer
            Dim resultado As String
            resultado = "prueba"
            textoJason = DAL.JsonSerializer(resultado)

            '   Return Json(resultado, JsonRequestBehavior.AllowGet)





            Dim val1 As String = Request.Form("val1")
            Dim val2 As String = Request.Form("val2")
            Dim campo0 As String = Request.Form("campo0")

            Dim campo1 As String = Request.Form("campo1")
            Dim campo2 As String = Request.Form("campo2")
            Dim campo3 As String = Request.Form("campo3")
            Dim campo4 As String = Request.Form("campo4")
            Dim campo5 As String = Request.Form("campo5")
            Dim campo6 As String = Request.Form("campo6")
            Dim campo7 As String = Request.Form("campo7")
            Dim campo8 As String = Request.Form("campo8")
            Dim campo9 As String = Request.Form("campo9")



            Dim oracleDAL As New OracleDataAccess

            resultado = oracleDAL.AddValores(val1, val2, campo0, campo1, campo2, campo3, campo4, campo5, campo6, campo7, campo8, campo9)
            'Dim Criticidades = oracleDAL.getCriticidades()
            'Return View("Criticidades2", Criticidades)

            'Dim listaType2 As List(Of Criticidad)


            'textoJason = DAL.JsonSerializer(resultado)

            'tempòral    Return Json(textoJason, JsonRequestBehavior.AllowGet)
            Return Json(resultado, JsonRequestBehavior.AllowGet)
            'Return Nothing si quiero que vaya por error en ajax

        End Function



        Function CreateFamily() As ActionResult
            Return View()
        End Function

        Function CreateSubFamily() As ActionResult
            Return View()
        End Function

        'Function CreateFullElement() As ActionResult
        '    Return View()
        'End Function

        Function CreateElementFullData() As ActionResult
            Return View()
        End Function

        Function CreateElement() As ActionResult
            Return View()
        End Function

        Function CreateComodity() As ActionResult
            Return View()
        End Function
#End Region

#Region "details"
        Function DetailsComodity(ByVal id As String) As ActionResult
            Return View(DAL.GetComodityByCode(id))
        End Function

        Function DetailsFamily(ByVal id As String) As ActionResult
            Return View(DAL.GetFamilyByCode(id))
        End Function

        Function DetailsSubfamily(ByVal id As String) As ActionResult
            Return View(DAL.GetSubFamilyByCode(id))
        End Function

        Function DetailsElement(ByVal id As String) As ActionResult
            Return View(DAL.GetFullElementByCode(id))
        End Function
#End Region

#Region "edit"
        Function EditComodity(ByVal id As String) As ActionResult
            Return View(DAL.GetComodityByCode(id))
        End Function

        Function EditFamily(ByVal id As String) As ActionResult
            Return View(DAL.GetFamilyByCode(id))
        End Function

        Function EditSubFamily(ByVal id As String) As ActionResult
            Return View(DAL.GetSubFamilyByCode(id))
        End Function

        Function EditElement(ByVal id As String, ByVal name As String) As ActionResult
            'Dim currentElement = DAL.GetElementByCode(id)
            Dim currentElement
            If id IsNot Nothing Then
                currentElement = DAL.GetFullElementByCode(id)
            Else
                currentElement = DAL.GetFullElementByName(name)
            End If
            Return View(currentElement)
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="id"></param>
        ''' <returns></returns>
        Function EditFullData(ByVal id As String, ByVal name As String) As ActionResult
            Dim myModel = DAL.GetFullData(id, name)
            Return View(myModel)
        End Function

        Function EditFullData2(ByVal id As String, ByVal name As String) As ActionResult
            Dim myModel = DAL.GetFullData(id, name)
            Return View(myModel)
        End Function

        Function UnAssign(ByVal id As String) As ActionResult
            DAL.UnAssign(id)
            Return View("~/Views/Home/Index2.vbhtml", DAL.GetFullData())
        End Function


        Function Homologar(ByVal id As String) As ActionResult
            DAL.Homologar(id)
            Return View("~/Views/Home/ValoresPlanta.vbhtml", DAL.GetFullData())
        End Function

#End Region

#Region "relative getters"
        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="newcode"></param>
        ''' <returns></returns>
        Function GetDropdownDataFromSubfamilyCode(ByVal newcode As String) As JsonResult
            'Dim e As New FullElement
            'e.Id = newcode
            'e.Name = "name"
            'Dim Subfamilia = DAL.GetSubFamilyByCode(newcode)
            'e.ParentId = Subfamilia.Id
            'e.Parent = Subfamilia.Name
            'Dim Family = DAL.GetFamilyFromSubfamilyCode(newcode)
            'e.Grandparent = Family.Name
            'e.GrandparentId = Family.Id
            'Dim Comodity = DAL.GetComodityFromFamilyCode(Family.Id)
            'e.Grandgrandparent = Comodity.Name
            'e.GrandgrandparentId = Comodity.Id
            'Return New JsonResult With {.Data = e}
            Dim e As New DataFull
            e.Code = newcode
            e.Name = "name"
            Dim Subfamilia = DAL.GetSubFamilyByCode(newcode)
            e.Subfamily = Subfamilia.Name
            e.SubfamilyId = Subfamilia.Id
            Dim Family = DAL.GetFamilyFromSubfamilyCode(newcode)
            e.Family = Family.Name
            e.FamilyId = Family.Id
            Dim Comodity = DAL.GetComodityFromFamilyCode(Family.Id)
            e.Comodity = Comodity.Name
            e.ComodityId = Comodity.Id
            Return New JsonResult With {.Data = e}
        End Function
        Function GetDropdownDataFromElementCode(ByVal newcode As String) As JsonResult
            Dim e As New DataFull
            Dim Elemento = DAL.GetFullElementByCode(newcode)
            'Dim Subfamilia = DAL.GetSubFamilyByCode(newcode)
            e.Name = Elemento.Name
            e.Code = newcode

            e.Subfamily = Elemento.Parent
            e.SubfamilyId = Elemento.ParentId

            e.Family = Elemento.Grandparent
            e.FamilyId = Elemento.GrandparentId

            e.Comodity = Elemento.Grandgrandparent
            e.ComodityId = Elemento.GrandgrandparentId

            Return New JsonResult With {.Data = e}
        End Function
        Function GetDropdownDataFromElementName(ByVal newname As String) As JsonResult
            Dim e As New DataFull
            'Dim myElement = DAL.GetFullElementByName(newname)
            Dim myElement = DAL.GetFullDataByName(newname)
            'Dim Subfamilia = DAL.GetSubFamilyByCode(newcode)
            'e.Name = Elemento.Name
            'e.Code = newcode

            'e.Subfamily = Elemento.Parent
            'e.SubfamilyId = Elemento.ParentId

            'e.Family = Elemento.Grandparent
            'e.FamilyId = Elemento.GrandparentId

            'e.Comodity = Elemento.Grandgrandparent
            'e.ComodityId = Elemento.GrandgrandparentId

            'Return New JsonResult With {.Data = e}
            Return New JsonResult With {.Data = myElement}
        End Function


        Function GetDropdownDataFullFromSubfamilyCode(ByVal newcode As String) As JsonResult
            Dim e As New DataFull
            e.Code = newcode
            e.Name = "name"
            Dim Subfamilia = DAL.GetSubFamilyByCode(newcode)
            e.SubfamilyId = Subfamilia.Id
            e.Subfamily = Subfamilia.Name
            Dim Family = DAL.GetFamilyFromSubfamilyCode(Subfamilia.Id)
            e.Family = Family.Name
            e.FamilyId = Family.Id
            Dim Comodity = DAL.GetComodityFromFamilyCode(Family.Id)
            e.Comodity = Comodity.Name
            e.ComodityId = Comodity.Id
            Return New JsonResult With {.Data = e}
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="familyName"></param>
        ''' <returns></returns>
        Function GetSubfamiliesFromFamilyName(ByVal familyName As String) As JsonResult
            Dim subfamilias = DAL.GetSubfamiliesFromFamilyName(familyName)
            Return New JsonResult With {.Data = subfamilias}
        End Function

        Function GetFamiliesFromComodityName(ByVal desc As String) As JsonResult
            Dim familias = DAL.GetFamiliesFromComodityName(desc)
            Return New JsonResult With {.Data = familias}
        End Function

        Function GetFamiliesFromComodityCode(ByVal newcode As String) As JsonResult
            Dim familias = DAL.GetFamiliesFromComodityCode(newcode)
            Return New JsonResult With {.Data = familias}
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="newcode"></param>
        ''' <returns></returns>
        Function GetDataFromComodityCode(ByVal newcode As String) As JsonResult
            Dim familias
            If newcode.Trim.Equals("") Then
                familias = DAL.GetFamilyAll()
            Else
                familias = DAL.GetFamiliesFromComodityCode(newcode)
            End If
            Dim subfamilias As New List(Of SubFamily)
            For Each familia In familias
                subfamilias.AddRange(DAL.GetSubfamiliesFromFamilyName(familia.Name))
            Next
            Dim data As New Data
            data.SubfamilyList = subfamilias
            data.FamilyList = familias
            Return New JsonResult With {.Data = data}
        End Function

        Function GetDataFromFamilyCode(ByVal newcode As String) As JsonResult
            Dim familyName
            Dim comodity
            Dim subfamilias
            If newcode.Trim.Equals("") Then
                familyName = "SELECT ONE:"
                comodity = "SELECT ONE:"
                subfamilias = "SELECT ONE:"
            Else
                familyName = DAL.GetFamilyByCode(newcode).Name
                comodity = DAL.GetComodityFromFamilyCode(newcode)
                subfamilias = DAL.GetSubfamiliesFromFamilyCode(newcode)
            End If
            Dim data As New Data
            data.SubfamilyList = subfamilias
            data.FamilyList = New List(Of Family)
            data.FamilyList.Add(New Family With {.Id = newcode, .Name = familyName})
            data.ComodityList = New List(Of Comodity)
            data.ComodityList.Add(comodity)
            Return New JsonResult With {.Data = data}
        End Function

        Function GetDataFromSubfamilyCode(ByVal newcode As String) As JsonResult
            Dim subfamilyName = DAL.GetSubFamilyByCode(newcode).Name
            Dim family = DAL.GetFamilyFromSubfamilyCode(newcode)
            Dim comodity = DAL.GetComodityFromFamilyCode(family.Id)
            Dim data As New Data
            data.SubfamilyList = New List(Of SubFamily)
            data.SubfamilyList.Add(New SubFamily With {.Id = newcode, .Name = subfamilyName})
            data.FamilyList = New List(Of Family)
            data.FamilyList.Add(family)
            data.ComodityList = New List(Of Comodity)
            data.ComodityList.Add(comodity)
            Return New JsonResult With {.Data = data}
        End Function

        Function GetComodityFromFamilyCode(ByVal newCode As String) As JsonResult
            Dim comodity = DAL.GetComodityFromFamilyCode(newCode)
            Return New JsonResult With {.Data = comodity}
        End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="desc"></param>
        ''' <returns></returns>
        Function GetComodityFromFamilyName(ByVal desc As String) As JsonResult
            Dim comodity = DAL.GetComodityFromFamilyName(desc)
            Return New JsonResult With {.Data = comodity}
        End Function

#End Region

#Region "updates (post)"
        <HttpPost()>
        Function SubmitElement(ByVal e As FullElement) As ActionResult
            Dim oldName
            If (ModelState.IsValid) Then
                oldName = Request.Form("oldName")
                If e.Id Is Nothing Then
                    ''''unassigned
                    If Not oldName.Equals(e.Name) Then
                        If Not DAL.isNameAvailable(e.Name, New Element) Then
                            ModelState.AddModelError(String.Empty, "The name of the element is already in use")
                            Return View("EditElement", DAL.GetFullElementByCode(e.Id))
                        End If
                    End If
                    DAL.saveUnassignedElementOracle(e, oldName) 'oracle NO dentro
                    DAL.saveUnassignedElement(e, oldName) 'oracle NO dentro
                    Return View("DetailsElement", DAL.GetFullElementByName(e.Name))
                ElseIf DAL.isNameAvailableEditMode(e.Id, e.Name, New FullElement) Then
                    DAL.saveFullElement(e) 'oracle dentro
                    Return View("DetailsElement", DAL.GetFullElementByCode(e.Id))
                Else
                    ModelState.AddModelError(String.Empty, "The name of the element is already in use")
                    Return View("EditElement", DAL.GetFullElementByCode(e.Id))
                End If
            Else
                Return View("EditElement")
            End If
        End Function


        <HttpPost()>
        Function CreateNewElement(ByVal e As Element) As ActionResult
            If (ModelState.IsValid) Then
                If DAL.isNameAvailable(e.Name, New Element) Then
                    Dim newId = DAL.getNextCodeFor(New Element)
                    e.Id = newId
                    DAL.createNewElementOracle(e)
                    DAL.createNewElement(e)

                    Return View("DetailsElement", DAL.GetFullElementByCode(e.Id))
                Else
                    ModelState.AddModelError(String.Empty, "The name of the element is already in use")
                    Return View("CreateElement")
                End If
            Else
                Return View("CreateElement")
            End If
        End Function

        '<HttpPost()>
        'Function SubmitFullElement(ByVal e As FullElement) As ActionResult
        '    If (ModelState.IsValid) Then
        '        DAL.saveFullElement(e)
        '        Return View("DetailsElement", e)
        '    Else
        '        Return View("EditFullElement")
        '    End If
        'End Function

        ''' <summary>
        ''' 
        ''' </summary>
        ''' <param name="e"></param>
        ''' <returns></returns>
        <HttpPost()>
        Function SubmitFullData(ByVal e As DataFull) As ActionResult
            'Dim isNew = Request.Form("status").Equals("1")
            'Dim newname = Request.Form("Name")
            'Dim newname1 = Request.Form("Name1")
            Dim newname = Request.Form("newElementName")
            e.Code = e.GMCode
            'e.Name = Request.Form("newElementName")
            'e.Name = e.GMName
            e.Name = newname
            'If isNew Then
            '    If (ModelState.IsValid AndAlso e.Name IsNot Nothing AndAlso e.ComodityId IsNot Nothing AndAlso e.FamilyId IsNot Nothing AndAlso e.SubfamilyId IsNot Nothing) Then
            '        If DAL.isNameAvailable(e.Name, New Element) Then
            '            DAL.saveFullData(e)
            '        Else
            '            ModelState.AddModelError(String.Empty, "The name of the element is already in use")
            '            'Return View("EditFullData", DAL.GetFullData(e.GMCode))
            '            Return View("EditFullData", e)
            '        End If
            '        Return View("DetailsFullData", e)
            '    Else
            '        ModelState.AddModelError(String.Empty, "Fill all the data")
            '        Return View("EditFullData", e)
            '    End If
            'Else
            If (ModelState.IsValid AndAlso e.Name IsNot Nothing AndAlso e.ComodityId IsNot Nothing AndAlso e.FamilyId IsNot Nothing AndAlso e.SubfamilyId IsNot Nothing) Then
                    'If DAL.isNameAvailable(e.Name, New Element) Then
                    DAL.assignFullData(e)
                'Else
                '    ModelState.AddModelError(String.Empty, "The name of the element is already in use")
                '    'Return View("EditFullData", DAL.GetFullData(e.GMCode))
                '    Return View("EditFullData", e)
                'End If
                e.Subfamily = DAL.GetSubFamilyByCode(e.SubfamilyId).Name
                e.Family = DAL.GetFamilyByCode(e.FamilyId).Name
                e.Comodity = DAL.GetComodityByCode(e.ComodityId).Name

                Return View("DetailsFullData", e)
                Else
                    ModelState.AddModelError(String.Empty, "Fill all the data")
                    Return View("EditFullData", e)
                End If
            'End If
        End Function

        <HttpPost()>
        Function CreateElementFullData(ByVal e As DataFull) As ActionResult
            If (ModelState.IsValid AndAlso e.Name IsNot Nothing AndAlso e.ComodityId IsNot Nothing AndAlso e.FamilyId IsNot Nothing AndAlso e.SubfamilyId IsNot Nothing) Then
                If DAL.isNameAvailable(e.Name, New Element) Then
                    'DAL.CreateElementFullData(e)
                    DAL.saveFullData(e) 'incluido oracle
                Else
                    ModelState.AddModelError(String.Empty, "The name of the element is already in use")
                    'Return View("EditFullData", DAL.GetFullData(e.GMCode))
                    Return View("CreateElementFullData", e)
                End If
                e.Subfamily = DAL.GetSubFamilyByCode(e.SubfamilyId).Name
                e.Family = DAL.GetFamilyByCode(e.FamilyId).Name
                e.Comodity = DAL.GetComodityByCode(e.ComodityId).Name
                Return View("DetailsFullData", e)
            Else
                ModelState.AddModelError(String.Empty, "Fill all the data")
                Return View("CreateElementFullData", e)
            End If
        End Function

        <HttpPost()>
        Function SubmitComodity(ByVal e As Comodity) As ActionResult
            If (ModelState.IsValid) Then
                DAL.saveComodity(e)
                Return View("DetailsComodity", e)
            Else
                ModelState.AddModelError(String.Empty, "Error on comodity edit")
                Return View("EditComodity", DAL.GetComodityByCode(e.Id))
            End If
        End Function

        <HttpPost()>
        Function SubmitFamily(ByVal e As Family) As ActionResult
            If (ModelState.IsValid) Then
                DAL.saveFamily(e)
                DAL.saveFamilyOracle(e)
                e.Parent = DAL.GetComodityByCode(e.ParentId).Name
                Return View("DetailsFamily", e)
            Else
                ModelState.AddModelError("Name", "Error on family edit")
                Return View("EditFamily", DAL.GetFamilyByCode(e.Id))
            End If
        End Function

        <HttpPost()>
        Function SubmitSubfamily(ByVal e As SubFamily) As ActionResult
            If (ModelState.IsValid) Then
                DAL.saveSubfamily(e)
                DAL.saveSubfamilyOracle(e)
                e.Parent = DAL.GetFamilyByCode(e.ParentId).Name
                e.Grandparent = DAL.GetComodityByCode(e.GrandparentId).Name
                Return View("DetailsSubfamily", e)
            Else
                ModelState.AddModelError(String.Empty, "Error on subfamily edit")
                Return View("EditSubfamily", DAL.GetSubFamilyByCode(e.Id))
            End If
        End Function
#End Region

#Region "Creates"
        ''' <summary>
        ''' TODO: MIRAR ESTO Y REPLICARLO PARA TODAS LAS ACCIONES
        ''' TODO: AÑADIR LOG
        ''' </summary>
        ''' <param name="e"></param>
        ''' <returns></returns>
        <HttpPost()>
        Function CreateComodity(ByVal e As Comodity) As ActionResult
            If (ModelState.IsValid) Then

                Dim code = DAL.CreateComodity(e.Name)
                If code Is Nothing Then
                    ModelState.AddModelError(String.Empty, "The name of the comodity is already in use")
                    Return View()
                Else
                    DAL.CreateComodityOracle(e.Name)
                    'Return View("DetailsComodity", DAL.GetComodityByCode(code))
                    ViewBag.Type = "Comodity"
                    ViewBag.Name = e.Name
                    ViewBag.Verb = "created"
                    ViewBag.Status = "success"
                    Return View("~/Views/Home/Index2.vbhtml", DAL.GetFullData())
                End If
            Else
                Return View()
            End If
        End Function

        <HttpPost()>
        Function CreateFamily(ByVal e As Family) As ActionResult
            If (ModelState.IsValid) Then

                Dim code = DAL.CreateFamily(e)
                If code Is Nothing Then
                    ModelState.AddModelError(String.Empty, "The name of the family is already in use")
                    Return View()
                End If
                DAL.CreateFamilyOracle(e)
                e.ParentId = e.Parent
                e.Parent = DAL.GetComodityByCode(e.Parent).Name
                Return View("DetailsFamily", e)
            Else
                Return View()
            End If
        End Function

        <HttpPost()>
        Function CreateSubfamily(ByVal e As SubFamily) As ActionResult
            If (ModelState.IsValid) Then

                Dim code = DAL.CreateSubFamily(e)
                If code Is Nothing Then
                    ModelState.AddModelError(String.Empty, "The name of the subfamily is already in use")
                    Return View()
                Else
                    e.Id = code
                    DAL.CreateSubFamilyOracle(e)

                End If
                e.Parent = DAL.GetFamilyByCode(e.ParentId).Name
                e.Grandparent = DAL.GetComodityByCode(e.GrandparentId).Name
                Return View("DetailsSubfamily", e)
            Else
                Return View()
            End If
        End Function

        '<HttpPost()>
        'Function CreateFullElement(ByVal e As FullElement) As ActionResult
        '    If (ModelState.IsValid) Then
        '        Dim code = DAL.CreateFullElement(e)
        '        If code Is Nothing Then
        '            ModelState.AddModelError(String.Empty, "The name of the element is already in use")
        '            Return View()
        '        Else
        '            e.Id = code
        '        End If
        '        Return View("DetailsElement", e)
        '    Else
        '        Return View()
        '    End If
        'End Function
#End Region

#Region "deletes"
        Function DeleteElement(ByVal e As Element) As ActionResult
            DAL.DeleteElementOracle(e.Id, e.Name)
            DAL.DeleteElement(e.Id, e.Name)
            Return View("../Default/GetElements", DAL.GetFullDataWithoutNulls())
        End Function

        Function DeleteSubfamily(ByVal s As SubFamily) As ActionResult
            Dim elements As List(Of Element) = DAL.GetElementsBySubfamilyCode(s.Id)
            For Each element In elements
                DAL.DeleteElementOracle(element.Id, element.Name)
                DAL.DeleteElement(element.Id, element.Name)
            Next
            DAL.DeleteSubFamilyOracle(s.Id)
            DAL.DeleteSubFamily(s.Id)
            Return View("../Default/GetSubFamilies", DAL.GetSubFamilyAll())
        End Function

        Function DeleteFamily(ByVal f As Family) As ActionResult
            Dim subfamilies As List(Of SubFamily) = DAL.GetSubfamiliesFromFamilyCode(f.Id)
            For Each subfamily In subfamilies
                Dim elements As List(Of Element) = DAL.GetElementsBySubfamilyCode(subfamily.Id)
                For Each element In elements
                    DAL.DeleteElementOracle(element.Id, element.Name)
                    DAL.DeleteElement(element.Id, element.Name)
                Next
                DAL.DeleteSubFamilyOracle(subfamily.Id)
                DAL.DeleteSubFamily(subfamily.Id)
            Next
            DAL.DeleteFamilyOracle(f.Id)
            DAL.DeleteFamily(f.Id)
            Return View("../Default/GetFamilies", DAL.GetFamilyAll())
        End Function

        Function DeleteComodity(ByVal c As Comodity) As ActionResult
            Try
                Dim families As List(Of Family) = DAL.GetFamiliesFromComodityCode(c.Id)
                For Each family In families
                    Dim subfamilies As List(Of SubFamily) = DAL.GetSubfamiliesFromFamilyCode(family.Id)
                    For Each subfamily In subfamilies
                        Dim elements As List(Of Element) = DAL.GetElementsBySubfamilyCode(subfamily.Id)
                        For Each element In elements
                            DAL.DeleteElementOracle(element.Id, element.Name)
                            DAL.DeleteElement(element.Id, element.Name)
                        Next
                        DAL.DeleteSubFamilyOracle(subfamily.Id)
                        DAL.DeleteSubFamily(subfamily.Id)
                    Next
                    DAL.DeleteFamilyOracle(family.Id)
                    DAL.DeleteFamily(family.Id)
                Next
                DAL.DeleteComodityOracle(c.Id)
                DAL.DeleteComodity(c.Id)
                ViewBag.Msg = "Comodity """ & c.Name & """ deleted (recursively)"
                ViewBag.Status = "success"
            Catch

            End Try
            Return View("../Default/GetComodities", DAL.GetComodityAll())
        End Function
#End Region


    End Class
End Namespace