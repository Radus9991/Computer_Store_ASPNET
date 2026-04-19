import { createContext, useContext } from "react";

export const TokenContext = createContext({token: null, setToken: null})


export const useToken = () => {
    return useContext(TokenContext)
}