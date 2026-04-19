import { useNavigate } from "react-router-dom";
import RgbRegisterForm from "../common/LoginRegisterForm/RgbRegisterForm.component";
import toast from "react-hot-toast";
import { useEffect, useRef, useState } from "react";
import CheckError from "../common/CheckError";
import { useApiClient } from "../../context/ApiClientContext";

function useOutsideClick(ref, setter) {
  useEffect(() => {
    function handleClickOutside(event) {
      if (ref.current && !ref.current.contains(event.target)) {
        setter(false);
        console.log("outside");
      } else {
        setter(true);
        console.log("inside");
      }
    }
    // Bind the event listener
    document.addEventListener("mousedown", handleClickOutside);
    return () => {
      // Unbind the event listener on clean up
      document.removeEventListener("mousedown", handleClickOutside);
    };
  }, [ref]);
}

const Register = () => {
  const FIELDS = [
    { id: "email", name: "email", label: "Email", type: "text" },
    { id: "name", name: "name", label: "Name", type: "text" },
    { id: "surname", name: "surname", label: "Surname", type: "text" },
    { id: "password", name: "password", label: "Password", type: "password" },
    {
      id: "repeatPassword",
      name: "repeatPassword",
      label: "Repeat password",
      type: "password",
    },
    { id: "birthday", name: "birthday", label: "Birthday", type: "date" },
  ];

  const [isClicked1, setIsClicked1] = useState(false);
  const [isClicked2, setIsClicked2] = useState(false);

  const [birthdayIsOpen, setBirthdayIsOpen] = useState(false);
  const navigate = useNavigate();
  const ref1 = useRef(null);
  const ref2 = useRef(null);

  useOutsideClick(ref1, setIsClicked1);
  useOutsideClick(ref2, setIsClicked2);

  const { apiClient } = useApiClient();

  const HandleForm = async (e) => {
    e.preventDefault();

    const formData = new FormData(e.target);
    const body = Object.fromEntries(formData);

    const { ok, error } = await apiClient.register(body);
    if (ok) {
      toast.success("Account created");
      navigate("/login");
    } else {
      CheckError(error, null);
    }
  };

  useEffect(() => {
    setBirthdayIsOpen(isClicked1 || isClicked2);
  }, [isClicked1, isClicked2]);

  return (
    <RgbRegisterForm
      FIELDS={FIELDS}
      onSubmit={HandleForm}
      ref1={ref1}
      ref2={ref2}
      birthdayIsOpen={birthdayIsOpen}
    ></RgbRegisterForm>
  );
};

export default Register;
