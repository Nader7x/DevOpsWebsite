import "./add_project.css"
import {Header} from "../../Shared/Header/header";
import React, {useRef, useState} from "react";
import {getAuthToken} from "../../Services/auth";
import {useNavigate} from "react-router-dom";
import axios from "axios";

export const AddProject = () =>{
    const { token, user } = getAuthToken();
    const navigate = useNavigate();

    const [project, setProject] = useState({
        loading: true,
        result: {},
        err: null,
    });

    const form = useRef({
        name: "",
        description: "",
    });

    const handleSubmit = (e) => {
        e.preventDefault();
        setProject({ ...project, loading: true });

        axios.post(`https://localhost:7157/Opsphere/project`, {
            name: form.current.name.value,
            description: form.current.description.value,
        }, {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        })
            .then(() => {
                setProject({ ...project, loading: false });
                navigate("/ProjectView");
            })
            .catch((errors) => {
                setProject({ ...project, loading: false, err: [{ msg: `something went wrong` }] });
            });
    };
    return (
        <>
            <Header />
            <div className="project-container">
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label htmlFor="nameInput" className="form-label">Name</label>
                        <input type="text" className="form-control" id="nameInput" ref={(val) => {
                            form.current.name = val;
                        }} />
                    </div>
                    <div className="mb-3">
                        <label htmlFor="descriptionInput" className="form-label">Description</label>
                        <textarea className="form-control" id="description" rows="5" placeholder="Enter description"
                                  ref={(val) => {
                                      form.current.description = val;
                                  }}></textarea></div>
                    <button type="submit" className="btn btn-primary">Submit</button>
                </form>
            </div>
        </>
    );
};