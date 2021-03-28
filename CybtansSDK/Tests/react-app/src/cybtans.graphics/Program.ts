import { validateLocaleAndSetLanguage } from "typescript";
import { ShaderDto, ShaderProgramDto } from "./models";
import SceneManager from "./SceneManager";
import Texture from "./Textures";
import { checkError, HashMap } from "./utils";

export type UniformTypes = 'float' | 'float2' | 'float3' | 'float4' | 'mat4' | 'sampler1D' | 'sampler2D' | 'samplerCUBE' | 'none';

export interface UniformSlot {
    name: string;
    target: string;
    property: string;
    type: UniformTypes;
    path?: string | null;

    location?: WebGLUniformLocation | null;
    resolver?: (target: any) => any;
    samplerSlot?: number;
}

interface AttributeLocation {
    name: string;
    semantic: string;
    location: number;
}

interface UniformSource {
    sourceType: string;
    parameters: UniformSlot[];
}


type UniformSetter = {
    [k in UniformTypes]: (gl: WebGL2RenderingContext, slot: UniformSlot, v: any) => void;
}

const setter: UniformSetter = {
    float: (gl, s, v) => gl.uniform1f(s.location!, v),
    float2: (gl, s, v) => gl.uniform2fv(s.location!, v),
    float3: (gl, s, v) => gl.uniform3fv(s.location!, v),
    float4: (gl, s, v) => gl.uniform4fv(s.location!, v),
    mat4: (gl, s, v) => gl.uniformMatrix4fv(s.location!, false, v),
    sampler1D: (gl, s, v) => setTexture(gl, s, v),
    sampler2D: (gl, s, v) => setTexture(gl, s, v),
    samplerCUBE: (gl, s, v) => setTexture(gl, s, v),
    none: (gl, s, v) => { }
}

function setTexture(gl: WebGL2RenderingContext, slot: UniformSlot, texture: Texture) {
    if (!slot.location || slot.samplerSlot == undefined) return;

    //activate slot
    gl.activeTexture(gl.TEXTURE0 + slot.samplerSlot);

    //bind texture
    texture.setTexture(slot.samplerSlot);

    // Tell the shader we bound the texture to texture unit 0
    gl.uniform1i(slot.location, slot.samplerSlot);
}

export default class Program {
    gl: WebGL2RenderingContext;
    glProgram: WebGLProgram;

    name: string;
    inputs: HashMap<AttributeLocation> = {};
    sources?: HashMap<UniformSource>;
    uniformSlots: UniformSource[] = [];
    parameters: HashMap<UniformSlot> = {};

    vertexShader: WebGLShader | null;
    fragmentShader: WebGLShader | null;
    samplerSlotUsed: number = 0;
    textures: Texture[];

    constructor(gl: WebGL2RenderingContext, data?: ShaderProgramDto) {
        this.gl = gl;
        let glProgram = gl.createProgram();
        if (!glProgram) {
            console.error('Failed to create the program object');
            throw new Error('Failed to create the program object');
        }

        this.glProgram = glProgram;
        this.vertexShader = null;
        this.fragmentShader = null;

        if (data) {
            this.name = data.name;

            if (data.vertexShader) {
                this.createSources(data.vertexShader);
                if (data.vertexShader.source)
                    this.loadShader('vertex', data.vertexShader.source);
            }
            if (data.fragmentShader) {
                this.createSources(data.fragmentShader);
                if (data.fragmentShader.source)
                    this.loadShader('fragment', data.fragmentShader.source);
            }

            if (this.vertexShader || this.fragmentShader) {
                this.linkProgram();

                this.LoadAttributes(data, gl, glProgram);

                if (this.sources) {
                    for (const key in this.sources) {
                        if (this.sources.hasOwnProperty(key)) {
                            this.loadUniformLocations(key);
                        }
                    }
                }
            }
        }

        this.textures = new Array(this.samplerSlotUsed);
    }

    private LoadAttributes(data: ShaderProgramDto, gl: WebGL2RenderingContext, glProgram: WebGLProgram) {
        if (data.vertexShader?.inputs) {
            for (const key in data.vertexShader.inputs) {
                if (Object.prototype.hasOwnProperty.call(data.vertexShader.inputs, key)) {
                    const name = data.vertexShader.inputs[key];
                    let location = gl.getAttribLocation(glProgram, name);
                    if (location >= 0) {
                        this.inputs[key] = { name: name, semantic: key, location };
                    }
                }
            }
        }
    }

    private createSources(shader: ShaderDto) {
        if (shader?.parameters) {
            let parameters = shader.parameters
            for (const key in parameters) {
                if (parameters.hasOwnProperty(key)) {
                    const value = parameters[key];
                    if (!value.target) continue;

                    let p: UniformSlot = { name: key, ...value } as UniformSlot;
                    if (p.path) {
                        p.resolver = this.createResolver(p.path, p.property);
                    }

                    this.sources = this.sources || {};
                    let source = this.sources[value.target]

                    if (!source) {
                        source = { sourceType: value.target, parameters: [] }
                        this.sources[value.target] = source;
                        this.uniformSlots.push(source);
                    }

                    source.parameters.push(p);
                    this.parameters[p.name] = p;
                }
            }
        }
    }

    private createResolver(path: string, property: string) {
        let s = path.split(".");
        return function (t: any) {
            let result = t;
            if (!result)
                return undefined;

            for (let i = 0; i < s.length; i++) {
                const k = s[i];
                result = result[k];
            }
            return result && result[property];
        }
    }

    private loadUniformLocations(target: string) {
        if (!this.sources) return;

        const gl = this.gl;
        const program = this.glProgram;

        let source = this.sources[target];
        for (let i = 0; i < source.parameters.length; i++) {
            const p = source.parameters[i];
            if (p.type === 'none')
                continue;

            p.location = gl.getUniformLocation(program, p.name);
            if (p.location) {
                if (p.type === 'sampler2D' || p.type === 'sampler1D' || p.type === 'samplerCUBE') {
                    p.samplerSlot = this.samplerSlotUsed++;
                }
            } else {
                console.log(`uniform not used ${p.name}`);
            }
        }
    }

    getInput(semantic: string) {
        return this.inputs[semantic];
    }

    loadShader(type: 'vertex' | 'fragment', source: string) {

        let gl = this.gl;
        let shader: WebGLShader | null;

        switch (type) {
            case 'vertex':
                shader = gl.createShader(gl.VERTEX_SHADER);
                this.vertexShader = shader;
                break;
            case 'fragment':
                shader = gl.createShader(gl.FRAGMENT_SHADER);
                this.fragmentShader = shader;
                break;
        }

        if (!shader) {
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
    }

    linkProgram() {
        // Link the WebGLProgram object
        let gl = this.gl;

        // Attach the shader objects
        if (this.vertexShader) {
            gl.attachShader(this.glProgram, this.vertexShader);
        }
        if (this.fragmentShader) {
            gl.attachShader(this.glProgram, this.fragmentShader);
        }

        checkError(gl);

        gl.linkProgram(this.glProgram);

        // Check for success
        let linked = gl.getProgramParameter(this.glProgram, gl.LINK_STATUS);

        if (!linked) {
            // There were errors, so get the errors and display them.
            var error = gl.getProgramInfoLog(this.glProgram);
            console.error('Fatal error: Failed to link program: ' + error);
            if (this.vertexShader) {
                gl.detachShader(this.glProgram, this.vertexShader);
                gl.detachShader(this.glProgram, this.vertexShader);
            }
            if (this.fragmentShader) {
                gl.deleteShader(this.vertexShader);
                gl.deleteShader(this.fragmentShader);
            }

            throw new Error('Fatal error: Failed to link program: ' + error);
        }
    }

    useProgram(ctx: SceneManager) {

        const gl = this.gl;
        const program = this.glProgram;
        gl.useProgram(program);

        const len = this.uniformSlots.length;
        for (let i = 0; i < len; i++) {
            const source = this.uniformSlots[i];

            const target = ctx.getSource(source.sourceType);
            if (!target) continue;

            for (let j = 0; j < source.parameters.length; j++) {
                const p = source.parameters[j];
                if (!p.location)
                    continue;

                let value = p.resolver ? p.resolver(target) : target[p.property];
                if (!value) continue;

                let func = setter[p.type];
                if (!func) continue;

                func(gl, p, value);

                if (p.samplerSlot) {
                    this.textures[p.samplerSlot] = value;
                }

                checkError(gl);
            }
        }
    }

    bindSource(sourceType: string, ctx: SceneManager) {
        if (!this.sources) return;

        const source = this.sources[sourceType];
        if (!source) return;

        const target = ctx.getSource(source.sourceType);

        if (!target) return;

        const gl = this.gl;

        if (this != ctx.program) {
            this.useProgram(ctx);
            return;
        }

        for (let j = 0; j < source.parameters.length; j++) {
            const p = source.parameters[j];
            if (!p.location) {
                continue;
            }

            let value = p.resolver ? p.resolver(target) : target[p.property];
            if (!value) continue;

            let func = setter[p.type];
            if (!func) continue;

            func(gl, p, value);

            if (p.samplerSlot) {
                this.textures[p.samplerSlot] = value;
            }

            checkError(gl);
        }
    }

    clearSamplers(sourceType: string) {
        if (!this.sources) return;
        const source = this.sources[sourceType];
        if (!source) return;

        const gl = this.gl;

        for (let j = 0; j < source.parameters.length; j++) {
            const p = source.parameters[j];
            if (!p.location) {
                continue;
            }

            if (p.samplerSlot) {
                let texture = this.textures[p.samplerSlot];
                if (texture) {
                    //activate slot
                    gl.activeTexture(gl.TEXTURE0 + p.samplerSlot);
                    //disable texture
                    texture.disable(p.samplerSlot);

                    this.textures[p.samplerSlot] = null;
                }
            }
        }
    }

    dispose() {
        if (this.glProgram) {
            this.gl.detachShader(this.glProgram, this.vertexShader);
            this.gl.detachShader(this.glProgram, this.fragmentShader);

            this.gl.deleteShader(this.vertexShader);
            this.gl.deleteShader(this.fragmentShader);

            this.gl.deleteProgram(this.glProgram);
        }
    }
}