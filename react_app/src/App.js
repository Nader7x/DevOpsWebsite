import React from 'react';

import {Outlet} from "react-router-dom";
import Notification from "./Services/notification";

const App = () => {
    return(

        <>
            <Notification/>
            <Outlet/>

        </>
    );
}

export default App;