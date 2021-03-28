
export interface EventHandler<TSender, TArg>{
     (args:TArg, sender:TSender):boolean|undefined|void;
}

export default class EventEmitter<TSender, TArg>{
    private listeners:EventHandler<TSender, TArg>[] = [];

    addListener(handler:EventHandler<TSender, TArg>){
        this.listeners.push(handler);
    }   

    clear(){
        this.listeners = [];
    }

    raiseEvent(sender:TSender, arg:TArg){
        for (const handler of this.listeners) {
            let result = handler(arg, sender);
            if(result === true){
                return;  
            }
        }   
    }
}