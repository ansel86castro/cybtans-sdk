import React, { useEffect, useState } from 'react';
import logo from './logo.svg';
import './App.css';
import { CustomerService } from './services';
import { Product } from './model';

function App() {  
  let [product, setProduct] = useState<Product|undefined|null>(null);
  let [loaded, setLoaded] = useState(false);

  async function load(srv:CustomerService){
    try{
      await srv.getCustomerNoReturn({id: 1});
    }catch(error){
      console.log(error);
    }

    let p = await srv.getCustomer({  id: 7});
    console.log(p);        
  }

  useEffect(()=>{      
      let srv = new CustomerService(fetch.bind(window), {  baseUrl: 'http://localhost:5000' });
      srv.getCustomer({  id: 7}).then(
        product=>{          
          setProduct(product);         
        },
        error=>{
          console.log(error);        
        });

        srv.getCustomers({pageSize:50, sort:'Name'})
        .then(response=>{
            console.log(JSON.stringify(response));
        },error=>{
            console.log(error);      
        });

        srv.getCustomerNoReturn({id: 1})
        .then(
          data=> {
             console.log(data)
          },
          error=>{
            console.log(error);
          });

         load(srv).then(()=>{
           console.log('done');
         });
         
        setLoaded(true);
  },[loaded]);

  return (
    <div className="App">
      <header className="App-header">
        <img src={logo} className="App-logo" alt="logo" />
        <p>
          Edit <code>src/App.tsx</code> and save to reload.
        </p>
        <a
          className="App-link"
          href="https://reactjs.org"
          target="_blank"
          rel="noopener noreferrer"
        >
          Learn React
        </a>
      </header>

      {product && product.name}

    </div>
  );
}

export default App;
