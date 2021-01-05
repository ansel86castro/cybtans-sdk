import * as React from 'react';
import { getService } from '../Api';
import { OrderService, CustomerService, OrderStateService } from '../services/services';
import {
    GetAllOrderResponse, OrderDto, GetCustomerRequest, OrderTypeEnum, CustomerDto, OrderStateDto
} from '../services/models';
import { useForm } from "react-hook-form";
import { createRef } from 'react';

type State = {
 invalidated: boolean;
 customers?: CustomerDto[] | null;
 orderStates?: OrderStateDto[] | null;
 item?:OrderDto,
 orderTypes: Array<{label:string, value:OrderTypeEnum}>;
};

export default function EditOrder(props:{
    id?:string;
    onSuccess:()=>void;
}){
    const service = getService(OrderService);
    const customerService = getService(CustomerService);
    const orderStateService = getService(OrderStateService);    

    const { register, handleSubmit, watch, errors } = useForm<OrderDto>();
   
    let [state, setState] = React.useState<State>({
        invalidated:true,
        orderTypes:[ 
            {label: 'Default', value :OrderTypeEnum.default}, 
            {label: 'Normal', value :OrderTypeEnum.normal},
            {label: 'Shipping', value :OrderTypeEnum.shipping}
        ]
    })

    const onSubmit = async (data:any) =>{
        console.log(data);
        let response:OrderDto|undefined = undefined;

        let order = {
             description: data.description,
             customerId: data.customerId,
             orderStateId: Number.parseInt(data.orderStateId),
             orderType : Number.parseInt(data.orderType)             
        };

        if(props.id){
            response = await service.update({
                id: props.id,
                value :order
            });
        }else{
            response = await service.create({ value : order });
        }

        if(response){
            props.onSuccess();
        }
    }

    React.useEffect(()=>{
        if(state.invalidated === false)
            return;      
        fetchData();        
    },[state.invalidated]);

    async function fetchData(){
        let result:Partial<State> = {invalidated:false};
        if(props.id){
            result.item = await service.get({id: props.id});
        }

        result.customers = (await customerService.getAll({})).items;
        result.orderStates = (await orderStateService.getAll({})).items;

        setState({...state, ...result});
    }

    return (
        <form onSubmit={handleSubmit(onSubmit)}>
            <h2>{ props.id && 'Update' || 'Create'} Order</h2>
            <div className="form-group">
                <label>Description</label>
                <input className="form-control" name="description" ref={register({ required: true })} defaultValue={state.item?.description || ''} />
                {errors.description && <span>This field is required</span>}
            </div>

            <div className="form-group">
                <label>Select Customer</label>
                <select className="form-control" name="customerId" ref={register} defaultValue={state.item?.customerId} >
                    {state.customers && state.customers.map(x=>(
                        <option selected={x.id === state.item?.customerId} key={x.id} value={x.id!}>{x.name} {x.firstLastName}</option>
                    ))}                  
                </select>
            </div>

            <div className="form-group">
                <label>Select Order State</label>
                <select className="form-control" name="orderStateId" ref={register} defaultValue={state.item?.orderStateId}>
                    {state.orderStates && state.orderStates.map(x=>(
                        <option selected={x.id === state.item?.orderStateId} key={x.id!} value={x.id!}>{x.name}</option>
                    ))}                  
                </select>
            </div>

            <div className="form-group">
                <label>Select Order Type</label>
                <select className="form-control" name="orderType" ref={register} defaultValue={state.item?.orderType} >
                    {state.orderTypes && state.orderTypes.map(x=>(
                        <option selected={x.value === state.item?.orderType} key={x.value} value={x.value}>{x.label}</option>
                    ))}                  
                </select>
            </div>
            <button type="submit" className="btn btn-primary mb-2">Save</button>
            <button className="btn btn-secundary mb-2" onClick={e=>props.onSuccess()}>Cancel</button>
        </form>
    );
}