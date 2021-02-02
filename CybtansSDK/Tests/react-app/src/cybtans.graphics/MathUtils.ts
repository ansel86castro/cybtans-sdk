import { mat4, vec3 } from "gl-matrix";

export function transformNormal(out:vec3, a:vec3, m:mat4) {
    var x = a[0],
        y = a[1],
        z = a[2];
    out[0] = (m[0] * x + m[4] * y + m[8] * z + m[12]);
    out[1] = (m[1] * x + m[5] * y + m[9] * z + m[13]);
    out[2] = (m[2] * x + m[6] * y + m[10] * z + m[14]);
    return out;
}

export function matrix(list?:number[]|null){
    if(!list){
        let m =  new Float32Array(16);
        mat4.identity(m);
        return m;
    }
    if(list.length != 16) throw new Error("invalid matrix");

    return new Float32Array(list);
}

export function float4(list?:number[]|null){
    if(!list){
        let m =  new Float32Array(4);
        m[3] = 1;
        return m;
    }
    if(list.length != 4) throw new Error("invalid vector4");
    return new Float32Array(list);
}


export function float3(list?:number[]|null){
    if(!list){
        return new Float32Array(3);               
    }
    if(list.length != 3) throw new Error("invalid vector3");
    return new Float32Array(list);
}

export function float2(list?:number[]|null){
    if(!list){
        return new Float32Array(2);               
    }
    if(list.length != 2) throw new Error("invalid vector2");
    return new Float32Array(list);
}

export function decomposeMatrix(m:mat4):[vec3, vec3, mat4]{
    const r:vec3 = float3([m[0], m[1], m[2]]);
    const u:vec3 = float3([m[4], m[5], m[6]]);
    const f:vec3 = float3([m[8], m[9], m[10]]);
    const t:vec3 = float3([m[12], m[13], m[14]]);

    let rn:vec3 = vec3.normalize(float3(), r);
    let un:vec3 = vec3.normalize(float3(), u);
    let fn:vec3 = vec3.normalize(float3(), f);

    let orientation:mat4 =matrix([
        r[0],r[1],r[2],0,
        u[0],u[1],u[2],0,
        f[0],f[1],f[2],0,
        0   ,0   ,0   ,1
    ]);

    let scale:vec3 = [vec3.len(r), vec3.len(u), vec3.len(f)];

    return [scale, t, orientation];
}

export function sphericalToCartesian(phi:number, theta:number, radius:number): vec3{
    var b = radius * Math.sin(phi);
    return  [b * Math.sin(theta), radius * Math.cos(phi), b * Math.cos(theta)];
}

export function raySphereIntersect(center:vec3, radius:number, rayPos:vec3, rayDir:vec3){
   
   var t1 = 0;
   var t2 = 0;
   var e = vec3.subtract(vec3.create(), rayPos, center);
   var a = vec3.dot(rayDir, rayDir);
   var b = 2 *  vec3.dot(e, rayDir);
   var c = vec3.dot(e, e) - radius * radius;

   var d = b * b - 4 * a * c;
   if (d < 0)
       return false;
   if (d == 0){
       t1 = t2 = -b / 2 * a;
       return t1 > 0;
   }
   var k = Math.sqrt(d);
   var a2 = 2 * a;
   t1 = (-b - k) / a2;
   t2 = (-b + k) / a2;

   return t1 > 0 || t2 > 0;
}

export class Spherical{
    constructor(public theta:number, public phi:number){

    }

    toCartesian(){
        var b = Math.sin(this.theta);
        return [b * Math.sin(this.phi), Math.cos(this.theta), b * Math.cos(this.phi)]; 
    }
}
