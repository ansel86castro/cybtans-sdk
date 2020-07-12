namespace Cybtans.Proto.Generators.CSharp
{
    public class GenerationOptions
    {        
        public TypeGeneratorOption ModelOptions { get; set; }

        public TypeGeneratorOption ServiceOptions { get; set; }

        public WebApiControllerGeneratorOption ControllerOptions { get; set; }

        public TypeGeneratorOption ClientOptions { get; set; }

        public ApiGateWayGeneratorOption ApiGatewayOptions { get; set; }
    }

    public class WebApiControllerGeneratorOption: TypeGeneratorOption
    {
        
    }

    public class ApiGateWayGeneratorOption : WebApiControllerGeneratorOption
    {

    }
}
