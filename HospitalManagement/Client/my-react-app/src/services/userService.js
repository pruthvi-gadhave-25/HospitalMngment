import { data } from "react-router-dom";
import baseApi from "../api/axios"


export const  register  =  async (data) => {
    return  await baseApi.post("auth/register" ,data);
}  

export const login  = async (data) => {
    return   await baseApi.post("auth/login",data);
}

 