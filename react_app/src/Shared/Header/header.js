import React from 'react';
import "../../Pages/CardView/card_view.css"
import {useNavigate} from "react-router-dom";

export const Header = ({ currentPage }) =>{
    const navigate = useNavigate();

    const handleAddTask = () => {
        if (currentPage === "/CardView") {
            navigate('/AddTask');
        } else if (currentPage === "/ProjectView") {
            navigate('/AddProject');
        }
    };

    return(
        <>
            <header>
                <div className="header-container">
                    <h1 className="header-title">Card View</h1>
                    <div className="header-buttons">
                        <button className="header-btn">Back</button>
                        <button className="header-btn">Home</button>
                        <button className="header-btn" onClick={handleAddTask}>Add Task</button>

                    </div>
                </div>
                <form className="d-flex">
                    <input className="form-control me-2" type="search" placeholder="Type query" aria-label="Search"/>
                    <button className="btn btn-primary" type="submit">Search</button>
                </form>
            </header>
        </>
    )
}