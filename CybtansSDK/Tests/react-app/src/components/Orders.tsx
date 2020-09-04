import * as React from 'react';
import { getService } from '../Api';
import { OrderService } from '../services/services';
import {
    GetAllOrderResponse, OrderDto
} from '../services/models';

interface OrderState {
    items: OrderDto[]|null;
    invalidated:boolean;
}

export default class Orders extends React.PureComponent<{}, OrderState> {

    service: OrderService;

    constructor(props: {}) {
        super(props);

        this.state = { invalidated:true, items:[] }
        this.service = getService(OrderService);
    }

    // This method is called when the component is first added to the document
    public componentDidMount() {
        if(this.state.invalidated === false)
            return;
        this.ensureDataFetched();
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
        if(this.state.invalidated === false)
            return;

        this.ensureDataFetched();
    }

    public render() {
        return (
            <React.Fragment>
                <h1 id="tabelLabel">Orders</h1>
                <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
                {this.renderTable()}                
            </React.Fragment>
        );
    }

    private async ensureDataFetched(): Promise<void> {
        let response = await this.service.getAll({ take: 50 });

        this.setState({ items: response.items, invalidated: false });
    }

    private renderTable() {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
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
                            <td>{item.id}</td>
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

}
