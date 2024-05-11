import React, {useEffect, useState} from 'react';
import "./header.css"
import {useNavigate} from "react-router-dom";
import {getAuthToken, removeAuthToken} from "../../Services/auth";
import axios from "axios";
import {NotificationBanner} from "./Notification/notificationBanner";

export const Header = (props) =>{
    const navigate = useNavigate();
    const { token, user } = getAuthToken();
    const handleAddTask = () => {
        if (props.currentPage === "/CardView") {
            navigate('/AddTask');
        } else if (props.currentPage === "/ProjectView") {
            navigate('/AddProject');
        }
        else{
            navigate('/AddTask');
        }
    };

    const [notifications, setNotifications] = useState({
        loading: true,
        result: [],
        err: null,
    });

    useEffect(() => {
        axios
            .get(`https://localhost:7157/Opsphere/User/UserNotifications/${user.nameid}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setNotifications({ ...notifications, result: response.data, loading: false, err: null });

            })
            .catch((errors) => {
                setNotifications({
                    ...notifications,
                    result: {
                        id: "",
                        type: "",
                        content: "",
                        userId: "",
                        isRead: false,
                    },
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
    }, []);

    const handleLogOut = () => {
        removeAuthToken();
        navigate("/login");
    }

    const refreshNotifications = () => {
        axios
            .get(`https://localhost:7157/Opsphere/User/UserNotifications/${user.nameid}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setNotifications({ ...notifications, result: response.data, loading: false, err: null });

            })
            .catch((errors) => {
                setNotifications({
                    ...notifications,
                    result: {
                        id: "",
                        type: "",
                        content: "",
                        userId: "",
                        isRead: false,
                    },
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
    }
    return(
        <>
            <header>
                    <h1 className="header-title">{props.pagename}</h1>

                    <div className="header-buttons-div">
                        <button className="header-buttons" onClick={() => navigate(-1)}>Back</button>
                        <button className="header-buttons" onClick={handleLogOut}>Log out</button>
                        <button className="header-buttons" onClick={handleAddTask}>
                            {props.type === "project" ? "Add Project" : "Add Task"}
                        </button>
                    </div>

                <div className="dropdown" id="notificationDropdown">
                    <button className="button">
                        <svg className="bell" viewBox="0 0 448 512">
                            <path
                                d="M224 0c-17.7 0-32 14.3-32 32V49.9C119.5 61.4 64 124.2 64 200v33.4c0 45.4-15.5 89.5-43.8 124.9L5.3 377c-5.8 7.2-6.9 17.1-2.9 25.4S14.8 416 24 416H424c9.2 0 17.6-5.3 21.6-13.6s2.9-18.2-2.9-25.4l-14.9-18.6C399.5 322.9 384 278.8 384 233.4V200c0-75.8-55.5-138.6-128-150.1V32c0-17.7-14.3-32-32-32zm0 96h8c57.4 0 104 46.6 104 104v33.4c0 47.9 13.9 94.6 39.7 134.6H72.3C98.1 328 112 281.3 112 233.4V200c0-57.4 46.6-104 104-104h8zm64 352H224 160c0 17 6.7 33.3 18.7 45.3s28.3 18.7 45.3 18.7s33.3-6.7 45.3-18.7z"
                            ></path>
                        </svg>
                        Notifications
                        <div className="arrow">›</div>
                    </button>
                    <div className="dropdown-content">
                        {
                            notifications.result
                            .filter(notification => notification.type === "ProjectInvite")
                            .map((notification, index) => (
                                <NotificationBanner
                                    key={index}
                                    type={notification.type}
                                    content={notification.content}
                                    id={notification.id}
                                    userId={notification.userId}
                                    refreshNotifications={refreshNotifications}
                                />
                            ))}



                    </div>
                </div>
            </header>
        </>
    )
}