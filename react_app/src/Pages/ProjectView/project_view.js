import React, {useEffect, useState} from 'react';
import "./project_view.css"
import {Header} from "../../Shared/Header/header";
import {Project} from "./Project/project";
import {useLocation, useNavigate} from "react-router-dom";
import {getAuthToken} from "../../Services/auth";
import axios from "axios";
export const ProjectView = () => {

    const navigate = useNavigate();
    const { token, user } = getAuthToken();
    const [username, setUsername] = useState('');
    const location = useLocation();

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
        axios
            .get(`https://localhost:7157/Opsphere/User/GetCurrentUserName`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setUsername(response.data);
            })
            .catch((error) => {
                console.error('Error fetching username:', error);
                setUsername('Unknown');
            });
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
                    {specificProject.err.map((err, index) => {
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

    const refreshProjectList = () => {
        // Function to refresh the card list after deletion
        axios
            .get(`https://localhost:7157/Opsphere/project`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setSpecificProject({
                    ...specificProject,
                    result: response.data,
                    loading: false,
                    err: null,
                });
            })
            .catch((errors) => {
                setSpecificProject({
                    ...specificProject,
                    result: [],
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
    };
    return (
        <>
            {specificProject.err !== null && error()}
            {specificProject.loading === true ? (
                loadingSpinner()
            ) : (
                <body>
                <Header currentPage={location.pathname}/>
                <div className="project-container">
                    {specificProject.result.map((project, index) => (
                        <Project key={index} title={project.name} description={project.description} id={project.id} refreshProjectList={refreshProjectList}/>
                    ))}

                    {/* Add the rest of the project cards here */}
                </div>
                </body>
            )}
        </>
    );
};


