import axios from "axios";

const sendRequest = async (url, type, data = null, token = null) => {
    const result = { ok: false, data: null, response: null, error: null }
    try {
        const headers = {
            headers: {
                Authorization: "Bearer " + token
            }
        }
        let response = null;
        if (type === "GET") {
            response = await axios.get(url, headers);
        } else if (type === "POST") {
            response = await axios.post(url, data, headers)
        } else {
            console.log("Invalid request type")
            throw new Error("Invalid request type")
        }

        if (response.status === 200 || response.status === 201 || response.status === 204) {
            result.ok = true
            result.data = response.data
        }
        result.error = { response: response }
    } catch (error) {
        result.error = error
    }
    return result
}

export const get = async (url, token = null) => {
    return await sendRequest(url, "GET", null, token)
}

export const post = async (url, data, token = null) => {
    return await sendRequest(url, "POST", data, token)
}
