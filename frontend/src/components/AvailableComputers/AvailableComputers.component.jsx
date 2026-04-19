import toast from "react-hot-toast";
import React, { useCallback, useContext, useEffect, useState } from "react";
import { addToBasket, getBasketItems } from "../../basket/Basket";
import Table from "../common/Table.component";
import CheckError from "../common/CheckError";
import { BasketContext } from "../../context/BasketContext";
import { useApiClient } from "../../context/ApiClientContext";
import { useToken } from "../../context/TokenContext";

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

const AvailableComputers = () => {
  const [data, setData] = useState([]);
  const [hasMore, setHasMore] = useState(true);
  const [hasInitData, setHasInitData] = useState(false);
  const { setBasket } = useContext(BasketContext);
  const { apiClient } = useApiClient();
  const { setToken } = useToken();

  const fetchData = useCallback(
    async (page, toClear = false) => {
      try {
        const response = await apiClient.getAvailableComputers(page);
        if (response.ok) {
          if (response.data.elements.length === 0) {
            setHasMore(false);
            return false;
          }
          setHasInitData(true);

          setData((prevData) =>
            toClear
              ? response.data.elements
              : [...prevData, ...response.data.elements]
          );

          if (
            response.data.offset + response.data.pageSize >=
            response.data.maxCount
          ) {
            setHasMore(false);
            return false;
          }

          return true;
        }
      } catch (error) {
        CheckError(error, setToken);
      }
    },
    [apiClient]
  );

  useEffect(() => {
    if (!hasInitData) {
      fetchData(1, true);
    }
  }, [apiClient, setToken]);

  useEffect(() => {
    console.log(data);
  }, [data]);

  const HandleAddToBasket = useCallback((id) => {
    addToBasket(id);
    setBasket(getBasketItems());
    toast.success("Added!");
    //fetchData(1, true);
  }, []);

  return (
    <div>
      <Table
        fields={FIELDS}
        data={data}
        onClickButton={HandleAddToBasket}
        buttonText="Buy now"
        title="Shop"
        onClickMoreButton={hasMore ? (page) => fetchData(page) : null}
      />
    </div>
  );
};

export default AvailableComputers;
