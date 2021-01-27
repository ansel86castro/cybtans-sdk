import { decode } from "base64-arraybuffer";
import { VertexDefinitionDto, VertexElementFormat } from "./models";
import Program from "./Program";


export class ByteBuffer{
    arr?:ArrayBuffer;
    glBuffer:WebGLBuffer|null;
    gl:WebGL2RenderingContext;
   

    constructor(gl:WebGL2RenderingContext){
        this.gl = gl;
        this.glBuffer = null;    
    }

    setDataBase64(base64:string){
        this.arr = decode(base64);
        this.setData(this.arr);
    }

    setData(data:ArrayBuffer){
        this.glBuffer = this.gl.createBuffer();
        if (!this.glBuffer) {            
            throw new Error('Failed to create the buffer object');
        }
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.glBuffer);
        this.gl.bufferData(this.gl.ARRAY_BUFFER, data, this.gl.STATIC_DRAW);    
    }

    dispose(){
        if(this.glBuffer){
            this.gl.deleteBuffer(this.glBuffer);
        }
    }
}

export class VertexBuffer extends ByteBuffer{
    vd: VertexDefinitionDto;
    constructor( gl:WebGL2RenderingContext, vd:VertexDefinitionDto){
        super(gl);
        
        this.vd = vd;
    }

    setVertexBuffer(program: Program){
        if(!this.glBuffer){           
            throw new Error('Buffer object not created');
        }
        
        let glProgram = program.glProgram;
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.glBuffer);
        
        if(this.vd.elements){
            let stride = this.vd.elements.length > 0 ? this.vd.size : 0;
            for (const e of this.vd.elements) {
                if(e.semantic == null) 
                    throw new Error("vertex attribute semantic can not be null");

                let attr = program.getInput(e.semantic);
                var attrLoc = this.gl.getAttribLocation( glProgram, attr);
                this.gl.enableVertexAttribArray( attrLoc );
                this.gl.vertexAttribPointer( attrLoc, e.size, this.getGlFormat(e.format), false, stride, e.offset);    
            }
        }
    }

    getGlFormat(format:VertexElementFormat): number{
        switch(format){
            case VertexElementFormat.ubyte:return this.gl.UNSIGNED_BYTE;
            case VertexElementFormat.byte: return this.gl.BYTE;            
            case VertexElementFormat.float: return this.gl.FLOAT;
            case VertexElementFormat.uint: return this.gl.UNSIGNED_INT;
            case VertexElementFormat.int: return this.gl.INT;
            case VertexElementFormat.ushort: return this.gl.UNSIGNED_SHORT;
            case VertexElementFormat.short: return this.gl.SHORT; 
            default:
                throw new Error('Format not supported');
        }
    }
}

export class IndexBuffer extends ByteBuffer{
    constructor(gl:WebGL2RenderingContext){
        super(gl);
    }

    setData(data:ArrayBuffer){
        this.glBuffer = this.gl.createBuffer();
        if (!this.glBuffer) {          
            throw new Error('Failed to create the buffer object');
        }
        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.glBuffer);
        this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER, data, this.gl.STATIC_DRAW);    
    }

    setIndexBuffer(){
        if(!this.glBuffer){            
            throw new Error('Buffer object not created');
        }
        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.glBuffer);
    }
}