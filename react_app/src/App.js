import React, {useEffect} from 'react';
import {LoginPage} from "./Pages/Login/login";
import {RegisterPage} from "./Pages/Register/register";
import {Card} from "./Pages/CardView/Card/card";
import {CardView} from "./Pages/CardView/card_view";
import {Header} from "./Shared/Header/header";
import {Outlet} from "react-router-dom";
import Notification from "./Pages/Notification";

const App = () => {
    return(
        // <LoginPage/>
        // <RegisterPage/>
        // <CardView/>
        // <Header/>
        <div>
            <Notification/>
            <Outlet/>
        </div>
    );
}

export default App;