import { ShaderProgramCollection, ShaderProgramDto } from "./models";
import Program from "./Program";
import { HashMap } from "./utils";

export default class ProgramRepository {
    programDtos?: ShaderProgramDto[]|null;
    defaultProgram: string = '';
    programs: HashMap<Program> = {};
    gl: WebGL2RenderingContext;

    constructor(gl:WebGL2RenderingContext, data?:ShaderProgramCollection){
        this.gl = gl;
        if(data){
            this.setPrograms(data);
        }
    }

    setPrograms(data:ShaderProgramCollection){
        this.programDtos = data.programs;        
        this.defaultProgram = data.defaultProgram || '';
        if(data.programs){
            for (const dto of data.programs) {
                if(!dto.name) throw new Error("program name not specified");

                if(this.defaultProgram === '')
                    this.defaultProgram = dto.name;

                this.programs[dto.name] = new Program(this.gl, dto);
            }
        }
    }
}