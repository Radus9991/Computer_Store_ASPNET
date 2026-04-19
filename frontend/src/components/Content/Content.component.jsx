import { Routes, Route } from "react-router-dom";
import Login from "../Login/Login.component";
import Register from "../Register/Register.component";
import NotFound from "../NotFound/NotFound.component";
import AddComputer from "../AddComputer/AddComputer.component";
import AvailableComputers from "../AvailableComputers/AvailableComputers.component";
import BasketList from "../BasketList/BasketList.component";
import Orders from "../Orders/Orders.component";
import Logout from "../Logout/Logout.component";

const Content = () => {
  return (
    <div>
      <Routes>
        <Route path="/addComputer" element={<AddComputer />} />
        <Route path="/availableComputers" element={<AvailableComputers />} />
        <Route path="/basket" element={<BasketList />} />
        <Route path="/login" element={<Login />} />
        <Route path="/register" element={<Register />} />
        <Route path="/orders" element={<Orders />} />
        <Route path="/logout" element={<Logout />}></Route>
        <Route path="*" element={<NotFound />} />
      </Routes>
    </div>
  );
};

export default Content;
