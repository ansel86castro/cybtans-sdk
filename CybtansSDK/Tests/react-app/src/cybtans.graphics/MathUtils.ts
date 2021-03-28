import { mat4, quat, ReadonlyMat4, vec3 } from "gl-matrix";

export const UnitX = float3([1, 0, 0]);
export const UnitY = float3([0, 1, 0]);
export const UnitZ = float3([0, 0, 1]);
export const PIover2 = Math.PI * 0.5;
export const TwoPI = Math.PI * 2;
export const Epsilon = 0.000005;

export function transformNormal(out: vec3, a: vec3, m: mat4) {
    var x = a[0],
        y = a[1],
        z = a[2];
    out[0] = (m[0] * x + m[4] * y + m[8] * z);
    out[1] = (m[1] * x + m[5] * y + m[9] * z);
    out[2] = (m[2] * x + m[6] * y + m[10] * z);
    return out;
}

export function matrix(list?: number[] | null) {
    if (!list) {
        let m = new Float32Array(16);
        mat4.identity(m);
        return m;
    }
    if (list.length != 16) throw new Error("invalid matrix");

    return new Float32Array(list);
}

export function float4(list?: number[] | null) {
    if (!list) {
        let m = new Float32Array(4);
        m[3] = 1;
        return m;
    }
    if (list.length != 4) throw new Error("invalid vector4");
    return new Float32Array(list);
}


export function float3(list?: number[] | null) {
    if (!list) {
        return new Float32Array(3);
    }
    if (list.length != 3) throw new Error("invalid vector3");
    return new Float32Array(list);
}

export function float2(list?: number[] | null) {
    if (!list) {
        return new Float32Array(2);
    }
    if (list.length != 2) throw new Error("invalid vector2");
    return new Float32Array(list);
}

export function decomposeMatrix(m: mat4): [vec3, vec3, mat4] {
    const r: vec3 = float3([m[0], m[1], m[2]]);
    const u: vec3 = float3([m[4], m[5], m[6]]);
    const f: vec3 = float3([m[8], m[9], m[10]]);
    const t: vec3 = float3([m[12], m[13], m[14]]);

    let scale: vec3 = float3([vec3.len(r), vec3.len(u), vec3.len(f)]);

    let rn: vec3 = vec3.normalize(r, r);
    let un: vec3 = vec3.normalize(u, u);
    let fn: vec3 = vec3.normalize(f, f);

    let orientation: mat4 = matrix([
        rn[0], rn[1], rn[2], 0,
        un[0], un[1], un[2], 0,
        fn[0], fn[1], fn[2], 0,
        0, 0, 0, 1
    ]);

    return [scale, t, orientation];
}


export function getRotationAxis(outU: vec3, outF: vec3, outR: vec3, m: ReadonlyMat4) {
    outR[0] = m[0];
    outR[1] = m[1];
    outR[2] = m[2];

    outU[0] = m[4];
    outU[1] = m[5];
    outU[2] = m[6];

    outF[0] = m[8];
    outF[1] = m[9];
    outF[2] = m[10];
}

export function getRotationAxisNormalized(outU: vec3, outF: vec3, outR: vec3, m: ReadonlyMat4) {
    outR[0] = m[0];
    outR[1] = m[1];
    outR[2] = m[2];

    outU[0] = m[4];
    outU[1] = m[5];
    outU[2] = m[6];

    outF[0] = m[8];
    outF[1] = m[9];
    outF[2] = m[10];

    vec3.normalize(outR, outR);
    vec3.normalize(outF, outF);
    vec3.normalize(outU, outU);
}


export function sphericalToCartesian(phi: number, theta: number, radius: number): vec3 {
    var b = radius * Math.sin(phi);
    return [b * Math.sin(theta), radius * Math.cos(phi), b * Math.cos(theta)];
}

export function raySphereIntersect(center: vec3, radius: number, rayPos: vec3, rayDir: vec3) {

    var t1 = 0;
    var t2 = 0;
    var e = vec3.subtract(vec3.create(), rayPos, center);
    var a = vec3.dot(rayDir, rayDir);
    var b = 2 * vec3.dot(e, rayDir);
    var c = vec3.dot(e, e) - radius * radius;

    var d = b * b - 4 * a * c;
    if (d < 0)
        return false;
    if (d == 0) {
        t1 = t2 = -b / 2 * a;
        return t1 > 0;
    }
    var k = Math.sqrt(d);
    var a2 = 2 * a;
    t1 = (-b - k) / a2;
    t2 = (-b + k) / a2;

    return t1 > 0 || t2 > 0;
}

export class Spherical {
    constructor(public theta: number, public phi: number) {

    }

    toCartesian() {
        var b = Math.sin(this.theta);
        return [b * Math.sin(this.phi), Math.cos(this.theta), b * Math.cos(this.phi)];
    }
}

export function toRadians(grade: number) {
    return (Math.PI * 2) * (grade / 360);
}

export type euler = Float32Array;

export function eulerFromAxis(out: euler, right: vec3, up: vec3, front: vec3) {
    //check the case when we are looking up or down
    let a = vec3.dot(front, UnitY);

    if (a < -0.9999) {
        //looking down
        out[2] = 0;
        out[1] = PIover2;
        out[0] = Math.atan2(up[0], up[2]);
    }
    else if (a > 0.9999) {
        //looking down
        out[2] = 0;
        out[1] = -PIover2;
        out[0] = Math.atan2(up[0], up[2]);
    }
    else {
        let x = front[0], y = front[1], z = front[2];
        let ux = up[0], uy = up[1], uz = up[2];

        let yaw = 0, pitch = 0, roll = 0;

        //Projection of Front on the XZ Plane
        front[1] = 0;
        vec3.normalize(front, front);

        if (front[0] < Epsilon && front[0] > -Epsilon)
            yaw = 0;
        else
            yaw = Math.atan2(front[0], front[2]);

        pitch = -Math.asin(y);

        out[0] = right[0];
        out[1] = right[1];
        out[2] = right[2];

        //Projection of Right on the XZ Plane
        vec3.cross(right, UnitY, front);
        vec3.normalize(right, right);

        front[0] = x;
        front[1] = y;
        front[2] = z;
        vec3.cross(up, front, right);

        x = right[0];
        y = right[1];
        z = right[2];

        right[0] = out[0];
        right[1] = out[1];
        right[2] = out[2];
        let a = vec3.dot(right, up);

        out[0] = x;
        out[1] = y;
        out[2] = z;
        let b = vec3.dot(right, out);

        roll = Math.atan2(a, b);

        up[0] = ux;
        up[1] = uy;
        up[2] = uz;

        out[0] = yaw;
        out[1] = pitch;
        out[2] = roll;
    }

}


export function matRotationYawPitchRoll(out: mat4, yaw: number, pitch: number, roll: number) {
    let num7 = roll * 0.5;
    let num = Math.sin(num7);
    let num2 = Math.cos(num7);
    let num8 = pitch * 0.5;
    let num3 = Math.sin(num8);
    let num4 = Math.cos(num8);
    let num9 = yaw * 0.5;
    let num5 = Math.sin(num9);
    let num6 = Math.cos(num9);

    //quaternion
    let qx = ((num6 * num3) * num2) + ((num5 * num4) * num);
    let qy = ((num5 * num4) * num2) - ((num6 * num3) * num);
    let qz = ((num6 * num4) * num) - ((num5 * num3) * num2);
    let qw = ((num6 * num4) * num2) + ((num5 * num3) * num);

    //matrix
    matFromQuaterion(out, qx, qy, qz, qw);
}


export function matFromQuaterion(out: mat4, qx: number, qy: number, qz: number, qw: number) {
    let xx = (qx * qx);
    let yy = (qy * qy);
    let zz = (qz * qz);
    let xy = qy * qx;
    let zw = qw * qz;
    let zx = qz * qx;
    let yw = qw * qy;
    let yz = qz * qy;
    let xw = qw * qx;
    out[0] = (1.0 - ((zz + yy) * 2.0));
    out[1] = ((zw + xy) * 2.0);
    out[2] = ((zx - yw) * 2.0);
    out[3] = 0;
    out[4] = ((xy - zw) * 2.0);
    out[5] = (1.0 - ((zz + xx) * 2.0));
    out[6] = ((xw + yz) * 2.0);
    out[7] = 0;
    out[8] = ((yw + zx) * 2.0);
    out[9] = ((yz - xw) * 2.0);
    out[10] = (1.0 - ((yy + xx) * 2.0));
    out[11] = 0;
    out[12] = 0;
    out[13] = 0;
    out[14] = 0;
    out[15] = 1;
}

export function quaternionFromMat(quaternion: quat, matrix: mat4): quat {
    let num2 = 0;
    let num3 = 0;
    let num = matrix[0] + matrix[5] + matrix[10];
    if (num > 0) {
        num2 = Math.sqrt(num + 1);
        quaternion[3] = num2 * 0.5;
        num2 = 0.5 / num2;
        quaternion[0] = (matrix[6] - matrix[9]) * num2;
        quaternion[1] = (matrix[8] - matrix[2]) * num2;
        quaternion[2] = (matrix[1] - matrix[4]) * num2;
        return quaternion;
    }
    if ((matrix[0] >= matrix[5]) && (matrix[0] >= matrix[10])) {
        num2 = Math.sqrt((((1 + matrix[0]) - matrix[5]) - matrix[10]));
        num3 = 0.5 / num2;
        quaternion[0] = 0.5 * num2;
        quaternion[1] = (matrix[1] + matrix[4]) * num3;
        quaternion[2] = (matrix[2] + matrix[8]) * num3;
        quaternion[3] = (matrix[6] - matrix[9]) * num3;
        return quaternion;
    }
    if (matrix[5] > matrix[10]) {
        num2 = Math.sqrt((((1 + matrix[5]) - matrix[0]) - matrix[10]));
        num3 = 0.5 / num2;
        quaternion[0] = (matrix[4] + matrix[1]) * num3;
        quaternion[1] = 0.5 * num2;
        quaternion[2] = (matrix[9] + matrix[6]) * num3;
        quaternion[3] = (matrix[8] - matrix[2]) * num3;
        return quaternion;
    }

    num2 = Math.sqrt((((1 + matrix[10]) - matrix[0]) - matrix[5]));
    num3 = 0.5 / num2;
    quaternion[0] = (matrix[8] + matrix[2]) * num3;
    quaternion[1] = (matrix[9] + matrix[6]) * num3;
    quaternion[2] = 0.5 * num2;
    quaternion[3] = (matrix[1] - matrix[4]) * num3;
    return quaternion;
}


export function matTranslate(out: mat4, a: mat4, x: number, y: number, z: number) {
    var a00, a01, a02, a03;
    var a10, a11, a12, a13;
    var a20, a21, a22, a23;

    if (a === out) {
        out[12] = a[0] * x + a[4] * y + a[8] * z + a[12];
        out[13] = a[1] * x + a[5] * y + a[9] * z + a[13];
        out[14] = a[2] * x + a[6] * y + a[10] * z + a[14];
        out[15] = a[3] * x + a[7] * y + a[11] * z + a[15];
    } else {
        a00 = a[0];
        a01 = a[1];
        a02 = a[2];
        a03 = a[3];
        a10 = a[4];
        a11 = a[5];
        a12 = a[6];
        a13 = a[7];
        a20 = a[8];
        a21 = a[9];
        a22 = a[10];
        a23 = a[11];
        out[0] = a00;
        out[1] = a01;
        out[2] = a02;
        out[3] = a03;
        out[4] = a10;
        out[5] = a11;
        out[6] = a12;
        out[7] = a13;
        out[8] = a20;
        out[9] = a21;
        out[10] = a22;
        out[11] = a23;
        out[12] = a00 * x + a10 * y + a20 * z + a[12];
        out[13] = a01 * x + a11 * y + a21 * z + a[13];
        out[14] = a02 * x + a12 * y + a22 * z + a[14];
        out[15] = a03 * x + a13 * y + a23 * z + a[15];
    }

    return out;
}

export const eulerUtils = {
    normalizeHeading(value: number) {
        let absValue = Math.abs(value);
        let cicles = absValue / TwoPI;
        let floorCicles = Math.floor(cicles);
        let frag = cicles - floorCicles;

        if (floorCicles > 0) {
            value = Math.sign(value) * TwoPI * frag;
        }
        return value;
    },

    normalizePith(value: number) {
        if (value > PIover2) {
            value = PIover2;
        }
        else if (value < -PIover2) {
            value = -PIover2;
        }
        return value;
    }
}

export interface Size {
    width: number;
    height: number;
}
