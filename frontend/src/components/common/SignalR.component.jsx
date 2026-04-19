import * as signalR from '@microsoft/signalr'
import { BASE_URL } from '../../api/Api';

export const startSignalRConnection = (onReceiveOrder) => {
        const connection = new signalR.HubConnectionBuilder()
        .configureLogging(signalR.LogLevel.Debug)
        .withUrl(`${BASE_URL}/orderHub`)
        .withAutomaticReconnect()
        .build();

    connection.on("NewOrderReceived", async (orderId, status) => {
      console.log(`SignalR order received: ${orderId}, status: ${status}`);
      await onReceiveOrder(orderId, status);
    });

    connection.start();

    return connection;
}

export const stopSignalRConnection = async (connection) => {
  if (connection && connection.state === signalR.HubConnectionState.Connected) {
    try {
      await connection.stop();
      console.log("SignalR connection stopped.");
    } catch (err) {
      console.error("Error stopping SignalR connection:", err);
    }
  }
};