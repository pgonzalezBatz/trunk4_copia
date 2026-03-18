@ModelType OBJETIVOPROCESO

@Code
    ViewData("Title") = "EditObjetivoProceso"
    Layout = "~/Views/Shared/_Layout.vbhtml"
    Dim accion As String
    Dim color As String = "#000"
    If Model Is Nothing Then
        accion = "Creación"
    Else
        accion = "Edición"
    End If
    If Not ViewData.ModelState.IsValid AndAlso ViewData.ModelState("ANNO").Errors.Any Then
        color = "#a94442"
    End If
End Code

<br />
<h2>@accion de objetivo de proceso</h2>
<br />

@Using (Html.BeginForm())
    @Html.AntiForgeryToken()
    @<div class="form-horizontal">
    @Html.ValidationSummary(True, "", New With {.class = "text-danger", .style = "text-align:center"})
    @Html.HiddenFor(Function(model) model.ID)

            <div Class="form-group">
                @Html.LabelFor(Function(model) model.ANNO, htmlAttributes:=New With {.class = "control-label col-md-6"})
                 <div Class="col-md-6">
                     @If Model Is Nothing Then
                         @Html.EditorFor(Function(model) model.ANNO, New With {.htmlAttributes = New With {.class = "form-control", .style = "color:" & color}})
                     Else
                         @Html.EditorFor(Function(model) model.ANNO, New With {.htmlAttributes = New With {.class = "form-control", .disabled = "disabled"}})

                     End If
                     @Html.ValidationMessageFor(Function(model) model.ANNO, "", New With {.class = "text-danger"})
                 </div>
            </div>

    @*@Html.HiddenFor(Function(model) model.ANNO)*@
    <div class="form-group">
        @Html.LabelFor(Function(model) model.REPETITIVAS, htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.REPETITIVAS, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.REPETITIVAS, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="form-group">
        @Html.LabelFor(Function(model) model.DIAS14, htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.DIAS14, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.DIAS14, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div class="form-group">
        @Html.LabelFor(Function(model) model.DIAS56, htmlAttributes:=New With {.class = "control-label col-md-6"})
        <div class="col-md-6">
            @Html.EditorFor(Function(model) model.DIAS56, New With {.htmlAttributes = New With {.class = "form-control"}})
            @Html.ValidationMessageFor(Function(model) model.DIAS56, "", New With {.class = "text-danger"})
        </div>
    </div>
    <div style="text-align:center">
        <input type="submit" value="Guardar" Class="btn btn-info" style="font-size:18px;" />
    </div>
</div>
End Using
