import "../AddProject/add_project.css"
import {Header} from "../../Shared/Header/header";
import React, {useEffect, useRef, useState} from "react";
import axios from "axios";
import {useNavigate, useParams} from "react-router-dom";
import {getAuthToken} from "../../Services/auth";

export const UpdateProject = () => {
    const { id } = useParams();
    const { token, user } = getAuthToken();
    const navigate = useNavigate();


    const [specificProject, setSpecificProject] = useState({
        loading: true,
        result: {},
        err: null,
    });

    useEffect(() => {
        axios
            .get(`https://localhost:7157/Opsphere/project`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setSpecificProject({ ...specificProject, result: response.data, loading: false, err: null });
                console.log(response.data);
            })
            .catch((errors) => {
                setSpecificProject({
                    ...specificProject,
                    result: {
                        id: "",
                        name: "",
                        description: "",
                    },
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
    }, []);
    const handleSubmit = (e) => {
        e.preventDefault();

    };
    const form = useRef({
        name: "",
        description: "",
    });
    return (
        <>

            <Header />
            <div className="project-container">
                <form onSubmit={handleSubmit}>
                    <div className="mb-3">
                        <label htmlFor="nameInput" className="form-label">name</label>
                        <input type="text" className="form-control" id="nameInput" placeholder={id} ref={(val) => {
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
    )
}