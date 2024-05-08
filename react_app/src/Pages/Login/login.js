import {
    MDBBtn,
    MDBContainer,
    MDBCard,
    MDBCardBody,
    MDBCardImage,
    MDBRow,
    MDBCol,
    MDBIcon,
    MDBInput
}
    from 'mdb-react-ui-kit';
import jwt from 'jwt-decode';
import {useNavigate} from "react-router-dom";
import {useRef, useState} from "react";
import axios from "axios";
import {setAuthToken} from "../../Services/auth";


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
        setLogin({ ...login, loading: true });
        axios
            .post("https://localhost:7157/Opsphere/User/login", {
                username: form.current.username.value,
                password: form.current.password.value,
            })
            .then((data) => {
                setLogin({ ...login, loading: false });
                const user = jwt(data.data.token);
                setAuthToken(data.data.token);
                console.log(user.role);
                navigate("/ProjectView");
                // if (user.role === "Admin") {
                //     alert("admin")
                //     navigate("/CardView");
                // } else if (user.role === "Developer") {
                //     navigate("/professor-home");
                //}
            })
            .catch((errors) => {
                setLogin({ ...login, loading: false, err: [{ msg: `something went wrong` }] });
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
    return(
        <>
            {login.err !== null && error()}
            {login.loading === true ? (
                loadingSpinner()
            ) : (
                <MDBContainer className="my-5">

                    <MDBCard>
                        <MDBRow className='g-0'>

                            <MDBCol md='6'>
                                <MDBCardImage src='https://mdbcdn.b-cdn.net/img/Photos/new-templates/bootstrap-login-form/img1.webp' alt="login form" className='rounded-start w-100'/>
                            </MDBCol>

                            <MDBCol md='6'>
                                <form onSubmit={(e) => submit(e)}> {/* Add onSubmit attribute here */}

                                    <MDBCardBody className='d-flex flex-column'>

                                    <div className='d-flex flex-row mt-2'>
                                        <MDBIcon fas icon="cubes fa-3x me-3" style={{ color: '#ff6219' }}/>
                                        <span className="h1 fw-bold mb-0">Logo</span>
                                    </div>

                                    <h5 className="fw-normal my-4 pb-3" style={{letterSpacing: '1px'}}>Sign into your account</h5>

                                    <MDBInput wrapperClass='mb-4' label='Username' id='formControlLg' type='text' size="lg" ref={(val) => {
                                        form.current.username = val;
                                    }}/>
                                    <MDBInput wrapperClass='mb-4' label='Password' id='formControlLg' type='password' size="lg" ref={(val) => {
                                        form.current.password = val;
                                    }}/>

                                    <MDBBtn className="mb-4 px-5" color='dark' size='lg'>Login</MDBBtn>
                                    <a className="small text-muted" href="#!">Forgot password?</a>
                                    <p className="mb-5 pb-lg-2" style={{color: '#393f81'}}>Don't have an account? <a href="#!" style={{color: '#393f81'}}>Register here</a></p>

                                    <div className='d-flex flex-row justify-content-start'>
                                        <a href="#!" className="small text-muted me-1">Terms of use.</a>
                                        <a href="#!" className="small text-muted">Privacy policy</a>
                                    </div>

                                    </MDBCardBody>
                                </form>
                            </MDBCol>

                        </MDBRow>
                    </MDBCard>

                </MDBContainer>
                )}
        </>

    )
}