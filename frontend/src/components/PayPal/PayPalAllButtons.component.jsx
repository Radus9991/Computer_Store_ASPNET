import { useContext } from "react";
import "./PayPal.component.css";
import {
  PayPalScriptProvider,
  PayPalButtons,
  usePayPalScriptReducer,
} from "@paypal/react-paypal-js";
import { TokenContext } from "../../context/TokenContext";

// This value is from the props in the UI
const style = { layout: "vertical" };

// Custom component to wrap the PayPalButtons and show loading spinner
const ButtonWrapper = ({ showSpinner, createOrder, onApprove }) => {
  const [{ isPending }] = usePayPalScriptReducer();

  return (
    <>
      {showSpinner && isPending && <div className="loader" />}
      <PayPalButtons
        currency={"EUR"}
        style={style}
        disabled={false}
        forceReRender={[style]}
        fundingSource={undefined}
        createOrder={createOrder}
        onApprove={onApprove}
      />
    </>
  );
};

const PayPalAllButtons = ({ createOrder, onApprove }) => {
  const { token } = useContext(TokenContext);

  return (
    <div style={{ maxWidth: "750px", minHeight: "200px" }}>
      <PayPalScriptProvider
        options={{
          clientId:
            "AetwBlqWovGmCP08VOtDrNNWueQo_YzHyuI43L6R0ETZoItv5vh3w2IqSrWafxiKyHRD4gYVwsuwpDCL",
          components: "buttons",
          currency: "EUR",
        }}
      >
        <ButtonWrapper
          showSpinner={false}
          createOrder={createOrder}
          onApprove={onApprove}
        />
      </PayPalScriptProvider>
    </div>
  );
};

export default PayPalAllButtons;
