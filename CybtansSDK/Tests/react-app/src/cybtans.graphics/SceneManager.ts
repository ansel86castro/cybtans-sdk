import { vec2, vec4 } from "gl-matrix";
import Camera from "./Camera";
import { float4 } from "./MathUtils";
import { ShaderProgramCollection } from "./models";
import Program from "./Program";
import ProgramRepository from "./ProgramRepository";
import Scene from "./Scene";
import { HashMap } from "./utils";

interface UpdateHandler {
    (elapsed:number):void;
} 

export default class SceneManager {   
    scenes:Scene[]=[];
    programs:ProgramRepository;
    current?:Scene;
    updateHandler?: UpdateHandler;
    elapsed:number = 0;
    private _program?:Program;
    gl:WebGL2RenderingContext;
    backColor:vec4 = float4([0.3,0.3,0.3,1]);
    width:number;
    height:number

    private lastTime?:number;
    private running:boolean = false;
    private sources:HashMap<any> = { };   

    constructor(gl:WebGL2RenderingContext, width:number, height:number){
        this.programs = new ProgramRepository(gl);  
        this.onFrame  = this.onFrame.bind(this);
        this.gl = gl;
        this.width = width;
        this.height = height;
    }

    get program(){
        return this._program;
    }

    private onFrame(timestamp:DOMHighResTimeStamp){
      if(this.running === false) return;

      if (this.lastTime === undefined)
        this.lastTime = timestamp;

     this.elapsed = timestamp - this.lastTime;

      if(this.updateHandler){
          this.updateHandler(this.elapsed);
      }

      if(this.current){
          this.current.update(this.elapsed);
      }

      this.render();

      this.lastTime = timestamp;
      
      requestAnimationFrame(this.onFrame);

    }

    private render(){
        const gl = this.gl;
        const color = this.backColor;

        gl.viewport(0,0, this.width, this.height);
        gl.clearColor(color[0], color[1], color[2], color[3]);  // Clear to black, fully opaque
        gl.clearDepth(1.0);                 // Clear everything
        gl.enable(gl.DEPTH_TEST);           // Enable depth testing
        gl.depthFunc(gl.LEQUAL);            // Near things obscure far things

        // Clear the canvas before we start drawing on it.

        gl.clear(gl.COLOR_BUFFER_BIT | gl.DEPTH_BUFFER_BIT);

        //gl.disable(gl.CULL_FACE);
        //gl.cullFace(gl.BACK);
        
        if(this.current == null) return;  

        this.current.render(this);
    }


    start(){
        this.running = true;
        requestAnimationFrame(this.onFrame);
    }

    stop(){
        this.running = false;    
    }

    programSource(type: Function, value: any) {
       this.sources[type.name] = value;
    }

    getSource(type:string){
        return this.sources[type];
    }

    setCurrent(sceneName:string){
        let scene = this.scenes.find(x=>x.name === sceneName);
        if(scene){
            this.current = scene;
        }
    }

    async loadScene(url:string, baseTextureUrl:string) {        
        let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};        
        var response =  await  fetch(url, options);

        let dto = await response.json();
        
        try{
            let scene = new Scene(this, dto.id, dto.name);
            scene.loadData(dto);    
            this.scenes.push(scene);
            await scene.load(baseTextureUrl);

            if(this.scenes.length == 1){
                this.current = scene;
            }
            
            return scene;
        }catch(e){
            console.error(e);
            throw e;
        }
    }

    async loadPrograms(url:string){        
        let options:RequestInit = { method: 'GET', headers: { Accept: 'application/json' }};        
        var response =  await  fetch(url, options);
        
        let dto = await response.json();
        
        this.programs.setPrograms(dto);
        this._program = this.programs.programs[this.programs.defaultProgram];

    }

}