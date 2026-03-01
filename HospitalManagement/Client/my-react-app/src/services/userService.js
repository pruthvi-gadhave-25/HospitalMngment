import { data } from "react-router-dom";
import baseApi from "../api/axios"


export const  register  =  (data) => {
    return  baseApi.post("auth/register" ,data);
}  

export const login  =(data) => {
    return baseApi.post("auth/login",data);
}