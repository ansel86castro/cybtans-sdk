import * as React from 'react';
import { connect } from 'react-redux';
import { getService } from '../api';
import { OrderService } from '../services/services';
import {
    GetAllOrderResponse
} from '../services/models';

interface OrderState {
    ordersResponse?: GetAllOrderResponse;
}

export default class Orders extends React.PureComponent<{}, OrderState> {

    service: OrderService;

    constructor(props: {}) {
        super(props);

        this.service = getService(OrderService);
    }

    // This method is called when the component is first added to the document
    public componentDidMount() {
        this.ensureDataFetched();
    }

    // This method is called when the route parameters change
    public componentDidUpdate() {
        this.ensureDataFetched();
    }

    public render() {
        return (
            <React.Fragment>
                <h1 id="tabelLabel">Weather forecast</h1>
                <p>This component demonstrates fetching data from the server and working with URL parameters.</p>
                {this.renderTable()}                
            </React.Fragment>
        );
    }

    private async ensureDataFetched(): Promise<void> {
        let response = await this.service.getAll({ take: 50 });
        this.setState({ ordersResponse: response });
    }

    private renderTable() {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Id</th>
                        <th>Description</th>
                        <th>Customer</th>
                        <th>State</th>
                    </tr>
                </thead>
                <tbody>
                    {this.state.ordersResponse?.items.map(item =>
                        <tr key={item.id}>
                            <td>{item.description}</td>
                            <td>{item.customer?.firstLastName}</td>
                            <td>{item.orderState?.name}</td>                            
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

}
