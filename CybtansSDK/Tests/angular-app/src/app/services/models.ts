
/** Customer Entity */
export interface CustomerDto {
  /** Customer's Name */
  name: string;
  /** Customer's FirstLastName */
  firstLastName: string;
  /** Customer's SecondLastName */
  secondLastName: string;
  /** Customer's Profile Id, can be null */
  customerProfileId?: string|null;
  customerProfile?: CustomerProfileDto|null;
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
  creator: string;
}


export interface CustomerProfileDto {
  name: string;
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
  creator: string;
}


export interface CustomerEventDto {
  fullName: string;
  customerProfileId?: string|null;
  id?: string;
}


export interface OrderItemDto {
  productName: string;
  price?: number;
  discount?: number;
  orderId?: string;
  id?: string;
}


export interface OrderDto {
  description: string;
  customerId?: string;
  orderStateId?: number;
  orderType?: OrderTypeEnum;
  orderState?: OrderStateDto|null;
  customer?: CustomerDto|null;
  items?: OrderItemDto[]|null;
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
  creator: string;
}



/** Enum Type Description */
export enum OrderTypeEnum {
  /** Default */
  default = 0,
  /** Normal */
  normal = 1,
  /** Shipping */
  shipping = 2,
}



export interface OrderStateDto {
  name: string;
  id?: number;
}


export interface SoftDeleteOrderDto {
  name: string;
  isDeleted?: boolean;
  items?: SoftDeleteOrderItemDto[]|null;
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
  creator: string;
}


export interface SoftDeleteOrderItemDto {
  name: string;
  isDeleted?: boolean;
  softDeleteOrderId?: string;
  tenantId?: string|null;
  id?: string;
  createDate?: string|Date;
  updateDate?: string|Date|null;
  creator: string;
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
  value?: CustomerDto|null;
}


export interface DeleteCustomerRequest {
  id?: string;
}


export interface GetAllCustomerResponse {
  items?: CustomerDto[]|null;
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface GetCustomerEventRequest {
  id?: string;
}


export interface UpdateCustomerEventRequest {
  id?: string;
  value?: CustomerEventDto|null;
}


export interface DeleteCustomerEventRequest {
  id?: string;
}


export interface GetAllCustomerEventResponse {
  items?: CustomerEventDto[]|null;
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface GetOrderRequest {
  id?: string;
}


export interface UpdateOrderRequest {
  id?: string;
  value?: OrderDto|null;
}


export interface DeleteOrderRequest {
  id?: string;
}


export interface GetAllOrderResponse {
  items?: OrderDto[]|null;
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface GetOrderStateRequest {
  id?: number;
}


export interface UpdateOrderStateRequest {
  id?: number;
  value?: OrderStateDto|null;
}


export interface DeleteOrderStateRequest {
  id?: number;
}


export interface GetAllOrderStateResponse {
  items?: OrderStateDto[]|null;
  page?: number;
  totalPages?: number;
  totalCount?: number;
}


export interface GetSoftDeleteOrderRequest {
  id?: string;
}


export interface UpdateSoftDeleteOrderRequest {
  id?: string;
  value?: SoftDeleteOrderDto|null;
}


export interface DeleteSoftDeleteOrderRequest {
  id?: string;
}


export interface GetAllSoftDeleteOrderResponse {
  items?: SoftDeleteOrderDto[]|null;
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


export interface DowndloadImageResponse {
  fileName: string;
  contentType: string;
  image: Blob|null;
}


export interface LoginRequest {
  username: string;
  password: string;
}


export interface LoginResponse {
  token: string;
}
