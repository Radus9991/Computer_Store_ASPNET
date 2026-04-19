import { useNavigate } from 'react-router-dom'
import toast from 'react-hot-toast'
import RgbLoginForm from "../common/LoginRegisterForm/RgbLoginForm.component"
import CheckError from "../common/CheckError";
import { useToken } from '../../context/TokenContext'
import { useApiClient } from '../../context/ApiClientContext';

const Login = () => {
    const FIELDS = [
        { id: "email", name: "email", label: "Email", type: "text" },
        { id: "password", name: "password", label: "Password", type: "password" },
    ]

    const navigate = useNavigate()
    const { apiClient } = useApiClient()
    const { setToken } = useToken()

    const HandleForm = async (e) => {
        e.preventDefault()
        const body = Object.fromEntries(new FormData(e.target))

        const {data, ok, error} = await apiClient.login(body)
        if (ok) {
            toast.success("You signed in!")
            setToken(data.token) //TODO: Poprawic setToken
            navigate("/availableComputers")
        } else {
            CheckError(error, setToken)
        }
    }

    return (
        <RgbLoginForm FIELDS={FIELDS} onSubmit={HandleForm}></RgbLoginForm>

    )
}

export default Login