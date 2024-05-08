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

export const RegisterPage = () =>{
    const navigate = useNavigate();

    const [register, setRegister] = useState({
        loading: true,
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
        setRegister({ ...register, loading: true });
        axios
            .post("https://localhost:7157/Opsphere/User/Register", {
                username: form.current.username.value,
                email: form.current.email.value,
                password: form.current.password.value,
            })
            .then(() => {
                alert("success");
                setRegister({ ...register, loading: false });
                navigate("");
            })
            .catch((errors) => {
                setRegister({ ...register, loading: false, err: [{ msg: `something went wrong` }] });
            });
    };
    useEffect(() => {
        setRegister({ ...register, loading: false });

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

    return(
        <>
            {register.err !== null && error()}
            {register.loading === true ? (
                loadingSpinner()
            ) : (
                <MDBContainer fluid>

                    <MDBCard className='text-black m-5' style={{borderRadius: '25px'}}>
                        <MDBCardBody>
                            <MDBRow>
                                <MDBCol md='10' lg='6' className='order-2 order-lg-1 d-flex flex-column align-items-center'>
                                    <form onSubmit={(e) => submit(e)}> {/* Add onSubmit attribute here */}
                                        <p className="text-center h1 fw-bold mb-5 mx-1 mx-md-4 mt-4">Sign up</p>

                                        <div className="d-flex flex-row align-items-center mb-4 ">
                                            <MDBIcon fas icon="user me-3" size='lg'/>
                                            <MDBInput label='Your Name' id='form1' type='text' className='w-100' ref={(val) => {
                                                form.current.username = val;
                                            }}/>
                                        </div>

                                        <div className="d-flex flex-row align-items-center mb-4">
                                            <MDBIcon fas icon="envelope me-3" size='lg'/>
                                            <MDBInput label='Your Email' id='form2' type='email' ref={(val) => {
                                                form.current.email = val;
                                            }}/>
                                        </div>

                                        <div className="d-flex flex-row align-items-center mb-4">
                                            <MDBIcon fas icon="lock me-3" size='lg'/>
                                            <MDBInput label='Password' id='form3' type='password' ref={(val) => {
                                                form.current.password = val;
                                            }}/>
                                        </div>

                                        <div className="d-flex flex-row align-items-center mb-4">
                                            <MDBIcon fas icon="key me-3" size='lg'/>
                                            <MDBInput label='Repeat your password' id='form4' type='password' ref={(val) => {
                                                form.current.confirm_password = val;
                                            }}/>
                                        </div>

                                        <div className='mb-4'>
                                            <MDBCheckbox name='flexCheck' value='' id='flexCheckDefault' label='Subscribe to our newsletter' />
                                        </div>

                                        <MDBBtn className='mb-4' size='lg' type={"submit"}>Register</MDBBtn>
                                    </form>
                                </MDBCol>

                                <MDBCol md='10' lg='6' className='order-1 order-lg-2 d-flex align-items-center'>
                                    <MDBCardImage src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-registration/draw1.webp' fluid/>
                                </MDBCol>

                            </MDBRow>
                        </MDBCardBody>
                    </MDBCard>

                </MDBContainer>
                )}
        </>

    );
}