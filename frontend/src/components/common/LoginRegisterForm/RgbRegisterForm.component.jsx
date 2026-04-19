import "./RgbInputForm.component.css";
import RgbButton from "../RgbButton.component.jsx";
import { useState } from "react";

const RgbRegisterForm = ({
  FIELDS = [],
  onSubmit = null,
  ref1,
  ref2,
  birthdayIsOpen,
}) => {
  const [birthday, setBirthday] = useState(null);

  const getBirthday = () => {
    if (birthdayIsOpen || birthday == null || birthday === "") {
      return birthday;
    } else {
      return birthday.split("-").reverse().join(".");
    }
  };

  return (
    <div className="credential-form">
      <div className="credential-box register-box-height">
        <form onSubmit={onSubmit}>
          <h2>Register</h2>
          {FIELDS.map((field) => (
            <div className="inputBox">
              {field.label !== "Birthday" ? (
                <>
                  <input
                    type={field.type}
                    name={field.name}
                    required="required"
                  ></input>
                  <span>{field.label}</span>
                </>
              ) : (
                <>
                  <input
                    ref={ref1}
                    type={birthdayIsOpen ? field.type : "text"}
                    name={field.name}
                    value={getBirthday()}
                    onInput={(e) => setBirthday(e.target.value)}
                    required="required"
                  ></input>
                  <span ref={ref2}>{field.label}</span>
                </>
              )}
              <i></i>
            </div>
          ))}
          <div>
            <RgbButton type="submit" text="Register now"></RgbButton>
          </div>
        </form>
      </div>
    </div>
  );
};

export default RgbRegisterForm;
