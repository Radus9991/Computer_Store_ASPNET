import { Link } from "react-router-dom";
import "./Navbar.component.css";
import logoImg from "../Images/logo.jpg";
import { useContext, useEffect, useState } from "react";
import { IsAdmin, IsLoggedIn } from "../../statuses/UserStatus";
import { TokenContext } from "../../context/TokenContext";
import { BasketContext } from "../../context/BasketContext";
import { useApiClient } from "../../context/ApiClientContext";

const NOTLOGGEDURLS = [
    { to: "/register", name: "Register" },
    { to: "/login", name: "Login" },
    { to: "/availableComputers", name: "PCs" },
  ];

  const LOGGEDURLS = [
    { to: "/basket", name: "Basket" },
    { to: "/availableComputers", name: "PCs" },
    { to: "/orders", name: "Orders" },
  ];

  const ADMINURLS = [{ to: "/addComputer", name: "Add PC" }];
const Navbar = () => {
  

  const [isLoggedIn, setIsLoggedIn] = useState(false);
  const [isAdmin, setIsAdmin] = useState(false);
  const { token } = useContext(TokenContext);
  const { basket } = useContext(BasketContext);
  const { apiClient } = useApiClient();

  const getUserRole = async () => {
    const { ok, data } = await apiClient.getUserRole();
    console.log(ok);
    if (ok) {
      if (data.role === "Admin") {
        setIsAdmin(true);
      } else {
        setIsAdmin(false);
      }
    }
  };

  useEffect(() => {
    setIsLoggedIn(IsLoggedIn(token));
  }, [token]);

  useEffect(() => {
    if (apiClient != null) {
      getUserRole();
    }
  }, [apiClient]);

  const renderAdminLinks = () => {
    return ADMINURLS.map((url) => (
      <li key={url.name}>
        <Link to={url.to}>{url.name}</Link>
      </li>
    ));
  };

  const renderNotLoggedInLinks = () => {
    return NOTLOGGEDURLS.map((url) => (
      <li key={url.name}>
        <Link to={url.to}>{url.name}</Link>
      </li>
    ));
  };

  const renderLoggedInLinks = () => {
    return LOGGEDURLS.map((url) => (
      <li key={url.name}>
        <Link to={url.to}>
          {url.name === "Basket" ? (
            <>
              {url.name} <span className="badge">({basket.length})</span>
            </>
          ) : (
            url.name
          )}
        </Link>
      </li>
    ));
  };

  return (
    <nav className="navbar">
      <div className="logo">
        <img src={logoImg} alt="Logo" />
      </div>
      <ul>
        {!isLoggedIn ? (
          renderNotLoggedInLinks()
        ) : (
          <>
            <li>
              {" "}
              <Link to="/logout">Logout</Link>
            </li>
            {isAdmin && renderAdminLinks()}
            {renderLoggedInLinks()}
          </>
        )}
      </ul>
    </nav>
  );
};

export default Navbar;
