import { Effect } from "./Effect";
import { EffectDto, EffectsManagerDto, ShaderProgramDto } from "./models";
import Program from "./Program";
import { HashMap } from "./utils";

export default class EffectManager {
    gl: WebGL2RenderingContext;
    private programDtos?: HashMap<ShaderProgramDto>;    
    private effectDtos?: HashMap<EffectDto>;
    private programs: HashMap<Program> = {};
    private effects: HashMap<Effect>={};
    private sources:HashMap<any> = {};  
    private effectStack: Effect[]=[];

    public currentProgram?:Program;
    public effect?:Effect;


    constructor(gl:WebGL2RenderingContext, data?:EffectsManagerDto){
        this.gl = gl;
        if(data){
            this.setPrograms(data);
        }
    }

    getProgramByName(name:string){
        return this.programs[name];
    }

    getEffectByName(name:string):Effect|undefined{
        return this.effects[name];
    }

    getProgramByType(type:Function):Program|undefined{
        if(!this.effect)
            return undefined;
        return this.effect.getProgram(type);
    }

    pushEffect(effect:Effect){
        this.effectStack.push(effect);
        this.effect = effect;
    }

    popEffect(){
        if(this.effectStack.length == 0) return;
        this.effect = this.effectStack.pop();
        return this.effect;
    }

    setSource(type: Function, value: any) {
        this.sources[type.name] = value;
    }
 
     getSource(type:string){
         return this.sources[type];
     }

    setPrograms(data:EffectsManagerDto){
        this.programDtos = data.programs || {};       
        this.effectDtos = data.effects || {}; 
        
        if(data.programs){
            for (const key of Object.keys(data.programs)) {
                let dto = data.programs[key];
                if(!dto.name) throw new Error("program name not specified");

                this.programs[dto.name] = new Program(this.gl, dto);
            }
        }
        if(data.effects){
            for (const key of Object.keys(data.effects)) {
                let dto = data.effects[key];
                this.effects[key] = new Effect(dto, this);
                if(key === 'Default'){
                    this.pushEffect(this.effects[key]);
                }
            }
        }
    }
}