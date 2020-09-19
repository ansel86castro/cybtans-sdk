
export interface ReadOnlyEntityDto {
  name: string;
  createDate?: string|Date|null;
  updateDate?: string|Date|null;
  id: number;
}


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
  id: string;
  createDate?: string|Date|null;
  updateDate?: string|Date|null;
}


export interface CustomerProfileDto {
  name: string;
  id: string;
  createDate?: string|Date|null;
  updateDate?: string|Date|null;
}


export interface CustomerEventDto {
  fullName: string;
  customerProfileId?: string|null;
  id: string;
}


export interface OrderItemDto {
  productName: string;
  price: number;
  discount: number;
  orderId: string;
  id: string;
}


export interface OrderDto {
  description: string;
  customerId: string;
  orderStateId: number;
  orderType: OrderTypeEnum;
  orderState?: OrderStateDto|null;
  customer?: CustomerDto|null;
  items?: OrderItemDto[]|null;
  id: string;
  createDate?: string|Date|null;
  updateDate?: string|Date|null;
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
  id: number;
}


export interface SoftDeleteOrderDto {
  name: string;
  isDeleted: boolean;
  items?: SoftDeleteOrderItemDto[]|null;
  id: string;
  createDate?: string|Date|null;
  updateDate?: string|Date|null;
}


export interface SoftDeleteOrderItemDto {
  name: string;
  isDeleted: boolean;
  softDeleteOrderId: string;
  id: string;
  createDate?: string|Date|null;
  updateDate?: string|Date|null;
}


export interface GetAllRequest {
  filter?: string;
  sort?: string;
  skip?: number|null;
  take?: number|null;
}


export interface GetReadOnlyEntityRequest {
  id: number;
}


export interface GetAllReadOnlyEntityResponse {
  items?: ReadOnlyEntityDto[]|null;
  page: number;
  totalPages: number;
  totalCount: number;
}


export interface GetCustomerRequest {
  id: string;
}


export interface UpdateCustomerRequest {
  id: string;
  value?: Partial<CustomerDto|null>;
}


export interface DeleteCustomerRequest {
  id: string;
}


export interface GetAllCustomerResponse {
  items?: CustomerDto[]|null;
  page: number;
  totalPages: number;
  totalCount: number;
}


export interface CreateCustomerRequest {
  value?: Partial<CustomerDto|null>;
}


export interface GetCustomerEventRequest {
  id: string;
}


export interface UpdateCustomerEventRequest {
  id: string;
  value?: Partial<CustomerEventDto|null>;
}


export interface DeleteCustomerEventRequest {
  id: string;
}


export interface GetAllCustomerEventResponse {
  items?: CustomerEventDto[]|null;
  page: number;
  totalPages: number;
  totalCount: number;
}


export interface CreateCustomerEventRequest {
  value?: Partial<CustomerEventDto|null>;
}


export interface GetOrderRequest {
  id: string;
}


export interface UpdateOrderRequest {
  id: string;
  value?: Partial<OrderDto|null>;
}


export interface DeleteOrderRequest {
  id: string;
}


export interface GetAllOrderResponse {
  items?: OrderDto[]|null;
  page: number;
  totalPages: number;
  totalCount: number;
}


export interface CreateOrderRequest {
  value?: Partial<OrderDto|null>;
}


export interface GetOrderStateRequest {
  id: number;
}


export interface UpdateOrderStateRequest {
  id: number;
  value?: Partial<OrderStateDto|null>;
}


export interface DeleteOrderStateRequest {
  id: number;
}


export interface GetAllOrderStateResponse {
  items?: OrderStateDto[]|null;
  page: number;
  totalPages: number;
  totalCount: number;
}


export interface CreateOrderStateRequest {
  value?: Partial<OrderStateDto|null>;
}


export interface GetSoftDeleteOrderRequest {
  id: string;
}


export interface UpdateSoftDeleteOrderRequest {
  id: string;
  value?: Partial<SoftDeleteOrderDto|null>;
}


export interface DeleteSoftDeleteOrderRequest {
  id: string;
}


export interface GetAllSoftDeleteOrderResponse {
  items?: SoftDeleteOrderDto[]|null;
  page: number;
  totalPages: number;
  totalCount: number;
}


export interface CreateSoftDeleteOrderRequest {
  value?: Partial<SoftDeleteOrderDto|null>;
}


export interface LoginRequest {
  username: string;
  password: string;
}


export interface LoginResponse {
  token: string;
}


export interface UploadImageRequest {
  name: string;
  size: number;
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
