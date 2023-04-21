using Microsoft.AspNetCore.Mvc.ActionConstraints;

namespace Temachti.Api.Utils;

public class HeaderContainsAttribute : Attribute, IActionConstraint
{
    private readonly string header;
    private readonly string value;

    public HeaderContainsAttribute(string header, string value)
    {
        this.header = header;
        this.value = value;
    }

    public int Order => 0;

    // comprobamos si la peticion HTTP contiene la cabecera y tiene el valor  proporcionado
    public bool Accept(ActionConstraintContext context)
    {
        var headers = context.RouteContext.HttpContext.Request.Headers;
        if(!headers.ContainsKey(header))
        {
            return false;
        }

        return string.Equals(headers[header], value, StringComparison.OrdinalIgnoreCase);
    }
}