import "./RgbInputForm.component.css"
import RgbButton from "../RgbButton.component.jsx"

const RgbLoginForm = ({FIELDS = [], onSubmit = null}) => {
    return (
        <div className='credential-form'>
            <div className="credential-box login-box-height">
                <form onSubmit={onSubmit}>
                    <h2>Login</h2>
                    {FIELDS.map((field, id) => (
                        <div className="inputBox" key={id}>
                            <input type={field.type} name={field.name} required="required" />
                            <span>{field.label}</span>
                            <i></i>
                        </div>
                    ))}
                    <div>
                        <RgbButton type="submit" text="Login now"></RgbButton>
                    </div>
                </form>
            </div>
        </div>
    )
}

export default RgbLoginForm