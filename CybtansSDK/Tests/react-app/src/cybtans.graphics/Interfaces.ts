import  SceneManager  from "./SceneManager";

export interface IRenderable {
    visible:boolean;
    render(context:SceneManager): void;
}