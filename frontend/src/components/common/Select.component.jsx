import React from "react";
import "./Select.component.css";

const Select = ({ options, value, onChange, text }) => {
  return (
    <div className="select-container">
      <select id="select" value={value} onChange={onChange}>
        <option value="" disabled selected>
          {text}
        </option>
        {options.map((option) => (
          <option key={option.value} value={option.value}>
            {option.label}
          </option>
        ))}
      </select>
    </div>
  );
};

export default Select;
