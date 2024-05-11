import React, {useEffect, useState} from 'react';
import "./project_view.css"
import {Header} from "../../Shared/Header/header";
import {Project} from "./Project/project";
import {useLocation} from "react-router-dom";
import {getAuthToken} from "../../Services/auth";
import axios from "axios";
export const ProjectView = () => {

    const { token, user } = getAuthToken();
    const location = useLocation();

    const [specificProject, setSpecificProject] = useState({
        loading: true,
        result: [],
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
                <>
                    <Header pagename={"Project View Page"} currentPage={location.pathname} type={"project"}/>
                    <body>
                    <div className="project-container">
                        {specificProject.result.map((project, index) => (
                            <Project key={index} title={project.name} description={project.description} id={project.id} refreshProjectList={refreshProjectList}/>
                        ))}

                    </div>
                    </body>
                </>

            )}
        </>
    );
};


