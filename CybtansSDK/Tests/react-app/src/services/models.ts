
export interface CustomerDto {
<<<<<<< HEAD
  name?: string;
  firstLastName?: string;
  secondLastName?: string;
  customerProfileId?: string|null;
  customerProfile?: CustomerProfileDto|null;
=======
  name: string;
  firstLastName: string;
  secondLastName: string;
  customerProfileId?: string|null;
  customerProfile: CustomerProfileDto|null;
>>>>>>> master
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
<<<<<<< HEAD
  creator?: string;
=======
  creator: string;
>>>>>>> master
}


export interface CustomerProfileDto {
<<<<<<< HEAD
  name?: string;
=======
  name: string;
>>>>>>> master
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
<<<<<<< HEAD
  creator?: string;
=======
  creator: string;
>>>>>>> master
}


export interface CustomerEventDto {
<<<<<<< HEAD
  fullName?: string;
=======
  fullName: string;
>>>>>>> master
  customerProfileId?: string|null;
  id?: string;
}


export interface OrderItemDto {
<<<<<<< HEAD
  productName?: string;
=======
  productName: string;
>>>>>>> master
  price?: number;
  discount?: number;
  orderId?: string;
  id?: string;
}


export interface OrderDto {
<<<<<<< HEAD
  description?: string;
  customerId?: string;
  orderStateId?: number;
  orderType?: OrderTypeEnum;
  orderState?: OrderStateDto|null;
  customer?: CustomerDto|null;
  items?: OrderItemDto[]|null;
=======
  description: string;
  customerId?: string;
  orderStateId?: number;
  orderType?: OrderTypeEnum;
  orderState: OrderStateDto|null;
  customer: CustomerDto|null;
  items: OrderItemDto[]|null;
>>>>>>> master
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
<<<<<<< HEAD
  creator?: string;
=======
  creator: string;
>>>>>>> master
}



export enum OrderTypeEnum {
  default = 0,
  normal = 1,
  shipping = 2,
}



export interface OrderStateDto {
<<<<<<< HEAD
  name?: string;
=======
  name: string;
>>>>>>> master
  id?: number;
}


export interface SoftDeleteOrderDto {
<<<<<<< HEAD
  name?: string;
  isDeleted?: boolean;
  items?: SoftDeleteOrderItemDto[]|null;
=======
  name: string;
  isDeleted?: boolean;
  items: SoftDeleteOrderItemDto[]|null;
>>>>>>> master
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
<<<<<<< HEAD
  creator?: string;
=======
  creator: string;
>>>>>>> master
}


export interface SoftDeleteOrderItemDto {
<<<<<<< HEAD
  name?: string;
=======
  name: string;
>>>>>>> master
  isDeleted?: boolean;
  softDeleteOrderId?: string;
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
<<<<<<< HEAD
  creator?: string;
=======
  creator: string;
>>>>>>> master
}


export interface GetAllRequest {
  filter?: string;
  sort?: string;
  skip?: number|null;
  take?: number|null;
}


export interface GetCustomerRequest {
  id?: string;
}


export interface UpdateCustomerRequest {
  id?: string;
<<<<<<< HEAD
  value?: CustomerDto|null;
=======
  value: CustomerDto|null;
>>>>>>> master
}


export interface DeleteCustomerRequest {
  id?: string;
}


export interface GetAllCustomerResponse {
<<<<<<< HEAD
  items?: CustomerDto[]|null;
=======
  items: CustomerDto[]|null;
>>>>>>> master
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface GetCustomerEventRequest {
  id?: string;
}


export interface UpdateCustomerEventRequest {
  id?: string;
<<<<<<< HEAD
  value?: CustomerEventDto|null;
=======
  value: CustomerEventDto|null;
>>>>>>> master
}


export interface DeleteCustomerEventRequest {
  id?: string;
}


export interface GetAllCustomerEventResponse {
<<<<<<< HEAD
  items?: CustomerEventDto[]|null;
=======
  items: CustomerEventDto[]|null;
>>>>>>> master
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface GetOrderRequest {
  id?: string;
}


export interface UpdateOrderRequest {
  id?: string;
<<<<<<< HEAD
  value?: OrderDto|null;
=======
  value: OrderDto|null;
>>>>>>> master
}


export interface DeleteOrderRequest {
  id?: string;
}


export interface GetAllOrderResponse {
<<<<<<< HEAD
  items?: OrderDto[]|null;
=======
  items: OrderDto[]|null;
>>>>>>> master
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface GetOrderStateRequest {
  id?: number;
}


export interface UpdateOrderStateRequest {
  id?: number;
<<<<<<< HEAD
  value?: OrderStateDto|null;
=======
  value: OrderStateDto|null;
>>>>>>> master
}


export interface DeleteOrderStateRequest {
  id?: number;
}


export interface GetAllOrderStateResponse {
<<<<<<< HEAD
  items?: OrderStateDto[]|null;
=======
  items: OrderStateDto[]|null;
>>>>>>> master
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface GetSoftDeleteOrderRequest {
  id?: string;
}


export interface UpdateSoftDeleteOrderRequest {
  id?: string;
<<<<<<< HEAD
  value?: SoftDeleteOrderDto|null;
=======
  value: SoftDeleteOrderDto|null;
>>>>>>> master
}


export interface DeleteSoftDeleteOrderRequest {
  id?: string;
}


export interface GetAllSoftDeleteOrderResponse {
<<<<<<< HEAD
  items?: SoftDeleteOrderDto[]|null;
=======
  items: SoftDeleteOrderDto[]|null;
>>>>>>> master
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface UploadImageRequest {
  name: string;
  size?: number;
  image: Blob|null;
}


export interface UploadImageResponse {
  url: string;
  m5checksum: string;
}


export interface UploadStreamByIdRequest {
  id: string;
  data: Blob|null;
}


export interface UploadStreamResponse {
  m5checksum: string;
}


export interface DownloadImageRequest {
  name: string;
}


<<<<<<< HEAD
export interface DowndloadImageResponse {
  fileName: string;
  contentType: string;
  image: Blob|null;
}


=======
>>>>>>> master
export interface LoginRequest {
  username: string;
  password: string;
}


export interface LoginResponse {
  token: string;
}
