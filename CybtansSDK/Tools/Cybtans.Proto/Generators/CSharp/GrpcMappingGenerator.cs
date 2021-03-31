using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;

namespace Cybtans.Proto.Generators.CSharp
{
    public class GrpcMappingGenerator : FileGenerator<ServiceGeneratorOptions>
    {
        class RpcTypeInfo
        {
            public bool IsResponse;
            public bool IsRequest;
            public MessageDeclaration Type;
            public bool VisitedRequest;
            public bool VisitedResponse;

        }

        private readonly IEnumerable<ProtoFile> _protos;
        private readonly ModelGeneratorOptions _modelOptions;

        public GrpcMappingGenerator(ProtoFile entry, IEnumerable<ProtoFile> protos, ServiceGeneratorOptions option, ModelGeneratorOptions modelOptions)
            : base(entry, option)
        {
            this._protos = protos;
            this._modelOptions = modelOptions;
        }

        public override void GenerateCode()
        {
            Dictionary<MessageDeclaration, RpcTypeInfo> typesMap = new Dictionary<MessageDeclaration, RpcTypeInfo>();


            foreach (var proto in _protos)
            {
                var types = proto.Declarations
              .Where(x => x is ServiceDeclaration)
              .Cast<ServiceDeclaration>()
              .Where(x => x.Option.GrpcProxy)
              .SelectMany(x => x.Rpcs)
              .Select(rpc => (request: rpc.RequestType, response: rpc.ResponseType));

                foreach (var (request, response) in types)
                {
                    if (request is MessageDeclaration requestMsg)
                    {
                        AddTypes(requestMsg, 0, typesMap);
                    }

                    if (response is MessageDeclaration responseMsg)
                    {
                        AddTypes(responseMsg, 1, typesMap);
                    }
                }
            }

            if (!typesMap.Any())
                return;

            var ns = _option.ImplementationNamespace ?? $"{Proto.Option.Namespace ?? Proto.Filename.Pascal()}.Services";           
            var modelNs = _modelOptions.Namespace ?? $"{Proto.Option.Namespace ?? Proto.Filename.Pascal()}.Models";

            var writer = CreateWriter(ns);

            writer.Usings.Append("using System.Threading.Tasks;").AppendLine();

            writer.Usings.Append($"using {modelNs};").AppendLine();
            writer.Usings.Append("using System.Collections.Generic;").AppendLine();
            writer.Usings.Append("using System.Linq;").AppendLine();

            writer.Usings.AppendLine().Append($"using mds = global::{modelNs};").AppendLine();


            var clsWriter = writer.Class;

            clsWriter.Append("public static class GrpcMappingExtensions").AppendLine().Append("{").AppendLine();
            clsWriter.Append('\t', 1);

            var bodyWriter = clsWriter.Block("BODY");

            foreach (var (key, item) in typesMap)
            {

                if (item.IsRequest)
                {
                    GenerateModelToProtobufMapping(key, bodyWriter);
                }

                if (item.IsResponse)
                {
                    GenerateProtobufToPocoMapping(key, bodyWriter);
                }
            }

            clsWriter.Append("}").AppendLine();

            writer.SaveTo(_option.ImplementationOutput, $"GrpcMappingExtensions");

        }

        private void GenerateModelToProtobufMapping(MessageDeclaration type, CodeWriter writer)
        {
            var proto = type.ProtoDeclaration;
            var typeName = type.GetTypeName();
            var grpcTypeName = $"{proto.Option.Namespace}.{type.GetProtobufName()}";

            writer.Append($"public static global::{grpcTypeName} ToProtobufModel(this mds::{typeName} model)")
                .AppendLine().Append("{").AppendLine().Append('\t', 1);

            var bodyWriter = writer.Block($"ToProtobufModel_{type.Name}_BODY");

            bodyWriter.Append($"if(model == null) return null;").AppendLine(2);

            bodyWriter.Append($"global::{grpcTypeName} result = new global::{grpcTypeName}();").AppendLine();

            foreach (var field in type.Fields)
            {
                var fieldName = field.Name.Pascal();
                var fieldType = field.FieldType;

                if (field.Type.IsArray)
                {
                    bodyWriter.Append($"if(model.{fieldName} != null) ");

                    var selector = ConvertToGrpc("x", fieldType);
                    if (selector == "x")
                    {
                        bodyWriter.Append($"result.{fieldName}.AddRange(model.{fieldName});").AppendLine();
                    }
                    else
                    {
                        bodyWriter.Append($"result.{fieldName}.AddRange(model.{fieldName}.Select(x => {selector} ));").AppendLine();
                    }
                }
                else if (field.Type.IsMap)
                {

                }
                else
                {
                    var path = ConvertToGrpc($"model.{fieldName}", fieldType);
                    bodyWriter.Append($"result.{fieldName} = {path};").AppendLine();
                }
            }

            bodyWriter.Append("return result;").AppendLine();

            writer.Append("}").AppendLine(2);
        }

        private void GenerateProtobufToPocoMapping(MessageDeclaration type, CodeWriter writer)
        {
            var proto = type.ProtoDeclaration;
            var typeName = type.GetTypeName();
            var grpcTypeName = $"{proto.Option.Namespace}.{type.GetProtobufName()}";

            writer.Append($"public static mds::{typeName} ToPocoModel(this global::{grpcTypeName} model)")
                .AppendLine().Append("{").AppendLine().Append('\t', 1);

            var bodyWriter = writer.Block($"ToPocoModel_{typeName}_BODY");

            bodyWriter.Append($"if(model == null) return null;").AppendLine(2);

            bodyWriter.Append($"mds::{typeName} result = new mds::{typeName}();").AppendLine();

            foreach (var field in type.Fields)
            {
                var fieldName = field.Name.Pascal();
                var fieldType = field.FieldType;

                if (field.Type.IsArray)
                {                  
                    var selector = ConvertToRest("x", fieldType);
                    if (selector == "x")
                    {
                        bodyWriter.Append($"result.{fieldName} = model.{fieldName}.ToList();").AppendLine();
                    }
                    else
                    {
                        bodyWriter.Append($"result.{fieldName} = model.{fieldName}.Select(x => {selector}).ToList();").AppendLine();
                    }
                }
                else if (field.Type.IsMap)
                {

                }              
                else
                {
                    var path = ConvertToRest($"model.{fieldName}", fieldType);
                    bodyWriter.Append($"result.{fieldName} = {path};").AppendLine();
                }
            }

            bodyWriter.Append("return result;").AppendLine();

            writer.Append("}").AppendLine(2);
        }

        private string ConvertToGrpc(string fieldName, ITypeDeclaration fieldType)
        {
            if (PrimitiveType.TimeStamp == fieldType)
            {
                return $"{fieldName}.HasValue ? Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime({fieldName}.Value): null";
            }
            else if(PrimitiveType.Datetime == fieldType)
            {
                return $"Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime({fieldName})";
            }
            else if (PrimitiveType.Duration == fieldType)
            {
                return $"{fieldName}.HasValue ? Google.Protobuf.WellKnownTypes.Duration.FromTimeSpan({fieldName}.Value): null";
            }
            else if (PrimitiveType.String.Equals(fieldType))
            {                
                return $"{ fieldName} ?? string.Empty";
            }
            else if (fieldType is MessageDeclaration)
            {
                return $"ToProtobufModel({fieldName})";
            }
            else if(fieldType is EnumDeclaration e)
            {
                var grpcTypeName = $"{Proto.Option.Namespace}.{e.GetProtobufName()}";
                return $"({grpcTypeName})(int){fieldName}";
            }
            else
            {
                return fieldName;
            }
        }

        private string ConvertToRest(string fieldName, ITypeDeclaration fieldType)
        {
            if (PrimitiveType.Datetime.Equals(fieldType))
            {
                return $"{fieldName}?.ToDateTime()";
            }
            else if (PrimitiveType.Duration.Equals(fieldType))
            {
                return $"{fieldName}?.ToTimeSpan()";
            }
            else if (fieldType is MessageDeclaration)
            {
                return $"ToPocoModel({fieldName})";
            }
            else if (fieldType is EnumDeclaration)
            {
                return $"({fieldType.GetTypeName()})(int){fieldName}";
            }
            else
            {
                return fieldName;
            }
        }

        private void AddTypes(MessageDeclaration type, int position, Dictionary<MessageDeclaration, RpcTypeInfo> typesMap)
        {
            RpcTypeInfo info;

            if (!typesMap.TryGetValue(type, out info))
            {
                info = new RpcTypeInfo
                {
                    Type = type,
                    IsRequest = position == 0,
                    IsResponse = position == 1
                };

                typesMap.Add(type, info);
            }            

            if (position == 0)
            {
                if (info.VisitedRequest) return;

                if (!info.IsRequest) info.IsRequest = true;
                info.VisitedRequest = true;
            }                

            if(position == 1)
            {
                if (info.VisitedResponse) return;
                info.VisitedResponse = true;
                if (!info.IsResponse) info.IsResponse = true;
            }

            foreach (var fieldType in type.Fields.Where(x=>x.FieldType is MessageDeclaration).Select(x=>x.FieldType).Cast<MessageDeclaration>())
            {
                AddTypes(fieldType, position, typesMap);
            }
        }

    }
}
