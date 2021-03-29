namespace Cybtans.Proto.Generators.CSharp
{
    public class GenerationOptions
    {        
        public ModelGeneratorOptions ModelOptions { get; set; }

        public ServiceGeneratorOptions ServiceOptions { get; set; }

        public WebApiControllerGeneratorOption ControllerOptions { get; set; }

        public TypeGeneratorOption ClientOptions { get; set; }

        public ApiGateWayGeneratorOption ApiGatewayOptions { get; set; }
    }

    public class ModelGeneratorOptions: TypeGeneratorOption
    {
        public bool GenerateAccesor { get; set; } = true;
    }

    public class ServiceGeneratorOptions: TypeGeneratorOption
    {
        public string? NameTemplate { get; set; }

        public bool AutoRegisterImplementation { get; set; }
        
    }

    public class WebApiControllerGeneratorOption: TypeGeneratorOption
    {
        
    }

    public class ApiGateWayGeneratorOption : WebApiControllerGeneratorOption
    {

    }
}
