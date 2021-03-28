{
    "Service": "@{SERVICE}",
    "Steps":[
        {
          "Type": "messages",
          "Output": ".",
          "ProtoFile": "./Proto/Data.proto",
          "AssemblyFile": "./@{SERVICE}.RestApi/bin/Debug/netcoreapp3.1/@{SERVICE}.Data.dll"
        },
        {
            "Type": "proto",
            "Output": ".",
            "ProtoFile": "./Proto/@{SERVICE}.proto"
        }
    ]
}