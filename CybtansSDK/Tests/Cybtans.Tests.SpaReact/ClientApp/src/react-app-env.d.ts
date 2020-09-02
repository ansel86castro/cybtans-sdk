/// <reference types="react-scripts" />

declare namespace NodeJS {
    // Merge the existing `ProcessEnv` definition with ours
    // https://www.typescriptlang.org/docs/handbook/declaration-merging.html#merging-interfaces

    export interface ProcessEnv {        
        readonly API_URL: string;        
    }
}