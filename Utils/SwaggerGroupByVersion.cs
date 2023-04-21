using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Temachti.Api.Utils;

// clase que agrupa nuestros controladores segun su namespace
public class SwaggerGroupByVersion : IControllerModelConvention
{
    public void Apply(ControllerModel controller)
    {
        var namespaceController = controller.ControllerType.Namespace; // Controller.V1
        var apiVersion = namespaceController.Split('.').Last().ToLower(); // v1
        controller.ApiExplorer.GroupName = apiVersion;
    }
}