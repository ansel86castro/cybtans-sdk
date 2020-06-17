
// **************************** START @{ENTITY} **********************************************

message GetAll@{ENTITY}Request {
	string filter = 1;
	string sort = 2;
	int32 skip = 3;
	int32 take = 4;
}

message Get@{ENTITY}Request {
	@{ID_TYPE} @{ID} = 1;
}

message Update@{ENTITY}Request {
	@{ID_TYPE} @{ID} = 1;
	@{ENTITYDTO} value = 2;
}

message Delete@{ENTITY}Request{
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

	rpc GetAll(GetAll@{ENTITY}Request) returns (GetAll@{ENTITY}Response){		
		option method = "GET";
		option policy = "@{ENTITY}.Read";
	};

	rpc Get(Get@{ENTITY}Request) returns (@{ENTITYDTO}){	
		option template = "{@{ID}}"; 
		option method = "GET";
		option policy = "@{ENTITY}.Read";
	};

	rpc Create(@{ENTITYDTO}) returns (@{ENTITYDTO}){			
		option method = "POST";
		option policy = "@{ENTITY}.Write";
	};

	rpc Update(Update@{ENTITY}Request) returns (@{ENTITYDTO}){			
		option template = "{@{ID}}"; 
		option method = "PUT";
		option policy = "@{ENTITY}.Write";
	};

	rpc Delete(Delete@{ENTITY}Request) returns (void){
		option template = "{@{ID}}"; 
		option method = "DELETE";
		option policy = "@{ENTITY}.Write";
	};
}

// **************************** END @{ENTITY} **********************************************