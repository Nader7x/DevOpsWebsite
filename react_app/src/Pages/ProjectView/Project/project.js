import React, { useEffect, useState } from "react";
import { getAuthToken } from "../../../Services/auth";
import axios from "axios";
import { useNavigate } from "react-router-dom";
import "./project.css"

export const Project = (props) => {
    const { token, user } = getAuthToken();
    const navigate = useNavigate();
    function toggleDetails(event) {
        event.currentTarget.classList.toggle('active');
    }
    function stopPropagation(event) {
        event.stopPropagation();
    }

    const [developers, setDevelopers] = useState({
        loading: true,
        result: [],
        err: null,
    });
    const [assignedDevelopers, setAssignedDevelopers] = useState({
        loading: true,
        result: [],
        err: null,
    });


    const [selectedDeveloperId, setSelectedDeveloperId] = useState(null);

    useEffect(() => {
        axios
            .get(`https://localhost:7157/Developers`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setDevelopers({
                    ...developers,
                    result: response.data,
                    loading: false,
                    err: null,
                });
            })
            .catch((error) => {
                setDevelopers({
                    ...developers,
                    result: [],
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
        axios
            .get(`https://localhost:7157/Opsphere/project/ProjectDevelopers/${props.id}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setAssignedDevelopers({
                    ...assignedDevelopers,
                    result: response.data.map(item => item.user),
                    loading: false,
                    err: null,
                });
            })
            .catch((error) => {
                setAssignedDevelopers({
                    ...assignedDevelopers,
                    result: [],
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });

    }, []);

    const handleDelete = () => {
        axios.delete(`https://localhost:7157/Opsphere/project/${props.id}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        })
            .then(response => {
                props.refreshProjectList();
            })
            .catch(error => {
                console.error('Error deleting project:', error);
                alert('Error deleting project. Please try again.');
            });
    };


    const confirmDeveloperSelection = () => {
        if (!selectedDeveloperId) {
            return;
        }
        axios.post(
            `https://localhost:7157/Opsphere/project/${props.id}/developer/${selectedDeveloperId}`,
            null,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        )
            .then(response => {
            })
            .catch(error => {
                console.error('Error assigning developer:', error);
                alert('Error assigning developer. Please try again.');
            });
    };

    return (
        <div className="project" onClick={toggleDetails}>
            <div className="title">{props.title}</div>
            <div className="details">
                {props.description}<br/>
                <button className="button-project update"
                        onClick={() => navigate(`/UpdateProject/${props.id}`)}>Update
                </button>
                <button className="button-project delete" onClick={handleDelete}>Delete</button>
                <button className="button-project open-cards" onClick={() => navigate(`/CardView/${props.id}`)}>Open Cards</button>

                <div className="project-info">
                    <div className="cardStatus-container">
                        <div className="dropdownopt" onClick={stopPropagation}><label htmlFor="status">Name:</label>
                            <select name="dropdown-opt" id="operation"
                                    onChange={(e) => setSelectedDeveloperId(e.target.value)}>
                                <option>Select developer</option>
                                {Array.isArray(developers.result) && developers.result.map(developer => (
                                    <option key={developer.id} value={developer.id}>{developer.username}</option>
                                ))}
                            </select>
                            <button className="btn btn-primary" type="submit" onClick={confirmDeveloperSelection}
                                    style={{backgroundColor: 'rgb(81, 143, 143)'}}>Add
                            </button>
                            <div className="developers">
                                Developers: {assignedDevelopers.result.map(developer => developer.username).join(', ')}
                            </div>
                        </div>
                    </div>


                </div>
            </div>
        </div>
    );
};
