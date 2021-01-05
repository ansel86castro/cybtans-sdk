import * as React from 'react';
import * as signalR from "@microsoft/signalr";
import { env } from 'process';

export default function Realtime(){
    let [messages, setMessages] = React.useState<string[]>([]);

    let connection:signalR.HubConnection;
    connection = new signalR.HubConnectionBuilder()
        .withUrl(`${process.env.REACT_APP_API_URL}/ordershub`)
        .build();

    connection.on('ReceiveMessage', (msg:string)=>{
        setMessages([...messages, msg]);
    });

    React.useEffect(()=>{
       
        connection.start().catch(err=>{
            console.error(err);
        });

       return ()=>{
        connection.stop().catch(err=>{
            console.error(err);
        });
       }
    });

    return (
        <div>
            <h2>Messages from server</h2>
            <MessageList messages ={messages} />
        </div>
    );
}

function MessageList(props:{messages:string[]}){
    let {messages} = props;
    return (
    <>
     { messages.map((msg, idx) => 
        <div key={idx}>
            {msg}
        </div>)
    }
    </>)
}