{
   "Serilog": {
    "MinimumLevel": {
      "Default": "Debug",
      "Override": {
        "Microsoft": "Information",
        "@{SERVICE}": "Debug",
        "System": "Warning"
      }
    }
  }
}
