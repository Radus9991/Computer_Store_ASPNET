export const BASE_URL = "https://localhost:7241";

const URLS = {
  //Post
  Register: BASE_URL + "/api/User",
  Login: BASE_URL + "/api/Auth",
  AddPC: BASE_URL + "/api/Computer",
  SelectedComputers: BASE_URL + "/api/Computer/basket",
  CreateOrder: BASE_URL + "/api/Order/create",
  OnApprove: BASE_URL + "/api/Order/approve",
  //Get
  AvailableComputers: BASE_URL + "/api/Computer/available/{pageIndex}",
  OrderComputers: BASE_URL + "/api/Order/{orderId}/computers/{pageIndex}",
  UserOrders: BASE_URL + "/api/Order/{pageIndex}",
  GetBasketCount: BASE_URL + "/api/Computer/count",
  GetUserRole: BASE_URL + "/api/User/",
  GetUserOrdersAllIds: BASE_URL + "/api/Order/GetOrderList",
  GetOrder: BASE_URL + "/api/Order/GetOrder/{orderId}",
};

export default URLS;
