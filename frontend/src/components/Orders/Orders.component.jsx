import "./Orders.component.css";
import React, { useCallback, useEffect, useState } from "react";
import { useToken } from "../../context/TokenContext";
import { useApiClient } from "../../context/ApiClientContext";
import CheckError from "../common/CheckError";
import Table from "../common/Table.component";
import Select from "../common/Select.component";
import { startSignalRConnection, stopSignalRConnection } from "../common/SignalR.component";

//Fields
const PAYMENT_STATUSES = {
  0: "PENDING",
  1: "SUCCESS",
  2: "CANCEL",
  3: "FAIL",
};
const FIELDS = [
  { id: "id", name: "id", label: "Order ID", type: "text" },
  { id: "paypalId", name: "paypalId", label: "Paypal ID", type: "text" },
  { id: "totalAmount", name: "totalAmount", label: "Price", type: "text" },
  { id: "paymentStatus", name: "paymentStatus", label: "Status", type: "text" },
  { id: "date", name: "date", label: "Date", type: "text" },
];

const PC_FIELDS = [
  { id: "mainboard", name: "mainboard", label: "Mainboard", type: "text" },
  { id: "gpu", name: "gpu", label: "GPU", type: "text" },
  { id: "cpu", name: "cpu", label: "CPU", type: "text" },
  { id: "ram", name: "ram", label: "Ram", type: "text" },
  { id: "ssd", name: "ssd", label: "SSD", type: "text" },
  { id: "powerSupply", name: "powerSupply", label: "Power Supply", type: "text" },
  { id: "cooler", name: "cooler", label: "Cooler", type: "text" },
  { id: "case", name: "case", label: "Case", type: "text" },
  { id: "price", name: "price", label: "Price", type: "text" },
];


// Helper function for date formatting
const formatDate = (dateString) => {
  if (!dateString) return "";
  const date = new Date(dateString);
  return date.toLocaleString("de-DE", { timeZone: "Europe/Berlin" });
};

const Orders = () => {

  //Hooks
  const { apiClient } = useApiClient();
  const { setToken } = useToken();
  const [orderDetails, setOrderDetails] = useState([]);
  const [pcDetails, setPcDetails] = useState([]);
  const [selectedOrderId, setSelectedOrderId] = useState(null);
  const [orderList, setOrderList] = useState([]);


  // Fetch data methods
  const fetchOrdersList = async () => {
    if (!apiClient) {
      return;
    }

    const { data, ok, error } = await apiClient.getUserOrdersAllIds();

    if (ok) {
      const adjustedData = (data.elements || []).map((e) => ({
        ...e,
        date: formatDate(e.date),
      }));
      setOrderList(adjustedData);
    } else {
      CheckError(error, setToken);
    }
  };

  const fetchPCDetails = useCallback(
    async (page, toClear = false) => {
      if (!selectedOrderId || !apiClient) {
        return;
      }

      setPcDetails([]);

      const { data, ok, error } = await apiClient.getOrderComputers(selectedOrderId, 1); // TODO: Only fetch page 1 !!!

      if (ok) {
        setPcDetails(data.elements || []);
      } else {
        CheckError(error, setToken);
      }
    }, [selectedOrderId, apiClient, setToken]);


  //Effects
  useEffect(() => {
    // fetchOrders(1, true);
    fetchOrdersList();

    const con = startSignalRConnection(() => fetchOrdersList());

    return () => stopSignalRConnection(con)
  }, [apiClient]);

  useEffect(() => {
    if (!selectedOrderId) {
      return;
    }

    const fetchOrderDetails = async () => {
      const { data, ok, error } = await apiClient.getOrder(selectedOrderId);

      if (ok) {
        const adjustedData = {
          ...data,
          paymentStatus: PAYMENT_STATUSES[data.paymentStatus],
          date: formatDate(data.date),
          totalAmount: `${data.totalAmount} €`,
        };
        setOrderDetails([adjustedData]);
        fetchPCDetails();
      } else {
        CheckError(error, setToken);
      }
    };

    fetchOrderDetails();
  }, [selectedOrderId, apiClient, fetchPCDetails, setToken]);

  //General Methods
  const handleOrderChange = async (e) => {
    setSelectedOrderId(e.target.value);
    setPcDetails([]);
  };

  const orderOptions = orderList.map(({ id, date }) => ({
    label: `${id} (${date})`,
    value: id,
  }));

  return (
    <div>
      <Select
        options={orderOptions}
        value={selectedOrderId}
        onChange={handleOrderChange}
        text="Select Order ID"
      />

      {selectedOrderId && (
        <>
          <Table
            fields={FIELDS}
            data={orderDetails}
            title="Order Details"
          />
          <Table
            fields={PC_FIELDS}
            data={pcDetails}
            title="Ordered Computers"
          />
        </>
      )}
    </div>
  );
};

export default Orders;
