import App from './App';
import {LoginPage} from './Pages/Login/login';
import {RegisterPage} from "./Pages/Register/register";
import {AuthGuard} from "./Guards/auth-guard";
import {Navigate} from "react-router-dom";
import {CardView} from "./Pages/CardView/card_view";

import { createBrowserRouter } from "react-router-dom";
import {AddTask} from "./Pages/AddTask/add_task";
import {ProjectView} from "./Pages/ProjectView/project_view";
import {AddProject} from "./Pages/AddProject/add_project";
import {UpdateProject} from "./Pages/UpdateProject/update_project";
import {UpdateTask} from "./Pages/UpdateTask/update_task";

export const router = createBrowserRouter([
    {
        path: "", // localhost:3000
        element: <App />,
        children: [
            {
                // Guard
                // element: <AuthGuard roles={[]} />,
                children: [
                    {
                        path: "",
                        element: <LoginPage />,
                    },
                    {
                        path: "/register",
                        element: <RegisterPage />,
                    },
                ],
            },
            {
                path: "/CardView",
                element: <CardView/>,
            },
            {
              path: "/AddTask",
              element: <AddTask/>,
            },
            {
                path: "/AddProject",
                element: <AddProject/>,
            },
            {
              path: "/ProjectView",
              element: <ProjectView/>,
            },
            {
                path: "/UpdateProject/:id",
                element: <UpdateProject/>,
            },
            {
                path: "/UpdateTask/:id",
                element: <UpdateTask/>,
            },
            {
                path: "*",
                element: <Navigate to={"/"} />,
            },
        ]
    },
])