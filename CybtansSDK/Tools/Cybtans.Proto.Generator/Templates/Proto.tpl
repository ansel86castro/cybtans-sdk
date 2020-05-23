syntax = "proto3";

package @{SERVICE};

// **************************** MODELS ************************************************

message Product{
	guid id = 1;
	string name = 2 [required = true];
	string description = 3;
	float Price = 4 [optional = true];	
	string pictureUrl = 6;
	int32 brandId = 7;
	int32 catalogId = 8;
	int32 avalaibleStock = 9;
	int32 RestockThreshold = 10;
	datetime createDate = 11;
	datetime updateDate = 12 [optional = true];	
	Brand brand = 13;
}

message Brand {
	int32 id = 1;
	string name = 2;
}

// **************************** REQUESTS **********************************************

message GetProductsRequest {
	string filter = 1;
	string sort = 2;
	string page = 3;
}

message GetProductRequest {
	guid id = 1;
}

message UpdateProductRequest {
	guid id = 1;
	Product Product = 2 [optional = true];
	map<string, object> data = 3 [optional = true];
}

message DeleteProductRequest{
	guid id = 1;
}

// **************************** RESPONSES **********************************************

message GetProductsResponse {
	repeated Product items = 1;
	int32 page = 2;
	int32 totalPages = 3;
}

// **************************** SERVICES **********************************************

service ProductService {
	option (prefix) ="api/product";

	rpc GetProducts(GetProductsRequest) returns (GetProductsResponse){		
		option method = "GET";
	};

	rpc GetProduct(GetProductRequest) returns (Product){	
		option template = "{id}"; 
		option method = "GET";
	};

	rpc CreateProduct(Product) returns (Product){			
		option method = "POST";
	};

	rpc UpdateProduct(UpdateProductRequest) returns (Product){			
		option template = "{id}"; 
		option method = "PUT";
	};

	rpc DeleteProduct(DeleteProductRequest) returns (void){
		option template = "{id}"; 
		option method = "DELETE";
	};
}