import React from "react";
import {getAuthToken} from "../../../Services/auth";
import axios from "axios";
import {useNavigate} from "react-router-dom";
export const Project = (props) =>{
    const { token, user } = getAuthToken();
    const navigate = useNavigate();

    function toggleDetails(event) {
        event.currentTarget.classList.toggle('active');
    }

    const handleDelete = () => {
        axios.delete(`https://localhost:7157/Opsphere/project/${props.id}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        })
            .then(response => {
                console.log('Card deleted successfully');
                props.refreshProjectList(); // Refresh the card list in the parent component
            })
            .catch(error => {
                console.error('Error deleting card:', error);
                alert('Error deleting card. Please try again.');
            });
    };

    return (
        <div className="project" onClick={toggleDetails}>  {/*onClick={() => toggleDetails(this)}*/}
            <div className="title">{props.title}</div>
            <div className="details">
                {props.description}<br/>
                <button className="button update" onClick={() => navigate(`/UpdateProject/${props.id}`)}>Update</button>
                <button className="button delete" onClick={handleDelete}>Delete</button>
                <div className="project-info">
                    <button className="button open-cards" onClick={() => navigate("/CardView")}>Open Cards</button>
                    <div className="developers">
                        Developers: John Doe, Jane Smith
                    </div>
                </div>
            </div>
        </div>
    )
}