import jwt from 'jwt-decode';
import {useNavigate} from "react-router-dom";
import {useRef, useState} from "react";
import axios from "axios";
import {setAuthToken} from "../../Services/auth";
import "./login.css"


export const LoginPage = () => {
    const navigate = useNavigate();

    const [login, setLogin] = useState({
        loading: false,
        err: null,
    });

    const form = useRef({
        username: "",
        password: "",
    });

    const submit = (e) => {
        e.preventDefault();
        setLogin({...login, loading: true});
        axios
            .post("https://localhost:7157/Opsphere/User/login", {
                username: form.current.username.value,
                password: form.current.password.value,
            })
            .then((data) => {
                setLogin({...login, loading: false});
                const user = jwt(data.data.token);
                setAuthToken(data.data.token);
                if (user.role === "Developer") {
                    navigate("/CardViewDev");
                } else if (user.role === "TeamLeader") {
                    navigate("/ProjectView");
                }
            })
            .catch((errors) => {
                setLogin({...login, loading: false, err: [{msg: `something went wrong`}]});
            });
    };

    const loadingSpinner = () => {
        return (
            <div className="container h-100">
                <div className="row h-100 justify-content-center align-items-center">
                    <div className="spinner-border" role="status">
                        <span className="sr-only">Loading...</span>
                    </div>
                </div>
            </div>
        );
    };

    const error = () => {
        return (
            <div className="container">
                <div className="row">
                    {login.err.map((err, index) => {
                        return (
                            <div key={index} className="col-sm-12 alert alert-danger" role="alert">
                                {err.msg}
                            </div>
                        );
                    })}
                </div>
            </div>
        );
    };
    return (
        <>
            {login.err !== null && error()}
            {login.loading === true ? (
                loadingSpinner()
            ) : (
                <>
                    <div className="body2">
                        <div className="login-container">
                            <form className="login-form" onSubmit={(e) => submit(e)}> {/* Add onSubmit attribute here */}
                                <h2>Welcome Back Old-timer!</h2>
                                <p>Please login to access your account.</p>
                                <div className="input-group">
                                    <label htmlFor="username">Username:</label>
                                    <input type="text" id="username" name="username" required
                                           ref={(val) => {
                                               form.current.username = val;
                                           }}/>
                                </div>
                                <div className="input-group">
                                    <label htmlFor="password">Password:</label>
                                    <input type="password" id="password" name="password" required ref={(val) => {
                                        form.current.password = val;
                                    }}/>
                                </div>
                                <div className="input-group"> {/* Changed class to className */}
                                    <input type="checkbox" id="remember-me" name="remember-me"/>
                                    <label htmlFor="remember-me">Remember me</label>
                                </div>
                                <button type="submit">Login</button>
                                <div className="extra-links">
                                    <a href="#">Forgot password?</a>
                                    <span>|</span>
                                    <a onClick={() => navigate("/register")}>Create an account</a>
                                </div>
                            </form>
                        </div>
                    </div>
                </>
            )}
        </>
    );
}