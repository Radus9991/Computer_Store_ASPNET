import axios from "axios";
import URLS, { BASE_URL } from "./Api";

class ApiClient {
  constructor(token) {
    this.client = axios.create({
      baseURL: BASE_URL,
      timeout: 1000,
      headers: { Authorization: `Bearer ${token}` },
    });
  }

  async register(body) {
    return await this.post(URLS.Register, body);
  }

  async login(body) {
    return await this.post(URLS.Login, body);
  }

  async getUserOrders(pageIndex) {
    return await this.get(URLS.UserOrders.replace("{pageIndex}", pageIndex));
  }

  async getUserOrdersAllIds() {
    return await this.get(URLS.GetUserOrdersAllIds);
  }

  async getOrderComputers(orderId, pageIndex) {
    return await this.get(
      URLS.OrderComputers.replace("{pageIndex}", pageIndex).replace(
        "{orderId}",
        orderId
      )
    );
  }

  async getOrder(orderId) {
    return await this.get(URLS.GetOrder.replace("{orderId}", orderId));
  }

  async approvePayment(orderId) {
    return await this.post(URLS.OnApprove, { orderID: orderId });
  }

  async addComputer(body) {
    return await this.post(URLS.AddPC, body);
  }

  async getSelectedComputers(basket) {
    return await this.post(URLS.SelectedComputers, basket);
  }

  async getAvailableComputers(pageIndex) {
    return await this.get(
      URLS.AvailableComputers.replace("{pageIndex}", pageIndex)
    );
  }

  async chargeUserSaldo(body) {
    return await this.post(URLS.ChargeUserSaldo, body);
  }

  async getUserRole() {
    return await this.get(URLS.GetUserRole);
  }

  async sendRequest(url, type, data = null) {
    const result = { ok: false, data: null, response: null, error: null };
    try {
      let response = null;
      if (type === "GET") {
        response = await this.client.get(url);
      } else if (type === "POST") {
        response = await this.client.post(url, data);
      } else {
        console.log("Invalid request type");
        throw new Error("Invalid request type");
      }

      if (
        response.status === 200 ||
        response.status === 201 ||
        response.status === 204
      ) {
        result.ok = true;
        result.data = response.data;
      }
      result.error = { response: response };
    } catch (error) {
      result.error = error;
    }
    return result;
  }

  async get(url) {
    return await this.sendRequest(url, "GET", null);
  }

  async post(url, data) {
    return await this.sendRequest(url, "POST", data);
  }
}

export default ApiClient;
