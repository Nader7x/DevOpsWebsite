import React, {useEffect, useState} from 'react';
import "./card_view.css"
import {Card} from "./Card/card";
import {Header} from "../../Shared/Header/header";
import {getAuthToken} from "../../Services/auth";
import {useLocation, useNavigate, useParams} from "react-router-dom";
import axios from "axios";
export const CardView = () => {
    const navigate = useNavigate();
    const { id } = useParams();
    const { token, user } = getAuthToken();
    const [ascending, setAscending] = useState(true);
    const [username, setUsername] = useState('');
    const location = useLocation();


    const [specificCard, setSpecificCard] = useState({
        loading: true,
        result: {},
        err: null,
    });

    useEffect(() => {
        axios
            .get(`https://localhost:7157/Opsphere/card`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setSpecificCard({ ...specificCard, result: response.data, loading: false, err: null });
                console.log(response.data);
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
                    {specificCard.err.map((err, index) => {
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
    const sortCardsByCardId = () => {
        const sortedData = [...specificCard.result];
        sortedData.sort((a, b) => {
            if (ascending) {
                return a.cardId - b.cardId;
            } else {
                return b.cardId - a.cardId;
            }
        });
        setAscending(!ascending);
        setSpecificCard({ ...specificCard, result: sortedData });
    };
    const refreshCardList = () => {
        // Function to refresh the card list after deletion
        axios
            .get(`https://localhost:7157/Opsphere/card`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setSpecificCard({
                    ...specificCard,
                    result: response.data,
                    loading: false,
                    err: null,
                });
            })
            .catch((errors) => {
                setSpecificCard({
                    ...specificCard,
                    result: [],
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
    };



    return (
        <>
            {specificCard.err !== null && error()}
            {specificCard.loading === true ? (
                loadingSpinner()
            ) : (
                <>
                    <Header currentPage={location.pathname}/>
                    <div className="button-container">
                        <button className="sidebar-btn">{username}</button>
                        <button className="sidebar-btn">{user.role}</button>
                        <button className="sidebar-btn" onClick={sortCardsByCardId}>Sort by ID</button>
                        <button className="sidebar-btn">Add Task</button>
                    </div>

                    <div className="card-container">
                        {specificCard.result.map((card, index) => (
                            <Card key={index} title={card.title} description={card.description} cardId={card.cardId} refreshCardList={refreshCardList}/>
                        ))}
                    </div>
                    <script src="https://cdn.jsdelivr.net/npm/mdb-ui-kit@3.2.0/js/mdb.min.js"></script>
                </>

            )}
        </>
    );

};

