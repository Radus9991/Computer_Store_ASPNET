import './App.css';
import Navbar from './components/Navbar/Navbar.component';
import Content from './components/Content/Content.component';
import { BrowserRouter } from 'react-router-dom';
import { Toaster } from 'react-hot-toast'
import { useEffect, useState } from 'react';
import { TokenContext } from './context/TokenContext';
import { getBasketItems } from './basket/Basket';
import { BasketContext } from './context/BasketContext';
import { ApiClientContext } from './context/ApiClientContext';
import ApiClient from './api/ApiClient';

function App() {
  const [token, setToken] = useState(localStorage.getItem("token"))
  const [basket, setBasket] = useState(getBasketItems())
  const [apiClient, setApiClient] = useState(null)

  const setTokenWithLocalStorage = (newToken) => {
    setToken(newToken)
    if (newToken == null) {
      localStorage.removeItem("token")
    } else {
      localStorage.setItem("token", newToken)
    }
  }

  useEffect(() => {
    setApiClient(new ApiClient(token))
  }, [token])


  return (
    <div>
      <BrowserRouter>
        <TokenContext.Provider value={{ token: token, setToken: setTokenWithLocalStorage }}>
          <ApiClientContext.Provider value={{apiClient: apiClient}}>
            <BasketContext.Provider value={{basket: basket, setBasket: setBasket}}>
              <Navbar />
              <Content />
            </BasketContext.Provider>
          </ApiClientContext.Provider>
        </TokenContext.Provider>

        <Toaster
          position="top-center"
          reverseOrder={false}
        />
      </BrowserRouter>
    </div>
  );
}

export default App;
