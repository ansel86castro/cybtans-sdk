import { mat4 } from "gl-matrix";

export interface IAffector {
    readonly worldMtx: mat4;
}
