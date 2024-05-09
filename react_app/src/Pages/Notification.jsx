import React, { useEffect } from 'react';
import { ToastContainer, toast } from 'react-toastify';
import 'react-toastify/dist/ReactToastify.css';
import * as signalR from '@microsoft/signalr';
import {getAuthToken} from "../Services/auth";
import * as jwt from "jwt-decode"
import { json } from 'react-router-dom';

const user = getAuthToken().user;
const Notification = () => {
    useEffect(() => {
        const connection = new signalR.HubConnectionBuilder()
            .configureLogging(signalR.LogLevel.Debug)
            .withUrl("https://localhost:7157/Notify", {
                skipNegotiation: true,
                transport: signalR.HttpTransportType.WebSockets,
                accessTokenFactory : ()=> getAuthToken().token
            })
            .build();

        connection.start().then(() => {
            console.log('SignalR Connected');
        }).catch(err => console.error('SignalR Connection Error: ', err));

        connection.on("SendNotification", notification => {
            console.log(notification)
            console.log(user)
            if(notification.userId == user.nameid){
                toast(notification.content);
            }
        });
        return () => {
            connection.stop().then(r => console.log('SignalR Disconnected'));
        };
    }, []);

    return (
        <div>
            <ToastContainer position="top-right" autoClose={5000} hideProgressBar newestOnTop closeOnClick rtl={false} pauseOnFocusLoss draggable pauseOnHover />
        </div>
    );
};

export default Notification;
