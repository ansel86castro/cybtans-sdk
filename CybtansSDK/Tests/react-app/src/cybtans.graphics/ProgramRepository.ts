import { ShaderProgramCollection, ShaderProgramDto } from "./models";
import Program from "./Program";
import { HashMap } from "./utils";

export default class ProgramRepository {
    programDtos?: ShaderProgramDto[]|null;
    defaultProgram?: string|null;
    programs: HashMap<Program> = {};
    gl: WebGL2RenderingContext;

    constructor(gl:WebGL2RenderingContext, data:ShaderProgramCollection){
        this.programDtos = data.programs;
        this.gl = gl;
        this.defaultProgram = data.defaultProgram;
        if(data.programs){
            for (const dto of data.programs) {
                if(!dto.name) throw new Error("program name not specified");
                this.programs[dto.name] = new Program(gl, dto);
            }
        }
    }
}