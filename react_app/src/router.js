import App from './App';
import { LoginPage } from './Pages/Login/login';
import { RegisterPage } from "./Pages/Register/register";
import { AuthGuard } from "./Guards/auth-guard";
import { Navigate } from "react-router-dom";
import { CardView } from "./Pages/CardView/card_view";
import { createBrowserRouter } from "react-router-dom";
import { AddTask } from "./Pages/AddTask/add_task";
import { ProjectView } from "./Pages/ProjectView/project_view";
import { AddProject } from "./Pages/AddProject/add_project";
import { UpdateProject } from "./Pages/UpdateProject/update_project";
import { UpdateTask } from "./Pages/UpdateTask/update_task";
import { TaskDetails } from "./Pages/TaskDetails/task_details";
import { CardViewDev } from "./Pages/CardViewDeveloper/card_view_dev";

export const router = createBrowserRouter([
    {
        path: "", // localhost:3000
        element: <App />,
        children: [
            {
                children: [
                    {
                        path: "/",
                        element: <LoginPage />,
                    },
                    {
                        path: "/register",
                        element: <RegisterPage />,
                    },
                ],
            },
            {
                // Guard for authenticated users (Developer or TeamLeader)
                element: <AuthGuard roles={["Developer", "TeamLeader"]} />,
                children: [
                    {
                        path: "/UpdateTask/:id", // Both Developer and TeamLeader
                        element: <UpdateTask />,
                    },
                    {
                        path: "/TaskDetails/:id", // Both Developer and TeamLeader
                        element: <TaskDetails />,
                    },
                    {
                        // Guard for TeamLeader role
                        element: <AuthGuard roles={["TeamLeader"]} />,
                        children: [
                            {
                                path: "/CardView/:id",
                                element: <CardView />,
                            },
                        ],
                    },
                ],
            },
            {
                // Guard for Developer role
                element: <AuthGuard roles={["Developer"]} />,
                children: [
                    {
                        path: "/CardViewDev", // Developer view
                        element: <CardViewDev />,
                    },
                ],
            },
            {
                element: <AuthGuard roles={["TeamLeader"]} />,
                children: [
                    {
                        path: "/ProjectView", // TeamLeader view
                        element: <ProjectView />,
                    },
                    {
                        path: "/AddProject", // TeamLeader only
                        element: <AddProject />,
                    },
                    {
                        path: "/UpdateProject/:id", // TeamLeader only
                        element: <UpdateProject />,
                    },
                    {
                        path: "/AddTask", // TeamLeader only
                        element: <AddTask />,
                    },
                ]
            },
            {
                path: "*", // Redirect to home for any other route
                element: <Navigate to={"/"} />,
            },
        ],
    },
]);
