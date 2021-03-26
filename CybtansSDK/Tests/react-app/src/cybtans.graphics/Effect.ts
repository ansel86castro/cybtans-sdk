import { convertTypeAcquisitionFromJson } from "typescript";
import EffectManager from "./EffectManager";
import { EffectDto, ParameterPredicateDto, PredicateProgramDto } from "./models";
import Program, { UniformSlot } from "./Program";
import SceneManager from "./SceneManager";
import { HashMap } from "./utils";

export class Effect {
    name:string;
    predicates:HashMap<PredicateProgram[]> = {};
    programsForTypes: HashMap<Program>={};
     
    constructor(dto:EffectDto, manager:EffectManager){
        this.name = dto.name;
        
        for (const key of Object.keys(dto.programs)) {            
            this.programsForTypes[key] = manager.getProgramByName(dto.programs[key]);
        }

        for(const key of Object.keys(dto.predicates)){
            this.predicates[key] = dto.predicates[key].items!.map(x=> new PredicateProgram(x, manager));
            
        }
    }

    getProgram(type:Function){
        let predicates = this.predicates[type.name];
        if(predicates){
            for (const item of predicates) {
                if(item.eval()){
                    return item.program;
                }
            }
        }
        return this.programsForTypes[type.name];       
    }

}

export class PredicateProgram {
    andConditions?:ParameterPredicate[];
    orConditions?:ParameterPredicate[];
    condition?:ParameterPredicate;
    program:Program;
    manager:EffectManager;

    constructor(dto:PredicateProgramDto, manager:EffectManager){
        this.program = manager.getProgramByName(dto.program);
        this.manager = manager;

        if(dto.condition){
            this.condition = new ParameterPredicate(dto.condition, this.program);
        }else{
            if(dto.andConditions){
                this.andConditions = dto.andConditions.map(x=> new ParameterPredicate(x, this.program));
            }
            if(dto.orConditions){
                this.orConditions = dto.orConditions.map(x=> new ParameterPredicate(x, this.program));
            }
        }        
    }

    eval(){
        if(this.condition){
            return this.condition.eval(this.manager);
        }else if(this.andConditions){
            for (const item of this.andConditions) {
                if(item.eval(this.manager) === false)
                    return false;
            }
            return true;
        }else if(this.orConditions){
            for (const item of this.orConditions) {
                if(item.eval(this.manager) === true)
                    return true;
            }
            return false;
        }
    }
}

enum PredicateOperator{
    isActive = 1,
    Equal = 2,
    NotEqual = 3,
    LessThan = 4,
    GreaterThan = 5,
    LessThanEqual = 6,
    GreatherThanEqual = 7
}

export class ParameterPredicate {
    op:PredicateOperator;
    parameter: UniformSlot;
    value:any;

    constructor(dto:ParameterPredicateDto, program:Program){
        this.op = dto.op;
        this.parameter = program.parameters[dto.parameter];
        this.value = dto.value;
    }

    eval(manager:EffectManager):boolean {
        if(!this.parameter.location)
            return false;

        let source = manager.getSource(this.parameter.target);
        let v = this.getValue(source);

        switch(this.op){
            case PredicateOperator.isActive:
                return v !== undefined && v !== null
            case PredicateOperator.Equal:
                return  v === this.value;
            case PredicateOperator.NotEqual:
                return v !== this.value;
            case PredicateOperator.GreaterThan:
                return v && v > this.value;
            case PredicateOperator.GreatherThanEqual:
                return v && v >= this.value;
            case PredicateOperator.LessThan:
                return v && v < this.value;
            case PredicateOperator.LessThanEqual:
                return v && v <= this.value;            
        }
    }

    getValue(source:any){
        return  this.parameter.resolver ? this.parameter.resolver(source) 
        : source[this.parameter.property];
    }

}