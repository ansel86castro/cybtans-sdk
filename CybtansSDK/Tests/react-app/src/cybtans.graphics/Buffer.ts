import { decode } from "base64-arraybuffer";
import { VertexDefinitionDto, VertexElementFormat } from "./models";
import Program from "./Program";
import { checkError } from "./utils";


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

        // let floatArray= new Float32Array(this.arr);
        // let stride = 12;
        // let pos = [];
        // for (let i = 0; i < floatArray.length/stride; i++) {
        //     const element = floatArray[i*stride];
        //     pos.push(element);
             
        // }
        // console.log("Array Buffer "+pos.join(","));

        this.setData(this.arr);
    }

    setDataArray(arr:number[]){
        let byteArray = new Uint8Array(arr);
        this.setData(byteArray);
    }

    setDataString(str:string){
        var enc = new TextEncoder();
        let byteArray = enc.encode(str);
        this.setData(byteArray);
    }

    setData(data:ArrayBuffer){
        if(!this.glBuffer){
            this.glBuffer = this.gl.createBuffer();
            if (!this.glBuffer) {            
                throw new Error('Failed to create the buffer object');
            }
        }

        let bytes = new Int8Array(data);
        this.gl.bindBuffer(this.gl.ARRAY_BUFFER, this.glBuffer);

        checkError(this.gl);

        this.gl.bufferData(this.gl.ARRAY_BUFFER, bytes, this.gl.STATIC_DRAW);    
        checkError(this.gl);
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

        let gl = this.gl;                    
        this.gl.bindBuffer(gl.ARRAY_BUFFER, this.glBuffer);
        checkError(gl);
        
        if(this.vd.elements){
            let stride = this.vd.elements.length > 0 ? this.vd.size : 0;
            for (const e of this.vd.elements) {
                if(e.semantic == null) 
                   continue;

                let attr = program.getInput(e.semantic);              
                if(attr) {
                    gl.enableVertexAttribArray( attr.location );
                    gl.vertexAttribPointer( attr.location, e.size, this.getGlFormat(e.format), false, stride, e.offset);                        
                    checkError(gl);
                }
            }
        }
    }

    clearVertexBuffer(program:Program){
        let gl = this.gl;        
        this.gl.bindBuffer(gl.ARRAY_BUFFER, null);

        checkError(gl);

        if(this.vd.elements){            
            for (const e of this.vd.elements) {
                if(e.semantic == null) 
                   continue

                let attr = program.getInput(e.semantic);               
                if(attr) {
                    gl.disableVertexAttribArray( attr.location );                                        
                }
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
    constructor(gl:WebGL2RenderingContext, public sixteenBitsIndices:boolean){
        super(gl);
    }

    setData(data:ArrayBuffer){
        this.glBuffer = this.gl.createBuffer();
        if (!this.glBuffer) {          
            throw new Error('Failed to create the buffer object');
        }
        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.glBuffer);
        checkError(this.gl);

        let arr = this.sixteenBitsIndices === true ? 
                    new Uint16Array(data) :
                    new Uint32Array(data);

        this.gl.bufferData(this.gl.ELEMENT_ARRAY_BUFFER, arr, this.gl.STATIC_DRAW);    
        checkError(this.gl);
    }

    setIndexBuffer(){
        if(!this.glBuffer){            
            throw new Error('Buffer object not created');
        }

        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, this.glBuffer);
        checkError(this.gl);
    }

    clearIndexBuffer(program:Program){
        if(!this.glBuffer){            
            throw new Error('Buffer object not created');
        }


        this.gl.bindBuffer(this.gl.ELEMENT_ARRAY_BUFFER, null);
    }

}