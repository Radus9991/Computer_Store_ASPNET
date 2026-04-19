import { useContext, useEffect, useState } from "react"
import { useNavigate } from "react-router-dom"
import { TokenContext } from "../../context/TokenContext"

const Logout = () => {
    const { setToken } = useContext(TokenContext)

    const navigate = useNavigate()

    useEffect(() => {
        setToken(null)
        navigate("/login")
    })
}

export default Logout