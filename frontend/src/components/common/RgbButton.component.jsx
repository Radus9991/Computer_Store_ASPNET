import "./RgbButton.component.css";

const RgbButton = ({ onClick = null, text = null, type = null }) => {
    return (
        <button className="button-rgb" type={type} onClick={onClick}>{text}</button>
    );
};

export default RgbButton;
