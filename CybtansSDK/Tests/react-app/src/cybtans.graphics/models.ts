

export enum ProjectionType {
  perspective = 1,
  orthographic = 2,
}



export interface CameraDto {
  projType: ProjectionType;
  name?: string|null;
  nearPlane: number;
  farPlane: number;
  fieldOfView: number;
  aspectRatio: number;
  width: number;
  height: number;
  localMatrix?: number[]|null;
  id: string;
}


export interface AmbientLightDto {
  ambientColor?: number[]|null;
  skyColor?: number[]|null;
  groundColor?: number[]|null;
  intensity: number;
  northPole?: number[]|null;
}



export enum LightType {
  none = 0,
  directional = 1,
  point = 2,
  spot = 3,
}



export interface LightDto {
  diffuse?: number[]|null;
  specular?: number[]|null;
  ambient?: number[]|null;
  attenuation?: number[]|null;
  enable: boolean;
  intensity: number;
  spotPower: number;
  type: LightType;
  range: number;
  id: string;
}


export interface VertexDefinitionDto {
  size: number;
  elements?: VertexElementDto[]|null;
}


export interface VertexElementDto {
  offset: number;
  stream: number;
  semantic?: string|null;
  usageIndex: number;
  format: VertexElementFormat;
  size: number;
}



export enum VertexElementFormat {
  unused = 0,
  byte = 1,
  ubyte = 2,
  short = 3,
  ushort = 4,
  int = 5,
  uint = 6,
  float = 7,
  double = 8,
}




export enum MeshPrimitive {
  pointList = 1,
  lineList = 2,
  lineStrip = 3,
  triangleList = 4,
  triangleStrip = 5,
  triangleFan = 6,
}



export interface MeshDto {
  id: string;
  materialSlots?: string[]|null;
  adjacency?: number[]|null;
  vertexCount: number;
  faceCount: number;
  vertexDeclaration: VertexDefinitionDto;
  vertexBuffer?: number[]|null;
  indexBuffer?: number[]|null;
  primitive: MeshPrimitive;
  layers: MeshPartDto[]|null;
  name?: string|null;
  sixteenBitsIndices: boolean;
}


export interface MeshPartDto {
  materialIndex: number;
  layerId: number;
  startIndex: number;
  primitiveCount: number;
  startVertex: number;
  vertexCount: number;
  indexCount: number;
}


export interface MeshSkinDto {
  id: string;
  bones?: string[]|null;
  mesh: string;
  bindShapeMatrix?: number[]|null;
  boneBindingMatrices?: number[]|null;
  layerBones?: MeshLayerBonesDto[]|null;
  rootBone?: FrameDto|null;
}


export interface MeshLayerBonesDto {
  layerIndex: number;
  bones: number[]|null;
}



export enum TextureType {
  none = 0,
  texture2d = 1,
  texture3d = 2,
  cubeMap = 3,
}



export interface CubeMapDto {
  positiveX?: string|null;
  negativeX?: string|null;
  positiveY?: string|null;
  negativeY?: string|null;
  positiveZ?: string|null;
  negativeZ?: string|null;
}


export interface TextureDto {
  filename?: string|null;
  type: TextureType;
  format?: string|null;
  id: string;
  cubeMap?: CubeMapDto|null;
}


export interface MaterialDto {
  name?: string|null;
  diffuse: number[]|null;
  specular: number[]|null;
  emissive: number[]|null;
  specularPower: number;
  textures?: { [key:string]: string }|null;
  id: string;
  reflectivity: number;
  refractivity: number;
  perograms?: { [key:string]: string }|null;
}



export enum Uomtype {
  meters = 0,
  kilometers = 1,
  foot = 2,
  miles = 3,
  centimeters = 4,
  milimeters = 5,
  inches = 6,
  parsec = 7,
  lightYear = 8,
}




export enum FrameType {
  frame = 0,
  bone = 1,
  root = 2,
}



export interface FrameDto {
  id: string;
  name?: string|null;
  localTransform: number[]|null;
  bindParentTransform: number[]|null;
  bindAffectorTransform: number[]|null;
  worldTransform: number[]|null;
  parentId?: string|null;
  type: FrameType;
  range: number;
  childrens?: FrameDto[]|null;
  bindTargetId?: string|null;
  tag?: string|null;
  component?: FrameComponentDto|null;
}


export interface FrameComponentDto {
  camera?: string|null;
  light?: FrameLightDto|null;
  mesh?: FrameMeshDto|null;
  meshSkin?: FrameMeshSkinDto|null;
}


export interface FrameLightDto {
  light: string;
  localPosition?: number[]|null;
  localDirection?: number[]|null;
}


export interface FrameMeshDto {
  mesh: string;
  materials?: string[]|null;
}


export interface FrameMeshSkinDto {
  meshSkin: string;
  materials?: string[]|null;
}


export interface SceneDto {
  id: string;
  name?: string|null;
  units: number;
  materials?: MaterialDto[]|null;
  cameras?: CameraDto[]|null;
  lights?: LightDto[]|null;
  meshes?: MeshDto[]|null;
  skins?: MeshSkinDto[]|null;
  textures?: TextureDto[]|null;
  root?: FrameDto|null;
  unitOfMeasure: Uomtype;
  currentCamera?: string|null;
  ambient?: AmbientLightDto|null;
}


export interface ShaderDto {
  inputs?: { [key:string]: string }|null;
  parameters?: { [key:string]: ShaderParameterDto }|null;
  source: string;
}


export interface ShaderParameterDto {
  target: string;
  property: string;
  type: string;
  path?: string|null;
}


export interface ShaderProgramDto {
  name: string;
  vertexShader: ShaderDto;
  fragmentShader: ShaderDto;
}


export interface EffectDto {
  name: string;
  predicates: { [key:string]: PredicateProgramList };
  programs: { [key:string]: string };
}


export interface PredicateProgramList {
  items?: PredicateProgramDto[]|null;
}


export interface PredicateProgramDto {
  andConditions?: ParameterPredicateDto[]|null;
  orConditions?: ParameterPredicateDto[]|null;
  condition?: ParameterPredicateDto|null;
  program: string;
}


export interface ParameterPredicateDto {
  op: number;
  parameter: string;
  value?: any|null;
}


export interface EffectsManagerDto {
  programs: { [key:string]: ShaderProgramDto };
  effects: { [key:string]: EffectDto };
}
