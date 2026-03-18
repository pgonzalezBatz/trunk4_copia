@imports web


@code
    Dim language_extension = "es-es"
    If Culture.ToLower().Contains("eu-es") Then
        language_extension = "eu-es"
    ElseIf Culture.ToLower().Contains("en-") Then
        language_extension = "en-gb"
    End If
End Code

<h3 class="my-3">Guías del manager</h3>
    <table class="table">
        <tr>
            <td>@H.Traducir("REUNIÓN DE COLABORADORES")</td>
            <td>
                <a href="@Url.Content("~/Content/kolaboratzaileak_" + language_extension + ".pdf") " target="_blank">
                    <img src="@Url.Content("~/Content/Pdf.png")" />
                </a>
            </td>
        </tr>
        <tr>
            <td>@H.Traducir("GUÍA BÁSICA PARA LA REALIZACIÓN DE LOS DESPACHOS")</td>
            <td>
                <a href="@Url.Content("~/Content/P_DESPACHOS_" + language_extension + ".pdf") " target="_blank">
                    <img src="@Url.Content("~/Content/Pdf.png")" />
                </a>
            </td>
        </tr>
    </table>
