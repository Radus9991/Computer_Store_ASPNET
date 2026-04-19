import toast from "react-hot-toast";

const CheckError = (error, setToken) => {
  console.log(error);
  try {
    if (error.response && error.response.status === 400) {
      const errors = error.response.data.errors;
      Object.keys(errors).forEach((key) => {
        toast.error(errors[key][0]);
      });
    } else if (error.response.status === 401) {
      setToken(null);
      toast.error("Session expired");
    } else {
      toast.error("Error!");
    }
  } catch (err) {
    console.log(err);
  }
};

export default CheckError;
