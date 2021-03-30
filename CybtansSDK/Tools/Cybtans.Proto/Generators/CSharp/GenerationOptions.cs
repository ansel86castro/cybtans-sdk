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
        string _implementationNs;
        private string _implementationOutput;

        public string? NameTemplate { get; set; }

        public bool AutoRegisterImplementation { get; set; }

        public string ImplementationNamespace
        {
            get
            {
                return _implementationNs ?? Namespace;
            }
            set
            {
                _implementationNs = value;
            }
        }
        
        public string ImplementationOutput
        {
            get
            {
                return _implementationOutput ?? OutputPath;
            }
            set
            {
                _implementationOutput = value;
            }
        }
    }

    public class WebApiControllerGeneratorOption: TypeGeneratorOption
    {
        
    }

    public class ApiGateWayGeneratorOption : WebApiControllerGeneratorOption
    {

    }
}
