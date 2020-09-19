
// **************************** START @{ENTITY} **********************************************

message Get@{ENTITY}Request {
	@{ID_TYPE} @{ID} = 1;
}

message GetAll@{ENTITY}Response {
	repeated @{ENTITYDTO} items = 1;
	int64 page = 2;
	int64 totalPages = 3;
	int64 totalCount = 4;
}

service @{ENTITY}Service {
	option (prefix) ="api/@{ENTITY}";

	rpc GetAll(GetAllRequest) returns (GetAll@{ENTITY}Response){		
		option method = "GET";
		@{READ_POLICY}
	};

	rpc Get(Get@{ENTITY}Request) returns (@{ENTITYDTO}){	
		option template = "{@{ID}}"; 
		option method = "GET";
		@{READ_POLICY}
	};	
}

// **************************** END @{ENTITY} **********************************************