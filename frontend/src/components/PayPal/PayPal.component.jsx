import { PayPalButtons, usePayPalScriptReducer } from "@paypal/react-paypal-js";
import "./PayPal.component.css";

const PayPal = () => {
    const [{isPending}] = usePayPalScriptReducer()

    return (
        <div>
            {isPending ? <div className="loader"></div> : null}
            {!isPending && <PayPalButtons style={{ layout: "horizontal" }}/> }
        </div>
    );
}

export default PayPal;
 