import React, { useEffect, useState } from 'react';
import "./card_view.css";
import { Card } from "./Card/card";
import { Header } from "../../Shared/Header/header";
import { getAuthToken } from "../../Services/auth";
import { useLocation, useParams } from "react-router-dom";
import axios from "axios";

export const CardView = () => {
    const { id } = useParams();
    const { token, user } = getAuthToken();
    const location = useLocation();

    const [specificCard, setSpecificCard] = useState({
        loading: true,
        result: [],
        err: null,
    });

    const [searchQuery, setSearchQuery] = useState('');

    useEffect(() => {
        axios
            .get(`https://localhost:7157/Opsphere/card/Cards/${id}`, {
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
    }, []);

    const handleSearch = (e) => {
        setSearchQuery(e.target.value);
    };

    return (
        <>
            <Header pagename={"Card View"} currentPage={location.pathname}/>
            <body>
            <div className="button-container">
                <button className="sidebar-btn">{user.given_name}</button>
                <button className="sidebar-btn">{user.role}</button>
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
                            <Card key={index} title={card.title} description={card.description} cardId={card.cardId}/>
                        ))}

                </div>

            </div>


            </body>
        </>
    );
};
