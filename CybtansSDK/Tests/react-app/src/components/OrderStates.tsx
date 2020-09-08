import * as React from 'react';
import { getService } from '../Api';
import { OrderStateService } from '../services/services';
import {
    GetAllOrderStateResponse, OrderStateDto
} from '../services/models';

interface State {
    items?: OrderStateDto[]|null;
    invalidated:boolean;
}

export default ()=>{
   let [state, setState] = React.useState<State>({invalidated:true, items:[]});
   let service = getService(OrderStateService);

   async function fetchData(){
    let response = await service.getAll({});
    setState({ items: response.items, invalidated: false});
   }

   React.useEffect(()=>{
        if(state.invalidated === true){
            fetchData();         
        }
   },[state.invalidated]);

   return (
    <>
    <h1 id="tabelLabel">Order States</h1>    
    <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
            <tr>
                <th>Id</th>
                <th>Name</th>                
            </tr>
        </thead>
        <tbody>
            {state.items?.map(item =>
                <tr key={item.id}>
                    <td>{item.id}</td>
                    <td>{item.name}</td>            
                </tr>
                )
            }
        </tbody>
    </table>             
    </>
   )
}