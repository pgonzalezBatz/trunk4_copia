@Helper ValidationSummaryBootstrap(vd As Mvc.ModelStateDictionary, h As Mvc.HtmlHelper)
    If vd.Keys.Any(Function(k) vd(k).Errors.Any()) Then
        @<div Class="alert alert-danger">
            <Button Class="close" data-dismiss="alert" aria-hidden="true">&times;</Button>
           @h.ValidationSummary(False)
        </div> End If
End Helper
