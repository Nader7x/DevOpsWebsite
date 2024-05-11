import React, {useEffect, useRef, useState} from 'react';
import "../CardView/card_view.css"
import {Card} from "../CardView/Card/card";
import {Header} from "../../Shared/Header/header";
import {getAuthToken} from "../../Services/auth";
import {useLocation} from "react-router-dom";
import axios from "axios";
export const CardViewDev = () => {
    const { token, user } = getAuthToken();
    const [ascending, setAscending] = useState(true);
    const location = useLocation();


    const [specificCard, setSpecificCard] = useState({
        loading: true,
        result: [],
        err: null,
    });

    const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
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
    const handleSearch = (e) => {
        setSearchQuery(e.target.value);
    };


    return (
        <>
            {specificCard.err !== null && error()}
            {specificCard.loading === true ? (
                loadingSpinner()
            ) : (
                <>
                    <Header pagename={"Card View Page"} currentPage={location.pathname}/>
                    <body>
                    <div className="button-container">
                        <button className="sidebar-btn">{user.given_name}</button>
                        <button className="sidebar-btn">{user.role}</button>
                        <button className="sidebar-btn" onClick={sortCardsByCardId}>Sort by ID</button>
                    </div>
                    <div className={"cardcontainerout"}>
                        <form className="search-bar2">
                            <input
                                className="form-control me-2"
                                type="search"
                                placeholder="Type developer ID or name"
                                aria-label="Search"
                                onChange={handleSearch}
                            />
                        </form>
                            <div className="card-container">
                                {specificCard.result
                                    .filter((card) => {
                                        return searchQuery === '' ||
                                            (card.assignedDeveloperId !== null && card.assignedDeveloperId.toString().includes(searchQuery)) ||
                                            (card.projectDeveloper && card.projectDeveloper.user && card.projectDeveloper.user.username && card.projectDeveloper.user.username.includes(searchQuery));
                                    })
                                    .map((card, index) => (
                                        <Card key={index} title={card.title} description={card.description}
                                              cardId={card.cardId}/>
                                    ))}
                            </div>
                    </div>
                            <form className="d-flex">
                                <input
                                    className="form-control me-2"
                                    type="search"
                                    placeholder="Type developer ID"
                                    aria-label="Search"
                                    onChange={handleSearch}
                                />

                            </form>
                            <script src="https://cdn.jsdelivr.net/npm/mdb-ui-kit@3.2.0/js/mdb.min.js"></script>
                    </body>

                </>

                )}
        </>
);

};

