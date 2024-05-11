import React from 'react';
import "./card.css"
import axios from "axios";
import {getAuthToken} from "../../../Services/auth";
import {useNavigate} from "react-router-dom";
export const Card= (props) => {
    const { token, user } = getAuthToken();
    const navigate = useNavigate();

    const handleDelete = () => {
        axios.delete(`https://localhost:7157/Opsphere/card/${props.cardId}`, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
    })
            .then(response => {
                props.refreshCardList();
            })
            .catch(error => {
                console.error('Error deleting card:', error);
                alert('Error deleting card. Please try again.');
            });
    };

    return (

        <div className="card">
            <img src="https://mdbootstrap.com/img/new/standard/nature/184.webp" className="card-img-top" alt="..."/>
            <div className="card-body">
                <h5 className="card-title">{props.title}</h5>
                <p className="card-text">
                    {props.description}
                </p>
                <button className="btn btn-primary me-1"
                        onClick={() => navigate(`/UpdateTask/${props.cardId}`)}>Update
                </button>
                <button className="btn btn-secondary me-1" onClick={handleDelete}>Delete</button>
                <button className="btn btn-primary me-1"
                        onClick={() => navigate(`/TaskDetails/${props.cardId}`)}>Open Card
                </button>

            </div>
        </div>
    );
}