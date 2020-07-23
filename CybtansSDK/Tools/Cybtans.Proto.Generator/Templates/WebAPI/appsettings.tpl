{
   "Serilog": { 
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning",
        "@{SERVICE}": "Information",
        "System": "Warning"
      }
    }
  },
  "AllowedHosts": "*"
}
