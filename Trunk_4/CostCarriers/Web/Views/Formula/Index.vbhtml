@Imports CostCarriersLib

@Code
    Dim costCarriers As List(Of ELL.CabeceraCostCarrier) = CType(ViewData("CostCarriers"), List(Of ELL.CabeceraCostCarrier))
    Dim tiposProyecto As List(Of ELL.TipoProyecto) = CType(ViewData("TiposProyecto"), List(Of ELL.TipoProyecto))
End Code

<script src="~/Scripts/jquery.alphanum.js"></script>

<script type="text/javascript">
    $(function () {
        $("#submitOperando").click(function(){
            $("#txtFormula").val($("#txtFormula").val() + $("#txtOperando").val());
            $("#hFormula").val($("#hFormula").val() + $("#txtOperando").val());
        })

        $("#submitVariable").click(function(){
            $("#txtFormula").val($("#txtFormula").val() + '[' + $("#VariablesFormula option:selected").text() + "]");
            $("#hFormula").val($("#hFormula").val() + '[' + $("#VariablesFormula option:selected").val() + "]");
        })

        $("#txtOperando").alphanum({
            allow              : '+ - * / ( ) . ,',    // Allow extra characters
	        allowLatin         : false,  // a-z A-Z
	        allowOtherCharSets : false  // eg é, Á, Arabic, Chinese etc
        });
    })
</script>

@Using Html.BeginForm()
    @Html.Hidden("hFormula") 
    @<div Class="form-row">
         <div class="form-group col-md-5">
             <label class="col-form-label">@Utils.Traducir("Operando")</label>
             @Html.TextBox("txtOperando", String.Empty, New With {.class = "form-control"})             
         </div>
         <div class="form-group col-md-1">   
             <label class="col-form-label">&nbsp;</label>
             <button type="button" id="submitOperando" class="btn btn-primary form-control" value="operando">+</button>
         </div>
         <div class="form-group col-md-5">
             <label class="col-form-label">@Utils.Traducir("Variable")</label>             
             @Html.DropDownList("VariablesFormula", Nothing, New With {.class = "form-control"})
         </div>
         <div class="form-group col-md-1">
             <label class="col-form-label">&nbsp;</label>             
             <button type="button" id="submitVariable" class="btn btn-primary form-control" value="variable">+</button>
         </div>
         <div Class="form-group col-md-12">
             <Label Class="col-form-label">@Utils.Traducir("Formula")</Label>
             @Html.TextBox("txtFormula", String.Empty, New With {.class = "form-control", .readonly = "readonly"})
         </div>
    </div>
End Using

<br />
<div>
    @Html.TextBox("txtResultado", ViewData("Resultado"), New With {.class = "form-control"})
</div>