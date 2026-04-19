import { createContext, useContext } from "react";

export const ApiClientContext = createContext({apiClient: null})


export const useApiClient = () => {
    return useContext(ApiClientContext)
}
