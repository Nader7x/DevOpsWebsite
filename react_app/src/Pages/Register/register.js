import {
    MDBBtn,
    MDBContainer,
    MDBRow,
    MDBCol,
    MDBCard,
    MDBCardBody,
    MDBCardImage,
    MDBInput,
    MDBIcon,
    MDBCheckbox
}
    from 'mdb-react-ui-kit';
import "./register.css"
import { useNavigate } from "react-router-dom";
import { useEffect, useRef, useState } from "react";
import axios from "axios";

export const RegisterPage = () => {
    const navigate = useNavigate();

    const [register, setRegister] = useState({
        loading: false,
        result: {},
        err: null,
    });

    const form = useRef({
        username: "",
        email: "",
        password: "",
        confirm_password: "",

    });
    const submit = (e) => {
        e.preventDefault();
        if (form.current.password.value !== form.current.confirm_password.value) {
            alert("Password and confirm password do not match");
            return;
        }
        setRegister({...register, loading: true});
        axios
            .post("https://localhost:7157/Opsphere/User/Register", {
                username: form.current.username.value,
                email: form.current.email.value,
                password: form.current.password.value,
            })
            .then(() => {
                setRegister({...register, loading: false});
                navigate("/");
            })
            .catch((errors) => {
                setRegister({...register, loading: false, err: [{msg: `something went wrong`}]});
            });
    };
    useEffect(() => {
        setRegister({...register, loading: false});

    }, []);
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
                    {register.err.map((err, index) => {
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
            {register.err !== null && error()}
            {register.loading === true ? (
                loadingSpinner()
            ) : (
                <div className="body2">
                    <div className="login-container">
                        <form className="login-form" onSubmit={(e) => submit(e)}>
                            <h2>Welcome Newbie</h2>
                            <p>Please enter your credentials to create an account.</p>
                            <div className="input-group">
                                <label htmlFor="username">Username:</label>
                                <input type="text" id="username" name="username" ref={(val) => {
                                    form.current.username = val;
                                }}/>
                            </div>
                            <div className="input-group">
                                <label htmlFor="email">Enter your email:</label> {/* Changed "Email" to "email" */}
                                <input type="text" id="email" name="email" required ref={(val) => {
                                    form.current.email = val;
                                }}/>
                            </div>
                            <div className="input-group">
                                <label htmlFor="password">Password:</label>
                                <input type="password" id="password" name="password" required ref={(val) => {
                                    form.current.password = val;
                                }}/>
                            </div>
                            <div className="input-group">
                                <label htmlFor="confirm-password">Repeat your
                                    Password:</label> {/* Changed "Re-password" to "confirm-password" */}
                                <input type="password" id="confirm-password" name="confirm-password" required
                                       ref={(val) => {
                                           form.current.confirm_password = val;
                                       }}/>
                            </div>
                            <div className="input-group">
                                <input type="checkbox" id="remember-me" name="remember-me"/>
                                <label htmlFor="remember-me">Remember me</label>
                            </div>
                            <button type="submit">Register</button>
                            <div className="extra-links">
                                <a onClick={() => navigate("/")}>Already have an account?</a>
                                <span>|</span>
                                <a href="#"></a>
                            </div>
                        </form>

                    </div>
                </div>
            )}
        </>
    );
}