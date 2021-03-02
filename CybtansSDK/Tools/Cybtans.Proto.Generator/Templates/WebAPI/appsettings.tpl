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
  
  "AllowedHosts": "*",

  "CorsOrigins":"*",
  
  "ConnectionStrings": {
    "DefaultConnection": "Server=(LocalDb)\\MSSQLLocalDB;Database=@{SERVICE};Trusted_Connection=True;MultipleActiveResultSets=true"
  },

  "Identity": {
    "Authority": "https://localhost:6001",    
    "Swagger": "https://localhost:6001",
    "Secret": "Secret"
  }

}
