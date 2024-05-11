import {useNavigate, useParams} from "react-router-dom";
import {getAuthToken} from "../../Services/auth";
import "./update_task.css"
import {Header} from "../../Shared/Header/header";
import {useEffect, useRef, useState} from "react";
import axios from "axios";

export const UpdateTask = () => {
    const { id } = useParams();
    const { token, user } = getAuthToken();
    const navigate = useNavigate();

    const [task, setTask] = useState({
        loading: true,
        result: [],
        err: null,
    });

    const [specificCard, setSpecificCard] = useState({
        loading: true,
        result: {},
        err: null,
    });

    const [specificProject, setSpecificProject] = useState({
        loading: true,
        result: [],
        err: null,
    });;

    const form = useRef({
        title: "",
        description: "",
        comment: "",
        project: "",
    });

    useEffect(() => {
        axios
            .get(`https://localhost:7157/Opsphere/card/Card/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setSpecificCard({ ...specificCard, result: response.data, loading: false, err: null });
            })
            .catch((errors) => {
                setSpecificCard({
                    ...specificCard,
                    result: {
                        cardId: "",
                        title: "",
                        description: "",
                        status: "",
                        comment: "",
                        projectId: null,
                        assignedDeveloperId: null,

                    },
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
    }, [specificCard.result.projectId]);

    const handleSubmit = (e) => {
        e.preventDefault();
        setTask({ ...task, loading: true });


        axios.put(`https://localhost:7157/Opsphere/card/${id}`, {
            title: form.current.title.value,
            description: form.current.description.value,
        }, {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        })
            .then(() => {
                setTask({ ...task, loading: false });
                navigate(`/CardView/${specificCard.result.projectId}`);
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
            {specificCard.err !== null && error()}
            {specificCard.loading === true ? (
                loadingSpinner()
            ) : (
                <body>
                <Header pagename={"Update Task"}/>
                <div className="button-container">
                    <button className="sidebar-btn">{user.given_name}</button>
                    <button className="sidebar-btn">{user.role}</button>
                </div>
                <div className="card-container">
                    <div className="form-container">
                        <form className="task-form" onSubmit={handleSubmit}>
                            <div className="mb-3">
                                <label htmlFor="taskTitle" className="form-label">Task Title</label>
                                <input type="text" className="form-control" id="taskTitle" defaultValue= {specificCard.result.title} ref={(val) => {
                                    form.current.title = val;
                                }}/>
                            </div>
                            <div className="mb-3">
                                <label htmlFor="description" className="form-label">Description</label>
                                <textarea className="form-control" id="description" rows="5" defaultValue={specificCard.result.description} ref={(val) => {
                                    form.current.description = val;
                                }}></textarea>
                            </div>

                            <button type="submit" className="btn btn-primary">Update Task</button>
                            <button type="button" className="btn btn-secondary" onClick={() => navigate(`/CardView/${specificCard.result.projectId}`)}>Cancel</button>
                        </form>
                    </div>
                </div>
                </body>
            )}
        </>
    );
}
