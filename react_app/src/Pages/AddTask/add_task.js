import {useNavigate} from "react-router-dom";
import {getAuthToken} from "../../Services/auth";
import "./add_task.css"
import {Header} from "../../Shared/Header/header";
import {useEffect, useRef, useState} from "react";
import axios from "axios";

export const AddTask = () => {
    const { token, user } = getAuthToken();
    const navigate = useNavigate();

    const [task, setTask] = useState({
        loading: true,
        result: {},
        err: null,
    });

    const [specificProject, setSpecificProject] = useState({
        loading: true,
        result: {},
        err: null,
    });

    const form = useRef({
        title: "",
        description: "",
        comment: "",
        project: "",
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
                        description: ""
                    },
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
    }, []);

    const handleSubmit = (e) => {
        e.preventDefault();
        setTask({ ...task, loading: true });

        const projectId = form.current.project;

        axios.post(`https://localhost:7157/Opsphere/card/${projectId}`, {
            title: form.current.title.value,
            description: form.current.description.value,
            comment: form.current.comment.value,
        }, {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        })
            .then(() => {
                setTask({ ...task, loading: false });
                navigate(`/CardView/${projectId}`);
            })
            .catch((errors) => {
                setTask({ ...task, loading: false, err: [{ msg: `something went wrong` }] });
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

    return (
        <>
            {specificProject.err !== null && error()}
            {specificProject.loading === true ? (
                loadingSpinner()
            ) : (
                <body>
                <Header pagename={"Add Card"}/>

                <div className="button-container">
                    <button className="sidebar-btn">{user.given_name}</button>
                    <button className="sidebar-btn">{user.role}</button>
                </div>
                <div className="card-container">
                    <div className="form-container">

                        <form className="task-form" onSubmit={handleSubmit}>
                            <div className="mb-3">
                                <label htmlFor="taskTitle" className="form-label">Task Title</label>
                                <input type="text" className="form-control" id="taskTitle" placeholder="Enter task title" ref={(val) => {
                                    form.current.title = val;
                                }}/>
                            </div>
                            <div className="mb-3">
                                <label htmlFor="description" className="form-label">Description</label>
                                <textarea className="form-control" id="description" rows="5" placeholder="Enter description" ref={(val) => {
                                    form.current.description = val;
                                }}></textarea>
                            </div>
                            <div className="mb-3">
                                <label htmlFor="comment" className="form-label">Comment</label>
                                <textarea className="form-control" id="comment" rows="3" placeholder="Enter comment" ref={(val) => {
                                    form.current.comment = val;
                                }}></textarea>
                            </div>
                            <div className="mb-3">
                                <label htmlFor="project" className="form-label">Project</label>
                                <select className="form-select" id="project" onChange={(e) => form.current.project = e.target.value}>
                                    <option>Select project</option>
                                    {Array.isArray(specificProject.result) && specificProject.result.map(project => (
                                        <option key={project.id} value={project.id}>{project.name}</option>
                                    ))}
                                </select>
                            </div>
                            <button type="submit" className="btn btn-primary">Add Task</button>
                            <button type="button" className="btn btn-secondary" onClick={() => navigate(-1)}>Cancel</button>
                        </form>
                    </div>
                </div>

                </body>
            )}
        </>
    );
}
