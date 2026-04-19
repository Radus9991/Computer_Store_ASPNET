import { useContext, useEffect, useState } from "react";
import {
  clearBasket,
  getBasketItems,
  removeFromBasket,
} from "../../basket/Basket";
import URLS from "../../api/Api";
import Table from "../common/Table.component";
import CheckError from "../common/CheckError";
import { TokenContext, useToken } from "../../context/TokenContext";
import { BasketContext } from "../../context/BasketContext";
import PayPalAllButtons from "../PayPal/PayPalAllButtons.component";
import { post } from "../../api/Requests";
import "./BasketList.component.css";
import { useApiClient } from "../../context/ApiClientContext";
import toast from "react-hot-toast";
import { useNavigate } from "react-router-dom";

const BasketList = () => {
  const FIELDS = [
    { id: "mainboard", name: "mainboard", label: "Mainboard", type: "text" },
    { id: "gpu", name: "gpu", label: "GPU", type: "text" },
    { id: "cpu", name: "cpu", label: "CPU", type: "text" },
    { id: "ram", name: "ram", label: "Ram", type: "text" },
    { id: "ssd", name: "ssd", label: "SSD", type: "text" },
    {
      id: "powerSupply",
      name: "powerSupply",
      label: "Power Supply",
      type: "text",
    },
    { id: "cooler", name: "cooler", label: "Cooler", type: "text" },
    { id: "case", name: "case", label: "Case", type: "text" },
    { id: "price", name: "price", label: "Price", type: "text" },
  ];

  const [data, setData] = useState([]);

  const { token, setToken } = useToken();
  const { basket, setBasket } = useContext(BasketContext);
  const { apiClient } = useApiClient();
  const [ successPayemnt, setSuccessPayment ] = useState(false);
  const navigate = useNavigate();

  const getComputers = async () => {
    try {
      if (apiClient == null) {
        return;
      }

      const response = await apiClient.getSelectedComputers(basket);

      if (response.ok) {
        setData(response.data);
      }
    } catch (error) {
      CheckError(error, setToken);
    }
  };

  const HandleRemoveFromBasket = (id) => {
    removeFromBasket(id);
    setBasket(getBasketItems());
  };

  useEffect(() => {
    getComputers();
  }, [basket, apiClient]);

  const createOrder = async () => {
    const { ok, data, error } = await post(URLS.CreateOrder, basket, token);

    if (ok) {
      return data.paypalId;
    } else {
      CheckError(error, setToken);
    }
  };

  const onApprove = async (body) => {
    const { data, ok, error } = await apiClient.approvePayment(body.orderID);
    if (ok) {
      toast.success("Buy successful");
      clearBasket();
      setBasket(getBasketItems());
      setSuccessPayment(true);
      console.log("JESTESMY W IFIE!");
      return data;
    } else {
      CheckError(error, setToken);
    }
  };

  useEffect(() => {
    if (successPayemnt) {
      navigate("/orders");
    }
  }, [successPayemnt]);

  return (
    <div>
      <Table
        fields={FIELDS}
        data={data}
        title="Items in the basket"
        onClickButton={HandleRemoveFromBasket}
        buttonText="Trash"
      />

      <div className="pp-center">
        <PayPalAllButtons createOrder={createOrder} onApprove={onApprove} />
      </div>
    </div>
  );
};

export default BasketList;
