import Program from "./Program";
import SceneManager from "./SceneManager";
import { HashMap } from "./utils";

interface ProgramSelector {
    (type:Function, sceneMgr:SceneManager):Program;
}

enum Operator {
    Exist = 1,
    Equal = 2,
    NotEqual = 3,
    LessThan = 4,
    GratherThan = 5
}

class Condition {
    selector: string;
    op:Operator;
    value?: any;

    resolver?: (target:any)=>any; 
}

class ProgramSelectorBuilder{
    condition:Condition;

    constructor(condition: Condition){
        this.condition= condition;
    }

    selector(type:Function, sceneMgr:SceneManager):Program {
        
    }
}


export default class Technique {
    selectors:HashMap<ProgramSelector> = {};

    getProgram(type:Function, sceneMgr:SceneManager): Program | null{
        let selector = this.selectors[type.name];
        if(!selector) return null;

        return selector(type, sceneMgr);
    }
}