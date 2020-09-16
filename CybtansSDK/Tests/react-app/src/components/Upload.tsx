import * as React from 'react';
import { getService } from '../Api';
import { OrderService } from '../services/services';
import {
    UploadImageRequest, UploadImageResponse
} from '../services/models';
import { Alert } from 'reactstrap';

interface State {
    file?:File;
    url?:string;
    invalidated:boolean;
}

export default ()=>{
   let [state, setState] = React.useState<State>({invalidated:true});
   let service = getService(OrderService);

   React.useEffect(()=>{
      
   },[state.invalidated]);

   function onChange(e:React.ChangeEvent<HTMLInputElement>){
        if(state.url){
          URL.revokeObjectURL(state.url);
        }

        if(e.target.files){
            let file = e.target.files[0];            
            let url = URL.createObjectURL(file);
            setState({ ...state, url: url, file:file});
        }else{
            setState({ ...state, url: undefined, file:undefined});
        }
   }

   async function onSubmit(e:React.FormEvent<HTMLFormElement>){
       e.preventDefault();
       
       if(state.file){
            let response = await service.uploadImage({
                name : state.file.name,
                image : state.file,
                size :state.file?.size 
            });

            console.log(response.m5checksum);
            URL.revokeObjectURL(state.url!);

            setState({... state, url: undefined});

            alert(`Upload success, checksum: ${response.m5checksum}`);
        }
   }

  async function download(){
<<<<<<< HEAD
     let response = await service.downloadImage({name : 'react-image'});
     let imageBlob = await response.blob();       
=======
     let imageBlob = await service.downloadImage({name : 'react-image'});
>>>>>>> master
     if(imageBlob){
        let url = URL.createObjectURL(imageBlob);
        var a = document.createElement('a');
        a.href = url;
        a.target = '_blank';
<<<<<<< HEAD

        let contentDiposition = response.headers.get('Content-Disposition');
        if(contentDiposition){
            let name = contentDiposition.split(';').map(x=>x.trim()).find(x=>x.startsWith('filename='));
            if(name){
                a.download = name?.split('=')[1];            
            }
        }        

=======
        //a.download = "react-image.jpg";
>>>>>>> master
        document.body.appendChild(a);
        a.click();    
        a.remove();
     }
  }

   return (
    <>
    <h1>Upload Image</h1>    
    <div>
       {state.url && <img src={state.url} width="400px" height="320px" />} 
    </div>
    <form onSubmit={onSubmit}>
        <div className="custom-file mb-3">
            <input type="file" className="custom-file-input" id="customFile" name="filename" onChange={onChange} />
            <label className="custom-file-label">{state.file && state.file.name || 'Choose file'}</label>
        </div>              
        <button type="submit" className="btn btn-primary mb-2">Upload</button>
    </form>

    <button className="btn btn-primary" onClick={download}>Download</button>
    </>
   )
}