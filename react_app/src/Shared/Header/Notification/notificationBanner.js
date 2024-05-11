import React from "react";
import axios from "axios";
import {getAuthToken} from "../../../Services/auth";
import "./notificationBanner.css"

export const NotificationBanner = (props) => {
    const { token, user } = getAuthToken();
    const handleAccept = () => {
        axios.patch(
            `https://localhost:7157/Opsphere/User/AcceptInvite/${user.nameid}/Notification/${props.id}`,
            null, // No request body
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        )
            .then(response => {
                props.refreshNotifications();
            })
            .catch(error => {
                console.error('Error accepting the request:', error);
                alert('Error accepting the request. Please try again.');
            });
    };
    const handleReject = () => {
        axios.delete(
            `https://localhost:7157/Opsphere/User/Reject/${props.id}`,
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        )
            .then(response => {
                props.refreshNotifications();
            })
            .catch(error => {
                console.error('Error rejecting the request:', error);
                alert('Error rejecting the request. Please try again.');
            });
    }
    return (
        <div className="notification">
            <h6>Project Invite</h6>
            <h20>{props.content}</h20><br/>
            <input type="checkbox" id="accept1" name="accept1" onClick={handleAccept}/>
            <label htmlFor="accept1">Accept</label>
            <input type="checkbox" id="reject1" name="reject1" onClick={handleReject}/>
            <label htmlFor="reject1">Reject</label>
        </div>
    )
}