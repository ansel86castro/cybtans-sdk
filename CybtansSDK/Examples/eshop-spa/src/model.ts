export interface Product {
    id: number;
    name: string;
    description: string;
    price: number | null;
    pictureFileName: string;
    pictureUrl: string;
    brandId: number;
    catalogId: number;
    avalaibleStock: number;
    restockThreshold: number;
    createDate: string;
    updateDate: string | null;
    catalog: Catalog;
    brand: Brand;
    comments: Comment[];
}
export interface Brand {
    id: number;
    name: string;
}

export interface Catalog {
    id: number;
    name: string;
}

export interface GetProductRequest{
    id:number;
}

export interface GetProductsRequest{
    filter:string;
    sort:string;
}
export interface GetProductListResponse {
    items: Product[];
    page: number;
    totalPages: number;
}

export interface GetProductListRequest {
    filter?: string;
    sort?: string;
    page?: number;
    pageSize?: number;
}

