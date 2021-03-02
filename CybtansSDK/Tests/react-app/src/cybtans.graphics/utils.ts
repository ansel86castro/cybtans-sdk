import { mat4, vec3 } from "gl-matrix";
import { float3, matrix } from "./MathUtils";

export interface HashMap<T>{
    [key:string]: T;
}

export function checkError(gl:WebGL2RenderingContext){
    const error = gl.getError();
    if(error !== gl.NO_ERROR){
        let msg = error.toString();
        switch(error){
            case gl.INVALID_ENUM:
                msg = 'INVALID_ENUM';
                break;
            case gl.INVALID_VALUE:
                msg = "INVALID_VALUE";
                break;
            case gl.INVALID_OPERATION:
                msg = "INVALID_OPERATION";
                break;
            case gl.INVALID_FRAMEBUFFER_OPERATION:
                msg = "INVALID_FRAMEBUFFER_OPERATION";
                break;
            case gl.OUT_OF_MEMORY:
                msg = "OUT_OF_MEMORY";
                break;
            case gl.CONTEXT_LOST_WEBGL:
                msg = "CONTEXT_LOST_WEBGL";
                break;
        }

        console.error(`GL Error ${msg}`);
        throw new Error(`GL Error r ${msg}`);        
    }

}
