import { validateLocaleAndSetLanguage } from "typescript";
import { ShaderDto, ShaderProgramDto } from "./models";
import { HashMap } from "./utils";

export type UniformTypes = 'float'|'float2'|'float3'|'float4'|'matrix'|'mat4'|'sampler1D'|'sampler2D'|'samplerCUBE';

interface UniformSlot {
    name:string;
    target?: string|null;
    property?: string|null;
    type?: UniformTypes|null;
    path?: string|null;

    location?:WebGLUniformLocation | null;
    resolver?: (target:any)=>any;
    samplerSlot:number;
}

interface UniformSource {
    sourceType:string;
    parameters: UniformSlot[];
}


export default class Program {
    gl:WebGL2RenderingContext;
    glProgram:WebGLProgram;

    inputs: HashMap<string> = {};    
    sources?: HashMap<UniformSource>;

    vertexShader: WebGLShader | null;
    fragmentShader: WebGLShader | null;
    samplerSlotUsed :number = 0;

    constructor(gl:WebGL2RenderingContext, data?:ShaderProgramDto){
        this.gl = gl;
        let glProgram = gl.createProgram();
        if(!glProgram){
            console.error('Failed to create the program object');
            throw new Error('Failed to create the program object');
        }

        this.glProgram = glProgram;
        this.vertexShader = null;
        this.fragmentShader = null;
        
        if(data){
            if(data.vertexShader){
                this.inputs = data.vertexShader.inputs || {};
                this.createSources(data.vertexShader);
                if(data.vertexShader.source)
                    this.loadShader('vertex', data.vertexShader.source);
            }
            if(data.fragmentShader){
                this.createSources(data.fragmentShader);
                if(data.fragmentShader.source)
                    this.loadShader('fragment', data.fragmentShader.source);
            }

            if(this.vertexShader || this.fragmentShader){
                this.linkProgram();

                if(this.sources){
                    for (const key in this.sources) {
                        if (this.sources.hasOwnProperty(key)) {
                            this.loadUniformLocations(key);                        
                        }
                    }
                }
            }
        }
    }    

    private createSources(shader:ShaderDto){
        if(shader?.parameters){
            let parameters = shader.parameters
            for (const key in parameters) {
                if (parameters.hasOwnProperty(key)) {
                    const value = parameters[key];
                    if(!value.target) continue;
                    
                    let p:UniformSlot = {name: key, ...value};
                    if(p.path){
                        p.resolver = this.createResolver(p.path);
                    }
                    
                    this.sources = this.sources || {};
                    let source = this.sources[value.target]
                    if(!source){
                        source = { sourceType: value.target, parameters: [] }
                        this.sources[value.target] = source;
                    }
                    source.parameters.push(p);
                }
            }
        }
    }

    private createResolver(path:string){
        let s = path.split(".");
        return function(t:any){
            let result = t;
            for (let i = 0; i < s.length; i++) {
                const k = s[i];
                result = result[k];
            }
            return result;
        }
    }

    private loadUniformLocations(target:string){
        if(!this.sources) return;

        const gl = this.gl;
        const program = this.glProgram;

        let source = this.sources[target];
        for (let i = 0; i < source.parameters.length; i++) {
            const p = source.parameters[i]; 
            p.location =  gl.getUniformLocation(program, p.name);
            if(p.type === 'sampler2D' || p.type === 'sampler1D' || p.type === 'samplerCUBE'){
                p.samplerSlot = this.samplerSlotUsed++;
            }
        }
    }


    getInput(semantic:string){
        return this.inputs[semantic];
    }

    loadShader(type:'vertex'|'fragment', source:string){    

        let gl = this.gl;
        let shader:WebGLShader|null;

        switch(type){
            case 'vertex':
                shader = gl.createShader(gl.VERTEX_SHADER);    
                this.vertexShader = shader;            
                break;
            case 'fragment':
                shader = gl.createShader(gl.FRAGMENT_SHADER);
                this.fragmentShader = shader;
                break;
        }

        if(!shader){
            console.error(`Can not create shader ${type}`);
            throw Error(`Can not create shader ${type}`)
        }

        // Put the source code into the gl shader object
        gl.shaderSource(shader, source);

        // Compile the shader code
        gl.compileShader(shader);

        // Check for any compiler errors
        var compiled = gl.getShaderParameter(shader, gl.COMPILE_STATUS);
        if (!compiled) {
            // There are errors, so display them
            var errors = gl.getShaderInfoLog(shader);
            console.error('Failed to compile ' + type + ' with these errors:' + errors);
            gl.deleteShader(shader);
            throw Error('Failed to compile ' + type + ' with these errors:' + errors);
        }

        // Attach the shader objects
        gl.attachShader(this.glProgram, shader);     
    }

    linkProgram(){
        // Link the WebGLProgram object
        let gl = this.gl;
        gl.linkProgram(this.glProgram);

        // Check for success
        let linked = gl.getProgramParameter(this.glProgram, gl.LINK_STATUS);
        if (!linked) {
            // There were errors, so get the errors and display them.
            var error = gl.getProgramInfoLog(this.glProgram);
            console.error('Fatal error: Failed to link program: ' + error);
            if(this.vertexShader){
                gl.detachShader(this.glProgram, this.vertexShader);
                gl.detachShader(this.glProgram, this.vertexShader);               
            }
            if(this.fragmentShader){
                gl.deleteShader(this.vertexShader);
                gl.deleteShader(this.fragmentShader);
            }
            
            throw new Error('Fatal error: Failed to link program: ' + error);
        }   
    }


}