syntax = "proto3";

package @{SERVICE};

// **************************** MODELS ************************************************

message Entity {
	guid id = 1;
	string name = 2 [required = true];
	datetime createDate = 3;
	int32 creatorId = 4;
}

// **************************** REQUESTS **********************************************

message GetListRequest {
	string filter = 1;
	string sort = 2;
	int32 skip = 3;
	int32 take = 4;
}

message GetEntityRequest {
	guid id = 1;
}

message UpdateEntityRequest {
	guid id = 1;
	map<string, object> data = 2;
}

message DeleteEntityRequest{
	guid id = 1;
}

// **************************** RESPONSES **********************************************

message GetListResponse {
	repeated Entity items = 1;
	int64 page = 2;
	int64 totalPages = 3;
	int64 totalCount = 4;
}

// **************************** SERVICES **********************************************

service EntityService {
	option (prefix) ="api/entity";

	rpc GetAll(GetListRequest) returns (GetListResponse){		
		option method = "GET";
	};

	rpc Get(GetEntityRequest) returns (Entity){	
		option template = "{id}"; 
		option method = "GET";
	};

	rpc Create(Entity) returns (Entity){			
		option method = "POST";
	};

	rpc Update(UpdateEntityRequest) returns (Entity){			
		option template = "{id}"; 
		option method = "PUT";
	};

	rpc Delete(DeleteEntityRequest) returns (void){
		option template = "{id}"; 
		option method = "DELETE";
	};
}