import React, {useEffect, useRef, useState} from 'react';
import "./task details.css"
import {Header} from "../../Shared/Header/header";
import {useParams} from "react-router-dom";
import {getAuthToken} from "../../Services/auth";
import axios from "axios";
import {CommentsAndReplies} from "./Comments/comments";

export const TaskDetails = () => {
    const { id } = useParams();
    const { token, user } = getAuthToken();
    const [specificCard, setSpecificCard] = useState({
        loading: true,
        result: [],
        err: null,
    });

    const [developers, setDevelopers] = useState({
        loading: true,
        result: [],
        err: null,
    });

    const [comment, setComment] = useState({
        loading: true,
        result: [],
        err: null,
    });

    const [comments, setComments] = useState({
        loading: true,
        result: [],
        err: null,
    });

    const commentForm = useRef({
        commentContent:""
    });

    const [selectedDeveloperId, setSelectedDeveloperId] = useState(null);

    const [status, setStatus] = useState(null);


    const [selectedFile, setSelectedFile] = useState(null);

    const handleFileUpload = (event) => {
        setSelectedFile(event.target.files[0]);
    };


    useEffect(() => {
        axios
            .get(`https://localhost:7157/Opsphere/card/Card/${id}`, {
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


                const projectId = response.data.projectId;
                axios
                    .get(`https://localhost:7157/Opsphere/project/ProjectDevelopers/${projectId}`, {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    })
                    .then((response) => {
                        setDevelopers({
                            ...developers,
                            result: response.data.map(item => item.user),
                            loading: false,
                            err: null,
                        });
                    })
                    .catch((errors) => {
                        setDevelopers({
                            ...developers,
                            result: [],
                            loading: false,
                            err: [{ msg: `something went wrong` }],
                        });
                    });
                axios
                    .get(`https://localhost:7157/Opsphere/CardComments/CardComments/${id}`, {
                        headers: {
                            Authorization: `Bearer ${token}`,
                        },
                    })
                    .then((response) => {
                        setComments({
                            ...comments,
                            result: response.data,
                            loading: false,
                            err: null,
                        });
                    })
                    .catch((errors) => {
                        setComments({
                            ...comments,
                            result: [],
                            loading: false,
                            err: [{ msg: `something went wrong` }],
                        });
                    });
            })
            .catch((errors) => {
                setComments({
                    ...comments,
                    result: {
                        commentContent: "",
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

    const refreshComments = () => {
        axios
            .get(`https://localhost:7157/Opsphere/CardComments/CardComments/${id}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then((response) => {
                setComments({
                    ...comments,
                    result: response.data,
                    loading: false,
                    err: null,
                });
            })
            .catch((errors) => {
                setComments({
                    ...setComments(),
                    result: [],
                    loading: false,
                    err: [{ msg: `something went wrong` }],
                });
            });
    };

    const handleAssignDeveloper = () => {
        if (!selectedDeveloperId) {
            return;
        }
        axios.patch(
            `https://localhost:7157/Opsphere/card/${specificCard.result.cardId}`,
            [
                {
                    "path": "/AssignedDeveloperId",
                    "op": "replace",
                    "value": selectedDeveloperId,
                }
            ],
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        )
            .then(response => {
            })
            .catch(error => {
                console.error('Error assigning developer:', error);
                alert('Error assigning developer. Please try again.');
            });
    }

    const handleAddComment = (e) => {
        e.preventDefault();
        setComment({ ...comment, loading: true });

        axios.post(`https://localhost:7157/Opsphere/CardComments/${specificCard.result.cardId}`, {
            commentContent: commentForm.current.commentContent.value,
        }, {
            headers: {
                Authorization: `Bearer ${token}`,
            }
        })
            .then(() => {
                commentForm.current.commentContent.value = "";
                setComment({ ...comment, loading: false });
                refreshComments();
            })
            .catch((errors) => {
                setComment({ ...comment, loading: false, err: [{ msg: `something went wrong` }] });
            });
    };

    const handleStatusChange = () => {
        if (!status) {
            return;
        }
        axios.patch(
            `https://localhost:7157/Opsphere/card/${specificCard.result.cardId}`,
            [
                {
                    "path": "/Status",
                    "op": "replace",
                    "value": status,
                }
            ],
            {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            }
        )
            .then(response => {
            })
            .catch(error => {
                console.error('Error changing status:', error);
                alert('Error assigning developer. Please try again.');
            });
    }
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

    const assignedDeveloperName = (developerId) => {
        if (!developerId) return "";

        const assignedDeveloper = developers.result.find(developer => developer.id === developerId);

        if (assignedDeveloper) {
            return assignedDeveloper.username;
        } else {
            return "Developer not found";
        }
    };

    const uploadAttachment = () => {
        if (!selectedFile) {
            return;
        }

        const formData = new FormData();
        formData.append('File', selectedFile);

        axios.post(`https://localhost:7157/Opsphere/Attachment/${specificCard.result.cardId}`, formData, {
            headers: {
                'Content-Type': 'multipart/form-data',
                Authorization: `Bearer ${token}`,
            },
        })
            .then((response) => {
            })
            .catch((error) => {
                console.error('Error uploading attachment:', error);
                alert('Error Uploading attachment.');
            });
    };

    const handleDownloadAttachment = () => {
        axios
            .get(`https://localhost:7157/Opsphere/Attachment/CardAttachment/${specificCard.result.cardId}`, {
                responseType: 'blob',
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })
            .then(response => {
                const contentType = response.headers['content-type'];

                let extension = 'unknown';
                if (contentType === 'application/pdf') {
                    extension = 'pdf';
                } else if (contentType === 'application/vnd.openxmlformats-officedocument.wordprocessingml.document') {
                    extension = 'docx';
                } else if (contentType === 'image/jpeg') {
                    extension = 'jpg';
                }

                const timestamp = new Date().getTime();
                const filename = `attachment_${timestamp}.${extension}`;

                const url = window.URL.createObjectURL(new Blob([response.data]));
                const link = document.createElement('a');
                link.href = url;
                link.setAttribute('download', filename);
                document.body.appendChild(link);
                link.click();
                link.parentNode.removeChild(link);
            })
            .catch(error => {
                console.error('Error downloading attachment:', error);
                alert('Error downloading attachment. Please try again.');
            });
    };



    return (
        <>
            {specificCard.err !== null && error()}
            {specificCard.loading === true ? (
                loadingSpinner()
            ) : (
                <>
                    <Header pagename={"Card Details"}/>
                    <div className="button-container">
                        <button className="sidebar-btn">{user.given_name}</button>
                        <button className="sidebar-btn">{user.role}</button>
                    </div>
                    <div className="card-container2">
                        <div className="cardbk-container">
                            <div id="test1" onClick={() => window.location.hash = 'back'}></div>
                            <div className="cardtitle-container">
                                <div className="title2">{specificCard.result.title}</div>
                            </div>
                            <div className="cardDev-container">
                                <div className="assignedDevelopers">

                                    <div className="description">
                                        Assigned
                                        developer: {assignedDeveloperName(specificCard.result.assignedDeveloperId)}
                                    </div>

                                    <div className="addDeveloper">
                                        <label htmlFor="dev-names">Add a developer:</label>
                                        <select name="dev-names" id="developers"
                                                onChange={(e) => setSelectedDeveloperId(e.target.value)}>
                                            <option>Select developer</option>
                                            {Array.isArray(developers.result) && developers.result.map(developer => (
                                                <option key={developer.id}
                                                        value={developer.id}>{developer.username}</option>
                                            ))}
                                        </select>
                                        <button className="btn btn-primary" type="submit"
                                                onClick={handleAssignDeveloper}
                                                style={{backgroundColor: 'rgb(81, 143, 143)'}}>Add
                                        </button>
                                    </div>

                                </div>

                                <div className="cardStatus-container2">
                                    <div className="updateStatus">
                                        <label htmlFor="status">Status:</label>
                                        <select name="status-opt" id="current-status"
                                                onChange={(e) => setStatus(e.target.value)}>
                                            <option value="ToDo">to do</option>
                                            <option value="Done">done</option>
                                            <option value="InProgress">in progress</option>
                                        </select>
                                        <button className="btn btn-primary" type="submit"
                                                onClick={handleStatusChange}
                                                style={{backgroundColor: 'rgb(81, 143, 143)'}}>Add
                                        </button>

                                    </div>
                                </div>


                            </div>
                            <div className="addAttachment">
                                <label htmlFor="attachment">Add Attachment:</label>
                                <input type="file" id="attachment" onChange={handleFileUpload}/>
                                <button className="btn btn-primary" type="submit"
                                        onClick={uploadAttachment}
                                        style={{backgroundColor: 'rgb(81, 143, 143)'}}>Upload
                                </button>
                                <button className="btn btn-primary"
                                        onClick={handleDownloadAttachment}>Download
                                    Attachment
                                </button>
                            </div>
                            <div className="cardDescription-container">
                                <div className="description">
                                    <br/>Description: {specificCard.result.description} <br/>
                                </div>
                            </div>
                            <div className="cardComment-container">
                                <div className="addComment">
                                    <form className="d-flex">
                                        <input className="form-control me-2" type="addcomment"
                                               placeholder="Type comment" aria-label="comment" ref={(val) => {
                                            commentForm.current.commentContent = val;
                                        }}/>
                                        <button className="btn btn-primary" type="submit" onClick={handleAddComment}
                                                style={{backgroundColor: 'rgb(81, 143, 143)'}}>submit
                                        </button>
                                    </form>
                                </div>

                                {comments.result.length > 0 && comments.result.map((comment, index) => (
                                    <CommentsAndReplies
                                        key={index}
                                        userName={comment.userName}
                                        commentContent={comment.commentContent}
                                        commentId={comment.id}
                                        cardId={id}
                                        replies={comment.replies}
                                        refreshComments={refreshComments}
                                    />
                                ))}

                            </div>

                        </div>
                    </div>
                </>
            )}
        </>
    );
};