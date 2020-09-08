import * as React from 'react';
import { getService } from '../Api';
import { OrderService, CustomerService } from '../services/services';
import {
    GetAllOrderResponse, OrderDto, GetCustomerRequest, OrderTypeEnum
} from '../services/models';
import {debounce } from 'debounce';
import EditOrder from './EditOrder';
import { createRef } from 'react';

interface OrderState {
    items?: OrderDto[]|null;
    invalidated:boolean;
    search?: string;
    edit:boolean;
    selected?: string;
}

export default class Orders extends React.PureComponent<{}, OrderState> {

    editPanelRef: React.RefObject<HTMLDivElement>;
    service: OrderService;

    constructor(props: {}) {
        super(props);

        this.editPanelRef = createRef<HTMLDivElement>();
        this.state = { invalidated:true, items:[] ,search:'', edit:false};

        this.service = getService(OrderService);
        this.Search = debounce(this.Search.bind(this), 500);
    }

    // This method is called when the component is first added to the document
    public componentDidMount() {
        if(this.state.invalidated === false)
            return;
        this.ensureDataFetched();
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
        if(this.state.edit === true && this.editPanelRef.current ){
            this.editPanelRef.current.scrollIntoView({
                behavior:'smooth',
                block:'nearest'
              });
        }
        if(this.state.invalidated === false)
            return;
        this.ensureDataFetched();
    }

    public render() {
        return (
            <React.Fragment>
                <h1 id="tabelLabel">Orders</h1>
                <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
                {this.renderSearch()}                  
                {this.renderTable()} 
                <div ref={this.editPanelRef}>
                {   this.state.edit && 
                    <EditOrder id={this.state.selected} onSuccess={()=> this.setState({edit:false, invalidated:true})} />
                }    
                </div>                       
            </React.Fragment>
        );
    }

    private async ensureDataFetched(): Promise<void> {
        let response = await this.service.getAll({ 
            filter: this.state.search && 
            `description like '%${this.state.search}%' 
            or customer.name like'${this.state.search}%' 
            or orderState.name like '${this.state.search}%'`, 
            take: 50 
        });
        this.setState({ items: response.items, invalidated: false });
    }

    private editItem(item:OrderDto){
        this.setState({edit:true, selected: item.id});
    }

    private renderTable() {
        return (
            <table className='table table-striped table-hover' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Description</th>
                        <th>Customer</th>
                        <th>Customer Profile</th>
                        <th>State</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.items?.map(item =>
                        <tr key={item.id}>
                            <td><a href='void();' onClick={e=> { e.preventDefault(); this.editItem(item)}}>{item.id}</a></td>
                            <td>{item.description}</td>
                    <td>{item.customer?.name} {item.customer?.firstLastName} {item.customer?.secondLastName}</td>
                            <td>{item.customer?.customerProfile?.name}</td>
                            <td>{item.orderState?.name}</td>                            
                        </tr>)
                    }
                </tbody>
            </table>
        );
    }

    onChangeSearch = (e:React.ChangeEvent<HTMLInputElement>)=>{
        this.setState({search: e.target.value});    

        this.Search();            
    }

    private Search() {
        this.setState({invalidated: true});
    }

    private renderSearch(){
        
        return (
            <div className="action-bar">
                <div>
                    <button className="btn btn-primary" onClick={e=> this.setState({edit:true})}>Create Order</button>
                </div>
                <div className="search-box">
                    <label>Search:</label>
                    <input value={this.state.search} className="form-control tb-search" onChange={ this.onChangeSearch }/>               
                </div>
            </div>
        );
    }    

    private async createNewOrder(){

       let customerService = getService(CustomerService);
       let customer = await customerService.getAll({
           filter:"name eq 'Jane'"
       });

       let response = await this.service.create({
            description:' Created Order',
            orderStateId : 1,
            customerId: customer.items && customer.items[0].id || undefined,
            orderType: OrderTypeEnum.normal
        });

        this.setState({invalidated: true});
    }
}
