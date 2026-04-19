import "./AddComputer.component.css";
import { useNavigate } from "react-router-dom";
import CheckError from "../common/CheckError";
import { useContext } from "react";
import { TokenContext } from "../../context/TokenContext";
import { useApiClient } from "../../context/ApiClientContext";

const AddComputer = () => {
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

  const { setToken } = useContext(TokenContext);

  const navigate = useNavigate();
  const { apiClient } = useApiClient();

  const HandleForm = async (e) => {
    e.preventDefault();

    const formData = new FormData(e.target);
    const body = Object.fromEntries(formData);

    try {
      const response = await apiClient.addComputer(body);

      if (response.ok) {
        navigate("/availableComputers");
      }
    } catch (error) {
      CheckError(error, setToken);
    }
  };

  return (
    <div class="container">
      <div class="form-container">
        <form onSubmit={HandleForm}>
          <h2>Add Computer</h2>
          {FIELDS.map((field, id) => (
            <div class="inputBox" key={id}>
              <span className="fontcolor-white">{field.label}</span>
              <input type={field.type} name={field.name} />
              <i></i>
            </div>
          ))}
          <div>
            <button class="button-add" type="submit">
              Add now
            </button>
          </div>
        </form>
      </div>
    </div>
  );
};

export default AddComputer;
